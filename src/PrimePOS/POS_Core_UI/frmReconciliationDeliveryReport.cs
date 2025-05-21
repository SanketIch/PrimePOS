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
using POS_Core.BusinessRules;
using POS_Core.Resources;
using NLog;
using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData;
using POS_Core_UI.UI;

namespace POS_Core_UI
{
    public partial class frmReconciliationDeliveryReport : frmMasterLayout
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        private int CurrentX;
        private int CurrentY;
        DataTable dtGetBatchDeliveryDetails = new DataTable();
        DataTable dtBatchDeliveryPatient = new DataTable();
        DataTable dtBatchDeliveryRx = new DataTable();
        DataTable dtAllHoldTransaction = new DataTable();
        string status = string.Empty;
        private int selectedRowIndex = -1;
        //POSTransaction posTrans = null;
        public frmPOSTransaction ofrmTrans = null;

        public frmReconciliationDeliveryReport(frmPOSTransaction frmTrans)
        {
            InitializeComponent();
            //posTrans = new POSTransaction();
            ofrmTrans = frmTrans;
        }
        public frmReconciliationDeliveryReport(frmPOSTransaction frmTrans, string batchNo)
        {
            InitializeComponent();
            //posTrans = new POSTransaction();
            ofrmTrans = frmTrans;
            txtDeliveryBatch.Text = batchNo;
            RefreshDisplay();
        }
        public frmReconciliationDeliveryReport()
        {
            InitializeComponent();
            //posTrans = new POSTransaction();
        }

