using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core_UI.Reports.Reports;
using POS_Core.Resources;
//using POS_Core.DataAccess;
//using POS.UI;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptDeadStockReport : Form
    {
        private DataSet oDataSet = new DataSet();
        //private IContainer components;
        public frmRptDeadStockReport()
        {
            InitializeComponent();
            try
            {
                grdSearch.DataSource = oDataSet;
                this.resizeColumns();
                grdSearch.Refresh();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void resizeColumns()
        {
            foreach (UltraGridColumn oCol in grdSearch.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 20;
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
                
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
       
        private void EditItem()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                frmItems oItems = new frmItems();
                oItems.Edit(grdSearch.ActiveRow.Cells[0].Text.Trim());
                oItems.ShowDialog(this);
                Search();
                
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void AddItem()
        {
            try
            {
                frmItems oItems = new frmItems();
                oItems.Initialize();
                oItems.ShowDialog(this);
                if (!oItems.IsCanceled) { }
                Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void frmRptDeadStockReport_Load(object sender, EventArgs e)
        {
			frmMain.getInstance().Cursor = Cursors.WaitCursor;
            clsUIHelper.SetAppearance(this.grdSearch);
            clsUIHelper.SetReadonlyRow(this.grdSearch);
            this.txtName.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtName.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.numNumberOfDays.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numNumberOfDays.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            clsUIHelper.setColorSchecme(this);
            this.btnEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998);   //PRIMEPOS-2538 24-Jun-2020 JY Added   
            Search();
			frmMain.getInstance().Cursor = Cursors.Default;
        }

        private void ApplyGridFormate()
        {
            grdSearch.DisplayLayout.Bands[0].Columns["ItemID"].Header.Caption = "Item ID";
            grdSearch.DisplayLayout.Bands[0].Columns["ItemDescription"].Header.Caption = "Item Description";    //PRIMEPOS-2538 24-Jun-2020 JY Added
            grdSearch.DisplayLayout.Bands[0].Columns["TransID"].Header.Caption = "Trans #";
            grdSearch.DisplayLayout.Bands[0].Columns["Price"].Header.Caption = "Price";
            grdSearch.DisplayLayout.Bands[0].Columns["TransDate"].Header.Caption = "Trans Date";
            grdSearch.DisplayLayout.Bands[0].Columns["QtySold"].Header.Caption = "Qty Sold";
            grdSearch.DisplayLayout.Bands[0].Columns["QtyInStock"].Header.Caption = "Qty In Stock";
            grdSearch.DisplayLayout.Bands[0].Columns["userID"].Header.Caption = "User ID";
			grdSearch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);
        }

        private void Preview(bool PrintId)
        {
            try
            {
                //PRIMEPOS-2538 24-Jun-2020 JY Added item Description
                ReportClass oRpt;               
                oRpt = new rptDeadStockReport();
                string sSQL = string.Empty;
                if (txtCode.Text != "" || this.txtName.Text != "")
                {
                    if (txtCode.Text != "" && this.txtName.Text != "")
                    {
                        sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                             "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                             " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                             " from POSTransaction PT,POSTransactionDetail PTD " +
                             " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                             " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                             " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) " +
                             " and (PTD.ItemDescription like '" + this.txtName.Text.Trim() + "%' or PTD.itemID like '" + this.txtCode.Text.Trim() + "%')";
                    }
                    else if (txtName.Text != "")
                    {
                        sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                            "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                            " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                            " from POSTransaction PT,POSTransactionDetail PTD " +
                            " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                            " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                            " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) " +
                            " and (PTD.ItemDescription like '" + this.txtName.Text.Trim() + "%')";
                    }
                    else
                    {
                        sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                              "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                              " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                              " from POSTransaction PT,POSTransactionDetail PTD " +
                              " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                              " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                              " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) " +
                              " and (PTD.itemID like '" + this.txtCode.Text.Trim() + "%')";
                    }
                }
                else
                {
                    sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                   "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                   " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                   " from POSTransaction PT,POSTransactionDetail PTD " +
                   " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                   " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                   " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) ";
                }

                if (chckUseQtyINStock.Checked)
                {
                    sSQL += " and I.QtyInStock>0 ";
                }
                sSQL += " GROUP BY PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty, I.QtyInStock,PT.userID, PTD.ItemDescription) AS RESULT" +
                    " WHERE RowNo = 1";

                clsReports.setCRTextObjectText("txtDays", "" + this.numNumberOfDays.Text +" Days", oRpt);
                clsReports.setCRTextObjectText("txtPrintDate", "" + DateTime.Now.ToShortDateString(), oRpt);
                clsReports.setCRTextObjectText("txtTotalRecords", "" + oDataSet.Tables[0].Rows.Count.ToString(), oRpt);
               // oDataSet = clsReports.GetReportSource(sSQL);
               // clsReports.Preview(PrintId, oDataSet, oRpt);
                clsReports.Preview(PrintId, sSQL, oRpt);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private DataSet GetRecords(DataSet dsMain)
        {
            DataSet Ds = null ;

            foreach (DataRow row in dsMain.Tables[0].Rows)
            {
                
            }
            return Ds;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Preview(false);
        }

        private void frmRptDeadStockReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    if (this.grdSearch.ContainsFocus == true)
                    {
                        if (this.grdSearch.Rows.Count > 0)
                        {
                            //IsCanceled = false;
                            this.Close();
                        }
                    }
                    if (this.txtCode.ContainsFocus == true)
                    {
                        //Search();
                    }
                    else
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.Escape)
                {
                    this.Close();
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    Search();
                }
                else if (this.btnEdit.Visible == true && e.KeyData == System.Windows.Forms.Keys.F3) //PRIMEPOS-2538 24-Jun-2020 JY modified
                {
                    EditItem();
                }
            }
            catch { }
        }

        private void Search()
        {
            try
            {
                //PRIMEPOS-2538 24-Jun-2020 JY Added item Description
                string sSQL = "";
                if (txtCode.Text != "" || this.txtName.Text != "")
                {
                    if (txtCode.Text != "" && this.txtName.Text != "")
                    {
                        sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                             "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                             " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                             " from POSTransaction PT,POSTransactionDetail PTD " +
                             " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                             " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                             " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) " +
                             " and (PTD.ItemDescription like '" + this.txtName.Text.Trim() + "%' or PTD.itemID like '" + this.txtCode.Text.Trim() + "%')";
                    }
                    else if (txtName.Text != "")
                    {
                        sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                            "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                            " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                            " from POSTransaction PT,POSTransactionDetail PTD " +
                            " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                            " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                            " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) " +
                            " and (PTD.ItemDescription like '" + this.txtName.Text.Trim() + "%')";
                    }
                    else
                    {
                        sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                              "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                              " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                              " from POSTransaction PT,POSTransactionDetail PTD " +
                              " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                              " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                              " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) " +
                              " and (PTD.itemID like '" + this.txtCode.Text.Trim() + "%')";
                    }
                }
                else
                {
                    sSQL = " SELECT ItemID, ItemDescription, Price, TransDate, TransID, QtySold, QtyInStock, userID FROM " +
                       "(Select max(PT.TransDate) as date,PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty as QtySold,I.QtyInStock,PT.userID, PTD.ItemDescription," +
                       " ROW_NUMBER() OVER(PARTITION BY PTD.ItemID ORDER BY PT.TransDate DESC) AS RowNo " +
                       " from POSTransaction PT,POSTransactionDetail PTD " +
                       " INNER JOIN Item I on I.ItemID=PTD.ItemID " +
                       " where PT.TransID=PTD.TransID and PTD.itemID=I.ItemID" +
                       " and PT.TransID NOT IN (select transID from POSTransaction POST where POST.transdate > DateAdd(D,-  " + numNumberOfDays.Value.ToString().Trim() + " ,getdate())) ";
                }

                if (chckUseQtyINStock.Checked)
                {
                    sSQL += " and I.QtyInStock>0 ";
                }
                sSQL += " GROUP BY PTD.ItemID,PTD.Price,PT.TransDate,PT.TransID,PTD.Qty, I.QtyInStock,PT.userID,PTD.ItemDescription)AS RESULT" +
                    " WHERE RowNo = 1";
                oDataSet = clsReports.GetReportSource(sSQL);
                grdSearch.DataSource = oDataSet;
                ApplyGridFormate();
                this.resizeColumns();
                grdSearch.Refresh();
                txtEditorNoOfOrds.Text = oDataSet.Tables[0].Rows.Count.ToString();
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditItem();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
