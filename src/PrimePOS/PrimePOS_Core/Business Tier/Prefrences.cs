using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS_Core.CommonData;
//using POS.Resources;
using System.Collections.Generic;
using System.Linq;
using Resources;
using POS_Core.Resources;
using NLog;

namespace POS_Core.BusinessRules
{
    /// <summary>
    /// Summary description for Prefrences.
    /// </summary>
    public class Prefrences
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger(); //PRIMEPOS-2841 27-05-2020 SAJID/NILESH ADDED
        PreferenceSvr oPreSvr = new PreferenceSvr();//PRIMEPOS-2841 27-05-2020 SAJID/NILESH ADDED
        public Prefrences()
        {
        }

        public void UpdatePreferences(string WinBC, string WinFC, string BtnBC1, string BtnBC2, string BtnFC, int AllowAddItem, int MoveToQtyCol, string ActBC, string ActFC, string HdrBC, string HdrFC, POSSET oPOSSet, int showNumPad, int showItemPad, CompanyInfo oCInfo, SettingDetail oSetting, POS_Core.CommonData.ItemData oIData, CustomerLoyaltyInfo oCustomerLoyaltyInfo, MerchantConfig oMerchantConfig, bool bEditMerchantConfig, DataTable dtPayTypeReceipts, string theme, PrimeEDISetting oPrimeEDISetting)    //PRIMEPOS-2227 05-May-2017 JY Added MerchantConfig paramter //PRIMEPOS-2308 16-May-2018 JY Added dtPayTypeReceipts parameter //PrimePOS-2523 Added theme parameter by Farman Ansari on 06-June-2018//ADDED SETTINGDETAIL IN PRIMEPOS-2739  //PRIMEPOS-3167 07-Nov-2022 JY Added oPrimeEDISetting
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;

            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                //Modify by Atul 2-10-10 Add one Parameter AlloRxPicked
                oPreSvr.UpdateInterfaceColor(oTrans, WinBC, WinFC, BtnBC1, BtnBC2, BtnFC, AllowAddItem, MoveToQtyCol, ActBC, ActFC, HdrBC, HdrFC, showNumPad, showItemPad, theme); //PrimePOS-2523 Added theme parameter by Farman Ansari on 06-June-2018
                ErrorHandler.SaveLog((int)LogENUM.Settings_Change, Configuration.UserName, "Success", "User settings changed successfully");

