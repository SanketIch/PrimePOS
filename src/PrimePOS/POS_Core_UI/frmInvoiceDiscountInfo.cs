//-------------------------------------------------------------------------------------------------------------------------
//Added By Shitaljit(QuicSolv) on 19 Sept 2011
// To Display the reasons of not applying invoice discount.
//-------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using NLog;

namespace POS_Core_UI
{
    public partial class frmInvoiceDiscountInfo : Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public frmInvoiceDiscountInfo()
        {
            InitializeComponent();
        }

        private void frmInvoiceDiscountInfo_Load(object sender, EventArgs e)
        {
            logger.Trace("frmInvoiceDiscountInfo_Load(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
            clsUIHelper.setColorSchecme(this);
            this.btnOK.Select();
            this.Text = "Invoice Discount";
            this.Location = new Point(170, 200);
            logger.Trace("frmInvoiceDiscountInfo_Load(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}