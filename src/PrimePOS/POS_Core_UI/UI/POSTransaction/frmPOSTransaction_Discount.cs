using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData;
using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmPOSTransaction
    {
        private bool ValidateInvDiscount(out string strDialogResult, out bool isPer, out Decimal disc, out bool isDiscountableTrans)
        {
            logger.Trace("ValidateInvDiscount() - " + clsPOSDBConstants.Log_Entering);

            #region Validating Invoice discount
            System.Decimal invdisc = 0;
            bool AllowDisOfItemsOnSale = false;
            frmInvoiceDiscountInfo ofrmInvoiceDiscountInfo = new frmInvoiceDiscountInfo();
            ListViewItem oListViewItem;
            string strMSG = "";

            System.Decimal totalPrice = 0;
            bool RetVal = false;
            isPer = false;
            isDiscountableTrans = true;
            IsHundredPerInvDisc = false;
            disc = 0;
            bool isDiscountableItemPresnt = false;

            frmPOSDicount ofrm = new frmPOSDicount();
            ofrm.Text = "Invoice Discount";
            ofrm.bCalledFromLineItem = false;
            if (ofrm.ShowDialog(this) == DialogResult.OK)
            {
                strDialogResult = "OK";

                ofrmInvoiceDiscountInfo.lvItemList.Items.Clear();
                foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                {
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    //InvDiscToDiscountableItemOnly == true/false logic is added on 27 Dec 2011
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                    {
                        //Following logic will look at each item to see if it is marked as discountable or not.
                        //If item is marked as discountbale then apply the invoice discount logic to this item.
                        //It item is not marked as discountable do not apply invoice discount logic at all to this item.
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true)
                        {
                            totalPrice += GetDiscountableTotal(ref ofrmInvoiceDiscountInfo, ref isDiscountableItemPresnt, AllowDisOfItemsOnSale, oGRow);
                        }
                    }
                    else //InvDiscToDiscountableItemOnly == false
                    {
                        //Following logic will egnore items marked as discountable or not.
                        //and it will consider all items for invoice discount logic.
                        totalPrice += GetDiscountableTotal(ref ofrmInvoiceDiscountInfo, ref isDiscountableItemPresnt, AllowDisOfItemsOnSale, oGRow);                        
                    }
                }
                oListViewItem = ofrmInvoiceDiscountInfo.lvItemList.Items.Add("");
                oListViewItem.SubItems.Add("");
                oListViewItem.SubItems.Add("");
                oListViewItem = ofrmInvoiceDiscountInfo.lvItemList.Items.Add("Total");
                oListViewItem.SubItems.Add("");
                oListViewItem.SubItems.Add(totalPrice.ToString("######0.00"));
                int count = ofrmInvoiceDiscountInfo.lvItemList.Items.Count - 1;
                ofrmInvoiceDiscountInfo.lvItemList.Items[count].BackColor = System.Drawing.Color.Green;

                #region Validation for Maximum Limit % Discount user can give
                if (Convert.ToDecimal(ofrm.numDiscPerc.Value) != 0) //Invoice Discount given in Percentage
                {
                    InvDicsValueToVerify = Convert.ToDecimal(ofrm.numDiscPerc.Value);
                }
                else if (totalPrice > 0) //Invoice Discount given in Dollar amount
                {
                    InvDicsValueToVerify = ((Convert.ToDecimal(ofrm.numDiscAmount.Value) / totalPrice) * 100);
                }
                
                UserManagement.clsLogin oLogin = new UserManagement.clsLogin();
                string sUserID = "";
                if (InvDicsValueToVerify > Configuration.UserMaxDiscountLimit && Configuration.IsDiscOverridefromPOSTrans == false)
                {
                    if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxDiscountLimit, "", out sUserID, "Security Override For Max Discount Limit") == false)
                    {
                        InvDicsValueToVerify = 0;
                        return false;
                    }
                    else
                    {
                        strMaxDiscountLimitOverrideUser = sUserID;
                    }
                }
                else if (Configuration.IsDiscOverridefromPOSTrans == true)  //PRIMEPOS-2979 19-Aug-2021 JY Added
                {
                    //Check with logged in user limit
                    if (InvDicsValueToVerify > Configuration.UserMaxDiscountLimit)
                    {
                        //Check with discount override user limit
                        bool bStatus = false;
                        if (strInvDiscOverrideUser != "" && strInvDiscOverrideUser != Configuration.UserName)
                            bStatus = UserPriviliges.IsUserHasPriviligesToOverrideInvoiceDiscount(strInvDiscOverrideUser, InvDicsValueToVerify);
                        if (bStatus == false)
                        {
                            if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxDiscountLimit, "", out sUserID, "Security Override For MaxDiscountLimit: exceeds logged-in & Disc Override user") == false)
                            {
                                InvDicsValueToVerify = 0;
                                return false;
                            }
                            else 
                            {
                                strMaxDiscountLimitOverrideUser = sUserID;
                            }
                        }
                    }
                }
                #endregion Validation for Maximum Limit % Discount user can give

                if (Convert.ToDecimal(ofrm.numDiscPerc.Value) != 0) //Invoice Discount given in Percentage
                {
                    isPer = true;
                    if (totalPrice > 0 && totalPrice > Convert.ToDecimal(this.txtAmtDiscount.Text))
                    {
                        totalPrice = totalPrice - Convert.ToDecimal(this.txtAmtDiscount.Text);
                    }
                    disc = Convert.ToDecimal(ofrm.numDiscPerc.Value);
                    //Added By Shitaljit in 17 Feb to add customer discount to the invoice discount.
                    if (oPOSTrans.oCustomerRow.Discount > 0 && (oPOSTrans.oCustomerRow.Discount + disc <= Hundred))
                    {
                        ofrm.numDiscPerc.Value = disc = disc + oPOSTrans.oCustomerRow.Discount;
                    }
                    else if ((oPOSTrans.oCustomerRow.Discount + disc) > Hundred)
                    {
                        clsUIHelper.ShowErrorMsg("Selected customer already has " + oPOSTrans.oCustomerRow.Discount + "% discount.\nYou cannot apply Invoice discount of " + disc + "%.");
                        RetVal = false;
                        return RetVal;
                    }

                    //Logic for Hundred percent invoice discount.
                    if (disc == Hundred && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    {
                        IsHundredPerInvDisc = true;
                        RetVal = true;
                        return RetVal;
                    }
                    else
                    {
                        invdisc = oPOSTrans.GetDiscount(Convert.ToDecimal((Convert.ToDecimal(ofrm.numDiscPerc.Value) / 100 * totalPrice)));
                    }
                } else //Invoice Discount given in Dollar Amount
                  {
                    invdisc = oPOSTrans.GetDiscount(Convert.ToDecimal(ofrm.numDiscAmount.Value));
                    disc = Convert.ToDecimal(ofrm.numDiscAmount.Value);

                    //Added By Shitaljit in 17 Feb to add customer discount to the invoice discount.
                    if (oPOSTrans.oCustomerRow.Discount > 0) {
                        Decimal custDiscount = oPOSTrans.GetDiscount(Convert.ToDecimal((oPOSTrans.oCustomerRow.Discount) / 100 * totalPrice));
                        ofrm.numDiscAmount.Value = invdisc = disc = disc + custDiscount;
                    }
                    //Logic for Hundred percent invoice discount.
                    if ((invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) == Convert.ToDecimal(this.txtAmtSubTotal.Text) && Configuration.CInfo.AllowHundredPerInvDiscount == true) {
                        IsHundredPerInvDisc = true;
                        RetVal = true;
                        return RetVal;
                    }
                }
                if ((invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) > Convert.ToDecimal(this.txtAmtSubTotal.Text) && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CPOSSet.AllItemDisc.Trim() != "2") {
                    /*Date 27-jan-2014
                            * Modified by Shitaljit
                            * For making currency symbol dynamic
                            */
                    //old code
                    //clsUIHelper.ShowErrorMsg("Total Discount amount $" + (invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) + " is greater than discountable amount $" + totalPrice + "\nUnable to apply Invoice Discount");
                    //new code
                    clsUIHelper.ShowErrorMsg("Total Discount amount " + Configuration.CInfo.CurrencySymbol.ToString() + (invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) + " is greater than discountable amount " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice + "\nUnable to apply Invoice Discount");
                } else if (totalPrice == 0 && isDiscountableItemPresnt == false) {
                    isDiscountableTrans = false;
                    if (Convert.ToDecimal(ofrm.numDiscPerc.Value) != 0) {
                        strMSG = "Unable to apply Invoice Discount of " + disc.ToString("######0.00") + "%\nbecause there is no discountable item in the transaction.";
                    } else {
                        /*Date 27-jan-2014
                            * Modified by Shitaljit
                            * For making currency symbol dynamic
                            */
                        //old code
                        //strMSG = "Unable to apply Invoice Discount of $" + disc.ToString("######0.00") + "\nbecause there is no discountable item in the transaction.";
                        //new code
                        strMSG = "Unable to apply Invoice Discount of " + Configuration.CInfo.CurrencySymbol.ToString() + disc.ToString("######0.00") + "\nbecause there is no discountable item in the transaction.";
                    }
                    clsUIHelper.ShowErrorMsg(strMSG);
                    disc = 0;
                    invdisc = 0;
                } else if (totalPrice == 0 && Configuration.CPOSSet.AllItemDisc.Trim() == "0" && isDiscountableItemPresnt == true) {
                    isDiscountableTrans = false;
                    clsUIHelper.ShowErrorMsg("Unable to apply Invoice Discount \nbecause there is no item in the transaction without individual item discount\non which we can apply invoice discount\nOR there is no discountable item in the transaction.");
                    disc = 0;
                    invdisc = 0;
                } else if (invdisc > totalPrice) {
                    /*Date 27-jan-2014
                            * Modified by Shitaljit
                            * For making currency symbol dynamic
                            */
                    //old code
                    //strMSG = "Discountable item total price is $" + totalPrice.ToString("######0.00") + "\nUnable to apply Invoice Discount of $" + invdisc.ToString("######0.00") + " because its greater than $" + totalPrice.ToString("######0.00") + ".";
                    //new code
                    strMSG = "Discountable item total price is " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice.ToString("######0.00") + "\nUnable to apply Invoice Discount of " + Configuration.CInfo.CurrencySymbol.ToString() + invdisc.ToString("######0.00") + " because its greater than " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice.ToString("######0.00") + ".";

                    ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                    ofrmInvoiceDiscountInfo.ShowDialog();
                    disc = 0;
                    invdisc = 0;
                } else if (totalPrice == invdisc) {
                    /*Date 27-jan-2014
                            * Modified by Shitaljit
                            * For making currency symbol dynamic
                            */
                    //old code
                    //strMSG = "Unable to apply Invoice Discount of $" + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at $0.00 after invoice discount.";
                    //new code
                    strMSG = "Unable to apply Invoice Discount of " + Configuration.CInfo.CurrencySymbol.ToString() + invdisc.ToString("######0.00")
                        + "\nbecause discountable item will be priced at " + Configuration.CInfo.CurrencySymbol.ToString() + "0.00 after invoice discount.";

                    ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                    ofrmInvoiceDiscountInfo.ShowDialog();
                    disc = 0;
                    invdisc = 0;
                } else if ((invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) == Convert.ToDecimal(this.txtAmtSubTotal.Text) && Configuration.CInfo.AllowHundredPerInvDiscount == false) {
                    /*Date 27-jan-2014
                            * Modified by Shitaljit
                            * For making currency symbol dynamic
                            */
                    //old code
                    //strMSG = "Unable to apply Invoice Discount of $" + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at $0.00 after invoice discount.";
                    //new code
                    strMSG = "Unable to apply Invoice Discount of " + Configuration.CInfo.CurrencySymbol.ToString() + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at " + Configuration.CInfo.CurrencySymbol.ToString() + "0.00 after invoice discount.";

                    ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                    ofrmInvoiceDiscountInfo.ShowDialog();
                    disc = 0;
                    invdisc = 0;
                } else if (totalPrice > invdisc) {
                    RetVal = true;
                }

                #endregion Validating Invoice discount
            } else {
                RetVal = true;
                strDialogResult = "Cancel";
            }
            logger.Trace("ValidateInvDiscount() - " + clsPOSDBConstants.Log_Exiting);
            return RetVal;
        }

        //completed by sandeep
        private bool ValidateCouponDiscount(decimal discountVal, out decimal CouponDiscount)
        {
            logger.Trace("ValidateCouponDiscount() - " + clsPOSDBConstants.Log_Entering);
            System.Decimal invdisc = 0;
            bool AllowDisOfItemsOnSale = false;
            bool RetVal = false;
            ListViewItem oListViewItem;
            string strMSG = "";
            CouponDiscount = 0;
            System.Decimal totalPrice = 0;
            bool isDiscountableTrans = false;
            bool isDiscountableItemPresnt = false;
            frmInvoiceDiscountInfo ofrmInvoiceDiscountInfo = new frmInvoiceDiscountInfo();
            ofrmInvoiceDiscountInfo.lvItemList.Items.Clear();

            //Do not allow invoice discount when individual item discounts are present in the transaction
            if (Configuration.CPOSSet.AllItemDisc.Trim() == "3") {
                bool itemDiscount = false;
                foreach (UltraGridRow oGRow in this.grdDetail.Rows) {
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true) {
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true) {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                            if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0) {
                                itemDiscount = true;
                                AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);
                            }
                        }
                    } else if (AllowDisOfItemsOnSale == true) {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0) {
                            itemDiscount = true;
                            AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);
                        }
                    }
                }

                if (itemDiscount == false) {
                    CouponDiscount = oPOSTrans.GetDiscount(discountVal / (100 * totalPrice));
                } else if (itemDiscount == true) {
                    strMSG = "Above items have individual discount\nUnable to apply promotional coupon discount. ";

                    ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                    ofrmInvoiceDiscountInfo.ShowDialog();
                    CouponDiscount = 0;
                    return RetVal;
                }
            } else {
                foreach (UltraGridRow oGRow in this.grdDetail.Rows) {
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    //InvDiscToDiscountableItemOnly == true/false logic is added on 27 Dec 2011
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true) {
                        //Following logic will look at each item to see if it is marked as discountable or not.
                        //If item is marked as discountbale then apply the invoice discount logic to this item.
                        //It item is not marked as discountable do not apply invoice discount logic at all to this item.
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true) {
                            totalPrice += GetDiscountableTotal(ref ofrmInvoiceDiscountInfo, ref isDiscountableItemPresnt, AllowDisOfItemsOnSale, oGRow);
                        }
                    } else //InvDiscToDiscountableItemOnly == false
                      {
                        //Following logic will egnore items marked as discountable or not.
                        //and it will consider all items for invoice discount logic.
                        totalPrice += GetDiscountableTotal(ref ofrmInvoiceDiscountInfo, ref isDiscountableItemPresnt, AllowDisOfItemsOnSale, oGRow);
                    }
                }

            }
            oListViewItem = ofrmInvoiceDiscountInfo.lvItemList.Items.Add("");
            oListViewItem.SubItems.Add("");
            oListViewItem.SubItems.Add("");
            oListViewItem = ofrmInvoiceDiscountInfo.lvItemList.Items.Add("Total");
            oListViewItem.SubItems.Add("");
            oListViewItem.SubItems.Add(totalPrice.ToString("######0.00"));
            int count = ofrmInvoiceDiscountInfo.lvItemList.Items.Count - 1;
            ofrmInvoiceDiscountInfo.lvItemList.Items[count].BackColor = System.Drawing.Color.Green;

            Decimal CurrInvDiscPer = 0;
            if (totalPrice > 0) {
                CurrInvDiscPer = Configuration.convertNullToDecimal(this.lblInvDiscount.Text) / totalPrice * 100;
            }
            if ((CurrInvDiscPer + discountVal) > Hundred) {
                clsUIHelper.ShowErrorMsg("Selected transaction already has " + CurrInvDiscPer + "% discount.\nYou cannot apply promotional coupon discount discount of " + oPOSTrans.oCustomerRow.Discount + "%.");
                RetVal = false;
                return RetVal;
            }
            if ((discountVal == Hundred && Configuration.CInfo.AllowHundredPerInvDiscount == true) || (CurrInvDiscPer + discountVal) == Hundred && totalPrice > 0) {
                IsHundredPerInvDisc = true;
                CouponDiscount = invdisc = oPOSTrans.GetDiscount(Convert.ToDecimal((discountVal) / 100 * totalPrice));
                RetVal = true;
                return RetVal;
            } else {
                CouponDiscount = invdisc = oPOSTrans.GetDiscount(Convert.ToDecimal((discountVal) / 100 * totalPrice));
            }

            if ((invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) > Convert.ToDecimal(this.txtAmtSubTotal.Text) && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CPOSSet.AllItemDisc.Trim() != "2") {
                /*Date 27-jan-2014
             * Modified by Shitaljit
             * For making currency symbol dynamic
             */
                //New Code
                clsUIHelper.ShowErrorMsg("Total Discount amount " + Configuration.CInfo.CurrencySymbol.ToString() + (invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) + " is greater than discountable amount " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice + "\nUnable to apply Promotional discount Discount");
                //old code
                //clsUIHelper.ShowErrorMsg("Total Discount amount $" + (invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) + " is greater than discountable amount $" + totalPrice + "\nUnable to apply Promotional discount Discount");
            } else if (totalPrice == 0 && isDiscountableItemPresnt == false) {
                isDiscountableTrans = false;
                strMSG = "Unable to apply promotional coupon discount of " + discountVal + "%\nbecause there is no discountable item in the transaction.";
                clsUIHelper.ShowErrorMsg(strMSG);
                CouponDiscount = 0;
                invdisc = 0;
            } else if (totalPrice == 0 && Configuration.CPOSSet.AllItemDisc.Trim() == "0" && isDiscountableItemPresnt == true) {
                isDiscountableTrans = false;
                clsUIHelper.ShowErrorMsg("Unable to apply promotional coupon discount \nbecause there is no item in the transaction without individual item discount\non which we can apply promotional coupon discount \nOR there is no discountable item in the transaction.");
                CouponDiscount = 0;
                invdisc = 0;
            } else if (invdisc > totalPrice) {
                /*Date 27-jan-2014
                 * Modified by Shitaljit
                 * For making currency symbol dynamic
                 */
                //New Code
                strMSG = "Discountable item total price is " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice.ToString("######0.00") + "\nUnable to apply promotional coupon discount of " + Configuration.CInfo.CurrencySymbol.ToString() + invdisc.ToString("######0.00") + " because its greater than " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice.ToString("######0.00") + ".";
                //Old Code
                //strMSG = "Discountable item total price is $"  + totalPrice.ToString("######0.00") + "\nUnable to apply promotional coupon discount of $" +  invdisc.ToString("######0.00") + " because its greater than $"  + totalPrice.ToString("######0.00") + ".";
                ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                ofrmInvoiceDiscountInfo.ShowDialog();
                CouponDiscount = 0;
                invdisc = 0;
            } else if (totalPrice == invdisc) {
                /*Date 27-jan-2014
                 * Modified by Shitaljit
                 * For making currency symbol dynamic
                 */
                //New Code
                strMSG = "Unable to apply promotional coupon discount of " + Configuration.CInfo.CurrencySymbol.ToString() + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at " + Configuration.CInfo.CurrencySymbol.ToString() + "0.00 after promotional coupon discount.";
                //old code
                //strMSG = "Unable to apply promotional coupon discount of $" + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at $0.00 after promotional coupon discount.";
                ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                ofrmInvoiceDiscountInfo.ShowDialog();
                CouponDiscount = 0;
                invdisc = 0;
            } else if ((invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) == Convert.ToDecimal(this.txtAmtSubTotal.Text) && Configuration.CInfo.AllowHundredPerInvDiscount == false) {
                /*Date 27-jan-2014
                 * Modified by Shitaljit
                 * For making currency symbol dynamic
                 */
                //New Code
                strMSG = "Unable to apply promotional coupon discount of " + Configuration.CInfo.CurrencySymbol.ToString() + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at " + Configuration.CInfo.CurrencySymbol.ToString() + "0.00 after promotional coupon discount.";
                //Old Code
                //strMSG = "Unable to apply promotional coupon discount of $" + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at $0.00 after promotional coupon discount.";
                ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                ofrmInvoiceDiscountInfo.ShowDialog();
                CouponDiscount = 0;
                invdisc = 0;
            } else if (totalPrice > invdisc) {
                RetVal = true;
                //Remove individual item discount and apply the invoice discount to the transaction net amount
                if (Configuration.CPOSSet.AllItemDisc.Trim() == "2") {
                    foreach (UltraGridRow oGRow in this.grdDetail.Rows) {
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0) {
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value = 0;
                        }
                    }
                }
            }
            logger.Trace("ValidateCouponDiscount() - " + clsPOSDBConstants.Log_Exiting);
            return RetVal;
        }
        //completed by sandeep
        private bool ValidateCustomerDiscount(decimal discountVal, out decimal custDiscount)
        {
            logger.Trace("ValidateCustomerDiscount() - " + clsPOSDBConstants.Log_Entering);
            decimal invdisc = 0;
            bool AllowDisOfItemsOnSale = false;
            bool RetVal = false;
            ListViewItem oListViewItem;
            string strMSG = "";
            custDiscount = 0;
            decimal totalPrice = 0;

            bool isDiscountableItemPresnt = false;
            bool isDiscountableTrans = false;
            frmInvoiceDiscountInfo ofrmInvoiceDiscountInfo = new frmInvoiceDiscountInfo();
            ofrmInvoiceDiscountInfo.lvItemList.Items.Clear();
            //Do not allow invoice discount when individual item discounts are present in the transaction
            if (Configuration.CPOSSet.AllItemDisc.Trim() == "3") {
                bool itemDiscount = false;
                foreach (UltraGridRow oGRow in grdDetail.Rows) {
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true) {
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true) {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                            if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0) {
                                itemDiscount = true;
                                AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);
                            }
                        }
                    } else if (AllowDisOfItemsOnSale == true) {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0) {
                            itemDiscount = true;
                            AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);
                        }
                    }
                }
                if (itemDiscount == false) {
                    custDiscount = oPOSTrans.GetDiscount(discountVal / (100 * totalPrice));
                } else if (itemDiscount == true) {
                    strMSG = "Above items have individual discount\nUnable to apply Customer Discount. ";
                    ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                    ofrmInvoiceDiscountInfo.ShowDialog();
                    custDiscount = 0;
                    return RetVal;
                }
            } else {
                foreach (UltraGridRow oGRow in this.grdDetail.Rows) {
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    //InvDiscToDiscountableItemOnly == true/false logic is added on 27 Dec 2011
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true) {
                        //Following logic will look at each item to see if it is marked as discountable or not.
                        //If item is marked as discountbale then apply the invoice discount logic to this item.
                        //It item is not marked as discountable do not apply invoice discount logic at all to this item.
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true) {
                            totalPrice += GetDiscountableTotal(ref ofrmInvoiceDiscountInfo, ref isDiscountableItemPresnt, AllowDisOfItemsOnSale, oGRow);
                        }
                    } else //InvDiscToDiscountableItemOnly == false
                      {
                        //Following logic will egnore items marked as discountable or not.
                        //and it will consider all items for invoice discount logic.
                        totalPrice += GetDiscountableTotal(ref ofrmInvoiceDiscountInfo, ref isDiscountableItemPresnt, AllowDisOfItemsOnSale, oGRow);
                    }
                }
            }
            oListViewItem = ofrmInvoiceDiscountInfo.lvItemList.Items.Add("");
            oListViewItem.SubItems.Add("");
            oListViewItem.SubItems.Add("");
            oListViewItem = ofrmInvoiceDiscountInfo.lvItemList.Items.Add("Total");
            oListViewItem.SubItems.Add("");
            oListViewItem.SubItems.Add(totalPrice.ToString("######0.00"));
            int count = ofrmInvoiceDiscountInfo.lvItemList.Items.Count - 1;
            ofrmInvoiceDiscountInfo.lvItemList.Items[count].BackColor = System.Drawing.Color.Green;
            Decimal CurrInvDiscPer = 0;
            if (totalPrice > 0) {
                CurrInvDiscPer = Configuration.convertNullToDecimal(this.lblInvDiscount.Text) / totalPrice * 100;
            }
            if ((CurrInvDiscPer + discountVal) > Hundred) {
                clsUIHelper.ShowErrorMsg("Selected transaction already has " + CurrInvDiscPer + "% discount.\nYou cannot apply customer discount of " + oPOSTrans.oCustomerRow.Discount + "%.");
                RetVal = false;
                return RetVal;
            }
            if ((discountVal == Hundred && Configuration.CInfo.AllowHundredPerInvDiscount == true) || (CurrInvDiscPer + discountVal) == Hundred && totalPrice > 0) {
                IsHundredPerInvDisc = true;
                custDiscount = invdisc = oPOSTrans.GetDiscount(Convert.ToDecimal((discountVal) / 100 * totalPrice));
                RetVal = true;
                return RetVal;
            } else {
                custDiscount = invdisc = oPOSTrans.GetDiscount(Convert.ToDecimal((discountVal) / 100 * totalPrice));
            }

            if ((invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) > Convert.ToDecimal(this.txtAmtSubTotal.Text) && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CPOSSet.AllItemDisc.Trim() != "2") {
                /*Date 27-jan-2014
                 * Modified by Shitaljit
                 * For making currency symbol dynamic
                 */
                //New Code
                clsUIHelper.ShowErrorMsg("Total Discount amount " + Configuration.CInfo.CurrencySymbol + (invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) + " is greater than discountable amount " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice + "\nUnable to apply Customer Discount");
                //old code
                //clsUIHelper.ShowErrorMsg("Total Discount amount " + (invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) + " is greater than discountable amount $" + totalPrice + "\nUnable to apply Customer Discount");
            } else if (totalPrice == 0 && isDiscountableItemPresnt == false) {
                isDiscountableTrans = false;
                strMSG = "Unable to apply customer discount of " + discountVal + "%\nbecause there is no discountable item in the transaction.";
                clsUIHelper.ShowErrorMsg(strMSG);
                custDiscount = 0;
                invdisc = 0;
            } else if (totalPrice == 0 && Configuration.CPOSSet.AllItemDisc.Trim() == "0" && isDiscountableItemPresnt == true) {
                isDiscountableTrans = false;
                clsUIHelper.ShowErrorMsg("Unable to apply customer Discount \nbecause there is no item in the transaction without individual item discount\non which we can apply customer discount\nOR there is no discountable item in the transaction.");
                custDiscount = 0;
                invdisc = 0;
            } else if (invdisc > totalPrice) {
                strMSG = "Discountable item total price is " + Configuration.CInfo.CurrencySymbol +
                         totalPrice.ToString("######0.00") + "\nUnable to apply customer discount of " +
                         Configuration.CInfo.CurrencySymbol + invdisc.ToString("######0.00") +
                         " because its greater than " + Configuration.CInfo.CurrencySymbol +
                         totalPrice.ToString("######0.00") + ".";
                ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                ofrmInvoiceDiscountInfo.ShowDialog();
                custDiscount = 0;
                invdisc = 0;
            } else if (totalPrice == invdisc) {
                strMSG = "Unable to apply customer discount of " + Configuration.CInfo.CurrencySymbol + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at " + Configuration.CInfo.CurrencySymbol + "0.00 after customer discount.";
                ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                ofrmInvoiceDiscountInfo.ShowDialog();
                custDiscount = 0;
                invdisc = 0;
            } else if ((invdisc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) == Convert.ToDecimal(this.txtAmtSubTotal.Text) && Configuration.CInfo.AllowHundredPerInvDiscount == false) {
                strMSG = "Unable to apply customer discount of " + Configuration.CInfo.CurrencySymbol + invdisc.ToString("######0.00") + "\nbecause discountable item will be priced at " + Configuration.CInfo.CurrencySymbol + "0.00 after customer discount.";
                ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                ofrmInvoiceDiscountInfo.ShowDialog();
                custDiscount = 0;
                invdisc = 0;
            } else if (totalPrice > invdisc) {
                RetVal = true;
                //Remove individual item discount and apply the invoice discount to the transaction net amount
                if (Configuration.CPOSSet.AllItemDisc.Trim() == "2") {
                    foreach (UltraGridRow oGRow in this.grdDetail.Rows) {
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0) {
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value = 0;
                        }
                    }
                }
            }
            logger.Trace("ValidateCustomerDiscount() - " + clsPOSDBConstants.Log_Exiting);
            return RetVal;
        }

        //private decimal ApplyDiscountToAllItem(string percentageValue)    //PRIMEPOS-2768 02-Jan-2020 JY Commented
        private decimal ApplyDiscountToAllItem(decimal disc, string percentageValue = null) //PRIMEPOS-2768 02-Jan-2020 JY Added
        {
            decimal totalPrice = 0;

            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                bool AllowDisOfItemsOnSale = false;
                AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                {
                    if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                    {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
                        if (Configuration.convertNullToDecimal(percentageValue) != 0)
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round((Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value)) * Configuration.convertNullToDecimal(percentageValue) / 100, 4, MidpointRounding.AwayFromZero);   //PRIMEPOS-2768 02-Jan-2020 JY Added
                    }
                }
                else if (AllowDisOfItemsOnSale == true)
                {
                    totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
                    if (Configuration.convertNullToDecimal(percentageValue) != 0)
                        oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round((Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value)) * Configuration.convertNullToDecimal(percentageValue) / 100, 4, MidpointRounding.AwayFromZero);   //PRIMEPOS-2768 02-Jan-2020 JY Added
                }
            }

            #region PRIMEPOS-2768 02-Jan-2020 JY Added
            if (Configuration.convertNullToDecimal(percentageValue) == 0)
            {
                if (totalPrice > disc)
                    disc = oPOSTrans.GetDiscount(disc);
                else if (totalPrice == disc && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    disc = oPOSTrans.GetDiscount(disc);
                else
                {
                    disc = 0;
                }
                if (disc != 0)
                {
                    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                    {
                        bool AllowDisOfItemsOnSale = false;
                        AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                        if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                        {
                            if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                            {
                                oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round((Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value)) * disc / totalPrice, 4, MidpointRounding.AwayFromZero);
                            }
                        }
                        else if (AllowDisOfItemsOnSale == true)
                        {
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round((Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value)) * disc / totalPrice, 4, MidpointRounding.AwayFromZero);
                        }
                    }
                }
            }
            else
            {
                if (totalPrice == 0)
                {
                    disc = 0;
                }
                else
                    disc = oPOSTrans.GetDiscount(Convert.ToDecimal((Convert.ToDecimal(percentageValue) / 100 * totalPrice)));
            }
            #endregion            
            return disc;
        }

        private decimal ApplyDiscountForBalRemaining(decimal disc, string percentageValue = null)
        {
            decimal totalPrice = 0;

            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                bool AllowDisOfItemsOnSale = false;
                AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                {
                    if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                    {
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                        {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                            if (Configuration.convertNullToDecimal(percentageValue) != 0)
                                oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * Configuration.convertNullToDecimal(percentageValue) / 100, 4, MidpointRounding.AwayFromZero);    //PRIMEPOS-2768 02-Jan-2020 JY Added
                        }
                    }
                }
                else if (AllowDisOfItemsOnSale == true)
                {
                    if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                    {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                        if (Configuration.convertNullToDecimal(percentageValue) != 0)
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * Configuration.convertNullToDecimal(percentageValue) / 100, 4, MidpointRounding.AwayFromZero);    //PRIMEPOS-2768 02-Jan-2020 JY Added
                    }
                }
            }

            //if (percentageValue == null)  //PRIMEPOS-2768 03-Jan-2020 JY commented
            if (Configuration.convertNullToDecimal(percentageValue) == 0)   //PRIMEPOS-2904 07-Oct-2020 JY Added
            {
                if (totalPrice > disc)
                    disc = oPOSTrans.GetDiscount(disc);
                else if (totalPrice == disc && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    disc = oPOSTrans.GetDiscount(disc);
                else
                {
                    disc = 0;
                }
                #region PRIMEPOS-2768 02-Jan-2020 JY Added
                if (disc != 0)
                {
                    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                    {
                        bool AllowDisOfItemsOnSale = false;
                        AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                        if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                        {
                            if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                            {
                                if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                                {
                                    oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * disc / totalPrice, 4, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        else if (AllowDisOfItemsOnSale == true)
                        {
                            if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                            {
                                oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * disc / totalPrice, 4, MidpointRounding.AwayFromZero);    //PRIMEPOS-2768 02-Jan-2020 JY Added
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                if (totalPrice == 0)
                {
                    disc = 0;
                }
                else
                    disc = oPOSTrans.GetDiscount(Convert.ToDecimal((Convert.ToDecimal(percentageValue) / 100 * totalPrice)));
            }
            return disc;
        }
        
        public decimal ApplyDiscountAfterRemovingInd(decimal disc, string percentageValue = null)
        {
            decimal totalPrice = 0;
            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                bool AllowDisOfItemsOnSale = false;
                AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                {
                    if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                    {
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0)
                        {
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value = 0;
                        }
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                        if (Configuration.convertNullToDecimal(percentageValue) != 0)
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * Configuration.convertNullToDecimal(percentageValue) / 100, MidpointRounding.AwayFromZero);   //PRIMEPOS-2768 02-Jan-2020 JY Added
                    }
                }
                else if (AllowDisOfItemsOnSale == true)
                {
                    if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0)
                    {
                        oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value = 0;
                    }
                    totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                    if (Configuration.convertNullToDecimal(percentageValue) != 0)
                        oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * Configuration.convertNullToDecimal(percentageValue) / 100, MidpointRounding.AwayFromZero);   //PRIMEPOS-2768 02-Jan-2020 JY Added
                }
            }

            //if (percentageValue == null)  //PRIMEPOS-2768 03-Jan-2020 JY commented
            if (Configuration.convertNullToDecimal(percentageValue) == 0)   //PRIMEPOS-2904 07-Oct-2020 JY Added
            {
                if (totalPrice > disc)
                    disc = oPOSTrans.GetDiscount(disc);
                else if (totalPrice == disc && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    disc = oPOSTrans.GetDiscount(disc);
                else
                {
                    disc = 0;
                }
                #region PRIMEPOS-2768 02-Jan-2020 JY Added
                if (disc != 0)
                {
                    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                    {
                        bool AllowDisOfItemsOnSale = false;
                        AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                        if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                        {
                            if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                            {                                
                                oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * disc / totalPrice, MidpointRounding.AwayFromZero);
                            }
                        }
                        else if (AllowDisOfItemsOnSale == true)
                        {
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * disc / totalPrice, MidpointRounding.AwayFromZero);
                        }
                    }
                }
                #endregion
            }
            else
            {
                disc = oPOSTrans.GetDiscount(Convert.ToDecimal((Convert.ToDecimal(percentageValue) / 100 * totalPrice)));
            }
            return disc;
        }
                
        public decimal RemoveDiscWhenIndvHave(frmInvoiceDiscountInfo ofrmInvoiceDiscountInfo, decimal disc, out bool itemDiscount, string percentageValue = null)
        {
            decimal totalPrice = 0;
            itemDiscount = false;
            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                bool AllowDisOfItemsOnSale = false;
                AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                {
                    if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                    {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0)
                        {
                            itemDiscount = true;
                            AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);                            
                        }
                        else
                        {
                            if (Configuration.convertNullToDecimal(percentageValue) != 0)
                                oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * Configuration.convertNullToDecimal(percentageValue) / 100, 4, MidpointRounding.AwayFromZero);   //PRIMEPOS-2768 02-Jan-2020 JY Added
                        }
                    }
                }
                else if (AllowDisOfItemsOnSale == true)
                {
                    totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                    if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0)
                    {
                        itemDiscount = true;
                        AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);                        
                    }
                    else
                    {
                        if (Configuration.convertNullToDecimal(percentageValue) != 0)
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * Configuration.convertNullToDecimal(percentageValue) / 100, 4, MidpointRounding.AwayFromZero);   //PRIMEPOS-2768 02-Jan-2020 JY Added
                    }
                }
            }

            //if (percentageValue == null)  //PRIMEPOS-2768 03-Jan-2020 JY commented
            if (Configuration.convertNullToDecimal(percentageValue) == 0)   //PRIMEPOS-2904 07-Oct-2020 JY Added
            {
                if (itemDiscount == false && totalPrice > disc)
                    disc = oPOSTrans.GetDiscount(disc);
                else if (itemDiscount == false && totalPrice == disc && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    disc = oPOSTrans.GetDiscount(disc);
                else if (itemDiscount == true)
                {
                    string strMSG = "Above items have individual discount\nUnable to apply Invoice Discount. ";
                    ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                    ofrmInvoiceDiscountInfo.ShowDialog();
                    disc = 0;
                }

                #region PRIMEPOS-2768 02-Jan-2020 JY Added
                if (disc != 0)
                {
                    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                    {
                        bool AllowDisOfItemsOnSale = false;
                        AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                        if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                        {
                            if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                            {
                                if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                                {
                                    oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * disc / totalPrice, 4, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        else if (AllowDisOfItemsOnSale == true)
                        {
                            if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                            {
                                oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = Math.Round(Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) * disc / totalPrice, 4, MidpointRounding.AwayFromZero);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                if (itemDiscount == false)
                {
                    disc = oPOSTrans.GetDiscount(Convert.ToDecimal((Convert.ToDecimal(percentageValue) / 100 * totalPrice)));
                }
                else if (itemDiscount == true)
                {
                    string strMSG = "Above items have individual discount\nUnable to apply Invoice Discount. ";
                    ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                    ofrmInvoiceDiscountInfo.ShowDialog();
                    disc = 0;
                }
            }
            return disc;
        }

        //completed by sandeep
        public void ApplyDiscEA()
        {
            if (Configuration.convertNullToDecimal(this.txtAmtTotal.Text) == 0) {
                return;
            }
            string sUserID = string.Empty;  //PRIMEPOS-2402 08-Jul-2021 JY Added sUserID
            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DiscOverridefromPOSTrans.ID, UserPriviliges.Permissions.DiscOverridefromPOSTrans.Name, out sUserID)) 
            {
                strInvDiscOverrideUser = sUserID;   //PRIMEPOS-2402 08-Jul-2021 JY Added sUserID
                //Added By Shitaljit(QuicSolv) on 19 Sept 2011
                #region Validating Invoice Discount
                bool isValid = false;
                string strDialogResult = "";
                bool isPer = false;
                bool isDiscountableTrans = false;

                System.Decimal disc = 0;
                frmInvoiceDiscountInfo ofrmInvoiceDiscountInfo = new frmInvoiceDiscountInfo();
                ListViewItem oListViewItem;
                string strMSG = "";
                ofrmInvoiceDiscountInfo.lvItemList.Items.Clear();
                frmPOSDicount ofrm = new frmPOSDicount();
                ofrm.Text = "Invoice Discount";
                System.Decimal totalPrice = 0;

                if (Configuration.CPOSSet.AllItemDisc.Trim() == "3" && Configuration.CInfo.AllowHundredPerInvDiscount == false) {
                    bool itemDiscount = false;
                    foreach (UltraGridRow oGRow in this.grdDetail.Rows) {
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true) {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                            if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) != 0) {
                                itemDiscount = true;
                                AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);
                            }
                        }
                    }
                    if (itemDiscount == true) {
                        strMSG = "Above items have individual discount\nUnable to apply Invoice Discount. ";

                        ofrmInvoiceDiscountInfo.lblMSG.Text = strMSG;
                        ofrmInvoiceDiscountInfo.ShowDialog();
                        disc = 0;
                        return;
                    }
                }

                while (isValid == false) {
                    isValid = ValidateInvDiscount(out strDialogResult, out isPer, out disc, out isDiscountableTrans);
                    if (strDialogResult == "Cancel" || isDiscountableTrans == false) {
                        return;
                    }
                }

                if (isPer == true) {
                    ofrm.numDiscPerc.Value = disc.ToString();
                } else {
                    ofrm.numDiscAmount.Value = disc.ToString();
                }
                //End of Added By Shitaljit(QuicSolv) on 19 Sept 2011

                #endregion Validating Invoice Discount

                if (isValid == true) {
                    //Logic For 100% Invoice Discount is set to true and 100% Invoice Discount is applied
                    #region 100% Invoice Discount

                    if (IsHundredPerInvDisc == true) {
                        this.lblInvDiscount.Text = Configuration.convertNullToString(Convert.ToDecimal(this.txtAmtSubTotal.Text) - (Convert.ToDecimal(this.txtAmtDiscount.Text) + Convert.ToDecimal(this.lblInvDiscount.Text)));
                        RecalculateTax();
                        this.ultraCalcManager1.ReCalc();
                        InitPayment();
                        return;
                    }

                    #endregion 100% Invoice Discount
                    disc = 0;
                    totalPrice = 0;

                    #region Invoice discount given in percentange
                    if (Convert.ToDecimal(ofrm.numDiscPerc.Value) != 0)
                    {
                        // if (Configuration.CPOSSet.AllItemDisc.Trim() == "true")//Commented by shitaljit(Quicsolv) on 7 September
                        //Apply invoice discount to all the items (irrespective of the current individual item discount)
                        if (Configuration.CPOSSet.AllItemDisc.Trim() == "1")//if statement is added by shitaljit(Quicsolv) on 7 September
                        {
                            disc = ApplyDiscountToAllItem(disc, ofrm.numDiscPerc.Value.ToString()); //PRIMEPOS-2768 02-Jan-2020 JY Added "disc" parameter
                        }
                        //Apply invoice discount to the balance remaining after applying individual item discount
                        else if (Configuration.CPOSSet.AllItemDisc.Trim() == "0")//if statement is added by shitaljit(Quicsolv) on 7 September
                        {
                            disc = ApplyDiscountForBalRemaining(disc, ofrm.numDiscPerc.Value.ToString());
                        }
                        //Added by shitaljit(Quicsolv) on 7 September
                        //Remove individual item discount and apply the invoice discount to the transaction net amount
                        else if (Configuration.CPOSSet.AllItemDisc.Trim() == "2")
                        {
                            disc = ApplyDiscountAfterRemovingInd(disc, ofrm.numDiscPerc.Value.ToString());
                        }
                        //Do not allow invoice discount when individual item discounts are present in the transaction
                        else if (Configuration.CPOSSet.AllItemDisc.Trim() == "3")
                        {
                            bool itemDiscount = false;
                            disc = RemoveDiscWhenIndvHave(ofrmInvoiceDiscountInfo, disc, out itemDiscount, ofrm.numDiscPerc.Value.ToString());
                            if (itemDiscount) { return; }
                        }
                        this.lblInvDiscount.Text = disc.ToString();
                        RecalculateTax();
                        this.ultraCalcManager1.ReCalc();
                        InitPayment();
                    }
                    #endregion Invoice discount given in percentange
                    #region Invoice discount given in dollar amount
                    else
                    {
                        //Added by shitaljit(Quicsolv) on 7 September
                        disc = oPOSTrans.GetDiscount(Convert.ToDecimal(ofrm.numDiscAmount.Value));
                        if (disc != 0)
                        {
                            if ((disc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) > Convert.ToDecimal(this.txtAmtSubTotal.Text) && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CPOSSet.AllItemDisc != "2")
                            {
                                clsUIHelper.ShowErrorMsg("Total Discount amount " + Configuration.CInfo.CurrencySymbol.ToString() + (disc + Convert.ToDecimal(this.txtAmtDiscount.Text) - Convert.ToDecimal(this.lblInvDiscount.Text)) + " is greater than discountable amount " + Configuration.CInfo.CurrencySymbol.ToString() + totalPrice + "\nUnable to apply Invoice Discount");
                            }
                            else
                            {
                                //Apply invoice discount to all the items (irrespective of the current individual item discount)
                                if (Configuration.CPOSSet.AllItemDisc.Trim() == "1")
                                {
                                    //disc = oPOSTrans.GetDiscount(disc);   //PRIMEPOS-2768 02-Jan-2020 JY Commented
                                    disc = ApplyDiscountToAllItem(disc, ofrm.numDiscPerc.Value.ToString()); //PRIMEPOS-2768 02-Jan-2020 JY Added
                                }
                                //Apply invoice discount to only items which are not individually discounted.
                                else if (Configuration.CPOSSet.AllItemDisc.Trim() == "0")//if statement is added by shitaljit(Quicsolv) on 7 September
                                {
                                    disc = ApplyDiscountForBalRemaining(disc);
                                }
                                //Remove individual item discount and apply the invoice discount to the transaction net amount
                                else if (Configuration.CPOSSet.AllItemDisc.Trim() == "2") {
                                    disc = ApplyDiscountAfterRemovingInd(disc);
                                }
                                //Do not allow invoice discount when individual item discounts are present in the transaction
                                else if (Configuration.CPOSSet.AllItemDisc.Trim() == "3")
                                {
                                    bool itemDiscount = false;
                                    disc = RemoveDiscWhenIndvHave(ofrmInvoiceDiscountInfo, disc, out itemDiscount);
                                    if (itemDiscount) { return; }
                                }

                                this.lblInvDiscount.Text = disc.ToString();
                                RecalculateTax();
                                this.ultraCalcManager1.ReCalc();
                                InitPayment();
                            }
                        }
                        //End of Added By shitaljit on 7 Sept 2011
                        #endregion Invoice discount given in dollar amount
                    }
                }
            }
        }

        public void AddItemToInvDisc(ref frmInvoiceDiscountInfo ofrmInvoiceDiscountInfo, UltraGridRow oGRow)
        {
            ListViewItem oListViewItem;
            oListViewItem = ofrmInvoiceDiscountInfo.lvItemList.Items.Add(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
            oListViewItem.SubItems.Add(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemDescription].Text.ToString());
            oListViewItem.SubItems.Add(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Text.ToString());
        }


        private decimal GetDiscountableTotal(ref frmInvoiceDiscountInfo ofrmInvoiceDiscountInfo, ref bool isDiscountableItemPresnt, bool AllowDisOfItemsOnSale, UltraGridRow oGRow)
        {
            decimal totalPrice = 0;

            if (Configuration.CPOSSet.AllItemDisc.Trim() == "0")
            {
                if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0 && AllowDisOfItemsOnSale == true)
                {
                    totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                    AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);
                }
                isDiscountableItemPresnt = true;
            }
            else if (AllowDisOfItemsOnSale == true)
            {
                totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                AddItemToInvDisc(ref ofrmInvoiceDiscountInfo, oGRow);
                isDiscountableItemPresnt = true;
            }
            return totalPrice;
        }

        //completed by sandeep
        public void AddCouponEA()
        {
            #region Add Coupon
            CouponItemDesc = string.Empty;  //Sprint-23 - PRIMEPOS-2279 19-Mar-2016 JY Added
            CouponID = 0;   //PRIMEPOS-2034 05-Mar-2018 JY Added
            if (Configuration.convertNullToDecimal(this.txtAmtTotal.Text) > 0) {
                frmPOSCoupon ofrmCoupon = new frmPOSCoupon();
                ofrmCoupon.bCalledFromTrans = true; //Sprint-23 - PRIMEPOS-2280 05-May-2016 JY Added to know whether form called from transaction screen
                ofrmCoupon.EnableDisableControls(false);  //Sprint-23 - PRIMEPOS-2280 05-May-2016 JY Added bStatus flag to enable/disable controls
                ofrmCoupon.btnSave.Text = "&OK";
                ofrmCoupon.Text = "Scan Coupon";
                ofrmCoupon.ControlBox = false;
                DialogResult dr = ofrmCoupon.ShowDialog();
                if (dr == DialogResult.OK && Configuration.CInfo.ApplyInvDiscSettingsForCoupon == false) {
                    CouponItemDesc = Configuration.convertNullToString(ofrmCoupon.txtCouponCode.Text) + "-" + Configuration.convertNullToString(ofrmCoupon.txtDesc.Text); //Sprint-23 - PRIMEPOS-2279 19-Mar-2016 JY Added
                    CouponID = Configuration.convertNullToInt64(ofrmCoupon.txtCouponID.Text);   //PRIMEPOS-2034 05-Mar-2018 JY Added
                    if (ofrmCoupon.DiscountPercent > 0) {
                        if (AddCouponToTrans(-1 * ofrmCoupon.DiscountPercent, ofrmCoupon.TransCouponRow.IsCouponInPercent) == true) {
                            RecalculateTax("Coupon");   //Sprint-21 - PRIMEPOS-2226 18-Feb-2016 JY Added parameter for coupon discount
                            this.ultraCalcManager1.ReCalc();
                            InitPayment();
                        }
                    }
                } else if (dr == DialogResult.OK && Configuration.CInfo.ApplyInvDiscSettingsForCoupon == true) {
                    CouponItemDesc = Configuration.convertNullToString(ofrmCoupon.txtCouponCode.Text) + "-" + Configuration.convertNullToString(ofrmCoupon.txtDesc.Text); //Sprint-23 - PRIMEPOS-2279 19-Mar-2016 JY Added
                    CouponID = Configuration.convertNullToInt64(ofrmCoupon.txtCouponID.Text);   //PRIMEPOS-2034 05-Mar-2018 JY Added
                    if (ofrmCoupon.DiscountPercent > 0 && ofrmCoupon.TransCouponRow != null) {
                        decimal CouponDiscVal = 0;
                        if (ValidateCouponDiscount(ofrmCoupon.DiscountPercent, out CouponDiscVal) == true) {
                            if (CouponDiscVal > 0) {
                                if (AddCouponToTrans(-1 * ofrmCoupon.DiscountPercent, ofrmCoupon.TransCouponRow.IsCouponInPercent) == true) //PRIMEPOS-2768 06-Jan-2020 JY modified
                                {
                                    //RecalculateTax();
                                    RecalculateTax("Coupon");   //PRIMEPOS-2768 06-Jan-2020 JY Added to resolve issue with the calculating total
                                    this.ultraCalcManager1.ReCalc();
                                    InitPayment();
                                }
                            }
                        }
                    }
                }
            }

            #endregion Add Coupon
        }

        //completed by sandeep
        private bool AddCouponToTrans(decimal CouponAmt, bool isPerValue)
        {
            try {
                logger.Trace("AddCouponToTrans() - " + clsPOSDBConstants.Log_Entering);
                bool retValue = false;
                ItemData oIData = oPOSTrans.PopulateItem(Configuration.CouponItemCode);
                string sCouponAmt = string.Empty;
                if (oIData.Item.Rows.Count > 0) {
                    decimal TotalAmtTotal = Configuration.convertNullToDecimal(this.txtAmtSubTotal.Text);  //Sprint-21 - PRIMEPOS-2226 18-Feb-2016 JY replaced "txtAmtTotal" by "txtAmtSubTotal" as coupon discount should apply on the total selling price without tax
                    if (isPerValue == true) {
                        oIData.Item[0].SellingPrice = Math.Round(TotalAmtTotal / 100 * CouponAmt, 2);
                    } else {
                        oIData.Item[0].SellingPrice = CouponAmt;
                    }
                    //Added by shitaljit to remove cash back item if charge amount is $0.00
                    if (Math.Abs(Math.Round(oIData.Item[0].SellingPrice, 2)) == 0) {
                        txtItemCode.Text = "";
                        return retValue;
                    }
                    if (Math.Abs(oIData.Item[0].SellingPrice) > 0) {
                        sCouponAmt = Configuration.convertNullToString(Math.Round(oIData.Item[0].SellingPrice, 2));
                        if (sCouponAmt.Contains(".") == false) {
                            sCouponAmt += ".00";
                        }
                        txtItemCode.Text = "1/" + sCouponAmt + "@" + Configuration.CouponItemCode;
                    }
                    oIData.Item[0].Description = CouponItemDesc;    //Sprint-23 - PRIMEPOS-2279 19-Mar-2016 JY Added
                    ItemBox_Validatiang(txtItemCode, new CancelEventArgs());
                    lblCouponDiscount.Text = sCouponAmt;
                    retValue = true;
                }
                logger.Trace("AddCouponToTrans() - " + clsPOSDBConstants.Log_Exiting);
                return retValue;
            } catch (Exception Ex) {
                throw Ex;
            }
        }

        #region ItemDiscount
        //done by sandeep
        private void ValidateItemDicount(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try {
                #region discount

                if (this.txtDiscount.Value == null) {
                    this.oPOSTrans.oTDRow.Discount = 0;
                } else {
                    if (this.isAddRow == true) {
                        this.oPOSTrans.oTDRow.Discount = Configuration.convertNullToDecimal(this.txtDiscount.Value.ToString());
                    }
                }

                string sTaxCodes = string.Empty;
                if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxCodes, oPOSTrans.oTDRow.TransDetailID) == true) //PRIMEPOS-2500 JY Changed
                {
                    EditTax(oPOSTrans.oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                }
                if (this.isEditRow == true) {
                    ValidateRow(sender, e);
                    ClearItemRow();
                    grdDetail.Focus();
                }

                #endregion discount
            } catch (Exception Ex) {
                logger.Fatal(Ex, "ValidateItemDicount()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        #endregion

    }
}
