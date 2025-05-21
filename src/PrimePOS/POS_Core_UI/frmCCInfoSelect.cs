using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using NLog;

namespace POS_Core_UI
{
    public partial class frmCCInfoSelect : Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public bool SearchByPatientNO = false;
        
        public frmCCInfoSelect()
        {
            InitializeComponent();
        }

        public DataTable CCInfo
        {
            get { return this.dtCCInfo; }
            set { this.dtCCInfo = value; }
        }

        private void frmCCInfoSelect_Load(object sender, EventArgs e)
        {
            this.dgCC.DataSource = dtCCInfo;
            clsUIHelper.setColorSchecme(this);
            clsUIHelper.SetAppearance(this.dgCC);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            logger.Trace("btnAccept_Click(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
            if (this.dgCC.ActiveRow != null && dgCC.ActiveRow.Cells["CCNUMBER"].Text.Trim() != "" )
            {
                CustomerCardInfo = new POS_Core.Resources.PccCardInfo();
                CustomerCardInfo.cardNumber = dgCC.ActiveRow.Cells["CCNUMBER"].Text;
                CustomerCardInfo.cardExpiryDate = dgCC.ActiveRow.Cells["CCEXPDATE"].Text;
                CustomerCardInfo.zipCode = dgCC.ActiveRow.Cells["ZIP"].Text;
                CustomerCardInfo.cardHolderName = dgCC.ActiveRow.Cells["NameOnCC"].Text;
                CustomerCardInfo.customerAddress = dgCC.ActiveRow.Cells["CardAddress"].Text; //Added by Manoj 7/18/2012 to pass customer address
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            logger.Trace("btnAccept_Click(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public POS_Core.Resources.PccCardInfo SelectedCC
        {
            get{return this.CustomerCardInfo;}
        }

        private void dgCC_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            if (this.SearchByPatientNO == true)
            {
                foreach (UltraGridRow oRow in dgCC.Rows)
                {
                    dgCC.ActiveRow = oRow;
                    if (dgCC.ActiveRow.Cells["CardInfoSource"].Value.ToString() == "Charge Account")
                    {
                        dgCC.ActiveRow.Appearance.BackColor = Color.Yellow;
                    }
                    else
                    {
                        dgCC.ActiveRow.Appearance.BackColor = Color.SpringGreen;
                    }
                }
            }
        }

        private void dgCC_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["CCNUMBER"].Hidden = true;
        }
        //Added By shitaljit(QuicSolv) on 11 August 2011
        private void frmCCInfoSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnAccept_Click(this, new System.EventArgs());
            }
        }
        //End of added By shitaljit

    }
}