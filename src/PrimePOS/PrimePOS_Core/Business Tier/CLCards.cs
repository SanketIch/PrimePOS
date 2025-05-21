using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
using System.Collections.Generic;
////using POS.Resources;
using POS_Core.Resources;
using POS_Core.TransType;

namespace POS_Core.BusinessRules
{

    /// <summary>
    /// This is business object of CLCards. It contains business rules of CLCards.
    /// </summary>
    public class CLCards : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating CLCardss data. 
        /// </summary>
        /// <param name="updates">It is CLCards type dataset class. It contains all information of CLCardss.</param>

        public void Persist(CLCardsData updates)
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    checkIsValidData(updates);
                    dao.Persist(updates);
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try {
                oConn = DataFactory.CreateConnection(DBConfig.ConnectionString);
                CLCardsSvr oSvr = new CLCardsSvr();
                return oSvr.DeleteRow(CurrentID);
            } catch (Exception ex) {
                throw (ex);
            }
        }
        public bool DeactivateCard(Int64 CLCardID)
        {
            try {
                CLCardsSvr oSvr = new CLCardsSvr();
                return oSvr.DeactivateCard(CLCardID);
            } catch (Exception ex) {
                throw (ex);
            }
        }


        public void UpdateLoyaltyCurrentPoints(int CLCardID, int points, System.Data.IDbTransaction oTrans)
        {
            CLCardsSvr oSvr = new CLCardsSvr();
            oSvr.UpdateLoyaltyCurrentPoints(CLCardID, points, oTrans);
        }

        #endregion

        #region Get Methods
        /// <summary>
        /// Get typed dataset (CLCardsData) according to CLCards code
        /// </summary>
        /// <param name="DeptCode">This code represent each CLCards.</param>
        /// <returns>CLCards type dataset.</returns>
        public CLCardsData Populate(System.Int64 iID)
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    return dao.Populate(iID);
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }

        public CLCardsData GetByCLCardID(System.Int64 iCLCardID)
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    return dao.GetByCLCardID(iCLCardID);
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }

        #region 07-Oct-2016 JY Added to check CL Card exists irrespective of active/inactive
        public CLCardsData CheckCLCardExists(System.Int64 iCLCardID)
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    return dao.CheckCLCardExists(iCLCardID);
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }
        #endregion

        public CLCardsData GetByCustomerID(System.Int32 iCustomerID)
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    return dao.GetByCustomerID(iCustomerID);
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }

        #region 30-Nov-2017 JY added to get customer loyalty info w.r.t. customer
        public DataTable GetCustomerLoyaltyGrid(System.Int32 iCustomerID)
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    return dao.GetCustomerLoyaltyGrid(iCustomerID);
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }
        #endregion

        public CLCardsRow GetActiveCardForCustomerID(System.Int32 iCustomerID)
        {
            CLCardsRow returnValue = null;
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    CLCardsData oCLCardsData = dao.GetByCustomerID(iCustomerID);
                    foreach (CLCardsRow oRow in oCLCardsData.CLCards.Rows) {
                        if (oRow.IsPrepetual == true || oRow.RegisterDate.AddDays(oRow.ExpiryDays) >= DateTime.Now.Date) {
                            returnValue = oRow;
                            break;
                        }
                    }
                }
            } catch (Exception ex) {
                throw (ex);
            }
            return returnValue;
        }

        public long GetNextCardNumber()
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    return dao.GetNextCardNumber();
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }

        /// <summary>
        /// Get CLCards type dataset with respect to condition.
        /// </summary>
        /// <param name="whereClause">Provide SQL where clause.</param>
        /// <returns>CLCards type dataset.</returns>
        public CLCardsData PopulateList(string whereClause)
        {
            try {
                using (CLCardsSvr dao = new CLCardsSvr()) {
                    return dao.PopulateList(whereClause);
                }
            } catch (Exception ex) {
                throw (ex);
            }
        }

        #endregion

        #region Validation Methods
        /// <summary>
        /// Check the CLCardss data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">CLCards type dataset.</param>
        public void checkIsValidData(CLCardsData updates)
        {
            CLCardsTable table;

            CLCardsRow oRow;

            oRow = (CLCardsRow)updates.Tables[0].Rows[0];

            table = (CLCardsTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CLCardsTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CLCardsTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CLCardsTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CLCardsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CLCardsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CLCardsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLCards.
        /// </summary>
        /// <param name="updates">CLCards type dataset.</param>
        public virtual void checkIsValidPrimaryKey(CLCardsData updates)
        {
            CLCardsTable table = (CLCardsTable)updates.Tables[clsPOSDBConstants.CLCards_tbl];
            foreach (CLCardsRow row in table.Rows) {
                if (this.Populate(row.CLCardID).Tables[clsPOSDBConstants.CLCards_tbl].Rows.Count != 0) {
                    throw new Exception("Primary key violation for CLCards ");
                }
            }
        }

        #endregion

        /// <summary>
        /// Dispose all resources of CLCards.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public Decimal CalculatePoints(TransHeaderRow oTransHeaderRow, TransDetailData oTransDetailData)
        {
            #region Sprint-18 - 2041 29-Oct-2014 JY Commented as return trans should use the same point calculation logic as sale trans 
            //if (oTransHeaderRow.TransType == (int)POSTransactionType.SalesReturn)
            //    return CalculatePointsForReturnTrans(oTransDetailData);
            #endregion

            decimal totalPoints = 0;
            bool isRowValid = false;
            decimal lineNetTotal;
            int lineTotalQty;
            decimal TotValidAmount = 0;
            decimal InvDiscount = oTransHeaderRow.InvoiceDiscount;
            bool isInvdiscAmtExceeded = false;
            foreach (TransDetailRow oRow in oTransDetailData.TransDetail.Rows) {
                lineTotalQty = 0;
                lineNetTotal = 0;
                isRowValid = false;
                ItemRow oItemRow = null;
                //if(Configuration.CLoyaltyInfo.ExcludeDiscountableItems)
                if (Configuration.CLoyaltyInfo.ExcludeItems.Data.Contains(oRow.ItemID) == false) {
                    Item oItem = new Item();
                    ItemData oItemData = oItem.Populate(oRow.ItemID);
                    oItemRow = oItemData.Item[0];

                    #region Sprint-27 - PRIMEPOS-2452 03-Oct-2017 JY Commented
                    //if ((oItemData.Item[0].isDiscountable && Configuration.CLoyaltyInfo.ExcludeDiscountableItems))
                    //{
                    //    isRowValid = false;
                    //}
                    //else if ((Configuration.CLoyaltyInfo.IncludeRXItems == true && oRow.ItemID == "RX")
                    //          || (Configuration.CLoyaltyInfo.IncludeOTCItems == true && oItemData.Item[0].isOTCItem == true)
                    //          || (oItemData.Item[0].isOTCItem == false && oRow.ItemID != "RX"))
                    //{
                    //    Department oDepartment = new Department();
                    //    DepartmentData oDeptData = null;
                    //    bool isItemOnSale = false;
                    //    bool isExcludeDepartment = false;
                    //    bool isExcludeSubDepartment = false;

                    //    if (oItemData.Item[0].DepartmentID != 0)
                    //    {
                    //        oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                    //        if (oDeptData.Department.Rows.Count > 0)
                    //        {
                    //            isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], oDeptData.Department[0]);
                    //            isExcludeDepartment = Configuration.CLoyaltyInfo.ExcludeDepts.Data.Contains(oDeptData.Department[0].DeptID.ToString());
                    //        }
                    //    }
                    //    else
                    //    {
                    //        isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], null);
                    //    }

                    //    if (oItemData.Item[0].SubDepartmentID != 0)
                    //    {
                    //        isExcludeSubDepartment = Configuration.CLoyaltyInfo.ExcludeSubDepts.Data.Contains(oItemData.Item[0].SubDepartmentID.ToString());
                    //    }

                    //    if (isExcludeDepartment == false && isExcludeSubDepartment == false)
                    //    {
                    //        if (Configuration.CLoyaltyInfo.ExcludeItemsOnSale == false || isItemOnSale == false)
                    //        {
                    //            isRowValid = true;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region Sprint-27 - PRIMEPOS-2452 03-Oct-2017 JY Added logic to consider OTC item with different settings like "Exclude Dept, Exclude Sub-Dept, Exclude Discountable, Exclude IsOnSale, Include OTC"
                    if (Configuration.CLoyaltyInfo.IncludeRXItems == true && oRow.ItemID == "RX") {
                        isRowValid = true;
                    } else {
                        if (Configuration.CLoyaltyInfo.IncludeOTCItems == false || (Configuration.CLoyaltyInfo.IncludeRXItems == false && oRow.ItemID == "RX")) {
                            isRowValid = false;
                        } else {
                            Department oDepartment = new Department();
                            DepartmentData oDeptData = null;
                            bool isItemOnSale = false;
                            bool isExcludeDepartment = false;
                            bool isExcludeSubDepartment = false;

                            if (oItemData.Item[0].DepartmentID != 0) {
                                oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                                if (oDeptData.Department.Rows.Count > 0) {
                                    isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], oDeptData.Department[0]);
                                    isExcludeDepartment = Configuration.CLoyaltyInfo.ExcludeDepts.Data.Contains(oDeptData.Department[0].DeptID.ToString());
                                }
                            } else {
                                isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], null);
                            }

                            if (oItemData.Item[0].SubDepartmentID != 0) {
                                isExcludeSubDepartment = Configuration.CLoyaltyInfo.ExcludeSubDepts.Data.Contains(oItemData.Item[0].SubDepartmentID.ToString());
                            }

                            bool isDiscountableItem = false;
                            if (Configuration.CLoyaltyInfo.ExcludeDiscountableItems) {
                                if (oItemData.Item[0].isDiscountable) {
                                    if (oItemData.Item[0].DiscountPolicy == "D") {
                                        if (oItemData.Item[0].DepartmentID != 0) {
                                            oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                                            if (oDeptData.Department.Rows.Count > 0) {
                                                if (Configuration.convertNullToDecimal(oDeptData.Department[0].Discount) > 0)
                                                    isDiscountableItem = true;
                                            }
                                        }
                                    } else if (Configuration.convertNullToDecimal(oItemData.Item[0].Discount) > 0) {
                                        isDiscountableItem = true;
                                    }
                                }
                            }

                            if (isExcludeDepartment == false && isExcludeSubDepartment == false && !(Configuration.CLoyaltyInfo.ExcludeItemsOnSale == true && isItemOnSale == true) && !(Configuration.CLoyaltyInfo.ExcludeDiscountableItems == true && isDiscountableItem == true)) {
                                isRowValid = true;
                            }
                        }
                    }
                    #endregion
                }

                if (isRowValid == true) {
                    lineNetTotal = (oRow.ExtendedPrice - (oRow.Discount));
                    lineTotalQty = oRow.QTY;
                    TotValidAmount += lineNetTotal;
                    CLCardsSvr oCLCardsSvr = new CLCardsSvr();
                    decimal PointsPerDollar = oCLCardsSvr.GetPointsPerDollar(oItemRow.CLPointPolicy, oItemRow.ItemID, oItemRow.DepartmentID, oItemRow.SubDepartmentID);

                    if ((Configuration.CLoyaltyInfo.PointsCalcMethod == "A") && (TotValidAmount > InvDiscount) || (oTransHeaderRow.TransType == (int)POSTransactionType.SalesReturn && TotValidAmount < InvDiscount)) {
                        #region Sprint-18 - 2041 28-Oct-2014 JY incorrect code commented
                        /*
                        if (isInvdiscAmtExceeded == false)
                        {
                            lineNetTotal = TotValidAmount - InvDiscount;
                            isInvdiscAmtExceeded = true;
                        }
                        if (Configuration.CLoyaltyInfo.PointsCalcMethod == "A")
                        {
                            if (oItemRow.IsDefaultCLPoint == false && oItemRow.PointsPerDollar > 0)
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)oItemRow.PointsPerDollar, 2);
                            }
                            else
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)Configuration.CLoyaltyInfo.RedeemValue, 2);
                            }
                        }
                        else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "L")
                        {
                            if (oItemRow.IsDefaultCLPoint == false && oItemRow.PointsPerDollar > 0)
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)oItemRow.PointsPerDollar, 2);
                            }
                            else
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((1) * (decimal)Configuration.CLoyaltyInfo.RedeemValue, 2);
                            }
                        }
                        else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "Q")
                        {
                            if (oItemRow.IsDefaultCLPoint == false && oItemRow.PointsPerDollar > 0)
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)oItemRow.PointsPerDollar, 2);
                            }
                            else
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineTotalQty) * (decimal)Configuration.CLoyaltyInfo.RedeemValue, 2);
                            }
                        }*/
                        #endregion

                        #region Sprint-18 - 2041 28-Oct-2014 JY added new logic along with corrected code - There are three CL point calculation methods as By Trans Amt, By line items and By quantity. Out of them first one is working fine but other two have issue. For "By line items" method line item count should be considered and for "By quantity" method total item quantity should be considered.
                        //CLCardsSvr oCLCardsSvr = new CLCardsSvr();
                        //decimal PointsPerDollar = oCLCardsSvr.GetPointsPerDollar(oItemRow.CLPointPolicy, oItemRow.ItemID, oItemRow.DepartmentID, oItemRow.SubDepartmentID);
                        if (isInvdiscAmtExceeded == false) {
                            lineNetTotal = TotValidAmount - InvDiscount;
                            isInvdiscAmtExceeded = true;
                        }
                        if (Configuration.CLoyaltyInfo.PointsCalcMethod == "A")
                            oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * PointsPerDollar, 2);
                        //else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "L")
                        //    oRow.LoyaltyPoints = (decimal)Math.Round((1) * PointsPerDollar, 2);
                        //else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "Q")
                        //    oRow.LoyaltyPoints = (decimal)Math.Round((lineTotalQty) * PointsPerDollar, 2);
                        #endregion

                        totalPoints += oRow.LoyaltyPoints;
                    } else {
                        if (Configuration.CLoyaltyInfo.PointsCalcMethod == "L")
                            oRow.LoyaltyPoints = (decimal)Math.Round((1) * PointsPerDollar, 2);
                        else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "Q")
                            oRow.LoyaltyPoints = (decimal)Math.Round((lineTotalQty) * PointsPerDollar, 2);

                        totalPoints += oRow.LoyaltyPoints;
                    }
                }
            }
            return totalPoints;
        }


        public Decimal CalculateMaxCoupmAmount(TransDetailTable transDetailTable, string strDiscount)    //09-Apr-2015 JY Added txtAmtDiscount.Text.ToString()
        {
            decimal totalPoints = 0;
            bool isRowValid = false;
            decimal lineNetTotal;
            int lineTotalQty;
            decimal TotValidAmount = 0;
            bool isInvdiscAmtExceeded = false;
            foreach (TransDetailRow oRow in transDetailTable.Rows) {
                lineTotalQty = 0;
                lineNetTotal = 0;
                isRowValid = false;
                ItemRow oItemRow = null;

                if (Configuration.CLoyaltyInfo.ExcludeClCouponItems.Data.Contains(oRow.ItemID) == false) {
                    Item oItem = new Item();
                    ItemData oItemData = oItem.Populate(oRow.ItemID);
                    oItemRow = oItemData.Item[0];

                    #region Sprint-27 - PRIMEPOS-2452 03-Oct-2017 JY Commented
                    //if ((oItemData.Item[0].isDiscountable && Configuration.CLoyaltyInfo.ExcludeDiscountableItems))
                    //{
                    //    isRowValid = false;
                    //}
                    //else if ((Configuration.CLoyaltyInfo.IncludeRXItems == true && oRow.ItemID == "RX")
                    //          || (Configuration.CLoyaltyInfo.IncludeOTCItems == true && oItemData.Item[0].isOTCItem == true)
                    //          || (oItemData.Item[0].isOTCItem == false && oRow.ItemID != "RX"))
                    //{
                    //    Department oDepartment = new Department();
                    //    DepartmentData oDeptData = null;
                    //    bool isItemOnSale = false;
                    //    bool isExcludeDepartment = false;
                    //    bool isExcludeSubDepartment = false;

                    //    if (oItemData.Item[0].DepartmentID != 0)
                    //    {
                    //        oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                    //        if (oDeptData.Department.Rows.Count > 0)
                    //        {
                    //            isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], oDeptData.Department[0]);
                    //            isExcludeDepartment = Configuration.CLoyaltyInfo.ExcludeClCouponDepts.Data.Contains(oDeptData.Department[0].DeptID.ToString());
                    //        }
                    //    }
                    //    else
                    //    {
                    //        isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], null);
                    //    }

                    //    if (oItemData.Item[0].SubDepartmentID != 0)
                    //    {
                    //        isExcludeSubDepartment = Configuration.CLoyaltyInfo.ExcludeClCouponSubDepts.Data.Contains(oItemData.Item[0].SubDepartmentID.ToString());
                    //    }

                    //    if (isExcludeDepartment == false && isExcludeSubDepartment == false)
                    //    {
                    //        if (Configuration.CLoyaltyInfo.ExcludeItemsOnSale == false || isItemOnSale == false)
                    //        {
                    //            isRowValid = true;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region Sprint-27 - PRIMEPOS-2452 03-Oct-2017 JY Added logic to consider OTC item with different settings like "Exclude Dept, Exclude Sub-Dept, Exclude Discountable, Exclude IsOnSale, Include OTC"
                    if (Configuration.CLoyaltyInfo.IncludeRXItems == true && oRow.ItemID == "RX") {
                        isRowValid = true;
                    } else {
                        if (Configuration.CLoyaltyInfo.IncludeOTCItems == false) {
                            isRowValid = false;
                        } else {
                            Department oDepartment = new Department();
                            DepartmentData oDeptData = null;
                            bool isItemOnSale = false;
                            bool isExcludeDepartment = false;
                            bool isExcludeSubDepartment = false;

                            if (oItemData.Item[0].DepartmentID != 0) {
                                oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                                if (oDeptData.Department.Rows.Count > 0) {
                                    isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], oDeptData.Department[0]);
                                    isExcludeDepartment = Configuration.CLoyaltyInfo.ExcludeClCouponDepts.Data.Contains(oDeptData.Department[0].DeptID.ToString());//change by sandeep to exclude department if it is not selected
                                }
                            } else {
                                isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], null);
                            }

                            if (oItemData.Item[0].SubDepartmentID != 0) {
                                isExcludeSubDepartment = Configuration.CLoyaltyInfo.ExcludeClCouponSubDepts.Data.Contains(oItemData.Item[0].SubDepartmentID.ToString());//change by sandeep to exclude subdepartment if it is not selected
                            }

                            bool isDiscountableItem = false;
                            if (Configuration.CLoyaltyInfo.ExcludeDiscountableItems) {
                                if (oItemData.Item[0].isDiscountable) {
                                    if (oItemData.Item[0].DiscountPolicy == "D") {
                                        if (oItemData.Item[0].DepartmentID != 0) {
                                            oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                                            if (oDeptData.Department.Rows.Count > 0) {
                                                if (Configuration.convertNullToDecimal(oDeptData.Department[0].Discount) > 0)
                                                    isDiscountableItem = true;
                                            }
                                        }
                                    } else if (Configuration.convertNullToDecimal(oItemData.Item[0].Discount) > 0) {
                                        isDiscountableItem = true;
                                    }
                                }
                            }

                            if (isExcludeDepartment == false && isExcludeSubDepartment == false && !(Configuration.CLoyaltyInfo.ExcludeItemsOnSale == true && isItemOnSale == true) && !(Configuration.CLoyaltyInfo.ExcludeDiscountableItems == true && isDiscountableItem == true)) {
                                isRowValid = true;
                            }
                        }
                    }
                    #endregion
                }

                if (isRowValid == true) {
                    //lineNetTotal = (oRow.ExtendedPrice - (oRow.Discount)); //09-Apr-2015 JY Commented 
                    lineNetTotal = (oRow.ExtendedPrice + oRow.TaxAmount); //09-Apr-2015 JY Added to consider tax
                    lineTotalQty = oRow.QTY;
                    TotValidAmount += lineNetTotal;
                }
            }

            TotValidAmount -= Configuration.convertNullToDecimal(strDiscount);  //09-Apr-2015 JY Added to consider invoice discount 

            return TotValidAmount;
        }
        /// <summary>
        /// Author:Shitaljit Created Date : 10/24/2013
        /// </summary>
        /// <param name="oTransHeaderRow"></param>
        /// <param name="oTransDetailData"></param>
        /// <returns></returns>
        public decimal CalculatePointsAfterCouponDiscount(TransHeaderRow oTransHeaderRow, TransDetailData oTransDetailData, decimal CouponAmt)
        {
            if (oTransHeaderRow.TransType == (int)POSTransactionType.SalesReturn) {
                return CalculatePointsForReturnTrans(oTransDetailData);
            }

            Decimal totalPoints = 0;
            decimal TotValidAmount = 0;
            bool isRowValid = false;
            decimal lineNetTotal;
            int lineTotalQty;
            ItemRow oItemRow = null;
            bool isCouponAmtExceeded = false;
            foreach (TransDetailRow oRow in oTransDetailData.TransDetail.Rows) {
                lineTotalQty = 0;
                lineNetTotal = 0;
                isRowValid = false;

                if (Configuration.CLoyaltyInfo.ExcludeItems.Data.Contains(oRow.ItemID) == false) {
                    Item oItem = new Item();
                    ItemData oItemData = oItem.Populate(oRow.ItemID);
                    oItemRow = oItemData.Item[0];

                    #region Sprint-27 - PRIMEPOS-2452 03-Oct-2017 JY Commented
                    //if ((oItemData.Item[0].isDiscountable && Configuration.CLoyaltyInfo.ExcludeDiscountableItems))
                    //{
                    //}
                    //else if ((Configuration.CLoyaltyInfo.IncludeRXItems == true && oRow.ItemID == "RX")
                    //          || (Configuration.CLoyaltyInfo.IncludeOTCItems == true && oItemData.Item[0].isOTCItem == true)
                    //          || (oItemData.Item[0].isOTCItem == false && oRow.ItemID != "RX"))
                    //{
                    //    Department oDepartment = new Department();
                    //    DepartmentData oDeptData = null;
                    //    bool isItemOnSale = false;
                    //    bool isExcludeDepartment = false;
                    //    bool isExcludeSubDepartment = false;

                    //    if (oItemData.Item[0].DepartmentID != 0)
                    //    {
                    //        oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                    //        if (oDeptData.Department.Rows.Count > 0)
                    //        {
                    //            isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], oDeptData.Department[0]);
                    //            isExcludeDepartment = Configuration.CLoyaltyInfo.ExcludeDepts.Data.Contains(oDeptData.Department[0].DeptID.ToString());
                    //        }
                    //    }
                    //    else
                    //    {
                    //        isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], null);
                    //    }

                    //    if (oItemData.Item[0].SubDepartmentID != 0)
                    //    {
                    //        isExcludeSubDepartment = Configuration.CLoyaltyInfo.ExcludeSubDepts.Data.Contains(oItemData.Item[0].SubDepartmentID.ToString());
                    //    }

                    //    if (isExcludeDepartment == false && isExcludeSubDepartment == false)
                    //    {
                    //        if (Configuration.CLoyaltyInfo.ExcludeItemsOnSale == false || isItemOnSale == false)
                    //        {
                    //            isRowValid = true;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region Sprint-27 - PRIMEPOS-2452 03-Oct-2017 JY Added logic to consider OTC item with different settings like "Exclude Dept, Exclude Sub-Dept, Exclude Discountable, Exclude IsOnSale, Include OTC"
                    if (Configuration.CLoyaltyInfo.IncludeRXItems == true && oRow.ItemID == "RX") {
                        isRowValid = true;
                    } else {
                        if (Configuration.CLoyaltyInfo.IncludeOTCItems == false) {
                            isRowValid = false;
                        } else {
                            Department oDepartment = new Department();
                            DepartmentData oDeptData = null;
                            bool isItemOnSale = false;
                            bool isExcludeDepartment = false;
                            bool isExcludeSubDepartment = false;

                            if (oItemData.Item[0].DepartmentID != 0) {
                                oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                                if (oDeptData.Department.Rows.Count > 0) {
                                    isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], oDeptData.Department[0]);
                                    isExcludeDepartment = Configuration.CLoyaltyInfo.ExcludeDepts.Data.Contains(oDeptData.Department[0].DeptID.ToString());
                                }
                            } else {
                                isItemOnSale = oItem.IsItemOnSale(oItemData.Item[0], null);
                            }

                            if (oItemData.Item[0].SubDepartmentID != 0) {
                                isExcludeSubDepartment = Configuration.CLoyaltyInfo.ExcludeSubDepts.Data.Contains(oItemData.Item[0].SubDepartmentID.ToString());
                            }

                            bool isDiscountableItem = false;
                            if (Configuration.CLoyaltyInfo.ExcludeDiscountableItems) {
                                if (oItemData.Item[0].isDiscountable) {
                                    if (oItemData.Item[0].DiscountPolicy == "D") {
                                        if (oItemData.Item[0].DepartmentID != 0) {
                                            oDeptData = oDepartment.Populate(oItemData.Item[0].DepartmentID);
                                            if (oDeptData.Department.Rows.Count > 0) {
                                                if (Configuration.convertNullToDecimal(oDeptData.Department[0].Discount) > 0)
                                                    isDiscountableItem = true;
                                            }
                                        }
                                    } else if (Configuration.convertNullToDecimal(oItemData.Item[0].Discount) > 0) {
                                        isDiscountableItem = true;
                                    }
                                }
                            }

                            if (isExcludeDepartment == false && isExcludeSubDepartment == false && !(Configuration.CLoyaltyInfo.ExcludeItemsOnSale == true && isItemOnSale == true) && !(Configuration.CLoyaltyInfo.ExcludeDiscountableItems == true && isDiscountableItem == true)) {
                                isRowValid = true;
                            }
                        }
                    }
                    #endregion
                }

                if (isRowValid == true) {
                    lineNetTotal = (oRow.ExtendedPrice - oRow.Discount);
                    lineTotalQty = oRow.QTY;
                    TotValidAmount += lineNetTotal;
                    CLCardsSvr oCLCardsSvr = new CLCardsSvr();
                    decimal PointsPerDollar = oCLCardsSvr.GetPointsPerDollar(oItemRow.CLPointPolicy, oItemRow.ItemID, oItemRow.DepartmentID, oItemRow.SubDepartmentID);

                    if ((Configuration.CLoyaltyInfo.PointsCalcMethod == "A") && (TotValidAmount > CouponAmt)) {
                        #region Sprint-18 - 2041 28-Oct-2014 JY incorrect code commented
                        /*if (isCouponAmtExceeded == false)
                        {
                            lineNetTotal = TotValidAmount - CouponAmt;
                            isCouponAmtExceeded = true;
                        }
                        if (Configuration.CLoyaltyInfo.PointsCalcMethod == "A")
                        {
                            if (oItemRow.IsDefaultCLPoint == false && oItemRow.PointsPerDollar > 0)
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)oItemRow.PointsPerDollar, 2);
                            }
                            else
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)Configuration.CLoyaltyInfo.RedeemValue, 2);
                            }
                        }
                        else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "L")
                        {
                            if (oItemRow.IsDefaultCLPoint == false && oItemRow.PointsPerDollar > 0)
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)oItemRow.PointsPerDollar, 2);
                            }
                            else
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((1) * (decimal)Configuration.CLoyaltyInfo.RedeemValue, 2);
                            }
                        }
                        else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "Q")
                        {
                            if (oItemRow.IsDefaultCLPoint == false && oItemRow.PointsPerDollar > 0)
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * (decimal)oItemRow.PointsPerDollar, 2);
                            }
                            else
                            {
                                oRow.LoyaltyPoints = (decimal)Math.Round((lineTotalQty) * (decimal)Configuration.CLoyaltyInfo.RedeemValue, 2);
                            }
                        }
                        */
                        #endregion

                        #region Sprint-18 - 2041 28-Oct-2014 JY added new logic along with corrected code - There are three CL point calculation methods as By Trans Amt, By line items and By quantity. Out of them first one is working fine but other two have issue. For "By line items" method line item count should be considered and for "By quantity" method total item quantity should be considered.
                        if (isCouponAmtExceeded == false) {
                            lineNetTotal = TotValidAmount - CouponAmt;
                            isCouponAmtExceeded = true;
                        }
                        if (Configuration.CLoyaltyInfo.PointsCalcMethod == "A")
                            oRow.LoyaltyPoints = (decimal)Math.Round((lineNetTotal) * PointsPerDollar, 2);
                        //else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "L")
                        //    oRow.LoyaltyPoints = (decimal)Math.Round((1) * PointsPerDollar, 2);
                        //else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "Q")
                        //    oRow.LoyaltyPoints = (decimal)Math.Round((lineTotalQty) * PointsPerDollar, 2);
                        #endregion

                        totalPoints += oRow.LoyaltyPoints;
                    } else {
                        if (Configuration.CLoyaltyInfo.PointsCalcMethod == "L")
                            oRow.LoyaltyPoints = (decimal)Math.Round((1) * PointsPerDollar, 2);
                        else if (Configuration.CLoyaltyInfo.PointsCalcMethod == "Q")
                            oRow.LoyaltyPoints = (decimal)Math.Round((lineTotalQty) * PointsPerDollar, 2);

                        totalPoints += oRow.LoyaltyPoints;
                    }
                }
            }
            return totalPoints;
        }

        public Decimal CalculatePointsForReturnTrans(TransDetailData oTransDetailData)
        {
            Decimal totalPoints = 0;

            foreach (TransDetailRow oRow in oTransDetailData.TransDetail.Rows) {
                totalPoints += oRow.LoyaltyPoints;
            }
            return totalPoints;
        }

        public bool DeactivateMergeCard(long iMergeCLCardID, List<long> iDeactivatingCardId)
        {
            bool RetVal = false;
            try {
                CLPointsRewardTier oCLPointsRewardTier = new CLPointsRewardTier();
                CLCardsSvr oCLCardsSvr = new CLCardsSvr();
                string InQuery = string.Join(",", iDeactivatingCardId.ToArray());
                CLCardsData oCLData = PopulateList(" WHERE CardId IN (" + InQuery + ")");
                if (Configuration.isNullOrEmptyDataSet(oCLData) == true) {
                    return RetVal;
                }
                foreach (CLCardsRow oCLCardRow in oCLData.CLCards.Rows) {
                    oCLCardRow.IsActive = false;
                }
                oCLPointsRewardTier.IsDeactivatingCard = true;
                if (iMergeCLCardID > 0) {
                    foreach (long CardId in iDeactivatingCardId) {
                        Decimal transactionPoints = oCLPointsRewardTier.GetCurrentTotalPoints(CardId);
                        oCLPointsRewardTier.ProcessPointsInTiers(transactionPoints, 0, iMergeCLCardID);
                        oCLPointsRewardTier.UpdatePointsInTiers(0, 0, CardId);
                    }
                }
                oCLCardsSvr.Persist(oCLData);
                RetVal = true;
            } catch (Exception ex) {
                throw (ex);
            }
            return RetVal;
        }

    }
}
