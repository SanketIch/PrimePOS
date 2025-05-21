using Infragistics.Win.UltraWinGrid;
using MMS.Device;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.ErrorLogging;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_Core.Resources.PaymentHandler;
using POS_Core.LabelHandler;
using POS_Core.LabelHandler.RxLabel;
using PharmData;
using System.Collections;
using Phw;
using MMSInterfaceLib.Common;
using MMSInterfaces.Helpers;
using PrimeRx.Models;
using MMSInterfaceLib.Models;
using MMSInterfaceLib.Utils;
using static MMSInterfaces.Helpers.EventHub;

namespace POS_Core_UI.UI
{
    public partial class frmPOSTransaction
    {
        DataTable dtSelectedRx = new DataTable(); //PRIMEPOS-3192
        Boolean bOneTimePerTrans = false;
        List<string> lstPatientNos = new List<string>(); //PRIMEPOS-2547 12-Jul-2018 JY Added
        public string controlNumber = string.Empty;//PRIMEPOS-2664 
        private bool isPSEItem = false;//PRIMEPOS-3109
        #region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019
        public bool IsStoreCredit = false;
        #endregion

        #region PRIMEPOS-2402 12-Jul-2021 JY Added
        private string strOverrideMaxStationCloseCashLimit = string.Empty;
        private string strMaxTransactionAmountUser = string.Empty;
        private string strMaxReturnTransactionAmountUser = string.Empty;
        private string strInvDiscOverrideUser = string.Empty;
        private string strMaxDiscountLimitOverrideUser = string.Empty;
        #endregion

        #region PRIMEPOS-3403
        public enum RxOrigin
        {
            NotSpecified = 0,
            Written = 1,
            Telephone = 2,
            Electronic = 3,
            Facsimile = 4,
            Pharmacy = 5
        }
        #endregion

        #region PaymentEvent
        //completed by sandeep
        private void btnCancelTrans_Click(object sender, EventArgs e)
        {
            #region BatchDelivery - NileshJ - PRIMERX-7688  
            //reset the Batch related variables as txn is cancelled
            isBatchDelivery = false;
            alreadyPaidAmount = 0;
            //allowUnPickedRX = string.Empty;
            #endregion

            //Edited on 6 May 2011 to Disable the Login Screen
            //following if-else is Added by shitaljit on 13 March 2012 to check user Priviliges

            if (this.grdDetail.Rows.Count > 0)
            {
                if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteTrans.ID, UserPriviliges.Permissions.DeleteTrans.Name))
                {
                    if (Resources.Message.Display("Are you sure, you want to clear the screen?", "Clear screen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        #region PRIMEPOS-3207
                        clearHyphenAlert();
                        #endregion
                        SetNew(false);
                        if (Configuration.CPOSSet.PaymentProcessor == "ELAVON")//2943
                        {
                            Elavon.ElavonProcessor elavonProcessor = Elavon.ElavonProcessor.getInstance("", 0);
                            elavonProcessor.IsIdleScreenAdded = true;
                            elavonProcessor.IdleScreen();
                            elavonProcessor.IsIdleScreenAdded = false;
                        }
                        if (Configuration.CPOSSet.UsePoleDisplay)
                        {
                            frmMain.PoleDisplay.ClearPoleDisplay();
                        }
                        clsUIHelper.ShowWelcomeMessage();
                    }
                }
            }
            else
            {
                #region PRIMEPOS-3207
                clearHyphenAlert();
                #endregion
                SetNew(false);
                this.txtItemCode.Focus();
            }

            //Added by shitaljit 0n 15 march 2012 to move focus from Cancell Trans (Clear All ) button
        }

