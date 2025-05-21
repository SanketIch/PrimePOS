using NLog;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.Resources;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmCustomerDetailsPrimeRxPay : Form
    {
        public bool IsMobile = false;
        public bool IsEmail = false;
        public string Email = "";
        public string Mobile = "";
        public string Name = "";
        public string DOB = "";
        public bool IsPrimeRxPayLinkSend = false;//PRIMEPOS-3248
        public bool IsCustomerDriven = false;
        public bool IsCancel = false;
        public bool IsOnHoldScreen = true;
        public bool IsSelectedCustomer = false;
        public CustomerData oCustData = null;
        public string CustomerCode = string.Empty;
        public string PosTranspaymentEmail = string.Empty;
        public string PosTranspaymentMobile = string.Empty;
        public string PosTranspaymentTransProcessMode = string.Empty;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmCustomerDetailsPrimeRxPay(bool IsfrmOnHoldScreen = false)
        {
            InitializeComponent();
            lblSelectCustomer.Appearance.ForeColor = System.Drawing.Color.Red;
            this.IsOnHoldScreen = IsfrmOnHoldScreen;
            clsUIHelper.setColorSchecme(this);
        }
        private void CustomerDetailsPrimeRxPay_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                        {
                            btnSearch_Click(null, null);
                        }
                        break;
                    case Keys.Escape:
                        btnCancel_Click(null, null);
                        break;
                    default:
                        if (e.Alt == true)
                        {
                            if (e.KeyCode == Keys.E)
                                btnEmail_Click(null, null);
                            if (e.KeyCode == Keys.M)
                                btnSMS_Click(null, null);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("In btnSearch_Click() Searching Customer");
                frmSearchMain oSearch = new frmSearchMain(txtSearch.Text, true, true, clsPOSDBConstants.Customer_tbl);
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    GetCustomer(strCode);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in btnSearch_Click");
            }
        }

        private void GetCustomer(string CustCode)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(CustCode))
                {
                    if (CustCode.ToUpper() == "16")
                    {
                        Resources.Message.Display("Cannot select the default customer");
                        return;
                    }

                    CustomerData oCustdata = new CustomerData();
                    POSTransaction oPOSTrans = new POSTransaction();
                    oCustdata = oPOSTrans.GetCustomerByID(Configuration.convertNullToInt(CustCode));

                    if (oCustdata?.Tables.Count > 0 && oCustdata?.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = Convert.ToString(oCustdata.Tables[0].Rows[0]["CustomerName"]) + "," + Convert.ToString(oCustdata.Tables[0].Rows[0]["FirstName"]);
                        if (!this.IsOnHoldScreen)
                        {
                            txtEmail.Text = Convert.ToString(oCustdata.Tables[0].Rows[0]["EmailAddress"]);
                            txtPhone.Text = Convert.ToString(oCustdata.Tables[0].Rows[0]["CellNo"]);
                        }
                        else
                        {
                            txtEmail.Text = this.PosTranspaymentEmail;
                            txtPhone.Text = this.PosTranspaymentMobile;
                        }
                        dtDOB.Value = Convert.ToDateTime(Convert.ToString(oCustdata.Tables[0].Rows[0]["DateOfBirth"]));
                    }
                    this.oCustData = oCustdata;
                    lblSelectCustomer.Visible = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in CustomerCode " + ex);
            }
        }

        private bool CustomerValidation()
        {
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (txtPhone.Text.Length != 10)
                {
                    Resources.Message.Display("Phone number must be 10 digit");
                    logger.Trace("Phone number must be 10 digit");
                    return false;
                }
            }
            else
            {
                Resources.Message.Display("Please enter your mobile number");
                logger.Trace("Please enter your mobile number");
                return false;
            }

            IsCustomerDriven = true;
            Email = txtEmail.Text;
            Name = txtName.Text;
            DOB = dtDOB.Text;
            Mobile = txtPhone.Text;
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsCustomerDriven = false;
            this.IsCancel = true;
            this.Close();
        }

        private void rdEmail_CheckedChanged(object sender, EventArgs e)
        {
            IsEmail = true;
            IsMobile = false;
        }

        private void rdMobile_CheckedChanged(object sender, EventArgs e)
        {
            IsEmail = false;
            IsMobile = true;
        }

        private void frmCustomerDetailsPrimeRxPay_Load(object sender, EventArgs e)
        {
            if (this.IsSelectedCustomer)
            {
                btnSearch.Visible = false;
                txtSearch.Visible = false;
                lblSelectCustomer.Visible = false;
                lblEnter.Visible = false;
                GetCustomer(CustomerCode);
            }
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (CustomerValidation())
                {
                    Regex regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                    if (string.IsNullOrWhiteSpace(txtEmail.Text) && !regex.IsMatch(txtEmail.Text))
                    {
                        Resources.Message.Display("Enter a valid email address");
                        logger.Trace("Enter a valid email address");
                        return;
                    }
                    if (this.oCustData == null)
                    {
                        clsUIHelper.ShowErrorMsg("Please select a customer");
                        return;
                    }
                    IsEmail = true;
                    IsMobile = false;
                    IsPrimeRxPayLinkSend = true; //PRIMEPOS-3248
                    this.Close();
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void btnSMS_Click(object sender, EventArgs e)
        {
            try
            {
                if (CustomerValidation())
                {
                    if(this.oCustData == null)
                    {
                        clsUIHelper.ShowErrorMsg("Please select a customer");
                        return;
                    }
                    IsMobile = true;
                    IsEmail = false;
                    IsPrimeRxPayLinkSend = true; //PRIMEPOS-3248
                    this.Close();
                }
                else
                    return;
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
