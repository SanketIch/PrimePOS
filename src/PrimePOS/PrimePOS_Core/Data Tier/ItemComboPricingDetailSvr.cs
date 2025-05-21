using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using System.Collections.Generic;
using NLog;

namespace POS_Core.DataAccess
{
	
	// Provides data access methods for DeptCode
 
	public class ItemComboPricingDetailSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.
		public void Persist(ItemComboPricingDetailData updates, int itemComboPricingId, IDbTransaction tx) 
		{
			try		
			{
				this.Delete(updates, tx);
                this.Insert(updates, itemComboPricingId, tx);
                this.Update(updates, itemComboPricingId, tx);

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
				logger.Fatal(ex, "Persist(ItemComboPricingDetailData updates, int itemComboPricingId, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}
		}

		#endregion

		#region Get Methods

		public ItemComboPricingDetailData Populate(System.Int32 ItemComboPricingId) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(ItemComboPricingId, conn));
			}
		}

		public ItemComboPricingDetailData Populate(System.Int32 ItemComboPricingId, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
					+ clsPOSDBConstants.ItemComboPricingDetail_Fld_Id
					+ " , " + clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId
					+ " , dt." +  clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID
					+ " , item." +  clsPOSDBConstants.Item_Fld_Description
					+ " , dt." +  clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY
					+ " , dt." +  clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice
					+ " FROM " 
					+ clsPOSDBConstants.ItemComboPricingDetail_tbl + " As dt"
					+ " , " + clsPOSDBConstants.Item_tbl + " As item"
					+ " WHERE " 
					+ " dt." + clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
					+ " AND dt." + clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId+ " = " + ItemComboPricingId;


				ItemComboPricingDetailData ds = new ItemComboPricingDetailData();
				ds.ItemComboPricingDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
					,  sSQL
					, PKParameters(ItemComboPricingId)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 ItemComboPricingId, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

        public List<KeyValuePair<Int32,string>> GetComboPricingList()
        {
            List<KeyValuePair<Int32, String>> comboList = new List<KeyValuePair<Int32, String>>();
            try
            {
                string sSQL = "Select cpd.ItemComboPricingId, cpd.ItemId "
                    + " From ItemComboPricingDetail cpd, ItemComboPricing cp where cp.Id=cpd.ItemComboPricingId and cp.IsActive=1";

                using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    IDataReader dr = DataHelper.ExecuteReader(conn, CommandType.Text, sSQL);
                    while (dr.Read())
                    {
                        comboList.Add(new KeyValuePair<Int32, string>(dr.GetInt32(0), dr.GetString(1)));
                    }
                }
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
				logger.Fatal(ex, "GetComboPricingList()");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
            }
            return comboList;
        }

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
        public void Insert(ItemComboPricingDetailData ds, int itemComboPricingId, IDbTransaction tx) 
		{

			ItemComboPricingDetailTable addedTable = (ItemComboPricingDetailTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (ItemComboPricingDetailRow row in addedTable.Rows) 
				{
					try 
					{
                        row.ItemComboPricingId = itemComboPricingId;
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemComboPricingDetail_tbl,insParam);
						
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
						logger.Fatal(ex, "Insert(ItemComboPricingDetailData ds, int itemComboPricingId, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a DeptCodes DataSet, within a given database transaction.
        public void Update(DataSet ds, int itemComboPricingId, IDbTransaction tx)
		{	
			ItemComboPricingDetailTable modifiedTable = (ItemComboPricingDetailTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (ItemComboPricingDetailRow row in modifiedTable.Rows) 
				{
					try 
					{
                        row.ItemComboPricingId = itemComboPricingId;
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemComboPricingDetail_tbl,updParam);

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
						logger.Fatal(ex, "Update(DataSet ds, int itemComboPricingId, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a DeptCodes DataSet, within a given database transaction.
        public void Delete(ItemComboPricingDetailData ds, IDbTransaction tx) 
		{

            ItemComboPricingDetailTable table = (ItemComboPricingDetailTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				foreach (ItemComboPricingDetailRow row in table.Rows) 
				{
					try 
					{
                        int id =POS_Core.Resources.Configuration.convertNullToInt( row["id", DataRowVersion.Original]);
                        delParam = PKParameters(id);
						sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemComboPricingDetail_tbl,delParam);
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
						logger.Fatal(ex, "Delete(ItemComboPricingDetailData ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
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

		private string BuildInsertSQL(string tableName, IDbDataParameter[] insParam)
		{
			string sInsertSQL = "INSERT INTO " + tableName + " ( ";
			// build where clause
			sInsertSQL = sInsertSQL + insParam[0].SourceColumn;

			for(int i = 1;i<insParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + insParam[i].SourceColumn ;
			}
			sInsertSQL = sInsertSQL + " ) Values (" + insParam[0].ParameterName;

			for(int i = 1;i<insParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + insParam[i].ParameterName ;
			}
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
		
        private IDbDataParameter[] PKParameters(System.Int32 Id) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@Id";
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = Id;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_Id;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemComboPricingDetailRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@Id";
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.Id;
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_Id;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemComboPricingDetailRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice, System.Data.DbType.Currency);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId, System.Data.DbType.Int32);
			
			sqlParams[0].SourceColumn  = clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId;
			
			sqlParams[0].Value = row.QTY;
			sqlParams[1].Value = row.SalePrice;
			sqlParams[2].Value = row.ItemID;
			sqlParams[3].Value = row.ItemComboPricingId;
			
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemComboPricingDetailRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricingDetail_Fld_Id, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_Id;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId;

            sqlParams[0].Value = row.Id;
            sqlParams[1].Value = row.QTY;
            sqlParams[2].Value = row.SalePrice;
            sqlParams[3].Value = row.ItemID;
            sqlParams[4].Value = row.ItemComboPricingId;

            return (sqlParams);
		}

		#endregion
    
		public void Dispose() {}   
	}
}
