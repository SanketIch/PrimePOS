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
using Infragistics.Win;
using POS_Core_UI.Reports.Reports;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.ReportsUI;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmIIASItemList : System.Windows.Forms.Form
    {
        public bool IsCanceled = true;
        private int CurrentX;
        private int CurrentY;

        #region builtin var
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        private Infragistics.Win.Misc.UltraLabel lbl2;
        private Infragistics.Win.Misc.UltraLabel lbl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private IContainer components;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboSubCategory;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboCategory;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUPCCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private CheckBox chkItemFilesItemsOnly;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;

        public DataTable SelectedData = null;
        #endregion

        public frmIIASItemList()
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
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
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.chkItemFilesItemsOnly = new System.Windows.Forms.CheckBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUPCCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboSubCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUPCCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSubCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.chkItemFilesItemsOnly);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel3);
            this.ultraTabPageControl1.Controls.Add(this.txtUPCCode);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel2);
            this.ultraTabPageControl1.Controls.Add(this.cboSubCategory);
            this.ultraTabPageControl1.Controls.Add(this.cboCategory);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Controls.Add(this.txtDescription);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(966, 115);
            // 
            // chkItemFilesItemsOnly
            // 
            this.chkItemFilesItemsOnly.AutoSize = true;
            this.chkItemFilesItemsOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkItemFilesItemsOnly.Location = new System.Drawing.Point(208, 84);
            this.chkItemFilesItemsOnly.Name = "chkItemFilesItemsOnly";
            this.chkItemFilesItemsOnly.Size = new System.Drawing.Size(12, 11);
            this.chkItemFilesItemsOnly.TabIndex = 5;
            this.chkItemFilesItemsOnly.UseVisualStyleBackColor = true;
            // 
            // ultraLabel3
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel3.Appearance = appearance1;
            this.ultraLabel3.Location = new System.Drawing.Point(9, 80);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(204, 18);
            this.ultraLabel3.TabIndex = 26;
            this.ultraLabel3.Text = "Only items found in Item file";
            // 
            // txtUPCCode
            // 
            this.txtUPCCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUPCCode.Location = new System.Drawing.Point(88, 13);
            this.txtUPCCode.MaxLength = 20;
            this.txtUPCCode.Name = "txtUPCCode";
            this.txtUPCCode.Size = new System.Drawing.Size(219, 23);
            this.txtUPCCode.TabIndex = 0;
            this.txtUPCCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel2
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance2;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Location = new System.Drawing.Point(9, 18);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(84, 18);
            this.ultraLabel2.TabIndex = 25;
            this.ultraLabel2.Text = "UPC Code";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboSubCategory
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboSubCategory.Appearance = appearance3;
            this.cboSubCategory.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboSubCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboSubCategory.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboSubCategory.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboSubCategory.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSubCategory.Location = new System.Drawing.Point(425, 48);
            this.cboSubCategory.Name = "cboSubCategory";
            this.cboSubCategory.Size = new System.Drawing.Size(219, 23);
            this.cboSubCategory.TabIndex = 4;
            // 
            // cboCategory
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboCategory.Appearance = appearance4;
            this.cboCategory.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append;
            this.cboCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.cboCategory.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboCategory.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboCategory.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCategory.Location = new System.Drawing.Point(88, 48);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(219, 23);
            this.cboCategory.TabIndex = 3;
            // 
            // ultraLabel1
            // 
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance5;
            this.ultraLabel1.Location = new System.Drawing.Point(9, 50);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(73, 18);
            this.ultraLabel1.TabIndex = 21;
            this.ultraLabel1.Text = "Category";
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(425, 13);
            this.txtDescription.MaxLength = 20;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(219, 23);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl2
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Appearance = appearance6;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(323, 50);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(117, 18);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "Sub-Category";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Appearance = appearance7;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(323, 18);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(84, 18);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "Description";
            this.lbl1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.SystemColors.Control;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance8;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance9;
            this.btnSearch.Location = new System.Drawing.Point(834, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(121, 30);
            this.btnSearch.TabIndex = 6;
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
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BackColorDisabled = System.Drawing.Color.White;
            appearance10.BackColorDisabled2 = System.Drawing.Color.White;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance10;
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
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance13;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackColorDisabled = System.Drawing.Color.White;
            appearance15.BackColorDisabled2 = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.Black;
            appearance15.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            appearance16.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance16.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance16.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BackColorDisabled = System.Drawing.Color.White;
            appearance20.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance21.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.FontData.BoldAsString = "True";
            appearance22.ForeColor = System.Drawing.Color.Black;
            appearance22.TextHAlignAsString = "Left";
            appearance22.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance22;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance24.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance26;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance27.BackColor = System.Drawing.Color.Navy;
            appearance27.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.Navy;
            appearance28.BackColorDisabled = System.Drawing.Color.Navy;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance28.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            appearance28.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance28;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance29;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.SystemColors.Control;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance31.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance33;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(9, 155);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(970, 364);
            this.grdSearch.TabIndex = 6;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdSearch_KeyUp);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // tabMain
            // 
            appearance34.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance34;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance35;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance36;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.ultraTabPageControl1);
            this.tabMain.Location = new System.Drawing.Point(9, 9);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(970, 140);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
            appearance37.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.Transparent;
            appearance38.BackColor2 = System.Drawing.Color.Transparent;
            appearance38.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.Transparent;
            appearance39.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance39;
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
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(966, 115);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.ultraButton1);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(11, 525);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(968, 57);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance40.FontData.BoldAsString = "True";
            appearance40.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Appearance = appearance40;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(685, 20);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 10;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance41.FontData.BoldAsString = "True";
            appearance41.ForeColor = System.Drawing.Color.White;
            this.ultraButton1.Appearance = appearance41;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton1.Location = new System.Drawing.Point(870, 20);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(85, 26);
            this.ultraButton1.TabIndex = 11;
            this.ultraButton1.Text = "&Close";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButton1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance42.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance42.FontData.BoldAsString = "True";
            appearance42.ForeColor = System.Drawing.Color.White;
            this.btnView.Appearance = appearance42;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(777, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 9;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // sbMain
            // 
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.SystemColors.Control;
            appearance43.BorderColor = System.Drawing.Color.Black;
            appearance43.FontData.Name = "Verdana";
            appearance43.FontData.SizeInPoints = 10F;
            appearance43.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance43;
            this.sbMain.Location = new System.Drawing.Point(0, 590);
            this.sbMain.Name = "sbMain";
            appearance44.BorderColor = System.Drawing.Color.Black;
            appearance44.BorderColor3DBase = System.Drawing.Color.Black;
            appearance44.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance44;
            appearance45.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance45;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(994, 21);
            this.sbMain.TabIndex = 19;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // frmIIASItemList
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(994, 611);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.grdSearch);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIIASItemList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IIAS Item List";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmSearchMain_KeyUp);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUPCCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSubCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                FillGrid();
                this.resizeColumns();
                grdSearch.Refresh();
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        private void FillGrid()
        {

            grdSearch.DataSource = Search();

            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
            grdSearch.Refresh();
            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count.ToString();
        }

        public DataSet Search()
        {
            DataSet oData = new DataSet();

            Search oSearch=new Search();
            string sSQL = "Select DISTINCT iias.UPCCode, iias.Description, iias.CategoryDescriptor, iias.SubCategoryDescriptor, iias.ChangeIndicator, iias.ChangeDate, "
                + " IsNull(i.SellingPrice,0) as SellingPrice, Case When I.ItemID Is Null then 0 Else 1 End as ItemFound "
                + " From IIAS_items iias Left Outer Join Item I On iias.UPCCode = I.ItemID where iias.IsActive=1 and iias.ChangeIndicator <> 'D' ";

            if (chkItemFilesItemsOnly.Checked == true)
            {
                sSQL += " and I.ItemID Is Not Null ";
            }

            if (txtUPCCode.Text.Trim().Length > 0)
            {
                sSQL += " And IIAS.UPCCode Like '" + txtUPCCode.Text.Replace("'", "''") + "%'";
            }

            if(txtDescription.Text.Trim().Length>0)
            {
                sSQL+=" And IIAS.Description Like '" + txtDescription.Text.Replace("'","''") + "%'";
            }

            if(cboCategory.Text.Trim().Length>0)
            {
                sSQL+=" And IIAS.CategoryDescriptor Like '" + cboCategory.Text.Replace("'","''") + "%'";
            }
            
            if(cboSubCategory.Text.Trim().Length>0)
            {
                sSQL+=" And IIAS.SubCategoryDescriptor Like '" + cboSubCategory.Text.Replace("'","''") + "%'";
            }

            sSQL += " Order by IIAS.UPCCode ";

            oData= oSearch.SearchData(sSQL);

            return oData;
        }
        
        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                Item oItem = new Item();

                clsUIHelper.SetAppearance(this.grdSearch);
                clsUIHelper.SetReadonlyRow(this.grdSearch);

                this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.cboCategory.DataSource = oItem.GetIIAS_Category_Descriptor();
                this.cboSubCategory.DataSource = oItem.GetIIAS_SubCategory_Descriptor();

                FillGrid();
                if (this.grdSearch.Rows.Count == 0)
                {
                    this.txtUPCCode.Focus();
                }
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

        private void grdSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
        }

        private void frmSearchMain_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.F3)
            {
            }
            else if (e.KeyData == System.Windows.Forms.Keys.F4)
            {
                btnSearch_Click(btnSearch, new EventArgs());
            }
        }

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                this.grdSearch.DisplayLayout.Bands[0].HeaderVisible = false;
                this.grdSearch.DisplayLayout.Bands[0].Columns["ChangeIndicator"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["ChangeDate"].Hidden = true;
                this.grdSearch.DisplayLayout.Bands[0].Columns["ItemFound"].Hidden = true;

                this.grdSearch.DisplayLayout.Bands[0].Columns["UPCCode"].Header.Caption = "    UPC";

                this.grdSearch.DisplayLayout.Bands[0].Columns["Description"].Header.Caption="Description";
                
                this.grdSearch.DisplayLayout.Bands[0].Columns["CategoryDescriptor"].Header.Caption = "Category";
                this.grdSearch.DisplayLayout.Bands[0].Columns["SubCategoryDescriptor"].Header.Caption = "Sub Category";

                this.grdSearch.DisplayLayout.Bands[0].Columns["SellingPrice"].Header.Caption = "Selling Price";
                this.grdSearch.DisplayLayout.Bands[0].Columns["SellingPrice"].Format = "######0.00";
                
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

        private void grdSearch_BeforeRowExpanded(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
        {
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                if (orow.Expanded == true)
                    orow.CollapseAll();
            }
            e.Row.Activate();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Preview(false);
        }

        private void Preview(bool PrintIt)
        {
            try
            {
                string sSQL = "";
                ReportClass oRpt = null;
                DataSet oDS = Search();
                oRpt=new rptIIASItemFileListing();
                clsReports.Preview(PrintIt, oDS, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(true);
        }

    }
}
