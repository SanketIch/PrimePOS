using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MMSAppUpdater
{
    class contStationSettings
    {

        clsUpdateManager oVerDown = null;
        List<clsAppUpdate> oAppList = null;


        public List<clsAppUpdate> AppList
        {
            get { return oAppList; }
            set { oAppList = value; }
        }

        public contStationSettings(clsUpdateManager oUpdateManager)
        {
            this.oVerDown = oUpdateManager;
        }
        public DataTable PopulateStationTable()
        {
            oAppList = oVerDown.GetStationList();
            DataTable odt = new DataTable();
       
            DataColumn dtcAppName = odt.Columns.Add("AppName", typeof(string));
            odt.Columns.Add("StationId", typeof(string));
            odt.Columns.Add("UpdateType", typeof(string));
            odt.Columns.Add("MachineName", typeof(string));
            odt.Columns.Add("LastRecordedRunning", typeof(string));
            odt.Columns.Add("ApplicationRunningpath", typeof(string));

            //  odt.PrimaryKey = new DataColumn[] { dtcAppName };

            foreach (clsAppUpdate oApp in oAppList)
            {
                odt.Rows.Add(new Object[] { oApp.ApplicationName, oApp.StationdId, oApp.UpdateType, oApp.MachineName, oApp.LastRecordedRunning, oApp.RunningFilePath });
            }
            return odt;
        }
        
        public bool UpdateSystemStation(string PrvStationId, string StationId, string sAppName)
        {
            return oVerDown.UpdateSystemStation(PrvStationId,StationId, sAppName);
        }
        
        public bool DeleteStation(string PrvStationId, string StationId, string sAppName)
        {
            return oVerDown.DeleteStation(PrvStationId,StationId, sAppName);
        }

        public List<string> GetApplicationList()
        {
            return oVerDown.GetApplicationList();
        }
    }
}
