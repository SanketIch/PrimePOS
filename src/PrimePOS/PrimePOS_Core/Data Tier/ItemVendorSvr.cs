// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using Resources;
//using Resources;
using System;
using System.Data;
using System.Data.SqlClient;
using NLog;

namespace POS_Core.DataAccess
{
	

    // Provides data access methods for ItemVendor

    public class ItemVendorSvr : IDisposable
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public void Persist(DataSet updates, IDbTransaction tx)
		{
			try
			{
				this.Delete(updates, tx);
				this.Insert(updates, tx);
				this.Update(updates, tx);

				updates.AcceptChanges();
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
				logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
			}
		}

		// Inserts, updates or deletes rows in a DataSet.

		public void Persist(DataSet updates)
		{

            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                conn.Open();
                IDbTransaction tx = conn.BeginTransaction();
                try
                {
                    this.Persist(updates, tx);
                    tx.Commit();
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
                    tx.Rollback();
					logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
					//ErrorHandler.throwException(ex, "", "");
                }
            }
		}

		#endregion Persist Methods

		#region Get Methods

		// Looks up a ItemVendor based on its primary-key:System.String VendorItemID

		public ItemVendorData Populate(System.String VendorItemID, IDbConnection conn)
		{
			try
			{
				#region Commected code

				//string sSQL = "Select "
				//        + " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID
				//        + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID
				//        + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemDetailID
				//        + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID
				//        + " , " + clsPOSDBConstants.Vendor_Fld_VendorCode
				//        + " , " + clsPOSDBConstants.Vendor_Fld_VendorName
				//        + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice
				//        + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_LastOrderDate
				//        + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_IsDeleted
				//    + " FROM "
				//        + clsPOSDBConstants.ItemVendor_tbl + " As ItemVendor "
				//        + " , " + clsPOSDBConstants.Vendor_tbl + " As Vendor "
				//    + " WHERE "
				//        + " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID + " = Vendor." + clsPOSDBConstants.Vendor_Fld_VendorId
				//        + " AND ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " = '" + VendorItemID + "'";

				#endregion Commected code

				string sSQL = "Select "
						 + " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemDetailID
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID
						 + " , " + clsPOSDBConstants.Vendor_Fld_VendorCode
						 + " , " + clsPOSDBConstants.Vendor_Fld_VendorName
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_LastOrderDate
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_CatPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ContractPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_NetItemPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ProducerPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_RetailPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_IsDeleted
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ResalePrice
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_BaseCharge
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckSize
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckQty
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckUnit
						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice
					//Added by Ravindra

						  + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass
						   + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice
                            + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_SaleStartDate
                           + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_SaleEndDate
					//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
					 + " FROM "
						 + clsPOSDBConstants.ItemVendor_tbl + " As ItemVendor "
						 + " , " + clsPOSDBConstants.Vendor_tbl + " As Vendor "
					 + " WHERE "
						 + " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID + " = Vendor." + clsPOSDBConstants.Vendor_Fld_VendorId
						 + " AND ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " = '" + VendorItemID + "'";

				ItemVendorData ds = new ItemVendorData();
				ds.ItemVendor.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(VendorItemID)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.String VendorItemID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
				return null;
			}
		}

		public ItemVendorData Populate(System.String VendorItemID)
		{
            using (IDbConnection conn = DataFactory.CreateConnection()) //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (Populate(VendorItemID, conn));
            }
		}

		// Fills a ItemVendorData with all ItemVendor

		public ItemVendorData PopulateList(string sWhereClause, IDbConnection conn)
		{
			try
			{
				string sSQL1 = "Select "
						+ " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemDetailID
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID
						+ " , " + clsPOSDBConstants.Vendor_Fld_VendorCode
						+ " , " + clsPOSDBConstants.Vendor_Fld_VendorName
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_LastOrderDate
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_CatPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ContractPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_NetItemPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ProducerPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_RetailPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_IsDeleted
					//Added by SRT(Abhishek) Date : 24/09/2009
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ResalePrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_BaseCharge
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckSize
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckQty
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckUnit
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice
					//Added by Ravindra

						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass
						  + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice
					//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

                         + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_SaleStartDate
                          + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_SaleEndDate
					//End Of Added by SRT(Abhishek) Date : 24/09/2009
					+ " FROM "
						+ clsPOSDBConstants.ItemVendor_tbl + " As ItemVendor "
						+ " , " + clsPOSDBConstants.Vendor_tbl + " As Vendor "
					+ " WHERE "
						+ " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID + " = Vendor." + clsPOSDBConstants.Vendor_Fld_VendorId;

				sWhereClause = sWhereClause.ToUpper().Replace("WHERE", "AND");
				string sSQL = String.Concat(sSQL1, sWhereClause);

				ItemVendorData ds = new ItemVendorData();
				ds.ItemVendor.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
				return null;
			}
		}

		// Fills a ItemVendorData with all ItemVendor
		public ItemVendorData PopulateList(string sWhereClause, IDbConnection conn, bool bUpdateWhereClause = true)
		{
			try
			{
				string sSQL1 = "Select "
						+ " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemDetailID
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID
						+ " , " + clsPOSDBConstants.Vendor_Fld_VendorCode
						+ " , " + clsPOSDBConstants.Vendor_Fld_VendorName
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_LastOrderDate
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_CatPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ContractPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_NetItemPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ProducerPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_RetailPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_IsDeleted
						//Added by SRT(Abhishek) Date : 24/09/2009
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ResalePrice
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_BaseCharge
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckSize
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckQty
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_PckUnit
						+ " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice
						 //Added by Ravindra

						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass
						  + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice
						 //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

						 + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_SaleStartDate
						  + " , ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_SaleEndDate
					//End Of Added by SRT(Abhishek) Date : 24/09/2009
					+ " FROM "
						+ clsPOSDBConstants.ItemVendor_tbl + " As ItemVendor "
						+ " , " + clsPOSDBConstants.Vendor_tbl + " As Vendor "
					+ " WHERE "
						+ " ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_VendorID + " = Vendor." + clsPOSDBConstants.Vendor_Fld_VendorId;
				if (sWhereClause.Trim().StartsWith("where"))
					sWhereClause = " AND " + sWhereClause.Trim().Substring(5, sWhereClause.Length - 6);
				string sSQL = String.Concat(sSQL1, sWhereClause);

				ItemVendorData ds = new ItemVendorData();
				ds.ItemVendor.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, bool bUpdateWhereClause = true)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
																							//ErrorHandler.throwException(ex, "", "");
				return null;
			}
		}

		public ItemVendorData PopulateList(string whereClause, bool bUpdateWhereClause = true)
		{
			using (IDbConnection conn = DataFactory.CreateConnection()) //Sprint-22 05-Nov-2015 JY Added using clause
			{
				conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
				return (PopulateList(whereClause, conn, bUpdateWhereClause));
			}
		}

		public ItemVendorData PopulateList(string whereClause)
		{
            using (IDbConnection conn = DataFactory.CreateConnection()) //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateList(whereClause, conn));
            }
		}

		#endregion Get Methods

		#region Insert, Update, and Delete Methods

		public void Insert(DataSet ds, IDbTransaction tx)
		{
			ItemVendorTable addedTable = (ItemVendorTable)ds.Tables[clsPOSDBConstants.ItemVendor_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter[] insParam;

			if (addedTable != null && addedTable.Rows.Count > 0)
			{
				foreach (ItemVendorRow row in addedTable.Rows)
				{
					try
					{
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.ItemVendor_tbl, insParam);
						for (int i = 0; i < insParam.Length; i++)
						{
							Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
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
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex, "", "");
					}
				}
				addedTable.AcceptChanges();
			}
		}

		// Update all rows in a ItemVendor DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx)
		{
			ItemVendorTable modifiedTable = (ItemVendorTable)ds.Tables[clsPOSDBConstants.ItemVendor_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter[] updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0)
			{
				foreach (ItemVendorRow row in modifiedTable.Rows)
				{
					try
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.ItemVendor_tbl, updParam);

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
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
						logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex, "", "");
					}
				}
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a ItemVendor DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx)
		{
			ItemVendorTable table = (ItemVendorTable)ds.Tables[clsPOSDBConstants.ItemVendor_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter[] delParam;

			if (table != null && table.Rows.Count > 0)
			{
				table.RejectChanges(); //so we can access the rows
				foreach (ItemVendorRow row in table.Rows)
				{
					try
					{
						delParam = PKParameters(row);

						sSQL = BuildDeleteSQL(clsPOSDBConstants.ItemVendor_tbl, delParam);
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
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
						logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex, "", "");
					}
				}
			}
		}

		private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
		{
			string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
			// build where clause
			for (int i = 0; i < delParam.Length; i++)
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

			for (int i = 1; i < delParam.Length; i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
			}
			sInsertSQL = sInsertSQL + " , UserId ";
			sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

			for (int i = 1; i < delParam.Length; i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
			}
			sInsertSQL = sInsertSQL + " , '" + Configuration.UserName + "'";
			sInsertSQL = sInsertSQL + " )";
			return sInsertSQL;
		}

		private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
		{
			string sUpdateSQL = "UPDATE " + tableName + " SET ";
			// build where clause
			sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

			for (int i = 2; i < updParam.Length; i++)
			{
				sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
			}
			sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'";
			sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
			return sUpdateSQL;
		}

		#endregion Insert, Update, and Delete Methods

		#region IDBDataParameter Generator Methods

		private IDbDataParameter[] whereParameters(string swhere)
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
			sqlParams[0] = DataFactory.CreateParameter();

			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Size = 2000;
			sqlParams[0].ParameterName = "@whereClause";

			sqlParams[0].Value = swhere;
			return (sqlParams);
		}

		private IDbDataParameter[] PKParameters(System.String VendorItemID)
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemVendor_Fld_VendorItemID;
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = VendorItemID;

			return (sqlParams);
		}

		private IDbDataParameter[] PKParameters(ItemVendorRow row)
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.ItemVendor_Fld_VendorItemID;
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.VendorItemID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorItemID;

			return (sqlParams);
		}

		private IDbDataParameter[] InsertParameters(ItemVendorRow row)
		{
			//Modified by Atul Joshi on 25-10-2010
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(27); //DataFactory.CreateParameterArray(23);      //(15);

			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ItemID, System.Data.SqlDbType.VarChar);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorID, System.Data.SqlDbType.Int);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorItemID, System.Data.SqlDbType.VarChar);
			sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice, System.Data.DbType.Currency);
			sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_LastOrderDate, System.Data.SqlDbType.DateTime);

			sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice, System.Data.DbType.Currency);
			sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_CatPrice, System.Data.DbType.Currency);
			sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ContractPrice, System.Data.DbType.Currency);
			sqlParams[8] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice, System.Data.DbType.Currency);
			sqlParams[9] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice, System.Data.DbType.Currency);
			sqlParams[10] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice, System.Data.DbType.Currency);
			sqlParams[11] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_NetItemPrice, System.Data.DbType.Currency);
			sqlParams[12] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ProducerPrice, System.Data.DbType.Currency);
			sqlParams[13] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_RetailPrice, System.Data.DbType.Currency);
			sqlParams[14] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_IsDeleted, System.Data.DbType.Boolean);

			//Added by SRT(Abhishek) Date:24/09/2009
			sqlParams[15] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice, System.Data.DbType.Currency);
			sqlParams[16] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity, System.Data.DbType.Currency);
			sqlParams[17] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ResalePrice, System.Data.DbType.Currency);
			sqlParams[18] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_BaseCharge, System.Data.DbType.Currency);

			sqlParams[19] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckSize, System.Data.SqlDbType.VarChar);
			sqlParams[20] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckQty, System.Data.SqlDbType.VarChar);
			sqlParams[21] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckUnit, System.Data.SqlDbType.VarChar);

			//Added by Atul Joshi on 25-10-2010
			sqlParams[22] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice, System.Data.DbType.Currency);
			//End Of Added by SRT(Abhishek) Date:24/09/2009
			sqlParams[23] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass, System.Data.DbType.String);//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
			sqlParams[24] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice, System.Data.DbType.Currency);//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

            sqlParams[25] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_SaleStartDate, System.Data.DbType.DateTime);//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
            sqlParams[26] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_SaleEndDate, System.Data.DbType.DateTime);//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ItemID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorItemID;
			sqlParams[3].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice;
			sqlParams[4].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_LastOrderDate;
			sqlParams[5].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice;
			sqlParams[6].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_CatPrice;
			sqlParams[7].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ContractPrice;
			sqlParams[8].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice;
			sqlParams[9].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice;
			sqlParams[10].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice;
			sqlParams[11].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_NetItemPrice;
			sqlParams[12].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ProducerPrice;
			sqlParams[13].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_RetailPrice;
			sqlParams[14].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_IsDeleted;

			//Added by SRT(Abhishek) Date:24/09/2009
			sqlParams[15].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice;
			sqlParams[16].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity;
			sqlParams[17].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ResalePrice;
			sqlParams[18].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_BaseCharge;

			sqlParams[19].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckSize;
			sqlParams[20].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckQty;
			sqlParams[21].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckUnit;
			//End Of Added by SRT(Abhishek) Date:24/09/2009

			//Added by Atul Joshi on 25-10-2010
			sqlParams[22].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice;

			sqlParams[23].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass;   //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
			sqlParams[24].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice; //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

            sqlParams[25].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_SaleStartDate;   //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
            sqlParams[26].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_SaleEndDate; //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

			if (row.ItemID != System.String.Empty)
				sqlParams[0].Value = row.ItemID;
			else
				sqlParams[0].Value = DBNull.Value;

			if (row.VendorID != 0)
				sqlParams[1].Value = row.VendorID;
			else
				sqlParams[1].Value = 0;

			if (row.VendorItemID != "")
				sqlParams[2].Value = row.VendorItemID;
			else
				sqlParams[2].Value = "";

			if (row.VenorCostPrice != 0)
				sqlParams[3].Value = row.VenorCostPrice;
			else
				sqlParams[3].Value = 0;

			if (row.LastOrderDate != System.DateTime.MinValue)
				sqlParams[4].Value = row.LastOrderDate;
			else
				sqlParams[4].Value = System.DateTime.MinValue;

			if (row.AverageWholeSalePrice != 0)
				sqlParams[5].Value = row.AverageWholeSalePrice;
			else
				sqlParams[5].Value = 0;

			if (row.CatalogPrice != 0)
				sqlParams[6].Value = row.CatalogPrice;
			else
				sqlParams[6].Value = 0;

			if (row.ContractPrice != 0)
				sqlParams[7].Value = row.ContractPrice;
			else
				sqlParams[7].Value = 0;

			if (row.DealerAdjustedPrice != 0)
				sqlParams[8].Value = row.DealerAdjustedPrice;
			else
				sqlParams[8].Value = 0;

			if (row.FedrelUpperLimitPrice != 0)
				sqlParams[9].Value = row.FedrelUpperLimitPrice;
			else
				sqlParams[9].Value = 0;

			if (row.ManufacturerSuggPrice != 0)
				sqlParams[10].Value = row.ManufacturerSuggPrice;
			else
				sqlParams[10].Value = 0;

			if (row.NetItemPrice != 0)
				sqlParams[11].Value = row.NetItemPrice;
			else
				sqlParams[11].Value = 0;

			if (row.ProducersPrice != 0)
				sqlParams[12].Value = row.ProducersPrice;
			else
				sqlParams[12].Value = 0;

			if (row.RetailPrice != 0)
				sqlParams[13].Value = row.RetailPrice;
			else
				sqlParams[13].Value = 0;

			sqlParams[14].Value = row.IsDeleted;

			//Added by SRT(Abhishek) Date : 24/09/2009
			if (row.InVoiceBillingPrice != 0)
				sqlParams[15].Value = row.InVoiceBillingPrice;
			else
				sqlParams[15].Value = 0;

			if (row.UnitPriceBegQuantity != 0)
				sqlParams[16].Value = row.UnitPriceBegQuantity;
			else
				sqlParams[16].Value = 0;

			if (row.Resale != 0)
				sqlParams[17].Value = row.Resale;
			else
				sqlParams[17].Value = 0;

			if (row.BaseCharge != 0)
				sqlParams[18].Value = row.BaseCharge;
			else
				sqlParams[18].Value = 0;

			if (row.PckSize != System.String.Empty)
				sqlParams[19].Value = row.PckSize;
			else
				sqlParams[19].Value = DBNull.Value;

			if (row.PckQty != System.String.Empty)
				sqlParams[20].Value = row.PckQty;
			else
				sqlParams[20].Value = DBNull.Value;

			if (row.PckUnit != System.String.Empty)
				sqlParams[21].Value = row.PckUnit;
			else
				sqlParams[21].Value = DBNull.Value;

			//End Of Added by SRT(Abhishek) Date : 24/09/2009

			//Added by Atul Joshi on 25-10-2010
			if (row.UnitCostPrice != 0)
				sqlParams[22].Value = row.UnitCostPrice;
			else
				sqlParams[22].Value = DBNull.Value;

			//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
			if (row.HammacherDeptClass != System.String.Empty)
                sqlParams[23].Value = row.HammacherDeptClass;
			else
				sqlParams[23].Value = DBNull.Value;
			if (row.VendorSalePrice != 0)
				sqlParams[24].Value = row.VendorSalePrice;
			else
				sqlParams[24].Value = DBNull.Value;
			//Till here Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
            if (row.SaleStartDate != DateTime.MinValue)
                sqlParams[25].Value = row.SaleStartDate;
            else
                sqlParams[25].Value = DBNull.Value;

            if (row.SaleEndDate != DateTime.MinValue)
                sqlParams[26].Value = row.SaleEndDate;
            else
                sqlParams[26].Value = DBNull.Value;

			return (sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(ItemVendorRow row)
		{
			//Modified by Atul Joshi on 25-10-2010
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(27);// DataFactory.CreateParameterArray(23); //(15);

			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ItemDetailID, System.Data.SqlDbType.Int);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorID, System.Data.SqlDbType.Int);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorItemID, System.Data.SqlDbType.VarChar);
			sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice, System.Data.DbType.Currency);
			sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_LastOrderDate, System.Data.SqlDbType.DateTime);

			sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice, System.Data.DbType.Currency);
			sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_CatPrice, System.Data.DbType.Currency);
			sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ContractPrice, System.Data.DbType.Currency);
			sqlParams[8] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice, System.Data.DbType.Currency);
			sqlParams[9] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice, System.Data.DbType.Currency);
			sqlParams[10] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice, System.Data.DbType.Currency);
			sqlParams[11] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_NetItemPrice, System.Data.DbType.Currency);
			sqlParams[12] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ProducerPrice, System.Data.DbType.Currency);
			sqlParams[13] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_RetailPrice, System.Data.DbType.Currency);
			sqlParams[14] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_IsDeleted, System.Data.DbType.Boolean);

			//Added by SRT(Abhishek) Date:24/09/2009
			sqlParams[15] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice, System.Data.DbType.Currency);
			sqlParams[16] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity, System.Data.DbType.Currency);
			sqlParams[17] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_ResalePrice, System.Data.DbType.Currency);
			sqlParams[18] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_BaseCharge, System.Data.DbType.Currency);

			sqlParams[19] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckSize, System.Data.SqlDbType.VarChar);
			sqlParams[20] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckQty, System.Data.SqlDbType.VarChar);
			sqlParams[21] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckUnit, System.Data.SqlDbType.VarChar);
			//End Of Added by SRT(Abhishek) Date:24/09/2009

			//Added by Atul Joshi on 25-10-2010
			sqlParams[22] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice, System.Data.DbType.Currency);
			sqlParams[23] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass, System.Data.DbType.String);//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
			sqlParams[24] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice, System.Data.DbType.Currency);//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
            sqlParams[25] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_SaleStartDate, System.Data.DbType.DateTime);
            sqlParams[26] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_SaleEndDate, System.Data.DbType.DateTime);
            

			sqlParams[0].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ItemDetailID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorItemID;
			sqlParams[3].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice;
			sqlParams[4].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_LastOrderDate;

			sqlParams[5].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice;
			sqlParams[6].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_CatPrice;
			sqlParams[7].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ContractPrice;
			sqlParams[8].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice;
			sqlParams[9].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice;
			sqlParams[10].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice;
			sqlParams[11].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_NetItemPrice;
			sqlParams[12].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ProducerPrice;
			sqlParams[13].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_RetailPrice;
			sqlParams[14].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_IsDeleted;

			//Added by SRT(Abhishek) Date:24/09/2009
			sqlParams[15].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice;
			sqlParams[16].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity;
			sqlParams[17].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_ResalePrice;
			sqlParams[18].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_BaseCharge;

			sqlParams[19].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckSize;
			sqlParams[20].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckQty;
			sqlParams[21].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckUnit;
			//End Of Added by SRT(Abhishek) Date:24/09/2009

			//Added by Atul Joshi on 25-10-2010
			sqlParams[22].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice;

			sqlParams[23].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass;   //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
			sqlParams[24].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice; //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

            sqlParams[25].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_SaleStartDate;  
            sqlParams[26].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_SaleEndDate; 

			if (row.ItemDetailID != 0)
				sqlParams[0].Value = row.ItemDetailID;
			else
				sqlParams[0].Value = DBNull.Value;

			if (row.VendorID != 0)
				sqlParams[1].Value = row.VendorID;
			else
				sqlParams[1].Value = 0;

			if (row.VendorItemID != "")
				sqlParams[2].Value = row.VendorItemID;
			else
				sqlParams[2].Value = "";

			if (row.VenorCostPrice != 0)
				sqlParams[3].Value = row.VenorCostPrice;
			else
				sqlParams[3].Value = 0;

			if (row.LastOrderDate != System.DateTime.MinValue)
				sqlParams[4].Value = row.LastOrderDate;
			else
				sqlParams[4].Value = System.DateTime.MinValue;

			if (row.AverageWholeSalePrice != 0)
				sqlParams[5].Value = row.AverageWholeSalePrice;
			else
				sqlParams[5].Value = 0;

			if (row.CatalogPrice != 0)
				sqlParams[6].Value = row.CatalogPrice;
			else
				sqlParams[6].Value = 0;

			if (row.ContractPrice != 0)
				sqlParams[7].Value = row.ContractPrice;
			else
				sqlParams[7].Value = 0;

			if (row.DealerAdjustedPrice != 0)
				sqlParams[8].Value = row.DealerAdjustedPrice;
			else
				sqlParams[8].Value = 0;

			if (row.FedrelUpperLimitPrice != 0)
				sqlParams[9].Value = row.FedrelUpperLimitPrice;
			else
				sqlParams[9].Value = 0;

			if (row.ManufacturerSuggPrice != 0)
				sqlParams[10].Value = row.ManufacturerSuggPrice;
			else
				sqlParams[10].Value = 0;

			if (row.NetItemPrice != 0)
				sqlParams[11].Value = row.NetItemPrice;
			else
				sqlParams[11].Value = 0;

			if (row.ProducersPrice != 0)
				sqlParams[12].Value = row.ProducersPrice;
			else
				sqlParams[12].Value = 0;

			if (row.RetailPrice != 0)
				sqlParams[13].Value = row.RetailPrice;
			else
				sqlParams[13].Value = 0;

			sqlParams[14].Value = row.IsDeleted;

			//Added by SRT(Abhishek) Date : 24/09/2009
			if (row.InVoiceBillingPrice != 0)
				sqlParams[15].Value = row.InVoiceBillingPrice;
			else
				sqlParams[15].Value = 0;

			if (row.UnitPriceBegQuantity != 0)
				sqlParams[16].Value = row.UnitPriceBegQuantity;
			else
				sqlParams[16].Value = 0;

			if (row.Resale != 0)
				sqlParams[17].Value = row.Resale;
			else
				sqlParams[17].Value = 0;

			if (row.BaseCharge != 0)
				sqlParams[18].Value = row.BaseCharge;
			else
				sqlParams[18].Value = 0;

			if (row.PckSize != System.String.Empty)
				sqlParams[19].Value = row.PckSize;
			else
				sqlParams[19].Value = DBNull.Value;

			if (row.PckQty != System.String.Empty)
				sqlParams[20].Value = row.PckQty;
			else
				sqlParams[20].Value = DBNull.Value;

			if (row.PckUnit != System.String.Empty)
				sqlParams[21].Value = row.PckUnit;
			else
				sqlParams[21].Value = DBNull.Value;

			//End Of Added by SRT(Abhishek) Date : 24/09/2009

			//Added by Atul Joshi on 25-10-2010
			if (row.UnitCostPrice != 0)
				sqlParams[22].Value = row.UnitCostPrice;
			else
				sqlParams[22].Value = DBNull.Value;
			//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
			if (row.HammacherDeptClass != System.String.Empty)
				sqlParams[23].Value = row.HammacherDeptClass;
			else
				sqlParams[23].Value = DBNull.Value;
			if (row.VendorSalePrice != 0)
				sqlParams[24].Value = row.VendorSalePrice;
			else
				sqlParams[24].Value = DBNull.Value;
			//Till here Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

            if (row.SaleStartDate != DateTime.MinValue)
                sqlParams[25].Value = row.SaleStartDate;
            else
                sqlParams[25].Value = DBNull.Value;
            if (row.SaleEndDate != DateTime.MinValue)
                sqlParams[26].Value = row.SaleEndDate;
            else
                sqlParams[26].Value = DBNull.Value;

			return (sqlParams);
		}

		#endregion IDBDataParameter Generator Methods

        #region Sprint-21 - 2207 13-Aug-2015 JY Added
        public void UpdateItemVendor(int nItemDetailID, string strVendorItemID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

                string strSQL = "UPDATE ItemVendor SET VendorItemID = '" + strVendorItemID + "' WHERE ItemDetailID = " + nItemDetailID;
                DataHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
            }
        }
        #endregion

        public void Dispose()
		{
		}
	}
}