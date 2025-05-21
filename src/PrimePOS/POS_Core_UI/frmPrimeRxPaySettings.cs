using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using POS_Core.Resources;
//using System.ServiceProcess;
using System.Management;
using System.IO;
using POS_Core.BusinessRules;
using NLog;
using POS_Core.CommonData;

namespace POS_Core_UI
{
    public partial class frmPrimeRxPaySettings : Form
    {
        #region local variables
        Prefrences oPrefrences = new Prefrences();
        public SettingDetail oSetting = new SettingDetail();
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private const string OnlinePayment = "OnlinePayment";
        public bool isMobileDefault = true;
        public string LinkExpiry = string.Empty;
        public string OnlineOption;
        public bool IsClose = false;
        private bool IsClosed = false;
        #endregion

        public frmPrimeRxPaySettings()
        {
            InitializeComponent();            
        }

        private void frmPrimeRxPaySettings_Load(object sender, EventArgs e)
        {
            try
            {
                //if (oSetting.IsDefaultMobile)
                //{
                //    rdMobile.Checked = true;
                //}
                //else
                //{
                //    rdEmail.Checked = true;
                //}
                txtExpiryInMints.Text = oSetting.LinkExpriyInMinutes;

                cmbOnlineOption.DataSource = Enum.GetValues(typeof(Configuration.OnlinePayment))//2915
                       .Cast<Enum>()
                       .Select(value => new
                       {
                           (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                           value
                       })
                       .OrderBy(item => item.value)
                       .ToList();

                cmbOnlineOption.Value = oSetting.OnlineOption?.ToString();

                //= 

                clsUIHelper.setColorSchecme(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                logger.Error(exp, "frmPrimeRxPaySettings==>frmPrimeRxPaySettings_Load(): An Error Occured");
            }
        }
      


        
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                //if(!rdMobile.Checked && !rdEmail.Checked)
                //{
                //    clsUIHelper.ShowErrorMsg("At least select one option");
                //    return;
                //}
                //else if (string.IsNullOrWhiteSpace(txtExpiryInMints.Text))
                //{
                //    clsUIHelper.ShowErrorMsg("Enter Link Expiry");
                //    return;
                //}

                if (string.IsNullOrWhiteSpace(Convert.ToString(cmbOnlineOption.Value)))//PRIMEPOS-3057
                {
                    clsUIHelper.ShowErrorMsg("Please select one option.");
                    cmbOnlineOption.Focus();
                    return;
                }

                //this.oSetting.IsDefaultMobile = this.isMobileDefault = rdMobile.Checked;
                this.oSetting.LinkExpriyInMinutes = this.LinkExpiry = txtExpiryInMints.Text;

                //Enum.GetName(typeof(Configuration.OnlinePayment), cmbOnlineOption.Value.ToString());
                this.oSetting.OnlineOption = this.OnlineOption = cmbOnlineOption.Value?.ToString();
                IsClosed = true;
                this.Close();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                logger.Error(exp, "frmPrimeRxPaySettings==>btnSaveConfig_Click(): An Error Occured");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.IsClose = true;
            this.Close();
        }

        private void frmPrimeRxPaySettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!IsClosed)
            {
                this.IsClose = true; //PRIMEPOS-3227
            }
            else
            {
                this.IsClose = false; //PRIMEPOS-3227
            }
            this.Close();
        }
    }    
}
