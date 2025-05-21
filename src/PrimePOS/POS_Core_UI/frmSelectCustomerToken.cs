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
using POS_Core.UserManagement;
//using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using NLog;

namespace POS_Core_UI
{
    public partial class frmSelectCustomerToken : Form
    {
        private CustomerData oCustData = null;
        private CCCustomerTokInfoData oCustomerCCProfileTokenData = null;
        public bool bIsCancelled = true;
        private ILogger logger = LogManager.GetCurrentClassLogger();

        //public //
        public frmSelectCustomerToken()
        {
            InitializeComponent();
        }

        public frmSelectCustomerToken(CustomerData _CustData)
        {
            InitializeComponent();

            oCustData = _CustData;

            this.SetTokenData();
            this.BindGrid();

        }

        private void frmSelectCustomerToken_Load(object sender, EventArgs e)
        {
            try
            {
                clsUIHelper.setColorSchecme(this);

                SetTitle();

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        public void SetCustomer(CustomerData _CustData)
        {
            oCustData = _CustData;
        }

        private void SetTitle()
        {

            if (oCustData != null && oCustData.Tables.Count > 0 && oCustData.Tables[0].Rows.Count > 0)
            {
                CustomerRow oCustRow = oCustData.Customer[0];
                this.Text = "Select Credit Card Profile for : " + oCustRow.CustomerFullName;
            }
            else
            {
                this.Text = "Select Credit Card Profile";
            }
        }

        private void SetTokenData()
        {
            if (oCustData != null && oCustData.Tables.Count > 0 && oCustData.Tables[0].Rows.Count > 0)
            {
                CustomerRow oCurrentCustRow = oCustData.Customer[0];
                //string sPatientno = oCurrentCustRow.CustomerId
                int iPatno = oCurrentCustRow.CustomerId;

                CCCustomerTokInfo tokinfo = new CCCustomerTokInfo();
                if(iPatno>0)
                oCustomerCCProfileTokenData = tokinfo.GetTokenByCustomerandProcessor(iPatno);
            }
        }
        private void BindGrid()
        {
            if (oCustomerCCProfileTokenData != null && oCustomerCCProfileTokenData.Tables.Count > 0 && oCustomerCCProfileTokenData.Tables[0].Rows.Count > 0)
            {
                grdTokenList.DataSource = oCustomerCCProfileTokenData;

                grdTokenList.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID].Hidden = true;    //PRIMEPOS-2635 04-Feb-2019 JY Added
                grdTokenList.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID].Hidden = true;
                grdTokenList.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID].Hidden = true;

                #region PRIMEPOS-2635 04-Feb-2019 JY Added for Preference                
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                UltraDropDown oUltraDropDown = new UltraDropDown();
                oUltraDropDown.SetDataBinding(oCCCustomerTokInfo.GetCardPreferences(), null);
                oUltraDropDown.ValueMember = "Id";
                oUltraDropDown.DisplayMember = "PreferenceDesc";
                oUltraDropDown.DisplayLayout.Bands[0].ColHeadersVisible = false;
                oUltraDropDown.DisplayLayout.Bands[0].Columns[0].Hidden = true;

                grdTokenList.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].ValueList = oUltraDropDown;
                grdTokenList.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].Width = 105;
                grdTokenList.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].Header.Caption = "Preference";
                grdTokenList.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                #endregion

                if (oCustomerCCProfileTokenData.Tables[0].Columns.Contains("ExpDate"))
                    grdTokenList.DisplayLayout.Bands[0].Columns["ExpDate"].Format = "MM/yyyy";

                if (grdTokenList.Rows.Count > 0)
                {
                    this.grdTokenList.ActiveRow = this.grdTokenList.Rows[0];
                    #region PRIMEPOS-2687 31-Jan-2021 JY Added
                    try
                    {
                        for (int i = 0; i < grdTokenList.Rows.Count; i++)
                        {
                            if (grdTokenList.Rows[i].Cells["ExpDate"].Value != null && grdTokenList.Rows[i].Cells["ExpDate"].Value.ToString() != "")
                            {
                                if (Convert.ToDateTime(grdTokenList.Rows[i].Cells["ExpDate"].Value).Year < DateTime.Now.Year ||
                                    (Convert.ToDateTime(grdTokenList.Rows[i].Cells["ExpDate"].Value).Year == DateTime.Now.Year && Convert.ToDateTime(grdTokenList.Rows[i].Cells["ExpDate"].Value).Month < DateTime.Now.Month))
                                {
                                    grdTokenList.Rows[i].Appearance.BackColor = Color.Red;
                                }
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        logger.Fatal(Ex, "BindGrid()");
                    }
                    #endregion
                    grdTokenList.Focus();
                }

                SetGridProperties();
                grdTokenList.PerformAction(UltraGridAction.FirstRowInGrid);
                grdTokenList.Refresh();

            }
        }

        private void SetGridProperties()
        {
            grdTokenList.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None;
            grdTokenList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            grdTokenList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            
            ResizeColumns();
        }   

        private void frmSelectCustomerToken_Activated(object sender, EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        public string SelectedCCProfileTokenID()
        {
            try
            {
                if(grdTokenList.ActiveRow!=null && grdTokenList.ActiveRow.Cells.Count>0)
                {
                    return grdTokenList.ActiveRow.Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID].Text;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public string SelectedCCProfileEntryID()
        {
            try
            {
                if (grdTokenList.ActiveRow != null && grdTokenList.ActiveRow.Cells.Count > 0)
                {
                    return grdTokenList.ActiveRow.Cells[clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID].Text;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public CCCustomerTokInfoRow SelectedRow()
        {
            int iSelectedRowId = POS_Core.Resources.Configuration.convertNullToInt(this.SelectedCCProfileEntryID());
            CCCustomerTokInfoRow oCusttRow = null;
            try
            {
                foreach(CCCustomerTokInfoRow orow in oCustomerCCProfileTokenData.Tables[0].Rows)
                {
                    if (orow.EntryID == iSelectedRowId)
                    {
                        oCusttRow = orow;
                        break;
                    }
                }

                /*if (grdTokenList.ActiveRow != null && grdTokenList.ActiveRow.Cells.Count > 0)
                {
                   
                }
                else
                {
                    //return string.Empty;
                }*/
            }
            catch
            {
                //return string.Empty;
            }

            return oCusttRow;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bIsCancelled = false;
            this.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bIsCancelled = true;
            this.Close();
        }

        private void ResizeColumns()
        {
            grdTokenList.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            foreach(UltraGridColumn oCol in grdTokenList.DisplayLayout.Bands[0].Columns)
            {
                //oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true);

                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }

            }
        }

        private void grdTokenList_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void grdTokenList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point _point = Cursor.Position;
                _point = this.grdTokenList.PointToClient(_point);
                Infragistics.Win.UIElement oUI = this.grdTokenList.DisplayLayout.UIElement.ElementFromPoint(_point);

                if (oUI == null)
                {
                    return;
                }

                while (oUI != null)
                {
                    if(oUI.GetType()==typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        if (grdTokenList.Rows.Count == 0)
                        {
                            return;
                        }

                        bIsCancelled = false;
                        this.Close();
                    }
                    oUI = oUI.Parent;
                }

            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        #region 
        //PRIMEPOS-2497 Jenny Added
        private void frmSelectCustomerToken_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                bIsCancelled = true;
                this.Close();
            }
        }

        private void btnOk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                bIsCancelled = true;
                this.Close();
            }
        }

        private void btnCancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                bIsCancelled = true;
                this.Close();
            }
        }

        private void grdTokenList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                bIsCancelled = true;
                this.Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (grdTokenList.Selected != null && grdTokenList.Selected.Rows.Count > 0)
                {
                    bIsCancelled = false;
                    this.Close();
                }
            }
        }
        #endregion
    }
}
