using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using POS_Core.DataAccess;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core_UI.Reports.Reports;
using POS_Core.Resources;
using POS_Core_UI.Reports.ReportsUI;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmViewStoreCreditSummary : System.Windows.Forms.Form
    {
        public string SearchTable = "";
        public bool IsCanceled = true;
        public bool DisplayRecordAtStartup = false;
        private SearchSvr oSearchSvr = new SearchSvr();
        private DataSet oDataSet = new DataSet();
        public bool isReadonly = false;
        private int CurrentX;
        private int CurrentY;
        private bool GridVisibleFlag = false;

        public string FormCaption = "";
        public string LabelText1 = "";
        public string LabelText2 = "";
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabMainControl;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        public Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraLabel lblCustId;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCustomer;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        public Infragistics.Win.Misc.UltraLabel lblCustomerName;
        public Infragistics.Win.Misc.UltraButton btnEdit;
        private IContainer components;
        private CheckBox chkSelectAll;
        bool allowMultiRowSelect = false;
        public bool isCallForDeactivateCard = false;
        private Infragistics.Win.Misc.UltraLabel lblCreditDeltail;
        private Infragistics.Win.Misc.UltraLabel lblNote;
        long CardID = 0;
        internal bool AllowMultiRowSelect
        {
            get { return allowMultiRowSelect; }
            set { allowMultiRowSelect = value; }
        }

        public frmViewStoreCreditSummary()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.btnSearch.Location = new System.Drawing.Point(496, 14);
            this.lblCustomerName.Location = new System.Drawing.Point(239, 16);
            this.txtCustomer.Location = new System.Drawing.Point(130, 16);
            this.lblCustId.Location = new System.Drawing.Point(13, 18);

            //
            // TODO: Add any constructor code after InitializeComponent call
            //              
        }
        //public frmViewStoreCreditSummary(string sCusId,long cardID, bool AlloMultiSelectRow)
        //{
        //    //
        //    // Required for Windows Form Designer support
        //    //
        //    InitializeComponent();
        //    allowMultiRowSelect = AlloMultiSelectRow;
        //    CLPointsRewardTier oCLPointsRewardTier = new CLPointsRewardTier();
        //    Decimal transactionPoints = oCLPointsRewardTier.GetCurrentTotalPoints(cardID);

        //    lblCreditDeltail.Text = " Card#: " + cardID + "       Current Points: " + transactionPoints;
        //    lblCreditDeltail.Visible = true;
        //    this.Text = "Select new card for merging current card points";
        //    if (AlloMultiSelectRow == false)
        //    {
        //        tabMain.Tabs[0].Text = "Deactivating Card Details";
        //    }
        //    else
        //    {
        //        tabMain.Tabs[0].Text = "Selected Card For Merge Details";
        //    }
        //    CardID = cardID;
        //    btnPrint.Text = "&OK";
        //    FKEdit(sCusId, clsPOSDBConstants.Customer_tbl);
        //    //
        //    // TODO: Add any constructor code after InitializeComponent call
        //    //              
        //}
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewStoreCreditSummary));
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            this.tabMainControl = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.lblNote = new Infragistics.Win.Misc.UltraLabel();
            this.lblCreditDeltail = new Infragistics.Win.Misc.UltraLabel();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.txtCustomer = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblCustId = new Infragistics.Win.Misc.UltraLabel();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.tabMainControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMainControl
            // 
            this.tabMainControl.Controls.Add(this.lblNote);
            this.tabMainControl.Controls.Add(this.lblCreditDeltail);
            this.tabMainControl.Controls.Add(this.chkSelectAll);
            this.tabMainControl.Controls.Add(this.btnSearch);
            this.tabMainControl.Controls.Add(this.lblCustomerName);
            this.tabMainControl.Controls.Add(this.grdSearch);
            this.tabMainControl.Controls.Add(this.txtCustomer);
            this.tabMainControl.Controls.Add(this.lblCustId);
            this.tabMainControl.Location = new System.Drawing.Point(2, 23);
            this.tabMainControl.Name = "tabMainControl";
            this.tabMainControl.Size = new System.Drawing.Size(991, 490);
            // 
            // lblNote
            // 
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance1.ForeColor = System.Drawing.Color.Blue;
            this.lblNote.Appearance = appearance1;
            this.lblNote.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblNote.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(7, 457);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(972, 26);
            this.lblNote.TabIndex = 89;
            this.lblNote.Text = "Merging cards will make selected card(s) current points and coupons added to card" +
    "#  and selected card deactivated.";
            this.lblNote.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblNote.Visible = false;
            // 
            // lblCreditDeltail
            // 
            appearance2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance2.ForeColor = System.Drawing.Color.Blue;
            this.lblCreditDeltail.Appearance = appearance2;
            this.lblCreditDeltail.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCreditDeltail.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCreditDeltail.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCreditDeltail.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditDeltail.Location = new System.Drawing.Point(13, 5);
            this.lblCreditDeltail.Name = "lblCreditDeltail";
            this.lblCreditDeltail.Size = new System.Drawing.Size(645, 21);
            this.lblCreditDeltail.TabIndex = 88;
            this.lblCreditDeltail.Text = "Store Credit Details";
            this.lblCreditDeltail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblCreditDeltail.Visible = false;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(27, 62);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(15, 14);
            this.chkSelectAll.TabIndex = 86;
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.Visible = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnSearch
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance3;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance4;
            this.btnSearch.Location = new System.Drawing.Point(496, 28);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(162, 26);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblCustomerName
            // 
            appearance5.BorderColor = System.Drawing.Color.Green;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Appearance = appearance5;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCustomerName.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(239, 30);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(250, 23);
            this.lblCustomerName.TabIndex = 36;
            this.lblCustomerName.Text = "Customer Name.";
            this.lblCustomerName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackColorDisabled = System.Drawing.Color.White;
            appearance6.BackColorDisabled2 = System.Drawing.Color.White;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance6;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
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
            appearance12.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance12.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance12;
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
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.SystemColors.Control;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.FontData.BoldAsString = "True";
            appearance18.ForeColor = System.Drawing.Color.Black;
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
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance22;
            this.grdSearch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
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
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance25;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.SystemColors.Control;
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance29;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(7, 59);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(972, 385);
            this.grdSearch.TabIndex = 6;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdSearch_DoubleClickRow);
            this.grdSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // txtCustomer
            // 
            appearance30.BorderColor = System.Drawing.Color.Lime;
            appearance30.BorderColor3DBase = System.Drawing.Color.Lime;
            this.txtCustomer.Appearance = appearance30;
            this.txtCustomer.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance31.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance31.Image = ((object)(resources.GetObject("appearance31.Image")));
            appearance31.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance31.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance31.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance31;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtCustomer.ButtonsRight.Add(editorButton1);
            this.txtCustomer.Location = new System.Drawing.Point(130, 30);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(103, 23);
            this.txtCustomer.TabIndex = 25;
            this.txtCustomer.TabStop = false;
            this.txtCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomer.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtCustomer_EditorButtonClick);
            this.txtCustomer.Leave += new System.EventHandler(this.txtCustomer_Leave);
            // 
            // lblCustId
            // 
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.lblCustId.Appearance = appearance32;
            this.lblCustId.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustId.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustId.Location = new System.Drawing.Point(13, 32);
            this.lblCustId.Name = "lblCustId";
            this.lblCustId.Size = new System.Drawing.Size(111, 18);
            this.lblCustId.TabIndex = 24;
            this.lblCustId.Text = "Customer ID";
            this.lblCustId.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // clTo1
            // 
            this.clTo1.BackColor = System.Drawing.SystemColors.Window;
            this.clTo1.Location = new System.Drawing.Point(0, 0);
            this.clTo1.Name = "clTo1";
            this.clTo1.NonAutoSizeHeight = 21;
            this.clTo1.Size = new System.Drawing.Size(121, 21);
            this.clTo1.TabIndex = 0;
            this.clTo1.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // clFrom1
            // 
            this.clFrom1.BackColor = System.Drawing.SystemColors.Window;
            this.clFrom1.Location = new System.Drawing.Point(1, 1);
            this.clFrom1.Name = "clFrom1";
            this.clFrom1.NonAutoSizeHeight = 21;
            this.clFrom1.Size = new System.Drawing.Size(121, 21);
            this.clFrom1.TabIndex = 0;
            this.clFrom1.Value = new System.DateTime(2008, 7, 11, 0, 0, 0, 0);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // tabMain
            // 
            appearance33.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance33;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance34;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance35;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.tabMainControl);
            this.tabMain.Location = new System.Drawing.Point(9, 9);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(995, 515);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
            appearance36.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance36;
            appearance37.BackColor = System.Drawing.Color.Transparent;
            appearance37.BackColor2 = System.Drawing.Color.Transparent;
            appearance37.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.Transparent;
            appearance38.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance38;
            ultraTab1.TabPage = this.tabMainControl;
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
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(991, 490);
            // 
            // sbMain
            // 
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            appearance39.BorderColor = System.Drawing.Color.Black;
            appearance39.FontData.Name = "Verdana";
            appearance39.FontData.SizeInPoints = 10F;
            appearance39.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance39;
            this.sbMain.Location = new System.Drawing.Point(0, 568);
            this.sbMain.Name = "sbMain";
            appearance40.BorderColor = System.Drawing.Color.Black;
            appearance40.BorderColor3DBase = System.Drawing.Color.Black;
            appearance40.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance40;
            appearance41.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance41;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(1022, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.SystemColors.Control;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.FontData.BoldAsString = "True";
            appearance42.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance42;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance43;
            this.btnClose.Location = new System.Drawing.Point(883, 532);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(121, 26);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance44.FontData.BoldAsString = "True";
            appearance44.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance44;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(749, 532);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(121, 26);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance45.BackColor = System.Drawing.Color.White;
            appearance45.BackColor2 = System.Drawing.SystemColors.Control;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.FontData.BoldAsString = "True";
            appearance45.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance45;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance46.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance46.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance46;
            this.btnEdit.Location = new System.Drawing.Point(20, 532);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(121, 26);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "&Edit (F3)";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // frmViewStoreCreditSummary
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1022, 593);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmViewStoreCreditSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Store Credit Summary";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.tabMainControl.ResumeLayout(false);
            this.tabMainControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
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
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        private void Search()
        {
            oDataSet = new DataSet();
            //string sSQL = @"select sc.CustomerID as [Customer_ID],c.CustomerName +', '+c.FIRSTNAME  as [Customer Name]
            //                ,sc.CreditAmt as [Amount],sc.UpdatedOn as [Updated Date],sc.UpdatedBy as [User Name] 
            //                ,sc.TransactionID as [Trans_ID],sc.StoreCreditID as [SC_ID]
            //                from StoreCreditDetails sc left join Customer c on c.CustomerID=sc.CustomerID";

            string sSQL = @"select sc.StoreCreditID as [Credit_ID],sc.CustomerID as [Customer_ID],c.CustomerName +', '+c.FIRSTNAME  as [Customer Name],
                          sc.CreditAmt as [Amount],sc.LastUpdated as [Updated Date],sc.LastUpdatedBy as [User Name] from StoreCredit sc left join Customer c on c.CustomerID=sc.CustomerID ";

            if (this.txtCustomer.Text.Trim() != "" && Configuration.convertNullToInt(this.txtCustomer.Tag.ToString()) != 0)
            {
                sSQL += " where sc." + clsPOSDBConstants.TransHeader_Fld_CustomerID + " ='" + this.txtCustomer.Tag.ToString().Trim() + "'";
            }
            

            oDataSet = oSearchSvr.Search(sSQL);
            grdSearch.DataSource = oDataSet;
            clsUIHelper.SetReadonlyRow(grdSearch);
            if (allowMultiRowSelect == true)
            {
                this.chkSelectAll.Visible = true;
                if (this.grdSearch.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Columns.Add("CHECK");
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "";

                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                }

                this.grdSearch.DisplayLayout.Bands[0].Columns["Check"].Header.VisiblePosition = 0;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Width = 50;
            }
            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);

            grdSearch.Refresh();
            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        void grdSearch_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            EditCLCards();
        }

        public UltraGridRow SelectedCardRow()
        {
            if (grdSearch.ActiveRow != null)
            {
                return grdSearch.ActiveRow;
            }
            else
            {
                return null;
            }
        }
        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtCustomer.Text))
                {
                    this.lblCustomerName.Text = "";
                }

                clsUIHelper.SetAppearance(this.grdSearch);
                clsUIHelper.SetReadonlyRow(this.grdSearch);
                this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeCell = SelectType.None;
                if (allowMultiRowSelect == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Single;
                }
                else
                {
                    this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Extended;
                }
               

                this.grdSearch.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
                this.txtCustomer.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCustomer.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                grdSearch.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
                grdSearch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                Search();
                clsUIHelper.setColorSchecme(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter && this.ActiveControl.Name != "grdSearch")
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                if (e.KeyData == Keys.F4)
                {
                    btnSearch_Click(sender, e);
                }
                if (e.KeyData == Keys.F3)
                {
                    EditCLCards();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
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
                for (int i = 0; i < this.grdSearch.DisplayLayout.Bands[0].Columns.Count; i++)
                {
                    if (this.grdSearch.DisplayLayout.Bands[0].Columns[i].DataType == typeof(System.Decimal))
                    {
                        this.grdSearch.DisplayLayout.Bands[0].Columns[i].Format = "#######0.00";
                        this.grdSearch.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                    }
                }
            }
            catch (Exception) { }
        }

        private void frmSearchMain_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void grdSearch_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.CurrentX = e.X;
            this.CurrentY = e.Y;
        }

        private void TextBoxKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Down)
                {
                    if (this.grdSearch.Rows.Count > 0)
                    {
                        this.grdSearch.Focus();
                        this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                    }
                }
            }
            catch (Exception) { }
        }

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdSearch.DisplayLayout.Bands)
                {
                    foreach (UltraGridColumn oCol in oBand.Columns)
                    {
                        oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            try
            {
                //if (this.isCallForDeactivateCard == false)
                //{
                    rptStoreCreditInfoView rpt = new rptStoreCreditInfoView();
                    clsReports.Preview(false, oDataSet, rpt);
                //}
                //else if (IsRowsSelected() == true)
                //{
                //    //added the if -else by shitaljit on 3Dec2013 to warn user before deactivating card.
                //    if (Resources.Message.Display("Are you sure you want to merge the selected card(s) to card# " + this.CardID + "?", "Merge Card", MessageBoxButtons.YesNo,
                //        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                //    {
                //        IsCanceled = false;
                //        this.Close();
                //    }
                //    else
                //    {
                //        IsCanceled = true;
                //        //this.Close();
                //    }
                //}
                //else
                //{
                //    clsUIHelper.ShowErrorMsg("Please select a card.");
                //}
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtCustomer_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                SearchCustomer();
            }
            catch (Exception) { }
        }

        private void SearchCustomer()
        {
            try
            {
                //frmCustomerSearch oSearch = new frmCustomerSearch(txtCustomer.Text);
                frmSearchMain oSearch = new frmSearchMain(txtCustomer.Text, true, clsPOSDBConstants.Customer_tbl);    //18-Dec-2017 JY Added new reference
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        ClearCustomer();
                        return;
                    }

                    FKEdit(strCode, clsPOSDBConstants.Customer_tbl);
                    this.txtCustomer.Focus();
                }
                else
                {
                    ClearCustomer();
                }
            }
            catch (Exception exp)
            {
                ClearCustomer();
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void ClearCustomer()
        {
            this.txtCustomer.Text = String.Empty;
            this.lblCustomerName.Text = String.Empty;
            this.txtCustomer.Tag = String.Empty;
            this.lblCustomerName.Text = String.Empty;
        }

        private void FKEdit(string code, string senderName)
        {
            if (senderName == clsPOSDBConstants.Customer_tbl)
            {
                #region Customer
                try
                {
                    Customer oCustomer = new Customer();
                    CustomerData oCustomerData;
                    CustomerRow oCustomerRow = null;
                    if (allowMultiRowSelect == false)
                    {
                        oCustomerData = oCustomer.Populate(code);
                    }
                    else
                    {
                        oCustomerData = oCustomer.GetCustomerByID(Configuration.convertNullToInt(code));
                    }
                    oCustomerRow = oCustomerData.Customer[0];
                    if (oCustomerRow != null)
                    {
                        this.txtCustomer.Text = oCustomerRow.AccountNumber.ToString();
                        this.txtCustomer.Tag = oCustomerRow.CustomerId.ToString();
                        this.lblCustomerName.Text = oCustomerRow.CustomerFullName;
                        //Added By Dharmendra(SRT) which will be required when processing
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    SearchCustomer();
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    SearchCustomer();
                }
                #endregion
            }
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            try
            {
                string txtValue = this.txtCustomer.Text;
                if (txtValue.Trim() != "")
                {
                    FKEdit(txtValue, clsPOSDBConstants.Customer_tbl);
                }
                else
                {
                    this.txtCustomer.Tag = "";
                    this.lblCustomerName.Text = "";
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditCLCards();
        }

        private void EditCLCards()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                frmCLCards ofrmCLCards = new frmCLCards();
                ofrmCLCards.Edit(Configuration.convertNullToInt64(grdSearch.ActiveRow.Cells[0].Text));
                ofrmCLCards.ShowDialog(this);
                if (!ofrmCLCards.IsCanceled)
                    Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region Code to allow user to select multiple rows
        private void grdSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.allowMultiRowSelect == true)
            {

                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.CellUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.CellUIElement cellUIElement = (Infragistics.Win.UltraWinGrid.CellUIElement)oUI;
                        if (cellUIElement.Column.Key.ToUpper() == "CHECK")
                        {
                            CheckUncheckGridRow(cellUIElement.Cell);
                        }
                        break;
                    }
                    oUI = oUI.Parent;
                }
            }
        }

        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            if ((bool)oCell.Value == false)
            {
                oCell.Value = true;
            }
            else
            {
                oCell.Value = false;
            }
            oCell.Row.Update();
        }

        /// <summary>
        /// Author:Shitaljit 
        /// Date:3 Dec 2013
        /// Added to check whether user has selected any rows or not.
        /// </summary>
        /// <returns></returns>
        private bool IsRowsSelected()
        {
            bool RetVal = false;
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                if (Configuration.convertNullToBoolean(oRow.Cells["check"].Value) == true)
                {
                    RetVal = true;
                    break;
                }
            }
            return RetVal;
        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            grdSearch.BeginUpdate();
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                oRow.Cells["check"].Value = chkSelectAll.Checked;
                oRow.Update();
            }
            grdSearch.EndUpdate();
        }
        #endregion

    }
}
