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
using POS_Core.CommonData;   //Sprint-21 - 1861 23-Jul-2015 JY Added
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptEODSummary : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private System.Windows.Forms.GroupBox ultraGroupBox2;
		private Infragistics.Win.UltraWinEditors.UltraOptionSet optEODBy;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.Panel pnlByCloseDate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private System.Windows.Forms.Panel pnlByEODId;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEODIdFrom;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtEODIdTo;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserId;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private GroupBox groupBox1;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet opnOrderBy;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptEODSummary()
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptEODSummary));
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.txtUserId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.pnlByCloseDate = new System.Windows.Forms.Panel();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.pnlByEODId = new System.Windows.Forms.Panel();
            this.txtEODIdTo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtEODIdFrom = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.optEODBy = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.opnOrderBy = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId)).BeginInit();
            this.pnlByCloseDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            this.pnlByEODId.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODIdTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODIdFrom)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optEODBy)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opnOrderBy)).BeginInit();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.txtUserId);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.pnlByCloseDate);
            this.gbInventoryReceived.Controls.Add(this.pnlByEODId);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(10, 92);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(406, 105);
            this.gbInventoryReceived.TabIndex = 3;
            this.gbInventoryReceived.TabStop = false;
            // 
            // txtUserId
            // 
            this.txtUserId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserId.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserId.Location = new System.Drawing.Point(189, 78);
            this.txtUserId.MaxLength = 20;
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(123, 20);
            this.txtUserId.TabIndex = 7;
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel12.Location = new System.Drawing.Point(10, 80);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(170, 15);
            this.ultraLabel12.TabIndex = 3;
            this.ultraLabel12.Text = "User ID <Blank = Any User>";
            // 
            // pnlByCloseDate
            // 
            this.pnlByCloseDate.Controls.Add(this.dtpToDate);
            this.pnlByCloseDate.Controls.Add(this.dtpFromDate);
            this.pnlByCloseDate.Controls.Add(this.ultraLabel1);
            this.pnlByCloseDate.Controls.Add(this.ultraLabel2);
            this.pnlByCloseDate.Location = new System.Drawing.Point(10, 14);
            this.pnlByCloseDate.Name = "pnlByCloseDate";
            this.pnlByCloseDate.Size = new System.Drawing.Size(390, 60);
            this.pnlByCloseDate.TabIndex = 4;
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.ItalicAsString = "False";
            appearance1.FontData.StrikeoutAsString = "False";
            appearance1.FontData.UnderlineAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance1;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(179, 34);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(123, 21);
            this.dtpToDate.TabIndex = 6;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.ItalicAsString = "False";
            appearance2.FontData.StrikeoutAsString = "False";
            appearance2.FontData.UnderlineAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance2;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(179, 6);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
            this.dtpFromDate.TabIndex = 5;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(121, 37);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(49, 15);
            this.ultraLabel1.TabIndex = 6;
            this.ultraLabel1.Text = "To Date";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(106, 9);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(64, 15);
            this.ultraLabel2.TabIndex = 3;
            this.ultraLabel2.Text = "From Date";
            // 
            // pnlByEODId
            // 
            this.pnlByEODId.Controls.Add(this.txtEODIdTo);
            this.pnlByEODId.Controls.Add(this.txtEODIdFrom);
            this.pnlByEODId.Controls.Add(this.ultraLabel13);
            this.pnlByEODId.Controls.Add(this.ultraLabel14);
            this.pnlByEODId.Location = new System.Drawing.Point(10, 14);
            this.pnlByEODId.Name = "pnlByEODId";
            this.pnlByEODId.Size = new System.Drawing.Size(390, 60);
            this.pnlByEODId.TabIndex = 4;
            this.pnlByEODId.Visible = false;
            // 
            // txtEODIdTo
            // 
            this.txtEODIdTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtEODIdTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEODIdTo.Location = new System.Drawing.Point(179, 34);
            this.txtEODIdTo.MaskInput = "nnnnn";
            this.txtEODIdTo.Name = "txtEODIdTo";
            this.txtEODIdTo.Size = new System.Drawing.Size(123, 20);
            this.txtEODIdTo.TabIndex = 6;
            // 
            // txtEODIdFrom
            // 
            this.txtEODIdFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtEODIdFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEODIdFrom.Location = new System.Drawing.Point(179, 6);
            this.txtEODIdFrom.MaskInput = "nnnnn";
            this.txtEODIdFrom.Name = "txtEODIdFrom";
            this.txtEODIdFrom.Size = new System.Drawing.Size(123, 20);
            this.txtEODIdFrom.TabIndex = 5;
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.AutoSize = true;
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(106, 37);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(64, 15);
            this.ultraLabel13.TabIndex = 6;
            this.ultraLabel13.Text = "EOD ID To";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.AutoSize = true;
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(92, 9);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(78, 15);
            this.ultraLabel14.TabIndex = 3;
            this.ultraLabel14.Text = "EOD Id From";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.optEODBy);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(10, 46);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(406, 50);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Filter By";
            // 
            // optEODBy
            // 
            this.optEODBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optEODBy.CheckedIndex = 0;
            appearance3.FontData.BoldAsString = "False";
            this.optEODBy.ItemAppearance = appearance3;
            this.optEODBy.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem1.DataValue = 1;
            valueListItem1.DisplayText = "Close Date";
            valueListItem2.DataValue = 2;
            valueListItem2.DisplayText = "EOD ID";
            this.optEODBy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.optEODBy.ItemSpacingHorizontal = 50;
            this.optEODBy.Location = new System.Drawing.Point(74, 20);
            this.optEODBy.Name = "optEODBy";
            this.optEODBy.Size = new System.Drawing.Size(238, 20);
            this.optEODBy.TabIndex = 2;
            this.optEODBy.Text = "Close Date";
            this.optEODBy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.optEODBy.ValueChanged += new System.EventHandler(this.optEODBy_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 57);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnPrint.Appearance = appearance4;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(131, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnClose.Appearance = appearance5;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(315, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            this.btnView.Appearance = appearance6;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(223, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.ForeColorDisabled = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance7;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(10, 10);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(406, 30);
            this.lblTransactionType.TabIndex = 30;
            this.lblTransactionType.Text = "End Of Day Summary";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.opnOrderBy);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 50);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Order By";
            // 
            // opnOrderBy
            // 
            this.opnOrderBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.opnOrderBy.CheckedIndex = 0;
            appearance8.FontData.BoldAsString = "False";
            this.opnOrderBy.ItemAppearance = appearance8;
            this.opnOrderBy.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem3.DataValue = 1;
            valueListItem3.DisplayText = "Date";
            valueListItem4.DataValue = 2;
            valueListItem4.DisplayText = "EOD ID";
            this.opnOrderBy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.opnOrderBy.ItemSpacingHorizontal = 50;
            this.opnOrderBy.Location = new System.Drawing.Point(74, 20);
            this.opnOrderBy.Name = "opnOrderBy";
            this.opnOrderBy.Size = new System.Drawing.Size(238, 20);
            this.opnOrderBy.TabIndex = 2;
            this.opnOrderBy.Text = "Date";
            this.opnOrderBy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // frmRptEODSummary
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(422, 307);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptEODSummary";
            this.Text = "End OF Day Summary";
            this.Load += new System.EventHandler(this.frmRptEODSummary_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptEODSummary_KeyDown);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserId)).EndInit();
            this.pnlByCloseDate.ResumeLayout(false);
            this.pnlByCloseDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.pnlByEODId.ResumeLayout(false);
            this.pnlByEODId.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODIdTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEODIdFrom)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optEODBy)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.opnOrderBy)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
        
		private void Preview(bool PrintId)
		{
			try
			{
                rptEODSummary oRpt = new rptEODSummary();

                string sSQL = " SELECT EODID, UserID, CloseDate as [Close Date], TotalSales as [Total Sales], " +
                        " TotalReturns as [Total Returns], (TotalDiscount * -1) as [Total Discount], TotalTax as [Total Tax], " +
                        " TotalROA AS [Total ROA], (ISNULL(TotalSales,0) + ISNULL(TotalReturns,0) - ISNULL(TotalDiscount,0) + ISNULL(TotalTax,0) + ISNULL(TotalROA,0)) AS [Total] " +
                        ", " + (Configuration.convertNullToBoolean(Configuration.CInfo.PrintEODDateTime) == false ? 0 : 1) + " AS NewDateFormat, TotalTransFee AS [Total Trans Fee] " +   //Sprint-23 - PRIMEPOS-2244 19-May-2016 JY Added //PRIMEPOS-3118 03-Aug-2022 JY Added TotalTransFee
                        " FROM " + clsPOSDBConstants.EndOfDay_tbl + " where 1=1 ";
                
                sSQL += buildCriteria();
                if (opnOrderBy.CheckedIndex == 0)
                    sSQL += " ORDER BY CloseDate";
                else
                    sSQL += " ORDER BY EODID";

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
			if (optEODBy.CheckedIndex == 0)
			{
				if (dtpFromDate.Value.ToString()!="")
                    sCriteria = " AND " + " Convert(smalldatetime,Convert(Varchar,CloseDate,107)) BETWEEN '" + dtpFromDate.Text + "' AND '" + dtpToDate.Text + "'";
			}
			else
			{
				int EODIdFrom = (int)txtEODIdFrom.Value;
				int EODIdTo = (int)txtEODIdTo.Value;

				if (EODIdFrom != 0)
                    sCriteria = sCriteria + " AND EODID >= " + EODIdFrom;
				if (EODIdTo!=0)
                    sCriteria = sCriteria + " AND EODID <= " + EODIdTo;
			}
			if (txtUserId.Text.Trim() !="")
                sCriteria = sCriteria + " AND UserID = '" + txtUserId.Text.Trim().Replace("'","''") + "'";
			
			return sCriteria;
		}

		private void frmRptEODSummary_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void btnView_Click(object sender, System.EventArgs e)
		{
			this.optEODBy.Focus();
			Preview(false);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			this.optEODBy.Focus();
			Preview(true);
		}


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void optEODBy_ValueChanged(object sender, EventArgs e)
        {
			pnlByCloseDate.Visible = false;
			pnlByEODId.Visible = false;

			if (optEODBy.CheckedIndex == 0)
				pnlByCloseDate.Visible = true;
			else
				pnlByEODId.Visible = true;
        }

        private void frmRptEODSummary_Load(object sender, System.EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;

            this.txtEODIdFrom.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtEODIdFrom.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtEODIdTo.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtEODIdTo.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtUserId.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtUserId.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpFromDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpFromDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.dtpToDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.dtpToDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
        }
	}
}
