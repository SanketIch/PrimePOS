using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using POS.UI;
using POS_Core.CommonData;
using Infragistics.Win.UltraWinGrid;
using POS_Core.DataAccess;
using POS_Core_UI.Reports.Reports;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptCouponReport : Form
    {
        private int CurrentX;
        private int CurrentY;

        public frmRptCouponReport()
        {
            InitializeComponent();
        }

        private void txtCoupon_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCouponId();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchCouponId()
        {
            try
            {
                frmSearch oSearch = new frmSearch(clsPOSDBConstants.Coupon_tbl);
                oSearch.AllowMultiRowSelect = true;
                oSearch.searchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCouponId = string.Empty;
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            if (strCouponId == string.Empty)
                                strCouponId = oRow.Cells["ID"].Text;
                            else
                                strCouponId += "," + oRow.Cells["ID"].Text;
                        }
                    }
                    txtCoupon.Text = strCouponId;
                }
                else
                {
                    txtCoupon.Text = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmRptCouponReport_Load(object sender, EventArgs e)
        {
            this.txtCoupon.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCoupon.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUserID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtStationID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtStationID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.setColorSchecme(this);
            this.dtpStartDate.Value = DateTime.Now;
            this.dtpEndDate.Value = DateTime.Now;
            this.optSortBy.Items[0].CheckState = CheckState.Checked;

            clsUIHelper.SetAppearance(this.grdData);
            clsUIHelper.SetReadonlyRow(this.grdData);
            grdData.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdData.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            btnSearch_Click(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchSvr oSearchSvr = new SearchSvr();
            string strSQL;
            try
            {
                DataSet dsCoupon = new DataSet();
                DataSet dsCouponTmp = new DataSet();

                GenerateSQL(out strSQL, false);
                dsCouponTmp.Tables.Add(oSearchSvr.Search(strSQL).Tables[0].Copy());

                var CouponSummary = from Coupon in dsCouponTmp.Tables[0].AsEnumerable()
                                  group Coupon by new
                                  {
                                      CouponID = Coupon.Field<Int64>("CouponID"),
                                      CouponCode = Coupon.Field<string>("CouponCode"),
                                      CouponDesc = Coupon.Field<string>("CouponDesc")
                                  } into userg
                                  select new
                                  {
                                      CouponID = userg.Key.CouponID,
                                      CouponCode = userg.Key.CouponCode,
                                      CouponDesc = userg.Key.CouponDesc,
                                      Qty = userg.Sum(x => x.Field<Int32>("Qty")),
                                      Amount = userg.Sum(x => x.Field<decimal>("Amount")),
                                  };

                DataTable dt = new DataTable();
                dt.Columns.Add("CouponID", typeof(System.Int64));
                dt.Columns.Add("Coupon Code", typeof(System.String));
                dt.Columns.Add("Coupon Desc", typeof(System.String));
                dt.Columns.Add("Quantity", typeof(System.Int32));
                dt.Columns.Add("Amount", typeof(System.Decimal));
                DataRow dr;

                foreach (var Summary in CouponSummary)
                {
                    dr = dt.NewRow();
                    dr["CouponID"] = Summary.CouponID;
                    dr["Coupon Code"] = Summary.CouponCode;
                    dr["Coupon Desc"] = Summary.CouponDesc;
                    dr["Quantity"] = Summary.Qty;
                    dr["Amount"] = Summary.Amount;

                    dt.Rows.Add(dr);
                }

                dsCoupon.Tables.Add(dt);   //Added table for summary by coupon
                dsCoupon.Tables[0].TableName = "Summary";
                dsCoupon.Tables.Add(dsCouponTmp.Tables[0].Copy());
                dsCoupon.Tables[1].TableName = "Details";

                dsCoupon.Relations.Add("Details", dsCoupon.Tables[0].Columns["CouponID"], dsCoupon.Tables[1].Columns["CouponID"]);

                grdData.DataSource = dsCoupon;
                grdData.DisplayLayout.Bands[0].HeaderVisible = false;
                //grdData.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                //grdData.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                //grdData.DisplayLayout.Bands[0].Header.Caption = "Coupon Summary";
                grdData.DisplayLayout.Bands[0].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdData.DisplayLayout.Bands[0].Columns["CouponID"].Hidden = true;

                grdData.DisplayLayout.Bands[1].HeaderVisible = false;
                //grdData.DisplayLayout.Bands[1].Header.Caption = "Detail";
                //grdData.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                //grdData.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdData.DisplayLayout.Bands[1].Expandable = true;
                grdData.DisplayLayout.Bands[1].Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
                grdData.DisplayLayout.Bands[1].Columns["CouponID"].Hidden = true;
                grdData.DisplayLayout.Bands[1].Columns["CouponCode"].Hidden = true;
                grdData.DisplayLayout.Bands[1].Columns["CouponDesc"].Hidden = true;
                grdData.DisplayLayout.Bands[1].Columns["TransDetailID"].Hidden = true;

                resizeColumns(grdData);
                grdData.PerformAction(UltraGridAction.FirstRowInGrid);
                grdData.Refresh();
            }
            catch (Exception Ex)
            {

            }
        }

        private void resizeColumns(Infragistics.Win.UltraWinGrid.UltraGrid grd)
        {
            try
            {
                foreach (UltraGridBand oBand in grd.DisplayLayout.Bands)
                {
                    foreach (UltraGridColumn oCol in oBand.Columns)
                    {
                        oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void GenerateSQL(out string strSQL, Boolean Flag)
        {
            //flag = true means call from PreviewReport
            string strFilter = string.Empty, strPayType = string.Empty, strDisc = string.Empty;

            try
            {
                string strOrderBy = (optSortBy.Items[0].CheckState == CheckState.Checked) ? "c.TransID" : "a.CouponDesc";

                if (!string.IsNullOrWhiteSpace(this.txtCoupon.Text))
                    strFilter = " AND a.CouponID IN (" + txtCoupon.Text.ToString().Trim() + ")";
                
                if (!string.IsNullOrWhiteSpace(this.txtUserID.Text))
                    strFilter += " AND c.UserID = '" + this.txtUserID.Text.ToString().Trim().Replace("'","''") + "'";

                if (!string.IsNullOrWhiteSpace(this.txtStationID.Text))
                    strFilter += " AND c.StationID = '" + this.txtStationID.Text.ToString().Trim().Replace("'", "''") + "'";

                //if (Flag == false)
                //{
                    strSQL = " SELECT a.CouponID, a.CouponCode, a.CouponDesc, c.TransID, c.UserID, convert(datetime, c.TransDate, 109) as TransDate, d.CustomerName + ', ' + d.FIRSTNAME as CustName, b.TransDetailID, b.Qty, " +
                            " (b.ExtendedPrice)AS Amount, '" + this.dtpStartDate.Text + "' as StartDate, '" + this.dtpEndDate.Text + "' as EndDate " +
                            " FROM Coupon a " +
                            " INNER JOIN POSTransactionDetail b ON a.CouponID = b.CouponID " +
                            " INNER JOIN POSTransaction c ON b.TransID = c.TransID " +
                            " INNER JOIN Customer d ON c.CustomerID = d.CustomerID " +
                            " WHERE convert(datetime,c.TransDate,109) between convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime), 113) and convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime), 113) " + strFilter +
                            " ORDER BY " + strOrderBy;
                //}
                //else
                //{
                //    strSQL = "";
                //}
            }
            catch (Exception Ex)
            {
                strSQL = "";
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            PreviewReport(false);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.dtpStartDate.Focus();
            PreviewReport(true);
        }

        private void txtCoupon_KeyPress(object sender, KeyPressEventArgs e)
        {
            char Delete = (char)8;
            char comma = (char)',';
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != comma;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRptCouponReport_KeyDown(object sender, KeyEventArgs e)
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
                    if (this.txtCoupon.ContainsFocus == true)
                    {
                        this.SearchCouponId();
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdData_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdData.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdData.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdData.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdData.DisplayLayout.Rows)
                        {
                            orow.CollapseAll();
                        }
                        oRowUI.Row.ExpandAll();
                    }
                    oUI = oUI.Parent;
                }
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        private void grdData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            FormatGridColumn(grdData, 2);
        }

        private void FormatGridColumn(Infragistics.Win.UltraWinGrid.UltraGrid grd, int nBand)
        {
            for (int i = 0; i < nBand; i++)
            {
                for (int j = 0; j < grd.DisplayLayout.Bands[i].Columns.Count; j++)
                {
                    if (grd.DisplayLayout.Bands[i].Columns[j].DataType == typeof(System.Decimal))
                    {
                        grd.DisplayLayout.Bands[i].Columns[j].Format = "#######0.00";
                        grd.DisplayLayout.Bands[i].Columns[j].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    }
                }
            }
        }

        private void grdData_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void PreviewReport(bool blnPrint)
        {
            String strSQL;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptCouponSummary orptCouponSummary = new rptCouponSummary();
                GenerateSQL(out strSQL, false);
                DataSet ds = clsReports.GetReportSource(strSQL);
                orptCouponSummary.SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds; //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(blnPrint, orptCouponSummary);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
    }
}
