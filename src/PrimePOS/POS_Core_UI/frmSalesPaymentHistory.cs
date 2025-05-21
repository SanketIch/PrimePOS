using POS_Core.BusinessRules;
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
    public partial class frmSalesPaymentHistory : Form
    {
        int nReturnTransID = 0;
        public frmSalesPaymentHistory()
        {
            InitializeComponent();
        }

        public frmSalesPaymentHistory(int ReturnTransID)
        {
            nReturnTransID = ReturnTransID;
            InitializeComponent();
        }

        private void frmSalesPaymentHistory_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            GetPaymentDetails();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetPaymentDetails()
        {
            string strSQL = "SELECT CASE WHEN PT.PAYMENTPROCESSOR <> '' AND (PAY.PayTypeID = '3' OR PAY.PayTypeID = '4' OR PAY.PayTypeID = '5' OR PAY.PayTypeID = '6')" + 
                        " THEN PT.PAYMENTPROCESSOR + ' - ' + PAY.PayTypeDesc ELSE PAY.PayTypeDesc END AS DESCRIPTION, CAST(PT.TransAmt AS VARCHAR) AS TransAmt," +
                        " CASE CHARINDEX('|', isnull(PT.RefNo, '')) WHEN 0 THEN" +
                        " (CASE WHEN ISNULL(PT.CLCOUPONID, 0) <> 0 THEN 'CL Coupon# ' + CAST(CLCOUPONID AS VARCHAR(100)) ELSE PT.RefNo END)" +
                        " ELSE '******' + RIGHT(RTRIM(LEFT(PT.RefNo, CHARINDEX('|', PT.RefNo) - 1)), 4) END AS RefNo" +
                        " FROM PAYTYPE PAY, (SELECT RefNo, TransID, TransTypeCode, SUM(POSTransPayment.Amount)AS TransAmt, CLCOUPONID," +
                        " CASE WHEN PAYMENTPROCESSOR IS NULL OR PAYMENTPROCESSOR = 'N/A' THEN '' ELSE PAYMENTPROCESSOR END AS PAYMENTPROCESSOR FROM POSTRANSPAYMENT GROUP BY TransTypeCode, TransID, RefNo, CLCOUPONID, PAYMENTPROCESSOR) AS PT" +
                        " WHERE PT.TransTypeCode = Pay.PayTypeID AND PT.TransID = " + nReturnTransID;

            Search oSearch = new Search();
            DataSet ds = oSearch.SearchData(strSQL);
            grdPaymentDetails.DataSource = ds;

            grdPaymentDetails.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Payment Type";
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Amount";
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[2].Header.Caption = "Ref No / CC No";

            grdPaymentDetails.DisplayLayout.Bands[0].Columns[0].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[1].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[2].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[0].Width = 180;
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[1].Width = 80;
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[2].Width = 150;

            grdPaymentDetails.DisplayLayout.Bands[0].Columns[1].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[1].MinValue = -99999999999.99;
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[1].MaxValue = 99999999999.99;
            grdPaymentDetails.DisplayLayout.Bands[0].Columns[1].Format = "##########0.00";
            clsUIHelper.SetReadonlyRow(this.grdPaymentDetails);
            this.grdPaymentDetails.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
        }

        private void frmSalesPaymentHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                btnClose_Click(btnClose, new System.EventArgs());
                e.Handled = true;
            }
        }
    }
}
