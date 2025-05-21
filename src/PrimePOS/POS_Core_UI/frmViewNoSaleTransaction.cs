using Infragistics.Win.UltraWinGrid;
using NLog;
using POS_Core.BusinessRules;
using POS_Core_UI.Reports.Reports;
using POS_Core_UI.Reports.ReportsUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmViewNoSaleTransaction : System.Windows.Forms.Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        NoSaleTransaction objNoSale = null;
        DataTable dtEntityName = new DataTable();
        DataTable dtUser = new DataTable();
        DataTable dtApplicationName = new DataTable();
        DataSet dsCcTransmissionDetail = new DataSet();
        public frmViewNoSaleTransaction()
        {
            InitializeComponent();
        }

        #region FORM EVENT

        private void frmViewNoSaleTransaction_Load(object sender, EventArgs e)
        {
            try
            {
                Clear();
                this.dtpFromDate.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.dtpToDate.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                LoadControlValues();
                Search();
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "frmViewNoSaleTransaction_Load()");
            }
        }

        private void frmViewNoSaleTransaction_KeyDown(object sender, KeyEventArgs e)
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

        private void frmViewNoSaleTransaction_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F4:
                    btnSearch_Click(null, null);
                    break;
            }
        }

        #endregion

        #region Methods
        private void Search()
        {
            //DataSet dsCcTransmissionDetail = new DataSet();
            objNoSale = new NoSaleTransaction();
            try
            {
                DateTime dtFrom = DateTime.Parse(this.dtpFromDate.Value.ToString());
                DateTime dtTo = DateTime.Parse(this.dtpToDate.Value.ToString());

                if (dtFrom <= dtTo)
                {
                    dsCcTransmissionDetail = objNoSale.GetNoSaleTransactionLog(dtFrom, dtTo, cboStationId.SelectedItem.DataValue.ToString(), cboUser.SelectedItem.DataValue.ToString());
                    if (dsCcTransmissionDetail?.Tables != null
                        && dsCcTransmissionDetail.Tables.Count > 0 && dsCcTransmissionDetail.Tables.Count > 0)
                    {
                        grdNoSaleTransaction.DataSource = dsCcTransmissionDetail.Tables[0];
                        grdNoSaleTransaction.DataBind();
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
        private void GridColumnDisplay()
        {
            //  EntityID, EntityName, FieldChanged, OldValue, NewValue, DateChanged, ActionBy, Operation, ApplicationName , SQLUser 
            if (grdNoSaleTransaction != null && grdNoSaleTransaction.Rows.Count > 0)
            {

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["EntityName"].Header.VisiblePosition = 0;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["EntityName"].Header.Caption = "Entity Name";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["FieldChanged"].Header.VisiblePosition = 1;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["FieldChanged"].Header.Caption = "Field Changed";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["OldValue"].Header.VisiblePosition = 2;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["OldValue"].Header.Caption = "Old Value";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["NewValue"].Header.VisiblePosition = 3;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["NewValue"].Header.Caption = "New Value";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["DateChanged"].Header.VisiblePosition = 4;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["DateChanged"].Header.Caption = "Modified Date";
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["DateChanged"].Format = "MM/dd/yyyy hh:mm tt";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["ActionBy"].Header.VisiblePosition = 5;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["ActionBy"].Header.Caption = "Action By";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["Operation"].Header.VisiblePosition = 6;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["Operation"].Header.Caption = "Operation";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["ApplicationName"].Header.VisiblePosition = 7;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["ApplicationName"].Header.Caption = "Application Name";

                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["SQLUser"].Header.VisiblePosition = 8;
                this.grdNoSaleTransaction.DisplayLayout.Bands[0].Columns["SQLUser"].Header.Caption = "SQLUser";

                ResizeColumns();

            }
            else
            {
                //pnlAdd.Visible = false;
                //tlpViewTransaction.Visible = false;
            }
        }
        private void ResizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdNoSaleTransaction.DisplayLayout.Bands)
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
                    btnClear_Click(btnClear, new EventArgs());
                    break;
                default:
                    break;
            }
        }

        private List<DropDown> GetUser()
        {
            objNoSale = new NoSaleTransaction();
            List<DropDown> UserList = new List<DropDown>();
            DataSet dsUser = new DataSet();
            dsUser = objNoSale.getUser();
            dtUser = dsUser.Tables[0];
            UserList = (from DataRow dr in dtUser.Rows
                        select new DropDown()
                        {
                            ddlField = Convert.ToString(dr["UserId"]).Trim(),
                        }).ToList();
            return UserList;
        }

        private List<DropDown> GetStations()
        {
            objNoSale = new NoSaleTransaction();
            List<DropDown> StationsList = new List<DropDown>();
            DataSet dsUser = new DataSet();
            dsUser = objNoSale.getStationID();
            dtUser = dsUser.Tables[0];
            StationsList = (from DataRow dr in dtUser.Rows
                            select new DropDown()
                            {
                                ddlField = Convert.ToString(dr["StationId"]).Trim(),
                            }).ToList();
            return StationsList;
        }

        private void LoadControlValues()
        {
            try
            {
                cboStationId.DataSource = GetStations();
                cboStationId.DisplayMember = "ddlField";
                cboStationId.ValueMember = "ddlField";
                cboStationId.SelectedIndex = 0;
                cboUser.DataSource = GetUser();
                cboUser.DisplayMember = "ddlField";
                cboUser.ValueMember = "ddlField";
                cboUser.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "LoadControlValues()");
            }
        }
        #endregion        

        private void grdNoSaleTransaction_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns["DrawerOpenedDate"].Format = "G"; //MM/dd/yyyy HH:mm:ss
        }

        #region Control Event

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dsCcTransmissionDetail.Tables.Count > 0)
                {
                    rptViewNoSaleTransaction rpt = new rptViewNoSaleTransaction();
                    clsReports.Preview(false, dsCcTransmissionDetail, rpt);
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg("Not found any Record for Print");
            }
        }
    }


}
