using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NLog;

namespace POS_Core_UI
{
    public partial class frmDiscountOptions : Form
    {
        public decimal trPrice;
        public decimal OrgGrdSellingPrice;
        public decimal discount;
        public static bool CallDiscountFrm = false;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmDiscountOptions(decimal pTrPrice,decimal pOrgGrdSellingPrice,decimal pDiscount)
        {
            InitializeComponent();
            trPrice = pTrPrice;
            OrgGrdSellingPrice = pOrgGrdSellingPrice;
            discount = pDiscount;
                
        }
        public frmDiscountOptions()
        {
            InitializeComponent();
        }

        private void frmDiscountOptions_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            //this.txtDiscount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtDiscount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.Location = new Point(75,200);
            lblMessage.Text = "Current Discount is more than Selling Price.";
            txtDiscount.Visible = false;            
            btnUpdateDiscount.Focus();
        }
        

        private void btnUpdateDiscount_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            CallDiscountFrm = true;
            this.Close();
            #region commented By Amit Date 25 May 2011
            //if (btnUpdateDiscount.Text == "&Update Discount")
            //{
            //    txtDiscount.Visible = true;
            //    txtDiscount.Focus();
            //    btnUpdateDiscount.Text = "&Ok";
            //    btnRemoveDiscount.Text = "&Cancel";
            //    btnRevertSellingPrice.Visible = false;
            //    lblMessage.Text = "Enter new Discount";
                
            //}
            //else 
            //{

            //    if (Convert.ToDouble(txtDiscount.Value)!= 0.00 && Convert.ToDouble( trPrice) >Convert.ToDouble( txtDiscount.Value))
            //    {
                    
            //        discount = Convert.ToDecimal(txtDiscount.Value);
            //        this.Close();
            //    }
            //    else
            //    {
            //        if(Convert.ToDouble(txtDiscount.Value)!= 0.00)
            //        clsUIHelper.ShowErrorMsg("Current Discount is more than Selling Price.\nPlease enter discount smaller than Selling Price");
            //        txtDiscount.Focus();                    
            //    }
            //}
#endregion
        }

       
        private void btnRemoveDiscount_Click(object sender, EventArgs e)
        {
            txtDiscount.Visible = false;
            discount = 0;
            this.Close();
            #region Commented Date 25 May 2011 as per new requirement by fahad
        //    if (btnRemoveDiscount.Text == "&Remove Discount")
        //    {
        //        txtDiscount.Visible = false;
        //        discount = 0;
        //        this.Close();
        //    }
        //    else
        //    {
        //        txtDiscount.Visible = false;
        //        txtDiscount.Value=0.0;
        //        btnRemoveDiscount.Enabled = true;
        //        btnRevertSellingPrice.Visible = true;
        //        btnUpdateDiscount.Text = "&Update Discount";
        //        btnRemoveDiscount.Text = "&Remove Discount"; 
        //        lblMessage.Text="Current Discount is more than Selling Price.";

            //    }
            #endregion
        }

        private void btnRevertSellingPrice_Click(object sender, EventArgs e)
        {
            trPrice = OrgGrdSellingPrice;
            this.Close();
        }

        private void frmDiscountOptions_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Trace("frmDiscountOptions_KeyDown(object sender, KeyEventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
            switch (e.KeyData)
            { 
                case Keys.U:
                    //if(btnUpdateDiscount.Text=="&Update Discount")
                        btnUpdateDiscount_Click(null, null);
                    break;
                case Keys.R:
                    //if(btnRemoveDiscount.Text=="&Remove Discount")
                        btnRemoveDiscount_Click(null, null);
                    break;
                case Keys.S:
                   // if(btnRevertSellingPrice.Visible==true)
                        btnRevertSellingPrice_Click(null,null);
                    break;
                //case Keys.O:
                //    if (btnUpdateDiscount.Text == "&Ok")
                //        btnUpdateDiscount_Click(null, null);
                //    break;
                //case Keys.C:
                //    if (btnRemoveDiscount.Text == "&Cancel")
                //        btnRemoveDiscount_Click(null, null);
                //    break;
            }
            logger.Trace("frmDiscountOptions_KeyDown(object sender, KeyEventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
        }
    }
}