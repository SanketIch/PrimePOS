// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
//using POS.Resources;
namespace POS_Core.DataAccess
{
	

	// Provides data access methods for TaxCode
 
	public class TaxCodesSvr: IDisposable  
	{

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(DataSet updates, SqlTransaction tx) 
		{
			try 
		
			{
				this.Delete(updates, tx);
				this.Insert(updates, tx);
				this.Update(updates, tx);

				updates.AcceptChanges();
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}

    
		// Inserts, updates or deletes rows in a DataSet.

		public  void Persist(DataSet updates) 
		{

			SqlTransaction tx;
			SqlConnection conn = new SqlConnection( POS_Core.Resources.Configuration.ConnectionString);

			conn.Open();
			tx = conn.BeginTransaction();
			try 
			{
				this.Persist(updates, tx);
				tx.Commit();
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				tx.Rollback();
				ErrorHandler.throwException(ex,"","");
			}

		}

        public bool DeleteRow(string CurrentID, ref bool bRxTax)
        {
            string sSQL = string.Empty;
			bRxTax = false;

			try
            {
				try
				{
					DataTable dtTax = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "select TaxCode from TaxCodes WHERE TaxID = '" + CurrentID + "'").Tables[0];
					if (dtTax != null && dtTax.Rows.Count > 0)
					{
						if (Configuration.convertNullToString(dtTax.Rows[0]["TaxCode"]).Trim().ToUpper() == clsPOSDBConstants.TaxCodes_Fld_RxTax.ToUpper())
                        {
							bRxTax = true;
							return false;
                        }
					}
				}
				catch (Exception ex)
                {
					ErrorHandler.throwException(ex, "", "");
					return false;
				}

				DataTable dtItemTax = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "SELECT TOP 1 TaxId FROM ItemTax WHERE TaxID = '" + CurrentID + "'").Tables[0];    //PRIMEPOS-2937 25-Jan-2021 JY Modified
				DataTable dtTrans = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "SELECT TOP 1 TaxID FROM POSTransactionDetailTax WHERE TaxID = '" + CurrentID + "'").Tables[0];    //PRIMEPOS-2937 25-Jan-2021 JY Added
				if (dtItemTax != null && dtItemTax.Rows.Count == 0 && dtTrans != null && dtTrans.Rows.Count == 0)
                {
                    sSQL = " delete from TaxCodes where TaxID = '" + CurrentID + "'";
                    DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                    //return false;  Commented By Shitaljit QuicSolv on 4 oct 2011
                    return true; //Added By Shitaljit QuicSolv on 4 oct 2011 to return true if successfully deleted to stop displaying error message even though it was deleted successfully.
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

		#endregion

		#region Get Methods

		// Looks up a TaxCode based on its primary-key:System.Int32 Taxcode

		public TaxCodesData Populate(System.Int32 TaxId, SqlConnection conn) 
		{
			try 
			{
				TaxCodesData ds = new TaxCodesData();
				ds.TaxCodes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.TaxCodes_Fld_TaxID +
                    " , " + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " , " +  clsPOSDBConstants.TaxCodes_Fld_Description + " , " +
                    clsPOSDBConstants.TaxCodes_Fld_Amount + " , " + clsPOSDBConstants.Users_Fld_UserID +" , " + clsPOSDBConstants.TaxCodes_Fld_TaxType + " , " + clsPOSDBConstants.TaxCodes_Fld_Active +//2974
					" FROM " + clsPOSDBConstants.TaxCodes_tbl + " WHERE " + 
                    clsPOSDBConstants.TaxCodes_Fld_TaxID + " =" + TaxId  , PKParameters(TaxId.ToString())).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public TaxCodesData Populate(System.Int32 TaxId) 
		{
			using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(TaxId, conn));
			}
		}

