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
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;
using System.Collections.Generic;
using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmRptItemPriceLog.
	/// </summary>
	public class frmRptCostAnalysis : System.Windows.Forms.Form, ICommandLIneTaskControl
    {
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
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
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numAvgProfitPerc;
        private CheckBox chkIgnore0Cost;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendorCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSubDepartment;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private CheckBox chkIgnoreRXItems;
        private IContainer components;

		public frmRptCostAnalysis()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.customControl = new usrDateRangeParams();  //PRIMEPOS-2485 02-Apr-2021 JY Added
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptCostAnalysis));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton4 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.chkIgnoreRXItems = new System.Windows.Forms.CheckBox();
            this.txtSubDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtVendorCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.chkIgnore0Cost = new System.Windows.Forms.CheckBox();
            this.numAvgProfitPerc = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
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
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvgProfitPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.chkIgnoreRXItems);
            this.gbInventoryReceived.Controls.Add(this.txtSubDepartment);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel3);
            this.gbInventoryReceived.Controls.Add(this.txtItemID);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel2);
            this.gbInventoryReceived.Controls.Add(this.txtVendorCode);
            this.gbInventoryReceived.Controls.Add(this.chkIgnore0Cost);
            this.gbInventoryReceived.Controls.Add(this.numAvgProfitPerc);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.txtDeptCode);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel11);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(15, 46);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(473, 246);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Text = "Report Criteria";
            this.gbInventoryReceived.Enter += new System.EventHandler(this.gbInventoryReceived_Enter_1);
            // 
            // chkIgnoreRXItems
            // 
            this.chkIgnoreRXItems.AutoSize = true;
            this.chkIgnoreRXItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIgnoreRXItems.Location = new System.Drawing.Point(24, 215);
            this.chkIgnoreRXItems.Name = "chkIgnoreRXItems";
            this.chkIgnoreRXItems.Size = new System.Drawing.Size(127, 17);
            this.chkIgnoreRXItems.TabIndex = 15;
            this.chkIgnoreRXItems.Text = "Ignore RX items";
            this.chkIgnoreRXItems.UseVisualStyleBackColor = true;
            // 
            // txtSubDepartment
            // 
            this.txtSubDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtSubDepartment.ButtonsRight.Add(editorButton1);
            this.txtSubDepartment.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubDepartment.Location = new System.Drawing.Point(253, 107);
            this.txtSubDepartment.MaxLength = 20;
            this.txtSubDepartment.Name = "txtSubDepartment";
            this.txtSubDepartment.Size = new System.Drawing.Size(192, 20);
            this.txtSubDepartment.TabIndex = 7;
            this.txtSubDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtSubDepartment_EditorButtonClick);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(25, 110);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(126, 15);
            this.ultraLabel1.TabIndex = 6;
            this.ultraLabel1.Text = "Sub Department";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(24, 161);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(186, 19);
            this.ultraLabel3.TabIndex = 10;
            this.ultraLabel3.Text = "Item ID <Blank = Any Item>";
            // 
            // txtItemID
            // 
            this.txtItemID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            editorButton2.Appearance = appearance2;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton2.Text = "";
            this.txtItemID.ButtonsRight.Add(editorButton2);
            this.txtItemID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemID.Location = new System.Drawing.Point(253, 160);
            this.txtItemID.MaxLength = 20;
            this.txtItemID.Name = "txtItemID";
            this.txtItemID.Size = new System.Drawing.Size(192, 20);
            this.txtItemID.TabIndex = 11;
            this.txtItemID.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemID_EditorButtonClick);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(24, 136);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(226, 23);
            this.ultraLabel2.TabIndex = 8;
            this.ultraLabel2.Text = "Vendor Code <Blank = Any Vendor>";
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            editorButton3.Appearance = appearance3;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton3.Text = "";
            this.txtVendorCode.ButtonsRight.Add(editorButton3);
            this.txtVendorCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorCode.Location = new System.Drawing.Point(253, 134);
            this.txtVendorCode.MaxLength = 20;
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.Size = new System.Drawing.Size(192, 20);
            this.txtVendorCode.TabIndex = 9;
            this.txtVendorCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendorCode_EditorButtonClick);
            // 
            // chkIgnore0Cost
            // 
            this.chkIgnore0Cost.AutoSize = true;
            this.chkIgnore0Cost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIgnore0Cost.Location = new System.Drawing.Point(255, 215);
            this.chkIgnore0Cost.Name = "chkIgnore0Cost";
            this.chkIgnore0Cost.Size = new System.Drawing.Size(181, 17);
            this.chkIgnore0Cost.TabIndex = 14;
            this.chkIgnore0Cost.Text = "Ignore cost with 0 value";
            this.chkIgnore0Cost.UseVisualStyleBackColor = true;
            // 
            // numAvgProfitPerc
            // 
            this.numAvgProfitPerc.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.numAvgProfitPerc.Location = new System.Drawing.Point(253, 186);
            this.numAvgProfitPerc.MaskInput = "nnnn";
            this.numAvgProfitPerc.MinValue = 0;
            this.numAvgProfitPerc.Name = "numAvgProfitPerc";
            this.numAvgProfitPerc.Size = new System.Drawing.Size(192, 22);
            this.numAvgProfitPerc.TabIndex = 13;
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance4.FontData.BoldAsString = "False";
            appearance4.FontData.ItalicAsString = "False";
            appearance4.FontData.StrikeoutAsString = "False";
            appearance4.FontData.UnderlineAsString = "False";
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance4;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(253, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(192, 21);
            this.dtpToDate.TabIndex = 3;
            this.dtpToDate.Tag = "To Date";
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.ItalicAsString = "False";
            appearance5.FontData.StrikeoutAsString = "False";
            appearance5.FontData.UnderlineAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance5;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(253, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(192, 21);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.Tag = "From Date";
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // txtDeptCode
            // 
            this.txtDeptCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            editorButton4.Appearance = appearance6;
            editorButton4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton4.Text = "";
            this.txtDeptCode.ButtonsRight.Add(editorButton4);
            this.txtDeptCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeptCode.Location = new System.Drawing.Point(253, 80);
            this.txtDeptCode.MaxLength = 20;
            this.txtDeptCode.Name = "txtDeptCode";
            this.txtDeptCode.Size = new System.Drawing.Size(192, 20);
            this.txtDeptCode.TabIndex = 5;
            this.txtDeptCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDeptCode_EditorButtonClick);
            this.txtDeptCode.Validating += new System.ComponentModel.CancelEventHandler(this.txtDeptCode_Validating);
            // 
            // ultraLabel11
            // 
            this.ultraLabel11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel11.Location = new System.Drawing.Point(24, 190);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(116, 18);
            this.ultraLabel11.TabIndex = 12;
            this.ultraLabel11.Text = "Average Profit %";
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel12.Location = new System.Drawing.Point(24, 83);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel12.TabIndex = 4;
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
            this.ultraLabel14.TabIndex = 0;
            this.ultraLabel14.Text = "From Date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 294);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(473, 57);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // ultraButton1
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            this.ultraButton1.Appearance = appearance7;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraButton1.Location = new System.Drawing.Point(174, 19);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "&Print";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // btnClose
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnClose.Appearance = appearance8;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(358, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            this.btnView.Appearance = appearance9;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(266, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.ForeColorDisabled = System.Drawing.Color.White;
            appearance10.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance10;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(59, 8);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(385, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Cost Analysis";
            // 
            // frmRptCostAnalysis
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(502, 366);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptCostAnalysis";
            this.Tag = "Header";
            this.Text = "Cost Analysis";
            this.Load += new System.EventHandler(this.frmRptItemPriceLog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptItemPriceLog_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptItemPriceLog_KeyUp);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvgProfitPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeptCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmRptItemPriceLog_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
			dtpFromDate.Value = DateTime.Now;
			dtpToDate.Value = DateTime.Now;
			
			this.numAvgProfitPerc.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.numAvgProfitPerc.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtDeptCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtDeptCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //Added By Amit Date 9 June 2011
            this.txtSubDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtSubDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtVendorCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtVendorCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtItemID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtItemID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //End
			this.dtpFromDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpFromDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			
			this.dtpToDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpToDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);			

		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			Preview(true);
		}

        #region Sprint-19 - 2126 20-Feb-2015 JY commented
        //private void Preview(bool PrintId)
        //{
        //    try
        //    {
        //        ReportClass oRpt;
        //        if(txtSubDepartment.Text!="")
        //            oRpt = new rptCostAnalysisReportGroupBy();
        //        else
        //            oRpt=new rptCostAnalysisReport();
        //        //Modified By Amit Date 8 June 2011
        //        //Original
        //        //string sSQL = " Select Department.DeptName,PTD.ItemID,Item.Description,Sum(PTD.Qty ) as TotalQty " +
        //        //    " ,Sum(PTD.ExtendedPrice-PTD.Discount) as TotalNetPrice " +
        //        //    " ,Sum(Isnull(Case When IsNull(PTD.ItemCost,0)<>0 Then IsNull(PTD.ItemCost,0) When IsNull(Item.LastCostPrice,0)<>0 then Item.LastCostPrice " +
        //        //    " Else (PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount)*(100-" + numAvgProfitPerc.Value.ToString() + ")/100 End,0))  TotalCostPrice " +
        //        //    " From POSTransaction PT Inner Join POSTransactionDetail PTD " +
        //        //    " on PT.TransID=PTD.TransID Inner Join Item On PTD.ItemID=Item.ItemID " +
        //        //    " Left Outer Join Department on Item.DepartmentID=Department.DeptID " +
        //        //    " where PT.TransType<>3 ";

        //        //PTD.Qty * is added by shitaljit 0n 11 Dec 2012 as per discussion with Fahad that the total cost price is not calcualting properly
        //        //the ITEMCOST price is recorded incorrectly when we group item qty in POSTransactionDetail table
        //        //is a quickfix that we are going to change the report query instead of changing into Database

        //        //Sprint-19 - 2126 16-Jan-2015 JY Added strSubQuery for Invoice discount 
        //        string sSQL=string.Empty, strSubQuery = string.Empty; 
        //        if (txtSubDepartment.Text != "")
        //        {
        //            //Sprint-19 - 2125 30-Dec-2014 JY Added "AND Item.SUBDEPARTMENTID = SubDepartment.SubDepartmentID" to corect sub-department filter
        //            sSQL = " Select Department.DeptName,SubDepartment.Description as SubDeptName,PTD.ItemID,Item.Description,Sum(PTD.Qty) as TotalQty " +
        //                ",Sum (PTD.ExtendedPrice) as [Gross Sale]" +
        //                ",Sum (PTD.Discount) as Discount" +
        //                " ,Sum(PTD.ExtendedPrice-PTD.Discount) as [Net Sale] " +
        //                ",Sum(PTD.TaxAmount) as Tax" +
        //                ",Sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as TotalAmount" +
        //                ",Sum(PTD.Qty *Isnull(Case When IsNull(PTD.ItemCost,0)<>0 Then IsNull(PTD.ItemCost,0) When IsNull(Item.LastCostPrice,0)<>0 then Item.LastCostPrice " +
        //                " Else (PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount)*(100-" + numAvgProfitPerc.Value.ToString() + ")/100 End,0))  TotalCostPrice " +
        //                " From POSTransaction PT Inner Join POSTransactionDetail PTD " +
        //                " on PT.TransID=PTD.TransID Inner Join Item On PTD.ItemID=Item.ItemID " +
        //                " Left Outer Join Department on Item.DepartmentID=Department.DeptID " +
        //                " Left Outer Join SubDepartment on Department.DeptID=SubDepartment.DepartmentID AND Item.SUBDEPARTMENTID = SubDepartment.SubDepartmentID" +
        //                " where PT.TransType<>3 ";
        //        }
        //        else 
        //        {
        //            sSQL = " Select Department.DeptName,PTD.ItemID,Item.Description,Sum(PTD.Qty) as TotalQty " +
        //                ",Sum (PTD.ExtendedPrice) as [Gross Sale]" +
        //                ",Sum (PTD.Discount) as Discount" +
        //                " ,Sum(PTD.ExtendedPrice-PTD.Discount) as [Net Sale] " +
        //                ",Sum(PTD.TaxAmount) as Tax" +
        //                ",Sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as TotalAmount" +
        //                ",Sum(PTD.Qty * Isnull(Case When IsNull(PTD.ItemCost,0)<>0 Then IsNull(PTD.ItemCost,0) When IsNull(Item.LastCostPrice,0)<>0 then Item.LastCostPrice " +
        //                " Else (PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount)*(100-" + numAvgProfitPerc.Value.ToString() + ")/100 End,0))  TotalCostPrice " +
        //                " From POSTransaction PT Inner Join POSTransactionDetail PTD " +
        //                " on PT.TransID=PTD.TransID Inner Join Item On PTD.ItemID=Item.ItemID " +
        //                " Left Outer Join Department on Item.DepartmentID=Department.DeptID " +
        //                " where PT.TransType<>3 "; 
        //        }
        //        //End
        //        strSubQuery = "SELECT SUM(ISNULL(PT.INVOICEDISCOUNT,0)) FROM POSTransaction PT WHERE PT.TransType<>3";
        //        if (dtpFromDate.Value.ToString() != "")
        //        {
        //            sSQL += " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) >= '" + dtpFromDate.Text + "'";
        //            strSubQuery += " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) >= '" + dtpFromDate.Text + "'";
        //        }
        //        if (dtpToDate.Value.ToString() != "")
        //        {
        //            sSQL += " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) <= '" + dtpToDate.Text + "'";
        //            strSubQuery += " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) <= '" + dtpToDate.Text + "'";
        //        }

        //       //if (txtDeptCode.Tag!= null)
        //        if (txtDeptCode.Text != "")
        //        {
        //            sSQL += " AND Department.DeptCode = '" + txtDeptCode.Text + "'";
        //            strSubQuery += " AND Department.DeptCode = '" + txtDeptCode.Text + "'";
        //        }
        //        //if (txtSubDepartment.Tag != null)
        //        if (txtSubDepartment.Text != "")
        //        {
        //            sSQL += "AND SubDepartment.SubDepartmentID='" + txtSubDepartment.Text + "'";
        //            strSubQuery += "AND SubDepartment.SubDepartmentID='" + txtSubDepartment.Text + "'";
        //        }
        //        //Added By Amit Date 8 June 2011
        //        if (this.txtItemID.Text.Trim() != "" && this.txtVendorCode.Text == "")
        //        {
        //            sSQL += " and PTD.ItemID='" + this.txtItemID.Text.Trim() + "' ";                    
        //        }
        //        else if (this.txtVendorCode.Text.Trim() != "")
        //        {
        //            if (txtItemID.Text.Trim() == "")
        //            {
        //                sSQL+= " and PTD.ItemID in(select IV.itemid from ItemVendor IV " +
        //                    "where IV.VendorId=(Select Vendorid from Vendor where VendorCode='" + this.txtVendorCode.Text + "'))";
        //            }
        //            else
        //            {
        //                sSQL += " and PTD.ItemID in(select IV.itemid from ItemVendor IV " +
        //                    "where IV.VendorId=(Select Vendorid from Vendor where VendorCode='" + this.txtVendorCode.Text + "')) and PTD.ItemID='" + this.txtItemID.Text + "'";
        //            }                   
        //        }

        //        if (chkIgnore0Cost.Checked == true)
        //        {
        //            sSQL += " And (IsNull(PTD.ItemCost,0)>0 Or IsNull(Item.LastCostPrice,0)>0 ) ";
        //        }
        //        //Modified By Amit Date 13 May 2011
        //        if (chkIgnoreRXItems.Checked == true)
        //        {
        //            sSQL += " And PTD.ItemID != 'RX' ";
        //        }
        //        if (txtSubDepartment.Text == "")
        //        {
        //            sSQL += " Group by PTD.ItemID,Item.Description, Department.DeptName" +
        //                " Having Sum(PTD.Qty)>0 ";

        //            sSQL += " Order by Department.DeptName,Item.Description";
                    
        //        }
        //        else 
        //        {
        //            sSQL += " Group by PTD.ItemID,Item.Description, Department.DeptName,SubDepartment.Description " +
        //                " Having Sum(PTD.Qty)>0 ";

        //            sSQL += " Order by Department.DeptName,SubDepartment.Description,Item.Description";
                    
        //        }//End
        //        clsReports.setCRTextObjectText("txtFromDate","From :" + this.dtpFromDate.Text,oRpt);
        //        clsReports.setCRTextObjectText("txtToDate","To :" + dtpToDate.Text,oRpt);
        //        clsReports.Preview(PrintId,sSQL,oRpt);
        //    }
        //    catch(Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}
        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void dtpToDate_BeforeDropDown(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
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

		private void btnView_Click(object sender, System.EventArgs e)
		{
			this.dtpFromDate.Focus();
			Preview(false);
		}

		private void ultraButton1_Click(object sender, System.EventArgs e)
		{
			this.dtpFromDate.Focus();
			Preview(true);
		}

		private void txtDeptCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl,"",txtDeptCode.Text);
            frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Department_tbl, "", txtDeptCode.Text, true);    //20-Dec-2017 JY Added new reference
            oSearch.ShowDialog();
            if (oSearch.IsCanceled)
            {
               // txtDeptCode.Tag = null;
                txtDeptCode.Text = "";
            }
            else
            {
               // txtDeptCode.Tag = oSearch.SelectedRowID();
               // txtDeptCode.Text = oSearch.SelectedCode();
                txtDeptCode.Text = oSearch.SelectedRowID();
            }
		}

		private void frmRptItemPriceLog_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtDeptCode.ContainsFocus)
						txtDeptCode_EditorButtonClick(null,null);
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
        //Start : Added By Amit Date 9 June 2011
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
        private void txtItemID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
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
                    this.txtItemID.Focus();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtVendorCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchVendor();
        }
        private void SearchVendor()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);                
                //if (txtVendorCode.Text != "")
                //    oSearch.txtCode.Text = txtVendorCode.Text;
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Vendor_tbl, txtVendorCode.Text,"",true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog();
                if (oSearch.IsCanceled) return;
                txtVendorCode.Text = oSearch.SelectedRowID();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void SearchSubDept()
        {
            try
            {
                POS_Core.BusinessRules.Department oDept = new Department();
                DepartmentData oDeptData;
                DepartmentRow oDeptRow = null;
                List<string> listDept = new List<string>();
                if (txtDeptCode.Text.ToString() != "")
                {
                    oDeptData = oDept.Populate(txtDeptCode.Text);
                }
                else
                {
                    oDeptData = oDept.PopulateList("");
                }
                for (int RowIndex = 0; RowIndex < oDeptData.Tables[0].Rows.Count; RowIndex++)
                {
                    oDeptRow = oDeptData.Department[RowIndex];
                    listDept.Add(oDeptRow.DeptID.ToString());
                }
                string InQuery = string.Join("','", listDept.ToArray());
                if (InQuery != "")
                {
                    SearchSvr.SubDeptIDFlag = true;
                }
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.SubDepartment_tbl, InQuery, "");
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.SubDepartment_tbl, InQuery, "", true);  //20-Dec-2017 JY Added new reference
                //oSearch.AllowMultiRowSelect = true;
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    //txtSubDepartment.Tag = oSearch.SelectedRowID();
                   // txtSubDepartment.Text = oSearch.SelectedCode();
                    txtSubDepartment.Text = oSearch.SelectedRowID();
                }
                else
                {
                    txtSubDepartment.Text = string.Empty;
                   // txtSubDepartment.Tag = string.Empty;
                }
                SearchSvr.SubDeptIDFlag = false;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtSubDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchSubDept();
        }
        //End

        #region Sprint-19 - 2125/2126 20-Feb-2015 JY Added modified method - also correct the 2125 subdepartment issue by adding "AND I.SUBDEPARTMENTID = SD.SubDepartmentID" to corect sub-department filter
        private void Preview(bool PrintId, bool bCalledFromScheduler = false)   //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
            string sSQL = string.Empty, strFilter = string.Empty;
            try
            {
                ReportClass oRpt;
                if (txtSubDepartment.Text != "")
                    oRpt = new rptCostAnalysisReportGroupBy();
                else
                    oRpt = new rptCostAnalysisReport();

                #region Filter
                if (Convert.ToString(dtpFromDate.Value) != "")
                    strFilter += " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) >= '" + dtpFromDate.Text + "'";

                if (Convert.ToString(dtpToDate.Value.ToString()) != "")
                    strFilter += " AND Convert(smalldatetime,Convert(Varchar,PT.TransDate,107)) <= '" + dtpToDate.Text + "'";

                if (Convert.ToString(txtDeptCode.Text) != "")
                    strFilter += " AND D.DeptCode = '" + txtDeptCode.Text + "'";

                if (Convert.ToString(txtSubDepartment.Text) != "")
                    strFilter += " AND SD.SubDepartmentID='" + txtSubDepartment.Text + "'";

                if (Convert.ToString(this.txtItemID.Text.Trim()) != "" && Convert.ToString(this.txtVendorCode.Text) == "")
                {
                    strFilter += " and PTD.ItemID = '" + this.txtItemID.Text.Trim() + "' ";
                }
                else if (Convert.ToString(this.txtVendorCode.Text.Trim()) != "")
                {
                    if (Convert.ToString(txtItemID.Text.Trim()) == "")
                    {
                        strFilter += " and PTD.ItemID IN (select IV.itemid from ItemVendor IV where IV.VendorId = (Select Vendorid from Vendor where VendorCode='" + this.txtVendorCode.Text + "'))";
                    }
                    else
                    {
                        strFilter += " and PTD.ItemID in(select IV.itemid from ItemVendor IV where IV.VendorId=(Select Vendorid from Vendor where VendorCode='" + this.txtVendorCode.Text + "')) and PTD.ItemID='" + this.txtItemID.Text + "'";
                    }
                }

                if (chkIgnore0Cost.Checked == true)
                    strFilter += " And (IsNull(PTD.ItemCost,0)>0 Or IsNull(I.LastCostPrice,0)>0 ) ";

                if (chkIgnoreRXItems.Checked == true)
                    strFilter += " And PTD.ItemID != 'RX' ";
                #endregion


                sSQL = " SELECT D.DeptName, SD.Description as SubDeptName, PTD.ItemID, I.Description, Sum(PTD.Qty) as TotalQty, Sum(PTD.ExtendedPrice) as [Gross Sale], " +
                        " Sum(PTD.Discount) as Discount, Sum(PTD.ExtendedPrice-PTD.Discount) as [Net Sale], Sum(PTD.TaxAmount) as Tax, Sum(PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount) as TotalAmount, " +
                        " Sum(PTD.Qty *Isnull(Case When IsNull(PTD.ItemCost,0)<>0 Then IsNull(PTD.ItemCost,0) When IsNull(I.LastCostPrice,0)<>0 then I.LastCostPrice Else (PTD.ExtendedPrice-PTD.Discount+PTD.TaxAmount)*(100-" + numAvgProfitPerc.Value.ToString() + ")/100 End,0))  TotalCostPrice, " +
                        " x.InvoiceDiscount FROM POSTransaction PT " +
                        " INNER JOIN POSTransactionDetail PTD ON PT.TransID = PTD.TransID " +
                        " INNER JOIN Item I ON PTD.ItemID = I.ItemID " +
                        " LEFT JOIN Department D ON I.DepartmentID = D.DeptID " +
                        " LEFT JOIN SubDepartment SD ON D.DeptID = SD.DepartmentID AND I.SUBDEPARTMENTID = SD.SubDepartmentID " +
                        " INNER JOIN (SELECT SUM(InvoiceDiscount) AS InvoiceDiscount FROM " +
                                        " (SELECT PTD.ItemID, CASE WHEN ISNULL(SUM(PT.GrossTotal),0) = 0 then 0 else SUM(ISNULL(PT.INVOICEDISCOUNT,0))*SUM(PTD.ExtendedPrice)/SUM(PT.GrossTotal) END AS InvoiceDiscount FROM POSTransaction PT " +
                                        " INNER JOIN POSTransactionDetail PTD on PT.transid = PTD.transid INNER JOIN Item I on I.ItemID = PTD.ItemID LEFT JOIN Department D on D.DeptID = I.DepartmentID LEFT JOIN subDepartment SD on SD.DepartmentID = D.DeptID AND SD.SubDepartmentID = I.SUBDEPARTMENTID " +
                                        " WHERE PT.TransType<>3 " + strFilter +
                                        " group by D.DeptID, D.DeptName, SD.SubDepartmentID, SD.Description, PTD.ItemID, PT.TransID " +
                                        " ) z ) x on 1=1 " +
                        " WHERE PT.TransType<>3 " + strFilter +
                        " Group by x.InvoiceDiscount, PTD.ItemID, I.Description, D.DeptName, SD.Description " +
                        " Having Sum(PTD.Qty)>0 " +
                        " Order by D.DeptName, SD.Description, I.Description";

                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpToDate.Text, oRpt);
                clsReports.Preview(PrintId, sSQL, oRpt, bCalledFromScheduler); //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
                oReport = oRpt; //PRIMEPOS-2485 02-Apr-2021 JY Added
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region PRIMEPOS-2485 02-Apr-2021 JY Added
        public bool bSendPrint = true;
        private ReportClass oReport = new ReportClass();
        public usrDateRangeParams customControl;
        private const string ReportName = "CostAnalysis";

        public bool CheckTags()
        {
            return true;
        }

        public bool SaveTaskParameters(DataTable dt, int ScheduledTasksID)
        {
            try
            {
                ScheduledTasks oScheduledTasks = new ScheduledTasks();
                oScheduledTasks.SaveTaskParameters(dt, ScheduledTasksID);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetControlParameters(int ScheduledTasksID)
        {
            ScheduledTasks oScheduledTasks = new ScheduledTasks();
            DataTable dt = oScheduledTasks.GetScheduledTasksControlsList(ScheduledTasksID);
            customControl.setControlsValues(ref dt);
            setControlsValues(ref dt);  //PRIMEPOS-3066 21-Mar-2022 JY Added
            return true;
        }

        #region PRIMEPOS-3066 21-Mar-2022 JY Added
        public void setControlsValues(ref DataTable dt)
        {
            double Num;

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpFromDate.Tag + " ' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpFromDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpFromDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }

            foreach (DataRow odr in dt.Select("ControlsName = '" + this.dtpToDate.Tag + "' "))
            {
                if (double.TryParse(odr["ControlsValue"].ToString().Trim(), out Num))
                {
                    dtpToDate.Value = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
                }
                else
                {
                    dtpToDate.Value = odr["ControlsValue"].ToString().Trim();
                }
            }
        }
        #endregion

        public bool RunTask(int TaskId, ref string filePath, bool bsendToPrint, ref string sNoOfRecordAffect)
        {
            SetControlParameters(TaskId);
            bSendPrint = bsendToPrint;
            //dtpFromDate.Value = DateTime.Now.AddDays(Left - 60);
            //dtpToDate.Value = DateTime.Now;
            Preview(bSendPrint, true);
            filePath = Application.StartupPath + @"\" + ReportName + (DateTime.Now).ToString().Replace("/", "").Replace(":", "") + ".pdf";
            this.oReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
            return true;
        }

        public void GetTaskParameters(ref DataTable dt, int ScheduledTasksID)
        {
            customControl.getControlsValues(ref dt);
        }

        public Control GetParameterControl()
        {
            customControl.setDateTimeControl();
            customControl.Dock = DockStyle.Fill;
            return customControl;
        }

        public bool checkValidation()
        {
            return true;
        }
        #endregion
    }
}
