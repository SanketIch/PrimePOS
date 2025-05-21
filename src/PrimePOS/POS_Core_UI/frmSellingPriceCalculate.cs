using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmSellingPriceCalculate : Form

    {

        public double addAmt;

        public frmSellingPriceCalculate()
        {
            InitializeComponent();
        }
        public frmSellingPriceCalculate(string strlblText)
        {
            InitializeComponent();
            lblAmt.Text = strlblText;
        }
       

        private void frmSellingPriceCalculate_Load(object sender, EventArgs e)
        {

            clsUIHelper.setColorSchecme(this);
            this.numAmt.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numAmt.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
 
        }

        private void bttnClear_Click(object sender, EventArgs e)
        {
            numAmt.Value=0;
        }
        bool retval;
        internal bool RETVAl
        {
            get { return retval; }
            set { retval = value; }
        }


        internal double ADDAMT
        {
            get { return addAmt; }
            set {addAmt =value; }
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmSetSellingPrice frmObj = new frmSetSellingPrice();
            if (Resources.Message.Display("Are You Sure you want to close this form?", "Abandon Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                RETVAl=false;
                ADDAMT = 0;
                              
                this.Close();

            }
        }

        private void bttnOK_Click(object sender, EventArgs e)
        {
            ADDAMT = Convert.ToDouble(numAmt.Value);
            RETVAl = true;
            
            this.Close();
        }

        

    }
}