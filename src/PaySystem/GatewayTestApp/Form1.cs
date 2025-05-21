using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using FirstMile;
using Gateway;

namespace GatewayTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //txtAcctID.Text = "WYWVM";
        }

        private void ProcessDebitCard()
        {
            using (ProcessDebitCard oProcessDB = new ProcessDebitCard())
            {
                GatewayManager.AccountId = txtAcctID.Text.Trim();

                oProcessDB.DbCEncryptedDeviceType = DeviceType.Ingenico;
                if (txtDBEncryptedSwipeData.Text.Length > 0)
                {
                    oProcessDB.DbCEncryptedSwipeData = txtDBEncryptedSwipeData.Text;

                }
                if (txtDBSwipe.Text.Length > 0)
                {
                    oProcessDB.SwipeData = txtDBSwipe.Text;
                }
                oProcessDB.EncryptedPinData = txtDBEncryptedPinData.Text;
                oProcessDB.Amount = float.Parse(txtDBSaleAmt.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

                TransactionResult result = null;
                Exception exp = new Exception();
                UpdateText("Charging Credit card..\n");
                UpdateText("Connecting to Gateway" + Gateway.GatewayManager.SelectedGateway.ToString());
                oProcessDB.ProcessDebitCardSale(out result, out exp);

                UpdateText("Order id  " + result.OrderId);
                UpdateText("History id  " + result.HistoryId);
                UpdateText("Status  " + result.Status);
                UpdateText("Result  " + result.Result);
                UpdateText("Transaction Type  " + result.TransactionType + "  Amount :" + result.TotalAmt.ToString());
            }

            
        }

        private void ProcessCreditCard()
        {
           /* ProcessCard ocard = new ProcessCard();
            ocard.Amount = decimal.Parse(txtCCSaleAmount.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

            ocard.TransType = TransactionType.Sale;
            ocard.RequireTax = false;

            ocard.AllowTokenization = true;
            ocard.Tokenize =true;
            

            UpdateText("Charging Credit card..\n");

            string result = ocard.DoTransaction();
            UpdateText(result);*/

           // UpdateText("Connecting to Gateway" + Gateway.GatewayManager.SelectedGateway.ToString());



            ProcessCCard oProcessCC = new ProcessCCard();


            //GatewayManager.AccountId = txtAcctID.Text.Trim();
            //oProcessCC.AccountId = "PYGNR";

            oProcessCC.CCNumber = txtCCCardNo.Text;

            if(txtCCExpMonth.Text.Trim().Length>0)
            oProcessCC.CCExpMonth = Convert.ToInt32(txtCCExpMonth.Text);

            if(txtCCExpYear.Text.Trim().Length>0)
            oProcessCC.CCExpYear = Convert.ToInt32(txtCCExpYear.Text);

            oProcessCC.Amount = float.Parse(txtCCSaleAmount.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            oProcessCC.CCCvv2Cid = "000";

            oProcessCC.CCSwipedata = txtCCSwipedata.Text;

            //oProcessCC.MerchantPin = "mmsdemo";

            //oProcessCC.CCEncryptedSwipeData = txtEncCCSwipeData.Text;
            if (txtEncCCSwipeData.Text.Trim().Length > 0)
            {
                oProcessCC.CCEncryptedDeviceType = DeviceType.Ingenico;
            }

            TransactionResult result = null;
            Exception exp = new Exception();
            UpdateText("Charging Credit card..\n");
            UpdateText("Connecting to Gateway"+Gateway.GatewayManager.SelectedGateway.ToString());
            oProcessCC.ProcessCreditCardSale(out result, out exp);

            UpdateText("Order id  "+result.OrderId);
            UpdateText("History id  " + result.HistoryId);
            UpdateText("Status  " + result.Status);
            UpdateText("Result  " + result.Result);
            UpdateText("Transaction Type  " + result.TransactionType+"  Amount :"+result.TotalAmt.ToString());
            

        }

        private void VoidCreditCard()
        {
            ReverseCCTrans oVoidCC = new ReverseCCTrans();

            GatewayManager.AccountId = txtAcctID.Text.Trim();

            //oVoidCC.AccountId = "PYGNR";
            oVoidCC.Amount = float.Parse(txtVoidCCHistoryID.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            oVoidCC.OrderId = txtVoidCCOrderID.Text;
            oVoidCC.HistoryId = txtVoidCCHistoryID.Text;

            //string result = "";
            Exception exp = new Exception();
            TransactionResult result;

            UpdateText("Reversing CC transaction ... ");
            UpdateText("Connecting to Gateway" + Gateway.GatewayManager.SelectedGateway.ToString());

            if (rbVoid.Checked)
            {
                oVoidCC.VoidCCTransaction(out result, out exp);
            }
            else
            {
                oVoidCC.CreditCCTransaction(out result, out exp);
            }

            UpdateText("Order id  " + result.OrderId);
            UpdateText("History id  " + result.HistoryId);
            UpdateText("Status  " + result.Status);
            UpdateText("Result  " + result.Result);
            UpdateText("Transaction Type  " + result.TransactionType + "  Amount :" + result.TotalAmt.ToString());

            


        }

        private void VoidDebitCard()
        {
            ReverseDebitTrans oVoidCC = new ReverseDebitTrans();

            GatewayManager.AccountId = txtAcctID.Text.Trim();

            //oVoidCC.AccountId = "PYGNR";
            oVoidCC.Amount = float.Parse(txtDBReverseAmt.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            oVoidCC.OrderId = txtDBReverseOrderID.Text;
            oVoidCC.HistoryId = txtDBReverseHistoryID.Text;

            //string result = "";
            Exception exp = new Exception();
            TransactionResult result;

            UpdateText("Reversing CC transaction ... ");
            UpdateText("Connecting to Gateway" + Gateway.GatewayManager.SelectedGateway.ToString());

            if (rbDBVoid.Checked)
            {
                oVoidCC.VoidDebitTrans(out result, out exp);
            }
            else
            {
                oVoidCC.ReturnDebitTrans(out result, out exp);
            }

            UpdateText("Order id  " + result.OrderId);
            UpdateText("History id  " + result.HistoryId);
            UpdateText("Status  " + result.Status);
            UpdateText("Result  " + result.Result);
            UpdateText("Transaction Type  " + result.TransactionType + "  Amount :" + result.TotalAmt.ToString());


            

        }

        private void CreditCreditCard()
        {
            ReverseCCTrans oVoidCC = new ReverseCCTrans();

            GatewayManager.AccountId = txtAcctID.Text.Trim();

            //oVoidCC.AccountId = "PYGNR";
            oVoidCC.Amount = float.Parse(txtVoidCCHistoryID.Text, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
            oVoidCC.OrderId = txtVoidCCOrderID.Text;
            oVoidCC.HistoryId = txtVoidCCHistoryID.Text;

            //string result = "";
            Exception exp = new Exception();
            TransactionResult result;

            UpdateText("Reversing CC transaction ... ");
            UpdateText("Connecting to Gateway" + Gateway.GatewayManager.SelectedGateway.ToString());

            if (rbVoid.Checked)
            {
                oVoidCC.VoidCCTransaction(out result, out exp);
            }
            else
            {
                oVoidCC.CreditCCTransaction(out result, out exp);
            }

            UpdateText("Order id  " + result.OrderId);
            UpdateText("History id  " + result.HistoryId);
            UpdateText("Status  " + result.Status);
            UpdateText("Result  " + result.Result);
            UpdateText("Transaction Type  " + result.TransactionType + "  Amount :" + result.TotalAmt.ToString());




        }

        private void btnProcessCC_Click(object sender, EventArgs e)
        {
            ProcessCreditCard();
        }

        private void UpdateText(string message)
        {
            message = message + "\n";
            rtbResult.AppendText(message);
        }

        private void tcTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcTest.SelectedIndex == 1)
            {
                rbVoid.Checked = true;
            }
        }

        private void btnVoidCC_Click(object sender, EventArgs e)
        {
            VoidCreditCard();
        }

        private void btnProcessDBCard_Click(object sender, EventArgs e)
        {
            ProcessDebitCard();
        }

        private void btnReverseDebit_Click(object sender, EventArgs e)
        {
            VoidDebitCard();
        }

       
    }
}