		public TaxCodesData Populate(System.String Taxcode, SqlConnection conn) 
		{
			try 
			{
				TaxCodesData ds = new TaxCodesData();
				ds.TaxCodes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.TaxCodes_Fld_TaxID + " , "
                    + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " , " +  clsPOSDBConstants.TaxCodes_Fld_Description + " , " 
                    +  clsPOSDBConstants.TaxCodes_Fld_Amount + " , " + clsPOSDBConstants.Users_Fld_UserID + " , " +  clsPOSDBConstants.TaxCodes_Fld_TaxType + " , " + clsPOSDBConstants.TaxCodes_Fld_Active + //2974
					" FROM " + clsPOSDBConstants.TaxCodes_tbl + " WHERE " + 
                    clsPOSDBConstants.TaxCodes_Fld_TaxCode + " ='" + Taxcode + "'" , PKParameters(Taxcode)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public TaxCodesData Populate(System.String Taxcode) 
		{
			using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(Taxcode, conn));
			}
		}

		// Fills a TaxCodesData with all TaxCode

		public TaxCodesData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = String.Concat("Select " + clsPOSDBConstants.TaxCodes_Fld_TaxID + " , " +
                    clsPOSDBConstants.TaxCodes_Fld_TaxCode + " , " + clsPOSDBConstants.TaxCodes_Fld_Description +
                    " , " + clsPOSDBConstants.TaxCodes_Fld_Amount + " , " + clsPOSDBConstants.Users_Fld_UserID + " , " + clsPOSDBConstants.TaxCodes_Fld_TaxType + " , " + clsPOSDBConstants.TaxCodes_Fld_Active +//2974

					" FROM " + clsPOSDBConstants.TaxCodes_tbl,sWhereClause);

				TaxCodesData ds = new TaxCodesData();
				ds.TaxCodes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch (Exception ex) 
			{    
				ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

		// Fills a TaxCodesData with all TaxCode

        public TaxCodesData PopulateTaxCodeUsedInTrans(string sItemID, int iTransID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateTaxCodeUsedInTrans(sItemID, iTransID, conn));
            }
        }

        public TaxCodesData PopulateTaxCodeUsedInTrans(string sItemID, int iTransID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select TC.* FROM " + clsPOSDBConstants.TaxCodes_tbl + " TC  INNER JOIN "+ clsPOSDBConstants.TransDetailTax_tbl + " TD "
                              + " ON TC."+clsPOSDBConstants.TransDetail_Fld_TaxID + " = TD."+ clsPOSDBConstants.TaxCodes_Fld_TaxID
                              + " AND "+clsPOSDBConstants.Item_Fld_ItemID +" = '"+ sItemID + "' AND "+ clsPOSDBConstants.TransDetail_Fld_TransID + " = "+ iTransID ;

                TaxCodesData ds = new TaxCodesData();
                ds.TaxCodes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }


