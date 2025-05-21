using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    public partial class frmInvoiceDiscountLogicInfo : Form
    {
        public frmInvoiceDiscountLogicInfo()
        {
            InitializeComponent();
        }

        private void frmInvoiceDiscountLogicInfo_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.Text = "Invoice Discount Logic";
            this.txtInvoiceDiscountLogic.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtInvoiceDiscountLogic.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            string text = @"* Allow 100% Invoice Discount

	Use this option if you want to give cashier the ability to apply 100% invoice discount to a POS transaction. 		(100% invoice discount equates to {0}0.00 net transaction amount) An invoice discount of 100% will override                         any and all other discounts.

*  Apply Invoice discount to only items marked as Discountable

	Use this option if you only want invoice discount to be applicable to items marked as discountable. Note that 		items are discountable if they have been marked as such in their respective item files.
 



*  Apply invoice discount to only items which are not individually discounted

	If the transaction has an item containing a line discount than that item will not be considered when applying 		the invoice discount. If used in conjunction with “Apply Invoice discount to only items marked as 			Discountable” check box then invoice discounts will only be applied to discountable items having no line 	                discounts.

*  Apply the invoice discount to all the items (irrespective of the current individual item disc)

	When using this option you will be allowed to apply invoice discounts in the normal manner. However if a line 		discount is present than that amount or percentage will also be added to overall discount. If used in 			conjunction with “Apply Invoice discount to only items marked as Discountable” check box then invoice 		discounts will only be applied to discountable items.

*  Remove individual item disc and apply the invoice disc to the transaction net amount

	When using this option you will be allowed to apply invoice discounts in the normal manner. However if line 		discounts are present they will be overwritten prior to invoice discount application. If used in conjunction 		with “Apply Invoice discount to only items marked as Discountable” check box then invoice discounts will only 		be applied to discountable items.

*  Do not allow invoice disc when individual item disc are present in the transaction

	Use this option if you want to disallow invoice discounts if line discounts have been given. If used in 			conjunction with “Apply Invoice discount to only items marked as Discountable” check box then invoice 		discounts will only be applied to discountable items having no line discounts.";
            text = string.Format(text, Configuration.CInfo.CurrencySymbol);
            this.txtInvoiceDiscountLogic.Text = text;

        }

        private void txtInvoiceDiscountLogic_Enter(object sender, EventArgs e)
        {
            this.txtInvoiceDiscountLogic.SelectionStart = this.txtInvoiceDiscountLogic.Text.Length;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}