        #region Form Events
        private void frmReconciliationDeliveryReport_Load(object sender, EventArgs e)
        {
            logger.Trace("frmReconciliationDeliveryReport_Load(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            SetDefaultValues();
            //this.txtDeliveryBatch.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtDeliveryBatch.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtDeliveryBatch.Focus();
            if (txtDeliveryBatch.Text == string.Empty)
            {
                btnUnHold.Visible = false;
                btnPayment.Visible = false;
            }
            logger.Trace("frmReconciliationDeliveryReport_Load(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }
        private void frmReconciliationDeliveryReport_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtDeliveryBatch.ContainsFocus)
                        BatchSearch();
                }

                if (e.KeyData == System.Windows.Forms.Keys.F3)
                {
                    if (txtDeliveryBatch.ContainsFocus)
                    {
                        if (txtDeliveryBatch.Text == string.Empty)
                        {
                            txtDeliveryBatch.Text = PosUiConstants.lastBatchNo;
                            RefreshDisplay();
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region Control Events
        // For unhold transaction 
        private void btnUnHold_Click(object sender, EventArgs e)
        {
            try
            {
                string rxNo = "";
                int holdTransId = 0;
                if (dtAllHoldTransaction.Rows.Count > 0)
                {
                    UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
                    UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
                    foreach (UltraGridRow Parentrow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
                    {
                        foreach (UltraGridRow Childrow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                        {
                            if (Parentrow.Cells["DelPatRecId"].Value.ToString() == Childrow.Cells["DelPatRecId"].Value.ToString())
                            {
                                if (Convert.ToString(Childrow.Cells["CHECK"].Value) == "True")
                                {
                                    rxNo = Childrow.Cells["RxNo"].Value.ToString();
                                    var transId = dtAllHoldTransaction.AsEnumerable()
                                    .Where(s => rxNo.Contains(Convert.ToString(s["RxNo"])))
                                    .Select(k => k[0].ToString()).FirstOrDefault();

                                    holdTransId = Convert.ToInt32(transId);

                                    POSTransaction osvr = new POSTransaction();
                                    osvr.RemoveOnHoldTrans(holdTransId);
                                }
                            }
                        }
                    }
                }
                RefreshDisplay();
                #region Commented
                //CheckHoldOrPaid();
                //SetProcessedRowAppearance();
                //#region Commneted
                ////DataTable dtHoldData = new DataTable();
                ////string status = string.Empty;
                ////// Child Gridview

                ////UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[1];
                ////foreach (DataRow row in dtBatchDeliveryRx.Rows)
                ////{

                ////    status = posTrans.CheckOnHoldOrAlreadyPaid(row, out dtHoldData);
                ////    if (status == "Hold")
                ////    {
                ////        UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
                ////        foreach (UltraGridRow gvrow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                ////        {
                ////            if (gvrow.Cells["RxNo"].Value.ToString() == row["RxNo"].ToString())
                ////            {
                ////                gvrow.Appearance.BackColor = Color.LightSkyBlue;
                ////                gvrow.Update();
                ////            }
                ////        }
                ////    }
                ////    else if (status == "Paid")
                ////    {

                ////        UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
                ////        foreach (UltraGridRow gvrow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                ////        {
                ////            if (gvrow.Cells["RxNo"].Value.ToString() == row["RxNo"].ToString())
                ////            {
                ////                gvrow.Appearance.BackColor = Color.LightGreen;
                ////                gvrow.Update();
                ////            }
                ////        }

                ////            //    //disable the check box and row and mark other color
                ////        }
                ////    //else
                ////    //{
                ////    //    // stay normal and enabled
                ////    //}
                ////}
                //////if any of the rows enabled enable at patient levels also
                //////update the PharmData.dll with follongs
                //////update the Delivery_details tables for all the rx's which are paid etc
                //////update the Delivery_Batch.POSStamped = "true"
                //////enable the PaymentButton
                //#endregion
                #endregion
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "btnUnHold_Click()");
            }
        }

        // For do payment and move to rx on POS screen
        private void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                string patNo = string.Empty;
                string[] rxNo = null;
                string delPatRecId = "";
                string delRx = "";
                decimal alreadyPaidAmount = 0;
                PosUiConstants.lastBatchNo = txtDeliveryBatch.Text;
                UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
                UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
                foreach (UltraGridRow parentRow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
                {
                    foreach (UltraGridRow childRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                    {
                        if (parentRow.Cells["DelPatRecId"].Value.ToString() == childRow.Cells["DelPatRecId"].Value.ToString())
                        {
                            if (Convert.ToString(childRow.Cells["CHECK"].Value) == "True")
                            {
                                delRx += "," + childRow.Cells["RxNo"].Value.ToString();
                            }
                        }
                    }
                    if (Convert.ToString(parentRow.Cells["CHECK"].Value) == "True")
                    {
                        delPatRecId += "," + parentRow.Cells["DelPatRecId"].Value.ToString();
                        if (!string.IsNullOrWhiteSpace(parentRow.Cells["TotalCopayCollected"].Value.ToString()))
                        {
                            alreadyPaidAmount += Convert.ToDecimal(parentRow.Cells["TotalCopayCollected"].Value.ToString());
                        }
                    }
                }

                rxNo = dtBatchDeliveryRx.AsEnumerable()
                          .Where(s => delPatRecId.Contains(Convert.ToString(s["DelPatRecId"])) && delRx.Contains(Convert.ToString(s["RxNo"])))
                          .Select(k => k[2].ToString() + "rx").ToArray();


#if BatchDel
                //order by hold as asending  (One issue if there are more than 1 rx in hold in same patient order with different transaction id how do we work?
                //and dont pass rxnumbers which are already paid in postransaction and having record in POStransaction.
#endif

                if (rxNo.Length > 0)
                {
#if BatchDel

                    frmPOSTransaction ofrmPosTransBatch = new frmPOSTransaction(args, true, rxdetails);  // payment already done??
                    ofrmPosTransBatch.ShowDialog();
                    //refresh/relaod data from PharmSQL
#endif
                    DataTable dtBatchDeliverySelectedRx = dtBatchDeliveryRx.AsEnumerable()
                            .Where(s => delPatRecId.Contains(Convert.ToString(s["DelPatRecId"])) && delRx.Contains(Convert.ToString(s["RxNo"])))
                            .CopyToDataTable();

                    if (ofrmTrans == null)
                    {
                        frmPOSTransaction ofrmPosTransBatch = new frmPOSTransaction(rxNo, true, dtBatchDeliverySelectedRx, alreadyPaidAmount);
                        ofrmPosTransBatch.isLaunchedByDelivery = true;
                        ofrmPosTransBatch.ShowDialog();
                    }
                    else
                    {
                        ofrmTrans.ReloadTransaction(rxNo, true, dtBatchDeliverySelectedRx, alreadyPaidAmount);
                        this.Close();
                    }
                    #region Commneted
                    //if (ofrmPosTransBatch.BatchDelCopayAmount != 0)
                    //{
                    //    using (ReconciliationDeliveryReport oReconciliationDeliveryReport = new ReconciliationDeliveryReport())
                    //    {
                    //        dtGetBatchDeliveryDetails.Rows[0]["TotalCopayCollected"] = ofrmPosTransBatch.BatchDelCopayAmount;

                    //        foreach (UltraGridRow row in grdData.Rows)
                    //        {
                    //            if (Convert.ToString(row.Cells["CHECK"].Value) == "True")
                    //            {
                    //                dtBatchDeliveryPatient.AsEnumerable().Where(s => Convert.ToString(s["DelBatchId"]).Equals(dtGetBatchDeliveryDetails.Rows[0]["DelBatchRecId"].ToString()) && Convert.ToString(s["DelPatientNo"]).Equals(row.Cells["DelPatientNo"].Value.ToString())).ToList().ForEach(D => D.SetField("TotalCopayCollected", ofrmPosTransBatch.BatchDelCopayAmount));
                    //                dtBatchDeliveryRx.AsEnumerable().Where(s => Convert.ToString(s["DelPatRecId"]).Equals(row.Cells["DelPatRecId"].Value.ToString())).ToList().ForEach(D => D.SetField("CopayCollected", ofrmPosTransBatch.BatchDelCopayAmount));
                    //                oReconciliationDeliveryReport.UpdateBatchDeliveryPaymentStatus(dtGetBatchDeliveryDetails);
                    //                oReconciliationDeliveryReport.UpdateBatchDeliveryOrderPaymentStatus(dtBatchDeliveryPatient);
                    //                oReconciliationDeliveryReport.UpdateBatchDeliveryDetailPaymentStatus(dtBatchDeliveryRx);
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    RefreshDisplay();
                    #region RITESH following code should be in PharmData UpdateBatchDeliveryDetailPaymentStatus
                    //using (ReconciliationDeliveryReport oReconciliationDeliveryReport = new ReconciliationDeliveryReport())
                    //{
                    //    if (dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "False" || dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "")
                    //    {
                    //        dtGetBatchDeliveryDetails.Rows[0]["POSStamped"] = true;
                    //        oReconciliationDeliveryReport.UpdateBatchDeliveryPaymentStatus(dtGetBatchDeliveryDetails);
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception Ex)
            {
                //RITESH MessageBox failure dialog??
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "btnPayment_Click()");
            }
        }

        // textbox Search button for searching batch
        private void txtDeliveryBatch_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            BatchSearch();
        }

        // On press Enter key open batch search screen
        private void txtDeliveryBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DisplayBatch();
            }
        }

        // Select All when click on Parent Checkbox
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            logger.Trace("chkSelectAll_CheckedChanged() - " + clsPOSDBConstants.Log_Entering);
            grdData.BeginUpdate();
            foreach (UltraGridRow oRow in grdData.Rows)
            {
                if (oRow.Appearance.BackColor != Color.LightGreen)
                {
                    oRow.Cells["check"].Value = chkSelectAll.Checked;
                    oRow.Update();
                }
            }
            grdData.EndUpdate();
            logger.Trace("chkSelectAll_CheckedChanged() - " + clsPOSDBConstants.Log_Exiting);
        }

        // Close the batch screen
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Resources.Message.Display("Are your sure, you want to cancel Delivery Transaction?", "Cancel Delivery Transaction", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //RITESH
                if (ofrmTrans != null)
                    ofrmTrans.isBatchDelivery = false;
                //frmPOSTransaction frm = new frmPOSTransaction();
                //frm.IsBatchDelivery = false;
                this.Close();
                //frm.Dispose();
            }
            else
            {
                return;
            }
        }