		public TaxCodesData PopulateList(string whereClause) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection()) 
			{
				conn.ConnectionString=POS_Core.Resources.Configuration.ConnectionString;
				return(PopulateList(whereClause,conn));
			}			
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, SqlTransaction tx) 
		{

			TaxCodesTable addedTable = (TaxCodesTable)ds.Tables[clsPOSDBConstants.TaxCodes_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (TaxCodesRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.TaxCodes_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						

					} 
					catch(POSExceptions ex) 
					{
						throw(ex);
					}

					catch(OtherExceptions ex) 
					{
						throw(ex);
					}

					catch (Exception ex) 
					{
						ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a TaxCodes DataSet, within a given database transaction.

		public void Update(DataSet ds, SqlTransaction tx) 
		{	
			TaxCodesTable modifiedTable = (TaxCodesTable)ds.Tables[clsPOSDBConstants.TaxCodes_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (TaxCodesRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.TaxCodes_tbl,updParam);

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
					} 
					catch(POSExceptions ex) 
					{
						throw(ex);
					}

					catch(OtherExceptions ex) 
					{
						throw(ex);
					}

					catch (Exception ex) 
					{
						ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a TaxCodes DataSet, within a given database transaction.
		public void Delete(DataSet ds, SqlTransaction tx) 
		{
		
			TaxCodesTable table = (TaxCodesTable)ds.Tables[clsPOSDBConstants.TaxCodes_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (TaxCodesRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.TaxCodes_tbl,delParam);
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL,delParam );
					} 
					catch(POSExceptions ex) 
					{
						throw(ex);
					}

					catch(OtherExceptions ex) 
					{
						throw(ex);
					}

					catch (Exception ex) 
					{
						ErrorHandler.throwException(ex,"","");
					}
				} 
			}
		}

		private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
		{
			string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
			// build where clause
			for(int i = 0;i<delParam.Length;i++)
			{
				sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
			}
			return sDeleteSQL;
		}
		private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
		{
			string sInsertSQL = "INSERT INTO " + tableName + " ( ";
			// build where clause
			sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

			for(int i = 1;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn ;
			}
			sInsertSQL = sInsertSQL + " , UserId ";
			sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

			for(int i = 1;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName ;
			}
			sInsertSQL = 	sInsertSQL + " , '" + Configuration.UserName + "'";
			sInsertSQL = 	sInsertSQL + " )";
			return sInsertSQL;
		}


		private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
		{
			string sUpdateSQL = "UPDATE " + tableName + " SET ";
			// build where clause
			sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn +"  = " + updParam[1].ParameterName ;

			for(int i = 2;i<updParam.Length;i++)
			{
				sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn +"  = " + updParam[i].ParameterName ;
			}

			sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'" ;

			sUpdateSQL = 	sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
			return sUpdateSQL;
		}
		#endregion
		#region IDBDataParameter Generator Methods
		private IDbDataParameter[] whereParameters(string swhere) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
			sqlParams[0] = DataFactory.CreateParameter();
	  
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Size = 2000;
			sqlParams[0].ParameterName = "@whereClause";

			sqlParams[0].Value = swhere;
			return(sqlParams);
		}
		private IDbDataParameter[] PKParameters(System.String Taxcode) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@Taxcode";
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = Taxcode;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(TaxCodesRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@Taxcode";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.TaxCode;
			sqlParams[0].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_TaxCode;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(TaxCodesRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5);//2974

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TaxCodes_Fld_TaxCode, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TaxCodes_Fld_Description, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TaxCodes_Fld_Amount, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TaxCodes_Fld_TaxType, System.Data.DbType.Int32);

			sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TaxCodes_Fld_Active, System.Data.DbType.Boolean);//2974

			sqlParams[0].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_TaxCode;
			sqlParams[1].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_Description;
			sqlParams[2].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_Amount;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_TaxType;

			sqlParams[4].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_Active;//2974

			if (row.TaxCode != System.String.Empty )
				sqlParams[0].Value = row.TaxCode;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.Description != System.String.Empty )
				sqlParams[1].Value = row.Description;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.Amount != System.Int32.MinValue )
				sqlParams[2].Value = row.Amount;
			else
				sqlParams[2].Value = DBNull.Value ;

            sqlParams[3].Value = row.TaxType;

			sqlParams[4].Value = row.Active;//2974
			return (sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(TaxCodesRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);//2974

			sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TaxCodes_Fld_TaxID, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TaxCodes_Fld_TaxCode, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TaxCodes_Fld_Description, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TaxCodes_Fld_Amount, System.Data.DbType.Int64);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TaxCodes_Fld_TaxType, System.Data.DbType.Int32);

			sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TaxCodes_Fld_Active, System.Data.DbType.Boolean);//2974

			sqlParams[0].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_TaxID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_TaxCode;
			sqlParams[2].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_Description;
			sqlParams[3].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_Amount;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_TaxType;

			sqlParams[5].SourceColumn = clsPOSDBConstants.TaxCodes_Fld_Active;//2974

			if (row.TaxID != System.Int32.MinValue )
				sqlParams[0].Value = row.TaxID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.TaxCode != System.String.Empty )
				sqlParams[1].Value = row.TaxCode;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.Description != System.String.Empty )
				sqlParams[2].Value = row.Description;
			else
				sqlParams[2].Value = DBNull.Value ;

			if (row.Amount != System.Int32.MinValue )
				sqlParams[3].Value = row.Amount;
			else
				sqlParams[3].Value = DBNull.Value ;

            sqlParams[4].Value = row.TaxType;

			sqlParams[5].Value = row.Active;//2974
			return (sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
