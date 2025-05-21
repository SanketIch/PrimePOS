using System;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmPatientPayCCSelect : Form
    {
        public bool IsPatientCCFromPrimeRx = false;
        public frmPatientPayCCSelect()
        {
            InitializeComponent();
        }

        private void frmPatientPayCCSelect_Load(object sender, EventArgs e)
        {
             
        }
        private void btnPrimeRxPaySource_Click(object sender, EventArgs e)
        {
            IsPatientCCFromPrimeRx = false;
            this.Close();
        }

        private void btnHPSSource_Click(object sender, EventArgs e)
        {
            IsPatientCCFromPrimeRx = true;
            this.Close();
        }
    }
}
