using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Windows.Forms;
using MMSAppUpdater.MMSUpdate;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MMSAppUpdater
{
    public enum enmDownloadAndIntsallStatus
    {
        Latest_Version,
        Download_Pending,
        Download_Complete,
        Installation_Start,
        Installation_Complete,
        Installation_Incomplate
    }

    public class clsUpdateManager
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        private List<clsAppUpdate> oColAppUpdates = new List<clsAppUpdate>();
        private Form oParentFrm = new Form();
        MmsUPdateService oMmsUpdateService = new MmsUPdateService();
        private string m_sServiceAddress = "";
        private string m_sPharmNo = "";
        private string m_AppName = "";
        private clsAppInstance oAppIns = new clsAppInstance();
        private clsDBConfigHelper odbMgr;
        private string StationId = "00";
        public BackgroundWorker bw = new BackgroundWorker();
        public BackgroundWorker bws = new BackgroundWorker();
        
        public clsLogService log = new clsLogService("SetupLogPOS_Update.log");
        public clsLogService TempLog = new clsLogService("AutoUpdaterEx.log");  //Added to log exceptions

        //this Initialize only call in Appstarter
        public clsUpdateManager(string sConnStr, string sApplName, string StationId)
        {
            if (odbMgr == null)
            {
                odbMgr = new clsDBConfigHelper(sConnStr);
                odbMgr.SaveAppVersion(sApplName, StationId,"", enmDownloadAndIntsallStatus.Installation_Complete.ToString());
            }

            bw.WorkerReportsProgress = true;
            bws.WorkerReportsProgress = true;
        }

        public clsUpdateManager()
        {
            odbMgr = new clsDBConfigHelper("");
        }

        public clsUpdateManager(string sWebServiceAddress, string sPharmNo, string sConnStr, string AppName, string StationId)
        {
            odbMgr = new clsDBConfigHelper(sConnStr); // Set the Connction String
            odbMgr.SaveAppVersion(StationId); // Update Prime POS version 
            m_sServiceAddress = sWebServiceAddress;//Set Servervice address 
            m_sPharmNo = sPharmNo; // pharmacy No
            odbMgr.ConnStr = sConnStr;// Connection String
            this.m_AppName = AppName; // Application Name
            oMmsUpdateService = new MmsUPdateService();
            oMmsUpdateService.Url = m_sServiceAddress;
            this.StationId = StationId; //APP Station Id
        }

        public clsUpdateManager(string sWebServiceAddress, string sPharmNo, string sConnStr, string AppName, string StationId, bool bUrgentBackgroundWorker)
        {
            odbMgr = new clsDBConfigHelper(sConnStr); // Set the Connction String
            odbMgr.SaveAppVersion(StationId); // Update Prime POS version 
            m_sServiceAddress = sWebServiceAddress;//Set Servervice address 
            m_sPharmNo = sPharmNo; // pharmacy No
            odbMgr.ConnStr = sConnStr;// Connection String
            this.m_AppName = AppName; // Application Name
            oMmsUpdateService = new MmsUPdateService();
            oMmsUpdateService.Url = m_sServiceAddress;
            this.StationId = StationId; //APP Station Id
            if (bUrgentBackgroundWorker == true)
            {
                this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
                this.bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);
                this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            }
            else 
            {
                this.bws.DoWork += new System.ComponentModel.DoWorkEventHandler(bws_DoWork);
                this.bws.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bws_ProgressChanged);
                this.bws.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bws_RunWorkerCompleted);
            }
        }

        public void SetParentForm(Form OParentFrm)
        {
            oParentFrm = OParentFrm;
        }

        public void checkforNewUpdates()
        {
            BackgroundWorker bwGetAppsList = new BackgroundWorker();
            bwGetAppsList.WorkerSupportsCancellation = true;
            bwGetAppsList.DoWork += new DoWorkEventHandler(bw_DoWork);
            bwGetAppsList.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bwGetAppsList.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bwGetAppsList.RunWorkerAsync();
        }
        
        void bws_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void bws_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                popupFornewUpdates(oParentFrm);
            }
            catch (Exception exp)
            {
                log.AddError(String.Format("Urgent Update Thread Error {0}", exp.Message));
            }
        }

        void bws_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<clsAppUpdate> oColAppUpdatesNotUrgent = new List<clsAppUpdate>();

                // get list of applications/updates from server
                DataTable oDtAppsList = GetApplicationsListfromServer();
                // create objetcs collection out of datatable
                oColAppUpdates = GetAppsCollection(oDtAppsList);
                // get list of updates for which new version is available and marked as urgent
                oColAppUpdatesNotUrgent = getListOfDownloadablesUrgent(oColAppUpdates, false);

                // schedule downloading for all passed Apps (do it in seperate thread worker and on completion event call popupFornewUpdates)
                downloadAllApps(oColAppUpdatesNotUrgent);

                // we are calling popupFornewUpdates in completed event see bw_RunWorkerCompleted
            }
            catch (Exception exp)
            {
                log.AddError(String.Format("Normal Updates Thread Error {0}  ", exp.Message));
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                popupFornewUpdates(oParentFrm);
            }
            catch (Exception exp)
            {
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<clsAppUpdate> oColAppUpdatesUrgent = new List<clsAppUpdate>();
                //List<clsAppUpdate> oColAppUpdatesNotUrgent = new List<clsAppUpdate>();

                // get list of applications/updates from server
                DataTable oDtAppsList = GetApplicationsListfromServer();
                // create objetcs collection out of datatable
                oColAppUpdates = GetAppsCollection(oDtAppsList);
                // get list of updates for which new version is available and marked as urgent
                oColAppUpdatesUrgent = getListOfDownloadablesUrgent(oColAppUpdates, true);
                // get list of updates for which new version is available and marked as urgent
                //oColAppUpdatesNotUrgent = getListOfDownloadablesUrgent(oColAppUpdates, false);

                // schedule downloading for all passed Apps (do it in seperate thread worker and on completion event call popupFornewUpdates)
                //ScheduleDownload(oColAppUpdatesNotUrgent);

                // download immigiatly
                downloadAllApps(oColAppUpdatesUrgent);

                // we are calling popupFornewUpdates in completed event see bw_RunWorkerCompleted
            }
            catch (Exception exp)
            {
                log.AddError(String.Format("Normal Updates Thread Error {0}  ", exp.Message));
            }
        }

        // simply download all apps in application directory download folder in its own folder
        public bool downloadAllApps(List<clsAppUpdate> oColAppUpdates)
        {
            bool bResult = false;
            clsFTPHelper oFtp = new clsFTPHelper(oMmsUpdateService);// FTP Helper Class
            string RunningFilePath = "";
            try
            {
                foreach (clsAppUpdate oApp in oColAppUpdates)
                {
                    if (oApp.SelectedForUpdate == true)
                    {
                        RunningFilePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

                        if (oApp.DownloadStatus == enmDownloadAndIntsallStatus.Latest_Version.ToString() || oApp.DownloadStatus == enmDownloadAndIntsallStatus.Download_Pending.ToString() || oApp.DownloadStatus.Trim() =="")
                        {
                            odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId, enmDownloadAndIntsallStatus.Download_Pending.ToString());

                            // create backup of actuall folder
                            //CopyFilesEx(System.IO.Path.GetDirectoryName(RunningFilePath), TempFileDestination, this.log, true);
                            try
                            {
                                string sSourceAppDirectory = RunningFilePath + @"\" + "Download" + @"\" + oApp.Product + @"\" + oApp.ApplicationName;
                                string sourceVerDirectory = RunningFilePath + @"\" + "Download" + @"\" + oApp.Product + @"\" + oApp.ApplicationName + @"\" + oApp.AppServerVersion;

                                // create backup of actuall folder
                                if (!Directory.Exists(RunningFilePath + @"\" + "Download"))
                                    Directory.CreateDirectory(RunningFilePath + @"\" + "Download" );
                                if (!Directory.Exists(RunningFilePath + @"\" + "Download" + @"\" + oApp.Product))
                                    Directory.CreateDirectory(RunningFilePath + @"\" + "Download" + @"\" + oApp.Product );
                                if (!Directory.Exists(sSourceAppDirectory))
                                    Directory.CreateDirectory(sSourceAppDirectory);
                                if (!Directory.Exists(sourceVerDirectory))
                                    Directory.CreateDirectory(sourceVerDirectory);
                                oFtp.DownLoadFolder(oApp.AppFTPFolder, sourceVerDirectory, log);
                                odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId,  enmDownloadAndIntsallStatus.Download_Complete.ToString());
                                RemoveOlderVersions(oApp.AppServerVersion, sSourceAppDirectory);
                                //odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId, enmDownloadAndIntsallStatus.Download_Complete.ToString());
                            }
                            catch (Exception exp)
                            {
                                log.AddError(String.Format("Downloading Error {0}  ", exp.Message));
                                
                                // roll bcack the copied files
                                odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId,  enmDownloadAndIntsallStatus.Download_Pending.ToString());
                            }
                        }
                    }
                }
                bResult = true;
            }
            catch (Exception ex)
            {
                bResult = false;
                throw ex;
            }
            return bResult;
        }

        private void RemoveOlderVersions(string sAppServerVersion, string sSourceAppDirectory)
        {
            DirectoryInfo oDI = new DirectoryInfo(sSourceAppDirectory);

            DirectoryInfo[] oDIArr = oDI.GetDirectories();

            foreach (DirectoryInfo oDIObj in oDIArr)
            {
                if (oDIObj.Name != sAppServerVersion)
                    Directory.Delete(oDIObj.FullName,true);
            }
        }

        // simply Install apps in application directory download folder 
        internal bool InstallApps(List<clsAppUpdate> oColAppUpdates, frmUpdate oFrmUpdate)
        {
            bool bResult = false;
            clsFTPHelper oFtp = new clsFTPHelper(oMmsUpdateService);// FTP Helper Class
            string FileSource = string.Empty;
            string CopyTempFolder = string.Empty;
            clsProcessManagment oClsProcessManagment = new clsProcessManagment();
            string RunningFilePath = "";

            try
            {
                foreach (clsAppUpdate oApp in oColAppUpdates)
                {
                    if (oApp.SelectedForUpdate == true)
                    {
                        try
                        {
                            // if the current application name to be updted is not equal to current product name then do not close the applcation 
                            // because it will be handeled by app starter application 

                            if (oApp.GetCurrVerMethod.Equals("I", StringComparison.OrdinalIgnoreCase) )
                            {
                                RunningFilePath = oAppIns.getPathByRunningInstance(oApp.ExeName,log);
                            }
                            else
                            {
                                RunningFilePath = odbMgr.getPathFromDB(oApp.ApplicationName, this.StationId, Environment.MachineName);
                            }
                            //oClsProcessManagment.StopService(oApp);// stop the process 
                            oApp.DownloadStatus = odbMgr.GetStatus(oApp.ApplicationName, this.StationId);
                            FileSource = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\" + "Download" + @"\" + oApp.Product + @"\" + oApp.ApplicationName + @"\" + oApp.AppServerVersion;//System.IO.Path.GetFileNameWithoutExtension(oApp.ExeName) + DateTime.Now.ToString("mm") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("yy") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mmss") + DateTime.Now.Minute.ToString();

                            if (oApp.DownloadStatus != enmDownloadAndIntsallStatus.Installation_Complete.ToString())
                            {
                                //if (oApp.DownloadStatus != enmDownloadAndIntsallStatus.Download_Complete.ToString() && oApp.DownloadStatus != enmDownloadAndIntsallStatus.Installation_Complete.ToString())
                                //{
                                odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId, enmDownloadAndIntsallStatus.Installation_Start.ToString());

                                if (oApp.ReplaceFilesDirectely && RunningFilePath != "")
                                {
                                    oClsProcessManagment.StopService(oApp);
                                    CopyTempFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\" + "Download" + @"\BackUp\" + oApp.Product + @"\" + System.IO.Path.GetFileNameWithoutExtension(oApp.ExeName) + DateTime.Now.ToString("mm") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("yy") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mmss") + DateTime.Now.Minute.ToString();
                                    Directory.CreateDirectory(CopyTempFolder);
                                    CopyFilesEx( System.IO.Path.GetDirectoryName(RunningFilePath),CopyTempFolder, this.log, true);
                                    try
                                    {
                                        // CopyFilesEx(System.IO.Path.GetDirectoryName(RunningFilePath), FileSource, this.log, true);
                                        CopyFilesEx(FileSource, System.IO.Path.GetDirectoryName(RunningFilePath), log, true);
                                        odbMgr.SaveAppVersion(oApp.ApplicationName, oApp.AppServerVersion, this.StationId, enmDownloadAndIntsallStatus.Installation_Complete.ToString());
                                    }
                                    catch (Exception exp)
                                    {
                                        bResult = false;
                                        log.AddError(String.Format("installation Error {0}  ", exp.Message));
                                        // roll bcack the copied files
                                        CopyFiles(CopyTempFolder, System.IO.Path.GetDirectoryName(RunningFilePath), log, true);
                                        
                                        odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId, enmDownloadAndIntsallStatus.Installation_Incomplate.ToString());
                                        return false;
                                    }
                                    finally
                                    {
                                        if (oApp.GetCurrVerMethod.Equals("I", StringComparison.OrdinalIgnoreCase) && System.IO.File.Exists(RunningFilePath))
                                            oClsProcessManagment.RunProcess(RunningFilePath, "", log, false);
                                         Directory.Delete(CopyTempFolder,true);
                                        // odbMgr.SaveAppVersion(oApp.ApplicationName, oApp.AppServerVersion, this.StationId, MachineName enmDownloadAndIntsallStatus.Installation_Complete.ToString());
                                    }
                                }
                                else
                                {
                                    if (oApp.SetupFileName.Trim() != "")
                                    {
                                        // Run Setup // wait for setup to be completed
                                        if (oApp.ApplicationName.ToLower() == this.m_AppName.ToLower())
                                        {
                                            FileSource = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\" + "Download" + @"\" + oApp.Product + @"\" + oApp.ApplicationName + @"\" + oApp.AppServerVersion;//System.IO.Path.GetFileNameWithoutExtension(oApp.ExeName) + DateTime.Now.ToString("mm") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("yy") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mmss") + DateTime.Now.Minute.ToString();

                                            odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId, enmDownloadAndIntsallStatus.Installation_Start.ToString());

                                            string Args = "\"" + FileSource + "\\" + oApp.SetupFileName + "\"" + " \"" + Application.ExecutablePath + " \"" +
                                                          " \"" + oMmsUpdateService.Url + " \"" + " \"" + oApp.ApplicationName.Trim() + " \"" + " \"" + this.m_sPharmNo.Trim() + " \"" + " \"" + oApp.AppServerVersion.Trim() + " \"" + "\"" + this.StationId;

                                            string SAppStarterPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\AppStarterPOS.exe";
                                            if (File.Exists(FileSource + "\\AppStarterPOS.exe"))
                                            {
                                                File.Copy(FileSource + "\\AppStarterPOS.exe", SAppStarterPath, true);
                                                File.Delete(FileSource + "\\AppStarterPOS.exe");
                                            }
                                            oClsProcessManagment.RunProcess(SAppStarterPath, Args, log, false);
                                            oClsProcessManagment.StopService(oApp);// stop the process 
                                        }
                                        else
                                        {
                                            oClsProcessManagment.RunProcess(FileSource + "\\" + oApp.SetupFileName, "", log, true);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            bResult = false;
                            oMmsUpdateService.InSertLog(oApp.ApplicationName, this.m_sPharmNo, oApp.AppServerVersion, DateTime.Now, true, "Filed to Update Application " + oApp.ApplicationName + " Due to following Error " + ex.Message);
                            odbMgr.SaveAppVersion(oApp.ApplicationName, "", this.StationId,  enmDownloadAndIntsallStatus.Installation_Incomplate.ToString());
                            log.AddError(String.Format("installation Error {0}  ", ex.Message));
                            return false;
                        }
                    }
                }
                bResult = true;
            }
            catch (Exception ex)
            {
                bResult = false;
                throw ex;
            }
            return bResult;
        }

        public List<clsAppUpdate> GetDownloadedUpdatesList(bool bHidden)
        {
            List<clsAppUpdate> oAppsList = new List<clsAppUpdate>();
            foreach (clsAppUpdate oApp in oColAppUpdates)
            {
                if (oApp.DownloadStatus == enmDownloadAndIntsallStatus.Download_Complete.ToString() || oApp.DownloadStatus == enmDownloadAndIntsallStatus.Installation_Incomplate.ToString() || oApp.DownloadStatus == enmDownloadAndIntsallStatus.Installation_Start.ToString())
                {
                    if (bHidden == oApp.HiddenMode)
                        oAppsList.Add(oApp);
                }
            }
            return oAppsList;
        }

        public static void CopyFiles(string source, string destination, clsLogService log, bool append)
        {
            string[] files = Directory.GetFiles(source);
            foreach (string file in files)
            {
                string destFileName = Path.Combine(destination, Path.GetFileName(file));
                try
                {
                    File.Copy(file, destFileName, true);
                    if (append)
                    {
                        log.AddMessage(String.Format("File {0} successfully copied to {1} ", Path.GetFileName(file), destination));
                    }
                }
                catch (Exception ex)
                {
                    if (append)
                    {
                        log.AddError("Failed to copy file " + file + " due to following error " + ex.Message);
                    }
                }
            }
        }

        public static void CopyFilesEx(string source, string destination, clsLogService log, bool append)
        {
            string[] files = Directory.GetFiles(source);
            foreach (string file in files)
            {
                string destFileName = Path.Combine(destination, Path.GetFileName(file));
                try
                {
                    File.Copy(file, destFileName, true);
                    if (append)
                    {
                        log.AddMessage(String.Format("File {0} successfully copied to {1} ", Path.GetFileName(file), destination));
                    }
                }
                catch (Exception ex)
                {
                    if (append)
                    {
                        log.AddError("Failed to copy file " + file + " due to following error " + ex.Message);
                    }
                    throw ex; // now thrown out the Exception 
                }
            }
        }
        
        frmNewUpdatesPopup ofrmNewUpdatesPopup = null;
        public void popupFornewUpdates(Form OParentFrm)
        {
            List<clsAppUpdate> oAppListHidden = new List<clsAppUpdate>();
            List<clsAppUpdate> oAppListNotHidden = new List<clsAppUpdate>();
            DataTable oDtAppsList = GetApplicationsListfromServer();

            oColAppUpdates = GetAppsCollection(oDtAppsList);
            oAppListNotHidden = GetDownloadedUpdatesList(false);//GetAppsCollection(oDtAppsList);
            oAppListHidden = GetDownloadedUpdatesList(true);//GetAppsCollection(oDtAppsList);

            InstallApps(oAppListHidden, null);
            try
            {
                //checkforUpdatesPopUp();

                if (oColAppUpdates.Count > 0)
                {
                    string UpdateString = "";

                    foreach (MMSAppUpdater.clsAppUpdate oApp in oAppListNotHidden)
                    {
                        if (oApp.HiddenMode) continue;
                        UpdateString += "<b> " + oApp.ApplicationName + " " + oApp.AppServerVersion;
                        UpdateString += " </b> <br />";
                        if (oApp.ReleaseNotes.Length <= 100)
                            UpdateString += oApp.ReleaseNotes + " <br />";
                        else
                            UpdateString += oApp.ReleaseNotes.Substring(0, 100) + "...More <br />";
                    }

                    if (UpdateString == "") return;
                    
                    //frmNewUpdatesPopup ofrmPopup = null;
                    //foreach (Form oFrm in OParentFrm.MdiChildren)
                    //{
                    //    if (OParentFrm.MdiChildren[0].Name == "frmRx")
                    //    {
                    //        ofrmPopup = (frmNewUpdatesPopup)OParentFrm.MdiChildren[0];
                    //        break;
                    //    }
                    //}
                    
                    if (ofrmNewUpdatesPopup == null || ofrmNewUpdatesPopup.IsDisposed)
                        ofrmNewUpdatesPopup = new frmNewUpdatesPopup(this, OParentFrm, true);
                    if (oColAppUpdates.Count > 1)
                    {
                        ofrmNewUpdatesPopup.ClientSize = new System.Drawing.Size(286, 132 + (oColAppUpdates.Count * 38));
                        ofrmNewUpdatesPopup.ultraGroupBox1.Size = new System.Drawing.Size(286, 132 + (oColAppUpdates.Count * 38));
                       // ofrmNewUpdatesPopup.txtUpdates.Location = new System.Drawing.Point(14 + (oColAppUpdates.Count * 38), 45);
                        ofrmNewUpdatesPopup.txtUpdates.Size = new System.Drawing.Size(223, 28 + (oColAppUpdates.Count * 38));
                        ofrmNewUpdatesPopup.label4.Location = new System.Drawing.Point(14, 86 + (oColAppUpdates.Count * 38));
                    }
                    ofrmNewUpdatesPopup.txtUpdates.Value = UpdateString;
                    ofrmNewUpdatesPopup.TopMost = true;
                    ofrmNewUpdatesPopup.Show(OParentFrm);
                    //if (ofrmPopup != null)
                    //    ofrmPopup.Activate();
                }
                else
                { 
                }
            }
            catch (Exception ex)
            {
                log.AddError(String.Format("New Updates Error {0}  ", ex.Message));
            }
            // loop through oColAppUpdates list 
            // if new version is available and downloaded but not installed then popup a message to user
            // (create a text for all available updates and pass it to frmNewUpdatesPopup and show it in same way as existing code)
            // if yes show installation form let user select the updates once selected then 
            // install application from download folder dont download it again
            // install it based on selected options i.e. {if run setup file is selected 
            // or if its a update for product it self (as we have a different method for updating product itself) 
            // or if it a system update or station update}
            // after updating to new version update AutoupdateAppver table in pharmSQL database 
        }

        public List<clsAppUpdate> GetStationList()
        {
            DataTable tbl = odbMgr.GetStationsList();
            clsAppUpdate oclsApplication = null;
            List<clsAppUpdate> AutoUpdateStationList = new List<clsAppUpdate>();
            try
            {
                foreach (DataRow Dr in tbl.Rows)
                {
                    oclsApplication = new clsAppUpdate();
                    oclsApplication.ApplicationName = Dr["AppName"].ToString().Trim();
                    oclsApplication.StationdId = Dr["STATIONID"].ToString();
                    oclsApplication.UpdateType = Dr["UPDATETYPE"].ToString();
                    oclsApplication.RunningFilePath = (Dr["Path"].Equals(null) ? "" : Dr["Path"].ToString().Trim());
                    oclsApplication.MachineName = (Dr["MachineName"].Equals(null) ? "" : Dr["MachineName"].ToString().Trim());
                    oclsApplication.LastRecordedRunning = (Dr["LastRecordedRunning"].Equals(null) ? "" : Dr["LastRecordedRunning"].ToString().Trim());
                    AutoUpdateStationList.Add(oclsApplication);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AutoUpdateStationList;
        }

        public List<string> GetApplicationList()
        {
            DataTable tbl = odbMgr.GetAppList();
            List<string> AppList = new List<string>();
            try
            {
                AppList.Add(string.Empty);
                foreach (DataRow Dr in tbl.Rows)
                {
                    AppList.Add(Dr["AppName"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AppList;
        }

        public List<clsAppUpdate> GetAppsCollection(DataTable oDtAppsList)
        {
            return GetAppsCollection(oDtAppsList, false);
        }

        private bool CheckFolderExist(clsAppUpdate oApp)
        {
            try
            {
                string RunningFilePath = "";
                RunningFilePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

                string sourceVerDirectory = RunningFilePath + @"\" + "Download" + @"\" + oApp.Product + @"\" + oApp.ApplicationName + @"\" + oApp.AppServerVersion;
                if (Directory.Exists(sourceVerDirectory))
                    return true;
                else
                    return false;
            }
            catch 
            {
                return false;
            }
        }
        
        // Create clsAppUpdate collection out of dataset
        public List<clsAppUpdate> GetAppsCollection(DataTable oDtAppsList,bool bgetAll)
        {
            List<clsAppUpdate> oApps = new List<clsAppUpdate>();
            clsAppUpdate oClsAppUpdate = null;
            oColAppUpdates.Clear();
            foreach (DataRow Dr in oDtAppsList.Rows)
            {
                oClsAppUpdate = new clsAppUpdate();
                //oClsAppUpdate.Productid=Convert.ToInt32(Dr["ProductId"].ToString());
                oClsAppUpdate.Product = Dr["Product"].ToString().Trim();
                oClsAppUpdate.ApplicationName = Dr["AppName"].ToString().Trim();
                oClsAppUpdate.AppServerVersion = Dr["AppServerVersion"].ToString();
                oClsAppUpdate.AppFTPFolder = Dr["AppFtpFolder"].ToString();

                oClsAppUpdate.ReleaseDate = Convert.ToDateTime(Dr["ReleaseDate"].ToString());
                oClsAppUpdate.ReleaseNotes = Dr["ReleaseNotes"].ToString();
                oClsAppUpdate.SetupFileName = Dr["SetupFileName"].ToString();
                oClsAppUpdate.CommLineArguments = Dr["CommandArguments"].ToString();
                oClsAppUpdate.StationdId = this.StationId;
                // possible values are "I"= check running Instance, 
                //"D"=get current Version From Db"
                oClsAppUpdate.GetCurrVerMethod = Dr["GetCurrVerMethod"].ToString();
                oClsAppUpdate.ExeName = Dr["ExeName"].ToString();
                oClsAppUpdate.AlwaysRunning = Convert.ToBoolean(Dr["AlwaysRunning"].ToString());
                oClsAppUpdate.ReplaceFilesDirectely = Convert.ToBoolean(Dr["ReplaceFileDirectly"].ToString());
                oClsAppUpdate.UpdateType = Dr["UPDATETYPE"].ToString();// set the update Type ST (""station") S("System")
                oClsAppUpdate.CurrentVersion = getCurrentAppVersion(oClsAppUpdate); // get the current Verison of EXE
                //oClsAppUpdate.Dbver = odbMgr.GetDbVer(oClsAppUpdate.ApplicationName, StationId);
                oClsAppUpdate.DownloadStatus = odbMgr.GetStatus(oClsAppUpdate.ApplicationName, StationId);
                if (Dr.Table.Columns.Contains("LocalMultiInstanceAllowed"))
                    oClsAppUpdate.LocalMultiInstanceAllowed = Convert.ToBoolean(Dr["LocalMultiInstanceAllowed"].ToString());
                if (Dr.Table.Columns.Contains("NetworkMultiinstanceAllowed"))
                    oClsAppUpdate.NetworkMultiinstanceAllowed = Convert.ToBoolean(Dr["NetworkMultiinstanceAllowed"].ToString());
                if (Dr.Table.Columns.Contains("HiddenMode"))
                    oClsAppUpdate.HiddenMode = Convert.ToBoolean(Dr["HiddenMode"].ToString());

                if (Dr.Table.Columns.Contains("UrgentDownload"))
                    oClsAppUpdate.UrgentDownload = Convert.ToBoolean(Dr["UrgentDownload"].ToString());
                if (!bgetAll)
                {
                    if (oClsAppUpdate.CurrentVersion != "" && CompareVersion(oClsAppUpdate.AppServerVersion, oClsAppUpdate.CurrentVersion) && !(oClsAppUpdate.UpdateType.ToUpper().Trim() == "ST" && oClsAppUpdate.StationdId.Trim() != "01"))
                    {
                        if ((CheckFolderExist(oClsAppUpdate) == false) || (enmDownloadAndIntsallStatus.Installation_Complete.ToString() == oClsAppUpdate.DownloadStatus) || (enmDownloadAndIntsallStatus.Download_Pending.ToString() == oClsAppUpdate.DownloadStatus) || (oClsAppUpdate.DownloadStatus == string.Empty))
                            {
                                odbMgr.SaveAppVersion(oClsAppUpdate.ApplicationName, "",this.StationId, enmDownloadAndIntsallStatus.Latest_Version.ToString());
                            }
                            oClsAppUpdate.DownloadStatus = odbMgr.GetStatus(oClsAppUpdate.ApplicationName, StationId).Replace(" ", "_");
                            oApps.Add(oClsAppUpdate);
                    }
                }
                else
                    oApps.Add(oClsAppUpdate);
            }
            return oApps;
        }

        private bool CompareVersion(string FileVersion, string ComparingVersion)
        {
            string[] ArrayFileVersion = FileVersion.Split('.');
            string[] ArrayComparingVersion = ComparingVersion.Split('.');
            if (ArrayComparingVersion.Length < 4)
            {
                string[] TempArray = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        TempArray[i] = ArrayComparingVersion[i];
                    }
                    catch
                    {
                        TempArray[i] = "0";
                    }
                }
                ArrayComparingVersion = TempArray;
            }

            if (ArrayComparingVersion.Length == 4)
            {
                string str = "0000000000";
                string[] TempArray = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (ArrayComparingVersion[i].Length > 1)
                            TempArray[i] = ((Convert.ToDecimal(ArrayComparingVersion[i])) / (Convert.ToDecimal("1" + str.Substring(0, (ArrayComparingVersion[i].Length) - 1)))).ToString();
                        else
                            TempArray[i] = ArrayComparingVersion[i];
                    }
                    catch
                    {
                        TempArray[i] = "0";
                    }
                }
                ArrayComparingVersion = TempArray;
            }
            if (ArrayFileVersion.Length == 4)
            {
                string str = "0000000000";
                string[] TempArray = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        if (ArrayFileVersion[i].Length > 1)
                            TempArray[i] = ((Convert.ToDecimal(ArrayFileVersion[i])) / (Convert.ToDecimal("1" + str.Substring(0, (ArrayFileVersion[i].Length) - 1)))).ToString();
                        else
                            TempArray[i] = ArrayFileVersion[i];
                    }
                    catch
                    {
                        TempArray[i] = "0";
                    }
                }
                ArrayFileVersion = TempArray;
            }

            try
            {
                if ((ArrayFileVersion[0] != null && ArrayComparingVersion[0] != null) && (Convert.ToDecimal(ArrayFileVersion[0]) > Convert.ToDecimal(ArrayComparingVersion[0])))
                {
                    return true;
                }
                else if ((ArrayFileVersion[0] != null && ArrayComparingVersion[0] != null) && (Convert.ToDecimal(ArrayFileVersion[0]) < Convert.ToDecimal(ArrayComparingVersion[0])))
                    return false;
                if ((ArrayFileVersion[1] != null && ArrayComparingVersion[1] != null) && (Convert.ToDecimal(ArrayFileVersion[1]) > Convert.ToDecimal(ArrayComparingVersion[1])))
                {
                    return true;
                }
                else if ((ArrayFileVersion[1] != null && ArrayComparingVersion[1] != null) && (Convert.ToDecimal(ArrayFileVersion[1]) < Convert.ToDecimal(ArrayComparingVersion[1])))
                    return false;
                if ((ArrayFileVersion[2] != null && ArrayComparingVersion[2] != null) && (Convert.ToDecimal(ArrayFileVersion[2]) > Convert.ToDecimal(ArrayComparingVersion[2])))
                {
                    return true;
                }
                else if ((ArrayFileVersion[2] != null && ArrayComparingVersion[2] != null) && (Convert.ToDecimal(ArrayFileVersion[2]) < Convert.ToDecimal(ArrayComparingVersion[2])))
                    return false;
                if ((ArrayFileVersion[3] != null && ArrayComparingVersion[3] != null) && (Convert.ToDecimal(ArrayFileVersion[3]) > Convert.ToDecimal(ArrayComparingVersion[3])))
                {
                    return true;
                }
                else if ((ArrayFileVersion[3] != null && ArrayComparingVersion[3] != null) && (Convert.ToDecimal(ArrayFileVersion[3]) < Convert.ToDecimal(ArrayComparingVersion[3])))
                    return false;
            }
            catch
            {
                return false;
            }
            return false;
        }

        // Get Current Version 
        private string getCurrentAppVersion(clsAppUpdate oAppUpd)
        {
            string sAppPath = string.Empty;
            string sExeVersion = "0.0.0.0";

            if (oAppUpd.UpdateType != "") // with out setting the update type(ST OR S) no installation will occured
            {
                if (oAppUpd.GetCurrVerMethod.Equals("I", StringComparison.OrdinalIgnoreCase))
                {
                    // if product Name is same as application name, it means this is update for product it self
                    // in this case get current running application path
                    if (oAppUpd.ApplicationName == oAppUpd.Product)//
                    {
                        #region 02-Sep-2015 JY Added code as few pharmacies are facing issue to return correct POS.exe directory path
                        //sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "") + @"\" + oAppUpd.Product + ".exe";
                        try
                        {
                            sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "") + @"\" + oAppUpd.Product + ".exe";
                            TempLog.AddMessage("getCurrentAppVersion - System.Reflection.Assembly.GetExecutingAssembly().CodeBase:" + sAppPath);
                            if (sAppPath != null && sAppPath.Trim() != "")
                                sExeVersion = FileVersionInfo.GetVersionInfo(sAppPath).FileVersion;
                            else
                                sExeVersion = "";
                        }
                        catch
                        {
                            try
                            {
                                sAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(oAppUpd.GetType()).Location) + @"\" + oAppUpd.Product + ".exe";
                                TempLog.AddMessage("getCurrentAppVersion - System.Reflection.Assembly.GetAssembly(oAppUpd.GetType()).Location:" + sAppPath);
                                if (sAppPath != null && sAppPath.Trim() != "")
                                    sExeVersion = FileVersionInfo.GetVersionInfo(sAppPath).FileVersion;
                                else
                                    sExeVersion = "";
                            }
                            catch
                            {
                                sAppPath = AppDomain.CurrentDomain.BaseDirectory + oAppUpd.Product + ".exe";
                                TempLog.AddMessage("getCurrentAppVersion - AppDomain.CurrentDomain.BaseDirectory:" + sAppPath);
                                if (sAppPath != null && sAppPath.Trim() != "")
                                    sExeVersion = FileVersionInfo.GetVersionInfo(sAppPath).FileVersion;
                                else
                                    sExeVersion = "";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        sAppPath = oAppIns.getPathByRunningInstance(oAppUpd.ExeName, log);
                        TempLog.AddMessage("getCurrentAppVersion - oAppIns.getPathByRunningInstance(oAppUpd.ExeName, log):" + sAppPath);
                        if (sAppPath != null && sAppPath.Trim() != "")
                            sExeVersion = FileVersionInfo.GetVersionInfo(sAppPath).FileVersion;
                        else
                            sExeVersion = "";
                    }
                }
                else
                {
                    sExeVersion = odbMgr.GetLastVersion(oAppUpd.ApplicationName, this.StationId, oAppUpd.UpdateType);
                }
            }
            return sExeVersion;
        }

        // compare versions with current apps and create collection of apps where new version
        //is available at server and marked as urgent
        private List<clsAppUpdate> getListOfDownloadablesUrgent(List<clsAppUpdate> oAppsList, bool bUrgent)
        {
            List<clsAppUpdate> oApps = new List<clsAppUpdate>();
            foreach (clsAppUpdate oApp in oAppsList)
            {
                if (oApp.UrgentDownload == bUrgent)
                {
                    oApps.Add(oApp);
                }
            }
            return oApps;
        }

        private void ScheduleDownload(List<clsAppUpdate> oColAppUpdates)
        {
        }

        public bool UpdateSystemStation(string PrvStationId, string StationId, string sAppName)
        {
            return odbMgr.UpdateSystemStation(PrvStationId, StationId, sAppName);
        }

        public bool DeleteStation(string PrvStationId, string StationId, string sAppName)
        {
            return odbMgr.DeleteStation(PrvStationId, StationId, sAppName);
        }

        public void ShowStationSettings(Form fParetnFrom)
        {
            try
            {
                log = new clsLogService("StationSettings.log");
                frmStationsSettings oSatationSetting = new frmStationsSettings(this);
                oSatationSetting.ShowDialog(fParetnFrom);
                this.log.Close();
            }
            catch (Exception ex)
            {
                log.AddError(string.Format("ERROR:{0} \n Stack Trace :{1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                this.log.Close();
            }
        }

        //Sprint-20 27-May-2015 JY not in use
        public bool CheckStationIdAvailability()
        {
            bool bFound = false;
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                DataTable odtLocaldb = odbMgr.GetMachinesRunningPOSOnStation(this.StationId);
                foreach (DataRow dr in odtLocaldb.Rows)
                {
                    if (dr["Path"].ToString().Trim().EndsWith("\\bin\\Debug\\POS.exe") || Environment.MachineName == dr["MachineName"].ToString().Trim())
                    {
                        continue;
                    }
                    else
                    {
                        // find list of first 6 available station ids
                        DataTable dt = odbMgr.GetAvailableStationsId();
                        string stationids = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            stationids += row["StationId"].ToString() + ", ";
                        }
                        stationids = stationids.Trim(' ', ',');
                        string msg = "StationID " + dr["StationId"].ToString().Trim() + " is already being used by " + dr["MachineName"].ToString().Trim() + ".\n PrimePOS will close now.\n To solve this please select one of the available ids(" + stationids + ").";
                        MessageBox.Show(msg, "Duplicate Station ID Detected", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        bFound = true;
                        break;
                    }
                }
            }
            return bFound;
        }

        // implemented by adeel shehzad Aug 26, 2012
        public void checkAlwaysRunningandMultiInstancing(bool isRemoteOrCitrix, string clientMachineName)
        {
            //IntPtr ptr = IntPtr.Zero;
            //Wow64DisableWow64FsRedirection(ref ptr);

            // get list of all applications from Server
           DataSet TempDs = oMmsUpdateService.GetProductList(this.m_AppName).Copy();
            // Get clsAppUpdate collections from above dataset
            List<clsAppUpdate> oColApps = GetAppsCollection(TempDs.Tables[0],true);
            
            // get running processes on this machine 
            DataTable tblRunnigTasks = GetRunningTasks();
            DataTable odtLocaldb = odbMgr.GetAutoUpdateAppVerTableData();

            clsProcessManagment oClsProcessManagment = new clsProcessManagment();

            // loop through applications collection loaded from server
            foreach (clsAppUpdate oApp in oColApps)
            {
                try
                {
                    bool bRunningOnThisMachine = false;
                    bool bEntryFoundForthisAppInDb = false;
                    // calculate if running on this machine
                    string sAppName = oApp.ExeName.Replace(".exe", "");
                    DataView oDvRunningTask = new DataView(tblRunnigTasks);
                    oDvRunningTask.RowFilter = " AppName = '" + sAppName + "'";

                    DataView oDVLocalDb = new DataView(odtLocaldb);
                    oDVLocalDb.RowFilter = " AppName = '" + sAppName + "' AND stationId = '" + oApp.StationdId + "' AND MachineName ='" + Environment.MachineName + "' ";

                    if (oDVLocalDb.Count > 0)
                        bEntryFoundForthisAppInDb = true;

                    if (oDvRunningTask.Count > 0)
                        bRunningOnThisMachine = true;

                    oApp.ExeName = odbMgr.getPathFromDB(oApp.ApplicationName, this.StationId, Environment.MachineName);

                    Process[] p = Process.GetProcessesByName(sAppName);
                    clsAppInstance oclsAppInstance = new clsAppInstance();
                   
                    if (p != null && p.Length > 0)
                    {
                        //oApp.ExeName = p[0].MainModule.FileName;
                        oApp.ExeName = oclsAppInstance.getPathByRunningInstance(sAppName, log);
                   }

                    if (oApp.Product.Trim().Equals(oApp.ApplicationName.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        odbMgr.SaveAppVersion(oApp.ApplicationName, oApp.CurrentVersion, this.StationId, oApp.ExeName, oApp.AlwaysRunning, oApp.LocalMultiInstanceAllowed, oApp.NetworkMultiinstanceAllowed, true, true, isRemoteOrCitrix, clientMachineName);
                        continue;
                    }

                    bool runningOnAnyOtherMachine = odbMgr.RunningOnAnyOtherMachine(sAppName);
                    // if running on this machine
                    if (bRunningOnThisMachine)
                    {
                        // found running on any other station and AllowmultiinstanceOnNetwork=false
                        if (oApp.NetworkMultiinstanceAllowed == false)
                        {
                            if (runningOnAnyOtherMachine)
                            {
                                // stop running exe and show error message to user
                                if (p != null && p.Length > 0)
                                {
                                    p[0].Kill();
                                }
                                MessageBox.Show("There is a problem detected with " + sAppName + " Multiple Instance running on the System. " + Environment.NewLine + "Please Call Micro Merchant immediately for troubleshooting.");
                            }
                            else
                                odbMgr.SaveAppVersion(oApp.ApplicationName, oApp.CurrentVersion, this.StationId, oApp.ExeName, oApp.AlwaysRunning, oApp.LocalMultiInstanceAllowed, oApp.NetworkMultiinstanceAllowed, true, true, isRemoteOrCitrix, clientMachineName);
                        }
                        // else if allow multistation is true 
                        else
                        {
                            // Update AutoupdateAppVer table
                            odbMgr.SaveAppVersion(oApp.ApplicationName, oApp.CurrentVersion, this.StationId, oApp.ExeName, oApp.AlwaysRunning, oApp.LocalMultiInstanceAllowed, oApp.NetworkMultiinstanceAllowed, true, true, isRemoteOrCitrix, clientMachineName);
                        }
                    }
                    // if not running on this machine
                    else if (bEntryFoundForthisAppInDb)
                    {
                        // found running on any other machine 
                        if (runningOnAnyOtherMachine)
                        {
                            if (oApp.NetworkMultiinstanceAllowed == false)
                            {
                                // Give Error message to user and dont update db and dont run Application
                                if (p != null && p.Length > 0)
                                {
                                    p[0].Kill();
                                }

                                MessageBox.Show("There is a problem detected with " + oApp.ExeName + " Multiple Instance running on the System. " + Environment.NewLine + "Please Call Micro Merchant immediately for troubleshooting.");
                            }
                            else
                            {
                                // Start the application and update AutoUpdateAppVer table in pharmaSQLDb
                                oClsProcessManagment.RunProcess(oApp.ExeName, "", log, false, oApp.ApplicationName);
                                odbMgr.SaveAppVersion(oApp.ApplicationName, oApp.CurrentVersion, this.StationId, oApp.ExeName, oApp.AlwaysRunning, oApp.LocalMultiInstanceAllowed, oApp.NetworkMultiinstanceAllowed, true, true, isRemoteOrCitrix, clientMachineName);
                            }
                        }
                        // not found on any other station
                        else
                        {
                            // run the exe and update AutoUpdateAppVer table in pharmaSQLDb
                            if (oApp.AlwaysRunning) oClsProcessManagment.RunProcess(oApp.ExeName, "", log, false, oApp.ApplicationName);
                            odbMgr.SaveAppVersion(oApp.ApplicationName, oApp.CurrentVersion, this.StationId, oApp.ExeName, oApp.AlwaysRunning, oApp.LocalMultiInstanceAllowed, oApp.NetworkMultiinstanceAllowed, true, true, isRemoteOrCitrix, clientMachineName);
                        }
                    }

                    // AllowMultiInstanceOnMachine true
                    if (!oApp.LocalMultiInstanceAllowed)
                    {
                        // if Multiinstance for this application is found leave one instance running and remove all others
                        KillAdditionalInstances(sAppName);
                    }
                }
                catch (Exception exp)
                {
                    log.AddError(String.Format("Always Running Product Error {0}  ", exp.Message));
                }
            }

            //Wow64RevertWow64FsRedirection(ptr);
            //RunAlwaysRunningProducts();
            //UpdateAppUpdateVersionForRunningtask();
            //CheckForMultipleInstances();
        }

        public bool RunApplication(string AppName)
        {
            DataTable dt = odbMgr.GetAppRecordForMachine(AppName, Environment.MachineName);
            if (dt != null && dt.Rows.Count > 0)
            {
                clsProcessManagment oClsProcessManagment = new clsProcessManagment();
                if (!oClsProcessManagment.IsAppRunning(AppName))
                {
                    oClsProcessManagment.RunProcess(dt.Rows[0]["Path"].ToString(), "", log, false, AppName);
                    return true;
                }
            }
            return false;
        }

        public void RunAlwaysRunningProducts()
        {
            List<clsAppUpdate> oAppList = CheckforAlwaysRunningProducts();
            log = new clsLogService("RunningProducts" + DateTime.Now.ToString("MM DD YY ") + ".log");
            clsProcessManagment oClsProcessManagment = new clsProcessManagment();
            foreach (clsAppUpdate oclsApp in oAppList)
            {
                if (oclsApp.ExeName.Trim() != "" && oclsApp.AlwaysRunning == true)
                {
                    Process[] pErx = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(oclsApp.ExeName.Trim()));
                    if (pErx == null || pErx.Length == 0)
                    {
                        oClsProcessManagment.RunProcess(oclsApp.ExeName.Trim(), "", log, false,oclsApp.ApplicationName);
                    }
                }
            }
        }

        public List<clsAppUpdate> CheckforAlwaysRunningProducts()
        {
            try
            {
                DataSet TempDs = oMmsUpdateService.GetAlwaysRunningProductList(this.m_AppName).Copy();
                clsAppUpdate oClsAppUpdate = null;
                oColAppUpdates.Clear();
                DataTable tblAlwayRunningProductList = odbMgr.GetInstalledAlwaysRunningProductList(this.StationId);
                DataRow[] SearchedRows = null;
                foreach (DataRow Dr in TempDs.Tables[0].Rows)
                {
                    SearchedRows = tblAlwayRunningProductList.Select("AppName='" + Dr["AppName"].ToString().Trim() + "'");

                    if (SearchedRows != null && SearchedRows.Length > 0)
                    {
                        oClsAppUpdate = new clsAppUpdate();
                        //oClsAppUpdate.Productid=Convert.ToInt32(Dr["ProductId"].ToString());
                        oClsAppUpdate.Product = Dr["Product"].ToString().Trim();
                        oClsAppUpdate.ApplicationName = Dr["AppName"].ToString().Trim();
                        oClsAppUpdate.AppServerVersion = Dr["AppServerVersion"].ToString();
                        oClsAppUpdate.AppFTPFolder = Dr["AppFtpFolder"].ToString();
                        oClsAppUpdate.ReleaseDate = Convert.ToDateTime(Dr["ReleaseDate"].ToString());
                        oClsAppUpdate.ReleaseNotes = Dr["ReleaseNotes"].ToString();
                        oClsAppUpdate.SetupFileName = Dr["SetupFileName"].ToString();
                        oClsAppUpdate.CommLineArguments = Dr["CommandArguments"].ToString();

                        // possible values are "I"= check running Instance, 
                        //"D"=get current Version From Db"
                        oClsAppUpdate.GetCurrVerMethod = Dr["GetCurrVerMethod"].ToString();
                        oClsAppUpdate.ExeName = SearchedRows[0]["Path"].ToString();//Dr["ExeName"].ToString();
                        oClsAppUpdate.AlwaysRunning = Convert.ToBoolean(Dr["AlwaysRunning"].ToString());
                        oClsAppUpdate.UpdateType = Dr["UPDATETYPE"].ToString();// set the update Type ST (""station") S("System")
                        oClsAppUpdate.ReplaceFilesDirectely = Convert.ToBoolean(Dr["ReplaceFileDirectly"].ToString());
                        oClsAppUpdate.CurrentVersion = getCurrentAppVersion(oClsAppUpdate); // get the current Verison of EXE
                        if (Dr.Table.Columns.Contains("LocalMultiInstanceAllowed"))
                            oClsAppUpdate.LocalMultiInstanceAllowed = Convert.ToBoolean(Dr["LocalMultiInstanceAllowed"].ToString());
                        if (Dr.Table.Columns.Contains("NetworkMultiinstanceAllowed"))
                            oClsAppUpdate.NetworkMultiinstanceAllowed = Convert.ToBoolean(Dr["NetworkMultiinstanceAllowed"].ToString());
                        if (CheckNetWorkMultiInstance(oClsAppUpdate.ApplicationName, oClsAppUpdate.NetworkMultiinstanceAllowed,false) == true)
                        {
                            oColAppUpdates.Add(oClsAppUpdate);
                        }
                    }
                }
            }
            catch (Exception ex)
            { log.AddError(String.Format("Always Running Product Error {0}  ", ex.Message)); }

            return oColAppUpdates;
        }

        private void KillAdditionalInstances(string ProcessName)
        {
            int Numberofinstances = getTotalRunningInstances(ProcessName);
            if (Numberofinstances > 1)
            {
                MessageBox.Show("There is a problem detected with " + ProcessName + " Multiple Instance running on the System. " + Environment.NewLine + "Please Call Micro Merchant immediately for troubleshooting.");
                for (int i = Numberofinstances; i > 1; i--)
                {
                    Process[] p = Process.GetProcessesByName(ProcessName);
                    if (p != null && p.Length > 1)
                    {
                        p[0].Kill();
                    }
                    Numberofinstances = getTotalRunningInstances(ProcessName);
                }
            }

        }

        public void CheckForMultipleInstances()
        {
            DataTable tblAlwayRunningProductList = odbMgr.GetInstalledAlwaysRunningProductList(this.StationId);
            foreach (DataRow DAppRow in tblAlwayRunningProductList.Rows)
            {
                string ProcessName = DAppRow["AppName"].ToString().Trim();
                if (Convert.ToBoolean(DAppRow["LocalMultiInstanceAllowed"]) == false)
                {
                    int Numberofinstances = getTotalRunningInstances(ProcessName);
                    if (Numberofinstances > 1)
                    {
                        MessageBox.Show("There is a problem detected with " + ProcessName + " Multiple Instance running on the System. " + Environment.NewLine + "Please Call Micro Merchant immediately for troubleshooting.");
                        for (int i = Numberofinstances; i > 1; i--)
                        {
                            Process[] p = Process.GetProcessesByName(ProcessName);
                            if (p != null && p.Length > 0)
                            {
                                p[0].Kill();
                            }
                        }
                    }
                }
                CheckNetWorkMultiInstance(ProcessName, Convert.ToBoolean(DAppRow["NetworkMultiinstanceAllowed"].ToString()),true);
            }
        }

        private bool CheckNetWorkMultiInstance(string appName, bool IsNetworkMultiInstanceAllowed,bool showmsg)
        {
            try
            {
                if (IsNetworkMultiInstanceAllowed == false)
                {
                    string ProcessName = appName.Trim();
                    DataTable dtLastReporttime = odbMgr.GetLastestRecordedRunningOnDifferentMachine(ProcessName, this.StationId);
                    foreach (DataRow drow in dtLastReporttime.Rows)
                    {
                        TimeSpan duration = DateTime.Now - Convert.ToDateTime(drow["LastRecordedRunning"]);
                        int Numberofinstances = getTotalRunningInstances(ProcessName);
                        if (duration.TotalHours < 1  )
                        {
                            if (showmsg)
                            {
                                MessageBox.Show("There is a problem detected with " + ProcessName + " Multiple Instance running on the Network. " + Environment.NewLine + "Please Call Micro Merchant immediately for troubleshooting.");
                            }
                            if (Numberofinstances > 0)
                            {
                                Process[] processes = Process.GetProcessesByName(ProcessName);
                                foreach (Process proc in processes)
                                {
                                    proc.Kill();
                                }
                            }
                            return false;
                            //MessageBox.Show("There is a problem detected with " + ProcessName + ". " + Environment.NewLine + "Please Call Micro Merchant immediately for troubleshooting.");
                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        private int getTotalRunningInstances(string fileName)
        {
            int count = 0;
            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    try
                    {
                        if (p.ProcessName.ToLower().Trim() == fileName.ToLower().Trim())
                        {
                            count++;
                        }
                    }
                    catch { }
                }
            }
            catch
            {
            }
            return count;
        }

        private DataTable GetRunningTasks()
        {
            try
            {
                //Environment.Is64BitOperatingSystem
                
                Process[] RunningProcess = Process.GetProcesses();
                DataTable dtProcess = new DataTable();
                dtProcess.Columns.Add("Pid");
                dtProcess.Columns.Add("AppName");
                dtProcess.Columns.Add("Path");
                clsAppInstance apins = new clsAppInstance();
                foreach (Process EachProcess in RunningProcess)
                {
                    DataRow drow = dtProcess.NewRow();
                    drow["Pid"] = EachProcess.Id;
                    drow["AppName"] = EachProcess.ProcessName;
                    try
                    {
                        drow["Path"] = apins.getPathByRunningInstance(EachProcess.ProcessName,log);
                        dtProcess.Rows.Add(drow);
                    }
                    catch (Exception ex)
                    {
                        log.AddError(String.Format("Running Task Error {0}  ", ex.Message));
                    }
                }
                return dtProcess;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void UpdateAppUpdateVersionForRunningtask()
        {
            try
            {
                DataSet TempDs = oMmsUpdateService.GetProductList(this.m_AppName).Copy();

                clsAppUpdate oClsAppUpdate = null;
                oColAppUpdates.Clear();
                DataTable tblAlwayRunningProductList = GetRunningTasks();

                DataRow[] SearchedRows = null;
                foreach (DataRow Dr in TempDs.Tables[0].Rows)
                {
                    SearchedRows = tblAlwayRunningProductList.Select("AppName='" + Dr["AppName"].ToString().Trim() + "'");

                    if (SearchedRows != null && SearchedRows.Length > 0)
                    {
                        oClsAppUpdate = new clsAppUpdate();
                        //oClsAppUpdate.Productid=Convert.ToInt32(Dr["ProductId"].ToString());
                        oClsAppUpdate.Product = Dr["Product"].ToString().Trim();
                        oClsAppUpdate.ApplicationName = Dr["AppName"].ToString().Trim();
                        oClsAppUpdate.AppServerVersion = Dr["AppServerVersion"].ToString();
                        oClsAppUpdate.AppFTPFolder = Dr["AppFtpFolder"].ToString();
                        oClsAppUpdate.ReleaseDate = Convert.ToDateTime(Dr["ReleaseDate"].ToString());
                        oClsAppUpdate.ReleaseNotes = Dr["ReleaseNotes"].ToString();
                        oClsAppUpdate.SetupFileName = Dr["SetupFileName"].ToString();
                        oClsAppUpdate.CommLineArguments = Dr["CommandArguments"].ToString();

                        // possible values are "I"= check running Instance, 
                        //"D"=get current Version From Db"
                        oClsAppUpdate.GetCurrVerMethod = "I";//Dr["GetCurrVerMethod"].ToString();
                        oClsAppUpdate.ExeName = SearchedRows[0]["Path"].ToString();//Dr["ExeName"].ToString();
                        oClsAppUpdate.AlwaysRunning = Convert.ToBoolean(Dr["AlwaysRunning"].ToString());
                        oClsAppUpdate.UpdateType = Dr["UPDATETYPE"].ToString();// set the update Type ST (""station") S("System")
                        oClsAppUpdate.ReplaceFilesDirectely = Convert.ToBoolean(Dr["ReplaceFileDirectly"].ToString());
                        oClsAppUpdate.CurrentVersion = getCurrentAppVersion(oClsAppUpdate); // get the current Verison of EXE
                        if (Dr.Table.Columns.Contains("LocalMultiInstanceAllowed"))
                            oClsAppUpdate.LocalMultiInstanceAllowed = Convert.ToBoolean(Dr["LocalMultiInstanceAllowed"].ToString());
                        if (Dr.Table.Columns.Contains("NetworkMultiinstanceAllowed"))
                            oClsAppUpdate.NetworkMultiinstanceAllowed = Convert.ToBoolean(Dr["NetworkMultiinstanceAllowed"].ToString());
                        //oColAppUpdates.Add(oClsAppUpdate);
                        
                        if (CheckNetWorkMultiInstance(oClsAppUpdate.ApplicationName, oClsAppUpdate.NetworkMultiinstanceAllowed,false) == true)
                        {
                            odbMgr.SaveAppVersion(oClsAppUpdate.ApplicationName, oClsAppUpdate.CurrentVersion, this.StationId, oClsAppUpdate.ExeName, oClsAppUpdate.AlwaysRunning, oClsAppUpdate.LocalMultiInstanceAllowed, oClsAppUpdate.NetworkMultiinstanceAllowed, true, false);
                        }
                    }
                }
                //CheckForMultipleInstances();
            }
            catch (Exception ex)
            { 
                log.AddError(String.Format("Update App Version For Running Task Error {0}  ", ex.Message)); 
            }
            // return oColAppUpdates;
        }

        private DataTable GetApplicationsListfromServer()
        {
            DataTable oDt = new DataTable();
            oDt = oMmsUpdateService.GetProductList(this.m_AppName).Tables[0].Copy();
            return oDt;
        }

        public List<clsAppUpdate> CheckforUpdates()
        {
            DataSet TempDs = oMmsUpdateService.GetProductList(this.m_AppName).Copy();
            clsAppUpdate oClsAppUpdate = null;
            oColAppUpdates.Clear();
            foreach (DataRow Dr in TempDs.Tables[0].Rows)
            {
                oClsAppUpdate = new clsAppUpdate();
                //oClsAppUpdate.Productid=Convert.ToInt32(Dr["ProductId"].ToString());
                oClsAppUpdate.Product = Dr["Product"].ToString().Trim();
                oClsAppUpdate.ApplicationName = Dr["AppName"].ToString().Trim();
                oClsAppUpdate.AppServerVersion = Dr["AppServerVersion"].ToString();
                oClsAppUpdate.AppFTPFolder = Dr["AppFtpFolder"].ToString();
                oClsAppUpdate.ReleaseDate = Convert.ToDateTime(Dr["ReleaseDate"].ToString());
                oClsAppUpdate.ReleaseNotes = Dr["ReleaseNotes"].ToString();
                oClsAppUpdate.SetupFileName = Dr["SetupFileName"].ToString();
                oClsAppUpdate.CommLineArguments = Dr["CommandArguments"].ToString();

                // possible values are "I"= check running Instance, 
                //"D"=get current Version From Db"
                oClsAppUpdate.GetCurrVerMethod = Dr["GetCurrVerMethod"].ToString();
                oClsAppUpdate.ExeName = Dr["ExeName"].ToString();
                oClsAppUpdate.AlwaysRunning = Convert.ToBoolean(Dr["AlwaysRunning"].ToString());
                oClsAppUpdate.ReplaceFilesDirectely = Convert.ToBoolean(Dr["ReplaceFileDirectly"].ToString());
                oClsAppUpdate.UpdateType = Dr["UPDATETYPE"].ToString();// set the update Type ST (""station") S("System")
                oClsAppUpdate.CurrentVersion = getCurrentAppVersion(oClsAppUpdate); // get the current Verison of EXE
                oClsAppUpdate.DownloadStatus = odbMgr.GetStatus(oClsAppUpdate.ApplicationName, StationId);
                if (Dr.Table.Columns.Contains("LocalMultiInstanceAllowed"))
                    oClsAppUpdate.LocalMultiInstanceAllowed = Convert.ToBoolean(Dr["LocalMultiInstanceAllowed"].ToString());
                if (Dr.Table.Columns.Contains("NetworkMultiinstanceAllowed"))
                    oClsAppUpdate.NetworkMultiinstanceAllowed = Convert.ToBoolean(Dr["NetworkMultiinstanceAllowed"].ToString());
               // if (oClsAppUpdate.UpdateType == "ST" ) 
                    
                if (oClsAppUpdate.CurrentVersion != "" && CompareVersion(oClsAppUpdate.AppServerVersion, oClsAppUpdate.CurrentVersion))
                {
                    if ((enmDownloadAndIntsallStatus.Installation_Complete.ToString() == oClsAppUpdate.DownloadStatus) || (oClsAppUpdate.DownloadStatus == string.Empty))
                    {
                        odbMgr.SaveAppVersion(oClsAppUpdate.ApplicationName, "",this.StationId, enmDownloadAndIntsallStatus.Latest_Version.ToString());
                    }
                    oClsAppUpdate.DownloadStatus = odbMgr.GetStatus(oClsAppUpdate.ApplicationName, StationId).Replace(" ", "_");
                    oColAppUpdates.Add(oClsAppUpdate);
                }
            }
            return oColAppUpdates;
        }

        public void CheckforUpdatesDialog(Form oParentForm)
        {
            try
            {
                log = new clsLogService("SetupLogPOS_Update.log");
                //ofrmUpdate = new frmUpdate(this);
                //ofrmUpdate.ShowDialog();
                popupFornewUpdates(oParentForm);
            }
            catch (Exception ex)
            {
                log.AddError(string.Format("ERROR:{0} \n Stack Trace :{1}", ex.Message, ex.StackTrace));
            }
            finally
            {
                this.log.Close();
            }
        }
    }
}
