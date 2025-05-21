using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
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
using System.Collections;
using PharmData;
using POS_Core.Document;
using System.IO;
using System.Threading;

namespace POS_Core_UI.UI
{
    public partial class frmPOSTransaction
    {
        POSTransaction posTrans = new POSTransaction();// PRIMEPOS-2794
        public static List<string> hyphenAlertDone = new List<string>(); //PRIMEPOS-3207

        #region CustomerEvent

        //done by sandeep
        private void txtCustomer_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                logger.Trace("txtCustomer_EditorButtonClick() - " + clsPOSDBConstants.Log_Entering);
                if (e.Button.Key == "S")
                {
                    SearchCustomer(false);
                }
                else if (e.Button.Key == "H")
                {
                    if (this.txtCustomer.Text.Trim().Length > 0)
                    {
                        frmViewPOSTransaction ofrm = new frmViewPOSTransaction();
                        ofrm.txtCustomer.Text = this.txtCustomer.Text;
                        ofrm.txtCustomer.Tag = this.txtCustomer.Tag;
                        ofrm.lblCustomerName.Text = this.lblCustomerName.Text;
                        ShowCustomerTokenImage(oPOSTrans.oCustomerRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                        ofrm.ShowDialog(this);
                    }
                }
                else if (e.Button.Key == "N")
                {
                    if (this.txtCustomer.Text.Trim().Length > 0)
                    {
                        frmCustomerNotesView ofrm = new frmCustomerNotesView(Configuration.convertNullToInt(this.txtCustomer.Tag.ToString()));
                        ofrm.ShowDialog(this);
                    }
                }
                #region Sprint-25 - PRIMEPOS-2371 22-Mar-2017 JY Added functionality to turn on the "Save Card Profile" flag
                else if (e.Button.Key == "C")
                {
                    if (this.txtCustomer.Text.Trim().Length > 0)
                    {
                        if (this.txtCustomer.Text.Trim() != this.txtCustomer.Tag.ToString().Trim())
                        {
                            clsUIHelper.ShowErrorMsg("Please select a customer to enable \"Save Card Profile\" settings.");
                            return;
                        }

                        int retVal = oPOSTrans.UpdateSaveCardProfileFlag(Configuration.convertNullToInt(this.txtCustomer.Tag.ToString()));
                        if (retVal > 0)
                        {
                            clsUIHelper.ShowSuccessMsg("\"Save Card Profile\" has been enabled successfully for selected customer.", "Success...");
                            if (oPOSTrans.oCustomerRow != null) oPOSTrans.oCustomerRow.SaveCardProfile = true;
                        }
                        else
                        {
                            clsUIHelper.ShowSuccessMsg("\"Save Card Profile\" already enabled for selected customer.", "Success...");
                        }

                    }
                }
                #endregion
                if (Configuration.CSetting.EnableCustomerEngagement)
                {
                    GetCustomerDetails(); //SAJID PRIMEPOS-2794
                    rightTabPayCust.Tabs[1].Selected = true;
                }
                logger.Trace("txtCustomer_EditorButtonClick() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "txtCustomer_EditorButtonClick()");
            }
        }

        //done by sandeep
        private void txtCustomer_Leave(object sender, System.EventArgs e)
        {
            try
            {
                logger.Trace("txtCustomer_Leave() - " + clsPOSDBConstants.Log_Entering);
                if (oPOSTrans.oCustomerRow != null && oPOSTrans.oCustomerRow.AccountNumber.ToString() == txtCustomer.Text.Trim() && oPOSTrans.oCustomerRow.CustomerId == Configuration.convertNullToInt(txtCustomer.Tag))
                {
                    string txtValue = this.txtCustomer.Tag.ToString();
                    if (txtValue.Trim() != "")
                    {
                        EditCustomer(txtValue, clsPOSDBConstants.Customer_tbl);
                        oPOSTrans.Validate_Customer(txtValue.Trim());
                    }
                }
                else
                {
                    loadedImages.Clear();   //PRIMEPOS-3157 28-Nov-2022 JY Added
                    pbImage.Image = null;   //PRIMEPOS-3157 28-Nov-2022 JY Added
                    if (txtCustomer.Text.Length > 0)
                    {
                        SearchCustomer(true);
                    }
                    else
                    {
                        ClearCustomer();
                    }
                }
                if (Configuration.CSetting.EnableCustomerEngagement)
                {
                    rightTabPayCust.Tabs[1].Selected = true;
                    GetCustomerDetails(); //SAJID PRIMEPOS-2794
                }
                logger.Trace("txtCustomer_Leave() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtCustomer_Leave()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                txtCustomer.Focus();
            }
        }

        //done by sandeep
        private void txtCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.txtItemCode.Focus();
                e.Handled = true;
            }
            else if (e.KeyData == Keys.F6)
            {
                txtCustomer_EditorButtonClick(sender, new Infragistics.Win.UltraWinEditors.EditorButtonEventArgs(txtCustomer.ButtonsRight["C"], sender));
                e.Handled = true;
            }
            else if (e.KeyData == Keys.F10)
            {
                txtCustomer_EditorButtonClick(sender, new Infragistics.Win.UltraWinEditors.EditorButtonEventArgs(txtCustomer.ButtonsRight["H"], sender));
                e.Handled = true;
            }
            else if (e.KeyData == Keys.F11)
            {
                txtCustomer_EditorButtonClick(sender, new Infragistics.Win.UltraWinEditors.EditorButtonEventArgs(txtCustomer.ButtonsRight["N"], sender));
                e.Handled = true;
            }
        }

