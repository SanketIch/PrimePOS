using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmPOPrintOptions : Form
    {
        public static string strBarCodeOf = null;

        public frmPOPrintOptions()
        {
            InitializeComponent();
        }

        private void optPrintBarcode_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(optPrintBarcode.CheckedItem.DataValue)== 1)
                strBarCodeOf = "VendItmCode";
            else if (Convert.ToInt32(optPrintBarcode.CheckedItem.DataValue)== 2)
                strBarCodeOf = "ItemID";
            else
                strBarCodeOf = "None";             

        }

        private void frmPOPrintOptions_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.ultraBtnPrintPreview.Select();
            strBarCodeOf = "VendItmCode";
        }

        private void ultraBtnPrintPreview_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();           
        }

        private void ultraBtnPrint_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes; 
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}