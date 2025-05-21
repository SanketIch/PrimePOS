// ----------------------------------------------------------------
// Library: Business Tier
//  Author: Adeel Shehzad.
// Company: D-P-S. (www.d-p-s.com)
//
// ----------------------------------------------------------------

namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;
    using NLog;

    // ItemPriceValidations Business Rules Class  

    public class ItemPriceValidation : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // A method for inserting and updating ItemPriceValidation data.

        public void Persist(ItemPriceValidationData updates)
        {

            try
            {

                checkIsValidData(updates);
                using (ItemPriceValidationSvr dao = new ItemPriceValidationSvr())
                {
                    dao.Persist(updates);
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
                logger.Fatal(ex, "Persist(ItemPriceValidationData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }
        #endregion

        #region Get Methods

        // Fills a DataSet with all ItemPriceValidations based on a condition.
        public ItemPriceValidationData PopulateList(string whereClause)
        {
            try
            {
                using (ItemPriceValidationSvr dao = new ItemPriceValidationSvr())
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
                logger.Fatal(ex, "PopulateList(string whereClause)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        #endregion



        #region Validation Methods
        // Validate a ItemPriceValidation.  This would be the place to put field validations.
        public void checkIsValidData(ItemPriceValidationData updates)
        {
            ItemPriceValidationTable table;

            ItemPriceValidationRow oRow;

            oRow = (ItemPriceValidationRow)updates.Tables[0].Rows[0];

            table = (ItemPriceValidationTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (ItemPriceValidationTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((ItemPriceValidationTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((ItemPriceValidationTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (ItemPriceValidationTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((ItemPriceValidationTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((ItemPriceValidationTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;
        }

        public ItemPriceValidationData PopulateByItem(System.String ItemID)
        {
            try
            {
                using (ItemPriceValidationSvr dao = new ItemPriceValidationSvr())
                {
                    return dao.PopulateByItem(ItemID);
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
                logger.Fatal(ex, "PopulateByItem(System.String ItemID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public ItemPriceValidationData PopulateByDept(System.Int32 DeptID)
        {
            try
            {
                using (ItemPriceValidationSvr dao = new ItemPriceValidationSvr())
                {
                    return dao.PopulateByDept(DeptID);
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
                logger.Fatal(ex, "PopulateByDept(System.Int32 DeptID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        // Check whether an attempted delete is valid for ItemPriceValidation
        public void checkIsValidDelete(ItemPriceValidationData updates)
        {
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public bool ValidateItem(string sItemID, decimal dSellingPrice)
        {
            bool retVal = true;
            if (Resources.Configuration.CPOSSet.ApplyPriceValidation == true)
            {
                Item oItem = new Item();
                ItemData oItemData = oItem.Populate(sItemID);
                ItemPriceValidationData oItemPriceValidData = this.PopulateByItem(sItemID);
                if (oItemPriceValidData.Tables[0].Rows.Count > 0)
                {
                    retVal = ValidatePrice(oItemData.Item[0], dSellingPrice, oItemPriceValidData.ItemPriceValidation[0]);
                }
                else
                {
                    if (oItemData.Item[0].DepartmentID > 0)
                    {
                        oItemPriceValidData = this.PopulateByDept(oItemData.Item[0].DepartmentID);
                        if (oItemPriceValidData.Tables[0].Rows.Count > 0)
                        {
                            retVal = ValidatePrice(oItemData.Item[0], dSellingPrice, oItemPriceValidData.ItemPriceValidation[0]);
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Author:Shitaljit 3/7/2013
        /// FOr price validation during transaction
        /// </summary>
        /// <param name="sItemID"></param>
        /// <param name="dSellingPrice"></param>
        /// <param name="dMinSellingAmount"></param>
        /// <returns></returns>
        public bool ValidateItem(string sItemID, decimal dSellingPrice, out decimal dMinSellingAmount)
        {
            bool retVal = true;
            dMinSellingAmount = 0;
            if (Resources.Configuration.CPOSSet.ApplyPriceValidation == true)
            {
                Item oItem = new Item();
                ItemData oItemData = oItem.Populate(sItemID);
                ItemPriceValidationData oItemPriceValidData = this.PopulateByItem(sItemID);
                if (oItemPriceValidData.Tables[0].Rows.Count > 0)
                {
                    retVal = ValidatePrice(oItemData.Item[0], dSellingPrice, oItemPriceValidData.ItemPriceValidation[0], out dMinSellingAmount);
                }
                else
                {
                    if (oItemData.Item[0].DepartmentID > 0)
                    {
                        oItemPriceValidData = this.PopulateByDept(oItemData.Item[0].DepartmentID);
                        if (oItemPriceValidData.Tables[0].Rows.Count > 0)
                        {
                            retVal = ValidatePrice(oItemData.Item[0], dSellingPrice, oItemPriceValidData.ItemPriceValidation[0], out dMinSellingAmount);
                        }
                    }
                }
            }
            return retVal;
        }

        public bool ValidateItem(ItemRow oItemRow, decimal dSellingPrice)
        {
            bool retVal = true;
            if (Resources.Configuration.CPOSSet.ApplyPriceValidation == true && oItemRow.ItemID != "RX")
            {
                Item oItem = new Item();
                ItemPriceValidationData oItemPriceValidData = this.PopulateByItem(oItemRow.ItemID);
                if (oItemPriceValidData.Tables[0].Rows.Count > 0)
                {
                    retVal = ValidatePrice(oItemRow, dSellingPrice, oItemPriceValidData.ItemPriceValidation[0]);
                }
                else
                {
                    if (oItemRow.DepartmentID > 0)
                    {
                        oItemPriceValidData = this.PopulateByDept(oItemRow.DepartmentID);
                        if (oItemPriceValidData.Tables[0].Rows.Count > 0)
                        {
                            retVal = ValidatePrice(oItemRow, dSellingPrice, oItemPriceValidData.ItemPriceValidation[0]);
                        }
                    }
                }
            }
            return retVal;
        }

        public bool ValidateItem(string sItemID, ItemPriceValidationRow oItemPriceValidRow)
        {
            bool retVal = true;
            if (Resources.Configuration.CPOSSet.ApplyPriceValidation == true)
            {
                Item oItem = new Item();
                ItemData oItemData = oItem.Populate(sItemID);
                ItemPriceValidationData oItemPriceValidData = this.PopulateByItem(sItemID);
                if (oItemPriceValidData.Tables[0].Rows.Count > 0)
                {
                    retVal = ValidatePrice(oItemData.Item[0], oItemData.Item[0].SellingPrice, oItemPriceValidRow);
                }

            }
            return retVal;
        }

        public bool ValidateDept(int iDeptID, ItemPriceValidationRow oItemPriceValidRow)
        {
            bool retVal = true;
            if (Resources.Configuration.CPOSSet.ApplyPriceValidation == true)
            {
                Item oItem = new Item();
                ItemData oItemData = oItem.PopulateList(" where departmentid=" + iDeptID.ToString());

                if (oItemData.Tables[0].Rows.Count > 0)
                {
                    foreach (ItemRow oItemRow in oItemData.Item.Rows)
                    {
                        retVal = ValidatePrice(oItemRow, oItemRow.SellingPrice, oItemPriceValidRow);
                        if (retVal == false)
                        {
                            break;
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Author:Shitaljit on 3/7/2013
        /// To validate item price durin gtransaction.
        /// </summary>
        /// <param name="oItemRow"></param>
        /// <param name="dSellingPrice"></param>
        /// <param name="oItemPriceValidRow"></param>
        /// <param name="dMinSellingAmount"></param>
        /// <returns></returns>
        private bool ValidatePrice(ItemRow oItemRow, decimal dSellingPrice, ItemPriceValidationRow oItemPriceValidRow, out Decimal dMinSellingAmount)
        {
            bool retVal = true;
            dMinSellingAmount = 0;
            if (oItemPriceValidRow.IsActive == true)
            {
                if (oItemPriceValidRow.AllowNegative == false && dSellingPrice < 0)
                {
                    dMinSellingAmount = 0;
                    retVal = false;
                }
                else
                {
                    if (oItemPriceValidRow.MinSellingAmount > 0)
                    {
                        if (dSellingPrice < oItemPriceValidRow.MinSellingAmount)
                        {
                            dMinSellingAmount = oItemPriceValidRow.MinSellingAmount;
                            retVal = false;
                        }
                    }
                    else if (oItemPriceValidRow.MinSellingPercentage > 0)
                    {
                        if (oItemRow.SellingPrice - (oItemPriceValidRow.MinSellingPercentage / 100 * oItemRow.SellingPrice) > dSellingPrice)
                        {
                            dMinSellingAmount = oItemRow.SellingPrice - (oItemPriceValidRow.MinSellingPercentage / 100 * oItemRow.SellingPrice);
                            retVal = false;
                        }
                    }
                    else if (oItemPriceValidRow.MinCostPercentage > 0)
                    {
                        if (oItemRow.LastCostPrice + (oItemPriceValidRow.MinCostPercentage / 100 * oItemRow.LastCostPrice) > dSellingPrice)
                        {
                            dMinSellingAmount = oItemRow.LastCostPrice + (oItemPriceValidRow.MinCostPercentage / 100 * oItemRow.LastCostPrice);
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        private bool ValidatePrice(ItemRow oItemRow, decimal dSellingPrice, ItemPriceValidationRow oItemPriceValidRow)
        {
            bool retVal = true;
            if (oItemPriceValidRow.IsActive == true)
            {
                if (oItemPriceValidRow.AllowNegative == false && dSellingPrice < 0)
                {
                    retVal = false;
                }
                else
                {
                    if (oItemPriceValidRow.MinSellingAmount > 0)
                    {
                        if (dSellingPrice < oItemPriceValidRow.MinSellingAmount)
                        {
                            retVal = false;
                        }
                    }
                    else if (oItemPriceValidRow.MinSellingPercentage > 0)
                    {
                        if (oItemRow.SellingPrice - (oItemPriceValidRow.MinSellingPercentage / 100 * oItemRow.SellingPrice) > dSellingPrice)
                        {
                            retVal = false;
                        }
                    }
                    else if (oItemPriceValidRow.MinCostPercentage > 0)
                    {
                        if (oItemRow.LastCostPrice + (oItemPriceValidRow.MinCostPercentage / 100 * oItemRow.LastCostPrice) > dSellingPrice)
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }

    }
}
