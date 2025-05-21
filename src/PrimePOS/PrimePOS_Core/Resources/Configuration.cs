using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Xml;
using Resources;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using System.Timers;
using System.Diagnostics;
using NLog;
using System.ComponentModel;    //PRIMEPOS-2613 24-Dec-2018 JY Added    
using POS_Core.Resources.DelegateHandler;
using static MMSInterfaces.Helpers.EventHub;
using System.IO;    //PRIMEPOS-2774 13-Jan-2020 JY Added
using System.Collections;
using iTextSharp.text.pdf;
using System.Windows.Forms; //PRIMEPOS-2779 17-Jan-2020 JY Added
using System.Linq;
using Newtonsoft.Json;
using PrimeRxPay;
using System.Management;
using System.Text.RegularExpressions;
using POS_Core.Document;

namespace POS_Core.Resources
{
    public sealed class Configuration
    {
        // Constant values for all expected entries in the POS.Configuration section
        public const string k_EncryptKey = "Lion";
        public const string k_InicializationVector = "akbar";

        public static clsCryption oCryption = new clsCryption("Lion", "");
        public static string m_ConnString = "";
        private static string m_ConnStringMaster = "";
        public static readonly string CBCItemCode = "CBC";
        public static readonly string CouponItemCode = "COUPON";//Added By Shitajit for coupon discount
        public static bool IsWelComeScreen = false;

        private static string m_UserName;

        public static DateTime MinimumDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

        public static CompanyInfo CInfo;
        public static POSSET CPOSSet;
        public static PrimeEDISetting CPrimeEDISetting;  //PRIMEPOS-3167 07-Nov-2022 JY Added
        public static CustomerLoyaltyInfo CLoyaltyInfo;
        public static MerchantConfig objMerchantConfig; //PRIMEPOS-2227 05-May-2017 JY Added
        public static List<MerchantConfig> lstMerchantConfig;    //PRIMEPOS-2227 05-May-2017 JY Added
        private static string m_CCProcessDB;

        private static string m_DBUser;
        private static string m_Passward;
        private static string m_ServerName;
        private static string m_DatabaseName;
        private static string m_StationID;
        private static string m_StationName;
        private static Int32 m_DrawNo;
        private static string m_HostIPAddress = "tcp://localhost:8080/PrimePosHostSrv";

        private static bool m_moveToQtyCol;
        private static bool m_AllowAddItemInTrans;
        private static bool m_showNumPad;
        private static bool m_showItemPad;
        //Adde by Atul 2-10-10
        private static int m_AllowRxPicked;  //PRIMEPOS-2865 15-Jul-2020 JY modified

        private static Int32 m_SecurityLevel;
        //		private static ConfigurationManager oSettings = new ConfigurationManager("PrimePOS");

        private static String m_Theme;
        private static String m_Window_Color;
        private static String m_Window_Button_Color1;

        private static String m_Window_Button_Color2;
        private static String m_Window_ForeColor;
        private static String m_Window_Button_ForeColor;
        private static String m_Active_BackColor;
        private static String m_Active_ForeColor;
        private static String m_Header_BackColor;
        private static String m_Header_ForeColor;

        private static Int32 m_LabelPerSheet;
        private static bool m_POSTbr;
        private static bool m_InvTbr;
        private static bool m_AdminTbr;
        private static bool m_UpdateInProgress = false;

        private static string m_DefaultPaymentProcessor; //Added By Dharmendra (SRT) on Dec-15-08 to set the value of default payment Processor
        //Added By Dharmendra (SRT) on Nov-18-08

        private static bool bIsPrimeInterfaceConnected = false; //PRIMEPOS-2643

        # region '------------ Constants with default values for Payment Processor & Pin Pad-----------'
        private const string PAYMENTPROCESSOR = "PCCHARGE";
        private const string AVSMODE = "N";
        private const string TXNTIMEOUT = "30";
        private const bool USEPINPAD = false;
        private const string PINPADMODEL = "VeriFone 101/1000";
        private const string PINPADBAUDRATE = "1200";
        private const string PINPADPAIRITY = "E";
        private const string PINPADPORTNO = "1";
        private const string PINPADDATABITS = "7";
        private const string PINPADDISPMESG = "WELCOME TO MMS";
        private const string PINPADKEYENCRYPTIONTYPE = "DUKPT";
        private const string HEARTBEATTIME = "10";
        private const bool PROCESSONLINE = false;
        //Added By Dharmendra on March-31-09
        private const string PRIMEPOHOSTADDRESSURL = "tcp://localhost:8090/";
        public const int PRIMEPOCONNECTIONTIMERVALUE = 1;
        public const int PRIMEPOPRICEUPDATETIMERVALUE = 1;
        public const int PRIMEPOPURCHASEORDERTIMERVALUE = 5;
        //Added Till Here March-31-09
        public const int PASSWORDRESETDAYS = 90;

        # endregion '---------Constants with default values for Payment Processor & Pin Pad till here--'
        // End Added N0v-18-08
        //Added By Dharmendra SRT on Feb-06-09
        // private static bool _HostServerRunningStatus;
        //  private static bool _PadConnectionStatus;
        //Added Till Here Feb-06-09
        //private static String Report_Heading="RUBEX DRUG, INC.";
        private static int m_ResetPwdDays;
        private static string S_ConnectionStringType = "MasterDatabase";
        private static string S_SQLUserPassword;
        private static string S_SQLUserID;
        private static Decimal D_MaxDiscountLimit;//Added By Shitaljit repersent Max Discount a user can give which in define in user Setup file.
        private static Decimal D_MaxTransactionLimit;//Added By Ravindra For  Max Transaction Limit for user.
        private static bool B_IsDiscountOverride = false;//Added By shitaljit on 2/11/2014 for JIRA PRIMEPOS-1810
        public static LabelPrintingSetup oLPSetup;//Added By Shitaljit for Label Printing obects set up.
        private static string S_POSWDirectoryPath;
        private static Decimal dMaxReturnTransLimit;   //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
        private static Decimal dMaxTenderedAmountLimit;   //Sprint-25 - PRIMEPOS-2411 20-Apr-2017 JY Added 
        private static Decimal dMaxCashbackLimit;   //PRIMEPOS-2741 25-Sep-2019 JY Added
        private static Decimal dHourlyRate; //PRIMEPOS-189 02-Aug-2021 JY Added
        private static string sAuthorizationNo; //12-Sep-2016 JY Added to preserve ID 
        private static string sDOB; //PRIMEPOS-2729 06-Sep-2019 JY Added

        private static string sCashierID;// PRIMEPOS-2664 ADDED BY ARVIND EVERTEC
        #region Sprint-25 - PRIMEPOS-2380 15-Feb-2017 JY Added PSE_Items constants
        public const string PSE_Items_UOM = "2";    //2 for gms used in Item monitoring category
        public const int PSE_Items_Monitoring_Category = 0;
        public const bool PSE_Items_SentToNplex = true;
        #endregion

        public static int[] arrCloseBk = new int[] { 232, 62, 76 };  //18-Jan-2018 JY background color for close button

        public static Dictionary<string, int> dctPayTypeReceipts; //PRIMEPOS-2308 16-May-2018 JY Added
        public static string sTransAlreadyReturned = "This transaction is already returned.";   //PRIMEPOS-2586 11-Sep-2018 JY Added
        public static int iTransAlreadyReturned = 1;   //PRIMEPOS-2586 11-Sep-2018 JY Added
        public static string sNegativeItemPrice = "Changing Item Price Will Result In Negative Value; This Change Is Not Allowed.";   //PRIMEPOS-2586 24-Sep-2018 JY Added
        public static int iNegativeItemPrice = 2;   //PRIMEPOS-2586 24-Sep-2018 JY Added
        public static string sCanNotScanSameRx = "Cannot scan same RX in same transaction.";   //PRIMEPOS-2586 24-Sep-2018 JY Added
        public static int iCanNotScanSameRx = 3;   //PRIMEPOS-2586 24-Sep-2018 JY Added        
        public static int iRxIsOlderThan = 4;   //PRIMEPOS-2586 24-Sep-2018 JY Added
        public static int iRxOnHoldExMsg = 5;   //PRIMEPOS-2586 27-Sep-2018 JY Added
        public static int iRxCannotBeCheckedOutExMsg = 6;   //PRIMEPOS-2586 27-Sep-2018 JY Added
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public static int TransactionRequestCount { get; set; } // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
        #region PRIMEPOS-2739 ADDED BY ARVIND
        public static SettingDetail CSetting;
        #endregion

        public static HyphenSetting hyphenSetting; //PRIMEPOS-3207
        public static MerchantConfig oMerchantConfig;//PRIMEPOS-TOKENURL
        public static List<NBSBinRange> nBSBinRange = new List<NBSBinRange>(); //PRIMEPOS-3372
        //PRIMEPOS-2643
        public static InterfaceStatus eInterfaceStatus;

        public static DateTime dtpSearchPOAckFromDate = DateTime.Now.Subtract(new TimeSpan(60, 0, 0, 0));   //PRIMEPOS-2894 12-Oct-2020 JY Added
        public static DateTime dtpSearchPOAckToDate = DateTime.Now; //PRIMEPOS-2894 12-Oct-2020 JY Added

