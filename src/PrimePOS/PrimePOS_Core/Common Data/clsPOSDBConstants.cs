namespace POS_Core.CommonData
{
    /// <summary>
    /// Summary description for clsPOSDBConstants.
    /// </summary>
    public class clsPOSDBConstants
    {
        public clsPOSDBConstants()
        {
        }

        #region PrimePOSErrorLog Constant
        public const string Log_ApplicationStarted = "Application Started";
        public const string Log_ApplicationLaunched = "Application Launched";
        public const string Log_ApplicationClosed = "Application Close";
        public const string Log_Exception_Occured = "Error Occured";
        public const string Log_Entering = "Entered";
        public const string Log_Success = "Success";
        public const string Log_Exiting = "Exited";
        public const string Log_CommunicatingRX = "Querying to PharmSQL DB";
        public const string Log_Module_Transaction = "POS Transaction ";
        public const string Log_Module_Login = "User Login";
        public const string Log_Module_PriceUpdate = "Price Update";
        public const string Log_Module_POStatusUpdate = "Purchase Order Status Update";
        public const string Log_Module_Transaction_Paymnet = "POS Transaction Payment ";
        public const string Log_Module_Sigpad = "Sig Pad ";
        public const string Log_Module_Payout = "POS Payout ";
        public const string Log_Intitialize_Object = "Intitialize Object";
        public const string Log_Dispose_Object = "Dispose Object";
        #endregion
        public static System.Drawing.Color ClickButtonBackColor = System.Drawing.Color.Black;//Added by shitaljit to identify the click button by changing back color.
        //Device Type Constants -Added by Prashant 21-sep-2010
        public const string VERIFONE = "VERIFONE";
        public const string HHP = "HHP";
        public const string UserBarcodeSeperatorString = "MMS";
        //End of Added by Prashant 21-sep-2010

        //Global constants - Added by Prashant 20-sep-2010
        public const string BINARYIMAGE = "M";
        public const string STRINGIMAGE = "D";
        //End of Added by Prashant 20-sep-2010
        public const string UserUsePayOutCatagory = "UserUsePayOutCatagory";
        public const string fld_ModifiedOn = "ModifiedOn";
        public const string fld_UserID = "UserID";
        public const string fld_StationID = "StationID";
        public const string fld_CreatedOn = "CreatedOn";
        public const string fld_CreatedBy = "CreatedBy";
        public const string fld_LastUpdatedOn = "LastUpdatedOn";
        public const string fld_LastUpdatedBy = "LastUpdatedBy";
        // User file Constants
        public const string Users_Fld_LastLoginIP = "LastLoginIP";

        // Vendor table Constants
        public const string Vendor_tbl = "Vendor";

        //Changed By SRT(Prashant)
        public const string Vendor_Wise_Item_View = "ViewItemListByvendor";
        public const string VendorWise = "VendorWise";
        public const string ItemId = "ItemID";
        public const string VendorItemId = "VendorItemID";
        public const string Description = "Description";
        public const string DescriptionWise = "DescriptionWise";
        public const string VendorItemCodeWise = "VendorItemCodeWise";

        //End Of Added By SRT(Gaurav)

        public const string Vendor_Fld_VendorId = "VendorId";
        public const string Vendor_Fld_VendorCode = "VendorCode";
        public const string Vendor_Fld_VendorName = "VendorName";
        public const string Vendor_Fld_Address1 = "Address1";
        public const string Vendor_Fld_Address2 = "Address2";
        public const string Vendor_Fld_City = "City";
        public const string Vendor_Fld_State = "State";
        public const string Vendor_Fld_Zip = "Zip";
        public const string Vendor_Fld_TelephoneNo = "PhoneOff";
        public const string Vendor_Fld_FaxNo = "FaxNo";
        public const string Vendor_Fld_CellNo = "CellNo";
        public const string Vendor_Fld_URL = "WebAddress";
        public const string Vendor_Fld_Email = "EmailAddress";
        public const string Vendor_Fld_IsActive = "IsActive";
        public const string Vendor_Fld_Process810 = "Process810";
        public const string Vendor_Fld_IsAutoClose = "IsAutoClose";
        public const string Vendor_Fld_UseFTP = "UseFTP";
        public const string Vendor_Fld_FTPURL = "FTPURL";
        public const string Vendor_Fld_FTPLogin = "FTPLogin";
        public const string Vendor_Fld_FTPPassword = "FTPPassword";
        public const string Vendor_Fld_FTPPort = "FTPPort";
        public const string Vendor_Fld_ISA_ID_Qualifier_Sender = "ISA_ID_Qualifier_Sender";
        public const string Vendor_Fld_ISA_ID_Qualifier_Receiver = "ISA_ID_Qualifier_Receiver";
        public const string Vendor_Fld_ISA_Interchange_SenderId = "ISA_Interchange_SenderId";
        public const string Vendor_Fld_ISA_Interchange_ReceiverID = "ISA_Interchange_ReceiverID";
        public const string Vendor_Fld_IEA_Interchange_Control_No = "IEA_Interchange_Control_No";
        public const string Vendor_Fld_ISA_Test_Indicator = "ISA_Test_Indicator";
        public const string Vendor_Fld_App_Sender_Code = "App_Sender_Code";
        public const string Vendor_Fld_App_Receiver_Code = "App_Receiver_Code";
        public const string Vendor_Fld_PO_Type = "PO_Type";
        public const string Vendor_Fld_ID_Code_Qualifier_By = "ID_Code_Qualifier_By";
        public const string Vendor_Fld_Identification_Code_By = "Identification_Code_By";
        public const string Vendor_Fld_ID_Code_Qualifier_SE = "ID_Code_Qualifier_SE";
        public const string Vendor_Fld_Identification_Code_SE = "Identification_Code_SE";
        public const string Vendor_Fld_Product_Qualifier = "Product_Qualifier";
        public const string Vendor_Fld_ISA_Interchange_Control_No = "ISA_Interchange_Control_No";
        public const string Vendor_Fld_ISA_Acknowledgement_Request = "ISA_Acknowledgement_Request";
        public const string Vendor_Fld_Version = "Version";

        public const string Vendor_Fld_StandardType = "StandardType";
        public const string Vendor_Fld_PER_Name = "PER_Name";
        public const string Vendor_Fld_Per_ContactFunctioncode = "Per_ContactFunctioncode";
        public const string Vendor_Fld_Per_Communication_Number_Qualifier = "Per_Communication_Number_Qualifier";
        public const string Vendor_Fld_Per_Communication_Number = "Per_Communication_Number";
        public const string Vendor_Fld_AMT_Amount_Qualifier = "AMT_Amount_Qualifier";
        public const string Vendor_Fld_AMT_Amount = "AMT_Amount";
        public const string Vendor_Fld_AckFileFormat = "AckFileFormat";
        public const string Vendor_Fld_PriceFileFormat = "PriceFileFormat";
        public const string Vendor_Fld_ElementSep = "ElementSep";
        public const string Vendor_Fld_ElementSubSep = "ElementSubSep";
        public const string Vendor_Fld_SegmentSep = "SegmentSep";
        public const string Vendor_Fld_InboundDir = "InboundDir";
        public const string Vendor_Fld_OutboundDir = "OutboundDir";

        public const string Vendor_Fld_PriceItemQualifier = "PriceItemQualifier";
        public const string Vendor_Fld_PriceQualifier = "PriceQualifier";
        public const string Vendor_Fld_USEVICForEPO = "USEVICForEPO";

        public const string Vendor_Fld_ISA_Interchng_Ctrl_Ver_No = "ISA_Interchng_Ctrl_Ver_No";
        public const string Vendor_Fld_EncryptionType = "EncryptionType";

        //Added By  SRT (Abhishek) Date : 01/07/2009
        public const string Vendor_Fld_PrimePOVendorCode = "PrimePOVendorCode";
        public const string Vendor_Fld_PrimePOVendorID = "PrimePOVendorID";
        public const string Vendor_Fld_CostQualifier = "CostQualifier";
        public const string Vendor_Fld_UpdatePrice = "UpdatePrice";
        public const string Vendor_Fld_SendVendorCostPrice = "SendVendorCostPrice";
        //End Of Added By  SRT (Abhishek) Date : 01/07/2009
        public const string Vendor_Fld_SalePriceQualifier = "SalePriceQualifier";
        //AckPriceUpdate Add by Ravindra(QuicSolv) on 20 feb 2013
        public const string Vendor_Fld_AckPriceUpdate = "AckPriceUpdate";
        //End Of Added By   Ravindra(QuicSolv)  on 20 feb 2013
        //Added By  SRT (Prashant) Date : 01/06/2009
        public const string Vendor_Fld_TimeToOrder = "TimeToOrder";
        //End of Added By  SRT (Prashant) Date : 01/06/2009
        public const string Vendor_Fld_SalePriceUpdate = "IsSalePriceUpdate";   //12-Nov-2014 JY added new field IsSalePriceUpdate
        public const string Vendor_Fld_ReduceSellingPrice = "ReduceSellingPrice";   //Sprint-21 - 2208 24-Jul-2015 JY Added

        // Customer file Constants
        public const string Customer_tbl = "Customer";
        public const string Customer_Fld_CustomerId = "CustomerId";
        public const string Customer_Fld_AcctNumber = "AcctNumber";
        public const string Customer_Fld_PrimaryContact = "PrimaryContact";
        public const string Customer_Fld_HouseChargeAcctID = "HouseChargeAcctID";

        public const string Customer_Fld_CustomerName = "CustomerName";
        public const string Customer_Fld_Address1 = "Address1";
        public const string Customer_Fld_Address2 = "Address2";
        public const string Customer_Fld_City = "City";
        public const string Customer_Fld_State = "State";
        public const string Customer_Fld_Zip = "Zip";
        public const string Customer_Fld_TelephoneNo = "PhoneOff";
        public const string Customer_Fld_PhoneOffice = "PhoneOff";
        public const string Customer_Fld_FaxNo = "FaxNo";
        public const string Customer_Fld_CellNo = "CellNo";
        public const string Customer_Fld_URL = "WebAddress";
        public const string Customer_Fld_PhoneHome = "PhoneHome";
        public const string Customer_Fld_Email = "EmailAddress";
        public const string Customer_Fld_IsActive = "IsActive";
        public const string Customer_Fld_Process810 = "Process810";
        public const string Customer_Fld_UseForCustomerLoyalty = "UseForCustomerLoyalty";
        public const string Customer_Fld_Comments = "Comments";
        public const string Customer_Fld_DateOfBirth = "DateOfBirth";
        public const string Customer_Fld_Gender = "Gender";
        public const string Customer_Fld_PatientNo = "PatientNo";
        public const string Customer_Fld_SaveCardProfile = "SaveCardProfile";   //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added

        //Prog1 07Aug2009
        public const string Customer_Fld_CustomerCode = "CustomerCode";
        public const string Customer_Fld_FirstName = "FirstName";
        //End Prog1 07Aug2009

        //PRIMEPOS-3556
        public const string Customer_Fld_DOB = "DOB";
        public const string Customer_Fld_Name = "Name";

        public const string Customer_Fld_DriveLicNo = "DriveLicNo";
        public const string Customer_Fld_DriveLicState = "DriveLicState";
        public const string Customer_Fld_CLCardID = "CLCardID";
        public const string Customer_Fld_Discount = "Discount";//Added By shitaljit 0n 17 Feb 2012
        public const string Customer_Fld_LanguageId = "LanguageId";//Added By Shitajit on 10 Oct 2013
        // TaxCodes file Constants
        public const string TaxCodes_tbl = "TaxCodes";
        public const string TaxCodes_Fld_TaxID = "TaxID";
        public const string TaxCodes_Fld_TaxCode = "TaxCode";
        public const string TaxCodes_Fld_Description = "Description";
        public const string TaxCodes_Fld_Amount = "Amount";
        public const string TaxCodes_Fld_TaxType = "TaxType";
        public const string TaxCodes_Fld_RxTax = "RxTax"; //PRIMEPOS-2924 03-Dec-2020 JY Added
        public const string TaxCodes_Fld_Active = "Active";//2664 Arvind

        // TaxCodes file Constants
        public const string InvTransType_tbl = "InvTransType";
        public const string InvTransType_Fld_ID = "ID";
        public const string InvTransType_Fld_TypeName = "TypeName";
        public const string InvTransType_Fld_TransType = "TransType";
        public const string InvTransType_Fld_UserID = "UserID";

        //ErrorLog Constants
        public const string ErrorLog_tbl = "ErrorLog";
        public const string ErrorLog_Fld_ErrorID = "ErrorID";
        public const string ErrorLog_Fld_ErrorType = "ErrorType";
        public const string ErrorLog_Fld_ErrorDateTime = "ErrorDateTime";
        public const string ErrorLog_Fld_ErrorDescription = "ErrorDescription";
        public const string ErrorLog_Fld_ErrorSource = "ErrorSource";
        public const string ErrorLog_Fld_ErrorStackTrace = "ErrorStackTrace";
        public const string ErrorLog_Fld_ErrorCausedByMethod = "ErrorCausedByMethod";
        public const string ErrorLog_Fld_HelpLink = "HelpLink";
        public const string ErrorLog_Fld_OptionalText1 = "OptionalText1";
        public const string ErrorLog_Fld_OptionalText2 = "OptionalText2";
        public const string ErrorLog_Fld_UserID = "UserID";
        public const string ErrorLog_Fld_StationID = "StationID";

        //Custom error file
        public const string CustomError_tbl = "CustomError";
        public const string CustomError_Fld_ErrorID = "ErrorID";
        public const string CustomError_Fld_ErrorMessage = "ErrorMessage";

        //SubDepartment file
        public const string SubDepartment_tbl = "SubDepartment";
        public const string SubDepartment_Fld_SubDepartmentID = "SubDepartmentID";
        public const string SubDepartment_Fld_DepartmentID = "DepartmentID";
        public const string SubDepartment_Fld_Description = "Description";
        public const string SubDepartment_Fld_IncludeOnSale = "IncludeOnSale";
        public const string SubDepartment_Fld_PointsPerDollar = "PointsPerDollar";  //Sprint-18 - 2041 27-Oct-2014 JY  Added
        public const string SubDepartment_Fld_EXCLUDEFROMCLCouponPay = "EXCLUDEFROMCLCouponPay";

        //Department file
        public const string Department_tbl = "Department";
        public const string Department_Fld_DeptID = "DeptID";
        public const string Department_Fld_DeptCode = "DeptCode";
        public const string Department_Fld_UserID = "UserID";
        public const string Department_Fld_DeptName = "DeptName";
        public const string Department_Fld_IsTaxable = "IsTaxable";
        public const string Department_Fld_TaxID = "TaxID";
        public const string Department_Fld_Discount = "Discount";
        public const string Department_Fld_SaleDiscount = "SaleDiscount";
        public const string Department_Fld_SaleStartDate = "SaleStartDate";
        public const string Department_Fld_SaleEndDate = "SaleEndDate";
        public const string Department_Fld_SalePrice = "SalePrice";
        public const string Department_Fld_PointsPerDollar = "PointsPerDollar"; //Sprint-18 - 2041 26-Oct-2014 JY Added
        public const string Department_Fld_EXCLUDEFROMCLCouponPay = "EXCLUDEFROMCLCouponPay";

        //Station Close Header
        public const string StationCloseHeader_tbl = "StationCloseHeader";
        public const string StationCloseHeader_Fld_StationCloseID = "StationCloseID";
        public const string StationCloseHeader_Fld_StationID = "StationID";
        public const string StationCloseHeader_Fld_CloseDate = "CloseDate";
        public const string StationCloseHeader_Fld_EODID = "EODID";
        public const string StationCloseHeader_Fld_UserId = "UserID";
        public const string StationCloseHeader_Fld_DefCDStartBalance = "DefCDStartBalance";   //Sprint-19 - 2165 18-Mar-2015 JY Added 

        //Item file
        public const string item_PriceInv_Lookup = "PricInvLookup";
        public const string Item_tbl = "Item";
        public const string Item_Fld_ItemID = "ItemID";
        public const string Item_Fld_DepartmentID = "DepartmentID";
        public const string Item_Fld_UserID = "UserID";
        public const string Item_Fld_Description = "Description";
        public const string Item_Fld_Itemtype = "Itemtype";
        public const string Item_Fld_ProductCode = "ProductCode";
        public const string Item_Fld_SaleTypeCode = "SaleTypeCode";
        public const string Item_Fld_SeasonCode = "SeasonCode";
        public const string Item_Fld_Unit = "Unit";
        public const string Item_Fld_Freight = "Freight";
        public const string Item_Fld_SellingPrice = "SellingPrice";
        public const string Item_Fld_AvgPrice = "AvgPrice";
        public const string Item_Fld_LastCostPrice = "LastCostPrice";
        public const string Item_Fld_isTaxable = "isTaxable";
        public const string Item_Fld_TaxID = "TaxID";
        public const string Item_Fld_isOnSale = "isOnSale";
        public const string Item_Fld_isDiscountable = "isDiscountable";
        public const string Item_Fld_Discount = "Discount";
        public const string Item_Fld_SaleStartDate = "SaleStartDate";
        public const string Item_Fld_SaleEndDate = "SaleEndDate";
        public const string Item_Fld_OnSalePrice = "OnSalePrice";
        public const string Item_Fld_QtyInStock = "QtyInStock";
        public const string Item_Fld_Location = "Location";
        public const string Item_Fld_MinOrdQty = "MinOrdQty";
        public const string Item_Fld_ReOrderLevel = "ReOrderLevel";
        public const string Item_Fld_QtyOnOrder = "QtyOnOrder";
        public const string Item_Fld_ExptDeliveryDate = "ExptDeliveryDate";
        public const string Item_Fld_LastVendor = "LastVendor";
        public const string Item_Fld_PreferredVendor = "PreferredVendor";
        public const string Item_Fld_LastRecievDate = "LastRecievDate";
        public const string Item_Fld_LastSellingDate = "LastSellingDate";
        public const string Item_Fld_Remarks = "Remarks";
        public const string Item_Fld_ExclFromAutoPO = "ExclFromAutoPO";
        public const string Item_Fld_ExclFromRecpt = "ExclFromRecpt";
        public const string Item_Fld_isOTCItem = "isOTCItem";
        public const string Item_Fld_IsOfVendor = "IsOfVendor";
        public const string Item_Fld_UpdatePrice = "UpdatePrice";
        public const string Item_Fld_SubDepartmentID = "SubDepartmentID";
        public const string Item_Fld_PacketQuant = "Packet Quantity";

        //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 18-04-2011
        public const string Item_Fld_ItemAddedDate = "ItemAddedDate";
        public const string Item_Fld_TaxPolicy = "TaxPolicy";//Added on 18 August 2011
        //End OfAdded By Shitaljit(QuicSolv)
        public const string Item_Fld_ManufacturerName = "ManufacturerName";
        //Added by Krishna on 5 October 2011
        public const string Item_Fld_ExpDate = "ExpDate";
        public const string Item_Fld_LotNumber = "LotNumber";
        //Till here Added by Krishna on 5 October 2011
        //Added by Ravindra for sale limit 22 March 2013
        public const string Item_Fld_SaleLimitQty = "SaleLimitQty";
        //Added by Ravindra for Sale limit
        public const string Item_Fld_DiscountPolicy = "DiscountPolicy"; //Added By Shitaljit(QuicSolv) on 3 April 2013
        public const string Item_Fld_IsEBTItem = "IsEBTItem";
        //Added By shitaljit for diff CL poins for RX and OTC items.
        public const string Item_Fld_IsDefaultCLPoint = "IsDefaultCLPoint";
        public const string Item_Fld_PointsPerDollar = "PointsPerDollar";
        public const string Item_Fld_CLPointPolicy = "CLPointPolicy";   //Sprint-18 - 2041 28-Oct-2014 JY  Added
        public const string Item_Fld_IsActive = "IsActive";  //Sprint-21 - 2206 06-Jul-2015 JY Added constant for IsActive
        public const string Item_Fld_IsNonRefundable = "IsNonRefundable";   //PRIMEPOS-2592 01-Nov-2018 JY Added

        public const string Item_Fld_EXCLUDEFROMCLCouponPay = "EXCLUDEFROMCLCouponPay";

        #region Solutran PRIMEPOS-2663 
        public const string Item_Fld_S3TransID = "S3TransID";
        public const string Item_Fld_S3PurAmount = "S3PurAmount";
        public const string Item_Fld_S3DiscountAmount = "S3DiscountAmount";
        public const string Item_Fld_S3TaxAmount = "S3TaxAmount";
        #endregion        

        //constants for payout
        public const string PayOut_tbl = "PayOut";
        public const string PayOut_Fld_PayOutID = "PayOutID";
        public const string PayOut_Fld_Description = "Description";
        public const string PayOut_Fld_UserID = "UserID";
        public const string PayOut_Fld_Amount = "Amount";
        public const string PayOut_Fld_StationID = "StationID";
        public const string PayOut_Fld_TransDate = "TransDate";
        public const string PayOut_Fld_DrawNo = "DrawNo";
        public const string PayOut_Fld_PayoutCatType = "PayoutCatType";
        public const string PayOut_Fld_PayoutCatID = "PayoutCatID";

        //constant for Payout category Table - Shitaljit
        public const string PayOutCat_tbl = "PayOutCategory";//PayOutCategory
        public const string payoutCat_Fld_Id = "ID";
        public const string PayOutCat_Fld_PayoutType = "PayoutCatType";
        public const string PayOutCat_Fld_PayoutTypeId = "PayoutTypeId";
        public const string PayOutCat_Fld_UserId = "UserId";
        public const string PayOutCat_Fld_DefaultDescription = "DefaultDescription";
        //constants for Companion Item
        public const string ItemCompanion_tbl = "ItemCompanion";
        public const string ItemCompanion_Fld_CompanionItemID = "CompanionItemID";
        public const string ItemCompanion_Fld_ItemID = "ItemID";
        public const string ItemCompanion_Fld_ItemDescription = "ItemDescription";
        public const string ItemCompanion_Fld_Amount = "Amount";
        public const string ItemCompanion_Fld_Percentage = "Percentage";

        //constants for ItemGroupPrice
        public const string ItemGroupPrice_tbl = "ItemGroupPrice";
        public const string ItemGroupPrice_Fld_GroupPriceID = "GroupPriceID";
        public const string ItemGroupPrice_Fld_ItemID = "ItemID";
        public const string ItemGroupPrice_Fld_Qty = "Qty";
        public const string ItemGroupPrice_Fld_Cost = "Cost";
        public const string ItemGroupPrice_Fld_SalePrice = "SalePrice";
        public const string ItemGroupPrice_Fld_StartDate = "StartDate";
        public const string ItemGroupPrice_Fld_EndDate = "EndDate";

        //constants for ItemGroupPrice
        public const string ItemVendor_tbl = "ItemVendor";
        public const string ItemVendor_Fld_VendorItemID = "VendorItemID";
        public const string ItemVendor_Fld_ItemDetailID = "ItemDetailID";
        public const string ItemVendor_Fld_ItemID = "ItemID";
        public const string ItemVendor_Fld_VendorID = "VendorID";
        public const string ItemVendor_Fld_VendorCode = "VendorCode";
        public const string ItemVendor_Fld_VendorName = "VendorName";
        public const string ItemVendor_Fld_VendorCostPrice = "VendorCostPrice";
        public const string ItemVendor_Fld_LastOrderDate = "LastOrderDate";
        public const string ItemVendor_Fld_IsDeleted = "IsDeleted";
        //fields Added by SRT(Abhishek)  Date : 01/12/2009
        // fields  in Item vendor table
        public const string ItemVendor_Fld_AvgWholeSalePrice = "AvgWholeSalePrice";
        public const string ItemVendor_Fld_CatPrice = "CatPrice";
        public const string ItemVendor_Fld_ContractPrice = "ContractPrice";
        public const string ItemVendor_Fld_DealerAdjustPrice = "DealerAdjustPrice";
        public const string ItemVendor_Fld_FederalUpperLimitPrice = "FederalUpperLimitPrice";
        public const string ItemVendor_Fld_ManufacturerSuggPrice = "ManufacturerSuggPrice";
        public const string ItemVendor_Fld_NetItemPrice = "NetItemPrice";
        public const string ItemVendor_Fld_ProducerPrice = "ProducerPrice";
        public const string ItemVendor_Fld_RetailPrice = "RetailPrice";
        // end of  fields in Item vendor table

        //Inventory recieved header file
        public const string InvRecvHeader_tbl = "InventoryRecieved";
        public const string InvRecvHeader_Fld_InvRecvID = "InvRecievedID";
        public const string InvRecvHeader_Fld_UserID = "UserID";
        public const string InvRecvHeader_Fld_RefNo = "RefNo";
        public const string InvRecvHeader_Fld_VendorID = "VendorID";
        public const string InvRecvHeader_Fld_RecieveDate = "RecieveDate";
        public const string InvRecvHeader_Fld_InvTransTypeID = "InvTransTypeID";//Added By Shitaljit(QuicSolv) on june 24 2011
        public const string InvRecvHeader_Fld_POOrderNo = "POOrderNo";//Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577

        //Inventory recieved detail file
        public const string InvRecvDetail_tbl = "InvRecievedDetail";
        public const string InvRecvDetail_Fld_InvRecvDetailID = "InvRecvDetailID";
        public const string InvRecvDetail_Fld_InvRecievedID = "InvRecievedID";
        public const string InvRecvDetail_Fld_QTY = "Qty";
        public const string InvRecvDetail_Fld_QtyOrdered = "QtyOrdered";
        public const string InvRecvDetail_Fld_Cost = "Cost";
        public const string InvRecvDetail_Fld_ItemID = "ItemID";
        public const string InvRecvDetail_Fld_SalePrice = "SalePrice";
        public const string InvRecvDetail_Fld_Comments = "Comments";
        public const string InvRecvDetail_Fld_TotalCost = "TotalCost";  //Sprint-21 - 2002 21-Jul-2015 JY Added
        public const string InvRecvDetail_Fld_ExpDate = "ExpDate";  //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
        public const string InvRecvDetail_Fld_LastInvUpdatedQty = "LastInvUpdatedQty";  //PRIMEPOS-2396 12-Jun-2018 JY Added
        public const string InvRecvDetail_Fld_DeptID = "DeptID";    //PRIMEPOS-2419 28-May-2019 JY Added
        public const string InvRecvDetail_Fld_SubDepartmentID = "SubDepartmentID";  //PRIMEPOS-2419 28-May-2019 JY Added
        public const string InvRecvDetail_Fld_IsEBTItem = "IsEBTItem";  //PRIMEPOS-2419 28-May-2019 JY Added
        //

        //Purchase Order header file
        public const string POHeader_tbl = "PurchaseOrder";
        public const string POHeader_CompNotRecvd = "PurchaseOrderCompNotRecvd";
        public const string POHeader_Fld_OrderID = "OrderID";
        public const string POHeader_Fld_UserID = "UserID";
        public const string POHeader_Fld_OrderNo = "OrderNo";
        public const string POHeader_Fld_VendorID = "VendorID";
        public const string POHeader_Fld_OrderDate = "OrderDate";
        public const string POHeader_Fld_ExptDelvDate = "ExptDeliveryDate";
        public const string POHeader_Fld_Status = "Status";
        public const string POHeader_Fld_isInvRecieved = "isInvRecieved";
        public const string POHeader_Fld_isFTPUsed = "isFTPUsed";
        public const string POHeader_Fld_AckType = "AckType";
        public const string POHeader_Fld_AckStatus = "AckStatus";
        public const string POHeader_Fld_AckDate = "AckDate";

        //Purchanse order on hold table 
        //Added By shitaljit on 6 May 2014
        public const string POHeaderOnHold_tbl = "PurchaseOrder_OnHold";
        public const string PODetailOnHold_tbl = "PurchaseOrderDetail_OnHold";
        public const string NotAvailableItemCode = "NA";
        //End 
        public const string HPS = "HPS";
        public const string POHeader_Fld_InvoiceDate = "InvoiceDate";
        public const string POHeader_Fld_InvoiceNumber = "InvoiceNumber";
        //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //Coulomns Added for VendorInterface
        public const string POHeader_Fld_PrimePOrderID = "PrimePOrderID";
        public const string POHeader_Fld_IsMaxReached = "IsMaxReached";
        //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //Added By Ravindra (QuicSolv) 16 Jan 2013
        public const string POHeader_Fld_RefOrderNo = "RefOrderNo";
        //End of Added by Ravindra(Quicsolv) 16 Jan 2013
        public const string POHeader_Fld_AckFileType = "AckFileType";
        //Added By SRT(Gaurav) Date : 04/07/2009
        public const string POHeader_Fld_Description = "Description";
        public const string POHeader_Fld_Flagged = "Flagged";
        //End Of Added By SRT(Gaurav)
        public const string POHeader_Fld_ProcessedBy = "ProcessedBy";   //Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added 
        public const string POHeader_Fld_ProcessedType = "ProcessedType";   //Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added 
        public const string POHeader_Fld_TransTypeCode = "TransTypeCode";   //PRIMEPOS-2901 05-Nov-2020 JY Added

        //Purchase Order detail file
        public const string PODetail_tbl = "PurchaseOrderDetail";
        public const string PODetail_Fld_PODetailID = "PODetailID";
        public const string PODetail_Fld_OrderID = "OrderID";
        public const string PODetail_Fld_QTY = "Qty";
        public const string PODetail_Fld_AckQTY = "AckQty";
        public const string PODetail_Fld_Cost = "Cost";
        public const string PODetail_Fld_Price = "Price";
        public const string PODetail_Fld_ItemID = "ItemID";
        public const string PODetail_Fld_DisplayItemID = "Item ID#";
        public const string PODetail_Fld_Comments = "Comments";
        public const string PODetail_Fld_AckStatus = "AckStatus";
        public const string PODetail_Fld_VendorItemCode = "VendorItemCode";
        public const string PODetail_Fld_LastCostPrice = "LastCostPrice";//Added BY Ravindra PRIMEPOS-1043
        public const string PODetail_Fld_ItemSold = "SoldItems";

        public const string PODetail_Fld_ItemDescType = "ItemDescType";
        public const string PODetail_Fld_Idescription = "Idescription";

        public const string PODetail_Fld_PackSize = "Packet Size";
        public const string PODetail_Fld_PackUnit = "Packet Unit";
        public const string PODetail_Fld_PackQuant = "Packet Quantity";
        public const string PODetail_Fld_QtyInHand = "Qty In Hand";

        public const string MasterOrderDetails_tbl = "MasterOrderDetailsTable";
        public const string MasterOrderDetails_Fld_VendorID = "VendorID";
        public const string MasterOrderDetails_Fld_VendorName = "VendorName";
        public const string MasterOrderDetails_Fld_TotalPOs = "TotalPO";

        public const string OrderDetail_tbl = "OrderDetailsTable";
        public const string OrderDetail_Tbl_TimeToOrder = "TimeToOrder";
        public const string OrderDetail_Fld_TimeToOrder = "Time To Order";
        public const string OrderDetail_Tbl_TotalItems = "TotalItems";
        public const string OrderDetail_Fld_TotalItems = "Total Items";
        public const string OrderDetail_Tbl_TotalQty = "TotalQty";
        public const string OrderDetail_Fld_TotalQty = "Total Qty";
        public const string OrderDetail_Tbl_TotalCost = "TotalCost";
        public const string OrderDetail_Fld_TotalCost = "Total Cost";
        public const string OrderDetail_Tbl_AuroSend = "IsAutoClose";
        public const string OrderDetail_Fld_AuroSend = "AutoSend";
        public const string OrderDetail_Tbl_VendorName = "VendorName";
        public const string OrderDetail_Fld_VendorName = "Vendor Name";
        public const string OrderDetail_Tbl_OrderNo = "OrderNo";
        public const string OrderDetail_Fld_OrderNo = "Order No";
        public const string OrderDetail_Tbl_OrderId = "OrderID";
        public const string OrderDetail_Fld_OrderId = "Order ID";
        public const string OrderDetail_Fld_CloseOrder = "Close Order";
        public const string OrderDetail_Tbl_CloseOrder = "CloseOrder";
        public const string PODetail_Fld_ProcessedQty = "ProcessedQty";// Add by Ravindra to Save Processed Qty 3 April 2013

        //Added by Prashant(SRT) Date:4-06-2009
        public const string PODetail_Fld_VendorCode = "VendorCode";
        public const string PODetail_Fld_VendorName = "VendorName";
        public const string PODetail_Fld_BestVendPrice = "Best Vend.Price";
        public const string PODetail_Fld_BestVendor = "Best Vendor";
        public const string PODetail_Fld_LastOrdVendor = "Last Ord.Vendor";
        public const string PODetail_Fld_LastOrdQty = "Last Ord.Qty";
        //End of Added by Prashant(SRT) Date:4-06-2009

        public const string PODetail_Fld_ChangedProductQualifier = "ChangedProductQualifier";
        public const string PODetail_Fld_ChangedProductID = "ChangedProductID";
        //Added By SRT(Gaurav) Date: 03-Jul-2009
        public const string PODetail_Fld_QtySold100Days = "QtySold100Days";
        public const string PODetail_Fld_ReOrderLevel = "ReOrderLevel";
        public const string PODetail_Fld_QtyInStock = "QtyInStock";
        //End OF Added By SRT(Gaurav)
        //Added By Amit Date 05 jul 2011
        public const string PODetail_Fld_DeptName = "DeptName";
        public const string PODetail_Fld_SubDeptName = "SubDeptName";
        //End
        //Added By Amit Date 27 july 2011
        public const string PODetail_Fld_RetailPrice = "RetailPrice";
        public const string PODetail_Fld_ItemPrice = "ItemPrice";
        public const string PODetail_Fld_Discount = "Discount";
        public const string PODetail_Fld_InvRecDate = "InvRecDate";
        //End
        public const string Users_tbl = "Users";
        public const string UsersGroup_tbl = "UsersGroup";
        public const string Users_Fld_ID = "ID";
        public const string Users_Fld_UserID = "UserID";
        public const string Users_Fld_Password = "Password";
        public const string Users_Fld_fName = "fName";
        public const string Users_Fld_lName = "lName";
        public const string Users_Fld_ModifiedOn = "ModifiedOn";
        public const string Users_Fld_DrawNo = "DrawNo";
        public const string Users_Fld_Process810 = "Process810";
        public const string Users_Fld_SecurityLevel = "SecurityLevel";
        public const string Users_Fld_IsActive = "IsActive";
        public const string Users_Fld_loginRegistration = "LoginRegistration";
        public const string Users_Fld_AllowReturnTransaction = "AllowReturnTransaction";
        public const string Users_Fld_AllowPriceOverride = "AllowPriceOverride";
        public const string Users_Fld_AllowItemDeletion = "AllowItemDeletion";
        public const string Users_Fld_AllowItemDiscount = "AllowItemDiscount";
        public const string Users_Fld_AllowNoSaleTransaction = "AllowNoSaleTransaction";
        public const string Users_Fld_AllowInventoryMenu = "AllowInventoryMenu";
        public const string Users_Fld_AllowItemFileEdit = "AllowItemFileEdit";
        public const string Users_Fld_AllowVendorFileEdit = "AllowVendorFileEdit";
        public const string Users_Fld_AllowInventoryReports = "AllowInventoryReports";
        public const string Users_Fld_AllowAllowAdministrativeMenu = "AllowAllowAdministrativeMenu";
        public const string Users_Fld_AllowCustomerFileEdit = "AllowCustomerFileEdit";
        public const string Users_Fld_AllowPurchaseOrder = "AllowPurchaseOrder";
        public const string Users_Fld_Role = "Role";
        public const string Users_Fld_AllowTransDeletion = "AllowTransDeletion";
        public const string Users_Fld_AllowDeptFileEdit = "AllowDeptFileEdit";
        public const string Users_Fld_AllowCloseStation = "AllowCloseStation";
        public const string Users_Fld_AllowEndOfDay = "AllowEndOfDay";
        public const string Users_Fld_AllowViewPOSTrans = "AllowViewPOSTrans";
        public const string Users_Fld_AllowPayout = "AllowPayout";
        public const string Users_Fld_AllowMgmtReports = "AllowMgmtReports";
        public const string Users_Fld_AllowAppSettings = "AllowAppSettings";
        public const string Users_Fld_AllowSystemSettings = "AllowSystemSettings";
        public const string Users_Fld_AllowEPO = "AllowEPO";
        public const string Users_Fld_AllowTransHold = "AllowHoldTrans";
        public const string Users_Fld_AllowUnclosedStation = "AllowUnclosedStation";
        public const string Users_Fld_IsLocked = "IsLocked";
        public const string Users_Fld_LastLoginAttempt = "LastLoginAttempt";
        public const string Users_Fld_NoOfAttempt = "NoOfAttempt";
        public const string Users_Fld_PasswordChangedOn = "PasswordChangedOn";
        public const string Users_Fld_Password1 = "Password1";
        public const string Users_Fld_Password2 = "Password2";
        public const string Users_Fld_Password3 = "Password3";
        public const string Users_Fld_MaxDiscountLimit = "MaxDiscountLimit";
        public const string Users_Fld_MaxTransactionLimit = "MaxTransactionLimit";//Column Added By Ravindra(QuicSolv) 24 Jan 2013
        public const string Users_Fld_UserType = "UserType";//Added By Shitaljit for Adding user Group 
        public const string Users_Fld_UserImage = "UserImage";
        public const string Users_Fld_MaxReturnTransLimit = "MaxReturnTransLimit";  //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
        public const string Users_Fld_MaxTenderedAmountLimit = "MaxTenderedAmountLimit";  //Sprint-25 - PRIMEPOS-2411 20-Apr-2017 JY Added 
        public const string Users_Fld_MaxCashbackLimit = "MaxCashbackLimit";    //PRIMEPOS-2741 25-Sep-2019 JY Added
        public const string Users_Fld_ModifiedBy = "ModifiedBy";    //PRIMEPOS-2562 01-Aug-2018 JY Added
        public const string Users_Fld_ChangePasswordAtLogin = "ChangePasswordAtLogin";    //PRIMEPOS-2577 15-Aug-2018 JY Added
        public const string Users_Fld_WindowsLoginId = "WindowsLoginId";    //PRIMEPOS-2616 14-Dec-2018 JY Added
        public const string Users_Fld_HourlyRate = "HourlyRate";    //PRIMEPOS-189 02-Aug-2021 JY Added
        public const string Users_Fld_EmailID = "EmailID";  //PRIMEPOS-2989 11-Aug-2021 JY Added
        public const string Users_Fld_GroupID = "GroupID";  //PRIMEPOS-2780 27-Sep-2021 JY Added
        public const string Users_Fld_LanID = "LanID";  //PRIMEPOS-3484
        //

        //Transaction header file
        public const string TransHeader_tbl = "POSTransaction";
        public const string TransHeader_OnHold_tbl = "POSTransaction_OnHold";
        public const string TransHeader_Fld_TransID = "TransID";
        public const string TransHeader_Fld_RxNo = "RxNo";
        public const string TransHeader_Fld_ReturnTransID = "ReturnTransID";
        public const string TransHeader_Fld_CustomerID = "CustomerID";
        public const string TransHeader_Fld_StationID = "StationID";
        public const string TransHeader_Fld_TransDate = "TransDate";
        public const string TransHeader_Fld_TransType = "TransType";
        public const string TransHeader_Fld_GrossTotal = "GrossTotal";
        public const string TransHeader_Fld_TotalDiscAmount = "TotalDiscAmount";
        public const string TransHeader_Fld_TotalTaxAmount = "TotalTaxAmount";
        public const string TransHeader_Fld_TenderedAmount = "TenderedAmount";
        public const string TransHeader_Fld_TotalPaid = "TotalPaid";
        public const string TransHeader_Fld_isStationClosed = "isStationClosed";
        public const string TransHeader_Fld_IsDelivery = "IsDelivery";
        public const string TransHeader_Fld_isEOD = "isEOD";
        public const string TransHeader_Fld_DrawerNo = "DrawerNo";
        public const string TransHeader_Fld_StClosedID = "StClosedID";
        public const string TransHeader_Fld_EODID = "EODID";
        public const string TransHeader_Fld_Account_No = "Account_No";
        public const string TransHeader_Fld_LoyaltyPoints = "LoyaltyPoints";
        public const string TransHeader_Fld_WasonHold = "WasonHold";    //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added     
        public const string TransHeader_Fld_DeliverySigSkipped = "DeliverySigSkipped";  //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

        //Following Code Added by Krishna on 2 June 2011
        public const string TransHeader_Fld_TransactionStartDate = "TransactionStartDate";
        //Till Here Added by Krishna on 2 June 2011

        //Added By Shitaljit(QuicSolv) on 31 August 2011
        public const string TransHeader_Fld_InvoiceDiscount = "InvoiceDiscount";
        //Till Here Added By Shitaljit(QuicSolv) on 31 August 2011
        public const string TransHeader_Fld_DeliveryAddress = "DeliveryAddress";

        #region Added for Solutran - PRIMEPOS-2663 - NileshJ
        public const string TransHeader_Fld_S3TransID = "S3TransID";
        public const string TransHeader_Fld_S3PurAmount = "S3TotalPurAmount";
        public const string TransHeader_Fld_S3TaxAmount = "S3TotalTaxAmount";
        public const string TransHeader_Fld_S3DiscountAmount = "S3TotalDiscountAmount";
        #endregion

        public const string TransHeader_Fld_AllowRxPicked = "AllowRxPicked";    //PRIMEPOS-2865 16-Jul-2020 JY Added
        public const string TransHeader_Fld_RxTaxPolicyID = "RxTaxPolicyID";  //PRIMEPOS-3053 08-Feb-2021 JY Added
        public const string TransHeader_Fld_TotalTransFeeAmt = "TotalTransFeeAmt";  //PRIMEPOS-3117 11-Jul-2022 JY Added

        // Added for Evertec - PRIMEPOS - 2664 Arvind        
        public const string EVERTEC = "EVERTEC";
        public const string VANTIV = "VANTIV"; //PRIMEPOS-2636

        public const string PRIMERXPAY = "PRIMERXPAY"; //PRIMEPOS-2841

        //Transaction detail file
        public const string TransDetail_OnHold_tbl = "POSTransactionDetail_OnHold";
        public const string TransDetail_tbl = "POSTransactionDetail";
        public const string TransDetail_Fld_TransDetailID = "TransDetailID";
        public const string TransDetail_Fld_TransID = "TransID";
        public const string TransDetail_Fld_QTY = "Qty";
        public const string TransDetail_Fld_Price = "Price";
        public const string TransDetail_Fld_ItemCost = "ItemCost";
        public const string TransDetail_Fld_ItemID = "ItemID";
        public const string TransDetail_Fld_Discount = "Discount";
        public const string TransDetail_Fld_TaxAmount = "TaxAmount";
        public const string TransDetail_Fld_ExtendedPrice = "ExtendedPrice";
        public const string TransDetail_Fld_TaxID = "TaxID";
        public const string TransDetail_Fld_ItemRow = "ItemRow";
        public const string TransDetail_Fld_ItemDescription = "ItemDescription";
        public const string TransDetail_Fld_ItemDescriptionMasked = "ItemDescriptionMasked"; //PRIMEPOS-3130
        public const string TransDetail_Fld_UserID = "UserID";
        public const string TransDetail_Fld_IsPriceChanged = "IsPriceChanged";
        public const string TransDetail_Fld_IsPriceChangedByOverride = "IsPriceChangedByOverride";  //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
        public const string TransDetail_Fld_IsIIAS = "IsIIAS";
        public const string TransDetail_Fld_IsCompanionItem = "IsCompanionItem";
        public const string TransDetail_Fld_NetPrice = "NetPrice";//Not a database field
        //Added By SRT(Ritesh Parekh) Date: 17-Aug-2009
        public const string TransDetail_Fld_IsRxItem = "IsRxItem";
        //End Of Added By SRT(Ritesh Parekh)
        public const string TransDetail_Fld_IsEBT = "IsEBT";
        //Following Added by krishna on 15 July 2011
        public const string TransDetail_Fld_ReturnTransDetailId = "ReturnTransDetailId";
        //Till here Added by krishna on 15 July 2011
        //Added By Amit Date 23 Nov 2011
        public const string TransDetail_Fld_IsMonitored = "IsMonitored";
        public const string TransDetail_Fld_Category = "Category";
        //End
        public const string TransDetail_Fld_OrignalPrice = "OrignalPrice";
        public const string TransDetail_Fld_IsComboItem = "IsComboItem";
        public const string TransDetail_Fld_LoyaltyPoints = "LoyaltyPoints";
        public const string TransDetail_Fld_CouponID = "CouponID";  //PRIMEPOS-2034 02-Mar-2018 JY Added
        public const string TransDetail_Fld_IsNonRefundable = "IsNonRefundable";    //PRIMEPOS-2592 02-Nov-2018 JY Added 

        #region Added for Solutran - PRIMEPOS-2663 - NileshJ 
        public const string TransDetail_Fld_S3TransID = "S3TransID";
        public const string TransDetail_Fld_S3PurAmount = "S3PurAmount";
        public const string TransDetail_Fld_S3TaxAmount = "S3TaxAmount";
        public const string TransDetail_Fld_S3DiscountAmount = "S3DiscountAmount";
        #endregion

        public const string TransDetail_Fld_InvoiceDiscount = "InvoiceDiscount";    //PRIMEPOS-2768 18-Dec-2019 JY Added
        public const string TransDetail_Fld_IsOnSale = "IsOnSale";  //PRIMEPOS-2907 13-Oct-2020 JY Added

        #region PRIMEPOS-2402 08-Jul-2021 JY Added
        public const string TransDetail_Fld_OldDiscountAmt = "OldDiscountAmt";
        public const string TransDetail_Fld_DiscountOverrideUser = "DiscountOverrideUser";
        public const string TransDetail_Fld_QuantityOverrideUser = "QuantityOverrideUser";
        public const string TransDetail_Fld_TaxOverrideUser = "TaxOverrideUser";
        public const string TransDetail_Fld_OldTaxCodesWithPercentage = "OldTaxCodesWithPercentage";
        public const string TransDetail_Fld_TaxOverrideAllOTCUser = "TaxOverrideAllOTCUser";
        public const string TransDetail_Fld_TaxOverrideAllRxUser = "TaxOverrideAllRxUser";
        public const string TransDetail_Fld_MaxDiscountLimitUser = "MaxDiscountLimitUser";
        #endregion
        public const string TransDetail_Fld_OverrideRemark = "OverrideRemark";  //PRIMEPOS-3015 26-Oct-2021 JY Added
        public const string TransDetail_Fld_ItemGroupPrice = "ItemGroupPrice";  //PRIMEPOS-3098 20-Jun-2022 JY Added
        public const string TransDetail_Fld_MonitorItemOverrideUser = "MonitorItemOverrideUser"; //PRIMEPOS-3166N
        //TranactionDetailTax file
        public const string TransDetailTax_tbl = "POSTransactionDetailTax";
        public const string TransDetailTax_OnHold_tbl = "POSTransactionDetailTax_OnHold";
        public const string TransDetailTax_fld_TransDetailTaxID = "TransDetailTaxID";
        public const string TransDetailTax_fld_TaxPercent = "TaxPercent";

        //Transaction header file
        public const string TransDetailRX_tbl = "POSTransactionRXDetail";
        public const string TransDetailRX_OnHold_tbl = "POSTransactionRXDetail_OnHold";
        public const string TransDetailRX_Fld_TransDetailID = "TransDetailID";
        public const string TransDetailRX_Fld_RXNo = "RXNo";
        public const string TransDetailRX_Fld_DateFilled = "DateFilled";
        public const string TransDetailRX_Fld_NRefill = "NRefill";
        public const string TransDetailRX_Fld_PartialFillNo = "PartialFillNo";
        public const string TransDetailRX_Fld_DrugNDC = "DrugNDC";
        public const string TransDetailRX_Fld_InsType = "InsType";
        public const string TransDetailRX_Fld_PatType = "PatType";
        public const string TransDetailRX_Fld_RXDetailID = "RXDetailID";
        public const string TransDetailRX_Fld_PatientNo = "PatientNo";
        //added by atul 07-jan-2011
        public const string TransDetailRX_Fld_CounsellingReq = "COUNSELLINGREQ";
        public const string TransDetailRX_Fld_Ezcap = "EZCAP";
        //end of added by atul 07-jan-2011
        public const string TransDetailRx_Fld_ReturnTransDetailID = "RETURNTRANSDETAILID";
        public const string TransDetailRX_Fld_Delivery = "Delivery";    //PRIMEPOS-3008 30-Sep-2021 JY Added
        //table FunctionKeys Constants
        public const string FunctionKeys_tbl = "FunctionKeys";
        public const string FunctionKeys_Fld_KeyId = "KeyId";
        public const string FunctionKeys_Fld_FunKey = "FunKey";
        public const string FunctionKeys_Fld_FunctionType = "FunctionType";
        public const string FunctionKeys_Fld_Parent = "Parent";
        public const string FunctionKeys_Fld_Operation = "Operation";
        public const string FunctionKeys_Fld_ButtonBackColor = "ButtonBackColor";
        public const string FunctionKeys_Fld_ButtonForeColor = "ButtonForeColor";
        public const string FunctionKeys_Type_Item = "I";
        public const string FunctionKeys_Type_Parent = "P";
        public const string FunctionKeys_Fld_MainPosition = "MainPosition";
        public const string FunctionKeys_Fld_SubPosition = "SubPosition";

        #region POS TransPayment
        //pos transaction payment file
        public const string POSTransPayment_tbl = "POSTransPayment";
        public const string POSTransPayment_Fld_TransPayID = "TransPayID";
        public const string POSTransPayment_Fld_TransTypeCode = "TransTypeCode";
        public const string POSTransPayment_Fld_HC_Posted = "HC_Posted";
        public const string POSTransPayment_Fld_TransDate = "TransDate";
        public const string POSTransPayment_Fld_TransID = "TransID";
        public const string POSTransPayment_Fld_Amount = "Amount";
        public const string POSTransPayment_Fld_RefNo = "RefNo";
        public const string POSTransPayment_Fld_AuthNo = "AuthNo";
        public const string POSTransPayment_Fld_CCName = "CCName";
        public const string POSTransPayment_Fld_CCTransNo = "CCTransNo";
        public const string POSTransPayment_Fld_ExpDate = "ExpDate";
        public const string POSTransPayment_Fld_CustomerSign = "CustomerSign";
        public const string POSTransPayment_Fld_BinarySign = "BinarySign";
        public const string POSTransPayment_Fld_SigType = "SigType";
        public const string POSTransPayment_Fld_IsIIASPayment = "IsIIASPayment";
        //Added By SRT(Gaurav) Date : 21-Jul-2009
        public const string POSTransPayment_Fld_PaymentProcessor = "PaymentProcessor";
        //End Of Added By SRT(gaurav)
        //Added By Shitaljit on 19 july 2012 to store Processor TransID
        public const string POSTransPayment_Fld_ProcessorTransID = "ProcessorTransID";
        public const string POSTransPayment_Fld_CLCouponID = "CLCouponID";
        public const string POSTransPayment_Fld_IsManual = "IsManual";    //Sprint-19 - 2139 06-Jan-2015 JY Added 
        public const string POSTransPayment_Fld_CashBack = "CashBack";

        public const string POSTransPayment_Fld_S3TransID = "S3TransID"; //  Added for Solutran - PRIMEPOS-2663 - NileshJ

        //Added by Arvind on July 18 2019 for Evertec Receipt PRIMEPOS-2664
        public const string POSTransPayment_Fld_BatchNumber = "BatchNumber";
        public const string POSTransPayment_Fld_TraceNumber = "TraceNumber";
        public const string POSTransPayment_Fld_InvoiceNumber = "InvoiceNumber";
        //Added by Arvind on July 18 2019 for Evertec Receipt PRIMEPOS-2636 VANTIV,
        public const string POSTransPayment_Fld_TerminalID = "TerminalID";
        public const string POSTransPayment_Fld_ReferenceNumber = "ReferenceNumber";
        public const string POSTransPayment_Fld_TransactionID = "TransactionID";
        public const string POSTransPayment_Fld_ResponseCode = "ResponseCode";

        //Added by Arvind PRIMEPOS-2738
        public const string POSTransPayment_Fld_ReversedAmount = "ReversedAmount";
        //

        #region Emv
        public const string POSTransPayment_Fld_AID = "Aid";
        public const string POSTransPayment_Fld_AIDNAME = "AidName";
        public const string POSTransPayment_Fld_CRYTOGRAM = "Cryptogram_Ac";
        public const string POSTransPayment_Fld_TransactionCounter = "TransactionCounter_Atc";
        public const string POSTransPayment_Fld_TerminalTVR = "Terminal_Tvr";
        public const string POSTransPayment_Fld_TransStatusInfo = "TransStatusInfo_Tsi";
        public const string POSTransPayment_Fld_AuthorizationResponse = "AuthorizationRespCode_Cd";
        public const string POSTransPayment_Fld_TransRefNum = "TransRefNum_Trn";
        public const string POSTransPayment_Fld_ValidateCode = "ValidateCode_Vc";
        public const string POSTransPayment_Fld_MerchantID = "MerchantID";
        public const string POSTransPayment_Fld_RTransactionID = "RTransID"; //for xcharge receipt_transactionID
        public const string POSTransPayment_Fld_EntryLegend = "EntryLegend";
        public const string POSTransPayment_Fld_EntryMethod = "EntryMethod";
        public const string POSTransPayment_Fld_ProfiledID = "ProfiledID";
        public const string POSTransPayment_Fld_CardType = "CardType_Ct";
        public const string POSTransPayment_Fld_ProcTransType = "ProcTransType";
        public const string POSTransPayment_Fld_Verbiage = "Verbiage";
        public const string POSTransPayment_Fld_ApprovalCode = "ApprovalCode";

        #region PRIMEPOS-2793 
        public const string POSTransPayment_Fld_ApplicationLabel = "ApplicationLabel";
        public const string POSTransPayment_Fld_PinVerified = "PinVerified";
        public const string POSTransPayment_Fld_LaneID = "LaneID";
        public const string POSTransPayment_Fld_CardLogo = "CardLogo";
        #endregion

        // Added By Rohit Nair on Sept 08 2016 for WP EMV
        public const string POSTransPayment_Fld_IssuerAppData = "IssuerAppData_IAD";
        public const string POSTransPayment_Fld_CardVerificationMethod = "CardVerificationMethod_CVM";
        public const string POSTransPayment_Fld_A_IDNAME = "A_idName"; // PRIMEPOS-2754 - NileshJ - 20_Jan_2020
        #endregion Emv

        //PRIMEPOS-2664 ADDED BY ARVIND 
        public const string POSTransPayment_Fld_ControlNumber = "ControlNumber";
        public const string POSTransPayment_Fld_ATHMovil = "ATHMovil";//2664
        public const string POSTransPayment_Fld_EbtBalance = "EbtBalance";
        public const string POSTransPayment_Fld_TransFeeAmt = "TransFeeAmt";    //PRIMEPOS-3117 11-Jul-2022 JY Added
        public const string POSTransPayment_Fld_Tokenize = "Tokenize";   //PRIMEPOS-3145 28-Sep-2022 JY Added
        #endregion POS TransPayment

        //pos transaction payment file
        public const string PayType_tbl = "PayType";
        public const string PayType_Fld_PayTypeID = "PayTypeID";
        public const string PayType_Fld_PayTypeDescription = "PayTypeDesc";
        public const string PayType_Fld_PayType = "PayType";
        public const string PayType_Fld_IsHide = "IsHide";    //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
        public const string PayType_Fld_StopAtRefNo = "StopAtRefNo";    //PRIMEPOS-2309 08-Mar-2019 JY Added
        public const string PayType_Fld_CustomPayType = "CustomPayType";    //PRIMEPOS-2940 30-Mar-2021 JY Added
        public const string PayType_Fld_SortOrder = "SortOrder";    //PRIMEPOS-2966 20-May-2021 JY Added

        //constants for physical inventory
        public const string PhysicalInv_tbl = "PhysicalInv";
        public const string PhysicalInv_Fld_ID = "ID";
        public const string PhysicalInv_Fld_ItemCode = "ItemCode";
        public const string PhysicalInv_Fld_UserID = "UserID";
        public const string PhysicalInv_Fld_PUserID = "PUserID";
        public const string PhysicalInv_Fld_OldQty = "OldQty";
        public const string PhysicalInv_Fld_NewQty = "NewQty";
        public const string PhysicalInv_Fld_TransDate = "TransDate";
        public const string PhysicalInv_Fld_PTransDate = "PTransDate";
        public const string PhysicalInv_Fld_isProcessed = "isProcessed";
        public const string PhysicalInv_Fld_OldSellingPrice = "OldSellingPrice";
        public const string PhysicalInv_Fld_OldCostPrice = "OldCostPrice";
        public const string PhysicalInv_Fld_NewSellingPrice = "NewSellingPrice";
        public const string PhysicalInv_Fld_NewCostPrice = "NewCostPrice";
        public const string PhysicalInv_Fld_OldExpDate = "OldExpDate";    //Sprint-21 - 2206 09-Mar-2016 JY Added
        public const string PhysicalInv_Fld_NewExpDate = "NewExpDate";    //Sprint-21 - 2206 09-Mar-2016 JY Added
        public const string PhysicalInv_Fld_LastInvUpdatedQty = "LastInvUpdatedQty";  //PRIMEPOS-2395 21-Jun-2018 JY Added

        public const string EndOfDay_tbl = "EndOfDayHeader";

        public const string PrimeRX_HouseChargeInterface = "RXHouseChargeInterface";
        public const string PrimeRX_PatientInterface = "RXPatientInterface";

        public const string VendorCostPrice_View = "VendorCostPrice";

        //constants for Item Price Validation
        public const string ItemPriceValidation_tbl = "ItemPriceValidation";
        public const string ItemPriceValidation_Fld_AllowNegative = "AllowNegative";
        public const string ItemPriceValidation_Fld_MinSellingAmount = "MinSellingAmount";
        public const string ItemPriceValidation_Fld_Process810 = "Process810";
        public const string ItemPriceValidation_Fld_MinSellingPercentage = "MinSellingPercentage";
        public const string ItemPriceValidation_Fld_MinCostPercentage = "MinCostPercentage";
        public const string ItemPriceValidation_Fld_IsActive = "IsActive";
        public const string ItemPriceValidation_Fld_ItemID = "ItemID";
        public const string ItemPriceValidation_Fld_DeptID = "DeptID";
        public const string ItemPriceValidation_Fld_ID = "ID";

        //FileHistory
        public const string FileHistory_tbl = "FileHistory";
        public const string FileHistory_Fld_FileID = "FileID";
        public const string FileHistory_Fld_LastUpdateDate = "LastUpdateDate";
        public const string FileHistory_Fld_SynchronizedCentrally = "SynchronizedCentrally";

        //Following Coded added by Krishna on 12 October 2011
        //Util_DBBackuplog
        public const string DBBackuplog_tbl = "Util_DBBackuplog";
        public const string DBBackuplog_BackupDate = "BackupDate";
        public const string DBBackuplog_BackupPath = "BackupPath";
        //till here added by Krishna on 12 October 2011

        public const string AWP = "AWP";
        public const string CAT = "CAT";
        public const string CON = "CON";
        public const string DAP = "DAP";
        public const string FUL = "FUL";
        public const string MSR = "MSR";
        public const string NET = "NET";
        public const string PRO = "PRO";
        public const string RES = "RES";
        //Added by Atul Joshi on 22-10-2010
        public const string UCP = "UCP";
        //Added by SRT(Abhishek) Date : 02/09/2009
        public const string RTL = "RTL";
        //End of Added by SRT(Abhishek) Date : 02/09/2009
        public const string RLT = "RLT";    //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added promotional price qualifier for kinray    
        public const string PRP = "PRP";    //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added promotional price qualifier for other vendors
        //Added by SRT(Abhishek) Date : 24/09/2009
        public const string INV = "INV";
        public const string PBQ = "PBQ";
        public const string BCH = "BCH";
        public const string RESM = "RESM";
        //End Of Added by SRT(Abhishek) Date : 24/09/2009

        //
        public const string NONE = "None";
        public const string Pending = "Pending";
        public const string Processed = "Processed";
        public const string Queued = "Queued";
        public const string Canceled = "Canceled";
        public const string Acknowledge = "Acknowledge";
        public const string AcknowledgeManually = "AcknowledgeManually";
        public const string Submitted = "Submitted";
        public const string MaxAttempt = "MaxAttempt";
        public const string Incomplete = "Incomplete";
        public const string Expired = "Expired";
        //START:Added by SRT(Prashant) Date:17-3-09
        public const string PartiallyAcknowledge = "PartiallyAck";
        public const string PartiallyAckReorder = "PartiallyAck-Reorder";
        //END:Added by SRT(Prashant) Date:17-3-09
        public const string Error = "Error";
        public const string All = "All";
        //Added by SRT(Abhishek) Date : 07/07/2009
        public const string Overdue = "Overdue";
        //None
        //Added by SRT(Abhishek) Date : 07/15/2009
        public const string SubmittedManually = "SubmittedManually";
        //
        //Add By SRT(Sachin) Date 18 Feb 2010
        public const string DirectAcknowledge = "DirectAcknowledge";
        //End of Add By SRT(Sachin) Date 18 Feb 2010
        public const string DeliveryReceived = "DeliveryReceived";
        //constants for timesheet
        public const string Timesheet_tbl = "Timesheet";
        public const string Timesheet_Fld_ID = "ID";
        public const string Timesheet_Fld_UserID = "UserID";
        public const string Timesheet_Fld_TimeIn = "TimeIn";
        public const string Timesheet_Fld_TimeOut = "TimeOut";
        public const string Timesheet_Fld_IsManualIn = "IsManualIn";
        public const string Timesheet_Fld_IsManualOut = "IsManualOut";
        public const string Timesheet_Fld_LastUpdatedBy = "LastUpdatedBy";
        public const string Timesheet_Fld_LastUpdatedOn = "LastUpdatedOn";

        //Constants for Add and Edit
        public const string Add = "Add";
        public const string Edit = "Edit";

        //Message Log Table Constants
        public const string MessageLog_tbl = "MessageLog";
        public const string MessageLog_Fld_Date = "LogDate";
        //Added By Abhishek(SRT) 06-02-2009
        public const string MessageLog_Fld_Time = "LogTime";
        //End of Added By Abhishek(SRT) 06-02-2009
        public const string MessageLog_Fld_MessageString = "LogMessage";

        //Message Log Error Constants
        public const string LogErrorMessage1 = "The Number of Days cannot be more than 99";
        public const string LogErrorMessage2 = "The Number of Days should be Numeric";

        //Constant for frmPurchaseOrder
        public const string PurchaseOrder = "PurchaseOrder";

        public const string TYPED = "TYPED";

        //PrimePO related constants
        public const string UpdatePrice = "UpdatePrice";

        //PrimePO Messages
        public const string AddItemMessage = "Item does not exist.Do you want to add item";

        public const int formLeft = 0;
        public const int formTop = 0;

        //Form constants
        public const string FrmSearch = "frmSearch";

        public const string POItemRelationName = "PO-Item-Details";
        public const string MasterPOItemRelationName = "Master-PO-Item-Details";

        //Added By SRT(Gaurav) Date: 19-06-2009
        //Added AutoPO Options
        public const string FORCEDVENDOR = "Forced Vendor";
        public const string PREFEREDVENDOR = "Prefered Vendor";
        public const string LASTVENDOR = "Last Vendor";
        public const string DEFAULTVENDOR = "Default Vendor";
        //End Of Added By SRT(Gaurav)
        public const string ITEMOWNER = "Item Owner";
        //Added By Krishna on 15 April 2011
        public const string IGNOREVENDOR = "Ignore Vendor";
        //Till here added by Krishna

        //Pay Types////Added By Krishna on 24 August 2011
        public const string PayType_Fld_Cash = "Cash";
        public const string PayType_Fld_Check = "Check";
        public const string PayType_Fld_Amex = "American Express";
        public const string PayType_Fld_Visa = "Visa";
        public const string PayType_Fld_MC = "Master Card";
        public const string PayType_Fld_Disc = "Disover";
        public const string PayType_Fld_Debit = "Debit Card";
        public const string PayType_Fld_CashBack = "Cash Back";
        public const string PayType_Fld_Coupon = "Coupon";
        public const string PayType_Fld_EBT = "Elect. Benefit Transfer";
        public const string PayType_Fld_HC = "House Charge";
        //Till here added by Krishna on 24 August 2011

        //PO Order msgbox captions
        public const string PO_Save = "Save";
        public const string PO_SaveClose = "SaveClose";
        public const string PO_Close = "Close";
        public const string PO_DiscardChanges = "DiscardChanges";
        public const string PO_Cancel = "Cancel";

        //constants for CustomerNOtes
        public const string CustomerNotes_tbl = "CustomerNotes";
        public const string CustomerNotes_Fld_ID = "ID";
        public const string CustomerNotes_Fld_CustomerID = "CustomerID";
        public const string CustomerNotes_Fld_Process810 = "Process810";
        public const string CustomerNotes_Fld_UserID = "UserID";
        public const string CustomerNotes_Fld_Notes = "Notes";
        public const string CustomerNotes_Fld_LastUpdatedOn = "LastUpdatedOn";
        public const string CustomerNotes_Fld_IsActive = "IsActive";

        //constants for ItemMonitorCategory
        public const string ItemMonitorCategory_tbl = "ItemMonitorCategory";
        public const string ItemMonitorCategory_Fld_ID = "ID";
        public const string ItemMonitorCategory_Fld_ItemMonCatID = "ItemMonCatID";
        public const string ItemMonitorCategory_Fld_Description = "Description";
        public const string ItemMonitorCategory_Fld_UOM = "UOM";
        public const string ItemMonitorCategory_Fld_DailyLimit = "DailyLimit";
        public const string ItemMonitorCategory_Fld_ThirtyDaysLimit = "ThirtyDaysLimit";
        public const string ItemMonitorCategory_Fld_MaxExempt = "MaxExempt";
        public const string ItemMonitorCategory_Fld_VerificationBy = "VerificationBy";
        public const string ItemMonitorCategory_Fld_LimitPeriodDays = "LimitPeriodDays";
        public const string ItemMonitorCategory_Fld_LimitPeriodQty = "LimitPeriodQty";
        public const string ItemMonitorCategory_Fld_AgeLimit = "AgeLimit"; //added by Manoj 7/11/2013
        public const string ItemMonitorCategory_Fld_IsAgeLimit = "IsAgeLimit";
        public const string ItemMonitorCategory_Fld_ePSE = "ePSE";    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        public const string ItemMonitorCategory_Fld_canOverrideMonitorItem = "canOverrideMonitorItem"; //PRIMEPOS-3166
        public const string ItemMonitorCategoryDetail_tbl = "ItemMonitorCategoryDetail";
        public const string ItemMonitorCategoryDetail_Fld_UnitsPerPackage = "UnitsPerPackage";

        //Constants for ItemDescription Added By Shitaljit on 10/10/2013
        public const string ItemDescription_tbl = "ItemDescription";
        public const string ItemDescription_Fld_ID = "ID";
        public const string ItemDescription_Fld_ItemID = "ItemID";
        public const string ItemDescription_Fld_LanguageId = "LanguageId";
        public const string ItemDescription_Fld_Description = "Description";
        public const string ItemDescription_Fld_UserID = "UserID";

        //Constants for Language
        public const string Language_tbl = "Language";
        public const string Language_Fld_ID = "ID";
        public const string Language_Fld_Name = "Name";
        public const string Language_Fld_Code = "Code";
        public const string Language_Fld_RightToLeft = "RightToLeft";

        #region Sprint-21 - 1272 26-Aug-2015 JY Added constants for DescInEnglish
        public const string DescInEnglish_tbl = "DescInEnglish";
        public const string DescInEnglish_Fld_Id = "Id";
        public const string DescInEnglish_Fld_ColumnName = "ColumnName";
        public const string DescInEnglish_Fld_Description = "Description";
        public const string DescInEnglish_Fld_Translation = "Translation";
        #endregion

        //Added By SRT(Gaurav) Date : 17-Jul-2009
        public const int ItemReorderPeriod = 60;
        //End Of Added By SRT(Gaurav)
        public const string UserMaxDiscountLimit = "UserMaxDiscountLimit";
        public const string StnCloseCashLimit = "StnCloseCashLimit";
        public const string UserMaxTransactionLimit = "UserMaxTransactionLimit";//Added by Ravindra
        public const string UserMaxReturnTransLimit = "UserMaxReturnTransLimit";    //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added
        public const string UserMaxTenderedAmountLimit = "UserMaxTenderedAmountLimit";    //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
        public const string AllowHouseChargePaytype = "AllowHouseChargePaytype";    //Sprint-24 - PRIMEPOS-2290 16-Jan-2017 JY Added        
        //Added By SRT(Gaurav) Date : 21-Jul-2009
        //When the data do not have the processor stored as PCCHARGE, XCHARGE, XLINK
        //value of NoProcessor will get displayed.
        public const string NOPROCESSOR = "N/A";
        //End Of Added By SRT(Gaurav)

        //Added By SRT(Ritesh Parekh) Date : 22-Jul-2009
        public const string PCCHARGE = "PCCHARGE";
        //End Of Added By SRT(Ritesh Parekh)

        //Added By SRT(Ritesh Parekh) Date : 23-Jul-2009
        public const string XCHARGE = "XCHARGE";
        public const string XLINK = "XLINK";
        //End Of Added By SRT(Ritesh Parekh)
        public const string WORLDPAY = "WORLDPAY";
        public const string EdgeExpressCloud = "EDGEEXPRESS CLOUD";
        //Added for HPSPAX Suraj
        public const string HPSPAX = "HPSPAX";

        // Added BY SRT(Abhishek) Date : 23 Sept. 2009
        public const string ItemVendor_Fld_InvBillingPrice = "InvBillingPrice";
        public const string ItemVendor_Fld_UnitPriceBegQuantity = "UnitPriceBegQuantity";
        public const string ItemVendor_Fld_ResalePrice = "ResalePrice";
        public const string ItemVendor_Fld_BaseCharge = "BaseCharge";
        // End Of Added BY SRT(Abhishek) Date : 23 Sept. 2009

        //added by Atul Joshi on 23-10-2010
        public const string ItemVendor_Fld_UnitCostPrice = "UnitCostPrice";

        public const string ItemVendor_Fld_PckSize = "PckSize";
        public const string ItemVendor_Fld_PckQty = "PckQty";
        public const string ItemVendor_Fld_PckUnit = "PckUnit";

        //Added By Ravindra //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
        public const string ItemVendor_Fld_HammacherDeptClass = "HammacherDeptClass";
        public const string ItemVendor_Fld_VendorSalePrice = "VendorSalePrice";
        //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
        public const string ItemVendor_Fld_SaleStartDate = "SaleStartDate";
        public const string ItemVendor_Fld_SaleEndDate = "SaleEndDate";


        //Warning Messages header file
        public const string WarningMessages_tbl = "WarningMessages";
        public const string WarningMessages_Fld_WarningMessageID = "WarningMessageID";
        public const string WarningMessages_Fld_WarningMessage = "WarningMessage";
        public const string WarningMessages_Fld_IsActive = "IsActive";
        public const string WarningMessages_Fld_Process810 = "Process810";

        //Warning Messages detail file
        public const string WarningMessagesDetail_tbl = "WarningMessagesDetail";
        public const string WarningMessagesDetail_Fld_WarningMessageDetailID = "WarningDetailID";
        public const string WarningMessagesDetail_Fld_WarningMessageID = "WarningMessageID";
        public const string WarningMessagesDetail_Fld_RefObjectID = "RefObjectID";
        public const string WarningMessagesDetail_Fld_RefObjectDescription = "Description";
        public const string WarningMessagesDetail_Fld_RefObjectType = "RefObjectType";


        public const string PckUnit_CS = "CS";
        public const string PckUnit_CA = "CA";  //Sprint-21 22-Feb-2016 JY Added CA for case item

        public const string CSItems_Packet_To_Order = "Packet To Order";

        //Log Table Constants
        public const string Log_tbl = "Log";
        public const string Log_Event = "Event";
        public const string Log_LogDate = "LogDate";
        public const string Log_UserID = "UserID";
        public const string Log_LogResult = "LogResult";
        public const string Log_LogMessage = "LogMessage";

        public const string CLPointsRewardTier_tbl = "CL_PointsRewardTier";
        public const string CLPointsRewardTier_Fld_ID = "ID";
        public const string CLPointsRewardTier_Fld_Description = "Description";
        public const string CLPointsRewardTier_Fld_Points = "Points";
        public const string CLPointsRewardTier_Fld_Discount = "Discount";
        public const string CLPointsRewardTier_Fld_RewardPeriod = "RewardPeriod";

        public const string CLCards_tbl = "CL_Cards";
        public const string CLCards_Fld_ID = "ID";
        public const string CLCards_Fld_IsPrepetual = "IsPrepetual";
        public const string CLCards_Fld_RegisterDate = "RegisterDate";
        public const string CLCards_Fld_CLCardID = "CardID";
        public const string CLCards_Fld_Description = "Description";
        public const string CLCards_Fld_CurrentPoints = "CurrentPoints";
        public const string CLCards_Fld_ExpiryDays = "ExpiryDays";
        public const string CLCards_Fld_CustomerID = "CustomerID";
        public const string CLCards_Fld_IsActive = "IsActive";

        public const string CLCoupons_tbl = "CL_Coupons";
        public const string CLCoupons_Fld_ID = "ID";
        public const string CLCoupons_Fld_IsCouponUsed = "IsCouponUsed";
        public const string CLCoupons_Fld_CreatedOn = "CreatedOn";
        public const string CLCoupons_Fld_CLCardID = "CardID";
        public const string CLCoupons_Fld_CreatedBy = "CreatedBy";
        public const string CLCoupons_Fld_Points = "Points";
        public const string CLCoupons_Fld_ExpiryDays = "ExpiryDays";
        public const string CLCoupons_Fld_CouponValue = "CouponValue";
        public const string CLCoupons_Fld_UsedInTransID = "UsedInTransID";
        public const string CLCoupons_Fld_CreatedInTransID = "CreatedInTransID";
        public const string CLCoupons_Fld_IsCouponValueInPercentage = "IsCouponValueInPercentage";
        //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon
        public const string CLCoupons_Fld_CLTierID = "CLTierID";
        //End
        public const string CLPointsAdjustmentLog_tbl = "CL_PointsAdjustmentLog";
        public const string CLPointsAdjustmentLog_Fld_ID = "ID";
        public const string CLPointsAdjustmentLog_Fld_CreatedOn = "CreatedOn";
        public const string CLPointsAdjustmentLog_Fld_CLCardID = "CardID";
        public const string CLPointsAdjustmentLog_Fld_CreatedBy = "CreatedBy";
        public const string CLPointsAdjustmentLog_Fld_OldPoints = "OldPoints";
        public const string CLPointsAdjustmentLog_Fld_NewPoints = "NewPoints";
        public const string CLPointsAdjustmentLog_Fld_Remarks = "Remarks";

        public const string IIAS_Items_Fld_IsEBTItem = "IsEBTItem";

        public const string AutoUpdateAppVer_tbl = "AutoUpdateAppVer";
        public const string AutoUpdateAppVer_Fld_AppName = "AppName";
        public const string AutoUpdateAppVer_Fld_CurrentVersion = "CurrentVersion";
        public const string AutoUpdateAppVer_Fld_LastUpdatedAt = "LastUpdatedAt";
        public const string AutoUpdateAppVer_Fld_StationId = "StationId";
        public const string AutoUpdateAppVer_Fld_UpdateType = "UpdateType";
        public const string AutoUpdateAppVer_Fld_Path = "Path";
        public const string AutoUpdateAppVer_Fld_MechineName = "MechineName";

        //Added By Shitaljit(QuicSolv) on 6 Oct 2011
        //Constant for Notes Table

        public const string Notes_tbl = "Notes";

        public const string Notes_Fld_NoteId = "NoteId";
        public const string Notes_Fld_EntityId = "EntityId";
        public const string Notes_Fld_EntityType = "EntityType";
        public const string Notes_Fld_Note = "Note";
        public const string Notes_Fld_CreatedBy = "CreatedBy";
        public const string Notes_Fld_CreatedDate = "CreatedDate";
        public const string Notes_Fld_UpdatedBy = "UpdatedBy";
        public const string Notes_Fld_UpdatedDate = "UpdatedDate";
        public const string Notes_Fld_POPUPMSG = "POPUPMSG";

        //Added By Shitaljit(QuicSolv) on 1 May 2012
        //constants for pos transaction Sign Log file
        public const string POSTransSignLog_tbl = "POSTransSignLog";
        public const string POSTransSignLog_Fld_SignLogID = "SignLogID";
        public const string POSTransSignLog_Fld_POSTransId = "POSTransId";
        public const string POSTransSignLog_Fld_SignContext = "SignContext";
        public const string POSTransSignLog_Fld_SignContextData = "SignContextData";
        public const string POSTransSignLog_Fld_SignDataBinary = "SignDataBinary";
        public const string POSTransSignLog_Fld_SignDataText = "SignDataText";
        public const string POSTransSignLog_Fld_CustomerIDType = "CustomerIDType";
        public const string POSTransSignLog_Fld_CustomerIDDetail = "CustomerIDDetail";
        public const string POSTransSignLog_Fld_TransDetailID = "TransDetailID";  //Sprint-23 - PRIMEPOS-2029 29-Apr-2016 JY Added

        //Item Combo Pricing file
        public const string ItemComboPricing_tbl = "ItemComboPricing";
        public const string ItemComboPricing_Fld_Id = "Id";
        public const string ItemComboPricing_Fld_Description = "Description";
        public const string ItemComboPricing_Fld_ForceGroupPricing = "ForceGroupPricing";
        public const string ItemComboPricing_Fld_ComboItemPrice = "ComboItemPrice";
        public const string ItemComboPricing_Fld_MinComboItems = "MinComboItems";
        public const string ItemComboPricing_Fld_IsActive = "IsActive";
        public const string ItemComboPricing_Fld_MaxComboItems = "MaxComboItems"; //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

        //Item Combo Pricing detail file
        public const string ItemComboPricingDetail_tbl = "ItemComboPricingDetail";
        public const string ItemComboPricingDetail_Fld_Id = "Id";
        public const string ItemComboPricingDetail_Fld_ItemComboPricingId = "ItemComboPricingId";
        public const string ItemComboPricingDetail_Fld_QTY = "Qty";
        public const string ItemComboPricingDetail_Fld_ItemID = "ItemID";
        public const string ItemComboPricingDetail_Fld_SalePrice = "SalePrice";

        //Color Scheme For View POS Trans  table
        public const string ColorSchemeForViewPOSTrans_tbl = "ColorSchemeForViewPOSTrans";
        public const string ColorSchemeForViewPOSTrans_Fld_FromAmount = "FromAmount";
        public const string ColorSchemeForViewPOSTrans_Fld_ToAmount = "ToAmount";
        public const string ColorSchemeForViewPOSTrans_Fld_BackColor = "BackColor";
        public const string ColorSchemeForViewPOSTrans_Fld_ForeColor = "ForeColor";
        public const string ColorSchemeForViewPOSTrans_Fld_UserID = "UserID";
        public const string ColorSchemeForViewPOSTrans_Fld_ID = "ID";


        public const string Coupon_Fld_CouponCode = "CouponCode";
        public const string Coupon_Fld_UserID = "UserID";
        public const string Coupon_Fld_DiscountPerc = "DiscountPerc";
        public const string Coupon_Fld_CreatedDate = "CreatedDate";
        public const string Coupon_Fld_StartDate = "StartDate";
        public const string Coupon_Fld_EndDate = "EndDate";
        public const string Coupon_Fld_IsCouponInPercent = "IsCouponInPercent";
        public const string Coupon_Fld_CouponID = "CouponID";
        public const string Coupon_tbl = "Coupon";
        public const string Coupon_Fld_CouponDesc = "CouponDesc";   //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added

        public const string Util_UserOptionDetailRights_Fld_UserID = "UserID";
        public const string Util_UserOptionDetailRights_Fld_DetailId = "DetailId";
        public const string Util_UserOptionDetailRights_Fld_ScreenID = "ScreenID";
        public const string Util_UserOptionDetailRights_Fld_ModuleID = "ModuleID";
        public const string Util_UserOptionDetailRights_Fld_PermissionId = "PermissionId";
        public const string Util_UserOptionDetailRights_Fld_isAllowed = "isAllowed";
        public const string Util_UserOptionDetailRights_tbl = "Util_UserOptionDetailRights";
        public const string Util_UserOptionDetailRights_Fld_ID = "ID";

        public const string ItemTaxTable_IDColumnName = "Id";
        public const string ItemTaxTable_EntityIdColumnName = "EntityID";
        public const string ItemTaxTable_EntityTypeColumnName = "EntityType";
        public const string ItemTaxTable_TaxIdColumnName = "TaxID";
        public const string ItemTaxTableName = "ItemTax";

        //Added By Rohit Nair on 3/22/2016
        public const string CCCustomerTokInfo_tbl = "CCCustomerTokInfo";
        public const string CCCustomerTokInfo__Fld_EntryID = "EntryID";
        public const string CCCustomerTokInfo__Fld_CustomerID = "CustomerID";
        public const string CCCustomerTokInfo__Fld_CardType = "CardType";
        public const string CCCustomerTokInfo__Fld_Last4 = "Last4";
        public const string CCCustomerTokInfo__Fld_ProfiedID = "ProfiledID";
        public const string CCCustomerTokInfo__Fld_Processor = "Processor";
        public const string CCCustomerTokInfo__Fld_EntryType = "EntryType";
        public const string CCCustomerTokInfo__Fld_TokenDate = "TokenDate"; //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
        public const string CCCustomerTokInfo__Fld_ExpDate = "ExpDate"; //PRIMEPOS-2612 04-Dec-2018 JY Added
        public const string CCCustomerTokInfo__Fld_CardAlias = "CardAlias"; //PRIMEPOS-2634 30-Jan-2019 JY Added
        public const string CCCustomerTokInfo__Fld_PreferenceId = "PreferenceId";   //PRIMEPOS-2635 31-Jan-2019 JY Added

        #region Sprint-18 - 2090 07-Oct-2014 JY Added CL_TransDetail table constants
        public const string CLTransDetail_tbl = "CL_TransDetail";
        public const string CLTransDetail_Fld_ID = "ID";
        public const string CLTransDetail_Fld_TransID = "TransID";
        public const string CLTransDetail_Fld_CardID = "CardID";
        public const string CLTransDetail_Fld_CurrentPoints = "CurrentPoints";
        public const string CLTransDetail_Fld_PointsAcquired = "PointsAcquired";
        public const string CLTransDetail_Fld_RunningTotalPoints = "RunningTotalPoints";
        public const string CLTransDetail_Fld_ActionType = "ActionType";
        public const string CLTransDetail_Fld_TransDate = "TransDate";
        #endregion

        #region PRIMEPOS-2227 05-May-2017 JY Added
        public const string MerchantConfig_CONFIGFILE = "MerchantConfig.xml";
        public const string MerchantConfig_tbl = "MerchantConfig";
        //public const string MerchantConfig_Fld_STATIONID = "STATIONID";
        public const string MerchantConfig_Fld_USER_ID = "USER_ID";
        public const string MerchantConfig_Fld_CARDINFO = "CARDINFO";
        public const string MerchantConfig_Fld_Merch_Num = "Merch_Num";
        public const string MerchantConfig_Fld_Processor_Id = "Processor_Id";
        public const string MerchantConfig_Fld_PAYMENT_SERVER = "PAYMENT_SERVER";
        public const string MerchantConfig_Fld_PORT_NO = "PORT_NO";
        public const string MerchantConfig_Fld_LICENSEID = "LICENSEID";
        public const string MerchantConfig_Fld_SITEID = "SITEID";
        public const string MerchantConfig_Fld_DEVICEID = "DEVICEID";
        public const string MerchantConfig_Fld_PAYMENTCLIENT = "PAYMENTCLIENT";
        public const string MerchantConfig_Fld_PAYMENTRESULTFILE = "PAYMENTRESULTFILE";
        public const string MerchantConfig_Fld_APPLICATION_NAME = "APPLICATIONNAME";
        public const string MerchantConfig_Fld_XCCLIENTUITITLE = "XCCLIENTUITITLE";
        public const string MerchantConfig_Fld_URL = "URL";
        public const string MerchantConfig_Fld_VCBIN = "VCBIN";
        public const string MerchantConfig_Fld_MCBIN = "MCBIN";
        public const string MerchantConfig_Fld_CARD = "CARD";
        #endregion

        #region Sprint-22 - PRIMEPOS-2245 15-Oct-2015 JY Added
        public const string SystemInfo_tbl = "SystemInfo";
        public const string SystemInfo_Fld_Id = "Id";
        public const string SystemInfo_Fld_PharmNo = "PharmNo";
        public const string SystemInfo_Fld_OSName = "OSName";
        public const string SystemInfo_Fld_Version = "Version";
        public const string SystemInfo_Fld_SystemName = "SystemName";
        public const string SystemInfo_Fld_SystemManufacturer = "SystemManufacturer";
        public const string SystemInfo_Fld_SystemModel = "SystemModel";
        public const string SystemInfo_Fld_SystemType = "SystemType";
        public const string SystemInfo_Fld_Processor = "Processor";
        public const string SystemInfo_Fld_RAM = "RAM";
        public const string SystemInfo_Fld_DriveInfo = "DriveInfo";
        public const string SystemInfo_Fld_UpdatedOn = "UpdatedOn";
        public const string SystemInfo_Fld_Status = "Status";
        #endregion

        #region Sprint-23 - PRIMEPOS-2029 19-Apr-2016 JY Added
        public const string ItemMonitorTransDetail_tbl = "ItemMonitorTransDetail";
        public const string ItemMonitorTransDetail_Fld_Id = "ID";
        public const string ItemMonitorTransDetail_Fld_TransDetailID = "TransDetailID";
        public const string ItemMonitorTransDetail_Fld_ItemMonCatID = "ItemMonCatID";
        public const string ItemMonitorTransDetail_Fld_UOM = "UOM";
        public const string ItemMonitorTransDetail_Fld_UnitsPerPackage = "UnitsPerPackage";
        public const string ItemMonitorTransDetail_Fld_SentToNplex = "SentToNplex";
        public const string ItemMonitorTransDetail_Fld_PostSaleInd = "PostSaleInd";
        public const string ItemMonitorTransDetail_Fld_pseTrxId = "pseTrxId";   //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
        #endregion

        #region PRIMEPOS-2339 04-Oct-2016 JY Added
        public const string InsSigTrans_tbl = "InsSigTrans";
        public const string InsSigTrans_Fld_Id = "ID";
        public const string InsSigTrans_Fld_TransID = "TransID";
        public const string InsSigTrans_Fld_PatientNo = "PatientNo";
        public const string InsSigTrans_Fld_InsType = "InsType";
        public const string InsSigTrans_Fld_TransData = "TransData";
        public const string InsSigTrans_Fld_TransSigData = "TransSigData";
        public const string InsSigTrans_Fld_CounselingReq = "CounselingReq";
        public const string InsSigTrans_Fld_SigType = "SigType";
        public const string InsSigTrans_Fld_BinarySign = "BinarySign";
        public const string InsSigTrans_Fld_PrivacyPatAccept = "PrivacyPatAccept";
        public const string InsSigTrans_Fld_PrivacyText = "PrivacyText";
        public const string InsSigTrans_Fld_PrivacySig = "PrivacySig";
        public const string InsSigTrans_Fld_PrivacySigType = "PrivacySigType";
        public const string InsSigTrans_Fld_PrivacyBinarySign = "PrivacyBinarySign";
        #endregion

        #region Sprint-24 - PRIMEPOS-2364 13-Jan-2017 JY Added
        public const string TaxCodes_With_NoTax = "TaxCodesWithNoTax";
        public const string TaxCodes_NoTax_Desc = "No Tax";
        public const string TaxCodes_NoTaxCode = "NoTax";
        #endregion

        #region Sprint-25 - PRIMEPOS-2379 09-Mar-2017 JY Added
        public const string PSE_Items_tbl = "PSE_Items";
        public const string PSE_Items_Fld_Id = "Id";
        public const string PSE_Items_Fld_ProductId = "ProductId";
        public const string PSE_Items_Fld_ProductName = "ProductName";
        public const string PSE_Items_Fld_ProductNDC = "ProductNDC";
        public const string PSE_Items_Fld_ProductGrams = "ProductGrams";
        public const string PSE_Items_Fld_ProductPillCnt = "ProductPillCnt";
        public const string PSE_Items_Fld_CreatedBy = "CreatedBy";
        public const string PSE_Items_Fld_CreatedOn = "CreatedOn";
        public const string PSE_Items_Fld_UpdatedBy = "UpdatedBy";
        public const string PSE_Items_Fld_UpdatedOn = "UpdatedOn";
        public const string PSE_Items_Fld_IsActive = "IsActive";
        public const string PSE_Items_Fld_RecordStatus = "RecordStatus";
        #endregion

        #region Sprint-24 - PRIMEPOS-2299 09-Dec-2016 JY Added
        public const string DeptComboPricing_tbl = "DeptComboPricing";
        public const string DeptComboPricing_Fld_Id = "ID";
        public const string DeptComboPricing_Fld_DeptComboDesc = "DeptComboDesc";
        public const string DeptComboPricing_Fld_ForceGroupPricing = "ForceGroupPricing";
        public const string DeptComboPricing_Fld_DeptComboPrice = "DeptComboPrice";
        public const string DeptComboPricing_Fld_MinComboItems = "MinComboItems";
        public const string DeptComboPricing_Fld_CreatedBy = "CreatedBy";
        public const string DeptComboPricing_Fld_CreatedOn = "CreatedOn";
        public const string DeptComboPricing_Fld_LastUpdatedBy = "LastUpdatedBy";
        public const string DeptComboPricing_Fld_LastUpdatedOn = "LastUpdatedOn";
        #endregion

        #region PRIMEPOS-2613 26-Dec-2018 JY Added
        public const string FMessage_tbl = "FMessage";
        public const string FMessage_Fld_RecID = "RecID";
        public const string FMessage_Fld_MessageCode = "MessageCode";
        public const string FMessage_Fld_Message = "Message";
        public const string FMessage_Fld_MessageSub = "MessageSub";
        public const string FMessage_Fld_MessageCatId = "MessageCatId";
        public const string FMessage_Fld_MessageTypeId = "MessageTypeId";
        public const string FMessage_Fld_CreatedBy = "CreatedBy";
        public const string FMessage_Fld_CreatedOn = "CreatedOn";
        public const string FMessage_Fld_UpdatedBy = "UpdatedBy";
        public const string FMessage_Fld_UpdatedOn = "UpdatedOn";
        #endregion

        public const string MMSSearch = "MMSSearch";    //PRIMEPOS-2671 18-Apr-2019 JY Added    

        #region NileshJ - SoluTran - PRIMEPOS-2663
        public const string s3_TransID = "S3TransID";
        public const string s3_PurAmount = "S3PurAmount";
        public const string s3_TaxAmount = "S3TaxAmount";
        public const string s3_DiscountAmount = "S3DiscountAmount";
        public const int s3Card_Len_Seventeen = 17; 
        public const int s3Card_Len_Sixteen = 16; //PRIMEPOS-3488
        public const int s3Card_Len_TwentyOne = 21;
        public const int s3Card_Len_SeventyEight = 78; //PRIMEPOS-3488
        public const int s3Card_Len_SixtyEight = 68; //PRIMEPOS-3488
        public const int s3Card_CVV_len_Three = 3; //PRIMEPOS-3488
        public const int s3Card_Len_thirteen = 13; //PRIMEPOS-3488
        public const int s3Card_Len_FiftyThree = 53; //PRIMEPOS-3488
        public const string s3Card_Manual_Pos_Code_data = "31"; //PRIMEPOS-3488
        public const string s3Card_s3_Pos_Code_data = "21"; //PRIMEPOS-3488
        public const string s3Card_s3m_Pos_Code_data = "22"; //PRIMEPOS-3488
        #endregion

        #region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019
        // StoreCredit Header
        public const string StoreCredit_tbl = "StoreCredit";
        public const string StoreCredit_ID = "StoreCreditID";
        public const string StoreCredit_CustomerID = "CustomerID";
        public const string StoreCredit_CreditAmt = "CreditAmt";
        public const string StoreCredit_LastUpdated = "LastUpdated";
        public const string StoreCredit_LastUpdatedBy = "LastUpdatedBy";
        public const string StoreCredit_ExpiresOn = "ExpiresOn";

        // StoreCredit Details 
        public const string StoreCreditDetails_tbl = "StoreCreditDetails";
        public const string StoreCreditDetails_ID = "StoreCreditDetailsID";
        public const string StoreCreditDetails_StoreCreditID = "StoreCreditID";
        public const string StoreCreditDetails_CustomerID = "CustomerID";
        public const string StoreCreditDetails_TransactionID = "TransactionID";
        public const string StoreCreditDetails_CreditAmt = "CreditAmt";
        public const string StoreCreditDetails_UpdatedOn = "UpdatedOn";
        public const string StoreCreditDetails_UpdatedBy = "UpdatedBy";
        public const string StoreCreditDetails_RemainingAmount = "RemainingAmount";
        #endregion

        #region PRIMEPOS-2774 13-Jan-2020 JY Added
        public const string SettingDetail_S3FTPURL = "S3FTPURL";
        public const string SettingDetail_S3FTPPort = "S3FTPPort";
        public const string SettingDetail_S3FTP = "S3FTP";
        public const string SettingDetail_S3FTPUserId = "S3FTPUserId";
        public const string SettingDetail_S3FTPPassword = "S3FTPPassword";
        public const string SettingDetail_S3Frequency = "S3Frequency";
        public const string SettingDetail_S3LastUploadDateOnFTP = "S3LastUploadDateOnFTP";
        public const string SettingDetail_S3FileName = "S3FileName";
        public const string SettingDetail_S3FTPFolderPath = "S3FTPFolderPath";
        public const string SettingDetail_S3UploadProcessDate = "S3UploadProcessDate";
        #endregion

        #region PRIMEPOS-3243
        public const string SettingDetail_LastUpdateInsSigTrans = "LastUpdateInsSigTrans";
        public const string SettingDetail_FrequencyIntervalInsSigTrans = "FrequencyIntervalInsSigTrans";
        public const string SettingDetail_LastUpdatePOSTransactionRxDetail = "LastUpdatePOSTransactionRxDetail";
        public const string SettingDetail_FrequencyIntervalPOSTransactionRxDetail = "FrequencyIntervalPOSTransactionRxDetail";
        public const string SettingDetail_LastUpdateReturnTransDetailId = "LastUpdateReturnTransDetailId";
        public const string SettingDetail_FrequencyIntervalReturnTransDetailId = "FrequencyIntervalReturnTransDetailId";
        public const string SettingDetail_LastUpdateTrimItemID = "LastUpdateTrimItemID";
        public const string SettingDetail_FrequencyIntervalTrimItemID = "FrequencyIntervalTrimItemID";
        //public const string SettingDetail_LastUpdateSign = "LastUpdateSign";
        //public const string SettingDetail_FrequencyIntervalSign = "FrequencyIntervalSign";
        public const string SettingDetail_LastUpdateMissingTransDetInPrimeRx = "LastUpdateMissingTransDetInPrimeRx";
        public const string SettingDetail_FrequencyIntervalMissingTransDetInPrimeRx = "FrequencyIntervalMissingTransDetInPrimeRx";
        public const string SettingDetail_LastUpdateBlankSignWithSamePatientsSign = "LastUpdateBlankSignWithSamePatientsSign";
        public const string SettingDetail_FrequencyIntervalBlankSignWithSamePatientsSign = "FrequencyIntervalBlankSignWithSamePatientsSign";
        #endregion

        #region PRIMEPOS-2761
        public const string POSTransPayment_Fld_TicketNumber = "TicketNumber";
        public const string RxTransactionData_tbl = "RxTransmission_Log";
        public const string RxTransmissionData_Fld_RxTransNo = "RxTransNo";
        public const string RxTransmissionData_Fld_PatientID = "PatientID";
        public const string RxTransmissionData_Fld_RxNo = "RxNo";
        public const string RxTransmissionData_Fld_Nrefill = "Nrefill";
        public const string RxTransmissionData_Fld_PickedUp = "PickedUp";
        public const string RxTransmissionData_Fld_PickUpPOS = "PickUpPOS";
        public const string RxTransmissionData_Fld_TransID = "TransID";
        public const string RxTransmissionData_Fld_IsDelivery = "IsDelivery";
        public const string RxTransmissionData_Fld_IsRxSync = "IsRxSync";
        public const string RxTransmissionData_Fld_TransAmount = "TransAmount";
        public const string RxTransmissionData_Fld_StationID = "StationID";
        public const string RxTransmissionData_Fld_UserID = "UserID";
        public const string RxTransmissionData_Fld_CreatedDate = "CreatedDate";
        public const string RxTransmissionData_Fld_ModifiedBy = "ModifiedBy";
        public const string RxTransmissionData_Fld_ModifiedDate = "ModifiedDate";
        public const string RxTransmissionData_Fld_TransDateTime = "TransDateTime";
        public const string RxTransmissionData_Fld_ConsentTextID = "ConsentTextID";
        public const string RxTransmissionData_Fld_ConsentTypeID = "ConsentTypeID";
        public const string RxTransmissionData_Fld_ConsentStatusID = "ConsentStatusID";
        public const string RxTransmissionData_Fld_ConsentCaptureDate = "ConsentCaptureDate";
        public const string RxTransmissionData_Fld_ConsentEffectiveDate = "ConsentEffectiveDate";
        public const string RxTransmissionData_Fld_ConsentEndDate = "ConsentEndDate";
        public const string RxTransmissionData_Fld_PatConsentRelationID = "RelationID";
        public const string RxTransmissionData_Fld_SigneeName = "SigneeName";
        public const string RxTransmissionData_Fld_SignatureData = "SignatureData";
        public const string RxTransmissionData_Fld_PickUpDate = "PickUpDate";
        public const string RxTransmissionData_Fld_CopayPaid = "CopayPaid";

        public const string RxTransmissionData_Fld_PackDATESIGNED = "PackDATESIGNED";
        public const string RxTransmissionData_Fld_PackPATACCEPT = "PackPATACCEPT";
        public const string RxTransmissionData_Fld_PackPRIVACYTEXT = "PackPRIVACYTEXT";
        public const string RxTransmissionData_Fld_PackPRIVACYSIG = "PackPRIVACYSIG";
        public const string RxTransmissionData_Fld_PackSIGTYPE = "PackSIGTYPE";
        public const string RxTransmissionData_Fld_PackBinarySign = "PackBinarySign";
        public const string RxTransmissionData_Fld_PackEventType = "PackEventType";
        public const string RxTransmissionData_Fld_DeliveryStatus = "DeliveryStatus";
        public const string RxTransmissionData_Fld_ConsentSourceID = "ConsentSourceID";//PRIMEPOS-2866
        public const string RxTransmissionData_Fld_IsConsentSkip = "IsConsentSkip";//PRIMEPOS-2866
        public const string RxTransmissionData_Fld_PartialFillNo = "PartialFillNo"; //PRIMEPOS-2982

        #endregion

        public const string SettingDetail_TagSolutran = "TagSolutran";  //PRIMEPOS-2836 21-Apr-2020 JY Added

        #region PRIMEPOS-2842 05-May-2020 JY Added
        public const string SettingDetail_EnablePromptForZipCode = "EnablePromptForZipCode";
        public const string SettingDetail_RequiredDriverForVerifonePads = "RequiredDriverForVerifonePads";
        public const string SettingDetail_Driver = "Driver";
        public const string SettingDetail_ComPort = "ComPort";
        public const string SettingDetail_DataBits = "DataBits";
        public const string SettingDetail_Parity = "Parity";
        public const string SettingDetail_StopBits = "StopBits";
        public const string SettingDetail_Handshake = "Handshake";
        public const string SettingDetail_BaudRate = "BaudRate";
        public const string SettingDetail_TestMode = "TestMode";
        public const string SettingDetail_AllowPartialApprovals = "AllowPartialApprovals";
        public const string SettingDetail_ConfirmOriginalAmount = "ConfirmOriginalAmount";
        public const string SettingDetail_CheckForDuplicateCreditCardTransactions = "CheckForDuplicateCreditCardTransactions";
        public const string SettingDetail_VantivsCashBackFeature = "VantivsCashBackFeature";
        public const string SettingDetail_PromptForCreditCardCVVNumberForKeyedCardTransactions = "PromptForCreditCardCVVNumberForKeyedCardTransactions";
        public const string SettingDetail_EnableDebitSale = "EnableDebitSale";
        public const string SettingDetail_EnableDebitRefunds = "EnableDebitRefunds";
        public const string SettingDetail_EnableEBTRefunds = "EnableEBTRefunds";
        public const string SettingDetail_EnableGiftCards = "EnableGiftCards";
        public const string SettingDetail_EnableEBTFoodStamp = "EnableEBTFoodStamp";
        public const string SettingDetail_EnableEBTCashBenefit = "EnableEBTCashBenefit";
        public const string SettingDetail_EnableEMVProcessing = "EnableEMVProcessing";
        public const string SettingDetail_FSAHRACardProcessing = "FSAHRACardProcessing";
        public const string SettingDetail_TippingDoesNotApplyToPharmacyBusiness = "TippingDoesNotApplyToPharmacyBusiness";
        public const string SettingDetail_ManualCreditCardEntry = "ManualCreditCardEntry";
        public const string SettingDetail_NearFieldCommunication = "NearFieldCommunication";
        public const string SettingDetail_TPWelcomeMessage = "TPWelcomeMessage";
        public const string SettingDetail_TPMessage = "TPMessage";
        #endregion


        public const string SettingDetail_QuickChip = "QuickChip"; //PRIMEPOS-3500
        public const string SettingDetail_CheckForPreReadId = "CheckForPreReadId"; //PRIMEPOS-3500
        public const string SettingDetail_QuickChipDataLifetime = "QuickChipDataLifetime"; //PRIMEPOS-3500
        public const string SettingDetail_ThresholdAmount = "ThresholdAmount"; //PRIMEPOS-3500
        public const string SettingDetail_ContactlessMsdEntryAllowed = "ContactlessMsdEntryAllowed"; //PRIMEPOS-3500
        public const string SettingDetail_DisplayCustomAidScreen = "DisplayCustomAidScreen"; //PRIMEPOS-3500
        public const string SettingDetail_Unattended = "Unattended"; //PRIMEPOS-3500
        public const string SettingDetail_ReturnResponseBeforeCardRemoval = "ReturnResponseBeforeCardRemoval"; //PRIMEPOS-3535

        #region PRIMEPOS-3024 08-Nov-2021 JY Added
        public const string SettingDetail_IPTerminalId = "IPTerminalId";
        public const string SettingDetail_IPTerminalType = "IPTerminalType";
        public const string SettingDetail_IPDriver = "IPDriver";
        public const string SettingDetail_IPPort = "IPPort";
        public const string SettingDetail_IPMessage = "IPMessage";
        public const string SettingDetail_IPTransactionAmountLimit = "IPTransactionAmountLimit";
        public const string SettingDetail_IPisContactlessEmvEntryAllowed = "IPisContactlessEmvEntryAllowed";
        public const string SettingDetail_IPisContactlessMsdEntryAllowed = "IPisContactlessMsdEntryAllowed";
        public const string SettingDetail_IPisManualEntryAllowed = "IPisManualEntryAllowed";
        public const string SettingDetail_IPisDisplayCustomAidScreen = "IPisDisplayCustomAidScreen"; //PRIMEPOS-3266
        public const string SettingDetail_IPisConfirmTotalAmountScreenDisplayed = "IPisConfirmTotalAmountScreenDisplayed"; //PRIMEPOS-3266
        public const string SettingDetail_IPisConfirmCreditSurchargeScreenDisplayed = "IPisConfirmCreditSurchargeScreenDisplayed"; //PRIMEPOS-3266
        #endregion

        public const string SettingDetail_AutoSearchPrimeRxPatient = "AutoSearchPrimeRxPatient";  //PRIMEPOS-2845 14-May-2020 JY Added

        public const string SettingDetail_ClientID = "client_id";  //PRIMEPOS-2841 Sajid/Nilesh
        public const string SettingDetail_SecretKey = "secret_key";  //PRIMEPOS-2841 Sajid/Nilesh
        public const string SettingDetail_URL = "PrimeRxPayURL";  //PRIMEPOS-2841 Sajid/Nilesh


        #region PRIMEPOS-2841
        public const string Util_Company_Info = "Util_Company_Info";
        public const string PHNPINO = "PHNPINO";
        #endregion

        #region PRIMEPOS-CONSENT SAJID DHUKKA
        public const string ConsentTransmissionData_tbl = "ConsentTransmissionLog";
        public const string ConsentTransmissionData_Fld_ConsentLogId = "ConsentLogId";
        public const string ConsentTransmissionData_Fld_ConsentSourceID = "ConsentSourceID";
        public const string ConsentTransmissionData_Fld_ConsentTypeId = "ConsentTypeId";
        public const string ConsentTransmissionData_Fld_ConsentTextId = "ConsentTextId";
        public const string ConsentTransmissionData_Fld_ConsentStatusId = "ConsentStatusId";
        public const string ConsentTransmissionData_Fld_ConsentRelationId = "ConsentRelationId";
        public const string ConsentTransmissionData_Fld_PatientNo = "PatientNo";
        public const string ConsentTransmissionData_Fld_RxNo = "RxNo";
        public const string ConsentTransmissionData_Fld_Nrefill = "Nrefill";
        public const string ConsentTransmissionData_Fld_ConsentCaptureDate = "ConsentCaptureDate";
        public const string ConsentTransmissionData_Fld_ConsentExpiryDate = "ConsentExpiryDate";
        public const string ConsentTransmissionData_Fld_SigneeName = "SigneeName";
        public const string ConsentTransmissionData_Fld_SignatureData = "SignatureData";
        #endregion 

        public const string SettingDetail_ProceedROATransWithHCaccNotLinked = "ProceedROATransWithHCaccNotLinked";  //PRIMEPOS-2570 17-Aug-2020 JY Added

        #region PRIMEPOS-2875 Added for Pointy 
        public const string SettingDetail_MMSKEY = "MMSKey";
        public const string SettingDetail_RetailerKey = "RetailerKey";
        public const string SettingDetail_Url = "URL";
        public const string SettingDetail_SupportMail = "SupportMail";
        public const string SettingDetail_MMSEmailID = "MMSEmailID";
        public const string SettingDetail_MMSEmailPass = "MMSEmailPass";
        public const string SettingDetail_EnableSSL = "EnableSSL";
        public const string SettingDetail_MMSEmailPort = "MMSEmailPort";
        public const string SettingDetail_MMSEmailServer = "MMSEmailServer";
        public const string SettingDetail_MMSEmailSig = "MMSEmailSig";
        public const string SettingDetail_PointyUTM = "PointyUTM";//PRIMEPOS-3005
        #endregion

        public const string SettingDetail_DefaultPaytype = "DefaultPaytype";  //PRIMEPOS-2512 02-Oct-2020 JY Added
        public const string SettingDetail_RestrictSignatureLineAndWordingOnReceipt = "RestrictSignatureLineAndWordingOnReceipt";    //PRIMEPOS-2910 29-Oct-2020 JY Added
        public const string SettingDetail_RxInsuranceToBeTaxed = "RxInsuranceToBeTaxed";    //PRIMEPOS-2924 02-Dec-2020 JY Added
        public const string SettingDetail_PatientCounselingPrompt = "PatientCounselingPrompt";  //PRIMEPOS-2461 02-Mar-2021 JY Added
        public const string SettingDetail_PrintCompanyLogo = "PrintCompanyLogo";  //PRIMEPOS-2386 26-Feb-2021 JY Added
        public const string SettingDetail_RunVantivSignatureFix = "RunVantivSignatureFix";  //PRIMEPOS-3232N
        public const string SettingDetail_CompanyLogoFileName = "CompanyLogoFileName";  //PRIMEPOS-2386 26-Feb-2021 JY Added
        public const string SettingDetail_SchedularMachine = "SchedularMachine";    //PRIMEPOS-2485 19-Mar-2021 JY Added
        public const string SettingDetail_SchedulerUser = "SchedulerUser";    //PRIMEPOS-2485 05-Apr-2021 JY Added
        public const string SettingDetail_AzureADMiddleTierUrl = "AzureADMiddleTierUrl";    //PRIMEPOS-2989 13-Aug-2021 JY Added
        public const string SettingDetail_ApplicationLaunchContext = "ApplicationLaunchContext";    //PRIMEPOS-2993 24-Aug-2021 JY Added

        #region PRIMEPOS-2999 09-Sep-2021 JY Added
        public const string SettingDetail_NPlexURL = "NPlexURL";
        public const string SettingDetail_NPlexTokenURL = "NPlexTokenURL";
        public const string SettingDetail_NPlexClientID = "NPlexClientID";
        public const string SettingDetail_NPlexClientSecret = "NPlexClientSecret";
        #endregion
        public const string SettingDetail_RxTaxPolicy = "RxTaxPolicy";  //PRIMEPOS-3053 04-Feb-2022 JY Added
        public const string SettingDetail_NotifyRefrigeratedMedication = "NotifyRefrigeratedMedication";    //PRIMEPOS-2651 08-Apr-2022 JY Added
        public const string SettingDetail_RestrictMultipleClockIn = "RestrictMultipleClockIn";    //PRIMEPOS-2790 18-Apr-2022 JY Added
        public const string SettingDetail_TransactionFeeApplicableFor = "TransactionFeeApplicableFor";  //PRIMEPOS-3115 11-Jul-2022 JY Added
        public const string SettingDetail_ResetPwdForceUserToChangePwd = "ResetPwdForceUserToChangePwd";    //PRIMEPOS-3129 22-Aug-2022 JY Added
        public const string SettingDetail_PromptToSaveCCToken = "PromptToSaveCCToken";  //PRIMEPOS-3145 28-Sep-2022 JY Added

        #region PRIMEPOS-3164 01-Nov-2022 JY Added
        public const string SettingDetail_TranslatorAPIkey = "TranslatorAPIkey";
        public const string SettingDetail_TranslatorAPIEndPoint = "TranslatorAPIEndPoint";
        public const string SettingDetail_TranslatorAPILocation = "TranslatorAPILocation";
        public const string SettingDetail_TranslatorAPIRoute = "TranslatorAPIRoute";
        #endregion

        public const string SettingDetail_PatientsSubCategories = "PatientsSubCategories";  //PRIMEPOS-3157 28-Nov-2022 JY Added

        #region PRIMEPOS-2808
        public const string NoSaleTableName = "NoSale";
        public const string NoSaleTable_Fld_Id = "Id";
        public const string NoSaleTable_Fld_UserId = "UserId";
        public const string NoSaleTable_Fld_StationId = "StationId";
        public const string NoSaleTable_Fld_DrawerOpenedDate = "DrawerOpenedDate";
        #endregion

        public const string TransHeader_Fld_IsCustomerDriven = "IsPaymentPending";//2915
        public const string POSTransPayment_Fld_PrimeRxPayTransID = "PrimeRxPayTransID";//2915
        public const string POSTransPayment_OnHold_tbl = "POSTransPayment_OnHold";//2915
        public const string POSTransPayment_Fld_ApprovedAmount = "ApprovedAmount";//2915
        public const string POSTransPayment_Fld_Status = "Status";//2915
        public const string POSTransPayment_Fld_TransactionProcessingMode = "TransactionProcessingMode";//2915
        public const string POSTransPayment_Fld_Email = "Email";//2915
        public const string POSTransPayment_Fld_Mobile = "Mobile";//2915

        public const string PosTransPayment_Status_InProgress = "In Progress";//PRIMEPOS-2915
        public const string PosTransPayment_Status_Completed = "Completed";//PRIMEPOS-2915
        public const string PosTransPayment_Status_Expired = "Expired";//PRIMEPOS-2915
        public const string PosTransPayment_Status_Cancelled = "Cancelled";//PRIMEPOS-2915
        public const string PosTransPayment_Status_PartialApproved = "Partial Approved";//PRIMEPOS-3343
        public const string PosTransPayment_Status_OverrideHousechargeLimitUser = "OverrideHousechargeLimitUser"; //PRIMEPOS-2402 09-Jul-2021 JY Added
        public const string PosTransPayment_Status_MaxTenderedAmountOverrideUser = "MaxTenderedAmountOverrideUser"; //PRIMEPOS-2402 20-Jul-2021 JY Added        

        #region   PRIMEPOS-2485 15-Mar-2021 JY Added
        public const string ScheduledTasks_tbl = "ScheduledTasks";
        public const string ScheduledTasks_Fld_ScheduledTasksID = "ScheduledTasksID";
        public const string ScheduledTasks_Fld_TaskName = "TaskName";
        public const string ScheduledTasks_Fld_TaskDescription = "TaskDescription";
        public const string ScheduledTasks_Fld_PerformTask = "PerformTask";
        public const string ScheduledTasks_Fld_RepeatTask = "RepeatTask";
        public const string ScheduledTasks_Fld_StartDate = "StartDate";
        public const string ScheduledTasks_Fld_StartTime = "StartTime";
        public const string ScheduledTasks_Fld_TaskId = "TaskId";
        public const string ScheduledTasks_Fld_SendEmail = "SendEmail";
        public const string ScheduledTasks_Fld_EmailAddress = "EmailAddress";
        public const string ScheduledTasks_Fld_AdvancedSeetings = "AdvancedSeetings";
        public const string ScheduledTasks_Fld_RepeatTaskInterval = "RepeatTaskInterval";
        public const string ScheduledTasks_Fld_Duration = "Duration";
        public const string ScheduledTasks_Fld_SendPrint = "SendPrint";
        public const string ScheduledTasks_Fld_Enabled = "Enabled";
        public const string ScheduledTasks_Fld_TaskNameOld = "TaskNameOld";
        public const string ScheduledTasks_Fld_PerformTaskText = "PerformTaskText";
        public const string ScheduledTasks_Fld_SendEmailText = "SendEmailText";
        public const string ScheduledTasks_Fld_EnabledText = "EnabledText";
        public const string ScheduledTasks_Fld_SendPrintText = "SendPrintText";
        public const string ScheduledTasks_Fld_colTask = "colTask";
        public const string ScheduledTasks_Fld_LastExecuted = "LastExecuted";

        public const string ScheduledTasksLog_tbl = "ScheduledTasksLog";
        public const string ScheduledTasksLog_Fld_ScheduledTasksLogID = "ScheduledTasksLogID";
        public const string ScheduledTasksLog_Fld_TaskStatus = "TaskStatus";
        public const string ScheduledTasksLog_Fld_LogDescription = "LogDescription";
        public const string ScheduledTasksLog_Fld_StartDate = "StartDate";
        public const string ScheduledTasksLog_Fld_StartTime = "StartTime";
        public const string ScheduledTasksLog_Fld_EndTime = "EndTime";
        public const string ScheduledTasksLog_Fld_ScheduledTasksID = "ScheduledTasksID";
        public const string ScheduledTasksLog_Fld_ComputerName = "ComputerName";

        public const string ScheduledTasksDetailWeek_tbl = "ScheduledTasksDetailWeek";
        public const string ScheduledTasksDetailWeek_Fld_ScheduledTasksID = "ScheduledTasksID";
        public const string ScheduledTasksDetailWeek_Fld_Days = "Days";
        public const string ScheduledTasksDetailWeek_Fld_SelectedDays = "SelectedDays";

        public const string ScheduledTasksDetailMonth_tbl = "ScheduledTasksDetailMonth";
        public const string ScheduledTasksDetailMonth_Fld_ScheduledTasksID = "ScheduledTasksID";
        public const string ScheduledTasksDetailMonth_Fld_DaysOrOn = "DaysOrOn";
        public const string ScheduledTasksDetailMonth_Fld_SelectionMonths = "SelectionMonths";
        public const string ScheduledTasksDetailMonth_Fld_SelectionDays = "SelectionDays";
        public const string ScheduledTasksDetailMonth_Fld_Monthperiods = "Monthperiods";
        public const string ScheduledTasksDetailMonth_Fld_weekDays = "weekDays";
        #endregion


        #region primepos-2831
        public const string POSTransPayment_Fld_IsEvertecForceTransaction = "IsEvertecForceTransaction";
        public const string POSTransPayment_Fld_IsEvertecSign = "IsEvertecSign";
        #endregion

        #region PRIMEPOS-2857        
        public const string POSTransPayment_Fld_EvertecTaxBreakdown = "EvertecTaxBreakdown";
        #endregion
        public const string SettingDetail_Extension_URL = "PrimeRxPayAdditionalUrl";  //PRIMEPOS-2956
        public const string SettingDetail_PrimeRxPayBGStatusUpdate = "PrimeRxPayBGStatusUpdate";  //PRIMEPOS-3187
        public const string SettingDetail_PrimeRxPayDefaultSelection = "PrimeRxPayDefaultSelection";  //PRIMEPOS-3250
        public const string SettingDetail_PrimeRxPayStatusUpdateIntervalInMin = "PrimeRxPayStatusUpdateIntervalInMin";  //PRIMEPOS-3187
        public const string SettingDetail_PrimeRxPayStatusUpdateFromLastDays = "PrimeRxPayStatusUpdateFromLastDays";  //PRIMEPOS-3187
        public const string SettingDetail_PseudoephedDisclaimer = "PseudoephedDisclaimer";  //PRIMEPOS-3109

        public const string ELAVON = "ELAVON"; //PRIMEPOS-2943

        public const string SettingDetail_SiteID = "SiteId";  //PRIMEPOS-2990
        public const string SettingDetail_LicenseID = "LicenseId";
        public const string SettingDetail_DeviceId = "DeviceId";
        public const string SettingDetail_Username = "Username";
        public const string SettingDetail_Password = "Password";
        public const string SettingDetail_DeveloperId = "DeveloperId";
        public const string SettingDetail_VersionNumber = "VersionNumber";

        public const string POSTransPayment_Fld_IsFsaCard = "IsFsaCard";//2990
        public const string CCCustomerTokInfo__Fld_IsFsaCard = "IsFsaCard";//2990

        public const string SettingDetail_LocationName = "LocationName";//3001
        public const string SettingDetail_ChainCode = "ChainCode";
        public const string SettingDetail_TerminalID = "TerminalID";
        public const string POSTransPayment_Fld_TokenID = "TokenID";//3009
        public const string POSTransPayment_Fld_NBSTransId = "NBSTransId";//PRIMEPOS-3375
        public const string POSTransPayment_Fld_NBSTransUid = "NBSTransUid";//PRIMEPOS-3375
        public const string POSTransPayment_Fld_NBSPaymentType = "NBSPaymentType";//PRIMEPOS-3375

        #region PRIMEPOS-3117 11-Jul-2022 JY TransactionFee table
        public const string TransFee_tbl = "TransactionFee";
        public const string TransFee_Fld_TransFeeID = "TransFeeID";
        public const string TransFee_Fld_TransFeeDesc = "TransFeeDesc";
        public const string TransFee_Fld_ChargeTransFeeFor = "ChargeTransFeeFor";
        public const string TransFee_Fld_TransFeeMode = "TransFeeMode";
        public const string TransFee_Fld_TransFeeValue = "TransFeeValue";
        public const string TransFee_Fld_PayTypeID = "PayTypeID";
        public const string TransFee_Fld_IsActive = "IsActive";
        #endregion

        #region PRIMEPOS-3370
        public const string SettingDetail_NBSEnable = "NBSEnable";
        public const string SettingDetail_NBSUrl = "NBSUrl";
        public const string SettingDetail_NBSToken = "NBSToken";
        public const string SettingDetail_NBSEntityID = "NBSEntityID";
        public const string SettingDetail_NBSStoreID = "NBSStoreID";
        public const string SettingDetail_NBSTerminalID = "NBSTerminalID";
        public const string SettingDetail_NBSBins = "NBSBins"; //PRIMEPOS-3529 //PRIMEPOS-3504
        #endregion

        public const string SettingDetail_WaiveTransactionFee = "WaiveTransactionFee";  //PRIMEPOS-3234
    }
}