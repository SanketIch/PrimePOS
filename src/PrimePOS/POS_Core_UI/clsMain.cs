using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS_Core.DataAccess;
using POS_Core.UserManagement;
using Microsoft.Win32;
using System.Globalization;
using Resources;
using NLog; //PRIMEPOS-2641 14-Feb-2019 JY Added

#region Sprint-22 - 2240 29-Sep-2015 JY Added
using System.Collections.Generic;
using System.Xml;
using POS_Core.DataAccess;
using System.Data;
using POS_Core.BusinessRules;
using System.Threading.Tasks;// PRIMEPOS-2522
using POS_Core_UI.UserManagement;
using POS_Core.Resources.DelegateHandler;
using POS_Core.Resources;
using POS_Core_UI.UI;
#endregion

using System.Security.AccessControl;    //PRIMEPOS-2742 15-Oct-2019 JY Added
using System.Security.Permissions;  //PRIMEPOS-2742 15-Oct-2019 JY Added

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for clsMain.
    /// </summary>
    public class clsMain
    {
        private static clsLogin oLogin;
        public static frmMain Mainform = null;
        public static bool islogOff = false;
        public const int MinuteDiffereceToEgnore = 5;

        //Added by shitaljit on 20 August 2012
        //Delete system temp files at launch of POS.exe in a separate thread
        public static Thread tempFileDeleteThread;
        public static Thread systemDateTimeCheckingThread;
        public static Thread HPSFileDownloadThread;
        static public bool bApplicationClosed;  //PRIMEPOS-2553 27-Jun-2018 JY Added
        private static ILogger logger = LogManager.GetCurrentClassLogger(); //PRIMEPOS-2641 14-Feb-2019 JY Added
        private static frmLogin ofrmLogin; // NileshJ
        public static bool bAutomation = false;  //PRIMEPOS-2949 29-Mar-2021 JY Added
        public clsMain()
        {
            //oLogin = new clsLogin();

        }

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                bAutomation = HideMsgForScheduler(args);  //PRIMEPOS-2949 29-Mar-2021 JY Added

                bool bTempStatus = CreateINIFile();    //PRIMEPOS-2742 15-Oct-2019 JY Added
                if (!bTempStatus)
                {
                    MessageBox.Show("POS.ini file not found. Please check with your applicaiton manager.", "PrimePOS");
                    return;
                }

                #region PRIMEPOS-2553 27-Jun-2018 JY Added
                Application.ApplicationExit += new EventHandler(OnApplicationExit);
                System.Threading.Thread oThreadCheckForceClosed = new System.Threading.Thread(new System.Threading.ThreadStart(CheckForceClosedStatus));
                oThreadCheckForceClosed.IsBackground = true;
                oThreadCheckForceClosed.Start();
                bApplicationClosed = false;
                #endregion

                #region PRIMEPOS-2736 18-Sep-2019 JY Added
                try
                {
                    System.Threading.Thread oResetSystemSettings = new System.Threading.Thread(new System.Threading.ThreadStart(ResetSystemSettings));
                    oResetSystemSettings.IsBackground = true;
                    oResetSystemSettings.Start();
                }
                catch { }
                #endregion

                bool bStatus = DoPreStartUp(args);
                if (!bStatus) return;   //PRIMEPOS-2504 20-Apr-2018 JY added 
                //Application.Run(new NewfrmPOSTransaction());
                if (POS_Core.Resources.Configuration.CPOSSet.TurnOnEventLog == true)
                {
                    Logs.Init();
                }
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);
                // The following properties run the new process as administrator
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";
            }
            catch (Exception Ex)
            {
                //Logs.Logger("Fail to launched PrimePOS system as Administrator.");
                logger.Fatal(Ex, "Fail to launched PrimePOS system as Administrator - Main()"); //PRIMEPOS-2641 14-Feb-2019 JY Added
                return; //PRIMEPOS-2826 28-Apr-2020 JY Added 
            }
            //Logs.Logger("*****************************" + clsPOSDBConstants.Log_ApplicationLaunched + "*****************************");
            logger.Trace("*****************************" + clsPOSDBConstants.Log_ApplicationLaunched + "*****************************");
            CurrentExeVersion();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            Application.ApplicationExit += Application_ApplicationExit;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                //frmSplashScreen spScreen = new frmSplashScreen();
                //spScreen.Show();
                DoStartup(ref args);
                //Added By Shitaljit to do System DateTime check and System Tepm file delete operation in seperate Thread.
                if (POS_Core.Resources.Configuration.CInfo.UseSameThreadToVerifySysDateTime == true)
                {
                    CheckSystemDateTime();
                }
                else
                {
                    clsMain.systemDateTimeCheckingThread = new Thread(new ThreadStart(CheckSystemDateTime));
                    clsMain.systemDateTimeCheckingThread.Start();
                }

                clsMain.tempFileDeleteThread = new Thread(new ThreadStart(DeleteTempFiles));
                clsMain.tempFileDeleteThread.Start();

                #region clsUIHelper
                clsCoreUIHelper.ShowErrorMsg += new clsCoreUIHelper.DelErrorMsg(clsUIHelper.ShowErrorMsg);
                clsCoreUIHelper.ShowOKMsg += new clsCoreUIHelper.DelOKMsg(clsUIHelper.ShowOKMsg);
                clsCoreUIHelper.ShowWarningMsg += new clsCoreUIHelper.DelWarnErrorMsg(clsUIHelper.ShowWarningMsg);
                clsCoreUIHelper.ShowWarningMsg2 += new clsCoreUIHelper.DelWarnErrorMsg2(clsUIHelper.ShowWarningMsg);
                clsCoreUIHelper.ShowBtnErrorMsg += new clsCoreUIHelper.DelButtonErrorMsg(clsUIHelper.ShowBtnErrorMsg);
                clsCoreUIHelper.ShowBtnIconMsg += new clsCoreUIHelper.DelBtnIconMsg(clsUIHelper.ShowBtnIconMsg);
                clsCoreUIHelper.ShowLoginErrorMessage += new clsCoreUIHelper.DelBtnIconDMsg(clsUIHelper.ShowLoginErrorMessage);
                clsCoreUIHelper.GetNextNumber += new clsCoreUIHelper.DelNextNumber(clsUIHelper.GetNextNumber);
                clsCoreUIHelper.isNumeric += new clsCoreUIHelper.DelNumeric(clsUIHelper.isNumeric);
                clsCoreUIHelper.setColorSchecme += new clsCoreUIHelper.DelColorScheme(clsUIHelper.setColorSchecme);
                clsCoreUIHelper.GetLocalHostIP += new clsCoreUIHelper.DelHostip(clsUIHelper.GetLocalHostIP);
                clsCoreUIHelper.GetSignature += new clsCoreUIHelper.DelGetSignature(clsUIHelper.GetSignature);
                clsCoreUIHelper.GetSignaturePAX += new clsCoreUIHelper.DelGetSignaturePAX(clsUIHelper.GetSignaturePAX);//Arvind   //PRIMEPOS-2952
                clsCoreUIHelper.ConvertPoints += new clsCoreUIHelper.DelConvertPoints(clsUIHelper.ConvertPoints);//PRIMEPOS-2636 
                clsCoreUIHelper.AfterEnterEditMode += new clsCoreUIHelper.DelAfterEnterEditMode(clsUIHelper.AfterEnterEditMode);
                clsCoreUIHelper.AfterExitEditMode += new clsCoreUIHelper.DelAfterExitEditMode(clsUIHelper.AfterExitEditMode);
                clsCoreUIHelper.SetKeyActionMappings += new clsCoreUIHelper.DelSetKeyActionMappings(clsUIHelper.SetKeyActionMappings);
                clsCoreUIHelper.SetAppearance += new clsCoreUIHelper.DelSetAppearance(clsUIHelper.SetAppearance);
                #region PRIMEPOS-2761
                clsCoreUIHelper.GetRandomNo += new clsCoreUIHelper.DelGetRandomNumber(clsUIHelper.GetRandomNo);
                clsCoreUIHelper.DisplayYesNo += new clsCoreUIHelper.DelDisplayYesNo(Resources.Message.Display);
                #endregion 
                #endregion

                #region PrimerxHelper
                clsCorePrimeRXHelper.ImportPatientAsCustomer += new clsCorePrimeRXHelper.CoreImportPatientAsCustomer(PrimeRXHelper.ImportPatientAsCustomer);
                clsCorePrimeRXHelper.GetPatientDeliveryAddress += new clsCorePrimeRXHelper.CoreGetPatientDeliveryAddress(PrimeRXHelper.GetPatientDeliveryAddress);
                clsCorePrimeRXHelper.VarifyChargeAlreadyPosted += new clsCorePrimeRXHelper.CoreVarifyChargeAlreadyPosted(PrimeRXHelper.VarifyChargeAlreadyPosted);
                #endregion

                #region House Charge
                clsCoreHouseCharge.GetPatientByChargeAccountNumber += new clsCoreHouseCharge.CoreGetPatientByChargeAccountNumber(clsHouseCharge.GetPatientByChargeAccountNumber);
                clsCoreHouseCharge.getHouseChargeInfo += new clsCoreHouseCharge.CoregetHouseChargeInfo(clsHouseCharge.getHouseChargeInfo);
                clsCoreHouseCharge.postROA += new clsCoreHouseCharge.CorepostROA(clsHouseCharge.postROA);
                clsCoreHouseCharge.postHouseCharge += new clsCoreHouseCharge.CorepostHouseCharge(clsHouseCharge.postHouseCharge);
                clsCoreHouseCharge.GetAccountInformation += new clsCoreHouseCharge.CoreGetAccountInformation(clsHouseCharge.GetAccountInformation);

                #endregion

                #region Login
                clsLogin clsLogn = new clsLogin();
                clsCoreLogin.loginForPreviliges += new clsCoreLogin.coreloginForPreviliges(clsLogn.loginForPreviliges);
                //clsCoreHouseCharge.GetAccountInformation += new clsCoreHouseCharge.CoreGetAccountInformation(clsHouseCharge.GetAccountInformation);
                #endregion

                clsCoreUIHelper.GetFrmMain += new clsCoreUIHelper.DelFrmMain(frmMain.getInstance);
                clsCoreUIHelper.CheckPOSInstance += new clsCoreUIHelper.DelCheckPOSInstance(CheckPOSInstance);
                // NileshJ-Till

                #region  PRIMEPOS-3185
                //if (POS_Core.Resources.Configuration.CInfo.PWEncrypted == false)
                //{
                //    //POS_Core.ErrorLogging.Logs.Logger("Not RUN Encrypt User Password Logic", "PWEncrypted = FALSE ", " Running from Station# " + POS_Core.Resources.Configuration.StationID);
                //    logger.Trace("Not RUN Encrypt User Password Logic", "PWEncrypted = FALSE ", " Running from Station# " + Configuration.StationID);
                //    DBUser.UpdateAllUsersWithDBUsers();
                //}
                //else
                //{
                //    //POS_Core.ErrorLogging.Logs.Logger("Not RUN Encrypt User Password Logic", "PWEncrypted = TRUE ", " Running from Station# " + POS_Core.Resources.Configuration.StationID);
                //    logger.Trace("Not RUN Encrypt User Password Logic", "PWEncrypted = TRUE ", " Running from Station# " + Configuration.StationID);
                //}

                ////Following Code added by Krishna on 1 August 2011
                //DBUser.CheckForUsers();
                #endregion
                //Till here added by Krishna on 1 August 2011                
                if (!bAutomation)
                    CheckPOSInstance();

                //CheckAndUpdateMerchantConfig();   //Sprint-22 - 2240 29-Sep-2015 JY Added logic to check and update merchant info data  //PRIMEPOS-2227 05-May-2017 JY Added

                #region Sprint-20 01-Jun-2015 JY Commented the old code which is not in use
                //MMSComponentInfoUtil componentInfoUtil = new MMSComponentInfoUtil();
                //componentInfoUtil.SearchAndUpdateComponentInfo();
                #endregion

                //Login:
                oLogin = new clsLogin();
                oLogin.ConnString = POS_Core.Resources.Configuration.ConnectionString;
                POS_Core.Resources.Configuration.UserName = "";
                clsMain.HPSFileDownloadThread = new Thread(new ThreadStart(DownloadFileFromHPSFTP));
                clsMain.HPSFileDownloadThread.Start();

                //spScreen.Close();
                //	if (islogOff==false)
                //	{
                if (args.Length > 1)
                {
                    #region PRIMEPOS-2485 29-Mar-2021 JY Added
                    if (args[1].Trim().ToUpper() == "ScheduledTaskExecute".ToUpper())
                    {
                        ofrmLogin = frmLogin.GetInstance();

                        User oUserBL = new User();
                        UserData oUserData = new UserData();
                        int UserId = Configuration.convertNullToInt(Configuration.CSetting.SchedulerUser);
                        if (UserId != 0)
                        {
                            string whereClause = " WHERE ID = " + UserId;
                            oUserData = oUserBL.PopulateList(whereClause);
                            if (oUserData != null && oUserData.Tables.Count > 0 && oUserData.User.Rows.Count > 0)
                            {
                                ofrmLogin.username = Configuration.convertNullToString(oUserData.User.Rows[0]["UserID"]);
                                ofrmLogin.password = EncryptString.Decrypt(Configuration.convertNullToString(oUserData.User.Rows[0]["Password"]));
                            }
                        }
                        if (ofrmLogin.username == "")
                        {
                            string whereClause = " ORDER BY LASTLOGINATTEMPT DESC ";
                            oUserData = oUserBL.PopulateList(whereClause);
                            if (oUserData != null && oUserData.Tables.Count > 0 && oUserData.User.Rows.Count > 0)
                            {
                                ofrmLogin.username = Configuration.convertNullToString(oUserData.User.Rows[0]["UserID"]);
                                ofrmLogin.password = EncryptString.Decrypt(Configuration.convertNullToString(oUserData.User.Rows[0]["Password"]));
                            }
                        }
                        ofrmLogin.bScheduledTaskExecute = true;
                    }
                    #endregion
                    else
                    {
                        bool IsPOSLite = Convert.ToBoolean(args[1]);
                        string userName = args[3].ToString();
                        string password = args[4].ToString();
                        if (IsPOSLite == true)
                        {
                            ofrmLogin = frmLogin.GetInstance();
                            ofrmLogin.username = userName;
                            ofrmLogin.password = password;
                            ofrmLogin.IsPOSLite = IsPOSLite;
                        }
                    }
                }

                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "oLogin.login(LoginENUM.Login() ", clsPOSDBConstants.Log_Entering);
                logger.Trace("Main() - " + clsPOSDBConstants.Log_Entering);
                oLogin.login(LoginENUM.Login);
                logger.Trace("Main() - " + clsPOSDBConstants.Log_Exiting);
                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "oLogin.login(LoginENUM.Login() ", clsPOSDBConstants.Log_Exiting);

                //	}
                //else
                //{
                //		oLogin.login(LoginENUM.LogOff);
                //	}
                //islogOff=false;				
                if (!oLogin.IsCanceled)
                {
                    UpdatePayment_ResultFilePath(); //PRIMEPOS-2665 10-Apr-2019 JY Added
                    Mainform = frmMain.getInstance();
                    ErrorHandler.SaveLog((int)LogENUM.Application_Start, POS_Core.Resources.Configuration.UserName, "Success", "Application Started");
                    // Logs.Logger(clsPOSDBConstants.Log_ApplicationStarted);
                    logger.Trace("Main() - " + clsPOSDBConstants.Log_ApplicationStarted);
                    
                    if (args.Length > 1)
                    {
                        #region PRIMEPOS-2485 29-Mar-2021 JY Added
                        if (args[1].Trim().ToUpper() == "ScheduledTaskExecute".ToUpper())
                        {
                            try
                            {
                                if (args.Length > 1 && args[1].Trim().ToUpper() == "ScheduledTaskExecute".ToUpper())
                                {
                                    clsCommandLineOptions oCmdLine = new clsCommandLineOptions();
                                    if (oCmdLine.ExecuteCommand(args))
                                        return;
                                }
                            }
                            catch (Exception exp)
                            {
                                Console.WriteLine(exp.Message);
                                return;
                            }
                        }
                        #endregion
                        else
                        {
                            Application.Run(new frmPOSTransaction(args));
                        }
                    }
                    else
                    {
                        Application.Run(Mainform);
                    }
                }
                else
                {
                    //Logs.Logger("Application close", POS_Core.Resources.Configuration.UserName, "Cancel Login ");
                    logger.Trace("Application close", POS_Core.Resources.Configuration.UserName, "Cancel Login");
                    ErrorHandler.SaveLog((int)LogENUM.Application_Close, POS_Core.Resources.Configuration.UserName, "Success", "Login Cancelled");
                    Application.Exit();
                }
                //if(clsMain.islogOff==true)
                //{
                //	goto Login;
                //}
            }
            catch (Exception exp)
            {
                //Logs.Logger(exp.Message + exp.StackTrace);
                logger.Fatal(exp, "Main()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                //throw(exp);
            }
            finally
            {
                ///RemotingUtility.StopRemoteServer();
            }
        }

        #region PRIMEPOS-2742 15-Oct-2019 JY Added
        private static bool CreateINIFile()
        {
            bool bStatus = true;
            string strExePath = Application.ExecutablePath;
            string strIniFilePath = string.Empty;
            string strIniFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".ini";

            try
            {
                int nPos = strExePath.LastIndexOf(".");
                if (nPos > 0)
                    strIniFilePath = strExePath.Substring(0, nPos + 1) + "ini";
                else
                    strIniFilePath = strExePath + ".ini";

                if (!System.IO.File.Exists(strIniFilePath)) //check file exists
                {
                    string strTempPath = string.Empty;
                    if (strIniFilePath.Trim().EndsWith(".ini", StringComparison.OrdinalIgnoreCase))
                        strTempPath = Path.GetDirectoryName(strIniFilePath);
                    else
                        strTempPath = strIniFilePath;
                    if (IsDirectoryExistAndWritable(strTempPath) == false)
                    {
                        MessageBox.Show(strTempPath + " path " + "\n not found or inaccessible. Please check with your system or network administrator to ensure access to this path.");
                        return false;
                    }

                    //create ini file
                    string strConfigPath = Path.Combine(Environment.CurrentDirectory, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config");
                    if (System.IO.File.Exists(strConfigPath))
                    {
                        bStatus = WriteInINIFile(strConfigPath, strIniFilePath);
                    }
                }
                return bStatus;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CreateINIFile()");
                return false;
            }
        }

        private static bool IsDirectoryExistAndWritable(string dirPath)
        {
            bool bStatus = false;
            try
            {
                if (Directory.Exists(dirPath))
                {
                    using (FileStream fs = File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                    {
                        bStatus = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CreateINIFile()");
                bStatus = false;
            }
            return bStatus;
        }

        private const string sPOSSERVER = "POSSERVER";
        private const string sPOSDB = "POSDB";
        private const string sDBTYPE = "DBTYPE";
        private const string sDBSERVER = "DBSERVER";
        private const string sCATALOG = "CATALOG";
        private const string sInterfaceDB = "InterfaceDB=PrimeInterface";
        private const string sGSDDDB = "GSDDDB=GSDD";   //PRIMEPOS-2651 07-Mar-2022 JY Added

        private static bool WriteInINIFile(string strConfigPath, string strIniFilePath)
        {
            try
            {
                System.Configuration.ExeConfigurationFileMap fileMap = new System.Configuration.ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = strConfigPath;
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(fileMap, System.Configuration.ConfigurationUserLevel.None);

                string strFileData = string.Empty;
                try
                {
                    string strServerName = config.AppSettings.Settings["ServerName"].Value;
                    string strDataBase = config.AppSettings.Settings["DataBase"].Value;
                    string strDBTYPE = config.AppSettings.Settings["DBTYPE"].Value;
                    string strDBSERVER = config.AppSettings.Settings["DBSERVER"].Value;
                    string strCATALOG = config.AppSettings.Settings["CATALOG"].Value;

                    strFileData = sPOSSERVER + "=" + strServerName + Environment.NewLine +
                                        sPOSDB + "=" + strDataBase + Environment.NewLine +
                                        sDBTYPE + "=" + strDBTYPE + Environment.NewLine +
                                        sDBSERVER + "=" + strDBSERVER + Environment.NewLine +
                                        sCATALOG + "=" + strCATALOG + Environment.NewLine +
                                        sInterfaceDB + Environment.NewLine + sGSDDDB;   //PRIMEPOS-2651 07-Mar-2022 JY Added sGSDDDB
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "WriteInINIFile() - Inner Inner");
                    return false;
                }

                FileStream outStream = null;
                StreamWriter Writer = null;
                FileSecurity security = new FileSecurity();
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, strIniFilePath);
                permission.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, strIniFilePath);
                permission.Demand();

                using (outStream = new FileStream(strIniFilePath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                {
                    using (Writer = new StreamWriter(outStream))
                    {
                        Writer.WriteLine(strFileData);
                    }
                }

                return true;
            }
            catch (IOException Ex)
            {
                //throw Ex;
                logger.Fatal(Ex, "WriteInINIFile() - Inner");
                return false;
            }
            catch (Exception Ex)
            {
                //throw Ex;
                logger.Fatal(Ex, "WriteInINIFile() - Outer");
                return false;
            }
        }
        #endregion

        #region PRIMEPOS-2553 27-Jun-2018 JY Added
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                bApplicationClosed = true;
            }
            catch { }

        }

        private static void CheckForceClosedStatus()
        {
            try
            {
                while (true)
                {
                    if (bApplicationClosed)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(60000); //30 seconds
                    if (POS_Core.Resources.Configuration.GetForceClose())
                    {
                        //shut off the exe
                        MMSAppUpdater.clsProcessManagment oClsProcessManagment = new MMSAppUpdater.clsProcessManagment();
                        MMSAppUpdater.clsAppUpdate oclsApp = new MMSAppUpdater.clsAppUpdate();
                        oclsApp.ExeName = System.IO.Path.GetFileName(Application.ExecutablePath);
                        oClsProcessManagment.StopService(oclsApp);
                    }
                }
            }
            catch (Exception exp)
            {
                //clsUIHelper.ShowErrorMsg(exp.Message);
                logger.Fatal(exp, "CheckForceClosedStatus()"); //PRIMEPOS-2641 14-Feb-2019 JY Added
            }
        }
        #endregion

        #region PRIMEPOS-2736 24-Sep-2019 JY Added
        private static void ResetSystemSettings()
        {
            try
            {
                SetRegionalDecimalFormat(); //PRIMEPOS-2874 28-Jul-2020 JY Added
                ChangePowerOptions();
                ChangeUSBHubPowerManagement();
                SetPrivateNetwork();
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "ResetSystemSettings()");    //31-Dec-2019 JY Commented
            }
        }

        private static void ChangePowerOptions()
        {
            string strCommand = string.Empty;

            try
            {
                //Control Panel\Hardware and Sound\Power Options\Edit Plan Settings - Change Advanced Power Settings

                //USB Selective Suspend
                //Process.Start("cmd.exe", "/c powercfg /SETDCVALUEINDEX SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226 0");    //To disable USB Selective Suspend when on battery
                strCommand = "powercfg /SETDCVALUEINDEX SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226 0";    //To disable USB Selective Suspend when on battery
                ExecuteCommandSync(strCommand);
                strCommand = "powercfg /SETACVALUEINDEX SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226 0";    //To disable USB Selective Suspend when plugged in
                ExecuteCommandSync(strCommand);
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "ChangePowerOptions() - USB Selective Suspend"); //31-Dec-2019 JY Commented
            }

            try
            {
                //Prevent Hard Disk from going to Sleep in Windows 10
                strCommand = "powercfg /SETDCVALUEINDEX SCHEME_CURRENT 0012ee47-9041-4b5d-9b77-535fba8b1442 6738e2c4-e8a5-4a42-b16a-e040e769756e 0";    //On Battery: 0 is same as never
                ExecuteCommandSync(strCommand);
                strCommand = "powercfg /SETACVALUEINDEX SCHEME_CURRENT 0012ee47-9041-4b5d-9b77-535fba8b1442 6738e2c4-e8a5-4a42-b16a-e040e769756e 0";    //Plugged in: 0 is same as never
                ExecuteCommandSync(strCommand);
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "ChangePowerOptions() - Prevent Hard Disk from going to Sleep"); //31-Dec-2019 JY Commented
            }

            try
            {
                //disable sleep options
                strCommand = "powercfg /x -standby-timeout-ac 0";
                ExecuteCommandSync(strCommand);
                strCommand = "/c powercfg /x -standby-timeout-dc 0";
                ExecuteCommandSync(strCommand);

                //hybrid sleep and hibernet
                strCommand = "powercfg /hibernate off";
                ExecuteCommandSync(strCommand);
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "ChangePowerOptions() - disable sleep options"); //31-Dec-2019 JY Commented
            }
        }

        public static void ExecuteCommandSync(string strCommand)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + strCommand);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ExecuteCommandSync() - " + strCommand);
            }
        }

        private static void ChangeUSBHubPowerManagement()
        {
            try
            {
                //turn off power Device manager
                //it will disable (hide) "Power Management" tab for all USB hub
                //administrative priviliges requred to update registry entry

                if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", null) == null)
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1);
                }
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "ChangeUSBHubPowerManagement()");  //31-Dec-2019 JY Commented
            }
        }

        private static void SetPrivateNetwork()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (RegistryKey key = hklm.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles", true))
                    {
                        UpdateSubKeys(key);
                    }
                }
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "SetPrivateNetwork()");  //31-Dec-2019 JY Commented
            }
        }

        private static void UpdateSubKeys(RegistryKey SubKey)
        {
            try
            {
                foreach (string sub in SubKey.GetSubKeyNames())
                {
                    try
                    {
                        RegistryKey local = Registry.LocalMachine;
                        local = SubKey.OpenSubKey(sub, true);
                        if (local != null && local.GetValue("Category").ToString() == "0")
                            local.SetValue("Category", "1", RegistryValueKind.DWord);   //0 for Public, 1 for Local, 2 for Domain

                        UpdateSubKeys(local); // By recalling itselfit makes sure it get all the subkey names
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "UpdateSubKeys()");
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "UpdateSubKeys()");
            }
        }

        #region PRIMEPOS-2874 28-Jul-2020 JY Added
        private static void SetRegionalDecimalFormat()
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                if (Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalDigits == 0)
                {
                    object obj = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iDigits", "0");
                    if (obj.ToString() == "0")
                    {
                        Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\International", "iDigits", 2);
                        logger.Trace("SetRegionalDecimalFormat() - No. of digits after decimal have been set to 2");
                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception Ex)
            {
                //logger.Fatal(Ex, "ChangeUSBHubPowerManagement()");  //31-Dec-2019 JY Commented
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Author:Shitaljit added date: 11/12/2013
        /// </summary>
        private static void DownloadFileFromHPSFTP()
        {
            try
            {
                ///RemotingUtility.StartRemoteServer();
                //Added By Manoj 10/10/2013 To check HPS Sftp for new FSA files
                if (POS_Core.Resources.Configuration.CPOSSet.PaymentProcessor.ToUpper() == "HPS")
                {
                    if (!string.IsNullOrEmpty(POS_Core.Resources.Configuration.CInfo.HpsSftpHost) && !string.IsNullOrEmpty(POS_Core.Resources.Configuration.CInfo.HpsSftpUser)
                       && !string.IsNullOrEmpty(POS_Core.Resources.Configuration.CInfo.HpsSftpPassword))
                    {
                        try
                        {
                            MMS.HPS.HpsFileChecker HpsSftp = new MMS.HPS.HpsFileChecker(POS_Core.Resources.Configuration.CInfo.HpsSftpHost, POS_Core.Resources.Configuration.CInfo.HpsSftpPort,
                                                                                        POS_Core.Resources.Configuration.CInfo.HpsSftpUser, POS_Core.Resources.Configuration.CInfo.HpsSftpPassword);
                            if (HpsSftp.HPSFileCheck())
                            {
                                //Logs.Logger("HPS FSA FileCheck Pass");
                                logger.Trace("DownloadFileFromHPSFTP() - HPS FSA FileCheck Pass");
                            }
                            else
                            {
                                //Logs.Logger("*****>> HPS FSA FileCheck Failed.");
                                logger.Trace("DownloadFileFromHPSFTP() -  * ****>> HPS FSA FileCheck Failed.");
                            }
                        }
                        catch (Exception ex)
                        {
                            //Logs.Logger("--->>(clsMain) HPS Sftp fail to connect: " + ex.ToString());
                            logger.Fatal(ex, "DownloadFileFromHPSFTP()--->>(clsMain) HPS Sftp fail to connect");
                        }
                    }
                }//End Hps FSA

            }
            catch (Exception Ex)
            {
                //Logs.Logger("Error Occured while downloading files from HPS FPT" + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "DownloadFileFromHPSFTP() - Error Occured while downloading files from HPS FPT");
            }
            finally
            {
                if (HPSFileDownloadThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    HPSFileDownloadThread.Abort();
                }
            }
        }
        private static int CurrentExeVersion()
        {
            int CurrentExeVersion = 0;

            try
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
                string version = versionInfo.ProductVersion; // Will typically return current version of PrimePOS exe
                version = version.Replace(".", String.Empty);
                CurrentExeVersion = POS_Core.Resources.Configuration.convertNullToInt(version);
                //Logs.Logger("Current Version of PrimePOS exe " + CurrentExeVersion);
                logger.Trace("CurrentExeVersion() - Current Version of PrimePOS exe " + CurrentExeVersion);
            }
            catch (Exception Ex)
            {
                //Logs.Logger("Error Occured while checking exe version");
                logger.Fatal(Ex, "CurrentExeVersion() - Error Occured while checking exe version");
            }
            return CurrentExeVersion;
        }

        #region Application Exit/Crash Log

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            Exception ex = new Exception();
            try
            {
                ex = (Exception)e.ExceptionObject;

                clsCoreUIHelper.ShowBtnIconMsg("Whoops! Please contact the MMS with "
                    + "the following information:\n\n" + ex.Message + ex.StackTrace,
                    "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                //Logs.Logger(ex.Message + ex.StackTrace);
                logger.Fatal(ex, "CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)");
                Application.Exit();
            }

            #region Commented Code

            //Exception ex = (Exception)e.ExceptionObject;
            //ErrorHandler.logException(ex, "", "");

            //MessageBox.Show(e.ExceptionObject.ToString());

            #endregion Commented Code
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            //Do Things you wnat to do in Aplication Exit
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //Do Things you wnat to do in Aplication Exit
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            DialogResult result = DialogResult.Abort;
            try
            {
                result = Resources.Message.Display("Whoops! Please contact the MMS " + "with the following information:\n\n" + e.Exception.Message + e.Exception.StackTrace, "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
            }
            finally
            {
                //Logs.Logger(e.Exception.Message + e.Exception.StackTrace);
                logger.Fatal(e.Exception, "Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)");
                if (result == DialogResult.Abort)
                {
                    Application.Exit();
                }
            }
        }

        #endregion Application Exit/Crash Log

        //Added By Shitaljit(QuicSolv) on 3 June 2011
        /// <summary>
        /// To check whether multiple instance of POS is allowed to run in one system or not.
        /// </summary>
        public static void CheckPOSInstance()
        {
            //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "CheckPOSInstance() ", clsPOSDBConstants.Log_Entering);
            logger.Trace("CheckPOSInstance() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                if (POS_Core.Resources.Configuration.CInfo.AllowMultipleInstanceOfPOS == 2) return; //PRIMEPOS-2936 21-Jan-2021 JY Added
                Process currentProc = Process.GetCurrentProcess();
                string StationName = POS_Core.Resources.Configuration.ApplicationName + " - [" + POS_Core.Resources.Configuration.CInfo.StoreName + "] " + POS_Core.Resources.Configuration.StationID;
                Process[] pArry = Process.GetProcessesByName("POS");
                if (POS_Core.Resources.Configuration.CInfo.AllowMultipleInstanceOfPOS == 1)
                {
                    if (pArry.Length > 1)
                    {
                        foreach (Process RunningPOS in pArry)
                        {
                            if (StationName == RunningPOS.MainWindowTitle)
                            {
                                Resources.Message.Display("Already One Instance Of PrimePOS Is Running With Station ID  " + POS_Core.Resources.Configuration.StationID, "Duplicate Instance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                currentProc.Kill();
                            }
                        }
                    }
                }
                else
                {
                    if (pArry.Length > 1)
                    {
                        Resources.Message.Display("Already One Instance Of PrimePOS Is Running  ", "Duplicate Instance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        currentProc.Kill();
                    }
                }
            }
            catch (Exception exp)
            {
                //Logs.Logger(exp.Message + exp.StackTrace.ToString());
                logger.Fatal(exp, "CheckPOSInstance()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "CheckPOSInstance() ", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CheckPOSInstance() - " + clsPOSDBConstants.Log_Exiting);
        }

        /// <summary>
        /// Created By shitaljit on 20 August 2012
        /// This Function will delete temporary files of the system.
        /// </summary>
        private static void DeleteTempFiles()
        {
            try
            {

                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "DeleteTempFiles ", clsPOSDBConstants.Log_Entering);
                logger.Trace("DeleteTempFiles() - " + clsPOSDBConstants.Log_Entering);
                System.IO.DirectoryInfo tempDirInfo = new DirectoryInfo(System.IO.Path.GetTempPath());

                foreach (FileInfo file in tempDirInfo.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception Ex)
                    {
                        continue;
                    }
                }
                foreach (DirectoryInfo dir in tempDirInfo.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception Ex)
                    {
                        continue;
                    }
                }
            }
            catch{ } //PRIMEPOS-2997 02-Sep-2021 JY Added catch block
            finally
            {
                if (tempFileDeleteThread != null && tempFileDeleteThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    clsMain.tempFileDeleteThread.Abort();
                }
                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "DeleteTempFiles ", clsPOSDBConstants.Log_Exiting);
                logger.Trace("DeleteTempFiles() - " + clsPOSDBConstants.Log_Exiting);
            }
        }

        #region Checking system DateTime Against Server DateTime

        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;

            /// <summary>
            /// Convert form System.DateTime
            /// </summary>
            /// <param name="time"></param>
            public void FromDateTime(DateTime time)
            {
                wYear = (ushort)time.Year;
                wMonth = (ushort)time.Month;
                wDayOfWeek = (ushort)time.DayOfWeek;
                wDay = (ushort)time.Day;
                wHour = (ushort)time.Hour;
                wMinute = (ushort)time.Minute;
                wSecond = (ushort)time.Second;
                wMilliseconds = (ushort)time.Millisecond;
            }

            /// <summary>
            /// Convert to System.DateTime
            /// </summary>
            /// <returns></returns>
            public DateTime ToDateTime()
            {
                return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
            }

            /// <summary>
            /// STATIC: Convert to System.DateTime
            /// </summary>
            /// <param name="time"></param>
            /// <returns></returns>
            public static DateTime ToDateTime(SYSTEMTIME time)
            {
                return time.ToDateTime();
            }
        }

        /// <summary>
        /// This function retrieves the current system date
        /// and time expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="lpSystemTime">[out] Pointer to a SYSTEMTIME structure to
        /// receive the current system date and time.</param>
        [DllImport("kernel32.dll")]
        public extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        /// <summary>
        /// This function sets the current system date
        /// and time expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="lpSystemTime">[in] Pointer to a SYSTEMTIME structure that
        /// contains the current system date and time.</param>
        [DllImport("kernel32.dll")]
        public extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        //SetLocalTime C# Signature
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME Time);

        /// <summary>
        /// Created By shitaljit on 4 sept 2012
        /// This Function will Check System Date Time is correct or not.
        /// This code will get Current DateTime for specific time zone where application is being run.
        /// </summary>
        private static void CheckSystemDateTime()
        {
            //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "CheckSystemDateTime ", clsPOSDBConstants.Log_Entering);
            logger.Trace("CheckSystemDateTime() - " + clsPOSDBConstants.Log_Entering);
            DateTime dateTime = DateTime.MinValue;
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime ServerDateTime = DateTime.MinValue;

            string[] servers = new string[] {
                                                "http://nist.time.gov/timezone.cgi?UTC/s/0",
                                                "http://www.worldtimeserver.com/current_time_in_US-NY.aspx"
                                            };
            try
            {


                foreach (string server in servers)
                {
                    try
                    {
                        // Connect to the server and get the response
                        string serverResponse = string.Empty;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server);
                        request.Method = "POST";
                        request.Accept = "text/html, application/xhtml+xml, */*";
                        request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore); //No caching
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            StreamReader stream = new StreamReader(response.GetResponseStream());
                            string html = stream.ReadToEnd().ToUpper();
                            string time = Regex.Match(html, @">\d+:\d+:\d+<").Value; //HH:mm:ss format
                            string date = Regex.Match(html, @">\w+,\s\w+\s\d+,\s\d+<").Value; //dddd, MMMM dd, yyyy
                            dateTime = DateTime.Parse((date + " " + time).Replace(">", "").Replace("<", ""));
                            ServerDateTime = dateTime.ToLocalTime();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                        // Ignore exception and try the next server
                    }
                }

                DateTime currentDate = DateTime.Now;
                if (ServerDateTime != System.DateTime.MinValue)
                {
                    int MinuteDifference = ServerDateTime.Minute - currentDate.Minute;

                    if ((ServerDateTime.Day != currentDate.Day || ServerDateTime.Month != currentDate.Month || ServerDateTime.Year != currentDate.Year ||
                        ServerDateTime.Hour != currentDate.Hour) && Math.Abs(MinuteDifference) > clsMain.MinuteDiffereceToEgnore)
                    {
                        if (POS_Core_UI.Resources.Message.Display("Your current system date and time is: " + currentDate.ToString() + ".\nWe have detected the correct date and time in your time zone is : " + ServerDateTime.ToString() + "." + "\nDo you want to update your system with correct date and time detected?", "PrimePOS",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, FormStartPosition.CenterScreen) == DialogResult.Yes)
                        {
                            SYSTEMTIME st = new SYSTEMTIME();
                            st.FromDateTime(ServerDateTime);
                            SetLocalTime(ref st);
                            Console.WriteLine(st.ToString());
                        }
                    }
                }

                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "CheckSystemDateTime ", clsPOSDBConstants.Log_Exiting);
                logger.Trace("CheckSystemDateTime() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "CheckSystemDateTime ", clsPOSDBConstants.Log_Exception_Occured+Ex.Message + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "CheckSystemDateTime()");
                throw Ex;
            }
            finally
            {
                if (systemDateTimeCheckingThread != null && systemDateTimeCheckingThread.ThreadState == System.Threading.ThreadState.Running)
                {
                    systemDateTimeCheckingThread.Abort();
                }
            }
        }

        #endregion Checking system DateTime Against Server DateTime

        //Till here Added By Shitaljit
        private static bool DoPreStartUp(string[] args)
        {
            string sArg1 = "";
            if (args.Length == 0)
            {
                sArg1 = "01";
                //throw (new Exception("No station id is specified."));
            }
            else //(args.Length > 0)
            {
                sArg1 = args[0];
            }

            POS_Core.Resources.Configuration.StationID = sArg1;
            //Rite Code for creating a MMSsup user should come here.

            //PRIMEPOS-2522: NileshJ added hashmap to ensure different keys can be handled by MMSCL based on application
            Dictionary<string, string> dbParameters = new Dictionary<string, string>();
            dbParameters.Add("CONNECTSTRING", "CONNECTSTRING");
            dbParameters.Add("CATALOG", "DataBase");
            dbParameters.Add("DBSERVER", "ServerName");
            dbParameters.Add("DBTYPE", "DBTYPE");
            dbParameters.Add("GSDDDB", "");
            dbParameters.Add("USEINI", "True");
            DBConfig.DBParameters = dbParameters;
            Task tskCreateMMSSupUser = Task.Factory.StartNew(() => { DBConfig.CreateMMSSUPUser(); });
            //PRIMEPOS-2522 till here
            POS_Core.Resources.Configuration.UpdatePrimeEDISetting();   //PRIMEPOS-3167 07-Nov-2022 JY Added
            POS_Core.Resources.Configuration.GetUserSettings("");
            if (Configuration.GetSessionId() == 0)
            {
                bool bStatus = POS_Core.Resources.Configuration.ValidateStation(POS_Core.Resources.Configuration.StationID, bAutomation);
                if (!bStatus) return false; //PRIMEPOS-2504 20-Apr-2018 JY added 
            }            
            return true;
        }

        #region PRIMEPOS-2949 29-Mar-2021 JY Added
        private static bool HideMsgForScheduler(string[] args)
        {
            bool bHideMsg = false;
            try
            {
                if (args.Length > 1 && (args[1].Trim().ToUpper() == "ScheduledTaskExecute".ToUpper() || Configuration.convertNullToBoolean(args[1]) == true))
                    bHideMsg = true;
            }
            catch { }
            return bHideMsg;
        }
        #endregion

        private static void DoStartup(ref string[] args)
        {
            string sArg1 = "";
            //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "DoStartup ", clsPOSDBConstants.Log_Entering);
            logger.Trace("DoStartup(string[] args) - " + clsPOSDBConstants.Log_Entering);
            string cst = POS_Core.Resources.Configuration.ConnectionStringType;
            //DBUser.CheckandCreateUser("MMSI", "MMSPhW110"); //PRIMEPOS-3422

            //if (sArg1 == "01" || sArg1 == "1") //PRIMEPOS-3422
            //{
            //    string sRes = DBUser.CheckandCreateUser("MMSPrime", "asmms$");
            //    if (sRes == "Y")
            //        DBUser.CreateMMSUserS();
            //}
            POS_Core.Resources.Configuration.ConnectionStringType = "MasterDatabase";

            POS_Core.Resources.Configuration.ConnectionStringType = cst;
            sArg1 = null;

            #region PRIMEPOS-2993 23-Aug-2021 JY Added
            int sessionId = 0;
            TerminalSessionInfo sessionDetail = null;
            DataSet ds;
            string exePath = null;
            string exeName = null;
            FileVersionInfo versionInfo = null;
            try
            {
                if (args.Length == 0)
                {
                    sessionId = Configuration.GetSessionId();
                    if (sessionId > 0)
                    {
                        exePath = Assembly.GetEntryAssembly().Location;
                        exeName = exePath.Substring(exePath.LastIndexOf("\\") + 1, (exePath.LastIndexOf(".") - 1) - exePath.LastIndexOf("\\"));
                        versionInfo = FileVersionInfo.GetVersionInfo(exePath);
                        if (!string.IsNullOrWhiteSpace(exeName))
                        {
                            sessionDetail = TermServicesManager.GetSessionInfo(Environment.MachineName, sessionId); //Server Name
                            ds = Configuration.SaveApplicationVersionInfo(exeName, sessionDetail.ClientName, versionInfo.FileVersion, exePath);//Accessing Machine
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                args = new string[1];
                                args[0] = ds.Tables[0].Rows[0]["StationId"].ToString();
                                
                                string strSQL = "select stationname from util_POSSet where stationid='" + args[0]  + "'";
                                DataSet ds1 = DataHelper.ExecuteDataset(strSQL);
                                if (!(ds1 != null && ds1.Tables != null && ds1.Tables[0].Rows.Count > 0))
                                {
                                    string strSTATIONNAME = "STATION " + args[0];
                                    bool bStatus = Configuration.CreateStation(args[0], strSTATIONNAME);
                                    if (bStatus)
                                    {
                                        Configuration.StationName = strSTATIONNAME;
                                        Configuration.GetPOSWDirectoryPath();
                                    }
                                }
                            }
                            else
                            {
                                args = new string[1];
                                args[0] = "01";
                            }
                            Configuration.StationID = args[0];
                            Configuration.StationName = "STATION " + args[0];
                        }
                    }
                    //else
                    //{
                    //    args = new string[2];
                    //    args[0] = "01";
                    //    args[1] = "";
                    //}
                }
            }
            finally
            {
                sessionDetail = null;
                ds = null;
                exePath = null;
                exeName = null;
            }
            #endregion

            //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "DoStartup ", clsPOSDBConstants.Log_Exiting);
            logger.Trace("DoStartup(string[] args) - " + clsPOSDBConstants.Log_Exiting);
        }

        #region Sprint-22 - 2240 29-Sep-2015 JY
        //Added logic to check and update merchant info data
        private static void CheckAndUpdateMerchantConfig()
        {
            try
            {
                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "CheckAndUpdateMerchantConfig", clsPOSDBConstants.Log_Entering);
                logger.Trace("CheckAndUpdateMerchantConfig() - " + clsPOSDBConstants.Log_Entering);

                //if (string.IsNullOrWhiteSpace(Configuration.objMerchantConfig.STATIONID))    //check the merchant info entry into database w.r.t. logged in station 
                //{
                string strFileName = "MerchantConfig.xml";
                String strFileDirectoryPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath.ToString());
                String strFilePath = System.IO.Path.Combine(strFileDirectoryPath, strFileName);
                if (System.IO.File.Exists(strFilePath)) //check file exists
                {
                    //fetch the data from MerchantConfig.xml
                    Dictionary<string, string> dctMerchantConfig = new Dictionary<string, string>();
                    Dictionary<String, ProcessorInfo> CardProcessors = null;
                    Dictionary<string, ProcessorInfo> HPSHearders = null;
                    string value = String.Empty;
                    XmlToKeys GetDataFromXML = new XmlToKeys();
                    GetDataFromXML.GetFields(strFilePath, "", ref dctMerchantConfig, true);
                    if (dctMerchantConfig.Count > 0)
                    {
                        MerchantConfig oMerchantConfig = new MerchantConfig();

                        //oMerchantConfig.STATIONID = Configuration.StationID;
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_USER_ID, out oMerchantConfig.User_ID);

                        value = string.Empty;
                        if (dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_CARDINFO, out value))
                        {
                            CardProcessors = new Dictionary<String, ProcessorInfo>();
                            ParseCardList(value.Trim(), CardProcessors);
                            ProcessorInfo pInfo;
                            CardProcessors.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_CARD, out pInfo);
                            oMerchantConfig.Merchant = pInfo.MERCHNUM;
                            oMerchantConfig.Processor_ID = pInfo.PROCESSORID;
                        }

                        //PaymentServer - pending - get the file having this tag then we can work on this
                        value = string.Empty;
                        if (dctMerchantConfig.TryGetValue("  ", out value))
                        {
                            HPSHearders = new Dictionary<string, ProcessorInfo>();
                            ParseHPSHeaders(value.Trim(), HPSHearders);
                        }

                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_PAYMENT_SERVER, out oMerchantConfig.Payment_Server);

                        value = string.Empty;
                        if (dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_PORT_NO, out value))
                            oMerchantConfig.Port_No = POS_Core.Resources.Configuration.convertNullToString(value);

                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_LICENSEID, out oMerchantConfig.LicenseID);
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_SITEID, out oMerchantConfig.SiteID);
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_DEVICEID, out oMerchantConfig.DeviceID);
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_PAYMENTCLIENT, out oMerchantConfig.Payment_Client);
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_PAYMENTRESULTFILE, out oMerchantConfig.Payment_ResultFile);
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_APPLICATION_NAME, out oMerchantConfig.Application_Name);

                        value = string.Empty;
                        if (dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_XCCLIENTUITITLE, out value))
                        {
                            oMerchantConfig.XCClientUITitle = value;
                        }
                        else
                        {
                            oMerchantConfig.XCClientUITitle = "Prime POS CC Processing";
                        }

                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_URL, out oMerchantConfig.URL);
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_VCBIN, out oMerchantConfig.VCBin);
                        dctMerchantConfig.TryGetValue(clsPOSDBConstants.MerchantConfig_Fld_MCBIN, out oMerchantConfig.MCBin);

                        PreferenceSvr oPreSvr = new PreferenceSvr();
                        System.Data.IDbConnection oConn = null;
                        System.Data.IDbTransaction oTrans = null;
                        try
                        {
                            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                            oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
                            oPreSvr.UpdateMerchantConfig(oTrans, oMerchantConfig);
                            oTrans.Commit();

                            //initialise the list and Configuration.MerchantConfig object 
                            POS_Core.Resources.Configuration.lstMerchantConfig.Clear();
                            POS_Core.Resources.Configuration.lstMerchantConfig.Add(oMerchantConfig);
                            POS_Core.Resources.Configuration.objMerchantConfig = oMerchantConfig;

                            #region Sprint-22 - 2240 29-Sep-2015 JY Added logic to rename the file as MerchantConfigRetired.xml. BUT YET RELAVANT CHANGES IN PAYMENT PROCESSOR NOT DONE. HENCE TO AVOID EXPTION COMMENTING THIS CODE
                            //try
                            //{
                            //    string strNewFileName = "MerchantConfigRetired.xml";
                            //    String strNewFilePath = System.IO.Path.Combine(strFileDirectoryPath, strNewFileName);
                            //    if (File.Exists(strNewFilePath)) System.IO.File.Delete(strNewFilePath);
                            //    File.Move(strFilePath, strNewFilePath);
                            //}
                            //catch (Exception ex1)
                            //{
                            //    Logs.Logger(ex1.Message + ex1.StackTrace.ToString());
                            //}
                            #endregion
                        }
                        catch (Exception exp)
                        {
                            if (oTrans != null)
                                oTrans.Rollback();
                            throw (exp);
                        }
                    }
                }
                else
                {
                    //fetch the data from "01" station and replicate it to logged in station if "01" not found then get any data
                    Search oSearch = new Search();
                    bool isUpdate = false;
                    DataSet ds = oSearch.SearchData("SELECT * FROM MerchantConfig");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //if (ds.Tables[0].Rows[i][clsPOSDBConstants.MerchantConfig_Fld_STATIONID].ToString().Trim() == "01")
                            //{
                            UpdateMerchantConfig(ds.Tables[0].Rows[i]);
                            isUpdate = true;
                            break;
                            //}
                        }
                        if (isUpdate == false)
                        {
                            UpdateMerchantConfig(ds.Tables[0].Rows[0]);
                        }
                    }
                    else
                    {
                        //if no record found then pop up message as "Please update Merchant Information manually"    
                        Resources.Message.Display("Please update Merchant Information manually", "Merchant Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                //}
            }
            catch
            {
            }
            finally
            {
                //Logs.Logger(clsPOSDBConstants.Log_Module_Login, "CheckAndUpdateMerchantConfig", clsPOSDBConstants.Log_Exiting);
                logger.Trace("CheckAndUpdateMerchantConfig() - " + clsPOSDBConstants.Log_Exiting);
            }
        }

        private static void ParseCardList(String validCards, Dictionary<String, ProcessorInfo> CardProcessors)
        {
            XmlDocument msgDocument = new XmlDocument();
            XmlNodeList nodeList = null;
            try
            {
                msgDocument.LoadXml(validCards);
                nodeList = msgDocument.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    //if no filter is ther means all child from root elemet are required
                    ProcessorInfo procInfo = null;
                    if (node.Name == "CARD")
                    {
                        XmlNode attibute = node.Attributes.Item(0);
                        procInfo = new ProcessorInfo(node.OuterXml);
                        try
                        {
                            CardProcessors.Add(attibute.InnerText.Trim(), procInfo);
                        }
                        catch { }
                    }
                }
                nodeList = null;
                msgDocument.RemoveAll();
                msgDocument = null;
            }
            catch (Exception ex)
            {
                //Logwrite ex
            }
            return;

        }

        //This function is Parse HPS Header tags 
        private static void ParseHPSHeaders(String validNode, Dictionary<String, ProcessorInfo> HPSHearders)
        {
            XmlDocument msgDocument = new XmlDocument();
            XmlNodeList nodeList = null;
            try
            {
                msgDocument.LoadXml(validNode);
                nodeList = msgDocument.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    //if no filter is ther means all child from root elemet are required
                    ProcessorInfo procInfo = null;
                    if (node.Name == "HPS_SERVER")
                    {
                        XmlNode attibute = node.Attributes.Item(0);
                        procInfo = new ProcessorInfo(node.OuterXml);
                        try
                        {
                            HPSHearders.Add(attibute.InnerText.Trim(), procInfo);
                        }
                        catch { }
                    }
                }
                nodeList = null;
                msgDocument.RemoveAll();
                msgDocument = null;
            }
            catch (Exception ex)
            { }
        }

        private static bool UpdateMerchantConfig(DataRow dr)
        {
            MerchantConfig oMerchantConfig = new MerchantConfig();
            //oMerchantConfig.STATIONID = Configuration.StationID;
            oMerchantConfig.User_ID = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_USER_ID]).Replace("'", "''");
            oMerchantConfig.Merchant = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_Merch_Num]).Replace("'", "''");
            oMerchantConfig.Processor_ID = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_Processor_Id]).Replace("'", "''");
            oMerchantConfig.Payment_Server = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_PAYMENT_SERVER]).Replace("'", "''");
            oMerchantConfig.Port_No = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_PORT_NO]).Replace("'", "''");
            oMerchantConfig.Payment_Client = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_PAYMENTCLIENT]).Replace("'", "''");
            oMerchantConfig.Payment_ResultFile = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_PAYMENTRESULTFILE]).Replace("'", "''");
            oMerchantConfig.Application_Name = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_APPLICATION_NAME]).Replace("'", "''");
            oMerchantConfig.XCClientUITitle = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_XCCLIENTUITITLE]).Replace("'", "''");
            oMerchantConfig.LicenseID = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_LICENSEID]).Replace("'", "''");
            oMerchantConfig.SiteID = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_SITEID]).Replace("'", "''");
            oMerchantConfig.DeviceID = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_DEVICEID]).Replace("'", "''");
            oMerchantConfig.URL = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_URL]).Replace("'", "''");
            oMerchantConfig.VCBin = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_VCBIN]).Replace("'", "''");
            oMerchantConfig.MCBin = POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.MerchantConfig_Fld_MCBIN]).Replace("'", "''");

            PreferenceSvr oPreSvr = new PreferenceSvr();
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
                oPreSvr.UpdateMerchantConfig(oTrans, oMerchantConfig);
                oTrans.Commit();

                //initialise the list and Configuration.MerchantConfig object 
                POS_Core.Resources.Configuration.lstMerchantConfig.Clear();
                POS_Core.Resources.Configuration.lstMerchantConfig.Add(oMerchantConfig);
                POS_Core.Resources.Configuration.objMerchantConfig = oMerchantConfig;
                return true;
            }
            catch (Exception exp)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                return false;
                throw (exp);
            }
        }
        #endregion

        #region PRIMEPOS-2665 10-Apr-2019 JY Added  
        private static void UpdatePayment_ResultFilePath()
        {
            try
            {
                logger.Trace("UpdatePayment_ResultFilePath() - " + clsPOSDBConstants.Log_Entering);
                string ResultFileFolderName = "ResultFile";
                string ResultFileFolder = @"C:\" + ResultFileFolderName;

                Search oSearch = new Search();
                DataSet dsMerchantConfig = oSearch.SearchData("SELECT Payment_ResultFile FROM MerchantConfig");
                if (dsMerchantConfig != null && dsMerchantConfig.Tables.Count > 0 && dsMerchantConfig.Tables[0].Rows.Count > 0)
                {
                    if (Configuration.convertNullToString(dsMerchantConfig.Tables[0].Rows[0][0]).Trim().ToUpper() == "")
                    {
                        string strSQL = "UPDATE MerchantConfig SET Payment_ResultFile = '" + ResultFileFolder + "'";
                        System.Data.IDbTransaction oTrans = null;
                        System.Data.IDbConnection oConn = null;
                        try
                        {
                            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                            oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
                            IDbCommand cmd = DataFactory.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = strSQL;
                            cmd.Transaction = oTrans;
                            cmd.Connection = oTrans.Connection;
                            int nRowsAffected = cmd.ExecuteNonQuery();
                            oTrans.Commit();
                        }
                        catch (Exception Ex)
                        {
                            oTrans.Rollback();
                            logger.Fatal(Ex, "UpdatePayment_ResultFilePath() - Inner");
                        }
                    }

                    #region PRIMEPOS-2717 08-Aug-2019 JY Added
                    string strpath = Configuration.convertNullToString(dsMerchantConfig.Tables[0].Rows[0][0]).Trim();
                    if (strpath == "") strpath = ResultFileFolder;
                    if (strpath != "")
                    {
                        try
                        {
                            string[] arrFolders = strpath.Split('\\');
                            string newpath = arrFolders[0];

                            for (int i = 1; i < arrFolders.Length; i++)
                            {
                                newpath += @"\" + arrFolders[i];
                                if (!Directory.Exists(newpath))
                                {
                                    System.IO.Directory.CreateDirectory(newpath); // create folder if not exist.
                                }
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "UpdatePayment_ResultFilePath() - Create ResultFileFolder");
                        }
                    }
                    #endregion
                }
                logger.Trace("UpdatePayment_ResultFilePath() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdatePayment_ResultFilePath() - Outer");
            }
        }
        #endregion
        public enum EnumArgs
        {
            Station,
            // POSLiteExe,
            //App,
            IsPOSLite,
            RXNumbers,
            username,
            password
        }
    }
    //Farman Added
}
