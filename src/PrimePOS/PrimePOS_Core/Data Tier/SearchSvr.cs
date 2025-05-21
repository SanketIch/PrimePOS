using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
using System.Text.RegularExpressions;   //Sprint-21 - 2206 15-Jul-2015 JY Added
//using POS.Resources;
using NLog;
using POS_Core.Resources.DelegateHandler;
using POS_Core.Resources;

namespace POS_Core.DataAccess
{
    // Provides search facility for all forms
    public class SearchSvr : IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        private bool isFor7Days = true;
        public bool IsFor7Days
        {
            set { isFor7Days = value; }
            get { return isFor7Days; }
        }
        public static bool ISManagePaytype = false; //Added By shitaljit for manage paytype. on 1 feb 2013 the flag will be responsible for not searching old payment types
        public static string strCustMasterSearchVal = string.Empty;//Added By shitaljit for Cust. Master Search .
        public static bool NameFlag = false;
        public static string StrName = "";
        //Added By Shitaljit(QuicSolv) on 3 June 2011
        public static bool SubDeptIDFlag = false;
        
        public SearchSvr() { }

        private DataSet Populate(string strQuery, SqlConnection oConn)
        {
            try
            {
                DataSet oDS = null;
                oDS = DataHelper.ExecuteDataset(oConn, CommandType.Text, strQuery);
                return oDS;
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "Populate(string strQuery, SqlConnection oConn)");
                throw (ex);
            }
        }

        //By SRT (Sachin) date : 13 Nov 2009
        //public DataSet SearchItem(System.String SearchTable,System.String SearchCode, System.String SearchName, System.Int32 ActiveOnly,System.String NumberOfRecord)
        //{
        //    string strField0 = "", strField1 = "", strField2 = "", strQuery = "", strDisplayFields = "", strWhere = "";
        //    strField1 = clsPOSDBConstants.Item_Fld_ItemID;
        //    strField2 = clsPOSDBConstants.Item_Fld_Description;

        //    strDisplayFields = clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
        //        clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
        //        clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
        //        clsPOSDBConstants.Item_Fld_Unit + " as Unit," +
        //        clsPOSDBConstants.Item_Fld_Location + " as Location," +
        //        clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
        //        clsPOSDBConstants.Item_Fld_Discount + " as Discount," +
        //        clsPOSDBConstants.Item_Fld_ExptDeliveryDate + " as [Delivery Date]," +
        //        clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level]," +
        //        clsPOSDBConstants.Item_Fld_SaleEndDate + " as [Sale End Date]," +
        //        clsPOSDBConstants.Item_Fld_SaleStartDate + " as [Sale Start Date]," +
        //        clsPOSDBConstants.Item_Fld_ProductCode + " as [SKU Code]," +
        //        clsPOSDBConstants.Item_Fld_Remarks;
        //    strQuery = "select Top "+ NumberOfRecord + " "+ strDisplayFields + " from " + SearchTable;
        //    using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
        //    {
        //        return (Populate(strQuery, conn));
        //    }
        //}
        //End Of By SRT (Sachin) date : 13 Nov 2009        

        // End of Aded By Shitaljit

        public DataSet Search(System.String SearchTable, System.String SearchCode, System.String SearchName, System.Int32 ActiveOnly, int AdditionalParameter, int EODID = 0, System.Boolean IsInActive = false, System.Boolean IsPrimeRxPayDisable = false)    //PRIMEPOS-2700 02-Jul-2019 JY Added EODID parameter//Added by Arvind 2664 //PRIMEPOS-3282
        {
            logger.Trace("Search(System.String SearchTable, System.String SearchCode, System.String SearchName, System.Int32 ActiveOnly, int AdditionalParameter) - " + clsPOSDBConstants.Log_Entering);
            string strField0 = "", strField1 = "", strField2 = "", strField3 = "", strQuery = "", strDisplayFields = "", strWhere = "";//strField3="" added by Ravindra(Quicsolv) to Search Customer By LastName And First Name
            string strOrderBy = "";
            string strActiveOnly = "";
            string strDisplayFields1 = string.Empty, strWhere1 = string.Empty, strQuery1 = string.Empty;    //18-Jun-2015 JY Added 
            //string strExpiryDateFilter = string.Empty;  //Sprint-21 - 2206 15-Jul-2015 JY Added   //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Commented

            if (SearchTable == clsPOSDBConstants.Vendor_tbl)
            {
                strField1 = clsPOSDBConstants.Vendor_Fld_VendorCode;
                strField2 = clsPOSDBConstants.Vendor_Fld_VendorName;

                strDisplayFields = clsPOSDBConstants.Vendor_Fld_VendorCode + " as Code," +
                                 clsPOSDBConstants.Vendor_Fld_VendorName + " as Name," +
                                clsPOSDBConstants.Vendor_Fld_Address1 + " as Address1," +
                                clsPOSDBConstants.Vendor_Fld_Address2 + " as Address2," +
                                clsPOSDBConstants.Vendor_Fld_City + " as City," +
                                clsPOSDBConstants.Vendor_Fld_CellNo + " as [Cell No.]," +
                                clsPOSDBConstants.Vendor_Fld_TelephoneNo + " [Phone No.]," +
                                clsPOSDBConstants.Vendor_Fld_Email + " as Email ," +
                                clsPOSDBConstants.Vendor_Fld_IsActive + " as IsActive ," +
                                clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode + " as PrimePOVendorCode , " +
                                clsPOSDBConstants.Vendor_Fld_UpdatePrice + " as  [Price Update]," +
                                clsPOSDBConstants.Vendor_Fld_VendorId;

                //Added by SRT(Abhishek) 12 Aug 2009
                //Added to sort the search by vendor
                strOrderBy = " order by " + clsPOSDBConstants.Vendor_Fld_VendorCode + " ASC";
                //End of Added by SRT(Abhishek)

                if (ActiveOnly == 1)
                {
                    strActiveOnly = clsPOSDBConstants.Vendor_Fld_IsActive + "=1";
                }
            }
            else if (SearchTable == clsPOSDBConstants.WarningMessages_tbl)
            {
                strField1 = clsPOSDBConstants.WarningMessages_Fld_WarningMessageID;
                strField2 = clsPOSDBConstants.WarningMessages_Fld_WarningMessage;

                strDisplayFields = clsPOSDBConstants.WarningMessages_Fld_WarningMessageID + " as Code," +
                                 clsPOSDBConstants.WarningMessages_Fld_WarningMessage + " as Name";
            }
            else if (SearchTable == clsPOSDBConstants.Customer_tbl)
            {
                strField1 = clsPOSDBConstants.Customer_Fld_AcctNumber;
                strField2 = clsPOSDBConstants.Customer_Fld_CustomerName;
                strField3 = clsPOSDBConstants.Customer_Fld_FirstName;//added by Ravindra(Quicsolv) to Search Customer By LastName And First Name

                strDisplayFields = clsPOSDBConstants.Customer_Fld_AcctNumber + " as [Acct. #]," +
                    clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name," +
                    clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                    clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                    clsPOSDBConstants.Customer_Fld_City + " as City," +
                    clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                    clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                    clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                    clsPOSDBConstants.Customer_Fld_Email + " as Email ," +
                    clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +//Added by Shitaljit on 20 Feb 2012
                    clsPOSDBConstants.Customer_Fld_CustomerId + " as CustomerId," +
                    clsPOSDBConstants.Customer_Fld_CustomerCode;    //PRIMEPOS-2987 12-Aug-2021 JY Added

                if (ActiveOnly == 1)
                {
                    strActiveOnly = clsPOSDBConstants.Customer_Fld_IsActive + "=1";
                }
            }
            else if (SearchTable == clsPOSDBConstants.CLPointsRewardTier_tbl)
            {
                strField1 = clsPOSDBConstants.CLPointsRewardTier_Fld_ID;
                strField2 = clsPOSDBConstants.CLPointsRewardTier_Fld_Description;
                strDisplayFields = clsPOSDBConstants.CLPointsRewardTier_Fld_ID + " as Code," +
                    clsPOSDBConstants.CLPointsRewardTier_Fld_Description + " as Name," +
                    clsPOSDBConstants.CLPointsRewardTier_Fld_Points + " as [Points]," +
                    clsPOSDBConstants.CLPointsRewardTier_Fld_Discount + " as [Discount]," +
                    " cast(" + Configuration.convertBoolToInt(Configuration.CLoyaltyInfo.IsTierValueInPercent) + " as bit) as [Is Discount In Percentage]";
            }
            else if (SearchTable == clsPOSDBConstants.CLCards_tbl)
            {
                strField1 = clsPOSDBConstants.CLCards_Fld_CLCardID;
                strField2 = clsPOSDBConstants.CLCards_Fld_Description;
                strDisplayFields = clsPOSDBConstants.CLCards_Fld_CLCardID + " as [CL Card #]," +
                    clsPOSDBConstants.CLCards_Fld_Description + " as [CL Card Name]," +
                    clsPOSDBConstants.CLCards_Fld_ExpiryDays + " as [Expiry Days]," +
                    clsPOSDBConstants.CLCards_Fld_IsPrepetual + " as [Does Not Expire?]," +
                    clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " as [Customer Name]," +
                    clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                    clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                    clsPOSDBConstants.Customer_Fld_City + " as City," +
                    clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                    clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                    clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                    clsPOSDBConstants.Customer_Fld_Email + " as Email," +
                    clsPOSDBConstants.Customer_tbl + "." + clsPOSDBConstants.Customer_Fld_CustomerId;
                strWhere = " WHERE " + clsPOSDBConstants.CLCards_tbl + "." + clsPOSDBConstants.CLCards_Fld_IsActive + " =1 ";
                SearchTable = clsPOSDBConstants.CLCards_tbl + " Left Join " + clsPOSDBConstants.Customer_tbl + " on " + clsPOSDBConstants.CLCards_tbl + "." + clsPOSDBConstants.CLCards_Fld_CustomerID + "=" +
                    clsPOSDBConstants.Customer_tbl + "." + clsPOSDBConstants.Customer_Fld_CustomerId;
            }
            else if (SearchTable == clsPOSDBConstants.Department_tbl)
            {
                strField1 = clsPOSDBConstants.Department_Fld_DeptCode;
                strField2 = clsPOSDBConstants.Department_Fld_DeptName;
                strDisplayFields = clsPOSDBConstants.Department_Fld_DeptCode + " as Code," +
                    clsPOSDBConstants.Department_Fld_DeptID + " as id," +
                    clsPOSDBConstants.Department_Fld_DeptName + " as Name," +
                    clsPOSDBConstants.Department_Fld_SaleStartDate + " as [Start Date]," +
                    clsPOSDBConstants.Department_Fld_SaleEndDate + " as [End Date]," +
                    clsPOSDBConstants.Department_Fld_SalePrice + " as [Sale Price]," +
                    clsPOSDBConstants.Department_Fld_Discount + " as Discount ";
            }
            else if (SearchTable == clsPOSDBConstants.SubDepartment_tbl)
            {
                strField1 = clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID;
                strField2 = clsPOSDBConstants.SubDepartment_Fld_Description;
                strField0 = clsPOSDBConstants.SubDepartment_Fld_DepartmentID;

                if (SubDeptIDFlag)
                {
                    strDisplayFields = "SD." + clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID + " as Code," +
                        "SD." + clsPOSDBConstants.SubDepartment_Fld_Description + " as [Sub Department Name]," +
                        "D." + clsPOSDBConstants.Department_Fld_DeptName + " as [Department Name]," +
                        "D." + clsPOSDBConstants.Department_Fld_DeptID + " as [Dept Code]";
                }
                else
                {
                    strDisplayFields = clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID + " as Code," +
                        clsPOSDBConstants.SubDepartment_Fld_Description + " as [Sub Department Name] ";
                }
            }
            else if (SearchTable == clsPOSDBConstants.TaxCodes_tbl)
            {
                strField1 = clsPOSDBConstants.TaxCodes_Fld_TaxCode;
                strField2 = clsPOSDBConstants.TaxCodes_Fld_Description;
                strDisplayFields = clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as Code," +
                                   clsPOSDBConstants.TaxCodes_Fld_Description + " as Description," +
                                   clsPOSDBConstants.TaxCodes_Fld_Amount + " as [Tax Rate] ," +
                                   clsPOSDBConstants.TaxCodes_Fld_TaxID +
                                   "," + clsPOSDBConstants.TaxCodes_Fld_TaxType +
                                   "," + clsPOSDBConstants.TaxCodes_Fld_Active;//2664

                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")//2664{
                {
                    if (!IsInActive)
                    {
                        strField1 = clsPOSDBConstants.TaxCodes_Fld_TaxCode;
                        strField2 = clsPOSDBConstants.TaxCodes_Fld_Description;
                        strDisplayFields = clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as Code," +
                                           clsPOSDBConstants.TaxCodes_Fld_Description + " as Description," +
                                           clsPOSDBConstants.TaxCodes_Fld_Amount + " as [Tax Rate] ," +
                                           clsPOSDBConstants.TaxCodes_Fld_TaxID +
                                           "," + clsPOSDBConstants.TaxCodes_Fld_TaxType +
                                           "," + clsPOSDBConstants.TaxCodes_Fld_Active;
                        //strWhere = " Where " + clsPOSDBConstants.TaxCodes_Fld_Active + "= 1";
                        strWhere = " Where " + clsPOSDBConstants.TaxCodes_Fld_Active + "= 1";
                    }
                }
            }
            #region Sprint-24 - PRIMEPOS-2364 13-Jan-2017 JY Added
            else if (SearchTable == clsPOSDBConstants.TaxCodes_With_NoTax)
            {
                strField1 = clsPOSDBConstants.TaxCodes_Fld_TaxCode;
                strField2 = clsPOSDBConstants.TaxCodes_Fld_Description;
                strDisplayFields = clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as Code," +
                                   clsPOSDBConstants.TaxCodes_Fld_Description + " as Description," +
                                   clsPOSDBConstants.TaxCodes_Fld_Amount + " as [Tax Rate] ," +
                                   clsPOSDBConstants.TaxCodes_Fld_TaxID +
                                   "," + clsPOSDBConstants.TaxCodes_Fld_TaxType;
            }
            #endregion
            else if (SearchTable == clsPOSDBConstants.PayType_tbl)
            {
                //Modified By Shitaljit for providing ability to add new paytypes.
                strField1 = clsPOSDBConstants.PayType_Fld_PayTypeID;
                strField2 = clsPOSDBConstants.PayType_Fld_PayTypeDescription;
                strDisplayFields = clsPOSDBConstants.PayType_Fld_PayTypeID + " as Code," +
                    clsPOSDBConstants.PayType_Fld_PayTypeDescription + " as Description," +
                    "ISNULL(" + clsPOSDBConstants.PayType_Fld_IsHide + ", 0) as IsHide," +  //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
                    "ISNULL(" + clsPOSDBConstants.PayType_Fld_StopAtRefNo + ", 0) as StopAtRefNo," +  //PRIMEPOS-2309 08-Mar-2019 JY Added
                    "ISNULL(" + clsPOSDBConstants.PayType_Fld_SortOrder + ", 0) as SortOrder";  //PRIMEPOS-2966 20-May-2021 JY Added
                if (ISManagePaytype == true)
                {
                    strWhere = "  WHERE PayTypeID NOT IN('1','2','3','4','5','6','7','B','C','E','H')";
                }
                #region PRIMEPOS-3282
                if (IsPrimeRxPayDisable == true)
                {
                    if (strWhere != "")
                    {
                        strWhere = "  OR PayTypeID !='O' ";
                    }
                    else
                    {
                        strWhere = "  WHERE PayTypeID !='O' ";
                    }
                }
                #endregion
                strOrderBy = " ORDER BY " + clsPOSDBConstants.PayType_Fld_SortOrder + "," + clsPOSDBConstants.PayType_Fld_CustomPayType + "," + clsPOSDBConstants.PayType_Fld_PayTypeID;   //PRIMEPOS-2940 30-Mar-2021 JY Added //PRIMEPOS-2966 20-May-2021 JY Added SortOrder
            }
            else if (SearchTable == clsPOSDBConstants.ItemMonitorCategory_tbl)
            {
                strField1 = clsPOSDBConstants.ItemMonitorCategory_Fld_ID;
                strField2 = clsPOSDBConstants.ItemMonitorCategory_Fld_Description;
                strDisplayFields = clsPOSDBConstants.ItemMonitorCategory_Fld_ID + " as Code," +
                    clsPOSDBConstants.ItemMonitorCategory_Fld_Description + " as Description," +
                    "ISNULL(" + clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE + ",0) as ePSE," +
                    //clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit + " as [Daily Limit]," + //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented
                    //clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit + " as [30 Days Limit]," +  //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Commented
                    clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays + " as [Limit Period Days]," +
                    clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty + " as [Limit Period Qty]," +
                    clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit + " as [Age Limit]," +//Added by Manoj 7/11/2013
                    "CASE " + clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy + " WHEN " +
                    "'S' THEN 'SIGNATURE' WHEN 'D' THEN 'DRIVER LICENCE' WHEN 'B' THEN 'SIGNATURE & DRIVER LICENCE' ELSE 'NONE' END" +
                    " as [Verification By]";
            }
            else if (SearchTable == clsPOSDBConstants.InvTransType_tbl)
            {
                strField1 = clsPOSDBConstants.InvTransType_Fld_ID;
                strField2 = clsPOSDBConstants.InvTransType_Fld_TypeName;
                strDisplayFields = clsPOSDBConstants.InvTransType_Fld_ID + " as Code," +
                    clsPOSDBConstants.InvTransType_Fld_TypeName + " as Description ";
            }
            else if (SearchTable == clsPOSDBConstants.Users_tbl)
            {
                strField1 = clsPOSDBConstants.Users_Fld_UserID;
                //Following commented by Krishna on 1 April 2011
                //strField2=clsPOSDBConstants.Users_Fld_loginRegistration;
                //Modified By Shitaljit(QuicSolv) on 23 May 2011(Addded field NoOfAttempt and IsLocked)
                //Modified By Shitaljit(QuicSolv) on 11 August 2011 (Added Name Field)
                strField2 = clsPOSDBConstants.Users_Fld_DrawNo;
                strDisplayFields = clsPOSDBConstants.Users_Fld_UserID + " as UserID," +
                    clsPOSDBConstants.Users_Fld_lName + " +'  '+ " + clsPOSDBConstants.Users_Fld_fName + " as [Name], " +
                    clsPOSDBConstants.Users_Fld_DrawNo + " as [Draw No.]," +
                    clsPOSDBConstants.Users_Fld_SecurityLevel + " as [Security Level]," +
                    clsPOSDBConstants.Users_Fld_ID + " ," +
                    clsPOSDBConstants.Users_Fld_loginRegistration + "," +
                    clsPOSDBConstants.Users_Fld_NoOfAttempt + " as [No Of Login Attempts]," +
                    clsPOSDBConstants.Users_Fld_IsLocked + " as [IsLocked], " +
                   clsPOSDBConstants.Users_Fld_IsActive;
                strWhere = " WHERE " + clsPOSDBConstants.Users_Fld_UserType + "<>'G'  OR " + clsPOSDBConstants.Users_Fld_UserType + " IS NULL ";//Added By Shitaljit on 26 Sept 13  for Adding user Group 
            }
            else if (SearchTable == clsPOSDBConstants.Item_tbl)
            {
                strField1 = clsPOSDBConstants.Item_Fld_ItemID;
                strField2 = clsPOSDBConstants.Item_Fld_Description;

                strDisplayFields = "1 as rnum, " + clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
                    clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
                    clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
                    clsPOSDBConstants.Item_Fld_Unit + " as Unit," +
                    clsPOSDBConstants.Item_Fld_Location + " as Location," +
                    clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
                    clsPOSDBConstants.Item_Fld_Discount + " as Discount," +
                    clsPOSDBConstants.Item_Fld_ExptDeliveryDate + " as [Delivery Date]," +
                    clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level]," +
                    clsPOSDBConstants.Item_Fld_SaleEndDate + " as [Sale End Date]," +
                    clsPOSDBConstants.Item_Fld_SaleStartDate + " as [Sale Start Date]," +
                    clsPOSDBConstants.Item_Fld_ProductCode + " as [SKU Code]," +
                    clsPOSDBConstants.Item_Fld_Remarks + "," +
                    "ISNULL(" + clsPOSDBConstants.Item_Fld_IsNonRefundable + ",0)" + " as [Non-Refundable]," +   //PRIMEPOS-2592 06-Nov-2018 JY Added 
                    clsPOSDBConstants.Item_Fld_ExpDate + " AS [Exp. Date]," +
                    clsPOSDBConstants.Item_Fld_PreferredVendor + " AS [Preferred Vendor]," +
                    clsPOSDBConstants.Item_Fld_LastVendor + " AS [Last Vendor]";

                #region 18-Jun-2015 JY Added
                if (SearchName.Trim() != "")
                {
                    strDisplayFields1 = "2 as rnum, " + clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
                    clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
                    clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
                    clsPOSDBConstants.Item_Fld_Unit + " as Unit," +
                    clsPOSDBConstants.Item_Fld_Location + " as Location," +
                    clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
                    clsPOSDBConstants.Item_Fld_Discount + " as Discount," +
                    clsPOSDBConstants.Item_Fld_ExptDeliveryDate + " as [Delivery Date]," +
                    clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level]," +
                    clsPOSDBConstants.Item_Fld_SaleEndDate + " as [Sale End Date]," +
                    clsPOSDBConstants.Item_Fld_SaleStartDate + " as [Sale Start Date]," +
                    clsPOSDBConstants.Item_Fld_ProductCode + " as [SKU Code]," +
                    clsPOSDBConstants.Item_Fld_Remarks + "," +
                    "ISNULL(" + clsPOSDBConstants.Item_Fld_IsNonRefundable + ",0)" + " as [Non-Refundable]," +   //PRIMEPOS-2592 06-Nov-2018 JY Added 
                    clsPOSDBConstants.Item_Fld_ExpDate + " AS [Exp. Date]," +
                    clsPOSDBConstants.Item_Fld_PreferredVendor + " AS [Preferred Vendor]," +
                    clsPOSDBConstants.Item_Fld_LastVendor + " AS [Last Vendor]";
                }
                #endregion
            }
            else if (SearchTable == clsPOSDBConstants.item_PriceInv_Lookup)
            {
                //Modified By Shitaljit(QuicSolv) on 8 August 2011
                //Added Cost Price and Last Vendor.
                strField1 = clsPOSDBConstants.Item_Fld_ItemID;
                strField2 = clsPOSDBConstants.Item_Fld_Description;
                strDisplayFields = clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
                clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
                clsPOSDBConstants.Item_Fld_Unit + " as [U/M]," +
                clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
                clsPOSDBConstants.Item_Fld_LastCostPrice + " as [Cost Price], " +
                clsPOSDBConstants.Item_Fld_LastVendor + " as [Vendor], " +
                clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
                clsPOSDBConstants.Item_Fld_Location + " as Location";

                SearchTable = clsPOSDBConstants.Item_tbl;
            }
            else if (SearchTable == clsPOSDBConstants.POHeader_tbl)
            {
                strField1 = clsPOSDBConstants.POHeader_Fld_OrderNo;
                strField2 = clsPOSDBConstants.Vendor_Fld_VendorName;
                //Start: Changed by PRASHAT(SRT) Date:8-7-09
                strDisplayFields =
                    "PO." + clsPOSDBConstants.POHeader_Fld_OrderID + " , "
                    + clsPOSDBConstants.POHeader_Fld_Flagged + "  AS Template , PO."
                    + clsPOSDBConstants.POHeader_Fld_OrderNo + " , "
                    + clsPOSDBConstants.Vendor_Fld_VendorName + " as Vendor ,"
                    + clsPOSDBConstants.POHeader_Fld_Description + " as Reference,"
                    + "case " + clsPOSDBConstants.POHeader_Fld_Status +
                    " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 10  then 'PartiallyAck' when 11  then 'PartiallyAck-Reorder' when 12 then 'Error' when 13 then 'Overdue' when 14 then 'SubmittedManually' when 15 then 'DirectAcknowledge' when 16 then 'DeliveryReceived' when 17 then 'DirectDelivery' end  as [PO Status]," //Change by SRT (Sachin) Date : 19 Feb 2010
                    + "os.[Total Items],os.[Total Qty],os.[Total Price] as [Total Cost], "
                    + " (CONVERT(VARCHAR, OrderDate, 6)) as [Order Date] ,(CONVERT(VARCHAR, OrderDate, 8))  as [Order Time], (CONVERT(VARCHAR, "
                    + clsPOSDBConstants.POHeader_Fld_ExptDelvDate + ", 6)) as [Exp. Delivery Dt.]";

                SearchTable = SearchTable + " as PO," + clsPOSDBConstants.Vendor_tbl + " as ven ,(select PO1.OrderID,COUNT(pod.ItemID) as [Total Items],SUM(pod.Qty)as [Total Qty],SUM(pod.Qty*pod.Cost) as [Total Price] from " +
                clsPOSDBConstants.POHeader_tbl + " as PO1, " + clsPOSDBConstants.PODetail_tbl + " as pod where po1.OrderID = pod.OrderID group by po1.OrderID) as os";

                if (IsFor7Days)
                    strWhere = " where po.vendorid=ven.vendorid and os.OrderId=PO.OrderID and po.OrderDate between CURRENT_TIMESTAMP-7 and CURRENT_TIMESTAMP";
                else
                    strWhere = " where po.vendorid=ven.vendorid and os.OrderId=PO.OrderID and pO." + clsPOSDBConstants.POHeader_Fld_Flagged + " = 1";

                //End: Changed by PRASHAT(SRT) Date:8-7-09
                strOrderBy = " order by " + clsPOSDBConstants.POHeader_Fld_OrderDate + " DESC";
            }

            else if (SearchTable == clsPOSDBConstants.POHeader_CompNotRecvd)
            {
                strField1 = clsPOSDBConstants.POHeader_Fld_OrderNo;
                strField2 = clsPOSDBConstants.Vendor_Fld_VendorName;
                //Changed by Prashant(SRT) Date:30-5-09
                strDisplayFields = clsPOSDBConstants.POHeader_Fld_OrderID + " , " +
                    clsPOSDBConstants.POHeader_Fld_Flagged + " , " + clsPOSDBConstants.POHeader_Fld_OrderNo + " , " + clsPOSDBConstants.POHeader_Fld_Description + " , " +
                    clsPOSDBConstants.Vendor_Fld_VendorName + " as Vendor ," +
                    " (CONVERT(VARCHAR, OrderDate, 6)) as [Order Date] ,(cast(DATEPART(hh, " + clsPOSDBConstants.POHeader_Fld_OrderDate + ") as VARCHAR)+':'+cast(DATEPART(mi, " + clsPOSDBConstants.POHeader_Fld_OrderDate + ")as VARCHAR)+':'+cast(DATEPART(ss, " + clsPOSDBConstants.POHeader_Fld_OrderDate + ")as VARCHAR)) as [Order Time],cast(" +
                    clsPOSDBConstants.POHeader_Fld_ExptDelvDate + " as varchar) as [Expected Delivary Date] ";
                SearchTable = clsPOSDBConstants.POHeader_tbl + " as PO," + clsPOSDBConstants.Vendor_tbl + " as ven ";
                strWhere = " where po.vendorid=ven.vendorid and ((isnull(po.status,0)=1 and isnull(isInvRecieved,0)=0 and  isnull(po.isFTPUsed,0)=0) or   " +
                    " (isnull(po.status,0)=1 and isnull(isInvRecieved,0)=0 and  isnull(po.isFTPUsed,0)=2)) ";
                strOrderBy = " order by " + clsPOSDBConstants.POHeader_Fld_OrderDate + " ";
                //End of Changed by Prashant(SRT) Date:30-5-09
            }
            else if (SearchTable == clsPOSDBConstants.VendorCostPrice_View)
            {
                strField1 = clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID;
                strField2 = clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID;

                #region Commented By abhsiehk(SRT)  for price history from Item vendor table

                //strDisplayFields=
                //clsPOSDBConstants.InvRecvDetail_Fld_ItemID + " as [Item Code], " +
                //clsPOSDBConstants.Vendor_Fld_VendorCode + " as [Vendor Code], " +
                //clsPOSDBConstants.Vendor_Fld_VendorName + " as [Vendor Name], " +
                //clsPOSDBConstants.InvRecvHeader_Fld_RefNo + " as 'Reference No' , " +
                //clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate + " as [Recieve Date], " +
                //clsPOSDBConstants.InvRecvDetail_Fld_Cost + " as [Cost Price], " +
                //clsPOSDBConstants.InvRecvDetail_Fld_SalePrice + " as [Sale Price], " +
                ////clsPOSDBConstants.InvRecvDetail_Fld_QTY + " as [Qty Recieved], " +
                ////clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered + " as [Qty Ordered], " +
                //clsPOSDBConstants.InvRecvDetail_Fld_Comments + " as [Comments]" ;
                //SearchTable= clsPOSDBConstants.InvRecvHeader_tbl + " as invH," + clsPOSDBConstants.Vendor_tbl + " as ven ," + clsPOSDBConstants.InvRecvDetail_tbl + " as invD ";
                //strWhere = " where invH.vendorid=ven.vendorid  and invH.invrecievedid=invd.invrecievedid  " ;
                //strOrderBy=" order by " + clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate + " desc";

                #endregion Commented By abhsiehk(SRT)  for price history from Item vendor table

                strDisplayFields = clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code] , " +
                          clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " as [VendorItem Code], " +
                          clsPOSDBConstants.Item_Fld_Description + " as [Item Description] , " +
                          clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice + " as [Vendor CostPrice] ," +
                          clsPOSDBConstants.Vendor_Fld_VendorCode + " as [Vendor Code] ," +
                          clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level], " +
                          clsPOSDBConstants.Item_Fld_QtyInStock + " as [Quantity InStock]";

                SearchTable = clsPOSDBConstants.Item_tbl + " , " + clsPOSDBConstants.ItemVendor_tbl + " , " + clsPOSDBConstants.Vendor_tbl;
                strWhere = " where " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + "=" + clsPOSDBConstants.ItemVendor_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID +
                    " AND " + clsPOSDBConstants.Vendor_tbl + "." + clsPOSDBConstants.Vendor_Fld_VendorId + "=" + clsPOSDBConstants.ItemVendor_tbl + "." + clsPOSDBConstants.Vendor_Fld_VendorId;
            }
            else if (SearchTable == clsPOSDBConstants.TransHeader_tbl)
            {
                strField1 = clsPOSDBConstants.TransHeader_Fld_TransID;
                strDisplayFields = clsPOSDBConstants.TransHeader_Fld_TransID + "," +
                    " cast(" + clsPOSDBConstants.TransHeader_Fld_TransDate + " as varchar) as [Trans Date] ," + clsPOSDBConstants.TransHeader_Fld_TransType + " , " + clsPOSDBConstants.TransHeader_Fld_GrossTotal + " , " +
                    clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount + "," +
                    clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount + "," +
                    clsPOSDBConstants.TransHeader_Fld_TenderedAmount + "," + clsPOSDBConstants.TransHeader_Fld_TotalPaid + "," +
                    clsPOSDBConstants.TransHeader_Fld_Account_No;
                SearchTable = clsPOSDBConstants.TransHeader_tbl;
                strOrderBy = " order by " + clsPOSDBConstants.TransHeader_Fld_TransID + " desc ";
            }
            else if (SearchTable == clsPOSDBConstants.PayOutCat_tbl)
            {
                strField1 = clsPOSDBConstants.payoutCat_Fld_Id;
                strField2 = clsPOSDBConstants.PayOutCat_Fld_PayoutType;
                strDisplayFields = clsPOSDBConstants.payoutCat_Fld_Id + " , " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + " as Category," +
                clsPOSDBConstants.PayOutCat_Fld_UserId;
            }
            else if (SearchTable == clsPOSDBConstants.Coupon_tbl)
            {
                strField1 = clsPOSDBConstants.Coupon_Fld_CouponCode;
                strField2 = clsPOSDBConstants.Coupon_Fld_CouponID;
                strDisplayFields = clsPOSDBConstants.Coupon_Fld_CouponID + " as ID," +
                    clsPOSDBConstants.Coupon_Fld_CouponCode + " as [Coupon Code]," +
                    clsPOSDBConstants.Coupon_Fld_CouponDesc + " as [Description]," +    //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added
                    clsPOSDBConstants.Coupon_Fld_DiscountPerc + " as [Discount %]," +
                    clsPOSDBConstants.Coupon_Fld_CreatedDate + " as [Created Date]," +
                    clsPOSDBConstants.Coupon_Fld_StartDate + " as [Start Date]," +
                    clsPOSDBConstants.Coupon_Fld_EndDate + " as [End Date]," +
                clsPOSDBConstants.Coupon_Fld_UserID + " as [User]";
            }
            else if (SearchTable == clsPOSDBConstants.Department_tbl)
            {
                strField1 = clsPOSDBConstants.Department_Fld_DeptCode;
                strField2 = clsPOSDBConstants.Department_Fld_DeptName;
                strDisplayFields = clsPOSDBConstants.Department_Fld_DeptCode + " as Code," +
                    clsPOSDBConstants.Department_Fld_DeptID + " as id," +
                    clsPOSDBConstants.Department_Fld_DeptName + " as Name," +
                    clsPOSDBConstants.Department_Fld_SaleStartDate + " as [Start Date]," +
                    clsPOSDBConstants.Department_Fld_SaleEndDate + " as [End Date]," +
                    clsPOSDBConstants.Department_Fld_SalePrice + " as [Sale Price]," +
                    clsPOSDBConstants.Department_Fld_Discount + " as Discount ";
            }
            else if (SearchTable == clsPOSDBConstants.ItemComboPricing_tbl)
            {
                strField1 = clsPOSDBConstants.ItemComboPricing_Fld_Id;
                strField2 = clsPOSDBConstants.ItemComboPricing_Fld_Description;
                strDisplayFields = clsPOSDBConstants.ItemComboPricing_Fld_Id + " as Code," +
                    clsPOSDBConstants.ItemComboPricing_Fld_Description + " as Name," +
                    clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing + " as [Use Combo Item Price]," +
                    clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice + " as [Combo Item Price]";
            }
            #region Sprint-24 - PRIMEPOS-2299 09-Dec-2016 JY Added
            else if (SearchTable == clsPOSDBConstants.DeptComboPricing_tbl)
            {
                strField1 = clsPOSDBConstants.DeptComboPricing_Fld_Id;
                strField2 = clsPOSDBConstants.DeptComboPricing_Fld_DeptComboDesc;
                strDisplayFields = clsPOSDBConstants.DeptComboPricing_Fld_Id + " as Code," +
                    clsPOSDBConstants.DeptComboPricing_Fld_DeptComboDesc + " as Name," +
                    clsPOSDBConstants.DeptComboPricing_Fld_ForceGroupPricing + " as [Use Combo Dept Price]," +
                    clsPOSDBConstants.DeptComboPricing_Fld_DeptComboPrice + " as [Combo Dept Price]";
            }
            #endregion
            #region PRIMEPOS-3116 11-Jul-2022 JY Added
            else if (SearchTable == clsPOSDBConstants.TransFee_tbl)
            {
                strField1 = clsPOSDBConstants.TransFee_Fld_TransFeeID;
                strField2 = clsPOSDBConstants.TransFee_Fld_TransFeeDesc;
                strDisplayFields = "a.TransFeeID, a.TransFeeDesc, a.ChargeTransFeeFor, CASE WHEN a.ChargeTransFeeFor = 0 THEN 'Both (Sale & Return)' WHEN a.ChargeTransFeeFor = 1 THEN 'Sale Trans' WHEN a.ChargeTransFeeFor = 2 THEN 'Return Trans' END AS ChargeTransFee, a.TransFeeMode, CASE WHEN ISNULL(a.TransFeeMode,0) = 0 THEN 'Trans Fee in Percentage' ELSE 'Trans Fee in Amount' END AS TransFeeType, a.TransFeeValue, a.PayTypeID, b.PayTypeDesc, a.IsActive";
            }
            #endregion

            //strQuery = "select " + strDisplayFields + " from " + SearchTable;//Orignal Commented by Krishna on 30 June 2011
            if (SubDeptIDFlag)
            {
                //strQuery = "select " + strDisplayFields + " from " + SearchTable + " SD,Department D";    //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Commented
                strQuery = "select " + strDisplayFields + " from " + SearchTable + " SD INNER JOIN Department D ON D.DeptId = SD.DepartmentId";    //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added for query optimization and correction
            }
            else
            {
                if (SearchTable == clsPOSDBConstants.TaxCodes_With_NoTax)   //Sprint-24 - PRIMEPOS-2364 13-Jan-2017 JY Added if clause
                {
                    strQuery = "SELECT '" + clsPOSDBConstants.TaxCodes_NoTaxCode + "' as Code, '" +
                                   clsPOSDBConstants.TaxCodes_NoTax_Desc + "' as Description, 0.00 AS [Tax Rate], 0 AS TaxID, '' AS TaxType UNION " +
                                " SELECT " + strDisplayFields + " FROM " + clsPOSDBConstants.TaxCodes_tbl;
                }
                #region PRIMEPOS-3116 11-Jul-2022 JY Added
                else if (SearchTable == clsPOSDBConstants.TransFee_tbl)
                {
                    strQuery = "SELECT " + strDisplayFields + " FROM TransactionFee a" +
                            " INNER JOIN(SELECT '-99' AS PayTypeID, 'Credit Card' AS PayTypeDesc UNION SELECT PayTypeID, PayTypeDesc FROM PayType) b ON a.PayTypeID = b.PayTypeID";
                }
                #endregion
                else
                {
                    strQuery = "select " + strDisplayFields + " from " + SearchTable;
                }
                if (SearchTable == clsPOSDBConstants.Item_tbl && SearchName.Trim() != "" && strDisplayFields1.Trim() != String.Empty)  //18-Jun-2015 JY Added if condition to filter items by matching any string within the Item Description
                {
                    strQuery1 = "select " + strDisplayFields1 + " from " + SearchTable;
                }
            }

            if(AdditionalParameter != -1)
                if(strWhere == "")
                    strWhere = " where " + strField0 + " = " + AdditionalParameter.ToString() + "";
                else
                    strWhere += " and " + strField0 + " = " + AdditionalParameter.ToString() + "";

            #region Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Commented
            //#region Sprint-21 - 2206 15-Jul-2015 JY Added
            //if (SearchTable == clsPOSDBConstants.Item_tbl)
            //{
            //    if (SearchCode.Contains("\r\n"))
            //    {
            //        string[] strArr = Regex.Split(SearchCode, "\r\n");
            //        SearchCode = strArr[0].Trim();
            //        strExpiryDateFilter = strArr[1].Trim();
            //    }
            //}
            //#endregion
            #endregion

            if (SearchCode != "")
            {
                if (strWhere == "")
                {
                    //strWhere = " where LTRIM(RTRIM(" + strField1 + ")) like '" + SearchCode.Replace("'","''") + "%'";
                    //Change by SRT(Sachin) Date : 23 Nov 2009
                    //Removing LTRIM and RTRIM

                    //Following if part is Added By Shitaljit(QuicSolv) on 3 June 2011
                    //To Search Sub Department By DepartmentID
                    if (SubDeptIDFlag)
                    {
                        //strWhere = " where D.DeptId=SD.DepartmentId AND " + strField0 + " IN('" + SearchCode + "')";  //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Commented
                        strWhere = " where 1=1 AND " + strField0 + " IN('" + SearchCode + "')";    //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added for query optimization and correction
                    }
                    else
                    {
                        strWhere = " where " + strField1 + " like '" + SearchCode.Replace("'", "''") + "%'";
                        //if (SearchTable == clsPOSDBConstants.Item_tbl && strExpiryDateFilter != "") //Sprint-21 - 2206 15-Jul-2015 JY Added for exp. date filter    //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Commented
                        //{
                        //    strWhere += " AND " + strExpiryDateFilter;
                        //}
                        if (SearchTable == clsPOSDBConstants.Item_tbl && SearchName.Trim() != "")  //18-Jun-2015 JY Added if condition to filter items by matching any string within the Item Description
                        {
                            strWhere1 = " where " + strField1 + " like '" + SearchCode.Replace("'", "''") + "%'";
                            //if (strExpiryDateFilter != "")  //Sprint-21 - 2206 15-Jul-2015 JY Added for exp. date filter    //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Commented
                            //{
                            //    strWhere1 += " AND " + strExpiryDateFilter;
                            //}
                        }
                    }
                }
                else
                    strWhere += " and " + strField1 + " like '" + SearchCode.Replace("'", "''") + "%'";
            }
            //else    //Sprint-21 - 2206 15-Jul-2015 JY Added else part for exp. date //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Commented
            //{
            //    if (SearchTable == clsPOSDBConstants.Item_tbl && strExpiryDateFilter != "")
            //    {
            //        strWhere = " WHERE " + strExpiryDateFilter;
            //        if (SearchName.Trim() != "") 
            //        {
            //            strWhere1 = " WHERE " + strExpiryDateFilter;
            //        }
            //    }
            //}

            //Added by krishna on 4 May 2011
            if(NameFlag == true && StrName != "")
            {
                SearchName = StrName;
                StrName = "";
            }
            //Till here Added by Krishna on 4 may 2011
            if(SearchName != "")
            {
                if(strWhere == "")
                {
                    if(SearchTable == clsPOSDBConstants.Customer_tbl && string.IsNullOrEmpty(strCustMasterSearchVal) == false)
                    {
                        strWhere = "where " + clsPOSDBConstants.Customer_Fld_CustomerCode + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_CustomerName + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_FirstName + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_Address1 + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_Address2 + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Zip + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_City + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_CellNo + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_PhoneOffice + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_Comments + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_FaxNo + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_DriveLicNo + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                  + " CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_DriveLicState + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_PatientNo + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_PhoneHome + " LIKE (" + "'%" + strCustMasterSearchVal + "%')OR   "
                    + clsPOSDBConstants.Customer_Fld_Email + " LIKE (" + "'%" + strCustMasterSearchVal + "%')  ";
                    }
                    else if(SearchTable == clsPOSDBConstants.Customer_tbl && SearchName.Contains(","))//added by Ravindra(Quicsolv) to Search Customer By LastName And First Name
                    {
                        string lName = "";
                        string fName = "";

                        char[] seperater = { ',' };
                        string[] lFname = SearchName.Split(seperater);
                        lName = lFname[0].Trim();
                        if(lFname.Length > 1)
                            fName = lFname[1].Trim();

                        strWhere = " where " + strField2 + " Like '" + lName.Trim() + "%' AND " + strField3 + " Like '" + fName.Trim() + "%'";
                    }
                    else
                    {
                        if (SearchTable == clsPOSDBConstants.Item_tbl)  //18-Jun-2015 JY Added if condition to filter items by matching any string within the Item Description
                        {
                            strWhere = " where " + strField2 + " Like '" + SearchName.Replace("'", "''") + "%'";
                            strWhere1 = " where " + strField2 + " Like '%" + SearchName.Replace("'", "''") + "%' AND " + strField2 + " NOT Like '" + SearchName.Replace("'", "''") + "%'";
                        }
                        else
                            strWhere = " where " + strField2 + " Like '" + SearchName.Replace("'", "''") + "%'";
                    }
                }
                else
                    //Edited By Krishna on 4 may 2011--------------------------
                    //				strWhere += " and " + strField2 + " Like '" + SearchName.Replace("'","''") + "%'";//ORIGNAl
                    if(NameFlag == true)
                    {
                        if(SearchTable == clsPOSDBConstants.Customer_tbl && SearchName.Contains(","))//added by Ravindra(Quicsolv) to Search Customer By LastName And First Name
                        {
                            string lName = "";
                            string fName = "";

                            char[] seperater = { ',' };
                            string[] lFname = SearchName.Split(seperater);
                            lName = lFname[0].Trim();
                            if(lFname.Length > 1)
                                fName = lFname[1].Trim();

                            strWhere = " or  (" + strField2 + " Like '" + lName.Trim() + "%' AND " + strField3 + " Like '" + fName.Trim() + "%')";
                        }
                        else
                        {
                            strWhere += " OR " + strField2 + " Like '" + SearchName.Replace("'", "''") + "%'";
                        }
                        NameFlag = false;
                    }
                    else
                    {
                        if (SearchTable == clsPOSDBConstants.Customer_tbl && SearchName.Contains(","))//added by Ravindra(Quicsolv) to Search Customer By LastName And First Name
                        {
                            string lName = "";
                            string fName = "";

                            char[] seperater = { ',' };
                            string[] lFname = SearchName.Split(seperater);
                            lName = lFname[0].Trim();
                            if (lFname.Length > 1)
                                fName = lFname[1].Trim();

                            strWhere += " AND " + strField2 + " Like '" + lName.Trim() + "%' AND " + strField3 + " Like '" + fName.Trim() + "%'";
                        }
                        else
                        {
                            if (SearchTable == clsPOSDBConstants.Item_tbl)  //18-Jun-2015 JY Added if condition to filter items by matching any string within the Item Description
                            {
                                strWhere += " AND " + strField2 + " Like '" + SearchName.Replace("'", "''") + "%'";
                                strWhere1 += " AND " + strField2 + " Like '%" + SearchName.Replace("'", "''") + "%' AND " + strField2 + " NOT Like '" + SearchName.Replace("'", "''") + "%'";
                            }
                            else
                                strWhere += " AND " + strField2 + " Like '" + SearchName.Replace("'", "''") + "%'";
                        }
                    }
                //Till here by Krishna------------------------------------------
            }

            strQuery += strWhere;
            if (SearchTable == clsPOSDBConstants.Item_tbl && SearchName.Trim() != "" && strQuery1.Trim() != String.Empty)  //18-Jun-2015 JY Added if condition to filter items by matching any string within the Item Description
            {
                strQuery1 += strWhere1;
            }

            if(strActiveOnly != "")
            {
                if(strWhere == "")
                    strQuery += " where " + strActiveOnly;
                else
                    strQuery += " and " + strActiveOnly;
            }


            if (SearchTable == clsPOSDBConstants.Item_tbl && SearchName.Trim() != "")  //18-Jun-2015 JY Added if condition to filter items by matching any string within the Item Description
            {
            }
            else
                strQuery += strOrderBy;

            if(SearchTable == clsPOSDBConstants.StationCloseHeader_tbl)
            {
                //Modified By Shitaljit(QuicSolv) on 17 June 2011
                //Added IsVerifeid field on the query.
                if (EODID > 0)
                {
                    strQuery = " SELECT StationClose." +  clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID + "," +
                        " ps.stationname as Station," +
                        " StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_CloseDate + "," +
                        " StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_EODID + "," +
                        " StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_UserId + "," +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'S' AND StationCloseId = StationClose.StationCloseId ) As [Total Sale] , " +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'SR' AND StationCloseId = StationClose.StationCloseId ) As [Total Returns] , " +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'TX' AND StationCloseId = StationClose.StationCloseId ) As [Total Tax] , " +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'DT' AND StationCloseId = StationClose.StationCloseId ) As [Total Discount] , " +
                        " (SELECT sum(TransAmount) FROM StationCloseDetail WHERE TransType = 'TF' AND StationCloseId = StationClose.StationCloseId) As [Transaction Fee], " +   //PRIMEPOS-3118 03-Aug-2022 JY Added
                        " (SELECT TOP 1 subSC.IsVerified FROM  StationCloseCash subSC WHERE subSC.StationCloseID = StationClose.StationcloseId) as [IsVerified]" +
                    " FROM " +
                        clsPOSDBConstants.StationCloseHeader_tbl +
                        " StationClose, util_POSSet ps where ps.stationid = stationclose.stationid and StationClose." +  clsPOSDBConstants.StationCloseHeader_Fld_EODID +
                        " = " + EODID +
                        " order by StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID + " desc";
                }
                else
                {
                    strQuery = " SELECT StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID + "," +
                        " ps.stationname as Station," +
                        " StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_CloseDate + "," +
                        " StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_EODID + "," +
                        " StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_UserId + "," +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'S' AND StationCloseId = StationClose.StationCloseId) As [Total Sale], " +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'SR' AND StationCloseId = StationClose.StationCloseId) As [Total Returns], " +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'TX' AND StationCloseId = StationClose.StationCloseId) As [Total Tax], " +
                        " (SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'DT' AND StationCloseId = StationClose.StationCloseId) As [Total Discount], " +
                        " (SELECT sum(TransAmount) FROM StationCloseDetail WHERE TransType = 'TF' AND StationCloseId = StationClose.StationCloseId) As [Transaction Fee], " +   //PRIMEPOS-3118 03-Aug-2022 JY Added
                        " (SELECT TOP 1 subSC.IsVerified FROM  StationCloseCash subSC WHERE subSC.StationCloseID = StationClose.StationcloseId) as [IsVerified]" +
                        " FROM " + clsPOSDBConstants.StationCloseHeader_tbl +
                        " StationClose, util_POSSet ps where ps.stationid = stationclose.stationid and StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_CloseDate +
                        " between  cast('" + SearchCode + "' as datetime) and cast('" + SearchName + "' as datetime)" +
                        "  order by StationClose." + clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID + " desc";
                }
            }
            else if(SearchTable == clsPOSDBConstants.EndOfDay_tbl)
            {
                strQuery = " select EODID,UserID,CloseDate as [Close Date], TotalSales as [Total Sales], " +
                        " TotalReturns as [Total Returns],TotalDiscount as [Total Discount],TotalTax as [Total Tax], TotalTransFee AS [Trans Fee] " +   //PRIMEPOS-3118 03-Aug-2022 JY Added TotalTransFee
                        " FROM " +
                        clsPOSDBConstants.EndOfDay_tbl +
                        " where CloseDate " +
                        " between  cast('" + SearchCode + "' as datetime) and cast('" + SearchName + "' as datetime)" +
                        " order by eodid desc ";
            }
            #region Sprint-19 - 1883 20-Mar-2015 JY Added
            else if (SearchTable == "ITEMLOCATION")
            {
                strQuery = "SELECT DISTINCT LOCATION FROM ITEM WHERE LOCATION IS NOT NULL AND LOCATION <> '' ORDER BY LOCATION";
            }
            #endregion

            using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                if (SearchTable == clsPOSDBConstants.Item_tbl)  //18-Jun-2015 JY Added if condition to filter items by matching any string within the Item Description
                {
                    if (SearchName.Trim() != "" && strQuery1.Trim() != String.Empty)
                    {
                        strQuery += " UNION " + strQuery1 + " order by 1, 5";
                    }
                    DataSet ds = Populate(strQuery, conn);
                    ds.Tables[0].Columns.RemoveAt(0);
                    return (ds);
                }
                logger.Trace("Search(System.String SearchTable, System.String SearchCode, System.String SearchName, System.Int32 ActiveOnly, int AdditionalParameter) - " + clsPOSDBConstants.Log_Exiting);
                return (Populate(strQuery, conn));
            }            
        }

        public DataSet Search(String strQuery)
        {
            using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(strQuery, conn));
            }
        }

        public string SearchScalar(String strQuery)
        {
            logger.Trace("SearchScalar(String strQuery) - " + clsPOSDBConstants.Log_Entering);
            string strReturn = "";

            using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                object objReturn = DataHelper.ExecuteScalar(conn, CommandType.Text, strQuery);
                if(objReturn != null)
                {
                    strReturn = objReturn.ToString();
                }
            }
            logger.Trace("SearchScalar(String strQuery) - " + clsPOSDBConstants.Log_Exiting);
            return strReturn;
        }

        public bool DeleteUserRow(string UserID, bool bUser = true) //PRIMEPOS-2780 27-Sep-2021 JY added bUser parameter
        {
            string sSQL;
            bool bDelete = false;
            try
            {
                logger.Trace("DeleteUserRow(string UserID) - " + clsPOSDBConstants.Log_Entering);
                #region PRIMEPOS-2780 27-Sep-2021 JY Added

                if (bUser)
                {
                    DataTable dt = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, "SELECT TOP 1 TransID FROM POSTransaction WHERE UserID = '" + UserID.Replace("'", "''") + "'").Tables[0];
                    if (dt != null && dt.Rows.Count == 0)
                    {
                        dt = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, "SELECT TOP 1 OrderID FROM PurchaseOrder WHERE UserID = '" + UserID.Replace("'", "''") + "'").Tables[0];
                        if (dt != null && dt.Rows.Count == 0)
                        {
                            sSQL = "DELETE FROM Users WHERE UserID = '" + UserID + "'";
                            DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                            bDelete = true;
                        }
                    }
                    if (!bDelete)
                    {
                        if (MessageBox.Show("You cannot delete selected user as it is being referenced in the trasnaction(s). Do you want to make it inactive?","PrimePOS",MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            sSQL = "UPDATE Users SET IsActive = 0 WHERE UserID = '" + UserID + "'";
                            DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                            bDelete = false;
                        }
                    }
                }
                else
                {
                    DataTable dt = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, "SELECT ID FROM Users WHERE UserID = '" + UserID.Replace("'", "''") + "'").Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataTable dt1 = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, "SELECT TOP 1 GroupID FROM Users WHERE GroupID = " + Configuration.convertNullToInt(dt.Rows[0][0])).Tables[0];
                        if (dt1 != null && dt1.Rows.Count == 0)
                        {
                            sSQL = "delete from Users where UserID = '" + UserID + "'";
                            DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                            bDelete = true;
                        }
                        else
                        {
                            MessageBox.Show("You cannot delete selected group as it is being referenced in the users table.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            bDelete = false;
                        }
                    }
                }
                #endregion
                //sSQL = " delete from Users where UserID = '" + UserID + "'";
                //DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                logger.Trace("DeleteUserRow(string UserID) - " + clsPOSDBConstants.Log_Exiting);
                return bDelete;
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "DeleteUserRow(string UserID)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        public bool UpdateUserRow(string pass, string UserID)
        {
            string sSQL;
            try
            {
                logger.Trace("UpdateUserRow(string pass, string UserID) - " + clsPOSDBConstants.Log_Entering);
                //sSQL = " UPDATE Users SET Password ='" + pass +"', passwordchangedon = '" + System.DateTime.Now + "' where UserID= '" + UserID + "'";
                //Fix by Manoj 4/1/2015 bug with different characters
                //sSQL = "update users set password=@password, passwordchangedon=@passwordchangedon where userid=@userID";
                #region PRIMEPOS-3129 22-Aug-2022 JY Added ChangePasswordAtLogin
                string strSQL = string.Empty;
                if (Configuration.convertNullToBoolean(Configuration.CSetting.ResetPwdForceUserToChangePwd) == true)
                    sSQL = "update users set ChangePasswordAtLogin = 1, password=@password, passwordchangedon=@passwordchangedon where userid=@userID"; //PRIMEPOS-3129 22-Aug-2022 JY Added ChangePasswordAtLogin
                else
                    sSQL = "update users set password=@password, passwordchangedon=@passwordchangedon where userid=@userID";
                #endregion

                System.Data.IDbDataParameter[] oparm = DataFactory.CreateParameterArray(3);
                oparm[0] = DataFactory.CreateParameter();
                oparm[0].ParameterName = "@password";
                oparm[0].Value = pass;
                oparm[0].DbType = System.Data.DbType.String;

                oparm[1] = DataFactory.CreateParameter();
                oparm[1].ParameterName = "@userID";
                oparm[1].Value = UserID;
                oparm[1].DbType = System.Data.DbType.String;

                oparm[2] = DataFactory.CreateParameter();
                oparm[2].ParameterName = "@passwordchangedon";
                oparm[2].Value = System.DateTime.Now;
                oparm[2].DbType = System.Data.DbType.DateTime;

                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, oparm);
                logger.Trace("UpdateUserRow(string pass, string UserID) - " + clsPOSDBConstants.Log_Exiting);
                return true;
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "UpdateUserRow(string pass, string UserID)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        public AutoCompleteStringCollection GetAutoCompleteCollectionData(string tblName, string dbFieldName)
        {
            string sSQL= "";
            try
            {
                logger.Trace("GetAutoCompleteCollectionData(string tblName, string dbFieldName) - " + clsPOSDBConstants.Log_Entering);
                AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
                IDataReader dReader;
                if (tblName == clsPOSDBConstants.Customer_tbl && dbFieldName.Contains("'") == true)
                {
                    sSQL = "SELECT " + dbFieldName + " AS " + clsPOSDBConstants.Customer_Fld_CustomerName + " FROM " + tblName;
                }
                else
                {   
                    sSQL = "SELECT " + dbFieldName  + " FROM " + tblName;
                }

                dReader = DataHelper.ExecuteReader(DBConfig.ConnectionString, CommandType.Text, sSQL);
                if (tblName == clsPOSDBConstants.Customer_tbl && dbFieldName.Contains("'") == true)
                {
                    dbFieldName = clsPOSDBConstants.Customer_Fld_CustomerName;
                }
                if (dReader.FieldCount == 1)
                {
                    while (dReader.Read())
                    {
                        namesCollection.Add(dReader[dbFieldName].ToString());
                    }
                }
                else
                {
                    while (dReader.Read())
                    {
                        for (int index = 0; index < dReader.FieldCount; index++)
                        {
                            namesCollection.Add(dReader[index].ToString());
                        }
                    }
                }
                dReader.Close();
                logger.Trace("GetAutoCompleteCollectionData(string tblName, string dbFieldName) - " + clsPOSDBConstants.Log_Exiting);
                return namesCollection;
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "GetAutoCompleteCollectionData(string tblName, string dbFieldName)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetCustomerSearchResult(DateTime DOB1, DateTime DOB2, DateTime ExpDate1, DateTime ExpDate2, string strCode, string strName, string strMasterSearchVal, Boolean bIncludeRXCust, out DataSet dsRxPatient, string strContactNumber = "", Boolean includeCPLCardInfo = false, int ActiveOnly = 0, Boolean bOnlyCLPCardCustomers = false, Boolean bSearchExactCustomer = false, Boolean bSelection = false, string ExpDateOption = "", string DOBOption = "", bool IsNoStoreCard = false)//PRIMEPOS-2896 Added ISNOSTORECARD
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                logger.Trace("GetCustomerSearchResult() - " + clsPOSDBConstants.Log_Entering);
                string sSQL = string.Empty;
                if (bSelection == false)
                    sSQL = GetCustomerSearchQuery(DOB1, DOB2, strCode, strName, strMasterSearchVal, DOBOption, IsNoStoreCard);//PRIMEPOS-2896
                else
                    sSQL = GetCustomerSearchQuery(DOB1, DOB2, strCode, strName, strMasterSearchVal, strContactNumber, includeCPLCardInfo, ActiveOnly, bOnlyCLPCardCustomers, bSearchExactCustomer, DOBOption);

                #region PRIMEPOS-2613 07-Dec-2018 JY Added
                if (string.IsNullOrEmpty(strMasterSearchVal) == true && ExpDateOption.Trim() != "" && ExpDateOption.Trim().ToUpper() != "ALL")
                {
                    string tempSql = "SELECT DISTINCT C.* FROM (" + sSQL + ") C " +
                            " INNER JOIN CCCustomerTokInfo TOK ON C.CustomerID = TOK.CustomerID";
                    string tempWhereClause = string.Empty;
                    if (ExpDateOption == "NULL")
                    {
                        //tempWhereClause = " WHERE ISNULL(CONVERT(Date, '01/' + SUBSTRING(TOK.ExpDate, 1, 2) + '/' + SUBSTRING(TOK.ExpDate, 3, 2), 3), '') = ''";
                        tempWhereClause = " WHERE TOK.ExpDate IS NULL";
                    }
                    else if (ExpDateOption == "NOT NULL")
                    {
                        //tempWhereClause = " WHERE ISNULL(CONVERT(Date, '01/' + SUBSTRING(TOK.ExpDate, 1, 2) + '/' + SUBSTRING(TOK.ExpDate, 3, 2), 3), '') <> ''";
                        tempWhereClause = " WHERE TOK.ExpDate IS NOT NULL";
                    }
                    else if (ExpDateOption == "=" || ExpDateOption == ">" || ExpDateOption == "<")
                    {
                        //tempWhereClause = " WHERE CONVERT(Date, '01/' + SUBSTRING(TOK.ExpDate, 1, 2) + '/' + SUBSTRING(TOK.ExpDate, 3, 2), 3) " + ExpDateOption + " Convert(date, '" + ExpDate1 + "')";
                        tempWhereClause = " WHERE CONVERT(Date,TOK.ExpDate) " + ExpDateOption + " Convert(date, '" + ExpDate1 + "')";
                    }
                    else if (ExpDateOption == "Between")
                    {
                        //tempWhereClause = " WHERE CONVERT(Date, '01/' + SUBSTRING(TOK.ExpDate, 1, 2) + '/' + SUBSTRING(TOK.ExpDate, 3, 2), 3) > " + " Convert(date, '" + ExpDate1 + "') " +
                        //    " AND CONVERT(Date, '01/' + SUBSTRING(TOK.ExpDate, 1, 2) + '/' + SUBSTRING(TOK.ExpDate, 3, 2), 3) < " + " Convert(date, '" + ExpDate2 + "')";
                        tempWhereClause = " WHERE CONVERT(Date,TOK.ExpDate) BETWEEN " + " Convert(date, '" + ExpDate1 + "')" + " AND Convert(date, '" + ExpDate2 + "')";
                    }
                    sSQL = tempSql + tempWhereClause;
                }
                #endregion

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);

                //Added By shitaljit to display new RX customers.
                #region code to display new RX customers.
                if (bIncludeRXCust == true && bSearchExactCustomer == false && (!string.IsNullOrWhiteSpace(strCode) || !string.IsNullOrWhiteSpace(strName) || !string.IsNullOrWhiteSpace(strContactNumber) || !string.IsNullOrWhiteSpace(strMasterSearchVal)))
                {
                    dsRxPatient = GetPatientData(strCode, strName, strMasterSearchVal, strContactNumber);
                }
                else
                {
                    dsRxPatient = null;
                }
                #endregion
                logger.Trace("GetCustomerSearchResult() - " + clsPOSDBConstants.Log_Exiting);
                return ds;
            }
        }

        private string GetCustomerSearchQuery(DateTime DOB1, DateTime DOB2, string strCode, string strName, string strMasterSearchVal, string DOBOption = "", bool IsNoStoreCard = false)//PRIMEPOS-2896
        {
            string sSQL = string.Empty;
            string sAcctNoClause = string.Empty;
            string strNewRxCust = @"'PrimePOS'" + " as [Cust. Source]";
            string sSQL1 = string.Empty;
            string strLookForByCommaSeparatedLastNameFirstName = string.Empty;    //PRIMEPOS-2893 30-Sep-2020 JY Added

            sSQL = " Select "
                + clsPOSDBConstants.Customer_Fld_AcctNumber + " as Account#," +
                clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name," +
                clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                clsPOSDBConstants.Customer_Fld_City + " as City," +
                clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                clsPOSDBConstants.Customer_Fld_Email + " as Email ," +
                clsPOSDBConstants.Customer_tbl + "." + clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +//added by shitaljit Quicsolv on 20 Feb 2012
                clsPOSDBConstants.Customer_Fld_Zip + " as Zip ," +
                clsPOSDBConstants.Customer_Fld_FaxNo + " as Fax# , " +
                clsPOSDBConstants.Customer_Fld_DriveLicNo + " as DL# , " +
                clsPOSDBConstants.Customer_Fld_DriveLicState + " as [DL State], " +
                clsPOSDBConstants.Customer_Fld_DateOfBirth + " as DOB , " +
                clsPOSDBConstants.Customer_Fld_PatientNo + " as Patient# ," +
                clsPOSDBConstants.Customer_Fld_Comments + ", " +
                clsPOSDBConstants.Customer_Fld_CustomerId + " as CustomerId ," +
                clsPOSDBConstants.Customer_Fld_CustomerCode + "," + //PRIMEPOS-2987 12-Aug-2021 JY Added
                strNewRxCust + //added by shitaljit Quicsolv on 26 May 2012
                 " From Customer Where 1=1 ";

            #region Master Search Code Added By shitaljit for JIRA-375 on 29Apr13
            if (string.IsNullOrEmpty(strMasterSearchVal) == false)
            {
                //Added By shitaljit to give preference to Acct# in search Query.
                if (clsCoreUIHelper.isNumeric(strMasterSearchVal) == true)
                {
                    sSQL1 += sSQL + " AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'" + strMasterSearchVal + "%') ";   //PRIMEPOS-2613 07-Dec-2018 JY changed "Union all" to "Union" to avoid duplicates 
                    sAcctNoClause = " AND (";
                }
                else
                {
                    //sAcctNoClause = "AND (" + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + strMasterSearchVal + "%')OR ";//PRIMEPOS-2896
                    #region  PRIMEPOS-2893 30-Sep-2020 JY Added                 
                    if (strMasterSearchVal.Trim().Length > 1 && strMasterSearchVal.Contains(","))
                    {
                        string[] arrName = strMasterSearchVal.Split(',');
                        if (arrName.Length > 1)
                        {
                            strLookForByCommaSeparatedLastNameFirstName = " AND CustomerName LIKE '" + arrName[0].Trim().Replace("'", "''") + "%' AND FIRSTNAME LIKE '" + arrName[1].Trim().Replace("'", "''") + "%'";
                        }
                    }

                    if (strLookForByCommaSeparatedLastNameFirstName != "")
                    {
                        sSQL1 += sSQL + strLookForByCommaSeparatedLastNameFirstName;
                    }
                    else
                    {
                        sAcctNoClause = "AND (" + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + strMasterSearchVal + "%')OR ";//PRIMEPOS-2896
                    }
                    #endregion
                }

                sSQL += sAcctNoClause
                 + clsPOSDBConstants.Customer_Fld_CustomerName + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_FirstName + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Address1 + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Address2 + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Zip + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_City + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_CellNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_PhoneOffice + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Comments + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_FaxNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_DriveLicNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                + " CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_DriveLicState + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_PatientNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_PhoneHome + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Email + " LIKE (" + "'%" + strMasterSearchVal + "%')  )";//PRIMEPOS-2896
            }
            #endregion
            else
            {
                if (strCode.Trim() != "")
                {
                    Int32 iID = Configuration.convertNullToInt(strCode);
                    sSQL += " And AcctNumber LIKE (" + "'" + iID.ToString() + "%')";
                }

                if (strName.Trim() != "")
                {//from here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                    string lName = "";
                    string fName = "";
                    if (strName.Trim().Contains(","))
                    {
                        char[] seperater = { ',' };
                        string[] lFname = strName.Split(seperater);
                        lName = lFname[0].Trim();
                        if (lFname.Length > 1)
                            fName = lFname[1].Trim();
                    }
                    else
                    {
                        lName = strName.Trim();
                    }
                    //Till here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                    // commented by Ravindra(Quicsolv) to Search Customer by   last and first name
                    //sSQL += " And CustomerName+','+FirstName like '" + strName.Replace("'", "''").Replace(",", "%,") + "%'";
                    sSQL += " And CustomerName like '" + lName + "%' and +FirstName like '" + fName + "%'";

                    //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE               
                    //if (ActiveOnly == 1)
                    //{
                    // sSQL += " And isActive=1 ";
                    //}
                }

                sSQL += DOBFilter(DOB1, DOB2, DOBOption);  //PRIMEPOS-2645 06-Mar-2019 JY Added
            }

            if (IsNoStoreCard)//PRIMEPOS-2896
            {
                sSQL += " And CustomerID not in (select distinct CustomerID from CCCustomerTokInfo WHERE ISNULL(IsActive,1) = 1)";
                if (sSQL1 != "") sSQL1 += " And CustomerID not in (select distinct CustomerID from CCCustomerTokInfo WHERE ISNULL(IsActive,1) = 1)";
            }

            if (string.IsNullOrEmpty(sSQL1) == false)
            {
                //return (sSQL1 + " UNION " + sSQL);    //PRIMEPOS-2893 30-Sep-2020 JY Commented
                #region PRIMEPOS-2893 30-Sep-2020 JY Added
                if (strLookForByCommaSeparatedLastNameFirstName != "")
                    sSQL = sSQL1;
                else
                    sSQL = sSQL1 + " UNION " + sSQL;
                #endregion
            }
            return sSQL;
        }

        private DataSet GetPatientData(string strCode, string strName, string strMasterSearchVal, string strContactNumber = "")
        {
            logger.Trace("GetPatientData() - " + clsPOSDBConstants.Log_Entering);
            DataSet dsRxPatient = null;

            string sSQLPat = string.Empty;
            string strLookForByCommaSeparatedLastNameFirstName = string.Empty;    //PRIMEPOS-2893 30-Sep-2020 JY Added
            sSQLPat = "SELECT * FROM Patient ";

            string strSelectClause = " WHERE 1=1 ";
            if (string.IsNullOrWhiteSpace(strMasterSearchVal) == false)
            {
                #region  PRIMEPOS-2893 30-Sep-2020 JY Added                 
                if (strMasterSearchVal.Trim().Length > 1 && strMasterSearchVal.Contains(","))
                {
                    string[] arrName = strMasterSearchVal.Split(',');
                    if (arrName.Length > 1)
                    {
                        strLookForByCommaSeparatedLastNameFirstName = " WHERE LNAME LIKE '" + arrName[0].Trim().Replace("'", "''") + "%' AND FNAME LIKE '" + arrName[1].Trim().Replace("'", "''") + "%'";
                    }
                }
                #endregion

                if (strLookForByCommaSeparatedLastNameFirstName != "")
                {
                    strSelectClause = strLookForByCommaSeparatedLastNameFirstName;
                }
                else
                {
                    strSelectClause += "AND LNAME LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "FNAME LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "ADDRSTR LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "ADDRCT LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "ADDRST LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "ADDRZP LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "PHONE LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "EMAIL LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "WORKNO LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                            + "MOBILENO LIKE (" + "'%" + strMasterSearchVal + "%')   ";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(strName) == false)
                {
                    //from here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                    string lName = "";
                    string fName = "";
                    if (strName.Trim().Contains(","))
                    {
                        char[] seperater = { ',' };
                        string[] lFname = strName.Split(seperater);
                        lName = lFname[0].Trim();
                        if (lFname.Length > 1)
                            fName = lFname[1].Trim();
                    }
                    else
                    {
                        lName = strName.Trim();
                    }
                    strSelectClause += "AND   LNAME like '" + lName + "%'AND FNAME like '" + fName + "%'";
                }
                else if (strContactNumber.Trim() != "")
                {
                    strSelectClause += " AND  MOBILENO LIKE ('" + strContactNumber + "%')";
                    strSelectClause += " OR  WORKNO LIKE ('" + strContactNumber + "%')";
                    strSelectClause += " OR  PHONE LIKE ('" + strContactNumber + "%')";
                }
            }
            sSQLPat = sSQLPat + strSelectClause;
            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
            oAcct.GetRecs(sSQLPat, out dsRxPatient);
            logger.Trace("GetPatientData() - " + clsPOSDBConstants.Log_Exiting);
            return dsRxPatient;
        }

        private string GetCustomerSearchQuery(DateTime DOB1, DateTime DOB2, string strCode, string strName, string strMasterSearchVal, string strContactNumber, Boolean includeCPLCardInfo = false, int ActiveOnly = 0, Boolean bOnlyCLPCardCustomers = false, Boolean bSearchExactCustomer = false, string DOBOption = "", bool IsNoStoreCard = false)//PRIMEPOS-2896
        {
            //MOdified the logic by shitaljit to do exact match first OR give preference to account# in the like query
            string sSQL = string.Empty;
            string sAcctNoClause = string.Empty;
            string strNewRxCust = @"'PrimePOS'" + " as [Cust. Source]";
            string sSQL1 = string.Empty;
            string strLookForByCommaSeparatedLastNameFirstName = string.Empty;    //PRIMEPOS-2893 30-Sep-2020 JY Added

            if (includeCPLCardInfo == true)
            {
                sSQL = " Select "
                + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerId + ", "
                + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerCode + ", "//added by shitaljit Quicsolv on 2 Nov 2011
                + clsPOSDBConstants.Customer_Fld_AcctNumber + " as Account#," +
                clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name," +
                "clCard." + clsPOSDBConstants.CLCards_Fld_CLCardID + " as [CLP Card ID], " +
                "clCard." + clsPOSDBConstants.CLCards_Fld_RegisterDate + " as [CLP Registered Date]," +
                clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                clsPOSDBConstants.Customer_Fld_City + " as City," +
                clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                clsPOSDBConstants.Customer_Fld_Email + " as Email , " +
                clsPOSDBConstants.Customer_tbl + "." + clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +//added by shitaljit Quicsolv on 20 Feb 2012
                strNewRxCust + " , " +//added by shitaljit Quicsolv on 26 May 2012
                clsPOSDBConstants.Customer_Fld_Zip + " as Zip ," +
                clsPOSDBConstants.Customer_Fld_FaxNo + " as Fax# , " +
                clsPOSDBConstants.Customer_Fld_DriveLicNo + " as DL# , " +
                clsPOSDBConstants.Customer_Fld_DriveLicState + " as [DL State], " +
                clsPOSDBConstants.Customer_Fld_DateOfBirth + " as DOB , " +
                clsPOSDBConstants.Customer_Fld_PatientNo + " as Patient# ," +
                clsPOSDBConstants.Customer_Fld_Comments +
                " From Customer Left Join " + clsPOSDBConstants.CLCards_tbl + " as clCard On Customer.CustomerID=clCard.CustomerID " +
                " where 1=1 AND  clCard.isActive=1 ";

                #region Master Search Code Added By shitaljit for JIRA-375 on 29Apr13
                if (string.IsNullOrEmpty(strMasterSearchVal) == false)
                {
                    //Added By shitaljit to give preference to Acct# in search Query.
                    if (clsCoreUIHelper.isNumeric(strMasterSearchVal) == true)
                    {
                        sSQL1 += sSQL + " AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'" + strMasterSearchVal + "%') ";

                        sAcctNoClause = " AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " NOT LIKE (" + "'" + strMasterSearchVal + "%') AND (";
                        if (ActiveOnly == 1)
                        {
                            sSQL1 += " And Customer.isActive=1 ";
                        }

                        if (bOnlyCLPCardCustomers)
                        {
                            sSQL1 += " And clCard." + clsPOSDBConstants.CLCards_Fld_CLCardID + " Is Not Null ";
                        }
                        //sSQL1 += " UNION "; //Sprint-25 - PRIMEPOS-2411 27-Apr-2017 JY changed "Union all" to "Union" to avoid duplicates 
                    }
                    else
                    {
                        //sAcctNoClause = "AND (" + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + strMasterSearchVal + "%')OR   ";
                        #region  PRIMEPOS-2893 30-Sep-2020 JY Added                 
                        if (strMasterSearchVal.Trim().Length > 1 && strMasterSearchVal.Contains(","))
                        {
                            string[] arrName = strMasterSearchVal.Split(',');
                            if (arrName.Length > 1)
                            {
                                strLookForByCommaSeparatedLastNameFirstName = " AND CustomerName LIKE '" + arrName[0].Trim().Replace("'", "''") + "%' AND FIRSTNAME LIKE '" + arrName[1].Trim().Replace("'", "''") + "%'";
                            }
                        }

                        if (strLookForByCommaSeparatedLastNameFirstName != "")
                        {
                            sSQL1 += sSQL + strLookForByCommaSeparatedLastNameFirstName;
                        }
                        else
                        {
                            sAcctNoClause = "AND (" + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + strMasterSearchVal + "%') OR ";//PRIMEPOS-2896
                        }
                        #endregion
                    }

                    sSQL += sAcctNoClause
                     + clsPOSDBConstants.Customer_Fld_CustomerName + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FirstName + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address1 + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address2 + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                      + clsPOSDBConstants.Customer_Fld_Zip + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_City + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_CellNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneOffice + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Comments + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FaxNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + " CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicState + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PatientNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneHome + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Email + " LIKE (" + "'%" + strMasterSearchVal + "%') ";
                }
                #endregion
                else
                {
                    if (strCode.Trim() != "")
                    {
                        Int32 iID = Configuration.convertNullToInt(strCode);
                        if (bSearchExactCustomer == true)
                        {
                            sSQL += " And Customer.AcctNumber='" + iID.ToString() + "' ";
                        }
                        else
                        {
                            sSQL += " And AcctNumber LIKE (" + "'" + iID.ToString() + "%')"; //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                        }
                    }

                    if (strName.Trim() != "")
                    {
                        sSQL += " And CustomerName+','+FirstName like '" + strName.Replace("'", "''").Replace(",", "%,") + "%'";
                    }
                    //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                    if (strContactNumber.Trim() != "")
                    {
                        sSQL += " And ( CellNo LIKE ('" + strContactNumber + "%')";
                        sSQL += " Or  PhoneOff LIKE ('" + strContactNumber + "%')";
                        sSQL += " Or  PhoneHome LIKE ('" + strContactNumber + "%'))";
                    }
                }
                if (string.IsNullOrEmpty(strMasterSearchVal) == false && (ActiveOnly == 1 || bOnlyCLPCardCustomers == true))
                {
                    sSQL += ")";
                }
                if (ActiveOnly == 1)
                {
                    sSQL += " And Customer.isActive=1 ";
                }
                if (bOnlyCLPCardCustomers)
                {
                    sSQL += " And clCard." + clsPOSDBConstants.CLCards_Fld_CLCardID + " Is Not Null ";
                }

                sSQL += DOBFilter(DOB1, DOB2, DOBOption);  //PRIMEPOS-2645 06-Mar-2019 JY Added

                if (string.IsNullOrEmpty(strMasterSearchVal) == false && clsCoreUIHelper.isNumeric(strMasterSearchVal) == true && ActiveOnly == 0 && bOnlyCLPCardCustomers == false)
                {
                    sSQL += ")";
                }
            }
            else
            {
                sSQL = " Select "
                    + clsPOSDBConstants.Customer_Fld_CustomerId + ", "
                    + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerCode + ", "//added by shitaljit Quicsolv on 2 Nov 2011
                    + clsPOSDBConstants.Customer_Fld_AcctNumber + " as Account#," +
                    clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name," +
                    clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                    clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                    clsPOSDBConstants.Customer_Fld_City + " as City," +
                    clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                    clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                    clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                    clsPOSDBConstants.Customer_Fld_Email + " as Email ," +
                    clsPOSDBConstants.Customer_tbl + "." + clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +//added by shitaljit Quicsolv on 20 Feb 2012
                    strNewRxCust + " , " +//added by shitaljit Quicsolv on 26 May 2012
                    clsPOSDBConstants.Customer_Fld_Zip + " as Zip ," +
                    clsPOSDBConstants.Customer_Fld_FaxNo + " as Fax# , " +
                    clsPOSDBConstants.Customer_Fld_DriveLicNo + " as DL# , " +
                    clsPOSDBConstants.Customer_Fld_DriveLicState + " as [DL State], " +
                    clsPOSDBConstants.Customer_Fld_DateOfBirth + " as DOB , " +
                    clsPOSDBConstants.Customer_Fld_PatientNo + " as Patient# ," +
                    clsPOSDBConstants.Customer_Fld_Comments +
                    " From Customer  Where 1=1";

                Int32 iID = Configuration.convertNullToInt(strCode);
                #region Master Search Code Added By shitaljit for JIRA-375 on 29Apr13
                if (string.IsNullOrEmpty(strCode.Trim()) == false && bSearchExactCustomer == true)
                {
                    sSQL += " And AcctNumber = '" + iID.ToString() + "'";
                    return sSQL;
                }

                if (string.IsNullOrEmpty(strMasterSearchVal) == false)
                {
                    //Added By shitaljit to give preference to Acct# in search Query.
                    if (clsCoreUIHelper.isNumeric(strMasterSearchVal) == true)
                    {
                        sSQL1 += sSQL + " AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'" + strMasterSearchVal + "%') ";    //Sprint-25 - PRIMEPOS-2411 27-Apr-2017 JY changed "Union all" to "Union" to avoid duplicates 
                        sAcctNoClause = " AND (";
                    }
                    else
                    {
                        //sAcctNoClause = "AND (" + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + strMasterSearchVal + "%') OR ";
                        #region  PRIMEPOS-2893 30-Sep-2020 JY Added                 
                        if (strMasterSearchVal.Trim().Length > 1 && strMasterSearchVal.Contains(","))
                        {
                            string[] arrName = strMasterSearchVal.Split(',');
                            if (arrName.Length > 1)
                            {
                                strLookForByCommaSeparatedLastNameFirstName = " AND CustomerName LIKE '" + arrName[0].Trim().Replace("'", "''") + "%' AND FIRSTNAME LIKE '" + arrName[1].Trim().Replace("'", "''") + "%'";
                            }
                        }

                        if (strLookForByCommaSeparatedLastNameFirstName != "")
                        {
                            sSQL1 += sSQL + strLookForByCommaSeparatedLastNameFirstName;
                        }
                        else
                        {
                            sAcctNoClause = "AND (" + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'" + strMasterSearchVal + "%') OR ";
                        }
                        #endregion
                    }

                    sSQL += sAcctNoClause
                     + clsPOSDBConstants.Customer_Fld_CustomerName + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FirstName + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address1 + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address2 + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Zip + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_City + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_CellNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneOffice + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Comments + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FaxNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                    + " CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicState + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PatientNo + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneHome + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Email + " LIKE (" + "'%" + strMasterSearchVal + "%') )";  //PRIMEPOS-2896
                }
                #endregion
                else
                {
                    if (string.IsNullOrEmpty(strCode.Trim()) == false && bSearchExactCustomer == false)
                    {
                        sSQL += " And AcctNumber LIKE (" + "'" + iID.ToString() + "%')";  //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                    }

                    if (string.IsNullOrEmpty(strName.Trim()) == false)
                    {//from here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                        string lName = "";
                        string fName = "";
                        if (strName.Trim().Contains(","))
                        {
                            char[] seperater = { ',' };
                            string[] lFname = strName.Split(seperater);
                            lName = lFname[0].Trim();
                            if (lFname.Length > 1)
                                fName = lFname[1].Trim();
                        }
                        else
                        {
                            lName = strName.Trim();
                        }
                        //Till here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                        // commented by Ravindra(Quicsolv) to Search Customer by   last and first name
                        //sSQL += " And CustomerName+','+FirstName like '" + strName.Replace("'", "''").Replace(",", "%,") + "%'";
                        sSQL += " And CustomerName like '" + lName + "%' and +FirstName like '" + fName + "%'";
                    }
                    //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                    if (strContactNumber.Trim() != "")
                    {
                        sSQL += " And ( CellNo LIKE ('" + strContactNumber + "%')";
                        sSQL += " Or  PhoneOff LIKE ('" + strContactNumber + "%')";
                        sSQL += " Or  PhoneHome LIKE ('" + strContactNumber + "%'))";
                    }
                    if (ActiveOnly == 1)
                    {
                        sSQL += " And Customer.isActive=1 ";
                    }

                    sSQL += DOBFilter(DOB1, DOB2, DOBOption);  //PRIMEPOS-2645 06-Mar-2019 JY Added
                }
            }

            if (IsNoStoreCard)//PRIMEPOS-2896
            {
                sSQL += " And Customer.CustomerID not in (select distinct CustomerID from CCCustomerTokInfo WHERE ISNULL(IsActive,1) = 1)";
                if (sSQL1 != "") sSQL1 += " And Customer.CustomerID not in (select distinct CustomerID from CCCustomerTokInfo WHERE ISNULL(IsActive,1) = 1)";
            }

            if (string.IsNullOrEmpty(sSQL1) == false)
            {
                //return (sSQL1 + " UNION " + sSQL);    //PRIMEPOS-2893 30-Sep-2020 JY Commented
                #region PRIMEPOS-2893 30-Sep-2020 JY Added
                if (strLookForByCommaSeparatedLastNameFirstName != "")
                    sSQL = sSQL1;
                else
                    sSQL = sSQL1 + " UNION " + sSQL;
                #endregion
            }
            return sSQL;
        }

        public DataSet GetHouseChargeSearchData(string strCode, string strName, string strMasterSearchVal)
        {
            logger.Trace("GetHouseChargeSearchData() - " + clsPOSDBConstants.Log_Entering);
            DataSet ds = null;
            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
            string sSQL = GetHouseChargeSearchQuery(strCode, strName, strMasterSearchVal);
            oAcct.GetRecs(sSQL, out ds);
            logger.Trace("GetHouseChargeSearchData() - " + clsPOSDBConstants.Log_Exiting);
            return ds;
        }

        private string GetHouseChargeSearchQuery(string strCode, string strName, string strMasterSearchVal)
        {
            string sSQL = string.Empty;
            sSQL = @"SELECT ACCT_NO,ACCT_NAME,ADDRESS1,ADDRESS2,CITY ,STATE,ZIP,PHONE_NO,STATUS,APPFINCHRG,RECURRINGB,CCTYPE,CCNUMBER,CCEXPDATE,CREDIT_LMT
                ,BALANCE,DISCOUNT,MTD_CHARGE,YTD_CHARGE,LY_CHARGE,MTD_PAYM,YTD_PAYM,LY_PAYM,LAST_SBAL, (CONVERT(VARCHAR(10),LAST_SDATE , 126)) as 'LAST_SDATE',COMMENT,ACCEPTCHK,MOBILENO,NAMEONCC,IE FROM ACCOUNT WHERE 1=1 ";
            string sWhrClause = string.Empty;
            if (string.IsNullOrEmpty(strMasterSearchVal) == false)
            {
                sWhrClause = @" AND ACCT_NO  " + " LIKE (" + "'" + strMasterSearchVal + "%')OR   "
                          + "ACCT_NAME" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "ADDRESS1" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "ADDRESS2" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "CITY" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "STATE" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "ZIP" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "PHONE_NO" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "STATUS" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "APPFINCHRG" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "RECURRINGB" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "CCTYPE" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "CCNUMBER" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "CCEXPDATE" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "CREDIT_LMT" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "BALANCE" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "DISCOUNT" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "MTD_CHARGE" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "YTD_CHARGE" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "LY_CHARGE" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "MTD_PAYM" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "YTD_PAYM" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "LY_PAYM" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "LAST_SBAL" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + " CONVERT(VARCHAR(25), LAST_SDATE , 126)" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "COMMENT" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "ACCEPTCHK" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "MOBILENO" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "NAMEONCC" + " LIKE (" + "'%" + strMasterSearchVal + "%')OR   "
                          + "IE " + " LIKE (" + "'%" + strMasterSearchVal + "%')   ";
            }
            else
            {
                sWhrClause = @" AND ACCT_NO  " + " LIKE (" + "'" + strCode + "%')OR   "
                            + "ACCT_NAME" + " LIKE (" + "'" + strName + "%')  ";
            }
            return sSQL = sSQL + sWhrClause;
        }

        public DataSet SetLastVendor(DataSet oDataSet)
        {
            logger.Trace("SetLastVendor() - " + clsPOSDBConstants.Log_Entering);
            DataSet lastVendorName = new DataSet();

            if (!oDataSet.Tables[0].Columns.Contains("LastVendorName"))
                oDataSet.Tables[0].Columns.Add("LastVendorName");
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                //Updated By SRT(Gaurav) Date: 15-Jul-2009
                //Updated query to avoid error in converting data of wrong type / (here is Int -> String)
                string strSql = "Select Item.ItemID,Item.LastVendor ,Vendor.VendorName as LastVendorName from  Item ,Vendor where CAST(Item.LastVendor as VARCHAR(50))  = CAST(Vendor.VendorCode as VARCHAR(50))";
                lastVendorName = DataHelper.ExecuteDataset(conn, CommandType.Text, strSql);

                foreach (DataRow row in oDataSet.Tables[0].Rows)
                {
                    int count = 0;
                    DataRow[] rowLastVendoName = lastVendorName.Tables[0].Select(" ItemID ='" + row["ItemID"].ToString() + "'");
                    for (count = 0; count < rowLastVendoName.Length; count++)
                    {
                        row["LastVendorName"] = rowLastVendoName[count].ItemArray[2].ToString();
                    }
                }
                logger.Trace("SetLastVendor() - " + clsPOSDBConstants.Log_Exiting);
                return oDataSet;
            }
        }

        public DataSet SetBestPriceBestVendorValues(DataSet oDataSet)
        {
            logger.Trace("SetBestPriceBestVendorValues() - " + clsPOSDBConstants.Log_Entering);
            if (!oDataSet.Tables[0].Columns.Contains("BestVendor"))
                oDataSet.Tables[0].Columns.Add("BestVendor");
            if (!oDataSet.Tables[0].Columns.Contains("BestPrice"))
                oDataSet.Tables[0].Columns.Add("BestPrice");

            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                DataSet dsBestVendPrice = new DataSet();
                foreach (DataRow row in oDataSet.Tables[0].Rows)
                {                        
                    dsBestVendPrice = null;
                    string strSql = "Select VendorCostPrice,Vendor.VendorName from ItemVendor, Vendor where Vendor.VendorID=ItemVendor.VendorID AND ItemVendor.VendorCostPrice= (Select MIN(ItemVendor.VendorCostPrice) from ItemVendor where ItemID = '" + row["ItemID"].ToString() + "')" +
                                    " AND ItemID = '" + row["ItemID"].ToString() + "'";

                    dsBestVendPrice = DataHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                    if (dsBestVendPrice.Tables.Count > 0)
                    {
                        if (dsBestVendPrice.Tables[0].Rows.Count > 0)
                        {
                            row["BestPrice"] = dsBestVendPrice.Tables[0].Rows[0]["VendorCostPrice"];
                            row["BestVendor"] = dsBestVendPrice.Tables[0].Rows[0]["VendorName"];
                        }
                    }                        
                }
                logger.Trace("SetBestPriceBestVendorValues() - " + clsPOSDBConstants.Log_Exiting);
                return oDataSet;
            }
        }

        public DataSet GetFrmSearchData(string SearchTable, string strCode, string strName, string strMasterSearchVal, string PrgFlag, string ParamValue, bool IsFromPurchaseOrder, ref string DefaultCode, int ActiveOnly, int AdditionalParameter)
        {
            logger.Trace("GetFrmSearchData() - " + clsPOSDBConstants.Log_Entering);
            DataSet oDataSet = new DataSet();

            string queryToFetchItem = "Select Item.ItemID,Item.Description,Vendor.VendorName as VendorName,Item.QtyInStock,Item.QtyOnOrder,Item.MinOrdQty,Item.PreferredVendor,Item.SellingPrice , " +
                                            " ItemVendor.VendorItemId,ItemVendor.VendorCostPrice,Item.ReOrderLevel,Vendor.VendorID, ISNULL(ItemVendor.PCKQTY,'') AS PCKQTY, ISNULL(ItemVendor.PCKSIZE,'') AS PCKSIZE, ISNULL(ItemVendor.PCKUNIT,'') AS PCKUNIT " +
                                            " from  Item, ItemVendor, Vendor where Item.ItemID =ItemVendor.ItemID AND " +
                                            " ItemVendor.VendorID = Vendor.VendorID";   //Sprint-25 - PRIMEPOS-1041 03-Apr-2017 JY Added ItemVendor.PCKQTY, ItemVendor.PCKSIZE, ItemVendor.PCKUNIT 

            if (PrgFlag == clsPOSDBConstants.ItemId)
            {
                string StrQuery = string.Empty;
                if (!string.IsNullOrWhiteSpace(strName) || !string.IsNullOrWhiteSpace(strCode))
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + strName + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + strCode + "%'";
                else
                    StrQuery = queryToFetchItem + "  AND " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + ParamValue + "%'";

                oDataSet = FetchItem(StrQuery, oDataSet);
            }
            else if (PrgFlag == clsPOSDBConstants.VendorItemCodeWise)
            {
                string StrQuery = string.Empty;
                if (!string.IsNullOrWhiteSpace(strName) || !string.IsNullOrWhiteSpace(strCode))
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + strName + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + strCode + "%'";
                else
                    StrQuery = queryToFetchItem + "  AND " + clsPOSDBConstants.ItemVendor_tbl + "." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " like '" + ParamValue + "%'";

                oDataSet = FetchItem(StrQuery, oDataSet);
            }
            else if (PrgFlag == clsPOSDBConstants.DescriptionWise)
            {
                string StrQuery = string.Empty;
                if (!string.IsNullOrWhiteSpace(strName) || !string.IsNullOrWhiteSpace(strCode))
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + strName + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + strCode + "%'";
                else
                    StrQuery = queryToFetchItem + "  AND " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + ParamValue + "%'";

                oDataSet = FetchItem(StrQuery, oDataSet);
            }
            else if (IsFromPurchaseOrder == true && PrgFlag == "")
            {
                string StrQuery = string.Empty;
                if (!string.IsNullOrWhiteSpace(strName) || !string.IsNullOrWhiteSpace(strCode))
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + strName + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + strCode + "%'";
                else
                    StrQuery = queryToFetchItem;  //+ "  AND " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + ParamValue + "%'";

                oDataSet = FetchItem(StrQuery, oDataSet);
            }
            else if (IsFromPurchaseOrder == false)
            {
                #region ROA record search Filter.                
                if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface && (string.IsNullOrEmpty(strMasterSearchVal) == false || (string.IsNullOrEmpty(strName) == false && string.IsNullOrEmpty(strCode) == false)))
                {
                    oDataSet = GetHouseChargeSearchData(strCode, strName, strMasterSearchVal);
                }
                #endregion
                else
                {
                    if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface)
                    {
                        oDataSet = clsCoreHouseCharge.getHouseChargeInfo(DefaultCode, strName);
                    }
                    else if (SearchTable == clsPOSDBConstants.PrimeRX_PatientInterface)
                    {
                        oDataSet = clsPatient.SearchPatientInfo(DefaultCode, strName);
                    }
                    else
                    {
                        oDataSet = Search(SearchTable, DefaultCode, strName, ActiveOnly, AdditionalParameter);
                    }
                    //if (SearchTable == clsPOSDBConstants.Item_tbl && oDataSet.Tables[0].Rows.Count == 0 && clsCoreUIHelper.isNumeric(strCode))    //PRIMEPOS-3100 14-Jun-2022 JY Commented
                    if (SearchTable == clsPOSDBConstants.Item_tbl && oDataSet.Tables[0].Rows.Count == 0 && strCode != "")  //PRIMEPOS-3100 14-Jun-2022 JY Added
                    {
                        //string sqlQuery = "SELECT ITEMID AS [Item Code], DESCRIPTION as [Item Description], SELLINGPRICE as [Unit Price], " +
                        //                  "QTYINSTOCK as [Qty In Stock], Unit, EXPTDELIVERYDATE as [Delivery Date], REORDERLEVEL as [Reorder Level], " +
                        //                  "SALEENDDATE as [Sale End Date], SALESTARTDATE as [Sale Start Date], Discount, PRODUCTCODE as [SKU Code], " +
                        //                  "Location, Remarks FROM ITEM WHERE PRODUCTCODE like '" + strCode + "%'";
                        //PRIMEPOS-3107 27-Jun-2022 JY modified
                        string sqlQuery = "SELECT " +
                                            "I." + clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Unit + " as Unit," +
                                            "I." + clsPOSDBConstants.Item_Fld_Location + " as Location," +
                                            "I." + clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Discount + " as Discount," +
                                            "I." + clsPOSDBConstants.Item_Fld_ExptDeliveryDate + " as [Delivery Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SaleEndDate + " as [Sale End Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SaleStartDate + " as [Sale Start Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_ProductCode + " as [SKU Code]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Remarks + " as [Remarks]," +
                                            "ISNULL(I." + clsPOSDBConstants.Item_Fld_IsNonRefundable + ",0)" + " as [Non-Refundable]," +   //PRIMEPOS-2592 06-Nov-2018 JY Added 
                                            "I." + clsPOSDBConstants.Item_Fld_ExpDate + " AS [Exp. Date]," +  //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Added
                                            "I." + clsPOSDBConstants.Item_Fld_PreferredVendor + " AS [Preferred Vendor]," +
                                            "I." + clsPOSDBConstants.Item_Fld_LastVendor + " AS [Last Vendor]" +
                                            " FROM ITEM I WHERE I.PRODUCTCODE like '" + strCode + "%'";
                        oDataSet = Search(sqlQuery);
                    }
                    DefaultCode = string.Empty;
                }
            }
            logger.Trace("GetFrmSearchData() - " + clsPOSDBConstants.Log_Exiting);
            return oDataSet;
        }

        private DataSet FetchItem(string queryTofetchItems, DataSet oDataSet)
        {
            try
            {
                logger.Trace("FetchItem() - " + clsPOSDBConstants.Log_Entering);
                oDataSet = Search(queryTofetchItems);
                oDataSet = SetLastVendor(oDataSet);
                oDataSet = SetBestPriceBestVendorValues(oDataSet);
                logger.Trace("FetchItem() - " + clsPOSDBConstants.Log_Exiting);
                return oDataSet;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FetchItem(string queryTofetchItems, DataSet oDataSet)");
                return null;
                //ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }

        #region PRIMEPOS-2645 06-Mar-2019 JY Added
        private string DOBFilter(DateTime DOB1, DateTime DOB2, string DOBOption = "")
        {
            string strDOBFilter = string.Empty;

            if (DOBOption.Trim() != "" && DOBOption.Trim().ToUpper() != "ALL")
            {
                if (DOBOption == "NULL")
                {
                    strDOBFilter = " AND Customer.DATEOFBIRTH IS NULL ";
                }
                else if (DOBOption == "NOT NULL")
                {
                    strDOBFilter = " AND Customer.DATEOFBIRTH IS NOT NULL ";
                }
                else if (DOBOption == "=" || DOBOption == ">" || DOBOption == "<")
                {
                    strDOBFilter = " AND CONVERT(Date,Customer.DATEOFBIRTH) " + DOBOption + " Convert(date, '" + DOB1 + "') ";
                }
                else if (DOBOption == "Between")
                {
                    strDOBFilter = " AND CONVERT(Date,Customer.DATEOFBIRTH) BETWEEN " + " Convert(date, '" + DOB1 + "')" + " AND Convert(date, '" + DOB2 + "') ";
                }
            }
            return strDOBFilter;
        }
        #endregion        

        public void Dispose() { }
    }
}