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
using POS_Core.CommonData;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
using Resources;
using System.Data.SqlClient;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;

namespace POS_Core_UI
{
    public partial class frmSetItemTax : Form
    {
        #region local variables
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        DataSet ds = new DataSet();
        private bool m_exceptionAccoured = false;
        #endregion
        public frmSetItemTax()
        {
            InitializeComponent();
        }

        private void frmSetItemTax_Load(object sender, EventArgs e)
        {
            logger.Trace("frmTriPOSSettings_Load() - " + clsPOSDBConstants.Log_Entering);
            clsUIHelper.SetKeyActionMappings(this.grdSearch);
            this.grdSearch.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Tab, UltraGridAction.NextCellByTab, 0, UltraGridState.InEdit, 0, 0));
            this.grdSearch.DisplayLayout.TabNavigation = TabNavigation.NextCell;
            this.ApplyGrigFormat();

            #region not in use
            //this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //txtDepartment.Tag = "";
            //this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //this.txtItemDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtItemDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //opnMode.Value = "0";
            //grdItems.DisplayLayout.Bands[0].Columns["Check Items"].Header.Caption = "";
            //grdItems.DisplayLayout.Bands[0].Columns["Check Items"].Header.VisiblePosition = 0;
            #endregion

            this.txtTaxCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTaxCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            clsUIHelper.setColorSchecme(this);            
            txtTaxCode.Focus();
            logger.Trace("frmTriPOSSettings_Load() - " + clsPOSDBConstants.Log_Exiting);
        }

        #region not in use
        private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchDept();
        }

        private void SearchDept()
        {
            frmSearchMain oSearch = new frmSearchMain(true);
            oSearch.SearchTable = clsPOSDBConstants.Department_tbl;
            oSearch.ShowDialog();
            if (!oSearch.IsCanceled)
            {
                if (oSearch.grdSearch.ActiveRow != null && oSearch.grdSearch.ActiveRow.Cells.Count > 0)
                {
                    txtDepartment.Text = oSearch.grdSearch.ActiveRow.Cells["Code"].Value.ToString().Trim();
                    txtDepartment.Tag = oSearch.grdSearch.ActiveRow.Cells["id"].Value.ToString().Trim();
                }
                if (txtDepartment.Text == "")
                    return;
            }
            else
            {
                txtDepartment.Text = "";
                txtDepartment.Tag = "";
            }
        }

        private void opnMode_ValueChanged(object sender, EventArgs e)
        {
            if (Configuration.convertNullToInt(opnMode.Value) == 0)
            {
                txtDepartment.Enabled = true;
                txtItemCode.Enabled = false;
                txtItemDescription.Enabled = false;
                txtDepartment.Focus();
            }
            else
            {
                txtDepartment.Enabled = false;
                txtItemCode.Enabled = true;
                txtItemDescription.Enabled = true;
                txtItemCode.Focus();
            }
        }

        private void btnShowItems_Click(object sender, EventArgs e)
        {
            try
            {
                bool bStatus = false;
                if (Configuration.convertNullToInt(opnMode.Value) == 0)
                {
                    if (txtDepartment.Text.Trim() != "")
                        bStatus = ShowData(true);
                    else
                    {
                        Resources.Message.Display("Please select department", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClear_Click(null, null);
                    }
                }
                else
                {
                    if (txtItemCode.Text.Trim() != "" || txtItemDescription.Text.Trim() != "")
                        bStatus = ShowData(false);
                    else
                    {
                        Resources.Message.Display("Please enter ItemID/Description", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnClear_Click(null, null);
                    }
                }

                if (bStatus)
                {
                    int rowIndex = 0;
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oRow in ds.Tables[0].Rows)
                        {
                            this.grdSearch.Rows[rowIndex].Cells["Check Items"].Value = false;
                            rowIndex++;
                        }
                        UpdateNewTaxCodeInGrid();
                        ApplyGrigFormat();
                        chkSelectAll.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private bool ShowData(bool byDepatment)
        {
            try
            {
                string strSQL = string.Empty;
                IDbCommand cmd = DataFactory.CreateCommand();

                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    try
                    {
                        strSQL = "SELECT DISTINCT '' AS [Check Items], a.ItemID, a.Description, STUFF((SELECT ', ' + CAST(bb.TaxCode AS varchar(100)) FROM ItemTax aa INNER JOIN TaxCodes bb ON aa.TaxID = bb.TaxID WHERE aa.EntityID = a.ItemID FOR XML PATH('')),1,1,'') AS [Tax Codes], '' AS NewTaxId, '' AS [New Tax Codes] " +
                                    " FROM Item a LEFT JOIN ItemTax b ON a.ItemID = b.EntityID " +
                                    " WHERE 1 = 1 ";

                        if (byDepatment)
                        {
                            strSQL += " AND a.DepartmentID = '" + txtDepartment.Tag.ToString().Trim() + "'";
                        }
                        else
                        {
                            if (txtItemCode.Text.Trim() != "")
                            {
                                strSQL += " AND a.ItemID LIKE '" + txtItemCode.Text.Trim().Replace("'", "''") + "%'";
                            }
                            if (txtItemDescription.Text.Trim() != "")
                            {
                                strSQL += " AND a.Description LIKE '%" + txtItemDescription.Text.Trim().Replace("'", "''") + "%'";
                            }
                        }
                        strSQL += " ORDER BY a.Description";

                        ds = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        int RowCount = ds.Tables[0].Rows.Count;
                        grdSearch.DataSource = ds;
                    }
                    catch (Exception exp)
                    {
                        throw (exp);
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return true;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            grdSearch.BeginUpdate();
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                oRow.Cells["Check Items"].Value = chkSelectAll.Checked;
                oRow.Update();
            }
            grdSearch.EndUpdate();
        }
        #endregion

        private void frmSetItemTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyData == System.Windows.Forms.Keys.Enter)
                //{
                //    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                //}
                if (e.KeyData == Keys.Escape)
                    this.Close();
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtTaxCode.ContainsFocus == true)
                    {
                        txtTaxCode_EditorButtonClick(null, null);
                    }
                }
                //else if (e.KeyData == System.Windows.Forms.Keys.F4)
                //{
                //    if (this.txtDepartment.ContainsFocus == true)
                //    {
                //        this.SearchDept();
                //    }
                //}
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtTaxCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchTaxCode();
            UpdateNewTaxCodeInGrid();
        }

        private void UpdateNewTaxCodeInGrid()
        {
            if (txtTaxCode.Text != "")
            {
                if (this.grdSearch != null && this.grdSearch.Rows.Count > 0)
                {

                    for (int i = 0; i < this.grdSearch.Rows.Count; i++)
                    {
                        this.grdSearch.Rows[i].Cells[3].Value = txtTaxCode.Tag.ToString();
                        this.grdSearch.Rows[i].Cells[4].Value = txtTaxCode.Text.ToString();
                    }
                }
            }
        }

        private void SearchTaxCode()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);
                oSearch.SearchTable = clsPOSDBConstants.TaxCodes_tbl;
                oSearch.SearchInConstructor = true;

                if (Configuration.CPOSSet.SelectMultipleTaxes)
                    oSearch.AllowMultiRowSelect = true;
                else
                    oSearch.AllowMultiRowSelect = false;

                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    if (!Configuration.CPOSSet.SelectMultipleTaxes)
                    {
                        if (oSearch.grdSearch.ActiveRow != null && oSearch.grdSearch.ActiveRow.Cells.Count > 0)
                        {
                            txtTaxCode.Text = oSearch.grdSearch.ActiveRow.Cells["Code"].Value.ToString().Trim();
                            txtTaxCode.Tag = oSearch.grdSearch.ActiveRow.Cells["TaxID"].Value.ToString().Trim();
                        }
                    }
                    else
                    {
                        string strTaxID = string.Empty;
                        string strTaxCode = string.Empty;
                        foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                        {
                            if ((bool)oRow.Cells["check"].Value == true)
                            {
                                if (strTaxID == string.Empty)
                                {
                                    strTaxID = oRow.Cells["TaxID"].Text;
                                    strTaxCode = oRow.Cells["Code"].Text.Trim();
                                }
                                else
                                {
                                    strTaxID += "," + oRow.Cells["TaxID"].Text;
                                    strTaxCode += "," + oRow.Cells["Code"].Text.Trim();
                                }
                            }
                        }
                        txtTaxCode.Text = strTaxCode;
                        txtTaxCode.Tag = strTaxID;
                    }

                    if (txtTaxCode.Text == "")
                        return;
                }
                else
                {
                    txtTaxCode.Text = string.Empty;
                    txtTaxCode.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }       

        private void ApplyGrigFormat()
        {
            clsUIHelper.SetAppearance(this.grdSearch);

            grdSearch.DisplayLayout.Bands[0].Columns[3].Hidden = true;

            grdSearch.DisplayLayout.Bands[0].Columns[0].CellActivation = Activation.AllowEdit;
            grdSearch.DisplayLayout.Bands[0].Columns[1].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns[2].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns[3].CellActivation = Activation.NoEdit;
            grdSearch.DisplayLayout.Bands[0].Columns[4].CellActivation = Activation.NoEdit;
        }

        #region grid events
        private void grdSearch_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridCell oCurrentCell;
            oCurrentCell = this.grdSearch.ActiveCell;
            try
            {
                if (oCurrentCell.DataChanged == false)
                    return;
            }
            catch (Exception ex)
            {
            }
            try
            {
                if (oCurrentCell.Column.Key == clsPOSDBConstants.Item_Fld_ItemID && oCurrentCell.Value.ToString() != "")
                {
                    FKEdit(oCurrentCell.Value.ToString(), clsPOSDBConstants.Item_tbl);
                    if (oCurrentCell.Value.ToString() == "")
                    {
                        e.Cancel = true;
                        this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
            }
            catch (Exception exp)
            {
                m_exceptionAccoured = true;
                clsUIHelper.ShowErrorMsg(exp.Message);
                e.Cancel = true;
                this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            }
        }

        private void grdSearch_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell;
            oCurrentRow = this.grdSearch.ActiveRow;
            oCurrentCell = null;
            bool blnCellChanged;
            blnCellChanged = false;

            foreach (UltraGridCell oCell in oCurrentRow.Cells)
            {
                if (oCell.DataChanged == true || oCell.Text.Trim() != "")
                {
                    blnCellChanged = true;
                    break;
                }
            }

            if (blnCellChanged == false)
            {
                return;
            }
            try
            {
                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.Item_Fld_ItemID];
                Validate_ItemID(oCurrentCell.Text.ToString());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    m_exceptionAccoured = true;
                    e.Cancel = true;
                    this.grdSearch.ActiveCell = oCurrentCell;
                    this.grdSearch.PerformAction(UltraGridAction.ActivateCell);
                    this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);

                }
            }
        }

        private void grdSearch_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (m_exceptionAccoured)
                {
                    m_exceptionAccoured = false;
                    return;
                }

                if (e.Cell.Column.Key == clsPOSDBConstants.Item_Fld_ItemID)
                    SearchItem();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdSearch_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            SummarySettings summary = new SummarySettings();
            UltraGridColumn columnToSummarize = e.Layout.Bands[0].Columns[0];
            try
            {
                summary = e.Layout.Bands[0].Summaries.Add("Record(s) Count = ", SummaryType.Count, columnToSummarize);
            }
            catch { }
            summary.DisplayFormat = "Record(s) Count = {0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Left;
            summary.SummaryPosition = SummaryPosition.Left;
            summary.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;
            e.Layout.Bands[0].Summaries[0].SummaryPositionColumn = columnToSummarize;
            e.Layout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.BottomFixed;

            e.Layout.Override.SummaryFooterAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.BackColor = Color.Silver;
            e.Layout.Override.SummaryValueAppearance.ForeColor = Color.Maroon;
            e.Layout.Override.SummaryValueAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

            e.Layout.Override.SummaryFooterSpacingAfter = 5;
            e.Layout.Override.SummaryFooterSpacingBefore = 5;
        }

        private void grdSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.grdSearch.ContainsFocus == true)
                    {
                        if (this.grdSearch.ActiveCell != null)
                        {
                            if (this.grdSearch.ActiveCell.Column.Key == clsPOSDBConstants.PODetail_Fld_ItemID)
                                this.SearchItem();
                        }
                    }
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F2)
                {
                    AddRow();

                }
                e.Handled = true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Item_tbl)
            {
                #region Items
                try
                {
                    Item oItem = new Item();
                    ItemData oItemData;
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(code);
                    if (oItemData.Tables[0].Rows.Count == 0)
                    {
                        if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998))
                        {
                            frmItems ofrmItem = new frmItems(code);
                            ofrmItem.numQtyInStock.ReadOnly = true;
                            ofrmItem.numQtyInStock.Enabled = false;
                            ofrmItem.ShowDialog();
                            oItemData = oItem.Populate(code);
                        }
                    }
                    oItemRow = oItemData.Item[0];
                    if (oItemRow != null)
                    {
                        if (grdSearch.ActiveRow == null)
                            this.grdSearch.Rows.Band.AddNew();
                        this.grdSearch.ActiveCell.Value = oItemRow.ItemID;
                        this.grdSearch.ActiveRow.Cells[1].Value = oItemRow.Description;
                        string strTaxCodes;
                        GetItemDetails(oItemRow.ItemID, out strTaxCodes);
                        this.grdSearch.ActiveRow.Cells[2].Value = strTaxCodes;
                        this.grdSearch.ActiveRow.Cells[3].Value = Configuration.convertNullToString(txtTaxCode.Tag);
                        this.grdSearch.ActiveRow.Cells[4].Value = Configuration.convertNullToString(txtTaxCode.Text);
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.grdSearch.ActiveCell.Value = String.Empty;
                    this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.grdSearch.ActiveCell.Value = String.Empty;
                    this.grdSearch.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                #endregion
            }
        }

        public void Validate_ItemID(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_ItemCodeCanNotNull);
            }
            else
            {
                ItemData oID = new ItemData();
                Item oI = new Item();
                oID = oI.Populate(strValue);
                if (oID == null)
                {
                    throw (new Exception("Invalid Item code"));
                }
                else if (oID.Item.Rows.Count == 0)
                {
                    throw (new Exception("Invalid Item code"));
                }
            }
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
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddRow()
        {
            try
            {
                this.grdSearch.Focus();
                this.grdSearch.PerformAction(UltraGridAction.LastCellInGrid);
                this.grdSearch.PerformAction(UltraGridAction.FirstCellInRow);
                this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
            }
            catch (Exception) { }
        }

        private void GetItemDetails(string strItemCode, out string strTaxCodes)
        {
            strTaxCodes = string.Empty;
            string strSQL = string.Empty;
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    try
                    {
                        strSQL = "SELECT DISTINCT a.ItemID, a.Description, " +
                                    " STUFF((SELECT ', ' + CAST(bb.TaxCode AS varchar(100)) FROM ItemTax aa INNER JOIN TaxCodes bb ON aa.TaxID = bb.TaxID WHERE aa.EntityType = 'I' AND aa.EntityID = a.ItemID FOR XML PATH('')),1,1,'') AS TaxCodes " +
                                    " FROM Item a LEFT JOIN ItemTax b ON a.ItemID = b.EntityID " +
                                    " WHERE 1 = 1 ";

                        if (strItemCode != "")
                        {
                            strSQL += " AND a.ItemID = '" + strItemCode.Trim().Replace("'", "''") + "'";
                        }

                        ds = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            strTaxCodes = ds.Tables[0].Rows[0]["TaxCodes"].ToString().Trim();
                        }
                    }
                    catch (Exception exp)
                    {
                        throw (exp);
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnApplyNewTax_Click(object sender, EventArgs e)
        {
            try
            {
                string strTaxIds = Configuration.convertNullToString(txtTaxCode.Tag).Trim();
                if (strTaxIds != string.Empty)
                {
                    if (grdSearch != null && grdSearch.Rows.Count > 0)
                    {
                        string strItemIds = string.Empty;
                        for (int i = 0; i < grdSearch.Rows.Count; i++)
                        {
                            if (Configuration.convertNullToString(this.grdSearch.Rows[i].Cells["ItemID"].Value) != "")
                            {
                                if (strItemIds == string.Empty)
                                    strItemIds = "'" + Configuration.convertNullToString(this.grdSearch.Rows[i].Cells["ItemID"].Value).Replace("'", "''") + "'";
                                else
                                    strItemIds += ",'" + Configuration.convertNullToString(this.grdSearch.Rows[i].Cells["ItemID"].Value).Replace("'", "''") + "'";
                            }
                        }
                        if (strItemIds != string.Empty)
                        {
                            Item oItem = new Item();
                            oItem.UpdateItemTax(strItemIds, strTaxIds);
                            Resources.Message.Display("Tax assigned successfully for grid items.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnClear_Click(null, null);
                        }
                        else
                        {
                            Resources.Message.Display("Please select item(s) from the list", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        Resources.Message.Display("Please add items to the list", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    Resources.Message.Display("Please select tax", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //opnMode.Value = "0";
                //txtDepartment.Text = "";
                //txtDepartment.Tag = "";
                //txtItemCode.Text = "";
                //txtItemDescription.Text = "";                
                //chkSelectAll.Checked = false;

                for (int i = grdSearch.Rows.Count - 1; i >= 0; i--)
                    grdSearch.Rows[i].Delete(false);
                
                grdSearch.Refresh();
                ApplyGrigFormat();
                txtTaxCode.Text = "";
                txtTaxCode.Tag = "";

                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnRemoveTax_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdSearch != null && grdSearch.Rows.Count > 0)
                {
                    if (Resources.Message.Display("Are you sure, you want to remove tax codes for items?", "Remove Tax Codes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        string strItemIds = string.Empty;
                        for (int i = 0; i < grdSearch.Rows.Count; i++)
                        {
                            if (Configuration.convertNullToString(this.grdSearch.Rows[i].Cells["ItemID"].Value) != "")
                            {
                                if (strItemIds == string.Empty)
                                    strItemIds = "'" + Configuration.convertNullToString(this.grdSearch.Rows[i].Cells["ItemID"].Value).Replace("'", "''") + "'";
                                else
                                    strItemIds += ",'" + Configuration.convertNullToString(this.grdSearch.Rows[i].Cells["ItemID"].Value).Replace("'", "''") + "'";
                            }
                        }
                        if (strItemIds != string.Empty)
                        {
                            Item oItem = new Item();
                            oItem.UpdateItemTax(strItemIds, "");
                            Resources.Message.Display("Tax removed successfully for grid items.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnClear_Click(null, null);
                        }
                        else
                        {
                            Resources.Message.Display("Please select item(s) from the list", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                else
                {
                    Resources.Message.Display("Please add items to the list", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }       
    }
}
