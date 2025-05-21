using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace AppStarter
{
    public partial class frmMessage : Form
    {
        string sApplictionPath = "";
        public string ApplicationName
        {
            get {

                if (sApplictionPath.Trim() != "")
                {
                    return "";
                    return System.IO.Path.GetFileNameWithoutExtension(sApplictionPath);
                }
                else
                {
                    return "";
                }
            }
            set { sApplictionPath = value; }
        }
        string FileInformation
        {
            get
            {
                if (sApplictionPath.Trim() == "")
                {
                    return "";
                }
                else
                {

                  return FileVersionInfo.GetVersionInfo(sApplictionPath).ProductName;
                }

            }
        }
        public frmMessage()
        {
            InitializeComponent();
        }
        public frmMessage(string sAppPath)
        {
            sApplictionPath = sAppPath;
            InitializeComponent();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Modal)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Modal)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.Close();
            }
        }

        private void frmMessage_Load(object sender, EventArgs e)
        {
            string sMessage = "";
            switch (ApplicationName)
            {
                case "Phw":
                             sMessage = @" In order to update PrimeRx,the pharmacy program (PrimeRx) needs to close from all the stations."+Environment.NewLine
                             +" If you already have closed all the stations and still not able to continue, try turning off all the workstations (Slave computers), and then click Retry.. "+Environment.NewLine+
                             "  If you do not want to update the program, click Cancel, and system will prompt you again next time you start PrimeRx.";
   
                    break;
                case"POS":
                    sMessage = @" In order to update POS,the pharmacy program (POS) needs to close from all the stations."+Environment.NewLine
                             +" If you already have closed all the stations and still not able to continue, try turning off all the workstations (Slave computers), and then click Retry.. "+Environment.NewLine+
                             "  If you do not want to update the program, click Cancel, and system will prompt you again next time you start POS.";
                    break;
                
            }
            if (sMessage.Trim() == "")
            {
                sMessage = " In order to update " + FileInformation + ",the program(" + FileInformation + ") needs to close from all the stations."+Environment.NewLine
                         +" If you already have closed all the stations and still not able to continue, try turning off all the workstations (Slave computers), and then click Retry.."+Environment.NewLine+
                          " If you do not want to update the program, click Cancel, and system will prompt you again next time you start " + FileInformation;
            }

            txtMessage.Text = sMessage;
        }
    }
}