//Sprint-22 - PRIMEPOS-2245 15-Oct-2015 JY Added class
namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;
    //using POS.Resources;
    //using POS.UI;
    using System.Collections.Generic;
    using System.Management;
    using System.Text;
    using System.Diagnostics;
    using Resources;

    public class SystemInfo : IDisposable
    {
        public void UpdateSystemInfo()
        {
            try
            {
                Logs.Logger("frmMain", "UpdateSystemInfo", clsPOSDBConstants.Log_Entering);
                using (SystemInfoData oSystemInfo = GetSystemInfo())
                {
                    if (oSystemInfo.Tables.Count > 0 && oSystemInfo.SystemInfo.Rows.Count > 0)
                    {
                        using (SystemInfoSvr dao = new SystemInfoSvr())
                        {
                            DataSet ds = GetSystemInfo(oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_SystemName].ToString());
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_PharmNo].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_PharmNo].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_OSName].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_OSName].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_Version].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_Version].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_SystemModel].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_SystemModel].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_SystemType].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_SystemType].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_Processor].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_Processor].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_RAM].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_RAM].ToString().ToUpper().Trim() ||
                                    ds.Tables[0].Rows[0][clsPOSDBConstants.SystemInfo_Fld_DriveInfo].ToString().ToUpper().Trim() != oSystemInfo.SystemInfo.Rows[0][clsPOSDBConstants.SystemInfo_Fld_DriveInfo].ToString().ToUpper().Trim())
                                {
                                    dao.Persist(oSystemInfo, 'U');
                                }
                            }
                            else
                            {
                                dao.Persist(oSystemInfo, 'I');
                            }
                        }
                    }
                }
                Logs.Logger("frmMain", "UpdateSystemInfo", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                Logs.Logger("SystemInfo", "UpdateSystemInfo", clsPOSDBConstants.Log_Exception_Occured + Ex.Message + Ex.StackTrace.ToString());
            }
        }
        
        public SystemInfoData GetSystemInfo()
        {
            string sOSName = string.Empty, sVersion = string.Empty, sSystemName = string.Empty, sSystemManufacturer = string.Empty, sSystemModel = string.Empty, sSystemType = string.Empty, sProcessor = string.Empty, sRAM = string.Empty, sDriveInfo = string.Empty;
            try
            {
                ManagementObjectSearcher searcher;

                searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
                foreach (ManagementObject mgtObject in searcher.Get())
                {
                    sOSName = mgtObject["Caption"].ToString();
                    break;
                }

                sVersion = Environment.OSVersion.ToString();
                sSystemName = Environment.MachineName.ToString();

                searcher = new ManagementObjectSearcher("SELECT Manufacturer, Model, SystemType FROM Win32_ComputerSystem");
                foreach (ManagementObject mgtObject in searcher.Get())
                {
                    sSystemManufacturer = mgtObject["Manufacturer"].ToString();
                    sSystemModel = mgtObject["Model"].ToString();
                    sSystemType = mgtObject["SystemType"].ToString();
                    break;
                }

                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject mgtObject in searcher.Get())
                {
                    sProcessor = mgtObject["name"].ToString();
                    break;
                }

                long mCap = 0;
                searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
                // In case more than one Memory sticks are installed
                foreach (ManagementObject mgtObject in searcher.Get())
                {
                    mCap += Configuration.convertNullToInt64(mgtObject["Capacity"]);

                }
                sRAM = (mCap / 1024 / 1024 / 1024).ToString() + " GB";

                StringBuilder sb = new StringBuilder();
                foreach (System.IO.DriveInfo oDriveInfo in System.IO.DriveInfo.GetDrives())
                {
                    try
                    {
                        sb.AppendFormat("Drive:{0}, Type:{1}, Format:{2}, TotalSize:{3} GB, FreeSpace: {4} GB | ",
                          oDriveInfo.Name, oDriveInfo.DriveType, oDriveInfo.DriveFormat, (oDriveInfo.TotalSize / 1024 / 1024 / 1024), (oDriveInfo.AvailableFreeSpace / 1024 / 1024 / 1024));
                    }
                    catch
                    {
                    }
                }
                if (sb.Length > 1000)
                    sDriveInfo = sb.ToString().Substring(0, 1000);
                else
                    sDriveInfo = sb.ToString();

                SystemInfoData oSystemInfoData = new SystemInfoData();
                oSystemInfoData.SystemInfo.AddRow(0,Configuration.CInfo.StoreID,sOSName,sVersion,sSystemName,sSystemManufacturer,sSystemModel,sSystemType,sProcessor,sRAM,sDriveInfo);
                return oSystemInfoData;
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        public SystemInfoData GetSystemInfo(string SystemName)
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetBySystemName(SystemName);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //Sprint-22 PRIMEPOS-2245 25-Nov-2015 JY Added
        public void UpdateApplicationInfo()
        {
            try
            {
                Logs.Logger("frmMain", "UpdateApplicationInfo", clsPOSDBConstants.Log_Entering);
                bool bUpdate = false;
                DataTable dtAppInfo = GetApplicationInfo();

                if (dtAppInfo != null && dtAppInfo.Rows.Count > 0)
                {
                    using (SystemInfoSvr dao = new SystemInfoSvr())
                    {
                        DataSet ds = GetApplicationLog(Environment.MachineName.ToString());
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAppInfo.Rows.Count; i++)
                            {
                                DataRow[] dr = ds.Tables[0].Select("SystemName = '" + dtAppInfo.Rows[i]["SystemName"].ToString() + "' AND AppId = " + dtAppInfo.Rows[i]["AppId"].ToString() + " AND StationId = '" + dtAppInfo.Rows[i]["StationId"].ToString() + "'");
                                if (dr.Length > 0)
                                {
                                    //if changed then update else ignore
                                    if (dtAppInfo.Rows[i]["Version"].ToString().ToUpper().Trim() != dr[0]["Version"].ToString().ToUpper().Trim() ||
                                        dtAppInfo.Rows[i]["BuildDate"].ToString().ToUpper().Trim() != dr[0]["BuildDate"].ToString().ToUpper().Trim() ||
                                        dtAppInfo.Rows[i]["AppPath"].ToString().ToUpper().Trim() != dr[0]["AppPath"].ToString().ToUpper().Trim())
                                    {
                                        dtAppInfo.Rows[i]["UpdStatus"] = "U";
                                        bUpdate = true;
                                    }
                                    else
                                    {
                                        dtAppInfo.Rows[i]["UpdStatus"] = "";
                                    }
                                }
                                else
                                {
                                    bUpdate = true;
                                }
                            }
                        }
                        else
                        {
                            bUpdate = true;
                        }

                        if (dtAppInfo.Rows.Count > 0 && bUpdate == true)
                        {
                            DataView view = new DataView(dtAppInfo);
                            DataTable distinctValues = view.ToTable(true,"SystemName","AppId","StationId","Version", "AppPath", "BuildDate");
                            dao.PersistApplicationLog(dtAppInfo);
                        }
                    }
                }
                Logs.Logger("frmMain", "UpdateApplicationInfo", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                Logs.Logger("SystemInfo", "UpdateApplicationInfo", clsPOSDBConstants.Log_Exception_Occured + Ex.Message + Ex.StackTrace.ToString());
            }
        }

        public void UpdateDllInfo()
        {
            try
            {
                Logs.Logger("frmMain", "UpdateDllInfo", clsPOSDBConstants.Log_Entering);
                bool bUpdate = false;
                DataTable dtDllInfo = GetDllInfo();

                if (dtDllInfo != null && dtDllInfo.Rows.Count > 0)
                {
                    using (SystemInfoSvr dao = new SystemInfoSvr())
                    {
                        DataSet ds = GetDllLog();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dtDllInfo.Rows.Count; i++)
                            {
                                DataRow[] dr = ds.Tables[0].Select("AppId = " + dtDllInfo.Rows[i]["AppId"].ToString());
                                if (dr.Length > 0)
                                {
                                    //if changed then update else ignore
                                    if (dtDllInfo.Rows[i]["Version"].ToString().ToUpper().Trim() != dr[0]["Version"].ToString().ToUpper().Trim() ||
                                        dtDllInfo.Rows[i]["BuildDate"].ToString().ToUpper().Trim() != dr[0]["BuildDate"].ToString().ToUpper().Trim())
                                    {
                                        dtDllInfo.Rows[i]["UpdStatus"] = "U";
                                        bUpdate = true;
                                    }
                                    else
                                    {
                                        dtDllInfo.Rows[i]["UpdStatus"] = "";
                                    }
                                }
                                else
                                {
                                    bUpdate = true;
                                }
                            }
                        }
                        else
                        {
                            bUpdate = true;
                        }

                        if (dtDllInfo.Rows.Count > 0 && bUpdate == true)
                        {
                            DataView view = new DataView(dtDllInfo);
                            DataTable distinctValues = view.ToTable(true, "AppId", "Version", "BuildDate");
                            dao.PersistApplicationLog(dtDllInfo);
                        }
                    }
                }
                Logs.Logger("frmMain", "UpdateDllInfo", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                Logs.Logger("SystemInfo", "UpdateDllInfo", clsPOSDBConstants.Log_Exception_Occured + Ex.Message + Ex.StackTrace.ToString());
            }
        }

        public DataSet GetApplicationList(string AppType)
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetApplicationList(AppType);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataTable GetApplicationInfo()
        {
            DataTable tblRunningApplications = null;
            try
            {
                DataSet dsApp = GetApplicationList("exe");  //get application list
                tblRunningApplications = GetRunningTasks(dsApp);
            }
            catch (Exception ex)
            {
                Logs.Logger("SystemInfo.cs", "GetApplicationInfo", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
            }
            return tblRunningApplications;
        }

        public DataTable GetDllInfo()
        {
            DataTable dt = null;
            try
            {
                DataSet dsApp = GetApplicationList("dll");  //get application list
                if (dsApp.Tables.Count > 0 && dsApp.Tables[0].Rows.Count > 0)
                {
                    dt = new DataTable();
                    dt.Columns.Add("PharmNo");
                    dt.Columns.Add("SystemName");
                    dt.Columns.Add("AppId");
                    dt.Columns.Add("AppName");
                    dt.Columns.Add("StationId");
                    dt.Columns.Add("Version");
                    dt.Columns.Add("BuildDate");
                    dt.Columns.Add("AppPath");
                    dt.Columns.Add("UpdStatus");

                    string AppPath = GetAppPath();
                    foreach (DataRow dr in dsApp.Tables[0].Rows)
                    {
                        try
                        {
                            DataRow drow = dt.NewRow();
                            drow["SystemName"]="";
                            drow["AppName"]="";
                            drow["StationId"] = "";

                            drow["PharmNo"] = Configuration.CInfo.StoreID;
                            drow["AppId"] = dr["AppId"].ToString();
                            //drow["AppName"] = dr["AppName"].ToString();
                            drow["AppPath"] = AppPath + @"\" + dr["AppName"].ToString();
                            if (drow["AppPath"] != null)
                            {
                                drow["Version"] = FileVersionInfo.GetVersionInfo(drow["AppPath"].ToString()).FileVersion;
                                drow["BuildDate"] = System.IO.File.GetLastWriteTime(drow["AppPath"].ToString());
                            }
                            drow["UpdStatus"] = "I";
                            dt.Rows.Add(drow);
                        }
                        catch
                        { 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.Logger("SystemInfo.cs", "GetDllInfo", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
            }
            return dt;
        }

        //Sprint-22 PRIMEPOS-2245 25-Nov-2015 JY Added
        public DataSet GetApplicationLog(string SystemName)
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetApplicationLog(SystemName);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataSet GetDllLog()
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetDllLog();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //Sprint-22 PRIMEPOS-2245 28-Oct-2015 JY Added
        public DataTable GetRunningTasks(DataSet dsApp)
        {
            try
            {
                Process[] RunningProcess = Process.GetProcesses();
                DataTable dtProcess = new DataTable();
                dtProcess.Columns.Add("PharmNo");
                dtProcess.Columns.Add("SystemName");
                dtProcess.Columns.Add("AppId");
                dtProcess.Columns.Add("AppName");
                dtProcess.Columns.Add("StationId");
                dtProcess.Columns.Add("Version");
                dtProcess.Columns.Add("BuildDate");
                dtProcess.Columns.Add("AppPath");
                dtProcess.Columns.Add("UpdStatus");
                
                foreach (Process EachProcess in RunningProcess)
                {
                    try
                    {
                        string sAppName = EachProcess.ProcessName;
                        if (sAppName.ToUpper().Contains(".EXE") == false)
                            sAppName = sAppName + ".exe";
                        DataRow[] dr = dsApp.Tables[0].Select("AppName ='" + sAppName + "'");   //return only specific application processes that we want to monitor
                        if (dr.Length > 0)
                        {
                            DataRow drow = dtProcess.NewRow();
                            drow["PharmNo"] = Configuration.CInfo.StoreID;
                            drow["SystemName"] = Environment.MachineName.ToString();
                            drow["AppId"] = dr[0]["AppId"].ToString();
                            drow["AppName"] = sAppName;
                            if (sAppName.ToUpper().Trim() == "POS.EXE")
                                drow["StationId"] = Configuration.StationID;
                            else
                                drow["StationId"] = "";
                            drow["AppPath"] = getPathByRunningInstance(sAppName);
                            if (drow["AppPath"] != null)
                            {
                                drow["Version"] = FileVersionInfo.GetVersionInfo(drow["AppPath"].ToString()).FileVersion;
                                drow["BuildDate"] = System.IO.File.GetLastWriteTime(drow["AppPath"].ToString());
                            }
                            drow["UpdStatus"] = "I";
                            dtProcess.Rows.Add(drow);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logs.Logger("SystemInfo.cs", "GetRunningTasks", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
                    }
                }
                //Add the running application in the list
                try
                {
                    bool isRunningPOSRecordExists = false;
                    if (dtProcess.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtProcess.Rows.Count; i++)
                        {
                            if ((dtProcess.Rows[i]["AppName"].ToString().ToUpper().Trim() == "POS.EXE") && (dtProcess.Rows[i]["SystemName"].ToString().ToUpper().Trim() == Environment.MachineName.ToString().ToUpper().Trim()) && (dtProcess.Rows[i]["StationId"].ToString().ToUpper().Trim() == Configuration.StationID.ToUpper().Trim()))
                            {
                                isRunningPOSRecordExists = true;
                                break;
                            }
                        }
                    }
                    if (isRunningPOSRecordExists == false)
                    {
                        DataRow drow = dtProcess.NewRow();
                        drow["PharmNo"] = Configuration.CInfo.StoreID;
                        drow["SystemName"] = Environment.MachineName.ToString();
                        DataRow[] dr = dsApp.Tables[0].Select("AppName ='POS.EXE'");
                        if (dr.Length > 0)
                            drow["AppId"] = dr[0]["AppId"].ToString();
                        drow["AppName"] = "POS.EXE";
                        drow["StationId"] = Configuration.StationID;
                        drow["AppPath"] = GetAppPath();
                        if (drow["AppPath"] != null)
                        {
                            drow["AppPath"] = drow["AppPath"] + @"\POS.EXE";
                            drow["Version"] = FileVersionInfo.GetVersionInfo(drow["AppPath"].ToString()).FileVersion;
                            drow["BuildDate"] = System.IO.File.GetLastWriteTime(drow["AppPath"].ToString());
                        }
                        drow["UpdStatus"] = "I";
                        dtProcess.Rows.Add(drow);
                    }
                }
                catch (Exception ex)
                {
                    Logs.Logger("SystemInfo.cs", "GetRunningTasks", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
                }

                return dtProcess;
            }
            catch (Exception ex1)
            {
                Logs.Logger("SystemInfo.cs", "GetRunningTasks", clsPOSDBConstants.Log_Exception_Occured + ex1.Message + ex1.StackTrace.ToString());
                return null;
            }
        }

        //Sprint-22 - PRIMEPOS-2245 24-Nov-2015 JY Added
        public string getPathByRunningInstance(string sAppName)
        {
            string ProcessName = string.Empty;
            try
            {
                ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");

                ObjectQuery query = new ObjectQuery("select ExecutablePath from win32_process where name='" + sAppName.Trim() + "'");

                //create object searcher
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                //get collection of WMI objects
                ManagementObjectCollection queryCollection = searcher.Get();

                //enumerate the collection.
                foreach (ManagementObject m in queryCollection)
                {
                    if (sAppName.Equals(System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath), StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessName = System.Windows.Forms.Application.ExecutablePath;
                        break;
                    }
                    else
                    {
                        try
                        {
                            if (m["ExecutablePath"] != null)
                                ProcessName = m["ExecutablePath"].ToString();// get the file name from the process 
                            break;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log.AddError(String.Format("Always Running Product Error {0}  ", ex.Message));
                Logs.Logger("SystemInfo.cs", "getPathByRunningInstance", clsPOSDBConstants.Log_Exception_Occured + ex.Message + ex.StackTrace.ToString());
            }
            return ProcessName;
        }

        private string GetAppPath()
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
                    sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(this.GetType()).Location);
                }
                catch
                {
                    sAppPath = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            return sAppPath;
        }

        #region Sprint-25 - PRIMEPOS-2245 06-Mar-2017 JY Added logic to log POS settings
        public void UpdateSystemLevelSettingsLog()
        {
            try
            {
                Logs.Logger("frmMain", "UpdateSystemLevelSettingsLog", clsPOSDBConstants.Log_Entering);
                if (Configuration.CInfo.StoreID.Trim() == string.Empty)
                {
                    Logs.Logger("Can't update System Level Settings log as StoreID/PharmNo is blank");
                    return;
                }

                DataTable dtSystemLevelSettingsLog = new DataTable();
                dtSystemLevelSettingsLog.Columns.Add("PharmNo");
                dtSystemLevelSettingsLog.Columns.Add("StoreID");
                dtSystemLevelSettingsLog.Columns.Add("CompanyName");
                dtSystemLevelSettingsLog.Columns.Add("MerchantNo");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_User_ID");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_Merchant");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_Processor_ID");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_Payment_Server");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_Port_No");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_Payment_Client");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_Payment_ResultFile");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_Application_Name");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_XCClientUITitle");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_LicenseID");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_SiteID");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_DeviceID");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_URL");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_VCBin");
                dtSystemLevelSettingsLog.Columns.Add("MerchantConfig_MCBin");
                dtSystemLevelSettingsLog.Columns.Add("UpdatedOn");
                dtSystemLevelSettingsLog.Columns.Add("Status");

                DataSet dsSystemLevelSettings = GetSystemLevelSettings();
                if (dsSystemLevelSettings != null && dsSystemLevelSettings.Tables.Count > 0)
                {
                    DataRow drow = dtSystemLevelSettingsLog.NewRow();
                    drow["PharmNo"] = Configuration.CInfo.StoreID;

                    //Util_Company_Info table parameters
                    if (dsSystemLevelSettings.Tables[0].Rows.Count > 0)
                    {
                        drow["StoreID"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[0].Rows[0]["StoreID"]);
                        drow["CompanyName"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[0].Rows[0]["CompanyName"]);
                        drow["MerchantNo"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[0].Rows[0]["MerchantNo"]);
                    }

                    if (dsSystemLevelSettings.Tables.Count > 1 && dsSystemLevelSettings.Tables[1].Rows.Count > 0)
                    {
                        //MerchantConfig tabe parameters
                        drow["MerchantConfig_User_ID"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["User_ID"]);
                        drow["MerchantConfig_Merchant"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["Merchant"]);
                        drow["MerchantConfig_Processor_ID"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["Processor_ID"]);
                        drow["MerchantConfig_Payment_Server"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["Payment_Server"]);
                        drow["MerchantConfig_Port_No"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["Port_No"]);
                        drow["MerchantConfig_Payment_Client"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["Payment_Client"]);
                        drow["MerchantConfig_Payment_ResultFile"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["Payment_ResultFile"]);
                        drow["MerchantConfig_Application_Name"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["Application_Name"]);
                        drow["MerchantConfig_XCClientUITitle"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["XCClientUITitle"]);
                        drow["MerchantConfig_LicenseID"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["LicenseID"]);
                        drow["MerchantConfig_SiteID"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["SiteID"]);
                        drow["MerchantConfig_DeviceID"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["DeviceID"]);
                        drow["MerchantConfig_URL"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["URL"]);
                        drow["MerchantConfig_VCBin"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["VCBin"]);
                        drow["MerchantConfig_MCBin"] = Configuration.convertNullToString(dsSystemLevelSettings.Tables[1].Rows[0]["MCBin"]);
                    }
                    dtSystemLevelSettingsLog.Rows.Add(drow);

                    DataSet dsSystemLevelSettingsLog = GetSystemLevelSettingsLog();
                    if (dsSystemLevelSettingsLog != null && dsSystemLevelSettingsLog.Tables.Count > 0 && dsSystemLevelSettingsLog.Tables[0].Rows.Count > 0)
                    {
                        //update if change found
                        if (drow["StoreID"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["StoreID"]).Trim().ToUpper() ||
                            drow["CompanyName"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["CompanyName"]).Trim().ToUpper() ||
                            drow["MerchantNo"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantNo"]).Trim().ToUpper() ||
                            drow["MerchantConfig_User_ID"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_User_ID"]).Trim().ToUpper() ||
                            drow["MerchantConfig_Merchant"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_Merchant"]).Trim().ToUpper() ||
                            drow["MerchantConfig_Processor_ID"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_Processor_ID"]).Trim().ToUpper() ||
                            drow["MerchantConfig_Payment_Server"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_Payment_Server"]).Trim().ToUpper() ||
                            drow["MerchantConfig_Port_No"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_Port_No"]).Trim().ToUpper() ||
                            drow["MerchantConfig_Payment_Client"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_Payment_Client"]).Trim().ToUpper() ||
                            drow["MerchantConfig_Payment_ResultFile"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_Payment_ResultFile"]).Trim().ToUpper() ||
                            drow["MerchantConfig_Application_Name"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_Application_Name"]).Trim().ToUpper() ||
                            drow["MerchantConfig_XCClientUITitle"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_XCClientUITitle"]).Trim().ToUpper() ||
                            drow["MerchantConfig_LicenseID"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_LicenseID"]).Trim().ToUpper() ||
                            drow["MerchantConfig_SiteID"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_SiteID"]).Trim().ToUpper() ||
                            drow["MerchantConfig_DeviceID"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_DeviceID"]).Trim().ToUpper() ||
                            drow["MerchantConfig_URL"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_URL"]).Trim().ToUpper() ||
                            drow["MerchantConfig_VCBin"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_VCBin"]).Trim().ToUpper() ||
                            drow["MerchantConfig_MCBin"].ToString().Trim().ToUpper() != Configuration.convertNullToString(dsSystemLevelSettingsLog.Tables[0].Rows[0]["MerchantConfig_MCBin"]).Trim().ToUpper())
                        {
                            using (SystemInfoSvr dao = new SystemInfoSvr())
                            {
                                dao.PersistSystemLevelSettingsLog(dtSystemLevelSettingsLog, "U");
                            }
                        }
                    }
                    else
                    {
                        //insert
                        using (SystemInfoSvr dao = new SystemInfoSvr())
                        {
                            dao.PersistSystemLevelSettingsLog(dtSystemLevelSettingsLog, "I");
                        }
                    }
                }
                Logs.Logger("frmMain", "UpdateSystemLevelSettingsLog", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                Logs.Logger("SystemInfo", "UpdateSystemLevelSettingsLog", clsPOSDBConstants.Log_Exception_Occured + Ex.Message + Ex.StackTrace.ToString());
            }
        }

        public DataSet GetSystemLevelSettings()
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetSystemLevelSettings();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataSet GetSystemLevelSettingsLog()
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetSystemLevelSettingsLog();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void UpdateStationLevelSettingsLog()
        {
            try
            {
                Logs.Logger("frmMain", "UpdateStationLevelSettingsLog", clsPOSDBConstants.Log_Entering);
                bool bUpdate = false;

                if (Configuration.CInfo.StoreID.Trim() == string.Empty)
                {
                    Logs.Logger("Can't update Station Level Settings log as StoreID/PharmNo is blank");
                    return;
                }

                DataSet dsStationLevelSettings = GetStationLevelSettings();
                if (dsStationLevelSettings != null && dsStationLevelSettings.Tables.Count > 0)
                {
                    using (SystemInfoSvr dao = new SystemInfoSvr())
                    {
                        DataSet dsLog = GetStationLevelSettingsLog();
                        if (dsLog != null && dsLog.Tables.Count > 0 && dsLog.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsStationLevelSettings.Tables[0].Rows.Count; i++)
                            {
                                DataRow[] drLog = dsLog.Tables[0].Select("StationId = '" + dsStationLevelSettings.Tables[0].Rows[i]["StationId"].ToString() + "'");
                                if (drLog.Length > 0)
                                {
                                    //if changed then update else ignore
                                    if (dsStationLevelSettings.Tables[0].Rows[i]["StationName"].ToString().ToUpper().Trim() != drLog[0]["StationName"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["UsePoleDSP"].ToString().ToUpper().Trim() != drLog[0]["UsePoleDSP"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["UseCashDRW"].ToString().ToUpper().Trim() != drLog[0]["UseCashDRW"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["UsePinPad"].ToString().ToUpper().Trim() != drLog[0]["UsePinPad"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["USESigPad"].ToString().ToUpper().Trim() != drLog[0]["USESigPad"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["SigPadHostAddr"].ToString().ToUpper().Trim() != drLog[0]["SigPadHostAddr"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["PinPadModel"].ToString().ToUpper().Trim() != drLog[0]["PinPadModel"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["PaymentProcessor"].ToString().ToUpper().Trim() != drLog[0]["PaymentProcessor"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["HPS_UserName"].ToString().ToUpper().Trim() != drLog[0]["HPS_UserName"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["HPS_Password"].ToString().ToUpper().Trim() != drLog[0]["HPS_Password"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["UsePrimePO"].ToString().ToUpper().Trim() != drLog[0]["UsePrimePO"].ToString().ToUpper().Trim() ||
                                        dsStationLevelSettings.Tables[0].Rows[i]["DefaultVendor"].ToString().ToUpper().Trim() != drLog[0]["DefaultVendor"].ToString().ToUpper().Trim())
                                    {
                                        dsStationLevelSettings.Tables[0].Rows[i]["UpdStatus"] = "U";
                                        bUpdate = true;
                                    }
                                    else
                                    {
                                        dsStationLevelSettings.Tables[0].Rows[i]["UpdStatus"] = "";
                                    }
                                }
                                else
                                {
                                    bUpdate = true;
                                }
                            }
                        }
                        else
                        {
                            bUpdate = true;
                        }

                        if (bUpdate == true)
                        {
                            dao.PersistStationLevelSettingsLog(dsStationLevelSettings.Tables[0]);
                        }
                    }
                }
                Logs.Logger("frmMain", "UpdateStationLevelSettingsLog", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                Logs.Logger("SystemInfo", "UpdateStationLevelSettingsLog", clsPOSDBConstants.Log_Exception_Occured + Ex.Message + Ex.StackTrace.ToString());
            }
        }

        public DataSet GetStationLevelSettings()
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetStationLevelSettings();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataSet GetStationLevelSettingsLog()
        {
            try
            {
                using (SystemInfoSvr dao = new SystemInfoSvr())
                {
                    return dao.GetStationLevelSettingsLog();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }

    #region Sprint-25 - PRIMEPOS-2245 06-Mar-2017 JY Added logic to log POS settings
    public class SystemLevelSettingsLog : IDisposable
    {
        private string _PharmNo;
        private string _StoreID;
        private string _CompanyName;
        private string _MerchantNo;
        private string _MerchantConfig_User_ID;
        private string _MerchantConfig_Merchant;
        private string _MerchantConfig_Processor_ID;
        private string _MerchantConfig_Payment_Server;
        private string _MerchantConfig_Port_No;
        private string _MerchantConfig_Payment_Client;
        private string _MerchantConfig_Payment_ResultFile;
        private string _MerchantConfig_Application_Name;
        private string _MerchantConfig_XCClientUITitle;
        private string _MerchantConfig_LicenseID;
        private string _MerchantConfig_SiteID;
        private string _MerchantConfig_DeviceID;
        private string _MerchantConfig_URL;
        private string _MerchantConfig_VCBin;
        private string _MerchantConfig_MCBin;
        private DateTime _UpdatedOn;
        private Boolean _Status;
        
        public string PharmNo { get; set; }
        public string StoreID { get; set; }
        public string CompanyName { get; set; }
        public string MerchantNo { get; set; }
        public string MerchantConfig_User_ID { get; set; }
        public string MerchantConfig_Merchant { get; set; }
        public string MerchantConfig_Processor_ID { get; set; }
        public string MerchantConfig_Payment_Server { get; set; }
        public string MerchantConfig_Port_No { get; set; }
        public string MerchantConfig_Payment_Client { get; set; }
        public string MerchantConfig_Payment_ResultFile { get; set; }
        public string MerchantConfig_Application_Name { get; set; }
        public string MerchantConfig_XCClientUITitle { get; set; }
        public string MerchantConfig_LicenseID { get; set; }
        public string MerchantConfig_SiteID { get; set; }
        public string MerchantConfig_DeviceID { get; set; }
        public string MerchantConfig_URL { get; set; }
        public string MerchantConfig_VCBin { get; set; }
        public string MerchantConfig_MCBin { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Boolean Status { get; set; }

        //public DataTable objToDataTable(SystemLevelSettingsLog oSystemLevelSettingsLog)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Column_Name");
        //    foreach (PropertyInfo info in typeof(SystemLevelSettingsLog).GetProperties())
        //    {
        //        dt.Rows.Add(info.Name);
        //    }
        //    dt.AcceptChanges();
        //    return dt;
        //}

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
    #endregion
}
