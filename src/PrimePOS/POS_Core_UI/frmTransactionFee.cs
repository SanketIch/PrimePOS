
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.Resources;
using POS_Core_UI.Layout;
using POS_Core.CommonData.Rows;

namespace POS_Core_UI
{
    public partial class frmTransactionFee : frmTransactionLayout
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        TransFeeData oTransFeeData = new TransFeeData();
        private decimal TotalAmount = 0;
        private decimal AmountToBePaid = 0;
        private decimal OTCAmount = 0;
        private decimal RxAmount = 0;
        private POSTransaction oPosTrans = null;
        public decimal FinalAmount = 0;
        public decimal TransFeeAmt = 0;

        public frmTransactionFee()
        {
            InitializeComponent();
        }

        public frmTransactionFee(TransFeeData transFeeData, decimal amountToBePaid, decimal totalAmount, decimal oTCAmount, decimal rxAmount)
        {
            InitializeComponent();
            oTransFeeData = transFeeData;
            AmountToBePaid = amountToBePaid;
            TotalAmount = totalAmount;
            OTCAmount = oTCAmount;
            RxAmount = rxAmount;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnWaive_Click(object sender, EventArgs e)  // PRIMEPOS-3234
        {
            TransFeeAmt = 0;
            FinalAmount = AmountToBePaid + TransFeeAmt;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void frmTransactionFee_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            TransFeeAmt = CalculateTransFee();
            lblTotalAmt.Text = AmountToBePaid.ToString("##########0.00");
            lblTransFeeAmt.Text = TransFeeAmt.ToString("##########0.00");
            FinalAmount = AmountToBePaid + TransFeeAmt;
            lblFinalAmt.Text = FinalAmount.ToString("##########0.00");
            #region PRIMEPOS-3234
            if (TransFeeAmt > 0 && Configuration.CSetting.WaiveTransactionFee == true)
            {
                this.btnWaive.Enabled = true;
            }
            else
            {
                this.btnWaive.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
                this.btnWaive.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            }
            #endregion
        }

        private void frmTransactionFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                btnCancel_Click(btnCancel, new System.EventArgs());
                e.Handled = true;
            }
        }

        private decimal CalculateTransFee()
        {
            decimal TransFeeAmt = 0;
            try
            {                
                decimal AmtEligibleForTransFee = 0;
                if (Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "1")    //All
                {
                    AmtEligibleForTransFee = (AmountToBePaid > TotalAmount ? TotalAmount : AmountToBePaid);
                }
                else if (Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "2")   //OTC
                {
                    AmtEligibleForTransFee = (AmountToBePaid > OTCAmount ? OTCAmount : AmountToBePaid);
                }
                else if (Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "3") //Rx
                {
                    AmtEligibleForTransFee = (AmountToBePaid > RxAmount ? RxAmount : AmountToBePaid);
                }

                if (oTransFeeData!= null && oTransFeeData.Tables.Count > 0 && oTransFeeData.TransFee.Rows.Count > 0)
                {
                    foreach(TransFeeRow oTransFeeRow in oTransFeeData.TransFee.Rows)
                    {
                        if (oTransFeeRow.TransFeeMode == false)
                            TransFeeAmt += Math.Abs(AmtEligibleForTransFee) * oTransFeeRow.TransFeeValue / 100;
                        else if (oTransFeeRow.TransFeeMode == true)
                            TransFeeAmt += oTransFeeRow.TransFeeValue;
                    }
                }
            }
            catch(Exception Ex)
            {
            }
            return Math.Round(TransFeeAmt, 2);
        }
    }
}
