// ----------------------------------------------------------------
// Library: Business Tier
//  Author: Adeel Shehzad.
// Company: D-P-S. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS.Resources;

using Resources;
using POS_Core.Resources;

namespace POS_Core.BusinessRules
{


    /// <summary>
    /// This represent item entity business rules.
    /// </summary>
    public class Item : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// Insert or update item data.
        /// </summary>
        /// <param name="updates">Item type dataset.</param>
        public void Persist(ItemData updates)
        {
            try
            {
                using (IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))    //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    IDbTransaction tx = oConn.BeginTransaction();
                    Persist(updates, tx);
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
                ErrorHandler.throwException(ex, "", "");
            }
        }

        /// <summary>
        /// Insert or update data by checking user priviliges. 
        /// This insertion or updation takes place under Transactions.
        /// </summary>
        /// <param name="updates">Item type dataset.</param>
        /// <param name="tx">This insertion or updation will be the part of this transaction.</param>
        public void Persist(ItemData updates, IDbTransaction tx)
        {
            try
            {
                //UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998);    //PRIMEPOS-3125 22-Dec-2022 JY Commented
                checkIsValidData(updates);
                using (ItemSvr dao = new ItemSvr())
                {
                    dao.Persist(updates, tx);
                    tx.Commit();
                }
            }
            catch (POSExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                tx.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            try
            {
                using (ItemSvr oItemSvr = new ItemSvr())
                {
                    return oItemSvr.DeleteRow(CurrentID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #region PRIMEPOS-3125 22-Dec-2022 JY Modified
        /// <summary>
        /// Update selling price of items
        /// </summary>
        /// <param name="dtItemSellingPriceType">ItemSellingPriceType</param>
        public void UpdateBulkSellingPrice(DataTable dtItemSellingPriceType)
        {
            try
            {
                using (IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    IDbTransaction tx = oConn.BeginTransaction();
                    UpdateBulkSellingPrice(dtItemSellingPriceType, tx);
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
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public void UpdateBulkSellingPrice(DataTable dtItemSellingPriceType, IDbTransaction tx)
        {
            try
            {
                using (ItemSvr oItemSvr = new ItemSvr())
                {
                    oItemSvr.UpdateBulkSellingPrice(dtItemSellingPriceType, tx);
                    tx.Commit();
                }
            }
            catch (POSExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                tx.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #endregion

        #region Get Methods
        /// <summary>
        /// Fills a DataSet with all Items based on a condition.
        /// </summary>
        /// <param name="whereClause">SQL WHERE CLUASE type condition.</param>
        /// <returns>returns Typed DataSet (ItemData).</returns>
        public ItemData PopulateList(string whereClause)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    return dao.PopulateList(whereClause);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetCLExcludedItemData()
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.PopulateListWithIdName(" where ExcludeFromCL=1 ");
            }
        }

        public DataSet GetCLCouponExcludedItemData()
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.PopulateListWithIdName(" where ExcludeFromCL=1 and EXCLUDEFROMCLCouponPay=1 ");
            }
        }

        public DataSet GetItemPriceLog(string sItemCode)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.GetItemPriceLog(sItemCode);
            }
        }

        public bool IsIIASItem(string sItemCode)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.IsIIASItem(sItemCode);
            }
        }

        public bool IsEBTItem(string sItemCode)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.IsEBTItem(sItemCode);
            }
        }

        public bool IsItemOnSale(ItemRow oItemRow,DepartmentRow oDepartmentRow)
        {
            bool returnValue = false;
            if (oItemRow.isOnSale == true)
            {
                if (oItemRow.SaleStartDate != DBNull.Value || oItemRow.SaleEndDate != DBNull.Value)
                    if (DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                    {
                        returnValue = true;
                    }
            }
            else
            {
                try
                {
                    if (oDepartmentRow != null)
                    {
                        if (DateTime.Now.Date >= Convert.ToDateTime(oDepartmentRow.SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDepartmentRow.SaleEndDate))
                        {
                            if (oDepartmentRow.SalePrice > 0)
                            {
                                #region Sprint-26 - PRIMEPOS-2412 29-Aug-2017 JY Added logic to consider subdepartment                                            
                                if (oItemRow.SubDepartmentID != 0)
                                {                                    
                                    SubDepartmentSvr oSubDepartmentSvr = new SubDepartmentSvr();
                                    SubDepartmentData oSubDepartmentData = oSubDepartmentSvr.Populate(oItemRow.DepartmentID);

                                    if (oSubDepartmentData != null && oSubDepartmentData.Tables.Count > 0 && oSubDepartmentData.Tables[0].Rows.Count > 0)
                                    {
                                        if (Configuration.convertNullToBoolean(oSubDepartmentData.SubDepartment[0].IncludeOnSale))
                                        {
                                            returnValue = true;
                                        }
                                        else
                                        {
                                            returnValue = false;
                                        }
                                    }
                                }
                                #endregion
                                returnValue = true;
                            }
                        }
                    }
                }
                catch { }
            }
            return returnValue;
        }

        public int GetFineLineCode(string sItemCode)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.GetFineLineCode(sItemCode);
            }
        }

        public DataTable GetIIAS_Category_Descriptor()
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.GetIIAS_Category_Descriptor();
            }
        }

        public DataTable GetIIAS_SubCategory_Descriptor()
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.GetIIAS_SubCategory_Descriptor();
            }
        }

        public DataSet GetPacketInfo(string itemCode)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.GetPacketInfo(itemCode);
            }
        }

        public string GetItemNames(string codes)
        {
            string returnValue = "";
            try
            {
                SearchSvr oSearch = new SearchSvr();

                string[] codesArray= codes.Split(',');

                for (int i = 0; i < codesArray.Length; i = i + 1000)
                {
                    string codesToSearch = string.Empty;
                    for (int j = i; j < i + 1000; j++)
                    {
                        if (j == codesArray.Length)
                        {
                            break;
                        }
                        else
                        {
                            codesToSearch+="," + codesArray[j];
                        }
                    }

                    if (codesToSearch.StartsWith(","))
                    {
                        codesToSearch = codesToSearch.Substring(1);
                    }

                    DataSet oData = oSearch.Search(" select description from item where itemID in (" + codesToSearch + ")");

                    foreach (System.Data.DataRow oRow in oData.Tables[0].Rows)
                    {
                        returnValue += "," + oRow[0].ToString();
                    }
                }

                if (returnValue.StartsWith(","))
                {
                    returnValue = returnValue.Substring(1);
                }
            } 
            catch (Exception exp)
            {
                returnValue = "";
            }
            return returnValue;
        }
        
        public bool IsDiscountable(string sItemCode)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.IsDiscountable(sItemCode);
            }
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validate a Item. This would be the place to put field validations.
        /// 
        /// </summary>
        /// <param name="updates"></param>
        public void checkIsValidData(ItemData updates)
        {
            ItemTable table;

            ItemRow oRow;

            oRow = (ItemRow)updates.Tables[0].Rows[0];

            table = (ItemTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (ItemTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((ItemTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((ItemTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (ItemTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((ItemTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((ItemTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (ItemRow row in table.Rows)
            {
                if (row.ItemID.Trim() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Item_CodeCanNotBeNULL);
            }
        }

        public virtual ItemData Populate(System.String ItemID)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    return dao.Populate(ItemID);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        //added to populate data for Auto update price 
        public virtual ItemData Populate(System.String ItemID, System.String action,System.Boolean isPrimePO)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    return dao.Populate(ItemID,action,isPrimePO);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        //Populate by vendor item code.
        public virtual ItemData Populate(System.String VendorItemCode,System.String VendorId)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    return dao.Populate(VendorItemCode,VendorId);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        public virtual ItemRow FindItemBySKUCode(System.String SKUCode)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    ItemData oItemData= dao.FindItemBySKUCode(SKUCode);
                    if (oItemData.Tables[0].Rows.Count > 0)
                    {
                        return oItemData.Item[0];
                    }
                    else
                    {
                        return null;
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public virtual ItemRow FindItemByID(System.String itemID)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    ItemData oItemData = dao.FindItemByID(itemID);
                    if (oItemData.Tables[0].Rows.Count > 0)
                    {
                        return oItemData.Item[0];
                    }
                    else
                    {
                        return null;
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public void checkIsValidPrimaryKey(ItemData updates)
        {
            ItemTable table = (ItemTable)updates.Tables[clsPOSDBConstants.Item_tbl];
            foreach (ItemRow row in table.Rows)
            {
                if (this.Populate(row.ItemID).Tables[clsPOSDBConstants.Item_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for Item ");
                }
            }
        }

        // Check whether an attempted delete is valid for Item
        public void checkIsValidDelete(ItemData updates)
        {
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public DataTable GetRecordFromItemPriceHistory(System.String ItemID, System.String ChangedIn)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.GetRecordFromItemPriceHistory(ItemID, ChangedIn);
            }
        }

        //Sprint-25 - PRIMEPOS-2380 Added logic to check Item exists in PSE_Items table
        public DataTable IsPSEItemData(string sItemCode)
        {
            using (ItemSvr dao = new ItemSvr())
            {
                return dao.IsPSEItemData(sItemCode);
            }
        }

        #region PRIMEPOS-2395 22-Jun-2018 JY Added logic to get item details with ItemVendorId and LastInvUpdatedQty
        public virtual DataTable GetItemDetails(System.String ItemID)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    return dao.GetItemDetails(ItemID);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public virtual DataTable GetItemDetailsWithVendor(System.String ItemID, System.String vendorCode)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    return dao.GetItemDetailsWithVendor(ItemID, vendorCode);
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-2464 05-Mar-2020 JY Added
        public virtual void UpdateDisplayItemCost(int ModuleID, int ScreenID, int PermissionID, bool isAllowed)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    dao.UpdateDisplayItemCost(ModuleID, ScreenID, PermissionID, isAllowed);
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
                //return null;
            }
        }
        #endregion

        #region PRIMEPOS-1633 31-Dec-2020 JY Added
        public void UpdateItemTax(string strItemIds, string strTaxIds)
        {
            try
            {
                using (ItemSvr dao = new ItemSvr())
                {
                    dao.UpdateItemTax(strItemIds, strTaxIds);
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
                //return null;
            }
        }
        #endregion
    }
}
