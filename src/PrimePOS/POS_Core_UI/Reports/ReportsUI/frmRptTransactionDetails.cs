using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
//using POS.UI;
using POS_Core_UI.Reports.Reports;
using System.Data;
using POS_Core.DataAccess;
//using POS_Core.DataAccess;
using Resources;
using POS_Core.Resources;
using PharmData;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for frmViewTransaction.
    /// </summary>
    public class frmRptTransactionDetail : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private UltraTextEditor txtPaymentType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraButton btnPayTypeList;
        private GroupBox grpPayTypeList;
        private CheckBox chkSelectAll;
        private DataGridView dataGridList;
        private DataGridViewCheckBoxColumn chkBox;
        private UltraGrid grdSearch;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private IContainer components;
        private SearchSvr oSearchSvr = new SearchSvr(); //Sprint-19 - 2154 14-Jan-2015 JY Added
        private int CurrentX; //Sprint-19 - 2154 14-Jan-2015 JY Added
        private int CurrentY; //Sprint-19 - 2154 14-Jan-2015 JY Added
        public UltraTextEditor txtCloseStationId;
        private Infragistics.Win.Misc.UltraLabel lblCloseStationId;
        private UltraCheckEditor chkOnlyDiscountedTrans;
        private UltraCheckEditor chkPrimeRxPayTrans;//PRIMEPOS-3282
        private RadioButton optDetail;
        private RadioButton optSummary;
        public UltraTextEditor txtEODId;
        private Infragistics.Win.Misc.UltraLabel lblEODId;
        private CheckBox chkConsiderPaidAmount;
        private Infragistics.Win.Misc.UltraLabel lblAmountTo;
        private Infragistics.Win.Misc.UltraLabel lblAmountFrom;
        private UltraNumericEditor numAmountTo;
        private UltraNumericEditor numAmountFrom;
        private UltraCheckEditor chkControlDrugs;
        private DataSet oDataSet;   //Sprint-19 - 2154 14-Jan-2015 JY Added
        private bool isPrimeRxPaySelected = false; //PRIMEPOS-3282

        public frmRptTransactionDetail()
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
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();//PRIMEPOS-3282
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkControlDrugs = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkConsiderPaidAmount = new System.Windows.Forms.CheckBox();
            this.lblAmountTo = new Infragistics.Win.Misc.UltraLabel();
            this.lblAmountFrom = new Infragistics.Win.Misc.UltraLabel();
            this.numAmountTo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numAmountFrom = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtEODId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblEODId = new Infragistics.Win.Misc.UltraLabel();
            this.optDetail = new System.Windows.Forms.RadioButton();
            this.chkOnlyDiscountedTrans = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkPrimeRxPayTrans = new Infragistics.Win.UltraWinEditors.UltraCheckEditor(); //PRIMEPOS-3282
            this.lblCloseStationId = new Infragistics.Win.Misc.UltraLabel();
            this.optSummary = new System.Windows.Forms.RadioButton();
            this.btnPayTypeList = new Infragistics.Win.Misc.UltraButton();
            this.txtPaymentType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.txtCloseStationId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.grpPayTypeList = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.chkBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkControlDrugs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseStationId)).BeginInit();
            this.grpPayTypeList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkControlDrugs);
            this.groupBox1.Controls.Add(this.chkConsiderPaidAmount);
            this.groupBox1.Controls.Add(this.lblAmountTo);
            this.groupBox1.Controls.Add(this.lblAmountFrom);
            this.groupBox1.Controls.Add(this.numAmountTo);
            this.groupBox1.Controls.Add(this.numAmountFrom);
            this.groupBox1.Controls.Add(this.txtEODId);
            this.groupBox1.Controls.Add(this.lblEODId);
            this.groupBox1.Controls.Add(this.optDetail);
            this.groupBox1.Controls.Add(this.chkOnlyDiscountedTrans);
            this.groupBox1.Controls.Add(this.chkPrimeRxPayTrans);//PRIMEPOS-3282
            this.groupBox1.Controls.Add(this.lblCloseStationId);
            this.groupBox1.Controls.Add(this.optSummary);
            this.groupBox1.Controls.Add(this.btnPayTypeList);
            this.groupBox1.Controls.Add(this.txtPaymentType);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.txtItemCode);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.cboTransType);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.txtStationID);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtCloseStationId);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.groupBox1.Location = new System.Drawing.Point(10, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(994, 146);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // chkControlDrugs
            // 
            appearance46.TextVAlignAsString = "Middle";
            this.chkControlDrugs.Appearance = appearance46;
            this.chkControlDrugs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkControlDrugs.Location = new System.Drawing.Point(561, 90);
            this.chkControlDrugs.Name = "chkControlDrugs";
            this.chkControlDrugs.Size = new System.Drawing.Size(153, 20);
            this.chkControlDrugs.TabIndex = 13;
            this.chkControlDrugs.Text = "Control Drugs";
            // 
            // chkConsiderPaidAmount
            // 
            this.chkConsiderPaidAmount.AutoSize = true;
            this.chkConsiderPaidAmount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkConsiderPaidAmount.Location = new System.Drawing.Point(9, 117);
            this.chkConsiderPaidAmount.Name = "chkConsiderPaidAmount";
            this.chkConsiderPaidAmount.Size = new System.Drawing.Size(160, 20);
            this.chkConsiderPaidAmount.TabIndex = 4;
            this.chkConsiderPaidAmount.Text = "Consider Paid Amount";
            this.chkConsiderPaidAmount.UseVisualStyleBackColor = true;
            this.chkConsiderPaidAmount.CheckedChanged += new System.EventHandler(this.chkConsiderPaidAmount_CheckedChanged);
            // 
            // lblAmountTo
            // 
            appearance47.ForeColor = System.Drawing.Color.Black;
            this.lblAmountTo.Appearance = appearance47;
            this.lblAmountTo.AutoSize = true;
            this.lblAmountTo.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblAmountTo.Location = new System.Drawing.Point(343, 117);
            this.lblAmountTo.Name = "lblAmountTo";
            this.lblAmountTo.Size = new System.Drawing.Size(20, 16);
            this.lblAmountTo.TabIndex = 62;
            this.lblAmountTo.Text = "To";
            // 
            // lblAmountFrom
            // 
            appearance48.ForeColor = System.Drawing.Color.Black;
            this.lblAmountFrom.Appearance = appearance48;
            this.lblAmountFrom.AutoSize = true;
            this.lblAmountFrom.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblAmountFrom.Location = new System.Drawing.Point(176, 117);
            this.lblAmountFrom.Name = "lblAmountFrom";
            this.lblAmountFrom.Size = new System.Drawing.Size(35, 16);
            this.lblAmountFrom.TabIndex = 61;
            this.lblAmountFrom.Text = "From";
            // 
            // numAmountTo
            // 
            this.numAmountTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numAmountTo.Enabled = false;
            this.numAmountTo.FormatString = "";
            this.numAmountTo.Location = new System.Drawing.Point(380, 115);
            this.numAmountTo.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountTo.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountTo.MaskInput = "nnnnnnnnnnnnnnn";
            this.numAmountTo.MaxValue = 99999999D;
            this.numAmountTo.MinValue = -99999999D;
            this.numAmountTo.Name = "numAmountTo";
            this.numAmountTo.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAmountTo.Size = new System.Drawing.Size(116, 21);
            this.numAmountTo.TabIndex = 6;
            // 
            // numAmountFrom
            // 
            this.numAmountFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numAmountFrom.Enabled = false;
            this.numAmountFrom.FormatString = "";
            this.numAmountFrom.Location = new System.Drawing.Point(219, 115);
            this.numAmountFrom.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountFrom.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numAmountFrom.MaskInput = "nnnnnnnnnnnnnnn";
            this.numAmountFrom.MaxValue = 99999999D;
            this.numAmountFrom.MinValue = -99999999D;
            this.numAmountFrom.Name = "numAmountFrom";
            this.numAmountFrom.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAmountFrom.Size = new System.Drawing.Size(116, 21);
            this.numAmountFrom.TabIndex = 5;
            // 
            // txtEODId
            // 
            this.txtEODId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance4;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtEODId.ButtonsRight.Add(editorButton1);
            this.txtEODId.Location = new System.Drawing.Point(400, 90);
            this.txtEODId.Name = "txtEODId";
            this.txtEODId.Size = new System.Drawing.Size(144, 21);
            this.txtEODId.TabIndex = 10;
            this.txtEODId.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtEODId_EditorButtonClick);
            this.txtEODId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEODId_KeyPress);
            // 
            // lblEODId
            // 
            appearance49.ForeColor = System.Drawing.Color.Black;
            this.lblEODId.Appearance = appearance49;
            this.lblEODId.AutoSize = true;
            this.lblEODId.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblEODId.Location = new System.Drawing.Point(245, 92);
            this.lblEODId.Name = "lblEODId";
            this.lblEODId.Size = new System.Drawing.Size(40, 16);
            this.lblEODId.TabIndex = 54;
            this.lblEODId.Text = "EOD#";
            // 
            // optDetail
            // 
            this.optDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optDetail.Location = new System.Drawing.Point(665, 115);
            this.optDetail.Name = "optDetail";
            this.optDetail.Size = new System.Drawing.Size(64, 21);
            this.optDetail.TabIndex = 15;
            this.optDetail.Text = "Detail";
            // 
            // chkOnlyDiscountedTrans
            // 
            appearance50.TextVAlignAsString = "Middle";
            this.chkOnlyDiscountedTrans.Appearance = appearance50;
            this.chkOnlyDiscountedTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkOnlyDiscountedTrans.Location = new System.Drawing.Point(561, 65);
            this.chkOnlyDiscountedTrans.Name = "chkOnlyDiscountedTrans";
            this.chkOnlyDiscountedTrans.Size = new System.Drawing.Size(153, 20);
            this.chkOnlyDiscountedTrans.TabIndex = 12;
            this.chkOnlyDiscountedTrans.Text = "Only Discounted Trans";
            #region PRIMEPOS-3282
            // 
            // chkPrimeRxPayTrans
            // 
            appearance64.TextVAlignAsString = "Middle";
            this.chkPrimeRxPayTrans.Appearance = appearance64;
            this.chkPrimeRxPayTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPrimeRxPayTrans.Location = new System.Drawing.Point(730, 65);
            this.chkPrimeRxPayTrans.Name = "chkPrimeRxPayTrans";
            this.chkPrimeRxPayTrans.Size = new System.Drawing.Size(134, 20);
            this.chkPrimeRxPayTrans.TabIndex = 63;
            this.chkPrimeRxPayTrans.Text = "PrimeRxPay";
            #endregion
            // 
            // lblCloseStationId
            // 
            appearance51.ForeColor = System.Drawing.Color.Black;
            appearance51.TextVAlignAsString = "Middle";
            this.lblCloseStationId.Appearance = appearance51;
            this.lblCloseStationId.AutoSize = true;
            this.lblCloseStationId.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCloseStationId.Location = new System.Drawing.Point(9, 92);
            this.lblCloseStationId.Name = "lblCloseStationId";
            this.lblCloseStationId.Size = new System.Drawing.Size(89, 16);
            this.lblCloseStationId.TabIndex = 50;
            this.lblCloseStationId.Text = "Close Station#";
            // 
            // optSummary
            // 
            this.optSummary.Checked = true;
            this.optSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optSummary.Location = new System.Drawing.Point(561, 115);
            this.optSummary.Name = "optSummary";
            this.optSummary.Size = new System.Drawing.Size(93, 21);
            this.optSummary.TabIndex = 14;
            this.optSummary.TabStop = true;
            this.optSummary.Text = "Summary";
            // 
            // btnPayTypeList
            // 
            appearance52.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance52.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance52.FontData.BoldAsString = "True";
            appearance52.ForeColor = System.Drawing.Color.White;
            this.btnPayTypeList.Appearance = appearance52;
            this.btnPayTypeList.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPayTypeList.Location = new System.Drawing.Point(715, 14);
            this.btnPayTypeList.Name = "btnPayTypeList";
            this.btnPayTypeList.Size = new System.Drawing.Size(144, 23);
            this.btnPayTypeList.TabIndex = 11;
            this.btnPayTypeList.Text = "Pay Types";
            this.btnPayTypeList.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPayTypeList.Click += new System.EventHandler(this.btnPayTypeList_Click);
            this.btnPayTypeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.butPayTypeList_KeyDown);
            this.btnPayTypeList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.butPayTypeList_KeyUp);
            // 
            // txtPaymentType
            // 
            this.txtPaymentType.AutoSize = false;
            this.txtPaymentType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPaymentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtPaymentType.Location = new System.Drawing.Point(561, 40);
            this.txtPaymentType.Name = "txtPaymentType";
            this.txtPaymentType.ReadOnly = true;
            this.txtPaymentType.Size = new System.Drawing.Size(294, 20);
            this.txtPaymentType.TabIndex = 35;
            this.txtPaymentType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPaymentType.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtPaymentType_EditorButtonClick);
            // 
            // ultraLabel4
            // 
            appearance53.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel4.Appearance = appearance53;
            this.ultraLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel4.Location = new System.Drawing.Point(561, 17);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(145, 16);
            this.ultraLabel4.TabIndex = 36;
            this.ultraLabel4.Text = "Pay Types <Blank = All>";
            // 
            // txtItemCode
            // 
            this.txtItemCode.AutoSize = false;
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance10.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance10.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance10.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton2.Appearance = appearance10;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtItemCode.ButtonsRight.Add(editorButton2);
            this.txtItemCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtItemCode.Location = new System.Drawing.Point(400, 65);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(144, 20);
            this.txtItemCode.TabIndex = 9;
            this.txtItemCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            // 
            // ultraLabel2
            // 
            appearance54.ForeColor = System.Drawing.Color.Black;
            appearance54.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance54;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel2.Location = new System.Drawing.Point(9, 67);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(70, 16);
            this.ultraLabel2.TabIndex = 23;
            this.ultraLabel2.Text = "Trans Type";
            // 
            // cboTransType
            // 
            this.cboTransType.AutoSize = false;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.cboTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(103, 65);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(130, 20);
            this.cboTransType.TabIndex = 2;
            this.cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboTransType_KeyDown);
            this.cboTransType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTransType_KeyPress);
            // 
            // ultraLabel20
            // 
            appearance55.ForeColor = System.Drawing.Color.Black;
            appearance55.TextVAlignAsString = "Middle";
            this.ultraLabel20.Appearance = appearance55;
            this.ultraLabel20.AutoSize = true;
            this.ultraLabel20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel20.Location = new System.Drawing.Point(9, 17);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(63, 16);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance56.ForeColor = System.Drawing.Color.Black;
            appearance56.TextVAlignAsString = "Middle";
            this.ultraLabel19.Appearance = appearance56;
            this.ultraLabel19.AutoSize = true;
            this.ultraLabel19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel19.Location = new System.Drawing.Point(9, 42);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(58, 16);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.AutoSize = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton3);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(103, 40);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(130, 20);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.AutoSize = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton4);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(103, 15);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(130, 20);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel1
            // 
            appearance57.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance57;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel1.Location = new System.Drawing.Point(245, 67);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(144, 16);
            this.ultraLabel1.TabIndex = 19;
            this.ultraLabel1.Text = "Item Code <Blank = All>";
            // 
            // txtStationID
            // 
            this.txtStationID.AutoSize = false;
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtStationID.Location = new System.Drawing.Point(400, 40);
            this.txtStationID.MaxLength = 20;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(144, 20);
            this.txtStationID.TabIndex = 8;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel21
            // 
            appearance58.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance58;
            this.ultraLabel21.AutoSize = true;
            this.ultraLabel21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel21.Location = new System.Drawing.Point(245, 42);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(135, 16);
            this.ultraLabel21.TabIndex = 17;
            this.ultraLabel21.Text = "Station # <Blank = All>";
            // 
            // ultraLabel12
            // 
            appearance59.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance59;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.ultraLabel12.Location = new System.Drawing.Point(245, 17);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(136, 16);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "User ID <Blank = Any>";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.txtUserID.Location = new System.Drawing.Point(400, 15);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(144, 20);
            this.txtUserID.TabIndex = 7;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSearch
            // 
            appearance60.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance60.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance60.FontData.BoldAsString = "True";
            appearance60.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance60;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(865, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 23);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtCloseStationId
            // 
            this.txtCloseStationId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance18.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance18.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance18.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton3.Appearance = appearance18;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton3.Text = "";
            editorButton3.Width = 20;
            this.txtCloseStationId.ButtonsRight.Add(editorButton3);
            this.txtCloseStationId.Location = new System.Drawing.Point(103, 90);
            this.txtCloseStationId.Name = "txtCloseStationId";
            this.txtCloseStationId.Size = new System.Drawing.Size(130, 21);
            this.txtCloseStationId.TabIndex = 3;
            this.txtCloseStationId.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCloseStationId_EditorButtonClick);
            this.txtCloseStationId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCloseStationId_KeyPress);
            // 
            // grpPayTypeList
            // 
            this.grpPayTypeList.Controls.Add(this.chkSelectAll);
            this.grpPayTypeList.Controls.Add(this.dataGridList);
            this.grpPayTypeList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPayTypeList.Location = new System.Drawing.Point(725, 35);
            this.grpPayTypeList.Name = "grpPayTypeList";
            this.grpPayTypeList.Size = new System.Drawing.Size(144, 61);
            this.grpPayTypeList.TabIndex = 33;
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
            this.dataGridList.Location = new System.Drawing.Point(1, 10);
            this.dataGridList.Name = "dataGridList";
            this.dataGridList.RowHeadersVisible = false;
            this.dataGridList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridList.Size = new System.Drawing.Size(144, 21);
            this.dataGridList.TabIndex = 12;
            this.dataGridList.Visible = false;
            this.dataGridList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridList_RowsAdded);
            // 
            // chkBox
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.NullValue = false;
            this.chkBox.DefaultCellStyle = dataGridViewCellStyle2;
            this.chkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBox.HeaderText = " ";
            this.chkBox.Name = "chkBox";
            this.chkBox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chkBox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.chkBox.Width = 20;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 513);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(994, 45);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance61.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance61.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance61.FontData.BoldAsString = "True";
            appearance61.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance61;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(718, 14);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance62.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance62.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance62.FontData.BoldAsString = "True";
            appearance62.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance62;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(900, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance63.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance63.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance63.FontData.BoldAsString = "True";
            appearance63.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance63;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(810, 14);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackColorDisabled = System.Drawing.Color.White;
            appearance22.BackColorDisabled2 = System.Drawing.Color.White;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance22;
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
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.White;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance25;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.Color.White;
            appearance27.BackColorDisabled = System.Drawing.Color.White;
            appearance27.BackColorDisabled2 = System.Drawing.Color.White;
            appearance27.BorderColor = System.Drawing.Color.Black;
            appearance27.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            appearance28.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance28.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance28.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance29;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance30;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.Color.White;
            appearance32.BackColorDisabled = System.Drawing.Color.White;
            appearance32.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance33.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.SystemColors.Control;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.FontData.BoldAsString = "True";
            appearance34.ForeColor = System.Drawing.Color.Black;
            appearance34.TextHAlignAsString = "Left";
            appearance34.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance34;
            appearance35.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.White;
            appearance36.BackColor2 = System.Drawing.Color.White;
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance36.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance36;
            appearance37.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.SystemColors.Control;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance38;
            this.grdSearch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance39.BackColor = System.Drawing.Color.Navy;
            appearance39.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.Navy;
            appearance40.BackColorDisabled = System.Drawing.Color.Navy;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance40.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance40.BorderColor = System.Drawing.Color.Gray;
            appearance40.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance40;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance41.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance41;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.SystemColors.Control;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.SystemColors.Control;
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance43.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance44;
            appearance45.BackColor = System.Drawing.Color.White;
            appearance45.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance45;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(10, 149);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(994, 367);
            this.grdSearch.TabIndex = 0;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSearch_InitializeRow);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSearch_ClickCellButton);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // frmRptTransactionDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.grpPayTypeList);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmRptTransactionDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Detail Report";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkControlDrugs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmountFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyDiscountedTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPaymentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCloseStationId)).EndInit();
            this.grpPayTypeList.ResumeLayout(false);
            this.grpPayTypeList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmViewTransaction_Load(object sender, System.EventArgs e)
        {
            cboTransType.AlwaysInEditMode = false;

            this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleStartDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleStartDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpSaleEndDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpSaleEndDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.Left = (frmMain.getInstance().Width - frmMain.getInstance().ultraExplorerBar1.Width - this.Width) / 2;
            this.Top = (frmMain.getInstance().Height - this.Height) / 2;

            populateTransType();
            FillPayType();//Added by Krishna on 22 November 2011

            clsUIHelper.setColorSchecme(this);
            this.dtpSaleEndDate.Value = DateTime.Now;
            this.dtpSaleStartDate.Value = DateTime.Now;

            //this.Location = new Point(340, 50);   //Sprint-19 - 2154 14-Jan-2015 JY Commented
            this.Location = new Point(45, 2);   //Sprint-19 - 2154 14-Jan-2015 JY Added
            // this.txtPaymentType.ButtonsRight[0].Enabled = chkShowTransactionPayment.Checked;//Commented by Krishna on 23 November 2011        

            #region Sprint-19 - 2154 14-Jan-2015 JY Added
            clsUIHelper.SetAppearance(this.grdSearch);
            clsUIHelper.SetReadonlyRow(this.grdSearch);
            grdSearch.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
            grdSearch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            btnSearch_Click(sender, e);
            #endregion
            this.optSummary.Checked = true; //PRIMEPOS-2604 28-Nov-2018 JY Added
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            PreviewReport(false);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void frmViewTransaction_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == Keys.Escape)
                    this.Close();
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtItemCode.ContainsFocus == true)
                    {
                        this.SearchItem();
                    }
                    else if (this.txtPaymentType.ContainsFocus == true)
                    {
                        this.SearchPayType();
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void populateTransType()
        {
            try
            {

                this.cboTransType.Items.Add("All", "All");
                this.cboTransType.Items.Add("Sales", "Sales");
                this.cboTransType.Items.Add("Returns", "Returns");
                this.cboTransType.SelectedIndex = 0;
            }
            catch (Exception) { }
        }

        private void PreviewReport(bool blnPrint)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                rptTransactionDetail oRpt = new rptTransactionDetail();
                String strQuery, strSubQuery, strTotalsQuery = string.Empty, strTotalsQueryFilter = string.Empty, strPayoutFilter = string.Empty;
                string strWhereClause = " WHERE convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113)";

                //Sprint-19 - 2154 16-Jan-2015 JY Added below filter
                //string strItemFilter = string.Empty, strItemFilterSql = string.Empty;
                //if (this.txtItemCode.Text.Trim() != "")
                //{
                //    strItemFilter = " WHERE I.ItemID ='" + this.txtItemCode.Text.Trim() + "' ";
                //    strItemFilterSql = " INNER JOIN (SELECT DISTINCT PTD.TransID from POSTransactionDetail PTD INNER JOIN Item I ON I.ItemID=PTD.ItemID " + strItemFilter + ") a ON a.TransID = PTH.TransID ";
                //}

                //Sprint-19 - 2154 16-Jan-2015 JY Optimized the old queries
                strQuery = "select PT.TotalDiscAmount, PT.TransID, PT.TransDate, PT.UserID, PT.StationID, " +
                        "Case PT.TransType when 1 Then 'Sale' when 2 Then 'Return' end as TransType, " +
                        " I.ItemID, PTD.Qty, PTD.DISCOUNT, PTD.ItemDescription as Description, PTD.Price, PTD.TaxAMOUNT, PTD.ExtendedPrice, ps.StatioNname, PT.StClosedID " +
                        " , PT.EODID " + //PRIMEPOS-2393 14-May-2019 JY Added
                        " from postransaction PT INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                        " INNER JOIN Item I ON I.ItemID=PTD.ItemID " +
                        " INNER JOIN util_POSSet ps ON ps.stationid=pt.stationid " +
                        " where convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

                strSubQuery = "select PTY.PayTypeDesc as [Payment Type], PTP.Amount, case CHARINDEX('|',isnull(PTP.refno,'')) " +
                    " when 0 then PTP.refno else '******'+right(rtrim(left(PTP.refno,CHARINDEX('|',PTP.refno)-1)) ,4) End as [Ref No.], PTP.CustomerSign, PTP.BinarySign, PTP.SigType, PTH.TransID " +
                    " from POSTransaction PTH INNER JOIN postranspayment PTP ON PTH.TransID = PTP.TransID " +
                    " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PayTypeID " +
                    " INNER JOIN Customer Cus ON PTH.CustomerID = Cus.CustomerID  " +
                    " INNER JOIN util_POSSet ps ON ps.stationid = PTH.stationid " +
                    " where convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

                if (this.txtItemCode.Text.Trim() != "")
                {
                    strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
                    strQuery += " AND PT.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
                    strSubQuery += " AND PTH.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
                    strTotalsQueryFilter += " AND PTH.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
                }
                if (this.txtPaymentType.Text.Trim() != "" && txtPaymentType.Tag != null && txtPaymentType.Tag.ToString().Trim().Length > 0)
                {
                    //if (isPrimeRxPaySelected) //PRIMEPOS-3282
                    if (chkPrimeRxPayTrans.Checked) //PRIMEPOS-3282
                    {
                        //strWhereClause += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                        //strQuery += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                        //strSubQuery += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                        //strTotalsQueryFilter += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                        strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                        strQuery += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                        strSubQuery += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                        strTotalsQueryFilter += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                    }
                    else
                    {
                        strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                        strQuery += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                        strSubQuery += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                        strTotalsQueryFilter += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                    }
                }
                else if (chkPrimeRxPayTrans.Checked) //PRIMEPOS-3282
                {
                    strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                    strQuery += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                    strSubQuery += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                    strTotalsQueryFilter += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                }

                if (this.txtUserID.Text.Trim() != "")
                {
                    strWhereClause += " and PT.UserID = '" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                    strQuery += " and PT.UserID = '" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                    strSubQuery += " and PTH.UserID = '" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                    strTotalsQueryFilter += " and PTH.UserID = '" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                    strPayoutFilter += " AND PO.UserID = '" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                }

                if (this.cboTransType.SelectedIndex == 1)
                {
                    strWhereClause += " and PT.TransType = 1";
                    strQuery += " and PT.TransType = 1";
                    strSubQuery += " and PTH.TransType = 1";
                    strTotalsQueryFilter += " and PTH.TransType = 1";
                }
                else if (this.cboTransType.SelectedIndex == 2)
                {
                    strWhereClause += " and PT.TransType = 2";
                    strQuery += " and PT.TransType = 2";
                    strSubQuery += " and PTH.TransType = 2";
                    strTotalsQueryFilter += " and PTH.TransType = 2";
                }

                if (this.txtStationID.Text.Trim() != "")
                {
                    strWhereClause += " and Ps.StationID = '" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
                    strQuery += " and Ps.StationID = '" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
                    strSubQuery += " and Ps.StationID = '" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";    //Sprint-19 - 2154 16-Jan-2015 JY Added to work sub-query for station filter
                    strTotalsQueryFilter += " and PS.StationID = '" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
                    strPayoutFilter += " and PO.StationID = '" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
                }

                if (!string.IsNullOrWhiteSpace(this.txtCloseStationId.Text))    //PRIMEPOS-2494 27-Feb-2018 JY Added
                {
                    strWhereClause += " and PT." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                    strQuery += " and PT." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                    strSubQuery += " and PTH." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                    strTotalsQueryFilter += " and PTH." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                    strPayoutFilter += " and PO.StationCloseID IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                }

                if (!string.IsNullOrWhiteSpace(this.txtEODId.Text))    //PRIMEPOS-2393 14-May-2019 JY Added
                {
                    strWhereClause += " and PT." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                    strQuery += " and PT." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                    strSubQuery += " and PTH." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                    strTotalsQueryFilter += " and PTH." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                    strPayoutFilter += " and PO." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                }

                if (chkOnlyDiscountedTrans.Checked) //PRIMEPOS-2561 01-Aug-2018 JY Added condition to consider only discounted transactions
                {
                    strWhereClause += " AND PT.TotalDiscAmount <> 0 ";
                    strQuery += " AND PT.TotalDiscAmount <> 0 ";
                    strSubQuery += " AND PTH.TotalDiscAmount <> 0";
                    strTotalsQueryFilter += " AND PTH.TotalDiscAmount <> 0";
                }

                #region PRIMEPOS-2465 12-Mar-2020 JY Added
                if (chkConsiderPaidAmount.Checked)
                {
                    decimal nFromAmt = Configuration.convertNullToDecimal(numAmountFrom.Value);
                    decimal nToAmt = Configuration.convertNullToDecimal(numAmountTo.Value);
                    if (nFromAmt < nToAmt)
                    {
                        strWhereClause += " AND PT.TotalPaid >= " + nFromAmt + " AND PT.TotalPaid <= " + nToAmt;
                        strQuery += " AND PT.TotalPaid >= " + nFromAmt + " AND PT.TotalPaid <= " + nToAmt;
                        strSubQuery += " AND PTH.TotalPaid >= " + nFromAmt + " AND PTH.TotalPaid <= " + nToAmt;
                        strTotalsQueryFilter += " AND PTH.TotalPaid >= " + nFromAmt + " AND PTH.TotalPaid <= " + nToAmt;
                    }
                    else
                    {
                        strWhereClause += " AND PT.TotalPaid >= " + nToAmt + " AND PT.TotalPaid <= " + nFromAmt;
                        strQuery += " AND PT.TotalPaid >= " + nToAmt + " AND PT.TotalPaid <= " + nFromAmt;
                        strSubQuery += " AND PTH.TotalPaid >= " + nToAmt + " AND PTH.TotalPaid <= " + nFromAmt;
                        strTotalsQueryFilter += " AND PTH.TotalPaid >= " + nToAmt + " AND PTH.TotalPaid <= " + nFromAmt;
                    }
                }
                #endregion

                #region PRIMEPOS-1637 26-May-2021 JY Added
                string strTransIds = "-999", sSQL = string.Empty;
                try
                {
                    if (chkControlDrugs.Checked == true)
                    {
                        sSQL = "Select PTD.TransID, PTDrx.RXNo, PTDrx.NRefill FROM POSTransaction PT" +
                                " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID" +
                                " INNER JOIN POSTransactionRXDetail PTDrx ON PTDrx.TransDetailID = PTD.TransDetailID" +
                                " INNER JOIN util_POSSet PS ON PS.stationid = PT.stationid";
                        sSQL += strWhereClause;
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
                        strQuery += " AND PT.TransID IN (" + strTransIds + ")";
                        strSubQuery += " AND PTH.TransID IN (" + strTransIds + ")";
                        strTotalsQueryFilter += " AND PTH.TransID IN (" + strTransIds + ")";
                    }
                }
                catch (Exception Ex)
                {
                }
                #endregion

                #region PRIMEPOS-2604 20-Nov-2018 JY Added
                //    strTotalsQuery = " SELECT '0.2' AS PayTypeID, 'Tax Collected' AS PayTypeDesc, ISNULL(SUM(PTD.TaxAmount), 0) AS Amt FROM POSTransactionDetail PTD " +
                //    " INNER JOIN POSTransaction PTH ON PTH.TransID = PTD.TransID " +
                //    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                //    strItemFilterSql +
                //    " WHERE convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                //    strTotalsQueryFilter +
                //" UNION " +
                //    "SELECT '0.3' AS PayTypeID, 'PayOut' AS PayTypeDesc, ISNULL(SUM(Amount),0) As Amt FROM Payout PO" +
                //    " WHERE convert(datetime,PO.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                //    strPayoutFilter + 
                //" UNION " +
                //    " SELECT '1.1' AS PayTypeID, 'CC' AS PayTypeDesc, ISNULL(SUM(PTP.Amount),0) AS Amt FROM POSTransaction PTH " +
                //    " INNER JOIN POSTransPayment PTP ON PTH.TransID = PTP.TransID " +
                //    " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PaytypeID " +
                //    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                //    strItemFilterSql +
                //    " WHERE PTP.TransTypeCode IN ('3', '4', '5', '6', '7') " +
                //    " AND convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                //    strTotalsQueryFilter +
                //" UNION " +
                //    " SELECT PTY.PayTypeID, PTY.PayTypeDesc, ISNULL(SUM(PTP.Amount),0) AS Amt FROM POSTransaction PTH " +
                //    " INNER JOIN POSTransPayment PTP ON PTH.TransID = PTP.TransID " +
                //    " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PaytypeID " +
                //    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                //    strItemFilterSql +
                //    " WHERE PTP.TransTypeCode NOT IN('3', '4', '5', '6', '7') " +
                //    " AND convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                //    strTotalsQueryFilter;

                //    strTotalsQuery += " GROUP BY PTY.PayTypeID, PTY.PayTypeDesc";

                strTotalsQuery = "SELECT 1 AS GroupType, 'CC' AS TransType, 'CC' AS PayTypeDesc, ISNULL(SUM(PTP.Amount),0) As TransAmount, 0 As TransCount FROM POSTransaction PTH " +
                    " INNER JOIN POSTransPayment PTP ON PTH.TransID = PTP.TransID " +
                    " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PaytypeID " +
                    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE PTY.PayTypeID IN ('3','4','5','6','7') " +
                    " AND convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter +
                " UNION " +
                    " SELECT 1 AS GroupType, PTY.PayTypeID AS TransType, PTY.PayTypeDesc, ISNULL(SUM(PTP.Amount), 0) As TransAmount, 0 As TransCount FROM POSTransaction PTH " +
                    " INNER JOIN POSTransPayment PTP ON PTH.TransID = PTP.TransID " +
                    " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PaytypeID " +
                    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE PTY.PayTypeID NOT IN ('3','4','5','6','7') " +
                    " AND convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter +
                    " GROUP BY PTY.PayTypeID, PTY.PayType, PTY.PayTypeDesc " +
                " UNION " +
                    " SELECT 3 AS GroupType, 'PO' AS TransType, 'PayOut' AS PayTypeDesc, ISNULL(SUM(Amount), 0) As TransAmount, 0 As TransCount FROM Payout PO " +
                    " WHERE convert(datetime,PO.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strPayoutFilter +
                " UNION " +
                    " SELECT 2 AS GroupType, 'TX' AS TransType, 'Tax' AS PayTypeDesc, ISNULL(SUM(PTD.TaxAmount), 0) As TransAmount, 0 As TransCount FROM POSTransactionDetail PTD " +
                    " INNER JOIN POSTransaction PTH ON PTH.TransID = PTD.TransID  INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter +
                " UNION " +
                    " SELECT 2 AS GroupType, 'DT' AS TransType, 'Discount' AS PayTypeDesc, ISNULL(SUM(PTH.TotalDiscAmount), 0) As TransAmount, 0 As TransCount FROM POSTransaction PTH " +
                    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter +
                " UNION " +
                    " SELECT 2 AS GroupType, 'S' AS TransType, 'Sale' AS PayTypeDesc, ISNULL(SUM(PTH.GrossTotal), 0) As TransAmount, Count(PTH.GrossTotal)As TransCount FROM POSTransaction PTH " +
                    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE PTH.TransType = 1 " +
                    " AND convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter +
                " UNION " +
                    " SELECT 2 AS GroupType, 'SR' AS TransType, 'Return' AS PayTypeDesc, ISNULL(SUM(PTH.GrossTotal), 0) As TransAmount, Count(PTH.GrossTotal) As TransCount FROM POSTransaction PTH " +
                    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE PTH.TransType = 2 " +
                    " AND convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter +
                " UNION " +
                    " SELECT 2 AS GroupType, 'A' AS TransType, 'ROA' AS PayTypeDesc, ISNULL(SUM(PTH.TotalPaid), 0) As TransAmount, Count(PTH.TotalPaid)As TransCount FROM POSTransaction PTH " +
                    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE PTH.TransType = 3 " +
                    " AND convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter +
                " UNION " +
                " SELECT 2 AS GroupType, 'TF' AS TransType, 'Transaction Fee' AS PayTypeDesc, ISNULL(SUM(PTH.TotalTransFeeAmt), 0) As TransAmount, Count(PTH.TotalTransFeeAmt) As TransCount FROM POSTransaction PTH " +
                    " INNER JOIN util_POSSet PS ON ps.stationid = PTH.stationid " +
                    " WHERE convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) " +
                    strTotalsQueryFilter;   //PRIMEPOS-3119 11-Aug-2022 JY Added Transaction Fee sql
                DataTable dtOtherTotals = new DataTable();
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;
                    dtOtherTotals = DataHelper.ExecuteDataTable(conn, CommandType.Text, strTotalsQuery);
                }
                #endregion

                //oRpt.GroupFooterPayment.SectionFormat.EnableSuppress = !chkShowTransactionPayment.Checked;//Commented by Krishna on 23 November 2011
                //Added by Krishna to use the above functionality i.e Groupfooter supression without checkbox as per new changes(DropDown list of Paytypes) on 23 November 2011
                if (this.txtPaymentType.Text.Trim() != "" && txtPaymentType.Tag != null && txtPaymentType.Tag.ToString().Trim().Length > 0)
                    oRpt.GroupFooterPayment.SectionFormat.EnableSuppress = false;
                else
                    oRpt.GroupFooterPayment.SectionFormat.EnableSuppress = true;
                //Till here Added  by krishna

                #region PRIMEPOS-2604 28-Nov-2018 JY Added
                if (this.optSummary.Checked == true)
                {
                    oRpt.PageHeaderSection1.SectionFormat.EnableSuppress = true;
                    oRpt.GroupHeaderSection1.SectionFormat.EnableSuppress = true;
                    oRpt.DetailSection1.SectionFormat.EnableSuppress = true;
                    oRpt.GroupFooterSection1.SectionFormat.EnableSuppress = true;
                    oRpt.GroupFooterPayment.SectionFormat.EnableSuppress = true;
                    if (oRpt.ReportDefinition.ReportObjects["Text1"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["Text1"]).Text = "POS Transaction Summary";
                }
                else
                {
                    if (oRpt.ReportDefinition.ReportObjects["Text1"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["Text1"]).Text = "POS Transaction Detail";
                }
                #endregion

                DataSet dsMainRptSource = clsReports.GetReportSource(strQuery);
                clsReports.DStoExport = dsMainRptSource;

                oRpt.Database.Tables[0].SetDataSource(dsMainRptSource.Tables[0]);
                oRpt.OpenSubreport("rptTransactionDetailPayments").Database.Tables[0].SetDataSource(clsReports.GetReportSource(strSubQuery).Tables[0]);
                oRpt.OpenSubreport("rptTransactionDetailSummary").Database.Tables[0].SetDataSource(dtOtherTotals);
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpSaleStartDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpSaleEndDate.Text, oRpt);
                clsReports.Preview(blnPrint, oRpt);
                /*Search oSearch=new Search();
				DataSet ds = oSearch.Search(strQuery);
				oRpt.SetDataSource(ds);
				frmReportViewer oViewer=new frmReportViewer();
				oViewer.rvReportViewer.ReportSource=oRpt;
				oViewer.MdiParent=frmMain.getInstance();
				oViewer.WindowState=FormWindowState.Maximized;
				oViewer.Show();*/
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            PreviewReport(true);
        }

        private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem();
        }

        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Item_tbl);
                    this.txtItemCode.Focus();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Item_tbl)
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
                        this.txtItemCode.Text = oItemRow.ItemID;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.txtItemCode.Text = String.Empty;
                    SearchItem();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.txtItemCode.Text = String.Empty;
                    SearchItem();
                }
                #endregion
            }
        }

        private void txtPaymentType_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            SearchPayType();
        }

        private void SearchPayType()
        {
            try
            {
                if (this.txtPaymentType.ButtonsRight[0].Enabled == false)
                {
                    return;
                }

                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.PayType_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.PayType_tbl;    //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strPayTypeCode = "";
                    string strPayTypeName = "";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            strPayTypeCode += ",'" + oRow.Cells["Code"].Text + "'";
                            strPayTypeName += "," + oRow.Cells["Description"].Text;
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
                else
                {
                    txtPaymentType.Text = string.Empty;
                    txtPaymentType.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void chkShowTransactionPayment_CheckedChanged(object sender, EventArgs e)
        {
            //this.txtPaymentType.ButtonsRight[0].Enabled = chkShowTransactionPayment.Checked;//Commented by Krishna on 23 November 2011
        }

        //Following Code Added by Krishna on 22 November 2011

        private bool GridVisibleFlag = false;
        private void btnPayTypeList_Click(object sender, EventArgs e)
        {
            if (grpPayTypeList.Visible == false)
                PayTypeList_Expand(true);
            else
            {
                PayTypeList_Expand(false);
                GetSelectedPayTypes();//Added by Krishna on 23 November 2011
            }
        }

        private void PayTypeList_Expand(bool Value)
        {
            if (Value == true)
            {
                if (grpPayTypeList.Visible == false)
                {
                    dataGridList.Visible = true;
                    grpPayTypeList.Visible = true;
                    dataGridList.Height = 230;
                    grpPayTypeList.Height = dataGridList.Height + 31;
                    GridVisibleFlag = true;
                    chkSelectAll.Location = new Point(chkSelectAll.Location.X, dataGridList.Height + 10);
                    grpPayTypeList.Focus();

                    //dataGridList.Focus();
                    //dataGridList.Rows[1].Selected = true;
                }
            }
            else
            {
                if (grpPayTypeList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpPayTypeList.Visible = false;
                    GridVisibleFlag = false;
                    //butVendorList.Focus();
                }
            }
        }

        private void butPayTypeList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                dataGridList.Focus();
            }
        }

        private void butPayTypeList_KeyUp(object sender, KeyEventArgs e)
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
            PayTypeData = SearchSvr.Search(clsPOSDBConstants.PayType_tbl, "", "", 0, -1, 0, false, true); //PRIMEPOS-3282
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
            //int rowsCount = dataGridList.Rows.Count;
            //if (rowsCount > 0)
            //{
            //    PayTypeList_Expand(true);
            //    for (int i = 0; i < rowsCount; i++)
            //    {
            //        dataGridList.Rows[i].Cells["chkbox"].Value = true;
            //        chkSelectAll.Text = "Unselect All";
            //    }
            //    PayTypeList_Expand(false);
            //    GetSelectedPayTypes();
            //}
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
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
            //isPrimeRxPaySelected = false; //PRIEPOS-3282
            foreach (DataGridViewRow oRow in dataGridList.Rows)
            {
                if (POS_Core.Resources.Configuration.convertNullToBoolean(oRow.Cells[0].Value))
                {
                    strPayTypeCode += ",'" + oRow.Cells[1].Value.ToString() + "'";
                    strPayTypeName += "," + oRow.Cells["Description"].Value.ToString();
                    //if (oRow.Cells[1].Value.ToString().Trim() == "O" && oRow.Cells["Description"].Value.ToString().ToUpper() == "PRIMERXPAY") //PRIMEPOS-3282
                    //{
                    //    isPrimeRxPaySelected = true;
                    //}
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
        //Till here Added by Krishna on 22 November 2011

        #region Sprint-19 - 2154 13-Jan-2015 JY Added to populate data in grid
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                oDataSet = new DataSet();
                String strTransSQL, strTransDetailsSQL, strPaymentSQL;

                GenerateSQL(out strTransSQL, out strTransDetailsSQL, out strPaymentSQL);

                oDataSet.Tables.Add(oSearchSvr.Search(strTransSQL).Tables[0].Copy());
                oDataSet.Tables[0].TableName = "Master";

                oDataSet.Tables.Add(oSearchSvr.Search(strTransDetailsSQL).Tables[0].Copy());
                oDataSet.Tables[1].TableName = "Detail";

                oDataSet.Tables.Add(oSearchSvr.Search(strPaymentSQL).Tables[0].Copy());
                oDataSet.Tables[2].TableName = "Payment";

                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["TransID"], oDataSet.Tables[1].Columns["TransID"]);
                oDataSet.Relations.Add("MasterPayment", oDataSet.Tables[0].Columns["TransID"], oDataSet.Tables[2].Columns["TransID"]);

                grdSearch.DataSource = oDataSet;

                grdSearch.DisplayLayout.Bands[0].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 10;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[0].Header.Caption = "Transactions";
                grdSearch.DisplayLayout.Bands[0].Columns["StationID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["UserID"].Header.Caption = "User";

                grdSearch.DisplayLayout.Bands[1].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[1].Header.Caption = "Transaction Detail";
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 10;
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[1].Columns["TransID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Expandable = true;

                grdSearch.DisplayLayout.Bands[2].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[2].Header.Caption = "Payment Information";
                grdSearch.DisplayLayout.Bands[2].Header.Appearance.FontData.SizeInPoints = 10;
                grdSearch.DisplayLayout.Bands[2].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[2].Columns["TransID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[2].Columns["CustomerSign"].Hidden = true;
                grdSearch.DisplayLayout.Bands[2].Columns["BinarySign"].Hidden = true;
                grdSearch.DisplayLayout.Bands[2].Columns["SigType"].Hidden = true;
                grdSearch.DisplayLayout.Bands[2].Columns["PAYMENTPROCESSOR"].Hidden = true;  //PRIMEPOS-2900 16-Sep-2020 JY Added

                if (grdSearch.DisplayLayout.Bands[2].Columns.Exists("ViewSign") == false)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridColumn oCol = grdSearch.DisplayLayout.Bands[2].Columns.Add("ViewSign");
                    oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    oCol.Header.Caption = "View Sign";
                }

                this.resizeColumns();
                grdSearch.Focus();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSearch.Refresh();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void GenerateSQL(out String strTransSQL, out string strTransDetailsSQL, out string strPaymentSQL)
        {
            string strWhereClause = " WHERE convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113)";

            strTransSQL = "SELECT DISTINCT PT.TransID, PT.UserID, PT.TransDate, Case PT.TransType when 1 Then 'Sale' when 2 Then 'Return' end as TransType, PT.StationID, ps.StationName, PT.StClosedID" +
                    " , PT.EODID " + //PRIMEPOS-2393 14-May-2019 JY Added
                    " FROM postransaction PT " +
                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                    " INNER JOIN Item I ON I.ItemID=PTD.ItemID " +
                    " INNER JOIN util_POSSet ps ON ps.stationid=pt.stationid " +
                    " WHERE convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

            strTransDetailsSQL = "select PT.TransID, I.ItemID, PTD.Qty, PTD.DISCOUNT, PTD.ItemDescription as Description, PTD.Price, PTD.TaxAMOUNT, PTD.ExtendedPrice " +
                    " from postransaction PT " +
                    " INNER JOIN POSTransactionDetail PTD ON PT.TransID=PTD.TransID " +
                    " INNER JOIN Item I ON I.ItemID=PTD.ItemID " +
                    " INNER JOIN util_POSSet ps ON ps.stationid=pt.stationid " +
                    " WHERE convert(datetime,PT.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

            //PRIMEPOS-2900 16-Sep-2020 JY Added PAYMENTPROCESSOR column
            strPaymentSQL = "select PTH.TransID, PTY.PayTypeDesc as [Payment Type], PTP.Amount, case CHARINDEX('|',isnull(PTP.refno,'')) " +
                " when 0 then PTP.refno else '******'+right(rtrim(left(PTP.refno,CHARINDEX('|',PTP.refno)-1)) ,4) End as [Ref No.], PTP.CustomerSign, PTP.BinarySign, PTP.SigType, PTP.PAYMENTPROCESSOR " +
                " from POSTransaction PTH " +
                " INNER JOIN postranspayment PTP ON PTH.TransID = PTP.TransID " +
                " INNER JOIN PayType PTY ON PTP.TransTypeCode = PTY.PayTypeID " +
                " INNER JOIN Customer Cus ON PTH.CustomerID = Cus.CustomerID " +
                " INNER JOIN util_POSSet ps ON ps.stationid = PTH.stationid " +
                " INNER JOIN (SELECT DISTINCT PTD.TransID from POSTransactionDetail PTD INNER JOIN Item I ON I.ItemID = PTD.ItemID) a ON a.TransID = PTH.TransID " +
                " WHERE convert(datetime,PTH.TransDate,109) between convert(datetime, cast('" + this.dtpSaleStartDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + this.dtpSaleEndDate.Text + " 23:59:59' as datetime) ,113) ";

            if (this.txtItemCode.Text.Trim() != "")
            {
                //strItemFilter = " WHERE I.ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "' ";    //PRIMEPOS-1637 26-May-2021 JY Commented
                strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
                strTransSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
                strTransDetailsSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
                strPaymentSQL += " AND PTH.TransID IN (SELECT TransId FROM POSTransactionDetail WHERE ItemID ='" + this.txtItemCode.Text.Trim().Replace("'", "''") + "')";
            }

            if (this.txtPaymentType.Text.Trim() != "" && txtPaymentType.Tag != null && txtPaymentType.Tag.ToString().Trim().Length > 0)
            {
                //if (isPrimeRxPaySelected) //PRIMEPOS-3282 
                if (chkPrimeRxPayTrans.Checked) //PRIMEPOS-3282
                {
                    //strWhereClause += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                    //strTransSQL += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                    //strTransDetailsSQL += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                    //strPaymentSQL += " AND (PAYMENTPROCESSOR='PRIMERXPAY') ";
                    strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                    strTransSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                    strTransDetailsSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                    strPaymentSQL += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                }
                else
                {
                    strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                    strTransSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                    strTransDetailsSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                    strPaymentSQL += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE TransTypeCode in (" + this.txtPaymentType.Tag.ToString().Trim() + ") )";
                }
            }
            else if (chkPrimeRxPayTrans.Checked) //PRIMEPOS-3282
            {
                strWhereClause += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                strTransSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                strTransDetailsSQL += " AND PT.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
                strPaymentSQL += " AND PTH.TransID IN (SELECT TransId FROM POSTransPayment WHERE PAYMENTPROCESSOR='PRIMERXPAY' )";
            }

            if (this.txtUserID.Text.Trim() != "")
            {
                strWhereClause += " and PT.UserID='" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                strTransSQL += " and PT.UserID='" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                strTransDetailsSQL += " and PT.UserID='" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
                strPaymentSQL += " and PTH.UserID='" + this.txtUserID.Text.Trim().Replace("'", "''") + "' ";
            }

            if (this.cboTransType.SelectedIndex == 1)
            {
                strWhereClause += " and PT.TransType=1";
                strTransSQL += " and PT.TransType=1";
                strTransDetailsSQL += " and PT.TransType=1";
                strPaymentSQL += " and PTH.TransType=1";
            }
            else if (this.cboTransType.SelectedIndex == 2)
            {
                strWhereClause += " and PT.TransType=2";
                strTransSQL += " and PT.TransType=2";
                strTransDetailsSQL += " and PT.TransType=2";
                strPaymentSQL += " and PTH.TransType=2";
            }

            if (this.txtStationID.Text.Trim() != "")
            {
                strWhereClause += " and Ps.StationID='" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
                strTransSQL += " and Ps.StationID='" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
                strTransDetailsSQL += " and Ps.StationID='" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
                strPaymentSQL += " and Ps.StationID='" + this.txtStationID.Text.Trim().Replace("'", "''") + "' ";
            }

            if (!string.IsNullOrWhiteSpace(this.txtCloseStationId.Text))    //PRIMEPOS-2494 27-Feb-2018 JY Added
            {
                strWhereClause += " and PT." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                strTransSQL += " and PT." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                strTransDetailsSQL += " and PT." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
                strPaymentSQL += " and PTH." + clsPOSDBConstants.TransHeader_Fld_StClosedID + " IN (" + this.txtCloseStationId.Text.Trim().Replace("'", "''") + ")";
            }

            if (!string.IsNullOrWhiteSpace(this.txtEODId.Text))    //PRIMEPOS-2393 14-May-2019 JY Added
            {
                strWhereClause += " and PT." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                strTransSQL += " and PT." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                strTransDetailsSQL += " and PT." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
                strPaymentSQL += " and PTH." + clsPOSDBConstants.TransHeader_Fld_EODID + " IN (" + this.txtEODId.Text.Trim().Replace("'", "''") + ")";
            }

            if (chkOnlyDiscountedTrans.Checked) //PRIMEPOS-2561 01-Aug-2018 JY Added condition to consider only discounted transactions
            {
                strWhereClause += " AND PT.TotalDiscAmount <> 0 ";
                strTransSQL += " AND PT.TotalDiscAmount <> 0 ";
                strTransDetailsSQL += " AND PT.TotalDiscAmount <> 0 ";
                strPaymentSQL += " AND PTH.TotalDiscAmount <> 0";
            }

            #region PRIMEPOS-2465 12-Mar-2020 JY Added
            if (chkConsiderPaidAmount.Checked)
            {
                decimal nFromAmt = Configuration.convertNullToDecimal(numAmountFrom.Value);
                decimal nToAmt = Configuration.convertNullToDecimal(numAmountTo.Value);
                if (nFromAmt < nToAmt)
                {
                    strWhereClause += " AND PT.TotalPaid >= " + nFromAmt + " AND PT.TotalPaid <= " + nToAmt;
                    strTransSQL += " AND PT.TotalPaid >= " + nFromAmt + " AND PT.TotalPaid <= " + nToAmt;
                    strTransDetailsSQL += " AND PT.TotalPaid >= " + nFromAmt + " AND PT.TotalPaid <= " + nToAmt;
                    strPaymentSQL += " AND PTH.TotalPaid >= " + nFromAmt + " AND PTH.TotalPaid <= " + nToAmt;
                }
                else
                {
                    strWhereClause += " AND PT.TotalPaid >= " + nToAmt + " AND PT.TotalPaid <= " + nFromAmt;
                    strTransSQL += " AND PT.TotalPaid >= " + nToAmt + " AND PT.TotalPaid <= " + nFromAmt;
                    strTransDetailsSQL += " AND PT.TotalPaid >= " + nToAmt + " AND PT.TotalPaid <= " + nFromAmt;
                    strPaymentSQL += " AND PTH.TotalPaid >= " + nToAmt + " AND PTH.TotalPaid <= " + nFromAmt;
                }
            }
            #endregion

            #region PRIMEPOS-1637 26-May-2021 JY Added
            string strTransIds = "-999", sSQL = string.Empty;
            try
            {
                if (chkControlDrugs.Checked == true)
                {
                    sSQL = "Select PTD.TransID, PTDrx.RXNo, PTDrx.NRefill FROM POSTransaction PT" +
                            " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID" +
                            " INNER JOIN POSTransactionRXDetail PTDrx ON PTDrx.TransDetailID = PTD.TransDetailID" +
                            " INNER JOIN util_POSSet PS ON PS.stationid = PT.stationid";
                    sSQL += strWhereClause;
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
                    strTransSQL += " AND PT.TransID IN (" + strTransIds + ")";
                    strTransDetailsSQL += " AND PT.TransID IN (" + strTransIds + ")";
                    strPaymentSQL += " AND PTH.TransID IN (" + strTransIds + ")";
                }
            }
            catch (Exception Ex)
            {
            }
            #endregion           
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
            catch (Exception) { }
        }

        private void grdSearch_DoubleClick(object sender, EventArgs e)
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
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        private void grdSearch_BeforeRowExpanded(object sender, CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
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

        private void grdSearch_MouseMove(object sender, MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
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

        private void grdSearch_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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
                //if (this.grdSearch.DisplayLayout.Bands[0].Columns.Count > 0)
                //{
                //    ColorSchemeForViewPOSTrans oColorSchemeForViewPOSTrans = new ColorSchemeForViewPOSTrans();
                //    ColorSchemeForViewPOSTransData oColorSchemeForViewPOSTransData = oColorSchemeForViewPOSTrans.PopulateList("");
                //    if (oColorSchemeForViewPOSTransData != null)
                //    {
                //        if (oColorSchemeForViewPOSTransData.ColorSchemeForViewPOSTrans.Rows.Count > 0)
                //        {
                //            decimal FromAmt = 0;
                //            decimal ToAmt = 0;

                //            foreach (ColorSchemeForViewPOSTransRow oRow in oColorSchemeForViewPOSTransData.ColorSchemeForViewPOSTrans.Rows)
                //            {
                //                FromAmt = oRow.FromAmount;
                //                ToAmt = oRow.ToAmount;

                //                Color BackColor = Configuration.ExtractColor(oRow.BackColor);
                //                Color ForeColor = Configuration.ExtractColor(oRow.ForeColor);

                //                foreach (UltraGridRow oGridRow in grdSearch.Rows)
                //                {
                //                    if (Configuration.convertNullToDecimal(oGridRow.Cells["Tendered Amt"].Value.ToString()) >= FromAmt && Configuration.convertNullToDecimal(oGridRow.Cells["Tendered Amt"].Value.ToString()) <= ToAmt)
                //                    {
                //                        oGridRow.Appearance.BackColor = BackColor;
                //                        oGridRow.Appearance.ForeColor = ForeColor;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                //Following Code Added by Krishna on 24 June 2011
                //for (int i = 0; i < this.grdSearch.Rows.Count; i++)
                //{
                //    if (this.grdSearch.Rows[i].Cells["Trans Type"].Text == "Sale")
                //    {
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor = Color.Green;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor2 = Color.SeaGreen;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                //    }
                //    else if (this.grdSearch.Rows[i].Cells["Trans Type"].Text == "Return")
                //    {
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor = Color.Maroon;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor2 = Color.Red;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                //    }
                //    else if (this.grdSearch.Rows[i].Cells["Trans Type"].Text == "Receive on Account")
                //    {
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor = Color.Purple;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackColor2 = Color.MediumPurple;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                //        this.grdSearch.Rows[i].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                //    }
                //}
                //for (int i = 0; i < this.oDataSet.Tables["Payment"].Rows.Count; i++)
                //{
                //    string str = oDataSet.Tables["Payment"].Rows[i]["IsIIASPayment"].ToString();
                //    string transId = oDataSet.Tables["Payment"].Rows[i]["transid"].ToString();
                //    if (oDataSet.Tables["Payment"].Rows[i]["IsIIASPayment"].ToString() != "False")
                //    {
                //        DataRow row = oDataSet.Tables["Payment"].Rows[i].GetParentRow("MasterPayment");
                //        int k = oDataSet.Tables["Master"].Rows.IndexOf(row);
                //        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.BackColor = Color.CornflowerBlue;
                //        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.BackColor2 = Color.SteelBlue;
                //        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                //        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.ForeColor = Color.White;
                //        this.grdSearch.Rows[k].Cells["Trans Type"].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                //    }
                //}
                //Till here Added by Krishna on 24 June 2011
            }
            catch (Exception) { }
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
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchCloseStationId()
        {
            try
            {
                frmViewEODStation ofrmViewEODStation = new frmViewEODStation(clsPOSDBConstants.StationCloseHeader_tbl);
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
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtCloseStationId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char Delete = (char)8;
            char comma = (char)',';
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != comma;
        }
        #endregion

        private void cboTransType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboTransType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        #region PRIMEPOS-2393 14-May-2019 JY Added
        private void txtEODId_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            try
            {
                SearchEODId();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
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
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtEODId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char Delete = (char)8;
            char comma = (char)',';
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete && e.KeyChar != comma;
        }
        #endregion

        #region PRIMEPOS-2465 12-Mar-2020 JY Added
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
    }
}
