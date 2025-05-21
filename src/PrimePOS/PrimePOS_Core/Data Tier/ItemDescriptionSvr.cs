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
	
	// Provides data access methods for ItemDescription
 
	public class ItemDescriptionSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.
		public  void Persist(DataSet updates, IDbTransaction tx) 
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
				logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}
    
		// Inserts, updates or deletes rows in a DataSet.
		public  void Persist(DataSet updates) 
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
				logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}

		}
		#endregion

		#region Get Methods

		// Looks up a ItemDescription based on its primary-key:System.String CompanionItemID

		public ItemDescriptionData Populate(System.Int32 Id, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select Detail." 
									+ clsPOSDBConstants.ItemDescription_Fld_ID 
									+ " , Detail." + clsPOSDBConstants.Item_Fld_ItemID
                                    + " , Detail." + clsPOSDBConstants.ItemDescription_Fld_Description + " As Description "
									+ " , Detail." + clsPOSDBConstants.ItemDescription_Fld_LanguageId
									+ " , Detail." + clsPOSDBConstants.fld_UserID
                                    +", Master." +clsPOSDBConstants.Language_Fld_Name + " As Language"
								+ " FROM " 
									+ clsPOSDBConstants.ItemDescription_tbl + " As Detail"
									+ " , "  +  clsPOSDBConstants.Language_tbl + " As Master "
								+ " WHERE  Detail."
                                    + clsPOSDBConstants.ItemDescription_Fld_ID + " ='" + Id + "'"
                                    + " AND Master." + clsPOSDBConstants.Language_Fld_ID + " = Detail." + clsPOSDBConstants.ItemDescription_Fld_LanguageId;

				ItemDescriptionData ds = new ItemDescriptionData();
				ds.ItemDescription.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL ).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 Id, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public ItemDescriptionData Populate(System.Int32 Id) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			return(Populate(Id, conn));
		}

		// Fills a ItemDescriptionData with all ItemDescription

		public ItemDescriptionData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{

                string sSQL1 = "Select Detail."
                                    + clsPOSDBConstants.ItemDescription_Fld_ID
                                    + " , Detail." + clsPOSDBConstants.Item_Fld_ItemID
                                    + " , Master." + clsPOSDBConstants.Language_Fld_Name + " As 'Language' "
                                    + " , " + clsPOSDBConstants.ItemMonitorCategory_Fld_Description 
                                    + " , Detail." + clsPOSDBConstants.ItemDescription_Fld_LanguageId
                                    + " , Detail." + clsPOSDBConstants.fld_UserID
                                + " FROM "
                                    + clsPOSDBConstants.ItemDescription_tbl + " As Detail"
                                    + " , " + clsPOSDBConstants.Language_tbl + " As Master "
                                + " WHERE "
                                    + "  Master." + clsPOSDBConstants.Language_Fld_ID + " = Detail." + clsPOSDBConstants.ItemDescription_Fld_LanguageId;
                
				sWhereClause= String.Concat(" and " ,sWhereClause);

				string sSQL = String.Concat(sSQL1,sWhereClause);

				ItemDescriptionData ds = new ItemDescriptionData();
				ds.ItemDescription.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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

		// Fills a ItemDescriptionData with all ItemDescription

		public ItemDescriptionData PopulateList(string whereClause) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
			return(PopulateList(whereClause,conn));
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			ItemDescriptionTable addedTable = (ItemDescriptionTable)ds.Tables[clsPOSDBConstants.ItemDescription_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (ItemDescriptionRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemDescription_tbl,insParam);
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
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a ItemDescription DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			ItemDescriptionTable modifiedTable = (ItemDescriptionTable)ds.Tables[clsPOSDBConstants.ItemDescription_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (ItemDescriptionRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemDescription_tbl,updParam);

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
						logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a ItemDescription DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			ItemDescriptionTable table = (ItemDescriptionTable)ds.Tables[clsPOSDBConstants.ItemDescription_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (ItemDescriptionRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemDescription_tbl,delParam);
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
						logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
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
            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
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
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemDescription_Fld_ID;
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = ID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemDescriptionRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemDescription_Fld_ID;
			sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemDescription_Fld_ID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemDescriptionRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ItemID, System.Data.SqlDbType.VarChar);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemDescription_Fld_Description, System.Data.SqlDbType.NVarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemDescription_Fld_LanguageId, System.Data.SqlDbType.Int);

            sqlParams[0].SourceColumn = clsPOSDBConstants.Item_Fld_ItemID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemDescription_Fld_Description;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemDescription_Fld_LanguageId;


            if (row.ItemID != System.String.Empty)
                sqlParams[0].Value = row.ItemID;
            else
                sqlParams[0].Value = DBNull.Value;

            sqlParams[1].Value = row.Description;
            sqlParams[2].Value = row.LanguageId;
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemDescriptionRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemDescription_Fld_ID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ItemID, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemDescription_Fld_Description, System.Data.SqlDbType.NVarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemDescription_Fld_LanguageId, System.Data.SqlDbType.Int);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemDescription_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Item_Fld_ItemID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemDescription_Fld_Description;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemDescription_Fld_LanguageId;

            sqlParams[0].Value = row.ID;

            if (row.ItemID != System.String.Empty)
                sqlParams[1].Value = row.ItemID;
            else
                sqlParams[1].Value = DBNull.Value;

            sqlParams[2].Value = row.Description;
            sqlParams[3].Value = row.LanguageId;
            return (sqlParams);
		}
		#endregion

        #region Sprint-21 - 1272 17-Aug-2015 JY Added
        public DataSet GetItemDescriptionInOL(string strItemIds, int nCustomerId)
        {
            try
            {
                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

                string strSQL = "SELECT DISTINCT b.ItemId, d.Description AS ItemDescriptionInOL FROM POSTransaction a " +
                                " INNER JOIN POSTransactionDetail b on a.TRansId = b.Transid " +
                                " LEFT JOIN Customer c on c.CustomerID = a.CustomerID " +
                                " LEFT JOIN itemdescription d on b.ItemId = d.ItemId AND c.LanguageId = d.LanguageId " +
                                " WHERE b.ItemID in (" + strItemIds + ") AND c.CustomerID = " + nCustomerId;

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
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
				logger.Fatal(ex, "GetItemDescriptionInOL(string strItemIds, int nCustomerId)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        public void Dispose() {}   
	}
}
