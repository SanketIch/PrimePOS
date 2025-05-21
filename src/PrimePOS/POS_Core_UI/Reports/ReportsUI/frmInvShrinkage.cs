using Infragistics.Win.UltraWinGrid;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
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

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Inventory Shrinkage Report
    /// </summary>
    /// 
    public partial class frmInvShrinkage : Form
    {
        public frmInvShrinkage()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            PreviewReport(false);
        }

        private void PreviewReport(bool blnPrint, bool bCalledFromScheduler = false)
        {
            String strSQL;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportClass oRpt = null;
                rptInvShrinkage orptInvShrinkage = new rptInvShrinkage();
                GenerateSQL(out strSQL, false);
                DataSet ds = clsReports.GetReportSource(strSQL);
                orptInvShrinkage.SetDataSource(ds.Tables[0]);
                clsReports.DStoExport = ds;
                clsReports.Preview(blnPrint, orptInvShrinkage);
            }
            catch(Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            finally 
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void GenerateSQL(out string strSQL, Boolean Flag)
        {
            string strItem = string.Empty, strDept = string.Empty;
            string strFilter = string.Empty, strPayType = string.Empty, strDisc = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(this.txtItemID.Text.Trim()) == false)
                    strFilter += " AND a.ItemID = '" + this.txtItemID.Text.Trim().Replace("'","''") + "' ";

                if (Convert.ToString(this.txtDepartment.Tag).Trim() != "")
                    strFilter += " AND a.DepartmentID IN (" + this.txtDepartment.Tag.ToString().Trim() + ")";
                
                strSQL = "SELECT DISTINCT a.ItemID, a.Description, ISNULL(a.QtyInStock,0) AS QtyInStock, ISNULL(Sales.SaleQty,0) AS SaleQty, ISNULL(ret.ReturnQty,0) AS ReturnQty, ISNULL(PhyInv.OldPhyInv,0) AS OldPhyInv, ISNULL(PhyInv.NewPhyInv,0) AS NewPhyInv, ISNULL(IR.InvRecQty,0) AS InvRecQty, ISNULL(IR1.InvRetQty,0) AS InvRetQty, '" +
                    this.dtpStartDate.Text + "' as StartDate, '" + this.dtpEndDate.Text + "' as EndDate FROM Item a " +
                    " INNER JOIN (SELECT bb.ItemID, SUM(bb.Qty) AS SaleQty FROM POSTransaction aa INNER JOIN POSTransactionDetail bb ON aa.TransID = bb.TransID " +
                                    " WHERE aa.TransType = 1 AND convert(datetime,aa.TransDate,109) between convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime),113) and convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime),113) GROUP BY bb.ItemID) Sales ON a.ItemID = Sales.ItemID " +
                    " LEFT JOIN (SELECT bb.ItemID, SUM(bb.Qty) AS ReturnQty FROM POSTransaction aa INNER JOIN POSTransactionDetail bb ON aa.TransID = bb.TransID "+
                                    " WHERE aa.TransType = 2 AND convert(datetime,aa.TransDate,109) between convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime),113) and convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime),113) GROUP BY bb.ItemID) Ret ON a.ItemID = Ret.ItemID " +
                    " LEFT JOIN (SELECT Itemcode, SUM(OldQty)AS OldPhyInv, SUM(NewQty) AS NewPhyInv FROM PhysicalInv " +
                                    " WHERE isProcessed = 1 AND convert(datetime,PTransDate,109) between convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime),113) and convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime),113) GROUP BY ItemCode) PhyInv ON PhyInv.Itemcode = a.ItemID " +
                    " LEFT JOIN (SELECT c.ItemID, SUM(c.Qty) InvRecQty FROM InventoryRecieved a INNER JOIN InvTransType b ON a.InvTransTypeID = b.ID INNER JOIN InvRecievedDetail c ON a.InvRecievedID = c.InvRecievedID " +
                                    " WHERE b.TransType = 0 AND convert(datetime,a.RecieveDate,109) between convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime),113) and convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime),113) GROUP BY c.ItemID) AS IR ON IR.ItemID = a.ItemID " +
                    " LEFT JOIN (SELECT c.ItemID, SUM(c.Qty) InvRetQty FROM InventoryRecieved a INNER JOIN InvTransType b ON a.InvTransTypeID = b.ID INNER JOIN InvRecievedDetail c ON a.InvRecievedID = c.InvRecievedID " +
                                    " WHERE b.TransType = 1 AND convert(datetime,a.RecieveDate,109) between convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime),113) and convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime),113) GROUP BY c.ItemID) AS IR1 ON IR.ItemID = a.ItemID " +
                    " WHERE 1=1" + strFilter + " ORDER BY a.ItemID";
            }
            catch (Exception Ex)
            {
                strSQL = "";
            }
        }

        private void txtItemID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem();
        }

        private void SearchItem()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Item_tbl);
                    this.txtItemID.Focus();
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
                        this.txtItemID.Text = oItemRow.ItemID;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtItemID.Text = String.Empty;
                    SearchItem();
                }
                catch (Exception exp)
                {
                    exp = null;
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.txtItemID.Text = String.Empty;
                    SearchItem();
                }
                #endregion
            }
        }

        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        private void SearchDept()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl;
                oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strDeptID = "";
                    string strDeptName = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strDeptID += ",'" + oRow.Cells["id"].Text + "'";
                            strDeptName += "," + oRow.Cells["Name"].Text;
                        }
                    }

                    if (strDeptID.StartsWith(","))
                    {
                        strDeptID = strDeptID.Substring(1);
                        strDeptName = strDeptName.Substring(1);
                    }
                    txtDepartment.Text = strDeptName;
                    txtDepartment.Tag = strDeptID;
                }
                else
                {
                    txtDepartment.Text = string.Empty;
                    txtDepartment.Tag = string.Empty;
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.dtpStartDate.Focus();
            PreviewReport(true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInvShrinkage_Load(object sender, EventArgs e)
        {
            this.dtpStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.dtpEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtItemID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.setColorSchecme(this);
            this.dtpStartDate.Value = DateTime.Now;
            this.dtpEndDate.Value = DateTime.Now;
            this.dtpStartDate.Focus();
        }

        private void frmInvShrinkage_KeyDown(object sender, KeyEventArgs e)
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
                    if (this.txtItemID.ContainsFocus == true)
                        this.SearchItem();
                    if (this.txtDepartment.ContainsFocus == true)
                        this.SearchDept();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
    }
}
