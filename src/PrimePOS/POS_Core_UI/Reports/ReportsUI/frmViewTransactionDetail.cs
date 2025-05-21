using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core_UI.Reports.Reports;
using System.Data;
using POS_Core.BusinessRules;
//using POS.UI;
using POS_Core.CommonData;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Drawing.Imaging;
using POS_Core.Resources;
using POS_Core.LabelHandler.RxLabel;
using NLog;
using POS_Core.Resources.DelegateHandler;
//using POS_Core.DataAccess;
//using System.Windows.Media.Imaging;
//using System.Windows.Media;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmReportViewer.
    /// </summary>
    public class frmViewTransactionDetail : System.Windows.Forms.Form
    {
        private String strQuery;
        private Int32 CurrentInvoiceNo;
        private Int32 MaxInvoiceNo;
        private String strWhere;
        private String strSubQuery;
        private bool isForCopy;
        private bool misCopied = false;
        private bool misROATrans = false;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public CrystalDecisions.Windows.Forms.CrystalReportViewer rptViewer;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnPrevious;
        private Infragistics.Win.Misc.UltraButton btnNext;
        private Infragistics.Win.Misc.UltraButton btnCopy;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        Image brLabel;
        rptPrintTransaction oRptPrintTrancsaction = null;
        private Infragistics.Win.Misc.UltraButton btnPrintDuplicate;
        private GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransStationID;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransDate;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransUserID;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransType;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransID;
        private GroupBox groupBox5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel lblHExtPrice;
        private Infragistics.Win.Misc.UltraLabel lblHTaxCd;
        private Infragistics.Win.Misc.UltraLabel lblHDisc;
        private Infragistics.Win.Misc.UltraLabel lblHUPrice;
        private Infragistics.Win.Misc.UltraLabel lblHQty;
        private Infragistics.Win.Misc.UltraLabel lblHItemDescription;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
        private GroupBox groupBox7;
        private GroupBox groupBox6;
        private ListView lvPayType;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransTotalAmount;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransTotalTax;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransTotalDiscount;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransSubtotal;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private ColumnHeader columnHeader3;
        public CrystalDecisions.Windows.Forms.CrystalReportViewer rvReportViewer;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransCustomer;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraButton btnCloseReport;
        private Infragistics.Win.Misc.UltraButton btnEmailReceipt;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtLineItemCnt;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        #region PRIMEPOS-2738
        public bool isClosed = false;
        public bool isFirstTime = false;
        private Infragistics.Win.Misc.UltraButton btnPrintGiftReceipt;
        #endregion
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransFee;
        private Infragistics.Win.Misc.UltraLabel lblTransactionFee;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        //Following Constructor is Added By shitaljit(QuicSolv) on 22 Dec 2011
        public frmViewTransactionDetail()
        {
            InitializeComponent();
        }
        public frmViewTransactionDetail(String TransNo, String UserID, String StationID, bool isCopy, bool canClose = true)//PRIMEPOS-2738 add default parameter ARVIND
        {
            //
            // Required for Windows Form Designer support
            //
            isForCopy = isCopy;

            #region Sprint-27 - PRIMEPOS-2456 10-Oct-2017 JY Commented
            //strQuery = " select PT.TransID,PT.TransDate,PT.CustomerID,PT.UserID,PT.StationID,PT.GrossTotal,PT.TotalDiscAmount, PT.TotalTaxAmount, PT.TotalPaid ," +
            //            " Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType, " +
            //            " PTD.Qty,PTD.ItemID, PTD.ItemDescription as Description, PTD.Price,Tx.TaxCode as TaxID,PTD.TaxAmount,PTD.Discount,PTD.ExtendedPrice,ps.stationname " +
            //            " from postransaction PT left join POSTransactionDetail PTD on PT.TransID=PTD.TransID " +
            //            " left join Item I on I.ItemID=PTD.ItemID  " +
            //            " left join taxcodes tx on (ptd.taxid=tx.taxid) " +
            //            " left join util_POSSet ps on (ps.stationid=pt.stationid) where 1=1 ";   //and PT.StationID='" + StationID.Trim() + "'";

            ////Modified The Sub Query By shitaljit To Dispaly Payments of singgle Paytype togethr in single Row.
            //strSubQuery = "SELECT PAY.PAYTYPEDESC AS DESCRIPTION,cast(PT.TransAmt as varchar) as TransAmt, " +
            //            " case CHARINDEX('|',isnull(refno,'')) when  0 then refno else '******'+right(rtrim(left(refno,CHARINDEX('|',refno)-1)) ,4) End as RefNo ,PT.TRANSID " +
            //            // " FROM POSTRANSPAYMENT PT,PAYTYPE PAY where PT.TransTypeCode=Pay.PayTypeID ";
            //            " FROM PAYTYPE PAY, (Select RefNo,Transid, TransTypeCode, sum(POSTransPayment.Amount) " +
            //            "as TransAmt from POSTRANSPAYMENT  GROUP BY TransTypeCode,Transid,RefNo)  as PT WHERE PT.TransTypeCode = Pay.PayTypeID";


            //if (TransNo.Trim() != "" && TransNo.Trim() != "0")
            //    strWhere = " and PT.TransID=" + TransNo.Trim();
            //else
            //{
            //    if (UserID.Trim() != "")
            //        strWhere = " and PT.UserID='" + UserID.Trim() + "'";
            //    //if (StationID.Trim()!="")
            //    //	strWhere += " and PT.StationID='" + StationID.Trim() + "'";
            //    //else
            //    strWhere += " and PT.TransID =(Select Max(TransID) from POSTransaction)";
            //}
            #endregion

            GenerateSQL(TransNo, UserID, StationID);    //Sprint-27 - PRIMEPOS-2456 10-Oct-2017 JY Added

            InitializeComponent();
            this.btnCopy.Visible = isCopy;
            this.PrintPreview();
            #region PRIMEPOS-2738
            if (canClose == false && Configuration.CSetting.StrictReturn == true)
            {
                this.btnClose.Visible = false;
                this.ControlBox = false;
            }
            #endregion
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        #region Sprint-27 - PRIMEPOS-2456 10-Oct-2017 JY Added
        private void GenerateSQL(String TransNo, String UserID, String StationID)
        {
            //PRIMEPOS-2501 04-Apr-2018 JY Added logic to update Taxcode in transdetail so that it would displayed it
            //PRIMEPOS-2560 17-Jul-2018 JY Added CustName
            strQuery = " select PT.TransID,PT.TransDate,PT.CustomerID,PT.UserID,PT.StationID,PT.GrossTotal,PT.TotalDiscAmount, PT.TotalTaxAmount, PT.TotalPaid ," +
                        " Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType, " +
                        " PTD.Qty,PTD.ItemID, PTD.ItemDescription as Description, PTD.Price,A.TaxIds as TaxID,PTD.TaxAmount,PTD.Discount,PTD.ExtendedPrice,ps.stationname " +
                        " , (CustomerName + ', ' + FIRSTNAME) AS CustName, PT.TotalTransFeeAmt " +  //PRIMEPOS-3117 11-Jul-2022 JY Added TotalTransFeeAmt
                        " from postransaction PT left join POSTransactionDetail PTD on PT.TransID=PTD.TransID " +
                        " left join Item I on I.ItemID=PTD.ItemID " +
                        " left join(select DISTINCT a.TransDetailID, STUFF((SELECT distinct ',' + tc.TaxCode from POSTransactionDetailTax a1" +
                                                                            " left join TaxCodes tc on tc.TaxID = a1.TaxID " +
                                            " where a.TransDetailID = a1.TransDetailID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') TaxIds" + 
                                    " from POSTransactionDetailTax a) A ON A.TransDetailID = PTD.TransDetailID " +
                        " left join util_POSSet ps on (ps.stationid=pt.stationid) " +
                        " LEFT JOIN Customer C ON c.CustomerID = PT.CustomerID where 1=1 ";

            //PRIMEPOS-2783 18-Feb-2020 JY Commented
            //strSubQuery = "SELECT PAY.PAYTYPEDESC AS DESCRIPTION,cast(PT.TransAmt as varchar) as TransAmt, " +
            //            " case CHARINDEX('|',isnull(refno,'')) when  0 then refno else '******'+right(rtrim(left(refno,CHARINDEX('|',refno)-1)) ,4) End as RefNo ,PT.TRANSID " +
            //            " FROM PAYTYPE PAY, (Select RefNo,Transid, TransTypeCode, sum(POSTransPayment.Amount) " +
            //            "as TransAmt from POSTRANSPAYMENT  GROUP BY TransTypeCode,Transid,RefNo)  as PT WHERE PT.TransTypeCode = Pay.PayTypeID";

            //PRIMEPOS-2783 18-Feb-2020 JY Added to split coupon/CL coupon
            //PRIMEPOS-2860 12-Jun-2020 JY Added PAY.PayTypeID and PAYMENTPROCESSOR
            strSubQuery = "SELECT PAY.PayTypeDesc AS DESCRIPTION, CAST(PT.TransAmt AS VARCHAR) AS TransAmt," +
                        " CASE CHARINDEX('|', isnull(PT.RefNo, '')) WHEN 0 THEN" +
                        " (CASE WHEN ISNULL(PT.CLCOUPONID, 0) <> 0 THEN 'CL Coupon# ' + CAST(CLCOUPONID AS VARCHAR(100)) ELSE PT.RefNo END)" +
                        " ELSE '******' + RIGHT(RTRIM(LEFT(PT.RefNo, CHARINDEX('|', PT.RefNo) - 1)), 4) END AS RefNo, PT.TRANSID, PAY.PayTypeID, PT.PAYMENTPROCESSOR" +
                        " FROM PAYTYPE PAY, (SELECT RefNo, TransID, TransTypeCode, SUM(POSTransPayment.Amount)AS TransAmt, CLCOUPONID, CASE WHEN PAYMENTPROCESSOR IS NULL OR PAYMENTPROCESSOR = 'N/A' THEN '' ELSE PAYMENTPROCESSOR END AS PAYMENTPROCESSOR FROM POSTRANSPAYMENT GROUP BY TransTypeCode, TransID, RefNo, CLCOUPONID, PAYMENTPROCESSOR) AS PT" +
                        " WHERE PT.TransTypeCode = Pay.PayTypeID";

            if (TransNo.Trim() != "" && TransNo.Trim() != "0")
            {
                strWhere = " and PT.TransID=" + TransNo.Trim();
            }
            else
            {
                if (UserID.Trim() != "")
                    strWhere = " and PT.UserID='" + UserID.Trim() + "'";

                strWhere += " and PT.TransID =(Select Max(TransID) from POSTransaction)";
            }
        }
        #endregion

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewTransactionDetail));
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Qty");
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemDescription");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Price");
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Discount");
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TaxCode");
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TaxAmount");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExtendedPrice");
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPrintGiftReceipt = new Infragistics.Win.Misc.UltraButton();
            this.btnEmailReceipt = new Infragistics.Win.Misc.UltraButton();
            this.rptViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.btnPrintDuplicate = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnCopy = new Infragistics.Win.Misc.UltraButton();
            this.btnNext = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnPrevious = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtTransFee = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionFee = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransTotalAmount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransTotalTax = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransTotalDiscount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransSubtotal = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtTransCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTransDate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTransUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTransType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtLineItemCnt = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.lblHExtPrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblHTaxCd = new Infragistics.Win.Misc.UltraLabel();
            this.lblHDisc = new Infragistics.Win.Misc.UltraLabel();
            this.lblHUPrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblHQty = new Infragistics.Win.Misc.UltraLabel();
            this.lblHItemDescription = new Infragistics.Win.Misc.UltraLabel();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lvPayType = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rvReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.btnCloseReport = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransID)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransTotalTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransTotalDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransSubtotal)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransType)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLineItemCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnPrintGiftReceipt);
            this.groupBox1.Controls.Add(this.btnEmailReceipt);
            this.groupBox1.Controls.Add(this.rptViewer);
            this.groupBox1.Controls.Add(this.btnPrintDuplicate);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.btnCopy);
            this.groupBox1.Controls.Add(this.btnNext);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnPrevious);
            this.groupBox1.Location = new System.Drawing.Point(10, 611);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(972, 53);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnPrintGiftReceipt
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnPrintGiftReceipt.Appearance = appearance1;
            this.btnPrintGiftReceipt.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintGiftReceipt.Location = new System.Drawing.Point(221, 18);
            this.btnPrintGiftReceipt.Name = "btnPrintGiftReceipt";
            this.btnPrintGiftReceipt.Size = new System.Drawing.Size(110, 26);
            this.btnPrintGiftReceipt.TabIndex = 10;
            this.btnPrintGiftReceipt.Text = "Print &Gift Receipt";
            this.btnPrintGiftReceipt.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintGiftReceipt.Click += new System.EventHandler(this.btnPrintGiftReceipt_Click);
            // 
            // btnEmailReceipt
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnEmailReceipt.Appearance = appearance2;
            this.btnEmailReceipt.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnEmailReceipt.Location = new System.Drawing.Point(108, 18);
            this.btnEmailReceipt.Name = "btnEmailReceipt";
            this.btnEmailReceipt.Size = new System.Drawing.Size(103, 26);
            this.btnEmailReceipt.TabIndex = 9;
            this.btnEmailReceipt.Text = "&Email Receipt";
            this.btnEmailReceipt.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEmailReceipt.Click += new System.EventHandler(this.btnEmailReceipt_Click);
            // 
            // rptViewer
            // 
            this.rptViewer.ActiveViewIndex = -1;
            this.rptViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rptViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.rptViewer.DisplayBackgroundEdge = false;
            this.rptViewer.EnableDrillDown = false;
            this.rptViewer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rptViewer.Location = new System.Drawing.Point(6, 18);
            this.rptViewer.Name = "rptViewer";
            this.rptViewer.SelectionFormula = "";
            this.rptViewer.ShowCloseButton = false;
            this.rptViewer.ShowGroupTreeButton = false;
            this.rptViewer.ShowTextSearchButton = false;
            this.rptViewer.Size = new System.Drawing.Size(197, 24);
            this.rptViewer.TabIndex = 3;
            this.rptViewer.ViewTimeSelectionFormula = "";
            this.rptViewer.Visible = false;
            // 
            // btnPrintDuplicate
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.btnPrintDuplicate.Appearance = appearance3;
            this.btnPrintDuplicate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintDuplicate.Location = new System.Drawing.Point(341, 18);
            this.btnPrintDuplicate.Name = "btnPrintDuplicate";
            this.btnPrintDuplicate.Size = new System.Drawing.Size(150, 26);
            this.btnPrintDuplicate.TabIndex = 11;
            this.btnPrintDuplicate.Text = "Print &Duplicate Receipt";
            this.btnPrintDuplicate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintDuplicate.Click += new System.EventHandler(this.btnPrintDuplicate_Click);
            // 
            // btnPrint
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance4;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(501, 18);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(50, 26);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCopy
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnCopy.Appearance = appearance5;
            this.btnCopy.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCopy.Location = new System.Drawing.Point(7, 18);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(91, 26);
            this.btnCopy.TabIndex = 8;
            this.btnCopy.TabStop = false;
            this.btnCopy.Text = "C&opy (F2)";
            this.btnCopy.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnNext
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            this.btnNext.Appearance = appearance6;
            this.btnNext.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(731, 18);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(150, 26);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "&Next Inv.(Page Down)";
            this.btnNext.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            this.btnClose.Appearance = appearance7;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(891, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrevious
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnPrevious.Appearance = appearance8;
            this.btnPrevious.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrevious.Location = new System.Drawing.Point(561, 18);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(160, 26);
            this.btnPrevious.TabIndex = 5;
            this.btnPrevious.Text = "&Previous Inv.(Page Up)";
            this.btnPrevious.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.ultraLabel2);
            this.groupBox2.Controls.Add(this.txtTransID);
            this.groupBox2.Controls.Add(this.lblTransactionType);
            this.groupBox2.Controls.Add(this.groupBox7);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Location = new System.Drawing.Point(10, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(972, 600);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.SystemColors.Control;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance9;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance10;
            this.btnSearch.Location = new System.Drawing.Point(225, 72);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 26);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ultraLabel2
            // 
            appearance11.ForeColor = System.Drawing.Color.Navy;
            appearance11.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance11.TextHAlignAsString = "Left";
            appearance11.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance11;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(18, 76);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(80, 19);
            this.ultraLabel2.TabIndex = 9;
            this.ultraLabel2.Text = "Trans#";
            // 
            // txtTransID
            // 
            appearance12.FontData.BoldAsString = "True";
            appearance12.FontData.ItalicAsString = "False";
            appearance12.FontData.StrikeoutAsString = "False";
            appearance12.FontData.UnderlineAsString = "False";
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtTransID.Appearance = appearance12;
            this.txtTransID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransID.Location = new System.Drawing.Point(105, 76);
            this.txtTransID.MaxLength = 20;
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.Size = new System.Drawing.Size(113, 19);
            this.txtTransID.TabIndex = 0;
            this.txtTransID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransID_KeyDown);
            this.txtTransID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTransID_KeyPress);
            // 
            // lblTransactionType
            // 
            appearance13.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance13.BackColor2 = System.Drawing.Color.Azure;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.ForeColor = System.Drawing.Color.Navy;
            appearance13.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance13.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance13;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(2, 10);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(966, 47);
            this.lblTransactionType.TabIndex = 65;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "View POS Transaction";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.txtTransFee);
            this.groupBox7.Controls.Add(this.lblTransactionFee);
            this.groupBox7.Controls.Add(this.txtTransTotalAmount);
            this.groupBox7.Controls.Add(this.ultraLabel9);
            this.groupBox7.Controls.Add(this.txtTransTotalTax);
            this.groupBox7.Controls.Add(this.ultraLabel10);
            this.groupBox7.Controls.Add(this.txtTransTotalDiscount);
            this.groupBox7.Controls.Add(this.ultraLabel8);
            this.groupBox7.Controls.Add(this.txtTransSubtotal);
            this.groupBox7.Controls.Add(this.ultraLabel7);
            this.groupBox7.Enabled = false;
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(543, 453);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(418, 135);
            this.groupBox7.TabIndex = 64;
            this.groupBox7.TabStop = false;
            // 
            // txtTransFee
            // 
            appearance14.BackColor = System.Drawing.Color.LightCyan;
            appearance14.BorderColor = System.Drawing.Color.SteelBlue;
            appearance14.FontData.BoldAsString = "True";
            appearance14.FontData.ItalicAsString = "False";
            appearance14.FontData.StrikeoutAsString = "False";
            appearance14.FontData.UnderlineAsString = "False";
            appearance14.ForeColor = System.Drawing.Color.Black;
            appearance14.ForeColorDisabled = System.Drawing.Color.Black;
            appearance14.TextHAlignAsString = "Right";
            this.txtTransFee.Appearance = appearance14;
            this.txtTransFee.BackColor = System.Drawing.Color.LightCyan;
            this.txtTransFee.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransFee.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransFee.Location = new System.Drawing.Point(190, 84);
            this.txtTransFee.MaxLength = 20;
            this.txtTransFee.Name = "txtTransFee";
            this.txtTransFee.Size = new System.Drawing.Size(192, 22);
            this.txtTransFee.TabIndex = 23;
            this.txtTransFee.Text = "0.00";
            // 
            // lblTransactionFee
            // 
            appearance15.ForeColor = System.Drawing.Color.Navy;
            appearance15.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance15.TextHAlignAsString = "Left";
            appearance15.TextVAlignAsString = "Middle";
            this.lblTransactionFee.Appearance = appearance15;
            this.lblTransactionFee.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionFee.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionFee.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionFee.Location = new System.Drawing.Point(25, 86);
            this.lblTransactionFee.Name = "lblTransactionFee";
            this.lblTransactionFee.Size = new System.Drawing.Size(155, 19);
            this.lblTransactionFee.TabIndex = 24;
            this.lblTransactionFee.Text = "Transaction Fee";
            // 
            // txtTransTotalAmount
            // 
            appearance16.BackColor = System.Drawing.Color.LightCyan;
            appearance16.BorderColor = System.Drawing.Color.SteelBlue;
            appearance16.FontData.BoldAsString = "True";
            appearance16.FontData.ItalicAsString = "False";
            appearance16.FontData.StrikeoutAsString = "False";
            appearance16.FontData.UnderlineAsString = "False";
            appearance16.ForeColor = System.Drawing.Color.Black;
            appearance16.ForeColorDisabled = System.Drawing.Color.Black;
            appearance16.TextHAlignAsString = "Right";
            this.txtTransTotalAmount.Appearance = appearance16;
            this.txtTransTotalAmount.BackColor = System.Drawing.Color.LightCyan;
            this.txtTransTotalAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransTotalAmount.Location = new System.Drawing.Point(190, 107);
            this.txtTransTotalAmount.MaxLength = 20;
            this.txtTransTotalAmount.Name = "txtTransTotalAmount";
            this.txtTransTotalAmount.Size = new System.Drawing.Size(192, 22);
            this.txtTransTotalAmount.TabIndex = 25;
            this.txtTransTotalAmount.Text = "0.00";
            // 
            // ultraLabel9
            // 
            appearance17.ForeColor = System.Drawing.Color.Navy;
            appearance17.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance17.TextHAlignAsString = "Left";
            appearance17.TextVAlignAsString = "Middle";
            this.ultraLabel9.Appearance = appearance17;
            this.ultraLabel9.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel9.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel9.Location = new System.Drawing.Point(25, 109);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(155, 19);
            this.ultraLabel9.TabIndex = 22;
            this.ultraLabel9.Text = "Total Amount";
            // 
            // txtTransTotalTax
            // 
            appearance18.BackColor = System.Drawing.Color.LightCyan;
            appearance18.BorderColor = System.Drawing.Color.SteelBlue;
            appearance18.FontData.BoldAsString = "True";
            appearance18.FontData.ItalicAsString = "False";
            appearance18.FontData.StrikeoutAsString = "False";
            appearance18.FontData.UnderlineAsString = "False";
            appearance18.ForeColor = System.Drawing.Color.Black;
            appearance18.ForeColorDisabled = System.Drawing.Color.Black;
            appearance18.TextHAlignAsString = "Right";
            this.txtTransTotalTax.Appearance = appearance18;
            this.txtTransTotalTax.BackColor = System.Drawing.Color.LightCyan;
            this.txtTransTotalTax.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransTotalTax.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransTotalTax.Location = new System.Drawing.Point(190, 61);
            this.txtTransTotalTax.MaxLength = 20;
            this.txtTransTotalTax.Name = "txtTransTotalTax";
            this.txtTransTotalTax.Size = new System.Drawing.Size(192, 22);
            this.txtTransTotalTax.TabIndex = 21;
            this.txtTransTotalTax.Text = "0.00";
            // 
            // ultraLabel10
            // 
            appearance19.ForeColor = System.Drawing.Color.Navy;
            appearance19.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance19.TextHAlignAsString = "Left";
            appearance19.TextVAlignAsString = "Middle";
            this.ultraLabel10.Appearance = appearance19;
            this.ultraLabel10.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel10.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel10.Location = new System.Drawing.Point(25, 63);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(155, 19);
            this.ultraLabel10.TabIndex = 20;
            this.ultraLabel10.Text = "Total Tax";
            // 
            // txtTransTotalDiscount
            // 
            appearance20.BackColor = System.Drawing.Color.LightCyan;
            appearance20.BorderColor = System.Drawing.Color.SteelBlue;
            appearance20.FontData.BoldAsString = "True";
            appearance20.FontData.ItalicAsString = "False";
            appearance20.FontData.StrikeoutAsString = "False";
            appearance20.FontData.UnderlineAsString = "False";
            appearance20.ForeColor = System.Drawing.Color.Black;
            appearance20.ForeColorDisabled = System.Drawing.Color.Black;
            appearance20.TextHAlignAsString = "Right";
            this.txtTransTotalDiscount.Appearance = appearance20;
            this.txtTransTotalDiscount.BackColor = System.Drawing.Color.LightCyan;
            this.txtTransTotalDiscount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransTotalDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransTotalDiscount.Location = new System.Drawing.Point(190, 38);
            this.txtTransTotalDiscount.MaxLength = 20;
            this.txtTransTotalDiscount.Name = "txtTransTotalDiscount";
            this.txtTransTotalDiscount.Size = new System.Drawing.Size(192, 22);
            this.txtTransTotalDiscount.TabIndex = 19;
            this.txtTransTotalDiscount.Text = "0.00";
            // 
            // ultraLabel8
            // 
            appearance21.ForeColor = System.Drawing.Color.Navy;
            appearance21.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance21.TextHAlignAsString = "Left";
            appearance21.TextVAlignAsString = "Middle";
            this.ultraLabel8.Appearance = appearance21;
            this.ultraLabel8.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel8.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel8.Location = new System.Drawing.Point(25, 40);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(155, 19);
            this.ultraLabel8.TabIndex = 18;
            this.ultraLabel8.Text = "Total Discount";
            // 
            // txtTransSubtotal
            // 
            appearance22.BackColor = System.Drawing.Color.LightCyan;
            appearance22.BorderColor = System.Drawing.Color.SteelBlue;
            appearance22.FontData.BoldAsString = "True";
            appearance22.FontData.ItalicAsString = "False";
            appearance22.FontData.StrikeoutAsString = "False";
            appearance22.FontData.UnderlineAsString = "False";
            appearance22.ForeColor = System.Drawing.Color.Black;
            appearance22.ForeColorDisabled = System.Drawing.Color.Black;
            appearance22.TextHAlignAsString = "Right";
            this.txtTransSubtotal.Appearance = appearance22;
            this.txtTransSubtotal.BackColor = System.Drawing.Color.LightCyan;
            this.txtTransSubtotal.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransSubtotal.Location = new System.Drawing.Point(190, 15);
            this.txtTransSubtotal.MaxLength = 20;
            this.txtTransSubtotal.Name = "txtTransSubtotal";
            this.txtTransSubtotal.Size = new System.Drawing.Size(192, 22);
            this.txtTransSubtotal.TabIndex = 17;
            this.txtTransSubtotal.Text = "0.00";
            // 
            // ultraLabel7
            // 
            appearance23.ForeColor = System.Drawing.Color.Navy;
            appearance23.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance23.TextHAlignAsString = "Left";
            appearance23.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance23;
            this.ultraLabel7.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel7.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(25, 17);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(155, 19);
            this.ultraLabel7.TabIndex = 16;
            this.ultraLabel7.Text = "Sub Total";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.txtTransCustomer);
            this.groupBox4.Controls.Add(this.ultraLabel11);
            this.groupBox4.Controls.Add(this.txtTransStationID);
            this.groupBox4.Controls.Add(this.txtTransDate);
            this.groupBox4.Controls.Add(this.txtTransUserID);
            this.groupBox4.Controls.Add(this.txtTransType);
            this.groupBox4.Controls.Add(this.ultraLabel1);
            this.groupBox4.Controls.Add(this.ultraLabel4);
            this.groupBox4.Controls.Add(this.ultraLabel3);
            this.groupBox4.Controls.Add(this.ultraLabel5);
            this.groupBox4.Enabled = false;
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 60);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(951, 76);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // txtTransCustomer
            // 
            appearance24.FontData.BoldAsString = "True";
            appearance24.FontData.ItalicAsString = "False";
            appearance24.FontData.StrikeoutAsString = "False";
            appearance24.FontData.UnderlineAsString = "False";
            appearance24.ForeColor = System.Drawing.Color.Black;
            appearance24.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtTransCustomer.Appearance = appearance24;
            this.txtTransCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransCustomer.Location = new System.Drawing.Point(93, 46);
            this.txtTransCustomer.MaxLength = 20;
            this.txtTransCustomer.Name = "txtTransCustomer";
            this.txtTransCustomer.Size = new System.Drawing.Size(378, 22);
            this.txtTransCustomer.TabIndex = 17;
            // 
            // ultraLabel11
            // 
            appearance25.ForeColor = System.Drawing.Color.Navy;
            appearance25.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance25.TextHAlignAsString = "Left";
            appearance25.TextVAlignAsString = "Middle";
            this.ultraLabel11.Appearance = appearance25;
            this.ultraLabel11.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel11.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel11.Location = new System.Drawing.Point(6, 46);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(80, 22);
            this.ultraLabel11.TabIndex = 16;
            this.ultraLabel11.Text = "Customer";
            // 
            // txtTransStationID
            // 
            appearance26.FontData.BoldAsString = "True";
            appearance26.FontData.ItalicAsString = "False";
            appearance26.FontData.StrikeoutAsString = "False";
            appearance26.FontData.UnderlineAsString = "False";
            appearance26.ForeColor = System.Drawing.Color.Black;
            appearance26.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtTransStationID.Appearance = appearance26;
            this.txtTransStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransStationID.Location = new System.Drawing.Point(800, 46);
            this.txtTransStationID.MaxLength = 20;
            this.txtTransStationID.Name = "txtTransStationID";
            this.txtTransStationID.Size = new System.Drawing.Size(139, 22);
            this.txtTransStationID.TabIndex = 15;
            // 
            // txtTransDate
            // 
            appearance27.FontData.BoldAsString = "True";
            appearance27.FontData.ItalicAsString = "False";
            appearance27.FontData.StrikeoutAsString = "False";
            appearance27.FontData.UnderlineAsString = "False";
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtTransDate.Appearance = appearance27;
            this.txtTransDate.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransDate.Location = new System.Drawing.Point(569, 16);
            this.txtTransDate.MaxLength = 20;
            this.txtTransDate.Name = "txtTransDate";
            this.txtTransDate.Size = new System.Drawing.Size(370, 22);
            this.txtTransDate.TabIndex = 14;
            // 
            // txtTransUserID
            // 
            appearance28.FontData.BoldAsString = "True";
            appearance28.FontData.ItalicAsString = "False";
            appearance28.FontData.StrikeoutAsString = "False";
            appearance28.FontData.UnderlineAsString = "False";
            appearance28.ForeColor = System.Drawing.Color.Black;
            appearance28.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtTransUserID.Appearance = appearance28;
            this.txtTransUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransUserID.Location = new System.Drawing.Point(569, 46);
            this.txtTransUserID.MaxLength = 20;
            this.txtTransUserID.Name = "txtTransUserID";
            this.txtTransUserID.Size = new System.Drawing.Size(139, 22);
            this.txtTransUserID.TabIndex = 13;
            // 
            // txtTransType
            // 
            appearance29.FontData.BoldAsString = "True";
            appearance29.FontData.ItalicAsString = "False";
            appearance29.FontData.StrikeoutAsString = "False";
            appearance29.FontData.UnderlineAsString = "False";
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtTransType.Appearance = appearance29;
            this.txtTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransType.Location = new System.Drawing.Point(401, 16);
            this.txtTransType.MaxLength = 20;
            this.txtTransType.Name = "txtTransType";
            this.txtTransType.Size = new System.Drawing.Size(70, 22);
            this.txtTransType.TabIndex = 12;
            // 
            // ultraLabel1
            // 
            appearance30.ForeColor = System.Drawing.Color.Navy;
            appearance30.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance30.TextHAlignAsString = "Left";
            appearance30.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance30;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(312, 17);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(80, 19);
            this.ultraLabel1.TabIndex = 10;
            this.ultraLabel1.Text = "Trans. Type";
            // 
            // ultraLabel4
            // 
            appearance31.ForeColor = System.Drawing.Color.Navy;
            appearance31.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance31.TextHAlignAsString = "Left";
            appearance31.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance31;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(480, 17);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(80, 22);
            this.ultraLabel4.TabIndex = 6;
            this.ultraLabel4.Text = "Trans. Date";
            // 
            // ultraLabel3
            // 
            appearance32.ForeColor = System.Drawing.Color.Navy;
            appearance32.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance32.TextHAlignAsString = "Left";
            appearance32.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance32;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(480, 46);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(80, 19);
            this.ultraLabel3.TabIndex = 7;
            this.ultraLabel3.Text = "User ID";
            // 
            // ultraLabel5
            // 
            appearance33.ForeColor = System.Drawing.Color.Navy;
            appearance33.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance33.TextHAlignAsString = "Left";
            appearance33.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance33;
            this.ultraLabel5.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel5.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(716, 46);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(80, 19);
            this.ultraLabel5.TabIndex = 5;
            this.ultraLabel5.Text = "Station ID";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.txtLineItemCnt);
            this.groupBox5.Controls.Add(this.ultraLabel6);
            this.groupBox5.Controls.Add(this.lblHExtPrice);
            this.groupBox5.Controls.Add(this.lblHTaxCd);
            this.groupBox5.Controls.Add(this.lblHDisc);
            this.groupBox5.Controls.Add(this.lblHUPrice);
            this.groupBox5.Controls.Add(this.lblHQty);
            this.groupBox5.Controls.Add(this.lblHItemDescription);
            this.groupBox5.Controls.Add(this.grdDetail);
            this.groupBox5.Location = new System.Drawing.Point(12, 134);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(949, 315);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            // 
            // txtLineItemCnt
            // 
            appearance34.BackColor = System.Drawing.Color.Silver;
            appearance34.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            appearance34.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            appearance34.BorderColor3DBase = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            appearance34.FontData.BoldAsString = "True";
            appearance34.FontData.ItalicAsString = "False";
            appearance34.FontData.StrikeoutAsString = "False";
            appearance34.FontData.UnderlineAsString = "False";
            appearance34.ForeColor = System.Drawing.Color.Maroon;
            appearance34.ForeColorDisabled = System.Drawing.Color.Black;
            appearance34.TextHAlignAsString = "Left";
            appearance34.TextVAlignAsString = "Middle";
            this.txtLineItemCnt.Appearance = appearance34;
            this.txtLineItemCnt.AutoSize = false;
            this.txtLineItemCnt.BackColor = System.Drawing.Color.Silver;
            this.txtLineItemCnt.BorderStyle = Infragistics.Win.UIElementBorderStyle.Rounded4;
            this.txtLineItemCnt.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLineItemCnt.Location = new System.Drawing.Point(11, 286);
            this.txtLineItemCnt.MaxLength = 20;
            this.txtLineItemCnt.Name = "txtLineItemCnt";
            this.txtLineItemCnt.Size = new System.Drawing.Size(928, 23);
            this.txtLineItemCnt.TabIndex = 63;
            this.txtLineItemCnt.TabStop = false;
            this.txtLineItemCnt.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel6
            // 
            appearance35.ForeColor = System.Drawing.Color.Maroon;
            this.ultraLabel6.Appearance = appearance35;
            this.ultraLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(716, 19);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(90, 18);
            this.ultraLabel6.TabIndex = 62;
            this.ultraLabel6.Tag = "NOCOLOR";
            this.ultraLabel6.Text = "Tax Amount";
            // 
            // lblHExtPrice
            // 
            appearance36.ForeColor = System.Drawing.Color.Maroon;
            appearance36.TextHAlignAsString = "Left";
            this.lblHExtPrice.Appearance = appearance36;
            this.lblHExtPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHExtPrice.Location = new System.Drawing.Point(812, 19);
            this.lblHExtPrice.Name = "lblHExtPrice";
            this.lblHExtPrice.Size = new System.Drawing.Size(132, 18);
            this.lblHExtPrice.TabIndex = 61;
            this.lblHExtPrice.Tag = "NOCOLOR";
            this.lblHExtPrice.Text = "NET Price";
            // 
            // lblHTaxCd
            // 
            appearance37.ForeColor = System.Drawing.Color.Maroon;
            this.lblHTaxCd.Appearance = appearance37;
            this.lblHTaxCd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHTaxCd.Location = new System.Drawing.Point(619, 19);
            this.lblHTaxCd.Name = "lblHTaxCd";
            this.lblHTaxCd.Size = new System.Drawing.Size(66, 18);
            this.lblHTaxCd.TabIndex = 60;
            this.lblHTaxCd.Tag = "NOCOLOR";
            this.lblHTaxCd.Text = "Tax Code";
            // 
            // lblHDisc
            // 
            appearance38.ForeColor = System.Drawing.Color.Maroon;
            this.lblHDisc.Appearance = appearance38;
            this.lblHDisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHDisc.Location = new System.Drawing.Point(496, 19);
            this.lblHDisc.Name = "lblHDisc";
            this.lblHDisc.Size = new System.Drawing.Size(108, 18);
            this.lblHDisc.TabIndex = 59;
            this.lblHDisc.Tag = "NOCOLOR";
            this.lblHDisc.Text = "Discount";
            // 
            // lblHUPrice
            // 
            appearance39.ForeColor = System.Drawing.Color.Maroon;
            this.lblHUPrice.Appearance = appearance39;
            this.lblHUPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHUPrice.Location = new System.Drawing.Point(352, 19);
            this.lblHUPrice.Name = "lblHUPrice";
            this.lblHUPrice.Size = new System.Drawing.Size(138, 18);
            this.lblHUPrice.TabIndex = 58;
            this.lblHUPrice.Tag = "NOCOLOR";
            this.lblHUPrice.Text = "Unit Price";
            // 
            // lblHQty
            // 
            appearance40.ForeColor = System.Drawing.Color.Maroon;
            this.lblHQty.Appearance = appearance40;
            this.lblHQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHQty.Location = new System.Drawing.Point(18, 19);
            this.lblHQty.Name = "lblHQty";
            this.lblHQty.Size = new System.Drawing.Size(75, 18);
            this.lblHQty.TabIndex = 57;
            this.lblHQty.Tag = "NOCOLOR";
            this.lblHQty.Text = "Quantity";
            // 
            // lblHItemDescription
            // 
            appearance41.ForeColor = System.Drawing.Color.Maroon;
            this.lblHItemDescription.Appearance = appearance41;
            this.lblHItemDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHItemDescription.Location = new System.Drawing.Point(104, 19);
            this.lblHItemDescription.Name = "lblHItemDescription";
            this.lblHItemDescription.Size = new System.Drawing.Size(242, 18);
            this.lblHItemDescription.TabIndex = 56;
            this.lblHItemDescription.Tag = "NOCOLOR";
            this.lblHItemDescription.Text = "Description";
            // 
            // grdDetail
            // 
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BackColorDisabled = System.Drawing.Color.White;
            appearance42.BackColorDisabled2 = System.Drawing.Color.White;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance42.BorderColor = System.Drawing.Color.MediumBlue;
            this.grdDetail.DisplayLayout.Appearance = appearance42;
            this.grdDetail.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.ColHeadersVisible = false;
            appearance43.TextHAlignAsString = "Right";
            ultraGridColumn19.CellAppearance = appearance43;
            ultraGridColumn19.Header.VisiblePosition = 1;
            ultraGridColumn19.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn19.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn19.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(86, 25);
            ultraGridColumn19.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn19.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn19.Width = 62;
            ultraGridColumn20.Header.Caption = "Description";
            ultraGridColumn20.Header.VisiblePosition = 0;
            ultraGridColumn20.RowLayoutColumnInfo.OriginX = 1;
            ultraGridColumn20.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn20.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(231, 25);
            ultraGridColumn20.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn20.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn20.Width = 188;
            appearance44.FontData.BoldAsString = "True";
            appearance44.ForeColor = System.Drawing.Color.Green;
            appearance44.TextHAlignAsString = "Right";
            ultraGridColumn21.CellAppearance = appearance44;
            ultraGridColumn21.Header.Caption = "Unit Price";
            ultraGridColumn21.Header.VisiblePosition = 2;
            ultraGridColumn21.RowLayoutColumnInfo.OriginX = 3;
            ultraGridColumn21.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn21.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(137, 25);
            ultraGridColumn21.RowLayoutColumnInfo.SpanX = 1;
            ultraGridColumn21.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn21.Width = 86;
            appearance45.TextHAlignAsString = "Right";
            ultraGridColumn22.CellAppearance = appearance45;
            ultraGridColumn22.Header.Caption = "Disc.";
            ultraGridColumn22.Header.VisiblePosition = 3;
            ultraGridColumn22.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn22.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn22.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(110, 25);
            ultraGridColumn22.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn22.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn22.Width = 74;
            ultraGridColumn23.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance46.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance46.BackColor2 = System.Drawing.Color.White;
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            ultraGridColumn23.CellButtonAppearance = appearance46;
            ultraGridColumn23.Header.Caption = "Tax Code";
            ultraGridColumn23.Header.VisiblePosition = 4;
            ultraGridColumn23.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn23.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn23.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(99, 25);
            ultraGridColumn23.RowLayoutColumnInfo.SpanX = 5;
            ultraGridColumn23.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn23.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            ultraGridColumn23.Width = 77;
            ultraGridColumn25.Header.Caption = "Tax Amt.";
            ultraGridColumn25.Header.VisiblePosition = 5;
            ultraGridColumn25.RowLayoutColumnInfo.OriginX = 11;
            ultraGridColumn25.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn25.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn25.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn25.Width = 89;
            appearance47.TextHAlignAsString = "Right";
            ultraGridColumn26.CellAppearance = appearance47;
            ultraGridColumn26.Header.Caption = "Ext. Price";
            ultraGridColumn26.Header.VisiblePosition = 6;
            ultraGridColumn26.RowLayoutColumnInfo.OriginX = 13;
            ultraGridColumn26.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn26.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(113, 25);
            ultraGridColumn26.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn26.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn26.Width = 115;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn19,
            ultraGridColumn20,
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn25,
            ultraGridColumn26});
            appearance48.BackColor = System.Drawing.Color.Navy;
            appearance48.ForeColor = System.Drawing.Color.White;
            ultraGridBand1.Override.SelectedRowAppearance = appearance48;
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            ultraGridBand1.SummaryFooterCaption = "";
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Rounded4;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance49.BackColor = System.Drawing.Color.White;
            appearance49.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance49;
            appearance50.BackColor = System.Drawing.Color.White;
            appearance50.BackColor2 = System.Drawing.Color.White;
            appearance50.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.White;
            appearance51.BackColor2 = System.Drawing.Color.White;
            appearance51.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance51;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance52.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance52;
            appearance53.BackColor = System.Drawing.Color.White;
            appearance53.BackColor2 = System.Drawing.Color.White;
            appearance53.BackColorDisabled = System.Drawing.Color.White;
            appearance53.BackColorDisabled2 = System.Drawing.Color.White;
            appearance53.BorderColor = System.Drawing.Color.Black;
            appearance53.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance53;
            appearance54.BackColor = System.Drawing.Color.White;
            appearance54.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance54.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance54.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance54.BorderColor = System.Drawing.Color.Gray;
            appearance54.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance54.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance54.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance54;
            appearance55.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.DataErrorRowAppearance = appearance55;
            appearance56.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.DataErrorRowSelectorAppearance = appearance56;
            appearance57.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance57.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance57;
            appearance58.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance58;
            appearance59.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance59;
            appearance60.BackColor = System.Drawing.Color.White;
            appearance60.BackColor2 = System.Drawing.Color.White;
            appearance60.BackColorDisabled = System.Drawing.Color.White;
            appearance60.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance60;
            appearance61.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance61.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance61;
            appearance62.BackColor = System.Drawing.Color.White;
            appearance62.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance62.FontData.BoldAsString = "True";
            appearance62.ForeColor = System.Drawing.Color.Black;
            appearance62.TextHAlignAsString = "Left";
            appearance62.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance62;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            appearance63.BackColor = System.Drawing.Color.NavajoWhite;
            appearance63.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance63;
            appearance64.BackColor = System.Drawing.Color.White;
            appearance64.BackColor2 = System.Drawing.Color.White;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance64.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance64.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance64;
            appearance65.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance65;
            appearance66.BackColor = System.Drawing.Color.White;
            appearance66.BackColor2 = System.Drawing.SystemColors.Control;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance66.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance66;
            this.grdDetail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance67.BackColor = System.Drawing.Color.Navy;
            appearance67.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance67;
            appearance68.BackColor = System.Drawing.Color.Navy;
            appearance68.BackColorDisabled = System.Drawing.Color.Navy;
            appearance68.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance68.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance68.BorderColor = System.Drawing.Color.Gray;
            appearance68.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance68;
            this.grdDetail.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            this.grdDetail.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance69.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance69;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance70.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance70.BackColor2 = System.Drawing.Color.White;
            appearance70.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance70.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance70.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance70;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.grdDetail.Location = new System.Drawing.Point(11, 49);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(928, 236);
            this.grdDetail.TabIndex = 55;
            this.grdDetail.TabStop = false;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.lvPayType);
            this.groupBox6.Location = new System.Drawing.Point(12, 453);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(516, 135);
            this.groupBox6.TabIndex = 63;
            this.groupBox6.TabStop = false;
            // 
            // lvPayType
            // 
            this.lvPayType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvPayType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvPayType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPayType.FullRowSelect = true;
            this.lvPayType.GridLines = true;
            this.lvPayType.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvPayType.HideSelection = false;
            this.lvPayType.Location = new System.Drawing.Point(3, 15);
            this.lvPayType.MultiSelect = false;
            this.lvPayType.Name = "lvPayType";
            this.lvPayType.Size = new System.Drawing.Size(507, 115);
            this.lvPayType.TabIndex = 47;
            this.lvPayType.TabStop = false;
            this.lvPayType.UseCompatibleStateImageBehavior = false;
            this.lvPayType.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Payment Type";
            this.columnHeader1.Width = 207;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Amount";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 116;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Reference";
            this.columnHeader3.Width = 162;
            // 
            // rvReportViewer
            // 
            this.rvReportViewer.ActiveViewIndex = -1;
            this.rvReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rvReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.rvReportViewer.DisplayBackgroundEdge = false;
            this.rvReportViewer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rvReportViewer.Location = new System.Drawing.Point(307, 197);
            this.rvReportViewer.Name = "rvReportViewer";
            this.rvReportViewer.SelectionFormula = "";
            this.rvReportViewer.ShowGroupTreeButton = false;
            this.rvReportViewer.ShowTextSearchButton = false;
            this.rvReportViewer.Size = new System.Drawing.Size(378, 273);
            this.rvReportViewer.TabIndex = 6;
            this.rvReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.rvReportViewer.ViewTimeSelectionFormula = "";
            this.rvReportViewer.Visible = false;
            // 
            // btnCloseReport
            // 
            appearance71.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance71.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance71.FontData.BoldAsString = "True";
            appearance71.ForeColor = System.Drawing.Color.Black;
            this.btnCloseReport.Appearance = appearance71;
            this.btnCloseReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCloseReport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseReport.Location = new System.Drawing.Point(445, 324);
            this.btnCloseReport.Name = "btnCloseReport";
            this.btnCloseReport.Size = new System.Drawing.Size(103, 26);
            this.btnCloseReport.TabIndex = 7;
            this.btnCloseReport.Text = "&Close";
            this.btnCloseReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCloseReport.Visible = false;
            this.btnCloseReport.Click += new System.EventHandler(this.btnCloseReport_Click);
            // 
            // frmViewTransactionDetail
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(993, 674);
            this.Controls.Add(this.btnCloseReport);
            this.Controls.Add(this.rvReportViewer);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewTransactionDetail";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "View POS Transaction";
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            this.Shown += new System.EventHandler(this.frmViewTransactionDetail_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmViewTransactionDetail_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransID)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransTotalTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransTotalDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransSubtotal)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransType)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLineItemCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmReportViewer_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.Left = (frmMain.getInstance().Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;            
        }

        string strCQuery = "";
        Int32 TempInvoiceNo = 0;
        private void PrintPreview()
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //rptViewTransaction oRptViewTrans= new rptViewTransaction();

                Search oSearch = new Search();
                strCQuery = strQuery + strWhere;
                DataSet ds = oSearch.SearchData(strCQuery);
                //oRptViewTrans.Database.Tables[0].SetDataSource(ds.Tables[0]);
                //Following Commented part UnCommented by Krishna on 17 May 2011
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        CurrentInvoiceNo = Convert.ToInt32(ds.Tables[0].Rows[0]["TransID"]);
                        //Added By Shitaljit for ROA transaction.
                        if (POS_Core.Resources.Configuration.convertNullToString(ds.Tables[0].Rows[0]["TransType"].ToString().Trim()).Equals("ROA"))
                        {
                            misROATrans = true;
                        }
                        else
                        {
                            misROATrans = false;
                        }
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        clsUIHelper.ShowErrorMsg("Invalid TransID, Please enter a valid TransID."); //Sprint-27 - PRIMEPOS-2456 10-Oct-2017 JY Added
                        this.txtTransID.Text = Configuration.convertNullToString(CurrentInvoiceNo); //Sprint-27 - PRIMEPOS-2456 10-Oct-2017 JY Added
                        return;
                    }
                }
                //Comments removed till here by krishna

                DataSet subDs = oSearch.SearchData(strSubQuery + " and PT.TransID=" + CurrentInvoiceNo);
                #region Populate Values
                Customer oCustomer = new Customer();
                CustomerData oCustdata = new CustomerData();
                CustomerRow oCustRow = null;

                oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(ds.Tables[0].Rows[0]["CustomerID"].ToString().Trim()));
                if (oCustdata.Tables[0].Rows.Count > 0)
                {
                    oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                    this.txtTransCustomer.Text = oCustRow.CustomerFullName;
                }

                this.txtTransID.Text = Configuration.convertNullToString(CurrentInvoiceNo);


                this.txtTransUserID.Text = Configuration.convertNullToString(ds.Tables[0].Rows[0]["UserID"].ToString().Trim());
                this.txtTransType.Text = Configuration.convertNullToString(ds.Tables[0].Rows[0]["TransType"].ToString().Trim());
                this.txtTransStationID.Text = Configuration.GetStationName(Configuration.convertNullToString(ds.Tables[0].Rows[0]["StationID"].ToString().Trim()));
                this.txtTransDate.Text = Configuration.convertNullToString(ds.Tables[0].Rows[0]["TransDate"].ToString().Trim());
                int RowIndex = 0;
                DataSet oDataSet = new DataSet();
                oDataSet.Tables.Add("TransDetail");
                oDataSet.Tables[0].Columns.Add("Qty", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("ItemDescription", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("Price", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("Discount", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("TaxCode", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("TaxAmount", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("ExtendedPrice", Type.GetType("System.String"));

                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    DataRow drTranDetRow = oDataSet.Tables[0].NewRow();
                    drTranDetRow[0] = Configuration.convertNullToString(oRow["Qty"]);
                    drTranDetRow[1] = Configuration.convertNullToString(oRow["Description"]);
                    drTranDetRow[2] = Configuration.convertNullToDecimal(oRow["Price"]).ToString("########0.00");
                    drTranDetRow[3] = Configuration.convertNullToDecimal(oRow["Discount"]).ToString("########0.00");
                    drTranDetRow[4] = Configuration.convertNullToString(oRow["TaxID"]);
                    drTranDetRow[5] = Configuration.convertNullToDecimal(oRow["TaxAmount"]).ToString("########0.00");
                    drTranDetRow[6] = Configuration.convertNullToDecimal(oRow["ExtendedPrice"]).ToString("########0.00");
                    oDataSet.Tables[0].Rows.Add(drTranDetRow);
                    RowIndex++;

                }
                FillListView(subDs);
                this.txtTransSubtotal.Text = string.IsNullOrEmpty(Configuration.convertNullToString(ds.Tables[0].Rows[0]["GrossTotal"].ToString().Trim())) == true ? "0.00" : Configuration.convertNullToDecimal(ds.Tables[0].Rows[0]["GrossTotal"]).ToString("########0.00").Trim();
                this.txtTransTotalDiscount.Text = string.IsNullOrEmpty(Configuration.convertNullToString(ds.Tables[0].Rows[0]["TotalDiscAmount"].ToString().Trim())) == true ? "0.00" : Configuration.convertNullToDecimal(ds.Tables[0].Rows[0]["TotalDiscAmount"]).ToString("########0.00").Trim();
                this.txtTransTotalTax.Text = string.IsNullOrEmpty(Configuration.convertNullToString(ds.Tables[0].Rows[0]["TotalTaxAmount"].ToString().Trim())) == true ? "0.00" : Configuration.convertNullToDecimal(ds.Tables[0].Rows[0]["TotalTaxAmount"]).ToString("########0.00").Trim();
                this.txtTransFee.Text = string.IsNullOrEmpty(Configuration.convertNullToString(ds.Tables[0].Rows[0]["TotalTransFeeAmt"].ToString().Trim())) == true ? "0.00" : Configuration.convertNullToDecimal(ds.Tables[0].Rows[0]["TotalTransFeeAmt"]).ToString("########0.00").Trim();    //PRIMEPOS-3117 11-Jul-2022 JY Added
                this.txtTransTotalAmount.Text = string.IsNullOrEmpty(Configuration.convertNullToString(ds.Tables[0].Rows[0]["TotalPaid"].ToString().Trim())) == true ? "0.00" : Configuration.convertNullToDecimal(ds.Tables[0].Rows[0]["TotalPaid"]).ToString("########0.00").Trim();
                if (this.txtTransTotalDiscount.Text != "0.00")
                {
                    this.txtTransTotalDiscount.Appearance.BackColor = Color.Red;
                }

                grdDetail.DataSource = oDataSet;
                ApplyGriFormate();

                txtLineItemCnt.Text = "Line Items = " + grdDetail.Rows.Count.ToString();    //Sprint-24 - PRIMEPOS-2327 19-Oct-2016 JY Added

                TempInvoiceNo = CurrentInvoiceNo + 1;
                TransHeaderSvr oTHSvr = new TransHeaderSvr();
                MaxInvoiceNo = oTHSvr.GetMaxTransId();

                if (CurrentInvoiceNo != 0 && TempInvoiceNo != MaxInvoiceNo)
                {
                    this.btnNext.Enabled = true;
                }
                else
                {
                    this.btnNext.Enabled = false;
                }
                #endregion

                #region Commented Code


                ////clsUIHelper.ShowErrorMsg(subDs.Tables[0].Rows.Count.ToString());
                ////clsUIHelper.ShowErrorMsg(subDs.Tables[0].Rows[1]["TransAmt"].ToString());
                ////Added By Shitaljit(QuicSolv) on 22 July 2011
                //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptViewTrans.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptViewTrans.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptViewTrans.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptViewTrans.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                ////End of added By Shitaljit.
                //oRptViewTrans.OpenSubreport("rptViewTransPayment").Database.Tables[0].SetDataSource(subDs.Tables[0]);
                //this.rptViewer.ReportSource=oRptViewTrans;
                //this.rptViewer.Show();
                //oRptViewTrans.Refresh();
                #endregion
                strWhere = "";
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void FillListView(DataSet dsPaytype)
        {
            ListViewItem oItem;
            try
            {
                lvPayType.Items.Clear();
                string[] arr = { "", "", "" };

                for (int i = 0; i < dsPaytype.Tables[0].Rows.Count; i++)
                {
                    //PRIMEPOS-2860 12-Jun-2020 JY Added logic to concat "Credit Card" for American Express, Visa, Master Card and Disover
                    //if ("3456".Contains(Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PayTypeID"]).Trim()) && Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PAYMENTPROCESSOR"]).Trim() != "")
                    if ((Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PayTypeID"]).Trim() == "3" || Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PayTypeID"]).Trim() == "4" ||
                        Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PayTypeID"]).Trim() == "5" || Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PayTypeID"]).Trim() == "6")
                        && Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PAYMENTPROCESSOR"]).Trim() != "")
                    {
                        arr[0] = Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["PAYMENTPROCESSOR"]).Trim() + " - " + Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["DESCRIPTION"]).Trim();
                    }
                    else
                        arr[0] = Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["DESCRIPTION"]).Trim();

                    arr[1] = Configuration.convertNullToDecimal(dsPaytype.Tables[0].Rows[i]["TransAmt"]).ToString("########0.00").Trim();
                    arr[2] = Configuration.convertNullToString(dsPaytype.Tables[0].Rows[i]["RefNo"]).Trim();
                    oItem = new ListViewItem(arr);
                    lvPayType.Items.Add(oItem);
                    lvPayType.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg("Eorror while populating pay types.");
            }
        }

        private void btnPrevious_Click(object sender, System.EventArgs e)
        {
            if (CurrentInvoiceNo != 0)
            {
                strWhere = " and PT.TransID= (select top 1 TransID from POSTransaction where TransID<" + CurrentInvoiceNo.ToString() + " order by TransID desc)";
                this.PrintPreview();
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            if (CurrentInvoiceNo != 0)
            {
                strWhere = " and PT.TransID= (select top 1 TransID from POSTransaction where TransID>" + CurrentInvoiceNo.ToString() + " order by TransID)";
                this.PrintPreview();
            }
        }

        private void frmViewTransactionDetail_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                //Added this because earlier it was taking the key up event even if it's constructor is called so added this to remove that 
                if (isFirstTime)//PRIMEPOS-2738
                {
                    if (e.KeyData == System.Windows.Forms.Keys.Enter && this.txtTransID.ContainsFocus == false) //PRIMEPOS-2773 30-Dec-2019 JY Added logic to restrict next control when focus is in "TransId"
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                    else if (e.KeyData == Keys.Escape && Configuration.CSetting.StrictReturn != true)//PRIMEPOS-2738 ADDED NEW PARAMETER FOR STRICT RETURN
                        this.Close();
                    else if (e.KeyData == Keys.F2 && this.isForCopy)
                    {
                        misCopied = true;
                        this.Close();
                    }
                    //Added By shitaljit to allow user to browse previous , next trans by using Page Up down keys JIRA-903
                    else if (e.KeyData == Keys.PageDown)
                    {
                        btnNext_Click(null, null);
                    }
                    //PRIMEPOS-2738
                    else if (e.KeyData == Keys.F5)
                    {
                        isClosed = true;
                        this.Close();
                    }
                    //
                    else if (e.KeyData == Keys.PageUp)
                    {
                        btnPrevious_Click(null, null);
                    }
                    else if (e.KeyData == Keys.F4)  //Sprint-27 - PRIMEPOS-2456 10-Oct-2017 JY Added
                    {
                        btnSearch_Click(null, null);
                    }
                }
                else
                {
                    isFirstTime = true;
                    //do nothing
                }
                isFirstTime = true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }


        public bool isCopied
        {
            get
            {
                return misCopied;
            }
        }

        //Added By Shitaljit for ROA transaction.
        public bool isROATrans
        {
            get
            {
                return misROATrans;
            }
        }

        public Int32 TransID
        {
            get
            {
                return CurrentInvoiceNo;
            }
        }

        private void btnCopy_Click(object sender, System.EventArgs e)
        {
            misCopied = true;
            this.Close();
        }

        private void rptViewer_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }
        /// <summary>
        /// Author: Shitaljit on 22April2013
        /// TO apply grid formate.
        /// </summary>
        private void ApplyGriFormate()
        {
            this.grdDetail.DisplayLayout.Bands[0].Columns["ItemDescription"].Format = "########0.00";
            this.grdDetail.DisplayLayout.Bands[0].Columns["Price"].Format = "########0.00";
            this.grdDetail.DisplayLayout.Bands[0].Columns["Discount"].Format = "########0.00";
            this.grdDetail.DisplayLayout.Bands[0].Columns["TaxCode"].Format = "########0.00";
            this.grdDetail.DisplayLayout.Bands[0].Columns["TaxAmount"].Format = "########0.00";
            this.grdDetail.DisplayLayout.Bands[0].Columns["ExtendedPrice"].Format = "########0.00";
        }
        //Naim 29Jul2009
        //Made changes to show multiple credit card payment receipts
        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.TransID < 1)
                {
                    return;
                }

                TransHeaderData oTransHData;
                TransHeaderSvr oTransHSvr = new TransHeaderSvr();

                TransDetailData oTransDData;
                TransDetailSvr oTransDSvr = new TransDetailSvr();

                POSTransPaymentData oTransPaymentData;
                POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();

                //added by atul 07-jan-2011
                TransDetailRXData oTransRxData;
                TransDetailRXSvr oTransRxSvr = new TransDetailRXSvr();
                //End of added by atul 07-jan-2011

                oTransHData = oTransHSvr.Populate(TransID);

                oTransDData = oTransDSvr.PopulateData(TransID);

                oTransPaymentData = oTransPaymentSvr.Populate(TransID);

                TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(TransID); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

                //added by atul 07-jan-2011
                RxLabel oRxLabel;
                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    //if (oTransDData.TransDetail[0].ItemID.ToString().Trim().ToUpper() == "RX")    //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented as its checking only first item
                    oTransRxData = oTransRxSvr.PopulateData(TransID);   //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added 
                    if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                    {
                        //oTransRxData = oTransRxSvr.PopulateData(TransID); //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented 
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                    } //end of added by atul 07-jan-2011
                    else
                    {
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                    }
                }
                else
                {
                    oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                }

                oRptPrintTrancsaction = new rptPrintTransaction();
                Search oSearch = new Search();
                DataSet ds = oSearch.SearchData(strCQuery);


                ds.Tables[0].Columns.Add("CustomerSignature", typeof(System.Byte[]));
                int index = 0;
                DataTable oChargeAccTab = new DataTable();
                oChargeAccTab.Columns.Add("TransID", System.Type.GetType("System.Int32"));
                oChargeAccTab.Columns.Add("AcctSignature", typeof(System.Byte[]));

                oTransPaymentData.Tables[0].Columns.Add("CustomerSignature", typeof(System.Byte[]));
                oTransPaymentData.Tables[0].Columns.Add("UserID", typeof(System.String));
                oTransPaymentData.Tables[0].Columns.Add("TransType", typeof(System.String));
                oTransPaymentData.Tables[0].Columns.Add("RestrictSignatureLineAndWordingOnReceipt", typeof(System.String));    //PRIMEPOS-2910 29-Oct-2020 JY Added

                //**************
                Bitmap bit = null;
                System.IO.MemoryStream oStream = null;
                //*************

                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    try
                    {
                        if (oTransPaymentData.Tables[0].Rows[0]["BinarySign"] != System.DBNull.Value) //Added by prashant(SRT) 28-sep-2010
                        {
                            //MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                            //bit = new Bitmap(ms);
                            #region PRIMEPOS-2900 15-Sep-2020 JY Added if part for Vantiv
                            try
                            {
                                if (Configuration.convertNullToString(oTransPaymentData.Tables[0].Rows[0][clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]).Trim().ToUpper() == "VANTIV" || Configuration.convertNullToString(oTransPaymentData.Tables[0].Rows[0][clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]).Trim().ToUpper() == "NB_VANTIV") //PRIMEPOS-3482
                                {
                                    Byte[] strBinSign = (byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"];
                                    bit = POS_Core.Resources.DelegateHandler.clsCoreUIHelper.ConvertPoints(strBinSign);
                                }
                                else
                                {
                                    MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                    bit = new Bitmap(ms);
                                }
                            }
                            catch { }
                            #endregion
                        }
                        else
                        {
                            bit = clsUIHelper.GetSignature(oTransPaymentData.Tables[0].Rows[0]["CustomerSign"].ToString(), POS_Core.Resources.Configuration.CInfo.SigType);
                        }
                        oStream = new System.IO.MemoryStream();
                        if (bit != null)
                        {
                            bit.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            oRow["CustomerSignature"] = oStream.ToArray();
                        }
                        oRow["Description"] = oRxLabel.TransD(index).ItemDescription;
                        index++;
                    }
                    catch { }
                }

                foreach (DataRow oRow in oTransPaymentData.POSTransPayment.Rows)
                {
                    oRow["TransType"] = ds.Tables[0].Rows[0]["TransType"].ToString();
                    oRow["UserID"] = ds.Tables[0].Rows[0]["UserID"].ToString();
                    oRow["RestrictSignatureLineAndWordingOnReceipt"] = (Configuration.convertNullToBoolean(Configuration.CSetting.RestrictSignatureLineAndWordingOnReceipt) == false ? "0":"1");  //PRIMEPOS-2910 29-Oct-2020 JY Added

                    switch (oRow[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString().Trim())
                    {
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "H":
                            if (oRow["BinarySign"] != System.DBNull.Value) //Added by prashant(SRT) 28-sep-2010
                            {
                                //MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                //bit = new Bitmap(ms);

                                #region PRIMEPOS-2900 15-Sep-2020 JY Added if part for Vantiv
                                try
                                {
                                    if (Configuration.convertNullToString(oRow[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]).Trim().ToUpper() == "VANTIV")
                                    {
                                        Byte[] strBinSign = (byte[])oRow["BinarySign"];
                                        bit = POS_Core.Resources.DelegateHandler.clsCoreUIHelper.ConvertPoints(strBinSign);
                                    }
                                    else
                                    {
                                        MemoryStream ms = new MemoryStream((byte[])oRow["BinarySign"]);
                                        bit = new Bitmap(ms);
                                    }
                                }
                                catch { }
                                #endregion
                            }
                            else
                            {
                                bit = clsUIHelper.GetSignature(oRow[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].ToString(), POS_Core.Resources.Configuration.CInfo.SigType);
                            }

                            oStream = new System.IO.MemoryStream();
                            if (bit != null)
                            {
                                bit.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                oRow["CustomerSignature"] = oStream.ToArray();
                            }
                            if (oRow[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString().Trim() == "H")
                            {
                                DataRow oCRow = oChargeAccTab.NewRow();
                                oCRow["TransID"] = oRow[clsPOSDBConstants.POSTransPayment_Fld_TransID].ToString();
                                //oCRow["AcctSignature"] = oCStream.ToArray();
                                if (oStream != null)
                                    oCRow["AcctSignature"] = oStream.ToArray();
                                oChargeAccTab.Rows.Add(oCRow);

                            }
                            break;
                    }
                }

                oRptPrintTrancsaction.ReportOptions.EnableSaveDataWithReport = false;
                oRptPrintTrancsaction.Database.Tables[0].SetDataSource(ds.Tables[0]);
                oRptPrintTrancsaction.Database.Tables[1].SetDataSource(oChargeAccTab);
                oRptPrintTrancsaction.Subreports["rptPaymentDetail"].Database.Tables[0].SetDataSource(oTransPaymentData.Tables[0]);
                oRptPrintTrancsaction.Subreports["rptViewTransPayment"].Database.Tables[0].SetDataSource(oTransPaymentData.Tables[0]);
                // get printe Settings 
                //System.Windows.Forms.PrintDialog pd = new PrintDialog();
                //pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                //pd.AllowSomePages = true;
                //pd.AllowSelection = false;
                //pd.ShowNetwork = true;
                //oRptPrintTrancsaction.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;
                
                if (oRxLabel.AccCode.Trim() == "")
                {
                    oRptPrintTrancsaction.ReportFooterSection3.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    oRptPrintTrancsaction.ReportFooterSection3.SectionFormat.EnableSuppress = false;
                }

                PopulateChargeAcct(oRxLabel);
                ClearSubReportPearameters(oRxLabel);
                //clsReports.ShowReport(oRptPrintTrancsaction);
                //return;

                //if (oRxLabel.MergeCCWithRecpt == true) // Commente by Ravindra for PRIMEPOS-16471 sub-taskReicept does not print when Merge CC is turned on and non-laser receipt is used
                {
                    if (oRxLabel.CCPaymentRow == null || oRxLabel.CCPaymentRow.RefNo.ToString() == "")
                    {

                        //oRptPrintTrancsaction.ReportFooterSection2.SectionFormat.EnableSuppress = true;
                        ClearSubReportPearameters(oRxLabel);
                    }
                    else
                    {
                        // for (int i = 0; i <= Configuration.CPOSSet.RP_CCPrint; i++)
                        {
                            PrintCC(oRxLabel); //add compies of pos credit card transcation report
                            // print copies of the current rx
                            //oRptPrintTrancsaction.PrintToPrinter(1, false, 1, 2);

                            //check the no of  credit card copies to be printed with current rx 
                            //if (POS_Core.Resources.Configuration.CPOSSet.RP_CCPrint > 1)
                            //{
                            //    oRptPrintTrancsaction.PrintToPrinter(POS_Core.Resources.Configuration.CPOSSet.RP_CCPrint - 1, false, 2, 2);
                            //}
                        }
                    }
                }
                // from here Commente by Ravindra for PRIMEPOS-16471 sub-taskReicept does not print when Merge CC is turned on and non-laser receipt is used
                //else
                //{

                //   ClearSubReportPearameters(oRxLabel);
                //   //oRptPrintTrancsaction.PrintToPrinter(1, false, 1, 1);           

                //}
                //till here  from here Commente by Ravindra for PRIMEPOS-16471 sub-taskReicept does not print when Merge CC is turned on and non-laser receipt is used
                /// check to see the reportformat
                //frmTestRport rptview = new frmTestRport();
                //rptview.ReportViewer.ReportSource = oRptPrintTrancsaction;
                //rptview.Show();
                oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotalQuantity.ParameterFieldName, oRxLabel.TotalQty.ToString());
                ShowReport(oRptPrintTrancsaction);
                //oRxLabel.Print();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ShowReport(ReportClass pReport)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rvReportViewer.ReportSource = pReport;
                this.WindowState = FormWindowState.Maximized;
                this.rvReportViewer.DisplayGroupTree = false;
                this.rvReportViewer.Dock = DockStyle.Fill;
                this.groupBox1.Visible = false;
                this.groupBox2.Visible = false;
                this.rvReportViewer.Visible = true;
                this.btnCloseReport.Visible = true;
                this.btnCloseReport.Location = new Point(422, 2);
                this.Show();
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void ClearSubReportPearameters(RxLabel oRxlabel)
        {

            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Pharmacyname_.ParameterFieldName, POS_Core.Resources.Configuration.CInfo.StoreName);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_PhramecyAddress.ParameterFieldName, oRxlabel.CAddress);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_txtPharmacyCityStateZip.ParameterFieldName, Configuration.convertNullToString(oRxlabel.CCity) + ", " + Configuration.convertNullToString(oRxlabel.CState) + " " + Configuration.convertNullToString(oRxlabel.CZip));  //PRIMEPOS-2560 16-Jul-2018 JY Added
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TelephoneNo.ParameterFieldName, oRxlabel.CTelephone);

            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_MasterCard.ParameterFieldName, "");
            //Added By SRT(Ritesh Parekh) Date : 25-Jul-2009
            //oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_rptViewTransPayment_PayTypeDesc.ParameterFieldName, "Card Type :");
            //End Of Added By SRT(Ritesh Parekh)
            oRptPrintTrancsaction.SetParameterValue("MasterCard", "");
            oRptPrintTrancsaction.SetParameterValue("Charge", "0");
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Merchant.ParameterFieldName, "0");
            oRptPrintTrancsaction.SetParameterValue("Merchant", "0");
            oRptPrintTrancsaction.SetParameterValue("Authority", "0");
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotlaAmount.ParameterFieldName, "0");
            oRptPrintTrancsaction.ReportFooterSection2.SectionFormat.EnableSuppress = true;

        }

        private void PopulateChargeAcct(RxLabel oRxlabel)
        {
            oRptPrintTrancsaction.SetParameterValue("sAccCode", Configuration.convertNullToString(oRxlabel.AccCode));   //PRIMEPOS-2777 10-Jan-2020 JY Modified
            oRptPrintTrancsaction.SetParameterValue("sAccName", Configuration.convertNullToString(oRxlabel.AccName));   //PRIMEPOS-2777 10-Jan-2020 JY Modified
            oRptPrintTrancsaction.SetParameterValue("AcctAmount", Configuration.convertNullToDecimal(oRxlabel.AccAmount));  //PRIMEPOS-2777 10-Jan-2020 JY Modified

            oRptPrintTrancsaction.SetParameterValue("CurrBalance", Configuration.convertNullToDecimal(oRxlabel.AccCurrBalance));    //PRIMEPOS-2777 10-Jan-2020 JY Modified

            if (oRxlabel.TransactionTypeCode == 3)
            {
                oRptPrintTrancsaction.SetParameterValue("sChargeAccType", "Received On Account :");
                
                string hcReference = Configuration.convertNullToString(oRxlabel.HCReference);
                if (hcReference.Length > 25)
                {
                    oRptPrintTrancsaction.SetParameterValue("HCReference", hcReference.Substring(0, 25) + "..");
                    if (hcReference.Length > 25)
                    {
                        oRptPrintTrancsaction.SetParameterValue("HCReference2", hcReference.Substring(25));
                    }
                    else
                    {
                        oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                    }
                }
                else
                {
                    oRptPrintTrancsaction.SetParameterValue("HCReference", hcReference);
                    oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                }
            }
            else
            {
                oRptPrintTrancsaction.SetParameterValue("sChargeAccType", "Amount Charged :");
                oRptPrintTrancsaction.SetParameterValue("HCReference","");
                oRptPrintTrancsaction.SetParameterValue("HCReference2","");
            }
        }

        private void PrintCC(RxLabel oRxlabel)
        {
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Pharmacyname_.ParameterFieldName, POS_Core.Resources.Configuration.CInfo.StoreName);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_PhramecyAddress.ParameterFieldName, oRxlabel.CAddress);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_txtPharmacyCityStateZip.ParameterFieldName, Configuration.convertNullToString(oRxlabel.CCity) + ", " + Configuration.convertNullToString(oRxlabel.CState) + " " + Configuration.convertNullToString(oRxlabel.CZip));  //PRIMEPOS-2560 16-Jul-2018 JY Added
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TelephoneNo.ParameterFieldName, oRxlabel.CTelephone);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_MasterCard.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.RefNo.ToString()));
            //Added By SRT(Ritesh Parekh) Date : 25-Jul-2009
            //oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_rptViewTransPayment_PayTypeDesc.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "Card:" : oRxlabel.CCPaymentRow.TransTypeDesc.ToString()+":"));
            //End OF Added By SRT(Ritesh Parekh)
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Charge.ParameterFieldName, Convert.ToString(oRxlabel.TransH(0).TenderedAmount - oRxlabel.TransH(0).TotalPaid));
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Merchant.ParameterFieldName, (oRxlabel.CMerchantNo == null ? "" : oRxlabel.CMerchantNo));
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Authority.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.AuthNo.ToString()));
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotlaAmount.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.Amount.ToString()));

            oRptPrintTrancsaction.ReportFooterSection2.SectionFormat.EnableSuppress = false;
        }

        private void btnPrintDuplicate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.TransID < 1)
                {
                    return;
                }

                TransHeaderData oTransHData;
                TransHeaderSvr oTransHSvr = new TransHeaderSvr();

                TransDetailData oTransDData;
                TransDetailSvr oTransDSvr = new TransDetailSvr();

                POSTransPaymentData oTransPaymentData;
                POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();

                //added by atul 07-jan-2011
                TransDetailRXData oTransRxData;
                TransDetailRXSvr oTransRxSvr = new TransDetailRXSvr();
                //End of added by atul 07-jan-2011

                oTransHData = oTransHSvr.Populate(TransID);

                oTransDData = oTransDSvr.PopulateData(TransID);

                oTransPaymentData = oTransPaymentSvr.Populate(TransID);

                TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(TransID); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

                bool bReceiptsPrinted = false;  //PRIMEPOS-2647 12-Mar-2019 JY Added
                //added by atul 07-jan-2011
                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    //if (oTransDData.TransDetail[0].ItemID.ToString().Trim().ToUpper() == "RX")    //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented as its checking only first item
                    oTransRxData = oTransRxSvr.PopulateData(TransID);   //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added
                    if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                    {
                        //oTransRxData = oTransRxSvr.PopulateData(TransID); //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);
                        //Commented By Shitaljit on 2/10/2013 for JIRA PRIMEPOS-1772 Do not open cash drawer while printing duplicate receipt.
                        //oRxLabel.OpenDrawer();
                        //SetParameters(oRxLabel, oTransPaymentData);   //PRIMEPOS-2939 03-Mar-2021 JY Commented
                        bReceiptsPrinted = oRxLabel.Print();
                    }
                    else
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);
                        //Commented By Shitaljit on 2/10/2013 for JIRA PRIMEPOS-1772 Do not open cash drawer while printing duplicate receipt.
                        //oRxLabel.OpenDrawer();
                        //SetParameters(oRxLabel, oTransPaymentData);   //PRIMEPOS-2939 03-Mar-2021 JY Commented
                        oRxLabel.dtTax = oTransDetailTaxSvr.GetTaxCodeDetail(TransID);//2644
                        string tmpPaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                        if (oRxLabel.CCPaymentRow != null && tmpPaymentProcessor == "EVERTEC") //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.AuthNo = oRxLabel.CCPaymentRow.AuthNo;
                            oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ProcessorTransID;
                            oRxLabel.Trace = oRxLabel.CCPaymentRow.TraceNumber;
                            oRxLabel.Batch = oRxLabel.CCPaymentRow.BatchNumber;
                            oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID.Trim();
                            oRxLabel.InvoiceNumber = oRxLabel.CCPaymentRow.InvoiceNumber;
                            #region PRIMEPOS-2786 EVERTEC EBTBALANCE
                            if (oRxLabel.CCPaymentRow.EbtBalance.Length > 3)
                            {
                                oRxLabel.FoodBalance = oRxLabel.CCPaymentRow.EbtBalance.Split('|')[0];
                                oRxLabel.CashBalance = oRxLabel.CCPaymentRow.EbtBalance.Split('|')[1];
                            }
                            #endregion
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                            oRxLabel.ControlNumber = oRxLabel.CCPaymentRow.ControlNumber;
                            oRxLabel.IsEvertecForceTransaction = oRxLabel.CCPaymentRow.IsEvertecForceTransaction;//PRIMEPOS-2857
                            oRxLabel.IsEvertecSign = oRxLabel.CCPaymentRow.IsEvertecSign;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                oRxLabel.EvertecCityTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            }
                            oRxLabel.EvertecCashback = oRxLabel.CCPaymentRow.CashBack;//PRIMEPOS-2857
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.ATHMovil))
                                oRxLabel.ATHMovil = oRxLabel.CCPaymentRow.ATHMovil.Substring(2, oRxLabel.CCPaymentRow.ATHMovil.Length - 2);//2664
                        }
                        if (oRxLabel.ATHPaymentRow != null && Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                        {
                            oRxLabel.AuthNo = oRxLabel.ATHPaymentRow.AuthNo;
                            oRxLabel.ReferenceNumber = oRxLabel.ATHPaymentRow.ProcessorTransID;
                            oRxLabel.Trace = oRxLabel.ATHPaymentRow.TraceNumber;
                            oRxLabel.Batch = oRxLabel.ATHPaymentRow.BatchNumber;
                            oRxLabel.MerchantID = oRxLabel.ATHPaymentRow.MerchantID.Trim();
                            oRxLabel.InvoiceNumber = oRxLabel.ATHPaymentRow.InvoiceNumber;
                            #region PRIMEPOS-2664 EVERTEC EBTBALANCE
                            if (oRxLabel.ATHPaymentRow.EbtBalance.Length > 3)
                            {
                                oRxLabel.FoodBalance = oRxLabel.ATHPaymentRow.EbtBalance.Split('|')[0];
                                oRxLabel.CashBalance = oRxLabel.ATHPaymentRow.EbtBalance.Split('|')[1];
                            }
                            #endregion
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;
                            oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                            oRxLabel.ControlNumber = oRxLabel.ATHPaymentRow.ControlNumber;
                            oRxLabel.IsEvertecForceTransaction = oRxLabel.ATHPaymentRow.IsEvertecForceTransaction;//PRIMEPOS-2857
                            oRxLabel.IsEvertecSign = oRxLabel.ATHPaymentRow.IsEvertecSign;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                oRxLabel.EvertecCityTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            }
                            oRxLabel.EvertecCashback = oRxLabel.ATHPaymentRow.CashBack;//PRIMEPOS-2857
                            if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.ATHMovil))
                                oRxLabel.ATHMovil = oRxLabel.ATHPaymentRow.ATHMovil.Substring(2, oRxLabel.ATHPaymentRow.ATHMovil.Length - 2);//2664
                        }
                        //if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")  //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        if (tmpPaymentProcessor == "EVERTEC")  //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            if (oRxLabel.CashPaymentRow != null)
                            {
                                oRxLabel.AuthNo = oRxLabel.CashPaymentRow.AuthNo;
                                oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ProcessorTransID;
                                oRxLabel.Trace = oRxLabel.CashPaymentRow.TraceNumber;
                                oRxLabel.Batch = oRxLabel.CashPaymentRow.BatchNumber;
                                oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
                                oRxLabel.InvoiceNumber = oRxLabel.CashPaymentRow.InvoiceNumber;
                                oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                                oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                                oRxLabel.IsEvertecForceTransaction = oRxLabel.CashPaymentRow.IsEvertecForceTransaction; //primepos-2831
                                oRxLabel.IsEvertecSign = oRxLabel.CashPaymentRow.IsEvertecSign; //primepos-2831
                                oRxLabel.ControlNumber = oRxLabel.CashPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.EvertecCashback = oRxLabel.CashPaymentRow.CashBack;//PRIMEPOS-2857
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.ATHMovil))
                                    oRxLabel.ATHMovil = oRxLabel.CashPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.CheckPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.CheckPaymentRow.ControlNumber;
                                oRxLabel.ATHMovil = oRxLabel.CheckPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.CouponPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.CouponPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CouponPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.ATHMovil = oRxLabel.CheckPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.HCPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.HCPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.HCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.ATHMovil = oRxLabel.HCPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.CBPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.CBPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CBPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.ATHMovil = oRxLabel.CBPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.EBTPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.EBTPaymentRow.ControlNumber;
                                oRxLabel.IsEvertecForceTransaction = oRxLabel.EBTPaymentRow.IsEvertecForceTransaction;//PRIMEPOS-2857
                                oRxLabel.IsEvertecSign = oRxLabel.EBTPaymentRow.IsEvertecSign;
                                #region PRIMEPOS-2664 EVERTEC EBTBALANCE
                                if (oRxLabel.EBTPaymentRow.EbtBalance.Length > 3)
                                {
                                    oRxLabel.FoodBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[0];
                                    oRxLabel.CashBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[1];
                                }
                                #endregion
                                if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.EvertecCashback = oRxLabel.EBTPaymentRow.CashBack;//PRIMEPOS-2857
                            }
                        }
                        //if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")    //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        if (oRxLabel.CCPaymentRow != null && tmpPaymentProcessor == "VANTIV")  //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ReferenceNumber;
                            oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID;
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = oRxLabel.CCPaymentRow.TerminalID;
                            oRxLabel.TransactionID = oRxLabel.CCPaymentRow.TransactionID;
                            oRxLabel.ResponseCode = oRxLabel.CCPaymentRow.ResponseCode;
                            oRxLabel.Aid = oRxLabel.CCPaymentRow.Aid;
                            oRxLabel.Cryptogram = oRxLabel.CCPaymentRow.Cryptogram;
                            oRxLabel.EntryMethod = oRxLabel.CCPaymentRow.EntryMethod;
                            oRxLabel.ApprovalCode = oRxLabel.CCPaymentRow?.ApprovalCode;
                            oRxLabel.ApplicationLabel = oRxLabel.CCPaymentRow?.ApplicaionLabel;
                            oRxLabel.PinVerified = oRxLabel.CCPaymentRow.PinVerified;
                            oRxLabel.LaneID = oRxLabel.CCPaymentRow.LaneID;
                            oRxLabel.CardLogo = oRxLabel.CCPaymentRow.CardLogo;
                        }
                        //else if (oRxLabel.CashPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        else if (oRxLabel.CashPaymentRow != null && tmpPaymentProcessor == "VANTIV")   //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ReferenceNumber;
                            oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor; //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = oRxLabel.CashPaymentRow.TerminalID;
                            oRxLabel.TransactionID = oRxLabel.CashPaymentRow.TransactionID;
                            oRxLabel.ResponseCode = oRxLabel.CashPaymentRow.ResponseCode;
                            oRxLabel.Aid = oRxLabel.CashPaymentRow.Aid;
                            oRxLabel.Cryptogram = oRxLabel.CashPaymentRow.Cryptogram;
                            oRxLabel.EntryMethod = oRxLabel.CashPaymentRow.EntryMethod.ToUpper();
                            oRxLabel.ApprovalCode = oRxLabel.CashPaymentRow?.ApprovalCode;
                            oRxLabel.ApplicationLabel = oRxLabel.CashPaymentRow?.ApplicaionLabel;
                            oRxLabel.PinVerified = oRxLabel.CashPaymentRow.PinVerified;
                            oRxLabel.LaneID = oRxLabel.CashPaymentRow.LaneID;
                            oRxLabel.CardLogo = oRxLabel.CashPaymentRow.CardLogo;
                        }
                        //else if (oRxLabel.EBTPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")  //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        else if (oRxLabel.EBTPaymentRow != null && tmpPaymentProcessor == "VANTIV")    //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.ReferenceNumber = oRxLabel.EBTPaymentRow.ReferenceNumber;
                            oRxLabel.MerchantID = oRxLabel.EBTPaymentRow.MerchantID;
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = oRxLabel.EBTPaymentRow.TerminalID;
                            oRxLabel.TransactionID = oRxLabel.EBTPaymentRow.TransactionID;
                            oRxLabel.ResponseCode = oRxLabel.EBTPaymentRow.ResponseCode;
                            oRxLabel.Aid = oRxLabel.EBTPaymentRow.Aid;
                            oRxLabel.Cryptogram = oRxLabel.EBTPaymentRow.Cryptogram;
                            oRxLabel.EntryMethod = oRxLabel.EBTPaymentRow.EntryMethod;
                            oRxLabel.ApprovalCode = oRxLabel.EBTPaymentRow?.ApprovalCode;
                            oRxLabel.ApplicationLabel = oRxLabel.EBTPaymentRow?.ApplicaionLabel;
                            oRxLabel.PinVerified = oRxLabel.EBTPaymentRow.PinVerified;
                            oRxLabel.LaneID = oRxLabel.EBTPaymentRow.LaneID;
                            oRxLabel.CardLogo = oRxLabel.EBTPaymentRow.CardLogo;
                        }
                        bReceiptsPrinted = oRxLabel.Print();
                    }
                }
                else //End of added by atul 07-jan-2011
                {
                    RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);
                    //Commented By Shitaljit on 2/10/2013 for JIRA PRIMEPOS-1772 Do not open cash drawer while printing duplicate receipt.
                    //oRxLabel.OpenDrawer();
                    //SetParameters(oRxLabel, oTransPaymentData);   //PRIMEPOS-2939 03-Mar-2021 JY Commented
                    bReceiptsPrinted = oRxLabel.Print();
                }

                #region PRIMEPOS-2647 12-Mar-2019 JY Added
                //no need to bring up message, instead print receipt when settings is 0
                //if (!bReceiptsPrinted)
                //    Resources.Message.Display("We couldn't print a receipt, please set respective receipt printing related settings", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                #endregion
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region PRIMEPOS-2939 03-Mar-2021 JY Commented
        //private void SetParameters(RxLabel oRxLabel, POSTransPaymentData oTransPaymentData)
        //{
        //    try
        //    {               
        //        //Added by Arvind for the DUPLICATE Evertec receipt printing PRIMEPOS-2664
        //        string tmpPaymentProcessor = oRxLabel.GetPaymentProcessor(oTransPaymentData);    //PRIMEPOS-2876 27-Jul-2020 JY Added
        //                                                                                         //if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor == "EVERTEC") //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //        if (oRxLabel.CCPaymentRow != null && tmpPaymentProcessor == "EVERTEC") //PRIMEPOS-2876 27-Jul-2020 JY Added
        //        {
        //            oRxLabel.AuthNo = oRxLabel.CCPaymentRow.AuthNo;
        //            oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ProcessorTransID;
        //            oRxLabel.Trace = oRxLabel.CCPaymentRow.TraceNumber;
        //            oRxLabel.Batch = oRxLabel.CCPaymentRow.BatchNumber;
        //            oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID.Trim();
        //            oRxLabel.InvoiceNumber = oRxLabel.CCPaymentRow.InvoiceNumber;
        //            #region PRIMEPOS-2786 EVERTEC EBTBALANCE
        //            if (oRxLabel.CCPaymentRow.EbtBalance.Length > 3)
        //            {
        //                oRxLabel.FoodBalance = oRxLabel.CCPaymentRow.EbtBalance.Split('|')[0];
        //                oRxLabel.CashBalance = oRxLabel.CCPaymentRow.EbtBalance.Split('|')[1];
        //            }
        //            #endregion
        //            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
        //            oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
        //            oRxLabel.ControlNumber = oRxLabel.CCPaymentRow.ControlNumber;
        //        }
        //        //if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")  //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //        if (tmpPaymentProcessor == "EVERTEC")  //PRIMEPOS-2876 27-Jul-2020 JY Added
        //        {
        //            if (oRxLabel.CashPaymentRow != null)
        //            {
        //                oRxLabel.ControlNumber = oRxLabel.CashPaymentRow.ControlNumber;
        //            }
        //            else if (oRxLabel.CheckPaymentRow != null)
        //            {
        //                oRxLabel.ControlNumber = oRxLabel.CheckPaymentRow.ControlNumber;
        //            }
        //            else if (oRxLabel.CouponPaymentRow != null)
        //            {
        //                oRxLabel.ControlNumber = oRxLabel.CouponPaymentRow.ControlNumber;
        //            }
        //            else if (oRxLabel.HCPaymentRow != null)
        //            {
        //                oRxLabel.ControlNumber = oRxLabel.HCPaymentRow.ControlNumber;
        //            }
        //            else if (oRxLabel.CBPaymentRow != null)
        //            {
        //                oRxLabel.ControlNumber = oRxLabel.CBPaymentRow.ControlNumber;
        //            }
        //            else if (oRxLabel.EBTPaymentRow != null)
        //            {
        //                oRxLabel.ControlNumber = oRxLabel.EBTPaymentRow.ControlNumber;
        //                #region PRIMEPOS-2786 EVERTEC EBTBALANCE
        //                if (oRxLabel.EBTPaymentRow.EbtBalance.Length > 3)
        //                {
        //                    oRxLabel.FoodBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[0];
        //                    oRxLabel.CashBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[1];
        //                }
        //                #endregion
        //            }
        //        }
        //        //Added by Arvind PRIMEPOS-2636
        //        //if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")    //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //        if (oRxLabel.CCPaymentRow != null && tmpPaymentProcessor == "VANTIV")  //PRIMEPOS-2876 27-Jul-2020 JY Added
        //        {
        //            oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ReferenceNumber;
        //            oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID;
        //            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
        //            oRxLabel.TerminalID = oRxLabel.CCPaymentRow.TerminalID;
        //            oRxLabel.TransactionID = oRxLabel.CCPaymentRow.TransactionID;
        //            oRxLabel.ResponseCode = oRxLabel.CCPaymentRow.ResponseCode;
        //            oRxLabel.Aid = oRxLabel.CCPaymentRow.Aid;
        //            oRxLabel.Cryptogram = oRxLabel.CCPaymentRow.Cryptogram;
        //            oRxLabel.EntryMethod = oRxLabel.CCPaymentRow.EntryMethod;
        //            oRxLabel.ApprovalCode = oRxLabel.CCPaymentRow?.ApprovalCode;
        //            #region PRIMEPOS-2793
        //            oRxLabel.ApplicationLabel = oRxLabel.CCPaymentRow?.ApplicaionLabel;
        //            oRxLabel.PinVerified = oRxLabel.CCPaymentRow.PinVerified;
        //            oRxLabel.LaneID = oRxLabel.CCPaymentRow.LaneID;
        //            oRxLabel.CardLogo = oRxLabel.CCPaymentRow.CardLogo;
        //            #endregion
        //        }
        //        //else if (oRxLabel.CashPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //        else if (oRxLabel.CashPaymentRow != null && tmpPaymentProcessor == "VANTIV")   //PRIMEPOS-2876 27-Jul-2020 JY Added
        //        {
        //            oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ReferenceNumber;
        //            oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
        //            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
        //            oRxLabel.TerminalID = oRxLabel.CashPaymentRow.TerminalID;
        //            oRxLabel.TransactionID = oRxLabel.CashPaymentRow.TransactionID;
        //            oRxLabel.ResponseCode = oRxLabel.CashPaymentRow.ResponseCode;
        //            oRxLabel.Aid = oRxLabel.CashPaymentRow.Aid;
        //            oRxLabel.Cryptogram = oRxLabel.CashPaymentRow.Cryptogram;
        //            oRxLabel.EntryMethod = oRxLabel.CashPaymentRow.EntryMethod.ToUpper();
        //            oRxLabel.ApprovalCode = oRxLabel.CashPaymentRow?.ApprovalCode;
        //            #region PRIMEPOS-2793
        //            oRxLabel.ApplicationLabel = oRxLabel.CashPaymentRow?.ApplicaionLabel;
        //            oRxLabel.PinVerified = oRxLabel.CashPaymentRow.PinVerified;
        //            oRxLabel.LaneID = oRxLabel.CashPaymentRow.LaneID;
        //            oRxLabel.CardLogo = oRxLabel.CashPaymentRow.CardLogo;
        //            #endregion
        //        }
        //        //else if (oRxLabel.EBTPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")  //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //        else if (oRxLabel.EBTPaymentRow != null && tmpPaymentProcessor == "VANTIV")    //PRIMEPOS-2876 27-Jul-2020 JY Added
        //        {
        //            oRxLabel.ReferenceNumber = oRxLabel.EBTPaymentRow.ReferenceNumber;
        //            oRxLabel.MerchantID = oRxLabel.EBTPaymentRow.MerchantID;
        //            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
        //            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
        //            oRxLabel.TerminalID = oRxLabel.EBTPaymentRow.TerminalID;
        //            oRxLabel.TransactionID = oRxLabel.EBTPaymentRow.TransactionID;
        //            oRxLabel.ResponseCode = oRxLabel.EBTPaymentRow.ResponseCode;
        //            oRxLabel.Aid = oRxLabel.EBTPaymentRow.Aid;
        //            oRxLabel.Cryptogram = oRxLabel.EBTPaymentRow.Cryptogram;
        //            oRxLabel.EntryMethod = oRxLabel.EBTPaymentRow.EntryMethod;
        //            oRxLabel.ApprovalCode = oRxLabel.EBTPaymentRow?.ApprovalCode;
        //            #region PRIMEPOS-2793
        //            oRxLabel.ApplicationLabel = oRxLabel.EBTPaymentRow?.ApplicaionLabel;
        //            oRxLabel.PinVerified = oRxLabel.EBTPaymentRow.PinVerified;
        //            oRxLabel.LaneID = oRxLabel.EBTPaymentRow.LaneID;
        //            oRxLabel.CardLogo = oRxLabel.EBTPaymentRow.CardLogo;
        //            #endregion
        //        }

        //        if (oTransPaymentData != null && oTransPaymentData.Tables.Count > 0 && oTransPaymentData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (POSTransPaymentRow oRow in oTransPaymentData.POSTransPayment.Rows)
        //            {
        //                if (Configuration.convertNullToString(oRow.PaymentProcessor).Trim() != "" && oRow.PaymentProcessor.Trim() != "N/A")
        //                {
        //                    oRxLabel.TicketNumber = Configuration.convertNullToString(oRow.TicketNumber).Trim();
        //                    if (oRxLabel.ReferenceNumber.Trim() == "") oRxLabel.ReferenceNumber = Configuration.convertNullToString(oRow.ReferenceNumber).Trim();
        //                    if (oRxLabel.TransactionID.Trim() == "") oRxLabel.TransactionID = Configuration.convertNullToString(oRow.TransactionID).Trim();
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        logger.Fatal(Ex, "SetParameters(RxLabel oRxLabel, POSTransPaymentData oTransPaymentData)");
        //    }
        //}
        #endregion

        private void btnCloseReport_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.groupBox1.Visible = true;
            this.groupBox2.Visible = true;
            this.rvReportViewer.Visible = false;
            this.Show();
        }

        private void btnEmailReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                //Int32 TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());
                frmSendEmail email = new frmSendEmail(TransID);
                email.bPrintDuplicateReceipt = true;  //PRIMEPOS-2900 15-Sep-2020 JY Added
                email.ShowDialog();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);

                //throw;
            }
        }

        #region Sprint-27 - PRIMEPOS-2456 09-Oct-2017 JY Added
        private void btnSearch_Click(object sender, EventArgs e)
        {
            isForCopy = true;
            if (txtTransID.Text.Trim() != "" && txtTransID.Text.StartsWith("?")) //PRIMEPOS-2773 30-Dec-2019 JY Added
            {
                txtTransID.Text = txtTransID.Text.Trim().Substring(1, txtTransID.Text.Trim().Length - 1);                
                Application.DoEvents();
            }
            if (txtTransID.Text.Trim() != "" && clsCoreUIHelper.isNumeric(txtTransID.Text) == false)    //PRIMEPOS-3113 26-Jul-2022 JY Added
            {
                clsUIHelper.ShowErrorMsg("Trans# should be numeric.");
                return;
            }
            GenerateSQL(txtTransID.Text.ToString(), "", "");
            this.btnCopy.Visible = true;
            this.PrintPreview();
        }
        #endregion

        private void frmViewTransactionDetail_Shown(object sender, EventArgs e)
        {
            if (txtTransID.Enabled) txtTransID.Focus(); //Sprint-27 - PRIMEPOS-2456 09-Oct-2017 JY Added to set focus
        }

        #region PRIMEPOS-2773 30-Dec-2019 JY Added
        private void txtTransID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    //if (txtTransID.Text.Trim() != "") //PRIMEPOS-3113 26-Jul-2022 JY Commented
                    btnSearch_Click(btnSearch, new EventArgs());

                    if (txtTransID.Enabled)
                    {
                        txtTransID.SelectionStart = 0;
                        txtTransID.SelectionLength = txtTransID.Text.Length;
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion

        #region PRIMEPOS-2677 29-Jun-2020 JY Added
        private void btnPrintGiftReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.TransID < 1)
                {
                    return;
                }

                TransHeaderData oTransHData;
                TransHeaderSvr oTransHSvr = new TransHeaderSvr();
                TransDetailData oTransDData;
                TransDetailSvr oTransDSvr = new TransDetailSvr();
                POSTransPaymentData oTransPaymentData;
                POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();

                TransDetailRXData oTransRxData;
                TransDetailRXSvr oTransRxSvr = new TransDetailRXSvr();

                oTransHData = oTransHSvr.Populate(TransID);
                oTransDData = oTransDSvr.PopulateData(TransID);
                oTransPaymentData = oTransPaymentSvr.Populate(TransID);

                TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr();
                DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(TransID);

                bool bReceiptsPrinted = false;
                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    oTransRxData = oTransRxSvr.PopulateData(TransID);
                    if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);
                        oRxLabel.bPrintGiftReciept = true;
                        int NoOfGiftReceipt = 0;
                        if (Configuration.CPOSSet.NoOfGiftReceipt == 0) NoOfGiftReceipt = 1;
                        bReceiptsPrinted = oRxLabel.PrintGiftCoupon(NoOfGiftReceipt);
                    }
                    else
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);
                        string tmpPaymentProcessor = oRxLabel.GetPaymentProcessor(oTransPaymentData);    //PRIMEPOS-2876 27-Jul-2020 JY Added
                        //if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor == "EVERTEC") //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        if (oRxLabel.CCPaymentRow != null && tmpPaymentProcessor == "EVERTEC") //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.AuthNo = oRxLabel.CCPaymentRow.AuthNo;
                            oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ProcessorTransID;
                            oRxLabel.Trace = oRxLabel.CCPaymentRow.TraceNumber;
                            oRxLabel.Batch = oRxLabel.CCPaymentRow.BatchNumber;
                            oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID.Trim();
                            oRxLabel.InvoiceNumber = oRxLabel.CCPaymentRow.InvoiceNumber;
                            #region PRIMEPOS-2786 EVERTEC EBTBALANCE
                            if (oRxLabel.CCPaymentRow.EbtBalance.Length > 3)
                            {
                                oRxLabel.FoodBalance = oRxLabel.CCPaymentRow.EbtBalance.Split('|')[0];
                                oRxLabel.CashBalance = oRxLabel.CCPaymentRow.EbtBalance.Split('|')[1];
                            }
                            #endregion
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                            oRxLabel.ControlNumber = oRxLabel.CCPaymentRow.ControlNumber;
                            oRxLabel.IsEvertecForceTransaction = oRxLabel.CCPaymentRow.IsEvertecForceTransaction;//PRIMEPOS-2857
                            oRxLabel.IsEvertecSign = oRxLabel.CCPaymentRow.IsEvertecSign;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                oRxLabel.EvertecCityTax = oRxLabel.CCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            }
                            oRxLabel.EvertecCashback = oRxLabel.CCPaymentRow.CashBack;//PRIMEPOS-2857
                            if (!string.IsNullOrWhiteSpace(oRxLabel.CCPaymentRow.ATHMovil))
                                oRxLabel.ATHMovil = oRxLabel.CCPaymentRow.ATHMovil.Substring(2, oRxLabel.CCPaymentRow.ATHMovil.Length - 2);//2664
                        }
                        if (oRxLabel.ATHPaymentRow != null && Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                        {
                            oRxLabel.AuthNo = oRxLabel.ATHPaymentRow.AuthNo;
                            oRxLabel.ReferenceNumber = oRxLabel.ATHPaymentRow.ProcessorTransID;
                            oRxLabel.Trace = oRxLabel.ATHPaymentRow.TraceNumber;
                            oRxLabel.Batch = oRxLabel.ATHPaymentRow.BatchNumber;
                            oRxLabel.MerchantID = oRxLabel.ATHPaymentRow.MerchantID.Trim();
                            oRxLabel.InvoiceNumber = oRxLabel.ATHPaymentRow.InvoiceNumber;
                            #region PRIMEPOS-2664 EVERTEC EBTBALANCE
                            if (oRxLabel.ATHPaymentRow.EbtBalance.Length > 3)
                            {
                                oRxLabel.FoodBalance = oRxLabel.ATHPaymentRow.EbtBalance.Split('|')[0];
                                oRxLabel.CashBalance = oRxLabel.ATHPaymentRow.EbtBalance.Split('|')[1];
                            }
                            #endregion
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;
                            oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                            oRxLabel.ControlNumber = oRxLabel.ATHPaymentRow.ControlNumber;
                            oRxLabel.IsEvertecForceTransaction = oRxLabel.ATHPaymentRow.IsEvertecForceTransaction;//PRIMEPOS-2857
                            oRxLabel.IsEvertecSign = oRxLabel.ATHPaymentRow.IsEvertecSign;
                            if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                            {
                                oRxLabel.EvertecStateTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                oRxLabel.EvertecCityTax = oRxLabel.ATHPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            }
                            oRxLabel.EvertecCashback = oRxLabel.ATHPaymentRow.CashBack;//PRIMEPOS-2857
                            if (!string.IsNullOrWhiteSpace(oRxLabel.ATHPaymentRow.ATHMovil))
                                oRxLabel.ATHMovil = oRxLabel.ATHPaymentRow.ATHMovil.Substring(2, oRxLabel.ATHPaymentRow.ATHMovil.Length - 2);//2664
                        }
                        //if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")  //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        if (tmpPaymentProcessor == "EVERTEC")  //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            if (oRxLabel.CashPaymentRow != null)
                            {
                                oRxLabel.AuthNo = oRxLabel.CashPaymentRow.AuthNo;
                                oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ProcessorTransID;
                                oRxLabel.Trace = oRxLabel.CashPaymentRow.TraceNumber;
                                oRxLabel.Batch = oRxLabel.CashPaymentRow.BatchNumber;
                                oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
                                oRxLabel.InvoiceNumber = oRxLabel.CashPaymentRow.InvoiceNumber;
                                oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                                oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                                oRxLabel.IsEvertecForceTransaction = oRxLabel.CashPaymentRow.IsEvertecForceTransaction; //primepos-2831
                                oRxLabel.IsEvertecSign = oRxLabel.CashPaymentRow.IsEvertecSign; //primepos-2831
                                oRxLabel.ControlNumber = oRxLabel.CashPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.CashPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.EvertecCashback = oRxLabel.CashPaymentRow.CashBack;//PRIMEPOS-2857
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CashPaymentRow.ATHMovil))
                                    oRxLabel.ATHMovil = oRxLabel.CashPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.CheckPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.CheckPaymentRow.ControlNumber;
                                oRxLabel.ATHMovil = oRxLabel.CheckPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.CouponPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.CouponPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CouponPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.CouponPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.ATHMovil = oRxLabel.CheckPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.HCPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.HCPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.HCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.HCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.ATHMovil = oRxLabel.HCPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.CBPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.CBPaymentRow.ControlNumber;
                                if (!string.IsNullOrWhiteSpace(oRxLabel.CBPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.CBPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.ATHMovil = oRxLabel.CBPaymentRow.ATHMovil;//2664
                            }
                            else if (oRxLabel.EBTPaymentRow != null)
                            {
                                oRxLabel.ControlNumber = oRxLabel.EBTPaymentRow.ControlNumber;
                                oRxLabel.IsEvertecForceTransaction = oRxLabel.EBTPaymentRow.IsEvertecForceTransaction;//PRIMEPOS-2857
                                oRxLabel.IsEvertecSign = oRxLabel.EBTPaymentRow.IsEvertecSign;
                                #region PRIMEPOS-2664 EVERTEC EBTBALANCE
                                if (oRxLabel.EBTPaymentRow.EbtBalance.Length > 3)
                                {
                                    oRxLabel.FoodBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[0];
                                    oRxLabel.CashBalance = oRxLabel.EBTPaymentRow.EbtBalance.Split('|')[1];
                                }
                                #endregion
                                if (!string.IsNullOrWhiteSpace(oRxLabel.EBTPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                                {
                                    oRxLabel.EvertecStateTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                                    oRxLabel.EvertecCityTax = oRxLabel.EBTPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                                }
                                oRxLabel.EvertecCashback = oRxLabel.EBTPaymentRow.CashBack;//PRIMEPOS-2857
                            }
                        }
                        //if (oRxLabel.CCPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")    //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        if (oRxLabel.CCPaymentRow != null && tmpPaymentProcessor == "VANTIV")  //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.ReferenceNumber = oRxLabel.CCPaymentRow.ReferenceNumber;
                            oRxLabel.MerchantID = oRxLabel.CCPaymentRow.MerchantID;
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = oRxLabel.CCPaymentRow.TerminalID;
                            oRxLabel.TransactionID = oRxLabel.CCPaymentRow.TransactionID;
                            oRxLabel.ResponseCode = oRxLabel.CCPaymentRow.ResponseCode;
                            oRxLabel.Aid = oRxLabel.CCPaymentRow.Aid;
                            oRxLabel.Cryptogram = oRxLabel.CCPaymentRow.Cryptogram;
                            oRxLabel.EntryMethod = oRxLabel.CCPaymentRow.EntryMethod;
                            oRxLabel.ApprovalCode = oRxLabel.CCPaymentRow?.ApprovalCode;
                            oRxLabel.ApplicationLabel = oRxLabel.CCPaymentRow?.ApplicaionLabel;
                            oRxLabel.PinVerified = oRxLabel.CCPaymentRow.PinVerified;
                            oRxLabel.LaneID = oRxLabel.CCPaymentRow.LaneID;
                            oRxLabel.CardLogo = oRxLabel.CCPaymentRow.CardLogo;
                        }
                        //else if (oRxLabel.CashPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        else if (oRxLabel.CashPaymentRow != null && tmpPaymentProcessor == "VANTIV")   //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.ReferenceNumber = oRxLabel.CashPaymentRow.ReferenceNumber;
                            oRxLabel.MerchantID = oRxLabel.CashPaymentRow.MerchantID;
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor; //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = oRxLabel.CashPaymentRow.TerminalID;
                            oRxLabel.TransactionID = oRxLabel.CashPaymentRow.TransactionID;
                            oRxLabel.ResponseCode = oRxLabel.CashPaymentRow.ResponseCode;
                            oRxLabel.Aid = oRxLabel.CashPaymentRow.Aid;
                            oRxLabel.Cryptogram = oRxLabel.CashPaymentRow.Cryptogram;
                            oRxLabel.EntryMethod = oRxLabel.CashPaymentRow.EntryMethod.ToUpper();
                            oRxLabel.ApprovalCode = oRxLabel.CashPaymentRow?.ApprovalCode;
                            oRxLabel.ApplicationLabel = oRxLabel.CashPaymentRow?.ApplicaionLabel;
                            oRxLabel.PinVerified = oRxLabel.CashPaymentRow.PinVerified;
                            oRxLabel.LaneID = oRxLabel.CashPaymentRow.LaneID;
                            oRxLabel.CardLogo = oRxLabel.CashPaymentRow.CardLogo;
                        }
                        //else if (oRxLabel.EBTPaymentRow != null && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")  //PRIMEPOS-2876 27-Jul-2020 JY Commented
                        else if (oRxLabel.EBTPaymentRow != null && tmpPaymentProcessor == "VANTIV")    //PRIMEPOS-2876 27-Jul-2020 JY Added
                        {
                            oRxLabel.ReferenceNumber = oRxLabel.EBTPaymentRow.ReferenceNumber;
                            oRxLabel.MerchantID = oRxLabel.EBTPaymentRow.MerchantID;
                            //oRxLabel.PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Commented
                            oRxLabel.PaymentProcessor = tmpPaymentProcessor;   //PRIMEPOS-2876 27-Jul-2020 JY Added
                            oRxLabel.TerminalID = oRxLabel.EBTPaymentRow.TerminalID;
                            oRxLabel.TransactionID = oRxLabel.EBTPaymentRow.TransactionID;
                            oRxLabel.ResponseCode = oRxLabel.EBTPaymentRow.ResponseCode;
                            oRxLabel.Aid = oRxLabel.EBTPaymentRow.Aid;
                            oRxLabel.Cryptogram = oRxLabel.EBTPaymentRow.Cryptogram;
                            oRxLabel.EntryMethod = oRxLabel.EBTPaymentRow.EntryMethod;
                            oRxLabel.ApprovalCode = oRxLabel.EBTPaymentRow?.ApprovalCode;
                            oRxLabel.ApplicationLabel = oRxLabel.EBTPaymentRow?.ApplicaionLabel;
                            oRxLabel.PinVerified = oRxLabel.EBTPaymentRow.PinVerified;
                            oRxLabel.LaneID = oRxLabel.EBTPaymentRow.LaneID;
                            oRxLabel.CardLogo = oRxLabel.EBTPaymentRow.CardLogo;
                        }
                        oRxLabel.bPrintGiftReciept = true;
                        int NoOfGiftReceipt = 0;
                        if (Configuration.CPOSSet.NoOfGiftReceipt == 0) NoOfGiftReceipt = 1;
                        bReceiptsPrinted = oRxLabel.PrintGiftCoupon(NoOfGiftReceipt);
                    }
                }
                else
                {
                    RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);
                    oRxLabel.bPrintGiftReciept = true;
                    int NoOfGiftReceipt = 0;
                    if (Configuration.CPOSSet.NoOfGiftReceipt == 0) NoOfGiftReceipt = 1;
                    bReceiptsPrinted = oRxLabel.PrintGiftCoupon(NoOfGiftReceipt);                   
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-3113 10-Aug-2022 JY Added
        private void txtTransID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '?'))
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}