        /// <summary>
        /// Added By Shitaljit to get POSW directory path
        /// </summary>
        public static string POSWDirectoryPath
        {
            get
            {
                return S_POSWDirectoryPath;
            }
            set
            {
                S_POSWDirectoryPath = value;
            }
        }
        /// <summary>
        /// Author:Shitaljit
        /// To get the Lebel Printing Setup details
        /// </summary>
        public static void GetLabelPrintingSetup()
        {
            oLPSetup = new LabelPrintingSetup();
            Search oSearch = new Search();
            try
            {
                DataSet oDS = oSearch.SearchData("Select * from LabelPrintingSetup");
                if (Configuration.isNullOrEmptyDataSet(oDS) == false)
                {
                    oLPSetup.PrintItemID = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintItemID"]);
                    oLPSetup.PrintItemIDBarcode = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintItemIDBarcode"]);
                    oLPSetup.PrintDescription = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintDescription"]);
                    oLPSetup.PrintItemPrice = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintItemPrice"]);
                    oLPSetup.PrintItemVendorID = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintItemVendorID"]);
                    oLPSetup.PrintItemVendorIDBarcode = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintItemVendorIDBarcode"]);
                    oLPSetup.LabelTemplate = Configuration.convertNullToString(oDS.Tables[0].Rows[0]["LabelTemplate"]);
                    oLPSetup.PrintVendorName = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintVendorName"]);    //PRIMEPOS-2758 12-Nov-2019 JY Added

                    oLPSetup.ManufacturerName = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintManufacturerName"]);
                    oLPSetup.AvgPrice = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintAvgPrice"]);
                    oLPSetup.OnSalePrice = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintOnSalePrice"]);
                    oLPSetup.ProductCode = Configuration.convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintProductCode"]);
                }
                else
                {
                    oLPSetup.PrintItemID = true;
                    oLPSetup.PrintItemIDBarcode = true;
                    oLPSetup.PrintDescription = true;
                    oLPSetup.PrintItemPrice = true;
                    oLPSetup.PrintItemVendorID = true;
                    oLPSetup.PrintItemVendorIDBarcode = true;
                    oLPSetup.PrintVendorName = true;    //PRIMEPOS-2758 12-Nov-2019 JY Added

                    oLPSetup.ManufacturerName = true;
                    oLPSetup.AvgPrice = true;
                    oLPSetup.OnSalePrice = true;
                    oLPSetup.ProductCode = true;

                    oLPSetup.LabelTemplate = "";
                }
            }
            catch (Exception Ex)
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        /// <summary>
        /// Author: Shitaljit Date: 10/21/2013
        /// added to check internet conection is available or not.
        /// </summary>
        /// <returns></returns>
        public static bool CheckInternetConnection()
        {
            bool bStatus;
            try
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                {
                    bStatus = true;
                }
                else
                {
                    bStatus = false;
                }
            }
            catch (Exception)
            {
                bStatus = false;
            }
            return bStatus;
        }
        /// <summary>
        /// Author:Shitaljit
        /// To Save Label Printing SetUp Details.
        /// </summary>
        /// <param name="OLabelPintSetup"></param>
        public static void SaveLabelPrintingSetup(LabelPrintingSetup OLabelPintSetup)
        {
            string sSQL = string.Empty;
            try
            {
                sSQL = "DELETE FROM  LabelPrintingSetup ";
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                sSQL = "INSERT INTO LabelPrintingSetup VALUES (1,1,1,1,1,1,'',1,1,1,1,1)";  //PRIMEPOS-2758 12-Nov-2019 JY modified
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                sSQL = @"UPDATE LabelPrintingSetup
                    SET PrintItemID = " + Configuration.convertBoolToInt(OLabelPintSetup.PrintItemID) +
              " ,PrintItemIDBarcode =" + Configuration.convertBoolToInt(OLabelPintSetup.PrintItemIDBarcode) +
              " ,PrintItemVendorID = " + Configuration.convertBoolToInt(OLabelPintSetup.PrintItemVendorID) +
              " ,PrintItemPrice =" + Configuration.convertBoolToInt(OLabelPintSetup.PrintItemPrice) +
              " ,PrintDescription =" + Configuration.convertBoolToInt(OLabelPintSetup.PrintDescription) +
              " ,PrintItemVendorIDBarcode =" + Configuration.convertBoolToInt(OLabelPintSetup.PrintItemVendorIDBarcode) +
                " ,PrintOnSalePrice = " + Configuration.convertBoolToInt(OLabelPintSetup.OnSalePrice) +
              " ,PrintProductCode =" + Configuration.convertBoolToInt(OLabelPintSetup.ProductCode) +
              " ,PrintAvgPrice =" + Configuration.convertBoolToInt(OLabelPintSetup.AvgPrice) +
              " ,PrintManufacturerName =" + Configuration.convertBoolToInt(OLabelPintSetup.ManufacturerName) +
              " ,LabelTemplate = '" + Configuration.convertNullToString(OLabelPintSetup.LabelTemplate) + "'" +
              " ,PrintVendorName =" + Configuration.convertBoolToInt(OLabelPintSetup.PrintVendorName);    //PRIMEPOS-2758 12-Nov-2019 JY Added

                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
            }
            catch (Exception Ex)
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        /// <summary>
        /// Author:shitajit 
        /// TO check system is respondiong or not.
        /// </summary>
        /// <returns></returns>
        public static bool ProcessStatus(string sProcessName)
        {
            //POS_Core.ErrorLogging.Logs.Logger("Checking Process Status ", "ProcessStatus()", clsPOSDBConstants.Log_Entering); 
            bool RetVal = true;
            Process[] pArry = Process.GetProcessesByName(sProcessName);
            foreach (Process process in pArry)
            {
                if (process.Responding == false)
                {
                    POS_Core.ErrorLogging.Logs.Logger("Checking Process Status ", "ProcessStatus()", "System is not responding");
                    RetVal = false;
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }
                else
                {
                    POS_Core.ErrorLogging.Logs.Logger("Checking Process Status ", "ProcessStatus()", "Process is running");
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("Checking Process Status ", "ProcessStatus()", clsPOSDBConstants.Log_Exiting);
            return RetVal;
        }
        public static int INACTIVE_INTERVAL
        {
            get { return CPOSSet.Inactive_Interval; }
        }

        public static bool UpdateInProgress
        {
            get
            {
                return m_UpdateInProgress;
            }
            set
            {
                m_UpdateInProgress = value;
            }
        }

        private static bool IsPadSettingExists(string UserID)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";
            IDataReader dr;

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = m_ConnString;

            conn.Open();

            try
            {
                sSQL = "SELECT * FROM Util_Interface_Param WHERE Userid= '" + UserID + "'";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                dr = cmd.ExecuteReader();
                if (dr.Read() == false)
                {
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }

            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
        }

        public static FormLocation GetPadSetting(int isNumPad)
        {
            FormLocation frmLoc = new FormLocation();

            if (isNumPad == 0)
            {
                frmLoc.Left = 7;
                frmLoc.Top = 506;
                frmLoc.Width = 1009;
                frmLoc.Height = 208;
            }
            else
            {
                frmLoc.Left = 716;
                frmLoc.Top = 22;
                frmLoc.Width = 296;
                frmLoc.Height = 477;
            }

            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";
            IDataReader dr;

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = m_ConnString;

            conn.Open();

            try
            {
                sSQL = "SELECT * FROM Util_TransPadLoc WHERE userID = '" + m_UserName + "' AND isnumpad=" + isNumPad;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                dr = cmd.ExecuteReader();
                if (dr.Read() == true)
                {
                    frmLoc.Left = convertNullToInt(dr.GetInt32(dr.GetOrdinal("LocationLeft")).ToString());
                    frmLoc.Top = dr.GetInt32(dr.GetOrdinal("LocationTop"));
                    frmLoc.Width = dr.GetInt32(dr.GetOrdinal("LocationWidth"));
                    frmLoc.Height = dr.GetInt32(dr.GetOrdinal("LocationHeight"));
                }
                conn.Close();

                return frmLoc;
            }

            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
        }

        public static void SaveToolbarSettings(int POSTbr, int InvTbr, int AdminTbr)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                string sSQL = "";

                conn.ConnectionString = Configuration.ConnectionString;

                conn.Open();
                if (IsPadSettingExists(m_UserName))
                    sSQL = " UPDATE Util_Interface_Param Set isPOSTbr=" + POSTbr +
                        ", isInvTbr=" + InvTbr +
                        ", isAdminTbr=" + AdminTbr +
                        " WHERE userid= '" + m_UserName + "' ";
                else
                    sSQL = " INSERT INTO Util_Interface_Param (UserId ,isInvTbr ,isAdminTbr,isPOSTbr, Window_BackColor,Window_ForeColor, " +
                            " Button_BackColor1, Button_BackColor2, Button_ForeColor, , MoveToQtyCol, AllowAddItemInTrans " +
                            " , Active_BackColor, Active_ForeColor, Header_BackColor, Header_ForeColor) " +
                        " VALUES ('" + m_UserName + "'," + InvTbr + "," + AdminTbr + "," + POSTbr +
                        " '" + Window_Color + "', '" + Window_ForeColor +
                        "', '" + Window_Button_Color1 + "', '" + Window_Button_Color2 + "', '" + Window_Button_ForeColor +
                        "'," + moveToQtyCol.ToString() + "," + AllowAddItemInTrans.ToString() + "," +
                        " '" + Active_BackColor + "', '" + Active_ForeColor + "', '" + Header_BackColor + "', '" + Header_ForeColor + "' ) ";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                cmd.ExecuteNonQuery();
                conn.Close();
            }

            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
        }

        public static String Theme
        {
            get
            {
                return m_Theme;
            }
            set
            {
                m_Theme = value;
            }
        }

        public static String Window_Color
        {
            get
            {
                return m_Window_Color;
            }
            set
            {
                m_Window_Color = value;
            }
        }

        public static String CCProcessDB
        {
            get
            {
                return m_CCProcessDB;
            }
        }

        public static String Window_Button_Color1
        {
            get
            {
                return m_Window_Button_Color1;
            }
            set
            {
                m_Window_Button_Color1 = value;
            }
        }

        public static String Active_BackColor
        {
            get
            {
                return m_Active_BackColor;
            }
            set
            {
                m_Active_BackColor = value;
            }
        }

        public static Int32 LabelPerSheet
        {
            get
            {
                return m_LabelPerSheet;
            }
            set
            {
                m_LabelPerSheet = value;
            }
        }

        public static String Active_ForeColor
        {
            get
            {
                return m_Active_ForeColor;
            }
            set
            {
                m_Active_ForeColor = value;
            }
        }

        public static String Header_BackColor
        {
            get
            {
                return m_Header_BackColor;
            }
            set
            {
                m_Header_BackColor = value;
            }
        }

        public static String Header_ForeColor
        {
            get
            {
                return m_Header_ForeColor;
            }
            set
            {
                m_Header_ForeColor = value;
            }
        }

        public static String Window_Button_Color2
        {
            get
            {
                return m_Window_Button_Color2;
            }
            set
            {
                m_Window_Button_Color2 = value;
            }
        }

        public static String Window_ForeColor
        {
            get
            {
                return m_Window_ForeColor;
            }
            set
            {
                m_Window_ForeColor = value;
            }
        }

        public static String Window_Button_ForeColor
        {
            get
            {
                return m_Window_Button_ForeColor;
            }
            set
            {
                m_Window_Button_ForeColor = value;
            }
        }

        public static Int32 NumPad_Left
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings["NumPad_Left"]);
            }
            set
            {
                writeToConfiguration("NumPad_Left", Convert.ToString(value));
            }
        }

        /*		public static ConfigurationManager Settings
                {
                    get
                    {
                        if (oSettings ==null)
                            oSettings = new ConfigurationManager("PrimePOS");
                        return oSettings;
                    }
                }

        */

        public static Int32 NumPad_Top
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings["NumPad_Top"]);
            }
            set
            {
                writeToConfiguration("NumPad_Top", Convert.ToString(value));
            }
        }

        public static void writeToConfiguration(string strKey, string strValue)
        {
            XmlDocument oCXDoc = new XmlDocument();
            oCXDoc.Load("pos.exe.config");
            foreach (XmlNode oNode in oCXDoc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes)
            {
                if (oNode.Attributes.Item(0).Value == strKey)
                {
                    string str = oNode.Attributes.Item(1).Value;
                    oNode.Attributes.Item(1).Value = strValue;
                }
            }
            oCXDoc.Save("pos.exe.config");
        }

        public static bool moveToQtyCol
        {
            get
            {
                return m_moveToQtyCol;
            }
            set
            {
                m_moveToQtyCol = value;
            }
        }

        public static bool AllowAddItemInTrans
        {
            get
            {
                return m_AllowAddItemInTrans;
            }
            set
            {
                m_AllowAddItemInTrans = value;
            }
        }

        public static bool showNumPad
        {
            get
            {
                return m_showNumPad;
            }
            set
            {
                m_showNumPad = value;
            }
        }

        public static bool showItemPad
        {
            get
            {
                return m_showItemPad;
            }
            set
            {
                m_showItemPad = value;
            }
        }

        //Added by Atul 02-10-2010
        //PRIMEPOS-2865 15-Jul-2020 JY modified
        public static int AllowRxPicked
        {
            get
            {
                return m_AllowRxPicked;
            }
            set
            {
                m_AllowRxPicked = value;
            }
        }

        public static bool ShowPOSTbr
        {
            get
            {
                return m_POSTbr;
            }
        }

        public static bool ShowInvTbr
        {
            get
            {
                return m_InvTbr;
            }
        }

        public static bool ShowAdminTbr
        {
            get
            {
                return m_AdminTbr;
            }
        }

        public static String ApplicationName
        {
            get
            {
                return "Prime POS";
            }
        }

        public static String UserName
        {
            get
            {
                return m_UserName;
            }
            set
            {
                m_UserName = value;
            }
        }

        public static String Passward
        {
            get
            {
                return m_Passward;
            }
        }

        public static String ServerName
        {
            get
            {
                return m_ServerName;
            }
        }

        public static String DatabaseName
        {
            get
            {
                //return m_DatabaseName;
                return DBConfig.DatabaseName;
            }
        }

        public static string HostIPAddress
        {
            get
            {
                return m_HostIPAddress;
            }
        }

        public static String StationID
        {
            get
            {
                return m_StationID;
            }
            set
            {
                m_StationID = value;
            }
        }

        public static String StationName
        {
            get
            {
                return m_StationName;
            }
            set
            {
                m_StationName = value;
            }
        }

        public static Int32 DrawNo
        {
            get
            {
                return m_DrawNo;
            }
            set
            {
                //m_DrawNo = value; //PRIMEPOS-2638 07-Feb-2019 JY Commented
                #region PRIMEPOS-2638 07-Feb-2019 JY Added 
                if (value == 0)
                    m_DrawNo = 0;
                else if (value >= 3)
                    m_DrawNo = 2;
                else
                    m_DrawNo = value;
                #endregion
            }
        }

        public static Int32 SecurityLevel
        {
            get
            {
                return m_SecurityLevel;
            }
            set
            {
                m_SecurityLevel = value;
            }
        }

        public static Int32 ResetPwdDays
        {
            get
            {
                return m_ResetPwdDays;
            }
            set
            {
                m_ResetPwdDays = value;
            }
        }

        public static string ConnectionStringType
        {
            get
            {
                return S_ConnectionStringType;
            }
            set
            {
                S_ConnectionStringType = value;
            }
        }

        public static string SQLUserID
        {
            get
            {
                return S_SQLUserID;
            }
            set
            {
                S_SQLUserID = value;
            }
        }

        public static string SQLUserPassword
        {
            get
            {
                return S_SQLUserPassword;
            }
            set
            {
                S_SQLUserPassword = value;
            }
        }

        public static Decimal UserMaxDiscountLimit
        {
            get
            {
                return D_MaxDiscountLimit;
            }
            set
            {
                D_MaxDiscountLimit = value;
            }
        }

        /// <summary>
        /// Added By shitaljit on 2/11/2014 for JIRA PRIMEPOS-1810
        /// To store login users IsDiscountOverride property.
        /// </summary>
        public static bool IsDiscOverridefromPOSTrans
        {
            get
            {
                return B_IsDiscountOverride;
            }
            set
            {
                B_IsDiscountOverride = value;
            }
        }
        //Following Code Added By Ravindra(QuicSolv) for Max Transaction Limit
        public static Decimal UserMaxTransactionLimit
        {
            get
            {
                return D_MaxTransactionLimit;
            }
            set
            {
                D_MaxTransactionLimit = value;
            }
        }

        //Till here added by Ravindra

        //Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
        public static Decimal UserMaxReturnTransLimit
        {
            get
            {
                return dMaxReturnTransLimit;
            }
            set
            {
                dMaxReturnTransLimit = value;
            }
        }

        //Sprint-25 - PRIMEPOS-2411 20-Apr-2017 JY Added 
        public static Decimal UserMaxTenderedAmountLimit
        {
            get
            {
                return dMaxTenderedAmountLimit;
            }
            set
            {
                dMaxTenderedAmountLimit = value;
            }
        }

        //PRIMEPOS-2741 25-Sep-2019 JY Added
        public static Decimal UserMaxCashbackLimit
        {
            get
            {
                return dMaxCashbackLimit;
            }
            set
            {
                dMaxCashbackLimit = value;
            }
        }

        //PRIMEPOS-189 02-Aug-2021 JY Added
        public static Decimal HourlyRate
        {
            get
            {
                return dHourlyRate;
            }
            set
            {
                dHourlyRate = value;
            }
        }

        //12-Sep-2016 JY Added to preserve ID 
        public static string AuthorizationNo
        {
            get
            {
                return sAuthorizationNo;
            }
            set
            {
                sAuthorizationNo = value;
            }
        }

        //PRIMEPOS-2729 06-Sep-2019 JY Added
        public static string strDOB
        {
            get
            {
                return sDOB;
            }
            set
            {
                sDOB = value;
            }
        }

        // PRIMEPOS-2664 ADDED BY ARVIND EVERTEC
        public static string CashierID
        {
            get
            {
                return sCashierID;
            }
            set
            {
                sCashierID = value;
            }
        }

        private Configuration()
        {
            buildConnectionString();
        }

        private static void buildConnectionString()
        {
            Assembly a;
            a = Assembly.GetExecutingAssembly();
            AssemblyName[] b = a.GetReferencedAssemblies();
            foreach (AssemblyName obj in b)
            {
                if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                {
                    Assembly an;
                    an = Assembly.Load(obj.FullName);

                    if (ConnectionStringType == "MasterDatabase")
                    {
                        m_DBUser = "MMSI";
                        m_Passward = "MMSPhW110";
                    }
                    else
                    {
                        if (ConnectionStringType == "sa")
                        {
                            m_DBUser = "sa";
                            m_Passward = "MMSPhW110";
                        }
                        else
                        {
                            m_DBUser = SQLUserID;
                            m_Passward = SQLUserPassword;
                        }
                    }
                    m_ServerName = ConfigurationSettings.AppSettings["ServerName"];
                    m_DatabaseName = ConfigurationSettings.AppSettings["DataBase"];
                    //m_StationID= ConfigurationSettings.AppSettings["StationID"];
                    m_LabelPerSheet = Convert.ToInt32(convertNullToInt(ConfigurationSettings.AppSettings["LabelPerSheet"]));
                    m_CCProcessDB = ConfigurationSettings.AppSettings["CCProcessDB"];
                    if (ConfigurationSettings.AppSettings["HostIPAddress"] != null)
                    {
                        m_HostIPAddress = ConfigurationSettings.AppSettings["HostIPAddress"];
                    }
                    break;
                }
            }

            m_ConnString = String.Concat("server=", m_ServerName, ";Database=", m_DatabaseName, ";User ID =", m_DBUser, ";Password =", m_Passward, ";");
            m_ConnString = m_ConnString + ";Max Pool Size=60;Min Pool Size=5;Pooling=True;";

            if (ConnectionStringType == "MasterDatabase")
                m_ConnStringMaster = m_ConnString;

            //GetUserSettings("");
        }

        /// <summary>
        /// Author: Shitaljit
        /// Date: 12/2/2013
        /// Created to get the path from where PrimePOS exe is running.
        /// </summary>
        /// <returns></returns>
        public static void GetPOSWDirectoryPath()
        {
            try
            {
                string sPath = System.Windows.Forms.Application.ExecutablePath;
                System.IO.FileInfo FI = new System.IO.FileInfo(sPath);
                S_POSWDirectoryPath = FI.Directory.FullName;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #region PRIMEPOS-3540
        public static string WebViewPath
        {
            get
            {
                return $"POS.exe.WebView2/{Environment.MachineName}/{Environment.UserName}/{StationID}";
            }
            set
            {
                WebViewPath = value;
            }
        }
        #endregion

        #region PRIMEPOS-2739 
        public static Dictionary<string, string> getSettingDetail()
        {
            Dictionary<string, string> dicsetting = new Dictionary<string, string>();
            try
            {
                Search oSearch = new Search();
                DataSet oDS = oSearch.SearchData("select SettingID, FieldName,FieldValue,SettingFieldID from SettingDetail");
                DataTable dt = new DataTable();
                dt = oDS.Tables[0];
                if (oDS.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        dicsetting.Add(row["FieldName"].ToString(), row["FieldValue"] as string);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dicsetting;
        }
        #endregion

        public static void GetUserSettings(string UserID)
        {
            try
            {
                //Following variable string companyTelNo is Added By Shitaljit(QuicSolv) on 16 May 2011
                string companyTelNo;
                PharmData.PharmBL opharm = new PharmData.PharmBL(); //PRIMEPOS-3207
                Search oSearch = new Search();
                DataSet oDS = oSearch.SearchData("Select * from Util_Interface_Param where userid='" + UserID + "'");

                #region set preferences

                if (oDS != null)
                {
                    if (oDS.Tables[0].Rows.Count > 0)
                    {
                        m_Window_Color = oDS.Tables[0].Rows[0]["Window_Backcolor"].ToString();
                        m_Window_ForeColor = oDS.Tables[0].Rows[0]["Window_ForeColor"].ToString();
                        m_Window_Button_Color1 = oDS.Tables[0].Rows[0]["Button_BackColor1"].ToString();
                        m_Window_Button_Color2 = oDS.Tables[0].Rows[0]["Button_BackColor2"].ToString();
                        m_Window_Button_ForeColor = oDS.Tables[0].Rows[0]["Button_ForeColor"].ToString();
                        m_Active_BackColor = oDS.Tables[0].Rows[0]["Active_BackColor"].ToString();
                        m_Active_ForeColor = oDS.Tables[0].Rows[0]["Active_ForeColor"].ToString();

                        m_Header_BackColor = oDS.Tables[0].Rows[0]["Header_BackColor"].ToString();
                        m_Header_ForeColor = oDS.Tables[0].Rows[0]["Header_ForeColor"].ToString();
                        m_Theme = oDS.Tables[0].Rows[0]["Theme"].ToString(); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018

                        if (Convert.ToInt32(oDS.Tables[0].Rows[0]["MoveToQtyCol"].ToString()) == 1)
                        {
                            m_moveToQtyCol = true;
                        }
                        else
                        {
                            m_moveToQtyCol = false;
                        }
                        if (Convert.ToInt32(oDS.Tables[0].Rows[0]["AllowAddItemInTrans"].ToString()) == 1)
                        {
                            m_AllowAddItemInTrans = true;
                        }
                        else
                        {
                            m_AllowAddItemInTrans = false;
                        }

                        if (Convert.ToInt32(oDS.Tables[0].Rows[0]["isPOSTbr"].ToString()) == 1)
                        {
                            m_POSTbr = true;
                        }
                        else
                        {
                            m_POSTbr = false;
                        }

                        if (Convert.ToInt32(oDS.Tables[0].Rows[0]["isInvTbr"].ToString()) == 1)
                        {
                            m_InvTbr = true;
                        }
                        else
                        {
                            m_InvTbr = false;
                        }

                        if (Convert.ToInt32(oDS.Tables[0].Rows[0]["isAdminTbr"].ToString()) == 1)
                        {
                            m_AdminTbr = true;
                        }
                        else
                        {
                            m_AdminTbr = false;
                        }

                        if (oDS.Tables[0].Rows[0]["showNumPad"] == DBNull.Value)
                        {
                            m_showNumPad = false;
                        }
                        else if (Convert.ToInt32(oDS.Tables[0].Rows[0]["showNumPad"].ToString()) == 1)
                        {
                            m_showNumPad = true;
                        }
                        else
                        {
                            m_showNumPad = false;
                        }

                        if (oDS.Tables[0].Rows[0]["showItemPad"] == DBNull.Value)
                        {
                            m_showItemPad = false;
                        }
                        else if (Convert.ToInt32(oDS.Tables[0].Rows[0]["showItemPad"].ToString()) == 1)
                        {
                            m_showItemPad = true;
                        }
                        else
                        {
                            m_showItemPad = false;
                        }
                    }
                    else
                    {
                        setDefaultPref();
                    }
                }
                else
                {
                    setDefaultPref();
                }

                #endregion set preferences

                oDS = null;
                oDS = oSearch.SearchData("select * from Util_Company_Info");
                if (oDS.Tables[0].Rows.Count > 0)
                {
                    #region set company info

                    CInfo = new CompanyInfo();

                    CInfo.PWEncrypted = convertNullToBoolean(oDS.Tables[0].Rows[0]["PWEncrypted"].ToString());
                    CInfo.StoreID = convertNullToString(oDS.Tables[0].Rows[0]["StoreID"].ToString());
                    CInfo.StoreName = convertNullToString(oDS.Tables[0].Rows[0]["CompanyName"].ToString());
                    CInfo.Address = convertNullToString(oDS.Tables[0].Rows[0]["Address"].ToString());
                    CInfo.City = convertNullToString(oDS.Tables[0].Rows[0]["City"].ToString());
                    CInfo.State = convertNullToString(oDS.Tables[0].Rows[0]["State"].ToString());
                    CInfo.Zip = convertNullToString(oDS.Tables[0].Rows[0]["Zip"].ToString());
                    //Modified By Shitaljit(QuicSolv) on 16 May 2011(add the code to set the Telephone no to the format (201)337-7300))
                    ////CInfo.Telephone = convertNullToString(oDS.Tables[0].Rows[0]["Telephone"].ToString()); Original Code
                    companyTelNo = convertNullToString(oDS.Tables[0].Rows[0]["Telephone"].ToString());
                    //Commented By Shitaljit(QuicSolv) on 14 July 2011
                    //CInfo.Telephone = "(" + companyTelNo.Substring(0, 3) + ")" + companyTelNo.Substring(3, 3) + "-" + companyTelNo.Substring(6, 4);
                    CInfo.Telephone = FormateCompanyTelNo(companyTelNo);
                    CInfo.ReceiptMSG = convertNullToString(oDS.Tables[0].Rows[0]["ReceiptMSG"].ToString());
                    CInfo.NoOfReceipt = Convert.ToInt32(oDS.Tables[0].Rows[0]["NoOfReceipt"].ToString());
                    CInfo.NoOfOnHoldTransReceipt = Configuration.convertNullToInt(oDS.Tables[0].Rows[0]["NoOfOnHoldTransReceipt"].ToString());
                    CInfo.NoOfCC = Convert.ToInt32(oDS.Tables[0].Rows[0]["NoOfCC"].ToString());
                    CInfo.NoOfHCRC = Convert.ToInt32(oDS.Tables[0].Rows[0]["NoOfHCRC"].ToString());
                    CInfo.NoOfRARC = Convert.ToInt32(oDS.Tables[0].Rows[0]["NoOfRARC"].ToString());
                    CInfo.MerchantNo = convertNullToString(oDS.Tables[0].Rows[0]["MerchantNo"].ToString());
                    CInfo.PriceItemQualifier = convertNullToString(oDS.Tables[0].Rows[0]["PriceItemQualifier"].ToString());
                    CInfo.PriceQualifier = convertNullToString(oDS.Tables[0].Rows[0]["PriceQualifier"].ToString());
                    CInfo.SigType = convertNullToString(oDS.Tables[0].Rows[0]["SigType"].ToString());
                    CInfo.PrivacyText = convertNullToString(oDS.Tables[0].Rows[0]["PrivacyText"].ToString());
                    CInfo.PatientCounceling = convertNullToString(oDS.Tables[0].Rows[0]["PatientCounceling"].ToString());
                    CInfo.PrivacyExpiry = Convert.ToInt32(oDS.Tables[0].Rows[0]["PrivacyExpiry"].ToString());
                    CInfo.RemoteDBServer = convertNullToString(oDS.Tables[0].Rows[0]["RemoteDBServer"].ToString());
                    CInfo.RemoteCatalog = convertNullToString(oDS.Tables[0].Rows[0]["RemoteCatalog"].ToString());
                    CInfo.UseRemoteServer = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseRemoteServer"].ToString());
                    CInfo.AllowUnPickedRX = convertNullToString(oDS.Tables[0].Rows[0]["AllowUnPickedRX"].ToString());
                    CInfo.UnPickedRXSearchDays = convertNullToInt(oDS.Tables[0].Rows[0]["UnPickedRXSearchDays"].ToString());
                    CInfo.CheckRXItemsForIIAS = convertNullToBoolean(oDS.Tables[0].Rows[0]["CheckRXItemsForIIAS"].ToString());
                    CInfo.AllowVerifiedRXOnly = convertNullToInt(oDS.Tables[0].Rows[0]["AllowVerifiedRXOnly"].ToString());  //PRIMEPOS-2593 23-Jun-2020 JY modified
                    CInfo.GroupTransItems = convertNullToBoolean(oDS.Tables[0].Rows[0]["GroupTransItems"].ToString());
                    CInfo.ForceCustomerInTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["ForceCustomerInTrans"].ToString());
                    //CInfo.ConfirmPatient = convertNullToBoolean(oDS.Tables[0].Rows[0]["ConfirmPatient"].ToString());//Added by Krishna on 20 August 2012  //PRIMEPOS-2317 15-Mar-2019 JY Commented
                    CInfo.ConfirmPatient = convertNullToInt(oDS.Tables[0].Rows[0]["ConfirmPatient"].ToString());    //PRIMEPOS-2317 15-Mar-2019 JY Added
                    CInfo.PromptForSellingPriceLessThanCost = convertNullToBoolean(oDS.Tables[0].Rows[0]["PromptForSellingPriceLessThanCost"].ToString());  //Added by Ravindra on 20 March 2013
                    CInfo.AllowItemComboPrice = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowItemComboPrice"].ToString());  //Added by Ravindra on 20 March 2013
                    //Added by SRT(Ritesh Parekh) Date: 28-Aug-2009
                    CInfo.DefaultDeptId = convertNullToInt(oDS.Tables[0].Rows[0]["DefaultDeptId"].ToString());
                    //End Of Added By SRT(Ritesh Parekh)
                    //Added By Shitaljit(QuicSolv)
                    //Modified By Shitaljit(QuicSolv) on 28 July 2011
                    //Added UserID!= ""
                    Department oDept = new Department();
                    if ((CInfo.DefaultDeptId == 0 && UserID != "") || Configuration.isNullOrEmptyDataSet(oDept.Populate(CInfo.DefaultDeptId)) == true)
                    {
                        SetDefaultDepartment();
                    }

                    //Added by Prog1 16Jun2009
                    CInfo.PostDescOnlyInHC = convertNullToBoolean(oDS.Tables[0].Rows[0]["PostDescOnlyInHC"].ToString());

                    //Added by Prog1 23dec2009
                    CInfo.WarnMultiPatientRX = convertNullToBoolean(oDS.Tables[0].Rows[0]["WarnMultiPatientRX"].ToString());
                    CInfo.ChargeCashBackMode = convertNullToString(oDS.Tables[0].Rows[0]["ChargeCashBackMode"].ToString());
                    CInfo.AllowCashBack = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowCashBack"].ToString());
                    CInfo.PostRXNumberOnlyInHC = convertNullToBoolean(oDS.Tables[0].Rows[0]["PostRXNumberOnlyInHC"].ToString());
                    CInfo.PrintReceiptForOnHoldTrans = convertNullToString(oDS.Tables[0].Rows[0]["PrintReceiptForOnHoldTrans"].ToString());
                    //Start : Added By Amit 19 April 2011
                    CInfo.PrintStCloseNo = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["PrintStCloseNo"].ToString());
                    CInfo.PrintEODNo = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["PrintEODNo"].ToString());
                    CInfo.ByPassPayScreen = convertNullToBoolean(oDS.Tables[0].Rows[0]["ByPassPayScreen"].ToString()); //Added by Manoj 9/26/2013
                    CInfo.MergeSign = convertNullToBoolean(oDS.Tables[0].Rows[0]["MergeSign"].ToString());//Added by Manoj 10/01/2013

                    CInfo.ShowOnlyOneCCType = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowOnlyOneCCType"].ToString());
                    CInfo.ShowTextPrediction = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowTextPrediction"].ToString());//added on 10 Aug 2012
                    //End
                    //Added By Shitaljit(QuicSolv) on 3 June 2011
                    CInfo.UseCashManagement = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseCashManagement"].ToString());
                    CInfo.DefalutInvRecievedID = convertNullToInt(oDS.Tables[0].Rows[0]["DefInvRecvId"].ToString());
                    CInfo.DefaultInvReturnID = convertNullToInt(oDS.Tables[0].Rows[0]["DefInvRetId"].ToString());
                    CInfo.AllowMultipleInstanceOfPOS = convertNullToInt(oDS.Tables[0].Rows[0]["AllowMultipleInstanceOfPOS"].ToString());    //PRIMEPOS-2936 21-Jan-2021 JY modified
                    CInfo.DefCDStartBalance = convertNullToDecimal(oDS.Tables[0].Rows[0]["DefCDStartBalance"].ToString());//Added on 15 June 2011
                    if ((CInfo.DefaultInvReturnID == 0 || CInfo.DefalutInvRecievedID == 0) && UserID != "")
                    {
                        SetDefaultTransType();
                    }
                    CInfo.AllowHundredPerInvDiscount = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowHundredPerInvDiscount"].ToString());//Added By shitaljit on 7 Dec 2011
                    CInfo.InvDiscToDiscountableItemOnly = convertNullToBoolean(oDS.Tables[0].Rows[0]["InvDiscToDiscountableItemOnly"].ToString());//Added By shitaljit on 27 Dec 2011;
                    //Till here aded by Shitaljit.
                    CInfo.DaysForWarn = convertNullToInt(oDS.Tables[0].Rows[0]["DaysForWarn"].ToString());
                    //Added by Amit Date 23 Nov 20111
                    CInfo.TagFSA = convertNullToBoolean(oDS.Tables[0].Rows[0]["TagFSA"].ToString());
                    CInfo.TagTaxable = convertNullToBoolean(oDS.Tables[0].Rows[0]["TagTaxable"].ToString());
                    CInfo.TagMonitored = convertNullToBoolean(oDS.Tables[0].Rows[0]["TagMonitored"].ToString());
                    CInfo.TagEBT = convertNullToBoolean(oDS.Tables[0].Rows[0]["TagEBT"].ToString());
                    //End
                    CInfo.AllowDiscountOfItemsOnSale = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowDiscountOfItemsOnSale"].ToString());//Added By shitaljit on 17 Feb 2012
                    CInfo.ByPassPayScreen = convertNullToBoolean(oDS.Tables[0].Rows[0]["ByPassPayScreen"].ToString()); //Added by Manoj 9/26/2013
                    CInfo.MergeSign = convertNullToBoolean(oDS.Tables[0].Rows[0]["MergeSign"].ToString());//Added by Manoj 10/01/2013
                    //CInfo.NoOfCheckRC = Configuration.convertNullToInt(oDS.Tables[0].Rows[0]["NoOfCheckRC"].ToString());//Added By shitaljit on 13 March 2012
                    CInfo.AllowMultipleRXRefillsInSameTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowMultipleRXRefillsInSameTrans"].ToString());
                    CInfo.EBTAsManualTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["EBTAsManualTrans"].ToString());//Added By shitaljit on 4 April 2012
                    CInfo.AutoImportCustAtTrans = convertNullToInt(oDS.Tables[0].Rows[0]["AutoImportCustAtTrans"].ToString());//Added By shitaljit on 23 April 2012 //PRIMEPOS-2886 25-Sep-2020 JY modified
                    //CInfo.NoOfCashReceipts = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfCashReceipts"].ToString());
                    CInfo.OtcPrivacyNotice = convertNullToString(oDS.Tables[0].Rows[0]["MonitorCatSignText"].ToString()); //Added by Manoj on 5/15/2012
                    //CInfo.NoOfGiftReceipt = Configuration.convertNullToInt(oDS.Tables[0].Rows[0]["NoOfGiftReceipt"].ToString());//Added By shitaljit on 9 jan 2013
                    if (String.IsNullOrEmpty(oDS.Tables[0].Rows[0]["PrintReceipt"].ToString()) == false)
                    {
                        CInfo.PrintReceipt = Convert.ToChar(oDS.Tables[0].Rows[0]["PrintReceipt"].ToString().ToCharArray()[0]);
                    }
                    else
                    {
                        CInfo.PrintReceipt = 'Y';
                    }

                    CInfo.AllowPrintZeroTrans = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["AllowPrintZeroTrans"].ToString());
                    CInfo.DoNotOpenDrawerForCCOnlyTrans = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["DoNotOpenDrawerForCCOnlyTrans"].ToString());
                    CInfo.UseSameThreadToVerifySysDateTime = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["UseSameThreadToVerifySysDateTime"].ToString());//Added By shitaljit on 12 sept 2012.
                    CInfo.EnforceComplexPassword = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["EnforceComplexPassword"].ToString());//Added By shitaljit on 12 sept 2012.
                    CInfo.DefaultTaxableItem = convertNullToString(oDS.Tables[0].Rows[0]["DefaultTaxableItem"].ToString());//Added By shitaljit on 16 May 2013.
                    CInfo.DefaultNonTaxableItem = convertNullToString(oDS.Tables[0].Rows[0]["DefaultNonTaxableItem"].ToString());//Added By shitaljit on 16 May 2013.
                    CInfo.OpenDrawerForZeroAmtTrans = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["OpenDrawerForZeroAmtTrans"].ToString());//Added By shitaljit on 7 July 2013.
                    //Added By shitaljit on 19 July 2013. PRIMEPOS-1235 Add Preference to control Updating patient data from PrimeRX during transaction.
                    CInfo.UpdatePatientData = convertNullToString(oDS.Tables[0].Rows[0]["UpdatePatientData"].ToString());
                    CInfo.AutoPopulateCLCardDetails = convertNullToBoolean(oDS.Tables[0].Rows[0]["AutoPopulateCLCardDetails"].ToString());//added By shitaljit on 25July2013 for PrimePOS-436 Loyalty cards could be displayed for Customer during payment

                    CInfo.DoNotOpenDrawerForChequeTrans = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["DoNotOpenDrawerForChequeTrans"].ToString()); //Added By Ravindra 10/01/2014 

                    CInfo.PreventRxMaxFillDayLimit = convertNullToInt(oDS.Tables[0].Rows[0]["PreventRxMaxFillDayLimit"].ToString()); //Added By Ravindra 10/07/2014 ;
                    CInfo.PreventRxMaxFillDayNotifyAction = convertNullToInt(oDS.Tables[0].Rows[0]["PreventRxMaxFillDayNotifyAction"].ToString()); //Added By Ravindra 10/07/2014 ;

                    #region HPS Sftp info - Added By Manoj

                    CInfo.HpsSftpHost = convertNullToString(oDS.Tables[0].Rows[0]["HpsSftpHost"].ToString());
                    CInfo.HpsSftpPort = convertNullToInt(oDS.Tables[0].Rows[0]["HpsSftpPort"].ToString());
                    CInfo.HpsSftpUser = convertNullToString(oDS.Tables[0].Rows[0]["HpsSftpUser"].ToString());
                    CInfo.HpsSftpPassword = convertNullToString(oDS.Tables[0].Rows[0]["HpsSftpPassword"].ToString());

                    #endregion HPS Sftp info - Added By Manoj

                    CInfo.OutGoingEmailBody = convertNullToString(oDS.Tables[0].Rows[0]["OutGoingEmailBody"].ToString());//Added By Ravindra 
                    CInfo.OutGoingEmailEnableSSL = convertNullToBoolean(oDS.Tables[0].Rows[0]["OutGoingEmailEnableSSL"].ToString()); ;//Added By Ravindra 
                    CInfo.OutGoingEmailID = convertNullToString(oDS.Tables[0].Rows[0]["OutGoingEmailID"].ToString()); ;//Added By Ravindra 
                    CInfo.OwnersEmailId = convertNullToString(oDS.Tables[0].Rows[0]["OwnersEmailId"].ToString());   //Sprint-24 - PRIMEPOS-2363 28-Dec-2016 JY Added
                    CInfo.AutoEmailStationCloseReport = convertNullToBoolean(oDS.Tables[0].Rows[0]["AutoEmailStationCloseReport"].ToString());    //Sprint-24 - PRIMEPOS-2363 27-Jan-2017 JY Added
                    CInfo.AutoEmailEODReport = convertNullToBoolean(oDS.Tables[0].Rows[0]["AutoEmailEODReport"].ToString());    //Sprint-24 - PRIMEPOS-2363 27-Jan-2017 JY Added
                    CInfo.OutGoingEmailPass = convertNullToString(oDS.Tables[0].Rows[0]["OutGoingEmailPass"].ToString()); ;//Added By Ravindra 
                    CInfo.OutGoingEmailPort = Configuration.convertNullToInt(oDS.Tables[0].Rows[0]["OutGoingEmailPort"].ToString());
                    CInfo.OutGoingEmailServer = convertNullToString(oDS.Tables[0].Rows[0]["OutGoingEmailServer"].ToString()); ;//Added By Ravindra 
                    CInfo.OutGoingEmailSignature = convertNullToString(oDS.Tables[0].Rows[0]["OutGoingEmailSignature"].ToString()); ;//Added By Ravindra 
                    CInfo.OutGoingEmailSubject = convertNullToString(oDS.Tables[0].Rows[0]["OutGoingEmailSubject"].ToString()); ;//Added By Ravindra 
                    CInfo.OutGoingEmailUserID = convertNullToString(oDS.Tables[0].Rows[0]["OutGoingEmailUserID"].ToString()); ;//Added By Ravindra 
                    CInfo.UseEmailInvoice = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseEmailInvoice"].ToString()); ;//Added By Ravindra 
                    CInfo.OutGoingEmailPromptAutomatically = convertNullToBoolean(oDS.Tables[0].Rows[0]["OutGoingEmailPromptAutomatically"].ToString());//Added By Ravindra 
                    //Added By shitaljit for JIRA- PRIMEPOS-1652 Add preference to manage Promotional coupon discount to abide with discount settings
                    CInfo.ApplyInvDiscSettingsForCoupon = convertNullToBoolean(oDS.Tables[0].Rows[0]["ApplyInvDiscSettingsForCoupon"].ToString());
                    //END OutGoingEmailPromptAutomatically

                    /*Date 29/01/2014
                     * Modified by Shitaljit
                     * Change currency from Char to string
                     */
                    //new code
                    CInfo.CurrencySymbol = convertNullToString(oDS.Tables[0].Rows[0]["CurrencySymbol"].ToString());
                    CInfo.CurrencySymbol = (string.IsNullOrEmpty(CInfo.CurrencySymbol)) ? "$" : CInfo.CurrencySymbol;
                    //old code
                    //CInfo.CurrencySymbol = Convert.ToChar(oDS.Tables[0].Rows[0]["CurrencySymbol"].ToString());
                    //Added By Shitaljit on 6/2/2104 for PRIMEPOS-1816 Ability to turn on\off delivery prompt
                    CInfo.WarnForRXDelivery = convertNullToBoolean(oDS.Tables[0].Rows[0]["WarnForRXDelivery"].ToString());
                    //END
                    //Added By Shitaljit on 6/2/2104 for PRIMEPOS-1804 Auto Populate Email address from customer
                    CInfo.AutoPopulateCustEmail = convertNullToBoolean(oDS.Tables[0].Rows[0]["AutoPopulateCustEmail"].ToString());
                    //END
                    #region IVU Loto
                    CInfo.IVULottoMerchantID = convertNullToString(oDS.Tables[0].Rows[0]["IVULottoMerchantID"].ToString());
                    CInfo.IVULottoCommunicationMode = convertNullToString(oDS.Tables[0].Rows[0]["IVULottoSetUpMode"].ToString());
                    CInfo.UseIVULottoProgram = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseIVULottoProgram"].ToString());
                    #endregion

                    CInfo.PromptForZeroSellingPrice = convertNullToBoolean(oDS.Tables[0].Rows[0]["PromptForZeroSellingPrice"].ToString());
                    CInfo.PromptForZeroCostPrice = convertNullToBoolean(oDS.Tables[0].Rows[0]["PromptForZeroCostPrice"].ToString());
                    CInfo.DoNotOpenDrawerForHouseChargeOnlyTrans = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["DoNotOpenDrawerForHouseChargeOnlyTrans"].ToString());    //Sprint-19 - 2161 27-Mar-2015 JY Added 

                    #region Sprint-20 26-May-2015 Auto updater JY Added settings for auto updater
                    CInfo.AllowAutomaticUpdates = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["AllowAutomaticUpdates"].ToString());
                    CInfo.AllowRunningUpdates = convertNullToBooleanTrue(oDS.Tables[0].Rows[0]["AllowRunningUpdates"].ToString());
                    CInfo.AutoUpdateServiceAddress = convertNullToString(oDS.Tables[0].Rows[0]["AutoUpdateServiceAddress"].ToString());
                    CInfo.RunningTasksTimerInterval = convertNullToInt(oDS.Tables[0].Rows[0]["RunningTasksTimerInterval"].ToString());
                    #endregion

                    #region Sprint-23 - PRIMEPOS-2029 11-Apr-2016 JY Added
                    CInfo.useNplex = convertNullToBoolean(oDS.Tables[0].Rows[0]["Nplex"].ToString());
                    CInfo.nplexStoreId = convertNullToString(oDS.Tables[0].Rows[0]["nplexStoreId"].ToString());
                    CInfo.StoreSiteId = convertNullToString(oDS.Tables[0].Rows[0]["StoreSiteId"].ToString());
                    CInfo.postSaleInd = convertNullToBoolean(oDS.Tables[0].Rows[0]["postSaleInd"].ToString());
                    #endregion

                    #region Sprint-23 - PRIMEPOS-2244 19-May-2016 JY Added 
                    CInfo.PrintStationCloseDateTime = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintStationCloseDateTime"].ToString());
                    CInfo.PrintEODDateTime = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintEODDateTime"].ToString());
                    #endregion
                    CInfo.SearchRxsWithPatientName = convertNullToBoolean(oDS.Tables[0].Rows[0]["SearchRxsWithPatientName"].ToString());    //Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
                    CInfo.FetchFamilyRx = convertNullToBoolean(oDS.Tables[0].Rows[0]["FetchFamilyRx"].ToString());  //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added
                    CInfo.SaveCCToken = convertNullToBoolean(oDS.Tables[0].Rows[0]["SaveCCToken"].ToString());    //Sprint-23 - PRIMEPOS-2313 09-Jun-2016 JY Added

                    CInfo.AllowZeroSellingPrice = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowZeroSellingPrice"].ToString());    //Sprint-21 - 2204 26-Jun-2015 JY Added
                    CInfo.RestrictInActiveItem = convertNullToBoolean(oDS.Tables[0].Rows[0]["RestrictInActiveItem"].ToString());    //Sprint-21 - 2173 10-Jul-2015 JY Added
                    CInfo.PrintReceiptInMultipleLanguage = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintReceiptInMultipleLanguage"].ToString());    //Sprint-21 - 1272 25-Aug-2015 JY Added
                    CInfo.ConsiderItemType = convertNullToBoolean(oDS.Tables[0].Rows[0]["ConsiderItemType"].ToString());    //Sprint-22 16-Dec-2015 JY Added settings to control the ItemType behavior 
                    #endregion set company info

                    #region Sprint-24 - PRIMEPOS-2344 02-Dec-2016 JY Added
                    CInfo.IIASFTPAddress = convertNullToString(oDS.Tables[0].Rows[0]["IIASFTPAddress"].ToString());
                    CInfo.IIASFTPUserId = convertNullToString(oDS.Tables[0].Rows[0]["IIASFTPUserId"].ToString());
                    CInfo.IIASFTPPassword = convertNullToString(oDS.Tables[0].Rows[0]["IIASFTPPassword"].ToString());
                    CInfo.IIASFileName = convertNullToString(oDS.Tables[0].Rows[0]["IIASFileName"].ToString());
                    CInfo.IIASDownloadInterval = convertNullToInt(oDS.Tables[0].Rows[0]["IIASDownloadInterval"].ToString());
                    if (oDS.Tables[0].Rows[0]["IIASFileModifiedDateOnFTP"].ToString() != string.Empty)
                    {
                        try
                        {
                            CInfo.IIASFileModifiedDateOnFTP = Convert.ToDateTime(oDS.Tables[0].Rows[0]["IIASFileModifiedDateOnFTP"]);
                        }
                        catch
                        {
                            CInfo.IIASFileModifiedDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");
                        }
                    }
                    else
                        CInfo.IIASFileModifiedDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    #endregion

                    #region Sprint-25 - PRIMEPOS-2379 07-Feb-2017 JY Added
                    CInfo.PSEFTPAddress = convertNullToString(oDS.Tables[0].Rows[0]["PSEFTPAddress"].ToString());
                    CInfo.PSEFTPUserId = convertNullToString(oDS.Tables[0].Rows[0]["PSEFTPUserId"].ToString());
                    CInfo.PSEFTPPassword = convertNullToString(oDS.Tables[0].Rows[0]["PSEFTPPassword"].ToString());
                    CInfo.PSEFileName = convertNullToString(oDS.Tables[0].Rows[0]["PSEFileName"].ToString());
                    CInfo.PSEDownloadInterval = convertNullToInt(oDS.Tables[0].Rows[0]["PSEDownloadInterval"].ToString());
                    CInfo.UpdatePSEItem = convertNullToBoolean(oDS.Tables[0].Rows[0]["UpdatePSEItem"].ToString());
                    if (oDS.Tables[0].Rows[0]["PSEFileModifiedDateOnFTP"].ToString() != string.Empty)
                    {
                        try
                        {
                            CInfo.PSEFileModifiedDateOnFTP = Convert.ToDateTime(oDS.Tables[0].Rows[0]["PSEFileModifiedDateOnFTP"]);
                        }
                        catch
                        {
                            CInfo.PSEFileModifiedDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");
                        }
                    }
                    else
                        CInfo.PSEFileModifiedDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    #endregion
                    CInfo.DefaultCustomerTokenValue = convertNullToBoolean(oDS.Tables[0].Rows[0]["DefaultCustomerTokenValue"].ToString());  //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added

                    #region PRIMEPOS-2643 05-Sep-2019
                    CInfo.PIEnable = convertNullToBoolean(oDS.Tables[0].Rows[0]["IsEnablePrimeInterface"].ToString());
                    CInfo.PIURL = convertNullToString(oDS.Tables[0].Rows[0]["PrimeInterfaceUrl"].ToString());
                    CInfo.PUser = convertNullToString(oDS.Tables[0].Rows[0]["PrimeInterfaceUser"].ToString());

                    MMS.Encryption.NativeEncryption.DecryptText(oDS.Tables[0].Rows[0]["PrimeInterfacePassword"].ToString(), ref CInfo.PPassword);
                    //CInfo.PPassword = convertNullToString(oDS.Tables[0].Rows[0]["PrimeInterfacePassword"].ToString());
                    #endregion

                    #region PRIMEPOS-2442 ADDED BT ROHIT NAIR
                    CInfo.EnableConsentCapture = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnableConsentCapture"].ToString());
                    CInfo.SelectedConsentSource = convertNullToString(oDS.Tables[0].Rows[0]["SelectedConsentSource"].ToString());
                    #endregion

                    #region PrimePOS-2448 Added BY Rohit Nair
                    CInfo.EnableIntakeBatch = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnableIntakeBatch"].ToString());
                    CInfo.SkipSignatureForInatkeBatch = convertNullToBoolean(oDS.Tables[0].Rows[0]["SkipSignatureForInatkeBatch"].ToString());
                    CInfo.IntakeBatchMarkAsPickedup = convertNullToBoolean(oDS.Tables[0].Rows[0]["IntakeBatchMarkAsPickedup"].ToString());
                    CInfo.IntakeBatchCode = convertNullToString(oDS.Tables[0].Rows[0]["IntakeBatchCode"].ToString());
                    CInfo.IntakeBatchStatus = convertNullToString(oDS.Tables[0].Rows[0]["IntakeBatchStatus"].ToString());//PrimePOS-2518 Jenny Added
                    #endregion

                    CInfo.PromptForPartialPayment = convertNullToBoolean(oDS.Tables[0].Rows[0]["PromptForPartialPayment"].ToString());  //PRIMEPOS-2499 27-Mar-2018 JY Added     

                    CInfo.EnableEPrimeRx = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnableEPrimeRx"].ToString());
                    CInfo.EPrimeRxURL = convertNullToString(oDS.Tables[0].Rows[0]["EPrimeRxURL"].ToString());
                    CInfo.EPrimeRxToken = convertNullToString(oDS.Tables[0].Rows[0]["EPrimeRxToken"].ToString());

                    #region  PRIMEPOS-2739 ADDED BY ARVIND
                    Dictionary<string, string> getsett = getSettingDetail();
                    CSetting = new SettingDetail();
                    CSetting.StrictReturn = convertNullToBoolean(getsett.ContainsKey("StrictReturn") ? getsett["StrictReturn"].Trim() == "0" ? false : true : false);
                    CSetting.MaskDrugName = convertNullToBoolean(getsett.ContainsKey("MaskDrugName") ? getsett["MaskDrugName"].Trim() == "0" ? false : true : false);  //PRIMEPOS-3130
                    //CSetting.PromptForEvertecManual = convertNullToBoolean(getsett.ContainsKey("PromtManualEvertec") ? getsett["PromtManualEvertec"].Trim() == "0" ? false : true : false);//PRIMEPOS-2805
                    CSetting.PromptForEvertecManual = getsett.ContainsKey("PromtManualEvertec") ? convertNullToBoolean(getsett["PromtManualEvertec"].Trim()) : false;//PRIMEPOS-2805 // SAJID DHUKKA
                    #endregion

                    #region PRIMEPOS-2774 10-Jan-2020 JY Added
                    CSetting.S3FTPURL = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3FTPURL) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_S3FTPURL]).Trim() : "";
                    CSetting.S3FTPPort = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3FTPPort) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_S3FTPPort]) : 0;
                    CSetting.S3FTP = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3FTP) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_S3FTP]) : false;
                    CSetting.S3FTPUserId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3FTPUserId) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_S3FTPUserId]).Trim() : "";
                    CSetting.S3FTPPassword = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3FTPPassword) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_S3FTPPassword]).Trim() : "";
                    CSetting.S3Frequency = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3Frequency) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_S3Frequency]) : 0;
                    CSetting.S3FileName = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3FileName) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_S3FileName]).Trim() : "";
                    CSetting.S3FTPFolderPath = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3FTPFolderPath) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_S3FTPFolderPath]).Trim() : "";

                    #region PRIMEPOS-3370
                    CSetting.NBSEnable = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NBSEnable) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_NBSEnable]) : false;
                    CSetting.NBSUrl = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NBSUrl) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NBSUrl]).Trim() : "";
                    //CSetting.NBSToken = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NBSToken) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NBSToken]).Trim() : ""; //PRIMEPOS-3412-Need-To-Change
                    CSetting.NBSEntityID = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NBSEntityID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NBSEntityID]).Trim() : "";
                    CSetting.NBSStoreID = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NBSStoreID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NBSStoreID]).Trim() : "";
                    //CSetting.NBSTerminalID = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NBSTerminalID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NBSTerminalID]).Trim() : "";
                    CSetting.NBSBins = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NBSBins) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NBSBins]) : "";  //PRIM0EPOS-3529 //PRIM0EPOS-3504
                    #endregion

                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3LastUploadDateOnFTP))
                    {
                        try
                        {
                            CSetting.S3LastUploadDateOnFTP = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_S3LastUploadDateOnFTP]);
                        }
                        catch
                        {
                            CSetting.S3LastUploadDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");
                        }
                    }
                    else
                        CSetting.S3LastUploadDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");

                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_S3UploadProcessDate))
                    {
                        try
                        {
                            CSetting.S3UploadProcessDate = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_S3UploadProcessDate]);
                        }
                        catch
                        {
                            CSetting.S3UploadProcessDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
                        }
                    }
                    else
                        CSetting.S3UploadProcessDate = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    #endregion
                    CSetting.TagSolutran = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TagSolutran) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_TagSolutran]) : false;   //PRIMEPOS-2836 21-Apr-2020 JY Added

                    #region PRIMEPOS-3243
                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LastUpdateInsSigTrans))
                    {
                        try
                        {
                            CSetting.LastUpdateInsSigTrans = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_LastUpdateInsSigTrans]);
                        }
                        catch
                        {
                            CSetting.LastUpdateInsSigTrans = DateTime.MinValue;
                        }
                    }
                    else
                        CSetting.LastUpdateInsSigTrans = DateTime.MinValue;

                    CSetting.FrequencyIntervalInsSigTrans = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FrequencyIntervalInsSigTrans) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_FrequencyIntervalInsSigTrans]) : 0;

                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LastUpdatePOSTransactionRxDetail))
                    {
                        try
                        {
                            CSetting.LastUpdatePOSTransactionRxDetail = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_LastUpdatePOSTransactionRxDetail]);
                        }
                        catch
                        {
                            CSetting.LastUpdatePOSTransactionRxDetail = DateTime.MinValue;
                        }
                    }
                    else
                        CSetting.LastUpdatePOSTransactionRxDetail = DateTime.MinValue;

                    CSetting.FrequencyIntervalPOSTransactionRxDetail = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FrequencyIntervalPOSTransactionRxDetail) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_FrequencyIntervalPOSTransactionRxDetail]) : 0;

                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LastUpdateReturnTransDetailId))
                    {
                        try
                        {
                            CSetting.LastUpdateReturnTransDetailId = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_LastUpdateReturnTransDetailId]);
                        }
                        catch
                        {
                            CSetting.LastUpdateReturnTransDetailId = DateTime.MinValue;
                        }
                    }
                    else
                        CSetting.LastUpdateReturnTransDetailId = DateTime.MinValue;

                    CSetting.FrequencyIntervalReturnTransDetailId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FrequencyIntervalReturnTransDetailId) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_FrequencyIntervalReturnTransDetailId]) : 0;

                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LastUpdateTrimItemID))
                    {
                        try
                        {
                            CSetting.LastUpdateTrimItemID = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_LastUpdateTrimItemID]);
                        }
                        catch
                        {
                            CSetting.LastUpdateTrimItemID = DateTime.MinValue;
                        }
                    }
                    else
                        CSetting.LastUpdateTrimItemID = DateTime.MinValue;

                    CSetting.FrequencyIntervalTrimItemID = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FrequencyIntervalTrimItemID) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_FrequencyIntervalTrimItemID]) : 0;

                    //if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LastUpdateSign))
                    //{
                    //    try
                    //    {
                    //        CSetting.LastUpdateSign = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_LastUpdateSign]);
                    //    }
                    //    catch
                    //    {
                    //        CSetting.LastUpdateSign = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    //    }
                    //}
                    //else
                    //    CSetting.LastUpdateSign = Convert.ToDateTime("1900-01-01 00:00:00.000");

                    //CSetting.FrequencyIntervalSign = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FrequencyIntervalSign) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_FrequencyIntervalSign]) : 0;

                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LastUpdateMissingTransDetInPrimeRx))
                    {
                        try
                        {
                            CSetting.LastUpdateMissingTransDetInPrimeRx = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_LastUpdateMissingTransDetInPrimeRx]);
                        }
                        catch
                        {
                            CSetting.LastUpdateMissingTransDetInPrimeRx = DateTime.MinValue;
                        }
                    }
                    else
                        CSetting.LastUpdateMissingTransDetInPrimeRx = DateTime.MinValue;

                    CSetting.FrequencyIntervalMissingTransDetInPrimeRx = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FrequencyIntervalMissingTransDetInPrimeRx) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_FrequencyIntervalMissingTransDetInPrimeRx]) : 0;

                    if (getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LastUpdateBlankSignWithSamePatientsSign))
                    {
                        try
                        {
                            CSetting.LastUpdateBlankSignWithSamePatientsSign = Convert.ToDateTime(getsett[clsPOSDBConstants.SettingDetail_LastUpdateBlankSignWithSamePatientsSign]);
                        }
                        catch
                        {
                            CSetting.LastUpdateBlankSignWithSamePatientsSign = DateTime.MinValue;
                        }
                    }
                    else
                        CSetting.LastUpdateBlankSignWithSamePatientsSign = DateTime.MinValue;

                    CSetting.FrequencyIntervalBlankSignWithSamePatientsSign = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FrequencyIntervalBlankSignWithSamePatientsSign) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_FrequencyIntervalBlankSignWithSamePatientsSign]) : 0;

                    #endregion

                    #region PRIMEPOS-2842 05-May-2020 JY Added 
                    CSetting.TPWelcomeMessage = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TPWelcomeMessage) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_TPWelcomeMessage]).Trim() : "";
                    CSetting.TPMessage = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TPMessage) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_TPMessage]).Trim() : "";
                    CSetting.EnablePromptForZipCode = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnablePromptForZipCode) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_EnablePromptForZipCode]).Trim() : "";
                    CSetting.RequiredDriverForVerifonePads = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_RequiredDriverForVerifonePads) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_RequiredDriverForVerifonePads]).Trim() : "";
                    CSetting.Driver = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Driver) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_Driver]).Trim() : "";
                    CSetting.ComPort = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ComPort) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_ComPort]).Trim() : "";
                    CSetting.DataBits = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_DataBits) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_DataBits]) : 0;
                    CSetting.Parity = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Parity) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_Parity]).Trim() : "";
                    CSetting.StopBits = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_StopBits) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_StopBits]).Trim() : "";
                    CSetting.Handshake = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Handshake) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_Handshake]).Trim() : "";
                    CSetting.BaudRate = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_BaudRate) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_BaudRate]) : 0;
                    #region PRIMEPOS-3500
                    CSetting.QuickChip = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_QuickChip) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_QuickChip]) : false;
                    CSetting.ReturnResponseBeforeCardRemoval = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ReturnResponseBeforeCardRemoval) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_ReturnResponseBeforeCardRemoval]) : false; //PRIMEPOS-3535
                    CSetting.CheckForPreReadId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_CheckForPreReadId) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_CheckForPreReadId]) : false;
                    CSetting.ContactlessMsdEntryAllowed = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ContactlessMsdEntryAllowed) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_ContactlessMsdEntryAllowed]) : false;
                    CSetting.DisplayCustomAidScreen = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_DisplayCustomAidScreen) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_DisplayCustomAidScreen]) : false;
                    CSetting.Unattended = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Unattended) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_Unattended]) : false;
                    CSetting.QuickChipDataLifetime = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_QuickChipDataLifetime) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_QuickChipDataLifetime]).Trim() : "120";
                    CSetting.ThresholdAmount = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ThresholdAmount) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_ThresholdAmount]).Trim() : "";
                    #endregion
                    CSetting.TestMode = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TestMode) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_TestMode]) : false;
                    CSetting.AllowPartialApprovals = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_AllowPartialApprovals) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_AllowPartialApprovals]) : false;
                    CSetting.ConfirmOriginalAmount = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ConfirmOriginalAmount) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_ConfirmOriginalAmount]) : false;
                    CSetting.CheckForDuplicateCreditCardTransactions = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_CheckForDuplicateCreditCardTransactions) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_CheckForDuplicateCreditCardTransactions]) : false;
                    CSetting.VantivsCashBackFeature = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_VantivsCashBackFeature) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_VantivsCashBackFeature]) : false;
                    CSetting.PromptForCreditCardCVVNumberForKeyedCardTransactions = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PromptForCreditCardCVVNumberForKeyedCardTransactions) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_PromptForCreditCardCVVNumberForKeyedCardTransactions]) : false;
                    CSetting.EnableDebitSale = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableDebitSale) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableDebitSale]) : false;
                    CSetting.EnableDebitRefunds = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableDebitRefunds) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableDebitRefunds]) : false;
                    CSetting.EnableEBTRefunds = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableEBTRefunds) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableEBTRefunds]) : false;
                    CSetting.EnableGiftCards = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableGiftCards) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableGiftCards]) : false;
                    CSetting.EnableEBTFoodStamp = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableEBTFoodStamp) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableEBTFoodStamp]) : false;
                    CSetting.EnableEBTCashBenefit = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableEBTCashBenefit) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableEBTCashBenefit]) : false;
                    CSetting.EnableEMVProcessing = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableEMVProcessing) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableEMVProcessing]) : false;
                    CSetting.FSAHRACardProcessing = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_FSAHRACardProcessing) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_FSAHRACardProcessing]) : false;
                    CSetting.TippingDoesNotApplyToPharmacyBusiness = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TippingDoesNotApplyToPharmacyBusiness) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_TippingDoesNotApplyToPharmacyBusiness]) : false;
                    CSetting.ManualCreditCardEntry = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ManualCreditCardEntry) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_ManualCreditCardEntry]) : false;
                    CSetting.NearFieldCommunication = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NearFieldCommunication) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_NearFieldCommunication]) : false;
                    #endregion

                    #region PRIMEPOS-3024 08-Nov-2021 JY Added
                    CSetting.IPTerminalId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPTerminalId) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_IPTerminalId]).Trim() : "";
                    CSetting.IPTerminalType = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPTerminalType) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_IPTerminalType]).Trim() : "";
                    CSetting.IPDriver = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPDriver) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_IPDriver]).Trim() : "";
                    CSetting.IPPort = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPPort) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_IPPort]).Trim() : "";
                    CSetting.IPMessage = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPMessage) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_IPMessage]).Trim() : "";
                    CSetting.IPTransactionAmountLimit = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPTransactionAmountLimit) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_IPTransactionAmountLimit]).Trim() : "";
                    CSetting.IPisContactlessEmvEntryAllowed = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPisContactlessEmvEntryAllowed) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_IPisContactlessEmvEntryAllowed]) : false;
                    CSetting.IPisContactlessMsdEntryAllowed = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPisContactlessMsdEntryAllowed) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_IPisContactlessMsdEntryAllowed]) : false;
                    CSetting.IPisManualEntryAllowed = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPisManualEntryAllowed) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_IPisManualEntryAllowed]) : false;
                    #region PRIMEPOS-3266
                    CSetting.IPisDisplayCustomAidScreen = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPisDisplayCustomAidScreen) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_IPisDisplayCustomAidScreen]) : false;
                    CSetting.IPisConfirmTotalAmountScreenDisplayed = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPisConfirmTotalAmountScreenDisplayed) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_IPisConfirmTotalAmountScreenDisplayed]) : false;
                    CSetting.IPisConfirmCreditSurchargeScreenDisplayed = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_IPisConfirmCreditSurchargeScreenDisplayed) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_IPisConfirmCreditSurchargeScreenDisplayed]) : false;
                    #endregion
                    #endregion

                    #region PRIMEPOS-2841 added by Arvind
                    //CSetting.OnlinePayment = convertNullToBoolean(getsett.ContainsKey("OnlinePayment") ? getsett["OnlinePayment"].Trim() == "0" ? false : true : false);
                    CSetting.OnlinePayment = getsett.ContainsKey("OnlinePayment") ? convertNullToBoolean(getsett["OnlinePayment"].Trim()) : false; // SAJID DHUKKA
                    #endregion
                    CSetting.AutoSearchPrimeRxPatient = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_AutoSearchPrimeRxPatient) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_AutoSearchPrimeRxPatient]) : false;    //PRIMEPOS-2845 14-May-2020 JY Added

                    #region PRIMERX 2841
                    CSetting.PrimeRxPayClientId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ClientID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_ClientID]).Trim() : "";
                    string decryptedPass = string.Empty;
                    if (!string.IsNullOrEmpty(getsett[clsPOSDBConstants.SettingDetail_SecretKey]))
                    {
                        MMS.Encryption.NativeEncryption.DecryptText(convertNullToString(getsett[clsPOSDBConstants.SettingDetail_SecretKey]).Trim(), ref decryptedPass);
                    }
                    CSetting.PrimeRxPaySecretKey = decryptedPass;

                    CSetting.PrimeRxPayUrl = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_URL) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_URL]).Trim() : "";

                    CSetting.PrimerxPayExtensionUrl = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Extension_URL) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_Extension_URL]).Trim() : "";//2956
                    CSetting.PrimeRxPayBGStatusUpdate = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PrimeRxPayBGStatusUpdate) ? convertNullToBooleanTrue(getsett[clsPOSDBConstants.SettingDetail_PrimeRxPayBGStatusUpdate]) : false;//PRIMEPOS-3187
                    CSetting.PrimeRxPayDefaultSelection = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PrimeRxPayDefaultSelection) ? convertNullToBooleanTrue(getsett[clsPOSDBConstants.SettingDetail_PrimeRxPayDefaultSelection]) : false;//PRIMEPOS-3250
                    CSetting.PrimeRxPayStatusUpdateIntervalInMin = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PrimeRxPayStatusUpdateIntervalInMin) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_PrimeRxPayStatusUpdateIntervalInMin]) : 180;//PRIMEPOS-3187
                    CSetting.PrimeRxPayStatusUpdateFromLastDays = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PrimeRxPayStatusUpdateFromLastDays) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_PrimeRxPayStatusUpdateFromLastDays]) : 2;//PRIMEPOS-3187
                    #endregion
                    CSetting.PseudoephedDisclaimer = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PseudoephedDisclaimer) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_PseudoephedDisclaimer]).Trim() : "";//PRIMEPOS-3109
                    #region PRIMEPOS-2794 - CUstomerEngagement - Nileshj
                    CSetting.EnableCustomerEngagement = convertNullToBoolean(getsett.ContainsKey("EnableCustomerEngagement") ? getsett["EnableCustomerEngagement"].Trim() == "0" ? false : true : false);
                    #endregion

                    #region PRIMEPOS-2902                    
                    if (string.IsNullOrWhiteSpace(CSetting.PharmacyNPI))
                    {
                        POSTransaction oPosTrans = new BusinessRules.POSTransaction();
                        CSetting.PharmacyNPI = oPosTrans.GetPharmacyNPI().Tables[0].Rows[0][0].ToString();
                    }
                    CSetting.PayProviderName = getsett.ContainsKey("PayProviderName") ? convertNullToString(getsett["PayProviderName"]).Trim() : "";

                    CSetting.LinkExpriyInMinutes = getsett.ContainsKey("LinkExpiryInMinutes") ? convertNullToString(getsett["LinkExpiryInMinutes"]) : "";//2915
                    CSetting.OnlineOption = getsett.ContainsKey("OnlineOption") ? convertNullToString(getsett["OnlineOption"]) : "";//2915

                    if (Configuration.PayProviderIdAndNames != null)
                    {
                        if (Configuration.PayProviderIdAndNames.ContainsKey(Configuration.PayProviderIdAndNames.FirstOrDefault(x => x.Value == CSetting.PayProviderName).Key))
                        {
                            CSetting.PayProviderID = Configuration.PayProviderIdAndNames.FirstOrDefault(x => x.Value == CSetting.PayProviderName).Key;
                        }
                        else
                            CSetting.PayProviderID = getsett.ContainsKey("PayProviderID") ? convertNullToInt(getsett["PayProviderID"]) : 0;
                    }
                    else
                    {
                        CSetting.PayProviderID = getsett.ContainsKey("PayProviderID") ? convertNullToInt(getsett["PayProviderID"]) : 0;
                    }
                    #endregion

                    CSetting.VantivDelayInSecond = getsett.ContainsKey("VantivDelayInSecond") ? convertNullToString(getsett["VantivDelayInSecond"]).Trim() : "";

                    CSetting.ProceedROATransWithHCaccNotLinked = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ProceedROATransWithHCaccNotLinked) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_ProceedROATransWithHCaccNotLinked]) : false; //PRIMEPOS-2570 17-Aug-2020 JY Added
                    CSetting.DefaultPaytype = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_DefaultPaytype) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_DefaultPaytype]) : ""; //PRIMEPOS-2512 02-Oct-2020 JY Added
                    CSetting.RestrictSignatureLineAndWordingOnReceipt = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_RestrictSignatureLineAndWordingOnReceipt) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_RestrictSignatureLineAndWordingOnReceipt]) : false;    //PRIMEPOS-2910 29-Oct-2020 JY Added
                    CSetting.RxInsuranceToBeTaxed = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_RxInsuranceToBeTaxed) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_RxInsuranceToBeTaxed]) : "";    //PRIMEPOS-2924 02-Dec-2020 JY Added
                    CSetting.PatientCounselingPrompt = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PatientCounselingPrompt) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_PatientCounselingPrompt]) : "";   //PRIMEPOS-2461 02-Mar-2021 JY Added
                    CSetting.PrintCompanyLogo = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PrintCompanyLogo) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_PrintCompanyLogo]) : false;   //PRIMEPOS-2386 26-Feb-2021 JY Added
                    CSetting.RunVantivSignatureFix = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_RunVantivSignatureFix) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_RunVantivSignatureFix]) : false;   //PRIMEPOS-3232N
                    CSetting.CompanyLogoFileName = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_CompanyLogoFileName) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_CompanyLogoFileName]) : "";   //PRIMEPOS-2386 26-Feb-2021 JY Added
                    CSetting.SchedularMachine = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_SchedularMachine) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_SchedularMachine]) : "";    //PRIMEPOS-2485 19-Mar-2021 JY Added
                    CSetting.SchedulerUser = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_SchedulerUser) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_SchedulerUser]) : ""; //PRIMEPOS-2485 05-Apr-2021 JY Added
                    CSetting.AzureADMiddleTierUrl = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_AzureADMiddleTierUrl) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_AzureADMiddleTierUrl]) : "";    //PRIMEPOS-2989 13-Aug-2021 JY Added
                    CSetting.ApplicationLaunchContext = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ApplicationLaunchContext) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_ApplicationLaunchContext]) : "";    //PRIMEPOS-2993 24-Aug-2021 JY Added
                    #region PRIMEPOS-2999 09-Sep-2021 JY Added
                    CSetting.NPlexURL = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NPlexURL) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NPlexURL]) : "";
                    CSetting.NPlexTokenURL = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NPlexTokenURL) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NPlexTokenURL]) : "";
                    CSetting.NPlexClientID = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NPlexClientID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NPlexClientID]) : "";
                    CSetting.NPlexClientSecret = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NPlexClientSecret) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_NPlexClientSecret]) : "";
                    #endregion
                    #region PRIMEPOS-2990
                    CSetting.SiteId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_SiteID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_SiteID]).Trim() : "";//2956
                    CSetting.LicenseId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_LicenseID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_LicenseID]).Trim() : "";//2956
                    CSetting.DeviceId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_DeviceId) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_DeviceId]).Trim() : "";//2956
                    CSetting.Username = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Username) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_Username]).Trim() : "";//2956
                    CSetting.Password = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Password) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_Password]).Trim() : "";//2956
                    CSetting.DeveloperId = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_DeveloperId) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_DeveloperId]).Trim() : "";//2956
                    CSetting.VersionNumber = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_VersionNumber) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_VersionNumber]).Trim() : "";//2956
                    #endregion

                    #region 2943
                    Configuration.CSetting.ChainCode = getsett.ContainsKey("ChainCode") ? convertNullToString(getsett["ChainCode"]) : "";
                    Configuration.CSetting.LocationName = getsett.ContainsKey("LocationName") ? convertNullToString(getsett["LocationName"]) : "";
                    Configuration.CSetting.TerminalID = getsett.ContainsKey("TerminalID") ? convertNullToString(getsett["TerminalID"]) : "";
                    #endregion
                    CSetting.RxTaxPolicy = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_RxTaxPolicy) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_RxTaxPolicy]) : "";   //PRIMEPOS-3053 04-Feb-2022 JY Added
                    CSetting.NotifyRefrigeratedMedication = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_NotifyRefrigeratedMedication) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_NotifyRefrigeratedMedication]) : false;    //PRIMEPOS-2651 08-Apr-2022 JY Added
                    CSetting.RestrictMultipleClockIn = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_RestrictMultipleClockIn) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_RestrictMultipleClockIn]) : false;    //PRIMEPOS-2790 18-Apr-2022 JY Added
                    CSetting.TransactionFeeApplicableFor = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TransactionFeeApplicableFor) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_TransactionFeeApplicableFor]) : "";   //PRIMEPOS-3115 11-Jul-2022 JY Added
                    CSetting.ResetPwdForceUserToChangePwd = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_ResetPwdForceUserToChangePwd) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_ResetPwdForceUserToChangePwd]) : false;    //PRIMEPOS-3129 22-Aug-2022 JY Added
                    CSetting.PromptToSaveCCToken = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PromptToSaveCCToken) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_PromptToSaveCCToken]) : "";   //PRIMEPOS-3145 28-Sep-2022 JY Added

                    #region PRIMEPOS-3164 01-Nov-2022 JY Added
                    CSetting.TranslatorAPIkey = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TranslatorAPIkey) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_TranslatorAPIkey]) : "";
                    CSetting.TranslatorAPIEndPoint = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TranslatorAPIEndPoint) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_TranslatorAPIEndPoint]) : "";
                    CSetting.TranslatorAPILocation = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TranslatorAPILocation) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_TranslatorAPILocation]) : "";
                    CSetting.TranslatorAPIRoute = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_TranslatorAPIRoute) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_TranslatorAPIRoute]) : "";
                    #endregion

                    CSetting.PatientsSubCategories = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PatientsSubCategories) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_PatientsSubCategories]) : ""; //PRIMEPOS-3157 28-Nov-2022 JY Added

                    #region PRIMEPOS-2562 27-Jul-2018 JY Added
                    CInfo.EnforceLowerCaseChar = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnforceLowerCaseChar"].ToString());
                    CInfo.EnforceUpperCaseChar = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnforceUpperCaseChar"].ToString());
                    CInfo.EnforceSpecialChar = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnforceSpecialChar"].ToString());
                    CInfo.EnforceNumber = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnforceNumber"].ToString());
                    CInfo.PasswordExpirationDays = convertNullToInt(oDS.Tables[0].Rows[0]["PasswordExpirationDays"].ToString());
                    CInfo.PasswordLength = convertNullToInt(oDS.Tables[0].Rows[0]["PasswordLength"].ToString());
                    CInfo.PasswordHistoryCount = convertNullToInt(oDS.Tables[0].Rows[0]["PasswordHistoryCount"].ToString());
                    #endregion
                    CInfo.UseBiometricDevice = convertNullToString(oDS.Tables[0].Rows[0]["UseBiometricDevice"].ToString()); //PRIMEPOS-2576 23-Aug-2018 JY Added
                    CInfo.IgnoreFutureRx = convertNullToBoolean(oDS.Tables[0].Rows[0]["IgnoreFutureRx"].ToString());    //PRIMEPOS-2591 25-Oct-2018 JY Added
                    CInfo.ShowPaytypeDetails = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowPaytypeDetails"].ToString());    //PRIMEPOS-2384 29-Oct-2018 JY Added
                    CInfo.RestrictFSACardMessage = convertNullToBoolean(oDS.Tables[0].Rows[0]["RestrictFSACardMessage"].ToString());    //PRIMEPOS-2621 17-Dec-2018 JY Added
                    CInfo.AuthenticationMode = convertNullToInt(oDS.Tables[0].Rows[0]["AuthenticationMode"].ToString());    //PRIMEPOS-2616 19-Dec-2018 JY Added

                    #region PRIMEPOS-2613 28-Dec-2018 JY Added
                    CInfo.CardExpAlert = convertNullToBoolean(oDS.Tables[0].Rows[0]["CardExpAlert"].ToString());
                    CInfo.CardExpEmail = convertNullToBoolean(oDS.Tables[0].Rows[0]["CardExpEmail"].ToString());
                    CInfo.CardExpAlertDays = convertNullToInt(oDS.Tables[0].Rows[0]["CardExpAlertDays"].ToString());
                    CInfo.SPEmailFormatId = convertNullToInt(oDS.Tables[0].Rows[0]["SPEmailFormatId"].ToString());
                    #endregion

                    CInfo.PromptForItemPriceUpdate = convertNullToBoolean(oDS.Tables[0].Rows[0]["PromptForItemPriceUpdate"].ToString());    //PRIMEPOS-2602 28-Jan-2019 JY Added
                    CInfo.UsePrimeESC = convertNullToBoolean(oDS.Tables[0].Rows[0]["UsePrimeESC"].ToString());    //PRIMEPOS-2385 14-Mar-2019 JY Added
                    CInfo.ShowPatientsData = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowPatientsData"].ToString());    //PRIMEPOS-2317 21-Mar-2019 JY Added
                    CInfo.RestrictIfDOBMismatch = convertNullToBoolean(oDS.Tables[0].Rows[0]["RestrictIfDOBMismatch"].ToString());    //PRIMEPOS-2317 21-Mar-2019 JY Added
                    CInfo.PHNPINO = convertNullToString(oDS.Tables[0].Rows[0]["PHNPINO"].ToString());   //PRIMEPOS-2667 12-Apr-2019 JY Added
                    CInfo.PSServiceAddress = convertNullToString(oDS.Tables[0].Rows[0]["PSServiceAddress"].ToString()); //PRIMEPOS-2671 18-Apr-2019 JY Added

                    #region Added By Arvind S3 Setting - Solutran - PRIMEPOS-2663
                    CInfo.S3Enable = convertNullToBoolean(oDS.Tables[0].Rows[0]["S3Enable"].ToString());
                    CInfo.S3Url = convertNullToString(oDS.Tables[0].Rows[0]["S3Url"].ToString());
                    CInfo.S3Key = convertNullToString(oDS.Tables[0].Rows[0]["S3Key"].ToString());
                    CInfo.S3Merchant = convertNullToString(oDS.Tables[0].Rows[0]["S3Merchant"].ToString());
                    #endregion
                    CInfo.HidePatientCounseling = convertNullToBoolean(oDS.Tables[0].Rows[0]["HidePatientCounseling"].ToString());

                    #region BatchDelivery - NileshJ - PRIMERX-7688
                    CInfo.isPrimeDeliveryReconciliation = convertNullToBoolean(oDS.Tables[0].Rows[0]["isPrimeDeliveryReconciliation"].ToString());
                    #endregion

                    #region Pointy PRIMEPOS-2875
                    CSetting.MMSKey = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_MMSKEY) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_MMSKEY]).Trim() : "";
                    CSetting.RetailerKey = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_RetailerKey) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_RetailerKey]).Trim() : "";
                    CSetting.URL = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_Url) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_Url]).Trim() : "";
                    CSetting.SupportMail = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_SupportMail) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_SupportMail]).Trim() : "";
                    CSetting.MMSEmailID = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_MMSEmailID) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_MMSEmailID]).Trim() : "";
                    CSetting.MMSEmailPass = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_MMSEmailPass) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_MMSEmailPass]).Trim() : "";
                    CSetting.EnableSSL = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_EnableSSL) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_EnableSSL]) : false;
                    CSetting.MMSEmailPort = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_MMSEmailPort) ? convertNullToInt(getsett[clsPOSDBConstants.SettingDetail_MMSEmailPort]) : 587;
                    CSetting.MMSEmailServer = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_MMSEmailServer) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_MMSEmailServer]).Trim() : "";
                    CSetting.MMSEmailSig = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_MMSEmailSig) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_MMSEmailSig]).Trim() : "";
                    CSetting.PointyUTM = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_PointyUTM) ? convertNullToString(getsett[clsPOSDBConstants.SettingDetail_PointyUTM]).Trim() : "";//PRIMEPOS-3005
                    #endregion

                    CSetting.AllowZeroShippingCharge = getsett.ContainsKey("AllowZeroDollarShipCharge") ? convertNullToBoolean(getsett["AllowZeroDollarShipCharge"]) : false;//2927
                    CSetting.AllowMailOrder = getsett.ContainsKey("AllowMailOrder") ? convertNullToBoolean(getsett["AllowMailOrder"]) : false;//2927
                    CInfo.OldEDIComatibility = convertNullToString(oDS.Tables[0].Rows[0]["OldEDIComatibility"].ToString());//PRIMEPOS-2679
                    CSetting.WaiveTransactionFee = getsett.ContainsKey(clsPOSDBConstants.SettingDetail_WaiveTransactionFee) ? convertNullToBoolean(getsett[clsPOSDBConstants.SettingDetail_WaiveTransactionFee]) : false; //PRIMEPOS-3234
                    CInfo.SSOIdentifier = convertNullToInt(oDS.Tables[0].Rows[0]["SSOIdentifier"].ToString());  //PRIMEPOS-3484
                }
                else
                {
                    #region company info

                    CInfo = new CompanyInfo();
                    CInfo.StoreID = "";
                    CInfo.StoreName = "";
                    CInfo.Address = "";
                    CInfo.City = "";
                    CInfo.State = "";
                    CInfo.Zip = "";
                    CInfo.Telephone = "";
                    CInfo.ReceiptMSG = "";
                    CInfo.NoOfReceipt = 1;
                    CInfo.NoOfOnHoldTransReceipt = 1;
                    CInfo.NoOfCC = 1;
                    CInfo.NoOfHCRC = 1;
                    CInfo.NoOfRARC = 1;
                    CInfo.MerchantNo = "";
                    CInfo.SigType = clsPOSDBConstants.STRINGIMAGE;
                    CInfo.PrivacyText = "";
                    CInfo.PatientCounceling = "N";
                    CInfo.PrivacyExpiry = 6;
                    CInfo.RemoteDBServer = "";
                    CInfo.RemoteCatalog = "";
                    CInfo.UseRemoteServer = false;
                    CInfo.AllowUnPickedRX = "0";
                    CInfo.UnPickedRXSearchDays = 0;
                    CInfo.CheckRXItemsForIIAS = false;
                    CInfo.AllowVerifiedRXOnly = 0;  //PRIMEPOS-2593 23-Jun-2020 JY modified
                    CInfo.GroupTransItems = false;
                    CInfo.ForceCustomerInTrans = false;
                    //Added By SRT(Ritesh Parekh) Date: 28-Aug-2009
                    CInfo.DefaultDeptId = 0;
                    //End Of Added By SRT(Ritesh Parekh)

                    CInfo.PostDescOnlyInHC = false;
                    CInfo.WarnMultiPatientRX = false;
                    CInfo.ChargeCashBackMode = "N";
                    CInfo.AllowCashBack = false;
                    CInfo.PostRXNumberOnlyInHC = false;
                    CInfo.PrintReceiptForOnHoldTrans = "0";
                    //Start : Added By Amit 19 April 2011
                    CInfo.PrintStCloseNo = true;
                    CInfo.PrintEODNo = true;
                    CInfo.PWEncrypted = false;
                    CInfo.ShowOnlyOneCCType = false;
                    //End
                    //Added By Shitaljit(QuicSolv) on 3 June 2011
                    CInfo.DefalutInvRecievedID = 0;
                    CInfo.DefaultInvReturnID = 0;
                    CInfo.AllowMultipleInstanceOfPOS = 0;   //PRIMEPOS-2936 21-Jan-2021 JY modified
                    CInfo.UpdateDeftInvRecv = false;
                    CInfo.UpdateDeftInvRet = false;
                    CInfo.AllowHundredPerInvDiscount = false;//Added By shitaljit on 7 Dec 2011
                    CInfo.InvDiscToDiscountableItemOnly = true;//Added By shitaljit on 27 Dec 2011
                    CInfo.AllowMultipleRXRefillsInSameTrans = false;
                    CInfo.EBTAsManualTrans = false;//Added By shitaljit on 6 April 2012
                    CInfo.AutoImportCustAtTrans = 0;    //PRIMEPOS-2886 25-Sep-2020 JY Modified
                    //Till Here Added By Shitaljit
                    CInfo.NoOfCashReceipts = 1;
                    CInfo.NoOfGiftReceipt = 1;
                    CInfo.OtcPrivacyNotice = ""; //Added by Manoj 5/15/2012
                    CInfo.PrintReceipt = 'Y';
                    CInfo.AllowPrintZeroTrans = true;
                    CInfo.DoNotOpenDrawerForCCOnlyTrans = false;
                    CInfo.DoNotOpenDrawerForChequeTrans = false;//Added by Ravindra 10/1/2014

                    CInfo.PreventRxMaxFillDayLimit = 0;//Added by Ravindra 10/07/2014 PRIMEPOS-2052
                    CInfo.PreventRxMaxFillDayNotifyAction = 0;//Added by Ravindra 10/07/2014 PRIMEPOS-2052

                    CInfo.ConfirmPatient = 0;//Added by Krishna on 20 August 2012   //PRIMEPOS-2317 15-Mar-2019 JY modified
                    CInfo.ShowTextPrediction = false;
                    CInfo.UseSameThreadToVerifySysDateTime = true;
                    //CInfo.EnforceComplexPassword = true;  //PRIMEPOS-2562 27-Jul-2018 JY Commented
                    //CInfo.EnforceComplexPassword = false;//Add by Ravindra 21 March 2013  //PRIMEPOS-2562 27-Jul-2018 JY Commented  
                    CInfo.OpenDrawerForZeroAmtTrans = true;//Added By shitaljit on 7 July 2013.
                    CInfo.UpdatePatientData = "N";//Added By shitaljit on 19 July 2013. PRIMEPOS-1235 Add Preference to control Updating patient data from PrimeRX during transaction.
                    CInfo.AutoPopulateCLCardDetails = false;
                    CInfo.OutGoingEmailBody = convertNullToString("");//Added By Ravindra 
                    CInfo.OutGoingEmailEnableSSL = convertNullToBoolean("true"); ;//Added By Ravindra 
                    CInfo.OutGoingEmailID = convertNullToString(""); ;//Added By Ravindra 
                    CInfo.OwnersEmailId = string.Empty; //Sprint-24 - PRIMEPOS-2363 28-Dec-2016 JY Added
                    CInfo.AutoEmailStationCloseReport = false;  //Sprint-24 - PRIMEPOS-2363 27-Jan-2017 JY Added
                    CInfo.AutoEmailEODReport = false;   //Sprint-24 - PRIMEPOS-2363 27-Jan-2017 JY Added
                    CInfo.OutGoingEmailPass = convertNullToString(""); ;//Added By Ravindra 
                    CInfo.OutGoingEmailPort = Configuration.convertNullToInt("587");
                    CInfo.OutGoingEmailServer = convertNullToString(""); ;//Added By Ravindra 
                    CInfo.OutGoingEmailSignature = convertNullToString(""); ;//Added By Ravindra 
                    CInfo.OutGoingEmailSubject = convertNullToString(""); ;//Added By Ravindra 
                    CInfo.OutGoingEmailUserID = convertNullToString(""); ;//Added By Ravindra                     
                    CInfo.OutGoingEmailPromptAutomatically = convertNullToBoolean("true");//Added By Ravindra 
                    CInfo.UseEmailInvoice = false;
                    CInfo.ApplyInvDiscSettingsForCoupon = true;//Added By Shitaljit for PRIMEPOS-1652	Add preference to manage Promotional coupon discount to abide with discount settings
                    //CInfo.out

                    CInfo.CurrencySymbol = "$";//Added by Ravindra for Glogal currency Symbol
                    CInfo.WarnForRXDelivery = false;//Added By Shitaljit on 6/2/2104 for PRIMEPOS-1816 Ability to turn on\off delivery prompt
                    //Added By Shitaljit on 6/2/2104 for PRIMEPOS-1804 Auto Populate Email address from customer
                    CInfo.AutoPopulateCustEmail = false;
                    //END
                    CInfo.PromptForZeroSellingPrice = false;
                    CInfo.PromptForZeroCostPrice = false;
                    CInfo.DoNotOpenDrawerForHouseChargeOnlyTrans = false;   //Sprint-19 - 2161 27-Mar-2015 JY Added 

                    #region Sprint-20 26-May-2015 Auto updater JY Added settings for auto updater
                    CInfo.AllowAutomaticUpdates = false;
                    CInfo.AllowRunningUpdates = false;
                    CInfo.AutoUpdateServiceAddress = string.Empty;
                    CInfo.RunningTasksTimerInterval = 10;
                    #endregion

                    #region Sprint-23 - PRIMEPOS-2029 11-Apr-2016 JY Added
                    CInfo.useNplex = false;
                    CInfo.nplexStoreId = string.Empty;
                    CInfo.StoreSiteId = string.Empty;
                    CInfo.postSaleInd = false;
                    #endregion

                    #region PRIMEPOS-2643 05-Sep-2019
                    CInfo.PIEnable = false;
                    CInfo.PIURL = string.Empty;
                    CInfo.PUser = string.Empty;
                    CInfo.PPassword = string.Empty;
                    #endregion

                    #region Sprint-23 - PRIMEPOS-2244 19-May-2016 JY Added 
                    CInfo.PrintStationCloseDateTime = false;
                    CInfo.PrintEODDateTime = false;
                    #endregion
                    CInfo.SearchRxsWithPatientName = false;    //Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
                    CInfo.FetchFamilyRx = false;    //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added
                    CInfo.SaveCCToken = false;    //Sprint-23 - PRIMEPOS-2313 09-Jun-2016 JY Added

                    CInfo.AllowZeroSellingPrice = false;    //Sprint-21 - 2204 26-Jun-2015 JY Added
                    CInfo.RestrictInActiveItem = false;    //Sprint-21 - 2173 10-Jul-2015 JY Added
                    CInfo.PrintReceiptInMultipleLanguage = false;   //Sprint-21 - 1272 25-Aug-2015 JY Added
                    CInfo.ConsiderItemType = false;    //Sprint-22 16-Dec-2015 JY Added settings to control the ItemType behavior 
                    #endregion company info

                    #region Sprint-24 - PRIMEPOS-2344 02-Dec-2016 JY Added
                    CInfo.IIASFTPAddress = string.Empty;
                    CInfo.IIASFTPUserId = string.Empty;
                    CInfo.IIASFTPPassword = string.Empty;
                    CInfo.IIASFileName = string.Empty;
                    CInfo.IIASDownloadInterval = 6;
                    CInfo.IIASFileModifiedDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    #endregion

                    #region Sprint-25 - PRIMEPOS-2379 07-Feb-2017 JY Added
                    CInfo.PSEFTPAddress = string.Empty;
                    CInfo.PSEFTPUserId = string.Empty;
                    CInfo.PSEFTPPassword = string.Empty;
                    CInfo.PSEFileName = string.Empty;
                    CInfo.PSEDownloadInterval = 6;
                    CInfo.PSEFileModifiedDateOnFTP = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    CInfo.UpdatePSEItem = false;
                    #endregion
                    CInfo.DefaultCustomerTokenValue = false;    //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added

                    #region PRIMEPOS-2442 ADDED BT ROHIT NAIR
                    CInfo.EnableConsentCapture = false;
                    CInfo.SelectedConsentSource = "";
                    #endregion

                    #region PrimePOS-2448 Added BY Rohit Nair

                    CInfo.EnableIntakeBatch = false;
                    CInfo.SkipSignatureForInatkeBatch = false;
                    CInfo.IntakeBatchMarkAsPickedup = false;
                    CInfo.IntakeBatchCode = "";
                    CInfo.IntakeBatchStatus = "";//PrimePOS-2518 Jenny Added

                    #endregion

                    CInfo.PromptForPartialPayment = false;  //PRIMEPOS-2499 27-Mar-2018 JY Added
                    CInfo.UseBiometricDevice = "";  //PRIMEPOS-2576 23-Aug-2018 JY Added
                    CInfo.IgnoreFutureRx = false;   //PRIMEPOS-2591 25-Oct-2018 JY Added
                    CInfo.ShowPaytypeDetails = false;   //PRIMEPOS-2384 29-Oct-2018 JY Added 
                    CInfo.RestrictFSACardMessage = false; //PRIMEPOS-2621 17-Dec-2018 JY Added
                    CInfo.AuthenticationMode = 0;   //PRIMEPOS-2616 19-Dec-2018 JY Added

                    #region PRIMEPOS-2613 28-Dec-2018 JY Added
                    CInfo.CardExpAlert = false;
                    CInfo.CardExpEmail = false;
                    CInfo.CardExpAlertDays = 0;
                    CInfo.SPEmailFormatId = 0;
                    #endregion

                    CInfo.PromptForItemPriceUpdate = false; //PRIMEPOS-2602 28-Jan-2019 JY Added
                    CInfo.UsePrimeESC = false; //PRIMEPOS-2385 14-Mar-2019 JY Added
                    CInfo.ShowPatientsData = false; //PRIMEPOS-2317 21-Mar-2019 JY Added
                    CInfo.RestrictIfDOBMismatch = false; //PRIMEPOS-2317 21-Mar-2019 JY Added
                    CInfo.PHNPINO = ""; //PRIMEPOS-2667 12-Apr-2019 JY Added
                    CInfo.PSServiceAddress = "";    //PRIMEPOS-2671 18-Apr-2019 JY Added

                    #region Added By Arvind S3 Setting - Solutran - PRIMEPOS-2663
                    CInfo.S3Enable = false;
                    CInfo.S3Url = "";
                    CInfo.S3Key = "";
                    CInfo.S3Merchant = "";
                    #endregion
                    CInfo.HidePatientCounseling = false;

                    #region BatchDelivery - NileshJ - PRIMERX-7688
                    CInfo.isPrimeDeliveryReconciliation = false;
                    #endregion

                    CInfo.OldEDIComatibility = "";//PRIMEPOS-2679

                    CInfo.EnableEPrimeRx = false;
                    CInfo.EPrimeRxURL = string.Empty;
                    CInfo.EPrimeRxToken = string.Empty;
                    CInfo.SSOIdentifier = (int)SSOIdentifier.Email;  //PRIMEPOS-3484
                }

                oDS = null;
                oDS = oSearch.SearchData("Select * from util_POSSet where stationid='" + Configuration.StationID + "'");

                if (oDS.Tables[0].Rows.Count > 0)
                {
                    #region set cposset values
                    CPOSSet = new POSSET();
                    CPOSSet.UseScanner = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseScanner"].ToString());
                    CPOSSet.UsePoleDisplay = convertNullToBoolean(oDS.Tables[0].Rows[0]["UsePoleDsp"].ToString());
                    CPOSSet.PD_PORT = convertNullToString(oDS.Tables[0].Rows[0]["PD_PORT"].ToString());
                    CPOSSet.PDP_BAUD = convertNullToString(oDS.Tables[0].Rows[0]["PDP_BAUD"].ToString());
                    CPOSSet.PDP_PARITY = convertNullToString(oDS.Tables[0].Rows[0]["PDP_PARITY"].ToString());
                    CPOSSet.PDP_DBITS = convertNullToString(oDS.Tables[0].Rows[0]["PDP_DBITS"].ToString());
                    CPOSSet.PDP_STOPB = convertNullToString(oDS.Tables[0].Rows[0]["PDP_STOPB"].ToString());
                    CPOSSet.PDP_CODE = convertNullToString(oDS.Tables[0].Rows[0]["PDP_CODE"].ToString());
                    CPOSSet.PDP_CLSCD = convertNullToString(oDS.Tables[0].Rows[0]["PDP_CLSCD"].ToString());
                    CPOSSet.PDP_CUROFF = convertNullToString(oDS.Tables[0].Rows[0]["PDP_CUROFF"].ToString());

                    CPOSSet.PD_MSG = convertNullToString(oDS.Tables[0].Rows[0]["PD_MSG"].ToString());
                    CPOSSet.PD_LINES = convertNullToInt(oDS.Tables[0].Rows[0]["PD_LINES"].ToString());
                    CPOSSet.PD_LINELEN = convertNullToInt(oDS.Tables[0].Rows[0]["PD_LINELEN"].ToString());
                    CPOSSet.PD_INTRFCE = convertNullToString(oDS.Tables[0].Rows[0]["PD_INTRFCE"].ToString());
                    CPOSSet.USECASHDRW = convertNullToBoolean(oDS.Tables[0].Rows[0]["USECASHDRW"].ToString());
                    CPOSSet.CD_TYPE = convertNullToString(oDS.Tables[0].Rows[0]["CD_TYPE"].ToString());
                    CPOSSet.CD_PORT = convertNullToString(oDS.Tables[0].Rows[0]["CD_PORT"].ToString());
                    CPOSSet.CDP_BAUD = convertNullToString(oDS.Tables[0].Rows[0]["CDP_BAUD"].ToString());
                    CPOSSet.CDP_PARITY = convertNullToString(oDS.Tables[0].Rows[0]["CDP_PARITY"].ToString());
                    CPOSSet.CDP_DBITS = convertNullToString(oDS.Tables[0].Rows[0]["CDP_DBITS"].ToString());
                    CPOSSet.CDP_STOPB = convertNullToString(oDS.Tables[0].Rows[0]["CDP_STOPB"].ToString());
                    CPOSSet.CDP_CODE = convertNullToString(oDS.Tables[0].Rows[0]["CDP_CODE"].ToString());
                    CPOSSet.CDP_BAUD2 = convertNullToString(oDS.Tables[0].Rows[0]["CDP_BAUD2"].ToString());
                    CPOSSet.CDP_PARITY2 = convertNullToString(oDS.Tables[0].Rows[0]["CDP_PARIT2"].ToString());
                    CPOSSet.CDP_DBITS2 = convertNullToString(oDS.Tables[0].Rows[0]["CDP_DBITS2"].ToString());
                    CPOSSet.CDP_STOPB2 = convertNullToString(oDS.Tables[0].Rows[0]["CDP_STOPB2"].ToString());
                    CPOSSet.CDP_CODE2 = convertNullToString(oDS.Tables[0].Rows[0]["CDP_CODE2"].ToString());
                    CPOSSet.RP_TYPE = convertNullToString(oDS.Tables[0].Rows[0]["RP_TYPE"].ToString());
                    CPOSSet.RP_Name = convertNullToString(oDS.Tables[0].Rows[0]["RP_Name"].ToString());
                    CPOSSet.LabelPrinter = convertNullToString(oDS.Tables[0].Rows[0]["LabelPrinter"].ToString());
                    CPOSSet.ONLINECCT = convertNullToBoolean(oDS.Tables[0].Rows[0]["ONLINECCT"].ToString());
                    //CPOSSet.AllItemDisc = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllItemDisc"].ToString());//Commented By Shitaljit(QuicSolv) 7 Sept 2011
                    CPOSSet.AllItemDisc = convertNullToString(oDS.Tables[0].Rows[0]["AllItemDisc"].ToString());//Added By Shitaljit(QuicSolv) 7 Sept 2011
                    CPOSSet.RXCode = convertNullToString(oDS.Tables[0].Rows[0]["RXCode"].ToString());
                    //CPOSSet.RP_CCPrint = convertNullToInt(oDS.Tables[0].Rows[0]["RP_CCPrint"].ToString());
                    CPOSSet.SingleUserLogin = convertNullToBoolean(oDS.Tables[0].Rows[0]["SingleUserLogin"].ToString());
                    CPOSSet.MergeCCWithRcpt = convertNullToBoolean(oDS.Tables[0].Rows[0]["MergeCCWithRcpt"].ToString());
                    CPOSSet.PrintRXDescription = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintRXDescription"].ToString());
                    CPOSSet.UseRcptForCloseStation = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseRcptForCloseStation"].ToString());
                    CPOSSet.UseRcptForEOD = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseRcptForEOD"].ToString());
                    CPOSSet.UsePrimeRX = convertNullToBoolean(oDS.Tables[0].Rows[0]["UsePrimeRX"].ToString());
                    CPOSSet.LoginBeforeTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["LoginBeforeTrans"].ToString());
                    CPOSSet.Inactive_Interval = convertNullToInt(oDS.Tables[0].Rows[0]["Inactive_Interval"].ToString());
                    CPOSSet.StationName = oDS.Tables[0].Rows[0]["stationname"].ToString();
                    CPOSSet.RoundTaxValue = convertNullToBoolean(oDS.Tables[0].Rows[0]["RoundTaxValue"].ToString());
                    CPOSSet.RXInsToIgnoreCopay = oDS.Tables[0].Rows[0]["RXInsToIgnoreCopay"].ToString();
                    CPOSSet.RestrictConcurrentLogin = convertNullToBoolean(oDS.Tables[0].Rows[0]["RestrictConcurrentLogin"].ToString());
                    CPOSSet.UseSigPad = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseSigPad"].ToString());
                    CPOSSet.SigPadHostAddr = oDS.Tables[0].Rows[0]["SigPadHostAddr"].ToString();
                    CPOSSet.DispSigOnTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["DispSigOnTrans"].ToString());
                    CPOSSet.DisableNOPP = convertNullToBoolean(oDS.Tables[0].Rows[0]["DisableNOPP"].ToString());
                    CPOSSet.ApplyPriceValidation = convertNullToBoolean(oDS.Tables[0].Rows[0]["ApplyPriceValidation"].ToString());

                    //Added By SRT(Prashant) On Date: 7/04/2009                    
                    //End of added
                    CPOSSet.AllowManualCCTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowManualCCTrans"].ToString());
                    //Added By Dharmendra(SRT) on Nov-13-08 to add those settings related with Card Payments & Pin Pad

                    CPOSSet.PaymentProcessor = convertNullToString(oDS.Tables[0].Rows[0]["PaymentProcessor"].ToString());
                    CPOSSet.AvsMode = convertNullToString(oDS.Tables[0].Rows[0]["AvsMode"].ToString());
                    CPOSSet.TxnTimeOut = convertNullToString(oDS.Tables[0].Rows[0]["TxnTimeOut"].ToString());
                    CPOSSet.UsePinPad = convertNullToBoolean(oDS.Tables[0].Rows[0]["UsePinPad"].ToString());
                    CPOSSet.OrigPinPadModel = convertNullToString(oDS.Tables[0].Rows[0]["PinPadModel"].ToString());
                    CPOSSet.PinPadModel = convertNullToString(oDS.Tables[0].Rows[0]["PinPadModel"].ToString());
                    if (CPOSSet.PinPadModel.Contains("TouchScreen"))
                        CPOSSet.IsTouchScreen = true;
                    else
                        CPOSSet.IsTouchScreen = false;
                    if (CPOSSet.PinPadModel.Equals("Ingenico_Lane7000", StringComparison.OrdinalIgnoreCase))
                        CPOSSet.PinPadModel = "WPIngenico_ISC480";
                    CPOSSet.PinPadBaudRate = convertNullToString(oDS.Tables[0].Rows[0]["PinPadBaudRate"].ToString());
                    CPOSSet.PinPadPairity = convertNullToString(oDS.Tables[0].Rows[0]["PinPadPairity"].ToString());
                    CPOSSet.PinPadPortNo = convertNullToString(oDS.Tables[0].Rows[0]["PinPadPortNo"].ToString());
                    CPOSSet.PinPadDataBits = convertNullToString(oDS.Tables[0].Rows[0]["PinPadDataBits"].ToString());
                    CPOSSet.PinPadDispMesg = convertNullToString(oDS.Tables[0].Rows[0]["PinPadDispMesg"].ToString());
                    CPOSSet.PinPadKeyEncryptionType = convertNullToString(oDS.Tables[0].Rows[0]["PinPadKeyEncryptionType"].ToString());
                    CPOSSet.HeartBeatTime = convertNullToString(oDS.Tables[0].Rows[0]["HeartBeatTime"].ToString());
                    CPOSSet.ProcessOnLine = convertNullToBoolean(oDS.Tables[0].Rows[0]["ProcessOnLine"].ToString()); //Added by Dharmendra(SRT) on Nov-03-08 to make ProcessOnLine configurable as suggested by MMS                                                            

                    CPOSSet.ReceiptPrinterType = convertNullToString(oDS.Tables[0].Rows[0]["ReceiptPrinterType"].ToString()); //Added by Prog1 15Oct09
                    CPOSSet.MaxCATransAmt = convertNullToInt(oDS.Tables[0].Rows[0]["MaxCATransAmt"].ToString()); //Added by Prog1 20Jan2010

                    //ShowAuthorization
                    CPOSSet.ShowAuthorization = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowAuthorization"].ToString()); // Added By Dharmendra on May-15-09
                    //Added By SRT(Abhishek) Date : 17-Aug-2009
                    CPOSSet.FetchUnbilledRx = convertNullToInt(oDS.Tables[0].Rows[0]["FetchUnbilledRx"].ToString());    //PRIMEPOS-2398 04-Jan-2021 JY modified
                    //End Of Added By SRT(Abhishek)
                    //Added By SRT(Ritesh Parekh) Date : 20-Aug-2009
                    //Initialize Database setting for CaptureSigForDebit
                    CPOSSet.CaptureSigForDebit = convertNullToBoolean(oDS.Tables[0].Rows[0]["CaptureSigForDebit"].ToString());
                    CPOSSet.DispSigOnHouseCharge = convertNullToBoolean(oDS.Tables[0].Rows[0]["DispSigOnHouseCharge"].ToString());  // Added by Manoj 11/21/2011
                    CPOSSet.CaptureSigForEBT = convertNullToBoolean(oDS.Tables[0].Rows[0]["CaptureSigForEBT"].ToString()); //Added by Manoj 7/2/2013
                    CPOSSet.SkipF10Sign = convertNullToBoolean(oDS.Tables[0].Rows[0]["SkipF10Sign"].ToString()); //Added by Manoj 9/25/2013
                    CPOSSet.SkipAmountSign = convertNullToBoolean(oDS.Tables[0].Rows[0]["SkipAmountSign"].ToString()); //Added by Manoj 9/25/2013
                    CPOSSet.SkipEMVCardSign = convertNullToBoolean(oDS.Tables[0].Rows[0]["SkipEMVCardSign"].ToString());
                    //End Of Added By SRT(Ritesh Parekh)
                    CPOSSet.DaysToResetPwd = convertNullToInt(oDS.Tables[0].Rows[0]["PWDEXPIREDAYS"].ToString());
                    CPOSSet.HPS_USERNAME = oDS.Tables[0].Rows[0]["HPS_USERNAME"].ToString();
                    CPOSSet.HPS_PASSWORD = oDS.Tables[0].Rows[0]["HPS_PASSWORD"].ToString();
                    if (oDS.Tables[0].Columns.Contains("TerminalID"))
                        CPOSSet.TerminalID = oDS.Tables[0].Rows[0]["TerminalID"].ToString();
                    else
                        CPOSSet.TerminalID = string.Empty;
                    CPOSSet.ALLOWDUP = convertNullToBoolean(oDS.Tables[0].Rows[0]["ALLOWDUP"].ToString());
                    CPOSSet.PreferReverse = convertNullToBoolean(oDS.Tables[0].Rows[0]["PreferReverse"].ToString());

                    //CPOSSet.NoOfCC = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfCC"].ToString());//Added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set                         
                    CPOSSet.NoOfReceipt = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfReceipt"].ToString());//Added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    
                    CPOSSet.NoOfOnHoldTransReceipt = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfOnHoldTransReceipt"].ToString());//Added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    
                    //CPOSSet.NoOfHCRC = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfHCRC"].ToString());//Added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    
                    CPOSSet.NoOfRARC = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfRARC"].ToString());//Added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    
                    //CPOSSet.NoOfCheckRC = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfCheckRC"].ToString());//Added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    
                    CPOSSet.NoOfGiftReceipt = convertNullToInt(oDS.Tables[0].Rows[0]["NoOfGiftReceipt"].ToString());//Added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    

                    //if(oDS.Tables[0].Rows[0]["AllowRxPicked"] == DBNull.Value)
                    //{
                    //    Configuration.AllowRxPicked = true;
                    //    CPOSSet.AllowRxPicked = true;
                    //}
                    //else
                    //{
                    //    Configuration.AllowRxPicked = Convert.ToBoolean(oDS.Tables[0].Rows[0]["AllowRxPicked"]);
                    //    CPOSSet.AllowRxPicked = Convert.ToBoolean(oDS.Tables[0].Rows[0]["AllowRxPicked"]);
                    //}
                    #region PRIMEPOS-2865 15-Jul-2020 JY Added
                    if (oDS.Tables[0].Rows[0]["AllowRxPicked"] == DBNull.Value)
                        Configuration.AllowRxPicked = CPOSSet.AllowRxPicked = 1;
                    else
                        Configuration.AllowRxPicked = CPOSSet.AllowRxPicked = convertNullToInt(oDS.Tables[0].Rows[0]["AllowRxPicked"]);
                    #endregion

                    //Added by Manoj 1/23/2013
                    CPOSSet.AllowPickedUpRxToTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowPickedUpRx"].ToString());

                    //Added by Manoj 4/2/2013
                    CPOSSet.ControlByID = convertNullToInt(oDS.Tables[0].Rows[0]["ControlByID"].ToString());    //PRIMEPOS-2547 03-Jul-2018 JY Modified
                    CPOSSet.AskVerificationIdMode = convertNullToInt(oDS.Tables[0].Rows[0]["AskVerificationIdMode"].ToString());    //PRIMEPOS-2547 11-Jul-2018 JY Added

                    //Added by Manoj 5/8/2013
                    CPOSSet.SkipDelSign = convertNullToBoolean(oDS.Tables[0].Rows[0]["SkipDeliverySign"].ToString());

                    //Add Ended
                    CPOSSet.ApplyGroupPriceOverCompanionItem = convertNullToBoolean(oDS.Tables[0].Rows[0]["ApplyGPOverCompItem"].ToString());
                    // Added By SRT(Gaurav) Date: 18/02/2009                    

                    //End Of Added
                    CPOSSet.ShowCustomerNotes = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowCustomerNotes"].ToString());
                    //Added By SRT(Gaurav) Date : 21-Jul-2009
                    CPOSSet.procFSAWithXLINK = convertNullToBoolean(oDS.Tables[0].Rows[0]["procFSAwithXCharge"].ToString());
                    //End Of Added By SRT(Gaurav)

                    //Added by SRT(Sachin) Date : 23 Feb 2010                    
                    //End of Added by SRT(Sachin) Date : 23 Feb 2010
                    //Added by SRT (Abhishek D) DAte : 12 March 2010
                    CPOSSet.AllowZeroAmtTransaction = convertNullToBoolean(oDS.Tables[0].Rows[0]["ALLOWZEROAMTTRANSACTION"].ToString());

                    //ADDED PRASHANT 5 JUN 2010

                    Boolean.TryParse(oDS.Tables[0].Rows[0]["PromptForAllTrans"].ToString(), out CPOSSet.PromptForAllTrans);
                    //CPOSSet.PromptForAllTrans = Convert.ToBoolean(oDS.Tables[0].Rows[0]["PromptForAllTrans"]);
                    //CPOSSet.PromptForReturnTrans = Convert.ToBoolean(oDS.Tables[0].Rows[0]["PromptForReturnTrans"]);
                    Boolean.TryParse(oDS.Tables[0].Rows[0]["PromptForReturnTrans"].ToString(), out CPOSSet.PromptForReturnTrans);
                    //END ADDED PRASHANT 5 JUN 2010
                    //End of Added by SRT (Abhishek D) DAte : 12 March 2010
                    CPOSSet.FetchFiledRx = convertNullToBoolean(oDS.Tables[0].Rows[0]["FetchFiledRx"].ToString());
                    CPOSSet.MaxCashLimitForStnCose = convertNullToDecimal(oDS.Tables[0].Rows[0]["MaxCashLimitForStnCose"].ToString());
                    CPOSSet.TurnOnEventLog = convertNullToBoolean(oDS.Tables[0].Rows[0]["TurnOnEventLog"].ToString());//Added By Shitaljit on 12/11/2013 to turn OFF/ON log
                    CPOSSet.IVULottoPassword = convertNullToString(oDS.Tables[0].Rows[0]["IVULottoPassword"].ToString());
                    CPOSSet.IVULottoTerminalID = convertNullToString(oDS.Tables[0].Rows[0]["IVULottoTerminalID"].ToString());
                    CPOSSet.IVULottoServerURL = convertNullToString(oDS.Tables[0].Rows[0]["IVULottoServerURL"].ToString());
                    CPOSSet.SelectMultipleTaxes = convertNullToBoolean(oDS.Tables[0].Rows[0]["SelectMultipleTaxes"].ToString());    //Sprint-19 - 2146 26-Dec-2014 JY Added to select multiple taxes functionality
                    CPOSSet.WP_SubID = convertNullToString(oDS.Tables[0].Rows[0]["WP_SubID"].ToString()); //Added by Rohit Nair on May-3-2016 for WorldPay Integration                    
                    CPOSSet.ShowRxNotes = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowRxNotes"].ToString());    //PRIMEPOS-2459 03-Apr-2019 JY Added
                    CPOSSet.ShowPatientNotes = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowPatientNotes"].ToString());    //PRIMEPOS-2459 03-Apr-2019 JY Added
                    CPOSSet.ShowItemNotes = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowItemNotes"].ToString());    //PRIMEPOS-2536 14-May-2019 JY Added
                    #region allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019
                    CPOSSet.AllowManualFirstMiles = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowManualFirstMiles"].ToString());
                    CPOSSet.SkipRxSignature = convertNullToBoolean(oDS.Tables[0].Rows[0]["SkipRxSignature"].ToString());
                    #endregion

                    #region PRIMEPOS-3455
                    CPOSSet.IsSecureDevice = convertNullToBoolean(oDS.Tables[0].Rows[0]["IsSecuredDevice"]);
                    CPOSSet.SecureDeviceModel = convertNullToString(oDS.Tables[0].Rows[0]["SecureDeviceModel"]);
                    CPOSSet.SecureDeviceSrNumber = convertNullToString(oDS.Tables[0].Rows[0]["SecureDeviceSrNumber"]);
                    #endregion


                    //if (CPOSSet.PaymentProcessor.ToUpper() == "WORLDPAY")
                    //{
                    //    POS.Resources.SigPadUtil.DefaultInstance.isISC = true;
                    //}
                    #region StoreCredit PRIMEPOS-2747
                    CPOSSet.EnableStoreCredit = convertNullToBoolean(oDS.Tables[0].Rows[0]["EnableStoreCredit"].ToString());
                    #endregion

                    #region PRIMEPOS-2996 22-Sep-2021 JY Added
                    CPOSSet.ReportPrinter = convertNullToString(oDS.Tables[0].Rows[0]["ReportPrinter"].ToString());
                    CPOSSet.ReceiptPrinterPaperSource = convertNullToString(oDS.Tables[0].Rows[0]["ReceiptPrinterPaperSource"].ToString());
                    CPOSSet.LabelPrinterPaperSource = convertNullToString(oDS.Tables[0].Rows[0]["LabelPrinterPaperSource"].ToString());
                    CPOSSet.ReportPrinterPaperSource = convertNullToString(oDS.Tables[0].Rows[0]["ReportPrinterPaperSource"].ToString());
                    #endregion

                    CheckValidValues(); //Added By Dharmendra (SRT) on Nov-18-08

                    #region PRIMEPOS-3167 07-Nov-2022 JY Commented
                    //CPOSSet.USePrimePO = convertNullToBoolean(oDS.Tables[0].Rows[0]["USePrimePO"].ToString()); // Added By Dharmendra on Apr-29-09
                    //CPOSSet.HostAddress = convertNullToString(oDS.Tables[0].Rows[0]["HostAddress"].ToString());
                    //CPOSSet.ConnectionTimer = convertNullToInt(oDS.Tables[0].Rows[0]["ConnectionTimer"].ToString());
                    //CPOSSet.PurchaseOrdTimer = convertNullToInt(oDS.Tables[0].Rows[0]["PurchaseOrderTimer"].ToString());
                    //CPOSSet.PriceUpdateTimer = convertNullToInt(oDS.Tables[0].Rows[0]["PriceUpdateTimer"].ToString());
                    //CPOSSet.RemoteURL = oDS.Tables[0].Rows[0]["RemoteURL"].ToString();
                    //CPOSSet.ConsiderReturnTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["ConsiderReturnTrans"].ToString());    //Sprint-27 - PRIMEPOS-2390 27-Sep-2017 JY Added 
                    //                                                                                                                //Changed By Prashant(SRT) on 22-04-09
                    //String updateVendorPrice = oDS.Tables[0].Rows[0]["UpdateVendorPrice"].ToString();
                    //if (updateVendorPrice == null || updateVendorPrice.Trim() == "")
                    //{
                    //    CPOSSet.UpdateVendorPrice = true;
                    //}
                    //else
                    //{
                    //    CPOSSet.UpdateVendorPrice = Convert.ToBoolean(oDS.Tables[0].Rows[0]["UpdateVendorPrice"].ToString());
                    //}
                    ////End of added
                    //CPOSSet.UpdateDescription = convertNullToBoolean(oDS.Tables[0].Rows[0]["UpdateDescription"].ToString());
                    //CPOSSet.Insert11DigitItem = convertNullToBoolean(oDS.Tables[0].Rows[0]["INSERT11DIGITITEM"].ToString());
                    //CPOSSet.IgnoreVendorSequence = convertNullToBoolean(oDS.Tables[0].Rows[0]["IgnoreVendorSequence"].ToString());
                    //CPOSSet.DefaultVendor = oDS.Tables[0].Rows[0]["DEFAULTVENDOR"].ToString();
                    //CPOSSet.UseDefaultVendor = convertNullToBoolean(oDS.Tables[0].Rows[0]["USEDEFAULTVENDOR"].ToString());
                    //CPOSSet.AutoPOSeq = convertNullToString(oDS.Tables[0].Rows[0]["AutoPOSeq"].ToString());
                    #endregion

                    #endregion set cposset values
                }
                else
                {
                    #region default values
                    CPOSSet = new POSSET();
                    CPOSSet.UseScanner = false;
                    CPOSSet.UsePoleDisplay = false;
                    CPOSSet.PD_PORT = "1";
                    CPOSSet.PDP_BAUD = "";
                    CPOSSet.PDP_PARITY = "";
                    CPOSSet.PDP_DBITS = "";
                    CPOSSet.PDP_STOPB = "";
                    CPOSSet.PDP_CODE = "";
                    CPOSSet.PDP_CLSCD = "";
                    CPOSSet.PDP_CUROFF = "";

                    CPOSSet.PD_MSG = "";
                    CPOSSet.PD_LINES = 0;
                    CPOSSet.PD_LINELEN = 0;
                    CPOSSet.PD_INTRFCE = "";
                    CPOSSet.USECASHDRW = false;
                    CPOSSet.CD_TYPE = "";
                    CPOSSet.CD_PORT = "";
                    CPOSSet.CDP_BAUD = "";
                    CPOSSet.CDP_PARITY = "";
                    CPOSSet.CDP_DBITS = "";
                    CPOSSet.CDP_STOPB = "";
                    CPOSSet.CDP_CODE = "";
                    CPOSSet.CDP_BAUD2 = "";
                    CPOSSet.CDP_PARITY2 = "";
                    CPOSSet.CDP_DBITS2 = "";
                    CPOSSet.CDP_STOPB2 = "";
                    CPOSSet.CDP_CODE2 = "";
                    CPOSSet.RP_TYPE = "";
                    CPOSSet.RP_Name = "";
                    CPOSSet.LabelPrinter = "";
                    CPOSSet.ONLINECCT = false;

                    CPOSSet.NoOfReceipt = 1;
                    CPOSSet.NoOfOnHoldTransReceipt = 1;
                    //CPOSSet.NoOfCC = 1;
                    //CPOSSet.NoOfHCRC = 1;
                    CPOSSet.NoOfRARC = 1;
                    //CPOSSet.NoOfCheckRC = 1;
                    CPOSSet.NoOfGiftReceipt = 1;
                    //CPOSSet.NoOfCashReceipts = 1;

                    //CPOSSet.AllItemDisc = false;//Commented By Shitaljit(QuicSolv) 7 Sept 2011
                    CPOSSet.AllItemDisc = "0";//Added By Shitaljit(QuicSolv) 7 Sept 2011
                    CPOSSet.RXCode = "";
                    //CPOSSet.RP_CCPrint = 1;
                    CPOSSet.SingleUserLogin = false;
                    CPOSSet.MergeCCWithRcpt = false;
                    CPOSSet.PrintRXDescription = false;
                    CPOSSet.UseRcptForCloseStation = false;
                    CPOSSet.UseRcptForEOD = false;
                    CPOSSet.UsePrimeRX = false;
                    CPOSSet.LoginBeforeTrans = false;
                    CPOSSet.Inactive_Interval = 60;
                    CPOSSet.RoundTaxValue = false;
                    CPOSSet.RXInsToIgnoreCopay = "";
                    CPOSSet.RestrictConcurrentLogin = false;
                    CPOSSet.UseSigPad = false;
                    CPOSSet.SigPadHostAddr = "tcp://localhost:8080/PrimePosHostSrv";
                    CPOSSet.DispSigOnTrans = false;
                    CPOSSet.DisableNOPP = false;
                    CPOSSet.ApplyPriceValidation = false;
                    CPOSSet.AllowManualCCTrans = false;
                    //Added By Dharmendra(SRT) on Nov-13-08 to add those settings related with Card Payments & Pin Pad
                    CPOSSet.PaymentProcessor = "PCCHARGE";
                    CPOSSet.AvsMode = "N";
                    CPOSSet.TxnTimeOut = "30";
                    CPOSSet.UsePinPad = true;
                    CPOSSet.PinPadModel = "VeriFone 1001/1000";
                    CPOSSet.PinPadBaudRate = "1200";
                    CPOSSet.PinPadPairity = "E";
                    CPOSSet.PinPadPortNo = "1";
                    CPOSSet.PinPadDataBits = "7";
                    CPOSSet.PinPadDispMesg = "WELCOME TO MMS";
                    CPOSSet.PinPadKeyEncryptionType = "DUKPT";
                    CPOSSet.HeartBeatTime = "10";
                    CPOSSet.ProcessOnLine = false; //Added by Dharmendra(SRT) on Nov-13-08 to make ProcessOnLine configurable as suggested by MMS
                    //Add Ended
                    CPOSSet.ApplyGroupPriceOverCompanionItem = false;
                    CPOSSet.ShowCustomerNotes = false;
                    //Added By SRT(Gaurav) Date: 21-Jul-2009
                    CPOSSet.procFSAWithXLINK = false;
                    //End Of Added By SRT(Gaurav)

                    //Added By SRT(Abhishek) Date: 17-Aug-2009
                    CPOSSet.FetchUnbilledRx = 0;    //PRIMEPOS-2398 04-Jan-2021 JY modified
                    //End Of Added By SRT(Abhishek)
                    //Added By SRT(Ritesh PArekh) Date: 20-Aug-2009
                    CPOSSet.CaptureSigForDebit = false;
                    //End Of Added By SRT(Ritesh Parekh)
                    CPOSSet.DispSigOnHouseCharge = false; // Added by Manoj 11/21/2011
                    CPOSSet.CaptureSigForEBT = false; //Added by Manoj 7/2/2013

                    //Added by Manoj 9/25/2013
                    CPOSSet.SkipF10Sign = false;
                    CPOSSet.SkipAmountSign = false;
                    CPOSSet.SkipEMVCardSign = false;

                    CPOSSet.ReceiptPrinterType = clsPOSDBConstants.STRINGIMAGE;
                    CPOSSet.MaxCATransAmt = 0;
                    CPOSSet.DaysToResetPwd = 90;
                    Configuration.AllowRxPicked = CPOSSet.AllowRxPicked = 0;  //PRIMEPOS-2865 15-Jul-2020 JY modified
                    CPOSSet.AllowPickedUpRxToTrans = false; //Added by Manoj 1/23/2013
                    CPOSSet.ControlByID = 0; //Added by Manoj 4/2/2013  //PRIMEPOS-2547 03-Jul-2018 JY Modified
                    CPOSSet.AskVerificationIdMode = 0;  //PRIMEPOS-2547 11-Jul-2018 JY Added
                    CPOSSet.SkipDelSign = false; //Added by Manoj 5/8/2013
                    CPOSSet.FetchFiledRx = false;
                    CPOSSet.MaxCashLimitForStnCose = 0;
                    //CPOSSet.NoOfCC = 1;
                    CPOSSet.TurnOnEventLog = false;//Added By Shitaljit on 12/11/2013 to turn OFF/ON log  
                    CPOSSet.SelectMultipleTaxes = false;
                    CPOSSet.WP_SubID = ""; //Added by Rohit Nair on May-3-2016 for WorldPay Integration
                    #region allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019
                    CPOSSet.AllowManualFirstMiles = false;
                    CPOSSet.SkipRxSignature = false;
                    #endregion
                    #region StoreCredit PRIMEPOS-2747
                    CPOSSet.EnableStoreCredit = false;
                    #endregion

                    #region PRIMEPOS-2996 22-Sep-2021 JY Added
                    CPOSSet.ReportPrinter = "";
                    CPOSSet.ReceiptPrinterPaperSource = "";
                    CPOSSet.LabelPrinterPaperSource = "";
                    CPOSSet.ReportPrinterPaperSource = "";
                    #endregion

                    #region PRIMEPOS-3455
                    CPOSSet.IsSecureDevice = false;
                    CPOSSet.SecureDeviceModel = "SREDKey 2";
                    CPOSSet.SecureDeviceSrNumber = "";
                    #endregion

                    #region PRIMEPOS-3167 07-Nov-2022 JY Commented
                    //CPOSSet.HostAddress = "localhost";
                    //CPOSSet.ConnectionTimer = 10000;//
                    //CPOSSet.PurchaseOrdTimer = 10000;//
                    //CPOSSet.PriceUpdateTimer = 10000;
                    ////Added By Prashant(SRT) on 15-04-2009
                    //CPOSSet.UpdateVendorPrice = true;
                    ////End of added
                    //CPOSSet.UpdateDescription = false;
                    ////Added By SRT(Prashant) On Date: 7/04/2009
                    //CPOSSet.UseDefaultVendor = false;
                    //CPOSSet.DefaultVendor = "";
                    ////End of added
                    //CPOSSet.AutoPOSeq = "PREFERED";
                    #endregion

                    #endregion default values
                }

                #region PRIMEPOS-3207

                hyphenSetting = new HyphenSetting(); //PRIMEPOS-3207N
                try
                {
                    DataTable hypSet = opharm.GetHyphenSettings();
                    hyphenSetting.ApiVersion = "V1";  //PRIMEPOS-3394 : provided default value for api_version
                    if (hypSet != null && hypSet.Rows.Count > 0)
                    {
                        //hyphenSetting = new HyphenSetting(); //PRIMEPOS-3207N
                        foreach (DataRow dt in hypSet.Rows)
                        {
                            if (Convert.ToString(dt["FieldName"]) == "EnableHyphenIntegration")
                            {
                                hyphenSetting.EnableHyphenIntegration = Convert.ToString(dt["FieldValue"]);
                            }
                            //else if (Convert.ToString(dt["FieldName"]) == "ClientID")
                            //{
                            //    hyphenSetting.ClientID = Convert.ToString(dt["FieldValue"]);
                            //}
                            //else if (Convert.ToString(dt["FieldName"]) == "ClientSecret")
                            //{
                            //    hyphenSetting.ClientSecret = Convert.ToString(dt["FieldValue"]);
                            //}
                            //else if (Convert.ToString(dt["FieldName"]) == "VendorBaseUrl")
                            //{
                            //    hyphenSetting.VendorBaseUrl = Convert.ToString(dt["FieldValue"]);
                            //}
                            //else if (Convert.ToString(dt["FieldName"]) == "AuthEndpoint")
                            //{
                            //    hyphenSetting.AuthEndpoint = Convert.ToString(dt["FieldValue"]);
                            //}
                            //else if (Convert.ToString(dt["FieldName"]) == "AlertEndpoint")
                            //{
                            //    hyphenSetting.AlertEndpoint = Convert.ToString(dt["FieldValue"]);
                            //}
                            else if (Convert.ToString(dt["FieldName"]) == "AzureFunctionUrl")
                            {
                                hyphenSetting.AzureFunctionUrl = Convert.ToString(dt["FieldValue"]);
                            }
                            else if (Convert.ToString(dt["FieldName"]) == "AzureFunctionKeyCode")
                            {
                                hyphenSetting.AzureFunctionKeyCode = Convert.ToString(dt["FieldValue"]);
                            }
                            else if (Convert.ToString(dt["FieldName"]) == "InsuranceSet")
                            {
                                hyphenSetting.InsuranceSet = Convert.ToString(dt["FieldValue"]);
                            }
                            else if (Convert.ToString(dt["FieldName"]) == "RejectionCodeSet")
                            {
                                hyphenSetting.RejectionCodeSet = Convert.ToString(dt["FieldValue"]);
                            }
                            else if (Convert.ToString(dt["FieldName"]) == "API_VERSION")  //PRIMEPOS-3394
                            {
                                hyphenSetting.ApiVersion = Convert.ToString(dt["FieldValue"]);
                            }
                        }
                    }
                    else
                    {
                        hyphenSetting.EnableHyphenIntegration = "N";
                    }
                }
                catch (Exception ex)
                {
                    hyphenSetting.EnableHyphenIntegration = "N";
                    logger.Error(ex, "Configuration==>HyphenSetting(): An Exception Occured");
                }
                #endregion

                oDS = null;
                oDS = oSearch.SearchData("select * from CL_Setup where ID=(Select max(ID) from cl_setup)");
                if (oDS.Tables[0].Rows.Count > 0)
                {
                    #region set customer Loyalty info

                    CLoyaltyInfo = new CustomerLoyaltyInfo();

                    CLoyaltyInfo.UseCustomerLoyalty = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseCustomerLoyalty"].ToString());
                    CLoyaltyInfo.ProgramName = convertNullToString(oDS.Tables[0].Rows[0]["ProgramName"].ToString());
                    CLoyaltyInfo.CardRangeFrom = convertNullToInt64(oDS.Tables[0].Rows[0]["CardRangeFrom"].ToString());
                    CLoyaltyInfo.CardRangeTo = convertNullToInt64(oDS.Tables[0].Rows[0]["CardRangeTo"].ToString());

                    CLoyaltyInfo.DefaultCardExpiryDays = convertNullToInt(oDS.Tables[0].Rows[0]["DefaultCardExpiryDays"].ToString());

                    CLoyaltyInfo.IsCardPrepetual = convertNullToBoolean(oDS.Tables[0].Rows[0]["IsCardPrepetual"].ToString());
                    CLoyaltyInfo.PrintCLCouponSeparately = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintCopounWithReceipt"].ToString());
                    CLoyaltyInfo.PrintCLCouponOnlyIfTierIsReached = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintCLCouponOnlyIfTierIsReached"].ToString());   //Sprint-18 - 2039 01-Dec-2014 JY Added to print CL coupon only if tier is reached

                    Prefrences oPref = new Prefrences();
                    CLoyaltyInfo.ExcludeDepts = new ExcludeData() { Data = oPref.GetExcludedDeptIds(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeItems = new ExcludeData() { Data = oPref.GetExcludedItemIds(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeSubDepts = new ExcludeData() { Data = oPref.GetExcludedSubDeptIds(), IsDataChanged = false };

                    CLoyaltyInfo.ExcludeClCouponDepts = new ExcludeData() { Data = oPref.GetExcludedCLCouponDeptIds(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeClCouponItems = new ExcludeData() { Data = oPref.GetExcludedCLCouponItemIds(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeClCouponSubDepts = new ExcludeData() { Data = oPref.GetExcludedCLCouponSubDeptIds(), IsDataChanged = false };

                    CLoyaltyInfo.ExcludeItemsOnSale = convertNullToBoolean(oDS.Tables[0].Rows[0]["ExcludeItemsOnSale"].ToString());
                    CLoyaltyInfo.PrintMsgOnReceipt = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintMsgOnReceipt"].ToString());
                    CLoyaltyInfo.Message = oDS.Tables[0].Rows[0]["Message"].ToString();
                    CLoyaltyInfo.RedeemValue = convertNullToDecimal(oDS.Tables[0].Rows[0]["RedeemValue"].ToString());
                    CLoyaltyInfo.RedeemMethod = convertNullToInt(oDS.Tables[0].Rows[0]["RedeemMethod"].ToString());
                    CLoyaltyInfo.ShowCLCardInputOnTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowCLCardInputOnTrans"].ToString());
                    CLoyaltyInfo.PrintCoupon = convertNullToBoolean(oDS.Tables[0].Rows[0]["PrintCoupon"].ToString());
                    CLoyaltyInfo.ShowDiscountAppliedMsg = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowDiscountAppliedMsg"].ToString());
                    CLoyaltyInfo.AllowMultipleCouponsInTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["AllowMultipleCouponsInTrans"].ToString());
                    CLoyaltyInfo.IsTierValueInPercent = convertNullToBoolean(oDS.Tables[0].Rows[0]["IsTierValueInPercent"].ToString());
                    CLoyaltyInfo.IncludeItemsWithType = oDS.Tables[0].Rows[0]["IncludeItemsWithType"].ToString();

                    if (CLoyaltyInfo.IncludeItemsWithType.Length > 0)
                    {
                        if (CLoyaltyInfo.IncludeItemsWithType.Contains("R"))
                        {
                            CLoyaltyInfo.IncludeRXItems = true;
                        }
                        if (CLoyaltyInfo.IncludeItemsWithType.Contains("T"))
                        {
                            CLoyaltyInfo.IncludeOTCItems = true;
                        }
                    }

                    CLoyaltyInfo.PointsCalcMethod = "A";
                    if (oDS.Tables[0].Rows[0]["PointsCalcMethod"] != null && oDS.Tables[0].Rows[0]["PointsCalcMethod"].ToString() != "")
                    {
                        CLoyaltyInfo.PointsCalcMethod = oDS.Tables[0].Rows[0]["PointsCalcMethod"].ToString();
                    }
                    CLoyaltyInfo.ShowCLControlPane = convertNullToBoolean(oDS.Tables[0].Rows[0]["ShowCLControlPane"].ToString());
                    CLoyaltyInfo.DisableAutoPointCalc = convertNullToBoolean(oDS.Tables[0].Rows[0]["DisableAutoPointCalc"].ToString());
                    CLoyaltyInfo.DoNotGenerateCoupons = convertNullToBoolean(oDS.Tables[0].Rows[0]["DoNotGenerateCoupons"].ToString());
                    CLoyaltyInfo.ExcludeDiscountableItems = convertNullToBoolean(oDS.Tables[0].Rows[0]["ExcludeDiscountableItems"].ToString());
                    CLoyaltyInfo.ApplyCLOrCusDisc = convertNullToBoolean(oDS.Tables[0].Rows[0]["ApplyCLOrCusDisc"].ToString());
                    //Added By Shitaljit on 4Feb2014 for PRIMEPOS-1703 CL Tier incremental Scheme
                    CLoyaltyInfo.SingleCouponPerRewardTier = convertNullToBoolean(oDS.Tables[0].Rows[0]["SingleCouponPerRewardTier"].ToString());
                    //END
                    CLoyaltyInfo.ApplyDiscountOnlyIfTierIsReached = convertNullToBoolean(oDS.Tables[0].Rows[0]["ApplyDiscountOnlyIfTierIsReached"].ToString()); //Sprint-25 - PRIMEPOS-2297 21-Feb-2017 JY Added
                    #endregion set customer Loyalty info
                }
                else
                {
                    #region set customer Loyalty info

                    CLoyaltyInfo = new CustomerLoyaltyInfo();
                    CLoyaltyInfo.ProgramName = "Customer Loyalty Program";
                    CLoyaltyInfo.CardRangeFrom = 0;
                    CLoyaltyInfo.CardRangeTo = 0;
                    CLoyaltyInfo.DefaultCardExpiryDays = 0;

                    CLoyaltyInfo.IsCardPrepetual = false;
                    CLoyaltyInfo.PrintCLCouponSeparately = false;
                    CLoyaltyInfo.PrintCLCouponOnlyIfTierIsReached = false;  //Sprint-18 - 2039 01-Dec-2014 JY Added to print CL coupon only if tier is reached

                    CLoyaltyInfo.ExcludeDepts = new ExcludeData() { Data = new List<String>(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeItems = new ExcludeData() { Data = new List<String>(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeSubDepts = new ExcludeData() { Data = new List<String>(), IsDataChanged = false };

                    CLoyaltyInfo.ExcludeClCouponDepts = new ExcludeData() { Data = new List<String>(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeClCouponItems = new ExcludeData() { Data = new List<String>(), IsDataChanged = false };
                    CLoyaltyInfo.ExcludeClCouponSubDepts = new ExcludeData() { Data = new List<String>(), IsDataChanged = false };

                    CLoyaltyInfo.ExcludeItemsOnSale = false;
                    CLoyaltyInfo.PrintMsgOnReceipt = false;
                    CLoyaltyInfo.Message = string.Empty;
                    CLoyaltyInfo.RedeemValue = 0;
                    CLoyaltyInfo.RedeemMethod = 0;
                    CLoyaltyInfo.ShowCLCardInputOnTrans = false;
                    CLoyaltyInfo.PrintCoupon = false;
                    CLoyaltyInfo.ShowDiscountAppliedMsg = false;
                    CLoyaltyInfo.AllowMultipleCouponsInTrans = false;
                    CLoyaltyInfo.IsTierValueInPercent = false;
                    CLoyaltyInfo.IncludeOTCItems = false;
                    CLoyaltyInfo.IncludeRXItems = false;
                    CLoyaltyInfo.IncludeItemsWithType = string.Empty;
                    CLoyaltyInfo.PointsCalcMethod = "A";
                    CLoyaltyInfo.ShowCLControlPane = false;
                    CLoyaltyInfo.DisableAutoPointCalc = false;
                    CLoyaltyInfo.DoNotGenerateCoupons = false;
                    CLoyaltyInfo.ExcludeDiscountableItems = false;
                    CLoyaltyInfo.ApplyCLOrCusDisc = false;
                    //Added By Shitaljit on 4Feb2014 for PRIMEPOS-1703 CL Tier incremental Scheme
                    CLoyaltyInfo.SingleCouponPerRewardTier = false;
                    CLoyaltyInfo.ApplyDiscountOnlyIfTierIsReached = false;  //Sprint-25 - PRIMEPOS-2297 21-Feb-2017 JY Added
                    //END
                    #endregion set customer Loyalty info
                }


                ////PRIMEPOS-TOKENURL
                oDS = null;
                oDS = oSearch.SearchData("select * from MerchantConfig ");
                oMerchantConfig = new MerchantConfig();
                if (oDS.Tables[0].Rows.Count > 0)
                {
                    oMerchantConfig.VantivAccountUrl = oDS.Tables[0].Rows[0]["PaymentAccountAPI"].ToString();
                    oMerchantConfig.VantivTokenUrl = oDS.Tables[0].Rows[0]["CreditCardAPI"].ToString();
                    oMerchantConfig.VantivReportUrl = oDS.Tables[0].Rows[0]["VantiveReportApi"].ToString();//PRIMEPOS-3156
                }

                #region Sprint-26 - PRIMEPOS-2441 21-Jul-2017 JY Commented
                //#region PRIMEPOS-2227 05-May-2017 JY Added code to get merchant info settings
                //oDS = null;
                //oDS = oSearch.SearchData("SELECT * FROM MerchantConfig");
                //lstMerchantConfig = new List<MerchantConfig>();
                //if (oDS.Tables[0].Rows.Count > 0)
                //{
                //    objMerchantConfig = new MerchantConfig();

                //    objMerchantConfig.User_ID = convertNullToString(oDS.Tables[0].Rows[0]["User_ID"].ToString());
                //    objMerchantConfig.Merchant = convertNullToString(oDS.Tables[0].Rows[0]["Merchant"].ToString());
                //    objMerchantConfig.Processor_ID = convertNullToString(oDS.Tables[0].Rows[0]["Processor_ID"].ToString());
                //    objMerchantConfig.Payment_Server = convertNullToString(oDS.Tables[0].Rows[0]["Payment_Server"].ToString());
                //    objMerchantConfig.Port_No = convertNullToString(oDS.Tables[0].Rows[0]["Port_No"].ToString());
                //    objMerchantConfig.Payment_Client = convertNullToString(oDS.Tables[0].Rows[0]["Payment_Client"].ToString());
                //    objMerchantConfig.Payment_ResultFile = convertNullToString(oDS.Tables[0].Rows[0]["Payment_ResultFile"].ToString());
                //    objMerchantConfig.Application_Name = convertNullToString(oDS.Tables[0].Rows[0]["Application_Name"].ToString());
                //    objMerchantConfig.XCClientUITitle = convertNullToString(oDS.Tables[0].Rows[0]["XCClientUITitle"].ToString());
                //    objMerchantConfig.LicenseID = convertNullToString(oDS.Tables[0].Rows[0]["LicenseID"].ToString());
                //    objMerchantConfig.SiteID = convertNullToString(oDS.Tables[0].Rows[0]["SiteID"].ToString());
                //    objMerchantConfig.DeviceID = convertNullToString(oDS.Tables[0].Rows[0]["DeviceID"].ToString());
                //    objMerchantConfig.URL = convertNullToString(oDS.Tables[0].Rows[0]["URL"].ToString());
                //    objMerchantConfig.VCBin = convertNullToString(oDS.Tables[0].Rows[0]["VCBin"].ToString());
                //    objMerchantConfig.MCBin = convertNullToString(oDS.Tables[0].Rows[0]["MCBin"].ToString());
                //}
                //else
                //{
                //    objMerchantConfig = new MerchantConfig();

                //    objMerchantConfig.User_ID = "";
                //    objMerchantConfig.Merchant = "";
                //    objMerchantConfig.Processor_ID = "";
                //    objMerchantConfig.Payment_Server = "localhost";
                //    objMerchantConfig.Port_No = 0.ToString();
                //    objMerchantConfig.Payment_Client = "";
                //    objMerchantConfig.Payment_ResultFile = "";
                //    objMerchantConfig.Application_Name = "PrimePOS";
                //    objMerchantConfig.XCClientUITitle = "Prime POS CC Processing";
                //    objMerchantConfig.LicenseID = "";
                //    objMerchantConfig.SiteID = "";
                //    objMerchantConfig.DeviceID = "";
                //    objMerchantConfig.URL = "";
                //    objMerchantConfig.VCBin = "";
                //    objMerchantConfig.MCBin = "";
                //}
                ////initialise list
                //lstMerchantConfig.Add(objMerchantConfig);
                //#endregion
                #endregion

                #region PRIMEPOS-3167 07-Nov-2022 JY Added
                oDS = null;
                oDS = oSearch.SearchData("SELECT * FROM PrimeEDISetting");
                if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                {
                    CPrimeEDISetting.UsePrimePO = convertNullToBoolean(oDS.Tables[0].Rows[0]["UsePrimePO"].ToString());
                    CPrimeEDISetting.HostAddress = convertNullToString(oDS.Tables[0].Rows[0]["HostAddress"].ToString());
                    CPrimeEDISetting.ConnectionTimer = convertNullToInt(oDS.Tables[0].Rows[0]["ConnectionTimer"].ToString());
                    CPrimeEDISetting.PurchaseOrderTimer = convertNullToInt(oDS.Tables[0].Rows[0]["PurchaseOrderTimer"].ToString());
                    CPrimeEDISetting.PriceUpdateTimer = convertNullToInt(oDS.Tables[0].Rows[0]["PriceUpdateTimer"].ToString());
                    CPrimeEDISetting.RemoteURL = oDS.Tables[0].Rows[0]["RemoteURL"].ToString();
                    CPrimeEDISetting.ConsiderReturnTrans = convertNullToBoolean(oDS.Tables[0].Rows[0]["ConsiderReturnTrans"].ToString());    //Sprint-27 - PRIMEPOS-2390 27-Sep-2017 JY Added 
                    String updateVendorPrice = oDS.Tables[0].Rows[0]["updateVendorPrice"].ToString();
                    if (updateVendorPrice == null || updateVendorPrice.Trim() == "")
                    {
                        CPrimeEDISetting.UpdateVendorPrice = true;
                    }
                    else
                    {
                        CPrimeEDISetting.UpdateVendorPrice = Convert.ToBoolean(oDS.Tables[0].Rows[0]["updateVendorPrice"].ToString());
                    }
                    CPrimeEDISetting.UpdateDescription = convertNullToBoolean(oDS.Tables[0].Rows[0]["UpdateDescription"].ToString());
                    CPrimeEDISetting.Insert11DigitItem = convertNullToBoolean(oDS.Tables[0].Rows[0]["Insert11DigitItem"].ToString());
                    CPrimeEDISetting.IgnoreVendorSequence = convertNullToBoolean(oDS.Tables[0].Rows[0]["IgnoreVendorSequence"].ToString());
                    CPrimeEDISetting.DefaultVendor = oDS.Tables[0].Rows[0]["DefaultVendor"].ToString();
                    CPrimeEDISetting.UseDefaultVendor = convertNullToBoolean(oDS.Tables[0].Rows[0]["UseDefaultVendor"].ToString());
                    CPrimeEDISetting.AutoPOSeq = convertNullToString(oDS.Tables[0].Rows[0]["AutoPOSeq"].ToString());
                }
                else
                {
                    CPrimeEDISetting.UsePrimePO = false;
                    CPrimeEDISetting.HostAddress = "localhost";
                    CPrimeEDISetting.ConnectionTimer = 10000;
                    CPrimeEDISetting.PurchaseOrderTimer = 10000;
                    CPrimeEDISetting.PriceUpdateTimer = 10000;
                    CPrimeEDISetting.UpdateVendorPrice = true;
                    CPrimeEDISetting.UpdateDescription = false;
                    CPrimeEDISetting.DefaultVendor = "";
                    CPrimeEDISetting.UseDefaultVendor = false;
                    CPrimeEDISetting.AutoPOSeq = "PREFERED";
                }
                #endregion

                PreferenceSvr oPreferenceSvr = new PreferenceSvr();
                oPreferenceSvr.GetPayTypesReceipts();    //PRIMEPOS-2308 16-May-2018 JY Added
                oDS.Dispose();

                MMSChargeAccount.RemoteSettings.DBServer = CInfo.RemoteDBServer;
                MMSChargeAccount.RemoteSettings.Catalog = CInfo.RemoteCatalog;
                MMSChargeAccount.RemoteSettings.UseRemoteServer = CInfo.UseRemoteServer;
            }
            catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Added By Shitaljit(QuicSolv) on 14 July 2011
        //To Formate Company Telephone no to standard formate(XXX)XXX-XXXX
        public static string FormateCompanyTelNo(string companyTelNo)
        {
            StringBuilder sb = new StringBuilder();
            string formatedTeNo = "";
            for (int i = 0; i < companyTelNo.Length; i++)
            {
                if ((companyTelNo[i] >= '0' && companyTelNo[i] <= '9'))
                    sb.Append(companyTelNo[i]);
            }
            formatedTeNo = sb.ToString();
            if (formatedTeNo != "" && formatedTeNo.Length >= 10)
            {
                formatedTeNo = "(" + formatedTeNo.Substring(0, 3) + ")" + formatedTeNo.Substring(3, 3) + "-" + formatedTeNo.Substring(6, 4);
                return formatedTeNo;
            }
            else
                return companyTelNo;
        }

        //Added By Shitaljit(Quicsolv) on 28 June 2011
        public static bool ValidateDeptCode(string DeptCode)
        {
            DepartmentData oDepartmentData = new DepartmentData();
            Department oBRDepartment = new Department();
            oDepartmentData = oBRDepartment.PopulateList("");
            for (int i = 0; i < oDepartmentData.Tables[0].Rows.Count; i++)
            {
                string str = oDepartmentData.Tables[0].Rows[i]["DeptCode"].ToString();
                if (DeptCode == oDepartmentData.Tables[0].Rows[i]["DeptCode"].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This function will check whether DEFDEPTID is exist or not in Util_Company_Info Table
        /// if not it will add a default department oin the Department Table with DeptCode as 999 if not available by 9999 and so on as per the availavility.
        /// </summary>
        public static void SetDefaultDepartment()
        {
            DepartmentData oDepartmentData = new DepartmentData();
            DepartmentRow oDepartmentRow;
            Department oBRDepartment = new Department();
            string DeptCode = "999";
            bool DeptCodeExist = false;
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            System.Data.IDbTransaction oTrans = null;
            Item oItem = new Item();
            ItemData oItemData = new ItemData();
            oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                if (CInfo.DefaultDeptId > 0)
                {
                    oItemData = oItem.PopulateList("  WHERE  DepartmentID = '" + CInfo.DefaultDeptId + "'");
                    if (isNullOrEmptyDataSet(oItemData) == false)
                    {
                        for (int i = 0; i < oItemData.Tables[0].Rows.Count; i++)
                        {
                            oItemData.Tables[0].Rows[i]["DepartmentID"] = DBNull.Value;
                        }
                        oItem.Persist(oItemData);
                    }
                }
                DeptCodeExist = ValidateDeptCode(DeptCode);
                while (DeptCodeExist)
                {
                    DeptCode += "9";
                    DeptCodeExist = ValidateDeptCode(DeptCode);
                }
                if (!DeptCodeExist)
                {
                    if (oDepartmentData != null)
                        oDepartmentData.Department.Rows.Clear();
                    oDepartmentRow = oDepartmentData.Department.AddRow(0, "", "", 0, 0, false, System.DateTime.Now, System.DateTime.Now, 0, 0, 0);//Sprint-18 - 2041 27-Oct-2014 JY  added new parameter
                    oDepartmentRow.DeptCode = DeptCode;
                    oDepartmentRow.DeptName = "System Default Department";
                    int DeptID = 0;
                    oBRDepartment.Persist(oDepartmentData, ref DeptID); //Sprint-22 20-Oct-2015 JY Added DeptID
                    oDepartmentData = oBRDepartment.Populate(DeptCode);
                    oDepartmentRow = oDepartmentData.Department.GetRowByID(DeptCode);
                    PreferenceSvr oPreSvr = new PreferenceSvr();
                    CInfo.DefaultDeptId = Configuration.convertNullToInt(oDepartmentRow.DeptID);
                    oPreSvr.UpdateCompanyInfo(oTrans, CInfo);


                    oItemData = oItem.PopulateList("  WHERE  DepartmentID is null OR  DepartmentID = ' ' OR LEN(DepartmentID) = 0 ");
                    if (isNullOrEmptyDataSet(oItemData) == false)
                    {
                        for (int i = 0; i < oItemData.Tables[0].Rows.Count; i++)
                        {
                            oItemData.Tables[0].Rows[i]["DepartmentID"] = oDepartmentRow.DeptID;
                        }
                        oItem.Persist(oItemData);
                    }

                    oTrans.Commit();
                }
            }
            catch (Exception exp)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //Added By Shitaljit(QuicSolv) on 3 June 2011
        public static void SetDefaultInvTransID()
        {
            InvTransTypeData oInvTransTypeData = new InvTransTypeData();
            InvTransType oInvTransType = new InvTransType();
            string strDefaultInvRec = "Inventory Received";
            string strDefaultIvRet = "Inventory Returned";
            oInvTransTypeData = oInvTransType.PopulateList("");
            if (oInvTransTypeData.InvTransType.Rows.Count > 0)
            {
                foreach (InvTransTypeRow oDRow in oInvTransTypeData.InvTransType.Rows)
                {
                    if (strDefaultInvRec == oDRow.TypeName.ToString() && oDRow.TransType == 0 && CInfo.DefalutInvRecievedID == 0)
                    {
                        CInfo.DefalutInvRecievedID = oDRow.ID;
                        CInfo.UpdateDeftInvRecv = true;
                    }
                    if (strDefaultIvRet == oDRow.TypeName.ToString() && oDRow.TransType == 1 && CInfo.DefaultInvReturnID == 0)
                    {
                        CInfo.DefaultInvReturnID = oDRow.ID;
                        CInfo.UpdateDeftInvRet = true;
                    }
                }
            }
        }

        /// <summary>
        /// Sets The Default Inventory Recieved and Return ID.
        /// If Inventory Recieved and Return ID are not found in InvTransType Table then insert the record to it.
        /// </summary>
        public static void SetDefaultTransType()
        {
            InvTransTypeData oInvTransTypeData = new InvTransTypeData();
            InvTransTypeRow oInvTransTypeRow;
            oInvTransTypeRow = oInvTransTypeData.InvTransType.AddRow(0, "", 0, "");
            InvTransType oInvTransType = new InvTransType();
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            System.Data.IDbTransaction oTrans = null;
            oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                oInvTransTypeData = oInvTransType.PopulateList("");
                if (oInvTransTypeData.InvTransType.Rows.Count == 0)
                {
                    InvTransTypeRow oNewRow = oInvTransTypeData.InvTransType.AddRow(0, "", 0, "");
                    oNewRow.TypeName = "Inventory Received";
                    oNewRow.TransType = 0;
                    oNewRow.UserID = Resources.Configuration.UserName;
                    oNewRow = oInvTransTypeData.InvTransType.AddRow(1, "", 1, "");
                    oNewRow.TypeName = "Inventory Returned";
                    oNewRow.TransType = 1;
                    oNewRow.UserID = Resources.Configuration.UserName;
                    oInvTransType.Persist(oInvTransTypeData);
                    SetDefaultInvTransID();
                }
                else
                {
                    SetDefaultInvTransID();
                }
                if (!CInfo.UpdateDeftInvRet)
                {
                    oInvTransTypeData.Clear();
                    oInvTransTypeRow = oInvTransTypeData.InvTransType.AddRow(0, "", 0, "");
                    oInvTransTypeRow.TypeName = "Inventory Returned";
                    oInvTransTypeRow.UserID = Resources.Configuration.UserName;
                    oInvTransTypeRow.TransType = 1;
                    oInvTransType.Persist(oInvTransTypeData);
                    SetDefaultInvTransID();
                }
                if (!CInfo.UpdateDeftInvRecv)
                {
                    oInvTransTypeData.Clear();
                    oInvTransTypeRow = oInvTransTypeData.InvTransType.AddRow(0, "", 0, "");
                    oInvTransTypeRow.TypeName = "Inventory Received";
                    oInvTransTypeRow.UserID = Resources.Configuration.UserName;
                    oInvTransTypeRow.TransType = 0;
                    oInvTransType.Persist(oInvTransTypeData);
                    SetDefaultInvTransID();
                }
                if (CInfo.UpdateDeftInvRecv || CInfo.UpdateDeftInvRet)
                {
                    PreferenceSvr oPreSvr = new PreferenceSvr();
                    oPreSvr.UpdateCompanyInfo(oTrans, CInfo);
                }
                oTrans.Commit();
            }
            catch (Exception exp)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public static string GetNPI()
        {
            string npi = string.Empty;
            try
            {
                Search oSearch = new Search();
                //DataSet oDS = oSearch.SearchData("select MerchantNo from Util_Company_Info"); //PRIMEPOS-2671 24-Apr-2019 JY Commented
                DataSet oDS = oSearch.SearchData("select PHNPINO from Util_Company_Info");  //PRIMEPOS-2671 24-Apr-2019 JY Added
                if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                {
                    npi = oDS.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            { }
            return npi;
        }

        private static void CheckValidValues()
        {
            if (CPOSSet.PaymentProcessor.Equals(string.Empty))
            {
                CPOSSet.PaymentProcessor = PAYMENTPROCESSOR;
            }
            if (CPOSSet.AvsMode.Equals(string.Empty))
            {
                CPOSSet.AvsMode = AVSMODE;
            }
            if (CPOSSet.TxnTimeOut.Equals(string.Empty))
            {
                CPOSSet.TxnTimeOut = TXNTIMEOUT;
            }
            //CPOSSet.UsePinPad = convertNullToBoolean(oDS.Tables[0].Rows[0]["UsePinPad"].ToString());
            if (CPOSSet.PinPadModel.Equals(string.Empty))
            {
                CPOSSet.PinPadModel = PINPADMODEL;
            }
            if (CPOSSet.PinPadBaudRate.Equals(string.Empty))
            {
                CPOSSet.PinPadBaudRate = PINPADBAUDRATE;
            }
            if (CPOSSet.PinPadPairity.Equals(string.Empty))
            {
                CPOSSet.PinPadPairity = PINPADPAIRITY;
            }
            if (CPOSSet.PinPadPortNo.Equals(string.Empty))
            {
                CPOSSet.PinPadPortNo = PINPADPORTNO;
            }
            if (CPOSSet.PinPadDataBits.Equals(string.Empty))
            {
                CPOSSet.PinPadDataBits = PINPADDATABITS;
            }

            if (CPOSSet.PinPadKeyEncryptionType.Equals(string.Empty))
            {
                CPOSSet.PinPadKeyEncryptionType = PINPADKEYENCRYPTIONTYPE;
            }
            if (CPOSSet.HeartBeatTime.Equals(string.Empty))
            {
                CPOSSet.HeartBeatTime = HEARTBEATTIME;
            }

            #region PRIMEPOS-3167 07-Nov-2022 JY Commented
            //Added By Dharmendra SRT on Mar-31-09
            //Setting default values for Prime PO server related variables
            //if (CPOSSet.HostAddress.Equals(string.Empty))
            //{
            //    CPOSSet.HostAddress = PRIMEPOHOSTADDRESSURL;
            //}
            //if (CPOSSet.ConnectionTimer < 1)
            //{
            //    CPOSSet.ConnectionTimer = PRIMEPOCONNECTIONTIMERVALUE;
            //}
            //if (CPOSSet.PriceUpdateTimer < 1)
            //{
            //    CPOSSet.PriceUpdateTimer = PRIMEPOPRICEUPDATETIMERVALUE;
            //}
            //if (CPOSSet.PurchaseOrdTimer < 1)
            //{
            //    CPOSSet.PurchaseOrdTimer = PRIMEPOPURCHASEORDERTIMERVALUE;
            //}
            //Added Till Here Mar-31-09
            #endregion

            DefaultPaymentProcessor = CPOSSet.PaymentProcessor; // Added By Dharmendra (SRT) on Dec-15-08 to assign the default value of PaymentProcessor
            //CPOSSet.ProcessOnLine = convertNullToBoolean(oDS.Tables[0].Rows[0]["ProcessOnLine"].ToString());
            if (CPOSSet.DaysToResetPwd < 1)
            {
                CPOSSet.DaysToResetPwd = PASSWORDRESETDAYS;
            }
            //if(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.DeviceSettings.ID, 0))    //PRIMEPOS-2484 04-Jun-2020 JY Commented
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.Preferences.ID, UserPriviliges.Permissions.DeviceSettings.ID)) //PRIMEPOS-2484 04-Jun-2020 JY Added
            {
                POS_Core.DataAccess.PreferenceSvr oPref = new POS_Core.DataAccess.PreferenceSvr();
                oPref.UpdateDeviceSettings(CPOSSet);
                oPref = null;
            }
        }

        #region PRIMEPOS-3167 07-Nov-2022 JY Added 
        private static void CheckValidValuesForPrimeEDISetting()
        {
            //Setting default values for Prime PO server related variables
            if (CPrimeEDISetting.HostAddress.Equals(string.Empty))
            {
                CPrimeEDISetting.HostAddress = PRIMEPOHOSTADDRESSURL;
            }
            if (CPrimeEDISetting.ConnectionTimer < 1)
            {
                CPrimeEDISetting.ConnectionTimer = PRIMEPOCONNECTIONTIMERVALUE;
            }
            if (CPrimeEDISetting.PriceUpdateTimer < 1)
            {
                CPrimeEDISetting.PriceUpdateTimer = PRIMEPOPRICEUPDATETIMERVALUE;
            }
            if (CPrimeEDISetting.PurchaseOrderTimer < 1)
            {
                CPrimeEDISetting.PurchaseOrderTimer = PRIMEPOPURCHASEORDERTIMERVALUE;
            }
        }
        #endregion
        // Constant values for all of the default settings.

        private static void setDefaultPref()
        {
            m_Window_Color = "176,196,222";
            m_Window_ForeColor = "0,0,0";
            m_Window_Button_Color1 = "176,224,230";
            m_Window_Button_Color2 = "0,0,128";
            m_Window_Button_ForeColor = "255,255,255";
            m_Active_BackColor = "255,192,128";
            m_Active_ForeColor = "0,0,0";

            m_Header_BackColor = "176,196,222";
            m_Header_ForeColor = "0,0,128";

            m_moveToQtyCol = false;
            m_AllowAddItemInTrans = false;
            m_showItemPad = false;
            m_showNumPad = false;
        }

        public static string convertNullToString(System.Object strValue)
        {
            if (strValue == null)
                return "";
            else
                return strValue.ToString();
        }

        public static bool convertNullToBoolean(String strValue)
        {
            if (strValue == null)
            {
                return false;
            }
            else if (strValue.Trim() == "")
            {
                return false;
            }
            else
                try
                {
                    if (strValue.ToUpper().Trim() == "TRUE" || strValue.Trim() == "1")
                    {
                        return true;
                    }
                    else if (strValue.ToUpper().Trim() == "FALSE" || strValue.Trim() == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return Convert.ToBoolean(strValue);
                    }
                }
                catch (Exception) { return false; }
        }

        //Start : Added By Amit Date 21 April 2011
        public static bool convertNullToBooleanTrue(String strValue)
        {
            if (strValue == null)
            {
                return true;
            }
            else if (strValue.Trim() == "")
            {
                return true;
            }
            else
                try
                {
                    if (strValue.ToUpper().Trim() == "TRUE" || strValue.Trim() == "1")
                    {
                        return true;
                    }
                    else if (strValue.ToUpper().Trim() == "FALSE" || strValue.Trim() == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return Convert.ToBoolean(strValue);
                    }
                }
                catch (Exception) { return false; }
        }

        /// <summary>
        /// Author :  Gaurav
        /// Date : 15/Jul/2009
        /// Details : This override will accept null value of any form and convert to the boolean with desired value.
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool convertNullToBoolean(Object objValue)
        {
            string strValue = string.Empty;
            if (objValue == null)
            {
                return false;
            }
            else
            {
                strValue = objValue.ToString();
            }

            if (strValue.Trim() == "")
            {
                return false;
            }
            else
                try
                {
                    if (strValue.ToUpper().Trim() == "TRUE" || strValue.Trim() == "1")
                    {
                        return true;
                    }
                    else if (strValue.ToUpper().Trim() == "FALSE" || strValue.Trim() == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return Convert.ToBoolean(strValue);
                    }
                }
                catch (Exception) { return false; }
        }

        //Moved from frmPOsTransaction By Shitaljit to make availabel to whole project.
        public static Color ExtractColor(String str)
        {
            Color c = Color.Empty;
            try
            {
                if (str.IndexOf(",") == -1)
                {
                    str = str.Substring(7, str.Length - 8);
                    c = Color.FromName(str);
                }
                else
                {
                    str = str.Substring(7, str.Length - 8);
                    string[] i = str.Split(',');
                    c = Color.FromArgb(Convert.ToInt32(i[0].Substring(2)), Convert.ToInt32(i[1].Substring(3)), Convert.ToInt32(i[2].Substring(3)), Convert.ToInt32(i[3].Substring(3)));
                }
                return c;
            }
            catch (Exception) { return Color.Empty; }
        }

        public static int convertBoolToInt(bool bValue)
        {
            try
            {
                if (bValue == true)
                    return 1;
                else
                    return 0;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Added By shitaljit to check that datatable is null or empty
        /// return true is null or empty and if has any rows return false
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool isNullOrEmptyDataTable(System.Data.DataTable dt)
        {
            try
            {
                if (dt == null)
                    return true;
                else if (dt != null && dt.Rows.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return true; }
        }

        /// <summary>
        /// Added By shitaljit to check that datatable is null or empty
        /// return true is null or empty and if has any rows return false
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool isNullOrEmptyDataSet(System.Data.DataSet ds)
        {
            try
            {
                if (ds == null)
                    return true;
                else if (ds != null && ds.Tables[0].Rows.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return true; }
        }

        public static int convertNullToInt(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    return Convert.ToInt32(strValue.ToString().Trim());
                }
                catch (Exception) { return 0; }
        }

        public static Int16 convertNullToShort(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    return Convert.ToInt16(strValue.ToString().Trim());
                }
                catch (Exception) { return 0; }
        }

        public static Int64 convertNullToInt64(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    return Convert.ToInt64(strValue.ToString().Trim());
                }
                catch (Exception) { return 0; }
        }

        public static double convertNullToDouble(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    return Convert.ToDouble(strValue.ToString().Trim());
                }
                catch (Exception) { return 0; }
        }

        public static System.Decimal convertNullToDecimal(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    if (strValue.ToString().StartsWith("(") && strValue.ToString().EndsWith(")"))
                    {
                        strValue = string.Concat("-", strValue.ToString().Replace("(", "").Replace(")", ""));
                    }
                    return Convert.ToDecimal(strValue.ToString().Trim());
                }
                catch (Exception) { return 0; }
        }

        public static String ConnectionString
        {
            get
            {
                //if(m_ConnString == "")
                //    buildConnectionString();
                //return m_ConnString;
                return DBConfig.ConnectionString;
            }
        }

        public static String ConnectionStringMaster
        {
            get
            {
                if (m_ConnStringMaster == "")
                {
                    ConnectionStringType = "MasterDatabase";
                    buildConnectionString();
                    ConnectionStringType = "";
                }
                return m_ConnStringMaster;
            }
        }

        public static bool ValidateStation(string StationID, bool bAutomation)
        {
            try
            {
                bool bStatus = ValidateStationID(StationID);    //PRIMEPOS-2826 28-Apr-2020 JY Added
                if (!bStatus) return false; //PRIMEPOS-2826 28-Apr-2020 JY Added

                BusinessRules.Search oSearch = new BusinessRules.Search();
                DataSet oDS = oSearch.SearchData("select stationname from util_POSSet where stationid='" + StationID.ToString() + "' ");
                #region PRIMEPOS-2826 27-Apr-2020 JY Commented
                //if (oDS == null)
                //{
                //    throw (new Exception("This station is not registered in PrimePOS."));
                //}
                //else if (oDS.Tables[0].Rows.Count == 0)
                //{
                //    throw (new Exception("This station is not registered in PrimePOS."));
                //}
                //else
                //{
                //    StationName = oDS.Tables[0].Rows[0]["stationname"].ToString();
                //    GetPOSWDirectoryPath();
                //}
                #endregion

                #region PRIMEPOS-2826 27-Apr-2020 JY Added

                if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                {
                    string strSQL = "SELECT DISTINCT MachineName FROM AutoUpdateAppVer WHERE AppName = 'POS' AND StationId = '" + StationID.Trim() + "' AND MachineName <> '" + Environment.MachineName + "'";
                    DataSet dsAutoUpdateAppVer = oSearch.SearchData(strSQL);
                    if (dsAutoUpdateAppVer != null && dsAutoUpdateAppVer.Tables.Count > 0 && dsAutoUpdateAppVer.Tables[0].Rows.Count > 0)
                    {
                        string strWK = string.Empty;
                        for (int i = 0; i < dsAutoUpdateAppVer.Tables[0].Rows.Count; i++)
                        {
                            if (strWK == string.Empty)
                            {
                                strWK = dsAutoUpdateAppVer.Tables[0].Rows[i]["MachineName"].ToString();
                            }
                            else
                            {
                                strWK = ", " + dsAutoUpdateAppVer.Tables[0].Rows[i]["MachineName"].ToString();
                            }
                        }
                        if (bAutomation == false && MessageBox.Show("Station \"" + StationID + "\" is already linked with : " + strWK + ",\nDo you want to use it on this workstation?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    StationName = oDS.Tables[0].Rows[0]["stationname"].ToString();
                    GetPOSWDirectoryPath();
                }
                else
                {
                    if (MessageBox.Show("Station \"" + StationID + "\" is not registered in PrimePOS,\nDo you want to create it?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string strSTATIONNAME = "STATION " + StationID;
                        bStatus = CreateStation(StationID, strSTATIONNAME);
                        if (bStatus)
                        {
                            StationName = strSTATIONNAME;
                            GetPOSWDirectoryPath();
                        }
                        return bStatus;
                    }
                    else
                    {
                        return false;
                    }
                }
                #endregion

                return true;
            }
            catch (Exception Ex) //PRIMEPOS-2504 20-Apr-2018 JY added 
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
                return false;
            }
        }

        #region PRIMEPOS-2826 27-Apr-2020 JY Added
        private static bool ValidateStationID(string StationID)
        {
            bool bStatus = true;
            if (!StationID.Contains("ScheduledTaskExecute"))    //PRIMEPOS-2485 29-Mar-2021 JY Added
            {
                if (StationID.Trim().Length > 3)
                {
                    MessageBox.Show("StationID  \"" + StationID + "\" should not be more than 3 characters", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bStatus = false;
                }
                else if (Configuration.convertNullToInt(StationID) == 0)
                {
                    MessageBox.Show("StationID \"" + StationID + "\" should be numeric", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bStatus = false;
                }
            }
            return bStatus;
        }

        public static bool CreateStation(string StationID, string strSTATIONNAME)
        {
            bool bstatus = false;
            string strSQL = string.Empty;
            try
            {
                BusinessRules.Search oSearch = new BusinessRules.Search();
                DataSet oDs = oSearch.SearchData("SELECT * FROM Util_POSSET WHERE STATIONID = '01'");
                if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
                {
                    bstatus = CreateStationRecord(StationID, "01", strSTATIONNAME);
                }
                else
                {
                    oDs = oSearch.SearchData("SELECT * FROM Util_POSSET WHERE STATIONID = '50'");
                    if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
                    {
                        bstatus = CreateStationRecord(StationID, "50", strSTATIONNAME);
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("01 or 50 station not found to being copied in the new station, so couldnt create " + StationID + " station");
                        bstatus = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
                bstatus = false;
            }
            return bstatus;
        }

        private static bool CreateStationRecord(string strNewStationID, string strCopyStationId, string strSTATIONNAME)
        {
            bool bStatus = false;
            try
            {
                string strSQL = "INSERT INTO Util_POSSET (STATIONID, USESCANNER, USEPOLEDSP, PD_PORT, PDP_BAUD, PDP_PARITY, PDP_DBITS, PDP_STOPB, PDP_CODE, PDP_CLSCD, PDP_CUROFF, PD_MSG, PD_LINES, PD_LINELEN, PD_INTRFCE, USECASHDRW," +
                            " CD_TYPE, CD_PORT, CDP_BAUD, CDP_PARITY, CDP_DBITS, CDP_STOPB, CDP_CODE, CDP_BAUD2, CDP_PARIT2, CDP_DBITS2, CDP_STOPB2, CDP_CODE2, RP_TYPE, RP_Name, ONLINECCT, RXCode, RP_CCPrint," +
                            " AllItemDisc, SingleUserLogin, MERGECCWITHRCPT, PRINTRXDESCRIPTION, USERCPTFORCLOSESTATION, USERCPTFOREOD, USEPRIMERX, LOGINBEFORETRANS, INACTIVE_INTERVAL, STATIONNAME, ROUNDTAXVALUE," +
                            " RXINSTOIGNORECOPAY, RESTRICTCONCURRENTLOGIN, UseSigPad, SigPadHostAddr, DispSigOnTrans, DISABLENOPP, APPLYPRICEVALIDATION, ALLOWMANUALCCTRANS, PAYMENTPROCESSOR, AVSMODE," +
                            " TXNTIMEOUT, USEPINPAD, PINPADMODEL, PINPADBAUDRATE, PINPADPAIRITY, PINPADPORTNO, PINPADDATABITS, PINPADDISPMESG, PINPADKEYENCRYPTIONTYPE, HEARTBEATTIME, PROCESSONLINE, HOSTADDRESS," +
                            " CONNECTIONTIMER, PRICEUPDATETIMER, PURCHASEORDERTIMER, APPLYGPOVERCOMPITEM, LABELPRINTER, UPDATEVENDORPRICE, USEPRIMEPO, USEPREFERREDVENDOR, PREFERREDVENDOR, SHOWAUTHORIZATION," +
                            " SHOWCUSTOMERNOTES, UPDATEDESCRIPTION, AUTOPOSEQ, USEDEFAULTVENDOR, DEFAULTVENDOR, PROCFSAWITHXCHARGE, FETCHUNBILLEDRX, CAPTURESIGFORDEBIT, RECEIPTPRINTERTYPE, MAXCATRANSAMT," +
                            " INSERT11DIGITITEM, ALLOWZEROAMTTRANSACTION, IGNOREVENDORSEQUENCE, PROMPTFORALLTRANS, PROMPTFORRETURNTRANS, RemoteURL, HPS_USERNAME, HPS_PASSWORD, ALLOWDUP, PreferReverse, KeyA," +
                            " KeyB, KeyC, KeyD, KeyE, AllowRxPicked, PWDEXPIREDAYS, DispSigOnHouseCharge, CHECKVENDORFORPRICEUPDATE, FetchFiledRx, AllowPickedUpRx, ControlByID, SKIPDELIVERYSIGN," +
                            " MaxCashLimitForStnCose, CaptureSigForEBT, SkipAmountSign, SkipF10Sign, NoOfCC, NoOfCashReceipts, NoOfReceipt, NoOfOnHoldTransReceipt, NoOfHCRC, NoOfRARC, NoOfCheckRC," +
                            " NoOfGiftReceipt, TurnOnEventLog, IVULottoTerminalID, IVULottoPassword, IVULottoServerURL, SelectMultipleTaxes, WP_SubID, ConsiderReturnTrans, TerminalID, AskVerificationIdMode," +
                            " SkipEMVCardSign, ShowRxNotes, ShowPatientNotes, ShowItemNotes, AllowManualFirstMiles, SkipRxSignature, EnableStoreCredit)" +
                            " SELECT '" + strNewStationID + "' AS STATIONID, USESCANNER, USEPOLEDSP, PD_PORT, PDP_BAUD, PDP_PARITY, PDP_DBITS, PDP_STOPB, PDP_CODE, PDP_CLSCD, PDP_CUROFF, PD_MSG, PD_LINES, PD_LINELEN, PD_INTRFCE, USECASHDRW," +
                            " CD_TYPE, CD_PORT, CDP_BAUD, CDP_PARITY, CDP_DBITS, CDP_STOPB, CDP_CODE, CDP_BAUD2, CDP_PARIT2, CDP_DBITS2, CDP_STOPB2, CDP_CODE2, RP_TYPE, RP_Name, ONLINECCT, RXCode, RP_CCPrint," +
                            " AllItemDisc, SingleUserLogin, MERGECCWITHRCPT, PRINTRXDESCRIPTION, USERCPTFORCLOSESTATION, USERCPTFOREOD, USEPRIMERX, LOGINBEFORETRANS, INACTIVE_INTERVAL, '" + strSTATIONNAME + "' AS STATIONNAME, ROUNDTAXVALUE," +
                            " RXINSTOIGNORECOPAY, RESTRICTCONCURRENTLOGIN, UseSigPad, SigPadHostAddr, DispSigOnTrans, DISABLENOPP, APPLYPRICEVALIDATION, ALLOWMANUALCCTRANS, PAYMENTPROCESSOR, AVSMODE," +
                            " TXNTIMEOUT, USEPINPAD, PINPADMODEL, PINPADBAUDRATE, PINPADPAIRITY, PINPADPORTNO, PINPADDATABITS, PINPADDISPMESG, PINPADKEYENCRYPTIONTYPE, HEARTBEATTIME, PROCESSONLINE, HOSTADDRESS," +
                            " CONNECTIONTIMER, PRICEUPDATETIMER, PURCHASEORDERTIMER, APPLYGPOVERCOMPITEM, LABELPRINTER, UPDATEVENDORPRICE, 0 AS USEPRIMEPO, USEPREFERREDVENDOR, PREFERREDVENDOR, SHOWAUTHORIZATION," +
                            " SHOWCUSTOMERNOTES, UPDATEDESCRIPTION, AUTOPOSEQ, USEDEFAULTVENDOR, DEFAULTVENDOR, PROCFSAWITHXCHARGE, FETCHUNBILLEDRX, CAPTURESIGFORDEBIT, RECEIPTPRINTERTYPE, MAXCATRANSAMT," +
                            " INSERT11DIGITITEM, ALLOWZEROAMTTRANSACTION, IGNOREVENDORSEQUENCE, PROMPTFORALLTRANS, PROMPTFORRETURNTRANS, RemoteURL, HPS_USERNAME, HPS_PASSWORD, ALLOWDUP, PreferReverse, KeyA," +
                            " KeyB, KeyC, KeyD, KeyE, AllowRxPicked, PWDEXPIREDAYS, DispSigOnHouseCharge, CHECKVENDORFORPRICEUPDATE, FetchFiledRx, AllowPickedUpRx, ControlByID, SKIPDELIVERYSIGN," +
                            " MaxCashLimitForStnCose, CaptureSigForEBT, SkipAmountSign, SkipF10Sign, NoOfCC, NoOfCashReceipts, NoOfReceipt, NoOfOnHoldTransReceipt, NoOfHCRC, NoOfRARC, NoOfCheckRC," +
                            " NoOfGiftReceipt, TurnOnEventLog, IVULottoTerminalID, IVULottoPassword, IVULottoServerURL, SelectMultipleTaxes, WP_SubID, ConsiderReturnTrans, TerminalID, AskVerificationIdMode," +
                            " SkipEMVCardSign, ShowRxNotes, ShowPatientNotes, ShowItemNotes, AllowManualFirstMiles, SkipRxSignature, EnableStoreCredit FROM Util_POSSET" +
                            " WHERE STATIONID = '" + strCopyStationId + "'";
                int nRowsAffected = DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);

                if (nRowsAffected > 0)
                {
                    strSQL = "DELETE FROM Util_PayTypeReceipts WHERE STATIONID = '" + strNewStationID + "'";
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);

                    strSQL = "INSERT INTO Util_PayTypeReceipts(STATIONID, PayTypeID, NoOfReceipts)" +
                            " SELECT '" + strNewStationID + "' AS STATIONID, PayTypeID, NoOfReceipts FROM Util_PayTypeReceipts WHERE STATIONID = '" + strCopyStationId + "'";
                    nRowsAffected = DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
                    if (nRowsAffected > 0)
                        bStatus = true;
                }
            }
            catch (Exception Ex)
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
                bStatus = false;
            }
            return bStatus;
        }
        #endregion

        public static string GetStationName(string sStationID)
        {
            BusinessRules.Search oSearch = new BusinessRules.Search();
            DataSet oDS = oSearch.SearchData("select stationname from util_POSSet where stationid='" + sStationID.ToString() + "' ");
            string sStationName = "";
            if (oDS == null)
            {
                sStationName = "";
            }
            else if (oDS.Tables[0].Rows.Count == 0)
            {
                sStationName = "";
            }
            else
            {
                sStationName = oDS.Tables[0].Rows[0]["stationname"].ToString();
            }
            return sStationName;
        }

        //Added By Dharmendra (SRT) on Dec-15-08 this value will be accessed in frmPoSProcessCC. to compare with the current PaymentProcessor Value
        public static string DefaultPaymentProcessor
        {
            get { return m_DefaultPaymentProcessor; }
            set { m_DefaultPaymentProcessor = value; }
        }

        //added by naim 21may2009
        public static bool PrintBarcode(string Barcode, long lnX, long lnY, int Height, int Width, string BCType, string Orientation, string sFilePath)
        {
            bool bPrOk = true;
            Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
            Mabry.Windows.Forms.Barcode.Barcode BC = new Mabry.Windows.Forms.Barcode.Barcode();
            BC.Size = new Size(Height - 15, Width);
            Image bci;
            BC.Data = Barcode;
            BC.DisplayAlignment = Mabry.Windows.Forms.Barcode.Barcode.TextAlignment.Standard;

            //BC.DisplayData=false;
            //BC.Xunit=8;
            //BC.BarcodeHeight = Height;

            switch (BCType)
            {
                case "CODE39":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code39;
                    Width *= 2;
                    Height *= 2;
                    BC.ChecksumStyle = Mabry.Windows.Forms.Barcode.Barcode.ChecksumStyles.None;
                    break;
                case "CODE128": // coDe 128
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128;
                    break;
                case "CODE128B":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128B;
                    break;
                case "CODE128A":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128A;
                    break;

                case "IL25": // interleaved 2 of 5
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Interleaved2of5;
                    break;

                case "ST25": // standard 2 of 5
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code2of5;
                    break;

                case "UPCA": // upc A
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.UPCA;
                    break;

                case "UPCE": // upc E
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.UPCE;
                    break;

                case "CODE39X":
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code39Extended;
                    break;
                default:
                    BC.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code39;
                    Width *= 2;
                    Height *= 2;
                    BC.ChecksumStyle = Mabry.Windows.Forms.Barcode.Barcode.ChecksumStyles.None;

                    break;
            }

            //			if (Orientation == "V")
            //				BC.Orientation = 90;

            BC.BarRatio = BC.SuggestedBarRatio;
            //BC.BarRatio = 1.75F;

            bci = BC.Image(Width * 3, Height * 3);
            if (Orientation == "V")
                bci.RotateFlip(RotateFlipType.Rotate90FlipNone);//.Rotate270FlipXY); //.Rotate270FlipNone);

            //PR.Graphics.DrawImage(BC.Barcode(this.PR.Graphics),lnX, lnY);
            bci.Save(sFilePath);
            return bPrOk;
        }

        public static byte[] GetImageData(String fileName)
        {
            //'Method to load an image from disk and return it as a bytestream
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            return (br.ReadBytes(Convert.ToInt32(br.BaseStream.Length)));
        }

        //Added By Dharmendra on Feb-06-09
        //This proper is used to indicate whether host server is running or not
        //public static bool HostServerRunningStatus
        //{
        //    get { return _HostServerRunningStatus; }
        //    set { _HostServerRunningStatus = value; }
        //}
        ////This property is used to indicate whether pad is plugged in with Pos terminal or not
        //public static bool PadConnectionStatus
        //{
        //    get { return _PadConnectionStatus; }
        //    set { _PadConnectionStatus = value; }
        //}
        //Added Till Here Feb-06-09
        public static string priceUpdatedby = string.Empty;

        public static string UpdatedBy
        {
            set
            {
                priceUpdatedby = value;
            }
            get
            {
                return priceUpdatedby;
            }
        }

        public static DateTime GetMonthStartDate(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime GetMonthEndDate(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        #region Sprint-20 27-May-2015 JY Added for auto updater
        static private bool bArgumentEmpty = false;
        public static bool ArgumentEmpty
        {
            get { return Configuration.bArgumentEmpty; }
            set { Configuration.bArgumentEmpty = value; }
        }
        #endregion

        #region PRIMEPOS-2553 27-Jun-2018 JY Added
        public static bool GetForceClose()
        {
            try
            {
                bool ForceClose = false;
                string strSql = "Select ForceClosed, TimeStemp from Util_Company_Info WHERE ForceClosed = 1";

                using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    bool bUpdateForceClosed = false;
                    IDataReader dReader = DataHelper.ExecuteReader(conn, CommandType.Text, strSql);
                    while (dReader.Read())
                    {
                        DateTime dTimeStamp = System.DateTime.Now.AddDays(-1);
                        try
                        {
                            dTimeStamp = Convert.ToDateTime(dReader["TimeStemp"]);
                        }
                        catch
                        {
                            bUpdateForceClosed = true;
                        }
                        if (dTimeStamp >= System.DateTime.Now)
                        {
                            logger.Trace("GetForceClose() - exe supposed to be closed");
                            ForceClose = true;
                        }
                        else
                        {
                            bUpdateForceClosed = true;
                        }
                    }
                    dReader.Close();

                    if (bUpdateForceClosed)
                    {
                        strSql = "UPDATE Util_Company_Info SET ForceClosed = 0, TimeStemp ='" + DateTime.Now.ToString() + "'";
                        DataHelper.ExecuteNonQuery(conn, CommandType.Text, strSql);
                    }
                    return ForceClose;
                }
            }
            catch (Exception Ex)
            {
                return false;
            }
        }
        #endregion

        #region PRIMEPOS-2774 13-Jan-2020 JY Added       
        public static bool ExportDataToCSV(System.Data.DataTable dt, string ExportFileName)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ExportFileName))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sb.Append(dc.ColumnName + ",");
                    }
                    sw.WriteLine(sb.ToString().Substring(0, sb.ToString().Length - 1));

                    foreach (DataRow dr in dt.Rows)
                    {
                        sb = new StringBuilder();
                        string rec = "";
                        foreach (DataColumn dc in dt.Columns)
                        {
                            rec = "\"" + dr[dc.ColumnName].ToString() + "\"";
                            sb.Append(rec + ",");
                        }
                        sw.WriteLine(sb.ToString().Substring(0, sb.ToString().Length - 1));
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ExportDataToCSV()");
                return false;
            }
            return true;
        }
        #endregion

        #region PRIMEPOS-2779 17-Jan-2020 JY Added
        public static void ExportData(System.Data.DataTable dt, string ExportFileName)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = ExportFileName;
            sfd.Filter = "CSV files (*.csv)|*.csv|PDF files (*.pdf)|*.pdf";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 1)
                {
                    bool ExcelImport = false;
                    if (MessageBox.Show("Will this file be Imported into MS EXCEL?", "EXCEL", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                        ExcelImport = true;

                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            sb.Append(dc.ColumnName + ",");
                        }
                        sw.WriteLine(sb.ToString().Substring(0, sb.ToString().Length - 1));

                        foreach (DataRow dr in dt.Rows)
                        {
                            sb = new StringBuilder();
                            string rec = "";
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (ExcelImport)
                                    rec = @"=" + "\"" + dr[dc.ColumnName].ToString() + "\"";
                                else
                                    rec = dr[dc.ColumnName].ToString();

                                sb.Append(rec + ",");
                            }

                            sw.WriteLine(sb.ToString().Substring(0, sb.ToString().Length - 1));
                        }
                    }
                }
                else if (sfd.FilterIndex == 2)
                {
                    //Document is inbuilt class, available in iTextSharp
                    iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 80, 50, 30, 65);
                    StringBuilder strData = new StringBuilder(string.Empty);
                    try
                    {
                        StringWriter sw = new StringWriter();
                        sw.WriteLine(Environment.NewLine);
                        sw.WriteLine(Environment.NewLine);
                        sw.WriteLine(Environment.NewLine);
                        sw.WriteLine(Environment.NewLine);
                        StreamWriter strWriter = new StreamWriter(sfd.FileName, false, Encoding.UTF8);

                        #region Write in StreamWriter
                        string sError = string.Empty;
                        int iNoOfColsinaPage = 7;
                        int iRunningColCount = 0;
                        strWriter.Write("<HTML><BODY><TABLE>");
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (iRunningColCount % iNoOfColsinaPage == 0)
                            {
                                strWriter.Write("<TR>");
                            }
                            strWriter.Write("<TD>&nbsp;" + dc.ColumnName + "&nbsp;</TD>");

                            iRunningColCount++;

                            if (iRunningColCount % iNoOfColsinaPage == 0)
                            {
                                strWriter.Write("</TR>");
                            }
                        }

                        foreach (DataRow dr in dt.Rows)
                        {
                            iRunningColCount = 0;
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (iRunningColCount % iNoOfColsinaPage == 0)
                                {
                                    strWriter.Write("<TR>");
                                }
                                strWriter.Write("<TD>" + dr[dc.ColumnName].ToString() + "&nbsp;</TD>");
                                iRunningColCount++;
                                if (iRunningColCount % iNoOfColsinaPage == 0)
                                {
                                    strWriter.Write("</TR>");
                                }
                            }
                        }

                        strWriter.Write("</TABLE></BODY></HTML>");
                        #endregion

                        strWriter.Close();
                        strWriter.Dispose();
                        iTextSharp.text.html.simpleparser.
                        StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                        styles.LoadTagStyle("ol", "leading", "16,0");
                        ArrayList objects;
                        styles.LoadTagStyle("li", "face", "garamond");
                        styles.LoadTagStyle("span", "size", "8px");
                        styles.LoadTagStyle("body", "font-family", "times new roman");
                        styles.LoadTagStyle("body", "font-size", "10px");
                        StreamReader sr = new StreamReader(sfd.FileName, Encoding.Default);
                        objects = iTextSharp.text.html.simpleparser.
                        HTMLWorker.ParseToList(sr, styles);
                        sr.Close();
                        PdfWriter.GetInstance(document, new FileStream(sfd.FileName, FileMode.Create));
                        document.Add(new iTextSharp.text.Header(iTextSharp.text.html.Markup.HTML_ATTR_STYLESHEET, "Style.css"));
                        document.Open();
                        document.NewPage();
                        for (int k = 0; k < objects.Count; k++)
                        {
                            document.Add((iTextSharp.text.IElement)objects[k]);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        document.Close();
                    }
                }
                // Open automatically
                System.Diagnostics.Process.Start(sfd.FileName);
                MessageBox.Show("File exported successfully");
            }
        }
        #endregion

        //PRIMEPOS-TOKENSALE Arvind Added 
        public static bool isPrimeRxPay = false;
        #region PRIMEPOS-2902 
        public static Dictionary<int, string> PayProviderIdAndNames = null;
        public static List<string> GetPayProviders()
        {
            PayProviderNames payProvider = new PayProviderNames();
            List<string> lstPayProvider = new List<string>();
            try
            {
                string json = payProvider.GetPayProviderNameAndId(CSetting.PrimeRxPayUrl + CSetting.PrimerxPayExtensionUrl, CSetting.PharmacyNPI, CSetting.PrimeRxPayClientId, CSetting.PrimeRxPaySecretKey);//2956

                PayProviderNames lstNames = JsonConvert.DeserializeObject<PayProviderNames>(json);

                PayProviderIdAndNames = new Dictionary<int, string>();

                foreach (var names in lstNames.PayProviders)
                {
                    if (names.PayProviderName.ToUpper() == "WORLDPAY")
                    {
                        lstPayProvider.Add("VANTIV");
                        PayProviderIdAndNames.Add(1, "VANTIV");
                    }
                    else
                    {
                        lstPayProvider.Add(names.PayProviderName.ToUpper());
                        PayProviderIdAndNames.Add(names.PayProviderID, names.PayProviderName.ToUpper());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetPayProvider" + ex.Message.ToString());
            }
            return lstPayProvider;
        }

        public static string PrimeRxPayHealthTest()
        {
            PayProviderNames payProvider = new PayProviderNames();
            return payProvider.HealthTest(CSetting.PrimeRxPayUrl + CSetting.PrimerxPayExtensionUrl, CSetting.PharmacyNPI, CSetting.PayProviderID.ToString(), CSetting.PrimeRxPayClientId, CSetting.PrimeRxPaySecretKey);//2956
        }
        #endregion

        //2927       
        public const string ShippingItem = "Shipping";

        #region PRIMEPOS-2386 26-Feb-2021 JY Added
        public static string GetCompanyLogoPath(object obj)
        {
            string strCompanyLogoPath = "";
            try
            {
                if (Configuration.CSetting.PrintCompanyLogo == true)
                {
                    string strCompanyLogoFileName = (Configuration.CSetting.CompanyLogoFileName != "") ? Configuration.CSetting.CompanyLogoFileName : "Pharmacy.JPG";

                    string sAppPath = Configuration.GetAppPath(obj);
                    if (sAppPath != string.Empty)
                    {
                        string sFilePath = sAppPath + @"\" + strCompanyLogoFileName;
                        if (System.IO.File.Exists(sFilePath))
                            strCompanyLogoPath = sFilePath;
                    }
                }
            }
            catch (Exception Ex)
            {
                strCompanyLogoPath = "";
            }
            return strCompanyLogoPath;
        }
        #endregion

        #region Sprint-22 - PRIMEPOS-2247 23-Nov-2015 JY Added to get aplication path
        public static string GetAppPath(object obj)
        {
            string sAppPath = string.Empty;
            try
            {
                sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "");
            }
            catch
            {
                try
                {
                    sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(obj.GetType()).Location);
                }
                catch
                {
                    sAppPath = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            return sAppPath;
        }
        #endregion

        public static DataSet GetOnHoldCount()//2915
        {
            try
            {
                System.Data.IDbConnection oConn = null;
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = " SELECT COUNT(*) FROM POSTRANSACTION_ONHOLD  ";

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(oConn, CommandType.Text, sSQL);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetOnHoldCount()");
                return null;
            }
        }

        public enum OnlinePayment //2915
        {
            [Description("NoOnlinePayment")]
            NoOnlinePayment = 0,
            [Description("AskForOption")]
            AskForOption = 1,
            [Description("OnlyOnlinePayment")]
            OnlyOnlinePayment = 2
        }

        #region PRIMEPOS-2485 17-Mar-2021 JY Added
        public static bool ValidateEmailAddress(string inputEmail)
        {

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        #endregion
        public static string GetConfigFilePath()//PRIMEPOS-2895 Vantiv Arvind
        {
            logger.Trace("GetConfigFilePath() - " + clsPOSDBConstants.Log_Entering);
            string strConfigFilePath = string.Empty;

            using (ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + "TriPosService" + "'"))
            {
                wmiService.Get();
                string currentserviceExePath = wmiService["PathName"].ToString();
                var cleanPath = string.Join("", currentserviceExePath.Split(Path.GetInvalidPathChars()));
                if (cleanPath.Length > 0 && cleanPath.EndsWith(@"\"))
                    cleanPath = cleanPath.Substring(0, cleanPath.Length - 1);

                strConfigFilePath = Path.GetDirectoryName(cleanPath);
            }
            logger.Trace("GetConfigFilePath() - " + clsPOSDBConstants.Log_Exiting);
            return strConfigFilePath;
        }

        #region PRIMEPOS-2993 24-Aug-2021 JY Added
        public static int GetSessionId()
        {
            int sessionId = 0;
            enumApplicationLaunchContext launchingContext = enumApplicationLaunchContext.Local;
            launchingContext = (enumApplicationLaunchContext)Enum.Parse(typeof(enumApplicationLaunchContext), convertNullToInt(CSetting.ApplicationLaunchContext).ToString());
            if (launchingContext == enumApplicationLaunchContext.RemoteDesktop || launchingContext == enumApplicationLaunchContext.Citrix)
            {
                sessionId = Process.GetCurrentProcess().SessionId;
            }
            return sessionId;
        }

        public static DataSet SaveApplicationVersionInfo(string applName, string machineName, string currentVersion, string path)
        {
            string strSQL = "SELECT TOP 1 StationId FROM AutoUpdateAppVer WHERE LTRIM(RTRIM(StationId)) <> '01' AND LTRIM(RTRIM(AppName)) = ltrim(rtrim('" + applName + "')) AND LTRIM(RTRIM(MachineName))=ltrim(rtrim('" + machineName + "'))";
            DataSet ds = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);
            if (!(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
            {
                strSQL = "DECLARE @StationId varchar(20);" +
                    " SET @StationId = (SELECT TOP 1  t1.StationId + 1 AS NewStationId FROM AutoUpdateAppVer t1 WHERE AppName = 'POS' AND isnumeric(t1.StationId) = 1 AND t1.stationid > 0" +
                    " AND NOT EXISTS(SELECT * FROM AutoUpdateAppVer t2 WHERE isnumeric(t2.StationId) = 1 and t2.StationId = t1.StationId + 1 AND t2.stationid > 0" +
                    " AND t2.StationId is null and t2.AppName = 'POS') ORDER BY t1.StationId DESC);" +
                    " SET @StationId = ISNULL(@StationId, 1)" +
                    " SET @StationId = (SELECT RIGHT('00' + CAST(@StationId AS VARCHAR(3)), 2));" +
                    " select @StationId;";
                ds = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strSQL = "INSERT INTO AutoUpdateAppVer(AppName, CurrentVersion, LastUpdatedAt, StationId,[Path], MachineName)" +
                        " VALUES('" + applName + "','" + currentVersion + "', getdate(), '" + convertNullToString(ds.Tables[0].Rows[0][0]) + "','" + path + "','" + machineName + "')";
                    DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
                }
            }

            strSQL = "SELECT TOP 1 LTRIM(RTRIM(StationId)) AS StationId FROM AutoUpdateAppVer WHERE LTRIM(RTRIM(AppName)) = ltrim(rtrim('" + applName + "')) AND LTRIM(RTRIM(MachineName))=ltrim(rtrim('" + machineName + "')) ORDER BY StationId DESc";
            ds = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, strSQL);
            return ds;
        }
        #endregion

        #region PRIMEPOS-2996 23-Sep-2021 JY Added
        public static void SetReportPrinter(ref string strReportPrinter)
        {
            bool bReportPrinter = false;
            try
            {
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (printer.Trim().ToUpper() == CPOSSet.ReportPrinter.Trim().ToUpper())
                    {
                        bReportPrinter = true;
                        break;
                    }
                }

                if (bReportPrinter)
                    strReportPrinter = CPOSSet.ReportPrinter.Trim().ToUpper();
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        #region PRIMEPOS-3024 08-Nov-2021 JY Added
        public static void AddIPLaneToConfig()
        {
            try
            {
                logger.Trace("AddIPLaneToConfig() - " + clsPOSDBConstants.Log_Entering);
                string strConfigFilePath = string.Empty;
                string strConfigFileName = "triPOS.config";
                Prefrences oPrefrences = new Prefrences();
                DataTable dt = oPrefrences.GetTriPOSSettings();
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Configuration.convertNullToString(dt.Rows[0]["TriPOSConfigFilePath"]).Trim() != "")
                        strConfigFilePath = Configuration.convertNullToString(dt.Rows[0]["TriPOSConfigFilePath"]).Trim();
                }

                if (strConfigFilePath == "")
                {
                    strConfigFilePath = Configuration.GetConfigFilePath();
                    if (strConfigFilePath != string.Empty)
                    {
                        strConfigFilePath = string.Concat(strConfigFilePath, @"\", strConfigFileName);
                        AddIPLaneToConfig(strConfigFilePath);
                    }
                }
                else
                {
                    AddIPLaneToConfig(strConfigFilePath);
                }
                logger.Trace("AddIPLaneToConfig() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                //clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private static void AddIPLaneToConfig(string strConfigFilePath)
        {
            logger.Trace("AddIPLaneToConfig(string strConfigFilePath) - " + clsPOSDBConstants.Log_Entering);

            XmlDocument xmlDocument = new XmlDocument();
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;
            using (XmlReader reader = XmlReader.Create(strConfigFilePath, readerSettings))
            {
                xmlDocument.Load(reader);
            }
            XmlNodeList xmlNodeList;

            try
            {
                Boolean isParentLaneFound = false;
                Boolean isLaneFound = false;
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    if (xmlNode.Attributes != null)
                    {
                        isParentLaneFound = true;
                        break;
                    }
                }

                if (isParentLaneFound)
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                        {
                            isLaneFound = true;
                            break;
                        }
                    }
                }
                else
                {
                    XmlElement elem = xmlDocument.CreateElement("lanes");
                    xmlDocument.GetElementsByTagName("tripos")[0].AppendChild(elem);
                    xmlDocument.Save(strConfigFilePath);
                }

                if (!isLaneFound)
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos");
                    foreach (XmlNode xmlnodes in xmlNodeList)
                    {
                        int id = Configuration.convertNullToInt(Configuration.StationID);
                        if (id != 0)
                            id += 1000;
                        else
                            id += 2000;
                        XmlAttribute attribute1 = xmlDocument.CreateAttribute("description");
                        attribute1.Value = "lane " + id.ToString();
                        XmlAttribute attribute2 = xmlDocument.CreateAttribute("laneId");
                        attribute2.Value = id.ToString();
                        XmlNode nodeIPlLane = xmlDocument.CreateElement("ipLane");
                        XmlNode nodePinpad = xmlDocument.CreateElement("pinpad");
                        XmlNode nodehost = xmlDocument.CreateElement("host");
                        XmlNode terminalType = xmlDocument.CreateElement("terminalType");
                        terminalType.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPTerminalType);
                        XmlNode nodestore = xmlDocument.CreateElement("store");
                        XmlNode transactionAmountLimit = xmlDocument.CreateElement("transactionAmountLimit");
                        transactionAmountLimit.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPTransactionAmountLimit);
                        nodestore.AppendChild(transactionAmountLimit);
                        XmlNode terminalID = xmlDocument.CreateElement("terminalId"); //PRIMEPOS-3513
                        terminalID.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPTerminalId);
                        nodehost.AppendChild(terminalID);

                        XmlNode driver = xmlDocument.CreateElement("driver");
                        driver.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPDriver);
                        XmlNode ipAddress = xmlDocument.CreateElement("ipAddress");
                        ipAddress.InnerText = "";
                        XmlNode ipPort = xmlDocument.CreateElement("ipPort");
                        ipPort.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPPort);
                        XmlNode ManualEntry = xmlDocument.CreateElement("isManualEntryAllowed");
                        ManualEntry.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPisManualEntryAllowed).ToLower();
                        XmlNode NearCommunication = xmlDocument.CreateElement("isContactlessEmvEntryAllowed");
                        NearCommunication.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPisContactlessEmvEntryAllowed).ToLower();
                        XmlNode NearCommunicationMSD = xmlDocument.CreateElement("isContactlessMsdEntryAllowed");
                        NearCommunicationMSD.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPisContactlessMsdEntryAllowed).ToLower();
                        #region PRIMEPOS-3266
                        XmlNode DisplayCustomScreen = xmlDocument.CreateElement("isDisplayCustomAidScreen");
                        DisplayCustomScreen.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPisDisplayCustomAidScreen).ToLower();
                        XmlNode TotalAmountScreenDisplayed = xmlDocument.CreateElement("isConfirmTotalAmountScreenDisplayed");
                        TotalAmountScreenDisplayed.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPisConfirmTotalAmountScreenDisplayed).ToLower();
                        XmlNode CreditSurchargeScreenDisplayed = xmlDocument.CreateElement("isConfirmCreditSurchargeScreenDisplayed");
                        CreditSurchargeScreenDisplayed.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPisConfirmCreditSurchargeScreenDisplayed).ToLower();
                        #endregion
                        XmlNode isUnattended = xmlDocument.CreateElement("isUnattended"); //PRIMEPOS-3513
                        isUnattended.InnerText = Configuration.convertNullToString(Configuration.CSetting.Unattended).ToLower(); //PRIMEPOS-3513
                        XmlNode idleScreen = xmlDocument.CreateElement("idleScreen");
                        XmlNode message = xmlDocument.CreateElement("message");
                        message.InnerText = Configuration.convertNullToString(Configuration.CSetting.IPMessage);
                        idleScreen.AppendChild(message);

                        xmlnodes["lanes"].AppendChild(nodeIPlLane);
                        nodeIPlLane.AppendChild(nodePinpad);
                        nodeIPlLane.AppendChild(nodehost);
                        nodePinpad.AppendChild(terminalType);
                        nodePinpad.AppendChild(driver);
                        nodePinpad.AppendChild(NearCommunication);
                        nodePinpad.AppendChild(NearCommunicationMSD);
                        nodePinpad.AppendChild(DisplayCustomScreen); //PRIMEPOS-3266
                        nodePinpad.AppendChild(TotalAmountScreenDisplayed); //PRIMEPOS-3266
                        nodePinpad.AppendChild(CreditSurchargeScreenDisplayed); //PRIMEPOS-3266
                        nodePinpad.AppendChild(ManualEntry);
                        nodePinpad.AppendChild(ipAddress);
                        nodePinpad.AppendChild(ipPort);
                        nodePinpad.AppendChild(idleScreen);
                        nodePinpad.AppendChild(isUnattended); //PRIMEPOS-3513
                        nodeIPlLane.AppendChild(nodestore);

                        nodeIPlLane.Attributes.Append(attribute1);
                        nodeIPlLane.Attributes.Append(attribute2);
                    }
                    xmlDocument.Save(strConfigFilePath);
                }
            }
            catch (Exception Ex)
            {
                //clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            logger.Trace("AddIPLaneToConfig(string strConfigFilePath) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        #region PRIMEPOS-3167 07-Nov-2022 JY Added
        public static void UpdatePrimeEDISetting()
        {
            try
            {
                string strSQL = "IF NOT EXISTS(SELECT ID FROM PrimeEDISetting)" +
                                " BEGIN" +
                                    " IF EXISTS(SELECT STATIONID FROM Util_POSSET WHERE USEPRIMEPO = 1)" +
                                    " BEGIN" +
                                        " INSERT INTO PrimeEDISetting" +
                                        " SELECT 1, USEPRIMEPO, HOSTADDRESS, CONNECTIONTIMER, PURCHASEORDERTIMER, PRICEUPDATETIMER, RemoteURL, ConsiderReturnTrans, UPDATEVENDORPRICE, UPDATEDESCRIPTION, INSERT11DIGITITEM, IGNOREVENDORSEQUENCE, DEFAULTVENDOR, USEDEFAULTVENDOR, AUTOPOSEQ FROM Util_POSSET WHERE USEPRIMEPO = 1" +
                                    " END" +
                                    " ELSE IF EXISTS(SELECT STATIONID FROM Util_POSSET WHERE STATIONID = '01')" +
                                    " BEGIN" +
                                        " INSERT INTO PrimeEDISetting" +
                                        " SELECT 1, USEPRIMEPO, HOSTADDRESS, CONNECTIONTIMER, PURCHASEORDERTIMER, PRICEUPDATETIMER, RemoteURL, ConsiderReturnTrans, UPDATEVENDORPRICE, UPDATEDESCRIPTION, INSERT11DIGITITEM, IGNOREVENDORSEQUENCE, DEFAULTVENDOR, USEDEFAULTVENDOR, AUTOPOSEQ FROM Util_POSSET WHERE STATIONID = '01'" +
                                    " END" +
                                " END";
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, strSQL);
            }
            catch (Exception Ex)
            {
                //clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        #endregion

        #region PRIMEPOS-3157 28-Nov-2022 JY Added
        public static Byte[] GetImage(string documentId)
        {
            byte[] img = null;
            try
            {
                DocumentContext oDocumentContext = new DocumentContext();
                img = oDocumentContext.GetImageDocument(documentId);
            }
            catch (Exception ex) 
            { 
                //MessageBox.Show(ex.InnerException.ToString()); 
            }
            return img;
        }
        #endregion
    }

    public struct CompanyInfo
    {
        public string StoreID;
        public string StoreName;
        public string Address;
        public string City;
        public string State;
        public string Zip;
        public string Telephone;
        public string ReceiptMSG;
        public int NoOfReceipt;
        public int NoOfOnHoldTransReceipt;//Added by shitaljit.
        public int NoOfGiftReceipt;//Added by shitaljit.
        public int NoOfCC;
        public int NoOfHCRC;
        public int NoOfRARC;
        public string MerchantNo;
        public string PriceItemQualifier;
        public string PriceQualifier;
        public string SigType;
        public string PrivacyText;
        public string OtcPrivacyNotice;
        public string PatientCounceling;
        public int PrivacyExpiry;
        public string RemoteDBServer;
        public string RemoteCatalog;
        public bool UseRemoteServer;
        public string AllowUnPickedRX; //Added by Prog1 15Mar2009//change from char to string by shitaljit.
        public int UnPickedRXSearchDays; //Added by Prog1 16Mar2009
        public bool CheckRXItemsForIIAS; //Added by Prog1 17Mar2009
        public int AllowVerifiedRXOnly; //Added by Prog1 24Apr2009  //PRIMEPOS-2593 23-Jun-2020 JY changed from bool to int
        public bool GroupTransItems; //Added by Prog1 06Jun2009
        public bool ForceCustomerInTrans; //Added by Prog1 16Jun2009
        public int DefaultDeptId;//Added By SRT(Ritesh Parekh) Date: 28-Aug-2009 Details: To hold Default Department ID.
        public bool PostDescOnlyInHC; //Added by Prog1 21Oct2009
        public bool WarnMultiPatientRX;//Added byt Prog1 23Dec2009
        //public bool ChargeCashBackInPercent;//Added by Prog1 24Mar2010 //Commented by Shitaljit.
        public string ChargeCashBackMode;//Added by Shitaljit 17 April 2013
        public bool AllowCashBack;//Added by Prog1 05Apr2010
        public bool PostRXNumberOnlyInHC;//Added by Prog1 23sep2010
        public string PrintReceiptForOnHoldTrans; //Added by shitaljit 9Nov2012
        public bool PrintStCloseNo; //Added By Amit Date 19 April 2011
        public bool PrintEODNo; //Added By Amit Date 19 April 2011
        public bool PWEncrypted;
        public bool ShowOnlyOneCCType;
        //Aded by shitaljit(QuickSolv) on 2 june 2011
        public int AllowMultipleInstanceOfPOS;  //PRIMEPOS-2936 21-Jan-2021 JY modified
        public int DefaultInvReturnID;
        public int DefalutInvRecievedID;
        public bool UpdateDeftInvRet;
        public bool UpdateDeftInvRecv;
        public bool UseCashManagement;
        public decimal DefCDStartBalance;//Added on 14 June 2011
        public bool AllowHundredPerInvDiscount;//Added on 7 Dec 2011
        public bool InvDiscToDiscountableItemOnly;//Added on 27 Dec 2011
        //Till here Added By Shitaljit.
        public int DaysForWarn;//Added by Krishna on 12 October 2011
        //Added By Amit Date 22 Nov 2011
        public bool TagFSA;
        public bool TagTaxable;
        public bool TagMonitored;
        public bool TagEBT;
        //End
        public bool AllowDiscountOfItemsOnSale;//Added By Shitaljit on 17 Feb 2012
        public int NoOfCheckRC;//Added By Shitaljit on 13 March 2012
        public bool EBTAsManualTrans;//Added By Shitaljit on 6 April 2012
        public int AutoImportCustAtTrans;//Added By Shitaljit on 23 April 2012  //PRIMEPOS-2886 25-Sep-2020 JY changed from bool to int
        public bool ShowTextPrediction;
        public bool AllowMultipleRXRefillsInSameTrans;
        public int NoOfCashReceipts;
        public char PrintReceipt;
        public bool AllowPrintZeroTrans;
        public bool DoNotOpenDrawerForCCOnlyTrans;
        public bool DoNotOpenDrawerForChequeTrans;

        public int PreventRxMaxFillDayLimit;
        public int PreventRxMaxFillDayNotifyAction;

        //public bool ConfirmPatient;//Added by Krishna on 20 August 2012   //PRIMEPOS-2317 15-Mar-2019 JY Commented
        public int ConfirmPatient; //PRIMEPOS-2317 15-Mar-2019 JY Added
        public bool UseSameThreadToVerifySysDateTime;//added by shitaljit on 12 sept 2012
        public bool EnforceComplexPassword;//added by shitaljit on 8 Oct 2012
        public bool PromptForSellingPriceLessThanCost;//Add by Ravindra 21 March 2013
        public bool AllowItemComboPrice;
        public string DefaultTaxableItem;//added by shitaljit on 16 May 2013
        public string DefaultNonTaxableItem;//added by shitaljit on 16 May 2013
        public bool OpenDrawerForZeroAmtTrans;//Added By Shitaljit on 9 July 2013
        public string UpdatePatientData;//Added By shitaljit on 19 July 2013. PRIMEPOS-1235 Add Preference to control Updating patient data from PrimeRX during transaction.
        public bool AutoPopulateCLCardDetails;//added By shitaljit on 25July2013 for PrimePOS-436 Loyalty cards could be displayed for Customer during payment
        public bool DoNotOpenDrawerForHouseChargeOnlyTrans; //Sprint-19 - 2161 27-Mar-2015 JY Added 

        #region Sprint-20 26-May-2015 Auto updater JY Added settings for auto updater
        public bool AllowAutomaticUpdates;
        public bool AllowRunningUpdates;
        public string AutoUpdateServiceAddress;
        public int RunningTasksTimerInterval;
        #endregion

        #region Sprint-23 - PRIMEPOS-2029 11-Apr-2016 JY Added
        public bool useNplex;
        public string nplexStoreId;
        public string StoreSiteId;
        public bool postSaleInd;
        #endregion

        #region PRIMEPOS-2643 05-Sep-2019
        public bool PIEnable;
        public string PIURL;
        public string PUser;
        public string PPassword;

        #endregion

        #region Sprint-23 - PRIMEPOS-2244 19-May-2016 JY Added 
        public bool PrintStationCloseDateTime;
        public bool PrintEODDateTime;
        #endregion
        public bool SearchRxsWithPatientName;   //Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
        public bool FetchFamilyRx;  //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added
        public bool SaveCCToken;   //Sprint-23 - PRIMEPOS-2313 09-Jun-2016 JY Added

        public bool AllowZeroSellingPrice; //Sprint-21 - 2204 26-Jun-2015 JY Added
        public bool RestrictInActiveItem; //Sprint-21 - 2173 10-Jul-2015 JY Added
        public bool PrintReceiptInMultipleLanguage; //Sprint-21 - 1272 25-Aug-2015 JY Added
        public bool ConsiderItemType; //Sprint-22 16-Dec-2015 JY Added settings to control the ItemType behavior 

        /// <summary>
        /// By Pass the Payment screen if the Amount is Zero
        /// </summary>
        public bool ByPassPayScreen; //added by Manoj 9/26/2013
        /// <summary>
        /// Use for Merging of Signatures
        /// </summary>
        public bool MergeSign; //Added by Manoj 10/01/2013
        /// <summary>
        /// Host Address for HPS sftp to get FSA files
        /// </summary>
        public string HpsSftpHost;
        /// <summary>
        /// Host Port for HPS sftp
        /// </summary>
        public int HpsSftpPort;
        /// <summary>
        /// User Name for HPS Sftp
        /// </summary>
        public string HpsSftpUser;
        /// <summary>
        /// Password for HPS Sftp
        /// </summary>
        public string HpsSftpPassword;

        public bool UseEmailInvoice;
        public string OutGoingEmailServer;
        public string OutGoingEmailID;
        public string OwnersEmailId;    //Sprint-24 - PRIMEPOS-2363 28-Dec-2016 JY Added
        public bool AutoEmailStationCloseReport;    //Sprint-24 - PRIMEPOS-2363 27-Jan-2017 JY Added
        public bool AutoEmailEODReport;    //Sprint-24 - PRIMEPOS-2363 27-Jan-2017 JY Added
        public string OutGoingEmailUserID;
        public string OutGoingEmailPass;
        public int OutGoingEmailPort;

        public bool OutGoingEmailEnableSSL;
        public string OutGoingEmailSubject;
        public string OutGoingEmailBody;
        public string OutGoingEmailSignature;
        //Added By shitaljit for JIRA- PRIMEPOS-1652 Add preference to manage Promotional coupon discount to abide with discount settings
        public bool ApplyInvDiscSettingsForCoupon;
        //END

        //Added By Ravindra PRIMEPOS-1657   Email prompt in preferences   
        public bool OutGoingEmailPromptAutomatically;
        //END

        /*Date : 29/01/2014
		 * Modified by - Shitaljit
		 * Change Currency Symbol from Char to String
		 */
        //new Code
        public string CurrencySymbol;
        //public char CurrencySymbol;
        public bool WarnForRXDelivery; //Added By Shitaljit on 6/2/2104 for PRIMEPOS-1816 Ability to turn on\off delivery prompt
        public bool AutoPopulateCustEmail; //Added By Shitaljit on 6/2/2104 for PRIMEPOS-1804 Auto Populate Email address from customer
        //Added By Shitaljit for PRIMEPOS-1879 IVU Lotto Program for Puerto Rico
        public string IVULottoMerchantID;
        public string IVULottoCommunicationMode;
        public bool UseIVULottoProgram;
        //END
        public bool PromptForZeroCostPrice;
        public bool PromptForZeroSellingPrice;

        #region Sprint-24 - PRIMEPOS-2344 02-Dec-2016 JY Added
        public string IIASFTPAddress;
        public string IIASFTPUserId;
        public string IIASFTPPassword;
        public string IIASFileName;
        public int IIASDownloadInterval;
        public DateTime IIASFileModifiedDateOnFTP;
        #endregion

        #region Sprint-25 - PRIMEPOS-2379 07-Feb-2017 JY Added
        public string PSEFTPAddress;
        public string PSEFTPUserId;
        public string PSEFTPPassword;
        public string PSEFileName;
        public int PSEDownloadInterval;
        public DateTime PSEFileModifiedDateOnFTP;
        public bool UpdatePSEItem;
        #endregion
        public bool DefaultCustomerTokenValue;  //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added

        public bool EnableConsentCapture;
        public string SelectedConsentSource;
        public Dictionary<int, string> ConsentSourceActiveList; //PRIMEPOS-CONSENT SAJID DHUKKA PRIMEPOS-2866
        #region PrimePOS-2448 Added BY Rohit Nair
        public bool EnableIntakeBatch;
        public bool SkipSignatureForInatkeBatch;
        public bool IntakeBatchMarkAsPickedup;
        public string IntakeBatchCode;
        public string IntakeBatchStatus;//PrimePOS-2518 Jenny Added
        #endregion
        public bool PromptForPartialPayment;  //PRIMEPOS-2499 27-Mar-2018 JY Added

        #region PRIMEPOS-2562 27-Jul-2018 JY Added
        public bool EnforceLowerCaseChar;
        public bool EnforceUpperCaseChar;
        public bool EnforceSpecialChar;
        public bool EnforceNumber;
        public int PasswordExpirationDays;
        public int PasswordLength;
        public int PasswordHistoryCount;
        #endregion

        public string UseBiometricDevice;   //PRIMEPOS-2576 23-Aug-2018 JY Added
        public bool IgnoreFutureRx;  //PRIMEPOS-2591 25-Oct-2018 JY Added
        public bool ShowPaytypeDetails; //PRIMEPOS-2384 29-Oct-2018 JY Added 
        public bool RestrictFSACardMessage;   //PRIMEPOS-2621 17-Dec-2018 JY Added
        public int AuthenticationMode;  //PRIMEPOS-2616 19-Dec-2018 JY Added

        #region PRIMEPOS-2613 28-Dec-2018 JY Added
        public bool CardExpAlert;
        public bool CardExpEmail;
        public int CardExpAlertDays;
        public int SPEmailFormatId;
        #endregion

        public bool PromptForItemPriceUpdate;   //PRIMEPOS-2602 28-Jan-2019 JY Added
        public bool UsePrimeESC;    //PRIMEPOS-2385 14-Mar-2019 JY Added
        public bool ShowPatientsData;   //PRIMEPOS-2317 21-Mar-2019 JY Added
        public bool RestrictIfDOBMismatch;  //PRIMEPOS-2317 21-Mar-2019 JY Added
        public string PHNPINO;  //PRIMEPOS-2667 12-Apr-2019 JY Added
        public string PSServiceAddress; //PRIMEPOS-2671 18-Apr-2019 JY Added
        #region Added for Solutran - PRIMEPOS-2663 - NileshJ
        public bool S3Enable;
        public string S3Url;
        public string S3Key;
        public string S3Merchant;
        #endregion
        public bool HidePatientCounseling;

        #region Added for BatchDelivery - PRIMERX-7688 - NileshJ
        public bool isPrimeDeliveryReconciliation;
        #endregion

        public string OldEDIComatibility;//PRIMEPOS-2679

        public bool EnableEPrimeRx;
        public string EPrimeRxURL;
        public string EPrimeRxToken;
        public int SSOIdentifier;  //PRIMEPOS-3484
    }

    public struct POSSET
    {
        public bool UseScanner;
        public bool UsePoleDisplay;
        public string PD_PORT;
        public string PDP_BAUD;
        public string PDP_PARITY;
        public string PDP_DBITS;
        public string PDP_STOPB;
        public string PDP_CODE;
        public string PDP_CLSCD;
        public string PDP_CUROFF;
        public string PD_MSG;
        public int PD_LINES;
        public int PD_LINELEN;
        public string PD_INTRFCE;
        public bool USECASHDRW;
        public string CD_TYPE;
        public string CD_PORT;
        public string CDP_BAUD;
        public string CDP_PARITY;
        public string CDP_DBITS;
        public string CDP_STOPB;
        public string CDP_CODE;
        public string CDP_BAUD2;
        public string CDP_PARITY2;
        public string CDP_DBITS2;
        public string CDP_STOPB2;
        public string CDP_CODE2;
        public string RP_TYPE;
        public string RP_Name;
        public string LabelPrinter;

        #region PRIMEPOS-2996 22-Sep-2021 JY Added
        public string ReportPrinter;
        public string ReceiptPrinterPaperSource;
        public string LabelPrinterPaperSource;
        public string ReportPrinterPaperSource;
        #endregion

        public bool ONLINECCT;
        public string RXCode;
        public int RP_CCPrint;
        //From here Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set

        public int NoOfReceipt;
        public int NoOfOnHoldTransReceipt;
        //public int NoOfCC;    //PRIMEPOS-2308 16-May-2018 JY Commented
        //public int  NoOfCheckRC;  //PRIMEPOS-2308 16-May-2018 JY Commented
        //public int NoOfHCRC;  //PRIMEPOS-2308 16-May-2018 JY Commented
        public int NoOfRARC;
        public int NoOfGiftReceipt;
        //public int NoOfCashReceipts;//NoOfCheckRC //PRIMEPOS-2308 16-May-2018 JY Commented
        //Till here Add by ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set

        // public bool AllItemDisc;//Commented By Shitaljit(QuicSolv) 7 Sept 2011
        public string AllItemDisc;//Added By Shitaljit(QuicSolv) 7 Sept 2011
        public bool SingleUserLogin;
        public bool MergeCCWithRcpt;
        public bool PrintRXDescription;
        public bool UseRcptForCloseStation;
        public bool UseRcptForEOD;
        public bool UsePrimeRX;
        public bool LoginBeforeTrans;
        public int Inactive_Interval;
        public string StationName;
        public bool RoundTaxValue;
        public string RXInsToIgnoreCopay;
        public bool RestrictConcurrentLogin;
        public bool UseSigPad;
        public int AllowRxPicked;    //PRIMEPOS-2865 15-Jul-2020 JY Added
        public string SigPadHostAddr;
        public bool DispSigOnTrans;
        public bool DisableNOPP;
        public bool ApplyPriceValidation;
        public bool AllowManualCCTrans;

        #region PRIMEPOS-3455
        public bool IsSecureDevice;
        public string SecureDeviceModel;
        public string SecureDeviceSrNumber;
        #endregion

        //Added by Dharmendra(SRT) on Nov-13-08 to add those settings related with Card Payments & Pin Pad
        public string PaymentProcessor;
        public string HeartBeatTime;
        public string AvsMode;
        public string TxnTimeOut;
        public bool UsePinPad;
        public string PinPadModel;
        public string OrigPinPadModel;
        public string PinPadBaudRate;
        public string PinPadPairity;
        public string PinPadPortNo;
        public string PinPadDataBits;
        public string PinPadDispMesg;
        public string PinPadKeyEncryptionType;
        public bool ProcessOnLine; //Added by Dharmendra(SRT) on Nov-13-08 to make ProcessOnLine configurable as suggested by MMS
        //Add Ended
        public bool ApplyGroupPriceOverCompanionItem; //Naim 2Feb09
                                                      //Added By SRT(Abhishek) On Date: 02/05/2009



        //End OF Added
        //Added By SRT(Prashant) On Date: 15/04/2009        
        public bool ShowAuthorization; // Added by Dharmendra on May-13-09

        //Added By SRT(Prashant) On Date: 7/04/2009
        //Updated By SRT(Gaurav) Date: 07/07/2009        
        //public bool UsePreferredVendor;
        //public string PreferredVendor;
        //End Of Updated By SRT(Gaurav)
        //End of added

        //Added By SRT(Gaurav) On Date: 18/06/2009        
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Abhishek) On Date: 6/26/2009        
        //End of added
        public bool ShowCustomerNotes; //Added by Prog1 30May2009

        //Added By SRT(Gaurav) Date:  21-Jul-2009
        //Hold the data about whether to process fsa transaction with XCHARGE or not.
        public bool procFSAWithXLINK;
        //End Of Added By

        public int FetchUnbilledRx;    //Added by SRT(Abhishek) Date: 17-Aug-2009   //PRIMEPOS-2398 04-Jan-2021 JY changed from bool to int

        //Added By SRT(Ritesh Parekh) Date : 20-Aug-2009
        //Holds value to capture signature for debit card or not.
        public bool CaptureSigForDebit;
        //End Of Added By SRT(Ritesh Parekh)

        //Capture Signature for EBT
        public bool CaptureSigForEBT; //Add by Manoj 7/2/2013

        /// <summary>
        /// Skip Signature for F10 Transaction. User must also have security Rights
        /// </summary>
        public bool SkipF10Sign; //Add by Manoj 9/25/2013
        public bool SkipEMVCardSign;

        /// <summary>
        /// Skip Signature for Transaction less than or equal $20
        /// </summary>
        public bool SkipAmountSign; //Add by Manoj 9/25/2013

        //Capture Signature for House Charges.
        public bool DispSigOnHouseCharge; //Add by Manoj 11/21/2011

        public string ReceiptPrinterType; //12Oct2009 Prog1 added for laser printer
        public int MaxCATransAmt; //20Jan2010 Prog1 added for max house charge transaction        
        public bool AllowZeroAmtTransaction; //Added By SRT(Abhishek D) Date: 12 March 2010        
        //ADDED PRASHANT 5 JUN 2010
        public bool PromptForAllTrans;
        public bool PromptForReturnTrans;
        //END ADDED PRASHANT 5 JUN 2010
        public int DaysToResetPwd;

        /// <summary>
        /// Username For Payment Processor
        /// Also USed For AccountID in WorldPay
        /// </summary>
        public string HPS_USERNAME;

        /// <summary>
        /// Password For payment Processor
        /// Also Used For MerchantPin in WorldPay
        /// </summary>
        public string HPS_PASSWORD;
        public string TerminalID;
        /// <summary>
        /// Used to Store SubID in WorldPay
        /// </summary>
        public string WP_SubID;

        public bool ALLOWDUP;
        public bool PreferReverse;

        public bool AllRxPicked;
        public bool FetchFiledRx;
        public bool AllowPickedUpRxToTrans; //Added by Manoj 1/23/2013
        public int ControlByID; //Added by Manoj 4/2/2013 //PRIMEPOS-2547 03-Jul-2018 JY changed datatype
        public int AskVerificationIdMode;   //PRIMEPOS-2547 11-Jul-2018 JY Added
        public bool SkipDelSign; //Added by Manoj 5/8/2013
        public decimal MaxCashLimitForStnCose;//Added By Shitaljit on 1/7/2013
        public bool TurnOnEventLog;//Added By Shitaljit on 12/11/2013 to turn OFF/ON log 
        //Added By Shitaljit for PRIMEPOS-1879 IVU Lotto Program for Puerto Rico
        public string IVULottoTerminalID;
        public string IVULottoPassword;
        public string IVULottoServerURL;
        //END

        public bool SelectMultipleTaxes;    //Sprint-19 - 2146 26-Dec-2014 JY Added to select multiple taxes functionality        
        public bool IsTouchScreen;
        public bool ShowRxNotes;    //PRIMEPOS-2459 03-Apr-2019 JY Added    
        public bool ShowPatientNotes;    //PRIMEPOS-2459 03-Apr-2019 JY Added
        public bool ShowItemNotes;  //PRIMEPOS-2536 14-May-2019 JY Added
        #region allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019
        public bool AllowManualFirstMiles;
        public bool SkipRxSignature;
        #endregion
        #region StoreCredit - PRIMEPOS-2747 - NileshJ
        public bool EnableStoreCredit;
        #endregion

        #region PRIMEPOS-3167 07-Nov-2022 JY Commented
        //public bool USePrimePO; //Added by Dharmendra on Apr-29-09
        //public string HostAddress;
        //public int ConnectionTimer;
        //public int PurchaseOrdTimer;
        //public int PriceUpdateTimer;
        //public string RemoteURL;
        //public bool ConsiderReturnTrans;    //Sprint-27 - PRIMEPOS-2390 27-Sep-2017 JY Added
        //public bool UpdateVendorPrice;
        //public bool UpdateDescription;
        //public bool Insert11DigitItem; //Added By SRT(Sachin) Date: 23 Feb 2010
        //public bool IgnoreVendorSequence;
        //public string DefaultVendor;
        //public bool UseDefaultVendor;
        //public string AutoPOSeq;
        #endregion
    }

    #region PRIMEPOS-3167 07-Nov-2022 JY Added
    public struct PrimeEDISetting
    {
        public bool UsePrimePO;
        public string HostAddress;
        public int ConnectionTimer;
        public int PurchaseOrderTimer;
        public int PriceUpdateTimer;
        public string RemoteURL;
        public bool ConsiderReturnTrans;
        public bool UpdateVendorPrice;
        public bool UpdateDescription;
        public bool Insert11DigitItem;
        public bool IgnoreVendorSequence;
        public string DefaultVendor;
        public bool UseDefaultVendor;
        public string AutoPOSeq;
    }
    #endregion

    public struct CustomerLoyaltyInfo
    {
        public bool UseCustomerLoyalty;
        public string ProgramName;
        public long CardRangeFrom;
        public long CardRangeTo;
        public int DefaultCardExpiryDays;
        public bool IsCardPrepetual;
        public bool PrintCLCouponSeparately; //Sprint-18 - 2039 03-Nov-2014 JY Changed PrintCopounWithReceipt to PrintCLCouponSeparately
        public bool PrintCLCouponOnlyIfTierIsReached; //Sprint-18 - 2039 01-Dec-2014 JY Added to print CL coupon only if tier is reached
        public ExcludeData ExcludeDepts;
        public ExcludeData ExcludeItems;
        public ExcludeData ExcludeSubDepts;

        public ExcludeData ExcludeClCouponDepts;
        public ExcludeData ExcludeClCouponItems;
        public ExcludeData ExcludeClCouponSubDepts;

        public bool ExcludeItemsOnSale;
        public bool PrintMsgOnReceipt;
        public string Message;
        public decimal RedeemValue;
        public int RedeemMethod;
        public bool ShowCLCardInputOnTrans;
        public bool PrintCoupon;
        public bool ShowDiscountAppliedMsg;
        public bool AllowMultipleCouponsInTrans;
        public bool IsTierValueInPercent;
        public bool IncludeRXItems;
        public bool IncludeOTCItems;
        public string PointsCalcMethod;
        public string IncludeItemsWithType;
        public bool ShowCLControlPane;
        public bool DisableAutoPointCalc;
        public bool DoNotGenerateCoupons;
        public bool ExcludeDiscountableItems;
        public bool ApplyCLOrCusDisc;
        //Added By Shitaljit on 4Feb2014 for PRIMEPOS-1703 CL Tier incremental Scheme
        public bool SingleCouponPerRewardTier;
        public bool ApplyDiscountOnlyIfTierIsReached;   //Sprint-25 - PRIMEPOS-2297 21-Feb-2017 JY Added
        //End
    }

    #region PRIMEPOS-2739 Added by Arvind
    public struct SettingDetail//23/09/19
    {
        public bool StrictReturn;

        #region PRIMEPOS-2774 10-Jan-2020 JY Added
        public string S3FTPURL;
        public int S3FTPPort;
        public bool S3FTP;
        public string S3FTPUserId;
        public string S3FTPPassword;
        public int S3Frequency;
        public DateTime S3LastUploadDateOnFTP;
        public string S3FileName;
        public string S3FTPFolderPath;
        public DateTime S3UploadProcessDate;
        #endregion

        #region PRIMEPOS-3243
        public DateTime LastUpdateInsSigTrans;
        public int FrequencyIntervalInsSigTrans;
        public DateTime LastUpdatePOSTransactionRxDetail;
        public int FrequencyIntervalPOSTransactionRxDetail;
        public DateTime LastUpdateReturnTransDetailId;
        public int FrequencyIntervalReturnTransDetailId;
        public DateTime LastUpdateTrimItemID;
        public int FrequencyIntervalTrimItemID;
        //public DateTime LastUpdateSign;
        //public int FrequencyIntervalSign;
        public DateTime LastUpdateMissingTransDetInPrimeRx;
        public int FrequencyIntervalMissingTransDetInPrimeRx;
        public DateTime LastUpdateBlankSignWithSamePatientsSign;
        public int FrequencyIntervalBlankSignWithSamePatientsSign;
        #endregion

        public bool PromptForEvertecManual;//PRIMEPOS-2805
        public bool TagSolutran;    //PRIMEPOS-2836 21-Apr-2020 JY Added

        #region PRIMEPOS-2842 05-May-2020 JY Added
        public string TPWelcomeMessage;
        public string TPMessage;
        public string EnablePromptForZipCode;
        public string RequiredDriverForVerifonePads;
        public string Driver;
        public string ComPort;
        public int DataBits;
        public string Parity;
        public string StopBits;
        public string Handshake;
        public int BaudRate;

        public bool QuickChip;//PRIMEPOS-3500
        public bool CheckForPreReadId;//PRIMEPOS-3500
        public string QuickChipDataLifetime; //PRIMEPOS-3500
        public string ThresholdAmount;//PRIMEPOS-3500
        public bool ContactlessMsdEntryAllowed;//PRIMEPOS-3500
        public bool DisplayCustomAidScreen;//PRIMEPOS-3500
        public bool Unattended;//PRIMEPOS-3500
        public bool ReturnResponseBeforeCardRemoval;//PRIMEPOS-3535


        public bool TestMode;
        public bool AllowPartialApprovals;
        public bool ConfirmOriginalAmount;
        public bool CheckForDuplicateCreditCardTransactions;
        public bool VantivsCashBackFeature;
        public bool PromptForCreditCardCVVNumberForKeyedCardTransactions;
        public bool EnableDebitSale;
        public bool EnableDebitRefunds;
        public bool EnableEBTRefunds;
        public bool EnableGiftCards;
        public bool EnableEBTFoodStamp;
        public bool EnableEBTCashBenefit;
        public bool EnableEMVProcessing;
        public bool FSAHRACardProcessing;
        public bool TippingDoesNotApplyToPharmacyBusiness;
        public bool ManualCreditCardEntry;
        public bool NearFieldCommunication;
        #endregion
        #region PRIMEPOS-3024 08-Nov-2021 JY Added
        public string IPTerminalId;
        public string IPTerminalType;
        public string IPDriver;
        public bool IPisContactlessEmvEntryAllowed;
        public bool IPisContactlessMsdEntryAllowed;
        public bool IPisManualEntryAllowed;
        public bool IPisDisplayCustomAidScreen;//PRIMEPOS-3266
        public bool IPisConfirmTotalAmountScreenDisplayed;//PRIMEPOS-3266
        public bool IPisConfirmCreditSurchargeScreenDisplayed;//PRIMEPOS-3266
        public string IPPort;
        public string IPMessage;
        public string IPTransactionAmountLimit;
        #endregion
        public bool AutoSearchPrimeRxPatient;    //PRIMEPOS-2845 14-May-2020 JY Added
        #region PRIMEPOS-2841
        public bool OnlinePayment;//PRIMEPOS-2841 added by Arvind
        /// <summary>
        /// Added By Sajid/Nilesh
        /// </summary>
        public string PrimeRxPayClientId;
        /// <summary>
        /// Added By Sajid/Nilesh
        /// </summary>
        public string PrimeRxPaySecretKey;
        /// <summary>
        /// Added By Sajid/Nilesh
        /// </summary>
        public string PrimeRxPayUrl;
        #endregion
        #region PRIMEPOS-2794
        public bool EnableCustomerEngagement;
        #endregion
        #region PRIMEPOS-2875 added for POinty 
        public string MMSKey;
        public string RetailerKey;
        public string URL;
        public string SupportMail;
        public string MMSEmailID;
        public string MMSEmailPass;
        public bool EnableSSL;
        public int MMSEmailPort;
        public string MMSEmailServer;
        public string MMSEmailSig;
        public string PointyUTM;//PRIMEPOS-3005
        #endregion

        public bool ProceedROATransWithHCaccNotLinked;  //PRIMEPOS-2570 17-Aug-2020 JY Added

        #region PRIMEPOS-2902
        public int PayProviderID;
        public string PayProviderName;
        public string PharmacyNPI;
        #endregion

        public string DefaultPaytype;  //PRIMEPOS-2512 02-Oct-2020 JY Added
        public bool RestrictSignatureLineAndWordingOnReceipt;    //PRIMEPOS-2910 29-Oct-2020 JY Added

        public bool AllowMailOrder;//2927
        public bool AllowZeroShippingCharge;//2927

        public string VantivDelayInSecond;  //Added by Arvind
        public string RxInsuranceToBeTaxed; //PRIMEPOS-2924 02-Dec-2020 JY Added
        public string PatientCounselingPrompt;   //PRIMEPOS-2461 01-Mar-2021 JY Added
        public bool PrintCompanyLogo;    //PRIMEPOS-2386 26-Feb-2021 JY Added
        public bool RunVantivSignatureFix;    //PRIMEPOS-3232N
        public string CompanyLogoFileName;    //PRIMEPOS-2386 26-Feb-2021 JY Added
        public string LinkExpriyInMinutes;//2915
        public string OnlineOption;//2915
        public string SchedularMachine; //PRIMEPOS-2485 11-Mar-2021 JY Added
        public string SchedulerUser;   //PRIMEPOS-2485 05-Apr-2021 JY Added

        public string TerminalID;//2943

        public string LocationName;//2943

        public string ChainCode;//2943

        public string PrimerxPayExtensionUrl;//2956
        public bool PrimeRxPayBGStatusUpdate;//PRIMEPOS-3187
        public bool PrimeRxPayDefaultSelection;//PRIMEPOS-3250
        public int PrimeRxPayStatusUpdateIntervalInMin;//PRIMEPOS-3187
        public int PrimeRxPayStatusUpdateFromLastDays;//PRIMEPOS-3187
        public string PseudoephedDisclaimer;//PRIMEPOS-3109
        public string AzureADMiddleTierUrl; //PRIMEPOS-2989 13-Aug-2021 JY Added
        public string ApplicationLaunchContext; //PRIMEPOS-2993 24-Aug-2021 JY Added
        #region PRIMEPOS-2999 09-Sep-2021 JY Added
        public string NPlexURL;
        public string NPlexTokenURL;
        public string NPlexClientID;
        public string NPlexClientSecret;
        #endregion
        //PRIMEPOS-2990
        public string SiteId;
        public string LicenseId;
        public string DeviceId;
        public string Username;
        public string Password;
        public string DeveloperId;
        public string VersionNumber;
        public string RxTaxPolicy;  //PRIMEPOS-3053 04-Feb-2022 JY Added
        public bool NotifyRefrigeratedMedication;   //PRIMEPOS-2651 08-Apr-2022 JY Added
        public bool RestrictMultipleClockIn;   //PRIMEPOS-2790 18-Apr-2022 JY Added
        public string TransactionFeeApplicableFor;    //PRIMEPOS-3115 11-Jul-2022 JY Added
        public bool ResetPwdForceUserToChangePwd;   //PRIMEPOS-3129 22-Aug-2022 JY Added
        public string PromptToSaveCCToken;  //PRIMEPOS-3145 28-Sep-2022 JY Added

        #region PRIMEPOS-3164 01-Nov-2022 JY Added
        public string TranslatorAPIkey;
        public string TranslatorAPIEndPoint;
        public string TranslatorAPILocation;
        public string TranslatorAPIRoute;
        #endregion

        public string PatientsSubCategories;    //PRIMEPOS-3157 28-Nov-2022 JY Added
        public bool MaskDrugName;    //PRIMEPOS-3130 
        #region PRIMEPOS-3370
        public bool NBSEnable;
        public string NBSUrl;
        public string NBSToken;
        public string NBSEntityID;
        public string NBSStoreID;
        public string NBSBins; //PRIM0EPOS-3529 //PRIMEPOS-3504
        //public string NBSTerminalID;
        public DateTime NBSTokenExpriresAt; //PRIMEPOS-3412
        #endregion
        public bool WaiveTransactionFee;  //PRIMEPOS-3234
    }
    #endregion

    public class HyphenSetting //PRIMEPOS-3207
    {
        //public bool HyphenIsActive { get; set; }

        public string EnableHyphenIntegration { get; set; } = "N";

        //public string ClientID { get; set; }

        //public string ClientSecret { get; set; }

        public string _insuranceSet = string.Empty;
        public string InsuranceSet
        {
            get
            {
                return _insuranceSet;
            }
            set
            {
                _insuranceSet = value;
                HyphenInsuranceCollection = JsonConvert.DeserializeObject<HyphenInsuranceSet[]>(_insuranceSet);
            }
        }

        public string RejectionCodeSet { get; set; }

        //public string VendorBaseUrl { get; set; }

        //public string AuthEndpoint { get; set; }

        //public string AlertEndpoint { get; set; }

        //public string UIEndpoint { get; set; }

        public string AzureFunctionUrl { get; set; }

        public string AzureFunctionKeyCode { get; set; }
        public string ApiVersion { get; set; }  //PRIMEPOS-3394

        //public List<string> hyphenAlertDone { get; set; }
        public HyphenInsuranceSet[] HyphenInsuranceCollection { get; set; }
    }



    public class HyphenInsuranceSet
    {
        [JsonProperty("bin")]
        public string Bin { get; set; }

        [JsonProperty("pcn")]
        public string PCN { get; set; }


        [JsonProperty("groupid")]
        public string GroupId { get; set; }
    }


    #region PRIMEPOS-2227 04-May-2017 JY Added code to get merchant info settings
    public struct MerchantConfig
    {
        public int Config_ID;
        public string User_ID;
        public string Merchant;
        public string Processor_ID;
        public string Payment_Server;
        public string Port_No;
        public string Payment_Client;
        public string Payment_ResultFile;
        public string Application_Name;
        public string XCClientUITitle;
        public string LicenseID;
        public string SiteID;
        public string DeviceID;
        public string URL;
        public string VCBin;
        public string MCBin;
        public string VantivAccountUrl;//PRIMEPOS-TOKENURL
        public string VantivTokenUrl;
        public string VantivReportUrl; //PRIMEPOS-3156;
    }
    #endregion

    #region PRIMEPOS-3372
    public struct NBSBinRange
    {
        public string binValue;
        public bool IsDeleted;
    }
    #endregion

    public class ExcludeData
    {
        public ExcludeData()
        {
            Data = new List<string>();
            IsDataChanged = false;
        }

        public List<String> Data { get; set; }

        public bool IsDataChanged { get; set; }
    }

    /// <summary>
    /// Author:Shitaljit
    /// To store Label Printing set up details.
    /// </summary>
    public struct LabelPrintingSetup
    {
        public bool PrintItemID;
        public bool PrintItemIDBarcode;
        public bool PrintItemVendorID;
        public bool PrintItemPrice;
        public bool PrintDescription;
        public bool PrintItemVendorIDBarcode;
        public string LabelTemplate;

        public bool ProductCode;
        public bool AvgPrice;
        public bool ManufacturerName;
        public bool OnSalePrice;
        public bool PrintVendorName;    //PRIMEPOS-2758 12-Nov-2019 JY Added 
    }

    public enum CLRedeemMethod
    {
        Auto,
        Manual
    }

    public struct FormLocation
    {
        public int Left;
        public int Top;
        public int Height;
        public int Width;
    }

    #region PRIMEPOS-2576 23-Aug-2018 JY Added
    public enum fpReaderAction
    {
        SendBitmap,
        SendFMD,
        SendFingerprint,
        SendMessage
    }
    #endregion

    #region PRIMEPOS-2613 24-Dec-2018 JY Added
    // Type of Messaging Supported by PrimeEx
    public enum eMessaging
    {
        None = -1,
        Email = 0,
        SMS = 1,
        PhoneCall = 4
    }

    // Alert Types - Message category   
    public enum eMessageCategory
    {
        [Description("None")]
        None = -1,
        //[Description("General Message Log")]
        //General,
        [Description("Stored Profiles Log")]
        StoredProfiles = 1
    }
    #endregion

    #region PRIMEPOS-2671 22-Apr-2019 JY Added
    public class ItemM
    {
        public string ItemID;
        public string Description;
        public string ProductCode;
        public string SeasonCode;
        public string Unit;
        public decimal Freight;
        public decimal SellingPrice;
        public string Itemtype;
        public bool isTaxable;
        public string PCKSIZE;
        public string PCKQTY;
        public string PCKUNIT;
        public string ItemStatus;
        public string ManufacturerName;
        public bool IsEBTItem;
    }
    #endregion

    #region PRIMEPOS-2993 24-Aug-2021 JY Added
    public enum enumApplicationLaunchContext
    {
        Local = 0,
        RemoteDesktop = 1,
        Citrix = 2
    }
    #endregion

    #region PRIMEPOS-3157 28-Nov-2022 JY Added
    public enum Category
    {
        PRESCRIBTION = 1,
        PATIENT = 2,
        INSURANCE = 4,
        DRUG = 5,
        PRESCRIBER = 3,
        Compound_Batch = 9
    }
    #endregion

    #region PRIMEPOS-3484
    public enum SSOIdentifier
    {
        Email = 0,
        LanID = 1
    }
    #endregion
}