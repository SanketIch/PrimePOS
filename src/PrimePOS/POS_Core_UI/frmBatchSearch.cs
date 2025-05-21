using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_Core_UI.Layout;
using NLog;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using Infragistics.Win.UltraWinGrid;

namespace POS_Core_UI
{
    public partial class frmBatchSearch : frmMasterLayout
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        public bool IsCanceled = true;
        public string strSelectedBatch;

        public frmBatchSearch()
        {
            InitializeComponent();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
                this.KeyPreview = false;
                if (this.grdSearch.Rows.Count > 0)
                {
                    this.grdSearch.Focus();
                    btnOk.Visible = true;
                }
                else
                {
                    this.dtpFromDate.Focus();
                    btnOk.Visible = false;
                }
                this.KeyPreview = true;
            }
            catch (Exception exp)
            {
            }
        }

        private void Search()
        {
            try
            {
                DateTime dtFrom = DateTime.Parse(DateTime.Parse(this.dtpFromDate.Value.ToString()).ToShortDateString() + " " + this.txtFromTime.Text);
                DateTime dtTo = DateTime.Parse(DateTime.Parse(this.dtpToDate.Value.ToString()).ToShortDateString() + " " + this.txtToTime.Text);

                //string strFrom = DateTime.Parse(this.dtpFromDate.Value.ToString()).ToShortDateString() + " " + DateTime.Parse(this.txtFromTime.Text).ToString("HH:mm");
                //string strTo = DateTime.Parse(this.dtpToDate.Value.ToString()).ToShortDateString() + " " + DateTime.Parse(this.txtToTime.Text).ToString("HH:mm");

                if (dtFrom <= dtTo)
                {
                    using (ReconciliationDeliveryReport oReconciliationDeliveryReport = new ReconciliationDeliveryReport())
                    {
                        DataTable dtBatch = oReconciliationDeliveryReport.GetBatchDeliveryData(dtFrom, dtTo);
                        if (dtBatch.Rows.Count > 0)
                        {
                            grdSearch.DataSource = dtBatch;
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("No Record Found");
                            return;
                        }

                    }

                    if (grdSearch != null && grdSearch.Rows.Count > 0)
                    {
                        this.grdSearch.DisplayLayout.Bands[0].Columns["DelBatchRecId"].Hidden = true;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["ShipService_CD"].Hidden = true;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["ShipCustomCD"].Hidden = true;

                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchNo"].Header.VisiblePosition = 0;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchNo"].Header.Caption = "Batch No";

                        this.grdSearch.DisplayLayout.Bands[0].Columns["CreationDate"].Header.VisiblePosition = 1;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["CreationDate"].Header.Caption = "Batch Date";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["creationdate"].Format = "MM/dd/yyyy hh:mm tt";

                        this.grdSearch.DisplayLayout.Bands[0].Columns["DelUserId"].Header.VisiblePosition = 2;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["DelUserId"].Header.Caption = "User";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchStatus"].Header.VisiblePosition = 3;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchStatus"].Header.Caption = "Batch Status";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchType"].Header.VisiblePosition = 4;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchType"].Header.Caption = "Batch Type";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["PatientCount"].Header.VisiblePosition = 5;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["PatientCount"].Header.Caption = "Patient Count";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["RxCount"].Header.VisiblePosition = 6;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["RxCount"].Header.Caption = "Rx Count";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopay"].Header.VisiblePosition = 7;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopay"].Header.Caption = "Total Copay";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopay"].Format = "0.00";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopay"].NullText = "0.00";

                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].Header.VisiblePosition = 8;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].Header.Caption = "TotalCopayCollected";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].Format = "0.00";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].NullText = "0.00";

                        this.grdSearch.DisplayLayout.Bands[0].Columns["CompletionDate"].Header.VisiblePosition = 9;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["CompletionDate"].Header.Caption = "Comp. Date";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["completiondate"].Format = "MM/dd/yyyy hh:mm tt";

                        this.grdSearch.DisplayLayout.Bands[0].Columns["Notes"].Header.VisiblePosition = 10;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["Notes"].Header.Caption = "Notes";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["FacilityCD"].Header.VisiblePosition = 11;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["FacilityCD"].Header.Caption = "FacilityCD";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["DeviceDelivery"].Header.VisiblePosition = 12;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["DeviceDelivery"].Header.Caption = "FacilityCD";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchCategory"].Header.VisiblePosition = 13;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["BatchCategory"].Header.Caption = "BatchCategory";
                        this.grdSearch.DisplayLayout.Bands[0].Columns["IntakeBatchID"].Header.VisiblePosition = 13;
                        this.grdSearch.DisplayLayout.Bands[0].Columns["IntakeBatchID"].Header.Caption = "IntakeBatchID";
                        resizeColumns();
                    }
                    else
                    {
                        btnOk.Visible = false;
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

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdSearch.DisplayLayout.Bands)
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            IsCanceled = false;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        public string SelectedBatch()
        {
            logger.Trace("SelectedRowID() - " + clsPOSDBConstants.Log_Entering);
            if (grdSearch.ActiveRow != null && grdSearch.ActiveRow.Cells.Count > 0)
            {
                strSelectedBatch = grdSearch.ActiveRow.Cells["BatchNo"].Value.ToString();
            }
            return strSelectedBatch;
        }

        private void Clear()
        {
            this.dtpFromDate.Value = DateTime.Now.AddDays(-30);
            this.dtpToDate.Value = DateTime.Now;
            this.txtFromTime.Text = "12:00 AM";
            this.txtToTime.Text = "11:59 PM";
            this.grdSearch.DataSource = null;
            this.ActiveControl = this.dtpFromDate;
        }

        private void frmBatchSearch_Load(object sender, EventArgs e)
        {
            Clear();
            this.dtpFromDate.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDate.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            Search();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void frmBatchSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    if (this.grdSearch.ContainsFocus == true)
                    {
                        if (this.grdSearch.Rows.Count > 0)
                        {
                            IsCanceled = false;
                            this.Close();
                        }
                    }
                    else
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
                else
                {
                    if (e.Alt)
                        ShortCutKeyAction(e.KeyCode);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                logger.Fatal(exp, "frmBatchSearch_KeyDown()");
            }
        }

        private void grdSearch_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            IsCanceled = false;
            this.Close();
        }

        private void frmBatchSearch_KeyUp(object sender, KeyEventArgs e)
        {
            logger.Trace("frmSearchMain_KeyUp() - " + clsPOSDBConstants.Log_Entering);

            switch (e.KeyData)
            {
                case Keys.F4:
                    Search();
                    break;
            }
        }

        private void ShortCutKeyAction(Keys KeyCode)
        {
            switch (KeyCode)
            {
                case Keys.L:
                    btnClear_Click(btnClear, new EventArgs());
                    break;
                case Keys.C:
                    btnCancel_Click(btnCancel, new EventArgs());
                    break;
                default:
                    break;
            }
        }
    }
}
