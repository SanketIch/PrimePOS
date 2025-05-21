using NLog;
using POS_Core.BusinessRules;
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

namespace POS_Core_UI
{
    public partial class frmHpspaxSettings : Form
    {
        Prefrences oPrefrences = new Prefrences();
        public SettingDetail oSetting = new SettingDetail();
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private const string OnlinePayment = "OnlinePayment";
        public bool isMobileDefault = true;
        public string SiteID = string.Empty;
        public string LicenseID;
        public string DeviceID = string.Empty;
        public string DeveloperID = string.Empty;
        public string username = string.Empty;
        public string Password;
        public string Version;
        public bool IsClose = false;

        public frmHpspaxSettings()
        {
            InitializeComponent();
        }

        private void frmHpspaxSettings_Load(object sender, EventArgs e)
        {
            txtSiteID.Text = oSetting.SiteId;
            txtLicenseID.Text = oSetting.LicenseId;
            txtUsername.Text = oSetting.Username;
            txtDeviceID.Text = oSetting.DeviceId;
            txtPassword.Text = oSetting.Password;
            txtDeveloperID.Text = oSetting.DeveloperId;
            txtVersion.Text = oSetting.VersionNumber;

            clsUIHelper.setColorSchecme(this);
        }

        private void frmHpspaxSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            this.oSetting.SiteId = this.SiteID = txtSiteID.Text;
            this.oSetting.LicenseId = this.LicenseID = txtLicenseID.Text;
            this.oSetting.DeveloperId = this.DeveloperID = txtDeveloperID.Text;
            this.oSetting.DeviceId = this.DeviceID = txtDeviceID.Text;
            this.oSetting.Username = this.username = txtUsername.Text;
            this.oSetting.Password = this.Password = txtPassword.Text;
            this.oSetting.VersionNumber = this.Version = txtVersion.Text;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.IsClose = true;
            this.Close();
        }
    }
}
