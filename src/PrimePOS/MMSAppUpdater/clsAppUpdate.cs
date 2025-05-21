using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Ftp;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace MMSAppUpdater
{
    /// <summary>
    /// IMPLEMENTED BY:Muhammad Saqib
    /// DATE: 15-jan-2010
    /// PURPOSE: base class for autoupdate applications, all Autoupdate applications need to inherit from this class
    /// 
    /// </summary>
    public class clsAppUpdate
    {
        public event EventHandler GetCurrentVersion;
        private bool bSelectedForUpdate = true;
        string sSetupFileName = string.Empty;
        string sSelectedForUpdate = string.Empty;
        string sCommLineArguments = string.Empty;
        int iProductid = 0;
        string sCurrentVersion = string.Empty;
        string bFindRunningInstance = string.Empty;
        DateTime dReleaseDate = DateTime.Now;
        string sReleaseNotes = string.Empty;
        string sApplicationName = string.Empty;
        string sAppServerVersion = string.Empty;
        string sAppFTPFolder = string.Empty;
        string sProduct = string.Empty;
        string sGetCurrVerMethod = string.Empty;
        string sExeName = string.Empty;
        string sUpdateType = string.Empty;
        string sStationdId = "00";
        bool bAlwaysRunning = false;
        string sMachineName = string.Empty;
        string sLastRecordedRunning = string.Empty;
        bool bUrgentDownload = false;
        bool bHiddenMode = false;
        bool localMultiInstanceAllowed = true;
        string sDownloadStatus = "";
        //string sDbver = "";
        string sRunningFilePath = string.Empty;

        public string RunningFilePath
        {
            get { return sRunningFilePath; }
            set { sRunningFilePath = value.Trim(); }
        }

        public string DownloadStatus
        {
            get { return sDownloadStatus; }
            set { sDownloadStatus = value.Trim(); }
        }

        public bool HiddenMode
        {
            get { return bHiddenMode; }
            set { bHiddenMode = value; }
        }

        public bool LocalMultiInstanceAllowed
        {
            get { return localMultiInstanceAllowed; }
            set { localMultiInstanceAllowed = value; }
        }
        bool networkMultiinstanceAllowed=true;

        public bool NetworkMultiinstanceAllowed
        {
            get { return networkMultiinstanceAllowed; }
            set { networkMultiinstanceAllowed = value; }
        }

        public bool AlwaysRunning
        {
            get { return bAlwaysRunning; }
            set
            {
              bAlwaysRunning = value;
            }
        }
        public string StationdId
        {
            get { return sStationdId; }
            set { sStationdId = value;}
        }


        public string UpdateType
        {
            get { return sUpdateType; }
            set { sUpdateType = value.Trim(); }
        }
        public string ExeName
        {
            get { return sExeName; }
            set { sExeName = value.Trim(); }
        }
      
        public string ApplicationName
        {
            get
            {
                return sApplicationName;
            }
            set
            {
                sApplicationName = value.Trim();
            }
        }

        public string AppServerVersion
        {
            get
            {
                return sAppServerVersion;
            }
            set
            {
                sAppServerVersion = value.Trim();
            }
        }

        public string AppFTPFolder
        {
            get
            {
                return sAppFTPFolder;
            }
            set
            {
                sAppFTPFolder = value.Trim();
            }
        }

        public string ReleaseNotes
        {
            get
            {
                return sReleaseNotes;
            }
            set
            {
                sReleaseNotes = value.Trim();
            }
        }

        public DateTime ReleaseDate
        {
            get
            {
                return dReleaseDate;
            }
            set
            {
                dReleaseDate = value;
            }
        }


        public string GetCurrVerMethod
        {
            get
            {
                return sGetCurrVerMethod;
            }
            set
            {
                sGetCurrVerMethod = value;
            }
        }

        public string CurrentVersion
        {
            get
            {
                return sCurrentVersion;
            }
            set
            {
                sCurrentVersion = value;
            }
        }

        public int Productid
        {
            get
            {
                return iProductid;
            }
            set
            {
                iProductid = value;
            }
        }

        public string  CommLineArguments
        {
            get
            {
                return sCommLineArguments;
            }
            set
            {
                sCommLineArguments = value.Trim();
            }
        }

        public bool SelectedForUpdate
        {
            get
            {
                              
                return bSelectedForUpdate;

            }
            set { bSelectedForUpdate = value; }
           
        }

        public string SetupFileName
        {
            get
            {
                return this.sSetupFileName;
            }
            set
            {
                this.sSetupFileName = value.Trim();
            }
        }
  

      
       
        public string Product
        {
            get
            {
                return sProduct;
            }
            set
            {
                sProduct = value.Trim();
            }
        }

        bool replaceFilesDirectely = false;

        public bool ReplaceFilesDirectely
        {
            get { return replaceFilesDirectely; }
            set { replaceFilesDirectely = value; }
        }

        public string MachineName
        {
            get { return sMachineName;}
            set { sMachineName = value.Trim(); }
        }
        public string LastRecordedRunning
        {
            get { return sLastRecordedRunning; }
            set { sLastRecordedRunning = value; }
        }
        public bool UrgentDownload
        {
            get { return bUrgentDownload; }
            set { bUrgentDownload = value; }
        }
        //public string Dbver
        //{
        //    get { return sDbver; }
        //    set { sDbver = value; }
        //}
        
    }
}
