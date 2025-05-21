using Infragistics.Win.UltraWinGrid;
using NBS;
using NBS.ResponseModels;
//using POS_Core_UI.Reports.ReportsUI;
using NLog;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using POS_Core.LabelHandler.RxLabel;
using POS_Core.Resources;
using PossqlData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmPOSOnHoldTrans : System.Windows.Forms.Form
    {
        public string SearchTable = "";
        public bool IsCanceled = true;
        public bool DisplayRecordAtStartup = false;
        private SearchSvr oSearchSvr = new SearchSvr();
        public DataSet oDataSet = new DataSet();
        public Int32 TransID;
        public bool isReadonly = false;
        private int CurrentX;
        private int CurrentY;

        public string FormCaption = "";
        public string LabelText1 = "";
        public string LabelText2 = "";
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
        private Infragistics.Win.Misc.UltraLabel lbl2;
        private Infragistics.Win.Misc.UltraLabel lbl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private Infragistics.Win.Misc.UltraButton btnRemoveTrans;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor txtTransID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabelPaidStatus;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboPaidStatus;
        private IContainer components;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCustomer;
        private Infragistics.Win.Misc.UltraLabel lblCustomerName;
        private POSTransaction oPOSTrans = new POSTransaction();//2915
        //public string strTransIDs = string.Empty;   //PRIMEPOS-2639 22-Feb-2019 JY Added

        //2915
        TransDetailData oHoldTransDData;
        TransDetailRXSvr oTransDetailRxSvr = new TransDetailRXSvr();
        TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
        POSTransPaymentSvr oPosTransPaymentSvr = new POSTransPaymentSvr();
        List<PaymentTypes> objPaymentTypes;
        public bool IsPendingPayment = false;//2915
        public bool IsGetTransactionAgain = true;//2915
        public bool IsPartialTrans = false;//PRIMEPOS-3319
        private Infragistics.Win.Misc.UltraButton btnResendLink;
        private TableLayoutPanel tableLayoutPanel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10; //PRIMEPOS-3343
        private Infragistics.Win.Misc.UltraLabel ultraLabel13; //PRIMEPOS-3343
        public decimal PendingAmount = 0;//2915
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        public Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();//3006
        public frmPOSOnHoldTrans(POSTransaction oPostrans = null)//2915
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            oPOSTrans = oPostrans;//2915
        }

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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance(); //PRIMEPOS-3343
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance(); //PRIMEPOS-3343
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel(); //PRIMEPOS-3343
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel(); //PRIMEPOS-3343
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.txtCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtItemID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelPaidStatus = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboPaidStatus = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtTransID = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.clTo = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnRemoveTrans = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnResendLink = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl1.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPaidStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.tableLayoutPanel7);
            this.ultraTabPageControl1.Controls.Add(this.lblCustomerName);
            this.ultraTabPageControl1.Controls.Add(this.txtCustomer);
            this.ultraTabPageControl1.Controls.Add(this.txtItemID);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel3);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel12);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel2);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabelPaidStatus);
            this.ultraTabPageControl1.Controls.Add(this.cboTransType);
            this.ultraTabPageControl1.Controls.Add(this.cboPaidStatus);
            this.ultraTabPageControl1.Controls.Add(this.txtTransID);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Controls.Add(this.clTo);
            this.ultraTabPageControl1.Controls.Add(this.clFrom);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Font = new System.Drawing.Font("Verdana", 9F);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1114, 117);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 8; //PRIMEPOS-3343
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.512605F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.86555F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.352941F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.26891F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F)); //PRIMEPOS-3343
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F)); //PRIMEPOS-3343
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel6, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel5, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel11, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel4, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel7, 4, 0);
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel8, 5, 0);
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel13, 6, 0);//PRIMEPOS-3343
            this.tableLayoutPanel7.Controls.Add(this.ultraLabel10, 7, 0);//PRIMEPOS-3343
            this.tableLayoutPanel7.Location = new System.Drawing.Point(5, 89);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(663, 25);
            this.tableLayoutPanel7.TabIndex = 160;
            // 
            // ultraLabel6
            // 
            appearance1.BackColor = System.Drawing.Color.LightGreen;
            this.ultraLabel6.Appearance = appearance1;
            this.ultraLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ultraLabel6.Location = new System.Drawing.Point(33, 3);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(178, 19);
            this.ultraLabel6.TabIndex = 11;
            this.ultraLabel6.Text = "Paid Transaction";
            // 
            // ultraLabel5
            // 
            appearance2.BackColor = System.Drawing.Color.SpringGreen;
            appearance2.BackColor2 = System.Drawing.Color.SpringGreen;
            appearance2.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance2.BackColorDisabled = System.Drawing.Color.SpringGreen;
            appearance2.BackColorDisabled2 = System.Drawing.Color.SpringGreen;
            appearance2.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.ultraLabel5.Appearance = appearance2;
            this.ultraLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel5.Location = new System.Drawing.Point(3, 3);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(24, 19);
            this.ultraLabel5.TabIndex = 10;
            // 
            // ultraLabel11
            // 
            appearance3.BackColor = System.Drawing.Color.LightPink;
            appearance3.BackColor2 = System.Drawing.Color.LightPink;
            appearance3.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance3.BackColorDisabled = System.Drawing.Color.LightPink;
            appearance3.BackColorDisabled2 = System.Drawing.Color.LightPink;
            appearance3.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.ultraLabel11.Appearance = appearance3;
            this.ultraLabel11.Location = new System.Drawing.Point(217, 3);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(28, 19);
            this.ultraLabel11.TabIndex = 6;
            // 
            // ultraLabel4
            // 
            appearance4.BackColor = System.Drawing.Color.LightPink;
            this.ultraLabel4.Appearance = appearance4;
            this.ultraLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ultraLabel4.Location = new System.Drawing.Point(251, 3);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(218, 19);
            this.ultraLabel4.TabIndex = 7;
            this.ultraLabel4.Text = "Unpaid Transaction";
            // 
            // ultraLabel7
            // 
            appearance5.BackColor = System.Drawing.Color.GhostWhite;
            appearance5.BackColor2 = System.Drawing.Color.GhostWhite;
            appearance5.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance5.BackColorDisabled = System.Drawing.Color.GhostWhite;
            appearance5.BackColorDisabled2 = System.Drawing.Color.GhostWhite;
            appearance5.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.ultraLabel7.Appearance = appearance5;
            this.ultraLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel7.Location = new System.Drawing.Point(475, 3);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(26, 19);
            this.ultraLabel7.TabIndex = 12;
            // 
            // ultraLabel8
            // 
            appearance6.BackColor = System.Drawing.Color.GhostWhite;
            this.ultraLabel8.Appearance = appearance6;
            this.ultraLabel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ultraLabel8.Location = new System.Drawing.Point(507, 3);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(153, 19);
            this.ultraLabel8.TabIndex = 13;
            this.ultraLabel8.Text = "On Hold Transaction";
            #region PRIMEPOS-3343
            // 
            // ultraLabel13
            // 
            appearance56.BackColor = System.Drawing.Color.LightYellow;
            appearance56.BackColor2 = System.Drawing.Color.LightYellow;
            appearance56.BackColorAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance56.BackColorDisabled = System.Drawing.Color.LightYellow;
            appearance56.BackColorDisabled2 = System.Drawing.Color.LightYellow;
            appearance56.BackHatchStyle = Infragistics.Win.BackHatchStyle.Percent05;
            this.ultraLabel13.Appearance = appearance56;
            this.ultraLabel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel13.Location = new System.Drawing.Point(475, 3);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(26, 19);
            this.ultraLabel13.TabIndex = 12;
            // 
            // ultraLabel10
            // 
            appearance55.BackColor = System.Drawing.Color.LightYellow;
            this.ultraLabel10.Appearance = appearance55;
            this.ultraLabel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ultraLabel10.Location = new System.Drawing.Point(507, 3);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(153, 19);
            this.ultraLabel10.TabIndex = 13;
            this.ultraLabel10.Text = "Partial Payment Transaction";
            #endregion
            // 
            // lblCustomerName
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Appearance = appearance7;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.Font = new System.Drawing.Font("Verdana", 8F);
            this.lblCustomerName.Location = new System.Drawing.Point(662, 12);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(132, 18);
            this.lblCustomerName.TabIndex = 32;
            this.lblCustomerName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtCustomer
            // 
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance8.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance8.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance8;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton1);
            this.txtCustomer.Location = new System.Drawing.Point(536, 10);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(120, 22);
            this.txtCustomer.TabIndex = 2;
            this.txtCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // txtItemID
            // 
            this.txtItemID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance9.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance9.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton2.Appearance = appearance9;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtItemID.ButtonsRight.Add(editorButton2);
            this.txtItemID.Location = new System.Drawing.Point(536, 50);
            this.txtItemID.Name = "txtItemID";
            this.txtItemID.Size = new System.Drawing.Size(120, 22);
            this.txtItemID.TabIndex = 5;
            this.txtItemID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItemID.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemID_EditorButtonClick);
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(432, 52);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(53, 17);
            this.ultraLabel3.TabIndex = 31;
            this.ultraLabel3.Text = "Item ID";
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(432, 13);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(100, 17);
            this.ultraLabel12.TabIndex = 28;
            this.ultraLabel12.Text = "Customer Code";
            // 
            // ultraLabel2
            // 
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance10;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Location = new System.Drawing.Point(218, 13);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(73, 17);
            this.ultraLabel2.TabIndex = 10;
            this.ultraLabel2.Text = "Trans Type";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            //
            // ultraLabel2
            // 
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.ultraLabelPaidStatus.Appearance = appearance10;
            this.ultraLabelPaidStatus.AutoSize = true;
            this.ultraLabelPaidStatus.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabelPaidStatus.Location = new System.Drawing.Point(660, 53);
            this.ultraLabelPaidStatus.Name = "ultraLabelPaidStatus";
            this.ultraLabelPaidStatus.Size = new System.Drawing.Size(73, 17);
            this.ultraLabelPaidStatus.TabIndex = 10;
            this.ultraLabelPaidStatus.Text = "Payment Status";
            this.ultraLabelPaidStatus.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboTransType
            // 
            this.cboTransType.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboTransType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem4.DataValue = "0";
            valueListItem4.DisplayText = "All";
            valueListItem5.DataValue = "1";
            valueListItem5.DisplayText = "Delivery";
            valueListItem6.DataValue = "2";
            valueListItem6.DisplayText = "Non-Delivery";
            valueListItem7.DataValue = "3";
            valueListItem7.DisplayText = "PrimeRxPay Trans";
            valueListItem8.DataValue = "4";
            valueListItem8.DisplayText = "PrimeRxPay Expired";
            this.cboTransType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5,
            valueListItem6,
            valueListItem7,
            valueListItem8});
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(295, 10);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Nullable = false;
            this.cboTransType.Size = new System.Drawing.Size(130, 22);
            this.cboTransType.TabIndex = 1;
            // 
            // cboPaidStatus
            // 
            this.cboPaidStatus.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboPaidStatus.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboPaidStatus.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboPaidStatus.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem9.DataValue = "0";
            valueListItem9.DisplayText = "All";
            valueListItem10.DataValue = "1";
            valueListItem10.DisplayText = "Paid";
            valueListItem11.DataValue = "2";
            valueListItem11.DisplayText = "Unpaid";
            this.cboPaidStatus.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem9,
            valueListItem10,
            valueListItem11 });
            this.cboPaidStatus.LimitToList = true;
            this.cboPaidStatus.Location = new System.Drawing.Point(780, 50);
            this.cboPaidStatus.Name = "cboPaidStatus";
            this.cboPaidStatus.Nullable = false;
            this.cboPaidStatus.Size = new System.Drawing.Size(130, 22);
            this.cboPaidStatus.TabIndex = 1;
            // 
            // txtTransID
            // 
            appearance11.FontData.BoldAsString = "False";
            this.txtTransID.Appearance = appearance11;
            this.txtTransID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransID.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtTransID.FormatString = "###############";
            this.txtTransID.Location = new System.Drawing.Point(75, 10);
            this.txtTransID.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtTransID.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtTransID.MaskInput = "nnnnnnnnnn";
            this.txtTransID.MaxValue = 99999999D;
            this.txtTransID.MinValue = -1;
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.NullText = "0";
            this.txtTransID.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtTransID.Size = new System.Drawing.Size(130, 22);
            this.txtTransID.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtTransID.TabIndex = 0;
            this.txtTransID.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtTransID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance12;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Location = new System.Drawing.Point(5, 13);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(65, 17);
            this.ultraLabel1.TabIndex = 8;
            this.ultraLabel1.Text = "Trans No.";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // clTo
            // 
            this.clTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clTo.DateButtons.Add(dateButton1);
            this.clTo.Location = new System.Drawing.Point(295, 50);
            this.clTo.Name = "clTo";
            this.clTo.NonAutoSizeHeight = 22;
            this.clTo.Size = new System.Drawing.Size(130, 21);
            this.clTo.TabIndex = 4;
            this.clTo.Value = new System.DateTime(2011, 1, 11, 0, 0, 0, 0);
            // 
            // clFrom
            // 
            this.clFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clFrom.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clFrom.DateButtons.Add(dateButton2);
            this.clFrom.Location = new System.Drawing.Point(75, 50);
            this.clFrom.Name = "clFrom";
            this.clFrom.NonAutoSizeHeight = 22;
            this.clFrom.Size = new System.Drawing.Size(130, 21);
            this.clFrom.TabIndex = 3;
            this.clFrom.Value = new System.DateTime(2011, 1, 11, 0, 0, 0, 0);
            // 
            // lbl2
            // 
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance13;
            this.lbl2.AutoSize = true;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(218, 52);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(20, 17);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "To ";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance14.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Appearance = appearance14;
            this.lbl1.AutoSize = true;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(5, 52);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(36, 17);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "From ";
            this.lbl1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.SystemColors.Control;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance15;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance16;
            this.btnSearch.Location = new System.Drawing.Point(921, 46);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // clTo1
            // 
            this.clTo1.Location = new System.Drawing.Point(0, 0);
            this.clTo1.Name = "clTo1";
            this.clTo1.NonAutoSizeHeight = 21;
            this.clTo1.Size = new System.Drawing.Size(121, 21);
            this.clTo1.TabIndex = 0;
            this.clTo1.Value = new System.DateTime(2011, 1, 11, 0, 0, 0, 0);
            // 
            // clFrom1
            // 
            this.clFrom1.Location = new System.Drawing.Point(1, 1);
            this.clFrom1.Name = "clFrom1";
            this.clFrom1.NonAutoSizeHeight = 21;
            this.clFrom1.Size = new System.Drawing.Size(121, 21);
            this.clFrom1.TabIndex = 0;
            this.clFrom1.Value = new System.DateTime(2011, 1, 11, 0, 0, 0, 0);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSearch.DataSource = this.ultraDataSource1;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackColorDisabled = System.Drawing.Color.White;
            appearance17.BackColorDisabled2 = System.Drawing.Color.White;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance17;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand1.HeaderVisible = true;
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance20;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance21.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackColorDisabled = System.Drawing.Color.White;
            appearance22.BackColorDisabled2 = System.Drawing.Color.White;
            appearance22.BorderColor = System.Drawing.Color.Black;
            appearance22.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            appearance23.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance23.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance23.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.Color.White;
            appearance27.BackColorDisabled = System.Drawing.Color.White;
            appearance27.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance28.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance29.FontData.BoldAsString = "True";
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.TextHAlignAsString = "Left";
            appearance29.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance29;
            this.grdSearch.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.White;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance31.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.SystemColors.Control;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance33;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance34.BackColor = System.Drawing.Color.Navy;
            appearance34.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.Navy;
            appearance35.BackColorDisabled = System.Drawing.Color.Navy;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance35.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance35.BorderColor = System.Drawing.Color.Gray;
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance35;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.SystemColors.Control;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.White;
            appearance40.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance40;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(9, 157);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(1118, 339);
            this.grdSearch.TabIndex = 6;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSearch_CellChange);
            this.grdSearch.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdSearch_ClickCell);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // tabMain
            // 
            appearance41.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance41;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance42;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance43;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.ultraTabPageControl1);
            this.tabMain.Location = new System.Drawing.Point(9, 9);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(1118, 142);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
            appearance44.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance44;
            appearance45.BackColor = System.Drawing.Color.Transparent;
            appearance45.BackColor2 = System.Drawing.Color.Transparent;
            appearance45.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance45;
            appearance46.BackColor = System.Drawing.Color.Transparent;
            appearance46.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance46;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Criteria";
            this.tabMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.tabMain.TabStop = false;
            this.tabMain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tabMain.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1114, 117);
            // 
            // sbMain
            // 
            appearance47.BackColor = System.Drawing.Color.White;
            appearance47.BackColor2 = System.Drawing.SystemColors.Control;
            appearance47.BorderColor = System.Drawing.Color.Black;
            appearance47.FontData.Name = "Verdana";
            appearance47.FontData.SizeInPoints = 10F;
            appearance47.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance47;
            this.sbMain.Location = new System.Drawing.Point(0, 542);
            this.sbMain.Name = "sbMain";
            appearance48.BorderColor = System.Drawing.Color.Black;
            appearance48.BorderColor3DBase = System.Drawing.Color.Black;
            appearance48.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance48;
            appearance49.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance49;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(1142, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnCancel
            // 
            appearance50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance50.FontData.BoldAsString = "True";
            appearance50.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Appearance = appearance50;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(999, 502);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 28);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            appearance51.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance51.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance51.FontData.BoldAsString = "True";
            appearance51.ForeColor = System.Drawing.Color.White;
            this.btnOk.Appearance = appearance51;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(895, 502);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(95, 28);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&Process";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnRemoveTrans
            // 
            appearance52.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance52.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance52.FontData.BoldAsString = "True";
            appearance52.ForeColor = System.Drawing.Color.White;
            this.btnRemoveTrans.Appearance = appearance52;
            this.btnRemoveTrans.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnRemoveTrans.Location = new System.Drawing.Point(794, 502);
            this.btnRemoveTrans.Name = "btnRemoveTrans";
            this.btnRemoveTrans.Size = new System.Drawing.Size(95, 28);
            this.btnRemoveTrans.TabIndex = 11;
            this.btnRemoveTrans.Text = "&Remove";
            this.btnRemoveTrans.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnRemoveTrans.Click += new System.EventHandler(this.btnRemoveTrans_Click);
            // 
            // btnPrint
            // 
            appearance53.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance53.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance53.FontData.BoldAsString = "True";
            appearance53.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance53;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(685, 502);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(102, 28);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnResendLink
            // 
            appearance54.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance54.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance54.FontData.BoldAsString = "True";
            appearance54.ForeColor = System.Drawing.Color.White;
            this.btnResendLink.Appearance = appearance54;
            this.btnResendLink.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnResendLink.Location = new System.Drawing.Point(570, 502);
            this.btnResendLink.Name = "btnResendLink";
            this.btnResendLink.Size = new System.Drawing.Size(109, 28);
            this.btnResendLink.TabIndex = 17;
            this.btnResendLink.Text = "&ResendLink";
            this.btnResendLink.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnResendLink.Click += new System.EventHandler(this.btnResendLink_Click);
            // 
            // frmPOSOnHoldTrans
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1142, 567);
            this.Controls.Add(this.btnResendLink);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnRemoveTrans);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.grdSearch);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSOnHoldTrans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OnHold Transactions";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPaidStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                Search(true);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnSearch_Click(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void Search(bool isGetTransactionStatus)//To-Do Arvind
        {
            oDataSet = new DataSet();

            string sSQL = "Select Distinct TH." + clsPOSDBConstants.TransHeader_Fld_TransID + " as [Trans ID]"//2915
                + " , TH." + clsPOSDBConstants.TransHeader_Fld_TransDate + " as [Trans Date] "
                + ",TH.CustomerID as CustomerID "
                + " , " + "case TransType when 1 then 'Sale' else 'Return' end  as [Trans Type]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_StationID + " as [Station ID]"
                //+ " , " + clsPOSDBConstants.Customer_Fld_CustomerName + " as [Customer]"  //Sprint-23 - PRIMEPOS-2325 22-Jul-2016 JY Commented
                + " , CASE WHEN " + clsPOSDBConstants.Customer_Fld_FirstName + " IS NOT NULL OR LTRIM(RTRIM(" + clsPOSDBConstants.Customer_Fld_FirstName + " )) <> '' THEN " + clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+" + clsPOSDBConstants.Customer_Fld_FirstName + " ELSE " + clsPOSDBConstants.Customer_Fld_CustomerName + " END as [Customer]"    //Sprint-23 - PRIMEPOS-2325 22-Jul-2016 JY Added
                + " , " + clsPOSDBConstants.TransHeader_Fld_GrossTotal + " as [Gross Total]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount + " as [Disc. Amt]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount + " as [Tax Amt]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_TenderedAmount + " as [Tendered Amt]"
                + " , TH." + clsPOSDBConstants.Users_Fld_UserID + " as [User ID]"
                + " , TH." + clsPOSDBConstants.TransHeader_Fld_IsCustomerDriven + " as [IsCustomerDriven]"//2915
                + " , TH." + clsPOSDBConstants.TransHeader_Fld_TotalPaid + " as [TotalPaid]"//2915
                                                                                            //+ " , TH." + clsPOSDBConstants.TransHeader_Fld_PrimeRxPayTransID + " as [PrimeRxPayTransID]"
                + " FROM " + clsPOSDBConstants.TransHeader_OnHold_tbl + " as TH WITH(NOLOCK) "
                + "INNER JOIN " + clsPOSDBConstants.Customer_tbl + " as Cus  WITH(NOLOCK)  ON TH." + clsPOSDBConstants.Customer_Fld_CustomerId + " = Cus." + clsPOSDBConstants.TransHeader_Fld_CustomerID
                //+ " INNER JOIN  POSTransPayment_OnHold PTP on TH.TransID = PTP.TransID "
                + " LEFT JOIN POSTransPayment_OnHold as PTP  WITH(NOLOCK)  ON TH.TransID = PTP.TransID "
                + " WHERE 1=1 ";


            if (Configuration.convertNullToInt(txtTransID.Value) > 0)
            {
                sSQL += " and TH.TransID=" + Configuration.convertNullToInt(txtTransID.Value).ToString();
            }
            else
            {
                sSQL += " and convert(datetime,TH.TransDate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59' as datetime) ,113) ";

                if (Configuration.convertNullToInt(cboTransType.Value) == 1)
                {
                    sSQL += " and TH.IsDelivery=1 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 2)
                {
                    sSQL += " and TH.IsDelivery=0 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 3)//2915
                {
                    sSQL += " and TH.IsPaymentPending = 1 and TH.IsDelivery=0 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 4)//2915
                {
                    sSQL += " and PTP.Status = 'Expired'";
                }
                if (!isGetTransactionStatus)
                {
                    if (Configuration.convertNullToInt(cboPaidStatus.Value) == 1)//Paid
                    {
                        sSQL += " and ISNULL(PTP.Status,'Completed') = 'Completed'  ";
                    }
                    else if (Configuration.convertNullToInt(cboPaidStatus.Value) == 2)//Unpaid
                    {
                        sSQL += " and ISNULL(PTP.Status,'Completed') = 'In Progress' ";
                    }
                }
                #region PRIMEPOS-2646 06-May-2019 JY Added
                if (this.txtCustomer.Tag != null)
                {
                    sSQL += " and TH.CustomerID = " + Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";
                }

                if (string.IsNullOrEmpty(this.txtItemID.Text.Trim()) == false)
                {
                    if (this.txtItemID.Text.Trim().ToUpper().EndsWith(Configuration.CPOSSet.RXCode.ToUpper()))
                    {
                        sSQL += "AND TH.TransID IN (SELECT TransID from POSTransactionDetail_OnHold WHERE ItemDescription LIKE '" + this.txtItemID.Text.Trim().Substring(0, this.txtItemID.Text.Length - Configuration.CPOSSet.RXCode.Length).Replace("'", "''") + "%')";
                    }
                    else
                    {
                        sSQL += "AND TH.TransID IN (SELECT TransID from POSTransactionDetail_OnHold WHERE ItemID = '" + this.txtItemID.Text.Trim().Replace("'", "''") + "')";
                    }
                }
                #endregion
            }

            sSQL += " order by TH.transid desc ";

            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
            oDataSet.Tables[0].TableName = "Master";

            sSQL = "Select Distinct TransDetailID, "//2915
                   + clsPOSDBConstants.TransDetail_Fld_ItemID
                   + " , " + clsPOSDBConstants.TransDetail_Fld_ItemDescription + " as [Item Name] "
                   + " , " + clsPOSDBConstants.TransDetail_Fld_QTY
                   + " , " + clsPOSDBConstants.TransDetail_Fld_Price + " as [Unit Price] "
                   + " , " + clsPOSDBConstants.TransDetail_Fld_TaxAmount + " as [Tax Amt] "
                   + " ,PTD." + clsPOSDBConstants.TransDetail_Fld_Discount + " as [Disc. Amt] "
                   + " , " + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice + " as [Ext. Price]"
                   + " , PTD." + clsPOSDBConstants.TransDetail_Fld_TransID
                   + " FROM "
                   + clsPOSDBConstants.TransDetail_OnHold_tbl + " as PTD  WITH(NOLOCK) "
                   + " INNER JOIN " + clsPOSDBConstants.TransHeader_OnHold_tbl + " as PTH  WITH(NOLOCK) ON PTH." + clsPOSDBConstants.TransHeader_Fld_TransID + " = PTD." + clsPOSDBConstants.TransDetail_Fld_TransID
                   + " INNER JOIN " + clsPOSDBConstants.Customer_tbl + " as CTS  WITH(NOLOCK) ON PTH." + clsPOSDBConstants.CustomerNotes_Fld_CustomerID + " = CTS." + clsPOSDBConstants.CustomerNotes_Fld_CustomerID
                   + " LEFT JOIN POSTransPayment_OnHold as PTP ON PTP.transid = PTD.TransID"
                   + " where 1=1 ";


            if (Configuration.convertNullToInt(txtTransID.Value) > 0)
            {
                sSQL += " and PTH.TransID=" + Configuration.convertNullToInt(txtTransID.Value).ToString();
            }
            else
            {
                sSQL += " and convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113) ";


                if (Configuration.convertNullToInt(cboTransType.Value) == 1)
                {
                    sSQL += " and PTH.IsDelivery=1 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 2)
                {
                    sSQL += " and PTH.IsDelivery=0 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 3)//2915
                {
                    sSQL += " and PTH.IsPaymentPending = 1 and PTH.IsDelivery=0 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 4)//2915
                {
                    sSQL += " and PTP.Status = 'Expired' ";
                }
                if (!isGetTransactionStatus)
                {
                    if (Configuration.convertNullToInt(cboPaidStatus.Value) == 1)//Paid
                    {
                        sSQL += " and ISNULL(PTP.Status,'Completed') = 'Completed'  ";
                    }
                    else if (Configuration.convertNullToInt(cboPaidStatus.Value) == 2)//Unpaid
                    {
                        sSQL += " and ISNULL(PTP.Status,'Completed') = 'In Progress' ";
                    }
                }
                #region PRIMEPOS-2646 06-May-2019 JY Added
                if (this.txtCustomer.Tag != null)
                {
                    sSQL += " and PTH.CustomerID = " + Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";
                }

                if (string.IsNullOrEmpty(this.txtItemID.Text.Trim()) == false)
                {
                    if (this.txtItemID.Text.Trim().ToUpper().EndsWith(Configuration.CPOSSet.RXCode.ToUpper()))
                    {
                        sSQL += "AND PTH.TransID IN (SELECT TransID from POSTransactionDetail_OnHold WHERE ItemDescription LIKE '" + this.txtItemID.Text.Trim().Substring(0, this.txtItemID.Text.Length - Configuration.CPOSSet.RXCode.Length).Replace("'", "''") + "%')";
                    }
                    else
                    {
                        sSQL += "AND PTH.TransID IN (SELECT TransID from POSTransactionDetail_OnHold WHERE ItemID = '" + this.txtItemID.Text.Trim().Replace("'", "''") + "')";
                    }
                }
                #endregion
            }


            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());

            oDataSet.Tables[1].TableName = "Detail";

            oPosTransPaymentSvr.DeleteDuplicateRows();            
            sSQL = "Select "
                   + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_Amount + " as [Amount] "
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount + " as [Approved Amount]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_Email + " as [Email]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_Mobile + " as [Mobile]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode + " as [TransactionProcessingMode]"
                   + " ,PTD. " + clsPOSDBConstants.POSTransPayment_Fld_TransDate + " as [TransDate]" //PRIMEPOS-3453
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " as [TransPayId]"
                   + " , ISNULL(" + clsPOSDBConstants.POSTransPayment_Fld_Status + ",'Completed') as [Status]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_TransID
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_NBSTransId + " as [NBSTransId]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid + " as [NBSTransUid]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType + " as [NBSPaymentType]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID + " as [PrimeRxPayTransID]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID + " as [ProcessorTransID]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor + " as [PaymentProcessor]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_TicketNumber + " as [TicketNo]"
                   + " FROM " + clsPOSDBConstants.POSTransPayment_OnHold_tbl + " as PTD  WITH(NOLOCK) "
                   + " INNER JOIN " + clsPOSDBConstants.TransHeader_OnHold_tbl + " as PTH  WITH(NOLOCK) ON PTH." + clsPOSDBConstants.TransHeader_Fld_TransID + " = PTD." + clsPOSDBConstants.TransDetail_Fld_TransID
                   + " INNER JOIN " + clsPOSDBConstants.Customer_tbl + " as CTS  WITH(NOLOCK) ON PTH." + clsPOSDBConstants.CustomerNotes_Fld_CustomerID + " = CTS." + clsPOSDBConstants.CustomerNotes_Fld_CustomerID
                   + " where 1=1  ";


            if (Configuration.convertNullToInt(txtTransID.Value) > 0)
            {
                sSQL += " and PTH.TransID=" + Configuration.convertNullToInt(txtTransID.Value).ToString();
            }
            else
            {
                sSQL += " and convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113) ";    //PRIMEPOS-3058 03-Mar-2022 JY Added

                if (Configuration.convertNullToInt(cboTransType.Value) == 1)
                {
                    sSQL += " and PTH.IsDelivery=1 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 2)
                {
                    sSQL += " and PTH.IsDelivery=0 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 3)//2915
                {
                    sSQL += " and PTH.IsPaymentPending = 1 and PTH.IsDelivery=0 ";
                }
                else if (Configuration.convertNullToInt(cboTransType.Value) == 4)//2915
                {
                    sSQL += " and PTD.Status = 'Expired' ";
                }
                if (!isGetTransactionStatus)
                {
                    if (Configuration.convertNullToInt(cboPaidStatus.Value) == 1)//Paid
                    {
                        sSQL += " and ISNULL(PTD.Status,'Completed') = 'Completed'  ";
                    }
                    else if (Configuration.convertNullToInt(cboPaidStatus.Value) == 2)//Unpaid
                    {
                        sSQL += " and ISNULL(PTD.Status,'Completed') = 'In Progress' ";
                    }
                }
                #region PRIMEPOS-2646 06-May-2019 JY Added
                if (this.txtCustomer.Tag != null)
                {
                    sSQL += " and PTH.CustomerID = " + Configuration.convertNullToInt(this.txtCustomer.Tag.ToString().Trim()).ToString() + " ";
                }

                if (string.IsNullOrEmpty(this.txtItemID.Text.Trim()) == false)
                {
                    if (this.txtItemID.Text.Trim().ToUpper().EndsWith(Configuration.CPOSSet.RXCode.ToUpper()))
                    {
                        sSQL += "AND PTH.TransID IN (SELECT TransID from POSTransactionDetail_OnHold WHERE ItemDescription LIKE '" + this.txtItemID.Text.Trim().Substring(0, this.txtItemID.Text.Length - Configuration.CPOSSet.RXCode.Length).Replace("'", "''") + "%')";
                    }
                    else
                    {
                        sSQL += "AND PTH.TransID IN (SELECT TransID from POSTransactionDetail_OnHold WHERE ItemID = '" + this.txtItemID.Text.Trim().Replace("'", "''") + "')";
                    }
                }
                #endregion
            }
            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());

            oDataSet.Tables[2].TableName = "Payment";

            oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["Trans ID"], oDataSet.Tables[1].Columns["TransID"]);
            oDataSet.Relations.Add("MasterDetailPayment", oDataSet.Tables[0].Columns["Trans ID"], oDataSet.Tables[2].Columns["TransID"]);
            grdSearch.DataSource = oDataSet;


            grdSearch.DisplayLayout.Bands[0].HeaderVisible = true;
            grdSearch.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 12;
            grdSearch.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

            grdSearch.DisplayLayout.Bands[0].Header.Caption = "Transactions";

            grdSearch.DisplayLayout.Bands[1].Columns["TransID"].Hidden = true;
            grdSearch.DisplayLayout.Bands[1].Expandable = true;

            grdSearch.DisplayLayout.Bands[1].HeaderVisible = true;
            grdSearch.DisplayLayout.Bands[1].Header.Caption = "Transaction Detail";
            grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 12;
            grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

            grdSearch.DisplayLayout.Bands[2].Columns["TransID"].Hidden = true;//2915
            grdSearch.DisplayLayout.Bands[2].Expandable = true;

            grdSearch.DisplayLayout.Bands[2].HeaderVisible = true;
            grdSearch.DisplayLayout.Bands[2].Header.Caption = "Payment Detail";
            grdSearch.DisplayLayout.Bands[2].Header.Appearance.FontData.SizeInPoints = 12;
            grdSearch.DisplayLayout.Bands[2].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;


            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
            grdSearch.Refresh();
            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;

            if (this.IsGetTransactionAgain && isGetTransactionStatus)
            {
                //GetTransactionStatus();
                GetMultipleTransactionStatus(); //PRIMEPOS-3333
            }
            ColorChangeTransaction();
        }
        private void ColorChangeTransaction()
        {
            bool isTransCOMPLETED = false;
            bool isTransEXPIRED = false;
            bool isTransPARTIALAPPROVED = false; 
            bool isTransINPROGRESS = false; 
            bool isTransCANCELLED = false; //PRIMPPOS-3343
            UltraGridBand bandMaster = this.grdSearch.DisplayLayout.Bands[0];
            foreach (UltraGridRow Parentrow in bandMaster.GetRowEnumerator(GridRowType.DataRow)) //PRIMEPOS-3343-TEMP master table of on hold transactions 
            {

                UltraGridBand bandDetail = this.grdSearch.DisplayLayout.Bands[2];

                foreach (UltraGridRow ChildPaymentrow in bandDetail.GetRowEnumerator(GridRowType.DataRow)) //PRIMEPOS-3343-TEMP payment table of perticular on hold transaction 

                {
                    if (Convert.ToString(ChildPaymentrow.Cells["TransID"].Value) == Convert.ToString(Parentrow.Cells["Trans ID"].Value))
                    {
                        if (Convert.ToBoolean(Convert.ToString(Parentrow.Cells["IsCustomerDriven"].Value)))
                        {
                            //if (Convert.ToBoolean(Convert.ToString(Parentrow.Cells["IsCustomerDriven"].Value))
                            //    && Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Expired)
                            //{
                            //    //Parentrow.Cells["TransNo"].Appearance.BackColor = Color.LightSkyBlue;
                            //    Parentrow.Appearance.BackColor = Color.DarkBlue;
                            //    Parentrow.Update();
                            //}

                            //if (Convert.ToBoolean(Convert.ToString(Parentrow.Cells["IsCustomerDriven"].Value))
                            //    && Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Cancelled)
                            //{
                            //    //Parentrow.Cells["TransNo"].Appearance.BackColor = Color.LightSkyBlue;
                            //    Parentrow.Appearance.BackColor = Color.Black;
                            //    Parentrow.Update();
                            //}
                            #region PRIMEPOS-3343
                            //if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_InProgress || Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Expired)
                            //{
                            //    //Parentrow.Cells["TransNo"].Appearance.BackColor = Color.LightSkyBlue;
                            //    Parentrow.Appearance.BackColor = Color.LightPink;
                            //    Parentrow.Update();
                            //    break;
                            //}
                            //if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Completed)
                            //{
                            //    //Parentrow.Cells["TransNo"].Appearance.BackColor = Color.LightSkyBlue;
                            //    Parentrow.Appearance.BackColor = Color.LightGreen;
                            //    Parentrow.Update();
                            //    break;
                            //}
                            if (Convert.ToString(ChildPaymentrow.Cells["Amount"].Value) == Convert.ToString(ChildPaymentrow.Cells["Approved Amount"].Value))
                            {
                                if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Completed)
                                {
                                    isTransCOMPLETED = true;
                                }
                            }
                            else 
                            {
                                if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Completed) //PRIMEPOS-3343
                                {
                                    isTransPARTIALAPPROVED = true;
                                }
                                if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_InProgress)
                                {
                                    isTransINPROGRESS = true;
                                }
                                else if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Expired)
                                {
                                    isTransEXPIRED = true;
                                }
                                else if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_PartialApproved)
                                {
                                    isTransPARTIALAPPROVED = true;
                                }
                                else if (Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Cancelled)
                                {
                                    isTransCANCELLED = true;
                                }
                            }


                            //if (Convert.ToString(ChildPaymentrow.Cells["Amount"].Value) == Convert.ToString(ChildPaymentrow.Cells["Amount Approved"].Value) && Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_Completed) //PRIMEPOS-3334
                            //{
                            //    isTransCOMPLETED = true;
                            //}
                            //else if (Convert.ToString(ChildPaymentrow.Cells["Amount"].Value) != Convert.ToString(ChildPaymentrow.Cells["Amount Approved"].Value) && Convert.ToString(ChildPaymentrow.Cells["Status"].Value) == clsPOSDBConstants.PosTransPayment_Status_InProgress) //PRIMEPOS-3334
                            //{
                            //    isTransINPROGRESS = true;
                            //}

                            #endregion
                        }
                    }
                }
                #region PRIMEPOS-3343 
                if (isTransCOMPLETED)
                {
                    if (isTransINPROGRESS)
                    {
                        Parentrow.Appearance.BackColor = Color.LightPink;
                    }
                    else if (isTransEXPIRED || isTransPARTIALAPPROVED || isTransCANCELLED)
                    {
                        Parentrow.Appearance.BackColor = Color.LightYellow;
                    }
                    else 
                    {
                        Parentrow.Appearance.BackColor = Color.LightGreen;
                    }

                }
                else if (isTransPARTIALAPPROVED) 
                {
                    Parentrow.Appearance.BackColor = Color.LightYellow;
                }
                else
                {
                    if (isTransINPROGRESS || isTransEXPIRED || isTransCANCELLED)
                    {
                        Parentrow.Appearance.BackColor = Color.LightPink;
                        //break;
                    }
                }
                Parentrow.Update();
                isTransCOMPLETED = false;
                isTransEXPIRED = false;
                isTransPARTIALAPPROVED = false;
                isTransINPROGRESS = false;
                isTransCANCELLED = false;
                #endregion
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void grdSearch_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.RowUIElement oRowUI = oUI as Infragistics.Win.UltraWinGrid.RowUIElement;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
                        {
                            orow.CollapseAll();
                        }
                        oRowUI.Row.ExpandAll();
                    }
                    oUI = oUI.Parent;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "grdSearch_DoubleClick(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                //this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(1, 0, 0, 0)); //PRIMEPOS-3250
                //this.clTo.Value = DateTime.Today; //PRIMEPOS-3250
                if (Configuration.CSetting.PrimeRxPayDefaultSelection) //PRIMEPOS-3250
                {
                    if (Configuration.CSetting.PrimeRxPayBGStatusUpdate)
                    {
                        this.cboTransType.Value = 3;
                        this.cboPaidStatus.Value = 1;
                        this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(1, 0, 0, 0)); //PRIMEPOS-3250
                        this.clTo.Value = DateTime.Today; //PRIMEPOS-3250
                    }
                    else
                    {
                        this.cboTransType.Value = 0;
                        this.cboPaidStatus.Value = 0;
                        this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(8, 0, 0, 0)); //PRIMEPOS-3250
                        this.clTo.Value = DateTime.Today; //PRIMEPOS-3250
                    }
                }
                else
                {
                    this.cboTransType.Value = 0;
                    this.cboPaidStatus.Value = 0;
                    this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(8, 0, 0, 0)); //PRIMEPOS-3250
                    this.clTo.Value = DateTime.Today; //PRIMEPOS-3250
                }

                clsUIHelper.SetKeyActionMappings(this.grdSearch);
                clsUIHelper.SetAppearance(this.grdSearch);
                clsUIHelper.SetReadonlyRow(this.grdSearch);

                this.clFrom.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.clFrom.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.clTo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.clTo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtTransID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtTransID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                using (var db = new Possql())
                {
                    objPaymentTypes = db.PayTypes.ToList();
                }

                #region PRIMEPOS-3291
                //Search(!Configuration.CSetting.PrimeRxPayBGStatusUpdate);
                if (Configuration.CSetting.PrimeRxPayDefaultSelection)
                {
                    Search(!Configuration.CSetting.PrimeRxPayBGStatusUpdate);
                }
                else
                {
                    Search(true);
                }
                #endregion
                this.txtTransID.Focus();
                clsUIHelper.setColorSchecme(this);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearch_Load(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter && this.ActiveControl.Name != "grdSearch")
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.grdSearch.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    if (this.grdSearch.DisplayLayout.Bands[0].Columns[i].DataType == typeof(System.Decimal))
                    {
                        this.grdSearch.DisplayLayout.Bands[0].Columns[i].Format = "#######0.00";
                        this.grdSearch.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)");
            }
        }

        private void frmSearchMain_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void grdSearch_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void TextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Down)
                {
                    if (this.grdSearch.Rows.Count > 0)
                    {
                        this.grdSearch.Focus();
                        this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "TextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
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
                        oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "resizeColumns()");
            }
        }

        private void grdSearch_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }



        private void btnOk_Click(object sender, System.EventArgs e)
        {
            try
            {
                IsPartialTrans = false;
                logger.Trace("btnOk_Click(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Entering);
                if (this.grdSearch.ActiveRow == null)
                    return;

                if (grdSearch.ActiveRow.Band.Index > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
                }

                TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());
                if (Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["IsCustomerDriven"].Text.ToString()))//2915    //PRIMEPOS-2964 12-May-2021 JY modified to handle NULL properly 
                {
                    if (CheckInprogressPayment(TransID))
                    {
                        MessageBox.Show($"One or more payment(s) are pending.{Environment.NewLine}Please refresh with Payment Status(All) to have complete information about the pending payments.", "Onhold Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    bool retVal = GetTransactionDetail(grdSearch.ActiveRow);
                    if (!CheckAnyPaymentCompleted(TransID))
                    {
                        MessageBox.Show($"No Payment are completed, Can not process.", "Onhold Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else 
                    {
                        if (CheckAnyPaymentCompletedAndRemainingPay(TransID) || IsPartialTrans) //PRIMEPOS-3343
                        {
                            if (MessageBox.Show("This transaction contains a pending amount. Do you want to proceed?", "Onhold Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                IsPartialTrans = false;
                                return;
                            }
                        }
                    }
                    Decimal Amounts = 0; //PRIEMPOS-3354
                    if (Configuration.convertNullToInt(cboTransType.Value) == 4) //PRIEMPOS-3354
                    {
                        Amounts = Convert.ToDecimal(GetTotalAmountInPosTransIfExpired(Convert.ToInt32(this.grdSearch.ActiveRow.Cells[0].Text)));
                    }
                    else
                    {
                        Amounts = Convert.ToDecimal(GetTotalAmountInPosTrans(this.grdSearch.ActiveRow.Cells[0].Text.ToString()));
                    }

                    if (Amounts > 0 && retVal)
                    {
                        var TotalPaid = oDataSet.Tables[0].AsEnumerable().Where(r1 =>
                        r1.Field<int>("Trans ID") == Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString()))
                        .Select(g => g.Field<decimal>("TotalPaid")).FirstOrDefault();

                        decimal PendingAmount = TotalPaid - Amounts;

                        //if (PendingAmount > 0)
                        //{
                        this.PendingAmount = PendingAmount;
                        this.IsPendingPayment = true;
                        this.Close();
                        //}
                    }
                }
                else
                {
                    this.IsCanceled = false;
                    //Added by krishna on 28 July 2011
                    TransDetailRXSvr.ProcOnHoldFlag = true;
                    //till here added by krishna on 28 July 2011
                }
                this.Close();

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnOk_Click(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                logger.Trace("btnOk_Click(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            }
        }

        private bool CheckInprogressPayment(int transID)
        {
            bool isInProgress = false;

            string sql = $"Select Count(1) from POSTransPayment_OnHold where [Status]='In Progress' and TransID={transID}";

            isInProgress = (Convert.ToInt32(oSearchSvr.SearchScalar(sql)) > 0);
            return isInProgress;
        }
        private bool CheckAnyPaymentCompleted(int transID)
        {
            bool isIPaymentCompleted = false;

            string sql = $"Select Count(1) from POSTransPayment_OnHold where ISNULL([Status], 'Completed')='Completed' and TransID={transID}";

            isIPaymentCompleted = (Convert.ToInt32(oSearchSvr.SearchScalar(sql)) > 0);
            return isIPaymentCompleted;
        }

        #region PRIMEPOS-3247
        private DataTable GetAllPaymentDetails(string transID)
        {
            string sSQL = "Select "
                   + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_Amount + " as [Amount] "
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount + " as [Approved Amount]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_Email + " as [Email]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_Mobile + " as [Mobile]"
                   + " , PTD." + clsPOSDBConstants.POSTransPayment_Fld_TransDate + " as [TransDate]" //PRIMEPOS-3453
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode + " as [TransactionProcessingMode]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " as [TransPayId]"
                   + " , ISNULL(" + clsPOSDBConstants.POSTransPayment_Fld_Status + ",'Completed') as [Status]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_TransID
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_NBSTransId + " as [NBSTransId]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid + " as [NBSTransUid]"
                   + " , " + clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType + " as [NBSPaymentType]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID + " as [PrimeRxPayTransID]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID + " as [ProcessorTransID]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor + " as [PaymentProcessor]"
                   + " , " + "PTD." + clsPOSDBConstants.POSTransPayment_Fld_TicketNumber + " as [TicketNo]"
                   + " FROM " + clsPOSDBConstants.POSTransPayment_OnHold_tbl + " as PTD  WITH(NOLOCK) "
                   + " where 1=1 AND TransID= "+transID;

            return oSearchSvr.Search(sSQL).Tables[0];
        }
        #endregion

        #region PRIMEPOS-3343
        private bool CheckAnyPaymentCompletedAndRemainingPay(int transID)
        {
            bool isIPaymentCompletedAndRemainingPay = false;

            string sql = $"SELECT count(1) FROM POSTransPayment_OnHold WHERE [Status] IN ('{clsPOSDBConstants.PosTransPayment_Status_PartialApproved}', 'expired', 'cancelled') and TransID={transID}"; //PRIMEPOS-3343

            isIPaymentCompletedAndRemainingPay = (Convert.ToInt32(oSearchSvr.SearchScalar(sql)) > 0);
            return isIPaymentCompletedAndRemainingPay;
        }
        #endregion

        #region PRIMEPOS-3354
        private Decimal GetTotalAmountInPosTransIfExpired(int transID)
        {
            Decimal approvedAmount = 0;

            string sql = $"Select Sum(ApprovedAmount) from POSTransPayment_OnHold WITH (NOLOCK) where TransID={transID}";

            approvedAmount = (Convert.ToDecimal(oSearchSvr.SearchScalar(sql)));
            return approvedAmount;
        }
        #endregion


        private void btnRemoveTrans_Click(object sender, System.EventArgs e)
        {
            try
            {
                logger.Trace("btnRemoveTrans_Click(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Entering);

                if (this.grdSearch.ActiveRow == null)
                    return;

                if (grdSearch.ActiveRow.Band.Index > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
                }
                //added by shitaljit to check User UserPriviliges  and  show warning message 
                if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteTransOnHold.ID, UserPriviliges.Permissions.DeleteTransOnHold.Name))
                {
                    if (Resources.Message.Display("Are you sure you want to remove Selected On-Hold Transaction?", "Transaction On Hold", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        if (Convert.ToBoolean(grdSearch.ActiveRow.Cells["IsCustomerDriven"].Text.ToString()))
                        {
                            //PRIMEPOS-3247
                            //Get the all payment details of the transaction
                            //DataSet ds = new DataSet();
                            DataTable PosTransPaymentDetail = GetAllPaymentDetails(Convert.ToString(grdSearch.ActiveRow.Cells[0].Value));

                            DataRow[] PosTransPaymenttbl = PosTransPaymentDetail.Select("TransID = " + Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Value));

                            //DataRow[] PosTransPaymenttbl = oDataSet.Tables[2].
                            //        Select("TransID = " + Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Value));
                            int count = 0;
                            foreach (DataRow datarow in PosTransPaymenttbl)
                            {
                                logger.Trace("Removing the payment now");
                                if (Convert.ToString(datarow["Status"]) == clsPOSDBConstants.PosTransPayment_Status_Expired)
                                {
                                    if (grdSearch.ActiveRow != null)
                                    {
                                        clsUIHelper.ShowOKMsg("Transaction voided successfully ");
                                        int iTransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());
                                        POSTransaction osvr = new POSTransaction();
                                        osvr.RemoveOnHoldTrans(iTransID);
                                        Search(false);
                                    }
                                }
                                else if (RemovePrimeRxPay(datarow))
                                {
                                    count++;
                                    oPosTransPaymentSvr.DeletePrimeRxPayCanceledTrans(datarow["TransPayID"].ToString());

                                    if (count == PosTransPaymenttbl.ToList().Count)
                                    {
                                        if (grdSearch.ActiveRow != null)
                                        {
                                            clsUIHelper.ShowOKMsg("Transaction voided successfully ");
                                            int iTransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());
                                            POSTransaction osvr = new POSTransaction();
                                            osvr.RemoveOnHoldTrans(iTransID, false);
                                            Search(false);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            int iTransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());
                            POSTransaction osvr = new POSTransaction();
                            osvr.RemoveOnHoldTrans(iTransID);
                            Search(false);
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnRemoveTrans_Click(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                logger.Trace("btnRemoveTrans_Click(object sender, System.EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnPrint_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Entering);

                if (this.grdSearch.ActiveRow == null)
                    return;

                if (grdSearch.ActiveRow.Band.Index > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
                }

                int printTransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());

                if (printTransID < 1)
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

                oTransHData = oTransHSvr.GetOnHoldData(printTransID);

                oTransDData = oTransDSvr.GetOnHoldTransDetail(printTransID);

                TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTaxOnHold(printTransID); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

                oTransPaymentData = new POSTransPaymentData();

                //added by atul 07-jan-2011
                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    if (oTransDData.TransDetail[0].ItemID.ToString().Trim().ToUpper() == "RX")
                    {
                        //oTransRxData = oTransRxSvr.PopulateData(printTransID);//Commented By Shitaljit On 12 June 2012
                        //To pupulate oTransRxData code should look into hold tables not in trasaction able.
                        oTransRxData = oTransRxSvr.getOnHoldTransDetailRX(printTransID);
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, true, ReceiptType.OnHoldTransction, dtTransDetailTax);
                        oRxLabel.Print();
                    }
                    else
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, true, ReceiptType.OnHoldTransction, dtTransDetailTax);
                        oRxLabel.Print();
                    }
                }
                else //End of added by atul 07-jan-2011
                {
                    RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, true, ReceiptType.OnHoldTransction, dtTransDetailTax);
                    oRxLabel.Print();
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnPrint_Click(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                logger.Trace("btnPrint_Click(object sender, EventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            }
        }

        #region PRIMEPOS-2646 06-May-2019 JY Added
        private void txtCustomer_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCustomer();
            }
            catch (Exception)
            {
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
            //FKEdit("-1", clsPOSDBConstants.Customer_tbl);
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.txtCustomer.Tag = String.Empty;
            //this.lblCustomerName.Text = String.Empty; //PRIMEPOS-2426 06-Jun-2019 JY Added
        }

        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Customer_tbl)
            {
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
                        //Added By Dharmendra(SRT) which will be required when processing
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
            }
            else if (senderName == clsPOSDBConstants.Item_tbl)
            {
                #region item
                try
                {
                    POS_Core.BusinessRules.Item oItem = new Item();
                    ItemData oItemData;
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(code);
                    oItemRow = oItemData.Item[0];
                    if (oItemRow != null)
                    {
                        this.txtItemID.Text = oItemRow.ItemID;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtItemID.Text = String.Empty;
                    SearchItem();
                }
                catch (Exception exp)
                {
                    exp = null;
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.txtItemID.Text = String.Empty;
                    SearchItem();
                }
                #endregion
            }
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
                    this.txtCustomer.Tag = null; //Modified by Amit Date 1 Nov 2011 ORIGINAL this.txtCustomer.Tag = ""; 
                    this.lblCustomerName.Text = "";
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtItemID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem();
        }

        private void SearchItem()
        {
            try
            {
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Item_tbl);
                    this.txtItemID.Focus();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private bool GetTransactionDetail(UltraGridRow row)//2915{
        {
            bool retval = true;
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                logger.Trace("Entered in GetTransactionDetail ");
                UltraGridRow grdRow = row;
                IsGetTransactionAgain = false;
                DataRow[] PosTransPaymenttbl = null;
                PrimeRxPay.PrimeRxPayResponse oPrimeRxPayResponse = new PrimeRxPay.PrimeRxPayResponse();
                PrimeRxPay.PrimeRxPayProcessor oPrimeRxPay = PrimeRxPay.PrimeRxPayProcessor.GetInstance();
                string cardType = string.Empty;
                PaymentTypes paymentTypes = null;
                string TotalAmountInPosTrans = string.Empty;
                decimal pendingAmount = 0;
                if (Convert.ToBoolean(grdRow.Cells["IsCustomerDriven"].Value))
                {
                    //PRIMEPOS-3422
                    DataTable dt = GetAllPaymentDetails(Convert.ToString(grdRow.Cells["Trans Id"].Value));
                    PosTransPaymenttbl = dt.Select("TransID = " + Convert.ToString(grdRow.Cells["Trans Id"].Value));

                    //PosTransPaymenttbl = oDataSet.Tables[2].Select("TransID = " + Convert.ToString(grdRow.Cells["Trans Id"].Value));

                    foreach (var Postransrow in PosTransPaymenttbl)
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(Postransrow["PrimeRxPayTransID"]))
                            && Convert.ToString(Postransrow["Status"]).ToUpper() != clsPOSDBConstants.PosTransPayment_Status_Completed.ToUpper())
                        {
                            //Add logic of lookupdays
                            TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(Postransrow["TransDate"]); //PRIMEPOS-3453
                            int lookUpDays = timeSpan.Days + 5; //PRIMEPOS-3453

                            oPrimeRxPayResponse = oPrimeRxPay.GetTransactionDetail(Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl, Convert.ToString(Postransrow["PrimeRxPayTransID"]), Configuration.CSetting.PayProviderID.ToString(), Configuration.CSetting.PrimeRxPayClientId, Configuration.CSetting.PrimeRxPaySecretKey, lookUpDays); //PRIMEPOS-3453 Added LookUpDays

                            if (oPrimeRxPayResponse != null && !string.IsNullOrWhiteSpace(oPrimeRxPayResponse.AmountApproved))
                            {
                                if (Convert.ToDouble(oPrimeRxPayResponse.AmountApproved) > 0 && oPrimeRxPayResponse.Result == "SUCCESS")
                                {
                                    cardType = PccPaymentSvr.SetActualCardType(oPrimeRxPayResponse.CardType);
                                    if (objPaymentTypes == null || objPaymentTypes?.Count == 0)
                                    {
                                        using (var db = new Possql())
                                        {
                                            objPaymentTypes = db.PayTypes.ToList();
                                        }
                                    }
                                    paymentTypes = objPaymentTypes.Where(a => a.PayTypeDesc == cardType).SingleOrDefault();
                                    cardType = paymentTypes.PayTypeID;
                                    #region PRIMEPOS-3186
                                    string profiledID = string.Empty;
                                    bool isFSATransaction = false;
                                    if (oPrimeRxPayResponse.ProfiledID.Contains("|"))
                                    {
                                        string[] vs = oPrimeRxPayResponse.ProfiledID.Split('|');
                                        if (vs.Length > 0)
                                        {
                                            profiledID = vs[0];
                                        }
                                        if (vs.Length > 1)
                                        {
                                            isFSATransaction = vs[1] == "Y";
                                        }
                                    }
                                    if (!string.IsNullOrWhiteSpace(oPrimeRxPayResponse.Expiration))
                                    {
                                        oPrimeRxPayResponse.MaskedCardNo = oPrimeRxPayResponse.MaskedCardNo + "|" + oPrimeRxPayResponse.Expiration;
                                    }

                                    oPosTransPaymentSvr.SetPrimeRxPayPartialTrans(Convert.ToString(Postransrow["TransPayId"]),
                                        oPrimeRxPayResponse.AmountApproved,
                                        oPrimeRxPayResponse.TransactionNo,
                                        cardType,
                                        oPrimeRxPayResponse.MaskedCardNo,
                                        profiledID,
                                        GetLastDateOfMonthAndYear(oPrimeRxPayResponse.Expiration),
                                        isFSATransaction);

                                    #endregion
                                    for (int i = 0; i < oDataSet.Tables[2].Rows.Count; i++)
                                    {
                                        if (Convert.ToString(oDataSet.Tables[2].Rows[i]["TransPayId"]) == Convert.ToString(Postransrow["TransPayId"]))
                                        {
                                            oDataSet.Tables[2].Rows[i]["Approved Amount"] = oPrimeRxPayResponse.AmountApproved;
                                        }
                                    }
                                    this.IsGetTransactionAgain = false;
                                    TotalAmountInPosTrans = GetTotalAmountInPosTrans(Convert.ToString(grdRow.Cells["Trans ID"].Value));

                                    //if (Convert.ToDouble(grdRow.Cells["TotalPaid"].Value) - Convert.ToDouble(TotalAmountInPosTrans) == 0)
                                    //{
                                    //UpdatePostransaction(Convert.ToInt32(grdRow.Cells["Trans Id"].Value));
                                    var TotalPaid = oDataSet.Tables[0].AsEnumerable().Where(r1 =>
                                r1.Field<int>("Trans ID") == Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString()))
                                .Select(g => g.Field<decimal>("TotalPaid")).FirstOrDefault();

                                    pendingAmount = TotalPaid - Convert.ToDecimal(TotalAmountInPosTrans);

                                    this.PendingAmount = pendingAmount;
                                    this.IsPendingPayment = true;
                                    this.Close();
                                    //}
                                    //                else
                                    //                {
                                    //                    //Set one variable to avoid GetTransactionStatus again
                                    //                    var TotalPaid = oDataSet.Tables[0].AsEnumerable().Where(r1 =>
                                    //r1.Field<int>("Trans ID") == Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString()))
                                    //.Select(g => g.Field<decimal>("TotalPaid")).FirstOrDefault();

                                    //                    pendingAmount = TotalPaid - Convert.ToDecimal(TotalAmountInPosTrans);

                                    //                    if (pendingAmount > 0)
                                    //                    {
                                    //                        this.PendingAmount = pendingAmount;
                                    //                        this.IsPendingPayment = true;
                                    //                        this.Close();
                                    //                    }
                                    //                }
                                }
                                else if (oPrimeRxPayResponse.Result.Contains("EXPIRED"))
                                {
                                    oPosTransPaymentSvr.SetPrimeRxPayExpiredTrans(Convert.ToString(Postransrow["TransPayId"]));
                                    retval = true;
                                }
                                else
                                {
                                    //logger.Error(oPrimeRxPayResponse.Result);
                                    this.IsGetTransactionAgain = false;
                                }
                            }
                            else
                            {
                                logger.Error("The Object returned is null");
                            }
                        }
                        else if (Convert.ToString(Postransrow["Status"]).ToUpper() == clsPOSDBConstants.PosTransPayment_Status_Completed.ToUpper())
                        {
                            this.IsGetTransactionAgain = false;
                            TotalAmountInPosTrans = GetTotalAmountInPosTrans(Convert.ToString(grdRow.Cells["Trans ID"].Value));

                            if (Convert.ToDecimal(grdRow.Cells["TotalPaid"].Value) - Convert.ToDecimal(TotalAmountInPosTrans) == 0)
                            {
                                //UpdatePostransaction(Convert.ToInt32(grdRow.Cells["Trans Id"].Value));
                                var TotalPaid = oDataSet.Tables[0].AsEnumerable().Where(r1 =>
                            r1.Field<int>("Trans ID") == Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString()))
                            .Select(g => g.Field<decimal>("TotalPaid")).FirstOrDefault();

                                pendingAmount = TotalPaid - Convert.ToDecimal(TotalAmountInPosTrans);

                                this.PendingAmount = pendingAmount;
                                this.IsPendingPayment = true;
                                this.Close();
                            }
                            else
                            {
                                IsPartialTrans = true;
                            }
                        }
                    }
                }
                else
                {
                    Search(false);
                }
                this.IsGetTransactionAgain = true;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Gettransactiondetail()" + ex);
            }
            finally
            {// Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
            return retval;
        }
        //{
        //    try
        //    {
        //        IsGetTransactionAgain = false;
        //        if (ds?.Tables.Count > 0 && ds?.Tables[0].Rows.Count > 0)
        //        {
        //            DataRow[] PosTransPaymenttbl = null;
        //            PrimeRxPay.PrimeRxPayResponse oPrimeRxPayResponse = new PrimeRxPay.PrimeRxPayResponse();
        //            PrimeRxPay.PrimeRxPayProcessor oPrimeRxPay = PrimeRxPay.PrimeRxPayProcessor.GetInstance();
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                if (Convert.ToBoolean(ds.Tables[0].Rows[i]["IsCustomerDriven"].ToString()))
        //                {
        //                    PosTransPaymenttbl = oDataSet.Tables[2].
        //                        Select("TransID = " + Convert.ToString(ds.Tables[0].Rows[i]["Trans Id"]));

        //                    foreach (var Postransrow in PosTransPaymenttbl)
        //                    {
        //                        if (!string.IsNullOrWhiteSpace(Convert.ToString(Postransrow["PrimeRxPayTransID"])) && Convert.ToString(Postransrow["Status"]).ToUpper() != "COMPLETE")
        //                        {
        //                            oPrimeRxPayResponse = oPrimeRxPay.GetTransactionDetail(Convert.ToString(Postransrow["PrimeRxPayTransID"]), Configuration.CSetting.PayProviderID.ToString());

        //                            if (oPrimeRxPayResponse != null && !string.IsNullOrWhiteSpace(oPrimeRxPayResponse.AmountApproved))
        //                            {
        //                                if (Convert.ToDouble(oPrimeRxPayResponse.AmountApproved) > 0 && oPrimeRxPayResponse.Result == "SUCCESS")
        //                                {
        //                                    string cardType = "";
        //                                    cardType = PccPaymentSvr.SetActualCardType(oPrimeRxPayResponse.CardType);
        //                                    using (var db = new Possql())
        //                                    {
        //                                        PaymentTypes paymentTypes = db.PayTypes.Where(a => a.PayTypeDesc == cardType).SingleOrDefault();
        //                                        cardType = paymentTypes.PayTypeID;
        //                                    }
        //                                    oPosTransPaymentSvr.SetPrimeRxPayPartialTrans(Convert.ToString(Postransrow["TransPayId"]), oPrimeRxPayResponse.AmountApproved, oPrimeRxPayResponse.TransactionNo, cardType);
        //                                    this.IsGetTransactionAgain = false;
        //                                    Search();
        //                                    string TotalAmountInPosTrans = GetTotalAmountInPosTrans(Convert.ToString(ds.Tables[0].Rows[i]["Trans ID"]));

        //                                    if (Convert.ToDouble(ds.Tables[0].Rows[i]["TotalPaid"]) - Convert.ToDouble(TotalAmountInPosTrans) == 0)
        //                                    {
        //                                        UpdatePostransaction(Convert.ToInt32(oDataSet.Tables[0].Rows[i]["Trans Id"]));
        //                                    }
        //                                    else
        //                                    {
        //                                        //Set one variable to avoid GetTransactionStatus again
        //                                        btnPaypending.Visible = true;
        //                                    }
        //                                }
        //                                else if (oPrimeRxPayResponse.Result == "EXPIRED")
        //                                {
        //                                    oPosTransPaymentSvr.SetPrimeRxPayExpiredTrans(Convert.ToString(Postransrow["TransPayId"]));
        //                                }
        //                                else
        //                                {                                            
        //                                    logger.Error(oPrimeRxPayResponse.Result);
        //                                    //Resources.Message.Display(oPrimeRxPayResponse.Result);
        //                                    this.IsGetTransactionAgain = false;
        //                                    btnPaypending.Visible = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                logger.Error("The Object returned is null");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            btnPaypending.Visible = false;
        //                        }
        //                    }
        //                }
        //            }
        //            Search();
        //        }
        //        this.IsGetTransactionAgain = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //    }
        //    return true;
        //}

        private void GetTransactionStatus()
        {
            frmWaitScreen ofrmWait = null;
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;
                //PRIMEPOS-3288
                //ofrmWait = new frmWaitScreen(true, "Please wait...", "Updating Payment Status ...");
                //ofrmWait.StartPosition = FormStartPosition.CenterScreen;
                //ofrmWait.Show();
                PaymentTypes paymentType;

                string ProfiledID = string.Empty;
                bool IsFSATransaction = false;
                string[] profileIdAndFSA;
                IsGetTransactionAgain = false;
                if (oDataSet?.Tables.Count > 0 && oDataSet?.Tables[0].Rows.Count > 0)
                {
                    DataRow[] PosTransPaymenttbl = null;
                    PrimeRxPay.PrimeRxPayResponse oPrimeRxPayResponse = new PrimeRxPay.PrimeRxPayResponse();
                    PrimeRxPay.PrimeRxPayProcessor oPrimeRxPay = PrimeRxPay.PrimeRxPayProcessor.GetInstance();

                    for (int i = 0; i < oDataSet.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (Convert.ToBoolean(oDataSet.Tables[0].Rows[i]["IsCustomerDriven"].ToString()))
                            {
                                PosTransPaymenttbl = oDataSet.Tables[2].Select("TransID = " + Convert.ToString(oDataSet.Tables[0].Rows[i]["Trans Id"]));

                                foreach (var Postransrow in PosTransPaymenttbl)
                                {
                                    if (!string.IsNullOrWhiteSpace(Convert.ToString(Postransrow["PrimeRxPayTransID"]))
                                        && Convert.ToString(Postransrow["Status"]).ToUpper() != clsPOSDBConstants.PosTransPayment_Status_Completed.ToUpper())
                                    {
                                        //PRIMEPOS-3453 Add logic of lookupdays 
                                        TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(Postransrow["TransDate"]);
                                        int lookUpDays = timeSpan.Days + 5;

                                        oPrimeRxPayResponse = oPrimeRxPay.GetTransactionDetail(Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl, Convert.ToString(Postransrow["PrimeRxPayTransID"]), Configuration.CSetting.PayProviderID.ToString(), Configuration.CSetting.PrimeRxPayClientId, Configuration.CSetting.PrimeRxPaySecretKey, lookUpDays);

                                        if (oPrimeRxPayResponse != null && !string.IsNullOrWhiteSpace(oPrimeRxPayResponse.AmountApproved))
                                        {
                                            if (!string.IsNullOrWhiteSpace(oPrimeRxPayResponse.AuthNo))
                                            {
                                                if (!keyValuePairs.ContainsKey(Convert.ToString(Postransrow["PrimeRxPayTransID"])))
                                                    keyValuePairs.Add(Convert.ToString(Postransrow["PrimeRxPayTransID"]),
                                                        Convert.ToString(oPrimeRxPayResponse.AuthNo));
                                            }
                                            if (Convert.ToDouble(oPrimeRxPayResponse.AmountApproved) > 0 && oPrimeRxPayResponse.Result == "SUCCESS")
                                            {
                                                string cardType = PccPaymentSvr.SetActualCardType(oPrimeRxPayResponse.CardType);
                                                if (objPaymentTypes == null || objPaymentTypes?.Count == 0)
                                                {
                                                    using (var db = new Possql())
                                                    {
                                                        objPaymentTypes = db.PayTypes.ToList();
                                                    }
                                                }
                                                paymentType = objPaymentTypes.Where(a => a.PayTypeDesc == cardType).SingleOrDefault();
                                                if (paymentType != null)
                                                {
                                                    cardType = paymentType.PayTypeID;
                                                }

                                                #region PRIMEPOS-3186
                                                ProfiledID = string.Empty;
                                                IsFSATransaction = false;
                                                if (oPrimeRxPayResponse.ProfiledID.Contains("|"))
                                                {
                                                    profileIdAndFSA = oPrimeRxPayResponse.ProfiledID.Split('|');
                                                    if (profileIdAndFSA.Length > 0)
                                                    {
                                                        ProfiledID = profileIdAndFSA[0];
                                                    }
                                                    if (profileIdAndFSA.Length > 1)
                                                    {
                                                        IsFSATransaction = profileIdAndFSA[1] == "Y";
                                                    }
                                                }
                                                if (!string.IsNullOrWhiteSpace(oPrimeRxPayResponse.Expiration))
                                                {
                                                    oPrimeRxPayResponse.MaskedCardNo = oPrimeRxPayResponse.MaskedCardNo + "|" + oPrimeRxPayResponse.Expiration;
                                                }
                                                Postransrow["Status"] = clsPOSDBConstants.PosTransPayment_Status_Completed;
                                                oPosTransPaymentSvr.SetPrimeRxPayPartialTrans(Convert.ToString(Postransrow["TransPayId"]),
                                                oPrimeRxPayResponse.AmountApproved,
                                                oPrimeRxPayResponse.TransactionNo,
                                                cardType,
                                                oPrimeRxPayResponse.MaskedCardNo,
                                                ProfiledID,
                                                GetLastDateOfMonthAndYear(oPrimeRxPayResponse.Expiration),
                                                IsFSATransaction);

                                                #endregion
                                                //this.IsGetTransactionAgain = false;
                                                //Search();
                                                string TotalAmountInPosTrans = GetTotalAmountInPosTrans(Convert.ToString(oDataSet.Tables[0].Rows[i]["Trans ID"]));

                                                if (Convert.ToDouble(oDataSet.Tables[0].Rows[i]["TotalPaid"]) - Convert.ToDouble(TotalAmountInPosTrans) == 0)
                                                {
                                                    //UpdatePostransaction(Convert.ToInt32(oDataSet.Tables[0].Rows[i]["Trans Id"]));

                                                }
                                                else
                                                {
                                                    //Set one variable to avoid GetTransactionStatus again
                                                    //btnPaypending.Visible = true;
                                                }
                                                #region PRIMEPOS-3344
                                                using (var db = new Possql())
                                                {
                                                    CCTransmission_Log cclog = new CCTransmission_Log();
                                                    cclog.TransDateTime = DateTime.Now;
                                                    cclog.TicketNo = Convert.ToString(Postransrow["TicketNo"]);
                                                    cclog.TransAmount =  Convert.ToDecimal(oPrimeRxPayResponse.TransactionAmount);
                                                    cclog.AmtApproved = Convert.ToDecimal(oPrimeRxPayResponse.AmountApproved);
                                                    cclog.TransDataStr = oPrimeRxPayResponse.request;
                                                    cclog.RecDataStr = oPrimeRxPayResponse.response;
                                                    cclog.PaymentProcessor = "PRIMERXPAY";
                                                    cclog.UserID = Configuration.UserName;
                                                    cclog.StationID = Configuration.StationID;
                                                    cclog.TransmissionStatus = "Completed";
                                                    cclog.HostTransID = oPrimeRxPayResponse.EmvReceipt.TransactionID;
                                                    cclog.TransType = "PRIMERXPAY_CREDIT_SALE";
                                                    cclog.ResponseMessage = oPrimeRxPayResponse.Result;
                                                    #region PRIMEPOS-3383
                                                    if (!string.IsNullOrWhiteSpace(oPrimeRxPayResponse.AccountNo) && oPrimeRxPayResponse.AccountNo.Trim().Length >= 4)
                                                    {
                                                        cclog.last4 = oPrimeRxPayResponse.AccountNo.Trim().Substring(oPrimeRxPayResponse.AccountNo.Trim().Length - 4, 4);
                                                    }
                                                    #endregion
                                                    db.CCTransmission_Logs.Add(cclog);
                                                    db.SaveChanges();
                                                    db.Entry(cclog).GetDatabaseValues();
                                                }
                                                #endregion
                                            }
                                            else if (oPrimeRxPayResponse.Result.ToUpper().Contains("EXPIRED"))
                                            {
                                                oPosTransPaymentSvr.SetPrimeRxPayExpiredTrans(Convert.ToString(Postransrow["TransPayId"]));
                                            }
                                            else
                                            {
                                                oPosTransPaymentSvr.SetPrimeRxPayInProgressTrans(Convert.ToString(Postransrow["TransPayId"]));
                                                //logger.Error(oPrimeRxPayResponse.Result);
                                                //Resources.Message.Display(oPrimeRxPayResponse.Result);
                                                this.IsGetTransactionAgain = false;
                                            }
                                        }
                                        else
                                        {
                                            logger.Error($"frmPOSOnHoldTrans===>GetTransactionStatus()===>PrimeRxPay Onhold transaction Status GetTransactionDetail() returned is null for PrimeRxPayTransID={Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"])}");
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Error(Ex, $"frmPOSOnHoldTrans===>GetTransactionStatus()===>Exception occured on PrimeRxPay Onhold transaction Status update for PrimeRxPayTransID={ Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"])}");
                        }
                    }
                    Search(false);
                }
                this.IsGetTransactionAgain = true;
                //ofrmWait.Close();
            }
            catch (Exception ex)
            {

                logger.Error(ex, $"frmPOSOnHoldTrans===>GetTransactionStatus()===>Exception occured on PrimeRxPay Onhold transaction Status update");
            }
            finally
            {
                //PRIMEPOS-3288
                //if (Application.OpenForms.OfType<frmWaitScreen>().Any())
                //{
                //    ofrmWait.Close();
                //}
                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }
        }

        #region PRIMEPOS-3333
        private void GetMultipleTransactionStatus()
        {
            try
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                PaymentTypes paymentType;
                DateTime paymentDate = DateTime.Now; //PRIMEPOS-3453
                string ProfiledID = string.Empty;
                bool IsFSATransaction = false;
                string[] profileIdAndFSA;
                IsGetTransactionAgain = false;
                List<long> PrimeRxPayTransID = new List<long>(); //PRIMEPOS-3333
                if (oDataSet?.Tables.Count > 0 && oDataSet?.Tables[0].Rows.Count > 0)
                {
                    DataRow[] PosTransPaymenttblToGetID = null;
                    List<PrimeRxPay.PrimeRxPayResponse> oPrimeRxPayResponses = new List<PrimeRxPay.PrimeRxPayResponse>();
                    PrimeRxPay.PrimeRxPayProcessor oPrimeRxPay = PrimeRxPay.PrimeRxPayProcessor.GetInstance();

                    //PRIMEPOS-3333 Get all ids of all the transactions that status is pending
                    for (int i = 0; i < oDataSet.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(oDataSet.Tables[0].Rows[i]["IsCustomerDriven"].ToString()))
                        {
                            PosTransPaymenttblToGetID = oDataSet.Tables[2].Select("TransID = " + Convert.ToString(oDataSet.Tables[0].Rows[i]["Trans Id"]));

                            #region PRIMEPOS-3333
                            foreach (var Postransrow in PosTransPaymenttblToGetID) //PRIMEPOS-3333
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(Postransrow["PrimeRxPayTransID"]))
                                        && Convert.ToString(Postransrow["Status"]).ToUpper() != clsPOSDBConstants.PosTransPayment_Status_Completed.ToUpper())
                                {
                                    PrimeRxPayTransID.Add(Convert.ToInt32(Postransrow["PrimeRxPayTransID"])); //PRIMEPOS-3333

                                    if(paymentDate > Convert.ToDateTime(Postransrow["TransDate"])) //PRIMEPOS-3453
                                    {
                                        paymentDate = Convert.ToDateTime(Postransrow["TransDate"]); //PRIMEPOS-3453
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    if(PrimeRxPayTransID!=null && PrimeRxPayTransID.Count>0)
                    {
                        //Calculate lookup days
                        TimeSpan timeSpan = DateTime.Now - paymentDate;
                        int lookUpDays = Convert.ToInt32(timeSpan.Days) + 5;
                        //PRIMEPOS-3333 Call PrimeRxPay-API
                        oPrimeRxPayResponses = oPrimeRxPay.GetMultipleTransactionDetail(Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl, PrimeRxPayTransID, Configuration.CSetting.PayProviderID, Configuration.CSetting.PrimeRxPayClientId, Configuration.CSetting.PrimeRxPaySecretKey, lookUpDays);
                    }

                    //PRIMEPOS-3333 Traverse the loop and update the status of that transactions that response is receive.
                    DataRow[] PosTransPaymenttbl = null;
                    for (int i = 0; i < oDataSet.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (Convert.ToBoolean(oDataSet.Tables[0].Rows[i]["IsCustomerDriven"].ToString()))
                            {
                                PosTransPaymenttbl = oDataSet.Tables[2].Select("TransID = " + Convert.ToString(oDataSet.Tables[0].Rows[i]["Trans Id"]));

                                foreach (var Postransrow in PosTransPaymenttbl)
                                {
                                    if (!string.IsNullOrWhiteSpace(Convert.ToString(Postransrow["PrimeRxPayTransID"]))
                                        && Convert.ToString(Postransrow["Status"]).ToUpper() != clsPOSDBConstants.PosTransPayment_Status_Completed.ToUpper())
                                    {
                                        if(oPrimeRxPayResponses!=null && oPrimeRxPayResponses.Count>0)
                                        {
                                            foreach(var oPrimeRxResp in oPrimeRxPayResponses)
                                            {
                                                if(oPrimeRxResp?.TransactionID == Convert.ToString(Postransrow["PrimeRxPayTransID"]))
                                                {
                                                    if(oPrimeRxResp != null && !string.IsNullOrWhiteSpace(oPrimeRxResp.AmountApproved))
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(oPrimeRxResp.AuthNo))
                                                        {
                                                            if (!keyValuePairs.ContainsKey(Convert.ToString(Postransrow["PrimeRxPayTransID"])))
                                                                keyValuePairs.Add(Convert.ToString(Postransrow["PrimeRxPayTransID"]),
                                                                    Convert.ToString(oPrimeRxResp.AuthNo));
                                                        }
                                                        if (Convert.ToDouble(oPrimeRxResp.AmountApproved) > 0 && oPrimeRxResp.Result == "SUCCESS")
                                                        {
                                                            string cardType = PccPaymentSvr.SetActualCardType(oPrimeRxResp.CardType);
                                                            if (objPaymentTypes == null || objPaymentTypes?.Count == 0)
                                                            {
                                                                using (var db = new Possql())
                                                                {
                                                                    objPaymentTypes = db.PayTypes.ToList();
                                                                }
                                                            }
                                                            paymentType = objPaymentTypes.Where(a => a.PayTypeDesc == cardType).SingleOrDefault();
                                                            if (paymentType != null)
                                                            {
                                                                cardType = paymentType.PayTypeID;
                                                            }
                                                            #region PRIMEPOS-3186
                                                            ProfiledID = string.Empty;
                                                            IsFSATransaction = false;
                                                            if (oPrimeRxResp.ProfiledID.Contains("|"))
                                                            {
                                                                profileIdAndFSA = oPrimeRxResp.ProfiledID.Split('|');
                                                                if (profileIdAndFSA.Length > 0)
                                                                {
                                                                    ProfiledID = profileIdAndFSA[0];
                                                                }
                                                                if (profileIdAndFSA.Length > 1)
                                                                {
                                                                    IsFSATransaction = profileIdAndFSA[1] == "Y";
                                                                }
                                                            }
                                                            if (!string.IsNullOrWhiteSpace(oPrimeRxResp.Expiration))
                                                            {
                                                                oPrimeRxResp.MaskedCardNo = oPrimeRxResp.MaskedCardNo + "|" + oPrimeRxResp.Expiration;
                                                            }
                                                            Postransrow["Status"] = clsPOSDBConstants.PosTransPayment_Status_Completed;
                                                            oPosTransPaymentSvr.SetPrimeRxPayPartialTrans(Convert.ToString(Postransrow["TransPayId"]),
                                                            oPrimeRxResp.AmountApproved,
                                                            oPrimeRxResp.TransactionNo,
                                                            cardType,
                                                            oPrimeRxResp.MaskedCardNo,
                                                            ProfiledID,
                                                            GetLastDateOfMonthAndYear(oPrimeRxResp.Expiration),
                                                            IsFSATransaction);
                                                            #endregion

                                                            #region PRIMEPOS-3344
                                                            using (var db = new Possql())
                                                            {
                                                                CCTransmission_Log cclog = new CCTransmission_Log();
                                                                cclog.TransDateTime = DateTime.Now;
                                                                cclog.TicketNo = Convert.ToString(Postransrow["TicketNo"]);
                                                                cclog.TransAmount = Convert.ToDecimal(oPrimeRxResp.TransactionAmount);
                                                                cclog.AmtApproved = Convert.ToDecimal(oPrimeRxResp.AmountApproved);
                                                                cclog.TransDataStr = oPrimeRxResp.request;
                                                                cclog.RecDataStr = oPrimeRxResp.response;
                                                                cclog.PaymentProcessor = "PRIMERXPAY";
                                                                cclog.UserID = Configuration.UserName;
                                                                cclog.StationID = Configuration.StationID;
                                                                cclog.TransmissionStatus = "Completed";
                                                                cclog.HostTransID = oPrimeRxResp.EmvReceipt.TransactionID;
                                                                cclog.TransType = "PRIMERXPAY_CREDIT_SALE";
                                                                cclog.ResponseMessage = oPrimeRxResp.Result;
                                                                #region PRIMEPOS-3383
                                                                if (!string.IsNullOrWhiteSpace(oPrimeRxResp.AccountNo) && oPrimeRxResp.AccountNo.Trim().Length >= 4)
                                                                {
                                                                    cclog.last4 = oPrimeRxResp.AccountNo.Trim().Substring(oPrimeRxResp.AccountNo.Trim().Length - 4, 4);
                                                                }
                                                                #endregion
                                                                db.CCTransmission_Logs.Add(cclog);
                                                                db.SaveChanges();
                                                                db.Entry(cclog).GetDatabaseValues();
                                                            }
                                                            #endregion

                                                        }
                                                        else if (oPrimeRxResp.Result.ToUpper().Contains("EXPIRED"))
                                                        {
                                                            oPosTransPaymentSvr.SetPrimeRxPayExpiredTrans(Convert.ToString(Postransrow["TransPayId"]));
                                                        }
                                                        else
                                                        {
                                                            oPosTransPaymentSvr.SetPrimeRxPayInProgressTrans(Convert.ToString(Postransrow["TransPayId"]));
                                                            this.IsGetTransactionAgain = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        logger.Error($"frmPOSOnHoldTrans===>GetMultipleTransactionStatus()===>PrimeRxPay Onhold transaction Status GetTransactionDetail() returned is null for PrimeRxPayTransID={Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"])}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Error(Ex, $"frmPOSOnHoldTrans===>GetMultipleTransactionStatus()===>Exception occured on PrimeRxPay Onhold transaction Status update for PrimeRxPayTransID={Convert.ToString(oDataSet.Tables[0].Rows[i]["PrimeRxPayTransID"])}");
                        }
                    }
                    Search(false);
                }
                this.IsGetTransactionAgain = true;
            }
            catch (Exception ex)
            {

                logger.Error(ex, $"frmPOSOnHoldTrans===>GetMultipleTransactionStatus()===>Exception occured on PrimeRxPay Onhold transaction Status update");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        private static bool IsFormOpen<T>(T myForm) where T : Form
        {
            return Application.OpenForms.OfType<T>().Any();
        }
        private void UpdatePostransaction(int TransId)
        {
            try
            {
                TransHeaderData oHoldTransHData = oPOSTrans.GetOnHoldTransData(TransId, out oHoldTransDData);
                oPOSTrans.oTransHRow = oHoldTransHData.TransHeader[0];
                TransDetailRXData oTransDetailRxData = oTransDetailRxSvr.PopulateData(TransId);
                TransDetailTaxData oTDTaxData = oTDTaxSvr.PopulateData(TransId);
                POSTransPaymentData oPosTransPaymentData = oPosTransPaymentSvr.PopulateOnHold(TransId);

                oPOSTrans.Persist(oHoldTransHData, oHoldTransDData, oPosTransPaymentData, TransId, false, oPOSTrans.oRXHeaderList, oTransDetailRxData, oTDTaxData);

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        private void grdSearch_CellChange(object sender, CellEventArgs e)
        {

        }

        //private void btnPaypending_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (this.grdSearch.ActiveRow == null)
        //            return;

        //        if (grdSearch.ActiveRow.Band.Index > 0)
        //        {
        //            this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
        //        }

        //        TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());

        //        Decimal Amounts = Convert.ToDecimal(GetTotalAmountInPosTrans(grdSearch.ActiveRow.Cells[0].Text.ToString()));

        //        if (Amounts > 0)
        //        {
        //            var TotalPaid = oDataSet.Tables[0].AsEnumerable().Where(r1 =>
        //            r1.Field<int>("Trans ID") == Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString()))
        //            .Select(g => g.Field<decimal>("TotalPaid")).FirstOrDefault();

        //            decimal PendingAmount = TotalPaid - Amounts;

        //            if (PendingAmount > 0)
        //            {
        //                this.PendingAmount = PendingAmount;
        //                this.IsPendingPayment = true;
        //                this.Close();
        //            }
        //        }

        //        //this.IsCanceled = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex);
        //    }

        //}

        private void grdSearch_ClickCell(object sender, ClickCellEventArgs e)
        {

            //            try
            //            {
            //                var ApprovedAmount = from row in oDataSet.Tables[2].AsEnumerable()
            //                                     where row.Field<int>("TransID") == Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString())
            //                                     select row.Field<decimal>("Approved Amount");

            //                Decimal PendingAmount = 0;
            //                foreach (var Amount in ApprovedAmount)
            //                {
            //                    PendingAmount += Amount;
            //                }

            //                //if (PendingAmount > 0)
            //                //    btnPaypending.Visible = true;
            //                //else
            //                //    btnPaypending.Visible = false;

            //                var Transac = from row in oDataSet.Tables[2].AsEnumerable()
            //                              where row.Field<int>("TransID") == Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString())
            //&& row.Field<string>("Status") == "Expired"
            //                              select row;

            //            }
            //            catch (Exception ex)
            //            {
            //                logger.Error(ex, "grdSearch_ClickCell");
            //            }
        }

        private string GetTotalAmountInPosTrans(string TransID)
        {
            try
            {
                var ApprovedAmount = from row in oDataSet.Tables[2].AsEnumerable()
                                     where row.Field<int>("TransID") == Convert.ToInt32(TransID)
                                     select row.Field<decimal>("Approved Amount");

                decimal TotalAmounts = 0;
                foreach (var Amount in ApprovedAmount)
                {
                    TotalAmounts += Amount;
                }

                return TotalAmounts.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "GetTotalAmountInPosTrans");
                return 0.ToString();
            }
        }

        //private void btnCancelTrans_Click(object sender, EventArgs e)
        //{
        //    if (this.grdSearch.ActiveRow == null)
        //        return;

        //    if (grdSearch.ActiveRow.Band.Index > 0)
        //    {
        //        this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
        //    }

        //    TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());

        //    //DataRow[] PosTransPaymenttbl = oDataSet.Tables[2].
        //    //                    Select("TransID = " + TransID);

        //    //foreach (DataRow datarow in PosTransPaymenttbl)
        //    //{
        //    //    if (RemovePrimeRxPay(datarow))
        //    //    {
        //    //        oDataSet.Tables[2].Rows.Remove(datarow);
        //    //    }
        //    //    //oPosTransPaymentSvr.SetPrimeRxPayCanceledTrans(Convert.ToString(dr["TransPayID"]));
        //    //}
        //    Search();
        //}
        private bool RemovePrimeRxPay(DataRow dr)
        {
            bool retVal = false;
            string tempPayType = string.Empty; //PRIMEPOS-3247
            string strPaymentType = dr[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString();
            switch (strPaymentType?.Trim())
            {
                case "1":
                case "2":
                case "C":
                case "H":
                    retVal = true;
                    break;
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "-99":
                case "E":
                case "O":
                    #region PRIMEPOS-3283
                    if (strPaymentType?.Trim() == "3" || strPaymentType?.Trim() == "4" || strPaymentType?.Trim() == "5" || strPaymentType?.Trim() == "6" || strPaymentType?.Trim() == "-99")
                    {
                        tempPayType = "CC";
                    }
                    else if (strPaymentType?.Trim() == "7")
                    {
                        tempPayType = "DB";
                    }
                    else if (strPaymentType?.Trim() == "E")
                    {
                        tempPayType = "BT";
                    }
                    #endregion
                    PccCardInfo oPci = new PccCardInfo();
                    oPci.PaymentProcessor = Convert.ToString(dr["PaymentProcessor"]);
                    PccPaymentSvr objPccPmt = null;
                    if (oPci.PaymentProcessor != "WORLDPAY")
                    {
                        if (oPci.PaymentProcessor == "PRIMERXPAY")
                        {
                            objPccPmt = new PccPaymentSvr(clsPOSDBConstants.PRIMERXPAY);
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["ProcessorTransID"])))
                                oPci.TransactionID = dr["ProcessorTransID"].ToString().Trim();// TransactionID
                            else
                            {
                                oPci.IsVoidCustomerDriven = true;
                                oPci.TransactionID = dr["PrimeRxPayTransID"].ToString().Trim();// TransactionID
                            }
                        }
                        else
                        {
                            objPccPmt = new PccPaymentSvr();
                            oPci.tRoutId = oPci.TransactionID = Convert.ToString(dr["ProcessorTransID"]).Trim();
                        }
                    }
                    else
                    {
                        objPccPmt = new PccPaymentSvr();
                        if (dr["TicketNo"].ToString() != "")
                        {
                            oPci.TicketNo = dr["TicketNo"].ToString().Trim();
                        }
                    }
                    oPci.transAmount = dr["Amount"].ToString().Trim();
                    String responseStatus = null;
                    #region PRIMEPOS-3373
                    if (oPci.PaymentProcessor.ToUpper() == "NB_VANTIV") //PRIMEPOS-3482
                    {
                        oPci.PaymentProcessor = "VANTIV";
                        PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).PerformVoidOnNBSCardSales(Configuration.StationID + clsUIHelper.GetRandomNo().ToString(), ref oPci);
                        responseStatus = PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).ResponseStatus;
                        if (responseStatus.ToUpper().Trim() == "SUCCESS")
                        {
                            NBSProcessor nbsProc = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                            VoidData voidData = nbsProc.VoidRequest(dr[clsPOSDBConstants.POSTransPayment_Fld_NBSTransId].ToString());
                            if (voidData != null && voidData.Response.Code== "000")
                            {
                                logger.Trace("The NBS transaction void successfully.");
                            }
                            else
                            {
                                logger.Trace("The error is occurring while attempting to retrieve the response for the NBSVoid request");
                            }
                        }
                    }
                    #endregion
                    else
                    {
                        //PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).PerformVoidOnCreditCardSales(Configuration.StationID + clsUIHelper.GetRandomNo().ToString(), ref oPci);
                        if (tempPayType == PayTypes.CreditCard) //PRIMEPOS-3247
                        {
                            PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).PerformVoidOnCreditCardSales(Configuration.StationID + clsUIHelper.GetRandomNo().ToString(), ref oPci);
                        }
                        else if (tempPayType == PayTypes.DebitCard) //PRIMEPOS-3247
                        {
                            if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON)
                            {
                                Resources.Message.Display("You cannot void a Debit transaction.");
                                return false;
                            }
                            else
                            {
                                PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).PerformVoidOnDebitCardSales(Configuration.StationID + clsUIHelper.GetRandomNo().ToString(), ref oPci);
                            }
                        }
                        else if (tempPayType == PayTypes.EBT) //PRIMEPOS-3247
                        {
                            if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                            {
                                Resources.Message.Display(" EBT cannot be voided.");
                            }
                            if (Configuration.CPOSSet.PaymentProcessor == "ELAVON")
                            {
                                PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).IsElavonEbtvoid = true;
                            }
                            PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).PerformEBTReturn(Configuration.StationID + clsUIHelper.GetRandomNo().ToString(), ref oPci);
                            PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).IsElavonEbtvoid = false;
                        }
                        else
                        {
                            PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).PerformVoidOnCreditCardSales(Configuration.StationID + clsUIHelper.GetRandomNo().ToString(), ref oPci);
                        }
                        responseStatus = PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).ResponseStatus;
                    }

                    if (responseStatus.ToUpper().Trim() == "SUCCESS")
                    {
                        retVal = true;
                    }
                    else
                    {
                        if (oPci.PaymentProcessor == "PRIMERXPAY")
                        {
                            oPci.IsVoidCustomerDriven = false;
                            oPci.IsVoidPayComplete = true;
                            PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).PerformVoidOnCreditCardSales(Configuration.StationID + clsUIHelper.GetRandomNo().ToString(), ref oPci);
                            string status = PccPaymentSvr.GetProcessorInstance(oPci.PaymentProcessor).ResponseStatus;
                            if (status.ToUpper().Trim() == "SUCCESS")
                            {
                                retVal = true;
                            }
                        }
                        retVal = false;
                    }
                    break;
                default:
                    retVal = true;
                    break;
            }
            return retVal;
        }

        private Nullable<DateTime> GetLastDateOfMonthAndYear(string expiryDate)
        {
            int noOfDaysInMonth;
            int year = 0;
            int month = 0;

            try
            {
                if (!string.IsNullOrWhiteSpace(expiryDate) && expiryDate.Length == 4)
                {
                    month = Convert.ToInt32(expiryDate.Substring(0, 2));
                    year = Convert.ToInt32(expiryDate.Substring(2, 2));
                }
                if (month > 0 && year > 0)
                {
                    noOfDaysInMonth = DateTime.DaysInMonth(year, month);
                    return Convert.ToDateTime($"{month}/{noOfDaysInMonth}/{year}");
                }
                return null;
            }
            catch
            {
                if (month > 0 && year > 0)
                {
                    noOfDaysInMonth = 28;
                    return Convert.ToDateTime($"{month}/{noOfDaysInMonth}/{year}");
                }
                return null;
            }
        }

        private void btnResendLink_Click(object sender, EventArgs e)
        {
            if (this.grdSearch.ActiveRow == null)
                return;

            try
            {
                if (grdSearch.ActiveRow.Band.Index > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
                }

                logger.Trace("Entered in resendlink button");

                TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());

                if (!Convert.ToBoolean(grdSearch.ActiveRow.Cells["IsCustomerDriven"].Text.ToString()))
                {
                    logger.Trace("The selected transaction is not Customer Driven");
                    return;
                }

                //PRIMEPOS-3247
                //Get the all payment details of the transaction
                //DataSet ds = new DataSet();
                DataTable PosTransPaymentDetail = GetAllPaymentDetails(Convert.ToString(TransID));

                DataRow[] PosTransPaymenttbl = PosTransPaymentDetail.Select("ISNULL(PrimeRxPayTransID,'')<>''");

                //DataRow[] PosTransPaymenttbl = oDataSet.Tables[2].Select("TransID = " + TransID + "AND ISNULL(PrimeRxPayTransID,'')<>''");

                bool isAllTransactionCompleted = false;
                foreach (var row in PosTransPaymenttbl)
                {
                    if (row["Status"].ToString() == clsPOSDBConstants.PosTransPayment_Status_Completed)
                    {
                        isAllTransactionCompleted = true;
                    }
                    else
                    {
                        isAllTransactionCompleted = false;
                        break;
                    }
                }
                if (isAllTransactionCompleted)
                {
                    MessageBox.Show($"All payment status are completed can not re-send link.", "Onhold Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GetTransactionDetail(grdSearch.ActiveRow);



                frmCustomerDetailsPrimeRxPay customerDetailsPrimeRxPay = new frmCustomerDetailsPrimeRxPay(true);
                Dictionary<string, string> fields = new Dictionary<string, string>();

                customerDetailsPrimeRxPay.IsSelectedCustomer = true;
                string Email = string.Empty;
                string Mobile = string.Empty;

                foreach (var row in PosTransPaymenttbl)
                {
                    if (row["Status"].ToString() == clsPOSDBConstants.PosTransPayment_Status_InProgress || row["Status"].ToString() == clsPOSDBConstants.PosTransPayment_Status_Expired)
                    {
                        customerDetailsPrimeRxPay.PosTranspaymentEmail = row["Email"].ToString();
                        customerDetailsPrimeRxPay.PosTranspaymentMobile = row["Mobile"].ToString();
                        fields.Add("TRANSID", row["PrimeRxPayTransID"].ToString());
                        break;
                    }
                }
                customerDetailsPrimeRxPay.CustomerCode = Convert.ToString(grdSearch.ActiveRow.Cells["CustomerID"].Text);

                customerDetailsPrimeRxPay.ShowDialog();
                if (customerDetailsPrimeRxPay.IsCancel)
                {
                    logger.Trace("Canceled from FrmPosOnHold screen ");
                    return;
                }
                else if (customerDetailsPrimeRxPay.IsCustomerDriven)
                {
                    logger.Trace("Setting values on FrmPosOnHold Screen ");
                    fields.Add("TransactionProcessingMode", customerDetailsPrimeRxPay.IsEmail ? "2" : "1");
                    fields.Add("Email", customerDetailsPrimeRxPay.Email);
                    fields.Add("Mobile", customerDetailsPrimeRxPay.Mobile);
                    fields.Add("Name", customerDetailsPrimeRxPay.Name);
                    fields.Add("DOB", customerDetailsPrimeRxPay.DOB);
                    fields.Add("URL", Configuration.CSetting.PrimeRxPayUrl + Configuration.CSetting.PrimerxPayExtensionUrl);
                    fields.Add("APIKEY", Configuration.CSetting.PrimeRxPayClientId);
                    fields.Add("PASSWORD", Configuration.CSetting.PrimeRxPaySecretKey);
                    fields.Add("LINKEXPIRY", Configuration.CSetting.LinkExpriyInMinutes);//PRIMEPOS-3134
                    fields.Add("PHARMACYNO", Configuration.CSetting.PharmacyNPI);//PRIMEPOS-3134
                    //onlineProcessCC.DOB = customerDetailsPrimeRxPay.dob                
                    //oPosPTList.oCurrentCustRow = customerDetailsPrimeRxPay.oCustData?.Customer[0];                
                }

                PrimeRxPay.PrimeRxPayProcessor oPrimeRxPay = PrimeRxPay.PrimeRxPayProcessor.GetInstance();
                string Response = string.Empty;

                foreach (var row in PosTransPaymenttbl)
                {
                    if (row["Status"].ToString() == clsPOSDBConstants.PosTransPayment_Status_InProgress || row["Status"].ToString() == clsPOSDBConstants.PosTransPayment_Status_Expired)
                    {
                        logger.Trace("Sending resend link API");
                        fields["TRANSID"] = row["PrimeRxPayTransID"].ToString();
                        Response = oPrimeRxPay.ResendLink(fields);

                        if (Response.Contains("Payment request sent successfully"))
                        {
                            row["Status"] = clsPOSDBConstants.PosTransPayment_Status_InProgress;
                            oPosTransPaymentSvr.SetPrimeRxPayInProgressTrans(Convert.ToString(row["TransPayId"]));
                        }

                        logger.Trace("Message from ResendLink for PrimeRxPayTransID = " + row["PrimeRxPayTransID"].ToString() + " : " + Response);
                        clsUIHelper.ShowOKMsg(Response);
                    }
                }
                //this.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Error in ResendLink " + ex.ToString());
            }
        }
    }
}
