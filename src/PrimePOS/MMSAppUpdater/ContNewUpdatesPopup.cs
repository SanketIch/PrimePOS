using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace MMSAppUpdater
{
    class ContNewUpdatesPopup
    {
        clsUpdateManager oVerDown = null;
        List<clsAppUpdate> oAppList = null;
        public ContNewUpdatesPopup(clsUpdateManager oUpdateManager)
        {
            this.oVerDown = oUpdateManager;
        }
        List<clsAppUpdate> AppsList
        {
            get { return oAppList; }
            set { oAppList = value; }
        }
        public string GetUpdatedString()
        {
          string UpdateString=string.Empty;
          oAppList = oVerDown.CheckforUpdates();
          foreach (clsAppUpdate oApp in oAppList)
            {
                UpdateString += oApp.ApplicationName + " " + oApp.AppServerVersion+Environment.NewLine;
            }
            return UpdateString;
        }

        
        public void LoadFrmUpdate(Form oFrm)
       {
            frmUpdate ofrmUpdate = new frmUpdate(oVerDown);
            ofrmUpdate.ShowDialog(oFrm);
            oVerDown.log.Close();
           
       }
        

    }
}
