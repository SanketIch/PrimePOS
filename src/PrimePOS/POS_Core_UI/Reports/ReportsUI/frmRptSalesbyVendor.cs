using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core_UI.Reports.Reports;
//using POS_Core.DataAccess;
//using POS.UI;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptSalesbyVendor : Form
    {
        private struct ItemTransDetail
        {
            public decimal ExtendedPrice;
            public decimal TaxAmount;
            public decimal Discount;
            public long Itemcode;

        }
        private struct ITrans
        {
             public long tranceID;
             public string ItemCode;
            //decimal Discount;
            //long Itemcode;

        }
        public frmRptSalesbyVendor()
        {
            InitializeComponent();
            this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            populateTransType();           

            clsUIHelper.setColorSchecme(this);
            this.dtpSaleEndDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Value = DateTime.Now;

            this.Location = new Point(340, 50);
        }

        private void txtVendorCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {

            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);            
            //if (this.txtVendorCode.Text != "")
            //{
            //    oSearch.txtCode.Text = this.txtVendorCode.Text;
            //    oSearch.searchInConstructor = true;
            //}
            frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, this.txtVendorCode.Text, "", true);    //20-Dec-2017 JY Added new reference
            oSearch.ShowDialog();

            if (oSearch.IsCanceled) return;
            txtVendorCode.Text = oSearch.SelectedRowID();
        }
        private void populateTransType()
        {
            try
            {
                this.cboTransType.Items.Clear();
                this.cboTransType.Items.Add("All", "All");
                this.cboTransType.Items.Add("Sales", "Sales");
                this.cboTransType.Items.Add("Returns", "Returns");
                this.cboTransType.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            PreviewReport(false);
        }
        private void PreviewReport(bool blnPrint)
        {
            try
            {
                Dictionary<long, string> dictionary =
                    new Dictionary<long, string>();
                DateTime sdt, fDate;
                if (DateTime.TryParse(this.dtpSaleStartDate.Text, out sdt) && DateTime.TryParse(this.dtpSaleEndDate.Text, out fDate))
                {
                    if (sdt > fDate)
                    {
                        clsUIHelper.ShowErrorMsg("Start Date should be less than End Date");
                        return;
                    }

                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Enter valid Date ");
                    return;
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptrptSaleByVendor oRpt = new rptrptSaleByVendor();


                String strQuery;
                String strSubQuery;

                strQuery = @"  select V.VendorCode,IV.VendorItemID, PT.TotalDiscAmount, PT.TransID,CONVERT(VARCHAR(10) ,PT.TransDate, 111) as TransDate, PT.UserID,PT.StationID,PT.totalPaid,
                                PT.[GrossTotal],PT.[TotalDiscAmount],PT.[TotalTaxAmount],Case TransType when 1 Then 'Sale' 
                                when 2 Then 'Return' end as TransType,
                                I.ItemID,PTD.Qty,PTD.DISCOUNT, PTD.ItemDescription as Description, PTD.Price,PTD.TaxAMOUNT,PTD.ExtendedPrice,
                                ps.stationname  from Item I
                                left outer join POSTransactionDetail PTD on PTD.ItemID=I.ItemID
                                left outer  join ItemVendor IV on IV.ItemID=I.ItemID
                                left outer  join Vendor V on V.VendorID=IV.VendorID 
                                left outer  join postransaction PT on PT.TransID=PTD.TransID 
                                left outer  join util_POSSet ps on ps.stationid=pt.stationid where ";



                strSubQuery = @"select Sum(PTD.ExtendedPrice)as GrossTotal,
                                Sum(PTD.Discount) as TotalDiscAmount ,
                                Sum(PTD.TaxAmount) as TotalTaxAmount  
                                ,Sum((PTD.ExtendedPrice + PTD.TaxAmount) - PTD.Discount) as TotalPaid from [POSTransaction] PT
                                 join POSTransactionDetail PTD on PTD.TransID=PT.TransID 
                                 join Item I on I.ItemID=PTD.ItemID
                                left outer  join Vendor V on (V.VendorCode=I.LastVendor OR V.VendorCode=I.LastVendor) 
                                 join util_POSSet ps on ps.stationid=pt.stationid where ";
                strQuery = strQuery + "convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";
                strSubQuery = strSubQuery + "convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";


                if (this.txtUserID.Text.Trim() != "")
                {
                    strQuery += " and PT.UserID='" + this.txtUserID.Text.Trim() + "' ";
                    strSubQuery += " and PT.UserID='" + this.txtUserID.Text.Trim() + "' ";
                }

                if (this.cboTransType.SelectedIndex == 1)
                {
                    strQuery += " and TransType=1";
                    strSubQuery += " and TransType=1";
                }
                else if (this.cboTransType.SelectedIndex == 2)
                {
                    strQuery += " and TransType=2";
                    strSubQuery += " and TransType=2";
                }

                if (this.txtItemCode.Text.Trim() != "")
                {
                    strQuery += " and PTD.ItemID='" + this.txtItemCode.Text.Trim() + "' ";
                    strSubQuery += " and PTD.ItemID='" + this.txtItemCode.Text.Trim() + "' ";
                }

                if (this.txtStationID.Text.Trim() != "")
                {
                    strQuery += " and PT.StationID='" + this.txtStationID.Text.Trim() + "' ";
                    strSubQuery += " and PT.StationID='" + this.txtStationID.Text.Trim() + "' ";
                }
                if (this.txtVendorCode.Text.Trim() != "")
                {
                    strQuery += " and V.VendorCode='" + this.txtVendorCode.Text.Trim() + "' ";
                    strSubQuery += " and V.VendorCode='" + this.txtVendorCode.Text.Trim() + "' ";
                }

                DataSet dsMainRptSource = clsReports.GetReportSource(strQuery);
                //decimal GrossTotal = 0;
                //decimal NetTotal = 0;
                //decimal TotalDisc = 0;
                //decimal TotalTax = 0;
                decimal GTotal = 0;
                decimal NTotal = 0;
                decimal TDisc = 0;
                decimal TTax = 0;
                Dictionary<ITrans, ItemTransDetail> dirItemTrance = new Dictionary<ITrans, ItemTransDetail>();
                Dictionary<string, String> dirVendItem = new Dictionary<string, string>();
                //if (string.IsNullOrEmpty(txtVendorCode.Text) == false && POS_Core.Resources.Configuration.isNullOrEmptyDataSet(dsMainRptSource) == false)
                //{
                //    dictionary.Clear();
                //    dirItemTrance.Clear();
                //    foreach (DataRow oRow in dsMainRptSource.Tables[0].Rows)
                //    {
                //        long tranceID = POS_Core.Resources.Configuration.convertNullToInt64(oRow["TransID"]);
                //        string ItemCode = POS_Core.Resources.Configuration.convertNullToString(oRow["ItemID"]);                      
                //        string itemList = "";

                //        if (!dictionary.TryGetValue(tranceID, out itemList))
                //        {//ItemTransDetail itd = new ItemTransDetail();
                //            DataRow[] result = dsMainRptSource.Tables[0].Select("TransID = " + tranceID);
                //            foreach (DataRow row in result)
                //            {
                //                itemList += "'" + row["ItemID"].ToString() + "',";
                //            }
                //            if (itemList.Length > 0)
                //                itemList = itemList.Remove(itemList.Length - 1);
                //            dictionary.Add(tranceID, itemList);

                //        }


                //        GrossTotal += POS_Core.Resources.Configuration.convertNullToDecimal(oRow["ExtendedPrice"]);
                //        TotalDisc += POS_Core.Resources.Configuration.convertNullToDecimal(oRow["Discount"]);
                //        TotalTax += POS_Core.Resources.Configuration.convertNullToDecimal(oRow["TaxAmount"]);
                //        //if(POS_Core.Resources.Configuration.convertNullToDecimal(oRow["TaxAmount"])
                //    }
                //}
                //else
                {
                    foreach (DataRow oRow in dsMainRptSource.Tables[0].Rows)
                    {
                        long tranceID = POS_Core.Resources.Configuration.convertNullToInt64(oRow["TransID"]);
                        string ItemCode = POS_Core.Resources.Configuration.convertNullToString(oRow["ItemID"]);
                        ITrans s = new ITrans();
                        s.ItemCode = ItemCode;
                        s.tranceID = tranceID;

                        string itemList = "";

                        if (!dictionary.TryGetValue(tranceID, out itemList))
                        {//ItemTransDetail itd = new ItemTransDetail();
                            DataRow[] result = dsMainRptSource.Tables[0].Select("TransID = " + tranceID);
                            foreach (DataRow row in result)
                            {
                                itemList += "'" + row["ItemID"].ToString() + "',";
                            }
                            if(itemList.Length>0)
                            itemList=itemList.Remove(itemList.Length - 1);
                            dictionary.Add(tranceID, itemList);

                        }
                        //dsMainRptSource.Tables[0].
                    }
                }
                foreach (long TId in dictionary.Keys)
                {
                    DataRow[] result = dsMainRptSource.Tables[0].Select("TransID = " + TId);
                    string strwhr="";
                    dictionary.TryGetValue(TId, out strwhr);
                    strwhr = "(" + strwhr + ")";
                    string strSelect = @"SELECT sum(Discount) as Discount,sum(TaxAmount) as TaxAmount ,sum(ExtendedPrice) as ExtendedPrice  FROM [POSTransactionDetail] where TransID='" + TId + "' and ItemID in " + strwhr;
                    DataSet dsTotal = clsReports.GetReportSource(strSelect);
                    
                    foreach (DataRow oRow in result)
                    {//PT.[GrossTotal],PT.[TotalDiscAmount],PT.[TotalTaxAmount]
                        // oRow["GrossTotal"]
                        oRow["GrossTotal"] = dsTotal.Tables[0].Rows[0]["ExtendedPrice"];
                        oRow["TotalDiscAmount"] = dsTotal.Tables[0].Rows[0]["Discount"];
                        oRow["TotalTaxAmount"] = dsTotal.Tables[0].Rows[0]["TaxAmount"];
                        oRow["totalPaid"] = POS_Core.Resources.Configuration.convertNullToDecimal(dsTotal.Tables[0].Rows[0]["ExtendedPrice"]) + POS_Core.Resources.Configuration.convertNullToDecimal(dsTotal.Tables[0].Rows[0]["TaxAmount"]) - POS_Core.Resources.Configuration.convertNullToDecimal(dsTotal.Tables[0].Rows[0]["Discount"]);
                        
                        
                        
                    }
                    GTotal += POS_Core.Resources.Configuration.convertNullToDecimal(dsTotal.Tables[0].Rows[0]["ExtendedPrice"]);
                    TDisc += POS_Core.Resources.Configuration.convertNullToDecimal(dsTotal.Tables[0].Rows[0]["Discount"]);
                    TTax += POS_Core.Resources.Configuration.convertNullToDecimal(dsTotal.Tables[0].Rows[0]["TaxAmount"]);
                
                }
                NTotal = GTotal + TTax - TDisc;
                //NetTotal = (GrossTotal + TotalTax) - TotalDisc;
                try
                {
                    //if (string.IsNullOrEmpty(txtVendorCode.Text) == true)
                    {
                        DataSet dsTotal = clsReports.GetReportSource(strSubQuery);
                        //clsReports.DStoExport = dsMainRptSource;
                        //oRpt.Database.Tables[0].SetDataSource(dsMainRptSource.Tables[0]);
                        // oRpt.Open("rptrptSaleByVendor").Database.Tables[0];//.SetDataSource(clsReports.GetReportSource(strSubQuery).Tables[0]);
                        //                String.Format("{0:0.00}", 123.4);
                        var curSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                        string total = curSymbol + Configuration.convertNullToDecimal(TTax).ToString("##########.00");
                        clsReports.setCRTextObjectText("txtGrTotalTax", total, oRpt);
                        total = curSymbol + Configuration.convertNullToDecimal(NTotal).ToString("##########.00");
                        clsReports.setCRTextObjectText("txtGrTotalPaid", total, oRpt);
                        total = curSymbol + Configuration.convertNullToDecimal(TDisc).ToString("##########.00");
                        clsReports.setCRTextObjectText("txtGrTotalDis", total, oRpt);
                        total = curSymbol + Configuration.convertNullToDecimal(NTotal).ToString("##########.00");
                        clsReports.setCRTextObjectText("txtNetSale", total, oRpt);

                        //Flowing lines are commented by Shrikant M as the above line replaced then which contains the Dymic currency symbol.
                        // Changed date 1-24-2014
                        //string total = "$" + POS_Core.Resources.Configuration.convertNullToDecimal(TTax).ToString("##########.00");
                        //clsReports.setCRTextObjectText("txtGrTotalTax", total, oRpt);
                        //total = "$" + POS_Core.Resources.Configuration.convertNullToDecimal(NTotal).ToString("##########.00");
                        //clsReports.setCRTextObjectText("txtGrTotalPaid", total, oRpt);
                        //total = "$" + POS_Core.Resources.Configuration.convertNullToDecimal(TDisc).ToString("##########.00");
                        //clsReports.setCRTextObjectText("txtGrTotalDis", total, oRpt);
                        //total = "$" + POS_Core.Resources.Configuration.convertNullToDecimal(NTotal).ToString("##########.00");
                        //clsReports.setCRTextObjectText("txtNetSale", total, oRpt);
                    }
                    //else
                    //{
                    //    string total = "$" + TotalTax.ToString("##########.00");
                    //    clsReports.setCRTextObjectText("txtGrTotalTax", total, oRpt);
                    //    total = "$" + NetTotal.ToString("##########.00");
                    //    clsReports.setCRTextObjectText("txtGrTotalPaid", total, oRpt);
                    //    total = "$" + TotalDisc.ToString("##########.00");
                    //    clsReports.setCRTextObjectText("txtGrTotalDis", total, oRpt);
                    //    total = "$" + NetTotal.ToString("##########.00");
                    //    clsReports.setCRTextObjectText("txtNetSale", total, oRpt);
                    //}
                    //clsReports.DStoExport = dsMainRptSource;

                }
                catch
                {
                    clsReports.setCRTextObjectText("txtGrTotalTax", String.Format("{0:0.00}", "00.00"), oRpt);
                    clsReports.setCRTextObjectText("txtGrTotalPaid", String.Format("{0:0.00}", "00.00"), oRpt);
                    clsReports.setCRTextObjectText("txtGrTotalDis", String.Format("{0:0.00}", "00.00"), oRpt);
                    clsReports.setCRTextObjectText("txtNetSale", String.Format("{0:0.00}", "00.00"), oRpt);
                }

                clsReports.setCRTextObjectText("txtFromDate", this.dtpSaleStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", dtpSaleEndDate.Text, oRpt);
                clsReports.DStoExport = dsMainRptSource;

                oRpt.Database.Tables[0].SetDataSource(dsMainRptSource.Tables[0]);
                //oRpt.OpenSubreport("rptrpt").Database.Tables[0].SetDataSource(clsReports.GetReportSource(strSubQuery).Tables[0]);
                System.Data.DataSet ds = clsReports.GetReportSource(strSubQuery);
                //this.
                this.TopMost = false;
                clsReports.Preview(blnPrint, oRpt);
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem();
        }

        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Item_tbl, this.txtItemCode.Text, "", true);    //20-Dec-2017 JY Added new reference
                if (this.txtItemCode.Text != "")
                {
                    //oSearch.txtCode.Text = this.txtItemCode.Text;
                    oSearch.SearchInConstructor = true;
                }
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Item_tbl);
                    this.txtItemCode.Focus();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Item_tbl)
            {
                #region item
                try
                {
                    POS_Core.BusinessRules.Item oItem = new Item();
                    ItemData oItemData;
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(code);
                    oItemRow = oItemData.Item[0];
                    if (oItemRow != null)
                    {
                        this.txtItemCode.Text = oItemRow.ItemID;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtItemCode.Text = String.Empty;
                    SearchItem();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.txtItemCode.Text = String.Empty;
                    SearchItem();
                }
                #endregion
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PreviewReport(true);
        }

        private void frmRptSalesbyVendor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.Escape)
                    this.Close();
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtItemCode.ContainsFocus == true)
                    {
                        this.SearchItem();
                    }                    
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmRptSalesbyVendor_Load(object sender, EventArgs e)
        {
            this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                      populateTransType();
            //FillPayType();//Added by Krishna on 22 November 2011

            clsUIHelper.setColorSchecme(this);
            this.dtpSaleEndDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Value = DateTime.Now;

            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboTransType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboTransType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
