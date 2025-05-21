using MMS.Device;
using MMSChargeAccount;
using NLog;
using PharmData;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.DataAccess;
//using POS.Reports.ReportsUI;
//using POS.Resources;
////using POS.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using POS_Core.TaxType;
using System.Text;
using System.Threading.Tasks;
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core.Resources.PaymentHandler;
using POS_Core.Resources.DelegateHandler;
using POS_Core.TransType;
using POS_Core.LabelHandler.RxLabel;
using System.Globalization;
using PossqlData;

namespace POS_Core.BusinessRules
{
    public partial class POSTransaction
    {
        public string controlNumber = string.Empty;//PRIMEPOS-2664
        public void ProcessItemsForComboPricing(TransDetailTable transDetail, TransDetailRow currentRow, bool isDeleted, TransDetailTaxData oTDTaxData)
        {
            if (Configuration.CInfo.AllowItemComboPrice == false) return;

            ItemComboPricingDetailSvr comboDetailSvr = new ItemComboPricingDetailSvr();
            List<KeyValuePair<Int32, String>> comboList = comboDetailSvr.GetComboPricingList();
            Int32 comboId = (from cb in comboList where cb.Value.Trim().ToUpper() == currentRow.ItemID.Trim().ToUpper() select cb.Key).FirstOrDefault<Int32>();  //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added Trim().ToUpper() to resove issue with the item case
            if (comboId == 0) return;

            ItemComboPricingDetailTable comboDetailTable = comboDetailSvr.Populate(comboId).ItemComboPricingDetail;
            //List<KeyValuePair<Int32, String>> selectedCombo = (from cb in comboList where cb.Key== comboId select cb).ToList();
            //List<String> selectedItems=(from x in selectedCombo select x.Value).ToList<String>();
            //selectedCombo.OrderBy(o => o.Key);

            if (comboDetailTable.Count > 0)
            {
                List<KeyValuePair<TransDetailRow, ItemComboPricingDetailRow>> rows = new List<KeyValuePair<TransDetailRow, ItemComboPricingDetailRow>>();

                #region Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Commented
                //foreach (ItemComboPricingDetailRow comboDetailRow in comboDetailTable)
                //{
                //    TransDetailRow filteredRow = (from t in transDetail.AsEnumerable() where t.Field<String>("itemId").Trim().ToUpper() == comboDetailRow.ItemID.Trim().ToUpper() select t).FirstOrDefault<TransDetailRow>();
                //    if (filteredRow != null && filteredRow.RowState != DataRowState.Deleted)
                //    {
                //        rows.Add(new KeyValuePair<TransDetailRow, ItemComboPricingDetailRow>(filteredRow, comboDetailRow));
                //        ComboItemQtyInTrans += filteredRow.QTY; //Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Added
                //    }
                //}
                #endregion

                #region Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Added
                int ComboItemQtyInTrans = 0;    //Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Added logic to calculate combo items count
                foreach (TransDetailRow filteredRow in transDetail)
                {
                    if (filteredRow.RowState != DataRowState.Deleted)
                    {
                        ItemComboPricingDetailRow comboDetailRow = (from t in comboDetailTable.AsEnumerable() where t.Field<String>("ItemID").Trim().ToUpper() == filteredRow.ItemID.Trim().ToUpper() select t).FirstOrDefault<ItemComboPricingDetailRow>();
                        if (comboDetailRow != null && comboDetailRow.RowState != DataRowState.Deleted)
                        {
                            rows.Add(new KeyValuePair<TransDetailRow, ItemComboPricingDetailRow>(filteredRow, comboDetailRow));
                            ComboItemQtyInTrans += filteredRow.QTY;
                        }
                    }
                }
                #endregion

                ItemComboPricingSvr comboSvr = new ItemComboPricingSvr();
                ItemComboPricingRow comboRow = comboSvr.Populate(comboId).ItemComboPricing[0];
                string sTaxIDs = string.Empty;
                //if (rows.Count < comboRow.MinComboItems)  //Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Commented
                if (ComboItemQtyInTrans < comboRow.MinComboItems)    //Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Added
                {
                    ItemSvr itemSvr = new ItemSvr();
                    foreach (KeyValuePair<TransDetailRow, ItemComboPricingDetailRow> row in rows)
                    {
                        sTaxIDs = string.Empty;
                        if (row.Key.ItemComboPricingID > 0)
                        {
                            ItemRow itemRow = itemSvr.FindItemByID(row.Key.ItemID).Item[0];
                            row.Key.Price = row.Key.NonComboUnitPrice;
                            RecaclulateTransDetailItem(row.Key, itemRow, oTDTaxData);
                            //Added By shitaljit to resolved Tax not calcualting properly while applying original price back to items.
                            if (IsItemTaxableForTrasaction(oTDTaxData, row.Key.ItemID, out sTaxIDs, row.Key.TransDetailID)) //PRIMEPOS-3230
                            {
                                string tempItedId = row.Key.ItemID;
                                TaxCodes oTaxCodes = new TaxCodes();
                                TaxCodesData oTaxCodesData;
                                oTaxCodesData = oTaxCodes.PopulateList(" WHERE TaxID IN " + sTaxIDs);
                                CalculateTax(row.Key, oTaxCodesData, oTDTaxData);
                                decimal taxAmt = row.Key.TaxAmount;
                            }
                            row.Key.ItemComboPricingID = 0;
                            row.Key.IsComboItem = false;
                        }
                    }
                }
                //else if (rows.Count >= comboRow.MinComboItems)
                else if (ComboItemQtyInTrans >= comboRow.MinComboItems && ComboItemQtyInTrans <= comboRow.MaxComboItems) //Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Added
                {
                    foreach (KeyValuePair<TransDetailRow, ItemComboPricingDetailRow> row in rows)
                    {
                        if (comboRow.ForceGroupPricing == true)
                        {
                            row.Key.Price = comboRow.ComboItemPrice;
                        }
                        else
                        {
                            row.Key.Price = row.Value.SalePrice;
                        }
                        CalcExtdPrice(row.Key);
                        row.Key.ItemComboPricingID = comboRow.Id;
                        row.Key.Discount = 0;
                        //Added By shitaljit to resolved Tax not calcualting properly while applying combo price
                        //Added By shitaljit to resolved Tax not calcualting properly while applying original price back to items.
                        if (IsItemTaxableForTrasaction(oTDTaxData, row.Key.ItemID, out sTaxIDs, row.Key.TransDetailID)) //PRIMEPOS-3230
                        {
                            string tempItedId = row.Key.ItemID;
                            TaxCodes oTaxCodes = new TaxCodes();
                            TaxCodesData oTaxCodesData;
                            oTaxCodesData = oTaxCodes.PopulateList(" WHERE TaxID IN " + sTaxIDs);
                            CalculateTax(row.Key, oTaxCodesData, oTDTaxData);
                            decimal taxAmt = row.Key.TaxAmount;
                        }
                        row.Key.IsPriceChanged = true;
                        row.Key.IsComboItem = true;
                        row.Key.IsPriceChangedByOverride = false;   //Sprint-26 - PRIMEPOS-2294 28-Jul-2017 JY Added
                    }
                }
                #region Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Added
                else if (ComboItemQtyInTrans > comboRow.MaxComboItems)
                {
                    int eligibleComboItemQty = comboRow.MaxComboItems;
                    foreach (KeyValuePair<TransDetailRow, ItemComboPricingDetailRow> row in rows)
                    {
                        if (eligibleComboItemQty > 0)
                        {
                            if (comboRow.ForceGroupPricing == true)
                            {
                                row.Key.Price = comboRow.ComboItemPrice;
                            }
                            else
                            {
                                row.Key.Price = row.Value.SalePrice;
                            }
                            CalcExtdPrice(row.Key, ComboItemQtyInTrans, eligibleComboItemQty);
                            eligibleComboItemQty -= Convert.ToInt32(row.Key.QTY);
                            row.Key.ItemComboPricingID = comboRow.Id;
                            row.Key.Discount = 0;
                            row.Key.IsPriceChanged = true;
                            row.Key.IsComboItem = true;
                            row.Key.IsPriceChangedByOverride = false;   //Sprint-26 - PRIMEPOS-2294 28-Jul-2017 JY Added
                        }
                        else
                        {
                            row.Key.Price = row.Key.OrignalPrice;
                            row.Key.ItemComboPricingID = 0;
                            row.Key.IsPriceChanged = false;
                            row.Key.IsComboItem = false;
                            row.Key.Discount = CalculateDiscount(row.Key.ItemID, row.Key.QTY, row.Key.Price, row.Key.IsPriceChanged);
                        }

                        if (IsItemTaxableForTrasaction(oTDTaxData, row.Key.ItemID, out sTaxIDs, row.Key.TransDetailID)) //PRIMEPOS-3230
                        {
                            string tempItedId = row.Key.ItemID;
                            TaxCodes oTaxCodes = new TaxCodes();
                            TaxCodesData oTaxCodesData;
                            oTaxCodesData = oTaxCodes.PopulateList(" WHERE TaxID IN " + sTaxIDs);
                            CalculateTax(row.Key, oTaxCodesData, oTDTaxData);
                            decimal taxAmt = row.Key.TaxAmount;
                        }
                    }
                }
                #endregion
            }
        }

        #region Sprint-26 - PRIMEPOS-1857 18-Jul-2017 JY Added
        public void CalcExtdPrice(TransDetailRow oRow, int ComboItemQtyInTrans, int eligibleComboItemQty)
        {
            try
            {
                if (oRow.ItemID.Trim() == "")
                    oRow.ExtendedPrice = 0;
                else
                {
                    if (Convert.ToDecimal(oRow.QTY) < eligibleComboItemQty)
                    {
                        oRow.ExtendedPrice = Convert.ToInt32(oRow.QTY) * Convert.ToDecimal(oRow.Price);
                    }
                    else
                    {
                        oRow.ExtendedPrice = (eligibleComboItemQty * Convert.ToDecimal(oRow.Price)) + ((Convert.ToInt32(oRow.QTY) - eligibleComboItemQty) * Convert.ToDecimal(oRow.OrignalPrice));
                        oRow.Price = oRow.ExtendedPrice / oRow.QTY;
                    }
                }
            }
            catch (Exception exp)
            {
                return;
            }
        }
        #endregion

        #region Sprint-27 - PRIMEPOS-2413 19-Sep-2017 JY Added 
        public void ProcessItemsForSalePrice(TransDetailTable transDetail, TransDetailRow currentRow, bool isDeleted, TransDetailTaxData oTDTaxData)
        {
            Item oItem = new Item();
            ItemData oItemData;
            ItemRow oItemRow = null;
            oItemData = oItem.Populate(currentRow.ItemID);
            if (oItemData.Item.Rows.Count > 0)
            {
                oItemRow = oItemData.Item[0];
                if (oItemData.Item.Rows.Count > 0 && oItemRow.isOnSale && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                {
                    int iSalelimitQty = Configuration.convertNullToInt(oItemRow.SaleLimitQty);
                    int ProcessedQty = 0;
                    foreach (TransDetailRow oRow in transDetail.Rows)
                    {
                        if (currentRow.ItemID == oRow.ItemID)
                        {
                            if (ProcessedQty + oRow.QTY <= iSalelimitQty || iSalelimitQty == 0)
                            {
                                oRow.NonComboUnitPrice = oRow.Price = Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero);
                                oRow.ExtendedPrice = oRow.QTY * oRow.Price;
                                oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                            }
                            else
                            {
                                oTDRow.IsOnSale = false; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                if (ProcessedQty < iSalelimitQty)
                                {
                                    //we need to apply sale price for few quantity
                                    //decimal TempPrice = Math.Round(((((iSalelimitQty - ProcessedQty) * Math.Round(oItemRow.OnSalePrice,2, MidpointRounding.AwayFromZero) + ((oRow.QTY - (iSalelimitQty - ProcessedQty)) * oRow.OrignalPrice))) / oRow.QTY), 2, MidpointRounding.AwayFromZero);
                                    decimal TempPrice = (((iSalelimitQty - ProcessedQty) * Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero) + ((oRow.QTY - (iSalelimitQty - ProcessedQty)) * oRow.OrignalPrice))) / oRow.QTY; //Sprint-27 - PRIMEPOS-2413 07-Nov-2017 JY Added to get exact price of item
                                    oRow.NonComboUnitPrice = oRow.Price = TempPrice;
                                    oRow.ExtendedPrice = oRow.QTY * oRow.Price;
                                }
                                else
                                {
                                    oRow.NonComboUnitPrice = oRow.Price = oRow.OrignalPrice;
                                    oRow.ExtendedPrice = oRow.QTY * oRow.Price;
                                }
                            }
                            ProcessedQty += oRow.QTY;
                            string sTaxIDs = string.Empty;
                            if (IsItemTaxableForTrasaction(oTDTaxData, oRow.ItemID, out sTaxIDs, oRow.TransDetailID))
                            {
                                string tempItedId = oRow.ItemID;
                                TaxCodes oTaxCodes = new TaxCodes();
                                TaxCodesData oTaxCodesData;
                                oTaxCodesData = oTaxCodes.PopulateList(" WHERE TaxID IN " + sTaxIDs);
                                CalculateTax(oRow, oTaxCodesData, oTDTaxData);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public decimal CalculateDiscount(string itemID, int QTY, decimal price, bool IsPriceChanged)
        {
            logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Entering);
            decimal returnValue = 0;
            bool isDeptSale = false;
            Item oItem = new Item();
            ItemData oItemData;
            ItemRow oItemRow = null;
            oItemData = oItem.Populate(itemID);
            if (oItemData.Item.Rows.Count > 0)
            {
                oItemRow = oItemData.Item[0];
                string strDiscountPolicy = oItemRow.DiscountPolicy;
                //if Item is on sale dont give any other discount
                //if (oItemRow.isOnSale == true)
                if (oItemRow.isOnSale == true && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value
                    && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)  //PRIMEPOS-3138 01-Sep-2022 JY modified if condition
                {
                    //To not apply discount on sale item if AllowDiscountOfItemsOnSale is false
                    if (Configuration.CInfo.AllowDiscountOfItemsOnSale == false)
                    {
                        if (IsPriceChanged)
                        {
                            logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "1");
                            return returnValue;
                        }
                    }
                    if (oItemRow.SaleStartDate != DBNull.Value || oItemRow.SaleEndDate != DBNull.Value)
                        if (DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                        {
                            if (IsPriceChanged)
                            {
                                returnValue = this.GetDiscount(QTY * Convert.ToDecimal((oItemRow.Discount / 100 * price))); //Added logic to apply disc for sale item
                                logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "2");
                                return returnValue;
                            }
                        }
                }

                if (oItemRow.isDiscountable == false)
                {
                    return 0;
                }
                bool isDeptDiscount = false;

                DepartmentData oDeptData = new DepartmentData();
                if (oItemRow.DepartmentID != 0)
                {
                    Department oDept = new Department();
                    oDeptData = new DepartmentData();
                    oDeptData = oDept.Populate(oItemRow.DepartmentID);

                    if (oDeptData.Tables[0].Rows.Count > 0 && DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate))
                    {
                        if (oItemRow.SubDepartmentID == 0)
                        {
                            isDeptSale = true;
                        }
                        //Added to check whether the sub department is on sale or not
                        //JIRA#-PRIMEPOS-1038  Ability to choose which sub-dept sale to apply
                        else
                        {
                            SubDepartment oSubDepartment = new SubDepartment();
                            SubDepartmentData oSubDepartmentData = new SubDepartmentData();
                            int deptID = oDeptData.Department[0].DeptID;
                            oSubDepartmentData = oSubDepartment.PopulateList(" WHERE IncludeOnSale = 1 AND SubDepartment." + clsPOSDBConstants.SubDepartment_Fld_DepartmentID + " = " + deptID + " AND SubDepartmentID = " + oItemRow.SubDepartmentID);
                            if (Configuration.isNullOrEmptyDataSet(oSubDepartmentData) == false)
                            {
                                isDeptSale = true;
                            }
                        }
                    }
                }

                ApplyDiscPolicy(oItemRow, out isDeptDiscount);

                #region get discount
                if (oItemRow.isDiscountable == true && (oItemRow.DiscountPolicy == "I" || string.IsNullOrEmpty(oItemRow.DiscountPolicy) == true))
                {
                    returnValue = this.GetDiscount(QTY * Convert.ToDecimal((oItemRow.Discount / 100 * price)));
                }
                else if (isDeptDiscount == true)
                {
                    if (DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate) && isDeptSale == true)
                    {
                        if (oDeptData.Department[0].SaleDiscount > 0)
                        {
                            returnValue = this.GetDiscount(QTY * Convert.ToDecimal((oDeptData.Department[0].SaleDiscount / 100 * price)));
                        }
                    }
                    else if (DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate) && isDeptSale == false)
                    {
                        logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "3");
                        return returnValue;
                    }
                    else
                    {
                        returnValue = this.GetDiscount(QTY * Convert.ToDecimal((oDeptData.Department[0].Discount / 100 * price)));
                    }
                }
                #endregion get discount
            }
            logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "4");

