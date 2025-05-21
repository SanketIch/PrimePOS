using Infragistics.Win.UltraWinGrid;
using NLog;
using POS_Core.BusinessRules;
using System;
using System.Data;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmRxRecovery : System.Windows.Forms.Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        POSTransaction posTrans = null;
        public frmRxRecovery()
        {
            InitializeComponent();
        }

        #region Form Event
        private void frmRxRecovery_Load(object sender, EventArgs e)
        {
            Clear();
            this.dtpFromDate.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDate.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            Search();
        }
        private void frmRxRecovery_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Trace("frmRxRecovery_KeyDown(object sender, KeyEventArgs e) - Entered");
            try
            {
                //if (e.KeyData == System.Windows.Forms.Keys.Enter)
                //{
                //}
                //else  
                //{
                if (e.Alt)
                {
                    ShortCutKeyAction(e.KeyCode);
                }
                else if (e.KeyData == Keys.Escape)
                {
                    this.Close();
                }
                //}
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, "frmRxRecovery_KeyDown(object sender, KeyEventArgs e)");
                throw Ex;
            }
            logger.Trace("frmRxRecovery_KeyDown(object sender, KeyEventArgs e) - Exited");
        }
        private void frmRxRecovery_KeyUp(object sender, KeyEventArgs e)
        {
            logger.Trace("frmRxRecovery_KeyUp(object sender, KeyEventArgs e) - Entered");
            try
            {
                switch (e.KeyData)
                {
                    case Keys.F2:
                        btnRxRecoverTransaction_Click(null, null);
                        break;
                    case Keys.F4:
                        btnSearch_Click(null, null);
                        break;
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, "frmRxRecovery_KeyUp(object sender, KeyEventArgs e)");
                throw Ex;
            }
            logger.Trace("frmRxRecovery_KeyUp(object sender, KeyEventArgs e) - Exited");
        }
        private void btnRxRecoverTransaction_Click(object sender, EventArgs e)
        {
            logger.Trace("btnRxRecoverTransaction_Click(object sender, EventArgs e) - Entered");
            try
            {
                posTrans = new POSTransaction();
                DataRow row = (grdRecovery.ActiveRow.ListObject as DataRowView).Row;
                int TransID = row.Field<int>("TransID");
                DataSet dsRxTransLog = new DataSet();
                DataTable dtRxPrescConsentLog = new DataTable(); //PRIMEPOS-3192
                dtRxPrescConsentLog = posTrans.RxPrescConsentPopulate();//PRIMEPOS-3192
                dsRxTransLog = posTrans.RxTransactionPopulate(TransID);
                posTrans.UpdatePrimeRXData(dsRxTransLog, null, false, null, dtRxPrescConsentLog);//PRIMEPOS-3192
                Search();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, "btnRxRecoverTransaction_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
                throw Ex;
            }
            logger.Trace("btnRxRecoverTransaction_Click(object sender, EventArgs e) - Exited");
        }
        #endregion

        #region Button Event
        private void btnClose_Click(object sender, EventArgs e)
        {
            //if (Resources.Message.Display("Are your sure, you want to Close ?", "Cancel Recovery", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            this.Close();
            //}
            //else
            //{
            //    return;
            //}
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            logger.Trace("btnSearch_Click(object sender, EventArgs e) - Entered");
            try
            {
                Search();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, "btnSearch_Click(object sender, EventArgs e)");
                throw Ex;
            }
            logger.Trace("btnSearch_Click(object sender, EventArgs e) - Exited");
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion
        #region Method
        private void Search()
        {
            DataSet dsCcTransmissionDetail = new DataSet();
            DataTable dtRxTransmissionDetail = new DataTable();
            posTrans = new POSTransaction();
            try
            {

                DateTime dtFrom = DateTime.Parse(DateTime.Parse(this.dtpFromDate.Value.ToString()).ToShortDateString() + " " + this.txtFromTime.Text);
                DateTime dtTo = DateTime.Parse(DateTime.Parse(this.dtpToDate.Value.ToString()).ToShortDateString() + " " + this.txtToTime.Text);

                if (dtFrom <= dtTo)
                {
                    dtRxTransmissionDetail = posTrans.GetRxTransmission(dtFrom, dtTo);

                    if (dtRxTransmissionDetail.Rows.Count > 0 && dtRxTransmissionDetail != null)
                    {
                        tlpRecover.Visible = true;
                    }
                    else
                    {
                        tlpRecover.Visible = false;
                    }

                    grdRecovery.DataSource = dtRxTransmissionDetail;
                    grdRecovery.DataBind();
                    GridColumnDisplay();
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("'From Date' should not be greater than 'To Date'");
                }
            }
            catch (Exception Ex)
            {

                throw;
            }
        }
        private void GridColumnDisplay()
        {
            if (grdRecovery != null && grdRecovery.Rows.Count > 0)
            {

                this.grdRecovery.DisplayLayout.Bands[0].Columns["RxTransNo"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransDateTime"].Header.VisiblePosition = 3;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransDateTime"].Header.Caption = "Transactio Date";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransDateTime"].Format = "MM/dd/yyyy hh:mm tt";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["PatientID"].Header.VisiblePosition = 2;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["PatientID"].Header.Caption = "PatientNo";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["RxNo"].Header.VisiblePosition = 0;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["RxNo"].Header.Caption = "RxNo";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["Nrefill"].Header.VisiblePosition = 1;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["Nrefill"].Header.Caption = "RefillNo";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["PickedUp"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransID"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["IsDelivery"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["IsRxSync"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransAmount"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["StationID"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["UserID"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["CreatedDate"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ModifiedBy"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ModifiedDate"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["PickUpPOS"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ConsentTextID"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ConsentTypeID"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ConsentStatusID"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ConsentCaptureDate"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ConsentEffectiveDate"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ConsentEndDate"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["RelationID"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["SigneeName"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["SignatureData"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["PickUpdate"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["CopayPaid"].Hidden = true;


                ResizeColumns();
                //  ColorChangeTransactionWise();
            }
            else
            {
                tlpRecover.Visible = false;
            }
        }
        private void ResizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdRecovery.DisplayLayout.Bands)
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
            this.dtpFromDate.Value = DateTime.Now.AddDays(-1);
            this.dtpToDate.Value = DateTime.Now;
            this.txtFromTime.Text = "12:00 AM";
            this.txtToTime.Text = "11:59 PM";
            // this.grdRecovery.DataSource = null;
            this.ActiveControl = this.dtpFromDate;

        }
        private void ShortCutKeyAction(Keys KeyCode)
        {
            try
            {
                switch (KeyCode)
                {
                    case Keys.C:
                        btnClose_Click(btnClose, new EventArgs());
                        break;
                    case Keys.L:
                        btnClear_Click(btnClear, new EventArgs());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, "ShortCutKeyAction(Keys KeyCode)");
                throw;
            }
        }
        #endregion


    }
}
