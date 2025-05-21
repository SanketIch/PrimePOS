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
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptCloseStationSummary : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private System.Windows.Forms.GroupBox ultraGroupBox2;
		private Infragistics.Win.UltraWinEditors.UltraOptionSet optByName;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private System.Windows.Forms.Panel pnlByCloseDate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private System.Windows.Forms.Panel pnlByCloseNo;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtCloseNoFrom;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtCloseNoTo;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationId;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptCloseStationSummary()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmRptCloseStationSummary));
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
			this.txtStationId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
			this.pnlByCloseDate = new System.Windows.Forms.Panel();
			this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
			this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
			this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
			this.pnlByCloseNo = new System.Windows.Forms.Panel();
			this.txtCloseNoTo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
			this.txtCloseNoFrom = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
			this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
			this.optByName = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnPrint = new Infragistics.Win.Misc.UltraButton();
			this.btnClose = new Infragistics.Win.Misc.UltraButton();
			this.btnView = new Infragistics.Win.Misc.UltraButton();
			this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
			this.gbInventoryReceived.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtStationId)).BeginInit();
			this.pnlByCloseDate.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
			this.pnlByCloseNo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtCloseNoTo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCloseNoFrom)).BeginInit();
			this.ultraGroupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.optByName)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbInventoryReceived
			// 
			this.gbInventoryReceived.Controls.Add(this.txtStationId);
			this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
			this.gbInventoryReceived.Controls.Add(this.pnlByCloseDate);
			this.gbInventoryReceived.Controls.Add(this.pnlByCloseNo);
			this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.gbInventoryReceived.Location = new System.Drawing.Point(16, 116);
			this.gbInventoryReceived.Name = "gbInventoryReceived";
			this.gbInventoryReceived.Size = new System.Drawing.Size(406, 105);
			this.gbInventoryReceived.TabIndex = 3;
			this.gbInventoryReceived.TabStop = false;
			this.gbInventoryReceived.Text = "Station Close";
			// 
			// txtStationId
			// 
			this.txtStationId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtStationId.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtStationId.Location = new System.Drawing.Point(144, 78);
			this.txtStationId.MaxLength = 20;
			this.txtStationId.Name = "txtStationId";
			this.txtStationId.Size = new System.Drawing.Size(123, 20);
			this.txtStationId.TabIndex = 7;
			// 
			// ultraLabel12
			// 
			this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel12.Location = new System.Drawing.Point(22, 80);
			this.ultraLabel12.Name = "ultraLabel12";
			this.ultraLabel12.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel12.TabIndex = 3;
			this.ultraLabel12.Text = "Station Id";
			// 
			// pnlByCloseDate
			// 
			this.pnlByCloseDate.Controls.Add(this.dtpToDate);
			this.pnlByCloseDate.Controls.Add(this.dtpFromDate);
			this.pnlByCloseDate.Controls.Add(this.ultraLabel1);
			this.pnlByCloseDate.Controls.Add(this.ultraLabel2);
			this.pnlByCloseDate.Location = new System.Drawing.Point(10, 14);
			this.pnlByCloseDate.Name = "pnlByCloseDate";
			this.pnlByCloseDate.Size = new System.Drawing.Size(266, 60);
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
			this.dtpToDate.BackColor = System.Drawing.SystemColors.Window;
			this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			dateButton1.Caption = "Today";
			this.dtpToDate.DateButtons.Add(dateButton1);
			this.dtpToDate.FlatMode = true;
			this.dtpToDate.Location = new System.Drawing.Point(134, 34);
			this.dtpToDate.Name = "dtpToDate";
			this.dtpToDate.NonAutoSizeHeight = 10;
			this.dtpToDate.Size = new System.Drawing.Size(123, 21);
			this.dtpToDate.TabIndex = 6;
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
			this.dtpFromDate.BackColor = System.Drawing.SystemColors.Window;
			this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
			dateButton2.Caption = "Today";
			this.dtpFromDate.DateButtons.Add(dateButton2);
			this.dtpFromDate.FlatMode = true;
			this.dtpFromDate.Location = new System.Drawing.Point(134, 6);
			this.dtpFromDate.Name = "dtpFromDate";
			this.dtpFromDate.NonAutoSizeHeight = 10;
			this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
			this.dtpFromDate.TabIndex = 5;
			this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
			// 
			// ultraLabel1
			// 
			this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel1.Location = new System.Drawing.Point(12, 37);
			this.ultraLabel1.Name = "ultraLabel1";
			this.ultraLabel1.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel1.TabIndex = 6;
			this.ultraLabel1.Text = "To Date";
			// 
			// ultraLabel2
			// 
			this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel2.Location = new System.Drawing.Point(12, 9);
			this.ultraLabel2.Name = "ultraLabel2";
			this.ultraLabel2.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel2.TabIndex = 3;
			this.ultraLabel2.Text = "From Date";
			// 
			// pnlByCloseNo
			// 
			this.pnlByCloseNo.Controls.Add(this.txtCloseNoTo);
			this.pnlByCloseNo.Controls.Add(this.txtCloseNoFrom);
			this.pnlByCloseNo.Controls.Add(this.ultraLabel13);
			this.pnlByCloseNo.Controls.Add(this.ultraLabel14);
			this.pnlByCloseNo.Location = new System.Drawing.Point(10, 14);
			this.pnlByCloseNo.Name = "pnlByCloseNo";
			this.pnlByCloseNo.Size = new System.Drawing.Size(268, 60);
			this.pnlByCloseNo.TabIndex = 4;
			this.pnlByCloseNo.Visible = false;
			// 
			// txtCloseNoTo
			// 
			this.txtCloseNoTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtCloseNoTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtCloseNoTo.Location = new System.Drawing.Point(135, 34);
			this.txtCloseNoTo.MaskInput = "nnnnn";
			this.txtCloseNoTo.Name = "txtCloseNoTo";
			this.txtCloseNoTo.Size = new System.Drawing.Size(123, 20);
			this.txtCloseNoTo.TabIndex = 6;
			// 
			// txtCloseNoFrom
			// 
			this.txtCloseNoFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtCloseNoFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtCloseNoFrom.Location = new System.Drawing.Point(136, 6);
			this.txtCloseNoFrom.MaskInput = "nnnnn";
			this.txtCloseNoFrom.Name = "txtCloseNoFrom";
			this.txtCloseNoFrom.Size = new System.Drawing.Size(123, 20);
			this.txtCloseNoFrom.TabIndex = 5;
			// 
			// ultraLabel13
			// 
			this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel13.Location = new System.Drawing.Point(12, 37);
			this.ultraLabel13.Name = "ultraLabel13";
			this.ultraLabel13.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel13.TabIndex = 6;
			this.ultraLabel13.Text = "Close No. To";
			// 
			// ultraLabel14
			// 
			this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraLabel14.Location = new System.Drawing.Point(12, 9);
			this.ultraLabel14.Name = "ultraLabel14";
			this.ultraLabel14.Size = new System.Drawing.Size(91, 15);
			this.ultraLabel14.TabIndex = 3;
			this.ultraLabel14.Text = "Close No. From";
			// 
			// ultraGroupBox2
			// 
			this.ultraGroupBox2.Controls.Add(this.optByName);
			this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ultraGroupBox2.Location = new System.Drawing.Point(16, 58);
			this.ultraGroupBox2.Name = "ultraGroupBox2";
			this.ultraGroupBox2.Size = new System.Drawing.Size(406, 50);
			this.ultraGroupBox2.TabIndex = 1;
			this.ultraGroupBox2.TabStop = false;
			this.ultraGroupBox2.Text = "Close Station By";
			// 
			// optByName
			// 
			this.optByName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.optByName.CheckedIndex = 0;
			this.optByName.FlatMode = true;
			appearance3.FontData.BoldAsString = "False";
			this.optByName.ItemAppearance = appearance3;
			this.optByName.ItemOrigin = new System.Drawing.Point(0, 2);
			valueListItem1.DataValue = 1;
			valueListItem1.DisplayText = "Close Date";
			valueListItem2.DataValue = 2;
			valueListItem2.DisplayText = "Close No";
			this.optByName.Items.Add(valueListItem1);
			this.optByName.Items.Add(valueListItem2);
			this.optByName.ItemSpacingHorizontal = 50;
			this.optByName.Location = new System.Drawing.Point(74, 20);
			this.optByName.Name = "optByName";
			this.optByName.Size = new System.Drawing.Size(238, 20);
			this.optByName.TabIndex = 2;
			this.optByName.Text = "Close Date";
			this.optByName.ValueChanged += new System.EventHandler(this.optByName_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnPrint);
			this.groupBox2.Controls.Add(this.btnClose);
			this.groupBox2.Controls.Add(this.btnView);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(16, 226);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(406, 57);
			this.groupBox2.TabIndex = 29;
			this.groupBox2.TabStop = false;
			// 
			// btnPrint
			// 
			appearance4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance4.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance4.FontData.BoldAsString = "True";
			appearance4.ForeColor = System.Drawing.Color.White;
			appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
			this.btnPrint.Appearance = appearance4;
			this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnPrint.Location = new System.Drawing.Point(117, 19);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(85, 26);
			this.btnPrint.SupportThemes = false;
			this.btnPrint.TabIndex = 6;
			this.btnPrint.Text = "&Print";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnClose
			// 
			appearance5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance5.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance5.FontData.BoldAsString = "True";
			appearance5.ForeColor = System.Drawing.Color.White;
			appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
			this.btnClose.Appearance = appearance5;
			this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(301, 20);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(85, 26);
			this.btnClose.SupportThemes = false;
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnView
			// 
			appearance6.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance6.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance6.FontData.BoldAsString = "True";
			appearance6.ForeColor = System.Drawing.Color.White;
			appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
			this.btnView.Appearance = appearance6;
			this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnView.Location = new System.Drawing.Point(209, 20);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(85, 26);
			this.btnView.SupportThemes = false;
			this.btnView.TabIndex = 5;
			this.btnView.Text = "&View";
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// lblTransactionType
			// 
			appearance7.ForeColor = System.Drawing.Color.White;
			appearance7.ForeColorDisabled = System.Drawing.Color.White;
			appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
			this.lblTransactionType.Appearance = appearance7;
			this.lblTransactionType.BackColor = System.Drawing.Color.Transparent;
			this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTransactionType.Location = new System.Drawing.Point(26, 16);
			this.lblTransactionType.Name = "lblTransactionType";
			this.lblTransactionType.Size = new System.Drawing.Size(385, 30);
			this.lblTransactionType.TabIndex = 30;
			this.lblTransactionType.Text = "Station Close Summary";
			// 
			// frmRptCloseStationSummary
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(437, 302);
			this.Controls.Add(this.lblTransactionType);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.ultraGroupBox2);
			this.Controls.Add(this.gbInventoryReceived);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmRptCloseStationSummary";
			this.Text = "Station Close Summary";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptCloseStationSummary_KeyDown);
			this.Load += new System.EventHandler(this.frmInventoryReports_Load);
			this.gbInventoryReceived.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.txtStationId)).EndInit();
			this.pnlByCloseDate.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
			this.pnlByCloseNo.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.txtCloseNoTo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCloseNoFrom)).EndInit();
			this.ultraGroupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.optByName)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmInventoryReports_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
			dtpFromDate.Value = DateTime.Now;
			dtpToDate.Value = DateTime.Now;

			this.txtCloseNoFrom.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtCloseNoFrom.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtCloseNoTo.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtCloseNoTo.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtStationId.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtStationId.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.dtpFromDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpFromDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			
			this.dtpToDate.Enter+= new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.dtpToDate.Leave+= new System.EventHandler(clsUIHelper.AfterExitEditMode);			
		}

		private void Preview(bool PrintId)
		{
			try
			{
				rptIRStationClsoeSummary oRpt =new rptIRStationClsoeSummary();

				string sSQL = " SELECT " +
                                    " StationClose.StationCloseID, StationClose.CloseDate, Stationclose.StationID, ps.Stationname, StationClose.UserID " +
									" , ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'S' AND StationCloseId = StationClose.StationCloseId),0) As Sale " +
									" ,  ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'SR' AND StationCloseId = StationClose.StationCloseId),0) As SReturn  " +
									" ,  ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'DT' AND StationCloseId = StationClose.StationCloseId),0) As Discount  " +
									" , (ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'S' AND StationCloseId = StationClose.StationCloseId),0)  " +
									" + ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'SR' AND StationCloseId = StationClose.StationCloseId),0)  " +
									" - ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'DT' AND StationCloseId = StationClose.StationCloseId),0))As NetSale  " +
                                    " , ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'TX' AND StationCloseId = StationClose.StationCloseId),0) As tax " +
                                    " , ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'A' AND StationCloseId = StationClose.StationCloseId),0) As ROA " +  //PRIMEPOS-2762 14-Nov-2019 JY Added ROA (previously it was hardcoded to "0")
                                    " , ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'TF' AND StationCloseId = StationClose.StationCloseId),0) As TransFee " + //PRIMEPOS-3118 03-Aug-2022 JY Added
                                    " , ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'S' AND StationCloseId = StationClose.StationCloseId),0) " +
									" + ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'SR' AND StationCloseId = StationClose.StationCloseId),0) " +
									" - ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'DT' AND StationCloseId = StationClose.StationCloseId),0) " +
									" + ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'TX' AND StationCloseId = StationClose.StationCloseId),0) " +
                                    " + ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'TF' AND StationCloseId = StationClose.StationCloseId),0) " + //PRIMEPOS-3118 03-Aug-2022 JY Added
                                    " + ISNULL((SELECT SUM(TransAmount) FROM StationCloseDetail WHERE TransType = 'A' AND StationCloseId = StationClose.StationCloseId),0) As GTotal  " +   //PRIMEPOS-2762 14-Nov-2019 JY Added ROA in Grand Total
                                    ", " + (Configuration.convertNullToBoolean(Configuration.CInfo.PrintStationCloseDateTime) == false?0:1) + " AS NewDateFormat" +   //Sprint-23 - PRIMEPOS-2244 19-May-2016 JY Added 
                            " FROM  " +
									" StationCloseHeader As StationClose, util_POSSet ps where ps.stationid=stationclose.stationid ";
	
				sSQL = sSQL + buildCriteria();
				sSQL += " ORDER BY StationClose.StationCloseID";
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
			if (optByName.CheckedIndex == 0 )
			{
				if (dtpFromDate.Value.ToString()!="")
					sCriteria = " AND " + " Convert(smalldatetime,Convert(Varchar,CloseDate,107)) >= '" + dtpFromDate.Text + "'";
				if (dtpToDate.Value.ToString()!="")
					sCriteria = sCriteria + " AND " + " Convert(smalldatetime,Convert(Varchar,CloseDate,107)) <= '" + dtpToDate.Text + "'";
			}
			else
			{
				int CloseNoFrom = (int)txtCloseNoFrom.Value;
				int CloseNoTo = (int)txtCloseNoTo.Value;

				if (CloseNoFrom!=0)
					sCriteria =  sCriteria +  " AND StationCloseID >= " + CloseNoFrom ;
				if (CloseNoTo!=0)
					sCriteria =  sCriteria +  " AND StationCloseID <= " + CloseNoTo ;
			}
			if (txtStationId.Text.Trim().Replace("'","''")!="")
				sCriteria = sCriteria + " AND PS.STATIONID = '" + txtStationId.Text + "'" ;
			
			return sCriteria;
		}

		private void frmRptCloseStationSummary_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void optByName_ValueChanged(object sender, System.EventArgs e)
		{
			pnlByCloseDate.Visible = false;
			pnlByCloseNo.Visible = false;

			if (optByName.CheckedIndex ==0)
				pnlByCloseDate.Visible = true;
			else
				pnlByCloseNo.Visible = true;

		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			this.optByName.Focus();
			Preview(false);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			this.optByName.Focus();
			Preview(true);
		}


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}
