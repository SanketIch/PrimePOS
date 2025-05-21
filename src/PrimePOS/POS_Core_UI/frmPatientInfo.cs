
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using MMSChargeAccount;
using POS_Core.Resources;
using NLog;

namespace POS_Core_UI
{
    public partial class frmPatientInfo : Form
    {
        DataTable dtPatInfo;
        public bool bInputDOB = false;  //PRIMEPOS-2317 18-Mar-2019 JY Added
        #region PRIMEPOS-3065 10-Mar-2022 JY Added
        public string strDriversLicense = string.Empty;
        public DateTime DriversLicenseExpDate = DateTime.MinValue;
        private string data;
        private bool isScan = false;
        public POS_Core.Business_Tier.DL ID = null;
        private delegate void ScanEventHandler();
        private event ScanEventHandler AfterScanData;
        private event ScanEventHandler ScanData;    //PRIMEPOS-3162 18-Nov-2022 JY Added

        private int FormWithoutInputDOBSize = 375;   //385
        private int FormOriginalSize = 454;   //390
        private int FormMatchDOBSize = 190;
        public bool bIsPatient = false;
        #endregion
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmPatientInfo(DataTable dtPInfo)
        {
            InitializeComponent();
            dtPatInfo = dtPInfo;
        }

        private void frmPatientInfo_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.dtpInputPatDOB.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpInputPatDOB.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #region PRIMEPOS-3065 10-Mar-2022 JY Added
            this.txtDriversLicense.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDriversLicense.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.dtpDriversLicenseExpDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpDriversLicenseExpDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #endregion

            SetPatientInfo();

            ScanData += frmPatientInfo_ScanData;    //PRIMEPOS-3162 18-Nov-2022 JY Added
            ScanData.Invoke();  //PRIMEPOS-3162 18-Nov-2022 JY Added
        }

        //PRIMEPOS-3162 18-Nov-2022 JY Added
        void frmPatientInfo_ScanData()
        {
            txtDriversLicense.TextChanged -= new EventHandler(txtDriversLicense_TextChanged);
            txtDriversLicense.TextChanged += new EventHandler(txtDriversLicense_TextChanged);
            txtDriversLicense.Leave += new EventHandler(txtDriversLicense_LostFocus);
            txtDriversLicense.Leave += new EventHandler(txtDriversLicense_LostFocus);
        }

