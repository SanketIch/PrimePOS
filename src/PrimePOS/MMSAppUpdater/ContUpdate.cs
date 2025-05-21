using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace MMSAppUpdater
{
    class ContUpdate
    {
        clsUpdateManager oVerDown = null;
        List<clsAppUpdate> oAppList = null;


        public List<clsAppUpdate> AppList
        {
            get { return oAppList; }
            set { oAppList = value; }
        }
        public ContUpdate(clsUpdateManager oUpdateManager)
        {
            this.oVerDown = oUpdateManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable PopulateGridTable()
        {
            oAppList = oVerDown.GetDownloadedUpdatesList(false);
           
 
           DataTable odt = new DataTable();
           odt.Columns.Add("Update", typeof(Boolean));
           DataColumn dtcAppName = odt.Columns.Add("AppName", typeof(string));
           odt.Columns.Add("Version", typeof(string));
           odt.Columns.Add("ReleaseNotes", typeof(string));
           odt.Columns.Add("Status", typeof(string));
           odt.PrimaryKey = new DataColumn[] { dtcAppName };
         
           foreach (clsAppUpdate oApp in oAppList)
            {
               odt.Rows.Add(new Object[] { oApp.SelectedForUpdate, oApp.ApplicationName, oApp.AppServerVersion, oApp.ReleaseNotes,oApp.DownloadStatus  });
            }
            return odt;
        }
        
        public bool DownloadAndInstall(frmUpdate oFrmUpdate)
        {
         
            bool bResult = false;
            if (oVerDown.InstallApps(oAppList, oFrmUpdate))
                bResult = true;          
            return bResult;

        }

        public void SynchronizeChanges(DataTable odt)
        {
            foreach (clsAppUpdate oApp in oAppList)
            {
                DataRow odr = odt.Rows.Find(oApp.ApplicationName);
                if (odr != null && bool.Parse(odr["Update"].ToString()) == true)
                    oApp.SelectedForUpdate = true;
                else
                {
                    oApp.SelectedForUpdate = false;
                }
            }
        }

        
    }
}
