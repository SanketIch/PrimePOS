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
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmCreditCardProfiles : Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public frmCreditCardProfiles()
        {
            InitializeComponent();
        }

        private void frmCreditCardProfiles_Load(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("frmCreditCardProfiles_Load() - " + clsPOSDBConstants.Log_Entering);

                clsUIHelper.setColorSchecme(this);
                this.cboTokenDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.cboTokenDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.cboCardExpDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.cboCardExpDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.cboStatus.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.cboStatus.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.cboProcessor.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.cboProcessor.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.txtCustomer.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCustomer.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                ResetControls();
                PopulateProcessors();
                PopulateCardType();
                Search();               

                logger.Trace("frmCreditCardProfiles_Load() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmCreditCardProfiles_Load()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmCreditCardProfiles_KeyUp(object sender, KeyEventArgs e)
        {
            logger.Trace("frmCreditCardProfiles_KeyUp() - " + clsPOSDBConstants.Log_Entering);

            switch (e.KeyData)
            {
                case Keys.F4:
                    if (pnlSearch.Visible == true && pnlSearch.Enabled == true)
                    {
                        btnSearch_Click(sender, e);
                    }
                    break;
                case Keys.F9:
                    if (pnlDeleteExpiredTokens.Visible == true && pnlDeleteExpiredTokens.Enabled == true)
                    {
                        btnDeleteExpiredTokens_Click(sender, e);
                    }
                    break;
                case Keys.F10:
                    if (pnlDeleteSelectedTokens.Visible == true && pnlDeleteSelectedTokens.Enabled == true)
                    {
                        btnDeleteSelectedTokens_Click(sender, e);
                    }
                    break;
                default: break;
            }

            logger.Trace("frmCreditCardProfiles_KeyUp() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmCreditCardProfiles_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                logger.Trace("frmCreditCardProfiles_KeyDown() - " + clsPOSDBConstants.Log_Entering);
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {

                }
                else  //PRIMEPOS-2475 07-Jun-2018 JY Added
                {
                    if (e.Alt)
                        ShortCutKeyAction(e.KeyCode);
                }
                logger.Trace("frmCreditCardProfiles_KeyDown() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmCreditCardProfiles_KeyDown()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ResetControls()
        {
            txtCustomer.Text = "";
            txtCustomer.Tag = "";
            lblCustomerName.Text = "";
            txtLast4DigitsOfCC.Text = "";

            cboTokenDate.SelectedIndex = 0;
            cboCardExpDate.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            cboProcessor.SelectedIndex = 0;
            cboCardType.SelectedIndex = 0;

            this.dtpTokenDate1.Value = DateTime.Today;
            this.dtpTokenDate2.Value = DateTime.Today.Date.AddMonths(1);
            this.dtpCardExpDate1.Value = DateTime.Today;
            this.dtpCardExpDate2.Value = DateTime.Today.Date.AddMonths(1);
        }

        private void PopulateProcessors()
        {
            try
            {
                logger.Trace("PopulateProcessors() - " + clsPOSDBConstants.Log_Entering);
                cboProcessor.Items.Clear();
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                DataTable dtProcessor = oCCCustomerTokInfo.PopulateProcessors();
                if (Configuration.isNullOrEmptyDataTable(dtProcessor) == true)
                {
                    return;
                }
                this.cboProcessor.Items.Add("All", "All");
                foreach (DataRow oRow in dtProcessor.Rows)
                {
                    this.cboProcessor.Items.Add(oRow["Processor"].ToString(), oRow["Processor"].ToString());
                }
                cboProcessor.SelectedIndex = 0;
                logger.Trace("PopulateProcessors() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateLanguage()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void PopulateCardType()
        {
            try
            {
                logger.Trace("PopulateCardType() - " + clsPOSDBConstants.Log_Entering);
                cboCardType.Items.Clear();
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                DataTable dtCardType = oCCCustomerTokInfo.PopulateCardType();
                if (Configuration.isNullOrEmptyDataTable(dtCardType) == true)
                {
                    return;
                }
                this.cboCardType.Items.Add("All", "All");
                foreach (DataRow oRow in dtCardType.Rows)
                {
                    this.cboCardType.Items.Add(oRow["CardType"].ToString(), oRow["CardType"].ToString());
                }
                cboCardType.SelectedIndex = 0;
                logger.Trace("PopulateCardType() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateCardType()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void resizeColumns()
        {
            logger.Trace("resizeColumns() - " + clsPOSDBConstants.Log_Entering);
            grdSearch.Refresh();
            foreach (UltraGridColumn oCol in grdSearch.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }
            logger.Trace("resizeColumns() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void txtCustomer_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCustomer();
            }
            catch (Exception Ex) { }
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            try
            {
                string txtValue = this.txtCustomer.Text;
                if (txtValue.Trim() != "")
                {
                    FKEdit(txtValue, clsPOSDBConstants.Customer_tbl);
                }
                else
                {
                    this.txtCustomer.Tag = null;
                    this.lblCustomerName.Text = "";
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchCustomer()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedAcctNo();
                    if (strCode == "")
                    {
                        ClearCustomer();
                        return;
                    }

                    FKEdit(strCode, clsPOSDBConstants.Customer_tbl);
                    this.txtCustomer.Focus();
                }
                else
                {
                    ClearCustomer();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                ClearCustomer();
            }
        }

        private void ClearCustomer()
        {
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.txtCustomer.Tag = String.Empty;
            this.lblCustomerName.Text = String.Empty;
        }

        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Customer_tbl)
            {
                #region Customer
                try
                {
                    Customer oCustomer = new Customer();
                    CustomerData oCustomerData;
                    CustomerRow oCustomerRow = null;
                    oCustomerData = oCustomer.Populate(code);
                    oCustomerRow = oCustomerData.Customer[0];
                    if (oCustomerRow != null)
                    {
                        this.txtCustomer.Text = oCustomerRow.AccountNumber.ToString();
                        this.txtCustomer.Tag = oCustomerRow.CustomerId.ToString();
                        this.lblCustomerName.Text = oCustomerRow.CustomerFullName;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    SearchCustomer();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    SearchCustomer();
                }
                #endregion
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //IsCanceled = true;
            this.Close();
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            btnClose_Click(sender, e);
        }

        private void ShortCutKeyAction(Keys KeyCode)
        {
            switch (KeyCode)
            {
                case Keys.C:
                    if (pnlClose.Visible == true && pnlClose.Enabled == true)
                        btnClose_Click(btnClose, new EventArgs());
                    break;
                case Keys.L:
                    if (pnlClear.Visible == true && pnlClear.Enabled == true)
                        btnClear_Click(btnClear, new EventArgs());
                    break;
                default:
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void cboTokenDate_ValueChanged(object sender, EventArgs e)
        {
            if (this.cboTokenDate.Visible == false) return;

            if (this.cboTokenDate.SelectedItem.DataValue.ToString() == "All" || this.cboTokenDate.SelectedItem.DataValue.ToString() == "NULL" || this.cboTokenDate.SelectedItem.DataValue.ToString() == "NOT NULL")
            {
                this.dtpTokenDate1.Visible = false;
                this.dtpTokenDate2.Visible = false;
            }
            else if (this.cboTokenDate.SelectedItem.DataValue.ToString() == "=" || this.cboTokenDate.SelectedItem.DataValue.ToString() == ">" || this.cboTokenDate.SelectedItem.DataValue.ToString() == "<")
            {
                this.dtpTokenDate1.Visible = true;
                this.dtpTokenDate2.Visible = false;
            }
            else if (this.cboTokenDate.SelectedItem.DataValue.ToString() == "Between")
            {
                this.dtpTokenDate1.Visible = true;
                this.dtpTokenDate2.Visible = true;
            }
        }

        private void cboCardExpDate_ValueChanged(object sender, EventArgs e)
        {
            if (this.cboCardExpDate.Visible == false) return;

            if (this.cboCardExpDate.SelectedItem.DataValue.ToString() == "All" || this.cboCardExpDate.SelectedItem.DataValue.ToString() == "NULL" || this.cboCardExpDate.SelectedItem.DataValue.ToString() == "NOT NULL")
            {
                this.dtpCardExpDate1.Visible = false;
                this.dtpCardExpDate2.Visible = false;
            }
            else if (this.cboCardExpDate.SelectedItem.DataValue.ToString() == "=" || this.cboCardExpDate.SelectedItem.DataValue.ToString() == ">" || this.cboCardExpDate.SelectedItem.DataValue.ToString() == "<")
            {
                this.dtpCardExpDate1.Visible = true;
                this.dtpCardExpDate2.Visible = false;
            }
            else if (this.cboCardExpDate.SelectedItem.DataValue.ToString() == "Between")
            {
                this.dtpCardExpDate1.Visible = true;
                this.dtpCardExpDate2.Visible = true;
            }
        }

        private void Search()
        {
            try
            {
                chkSelectAll.Checked = false;
                if (Configuration.convertNullToInt(txtLast4DigitsOfCC.Text) > 0 && txtLast4DigitsOfCC.TextLength < 4)
                {
                    Resources.Message.Display("Please provide Last 4 Digits of CC", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (txtLast4DigitsOfCC.Enabled) this.txtLast4DigitsOfCC.Focus();
                    return;
                }

                logger.Trace("Search() - " + clsPOSDBConstants.Log_Entering);

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;                
                DataTable dt = new DataTable();
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                dt = oCCCustomerTokInfo.GetCreditCardProfiles(Configuration.convertNullToInt(txtCustomer.Tag), cboProcessor.Text, cboCardType.Text, Configuration.convertNullToInt(cboStatus.SelectedItem.DataValue), txtLast4DigitsOfCC.Text,
                    cboTokenDate.SelectedItem.DataValue.ToString(), Convert.ToDateTime(dtpTokenDate1.Value.ToString()), Convert.ToDateTime(dtpTokenDate2.Value.ToString()),
                    cboCardExpDate.SelectedItem.DataValue.ToString(), Convert.ToDateTime(dtpCardExpDate1.Value.ToString()), Convert.ToDateTime(dtpCardExpDate2.Value.ToString()));

                grdSearch.DataSource = dt;

                if (this.grdSearch.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Columns.Add("CHECK");
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "";
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                }
                this.grdSearch.DisplayLayout.Bands[0].Columns["Check"].Header.VisiblePosition = 0;                

                this.grdSearch.DisplayLayout.Bands[0].Columns["EntryID"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["IsActive"].Hidden = true;
                //this.grdSearch.DisplayLayout.Bands[0].Columns["Preference"].Width = 80;

                resizeColumns();

                this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Width = 35;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CustName"].Width = 150;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CardAlias"].Width = 95;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Preference"].Width = 75;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CardType"].Width = 100;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Last4"].Width = 40;
                this.grdSearch.DisplayLayout.Bands[0].Columns["ProfiledID"].Width = 200;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Processor"].Width = 75;
                this.grdSearch.DisplayLayout.Bands[0].Columns["EntryType"].Width = 70;
                this.grdSearch.DisplayLayout.Bands[0].Columns["TokenDate"].Width = 80;
                this.grdSearch.DisplayLayout.Bands[0].Columns["ExpDate"].Width = 80;
                this.grdSearch.DisplayLayout.Bands[0].Columns["IsFsaCard"].Width = 45;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CardStatus"].Width = 80;

                grdSearch.Focus();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSearch.Refresh();

                logger.Trace("Search() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Search()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            btnClear_Click(sender, e);
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

            if (grdSearch.Rows.Count > 0)
                chkSelectAll.Visible = true;
            else
                chkSelectAll.Visible = false;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            grdSearch.BeginUpdate();
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                oRow.Cells["check"].Value = chkSelectAll.Checked;
                oRow.Update();
            }
            grdSearch.EndUpdate();
        }

        private void grdSearch_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = System.Windows.Forms.Cursor.Position;
            point = this.grdSearch.PointToClient(point);
            Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
            if (oUI == null)
                return;

            while (oUI != null)
            {
                if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.CellUIElement))
                {
                    Infragistics.Win.UltraWinGrid.CellUIElement cellUIElement = (Infragistics.Win.UltraWinGrid.CellUIElement)oUI;
                    if (cellUIElement.Column.Key.ToUpper() == "CHECK")
                    {
                        CheckUncheckGridRow(cellUIElement.Cell);
                    }
                    break;
                }
                oUI = oUI.Parent;
            }
        }

        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            if ((bool)oCell.Value == false)
            {
                oCell.Value = true;
            }
            else
            {
                oCell.Value = false;
            }
            oCell.Row.Update();
        }

        private void btnDeleteSelectedTokens_Click(object sender, EventArgs e)
        {
            string sUserID = string.Empty;
            bool bDelete = UserPriviliges.getPermission(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteCreditCardProfiles.ID, 0, UserPriviliges.Screens.DeleteCreditCardProfiles.Name, out sUserID);
            if (bDelete == false)
            {
                Resources.Message.Display("Logged in user haven't enough permission to delete card profile.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                if (Configuration.convertNullToBoolean(oRow.Cells["check"].Value) == true)
                {
                    sb.Append(oRow.Cells["EntryID"].Value.ToString());
                    sb.Append(",");
                }
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length-1, 1);
            if (sb.Length > 0)
            { 
                if (Resources.Message.Display("Are you sure you want to delete a valid token? This token will not be available for future use once deleted.", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                    oCCCustomerTokInfo.DeleteTokens(sb);
                    Resources.Message.Display("Token(s) deleted successfully.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSearch_Click(sender, e);
                }
            }
            else
            {
                Resources.Message.Display("Please select row(s) to delete", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                grdSearch.Focus();
                return;
            }
        }

        private void lblDeleteSelectedTokens_Click(object sender, EventArgs e)
        {
            btnDeleteSelectedTokens_Click(sender, e);
        }

        private void btnDeleteExpiredTokens_Click(object sender, EventArgs e)
        {
            string sUserID = string.Empty;
            bool bDelete = UserPriviliges.getPermission(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteCreditCardProfiles.ID, 0, UserPriviliges.Screens.DeleteCreditCardProfiles.Name, out sUserID);
            if (bDelete == false)
            {
                Resources.Message.Display("Logged in user haven't enough permission to delete card profile.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ResetControls();
            cboStatus.SelectedIndex = 3;
            Search();
            if (Resources.Message.Display("Are you sure you want to delete all expired tokens? The deleted tokens will not be available for future.", "PrimePOS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                oCCCustomerTokInfo.DeleteExpiredTokens();
                Resources.Message.Display("Token(s) deleted successfully.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSearch_Click(sender, e);                
            }
        }

        private void lblDeleteExpiredTokens_Click(object sender, EventArgs e)
        {
            btnDeleteExpiredTokens_Click(sender, e);
        }
    }
}