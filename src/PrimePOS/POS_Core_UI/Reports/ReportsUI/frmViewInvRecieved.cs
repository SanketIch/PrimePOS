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
using POS_Core.LabelHandler;
using POS_Core.LabelHandler.RxLabel;
using System.Timers;
using POS_Core.Resources;
using POS_Core_UI.Reports.ReportsUI;
namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmVendorSearch.
	/// </summary>
	public class frmViewInvRecieved : System.Windows.Forms.Form
	{
		private SearchSvr oSearchSvr= new SearchSvr();
		private DataSet oDataSet;

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
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendor;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraPanel pnlData;
        public Infragistics.Win.Misc.UltraLabel lblMessage;
        private IContainer components;

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private Infragistics.Win.Misc.UltraButton btnPrintShelfStickers;
        private long iBlinkCnt = 0;
        #endregion
        private static frmRptItemPriceLogLable ofrmRptItemPriceLogLable;

        public frmViewInvRecieved()
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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewInvRecieved));
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
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
            this.pnlData = new Infragistics.Win.Misc.UltraPanel();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.btnPrintShelfStickers = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl1.SuspendLayout();
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
            this.pnlData.ClientArea.SuspendLayout();
            this.pnlData.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.txtVendor);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Controls.Add(this.clTo);
            this.ultraTabPageControl1.Controls.Add(this.clFrom);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(857, 50);
            // 
            // txtVendor
            // 
            this.txtVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtVendor.ButtonsRight.Add(editorButton1);
            this.txtVendor.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendor.Location = new System.Drawing.Point(436, 17);
            this.txtVendor.MaxLength = 20;
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.Size = new System.Drawing.Size(123, 20);
            this.txtVendor.TabIndex = 11;
            this.txtVendor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendor_EditorButtonClick);
            // 
            // ultraLabel1
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Location = new System.Drawing.Point(380, 18);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(54, 18);
            this.ultraLabel1.TabIndex = 10;
            this.ultraLabel1.Text = "Vendor";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // clTo
            // 
            this.clTo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clTo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clTo.DateButtons.Add(dateButton1);
            this.clTo.Location = new System.Drawing.Point(230, 17);
            this.clTo.Name = "clTo";
            this.clTo.NonAutoSizeHeight = 22;
            this.clTo.Size = new System.Drawing.Size(123, 21);
            this.clTo.TabIndex = 2;
            this.clTo.Value = new System.DateTime(2015, 7, 17, 0, 0, 0, 0);
            this.clTo.ValueChanged += new System.EventHandler(this.clTo_ValueChanged);
            // 
            // clFrom
            // 
            this.clFrom.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.clFrom.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.clFrom.DateButtons.Add(dateButton2);
            this.clFrom.Location = new System.Drawing.Point(56, 17);
            this.clFrom.Name = "clFrom";
            this.clFrom.NonAutoSizeHeight = 22;
            this.clFrom.Size = new System.Drawing.Size(123, 21);
            this.clFrom.TabIndex = 1;
            this.clFrom.Value = new System.DateTime(2015, 7, 17, 0, 0, 0, 0);
            this.clFrom.ValueChanged += new System.EventHandler(this.clFrom_ValueChanged);
            // 
            // lbl2
            // 
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance3;
            this.lbl2.AutoSize = true;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(202, 18);
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
            this.lbl1.Location = new System.Drawing.Point(14, 18);
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
            this.btnSearch.Location = new System.Drawing.Point(721, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 3;
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
            this.clTo1.Value = new System.DateTime(2015, 7, 17, 0, 0, 0, 0);
            // 
            // clFrom1
            // 
            this.clFrom1.BackColor = System.Drawing.SystemColors.Window;
            this.clFrom1.Location = new System.Drawing.Point(1, 1);
            this.clFrom1.Name = "clFrom1";
            this.clFrom1.NonAutoSizeHeight = 21;
            this.clFrom1.Size = new System.Drawing.Size(121, 21);
            this.clFrom1.TabIndex = 0;
            this.clFrom1.Value = new System.DateTime(2015, 7, 17, 0, 0, 0, 0);
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
            this.grdSearch.DataSource = this.ultraDataSource1;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackColorDisabled = System.Drawing.Color.White;
            appearance7.BackColorDisabled2 = System.Drawing.Color.White;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance7;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand1.HeaderVisible = true;
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
            this.grdSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(0, 0);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(861, 375);
            this.grdSearch.TabIndex = 4;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
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
            this.tabMain.Location = new System.Drawing.Point(5, 10);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(861, 75);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
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
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(857, 50);
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
            this.sbMain.Location = new System.Drawing.Point(0, 375);
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
            this.sbMain.Size = new System.Drawing.Size(861, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnClose
            // 
            appearance40.BackColor = System.Drawing.Color.White;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance40.FontData.BoldAsString = "True";
            appearance40.ForeColor = System.Drawing.Color.Black;
            appearance40.Image = ((object)(resources.GetObject("appearance40.Image")));
            this.btnClose.Appearance = appearance40;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance41;
            this.btnClose.Location = new System.Drawing.Point(781, 502);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
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
            this.btnPrint.Location = new System.Drawing.Point(398, 500);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pnlData
            // 
            // 
            // pnlData.ClientArea
            // 
            this.pnlData.ClientArea.Controls.Add(this.grdSearch);
            this.pnlData.ClientArea.Controls.Add(this.sbMain);
            this.pnlData.Location = new System.Drawing.Point(5, 94);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(861, 400);
            this.pnlData.TabIndex = 9;
            // 
            // lblMessage
            // 
            appearance43.ForeColor = System.Drawing.Color.Red;
            appearance43.TextHAlignAsString = "Center";
            this.lblMessage.Appearance = appearance43;
            this.lblMessage.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.lblMessage.Location = new System.Drawing.Point(0, 536);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(874, 20);
            this.lblMessage.TabIndex = 33;
            this.lblMessage.Tag = "NOCOLOR";
            this.lblMessage.Text = "cost price is hidden due to the user does not have enough permissions";
            this.lblMessage.Visible = false;
            // 
            // btnPrintShelfStickers
            // 
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.BackColor2 = System.Drawing.SystemColors.Control;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance44.FontData.BoldAsString = "True";
            appearance44.ForeColor = System.Drawing.Color.Black;
            appearance44.Image = ((object)(resources.GetObject("appearance44.Image")));
            this.btnPrintShelfStickers.Appearance = appearance44;
            this.btnPrintShelfStickers.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance45.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnPrintShelfStickers.HotTrackAppearance = appearance45;
            this.btnPrintShelfStickers.Location = new System.Drawing.Point(593, 502);
            this.btnPrintShelfStickers.Name = "btnPrintShelfStickers";
            this.btnPrintShelfStickers.Size = new System.Drawing.Size(178, 26);
            this.btnPrintShelfStickers.TabIndex = 6;
            this.btnPrintShelfStickers.Text = "&Print Shelf Stickers";
            this.btnPrintShelfStickers.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnPrintShelfStickers.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrintShelfStickers.Click += new System.EventHandler(this.btnPrintShelfStickers_Click);
            // 
            // frmViewInvRecieved
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(874, 556);
            this.Controls.Add(this.btnPrintShelfStickers);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewInvRecieved";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Inventory Received";
            this.Activated += new System.EventHandler(this.frmViewInvRecieved_Activated);
            this.Load += new System.EventHandler(this.frmViewInvRecieved_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewInvRecieved_KeyDown);
            this.Resize += new System.EventHandler(this.frmViewInvRecieved_Resize);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
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
            this.pnlData.ClientArea.ResumeLayout(false);
            this.pnlData.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		private void btnSearch_Click(object sender, System.EventArgs e)
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
				Search();
			}
			catch (Exception exp) {clsUIHelper.ShowErrorMsg(exp.Message);}
		}
        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)clFrom.Value > (DateTime)clTo.Value)
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
		private void Search()
		{
			oDataSet=new DataSet();
			string sSQL = "Select " 
				+ " TH." +  clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " as [InvRecvID]"
				+ " , TH." +  clsPOSDBConstants.InvRecvHeader_Fld_RefNo + " as [Ref No]"
				+ " , Cus." +  clsPOSDBConstants.Vendor_Fld_VendorName + " as [Vendor]"
				+ " , cast(" +  clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate + " as varchar) as [Recv. Date]"
				+ " , TH." +  clsPOSDBConstants.InvRecvHeader_Fld_UserID + " as [User ID]"
                + " , TH." + clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo + " as [Purchase Order No]"
                + " , (SELECT SUM(Qty * Cost) FROM InvRecievedDetail WHERE InvRecievedID = TH.InvRecievedID) AS [Grand Total Cost]"
				+ " FROM " 
				+ clsPOSDBConstants.InvRecvHeader_tbl + " as TH "
				+ " left join " + clsPOSDBConstants.Vendor_tbl + " as Cus "
				+ " on TH." + clsPOSDBConstants.InvRecvHeader_Fld_VendorID + " = Cus." + clsPOSDBConstants.Vendor_Fld_VendorId
				+ " where convert(datetime," + clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate +",109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) ";

			if (this.txtVendor.Text.Trim()!="")
			{
				sSQL+= " and Cus." + clsPOSDBConstants.Vendor_Fld_VendorCode + "='" + this.txtVendor.Text + "' ";
			}
			sSQL+= " order by " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " desc " ;

			oDataSet.Tables.Add( oSearchSvr.Search(sSQL).Tables[0].Copy());
			oDataSet.Tables[0].TableName="Master";

			if (oDataSet.Tables[0].Rows.Count==0)
			{
				grdSearch.DataSource = oDataSet;
				return;
			}

			sSQL = "Select " 
					+ " IRD." + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " As [InvRecvID] "
					+ " , IRD." + clsPOSDBConstants.InvRecvDetail_Fld_ItemID
					+ " , " + clsPOSDBConstants.Item_Fld_Description + " as [Item Name] " 
					+ " , " + clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered + " as [Actual Order] "
					+ " , " + clsPOSDBConstants.InvRecvDetail_Fld_QTY
					+ " , " + clsPOSDBConstants.InvRecvDetail_Fld_SalePrice
					+ " , " + clsPOSDBConstants.InvRecvDetail_Fld_Cost + " AS [Unit Cost]"
                    + "," + clsPOSDBConstants.InvRecvDetail_Fld_QTY + "*" + clsPOSDBConstants.InvRecvDetail_Fld_Cost + " AS [Total Cost]"   //Sprint-21 - 2002 21-Jul-2015 JY Added
					+ " , " + clsPOSDBConstants.InvRecvDetail_Fld_Comments
					+ " FROM " 
					+ clsPOSDBConstants.InvRecvHeader_tbl +  " as IR "
					+ " left join " + clsPOSDBConstants.Vendor_tbl + " as Cus "
					+ " on IR." + clsPOSDBConstants.InvRecvHeader_Fld_VendorID + " = Cus." + clsPOSDBConstants.Vendor_Fld_VendorId
					+ " , " + clsPOSDBConstants.InvRecvDetail_tbl  + " as IRD "
					+ " , " + clsPOSDBConstants.Item_tbl + " as itm "
					+ " where IR." + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " = IRD." + clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID
					+ " and IRD." + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + " = itm." + clsPOSDBConstants.Item_Fld_ItemID
					+ " and convert(datetime, " + clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate + " ,109) between convert(datetime, cast('" + this.clFrom.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.clTo.Text  + " 23:59:59 ' as datetime) ,113) ";

					if (this.txtVendor.Text.Trim()!="")
					{
						sSQL+= " and Cus." + clsPOSDBConstants.Vendor_Fld_VendorCode + "='" + this.txtVendor.Text + "' ";
					}

			oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
			oDataSet.Tables[1].TableName="Detail";

			//DataRelation dr=new DataRelation("MasterDetail",oDataSet.Tables[0].Columns["TransID"],oDataSet.Tables[1].Columns["TransID"]);
			oDataSet.Relations.Add("MasterDetail",oDataSet.Tables[0].Columns["InvRecvID"],oDataSet.Tables[1].Columns["InvRecvID"]);
			
			grdSearch.DataSource = oDataSet;            		

			grdSearch.DisplayLayout.Bands[0].HeaderVisible=true;
			grdSearch.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints=12;
			grdSearch.DisplayLayout.Bands[0].Header.Appearance.TextHAlign=Infragistics.Win.HAlign.Center;

			grdSearch.DisplayLayout.Bands[0].Header.Caption= "Inventory Received";  //PRIMEPOS-2824 25-Mar-2020 JY modified

            grdSearch.DisplayLayout.Bands[0].Columns["InvRecvID"].Hidden=true;
			grdSearch.DisplayLayout.Bands[1].Columns["InvRecvID"].Hidden=true;

			grdSearch.DisplayLayout.Bands[1].Expandable=true;
			
			grdSearch.DisplayLayout.Bands[1].HeaderVisible=true;
			grdSearch.DisplayLayout.Bands[1].Header.Caption="Detail";
			grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints=12;
			grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign=Infragistics.Win.HAlign.Center;

			this.resizeColumns();
            grdSearch.DisplayLayout.Bands[0].Columns["Recv. Date"].Width = 100;
            grdSearch.DisplayLayout.Bands[1].Columns["Item Name"].Width = 200;
            grdSearch.DisplayLayout.Bands[1].Columns["Actual Order"].Width = 20;

            #region PRIMEPOS-2464 10-Mar-2020 JY Added
            try
            {
                if (nDisplayItemCost == 0)
                {
                    grdSearch.DisplayLayout.Bands[1].Columns["Unit Cost"].Hidden = true;
                    grdSearch.DisplayLayout.Bands[1].Columns["Total Cost"].Hidden = true;
                }
            }
            catch(Exception ex)
            {
            }
            #endregion

            grdSearch.Focus();
			grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
			grdSearch.Refresh();
			sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
		}
		
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
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

		private void frmViewInvRecieved_Load(object sender, System.EventArgs e)
		{
			try
			{
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

                this.clTo.Value = DateTime.Today;
				this.clFrom.Value=DateTime.Today.Date.Subtract(new System.TimeSpan(7,0,0,0));

				clsUIHelper.SetAppearance(this.grdSearch);
				clsUIHelper.SetReadonlyRow(this.grdSearch);
				Search();
				if (this.grdSearch.Rows.Count==0)
				{
					this.clFrom.Focus();
				}
				clsUIHelper.setColorSchecme(this);
            }
            catch (Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void frmViewInvRecieved_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter && this.ActiveControl.Name != "grdSearch")
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
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
			catch (Exception ) {}
		}

		private void frmViewInvRecieved_Activated(object sender, System.EventArgs e)
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
						if ( oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
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

				Int32 TransID= Convert.ToInt32(grdSearch.ActiveRow.Cells[0].Text.ToString());

				TransHeaderData oTransHData;
				TransHeaderSvr oTransHSvr=new TransHeaderSvr();
			
				TransDetailData oTransDData;
				TransDetailSvr oTransDSvr=new TransDetailSvr();

				POSTransPaymentData oTransPaymentData;
				POSTransPaymentSvr oTransPaymentSvr=new POSTransPaymentSvr();

                //added by atul 07-jan-2011
                TransDetailRXData oTransRxData;
                TransDetailRXSvr oTransRxSvr = new TransDetailRXSvr();
                //End of added by atul 07-jan-2011
			
				oTransHData=oTransHSvr.Populate(TransID);

				oTransDData=oTransDSvr.PopulateData(TransID);

				oTransPaymentData=oTransPaymentSvr.Populate(TransID);

                TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(TransID); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

                //added by atul 07-jan-2011
                if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                {
                    //if (oTransDData.TransDetail[0].ItemID.ToString().Trim().ToUpper() == "RX")    //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented as its checking only first item
                    oTransRxData = oTransRxSvr.PopulateData(TransID);   //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added
                    if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                    {
                        //oTransRxData = oTransRxSvr.PopulateData(TransID); //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                        oRxLabel.Print();
                    }
                    else
                    {
                        RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                        oRxLabel.Print();
                    }
                }
                else //End of added by atul 07-jan-2011
                {
                    RxLabel oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax);
                    oRxLabel.Print();
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				exp=null;
			}
		}

		private void txtVendor_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
			if (oSearch.IsCanceled) return;
			txtVendor.Text = oSearch.SelectedRowID();
		}

        private void frmViewInvRecieved_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }

        private void clFrom_ValueChanged(object sender, EventArgs e)
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

        private void clTo_ValueChanged(object sender, EventArgs e)
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

        #region PRIMEPOS-2397 17-Nov-2020 JY Added
        private void btnPrintShelfStickers_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count <= 0)
                return;
            try
            {
                if (grdSearch.ActiveRow == null)
                    if (grdSearch.ActiveRow.Cells.Count == 0)
                        return;
                if (ofrmRptItemPriceLogLable == null)
                {
                    ofrmRptItemPriceLogLable = new frmRptItemPriceLogLable();
                }
                else
                {
                    ofrmRptItemPriceLogLable.Dispose();
                    ofrmRptItemPriceLogLable = new frmRptItemPriceLogLable();
                }
                ofrmRptItemPriceLogLable.InvRecievedID = Configuration.convertNullToInt(grdSearch.ActiveRow.Cells["InvRecvID"].Text);
                ofrmRptItemPriceLogLable.Show();
            }
            catch(Exception Ex)
            {

            }
        }

        public static frmRptItemPriceLogLable rptItemPriceLogLable
        {
            get
            {
                if (ofrmRptItemPriceLogLable == null || ofrmRptItemPriceLogLable.IsDisposed)
                    ofrmRptItemPriceLogLable = new frmRptItemPriceLogLable();
                return ofrmRptItemPriceLogLable;
            }
        }
        #endregion
    }
}