                //if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID,UserPriviliges.Screens.DeviceSettings.ID,0)) //PRIMEPOS-2484 04-Jun-2020 JY Commented
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.Preferences.ID, UserPriviliges.Permissions.DeviceSettings.ID)) //PRIMEPOS-2484 04-Jun-2020 JY Added 
                {
                    oPreSvr.UpdateDeviceSettings(oTrans, oPOSSet);
                    ErrorHandler.SaveLog((int)LogENUM.Settings_Change, Configuration.UserName, "Success", "Device settings changed successfully");
                }
                //if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID,UserPriviliges.Screens.Preferences.ID,0))    //PRIMEPOS-2484 04-Jun-2020 JY Commented
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.Preferences.ID, UserPriviliges.Permissions.AppSettings.ID))    //PRIMEPOS-2484 04-Jun-2020 JY Added
                {
                    oPreSvr.UpdateAppSettings(oTrans, oPOSSet);
                    ErrorHandler.SaveLog((int)LogENUM.Settings_Change, Configuration.UserName, "Success", "Application settings changed successfully");

                    oPreSvr.UpdateCompanyInfo(oTrans, oCInfo);
                    ErrorHandler.SaveLog((int)LogENUM.Settings_Change, Configuration.UserName, "Success", "Company settings changed successfully");

                    oPreSvr.UpdatePayTypesReceipts(oTrans, dtPayTypeReceipts);  //PRIMEPOS-2308 16-May-2018 JY Added                    

                    ItemSvr oItemSvr = new ItemSvr();
                    oItemSvr.Persist(oIData, oTrans);
                }

                //if (oMerchantConfig.STATIONID.Trim() != string.Empty) //16-Jun-2016 JY Added
                if (bEditMerchantConfig)
                    oPreSvr.UpdateMerchantConfig(oTrans, oMerchantConfig);  //PRIMEPOS-2227 05-May-2017 JY Added

                //if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.CLPSettings.ID, 0))  //PRIMEPOS-2484 04-Jun-2020 JY Commented
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.Preferences.ID, UserPriviliges.Permissions.CLPSettings.ID))   //PRIMEPOS-2484 04-Jun-2020 JY Added
                {
                    oPreSvr.UpdateCustomerLoyaltyInfo(oTrans, oCustomerLoyaltyInfo);
                }

                //if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.RxSettings.ID, 0))   //PRIMEPOS-2484 04-Jun-2020 JY Commented
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.Preferences.ID, UserPriviliges.Permissions.RxSettings.ID)) //PRIMEPOS-2484 04-Jun-2020 JY Added
                {
                    oPreSvr.UpdateRxSettings(oTrans, oCInfo, oPOSSet);    //Update Rx settings
                }

                //if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.PrimePOSettings.ID, 0))  //PRIMEPOS-2484 04-Jun-2020 JY Commented
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.Preferences.ID, UserPriviliges.Permissions.PrimePOSettings.ID))    //PRIMEPOS-2484 04-Jun-2020 JY Added
                {
                    //oPreSvr.UpdatePrimePOSettings(oTrans, oCInfo, oPOSSet);    //PrimePO settings //PRIMEPOS-3167 07-Nov-2022 JY Commented
                    oPreSvr.UpdatePrimeEDISettings(oTrans, oPrimeEDISetting);    //PrimePO settings  //PRIMEPOS-3167 07-Nov-2022 JY Added
                }

                //if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.TransSettings.ID, 0))    //PRIMEPOS-2484 04-Jun-2020 JY Commented
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.Preferences.ID, UserPriviliges.Permissions.TransSettings.ID))  //PRIMEPOS-2484 04-Jun-2020 JY Added
                {
                    oPreSvr.UpdateTransSettings(oTrans, oCInfo, oPOSSet);    //PrimePO settings
                }

                oPreSvr.UpdateEmailsettings(oTrans, oCInfo);    //Update Email Settings
                oPreSvr.UpdateInterfaceSettings(oTrans, oCInfo);    //Update Interface Settings
                oPreSvr.UpdateIVULottoSettings(oTrans, oCInfo, oPOSSet);    //Update IVU Lotto settings
                oPreSvr.UpdateMessagingOptions(oTrans, oCInfo);    //Update Messaging Options
                oPreSvr.UpdateConsentCapture(oTrans, oCInfo);    //Update Consent Capture

                #region ADDED FOR SETTING DETAIL PRIMEPOS-2739
                oPreSvr.SaveSettingDetails(oTrans, oSetting);
                #endregion

                oTrans.Commit();
            }
            catch (Exception exp)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                //ErrorHandler.throwCustomError(POSErrorENUM.Pref_UnableToSave);
                throw (exp);
            }
        }

        public DataSet Search(String strQuery)
        {
            try
            {
                SearchSvr oSearchSvr = new SearchSvr();
                return oSearchSvr.Search(strQuery);
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
        //Added by Atul 06-10-2010
        public DataSet PopulateGetDiscount(String StationID)
        {
            PreferenceSvr dao = new PreferenceSvr();
            return dao.PopulateKeyDiscount(StationID);
        }
        //Added by Atul 06-10-2010
        public void UpdateKeyDiscount(string StationID, Decimal A, Decimal B, Decimal C, Decimal D, Decimal E)
        {
            PreferenceSvr pre = new PreferenceSvr();
            pre.UpdateKeyDiscount(StationID, A, B, C, D, E);
        }

        public List<String> GetExcludedDeptIds()
        {
            Department dept = new Department();
            DataTable table = dept.GetCLExcludedDeptData().Tables[0];
            List<String> data = new List<String>();
            return Enumerable.Select(table.AsEnumerable(), deptIdRow => deptIdRow["DeptId"].ToString()).ToList<String>();
        }

        public List<String> GetExcludedSubDeptIds()
        {
            SubDepartment dept = new SubDepartment();
            DataTable table = dept.GetCLExcludedSubDeptData().Tables[0];
            List<String> data = new List<String>();
            return Enumerable.Select(table.AsEnumerable(), deptIdRow => deptIdRow["subdepartmentid"].ToString()).ToList<String>();
        }

        public List<String> GetExcludedItemIds()
        {
            Item dept = new Item();
            DataTable table = dept.GetCLExcludedItemData().Tables[0];
            List<String> data = new List<String>();
            return Enumerable.Select(table.AsEnumerable(), itemIdRow => itemIdRow["itemId"].ToString()).ToList<String>();
        }

        public List<String> GetExcludedCLCouponDeptIds()
        {
            Department dept = new Department();
            DataTable table = dept.GetCLCouponExcludedDeptData().Tables[0];
            List<String> data = new List<String>();
            return Enumerable.Select(table.AsEnumerable(), deptIdRow => deptIdRow["DeptId"].ToString()).ToList<String>();
        }

        public List<String> GetExcludedCLCouponSubDeptIds()
        {
            SubDepartment dept = new SubDepartment();
            DataTable table = dept.GetCLCouponExcludedSubDeptData().Tables[0];
            List<String> data = new List<String>();
            return Enumerable.Select(table.AsEnumerable(), deptIdRow => deptIdRow["subdepartmentid"].ToString()).ToList<String>();
        }

        public List<String> GetExcludedCLCouponItemIds()
        {
            Item dept = new Item();
            DataTable table = dept.GetCLCouponExcludedItemData().Tables[0];
            List<String> data = new List<String>();
            return Enumerable.Select(table.AsEnumerable(), itemIdRow => itemIdRow["itemId"].ToString()).ToList<String>();
        }

        #region Sprint-24 - PRIMEPOS-2344 18-Jan-2017 JY Added to get last updated IIAS file date
        public DateTime? GetIIASFileLastUpdatedDate()
        {
            PreferenceSvr dao = new PreferenceSvr();
            return dao.GetIIASFileLastUpdatedDate();
        }
        #endregion

        #region Sprint-25 - PRIMEPOS-2379 08-Feb-2017 JY Added
        public DateTime? GetPSEFileLastUpdatedDate()
        {
            PreferenceSvr dao = new PreferenceSvr();
            return dao.GetPSEFileLastUpdatedDate();
        }
        #endregion

        #region Sprint-26 - PRIMEPOS-2441 21-Jul-2017 JY Added
        public DataTable GetMerchantConfig()
        {
            DataTable dtMerchantConfig = null;
            try
            {
                PreferenceSvr oPreferenceSvr = new PreferenceSvr();
                dtMerchantConfig = oPreferenceSvr.GetMerchantConfig();
            }
            catch (Exception Ex)
            {
            }
            return dtMerchantConfig;
        }
        #endregion

        #region PRIMEPOS-2308 15-May-2018 JY Added to load paytype grid to set # of receipts 
        public DataTable GetPayTypesReceipts()
        {
            DataTable dt = null;
            try
            {
                PreferenceSvr oPreferenceSvr = new PreferenceSvr();
                dt = oPreferenceSvr.GetPayTypesReceipts();
            }
            catch (Exception Ex)
            {
            }
            return dt;
        }
        #endregion

        #region PRIMEPOS-2613 26-Dec-2018 JY Added
        public DataTable getMessageFormat(int MessageCatId, int MessageTypeId)
        {
            DataTable dt = null;
            try
            {
                PreferenceSvr oPreferenceSvr = new PreferenceSvr();
                dt = oPreferenceSvr.getMessageFormat(MessageCatId, MessageTypeId);

                DataRow dr = dt.NewRow();
                dr["RecID"] = 0;
                dr["MessageCode"] = "None";
                dt.Rows.InsertAt(dr, 0);
            }
            catch (Exception Ex)
            {
            }
            return dt;
        }
        #endregion  

        #region PRIMEPOS-2842 05-May-2020 JY Added
        public DataTable GetTriPOSSettings()
        {
            DataTable dt = null;
            try
            {
                PreferenceSvr oPreferenceSvr = new PreferenceSvr();
                dt = oPreferenceSvr.GetTriPOSSettings();
            }
            catch (Exception Ex)
            {
            }
            return dt;
        }

        public void UpdateTriPOSSettings(System.Int64 TriPOSAcceptorID, System.Int64 TriPOSAccountID, string TriPOSAccountToken, int TriPOSLaneID, string TriPOSLaneDesc, string TriPOSConfigFilePath, string TriPOSTerminalId)
        {
            System.Data.IDbTransaction oTrans = null;
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);


                oPreSvr.UpdateTriPOSSettings(oTrans, TriPOSAcceptorID, TriPOSAccountID, TriPOSAccountToken, TriPOSLaneID, TriPOSLaneDesc, TriPOSConfigFilePath, TriPOSTerminalId);
                oTrans.Commit();
            }
            catch (Exception Ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                //throw (exp);
            }
        }

        #region PRIMEPOS-3024 08-Nov-2021 JY Added
        public void UpdateTriPOSIPLane(int TriPOSLaneID, string TriPOSLaneDesc, string TriPOSTerminalId)
        {
            System.Data.IDbTransaction oTrans = null;
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
                oPreSvr.UpdateTriPOSIPLane(oTrans, TriPOSLaneID, TriPOSLaneDesc, TriPOSTerminalId);
                oTrans.Commit();
            }
            catch (Exception Ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
            }
        }
        #endregion

        public void UpdateTriPOSConfigFilePath(string strConfigFilePath)
        {
            System.Data.IDbTransaction oTrans = null;
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);


                oPreSvr.UpdateTriPOSConfigFilePath(oTrans, strConfigFilePath);
                oTrans.Commit();
            }
            catch (Exception Ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                //throw (exp);
            }
        }
        #endregion

        #region PRIMEPOS-2841 27-05-2020 SAJID/NILESH ADDED
        public DataSet GetPrimeRxPayDetails(string fieldName)
        {

            DataSet ds = new DataSet();
            try
            {
                ds = oPreSvr.GetPrimeRxPayDetails(fieldName);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetPrimeRxPayDetails");
            }
            return ds;
        }

        public void UpdatePrimeRxPayDetails(string FieldName, string FieldValue)
        {
            try
            {
                oPreSvr.UpdatePrimeRxPayDetails(FieldName, FieldValue);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdatePrimeRxPayDetails");
            }
        }
        #endregion

        #region MyRegion
        public DataTable GetPayTypeData()
        {
            DataTable dt = null;
            try
            {
                PreferenceSvr oPreferenceSvr = new PreferenceSvr();
                dt = oPreferenceSvr.GetPayTypeData();
            }
            catch (Exception Ex)
            {
            }
            return dt;
        }

        public POSTransPaymentData PopulatePayTypeData()
        {
            POSTransPaymentData oPOSTransPaymentData = new POSTransPaymentData();
            try
            {
                PayType oPayType = new PayType();
                DataSet oPayTypeData;
                oPayTypeData = oPayType.PopulateList("");

            }
            catch (Exception Ex)
            {

            }
            return oPOSTransPaymentData;
        }
        #endregion

        #region PRIMEPOS-2485 11-Mar-2021 JY Added
        public void UpdateSettingDetails(SettingDetail oSetting)
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
                oPreSvr.SaveSettingDetails(oTrans, oSetting);
                oTrans.Commit();
            }
            catch (Exception exp)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                throw (exp);
            }
        }
        #endregion
    }
}