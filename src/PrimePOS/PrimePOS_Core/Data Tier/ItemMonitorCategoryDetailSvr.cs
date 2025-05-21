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
using NLog;
using POS_Core.Resources;
//using POS.Resources;

namespace POS_Core.DataAccess
{
	

	// Provides data access methods for ItemMonitorCategoryDetail
 
	public class ItemMonitorCategoryDetailSvr: IDisposable  
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
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");
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
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
				//ErrorHandler.throwException(ex,"","");
			}

		}
		#endregion

		#region Get Methods

		// Looks up a ItemMonitorCategoryDetail based on its primary-key:System.String CompanionItemID

		public ItemMonitorCategoryDetailData Populate(System.String ItemID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select Detail." 
									+ clsPOSDBConstants.ItemMonitorCategory_Fld_ID 
									+ " , Detail." + clsPOSDBConstants.Item_Fld_ItemID 
									+ " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_Description + " As Description "
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE + " As ePSE " //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
									+ " , Detail." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID
									+ " , Detail." + clsPOSDBConstants.fld_UserID
                                    + " , Detail." + clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage
								+ " FROM " 
									+ clsPOSDBConstants.ItemMonitorCategoryDetail_tbl + " As Detail"
									+ " , "  +  clsPOSDBConstants.ItemMonitorCategory_tbl + " As Master "
								+ " WHERE " 
									+ clsPOSDBConstants.Item_Fld_ItemID + " ='" + ItemID + "'" 
									+ " AND Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ID + " = Detail." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID;

				ItemMonitorCategoryDetailData ds = new ItemMonitorCategoryDetailData();
				ds.ItemMonitorCategoryDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL ).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.String ItemID, IDbConnection conn)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.String ItemID, IDbConnection conn)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Populate(System.String ItemID, IDbConnection conn)");
                //ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public ItemMonitorCategoryDetailData Populate(System.String ItemID) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			return(Populate(ItemID, conn));
		}

		// Fills a ItemMonitorCategoryDetailData with all ItemMonitorCategoryDetail

		public ItemMonitorCategoryDetailData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{

                string sSQL1 = "Select Detail."
                                    + clsPOSDBConstants.ItemMonitorCategory_Fld_ID
                                    + " , Detail." + clsPOSDBConstants.Item_Fld_ItemID
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_Description + " As Description "
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE + " As ePSE "   //Sprint-23 - PRIMEPOS-2029 31-Mar-2016 JY Added
                    //+ " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit + " As 'Daily Limit' "  //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented
                    //+ " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit + " As 'Thirty Days Limit' "   //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays + " As 'Limit Period Days'"
                                    + " , Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty + " As 'Limit Period Qty'"
                                    + " , Detail." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID
                                    + " , Detail." + clsPOSDBConstants.fld_UserID
                                    + " , Detail." + clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage
                                + " FROM "
                                    + clsPOSDBConstants.ItemMonitorCategoryDetail_tbl + " As Detail"
                                    + " , " + clsPOSDBConstants.ItemMonitorCategory_tbl + " As Master "
                                + " WHERE "
                                    + " Master." + clsPOSDBConstants.ItemMonitorCategory_Fld_ID + " = Detail." + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID;
                
                //Added Till Here Dec-24-08
                //sWhereClause = sWhereClause.Replace("WHERE","AND");
				sWhereClause= String.Concat(" and " ,sWhereClause);

				string sSQL = String.Concat(sSQL1,sWhereClause);

				ItemMonitorCategoryDetailData ds = new ItemMonitorCategoryDetailData();
				ds.ItemMonitorCategoryDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
			}

			catch (Exception ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                //ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

		// Fills a ItemMonitorCategoryDetailData with all ItemMonitorCategoryDetail

		public ItemMonitorCategoryDetailData PopulateList(string whereClause) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
			return(PopulateList(whereClause,conn));
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			ItemMonitorCategoryDetailTable addedTable = (ItemMonitorCategoryDetailTable)ds.Tables[clsPOSDBConstants.ItemMonitorCategoryDetail_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (ItemMonitorCategoryDetailRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemMonitorCategoryDetail_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);

						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);

                        #region Sprint-25 - PRIMEPOS-2379 16-Mar-2017 JY Added logic to update PSE Item table
                        if (row.ePSE == true && row.UnitsPerPackage > 0)
                        {
                            using (PSEItemSvr oPSEItemSvr = new PSEItemSvr())
                            {
                                oPSEItemSvr.UpdatePSEItem(row.ItemID, row.ItemMonCatID, tx);
                            }
                        }
                        #endregion
                    }
                    catch (POSExceptions ex) 
					{
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");
                        throw (ex);
					}

					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");
                        throw (ex);
					}

					catch (Exception ex) 
					{
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");
                        //ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a ItemMonitorCategoryDetail DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			ItemMonitorCategoryDetailTable modifiedTable = (ItemMonitorCategoryDetailTable)ds.Tables[clsPOSDBConstants.ItemMonitorCategoryDetail_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (ItemMonitorCategoryDetailRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemMonitorCategoryDetail_tbl,updParam);

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);

                        #region Sprint-25 - PRIMEPOS-2379 16-Mar-2017 JY Added logic to update PSE Item table
                        if (row.ePSE == true && row.UnitsPerPackage > 0)
                        {
                            using (PSEItemSvr oPSEItemSvr = new PSEItemSvr())
                            {
                                oPSEItemSvr.UpdatePSEItem(row.ItemID, row.ItemMonCatID, tx);
                            }
                        }
                        #endregion
                    }
                    catch (POSExceptions ex) 
					{
                        logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");
                        throw (ex);
					}

					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");
                        throw (ex);
					}

					catch (Exception ex) 
					{
                        logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");
                        //ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a ItemMonitorCategoryDetail DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			ItemMonitorCategoryDetailTable table = (ItemMonitorCategoryDetailTable)ds.Tables[clsPOSDBConstants.ItemMonitorCategoryDetail_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (ItemMonitorCategoryDetailRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemMonitorCategoryDetail_tbl,delParam);
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL,delParam );
					} 
					catch(POSExceptions ex) 
					{
                        logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");
                        throw (ex);
					}

					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");
                        throw (ex);
					}

					catch (Exception ex) 
					{
                        logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");
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
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ID;
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = ID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemMonitorCategoryDetailRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ID;
			sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemMonitorCategoryDetailRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID, System.Data.SqlDbType.Int);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ItemID, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage, System.Data.SqlDbType.Decimal);

			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.Item_Fld_ItemID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage;


            sqlParams[0].Value = row.ItemMonCatID;

			if (row.ItemID != System.String.Empty )
				sqlParams[1].Value = row.ItemID;
			else
				sqlParams[1].Value = DBNull.Value ;

            sqlParams[2].Value = row.UnitsPerPackage;
			
			
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemMonitorCategoryDetailRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ID, System.Data.SqlDbType.Int);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID, System.Data.SqlDbType.Int);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ItemID, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage, System.Data.SqlDbType.Decimal);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Item_Fld_ItemID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage;


            sqlParams[0].Value = row.ID;
            sqlParams[1].Value = row.ItemMonCatID;

            if (row.ItemID != System.String.Empty)
                sqlParams[2].Value = row.ItemID;
            else
                sqlParams[2].Value = DBNull.Value;
            sqlParams[3].Value = row.UnitsPerPackage;


            return (sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