        //done by sandeep
        private void txtCustomer_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.X != this.lastX || e.Y != this.lastY)
                {
                    this.lastX = e.X;
                    this.lastY = e.Y;

                    UltraTextEditor ultraTextEditor = (UltraTextEditor)sender;
                    UIElement element = ultraTextEditor.UIElement.LastElementEntered;
                    if (element != null)
                    {
                        EditorButtonBase editorButton = element.GetContext(typeof(EditorButtonBase)) as EditorButtonBase;
                        if (editorButton != null)
                        {
                            this.toolTip1.SetToolTip(this.txtCustomer, oPOSTrans.GetToolKeyDescForCustomer(editorButton.Key));
                        }
                    }
                    else
                    {
                        this.toolTip1.SetToolTip(this.txtCustomer, "");
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtCustomer_MouseMove()");
            }
        }

        #region PRIMEPOS-2896 
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.txtCustomer.Text.Trim() != "")
            {
                frmCustomers oCustomer = new frmCustomers();
                oCustomer.Edit(this.txtCustomer.Text);
                oCustomer.ShowDialog(this);
                if (!this.txtCustomer.Text.Equals("-1") && oCustomer.IsCanceled == false)
                    EditCustomer(this.txtCustomer.Text, clsPOSDBConstants.Customer_tbl);    //PRIMEPOS-3139 31-Aug-2022 JY Added
            }
        }

        private void btnCreditCardProfile_Click(object sender, EventArgs e)
        {
            if (this.txtCustomer.Text.Trim() != "")
            {
                frmCustomers oCustomer = new frmCustomers();
                oCustomer.Edit(this.txtCustomer.Text, true);
                oCustomer.ShowDialog(this);
                if (!this.txtCustomer.Text.Equals("-1") && oCustomer.IsCanceled == false)
                    EditCustomer(this.txtCustomer.Text, clsPOSDBConstants.Customer_tbl);    //PRIMEPOS-3139 31-Aug-2022 JY Added
            }
        }
        #endregion

        #endregion

        #region customerMethod
        //completed by sandeep
        private bool GetCustomer(string sCustContactnumber)
        {
            logger.Trace("GetCustomer() - " + clsPOSDBConstants.Log_Entering);
            sCustContactnumber = sCustContactnumber.Substring(0, txtItemCode.Text.Length - 1);
            bool bCustSelected = false;
            CustomerData oData = oPOSTrans.GetCustomerByContactNumber(sCustContactnumber);
            if (oData != null && oData.Customer.Rows.Count > 0)
            {
                this.txtCustomer.Text = oData.Customer[0].CustomerId.ToString();
                this.lblCustomerName.Text = oData.Customer[0].CustomerFullName;

                #region PRIMEPOS-2426 06-Jun-2019 JY Added
                try
                {
                    if (Configuration.convertNullToString(oData.Customer[0].AccountNumber).Trim() != "-1" && oData.Customer[0].DateOfBirth != null)
                    {
                        this.lblCustomerName.Text = oData.Customer[0].CustomerFullName.Trim() + " [DOB: " + Convert.ToDateTime(oData.Customer[0].DateOfBirth).ToShortDateString() + "]";
                    }
                }
                catch { }
                #endregion

                ShowCustomerTokenImage(oData.Customer[0].CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                this.txtCustomer.Tag = oData.Customer[0].CustomerId;
                this.txtItemCode.Text = "";
                bCustSelected = true;
            }
            if (Configuration.CSetting.EnableCustomerEngagement)//PRIMEPOS-2794
            {
                GetCustomerDetails();//PRIMEPOS-2794
            }
            logger.Trace("GetCustomer() - " + clsPOSDBConstants.Log_Exiting);
            return bCustSelected;
        }

        //completed by sandeep
        private void GetCustomer(string sCode, bool bByAcctNo)
        {

            string strCode = "";
            try
            {
                strCode = oPOSTrans.GetCustomerId(sCode, bByAcctNo);
                logger.Trace("GetCustomer() - About to enter FKEDIT with customerID: " + strCode);
                if (strCode.Trim() != "")
                {
                    EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                }
                logger.Trace("GetCustomer() - Successfully exit FKEDIT with customerID: " + strCode);

            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetCustomer()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
                ClearCustomer();
            }
            logger.Trace("GetCustomer() - " + clsPOSDBConstants.Log_Exiting);
            if (Configuration.CSetting.EnableCustomerEngagement)//PRIMEPOS-2794
            {
                GetCustomerDetails();//PRIMEPOS-2794
            }
            GetCustomerDocuments(); //PRIMEPOS-3157 28-Nov-2022 JY Added
        }

        //completed by sandeep
        private void ClearCustomer()
        {
            logger.Trace("ClearCustomer() - " + clsPOSDBConstants.Log_Entering);
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.lblCustomerName.Appearance.Image = null;   //PRIMEPOS-2611 13-Nov-2018 JY Added 
            this.txtCustomer.Tag = String.Empty;
            this.oCLCardRow = null;
            this.oPOSTrans.oCustomerRow = null;
            tempCustId = -1;            
            logger.Trace("ClearCustomer() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private CustomerData CheckAndUpdateCustomerFromPrimeRX(CustomerData oData)
        {
            logger.Trace("CheckAndUpdateCustomerFromPrimeRX() - " + clsPOSDBConstants.Log_Entering);
            CustomerRow orow = oData.Customer[0];
            bool isCustomerChangedInPrimeRX = false;
            DataSet oDs = oPOSTrans.IsCustomerChangedInPrimeRX(oData, tempPatNo, out isCustomerChangedInPrimeRX);
            if (isCustomerChangedInPrimeRX)
            {
                //Added By shitaljit on 19 July 2013. PRIMEPOS-1235 Add Preference to control Updating patient data from PrimeRX during transaction.
                bool bUpdateCustData = false;
                if (Configuration.CInfo.UpdatePatientData == "Y")
                {
                    bUpdateCustData = true;
                }
                else if (POS_Core_UI.Resources.Message.Display(this, "Customer Information has changed in the Pharmacy system. Do you want to update with the latest data?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bUpdateCustData = true;
                }
                else
                {
                    tempPatNo = orow.PatientNo;
                }
                if (bUpdateCustData == true)
                {
                    oData = oPOSTrans.CreateCustomerDSFromPatientDS(oDs, false);
                    oPOSTrans.PersistCustomer(oData, true);
                }
            }

            logger.Trace("CheckAndUpdateCustomerFromPrimeRX() - " + clsPOSDBConstants.Log_Exiting);
            return oData;
        }

        //completed by sandeep
        private void SearchCustomer(bool autoSelectSingleRow)
        {
            try
            {
                logger.Trace("SearchCustomer() - " + clsPOSDBConstants.Log_Entering);
                CustomerData oCustdata = new CustomerData();
                CustomerRow oCustRow = null;
                string strCode = "";
                tempCustId = -1;
                if (autoSelectSingleRow == true && txtCustomer.Text.Trim() != "" && txtCustomer.Text != "-1")
                {
                    logger.Trace("SearchCustomer() - About to GetCustomerByID: " + Configuration.convertNullToInt(txtCustomer.Text));
                    oCustdata = oPOSTrans.GetCustomerByID(Configuration.convertNullToInt(txtCustomer.Text));
                    logger.Trace("SearchCustomer() - GetCustomerByID completed");
                    if (oCustdata.Tables[0].Rows.Count > 0)
                    {
                        logger.Trace("SearchCustomer() - Customer exist count > 0");
                        oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                        strCode = oCustRow.CustomerId.ToString();
                        logger.Trace("SearchCustomer() - About to enter FKEDIT with customerID: " + strCode);
                        EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                        logger.Trace("SearchCustomer() - Successfully exit FKEDIT with customerID: " + strCode);
                        logger.Trace("SearchCustomer() - " + clsPOSDBConstants.Log_Exiting);

                        if (isOnHoldTrans == false && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CInfo.SearchRxsWithPatientName == true && Configuration.convertNullToInt(oCustdata.Customer[0].PatientNo) != 0) //PRIMEPOS-2639 25-Feb-2019 JY Added "isOnHoldTrans == false" to handle one exception - there might be posibility that same Rx is on hold and in the unpicked Rx list, so when we process on-hold rx no need to popup unpicked rx list
                            CheckUnPickedRXs(oCustdata.Customer[0].PatientNo.ToString()); //Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
                        return;
                    }
                }
                if (autoSelectSingleRow == true && (this.txtCustomer.Text.Trim() == "" || this.txtCustomer.Text == "-1"))
                {
                    logger.Trace("SearchCustomer() - About to Populate Customer: " + txtCustomer.Text);
                    oCustdata = oPOSTrans.PopulateCustomer(txtCustomer.Text);
                    logger.Trace("SearchCustomer() - Populate Customer completed");
                    if (oCustdata.Tables[0].Rows.Count > 0)
                    {
                        logger.Trace("SearchCustomer() - Customer exist count > 0");
                        oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                        strCode = oCustRow.CustomerId.ToString();
                        logger.Trace("SearchCustomer() - About to enter FKEDIT with customerID: " + strCode);
                        EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                        logger.Trace("SearchCustomer() - Successfully exit FKEDIT with customerID: " + strCode);
                        logger.Trace("SearchCustomer() - " + clsPOSDBConstants.Log_Exiting);
                        return;
                    }
                }
                //frmCustomerSearch oSearch = new frmCustomerSearch(txtCustomer.Text, autoSelectSingleRow);
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, autoSelectSingleRow, true, clsPOSDBConstants.Customer_tbl);
                if (Configuration.CSetting.EnableCustomerEngagement)//PRIMEPOS-2794
                {
                    GetCustomerDetails(); //SAJID PRIMEPOS-2794
                }
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (oSearch.IsCanceled == true && string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == false)
                {
                    return;
                }
                else if (oSearch.IsCanceled == true && string.IsNullOrEmpty(this.txtCustomer.Text.Trim()) == true)
                {
                    txtCustomer.Text = "-1";
                    GetCustomer(this.txtCustomer.Text, true);
                    return;
                }
                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        ClearCustomer();
                        logger.Trace("SearchCustomer() - " + clsPOSDBConstants.Log_Exiting);
                        return;
                    }

                    oCustdata = oPOSTrans.GetCustomerByID(Configuration.convertNullToInt(strCode));
                    if (oCustdata.Tables[0].Rows.Count == 0)
                    {
                        oCustRow = oSearch.SelectedRow();
                        if (oCustRow != null)
                        {
                            oCustdata.Tables[0].ImportRow(oCustRow);
                            oPOSTrans.PersistCustomer(oCustdata, true);
                            oCustdata = oPOSTrans.GetCustomerByPatientNo(oCustRow.PatientNo);
                            if (oCustdata.Tables[0].Rows.Count > 0)
                            {
                                oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                                strCode = oCustRow.CustomerId.ToString();
                            }
                        }
                    }
                    EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                    this.txtItemCode.Text = "";

                    if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CInfo.SearchRxsWithPatientName == true && Configuration.convertNullToInt(oCustdata.Customer[0].PatientNo) != 0)
                        CheckUnPickedRXs(oCustdata.Customer[0].PatientNo.ToString()); //Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
                }
                else if (oPOSTrans.oCustomerRow != null && oSearch.IsCanceled == false)
                {
                    EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                }
                else
                {
                    ClearCustomer();
                }
                logger.Trace("SearchCustomer() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchCustomer()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                ClearCustomer();
            }
        }

        //completed by sandeep
        private void EditCustomer(string code, string senderName)
        {
            try
            {
                if (senderName == clsPOSDBConstants.Customer_tbl)
                {
                    #region Customer

                    try
                    {
                        CustomerData oCustomerData;
                        logger.Trace("EditCustomer() - FKEdit " + senderName + " About to get Customer by ID: " + Configuration.convertNullToInt(code));
                        oCustomerData = oPOSTrans.GetCustomerByID(Configuration.convertNullToInt(code));
                        logger.Trace("EditCustomer() - FKEdit " + senderName + " After GetCustomer by ID: " + Configuration.convertNullToInt(code));
                        //Updated By SRT(Gaurav) Date : 21-Jul-2009
                        //Validated whether the customer DATASET  IS NOT NULL
                        if (oCustomerData == null || oCustomerData.Customer.Count == 0)
                        {
                            logger.Trace("EditCustomer() - FKEdit " + senderName + " Customer is null");
                            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -999))
                            {
                                if (Resources.Message.Display("Customer Not Found found.\nDo You Want To Add This Customer.", "Manage Customer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    logger.Trace("EditCustomer() - FKEdit " + senderName + " About to Add customer");
                                    frmCustomers ofrmCustomers = new frmCustomers();
                                    ofrmCustomers.ShowDialog();
                                    if (ofrmCustomers.IsCanceled == true)
                                    {
                                        ClearCustomer();
                                        txtCustomer.Focus();
                                        return;
                                    }
                                    else
                                    {
                                        logger.Trace("EditCustomer() - FKEdit " + senderName + " Before Add customer");
                                        oCustomerData = ofrmCustomers.CustmerData;
                                        logger.Trace("EditCustomer() - " + senderName + " Done Add customer");
                                    }
                                }
                                else
                                {
                                    logger.Trace("EditCustomer() - " + senderName + " Do NOT Add customer (No was click)");
                                    ClearCustomer();
                                    txtCustomer.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                logger.Trace("EditCustomer() - " + senderName + " Customer Not Found");
                                clsUIHelper.ShowErrorMsg("Customer Not Found.");
                                ClearCustomer();
                                txtCustomer.Focus();
                                return;
                            }
                        }

                        logger.Trace("EditCustomer() - " + senderName + " About to get customer Data");
                        oPOSTrans.oCustomerRow = oCustomerData.Customer[0];
                        logger.Trace("EditCustomer() - " + senderName + " Get customer Data completed");
                        if (oPOSTrans.oCustomerRow != null)
                        {
                            logger.Trace("EditCustomer() - " + senderName + " CustomerRow is not Null");
                            if (oPOSTrans.oCustomerRow.AccountNumber != -1 && Configuration.convertNullToString(this.txtCustomer.Tag) != oPOSTrans.oCustomerRow.CustomerId.ToString())
                            {
                                #region PRIMEPOS-3207
                                if (hyphenAlertDone != null && hyphenAlertDone.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(oPOSTrans.oCustomerRow.CustomerCode) || !oPOSTrans.oCustomerRow.CustomerCode.Equals("-1"))
                                    {
                                        if (!hyphenAlertDone.Contains(oPOSTrans.oCustomerRow.CustomerCode))
                                        {
                                            var rss3 = GetHyphenData(oPOSTrans.oCustomerRow.CustomerCode);
                                            hyphenAlertDone.Add(oPOSTrans.oCustomerRow.CustomerCode);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(oPOSTrans.oCustomerRow.CustomerCode) || !oPOSTrans.oCustomerRow.CustomerCode.Equals("-1"))
                                    {
                                        var rss4 = GetHyphenData(oPOSTrans.oCustomerRow.CustomerCode);
                                        hyphenAlertDone.Add(oPOSTrans.oCustomerRow.CustomerCode);
                                    }
                                }
                                #endregion

                                logger.Trace("EditCustomer() - " + senderName + " Customer is not -1, customer ID: " + oPOSTrans.oCustomerRow.CustomerId.ToString());
                                if (Configuration.CPOSSet.ShowCustomerNotes == true)
                                {
                                    logger.Trace("EditCustomer() - " + senderName + " About Populate customer Notes");
                                    CustomerNotesData oData = oPOSTrans.GetCustomerNotesByCustId(oPOSTrans.oCustomerRow.CustomerId, true);
                                    logger.Trace("EditCustomer() - " + senderName + " Populate customer Notes");
                                    if (oData.CustomerNotes.Rows.Count > 0)
                                    {
                                        logger.Trace("EditCustomer() - " + senderName + " About to Show customer notes, customerID: " + oPOSTrans.oCustomerRow.CustomerId);
                                        frmCustomerNotesView ofrm = new frmCustomerNotesView(oPOSTrans.oCustomerRow.CustomerId);
                                        ofrm.ShowDialog(this);
                                        logger.Trace("EditCustomer() - " + senderName + " Show customer Notes");
                                    }
                                }
                            }
                            if (Configuration.CInfo.UpdatePatientData != "N" && oPOSTrans.oCustomerRow.AccountNumber != -1 && UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -999))
                            {
                                oCustomerData = CheckAndUpdateCustomerFromPrimeRX(oCustomerData);
                                if (Configuration.isNullOrEmptyDataSet(oCustomerData) == false)
                                {
                                    oPOSTrans.oCustomerRow = (CustomerRow)oCustomerData.Customer.Rows[0];
                                    if (Configuration.CSetting.EnableCustomerEngagement)//PRIMEPOS-2794
                                    {
                                        GetCustomerDetails(); //PRIMEPOS-2794 SAJID DHUKKA
                                        rightTabPayCust.Tabs[1].Selected = true;
                                    }
                                    GetCustomerDocuments(); //PRIMEPOS-3157 28-Nov-2022 JY Added
                                }
                            }
                            else
                            {
                                if (Configuration.CSetting.EnableCustomerEngagement)//PRIMEPOS-2794
                                {
                                    rightTabPayCust.Tabs[0].Selected = true;
                                }
                            }// PRIMEPOS-2794
                            this.txtCustomer.Text = oPOSTrans.oCustomerRow.AccountNumber.ToString();
                            this.lblCustomerName.Text = oPOSTrans.oCustomerRow.CustomerFullName;

                            #region PRIMEPOS-2426 06-Jun-2019 JY Added
                            try
                            {
                                if (Configuration.convertNullToString(oPOSTrans.oCustomerRow.AccountNumber).Trim() != "-1" && oPOSTrans.oCustomerRow.DateOfBirth != null)
                                {
                                    this.lblCustomerName.Text = oPOSTrans.oCustomerRow.CustomerFullName.Trim() + " [DOB: " + Convert.ToDateTime(oPOSTrans.oCustomerRow.DateOfBirth).ToShortDateString() + "]";
                                }
                            }
                            catch { }
                            #endregion

                            ShowCustomerTokenImage(oPOSTrans.oCustomerRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                            this.txtCustomer.Tag = oPOSTrans.oCustomerRow.CustomerId;
                            //Added By Dharmendra(SRT) which will be required when processing
                            if (oPOSTrans.oCustomerRow.UseForCustomerLoyalty == true)
                            {
                                if (this.oCLCardRow != null && this.oCLCardRow.CustomerID != oPOSTrans.oCustomerRow.CustomerId)
                                {
                                    oCLCardRow = oPOSTrans.GetActiveCardForCustomerID(oPOSTrans.oCustomerRow.CustomerId);
                                }
                            }
                        }
                    }
                    catch (System.IndexOutOfRangeException e)
                    {
                        logger.Fatal(e, "(EditCustomer()");
                        logger.Trace("EditCustomer() - " + senderName + " About search customer");
                        SearchCustomer(true);
                        logger.Trace("EditCustomer() - " + senderName + " Done search customer");
                    }
                    catch (Exception exp)
                    {
                        clsUIHelper.ShowErrorMsg(exp.Message);
                        logger.Fatal(exp, "EditCustomer()");
                        SearchCustomer(true);
                    }
                    #endregion Customer
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "EditCustomer()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        //completed by sandeep
        private bool CheckUnPickedRXs(string PatientNo)
        {
            RXHeader oRXHeader = null;
            int iNDC = 0;
            try
            {
                logger.Trace("CheckUnPickedRXs() - " + clsPOSDBConstants.Log_Entering);
                DataTable oRxInfo;

                frmPatientRXSearch ofrm = new frmPatientRXSearch(PatientNo, 'P');
                DateTime oDate = Configuration.MinimumDate;
                ofrm.Search();
                DataTable dt = ofrm.SelectedData;

                if (dt == null)
                {
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                oRxInfo = ofrm.SelectedData;
                if (oPOSTrans.CheckUnpickedRxLocally(oRxInfo))
                {
                    return false;
                }
                if (Resources.Message.Display("There are unpicked Rxs for the selected customer." + Environment.NewLine + "Would you like to view them?", "UnPicked RXs", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    isSearchUnPickCancel = false;//PRIMEPOS-3319
                    if (ofrm.ShowDialog(this) == DialogResult.Cancel)
                    {
                        isSearchUnPickCancel = true; //User click cancel
                        return false;
                    }
                    oRxInfo = ofrm.SelectedData;
                    if (oRxInfo.Rows.Count == 0)
                    {
                        isSearchUnPickCancel = true; //Added by Manoj 3/31/2014 -- User did not select any rx
                        return false;
                    }

                    #region  PRIMEPOS-2459 27-Feb-2019 JY Added
                    bool bNewPatient = false;
                    ArrayList alRxNo = new ArrayList();
                    ArrayList alPatientNo = new ArrayList();
                    try
                    {
                        foreach (DataRow row in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                        {
                            if (!alRxNo.Contains(row["RXNo"].ToString()))
                                alRxNo.Add(row["RXNo"].ToString());
                            if (!alPatientNo.Contains(row["PatientNo"].ToString()))
                            {
                                bNewPatient = true;
                                alPatientNo.Add(row["PatientNo"].ToString());
                            }
                        }
                    }
                    catch { }
                    #endregion

                    bool bSkipRx = false;
                    foreach (DataRow oRXRow in oRxInfo.Rows)
                    {
                        bNewPatient = false;
                        string sRXNo = oRXRow["RXNo"].ToString();
                        string sRefill = oRXRow["nrefill"].ToString();
                        string sPartialFillNo = "0";
                        if (oRxInfo.Columns.Contains("PartialFillNo"))
                            sPartialFillNo = oRXRow["PartialFillNo"].ToString();

                        if ((oRxInfo.Rows[0]["Pickedup"].ToString() == "Y" || oRxInfo.Rows[0]["PickupPOS"].ToString() == "Y")
                          && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            continue;
                        }

                        bool itemAlreadyExist = false;
                        TransDetailRXRow oDetail = null;
                        foreach (DataRow rID in oPOSTrans.oTransDRXData.TransDetailRX.Rows)
                        {
                            bNewPatient = false;
                            oDetail = oPOSTrans.oTransDRXData.TransDetailRX.GetRowByID(Convert.ToInt32(rID["RXDETAILID"].ToString()));
                            if (oDetail == null)
                            {
                                continue;
                            }
                            if ((oDetail.RXNo.ToString() == oRXRow["RXNo"].ToString() && Configuration.CInfo.AllowMultipleRXRefillsInSameTrans == false)
                                || (oDetail.RXNo.ToString() == oRXRow["RXNo"].ToString() && oDetail.NRefill.ToString() == oRXRow["nrefill"].ToString()
                                && Configuration.CInfo.AllowMultipleRXRefillsInSameTrans == true))
                            {
                                itemAlreadyExist = true;
                                break;
                            }
                            else
                            {
                                #region PRIMEPOS-2459 27-Feb-2019 JY Added
                                bool bNewRx = false;
                                try
                                {
                                    if (!alRxNo.Contains(oRXRow["RXNo"].ToString()))
                                    {
                                        bNewRx = true;
                                        alRxNo.Add(oRXRow["RXNo"].ToString());
                                    }

                                    if (!alPatientNo.Contains(oRXRow["PatientNo"].ToString()))
                                    {
                                        bNewPatient = true;
                                        alPatientNo.Add(oRXRow["PatientNo"].ToString());
                                    }

                                    #region PRIMEPOS-2317 02-Apr-2019 JY Added
                                    try
                                    {
                                        if (bNewPatient)
                                        {
                                            bSkipRx = false;
                                            PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                                            DataTable oTable1 = oPharmBL.GetPatient(oRXRow["PatientNo"].ToString());
                                            if (Configuration.CInfo.ConfirmPatient == 1)
                                            {
                                                ShowPatientInformation(oTable1, false);
                                            }
                                            else if (Configuration.CInfo.ConfirmPatient == 2)
                                            {
                                                if (!ShowPatientInformation(oTable1, true))
                                                {
                                                    bool bTemp = isSearchUnPickCancel;
                                                    oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, isAnyUnPickRx, ref isSearchUnPickCancel); //PRIMEPOS-3319
                                                    isSearchUnPickCancel = bTemp;
                                                    bSkipRx = true;
                                                    break;
                                                }
                                            }
                                            oTable1.Dispose();
                                            oTable1 = null;
                                        }
                                    }
                                    catch { }
                                    #endregion
                                    if (bSkipRx == false)
                                    {
                                        if (bNewRx && bNewPatient)
                                            ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), oRXRow["PatientNo"].ToString());
                                        else if (bNewRx && !bNewPatient)
                                            ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), "");
                                        else if (!bNewRx && bNewPatient)
                                            ShowPrimeRXPOSNotes("", oRXRow["PatientNo"].ToString());
                                    }
                                }
                                catch { }
                                #endregion
                            }
                        }

                        #region PRIMEPOS-2317 04-Apr-2019 JY Added
                        if (oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count == 0)
                        {
                            if (!alRxNo.Contains(oRXRow["RXNO"].ToString()))
                                alRxNo.Add(oRXRow["RXNO"].ToString());
                            if (!alPatientNo.Contains(oRXRow["PatientNo"].ToString()))
                            {
                                bNewPatient = true;
                                alPatientNo.Add(oRXRow["PatientNo"].ToString());
                            }

                            try
                            {
                                if (bNewPatient)
                                {
                                    bSkipRx = false;
                                    PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                                    DataTable oTable1 = oPharmBL.GetPatient(oRXRow["PatientNo"].ToString());
                                    if (Configuration.CInfo.ConfirmPatient == 1)
                                    {
                                        ShowPatientInformation(oTable1, false);
                                    }
                                    else if (Configuration.CInfo.ConfirmPatient == 2)
                                    {
                                        if (!ShowPatientInformation(oTable1, true))
                                        {
                                            bool bTemp = isSearchUnPickCancel;
                                            oPOSTrans.DeleteRxOnValidating(RxWithValidClass, DrugClassInfoCapture, false, ref isSearchUnPickCancel);
                                            isSearchUnPickCancel = bTemp;
                                            bSkipRx = true;
                                        }
                                    }
                                    oTable1.Dispose();
                                    oTable1 = null;

                                    if (bSkipRx == false) ShowPrimeRXPOSNotes(oRXRow["RXNo"].ToString(), oRXRow["PatientNo"].ToString());
                                }
                            }
                            catch { }
                        }
                        #endregion

                        if (itemAlreadyExist == true | bSkipRx == true)
                        {
                            continue;
                        }

                        oPOSTrans.isValidDrugClassRx(oRXRow["RXNO"].ToString(), oRXRow["NRefill"].ToString(), oRXRow["Class"].ToString(), ref RxWithValidClass, sPartialFillNo);
                        oPOSTrans.SetTransactionRowForUnPickedRx(oRXRow, DefaultQTY);
                        grdDetail.Refresh();

                        //setRxHeader
                        RXDetail oRXDetail;
                        oRXHeader = oPOSTrans.SetRxHeaderForUnPickedRx(oRXRow, out oRXDetail);
                        if (oRXHeader.RXDetails.Count > 0)
                        {
                            isAnyUnPickRx = true;
                        }
                        //Comment by Manoj 3/31/2014 this was causing the POS to pass x# RxHeaderlist for the same patient
                        //making ask X times for hippa - the number of #Rx == No of Hippa. Remove to Fix
                        //this.oRXHeaderList.Add(oRXHeader);
                        if (oPOSTrans.oTDRow != null)
                        {
                            DataTable oTable = oPOSTrans.GetPatient(oRxInfo.Rows[iNDC]["PatientNo"].ToString());//added by atul 07-jan-2011
                            string ezcap = oTable.Rows[0]["EZCAP"].ToString();
                            // added 2 parameters counsellingReq & ezcap by atul 07-jan-2011
                            if (oRXHeader.CounselingRequest == null)
                            {
                                oRXHeader.CounselingRequest = "";
                            }
                            if (this.grdDetail.Rows.Count == 0 && oPOSTrans.oTransDRXData.TransDetailRX.Rows.Count > 0)
                            {
                                oPOSTrans.oTransDRXData.TransDetailRX.Rows.Clear();
                            }
                            oPOSTrans.oTransDRXData.TransDetailRX.AddRow(oPOSTrans.oTDRow.TransDetailID, Convert.ToDateTime(oRXDetail.RxDate), oRXDetail.RXNo, oRxInfo.Rows[iNDC]["ndc"].ToString(), Configuration.convertNullToInt64(oTable.Rows[0]["PatientNo"]), oRxInfo.Rows[iNDC]["BillType"].ToString(), oRxInfo.Rows[iNDC]["PatType"].ToString(), (int)oRXDetail.RefillNo, oRXHeader.CounselingRequest, ezcap, Configuration.convertNullToString(oRxInfo.Rows[iNDC]["DELIVERY"]), (int)oRXDetail.PartialFillNo);  //Sprint-25 - PRIMEPOS-2322 02-Feb-2017 JY Added logic to update correct PatientNo w.r.t. Rx //PRIMEPOS-3008 30-Sep-2021 JY Added

                            oTable.Dispose();
                            oTable = null;
                            ezcap = string.Empty;//end of added by atul 07-jan-2011
                            iNDC += 1; //Added by Manoj 1/7/2015
                        }
                        this.isAddRow = true;
                        if (Configuration.CPOSSet.UsePoleDisplay)
                        {
                            ShowItemOnPoleDisp(oPOSTrans.oTDRow);
                        }
                        DisplayItemOnSigPad();
                        oPOSTrans.oTDRow = null;  //Sprint-18 20-Nov-2014 JY Added to resolve blank item description or update item desc as per search text box  
                    }
                    if (grdDetail.Rows.Count == 1 || string.IsNullOrEmpty(TransStartDateTime))
                    {
                        TransStartDateTime = DateTime.Now.ToString();//To Capture TranXn Start time;
                    }
                }
                logger.Trace("CheckUnPickedRXs() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CheckUnPickedRXs()");
                return false;
            }
            return true;
        }



        //completed by sandeep
        public void ApplyCustomerDiscount(out bool isException, string sCustName)
        {
            logger.Trace("ApplyCustomerDiscount() - " + clsPOSDBConstants.Log_Entering);
            custDiscount = 0;
            isException = false;
            bool isDiscountableTrans = false;

            isDiscountableTrans = ValidateCustomerDiscount(oPOSTrans.oCustomerRow.Discount, out custDiscount);
            if (custDiscount > 0 && isDiscountableTrans == true)
            {
                if (Configuration.convertNullToDecimal(this.lblInvDiscount.Text) == 0)
                {
                    this.lblInvDiscount.Text = Configuration.convertNullToString(custDiscount);
                }
                //Added in case customer is changed in CL Card input screen.
                else if (Configuration.convertNullToDecimal(this.lblInvDiscount.Text) > 0)
                {
                    //Warn if there is already invoice dicount in the transaction.

                    /*Date 27-jan-2014
                     * Modified by Shitaljit
                     * For making currency symbol dynamic
                     */
                    if (Resources.Message.Display("Current transaction already has invoice discount of " + Configuration.CInfo.CurrencySymbol.ToString() + Configuration.convertNullToDecimal(this.lblInvDiscount.Text).ToString() + " for customer \"" + Configuration.convertNullToString(sCustName).Trim() + "\".\nWould you like to add discount of " + Configuration.CInfo.CurrencySymbol.ToString() + custDiscount + " to existing discount? ", "Invoice Discount", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        this.lblInvDiscount.Text = "0.00";
                    }
                    //Added in case customer is changed in CL Card input screen.
                    this.lblInvDiscount.Text = Configuration.convertNullToString(Configuration.convertNullToDecimal(this.lblInvDiscount.Text) + custDiscount);
                }
                RecalculateTax();
                this.ultraCalcManager1.ReCalc();
            }
            else if (isDiscountableTrans == false)
            {
                isException = true;
            }
            logger.Trace("ApplyCustomerDiscount() - " + clsPOSDBConstants.Log_Exiting);
        }
        //completed by sandeep
        private void oCustomer_DataRowSave()
        {
            Application.DoEvents();
        }

        //completed by sandeep
        private void AutoSelectRxCustomer(string PatientNo, DataSet dsPatient=null)
        {
            DataSet oDS = dsPatient;
            PharmBL oPBL = new PharmBL();
            if (oDS == null || !oPBL.ConnectedTo_ePrimeRx())
            {
                oPBL.GetPatient(PatientNo, out oDS);
            }
            if (oDS != null && oDS.Tables.Count > 0)
            {
                Application.DoEvents();
                CustomerData oCustomerData = oPOSTrans.CreateCustomerDSFromPatientDS(oDS, false);
                if (oCustomerData.Customer.Rows.Count == 0)
                {
                    POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Application.DoEvents();
                    Customer oCustomer = new Customer();
                    oCustomer.DataRowSaved += new Customer.DataRowSavedHandler(oCustomer_DataRowSave);
                    oPOSTrans.PersistCustomer(oCustomerData, true);
                    CustomerRow oCustRow = oCustomerData.Customer[0];
                    string strCode = oCustRow.CustomerId.ToString();
                    EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                }
            }
            else
            {
                POS_Core_UI.Resources.Message.Display(this, "No Patient data found.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AutoSelectFirstRxCustomer(bool isBatchSelected = false)
        {
            if (!txtCustomer.Enabled) return;   //Sprint-25 - PRIMEPOS-2410 27-Apr-2017 JY Added 
            logger.Trace("AutoSelectFirstRxCustomer() - " + clsPOSDBConstants.Log_Entering);
            foreach (TransDetailRow tmpRow in this.oPOSTrans.oTransDData.TransDetail.Rows)
            {
                if (tmpRow.ItemID.Trim().ToUpper() == "RX" && tmpRow.ItemDescription.Contains("-")) //PRIMEPOS-3053 09-Feb-2021 JY modified to avoid exception
                {
                    long iRxNo; //PRIMEPOS-2515 17-Oct-2018 JY changed RxNo datatype from int to long
                    int iRefillNo;
                    int iPartialFillNo = 0;
                    oPOSTrans.ExtractRXInfoFromDescription(tmpRow.ItemDescription, out iRxNo, out iRefillNo, out iPartialFillNo);
                    int cnt = 0;
                    foreach (RXHeader oRXHeader1 in oPOSTrans.oRXHeaderList)
                    {
                        if (oRXHeader1.RXDetails[cnt].RXNo == iRxNo && oRXHeader1.RXDetails[cnt].RefillNo == iRefillNo)
                        {
                            string PatientNo = oRXHeader1.PatientNo;
                            if (PatientNo.Trim().Length > 0)
                            {
                                string strCode = oPOSTrans.GetCustomerId(" where PatientNo=" + PatientNo);
                                if (strCode != "")
                                {
                                    if (txtCustomer.Text.Trim() != strCode.Trim())
                                        EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                                }
                                else
                                {
                                    if (Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2)
                                    {
                                        #region PRIMEPOS-2886 28-Aug-2020 JY Added
                                        if (!isBatchSelected)
                                        {
                                            DataSet oDS = null;
                                            if (oRXHeader1.TblPatient != null && oRXHeader1.TblPatient.Rows.Count > 0)
                                            {
                                                oDS = new DataSet("dsPatient");
                                                oDS.Tables.Add(oRXHeader1.TblPatient.Copy());
                                            }
                                            else
                                            {
                                                PharmBL oPBL = new PharmBL();
                                                oPBL.GetPatient(PatientNo, out oDS);
                                            }
                                            CustomerData oCustomerData = new CustomerData();
                                            if (oDS != null && oDS.Tables.Count > 0)
                                            {
                                                Customer oCustomer = new Customer();
                                                oCustomerData = oCustomer.GetExactCustomerByMultiplePatientsPatameters(oDS);
                                                if (oCustomerData != null && oCustomerData.Tables.Count > 0 && oCustomerData.Tables[0].Rows.Count > 0)
                                                {
                                                    oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(oDS, false);
                                                    oCustomer.Persist(oCustomerData, true);
                                                    strCode = Configuration.convertNullToString(oCustomerData.Tables[0].Rows[0]["CustomerID"]);
                                                    if (txtCustomer.Text.Trim() != strCode.Trim())
                                                        EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                                                }
                                                else
                                                {
                                                    if (Configuration.CInfo.AutoImportCustAtTrans == 2)
                                                    {
                                                        frmCustomerPatientMapping ofrmCustomerPatientMapping = new frmCustomerPatientMapping(oDS);
                                                        ofrmCustomerPatientMapping.ShowDialog();
                                                        oCustomerData = ofrmCustomerPatientMapping.oCustomerData;
                                                        if (oCustomerData != null && oCustomerData.Tables.Count > 0 && oCustomerData.Tables[0].Rows.Count > 0)
                                                        {
                                                            strCode = Configuration.convertNullToString(oCustomerData.Tables[0].Rows[0]["CustomerID"]);
                                                            if (txtCustomer.Text.Trim() != strCode.Trim())
                                                                EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AutoSelectRxCustomer(PatientNo, oDS);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            AutoSelectRxCustomer(PatientNo);
                                        #endregion
                                    }
                                    else
                                    {
                                        if (txtCustomer.Text != "-1" && txtCustomer.Text.Trim() != "")
                                        {
                                            if (!isBatchSelected && Resources.Message.Display("Auto import customer is off. Customer not found. Would you like to add?", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                            {
                                                AutoSelectRxCustomer(PatientNo);
                                                #region PRIMEPOS-2886 28-Aug-2020 JY Added
                                                //DataSet oDS = null;
                                                //MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                                                //oAcct.GetPatientByCode(Convert.ToInt32(PatientNo), out oDS);
                                                //CustomerData oCustomerData = new CustomerData();
                                                //if (oDS != null)
                                                //{
                                                //    Customer oCustomer = new Customer();
                                                //    oCustomerData = oCustomer.GetExactCustomerByMultiplePatientsPatameters(oDS);
                                                //    if (oCustomerData != null && oCustomerData.Tables.Count > 0 && oCustomerData.Tables[0].Rows.Count > 0)
                                                //    {
                                                //        oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(oDS, false);
                                                //        oCustomer.Persist(oCustomerData, true);
                                                //        strCode = Configuration.convertNullToString(oCustomerData.Tables[0].Rows[0]["CustomerID"]);
                                                //        if (txtCustomer.Text.Trim() != strCode.Trim())
                                                //            EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                                                //    }
                                                //    else
                                                //    {
                                                //        frmCustomerPatientMapping ofrmCustomerPatientMapping = new frmCustomerPatientMapping(oDS);
                                                //        ofrmCustomerPatientMapping.ShowDialog();
                                                //        oCustomerData = ofrmCustomerPatientMapping.oCustomerData;
                                                //        if (oCustomerData != null && oCustomerData.Tables.Count > 0 && oCustomerData.Tables[0].Rows.Count > 0)
                                                //        {
                                                //            strCode = Configuration.convertNullToString(oCustomerData.Tables[0].Rows[0]["CustomerID"]);
                                                //            if (txtCustomer.Text.Trim() != strCode.Trim())
                                                //                EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                                                //        }
                                                //    }
                                                //}                                                
                                                #endregion
                                            }
                                            else
                                            {
                                                txtCustomer.Text = "-1";
                                                GetCustomer(this.txtCustomer.Text, true);
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                        cnt++;
                    }
                    break;
                }
            }
            logger.Trace("AutoSelectFirstRxCustomer() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private CustomerRow SearchCustomerForOnHoldTrans()
        {
            logger.Trace("SearchCustomerForOnHoldTrans() - " + clsPOSDBConstants.Log_Entering);
            CustomerRow oCustRow = null;

            //Added By shitaljit (QuicSolv) on 28 May 2012
            CustomerData oCustdata = new CustomerData();
            string strCode = "";
            tempCustId = -1;
            //frmCustomerSearch oSearch = new frmCustomerSearch("");
            frmSearchMain oSearch = new frmSearchMain("", true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference            
            //Added By Shitaljit if customer is already populated in transaction screen no need to search the customer again.
            if (oPOSTrans.oCustomerRow.AccountNumber != -1)
            {
                //oSearch = new frmCustomerSearch(Configuration.convertNullToString(oPOSTrans.oCustomerRow.AccountNumber));
                oSearch = new frmSearchMain(Configuration.convertNullToString(oPOSTrans.oCustomerRow.AccountNumber), true, clsPOSDBConstants.Customer_tbl);   //18-Dec-2017 JY Added new reference
                oSearch.bSearchExactCustomer = true;
            }
            oSearch.ActiveOnly = 1;
            oSearch.ShowDialog(this);
            if (!oSearch.IsCanceled)
            {
                strCode = oSearch.SelectedRowID();
                if (strCode == "")
                {
                    return null;
                }

                //Added By shitaljit to add customer to DB if it is a customer from PrimeRx that is not exist in POS currently.
                oCustdata = oPOSTrans.GetCustomerByID(Configuration.convertNullToInt(strCode));
                if (oCustdata.Tables[0].Rows.Count == 0)
                {
                    oCustRow = oSearch.SelectedRow();
                    if (oCustRow != null)
                    {
                        oCustdata.Tables[0].ImportRow(oCustRow);
                        oPOSTrans.PersistCustomer(oCustdata, true);
                        oCustdata = oPOSTrans.GetCustomerByPatientNo(oCustRow.PatientNo);
                        if (oCustdata.Tables[0].Rows.Count > 0)
                        {
                            oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                            strCode = oCustRow.CustomerId.ToString();
                        }
                    }
                }
                else
                {
                    oCustRow = oCustdata.Customer[0];
                }
            }
            logger.Trace("SearchCustomerForOnHoldTrans() - " + clsPOSDBConstants.Log_Exiting);
            return oCustRow;
        }
        private void PopulateCustForRetTrans(string custId)
        {
            CustomerData oCustdata = new CustomerData();
            CustomerRow oCustRow = null;
            string strCode = "";
            if (oPOSTrans.oTransHRow.ReturnTransID > 0)
            {
                oPOSTrans.oTransHRow.CustomerID = Configuration.convertNullToInt(custId);
                oCustdata = oPOSTrans.GetCustomerByID(oPOSTrans.oTransHRow.CustomerID);
                if (oCustdata.Tables[0].Rows.Count > 0)
                {
                    oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                    strCode = oCustRow.CustomerId.ToString();
                    EditCustomer(strCode, clsPOSDBConstants.Customer_tbl);
                    this.txtCustomer.Enabled = false;
                }
            }

        }

        //completed by sandeep
        private void MakeHouseCharge()
        {
            try
            {
                logger.Trace("MakeHouseCharge() - " + clsPOSDBConstants.Log_Entering);
                frmSearch oSearch = new frmSearch(clsPOSDBConstants.PrimeRX_HouseChargeInterface);
                oSearch.searchInConstructor = true;//Added By Shitaljit(QuicSolv) on 13 Sept 2011
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;
                    string AcctName = "";
                    Decimal Amount = 0;
                    DataTable ChargeAccount;
                    int PatientNo = 0;  //PRIMEPOS-2570 02-Jul-2020 JY Added
                    bool result = clsHouseCharge.GetReceiveOnAccount(strCode, out AcctName, out Amount, out ChargeAccount, 0, 0, ref PatientNo);
                    if (result == true)
                    {
                        /////Added By Rohit Nair on 01/17/2017 for PRIMEPOS-2368
                        //#region PrimePOS-2368 Getting Customer Info from CHarge Account info to be able to Generate Tokens
                        //CustomerData chargeCustomer = oPOSTrans.GetChargeCustomer(ChargeAccount);
                        //if (chargeCustomer != null && chargeCustomer.Tables.Count > 0 && chargeCustomer.Tables[0].Rows.Count > 0)
                        //{
                        //    oPOSTrans.oCustomerRow = chargeCustomer.Customer[0];
                        //    if (oPOSTrans.oCustomerRow != null)
                        //    {
                        //        this.txtCustomer.Text = oPOSTrans.oCustomerRow.AccountNumber.ToString();
                        //        this.lblCustomerName.Text = oPOSTrans.oCustomerRow.CustomerFullName;
                        //        ShowCustomerTokenImage(oPOSTrans.oCustomerRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                        //        this.txtCustomer.Tag = oPOSTrans.oCustomerRow.CustomerId;
                        //        tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                        //    }
                        //}
                        //#endregion

                        #region PRIMEPOS-2570 08-Jun-2020 JY Added
                        CustomerData chargeCustomer = new CustomerData();
                        if (PatientNo > 0)
                        {
                            chargeCustomer = oPOSTrans.GetChargeCustomer(PatientNo);

                            if (chargeCustomer == null || chargeCustomer.Tables.Count == 0 || chargeCustomer.Tables[0].Rows.Count == 0)
                            {
                                logger.Trace("MakeHouseCharge() - couldn't find POS customer against selected charge account");
                                if (Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2)
                                {
                                    logger.Trace("MakeHouseCharge() - copying the linked patient from primerx to pos database");
                                    DataSet oDS = null;
                                    PharmBL oPBL = new PharmBL();
                                    oPBL.GetPatient(PatientNo.ToString(), out oDS);
                                    if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                                    {
                                        Customer oCustomer = new Customer();
                                        CustomerData oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(oDS, false);
                                        if (oCustomerData != null)
                                        {
                                            oCustomer.Persist(oCustomerData, true);
                                            oPOSTrans.oCustomerRow = oCustomerData.Customer[0];
                                        }
                                    }
                                }
                                else
                                {
                                    Resources.Message.Display("couldn't find POS customer against selected charge account in POS database and logged in user don't have enough privileges", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            else
                            {
                                oPOSTrans.oCustomerRow = chargeCustomer.Customer[0];
                            }
                        }
                        else
                        {
                            if (Configuration.CSetting.ProceedROATransWithHCaccNotLinked)  //PRIMEPOS-2570 17-Aug-2020 JY Added
                            {
                                Resources.Message.Display("Selected HC account is not linked to a patient, so we cant proceed", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                //proceed with the transaction
                            }
                        }

                        if (oPOSTrans.oCustomerRow != null)
                        {
                            this.txtCustomer.Text = oPOSTrans.oCustomerRow.AccountNumber.ToString();
                            this.lblCustomerName.Text = oPOSTrans.oCustomerRow.CustomerFullName;
                            ShowCustomerTokenImage(oPOSTrans.oCustomerRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                            this.txtCustomer.Tag = oPOSTrans.oCustomerRow.CustomerId;
                            tempCustId = oPOSTrans.oCustomerRow.CustomerId;
                        }
                        #endregion

                        oPOSTrans.oTransHRow.TransactionStartDate = DateTime.Now;

                        #region PRIMEPOS-2434 06-May-2021 JY Added
                        POS_Core.TransType.POSTransactionType TransType = POS_Core.TransType.POSTransactionType.Sales;
                        Amount = Configuration.convertNullToDecimal(Amount.ToString("######0.00")); //PRIMEPOS-3385
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        {
                            Amount *= -1;
                            TransType = POS_Core.TransType.POSTransactionType.SalesReturn;
                        }
                        #endregion

                        frmPOSPayTypesList oPTList = new frmPOSPayTypesList(Amount, TransType, 0, true, this.sSigPadTransID, 0, lblCustomerName.Text, bIsCustomerTokenExists, oPOSTrans, false, 0); //PRIMEPOS-2611 13-Nov-2018 JY Added last parameter //  Added oPOSTrans for Solutran - PRIMEPOS-2663 - NileshJ // PRIMERX-7688 - NileshJ BatchDelviery added false and 0 23-Sept-2019   //PRIMEPOS-2434 06-May-2021 JY modified
                        oPTList.ChargeAccount = ChargeAccount;
                        oPTList.ParentForm = this;
                        //Added By Rohit Nair on 01/17/2017 for PRIMEPOS-2368
                        if (oPOSTrans.oCustomerRow != null)
                        {
                            oPTList.oPosPTList.oCurrentCustRow = oPOSTrans.oCustomerRow;
                        }
                        oPTList.oPosPTList.maxClCouponAmount = oPOSTrans.CalculateMaxCouponAmount(txtAmtDiscount.Text.ToString()); //09-Apr-2015 JY Added txtAmtDiscount.Text.ToString()
                        oPTList.ShowDialog(this);
                        if (oPTList.oPosPTList.CancelTransaction == true)
                        {
                            this.SetNew(true);
                            return;
                        }
                        if (oPTList.oPosPTList.oPOSTransPaymentDataPaid == null)
                        {
                            this.oPOSTrans.oPOSTransPaymentData = new POSTransPaymentData();
                            this.txtAmtBalanceDue.Text = "0";
                            this.txtAmtChangeDue.Text = "0";
                            this.ChangeDue = 0;
                            this.txtAmtTendered.Value = 0;
                            this.lblInvDiscount.Text = "0";
                            SetNew(true);
                        }
                        else
                        {
                            this.oPOSTrans.oPOSTransPaymentData = oPTList.oPosPTList.oPOSTransPaymentDataPaid;
                            // Added By Rohit Nair For Tokenisation
                            #region Save Token
                            oPOSTrans.SaveToken();
                            #endregion
                            try
                            {
                                Amount += oPTList.oPosPTList.TransFeeAmt;
                                SaveReceiveOnAccount(long.Parse(strCode.Trim()), Convert.ToDecimal(Amount), Convert.ToDecimal(oPTList.txtAmtPaid.Text), Convert.ToDecimal(oPTList.oPosPTList.ChangeDue), 0);
                            }
                            catch (Exception) { }
                        }

                        txtItemCode.Focus();
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Trace(exp, "MakeHouseCharge()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                SetNew(true);
            }
            finally
            {
                clsUIHelper.CurrentForm = this;
            }
            logger.Trace("MakeHouseCharge() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        private bool ShowCLCardInputScreen(bool inViewMode) //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added return type
        {
            logger.Trace("ShowCLCardInputScreen() - " + clsPOSDBConstants.Log_Entering);
            bool bStatus = true;
            if (oPOSTrans.oCustomerRow != null)
            {
                if (Configuration.CLoyaltyInfo.UseCustomerLoyalty == true)
                {
                    if (Configuration.CLoyaltyInfo.ShowCLCardInputOnTrans == true || inViewMode == true)
                    {
                        if (Configuration.CLoyaltyInfo.DisableAutoPointCalc == true && grdDetail.Rows.Count > 0)
                        {
                            inViewMode = false;
                        }
                        frmPOSTransCLCardInput ofrm = null;
                        //Added By shitaljit Auto Populate Card Details only if cuystomer has Active CL card.
                        CLCardsRow oCLRow = oPOSTrans.GetActiveCardForCustomerID(oPOSTrans.oCustomerRow.CustomerId);
                        //Configuration.CInfo.AutoPopulateCLCardDetails == true clause is added by shitaljit to handel this feature from Preference.
                        //13-Apr-2015 JY Added if condition to bypass CL screen for return trans
                        if (oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                        {
                            if (Configuration.convertNullToInt(txtCustomer.Text.ToString()) > 0)
                            {
                                try
                                {
                                    ofrm = new frmPOSTransCLCardInput(inViewMode, Configuration.convertNullToInt(txtCustomer.Text.ToString()));
                                    ofrm.Visible = false;
                                    ofrm.Hide();
                                    ofrm.ShowInTaskbar = true;
                                    ofrm.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                                    ofrm.Show();
                                    ofrm.Visible = false;
                                    inViewMode = false;
                                    object sender = null;
                                    System.EventArgs e = null;
                                    ofrm.btnClose_Click(sender, e);
                                }
                                catch
                                {
                                }
                            }
                        }
                        else if (oCLRow != null && Configuration.CInfo.AutoPopulateCLCardDetails == true && txtCustomer.Text.Trim().Length > 0 && Configuration.convertNullToInt(txtCustomer.Text.ToString()) > 0)
                        {
                            ofrm = new frmPOSTransCLCardInput(inViewMode, Configuration.convertNullToInt(txtCustomer.Text.ToString()));
                            DialogResult dr = ofrm.ShowDialog(this);    //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added DialogResult
                            if (dr.ToString().ToUpper().Trim() == "CANCEL") //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added if block
                            {
                                bStatus = false;
                            }
                        }
                        else
                        {
                            ofrm = new frmPOSTransCLCardInput(inViewMode);
                            DialogResult dr = ofrm.ShowDialog(this);    //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added DialogResult
                            if (dr.ToString().ToUpper().Trim() == "CANCEL") //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added if block
                            {
                                bStatus = false;
                            }
                        }

                        if (ofrm != null && ofrm.CLCardRow != null)
                        {
                            this.oCLCardRow = ofrm.CLCardRow;
                            txtCustomer.Tag = this.oCLCardRow.CustomerID;
                            EditCustomer(this.oCLCardRow.CustomerID.ToString(), clsPOSDBConstants.Customer_tbl);
                            oPOSTrans.oTransHRow.LoyaltyPoints = ofrm.CLTransPoints;
                            #region Sprint-18 - 2133 13-Nov-2014 JY Added to handle the scenario  - if user selected valid customer for sudafed and on CL screen enter the coupon whoes sudafed limit exceeded
                            if (inViewMode == false && oPOSTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                if (!Configuration.CInfo.useNplex)
                                    oPOSTrans.ValidateOTCItems(txtCustomer.Tag.ToString());
                            }
                            #endregion
                        }
                        else
                        {
                            this.oCLCardRow = null;
                            oPOSTrans.oTransHRow.LoyaltyPoints = 0;
                        }
                    }
                    oPOSTrans.CalculateLoyalityPoints();
                }
            }
            logger.Trace("ShowCLCardInputScreen() - " + clsPOSDBConstants.Log_Exiting);
            return bStatus;
        }

        #region PRIMEPOS-2611 13-Nov-2018 JY Added 
        private void ShowCustomerTokenImage(int CustomerId)
        {
            try
            {
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                if (oCCCustomerTokInfo.IsCustomerTokenExists(CustomerId))
                {
                    bIsCustomerTokenExists = true;
                    lblCustomerName.Appearance.Image = Properties.Resources.CreditCard;
                    tmrBlinking.Enabled = true;
                }
                else
                {
                    bIsCustomerTokenExists = false;
                    lblCustomerName.Appearance.Image = null;
                    tmrBlinking.Enabled = false;
                    lblCustomerName.Appearance.ForeColor = Color.White;
                    iBlinkCnt = 0;
                }
            }
            catch (Exception Ex)
            {
                logger.Trace(Ex, "ShowCustomerTokenImage(int CustomerId)");
            }
        }
        #endregion

        #endregion

        #region Customer Engagement Details PRIMEPOS-2794 SAJID DHUKKA
        private void GetCustomerDetails()
        {
            try
            {
                lblInsurance.Text = lblHouseChargeBal.Text = lblStoredCreditBal.Text = lblSex.Text = lblDOB.Text = lblPatientCounceling.Text = lblCustomerLoyalty.Text = lblCouponBalance.Text = lblStoredCC.Text = lblLastVisited.Text = "N/A";
                int customerId = 0;
                if (this.txtCustomer.Tag != null)
                {
                    customerId = Configuration.convertNullToInt(this.txtCustomer.Tag.ToString());
                }
                logger.Trace("GetCustomerDetails() - enter for Customer Engagement with customerID: " + customerId);
                //this.oPOSTrans.oCustomerRow (this value we get from GetCustomer(customerId.ToString(), true));
                if (this.oPOSTrans != null && this.oPOSTrans.oCustomerRow != null)
                {
                    string tempDate = Convert.ToString(this.oPOSTrans.oCustomerRow.DateOfBirth);
                    if (!string.IsNullOrEmpty(tempDate))
                    {
                        lblDOB.Text = DateTime.Parse(tempDate).ToShortDateString();
                    }
                    lblSex.Text = (int)PrimePOSEnum.Gender.F == this.oPOSTrans.oCustomerRow.Gender ? PrimePOSEnum.Gender.F.ToString() : PrimePOSEnum.Gender.M.ToString();
                    POSPayTypeList oPosPTList = new POSPayTypeList();
                    StoreCredit oStoreCredit = new StoreCredit();
                    oPosPTList.CustomerID = customerId;
                    string sHousechargeAccountNo = oPosPTList.GetHouseChargeAccount();
                    logger.Trace("GetCustomerDetails() - oPosPTList.GetHouseChargeAccount Get House Charge Account with customerID: " + customerId);
                    if (sHousechargeAccountNo != "" && sHousechargeAccountNo != "0")
                    {
                        MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                        DataSet oDS = new DataSet();
                        //oAcct.GetAccountByCode(sHousechargeAccountNo, out oDS);
                        oAcct.GetAccountByCode(sHousechargeAccountNo, out oDS, true);   //PRIMEPOS-2888 28-Aug-2020 JY Added third parameter as "true" to get the exact HouseCharge record
                        if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                        {
                            decimal balance = 0;
                            decimal.TryParse(oDS.Tables[0].Rows[0]["Balance"].ToString(), out balance);
                            lblHouseChargeBal.Text = balance.ToString("c");
                        }
                        logger.Trace("GetCustomerDetails() - oPosPTList.GetHouseChargeAccount Exit House Charge Account with customerID: " + customerId);
                        oDS = null;
                    }

                    CLCardsData oCLData = new CLCardsData();
                    CLCards oCLCards = new CLCards();
                    logger.Trace("GetCustomerDetails() - oCLCards.GetByCustomerID Enter customerID: " + customerId);
                    oCLData = oCLCards.GetByCustomerID(customerId);
                    if (oCLData.CLCards.Rows.Count > 0)
                    {
                        string commaSeparetedCardId = string.Join(",", oCLData.CLCards.AsEnumerable().Select(a => a.Field<Int64>("CardId")).ToArray());
                        CLCoupons coupons = new CLCoupons();
                        CLCouponsData oCLCouponData = new CLCouponsData();
                        logger.Trace("GetCustomerDetails() - coupons.GetByCLCardID Enter CardId: " + commaSeparetedCardId);
                        oCLCouponData = coupons.GetByCLCardID(commaSeparetedCardId);
                        lblCustomerLoyalty.Text = "Y";
                        if (oCLCouponData?.Tables?.Count > 0 && oCLCouponData?.Tables[0]?.Rows?.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = oCLCouponData.Tables[0];
                            decimal couponvalue = dt.AsEnumerable().Select(s => s.Field<decimal>("CouponValue")).Sum();
                            lblCouponBalance.Text = couponvalue.ToString("c");
                        }
                        else
                        {
                            lblCouponBalance.Text = Convert.ToDecimal("0").ToString("c");
                        }
                    }
                    else
                    {
                        lblCustomerLoyalty.Text = "N";
                    }

                    PharmBL oPBL = new PharmBL();
                    DataTable otable = new DataTable();
                    try //PRIMEPOS-3106 13-Jul-2022 JY Added try catch block
                    {
                        if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
                        {
                            logger.Trace("GetCustomerDetails() - oPBL.GetLastPatientConsent Enter PatientNo with customerID: " + customerId);
                            otable = oPBL.GetLastPatientConsent(this.oPOSTrans.oCustomerRow.PatientNo);
                            if (otable != null && otable.Rows.Count > 0)
                            {
                                lblPatientCounceling.Text = String.Concat(otable.Rows[0]["COUNSELINGREQ"]?.ToString() + "      ", otable.Rows[0]["TransDate"].ToString());
                            }
                            else
                            {
                                lblPatientCounceling.Text = "N";
                            }

                            otable = oPBL.GetAllPatInsurance(this.oPOSTrans.oCustomerRow.PatientNo.ToString());
                            //SAJID PRIMEPOS-2794
                            logger.Trace("GetCustomerDetails() - oPBL.GetPatient Enter PATTYPE with customerID: " + customerId);
                            string insurance = string.Empty;
                            if (otable?.Rows?.Count > 0)
                            {
                                //lblInsurance.Text = string.Join(",", otable.AsEnumerable().Select(s => s.Field<string>("INS_CD").Trim()).ToArray());  //PRIMEPOS-3033 01-Dec-2021 JY Commented
                                lblInsurance.Text = string.Join(",", otable.AsEnumerable().Where(a => a.Field<string>("INS_CD") != null).Select(s => s.Field<string>("INS_CD").Trim()).ToArray());    //PRIMEPOS-3033 01-Dec-2021 JY Added
                            }
                            otable = oPBL.GetPatient(this.oPOSTrans.oCustomerRow.PatientNo.ToString());
                            logger.Trace("GetCustomerDetails() - oPBL.GetPatient Enter PATTYPE with customerID: " + customerId);
                            if (otable != null && otable.Rows.Count > 0)
                            {
                                lblInsurance.Text = string.Concat(otable.Rows[0]["PATTYPE"].ToString().Trim(), ",", lblInsurance.Text);
                            }
                        }
                    }
                    catch
                    {
                        lblPatientCounceling.Text = "N/A";
                        lblInsurance.Text = "N/A";
                    }

                    DataSet dsStoreDetails = new DataSet();
                    dsStoreDetails = oStoreCredit.GetByCustomerID(customerId);
                    logger.Trace("GetCustomerDetails() - oStoreCredit.GetByCustomerID Enter CreditAmount with customerID: " + customerId);
                    if (dsStoreDetails != null && dsStoreDetails.Tables.Count > 0 && dsStoreDetails.Tables[0].Rows.Count > 0)
                    {
                        decimal balance = 0;
                        decimal.TryParse(dsStoreDetails.Tables[0].Rows[0]["CreditAmt"].ToString(), out balance);
                        lblStoredCreditBal.Text = balance.ToString("c");
                    }
                    Customer oBRCustomer = new Customer();
                    DataSet dsPaymentProcess = new DataSet();
                    dsPaymentProcess = oBRCustomer.GetTokenByCustomerID(customerId);
                    logger.Trace("GetCustomerDetails() - oBRCustomer.GetTokenByCustomerID Enter Processor with customerID: " + customerId);
                    if (dsPaymentProcess != null && dsPaymentProcess.Tables.Count > 0 && dsPaymentProcess.Tables[0].Rows.Count > 0)
                    {
                        lblStoredCC.Text = "Y";
                    }
                    else
                        lblStoredCC.Text = "N";
                    CalculateStarRating();
                    logger.Trace("GetCustomerDetails() - posTrans.calculateStarRating Enter lblStarRating with customerID: " + customerId);
                    string transDate = posTrans.GetLastTransactionByCustId(customerId);
                    logger.Trace("GetCustomerDetails() - posTrans.GetLastTransactionByCustId Enter TransDate with customerID: " + customerId);
                    lblLastVisited.Text = transDate;
                    logger.Trace("GetCustomerDetails() - Exit Customer Engagement with customerID: " + customerId);
                    dsStoreDetails = dsPaymentProcess = null; otable = null;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetCustomerDetails()");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        //PRIMEPOS-2794 SAJID DHUKKA
        private void CalculateStarRating()
        {
            int starcount = posTrans.CalculateStarRating(this.oPOSTrans.oCustomerRow.PatientNo);
            lblStarRating.Show();
            lblStarRating.Height = 18;
            switch (starcount)
            {
                case 1:
                    lblStarRating.Width = 1 * 18;
                    lblStarRating.StarCount = 1;
                    break;
                case 2:
                    lblStarRating.Width = 2 * 23;
                    lblStarRating.StarCount = 2;
                    break;
                case 3:
                    lblStarRating.Width = 3 * 28;
                    lblStarRating.StarCount = 3;
                    break;
                case 4:
                    lblStarRating.Width = 4 * 33;
                    lblStarRating.StarCount = 4;
                    break;
                case 5:
                    lblStarRating.Width = 5 * 38;
                    lblStarRating.StarCount = 5;
                    break;
                default:
                    lblStarRating.Hide();
                    break;
            }
        }
        #endregion

        #region PRIMEPOS-3157 28-Nov-2022 JY Added
        private void GetCustomerDocuments()
        {
            CancellationToken _cancelToken;
            Task.Factory.StartNew(() => GetCustDocs(), _cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void GetCustDocs()
        {
            try
            {
                loadedImages.Clear();
                pbImage.Image = null;

                if (Configuration.CPOSSet.UsePrimeRX && Configuration.CSetting.PatientsSubCategories.Trim() != "")
                {
                    if (oPOSTrans.oCustomerRow.PatientNo != 0)
                    {
                        PharmBL oPharmBL = new PharmBL();
                        DataTable dt = oPharmBL.GetDocuments(oPOSTrans.oCustomerRow.PatientNo, (int)Category.PATIENT, Configuration.CSetting.PatientsSubCategories);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                try
                                {
                                    string docExt = Configuration.convertNullToString(dt.Rows[i]["DocTypeExt"]).Trim();
                                    if (string.IsNullOrWhiteSpace(docExt) || !docExt.ToLower().Contains("pdf"))
                                    {
                                        Byte[] imageBytes = Configuration.GetImage(Configuration.convertNullToString(dt.Rows[i]["DocumentId"]).Trim());
                                        MemoryStream ms = new MemoryStream(imageBytes);
                                        loadedImages.Add(Image.FromStream(ms));
                                        if (loadedImages.Count > 0)
                                        {
                                            currentImageIndex = 0;
                                            pbImage.Image = loadedImages[currentImageIndex];
                                        }
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    logger.Fatal(Ex, "GetCustomerDocuments() - 0");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetCustomerDocuments() - 1");
            }
        }
        #endregion
    }
}