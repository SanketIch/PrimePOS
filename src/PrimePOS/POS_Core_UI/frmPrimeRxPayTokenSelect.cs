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
    public partial class frmPrimeRxPayTokenSelect : Form
    {
        public bool isPrimeRxPay = false;
        public frmPrimeRxPayTokenSelect(string PaymentProcessor)
        {
            InitializeComponent();
            btnPaymentProcessor.Text = PaymentProcessor;
            ultraLabel1.Text = $"The token is compatible with both {PaymentProcessor} and PrimeRxPay payment methods. Please select which one to use.";//PRIMEPOS-3057
        }

        private void frmPrimeRxPayTokenSelect_Load(object sender, EventArgs e)
        {
            btnPaymentProcessor.Focus();
        }

        private void btnPaymentProcessor_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void btnPrimeRxPay_Click(object sender, EventArgs e)
        {
            isPrimeRxPay = true;
            this.Close();
        }

        private void btnPaymentProcessor_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            isPrimeRxPay = true;
            this.Close();
        }
    }
}
