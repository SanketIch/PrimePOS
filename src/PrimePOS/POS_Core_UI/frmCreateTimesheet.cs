using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
using System.Timers;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPayOut.
	/// </summary>
	public class frmCreateTimesheet : System.Windows.Forms.Form
    {
        #region declarations
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
//		private static frmfunctionKeySelection oFunctionKeySelection=new frmfunctionKeySelection();
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
        private GroupBox gbInventoryReceived;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel lblUserDescription;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;
        private IContainer components;
        #endregion
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIncludeProcessedTimesheet;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private System.Data.DataSet oSearchData=null;

        #region PRIMEPOS-189 06-Aug-2021 JY Added
        int nDisplayHourlyRate = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion

        public frmCreateTimesheet()
		{
			//
			// Required for Windows Form Designer support
			//
			try
			{
				InitializeComponent();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void ApplyGrigFormat()
		{

			//grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_KeyId].Hidden =  true;

			grdDetail.DisplayLayout.Bands[0].Columns["TimeIn"].Header.Caption =  "Time In";
            grdDetail.DisplayLayout.Bands[0].Columns["TimeOut"].Header.Caption = "Time Out";

			grdDetail.DisplayLayout.Bands[0].Columns["TimeIn"].MaxLength = 300;
            grdDetail.DisplayLayout.Bands[0].Columns["TimeOut"].MaxLength = 300;

            grdDetail.DisplayLayout.Bands[0].Columns["TimeOutID"].Hidden=true;
            grdDetail.DisplayLayout.Bands[0].Columns["TimeInID"].Hidden = true;
            grdDetail.DisplayLayout.Bands[0].Columns["HourlyRate"].Hidden = true;   //PRIMEPOS-189 05-Aug-2021 JY Added
            if (nDisplayHourlyRate == 0)
                grdDetail.DisplayLayout.Bands[0].Columns["Total Earnings"].Hidden = true;   //PRIMEPOS-189 05-Aug-2021 JY Added
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void Save()
		{
			try
			{
                Timesheet oTimesheet = new Timesheet();
                oTimesheet.AddToTimesheet(oSearchData);
                SearchData();
                if (oSearchData.Tables[0].Rows.Count > 0)
                {
                    this.grdDetail.PerformAction(UltraGridAction.FirstCellInGrid);
                    if (this.grdDetail.ActiveRow == null) return;   //Sprint-19 - 2053 12-Mar-2015 JY Added 
                    if (string.IsNullOrEmpty(this.grdDetail.Rows[0].Cells["TimeIn"].Text))
                    {
                        this.grdDetail.ActiveCell = this.grdDetail.ActiveRow.Cells["TimeIn"];
                        this.grdDetail.ActiveCell.Activate();
                        this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                        this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                    }
                    else if (string.IsNullOrEmpty(this.grdDetail.Rows[0].Cells["TimeOut"].Text))
                    {
                        this.grdDetail.ActiveCell = this.grdDetail.ActiveRow.Cells["TimeOut"];
                        this.grdDetail.ActiveCell.Activate();
                        this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                        this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                    }
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Key");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Operation");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateTimesheet));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeIn");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeOut");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Total Hours", 0);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Edit", 1);
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Total Earnings", 2);
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.chkIncludeProcessedTimesheet = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.lblUserDescription = new Infragistics.Win.Misc.UltraLabel();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.gbInventoryReceived.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeProcessedTimesheet)).BeginInit();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraDateTimeEditor1
            // 
            this.ultraDateTimeEditor1.AlwaysInEditMode = true;
            this.ultraDateTimeEditor1.AutoSize = false;
            this.ultraDateTimeEditor1.DateTime = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(238, -473);
            this.ultraDateTimeEditor1.MaskInput = "{LOC}mm/dd/yyyy hh:mm";
            this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
            this.ultraDateTimeEditor1.Size = new System.Drawing.Size(181, 25);
            this.ultraDateTimeEditor1.TabIndex = 4;
            this.ultraDateTimeEditor1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDateTimeEditor1.Value = new System.DateTime(2015, 3, 9, 0, 0, 0, 0);
            this.ultraDateTimeEditor1.Visible = false;
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            this.ultraDataSource2.Rows.AddRange(new object[] {
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F2"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F3"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F4"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F5"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F6"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F7"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F8"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F9"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F10"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F11"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F12"))})});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(718, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnSave.Appearance = appearance2;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(616, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.ForeColorDisabled = System.Drawing.Color.White;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance3;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(834, 30);
            this.lblTransactionType.TabIndex = 25;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Create Timesheet";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdDetail);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(817, 340);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // grdDetail
            // 
            this.grdDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BackColorDisabled = System.Drawing.Color.White;
            appearance4.BackColorDisabled2 = System.Drawing.Color.White;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance4;
            this.grdDetail.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance5.TextHAlignAsString = "Left";
            appearance5.TextVAlignAsString = "Middle";
            ultraGridColumn1.CellAppearance = appearance5;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 125;
            ultraGridColumn2.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance6.Image = global::POS_Core_UI.Properties.Resources.search;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Right;
            appearance6.TextHAlignAsString = "Left";
            appearance6.TextVAlignAsString = "Middle";
            ultraGridColumn2.CellAppearance = appearance6;
            ultraGridColumn2.EditorComponent = this.ultraDateTimeEditor1;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 151;
            ultraGridColumn3.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance7.Image = global::POS_Core_UI.Properties.Resources.search;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Right;
            appearance7.TextHAlignAsString = "Left";
            appearance7.TextVAlignAsString = "Middle";
            ultraGridColumn3.CellAppearance = appearance7;
            ultraGridColumn3.EditorComponent = this.ultraDateTimeEditor1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 151;
            ultraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance8.TextHAlignAsString = "Right";
            appearance8.TextVAlignAsString = "Middle";
            ultraGridColumn4.CellAppearance = appearance8;
            ultraGridColumn4.DataType = typeof(decimal);
            ultraGridColumn4.Format = "##,##,##0.00";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.MaskInput = "{currency:9.2}";
            ultraGridColumn4.Width = 101;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn5.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance9.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            ultraGridColumn5.CellAppearance = appearance9;
            ultraGridColumn5.Header.Caption = "";
            ultraGridColumn5.Header.VisiblePosition = 5;
            ultraGridColumn5.NullText = "Edit";
            ultraGridColumn5.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn5.TabStop = false;
            ultraGridColumn5.Width = 115;
            ultraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance10.TextHAlignAsString = "Right";
            appearance10.TextVAlignAsString = "Middle";
            ultraGridColumn6.CellAppearance = appearance10;
            ultraGridColumn6.DataType = typeof(decimal);
            ultraGridColumn6.Format = "##,##,##0.00";
            ultraGridColumn6.Header.VisiblePosition = 4;
            ultraGridColumn6.MaskInput = "{currency:9.2}";
            ultraGridColumn6.Width = 121;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance13;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance16;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            appearance18.BackColorDisabled = System.Drawing.Color.White;
            appearance18.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance19.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.FontData.BoldAsString = "True";
            appearance20.FontData.SizeInPoints = 10F;
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.TextHAlignAsString = "Left";
            appearance20.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance20;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDetail.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance22.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            appearance24.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance24;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.grdDetail.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance25.BackColor = System.Drawing.Color.Navy;
            appearance25.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.Navy;
            appearance26.BackColorDisabled = System.Drawing.Color.Navy;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            appearance26.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance26;
            this.grdDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance27;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance28.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance28;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(13, 14);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(795, 319);
            this.grdDetail.TabIndex = 4;
            this.grdDetail.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDetail_InitializeLayout);
            this.grdDetail.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdDetail_InitializeRow);
            this.grdDetail.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDetail_AfterRowUpdate);
            this.grdDetail.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_CellChange);
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdDetail_Error);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            this.grdDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdDetail_KeyDown);
            this.grdDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grdDetail_KeyPress);
            this.grdDetail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmFunctionKeys_KeyUp);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ultraDateTimeEditor1);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 488);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(817, 50);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.ultraPanel3);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel3);
            this.gbInventoryReceived.Controls.Add(this.chkIncludeProcessedTimesheet);
            this.gbInventoryReceived.Controls.Add(this.ultraPanel2);
            this.gbInventoryReceived.Controls.Add(this.ultraPanel1);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel2);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.btnSearch);
            this.gbInventoryReceived.Controls.Add(this.txtUserID);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.lblUserDescription);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Location = new System.Drawing.Point(10, 30);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(817, 120);
            this.gbInventoryReceived.TabIndex = 73;
            this.gbInventoryReceived.TabStop = false;
            // 
            // ultraPanel3
            // 
            appearance29.BackColor = System.Drawing.Color.Yellow;
            this.ultraPanel3.Appearance = appearance29;
            this.ultraPanel3.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraPanel3.Location = new System.Drawing.Point(6, 98);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(15, 15);
            this.ultraPanel3.TabIndex = 20;
            // 
            // ultraLabel3
            // 
            appearance30.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance30;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(27, 98);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(173, 15);
            this.ultraLabel3.TabIndex = 19;
            this.ultraLabel3.Text = "Processed Timesheet Records";
            // 
            // chkIncludeProcessedTimesheet
            // 
            appearance31.ForeColor = System.Drawing.Color.White;
            this.chkIncludeProcessedTimesheet.Appearance = appearance31;
            this.chkIncludeProcessedTimesheet.BackColor = System.Drawing.Color.Transparent;
            this.chkIncludeProcessedTimesheet.BackColorInternal = System.Drawing.Color.Transparent;
            this.chkIncludeProcessedTimesheet.Location = new System.Drawing.Point(492, 39);
            this.chkIncludeProcessedTimesheet.Name = "chkIncludeProcessedTimesheet";
            this.chkIncludeProcessedTimesheet.Size = new System.Drawing.Size(191, 20);
            this.chkIncludeProcessedTimesheet.TabIndex = 1;
            this.chkIncludeProcessedTimesheet.Text = "Include processed timesheet";
            this.chkIncludeProcessedTimesheet.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIncludeProcessedTimesheet.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraPanel2
            // 
            appearance32.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ultraPanel2.Appearance = appearance32;
            this.ultraPanel2.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraPanel2.Location = new System.Drawing.Point(6, 81);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(15, 15);
            this.ultraPanel2.TabIndex = 18;
            // 
            // ultraPanel1
            // 
            appearance33.BackColor = System.Drawing.Color.LightPink;
            this.ultraPanel1.Appearance = appearance33;
            this.ultraPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraPanel1.Location = new System.Drawing.Point(6, 64);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(15, 15);
            this.ultraPanel1.TabIndex = 17;
            // 
            // ultraLabel2
            // 
            appearance34.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance34;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(27, 81);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(537, 15);
            this.ultraLabel2.TabIndex = 16;
            this.ultraLabel2.Text = "Please correct the Timein/TimeOut as difference between them should be less than " +
    "24 hours.";
            // 
            // ultraLabel1
            // 
            appearance35.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance35;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(27, 64);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(354, 15);
            this.ultraLabel1.TabIndex = 14;
            this.ultraLabel1.Text = "Please correct the TimeIn which is greater than the TimeOut.";
            // 
            // btnSearch
            // 
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance36.FontData.BoldAsString = "True";
            appearance36.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance36;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(690, 35);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 28);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            editorButton1.Appearance = appearance37;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtUserID.ButtonsRight.Add(editorButton1);
            this.txtUserID.Location = new System.Drawing.Point(90, 13);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(148, 20);
            this.txtUserID.TabIndex = 0;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtUserID.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtUserID_EditorButtonClick);
            this.txtUserID.Leave += new System.EventHandler(this.txtUserID_Leave);
            // 
            // ultraLabel12
            // 
            appearance38.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance38;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(6, 15);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(60, 15);
            this.ultraLabel12.TabIndex = 9;
            this.ultraLabel12.Text = "Employee";
            // 
            // lblUserDescription
            // 
            this.lblUserDescription.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblUserDescription.Location = new System.Drawing.Point(238, 13);
            this.lblUserDescription.Name = "lblUserDescription";
            this.lblUserDescription.Size = new System.Drawing.Size(245, 20);
            this.lblUserDescription.TabIndex = 11;
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance39.FontData.BoldAsString = "False";
            appearance39.FontData.ItalicAsString = "False";
            appearance39.FontData.StrikeoutAsString = "False";
            appearance39.FontData.UnderlineAsString = "False";
            appearance39.ForeColor = System.Drawing.Color.Black;
            appearance39.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance39;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(335, 39);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(148, 21);
            this.dtpToDate.TabIndex = 3;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance40.FontData.BoldAsString = "False";
            appearance40.FontData.ItalicAsString = "False";
            appearance40.FontData.StrikeoutAsString = "False";
            appearance40.FontData.UnderlineAsString = "False";
            appearance40.ForeColor = System.Drawing.Color.Black;
            appearance40.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance40;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(90, 37);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(148, 21);
            this.dtpFromDate.TabIndex = 2;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel13
            // 
            appearance41.ForeColor = System.Drawing.Color.White;
            this.ultraLabel13.Appearance = appearance41;
            this.ultraLabel13.AutoSize = true;
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(273, 41);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(49, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            appearance42.ForeColor = System.Drawing.Color.White;
            this.ultraLabel14.Appearance = appearance42;
            this.ultraLabel14.AutoSize = true;
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(6, 40);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(64, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "From Date";
            // 
            // lblMessage
            // 
            appearance43.ForeColor = System.Drawing.Color.Red;
            appearance43.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance43;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 541);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(834, 20);
            this.lblMessage.TabIndex = 74;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "\"Total Earnings\" is hidden due to the logged-in user does not have enough permiss" +
    "ions.";
            this.lblMessage.Visible = false;
            // 
            // frmCreateTimesheet
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(834, 561);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.gbInventoryReceived);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCreateTimesheet";
            this.Text = " Manage / Create Timesheet";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.frmCreateTimesheet_HelpButtonClicked);
            this.Activated += new System.EventHandler(this.frmFunctionKeys_Activated);
            this.Load += new System.EventHandler(this.frmFunctionKeys_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmFunctionKeys_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeProcessedTimesheet)).EndInit();
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			Save();
		}

        private void frmFunctionKeys_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (this.grdDetail.ContainsFocus == false)
                {
                    if (e.KeyData == System.Windows.Forms.Keys.Enter)
                    {
                        this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

		private void frmFunctionKeys_Load(object sender, System.EventArgs e)
		{
			this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			SetKeyActionMappings(this.grdDetail);
			clsUIHelper.SetAppearance(this.grdDetail);
			clsUIHelper.setColorSchecme(this);

            this.dtpFromDate.Value = DateTime.Now;
            this.dtpToDate.Value = DateTime.Now;

            #region PRIMEPOS-189 06-Aug-2021 JY Added            
            nDisplayHourlyRate = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.DisplayHourlyRate.ID));
            if (nDisplayHourlyRate == 0)
            {
                lblMessage.Visible = true;
                tmrBlinking = new System.Timers.Timer();
                tmrBlinking.Interval = 1000;//1 seconds
                tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
                tmrBlinking.Enabled = true;
            }
            else
            {
                this.Height -= 15;
            }
            #endregion

            SearchData();
            //oFunctionKeysData = oFunctionKeys.PopulateList("");
            //grdDetail.DataSource = oFunctionKeysData;

            //grdDetail.Refresh();
            //ApplyGrigFormat();

            chkIncludeProcessedTimesheet.Enabled = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Timesheet.ID, UserPriviliges.Screens.TimesheetCreate.ID, UserPriviliges.Permissions.EditProcessedTimesheet.ID);    //Sprint-25 - PRIMEPOS-2253 23-Mar-2017 JY Added            
        }

        #region PRIMEPOS-189 06-Aug-2021 JY Added
        public void tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                iBlinkCnt++;
                if (iBlinkCnt % 4 == 0)
                    lblMessage.Appearance.ForeColor = Color.Transparent;
                else
                    lblMessage.Appearance.ForeColor = Color.Red;
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion

        private void SetKeyActionMappings(UltraGrid oUGrid)
        {
            oUGrid.KeyActionMappings.Clear();

            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Enter, UltraGridAction.NextCellByTab, 0, UltraGridState.InEdit, 0, 0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Right, UltraGridAction.NextCellByTab, 0, UltraGridState.InEdit, 0, 0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Left, UltraGridAction.PrevCellByTab, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Up, UltraGridAction.AboveCell, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Up, UltraGridAction.EnterEditMode, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Down, UltraGridAction.BelowCell, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Down, UltraGridAction.EnterEditMode, 0, UltraGridState.InEdit, 0, 0));

            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.UndoCell, 0, UltraGridState.InEdit, 0, 0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape,UltraGridAction.ExitEditMode,0,UltraGridState.InEdit,0,0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.UndoRow, 0, UltraGridState.AddRow, 0, 0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape,UltraGridAction.LastRowInGrid,0,UltraGridState.InEdit,0,0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape,UltraGridAction.FirstCellInRow,0,UltraGridState.InEdit,0,0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.EnterEditMode, 0, UltraGridState.InEdit, 0, 0));

            oUGrid.DisplayLayout.TabNavigation = TabNavigation.NextControl;
        }


		private void grdDetail_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{

			if (this.grdDetail.ActiveCell == null) return;

			if (e.KeyData == Keys.Enter)
			{
				if(this.grdDetail.ContainsFocus==false)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdDetail_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
            if (e.KeyChar == Convert.ToChar(13))
            {
                e.Handled = true;
            }
		}
		
		private void grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
            try
            {
                if (e.Cell.Row.IsAddRow == false)
                {

                    if (e.Cell.Column.Key == "Edit")
                    {
                        e.Cell.Appearance.BackColor = grdDetail.ActiveRow.Appearance.BackColor;
                        grdDetail.ActiveRow.Cells["TimeIn"].IgnoreRowColActivation = true;
                        grdDetail.ActiveRow.Cells["TimeIn"].Activation = Activation.AllowEdit;
                        grdDetail.ActiveRow.Cells["TimeOut"].IgnoreRowColActivation = true;
                        grdDetail.ActiveRow.Cells["TimeOut"].Activation = Activation.AllowEdit;
                    }
                }
                //else
                //{
                //    if (grdDetail.ActiveCell.Column.Index == grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].Index)
                //    {
                //        SearchItem();
                //    }
                //}
            }
            catch (POSExceptions exp)
            {
                //clsUIHelper.ShowErrorMsg(exp.ErrMessage);
            }
		}

		private void grdDetail_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
		{
			grdDetail.Focus();
		}

		private void frmFunctionKeys_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

		private void grdDetail_Enter(object sender, System.EventArgs e)
		{
			try
			{
				if (this.grdDetail.Rows.Count>0)
				{
                    if (this.grdDetail.ActiveCell == null)
                    {
                        this.grdDetail.Rows[0].Cells["TimeIn"].Activate();
                        this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                    }
                    else
                    {
                        this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                    }
				}
			}
			catch (Exception )
			{}
		}

		private void groupBox2_Enter(object sender, System.EventArgs e)
		{
		
		}

        private void grdDetail_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            //e.Row.Height = 40;
        }

        private void txtUserID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchUser();
        }

        private void txtUserID_Leave(object sender, System.EventArgs e)
        {
            try
            {
                if (txtUserID.Text.Length > 0)
                {
                    ValidateUser();
                }
                else
                {
                    lblUserDescription.Text = "";
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                txtUserID.Text = "";
                txtUserID.Focus();
            }
        }

        private void ValidateUser()
        {
            if (txtUserID.Text != null || this.txtUserID.Text.Trim() != "")
            {
                DisplayUserName(txtUserID.Text);
            }
            else
            {
                lblUserDescription.Text = "";
            }
        }

        private void SearchUser()
        {
            try
            {

                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Users_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Users_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    this.txtUserID.Text = strCode;
                    DisplayUserName(strCode);
                    //Edit(strCode);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void DisplayUserName(string userID)
        {
            UserManagement.clsLogin login = new POS_Core_UI.UserManagement.clsLogin();
            lblUserDescription.Text = login.GetUsername(userID);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            try
            {
                Timesheet oTimesheet = new Timesheet();
                oSearchData = oTimesheet.SearchDataFromEvents(this.txtUserID.Text, Convert.ToDateTime(this.dtpFromDate.Value), Convert.ToDateTime(this.dtpToDate.Value), chkIncludeProcessedTimesheet.Checked); //Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added chkIncludeProcessedTimesheet 
                this.grdDetail.DataSource = oSearchData.Tables[0];
                ApplyGrigFormat();
                if (grdDetail.Rows.Count > 0) CalculateTotalHours(); //Sprint-19 - 2053 09-Mar-2015 JY Added to calculate total hours
                oSearchData.Tables[0].AcceptChanges();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmCreateTimesheet_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            POSToolTip.Show(this, "Create / Update Timesheet from Timesheet event applicatin", 600);
        }

        #region Sprint-19 - 2053 09-Mar-2015 JY Added to calculate total hours
        private void CalculateTotalHours()
        {
            TimeSpan ts;
            DateTime StartDate, EndDate;
            try
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {                    
                    if (grdDetail.Rows[i].Cells["TimeIn"].Value.ToString() != "" && grdDetail.Rows[i].Cells["TimeOut"].Value.ToString() != "")
                    {
                        StartDate = Convert.ToDateTime(grdDetail.Rows[i].Cells["TimeIn"].Value);
                        EndDate = Convert.ToDateTime(grdDetail.Rows[i].Cells["TimeOut"].Value);
                        ts = EndDate.Subtract(StartDate);
                        grdDetail.Rows[i].Cells["Total Hours"].Value = ts.TotalHours;
                        if (nDisplayHourlyRate != 0)
                            grdDetail.Rows[i].Cells["Total Earnings"].Value = CalculateTotalEarnings(Configuration.convertNullToDecimal(ts.TotalHours), Configuration.convertNullToDecimal(grdDetail.Rows[i].Cells["HourlyRate"].Value));  //PRIMEPOS-189 05-Aug-2021 JY Added
                        if (ts.TotalHours < 0)
                            grdDetail.DisplayLayout.Rows[i].Appearance.BackColor = Color.LightPink;
                        if (ts.TotalHours > 24)
                           grdDetail.DisplayLayout.Rows[i].Appearance.BackColor = Color.LightSkyBlue;
                    }
                    if (Configuration.convertNullToBoolean(grdDetail.Rows[i].Cells["isProcessed"].Value))  //Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added
                    {
                        grdDetail.DisplayLayout.Rows[i].Appearance.BackColor = Color.Yellow;
                    }
                }
            }
            catch { }
        }

        #region PRIMEPOS-189 05-Aug-2021 JY Added
        private decimal CalculateTotalEarnings(decimal TotalHours, decimal HourlyRate)
        {
            decimal TotalEarnings = 0;
            try
            {
                if (TotalHours > 0 && HourlyRate > 0)
                    TotalEarnings = TotalHours * HourlyRate;
            }
            catch(Exception Ex)
            {

            }
            return TotalEarnings;
        }
        #endregion

        private void grdDetail_CellChange(object sender, CellEventArgs e)
        {
            TimeSpan ts;
            DateTime StartDate, EndDate;
            int i = grdDetail.ActiveRow.Index;;

            try
            {                
                if (grdDetail.Rows[i].Cells["TimeIn"].Text != "" && grdDetail.Rows[i].Cells["TimeOut"].Text != "")
                {
                    StartDate = Convert.ToDateTime(grdDetail.Rows[i].Cells["TimeIn"].Text);
                    EndDate = Convert.ToDateTime(grdDetail.Rows[i].Cells["TimeOut"].Text);
                    ts = EndDate.Subtract(StartDate);
                    grdDetail.Rows[i].Cells["Total Hours"].Value = ts.TotalHours;

                    if (ts.TotalHours < 0)
                        grdDetail.DisplayLayout.Rows[i].Appearance.BackColor = Color.LightPink;
                    else if (ts.TotalHours > 24)
                        grdDetail.DisplayLayout.Rows[i].Appearance.BackColor = Color.LightSkyBlue;
                    else
                        grdDetail.DisplayLayout.Rows[i].Appearance.BackColor = Color.White;
                }
                if (Configuration.convertNullToBoolean(grdDetail.Rows[i].Cells["isProcessed"].Value))  //Sprint-25 - PRIMEPOS-2253 24-Mar-2017 JY Added
                {
                    grdDetail.DisplayLayout.Rows[i].Appearance.BackColor = Color.Yellow;
                }
            }
            catch { grdDetail.Rows[i].Cells["Total Hours"].Value = null; }
        }
        #endregion

        private void grdDetail_AfterRowUpdate(object sender, RowEventArgs e)
        {
            int i = e.Row.Index;
            try
            {
                if (grdDetail.Rows[i].Cells["TimeIn"].Text != "" && grdDetail.Rows[i].Cells["TimeOut"].Text != "" && string.IsNullOrEmpty(grdDetail.Rows[i].Cells["Total Hours"].Text) == true)
                {
                    TimeSpan ts;
                    DateTime StartDate, EndDate;
                    try
                    {
                        StartDate = Convert.ToDateTime(grdDetail.Rows[i].Cells["TimeIn"].Text);
                        EndDate = Convert.ToDateTime(grdDetail.Rows[i].Cells["TimeOut"].Text);
                        ts = EndDate.Subtract(StartDate);
                        grdDetail.Rows[i].Cells["Total Hours"].Value = ts.TotalHours;
                    }
                    catch { }
                }

            }
            catch { }
        }

        private void grdDetail_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            this.grdDetail.DisplayLayout.Bands[0].Columns["isProcessed"].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns["TimesheetId"].Hidden = true;

            grdDetail.DisplayLayout.Bands[0].Columns["TimeIn"].CellActivation = Activation.NoEdit;
            grdDetail.DisplayLayout.Bands[0].Columns["TimeOut"].CellActivation = Activation.NoEdit;
        }
    }
}
