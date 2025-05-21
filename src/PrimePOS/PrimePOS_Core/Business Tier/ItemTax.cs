using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
//using POS.Resources;
using System;
using System.Data;
using Resources;
using POS_Core.Resources;
using NLog;

namespace POS_Core.BusinessRules
{
    public class ItemTax : IDisposable
    {
        private const string Department = "D";
        private const string Item = "I";
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Insert or update item data.
        /// </summary>
        /// <param name="updates">ItemTax type dataset.</param>
        public void Persist(ItemTaxData updates)
        {
            try
            {
                IDbTransaction tx;
                IDbConnection oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
                tx = oConn.BeginTransaction();
                Persist(updates, tx);
            }
            catch (POSExceptions)
            {
                throw;
            }

            catch (OtherExceptions)
            {
                throw;
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(ItemTaxData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                   //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }

        /// <summary>
        /// Insert or update data by checking user priviliges.
        /// This insertion or updation takes place under Transactions.
        /// </summary>
        /// <param name="updates">Item type dataset.</param>
        /// <param name="tx">This insertion or updation will be the part of this transaction.</param>
        public void Persist(ItemTaxData updates, IDbTransaction tx)
        {
            try
            {
                UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998);

                CheckIsValidData(updates);
                using (ItemTaxSvr dao = new ItemTaxSvr())
                {
                    dao.Persist(updates, tx);
                    tx.Commit();
                }
            }
            catch (POSExceptions)
            {
                tx.Rollback();
                throw;
            }

            catch (OtherExceptions)
            {
                tx.Rollback();
                throw;
            }

            catch (Exception ex)
            {
                tx.Rollback();
                logger.Fatal(ex, "Persist(ItemTaxData updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                      //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }

        ///// <summary>
        ///// Fills a DataSet with all Items based on a condition.
        ///// </summary>
        ///// <param name="whereClause">SQL WHERE CLUASE type condition.</param>
        ///// <returns>returns Typed DataSet (ItemData).</returns>
        //public ItemTaxData PopulateList(string whereClause)
        //{
        //	try
        //	{
        //		using (ItemTaxSvr dao = new ItemTaxSvr())
        //		{
        //			return dao.PopulateList(whereClause);
        //		}
        //	}
        //	catch (POSExceptions)
        //	{
        //		throw;
        //	}

        //	catch (OtherExceptions)
        //	{
        //		throw;
        //	}

        //	catch (Exception ex)
        //	{
        //		ErrorHandler.throwException(ex, "", "");
        //		return null;
        //	}
        //}

        /// <summary>
        /// Validate a Item. This would be the place to put field validations.
        ///
        /// </summary>
        /// <param name="updates"></param>
        public void CheckIsValidData(ItemTaxData updates)
        {
            ItemTaxTable table;

            ItemTaxRow oRow;

            oRow = (ItemTaxRow)updates.Tables[0].Rows[0];

            table = (ItemTaxTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (ItemTaxTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if (updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable(updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (ItemTaxTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if (updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable(updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (ItemTaxRow row in table.Rows)
            {
                if (row.ID == -1)
                    ErrorHandler.throwCustomError(POSErrorENUM.Item_CodeCanNotBeNULL);
            }
        }

        public ItemTaxData Populate(string itemId, EntityType entityType)
        {
            try
            {
                using (var dao = new ItemTaxSvr())
                {
                    return dao.Populate(itemId, entityType == EntityType.Department ? Department : Item);
                }
            }
            catch (POSExceptions)
            {
                throw;
            }

            catch (OtherExceptions)
            {
                throw;
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(string itemId, EntityType entityType)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                     //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public virtual TaxCodesData PopulateTaxCodeData(string itemId, EntityType entityType, bool bItem = true)    //PRIMEPOS-3037 13-Dec-2021 JY Added bItem
        {
            try
            {
                ItemTaxData oItemTaxData = new ItemTaxData();
                string sTaxIds = string.Empty;
                if (bItem)
                {
                    using (var dao = new ItemTaxSvr())
                    {
                        oItemTaxData = dao.Populate(itemId, entityType == EntityType.Department ? Department : Item);
                        if (Configuration.isNullOrEmptyDataSet(oItemTaxData) == true)
                        {
                            return new TaxCodesData();
                        }
                        foreach (ItemTaxRow oRow in oItemTaxData.ItemTaxTable.Rows)
                        {
                            sTaxIds += oRow.TaxID + ",";
                        }
                    }
                    if (sTaxIds.EndsWith(","))
                    {
                        sTaxIds = sTaxIds.Substring(0, sTaxIds.Length - 1);
                    }
                }
                #region PRIMEPOS-3037 13-Dec-2021 JY Added
                else
                {
                    string[] arrTaxCodes = itemId.Split(',');
                    if (arrTaxCodes.Length > 0)
                    {
                        for (int i = 0; i < arrTaxCodes.Length; i++)
                        {
                            if (sTaxIds == "")
                                sTaxIds = "'" + arrTaxCodes[i].Trim() + "'";
                            else
                                sTaxIds += ",'" + arrTaxCodes[i].Trim() + "'";
                        }
                    }
                }
                #endregion

                using (var dao = new TaxCodesSvr())
                {
                    if (bItem)
                        return dao.PopulateList(" WHERE " + clsPOSDBConstants.TaxCodes_Fld_TaxID + " IN (" + sTaxIds + ")");
                    else
                        return dao.PopulateList(" WHERE " + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " IN (" + sTaxIds.ToUpper() + ")");  //PRIMEPOS-3037 13-Dec-2021 JY Added
                }
            }
            catch (POSExceptions)
            {
                throw;
            }

            catch (OtherExceptions)
            {
                throw;
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateTaxCodeData(string itemId, EntityType entityType)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public virtual TaxCodesData PopulateTaxCodeUsedInTrans(string sItemId, int iTransId)
        {
            try
            {
                ItemTaxData oItemTaxData = new ItemTaxData();
                string sTaxIds = string.Empty;

                using (var dao = new TaxCodesSvr())
                {
                    return dao.PopulateTaxCodeUsedInTrans(sItemId, iTransId);
                }

            }
            catch (POSExceptions)
            {
                throw;
            }

            catch (OtherExceptions)
            {
                throw;
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateTaxCodeUsedInTrans(string sItemId, int iTransId)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                               //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }

    public enum EntityType
    {
        Department,
        Item
    }
}