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
using System.Collections.Generic;
using System.Timers;
using POS_Core.Resources;
using POS_Core_UI.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmVendorSearch.
	/// </summary>
	public class frmViewPurchaseOrderStatus : System.Windows.Forms.Form
	{
		public string SearchTable ="";
		public bool IsCanceled  = true;
		public bool DisplayRecordAtStartup = false;
		private SearchSvr oSearchSvr= new SearchSvr();
		private DataSet oDataSet = new DataSet();
        private System.Timers.Timer viewPurchaseOrdTimer = null; 

		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.Misc.UltraButton btnSearch;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lbl2;
		private Infragistics.Win.Misc.UltraLabel lbl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom;
		private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendor;
        private Infragistics.Win.Misc.UltraButton btnSendEPO;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnRetry;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboStatusList;
        private IContainer components;
        private Infragistics.Win.Misc.UltraButton btnFillPartial;
        private Infragistics.Win.Misc.UltraButton btnCpyOrder;
        private Infragistics.Win.Misc.UltraButton btnSetAckManually;
        private ToolTip toolTipVendor;
        private Infragistics.Win.Misc.UltraButton btnReSubmit;
        private Infragistics.Win.Misc.UltraButton btnDeleteOrder;
        private ImageList imageList1;

		public frmViewPurchaseOrderStatus()
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem15 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem16 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem17 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem18 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem19 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewPurchaseOrderStatus));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn30 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn31 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn32 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.cboStatusList = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtVendor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.clTo = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnSendEPO = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnRetry = new Infragistics.Win.Misc.UltraButton();
            this.toolTipVendor = new System.Windows.Forms.ToolTip(this.components);
            this.btnFillPartial = new Infragistics.Win.Misc.UltraButton();
            this.btnCpyOrder = new Infragistics.Win.Misc.UltraButton();
            this.btnSetAckManually = new Infragistics.Win.Misc.UltraButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnReSubmit = new Infragistics.Win.Misc.UltraButton();
            this.btnDeleteOrder = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatusList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.cboStatusList);
            this.ultraTabPageControl1.Controls.Add(this.txtVendor);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Controls.Add(this.clTo);
            this.ultraTabPageControl1.Controls.Add(this.clFrom);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(904, 51);
            // 
            // cboStatusList
            // 
            this.cboStatusList.AutoSize = false;
            this.cboStatusList.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "Incomplete";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "Pending";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "Queued";
            valueListItem4.DataValue = "7";
            valueListItem4.DisplayText = "MaxAttempt";
            valueListItem5.DataValue = "3";
            valueListItem5.DisplayText = "Submitted";
            valueListItem6.DataValue = "5";
            valueListItem6.DisplayText = "Acknowledge";
            valueListItem7.DataValue = "6";
            valueListItem7.DisplayText = "AcknowledgeManually";
            valueListItem8.DataValue = "4";
            valueListItem8.DisplayText = "Canceled";
            valueListItem9.DataValue = "9";
            valueListItem9.DisplayText = "Expired";
            valueListItem10.DataValue = "8";
            valueListItem10.DisplayText = "Processed";
            valueListItem11.DataValue = "10";
            valueListItem11.DisplayText = "PartiallyAck";
            valueListItem12.DataValue = "11";
            valueListItem12.DisplayText = "PartiallyAck-Reorder";
            valueListItem13.DataValue = "12";
            valueListItem13.DisplayText = "Error";
            valueListItem14.DataValue = "13";
            valueListItem14.DisplayText = "Overdue";
            valueListItem15.DataValue = "14";
            valueListItem15.DisplayText = "SubmittedManually";
            valueListItem16.DataValue = "15";
            valueListItem16.DisplayText = "DirectAcknowledge";
            valueListItem17.DataValue = "16";
            valueListItem17.DisplayText = "DeliveryReceived";
            valueListItem18.DataValue = "ValueListItem17";
            valueListItem18.DisplayText = "DirectDelivery";
            valueListItem19.DataValue = "ValueListItem18";
            valueListItem19.DisplayText = "All";
            this.cboStatusList.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5,
            valueListItem6,
            valueListItem7,
            valueListItem8,
            valueListItem9,
            valueListItem10,
            valueListItem11,
            valueListItem12,
            valueListItem13,
            valueListItem14,
            valueListItem15,
            valueListItem16,
            valueListItem17,
            valueListItem18,
            valueListItem19});
            this.cboStatusList.Location = new System.Drawing.Point(473, 18);
            this.cboStatusList.Name = "cboStatusList";
            this.cboStatusList.Size = new System.Drawing.Size(197, 25);
            this.cboStatusList.TabIndex = 4;
            this.cboStatusList.SelectionChangeCommitted += new System.EventHandler(this.cboStatusList_SelectionChangeCommitted);
            this.cboStatusList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboStatusList_KeyDown);
            // 
            // txtVendor
            // 
            this.txtVendor.AutoSize = false;
            this.txtVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtVendor.ButtonsRight.Add(editorButton1);
            this.txtVendor.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendor.Location = new System.Drawing.Point(347, 18);
            this.txtVendor.MaxLength = 20;
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.Size = new System.Drawing.Size(112, 24);
            this.txtVendor.TabIndex = 3;
            this.toolTipVendor.SetToolTip(this.txtVendor, " Press F4 To Search");
            this.txtVendor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendor_EditorButtonClick);
            this.txtVendor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtVendor_KeyUp);
            // 
            // ultraLabel1
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Location = new System.Drawing.Point(287, 21);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(54, 18);
            this.ultraLabel1.TabIndex = 8;
            this.ultraLabel1.Text = "Vendor";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // clTo
            // 
            this.clTo.AutoSize = false;
            this.clTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clTo.DateButtons.Add(dateButton1);
            this.clTo.Location = new System.Drawing.Point(181, 18);
            this.clTo.Name = "clTo";
            this.clTo.NonAutoSizeHeight = 22;
            this.clTo.Size = new System.Drawing.Size(100, 25);
            this.clTo.TabIndex = 2;
            this.clTo.Value = new System.DateTime(2009, 7, 13, 0, 0, 0, 0);
            // 
            // clFrom
            // 
            this.clFrom.AutoSize = false;
            this.clFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clFrom.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clFrom.DateButtons.Add(dateButton2);
            this.clFrom.Location = new System.Drawing.Point(47, 18);
            this.clFrom.Name = "clFrom";
            this.clFrom.NonAutoSizeHeight = 22;
            this.clFrom.Size = new System.Drawing.Size(100, 26);
            this.clFrom.TabIndex = 1;
            this.clFrom.Value = new System.DateTime(2009, 7, 13, 0, 0, 0, 0);
            // 
            // lbl2
            // 
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance3;
            this.lbl2.AutoSize = true;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(153, 22);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(22, 18);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "To ";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Appearance = appearance4;
            this.lbl1.AutoSize = true;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(1, 22);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(40, 18);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "From ";
            this.lbl1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnSearch.Appearance = appearance5;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance6;
            this.btnSearch.Location = new System.Drawing.Point(776, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 25);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // clTo1
            // 
            this.clTo1.BackColor = System.Drawing.SystemColors.Window;
            this.clTo1.Location = new System.Drawing.Point(0, 0);
            this.clTo1.Name = "clTo1";
            this.clTo1.NonAutoSizeHeight = 21;
            this.clTo1.Size = new System.Drawing.Size(121, 21);
            this.clTo1.TabIndex = 0;
            this.clTo1.Value = new System.DateTime(2009, 7, 13, 0, 0, 0, 0);
            // 
            // clFrom1
            // 
            this.clFrom1.BackColor = System.Drawing.SystemColors.Window;
            this.clFrom1.Location = new System.Drawing.Point(1, 1);
            this.clFrom1.Name = "clFrom1";
            this.clFrom1.NonAutoSizeHeight = 21;
            this.clFrom1.Size = new System.Drawing.Size(121, 21);
            this.clFrom1.TabIndex = 0;
            this.clFrom1.Value = new System.DateTime(2009, 7, 13, 0, 0, 0, 0);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSearch.DataSource = this.ultraDataSource1;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackColorDisabled = System.Drawing.Color.White;
            appearance7.BackColorDisabled2 = System.Drawing.Color.White;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance7;
            this.grdSearch.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn30.Header.VisiblePosition = 0;
            ultraGridColumn30.Width = 296;
            ultraGridColumn31.Header.VisiblePosition = 1;
            ultraGridColumn31.Width = 297;
            ultraGridColumn32.Header.VisiblePosition = 2;
            ultraGridColumn32.Width = 296;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn30,
            ultraGridColumn31,
            ultraGridColumn32});
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance10;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackColorDisabled = System.Drawing.Color.White;
            appearance12.BackColorDisabled2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Black;
            appearance12.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            appearance13.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            appearance13.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance14;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackColorDisabled = System.Drawing.Color.White;
            appearance17.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance18.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.SystemColors.Control;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance19.FontData.BoldAsString = "True";
            appearance19.ForeColor = System.Drawing.Color.Black;
            appearance19.TextHAlignAsString = "Left";
            appearance19.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.White;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance21.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.SystemColors.Control;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.Navy;
            appearance24.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance24;
            appearance25.BackColor = System.Drawing.Color.Navy;
            appearance25.BackColorDisabled = System.Drawing.Color.Navy;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance25.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance25;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance26;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.SystemColors.Control;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance30;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(9, 100);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(903, 345);
            this.grdSearch.TabIndex = 6;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdSearch_AfterSelectChange);
            this.grdSearch.Click += new System.EventHandler(this.grdSearch_Click);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            // 
            // tabMain
            // 
            appearance31.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance31;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance32;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance33.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance33;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.ultraTabPageControl1);
            this.tabMain.Location = new System.Drawing.Point(7, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(908, 76);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
            this.tabMain.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.TopLeft;
            appearance34.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.Transparent;
            appearance35.BackColor2 = System.Drawing.Color.Transparent;
            appearance35.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.Transparent;
            appearance36.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance36;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Criteria";
            this.tabMain.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.tabMain.TabStop = false;
            this.tabMain.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.tabMain.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(904, 51);
            // 
            // sbMain
            // 
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BorderColor = System.Drawing.Color.Black;
            appearance37.FontData.Name = "Verdana";
            appearance37.FontData.SizeInPoints = 10F;
            appearance37.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance37;
            this.sbMain.Location = new System.Drawing.Point(0, 518);
            this.sbMain.Name = "sbMain";
            appearance38.BorderColor = System.Drawing.Color.Black;
            appearance38.BorderColor3DBase = System.Drawing.Color.Black;
            appearance38.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance38;
            appearance39.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance39;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(924, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance40.FontData.BoldAsString = "True";
            appearance40.ForeColor = System.Drawing.Color.Black;
            appearance40.Image = ((object)(resources.GetObject("appearance40.Image")));
            this.btnClose.Appearance = appearance40;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance41;
            this.btnClose.Location = new System.Drawing.Point(760, 486);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 27);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance42.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance42.FontData.BoldAsString = "True";
            appearance42.ForeColor = System.Drawing.Color.White;
            appearance42.Image = ((object)(resources.GetObject("appearance42.Image")));
            this.btnPrint.Appearance = appearance42;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(386, 486);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(150, 27);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSendEPO
            // 
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance43.FontData.BoldAsString = "True";
            appearance43.ForeColor = System.Drawing.Color.White;
            this.btnSendEPO.Appearance = appearance43;
            this.btnSendEPO.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSendEPO.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendEPO.Location = new System.Drawing.Point(199, 486);
            this.btnSendEPO.Name = "btnSendEPO";
            this.btnSendEPO.Size = new System.Drawing.Size(150, 27);
            this.btnSendEPO.TabIndex = 13;
            this.btnSendEPO.Text = "&Send To Host (F11)";
            this.btnSendEPO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSendEPO.Click += new System.EventHandler(this.btnSendEPO_Click);
            // 
            // btnCancel
            // 
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance44.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance44.FontData.BoldAsString = "True";
            appearance44.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Appearance = appearance44;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(760, 453);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 27);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel  Order(F5)";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnRetry
            // 
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance45.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance45.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance45.FontData.BoldAsString = "True";
            appearance45.ForeColor = System.Drawing.Color.White;
            this.btnRetry.Appearance = appearance45;
            this.btnRetry.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnRetry.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRetry.Location = new System.Drawing.Point(573, 453);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(150, 27);
            this.btnRetry.TabIndex = 10;
            this.btnRetry.Text = "Retry Order(F3)";
            this.btnRetry.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnFillPartial
            // 
            appearance46.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance46.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance46.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance46.FontData.BoldAsString = "True";
            appearance46.ForeColor = System.Drawing.Color.White;
            this.btnFillPartial.Appearance = appearance46;
            this.btnFillPartial.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnFillPartial.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFillPartial.Location = new System.Drawing.Point(386, 453);
            this.btnFillPartial.Name = "btnFillPartial";
            this.btnFillPartial.Size = new System.Drawing.Size(150, 27);
            this.btnFillPartial.TabIndex = 9;
            this.btnFillPartial.Text = "Fill Partial(F8)";
            this.btnFillPartial.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnFillPartial.Click += new System.EventHandler(this.btnFillPartial_Click);
            // 
            // btnCpyOrder
            // 
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance47.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance47.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance47.FontData.BoldAsString = "True";
            appearance47.ForeColor = System.Drawing.Color.White;
            this.btnCpyOrder.Appearance = appearance47;
            this.btnCpyOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCpyOrder.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCpyOrder.Location = new System.Drawing.Point(199, 453);
            this.btnCpyOrder.Name = "btnCpyOrder";
            this.btnCpyOrder.Size = new System.Drawing.Size(150, 27);
            this.btnCpyOrder.TabIndex = 8;
            this.btnCpyOrder.Text = "Copy Order(F6) ";
            this.btnCpyOrder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCpyOrder.Click += new System.EventHandler(this.btnCpyOrder_Click);
            // 
            // btnSetAckManually
            // 
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance48.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance48.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance48.FontData.BoldAsString = "True";
            appearance48.ForeColor = System.Drawing.Color.White;
            this.btnSetAckManually.Appearance = appearance48;
            this.btnSetAckManually.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSetAckManually.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetAckManually.Location = new System.Drawing.Point(12, 453);
            this.btnSetAckManually.Name = "btnSetAckManually";
            this.btnSetAckManually.Size = new System.Drawing.Size(150, 27);
            this.btnSetAckManually.TabIndex = 7;
            this.btnSetAckManually.Text = "&A&ckManual(F7)";
            this.btnSetAckManually.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSetAckManually.Click += new System.EventHandler(this.btnSetAckManually_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "WB01238_.GIF");
            this.imageList1.Images.SetKeyName(1, "WB01241_.GIF");
            // 
            // btnReSubmit
            // 
            appearance49.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance49.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance49.FontData.BoldAsString = "True";
            appearance49.ForeColor = System.Drawing.Color.White;
            this.btnReSubmit.Appearance = appearance49;
            this.btnReSubmit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnReSubmit.Location = new System.Drawing.Point(12, 486);
            this.btnReSubmit.Name = "btnReSubmit";
            this.btnReSubmit.Size = new System.Drawing.Size(150, 27);
            this.btnReSubmit.TabIndex = 12;
            this.btnReSubmit.Text = "ReSubmit(F9)";
            this.btnReSubmit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnReSubmit.Click += new System.EventHandler(this.btnReSubmit_Click);
            // 
            // btnDeleteOrder
            // 
            appearance50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance50.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance50.FontData.BoldAsString = "True";
            appearance50.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOrder.Appearance = appearance50;
            this.btnDeleteOrder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnDeleteOrder.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeleteOrder.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteOrder.Location = new System.Drawing.Point(573, 486);
            this.btnDeleteOrder.Name = "btnDeleteOrder";
            this.btnDeleteOrder.Size = new System.Drawing.Size(150, 27);
            this.btnDeleteOrder.TabIndex = 16;
            this.btnDeleteOrder.Text = "&Delete Order";
            this.btnDeleteOrder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteOrder.Click += new System.EventHandler(this.btnDeleteOrder_Click);
            // 
            // frmViewPurchaseOrderStatus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(924, 543);
            this.Controls.Add(this.btnDeleteOrder);
            this.Controls.Add(this.btnReSubmit);
            this.Controls.Add(this.btnSetAckManually);
            this.Controls.Add(this.btnCpyOrder);
            this.Controls.Add(this.btnFillPartial);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSendEPO);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdSearch);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewPurchaseOrderStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Purchsase Order";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmViewPurchaseOrderStatus_FormClosing);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.Resize += new System.EventHandler(this.frmViewPurchaseOrderStatus_Resize);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatusList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				Search();
			}
			catch (Exception exp) {clsUIHelper.ShowErrorMsg(exp.Message);}
		}

        private void InitializeViewPurchaseOrdTimer()
        {
            int viewPurchaseOrdTimerTime = Configuration.CPrimeEDISetting.PurchaseOrderTimer*1000*60;  //PRIMEPOS-3167 07-Nov-2022 JY Modified

            viewPurchaseOrdTimer = new System.Timers.Timer();
            viewPurchaseOrdTimer.Interval = viewPurchaseOrdTimerTime;
            viewPurchaseOrdTimer.Enabled = true;
            viewPurchaseOrdTimer.Elapsed += new ElapsedEventHandler(viewPurchaseOrdTimer_Elapsed); 

        }

        void viewPurchaseOrdTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
               lock(this.viewPurchaseOrdTimer)
                {
                   Search();
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());  
            }
        }


		private void Search()
		{

            try
            {
                oDataSet = new DataSet();

                //Query change by (SRT)Abhishek  Date : 01/14/2009
                //Added status condition for purchase order.

                PurchseOrdStatus poStatus = PurchseOrdStatus.All;

                #region
                //string sSQL = "Select " 
                //    + " TH." +  clsPOSDBConstants.POHeader_Fld_OrderID + " as [OrderID]"
                //    + " , TH." +  clsPOSDBConstants.POHeader_Fld_OrderNo + " as [Order No]"
                //    + " , Cus." +  clsPOSDBConstants.Vendor_Fld_VendorName + " as [Vendor]"
                //    + " , cast(" +  clsPOSDBConstants.POHeader_Fld_OrderDate + " as varchar) as [Order Date]"
                //    + " , cast(" +  clsPOSDBConstants.POHeader_Fld_ExptDelvDate + " as varchar) as [Exp. Delv. Date]"
                //    + " , " + "case " + clsPOSDBConstants.POHeader_Fld_Status + " when 1 then 'Processed' when 0 then 'Pending' when 2 then 'Queued' when 3 then 'Cancel' when 4 then 'Acknowledge' when 5 then 'AcknowledgeManually' when 6 then 'Submitted' when 7 then 'MaxAttempt' end  as [PO Status]"
                //    + " , " + "case " + clsPOSDBConstants.POHeader_Fld_isFTPUsed + " when 0 then 'Not Used' when 1 then 'Ack Not Processed' when 2 then 'Ack Processed' end as [Host Status]"
                //    + " , " +  "case isInvRecieved when 1 then 'Yes' else 'No' end  as [Is Inv Recieved]"
                //    + " , " +  clsPOSDBConstants.POHeader_Fld_Status
                //    + " , " + clsPOSDBConstants.POHeader_Fld_isFTPUsed
                //    + " , IsNull(" +  clsPOSDBConstants.POHeader_Fld_isInvRecieved + ",0) as " +  clsPOSDBConstants.POHeader_Fld_isInvRecieved 
                //    + " FROM " 
                //    + clsPOSDBConstants.POHeader_tbl + " as TH "
                //    + ", " + clsPOSDBConstants.Vendor_tbl + " as Cus "
                //    + " where TH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Cus." + clsPOSDBConstants.Vendor_Fld_VendorId
                //    + " and  TH." + clsPOSDBConstants.POHeader_Fld_Status + " = " + (int)poStatus 
                //    + " and convert(datetime,OrderDate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) ";

                //if (this.txtVendor.Text.Trim()!="")
                //{
                //    sSQL+= " and Cus." + clsPOSDBConstants.Vendor_Fld_VendorCode + "='" + this.txtVendor.Text + "' ";
                //}

                //sSQL+= " order by " + clsPOSDBConstants.POHeader_Fld_OrderID + " desc " ;
                #endregion

                string order = string.Empty;
                order = frmMain.GetViewPOForm;

                if (order == "")
                    order = poStatus.ToString();

                DateTime dtFromDate = Convert.ToDateTime(this.clFrom.Text.Trim());
                DateTime dtToDate = Convert.ToDateTime(this.clTo.Text.Trim());
                if (dtToDate < dtFromDate)
                {
                    clsUIHelper.ShowErrorMsg("To Date can not be less than From Date");
                    return;
                }
                //grdSearch.b
                string sSQL = GetString(order);
                grdSearch.BeginUpdate();
                oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
                oDataSet.Tables[0].TableName = "Master";
                // frmMain.GetViewPOForm = "";

                SetCombValue(order);
                sbMain.Panels[0].Text = "Record(s) Count = " + oDataSet.Tables[0].Rows.Count;

                if (oDataSet.Tables[0].Rows.Count == 0)
                {
                    grdSearch.DataSource = oDataSet;
                    grdSearch.EndUpdate();
                    return;
                }

                DataSet poDetials = new DataSet();

                foreach (DataRow row in oDataSet.Tables[0].Rows)
                {

                    DataSet details = new DataSet();

                    sSQL = "Select "
                             + " POD." + clsPOSDBConstants.PODetail_Fld_OrderID + " As [OrderID] "
                             + " , POD." + clsPOSDBConstants.PODetail_Fld_ItemID
                             + " , itm." + clsPOSDBConstants.Item_Fld_Description + " as [Item Name] "
                             + " , " + clsPOSDBConstants.PODetail_Fld_QTY
                             + " , " + clsPOSDBConstants.PODetail_Fld_Cost
                             + " , " + clsPOSDBConstants.PODetail_Fld_Comments
                             + " , POD." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as [Ack Status] "
                             + " , POD." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as [Act Qty] "
                             + " FROM "
                             + clsPOSDBConstants.PODetail_tbl + " as POD "
                             + " , " + clsPOSDBConstants.POHeader_tbl + " as POH "
                             + " , " + clsPOSDBConstants.Item_tbl + " as itm "
                             + ", " + clsPOSDBConstants.Vendor_tbl + " as Cus "
                             + " where POH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Cus." + clsPOSDBConstants.Vendor_Fld_VendorId
                             + " and POH." + clsPOSDBConstants.POHeader_Fld_OrderID + " = POD." + clsPOSDBConstants.PODetail_Fld_OrderID
                             + " and POD." + clsPOSDBConstants.PODetail_Fld_ItemID + " = itm." + clsPOSDBConstants.PODetail_Fld_ItemID
                             + " and convert(datetime,orderdate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113) ";

                    if (this.txtVendor.Text.Trim() != "")
                    {
                        sSQL += " and Cus." + clsPOSDBConstants.Vendor_Fld_VendorCode + "='" + this.txtVendor.Text + "' ";
                    }

                    if (order != PurchseOrdStatus.All.ToString())
                    {
                        sSQL += " and POH." + clsPOSDBConstants.POHeader_Fld_OrderID + " = " + row[clsPOSDBConstants.POHeader_Fld_OrderID];
                    }
                    else
                    {
                        details = oSearchSvr.Search(sSQL);
                        poDetials.Merge(details);
                        break;
                    }
                    details = oSearchSvr.Search(sSQL);
                    poDetials.Merge(details);
                    disableButtons();
                }

                oDataSet.Tables.Add(poDetials.Tables[0].Copy());
                oDataSet.Tables[1].TableName = "Detail";

                //DataRelation dr=new DataRelation("MasterDetail",oDataSet.Tables[0].Columns["TransID"],oDataSet.Tables[1].Columns["TransID"]);
                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["OrderID"], oDataSet.Tables[1].Columns["OrderID"]);

                grdSearch.DataSource = oDataSet;
                grdSearch.DisplayLayout.Bands[0].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 12;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[0].Header.Caption = "Purchase Order";

                grdSearch.DisplayLayout.Bands[0].Columns["OrderID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns["OrderID"].Hidden = true;

                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_Status].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_isFTPUsed].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_isInvRecieved].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["Is Inv Received"].Hidden = true;  //PRIMEPOS-2824 25-Mar-2020 JY modified
                grdSearch.DisplayLayout.Bands[0].Columns["Host Status"].Hidden = true;

                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_Comments].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Expandable = true;
                grdSearch.DisplayLayout.Bands[1].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[1].Header.Caption = "Order Detail";
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 12;
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;


                this.resizeColumns();
                grdSearch.Focus();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                //Added By SRT(Gaurav) Date: 06/07/2009
                this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].CellAppearance.Image = imageList1.Images[1];
                this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].ButtonDisplayStyle = ButtonDisplayStyle.Always;

                this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].CellButtonAppearance.Image = imageList1.Images[1];
                this.grdSearch.DisplayLayout.Bands[0].Columns["Template"].Width = 15;

                foreach (UltraGridRow uRow in this.grdSearch.Rows)
                {
                    if (uRow.Cells["Template"].Value.ToString() == "True")
                    {
                        uRow.Cells["Template"].ButtonAppearance.Image = imageList1.Images[0];
                    }
                    else
                    {
                        uRow.Cells["Template"].ButtonAppearance.Image = imageList1.Images[1];
                    }
                }
            
                this.grdSearch.EndUpdate();
                this.grdSearch.Refresh();
                //End Of Added By sRT(Gaurav)
                //grdSearch.Refresh();            
            }
            catch (Exception ex)
            {
                grdSearch.EndUpdate();
            }            
		}

        private string GetString(string order)
        {
            PurchseOrdStatus poStatus = PurchseOrdStatus.All;
            string viewPurchaseOrd = string.Empty;
            viewPurchaseOrd = order;
            //START:Changed by SRT(Prashant) Date:17-3-09
            switch(viewPurchaseOrd)
            {
                
                case clsPOSDBConstants.Incomplete:
                    poStatus = PurchseOrdStatus.Incomplete;
                    break;
                case clsPOSDBConstants.Processed :   
                     poStatus = PurchseOrdStatus.Processed;
                     break;                
                case clsPOSDBConstants.Pending:
                     poStatus = PurchseOrdStatus.Pending;
                     break;
                 case clsPOSDBConstants.Queued :
                     poStatus = PurchseOrdStatus.Queued;
                     break;
                 case clsPOSDBConstants.Canceled:
                     poStatus = PurchseOrdStatus.Canceled;
                     break;
                 case clsPOSDBConstants.Acknowledge :
                     poStatus = PurchseOrdStatus.Acknowledge;
                     break;
                case clsPOSDBConstants.AcknowledgeManually :
                     poStatus = PurchseOrdStatus.AcknowledgeManually;
                     break;
                case clsPOSDBConstants.Submitted:
                     poStatus = PurchseOrdStatus.Submitted;
                     break;
                case clsPOSDBConstants.MaxAttempt:
                     poStatus = PurchseOrdStatus.MaxAttempt;
                     break;
                case clsPOSDBConstants.Expired:
                     poStatus = PurchseOrdStatus.Expired;
                     break;
               case clsPOSDBConstants.PartiallyAcknowledge:
                     poStatus = PurchseOrdStatus.PartiallyAck;
                     break;
                 case clsPOSDBConstants.PartiallyAckReorder:
                     poStatus = PurchseOrdStatus.PartiallyAckReorder;
                     break;
                 case clsPOSDBConstants.Error:
                     poStatus = PurchseOrdStatus.Error;
                     break;
                 case clsPOSDBConstants.All :
                    poStatus = PurchseOrdStatus.All;
                    break;
                case clsPOSDBConstants.Overdue:
                    poStatus = PurchseOrdStatus.Overdue;
                    break;
                case clsPOSDBConstants.SubmittedManually:
                    poStatus = PurchseOrdStatus.SubmittedManually;
                    break;
                    //Add by SRT(Sachin) Date : 18 Feb 2010
                case clsPOSDBConstants.DirectAcknowledge:
                    poStatus = PurchseOrdStatus.DirectAcknowledge;
                    break;
                    //End of Add by SRT(Sachin) Date : 18 Feb 2010
                case clsPOSDBConstants.DeliveryReceived:
                    poStatus = PurchseOrdStatus.DeliveryReceived;
                    break;
            }

            //Modified the Query By shitaljit to Fest Total Items in the Purchase ORDER.
            string sSQL = "Select "
             + " TH." + clsPOSDBConstants.POHeader_Fld_OrderID + " as [OrderID]"
             + " , TH." + clsPOSDBConstants.POHeader_Fld_OrderNo + " as [Order No]"
             + " , TH." + clsPOSDBConstants.POHeader_Fld_Flagged + " as [Template]"
             + " , Cus." + clsPOSDBConstants.Vendor_Fld_VendorName + " as [Vendor]"
             + " , TH." + clsPOSDBConstants.POHeader_Fld_Description + " as [Reference]"
             + " , " + "case " + clsPOSDBConstants.POHeader_Fld_Status + " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 10  then 'PartiallyAck' when 11  then 'PartiallyAck-Reorder' when 12 then 'Error' when 13 then 'Overdue' when 14 then 'SubmittedManually' when 15 then 'DirectAcknowledge' when 16 then 'DeliveryReceived' when 17 then 'DirectDelivery' end  as [PO Status]"  //Change By SRT (Sachin) Date : 19 Feb 2010 //changeby shitaljit add DirectDelivery for 810 file processing.
             + " , os.[Total Items], os.[Total Qty],os.[Total Price]"
             + " , cast(" + clsPOSDBConstants.POHeader_Fld_OrderDate + " as varchar) as [Order Date]"
             + " , cast(" + clsPOSDBConstants.POHeader_Fld_ExptDelvDate + " as varchar) as [Exp. Delv. Date]"
              + " , " + "case " + clsPOSDBConstants.POHeader_Fld_isFTPUsed + " when 0 then 'Not Used' when 1 then 'Ack Not Processed' when 2 then 'Ack Processed' end as [Host Status]"
             + " , " + "case isInvRecieved when 1 then 'Yes' else 'No' end  as [Is Inv Received]"
             + " , " + clsPOSDBConstants.POHeader_Fld_Status
             + " , " + clsPOSDBConstants.POHeader_Fld_isFTPUsed
             + " , IsNull(" + clsPOSDBConstants.POHeader_Fld_isInvRecieved + ",0) as " + clsPOSDBConstants.POHeader_Fld_isInvRecieved
             + " FROM "
             + clsPOSDBConstants.POHeader_tbl + " as TH "
             + ", " + clsPOSDBConstants.Vendor_tbl + " as Cus "

             +" , (select PO1.OrderID,COUNT(pod.ItemID) as [Total Items],SUM(pod.Qty)as [Total Qty],SUM(pod.Qty*pod.Cost) as [Total Price] from " +
            clsPOSDBConstants.POHeader_tbl + " as PO1, " + clsPOSDBConstants.PODetail_tbl + " as pod where po1.OrderID = pod.OrderID group by po1.OrderID) as os"

             + "  where os.OrderId=TH.OrderID AND TH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Cus." + clsPOSDBConstants.Vendor_Fld_VendorId 
             + " and convert(datetime,OrderDate,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as  datetime) ,113) and convert(datetime, cast('" + this.clTo.Text + " 23:59:59 ' as datetime) ,113)";
            //END:Changed by SRT(Prashant) Date:17-3-09
            if (this.txtVendor.Text.Trim() != "")
            {
                sSQL += " and Cus." + clsPOSDBConstants.Vendor_Fld_VendorCode + "='" + this.txtVendor.Text + "' ";
            }

            if(poStatus != PurchseOrdStatus.All)
            {
               if(frmMain.FromPanel == true && poStatus == PurchseOrdStatus.Queued) 
                {
                   sSQL += " and TH." + clsPOSDBConstants.POHeader_Fld_Status + " IN ("+ (int)PurchseOrdStatus.Queued +","+ (int)PurchseOrdStatus.Pending +")";
                }
                else
                    sSQL += " and TH." + clsPOSDBConstants.POHeader_Fld_Status + " = " + (int)poStatus;
            }
          
            sSQL += " order by " + clsPOSDBConstants.POHeader_Fld_OrderID + " desc ";
            return sSQL;              
        }

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

		private void grdSearch_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				Point point = System.Windows.Forms.Cursor.Position;
				point = this.grdSearch.PointToClient(point);
				Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point);
				if ( oUI == null )
					return;

				while ( oUI != null )
				{
					if ( oUI.GetType() == typeof( Infragistics.Win.UltraWinGrid.RowUIElement ) )
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
			catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message);}
		}

		private void frmSearch_Load(object sender,System.EventArgs e)
		{
			try
			{
				this.clFrom.Value=DateTime.Today.Date.Subtract(new System.TimeSpan(7,0,0,0));
				this.clTo.Value=DateTime.Today;
                disableButtons();
				clsUIHelper.SetAppearance(this.grdSearch);
				clsUIHelper.SetReadonlyRow(this.grdSearch);
				Search();
                InitializeViewPurchaseOrdTimer();
				if (this.grdSearch.Rows.Count==0)
				{
					this.clFrom.Focus();
				}
				clsUIHelper.setColorSchecme(this);
				if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID,UserPriviliges.Screens.PurchaseOrder.ID,UserPriviliges.Permissions.POAckProcess.ID)==false)
				{
					this.btnSendEPO.Visible=false;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

        private void disableButtons()
        {
            btnSetAckManually.Enabled = false;
            btnCpyOrder.Enabled = false;
            btnFillPartial.Enabled = false;
            btnRetry.Enabled = false;
            btnCancel.Enabled = false;
            btnSendEPO.Enabled = false;
            btnReSubmit.Enabled = false;
            btnDeleteOrder.Enabled=false;
            btnPrint.Enabled = grdSearch.Rows.Count > 0 ? true : false;
        }

		private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter && this.ActiveControl.Name != "grdSearch")
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
                if (e.KeyData == System.Windows.Forms.Keys.F7)
                {
                    if(btnSetAckManually.Enabled == true)
                       AcknowledgeManually();
                }
                if (e.KeyData == System.Windows.Forms.Keys.F6)
                {
                    if(btnCpyOrder.Enabled == true)
                        CopyOrder();
                }
                if (e.KeyData == System.Windows.Forms.Keys.F8)
                { 
                    if(btnFillPartial.Enabled == true)
                        FillOrder();
                }
                if (e.KeyData == System.Windows.Forms.Keys.F3)
                {
                    if(btnRetry.Enabled == true)
                        RetryOrder();
                }
                if (e.KeyData == System.Windows.Forms.Keys.F5)
                {
                    if(btnCancel.Enabled == true)
                        Cancel();
                }
                if (e.KeyData == System.Windows.Forms.Keys.F9)
                {
                    if(btnReSubmit.Enabled == true)
                        ReSumitOrder();   
                }
                if (e.KeyData == System.Windows.Forms.Keys.F11)
                {
                    if (btnSendEPO.Enabled == true)
                        SendEPO();
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				for (int i=0; i<this.grdSearch.DisplayLayout.Bands[0].Columns.Count;i++)
				{
					if ( this.grdSearch.DisplayLayout.Bands[0].Columns[i].DataType==typeof(System.Decimal))
					{
						this.grdSearch.DisplayLayout.Bands[0].Columns[i].Format="#######0.00";
						this.grdSearch.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right;
					}
				}
			}
			catch (Exception) {}
		}

		private void frmSearchMain_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
		}

		private void TextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==Keys.Down)
				{
					if (this.grdSearch.Rows.Count>0)
					{
						this.grdSearch.Focus();
						this.grdSearch.ActiveRow=this.grdSearch.Rows[0];
					}
				}
			}
			catch (Exception ){}
		}

		private void resizeColumns()
		{
            try
			{
			   foreach(UltraGridBand oBand in grdSearch.DisplayLayout.Bands)
				{
					foreach (UltraGridColumn oCol in oBand.Columns)
					{
						oCol.Width =oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows,true)+10;
						if(oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
						{
							oCol.CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right ;
						}
					}
				}
			}
			catch (Exception ){}
		}

		private void grdSearch_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
			{
				if (orow.Expanded==true)
					orow.CollapseAll();
			}
			e.Row.Activate();
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.grdSearch.ActiveRow==null)
					return;

                if (grdSearch.ActiveRow.Band.Index>0)
				{
					this.grdSearch.ActiveRow= this.grdSearch.ActiveRow.ParentRow;
				}

				Int32 OrderID= Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());
				clsUIHelper.PrintPurchaseOrder(OrderID.ToString(),true,true);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void txtVendor_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            #region Commented By Abhishek 
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
            //oSearch.ShowDialog();
            //if (oSearch.IsCanceled) return;
            //txtVendor.Text = oSearch.SelectedRowID();
            #endregion
            SearchVendor();
		}

		private void btnSendEPO_Click(object sender, System.EventArgs e)
		{
            SendEPO();
		}

        private void SendEPO()
        {
            try
            {
                if (grdSearch.Selected.Rows.Count > 0)
                {
                    #region Added By (SRT)Abhishek  Date : 01/16/2009

                    string status = Convert.ToString(grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString());

                    if (status == clsPOSDBConstants.Pending)
                    {
                        PurchaseOrder purchaseOrder = new PurchaseOrder();
                        POHeaderData poHeaderData = new POHeaderData();
                        PODetailData poDetailData = new PODetailData();

                        poHeaderData = purchaseOrder.PopulateHeader(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));
                        poDetailData = purchaseOrder.PopulateDetail(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));

                        int isSend = PrimePOUtil.DefaultInstance.SendPO(poHeaderData, poDetailData);

                        if (isSend == PrimePOUtil.SUCCESS)
                        {
                            Resources.Message.Display("Purchase Order sent successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Queued;          //2;
                            purchaseOrder.Persist(poHeaderData, poDetailData);
                            ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Purchase Order -" + poHeaderData.POHeader[0].OrderNo.ToString() + ", Status - " + PurchseOrdStatus.Queued.ToString());
                        }
                        else if (isSend == PrimePOUtil.PODISCOONECT)
                        {
                            clsUIHelper.ShowErrorMsg(" PrimePO is Disconnected ");
                        }
                        //Added by SRT(Abhishek) 21 Aug 2009
                        //Added one more condition to check if there is error while creating PO on 
                        //primepo side.
                        else if (isSend == PrimePOUtil.ERROR)
                        {
                            clsUIHelper.ShowErrorMsg(" PurchaseOrder Order Can Not Be Send ");
                        }
                        //End of Added by SRT(Abhishek) 21 Aug 2009
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("For Send PO Status Should Be Pending");
                    }
                    #endregion
                    Search();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(" PurchaseOrder Order Can Not Be Send");
            }   
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            RetryOrder();                     
        }

        private void RetryOrder()
        {
            try
            {
                POS_Core.ErrorLogging.Logs.Logger(" ** Start Retry Purchase Order");
                string status = Convert.ToString(grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString());

                if (status == clsPOSDBConstants.MaxAttempt)
                {
                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                    POHeaderData poHeaderData = new POHeaderData();
                    PODetailData poDetailData = new PODetailData();

                    poHeaderData = purchaseOrder.PopulateHeader(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));
                    Dictionary<long, string> dictStatus = new Dictionary<long, string>();
                    if (poHeaderData.POHeader.Rows.Count > 0)
                    {
                        dictStatus.Add(poHeaderData.POHeader[0].PrimePOrderId, PurchseOrdStatus.Queued.ToString());
                    }
                    if (PrimePOUtil.DefaultInstance.RetryPO(dictStatus))
                    {
                        poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Queued;
                        poDetailData = purchaseOrder.PopulateDetail(poHeaderData.POHeader[0].OrderID);
                        purchaseOrder.Persist(poHeaderData, poDetailData);
                        Search();

                        ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + " , Status -" + PurchseOrdStatus.Queued.ToString());
                        ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                        POS_Core.ErrorLogging.Logs.Logger(" **  Retry Order Purchase Order No-" + poHeaderData.POHeader[0].OrderNo);
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("For Retry Order Status Should Be " + clsPOSDBConstants.MaxAttempt);
                   POS_Core.ErrorLogging.Logs.Logger(" ** For Retry Order Status Should Be " + clsPOSDBConstants.MaxAttempt );
                }
               POS_Core.ErrorLogging.Logs.Logger(" ** END Retry Purchase Order");
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
               POS_Core.ErrorLogging.Logs.Logger(" **  Retry Order Error " + ex.Message);
            } 
        }
 
        //START:Added by SRT(Prashant) Date:17-3-2009
        private void FillOrder()
        {
            frmCreateNewPurchaseOrder editPO = null;
            String poStatus = String.Empty;
            try
            {
                editPO = new frmCreateNewPurchaseOrder();
                editPO.IsEditPO = true;
                string poNumber = grdSearch.ActiveRow.Cells["OrderID"].Text;
                poStatus = grdSearch.ActiveRow.Cells["PO Status"].Text;

                if (clsPOSDBConstants.PartiallyAcknowledge == poStatus)
                {
                    editPO.IsPartiallyAck = true;
                    editPO.Edit(poNumber);
                    editPO.ShowDialog(this);
                    if (!editPO.IsCanceled)
                        Search();
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("For Fill Order Status Should Be Partially Acknowledge");
                }
                
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                editPO.IsPartiallyAck = false;
            }
        }

        private void CopyOrder()
        {
            bool flagStatus = false;
            if (grdSearch.Rows.Count < 1)
                return;

            frmCreateNewPurchaseOrder editPO = null;
            String poStatus = String.Empty;          
            
            try
            {
                if (grdSearch.ActiveRow.Cells["Template"].Value != DBNull.Value)
                   flagStatus = Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["Template"].Value.ToString());
        
                editPO = new frmCreateNewPurchaseOrder();
                editPO.IsEditPO = true;
                string poNumber = grdSearch.ActiveRow.Cells["OrderID"].Text;
                poStatus = grdSearch.ActiveRow.Cells["PO Status"].Text;

                if (clsPOSDBConstants.Expired == poStatus || clsPOSDBConstants.Canceled == poStatus || flagStatus == true)
                {
                    editPO.IsCopyOrder = true;
                    editPO.Edit(poNumber);
                    editPO.ShowDialog(this);
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("For Copy Order PO Status should be Expired/Cancelled/Flagged");
                }
                if (!editPO.IsCanceled)
                    Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally
            {
                editPO.IsCopyOrder = false;
            }            
        }

        private void AcknowledgeManually()
        {
            POHeaderData oPOHeaderData = null;
            PODetailData oPODetailData = null;
            PurchaseOrder oPurchaseOrder = null; 
            try
            {
                if (grdSearch.Selected.Rows.Count == 0)
                    return;
                oPurchaseOrder = new PurchaseOrder();
                String poStatus=grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                //poStatus == clsPOSDBConstants.Incomplete is added By Shitaljit on 20 Feb
                if (poStatus == clsPOSDBConstants.Expired || poStatus == clsPOSDBConstants.Canceled || poStatus == clsPOSDBConstants.SubmittedManually || poStatus == clsPOSDBConstants.Incomplete)
                {
                    string POID = grdSearch.ActiveRow.Cells[0].Text;

                    oPOHeaderData = oPurchaseOrder.PopulateHeader(Convert.ToInt32(POID));
                    oPODetailData = oPurchaseOrder.PopulateDetail(Convert.ToInt32(POID));
                    oPOHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.AcknowledgeManually;
                    oPurchaseOrder.Persist(oPOHeaderData, oPODetailData);
                    Search();
                }
                else
                {
                    //Changed by shitaljit on 20 feb 2012 added "SubmittedManually or Incomplete" in the message.
                    clsUIHelper.ShowErrorMsg("For Manual Acknowledgement, PO Status should be Expired or Cancelled or SubmittedManually or Incomplete");
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        //END:Added by SRT(Prashant) Date:18-3-2009
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Cancel()
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            POHeaderData poHeaderData = new POHeaderData();
            PODetailData poDetailData = new PODetailData();

            try
            {
                poHeaderData = purchaseOrder.PopulateHeader(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));
                poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Canceled;
                poDetailData = purchaseOrder.PopulateDetail(poHeaderData.POHeader[0].OrderID);
                purchaseOrder.Persist(poHeaderData, poDetailData);
                Search();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg("Order Can Not Be Canceled");
            }        
        }

        private void grdSearch_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            SetButtnSeeting();
        }

        private void frmViewPurchaseOrderStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                frmMain.GetViewPOForm = "";
                viewPurchaseOrdTimer.AutoReset = false;
                viewPurchaseOrdTimer.Enabled = false;
                viewPurchaseOrdTimer.Stop();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);   
            }
        }

        private void  SetCombValue(string viewPOScreenVal)
        {

            switch (viewPOScreenVal)
            {
                case clsPOSDBConstants.Incomplete:
                    cboStatusList.Text = clsPOSDBConstants.Incomplete;
                    break; 
                case clsPOSDBConstants.Processed :
                     cboStatusList.Text = clsPOSDBConstants.Processed;
                     break;                
                case clsPOSDBConstants.Pending:
                     cboStatusList.Text = clsPOSDBConstants.Pending;
                     break;
                case clsPOSDBConstants.Queued :
                    cboStatusList.Text = clsPOSDBConstants.Queued;
                     break;
                case clsPOSDBConstants.Canceled:
                     cboStatusList.Text = clsPOSDBConstants.Canceled;
                     break;
                case clsPOSDBConstants.Acknowledge :
                     cboStatusList.Text = clsPOSDBConstants.Acknowledge;
                     break;
                case clsPOSDBConstants.AcknowledgeManually :
                     cboStatusList.Text = clsPOSDBConstants.AcknowledgeManually;
                     break;
                case clsPOSDBConstants.Submitted :
                     cboStatusList.Text = clsPOSDBConstants.Submitted;
                     break;
                case clsPOSDBConstants.MaxAttempt:
                     cboStatusList.Text = clsPOSDBConstants.MaxAttempt;
                     break;
                case clsPOSDBConstants.Error:
                     cboStatusList.Text = clsPOSDBConstants.Error;
                     break;
                case clsPOSDBConstants.All :
                     cboStatusList.Text = clsPOSDBConstants.All;
                     break;
                 case clsPOSDBConstants.Expired:
                     cboStatusList.Text = clsPOSDBConstants.Expired;
                     break;
                 case clsPOSDBConstants.Overdue:
                     cboStatusList.Text = clsPOSDBConstants.Overdue;
                     break;
                 case clsPOSDBConstants.SubmittedManually:
                     cboStatusList.Text = clsPOSDBConstants.SubmittedManually;
                     break;
                 //Added by SRT(Sachin) Date 19 Feb 2010
                 case clsPOSDBConstants.DirectAcknowledge:
                     cboStatusList.Text = clsPOSDBConstants.DirectAcknowledge;
                     break;
                 //End of Added by SRT(Sachin) Date 19 Feb 2010
             }   
        }

        private void cboStatusList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                frmMain.GetViewPOForm = (string)cboStatusList.SelectedItem.DisplayText;
                Search();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }            
        }

        private void txtVendor_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (this.txtVendor.ContainsFocus == true)
                        this.SearchVendor();                   
                }               
                e.Handled = true;
            }
            catch(Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchVendor()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
                oSearch.ShowDialog();
                if (oSearch.IsCanceled) return;
                txtVendor.Text = oSearch.SelectedRowID();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);             
            } 
        }

        private void btnFillPartial_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count < 1)
                return;  
            FillOrder();
        }

        private void btnCpyOrder_Click(object sender, EventArgs e)
        {
           CopyOrder();
        }

        private void btnSetAckManually_Click(object sender, EventArgs e)
        {
            AcknowledgeManually();
        }

        private void frmViewPurchaseOrderStatus_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }
        private void SetButtnSeeting()
        {
            if (grdSearch.Selected.Rows.Count > 0)
            {

                string statusInGrid = string.Empty;
                try
                {
                    disableButtons();
                    statusInGrid = grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                    switch (statusInGrid)
                    {
                        case "Incomplete":
                            btnSetAckManually.Enabled = true;
                            btnCancel.Enabled = true;
                            btnReSubmit.Enabled = false;
                            btnDeleteOrder.Enabled = true;
                            break;
                        case "Processed":
                            btnCancel.Enabled = false;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Expired":
                            this.btnCpyOrder.Enabled = true;
                            btnCancel.Enabled = true;
                            btnReSubmit.Enabled = true;
                            btnDeleteOrder.Enabled = true;
                            break;
                        case "Canceled":
                            this.btnCpyOrder.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Pending":
                            btnCancel.Enabled = true;
                            btnSendEPO.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Queued":
                            btnReSubmit.Enabled = false;
                            break;
                        case "Submitted":
                            btnSetAckManually.Enabled = true;
                            btnReSubmit.Enabled = true;
                            break;
                        case "Overdue":
                            btnSetAckManually.Enabled = true;
                            btnReSubmit.Enabled = true;
                            break;
                        case "Acknowledge":
                            btnReSubmit.Enabled = false;
                            break;
                        case "AcknowledgeManually":
                            btnReSubmit.Enabled = false;
                            break;
                        case "MaxAttempt":
                            this.btnRetry.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Error":
                            this.btnRetry.Enabled = true;
                            btnReSubmit.Enabled = true;
                            break;
                        case "PartiallyAck":
                            this.btnFillPartial.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "PartiallyAck-Reorder":
                            btnReSubmit.Enabled = false;
                            break;
                        case "SubmittedManually":
                            btnSetAckManually.Enabled = true;
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void grdSearch_Click(object sender, EventArgs e)
        {
            
            if (grdSearch.Selected.Rows.Count > 0)
            {

                string statusInGrid = string.Empty;
                try
                {
                    disableButtons();
                    statusInGrid = grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                    switch (statusInGrid)
                    {
                        case "Incomplete":
                            btnSetAckManually.Enabled = true;
                            btnCancel.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Processed":
                            btnCancel.Enabled = false;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Expired":
                            this.btnCpyOrder.Enabled = true;
                            btnCancel.Enabled = true;
                            btnReSubmit.Enabled = true;
                            break;
                        case "Canceled":
                            this.btnCpyOrder.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Pending":
                            btnCancel.Enabled = true;
                            btnSendEPO.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Queued":
                            btnReSubmit.Enabled = false;
                            break;
                        case "Submitted":
                            btnSetAckManually.Enabled = true;
                            btnReSubmit.Enabled = true;
                            break;
                        case "Overdue":
                            btnSetAckManually.Enabled = true;
                            btnReSubmit.Enabled = true;
                            break;
                        case "Acknowledge":
                            btnReSubmit.Enabled = false;
                            break;
                        case "AcknowledgeManually":
                            btnReSubmit.Enabled = false;
                            break;
                        case "MaxAttempt":
                            this.btnRetry.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "Error":
                            this.btnRetry.Enabled = true;
                            btnReSubmit.Enabled = true;
                            break;
                        case "PartiallyAck":
                            this.btnFillPartial.Enabled = true;
                            btnReSubmit.Enabled = false;
                            break;
                        case "PartiallyAck-Reorder":
                            btnReSubmit.Enabled = false;
                            break;
                        case "SubmittedManually":
                            btnSetAckManually.Enabled = true;
                            break;
                        default:
                            break;
                    }
                    SetButtnSeeting();
                }
                catch (Exception ex)
                {

                }
            }
        }      

        private void ReSumitOrder()
        {
            PurchaseOrder oPurchaseOrder = null;
            try
            {
                if (grdSearch.Selected.Rows.Count == 0)
                    return;
                oPurchaseOrder = new PurchaseOrder();
                String poStatus = grdSearch.Selected.Rows[0].Cells["PO Status"].Value.ToString();
                if (poStatus == clsPOSDBConstants.Expired || poStatus == clsPOSDBConstants.Error || poStatus == clsPOSDBConstants.Submitted || poStatus == clsPOSDBConstants.Overdue)
                {
                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                    POHeaderData poHeaderData = new POHeaderData();
                    PODetailData poDetailData = new PODetailData();

                    poHeaderData = purchaseOrder.PopulateHeader(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));
                    Dictionary<long, string> dictStatus = new Dictionary<long, string>();
                    if (poHeaderData.POHeader.Rows.Count > 0)
                    {
                        dictStatus.Add(poHeaderData.POHeader[0].PrimePOrderId, PurchseOrdStatus.Queued.ToString());
                    }
                    if (PrimePOUtil.DefaultInstance.ReSubmitPO(dictStatus))
                    {
                        poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Queued;
                        poDetailData = purchaseOrder.PopulateDetail(poHeaderData.POHeader[0].OrderID);
                        purchaseOrder.Persist(poHeaderData, poDetailData);
                        Search();

                        ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + " , Status -" + PurchseOrdStatus.Queued.ToString());
                        ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("For Re-Submit Order Status Should Be Expired/Error/Submitted/Overdue");
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnReSubmit_Click(object sender, EventArgs e)
        {
            ReSumitOrder();       
        }

        private void cboStatusList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData != Keys.Enter)
                {
                    cboStatusList.DropDown();
                }
            }

        //private void cboStatusList_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.KeyData != Keys.Enter)
        //        {
        //            cboStatusList.DropDown();
        //        }
        //    }

            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
           POS_Core.ErrorLogging.Logs.Logger(" ** Start Delete Purchase Order");
            string message = " Are you sure you wants to Delete  Order  ";
            if (Resources.Message.Display(message, Configuration.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder();
                POHeaderData poHeaderData = new POHeaderData();
                PODetailData poDetailData = new PODetailData();

                try
                {
                    poHeaderData = purchaseOrder.PopulateHeader(Convert.ToInt32(grdSearch.Selected.Rows[0].Cells[clsPOSDBConstants.POHeader_Fld_OrderID].Value));
                    poDetailData = purchaseOrder.PopulateDetail(poHeaderData.POHeader[0].OrderID);
                    poHeaderData.POHeader[0].Delete();
                   POS_Core.ErrorLogging.Logs.Logger(" **  Delete Purchase Order OrderID:"+poHeaderData.POHeader[0].OrderID);
                    poDetailData.PODetail[0].Delete();
                    purchaseOrder.Persist(poHeaderData, poDetailData);
                    Search();
                }
                catch (Exception ex)
                {
                   POS_Core.ErrorLogging.Logs.Logger(" ** Error Delete Purchase Order" + ex.Message);
                    clsUIHelper.ShowErrorMsg("Order Can Not Be Delete");
                }POS_Core.ErrorLogging.Logs.Logger(" ** End Delete Purchase Order");
            }
        }       
	}
}
