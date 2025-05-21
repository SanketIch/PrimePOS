using Evertech;
using Infragistics.Win.UltraWinGrid;
using NLog;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.Resources;
using POS_Core.Resources.DelegateHandler;
using POS_Core.TransType;
using POS_Core_UI.Reports.Reports;
using POS_Core_UI.Reports.ReportsUI;
using PossqlData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmViewAuditTrail : System.Windows.Forms.Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        AuditTrail objAuditTrail = null;
        DataTable dtEntityName = new DataTable();
        DataTable dtUser = new DataTable();
        DataTable dtApplicationName = new DataTable();
        DataSet dsCcTransmissionDetail = new DataSet();
        public frmViewAuditTrail()
        {
            InitializeComponent();
        }

        #region FORM EVENT

        private void frmViewAuditTrail_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
                this.dtpFromDate.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.dtpToDate.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                LoadControlValues();
                Search();
                //Insert(); //Added by Durgesh
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
                logger.Fatal(ex, "Search()");
            }
        }

        private void frmViewAuditTrail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                ShortCutKeyAction(e.KeyCode);
            }
            else if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void frmViewAuditTrail_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F4:
                    btnSearch_Click_1(null, null);
                    break;
            }
        }

        #endregion

        #region Methods
        private void Search()
        {
            //DataSet dsCcTransmissionDetail = new DataSet();
            objAuditTrail = new AuditTrail();
            try
            {
                DateTime dtFrom = DateTime.Parse(this.dtpFromDate.Value.ToString());
                DateTime dtTo = DateTime.Parse(this.dtpToDate.Value.ToString());

                if (dtFrom <= dtTo)
                {
                    dsCcTransmissionDetail = objAuditTrail.getAuditTrailLog(dtFrom, dtTo, cboEntityName.SelectedItem.DataValue.ToString(), cboApplicationName.SelectedItem.DataValue.ToString(), cboUser.SelectedItem.DataValue.ToString());
                    if (dsCcTransmissionDetail?.Tables?.Count > 0)
                    {
                        grdAuditLog.DataSource = dsCcTransmissionDetail.Tables[0];
                        grdAuditLog.DataBind();

                        GridColumnDisplay();
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("'From Date' should not be greater than 'To Date'");
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "Search()");
            }
        }
        //private void Insert()
        //{
        //    try
        //    {
        //        objAuditTrail = new AuditTrail();
        //        objAuditTrail.InsertAuditTrail();
        //    }
        //    catch(Exception Ex)
        //    {
        //        clsUIHelper.ShowErrorMsg(Ex.Message);
        //    }
        //}
        private void GridColumnDisplay()
        {
            if (grdAuditLog != null && grdAuditLog.Rows.Count > 0)
            {

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["EntityName"].Header.VisiblePosition = 0;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["EntityName"].Header.Caption = "Entity Name";

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["FieldChanged"].Header.VisiblePosition = 1;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["FieldChanged"].Header.Caption = "Field Changed";

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["OldValue"].Header.VisiblePosition = 2;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["OldValue"].Header.Caption = "Old Value";

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["NewValue"].Header.VisiblePosition = 3;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["NewValue"].Header.Caption = "New Value";

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["DateChanged"].Header.VisiblePosition = 4;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["DateChanged"].Header.Caption = "Modified Date";
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["DateChanged"].Format = "MM/dd/yyyy hh:mm tt";

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["ActionBy"].Header.VisiblePosition = 5;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["ActionBy"].Header.Caption = "Action By";

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["Operation"].Header.VisiblePosition = 6;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["Operation"].Header.Caption = "Operation";

                this.grdAuditLog.DisplayLayout.Bands[0].Columns["ApplicationName"].Header.VisiblePosition = 7;
                this.grdAuditLog.DisplayLayout.Bands[0].Columns["ApplicationName"].Header.Caption = "Application Name";

                //this.grdAuditLog.DisplayLayout.Bands[0].Columns["SQLUser"].Header.VisiblePosition = 8;
                //this.grdAuditLog.DisplayLayout.Bands[0].Columns["SQLUser"].Header.Caption = "SQLUser";

                ResizeColumns();

            }
            else
            {
                // pnlAdd.Visible = false;               
            }
        }
        private void ResizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdAuditLog.DisplayLayout.Bands)
                {
                    foreach (UltraGridColumn oCol in oBand.Columns)
                    {
                        oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 28;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception) { }
        }
        private void Clear()
        {
            try
            {
                this.dtpFromDate.Value = DateTime.Now.AddDays(-1);
                this.dtpToDate.Value = DateTime.Now;
                this.ActiveControl = this.dtpFromDate;
                LoadControlValues();
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "Clear()");
            }
        }
        private void ShortCutKeyAction(Keys KeyCode)
        {
            switch (KeyCode)
            {
                case Keys.C:
                    btnClose_Click_1(btnClose, new EventArgs());
                    break;
                case Keys.L:
                    btnClear_Click_1(btnClear, new EventArgs());
                    break;
                default:
                    break;
            }
        }
        private List<DropDown> GetEntityName()
        {
            objAuditTrail = new AuditTrail();
            List<DropDown> EntityList = new List<DropDown>();
            DataSet dsStatus = new DataSet();
            dsStatus = objAuditTrail.getEntityName();
            dtEntityName = dsStatus.Tables[0];
            EntityList = (from DataRow dr in dtEntityName.Rows
                          select new DropDown()
                          {
                              ddlField = Convert.ToString(dr["EntityName"]).Trim(),
                          }).ToList();
            return EntityList;
        }

        private List<DropDown> GetUser()
        {
            objAuditTrail = new AuditTrail();
            List<DropDown> UserList = new List<DropDown>();
            DataSet dsUser = new DataSet();
            dsUser = objAuditTrail.getUser();
            dtUser = dsUser.Tables[0];
            UserList = (from DataRow dr in dtUser.Rows
                        select new DropDown()
                        {
                            ddlField = Convert.ToString(dr["ActionBy"]).Trim(),
                        }).ToList();
            return UserList;
        }

        private List<DropDown> GetApplicationName()
        {
            objAuditTrail = new AuditTrail();
            List<DropDown> ApplicatioNameList = new List<DropDown>();
            DataSet dsStation = new DataSet();
            dsStation = objAuditTrail.getApplicationName();
            dtApplicationName = dsStation.Tables[0];
            ApplicatioNameList = (from DataRow dr in dtApplicationName.Rows
                                  select new DropDown()
                                  {
                                      ddlField = Convert.ToString(dr["ApplicationName"]).Trim(),
                                  }).ToList();
            return ApplicatioNameList;
        }

        private void LoadControlValues()
        {
            try
            {
                cboEntityName.DataSource = GetEntityName();
                cboEntityName.DisplayMember = "ddlField";
                cboEntityName.ValueMember = "ddlField";
                cboEntityName.SelectedIndex = 0;
                cboUser.DataSource = GetUser();
                cboUser.DisplayMember = "ddlField";
                cboUser.ValueMember = "ddlField";
                cboUser.SelectedIndex = 0;
                cboApplicationName.DataSource = GetApplicationName();
                cboApplicationName.DisplayMember = "ddlField";
                cboApplicationName.ValueMember = "ddlField";
                cboApplicationName.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "Search()");
            }
        }
        #endregion
        #region Control Event
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            Search();
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dsCcTransmissionDetail.Tables.Count > 0)
                {
                    rptViewAuditTrail rpt = new rptViewAuditTrail();
                    clsReports.Preview(false, dsCcTransmissionDetail, rpt);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg("Not found any Record for Print");
            }
        }

        private void ultraLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
