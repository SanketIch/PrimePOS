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
using System.Collections.Generic;
using System.Timers;
using POS_Core_UI.Reports.Reports;
using POS_Core_UI.Resources;
using POS_Core.Resources;
//using POS_Core_UI.Reports.ReportsUI;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmVendorSearch.
	/// </summary>
	public class frmPOOnHold : System.Windows.Forms.Form
    {
        #region Variable Declaration
          
        private int CurrentX;
		private int CurrentY;
        DataSet datasetVendorID = new DataSet();
        private static frmPOOnHold defaultInstance = null;
        string vendorid = string.Empty;
        #endregion

        #region Windows Object

        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnProcess;
		private Infragistics.Win.Misc.UltraButton btnRefresh;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private IContainer components;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private System.Timers.Timer poAckTimer = null;
        private DataSet oDataSet = null;
        private DataSet dsCommon = null;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboVendorList;
        #endregion
       
		public frmPOOnHold()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			try
			{
                defaultInstance = this;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}		

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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOOnHold));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ColumnName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem16 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnProcess = new Infragistics.Win.Misc.UltraButton();
            this.btnRefresh = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.cboVendorList = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboVendorList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.SystemColors.Control;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance2;
            this.btnClose.Location = new System.Drawing.Point(689, 501);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 26);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.btnProcess.Appearance = appearance3;
            this.btnProcess.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnProcess.Location = new System.Drawing.Point(590, 501);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(93, 26);
            this.btnProcess.TabIndex = 7;
            this.btnProcess.Text = "&Process";
            this.btnProcess.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            this.btnRefresh.Appearance = appearance4;
            this.btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnRefresh.Location = new System.Drawing.Point(392, 501);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(93, 26);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance5.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance5;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(10, 8);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(776, 38);
            this.lblTransactionType.TabIndex = 69;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Process On Hold Purchase Order";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grdSearch
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackColorDisabled = System.Drawing.Color.White;
            appearance6.BackColorDisabled2 = System.Drawing.Color.White;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance6;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance9;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance10.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BackColorDisabled = System.Drawing.Color.White;
            appearance11.BackColorDisabled2 = System.Drawing.Color.White;
            appearance11.BorderColor = System.Drawing.Color.Black;
            appearance11.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            appearance12.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            appearance12.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance12.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance12;
            this.grdSearch.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance14;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.White;
            appearance16.BackColorDisabled = System.Drawing.Color.White;
            appearance16.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance17.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.FontData.BoldAsString = "True";
            appearance18.FontData.SizeInPoints = 10F;
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.TextHAlignAsString = "Left";
            appearance18.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance20.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance20;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance22;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance23.BackColor = System.Drawing.Color.Navy;
            appearance23.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.Navy;
            appearance24.BackColorDisabled = System.Drawing.Color.Navy;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance24.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            appearance24.ForeColor = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance26.BackColor2 = System.Drawing.Color.White;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance26.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance26;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Both;
            this.grdSearch.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(17, 138);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(769, 349);
            this.grdSearch.TabIndex = 5;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSearch_InitializeRow);
            this.grdSearch.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdSearch_AfterRowInsert);
            this.grdSearch.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdSearch_AfterSelectChange);
            this.grdSearch.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdSearch_CellDataError);
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.cboVendorList);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(17, 47);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(769, 77);
            this.gbInventoryReceived.TabIndex = 72;
            this.gbInventoryReceived.TabStop = false;
            // 
            // cboVendorList
            // 
            this.cboVendorList.AutoSize = false;
            this.cboVendorList.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboVendorList.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem16.DataValue = "15";
            valueListItem16.DisplayText = "All";
            this.cboVendorList.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem16});
            this.cboVendorList.Location = new System.Drawing.Point(554, 17);
            this.cboVendorList.Name = "cboVendorList";
            this.cboVendorList.Size = new System.Drawing.Size(206, 21);
            this.cboVendorList.TabIndex = 8;
            this.cboVendorList.SelectionChangeCommitted += new System.EventHandler(this.cboVendorList_SelectionChangeCommitted);
            // 
            // ultraLabel1
            // 
            appearance27.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance27;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.ultraLabel1.Location = new System.Drawing.Point(491, 20);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(76, 15);
            this.ultraLabel1.TabIndex = 7;
            this.ultraLabel1.Text = "Vendor";
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance28.FontData.BoldAsString = "False";
            appearance28.FontData.ItalicAsString = "False";
            appearance28.FontData.StrikeoutAsString = "False";
            appearance28.FontData.UnderlineAsString = "False";
            appearance28.ForeColor = System.Drawing.Color.Black;
            appearance28.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance28;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(329, 17);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(123, 21);
            this.dtpToDate.TabIndex = 2;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance29.FontData.BoldAsString = "False";
            appearance29.FontData.ItalicAsString = "False";
            appearance29.FontData.StrikeoutAsString = "False";
            appearance29.FontData.UnderlineAsString = "False";
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance29;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(113, 17);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(264, 20);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(59, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(16, 20);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(72, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "From Date";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance30.FontData.BoldAsString = "True";
            appearance30.ForeColor = System.Drawing.Color.White;
            appearance30.Image = global::POS_Core_UI.Properties.Resources.close2;
            this.btnDelete.Appearance = appearance30;
            this.btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnDelete.Location = new System.Drawing.Point(491, 501);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(93, 26);
            this.btnDelete.TabIndex = 73;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmPOOnHold
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(798, 543);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.gbInventoryReceived);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOOnHold";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process On Hold Purchase Order";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPOOnHold_FormClosing);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.Shown += new System.EventHandler(this.frmPOOnHold_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.Resize += new System.EventHandler(this.frmPOOnHold_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboVendorList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.ResumeLayout(false);

		}

        void grdSearch_AfterRowInsert(object sender, RowEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
		#endregion


		private void frmSearch_Load(object sender, System.EventArgs e)
		{
			try
			{
				clsUIHelper.SetAppearance(this.grdSearch);
				clsUIHelper.SetReadonlyRow(this.grdSearch);
				this.dtpToDate.Value=DateTime.Now;
				this.dtpFromDate.Value=DateTime.Now.Subtract(new TimeSpan(60,0,0,0));
			    getData();
                LoadVendorList();
                InitializePOAckTimer();
                this.btnProcess.Enabled = false;
                clsUIHelper.setColorSchecme(this);
                
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

        private void InitializePOAckTimer()
        {
            int poAckTime = Configuration.CPrimeEDISetting.PurchaseOrderTimer * 1000*60;   //PRIMEPOS-3167 07-Nov-2022 JY Modified

            poAckTimer = new System.Timers.Timer();
            poAckTimer.Interval = poAckTime;
            poAckTimer.Enabled = true;
            poAckTimer.Elapsed += new ElapsedEventHandler(poAckTimer_Elapsed);                 
        }
        
        void poAckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock(this.poAckTimer)
                {
                    getData();                    
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        
		private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
			   if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
                else if(e.KeyData == System.Windows.Forms.Keys.Escape)
                {
                    System.EventArgs arg = new EventArgs();                    
                    this.btnClose_Click(this.btnClose, arg);
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}      

		private void frmSearchMain_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
		}

		private void grdSearch_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.CurrentX=e.X;
			this.CurrentY=e.Y;
		}

		private void resizeColumns()
		{
			foreach (UltraGridColumn oCol in grdSearch.DisplayLayout.Bands[0].Columns)
			{
				oCol.Width =oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows,true)+10;
				
                if( oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
				{
					oCol.CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right ;
				}              
			}
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            frmMain.GetPOACKForm = "";
            this.Close();
		}
		
	    private void btnRefresh_Click(object sender, System.EventArgs e)
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
                UpdatePOStatus();
                this.grdSearch.DataSource = null;
                getData();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
		}

        private void getData()
        {
            //Added By (SRT)Abhishek  Date : 01/15/2009
            try
            {
                oDataSet = new DataSet();
                dsCommon = new DataSet();
                PurchaseOrder purchaseOrd = new PurchaseOrder();
                PODetailData poDetailData = new PODetailData();
                POHeaderData poHeaderData = new POHeaderData();
                string poAckForm = string.Empty;
                poAckForm = frmMain.GetPOACKForm;


                poHeaderData = purchaseOrd.PopulateListFromHold(" AND OrderDate Between '" + dtpFromDate.Value + "' AND '" + dtpToDate.Value + "' Order By OrderDate DESC");


                oDataSet.Tables.Add(poHeaderData.POHeader.Copy());
                dsCommon.Tables.Add(poHeaderData.POHeader.Copy());
                dsCommon.Tables[0].TableName = poHeaderData.POHeader.TableName.ToString();
                oDataSet.Tables[0].Columns.Add("OrderStatus");
                oDataSet.Tables[0].Columns.Add("VendorAckStatus");
                oDataSet.Tables[0].TableName = "Master";

                if (oDataSet.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                else if (grdSearch.Selected.Rows.Count == 0 && grdSearch.Rows.Count > 0)
                {
                    grdSearch.Selected.Rows.Add(grdSearch.Rows[0]);
                    grdSearch.ActiveRow = grdSearch.Rows[0];
                }

                foreach (POHeaderRow poRow in poHeaderData.POHeader.Rows)
                {
                    PODetailData poDetailDataForOrderID = new PODetailData();
                    poDetailDataForOrderID = purchaseOrd.PopulateDetailFromHold(poRow.OrderID);
                    poDetailData.Merge(poDetailDataForOrderID);
                }

                oDataSet.Tables.Add(poDetailData.PODetail.Copy());
                dsCommon.Tables.Add(poDetailData.PODetail.Copy());
                dsCommon.Tables[1].TableName = poDetailData.PODetail.TableName.ToString();
                oDataSet.Tables[1].TableName = "Detail";
                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["OrderID"], oDataSet.Tables[1].Columns["OrderID"]);
                grdSearch.DataSource = oDataSet;
                this.grdSearch.Refresh();

                grdSearch.DisplayLayout.Bands[0].Columns["Status"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["OrderID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["VendorID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["ExptDeliveryDate"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["isFTPUsed"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["PrimePOrderID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["VendorName"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["AckStatus"].Hidden = true;

                grdSearch.DisplayLayout.Bands[0].Columns["OrderStatus"].Header.VisiblePosition = 6;
                grdSearch.DisplayLayout.Bands[0].Columns["VendorAckStatus"].Header.VisiblePosition = 7;
                grdSearch.DisplayLayout.Bands[1].Columns["PODetailID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns["Comments"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns["OrderID"].Hidden = true;

                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_BestVendor].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty].Hidden = true;

                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_QtySold100Days].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_ReOrderLevel].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock].Hidden = true;

                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductID].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_PackSize].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_PackQuant].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_PackUnit].Hidden = true;

                grdSearch.DisplayLayout.Bands[1].Expandable = true;
                grdSearch.DisplayLayout.Bands[1].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[1].Header.Caption = "Order Detail";
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 12;
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void btnProcess_Click(object sender, System.EventArgs e)
		{         
            try
            {
               POS_Core.ErrorLogging.Logs.Logger("**Process Purchase Order "  );
                if (this.grdSearch.Selected.Rows.Count > 0)
                {

                    UltraGridRow oGRow = this.grdSearch.Selected.Rows[0];
                    if (oGRow == null)
                    {
                       POS_Core.ErrorLogging.Logs.Logger("**Purchase Order Processing Error :Invalid row selected. ");
                        throw (new Exception("Invalid row selected."));
                    }

                    String orderID = oGRow.Cells["OrderID"].Value.ToString().Trim();

                    if (orderID.Trim() == "")
                    {
                       POS_Core.ErrorLogging.Logs.Logger("** Purchase Order Processing Error :File is invalid.\nOrder ID is blank. ");
                        throw (new Exception("File is invalid.\nOrder ID is blank."));
                    }

                    PurchaseOrder oPO = new PurchaseOrder();
                    POS_Core.DataAccess.POHeaderSvr oPOHSvr = new POS_Core.DataAccess.POHeaderSvr();
                    POHeaderData oPOHData = oPOHSvr.PopulateListFromHold(" posand " + clsPOSDBConstants.POHeader_Fld_OrderID + " ='" + orderID + "'");
                    if (oPOHData.POHeader.Rows.Count == 0)
                    {
                       POS_Core.ErrorLogging.Logs.Logger("** Purchase Order Processing Error :Invalid order id in the row. ");
                        throw (new Exception("Invalid order id in the row."));
                    }

                    POS_Core.BusinessRules.Vendor oVend = new POS_Core.BusinessRules.Vendor();
                    VendorData oVData = oVend.Populate(oPOHData.POHeader[0].VendorCode);
                    PODetailData oPODData = oPO.PopulateDetailFromHold(oPOHData.POHeader[0].OrderID);
                   POS_Core.ErrorLogging.Logs.Logger("** Purchase Order Processing Order No: " + oPOHData.POHeader[0].OrderNo);
                    //call method to show invendory recived form . 
                    frmInventoryRecieved.isFromPurchaseOrder = true;
                    frmInventoryRecieved oIFrm = (frmInventoryRecieved)frmMain.ShowForm(POSMenuItems.InventoryRecieved);
                    oIFrm.InventoryWay = clsPOSDBConstants.PurchaseOrder;
                    //oIFrm.
                    
                    //initialize the form with values from particular PO

                    //otherwise do not update the inventory. If process810 flag is not true inventory can be processed even if status is 
                    //acnowledged(5).
                   
                    bool process810 = Convert.ToBoolean(oVData.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_Process810]);
                    if (process810)
                    {
                        //if (oPOHData.POHeader[0].Status == 17) //Commented By ravindra (Quicsolv) Status 17 Indicate only DirectDelivery 810 Also have status As  DeliveryReceived
                        if (oPOHData.POHeader[0].Status == (int)PurchseOrdStatus.DeliveryReceived || oPOHData.POHeader[0].Status == (int)PurchseOrdStatus.DirectDelivery)// Add by Ravindra (Quicsolv) Status 17 Indicate only DirectDelivery 810 Also have status As  DeliveryReceived
                        {
                           POS_Core.ErrorLogging.Logs.Logger("** Purchase Order Processing Ack File Type 810 Order No: " + oPOHData.POHeader[0].OrderNo);
                            oIFrm.FillFromPO(oPOHData.POHeader.Rows[0][clsPOSDBConstants.POHeader_Fld_OrderID].ToString());
                            
                        }
                    }
                    else // end of added by atul 26-oct-2010
                    {
                       POS_Core.ErrorLogging.Logs.Logger("** Purchase Order Processing Ack File Type 855 Order No: " + oPOHData.POHeader[0].OrderNo);
                        oIFrm.FillFromPO(oPOHData.POHeader.Rows[0][clsPOSDBConstants.POHeader_Fld_OrderID].ToString());
                    }
                    
                    oIFrm.btnDeleteItem.Enabled = false;
                    oIFrm.btnFillFromPO.Enabled = false;
                    oIFrm.btnAddItem.Enabled = false;
                    this.Close();
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Please select a row first.");
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
		}

		private void ultraButton1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void btnDeleteAck_Click(object sender, System.EventArgs e)
		{

		}

        
		private void grdSearch_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs eArg)
        {
           try
            {  
               //Added By shitaljit to AddingNewEventArgs tool Tip to Ack Type  
                //AC:  Acknowledge With Detail Change 
                //AD: Acknowledge With Detail No Change
                //AE:  Acknowledge With Exception Detail Only
                //RJ:  Rejected No Detail  
                string AckType = eArg.Row.Cells["AckType"].Text.ToString().Trim();
                string AckTypeTollTip = string.Empty;
                switch (AckType)
                {
                    case "AC": AckTypeTollTip = "Acknowledge With Detail Change";
                        break;
                    case "AD": AckTypeTollTip = "Acknowledge With Detail No Change";
                        break;
                    case "AE": AckTypeTollTip = "Acknowledge With Exception Detail Only";
                        break;
                    case "RJ": AckTypeTollTip = "Rejected No Detail";
                        break;
                }
                eArg.Row.Cells["AckType"].ToolTipText = AckTypeTollTip;
               //End of added by shitaljit
                //InitializeComponent row  with values assigned 
                string AckStatus = eArg.Row.Cells["AckStatus"].Text.ToString().Trim();
                if (AckStatus == "01")
                {
                    eArg.Row.Cells["VendorAckStatus"].Value = "Cancellation";
                }
                else if (AckStatus == "04")
                {
                    eArg.Row.Cells["VendorAckStatus"].Value = "Change";
                }
                else if (AckStatus == "06")
                {
                    eArg.Row.Cells["VendorAckStatus"].Value = "Confirmation";
                }

                string status = eArg.Row.Cells["Status"].Text.ToString().Trim();
               
                switch (status)
                {

                    case "0":
                        eArg.Row.Cells["OrderStatus"].Value = "Incomplete";
                        break;
                    case "1":
                        eArg.Row.Cells["OrderStatus"].Value = "Pending";
                        break;
                    case "2":
                        eArg.Row.Cells["OrderStatus"].Value = "Queued";
                        break;
                    case "3":
                        eArg.Row.Cells["OrderStatus"].Value = "Submitted";
                        break;
                    case "4":
                        eArg.Row.Cells["OrderStatus"].Value = "Canceled";
                        break;
                    case "5":
                        eArg.Row.Cells["OrderStatus"].Value = "Acknowledge";
                        break;
                    case "6":
                        eArg.Row.Cells["OrderStatus"].Value = "AcknowledgeManually";
                        break;
                    case "7":
                        eArg.Row.Cells["OrderStatus"].Value = "MaxAttempt";
                        break;
                    case "8":
                        eArg.Row.Cells["OrderStatus"].Value = "Processed";
                        break;
                    case "9":
                        eArg.Row.Cells["OrderStatus"].Value = "Expired";
                        break;
                    case "10":
                        eArg.Row.Cells["OrderStatus"].Value = "PartiallyAcknowledge";
                        break;
                    case "11":
                        eArg.Row.Cells["OrderStatus"].Value = "PartiallyAck-Reorder";
                        break;
                    case "12":
                        eArg.Row.Cells["OrderStatus"].Value = "Error";
                        break;
                    case "15":
                        eArg.Row.Cells["OrderStatus"].Value = "DirectAcknowledge";
                        break;
                     //added by atul 25-oct-2010
                    case "16":
                        eArg.Row.Cells["OrderStatus"].Value = "DeliveryReceived";
                        break;
                    //End of added by atul 25-oct-2010
                    case "17"://Added By Shitaljit for 810 file
                        eArg.Row.Cells["OrderStatus"].Value = "DirectDelivery";
                        break;
                    case "":
                        eArg.Row.Cells["OrderStatus"].Value = "NoStatus";
                        break;

                }
                //Added by shitaljit on 3 April 2012 to disable the row for 855 files if  process 810 is checked for vendor.
                #region Desable Row Logic
               

                POHeaderData oPOHData = new POHeaderData();
                POS_Core.BusinessRules.Vendor oVend = new POS_Core.BusinessRules.Vendor();

                string vendcode = eArg.Row.Cells["VendorCode"].Value.ToString();

                VendorData oVData = oVend.Populate(vendcode);

                bool process810 = Convert.ToBoolean(oVData.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_Process810]);
                if (process810)
                {
                    if ((eArg.Row.Cells["OrderStatus"].Value.ToString() == "Acknowledge") || (eArg.Row.Cells["OrderStatus"].Value.ToString() == "DirectAcknowledge") || (eArg.Row.Cells["OrderStatus"].Value.ToString() == "PartiallyAcknowledge") || (eArg.Row.Cells["OrderStatus"].Value.ToString() == "PartiallyAck-Reorder"))
                    {
                        eArg.Row.Activation = Activation.NoEdit;
                        eArg.Row.Activation = Activation.Disabled;
                        eArg.Row.Selected =false;
                    }
                }
                else if ((eArg.Row.Cells["OrderStatus"].Value.ToString() == "DeliveryReceived") || (eArg.Row.Cells["OrderStatus"].Value.ToString() == "DirectDelivery"))
                    {
                        eArg.Row.Activation = Activation.NoEdit;
                        eArg.Row.Activation = Activation.Disabled;
                        eArg.Row.Selected =false;
                    }
                #endregion

            }
            catch(Exception ex)
            {
                        
            }
        }

        private bool UpdatePOStatus()
        {
            if (Configuration.UpdateInProgress == true)
                return false;
            POHeaderSvr poHeaderSvr = new POHeaderSvr();
            POHeaderData poHeaderData = poHeaderSvr.PopulateList(" AND status IN (" + (int)PurchseOrdStatus.Queued + " , " + (int)PurchseOrdStatus.Submitted + " , " + (int)PurchseOrdStatus.Expired + ")");
            PODetailData poDetailData = new PODetailData();
            PODetailSvr poDetailSvr = new PODetailSvr();
            PODetailData poDetaData = new PODetailData();
            List<String> POOrdersHavingIssue = new List<string>();
            try
            {
                if (poHeaderData.POHeader.Rows.Count > 0)
                {
                    foreach (POHeaderRow rows in poHeaderData.POHeader.Rows)
                    {
                        poDetaData = poDetailSvr.Populate(rows.OrderID);
                        poDetailData.Merge(poDetaData);
                    }

                    Dictionary<long, string> dict = new Dictionary<long, string>();
                    int count = 0;

                    if (poHeaderData.POHeader.Rows.Count > 0)
                    {
                        foreach (POHeaderRow row in poHeaderData.POHeader.Rows)
                        {
                            //Here handeled orders having PrimePOOrderiId as 0 or duplicate primePOOrderId present.
                            if (poHeaderData.POHeader[count].PrimePOrderId > 0 && !dict.ContainsKey(poHeaderData.POHeader[count].PrimePOrderId))
                            {
                                dict.Add(poHeaderData.POHeader[count].PrimePOrderId, "");                                
                            }
                            else
                            {
                                POOrdersHavingIssue.Add(row.OrderNo.ToString());
                            }
                            count++;
                        }
                    }                      
                    PrimePOUtil.DefaultInstance.UpdatePOStatus(ref dict, ref poHeaderData, ref poDetailData);
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                }
                else
                {                   
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                }
                return true;
            }
            catch(Exception ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Could Not Update Status For PO ");
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                return true;
            }       
        }
 
        private int GetStatus(string status)
        {
            int poStatus = 0;
          
            switch (status)
            { 
                case "Queued" :
                    poStatus = 2;
                    break;
                case "Acknowledged":
                    poStatus = 4;
                    break;
                case "Confirmed":
                    poStatus = 1;
                    break;
                case "MaxAttemptsOver":
                    poStatus = 5;
                    break;
                case "":
                    poStatus = 0;
                    break;            
            }
           return poStatus;
        }

        private void grdSearch_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            for (int i = 0; i < this.grdSearch.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                if (this.grdSearch.DisplayLayout.Bands[0].Columns[i].DataType == typeof(System.Decimal))
                {
                    this.grdSearch.DisplayLayout.Bands[0].Columns[i].Format = "#######0.00";
                    this.grdSearch.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }
        }

        private void frmPOOnHold_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                frmMain.GetPOACKForm = "";
                poAckTimer.AutoReset = false;
                poAckTimer.Enabled = false;
                poAckTimer.Stop();               
            }
            catch(Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());      
            }
        }

        private void grdSearch_CellDataError(object sender, CellDataErrorEventArgs e)
        {
              
        }

        private void optProcessed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.grdSearch.DataSource = null;
                getData();               
            }
            catch (Exception ex)
            {
               clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }

        private void grdSearch_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {

            if (grdSearch.Selected.Rows.Count > 0)
            {
                try
                {
                    if (grdSearch.Selected.Rows[0].Activation == Activation.Disabled)
                    {
                        return;
                    }
                    //Addded For Max and Retry                     
                    //Enable or Disable Retry  or Cancel button depending upon status in [PO Status] coloumn  

                    //Modified by SRT(Abhishek) Date : 11/09/2009
                    //Added one condition to check if processed option is checked.

                    //added by atul 07-dec-2010
                    // If process810 flag is checked, and status is acknowledge, then process button should be disabled.
                    POHeaderData oPOHData = new POHeaderData();
                    POS_Core.BusinessRules.Vendor oVend = new POS_Core.BusinessRules.Vendor();

                    string vendcode = grdSearch.Selected.Rows[0].Cells["VendorCode"].Value.ToString();

                    VendorData oVData = oVend.Populate(vendcode);

                    bool process810 = Convert.ToBoolean(oVData.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_Process810]);
                    if (process810)
                    {
                        if ((grdSearch.Selected.Rows[0].Cells["OrderStatus"].Value.ToString() == "Acknowledge") || (grdSearch.Selected.Rows[0].Cells["OrderStatus"].Value.ToString() == "DirectAcknowledge") || (grdSearch.Selected.Rows[0].Cells["OrderStatus"].Value.ToString() == "PartiallyAck"))
                        {
                            btnProcess.Enabled = false;
                            return;
                        }
                    }
                    //End of added by atul 07-dec-2010


                    if ((grdSearch.Selected.Rows[0].Cells["VendorAckStatus"].Value.ToString() == "Cancellation")  ||(grdSearch.Selected.Rows[0].Cells["AckType"].Value.ToString() == "RJ") /*Added by atul 27-oct-2010*/ )
                    {
                        this.btnProcess.Enabled = false;
                    }
                    else
                    {
                        this.btnProcess.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                this.btnProcess.Enabled = false;
            }
        }

        private void frmPOOnHold_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
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

        public static frmPOOnHold GetCurrentInstance()
        {
            if (defaultInstance == null || defaultInstance.IsDisposed == true)
            {
                defaultInstance = new frmPOOnHold();
            }
            return defaultInstance;
        }

        public void UpdateUI()
        {
            Application.DoEvents();        
        }

        private void frmPOOnHold_Shown(object sender, EventArgs e)
        {
            try
            {
                UpdatePOStatus();
                getData();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
        
        private void LoadVendorList()
        {
            DataTable dataTable = new DataTable();
            DataColumn oCol1 = dataTable.Columns.Add(clsPOSDBConstants.Vendor_Fld_VendorId);
            DataColumn oCol2 = dataTable.Columns.Add(clsPOSDBConstants.Vendor_Fld_VendorName);
            POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
            int count = 0;

            try
            {
                cboVendorList.SelectedIndex = 0;
                VendorData vendorData = vendor.PopulateList(" Where IsActive=1");

                foreach (VendorRow vendorRow in vendorData.Vendor.Rows)
                {
                    DataRow row = dataTable.NewRow();
                    cboVendorList.Items.Add(vendorRow.Vendorname);
                    row[clsPOSDBConstants.Vendor_Fld_VendorName] = vendorRow.Vendorname;
                    row[clsPOSDBConstants.Vendor_Fld_VendorId] = vendorRow.VendorId;
                    dataTable.Rows.Add(row);
                    count++;
                }
                datasetVendorID.Tables.Add(dataTable);
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }


        private void cboVendorList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (datasetVendorID.Tables.Count > 0 && datasetVendorID.Tables[0].Rows.Count > 0)
                {
                    this.cboVendorList.Enabled = true;
                    DataRow[] row = datasetVendorID.Tables[0].Select(clsPOSDBConstants.Vendor_Fld_VendorName + "='" + this.cboVendorList.Value.ToString() + "'");
                    if (row.Length > 0)
                    {
                        vendorid = row[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

    }
}
