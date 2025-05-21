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
	
	// Provides data access methods for ItemGroupPrice
 
	public class ItemGroupPriceSvr: IDisposable  
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

		// Looks up a ItemGroupPrice based on its primary-key:System.String GroupPriceID

		public ItemGroupPriceData Populate(System.Int32 GroupPriceID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
						+ clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID 
						+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_ItemID 
						+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_Qty 
						+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_Cost 
						+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice 
                        + " , " + clsPOSDBConstants.ItemGroupPrice_Fld_StartDate
                        + " , " + clsPOSDBConstants.ItemGroupPrice_Fld_EndDate
					+ " FROM " 
						+ clsPOSDBConstants.ItemGroupPrice_tbl + " As GroupPrice "
					+ " WHERE " 
						+ clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID + " =" + GroupPriceID;

				ItemGroupPriceData ds = new ItemGroupPriceData();
				ds.ItemGroupPrice.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , PKParameters(GroupPriceID)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 GroupPriceID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public ItemGroupPriceData Populate(System.Int32 GroupPriceID) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			return(Populate(GroupPriceID, conn));
		}

		// Fills a ItemGroupPriceData with all ItemGroupPrice

		public ItemGroupPriceData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL1 = "Select " 
					+ clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID 
					+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_ItemID 
					+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_Qty 
					+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_Cost 
					+ " , " + clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice
                    + " , " + clsPOSDBConstants.ItemGroupPrice_Fld_StartDate
                        + " , " + clsPOSDBConstants.ItemGroupPrice_Fld_EndDate
					+ " FROM " 
					+ clsPOSDBConstants.ItemGroupPrice_tbl + " As GroupPrice ";
				
				if (sWhereClause.Trim()!="")
					sWhereClause = String.Concat(" where " , sWhereClause);

				string sSQL = String.Concat(sSQL1,sWhereClause);

				ItemGroupPriceData ds = new ItemGroupPriceData();
				ds.ItemGroupPrice.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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

		// Fills a ItemGroupPriceData with all ItemGroupPrice

		public ItemGroupPriceData PopulateList(string whereClause) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
			return(PopulateList(whereClause,conn));
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			ItemGroupPriceTable addedTable = (ItemGroupPriceTable)ds.Tables[clsPOSDBConstants.ItemGroupPrice_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (ItemGroupPriceRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemGroupPrice_tbl,insParam);
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

		// Update all rows in a ItemGroupPrice DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			ItemGroupPriceTable modifiedTable = (ItemGroupPriceTable)ds.Tables[clsPOSDBConstants.ItemGroupPrice_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (ItemGroupPriceRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemGroupPrice_tbl,updParam);

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

		// Delete all rows within a ItemGroupPrice DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			ItemGroupPriceTable table = (ItemGroupPriceTable)ds.Tables[clsPOSDBConstants.ItemGroupPrice_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (ItemGroupPriceRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemGroupPrice_tbl,delParam);
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
		private IDbDataParameter[] PKParameters(System.Int32 GroupPriceID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID;
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = GroupPriceID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemGroupPriceRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID;
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.GroupPriceID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemGroupPriceRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_ItemID, System.Data.SqlDbType.VarChar);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_Qty, System.Data.SqlDbType.Int);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_Cost, System.Data.DbType.Currency);
			sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice, System.Data.DbType.Currency);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_StartDate, System.Data.SqlDbType.SmallDateTime);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_EndDate, System.Data.SqlDbType.SmallDateTime);
            
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_ItemID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_Qty;
			sqlParams[2].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_Cost;
			sqlParams[3].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_StartDate;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_EndDate;

			if (row.ItemID != System.String.Empty )
				sqlParams[0].Value = row.ItemID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.Qty != 0 )
				sqlParams[1].Value = row.Qty;
			else
				sqlParams[1].Value = 0 ;
			
			if (row.Cost != 0 )
				sqlParams[2].Value = row.Cost;
			else
				sqlParams[2].Value = 0 ;
			
			if (row.SalePrice != 0 )
				sqlParams[3].Value = row.SalePrice;
			else
				sqlParams[3].Value = 0 ;

            if (row.StartDate.HasValue == true)
            {
                sqlParams[4].Value = row.StartDate.Value;
            }
            else
            {
                sqlParams[4].Value = DBNull.Value;
            }

            if (row.EndDate.HasValue == true)
            {
                sqlParams[5].Value = row.EndDate.Value;
            }
            else
            {
                sqlParams[5].Value = DBNull.Value;
            }
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemGroupPriceRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID, System.Data.SqlDbType.Int);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_ItemID, System.Data.SqlDbType.VarChar);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_Qty, System.Data.SqlDbType.Int);
			sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_Cost, System.Data.DbType.Currency);
			sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice, System.Data.DbType.Currency);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_StartDate, System.Data.SqlDbType.SmallDateTime);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ItemGroupPrice_Fld_EndDate, System.Data.SqlDbType.SmallDateTime);
            
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_ItemID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_Qty;
			sqlParams[3].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_Cost;
			sqlParams[4].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_StartDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemGroupPrice_Fld_EndDate;

			if (row.GroupPriceID != 0 )
				sqlParams[0].Value = row.GroupPriceID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.ItemID != System.String.Empty )
				sqlParams[1].Value = row.ItemID;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.Qty != 0 )
				sqlParams[2].Value = row.Qty;
			else
				sqlParams[2].Value = DBNull.Value ;
			
			//if (row.Cost != 0 )
			sqlParams[3].Value = row.Cost;
			//else
			//	sqlParams[3].Value = DBNull.Value ;
			
			if (row.SalePrice != 0 )
				sqlParams[4].Value = row.SalePrice;
			else
				sqlParams[4].Value = DBNull.Value ;
            if (row.StartDate.HasValue == true)
            {
                sqlParams[5].Value = row.StartDate.Value;
            }
            else
            {
                sqlParams[5].Value = DBNull.Value;
            }

            if (row.EndDate.HasValue == true)
            {
                sqlParams[6].Value = row.EndDate.Value;
            }
            else
            {
                sqlParams[6].Value = DBNull.Value;
            }

			return(sqlParams);
		}
		#endregion

		public void Dispose() {}   
	}
}
