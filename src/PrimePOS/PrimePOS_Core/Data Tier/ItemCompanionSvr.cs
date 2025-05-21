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
	

	// Provides data access methods for ItemCompanion
 
	public class ItemCompanionSvr: IDisposable  
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

		// Looks up a ItemCompanion based on its primary-key:System.String CompanionItemID

		public ItemCompanionData Populate(System.String CompanionItemID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
									+ clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID 
									+ " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_ItemID 
									+ " , Item." + clsPOSDBConstants.Item_Fld_Description + " As ItemDescription "
									+ " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_Amount 
									+ " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_Percentage 
								+ " FROM " 
									+ clsPOSDBConstants.ItemCompanion_tbl + " As Companion "
									+ " , "  +  clsPOSDBConstants.Item_tbl + " As Item "
								+ " WHERE " 
									+ clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID + " ='" + CompanionItemID + "'" 
									+ " AND Companion." + clsPOSDBConstants.ItemCompanion_Fld_ItemID + " = Item." + clsPOSDBConstants.Item_Fld_ItemID;

				ItemCompanionData ds = new ItemCompanionData();
				ds.ItemCompanion.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , PKParameters(CompanionItemID)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.String CompanionItemID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public ItemCompanionData Populate(System.String CompanionItemID) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			return(Populate(CompanionItemID, conn));
		}

		// Fills a ItemCompanionData with all ItemCompanion

		public ItemCompanionData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
                //string sSQL1 = "Select " 
                //                    + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID 
                //                    + " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_ItemID 
                //                    + " , Item." + clsPOSDBConstants.Item_Fld_Description + " As ItemDescription "
                //                    + " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_Amount 
                //                    + " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_Percentage 
                //                + " FROM " 
                //                    + clsPOSDBConstants.ItemCompanion_tbl + " As Companion "
                //                    + " , "  +  clsPOSDBConstants.Item_tbl + " As Item "
                //                + " WHERE " 
                //                    + " Companion." + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID + " = Item." + clsPOSDBConstants.Item_Fld_ItemID;

                //Added By Dharmendra (SRT) on Dec-24-08. Since due to inclusion of wrong cloumn ItemCompanion_Fld_CompanionItemID in where clause 
                // instead of the column ItemCompanion_Fld_ItemID in the earlier query. This was causing non inclusion of companion items during the
                //Pos transaction.
                string sSQL1 = "Select "
                                    + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID
                                    + " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_ItemID
                                    + " , Item." + clsPOSDBConstants.Item_Fld_Description + " As ItemDescription "
                                    + " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_Amount
                                    + " , Companion." + clsPOSDBConstants.ItemCompanion_Fld_Percentage
                                + " FROM "
                                    + clsPOSDBConstants.ItemCompanion_tbl + " As Companion "
                                    + " , " + clsPOSDBConstants.Item_tbl + " As Item "
                                + " WHERE "
                                    + " Companion." + clsPOSDBConstants.ItemCompanion_Fld_ItemID + " = Item." + clsPOSDBConstants.Item_Fld_ItemID;
                //Added Till Here Dec-24-08
				//sWhereClause = sWhereClause.Replace("WHERE","AND");
				sWhereClause= String.Concat(" and " ,sWhereClause);

				string sSQL = String.Concat(sSQL1,sWhereClause);

				ItemCompanionData ds = new ItemCompanionData();
				ds.ItemCompanion.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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

		// Fills a ItemCompanionData with all ItemCompanion

		public ItemCompanionData PopulateList(string whereClause) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
			return(PopulateList(whereClause,conn));
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			ItemCompanionTable addedTable = (ItemCompanionTable)ds.Tables[clsPOSDBConstants.ItemCompanion_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (ItemCompanionRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemCompanion_tbl,insParam);
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

		// Update all rows in a ItemCompanion DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			ItemCompanionTable modifiedTable = (ItemCompanionTable)ds.Tables[clsPOSDBConstants.ItemCompanion_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (ItemCompanionRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemCompanion_tbl,updParam);

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

		// Delete all rows within a ItemCompanion DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			ItemCompanionTable table = (ItemCompanionTable)ds.Tables[clsPOSDBConstants.ItemCompanion_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (ItemCompanionRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemCompanion_tbl,delParam);
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
			sUpdateSQL = 	sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName + " and " +
				updParam[1].SourceColumn + " = " + updParam[1].ParameterName ;
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
		private IDbDataParameter[] PKParameters(System.String CompanionItemID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID;
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = CompanionItemID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemCompanionRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID;
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.CompanionItemID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemCompanionRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID, System.Data.SqlDbType.VarChar);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_ItemID, System.Data.SqlDbType.VarChar);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_Amount, System.Data.DbType.Currency);
			sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_Percentage, System.Data.SqlDbType.Float);

			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_ItemID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_Amount;
			sqlParams[3].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_Percentage;

			if (row.CompanionItemID != System.String.Empty )
				sqlParams[0].Value = row.CompanionItemID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.ItemID != System.String.Empty )
				sqlParams[1].Value = row.ItemID;
			else
				sqlParams[1].Value = DBNull.Value ;
			
			if (row.Amount != System.Decimal.MinValue )
				sqlParams[2].Value = row.Amount;
			else
				sqlParams[2].Value = DBNull.Value ;
			
			if (row.Percentage != System.Decimal.MinValue )
				sqlParams[3].Value = row.Percentage;
			else
				sqlParams[3].Value = DBNull.Value ;

			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemCompanionRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID, System.Data.SqlDbType.VarChar);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_ItemID, System.Data.SqlDbType.VarChar);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_Amount, System.Data.DbType.Currency);
			sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemCompanion_Fld_Percentage, System.Data.SqlDbType.Float);

			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_ItemID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_Amount;
			sqlParams[3].SourceColumn = clsPOSDBConstants.ItemCompanion_Fld_Percentage;

			if (row.CompanionItemID != System.String.Empty )
				sqlParams[0].Value = row.CompanionItemID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.ItemID != System.String.Empty )
				sqlParams[1].Value = row.ItemID;
			else
				sqlParams[1].Value = DBNull.Value ;
			
			if (row.Amount != System.Decimal.MinValue )
				sqlParams[2].Value = row.Amount;
			else
				sqlParams[2].Value = DBNull.Value ;
			
			if (row.Percentage != System.Decimal.MinValue )
				sqlParams[3].Value = row.Percentage;
			else
				sqlParams[3].Value = DBNull.Value ;

			return(sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
