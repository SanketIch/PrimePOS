using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using Infragistics.Win;
using POS_Core_UI.Reports.Reports;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.ReportsUI;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmIIASItemByTrans : System.Windows.Forms.Form
    {
        public bool IsCanceled = true;
    
        #region builtin var

        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
        private IContainer components;
        private GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraLabel lbl2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboCategory;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboSubCategory;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private GroupBox gbInventoryReceived;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeptCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;

        public DataTable SelectedData = null;
        #endregion

        public frmIIASItemByTrans()
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
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIIASItemByTrans));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cboCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboSubCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDeptCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSubCategory)).BeginInit();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).BeginInit();
            this.SuspendLayout();
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(11, 285);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(425, 57);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance1;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(142, 20);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraButton1.Appearance = appearance2;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton1.Location = new System.Drawing.Point(327, 20);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 11;
            this.ultraButton1.Text = "&Close";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance3;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(234, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 9;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lbl2
            // 
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance4;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(24, 168);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(117, 18);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "Sub-Category";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(24, 138);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(73, 18);
            this.ultraLabel1.TabIndex = 21;
            this.ultraLabel1.Text = "Category";
            // 
            // cboCategory
            // 
            this.cboCategory.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboCategory.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboCategory.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCategory.Location = new System.Drawing.Point(146, 135);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(248, 23);
            this.cboCategory.TabIndex = 3;
            this.cboCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboCategory_KeyDown);
            this.cboCategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCategory_KeyPress);
            // 
            // cboSubCategory
            // 
            this.cboSubCategory.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboSubCategory.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboSubCategory.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSubCategory.Location = new System.Drawing.Point(146, 165);
            this.cboSubCategory.Name = "cboSubCategory";
            this.cboSubCategory.Size = new System.Drawing.Size(248, 23);
            this.cboSubCategory.TabIndex = 4;
            this.cboSubCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboSubCategory_KeyDown);
            this.cboSubCategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboSubCategory_KeyPress);
            // 
            // lblTransactionType
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.ForeColorDisabled = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance5;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(12, 12);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 30);
            this.lblTransactionType.TabIndex = 32;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "IIAS Item Transaction Summary";
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.cboSubCategory);
            this.gbInventoryReceived.Controls.Add(this.lbl2);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.cboCategory);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.txtItemCode);
            this.gbInventoryReceived.Controls.Add(this.txtDeptCode);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel11);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(12, 48);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(424, 231);
            this.gbInventoryReceived.TabIndex = 33;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Text = "Report Criteria";
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.ItalicAsString = "False";
            appearance6.FontData.StrikeoutAsString = "False";
            appearance6.FontData.UnderlineAsString = "False";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance6;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(146, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(192, 21);
            this.dtpToDate.TabIndex = 2;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance7.FontData.BoldAsString = "False";
            appearance7.FontData.ItalicAsString = "False";
            appearance7.FontData.StrikeoutAsString = "False";
            appearance7.FontData.UnderlineAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance7;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(146, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(192, 21);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // txtItemCode
            // 
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            editorButton1.Appearance = appearance8;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtItemCode.ButtonsRight.Add(editorButton1);
            this.txtItemCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.Location = new System.Drawing.Point(146, 107);
            this.txtItemCode.MaxLength = 20;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(248, 20);
            this.txtItemCode.TabIndex = 4;
            this.txtItemCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            // 
            // txtDeptCode
            // 
            this.txtDeptCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            editorButton2.Appearance = appearance9;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton2.Text = "";
            this.txtDeptCode.ButtonsRight.Add(editorButton2);
            this.txtDeptCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeptCode.Location = new System.Drawing.Point(146, 80);
            this.txtDeptCode.MaxLength = 20;
            this.txtDeptCode.Name = "txtDeptCode";
            this.txtDeptCode.Size = new System.Drawing.Size(248, 20);
            this.txtDeptCode.TabIndex = 3;
            this.txtDeptCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDeptCode_EditorButtonClick);
            this.txtDeptCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeptCode_Validating);
            // 
            // ultraLabel11
            // 
            this.ultraLabel11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel11.Location = new System.Drawing.Point(24, 111);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel11.TabIndex = 4;
            this.ultraLabel11.Text = "Item Code";
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel12.Location = new System.Drawing.Point(24, 83);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel12.TabIndex = 3;
            this.ultraLabel12.Text = "Department";
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(24, 55);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(24, 27);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "From Date";
            // 
            // frmIIASItemByTrans
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.ClientSize = new System.Drawing.Size(447, 356);
            this.Controls.Add(this.gbInventoryReceived);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIIASItemByTrans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IIAS Item Transaction Summary";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmSearchMain_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSubCategory)).EndInit();
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        
        public DataSet Search()
        {
            DataSet oData = new DataSet();

            Search oSearch=new Search();
            string sSQL = "Select UPCCode, iias.Description, CategoryDescriptor,SubCategoryDescriptor, Sum(PTD.Qty) As TotalQtySold, Sum(PTD.ExtendedPrice) TotalSalePrice "
                + " From IIAS_items iias Inner Join Item I On (iias.UPCCode=I.ItemID) Inner Join POSTransactionDetail PTD On (PTD.ItemID=I.ItemID) "
                + " Left Outer Join Department Dept On (Dept.DeptID=I.DepartmentID) "
                + " Inner Join POSTransaction PT On (PT.TransID=PTD.TransID) "
                + " Where PTD.IsIIAS=1 And PTD.TransID In (Select TransID From POSTransPayment PTP Where IsIIASPayment=1) ";
                
                //Group By UPCCode, iias.Description, CategoryDescriptor,SubCategoryDescriptor, ChangeIndicator, 

            if (dtpFromDate.Value.ToString() != "")
                sSQL = sSQL + " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) >= '" + dtpFromDate.Text + "'";

            if (dtpToDate.Value.ToString() != "")
                sSQL = sSQL + " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) <= '" + dtpToDate.Text + "'";

            if (txtDeptCode.Tag != null)
                sSQL = sSQL + " AND Dept.DeptCode = '" + txtDeptCode.Tag.ToString() + "'";

            if (txtItemCode.Text.Trim().Length > 0)
            {
                sSQL += " And I.ItemID= '" + txtItemCode.Text.ToString().Replace("'", "''") + "'";
            }

            if(cboCategory.Text.Trim().Length>0)
            {
                sSQL+=" And IIAS.CategoryDescriptor Like '" + cboCategory.Text.Replace("'","''") + "%'";
            }

            if(cboSubCategory.Text.Trim().Length>0)
            {
                sSQL+=" And IIAS.SubCategoryDescriptor Like '" + cboSubCategory.Text.Replace("'","''") + "%'";
            }

            sSQL += " Group By UPCCode, iias.Description, CategoryDescriptor,SubCategoryDescriptor ";
            sSQL+=" Order by UPCCode ";

            oData= oSearch.SearchData(sSQL);

            return oData;
        }
        
        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                clsUIHelper.setColorSchecme(this);
                dtpFromDate.Value = DateTime.Now;
                dtpToDate.Value = DateTime.Now;

                this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.dtpFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.dtpFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.dtpToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.dtpToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                Item oItem = new Item();

                this.cboCategory.DataSource = oItem.GetIIAS_Category_Descriptor();
                this.cboSubCategory.DataSource = oItem.GetIIAS_SubCategory_Descriptor();

                this.dtpFromDate.Focus();
                
                clsUIHelper.setColorSchecme(this);
            }
            catch (Exception exp)
            {
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
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void grdSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
        }

        private void frmSearchMain_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtDeptCode.ContainsFocus)
                        txtDeptCode_EditorButtonClick(null, null);
                    else if (txtItemCode.ContainsFocus)
                        txtItemCode_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSearchMain_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Preview(false);
        }

        private void Preview(bool PrintIt)
        {
            try
            {
                ReportClass oRpt = null;
                DataSet oDS = Search();
                oRpt=new rptIIASTransByItem();
                
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpToDate.Text, oRpt);

                clsReports.Preview(PrintIt, oDS, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);
        }

        private void txtDeptCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
            if (oSearch.IsCanceled)
            {
                txtDeptCode.Text = "";
                txtDeptCode.Tag = null;
            }
            else
            {
                txtDeptCode.Tag = oSearch.SelectedRowID();
                txtDeptCode.Text = oSearch.SelectedCode();
            }
        }

        private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
            if (oSearch.IsCanceled) return;
            txtItemCode.Text = oSearch.SelectedRowID();
        }

        private void txtDeptCode_Validating(object sender, CancelEventArgs e)
        {
            if (txtDeptCode.Text.Trim() == "")
            {
                txtDeptCode.Tag = null;
            }
        }

        private void cboCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboSubCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void cboSubCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