        private void SetPatientInfo()
        {
            try
            {
                //Code is modified by shitaljit 0n 18Apr2013 to avoid null exception condition
                //PRIMEPOS JIRA- 801
                if (dtPatInfo != null && dtPatInfo.Rows.Count > 0)
                {
                    try
                    {
                        //dtpPatDOB.Value = string.IsNullOrEmpty(Configuration.convertNullToString(dtPatInfo.Rows[0]["DOB"].ToString().Trim())) ? Configuration.MinimumDate : Convert.ToDateTime(dtPatInfo.Rows[0]["DOB"].ToString().Trim());
                        //Sprint-26 - PRIMEPOS-2424 16-Aug-2017 JY Added logic to display null date
                        if (string.IsNullOrEmpty(Configuration.convertNullToString(dtPatInfo.Rows[0]["DOB"].ToString().Trim())))
                            dtpPatDOB.Value = "";
                        else
                            dtpPatDOB.Value = Convert.ToDateTime(dtPatInfo.Rows[0]["DOB"].ToString().Trim());
                    }
                    catch
                    {
                        dtpPatDOB.Value = "";
                    }
                    //End of added by Shitaljit.

                    txtPatName.Text = dtPatInfo.Rows[0]["LName"].ToString().Trim() + ", " + dtPatInfo.Rows[0]["FName"].ToString().Trim();
                    txtPatAddress.Text = dtPatInfo.Rows[0]["ADDRSTR"].ToString().Trim() + ", " + dtPatInfo.Rows[0]["ADDRCT"].ToString().Trim() + ", " + dtPatInfo.Rows[0]["ADDRST"].ToString().Trim();
                    txtPatGender.Text = dtPatInfo.Rows[0]["Sex"].ToString().Trim() == "M" ? "Male" : "Female";
                    // dtpPatDOB.Value = Convert.ToDateTime(dtPatInfo.Rows[0]["DOB"].ToString().Trim());
                    txtPatPhone.Text = dtPatInfo.Rows[0]["Phone"].ToString().Trim();

                    #region Sprint-26 - PRIMEPOS-2424 16-Aug-2017 JY Added
                    int AcctNo = Configuration.convertNullToInt(dtPatInfo.Rows[0]["acct_no"]);
                    if (AcctNo > 0)
                    {
                        ContAccount oAcct = new ContAccount();
                        txtBalance.Text = oAcct.GetAccountBalance(AcctNo).ToString("c");
                    }
                    else
                        txtBalance.Text = String.Empty;
                    #endregion

                    #region PRIMEPOS-3065 10-Mar-2022 JY Added
                    txtDriversLicense.Text = Configuration.convertNullToString(dtPatInfo.Rows[0]["DriversLicense"]).Trim();
                    try
                    {
                        if (string.IsNullOrEmpty(Configuration.convertNullToString(dtPatInfo.Rows[0]["DriversLicenseExpDT"]).Trim()))
                            dtpDriversLicenseExpDate.Text = "";
                        else
                            dtpDriversLicenseExpDate.Value = Convert.ToDateTime(dtPatInfo.Rows[0]["DriversLicenseExpDT"].ToString().Trim());
                    }
                    catch
                    {
                        dtpDriversLicenseExpDate.Text = "";
                    }
                    #endregion
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #region PRIMEPOS-2317 18-Mar-2019 JY Added
        private void btnOk_Click(object sender, EventArgs e)
        {
            #region PRIMEPOS-3065 10-Mar-2022 JY Added
            strDriversLicense = this.txtDriversLicense.Text.Trim();
            try
            {
                if (!string.IsNullOrEmpty(dtpDriversLicenseExpDate.Text))
                    DriversLicenseExpDate = Convert.ToDateTime(dtpDriversLicenseExpDate.Value);
            }
            catch
            {
            }
            #endregion
            if (bInputDOB && Configuration.CInfo.RestrictIfDOBMismatch == true)
            {
                try
                {
                    if (ValidateDOB() == false)
                    {
                        this.DialogResult = DialogResult.None;
                        return;
                    }

                    if (Convert.ToDateTime(dtpPatDOB.Value).Date.CompareTo(Convert.ToDateTime(dtpInputPatDOB.Value).Date) != 0)
                    {
                        POS_Core_UI.Resources.Message.Display(this, "Entered DOB does not match record on file, please try again...", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.None;
                        if (dtpInputPatDOB.Enabled) dtpInputPatDOB.Focus();
                    }
                }
                catch (Exception Ex)
                {

                }
            }
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            if (ValidateDOB() == false) return;

            if (Convert.ToDateTime(dtpPatDOB.Value).Date.CompareTo(Convert.ToDateTime(dtpInputPatDOB.Value).Date) != 0)
            {
                if (Configuration.CInfo.RestrictIfDOBMismatch == true)
                {
                    POS_Core_UI.Resources.Message.Display(this, "Entered DOB does not match record on file, please try again...", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dtpInputPatDOB.Enabled) dtpInputPatDOB.Focus();
                }
                else
                {
                    DialogResult dr = POS_Core_UI.Resources.Message.Display(this, "Entered DOB does not match record on file. Press Ok to re-enter the DOB or Press continue to proceed with the transaction anyway.", "PrimePOS", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (dr == DialogResult.Yes || dr == DialogResult.None)
                    {
                        if (dtpInputPatDOB.Enabled) dtpInputPatDOB.Focus();
                    }
                    else
                    {
                        if (Configuration.CInfo.ShowPatientsData)
                        {
                            lblMatch.Text = "Entered DOB mis-matched";
                            this.Height = FormWithoutInputDOBSize;
                            pnlPatientData.Visible = true;
                            pnlLicense.Visible = true;
                            if (btnCancel.Enabled && btnCancel.Visible)
                                btnCancel.Focus();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
            else
            {
                lblMatch.Text = "DOB matched successfully";
                if (Configuration.CInfo.ShowPatientsData)
                {
                    this.Height = FormOriginalSize-10;
                    pnlPatientData.Visible = true;
                    if (!bIsPatient)
                    {
                        pnlLicense.Visible = false;
                        this.Height -= 70;
                    }
                    else
                    {
                        pnlLicense.Visible = true;
                        //txtDriversLicense.Focus();
                    }
                }
                if (btnOk.Enabled && btnOk.Visible)
                    btnOk.Focus();
                if (btnCancel.Enabled && btnCancel.Visible)
                    btnCancel.Focus();
            }
        }

        private void dtpInputPatDOB_Leave(object sender, EventArgs e)
        {
            //ValidateDOB();
        }

        private bool ValidateDOB()
        {
            if (ValidateDOB(dtpInputPatDOB.Value.ToString()) == false)
            {
                POS_Core_UI.Resources.Message.Display(this, "Please select a valid date", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtpInputPatDOB.Focus();
                return false;
            }
            else
                return true;
        }

        private bool ValidateDOB(string Date)
        {
            DateTime dDate = new DateTime();
            if (!DateTime.TryParse(Date, out dDate))
                return false;
            if (Convert.ToDateTime(Date) < Convert.ToDateTime("1/1/1753"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        #endregion

        private void frmPatientInfo_Shown(object sender, EventArgs e)
        {
            if (bInputDOB)
            {
                if (dtpPatDOB.Value.ToString() == "")
                {
                    POS_Core_UI.Resources.Message.Display(this, "We couldn't ask Patients DOB as it is blank.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pnlInputDOB.Visible = false;
                    this.Height = FormWithoutInputDOBSize;
                    if (!bIsPatient)
                    {
                        pnlLicense.Visible = false;
                        this.Height -= 70;
                    }
                    else
                        pnlLicense.Visible = true;
                }
                else
                {
                    pnlInputDOB.Visible = true;
                    this.Text = "Patient Information - Input DOB";
                    this.lblTransactionType.Text = this.Text;

                    if (!Configuration.CInfo.ShowPatientsData || Configuration.CInfo.RestrictIfDOBMismatch || (Configuration.CInfo.ShowPatientsData && !Configuration.CInfo.RestrictIfDOBMismatch))
                    {
                        this.Height = FormMatchDOBSize;
                        pnlPatientData.Visible = false;
                        pnlLicense.Visible = false;
                    }
                    else
                    {
                        this.Height = FormOriginalSize;
                        pnlPatientData.Visible = true;
                        pnlLicense.Visible = true;
                    }

                    if (Configuration.CInfo.RestrictIfDOBMismatch)
                    {
                        btnOk.Visible = true;
                        btnCancel.Visible = false;
                    }
                    else
                    {
                        btnOk.Visible = false;
                        btnCancel.Visible = true;
                    }

                    if (dtpInputPatDOB.Enabled) dtpInputPatDOB.Focus();
                }
            }
            else
            {
                pnlInputDOB.Visible = false;
                this.Height = FormWithoutInputDOBSize;
                if (!bIsPatient)
                {
                    pnlLicense.Visible = false;
                    this.Height -= 70;
                }
                else
                    pnlLicense.Visible = true;
            }
        }


        private void frmPatientInfo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region PRIMEPOS-3065 10-Mar-2022 JY Added
        private void txtDriversLicense_TextChanged(object sender, EventArgs e)
        {
            data = txtDriversLicense.Text;
            if (data.StartsWith("@") && !isScan)
            {
                isScan = true;
                txtDriversLicense.Multiline = true;
                txtDriversLicense.PasswordChar = 'X';
                //try
                //{
                //    txtDriversLicense_LostFocus(sender, e);
                //}
                //catch { }
            }
        }

        private void txtDriversLicense_LostFocus(object sender, EventArgs e)
        {
            if (isScan && !string.IsNullOrEmpty(data))
            {
                logger.Trace("txtDriversLicense_LostFocus(object sender, EventArgs e) - " + data); //PRIMEPOS-3162 18-Nov-2022 JY Added
                if (ID != null)
                {
                    ID = null;
                }
                ID = new POS_Core.Business_Tier.DL(data);
                isScan = false;
                if (!string.IsNullOrEmpty(ID.DAQ))
                {
                    AfterScanData -= txtDriversLicense_AfterScanData;
                    AfterScanData += txtDriversLicense_AfterScanData;
                    AfterScanData.Invoke();
                }
            }
        }

        private void txtDriversLicense_AfterScanData()
        {
            if (!string.IsNullOrEmpty(ID.DAQ))
            {
                txtDriversLicense.Text = string.Empty;
                txtDriversLicense.Multiline = false;
                txtDriversLicense.PasswordChar = '\0';

                txtDriversLicense.Text = ID.DAQ; // Lic#
                Application.DoEvents();
                try
                {
                    String dateString = Configuration.convertNullToString(ID.DBA); //Get the Driver License Expiration Date
                    logger.Trace("txtDriversLicense_AfterScanData() - " + dateString); //PRIMEPOS-3162 18-Nov-2022 JY Added
                    String format = "MMddyyyy";
                    DateTime result = DateTime.MinValue;
                    bool bError = false;
                    try
                    {
                        result = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception Ex)
                    {
                        bError = true;
                        logger.Fatal(Ex, "txtDriversLicense_AfterScanData() - 1");    //PRIMEPOS-3162 18-Nov-2022 JY Added
                    }
                    if (!bError)
                    {
                        if (result.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                            dtpDriversLicenseExpDate.Value = result.ToShortDateString();
                        else
                        {
                            dtpDriversLicenseExpDate.Text = "";
                            try
                            {
                                dtpDriversLicenseExpDate.Text = ID.DBA.Substring(0, 2) + "/" + ID.DBA.Substring(2, 2) + "/" + ID.DBA.Substring(4, 4);
                            }
                            catch (Exception Ex)
                            {
                                logger.Fatal(Ex, "txtDriversLicense_AfterScanData() - 2");    //PRIMEPOS-3162 18-Nov-2022 JY Added
                            }
                        }
                    }
                }
                catch { }
            }
        }
        #endregion
    }
}