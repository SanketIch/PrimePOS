using Elavon;
using Evertech;
using Infragistics.Win.UltraWinGrid;
using NLog;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.Resources;
using POS_Core.Resources.DelegateHandler;
using POS_Core.TransType;
using POS_Core_UI.Reports.ReportsUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmRecoveryTransaction : System.Windows.Forms.Form
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        public bool IsCanceled = true;
        public string strSelectedData;
        POSTransaction posTrans = null;
        private PccPaymentSvr objPccPmt = new PccPaymentSvr();
        DataTable dtStatus = new DataTable();
        DataTable dtUser = new DataTable();
        DataTable dtStation = new DataTable();
        public frmRecoveryTransaction()
        {
            InitializeComponent();
        }
        #region FORM EVENT
        private void frmRecoveryTransaction_Load(object sender, EventArgs e)
        {
            Clear();
            this.dtpFromDate.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDate.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            cboStatus.DataSource = GetStatus();
            cboStatus.DisplayMember = "ddlField";
            cboStatus.ValueMember = "ddlField";
            cboStatus.SelectedIndex = 0;
            cboStation.DataSource = GetStation();
            cboStation.DisplayMember = "ddlField";
            cboStation.ValueMember = "ddlField";
            cboStation.SelectedIndex = 0;
            cboUser.DataSource = GetUser();
            cboUser.DisplayMember = "ddlField";
            cboUser.ValueMember = "ddlField";
            cboUser.SelectedIndex = 0;
            Search();
            Resources.Message.Display("The Credit Card Log contains historical data for the purpose of troubleshooting possible issues. \nIt does not provide any real-time information on existing unprocessed transactions.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information); //PRIMEPOS-3248
        }
        private void frmRecoveryTransaction_KeyDown(object sender, KeyEventArgs e)
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
        private void frmRecoveryTransaction_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F2:
                    btnRecoverTransaction_Click(null, null);
                    break;
                case Keys.F4:
                    btnSearch_Click(null, null);
                    break;
                case Keys.F3:
                    btnViewTransactionData_Click(null, null);
                    break;
                case Keys.F6:
                    btnviewtransactiondetail_Click(null, null);
                    break;
            }
        }
        #endregion

        #region Methods
        private void Search()
        {


            DataSet dsCcTransmissionDetail = new DataSet();
            posTrans = new POSTransaction();
            try
            {

                DateTime dtFrom = DateTime.Parse(DateTime.Parse(this.dtpFromDate.Value.ToString()).ToShortDateString() + " " + this.txtFromTime.Text);
                DateTime dtTo = DateTime.Parse(DateTime.Parse(this.dtpToDate.Value.ToString()).ToShortDateString() + " " + this.txtToTime.Text);

                if (dtFrom <= dtTo)
                {
                    dsCcTransmissionDetail = posTrans.getCcTransmissionLog(dtFrom, dtTo, cboStatus.SelectedItem.DataValue.ToString(), cboStation.SelectedItem.DataValue.ToString(), cboUser.SelectedItem.DataValue.ToString());
                    if (dsCcTransmissionDetail.Tables.Count > 0)
                    {
                        DataTable selectedTable = null;
                        if (dsCcTransmissionDetail.Tables[0].Rows.Count > 0)
                        {

                            if (dsCcTransmissionDetail.Tables[0].AsEnumerable()
                                .Where(r => r.Field<string>("PaymentProcessor") == Configuration.CPOSSet.PaymentProcessor).Count() > 0)
                            {
                                selectedTable = dsCcTransmissionDetail.Tables[0].AsEnumerable()
                                   .Where(r => r.Field<string>("PaymentProcessor") == Configuration.CPOSSet.PaymentProcessor)
                                   .CopyToDataTable();
                            }

                            //tlpRecover.Visible = true;
                            pnlAdd.Visible = true;
                            tlpViewTransaction.Visible = true;
                        }
                        //grdRecovery.DataSource = selectedTable;
                        grdRecovery.DataSource = dsCcTransmissionDetail.Tables[0];
                        grdRecovery.DataBind();


                        GridColumnDisplay();
                        //btnRecoverTransaction.Visible = true;
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
            //TransNo,TransDateTime,TransAmount,TransDataStr,RecDataStr,PaymentProcessor, UserID ,  StationID ,TicketNo , TransmissionStatus, HostTransID ,POSTransID,POSPaymentID ,TransType,IsReversed, AmtApproved , TerminalRefNumber , ResponseMessage
            //PR-2761
            if (grdRecovery != null && grdRecovery.Rows.Count > 0)
            {
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransDataStr"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["RecDataStr"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["IsReversed"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["AmtApproved"].Hidden = true;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TerminalRefNumber"].Hidden = true;

                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransNo"].Header.VisiblePosition = 0;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransNo"].Header.Caption = "CC Record No";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["POSTransID"].Header.VisiblePosition = 1;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["POSTransID"].Header.Caption = "POS Trans ID";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["POSTransID"].CellClickAction = CellClickAction.CellSelect;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["POSTransID"].CellClickAction = CellClickAction.RowSelect;

                this.grdRecovery.DisplayLayout.Bands[0].Columns["ResponseMessage"].Header.VisiblePosition = 2;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["ResponseMessage"].Header.Caption = "Status";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransAmount"].Header.VisiblePosition = 3;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransAmount"].Header.Caption = "Trans Amount";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransAmount"].Format = "0.00";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransAmount"].NullText = "0.00";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransDateTime"].Header.VisiblePosition = 4;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransDateTime"].Header.Caption = "Transaction Date";
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransDateTime"].Format = "MM/dd/yyyy hh:mm tt";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransType"].Header.VisiblePosition = 5;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransType"].Header.Caption = "Trans Type";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["StationID"].Header.VisiblePosition = 6;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["StationID"].Header.Caption = "Station ID";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["UserID"].Header.VisiblePosition = 7;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["UserID"].Header.Caption = "User";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["HostTransID"].Header.VisiblePosition = 8;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["HostTransID"].Header.Caption = "Host Trans ID";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["PaymentProcessor"].Header.VisiblePosition = 9;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["PaymentProcessor"].Header.Caption = "Payment Processor";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["TicketNo"].Header.VisiblePosition = 10;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TicketNo"].Header.Caption = "TicketNo";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["POSPaymentID"].Header.VisiblePosition = 11;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["POSPaymentID"].Header.Caption = "POS Payment ID";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransmissionStatus"].Header.VisiblePosition = 12;
                this.grdRecovery.DisplayLayout.Bands[0].Columns["TransmissionStatus"].Header.Caption = "Transmission";

                this.grdRecovery.DisplayLayout.Bands[0].Columns["last4"].Header.VisiblePosition = 13; //PRIMEPOS-3182
                this.grdRecovery.DisplayLayout.Bands[0].Columns["last4"].Header.Caption = "Last4"; //PRIMEPOS-3182

                ResizeColumns();
                ColorChangeTransactionWise();
            }
            else
            {
                pnlAdd.Visible = false;
                tlpViewTransaction.Visible = false;
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
            this.ActiveControl = this.dtpFromDate;

        }
        private void ColorChangeTransactionWise()
        {
            UltraGridBand bandMaster = this.grdRecovery.DisplayLayout.Bands[0];
            foreach (UltraGridRow Parentrow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            {

                if (Parentrow.Cells["POSTransID"].Value.ToString() != "")
                {
                    Parentrow.Cells["TransNo"].Appearance.BackColor = Color.LightSkyBlue;
                    //Parentrow.Appearance.BackColor = Color.LightSkyBlue;
                    Parentrow.Update();
                }
                if (Parentrow.Cells["PaymentProcessor"].Value.ToString() != "HPSPAX")
                {
                    if (Parentrow.Cells["HostTransID"].Value.ToString() == "")
                    {
                        Parentrow.Cells["TransNo"].Appearance.BackColor = Color.Red;
                        //Parentrow.Appearance.BackColor = Color.Red;
                        Parentrow.Update();
                    }
                }
                else
                {
                    if (Parentrow.Cells["TerminalRefNumber"].Value.ToString() == "")
                    {
                        Parentrow.Cells["TransNo"].Appearance.BackColor = Color.Red;
                        //Parentrow.Appearance.BackColor = Color.Red;
                        Parentrow.Update();
                    }
                }
                if (Parentrow.Cells["PaymentProcessor"].Value.ToString() != Configuration.CPOSSet.PaymentProcessor)
                {
                    if (Parentrow.Cells["PaymentProcessor"].Value.ToString().ToUpper() != "PRIMERXPAY")//primepos-2841
                    {
                        Parentrow.Cells["TransNo"].Appearance.BackColor = Color.Red;
                        //Parentrow.Appearance.BackColor = Color.Red;
                        Parentrow.Update();
                    }
                }
                if (Parentrow.Cells["IsReversed"].Value != null && Parentrow.Cells["IsReversed"].Value.ToString() != "")
                {
                    if (Convert.ToBoolean(Parentrow.Cells["IsReversed"].Value))
                    {
                        Parentrow.Cells["TransNo"].Appearance.BackColor = Color.LightGreen;
                        //Parentrow.Appearance.BackColor = Color.LightGreen;
                        Parentrow.Update();
                    }
                }
                if (Parentrow.Cells["TransType"].Value.ToString().ToUpper().Contains("VOID"))
                {
                    Parentrow.Cells["TransNo"].Appearance.BackColor = Color.LightGray;
                    //Parentrow.Appearance.BackColor = Color.LightPink;
                    Parentrow.Update();
                }
                #region PRIMEPOS-3248
                if (Parentrow.Cells["TransType"].Value.ToString().ToUpper() == "PRIMERXPAY_LINK_SENT")
                {
                    Parentrow.Cells["TransNo"].Appearance.BackColor = Color.Red;
                    //Parentrow.Appearance.BackColor = Color.LightPink;
                    Parentrow.Update();
                }
                #endregion
                if (Parentrow.Cells["TransNo"].Appearance.BackColor.ToString() == "Color [Empty]")//Suggestions by Fahad
                {
                    if (Parentrow.Cells["PaymentProcessor"].Value.ToString() == "ELAVON" && Parentrow.Cells["POSTransID"].Value.ToString() == "")
                    {
                        Parentrow.Cells["TransNo"].Appearance.BackColor = Color.Red;
                        Parentrow.Update();
                    }
                    else
                    {
                        Parentrow.Appearance.BackColor = Color.LightPink;
                        Parentrow.Update();
                    }
                }
            }
        }
        private void ShortCutKeyAction(Keys KeyCode)
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

        private List<DropDown> GetStatus()
        {
            posTrans = new POSTransaction();
            List<DropDown> StatusList = new List<DropDown>();
            DataSet dsStatus = new DataSet();
            dsStatus = posTrans.getStatus();
            dtStatus = dsStatus.Tables[0];
            StatusList = (from DataRow dr in dtStatus.Rows
                          select new DropDown()
                          {
                              ddlField = Convert.ToString(dr["ResponseMessage"]).Trim(),
                              // ddlValue = Convert.ToString(dr["ResponseStatus"]),
                          }).ToList();
            return StatusList;
        }

        //private DataTable GetStatus()
        //{
        //    posTrans = new POSTransaction();
        //    DataSet dsStatus = new DataSet();
        //    dsStatus = posTrans.getStatus();
        //    dtStatus = dsStatus.Tables[0];

        //    return dtStatus;
        //}
        private List<DropDown> GetUser()
        {
            posTrans = new POSTransaction();
            List<DropDown> UserList = new List<DropDown>();
            DataSet dsUser = new DataSet();
            dsUser = posTrans.getUser();
            dtUser = dsUser.Tables[0];
            UserList = (from DataRow dr in dtUser.Rows
                        select new DropDown()
                        {
                            ddlField = Convert.ToString(dr["UserID"]).Trim(),
                            // ddlValue = Convert.ToString(dr["ResponseStatus"]),
                        }).ToList();
            return UserList;
        }

        private List<DropDown> GetStation()
        {
            posTrans = new POSTransaction();
            List<DropDown> StationList = new List<DropDown>();
            DataSet dsStation = new DataSet();
            dsStation = posTrans.getStation();
            dtStation = dsStation.Tables[0];
            StationList = (from DataRow dr in dtStation.Rows
                           select new DropDown()
                           {
                               ddlField = Convert.ToString(dr["StationID"]).Trim(),
                               // ddlValue = Convert.ToString(dr["ResponseStatus"]),
                           }).ToList();
            return StationList;
        }


        #endregion

        #region Control Event
        private void btnRecoverTransaction_Click(object sender, EventArgs e)
        {
            logger.Trace("btnRecoverTransaction_Click(object sender, EventArgs e) - Entered");
            try
            {
                posTrans = new POSTransaction();
                DataRow row = (grdRecovery.ActiveRow.ListObject as DataRowView).Row;
                PccCardInfo pccCardInfo = posTrans.getPccRecoveryInfo(row);

                if (grdRecovery.ActiveRow != null && grdRecovery.ActiveRow.IsDataRow)
                {
                    if (this.grdRecovery.ActiveRow.Cells["TransNo"].Appearance.BackColor == Color.LightGray)
                    {
                        Resources.Message.Display("This is void transaction!");
                        return;
                    }
                    if (this.grdRecovery.ActiveRow.Cells["TransNo"].Appearance.BackColor == Color.LightGreen)
                    {
                        Resources.Message.Display("This transaction is already voided!");
                        return;
                    }

                    if (this.grdRecovery.ActiveRow.Cells["TransNo"].Appearance.BackColor != Color.Red)
                    {
                        if (this.grdRecovery.ActiveRow.Cells["TransNo"].Appearance.BackColor != Color.LightSkyBlue)
                        {
                            //if (this.grdRecovery.ActiveRow.Appearance.BackColor != Color.LightGreen)
                            //{
                            //DataRow row = (grdRecovery.ActiveRow.ListObject as DataRowView).Row;
                            //PccCardInfo pccCardInfo = posTrans.getPccRecoveryInfo(row);

                            if (pccCardInfo.PaymentProcessor != "WORLDPAY")
                            {
                                if (pccCardInfo.PaymentProcessor.ToUpper() == "PRIMERXPAY")//PRIMEPOS-2841
                                {
                                    objPccPmt = new PccPaymentSvr(pccCardInfo.PaymentProcessor.ToUpper());
                                }
                                if (pccCardInfo.PaymentProcessor.ToUpper() == "EVERTEC")
                                {
                                    string possettings = Configuration.CPOSSet.SigPadHostAddr;

                                    string ipAddress = possettings.Split(':')[0];
                                    string port = possettings.Split(':')[1].Split('/')[0];

                                    EvertechProcessor evertecProcessor = EvertechProcessor.getInstance(ipAddress, Convert.ToInt32(port));
                                    if (evertecProcessor.isLoggedOn == false)
                                    {
                                        evertecProcessor.Logon(Configuration.CPOSSet.TerminalID, Configuration.StationID, Configuration.CashierID);
                                    }
                                }
                                if (pccCardInfo.PaymentProcessor.ToUpper() == "ELAVON")//2943
                                {
                                    string hostAddress = Configuration.CPOSSet.SigPadHostAddr.Split(':')[0];
                                    string hostPort = Configuration.CPOSSet.SigPadHostAddr.Split(':')[1].Split('/')[0];

                                    ElavonProcessor elavon = ElavonProcessor.getInstance(hostAddress, Convert.ToInt32(hostPort));
                                }
                                string ticketNum = Configuration.StationID + clsCoreUIHelper.GetRandomNo().ToString();
                                if (pccCardInfo.Transtype.ToString() == POSTransactionType.Sales.ToString())
                                {
                                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo);
                                }
                                else if (pccCardInfo.Transtype.ToString() == POSTransactionType.SalesReturn.ToString())
                                {
                                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo);
                                }
                                if (pccCardInfo.Transtype.ToString().Contains("DEBIT"))
                                {
                                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnDebitCardSales(ticketNum, ref pccCardInfo);
                                }
                            }
                            else // if (pccCardInfo.PaymentProcessor == "WORLDPAY")
                            {
                                pccCardInfo.StationID = Configuration.StationID;
                                pccCardInfo.UserId = Configuration.UserName;
                                PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnWP(FirstMile.TransactionType.Void, ref pccCardInfo);
                            }
                            //SetrecoveryCCTransmissionLog();
                            Resources.Message.Display("Transaction void successfully");
                            //}
                            //else
                            //{
                            //    Resources.Message.Display("This transaction is already voided!");
                            //}
                        }
                        else
                        {
                            Resources.Message.Display("This transaction is successfully completed\nso cannot be voided from system!");
                        }
                    }
                    else
                    {
                        if (pccCardInfo.PaymentProcessor != Configuration.CPOSSet.PaymentProcessor)
                        {
                            Resources.Message.Display("This transaction cannot be voided from system.\nPlease check payment processor of current station!");
                        }
                        else
                        {
                            Resources.Message.Display("This transaction cannot be voided from system.\nPlease contact Payment Gateway!");
                        }
                    }

                    Search();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "btnRecoverTransaction_Click(object sender, EventArgs e)");
                throw ex;
            }
            logger.Trace("btnRecoverTransaction_Click(object sender, EventArgs e) - Exited");
        }
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
            Search();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion

        private void btnViewTransactionData_Click(object sender, EventArgs e)
        {
            if (grdRecovery.ActiveRow != null && grdRecovery.ActiveRow.IsDataRow)
            {
                DataRow oRow = (grdRecovery.ActiveRow.ListObject as DataRowView).Row;

                frmTransmissionMessage oAddCards = new frmTransmissionMessage(oRow["TransDataStr"].ToString().Trim(), oRow["RecDataStr"].ToString().Trim());
                oAddCards.ShowDialog(this);

                //Resources.Message.Display("Request = "+ oRow["TransDataStr"].ToString().Trim() + "\nResponse = " + oRow["RecDataStr"].ToString().Trim(),"Transmission Details");
            }
        }


        private void ProcessTransactionViewRequest(string sTransId, string stationID)
        {
            logger.Trace("ProcessTransactionViewRequest() - " + clsPOSDBConstants.Log_Entering);
            frmViewTransactionDetail ofrmVTD = new frmViewTransactionDetail();
            if (sTransId.Trim().Length > 1)
            {
                String TransID = sTransId.Substring(1);

                if (clsUIHelper.isNumeric(sTransId))
                {
                    ofrmVTD = new frmViewTransactionDetail(sTransId, "", stationID, false);
                }
                else
                {
                    ofrmVTD = new frmViewTransactionDetail("0", "", stationID, false);
                }
            }
            else
            {
                ofrmVTD = new frmViewTransactionDetail("0", "", stationID, false);
            }
            ofrmVTD.ShowDialog();
            logger.Trace("ProcessTransactionViewRequest() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void grdRecovery_ClickCell(object sender, ClickCellEventArgs e)
        {
            //try
            //{
            //    if (e.Cell.Column.Key == "POSTransID")
            //    {

            //        UltraGridRow dr = grdRecovery.Rows[e.Cell.Row.Index];
            //        if (dr.Cells["POSTransID"].Value.ToString() != "")
            //        {
            //            dr.Cells["StationID"].Value.ToString();
            //            dr.Cells["TicketNo"].Value.ToString();//TicketNo
            //            ProcessTransactionViewRequest(dr.Cells["POSTransID"].Value.ToString(), dr.Cells["StationID"].Value.ToString());
            //        }
            //        else
            //        {
            //            Resources.Message.Display("This transaction dont have transaction no!");
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    clsUIHelper.ShowErrorMsg(Ex.Message);
            //    logger.Fatal(Ex, "grdRecovery_ClickCell(object sender, ClickCellEventArgs e)");
            //}
        }
        private void btnviewtransactiondetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdRecovery.ActiveRow != null && grdRecovery.ActiveRow.IsDataRow)
                {
                    DataRow oRow = (grdRecovery.ActiveRow.ListObject as DataRowView).Row;

                    if (oRow["POSTransID"].ToString() != "")
                    {
                        oRow["StationID"].ToString();
                        oRow["TicketNo"].ToString();//TicketNo
                        ProcessTransactionViewRequest(oRow["POSTransID"].ToString(), oRow["StationID"].ToString());
                    }
                    else
                    {
                        Resources.Message.Display("This transaction dont have transaction no!");
                    }
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                logger.Fatal(Ex, "grdRecovery_ClickCell(object sender, ClickCellEventArgs e)");
            }
        }
    }

    public class DropDown
    {

        public string ddlField { get; set; }
        //  public string ddlValue { get; set; }

    }



}
