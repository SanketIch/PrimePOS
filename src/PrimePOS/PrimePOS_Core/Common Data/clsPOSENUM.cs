using System;

namespace POS_Core.CommonData
{
	/// <summary>
	/// Summary description for clsPOSErrors.
	/// </summary>
	/// 
	public enum SaveModeENUM : long
	{
		Create = 1 ,
		Modify = 2
	}

	public enum LoginENUM : int
	{
		Login = 1 ,
		Lock = 2 ,
        LogOff = 3,
		CheckPrevliges = 2 
	}
    //Notes_CodeCanNotBeNULL = 1079 is Added By Shitaljit(QuicSolv) 0n 5 oct 2011
	public enum POSErrorENUM : long
	{
		Vendor_PrimaryKeyVoilation = 1000 ,
		Vendor_CodeCanNotBeNULL = 1001 ,
		Vendor_NameCanNotBeNULL = 1002 ,
		Vendor_Address1CanNotBeNULL = 1003 ,
		Vendor_CityCanNotBeNull = 1006 ,
		Vendor_StateCannotBeNull = 1007 ,

		User_InvalidUserName = 1004 ,
		User_InValidPassward = 1005 ,

		Customer_PrimaryKeyVoilation = 1008 ,
		Customer_CodeCanNotBeNULL = 1009 ,
		Customer_NameCanNotBeNULL = 1010 ,
		Customer_Address1CanNotBeNULL = 1011 ,
		Customer_CityCanNotBeNull = 1012 ,
		Customer_StateCannotBeNull = 1013 ,

		TaxCodes_PrimaryKeyVoilation = 1014 ,
		TaxCodes_DescriptionCanNotBeNull = 1015 ,
		TaxCodes_AmountCanNotBeNull = 1016 ,

		Department_PrimaryKeyVoilation = 1017 ,
		Department_CodeCanNotBeNULL = 1018 ,
		Department_NameCanNotBeNULL = 1019 ,
		Department_SalePriceCanNotBeNULL = 1020 ,

		Item_PrimaryKeyVoilation = 1021 ,
		Item_CodeCanNotBeNULL = 1022 ,
		Item_DescriptionCanNotBeNULL = 1023 ,
		Item_DepartmentCanNotBeNull = 1024 ,
		Item_TaxCodeCanNotBeNull = 1025, 

		Database_ConnectionFailed=1026,

		PayOut_DescriptionCanNotBeNULL = 1027 ,
		PayOut_AmountCanNotBeNull = 1028 ,

		ItemCompanion_CompanionItemIDCanNotBeNULL = 1029 ,
		ItemCompanion_ItemIDCanNotBeNull = 1030 ,
		ItemCompanion_AmountCanNotBeNull = 1031 ,

		ItemGroupPrice_ItemIDCanNotBeNULL = 1032 ,
		ItemGroupPrice_QtyCanNotBeNull = 1033 ,
		ItemGroupPrice_CostCanNotBeNull = 1034 ,
		ItemGroupPrice_SalePriceCanNotBeNull = 1035 ,

		ItemVendor_ItemIDCanNotBeNULL = 1036 ,
		ItemVendor_VendorItemIDCanNotBeNull = 1037 ,
		ItemVendor_VendorIDCanNotBeNull = 1038 ,
		ItemVendor_VenorCostPriceCanNotBeNull = 1039,

		InvRecvHeader_VendorIDNotNull=1040,
		InvRecvHeader_RefNoNotNull=1041,
		InvRecvHeader_RecvDateNotNull=1042 ,

		InvRecvDetail_ItemCodeCanNotNull= 1044 ,
		InvRecvDetail_QTyCanNotNull = 1045,
		InvRecvDetail_CostCanNotBeNull = 1046 ,
		InvRecvDetail_SalePriceCanNotBeNull = 1047, 

		POHeader_VendorCodeCanNotNull=1043,
		POHeader_OrderDateCanNotNull=1048,
		
		PODetail_ItemCodeCanNotNull= 1049 ,
		PODetail_QTYCanNotNull = 1050,
		PODetail_CostCanNotBeNull = 1051 ,
		PODetail_AtleastOneDetailItemReq = 1052,

		TransHeader_CustomerIDCanNotNull=1053,
		TransHeader_DateIsInvalid=1054,
		TransHeader_CanNotChangeTransactionType=1055,

		TransDetail_ItemCodeCanNotNull= 1056 ,
		TransDetail_QTYCanNotNull = 1057,

		TransHeader_UnableToSaveData=1058 ,
		Users_UserNotHavePriviligesForThisAction=1059 ,
		POHeader_OrderNoNotNull = 1060 ,
		InvRecvHeader_RecvDetailMissing=1061,
		TransDetail_DetailIsMisssing = 1062,
		TransPayment_DetailIsMisssing = 1063 ,
		StationClose_NoTransactionFoundForEndOFDay = 1064 ,
		StationClose_NoTransactionFoundForCloseStation = 1065 ,
		EndOfDay_CloseAllStationsFirst = 1066 ,
		Customer_DuplicateCode = 1067 ,
		POHeader_ExpectedDlryDateShouldBeGreaterThanOrderDate=1068 ,
		Department_DuplicateCode = 1069 ,
		Item_DuplicateCode = 1070 ,
		Vendor_DuplicateCode = 1071 ,
		User_InvalidSecurityLevel = 1072,
		Pref_UnableToSave = 1073,
		TransDetail_RXNotFound=1074,
        ItemPrice_Validation = 1075,
        User_Locked = 1076,
        User_ResetPassword = 1077,
        User_Duplicate=1078,
        Notes_CodeCanNotBeNULL = 1079,
        Notes_DuplicateCode = 1080,
        TransDetail_UnbilledRX = 1081,
        TransDetail_FiledRX = 1082,
        ItemComboPricingDetail_ItemIDCanNotBeNULL = 1083,
        ItemComboPricingDetail_QtyCanNotBeNull = 1084,
        ItemComboPricingDetail_SalePriceCanNotBeNull = 1085,
        ItemComboPricing_DetailMissing = 1087,
        ItemComboPricing_DescriptionMissing = 1088,
        Customer_FirstNameCanNotBeNULL = 1089,
		TaxCode_DuplicateTaxCode = 1090,
        PSE_Items_ProductIdCanNotBeNULL = 1091,
        PSE_Items_ProductNameCanNotBeNULL = 1092,
        PSE_Items_ProductGramsCanNotBeNULL = 1093,
        PSE_Items_DuplicateProductId = 1094,
        PSE_Items_PrimaryKeyVoilation = 1095,
        User_ChangePasswordAtLogin = 1096  //PRIMEPOS-2577 15-Aug-2018 JY Added
    }
    
    public enum LogENUM : int
    {
        Application_Start = 1,
        Application_Close = 2,
        Login = 3,
        Settings_Change = 4,
        UserRights_Change = 5,
        View_Report = 6,
        Create_User = 7,
        Update_User = 8,
        Delete_User = 9,
        Change_Password = 10,
        Reset_Password = 11,
        Account_Locked = 12,
        PCCharge_Selected = 13,
        XCharge_Selected = 14,
        XLink_Selected = 15,
        Exception_Occured = 16
    }
}
