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
using POS_Core.Resources;
using System.Timers;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmRptItemPriceLog.
	/// </summary>
	public class frmRptItemPriceLog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeptCode;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton ultraButton1;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private CheckBox chkInStockItm;
        private GroupBox ultraGroupBox2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optSortBy;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtChangedByUser;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor cmbChangedTrough;
        private Infragistics.Win.Misc.UltraLabel lblChangedTrough;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLocation;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        public Infragistics.Win.Misc.UltraLabel lblMessage;
        private IContainer components;

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion

        public frmRptItemPriceLog()
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptItemPriceLog));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.txtLocation = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbChangedTrough = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblChangedTrough = new Infragistics.Win.Misc.UltraLabel();
            this.txtChangedByUser = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.chkInStockItm = new System.Windows.Forms.CheckBox();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDeptCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.optSortBy = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbChangedTrough)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChangedByUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optSortBy)).BeginInit();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.txtLocation);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel3);
            this.gbInventoryReceived.Controls.Add(this.cmbChangedTrough);
            this.gbInventoryReceived.Controls.Add(this.lblChangedTrough);
            this.gbInventoryReceived.Controls.Add(this.txtChangedByUser);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.chkInStockItm);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel2);
            this.gbInventoryReceived.Controls.Add(this.cboTransType);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.txtItemCode);
            this.gbInventoryReceived.Controls.Add(this.txtDeptCode);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel11);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(15, 40);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(424, 282);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Text = "Report Criteria";
            // 
            // txtLocation
            // 
            this.txtLocation.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocation.Location = new System.Drawing.Point(185, 136);
            this.txtLocation.MaxLength = 20;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(192, 20);
            this.txtLocation.TabIndex = 4;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(26, 140);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(114, 15);
            this.ultraLabel3.TabIndex = 52;
            this.ultraLabel3.Text = "Location";
            // 
            // cmbChangedTrough
            // 
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance1.BorderColor3DBase = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.ItalicAsString = "False";
            appearance1.FontData.StrikeoutAsString = "False";
            appearance1.FontData.UnderlineAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbChangedTrough.Appearance = appearance1;
            this.cmbChangedTrough.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cmbChangedTrough.ButtonAppearance = appearance2;
            valueListItem1.DataValue = "A";
            valueListItem1.DisplayText = "All";
            valueListItem2.DataValue = "M";
            valueListItem2.DisplayText = "Manually";
            valueListItem3.DataValue = "E";
            valueListItem3.DisplayText = "Electronically";
            this.cmbChangedTrough.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.cmbChangedTrough.Location = new System.Drawing.Point(184, 186);
            this.cmbChangedTrough.MaxLength = 20;
            this.cmbChangedTrough.Name = "cmbChangedTrough";
            this.cmbChangedTrough.Size = new System.Drawing.Size(192, 20);
            this.cmbChangedTrough.TabIndex = 6;
            this.cmbChangedTrough.ValueChanged += new System.EventHandler(this.cmbChangedTrough_ValueChanged);
            // 
            // lblChangedTrough
            // 
            this.lblChangedTrough.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangedTrough.Location = new System.Drawing.Point(25, 188);
            this.lblChangedTrough.Name = "lblChangedTrough";
            this.lblChangedTrough.Size = new System.Drawing.Size(114, 15);
            this.lblChangedTrough.TabIndex = 50;
            this.lblChangedTrough.Text = "Changed Through";
            // 
            // txtChangedByUser
            // 
            this.txtChangedByUser.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtChangedByUser.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangedByUser.Location = new System.Drawing.Point(184, 217);
            this.txtChangedByUser.MaxLength = 20;
            this.txtChangedByUser.Name = "txtChangedByUser";
            this.txtChangedByUser.Size = new System.Drawing.Size(192, 20);
            this.txtChangedByUser.TabIndex = 7;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(25, 220);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(114, 15);
            this.ultraLabel1.TabIndex = 48;
            this.ultraLabel1.Text = "Changed By User";
            // 
            // chkInStockItm
            // 
            this.chkInStockItm.AutoSize = true;
            this.chkInStockItm.Location = new System.Drawing.Point(185, 162);
            this.chkInStockItm.Name = "chkInStockItm";
            this.chkInStockItm.Size = new System.Drawing.Size(151, 17);
            this.chkInStockItm.TabIndex = 5;
            this.chkInStockItm.Text = "InStock Items Only";
            this.chkInStockItm.UseVisualStyleBackColor = true;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.FontData.BoldAsString = "False";
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance3;
            this.ultraLabel2.Location = new System.Drawing.Point(25, 251);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(67, 15);
            this.ultraLabel2.TabIndex = 42;
            this.ultraLabel2.Text = "Trans Type";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraLabel2.Visible = false;
            // 
            // cboTransType
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance4.ForeColor = System.Drawing.Color.White;
            this.cboTransType.Appearance = appearance4;
            this.cboTransType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance5.BackColor2 = System.Drawing.Color.Silver;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboTransType.ButtonAppearance = appearance5;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboTransType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboTransType.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(183, 249);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(192, 23);
            this.cboTransType.TabIndex = 8;
            this.cboTransType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboTransType.Visible = false;
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
            this.dtpToDate.Location = new System.Drawing.Point(185, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(192, 21);
            this.dtpToDate.TabIndex = 1;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
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
            this.dtpFromDate.Location = new System.Drawing.Point(185, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(192, 21);
            this.dtpFromDate.TabIndex = 0;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
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
            this.txtItemCode.Location = new System.Drawing.Point(185, 108);
            this.txtItemCode.MaxLength = 20;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(192, 20);
            this.txtItemCode.TabIndex = 3;
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
            this.txtDeptCode.Location = new System.Drawing.Point(185, 80);
            this.txtDeptCode.MaxLength = 20;
            this.txtDeptCode.Name = "txtDeptCode";
            this.txtDeptCode.Size = new System.Drawing.Size(192, 20);
            this.txtDeptCode.TabIndex = 2;
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 375);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // ultraButton1
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            this.ultraButton1.Appearance = appearance10;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraButton1.Location = new System.Drawing.Point(142, 19);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 1;
            this.ultraButton1.Text = "&Print";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // btnClose
            // 
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            this.btnClose.Appearance = appearance11;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(326, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance12.FontData.BoldAsString = "True";
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            this.btnView.Appearance = appearance12;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(234, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance13.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance13;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(449, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Item Price Change Log";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.optSortBy);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(16, 325);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(424, 50);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Sort Item By";
            // 
            // optSortBy
            // 
            this.optSortBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optSortBy.CheckedIndex = 0;
            appearance14.FontData.BoldAsString = "False";
            this.optSortBy.ItemAppearance = appearance14;
            this.optSortBy.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem4.DataValue = 1;
            valueListItem4.DisplayText = "Changed Date";
            valueListItem5.DataValue = 2;
            valueListItem5.DisplayText = "Item";
            valueListItem6.DataValue = "3";
            valueListItem6.DisplayText = "Department";
            valueListItem7.DataValue = "4";
            valueListItem7.DisplayText = "User";
            this.optSortBy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5,
            valueListItem6,
            valueListItem7});
            this.optSortBy.ItemSpacingHorizontal = 20;
            this.optSortBy.Location = new System.Drawing.Point(34, 20);
            this.optSortBy.Name = "optSortBy";
            this.optSortBy.Size = new System.Drawing.Size(359, 20);
            this.optSortBy.TabIndex = 0;
            this.optSortBy.Text = "Changed Date";
            this.optSortBy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // lblMessage
            // 
            appearance15.ForeColor = System.Drawing.Color.Red;
            appearance15.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance15;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 436);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(449, 20);
            this.lblMessage.TabIndex = 33;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "cost price is hidden due to the user does not have enough permissions";
            this.lblMessage.Visible = false;
            // 
            // frmRptItemPriceLog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(449, 456);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptItemPriceLog";
            this.Tag = "Header";
            this.Text = "Item Price Change Log";
            this.Load += new System.EventHandler(this.frmRptItemPriceLog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptItemPriceLog_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptItemPriceLog_KeyUp);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbChangedTrough)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChangedByUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optSortBy)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptItemPriceLog_Load(object sender, System.EventArgs e)
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
			
			this.txtItemCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtItemCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtLocation.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtLocation.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.dtpFromDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpFromDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			
			this.dtpToDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpToDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //Start : Added By Amit Date 24 April 2011
            this.txtChangedByUser.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtChangedByUser.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            
            this.cmbChangedTrough.SelectedIndex = 0;
            //End

			populateTransType();

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

        private void populateTransType()
		{
			/*try
			{
				InvTransTypeData oInvTransTypeData=new InvTransTypeData();
				InvTransType oInvTransType=new InvTransType();
				oInvTransTypeData=oInvTransType.PopulateList("");

				if (oInvTransTypeData.InvTransType.Rows.Count==0)
				{
					InvTransTypeRow oNewRow= oInvTransTypeData.InvTransType.AddRow(0,"",0,"");
					oNewRow.TypeName="Inventory Received";
					oNewRow.TransType=0;
					oNewRow.UserID=Resources.Configuration.UserName;
					oInvTransType.Persist(oInvTransTypeData);
					oInvTransTypeData=oInvTransType.PopulateList("");

				}
				
				Infragistics.Win.ValueListItem oVl=new Infragistics.Win.ValueListItem();
				this.cboTransType.Items.Clear();
				foreach (InvTransTypeRow oDRow in oInvTransTypeData.InvTransType.Rows)
				{
					this.cboTransType.Items.Add(oDRow.ID.ToString(),oDRow.TypeName.ToString());
				}
				this.cboTransType.SelectedIndex=0;
			}
			catch(Exception exp) {clsUIHelper.ShowErrorMsg(exp.Message);}*/
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
                int nDisplayItemCost = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID));   //PRIMEPOS-2464 09-Mar-2020 JY Added

                rptItemPriceChangeLog oRpt =new rptItemPriceChangeLog();

                //PRIMEPOS-2464 09-Mar-2020 JY Added nDisplayItemCost
                //PRIMEPOS-1871 23-Mar-2020 JY made some corrections in the query
                string sSQL = " SELECT IPH.*, Item.Description, Dept.DeptName, item.Location, " + nDisplayItemCost + " AS DisplayItemCost FROM Itempricehistory IPH" +
                            " INNER JOIN Item ON IPH.ItemID = Item.ItemID" +
                            " LEFT OUTER JOIN Department Dept ON Item.DepartmentID = Dept.DeptID" +
                            " WHERE ((IPH.UpdatedBy = 'M' AND IPH.SalePrice <> IPH.ORGSELLINGPRICE) OR IPH.UpdatedBy = 'E') ";
				sSQL = sSQL + buildCriteria(oRpt);
                //Commented By Amit Date 24 April 2011
                //sSQL += " Order by Dept.DeptName,Item.Description,IPH.AddedON Desc";
                //Start Added By Amit Date 24 April 2011
                if (optSortBy.CheckedIndex == 0)
                {
                    sSQL += " Order by IPH.AddedOn";
                }
                else if (optSortBy.CheckedIndex == 1)
                {
                    sSQL += " Order by IPH.ItemID";
                }
                else if (optSortBy.CheckedIndex == 2)
                {
                    sSQL += " Order by Dept.DeptName";
                }
                else if (optSortBy.CheckedIndex == 3)
                {
                    sSQL += " Order by IPH.UserID";
                }
                //End
                clsReports.setCRTextObjectText("txtFromDate","From :" + this.dtpFromDate.Text,oRpt);
                clsReports.setCRTextObjectText("txtToDate","To :" + dtpToDate.Text,oRpt);
				clsReports.Preview(PrintId,sSQL,oRpt);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

        private string buildCriteria(rptItemPriceChangeLog oRpt)
		{
			string sCriteria = "";
			
			if (dtpFromDate.Value.ToString()!="")
				sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,AddedOn,107)) >= '" + dtpFromDate.Text + "'";
			if (dtpToDate.Value.ToString()!="")
				sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,AddedOn,107)) <= '" + dtpToDate.Text + "'";
			if (txtDeptCode.Tag!=null)
				sCriteria = sCriteria + " AND Dept.DeptCode = '" + txtDeptCode.Tag.ToString() + "'";
			if (txtItemCode.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + " AND item.ItemId = '" + txtItemCode.Text + "'";
            //Start: Added By Amit Date 24 April 2011
            if (cmbChangedTrough.SelectedItem.ToString() == "All")
            {
                clsReports.setCRTextObjectText("txtChangedTrough", "All", oRpt);
            }
            else if (cmbChangedTrough.SelectedItem.ToString() == "Manually")    //records updated through POS
            {
                sCriteria = sCriteria + " AND (IPH.UpdatedBy = 'M' AND IPH.SalePrice <> IPH.ORGSELLINGPRICE)";  //PRIMEPOS-1871 23-Mar-2020 JY modified

                if (txtChangedByUser.Text != "")
                {
                    sCriteria = sCriteria + " AND IPH.UserID = '" + txtChangedByUser.Text + "'";
                }

                clsReports.setCRTextObjectText("txtChangedTrough", "Manually", oRpt);
            }
            else if (cmbChangedTrough.SelectedItem.ToString() == "Electronically")  //records updated through EDI
            {
                sCriteria = sCriteria + " AND IPH.UpDatedBy = 'E'";
                clsReports.setCRTextObjectText("txtChangedTrough", "Electronically", oRpt);
            }
            //End
			
			if(cboTransType.SelectedIndex>0)
			{
				sCriteria = sCriteria + " AND IPH.ChangedIn = '" + cboTransType.Value.ToString() + "'";
			}

            if(chkInStockItm.Checked)
                sCriteria = sCriteria + " AND Item.QtyInStock > 0 ";

            if (string.IsNullOrEmpty(this.txtLocation.Text.Trim()) == false)
            {
                sCriteria += " AND Item.Location = '" + this.txtLocation.Text.Trim() + "'";
            }
			return sCriteria;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptItemPriceLog_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void frmRptItemPriceLog_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
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

        private void txtDeptCode_Validating(object sender, CancelEventArgs e)
        {
            if (txtDeptCode.Text.Trim() == "")
            {
                txtDeptCode.Tag = null;
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

        //Start : Added By Amit 24 Apr 2011
        private void cmbChangedTrough_ValueChanged(object sender, EventArgs e)
        {
            if (cmbChangedTrough.SelectedItem.ToString() == "All" || cmbChangedTrough.SelectedItem.ToString() == "Electronically")
            {
                this.txtChangedByUser.Enabled = false;
            }
            else
            {
                this.txtChangedByUser.Enabled = true;
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