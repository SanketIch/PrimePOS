using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
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

        #region POSPayTypeList
        //method will be called from frmPOSPaytypeList
        public bool RevomeTaxFromEBTItems()
        {
            logger.Trace("RevomeTaxFromEBTItems() - " + clsPOSDBConstants.Log_Entering);
            bool RetVal = false;
            decimal TotalEBTItemsTax = 0;
            try {
                foreach (UltraGridRow oGRow in this.grdDetail.Rows) {
                    if (Configuration.convertNullToBoolean(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_IsEBT].Value) == true) {
                        this.grdDetail.ActiveRow = oGRow;
                        TotalEBTItemsTax += Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value);
                        oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value = 0;
                        this.cmbTaxCode.Text = "N";
                        taxoverflag = 1;
                        tbEditItemActions("removeindTax");
                        taxoverflag = 0;
                    }
                }
                foreach (TransDetailRow oRow in this.oPOSTrans.oTransDData.TransDetail.Rows) {
                    if (oRow.IsEBTItem == true) {
                        oRow.TaxAmount = 0;
                    }
                }
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                this.grdDetail.UpdateData();
                RecalculateTax();
                this.Refresh();
                taxoverflag = 0;
                Application.DoEvents();
                RetVal = true;
            } catch (Exception Ex) {
                RetVal = false;
                logger.Fatal(Ex, "RevomeTaxFromEBTItems()");
                throw Ex;
            }
            logger.Trace("RevomeTaxFromEBTItems() - " + clsPOSDBConstants.Log_Exiting);
            return RetVal;
        }

        //method will be called from frmPOSPaytypeList
        public void RemoveCashBack(ref decimal totalAmount)
        {
            logger.Trace("RemoveCashBack() - " + clsPOSDBConstants.Log_Entering);
            foreach (TransDetailRow oTDRow in oPOSTrans.oTransDData.TransDetail.Rows) {
                if (oTDRow.ItemID.ToUpper() == "CBC") {
                    oTDRow.Delete();
                    grdDetail.Update();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);
                    totalAmount = Math.Round(Convert.ToDecimal(this.txtAmtTotal.Text), 2);
                    break;
                }
            }
            logger.Trace("RemoveCashBack() - " + clsPOSDBConstants.Log_Exiting);
        }
        //method will be called from frmPOSPaytypeList
        public bool ChargeCashBack(decimal cashBackAmount, ref decimal totalAmount)
        {
            logger.Trace("ChargeCashBack() - " + clsPOSDBConstants.Log_Entering);
            bool retValue = false;
            ItemSvr oItemSvr = new ItemSvr();
            ItemData oIData = oItemSvr.Populate("CBC");
            if (oIData.Item.Rows.Count > 0) {
                string oldAmtTotal = this.txtAmtTotal.Text;

                txtItemCode.Text = "1/CBC";
                //Cashback Chaarge MODE- P= % , A= $,N= NO charges.
                //Added By Shitaljit for PrimePOS JIRA- 800
                if (Configuration.CInfo.ChargeCashBackMode == "P") {
                    //Added by shitaljit to remove cash back item if charge amount is $0.00
                    if (Math.Round(oIData.Item[0].SellingPrice / 100 * cashBackAmount, 2) == 0) {
                        txtItemCode.Text = "";
                        return retValue;
                    }
                    if (oIData.Item[0].SellingPrice > 0) {
                        txtItemCode.Text = "1/" + Math.Round(oIData.Item[0].SellingPrice / 100 * cashBackAmount, 2) + "@CBC";
                    }
                }

                ItemBox_Validatiang(txtItemCode, new CancelEventArgs());
                if (this.txtAmtTotal.Text == oldAmtTotal) {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(500);
                    System.Windows.Forms.Application.DoEvents();
                }
                totalAmount = Math.Round(Convert.ToDecimal(this.txtAmtTotal.Text), 2);
                retValue = true;
            }
            logger.Trace("ChargeCashBack() - " + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }
        #endregion

    }
}
