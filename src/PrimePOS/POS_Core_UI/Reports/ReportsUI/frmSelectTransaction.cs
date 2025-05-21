using System;

using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POS_Core_UI.Layout;
using POS_Core.BusinessRules;
using POS_Core.Resources;
using NLog;
using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData;
using POS_Core_UI.UI;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmSelectTransaction : frmMasterLayout
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        private int CurrentX;
        private int CurrentY;
        //DataTable dtGetBatchDeliveryDetails = new DataTable();
        //DataTable dtBatchDeliveryPatient = new DataTable();
        //DataTable dtBatchDeliveryRx = new DataTable();
        //DataTable dtAllHoldTransaction = new DataTable();
        string status = string.Empty;
        private int selectedRowIndex = -1;
        //POSTransaction posTrans = null;
        public frmPOSTransaction ofrmTrans = null;
        public POSTransaction oPosTrans = null;
        string strPOSTransQuery = string.Empty;
        string strPOSTransDetailQuery = string.Empty;
        string strWhereDate = string.Empty;
        string strWhereTransId = string.Empty;
        string strGroupBy = string.Empty;
        public bool isClosed = false;
        public bool isViewTransaction = false;
        public bool isCopied = false;
        public bool isReturnCash = false;
        public string TransID = string.Empty;
        DataSet dsTrans = new DataSet();
        DataSet dsTransDetails = new DataSet();
        DataTable dtPOSTransaction = new DataTable();
        DataTable dtPOSTransactionDetails = new DataTable();
        bool alreadyFocused;
        #region PRIMEPOS-3105 16-Jun-2022 JY Added
        private bool misROATrans = false;
        public bool isROATrans
        {
            get
            {
                return misROATrans;
            }
        }
        #endregion

        //public frmViewStrictReturn(frmPOSTransaction frmTrans)
        //{
        //    InitializeComponent();
        //    //posTrans = new POSTransaction();
        //    ofrmTrans = frmTrans;
        //}
        //public frmViewStrictReturn(frmPOSTransaction frmTrans, string batchNo)
        //{
        //    InitializeComponent();
        //    //posTrans = new POSTransaction();
        //    ofrmTrans = frmTrans;
        //    txtTransID.Text = batchNo;
        //    //RefreshDisplay();
        //}
        public frmSelectTransaction(string TransNo)
        {
            InitializeComponent();
            txtTransID.Text = TransID = TransNo;
            clFrom.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            clFrom.Text = DateTime.Now.ToShortDateString();

            btnSearch.Visible = true;
            btnCopy.Visible = true;
            btnClose.Visible = true;
            btnViewTransaction.Visible = true;
            btnReturnCashItem.Visible = true;
            //posTrans = new POSTransaction();
        }

        #region Form Events
        private void frmViewStrictReturn_Load(object sender, EventArgs e)
        {
            logger.Trace("frmViewStrictReturn_Load(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);

            this.clFrom.Value = DateTime.Today.AddDays(-1);
            this.clTo.Value = DateTime.Today;

            clsUIHelper.SetAppearance(this.grdData);
            clsUIHelper.SetReadonlyRow(this.grdData);

            this.Left = (frmMain.getInstance().Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;
            //SetDefaultValues();
            this.txtTransID.Focus();
            this.clFrom.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.clFrom.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.clTo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.clTo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //DataSet ds = new DataSet();
            oPosTrans = new POSTransaction();
            getReturnDetails();
            /*
            GenerateSqlQuery(txtTransID.Text, txtFromDate.Text, txtToDate.Text);
            Search oSearch = new Search();
            string strCQuery = string.Empty;
            if (txtTransID.Text != string.Empty && txtTransID.Text != "0")
                strCQuery = strQuery + strWhereTransId + strGroupBy;
            else
                strCQuery = strQuery + strWhereDate + strGroupBy;
            ds = oSearch.SearchData(strCQuery);
            //ds = oPosTrans.GetPatient

            grdData.DataSource = ds;
            grdData.DataBind();
            */
            //if (txtTransID.Text == string.Empty)
            //{
            //    btnUnHold.Visible = false;
            //    btnPayment.Visible = false;
            //}
            clsUIHelper.setColorSchecme(this);
            logger.Trace("frmViewStrictReturn_Load(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmViewStrictReturn_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Trace("frmViewStrictReturn_KeyDown(object sender, KeyEventArgs e) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                //Added this because earlier it was taking the key up event even if it's constructor is called so added this to remove that                
                //if (e.KeyData == System.Windows.Forms.Keys.Enter)
                //{
                //    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                //}
                /*else*/
                if (e.KeyData == Keys.Escape)//PRIMEPOS-2738 ADDED NEW PARAMETER FOR STRICT RETURN
                {
                    isClosed = true;
                    this.Close();
                }
                else if (e.KeyData == Keys.F2)
                {
                    isCopied = true;
                    this.Close();
                }
                else if (e.KeyData == Keys.F4)
                {
                    btnSearch_Click(null, null);
                }
                else if (e.KeyData == Keys.F5)
                {
                    btnViewTransaction_Click(null, null);
                }
                else if (e.KeyData == Keys.F3)
                {
                    btnReturnCashItem_Click(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            logger.Trace("frmViewStrictReturn_KeyDown(object sender, KeyEventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmViewStrictReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Trace("frmViewStrictReturn_FormClosing(object sender, FormClosingEventArgs e) - " + clsPOSDBConstants.Log_Entering);
            if (isCopied == true)
            {
                if (grdData.Rows.Count > 0)
                {
                    #region PRIMEPOS-3189
                    if (grdData.ActiveRow.Band.Index > 0)
                    {
                        this.grdData.ActiveRow = this.grdData.ActiveRow.ParentRow;
                    }
                    #endregion

                    this.TransID = grdData.ActiveRow.Cells[clsPOSDBConstants.TransHeader_Fld_TransID].Value.ToString();

                    #region PRIMEPOS-3105 16-Jun-2022 JY Added
                    if (grdData.ActiveRow.Cells[clsPOSDBConstants.TransHeader_Fld_TransType].Value.ToString().Trim().Equals("ROA"))
                    {
                        misROATrans = true;
                    }
                    else
                    {
                        misROATrans = false;
                    }
                    #endregion
                }
            }
            logger.Trace("frmViewStrictReturn_FormClosing(object sender, FormClosingEventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        #endregion

        #region Control Events        


        private void btnSearch_Click(object sender, EventArgs e)
        {
            logger.Trace(" btnSearch_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);

            DateTime dtFrom = new DateTime();
            DateTime.TryParse(clFrom.Text, out dtFrom);
            DateTime dtTo = new DateTime();
            DateTime.TryParse(clTo.Text, out dtTo);

            if (dtFrom > dtTo)
            {
                clsUIHelper.ShowErrorMsg(" From date cannot be greater than To date");
                return;
            }

            if (txtTransID.Text.Contains("?")){
                txtTransID.Text = txtTransID.Text.Remove(0, 1);
            }

            //Search oSearch = new Search();
            //DataSet ds = new DataSet();
            //string strCQuery = string.Empty;
            //if (txtTransID.Text != string.Empty && txtTransID.Text != "0")
            //    strCQuery = strQuery + @" AND PT.TransID = " + txtTransID.Text + "";
            //else
            //    strCQuery = strQuery + @" AND convert(date, cast(PT.TransDate as date), 101) between 	'" + txtFromDate.Text + "' and '" + txtToDate.Text + "'" + strGroupBy;
            //ds = oSearch.SearchData(strCQuery);

            //grdData.DataSource = ds;
            //grdData.DataBind();

            getReturnDetails();

            this.grdData.Focus();
            logger.Trace(" btnSearch_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnViewTransaction_Click(object sender, EventArgs e)
        {
            logger.Trace(" btnViewTransaction_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            if (grdData.Rows.Count > 0)
            {
                #region PRIMEPOS-3189
                if (grdData.ActiveRow.Band.Index > 0)
                {
                    this.grdData.ActiveRow = this.grdData.ActiveRow.ParentRow;
                }
                #endregion

                this.TransID = grdData.ActiveRow.Cells[clsPOSDBConstants.TransHeader_Fld_TransID].Value.ToString();
                isViewTransaction = true;

                #region PRIMEPOS-3105 16-Jun-2022 JY Added
                if (grdData.ActiveRow.Cells[clsPOSDBConstants.TransHeader_Fld_TransType].Value.ToString().Trim().Equals("ROA"))
                {
                    misROATrans = true;
                }
                else
                {
                    misROATrans = false;
                }
                #endregion
                this.Close();
            }
            logger.Trace(" btnViewTransaction_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnReturnCashItem_Click(object sender, EventArgs e)
        {
            logger.Trace(" btnReturnCashItem_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            isReturnCash = true;
            //isClosed = true;
            //isCopied = true; // Nileshj
            this.Close();
            logger.Trace(" btnReturnCashItem_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            logger.Trace(" btnCopy_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            isCopied = true;
            this.Close();
            logger.Trace(" btnCopy_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            logger.Trace(" btnClose_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);
            isClosed = true;
            this.Close();
            logger.Trace(" btnClose_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }


        private void txtTransID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }

        #region Ultragrid events
        private void grdData_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Trace("grdData_KeyDown(object sender, KeyEventArgs e) - " + clsPOSDBConstants.Log_Entering);
            //if (e.KeyCode == Keys.Space)
            //{
            //}

            logger.Trace("grdData_KeyDown(object sender, KeyEventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }
        private void grdData_MouseClick(object sender, MouseEventArgs e)
        {
            logger.Trace("grdData_MouseClick(object sender, MouseEventArgs e) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdData.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdData.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
                if (oUI == null)
                    return;
            }
            catch (Exception Ex)
            {
                logger.Trace(Ex.Message);
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            logger.Trace(" grdData_MouseClick(object sender, MouseEventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }
        private void grdData_MouseMove(object sender, MouseEventArgs e)
        {
            logger.Trace(" grdData_MouseMove(object sender, MouseEventArgs e) - " + clsPOSDBConstants.Log_Entering);
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
            logger.Trace(" grdData_MouseMove(object sender, MouseEventArgs e) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        #endregion

        #region Methods

        /*
        private void GenerateSqlQuery(string TransId, string DateFrom, string DateTo)
        {
            logger.Trace(" GenerateSqlQuery(string TransId, string DateFrom, string DateTo) - " + clsPOSDBConstants.Log_Entering);
            //strQuery = @" select PT.TransID,PT.TransDate,PT.CustomerID,PT.UserID,PT.StationID,PT.GrossTotal,PT.TotalDiscAmount, 
            //                     PT.TotalTaxAmount, PT.TotalPaid , Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType,
            //                       PTD.Qty,PTD.ItemID, PTD.ItemDescription as Description, PTD.Price,A.TaxIds as TaxID,PTD.TaxAmount,PTD.Discount,PTD.ExtendedPrice,ps.stationname  ,
            //                        (CustomerName + ', ' + FIRSTNAME) AS CustName  from postransaction PT left join POSTransactionDetail PTD on PT.TransID=PTD.TransID
            //                   left join Item I on I.ItemID=PTD.ItemID 
            //                left join(select DISTINCT a.TransDetailID,
            //               STUFF((SELECT distinct ',' + tc.TaxCode from POSTransactionDetailTax a1 
            //                 left join TaxCodes tc on tc.TaxID = a1.TaxID  where a.TransDetailID = a1.TransDetailID
            //                 FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') TaxIds from POSTransactionDetailTax a) A ON A.TransDetailID = PTD.TransDetailID  
            //           left join util_POSSet ps on (ps.stationid=pt.stationid)  LEFT JOIN Customer C ON c.CustomerID = PT.CustomerID where 1=1 ";


            strPOSTransQuery = @" select PT.TransID , (CustomerName + ', ' + FIRSTNAME) AS CustName , PT.TotalPaid, PT.TotalTaxAmount,PT.TotalDiscAmount, 
                        Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType,PT.TransDate,PT.UserID,PT.StationID
                        from postransaction PT 
                        left join POSTransactionDetail PTD on PT.TransID=PTD.TransID
                        left join Item I on I.ItemID=PTD.ItemID 
                        left join(select DISTINCT a.TransDetailID,STUFF((SELECT distinct ',' + tc.TaxCode from POSTransactionDetailTax a1 
                        left join TaxCodes tc on tc.TaxID = a1.TaxID  where a.TransDetailID = a1.TransDetailID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') TaxIds from POSTransactionDetailTax a) A ON A.TransDetailID = PTD.TransDetailID  
                        left join util_POSSet ps on (ps.stationid=pt.stationid)  LEFT JOIN Customer C ON c.CustomerID = PT.CustomerID where 1=1  "; // NileshJ


            if (TransId != "" && TransId != "0")
            {
                strWhereTransId = @" AND PT.TransID = " + TransId + "";
            }
            else
                strWhereDate = @" AND convert(date,
                    		  cast(PT.TransDate as date), 101) between 	'" + DateFrom + "' and '" + DateTo + "'";

            strGroupBy = @" group by PT.TransID ,CustomerName,FIRSTNAME,TransType,PT.TotalPaid, PT.TotalTaxAmount ,PT.TotalDiscAmount,PT.TransDate,PT.UserID,PT.StationID "; // NileshJ
            logger.Trace(" GenerateSqlQuery(string TransId, string DateFrom, string DateTo) - " + clsPOSDBConstants.Log_Exiting);
        }
        */

        private void GenerateSqlPOSTransQuery(string TransId, string DateFrom, string DateTo)
        {
            logger.Trace(" GenerateSqlPOSTransQuery(string TransId, string DateFrom, string DateTo) - " + clsPOSDBConstants.Log_Entering);

            strPOSTransQuery = @" select PT.TransID , (CustomerName + ', ' + FIRSTNAME) AS CustName , PT.TotalPaid, PT.TotalTaxAmount,PT.TotalDiscAmount, 
                        Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType,PT.TransDate,PT.UserID,PT.StationID
                        from postransaction PT 
                        left join POSTransactionDetail PTD on PT.TransID=PTD.TransID
                        left join Item I on I.ItemID=PTD.ItemID 
                        left join(select DISTINCT a.TransDetailID,STUFF((SELECT distinct ',' + tc.TaxCode from POSTransactionDetailTax a1 
                        left join TaxCodes tc on tc.TaxID = a1.TaxID  where a.TransDetailID = a1.TransDetailID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') TaxIds from POSTransactionDetailTax a) A ON A.TransDetailID = PTD.TransDetailID  
                        left join util_POSSet ps on (ps.stationid=pt.stationid)  LEFT JOIN Customer C ON c.CustomerID = PT.CustomerID where 1=1 and (PT.TransType = 1 OR (PT.TransType = 3 AND PT.TotalPaid > 0))"; // NileshJ  //PRIMEPOS-3105 16-Jun-2022 JY modified to include ROA

            if (TransId != "" && TransId != "0")
            {
                strPOSTransQuery += @" AND PT.TransID = " + TransId + "";
            }
            else
                strPOSTransQuery += @" AND convert(date,cast(PT.TransDate as date), 101) between 	'" + DateFrom + "' and '" + DateTo + "'";

            strPOSTransQuery += @" group by PT.TransID ,CustomerName,FIRSTNAME,TransType,PT.TotalPaid, PT.TotalTaxAmount ,PT.TotalDiscAmount,PT.TransDate,PT.UserID,PT.StationID Order by PT.TransID desc ;";

            logger.Trace(" GenerateSqlPOSTransQuery(string TransId, string DateFrom, string DateTo) - " + clsPOSDBConstants.Log_Exiting);
        }

        private string GenerateSqlPOSTransDetailsQuery(string TransId)
        {
            logger.Trace(" GenerateSqlPOSTransDetailsQuery(string TransId, string DateFrom, string DateTo) - " + clsPOSDBConstants.Log_Entering);

            strPOSTransDetailQuery = @" select TransDetailID,TransID,TaxID,ItemID,ItemDescription,Qty,Price,Discount,TaxAmount,ExtendedPrice
                        ,OrignalPrice
                         from  POSTransactionDetail   where 1=1  ";
            if (TransId != "" && TransId != "0")
            {
                strPOSTransDetailQuery += @" AND TransID in( " + TransId + ")";
            }
            logger.Trace(" GenerateSqlPOSTransDetailsQuery(string TransId, string DateFrom, string DateTo) - " + clsPOSDBConstants.Log_Exiting);
            return strPOSTransDetailQuery;
        }

        /*
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

            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollectedPOS"].Header.VisiblePosition = 10;
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollectedPOS"].Header.Caption = "POS Copay Collected";
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollectedPOS"].Format = "##0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollectedPOS"].NullText = "0.00";
            this.grdData.DisplayLayout.Bands[1].Columns["CopayCollectedPOS"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

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
     */

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

        private void getReturnDetails()
        {
            logger.Trace(" getReturnDetails() - " + clsPOSDBConstants.Log_Entering);
            try
            {               

                /*
                //GenerateSqlQuery(txtTransID.Text, txtFromDate.Text, txtToDate.Text);
                //string strCQuery = string.Empty;
                //if (txtTransID.Text != string.Empty && txtTransID.Text != "0")
                //{
                //    strCQuery = strPOSTransQuery + strWhereTransId + strGroupBy;
                //}
                //else
                //{
                //    strCQuery = strPOSTransQuery + strWhereDate + strGroupBy;
                //}
                */
                Search oSearch = new Search();
                GenerateSqlPOSTransQuery(txtTransID.Text, clFrom.Text, clTo.Text);
                dsTrans = oSearch.SearchData(strPOSTransQuery);


                if (dsTrans.Tables.Count > 0)
                {
                    if (dsTrans.Tables[0].Rows.Count > 0)
                    {
                        dtPOSTransaction = dsTrans.Tables[0];
                        if (dtPOSTransaction.Rows.Count > 0)
                        {
                            string commaSeparatedString = String.Join(",", dtPOSTransaction.AsEnumerable().Select(x => x.Field<int>("TransID").ToString()).ToArray());
                            GenerateSqlPOSTransDetailsQuery(commaSeparatedString);
                            dsTransDetails = oSearch.SearchData(strPOSTransDetailQuery);
                        }
                        dtPOSTransactionDetails = dsTransDetails.Tables[0];
                        PopulateGrid();
                        //grdData.DataSource = ds;
                        //grdData.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message);
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            logger.Trace(" getReturnDetails() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void PopulateGrid()
        {
            try
            {
                logger.Trace("PopulateGrid() - " + clsPOSDBConstants.Log_Entering);

                DataSet oDataSet = new DataSet();
                oDataSet.Tables.Add(dtPOSTransaction.Copy());
                oDataSet.Tables[0].TableName = "Master";

                oDataSet.Tables.Add(dtPOSTransactionDetails.Copy());
                oDataSet.Tables[1].TableName = "Detail";

                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["TransID"], oDataSet.Tables[1].Columns["TransID"]);

                grdData.DataSource = oDataSet;

                if (grdData != null && grdData.Rows.Count > 0)
                {
                    // AddCheckBoxColumn();
                    // FormatGrid();
                    this.resizeColumns();
                    //SetProcessedRowAppearance();
                    grdData.Focus();
                    grdData.PerformAction(UltraGridAction.FirstRowInGrid);
                }
                grdData.Refresh();
                logger.Trace("PopulateGrid() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "PopulateGrid()");
            }
        }

        #endregion

        private void txtTransID_Enter(object sender, EventArgs e)
        {
            // Select all text only if the mouse isn't down.
            // This makes tabbing to the textbox give focus.
            if (MouseButtons == MouseButtons.None)
            {
                this.txtTransID.SelectAll();
                alreadyFocused = true;
            }
        }

        private void txtTransID_MouseUp(object sender, MouseEventArgs e)
        {
            // Web browsers like Google Chrome select the text on mouse up.
            // They only do it if the textbox isn't already focused,
            // and if the user hasn't selected all text.
            if (!alreadyFocused && this.txtTransID.SelectionLength == 0)
            {
                alreadyFocused = true;
                this.txtTransID.SelectAll();
            }
        }

        private void txtTransID_Leave(object sender, EventArgs e)
        {
            alreadyFocused = false;
        }
    }
}
