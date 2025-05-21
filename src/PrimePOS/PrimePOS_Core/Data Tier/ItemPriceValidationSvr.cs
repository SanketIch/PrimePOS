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
using NLog;

namespace POS_Core.DataAccess
{
	

	// Provides data access methods for ItemPriceValidation
 
	public class ItemPriceValidationSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(ItemPriceValidationData updates, IDbTransaction tx) 
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
				logger.Fatal(ex, "Persist(ItemPriceValidationData updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}

    
		// Inserts, updates or deletes rows in a DataSet.

		public  void Persist(ItemPriceValidationData  updates) 
		{

			IDbTransaction tx;
			IDbConnection conn = DataFactory.CreateConnection(); 
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
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
				logger.Fatal(ex, "Persist(ItemPriceValidationData  updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}

		}
		#endregion

		#region Get Methods

		// Looks up a ItemPriceValidation based on its primary-key:System.String CompanionItemID

		public ItemPriceValidationData Populate(System.String ItemID,Int32 DeptID, IDbConnection conn) 
		{
			try 
			{
                string sSQL = "Select * FROM " + clsPOSDBConstants.ItemPriceValidation_tbl + " WHERE ";
                if (ItemID.Trim() != "")
                {
                    sSQL += clsPOSDBConstants.ItemPriceValidation_Fld_ItemID + " ='" + ItemID + "'";
                }
                else
                {
                    sSQL += clsPOSDBConstants.ItemPriceValidation_Fld_DeptID + " =" + DeptID + "";
                }

				ItemPriceValidationData ds = new ItemPriceValidationData();
				ds.ItemPriceValidation.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL ).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.String ItemID,Int32 DeptID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

        public ItemPriceValidationData PopulateByItem(System.String ItemID) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(ItemID,0, conn));
		}

        public ItemPriceValidationData PopulateByDept(System.Int32 DeptID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate("",DeptID, conn));
        }

		// Fills a ItemPriceValidationData with all ItemPriceValidation

		public ItemPriceValidationData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{
                string sSQL1 = "Select * FROM "
                                    + clsPOSDBConstants.ItemPriceValidation_tbl;
				
				//sWhereClause = sWhereClause.Replace("WHERE","AND");
				sWhereClause= String.Concat(" where " ,sWhereClause);

				string sSQL = String.Concat(sSQL1,sWhereClause);

				ItemPriceValidationData ds = new ItemPriceValidationData();
				ds.ItemPriceValidation.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

		// Fills a ItemPriceValidationData with all ItemPriceValidation

		public ItemPriceValidationData PopulateList(string whereClause) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
			return(PopulateList(whereClause,conn));
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			ItemPriceValidationTable addedTable = (ItemPriceValidationTable)ds.Tables[clsPOSDBConstants.ItemPriceValidation_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (ItemPriceValidationRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemPriceValidation_tbl,insParam);
						/*for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);

						}*/
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
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a ItemPriceValidation DataSet, within a given database transaction.

		public void Update(ItemPriceValidationData ds, IDbTransaction tx) 
		{	
			ItemPriceValidationTable modifiedTable = (ItemPriceValidationTable)ds.Tables[clsPOSDBConstants.ItemPriceValidation_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (ItemPriceValidationRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemPriceValidation_tbl,updParam);

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
						logger.Fatal(ex, "Update(ItemPriceValidationData ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a ItemPriceValidation DataSet, within a given database transaction.
		public void Delete(ItemPriceValidationData ds, IDbTransaction tx) 
		{
		
			ItemPriceValidationTable table = (ItemPriceValidationTable)ds.Tables[clsPOSDBConstants.ItemPriceValidation_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (ItemPriceValidationRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemPriceValidation_tbl,delParam);
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
						logger.Fatal(ex, "Delete(ItemPriceValidationData ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
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
		private IDbDataParameter[] PKParameters(System.Int32 ID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemPriceValidation_Fld_ID;
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = ID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemPriceValidationRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemPriceValidation_Fld_ID;
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.ID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_ID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemPriceValidationRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_DeptID, System.Data.SqlDbType.Int);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_ItemID, System.Data.SqlDbType.VarChar);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative, System.Data.DbType.Boolean);
			sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage, System.Data.SqlDbType.Decimal);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount, System.Data.SqlDbType.Decimal);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage, System.Data.SqlDbType.Decimal);

			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_DeptID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_ItemID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_IsActive;
			sqlParams[3].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage;

			if (row.DeptID != 0)
				sqlParams[0].Value = row.DeptID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.ItemID != System.String.Empty )
				sqlParams[1].Value = row.ItemID;
			else
				sqlParams[1].Value = DBNull.Value ;

            sqlParams[2].Value = row.IsActive;
            sqlParams[3].Value=row.AllowNegative;

			if (row.MinCostPercentage != System.Decimal.MinValue )
				sqlParams[4].Value = row.MinCostPercentage;
			else
				sqlParams[4].Value = DBNull.Value ;
			
			if (row.MinSellingAmount != System.Decimal.MinValue )
				sqlParams[5].Value = row.MinSellingAmount;
			else
				sqlParams[5].Value = DBNull.Value ;
            
            if (row.MinSellingPercentage != System.Decimal.MinValue)
                sqlParams[6].Value = row.MinSellingPercentage;
            else
                sqlParams[6].Value = DBNull.Value;

			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemPriceValidationRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_ID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_DeptID, System.Data.SqlDbType.Int);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_ItemID, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative, System.Data.DbType.Boolean);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage, System.Data.SqlDbType.Decimal);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount, System.Data.SqlDbType.Decimal);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage, System.Data.SqlDbType.Decimal);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_DeptID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_ItemID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_IsActive;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage;

            if (row.ID != 0)
                sqlParams[0].Value = row.ID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.DeptID != 0)
                sqlParams[1].Value = row.DeptID;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.ItemID != System.String.Empty)
                sqlParams[2].Value = row.ItemID;
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = row.IsActive;
            sqlParams[4].Value = row.AllowNegative;

            if (row.MinCostPercentage != System.Decimal.MinValue)
                sqlParams[5].Value = row.MinCostPercentage;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.MinSellingAmount != System.Decimal.MinValue)
                sqlParams[6].Value = row.MinSellingAmount;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.MinSellingPercentage != System.Decimal.MinValue)
                sqlParams[7].Value = row.MinSellingPercentage;
            else
                sqlParams[7].Value = DBNull.Value;

            return (sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
