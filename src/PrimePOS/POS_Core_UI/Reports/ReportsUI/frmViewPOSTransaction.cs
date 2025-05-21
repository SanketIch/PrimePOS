using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using NLog;
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.LabelHandler.RxLabel;
using PharmData;
using POS_Core.Resources.DelegateHandler;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmViewPOSTransaction : System.Windows.Forms.Form
    {
        public string SearchTable = "";
        public bool IsCanceled = true;
        public bool DisplayRecordAtStartup = false;
        private SearchSvr oSearchSvr = new SearchSvr();
        private DataSet oDataSet = new DataSet();
        public bool isReadonly = false;
        private int CurrentX;
        private int CurrentY;
        private bool GridVisibleFlag = false;

        public string FormCaption = "";
        public string LabelText1 = "";
        public string LabelText2 = "";
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabMainControl;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lbl2;
        private Infragistics.Win.Misc.UltraLabel lbl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private CheckBox chkCCTransOnly;
        private CheckBox chkIIASTransactionsOnly;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCustomer;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboStationId;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransID;
        private CheckBox chkSrchByTransID;
        private Infragistics.Win.Misc.UltraGroupBox groupBoxTransType;
        private CheckBox chkROA;
        private CheckBox chkReturn;
        private CheckBox chkSale;
        private CheckBox chkAll;
        private Infragistics.Win.Misc.UltraButton btnPayTypeList;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPaymentType;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private GroupBox grpPayTypeList;
        private CheckBox chkSelectAll;
        private DataGridView dataGridList;
        private DataGridViewCheckBoxColumn chkBox;
        public Infragistics.Win.Misc.UltraLabel lblCustomerName;
        private Infragistics.Win.Misc.UltraButton btnEmailInvoice;
        private CheckBox chkOnhold;
        private IContainer components;
        private Infragistics.Win.Misc.UltraLabel lblLast4DigitsOfCC;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLast4DigitsOfCC;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCloseStationId;
        private Infragistics.Win.Misc.UltraLabel lblCloseStationId;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTrans;
        private Infragistics.Win.Misc.UltraLabel lblTransactions;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtEODId;
        private Infragistics.Win.Misc.UltraLabel lblEODId;
        private Infragistics.Win.Misc.UltraButton btnReset;
        private Infragistics.Win.Misc.UltraLabel lblAmountTo;
        private Infragistics.Win.Misc.UltraLabel lblAmountFrom;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAmountTo;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAmountFrom;
        private CheckBox chkConsiderPaidAmount;
        private Infragistics.Win.Misc.UltraButton btnPrintGiftReceipt;
        private CheckBox chkControlDrugs;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmViewPOSTransaction()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //              
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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            this.tabMainControl = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chkControlDrugs = new System.Windows.Forms.CheckBox();
            this.chkConsiderPaidAmount = new System.Windows.Forms.CheckBox();
            this.lblAmountTo = new Infragistics.Win.Misc.UltraLabel();
            this.lblAmountFrom = new Infragistics.Win.Misc.UltraLabel();
            this.numAmountTo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numAmountFrom = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnReset = new Infragistics.Win.Misc.UltraButton();
            this.txtEODId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblEODId = new Infragistics.Win.Misc.UltraLabel();
            this.cboTrans = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblTransactions = new Infragistics.Win.Misc.UltraLabel();
            this.txtCloseStationId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblCloseStationId = new Infragistics.Win.Misc.UltraLabel();
            this.groupBoxTransType = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chkROA = new System.Windows.Forms.CheckBox();
            this.chkReturn = new System.Windows.Forms.CheckBox();
            this.chkSale = new System.Windows.Forms.CheckBox();
            this.txtLast4DigitsOfCC = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblLast4DigitsOfCC = new Infragistics.Win.Misc.UltraLabel();
            this.chkOnhold = new System.Windows.Forms.CheckBox();
            this.btnPayTypeList = new Infragistics.Win.Misc.UltraButton();
            this.grpPayTypeList = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.chkBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.txtPaymentType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.chkSrchByTransID = new System.Windows.Forms.CheckBox();
            this.txtTransID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cboStationId = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.chkIIASTransactionsOnly = new System.Windows.Forms.CheckBox();
            this.chkCCTransOnly = new System.Windows.Forms.CheckBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.clTo = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnEmailInvoice = new Infragistics.Win.Misc.UltraButton();
            this.btnPrintGiftReceipt = new Infragistics.Win.Misc.UltraButton();
            this.tabMainControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseStationId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxTransType)).BeginInit();
            this.groupBoxTransType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLast4DigitsOfCC)).BeginInit();
            this.grpPayTypeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStationId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMainControl
            // 
            this.tabMainControl.Controls.Add(this.chkControlDrugs);
            this.tabMainControl.Controls.Add(this.chkConsiderPaidAmount);
            this.tabMainControl.Controls.Add(this.lblAmountTo);
            this.tabMainControl.Controls.Add(this.lblAmountFrom);
            this.tabMainControl.Controls.Add(this.numAmountTo);
            this.tabMainControl.Controls.Add(this.numAmountFrom);
            this.tabMainControl.Controls.Add(this.btnReset);
            this.tabMainControl.Controls.Add(this.txtEODId);
            this.tabMainControl.Controls.Add(this.lblEODId);
            this.tabMainControl.Controls.Add(this.cboTrans);
            this.tabMainControl.Controls.Add(this.lblTransactions);
            this.tabMainControl.Controls.Add(this.txtCloseStationId);
            this.tabMainControl.Controls.Add(this.lblCloseStationId);
            this.tabMainControl.Controls.Add(this.groupBoxTransType);
            this.tabMainControl.Controls.Add(this.txtLast4DigitsOfCC);
            this.tabMainControl.Controls.Add(this.lblLast4DigitsOfCC);
            this.tabMainControl.Controls.Add(this.chkOnhold);
            this.tabMainControl.Controls.Add(this.btnPayTypeList);
            this.tabMainControl.Controls.Add(this.grpPayTypeList);
            this.tabMainControl.Controls.Add(this.btnSearch);
            this.tabMainControl.Controls.Add(this.lblCustomerName);
            this.tabMainControl.Controls.Add(this.txtPaymentType);
            this.tabMainControl.Controls.Add(this.grdSearch);
            this.tabMainControl.Controls.Add(this.chkSrchByTransID);
            this.tabMainControl.Controls.Add(this.txtTransID);
            this.tabMainControl.Controls.Add(this.cboStationId);
            this.tabMainControl.Controls.Add(this.txtCustomer);
            this.tabMainControl.Controls.Add(this.ultraLabel2);
            this.tabMainControl.Controls.Add(this.chkIIASTransactionsOnly);
            this.tabMainControl.Controls.Add(this.chkCCTransOnly);
            this.tabMainControl.Controls.Add(this.ultraLabel1);
            this.tabMainControl.Controls.Add(this.txtUserID);
            this.tabMainControl.Controls.Add(this.ultraLabel21);
            this.tabMainControl.Controls.Add(this.clTo);
            this.tabMainControl.Controls.Add(this.clFrom);
            this.tabMainControl.Controls.Add(this.lbl2);
            this.tabMainControl.Controls.Add(this.lbl1);
            this.tabMainControl.Location = new System.Drawing.Point(2, 22);
            this.tabMainControl.Name = "tabMainControl";
            this.tabMainControl.Size = new System.Drawing.Size(1222, 491);
            // 
            // chkControlDrugs
            // 
            this.chkControlDrugs.AutoSize = true;
            this.chkControlDrugs.Location = new System.Drawing.Point(435, 60);
            this.chkControlDrugs.Name = "chkControlDrugs";
            this.chkControlDrugs.Size = new System.Drawing.Size(113, 18);
            this.chkControlDrugs.TabIndex = 14;
            this.chkControlDrugs.Text = "Control Drugs";
            this.chkControlDrugs.UseVisualStyleBackColor = true;
            // 
            // chkConsiderPaidAmount
            // 
            this.chkConsiderPaidAmount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkConsiderPaidAmount.Location = new System.Drawing.Point(7, 123);
            this.chkConsiderPaidAmount.Name = "chkConsiderPaidAmount";
            this.chkConsiderPaidAmount.Size = new System.Drawing.Size(218, 18);
            this.chkConsiderPaidAmount.TabIndex = 4;
            this.chkConsiderPaidAmount.Text = "Consider Paid Amount";
            this.chkConsiderPaidAmount.UseVisualStyleBackColor = true;
            this.chkConsiderPaidAmount.CheckedChanged += new System.EventHandler(this.chkConsiderPaidAmount_CheckedChanged);
            // 
            // lblAmountTo
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.lblAmountTo.Appearance = appearance1;
            this.lblAmountTo.AutoSize = true;
            this.lblAmountTo.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblAmountTo.Location = new System.Drawing.Point(437, 123);
            this.lblAmountTo.Name = "lblAmountTo";
            this.lblAmountTo.Size = new System.Drawing.Size(20, 17);
            this.lblAmountTo.TabIndex = 57;
            this.lblAmountTo.Text = "To";
            // 
            // lblAmountFrom
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.lblAmountFrom.Appearance = appearance2;
            this.lblAmountFrom.AutoSize = true;
            this.lblAmountFrom.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblAmountFrom.Location = new System.Drawing.Point(229, 123);
            this.lblAmountFrom.Name = "lblAmountFrom";
            this.lblAmountFrom.Size = new System.Drawing.Size(36, 17);
            this.lblAmountFrom.TabIndex = 56;
            this.lblAmountFrom.Text = "From";
            // 
            // numAmountTo
            // 
            this.numAmountTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numAmountTo.Enabled = false;
            this.numAmountTo.FormatString = "";
            this.numAmountTo.Location = new System.Drawing.Point(523, 120);
            this.numAmountTo.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountTo.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountTo.MaskInput = "nnnnnnnnnnnnnnn";
            this.numAmountTo.MaxValue = 99999999D;
            this.numAmountTo.MinValue = -99999999D;
            this.numAmountTo.Name = "numAmountTo";
            this.numAmountTo.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAmountTo.Size = new System.Drawing.Size(116, 22);
            this.numAmountTo.TabIndex = 6;
            // 
            // numAmountFrom
            // 
            this.numAmountFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numAmountFrom.Enabled = false;
            this.numAmountFrom.FormatString = "";
            this.numAmountFrom.Location = new System.Drawing.Point(317, 120);
            this.numAmountFrom.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountFrom.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountFrom.MaskInput = "nnnnnnnnnnnnnnn";
            this.numAmountFrom.MaxValue = 99999999D;
            this.numAmountFrom.MinValue = -99999999D;
            this.numAmountFrom.Name = "numAmountFrom";
            this.numAmountFrom.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAmountFrom.Size = new System.Drawing.Size(116, 22);
            this.numAmountFrom.TabIndex = 5;
            // 
            // btnReset
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.btnReset.Appearance = appearance3;
            this.btnReset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnReset.HotTrackAppearance = appearance4;
            this.btnReset.Location = new System.Drawing.Point(1007, 1);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(99, 30);
            this.btnReset.TabIndex = 53;
            this.btnReset.Text = "&Reset All";
            this.btnReset.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtEODId
            // 
            this.txtEODId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance5.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance5.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance5.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance5;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtEODId.ButtonsRight.Add(editorButton1);
            this.txtEODId.Location = new System.Drawing.Point(109, 64);
            this.txtEODId.Name = "txtEODId";
            this.txtEODId.Size = new System.Drawing.Size(116, 22);
            this.txtEODId.TabIndex = 2;
            this.txtEODId.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtEODId_EditorButtonClick);
            this.txtEODId.TextChanged += new System.EventHandler(this.txtEODId_TextChanged);
            this.txtEODId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEODId_KeyPress);
            // 
            // lblEODId
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.lblEODId.Appearance = appearance6;
            this.lblEODId.AutoSize = true;
            this.lblEODId.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblEODId.Location = new System.Drawing.Point(7, 67);
            this.lblEODId.Name = "lblEODId";
            this.lblEODId.Size = new System.Drawing.Size(42, 17);
            this.lblEODId.TabIndex = 52;
            this.lblEODId.Text = "EOD#";
            // 
            // cboTrans
            // 
            this.cboTrans.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "All";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "Only Rx";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "Exclude Rx";
            this.cboTrans.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.cboTrans.Location = new System.Drawing.Point(317, 92);
            this.cboTrans.Name = "cboTrans";
            this.cboTrans.Size = new System.Drawing.Size(116, 24);
            this.cboTrans.TabIndex = 10;
            // 
            // lblTransactions
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.lblTransactions.Appearance = appearance7;
            this.lblTransactions.AutoSize = true;
            this.lblTransactions.Location = new System.Drawing.Point(229, 95);
            this.lblTransactions.Name = "lblTransactions";
            this.lblTransactions.Size = new System.Drawing.Size(39, 17);
            this.lblTransactions.TabIndex = 50;
            this.lblTransactions.Text = "Trans";
            // 
            // txtCloseStationId
            // 
            this.txtCloseStationId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance8.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance8.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton2.Appearance = appearance8;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtCloseStationId.ButtonsRight.Add(editorButton2);
            this.txtCloseStationId.Location = new System.Drawing.Point(109, 92);
            this.txtCloseStationId.Name = "txtCloseStationId";
            this.txtCloseStationId.Size = new System.Drawing.Size(116, 22);
            this.txtCloseStationId.TabIndex = 3;
            this.txtCloseStationId.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCloseStationId_EditorButtonClick);
            this.txtCloseStationId.TextChanged += new System.EventHandler(this.txtCloseStationId_TextChanged);
            this.txtCloseStationId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCloseStationId_KeyPress);
            // 
            // lblCloseStationId
            // 
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.lblCloseStationId.Appearance = appearance9;
            this.lblCloseStationId.AutoSize = true;
            this.lblCloseStationId.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCloseStationId.Location = new System.Drawing.Point(7, 95);
            this.lblCloseStationId.Name = "lblCloseStationId";
            this.lblCloseStationId.Size = new System.Drawing.Size(97, 17);
            this.lblCloseStationId.TabIndex = 48;
            this.lblCloseStationId.Text = "Close Station#";
            // 
            // groupBoxTransType
            // 
            this.groupBoxTransType.Controls.Add(this.chkAll);
            this.groupBoxTransType.Controls.Add(this.chkROA);
            this.groupBoxTransType.Controls.Add(this.chkReturn);
            this.groupBoxTransType.Controls.Add(this.chkSale);
            this.groupBoxTransType.Location = new System.Drawing.Point(569, 31);
            this.groupBoxTransType.Name = "groupBoxTransType";
            this.groupBoxTransType.Size = new System.Drawing.Size(238, 45);
            this.groupBoxTransType.TabIndex = 13;
            this.groupBoxTransType.Text = "Transaction  Type";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Checked = true;
            this.chkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAll.Location = new System.Drawing.Point(7, 19);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(40, 18);
            this.chkAll.TabIndex = 39;
            this.chkAll.Text = "All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // chkROA
            // 
            this.chkROA.AutoSize = true;
            this.chkROA.Checked = true;
            this.chkROA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkROA.Enabled = false;
            this.chkROA.Location = new System.Drawing.Point(180, 19);
            this.chkROA.Name = "chkROA";
            this.chkROA.Size = new System.Drawing.Size(52, 18);
            this.chkROA.TabIndex = 38;
            this.chkROA.Text = "ROA";
            this.chkROA.UseVisualStyleBackColor = true;
            // 
            // chkReturn
            // 
            this.chkReturn.AutoSize = true;
            this.chkReturn.Checked = true;
            this.chkReturn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReturn.Enabled = false;
            this.chkReturn.Location = new System.Drawing.Point(108, 19);
            this.chkReturn.Name = "chkReturn";
            this.chkReturn.Size = new System.Drawing.Size(68, 18);
            this.chkReturn.TabIndex = 37;
            this.chkReturn.Text = "Return";
            this.chkReturn.UseVisualStyleBackColor = true;
            // 
            // chkSale
            // 
            this.chkSale.AutoSize = true;
            this.chkSale.Checked = true;
            this.chkSale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSale.Enabled = false;
            this.chkSale.Location = new System.Drawing.Point(51, 19);
            this.chkSale.Name = "chkSale";
            this.chkSale.Size = new System.Drawing.Size(53, 18);
            this.chkSale.TabIndex = 36;
            this.chkSale.Text = "Sale";
            this.chkSale.UseVisualStyleBackColor = true;
            // 
            // txtLast4DigitsOfCC
            // 
            appearance10.TextHAlignAsString = "Right";
            this.txtLast4DigitsOfCC.Appearance = appearance10;
            this.txtLast4DigitsOfCC.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtLast4DigitsOfCC.Location = new System.Drawing.Point(315, 64);
            this.txtLast4DigitsOfCC.MaxLength = 4;
            this.txtLast4DigitsOfCC.Name = "txtLast4DigitsOfCC";
            this.txtLast4DigitsOfCC.Size = new System.Drawing.Size(116, 22);
            this.txtLast4DigitsOfCC.TabIndex = 9;
            this.txtLast4DigitsOfCC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLast4DigitsOfCC_KeyPress);
            // 
            // lblLast4DigitsOfCC
            // 
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.lblLast4DigitsOfCC.Appearance = appearance11;
            this.lblLast4DigitsOfCC.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblLast4DigitsOfCC.Location = new System.Drawing.Point(229, 59);
            this.lblLast4DigitsOfCC.Name = "lblLast4DigitsOfCC";
            this.lblLast4DigitsOfCC.Size = new System.Drawing.Size(85, 33);
            this.lblLast4DigitsOfCC.TabIndex = 46;
            this.lblLast4DigitsOfCC.Text = "Last 4 Digits of CC";
            // 
            // chkOnhold
            // 
            this.chkOnhold.AutoSize = true;
            this.chkOnhold.Location = new System.Drawing.Point(435, 42);
            this.chkOnhold.Name = "chkOnhold";
            this.chkOnhold.Size = new System.Drawing.Size(76, 18);
            this.chkOnhold.TabIndex = 13;
            this.chkOnhold.Text = "On Hold";
            this.chkOnhold.UseVisualStyleBackColor = true;
            // 
            // btnPayTypeList
            // 
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance12.FontData.BoldAsString = "True";
            appearance12.ForeColor = System.Drawing.Color.White;
            this.btnPayTypeList.Appearance = appearance12;
            this.btnPayTypeList.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPayTypeList.Location = new System.Drawing.Point(812, 6);
            this.btnPayTypeList.Name = "btnPayTypeList";
            this.btnPayTypeList.Size = new System.Drawing.Size(101, 25);
            this.btnPayTypeList.TabIndex = 17;
            this.btnPayTypeList.Text = "Pay Types";
            this.btnPayTypeList.Click += new System.EventHandler(this.btnPayTypeList_Click);
            this.btnPayTypeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyDown);
            this.btnPayTypeList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btnPayTypeList_KeyUp);
            // 
            // grpPayTypeList
            // 
            this.grpPayTypeList.Controls.Add(this.chkSelectAll);
            this.grpPayTypeList.Controls.Add(this.dataGridList);
            this.grpPayTypeList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPayTypeList.Location = new System.Drawing.Point(812, 55);
            this.grpPayTypeList.Name = "grpPayTypeList";
            this.grpPayTypeList.Size = new System.Drawing.Size(186, 61);
            this.grpPayTypeList.TabIndex = 14;
            this.grpPayTypeList.TabStop = false;
            this.grpPayTypeList.Visible = false;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(5, 38);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(87, 17);
            this.chkSelectAll.TabIndex = 13;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // dataGridList
            // 
            this.dataGridList.AllowDrop = true;
            this.dataGridList.AllowUserToAddRows = false;
            this.dataGridList.AllowUserToDeleteRows = false;
            this.dataGridList.AllowUserToResizeColumns = false;
            this.dataGridList.AllowUserToResizeRows = false;
            this.dataGridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridList.ColumnHeadersVisible = false;
            this.dataGridList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkBox});
            this.dataGridList.Location = new System.Drawing.Point(0, 10);
            this.dataGridList.Name = "dataGridList";
            this.dataGridList.RowHeadersVisible = false;
            this.dataGridList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridList.Size = new System.Drawing.Size(180, 21);
            this.dataGridList.TabIndex = 12;
            this.dataGridList.Visible = false;
            this.dataGridList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridList_RowsAdded);
            // 
            // chkBox
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.NullValue = false;
            this.chkBox.DefaultCellStyle = dataGridViewCellStyle1;
            this.chkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBox.HeaderText = " ";
            this.chkBox.Name = "chkBox";
            this.chkBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chkBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chkBox.Width = 20;
            // 
            // btnSearch
            // 
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.SystemColors.Control;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance13;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance14;
            this.btnSearch.Location = new System.Drawing.Point(1007, 68);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(99, 30);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblCustomerName
            // 
            appearance15.BorderColor = System.Drawing.Color.Green;
            appearance15.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Appearance = appearance15;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.Location = new System.Drawing.Point(643, 94);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(234, 18);
            this.lblCustomerName.TabIndex = 36;
            this.lblCustomerName.Text = "Customer Name";
            // 
            // txtPaymentType
            // 
            this.txtPaymentType.AutoSize = false;
            this.txtPaymentType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPaymentType.Location = new System.Drawing.Point(812, 33);
            this.txtPaymentType.MaxLength = 20;
            this.txtPaymentType.Name = "txtPaymentType";
            this.txtPaymentType.ReadOnly = true;
            this.txtPaymentType.Size = new System.Drawing.Size(398, 23);
            this.txtPaymentType.TabIndex = 13;
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.White;
            appearance16.BackColorDisabled = System.Drawing.Color.White;
            appearance16.BackColorDisabled2 = System.Drawing.Color.White;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance16;
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
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance19;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance20.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.White;
            appearance21.BackColorDisabled = System.Drawing.Color.White;
            appearance21.BackColorDisabled2 = System.Drawing.Color.White;
            appearance21.BorderColor = System.Drawing.Color.Black;
            appearance21.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            appearance22.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance22.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance22.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance23;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.Color.White;
            appearance26.BackColorDisabled = System.Drawing.Color.White;
            appearance26.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance27.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.FontData.BoldAsString = "True";
            appearance28.ForeColor = System.Drawing.Color.Black;
            appearance28.TextHAlignAsString = "Left";
            appearance28.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance28;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.Color.White;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance30.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance30;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance32.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance32;
            this.grdSearch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance33.BackColor = System.Drawing.Color.Navy;
            appearance33.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.Navy;
            appearance34.BackColorDisabled = System.Drawing.Color.Navy;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance34.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance34.BorderColor = System.Drawing.Color.Gray;
            appearance34.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance34;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance35.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance35;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance36.BackColor = System.Drawing.Color.White;
            appearance36.BackColor2 = System.Drawing.SystemColors.Control;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance36;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance39;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(7, 148);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(1203, 317);
            this.grdSearch.TabIndex = 16;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSearch_InitializeRow);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSearch_ClickCellButton);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // chkSrchByTransID
            // 
            this.chkSrchByTransID.AutoSize = true;
            this.chkSrchByTransID.Location = new System.Drawing.Point(569, 9);
            this.chkSrchByTransID.Name = "chkSrchByTransID";
            this.chkSrchByTransID.Size = new System.Drawing.Size(172, 18);
            this.chkSrchByTransID.TabIndex = 16;
            this.chkSrchByTransID.Text = "Search By TransID Only";
            this.chkSrchByTransID.UseVisualStyleBackColor = true;
            this.chkSrchByTransID.CheckedChanged += new System.EventHandler(this.chkSrchByTransID_CheckedChanged);
            // 
            // txtTransID
            // 
            this.txtTransID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransID.Enabled = false;
            this.txtTransID.Location = new System.Drawing.Point(746, 6);
            this.txtTransID.MaxLength = 20;
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.Size = new System.Drawing.Size(57, 22);
            this.txtTransID.TabIndex = 17;
            this.txtTransID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTransID_KeyPress);
            // 
            // cboStationId
            // 
            this.cboStationId.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboStationId.Location = new System.Drawing.Point(315, 6);
            this.cboStationId.Name = "cboStationId";
            this.cboStationId.Size = new System.Drawing.Size(116, 24);
            this.cboStationId.TabIndex = 7;
            // 
            // txtCustomer
            // 
            appearance40.BorderColor = System.Drawing.Color.Lime;
            appearance40.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtCustomer.Appearance = appearance40;
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance41.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance41.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance41.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance41.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton3.Appearance = appearance41;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton3.Text = "";
            editorButton3.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton3);
            this.txtCustomer.Location = new System.Drawing.Point(523, 92);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(116, 22);
            this.txtCustomer.TabIndex = 15;
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // ultraLabel2
            // 
            appearance42.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance42;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Location = new System.Drawing.Point(437, 95);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(83, 17);
            this.ultraLabel2.TabIndex = 24;
            this.ultraLabel2.Text = "Customer ID";
            // 
            // chkIIASTransactionsOnly
            // 
            this.chkIIASTransactionsOnly.AutoSize = true;
            this.chkIIASTransactionsOnly.Location = new System.Drawing.Point(435, 24);
            this.chkIIASTransactionsOnly.Name = "chkIIASTransactionsOnly";
            this.chkIIASTransactionsOnly.Size = new System.Drawing.Size(122, 18);
            this.chkIIASTransactionsOnly.TabIndex = 12;
            this.chkIIASTransactionsOnly.Text = "IIAS Trans Only";
            this.chkIIASTransactionsOnly.UseVisualStyleBackColor = true;
            // 
            // chkCCTransOnly
            // 
            this.chkCCTransOnly.AutoSize = true;
            this.chkCCTransOnly.Location = new System.Drawing.Point(435, 6);
            this.chkCCTransOnly.Name = "chkCCTransOnly";
            this.chkCCTransOnly.Size = new System.Drawing.Size(130, 18);
            this.chkCCTransOnly.TabIndex = 11;
            this.chkCCTransOnly.Text = "Credit Card Only";
            this.chkCCTransOnly.UseVisualStyleBackColor = true;
            this.chkCCTransOnly.CheckedChanged += new System.EventHandler(this.chkCCTransOnly_CheckedChanged);
            // 
            // ultraLabel1
            // 
            appearance43.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance43;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(229, 39);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(52, 17);
            this.ultraLabel1.TabIndex = 21;
            this.ultraLabel1.Text = "User ID";
            this.ultraLabel1.Click += new System.EventHandler(this.ultraLabel1_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(315, 36);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(116, 22);
            this.txtUserID.TabIndex = 8;
            // 
            // ultraLabel21
            // 
            appearance44.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance44;
            this.ultraLabel21.AutoSize = true;
            this.ultraLabel21.Location = new System.Drawing.Point(229, 10);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(63, 17);
            this.ultraLabel21.TabIndex = 19;
            this.ultraLabel21.Text = "Station #";
            // 
            // clTo
            // 
            this.clTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clTo.DateButtons.Add(dateButton1);
            this.clTo.Location = new System.Drawing.Point(109, 36);
            this.clTo.Name = "clTo";
            this.clTo.NonAutoSizeHeight = 22;
            this.clTo.Size = new System.Drawing.Size(116, 21);
            this.clTo.TabIndex = 1;
            this.clTo.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // clFrom
            // 
            this.clFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clFrom.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clFrom.DateButtons.Add(dateButton2);
            this.clFrom.Location = new System.Drawing.Point(109, 6);
            this.clFrom.Name = "clFrom";
            this.clFrom.NonAutoSizeHeight = 22;
            this.clFrom.Size = new System.Drawing.Size(116, 21);
            this.clFrom.TabIndex = 0;
            this.clFrom.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // lbl2
            // 
            appearance45.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance45;
            this.lbl2.AutoSize = true;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(7, 39);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(20, 17);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "To ";
            // 
            // lbl1
            // 
            appearance46.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Appearance = appearance46;
            this.lbl1.AutoSize = true;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(7, 10);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(36, 17);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "From ";
            // 
            // clTo1
            // 
            this.clTo1.BackColor = System.Drawing.SystemColors.Window;
            this.clTo1.Location = new System.Drawing.Point(0, 0);
            this.clTo1.Name = "clTo1";
            this.clTo1.NonAutoSizeHeight = 21;
            this.clTo1.Size = new System.Drawing.Size(121, 21);
            this.clTo1.TabIndex = 0;
            this.clTo1.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // clFrom1
            // 
            this.clFrom1.BackColor = System.Drawing.SystemColors.Window;
            this.clFrom1.Location = new System.Drawing.Point(1, 1);
            this.clFrom1.Name = "clFrom1";
            this.clFrom1.NonAutoSizeHeight = 21;
            this.clFrom1.Size = new System.Drawing.Size(121, 21);
            this.clFrom1.TabIndex = 0;
            this.clFrom1.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // tabMain
            // 
            appearance47.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance47;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance48;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance49.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance49;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.tabMainControl);
            this.tabMain.Location = new System.Drawing.Point(9, 9);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(1226, 515);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
            appearance50.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.Transparent;
            appearance51.BackColor2 = System.Drawing.Color.Transparent;
            appearance51.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance51;
            appearance52.BackColor = System.Drawing.Color.Transparent;
            appearance52.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance52;
            ultraTab1.TabPage = this.tabMainControl;
            ultraTab1.Text = "Criteria";
            this.tabMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.tabMain.TabStop = false;
            this.tabMain.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1222, 491);
            // 
            // sbMain
            // 
            appearance53.BackColor = System.Drawing.Color.White;
            appearance53.BackColor2 = System.Drawing.SystemColors.Control;
            appearance53.BorderColor = System.Drawing.Color.Black;
            appearance53.FontData.Name = "Verdana";
            appearance53.FontData.SizeInPoints = 10F;
            appearance53.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance53;
            this.sbMain.Location = new System.Drawing.Point(0, 568);
            this.sbMain.Name = "sbMain";
            appearance54.BorderColor = System.Drawing.Color.Black;
            appearance54.BorderColor3DBase = System.Drawing.Color.Black;
            appearance54.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance54;
            appearance55.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance55;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(1253, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance56.BackColor = System.Drawing.Color.White;
            appearance56.BackColor2 = System.Drawing.SystemColors.Control;
            appearance56.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance56.FontData.BoldAsString = "True";
            appearance56.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance56;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance57.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance57.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance57;
            this.btnClose.Location = new System.Drawing.Point(1113, 532);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(121, 26);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "&Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance58.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance58.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance58.FontData.BoldAsString = "True";
            appearance58.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance58;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(831, 532);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(121, 26);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "&Print Receipt";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnEmailInvoice
            // 
            this.btnEmailInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance59.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance59.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance59.FontData.BoldAsString = "True";
            appearance59.ForeColor = System.Drawing.Color.White;
            this.btnEmailInvoice.Appearance = appearance59;
            this.btnEmailInvoice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnEmailInvoice.Location = new System.Drawing.Point(692, 532);
            this.btnEmailInvoice.Name = "btnEmailInvoice";
            this.btnEmailInvoice.Size = new System.Drawing.Size(128, 26);
            this.btnEmailInvoice.TabIndex = 7;
            this.btnEmailInvoice.Text = "&Email Invoice";
            this.btnEmailInvoice.Click += new System.EventHandler(this.btnEmailInvoice_Click);
            // 
            // btnPrintGiftReceipt
            // 
            this.btnPrintGiftReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance60.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance60.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance60.FontData.BoldAsString = "True";
            appearance60.ForeColor = System.Drawing.Color.White;
            this.btnPrintGiftReceipt.Appearance = appearance60;
            this.btnPrintGiftReceipt.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrintGiftReceipt.Location = new System.Drawing.Point(963, 532);
            this.btnPrintGiftReceipt.Name = "btnPrintGiftReceipt";
            this.btnPrintGiftReceipt.Size = new System.Drawing.Size(139, 26);
            this.btnPrintGiftReceipt.TabIndex = 9;
            this.btnPrintGiftReceipt.Text = "Print &Gift Receipt";
            this.btnPrintGiftReceipt.Click += new System.EventHandler(this.btnPrintGiftReceipt_Click);
            // 
            // frmViewPOSTransaction
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1253, 593);
            this.Controls.Add(this.btnPrintGiftReceipt);
            this.Controls.Add(this.btnEmailInvoice);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "frmViewPOSTransaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "View POS Transactions";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.tabMainControl.ResumeLayout(false);
            this.tabMainControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseStationId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxTransType)).EndInit();
            this.groupBoxTransType.ResumeLayout(false);
            this.groupBoxTransType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLast4DigitsOfCC)).EndInit();
            this.grpPayTypeList.ResumeLayout(false);
            this.grpPayTypeList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStationId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
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
                if (chkSrchByTransID.Checked && txtTransID.Text != "")
                {
                    if (clsCoreUIHelper.isNumeric(txtTransID.Text) == false)    //PRIMEPOS-3113 02-Sep-2022 JY Added
                    {
                        clsUIHelper.ShowErrorMsg("Trans# should be numeric.");
                        return;
                    }
                    else
                        Search(txtTransID.Text);
                }
                else
                {
                    if (Configuration.convertNullToInt(txtLast4DigitsOfCC.Text) > 0 && txtLast4DigitsOfCC.TextLength < 4)
                    {
                        Resources.Message.Display("Please provide Last 4 Digits of CC", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (txtLast4DigitsOfCC.Enabled) this.txtLast4DigitsOfCC.Focus();
                        return;
                    }
                    Search();
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnSearch_Click()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Search()
        {
            string sSQL = string.Empty;
            #region PRIMEPOS-1637 26-May-2021 JY Added
            string strTransIds = "-999";
            try
            {
                if (chkControlDrugs.Checked == true)
                {
                    sSQL = "Select PTD.TransID, PTDrx.RXNo, PTDrx.NRefill FROM POSTransaction TH" +
                            " INNER JOIN POSTransactionDetail PTD ON TH.TransID = PTD.TransID" +
                            " INNER JOIN POSTransactionRXDetail PTDrx ON PTDrx.TransDetailID = PTD.TransDetailID" +
                            " INNER JOIN Customer CUS ON TH.CustomerId = CUS.CustomerID" +
                            " INNER JOIN util_POSSet PS ON PS.stationid = TH.stationid";
                    sSQL += GenerateWhereClause();
                    DataSet ds = oSearchSvr.Search(sSQL);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        PharmBL oPharmBL = new PharmBL();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataTable dt = oPharmBL.GetDrugClass(Configuration.convertNullToInt64(ds.Tables[0].Rows[i]["RXNo"]).ToString(), Configuration.convertNullToInt(ds.Tables[0].Rows[i]["NRefill"]).ToString());
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (Configuration.convertNullToInt(dt.Rows[0][0]) > 0)
                                {
                                    if (strTransIds == "-999")
                                        strTransIds = Configuration.convertNullToInt(ds.Tables[0].Rows[i]["TransID"]).ToString();
                                    else
                                        strTransIds += "," + Configuration.convertNullToInt(ds.Tables[0].Rows[i]["TransID"]).ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
            }
            #endregion

            oDataSet = new DataSet();
            sSQL = "Select DISTINCT "
                + " TH.TransID as [Trans ID]"
                + " , TH.TransDate as [Trans Date] "
                + ", Convert(char(2), DATEPART(hour, TH.TransactionStartDate)) + ':'+convert(char(2),datepart(mi, TH.TransactionStartDate)) + ':' + convert(char(2),datepart(s, TH.TransactionStartDate)) as [Start Time]"
                + ", Convert(char(2), DATEPART(hour, TH.TransDate)) + ':'+convert(char(2),datepart(mi,TH.TransDate)) + ':' + convert(char(2),datepart(s,TH.TransDate)) as [End Time]"
                + ", Convert(char(2), DATEPART(hour, CAST(CAST( TH.TransDate AS FLOAT )-CAST( TH.TransactionStartDate AS FLOAT ) AS DateTime)))"
                + " + ':'+convert(char(2),datepart(mi, CAST(CAST( TH.TransDate AS FLOAT )-CAST( TH.TransactionStartDate AS FLOAT ) AS DateTime))) "
                + " + ':' + convert(char(2),datepart(s,CAST(CAST( TH.TransDate AS FLOAT )- CAST( TH.TransactionStartDate AS FLOAT ) AS DateTime)))"
                + " as [Total Time]"
                + " , " + "case TH.TransType when 1 then 'Sale' when 2 then 'Return' when 3 then 'Receive on Account' end  as [Trans Type]"
                + " , PS.stationname" + " as [Station ID]"
                + " , CUS.CustomerName as [Customer]"
                + " , TH.GrossTotal as [Gross Total]"
                + " , TH.TotalDiscAmount as [Disc. Amt]"
                + " , TH.TotalTaxAmount as [Tax Amt]"
                + " , TH.TenderedAmount as [Tendered Amt]"
                + ", TH.TotalTransFeeAmt AS [Trans Fee]"   //PRIMEPOS-3119 11-Aug-2022 JY Added
                + " , TH.UserID as [User ID]"
                + " , TH.IsDelivery AS [IsDelivery]" //Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
                + " , TH.DeliverySigSkipped AS [Delivery Sign. Skipped]"  //Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
                + " , TH.StClosedID"
                + " , TH.EODID" //PRIMEPOS-2393 14-May-2019 JY Added
                + " FROM POSTransaction as TH "
                + " INNER JOIN Customer as CUS ON TH.CustomerId = CUS.CustomerID"
                + " INNER JOIN util_POSSet PS ON PS.stationid = TH.stationid";

            sSQL += GenerateWhereClause();
            if (chkControlDrugs.Checked == true) sSQL += " AND TH.TransID IN (" + strTransIds + ")"; //PRIMEPOS-1637 26-May-2021 JY Added
            sSQL += " ORDER BY TH.TransID DESC ";

            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
            oDataSet.Tables[0].TableName = "Master";

            sSQL = "Select "
                    + " PTD." + clsPOSDBConstants.TransDetail_Fld_ItemID
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_ItemDescription + " as [Item Name] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_QTY
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_Price + " as [Unit Price] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_TaxAmount + " as [Tax Amt] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_Discount + " as [Disc. Amt] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice + " as [Ext. Price]"
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_TransID
                    + " FROM POSTransaction as TH "
                    + " INNER JOIN POSTransactionDetail as PTD ON TH.TransID = PTD.TransID"
                    + " INNER JOIN Customer CUS ON TH.CustomerId = CUS.CustomerID"
                    + " INNER JOIN util_POSSet PS ON PS.stationid=TH.stationid";

            sSQL += GenerateWhereClause();
            if (chkControlDrugs.Checked == true) sSQL += " AND TH.TransID IN (" + strTransIds + ")"; //PRIMEPOS-1637 26-May-2021 JY Added

            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
            oDataSet.Tables[1].TableName = "Detail";

            //PRIMEPOS-2900 16-Sep-2020 JY Added PAYMENTPROCESSOR column
            sSQL = " select PTY.PayTypeDesc as [Payment Type], PTP.Amount, case CHARINDEX('|',isnull(PTP.refno,'')) when  0 then PTP.refno else '******'+right(rtrim(left(PTP.refno,CHARINDEX('|',PTP.refno)-1)) ,4) End as [Ref No.], PTP.CustomerSign, PTP.BinarySign, PTP.SigType, TH.TransID, PTP.IsIIASPayment, PTP.PAYMENTPROCESSOR"
                  + " FROM POSTransaction TH"
                  + " INNER JOIN postranspayment PTP ON TH.TransID=PTP.TransID"
                  + " INNER JOIN PayType PTY ON PTP.TransTypeCode=PTY.PayTypeID"
                  + " INNER JOIN Customer CUS ON TH.CustomerID=CUS.CustomerID"
                  + " INNER JOIN util_POSSet PS ON PS.stationid=TH.stationid";

            sSQL += GenerateWhereClause();
            if (chkControlDrugs.Checked == true) sSQL += " AND TH.TransID IN (" + strTransIds + ")"; //PRIMEPOS-1637 26-May-2021 JY Added

            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
            oDataSet.Tables[2].TableName = "Payment";

            oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["Trans ID"], oDataSet.Tables[1].Columns["TransID"]);
            oDataSet.Relations.Add("MasterPayment", oDataSet.Tables[0].Columns["Trans ID"], oDataSet.Tables[2].Columns["TransID"]);

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

            grdSearch.DisplayLayout.Bands[2].Columns["TransID"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["CustomerSign"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["BinarySign"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["SigType"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["PAYMENTPROCESSOR"].Hidden = true; //PRIMEPOS-2900 16-Sep-2020 JY Added
            //grdSearch.DisplayLayout.Bands[2].CardView=true;

            //grdSearch.DisplayLayout.Bands[2].CardSettings.Style=CardStyle.MergedLabels;
            //grdSearch.DisplayLayout.Bands[2].CardSettings.ShowCaption=false;
            //grdSearch.DisplayLayout.Bands[2].CardSettings.Width=80;

            if (grdSearch.DisplayLayout.Bands[2].Columns.Exists("ViewSign") == false)
            {
                Infragistics.Win.UltraWinGrid.UltraGridColumn oCol = grdSearch.DisplayLayout.Bands[2].Columns.Add("ViewSign");
                oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                oCol.Header.Caption = "View Sign";
            }

            grdSearch.DisplayLayout.Bands[2].HeaderVisible = true;
            grdSearch.DisplayLayout.Bands[2].Header.Caption = "Payment Information";
            grdSearch.DisplayLayout.Bands[2].Header.Appearance.FontData.SizeInPoints = 12;
            grdSearch.DisplayLayout.Bands[2].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);

            //foreach (UltraGridRow oRow in grdSearch.Rows)
            //{
            //    oRow.ExpandAll();
            //}
            //if (grdSearch.Rows.Count > 0)
            //{
            //    grdSearch.ActiveRow = grdSearch.Rows[0];
            //}

            grdSearch.Refresh();
            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
        }

        private void Search(string TransID)
        {
            oDataSet = new DataSet();
            string sSQL = "Select "
                + " TH." + clsPOSDBConstants.TransHeader_Fld_TransID + " as [Trans ID]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_TransDate + " as [Trans Date] "
                //Following Code Added by Krishna on 7 June 2011
                + ", Convert(char(2),   DATEPART(hour, TransactionStartDate)) + ':'+convert(char(2),datepart(mi, TransactionStartDate)) + ':' + convert(char(2),datepart(s, TransactionStartDate)) as [Start Time]"
                + ", Convert(char(2),   DATEPART(hour, TransDate)) + ':'+convert(char(2),datepart(mi,TransDate)) + ':' + convert(char(2),datepart(s,Transdate)) as [End Time]"
                + ", Convert(char(2),DATEPART(hour, CAST(CAST( TransDate AS FLOAT )-CAST( TransactionStartDate AS FLOAT ) AS DateTime)))"
                + " + ':'+convert(char(2),datepart(mi, CAST(CAST( TransDate AS FLOAT )-CAST( TransactionStartDate AS FLOAT ) AS DateTime))) "
                + " + ':' + convert(char(2),datepart(s,CAST(CAST( TransDate AS FLOAT )- CAST( TransactionStartDate AS FLOAT ) AS DateTime)))"
                + " as [Total Time]"
                //Till here added by krishna on 7 june 2011
                + " , " + "case TransType when 1 then 'Sale' when 2 then 'Return' when 3 then 'Receive on Account' end  as [Trans Type]"
                + " , ps.stationname" + " as [Station ID]"
                + " , " + clsPOSDBConstants.Customer_Fld_CustomerName + " as [Customer]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_GrossTotal + " as [Gross Total]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount + " as [Disc. Amt]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount + " as [Tax Amt]"
                + " , " + clsPOSDBConstants.TransHeader_Fld_TenderedAmount + " as [Tendered Amt]"
                + ", TH.TotalTransFeeAmt AS [Trans Fee]"   //PRIMEPOS-3119 11-Aug-2022 JY Added
                + " , TH." + clsPOSDBConstants.Users_Fld_UserID + " as [User ID]"
                + " , TH." + clsPOSDBConstants.TransHeader_Fld_IsDelivery + " AS [IsDelivery]"   //Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
                + " , TH." + clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped + " AS [Delivery Sign. Skipped]"  //Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
                + " , TH.StClosedID"    //PRIMEPOS-2684 10-May-2019 JY Added missing column
                + " , TH.EODID" //PRIMEPOS-2393 14-May-2019 JY Added
                + " FROM "
                + clsPOSDBConstants.TransHeader_tbl + " as TH "
                + ", " + clsPOSDBConstants.Customer_tbl + " as Cus, util_POSSet ps "
                + " where ps.stationid=th.stationid and TH." + clsPOSDBConstants.Customer_Fld_CustomerId + " = Cus." + clsPOSDBConstants.TransHeader_Fld_CustomerID
                + " and TH.TransID = " + txtTransID.Text;

            #region Sprint-26 - PRIMEPOS-2388 12-Jul-2017 JY Commented
            ////Changed by SRT(Abhishek) Date : 22/10/2009
            ////Added to search using  no of station id.
            //if (this.cboStationId.SelectedItem.DisplayText.Trim() != "ALL")
            //{
            //    sSQL += " and TH." + clsPOSDBConstants.TransHeader_Fld_StationID + " ='" + this.cboStationId.SelectedItem.DisplayText.Trim() + "'";
            //}

            //if (chkCCTransOnly.Checked == true)
            //{
            //    sSQL += " and TH.TransID in " +
            //        " (SELECT DISTINCT pt.TransID FROM POSTransPayment AS pt INNER JOIN PayType ON pt.TransTypeCode = PayType.PayTypeID WHERE (PayType.PayType = 'CC'))";
            //}

            //if (chkIIASTransactionsOnly.Checked == true)
            //{
            //    sSQL += " and TH.TransID in (SELECT DISTINCT pt.TransID FROM POSTransPayment AS pt WHERE IsIIASPayment=1)";
            //}

            //#region Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
            //if (chkOnhold.Checked == true)
            //{
            //    sSQL += " AND TH." + clsPOSDBConstants.TransHeader_Fld_WasonHold + " = 1 ";
            //}
            //#endregion
            //sSQL += " order by transid desc ";
            #endregion

            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
            oDataSet.Tables[0].TableName = "Master";

            sSQL = "Select "
                    + " PTD." + clsPOSDBConstants.TransDetail_Fld_ItemID
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_ItemDescription + " as [Item Name] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_QTY
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_Price + " as [Unit Price] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_TaxAmount + " as [Tax Amt] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_Discount + " as [Disc. Amt] "
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice + " as [Ext. Price]"
                    + " , PTD." + clsPOSDBConstants.TransDetail_Fld_TransID
                    + " FROM "
                    + clsPOSDBConstants.TransDetail_tbl + " as PTD "
                    + " , " + clsPOSDBConstants.TransHeader_tbl + " as PTH "
                    + ", " + clsPOSDBConstants.Customer_tbl + " as Cus, util_POSSet ps  "
                    + " where PTH." + clsPOSDBConstants.TransHeader_Fld_TransID + " = PTD." + clsPOSDBConstants.TransDetail_Fld_TransID
                    + " and ps.stationid=pth.stationid and PTH." + clsPOSDBConstants.Customer_Fld_CustomerId + " = Cus." + clsPOSDBConstants.TransHeader_Fld_CustomerID
                    + " and PTH.TransID = " + txtTransID.Text;

            #region Sprint-26 - PRIMEPOS-2388 12-Jul-2017 JY Commented
            ////Changed by SRT(Abhishek) Date : 22/10/2009
            ////Added to search using  no of station id.
            //if (this.cboStationId.SelectedItem.DisplayText.Trim() != "ALL")
            //{
            //    sSQL += " and PTH." + clsPOSDBConstants.TransHeader_Fld_StationID + " ='" + this.cboStationId.SelectedItem.DisplayText.Trim() + "'";
            //}
            ////End of Changed by SRT(Abhishek) Date : 22/10/2009

            //if (chkCCTransOnly.Checked == true)
            //{
            //    sSQL += " and PTH.TransID in " +
            //        " (SELECT DISTINCT pt.TransID FROM POSTransPayment AS pt INNER JOIN PayType ON pt.TransTypeCode = PayType.PayTypeID WHERE (PayType.PayType = 'CC'))";
            //}

            //if (chkIIASTransactionsOnly.Checked == true)
            //{
            //    sSQL += " and PTH.TransID in (SELECT DISTINCT pt.TransID FROM POSTransPayment AS pt WHERE IsIIASPayment=1)";
            //}
            #endregion

            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
            oDataSet.Tables[1].TableName = "Detail";
            //PRIMEPOS-2684 10-May-2019 JY Added IsIIASPayment missing column
            //PRIMEPOS-2900 16-Sep-2020 JY Added PAYMENTPROCESSOR column
            sSQL = " select PayTypeDesc as [Payment Type],Amount,case CHARINDEX('|',isnull(refno,'')) when  0 then refno else '******'+right(rtrim(left(refno,CHARINDEX('|',refno)-1)) ,4) End as [Ref No.],CustomerSign, BinarySign, SigType, PTH.TransID, PTP.IsIIASPayment, PTP.PAYMENTPROCESSOR from "
                    + " postranspayment PTP, PayType PTY, POSTransaction PTH,Customer Cus, util_POSSet ps  "
                    + " where PTP.TransTypeCode=PTY.PayTypeID And PTH.CustomerID=Cus.CustomerID and ps.stationid=pth.stationid "
                    + " and PTH.TransID=PTP.TransID "
                    + " and PTH.TransID = " + txtTransID.Text;

            #region Sprint-26 - PRIMEPOS-2388 12-Jul-2017 JY Commented
            ////Changed by SRT(Abhishek) Date : 22/10/2009
            ////Added to search using  no of station id.
            //if (this.cboStationId.SelectedItem.DisplayText.Trim() != "ALL")
            //{
            //    sSQL += " and PTH." + clsPOSDBConstants.TransHeader_Fld_StationID + " ='" + this.cboStationId.SelectedItem.DisplayText.Trim() + "'";
            //}
            ////End of Changed by SRT(Abhishek) Date : 22/10/2009

            //if (chkCCTransOnly.Checked == true)
            //{
            //    sSQL += " and PTH.TransID in " +
            //        " (SELECT DISTINCT pt.TransID FROM POSTransPayment AS pt INNER JOIN PayType ON pt.TransTypeCode = PayType.PayTypeID WHERE (PayType.PayType = 'CC'))";
            //}

            //if (chkIIASTransactionsOnly.Checked == true)
            //{
            //    sSQL += " and PTP.IsIIASPayment=1";
            //}
            #endregion

            oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
            oDataSet.Tables[2].TableName = "Payment";

            //DataRelation dr=new DataRelation("MasterDetail",oDataSet.Tables[0].Columns["TransID"],oDataSet.Tables[1].Columns["TransID"]);
            oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["Trans ID"], oDataSet.Tables[1].Columns["TransID"]);
            oDataSet.Relations.Add("MasterPayment", oDataSet.Tables[0].Columns["Trans ID"], oDataSet.Tables[2].Columns["TransID"]);

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

            grdSearch.DisplayLayout.Bands[2].Columns["TransID"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["CustomerSign"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["BinarySign"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["SigType"].Hidden = true;
            grdSearch.DisplayLayout.Bands[2].Columns["PAYMENTPROCESSOR"].Hidden = true; //PRIMEPOS-2900 16-Sep-2020 JY Added
            //grdSearch.DisplayLayout.Bands[2].CardView=true;

            //grdSearch.DisplayLayout.Bands[2].CardSettings.Style=CardStyle.MergedLabels;
            //grdSearch.DisplayLayout.Bands[2].CardSettings.ShowCaption=false;
            //grdSearch.DisplayLayout.Bands[2].CardSettings.Width=80;

            if (grdSearch.DisplayLayout.Bands[2].Columns.Exists("ViewSign") == false)
            {
                Infragistics.Win.UltraWinGrid.UltraGridColumn oCol = grdSearch.DisplayLayout.Bands[2].Columns.Add("ViewSign");
                oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                oCol.Header.Caption = "View Sign";
            }

            grdSearch.DisplayLayout.Bands[2].HeaderVisible = true;
            grdSearch.DisplayLayout.Bands[2].Header.Caption = "Payment Information";
            grdSearch.DisplayLayout.Bands[2].Header.Appearance.FontData.SizeInPoints = 12;
            grdSearch.DisplayLayout.Bands[2].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;

            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);

            //foreach (UltraGridRow oRow in grdSearch.Rows)
            //{
            //    oRow.ExpandAll();
            //}
            //if (grdSearch.Rows.Count > 0)
            //{
            //    grdSearch.ActiveRow = grdSearch.Rows[0];
            //}

            grdSearch.Refresh();
            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
        }

        private void FillStationIDs()
        {
            try
            {
                DataSet oStationDs = new DataSet();
                string sSQL = "Select Distinct(StationID) From POSTransaction";
                oStationDs.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
                oStationDs.Tables[0].TableName = "STATION";

                this.cboStationId.Items.Add("All");
                //this.cboStationId.SelectedItem("ALL");

                foreach (DataRow stationRow in oStationDs.Tables[0].Rows)
                {
                    this.cboStationId.Items.Add(stationRow["StationID"].ToString());
                }
                this.cboStationId.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "FillStationIDs()");
                clsUIHelper.ShowErrorMsg(exp.Message);
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
                logger.Fatal(exp, "grdSearch_DoubleClick()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.cboTrans.SelectedIndex = 0;    //PRIMEPOS-2389 15-Oct-2018 JY Added
                this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(7, 0, 0, 0));
                this.clTo.Value = DateTime.Today;
                if (string.IsNullOrEmpty(this.txtCustomer.Text))
                {
                    this.lblCustomerName.Text = "";
                }

                clsUIHelper.SetAppearance(this.grdSearch);
                clsUIHelper.SetReadonlyRow(this.grdSearch);

                this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtCustomer.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCustomer.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                //this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                //this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.clFrom.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.clFrom.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.clTo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.clTo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                //Added by SRT(Abhishek) Date : 22/10/2009
                //Method will be called to 
                FillStationIDs();
                //End Of Added by SRT(Abhishek) Date : 22/10/2009

                //Added by Krishna on 28 June 2011
                grdSearch.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
                grdSearch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                //Till here added by Krishna on 28 June 2011

                //Added by shitaljit on 7 November 2012
                FillPayType();
                tabMain.Height = this.Height - 110;
                btnSearch.Left = btnPayTypeList.Left;
                btnReset.Top = btnSearch.Top;   //PRIMEPOS-2700 02-Jul-2019 JY Added
                btnReset.Left = btnSearch.Left + btnSearch.Width + 10;  //PRIMEPOS-2700 02-Jul-2019 JY Added
                Search();
                if (this.grdSearch.Rows.Count == 0)
                {
                    this.clFrom.Focus();
                }
                clsUIHelper.setColorSchecme(this);
                this.TopMost = false;//added by shitaljit 
                this.MaximizeBox = false;   //Sprint-21 19-Nov-2015 JY Added to avoid overlap bottom buttons with taskbar
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearch_Load()");
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
                if (e.KeyData == Keys.F4)
                {
                    btnSearch_Click(sender, e);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmSearch_KeyDown()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void ExtractFromToAmount(String strFromToAmt, out decimal FromAmt, out decimal ToAmt)
        {
            string[] strArr = strFromToAmt.Split('-');
            FromAmt = 0;
            ToAmt = 0;
            if (strArr.Length == 2)
            {
                FromAmt = Configuration.convertNullToDecimal(strArr[0].ToString());
                ToAmt = Configuration.convertNullToDecimal(strArr[1].ToString());
            }
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
                #region Apply color Scheme
                if (this.grdSearch.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    ColorSchemeForViewPOSTrans oColorSchemeForViewPOSTrans = new ColorSchemeForViewPOSTrans();
                    ColorSchemeForViewPOSTransData oColorSchemeForViewPOSTransData = oColorSchemeForViewPOSTrans.PopulateList("");
                    if (oColorSchemeForViewPOSTransData != null)
                    {
                        if (oColorSchemeForViewPOSTransData.ColorSchemeForViewPOSTrans.Rows.Count > 0)
                        {
                            decimal FromAmt = 0;
                            decimal ToAmt = 0;

                            foreach (ColorSchemeForViewPOSTransRow oRow in oColorSchemeForViewPOSTransData.ColorSchemeForViewPOSTrans.Rows)
                            {
                                FromAmt = oRow.FromAmount;
                                ToAmt = oRow.ToAmount;

                                Color BackColor = Configuration.ExtractColor(oRow.BackColor);
                                Color ForeColor = Configuration.ExtractColor(oRow.ForeColor);

                                foreach (UltraGridRow oGridRow in grdSearch.Rows)
                                {
                                    if (Configuration.convertNullToDecimal(oGridRow.Cells["Tendered Amt"].Value.ToString()) >= FromAmt && Configuration.convertNullToDecimal(oGridRow.Cells["Tendered Amt"].Value.ToString()) <= ToAmt)
                                    {
                                        oGridRow.Appearance.BackColor = BackColor;
                                        oGridRow.Appearance.ForeColor = ForeColor;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                //Following Code Added by Krishna on 24 June 2011
                for (int i = 0; i < this.grdSearch.Rows.Count; i++)
                {
                    if (this.grdSearch.Rows[i].Cells["Trans Type"].Text == "Sale")
                    {
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor = Color.Green;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor2 = Color.SeaGreen;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                    }
                    else if (this.grdSearch.Rows[i].Cells["Trans Type"].Text == "Return")
                    {
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor = Color.Maroon;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor2 = Color.Red;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                    }
                    else if (this.grdSearch.Rows[i].Cells["Trans Type"].Text == "Receive on Account")
                    {
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor = Color.Purple;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor2 = Color.MediumPurple;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                    }
                }
                for (int i = 0; i < this.oDataSet.Tables["Payment"].Rows.Count; i++)
                {
                    string str = oDataSet.Tables["Payment"].Rows[i]["IsIIASPayment"].ToString();
                    string transId = oDataSet.Tables["Payment"].Rows[i]["transid"].ToString();
                    if (oDataSet.Tables["Payment"].Rows[i]["IsIIASPayment"].ToString() != "False")
                    {
                        DataRow row = oDataSet.Tables["Payment"].Rows[i].GetParentRow("MasterPayment");
                        int k = oDataSet.Tables["Master"].Rows.IndexOf(row);
                        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.BackColor = Color.CornflowerBlue;
                        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.BackColor2 = Color.SteelBlue;
                        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                    }
                }
                //Till here Added by Krishna on 24 June 2011
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "grdSearch_InitializeLayout()");
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
            catch (Exception exp)
            {
                logger.Fatal(exp, "TextBoxKeyDown()");
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
            catch (Exception exp)
            {
                logger.Fatal(exp, "resizeColumns()");
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

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.grdSearch.ActiveRow == null)
                {
                    clsUIHelper.ShowErrorMsg("No Transaction Available");
                    return;
                }

                if (grdSearch.ActiveRow.Band.Index > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
                }

                Int32 TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());

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
                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    //if (oTransDData.TransDetail[0].ItemID.ToString().Trim().ToUpper() == "RX")    //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented as its checking only first item
                    oTransRxData = oTransRxSvr.PopulateData(TransID);   //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added
                    if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                    {
                        //oTransRxData = oTransRxSvr.PopulateData(TransID); //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true); //PRIMEPOS-2884 21-Aug-2020 JY Added bPrintDupReceipt parameter as "True"
                        oRxLabel.Print();
                    }
                    else
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);    //PRIMEPOS-2884 21-Aug-2020 JY Added bPrintDupReceipt parameter as "True"
                        oRxLabel.Print();
                    }
                }
                else //End of added by atul 07-jan-2011
                {
                    RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, true);   //PRIMEPOS-2884 21-Aug-2020 JY Added bPrintDupReceipt parameter as "True"
                    oRxLabel.Print();
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnPrint_Click()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void grdSearch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Band.Index == 2)
            {
                e.Row.Cells["Viewsign"].Value = "View Sign";

                if ((e.Row.Cells["SigType"].Value.ToString().Trim() == clsPOSDBConstants.STRINGIMAGE))//|| e.Row.Cells["SigType"].Value.ToString().Trim() == "*") )
                {
                    if (e.Row.Cells["CustomerSign"].Value.ToString().Trim() == "")
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                    }
                    else
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.ActivateOnly;
                    }
                }
                else if ((e.Row.Cells["SigType"].Value.ToString().Trim() == clsPOSDBConstants.BINARYIMAGE))//|| e.Row.Cells["SigType"].Value.ToString().Trim() == "*") )
                {
                    if (e.Row.Cells["BinarySign"].Value == System.DBNull.Value)
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                    }
                    else
                    {
                        e.Row.Cells["ViewSign"].Activation = Activation.ActivateOnly;
                    }
                }
                else if (e.Row.Cells["SigType"].Value.ToString().Trim() == string.Empty)
                {
                    e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                }
                else if (e.Row.Cells["SigType"].Value.ToString().Trim() == "*")
                {
                    e.Row.Cells["ViewSign"].Activation = Activation.Disabled;
                }



            }

        }

        private void grdSearch_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == "ViewSign")
            {
                //
                //Configuration.CInfo.SigType = e.Cell.Row.Cells["SigType"].Value.ToString();
                string Sigtype = Configuration.CInfo.SigType;
                if (e.Cell.Row.Cells["CustomerSign"].Text.Trim() != "")
                {
                    Sigtype = "D";
                    frmViewSignature ofrm = new frmViewSignature(e.Cell.Row.Cells["CustomerSign"].Text, Sigtype);
                    ofrm.callByUI = true;
                    ofrm.ShowDialog();
                }
                else if (e.Cell.Row.Cells["BinarySign"].Text.Trim() != string.Empty)//|| (e.Cell.Row.Cells["BinarySign"].Text.Trim() != null))
                {
                    Sigtype = "M";
                    frmViewSignature ofrm = new frmViewSignature((byte[])e.Cell.Row.Cells["BinarySign"].Value, Sigtype, Configuration.convertNullToString(e.Cell.Row.Cells["PAYMENTPROCESSOR"].Value).Trim().ToUpper());    //PRIMEPOS-2900 16-Sep-2020 JY Added PAYMENTPROCESSOR parameter
                    ofrm.callByUI = true;
                    ofrm.ShowDialog();
                }
            }
        }

        private void txtCustomer_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCustomer();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtCustomer_EditorButtonClick()");
            }
        }

        private void SearchCustomer()
        {
            try
            {
                //frmCustomerSearch oSearch = new frmCustomerSearch(txtCustomer.Text);
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
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
                ClearCustomer();
                logger.Fatal(exp, "SearchCustomer()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }


        private void ClearCustomer()
        {
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.txtCustomer.Tag = String.Empty;
            this.lblCustomerName.Text = String.Empty;
        }

        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Customer_tbl)
            {
                #region Customer
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
                    logger.Fatal(exp, "FKEdit()");
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    SearchCustomer();
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
                    this.txtCustomer.Tag = "";
                    this.lblCustomerName.Text = "";
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtCustomer_Leave()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void chkSrchByTransID_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSrchByTransID.Checked)
            {
                txtTransID.Enabled = true;
                txtTransID.Focus();
            }
            else
            {
                txtTransID.Enabled = false;
            }
        }
        /// <summary>
        /// Added by Amit Date 15 july 2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAll.Checked == true)
            {
                this.chkSale.Checked = true;
                this.chkSale.Enabled = false;
                this.chkReturn.Checked = true;
                this.chkReturn.Enabled = false;
                this.chkROA.Checked = true;
                this.chkROA.Enabled = false;
            }
            else
            {
                this.chkSale.Checked = false;
                this.chkSale.Enabled = true;
                this.chkReturn.Checked = false;
                this.chkReturn.Enabled = true;
                this.chkROA.Checked = false;
                this.chkROA.Enabled = true;

            }
        }

        private void btnPayTypeList_Click(object sender, EventArgs e)
        {
            if (grpPayTypeList.Visible == false)
            {
                PayTypeListExpand(true);
            }
            else
            {
                PayTypeListExpand(false);
                GetSelectedPayTypes();
            }
        }

        private void PayTypeListExpand(bool Value)
        {
            if (Value == true)
            {
                if (grpPayTypeList.Visible == false)
                {
                    dataGridList.Visible = true;
                    grpPayTypeList.Visible = true;
                    dataGridList.Height = 250;
                    grpPayTypeList.Height = dataGridList.Height + 50;
                    GridVisibleFlag = true;
                    chkSelectAll.Location = new Point(chkSelectAll.Location.X, dataGridList.Height + 10);
                    grpPayTypeList.BringToFront();
                    grpPayTypeList.Focus();
                }
            }
            else
            {
                if (grpPayTypeList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpPayTypeList.Visible = false;
                    GridVisibleFlag = false;
                }
            }
        }

        private void btnPayTypeList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                dataGridList.Focus();
            }
        }

        private void btnPayTypeList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                if (grpPayTypeList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpPayTypeList.Visible = false;
                }
            }
        }

        POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
        VendorData vendorData = new VendorData();
        POS_Core.DataAccess.SearchSvr SearchSvr = new POS_Core.DataAccess.SearchSvr();

        private void FillPayType()
        {
            DataSet PayTypeData = new DataSet();
            PayTypeData = SearchSvr.Search(clsPOSDBConstants.PayType_tbl, "", "", 0, -1);
            dataGridList.DataSource = PayTypeData.Tables[0];
            for (int i = 1; i <= PayTypeData.Tables[0].Columns.Count; i++)
            {
                dataGridList.Columns[i].ReadOnly = true;
                if (i == 1)
                {
                    dataGridList.Columns[i].Width = 30;
                    dataGridList.Columns[i].Name = "PayType";
                }
                else
                    dataGridList.Columns[i].Width = 115;
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int rowsCount = dataGridList.Rows.Count;
            if (rowsCount > 0)
            {
                for (int i = 0; i < rowsCount; i++)
                {

                    if (chkSelectAll.Checked == true)
                    {

                        dataGridList.Rows[i].Cells[0].Value = true;
                        chkSelectAll.Text = "Unselect All";
                    }
                    else
                    {
                        dataGridList.Rows[i].Cells[0].Value = false;
                        chkSelectAll.Text = "Select All";
                    }
                }
            }

        }

        public void GetSelectedPayTypes()
        {
            string strPayTypeCode = "";
            string strPayTypeName = "";
            foreach (DataGridViewRow oRow in dataGridList.Rows)
            {
                if (POS_Core.Resources.Configuration.convertNullToBoolean(oRow.Cells[0].Value))
                {
                    strPayTypeCode += ",'" + oRow.Cells[1].Value.ToString() + "'";
                    strPayTypeName += "," + oRow.Cells["Description"].Value.ToString();
                }
            }

            if (strPayTypeCode.StartsWith(","))
            {
                strPayTypeCode = strPayTypeCode.Substring(1);
                strPayTypeName = strPayTypeName.Substring(1);
            }
            txtPaymentType.Text = strPayTypeName;
            txtPaymentType.Tag = strPayTypeCode;
        }

        private void dataGridList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dataGridList.Rows.Count; i++)
            {
                dataGridList.Rows[i].Cells[dataGridList.Columns["chkBox"].Index].Value = true;
            }
            chkSelectAll.Checked = true;
            GetSelectedPayTypes();

        }

        private void chkCCTransOnly_CheckedChanged(object sender, EventArgs e)
        {
            btnPayTypeList.Enabled = !(this.chkCCTransOnly.Checked);
        }

        private void btnEmailInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());
                frmSendEmail email = new frmSendEmail(TransID);
                email.bPrintDuplicateReceipt = true;  //PRIMEPOS-2900 15-Sep-2020 JY Added
                email.ShowDialog();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnEmailInvoice_Click()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ultraLabel1_Click(object sender, EventArgs e)
        {

        }

        private void txtLast4DigitsOfCC_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        #region Sprint-26 - PRIMEPOS-2388 12-Jul-2017 JY Added
        private string GenerateWhereClause()
        {
            string strWhere = " WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(this.txtCloseStationId.Text))    //PRIMEPOS-2494 26-Feb-2018 JY Added
            {
                strWhere += " AND TH." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
            }
            if (!string.IsNullOrWhiteSpace(this.txtEODId.Text))    //PRIMEPOS-2393 14-May-2019 JY Added
            {
                strWhere += " AND TH." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
            }
            if (string.IsNullOrWhiteSpace(this.txtCloseStationId.Text) && string.IsNullOrWhiteSpace(this.txtEODId.Text))
            {
                strWhere += " AND convert(datetime,TH.TransDate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59' as datetime) ,113) ";
            }

            #region Sprint-24 - PRIMEPOS-2342 17-Oct-2016 JY Added
            if (chkOnhold.Checked == true && string.IsNullOrWhiteSpace(this.txtCloseStationId.Text) && string.IsNullOrWhiteSpace(this.txtEODId.Text))
            {
                strWhere += " AND TH." + clsPOSDBConstants.TransHeader_Fld_WasonHold + " = 1 ";
            }
            #endregion

            if (this.cboStationId.SelectedItem.DisplayText.Trim() != "All")
            {
                strWhere += " and TH." + clsPOSDBConstants.TransHeader_Fld_StationID + " ='" + this.cboStationId.SelectedItem.DisplayText.Trim() + "'";
            }

            if (this.txtUserID.Text.Trim() != "")
            {
                strWhere += " and TH." + clsPOSDBConstants.Users_Fld_UserID + " ='" + this.txtUserID.Text.Trim().Replace("'", "''") + "'";
            }

            if (this.txtCustomer.Text.Trim() != "" && Configuration.convertNullToInt(this.txtCustomer.Tag.ToString()) != 0)
            {
                strWhere += " and TH." + clsPOSDBConstants.TransHeader_Fld_CustomerID + " ='" + this.txtCustomer.Tag.ToString().Trim().Replace("'", "''") + "'";
            }

            string tempWhere = string.Empty;
            if (chkCCTransOnly.Checked == true)
            {
                //tempWhere = " AND b.PayType = 'CC'";  //PRIMEPOS-2661 05-Apr-2019 JY Commented
                tempWhere = " AND b.PayTypeID IN ('3', '4', '5', '6', '7')";    //PRIMEPOS-2661 05-Apr-2019 JY Added  
            }
            if (chkIIASTransactionsOnly.Checked == true)
            {
                tempWhere += " AND a.IsIIASPayment = 1";
            }
            if (string.IsNullOrEmpty(this.txtPaymentType.Text) == false)
            {
                string s = " AND a.TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") ";
                if (Configuration.convertNullToInt(txtLast4DigitsOfCC.Text) > 0)
                {
                    //s = " AND b.PayType = 'CC' AND SUBSTRING(a.RefNo,13,4) = '" + Configuration.convertNullToString(txtLast4DigitsOfCC.Text).Trim() + "'";    //PRIMEPOS-2661 05-Apr-2019 JY Commented
                    s = " AND b.PayTypeID IN ('3', '4', '5', '6', '7') AND SUBSTRING(a.RefNo,13,4) = '" + Configuration.convertNullToString(txtLast4DigitsOfCC.Text).Trim() + "'";  //PRIMEPOS-2661 05-Apr-2019 JY Added  
                }
                tempWhere += s;
            }
            else
            {
                if (Configuration.convertNullToInt(txtLast4DigitsOfCC.Text) > 0)
                {
                    //tempWhere += "AND b.PayType = 'CC' and SUBSTRING(a.RefNo,13,4) = '" + Configuration.convertNullToString(txtLast4DigitsOfCC.Text).Trim() + "'";    //PRIMEPOS-2661 05-Apr-2019 JY Commented
                    tempWhere += "AND b.PayTypeID IN ('3', '4', '5', '6', '7') and SUBSTRING(a.RefNo,13,4) = '" + Configuration.convertNullToString(txtLast4DigitsOfCC.Text).Trim() + "'";  //PRIMEPOS-2661 05-Apr-2019 JY Added  
                }
            }
            if (!String.IsNullOrWhiteSpace(tempWhere))
            {
                strWhere += " and TH.TransID in (SELECT DISTINCT a.TransID FROM POSTransPayment a INNER JOIN PayType b ON a.TransTypeCode = b.PayTypeID WHERE 1=1 " + tempWhere + ")";
            }

            #region PRIMEPOS-2389 15-Oct-2018 JY Added
            string strTemp = string.Empty;
            if (cboTrans.SelectedIndex == 1)
            {
                strTemp = " AND TH.TransID IN (SELECT DISTINCT TransID FROM POSTransactionDetail WHERE ItemID = 'RX')";
            }
            else if (cboTrans.SelectedIndex == 2)
            {
                strTemp = " AND TH.TransID NOT IN (SELECT DISTINCT TransID FROM POSTransactionDetail WHERE ItemID = 'RX')";
            }
            if (strTemp != string.Empty)
                strWhere += strTemp;
            #endregion

            List<String> lstTransType = new List<String>();

            if (this.chkAll.Checked == true)
            {
                lstTransType.Add("1");  //Sale
                lstTransType.Add("2");  //Return
                lstTransType.Add("3");  //ROA
            }
            else
            {
                if (chkSale.Checked == true)
                {
                    lstTransType.Add("1");  //Sale
                }
                if (chkReturn.Checked == true)
                {
                    lstTransType.Add("2");  //Return
                }
                if (chkROA.Checked == true)
                {
                    lstTransType.Add("3");  //ROA
                }
            }

            if (lstTransType.Count > 0)
            {
                strWhere += " and TransType in (" + string.Join(",", lstTransType.ToArray()) + ")";
            }

            #region PRIMEPOS-2465 11-Mar-2020 JY Added
            if (chkConsiderPaidAmount.Checked)
            {
                decimal nFromAmt = Configuration.convertNullToDecimal(numAmountFrom.Value);
                decimal nToAmt = Configuration.convertNullToDecimal(numAmountTo.Value);
                if (nFromAmt < nToAmt)
                    strWhere += " AND TH.TotalPaid >= " + nFromAmt + " AND TH.TotalPaid <= " + nToAmt;
                else
                    strWhere += " AND TH.TotalPaid >= " + nToAmt + " AND TH.TotalPaid <= " + nFromAmt;
            }
            #endregion         
            return strWhere;
        }
        #endregion

        #region PRIMEPOS-2494 26-Feb-2018 JY Added
        private void txtCloseStationId_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCloseStationId();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtCloseStationId_EditorButtonClick()");
            }
        }

        private void SearchCloseStationId()
        {
            try
            {
                //frmViewEODStation ofrmViewEODStation = new frmViewEODStation(clsPOSDBConstants.StationCloseHeader_tbl); //PRIMEPOS-2700 02-Jul-2019 JY Commented
                #region PRIMEPOS-2700 02-Jul-2019 JY Added
                frmViewEODStation ofrmViewEODStation;
                if (txtEODId.Text.Trim() != "")
                {
                    ofrmViewEODStation = new frmViewEODStation(clsPOSDBConstants.StationCloseHeader_tbl, Configuration.convertNullToInt(txtEODId.Text.Trim()));
                }
                else
                {
                    ofrmViewEODStation = new frmViewEODStation(clsPOSDBConstants.StationCloseHeader_tbl);
                }
                #endregion                
                ofrmViewEODStation.AllowMultiRowSelect = true;
                //ofrmViewEODStation.searchInConstructor = true;
                ofrmViewEODStation.DisplayRecordAtStartup = true;
                ofrmViewEODStation.ShowDialog(this);
                if (!ofrmViewEODStation.IsCanceled)
                {
                    string strCloseStationId = string.Empty;
                    foreach (UltraGridRow oRow in ofrmViewEODStation.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            if (strCloseStationId == string.Empty)
                                strCloseStationId = oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID].Text;
                            else
                                strCloseStationId += "," + oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_StationCloseID].Text;
                        }
                    }
                    txtCloseStationId.Text = strCloseStationId;
                }
                else
                {
                    txtCloseStationId.Text = string.Empty;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchCloseStationId()");
            }
        }

        private void txtCloseStationId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char Delete = (char)8;
            char comma = (char)',';
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != comma;
        }
        #endregion

        #region PRIMEPOS-2393 14-May-2019 JY Added
        private void txtEODId_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchEODId();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "txtEODId_EditorButtonClick()");
            }
        }

        private void SearchEODId()
        {
            try
            {
                frmViewEODStation ofrmViewEODStation = new frmViewEODStation(clsPOSDBConstants.EndOfDay_tbl);
                ofrmViewEODStation.AllowMultiRowSelect = true;
                ofrmViewEODStation.DisplayRecordAtStartup = true;
                ofrmViewEODStation.ShowDialog(this);
                if (!ofrmViewEODStation.IsCanceled)
                {
                    string strEODId = string.Empty;
                    foreach (UltraGridRow oRow in ofrmViewEODStation.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            if (strEODId == string.Empty)
                                strEODId = oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_EODID].Text;
                            else
                                strEODId += "," + oRow.Cells[clsPOSDBConstants.StationCloseHeader_Fld_EODID].Text;
                        }
                    }
                    txtEODId.Text = strEODId;
                }
                else
                {
                    txtEODId.Text = string.Empty;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchEODId()");
            }
        }

        private void txtEODId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char Delete = (char)8;
            char comma = (char)',';
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != comma;
        }
        #endregion

        #region PRIMEPOS-2700 01-Jul-2019 JY Added
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.clFrom.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(7, 0, 0, 0));
                this.clTo.Value = DateTime.Today;
                txtCloseStationId.Text = "";
                txtEODId.Text = "";
                this.cboStationId.SelectedIndex = 0;
                txtUserID.Text = "";
                txtLast4DigitsOfCC.Text = "";
                cboTrans.SelectedIndex = 0;
                chkCCTransOnly.Checked = false;
                chkIIASTransactionsOnly.Checked = false;
                chkOnhold.Checked = false;
                txtCustomer.Text = "";
                lblCustomerName.Text = "";
                chkSrchByTransID.Checked = false;
                txtTransID.Text = "";
                txtTransID.Enabled = false;
                chkAll.Checked = true;
                chkAll.Enabled = true;
                chkSale.Checked = false;
                chkReturn.Checked = false;
                chkROA.Checked = false;
                chkSale.Enabled = false;
                chkReturn.Enabled = false;
                chkROA.Enabled = false;
                dataGridList_RowsAdded(sender, null);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnReset_Click(object sender, EventArgs e)");
            }
        }

        private void txtCloseStationId_TextChanged(object sender, EventArgs e)
        {
            if (txtCloseStationId.Text.Trim() == "")
            {
                clFrom.Enabled = clTo.Enabled = chkOnhold.Enabled = true;
            }
            else
            {
                clFrom.Enabled = clTo.Enabled = chkOnhold.Enabled = false;
            }
        }

        private void txtEODId_TextChanged(object sender, EventArgs e)
        {
            if (txtEODId.Text.Trim() == "")
            {
                clFrom.Enabled = clTo.Enabled = chkOnhold.Enabled = true;
            }
            else
            {
                clFrom.Enabled = clTo.Enabled = chkOnhold.Enabled = false;
            }
        }
        #endregion

        #region PRIMEPOS-2465 11-Mar-2020 JY Added
        private void chkConsiderPaidAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConsiderPaidAmount.Checked)
            {
                numAmountFrom.Enabled = numAmountTo.Enabled = true;
                numAmountFrom.Focus();
            }
            else
            {
                numAmountFrom.Enabled = numAmountTo.Enabled = false;
            }
        }
        #endregion

        #region PRIMEPOS-2677 10-Jul-2020 JY Added
        private void btnPrintGiftReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                bool bReceiptsPrinted = false;
                if (this.grdSearch.ActiveRow == null)
                {
                    clsUIHelper.ShowErrorMsg("No Transaction Available");
                    return;
                }

                if (grdSearch.ActiveRow.Band.Index > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.ActiveRow.ParentRow;
                }

                Int32 TransID = Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());

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

                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    oTransRxData = oTransRxSvr.PopulateData(TransID);
                    if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                        oRxLabel.bPrintGiftReciept = true;
                        int NoOfGiftReceipt = 0;
                        if (Configuration.CPOSSet.NoOfGiftReceipt == 0) NoOfGiftReceipt = 1;
                        bReceiptsPrinted = oRxLabel.PrintGiftCoupon(NoOfGiftReceipt);
                    }
                    else
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                        oRxLabel.bPrintGiftReciept = true;
                        int NoOfGiftReceipt = 0;
                        if (Configuration.CPOSSet.NoOfGiftReceipt == 0) NoOfGiftReceipt = 1;
                        bReceiptsPrinted = oRxLabel.PrintGiftCoupon(NoOfGiftReceipt);
                    }
                }
                else
                {
                    RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                    oRxLabel.bPrintGiftReciept = true;
                    int NoOfGiftReceipt = 0;
                    if (Configuration.CPOSSet.NoOfGiftReceipt == 0) NoOfGiftReceipt = 1;
                    bReceiptsPrinted = oRxLabel.PrintGiftCoupon(NoOfGiftReceipt);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnPrintGiftReceipt_Click()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-3113 02-Sep-2022 JY Added
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