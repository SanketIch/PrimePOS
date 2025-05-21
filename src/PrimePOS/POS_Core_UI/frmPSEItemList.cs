using CrystalDecisions.CrystalReports.Engine;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using POS_Core.BusinessRules;
using POS_Core_UI.Reports.Reports;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    public partial class frmPSEItemList : Form
    {
        public bool IsCanceled = true;
        private int CurrentX;
        private int CurrentY;

        public frmPSEItemList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Preview(false);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                grdSearch.DataSource = Search();
                grdSearch.Focus();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count.ToString();
                grdSearch.Refresh();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmPSEItemList_Load(object sender, EventArgs e)
        {
            try
            {
                this.btnAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.PSEItemList.ID, -999);
                this.btnEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.PSEItemList.ID, -998);

                clsUIHelper.SetAppearance(this.grdSearch);
                clsUIHelper.SetReadonlyRow(this.grdSearch);

                this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.txtItemName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtItemName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                FillGrid();
                if (this.grdSearch.Rows.Count == 0)
                {
                    this.txtItemCode.Focus();
                }
                clsUIHelper.setColorSchecme(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void FillGrid()
        {
            grdSearch.DataSource = Search();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count.ToString();
            this.resizeColumns();
            grdSearch.Refresh();
        }

        public DataSet Search()
        {
            DataSet oData = new DataSet();

            Search oSearch = new Search();
            string sSQL = "SELECT a.ProductId, a.ProductName, a.ProductNDC, ISNULL(a.ProductGrams,0.00) AS ProductGrams, ISNULL(a.ProductPillCnt,'') AS ProductPillCnt, Case When b.ItemID Is Null then 0 Else 1 End as ItemFound, ISNULL(a.IsActive,0) AS IsActive, RecordStatus " +
                        " FROM PSE_Items a LEFT JOIN Item b ON SUBSTRING(a.ProductID,1,11) = SUBSTRING(b.ItemID,1,11) " +
                        " WHERE 1=1 ";

            if (chkIsActive.Checked == true)
            {
                sSQL += " AND a.IsActive = 1 ";
            }

            if (chkItemFileItems.Checked == true)
            {
                sSQL += " AND b.ItemID IS NOT NULL ";
            }

            if (txtItemCode.Text.Trim().Length > 0)
            {
                sSQL += " AND a.ProductId LIKE '" + txtItemCode.Text.Replace("'", "''") + "%'";
            }

            if (txtItemName.Text.Trim().Length > 0)
            {
                sSQL += " AND a.ProductName LIKE '" + txtItemName.Text.Replace("'", "''") + "%'";
            }

            sSQL += " ORDER BY a.ProductName";

            oData = oSearch.SearchData(sSQL);

            return oData;
        }

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdSearch.DisplayLayout.Bands)
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

        private void frmPSEItemList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter && this.ActiveControl.Name != "grdSearch")
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmPSEItemList_Activated(object sender, EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void frmPSEItemList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.F3)
            {
            }
            else if (e.KeyData == System.Windows.Forms.Keys.F4)
            {
                btnSearch_Click(btnSearch, new EventArgs());
            }
        }

        private void grdSearch_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                this.grdSearch.DisplayLayout.Bands[0].HeaderVisible = false;
                this.grdSearch.DisplayLayout.Bands[0].Columns["ItemFound"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["ProductId"].Header.Caption = "Item Code";
                this.grdSearch.DisplayLayout.Bands[0].Columns["ProductName"].Header.Caption = "Item Description";
                this.grdSearch.DisplayLayout.Bands[0].Columns["ProductNDC"].Header.Caption = "NDC";
                this.grdSearch.DisplayLayout.Bands[0].Columns["ProductGrams"].Header.Caption = "Grams";
                this.grdSearch.DisplayLayout.Bands[0].Columns["ProductGrams"].Format = "######0.00";
                this.grdSearch.DisplayLayout.Bands[0].Columns["ProductPillCnt"].Header.Caption = "Pills";

                #region Record Count Summary
                //e.Layout.Override.AllowRowSummaries = AllowRowSummaries.True;
                //UltraGridColumn columnToSummarize = e.Layout.Bands[0].Columns["ProductName"];
                //SummarySettings summary = e.Layout.Bands[0].Summaries.Add("Count", SummaryType.Count, columnToSummarize);
                ////SummarySettings summary1 = new SummarySettings("", SummaryType.Count, null, "ProductId", 0, true, "Band 0", 0, SummaryPosition.Left, null, -1, false);
                //summary.DisplayFormat = "Record(s) Count = {0}";
                //summary.Appearance.TextHAlign = HAlign.Left;
                //e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
                #endregion
            }
            catch (Exception) { }
        }

        private void grdSearch_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void Preview(bool PrintIt)
        {
            try
            {
                ReportClass oRpt = null;
                DataSet oDS = Search();
                oRpt = new rptPSEItemList();
                POS_Core_UI.Reports.ReportsUI.clsReports.Preview(PrintIt, oDS, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                FrmPSEItem oFrmPSEItem = new FrmPSEItem();
                oFrmPSEItem.Initialize();
                oFrmPSEItem.ShowDialog(this);
                if (!oFrmPSEItem.IsCanceled)
                {
                    grdSearch.DataSource = Search();
                    grdSearch.Focus();
                    grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                    sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count.ToString();
                    grdSearch.Refresh();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                Edit();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Edit()
        {
            FrmPSEItem oFrmPSEItem = new FrmPSEItem();
            oFrmPSEItem.Edit(grdSearch.ActiveRow.Cells[0].Text);
            oFrmPSEItem.ShowDialog(this);
            if (!oFrmPSEItem.IsCanceled)
            {
                grdSearch.DataSource = Search();
                grdSearch.Focus();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count.ToString();
                grdSearch.Refresh();
            }
        }

        private void grdSearch_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            Edit();
        }
    }
}