        //completed by sandeep
        private void btnPayment_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count > 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    if (!RecordPatientCounseling())
                        return;
                }
                #region PRIMEPOS-3207
                clearHyphenAlert();
                #endregion
                if (aboutToOpenDrawer)
                    throw new Exception("Cash Drawer is opening. Please wait");

                InitPayment();
                this.txtItemCode.Focus();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnPayment_Click()");
                //commented by SRT(Abhishek) as it was showing error of signature whatever might be the issue
                //MessageBox.Show("Exception while Rx signature is cancelled\r\n" + ex.ToString());
                //Added by SRT(Abhishek) Date : 25 Aug 2009
                clsUIHelper.ShowErrorMsg(ex.Message);
                //End of Added by SRT(Abhishek) Date : 25 Aug 2009
            }
        }
        #endregion 


        #region Payment
        private void InitPayment()
        {
            try
            {
                pseTrxId = string.Empty;    //Sprint-23 - PRIMEPOS-2029 27-Apr-2016 JY Added
                bCaptureSignature = false;  //PRIMEPOS-2605 27-Oct-2018 JY Added
                bItemMonitorInTrans = false;
                TransFeeAmt = 0;    //PRIMEPOS-3117 11-Jul-2022 JY Added

                //if (SigPadUtil.DefaultInstance.InMethod)//2730 Arvind Commented because the screen was not goin until it gets fully loaded 
                //    return;

                logger.Trace("InitPayment() - " + clsPOSDBConstants.Log_Entering);

                #region PRIMEPOS-3093 24-May-2022 JY Added
                try
                {
                    if (oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count > 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CInfo.AllowUnPickedRX == "0")
                    {
                        List<string> lstRxNo = new List<string>();
                        ArrayList alPatientNo = new ArrayList();
                        foreach (DataRow row in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                        {
                            string strTemp = Configuration.convertNullToString(row["RXNo"]) + "-" + Configuration.convertNullToString(row["NRefill"]);
                            if (!lstRxNo.Contains(strTemp))
                                lstRxNo.Add(strTemp);
                            if (!alPatientNo.Contains(row["PatientNo"].ToString()))
                                alPatientNo.Add(row["PatientNo"].ToString());
                        }
                        DataTable dtUnpickedRxs = FillUnPickedRXs(alPatientNo);
                        if (lstRxNo.Count > 0)
                        {
                            List<DataRow> rowsToDelete = new List<DataRow>();
                            foreach (DataRow row in dtUnpickedRxs.Rows)
                            {
                                string strTemp = Configuration.convertNullToString(row["Rx No"]) + "-" + Configuration.convertNullToString(row["Refill No"]);
                                if (lstRxNo.Contains(strTemp))
                                {
                                    rowsToDelete.Add(row);
                                }
                            }

                            foreach (DataRow row in rowsToDelete)
                            {
                                dtUnpickedRxs.Rows.Remove(row);
                            }

                            dtUnpickedRxs.AcceptChanges();
                        }

                        if (dtUnpickedRxs != null && dtUnpickedRxs.Rows.Count > 0)
                        {
                            frmTaxBreakDown ofrmTaxBreakDown = new frmTaxBreakDown(dtUnpickedRxs, FormDataOnScreen.UnPickedRxs);
                            DialogResult dr = ofrmTaxBreakDown.ShowDialog();
                            if (dr == DialogResult.OK)
                                return;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "InitPayment() - Show unpicked Rxs");
                }
                #endregion

                #region PRIMEPOS-2651 08-Apr-2022 JY Added
                try
                {
                    if (Configuration.convertNullToBoolean(Configuration.CSetting.NotifyRefrigeratedMedication) == true && oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count > 0)
                    {
                        List<int> lstTransDetailID = new List<int>();
                        bool bIsDrugRefrigerated = false;
                        PharmBL oPharmBL = new PharmBL();
                        foreach (TransDetailRXRow tdRxRow in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                        {
                            bIsDrugRefrigerated = oPharmBL.IsDrugRefrigerated(Configuration.convertNullToString(tdRxRow.DrugNDC));
                            if (bIsDrugRefrigerated)
                                lstTransDetailID.Add(tdRxRow.TransDetailID);
                        }
                        if (lstTransDetailID.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Item Description", typeof(string));
                            foreach (TransDetailRow tdRow in oPOSTrans.oTransDData.TransDetail.Rows)
                            {
                                if (lstTransDetailID.Contains(tdRow.TransDetailID))
                                {
                                    dt.Rows.Add(tdRow.ItemDescription);
                                }
                            }
                            frmTaxBreakDown ofrmTaxBreakDown = new frmTaxBreakDown(dt, FormDataOnScreen.RefrigeratedDrugsData); //PRIMEPOS-2651 08-Apr-2022 JY Modified
                            ofrmTaxBreakDown.ShowDialog();
                        }
                    }
                }
                catch (Exception Ex)
                {
                }
                #endregion

                #region PRIMEPOS-2760 18-Nov-2019 JY Added logic to verify line items "Rx" exists in "PrimeRx"
                if (oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count > 0)
                {
                    string strMessage = string.Empty;
                    if (oPOSTrans.RxExistsInPrimeRxDb(out strMessage) == true)
                    {
                        Resources.Message.Display(strMessage, "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                #endregion

                #region PRIMEPOS-2592 05-Nov-2018 JY Added logic to check IsNonRefundable item exists in the transaction, if yes, then bring up message and exit
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    string strItemDesc = string.Empty;
                    foreach (TransDetailRow tdRow in oPOSTrans.oTransDData.TransDetail.Rows)
                    {
                        if (Configuration.convertNullToBoolean(tdRow.IsNonRefundable) == true)
                        {
                            if (strItemDesc == string.Empty)
                                strItemDesc = Configuration.convertNullToString(tdRow.ItemDescription);
                            else
                                strItemDesc += "," + Configuration.convertNullToString(tdRow.ItemDescription);
                        }
                    }
                    #region - Solutran PRIMEPOS-2663 -  NileshJ - If S3 product is available in TransactionDetailTable then Show following Message
                    try
                    {//PRIMEPOS-2699 27-Aug-2019 JY Added try-catch to bypass exception caused due to "null/blank" S3TransID
                        bool S3exists = oPOSTrans.oTransDData.TransDetail.AsEnumerable().Where(c => c.Field<Int64>("S3TransID") != (0)).Count() > 0; //PRIMEPOS-3265
                        if (S3exists)
                        {
                            Resources.Message.Display("This transaction was processed with an S3 Card payment.\nOnce a S3 Card refund is started, then it cannot be aborted\nand the entire transaction must be completed", "Solutran", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch { }
                    #endregion
                    if (strItemDesc != string.Empty)
                    {
                        Resources.Message.Display(strItemDesc + Environment.NewLine + "Item(s) are non-refundable", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                #endregion

                POSSET oPOSSet;
                oPOSSet = Configuration.CPOSSet;

                this.ultraCalcManager1.ReCalc(50);
                if (DisplayWarningMSGForHugeTransAmt() == false)
                {
                    return;
                }
                if (!oPOSSet.AllowZeroAmtTransaction && double.Parse(this.txtAmtTotal.Text) == 0)
                {
                    /*Date 27-jan-2014
                    * Modified by Shitaljit
                    * For making currency symbol dynamic
                    */
                    //old code
                    //clsUIHelper.ShowErrorMsg("$0.00 transaction not permitted");//Added By Shitaljit(QuicSolv) on Sept 2011
                    // new code
                    clsUIHelper.ShowErrorMsg(Configuration.CInfo.CurrencySymbol.ToString() + "0.00 transaction not permitted");
                    //Added By shitaljit to resolve JIRA 415 bug
                    this.lblInvDiscount.Text = "0.00";
                    return;
                }

                #region Populate Default cust if there is no customer at transaction

                if (string.IsNullOrEmpty(this.txtCustomer.Text) == true)
                {
                    this.txtCustomer.Text = "-1";
                    GetCustomer(this.txtCustomer.Text, true);
                }

                #endregion Populate Default cust if there is no customer at transaction

                #region Check Max Cash Limit for Station close
                //MaxStnCloseCashLimitThread = new Thread(() => { bContinuePayment = CheckStationCloseCashLimit(); });
                //MaxStnCloseCashLimitThread.Start();
                #endregion Check Max Cash Limit for Station close
                #region PRIMEPOS-2856 01-Jun-2020 JY Added to Check Max Cash Limit for Station close
                string strUserName = Configuration.UserName;
                bContinuePayment = true;
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.OverrideMaxStationCloseCashLimit.ID, strUserName) == false)
                {
                    bContinuePayment = CheckStationCloseCashLimit();
                }
                #endregion 

                if (this.grdDetail.Rows.Count != 0 || this.IsCustomerDriven)//2915
                {
                    //Added by SRT(Abhishek) Date : 19 Aug 2009
                    //check if rx is unbilled
                    //if rx is unbilled then it will popup message whether to process that rx
                    //if yes then move ahead else it will return and show transaction screen.
                    if (oPOSTrans.countUnBilledRx > 0)
                    {
                        if (Resources.Message.Display("One or more Rx(s) in the Transaction are UNBILLED (" + oPOSTrans.unbilledRx + ").\nDo you want to Process Unbilled Rx in Transaction?", "Unbilled Rx", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    //End of Added by SRT(Abhishek) Date:19 Aug 2009

                    ItemRow oIRow = null;
                    #region Sprint-25 - PRIMEPOS-2380 13-Feb-2017 JY Now onwards we will have all PSE items in PSE_Items table, so added logic to check Item over there
                    bool bPSEItemForNplex = false, bPSEItemForManual = false;
                    DataTable dtPSE_Items = new DataTable();
                    if (Configuration.CInfo.useNplex == true)   //Need to send the item to nplex for verification
                    {
                        dtPSE_Items = oPOSTrans.checkPSEItems();
                        if (dtPSE_Items.Rows.Count > 0)
                        {
                            isPSEItem = true;//PRIMEPOS-3109
                            bPSEItemForNplex = true;
                            bItemMonitorInTrans = true; //Used to save data in ItemMonitorTransDetail table
                        }
                        #region PRIMEPOS-2525 23-May-2018 JY added logic to handle monitored item scenario when Nplex settings is on      
                        oIRow = oPOSTrans.checkOTCItems();
                        if (oIRow != null)
                        {
                            bPSEItemForManual = true;
                            bItemMonitorInTrans = true;
                        }
                        #endregion
                    }
                    #endregion
                    else //else use manual item monitoring logic
                    {
                        oIRow = oPOSTrans.checkOTCItems();
                        if (oIRow != null)
                        {
                            bPSEItemForManual = true;
                            bItemMonitorInTrans = true; //Sprint-23 - PRIMEPOS-2029 20-Apr-2016 JY Added
                        }
                        #region PRIMEPOS-3109 
                        dtPSE_Items = oPOSTrans.checkPSEItems();
                        if (dtPSE_Items.Rows.Count > 0)
                        {
                            isPSEItem = true;//PRIMEPOS-3109 
                        }
                        #endregion
                    }

                    if ((bPSEItemForNplex == true || bPSEItemForManual == true) && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                    {
                        if (bPSEItemForNplex == true)
                        {
                            clsUIHelper.ShowErrorMsg(dtPSE_Items.Rows[0]["Description"].ToString().Trim() + "- is PSE Item,\n" + "Please Select a Customer other than Default.");
                            ClearCustomer();
                            SearchCustomer(false);
                            if (oPOSTrans.oCustomerRow.AccountNumber.ToString().Trim() == "-1")
                                return;
                            if (this.oPOSTrans.oCustomerRow == null)
                            {
                                GetCustomer("-1", true);
                                return;
                            }
                            bool bStatus = GetPSEItemsVerificationDetails();
                            if (bStatus == false)
                                return;   //PRIMEPOS-2693 13-Jun-2019 JY Added

                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Boolean isValid = false;
                                isValid = DoNplexProcessPSE();
                                if (isValid == false)
                                    return;
                            }
                            tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                            if (IsOTCsignCancelled == true || ofrmPOSPayAuthNo.isCancelled == true)
                            {
                                return;
                            }
                            if (Configuration.CInfo.ForceCustomerInTrans == true && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                            {
                                clsUIHelper.ShowErrorMsg("Please Select a Customer other than Default.");
                                txtCustomer.Focus();
                            }
                            else
                            {
                                oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                                this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                                this.ChangeDue = 0;
                                showPayTypes();
                            }
                        }
                        else
                        {
                            if (bPSEItemForManual == true)
                            {
                                oPOSTrans.oIMCDetailRow = oPOSTrans.GetItemMonitoringDetails(oIRow.ItemID.Trim());//Added By Shitaljit(QuicSolv) 0n 5 oct 2011
                                if (oPOSTrans.oIMCDetailRow != null)
                                {
                                    oPOSTrans.oIMCategoryRow = oPOSTrans.GetItemMonitoringCategoryRow(oPOSTrans.oIMCDetailRow.ItemMonCatID);
                                    //Edited the message add oIMCDetailRow.Description.Trim() By Shitaljit(QuicSolv) 0n 5 oct 2011
                                    clsUIHelper.ShowErrorMsg(oIRow.Description.Trim() + ", Item is Marked for Monitoring,\n" + oPOSTrans.oIMCDetailRow.Description.Trim() + "\nPlease Select a Customer other than Default.");
                                    //Added By Shitaljit on 5 May 2012
                                    ClearCustomer();
                                    SearchCustomer(false);

                                    if (oPOSTrans.oCustomerRow.AccountNumber.ToString().Trim() == "-1")
                                        return;   //PRIMEPOS-2029 11-Aug-2016 JY Added code to restrict default customer selection for monitored item transaction                                    

                                    if (this.oPOSTrans.oCustomerRow == null)
                                    {
                                        GetCustomer("-1", true);
                                        return;
                                    }
                                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                                    {
                                        oPOSTrans.ValidateOTCItems(txtCustomer.Tag.ToString());
                                    }
                                    GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual parameter

                                    tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                                    if (IsOTCsignCancelled == true || ofrmPOSPayAuthNo.isCancelled == true)
                                        return;

                                    if (Configuration.CInfo.ForceCustomerInTrans == true && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                                    {
                                        clsUIHelper.ShowErrorMsg("Please Select a Customer other than Default.");
                                        txtCustomer.Focus();
                                    }
                                    else
                                    {
                                        oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                                        this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                                        this.ChangeDue = 0;
                                        showPayTypes();
                                    }
                                }
                            }
                            else
                            {
                                oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                                this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                                this.ChangeDue = 0;
                                showPayTypes();
                            }
                        }
                    }
                    //Following esle if is Added By Shitaljit on 5 May 2012
                    else if ((bPSEItemForNplex == true || bPSEItemForManual == true) && tempCustId != oPOSTrans.oCustomerRow.CustomerId && oPOSTrans.oCustomerRow.CustomerId != -1)
                    {
                        oPOSTrans.oTransSignLogData.Clear();
                        if (bPSEItemForNplex == true)
                        {
                            bool bStatus = GetPSEItemsVerificationDetails();
                            if (bStatus == false)
                                return;   //PRIMEPOS-2693 13-Jun-2019 JY Added

                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Boolean isValid = false;
                                isValid = DoNplexProcessPSE();
                                if (isValid == false)
                                    return;
                            }
                        }
                        else if (bPSEItemForManual == true)
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                oPOSTrans.ValidateOTCItems(txtCustomer.Tag.ToString());
                            }
                            GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual parameter
                        }

                        tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                        if (IsOTCsignCancelled == true || ofrmPOSPayAuthNo.isCancelled == true)
                            return;

                        if (Configuration.CInfo.ForceCustomerInTrans == true && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                        {
                            clsUIHelper.ShowErrorMsg("Please Select a Customer other than Default.");
                            txtCustomer.Focus();
                        }
                        else
                        {
                            oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                            this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                            this.ChangeDue = 0;
                            showPayTypes();
                        }
                    }
                    //Following else if is Added By Shitaljit on 5 May 2012
                    //to make sure customer enter details or signing if there is OTC items for which signing or entering ID details is necessery
                    else if ((oPOSTrans.isByBoth == true || oPOSTrans.isBySignPresent == true) && (OTCSignDataBinary == null && this.SigType == clsPOSDBConstants.BINARYIMAGE))
                    {
                        oPOSTrans.oTransSignLogData.Clear();
                        if (bPSEItemForNplex == true)
                        {
                            bool bStatus = GetPSEItemsVerificationDetails();
                            if (bStatus == false)
                                return;   //PRIMEPOS-2693 13-Jun-2019 JY Added

                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Boolean isValid = false;
                                isValid = DoNplexProcessPSE();
                                if (isValid == false)
                                    return;
                            }
                        }
                        else if (bPSEItemForManual == true)
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                oPOSTrans.ValidateOTCItems(txtCustomer.Tag.ToString());
                            }
                            GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual parameter
                        }

                        tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                        if (IsOTCsignCancelled == true || ofrmPOSPayAuthNo.isCancelled == true)
                            return;

                        if (Configuration.CInfo.ForceCustomerInTrans == true && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                        {
                            clsUIHelper.ShowErrorMsg("Please Select a Customer other than Default.");
                            txtCustomer.Focus();
                        }
                        else
                        {
                            oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                            this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                            this.ChangeDue = 0;
                            showPayTypes();
                        }
                    }
                    else if ((oPOSTrans.isByBoth == true || oPOSTrans.isBySignPresent == true) && (oPOSTrans.OTCSignDataText == "" && this.SigType != clsPOSDBConstants.BINARYIMAGE))
                    {
                        oPOSTrans.oTransSignLogData.Clear();
                        if (bPSEItemForNplex == true)
                        {
                            bool bStatus = GetPSEItemsVerificationDetails();
                            if (bStatus == false)
                                return;   //PRIMEPOS-2693 13-Jun-2019 JY Added

                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Boolean isValid = false;
                                isValid = DoNplexProcessPSE();
                                if (isValid == false)
                                    return;
                            }
                        }
                        else if (bPSEItemForManual == true)
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                oPOSTrans.ValidateOTCItems(txtCustomer.Tag.ToString());
                            }

                            GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual parameter
                        }

                        tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                        if (IsOTCsignCancelled == true || ofrmPOSPayAuthNo.isCancelled == true)
                        {
                            return;
                        }
                        if (Configuration.CInfo.ForceCustomerInTrans == true && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                        {
                            clsUIHelper.ShowErrorMsg("Please Select a Customer other than Default.");
                            txtCustomer.Focus();
                        }
                        else
                        {
                            oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                            this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                            this.ChangeDue = 0;
                            showPayTypes();
                        }
                    }
                    else if (oPOSTrans.isByDLPresent == true && ofrmPOSPayAuthNo.isCancelled == true && ofrmPOSPayAuthNo.txtAuthorizationNo.Text == "")
                    {
                        oPOSTrans.oTransSignLogData.Clear();
                        if (bPSEItemForNplex == true)
                        {
                            bool bStatus = GetPSEItemsVerificationDetails();
                            if (bStatus == false)
                                return;   //PRIMEPOS-2693 13-Jun-2019 JY Added

                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                Boolean isValid = false;
                                isValid = DoNplexProcessPSE();
                                if (isValid == false)
                                    return;
                            }
                        }
                        else if (bPSEItemForManual == true)
                        {
                            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                oPOSTrans.ValidateOTCItems(txtCustomer.Tag.ToString());
                            }
                            GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual parameter
                        }

                        tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                        if (IsOTCsignCancelled == true || ofrmPOSPayAuthNo.isCancelled == true)
                        {
                            return;
                        }
                        if (Configuration.CInfo.ForceCustomerInTrans == true && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                        {
                            clsUIHelper.ShowErrorMsg("Please Select a Customer other than Default.");
                            txtCustomer.Focus();
                        }
                        else
                        {
                            oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                            this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                            this.ChangeDue = 0;
                            showPayTypes();
                        }
                    }
                    else
                    {
                        //Added By shitaljit to bypass OTC item quantity verification in case of return transaction.
                        if (this.oPOSTrans.oCustomerRow != null && txtCustomer.Text.Trim() != "" && txtCustomer.Text.Trim() != "-1" && oIRow != null)
                        {
                            oPOSTrans.oTransSignLogData.Clear();
                            if (bPSEItemForNplex == true)
                            {
                                bool bStatus = GetPSEItemsVerificationDetails();
                                if (bStatus == false)
                                    return;   //PRIMEPOS-2693 13-Jun-2019 JY Added

                                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                                {
                                    Boolean isValid = false;
                                    isValid = DoNplexProcessPSE();
                                    if (isValid == false)
                                        return;
                                }
                            }
                            else if (bPSEItemForManual == true)
                            {
                                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                                {
                                    oPOSTrans.ValidateOTCItems(txtCustomer.Tag.ToString());
                                }
                                GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual parameter
                            }

                            if (IsOTCsignCancelled == true || ofrmPOSPayAuthNo.isCancelled == true)
                            {
                                return;
                            }
                        }
                        if (Configuration.CInfo.ForceCustomerInTrans == true && (txtCustomer.Text.Trim() == "" || txtCustomer.Text.Trim() == "-1"))
                        {
                            clsUIHelper.ShowErrorMsg("Please Select a Customer other than Default.");
                            txtCustomer.Focus();
                        }
                        else if (this.IsCustomerDriven)//2915
                        {
                            //oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                            this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                            this.ChangeDue = 0;
                            showPayTypes();
                        }
                        else//2915
                        {
                            oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                            this.txtAmtBalanceDue.Text = this.txtAmtTotal.Text;
                            this.ChangeDue = 0;
                            showPayTypes();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "InitPayment()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            logger.Trace("InitPayment() - " + clsPOSDBConstants.Log_Exiting);
        }

        #region PRIMEPOS-3093 24-May-2022 JY Added
        private DataTable FillUnPickedRXs(ArrayList alPatientNo)
        {
            logger.Trace("FillUnPickedRXs(ArrayList alPatientNo) - " + clsPOSDBConstants.Log_Entering);
            DataTable dtUnpickedRxs = new DataTable();
            dtUnpickedRxs.Clear();
            dtUnpickedRxs.Columns.Add("Patient Name");
            dtUnpickedRxs.Columns.Add("Rx No");
            dtUnpickedRxs.Columns.Add("Refill No");
            dtUnpickedRxs.Columns.Add("Date Filled");
            dtUnpickedRxs.Columns.Add("Drug Name");
            dtUnpickedRxs.Columns.Add("Status");

            PharmBL oPharmBL = new PharmBL();
            frmPatientRXSearch ofrmPatientRXSearch;
            DataTable dtPatient = new DataTable();
            string sFamilyID = string.Empty;

            for (int i = 0; i < alPatientNo.Count; i++)
            {
                dtPatient = oPharmBL.GetPatient(alPatientNo[i].ToString());
                if (dtPatient != null && dtPatient.Rows.Count > 0)
                    sFamilyID = Configuration.convertNullToString(dtPatient.Rows[0]["FamilyID"]);
                else
                    sFamilyID = "";

                ofrmPatientRXSearch = new frmPatientRXSearch(alPatientNo[i].ToString(), 'P');
                ofrmPatientRXSearch.sFamilyID = sFamilyID;
                ofrmPatientRXSearch.Search();
                DataTable oRxInfo = ofrmPatientRXSearch.SelectedData;
                if (!oPOSTrans.CheckUnpickedRxLocally(oRxInfo))
                {
                    if (oRxInfo != null && oRxInfo.Rows.Count > 0)
                    {
                        for (int j = 0; j < oRxInfo.Rows.Count; j++)
                        {
                            DataRow oDataRow = dtUnpickedRxs.NewRow();
                            oDataRow["Patient Name"] = Configuration.convertNullToString(oRxInfo.Rows[j]["PatientName"]);
                            oDataRow["Rx No"] = Configuration.convertNullToString(oRxInfo.Rows[j]["RxNo"]);
                            oDataRow["Refill No"] = Configuration.convertNullToString(oRxInfo.Rows[j]["nRefill"]);
                            oDataRow["Date Filled"] = Configuration.convertNullToString(oRxInfo.Rows[j]["DateF"]) == "" ? "" : Convert.ToDateTime(oRxInfo.Rows[j]["DateF"]).ToShortDateString();
                            oDataRow["Drug Name"] = Configuration.convertNullToString(oRxInfo.Rows[j]["DrgName"]);
                            oDataRow["Status"] = Configuration.convertNullToString(oRxInfo.Rows[j]["Status"]);
                            dtUnpickedRxs.Rows.Add(oDataRow);
                        }
                    }
                }
            }
            logger.Trace("FillUnPickedRXs(ArrayList alPatientNo) - " + clsPOSDBConstants.Log_Exiting);
            return dtUnpickedRxs;
        }
        #endregion

        private bool DisplayWarningMSGForHugeTransAmt()
        {
            logger.Trace("DisplayWarningMSGForHugeTransAmt() - " + clsPOSDBConstants.Log_Entering);
            string strMsg = string.Empty;
            string sConstMsg = "There are some un-usual figures in transaction:\n\n";
            bool RetVal = false;
            try
            {
                strMsg = oPOSTrans.GetWarningMSGForHugeTransAmt(txtAmtSubTotal.Text, txtAmtDiscount.Text, txtAmtTax.Text, txtAmtTotal.Text);
                if (string.IsNullOrEmpty(strMsg) == false && Resources.Message.Display(sConstMsg + strMsg + "Do you want to proceed for payment?", "Transaction Amount", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    RetVal = true;
                }
                else if (string.IsNullOrEmpty(strMsg) == true)
                {
                    RetVal = true;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "DisplayWarningMSGForHugeTransAmt()");
                throw Ex;
            }
            logger.Trace("DisplayWarningMSGForHugeTransAmt() - " + clsPOSDBConstants.Log_Exiting);
            return RetVal;
        }


        //completed by sandeep
        private bool CheckStationCloseCashLimit()
        {
            try
            {
                logger.Trace("CheckStationCloseCashLimit() - " + clsPOSDBConstants.Log_Entering);
                if (Configuration.CPOSSet.MaxCashLimitForStnCose <= 0)
                {
                    logger.Trace("CheckStationCloseCashLimit() - Configuration.CPOSSet.MaxCashLimitForStnCose <= 0");
                    return true;
                }
                decimal CurrentCashInDrawer = oPOSTrans.GetCurrentCashStatus();
                if (CurrentCashInDrawer >= Configuration.CPOSSet.MaxCashLimitForStnCose)
                {
                    logger.Trace("CheckStationCloseCashLimit() - CurrentCashInDrawer >= Configuration.CPOSSet.MaxCashLimitForStnCose");
                    return false;
                }
                else
                {
                    decimal CashPerc = Convert.ToDecimal((Convert.ToDecimal(CurrentCashInDrawer) / Configuration.CPOSSet.MaxCashLimitForStnCose * 100));
                    if (CashPerc >= 90)
                    {
                        clsUIHelper.ShowWarningMsg("Current cash in the drawer reached " + CashPerc.ToString(Configuration.CInfo.CurrencySymbol + "##0.00") + "% of maximum station close cash limit define for the station# \"" + Configuration.StationID + "\".\nPlease close the station to perform more transactions.");
                    }
                }
                logger.Trace("CheckStationCloseCashLimit() - " + clsPOSDBConstants.Log_Exiting);
                return true;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CheckStationCloseCashLimit()");
                return false;
                throw Ex;
            }
            finally
            {
                //if (MaxStnCloseCashLimitThread != null && MaxStnCloseCashLimitThread.ThreadState == System.Threading.ThreadState.Running)
                //{
                //    MaxStnCloseCashLimitThread.Abort();
                //    logger.Trace("CheckStationCloseCashLimit() - Station close Cash Limit thread aborted");
                //}
            }
        }

        //private async Task CheckStationCloseCashLimitNew()
        //{
        //    try
        //    {
        //        logger.Trace("CheckStationCloseCashLimit() - " + clsPOSDBConstants.Log_Entering);
        //        if (Configuration.CPOSSet.MaxCashLimitForStnCose <= 0)
        //        {
        //            logger.Trace("CheckStationCloseCashLimit() - Configuration.CPOSSet.MaxCashLimitForStnCose <= 0");
        //            //return true;
        //        }
        //        decimal CurrentCashInDrawer = oPOSTrans.GetCurrentCashStatus();
        //        if (CurrentCashInDrawer >= Configuration.CPOSSet.MaxCashLimitForStnCose)
        //        {
        //            logger.Trace("CheckStationCloseCashLimit() - CurrentCashInDrawer >= Configuration.CPOSSet.MaxCashLimitForStnCose");
        //            //return false;
        //        }
        //        else
        //        {
        //            decimal CashPerc = Convert.ToDecimal((Convert.ToDecimal(CurrentCashInDrawer) / Configuration.CPOSSet.MaxCashLimitForStnCose * 100));
        //            if (CashPerc >= 90)
        //            {
        //                clsUIHelper.ShowWarningMsg("Current cash in the drawer reached " + CashPerc.ToString(Configuration.CInfo.CurrencySymbol + "##0.00") + "% of maximum station close cash limit define for the station# \"" + Configuration.StationID + "\".\nPlease close the station to perform more transactions.");
        //            }
        //        }
        //        logger.Trace("CheckStationCloseCashLimit() - " + clsPOSDBConstants.Log_Exiting);
        //        //return true;
        //    }
        //    catch (Exception Ex)
        //    {
        //        logger.Fatal(Ex, "CheckStationCloseCashLimit()");
        //        //return false;
        //        throw Ex;
        //    }
        //    finally
        //    {
        //        //if (MaxStnCloseCashLimitThread != null && MaxStnCloseCashLimitThread.ThreadState == System.Threading.ThreadState.Running)
        //        //{
        //        //    MaxStnCloseCashLimitThread.Abort();
        //        //    logger.Trace("CheckStationCloseCashLimit() - Station close Cash Limit thread aborted");
        //        //}
        //    }
        //}

        #region PRIMEPOS-2730
        private void ClearDeviceScreen()
        {
            SigPadUtil.DefaultInstance.ClearDeviceQueue();
        }
        #endregion
        //completed by sandeep
        private void showPayTypes()
        {
            bool bCombineSignature = false;
            DrugClassInfoCapture = null;    //PRIMEPOS-2547 23-Jul-2018 JY Added
            bOneTimePerTrans = false;   //PRIMEPOS-2547 23-Jul-2018 JY Added
            lstPatientNos.Clear();  //PRIMEPOS-2547 23-Jul-2018 JY Added

            #region PRIMEPOS-2402 12-Jul-2021 JY Added
            strOverrideMaxStationCloseCashLimit = string.Empty;
            strMaxTransactionAmountUser = string.Empty;
            strMaxReturnTransactionAmountUser = string.Empty;
            #endregion

            if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_ARIES8.ToUpper().Trim() || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_PAX7.ToUpper().Trim())
            {
                //ClearDeviceScreen(); //Aries8 Do not clear device date because Index based Add/Update/Delete data so need resend all data again PRIMEPOS-2952
            }
            else
            {
                #region PRIMEPOS-2730
                ClearDeviceScreen();
                #endregion
            }
            #region Sprint-26 - PRIMEPOS-2383 18-Aug-2017 JY Added logic to bring up override screen if standalone return is off and user trying standalone return
            try
            {
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && Configuration.convertNullToInt(oPOSTrans.oTransHData.TransHeader.Rows[0]["ReturnTransID"]) == 0)
                {
                    string sUserID = string.Empty;
                    bool bStatus = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.StandAloneReturn.ID, UserPriviliges.Permissions.StandAloneReturn.Name, out sUserID); //Sprint-26 - PRIMEPOS-2383 09-Aug-2017 JY Added for StandAloneReturn
                    if (!bStatus)
                    {
                        return;
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "showPayTypes() - Standalone return");
            }
            #endregion

            DataSet oPayTypeData = oPOSTrans.PopulatePayTypeList();
            if (oPayTypeData.Tables[0].Rows.Count == 0)
            {
                POS_Core_UI.Resources.Message.Display("Payment Options are not set , Please contact MMS Support for Resolution.", "Pay Type List", MessageBoxButtons.OK);
                return;
            }

            bCombineSignature = HasMultipleSignature();
            //Following 'If' Condition commented by krishna on 5 August 2011 to allow SignPad on return tranX also
            //if (lblTransactionType.Text != "Return Transaction") //change by SRT (Abhishek D) Date : 12 March 2010
            //{
            if (oPOSTrans.oRXHeaderList != null && oPOSTrans.oRXHeaderList.Count > 0)
            {
                bool brequireConsent = false;
                bool IsShowHCDialog = false; //PRIMEPOS-3264
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    #region Warn For Duplicate Charge Posting
                    if (oPOSTrans.WarnForDuplicateChargePosting(txtAmtTotal.Text, out IsShowHCDialog))
                    {
                        return;
                    }
                    if (IsShowHCDialog)
                    {
                        #region PRIMEPOS-3264
                        AutoClosingMessageBox.Show("House Charge Applied!", "Information!", 25, MessageBoxButtons.OK, DialogResult.None, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        #endregion
                    }
                    #endregion Warn For Duplicate Charge Posting

                    #region Warn For Delivery if any RX is marked for delivery.
                    //Following if is added By Shitaljit on 6/2/2104 for PRIMEPOS-1816 Ability to turn on\off delivery prompt
                    if (Configuration.CInfo.WarnForRXDelivery == true && onHoldTransID == 0)
                    {
                        string sSelectedAction = PrimeRXHelper.GetListOfRxMarkedForDelivery(this.oPOSTrans.oTransDRXData);

                        if (string.IsNullOrEmpty(sSelectedAction) == false && sSelectedAction.Equals("P") == false)
                        {
                            #region  Sprint-26 - PRIMEPOS-417 25-Jul-2017 JY Added - user can't hold invoice if customer is blank
                            if (string.IsNullOrWhiteSpace(txtCustomer.Text.Trim()))
                            {
                                Resources.Message.Display("Please select customer", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (txtCustomer.Enabled)
                                    txtCustomer.Focus();
                                return;
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(txtCustomer.Tag.ToString().Trim()))
                                    txtCustomer_Leave(null, null);
                            }
                            #endregion
                            PutOnHold(sSelectedAction);
                            return;
                        }
                    }
                    #endregion Warn For Delivery if any RX is marked for delivery.

                    if (!isBatchDelivery) // PRIMERX-7688 - NileshJ BatchDelivery added to supress Healthix.
                    {

                        #region PRIMEPOS-2442 ADDED BY ROHIT NAIR  Modified By Rohit Nair for PrimePOS-2448
                        //Sprint-26 - PRIMEPOS-2442 17-Aug-2017 JY consent logic should work for only sale trans
                        //PRIMEPOS-CONSENT SAJID DHUKKA PRIMEPOS-2866
                        //RXHeaderList oRXHeaderList = null;
                        //brequireConsent = oPOSTrans.IsConsentRequired(out oRXHeaderList);

                        List<PatientConsent> patientList = null;
                        brequireConsent = oPOSTrans.IsConsentRequired(out patientList);

                        if (brequireConsent)
                        {
                            //bool? val = oPOSTrans.CapturePatientConsent(Configuration.CInfo.ConsentSourceActiveList, out oconsent);

                            #region PRIMEPOS-3192
                            PharmBL opharm = new PharmBL();//PRIMEPOS-3192N
                            bool isPrescriptionActive = false;//PRIMEPOS-3192N
                            string rxSelected = string.Empty;
                            if (patientList?.Count > 0) //PRIMEPOS-3192N to check whether prescription level autoconsent is enable
                            {
                                foreach (var item in patientList)
                                {
                                    if (item.ConsentSourceName != null && item.ConsentSourceName.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_AUTO_REFILL.ToUpper()) //PRIMEPOS-3287 modified
                                    {
                                        isPrescriptionActive = opharm.isPrescriptionConsentActive(item.ConsentSourceID, MMS.Device.Global.Constants.CONSENT_TYPE_CODE_PRESCRIPTION_AUTO_REFILL);
                                    }
                                }
                            }
                            try
                            {
                                if (isPrescriptionActive)//PRIMEPOS-3192N
                                {

                                    DataTable dtPendingRxSignature = new DataTable();
                                    //PharmBL opharm = new PharmBL(); //PRIMEPOS-3192N
                                    string sPatientNo = string.Empty;
                                    string sRxs = string.Empty;
                                    foreach (var item in oPOSTrans.oRXHeaderList)
                                    {
                                        sPatientNo = sPatientNo + Convert.ToString(item.PatientNo) + ',';
                                        foreach (var rxItem in item.RXDetails)
                                        {
                                            sRxs = sRxs + rxItem.RXNo + ',';
                                        }
                                    }
                                    if (!string.IsNullOrWhiteSpace(sPatientNo))
                                    {
                                        int sPatIndex = sPatientNo.LastIndexOf(",");
                                        sPatientNo = sPatientNo.Substring(0, sPatIndex);
                                    }
                                    if (!string.IsNullOrWhiteSpace(sRxs))
                                    {
                                        int sRxIndex = sRxs.LastIndexOf(",");
                                        sRxs = sRxs.Substring(0, sRxIndex);
                                    }
                                    dtPendingRxSignature = opharm.GetPendingSignatureForAutoRefillConsent(sPatientNo, sRxs);
                                    if (dtPendingRxSignature != null && dtPendingRxSignature.Rows.Count > 0)
                                    {
                                        frmRxConsent rxcon = new frmRxConsent("Rx Auto Refill Consent", "Processing Rx Auto Refill Consent", dtPendingRxSignature);
                                        rxcon.ShowDialog();
                                        rxSelected = rxcon.selectedRxs;
                                        dtSelectedRx = rxcon.selectedRxTable;
                                        rxcon.Dispose();
                                    }
                                    if (dtSelectedRx?.Rows.Count == 0)
                                    {
                                        if (POSTransaction.isRxConsentHave)
                                        {
                                            //patientList.Clear();
                                            #region PRIMEPOS-3287
                                            if (patientList?.Count > 0)
                                            {
                                                for (int consent = patientList.Count - 1; consent >= 0; consent--)
                                                {
                                                    if (patientList[consent].ConsentSourceName != null && patientList[consent].ConsentSourceName.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_AUTO_REFILL.ToUpper())
                                                    {
                                                        patientList.RemoveAt(consent);
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string message = "An Error occured GetPendingSignatureForAutoRefillConsent()" + ex.Message;
                                logger.Error(ex, message);
                            }
                            #endregion
                            //PRIMEPOS-CONSENT SAJID DHUKKA PRIMEPOS-2866
                            bool? val = oPOSTrans.CapturePatientConsent(patientList, oPOSTrans.oRXHeaderList[0].PatientName, rxSelected);//PRIMEPOS-3120 //PRIMEPOS-3192 Added Extra Parameter rxSelected
                        }
                        #endregion
                    }
                }

                foreach (RXHeader oRXHeader in oPOSTrans.oRXHeaderList)
                {
                    if (!isBatchDelivery)// PRIMERX-7688 - NileshJ - BatchDelivery //Skip the NOPP signature and other details for BatchDelivery
                    {
                        #region Capturing NOPP Modified By Rohit Nair for PrimePOS-2448
                        if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && (Configuration.CPOSSet.DisableNOPP == false && !oPOSTrans.SkipForDelivery()))
                        {
                            if (oRXHeader.isNOPPSignRequired == true)
                            {
                                //Added by SRT to hold the screen
                                if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen) //Sprint - 23 - PRIMEPOS - 2321 29 - Jul - 2016 JY Added if clause //PRIMEPOS-3231N
                                {
                                    bool? rval = CaptureNOPPSignatureWPF(oRXHeader, bCombineSignature);
                                    if (rval == false)
                                    {
                                        POS_Core_UI.Resources.Message.Display("Privacy Acknowledgement Ignored", "Privacy Acknowledgement", MessageBoxButtons.OK);
                                    }
                                    else if (rval == null)
                                    {
                                        POS_Core_UI.Resources.Message.Display("Privacy Ack is Continued ", "Privacy Acknowledgement", MessageBoxButtons.OK);
                                    }
                                }
                                else
                                {
                                    bool? rval = CaptureNOPPSignature(oRXHeader);
                                    if (rval == false && Configuration.CPOSSet.UseSigPad == true)
                                    {
                                        SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Acknowledgement Ignored");
                                        POS_Core_UI.Resources.Message.Display("Privacy Acknowledgement Ignored", "Privacy Acknowledgement", MessageBoxButtons.OK);
                                    }
                                    else if (rval == null && Configuration.CPOSSet.UseSigPad == true)
                                    {
                                        SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Ack is Continued ");
                                        POS_Core_UI.Resources.Message.Display("Privacy Ack is Continued ", "Privacy Acknowledgement", MessageBoxButtons.OK);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    //Modified By Dharmendra SRT on Feb-02-09
                    //Following if is added by Shitaljit(QuicSolv) on 21 sept 2011
                    //To Stop asking for signature in return RX transaction.
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        //Added by Manoj to check if its a valid drug class  Modified By Rohit Nair for PrimePOS-2448
                        //if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && (Configuration.CPOSSet.ControlByID && !oPOSTrans.SkipForDelivery() && RxWithValidClass != null && RxWithValidClass.Rows.Count > 0))  //PRIMEPOS-2547 03-Jul-2018 JY Commented
                        if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && (Configuration.CPOSSet.ControlByID == 1 && !oPOSTrans.SkipForDelivery() && RxWithValidClass != null && RxWithValidClass.Rows.Count > 0))    //PRIMEPOS-2547 03-Jul-2018 JY Added
                        {
                            if ((Configuration.CPOSSet.AskVerificationIdMode == 0 && bOneTimePerTrans == false) || (Configuration.CPOSSet.AskVerificationIdMode == 1 && !lstPatientNos.Contains(oRXHeader.PatientNo, StringComparer.OrdinalIgnoreCase)))    //PRIMEPOS-2547 12-Jul-2018 JY Added
                            {
                                //check patient have control drug
                                Boolean bIsRxValidClass = false;
                                foreach (RXDetail oRXDetail in oRXHeader.RXDetails)
                                {
                                    foreach (DataRow dr in RxWithValidClass.Rows)
                                    {
                                        if (dr["DrugClass"].ToString() != "" && oRXDetail.RXNo.ToString() == dr["RxNo"].ToString() && oRXDetail.RefillNo.ToString() == dr["RefillNo"].ToString())
                                        {
                                            bIsRxValidClass = true;
                                            break;
                                        }
                                    }
                                }
                                if (bIsRxValidClass)
                                {
                                    bOneTimePerTrans = true;    //PRIMEPOS-2547 12-Jul-2018 JY Added
                                    lstPatientNos.Add(oRXHeader.PatientNo); //PRIMEPOS-2547 12-Jul-2018 JY Added
                                    //frmPOSDrugClassCapture frm = new frmPOSDrugClassCapture(oRXHeader.PatientName);   //PRIMEPOS-2429 27-Jun-2019 JY Commented
                                    frmPOSDrugClassCapture frm = new frmPOSDrugClassCapture(oRXHeader.PatientNo, bIsPatient, strDriversLicense, DriversLicenseExpDate); //PRIMEPOS-2429 27-Jun-2019 JY Added  //PRIMEPOS-3065 10-Mar-2022 JY Added strDriversLicense, DriversLicenseExpDate
                                    frm.TblPatient = oRXHeader.TblPatient;
                                    frm.ShowDialog();
                                    if (frm.IsCancelled) //if cancel is click on drug class verification
                                    {
                                        return; // stop and return
                                    }
                                    else
                                    {
                                        #region PRIMEPOS-2547 20-Jul-2018 JY Added
                                        if (DrugClassInfoCapture == null)
                                        {
                                            DrugClassInfoCapture = new DataTable();
                                            DrugClassInfoCapture.Columns.Add("RxNo", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("RefillNo", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("IDTYPE", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("RELATION", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("IDNUM", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("STATE", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("LASTNAME", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("FIRSTNAME", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("PartialFillNo", typeof(string));
                                            DrugClassInfoCapture.Columns.Add("DriversLicenseExpDate", typeof(DateTime));  //PRIMEPOS-3065 10-Mar-2022 JY Added
                                        }

                                        string RxNo, RefillNo, PartialFillNo = string.Empty, IDTYPE = string.Empty, RELATION = string.Empty, IDNUM, STATE, LASTNAME, FIRSTNAME;
                                        oPOSTrans.GetDrugClassInfoCapture(frm.cboVerifID.Text, frm.cboRelation.Text, ref IDTYPE, ref RELATION);
                                        IDNUM = frm.txtIDNum.Text;
                                        if (frm.cboRelation.SelectedIndex == 0)
                                        {
                                            STATE = string.IsNullOrEmpty(oRXHeader.PatientState) ? "" : oRXHeader.PatientState;
                                            string[] PatName = oRXHeader.PatientName.Split(',');
                                            LASTNAME = string.IsNullOrEmpty(PatName[0]) ? "" : PatName[0];
                                            FIRSTNAME = string.IsNullOrEmpty(PatName[1]) ? "" : PatName[1];
                                        }
                                        else
                                        {
                                            STATE = frm.txtState.Text;
                                            LASTNAME = frm.txtLastName.Text;
                                            FIRSTNAME = frm.txtFirstName.Text;
                                        }
                                        foreach (RXDetail oRXDetails in oRXHeader.RXDetails)
                                        {
                                            RxNo = oRXDetails.RXNo.ToString();
                                            RefillNo = oRXDetails.RefillNo.ToString();
                                            PartialFillNo = oRXDetails.PartialFillNo.ToString();
                                            DrugClassInfoCapture.Rows.Add(RxNo, RefillNo, IDTYPE, RELATION, IDNUM, STATE, LASTNAME, FIRSTNAME, PartialFillNo, DriversLicenseExpDate);   //PRIMEPOS-3065 10-Mar-2022 JY Added DriversLicenseExpDate
                                        }
                                        #endregion
                                    }
                                    #region PRIMEPOS-2547 20-Jul-2018 JY Commented
                                    //DrugClassInfoCapture = oPOSTrans.GetDrugClassInfoCapture(DrugClassInfoCapture, frm.cboVerifID.Text, frm.cboRelation.Text);
                                    //if (frm.cboRelation.SelectedIndex == 0)
                                    //{
                                    //    string[] PatName = oRXHeader.PatientName.Split(',');
                                    //    DrugClassInfoCapture.Add("IDNUM", frm.txtIDNum.Text);
                                    //    DrugClassInfoCapture.Add("STATE", string.IsNullOrEmpty(oRXHeader.PatientState) ? "" : oRXHeader.PatientState);
                                    //    DrugClassInfoCapture.Add("LASTNAME", string.IsNullOrEmpty(PatName[0]) ? "" : PatName[0]);
                                    //    DrugClassInfoCapture.Add("FIRSTNAME", string.IsNullOrEmpty(PatName[1]) ? "" : PatName[1]);
                                    //}
                                    //else
                                    //{
                                    //    DrugClassInfoCapture.Add("IDNUM", frm.txtIDNum.Text);
                                    //    DrugClassInfoCapture.Add("STATE", frm.txtState.Text);
                                    //    DrugClassInfoCapture.Add("LASTNAME", frm.txtLastName.Text);
                                    //    DrugClassInfoCapture.Add("FIRSTNAME", frm.txtFirstName.Text);
                                    //}
                                    #endregion
                                }
                            }
                        }
                        #region PRIMEPOS-2547 05-Jul-2018 JY Added
                        else if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && (Configuration.CPOSSet.ControlByID == 2 && !oPOSTrans.SkipForDelivery()))
                        {
                            if ((Configuration.CPOSSet.AskVerificationIdMode == 0 && bOneTimePerTrans == false) || (Configuration.CPOSSet.AskVerificationIdMode == 1 && !lstPatientNos.Contains(oRXHeader.PatientNo, StringComparer.OrdinalIgnoreCase)))    //PRIMEPOS-2547 12-Jul-2018 JY Added
                            {
                                bOneTimePerTrans = true;    //PRIMEPOS-2547 12-Jul-2018 JY Added
                                lstPatientNos.Add(oRXHeader.PatientNo); //PRIMEPOS-2547 12-Jul-2018 JY Added
                                //frmPOSDrugClassCapture frm = new frmPOSDrugClassCapture(oRXHeader.PatientName);   //PRIMEPOS-2429 27-Jun-2019 JY Commented
                                frmPOSDrugClassCapture frm = new frmPOSDrugClassCapture(oRXHeader.PatientNo, bIsPatient, strDriversLicense, DriversLicenseExpDate);
                                frm.TblPatient = oRXHeader.TblPatient;
                                
                                //PRIMEPOS-2429 27-Jun-2019 JY Added    //PRIMEPOS-3065 10-Mar-2022 JY Added strDriversLicense, DriversLicenseExpDate
                                frm.ShowDialog();
                                if (frm.IsCancelled) //if cancel is click on drug class verification
                                {
                                    return; // stop and return
                                }
                                else
                                {
                                    if (DrugClassInfoCapture == null)
                                    {
                                        DrugClassInfoCapture = new DataTable();
                                        DrugClassInfoCapture.Columns.Add("RxNo", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("RefillNo", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("IDTYPE", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("RELATION", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("IDNUM", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("STATE", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("LASTNAME", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("FIRSTNAME", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("PartialFillNo", typeof(string));
                                        DrugClassInfoCapture.Columns.Add("DriversLicenseExpDate", typeof(DateTime));  //PRIMEPOS-3065 10-Mar-2022 JY Added
                                    }

                                    string RxNo, RefillNo, PartialFillNo = string.Empty, IDTYPE = string.Empty, RELATION = string.Empty, IDNUM, STATE, LASTNAME, FIRSTNAME;
                                    oPOSTrans.GetDrugClassInfoCapture(frm.cboVerifID.Text, frm.cboRelation.Text, ref IDTYPE, ref RELATION);
                                    IDNUM = frm.txtIDNum.Text;
                                    if (frm.cboRelation.SelectedIndex == 0)
                                    {
                                        STATE = string.IsNullOrEmpty(oRXHeader.PatientState) ? "" : oRXHeader.PatientState;
                                        string[] PatName = oRXHeader.PatientName.Split(',');
                                        LASTNAME = string.IsNullOrEmpty(PatName[0]) ? "" : PatName[0];
                                        FIRSTNAME = string.IsNullOrEmpty(PatName[1]) ? "" : PatName[1];
                                    }
                                    else
                                    {
                                        STATE = frm.txtState.Text;
                                        LASTNAME = frm.txtLastName.Text;
                                        FIRSTNAME = frm.txtFirstName.Text;
                                    }
                                    foreach (RXDetail oRXDetails in oRXHeader.RXDetails)
                                    {
                                        RxNo = oRXDetails.RXNo.ToString();
                                        RefillNo = oRXDetails.RefillNo.ToString();
                                        PartialFillNo = oRXDetails.PartialFillNo.ToString();
                                        DrugClassInfoCapture.Rows.Add(RxNo, RefillNo, IDTYPE, RELATION, IDNUM, STATE, LASTNAME, FIRSTNAME, PartialFillNo, DriversLicenseExpDate);   //PRIMEPOS-3065 10-Mar-2022 JY Added DriversLicenseExpDate
                                    }
                                }
                            }
                        }
                        #endregion

                        if (!isBatchDelivery)// PRIMERX-7688 NileshJ BatchDelivery to skip signature for Rx as it will not be possible for the operator to collect signature of the actual patient
                        {
                            if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && (!oPOSTrans.SkipForDelivery()) && (!Configuration.CPOSSet.SkipRxSignature))// Modified By Rohit Nair for PrimePOS-2448 // PRIMEPOS-2737 - Skip RX signature - NileshJ - 30-Sept-2019
                            {
                                if (CaptureRXSignature(oRXHeader) == false && Configuration.CPOSSet.UseSigPad == true)
                                {
                                    //Added by SRT to fix the Cancel Issue
                                    SigPadUtil.DefaultInstance.ShowCustomScreen("RX Signature rejected");
                                    POS_Core_UI.Resources.Message.Display("Rx Signature rejected", "Rx Approval", MessageBoxButtons.OK);
                                    //Added by SRT to fix the Cancel Issue till here
                                    foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                                    {
                                        oRXHeader.NOPPSignature = "";
                                        oRXHeader.RXSignature = "";
                                        oRXHeader.bBinarySign = null;
                                    }
                                    if (Configuration.CPOSSet.UseSigPad == true)
                                    {
                                        //Restoring Item Data as a precautionary measure. In case of Host Server Failure & relaunch
                                        //initially there might not be item transaction data on this stage, so it will require
                                        //restoration of item data in device store
                                        //SigPadUtil.DefaultInstance.RestoreItemData();
                                        SigPadUtil.DefaultInstance.ShowItemsScreen();
                                    }
                                    SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Ack is Continued ");
                                    return;
                                }
                                else if (!bCombineSignature && ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen))   //Sprint-23 - PRIMEPOS-2321 29-Jul-2016 JY Added if condition//3002 //PRIMEPOS-3231N
                                {
                                    bool? rval = CaptureRXSignatureWPF(oRXHeader);
                                    if (rval == false || rval == null)
                                    {
                                        POS_Core_UI.Resources.Message.Display("Rx Signature rejected", "Rx Approval", MessageBoxButtons.OK);
                                        foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                                        {
                                            oRXHeader.NOPPSignature = "";
                                            oRXHeader.RXSignature = "";
                                            oRXHeader.bBinarySign = null;
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                #region PRIMEPOS-3079
                if (SigPadUtil.DefaultInstance.IsVF)
                {
                    int maxCount = oPOSTrans.oRXHeaderList.Count;
                    if (maxCount > 1)
                    {
                        RXHeader oRXMaxHeader = oPOSTrans.oRXHeaderList[maxCount - 1];
                        foreach (RXHeader oRXHeader in oPOSTrans.oRXHeaderList)
                        {
                            oRXHeader.bBinarySign = oRXMaxHeader.bBinarySign;
                        }
                    }
                }
                #endregion


                #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && SigPadUtil.DefaultInstance.isISC)
                {
                    if (bCaptureSignature)
                    {
                        byte[] sigdata;
                        bool bReturn = CapturePatientSignature(out sigdata);
                        #region  Sprint-26 - PRIMEPOS-2442 18-Aug-2017 JY Added logic to redirect transaction screen if signature cancelled by user
                        if (bReturn == false)
                        {
                            SigPadUtil.DefaultInstance.ShowCustomScreen("RX Signature rejected");
                            POS_Core_UI.Resources.Message.Display("Rx Signature rejected", "Rx Approval", MessageBoxButtons.OK);
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowItemsScreen();
                            }
                            SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Ack is Continued ");
                            return;
                        }
                        #endregion
                        else if (sigdata != null)
                        {
                            foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                            {
                                if (oHeader.isNOPPSignRequired)
                                {
                                    oHeader.NoppBinarySign = sigdata;
                                }
                                if (brequireConsent && oHeader.IsConsentRequired)
                                {
                                    // PRIMEPOS - CONSENT SAJID DHUKKA PRIMEPOS-2866
                                    //if (oHeader.PatConsent != null && !oHeader.PatConsent.IsConsentSkip)
                                    //{
                                    //    oHeader.PatConsent.SignatureData = sigdata;
                                    //}
                                    foreach (PatientConsent item in oHeader.PatConsent)
                                    {
                                        if (!item.IsConsentSkip)
                                        {
                                            item.SignatureData = sigdata;
                                        }
                                    }
                                }
                                oHeader.bBinarySign = sigdata;
                            }
                        }
                    }
                }

                if (!isBatchDelivery)// PRIMERX-7688 NileshJ BatchDelivery addded to bypass capturePatientsignature when in BatchDelivery as patient is not in front of operator
                {
                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && SigPadUtil.DefaultInstance.isPAX)
                    {
                        if (bCaptureSignature)
                        {
                            byte[] sigPlots;
                            bool bReturn = CapturePatientSignaturePAX(out sigPlots);
                            #region  Sprint-26 - PRIMEPOS-2442 18-Aug-2017 JY Added logic to redirect transaction screen if signature cancelled by user
                            if (bReturn == false)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("RX Signature rejected");
                                POS_Core_UI.Resources.Message.Display("Rx Signature rejected", "Rx Approval", MessageBoxButtons.OK);

                                if (Configuration.CPOSSet.UseSigPad == true)
                                {
                                    SigPadUtil.DefaultInstance.ShowItemsScreen();
                                }
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Ack is Continued ");
                                return;
                            }
                            #endregion
                            else if (sigPlots != null)
                            {
                                foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                                {
                                    if (oHeader.isNOPPSignRequired)
                                    {
                                        oHeader.NoppBinarySign = sigPlots;
                                    }
                                    if (brequireConsent && oHeader.IsConsentRequired)
                                    {
                                        // PRIMEPOS - CONSENT SAJID DHUKKA PRIMEPOS-2866
                                        //if (oHeader.PatConsent != null && !oHeader.PatConsent.IsConsentSkip)
                                        //{
                                        //    oHeader.PatConsent.SignatureData = sigPlots;
                                        //}
                                        foreach (PatientConsent item in oHeader.PatConsent)
                                        {
                                            if (!item.IsConsentSkip)
                                            {
                                                item.SignatureData = sigPlots;
                                            }
                                        }
                                    }
                                    oHeader.bBinarySign = sigPlots;
                                }
                            }
                        }
                    }
                }

                // Added for Evertec - PRIMEPOS - 2664 Arvind      
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && SigPadUtil.DefaultInstance.isEvertec)
                {
                    if (bCaptureSignature)
                    {
                        byte[] sigPlots;
                        bool bReturn = CapturePatientSignature(out sigPlots);
                        if (bReturn == false)
                        {
                            SigPadUtil.DefaultInstance.ShowCustomScreen("RX Signature rejected");
                            POS_Core_UI.Resources.Message.Display("Rx Signature rejected", "Rx Approval", MessageBoxButtons.OK);

                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowItemsScreen();
                            }
                            SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Ack is Continued ");
                            return;
                        }
                        else if (sigPlots != null)
                        {
                            foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                            {
                                if (oHeader.isNOPPSignRequired)
                                {
                                    oHeader.NoppBinarySign = sigPlots;
                                }
                                if (brequireConsent && oHeader.IsConsentRequired)
                                {
                                    // PRIMEPOS - CONSENT SAJID DHUKKA PRIMEPOS-2866
                                    //if (oHeader.PatConsent != null && !oHeader.PatConsent.IsConsentSkip)
                                    //{
                                    //    oHeader.PatConsent.SignatureData = sigPlots;
                                    //}
                                    foreach (PatientConsent item in oHeader.PatConsent)
                                    {
                                        if (!item.IsConsentSkip)
                                        {
                                            item.SignatureData = sigPlots;
                                        }
                                    }
                                }
                                oHeader.bBinarySign = sigPlots;
                            }
                        }

                    }
                }
                //PRIMEPOS-2636 VANTIV
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && SigPadUtil.DefaultInstance.isVantiv && Configuration.CPOSSet.PinPadModel == "VANTIV")//3002
                {
                    if (bCaptureSignature)
                    {
                        byte[] sigPlots;
                        bool bReturn = CapturePatientSignature(out sigPlots);
                        #region  Sprint-26 - PRIMEPOS-2442 18-Aug-2017 JY Added logic to redirect transaction screen if signature cancelled by user
                        if (bReturn == false)
                        {
                            SigPadUtil.DefaultInstance.ShowCustomScreen("RX Signature rejected");
                            POS_Core_UI.Resources.Message.Display("Rx Signature rejected", "Rx Approval", MessageBoxButtons.OK);

                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowItemsScreen();
                            }
                            SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Ack is Continued ");
                            return;
                        }
                        #endregion
                        else if (sigPlots != null)
                        {
                            foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                            {
                                if (oHeader.isNOPPSignRequired)
                                {
                                    oHeader.NoppBinarySign = sigPlots;
                                }
                                if (brequireConsent && oHeader.IsConsentRequired)
                                {
                                    // PRIMEPOS - CONSENT SAJID DHUKKA PRIMEPOS-2866
                                    //if (oHeader.PatConsent != null && !oHeader.PatConsent.IsConsentSkip)
                                    //{
                                    //    oHeader.PatConsent.SignatureData = sigPlots;
                                    //}
                                    foreach (PatientConsent item in oHeader.PatConsent)
                                    {
                                        if (!item.IsConsentSkip)
                                        {
                                            item.SignatureData = sigPlots;
                                        }
                                    }
                                }
                                oHeader.bBinarySign = sigPlots;
                            }
                        }

                    }
                }
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && SigPadUtil.DefaultInstance.isElavon)
                {
                    if (bCaptureSignature)
                    {
                        byte[] sigPlots;
                        bool bReturn = CapturePatientSignature(out sigPlots);
                        #region  Sprint-26 - PRIMEPOS-2442 18-Aug-2017 JY Added logic to redirect transaction screen if signature cancelled by user
                        if (bReturn == false)
                        {
                            SigPadUtil.DefaultInstance.ShowCustomScreen("RX Signature rejected");
                            POS_Core_UI.Resources.Message.Display("Rx Signature rejected", "Rx Approval", MessageBoxButtons.OK);

                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowItemsScreen();
                            }
                            SigPadUtil.DefaultInstance.ShowCustomScreen("Privacy Ack is Continued ");
                            return;
                        }
                        #endregion
                        else if (sigPlots != null)
                        {
                            foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                            {
                                if (oHeader.isNOPPSignRequired)
                                {
                                    oHeader.NoppBinarySign = sigPlots;
                                }
                                if (brequireConsent && oHeader.IsConsentRequired)
                                {
                                    // PRIMEPOS - CONSENT SAJID DHUKKA PRIMEPOS-2866
                                    //if (oHeader.PatConsent != null && !oHeader.PatConsent.IsConsentSkip)
                                    //{
                                    //    oHeader.PatConsent.SignatureData = sigPlots;
                                    //}
                                    foreach (PatientConsent item in oHeader.PatConsent)
                                    {
                                        if (!item.IsConsentSkip)
                                        {
                                            item.SignatureData = sigPlots;
                                        }
                                    }
                                }
                                oHeader.bBinarySign = sigPlots;
                            }
                        }

                    }
                }
                //
                #endregion

                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && bCombineSignature)
                {
                    RXHeader oRxHd = new RXHeader();
                    bool? rval = CaptureRXSignatureWPF(oRxHd);
                    this.SigType = "M";
                    foreach (RXHeader oHeader in oPOSTrans.oRXHeaderList)
                    {
                        if (rval == true)
                        {
                            if (oHeader.isNOPPSignRequired)
                            {
                                oHeader.NoppBinarySign = oRxHd.bBinarySign;
                                oHeader.NOPPSignature = string.Empty;
                                oHeader.PrivacyText = Configuration.CInfo.PrivacyText;
                            }

                            oHeader.CounselingRequest = oRxHd.CounselingRequest;
                            if (brequireConsent && oHeader.IsConsentRequired)
                            {
                                // PRIMEPOS - CONSENT SAJID DHUKKA PRIMEPOS-2866
                                //if (oHeader.PatConsent != null && !oHeader.PatConsent.IsConsentSkip)
                                //{
                                //    oHeader.PatConsent.SignatureData = oRxHd.bBinarySign;
                                //}
                                foreach (PatientConsent item in oHeader.PatConsent)
                                {
                                    if (!item.IsConsentSkip)
                                    {
                                        item.SignatureData = oRxHd.bBinarySign;
                                    }
                                }
                            }

                            oHeader.bBinarySign = oRxHd.bBinarySign;
                        }
                        else
                        {
                            oHeader.NOPPSignature = "";
                            oHeader.RXSignature = "";
                            oHeader.bBinarySign = null;
                        }
                    }

                    if (rval == false || rval == null)
                    {
                        POS_Core_UI.Resources.Message.Display("Rx Signature has been cancelled.", "Rx Approval", MessageBoxButtons.OK);
                        return;
                    }
                }
            }

            if (Configuration.CPOSSet.UseSigPad == true)
            {
                /*Date 27-jan-2014
                    * Modified by Shitaljit
                    * For making currency symbol dynamic
                    */
                //old code
                //SigPadUtil.DefaultInstance.ShowCustomScreen(" Total Amount : $" + this.txtAmtTotal.Text + "\n Processing Payment...");//Added by SRT for better flow.
                //new code
                SigPadUtil.DefaultInstance.ShowCustomScreen(" Total Amount : " + Configuration.CInfo.CurrencySymbol.ToString() + this.txtAmtTotal.Text + "\n Processing Payment...");//Added by SRT for better flow.
            }
            //}//'If' Condition commented by krishna on 5 August 2011 to allow SignPad on return tranX also
            string tempCustCode = string.Empty;
            string tempCustName = string.Empty;
            if (this.oPOSTrans.oCustomerRow != null && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                tempCustCode = oPOSTrans.oCustomerRow.CustomerCode;
                tempCustName = oPOSTrans.oCustomerRow.CustomerFullName;
            }

            //From Here Added by Ravindra (QuicSolv) for User rights for Max Transaction limit 25 jan 2013
            if (Configuration.convertNullToDecimal(this.txtAmtTotal.Text) > 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                TransactionAmount = Configuration.convertNullToDecimal(this.txtAmtTotal.Text);
                if (TransactionAmount > Configuration.UserMaxTransactionLimit)  //Sprint-23 - PRIMEPOS-2303 25-May-2016 JY removed - && Configuration.UserMaxTransactionLimit > 0 - as 0 means 0 transaction limit
                {
                    POS_Core_UI.UserManagement.clsLogin oLogin = new POS_Core_UI.UserManagement.clsLogin();
                    //oLogin.GetUsersRole(Configuration.UserName);
                    string sUserID = "";
                    if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxTransactionLimit, "", out sUserID, "Security Override For Maximum Transaction Limit") == false)
                    {
                        TransactionAmount = 0;
                        return;
                    }
                    else //PRIMEPOS-2402 19-Jul-2021 JY Added
                    {
                        strMaxTransactionAmountUser = sUserID;
                    }
                }
            }
            //till here added by Ravindra (QuicSolv) fo Max Transaction limit 25 jan 2013
            #region Sprint-23 - PRIMEPOS-2303 24-May-2016 JY Added 
            if (Math.Abs(Configuration.convertNullToDecimal(this.txtAmtTotal.Text)) > 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                TransactionAmount = Math.Abs(Configuration.convertNullToDecimal(this.txtAmtTotal.Text));
                if (TransactionAmount > Configuration.UserMaxReturnTransLimit)
                {
                    POS_Core_UI.UserManagement.clsLogin oLogin = new POS_Core_UI.UserManagement.clsLogin();
                    string sUserID = "";
                    if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxReturnTransLimit, "", out sUserID, "Security Override For Maximum Return Transaction Limit") == false)
                    {
                        TransactionAmount = 0;
                        return;
                    }
                    else //PRIMEPOS-2402 19-Jul-2021 JY Added
                    {
                        strMaxReturnTransactionAmountUser = sUserID;
                    }
                }
            }
            #endregion

            // string user = Configuration.UserName;
            if (Configuration.CLoyaltyInfo.ShowCLCardInputOnTrans == true && this.oCLCardRow == null)
            {
                bool bStatus = ShowCLCardInputScreen(false);
                if (bStatus == false)
                {
                    if (IsCouponAddedIntoTransaction())    //PRIMEPOS-2034 12-Mar-2018 JY if coupon discount added into transaction then remove it
                    {
                        RemoveCouponDiscount();
                    }
                    return;   //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added 
                }
            }

            #region Customer Discount calcualtion logic
            if (this.oPOSTrans.oCustomerRow != null && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                bool isException = false;
                if (custDiscount > 0)
                {
                    this.lblInvDiscount.Text = (Configuration.convertNullToDecimal(this.lblInvDiscount.Text) - custDiscount).ToString();
                    RecalculateTax();
                    this.ultraCalcManager1.ReCalc();
                }

                if (oPOSTrans.oCustomerRow.Discount > 0 && ((Configuration.convertNullToDecimal(this.lblInvDiscount.Text) == 0) || tempCustId != oPOSTrans.oCustomerRow.CustomerId || tempCustCode.Equals("-1")))
                {
                    if (Configuration.CLoyaltyInfo.ApplyCLOrCusDisc == false || oPOSTrans.oCustomerRow.UseForCustomerLoyalty == false)
                    {
                        ApplyCustomerDiscount(out isException, tempCustName);

                        if (isException == true)
                        {
                            custDiscount = 0;
                        }
                        if (custDiscount > 0)
                        {
                            clsUIHelper.ShowSuccessMsg("Customer discount of " + Configuration.CInfo.CurrencySymbol.ToString() + custDiscount + " is applied to the transaction as invoice discount.", "Customer Discount");
                            InvDicsValueToVerify = oPOSTrans.oCustomerRow.Discount;
                        }
                    }
                }
            }
            #endregion Customer Discount calcualtion logic

            //PRIMEPOS-2979 19-Aug-2021 JY Commented
            //Added By Shitaljit to validate user rights for Invoice Discount
            //if (Configuration.convertNullToDecimal(this.lblInvDiscount.Text) > 0 && Configuration.convertNullToDecimal(this.txtAmtSubTotal.Text) > 0 && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            //{
            //    if (InvDicsValueToVerify > Configuration.UserMaxDiscountLimit && Configuration.IsDiscOverridefromPOSTrans == false)
            //    {
            //        POS_Core_UI.UserManagement.clsLogin oLogin = new POS_Core_UI.UserManagement.clsLogin();
            //        string sUserID = "";
            //        if (oLogin.loginForPreviliges(clsPOSDBConstants.UserMaxDiscountLimit, "", out sUserID, "Security Override For Invoice Discount") == false)
            //        {
            //            InvDicsValueToVerify = 0;
            //            custDiscount = 0;
            //            this.lblInvDiscount.Text = "0.00";
            //            return;
            //        }
            //    }
            //}

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (Convert.ToDecimal(this.txtAmtTotal.Text) == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(500);
                }
                //Added by shitaljit on 26Dec2013 for PRIMEPOS-1627 Remove Tax on EBT Transaction
                decimal TotalTaxForEBTItems = 0;
                decimal dIIASAmount = oPOSTrans.GetIIASTotal();
                decimal dEBTAmount = oPOSTrans.GetEBTTotal(out TotalTaxForEBTItems);

                //Added By SRT(Ritesh Parekh) Date: 17-Aug-2009
                //Details: To Recalculate the IIAS approved Rx Item total on every call of POSPayTypeList.
                decimal dIIASRxAmount = oPOSTrans.GetIIASRxTotal();
                //End Of Added By SRT(Ritesh Parekh)
                //Changed By SRT(Ritesh Parekh) date: 17-Aug-2009
                //Changed Constructor calling method here. Additional variable value passed.

                //Added by SRT(Abhishek) Date : 05/09/2009
                //Added for rounding the value to two digits.
                if (this.PendingAmount != 0)
                {
                    if (this.PendingAmount < 0)
                    {
                        this.PendingAmount = 0;
                    }
                    //this.txtAmtTotal.Text = Convert.ToString(this.PendingAmount); //PRIMEPOS-3345
                }
                decimal totalAmount = Math.Round(Convert.ToDecimal(this.txtAmtTotal.Text), 2);
                //End of Added by SRT(Abhishek) Date : 05/09/2009

                //Added by SRT(Abhishek) Date : 05/09/2009
                //Added for rounding the value to two digits.
                decimal taxAmount = Math.Round(Convert.ToDecimal(this.txtAmtTax.Text), 2);
                //End of Added by SRT(Abhishek) Date : 05/09/2009

                frmPOSPayTypesList oPTList = new frmPOSPayTypesList(totalAmount, oPOSTrans.CurrentTransactionType, taxAmount, false, sSigPadTransID, dIIASAmount, dIIASRxAmount, this.lblCustomerName.Text, bIsCustomerTokenExists, oPOSTrans, isBatchDelivery, alreadyPaidAmount);    //PRIMEPOS-2611 13-Nov-2018 JY Added bIsCustomerTokenExists parameter //  Added oPOSTrans for Solutran - PRIMEPOS-2663 - NileshJ //  NileshJ - BatchDelivery - Added IsBatchDelivery,alreadyPaidAmount PRIMERX-7688 23-Sept-2019

                oPTList.TotalEBTAmount = dEBTAmount;
                oPTList.oPosPTList.oCurrentCustRow = oPOSTrans.oCustomerRow;
                oPTList.TotalEBTItemsTaxAmount = TotalTaxForEBTItems;
                oPTList.CLCard = this.oCLCardRow;
                oPTList.parentForm = this;
                oPTList.isStrictReturn = this.isStrictReturn;//PRIMEPOS-2738 ARVIND
                oPTList.IsCustomerDriven = this.IsCustomerDriven;//PRIMEPOS-2915
                oPTList.EvertecTaxDetails = CalculateEvertecTax();//2857     
                oPTList.oPOSTransPaymentData = this.oPOSTrans.oPOSTransPaymentData;//PRIMEPOS-2915     
                string Tax = string.Empty;  //2943         
                oPTList.isTransOnHold = isOnHoldTrans; //PRIMEPOS-3283
                oPTList.IsElavonTax = CalculateElavonTax(out Tax);//2943
                oPTList.ElavonTotalTax = Tax;
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    oPTList.TransID = TransID;
                }
                try
                {
                    CLCards ccard = new CLCards();
                    oPTList.oPosPTList.maxClCouponAmount = ccard.CalculateMaxCoupmAmount(oPOSTrans.oTransDData.TransDetail, txtAmtDiscount.Text.ToString()); //09-Apr-2015 JY Added txtAmtDiscount.Text.ToString()
                }
                catch (Exception)
                {
                }
                if (this.txtCustomer.Tag != null)
                {
                    oPTList.oPosPTList.CustomerID = Configuration.convertNullToInt(this.txtCustomer.Tag.ToString());
                }
                oPTList.oPosPTList.RXHeaderList = oPOSTrans.oRXHeaderList;
                //Added By shitaljit for Station Close Cash Limit JITA- 1044
                if (bContinuePayment == false)
                {
                    string sUserID = "";
                    UserManagement.clsLogin oLogin = new POS_Core_UI.UserManagement.clsLogin();
                    clsUIHelper.ShowErrorMsg("Current cash in the drawer reached maximum station close cash limit define for the station# \"" + Configuration.StationID + "\".\nPlease close the station to perform more transactions.");
                    if (oLogin.loginForPreviliges(clsPOSDBConstants.StnCloseCashLimit, "", out sUserID, "Security Override For Station Close Cash Limit") == false)
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else //PRIMEPOS-2402 12-Jul-2021 JY Added
                    {
                        strOverrideMaxStationCloseCashLimit = sUserID;
                    }
                }
                ////Addedd to double check if the tread is still runing
                //if (MaxStnCloseCashLimitThread != null && MaxStnCloseCashLimitThread.ThreadState == System.Threading.ThreadState.Running) {
                //    MaxStnCloseCashLimitThread.Abort();
                //    logger.Trace("showPayTypes() - Station close Cash Limit thread aborted");
                //}
                //Addedd to double check if the thread is still runing
                //if (frmPOSTransaction.MaxStnCloseCashLimitThread != null && frmPOSTransaction.MaxStnCloseCashLimitThread.ThreadState == System.Threading.ThreadState.Running)
                //{
                //    frmPOSTransaction.MaxStnCloseCashLimitThread.Abort();
                //    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Station close Cash Limit thread", "Aborted");
                //    logger.Trace("showPayTypes() - Station close Cash Limit thread aborted");
                //}
                //End
                /*Date 27-jan-2014
                * Modified by Shitaljit
                * For making currency symbol dynamic
                */
                //old code
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "ShowPaytypes Item Count " + this.grdDetail.Rows.Count + "  Total Amount Due $" + totalAmount + "  ", clsPOSDBConstants.Log_Entering);
                //new code
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "ShowPaytypes Item Count " + this.grdDetail.Rows.Count + "  Total Amount Due " + Configuration.CInfo.CurrencySymbol.ToString() + totalAmount + "  ", clsPOSDBConstants.Log_Entering);
                logger.Trace("showPayTypes() - ShowPaytypes Item Count " + this.grdDetail.Rows.Count + "  Total Amount Due " + Configuration.CInfo.CurrencySymbol.ToString() + totalAmount + "  :" + clsPOSDBConstants.Log_Entering);

                //Added By Manoj 9/27/2013 If the transaction is 0 and ByPassPayScreen is check, by pass the payment screen
                if ((Configuration.CInfo.ByPassPayScreen && totalAmount == Convert.ToDecimal(0)) || (this.PendingAmount == 0 && oPTList.IsCustomerDriven))//2915
                {
                    //oPTList.Show();
                    bool retval = oPTList.IsCustomerDriven;
                    oPTList.Visible = true; //PRIMEPOS-2988 06-Aug-2021 JY Moved to its original place
                    oPTList.Visible = false;
                    if (this.PendingAmount == 0 && oPTList.IsCustomerDriven)
                    {
                        this.IsRemoveOnHoldPayment = true;
                    }
                    if (oPTList.grdPayment.ActiveCell != null)
                    {
                        if (!(this.PendingAmount == 0 && retval))//2915
                        {
                            #region PRIMEPOS-2988 06-Aug-2021 JY Always use the "Cash" Row for "0" payment
                            bool bSucceed = false;
                            try
                            {
                                foreach (UltraGridRow oRow in oPTList.grdPayment.Rows)
                                {
                                    if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim().ToUpper() == "1")
                                    {
                                        bSucceed = true;
                                        oPTList.AddZeroAmount(oRow);
                                        break;
                                    }
                                }
                            }
                            catch { }
                            if (bSucceed == false)
                                oPTList.AddZeroAmount(oPTList.grdPayment.ActiveCell.Row);
                            #endregion
                        }
                    }
                    //2915
                    oPTList.IsCustomerDriven = this.IsCustomerDriven;
                    oPTList.Close();
                    oPTList.txtAmtPaid.Text = "0.00";
                }
                else
                {
                    oPTList.IsHouseChargeAccount(); //Sprint-24 - PRIMEPOS-2277 03-Nov-2016 JY Added
                    oPTList.ShowDialog(this);
                }
                this.txtAmtTendered.Value = oPTList.oPosPTList.tenderedAmount;
                if (!this.IsCustomerDriven)//2915
                    this.IsCustomerDriven = oPTList.IsCustomerDriven;//2915
                this.PrimeRxPayTransID = oPTList.PrimeRxPayTransID;
                //end By Pass Payment Screen
                if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                {
                    this.controlNumber = oPTList.controlNumber;
                }
                #region PRIMEPOS-2663
                this.txtS3TransID.Text = oPTList.oPosPTList.S3TransID;
                this.txtS3PurAmt.Text = oPTList.oPosPTList.S3PurAmount.ToString();
                this.txtS3DiscAmt.Text = oPTList.oPosPTList.S3DiscAmount.ToString();
                this.txtS3TaxAmt.Text = oPTList.oPosPTList.S3TaxAmount.ToString();
                #endregion
                #region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019
                if (Configuration.CPOSSet.EnableStoreCredit)
                {
                    if (oPTList.oPosPTList.IsStoreCredit)
                    {
                        oStoreCreditData = oPTList.oPosPTList.oStoreCreditData;
                        this.txtCustomer.Tag = oPTList.CustomerTag;
                        this.txtCustomer.Text = oPTList.CustomerText;
                        this.lblCustomerName.Text = oPTList.CustomerName;

                    }
                    IsStoreCredit = oPTList.oPosPTList.IsStoreCredit;
                }
                #endregion
                if (oPTList.oPosPTList.CancelTransaction == true)
                {
                    /*Date 27-jan-2014
                    * Modified by Shitaljit
                    * For making currency symbol dynamic
                    */
                    logger.Trace("showPayTypes() - Cancel Transaction Item Count " + this.grdDetail.Rows.Count + "  Total Amount Due " + Configuration.CInfo.CurrencySymbol.ToString() + totalAmount + "  :" + clsPOSDBConstants.Log_Exiting);

                    this.SetNew(true);
                    //Added by SRT(Abhishek) Date : 19 Aug 2009
                    oPOSTrans.countUnBilledRx = 0;
                    oPOSTrans.unbilledRx = "";

                    #region Sprint-23 - PRIMEPOS-2029 19-Jul-2016 JY Added
                    if (pseTrxId != string.Empty)
                    {
                        NplexBL oNplex = new NplexBL();
                        VoidResponseType voidResponse = oNplex.DoVoid(pseTrxId);
                        if (voidResponse.trxStatus.resultCode == 0)
                        {
                            logger.Trace("showPayTypes() - nPlex Void request success for Purchase TransID: " + pseTrxId);
                        }
                        else
                        {
                            logger.Trace("showPayTypes() - nPlex Void request error for Purchase TransID: " + pseTrxId + " - " + voidResponse.trxStatus.errorMsg);
                        }
                    }
                    #endregion
                }
                else if (oPTList.oPosPTList.oPOSTransPaymentDataPaid == null)
                {
                    if (Configuration.CPOSSet.UseSigPad == true)
                    {
                        SigPadUtil.DefaultInstance.ShowItemsScreen();
                    }
                    this.oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                    this.txtAmtBalanceDue.Text = "0";
                    this.txtAmtChangeDue.Text = "0";
                    this.ChangeDue = 0;
                    this.txtAmtTendered.Value = 0;
                    //Added By Shitaljit(QuicSolv) on 31 August 2011
                    if (oPOSTrans.CurrentTransactionType != POS_Core.TransType.POSTransactionType.SalesReturn)
                    {
                        this.lblInvDiscount.Text = "0";//Orignal commented by krishna on 29 June 2011
                        #region PRIMEPOS-2768 02-Jan-2020 JY Added
                        foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                        {
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].Value = 0;
                        }
                        #endregion
                        strInvDiscOverrideUser = "";    //PRIMEPOS-2402 26-Jul-2021 JY Added

                        //Following Code Added by Krishna on 29 June 2011
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && Configuration.convertNullToDecimal(lblInvDiscount.Text) != 0)
                        {
                            oPOSTrans.oTDRow.Discount = Configuration.convertNullToDecimal(lblInvDiscount.Text);
                            this.lblInvDiscount.Text = "0";
                        }
                        else
                        {
                            this.lblInvDiscount.Text = "0";
                            custDiscount = 0;
                            IsHundredPerInvDisc = false;
                            bContinuePayment = true;
                        }
                        //Till here Added by Krishna on 29 June 2011
                        this.RecalculateTax();

                        #region Sprint-23 - PRIMEPOS-2029 14-Apr-2016 JY Added
                        if (pseTrxId != string.Empty)
                        {
                            NplexBL oNplex = new NplexBL();
                            VoidResponseType voidResponse = oNplex.DoVoid(pseTrxId);
                            if (voidResponse.trxStatus.resultCode == 0)
                            {
                                logger.Trace("showPayTypes() - nPlex Void request success for Purchase TransID: " + pseTrxId);
                            }
                            else
                            {
                                logger.Trace("showPayTypes() - nPlex Void request error for Purchase TransID: " + pseTrxId + " - " + voidResponse.trxStatus.errorMsg);
                            }
                        }
                        #endregion
                    }
                    if (Math.Abs(Configuration.convertNullToDecimal(this.lblCouponDiscount.Text)) > 0)
                    {
                        RemoveCouponDiscount();
                    }
                    if (TotalTaxForEBTItems > 0)
                    {
                        AddBackTaxToEBTItems();
                        TotalTaxForEBTItems = 0;
                    }
                    Configuration.IsDiscOverridefromPOSTrans = false;//Added By shitaljit on 2/11/2014 for JIRA PRIMEPOS-1810
                    //Till Here Added By shitaljit on 31 August 2011.

                    /*Date 27-jan-2014
                    * Modified by Shitaljit
                    * For making currency symbol dynamic
                    */
                    //old code
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Payment Cancel Item Count " + this.grdDetail.Rows.Count + "  Total Amount Due $" + totalAmount + "  ", clsPOSDBConstants.Log_Exiting);
                    // new code
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Payment Cancel Item Count " + this.grdDetail.Rows.Count + "  Total Amount Due " + Configuration.CInfo.CurrencySymbol.ToString() + totalAmount + "  ", clsPOSDBConstants.Log_Exiting);
                    logger.Trace("showPayTypes() - Payment Cancel Item Count " + this.grdDetail.Rows.Count + "  Total Amount Due " + Configuration.CInfo.CurrencySymbol.ToString() + totalAmount + "  :" + clsPOSDBConstants.Log_Exiting);

                    this.txtItemCode.Focus();
                }
                else if (oPTList.IsCustomerDriven && oPTList.IsStatusPending)//2915
                {
                    if (this.oPOSTrans.oPOSTransPaymentData != null && this.oPOSTrans.oPOSTransPaymentData.Tables.Count > 0 && this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows.Count > 0)//2915
                    {
                        this.oPOSTrans.oPOSTransPaymentData.Merge(oPTList.oPosPTList.oPOSTransPaymentDataPaid);
                        Decimal TotalAmount = 0;
                        for (int i = 0; i < this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows.Count; i++)
                        {
                            TotalAmount += Convert.ToDecimal(this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["Amount"]);
                        }
                        this.txtAmtTendered.Value = TotalAmount;
                    }
                    else
                    {
                        this.oPOSTrans.oPOSTransPaymentData = oPTList.oPosPTList.oPOSTransPaymentDataPaid;
                        this.txtAmtTendered.Value = Convert.ToDecimal(oPTList.txtAmtPaid.Text);
                    }
                    #region PRIMEPOS-3189
                    if (this.oPOSTrans.oPOSTransPaymentData != null && this.oPOSTrans.oPOSTransPaymentData.Tables.Count > 0)
                    {
                        for (int i = 0; i < this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["PrimeRxPayTransID"].ToString())
                                && string.IsNullOrWhiteSpace(this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["ApprovedAmount"].ToString()))
                            {
                                this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["ApprovedAmount"] = this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["Amount"];
                            }
                        }
                    }
                    #endregion
                    if (oPTList.oPosPTList.oCurrentCustRow != null && oPOSTrans.oCustomerRow.CustomerId != oPTList.oPosPTList.oCurrentCustRow.CustomerId)//oPTList.is_F10 == true && 
                    {
                        this.oPOSTrans.oCustomerRow = oPTList.oPosPTList.oCurrentCustRow;
                        EditCustomer(oPOSTrans.oCustomerRow.CustomerId.ToString(), clsPOSDBConstants.Customer_tbl);
                    }
                    TransFeeAmt = oPTList.oPosPTList.TransFeeAmt;   //PRIMEPOS-3117 11-Jul-2022 JY Added
                    PutOnHold("P");
                    this.ChangeDue = oPTList.oPosPTList.ChangeDue;
                    //if (oPTList.tenderedAmount != 0)
                    //    this.txtAmtTendered.Value = oPTList.tenderedAmount;
                    //else                    
                    this.txtAmtChangeDue.Text = Convert.ToString(this.ChangeDue);

                    if (this.MdiParent != null)
                    {
                        //((frmMain)this.MdiParent).ultraOnHoldStatusPanel.Text = "On Hold Trans : " + Configuration.GetOnHoldCount().Tables[0].Rows[0][0];
                        ((frmMain)this.MdiParent).ultraOnHoldStatusPanel = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel
                        {
                            Text = "On Hold Trans : " + Configuration.GetOnHoldCount().Tables[0].Rows[0][0]
                        };//PRIMEPOS-3056
                    }
                }
                else
                {
                    if (this.oPOSTrans.oPOSTransPaymentData != null && this.oPOSTrans.oPOSTransPaymentData.Tables.Count > 0 && this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows.Count > 0)//2915
                    {
                        this.oPOSTrans.oPOSTransPaymentData.Merge(oPTList.oPosPTList.oPOSTransPaymentDataPaid);
                        Decimal TotalAmount = 0;
                        for (int i = 0; i < this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows.Count; i++)
                        {
                            if (Convert.ToString(this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["ApprovedAmount"]) != string.Empty)
                            {
                                TotalAmount += Convert.ToDecimal(this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["ApprovedAmount"]);
                            }
                            else
                                TotalAmount += Convert.ToDecimal(this.oPOSTrans.oPOSTransPaymentData.Tables[0].Rows[i]["Amount"]);
                            this.txtAmtTendered.Value = TotalAmount;
                        }
                    }
                    else
                    {
                        this.oPOSTrans.oPOSTransPaymentData = oPTList.oPosPTList.oPOSTransPaymentDataPaid;
                        this.txtAmtTendered.Value = Convert.ToDecimal(oPTList.txtAmtPaid.Text);
                    }
                    TransFeeAmt = oPTList.oPosPTList.TransFeeAmt;   //PRIMEPOS-3117 11-Jul-2022 JY Added
                    this.ChangeDue = oPTList.oPosPTList.ChangeDue;
                    //if (oPTList.tenderedAmount != 0)
                    //    this.txtAmtTendered.Value = oPTList.tenderedAmount;
                    //else                    
                    this.txtAmtChangeDue.Text = Convert.ToString(this.ChangeDue);
                    oPOSTransPayment_CCLogList = oPTList.oPosPTList.POSTransPaymentList_CCLogList;
                    if (oPOSTrans.oTransHRow.IsDelivery == false)
                    {
                        this.oPOSTrans.oTransHRow.IsDelivery = oPTList.IsDelivery;
                        this.oPOSTrans.oTransHRow.DeliveryAddress = oPTList.DeliveryAddress;
                    }
                    if (oPTList.oPosPTList.oCurrentCustRow != null && oPOSTrans.oCustomerRow.CustomerId != oPTList.oPosPTList.oCurrentCustRow.CustomerId)//oPTList.is_F10 == true && 
                    {
                        this.oPOSTrans.oCustomerRow = oPTList.oPosPTList.oCurrentCustRow;
                        EditCustomer(oPOSTrans.oCustomerRow.CustomerId.ToString(), clsPOSDBConstants.Customer_tbl);
                    }

                    // Added By Rohit Nair For Tokenisation                    
                    oPOSTrans.SaveCCToken();
                    Configuration.isPrimeRxPay = false;//PRIMEPOS-TOKENSALE

                    /*Date 27-jan-2014
                    * Modified by Shitaljit
                    * For making currency symbol dynamic
                    */
                    logger.Trace("showPayTypes() - Payment Successfull Item Count " + this.grdDetail.Rows.Count + "  Total Amount Paid " + Configuration.CInfo.CurrencySymbol.ToString() + this.txtAmtTendered.Value);
                    bPrintGiftRecpt = oPTList.PrintGiftReciept;//Added By Shitaljit for printing gift receipt on 21 Jan 2013
                    btnSave();
                    #region PRIMEPOS-2738 REVERSAL
                    if (oPTList.oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true)
                    {
                        if (oPTList.oOrigPayTransData != null && oPTList.oOrigPayTransData.Tables.Count > 0)//Not Applicable for Cash and StoreCredit return transaction 
                            oPTList.oPosTrans.SetReversedAmountDetails(oPTList.oOrigPayTransData);
                    }
                    #endregion
                    //Added by SRT(Abhishek) Date : 19 Aug 2009
                    oPOSTrans.countUnBilledRx = 0;
                    oPOSTrans.unbilledRx = "";
                    //End of Added by SRT(Abhishek) Date : 19 Aug 2009
                }

                clsUIHelper.SETACTIVEWINDOW(this.Handle);
                txtItemCode.Focus();
                logger.Trace("showPayTypes() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "showPayTypes()");
                string warnMsg = exp.Message + "\nAn error has occurred while processing your request.\nPlease stop and call Micro Merchant systems technical support to resolve this issue.";
                MessageBox.Show(warnMsg);
                this.lblInvDiscount.Text = "0";
                RecalculateTax();
            }
            finally
            {
                clsUIHelper.CurrentForm = this;
                this.Cursor = Cursors.Default;
            }
        }

        //completed by sandeep
        private void btnSave()
        {
            try
            {
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave() ------->>> Save Transaction <<<<------- " + clsPOSDBConstants.Log_Entering);
                this.txtItemCode.Enabled = false;

                #region PRIMEPOS-3012 15-Nov-2021 JY Added
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    oPOSTrans.oTransHData.TransHeader[0].ReturnTransID = oPOSTrans.oTransHRow.ReturnTransID;
                }
                #endregion



                if (!this.IsCustomerDriven)//2915
                    oPOSTrans.oTransHRow = oPOSTrans.oTransHData.TransHeader[0];

                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Save Transaction - About to assign oTransHRow");
                oPOSTrans.oTransHRow = oPOSTrans.oTransHData.TransHeader[0];
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Save Transaction - Complete assigning oTransHRow");
                //Added By SRT(Ritesh Parekh) Date : 20-Jul-2009
                //Added following code to validate custid and assign -1 if found null.
                Int32 tempCustId = -1;
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Save Transaction - Complete assigning oTransHRow");
                bool isCLTierreached = false;
                decimal CLCouponValue = 0;  //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value  

                if (!Int32.TryParse(this.txtCustomer.Tag.ToString(), out tempCustId))
                {
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - TryParse: " + this.txtCustomer.Tag.ToString());

                    CustomerRow oCustomerRow = null;
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - TryParse, Before Populate(-1)");
                    CustomerData oCustomerData = oPOSTrans.PopulateCustomer("-1", true);
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - TryParse, After Populate(-1)");
                    if (oCustomerData == null || oCustomerData.Customer.Count == 0)
                    {
                        MessageBox.Show("Default Customer is not selected. Receipt will not be printed. Please contact MMS for support.");
                        logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - TryParse, Default Customer is not selected");
                        tempCustId = -1;
                    }
                    else
                    {
                        logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - About to get customer Data");
                        oCustomerRow = oCustomerData.Customer[0];
                        logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Customer Data completed");
                        if (oCustomerRow != null)
                        {
                            logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Customer Data NOT null");
                            this.txtCustomer.Text = oCustomerRow.AccountNumber.ToString();
                            this.lblCustomerName.Text = oCustomerRow.CustomerFullName;
                            ShowCustomerTokenImage(oCustomerRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                            this.txtCustomer.Tag = oCustomerRow.CustomerId;
                            tempCustId = oCustomerRow.CustomerId;
                            logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Customer Data NOT null, customerID: " + tempCustId);
                        }
                        else
                        {
                            MessageBox.Show("Default Customer not found. Please Contact MMS Customer support");
                            logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - TryParse, Default Customer not found");
                            tempCustId = -1;
                        }
                    }
                }
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Validating Customer Completed");
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Assigning values, Customer ID: " + tempCustId);

                setTransHRow(tempCustId);
                //-------------------
                //Following Code Added By Krishna on 14 July 2011
                //Need to Add the Logic here for retrieving the Transa detail Id of the Item being deleted.
                if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && oPOSTrans.oTransHRow.ReturnTransID != 0)
                {
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Marking Item(s) as Return");
                    int ReturnTransId = oPOSTrans.oTransHRow.ReturnTransID;
                    oPOSTrans.SetLoyalityPointForTransaction();
                    DataSet oTransDetData = oPOSTrans.PopulateTransactionList(" Where TransId = '" + ReturnTransId + "' AND TransDetailId NOT IN(Select ReturnTransDetailId FROM POSTransactionDetail Where ReturnTransDetailId  IN (" + oPOSTrans.GetTransactionDetailString(ReturnTransId) + ")) ORDER BY TransDetailID ");
                    //This for loop and column added on 7 December 2011 to resolve the return issue for Unique Items is grid
                    //Modified  "SelectClause" remove ItemDescription and ADD ItemID
                    int Index = 0;
                    grdDetail.ActiveCell = grdDetail.Rows[0].Cells[0];
                    foreach (UltraGridRow ActRow in grdDetail.Rows)
                    {
                        grdDetail.ActiveRow = ActRow;
                        //string itemID = Configuration.convertNullToString(grdDetail.ActiveRow.Cells["ItemID"].Value); //Shitaljit(QuicSolv) on 9 Feb 2012
                        //decimal discount = (-1) * Math.Abs(Configuration.convertNullToDecimal(grdDetail.ActiveRow.Cells["Discount"].Value));
                        //decimal taxAmount = (-1) * Math.Abs(Configuration.convertNullToDecimal(grdDetail.ActiveRow.Cells["TaxAmount"].Value));
                        //decimal Price = Math.Abs(Configuration.convertNullToDecimal(grdDetail.ActiveRow.Cells["Price"].Value));
                        //int Qty = (-1) * Math.Abs(Configuration.convertNullToInt(grdDetail.ActiveRow.Cells["Qty"].Value));
                        string ItemDesc = grdDetail.ActiveRow.Cells["ItemDescription"].Value.ToString();

                        //string SelectClause = @" ItemID = " + "'" + itemID + "'" + " AND Discount=" + discount + " AND TaxAmount=" + taxAmount + " AND Qty=" + Qty + " AND Price=" + Price + " AND Picked =0 ";   //PRIMEPOS-3012 07-Oct-2021 JY Commented
                        //DataRow[] TempRow = oTransDetData.Tables[0].Select(SelectClause);
                        ////Added this logic as we does not have ReturnTransDetailId for items those were not in original transaction.
                        ////we does not need to run the bellow logic for those items.
                        //if (Index >= oTransDetData.Tables[0].Rows.Count || TempRow.Length == 0)
                        //{
                        //    break;
                        //}
                        ////Added "SelectedIndex" to marked "Picked = True" of the corerect item.
                        //int SelectedIndex = Convert.ToInt16(TempRow[0]["RowIndex"]);
                        //oPOSTrans.oTransDData.Tables[0].Rows[Index]["ReturnTransDetailId"] = int.Parse(TempRow[0]["TransDetailId"].ToString());
                        //oTransDetData.Tables[0].Rows[SelectedIndex]["Picked"] = true;
                        ////oTransDetData.Tables[0].Rows[Index]["Picked"] = true;                        

                        #region PRIMEPOS-3012 07-Oct-2021 JY Added
                        try
                        {
                            if (Configuration.convertNullToInt(oPOSTrans.oTransDData.Tables[0].Rows[Index]["ReturnTransDetailId"]) == 0)
                            {
                                int nSecondOccurance = ItemDesc.IndexOf('-', ItemDesc.IndexOf('-') + 1);
                                string sRxNo = "";
                                if (nSecondOccurance != -1)
                                    sRxNo = ItemDesc.Substring(0, nSecondOccurance);
                                if (sRxNo != "")
                                {
                                    string SelectClause = @" ItemDescription LIKE " + "'" + sRxNo + "%'";
                                    TransDetailRow dr = (TransDetailRow)oPOSTrans.oTransDData.Tables[0].Select(SelectClause).FirstOrDefault();
                                    if (dr != null)
                                    {
                                        string[] arr = ItemDesc.Split('-');
                                        if (arr.Length > 1)
                                            dr.ReturnTransDetailId = oPOSTrans.GetTransDetailID(arr[0], arr[1]);
                                    }
                                }
                                #region PRIMEPOS-3069 28-Mar-2022 JY Added
                                else
                                {
                                    try
                                    {
                                        string itemID = Configuration.convertNullToString(grdDetail.ActiveRow.Cells["ItemID"].Value); //Shitaljit(QuicSolv) on 9 Feb 2012
                                        decimal discount = Math.Abs(Configuration.convertNullToDecimal(grdDetail.ActiveRow.Cells["Discount"].Value));
                                        decimal taxAmount = Math.Abs(Configuration.convertNullToDecimal(grdDetail.ActiveRow.Cells["TaxAmount"].Value));
                                        decimal Price = Math.Abs(Configuration.convertNullToDecimal(grdDetail.ActiveRow.Cells["Price"].Value));
                                        int Qty = Math.Abs(Configuration.convertNullToInt(grdDetail.ActiveRow.Cells["Qty"].Value));
                                        string SelectClause = @" ItemID = " + "'" + itemID + "'" + " AND Discount=" + discount + " AND TaxAmount=" + taxAmount + " AND Qty=" + Qty + " AND Price=" + Price + " AND Picked =0 ";
                                        DataRow[] TempRow = oTransDetData.Tables[0].Select(SelectClause);
                                        //Added this logic as we does not have ReturnTransDetailId for items those were not in original transaction.
                                        //we does not need to run the bellow logic for those items.
                                        if (Index >= oTransDetData.Tables[0].Rows.Count || TempRow.Length == 0)
                                        {
                                            break;
                                        }
                                        //Added "SelectedIndex" to marked "Picked = True" of the corerect item.
                                        int SelectedIndex = Convert.ToInt16(TempRow[0]["RowIndex"]);
                                        oPOSTrans.oTransDData.Tables[0].Rows[Index]["ReturnTransDetailId"] = int.Parse(TempRow[0]["TransDetailId"].ToString());
                                        oTransDetData.Tables[0].Rows[SelectedIndex]["Picked"] = true;
                                        oTransDetData.Tables[0].Rows[Index]["Picked"] = true;
                                    }
                                    catch { }
                                }
                                #endregion
                            }
                        }
                        catch { }
                        #endregion
                        Index++;
                    }
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Marking Item(s) as Return Completed");
                }
                //Added by Manoj to Save the Information for Control drugs class.
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - About to Capture drug information");
                oPOSTrans.CaptureDrugInformation(RxWithValidClass, DrugClassInfoCapture);
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Finish Capturing drug information completed");
                //Till here Added by Krishna on 14 July 2011
                //-------------------
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Initiating to write data to Database.");
                //Sprint-23 - PRIMEPOS-2029 06-Apr-2016 JY Added MethItem, PatInfo, inquiryId, pseTrxId parameters
                oPOSTrans.Persist(oPOSTrans.oTransHData, oPOSTrans.oTransDData, oPOSTrans.oPOSTransPaymentData, onHoldTransID, this.GetDiscPercentage(), false, oPOSTrans.oRXHeaderList, oPOSTransPayment_CCLogList, oPOSTrans.oTransDRXData, oCLCardRow, oPOSTrans.oTransSignLogData, RxWithValidClass, oPOSTrans.oTDTaxData, ref isCLTierreached, ref CLCouponValue, pseTrxId, bItemMonitorInTrans, lstOnHoldRxs, isBatchDelivery, strOverrideMaxStationCloseCashLimit, strMaxTransactionAmountUser, strMaxReturnTransactionAmountUser, strInvDiscOverrideUser, strMaxDiscountLimitOverrideUser, dtSelectedRx);   //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value  //PRIMEPOS-2639 27-Mar-2019 JY Added lstOnHoldRxs   // PRIMERX-7688 - NileshJ - BatchDelivery   //PRIMEPOS-2402 12-Jul-2021 JY Added strOverrideMaxStationCloseCashLimit, strMaxTransactionAmountUser, strMaxReturnTransactionAmountUser, strInvDiscOverrideUser, strMaxDiscountLimitOverrideUser //PRIMEPOS-3192 Added dtSelectedRx
                if (RxWithValidClass != null && RxWithValidClass.Rows.Count > 0)
                {
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - About to clear RxWithValidClass");
                    RxWithValidClass.Clear();
                    if (DrugClassInfoCapture != null && DrugClassInfoCapture.Rows.Count > 0)
                        DrugClassInfoCapture.Clear();   //PRIMEPOS-2547 23-Jul-2018 JY Added
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Finish clearing RxWithValidClass");
                }
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Initiating DisplayCashPmtOnDevice()");
                    SigPadUtil.DefaultInstance.DisplayCashPmtOnDevice(txtAmtSubTotal.Text, this.txtAmtTax.Text, this.txtAmtTotal.Text, String.Format("{0:0.00}", Convert.ToDecimal(this.txtAmtTendered.Value)), this.txtAmtChangeDue.Text);
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Completed DisplayCashPmtOnDevice()");
                }
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Initiating Display Change due");
                decimal dChangeDue = oPOSTrans.GetChangeDueFromPaymentTrans(txtAmtChangeDue.Text);
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - About to pass change due amount");

                #region Audit Trail - PRIMEPOS-2808 
                if (oAuditTrail.oAuditDataSet.Tables.Count > 0)
                {
                    oAuditTrail.InsertPriceTaxOverride(oAuditTrail.oAuditDataSet);
                }
                #endregion
                #region PRIMEPOS-3403
                if (Configuration.CInfo.PIEnable && Configuration.eInterfaceStatus == EventHub.InterfaceStatus.ServiceIsConnected)
                {
                    if (oPOSTrans.dsOrgRxData != null && oPOSTrans.dsOrgRxData.Tables.Count > 0 && oPOSTrans.dsOrgRxData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in oPOSTrans.dsOrgRxData.Tables[0].Rows)
                        {
                            string ItemID = "";
                            foreach (DataRow row in oPOSTrans.oTransDData.Tables[0].Rows)
                            {
                                if (row["ItemDescription"].ToString().Contains(dr["RXNO"].ToString()))
                                {
                                    ItemID = row["ItemID"].ToString();
                                }
                            }

                            if (ItemID == "RX")
                            {
                                PharmBL oPBL = new PharmBL();
                                List<PrimeRxIntData> rxData = new List<PrimeRxIntData>();
                                DataTable dsRxData = oPBL.GetRxs(dr["RXNO"].ToString(), dr["NREFILL"].ToString());
                                DataTable oPat = oPBL.GetPatient(dsRxData.Rows[0]["PATIENTNO"].ToString());
                                DataTable oPres = oPBL.GetDoctor(dsRxData.Rows[0]["PRESNO"].ToString());
                                DataTable oDrug = oPBL.GetPhDrug(dsRxData.Rows[0]["NDC"].ToString());
                                DataTable oPharmacy = oPBL.GetPhInfo();
                                string itemID = oPOSTrans.oTransDData.ToString();
                                PrimeRxIntData oPrimeRxIntData = new PrimeRxIntData();
                                oPrimeRxIntData.pharmacyInt = new PharmacyInt();
                                oPrimeRxIntData.patient = ConvertPatientDataToInterface(oPat);
                                oPrimeRxIntData.prescriber = ConvertPrescribDataToInterface(oPres);
                                oPrimeRxIntData.drug = ConvertDrugDataToInterface(oDrug);
                                oPrimeRxIntData.claims = ConvertClaimsDataToInterface(dsRxData);
                                oPrimeRxIntData.pharmacyInt = ConvertPharmacyDataToInterface(oPharmacy);
                                rxData.Add(oPrimeRxIntData);
                                EventHub.getInstance().RaiseEvent(PrimeRxEvents.RxSaved, rxData);
                            }
                        } 
                    }
                }
                #endregion
                frmPOSChangeDue oChagneDue = new frmPOSChangeDue(oPOSTrans.oTransHData.TransHeader[0], dChangeDue, oPOSTrans.oPOSTransPaymentData.POSTransPayment, oStoreCreditData, IsStoreCredit, txtCustomer.Text);    //PRIMEPOS-2384 29-Oct-2018 JY Added POSTransPayment         // PRIMEPOS-2747 - NileshJ - 20-Nov-2019 -Store Credit - 
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Finish passing change due amount");
                try
                {
                    oPOSTrans.controlNumber = this.controlNumber;//PRIMEPOS-2664
                    RxLabel oRxLabel = oPOSTrans.GetRxLabelForPrinting(bPrintGiftRecpt, isCLTierreached, CLCouponValue);
                    if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                    {
                        oRxLabel.AuthNo = oRxLabel.CCPaymentRow.AuthNo;
                        oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ProcessorTransID;
                        oRxLabel.Trace = oRxLabel.CCPaymentRow.TraceNumber;
                        oRxLabel.Batch = oRxLabel.CCPaymentRow.BatchNumber;
                        oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID;
                        oRxLabel.InvoiceNumber = oRxLabel.CCPaymentRow.InvoiceNumber;
                        oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                        oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                        oRxLabel.IsEvertecForceTransaction = oRxLabel.CCPaymentRow.IsEvertecForceTransaction; //primepos-2831
                        oRxLabel.IsEvertecSign = oRxLabel.CCPaymentRow.IsEvertecSign; //primepos-2831
                        if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            oRxLabel.EvertecCityTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                        }
                        oRxLabel.EvertecCashback = oRxLabel.CCPaymentRow.CashBack;//PRIMEPOS-2857
                        oRxLabel.EntryMethod = oRxLabel.CCPaymentRow.EntryMethod;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.CCPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.ATHMovil))
                            oRxLabel.ATHMovil = oRxLabel.CCPaymentRow.ATHMovil.Substring(2, oRxLabel.CCPaymentRow.ATHMovil.Length - 2);//2664
                    }
                    else if (oRxLabel.EBTPaymentRow != null && Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                    {
                        oRxLabel.AuthNo = oRxLabel.EBTPaymentRow.AuthNo;
                        oRxLabel.ReferenceNumber = oRxLabel.EBTPaymentRow.ProcessorTransID;
                        oRxLabel.Trace = oRxLabel.EBTPaymentRow.TraceNumber;
                        oRxLabel.Batch = oRxLabel.EBTPaymentRow.BatchNumber;
                        oRxLabel.MerchantID = oRxLabel.EBTPaymentRow.MerchantID;
                        oRxLabel.InvoiceNumber = oRxLabel.EBTPaymentRow.InvoiceNumber;
                        oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                        oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                        #region PRIMEPOS-2664 EVERTEC EBTBALANCE
                        if (oRxLabel.EBTPaymentRow.EbtBalance.Length >= 3)
                        {
                            oRxLabel.FoodBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[0];
                            oRxLabel.CashBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[1];
                        }
                        #endregion
                        oRxLabel.IsEvertecForceTransaction = oRxLabel.EBTPaymentRow.IsEvertecForceTransaction; //primepos-2831
                        oRxLabel.IsEvertecSign = oRxLabel.EBTPaymentRow.IsEvertecSign; //primepos-2831
                        if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.EvertecTaxBreakdown))//PRIMEPOS-2857
                        {
                            oRxLabel.EvertecStateTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            oRxLabel.EvertecCityTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                        }
                        oRxLabel.EvertecCashback = oRxLabel.EBTPaymentRow.CashBack;//PRIMEPOS-2857
                        oRxLabel.ControlNumber = oRxLabel.EBTPaymentRow.ControlNumber;//PRIMEPOS-2857
                        if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.ATHMovil))
                            oRxLabel.ATHMovil = oRxLabel.EBTPaymentRow.ATHMovil.Substring(2, oRxLabel.EBTPaymentRow.ATHMovil.Length - 2);//2664
                    }
                    if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")//2664
                    {
                        if (oRxLabel.CashPaymentRow != null)
                        {
                            oRxLabel.AuthNo = oRxLabel.CashPaymentRow.AuthNo;
                            oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ProcessorTransID;
                            oRxLabel.Trace = oRxLabel.CashPaymentRow.TraceNumber;
                            oRxLabel.Batch = oRxLabel.CashPaymentRow.BatchNumber;
                            oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
                            oRxLabel.InvoiceNumber = oRxLabel.CashPaymentRow.InvoiceNumber;
                            oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                            oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                            oRxLabel.IsEvertecForceTransaction = oRxLabel.CashPaymentRow.IsEvertecForceTransaction; //primepos-2831
                            oRxLabel.IsEvertecSign = oRxLabel.CashPaymentRow.IsEvertecSign; //primepos-2831
                            oRxLabel.ControlNumber = oRxLabel.CashPaymentRow.ControlNumber;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                oRxLabel.EvertecCityTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            }
                            oRxLabel.EntryMethod = oRxLabel.CashPaymentRow.EntryMethod;//PRIMEPOS-2857
                            oRxLabel.ControlNumber = oRxLabel.CashPaymentRow.ControlNumber;//PRIMEPOS-2857
                            oRxLabel.EvertecCashback = oRxLabel.CashPaymentRow.CashBack;//PRIMEPOS-2857
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.ATHMovil))
                                oRxLabel.ATHMovil = oRxLabel.CashPaymentRow.ATHMovil.Substring(2, oRxLabel.CashPaymentRow.ATHMovil.Length - 2);//2664
                        }
                        else if (oRxLabel.CheckPaymentRow != null)
                        {
                            oRxLabel.ControlNumber = oRxLabel.CheckPaymentRow.ControlNumber;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CheckPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.CheckPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                oRxLabel.EvertecCityTax = oRxLabel.CheckPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            }
                            oRxLabel.EntryMethod = oRxLabel.CheckPaymentRow.EntryMethod;//PRIMEPOS-2857
                            oRxLabel.ControlNumber = oRxLabel.CheckPaymentRow.ControlNumber;//PRIMEPOS-2857
                            oRxLabel.ATHMovil = oRxLabel.CheckPaymentRow.ATHMovil;//2664
                        }
                        else if (oRxLabel.CouponPaymentRow != null)
                        {
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CouponPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                oRxLabel.EvertecCityTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            }
                            oRxLabel.EntryMethod = oRxLabel.CouponPaymentRow.EntryMethod;//PRIMEPOS-2857
                            oRxLabel.ControlNumber = oRxLabel.CouponPaymentRow.ControlNumber;//PRIMEPOS-2857
                            oRxLabel.ATHMovil = oRxLabel.CouponPaymentRow.ATHMovil;//2664
                        }
                        else if (oRxLabel.HCPaymentRow != null)
                        {
                            oRxLabel.ControlNumber = oRxLabel.HCPaymentRow.ControlNumber;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.HCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                oRxLabel.EvertecCityTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            }
                            oRxLabel.EntryMethod = oRxLabel.HCPaymentRow.EntryMethod;//PRIMEPOS-2857
                            oRxLabel.ControlNumber = oRxLabel.HCPaymentRow.ControlNumber;//PRIMEPOS-2857
                            oRxLabel.ATHMovil = oRxLabel.HCPaymentRow.ATHMovil;//2664
                        }
                        else if (oRxLabel.CBPaymentRow != null)
                        {
                            oRxLabel.ControlNumber = oRxLabel.CBPaymentRow.ControlNumber;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CBPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                oRxLabel.EvertecCityTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            }
                            oRxLabel.EntryMethod = oRxLabel.CBPaymentRow.EntryMethod;//PRIMEPOS-2857
                            oRxLabel.ControlNumber = oRxLabel.CBPaymentRow.ControlNumber;//PRIMEPOS-2857
                            oRxLabel.ATHMovil = oRxLabel.CBPaymentRow.ATHMovil;//2664
                        }
                        if (oRxLabel.ATHPaymentRow != null)
                        {
                            oRxLabel.AuthNo = oRxLabel.ATHPaymentRow.AuthNo;
                            oRxLabel.ReferenceNumber = oRxLabel.ATHPaymentRow.ProcessorTransID;
                            oRxLabel.Trace = oRxLabel.ATHPaymentRow.TraceNumber;
                            oRxLabel.Batch = oRxLabel.ATHPaymentRow.BatchNumber;
                            oRxLabel.MerchantID = oRxLabel.ATHPaymentRow.MerchantID;
                            oRxLabel.InvoiceNumber = oRxLabel.ATHPaymentRow.InvoiceNumber;
                            oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                            oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                            oRxLabel.IsEvertecForceTransaction = oRxLabel.ATHPaymentRow.IsEvertecForceTransaction; //primepos-2831
                            oRxLabel.IsEvertecSign = oRxLabel.ATHPaymentRow.IsEvertecSign; //primepos-2831
                            if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                oRxLabel.EvertecCityTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            }
                            oRxLabel.EvertecCashback = oRxLabel.ATHPaymentRow.CashBack;//PRIMEPOS-2857
                            oRxLabel.EntryMethod = oRxLabel.ATHPaymentRow.EntryMethod;//PRIMEPOS-2857
                            oRxLabel.ControlNumber = oRxLabel.ATHPaymentRow.ControlNumber;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.ATHMovil))
                                oRxLabel.ATHMovil = oRxLabel.ATHPaymentRow.ATHMovil.Substring(2, oRxLabel.ATHPaymentRow.ATHMovil.Length - 2);//2664
                        }
                    }
                    OpenDrawAndPrintReceipt(oRxLabel);
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Done printing reciept (RX Else)");

                }
                catch (Exception ex)
                {
                    aboutToOpenDrawer = false;
                    logger.Fatal(ex, clsPOSDBConstants.Log_Module_Transaction + " btnSave()");
                }
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Completed printing reciept");
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    //Added By Dharmendra(SRT) on 02-09-08 to clear the card related information
                    if ((SigPadUtil.DefaultInstance.CardType == PayTypes.CreditCard || SigPadUtil.DefaultInstance.CardType == PayTypes.DebitCard) && (SigPadUtil.DefaultInstance.SigPadCardInfo.IsValidData == true))
                    {
                        SigPadUtil.DefaultInstance.ClearCardInfo();
                    }
                    //Added till here by SRT.
                }
                //Modified By Dharmendra on May-13-09
                // not to show change due screeen in case of change due amount is 0.00
                //Mofified by Akbar on Feb 19 2011
                // Please do not change the behaviour below without consulting with Ketan or me
                //if(Convert.ToDecimal(this.txtAmtChangeDue.Text)!= 0.0M)
                //{

                //PRIMEPOS-2534 (Suraj) 31-may-18 Suraj - Added VFInactivityMonitor for ingenico lane close issue
                StartVFInactivityMonitor(oChagneDue);
                oChagneDue.ShowDialog();
                StopVFInactivityMonitor();
                //

                /* Removed For Ingenico Lane Close Issue in change due form
                 oChagneDue.ShowDialog();
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Completed Display Change due");*/
                //}
                //Added Till Here May-13-09

                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    SigPadUtil.DefaultInstance.CompleteTransaction(Convert.ToDecimal(this.txtAmtTotal.Text));//NileshJ - Change txtChangeDUeAMout To txtAmtTotal
                    //SigPadUtil.DefaultInstance.CompleteTransaction(Convert.ToDecimal(this.txtAmtChangeDue.Text));
                }
                // Added for POSLITE - Close application after payment from PrimeRX
                if (IsPOSLite)
                {
                    //Application.Exit();
                    Environment.Exit(0);
                }

                #region Batch Delivery - PRIMERX-7688 - NileshJ
                if (isBatchDelivery)
                {
                    //Since the entire payment will be done for successful POSTranscation to close The copaycollected will be equated to copay amount and that needs to be updated back to Delivery tables
                    using (ReconciliationDeliveryReport oReconciliationDeliveryReport = new ReconciliationDeliveryReport())
                    {
                        for (int i = 0; i < this.oPOSTrans.oTransDRXData.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < dtRxDetailsData.Rows.Count; j++)
                            {
                                if (this.oPOSTrans.oTransDRXData.Tables[0].Rows[i]["RXNo"].ToString() == dtRxDetailsData.Rows[j]["RxNo"].ToString())
                                {
                                    dtRxDetailsData.Rows[j]["CopayCollectedPOS"] = dtRxDetailsData.Rows[j]["Copay"];
                                }
                            }
                        }
                        oReconciliationDeliveryReport.UpdateBatchDeliveryDetailPaymentStatus(dtRxDetailsData); //Delivery_Details table to be updated by PrimePOS. Which inturn will update Delviery_Order and Delivery_Batch tables

                        alreadyPaidAmount = 0;
                        //allowUnPickedRX = string.Empty;
                        this.tbTerminalActions("Esc");
                        SetFocusBack();

                    }
                }
                #endregion

                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Initiating SetNew(true)");
                SetNew(true);
                isStrictReturn = Configuration.CSetting.StrictReturn;// PRIMEPOS-2738
                #region PRIMERX-7688 - BatchDelivery - Call Delivery screen after payment with last batch no
                if (isBatchDelivery)
                {
                    isBatchDelivery = false;
                    if (!isLaunchedByDelivery)
                    {
                        frmReconciliationDeliveryReport objfrmReconciliationDeliveryReport = new frmReconciliationDeliveryReport(this, PosUiConstants.lastBatchNo);
                        objfrmReconciliationDeliveryReport.ShowDialog();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                #endregion
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Completed SetNew(true)");
                logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave() ------->>> Save Transaction <<<<------- " + clsPOSDBConstants.Log_Exiting);
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, clsPOSDBConstants.Log_Module_Transaction + " btnSave()");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                this.txtItemCode.Enabled = true;
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.TransHeader_CustomerIDCanNotNull:
                        this.txtCustomer.Focus();
                        break;
                    case (long)POSErrorENUM.TransHeader_DateIsInvalid:
                        this.dtpTransDate.Focus();
                        break;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, clsPOSDBConstants.Log_Module_Transaction + " btnSave()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            this.txtItemCode.Enabled = true;
            aboutToOpenDrawer = false;
        }

        //completed by sandeep
        public void OpenDrawAndPrintReceipt(RxLabel oRxLabel)
        {
            if (Convert.ToDouble(oPOSTrans.oTransHData.Tables[0].Rows[0]["TotalPaid"]) != 00.00 || Configuration.CInfo.OpenDrawerForZeroAmtTrans == true)
            {
                aboutToOpenDrawer = true;
                oPOSTrans.OpenDrawerOnTransComplete(oRxLabel);
                aboutToOpenDrawer = false;
            }
            if (oPOSTrans.isPrintAble(oPOSTrans.oTransDData))
            {
                PrintReceipt(oRxLabel);
            }

        }

        //completed by sandeep
        public void setTransHRow(int tempCustId)
        {
            oPOSTrans.oTransHRow.CustomerID = tempCustId; // updated By SRT(Gaurav) to solve naim issue of null custid parsing.
                                                          //End Of Added By SRT(Ritesh Parekh)
            oPOSTrans.oTransHRow.TransDate = (System.DateTime)this.dtpTransDate.Value;
            oPOSTrans.oTransHRow.TransType = Convert.ToInt32(oPOSTrans.CurrentTransactionType);

            if (!this.IsCustomerDriven)//PRIMEPOS-2915
            {
                oPOSTrans.oTransHRow.GrossTotal = Convert.ToDecimal(this.txtAmtSubTotal.Text);
                oPOSTrans.oTransHRow.TotalTaxAmount = Convert.ToDecimal(this.txtAmtTax.Text);
                oPOSTrans.oTransHRow.TotalDiscAmount = Convert.ToDecimal(this.txtAmtDiscount.Text);
                oPOSTrans.oTransHRow.TotalPaid = Convert.ToDecimal(this.txtAmtTotal.Text) + TransFeeAmt;    //PRIMEPOS-3117 11-Jul-2022 JY Added
                oPOSTrans.oTransHRow.TenderedAmount = Convert.ToDecimal(this.txtAmtTendered.Value);
            }

            oPOSTrans.oTransHRow.stClosedID = 0;
            oPOSTrans.oTransHRow.isStationClosed = 0;
            oPOSTrans.oTransHRow.isEOD = 0;
            oPOSTrans.oTransHRow.EODID = 0;
            oPOSTrans.oTransHRow.DrawerNo = Configuration.DrawNo;
            oPOSTrans.oTransHRow.StationID = Configuration.StationID;
            oPOSTrans.oTransHRow.TransDate = DateTime.Now;

            //Following Code Added By Krishna on 2 June 2011
            oPOSTrans.oTransHRow.TransactionStartDate = DateTime.Parse(TransStartDateTime);
            //Till here Added by Krishna on 2 June 2011

            //Added By Shitaljit(QuicSolv) on 31 August 2011
            oPOSTrans.oTransHRow.InvoiceDiscount = Configuration.convertNullToDecimal(lblInvDiscount.Text.ToString());
            //Till Here Added By Shitaljit(QuicSolv) on 31 August 2011

            #region //  Added for Solutran - PRIMEPOS-2663 - NileshJ
            //if(this.txtS3TransID.Text=="")
            oPOSTrans.oTransHRow.S3TransID = Convert.ToInt64(this.txtS3TransID.Text); //PRIMEPOS-3265
            oPOSTrans.oTransHRow.S3PurAmount = Convert.ToDecimal(this.txtS3PurAmt.Text);
            oPOSTrans.oTransHRow.S3DiscountAmount = Convert.ToDecimal(this.txtS3DiscAmt.Text);
            oPOSTrans.oTransHRow.S3TaxAmount = Convert.ToDecimal(this.txtS3TaxAmt.Text);
            #endregion

            oPOSTrans.oTransHRow.IsCustomerDriven = Convert.ToBoolean(this.IsCustomerDriven);//2915
        }

        //completed by sandeep
        private void PrintReceipt(RxLabel oRxLabel)
        {
            logger.Trace("PrintReceipt() - " + clsPOSDBConstants.Log_Entering);
            if (Configuration.convertNullToDouble(oPOSTrans.oTransHData.Tables[0].Rows[0]["TotalPaid"]) == 0.00 && Configuration.CInfo.AllowPrintZeroTrans == false)
            {
                logger.Trace("PrintReceipt() - " + clsPOSDBConstants.Log_Exiting);
                return;
            }
            else if (Configuration.CInfo.PrintReceipt == 'Y')
            {
                int taxCount = 0;
                if (oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
                {
                    taxCount = oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count;
                }
                logger.Trace("PrintReceipt() - set as Y" + taxCount);
                oRxLabel._oTDTaxData = oPOSTrans.oTDTaxData;
                oRxLabel.Print();
            }
            else if (Configuration.CInfo.PrintReceipt == 'A')
            {
                int taxCount = 0;
                if (oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
                {
                    taxCount = oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count;
                }
                if (oPOSTrans.oPOSTransPaymentData.POSTransPayment.Rows.Count > 0 && oPOSTrans.oPOSTransPaymentData.POSTransPayment[0].TransTypeCode.ToString().Trim().ToUpper() == "H") //PRIMEPOS-3028 19-Jan-2021 JY Added - there is strange issue when we proceed with the housecharge payment, it will not bring up the print receipt message.
                {
                    this.ActiveControl = this.txtItemCode;
                    Application.DoEvents();
                }
                if (Resources.Message.Display("Do you want to print this transaction?", "Print Receipt", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    logger.Trace("PrintReceipt() - set as A" + taxCount);
                    oRxLabel._oTDTaxData = oPOSTrans.oTDTaxData;
                    oRxLabel.Print();
                }
            }
            logger.Trace("PrintReceipt() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private bool GetPSEItemsVerificationDetails()
        {
            ofrmPOSPayAuthNo = new frmPOSPayAuthNo();
            ofrmPOSPayAuthNo.isAgeLimit = false;
            oPOSTrans.isByBoth = false;
            oPOSTrans.isAgeLimit = false;
            bool isPSEDetail = false;
            bool reDisplayID = false;

            POSTransSignLogData oTempTransSignLogData = new POSTransSignLogData();

            try
            {
                oTempTransSignLogData = oPOSTrans.AddPSEItemIntoTransSignLog();
                ofrmPOSPayAuthNo.isAgeLimit = oPOSTrans.isAgeLimit;
                ofrmPOSPayAuthNo.CustomerId = oPOSTrans.oCustomerRow.CustomerId;
                ofrmPOSPayAuthNo.bCalledForNplex = true;    //PRIMEPOS-2821 03-Nov-2020 JY Added
                if (oPOSTrans.isByBoth == true)
                {
                    ofrmPOSPayAuthNo.ShowDialog();
                    //bool checkAgeLimit = oPOSTrans.CheckAgeLimit(18, ofrmPOSPayAuthNo.Dob, ofrmPOSPayAuthNo.cmbVerificationID.SelectedIndex, ofrmPOSPayAuthNo.isCancelled, ref reDisplayID);  //PRIMEPOS-2525 24-May-2018 JY commented as need to execute it only if oPOSTrans.isAgeLimit == true
                    if (oPOSTrans.isAgeLimit && oPOSTrans.CheckAgeLimit(18, ofrmPOSPayAuthNo.Dob, ofrmPOSPayAuthNo.cmbVerificationID.SelectedIndex, ofrmPOSPayAuthNo.isCancelled, ref reDisplayID) && !reDisplayID) //please confirm hte default age limit
                    {
                        clsUIHelper.ShowErrorMsg("Customer is below the Age Limit of " + 18);
                        //reDisplayID = true; //PRIMEPOS-2572 15-Jun-2020 JY Added //no need to stop the user we just need to inform him as eventually we are sending data to Nplex
                    }
                    if (reDisplayID)
                    {
                        oTempTransSignLogData.POSTransSignLog.Reset();
                        bool bStatus = GetPSEItemsVerificationDetails();
                        if (bStatus == false)
                            return false;   //PRIMEPOS-2693 13-Jun-2019 JY Added

                        isPSEDetail = true;
                    }
                    if (ofrmPOSPayAuthNo.isCancelled)
                    {
                        return false; //PRIMEPOS-2693 13-Jun-2019 JY added return value as false
                    }
                    //if (!isPSEDetail && CaptureOTCItemsSignature(ref OTCSignDataBinary, ref oSignatureType) == false)
                    #region PRIMEPOS-2935 21-Jan-2021 JY Added
                    if (!isPSEDetail)
                    {
                        if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen || Configuration.CPOSSet.UseSigPad == true) //PRIMEPOS-3231N
                        {
                            if (CaptureOTCItemsSignature(ref OTCSignDataBinary, ref oSignatureType, isPSEItem) == false)//PRIMEPOS-3109
                            {
                                SigPadUtil.DefaultInstance.ShowItemsScreen();
                                return false;
                            }
                        }
                        else
                        {
                            clsUIHelper.ShowWarningMsg("NPLEX requires a signature to be captured and cannot be used without enabling signature capture.", "PrimePOS");
                            return false;
                        }
                    }
                    #endregion
                }
                foreach (POSTransSignLogRow oRow in oTempTransSignLogData.POSTransSignLog.Rows)
                {
                    if (clsVerificationBy.ByBoth == oRow.CustomerIDType.Trim())
                    {
                        if (Configuration.convertNullToString(ofrmPOSPayAuthNo.txtAuthorizationNo.Text).Length > 41)
                            ofrmPOSPayAuthNo.txtAuthorizationNo.Text = ofrmPOSPayAuthNo.txtAuthorizationNo.Text.Substring(0, 41); //PRIMEPOS-2959 28-Apr-2021 JY Added
                        oRow.CustomerIDDetail = Configuration.convertNullToString(ofrmPOSPayAuthNo.txtAuthorizationNo.Text) + "^" + Configuration.convertNullToString(ofrmPOSPayAuthNo.Dob);
                        oRow.SignDataText = oPOSTrans.OTCSignDataText;
                        oRow.SignDataBinary = OTCSignDataBinary;
                    }

                    if (ofrmPOSPayAuthNo.isCancelled == false && (clsVerificationBy.ByBoth == oRow.CustomerIDType.Trim() || clsVerificationBy.ByID == oRow.CustomerIDType.Trim()))
                    {
                        oRow.CustomerIDType = Configuration.convertNullToString(ofrmPOSPayAuthNo.cmbVerificationID.SelectedItem.DataValue);
                    }
                    else
                    {
                        oRow.CustomerIDType = "";
                    }
                    oPOSTrans.oTransSignLogData.POSTransSignLog.ImportRow(oRow);
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            return true;
        }
        //completed by sandeep

        private bool? CaptureNOPPSignature(RXHeader oRXHeader)
        {
            logger.Trace("CaptureNOPPSignature() - " + clsPOSDBConstants.Log_Entering);
            bool? retVal = true;
            string strSignature = "";
            bool? rval = null;
            bool NOPPsigdone = false;
            if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)//3013
            {
                return true;
            }
            if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected())
            {
                while (!NOPPsigdone)
                {


                    rval = SigPadUtil.DefaultInstance.CaptureNOPP(oRXHeader.PatientName, oRXHeader.PatientAddr);
                    if (rval == true)
                    {
                        //Modified By Rohit Nair on 08/07/2017 for PRIMEPOS-2442
                        if (SigPadUtil.DefaultInstance.isISC)
                        {
                            NOPPsigdone = true;
                            bCaptureSignature = true;
                        }
                        else if (SigPadUtil.DefaultInstance.isPAX)
                        {
                            NOPPsigdone = true;
                            bCaptureSignature = true;
                        }
                        else if (SigPadUtil.DefaultInstance.isEvertec) //PRIMEPOS-3209
                        {
                            NOPPsigdone = true;
                            bCaptureSignature = true;
                        }
                        //PRIMEPOS-2636
                        else if (SigPadUtil.DefaultInstance.isVantiv)
                        {
                            NOPPsigdone = true;
                            bCaptureSignature = true;
                        }
                        //
                        else if (SigPadUtil.DefaultInstance.isElavon)//2943
                        {
                            NOPPsigdone = true;
                            bCaptureSignature = true;
                        }
                        else
                        {
                            strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                            NOPPsigdone = true;

                            bool hasRPHVerifiedSignature;
                            bool isSignatureValid = VerifySignatureisValid(strSignature, SigPadUtil.DefaultInstance.SigType, out hasRPHVerifiedSignature);
                            if (!isSignatureValid)
                            {
                                strSignature = "";
                                NOPPsigdone = false;
                                //continue;
                                retVal = CaptureNOPPSignature(oRXHeader);
                            }
                            else if (Configuration.CPOSSet.DispSigOnTrans == true && !hasRPHVerifiedSignature)
                            {
                                if (Configuration.CPOSSet.UseSigPad == true)
                                {
                                    SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                                }
                                this.SigType = SigPadUtil.DefaultInstance.SigType;
                                //  Mantis Id : 0000119 Modified By Dharmendra (SRT) on Dec-02-08 passed extra parameter true
                                frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                                ofrm.SetMsgDetails("Validating Privacy Ack Signature...");
                                ofrm.ShowDialog();
                                if (ofrm.IsSignatureRejected == true)
                                {
                                    ofrm = null;
                                    SigPadUtil.DefaultInstance.CustomerSignature = null;//Added By & Modified by Dharmendra on Dec-15-08 to prevent from illegal flow continuation
                                    strSignature = "";
                                    NOPPsigdone = false;
                                    continue;
                                }
                                else
                                {
                                    retVal = true;
                                }
                                //Added & Modified By Dharmendra Till Here
                                //  Mantis Id : 0000119 Modified Till Here
                            }
                            else if (Configuration.CPOSSet.DispSigOnTrans == true && hasRPHVerifiedSignature)
                            {
                                if (Configuration.CPOSSet.UseSigPad == true)
                                {
                                    SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                                }
                                this.SigType = SigPadUtil.DefaultInstance.SigType;
                                //  Mantis Id : 0000119 Modified By Dharmendra (SRT) on Dec-02-08 passed extra parameter true

                                retVal = true;

                                //Added & Modified By Dharmendra Till Here
                                //  Mantis Id : 0000119 Modified Till Here
                            }


                            strSignature = SigPadUtil.DefaultInstance.CustomerSignature; //By Dharmendra on Apr-08-09
                        }


                    }
                    else if (rval == false)
                    {
                        retVal = false;
                        NOPPsigdone = true;
                    }
                    else if (rval == null)
                    {
                        retVal = null;
                        NOPPsigdone = true;
                    }
                }
            }

            if (rval == false)
            {
                oRXHeader.NOPPStatus = "N";
                oRXHeader.NOPPSignature = "";
                oRXHeader.PrivacyText = "";
            }
            else if (rval == null)
            {
                oRXHeader.NOPPStatus = null;
                oRXHeader.NOPPSignature = "";
                oRXHeader.PrivacyText = "";
            }
            else if (rval == true)
            {
                oRXHeader.NOPPStatus = "Y";
            }
            //HPSPAX Changes Suraj
            if (!SigPadUtil.DefaultInstance.isISC && !SigPadUtil.DefaultInstance.isPAX)
            {
                if (this.SigType == clsPOSDBConstants.STRINGIMAGE)
                    oRXHeader.NOPPSignature = strSignature;
                else
                {
                    oRXHeader.NOPPSignature = string.Empty;
                    Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    //Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    string errorMsg = string.Empty;
                    SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                    if (SigPadUtil.DefaultInstance.isISC)
                    {
                        byte[] iscsig = Convert.FromBase64String(strSignature);
                        sigDisp.DrawSignatureMX(iscsig, ref bmp, out errorMsg);
                    }
                    else
                    {
                        sigDisp.DrawSignature(strSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                    }
                    ImageConverter converter = new ImageConverter();
                    byte[] btarr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                    oRXHeader.NoppBinarySign = btarr;
                }
            }


            oRXHeader.PrivacyText = Configuration.CInfo.PrivacyText; //needs to get from device

            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "CaptureNOPPSignature()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CaptureNOPPSignature() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        //completed by sandeep
        private void RemoveCouponDiscount()
        {
            try
            {
                grdDetail.PerformAction(UltraGridAction.LastRowInGrid);
                grdDetail.ActiveRow = grdDetail.Rows[this.grdDetail.Rows.Count - 1];
                if (grdDetail.Rows.Count == 1)
                {
                    grdDetail.Rows[0].Selected = true;
                }
                string ItemID = Configuration.convertNullToString(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_ItemID].Value);
                if (ItemID.ToUpper().Equals(Configuration.CouponItemCode) == true)
                {
                    DelFromGridFlag = true;
                    tbEditItemActions("Del");
                    DelFromGridFlag = false;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "RemoveCouponDiscount()");
                throw Ex;
            }
        }

        //completed by sandeep
        public decimal GetDiscPercentage()
        {
            logger.Trace("GetDiscPercentage() - " + clsPOSDBConstants.Log_Entering);
            System.Decimal InvDisc = Configuration.convertNullToDecimal(this.lblInvDiscount.Text);
            System.Decimal discPerc = 0;
            //if (Configuration.CPOSSet.AllItemDisc == true)//ORIGINAL Commented by Shitalit(QuicSolv) 0n 7 sept 2011
            if (Configuration.CPOSSet.AllItemDisc == "1")// Added by Shitalit(QuicSolv) 0n 7 sept 2011
            {
                //if (Configuration.convertNullToDecimal(this.txtAmtSubTotal.Text) != 0)
                //{
                //    //discPerc = (InvDisc / Configuration.convertNullToDecimal(this.txtAmtSubTotal.Text) * 100);//ORIGINAL Commented by Shitalit(QuicSolv) 0n 7 sept 2011
                //    discPerc = (InvDisc / (Configuration.convertNullToDecimal(this.txtAmtSubTotal.Text) - (Configuration.convertNullToDecimal(this.txtAmtDiscount.Text) - InvDisc)) * 100);//Added By Shitaljit(QuicSolv) on 8 Sept 2011
                //}
                //Replacement code by shitaljit to handle the logic better
                System.Decimal totalPrice = 0;
                foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                {
                    totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
                }
                if (InvDisc != 0 && totalPrice != 0)//Added By Shitaljit(QuicSolv) on 30 August 2011
                {
                    //Logic For 100% Invoice Discount
                    //As we are adding Invoice discount to the existing item(s) discount it does not become exactly 100%
                    //by calculation even though 100% Invoice Discount was given.
                    if (IsHundredPerInvDisc == true && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    {
                        discPerc = Hundred;
                    }
                    else
                    {
                        discPerc = Convert.ToDecimal((Convert.ToDecimal(InvDisc) / totalPrice * 100));
                    }
                }
            }
            else
            {
                System.Decimal totalPrice = 0;
                foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                {
                    if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                    {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                    }
                }
                if (totalPrice != 0)
                {
                    //Logic For 100% Invoice Discount
                    //As we are adding Invoice discount to the existing item(s) sdiscount it does not become exactly 100%
                    //by calculation even though 100% Invoice Discount was given.
                    if (IsHundredPerInvDisc == true && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    {
                        discPerc = Hundred;
                    }
                    else
                        discPerc = Convert.ToDecimal((Convert.ToDecimal(InvDisc) / totalPrice * 100));
                }
            }
            logger.Trace("GetDiscPercentage() - " + clsPOSDBConstants.Log_Exiting);
            return discPerc;
        }

        //completed by sandeep
        public bool AddBackTaxToEBTItems()
        {
            logger.Trace("AddBackTaxToEBTItems() - " + clsPOSDBConstants.Log_Entering);
            bool RetVal = false;
            decimal TotalEBTItemsTax = 0;
            ItemData oItemData = new ItemData();
            ItemRow oItemRow = null;
            try
            {
                foreach (UltraGridRow oGRow in this.grdDetail.Rows)
                {
                    if (Configuration.convertNullToBoolean(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_IsEBT].Value) == true)
                    {
                        this.grdDetail.ActiveRow = oGRow;
                        string sItemID = Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
                        if (string.IsNullOrEmpty(sItemID) == false)
                        {
                            oItemData = oPOSTrans.PopulateItem(sItemID);
                            if (Configuration.isNullOrEmptyDataSet(oItemData) == false)
                            {
                                oItemRow = oItemData.Item[0];
                                if (oItemRow.TaxID > 0)
                                {
                                    oPOSTrans.GetTransDetailRow(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGRow.ListIndex);
                                    bool isDeptTaxable = false;
                                    DepartmentData oDeptData = null;
                                    if (oItemRow.DepartmentID != 0)
                                    {
                                        bool bTaxableDept = false;
                                        oDeptData = new DepartmentData();
                                        oDeptData = oPOSTrans.PopulateDepartment(oItemRow.DepartmentID.ToString());
                                        if (oDeptData == null || oDeptData.Department.Rows.Count == 0)
                                        {
                                            if (Configuration.CInfo.DefaultDeptId > 0)
                                            {
                                                oItemRow.DepartmentID = Configuration.CInfo.DefaultDeptId;
                                                oDeptData = oPOSTrans.PopulateDepartment(oItemRow.DepartmentID.ToString());
                                            }
                                        }
                                        if (oDeptData != null && oDeptData.Department.Rows.Count > 0)
                                        {
                                            bTaxableDept = oDeptData.Department[0].IsTaxable;
                                        }
                                        isDeptTaxable = bTaxableDept;
                                    }

                                    oPOSTrans.ApplyTaxPolicy(oItemRow, oPOSTrans.oTDRow.TransDetailID);   //Sprint-26 - PRIMEPOS-XXXX 01-Sep-2017 JY Added TransDetailId to bind TransDetailTax record with TransDetail record //PRIMEPOS-2500 JY Changed
                                }
                            }
                        }
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
                this.txtItemCode.Enabled = true;
                this.EnabledDisableItemRow(true);
            }
            catch (Exception Ex)
            {
                RetVal = false;
                logger.Trace(Ex, "AddBackTaxToEBTItems()");
                throw Ex;
            }
            logger.Trace("AddBackTaxToEBTItems() - " + clsPOSDBConstants.Log_Exiting);
            return RetVal;
        }
        //completed by sandeep
        private void GetOTCItemsVerificationDetails(bool bPSEItemForManual = false) //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual parameter
        {
            #region PRIMEPOS-2737 - NileshJ - Skip OTC signature - 30-Sept-2019
            if (bPSEItemForManual == false && Configuration.CPOSSet.SkipRxSignature) //PRIMEPOS-2920 02-Nov-2020 JY Added bPSEItemForManual
            {
                return;
            }
            #endregion
            ofrmPOSPayAuthNo = new frmPOSPayAuthNo();
            oPOSTrans.isByBoth = false;
            oPOSTrans.isBySignPresent = false;
            oPOSTrans.isByDLPresent = false;
            oPOSTrans.isAgeLimit = false; //added by Manoj 7/15/2013
            bool isOtcDetail = false;
            ofrmPOSPayAuthNo.isAgeLimit = false;
            bool reDisplayID = false; //Added by manoj 9/19/2013
            try
            {
                POSTransSignLogData oTempTransSignLogData = oPOSTrans.GetPOSSignTransLog();
                if (oPOSTrans.showOverrideBtn)//PRIMEPOS-3166
                {
                    ofrmPOSPayAuthNo.showOverrideBtn = true;
                }
                if (oPOSTrans.isAgeLimit)
                {
                    ofrmPOSPayAuthNo.isAgeLimit = true;
                }
                ofrmPOSPayAuthNo.CustomerId = this.oPOSTrans.oCustomerRow.CustomerId;//Added By Shitaljit on 10/10/2013
                if (oPOSTrans.isByDLPresent == true && oPOSTrans.isBySignPresent == false && oPOSTrans.isByBoth == false)
                {
                    ofrmPOSPayAuthNo.ShowDialog();
                    //added by Manoj 7/16/2013
                    //bool checkAgeLimit = oPOSTrans.CheckAgeLimit(oPOSTrans.oIMCategoryRow.AgeLimit, ofrmPOSPayAuthNo.Dob, ofrmPOSPayAuthNo.cmbVerificationID.SelectedIndex, ofrmPOSPayAuthNo.isCancelled, ref reDisplayID); //PRIMEPOS-2525 24-May-2018 JY commented as need to execute it only if oPOSTrans.isAgeLimit == true
                    if (!ofrmPOSPayAuthNo.isOverrideMonitorItem)
                    {
                        if (oPOSTrans.isAgeLimit && oPOSTrans.CheckAgeLimit(oPOSTrans.oIMCategoryRow.AgeLimit, ofrmPOSPayAuthNo.Dob, ofrmPOSPayAuthNo.cmbVerificationID.SelectedIndex, ofrmPOSPayAuthNo.isCancelled, ref reDisplayID) && !reDisplayID)
                        {
                            clsUIHelper.ShowErrorMsg("Customer is below the Age Limit of " + oPOSTrans.oIMCategoryRow.AgeLimit);
                            //reDisplayID = true; //PRIMEPOS-2572 15-Jun-2020 JY Added
                        }
                        if (reDisplayID) //added by Manoj 9/19/2013
                        {
                            oTempTransSignLogData.POSTransSignLog.Reset(); //Added  By Manoj 9/20/2013
                            GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2959 28-Apr-2021 JY Added bPSEItemForManual
                            isOtcDetail = true;
                        }
                    }
                    else if (ofrmPOSPayAuthNo.isOverrideMonitorItem) //PRIMEPOS-3166N
                    {
                        if (!string.IsNullOrWhiteSpace(ofrmPOSPayAuthNo.monitorItemOverriddenBy)) //PRIMEPOS-3166N
                        {
                            oPOSTrans.OverrideTempSaving(ofrmPOSPayAuthNo.monitorItemOverriddenBy);
                        }
                    }
                    if (ofrmPOSPayAuthNo.isCancelled)
                    {
                        return;  // added by Manoj 5/16/2012
                    }
                }
                else if (oPOSTrans.isBySignPresent == true && oPOSTrans.isByDLPresent == false && oPOSTrans.isByBoth == false)
                {
                    if (!isOtcDetail && CaptureOTCItemsSignature(ref OTCSignDataBinary, ref oSignatureType, isPSEItem) == false)//PRIMEPOS-3109
                    {
                        SigPadUtil.DefaultInstance.ShowItemsScreen(); // added by Manoj 5/14/2012
                        return;// added by Manoj 5/14/2012
                    }
                }
                else if (oPOSTrans.isByBoth == true || (oPOSTrans.isBySignPresent == true && oPOSTrans.isByDLPresent == true))
                {
                    ofrmPOSPayAuthNo.ShowDialog();
                    //added by Manoj 7/16/2013
                    if (!ofrmPOSPayAuthNo.isOverrideMonitorItem)
                    {
                        if (oPOSTrans.isAgeLimit)
                        {
                            bool checkAgeLimit = oPOSTrans.CheckAgeLimit(oPOSTrans.oIMCategoryRow.AgeLimit, ofrmPOSPayAuthNo.Dob, ofrmPOSPayAuthNo.cmbVerificationID.SelectedIndex, ofrmPOSPayAuthNo.isCancelled, ref reDisplayID);
                            if (checkAgeLimit && !reDisplayID)
                            {
                                clsUIHelper.ShowErrorMsg("Customer is below the Age Limit of " + oPOSTrans.oIMCategoryRow.AgeLimit);
                                //reDisplayID = true; //PRIMEPOS-2572 15-Jun-2020 JY Added
                            }
                        }
                        if (reDisplayID)//Added by Manoj
                        {
                            oTempTransSignLogData.POSTransSignLog.Reset(); //Added  By Manoj 9/20/2013
                            GetOTCItemsVerificationDetails(bPSEItemForManual);  //PRIMEPOS-2959 28-Apr-2021 JY Added bPSEItemForManual
                            isOtcDetail = true;
                        }
                        if (ofrmPOSPayAuthNo.isCancelled)
                        {
                            return;  // added by Manoj 5/16/2012
                        }
                        if (!isOtcDetail && CaptureOTCItemsSignature(ref OTCSignDataBinary, ref oSignatureType, isPSEItem) == false)//PRIMEPOS-3109
                        {
                            SigPadUtil.DefaultInstance.ShowItemsScreen();// added by Manoj 5/14/2012
                            return; // added by Manoj 5/14/2012
                        }
                    }
                    else if (ofrmPOSPayAuthNo.isOverrideMonitorItem)//PRIMEPOS-3166
                    {
                        if (!string.IsNullOrWhiteSpace(ofrmPOSPayAuthNo.monitorItemOverriddenBy)) //PRIMEPOS-3166N
                        {
                            oPOSTrans.OverrideTempSaving(ofrmPOSPayAuthNo.monitorItemOverriddenBy);
                        }
                    }
                    if (ofrmPOSPayAuthNo.isCancelled)
                    {
                        return;  // added by Manoj 5/16/2012
                    }
                }

                #region PRIMEPOS-2525 24-May-2018 JY added code to avoid exception due to "ofrmPOSPayAuthNo.cmbVerificationID.SelectedItem.DataValue" 
                string verificationId = string.Empty;
                try
                {
                    verificationId = Configuration.convertNullToString(ofrmPOSPayAuthNo.cmbVerificationID.SelectedItem.DataValue);
                }
                catch { }
                #endregion

                oPOSTrans.AddPOSSignTransLog(oTempTransSignLogData, OTCSignDataBinary, Configuration.convertNullToString(ofrmPOSPayAuthNo.txtAuthorizationNo.Text), Configuration.convertNullToString(ofrmPOSPayAuthNo.Dob), verificationId, ofrmPOSPayAuthNo.isCancelled);
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        //completed by sandeep

        private bool CapturePatientSignature(out byte[] sigdata)
        {

            logger.Trace("CapturePatientSignature() - " + clsPOSDBConstants.Log_Entering);
            sigdata = null;
            string strSignature = "";
            bool retVal = true;
            bool bPatSigDone = false;
            if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected())
            {
                while (!bPatSigDone)
                {
                    strSignature = "";
                    if (SigPadUtil.DefaultInstance.CapturePatientSignature())
                    {
                        strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                        bPatSigDone = true;

                        bool hasRPHVerifiedSignature;
                        bool isSignatureValid = VerifySignatureisValid(strSignature, SigPadUtil.DefaultInstance.SigType, out hasRPHVerifiedSignature);

                        if (!isSignatureValid)
                        {
                            strSignature = string.Empty;
                            retVal = CapturePatientSignature(out sigdata);
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && !hasRPHVerifiedSignature)
                        {
                            #region RPH NOt VErified SIgnature
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                            }
                            this.SigType = SigPadUtil.DefaultInstance.SigType;
                            //  Mantis Id : 0000119 Modified By Dharmendra (SRT) on Dec-02-08 passed extra parameter true
                            frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                            ofrm.SetMsgDetails("Validating RX Signature...");
                            ofrm.ShowDialog();
                            if ((Configuration.CPOSSet.PaymentProcessor == "EVERTEC" || Configuration.CPOSSet.PaymentProcessor == "ELAVON") && (!string.IsNullOrWhiteSpace(SigPadUtil.DefaultInstance.CustomerSignature)))//2943
                            {
                                strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                            }
                            if (ofrm.IsSignatureRejected == true)
                            {
                                ofrm = null;
                                SigPadUtil.DefaultInstance.CustomerSignature = null;//Added By Dharmendra on Dec-15-08 to prevent from illegal flow continuetion
                                strSignature = "";
                                retVal = CapturePatientSignature(out sigdata);
                            }
                            //Modified By Dharmendra (SRT) on Dec-15-08 to restrict the pos screen navigation to paymentlist
                            else
                            {
                                oPOSTrans.GetBytesForPatientSignature(strSignature, out sigdata);
                                retVal = true;
                            }
                            #endregion
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && hasRPHVerifiedSignature)
                        {
                            #region RPH Verified Signature
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                            }
                            this.SigType = SigPadUtil.DefaultInstance.SigType;
                            oPOSTrans.GetBytesForPatientSignature(strSignature, out sigdata);
                            retVal = true;

                            #endregion
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == false && strSignature.Trim().Length > 0)
                        {
                            if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" || Configuration.CPOSSet.PaymentProcessor == "ELAVON")//2664//2943
                            {
                                frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                                ofrm.SetMsgDetails("Validating RX Signature...");
                                if (!string.IsNullOrWhiteSpace(SigPadUtil.DefaultInstance.CustomerSignature))
                                {
                                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                                }
                            }
                            oPOSTrans.GetBytesForPatientSignature(strSignature, out sigdata);
                            retVal = true;
                        }

                    }
                    else
                    {
                        bPatSigDone = true;
                        retVal = false;
                    }
                }
            }

            logger.Trace("CapturePatientSignature() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;

        }

        //completed by sandeep
        private bool CapturePatientSignaturePAX(out byte[] sigPlots)
        {

            logger.Trace("CapturePatientSignature() - " + clsPOSDBConstants.Log_Entering);
            sigPlots = null;
            string strSignature = "";
            bool retVal = true;
            bool bPatSigDone = false;
            if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected())
            {
                while (!bPatSigDone)
                {
                    strSignature = "";
                    if (SigPadUtil.DefaultInstance.CapturePatientSignature())
                    {
                        strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                        bPatSigDone = true;

                        bool hasRPHVerifiedSignature;
                        bool isSignatureValid = VerifySignatureisValid(strSignature, SigPadUtil.DefaultInstance.SigType, out hasRPHVerifiedSignature);

                        if (!isSignatureValid)
                        {
                            strSignature = string.Empty;
                            retVal = CapturePatientSignaturePAX(out sigPlots);
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && !hasRPHVerifiedSignature)
                        {
                            #region RPH NOt VErified SIgnature
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                            }
                            this.SigType = clsPOSDBConstants.BINARYIMAGE;
                            //  Mantis Id : 0000119 Modified By Dharmendra (SRT) on Dec-02-08 passed extra parameter true
                            frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                            ofrm.SetMsgDetails("Validating RX Signature...");
                            ofrm.ShowDialog();
                            if (ofrm.IsSignatureRejected == true)
                            {
                                ofrm = null;
                                SigPadUtil.DefaultInstance.CustomerSignature = null;//Added By Dharmendra on Dec-15-08 to prevent from illegal flow continuetion
                                strSignature = "";
                                retVal = CapturePatientSignaturePAX(out sigPlots);
                            }
                            //Modified By Dharmendra (SRT) on Dec-15-08 to restrict the pos screen navigation to paymentlist
                            else
                            {
                                // oPOSTrans.GetBytesForPatientSignature(strSignature, out sigPlots); btes of points remaining
                                sigPlots = ofrm.BSigData;
                                retVal = true;
                            }
                            #endregion
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == true && strSignature.Trim().Length > 0 && hasRPHVerifiedSignature)
                        {
                            #region RPH Verified Signature
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                            }
                            this.SigType = SigPadUtil.DefaultInstance.SigType;
                            //oPOSTrans.GetBytesForPatientSignature(strSignature, out sigdata);
                            retVal = true;

                            #endregion
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == false && strSignature.Trim().Length > 0)
                        {
                            //oPOSTrans.GetBytesForPatientSignature(strSignature, out sigdata);
                            //PRIMEPOS-2607 - NileshJ - Changes for Saving Rx Signature
                            string oError = string.Empty;
                            byte[] SigData = null;
                            this.SigType = clsPOSDBConstants.BINARYIMAGE;
                            //clsUIHelper.GetSignature(strSignature, out oError, this.SigType, out SigData); //Commented For Aries8
                            Bitmap sigBitmap = clsUIHelper.GetSignaturePAX(strSignature, out oError, this.SigType, out SigData); //PRIMEPOS-2952
                            //Bitmap result = new Bitmap(sigBitmap.Width, sigBitmap.Height);
                            //if (Configuration.CPOSSet.PinPadModel == "HPSPAX_ARIES8")
                            //{
                            //    using (Graphics g = Graphics.FromImage(result))
                            //    {
                            //        g.DrawImage(sigBitmap, 0, 0, 999, 1000);
                            //    }
                            //}
                            //else
                            //{
                            //    using (Graphics g = Graphics.FromImage(result))
                            //    {
                            //        g.DrawImage(sigBitmap, 0, 0, 335, 450);
                            //    }
                            //}
                            //System.IO.MemoryStream ms = new System.IO.MemoryStream(); //Added for Signature issue Aries8                            
                            //result.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            //Bitmap bitmap = new Bitmap(ms);
                            //SigPadUtil.DefaultInstance.CustomerSignature = Encoding.Default.GetString(ms.ToArray());
                            //sigPlots = Encoding.Default.GetBytes(SigPadUtil.DefaultInstance.CustomerSignature);
                            sigPlots = SigData;//2998
                            retVal = true;
                        }

                    }
                    else
                    {
                        bPatSigDone = true;
                        retVal = false;
                    }
                }
            }

            logger.Trace("CapturePatientSignature() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;

        }

        //completed by sandeep
        private void RecalculateTax()
        {
            logger.Trace("RecalculateTax() - " + clsPOSDBConstants.Log_Entering);
            System.Decimal InvDisc = Configuration.convertNullToDecimal(this.lblInvDiscount.Text);
            System.Decimal discPerc = 0;
            string sTaxIDs = string.Empty;
            //if (Configuration.CPOSSet.AllItemDisc == "1")//Added By shitaljit(QuicSolv) on 7 Sept 2011
            //{
            //    //Added By Shitaljit for better logic to calculate correct percent invoice discount on 13 Jan 2013.
            //    System.Decimal totalPrice = 0;
            //    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            //    {
            //        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
            //    }
            //    if (InvDisc != 0 && totalPrice != 0)//Added By Shitaljit(QuicSolv) on 30 August 2011
            //    {
            //        //Logic For 100% Invoice Discount
            //        //As we are adding Invoice discount to the existing item(s) discount it does not become exactly 100%
            //        //by calculation even though 100% Invoice Discount was given.
            //        if (IsHundredPerInvDisc == true && Configuration.CInfo.AllowHundredPerInvDiscount == true)
            //        {
            //            discPerc = Hundred;
            //        }
            //        else
            //        {
            //            discPerc = Convert.ToDecimal((Convert.ToDecimal(InvDisc) / totalPrice * 100));
            //        }
            //    }
            //}
            //else
            //{
            //    System.Decimal totalPrice = 0;
            //    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            //    {
            //        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
            //        {
            //            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
            //        }
            //    }
            //    //if (InvDisc > 0)//Commented By Shitaljit(QuicSolv) on 30 August 2011
            //    if (InvDisc != 0 && totalPrice != 0)//Added By Shitaljit(QuicSolv) on 30 August 2011
            //    {
            //        //Logic For 100% Invoice Discount
            //        //As we are adding Invoice discount to the existing item(s) sdiscount it does not become exactly 100%
            //        //by calculation even though 100% Invoice Discount was given.
            //        if (IsHundredPerInvDisc == true && Configuration.CInfo.AllowHundredPerInvDiscount == true)
            //        {
            //            discPerc = Hundred;
            //        }
            //        else
            //        {
            //            discPerc = Convert.ToDecimal((Convert.ToDecimal(InvDisc) / totalPrice * 100));
            //        }
            //    }
            //}
            //if (Configuration.CPOSSet.AllItemDisc == "0")//Added By shitaljit(QuicSolv) on 7 Sept 2011
            //{
            //    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            //    {
            //        oPOSTrans.GetTransDetailRow(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGRow.ListIndex);
            //        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
            //        {
            //            string sItemID = Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
            //            if (!oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, sItemID, out sTaxIDs, oPOSTrans.oTDRow.TransDetailID))
            //            {
            //                continue;
            //            }
            //            Decimal TempExtPrice = oPOSTrans.oTDRow.ExtendedPrice;
            //            TaxCodes oTC = new TaxCodes();
            //            TaxCodesData oTCD = oTC.PopulateList(" WHERE TaxID IN " + sTaxIDs);
            //            System.Decimal LIDisc = 0;
            //            LIDisc = (discPerc / 100 * (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value.ToString())));
            //            //oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value.ToString()) - LIDisc);
            //            //Above line commented by Manoj. Causing tax to calculate wrong

            //            oPOSTrans.oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - LIDisc);    //Sprint-21 - PRIMEPOS-2225 17-Feb-2016 JY Added logic to calculate tax correctly after applying invoice discount
            //            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value = 0; //Sprint-21 - PRIMEPOS-2225 17-Feb-2016 JY If "Remove individual item disc and apply the invoice disc to the transaction net amount" application setting selected and we apply invoice discount then it will not recalculate tax properly. Fixed this issue. 
            //            oPOSTrans.CalculateTax(oPOSTrans.oTDRow, oTCD, oPOSTrans.oTDTaxData);
            //            oPOSTrans.oTDRow.ExtendedPrice = TempExtPrice;
            //        }
            //        #region Sprint-21 - PRIMEPOS-2226 18-Feb-2016 JY to calculate the tax for discountable item which should be required to display corectly when we press the "BACK" button on payment screen
            //        else
            //        {
            //            string sItemID = Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
            //            if (!oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, sItemID, out sTaxIDs, oPOSTrans.oTDRow.TransDetailID))
            //            {
            //                continue;
            //            }
            //            Decimal TempExtPrice = oPOSTrans.oTDRow.ExtendedPrice;
            //            TaxCodes oTC = new TaxCodes();
            //            TaxCodesData oTCD = oTC.PopulateList(" WHERE TaxID IN " + sTaxIDs);
            //            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value = 0; //Sprint-21 - PRIMEPOS-2225 17-Feb-2016 JY If "Remove individual item disc and apply the invoice disc to the transaction net amount" application setting selected and we apply invoice discount then it will not recalculate tax properly. Fixed this issue. 
            //            oPOSTrans.CalculateTax(oPOSTrans.oTDRow, oTCD, oPOSTrans.oTDTaxData);
            //        }
            //        #endregion
            //    }
            //}
            //else
            //{
            //    foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            //    {
            //        oPOSTrans.GetTransDetailRow(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGRow.ListIndex);
            //        string sItemID = Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
            //        if (!oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, sItemID, out sTaxIDs, oPOSTrans.oTDRow.TransDetailID))
            //        {
            //            continue;
            //        }
            //        Decimal TempExtPrice = oPOSTrans.oTDRow.ExtendedPrice;
            //        TaxCodes oTC = new TaxCodes();
            //        TaxCodesData oTCD = oTC.PopulateList(" WHERE TaxID IN " + sTaxIDs);
            //        System.Decimal LIDisc = 0;
            //        LIDisc = (discPerc / 100 * (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value.ToString())));
            //        //oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value.ToString()) - LIDisc);
            //        //Above line commented by Manoj. Causing tax to calculate wrong
            //        oPOSTrans.oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - LIDisc);    //Sprint-21 - PRIMEPOS-2225 11-Feb-2016 JY Added logic to calculate tax correctly after applying invoice discount
            //        oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value = 0; //Sprint-21 - PRIMEPOS-2225 17-Feb-2016 JY If "Remove individual item disc and apply the invoice disc to the transaction net amount" application setting selected and we apply invoice discount then it will not recalculate tax properly. Fixed this issue. 
            //        oPOSTrans.CalculateTax(oPOSTrans.oTDRow, oTCD, oPOSTrans.oTDTaxData);
            //        oPOSTrans.oTDRow.ExtendedPrice = TempExtPrice;
            //    }
            //    Application.DoEvents();
            //}

            #region PRIMEPOS-3143 16-Sep-2022 JY Added
            ArrayList alItemEligibleForTax = new ArrayList();
            System.Decimal totalPrice = 0;
            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                if (Configuration.CPOSSet.AllItemDisc == "1")
                {
                    bool AllowDisOfItemsOnSale = false;
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                    {
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                        {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
                            alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                        }
                    }
                    else if (AllowDisOfItemsOnSale == true)
                    {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
                        alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                    }
                }
                else if (Configuration.CPOSSet.AllItemDisc == "0")
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
                                alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                            }
                        }
                    }
                    else if (AllowDisOfItemsOnSale == true)
                    {
                        if (Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value) == 0)
                        {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                            alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                        }
                    }
                }
                else if (Configuration.CPOSSet.AllItemDisc == "2")
                {
                    bool AllowDisOfItemsOnSale = false;
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                    {
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                        {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                            alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                        }
                    }
                    else if (AllowDisOfItemsOnSale == true)
                    {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                        alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                    }
                }
                else if (Configuration.CPOSSet.AllItemDisc == "3")
                {
                    bool AllowDisOfItemsOnSale = false;
                    AllowDisOfItemsOnSale = oPOSTrans.AllowDiscountOfItemsOnSale(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString());
                    if (Configuration.CInfo.InvDiscToDiscountableItemOnly == true)
                    {
                        if (oPOSTrans.AllowDiscount(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Text.ToString()) == true && AllowDisOfItemsOnSale == true)
                        {
                            totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                            alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                        }
                    }
                    else if (AllowDisOfItemsOnSale == true)
                    {
                        totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value);
                        alItemEligibleForTax.Add(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value));
                    }
                }
            }
            if (InvDisc != 0 && totalPrice != 0)
            {
                //Logic For 100% Invoice Discount
                //As we are adding Invoice discount to the existing item(s) discount it does not become exactly 100%
                //by calculation even though 100% Invoice Discount was given.
                if (IsHundredPerInvDisc == true && Configuration.CInfo.AllowHundredPerInvDiscount == true)
                    discPerc = Hundred;
                else
                    discPerc = Convert.ToDecimal((Convert.ToDecimal(InvDisc) / totalPrice * 100));
            }

            if (alItemEligibleForTax.Count == 0)
                return;

            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                oPOSTrans.GetTransDetailRow(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGRow.ListIndex);
                string sItemID = Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
                if (!oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, sItemID, out sTaxIDs, oPOSTrans.oTDRow.TransDetailID))
                {
                    continue;
                }

                if (alItemEligibleForTax.Contains(Configuration.convertNullToInt(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value)))
                {
                    Decimal TempExtPrice = oPOSTrans.oTDRow.ExtendedPrice;
                    using (TaxCodes oTC = new TaxCodes())
                    {
                        using (TaxCodesData oTCD = oTC.PopulateList(" WHERE TaxID IN " + sTaxIDs))
                        {
                            System.Decimal LIDisc = 0;
                            LIDisc = (discPerc / 100 * (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value.ToString())));
                            oPOSTrans.oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - LIDisc);
                            oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value = 0;
                            oPOSTrans.CalculateTax(oPOSTrans.oTDRow, oTCD, oPOSTrans.oTDTaxData);
                            oPOSTrans.oTDRow.ExtendedPrice = TempExtPrice;
                        }
                    }
                }
            }
            Application.DoEvents();
            #endregion
            logger.Trace("RecalculateTax() - " + clsPOSDBConstants.Log_Exiting);
        }

        #endregion

        #region NPLEX

        //completed by sandeep
        private Boolean DoNplexProcessPSE()
        {
            logger.Trace("DoNplexProcessPSE() - " + clsPOSDBConstants.Log_Entering);
            Boolean isValid = true;
            string errMsg = string.Empty;
            try
            {
                string verificationId = Configuration.convertNullToString(ofrmPOSPayAuthNo.cmbVerificationID.SelectedItem.DataValue);
                string authorizationNo = ofrmPOSPayAuthNo.txtAuthorizationNo.Text.ToString();
                string Dob = ofrmPOSPayAuthNo.Dob;  //PRIMEPOS-2729 06-Sep-2019 JY Added
                //Boolean result = oPOSTrans.NplexProcessPSE(ofrmPOSPayAuthNo.ID, oSignatureType, verificationId, authorizationNo, out pseTrxId, out inquiryId, ref errMsg);
                //Boolean result = oPOSTrans.NplexProcessPSE(ofrmPOSPayAuthNo.ID, oSignatureType, verificationId, authorizationNo, Dob, out pseTrxId, out inquiryId, ref errMsg); //PRIMEPOS-2729 06-Sep-2019 JY Added Dob parameter
                Boolean result = oPOSTrans.NplexProcessPSE(ofrmPOSPayAuthNo.oCustAddress, oSignatureType, verificationId, authorizationNo, Dob, out pseTrxId, out inquiryId, ref errMsg); //PRIMEPOS-2729 06-Sep-2019 JY Added Dob parameter    //PRIMEPOS-2821 04-Nov-2020 JY Added ofrmPOSPayAuthNo.oCustAddress
                if (result == false)
                {
                    clsUIHelper.ShowErrorMsg(errMsg);
                    return false;
                }
                logger.Trace("DoNplexProcessPSE() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "DoNplexProcessPSE()");
                throw exp;
            }
            return isValid;
        }
        #endregion

        private bool HasMultipleSignature()
        {
            bool rtn = false;
            int iCount = 0;

            if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen) //only for Tablet//3002 //PRIMEPOS-3231N
            {
                foreach (RXHeader oRXHeader in oPOSTrans.oRXHeaderList)
                {
                    if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && (Configuration.CPOSSet.DisableNOPP == false && !oPOSTrans.SkipForDelivery()))
                    {
                        if (oRXHeader.isNOPPSignRequired == true)
                            iCount++;
                    }

                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && (!oPOSTrans.SkipForDelivery()))
                            iCount++;
                    }
                }
            }

            if (iCount >= 2)
                rtn = true;

            return rtn;
        }

        private bool RecordPatientCounseling()
        {
            bool rtn = true;
            DialogResult rtnDialog = DialogResult.Cancel;
            PharmBL oPharmBL = new PharmBL();

            if (oPharmBL.ConnectedTo_ePrimeRx())
                return rtn; // only implemented patient counseling audit in PrimeRx

            if (oPharmBL.GetSettingPatientCounselingAudited() != "Y")  // Switch in PrimeRx is off
                return rtn;

            foreach (RXHeader oRxH in oPOSTrans.oRXHeaderList)
            {
                //RXHeader oRxH = oPOSTrans.oRXHeaderList.FirstOrDefault();
                if (oRxH == null)
                    continue;

                string sPatientST = string.Empty;
                if (!string.IsNullOrWhiteSpace(Configuration.CInfo.State))
                    sPatientST = Configuration.CInfo.State.ToUpper().Trim();
                if (oRxH.TblPatient != null && oRxH.TblPatient.Rows.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(oRxH.TblPatient.Rows[0]["ADDRST"].ToString()))
                        sPatientST = oRxH.TblPatient.Rows[0]["ADDRST"].ToString().Trim();
                }
                DataTable dtRules = oPharmBL.GetPatCounselingRulesByState(sPatientST);
                if (dtRules == null || dtRules.Rows.Count == 0)  // there is no counseling rule defined (General or State rule)
                    continue;

                //1. check patient insurance to see they are matched or not
                bool bFoundIns = true;
                if (!string.IsNullOrWhiteSpace(dtRules.Rows[0]["Insurance"].ToString()))
                {
                    bFoundIns = false;
                    string[] ruleInsArray = dtRules.Rows[0]["Insurance"].ToString().Split(',');
                    string sPATTYPE = oRxH.TblPatient.Rows[0]["PATTYPE"].ToString();
                    for (int i = 0; i < ruleInsArray.Length; i++)
                    {
                        if (ruleInsArray[i].Trim().Equals(sPATTYPE.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            bFoundIns = true;
                            break;
                        }
                    }
                }
                if (!bFoundIns)
                    continue;

                DataTable dtRXs = new DataTable("Patient_RXs");
                dtRXs.Columns.Add("RxNo", typeof(long));
                dtRXs.Columns.Add("RefillNo", typeof(string));
                dtRXs.Columns.Add("Drug Name", typeof(string));
                for (int i = 0; i < oRxH.RXDetails.Count; i++)
                {
                    //2. check Rx Typpe
                    bool bDosageChanged = false;
                    if (!string.IsNullOrWhiteSpace(dtRules.Rows[0]["RxType"].ToString()))
                    {
                        if (oRxH.RXDetails[i].RefillNo == 0)  // new Rx
                        {
                            if (!dtRules.Rows[0]["RxType"].ToString().Contains("O"))
                                continue;
                        }
                        else // Refill
                        {
                            bool bRefillDosageChangedOnly = Configuration.convertNullToBoolean(dtRules.Rows[0]["RefillDosageChangedOnly"]);
                            if (!bRefillDosageChangedOnly)
                            {
                                if (!dtRules.Rows[0]["RxType"].ToString().Contains("R"))
                                    continue;
                            }
                            else
                            {
                                if (!oPharmBL.HasRXRefillDoseChanged(oRxH.RXDetails[i].RXNo, oRxH.RXDetails[i].RefillNo))
                                    continue;
                                else
                                    bDosageChanged = true;
                            }
                        }
                    }
                    if (!bDosageChanged)
                    {
                        //3. Check CounselingRenewalCycle
                        if (Configuration.convertNullToInt(dtRules.Rows[0]["CounselingRenewalCycle"]) != 0)
                        {
                            int cycleDays = Configuration.convertNullToInt(dtRules.Rows[0]["CounselingRenewalCycle"]);

                            DataTable tblLastCouns = oPharmBL.GetLastCounselledPatientDrugInfo(Configuration.convertNullToInt(oRxH.PatientNo), oRxH.RXDetails[i].TblClaims.Rows[0]["NDC"].ToString());
                            if (tblLastCouns != null && tblLastCouns.Rows.Count > 0)
                            {
                                DateTime LastCounsDate = Convert.ToDateTime(tblLastCouns.Rows[0]["CounsCompletedDate"].ToString());
                                int numDays = (DateTime.Today - LastCounsDate.Date).Days;
                                if (numDays < cycleDays)
                                    continue;
                            }
                        }
                    }
                    //4. Check drug class
                    if (!string.IsNullOrWhiteSpace(dtRules.Rows[0]["DrugClass"].ToString()))
                    {
                        string strClass = "0";
                        if (!string.IsNullOrWhiteSpace(oRxH.RXDetails[i].TblClaims.Rows[0]["class"].ToString()))
                            strClass = oRxH.RXDetails[i].TblClaims.Rows[0]["class"].ToString().Trim();
                        if (!dtRules.Rows[0]["DrugClass"].ToString().Contains(strClass))
                            continue;
                    }

                    DataRow dr = dtRXs.NewRow();
                    dr[0] = oRxH.RXDetails[i].RXNo;
                    dr[1] = oRxH.RXDetails[i].RefillNo.ToString();
                    dr[2] = oRxH.RXDetails[i].DrugName.Trim() + " " + oRxH.RXDetails[i].TblClaims.Rows[0]["STRONG"].ToString().Trim() + " " + oRxH.RXDetails[i].TblClaims.Rows[0]["FORM"].ToString().Trim();
                    dtRXs.Rows.Add(dr);
                }

                if (dtRXs.Rows.Count == 0)
                    continue;

                try
                {
                    FrmPatCounselingHistory oFrmHist = new FrmPatCounselingHistory();
                    oFrmHist.oRXHeader = oRxH;
                    oFrmHist.tblCounselRules = dtRules;
                    oFrmHist.tblCounselRXs = dtRXs;
                    rtnDialog = oFrmHist.ShowDialog(this);
                    oFrmHist.Dispose();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg("Patient Counseling History\n" + exp.Message);
                }

                if (rtnDialog == DialogResult.Cancel)
                    return false;
            }

            return rtn;
        }
        #region PRIMEPOS-3403
        public static PatientInt ConvertPatientDataToInterface(DataTable patient)
        {
            try
            {
                PatientInt interfacePatient = new PatientInt();
                interfacePatient.PatNo = patient.Rows[0]["PATIENTNO"].ToSafeString();
                interfacePatient.FirstName = patient.Rows[0]["FNAME"].ToSafeString();
                interfacePatient.LastName = patient.Rows[0]["LNAME"].ToSafeString();
                interfacePatient.MiddleInitial = patient.Rows[0]["MI"].ToSafeString();
                interfacePatient.DateOfBirth = patient.Rows[0]["DOB"].ToSafeString();
                interfacePatient.Sex = patient.Rows[0]["SEX"].ToSafeString();
                interfacePatient.City = patient.Rows[0]["ADDRCT"].ToSafeString();
                interfacePatient.State = patient.Rows[0]["ADDRST"].ToSafeString();
                interfacePatient.Zip = patient.Rows[0]["ADDRZP"].ToSafeString();
                interfacePatient.Address = patient.Rows[0]["ADDRSTR"].ToSafeString();
                interfacePatient.Address2 = patient.Rows[0]["ADDRSTR2"].ToSafeString();
                interfacePatient.WORKNO = patient.Rows[0]["WORKNO"].ToSafeString();
                interfacePatient.Phone = patient.Rows[0]["PHONE"].ToSafeString();
                interfacePatient.MOBILENO = patient.Rows[0]["MOBILENO"].ToSafeString();
                interfacePatient.Language = patient.Rows[0]["LANGUAGE"].ToSafeString();
                interfacePatient.EMAIL = patient.Rows[0]["EMAIL"].ToSafeString();
                interfacePatient.CUSTCATEG = patient.Rows[0]["CUSTCATEG"].ToSafeString() == "P" ? "Preffered" : patient.Rows[0]["CUSTCATEG"].ToSafeString() == "R" ? "Restricted" : "None";
                interfacePatient.DriversLicense = patient.Rows[0]["DRIVERSLICENSE"].ToSafeString();
                interfacePatient.DriversLicenseExpDT = patient.Rows[0]["DriversLicenseExpDT"].ToSafeString();
                return interfacePatient;
            }
            catch (Exception ex)
            {
                logger.Error("ConvertPatientDataToInterface() ==> Following error has occurred : {0}", ex.ToString());
                return null;
            }
        }

        public static DrugInt ConvertDrugDataToInterface(DataTable drug)
        {
            try
            {
                DrugInt interfaceDrug = new DrugInt();
                interfaceDrug.DRGNAME = drug.Rows[0]["DRGNAME"].ToSafeString();
                interfaceDrug.DRGNDC = drug.Rows[0]["DRGNDC"].ToSafeString();
                interfaceDrug.STRONG = drug.Rows[0]["STRONG"].ToSafeString();
                interfaceDrug.Strength = drug.Rows[0]["Strength"].ToSafeString();
                interfaceDrug.StrengthUM = drug.Rows[0]["StrengthUM"].ToSafeString();
                interfaceDrug.FORM = drug.Rows[0]["FORM"].ToSafeString();
                interfaceDrug.DRGMADE = drug.Rows[0]["DRGMADE"].ToSafeString();
                interfaceDrug.LOTNO = drug.Rows[0]["LOTNO"].ToSafeString();
                interfaceDrug.EXPDATE = drug.Rows[0]["EXPDATE"].ToSafeString();
                interfaceDrug.SpecificProductID = drug.Rows[0]["SPECIFICPRODUCTID"].ToSafeInt();
                interfaceDrug.CLASS = drug.Rows[0]["CLASS"].ToSafeString();
                return interfaceDrug;
            }
            catch (Exception ex)
            {
                logger.Error("ConvertDrugDataToInterface() ==> Following error has occurred : {0}", ex.ToString());
                return null;
            }
        }
        public static PrescriberInt ConvertPrescribDataToInterface(DataTable prescriber)
        {
            try
            {
                PrescriberInt interfacePrescriber = new PrescriberInt();
                interfacePrescriber.PRESNO = prescriber.Rows[0]["PRESNO"].ToSafeString();
                interfacePrescriber.PRESLNM = prescriber.Rows[0]["PRESLNM"].ToSafeString();
                interfacePrescriber.PRESFNM = prescriber.Rows[0]["PRESFNM"].ToSafeString();
                interfacePrescriber.PHONE = prescriber.Rows[0]["PHONE"].ToSafeString();
                interfacePrescriber.PHONE2 = prescriber.Rows[0]["PHONE2"].ToSafeString();
                interfacePrescriber.MOBILENO = prescriber.Rows[0]["MOBILENO"].ToSafeString();
                interfacePrescriber.FAXNO = prescriber.Rows[0]["FAXNO"].ToSafeString();
                interfacePrescriber.EMAIL = prescriber.Rows[0]["EMAIL"].ToSafeString();
                interfacePrescriber.PRESLIC = prescriber.Rows[0]["PRESLIC"].ToSafeString();
                interfacePrescriber.MCDIDNO = prescriber.Rows[0]["MCDIDNO"].ToSafeString();
                interfacePrescriber.PRESDEA = prescriber.Rows[0]["PRESDEA"].ToSafeString();
                interfacePrescriber.NPINO = prescriber.Rows[0]["NPINO"].ToSafeString();
                interfacePrescriber.ADDRSTR = prescriber.Rows[0]["ADDRSTR"].ToSafeString();
                interfacePrescriber.ADDRCT = prescriber.Rows[0]["ADDRCT"].ToSafeString();
                interfacePrescriber.ADDRST = prescriber.Rows[0]["ADDRST"].ToSafeString();
                interfacePrescriber.ADDRZP = prescriber.Rows[0]["ADDRZP"].ToSafeString();
                return interfacePrescriber;
            }
            catch (Exception ex)
            {
                logger.Error("ConvertPrescribDataToInterface() ==> Following error has occurred : {0}", ex.ToString());
                return null;
            }
        }
        public static ClaimsInt ConvertClaimsDataToInterface(DataTable claims)
        {
            try
            {
                ClaimsInt interfaceClaims = new ClaimsInt();
                interfaceClaims.RXNO = claims.Rows[0]["RXNO"].ToSafeInt();
                interfaceClaims.TREFILLS = claims.Rows[0]["TREFILLS"].ToSafeString();
                interfaceClaims.DAYS = claims.Rows[0]["DAYS"].ToSafeString();
                interfaceClaims.DATEF = claims.Rows[0]["DATEF"].ToSafeString();
                interfaceClaims.DATEO = claims.Rows[0]["DATEO"].ToSafeString();
                interfaceClaims.STATUS = claims.Rows[0]["STATUS"].ToSafeString();
                interfaceClaims.QTY_ORD = claims.Rows[0]["QTY_ORD"].ToSafeDecimal();
                interfaceClaims.QUANT = claims.Rows[0]["QUANT"].ToSafeDecimal();

                interfaceClaims.SIG = claims.Rows[0]["SIG"].ToSafeString();
                interfaceClaims.PICKEDUP = claims.Rows[0]["PICKEDUP"].ToSafeString();
                interfaceClaims.PICKUPDATE = claims.Rows[0]["PICKUPDATE"].ToSafeString();
                interfaceClaims.PICKUPTIME = claims.Rows[0]["PICKUPTIME"].ToSafeString();
                interfaceClaims.RXNOTES = claims.Rows[0]["RXNOTES"].ToSafeString();
                interfaceClaims.DELIVERY = claims.Rows[0]["DELIVERY"].ToSafeString();
                interfaceClaims.TOTAMT = claims.Rows[0]["TOTAMT"].ToSafeDecimal();
                interfaceClaims.COPAY = claims.Rows[0]["COPAY"].ToSafeDecimal();
                interfaceClaims.BILLTYPE = claims.Rows[0]["BILLTYPE"].ToSafeString();

                interfaceClaims.COPAYPAID = claims.Rows[0]["COPAYPAID"].ToSafeString();

                PharmBL oPBL = new PharmBL();
                DataTable dtRxExtra = oPBL.GetRxExtra(interfaceClaims.RXNO, claims.Rows[0]["NREFILL"].ToSafeInt());
                if (dtRxExtra != null && dtRxExtra.Rows.Count > 0)
                {
                    frmPOSTransaction frmPOS = new frmPOSTransaction();
                    interfaceClaims.RxOriginCode = dtRxExtra.Rows[0]["RX_ORIGCD"].ToSafeString();
                    interfaceClaims.RxOriginDescription = frmPOS.GetRxOriginDescription(dtRxExtra.Rows[0]["RX_ORIGCD"].ToSafeString());
                }

                interfaceClaims.RxDiagDataTable = oPBL.GetRxDiag(interfaceClaims.RXNO);

                DataTable dtClaimPaymentView = oPBL.GetClaimPaymentView(interfaceClaims.RXNO, claims.Rows[0]["NREFILL"].ToSafeInt());
                if (dtClaimPaymentView != null && dtClaimPaymentView.Rows.Count > 0)
                {
                    interfaceClaims.FollowupTag = dtClaimPaymentView.Rows[0]["TagName"].ToSafeString();
                    interfaceClaims.LabelPrinted = dtClaimPaymentView.Rows[0]["LabelPrinted"].ToSafeString();
                    interfaceClaims.PharmVerificationDone = dtClaimPaymentView.Rows[0]["PharmVerificationDone"].ToSafeString();
                    interfaceClaims.QueueName = dtClaimPaymentView.Rows[0]["QueueName"].ToSafeString();
                    interfaceClaims.PrimaryInsurancePaid = dtClaimPaymentView.Rows[0]["PriInsPaid"].ToSafeDecimal();
                    interfaceClaims.SecondaryInsurancePaid = dtClaimPaymentView.Rows[0]["SecInsPaid"].ToSafeDecimal();
                    interfaceClaims.TertiaryInsurancePaid = dtClaimPaymentView.Rows[0]["TerInsPaid"].ToSafeDecimal();
                }

                DataTable dtPayRecs = oPBL.GetPayRecs(interfaceClaims.RXNO, claims.Rows[0]["NREFILL"].ToSafeInt());
                if (dtPayRecs != null)
                {
                    RxPayInt rxPayInt = new RxPayInt();
                    for (int i = 0; i < dtPayRecs.Rows.Count; i++)
                    {
                        if (dtPayRecs.Rows[i]["RXNO"].ToSafeInt() != 0)
                        {
                            rxPayInt = new RxPayInt
                            {
                                InsuranceCode = dtPayRecs.Rows[i]["INS_CODE"].ToSafeString(),
                                RecType = dtPayRecs.Rows[i]["RECTYPE"].ToSafeString(),
                                RejectionCodes = dtPayRecs.Rows[i]["REJCODES"].ToSafeString(),
                            };
                            interfaceClaims.rxPayInts.Add(rxPayInt);
                        }
                    }

                }
                return interfaceClaims;
            }
            catch (Exception ex)
            {
                logger.Error("ConvertClaimsDataToInterface() ==> Following error has occurred : {0}", ex.ToString());
                return null;
            }
        }
        public static PharmacyInt ConvertPharmacyDataToInterface(DataTable pharmacy)
        {
            try
            {
                PharmacyInt interfacePharmacy = new PharmacyInt();
                MMSInterfaceLib.Models.AddressInt oAddress = new MMSInterfaceLib.Models.AddressInt();
                oAddress.AddressLine1 = pharmacy.Rows[0]["ADDL1"].ToSafeString();
                oAddress.AddressLine2 = pharmacy.Rows[0]["ADDL2"].ToSafeString();
                oAddress.City = pharmacy.Rows[0]["CITY"].ToSafeString();
                oAddress.State = pharmacy.Rows[0]["STATE"].ToSafeString();
                oAddress.ZipCode = pharmacy.Rows[0]["ZIP"].ToSafeString();
                interfacePharmacy.Address = oAddress;
                interfacePharmacy.NPINo = pharmacy.Rows[0]["PHNPINO"].ToSafeString();
                interfacePharmacy.NABP = pharmacy.Rows[0]["NABP"].ToSafeString();
                interfacePharmacy.Name = pharmacy.Rows[0]["DNAME"].ToSafeString();
                interfacePharmacy.Phone = pharmacy.Rows[0]["TEL_NO"].ToSafeString();
                interfacePharmacy.FaxNo = pharmacy.Rows[0]["FAX_NO"].ToSafeString();
                return interfacePharmacy;
            }
            catch (Exception ex)
            {
                logger.Error("ConvertPharmacyDataToInterface() ==> Following error has occurred : {0}", ex.ToString());
                return null;
            }
        }
        public string GetRxOriginDescription(string sRX_ORIGCD)
        {
            RxOrigin rxorig = RxOrigin.NotSpecified;
            switch (sRX_ORIGCD)
            {
                case "0":
                    rxorig = RxOrigin.NotSpecified;
                    break;
                case "1":
                    rxorig = RxOrigin.Written;
                    break;
                case "2":
                    rxorig = RxOrigin.Telephone;
                    break;
                case "3":
                    rxorig = RxOrigin.Electronic;
                    break;
                case "4":
                    rxorig = RxOrigin.Facsimile;
                    break;
                case "5":
                    rxorig = RxOrigin.Pharmacy;
                    break;

            }
            return rxorig.ToSafeString();
        }
        #endregion
    }
}
