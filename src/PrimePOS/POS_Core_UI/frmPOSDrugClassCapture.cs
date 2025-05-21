using System;
using System.Data;
using System.Windows.Forms;
using NLog;

namespace POS_Core_UI
{
    public partial class frmPOSDrugClassCapture : Form
    {
        private POS_Core.Business_Tier.POSDrugClassCapture ucontrol;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public bool IsCancelled
        {
            get { return ucontrol.isCancelled; }
        }

        private string _PatientNo;

        public string PatientNo
        {
            get { return _PatientNo; }
        }

        public DataTable TblPatient { get; set; }

        #region PRIMEPOS-3065 10-Mar-2022 JY Added
        public Boolean _bIsPatient = false;
        public string _strDriversLicense = string.Empty;
        public DateTime _DriversLicenseExpDate = DateTime.MinValue;
        #endregion

        public frmPOSDrugClassCapture(string PatientNo, Boolean bIsPatient, string strDriversLicense, DateTime DriversLicenseExpDate)    //PRIMEPOS-3065 10-Mar-2022 JY Added strDriversLicense, DriversLicenseExpDate
        {
            InitializeComponent();
            this._PatientNo = PatientNo;
            #region PRIMEPOS-3065 10-Mar-2022 JY Added
            _bIsPatient = bIsPatient;
            _strDriversLicense = strDriversLicense;
            _DriversLicenseExpDate = DriversLicenseExpDate;
            #endregion
        }

        private void frmPOSDrugClassCapture_Load(object sender, EventArgs e)
        {
            logger.Trace("frmPOSDrugClassCapture_Load(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
            clsUIHelper.setColorSchecme(this);
            ucontrol = new POS_Core.Business_Tier.POSDrugClassCapture(this);
            logger.Trace("frmPOSDrugClassCapture_Load(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
        }
    }
}