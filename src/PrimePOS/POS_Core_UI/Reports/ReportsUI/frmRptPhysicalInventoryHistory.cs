using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using POS_Core.CommonData;
using System.Timers;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptPhysicalInventoryHistory : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton ultraButton1;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private System.Windows.Forms.RadioButton optPBoth;
		private System.Windows.Forms.RadioButton optPNo;
		private System.Windows.Forms.RadioButton optPYes;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton optProcessed;
		private System.Windows.Forms.RadioButton optPending;
		private System.Windows.Forms.RadioButton optBoth;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeptCode;
        private GroupBox ultraGroupBox2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optSortBy;
        public Infragistics.Win.Misc.UltraLabel lblMessage;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion

        public frmRptPhysicalInventoryHistory()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptPhysicalInventoryHistory));
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.txtDeptCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.optPBoth = new System.Windows.Forms.RadioButton();
            this.optPNo = new System.Windows.Forms.RadioButton();
            this.optPYes = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optBoth = new System.Windows.Forms.RadioButton();
            this.optPending = new System.Windows.Forms.RadioButton();
            this.optProcessed = new System.Windows.Forms.RadioButton();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.optSortBy = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optSortBy)).BeginInit();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.txtDeptCode);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.txtItemCode);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel11);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(15, 40);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(424, 141);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Text = "Inventory Received Report";
            this.gbInventoryReceived.Enter += new System.EventHandler(this.gbInventoryReceived_Enter_1);
            // 
            // txtDeptCode
            // 
            this.txtDeptCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtDeptCode.ButtonsRight.Add(editorButton1);
            this.txtDeptCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeptCode.Location = new System.Drawing.Point(146, 80);
            this.txtDeptCode.MaxLength = 20;
            this.txtDeptCode.Name = "txtDeptCode";
            this.txtDeptCode.Size = new System.Drawing.Size(123, 20);
            this.txtDeptCode.TabIndex = 5;
            this.txtDeptCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDeptCode_EditorButtonClick);
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance10.FontData.BoldAsString = "False";
            appearance10.FontData.ItalicAsString = "False";
            appearance10.FontData.StrikeoutAsString = "False";
            appearance10.FontData.UnderlineAsString = "False";
            appearance10.ForeColor = System.Drawing.Color.Black;
            appearance10.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance10;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton3);
            this.dtpToDate.Location = new System.Drawing.Point(146, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(123, 21);
            this.dtpToDate.TabIndex = 4;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance11.FontData.BoldAsString = "False";
            appearance11.FontData.ItalicAsString = "False";
            appearance11.FontData.StrikeoutAsString = "False";
            appearance11.FontData.UnderlineAsString = "False";
            appearance11.ForeColor = System.Drawing.Color.Black;
            appearance11.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance11;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton4);
            this.dtpFromDate.Location = new System.Drawing.Point(146, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
            this.dtpFromDate.TabIndex = 3;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // txtItemCode
            // 
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            editorButton2.Appearance = appearance4;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton2.Text = "";
            this.txtItemCode.ButtonsRight.Add(editorButton2);
            this.txtItemCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.Location = new System.Drawing.Point(146, 108);
            this.txtItemCode.MaxLength = 20;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(123, 20);
            this.txtItemCode.TabIndex = 6;
            this.txtItemCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
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
            this.ultraLabel12.Text = "Dept Code";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 295);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // ultraButton1
            // 
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance12.FontData.BoldAsString = "True";
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            this.ultraButton1.Appearance = appearance12;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraButton1.Location = new System.Drawing.Point(142, 19);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 11;
            this.ultraButton1.Text = "&Print";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // btnClose
            // 
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.White;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            this.btnClose.Appearance = appearance13;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(326, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            this.btnView.Appearance = appearance14;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(234, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 10;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance15.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance15;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(454, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Text = "Physical Inventory History Report";
            // 
            // optPBoth
            // 
            this.optPBoth.Location = new System.Drawing.Point(0, 0);
            this.optPBoth.Name = "optPBoth";
            this.optPBoth.Size = new System.Drawing.Size(104, 24);
            this.optPBoth.TabIndex = 0;
            // 
            // optPNo
            // 
            this.optPNo.Location = new System.Drawing.Point(0, 0);
            this.optPNo.Name = "optPNo";
            this.optPNo.Size = new System.Drawing.Size(104, 24);
            this.optPNo.TabIndex = 0;
            // 
            // optPYes
            // 
            this.optPYes.Location = new System.Drawing.Point(0, 0);
            this.optPYes.Name = "optPYes";
            this.optPYes.Size = new System.Drawing.Size(104, 24);
            this.optPYes.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optBoth);
            this.groupBox1.Controls.Add(this.optPending);
            this.groupBox1.Controls.Add(this.optProcessed);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 57);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Include Only";
            // 
            // optBoth
            // 
            this.optBoth.Checked = true;
            this.optBoth.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optBoth.Location = new System.Drawing.Point(330, 20);
            this.optBoth.Name = "optBoth";
            this.optBoth.Size = new System.Drawing.Size(80, 24);
            this.optBoth.TabIndex = 9;
            this.optBoth.TabStop = true;
            this.optBoth.Text = "Both";
            // 
            // optPending
            // 
            this.optPending.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optPending.Location = new System.Drawing.Point(246, 20);
            this.optPending.Name = "optPending";
            this.optPending.Size = new System.Drawing.Size(104, 24);
            this.optPending.TabIndex = 8;
            this.optPending.Text = "Pending";
            // 
            // optProcessed
            // 
            this.optProcessed.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optProcessed.Location = new System.Drawing.Point(146, 20);
            this.optProcessed.Name = "optProcessed";
            this.optProcessed.Size = new System.Drawing.Size(104, 24);
            this.optProcessed.TabIndex = 7;
            this.optProcessed.Text = "Processed";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.optSortBy);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(15, 240);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(424, 50);
            this.ultraGroupBox2.TabIndex = 34;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Sort By";
            // 
            // optSortBy
            // 
            this.optSortBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optSortBy.CheckedIndex = 0;
            appearance16.FontData.BoldAsString = "False";
            this.optSortBy.ItemAppearance = appearance16;
            this.optSortBy.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem4.DataValue = 1;
            valueListItem4.DisplayText = "Trans. Date";
            valueListItem5.DataValue = 2;
            valueListItem5.DisplayText = "Item Code";
            valueListItem6.DataValue = "3";
            valueListItem6.DisplayText = "Item Desc.";
            this.optSortBy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5,
            valueListItem6});
            this.optSortBy.ItemSpacingHorizontal = 20;
            this.optSortBy.Location = new System.Drawing.Point(34, 20);
            this.optSortBy.Name = "optSortBy";
            this.optSortBy.Size = new System.Drawing.Size(359, 20);
            this.optSortBy.TabIndex = 0;
            this.optSortBy.Text = "Trans. Date";
            this.optSortBy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // lblMessage
            // 
            appearance17.ForeColor = System.Drawing.Color.Red;
            appearance17.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance17;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 356);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(454, 20);
            this.lblMessage.TabIndex = 35;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "cost price is hidden due to the user does not have enough permissions";
            this.lblMessage.Visible = false;
            // 
            // frmRptPhysicalInventoryHistory
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(454, 376);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptPhysicalInventoryHistory";
            this.Text = "Physical Inventory History Report";
            this.Load += new System.EventHandler(this.frmInventoryReports_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptInventoryReceived_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptInventoryReceived_KeyUp);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optSortBy)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmInventoryReports_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
            //Added By Shitaljit on 16 August 2011
            DateTime dtCurrTime = new DateTime();
            dtCurrTime = DateTime.Now;
            dtpToDate.Value = dtCurrTime;
            dtpFromDate.Value = dtCurrTime;

            //Commented By Shitaljit on 16 August 2011
            //dtpToDate.Value = DateTime.Now;
            //dtpFromDate.Value = DateTime.Now;
            //End of Coding By shitaljit on 16 Aug 2011

			this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.dtpFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			
			this.dtpToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);	
		
			this.dtpToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.optSortBy.CheckedIndex = 0;//Added By shitaljit to make record sort by tras date as default.

            #region PRIMEPOS-2464 10-Mar-2020 JY Added
            nDisplayItemCost = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID));   //PRIMEPOS-2464 09-Mar-2020 JY Added
            if (nDisplayItemCost == 0)
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
        }

        private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			Preview(true);
		}

		private void Preview(bool PrintId)
		{
			try
			{
				Reports.rptPhysicalInventoryHistory oRpt =new rptPhysicalInventoryHistory();
				string sSQL = " SELECT " +
									" Pinv.ItemCode,pInv.OldQty,pInv.NewQty,pInv.TransDate,pInv.isProcessed,pInv.pTransDate,I.Description,pInv.pUserID  " +
                                    " ,isnull(i.lastcostprice*newqty,0) as itemTotcost,isnull(i.sellingprice*newqty,0) as itemtotsellingprice, i.sellingprice as unitsellprice, pInv.NewExpDate " +
                                    ", " + nDisplayItemCost + " AS DisplayItemCost" +   //PRIMEPOS-2464 09-Mar-2020 JY Added
                                    " from physicalinv pInv,Item I left outer join Department Dept on I.DepartmentID=Dept.DeptID "  +
									" where pInv.ItemCode=I.ItemID " ;

				sSQL = sSQL + buildCriteria();
                //Added By shitaljit on 6 May 2012 to make record sort by tras date,ItemCode,Item Description.
                if (this.optSortBy.CheckedIndex == 0)
                {
                    sSQL += " ORDER BY pInv.TransDate ";
                }
                else if (this.optSortBy.CheckedIndex == 1)
                {
                    sSQL += " ORDER BY Pinv.ItemCode ";
                }
                else if (this.optSortBy.CheckedIndex == 2)
                {
                    sSQL += " ORDER BY I.Description ";
                }
                //Till here added by shitaljit 
				clsReports.Preview(PrintId,sSQL,oRpt);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private string buildCriteria()
		{
			string sCriteria = "";
			
			if (dtpFromDate.Value.ToString()!="")
				sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,pInv.TransDate,107)) >= '" + dtpFromDate.Text + "'";
			if (dtpToDate.Value.ToString()!="")
				sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,pInv.TransDate,107)) <= '" + dtpToDate.Text + "'";
			if (txtDeptCode.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + " AND Dept.DeptCode = '" + txtDeptCode.Text + "' ";
			if (txtItemCode.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + " AND i.ItemId = '" + txtItemCode.Text + "'";

			if (this.optPending.Checked==true)
				sCriteria = sCriteria + " and pInv.isProcessed=0";
			else if (this.optProcessed.Checked==true)
				sCriteria = sCriteria + " and pInv.isProcessed=1";
			
			return sCriteria;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void dtpToDate_BeforeDropDown(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}

		private void frmRptInventoryReceived_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void gbInventoryReceived_Enter(object sender, System.EventArgs e)
		{
			dtpFromDate.Focus();
		}

		private void gbInventoryReceived_Enter_1(object sender, System.EventArgs e)
		{
		
		}
        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }
		private void btnView_Click(object sender, System.EventArgs e)
		{
             string fieldName = string.Empty;
             try
             {
                 if (!validateFields(out fieldName))
                 {
                     if (fieldName == "DATE")
                     {
                         clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                     }

                     return;
                 }
                 this.dtpFromDate.Focus();
                 Preview(false);
             }
             catch (Exception ex)
             {
                 clsUIHelper.ShowErrorMsg(ex.Message);
             }
		}

		private void ultraButton1_Click(object sender, System.EventArgs e)
		{
			this.dtpFromDate.Focus();
			Preview(true);
		}

		private void txtDeptCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
			if (oSearch.IsCanceled) return;
			txtDeptCode.Text = oSearch.SelectedRowID();
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

		private void frmRptInventoryReceived_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtDeptCode.ContainsFocus)
						txtDeptCode_EditorButtonClick(null,null);
					else if ( txtItemCode.ContainsFocus)
						txtItemCode_EditorButtonClick(null,null);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
             string fieldName = string.Empty;
             try
             {
                 if (!validateFields(out fieldName))
                 {
                     if (fieldName == "DATE")
                     {
                         clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                     }

                     return;
                 }
             }
             catch (Exception ex)
             {
             }
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
 
        }

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
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
    }
}
