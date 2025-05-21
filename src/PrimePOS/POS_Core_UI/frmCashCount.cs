using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
//using POS_Core.DataAccess;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmCashCount : Form
    {
        static string LastClick = "";
        bool isCurrancyPress = true;
        bool isGridPress = false;
        DataTable dt = new DataTable();
        DataRow row = null;
        private string NValue;
        private string CurrName;
        private decimal Total = 0;
        decimal cntno = 0;
        private decimal icnt = 0;
        bool Repeate = false;
        POS_Core.CommonData.POSTransPaymentData objPayTypeCash = new POS_Core.CommonData.POSTransPaymentData();
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmCashCount()
        {
            InitializeComponent();
            this.ultraButton1.Text = Configuration.CInfo.CurrencySymbol + " 1000 ";
            this.ultraButton25.Text = Configuration.CInfo.CurrencySymbol + " 100 ";
            this.ultraButton23.Text = Configuration.CInfo.CurrencySymbol + " 20 ";
            this.ultraButton21.Text = Configuration.CInfo.CurrencySymbol + " 50 ";
            this.ultraButton19.Text = Configuration.CInfo.CurrencySymbol + " 10 ";
            this.ultraButton17.Text = Configuration.CInfo.CurrencySymbol + " 5 ";
            this.ultraButton15.Text = Configuration.CInfo.CurrencySymbol + " 2 ";
            this.ultraButton14.Text = Configuration.CInfo.CurrencySymbol + " 1 ";
            this.ultraLabel5.Text = Configuration.CInfo.CurrencySymbol.ToString(CultureInfo.InvariantCulture);
        }

        private void frmCashCount_Load(object sender, EventArgs e)
        {
            this.utxtTotalAmount.Text = "0.00";
            clsUIHelper.setColorSchecme(this);

            EnterGridRow();

        }
        private void Counter(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("Counter(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);

                if (ulttxtCurname.Text.Trim() != "" && ulttxtCurname.Text.Trim() != "Currency")
                {

                    if (isCurrancyPress)
                    {
                        isGridPress = false;
                        ultxtCurrencyNo.Clear();
                        isCurrancyPress = false;
                    }
                    if (isGridPress)
                    { ultxtCurrencyNo.Clear(); }
                    isGridPress = false;
                    Infragistics.Win.Misc.UltraButton btn = new Infragistics.Win.Misc.UltraButton();
                    btn = (Infragistics.Win.Misc.UltraButton)sender;

                    NValue = btn.Name.Remove(0, 6);
                    if (ultxtCurrencyNo.Text.Trim().Length < 16)
                    {
                        ultxtCurrencyNo.Text = ultxtCurrencyNo.Text + NValue;
                    }
                    cntno = Convert.ToInt64(ultxtCurrencyNo.Text);
                    cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                    if (CurrName != null)
                    {
                        CurrencyCount(CurrName);

                    }
                    else
                    {
                        return;
                    }
                    cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                }
                else { ultxtCurrencyNo.Text = "Currency Count"; }
                this.ultbtnEnt.Focus();
                logger.Trace("Counter(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Counter(object sender, EventArgs e)");
            }
        }

        private void EnterGridRow()
        {
            //grdCashDetail.Bounds.Size.
            try
            {
                grdCashDetail.DataSource = CreateTable();

                grdCashDetail.DisplayLayout.Bands[0].Columns["R"].Width = 75;
                grdCashDetail.DisplayLayout.Bands[0].Columns["Count"].Width = 60;
                grdCashDetail.DisplayLayout.Bands[0].Columns["Total"].Format = "##,###,####.00";
                grdCashDetail.DisplayLayout.Bands[0].Columns["Total"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                grdCashDetail.DisplayLayout.Bands[0].Columns["Coin/Bill"].Width = 80;
                grdCashDetail.DisplayLayout.Bands[0].Columns["R"].Header.VisiblePosition = 4;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "EnterGridRow()");
            }

        }
        private DataTable CreateTable()
        {
            try
            {
                if (!dt.Columns.Contains("Coin/Bill"))
                    dt.Columns.Add("Coin/Bill", typeof(string));
                if (!dt.Columns.Contains("Count"))
                    dt.Columns.Add("Count", typeof(Int64));
                if (!dt.Columns.Contains("Total"))
                    dt.Columns.Add("Total", typeof(decimal));
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CreateTable()");
            }
            return dt;
        }

        private void ultbtnEnt_Click(object sender, EventArgs e)
        {
            Int32 i;
            Int64 Totcnt = 0;
            Int64 resultCount;
            try
            {
                logger.Trace("ultbtnEnt_Click(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
                if (ultxtCurrencyNo.TextLength > 0 && ulttxtCurname.TextLength > 0 && Int64.TryParse(ultxtCurrencyNo.Text.Trim(), out resultCount) && ulttxtCurname.Text.Trim() != "Currency")
                {
                    cnt = Convert.ToInt64(ultxtCurrencyNo.Text);

                    if (NValue != "")
                    {
                        for (i = 0; i < grdCashDetail.Rows.Count; i++)
                        {
                            if (grdCashDetail.Rows[i].Cells[0].Text == CurrName)
                            {
                                row = dt.Rows[i];
                                row[0] = CurrName;
                                Repeate = true;
                                //if (isCurrancyPress)
                                //{
                                ///   cnt = Convert.ToInt64(grdCashDetail.Rows[i].Cells[1].Text) + (Convert.ToInt64(ultxtCurrencyNo.Text));// + (Convert.ToInt64(ultxtCurrencyNo.Text)));
                                //}
                                //else
                                {
                                    cnt = (Convert.ToInt64(ultxtCurrencyNo.Text));
                                }
                                if (cnt <= 0)
                                {
                                    grdCashDetail.Rows[i].Delete();

                                }
                                else
                                {
                                    row[1] = cnt;

                                    Total = CurrencyCount(CurrName);
                                }
                                row[2] = Total;
                                //utxtTotalAmount.Text = "";
                                utxtTotalAmount.Text = Convert.ToString(TotAmt());
                                ultxtCurrencyNo.Text = "Currency Count";
                                ulttxtCurname.Text = "Currency";
                                return;
                            }

                        }
                        cnt = (Convert.ToInt64(ultxtCurrencyNo.Text));
                        Totcnt = (Convert.ToInt64(ultxtCurrencyNo.Text));
                        
                        row = dt.NewRow();
                        row[0] = CurrName;
                        if (cnt <= 0)
                        {
                            row[1] = 0;
                            Total = CurrencyCount(CurrName);
                        }
                        else
                        { 
                            row[1] = icnt;
                            Total = CurrencyCount(CurrName);
                        }
                        row[2] = Total;
                        decimal Amount = Total;
                        if (Amount > 0)
                        {
                            dt.Rows.Add(row);
                        }


                    }
                    utxtTotalAmount.Text = "";
                    utxtTotalAmount.Text = Convert.ToString(TotAmt());
                    Total = 0;
                    ultxtCurrencyNo.Text = "Currency Count";
                    ulttxtCurname.Text = "Currency";
                  
                }
                 this.ultbtnEnt.Focus();
                logger.Trace("ultbtnEnt_Click(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ultbtnEnt_Click(object sender, EventArgs e)");
                MessageBox.Show(ex.ToString());
            }


        }

        private void ultbtnBK_Click(object sender, EventArgs e)
        {
            try
            {


                if (ultxtCurrencyNo.TextLength > 0)
                {
                    NValue = ultxtCurrencyNo.Text;
                    NValue = NValue.Substring(0, (ultxtCurrencyNo.TextLength - 1));
                    ultxtCurrencyNo.Text = NValue;
                    cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ultbtnBK_Click(object sender, EventArgs e)");
            }
             this.ultbtnEnt.Focus();
        }
        private decimal TotAmt()
        {

            decimal tot = 0;
            try
            {
                foreach (UltraGridRow dr in grdCashDetail.Rows)
                {
                    tot = tot + Convert.ToDecimal(dr.Cells["Total"].Value);

                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "TotAmt()");
            }
            return tot;
        }
        private decimal CurrencyCount(string CurrrencyType)
        {
            cntno = 0;
            try
            {
                logger.Trace("CurrencyCount(string CurrrencyType) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);

                grdCashDetail.DisplayLayout.Bands[0].Columns["R"].Header.VisiblePosition = 4;
                if (CurrrencyType == "Penny")
                {
                    Total = (icnt*1/100);
                }
                else if (CurrrencyType == "Nickel")
                {
                    Total = (icnt*1/20);
                }
                else if (CurrrencyType == "Dime")
                {
                    Total = (icnt*1/10);
                }
                else if (CurrrencyType == "Quarter")
                {
                    Total = (icnt*1/4);
                }
                else if (CurrrencyType == ".5 Dollar")
                {
                    Total = (icnt*1/2);
                }
                else if (CurrrencyType == "0.5 Dollar")
                {
                    Total = (icnt*1/2);
                }
                else if (CurrrencyType == "1 Dollar")
                {
                    Total = (icnt*1);
                }
                else if (CurrrencyType == "2 Dollar")
                {
                    Total = (icnt*2);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol+" 1 ")
                {
                    Total = (icnt*1);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol + " 2 ")   //Sprint-24 - PRIMEPOS-2274 28-Oct-2016 JY Added space after 2
                {
                    Total = (icnt*2);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol + " 5 ")
                {
                    Total = (icnt*5);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol + " 10 ")  //Sprint-24 - PRIMEPOS-2274 28-Oct-2016 JY Added space after 20
                {
                    Total = (icnt*10);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol + " 20 ")  //Sprint-24 - PRIMEPOS-2274 28-Oct-2016 JY Added space after 20
                {
                    Total = (icnt*20);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol + " 50 ")
                {
                    Total = (icnt*50);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol + " 100 ")
                {
                    Total = (icnt*100);
                }
                else if (CurrrencyType == Configuration.CInfo.CurrencySymbol + " 1000 ")
                {
                    Total = (icnt*1000);
                }
                logger.Trace("CurrencyCount(string CurrrencyType) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "CurrencyCount(string CurrrencyType)");
            }
            return Total;
        }

        private void CurrencyName(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("CurrencyName(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);

                string Currencycount = "";
                isGridPress = false;
                isCurrancyPress = true;
                Infragistics.Win.Misc.UltraButton btn = new Infragistics.Win.Misc.UltraButton();
                btn = (Infragistics.Win.Misc.UltraButton)sender;
                CurrName = btn.Text;
                for (int i = 0; i < grdCashDetail.Rows.Count; i++)
                {
                    if (grdCashDetail.Rows[i].Cells[0].Text == CurrName)
                    {
                        Currencycount = grdCashDetail.Rows[i].Cells[1].Text;
                    }
                }
                if (Currencycount != "")
                {
                    ultxtCurrencyNo.Text = Currencycount;
                }
                else
                {
                    ultxtCurrencyNo.Text = "1";
                }
                CurrName = btn.Text;
                ulttxtCurname.Text = CurrName;
                NValue = btn.Name.Remove(0, 6);
                //ultxtCurrencyNo.Text = ultxtCurrencyNo.Text + NValue;
                cntno = Convert.ToInt64(ultxtCurrencyNo.Text);
                cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                CurrencyCount(CurrName);
                //Counter(
                logger.Trace("CurrencyName(object sender, EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CurrencyName(object sender, EventArgs e)");
            }
            //try
            //{

            //    Infragistics.Win.Misc.UltraButton btn = new Infragistics.Win.Misc.UltraButton();
            //    btn = (Infragistics.Win.Misc.UltraButton)sender;
            //    ultxtCurrencyNo.Text = "1";
            //    CurrName = btn.Text;
            //    ulttxtCurname.Text = CurrName;
            //}
            //catch (Exception)
            //{

            //}

        }
        public decimal cnt //setting up currency count
        {
            get
            {
                ;
                return icnt;
            }
            set
            {
                if (Repeate == true)
                {
                    icnt = value;

                    Repeate = false;

                }
                else
                {
                    icnt = value;
                }
            }
        }

        private void grdCashDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {

            grdCashDetail.ActiveRow.Delete();
            ultxtCurrencyNo.Text = "Currency Count";
            ulttxtCurname.Text = "Currency";                  
            utxtTotalAmount.Text = Convert.ToString(TotAmt());
        }

        private void ultbtnok_Click(object sender, EventArgs e)
        { 
            if (utxtTotalAmount.Text != "")
            {  
                objPayTypeCash.PaytypeCash = Convert.ToDecimal(utxtTotalAmount.Text);
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void ultbtnclear_Click(object sender, EventArgs e)
        {

            //frmCashCount1 counttest = new frmCashCount1();
            //counttest.ShowDialog();
            try
            {
                dt.Clear();
                grdCashDetail.DataSource = dt;
                EnterGridRow();
                ultxtCurrencyNo.Text = "Currency Count";
                ulttxtCurname.Text = "Currency";                  
                utxtTotalAmount.Text = "00.00";
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ultbtnclear_Click(object sender, EventArgs e)");
            }
        }


        private void grdCashDetail_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            UltraGridRow row = this.grdCashDetail.ActiveRow;

            try
            {
                if (row != null)
                {
                    isGridPress = true;
                    isCurrancyPress = false;
                    ultxtCurrencyNo.Text = grdCashDetail.ActiveRow.Cells["Count"].Value.ToString();
                    CurrName = this.ulttxtCurname.Text = grdCashDetail.ActiveRow.Cells["Coin/Bill"].Text;
                    ulttxtCurname.Text = CurrName;
                    NValue = CurrName;
                    //ultxtCurrencyNo.Text = ultxtCurrencyNo.Text + NValue;
                    cntno = Convert.ToInt64(ultxtCurrencyNo.Text);
                    cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                    CurrencyCount(CurrName);
                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "grdCashDetail_DoubleClickRow(object sender, DoubleClickRowEventArgs e)");
            }
            //if (row != null)
            //{   
            //    this.ulttxtCurname.Text = row.Cells["Coin/Bill"].Text;
            //    this.ultxtCurrencyNo.Text ="";// row.Cells["Count"].Text;
            //    Int16 rowindex =Convert.ToInt16(e.Row.Index);
            //    grdCashDetail.Rows[rowindex].Cells["Count"].Value = 0;
            //}
        }

        private void grdCashDetail_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            UltraGridRow row = this.grdCashDetail.ActiveRow;
            if (row != null)
            {
                isGridPress = true;
                isCurrancyPress = false;
                ultxtCurrencyNo.Text = grdCashDetail.ActiveRow.Cells["Count"].Value.ToString();
                CurrName = this.ulttxtCurname.Text = grdCashDetail.ActiveRow.Cells["Coin/Bill"].Text;
                ulttxtCurname.Text = CurrName;
                NValue = CurrName;
                //ultxtCurrencyNo.Text = ultxtCurrencyNo.Text + NValue;
                cntno = Convert.ToInt64(ultxtCurrencyNo.Text);
                cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                CurrencyCount(CurrName);
            }
        }

        private void grdCashDetail_Click(object sender, EventArgs e)
        {
            UltraGridRow row = this.grdCashDetail.ActiveRow;
            try
            {
                if (row != null)
                {
                    isGridPress = true;
                    isCurrancyPress = false;
                    ultxtCurrencyNo.Text = grdCashDetail.ActiveRow.Cells["Count"].Value.ToString();
                    CurrName = this.ulttxtCurname.Text = grdCashDetail.ActiveRow.Cells["Coin/Bill"].Text;
                    ulttxtCurname.Text = CurrName;
                    NValue = CurrName;
                    //ultxtCurrencyNo.Text = ultxtCurrencyNo.Text + NValue;
                    cntno = Convert.ToInt64(ultxtCurrencyNo.Text);
                    cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                    CurrencyCount(CurrName);
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "grdCashDetail_Click(object sender, EventArgs e)");
            }
        }

        private void grdCashDetail_CellChange(object sender, CellEventArgs e)
        {
            UltraGridRow row = this.grdCashDetail.ActiveRow;
            try
            {
                if (row != null)
                {
                    isGridPress = true;
                    isCurrancyPress = false;
                    ultxtCurrencyNo.Text = grdCashDetail.ActiveRow.Cells["Count"].Value.ToString();
                    CurrName = this.ulttxtCurname.Text = grdCashDetail.ActiveRow.Cells["Coin/Bill"].Text;
                    ulttxtCurname.Text = CurrName;
                    NValue = CurrName;
                    //ultxtCurrencyNo.Text = ultxtCurrencyNo.Text + NValue;
                    cntno = Convert.ToInt64(ultxtCurrencyNo.Text);
                    cnt = Convert.ToInt64(ultxtCurrencyNo.Text);
                    CurrencyCount(CurrName);
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "grdCashDetail_CellChange(object sender, CellEventArgs e)");
            }
        }

        private void frmCashCount_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void frmCashCount_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Escape)
                {
                    this.Close();
                }
        }



    }
}