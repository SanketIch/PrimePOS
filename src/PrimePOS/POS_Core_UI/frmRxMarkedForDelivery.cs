using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmRxMarkedForDelivery : Form
    {
        #region Variable Declartation
        private string sSelectedAction = "P";
        #endregion
        public frmRxMarkedForDelivery()
        {
            InitializeComponent();
        }

        public string SelectedAction
        {
            get { return sSelectedAction; }
            set { sSelectedAction = value; }
        }

        private void btnHoldForDelivery_Click(object sender, EventArgs e)
        {
            ProcessAction("D");
        }
        private void btnNo_Click(object sender, EventArgs e)
        {
            ProcessAction("P");
        }
       
        private void ProcessAction(string sender)
        {
            POS_Core.ErrorLogging.Logs.Logger("ProcessAction for RX mark as delivery Enter.");
            try
            {
                switch (sender)
                {
                    case "D":
                        this.Close();
                        break;
                    case "P":
                        this.Close();
                        break;
                    default:
                        break;
                }
                sSelectedAction = sender;
            }
            catch (Exception exp)
            {
                POS_Core.ErrorLogging.Logs.Logger("ProcessAction for RX mark as delivery exception occured" + exp.Message + exp.StackTrace);
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            POS_Core.ErrorLogging.Logs.Logger("ProcessAction for RX mark as delivery Exited selected ACTION ." + sSelectedAction);
        }

        private void frmRxMarkedForDelivery_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void frmRxMarkedForDelivery_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
        }
    }
}