        #region Ultragrid events
        private void grdData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (grdData.ActiveRow != null)
                {
                    if (this.grdData.ActiveRow.Appearance.BackColor != Color.LightGreen)
                        if (HasHoldRecordSelected() || grdData.ActiveRow.Index == selectedRowIndex)
                        {
                            CheckUncheckGridRow(this.grdData.ActiveRow.Cells["check"]);
                            HasHoldRecordSelected();
                        }
                }
            }
        }
        private void grdData_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdData.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdData.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
                if (oUI == null)
                    return;
                HasParentRecordSelected();
                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.CellUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.CellUIElement cellUIElement = (Infragistics.Win.UltraWinGrid.CellUIElement)oUI;
                        if (cellUIElement.Column.Key.ToUpper() == "CHECK")
                        {
                            if (this.grdData.ActiveRow.Appearance.BackColor != Color.LightGreen)
                            {
                                if (cellUIElement.Row.Band.ToString() == "Master")
                                {
                                    UltraGridRow currentRow = (UltraGridRow)oUI.GetContext(typeof(UltraGridRow));
                                    UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
                                    UltraGridBand bandMasterMaster = this.grdData.DisplayLayout.Bands[1];
                                    foreach (UltraGridRow parentRow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
                                    {
                                        if (parentRow != currentRow)
                                        {
                                            foreach (UltraGridRow childRow in bandMasterMaster.GetRowEnumerator(GridRowType.DataRow))
                                            {
                                                if (parentRow.Cells["DelPatRecId"].Value.ToString() != childRow.Cells["DelPatRecId"].Value.ToString())
                                                {

                                                    childRow.Cells["check"].Value = false;
                                                    parentRow.Cells["check"].Value = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                }

                                if (HasHoldRecordSelected() || cellUIElement.Cell.Row.Index == selectedRowIndex)
                                {
                                    CheckUncheckGridRow(cellUIElement.Cell);

                                    //if (cellUIElement.Row.Cells["DelStatus"].Value.ToString() == "D" || cellUIElement.Row.Cells["DelStatus"].Value.ToString() == "C")
                                    //{
                                    if (cellUIElement.Row.Band.ToString() == "Master")
                                    {
                                        DeliveryStatusWiseCheckAll(cellUIElement.Cell);
                                    }
                                    //}
                                    HasHoldRecordSelected();
                                    DeliveryStatusWiseEnableDisableCheckbox();

                                    EnableDisableOnHoldButton();

                                }
                            }

                        }
                        break;
                    }
                    oUI = oUI.Parent;
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        private void grdData_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }
        #endregion

        #endregion

        #region Methods
        // Hold Transaction only once.. NileshJ
        // automatically checked all Hold Transaction when click on parent Checkbox
        private void DeliveryStatusWiseCheckAll(UltraGridCell oCell)
        {
            UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
            UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
            foreach (UltraGridRow parentRow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            {
                if (Convert.ToBoolean(parentRow.Cells["CHECK"].Value) && oCell.Row.Appearance.BackColor != Color.LightSkyBlue)
                {
                    //if (Parentrow.Cells["DelStatus"].Value.ToString() == "D" || Parentrow.Cells["DelStatus"].Value.ToString() == "C")
                    //{
                    foreach (UltraGridRow childRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                    {
                        if (parentRow.Cells["DelPatRecId"].Value.ToString() == childRow.Cells["DelPatRecId"].Value.ToString())
                        {
                            //if (Childrow.Cells["DelStatus"].Value.ToString() == "D" || Childrow.Cells["DelStatus"].Value.ToString() == "C")
                            //{
                            childRow.Cells["CHECK"].Value = true;
                            if (childRow.Appearance.BackColor == Color.LightSkyBlue)
                            {
                                childRow.Cells["CHECK"].Value = true; // chaanges false to true and added break;
                                //break;
                            }
                            if (childRow.Appearance.BackColor == Color.LightGreen)
                            {
                                childRow.Cells["CHECK"].Value = false;
                            }
                            //}
                        }
                    }
                    //}
                }
                else
                {
                    if (oCell.Row.Appearance.BackColor != Color.LightSkyBlue)
                    {
                        //if (Parentrow.Cells["DelStatus"].Value.ToString() == "D" || Parentrow.Cells["DelStatus"].Value.ToString() == "C")
                        //{
                        foreach (UltraGridRow childRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                        {
                            if (parentRow.Cells["DelPatRecId"].Value.ToString() == childRow.Cells["DelPatRecId"].Value.ToString())
                            {
                                //if (Childrow.Cells["DelStatus"].Value.ToString() == "D" || Childrow.Cells["DelStatus"].Value.ToString() == "C")
                                //{
                                childRow.Cells["CHECK"].Value = false;
                                //}
                            }
                        }
                        //}
                    }
                }
            }
        }

        // Delivery status wise Enable and Disable checkbox
        private void DeliveryStatusWiseEnableDisableCheckbox()
        {
            UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
            foreach (UltraGridRow oRow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            {
                if (oRow.Cells["DelStatus"].Value.ToString() == "U" || oRow.Cells["DelStatus"].Value.ToString() == "E")
                {
                    oRow.Cells["check"].Value = false;
                    oRow.Appearance.BackColor = Color.Red;
                    oRow.Update();
                }
            }

            UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
            foreach (UltraGridRow oRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
            {
                if (oRow.Cells["DelStatus"].Value.ToString() == "U" || oRow.Cells["DelStatus"].Value.ToString() == "E")
                {
                    oRow.Cells["check"].Value = false;
                    oRow.Appearance.BackColor = Color.Red;
                    oRow.Update();
                    btnUnHold.Visible = false;
                    btnPayment.Visible = true;
                }
            }
        }

        // when hold record select once then other record disable for selecting
        private bool HasHoldRecordSelected()
        {

            bool IsHoldRecordSelected = true;
            UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
            foreach (UltraGridRow oRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
            {
                if (oRow.Appearance.BackColor == Color.LightSkyBlue && Convert.ToBoolean(oRow.Cells["check"].Value)) // Hold Transaction and Check true
                {
                    selectedRowIndex = oRow.Index;
                    IsHoldRecordSelected = false;
                    break;
                }
            }
            if (!IsHoldRecordSelected)
            {
                grdData.BeginUpdate();
                foreach (UltraGridRow oRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                {
                    if (oRow.Appearance.BackColor != Color.LightSkyBlue)
                    {
                        oRow.Cells["check"].Value = false;
                        oRow.Update();
                    }
                }
                grdData.EndUpdate();
            }

            return IsHoldRecordSelected;
        }

        // Only one parent record we can select 
        private void HasParentRecordSelected()
        {

            UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
            if (bandMaster.ToString() == "Master")
            {
                UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
                foreach (UltraGridRow parentRow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
                {
                    foreach (UltraGridRow childRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                    {
                        if ((Convert.ToBoolean(parentRow.Cells["check"].Value) && Convert.ToBoolean(childRow.Cells["check"].Value)) && parentRow.Cells["DelPatRecId"].Value.ToString() != childRow.Cells["DelPatRecId"].Value.ToString())
                        {
                            parentRow.Cells["check"].Value = false;
                            childRow.Cells["check"].Value = false;
                        }
                        //else if(Convert.ToBoolean(Parentrow.Cells["check"].Value) &&  !Convert.ToBoolean(Childrow.Cells["check"].Value) && Parentrow.Cells["DelPatRecId"].Value.ToString() == Childrow.Cells["DelPatRecId"].Value.ToString())
                        //{
                        //    Parentrow.Cells["check"].Value = false;
                        //}
                        //else if (!Convert.ToBoolean(Parentrow.Cells["check"].Value) && Convert.ToBoolean(Childrow.Cells["check"].Value) && Parentrow.Cells["DelPatRecId"].Value.ToString() == Childrow.Cells["DelPatRecId"].Value.ToString())
                        //{
                        //    Childrow.Cells["check"].Value = false;
                        //}
                    }
                }
            }
            //UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
            //foreach (UltraGridRow oRow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            //{

            //    if (Convert.ToBoolean(oRow.Cells["check"].Value))
            //    {
            //        oRow.Cells["check"].Value = false;
            //    }

            //}

        }

        //when textbox is empty then open batch search screen else show data on ultragrid
        private void DisplayBatch()
        {
            try
            {
                logger.Trace("DisplayBatch() - " + clsPOSDBConstants.Log_Entering);
                if (txtDeliveryBatch.Text.Trim() == "")
                {
                    //Resources.Message.Display("Please enter Batch No", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //return;
                    BatchSearch();
                }
                else
                {
                    this.chkSelectAll.Visible = false;
                    this.chkSelectAll.Checked = false;
                    RefreshDisplay();
                }
                logger.Trace("DisplayBatch() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "DisplayBatch()");
            }
        }

        // Get data from PharmData
        private void Display()
        {
            try
            {
                logger.Trace("Display() - " + clsPOSDBConstants.Log_Entering);
                using (ReconciliationDeliveryReport oReconciliationDeliveryReport = new ReconciliationDeliveryReport())
                {
                    dtGetBatchDeliveryDetails = oReconciliationDeliveryReport.GetBatchDeliveryDetails(txtDeliveryBatch.Text.Trim()); // Get Delivery_Batch data

                    if (dtGetBatchDeliveryDetails != null && dtGetBatchDeliveryDetails.Rows.Count > 0)
                    {
                        dtBatchDeliveryPatient = oReconciliationDeliveryReport.GetBatchDeliveryPatient(Configuration.convertNullToString(dtGetBatchDeliveryDetails.Rows[0]["DelBatchRecId"])); // get Delivery_Order Data
                        dtBatchDeliveryRx = oReconciliationDeliveryReport.GetBatchDeliveryRx(Configuration.convertNullToString(dtGetBatchDeliveryDetails.Rows[0]["DelBatchRecId"]));

                        txtBatch.Text = Configuration.convertNullToString(dtGetBatchDeliveryDetails.Rows[0]["BatchNo"]);
                        txtType.Text = Configuration.convertNullToString(dtGetBatchDeliveryDetails.Rows[0]["BatchType"]);

                        txtStatus.Text = Configuration.convertNullToString(dtGetBatchDeliveryDetails.Rows[0]["BatchStatus"]);
                        try
                        {
                            if (dtGetBatchDeliveryDetails.Rows[0]["CreationDate"] != null)
                            {
                                txtDate.Text = Convert.ToDateTime(dtGetBatchDeliveryDetails.Rows[0]["CreationDate"]).ToShortDateString();
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "Display() - txtDate.Text");
                        }
                        txtDriverInitials.Text = Configuration.convertNullToString(dtGetBatchDeliveryDetails.Rows[0]["DelUserId"]);

                        txtOTC.Text = "0";
                        txtRx.Text = Configuration.convertNullToString(dtGetBatchDeliveryDetails.Rows[0]["RxCount"]);
                        txtItemsTotal.Text = (Configuration.convertNullToInt(txtOTC.Text) + Configuration.convertNullToInt(dtGetBatchDeliveryDetails.Rows[0]["RxCount"])).ToString();

                        if (dtBatchDeliveryPatient != null && dtBatchDeliveryPatient.Rows.Count > 0)
                        {
                            txtPatientsRemaining.Text = Configuration.convertNullToInt(dtBatchDeliveryPatient.AsEnumerable().Where(x => x.Field<decimal?>("TotalCopay") != x.Field<decimal?>("TotalCopayCollected")).Count()).ToString();
                            txtPatientsReconciled.Text = (Configuration.convertNullToInt(dtGetBatchDeliveryDetails.Rows[0]["PatientCount"]) - Configuration.convertNullToInt(txtPatientsRemaining.Text)).ToString();
                            txtPatientsTotal.Text = (Configuration.convertNullToInt(txtPatientsRemaining.Text) + Configuration.convertNullToInt(txtPatientsReconciled.Text)).ToString();
                        }

                        txtAmountRemaining.Text = (Configuration.convertNullToDecimal(dtGetBatchDeliveryDetails.Rows[0]["TotalCopay"]) - (Configuration.convertNullToDecimal(dtGetBatchDeliveryDetails.Rows[0]["TotalCopayCollected"]))).ToString("0.00");
                        txtAmountReconciled.Text = Configuration.convertNullToDecimal(dtGetBatchDeliveryDetails.Rows[0]["TotalCopayCollected"]).ToString("0.00");
                        txtAmountTotal.Text = Configuration.convertNullToDecimal(dtGetBatchDeliveryDetails.Rows[0]["TotalCopay"]).ToString("0.00");


                    }
                    else
                    {
                        if (dtBatchDeliveryPatient != null && dtBatchDeliveryPatient.Rows.Count > 0)
                        {
                            dtBatchDeliveryPatient.Clear();
                        }
                        //else
                        //{
                        //    clsUIHelper.ShowErrorMsg( " Patient not found for "+ txtDeliveryBatch.Text);
                        //}
                        if (dtBatchDeliveryRx != null && dtBatchDeliveryRx.Rows.Count > 0)
                        {
                            dtBatchDeliveryRx.Clear();
                        }

                        clsUIHelper.ShowErrorMsg(txtDeliveryBatch.Text + " Batch Details not Found");
                    }

                }

                logger.Trace("Display() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Display()");
            }
        }

        private void PopulateGrid()
        {
            try
            {
                logger.Trace("PopulateGrid() - " + clsPOSDBConstants.Log_Entering);

                DataSet oDataSet = new DataSet();
                oDataSet.Tables.Add(dtBatchDeliveryPatient.Copy());
                oDataSet.Tables[0].TableName = "Master";

                oDataSet.Tables.Add(dtBatchDeliveryRx.Copy());
                oDataSet.Tables[1].TableName = "Detail";

                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["DelPatRecId"], oDataSet.Tables[1].Columns["DelPatRecId"]);

                grdData.DataSource = oDataSet;

                if (grdData != null && grdData.Rows.Count > 0)
                {
                    AddCheckBoxColumn();
                    FormatGrid();
                    this.resizeColumns();
                    //SetProcessedRowAppearance();
                    grdData.Focus();
                    grdData.PerformAction(UltraGridAction.FirstRowInGrid);
                }
                grdData.Refresh();
                dtAllHoldTransaction.Clear();
                //CheckHoldOrPaid();
                btnPayment.Visible = true;
                logger.Trace("PopulateGrid() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateGrid()");
            }
        }

        private void AddCheckBoxColumn()
        {
            logger.Trace("AddCheckBoxColumn() - " + clsPOSDBConstants.Log_Entering);
            // Parent
            if (this.grdData.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
            {
                this.grdData.DisplayLayout.Bands[0].Columns.Add("CHECK");
                this.grdData.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "";

                this.grdData.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                this.grdData.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            }
            this.grdData.DisplayLayout.Bands[0].Columns["Check"].Header.VisiblePosition = 0;
            this.grdData.DisplayLayout.Bands[0].Columns["Check"].Width = 112;

            // Child
            if (this.grdData.DisplayLayout.Bands[1].Columns.Exists("CHECK") == false)
            {
                this.grdData.DisplayLayout.Bands[1].Columns.Add("CHECK");
                this.grdData.DisplayLayout.Bands[1].Columns["CHECK"].Header.Caption = "";

                this.grdData.DisplayLayout.Bands[1].Columns["CHECK"].DataType = typeof(bool);
                this.grdData.DisplayLayout.Bands[1].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            }
            this.grdData.DisplayLayout.Bands[1].Columns["Check"].Header.VisiblePosition = 0;
            this.grdData.DisplayLayout.Bands[1].Columns["Check"].Width = 112;


            // OnHold Flag

            //this.grdData.DisplayLayout.Bands[1].Columns.Add("ONHOLD");
            //this.grdData.DisplayLayout.Bands[1].Columns["ONHOLD"].Header.Caption = "";

            //this.grdData.DisplayLayout.Bands[1].Columns["ONHOLD"].DataType = typeof(int);
            //this.grdData.DisplayLayout.Bands[1].Columns["ONHOLD"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Integer;
            //this.grdData.DisplayLayout.Bands[1].Columns["ONHOLD"].Hidden = true;
            logger.Trace("AddCheckBoxColumn() - " + clsPOSDBConstants.Log_Exiting);

#if BatchDel // Done
            if (this.grdData.DisplayLayout.Bands[1].Columns.Exists("CHECK") == false)
            {
                this.grdData.DisplayLayout.Bands[1].Columns.Add("CHECK");
                this.grdData.DisplayLayout.Bands[1].Columns["CHECK"].Header.Caption = "";

                this.grdData.DisplayLayout.Bands[1].Columns["CHECK"].DataType = typeof(bool);
                this.grdData.DisplayLayout.Bands[1].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            }
            this.grdData.DisplayLayout.Bands[1].Columns["Check"].Header.VisiblePosition = 0;
            this.grdData.DisplayLayout.Bands[1].Columns["Check"].Width = 112;
#endif
        }

        private void FormatGrid()
        {
            grdData.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
            grdData.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

            //visible fields - PatientName, PatientNo, Amount Collected, Reconciled Amount, TransId, Notes, DelInstruction, ReqDelDate		
            this.grdData.DisplayLayout.Bands[0].Columns["DelPatientName"].Header.VisiblePosition = 1;
            this.grdData.DisplayLayout.Bands[0].Columns["DelPatientName"].Header.Caption = "Patient Name";

            this.grdData.DisplayLayout.Bands[0].Columns["DelPatientNo"].Header.VisiblePosition = 2;
            this.grdData.DisplayLayout.Bands[0].Columns["DelPatientNo"].Header.Caption = "Patient No";

            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopay"].Header.VisiblePosition = 3;
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopay"].Header.Caption = "Copay to Collect";
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopay"].Format = "##0.00";
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopay"].NullText = "0.00";
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopay"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].Header.VisiblePosition = 4;
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].Header.Caption = "Reconciled Amount";
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].Format = "##0.00";
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].NullText = "0.00";
            this.grdData.DisplayLayout.Bands[0].Columns["TotalCopayCollected"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            this.grdData.DisplayLayout.Bands[0].Columns["TRANSNO"].Header.VisiblePosition = 5;
            this.grdData.DisplayLayout.Bands[0].Columns["Notes"].Header.VisiblePosition = 6;
            this.grdData.DisplayLayout.Bands[0].Columns["DelInstructions"].Header.VisiblePosition = 7;
            this.grdData.DisplayLayout.Bands[0].Columns["ReqDelDate"].Header.VisiblePosition = 8;

            grdData.DisplayLayout.Bands[0].Columns["DelPatientNo"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DelPatRecId"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DelStatus"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DelBatchId"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DelAddress"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["PatientAddress"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["FacilityAddress"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DateDelivered"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DelAcceptedBy"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["NonDelReason"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["Priority"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["Driver"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DeliveryDestination"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DriverInitials"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DeliveryNote"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["NonDeliveryNote"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["DeliveryMethod"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["ShipService_CD"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["Ship_TrackingNo"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["IsExcluded"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["ShipCustomCD"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["Reconciled"].Hidden = true;
            grdData.DisplayLayout.Bands[0].Columns["CopayCollectedPOS"].Hidden = true;
            //visible fields for detail - RxNoItemId, REfillNo, Description, Item Type, Qty, UnitPrice, Discount, Net Price, NonDelReason
            this.grdData.DisplayLayout.Bands[1].Columns["RxNo"].Header.VisiblePosition = 1;
            this.grdData.DisplayLayout.Bands[1].Columns["RxNo"].Header.Caption = "RxNo";
            this.grdData.DisplayLayout.Bands[1].Columns["RefillNo"].Header.VisiblePosition = 2;
            this.grdData.DisplayLayout.Bands[1].Columns["RefillNo"].Header.Caption = "RefillNo";
            this.grdData.DisplayLayout.Bands[1].Columns["DrugName"].Header.VisiblePosition = 3;
            this.grdData.DisplayLayout.Bands[1].Columns["DrugName"].Header.Caption = "Description";
            this.grdData.DisplayLayout.Bands[1].Columns["ItemType"].Header.VisiblePosition = 4;
            this.grdData.DisplayLayout.Bands[1].Columns["ItemType"].Header.Caption = "ItemType";

            this.grdData.DisplayLayout.Bands[1].Columns["Qty"].Header.VisiblePosition = 5;
            this.grdData.DisplayLayout.Bands[1].Columns["Qty"].Header.Caption = "Qty";
            this.grdData.DisplayLayout.Bands[1].Columns["Qty"].Format = "##0";
            this.grdData.DisplayLayout.Bands[1].Columns["Qty"].NullText = "0";
            this.grdData.DisplayLayout.Bands[1].Columns["Qty"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            this.grdData.DisplayLayout.Bands[1].Columns["UnitPrice"].Header.VisiblePosition = 6;
            this.grdData.DisplayLayout.Bands[1].Columns["UnitPrice"].Header.Caption = "UnitPrice";
            this.grdData.DisplayLayout.Bands[1].Columns["UnitPrice"].Format = "##0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["UnitPrice"].NullText = "0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["UnitPrice"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            this.grdData.DisplayLayout.Bands[1].Columns["NonDelReason"].Header.VisiblePosition = 7;
            this.grdData.DisplayLayout.Bands[1].Columns["NonDelReason"].Header.Caption = "NonDelReason";


            this.grdData.DisplayLayout.Bands[1].Columns["Copay"].Header.VisiblePosition = 8;
            this.grdData.DisplayLayout.Bands[1].Columns["Copay"].Header.Caption = "Copay";
            this.grdData.DisplayLayout.Bands[1].Columns["Copay"].Format = "##0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["Copay"].NullText = "0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["Copay"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollected"].Header.VisiblePosition = 9;
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollected"].Header.Caption = "CopayCollected";
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollected"].Format = "##0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollected"].NullText = "0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollected"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grdData.DisplayLayout.Bands[1].Columns["DelDetRecId"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["DelPatRecId"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["DelStatus"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["DateDelivered"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["DelAcceptedBy"].Hidden = true;
            //grdData.DisplayLayout.Bands[1].Columns["Copay"].Hidden = true;
            //grdData.DisplayLayout.Bands[1].Columns["CopayCollected"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["TransID"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["RxNoInt"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["RXNoRefillNo"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["IsExcluded"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["NonDeliveryDetailNote"].Hidden = true;
            grdData.DisplayLayout.Bands[1].Columns["PatientNo"].Hidden = true;

        }

        private void SetProcessedRowAppearance()
        {
            try
            {
                if (dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "False")
                {
                    //btnPOSStamped.Visible = true;
                    btnPayment.Visible = false;

                }
                else
                {
                    // btnPOSStamped.Visible = false;
                    btnPayment.Visible = true;
                    //CheckHoldOrPaid();
                }
#if BatchDel // Done
                if (IsPOSStamped)
                {
                    //enable the Payment Button
                    //disable the ProcessButton
                }
                else
                {
                    //enable the ProcessButton
                    //disable the Payment Button
                }
                    

#endif

                #region Commented
                //UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
                //foreach (UltraGridRow row in bandMaster.GetRowEnumerator(GridRowType.DataRow))
                //{


                //    if (dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "True" && row.Cells["TotalCopay"].Value.ToString() == row.Cells["TotalCopayCollected"].Value.ToString())
                //    {
                //        row.Cells["Check"].Value = false;
                //        row.Appearance.BackColor = Color.LightGreen;
                //        row.Update();
                //    }
                //    else if (dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "False" && row.Cells["TotalCopay"].Value.ToString() == row.Cells["TotalCopayCollected"].Value.ToString())
                //    {
                //        row.Cells["Check"].Value = false;
                //        row.Appearance.BackColor = Color.LightBlue;
                //        row.Update();
                //    }
#if BatchDel //Done
                    if(!IsPOSStamped)
                    {
                         row.Cells["Check"].Value = false;
                         row.Update();
                    }
                    else
                    {
                        if (row.Cells["TotalCopay"].Value.ToString() == row.Cells["TotalCopayCollected"].Value.ToString())
                        {
                            row.Cells["Check"].Value = false;
                            row.Appearance.BackColor = Color.LightGreen;
                            row.Update();
                        }
                    }
#endif
                //  }
#if BatchDel // Done
                    if(!IsPOSStamped)
                    {
                         row.Cells["Check"].Value = false;
                         row.Update();
                    }
                    else
                    {
                        if (row.Cells["Copay"].Value.ToString() == row.Cells["CopayCollected"].Value.ToString())
                        {
                            row.Cells["Check"].Value = false;
                            row.Appearance.BackColor = Color.LightGreen;
                            row.Update();
                        }
                    }
#endif
                #endregion
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "SetProcessedRowAppearance()");
            }
        }

        private void SetDefaultValues()
        {
            txtOTC.Text = "0";
            txtRx.Text = "0";
            txtItemsTotal.Text = "0";

            txtPatientsRemaining.Text = "0";
            txtPatientsReconciled.Text = "0";
            txtPatientsTotal.Text = "0";

            txtAmountRemaining.Text = "0.00";
            txtAmountReconciled.Text = "0.00";
            txtAmountTotal.Text = "0.00";
            //btnPOSStamped.Visible = false;
        }

        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            logger.Trace("CheckUncheckGridRow() - " + clsPOSDBConstants.Log_Entering);
            grdData.BeginUpdate();

            if ((bool)oCell.Value == false && oCell.Row.Appearance.BackColor != Color.LightGreen)
            {
                oCell.Value = true;
            }
            else
            {
                oCell.Value = false;
            }
            oCell.Row.Update();
            grdData.EndUpdate();
            logger.Trace("CheckUncheckGridRow() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdData.DisplayLayout.Bands)
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

        private void BatchSearch()
        {
            try
            {
                frmBatchSearch ofrmBatchSearch = new frmBatchSearch();
                ofrmBatchSearch.ShowDialog(this);
                if (!ofrmBatchSearch.IsCanceled)
                {
                    string strSelectedBatch = ofrmBatchSearch.SelectedBatch();

                    if (strSelectedBatch == "")
                        return;

                    txtDeliveryBatch.Text = strSelectedBatch;
                    DisplayBatch();
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        public void CheckHoldOrPaid()
        {
            // Child Gridview
            btnUnHold.Visible = false;
            DataTable dtHoldData = new DataTable();
            DataSet dsTransRxPaid = new DataSet();
            foreach (DataRow row in dtBatchDeliveryRx.Rows)
            {

                status = POSTransaction.CheckOnHoldOrAlreadyPaid(row, out dtHoldData, out dsTransRxPaid);
                if (status == "Hold")
                {
                    UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
                    foreach (UltraGridRow gvrow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                    {
                        if (gvrow.Cells["RxNo"].Value.ToString() == row["RxNo"].ToString())
                        {
                            //gvrow.Cells["Check"].Value = false;
                            //gvrow.Cells["ONHOLD"].Value = 1;
                            gvrow.Appearance.BackColor = Color.LightSkyBlue;
                            gvrow.Update();
                        }
                    }

                    dtAllHoldTransaction.Merge(dtHoldData);
                    btnUnHold.Visible = true;
                    btnPayment.Visible = false;
                }
                else if (status == "Paid")
                {

                    UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
                    foreach (UltraGridRow gvrow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                    {
                        if (gvrow.Cells["RxNo"].Value.ToString() == row["RxNo"].ToString())
                        {
                            gvrow.Cells["Check"].Value = false;
                            gvrow.Appearance.BackColor = Color.LightGreen;
                            gvrow.Update();
                        }


                        //if (gvrow.Cells["RxNo"].Value.ToString() == row["RxNo"].ToString() && dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "True" && gvrow.Cells["Copay"].Value.ToString() == gvrow.Cells["CopayCollected"].Value.ToString())
                        //{
                        //    gvrow.Cells["Check"].Value = false;
                        //    gvrow.Appearance.BackColor = Color.LightGreen;
                        //    gvrow.Update();
                        //}
                        //else if (gvrow.Cells["RxNo"].Value.ToString() == row["RxNo"].ToString() && dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "False" && gvrow.Cells["Copay"].Value.ToString() == gvrow.Cells["CopayCollected"].Value.ToString())
                        //{
                        //    gvrow.Cells["Check"].Value = false;
                        //    gvrow.Appearance.BackColor = Color.LightGreen;
                        //    gvrow.Update();
                        //}

                    }

                    using (ReconciliationDeliveryReport oReconciliationDeliveryReport = new ReconciliationDeliveryReport())
                    {
                        if (row["CopayCollected"].ToString() == "0.0000")
                        {
                            //dtGetBatchDeliveryDetails.Rows[0]["TotalCopayCollected"] = dsTransRxPaid.Tables[0].Rows[0]["paidamount"].ToString();
                            //dtBatchDeliveryPatient.AsEnumerable().Where(s => Convert.ToString(s["DelBatchId"]).Equals(dtGetBatchDeliveryDetails.Rows[0]["DelBatchRecId"].ToString()) && Convert.ToString(s["DelPatRecId"]).Equals(row["DelPatRecId"].ToString())).ToList().ForEach(D => D.SetField("TotalCopayCollected", dsTransRxPaid.Tables[0].Rows[0]["paidamount"].ToString()));
                            dtBatchDeliveryRx.AsEnumerable().Where(s => Convert.ToString(s["DelPatRecId"]).Equals(row["DelPatRecId"].ToString()) && Convert.ToString(s["RxNo"]).Equals(row["RxNo"].ToString())).ToList().ForEach(D => D.SetField("CopayCollected", row["Copay"].ToString()));
                            //oReconciliationDeliveryReport.UpdateBatchDeliveryPaymentStatus(dtGetBatchDeliveryDetails);
                            //oReconciliationDeliveryReport.UpdateBatchDeliveryOrderPaymentStatus(dtBatchDeliveryPatient);
                            oReconciliationDeliveryReport.UpdateBatchDeliveryDetailPaymentStatus(dtBatchDeliveryRx);
                        }
                    }
                    UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
                    foreach (UltraGridRow rowParent in bandMaster.GetRowEnumerator(GridRowType.DataRow))
                    {

                        if (Configuration.convertNullToDecimal(rowParent.Cells["TotalCopay"].Value).ToString("0.00") == Configuration.convertNullToDecimal(rowParent.Cells["TotalCopayCollected"].Value).ToString("0.00") + Configuration.convertNullToDecimal(rowParent.Cells["CopayCollectedPOS"].Value).ToString("0.00") && rowParent.Cells["TotalCopay"].Value.ToString() != "0.0000")
                        {
                            rowParent.Cells["Check"].Value = false;
                            rowParent.Appearance.BackColor = Color.LightGreen;
                            rowParent.Update();
                        }

                        //if (dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "True" && row.Cells["TotalCopay"].Value.ToString() == row.Cells["TotalCopayCollected"].Value.ToString())
                        //{
                        //    row.Cells["Check"].Value = false;
                        //    row.Appearance.BackColor = Color.LightGreen;
                        //    row.Update();
                        //}
                        //else if (dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "False" && row.Cells["TotalCopay"].Value.ToString() == row.Cells["TotalCopayCollected"].Value.ToString())
                        //{
                        //    row.Cells["Check"].Value = false;
                        //    row.Appearance.BackColor = Color.LightBlue;
                        //    row.Update();
                        //}
                    }

                }
                //else
                //{
                //    // stay normal and enabled
                //}


            }


            // Update POSSTamped
            //using (ReconciliationDeliveryReport oReconciliationDeliveryReport = new ReconciliationDeliveryReport())
            //{
            //    if (dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "False" || dtGetBatchDeliveryDetails.Rows[0]["POSStamped"].ToString() == "")
            //    {
            //        dtGetBatchDeliveryDetails.Rows[0]["POSStamped"] = true;
            //        oReconciliationDeliveryReport.UpdateBatchDeliveryPaymentStatus(dtGetBatchDeliveryDetails);
            //    }
            //}

        }

        public void EnableDisableOnHoldButton()
        {
            UltraGridBand bandMaster = this.grdData.DisplayLayout.Bands[0];
            UltraGridBand bandMasterDetails = this.grdData.DisplayLayout.Bands[1];
            foreach (UltraGridRow parentRow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            {
                foreach (UltraGridRow childRow in bandMasterDetails.GetRowEnumerator(GridRowType.DataRow))
                {
                    if (childRow.Appearance.BackColor == Color.LightSkyBlue)
                    {
                        btnUnHold.Visible = true;
                        btnPayment.Visible = false;
                    }
                }
            }
        }

        private void RefreshDisplay()
        {
            try
            {
                Display();
                PopulateGrid();
                //SetProcessedRowAppearance();
                CheckHoldOrPaid();
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "RefreshDisplay()");
            }
        }
        #endregion
    }
}
