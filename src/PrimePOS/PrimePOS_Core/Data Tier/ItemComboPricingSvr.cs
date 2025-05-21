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
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{
	

	// Provides data access methods for DeptCode
 
	public class ItemComboPricingSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();
		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(DataSet updates, IDbTransaction tx,ref int ID) 
		{
			try 
		
			{
                this.Insert(updates, tx,ref ID);
                this.Update(updates, tx, ref ID);

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
				logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx,ref int ID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}

		#endregion

		#region Get Methods

		public ItemComboPricingData Populate(System.Int32 Id, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select * FROM " 
								+ clsPOSDBConstants.ItemComboPricing_tbl
								+ " WHERE " 
								+ clsPOSDBConstants.ItemComboPricing_Fld_Id + " =" + Id.ToString();

				ItemComboPricingData ds = new ItemComboPricingData();
				ds.ItemComboPricing.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
											,  sSQL
											, PKParameters(Id)).Tables[0]);
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

		public ItemComboPricingData Populate(System.Int32 Id) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(Id, conn));
			}
		}

		public ItemComboPricingData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{
                string sSQL = "Select * "
                    + " FROM "
                    + clsPOSDBConstants.ItemComboPricing_tbl;
					
				if (sWhereClause.Trim()!="")
					sSQL=String.Concat(sSQL,sWhereClause);

				ItemComboPricingData ds = new  ItemComboPricingData();
				ds.ItemComboPricing.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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

        #endregion //Get Method

		#region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx,ref int ID) 
		{

			ItemComboPricingTable addedTable = (ItemComboPricingTable)ds.Tables[clsPOSDBConstants.ItemComboPricing_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (ItemComboPricingRow row in addedTable.Rows) 
				{
					try 
					{
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemComboPricing_tbl,insParam);
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        ID= Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));
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
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx,ref int ID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}
		}

		// Update all rows in a DeptCodes DataSet, within a given database transaction.
		public void Update(DataSet ds, IDbTransaction tx,ref int ID) 
		{	
			ItemComboPricingTable modifiedTable = (ItemComboPricingTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (ItemComboPricingRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemComboPricing_tbl,updParam);

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                        ID = row.Id;
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
						logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx,ref int ID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
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
			sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

			for(int i = 1;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName ;
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

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemComboPricingRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@Id";
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.Id;
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_Id;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemComboPricingRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8); //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY changed from 7 to 8
            sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricing_Fld_Description, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing, System.Data.DbType.Boolean);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice, System.Data.DbType.Decimal);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.fld_CreatedBy, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.fld_CreatedOn, System.Data.DbType.DateTime);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems, System.Data.DbType.Int32);    //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

            sqlParams[0].SourceColumn  = clsPOSDBConstants.ItemComboPricing_Fld_Description;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.fld_CreatedBy;
            sqlParams[4].SourceColumn = clsPOSDBConstants.fld_CreatedOn;
            sqlParams[5].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_IsActive;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems;    //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

            sqlParams[0].Value = row.Description;
			sqlParams[1].Value = row.ForceGroupPricing;
			sqlParams[2].Value = row.ComboItemPrice;
            sqlParams[3].Value = Configuration.UserName;
			sqlParams[4].Value=DateTime.Now;
            sqlParams[5].Value = row.MinComboItems;
            sqlParams[6].Value = row.IsActive;
            sqlParams[7].Value = row.MaxComboItems; //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

            return (sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemComboPricingRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(9); //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY changed from 8 to 9

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_Id, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_Description, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing, System.Data.DbType.Boolean);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice, System.Data.DbType.Decimal);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.fld_LastUpdatedBy, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.fld_LastUpdatedOn, System.Data.DbType.DateTime);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems, System.Data.DbType.Int32);    //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_Id;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_Description;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice;
			sqlParams[4].SourceColumn  = clsPOSDBConstants.fld_LastUpdatedBy;
            sqlParams[5].SourceColumn = clsPOSDBConstants.fld_LastUpdatedOn;
            sqlParams[6].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems;
            sqlParams[7].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_IsActive;
            sqlParams[8].SourceColumn = clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems;   //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

            sqlParams[0].Value = row.Id;
			sqlParams[1].Value = row.Description;
			sqlParams[2].Value = row.ForceGroupPricing;
			sqlParams[3].Value = row.ComboItemPrice;
            sqlParams[4].Value = Configuration.UserName;
			sqlParams[5].Value=DateTime.Now;
            sqlParams[6].Value = row.MinComboItems;
            sqlParams[7].Value = row.IsActive;
            sqlParams[8].Value = row.MaxComboItems; //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

            return (sqlParams);
		}

        #endregion
          
		public void Dispose() {}   
	}
}