            return returnValue;
        }


        private void ApplyDiscPolicy(ItemRow oRow, out bool IsDeptDiscountable)
        {
            IsDeptDiscountable = false;
            if (oRow.DiscountPolicy == "D")
            {
                IsDeptDiscountable = true;
            }
        }

        public void CalcExtdPrice(TransDetailRow oRow)
        {
            try
            {
                if (oRow.ItemID.Trim() == "")
                    oRow.ExtendedPrice = 0;
                else //if (oRow.ItemID.Trim().ToUpper()!="RX")
                {
                    oRow.ExtendedPrice = Convert.ToDecimal(oRow.QTY) * Convert.ToDecimal(oRow.Price);
                }
            }
            catch (Exception exp)
            {
                return;
            }
        }

        public void RecaclulateTransDetailItem(TransDetailRow oRow, ItemRow oItemRow, TransDetailTaxData oTDTaxData)
        {
            System.Decimal GroupPrice;
            int nonGroupQty = 0;
            GroupPrice = CheckGroupPricing(oRow.ItemID, oRow.QTY, oRow.Price, out nonGroupQty, oRow.OrignalPrice);  //PRIMEPOS-3098 20-Jun-2022 JY Added OrignalPrice
            if (GroupPrice != -1 && Configuration.CPOSSet.ApplyGroupPriceOverCompanionItem == true)
            {
                SetRowTrans(oRow, false);
                oRow.ExtendedPrice = GroupPrice;

                if (oItemRow.isTaxable == true)
                {
                    TaxCodesData oTaxCodesData = null;
                    oTaxCodesData = GetItemTaxData(oItemRow.ItemID);
                    if (oTaxCodesData.Tables[0].Rows.Count > 0)
                    {
                        CalculateTax(oRow, oTaxCodesData, oTDTaxData);
                    }

                }
            }
            else
            {
                if (oItemRow.isTaxable == true)
                {
                    TaxCodesData oTaxCodesData = null;
                    oTaxCodesData = GetItemTaxData(oItemRow.ItemID);
                    if (oTaxCodesData.Tables[0].Rows.Count > 0)
                    {
                        CalculateTax(oRow, oTaxCodesData, oTDTaxData);
                    }
                }
                //frmPOSTransaction oTrnas = new frmPOSTransaction();
                CalcExtdPrice(oRow);
                oRow.Discount = CalculateDiscount(oRow.ItemID, oRow.QTY, oRow.Price);//Added by shitaljit on 9/23/2013 to fix bug of predefined idscount not apllying 
            }
        }


        /// <summary>
        /// Author Shitaljit 
        /// Added to get breakdown of tax totals.
        /// </summary>
        /// <param name="oTDTaxData"></param>
        /// <param name="StateTax"></param>
        /// <param name="LocalTax"></param>
        /// <param name="FederalTax"></param>
        /// <param name="CountyTax"></param>
        /// <param name="CityTax"></param>
        /// <param name="MunicipalityTax"></param>
        public void CalculateBreakDownForTax(TransDetailTaxData oTDTaxData, out Decimal StateTax, out Decimal LocalTax,
            out Decimal FederalTax, out Decimal CountyTax, out Decimal CityTax, out Decimal MunicipalityTax)
        {
            StateTax = 0;
            LocalTax = 0;
            FederalTax = 0;
            CountyTax = 0;
            CityTax = 0;
            MunicipalityTax = 0;

            TaxCodes oTaxCode = new TaxCodes();
            TaxCodesData oTaxCodeData = new TaxCodesData();
            TaxCodesRow oTaxCodeRow = null;
            try
            {
                foreach (TransDetailTaxRow oRow in oTDTaxData.TransDetailTax.Rows)
                {
                    oTaxCodeData = oTaxCode.Populate(oRow.TaxID);
                    if (Configuration.isNullOrEmptyDataSet(oTaxCodeData) == false)
                    {
                        oTaxCodeRow = oTaxCodeData.TaxCodes[0];
                        switch (oTaxCodeRow.TaxType)
                        {
                            case (Int32)TaxTypes.State:
                                StateTax += oRow.TaxAmount;
                                break;
                            case (Int32)TaxTypes.Municipality:
                                MunicipalityTax += oRow.TaxAmount;
                                break;
                            case (Int32)TaxTypes.Local:
                                LocalTax += oRow.TaxAmount;
                                break;
                            case (Int32)TaxTypes.City:
                                CityTax += oRow.TaxAmount;
                                break;
                            case (Int32)TaxTypes.Federal:
                                FederalTax += oRow.TaxAmount;
                                break;
                            case (Int32)TaxTypes.County:
                                CountyTax += oRow.TaxAmount;
                                break;
                            default:
                                StateTax += oRow.TaxAmount;
                                break;
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CalculateBreakDownForTax()");
                throw Ex;
            }
        }


        /// <summary>
        /// Author: Manoj 
        /// Creted Date:7/2/2015
        /// Created to calcualte total tax applicable for an item if item has multiple tax codes applied.
        /// </summary>
        /// <param name="oRow"></param>
        /// <param name="oTaxData"></param>
        public void CalculateTax(TransDetailRow oRow, TaxCodesData oTaxData, TransDetailTaxData oTDTaxData)
        {
            decimal tmpTax = 0.00M;
            decimal tmpTaxAmount = 0.00M;
            int prevItemRow = 0;

            if (oRow != null)
            {
                if (oTaxData != null)
                {
                    foreach (TaxCodesRow oTR in oTaxData.TaxCodes.Rows)
                    {
                        tmpTax = (oTR.Amount / 100) * (oRow.ExtendedPrice - oRow.Discount);

                        #region PRIMEPOS-2640 13-Feb-2019 JY Commented
                        //tmpTaxAmount += RoundTaxValue(tmpTax);
                        //oRow.TaxAmount = tmpTaxAmount; //Sprint-21 23-Sep-2015 JY Added as the tax in "POSTransactionDetail" table is not depends on the "POSTransactionDetailTax" table
                        #endregion

                        tmpTaxAmount += tmpTax; //PRIMEPOS-2640 13-Feb-2019 JY Added
                        oRow.TaxAmount = RoundTaxValue(tmpTaxAmount);   //PRIMEPOS-2640 13-Feb-2019 JY Added

                        foreach (TransDetailTaxRow oTDTaxRow in oTDTaxData.TransDetailTax.Rows)
                        {
                            #region PRIMEPOS-2757 08-Nov-2019 JY Added
                            if (oTR.TaxID == oTDTaxRow.TaxID && oTDTaxRow.ItemID == oRow.ItemID && oTDTaxRow.TransDetailID == oRow.TransDetailID)
                            {
                                //oTDTaxRow.TaxAmount = RoundTaxValue(tmpTax);  //PRIMEPOS-2640 13-Feb-2019 JY Commented
                                if (oTR.TaxCode.Trim().ToUpper() == clsPOSDBConstants.TaxCodes_Fld_RxTax.Trim().ToUpper())  //PRIMEPOS-2978 18-Jun-2021 JY Added if clause
                                {
                                    oTDTaxRow.TaxAmount = oRow.TaxAmount = Configuration.convertNullToDecimal(oTDTaxRow.TaxAmount);
                                }
                                else
                                {
                                    oTDTaxRow.TaxAmount = tmpTax;   //PRIMEPOS-2640 13-Feb-2019 JY Added
                                }
                            }
                            #endregion
                        }
                    }
                    IsInvoiceDisc = false;
                    oRow.TaxCode = string.Empty;
                    foreach (TaxCodesRow tRow in oTaxData.TaxCodes)
                    {
                        oRow.TaxCode += tRow.TaxCode + ",";
                    }
                    if (oRow.TaxCode.EndsWith(","))
                    {
                        oRow.TaxCode = oRow.TaxCode.Substring(0, oRow.TaxCode.Length - 1);
                    }
                }
            }
        }

        #region Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added logic to calculate tax breakup in cse of of on-hold item
        public void CalculateTaxOnHold(TransDetailRow oRow, TaxCodesData oTaxData, TransDetailTaxData oTDTaxData)
        {
            decimal tmpTax = 0.00M;
            decimal tmpTaxAmount = 0.00M;

            #region Update ItemRow in TransDetailTax table, in case of on-hold it creates issue
            int cnt = 1;
            int? previousTransDetailId = null;
            foreach (TransDetailTaxRow oTransDetailTaxRow in oTDTaxData.TransDetailTax.Rows)
            {
                if (previousTransDetailId == null || previousTransDetailId == oTransDetailTaxRow.TransDetailID)
                {
                    oTransDetailTaxRow.ItemRow = cnt;
                    previousTransDetailId = oTransDetailTaxRow.TransDetailID;
                }
                else
                {
                    oTransDetailTaxRow.ItemRow = cnt + 1;
                    previousTransDetailId = oTransDetailTaxRow.TransDetailID;
                }
            }
            #endregion

            if (oRow != null)
            {
                if (oTaxData != null)
                {
                    Dictionary<int, int> dctUpdatedRec = new Dictionary<int, int>();
                    foreach (TaxCodesRow oTR in oTaxData.TaxCodes.Rows)
                    {
                        tmpTax = (oTR.Amount / 100) * (oRow.ExtendedPrice - oRow.Discount);

                        #region PRIMEPOS-2640 13-Feb-2019 JY Added
                        //tmpTaxAmount += RoundTaxValue(tmpTax);
                        //oRow.TaxAmount = tmpTaxAmount; //Sprint-21 23-Sep-2015 JY Added as the tax in "POSTransactionDetail" table is not depends on the "POSTransactionDetailTax" table
                        #endregion

                        tmpTaxAmount += tmpTax;  //PRIMEPOS-2640 13-Feb-2019 JY Added
                        oRow.TaxAmount = RoundTaxValue(tmpTaxAmount);  //PRIMEPOS-2640 13-Feb-2019 JY Added

                        foreach (TransDetailTaxRow oTDTaxRow in oTDTaxData.TransDetailTax.Rows)
                        {
                            if (oTR.TaxID == oTDTaxRow.TaxID && oTDTaxRow.ItemID == oRow.ItemID && oTDTaxRow.TransDetailID == oRow.TransDetailID)
                            {
                                if (dctUpdatedRec.Count > 0)
                                {
                                    bool bRecUpdated = false;
                                    foreach (var obj in dctUpdatedRec)
                                    {
                                        if (obj.Key == oTR.TaxID && obj.Value == oTDTaxRow.TransDetailID)
                                        {
                                            bRecUpdated = true;
                                            break;
                                        }
                                    }
                                    if (bRecUpdated == false)
                                    {
                                        //oTDTaxRow.TaxAmount = RoundTaxValue(tmpTax);  //PRIMEPOS-2640 13-Feb-2019 JY Commented
                                        oTDTaxRow.TaxAmount = tmpTax;   //PRIMEPOS-2640 13-Feb-2019 JY Added
                                        dctUpdatedRec.Add(oTR.TaxID, oTDTaxRow.TransDetailID);
                                    }
                                }
                                else
                                {
                                    //oTDTaxRow.TaxAmount = RoundTaxValue(tmpTax);  //PRIMEPOS-2640 13-Feb-2019 JY Commented
                                    oTDTaxRow.TaxAmount = tmpTax;   //PRIMEPOS-2640 13-Feb-2019 JY Added
                                    dctUpdatedRec.Add(oTR.TaxID, oTDTaxRow.TransDetailID);
                                }
                            }
                        }
                    }
                    IsInvoiceDisc = false;
                    oRow.TaxCode = string.Empty;
                    foreach (TaxCodesRow tRow in oTaxData.TaxCodes)
                    {
                        oRow.TaxCode += tRow.TaxCode + ",";
                    }
                    if (oRow.TaxCode.EndsWith(","))
                    {
                        oRow.TaxCode = oRow.TaxCode.Substring(0, oRow.TaxCode.Length - 1);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Add by Manoj to Calculate Tax that is apply after the nontaxable item is added
        /// </summary>
        /// <param name="oRow"></param>
        /// <param name="oTaxData"></param>
        /// <param name="oTDTaxData"></param>
        public void ApplyTaxCalculation(TransDetailRow oRow, TaxCodesData oTaxData, TransDetailTaxData oTDTaxData)
        {
            decimal tmpTax = 0.0M;
            decimal tmpTaxAmount = 0.0M;
            int prevItemRow = 0;
            if (oRow != null)
            {
                foreach (TaxCodesRow oTR in oTaxData.TaxCodes.Rows)
                {
                    tmpTax = (oTR.Amount / 100) * (oRow.ExtendedPrice - oRow.Discount);
                    tmpTaxAmount += tmpTax;
                    oRow.TaxAmount = RoundTaxValue(tmpTaxAmount); //Sprint-21 23-Sep-2015 JY Added as the tax in "POSTransactionDetail" table is not depends on the "POSTransactionDetailTax" table
                    foreach (TransDetailTaxRow oTDTaxRow in oTDTaxData.TransDetailTax.Rows)
                    {
                        if (oTR.TaxID == oTDTaxRow.TaxID && oTDTaxRow.ItemID == oRow.ItemID) oTDTaxRow.TaxAmount = tmpTax;   //Sprint-21 23-Sep-2015 JY Added to resolve tax issue

                        #region Sprint-21 23-Sep-2015 JY Commented
                        //if (oTDTaxRow.TaxPercent == RoundTaxValue(oTR.Amount) && oTDTaxRow.TaxID == oTR.TaxID && oTDTaxRow.ItemID == oRow.ItemID && oTDTaxRow.TaxAmount == 0)
                        //{
                        //    oTDTaxRow.TaxAmount = tmpTax;
                        //    oRow.TaxID = oTDTaxRow.TaxID;
                        //    oRow.TaxAmount = RoundTaxValue(tmpTax);
                        //}
                        //if(oTDTaxRow.ItemRow != prevItemRow)
                        //{
                        //    prevItemRow = oTDTaxRow.ItemRow;
                        //}
                        //else
                        //{
                        //    oRow.TaxAmount = RoundTaxValue(tmpTaxAmount);
                        //}
                        #endregion
                    }
                }
                // oRow.TaxAmount = RoundTaxValue(tmpTaxAmount);

                oRow.TaxCode = string.Empty;
                foreach (TaxCodesRow tRow in oTaxData.TaxCodes)
                {
                    oRow.TaxCode += tRow.TaxCode + ",";
                }
                if (oRow.TaxCode.EndsWith(","))
                {
                    oRow.TaxCode = oRow.TaxCode.Substring(0, oRow.TaxCode.Length - 1);
                }
            }
        }



        /// <summary>
        /// Author:Shitaljit 
        /// To check whether item is taxable or not in transaction.
        /// </summary>
        /// <param name="oTDTaxData"></param>
        /// <param name="sItemID"></param>
        /// <param name="sTaxIds"></param>
        /// <returns></returns>
        public bool IsItemTaxableForTrasaction(TransDetailTaxData oTDTaxData, string sItemID, out string sTaxIds, int TransDetailID)    //PRIMEPOS-2500 05-Apr-2018 JY Added TransDetailID as in case of GROUPTRANSITEMS OFF, it returns wrong taxcodes if we assigned different tax codes for same item (can be achieved by Edit Item in transaction)
        {
            bool isItemTaxable = false;
            ItemTax oItemTax = new ItemTax();
            sTaxIds = string.Empty;
            try
            {
                foreach (TransDetailTaxRow oTDTaxRow in oTDTaxData.TransDetailTax.Rows)
                {
                    if (oTDTaxRow.ItemID.Equals(sItemID) == true && oTDTaxRow.TransDetailID.Equals(TransDetailID) == true)  //oTDTaxRow.TransDetailID.Equals(TransDetailID) == true added
                    {
                        sTaxIds += oTDTaxRow.TaxID + ",";
                    }
                }

                if (sTaxIds.EndsWith(","))
                {
                    sTaxIds = sTaxIds.Substring(0, sTaxIds.Length - 1);
                }
                if (string.IsNullOrEmpty(sTaxIds) == false)
                {
                    isItemTaxable = true;
                    sTaxIds = "(" + sTaxIds + ")";
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return isItemTaxable;
        }

        /// <summary>
        /// Author:Shitaljit
        /// Added to update tax details of item as per the tax information being changed.
        /// </summary>
        /// <param name="oTDTaxData"></param>
        /// <param name="sItemId"></param>
        public void UpdateTransTaxDetails(TransDetailTaxData oTDTaxData, string sItemId)
        {
            try
            {
                int RowIndex = 0;
                int RowCount = oTDTaxData.TransDetailTax.Rows.Count;
                //TransDetailTaxTable tmpTax = oTDTaxData.TransDetailTax;   //PRIMEPOS-2500 29-Mar-2018 JY Commented as not in use
                if (CurrentTransactionType == POSTransactionType.SalesReturn)
                {
                    //Added by Manoj 7/29/2015 - Fix issue where return tax is not save in the POSTransactionDetailTax
                    while (RowIndex < RowCount)
                    {
                        TransDetailTaxRow oRow = oTDTaxData.TransDetailTax[(RowCount - 1) - RowIndex];
                        if (oRow.ItemID.Equals(sItemId.Trim()) && ClickRow == oRow.ItemRow)
                        {
                            oRow.Delete();
                            int row = 1;
                            foreach (TransDetailTaxRow tr in oTDTaxData.TransDetailTax)
                            {
                                try
                                {
                                    tr.ItemRow = row;
                                    row++;
                                }
                                catch { }
                            }
                            break;
                        }
                        RowIndex++;
                    }
                    ClickRow = 0; //Reset after use.
                }
                else
                {
                    while (RowIndex < RowCount)
                    {
                        TransDetailTaxRow oRow = oTDTaxData.TransDetailTax[(RowCount - 1) - RowIndex];
                        if (oRow.ItemID.Equals(sItemId) && ClickRow == oRow.ItemRow)    //Sprint-26 - PRIMEPOS-2407 27-Jun-2017 JY Added ClickRow == oRow.ItemRow
                        {
                            oRow.Delete();
                            ClickRow = 0;
                            int row = 1;
                            foreach (TransDetailTaxRow tr in oTDTaxData.TransDetailTax)
                            {
                                try
                                {
                                    tr.ItemRow = row;
                                    row++;
                                }
                                catch { }
                            }
                            break;
                        }
                        RowIndex++;
                    }
                }
                oTDTaxData.AcceptChanges();
                oTDTaxData.TransDetailTax.AcceptChanges();
            }
            catch
            {
                UpdateTransTaxDetails(oTDTaxData, sItemId);
            }
        }

        #region Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added logic to delete taxdetails against deleted record
        public void UpdateTransTaxDetails(TransDetailTaxData oTDTaxData, int TransDetailID)
        {
            try
            {
                string strOldTaxCodesWithPercentage = string.Empty; //PRIMEPOS-2402 13-Jul-2021 JY Added
                for (int i = oTDTaxData.TransDetailTax.Rows.Count - 1; i >= 0; i--)
                {
                    TransDetailTaxRow oTransDetailTaxRow = oTDTaxData.TransDetailTax[i];
                    if (oTransDetailTaxRow.TransDetailID == TransDetailID)
                    {
                        //PRIMEPOS-2402 13-Jul-2021 JY Added
                        if (strOldTaxCodesWithPercentage == "")
                            strOldTaxCodesWithPercentage = oTransDetailTaxRow.TaxID.ToString() + "~" + oTransDetailTaxRow.TaxPercent.ToString();
                        else
                            strOldTaxCodesWithPercentage += "," + oTransDetailTaxRow.TaxID.ToString() + "~" + oTransDetailTaxRow.TaxPercent.ToString();

                        oTransDetailTaxRow.Delete();
                    }
                }
                oTDTaxData.AcceptChanges();
                oTDTaxData.TransDetailTax.AcceptChanges();

                oTDRow.TaxAmount = 0;   //when we clear the taxes, need to clear the respective amount and TaxCodes
                oTDRow.TaxCode = "";
                oTDRow.OldTaxCodesWithPercentage = strOldTaxCodesWithPercentage;    //PRIMEPOS-2402 13-Jul-2021 JY Added
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "UpdateTransTaxDetails()");
            }
        }
        #endregion
        /// <summary>
        /// Move from frmPOSPayTypeList.cs By Shitaljit(QuicSolv) on 16 May 2011
        /// this method does the following:
        /// if the Receive  on Account isROA is true, it get the cc information from the charge account table
        /// if the transaction has a Rx in it, it finds out the patient and gets the cc info from PatPayPref Table
        /// if cc info is not found returns blank
        /// if cc info is found, populates the PCCCardINfo object with the cc info and makes it available for the next actual cc process to use as default
        /// For Non-Rx Item Transactions also the aboe featureas are proided.
        /// </summary>
        /// <param name="sTransType"></param>
        /// <param name="oRxHList"></param>
        /// <param name="dtChargeAccount"></param>
        /// <param name="sPatientNo"></param>
        /// <param name="CustCardInfo"></param>
        /// <returns></returns>
        public string GetCCInformation(string sTransType, RXHeaderList oRxHList, DataTable dtChargeAccount, string sPatientNo, out PccCardInfo CustCardInfo)
        {
            CustomerCardInfo = new PccCardInfo();
            CustCardInfo = CustomerCardInfo;
            string sCardTypeName = "";
            bool ccinfoFound = false;
            frmCCInfoSelect frmccInfo = new frmCCInfoSelect();
            DataTable dtCC = new DataTable();
            DataTable dtPatPayPref = new DataTable();
            string sUECC = "";
            string sCCENC = "";
            string PatientName = "";
            string PatientAddr = "";
            DataTable dtPatInfo = new DataTable();
            PharmBL oPhBl = new PharmBL();
            string sCC = "";
            ContAccount oAcct = new ContAccount();
            string PatientNo = ""; //PRIMEPOS-3494 corrected spell
            if (sTransType == F10TransType.ROA)
            {
                dtCC.Columns.Add("Name");
                dtCC.Columns.Add("CCNUMBERMASK");
                dtCC.Columns.Add("CCNUMBER");
                dtCC.Columns.Add("CCEXPDATE");
                dtCC.Columns.Add("NameOnCC");
                dtCC.Columns.Add("CardAddress");
                dtCC.Columns.Add("ZIP");
                dtCC.Columns.Add("CardInfoSource");

                string[] fields = new String[8];
                if (dtChargeAccount != null && dtChargeAccount.Rows.Count > 0 && dtChargeAccount.Rows[0]["CCNUMBER"].ToString().Trim().Length > 0)
                {
                    if (Configuration.convertNullToBoolean(dtChargeAccount.Rows[0]["IE"].ToString()))
                    {
                        if (MMS.Encryption.NativeEncryption.DecryptText(dtChargeAccount.Rows[0]["CCNUMBER"].ToString(), ref sUECC))
                        {
                            dtChargeAccount.Rows[0]["CCNUMBER"] = sUECC;
                        }
                    }

                    fields[0] = dtChargeAccount.Rows[0]["ACCT_NAME"].ToString();
                    fields[1] = GetMaskedCC(dtChargeAccount.Rows[0]["CCNUMBER"].ToString());
                    fields[2] = dtChargeAccount.Rows[0]["CCNUMBER"].ToString();
                    fields[3] = dtChargeAccount.Rows[0]["CCEXPDATE"].ToString();
                    fields[4] = dtChargeAccount.Rows[0]["NameOnCC"].ToString();
                    fields[5] = "";
                    fields[6] = dtChargeAccount.Rows[0]["ZIP"].ToString();
                    fields[7] = "Charge Account";
                    dtCC.Rows.Add(fields);
                }

                //Logic To Populate Credit card info From PatPayPref for the selectted Acct_No
                string sAccNo = dtChargeAccount.Rows[0]["ACCT_NO"].ToString();
                dtPatPayPref = GetPatientPayPrefByAccNo(sAccNo);
                if (dtPatPayPref != null)
                {
                    foreach (DataRow oRow in dtPatPayPref.Rows)
                    {
                        sPatientNo = oRow["PatientNo"].ToString();
                        //ErrorLogging.Logs.Logger("POSTransaction", "GetCCInformation()", "About to call PharmSQL");
                        logger.Trace("GetCCInformation() - About to call PharmSQL");
                        dtPatInfo = oPhBl.GetPatient(sPatientNo);
                        //ErrorLogging.Logs.Logger("POSTransaction", "GetCCInformation()", "Call PharmSQL Sucessful");
                        logger.Trace("GetCCInformation() - Call PharmSQL Sucessful");
                        PatientName = dtPatInfo.Rows[0]["LNAME"].ToString().Trim() + " " + dtPatInfo.Rows[0]["FNAME"].ToString().Trim();
                        PatientAddr = dtPatInfo.Rows[0]["ADDRSTR"].ToString().Trim() + " " + dtPatInfo.Rows[0]["ADDRCT"].ToString().Trim()
                                      + " " + ", " + dtPatInfo.Rows[0]["ADDRST"].ToString().Trim()
                                      + " " + dtPatInfo.Rows[0]["ADDRZP"].ToString().Trim();

                        fields[0] = oRow["LNAME"].ToString();
                        sCCENC = oRow["IE"].ToString();
                        if (Configuration.convertNullToBoolean(sCCENC) == true)
                        {
                            MMS.Encryption.NativeEncryption.DecryptText(oRow["CardNo"].ToString(), ref sCC);
                        }
                        else
                        {
                            sCC = oRow["CardNo"].ToString();
                        }
                        fields[1] = GetMaskedCC(sCC);
                        fields[2] = sCC;
                        fields[3] = oRow["Expires"].ToString();
                        fields[4] = oRow["NameOnCard"].ToString();
                        fields[5] = oRow["CardAddress"].ToString();
                        fields[6] = oRow["CardZip"].ToString();
                        fields[7] = "Patient Pay Preference";
                        dtCC.Rows.Add(fields);
                    }
                }
                if (dtCC.Rows.Count > 0)
                {
                    frmccInfo.CCInfo = dtCC;
                    if (frmccInfo.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.CustomerCardInfo = frmccInfo.SelectedCC;
                        CustCardInfo = this.CustomerCardInfo;
                        ccinfoFound = true;
                    }
                }
                else
                {
                    clsCoreUIHelper.ShowErrorMsg("Account#" + sAccNo + " does not have credit card information.");
                }
            }
            else if (sTransType == F10TransType.RXTrans)
            {
                if (oRxHList != null)
                {
                    dtCC.Columns.Add("NAME");
                    dtCC.Columns.Add("ADDRESS");
                    dtCC.Columns.Add("CCNUMBERMASK");
                    dtCC.Columns.Add("CCNUMBER");
                    dtCC.Columns.Add("CCEXPDATE");
                    dtCC.Columns.Add("NameOnCC");
                    dtCC.Columns.Add("CardAddress");
                    dtCC.Columns.Add("ZIP");

                    foreach (RXHeader rxh in oRxHList)
                    {
                        PatientNo = rxh.PatientNo; //PRIMEPOS-3494 corrected spell
                        dtPatPayPref = PopulatePatientAccount(rxh.PatientNo);
                        if (dtPatPayPref != null)
                        {
                            foreach (DataRow oRow in dtPatPayPref.Rows)
                            {
                                string[] fields = new String[8];

                                fields[0] = rxh.PatientName;
                                fields[1] = rxh.PatientAddr;
                                sCCENC = (oRow["IE"].ToString());
                                if (Configuration.convertNullToBoolean(sCCENC) == true)
                                {
                                    MMS.Encryption.NativeEncryption.DecryptText(oRow["CardNo"].ToString(), ref sCC);
                                }
                                else
                                {
                                    sCC = oRow["CardNo"].ToString();
                                }

                                fields[2] = GetMaskedCC(sCC);
                                fields[3] = sCC;
                                fields[4] = oRow["Expires"].ToString();
                                fields[5] = oRow["NameOnCard"].ToString();
                                fields[6] = oRow["CardAddress"].ToString();
                                fields[7] = oRow["CardZip"].ToString();
                                dtCC.Rows.Add(fields);
                            }
                        }
                    }
                    if (dtCC.Rows.Count > 0)
                    {
                        frmccInfo.CCInfo = dtCC;
                        if (frmccInfo.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            this.CustomerCardInfo = frmccInfo.SelectedCC;
                            CustCardInfo = this.CustomerCardInfo;
                            ccinfoFound = true;
                        }
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Patient#" + sPatientNo + "  does not have credit card information.");
                    }
                }
            }
            else if (sTransType == F10TransType.NonRXTrans)
            {
                dtCC = new DataTable();
                dtCC.Columns.Add("NAME");
                dtCC.Columns.Add("ADDRESS");
                dtCC.Columns.Add("CCNUMBERMASK");
                dtCC.Columns.Add("CCNUMBER");
                dtCC.Columns.Add("CCEXPDATE");
                dtCC.Columns.Add("NameOnCC");
                dtCC.Columns.Add("CardAddress");
                dtCC.Columns.Add("ZIP");
                dtCC.Columns.Add("CardInfoSource");
                //ErrorLogging.Logs.Logger("POSTransaction", "GetCCInformation()", "(Else) About to call PharmSQL");
                logger.Trace("GetCCInformation() - (Else) About to call PharmSQL");
                dtPatInfo = oPhBl.GetPatient(sPatientNo);
                //ErrorLogging.Logs.Logger("POSTransaction", "GetCCInformation()", "(Else) Call PharmSQL Sucessful");
                logger.Trace("GetCCInformation() - (Else) Call PharmSQL Sucessful");
                if (dtPatInfo.Rows.Count > 0)
                {
                    PatientName = dtPatInfo.Rows[0]["LNAME"].ToString().Trim() + " " + dtPatInfo.Rows[0]["FNAME"].ToString().Trim();
                    PatientAddr = dtPatInfo.Rows[0]["ADDRSTR"].ToString().Trim() + " " + dtPatInfo.Rows[0]["ADDRCT"].ToString().Trim()
                                   + " " + ", " + dtPatInfo.Rows[0]["ADDRST"].ToString().Trim()
                                    + " " + dtPatInfo.Rows[0]["ADDRZP"].ToString().Trim();

                    dtPatPayPref = PopulatePatientAccount(sPatientNo);
                    if (dtPatPayPref != null)
                    {
                        foreach (DataRow oRow in dtPatPayPref.Rows)
                        {
                            string[] fields = new String[9];

                            fields[0] = PatientName;
                            fields[1] = PatientAddr;
                            sCCENC = (oRow["IE"].ToString());
                            if (Configuration.convertNullToBoolean(sCCENC) == true)
                            {
                                MMS.Encryption.NativeEncryption.DecryptText(oRow["CardNo"].ToString(), ref sCC);
                            }
                            else
                            {
                                sCC = oRow["CardNo"].ToString();
                            }
                            fields[2] = GetMaskedCC(sCC);
                            fields[3] = sCC;
                            fields[4] = oRow["Expires"].ToString();
                            fields[5] = oRow["NameOnCard"].ToString();
                            fields[6] = oRow["CardAddress"].ToString();
                            fields[7] = oRow["CardZip"].ToString();
                            fields[8] = "Patient Pay Preference";
                            dtCC.Rows.Add(fields);
                        }
                    }
                    DataSet oDS = new DataSet();
                    string AcctNo = dtPatInfo.Rows[0]["ACCT_NO"].ToString().Trim();
                    if (AcctNo != string.Empty) //PRIMEPOS-2205 02-Aug-2016 JY Added if condition as if AcctNo is NULL then it will return wrong data
                    {
                        oAcct.GetAccountByCode(AcctNo, out oDS, true);
                    }
                    if (oDS != null)
                    {
                        if (oDS.Tables[0].Rows.Count > 0)
                        {
                            dtChargeAccount = oDS.Tables[0];
                        }
                    }
                    if (dtChargeAccount != null && dtChargeAccount.Rows.Count > 0 && dtChargeAccount.Rows[0]["CCNUMBER"].ToString().Trim().Length > 0)
                    {
                        if (Configuration.convertNullToBoolean(dtChargeAccount.Rows[0]["IE"].ToString()))
                        {
                            if (MMS.Encryption.NativeEncryption.DecryptText(dtChargeAccount.Rows[0]["CCNUMBER"].ToString(), ref sUECC))
                            {
                                dtChargeAccount.Rows[0]["CCNUMBER"] = sUECC;
                            }
                        }

                        string[] fields = new String[9];

                        fields[0] = dtChargeAccount.Rows[0]["ACCT_NAME"].ToString();
                        fields[1] = PatientName;
                        fields[2] = GetMaskedCC(dtChargeAccount.Rows[0]["CCNUMBER"].ToString());
                        fields[3] = dtChargeAccount.Rows[0]["CCNUMBER"].ToString();
                        fields[4] = dtChargeAccount.Rows[0]["CCEXPDATE"].ToString();
                        fields[5] = dtChargeAccount.Rows[0]["NameOnCC"].ToString();
                        fields[6] = PatientAddr;
                        fields[7] = dtChargeAccount.Rows[0]["ZIP"].ToString();
                        fields[8] = "Charge Account";
                        dtCC.Rows.Add(fields);

                        frmccInfo.CCInfo = dtCC;
                    }
                }
                if (dtCC.Rows.Count > 0)
                {
                    frmccInfo.CCInfo = dtCC;
                    frmccInfo.SearchByPatientNO = true;
                    if (frmccInfo.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.CustomerCardInfo = frmccInfo.SelectedCC;
                        CustCardInfo = this.CustomerCardInfo;
                        ccinfoFound = true;
                    }
                }
                else
                {
                    clsCoreUIHelper.ShowErrorMsg("Patient#" + sPatientNo + "  does not have credit card information.");
                }
            }
            if (ccinfoFound)
            {
                PccPaymentSvr pmtSrv = new PccPaymentSvr();
                string sCardType = pmtSrv.GetCardType(CustomerCardInfo.cardNumber);

                //now convert the cardtype to the card name as present in our system
                if (sCardType.Trim().Length > 0)
                {
                    switch (sCardType)
                    {
                        case "VISA":
                            sCardTypeName = "VISA";
                            break;

                        case "MC":
                            sCardTypeName = "MASTER CARD";
                            break;

                        case "AMEX":
                            sCardTypeName = "AMERICAN EXPRESS";
                            break;

                        case "DISC":
                            sCardTypeName = "DISOVER";
                            break;
                        default:
                            sCardTypeName = "VISA";
                            break;
                    }
                }
            }
            return sCardTypeName;
        }

        private string GetMaskedCC(string CCNumber)
        {
            string smaskedCC = "";
            if (CCNumber.Trim().Length > 4)
            {
                string mX = CCNumber.Trim().Substring(0, CCNumber.Trim().Length - 4);
                if (mX.Length <= 16)
                    mX = "XXXXXXXXXXXXXXXX".Substring(0, mX.Length);
                string mN = CCNumber.Trim().Substring(CCNumber.Trim().Length - 4, 4);
                smaskedCC = mX + mN;
            }
            else
                smaskedCC = CCNumber;

            return smaskedCC;
        }

        //Till here added By Shitaljit(QuicSolv)
        public System.Decimal RoundTaxValue(System.Decimal dTax)
        {
            System.Decimal retVal = 0;
            String str = dTax.ToString();
            int dot = str.IndexOf(".", 1);
            if (dot > 0)
            {
                if (Configuration.CPOSSet.RoundTaxValue == false)
                {
                    String pre, post;
                    pre = str.Substring(0, dot);
                    post = str.Substring(dot + 1);
                    if (post.Length > 2)
                        post = post.Substring(0, 2);
                    retVal = Convert.ToDecimal(pre + "." + post);
                }
                else
                {
                    //retVal = Math.Round(Convert.ToDecimal(str), 2);   //Sprint-26 - PRIMEPOS-2407 22-Jun-2017 JY Commented
                    retVal = Math.Round(Convert.ToDecimal(str), 2, MidpointRounding.AwayFromZero);   //Sprint-26 - PRIMEPOS-2407 22-Jun-2017 JY Added
                }
            }
            return retVal;
        }

        #region NPLEX

        //public bool NplexProcessPSE(CustomerRow oCustAddress, Business_Tier.DL ID, signatureType oSignatureType, string verificationId, string authorizationNo, string Dob, out string pseTrxId, out string inquiryId, ref string errMsg)   //PRIMEPOS-2821 04-Nov-2020 JY Commented
        public bool NplexProcessPSE(CustomerRow oCustAddress, signatureType oSignatureType, string verificationId, string authorizationNo, string Dob, out string pseTrxId, out string inquiryId, ref string errMsg)   //PRIMEPOS-2821 04-Nov-2020 JY Added oCustAddress parameter
        {
            logger.Trace("NplexProcessPSE() - " + clsPOSDBConstants.Log_Entering);
            pseTrxId = string.Empty;
            inquiryId = string.Empty;
            bool isValid = false;
            NplexBL oNplex = new NplexBL();
            NplexBL.PatientInfo PatInfo = new NplexBL.PatientInfo();
            List<ProductType> MethItem = new List<ProductType>();

            oNplex.MethItem.Clear();
            MethItem.Clear();

            try
            {
                Item oItem = new Item();
                ItemData oIData = null;

                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    DataTable dtPSE_Items = oItem.IsPSEItemData(oRow.ItemID);
                    if (dtPSE_Items != null && dtPSE_Items.Rows.Count > 0)
                    {
                        oIData = oItem.Populate(oRow.ItemID.Trim());
                        ProductType item = new ProductType();

                        item.upc = oIData.Item[0].ItemID;
                        item.name = oIData.Item[0].Description;
                        item.grams = Convert.ToSingle(dtPSE_Items.Rows[0]["ProductGrams"]);
                        if ((int)dtPSE_Items.Rows[0]["ProductPillCnt"] > 0)
                        {
                            item.pills = (int)dtPSE_Items.Rows[0]["ProductPillCnt"];
                        }
                        //item.pediatricInd = false; //later on it should be populated from settings
                        item.boxCount = oRow.QTY;
                        MethItem.Add(item);
                    }
                }
                oNplex.MethItem = MethItem;

                //get patient/customer data
                //id, idType, idIssuingAgency, lastName, firstName, birthDate, addressLine1, City, state and postalCode.
                bool isValidPatientData = false;
                DateTime result = DateTime.Now;

                #region PRIMEPOS-2821 04-Nov-2020 JY Added
                try
                {
                    PatInfo.ID = oCustAddress.DriveLicNo;
                    try
                    {
                        PatInfo.IDType = GetIDType(oTransSignLogData.POSTransSignLog[0].CustomerIDType);
                    }
                    catch
                    {
                        PatInfo.IDType = GetIDType(Configuration.convertNullToString(verificationId));
                    }
                    PatInfo.IDIssuingAgency = Configuration.convertNullToString(oCustAddress.State);
                    PatInfo.LastName = oCustAddress.CustomerName;
                    PatInfo.FirstName = oCustAddress.FirstName;

                    if (Configuration.convertNullToString(Dob) != "")
                    {
                        String format = "MMddyyyy";
                        try
                        {
                            result = DateTime.ParseExact(Dob, format, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception Ex1) { }
                        PatInfo.DateOfBirth = result.ToShortDateString();
                    }
                    else
                    {
                        if (Configuration.convertNullToString(oCustomerRow.DateOfBirth) != "")
                            PatInfo.DateOfBirth = Convert.ToDateTime(oCustAddress.DateOfBirth).ToShortDateString();
                    }

                    PatInfo.Address1 = Configuration.convertNullToString(oCustAddress.Address1);
                    PatInfo.PostalCode = Configuration.convertNullToString(oCustAddress.Zip);
                    PatInfo.State = Configuration.convertNullToString(oCustAddress.State);
                    PatInfo.City = Configuration.convertNullToString(oCustAddress.City);
                }
                catch { }
                #endregion

                #region PRIMEPOS-2821 04-Nov-2020 JY commented
                ////ID-scan
                //if (ID != null)
                //{
                //    PatInfo.ID = Configuration.convertNullToString(ID.DAQ);  //DAQ	Customer ID Number (License No.)  
                //    try
                //    {
                //        PatInfo.IDType = GetIDType(oTransSignLogData.POSTransSignLog[0].CustomerIDType);
                //    }
                //    catch
                //    {
                //        PatInfo.IDType = GetIDType(Configuration.convertNullToString(verificationId));
                //    }

                //    PatInfo.IDIssuingAgency = Configuration.convertNullToString(ID.DAJ);    //DAJ	Address – Jurisdiction Code
                //    PatInfo.LastName = Configuration.convertNullToString(ID.DCS);    //DCS	Family Name
                //    PatInfo.FirstName = Configuration.convertNullToString(ID.DCT);  //DCT	Given Name

                //    String dateString = Configuration.convertNullToString(ID.DBB); //DBB	Date of Birth
                //    String format = "MMddyyyy";                    
                //    try
                //    {
                //        result = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture);                        
                //    } catch (Exception Ex1)
                //    {
                //        //This is not mandatory parameter
                //    }
                //    PatInfo.DateOfBirth = result.ToShortDateString();
                //    PatInfo.Address1 = Configuration.convertNullToString(ID.DAG);    //DAG	Street Address 1
                //    PatInfo.PostalCode = Configuration.convertNullToString(ID.DAK);   //DAK	Address Postal Code
                //    PatInfo.State = Configuration.convertNullToString(ID.DAJ);  //DAJ	Address – Jurisdiction Code    //required for PO
                //    PatInfo.City = Configuration.convertNullToString(ID.DAI);    //DAI	Address City    //not mandatory

                //    if (PatInfo.ID.Trim() != string.Empty && PatInfo.IDType.ToString() != string.Empty && PatInfo.IDIssuingAgency.Trim() != string.Empty && PatInfo.LastName.Trim() != string.Empty && PatInfo.FirstName.Trim() != string.Empty && Configuration.convertNullToString(PatInfo.DateOfBirth).Trim() != string.Empty && PatInfo.Address1.Trim() != string.Empty && PatInfo.PostalCode.Trim() != string.Empty && PatInfo.State.Trim() != string.Empty)
                //    {
                //        isValidPatientData = true;
                //        #region PRIMEPOS-2729 06-Sep-2019 JY as discussed with Vivek - he don't want to save data through id-scan in customer table
                //        ////SAVE THIS DATA IN CUSTOMER TABLE
                //        //Customer oCustomer = new Customer();
                //        //bool status = oCustomer.UpdateCustomer(oCustomerRow.CustomerId, PatInfo.DateOfBirth.Trim(), PatInfo.Address1.Trim(), PatInfo.PostalCode.Trim(), PatInfo.State.Trim());
                //        //if (status)
                //        //{
                //        //    logger.Trace("NplexProcessPSE() - Customer information updated successfully through ID-Scan");
                //        //}
                //        #endregion
                //    }
                //}

                //if (isValidPatientData == false && oCustomerRow != null)
                //{
                //    try
                //    {
                //        PatInfo.ID = oTransSignLogData.Tables[0].Rows[0]["CustomerIdDetail"].ToString().Substring(0, oTransSignLogData.Tables[0].Rows[0]["CustomerIdDetail"].ToString().IndexOf('^'));
                //    }
                //    catch
                //    {
                //        PatInfo.ID = authorizationNo;
                //    }
                //    try
                //    {
                //        PatInfo.IDType = GetIDType(oTransSignLogData.POSTransSignLog[0].CustomerIDType);
                //    }
                //    catch
                //    {
                //        PatInfo.IDType = GetIDType(Configuration.convertNullToString(verificationId));
                //    }

                //    PatInfo.IDIssuingAgency = Configuration.convertNullToString(oCustomerRow.State);
                //    PatInfo.LastName = Configuration.convertNullToString(oCustomerRow.CustomerName);
                //    PatInfo.FirstName = Configuration.convertNullToString(oCustomerRow.FirstName);
                //    //PatInfo.DateOfBirth = Convert.ToDateTime(oCustomerRow.DateOfBirth).ToShortDateString();   //PRIMEPOS-2729 06-Sep-2019 JY Commented
                //    #region PRIMEPOS-2729 06-Sep-2019 JY Added
                //    if (Configuration.convertNullToString(Dob) != "")
                //    {
                //        String format = "MMddyyyy";
                //        try
                //        {
                //            result = DateTime.ParseExact(Dob, format, System.Globalization.CultureInfo.InvariantCulture);
                //        }
                //        catch (Exception Ex1){}
                //        PatInfo.DateOfBirth = result.ToShortDateString();
                //    }
                //    else
                //    {
                //        if (Configuration.convertNullToString(oCustomerRow.DateOfBirth) != "")
                //            PatInfo.DateOfBirth = Convert.ToDateTime(oCustomerRow.DateOfBirth).ToShortDateString();
                //    }
                //    #endregion

                //    PatInfo.Address1 = Configuration.convertNullToString(oCustomerRow.Address1);
                //    PatInfo.PostalCode = Configuration.convertNullToString(oCustomerRow.Zip);
                //    PatInfo.State = Configuration.convertNullToString(oCustomerRow.State);    //required for PO
                //    PatInfo.Address2 = Configuration.convertNullToString(oCustomerRow.Address2);    //not mandatory
                //    PatInfo.City = Configuration.convertNullToString(oCustomerRow.City);
                //}
                #endregion

                string strErrorMsg = string.Empty;
                InquiryResponseType inqResponse = oNplex.DoInquiryRequest(PatInfo, ref strErrorMsg);    //PRIMEPOS-2999 10-Sep-2021 JY Added strErrorMsg

                if (inqResponse.trxStatus != null && inqResponse.trxStatus.resultCode == 0)
                {
                    logger.Trace("NplexProcessPSE() - Inquiry request success");
                    inquiryId = inqResponse.inquiryId;
                    int CustomerId = Configuration.convertNullToInt(oCustomerRow.CustomerId);   //PRIMEPOS-2572 11-Jun-2020 JY Added
                    PurchaseResponseType purResponse = oNplex.DoPurchaseRequest(PatInfo, inquiryId, oSignatureType, CustomerId);
                    if (purResponse.trxStatus.resultCode == 0)
                    {
                        if (purResponse.pseResult.result.Trim().ToUpper() == "SUCCESS")
                        {
                            logger.Trace("NplexProcessPSE() - Purchase request success");
                            pseTrxId = purResponse.pseTrxId;
                            isValid = true;
                        }
                        else
                        {
                            errMsg = "NPLEx Compliance Failed";
                            for (int i = 0; i < purResponse.pseResult.agent.GetLength(0); i++)
                            {
                                string sAgentName = purResponse.pseResult.agent[i].name;
                                errMsg += Environment.NewLine + Environment.NewLine + sAgentName;
                                for (int j = 0; j < purResponse.pseResult.agent[i].agentCheck.GetLength(0); j++)
                                {
                                    if (purResponse.pseResult.agent[i].agentCheck[j].result.Trim().ToUpper() == "FAIL")
                                    {
                                        errMsg += Environment.NewLine + "\t Limit: " + purResponse.pseResult.agent[i].agentCheck[j].Value + " : " + purResponse.pseResult.agent[i].agentCheck[j].result;
                                    }
                                }
                            }
                            try
                            {
                                NplexBL oNplexBL = new NplexBL();
                                oNplexBL.DeleteNplexRecovery(purResponse.pseTrxId);
                            }
                            catch (Exception Ex) { }
                            return false;
                        }
                    }
                    else
                    {
                        isValid = false;
                        errMsg = "NPLEx purchase Error: " + purResponse.trxStatus.errorMsg;
                        logger.Trace("NplexProcessPSE() - Purchase request error-" + purResponse.trxStatus.errorMsg);
                    }
                }
                else
                {
                    isValid = false;
                    if (inqResponse.trxStatus != null && Configuration.convertNullToString(inqResponse.trxStatus.errorMsg) != "")
                        errMsg = "NPLEx inquiry Error: " + inqResponse.trxStatus.errorMsg;
                    else
                        errMsg = "NPLEx inquiry Error: " + strErrorMsg;
                    logger.Trace("NplexProcessPSE() - " + errMsg);
                }
                logger.Trace("NplexProcessPSE() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "NplexProcessPSE()");
                isValid = false;
                errMsg = ex.Message;
            }
            return isValid;
        }
        #endregion

        public long GetInt(string code)
        {
            long iCode = 0;
            try
            {
                iCode = Convert.ToInt64(code);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while converting code");
            }
            return iCode;
        }

        public bool WarnForDuplicateChargePosting(string totalAmount, out bool IsShowHCDialog) //PRIMEPOS-3264
        {
            IsShowHCDialog = false;
            bool retVal = false;
            string sSelectedAction = string.Empty;
            try
            {
                if (Configuration.convertNullToDecimal(totalAmount) > 0)
                {
                    DataSet oDS = new DataSet();
                    sSelectedAction = clsCorePrimeRXHelper.VarifyChargeAlreadyPosted(oTransDRXData, oTransDData, ref oDS);   //PRIMEPOS-3015 26-Oct-2021 JY Added oDS
                    if (sSelectedAction.Equals("D"))
                    {
                        #region PRIMEPOS-3015 26-Oct-2021 JY Added
                        try
                        {
                            if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow oRow in oDS.Tables[0].Rows)
                                {
                                    foreach (TransDetailRow oTransDetailRow in oTransDData.TransDetail.Rows)
                                    {
                                        if (oTransDetailRow.ItemID.Trim().ToUpper() == "RX" && oTransDetailRow.ItemDescription.Contains(oRow["RXNO"].ToString().Trim() + "-" + oRow["REFILL_NO"].ToString().Trim()))
                                        {
                                            oTransDetailRow.ItemCost = oTransDetailRow.ExtendedPrice;
                                            oTransDetailRow.Price = 0;
                                            oTransDetailRow.ExtendedPrice = 0;
                                            oTransDetailRow.IsPriceChanged = true;
                                            oTransDetailRow.IsPriceChangedByOverride = true;
                                            #region PRIMEPOS-3053 09-Feb-2021 JY Added
                                            try
                                            {
                                                if (oTransDetailRow.TaxAmount != 0)
                                                {
                                                    oTransDetailRow.TaxAmount = 0;
                                                    UpdateTransTaxDetails(oTDTaxData, oTransDetailRow.TransDetailID);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                logger.Fatal(ex, "WarnForDuplicateChargePosting() - 0");
                                            }
                                            #endregion
                                            oTransDetailRow.OverrideRemark = "Duplicate HC Posting";
                                            IsShowHCDialog = true; //PRIMEPOS-3264
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "WarnForDuplicateChargePosting() - 1");
                        }
                        retVal = false;
                        #endregion
                        //retVal = true;
                    }
                    //else if (sSelectedAction.Equals("P") && UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DuplicateChargePosting.ID, UserPriviliges.Permissions.DuplicateChargePosting.Name) == false)  //PRIMEPOS-3015 26-Oct-2021 JY Commented
                    else if (sSelectedAction.Equals("P"))   //PRIMEPOS-3015 26-Oct-2021 JY Added
                    {
                        retVal = true;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "WarnForDuplicateChargePosting() - 2");
                throw exp;
            }
            return retVal;
        }

        public DataSet PopulatePayTypeList()
        {

            DataSet oPayTypeData;
            try
            {
                PayType oPayType = new PayType();
                oPayTypeData = oPayType.PopulateList("");
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulatePayTypeList()");
                throw exp;
            }


            return oPayTypeData;
        }


        public bool isPrintAble(TransDetailData oData)
        {
            bool retValue = false;
            foreach (TransDetailRow oRow in oData.TransDetail.Rows)
            {
                Item oItem = new Item();
                if (oItem.Populate(oRow.ItemID).Item[0].ExclFromRecpt == false)
                {
                    retValue = true;
                    break;
                }
            }
            return retValue;
        }


        public string GetWarningMSGForHugeTransAmt(string amtSubTotal, string amtDiscount, string amtTax, string amtTotal)
        {
            string strMsg = string.Empty;
            try
            {
                if (Configuration.convertNullToDecimal(amtSubTotal) > 10000)
                {
                    /*Date 27-jan-2014
                     * Modified by Shitaljit
                     * For making currency symbol dynamic
                     */
                    //old code
                    //strMsg += "Transaction sub total is $" + this.txtAmtSubTotal.Text + "\n";
                    //new code
                    strMsg += "Transaction sub total is " + Configuration.CInfo.CurrencySymbol.ToString() + amtSubTotal + "\n";
                }
                if (Configuration.convertNullToDecimal(amtDiscount) > 10000)
                {
                    /*Date 27-jan-2014
                     * Modified by Shitaljit
                     * For making currency symbol dynamic
                     */
                    //old code
                    strMsg += "Transaction discount total is " + Configuration.CInfo.CurrencySymbol.ToString() + amtDiscount + "\n";
                    //new code
                    //strMsg += "Transaction discount total is $" + this.txtAmtDiscount.Text + "\n";
                }
                if (Configuration.convertNullToDecimal(amtTax) > 10000)
                {
                    /*Date 27-jan-2014
                    * Modified by Shitaljit
                    * For making currency symbol dynamic
                    */
                    //old code
                    //strMsg += "Transaction tax total is $" + this.txtAmtTax.Text + "\n";
                    //new code
                    strMsg += "Transaction tax total is " + Configuration.CInfo.CurrencySymbol.ToString() + amtTax + "\n";
                }
                if (Configuration.convertNullToDecimal(amtTotal) > 10000)
                {
                    /*Date 27-jan-2014
                    * Modified by Shitaljit
                    * For making currency symbol dynamic
                    */
                    //old code
                    //strMsg += "Transaction total is $" + this.txtAmtTotal.Text + "\n\n";
                    //new code
                    strMsg += "Transaction total is " + Configuration.CInfo.CurrencySymbol.ToString() + amtTotal + "\n\n";
                }

            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetWarningMSGForHugeTransAmt()");
                throw Ex;
            }
            return strMsg;
        }

        public decimal GetCurrentCashStatus()
        {
            decimal CurrentCashInDrawer;
            try
            {
                StationClose oStnClose = new StationClose();
                CurrentCashInDrawer = oStnClose.CurrentCashStatus("");
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCurrentCashStatus()");
                throw exp;
            }
            return CurrentCashInDrawer;


        }



        #region TaxCode

        public void GetSalePrice(ItemRow oItemRow, DepartmentData oDeptData, ref int extQty, ref bool isSaleAppy)
        {
            if (oItemRow.isOnSale == true && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)  //Sprint-26 - PRIMEPOS-2412 30-Aug-2017 JY Added SaleStartDate and SaleEndDate parameters
            {
                int setSalelimit = Configuration.convertNullToInt(oItemRow.SaleLimitQty);//Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013
                if (oItemRow.SaleStartDate != DBNull.Value || oItemRow.SaleEndDate != DBNull.Value)
                    if (DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                    {
                        //Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013
                        int existingQty = GetExistingQuantity(oTDRow, oTransDData);
                        if (existingQty >= setSalelimit && setSalelimit != 0)
                        {
                            extQty = oTDRow.QTY - setSalelimit;
                            oTDRow.QTY = setSalelimit;
                        }
                        else if (setSalelimit != 0)
                        {
                            extQty = oTDRow.QTY - existingQty;
                            oTDRow.QTY = existingQty;
                        }//Added by SRT(Abhishek) Date : 05/09/2009
                        if ((oTDRow.QTY <= setSalelimit) || setSalelimit == 0)
                        {
                            oTDRow.QTY = oTDRow.QTY;//Add by Ravindra (QuicSolv) PRIMEPOS-670 8 April 2013
                            oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oItemRow.OnSalePrice, 2);
                            // End of Added by SRT(Abhishek) Date : 05/09/2009
                            oTDRow.IsPriceChanged = true;
                            isSaleAppy = true;
                            oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                        }
                        else
                        {
                        }
                    }
            }
            #region Sprint-26 - PRIMEPOS-2412 29-Aug-2017 JY Added department sale price logic
            else
            {
                try
                {
                    if (oDeptData.Department.Rows.Count > 0 && DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate))
                    {
                        if (oDeptData.Department[0].SalePrice > 0)
                        {
                            if (oItemRow.SubDepartmentID != 0)
                            {
                                SubDepartmentData oSubDepartmentData = new SubDepartmentData();
                                SubDepartmentSvr oSubDepartmentSvr = new SubDepartmentSvr();
                                oSubDepartmentData = oSubDepartmentSvr.Populate(oItemRow.DepartmentID);

                                if (oSubDepartmentData != null && oSubDepartmentData.Tables.Count > 0 && oSubDepartmentData.Tables[0].Rows.Count > 0)
                                {
                                    if (Configuration.convertNullToBoolean(oSubDepartmentData.SubDepartment[0].IncludeOnSale))
                                    {
                                        oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oDeptData.Department[0].SalePrice, 2);
                                        oTDRow.IsPriceChanged = true;
                                        oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                    }
                                }
                            }
                            else
                            {
                                oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oDeptData.Department[0].SalePrice, 2);
                                oTDRow.IsPriceChanged = true;
                                oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "IsOnSaleToApplyTaxPolicy()");
                }
            }
            #endregion
            //Move up this line of code before Tax Code Logic is being apllieid by shitaljit on 5 April 2013
            //MEPOS-6686 sub-tasksBug in Tax calculation
            CalcExtdPrice(oTDRow);
            //Add by Ravindra For testing
            oTDRow.TaxID = oItemRow.TaxID;
            oTDRow.ItemDescription = oItemRow.Description;
        }

        public string GetTaxPolicyMessage(ItemRow oItemRow, decimal DicountAmt, string strTaxPolicy, bool isDeptHasDisc, bool bTaxChanged, ref bool bSellingPriceChanged, ref bool bDiscountChanged, ref bool bIsNonRefundableChanged)
        {
            string ConstMsg = "Do you want to Apply these changes on Current Transaction?";
            string MSG = string.Empty;

            #region PRIMEPOS-2500 03-Apr-2018 JY Commented
            //if (oTDRow.ItemDescription != oItemRow.Description && oTDRow.TaxID != oItemRow.TaxID && oTDRow.Price != oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) != DicountAmt)
            //{
            //    MSG = "You have changed the Description, SellingPrice, TaxCode, Item Discount.\n";
            //}
            //else if (oTDRow.ItemDescription != oItemRow.Description && oTDRow.TaxID != oItemRow.TaxID && (oTDRow.Discount / oTDRow.QTY) != DicountAmt && oTDRow.Price == oItemRow.SellingPrice)
            //{
            //    MSG = "You have changed the Description, TaxCode, Item Discount.\n";
            //}
            //else if (oTDRow.ItemDescription != oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price != oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) != DicountAmt)
            //{
            //    MSG = "You have changed the Description, SellingPrice, Item Discount.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID != oItemRow.TaxID && oTDRow.Price != oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) != DicountAmt)
            //{
            //    MSG = "You have changed the SellingPrice, TaxCode, Item Discount.\n";
            //}
            //else if (oTDRow.ItemDescription != oItemRow.Description && oTDRow.TaxID != oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) == DicountAmt)
            //{
            //    MSG = "You have changed the Description, TaxCode.\n";
            //}
            //else if (oTDRow.ItemDescription != oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price != oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) == DicountAmt)
            //{
            //    MSG = "You have changed the Description, SellingPrice.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID != oItemRow.TaxID && oTDRow.Price != oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) == DicountAmt)
            //{
            //    MSG = "You have changed the SellingPrice, TaxCode.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price != oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) != DicountAmt)
            //{
            //    MSG = "You have changed SellingPrice, Item Discount.\n";
            //}
            //else if (oTDRow.ItemDescription != oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) != DicountAmt)
            //{
            //    MSG = "You have changed the Description, Item Discount.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID != oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) != DicountAmt)
            //{
            //    MSG = "You have changed the TaxCode, Item Discount.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price != oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) == DicountAmt)
            //{
            //    MSG = "You have changed SellingPrice.\n";
            //}
            //else if (oTDRow.ItemDescription != oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) == DicountAmt)
            //{
            //    MSG = "You have changed the Description.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID != oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) == DicountAmt)
            //{
            //    MSG = "You have changed the TaxCode.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) != DicountAmt && oItemRow.DiscountPolicy != "D")
            //{
            //    MSG = "You have changed the Item Discount.\n";
            //}
            //  //Added By Shitaljit(QuicSolv) on 23 August 2011
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice && (oTDRow.Discount / oTDRow.QTY) == DicountAmt && strTaxPolicy != oItemRow.TaxPolicy)
            //{
            //    MSG = "You have changed the TaxPolicy.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice &&
            //      (oTDRow.Discount / oTDRow.QTY) == DicountAmt && strDiscountPolicy != oItemRow.DiscountPolicy && oItemRow.DiscountPolicy == "I" && oItemRow.Discount > 0)
            //{
            //    MSG = "You have changed the Discount Policy.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice &&
            //  (oTDRow.Discount / oTDRow.QTY) == DicountAmt && strDiscountPolicy != oItemRow.DiscountPolicy && oItemRow.DiscountPolicy == "D" && isDeptHasDisc == true)
            //{
            //    MSG = "You have changed the Discount Policy.\n";
            //}
            //else if (oTDRow.ItemDescription == oItemRow.Description && oTDRow.TaxID == oItemRow.TaxID && oTDRow.Price == oItemRow.SellingPrice &&
            //  (oTDRow.Discount / oTDRow.QTY) == DicountAmt && strDiscountPolicy != oItemRow.DiscountPolicy && CalculateDiscount(oItemRow.ItemID, oTDRow.QTY, oTDRow.Price) != (oTDRow.Discount / oTDRow.QTY))
            //{
            //    MSG = "You have changed the Discount Policy.\n";
            //}
            //else if ((oItem.IsItemOnSale(oItemRow, oDeptRow) && oItemRow.SaleLimitQty > 0) && oTDRow.Price != oItemRow.OnSalePrice)
            //{
            //    MSG = "You have changed the Item Sale Detail.\n";
            //}
            //else if (oItemRow.IsEBTItem != oTDRow.IsEBTItem)
            //{
            //    MSG = (oItemRow.IsEBTItem == true) ? "You have set item as EBT item.\n" : "You have set item as non-EBT item.\n";
            //}
            //else if (oItemRow.isOTCItem != oTDRow.IsMonitored)
            //{
            //    MSG = MSG = (oItemRow.isOTCItem == true) ? "You have set item as monitered item.\n" : "You have set item as non-monitered item.\n";
            //}
            //else
            //{
            //    MSG = "";
            //}
            //if (DicountAmt != (oTDRow.Discount / oTDRow.QTY) && string.IsNullOrEmpty(MSG) == true)
            //{
            //    MSG = "You have changed the Item Discount.\n";
            //}
            //if (MSG != "")
            //    MSG = MSG + ConstMsg;
            #endregion

            #region PRIMEPOS-2500 03-Apr-2018 JY Added optimized logic
            if (oTDRow.ItemDescription != oItemRow.Description)
            {
                MSG = "Description";
            }

            if (oTDRow.Price != oItemRow.SellingPrice)
            {
                //first check that is it mismatches due to sale price is in use
                Item oItem = new Item();
                if (oItem.IsItemOnSale(oItemRow, oDeptRow) && oItemRow.SaleLimitQty > 0)
                {
                    if (oTDRow.Price != oItemRow.OnSalePrice)
                    {
                        bSellingPriceChanged = true;
                        if (string.IsNullOrWhiteSpace(MSG))
                            MSG = "Sale Detail";
                        else
                            MSG += "," + " Sale Detail";
                    }
                }
                else
                {
                    bSellingPriceChanged = true;
                    if (string.IsNullOrWhiteSpace(MSG))
                        MSG = "SellingPrice";
                    else
                        MSG += "," + " SellingPrice";
                }
            }

            #region PRIMEPOS-2514 08-May-2018 JY commented
            //if (oTDRow.Price != oItemRow.OnSalePrice)
            //{
            //    Item oItem = new Item();
            //    if (oItem.IsItemOnSale(oItemRow, oDeptRow) && oItemRow.SaleLimitQty > 0)
            //    {
            //        if (string.IsNullOrWhiteSpace(MSG))
            //            MSG = "Sale Detail";
            //        else
            //            MSG += "," + " Sale Detail";
            //    }
            //}
            #endregion

            if (bTaxChanged == true)    // no need to consider TaxPolicy change as this flag will always return the accurate tax change - it might be because of policy change as well    
            {
                if (string.IsNullOrWhiteSpace(MSG))
                    MSG = "TaxCode";
                else
                    MSG += "," + " TaxCode";
            }

            if (!bSellingPriceChanged && ((oTDRow.Discount / oTDRow.QTY) != DicountAmt))
            {
                bDiscountChanged = true;
                if (string.IsNullOrWhiteSpace(MSG))
                    MSG = "Item Discount";
                else
                    MSG += "," + " Item Discount";
            }

            #region PRIMEPOS-2514 09-May-2018 JY commented - because we already calculated discount and below below never satisfies
            //if (strDiscountPolicy != oItemRow.DiscountPolicy)   
            //{
            //    if ((oItemRow.DiscountPolicy == "I" && oItemRow.Discount > 0) || (oItemRow.DiscountPolicy == "D" && isDeptHasDisc == true) || (CalculateDiscount(oItemRow.ItemID, oTDRow.QTY, oTDRow.Price) != (oTDRow.Discount / oTDRow.QTY)))
            //    {
            //        if (string.IsNullOrWhiteSpace(MSG))
            //            MSG = "Discount Policy";
            //        else
            //            MSG += "," + " Discount Policy";
            //    }
            //}
            #endregion

            if (oItemRow.IsEBTItem != oTDRow.IsEBTItem)
            {
                string strTemp = (oItemRow.IsEBTItem == true) ? "item as EBT item\n" : "item as non-EBT item";
                if (string.IsNullOrWhiteSpace(MSG))
                    MSG = strTemp;
                else
                    MSG += ", " + strTemp;
            }
            if (oItemRow.isOTCItem != oTDRow.IsMonitored)
            {
                string strTemp = (oItemRow.isOTCItem == true) ? "item as monitered item" : "item as non-monitered item";
                if (string.IsNullOrWhiteSpace(MSG))
                    MSG = strTemp;
                else
                    MSG += ", " + strTemp;
            }
            #region PRIMEPOS-2592 05-Nov-2018 JY Added 
            if (oItemRow.IsNonRefundable != oTDRow.IsNonRefundable)
            {
                bIsNonRefundableChanged = true;
                if (string.IsNullOrWhiteSpace(MSG))
                    MSG = clsPOSDBConstants.Item_Fld_IsNonRefundable;
                else
                    MSG += ", " + clsPOSDBConstants.Item_Fld_IsNonRefundable;
            }
            #endregion

            if (!string.IsNullOrWhiteSpace(MSG))
                MSG = "You have changed \"" + MSG + "\"" + Environment.NewLine + ConstMsg;
            #endregion

            return MSG;
        }

        public TaxCodesData PopulateTaxCodeAccrdTONewTaxPolicy(ref ItemRow oItemRow, ref bool isDeptHasDisc, out DepartmentData oDeptData, string strOldTaxcodes, ref bool bTaxChanged)
        {
            TaxCodesData oItemTaxCodesData = null;
            try
            {
                isDeptHasDisc = false;
                bool isDeptDiscount = false;
                oDeptData = new DepartmentData();
                if (oItemRow.DepartmentID != 0)
                {
                    bool bTaxableDept = false;
                    bool bDiscountDept = true;
                    Department oDept = new Department();
                    oDeptData = new DepartmentData();
                    oDeptData = oDept.Populate(oItemRow.DepartmentID);
                    if (oDeptData == null || oDeptData.Department.Rows.Count == 0)
                    {
                        if (Configuration.CInfo.DefaultDeptId > 0)
                        {
                            oItemRow.DepartmentID = Configuration.CInfo.DefaultDeptId;
                            oDeptData = oDept.Populate(oItemRow.DepartmentID);
                        }
                    }
                    if (oDeptData != null && oDeptData.Department.Rows.Count > 0)
                    {
                        bTaxableDept = oDeptData.Department[0].IsTaxable;
                        isDeptHasDisc = (oDeptData.Department[0].Discount > 0) ? true : false;
                        oDeptRow = oDeptData.Department[0];
                    }

                    isDeptDiscount = bDiscountDept;
                }
                oItemTaxCodesData = GetItemTaxData(oTDRow.ItemID);

                #region PRIMEPOS-2500 03-Apr-2018 JY Added logic to verify the tax change
                string strNewTaxCodes = string.Empty;
                if (Configuration.isNullOrEmptyDataSet(oItemTaxCodesData) == false && oItemTaxCodesData.Tables.Count > 0 && oItemTaxCodesData.Tables[0].Rows.Count > 0)
                {
                    foreach (TaxCodesRow tc in oItemTaxCodesData.TaxCodes.Rows)
                    {
                        if (strNewTaxCodes == string.Empty)
                            strNewTaxCodes = tc.TaxCode;
                        else
                            strNewTaxCodes += "," + tc.TaxCode;
                    }
                }

                var OldTaxcodes = strOldTaxcodes.Split(',').Select(t => t.Trim()).OrderBy(t => t);
                var NewTaxCodes = strNewTaxCodes.Split(',').Select(t => t.Trim()).OrderBy(t => t);
                bool stringsAreEqual = OldTaxcodes.SequenceEqual(NewTaxCodes, StringComparer.OrdinalIgnoreCase);
                if (!stringsAreEqual)
                {
                    bTaxChanged = true;
                }
                #endregion

                #region PRIMEPOS-2500 03-Apr-2018 JY commented as few scenarios were not working like it updates tax immediately without waiting for user confirmation, non-taxable to taxable not working
                ////Compare old tax codes of item to new taxcode
                //TaxCodesData oTransTaxCodeData = new TaxCodesData();
                //string sTaxCodes = string.Empty;
                //if (IsItemTaxableForTrasaction(oTDTaxData, oTDRow.ItemID, out sTaxCodes) == true)
                //{
                //    TaxCodes oTaxCodes = new TaxCodes();
                //    oTransTaxCodeData = oTaxCodes.PopulateList(" WHERE TaxID IN " + sTaxCodes);
                //    if (IsItemTaxInfoChanged(oItemTaxCodesData, oTransTaxCodeData) == true)
                //    {
                //        oTransTaxCodeData.Clear();
                //        oTransTaxCodeData.Merge(oItemTaxCodesData);
                //        oTDRow.TaxCode = string.Empty;
                //        foreach (TaxCodesRow tc in oTransTaxCodeData.TaxCodes.Rows)
                //        {
                //            oTDRow.TaxCode += tc.TaxCode + ",";
                //        }
                //        if (oTDRow.TaxCode.EndsWith(","))
                //        {
                //            oTDRow.TaxCode = oTDRow.TaxCode.Substring(0, oTDRow.TaxCode.Length - 1);
                //        }
                //        oTDRow.TaxID = 0;
                //        oItemRow.TaxID = 1;
                //    }
                //    else
                //    {
                //        oTDRow.TaxCode = string.Empty;
                //        foreach (TaxCodesRow tc in oTransTaxCodeData.TaxCodes.Rows)
                //        {
                //            oTDRow.TaxCode += tc.TaxCode + ",";
                //        }
                //        if (oTDRow.TaxCode.EndsWith(","))
                //        {
                //            oTDRow.TaxCode = oTDRow.TaxCode.Substring(0, oTDRow.TaxCode.Length - 1);
                //        }
                //        oTDRow.TaxID = 0;
                //        oItemRow.TaxID = 0;
                //    }
                //    UpdateTransTaxDetails(oTDTaxData, oTDRow.ItemID);
                //    UpdateTaxCode(oTDTaxData, oItemTaxCodesData, oTDRow.TransDetailID); //PRIMEPOS-2500 02-Apr-2018 JY Added "TransDetailID" parameter 
                //}
                //else if (Configuration.isNullOrEmptyDataSet(oItemTaxCodesData) == false)
                //{
                //    oTDRow.TaxID = 0;
                //    oItemRow.TaxID = 1;
                //}
                //else
                //{
                //    oTDRow.TaxID = 0;
                //    oItemRow.TaxID = 0;
                //}
                #endregion
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateTaxCodeAccrdTONewTaxPolicy()");
                throw exp;
            }

            return oItemTaxCodesData;
        }

        public bool IsItemTaxInfoChanged(TaxCodesData oItemTaxCodeData, TaxCodesData oTrnsTaxCodeData)
        {
            bool RetVal = false;
            try
            {
                var idsNotInB = oItemTaxCodeData.TaxCodes.AsEnumerable().Select(r => r.Field<int>(clsPOSDBConstants.TransDetail_Fld_TaxID))
                                .Except(oTrnsTaxCodeData.TaxCodes.AsEnumerable().Select(r => r.Field<int>(clsPOSDBConstants.TransDetail_Fld_TaxID)));
                DataTable TableC = (from row in oItemTaxCodeData.TaxCodes.AsEnumerable()
                                    join id in idsNotInB
                                    on row.Field<int>(clsPOSDBConstants.TransDetail_Fld_TaxID) equals id
                                    select row).CopyToDataTable();
                RetVal = !Configuration.isNullOrEmptyDataTable(TableC);
            }
            catch
            {
                if (oItemTaxCodeData.TaxCodes.Rows.Count == 0 || oTrnsTaxCodeData.TaxCodes.Rows.Count == 0

                    || oItemTaxCodeData.TaxCodes.Rows.Count != oTrnsTaxCodeData.TaxCodes.Rows.Count)
                {
                    RetVal = true;
                }
                else
                {
                    RetVal = false;
                }
            }
            return RetVal;
        }

        public void UpdateTransTaxCode(List<int> ListTaxIds, out TaxCodesData oTaxCodesData, int TransDetailID)
        {
            try
            {
                oTaxCodesData = new TaxCodesData();
                string sTaxIds = string.Empty;
                foreach (var item in ListTaxIds)
                {
                    sTaxIds += Configuration.convertNullToString(item) + ",";
                }
                if (sTaxIds.EndsWith(","))
                {
                    sTaxIds = sTaxIds.Substring(0, sTaxIds.Length - 1);
                }
                if (ListTaxIds.Count > 0)
                {
                    oTaxCodesData = PopulateTaxCodeList(" WHERE TaxID IN (" + sTaxIds + ")");
                }
                UpdateTransTaxDetails(oTDTaxData, oTDRow.ItemID);
                UpdateTaxCode(oTDTaxData, oTaxCodesData, TransDetailID);   //PRIMEPOS-2500 02-Apr-2018 JY Added "TransDetailID" parameter 

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "UpdateTransTaxCode()");
                throw exp;
            }
        }

        public void UpdateTaxCode(TransDetailTaxData oTDTaxData, TaxCodesData oTaxCodeData, int TransDetailID)
        {
            int rowIndex = 1;
            bool AddRow = false;
            int ItemRow = 1;

            UpdateTransTaxDetails(oTDTaxData, TransDetailID);   //PRIMEPOS-2500 02-Apr-2018 JY first need to delete existing TaxDetail records
            oTDRow.TaxCode = "";
            if (oTDTaxData.TransDetailTax.Rows.Count > 0)
            {
                ItemRow += Convert.ToInt32(oTDTaxData.TransDetailTax.Rows[oTDTaxData.TransDetailTax.Rows.Count - 1]["ItemRow"].ToString());
            }
            for (int i = 1; i <= oTaxCodeData.TaxCodes.Count; i++)
            {
                if (oTDTaxData.TransDetailTax.Rows.Count > 0 && !AddRow)
                {
                    rowIndex += Convert.ToInt32(oTDTaxData.TransDetailTax.Rows[oTDTaxData.TransDetailTax.Rows.Count - 1]["TransDetailTaxID"].ToString());
                }
                AddRow = true;
                oTDTaxData.TransDetailTax.AddRow(rowIndex, TransDetailID, 0, RoundTaxValue(Convert.ToDecimal(oTaxCodeData.TaxCodes.Rows[i - 1]["Amount"].ToString())),
                    0, oTDRow.ItemID, Convert.ToInt32(oTaxCodeData.TaxCodes.Rows[i - 1]["TaxID"].ToString()), ItemRow); //PRIMEPOS-2500 02-Apr-2018 JY passed correct TransDetailID and all records should be against single transaction hence passed TransId = 0
                rowIndex++;

                #region added logic to update TaxCode in TransDetail
                if (oTDRow.TaxCode == "")
                    oTDRow.TaxCode = Configuration.convertNullToString(oTaxCodeData.TaxCodes[i - 1]["TaxCode"]);
                else
                    oTDRow.TaxCode += "," + Configuration.convertNullToString(oTaxCodeData.TaxCodes[i - 1]["TaxCode"]);
                #endregion
                //if (oTDRow.TaxCode.EndsWith(","))
                //{
                //    oTDRow.TaxCode = oTDRow.TaxCode.Substring(0, oTDRow.TaxCode.Length - 1);
                //}
            }
        }

        public void ApplyTaxPolicy(ItemRow oItemRow, int TransDetailID)   //PRIMEPOS-2500 02-Apr-2018 JY removed "isDeptTaxable" parameter which computed in wrong way
        {
            logger.Trace("ApplyTaxPolicy() - " + clsPOSDBConstants.Log_Entering);
            ItemTax oTaxCodes = new ItemTax();
            oTaxCodesData = new TaxCodesData();
            int ItemRow = 1;

            if (oItemRow.TaxPolicy == "O" || oItemRow.TaxPolicy == String.Empty)
            {
                bool isDeptTaxable = false;
                Department oDepartment = new Department();
                if (Configuration.convertNullToInt(oItemRow.DepartmentID) > 0)
                    isDeptTaxable = oDepartment.IsTaxable(oItemRow.DepartmentID);

                if (isDeptTaxable == true)
                {
                    oTaxCodesData = oTaxCodes.PopulateTaxCodeData(Configuration.convertNullToString(oItemRow.DepartmentID), EntityType.Department);
                    //if department is taxable but no tax record found then we need to consider item tax policy
                    if (Configuration.isNullOrEmptyDataSet(oTaxCodesData) == true && oItemRow.isTaxable == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                    }
                }
                else if (oItemRow.isTaxable == true)
                {
                    oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                }
            }
            else if (oItemRow.TaxPolicy == "D")
            {
                bool isDeptTaxable = false;
                Department oDepartment = new Department();
                if (Configuration.convertNullToInt(oItemRow.DepartmentID) > 0)
                    isDeptTaxable = oDepartment.IsTaxable(oItemRow.DepartmentID);

                if (isDeptTaxable == true)
                {
                    oTaxCodesData = oTaxCodes.PopulateTaxCodeData(Configuration.convertNullToString(oItemRow.DepartmentID), EntityType.Department);
                }
            }
            else if (oItemRow.TaxPolicy == "I")
            {
                if (oItemRow.isTaxable == true)
                {
                    oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                }
            }

            if (Configuration.isNullOrEmptyDataSet(oTaxCodesData) == false)
            {
                ///oPOSTrans.CalculateTax(oTDRow, oTaxCodesData, oTDTaxData);
                ///Fix by Manoj 5/27/2015
                int rowIndex = 1;
                bool AddRow = false;
                UpdateTransTaxDetails(oTDTaxData, TransDetailID);   //PRIMEPOS-2500 05-Apr-2018 JY first need to delete existing TaxDetail records
                if (oTDTaxData.TransDetailTax.Rows.Count > 0)
                {
                    ItemRow += Convert.ToInt32(oTDTaxData.TransDetailTax.Rows[oTDTaxData.TransDetailTax.Rows.Count - 1]["ItemRow"].ToString());
                }
                oTDRow.TaxCode = "";
                for (int i = 1; i <= oTaxCodesData.Tables[0].Rows.Count; i++)
                {
                    if (oTDTaxData.TransDetailTax.Rows.Count > 0 && !AddRow)
                    {
                        rowIndex += Convert.ToInt32(oTDTaxData.TransDetailTax.Rows[oTDTaxData.TransDetailTax.Rows.Count - 1]["TransDetailTaxID"].ToString());
                    }
                    AddRow = true;
                    oTDTaxData.TransDetailTax.AddRow(rowIndex, TransDetailID, 0, RoundTaxValue(Convert.ToDecimal(oTaxCodesData.Tables[0].Rows[i - 1]["Amount"].ToString())),
                        0, oTDRow.ItemID, Convert.ToInt32(oTaxCodesData.Tables[0].Rows[i - 1]["TaxID"].ToString()), ItemRow);
                    rowIndex++;

                    #region added logic to update TaxCode in TransDetail
                    if (oTDRow.TaxCode == "")
                        oTDRow.TaxCode = Configuration.convertNullToString(oTaxCodesData.TaxCodes[i - 1]["TaxCode"]);
                    else
                        oTDRow.TaxCode += "," + Configuration.convertNullToString(oTaxCodesData.TaxCodes[i - 1]["TaxCode"]);
                    #endregion
                    //if (oTDRow.TaxCode.EndsWith(","))
                    //{
                    //    oTDRow.TaxCode = oTDRow.TaxCode.Substring(0, oTDRow.TaxCode.Length - 1);
                    //}
                }

                //oPOSTrans.CalculateTax(oTDRow, oTaxCodesData, oTDTaxData);
            }
            else
            {
                oTDRow.TaxCode = String.Empty;
                oTDRow.TaxID = 0;
                oTDRow.TaxAmount = 0;
                UpdateTransTaxDetails(oTDTaxData, oTDRow.ItemID);
            }
            logger.Trace("ApplyTaxPolicy() - " + clsPOSDBConstants.Log_Exiting);
        }

        public TaxCodesData PopulateTaxCodeList(string whereCondn)
        {
            TaxCodesData oTaxCodesData = null;
            try
            {
                TaxCodes oTaxCodes = new TaxCodes();
                oTaxCodesData = oTaxCodes.PopulateList(whereCondn);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateTaxCodeList()");
                throw Ex;
            }
            return oTaxCodesData;
        }

        #endregion
        public bool IsInventoryLow(string ItemCode, out int RoeorderLevel, out int QtyInHand)
        {
            bool RetVal = false;
            RoeorderLevel = 0;
            QtyInHand = 0;
            Item oItem = new Item();
            ItemData oItemData;
            ItemRow oItemRow = null;

            try
            {
                oItemData = oItem.Populate(ItemCode);
                if (oItemData != null && oItemData.Item.Rows.Count > 0)
                {
                    oItemRow = oItemData.Item[0];
                    if (oItemRow.QtyInStock <= 0 || oItemRow.QtyInStock <= oItemRow.ReOrderLevel)
                    {
                        RoeorderLevel = oItemRow.ReOrderLevel;
                        QtyInHand = oItemRow.QtyInStock;
                        RetVal = true;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsInventoryLow()");
                throw exp;
            }
            return RetVal;
        }
        public void SetLoyalityPointForTransaction()
        {
            try
            {
                if (Configuration.CLoyaltyInfo.UseCustomerLoyalty == true)
                {
                    TransDetailRow orignalRow = null;
                    foreach (TransDetailRow row in oTransDData.TransDetail.Rows)
                    {
                        orignalRow = oTransDData.TransDetail.FindRow(row.TransDetailID); //row.ItemID
                        if (orignalRow != null)
                        {
                            row.LoyaltyPoints = orignalRow.LoyaltyPoints;
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SetLoyalityPointForTransaction()");
                throw exp;
            }
        }

        public RxLabel GetRXLabelForRecvOnAcct(bool isCLTierreached, decimal CLCouponValue)
        {
            RxLabel oRxLabel;
            try
            {

                DataTable dtTransDetailTax = GetTransDetailTax(Configuration.convertNullToInt(((TransHeaderRow)oTransHData.TransHeader.Rows[0]).TransID)); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    if (oTransDRXData != null && oTransDRXData.Tables.Count > 0 && oTransDRXData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                    {
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oPOSTransPaymentData, oTransDRXData, ReceiptType.ReceiveOnAccount, dtTransDetailTax);
                    }  // end of added by atul 07-jan-2011
                    else
                    {
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oPOSTransPaymentData, ReceiptType.ReceiveOnAccount, dtTransDetailTax);
                    }
                }
                else
                {
                    oRxLabel = new RxLabel(oTransHData, oTransDData, oPOSTransPaymentData, ReceiptType.ReceiveOnAccount, dtTransDetailTax);
                }
                oRxLabel.isCLTierreached = isCLTierreached;
                oRxLabel.CLCouponValue = CLCouponValue; //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetRXLabelForRecvOnAcct()");
                throw exp;
            }
            return oRxLabel;
        }

        public RxLabel GetRxLabelForPrinting(bool bPrintGiftRecpt, bool isCLTierreached, decimal CLCouponValue)
        {
            RxLabel oRxLabel;
            try
            {
                #region PRIMEPOS-2939 05-Mar-2021 JY Added
                if (oTransHData != null && oTransHData.Tables.Count > 0 && oTransHData.TransHeader.Rows.Count > 0)
                {
                    POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();
                    oPOSTransPaymentData = oTransPaymentSvr.Populate(Configuration.convertNullToInt(oTransHData.TransHeader.Rows[0]["TransID"]));
                }
                #endregion
                DataTable PatientTable = null;
                if (oRXHeaderList != null && oRXHeaderList.Count > 0)
                    PatientTable = oRXHeaderList[0].TblPatient.Copy();

                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    //Added By Amit Date 26 Nov 2011
                    //SetDescForTransData();    //PRIMEPOS-2884 21-Aug-2020 JY Commented
                    //End
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " GetRxLabelForPrinting - Initiating printing receipt");
                    TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                    DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(Configuration.convertNullToInt(((TransHeaderRow)oTransHData.TransHeader.Rows[0]).TransID)); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                    DataTable dtTaxCode = oTransDetailTaxSvr.GetTaxCodeDetail(Configuration.convertNullToInt(((TransHeaderRow)oTransHData.TransHeader.Rows[0]).TransID));
                    if (oTransDRXData != null && oTransDRXData.Tables.Count > 0 && oTransDRXData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                    {
                        logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " GetRxLabelForPrinting - About to get RXLabel");
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oPOSTransPaymentData, oTransDRXData, (CurrentTransactionType == POSTransactionType.Sales) ? ReceiptType.SalesTransaction : ReceiptType.ReturnTransaction, dtTransDetailTax, false, PatientTable);
                        oRxLabel.bPrintGiftReciept = bPrintGiftRecpt; //Added By Shitaljit for printing gift reciept on 21 Jan 2013
                        oRxLabel.isCLTierreached = isCLTierreached;
                        oRxLabel.CLCouponValue = CLCouponValue; //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value
                        //SetParameters(oRxLabel, oPOSTransPaymentData);    //PRIMEPOS-2939 03-Mar-2021 JY Commented
                        oRxLabel.dtTax = dtTaxCode;//2664
                    }  // end of added by atul 07-jan-2011
                    else
                    {
                        logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " GetRxLabelForPrinting - About to get RXLabel");
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oPOSTransPaymentData, (CurrentTransactionType == POSTransactionType.Sales) ? ReceiptType.SalesTransaction : ReceiptType.ReturnTransaction, dtTransDetailTax);
                        oRxLabel.bPrintGiftReciept = bPrintGiftRecpt; //Added By Shitaljit for printing gift reciept on 21 Jan 2013
                        oRxLabel.isCLTierreached = isCLTierreached;
                        oRxLabel.CLCouponValue = CLCouponValue; //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value
                        //SetParameters(oPOSTransPaymentData,ref oRxLabel);    //PRIMEPOS-2939 03-Mar-2021 JY Commented//2664 Uncommented Arvind
                        oRxLabel.dtTax = dtTaxCode;//2664
                    }
                }
                else
                {
                    TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                    DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(Configuration.convertNullToInt(((TransHeaderRow)oTransHData.TransHeader.Rows[0]).TransID)); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - printing reciept (Last else)");
                    oRxLabel = new RxLabel(oTransHData, oTransDData, oPOSTransPaymentData, (CurrentTransactionType == POSTransactionType.Sales) ? ReceiptType.SalesTransaction : ReceiptType.ReturnTransaction, dtTransDetailTax);
                    oRxLabel.isCLTierreached = isCLTierreached;
                    oRxLabel.CLCouponValue = CLCouponValue; //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value
                    //SetParameters(oRxLabel, oPOSTransPaymentData);    //PRIMEPOS-2939 03-Mar-2021 JY Commented
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetRxLabelForPrinting()");
                throw exp;
            }
            return oRxLabel;
        }

        #region PRIMEPOS-2939 03-Mar-2021 JY Commented
        private void SetParameters(RxLabel oRxLabel, POSTransPaymentData oTransPaymentData)
        {
            try
            {
                if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                {
                    oRxLabel.AuthNo = oRxLabel.CCPaymentRow.AuthNo;
                    oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ProcessorTransID;
                    oRxLabel.Trace = oRxLabel.CCPaymentRow.TraceNumber;
                    oRxLabel.Batch = oRxLabel.CCPaymentRow.BatchNumber;
                    oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID;
                    oRxLabel.InvoiceNumber = oRxLabel.CCPaymentRow.InvoiceNumber;
                    oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                    oRxLabel.IsEvertecForceTransaction = oRxLabel.CCPaymentRow.IsEvertecForceTransaction; //primepos-2831
                    oRxLabel.IsEvertecSign = oRxLabel.CCPaymentRow.IsEvertecSign; //primepos-2831
                    if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                    {
                        oRxLabel.EvertecStateTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        oRxLabel.EvertecCityTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                    }
                    oRxLabel.EvertecCashback = oRxLabel.CCPaymentRow.CashBack;//PRIMEPOS-2857
                    oRxLabel.EntryMethod = oRxLabel.CCPaymentRow.EntryMethod;//PRIMEPOS-2857
                    oRxLabel.ControlNumber = oRxLabel.CCPaymentRow.ControlNumber;
                    if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.ATHMovil))
                        oRxLabel.ATHMovil = oRxLabel.CCPaymentRow.ATHMovil.Substring(2, oRxLabel.CCPaymentRow.ATHMovil.Length - 2);//2664
                }
                if (oRxLabel.ATHPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")//2664
                {
                    oRxLabel.AuthNo = oRxLabel.ATHPaymentRow.AuthNo;
                    oRxLabel.ReferenceNumber = oRxLabel.ATHPaymentRow.ProcessorTransID;
                    oRxLabel.Trace = oRxLabel.ATHPaymentRow.TraceNumber;
                    oRxLabel.Batch = oRxLabel.ATHPaymentRow.BatchNumber;
                    oRxLabel.MerchantID = oRxLabel.ATHPaymentRow.MerchantID;
                    oRxLabel.InvoiceNumber = oRxLabel.ATHPaymentRow.InvoiceNumber;
                    oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                    oRxLabel.IsEvertecForceTransaction = oRxLabel.ATHPaymentRow.IsEvertecForceTransaction; //primepos-2831
                    oRxLabel.IsEvertecSign = oRxLabel.ATHPaymentRow.IsEvertecSign; //primepos-2831
                    if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                    {
                        oRxLabel.EvertecStateTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        oRxLabel.EvertecCityTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                    }
                    oRxLabel.EvertecCashback = oRxLabel.ATHPaymentRow.CashBack;//PRIMEPOS-2857
                    oRxLabel.EntryMethod = oRxLabel.ATHPaymentRow.EntryMethod;//PRIMEPOS-2857
                    oRxLabel.ControlNumber = oRxLabel.ATHPaymentRow.ControlNumber;
                    if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.ATHMovil))
                        oRxLabel.ATHMovil = oRxLabel.ATHPaymentRow.ATHMovil.Substring(2, oRxLabel.ATHPaymentRow.ATHMovil.Length - 2);//2664
                }

                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                {
                    if (oRxLabel.CashPaymentRow != null)
                    {
                        oRxLabel.AuthNo = oRxLabel.CashPaymentRow.AuthNo;
                        oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ProcessorTransID;
                        oRxLabel.Trace = oRxLabel.CashPaymentRow.TraceNumber;
                        oRxLabel.Batch = oRxLabel.CashPaymentRow.BatchNumber;
                        oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
                        oRxLabel.InvoiceNumber = oRxLabel.CashPaymentRow.InvoiceNumber;
                        oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                        oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                        oRxLabel.IsEvertecForceTransaction = oRxLabel.CashPaymentRow.IsEvertecForceTransaction; //primepos-2831
                        oRxLabel.IsEvertecSign = oRxLabel.CashPaymentRow.IsEvertecSign; //primepos-2831
                        oRxLabel.ControlNumber = oRxLabel.CashPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            oRxLabel.EvertecCityTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        oRxLabel.EntryMethod = oRxLabel.CashPaymentRow.EntryMethod;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.CashPaymentRow.ControlNumber;//PRIMEPOS-2857
                        oRxLabel.EvertecCashback = oRxLabel.CashPaymentRow.CashBack;//PRIMEPOS-2857
                        if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.ATHMovil))
                            oRxLabel.ATHMovil = oRxLabel.CashPaymentRow.ATHMovil.Substring(2, oRxLabel.CashPaymentRow.ATHMovil.Length - 2);//2664
                    }
                    else if (oRxLabel.CheckPaymentRow != null)
                    {
                        oRxLabel.ControlNumber = oRxLabel.CheckPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(oRxLabel.CheckPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.CheckPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            oRxLabel.EvertecCityTax = oRxLabel.CheckPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        oRxLabel.EntryMethod = oRxLabel.CheckPaymentRow.EntryMethod;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.CheckPaymentRow.ControlNumber;//PRIMEPOS-2857
                        oRxLabel.ATHMovil = oRxLabel.CheckPaymentRow.ATHMovil;//2664
                    }
                    else if (oRxLabel.CouponPaymentRow != null)
                    {
                        if (!string.IsNullOrWhiteSpace(oRxLabel.CouponPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            oRxLabel.EvertecCityTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        oRxLabel.EntryMethod = oRxLabel.CouponPaymentRow.EntryMethod;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.CouponPaymentRow.ControlNumber;//PRIMEPOS-2857
                        oRxLabel.ATHMovil = oRxLabel.CouponPaymentRow.ATHMovil;//2664
                    }
                    else if (oRxLabel.HCPaymentRow != null)
                    {
                        oRxLabel.ControlNumber = oRxLabel.HCPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(oRxLabel.HCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            oRxLabel.EvertecCityTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        oRxLabel.EntryMethod = oRxLabel.HCPaymentRow.EntryMethod;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.HCPaymentRow.ControlNumber;//PRIMEPOS-2857
                        oRxLabel.ATHMovil = oRxLabel.HCPaymentRow.ATHMovil;//2664
                    }
                    else if (oRxLabel.CBPaymentRow != null)
                    {
                        oRxLabel.ControlNumber = oRxLabel.CBPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(oRxLabel.CBPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            oRxLabel.EvertecCityTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        oRxLabel.EntryMethod = oRxLabel.CBPaymentRow.EntryMethod;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.CBPaymentRow.ControlNumber;//PRIMEPOS-2857
                        oRxLabel.ATHMovil = oRxLabel.CBPaymentRow.ATHMovil;//2664
                    }
                    else if (oRxLabel.EBTPaymentRow != null)
                    {
                        if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            oRxLabel.EvertecCityTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        oRxLabel.IsEvertecForceTransaction = oRxLabel.EBTPaymentRow.IsEvertecForceTransaction; //primepos-2831
                        oRxLabel.IsEvertecSign = oRxLabel.EBTPaymentRow.IsEvertecSign; //primepos-2831
                        oRxLabel.EvertecCashback = oRxLabel.EBTPaymentRow.CashBack;//PRIMEPOS-2857
                        oRxLabel.EntryMethod = oRxLabel.EBTPaymentRow.EntryMethod;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.EBTPaymentRow.ControlNumber;//PRIMEPOS-2857
                        if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.ATHMovil))
                            oRxLabel.ATHMovil = oRxLabel.EBTPaymentRow.ATHMovil.Substring(2, oRxLabel.EBTPaymentRow.ATHMovil.Length - 2);//2664
                    }
                }
                else if (oRxLabel.EBTPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                {
                    oRxLabel.AuthNo = oRxLabel.EBTPaymentRow.AuthNo;
                    oRxLabel.ReferenceNumber = oRxLabel.EBTPaymentRow.ProcessorTransID;
                    oRxLabel.Trace = oRxLabel.EBTPaymentRow.TraceNumber;
                    oRxLabel.Batch = oRxLabel.EBTPaymentRow.BatchNumber;
                    oRxLabel.MerchantID = oRxLabel.EBTPaymentRow.MerchantID;
                    oRxLabel.InvoiceNumber = oRxLabel.EBTPaymentRow.InvoiceNumber;
                    oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                    #region PRIMEPOS-2664 EVERTEC EBTBALANCE
                    if (oRxLabel.EBTPaymentRow.EbtBalance.Length >= 3)
                    {
                        oRxLabel.FoodBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[0];
                        oRxLabel.CashBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[1];
                    }
                    #endregion
                    oRxLabel.IsEvertecForceTransaction = oRxLabel.EBTPaymentRow.IsEvertecForceTransaction; //primepos-2831
                    oRxLabel.IsEvertecSign = oRxLabel.EBTPaymentRow.IsEvertecSign; //primepos-2831
                    if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.EvertecTaxBreakdown))//PRIMEPOS-2857
                    {
                        oRxLabel.EvertecStateTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        oRxLabel.EvertecCityTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                    }
                    oRxLabel.EvertecCashback = oRxLabel.EBTPaymentRow.CashBack;//PRIMEPOS-2857
                    oRxLabel.ControlNumber = oRxLabel.EBTPaymentRow.ControlNumber;//PRIMEPOS-2857
                    if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.ATHMovil))
                        oRxLabel.ATHMovil = oRxLabel.EBTPaymentRow.ATHMovil.Substring(2, oRxLabel.EBTPaymentRow.ATHMovil.Length - 2);//2664
                }
                //oRxLabel.ControlNumber = this.controlNumber;
                //Added by Arvind PRIMEPOS-2636
                if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")
                {
                    oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ReferenceNumber;
                    oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID;
                    oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    oRxLabel.TerminalID = oRxLabel.CCPaymentRow.TerminalID;
                    oRxLabel.TransactionID = oRxLabel.CCPaymentRow.TransactionID;
                    oRxLabel.ResponseCode = oRxLabel.CCPaymentRow.ResponseCode;
                    oRxLabel.Aid = oRxLabel.CCPaymentRow.Aid;
                    oRxLabel.Cryptogram = oRxLabel.CCPaymentRow.Cryptogram;
                    oRxLabel.EntryMethod = oRxLabel.CCPaymentRow.EntryMethod;
                    oRxLabel.ApprovalCode = oRxLabel.CCPaymentRow?.ApprovalCode;
                    #region PRIMEPOS-2793
                    oRxLabel.ApplicationLabel = oRxLabel.CCPaymentRow?.ApplicaionLabel;
                    oRxLabel.PinVerified = oRxLabel.CCPaymentRow.PinVerified;
                    oRxLabel.LaneID = oRxLabel.CCPaymentRow.LaneID;
                    oRxLabel.CardLogo = oRxLabel.CCPaymentRow.CardLogo;
                    #endregion
                }
                else if (oRxLabel.CashPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")
                {
                    oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ReferenceNumber;
                    oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
                    oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    oRxLabel.TerminalID = oRxLabel.CashPaymentRow.TerminalID;
                    oRxLabel.TransactionID = oRxLabel.CashPaymentRow.TransactionID;
                    oRxLabel.ResponseCode = oRxLabel.CashPaymentRow.ResponseCode;
                    oRxLabel.Aid = oRxLabel.CashPaymentRow.Aid;
                    oRxLabel.Cryptogram = oRxLabel.CashPaymentRow.Cryptogram;
                    oRxLabel.EntryMethod = oRxLabel.CashPaymentRow.EntryMethod.ToUpper();
                    #region PRIMEPOS-2793
                    oRxLabel.ApplicationLabel = oRxLabel.CashPaymentRow?.ApplicaionLabel;
                    oRxLabel.PinVerified = oRxLabel.CashPaymentRow.PinVerified;
                    oRxLabel.LaneID = oRxLabel.CashPaymentRow.LaneID;
                    oRxLabel.CardLogo = oRxLabel.CashPaymentRow.CardLogo;
                    #endregion
                }
                else if (oRxLabel.EBTPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")
                {
                    oRxLabel.ReferenceNumber = oRxLabel.EBTPaymentRow.ReferenceNumber;
                    oRxLabel.MerchantID = oRxLabel.EBTPaymentRow.MerchantID;
                    oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    oRxLabel.TerminalID = oRxLabel.EBTPaymentRow.TerminalID;
                    oRxLabel.TransactionID = oRxLabel.EBTPaymentRow.TransactionID;
                    oRxLabel.ResponseCode = oRxLabel.EBTPaymentRow.ResponseCode;
                    oRxLabel.Aid = oRxLabel.EBTPaymentRow.Aid;
                    oRxLabel.Cryptogram = oRxLabel.EBTPaymentRow.Cryptogram;
                    oRxLabel.EntryMethod = oRxLabel.EBTPaymentRow.EntryMethod;
                    oRxLabel.ApprovalCode = oRxLabel.EBTPaymentRow?.ApprovalCode;
                    #region PRIMEPOS-2793
                    oRxLabel.ApplicationLabel = oRxLabel.EBTPaymentRow?.ApplicaionLabel;
                    oRxLabel.PinVerified = oRxLabel.EBTPaymentRow.PinVerified;
                    oRxLabel.LaneID = oRxLabel.EBTPaymentRow.LaneID;
                    oRxLabel.CardLogo = oRxLabel.EBTPaymentRow.CardLogo;
                    #endregion
                }

                if (oTransPaymentData != null && oTransPaymentData.Tables.Count > 0 && oTransPaymentData.Tables[0].Rows.Count > 0)
                {
                    foreach (POSTransPaymentRow oRow in oTransPaymentData.POSTransPayment.Rows)
                    {
                        if (Configuration.convertNullToString(oRow.PaymentProcessor).Trim() != "" && oRow.PaymentProcessor.Trim() != "N/A")
                        {
                            oRxLabel.TicketNumber = Configuration.convertNullToString(oRow.TicketNumber).Trim();
                            if (oRxLabel.ReferenceNumber.Trim() == "") oRxLabel.ReferenceNumber = Configuration.convertNullToString(oRow.ReferenceNumber).Trim();
                            if (oRxLabel.TransactionID.Trim() == "") oRxLabel.TransactionID = Configuration.convertNullToString(oRow.TransactionID).Trim();
                            break;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "SetParameters(RxLabel oRxLabel, POSTransPaymentData oTransPaymentData)");
            }
        }
        #endregion

        public void OpenDrawerOnTransComplete(RxLabel oRxLabel)
        {
            logger.Trace("OpenDrawerOnTransComplete() - " + clsPOSDBConstants.Log_Entering);
            bool canOpenDrawer = IsOpenDrawer(oPOSTransPaymentData.POSTransPayment);
            if (canOpenDrawer == true || Configuration.CInfo.DoNotOpenDrawerForCCOnlyTrans == false)
            {
                logger.Trace("OpenDrawerOnTransComplete() - About to open the drawer");
                oRxLabel.OpenDrawer();
                logger.Trace("OpenDrawerOnTransComplete() - Finish opening the drawer");
            }
            logger.Trace("OpenDrawerOnTransComplete() - " + clsPOSDBConstants.Log_Exiting);
        }



        public void SetSalePrice(ItemRow oItemRow, DepartmentData oDeptData)
        {
            #region Item sale price logic
            if (oItemRow.isOnSale == true && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)  //Sprint-26 - PRIMEPOS-2412 30-Aug-2017 JY Added SaleStartDate and SaleEndDate parameters
            {
                int iSalelimitQty = Configuration.convertNullToInt(oItemRow.SaleLimitQty);
                int existingQty = GetExistingQuantity(oTDRow, oTransDData);
                if (existingQty + oTDRow.QTY <= iSalelimitQty || iSalelimitQty == 0) //apply sale price for all quantity
                {
                    oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oItemRow.OnSalePrice, 2);
                    oTDRow.IsPriceChanged = true;
                    oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                }
                else
                {
                    if (Configuration.CInfo.GroupTransItems == true)
                    {
                        decimal FinalPrice = Math.Round((((iSalelimitQty * Math.Round(oItemRow.OnSalePrice) + ((existingQty + oTDRow.QTY - iSalelimitQty) * oTDRow.OrignalPrice))) / (existingQty + oTDRow.QTY)), 2);   //if we chage this will creates multiple records
                        oTDRow.NonComboUnitPrice = oTDRow.Price = FinalPrice;
                        oTDRow.IsOnSale = false;    //PRIMEPOS-2907 13-Oct-2020 JY Added
                    }
                    else
                    {
                    }
                    oTDRow.IsPriceChanged = true;
                }
            }
            #endregion
            #region Department sale price 
            else
            {
                try
                {
                    if (oDeptData.Department.Rows.Count > 0 && DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate))
                    {
                        if (oDeptData.Department[0].SalePrice > 0)
                        {
                            #region Sprint-26 - PRIMEPOS-2412 29-Aug-2017 JY Added logic to consider subdepartment                                            
                            if (oItemRow.SubDepartmentID != 0)
                            {
                                SubDepartmentData oSubDepartmentData = new SubDepartmentData();
                                SubDepartmentSvr oSubDepartmentSvr = new SubDepartmentSvr();
                                oSubDepartmentData = oSubDepartmentSvr.Populate(oItemRow.DepartmentID);

                                if (oSubDepartmentData != null && oSubDepartmentData.Tables.Count > 0 && oSubDepartmentData.Tables[0].Rows.Count > 0)
                                {
                                    if (Configuration.convertNullToBoolean(oSubDepartmentData.SubDepartment[0].IncludeOnSale))
                                    {
                                        oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oDeptData.Department[0].SalePrice, 2);
                                        oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                        oTDRow.IsPriceChanged = true;
                                    }
                                }
                            }
                            else
                            {
                                //Added by SRT(Abhishek) Date : 05/09/2009
                                oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oDeptData.Department[0].SalePrice, 2);
                                //End of Added by SRT(Abhishek) Date : 05/09/2009
                                oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                oTDRow.IsPriceChanged = true;
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "SetSalePrice()");
                    throw exp;
                }
                #endregion
            }
            #endregion
        }

        public decimal GetDiscount(decimal discount)
        {
            discount = Math.Round(discount, 2);

            if (CurrentTransactionType == POSTransactionType.SalesReturn)
            {
                if (discount > 0)
                    return -1 * discount;
                else
                    return discount;
            }
            else
                return discount;
        }

        public bool AllowDiscount(string ItemID)
        {
            Item oItem = new Item();
            ItemData oIData = new ItemData();
            oIData = oItem.Populate(ItemID);
            if (oIData.Item.Rows.Count == 0)
                return false;
            else
            {
                return oIData.Item[0].isDiscountable;
            }
        }
        public System.Decimal GetIIASRxTotal()
        {
            logger.Trace("GetIIASRxTotal() - " + clsPOSDBConstants.Log_Entering);
            decimal dIIASRxTotal = 0;
            try
            {

                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    if (oRow.IsIIAS == true && (oRow.IsRxItem == true || oRow.ItemID.ToUpper().Trim() == "RX"))
                    {
                        dIIASRxTotal += oRow.ExtendedPrice + oRow.TaxAmount - oRow.Discount;
                    }
                }
                //Added by SRT(Abhishek) Date : 05/09/2009
                //Added to round up total up to 2 digits only while sending it to the PayType List screen
                dIIASRxTotal = Math.Round(dIIASRxTotal, 2);
                //End Of Added by SRT(Abhishek) Date : 05/09/2009
                logger.Trace("GetIIASRxTotal() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetIIASRxTotal()");
                throw exp;
            }
            return dIIASRxTotal;
        }

        public decimal GetEBTTotal(out Decimal TotalEBTItemsTax)
        {
            logger.Trace("GetEBTTotal() - " + clsPOSDBConstants.Log_Entering);
            decimal dEBTTotal = 0;
            try
            {

                TotalEBTItemsTax = 0;
                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    if (oRow.IsEBTItem == true)
                    {
                        //dEBTTotal += oRow.ExtendedPrice + oRow.TaxAmount - oRow.Discount;
                        //Modified by shitaljit on 26Dec2013 for PRIMEPOS-1627 Remove Tax on EBT Transaction
                        dEBTTotal += oRow.ExtendedPrice - oRow.Discount;
                        TotalEBTItemsTax += oRow.TaxAmount;
                    }
                }
                dEBTTotal = Math.Round(dEBTTotal, 2);
                logger.Trace("GetEBTTotal() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetEBTTotal()");
                throw exp;
            }
            return dEBTTotal;
        }


        public decimal GetIIASTotal()
        {
            logger.Trace("GetIIASTotal() - " + clsPOSDBConstants.Log_Entering);
            decimal dIIASTotal = 0;
            try
            {

                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    if (oRow.IsIIAS == true)
                    {
                        dIIASTotal += oRow.ExtendedPrice + oRow.TaxAmount - oRow.Discount;
                    }
                }
                //Added by SRT(Abhishek) Date : 05/09/2009
                //Added to round up total up to 2 digits only while sending it to the PayType List screen
                dIIASTotal = Math.Round(dIIASTotal, 2);
                //End Of Added by SRT(Abhishek) Date : 05/09/2009
                logger.Trace("GetIIASTotal() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetIIASTotal()");
                throw exp;
            }
            return dIIASTotal;
        }
        public decimal RecalculateDiscount(TransDetailRow oRow)
        {
            logger.Trace("RecalculateDiscount() - " + clsPOSDBConstants.Log_Entering);
            decimal returnValue = 0;
            decimal OriginalDisc = 0;
            OriginalDisc = CalculateDiscount(oRow.ItemID, oTDRow.QTY, oTDRow.Price);
            if (OriginalDisc == 0 && oTDRow.Discount != 0)
            {
                clsCoreUIHelper.ShowWarningMsg("As a result of price override line discount of " + Configuration.CInfo.CurrencySymbol + oTDRow.Discount + " for Item# " + oRow.ItemID + " will be discarded.\nPlease apply the desire discount again.", "Discard Line Item Discount");
            }
            else if (OriginalDisc != oTDRow.Discount)
            {
                returnValue = OriginalDisc;
            }
            logger.Trace("RecalculateDiscount() - " + clsPOSDBConstants.Log_Exiting);
            return returnValue;
        }

        public decimal CalculateDiscount(string itemID, int QTY, decimal price)
        {
            logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Entering);
            decimal returnValue = 0;
            bool isDeptSale = false;//added by Krishna on 28 December 2011
            Item oItem = new Item();
            ItemData oItemData;
            ItemRow oItemRow = null;
            oItemData = PopulateItem(itemID);
            if (oItemData.Item.Rows.Count > 0)
            {
                oItemRow = oItemData.Item[0];
                strDiscountPolicy = oItemRow.DiscountPolicy;
                //Following Code Added by Krishna on 28 December 2011 //if Item is on sale dont give any other discount

                //if (oItemRow.isOnSale == true)    //PRIMEPOS-2979 29-Jun-2021 JY Commented
                if (oItemRow.isOnSale && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value
                    && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)  //PRIMEPOS-2979 29-Jun-2021 JY Added
                {
                    //Following if Added By shitaljit on 21 Feb 2012
                    //To not apply discount on sale item if AllowDiscountOfItemsOnSale is false
                    if (Configuration.CInfo.AllowDiscountOfItemsOnSale == false)
                    {
                        //return returnValue;
                        if (oTDRow != null && oTDRow.ItemID.ToString() == oItemRow.ItemID.ToString() && oTDRow.IsPriceChanged)
                        {
                            logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "1");
                            return returnValue;
                        }
                    }
                    if (oItemRow.SaleStartDate != DBNull.Value || oItemRow.SaleEndDate != DBNull.Value)
                        if (DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                        {
                            if (oTDRow != null && oTDRow.ItemID.ToString() == oItemRow.ItemID.ToString() && oTDRow.IsPriceChanged)
                            {
                                returnValue = GetDiscount(QTY * Convert.ToDecimal((oItemRow.Discount / 100 * price))); //Sprint-21 - PRIMEPOS-225 11-Feb-2016 JY Added logic to apply disc for sale item
                                logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "2");
                                return returnValue;
                            }
                        }
                }

                if (oItemRow.isDiscountable == false)
                {
                    return 0;
                }
                bool isDeptDiscount = false;

                //End of Added by Krishna on 28 Decembder 2011

                DepartmentData oDeptData = new DepartmentData();
                if (oItemRow.DepartmentID != 0)
                {
                    Department oDept = new Department();
                    oDeptData = new DepartmentData();
                    oDeptData = oDept.Populate(oItemRow.DepartmentID);

                    //Following Code Added by Krishna on 28 December 2011
                    if (oDeptData.Tables[0].Rows.Count > 0 && DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate))
                    {
                        if (oItemRow.SubDepartmentID == 0)
                        {
                            isDeptSale = true;
                        }
                        //Added By Shitaljit to check whether the sub department is on sale or not
                        //JIRA#-PRIMEPOS-1038  Ability to choose which sub-dept sale to apply
                        else
                        {
                            SubDepartment oSubDepartment = new SubDepartment();
                            SubDepartmentData oSubDepartmentData = new SubDepartmentData();
                            int deptID = oDeptData.Department[0].DeptID;
                            oSubDepartmentData = oSubDepartment.PopulateList(" WHERE IncludeOnSale = 1 AND SubDepartment." + clsPOSDBConstants.SubDepartment_Fld_DepartmentID + " = " + deptID + " AND SubDepartmentID = " + oItemRow.SubDepartmentID);
                            if (Configuration.isNullOrEmptyDataSet(oSubDepartmentData) == false)
                            {
                                isDeptSale = true;
                            }
                        }
                    }
                }
                //End of Added by Krishna on 28 December 2011

                #region Apply DisCount policy
                ApplyDiscPolicy(oItemRow, out isDeptDiscount);
                #endregion Apply DisCount policy

                #region get discount
                if (oItemRow.isDiscountable == true && (oItemRow.DiscountPolicy == "I" || string.IsNullOrEmpty(oItemRow.DiscountPolicy) == true))
                {
                    returnValue = GetDiscount(QTY * Convert.ToDecimal((oItemRow.Discount / 100 * price)));
                }
                else if (isDeptDiscount == true)
                {
                    if (DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate) && isDeptSale == true)
                    {
                        if (oDeptData.Department[0].SaleDiscount > 0)
                        {
                            returnValue = GetDiscount(QTY * Convert.ToDecimal((oDeptData.Department[0].SaleDiscount / 100 * price)));
                        }
                    }
                    else if (DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate) && isDeptSale == false)
                    {
                        logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "3");
                        return returnValue;
                    }
                    else
                    {
                        returnValue = GetDiscount(QTY * Convert.ToDecimal((oDeptData.Department[0].Discount / 100 * price)));
                    }
                }
                #endregion get discount
            }
            logger.Trace("CalculateDiscount() - " + clsPOSDBConstants.Log_Exiting + "4");

            return returnValue;
        }
        public void CalculateLoyalityPoints()
        {
            try
            {

                if (CurrentTransactionType == POSTransactionType.SalesReturn && oCustomerRow != null)
                {
                    TransDetailSvr oTDSvr = new TransDetailSvr();
                    foreach (TransDetailRow row in oTransDData.TransDetail.Rows)
                    {
                        oTDSvr.CalculateLoyaltyPointsForReturnItem(row, oCustomerRow.CustomerId);
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CalculateLoyalityPoints()");
                throw exp;
            }
        }

        public void SaveCCToken()
        {
            if (!Configuration.CPOSSet.PaymentProcessor.Equals("HPS") && oCustomerRow.AccountNumber > 0 && Configuration.CInfo.SaveCCToken == true)  //Sprint-23 - PRIMEPOS-2315 21-Jun-2016 JY Added SaveCCToken condition
            {
                bool persist = false;
                CCCustomerTokInfoData ocustomerTokData = new CCCustomerTokInfoData();
                CCCustomerTokInfo tokinfo = new CCCustomerTokInfo();
                ocustomerTokData = tokinfo.GetTokenByCustomerandProcessor(oCustomerRow.CustomerId);

                if (oPOSTransPaymentData != null && oPOSTransPaymentData.Tables.Count > 0 && oPOSTransPaymentData.Tables[0].Rows.Count > 0)
                {
                    int entryID = 0;//PRIMEPOS-3189
                    foreach (POSTransPaymentRow opayRow in oPOSTransPaymentData.Tables[0].Rows)
                    {
                        if (opayRow.Tokenize)   //PRIMEPOS-3145 28-Sep-2022 JY Added if condition
                        {
                            if ((opayRow.ProfiledID.Length > 0 || Configuration.convertNullToString(opayRow.PrimeRxPayTransID).Trim() !="") && opayRow.TransTypeDesc.Length > 0)//&& opayRow.pay, for PaymentProcessor == "WORLDPAY", "XLINK" and "HPSPAX"
                            {
                                string last4 = string.Empty;
                                string[] temp;
                                string[] delimiter = new string[] { "|" };
                                temp = opayRow.RefNo.Split(delimiter, StringSplitOptions.None);
                                if (temp.Length > 0)
                                {
                                    last4 = temp[0].Replace("*", string.Empty);
                                }
                                bool cardExists = false;
                                if (ocustomerTokData.Tables.Count > 0 && ocustomerTokData.Tables[0].Rows.Count > 0)
                                {
                                    foreach (CCCustomerTokInfoRow otokRow in ocustomerTokData.CCCustomerTokInfo.Rows)//ocustomerTokData.Tables[0].Rows
                                    {
                                        if (otokRow.Last4?.Trim() == last4?.Trim() && otokRow.CardType == opayRow.TransTypeDesc)
                                        {
                                            cardExists = true;
                                            otokRow.ProfiledID = opayRow.ProfiledID;
                                            otokRow.EntryType = opayRow.EntryMethod;
                                            otokRow.TokenDate = opayRow.TransDate; //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
                                            otokRow.IsFsaCard = opayRow.IsFsaCard;//PRIMEPOS-3078
                                            otokRow.ExpDate = ExtractCreditCardExpiryDate(opayRow.RefNo);
                                            persist = true;
                                        }
                                        if (opayRow.PaymentProcessor == "HPSPAX" && Convert.ToString(opayRow.ProfiledID).Trim() == Convert.ToString(otokRow.ProfiledID).Trim()) //PRIMEPOS - 2952
                                        {
                                            cardExists = true;
                                        }
                                    }
                                }

                                if (!cardExists)
                                {
                                    CCCustomerTokInfoRow orow = (CCCustomerTokInfoRow)ocustomerTokData.CCCustomerTokInfo.NewRow();
                                    CCCustomerTokInfoTable otable = new CCCustomerTokInfoTable();
                                    orow.EntryID = entryID++;//PRIMEPOS-3189
                                    //if (oCustomerRow.SaveCardProfile == true)   //Sprint-23 - PRIMEPOS-2315 21-Jun-2016 JY Added condition - If only system level settings is true then it will save the token but should not save the CustomerId against that toekn. Means CustomerId commited as blank. If system level and customer level settings is true then token should be saved against selected customer.   //PRIMEPOS-3145 28-Sep-2022 JY Commented
                                    //{//Added By Rohit Nair on June 05,2017 for PrimePOS 2431
                                    orow.CustomerID = oCustomerRow.CustomerId;
                                    orow.CardType = opayRow.TransTypeDesc;
                                    orow.Last4 = last4;
                                    orow.ProfiledID = opayRow.ProfiledID;
                                    if (Configuration.isPrimeRxPay || opayRow.PaymentProcessor == "PRIMERXPAY")//PRIMEPOS-2902 //PRIMEPOS-3432
                                        orow.Processor = Configuration.CSetting.PayProviderName;
                                    else
                                        orow.Processor = opayRow.PaymentProcessor;
                                    orow.EntryType = opayRow.EntryMethod;
                                    orow.TokenDate = opayRow.TransDate;
                                    orow.IsFsaCard = opayRow.IsFsaCard;//2990
                                    orow.ExpDate = ExtractCreditCardExpiryDate(opayRow.RefNo);
                                    ocustomerTokData.CCCustomerTokInfo.AddRow(orow, true);
                                    persist = true;
                                    //}
                                }
                            }
                        }
                    }
                }

                if (persist)
                {
                    tokinfo.Persist(ocustomerTokData);
                }
            }
        }

        public void SaveOnlyCCToken(int CustomerID, string CardType, string Last4, string ProfileId, string EntryMethod, string PaymentProcessor, string request, string response, string ExpiryDate)//PRIMEPOS-2896 Arvind
        {
            logger.Trace("Entered in SaveOnlyCCToken method ");
            bool persist = false;
            string newCardType = string.Empty;
            //ADDED BY ARVIND TO HANDLE CARDTYPE PRIMEPOS-2636 
            if (CardType == null)
            {
                CardType = string.Empty;
            }
            //
            switch (CardType.ToUpper().Trim())
            {
                case "MASTERCARD":
                case "MASTER":
                case "MASTER CARD":
                case "MC":
                    newCardType = "Master Card";
                    break;
                case "VISACARD":
                case "VISA":
                case "VISA CARD":
                    newCardType = "Visa";
                    break;
                case "AMERICANEXPRESSCARD":
                case "AMERICANEXPRESS":
                case "AMERICAN EXPRESS":
                case "AMEX":
                    newCardType = "American Express";
                    break;
                case "DISCOVERCARD":
                case "DISCOVER CARD":
                case "DISCOVER":
                case "DISC":
                    newCardType = "Discover";
                    break;
                case "DEBITCARD":
                case "DEBIT CARD":
                case "DEBIT":
                case "DCCB":
                    newCardType = "Debit Card";
                    break;
                //PRIMEPOS-3057
                default:
                    newCardType = CardType.ToUpper().Trim();
                    break;
            }
            CCCustomerTokInfoData ocustomerTokData = new CCCustomerTokInfoData();
            CCCustomerTokInfo tokinfo = new CCCustomerTokInfo();
            ocustomerTokData = tokinfo.GetTokenByCustomerandProcessor(CustomerID);

            bool cardExists = false;
            if (ocustomerTokData.Tables.Count > 0 && ocustomerTokData.Tables[0].Rows.Count > 0)
            {
                foreach (CCCustomerTokInfoRow otokRow in ocustomerTokData.CCCustomerTokInfo.Rows)
                {
                    if (otokRow.Last4?.Trim() == Last4?.Trim())
                    {
                        cardExists = true;
                        otokRow.ProfiledID = ProfileId;
                        otokRow.EntryType = EntryMethod;
                        otokRow.TokenDate = DateTime.Now;
                        persist = true;
                    }
                }
            }

            if (!cardExists)
            {
                CCCustomerTokInfoRow orow = (CCCustomerTokInfoRow)ocustomerTokData.CCCustomerTokInfo.NewRow();
                CCCustomerTokInfoTable otable = new CCCustomerTokInfoTable();
                orow.EntryID = 0;

                orow.CustomerID = CustomerID;
                orow.CardType = newCardType;
                orow.Last4 = Last4;
                orow.ProfiledID = ProfileId;
                orow.Processor = PaymentProcessor;
                orow.EntryType = EntryMethod;
                orow.TokenDate = DateTime.Now;
                orow.ExpDate = ExtractCreditCardExpiryDate(Last4 + "|" + ExpiryDate);
                ocustomerTokData.CCCustomerTokInfo.AddRow(orow, true);
                persist = true;
            }

            if (persist)
            {
                tokinfo.Persist(ocustomerTokData);
            }
            using (var db = new Possql())
            {
                CCTransmission_Log cclog = new CCTransmission_Log();
                cclog.TransDateTime = DateTime.Now;
                cclog.TransAmount = 0;
                //cclog.TransDataStr = _DeviceRequestMessage;
                cclog.PaymentProcessor = PaymentProcessor;
                cclog.StationID = Configuration.StationID;
                cclog.UserID = Configuration.UserName;
                cclog.TransmissionStatus = "Completed";
                cclog.RecDataStr = response;
                cclog.TransDataStr = request;
                cclog.TransType = "Only Tokenize";
                db.CCTransmission_Logs.Add(cclog);
                db.SaveChanges();
            }

            logger.Trace("Exiting SaveOnlyCCToken method ");
        }

        private DateTime ExtractCreditCardExpiryDate(string strRefNo)
        {
            DateTime expDate = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(strRefNo))
                return expDate;

            int idx = strRefNo.IndexOf('|');
            if (idx >= 0)
            {
                string mmyyStr = strRefNo.Substring(idx + 1);
                mmyyStr = mmyyStr.Trim();
                int len = mmyyStr.Length;
                string yearStr = string.Empty;
                string monStr = string.Empty;
                int iPos = mmyyStr.IndexOf('/');
                if (iPos >= 0)
                {
                    yearStr = mmyyStr.Substring(iPos + 1);
                    monStr = mmyyStr.Substring(0, iPos);
                }
                else if (len > 2)
                {
                    yearStr = mmyyStr.Substring(len - 2, 2);
                    monStr = mmyyStr.Substring(0, len - 2);
                }

                if (!string.IsNullOrWhiteSpace(yearStr) && !string.IsNullOrWhiteSpace(monStr))
                {
                    int iYear = Convert.ToInt32(yearStr);
                    int iMonth = Convert.ToInt32(monStr);
                    int iDay = DateTime.DaysInMonth(iYear, iMonth);

                    string expiryDateStr = monStr + "/" + iDay.ToString() + "/" + yearStr;

                    #region Commented by Arvind PRIMEPOS-2727
                    //Added by Arvind PRIMEPOS-2727
                    //expDate = DateTime.ParseExact(expiryDateStr, "MM/dd/yy", CultureInfo.InvariantCulture);
                    //expDate = Convert.ToDateTime(expiryDateStr);
                    #endregion

                    #region Added by Arvind PRIMEPOS-2727
                    if (Configuration.CPOSSet.PaymentProcessor != "WORLDPAY")//Added by Arvind PRIMEPOS-2727
                    {
                        //Added by Arvind PRIMEPOS-2727
                        expDate = DateTime.ParseExact(expiryDateStr, "MM/dd/yy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        expDate = Convert.ToDateTime(expiryDateStr);
                    }
                    #endregion
                }
            }
            return expDate;
        }

        public decimal CalculateMaxCouponAmount(string discount)
        {
            decimal maxDiscount;
            try
            {
                CLCards ccard = new CLCards();
                maxDiscount = ccard.CalculateMaxCoupmAmount(oTransDData.TransDetail, discount); //09-Apr-2015 JY Added txtAmtDiscount.Text.ToString()

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CalculateMaxCouponAmount()");
                throw exp;
            }
            return maxDiscount;

        }

        public DataTable GetTransDetailTax(int transId)
        {
            DataTable dtTransDetailTax;
            try
            {
                TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(transId); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransDetailTax()");
                throw exp;
            }
            return dtTransDetailTax;
        }

        public bool IsOpenDrawer(POSTransPaymentTable posTransPayment)
        {
            bool canOpenDrawer = false;
            try
            {
                foreach (POSTransPaymentRow row in posTransPayment.Rows)
                {
                    switch (row.TransTypeCode.Trim())
                    {
                        case "1":
                            canOpenDrawer = true;
                            break;
                        case "2":
                            {
                                if (!Configuration.CInfo.DoNotOpenDrawerForChequeTrans)
                                {
                                    canOpenDrawer = true;
                                }
                                break;
                            }
                        case "H":
                            if (!Configuration.CInfo.DoNotOpenDrawerForHouseChargeOnlyTrans)    //Sprint-19 - 2161 30-Mar-2015 JY Added to control open drawer functionality for House charge
                            {
                                canOpenDrawer = true;
                            }
                            break;
                        case "B":
                        case "C":
                        case "E":
                            canOpenDrawer = true;
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsOpenDrawer()");
                throw exp;
            }
            return canOpenDrawer;
        }

        public decimal CalculateMaxCouponAmount(TransDetailTable transDetailTable, string strDiscount)
        {
            decimal disc;
            try
            {
                CLCards ccard = new CLCards();
                disc = ccard.CalculateMaxCoupmAmount(transDetailTable, strDiscount);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CalculateMaxCouponAmount()");
                throw exp;
            }
            return disc;
        }

        public bool AllowDiscountOfItemsOnSale(string ItemID)
        {
            Item oBRItem = new Item();
            ItemData oItemData = null;
            ItemRow oItemRow = null;
            bool RetVal = false;
            try
            {
                oItemData = oBRItem.Populate(ItemID);
                oItemRow = oItemData.Item[0];
                //if (oItemRow.isOnSale == true)
                if (oItemRow.isOnSale == true && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value
                    && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)  //PRIMEPOS-3138 01-Sep-2022 JY modified if condition
                {
                    if (Configuration.CInfo.AllowDiscountOfItemsOnSale == true)
                    {
                        RetVal = true;
                    }
                    else
                    {
                        RetVal = false;
                    }
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception Ex)
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }
            return RetVal;
        }

        public int UpdateSaveCardProfileFlag(int customerId)
        {
            int retVal = 0;
            try
            {
                Customer oCustomer = new Customer();
                retVal = oCustomer.UpdateSaveCardProfileFlag(customerId);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "UpdateSaveCardProfileFlag()");
                throw exp;
            }
            return retVal;
        }

        public CLCardsRow GetActiveCardForCustomerID(int customerId)
        {
            CLCardsRow oCLRow = null;
            try
            {
                CLCards oCLCards = new CLCards();
                oCLRow = oCLCards.GetActiveCardForCustomerID(customerId);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetActiveCardForCustomerID()");
                throw exp;
            }
            return oCLRow;
        }


        public string GetPaymentDetailForReturnTrans(string pickUpDate, out DataSet oTDData)
        {
            string strMsg = "";
            try
            {

                using (POS_Core.DataAccess.TransDetailSvr oTDsvr = new POS_Core.DataAccess.TransDetailSvr())
                {
                    oTDData = oTDsvr.GetTransDetail(Configuration.convertNullToInt(oTDRow.ReturnTransDetailId));
                    if (oTDData != null && oTDData.Tables.Count > 0 && oTDData.Tables[0].Rows.Count > 0)
                    {
                        string PaymentType = string.Empty;
                        POSTransPaymentSvr oTransPaySvr = new POSTransPaymentSvr();
                        PayType oPayType = new PayType();
                        DataSet dsTransPayData = oTransPaySvr.Populate(Configuration.convertNullToInt(oTDData.Tables[0].Rows[0]["TransID"]));
                        for (int count = 0; count < dsTransPayData.Tables[0].Rows.Count; count++)
                        {
                            DataSet oPayTypeData = oPayType.PopulateList(" PayTypeID = '" + dsTransPayData.Tables[0].Rows[count]["TransTypeCode"].ToString() + "'");

                            if (oPayTypeData.Tables[0].Rows.Count > 0)
                            {
                                PaymentType += oPayTypeData.Tables[0].Rows[0]["PayTypeDesc"].ToString() + " , ";
                            }
                        }
                        if (PaymentType.EndsWith(" , "))
                        {
                            int index = PaymentType.LastIndexOf(" , ");
                            PaymentType = PaymentType.Remove(index);
                        }

                        string strDate = Configuration.convertNullToString(pickUpDate);
                        strMsg = "Rx is already picked up on " + strDate;
                        strMsg += "\nTransaction ID : " + Configuration.convertNullToInt(oTDData.Tables[0].Rows[0]["TransID"]).ToString() + "\n" +
                        "User : " + Configuration.convertNullToString(oTDData.Tables[0].Rows[0]["UserID"]).ToString() + "\n" +
                        "Payment Type :" + PaymentType + "\n" + "Station ID :" + Configuration.convertNullToInt(oTDData.Tables[0].Rows[0]["StationID"]).ToString();

                        strMsg += "\n\n" + "Do You Wish to view the Transaction?";
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetPaymentDetailForReturnTrans()");
                throw exp;
            }
            return strMsg;
        }

        #region PRIMEPOS-2669 08-Jul-2020 JY Added
        public string GetTaxDescription(int TaxID)
        {
            string TaxDescription = string.Empty;
            TaxCodes oTaxCode = new TaxCodes();
            TaxCodesData oTaxCodeData = oTaxCode.Populate(TaxID);
            if (oTaxCodeData != null && oTaxCodeData.TaxCodes != null && oTaxCodeData.TaxCodes.Rows.Count > 0)
            {
                TaxDescription = oTaxCodeData.TaxCodes.Rows[0]["Description"].ToString();
            }
            return TaxDescription;

        }
        #endregion
    }
}
