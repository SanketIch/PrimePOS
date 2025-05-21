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
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
using System.Timers;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	/// 
	public  enum InventoryReportTypeENUM
	{
		ItemFileListing = 1 ,
		InventoryStatusReport = 2 ,
		PhysicalInventorySheet = 3
	}

	public class frmRptItemFileListing : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox gbItemFileListing;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSuppresNeg;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSuppres0;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendor;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLocation;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSaleType;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSeasonCode;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtProductCode;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDepartment;
		private Infragistics.Win.Misc.UltraLabel ultraLabel6;
		private Infragistics.Win.Misc.UltraLabel ultraLabel5;
		private Infragistics.Win.Misc.UltraLabel ultraLabel4;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private System.Windows.Forms.GroupBox ultraGroupBox2;
		private Infragistics.Win.UltraWinEditors.UltraOptionSet optByName;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton ultraButton1;
		private Infragistics.Win.Misc.UltraButton btnView;
		public Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor combEditorItemAddedBy;
        private Infragistics.Win.Misc.UltraLabel lblItemAddedBy;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.Misc.UltraLabel lblToDate;
        private Infragistics.Win.Misc.UltraLabel lblFromDate;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIncludeDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToExpiryDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromExpiryDate;
        private Infragistics.Win.Misc.UltraLabel lblToExpiryDate;
        private Infragistics.Win.Misc.UltraLabel lblFromExpiryDate;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIncludeExpiryDateToSearch;
        public Infragistics.Win.Misc.UltraLabel lblMessage;
        private InventoryReportTypeENUM mReportType ;

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;    
        private long iBlinkCnt = 0;
        #endregion

        public frmRptItemFileListing(InventoryReportTypeENUM reportType)
		{
			//
			// Required for Windows Form Designer support
			//

			InitializeComponent();
			mReportType = reportType;

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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem21 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem22 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem23 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptItemFileListing));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem24 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem25 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            this.gbItemFileListing = new System.Windows.Forms.GroupBox();
            this.dtpToExpiryDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromExpiryDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblToExpiryDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblFromExpiryDate = new Infragistics.Win.Misc.UltraLabel();
            this.chkIncludeExpiryDateToSearch = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.combEditorItemAddedBy = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblItemAddedBy = new Infragistics.Win.Misc.UltraLabel();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblToDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.chkIncludeDate = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkSuppresNeg = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkSuppres0 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtVendor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtLocation = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSaleType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSeasonCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtProductCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.optByName = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.gbItemFileListing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToExpiryDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromExpiryDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeExpiryDateToSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.combEditorItemAddedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSuppresNeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSuppres0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeasonCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProductCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optByName)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbItemFileListing
            // 
            this.gbItemFileListing.Controls.Add(this.dtpToExpiryDate);
            this.gbItemFileListing.Controls.Add(this.dtpFromExpiryDate);
            this.gbItemFileListing.Controls.Add(this.lblToExpiryDate);
            this.gbItemFileListing.Controls.Add(this.lblFromExpiryDate);
            this.gbItemFileListing.Controls.Add(this.chkIncludeExpiryDateToSearch);
            this.gbItemFileListing.Controls.Add(this.combEditorItemAddedBy);
            this.gbItemFileListing.Controls.Add(this.lblItemAddedBy);
            this.gbItemFileListing.Controls.Add(this.dtpToDate);
            this.gbItemFileListing.Controls.Add(this.dtpFromDate);
            this.gbItemFileListing.Controls.Add(this.lblToDate);
            this.gbItemFileListing.Controls.Add(this.lblFromDate);
            this.gbItemFileListing.Controls.Add(this.chkIncludeDate);
            this.gbItemFileListing.Controls.Add(this.chkSuppresNeg);
            this.gbItemFileListing.Controls.Add(this.chkSuppres0);
            this.gbItemFileListing.Controls.Add(this.txtVendor);
            this.gbItemFileListing.Controls.Add(this.txtLocation);
            this.gbItemFileListing.Controls.Add(this.txtSaleType);
            this.gbItemFileListing.Controls.Add(this.txtSeasonCode);
            this.gbItemFileListing.Controls.Add(this.txtProductCode);
            this.gbItemFileListing.Controls.Add(this.txtDepartment);
            this.gbItemFileListing.Controls.Add(this.ultraLabel6);
            this.gbItemFileListing.Controls.Add(this.ultraLabel5);
            this.gbItemFileListing.Controls.Add(this.ultraLabel4);
            this.gbItemFileListing.Controls.Add(this.ultraLabel3);
            this.gbItemFileListing.Controls.Add(this.ultraLabel2);
            this.gbItemFileListing.Controls.Add(this.ultraLabel1);
            this.gbItemFileListing.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbItemFileListing.Location = new System.Drawing.Point(14, 36);
            this.gbItemFileListing.Name = "gbItemFileListing";
            this.gbItemFileListing.Size = new System.Drawing.Size(401, 321);
            this.gbItemFileListing.TabIndex = 0;
            this.gbItemFileListing.TabStop = false;
            this.gbItemFileListing.Text = "Item File Listing";
            // 
            // dtpToExpiryDate
            // 
            this.dtpToExpiryDate.AllowNull = false;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.ItalicAsString = "False";
            appearance1.FontData.StrikeoutAsString = "False";
            appearance1.FontData.UnderlineAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToExpiryDate.Appearance = appearance1;
            this.dtpToExpiryDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToExpiryDate.DateButtons.Add(dateButton1);
            this.dtpToExpiryDate.Enabled = false;
            this.dtpToExpiryDate.Location = new System.Drawing.Point(263, 294);
            this.dtpToExpiryDate.Name = "dtpToExpiryDate";
            this.dtpToExpiryDate.NonAutoSizeHeight = 10;
            this.dtpToExpiryDate.Size = new System.Drawing.Size(123, 21);
            this.dtpToExpiryDate.TabIndex = 37;
            this.dtpToExpiryDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToExpiryDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpToExpiryDate.Visible = false;
            // 
            // dtpFromExpiryDate
            // 
            this.dtpFromExpiryDate.AllowNull = false;
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.ItalicAsString = "False";
            appearance2.FontData.StrikeoutAsString = "False";
            appearance2.FontData.UnderlineAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromExpiryDate.Appearance = appearance2;
            this.dtpFromExpiryDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromExpiryDate.DateButtons.Add(dateButton2);
            this.dtpFromExpiryDate.Enabled = false;
            this.dtpFromExpiryDate.Location = new System.Drawing.Point(68, 294);
            this.dtpFromExpiryDate.Name = "dtpFromExpiryDate";
            this.dtpFromExpiryDate.NonAutoSizeHeight = 10;
            this.dtpFromExpiryDate.Size = new System.Drawing.Size(123, 21);
            this.dtpFromExpiryDate.TabIndex = 35;
            this.dtpFromExpiryDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromExpiryDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpFromExpiryDate.Visible = false;
            // 
            // lblToExpiryDate
            // 
            this.lblToExpiryDate.Enabled = false;
            this.lblToExpiryDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToExpiryDate.Location = new System.Drawing.Point(217, 297);
            this.lblToExpiryDate.Name = "lblToExpiryDate";
            this.lblToExpiryDate.Size = new System.Drawing.Size(29, 15);
            this.lblToExpiryDate.TabIndex = 36;
            this.lblToExpiryDate.Text = "To";
            this.lblToExpiryDate.Visible = false;
            // 
            // lblFromExpiryDate
            // 
            this.lblFromExpiryDate.Enabled = false;
            this.lblFromExpiryDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromExpiryDate.Location = new System.Drawing.Point(24, 297);
            this.lblFromExpiryDate.Name = "lblFromExpiryDate";
            this.lblFromExpiryDate.Size = new System.Drawing.Size(44, 15);
            this.lblFromExpiryDate.TabIndex = 34;
            this.lblFromExpiryDate.Text = "From";
            this.lblFromExpiryDate.Visible = false;
            // 
            // chkIncludeExpiryDateToSearch
            // 
            this.chkIncludeExpiryDateToSearch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludeExpiryDateToSearch.Location = new System.Drawing.Point(24, 273);
            this.chkIncludeExpiryDateToSearch.Name = "chkIncludeExpiryDateToSearch";
            this.chkIncludeExpiryDateToSearch.Size = new System.Drawing.Size(279, 20);
            this.chkIncludeExpiryDateToSearch.TabIndex = 33;
            this.chkIncludeExpiryDateToSearch.Text = "Include Exp. Date To Search";
            this.chkIncludeExpiryDateToSearch.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIncludeExpiryDateToSearch.Visible = false;
            this.chkIncludeExpiryDateToSearch.CheckedChanged += new System.EventHandler(this.chkIncludeExpiryDateToSearch_CheckedChanged);
            // 
            // combEditorItemAddedBy
            // 
            appearance3.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance3.BorderColor3DBase = System.Drawing.Color.Black;
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.ItalicAsString = "False";
            appearance3.FontData.StrikeoutAsString = "False";
            appearance3.FontData.UnderlineAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.combEditorItemAddedBy.Appearance = appearance3;
            this.combEditorItemAddedBy.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.combEditorItemAddedBy.ButtonAppearance = appearance4;
            this.combEditorItemAddedBy.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem21.DataValue = "A";
            valueListItem21.DisplayText = "All";
            valueListItem22.DataValue = "M";
            valueListItem22.DisplayText = "Manually";
            valueListItem23.DataValue = "E";
            valueListItem23.DisplayText = "PrimePO";
            this.combEditorItemAddedBy.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem21,
            valueListItem22,
            valueListItem23});
            this.combEditorItemAddedBy.Location = new System.Drawing.Point(262, 247);
            this.combEditorItemAddedBy.MaxLength = 20;
            this.combEditorItemAddedBy.Name = "combEditorItemAddedBy";
            this.combEditorItemAddedBy.Size = new System.Drawing.Size(121, 20);
            this.combEditorItemAddedBy.TabIndex = 32;
            this.combEditorItemAddedBy.Visible = false;
            // 
            // lblItemAddedBy
            // 
            this.lblItemAddedBy.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemAddedBy.Location = new System.Drawing.Point(23, 252);
            this.lblItemAddedBy.Name = "lblItemAddedBy";
            this.lblItemAddedBy.Size = new System.Drawing.Size(91, 15);
            this.lblItemAddedBy.TabIndex = 31;
            this.lblItemAddedBy.Text = "Item Added By";
            this.lblItemAddedBy.Visible = false;
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.ItalicAsString = "False";
            appearance5.FontData.StrikeoutAsString = "False";
            appearance5.FontData.UnderlineAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance5;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton3);
            this.dtpToDate.Enabled = false;
            this.dtpToDate.Location = new System.Drawing.Point(262, 215);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(123, 21);
            this.dtpToDate.TabIndex = 30;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpToDate.Visible = false;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.ItalicAsString = "False";
            appearance6.FontData.StrikeoutAsString = "False";
            appearance6.FontData.UnderlineAsString = "False";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance6;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton4);
            this.dtpFromDate.Enabled = false;
            this.dtpFromDate.Location = new System.Drawing.Point(72, 215);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
            this.dtpFromDate.TabIndex = 28;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpFromDate.Visible = false;
            // 
            // lblToDate
            // 
            this.lblToDate.Enabled = false;
            this.lblToDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(217, 218);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(29, 15);
            this.lblToDate.TabIndex = 29;
            this.lblToDate.Text = "To";
            this.lblToDate.Visible = false;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Enabled = false;
            this.lblFromDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(25, 218);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(44, 15);
            this.lblFromDate.TabIndex = 27;
            this.lblFromDate.Text = "From";
            this.lblFromDate.Visible = false;
            // 
            // chkIncludeDate
            // 
            this.chkIncludeDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludeDate.Location = new System.Drawing.Point(25, 190);
            this.chkIncludeDate.Name = "chkIncludeDate";
            this.chkIncludeDate.Size = new System.Drawing.Size(279, 20);
            this.chkIncludeDate.TabIndex = 26;
            this.chkIncludeDate.Text = "Include Date To Search";
            this.chkIncludeDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIncludeDate.Visible = false;
            this.chkIncludeDate.CheckedChanged += new System.EventHandler(this.chkIncludeDate_CheckedChanged);
            // 
            // chkSuppresNeg
            // 
            this.chkSuppresNeg.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSuppresNeg.Location = new System.Drawing.Point(23, 222);
            this.chkSuppresNeg.Name = "chkSuppresNeg";
            this.chkSuppresNeg.Size = new System.Drawing.Size(280, 20);
            this.chkSuppresNeg.TabIndex = 20;
            this.chkSuppresNeg.Text = "Suppress Item with negative Quantity";
            this.chkSuppresNeg.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // chkSuppres0
            // 
            this.chkSuppres0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSuppres0.Location = new System.Drawing.Point(25, 194);
            this.chkSuppres0.Name = "chkSuppres0";
            this.chkSuppres0.Size = new System.Drawing.Size(279, 20);
            this.chkSuppres0.TabIndex = 19;
            this.chkSuppres0.Text = "Suppress Item with 0 Quantity";
            this.chkSuppres0.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // txtVendor
            // 
            this.txtVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            editorButton1.Appearance = appearance7;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtVendor.ButtonsRight.Add(editorButton1);
            this.txtVendor.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendor.Location = new System.Drawing.Point(263, 164);
            this.txtVendor.MaxLength = 20;
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.ReadOnly = true;
            this.txtVendor.Size = new System.Drawing.Size(123, 20);
            this.txtVendor.TabIndex = 11;
            this.txtVendor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendor_EditorButtonClick);
            // 
            // txtLocation
            // 
            this.txtLocation.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocation.Location = new System.Drawing.Point(263, 136);
            this.txtLocation.MaxLength = 20;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(123, 20);
            this.txtLocation.TabIndex = 9;
            // 
            // txtSaleType
            // 
            this.txtSaleType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtSaleType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaleType.Location = new System.Drawing.Point(263, 108);
            this.txtSaleType.MaxLength = 20;
            this.txtSaleType.Name = "txtSaleType";
            this.txtSaleType.Size = new System.Drawing.Size(123, 20);
            this.txtSaleType.TabIndex = 7;
            // 
            // txtSeasonCode
            // 
            this.txtSeasonCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtSeasonCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSeasonCode.Location = new System.Drawing.Point(263, 80);
            this.txtSeasonCode.MaxLength = 20;
            this.txtSeasonCode.Name = "txtSeasonCode";
            this.txtSeasonCode.Size = new System.Drawing.Size(123, 20);
            this.txtSeasonCode.TabIndex = 5;
            // 
            // txtProductCode
            // 
            this.txtProductCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtProductCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductCode.Location = new System.Drawing.Point(263, 52);
            this.txtProductCode.MaxLength = 20;
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(123, 20);
            this.txtProductCode.TabIndex = 3;
            // 
            // txtDepartment
            // 
            this.txtDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            editorButton2.Appearance = appearance8;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton2.Text = "";
            this.txtDepartment.ButtonsRight.Add(editorButton2);
            this.txtDepartment.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepartment.Location = new System.Drawing.Point(263, 23);
            this.txtDepartment.MaxLength = 20;
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(123, 20);
            this.txtDepartment.TabIndex = 1;
            this.txtDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDepartment_EditorButtonClick);
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(24, 167);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel6.TabIndex = 10;
            this.ultraLabel6.Text = "Vendor";
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(24, 139);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel5.TabIndex = 8;
            this.ultraLabel5.Text = "Location";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(24, 111);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel4.TabIndex = 6;
            this.ultraLabel4.Text = "Sale Type";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(24, 83);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel3.TabIndex = 4;
            this.ultraLabel3.Text = "Season Code";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(24, 55);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "SKU Code";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(24, 27);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Department";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.optByName);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(14, 357);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(401, 43);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Sort Item By";
            // 
            // optByName
            // 
            this.optByName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optByName.CheckedIndex = 0;
            appearance9.FontData.BoldAsString = "False";
            this.optByName.ItemAppearance = appearance9;
            this.optByName.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem24.DataValue = 1;
            valueListItem24.DisplayText = "Name";
            valueListItem25.DataValue = 2;
            valueListItem25.DisplayText = "Code";
            this.optByName.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem24,
            valueListItem25});
            this.optByName.ItemSpacingHorizontal = 50;
            this.optByName.Location = new System.Drawing.Point(204, 18);
            this.optByName.Name = "optByName";
            this.optByName.Size = new System.Drawing.Size(182, 20);
            this.optByName.TabIndex = 0;
            this.optByName.Text = "Name";
            this.optByName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 401);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(401, 47);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            this.btnClose.Appearance = appearance10;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(301, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraButton1
            // 
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.SystemColors.Control;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.ultraButton1.Appearance = appearance11;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.ultraButton1.HotTrackAppearance = appearance12;
            this.ultraButton1.Location = new System.Drawing.Point(117, 14);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "&Print";
            this.ultraButton1.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // btnView
            // 
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.SystemColors.Control;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.btnView.Appearance = appearance13;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnView.HotTrackAppearance = appearance14;
            this.btnView.Location = new System.Drawing.Point(209, 15);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 1;
            this.btnView.Text = "&View";
            this.btnView.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
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
            this.lblTransactionType.Size = new System.Drawing.Size(424, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Text = "Item File Listing";
            this.lblTransactionType.Click += new System.EventHandler(this.lblTransactionType_Click);
            // 
            // lblMessage
            // 
            appearance16.ForeColor = System.Drawing.Color.Red;
            appearance16.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance16;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 451);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(424, 20);
            this.lblMessage.TabIndex = 32;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "cost price is hidden due to the user does not have enough permissions";
            this.lblMessage.Visible = false;
            // 
            // frmRptItemFileListing
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(424, 471);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.gbItemFileListing);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptItemFileListing";
            this.Load += new System.EventHandler(this.frmInventoryReports_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptItemFileListing_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptItemFileListing_KeyUp);
            this.gbItemFileListing.ResumeLayout(false);
            this.gbItemFileListing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToExpiryDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromExpiryDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeExpiryDateToSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.combEditorItemAddedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIncludeDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSuppresNeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSuppres0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaleType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeasonCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProductCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optByName)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmInventoryReports_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
			switch(mReportType)
			{
				case InventoryReportTypeENUM.InventoryStatusReport:
					this.lblTransactionType.Text = this.Text = "Inventory Status Report";
					gbItemFileListing.Text = "Inventory Status Report";
					break;
				case InventoryReportTypeENUM.ItemFileListing:
					this.lblTransactionType.Text = this.Text = "Item File Listing";
					gbItemFileListing.Text = "Item File Listing";
                    chkIncludeDate.Visible = true;
                    lblFromDate.Visible = true;
                    dtpFromDate.Visible = true;
                    lblToDate.Visible = true;
                    dtpToDate.Visible = true;
                    lblItemAddedBy.Visible = true;
                    combEditorItemAddedBy.Visible = true;
					chkSuppres0.Visible = false;
					chkSuppresNeg.Visible = false;
                    #region Sprint-21 - 2206 13-Jul-2015 JY Added
                    chkIncludeExpiryDateToSearch.Visible = true;
                    lblFromExpiryDate.Visible = true;
                    dtpFromExpiryDate.Visible = true;
                    lblToExpiryDate.Visible = true;
                    dtpToExpiryDate.Visible = true;
                    #endregion
                    break;
				case InventoryReportTypeENUM.PhysicalInventorySheet:
					this.lblTransactionType.Text = this.Text = "Physical Inventory Sheet";
					gbItemFileListing.Text = "Physical Inventory Sheet";
					break;
			}
			this.txtDepartment.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtDepartment.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtLocation.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtLocation.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtProductCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtProductCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtSaleType.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtSaleType.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtSeasonCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtSeasonCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			

			this.txtVendor.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtVendor.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            //Start : Added By Amit Date 21 Apr 2011
            this.dtpToDate.Value = DateTime.Today;
            this.dtpFromDate.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(7, 0, 0, 0));
            this.combEditorItemAddedBy.SelectedIndex = 0;
            this.dtpToExpiryDate.Value = DateTime.Today;    //Sprint-21 - 2206 13-Jul-2015 JY Added
            this.dtpFromExpiryDate.Value = DateTime.Today.Date.Subtract(new System.TimeSpan(7, 0, 0, 0));   //Sprint-21 - 2206 13-Jul-2015 JY Added

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

        private void btnPrint_Click(object sender, System.EventArgs e)
		{
			Preview(true);
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void Preview(bool PrintIt)
		{
			try
			{
				string sSQL  = ""; 
				ReportClass oRpt =null;                               

                switch ( mReportType)
				{
					case InventoryReportTypeENUM.InventoryStatusReport:
						oRpt = new rptIRInventorystatus();
						sSQL = " SELECT " + 
									" Itm.ItemID" +
									" , Itm.Description" +
                                    " , Vend.VendorCode" +
									" , Dept.DeptCode" +
                                    " , itm.Location" +
                                    " , itm.ProductCode" +
                                    " , itm.QtyInStock" +
                                    " , itm.LastCostPrice" +
                                    " , itm.SellingPrice " +
                                    ", " + nDisplayItemCost + " AS DisplayItemCost" +   //PRIMEPOS-2464 09-Mar-2020 JY Added
                                " FROM " +
									" Item itm LEFT OUTER JOIN Department Dept ON itm.DepartmentID = Dept.DeptID" +
                                    //" LEFT OUTER JOIN Vendor Vend ON (itm.LastVendor = Vend.VendorCode OR itm.PREFERREDVENDOR = Vend.VendorCode) WHERE 1=1";  //Sprint-23 - PRIMEPOS-2298 25-May-2016 JY Commented
                                    " LEFT OUTER JOIN Vendor Vend ON (Vend.VendorCode = (CASE WHEN ISNULL(itm.LastVendor,'') = '' THEN ISNULL(itm.PREFERREDVENDOR,'') ELSE ISNULL(itm.LastVendor,'') END )) WHERE 1=1";    //Sprint-23 - PRIMEPOS-2298 25-May-2016 JY Added  - We can avoid the duplicate records means same quantity appears against two different vendors. We can achieve this by considering the "LastVendor" in item table, if it is NULL then will consider "Preferred Vendor".
                        break;
                    //Added Item.ItemAddedDate by shitaljit(QuicSolv)on 18-04-2011
                    //made a new report rptIRFileListingDetai
                    // to generate report of items added on a particular date
					case InventoryReportTypeENUM.ItemFileListing:
                        #region Sprint-21 - 2206 14-Jul-2015 JY Added
                        oRpt = new rptIRFileListing();

                        sSQL = "SELECT itm.ItemID, itm.Description, Vend.VendorCode, Dept.DeptCode, itm.Location," +
                                   " itm.ProductCode, itm.SaleTypeCode, itm.Unit, itm.LastCostPrice, itm.SellingPrice as SellingPrice," +
                                   " itm.isTaxable, itm.QtyInStock, itm.ReOrderLevel, itm.ExpDate " +
                                   ", " + nDisplayItemCost + " AS DisplayItemCost" +   //PRIMEPOS-2464 09-Mar-2020 JY Added
                                   " FROM Item itm LEFT OUTER JOIN Department Dept ON itm.DepartmentID = Dept.DeptID" +
                                   " LEFT OUTER JOIN Vendor Vend ON itm.LastVendor = Vend.VendorCode" +
                                   " where 1 = 1 ";

                        string ItemPriceHistory = string.Empty;
                        if (combEditorItemAddedBy.SelectedItem.DataValue.ToString() != "A")
                        {
                            ItemPriceHistory = "And IP.UPDATEDBY = '" + combEditorItemAddedBy.SelectedItem.DataValue + "'";
                        }
                        if (chkIncludeDate.Checked)
                        {
                            ItemPriceHistory += " And Convert(date, IP.AddedOn) >= Convert(date, '" + dtpFromDate.Text + "') " +
                                                " And Convert(date, IP.AddedOn) <= Convert(date, '" + dtpToDate.Text + "')";
                        }
                        if (ItemPriceHistory != "")
                        { 
                            ItemPriceHistory = " AND itm.ItemId = (select TOP 1 IP.ItemID from ItemPriceHistory IP " +
                                   " where IP.ItemID = Itm.ItemID  " + ItemPriceHistory + "  ORDER BY ID DESC)";
                            sSQL += ItemPriceHistory;
                        }
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtItemAddedBy"]).Text = combEditorItemAddedBy.SelectedItem.DisplayText;
                        #endregion

                        #region Sprint-21 - 2206 14-Jul-2015 JY Commented
                        //if (chkIncludeDate.Checked)
                        //{
                        //    oRpt = new rptIRFileListingDetail();
                        //    /*Date : 1/17/2014
                        //     * Added By  : Shitaljit  Ticket : 1714
                        //     * Items are showing multiple times in Inventory Management=> Inventory Reports=> Item file listing report. */

                        //    #region "Old Code"
                        //    /*Old Code*/
                        //    //sSQL = " SELECT " +
                        //    //        " itm.ItemID " +
                        //    //        " , itm.Description " +
                        //    //        ",  Vend.VendorCode" +
                        //    //        " , Dept.DeptCode " +
                        //    //        " , itm.Location " +
                        //    //        " , itm.ProductCode " +
                        //    //        " , itm.SaleTypeCode " +
                        //    //        " , itm.Unit " +
                        //    //        " , Tax.TaxCode " +
                        //    //        " , itm.LastCostPrice " +
                        //    //    //" , itm.SellingPrice " +//Commented By shitaljit(QuicSolv) on 23 Sept 2011
                        //    //        " , itmph.SalePrice as SellingPrice" +//Added By shitaljit(QuicSolv) on 23 Sept 2011
                        //    //        " , itm.isTaxable " +
                        //    //        " , itm.QtyInStock " +
                        //    //        " , itm.ReOrderLevel " +
                        //    //    //" , itm.ItemAddedDate  " +//Commented By shitaljit(QuicSolv) on 23 Sept 2011
                        //    //        " , itmph.AddedOn as ItemAddedDate  " +//Added By shitaljit(QuicSolv) on 23 Sept 2011
                        //    //    " FROM 	" +
                        //    //        " Item itm LEFT OUTER JOIN Department Dept ON (itm.DepartmentID = Dept.DeptID) " +
                        //    //        " LEFT OUTER JOIN  TaxCodes Tax ON (itm.TaxID = Tax.TaxID)  " +
                        //    //        " LEFT OUTER JOIN  Vendor Vend ON (itm.LastVendor = Vend.VendorCode) "+
                        //    //        //Added By Amit Date 22 Apr 2011
                        //    //        " LEFT OUTER JOIN ItemPriceHistory itmph ON (itm.ItemId=itmph.ItemId)";
                        //    #endregion

                        //    /*New Code*/
                        //    string Addedby = "";
                        //    if (combEditorItemAddedBy.SelectedItem.DataValue == "M")
                        //        Addedby = "And IP.UPDATEDBY = 'M'";
                        //    else if (combEditorItemAddedBy.SelectedItem.DataValue == "E")
                        //        Addedby = "And IP.UPDATEDBY = 'E'";
                        //    else Addedby = string.Empty;
                        //    Addedby = Addedby + " And Convert(date, AddedOn) >= Convert(date, '"+dtpFromDate.Text+"') "+
                        //        "And Convert(date, AddedOn) <= Convert(date, '"+dtpToDate.Text+"')";

                        //    sSQL = "SELECT  itm.ItemID  , itm.Description ,  Vend.VendorCode , Dept.DeptCode  , itm.Location , " +
                        //           " itm.ProductCode  , itm.SaleTypeCode  , itm.Unit  , Tax.TaxCode  , itm.LastCostPrice  , itm.SellingPrice as SellingPrice , " +
                        //           "itm.isTaxable  , itm.QtyInStock  , itm.ReOrderLevel  " +
                        //           " FROM Item itm LEFT OUTER JOIN Department Dept ON (itm.DepartmentID = Dept.DeptID)" +
                        //           " LEFT OUTER JOIN  TaxCodes Tax ON (itm.TaxID = Tax.TaxID)  " +
                        //           " LEFT OUTER JOIN  Vendor Vend ON (itm.LastVendor = Vend.VendorCode)" +
                        //           " where itm.ItemId= (select TOP 1 ItemID from ItemPriceHistory IP " +
                        //           " where IP.ItemID = Itm.ItemID " + Addedby + " ORDER BY ID DESC)";
                        //}
                        //else 
                        //{
                        //    oRpt = new rptIRFileListing();
                        //    /*Date : 1/17/2014
                        //     * Added By  : Shitaljit Ticket : 1714
                        //     * Items are showing multiple times in Inventory Management=> Inventory Reports=> Item file listing report. */

                        //    #region "Old Code"
                        //    /* Old Code */
                        //    //sSQL = " SELECT " +
                        //    //        " itm.ItemID " +
                        //    //        " , itm.Description " +
                        //    //        ",  Vend.VendorCode" +
                        //    //        " , Dept.DeptCode " +
                        //    //        " , itm.Location " +
                        //    //        " , itm.ProductCode " +
                        //    //        " , itm.SaleTypeCode " +
                        //    //        " , itm.Unit " +
                        //    //        " , Tax.TaxCode " +
                        //    //        " , itm.LastCostPrice " +
                        //    //        //" , itm.SellingPrice " +//Commented By shitaljit(QuicSolv) on 23 Sept 2011
                        //    //        " , itmph.SalePrice as SellingPrice " +//Added By shitaljit(QuicSolv) on 23 Sept 2011
                        //    //        " , itm.isTaxable " +
                        //    //        " , itm.QtyInStock " +
                        //    //        " , itm.ReOrderLevel " +
                                    
                        //    //    " FROM 	" +
                        //    //        " Item itm LEFT OUTER JOIN Department Dept ON (itm.DepartmentID = Dept.DeptID) " +
                        //    //        " LEFT OUTER JOIN  TaxCodes Tax ON (itm.TaxID = Tax.TaxID)  " +
                        //    //        " LEFT OUTER JOIN  Vendor Vend ON (itm.LastVendor = Vend.VendorCode) "+
                        //    //          //Added By Amit Date 22 Apr 2011
                        //    //        " LEFT OUTER JOIN ItemPriceHistory itmph ON (itm.ItemId=itmph.ItemId)";

                        //    #endregion

                        //    /*New Code*/

                        //    string Addedby = "";
                        //    if (combEditorItemAddedBy.SelectedItem.DataValue == "M")
                        //        Addedby = "And IP.UPDATEDBY = 'M'";
                        //    else if (combEditorItemAddedBy.SelectedItem.DataValue == "E")
                        //        Addedby = "And IP.UPDATEDBY = 'E'";
                        //    else Addedby = string.Empty;

                        //    sSQL = "SELECT  itm.ItemID  , itm.Description ,  Vend.VendorCode , Dept.DeptCode  , itm.Location , " +
                        //           " itm.ProductCode  , itm.SaleTypeCode  , itm.Unit  , Tax.TaxCode  , itm.LastCostPrice  , itm.SellingPrice as SellingPrice , " +
                        //           "itm.isTaxable  , itm.QtyInStock  , itm.ReOrderLevel  " +
                        //           " FROM Item itm LEFT OUTER JOIN Department Dept ON (itm.DepartmentID = Dept.DeptID)" +
                        //           " LEFT OUTER JOIN  TaxCodes Tax ON (itm.TaxID = Tax.TaxID)  " +
                        //           " LEFT OUTER JOIN  Vendor Vend ON (itm.LastVendor = Vend.VendorCode)" +
                        //           " where itm.ItemId= (select TOP 1 ItemID from ItemPriceHistory IP " +
                        //           " where IP.ItemID = Itm.ItemID  " + Addedby + "  ORDER BY ID DESC)";
                        //}
                        #endregion
						break;
					case InventoryReportTypeENUM.PhysicalInventorySheet:
						oRpt = new rptIRPhysicalInventorySheet();

						sSQL = " SELECT " +
									"itm.ItemID" +
									", itm.Description" +
									", Dept.DeptCode" +
                                    ", itm.Location" +
                                    ", itm.ProductCode" +
                                    ", itm.QtyInStock" +
									", '------------' ActualQuantity" +
                                    ", itm.SellingPrice" +
                                    ", itm.LastCostPrice " +
                                    ", " + nDisplayItemCost + " AS DisplayItemCost" +   //PRIMEPOS-2464 09-Mar-2020 JY Added
                                " FROM " +
									" Item itm LEFT OUTER JOIN Department Dept ON itm.DepartmentID = Dept.DeptID" +
									" LEFT OUTER JOIN Vendor Vend ON itm.LastVendor = Vend.VendorCode WHERE 1=1";
						break;
				}
				//
				//Modified By Amit Date 22 Apr 2011
				//sSQL = sSQL + buildCriteria();    //Original
                //if(mReportType != InventoryReportTypeENUM.ItemFileListing)    //Sprint-21 - 2206 14-Jul-2015 JY Commented
                sSQL = sSQL + buildCriteria(oRpt);
                //End
				clsReports.Preview(PrintIt,sSQL,oRpt);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private string buildCriteria(ReportClass oRpt)
		{
			string sCriteria = string.Empty;
            #region Sprint-21 - 2206 14-Jul-2015 JY Added
            if (txtDepartment.Text.Trim() != "")
                sCriteria += " AND Dept.DeptCode = '" + txtDepartment.Text.Trim().Replace("'", "''") + "'";
            if (txtLocation.Text.Trim() != "")
                sCriteria += " AND itm.Location = '" + txtLocation.Text.Trim().Replace("'", "''") + "'";
            if (txtProductCode.Text.Trim() != "")
                sCriteria += " AND itm.ProductCode = '" + txtProductCode.Text.Trim().Replace("'", "''") + "'";
            if (txtSaleType.Text.Trim() != "")
                sCriteria += " AND itm.SaleTypeCode = '" + txtSaleType.Text.Trim().Replace("'", "''") + "'";
            if (txtSeasonCode.Text.Trim() != "")
                sCriteria += " AND itm.SeasonCode = '" + txtSeasonCode.Text.Trim().Replace("'", "''") + "'";
            if (txtVendor.Text.Trim() != "")
                sCriteria += " AND Vend.VendorCode IN (" + this.txtVendor.Tag.ToString() + ")";
            
            if (mReportType != InventoryReportTypeENUM.ItemFileListing)
            {
                if (chkSuppres0.Enabled && chkSuppres0.Checked)
                    sCriteria += " AND isnull(itm.QtyInStock,0) <> 0 ";
                if (chkSuppresNeg.Enabled && chkSuppresNeg.Checked)
                    sCriteria += " AND isnull(itm.QtyInStock,0) >= 0 ";
            }

            if (mReportType == InventoryReportTypeENUM.ItemFileListing)
            {
                if (chkIncludeExpiryDateToSearch.Checked)
                    sCriteria += " AND Convert(date, itm.ExpDate) >= Convert(date, '" + dtpFromExpiryDate.Text + "') " +
                                " And Convert(date, itm.ExpDate) <= Convert(date, '" + dtpToExpiryDate.Text + "')";
            }
            sCriteria += " Order By " + ((optByName.CheckedIndex == 1) ? " itm.ItemId " : "itm.Description");
            #endregion

            #region Sprint-21 - 2206 14-Jul-2015 JY Commented
            //if (txtDepartment.Text.Trim().Replace("'","''")!="")
            //    sCriteria = sCriteria  +  ((sCriteria=="")? " WHERE " : " AND ") + " DeptCode = '" + txtDepartment.Text + "'";
            //if (txtLocation.Text.Trim().Replace("'","''")!="")
            //    sCriteria = sCriteria +  ((sCriteria=="")? " WHERE " : " AND ") + " Location = '" + txtLocation.Text + "'";
            //if (txtProductCode.Text.Trim().Replace("'","''")!="")
            //    sCriteria = sCriteria +  ((sCriteria=="")? " WHERE " : " AND ") + " ProductCode = '" + txtProductCode.Text + "'";
            //if (txtSaleType.Text.Trim().Replace("'","''")!="")
            //    sCriteria = sCriteria +  ((sCriteria=="")? " WHERE " : " AND ") + " SaleTypeCode = '" + txtSaleType.Text + "'";
            //if (txtSeasonCode.Text.Trim().Replace("'","''")!="")
            //    sCriteria = sCriteria +  ((sCriteria=="")? " WHERE " : " AND ") + " SeasonCode = '" + txtSeasonCode.Text + "'";
            //if (txtVendor.Text.Trim().Replace("'", "''") != "")
            //{
            //    if (mReportType == InventoryReportTypeENUM.InventoryStatusReport)
            //    {
            //        sCriteria = sCriteria + ((sCriteria == "") ? " WHERE " : " AND ") + " VendorCode IN (" + this.txtVendor.Tag.ToString().Trim() + ")";
            //    }
            //    else
            //    {
            //        sCriteria = sCriteria + ((sCriteria == "") ? " WHERE " : " AND ") + " VendorCode = '" + txtVendor.Text + "'";
            //    }
            //}
            ////Start: Added By Amit Date 22 Apr 2011
            //if (chkIncludeDate.Checked&&chkIncludeDate.Visible==true)
            //{
            //  if ((dtpFromDate.Value.ToString()) != ""&&(dtpToDate.Value.ToString())!="")
            //     sCriteria = sCriteria + ((sCriteria == "") ? " WHERE " : " AND ") + " convert(datetime,itmph.AddedOn,109) between convert(datetime, cast('" + this.dtpFromDate.Text + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" +this.dtpToDate.Text + " 23:59:59' as datetime) ,113) ";
            //}
            //if(combEditorItemAddedBy.SelectedItem.ToString()=="All"&&combEditorItemAddedBy.Visible==true)
            //{
            //    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtItemAddedBy"]).Text ="All";
            //}
            //else if (combEditorItemAddedBy.SelectedItem.ToString() == "Manually"&&combEditorItemAddedBy.Visible==true)
            //{
            //    sCriteria = sCriteria + ((sCriteria == "") ? " WHERE " : " AND ") + "UpdatedBy  = 'M'";
            //    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtItemAddedBy"]).Text = "Manually";
            //}
            //else if (combEditorItemAddedBy.SelectedItem.ToString() == "PrimePO"&&combEditorItemAddedBy.Visible==true)
            //{
            //    sCriteria = sCriteria + ((sCriteria == "") ? " WHERE " : " AND ") + "UpdatedBy  = 'E'";
            //    ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtItemAddedBy"]).Text = "PrimePO";
            //}
            ////End
            //if (chkSuppres0.Enabled && chkSuppres0.Checked)
            //{
            //    if (sCriteria.Trim()=="") sCriteria=" where "; else sCriteria+=" and ";
            //    sCriteria = sCriteria + " isnull(QtyInStock,0) <> 0 ";
            //}

            //if (chkSuppresNeg.Enabled && chkSuppresNeg.Checked)
            //{
            //    if (sCriteria.Trim()=="") sCriteria=" where "; else sCriteria+=" and ";
            //    sCriteria = sCriteria + " isnull(QtyInStock,0) >= 0 ";
            //}

            //sCriteria = sCriteria + "  Order By " + ((optByName.CheckedIndex==1)? " itm.ItemId " : "itm.Description");
            #endregion

            return sCriteria;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmRptItemFileListing_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void gbItemFileListing_Enter(object sender, System.EventArgs e)
		{
			optByName.Focus();
		}

		private void ultraGroupBox2_Enter(object sender, System.EventArgs e)
		{
			txtDepartment.Focus();
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
            //Start : Added By Amit Date 23 Apr 2011
            string fieldName = string.Empty;
            try
            {
                #region Sprint-21 - 2206 14-Jul-2015 JY Commented
                //if (!validateFields(out fieldName))
                //{
                //    if (fieldName == "DATE")
                //    {
                //        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                //    }
                //    return;
                //}
                //this.txtDepartment.Focus();
                #endregion
                
                if (ValidateFields())   //Sprint-21 - 2206 14-Jul-2015 JY Added
                    Preview(false);
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
            //End
            //Start : Commented By Amit Date 23 Apr 2011
            //this.txtDepartment.Focus();
            //Preview(false);
            //End
		}

		private void ultraButton1_Click(object sender, System.EventArgs e)
		{
            //Start : Added By Amit Date 23 Apr 2011
            string fieldName = string.Empty;
            try
            {
                #region Sprint-21 - 2206 14-Jul-2015 JY Commented
                //if (!validateFields(out fieldName))
                //{
                //    if (fieldName == "DATE")
                //    {
                //        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                //    }
                //    return;
                //}
                //this.txtDepartment.Focus();
                #endregion

                if (ValidateFields())   //Sprint-21 - 2206 14-Jul-2015 JY Added
                    Preview(true);
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
            //End
            //Start : Commented By Amit Date 23 Apr 2011
			//this.txtDepartment.Focus();
			//Preview(true);
            //End
		}

		private void txtDepartment_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
			if (oSearch.IsCanceled) return;
			txtDepartment.Text = oSearch.SelectedRowID();
		}

		private void txtVendor_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
            oSearch.SearchInConstructor = true;
            oSearch.AllowMultiRowSelect = true;
			oSearch.ShowDialog();
			if (oSearch.IsCanceled) return;
            string strVenCode = "";
            string strDisplayCode = "";
            if (!oSearch.IsCanceled)
            {
                foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                {
                    if ((bool)oRow.Cells["check"].Value == true)
                    {
                        strVenCode += ",'" + oRow.Cells["Code"].Text.Replace("'", "''") + "'";  //Sprint-21 - 2206 14-Jul-2015 JY Added replace to work queries with "'"
                        strDisplayCode += "," + oRow.Cells["Code"].Text.Trim() ;
                    }
                }

                if (strVenCode.StartsWith(","))
                {
                    strVenCode = strVenCode.Substring(1);
                }
                if (strDisplayCode.StartsWith(","))
                {
                    strDisplayCode = strDisplayCode.Substring(1);
                }
                txtVendor.Text = strDisplayCode;
                txtVendor.Tag = strVenCode;
            }
            else
            {
                txtVendor.Text = string.Empty;
                txtVendor.Tag = string.Empty;
            }
			//txtVendor.Text = oSearch.SelectedRowID();

		}

		private void frmRptItemFileListing_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtDepartment.ContainsFocus)
						txtDepartment_EditorButtonClick(null,null);
					else if ( txtVendor.ContainsFocus)
						txtVendor_EditorButtonClick(null,null);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

        private void ultraLabel20_Click(object sender, EventArgs e)
        {

        }

        private void dtpSaleStartDate_BeforeDropDown(object sender, CancelEventArgs e)
        {

        }

        private void lblTransactionType_Click(object sender, EventArgs e)
        {

        }

        private void chkIncludeDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIncludeDate.Checked == true)
            {
                lblFromDate.Enabled = true;
                lblToDate.Enabled = true;
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
            }
            else
            {
                lblFromDate.Enabled = false;
                lblToDate.Enabled = false;
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
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

        private void dtpToDate_Date(object sender, EventArgs e)
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

        #region Sprint-21 - 2206 14-Jul-2015 JY Added
        private bool ValidateFields()
        {
            bool Validationflag = false;
            bool retVal = true;
            string errorMsg = string.Empty;
            try
            {
                if (chkIncludeDate.Checked && ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value))
                {
                    Validationflag = true;
                    errorMsg += "\nFrom Date can not be greater than To Date.";
                }
                if (chkIncludeExpiryDateToSearch.Checked && ((DateTime)dtpFromExpiryDate.Value > (DateTime)dtpToExpiryDate.Value))
                {
                    Validationflag = true;
                    errorMsg = errorMsg + "\nFrom Exp. Date can not be greater than To Exp. Date.";
                }

                if (Validationflag)
                {
                    retVal = false;
                    throw (new Exception(errorMsg));
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return retVal;
        }

        private void chkIncludeExpiryDateToSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIncludeExpiryDateToSearch.Checked == true)
            {
                lblFromExpiryDate.Enabled = true;
                lblToExpiryDate.Enabled = true;
                dtpFromExpiryDate.Enabled = true;
                dtpToExpiryDate.Enabled = true;
            }
            else
            {
                lblFromExpiryDate.Enabled = false;
                lblToExpiryDate.Enabled = false;
                dtpFromExpiryDate.Enabled = false;
                dtpToExpiryDate.Enabled = false;
            }
        }
        #endregion

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
