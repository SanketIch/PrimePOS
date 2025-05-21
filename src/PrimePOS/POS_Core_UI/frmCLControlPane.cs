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
using POS_Core_UI.Reports.Reports;
//using POS_Core_UI.Reports.ReportsUI;
using POS_Core.DataAccess;
using System.Collections.Generic;
using POS_Core.UserManagement;
using POS_Core.Resources;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmCLControlPane : System.Windows.Forms.Form
    {
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCLPoints;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCustomerName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCLCardNo;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lbl2;
        private Infragistics.Win.Misc.UltraLabel lbl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabMain;
        private IContainer components;
        public Infragistics.Win.Misc.UltraButton btnEditCLCard;
        public Infragistics.Win.Misc.UltraButton btnEditCustomer;
        private ImageList imageList1;
        private UltraGrid grdCLCoupons;
        private bool allowedCLCardEdit = false;
        private bool allowedCustomerEdit = false;
        private CLCardsRow selectedCard = null;
        private CustomerRow selectedCustomer = null;

        public frmCLControlPane()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        public frmCLControlPane(Int64 clCardId)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.txtCLCardNo.Text = clCardId.ToString();
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
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCLControlPane));
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.txtCustomerName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCLCardNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnEditCustomer = new Infragistics.Win.Misc.UltraButton();
            this.btnEditCLCard = new Infragistics.Win.Misc.UltraButton();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdCLPoints = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.grdCLCoupons = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCLCardNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCLPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCLCoupons)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Controls.Add(this.txtCustomerName);
            this.ultraTabPageControl1.Controls.Add(this.txtCLCardNo);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Controls.Add(this.btnEditCustomer);
            this.ultraTabPageControl1.Controls.Add(this.btnEditCLCard);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(804, 84);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance66.BackColor = System.Drawing.Color.White;
            appearance66.BackColor2 = System.Drawing.SystemColors.Control;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance66.FontData.BoldAsString = "True";
            appearance66.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance66;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance67.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance67.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance67;
            this.btnSearch.Location = new System.Drawing.Point(665, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(128, 29);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomerName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCustomerName.Location = new System.Drawing.Point(369, 9);
            this.txtCustomerName.MaxLength = 50;
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(133, 23);
            this.txtCustomerName.TabIndex = 2;
            this.txtCustomerName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCustomerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomerName_KeyDown);
            // 
            // txtCLCardNo
            // 
            this.txtCLCardNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCLCardNo.Location = new System.Drawing.Point(98, 9);
            this.txtCLCardNo.MaxLength = 20;
            this.txtCLCardNo.Name = "txtCLCardNo";
            this.txtCLCardNo.Size = new System.Drawing.Size(141, 23);
            this.txtCLCardNo.TabIndex = 1;
            this.txtCLCardNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCLCardNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // lbl2
            // 
            appearance13.ForeColor = System.Drawing.Color.Black;
            appearance13.TextHAlignAsString = "Right";
            this.lbl2.Appearance = appearance13;
            this.lbl2.AutoSize = true;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(249, 11);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(116, 18);
            this.lbl2.TabIndex = 7;
            this.lbl2.Text = "Customer Name";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Center";
            this.lbl1.Appearance = appearance3;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(3, 11);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(96, 19);
            this.lbl1.TabIndex = 5;
            this.lbl1.Text = "CL Card No.";
            this.lbl1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnEditCustomer
            // 
            this.btnEditCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.FontData.BoldAsString = "True";
            appearance16.ForeColor = System.Drawing.Color.Black;
            this.btnEditCustomer.Appearance = appearance16;
            this.btnEditCustomer.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEditCustomer.HotTrackAppearance = appearance17;
            this.btnEditCustomer.Location = new System.Drawing.Point(665, 44);
            this.btnEditCustomer.Name = "btnEditCustomer";
            this.btnEditCustomer.Size = new System.Drawing.Size(128, 30);
            this.btnEditCustomer.TabIndex = 10;
            this.btnEditCustomer.Text = "Edit Customer";
            this.btnEditCustomer.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEditCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEditCustomer.Click += new System.EventHandler(this.btnEditCustomer_Click);
            // 
            // btnEditCLCard
            // 
            this.btnEditCLCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.SystemColors.Control;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.btnEditCLCard.Appearance = appearance8;
            this.btnEditCLCard.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEditCLCard.HotTrackAppearance = appearance9;
            this.btnEditCLCard.Location = new System.Drawing.Point(516, 44);
            this.btnEditCLCard.Name = "btnEditCLCard";
            this.btnEditCLCard.Size = new System.Drawing.Size(128, 30);
            this.btnEditCLCard.TabIndex = 8;
            this.btnEditCLCard.Text = "Edit CL Card";
            this.btnEditCLCard.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEditCLCard.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEditCLCard.Click += new System.EventHandler(this.btnEditCLCard_Click);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn4,
            ultraDataColumn5,
            ultraDataColumn6});
            // 
            // grdCLPoints
            // 
            this.grdCLPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance68.BackColor = System.Drawing.Color.White;
            appearance68.BackColor2 = System.Drawing.Color.White;
            appearance68.BackColorDisabled = System.Drawing.Color.White;
            appearance68.BackColorDisabled2 = System.Drawing.Color.White;
            appearance68.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdCLPoints.DisplayLayout.Appearance = appearance68;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            this.grdCLPoints.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCLPoints.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance69.BackColor = System.Drawing.Color.White;
            appearance69.FontData.BoldAsString = "True";
            appearance69.FontData.SizeInPoints = 9F;
            appearance69.ForeColor = System.Drawing.Color.Black;
            this.grdCLPoints.DisplayLayout.CaptionAppearance = appearance69;
            this.grdCLPoints.DisplayLayout.InterBandSpacing = 10;
            this.grdCLPoints.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCLPoints.DisplayLayout.MaxRowScrollRegions = 1;
            appearance70.BackColor = System.Drawing.Color.White;
            appearance70.BackColor2 = System.Drawing.Color.White;
            this.grdCLPoints.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance70;
            appearance71.BackColor = System.Drawing.Color.White;
            appearance71.BackColor2 = System.Drawing.Color.White;
            appearance71.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.ActiveRowAppearance = appearance71;
            appearance72.BackColor = System.Drawing.Color.White;
            appearance72.BackColor2 = System.Drawing.Color.White;
            appearance72.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.AddRowAppearance = appearance72;
            this.grdCLPoints.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdCLPoints.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCLPoints.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance73.BackColor = System.Drawing.Color.Transparent;
            this.grdCLPoints.DisplayLayout.Override.CardAreaAppearance = appearance73;
            appearance74.BackColor = System.Drawing.Color.White;
            appearance74.BackColor2 = System.Drawing.Color.White;
            appearance74.BackColorDisabled = System.Drawing.Color.White;
            appearance74.BackColorDisabled2 = System.Drawing.Color.White;
            appearance74.BorderColor = System.Drawing.Color.Black;
            appearance74.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdCLPoints.DisplayLayout.Override.CellAppearance = appearance74;
            appearance75.BackColor = System.Drawing.Color.White;
            appearance75.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance75.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance75.BorderColor = System.Drawing.Color.Gray;
            appearance75.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance75.Image = ((object)(resources.GetObject("appearance75.Image")));
            appearance75.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance75.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdCLPoints.DisplayLayout.Override.CellButtonAppearance = appearance75;
            appearance76.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance76.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdCLPoints.DisplayLayout.Override.EditCellAppearance = appearance76;
            appearance77.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.FilteredInRowAppearance = appearance77;
            appearance78.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.FilteredOutRowAppearance = appearance78;
            appearance79.BackColor = System.Drawing.Color.White;
            appearance79.BackColor2 = System.Drawing.Color.White;
            appearance79.BackColorDisabled = System.Drawing.Color.White;
            appearance79.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdCLPoints.DisplayLayout.Override.FixedCellAppearance = appearance79;
            appearance80.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance80.BackColor2 = System.Drawing.Color.Beige;
            this.grdCLPoints.DisplayLayout.Override.FixedHeaderAppearance = appearance80;
            appearance81.BackColor = System.Drawing.Color.White;
            appearance81.BackColor2 = System.Drawing.SystemColors.Control;
            appearance81.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance81.FontData.BoldAsString = "True";
            appearance81.FontData.SizeInPoints = 9F;
            appearance81.ForeColor = System.Drawing.Color.Black;
            appearance81.TextHAlignAsString = "Left";
            appearance81.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdCLPoints.DisplayLayout.Override.HeaderAppearance = appearance81;
            this.grdCLPoints.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance82.BackColor = System.Drawing.Color.LightCyan;
            appearance82.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowAlternateAppearance = appearance82;
            appearance83.BackColor = System.Drawing.Color.White;
            appearance83.BackColor2 = System.Drawing.Color.White;
            appearance83.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance83.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance83.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowAppearance = appearance83;
            appearance84.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowPreviewAppearance = appearance84;
            appearance85.BackColor = System.Drawing.Color.White;
            appearance85.BackColor2 = System.Drawing.SystemColors.Control;
            appearance85.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance85.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowSelectorAppearance = appearance85;
            this.grdCLPoints.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdCLPoints.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance86.BackColor = System.Drawing.Color.Navy;
            appearance86.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdCLPoints.DisplayLayout.Override.SelectedCellAppearance = appearance86;
            appearance87.BackColor = System.Drawing.Color.Navy;
            appearance87.BackColorDisabled = System.Drawing.Color.Navy;
            appearance87.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance87.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance87.BorderColor = System.Drawing.Color.Gray;
            appearance87.ForeColor = System.Drawing.Color.Black;
            this.grdCLPoints.DisplayLayout.Override.SelectedRowAppearance = appearance87;
            this.grdCLPoints.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLPoints.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLPoints.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance88.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.TemplateAddRowAppearance = appearance88;
            this.grdCLPoints.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdCLPoints.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance89.BackColor = System.Drawing.Color.White;
            appearance89.BackColor2 = System.Drawing.SystemColors.Control;
            appearance89.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance89;
            appearance90.BackColor = System.Drawing.Color.White;
            appearance90.BackColor2 = System.Drawing.SystemColors.Control;
            appearance90.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance90.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance90;
            appearance91.BackColor = System.Drawing.Color.White;
            appearance91.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance91;
            appearance92.BackColor = System.Drawing.Color.White;
            appearance92.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance92;
            this.grdCLPoints.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdCLPoints.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdCLPoints.Location = new System.Drawing.Point(14, 139);
            this.grdCLPoints.Name = "grdCLPoints";
            this.grdCLPoints.Size = new System.Drawing.Size(380, 345);
            this.grdCLPoints.TabIndex = 5;
            this.grdCLPoints.Text = "Current Points";
            this.grdCLPoints.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdCLPoints.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdCLPoints.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSearch_InitializeRow);
            // 
            // tabMain
            // 
            appearance30.FontData.BoldAsString = "True";
            this.tabMain.ActiveTabAppearance = appearance30;
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.Appearance = appearance31;
            this.tabMain.BackColorInternal = System.Drawing.Color.Transparent;
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance32.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.tabMain.ClientAreaAppearance = appearance32;
            this.tabMain.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabMain.Controls.Add(this.ultraTabPageControl1);
            this.tabMain.Location = new System.Drawing.Point(9, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabMain.Size = new System.Drawing.Size(808, 109);
            this.tabMain.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.tabMain.TabIndex = 0;
            appearance33.BackColor = System.Drawing.Color.Transparent;
            ultraTab1.Appearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.Transparent;
            appearance34.BackColor2 = System.Drawing.Color.Transparent;
            appearance34.ForeColor = System.Drawing.Color.Black;
            ultraTab1.ClientAreaAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.Transparent;
            appearance35.BackColor2 = System.Drawing.Color.Transparent;
            ultraTab1.SelectedAppearance = appearance35;
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
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(804, 84);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.SystemColors.Control;
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance43.FontData.BoldAsString = "True";
            appearance43.ForeColor = System.Drawing.Color.Black;
            appearance43.Image = ((object)(resources.GetObject("appearance43.Image")));
            this.btnClose.Appearance = appearance43;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance44;
            this.btnClose.Location = new System.Drawing.Point(676, 497);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(124, 30);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "&Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "WB01238_.GIF");
            this.imageList1.Images.SetKeyName(1, "WB01241_.GIF");
            // 
            // grdCLCoupons
            // 
            this.grdCLCoupons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackColorDisabled = System.Drawing.Color.White;
            appearance12.BackColorDisabled2 = System.Drawing.Color.White;
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdCLCoupons.DisplayLayout.Appearance = appearance12;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            this.grdCLCoupons.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdCLCoupons.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.FontData.BoldAsString = "True";
            appearance26.FontData.SizeInPoints = 9F;
            appearance26.ForeColor = System.Drawing.Color.Black;
            this.grdCLCoupons.DisplayLayout.CaptionAppearance = appearance26;
            this.grdCLCoupons.DisplayLayout.InterBandSpacing = 10;
            this.grdCLCoupons.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCLCoupons.DisplayLayout.MaxRowScrollRegions = 1;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.Color.White;
            this.grdCLCoupons.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.ActiveRowAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.Color.White;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.AddRowAppearance = appearance29;
            this.grdCLCoupons.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdCLCoupons.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCLCoupons.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance41.BackColor = System.Drawing.Color.Transparent;
            this.grdCLCoupons.DisplayLayout.Override.CardAreaAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BackColorDisabled = System.Drawing.Color.White;
            appearance42.BackColorDisabled2 = System.Drawing.Color.White;
            appearance42.BorderColor = System.Drawing.Color.Black;
            appearance42.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdCLCoupons.DisplayLayout.Override.CellAppearance = appearance42;
            appearance46.BackColor = System.Drawing.Color.White;
            appearance46.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance46.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            appearance46.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance46.Image = ((object)(resources.GetObject("appearance46.Image")));
            appearance46.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance46.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdCLCoupons.DisplayLayout.Override.CellButtonAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance47.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdCLCoupons.DisplayLayout.Override.EditCellAppearance = appearance47;
            appearance48.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.FilteredInRowAppearance = appearance48;
            appearance49.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.FilteredOutRowAppearance = appearance49;
            appearance50.BackColor = System.Drawing.Color.White;
            appearance50.BackColor2 = System.Drawing.Color.White;
            appearance50.BackColorDisabled = System.Drawing.Color.White;
            appearance50.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdCLCoupons.DisplayLayout.Override.FixedCellAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance51.BackColor2 = System.Drawing.Color.Beige;
            this.grdCLCoupons.DisplayLayout.Override.FixedHeaderAppearance = appearance51;
            appearance53.BackColor = System.Drawing.Color.White;
            appearance53.BackColor2 = System.Drawing.SystemColors.Control;
            appearance53.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance53.FontData.BoldAsString = "True";
            appearance53.FontData.SizeInPoints = 9F;
            appearance53.ForeColor = System.Drawing.Color.Black;
            appearance53.TextHAlignAsString = "Left";
            appearance53.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdCLCoupons.DisplayLayout.Override.HeaderAppearance = appearance53;
            this.grdCLCoupons.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance54.BackColor = System.Drawing.Color.LightCyan;
            appearance54.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowAlternateAppearance = appearance54;
            appearance55.BackColor = System.Drawing.Color.White;
            appearance55.BackColor2 = System.Drawing.Color.White;
            appearance55.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance55.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance55.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowAppearance = appearance55;
            appearance56.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowPreviewAppearance = appearance56;
            appearance57.BackColor = System.Drawing.Color.White;
            appearance57.BackColor2 = System.Drawing.SystemColors.Control;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance57.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowSelectorAppearance = appearance57;
            this.grdCLCoupons.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdCLCoupons.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance58.BackColor = System.Drawing.Color.Navy;
            appearance58.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdCLCoupons.DisplayLayout.Override.SelectedCellAppearance = appearance58;
            appearance59.BackColor = System.Drawing.Color.Navy;
            appearance59.BackColorDisabled = System.Drawing.Color.Navy;
            appearance59.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance59.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance59.BorderColor = System.Drawing.Color.Gray;
            appearance59.ForeColor = System.Drawing.Color.Black;
            this.grdCLCoupons.DisplayLayout.Override.SelectedRowAppearance = appearance59;
            this.grdCLCoupons.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLCoupons.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLCoupons.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance60.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.TemplateAddRowAppearance = appearance60;
            this.grdCLCoupons.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdCLCoupons.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance61.BackColor = System.Drawing.Color.White;
            appearance61.BackColor2 = System.Drawing.SystemColors.Control;
            appearance61.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook2.Appearance = appearance61;
            appearance62.BackColor = System.Drawing.Color.White;
            appearance62.BackColor2 = System.Drawing.SystemColors.Control;
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance62.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook2.ButtonAppearance = appearance62;
            appearance63.BackColor = System.Drawing.Color.White;
            appearance63.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook2.ThumbAppearance = appearance63;
            appearance64.BackColor = System.Drawing.Color.White;
            appearance64.BackColor2 = System.Drawing.Color.White;
            scrollBarLook2.TrackAppearance = appearance64;
            this.grdCLCoupons.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdCLCoupons.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdCLCoupons.Location = new System.Drawing.Point(424, 139);
            this.grdCLCoupons.Name = "grdCLCoupons";
            this.grdCLCoupons.Size = new System.Drawing.Size(380, 345);
            this.grdCLCoupons.TabIndex = 91;
            this.grdCLCoupons.Text = "# of available coupons";
            this.grdCLCoupons.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // frmCLControlPane
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(825, 548);
            this.Controls.Add(this.grdCLCoupons);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdCLPoints);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCLControlPane";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Loyalty Control Pane";
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmCLControlPane_KeyUp);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCustomerName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCLCardNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCLPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCLCoupons)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            Search();
        }

        public CLCardsRow SelectedCLCard
        {
            get { return selectedCard; }
        }

        public CustomerRow SelectedCustomer
        {
            get { return selectedCustomer; }
        }

        private void HideColumn(String colName)
        {
            try
            {
                this.grdCLPoints.DisplayLayout.Bands[0].Columns[colName].Hidden = true;
                this.grdCLPoints.Update();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void Search()
        {
            this.grdCLCoupons.Text = "# of available coupons :0";
            this.grdCLPoints.Text = "Current Point:0";
            PopulateCustomerAndCard();
            PopulateCurrentPointsLogGrid();
            PopulateCouponsStatusGrid();

            EnableOrDisable();
        }

        private void PopulateCustomerAndCard()
        {
            FillCLCardRow();
            if (selectedCard != null)
            {
                CustomerData oCData = (new CustomerSvr()).GetCustomerByID(selectedCard.CustomerID);
                if (oCData != null && oCData.Customer.Count > 0)
                {
                    selectedCustomer = oCData.Customer[0];
                    txtCustomerName.Text = selectedCustomer.CustomerFullName;
                }
                else
                {
                    selectedCustomer = null;
                }
            }
            else
            {
                if (txtCustomerName.Text.Trim().Length == 0)
                {
                    selectedCard = null;
                    selectedCustomer = null;
                }
                else
                {
                    CustomerData oCData = (new CustomerSvr()).Populate(this.txtCustomerName.Text, true, true);
                    if (oCData == null || oCData.Customer.Count == 0)
                    {
                        selectedCustomer = null;
                    }
                    else if (oCData.Customer.Count == 1)
                    {
                        selectedCustomer = oCData.Customer[0];
                        CLCardsData oCards = (new CLCardsSvr()).GetByCustomerID(selectedCustomer.CustomerId);
                        if (oCards != null && oCards.CLCards.Count > 0)
                        {
                            selectedCard = oCards.CLCards[0];
                        }
                        else
                        {
                            selectedCard = null;
                        }
                    }
                    else
                    {
                        //frmCustomerSearch ofrmSearch = new frmCustomerSearch(this.txtCustomerName.Text, true, true,true);
                        frmSearchMain ofrmSearch = new frmSearchMain(this.txtCustomerName.Text, true, true, true, true, clsPOSDBConstants.Customer_tbl);  //18-Dec-2017 JY Added new reference
                        //ofrmSearch.SearchTable = clsPOSDBConstants.Customer_tbl;   //18-Dec-2017 JY Added new reference
                        ofrmSearch.ShowDialog();
                        if (ofrmSearch.IsCanceled == false)
                        {
                            selectedCustomer = (new CustomerSvr()).GetCustomerByID(Configuration.convertNullToInt(ofrmSearch.SelectedRowID())).Customer[0];
                            selectedCard = (new CLCardsSvr()).GetByCLCardID(Configuration.convertNullToInt64(ofrmSearch.SelectedCLPCardID())).CLCards[0];
                            txtCLCardNo.Text = selectedCard.CLCardID.ToString();
                        }
                        else
                        {
                            selectedCard = null;
                            selectedCustomer = null;
                        }
                    }
                }
            }
            if (selectedCustomer != null)
            {
                this.txtCustomerName.Text = selectedCustomer.CustomerFullName;
            }
            if (selectedCard != null)
            {
                this.txtCLCardNo.Text = selectedCard.CLCardID.ToString();
            }
        }

        private void FillCLCardRow()
        {
            if (txtCLCardNo.Text.Trim().Length == 0)
            {
                selectedCard = null;
            }
            else
            {
                CLCardsSvr oSvr = new CLCardsSvr();
                CLCardsData oCards = oSvr.GetByCLCardID(Configuration.convertNullToInt64(txtCLCardNo.Text));
                if (oCards == null || oCards.CLCards.Rows.Count == 0)
                {
                    selectedCard = null;
                }
                else
                {
                    selectedCard = oCards.CLCards[0];
                }
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void EnableOrDisable()
        {
            btnEditCLCard.Enabled = (selectedCard != null);
            btnEditCustomer.Enabled=(selectedCustomer != null);
        }

        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
                clsUIHelper.SetAppearance(this.grdCLPoints);
                clsUIHelper.SetReadonlyRow(this.grdCLPoints);

                clsUIHelper.SetAppearance(this.grdCLCoupons);
                clsUIHelper.SetReadonlyRow(this.grdCLCoupons);

                Search();

                this.txtCLCardNo.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCLCardNo.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtCustomerName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCustomerName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.ActiveControl = this.txtCLCardNo;
                clsUIHelper.setColorSchecme(this);

                EnableOrDisable();

                allowedCLCardEdit = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID, 0);
                allowedCustomerEdit = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Customers.ID, 0);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            this.txtCLCardNo.Focus();
        }

        private void EditCustomer()
        {
            if (selectedCustomer==null) return;

            try
            {
                frmCustomers oCustomer = new frmCustomers();
                oCustomer.Edit(selectedCustomer.CustomerId.ToString());
                oCustomer.ShowDialog(this);
                if (!oCustomer.IsCanceled)
                    Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditCLCards()
        {
            if (selectedCard==null) return;

            try
            {
                frmCLCards ofrmCLCards = new frmCLCards();
                ofrmCLCards.Edit(selectedCard.CLCardID);
                ofrmCLCards.ShowDialog(this);
                if (!ofrmCLCards.IsCanceled)
                    Search();
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

        private void frmCLControlPane_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F2:
                    break;
                case Keys.F3:
                    EditCustomer();
                    break;
                case Keys.F4:
                    Search();
                    break;    
            }
        }

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            /*for (int i = 0; i < this.grdCLPoints.DisplayLayout.Bands[0].Columns.Count; i++)
            {
                if (this.grdCLPoints.DisplayLayout.Bands[0].Columns[i].DataType == typeof(System.Decimal))
                {
                    this.grdCLPoints.DisplayLayout.Bands[0].Columns[i].Format = "#######0.000#";// changed 0.00 to 0.000# by atul 11-jan-2010 for jira issue
                    this.grdCLPoints.DisplayLayout.Bands[0].Columns[i].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }

                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Home"].MaskInput = "(###) ###-####";
                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Home"].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Home"].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Home"].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Office"].MaskInput = "(###) ###-####";
                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Office"].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Office"].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdCLPoints.DisplayLayout.Bands[0].Columns["Phone Office"].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                grdCLPoints.DisplayLayout.Bands[0].Columns["Cell No."].MaskInput = "(###) ###-####";
                grdCLPoints.DisplayLayout.Bands[0].Columns["Cell No."].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdCLPoints.DisplayLayout.Bands[0].Columns["Cell No."].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdCLPoints.DisplayLayout.Bands[0].Columns["Cell No."].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;*/
        }

        private void resizeColumns(UltraGrid grdData)
        {
            foreach (UltraGridColumn oCol in grdData.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }
        }

        private void txtCode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCLCardNo.Text.Trim() != "")
                {
                    btnSearch_Click(btnSearch, new EventArgs());
                }
                else
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            if (e.KeyData == Keys.Down)
            {
                if (this.grdCLPoints.Rows.Count > 0)
                {
                    this.grdCLPoints.Focus();
                    this.grdCLPoints.ActiveRow = this.grdCLPoints.Rows[0];
                }
            }
        }

        private void txtCustomerName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCustomerName.Text.Trim() != "")
                {
                    btnSearch_Click(btnSearch, new EventArgs());
                }
                else
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Preview(false);
        }

        private void Preview(bool PrintId)
        {
            /*try
            {
                rptUsersLabel oRpt = new rptUsersLabel();
                Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
                DataSet rptDS = new DataSet();
                rptDS.Tables.Add();
                rptDS.Tables[0].Columns.Add("ItemID");
                rptDS.Tables[0].Columns.Add("Picture", System.Type.GetType("System.Byte[]"));

                foreach (DataRow oRow in oDataSet.Tables[0].Rows)
                {
                    try
                    {
                        string sBarcode = "99" + EncryptString.CustomEncrypt(Convert.ToInt32(oRow["ID"].ToString())) + "99";

                        this.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE39", "H", System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp");
                        rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp") });
                    }
                    catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }


                }

                oRpt.Database.Tables[0].SetDataSource(rptDS.Tables[0]);
                if (PrintId == false)
                {
                    clsReports.ShowReport(oRpt);
                }
                else
                {
                    clsReports.PrintReport(oRpt);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }*/
        }

        private void grdSearch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            //try
            //{
            //    bool isContains = e.Row.Band.Columns.Exists("PO Status");

            //    if(isContains)
            //    {
            //        string orderStatus = e.Row.Cells["PO Status"].Text.ToString().Trim();

            //        if (orderStatus == clsPOSDBConstants.Incomplete)
            //            e.Row.Appearance.ForeColor = Color.Red;
            //        else if (orderStatus == clsPOSDBConstants.Pending)
            //            e.Row.Appearance.ForeColor = Color.Red;
            //        else if (orderStatus == clsPOSDBConstants.Queued)
            //             e.Row.Appearance.ForeColor = Color.Red;      
            //    }
            //}
            //catch (Exception ex)
            //{
            //   POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            //}
        }

        private void btnEditCLCard_Click(object sender, EventArgs e)
        {
            EditCLCards();
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            EditCustomer();
        }

        private void PopulateCurrentPointsLogGrid()
        {
            Int64 clCardId=-1;
            if (selectedCard!=null)
            {
                clCardId=selectedCard.CLCardID;
            }

            String sql ="SELECT  pt.TransId as TransNo, pt.LoyaltyPoints as Points, pt.TransDate as TransDate " +
                        ",'T' as RowType From postransaction pt, customer c, cl_cards cld " +
                        "Where pt.customerid=c.customerid and c.customerid=cld.customerid " +
                        "and cardid=" + clCardId+ " and pt.LoyaltyPoints>0 " +
                         "Union All " +
                        "Select 0 as TransNo, NewPoints as Points, CreatedOn as TransDate,'A' as RowType  " +
                        "From cl_PointsAdjustmentLog Where cardid=" + clCardId + " " +
                        "Union All " +
                        "SELECT coupon.UsedInTransId as TransNo,-1*coupon.Points as Points,coupon.CreatedOn as TransDate " +
                        ",'U' as RowType From cl_Coupons coupon , postransaction pt " +
                        "Where coupon.UsedInTransId=pt.transid and coupon.cardid=" +clCardId+ " and coupon.isActive=1 " +
                        "And coupon.IsCouponUsed=1 " +
                        "Union All " +
                        "SELECT coupon.CreatedInTransId as TransNo,-1*coupon.Points as Points,coupon.CreatedOn as TransDate  " +
                        ",'X' as RowType From cl_Coupons coupon  " +
                        "Where cardid=" + clCardId+ " and coupon.isActive=1 and coupon.IsCouponUsed=0 " +
                        "And DateAdd(d,ExpiryDays,CreatedOn)<GetDate() " +
                        "Order by TransDate";
            DataSet result = new Search().SearchData(sql);
            result.Tables[0].Columns.Add("Balance", typeof(Int32));
            Int32 balance = 0;
            foreach (DataRow row in result.Tables[0].Rows)
            {
                if (row["RowType"].ToString() == "A")
                {
                    balance = 0;
                }
                balance+=Configuration.convertNullToInt( row["Points"]);
                row["Balance"] = balance;
            }

            this.grdCLPoints.DataSource = result;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransNo"].Header.Caption = "Trans #";
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["Balance"].Header.Caption = "Ending Point";
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Format= "MM/dd/yyyy hh:mm:ss";
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Header.VisiblePosition=4;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Header.Caption = "Date/Time";
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowType"].Hidden = true;
            this.grdCLPoints.Text = "Current Point:"+balance.ToString();
            this.resizeColumns(grdCLPoints);
        }

        private void PopulateCouponsStatusGrid()
        {
            Int64 clCardId = -1;
            if (selectedCard != null)
            {
                clCardId = selectedCard.CLCardID;
            }

            String sql = "SELECT coupon.id as CouponId ,Case When coupon.IsCouponUsed=1 Then coupon.UsedInTransId " +
                        " Else coupon.CreatedInTransId End as TransNo ,Case When coupon.IsCouponUsed=1 Then " +
                        "(Select TransDate From PosTransaction pt Where pt.TransId=coupon.UsedInTransId) " +
                        "Else coupon.CreatedOn End as TransDate,Case When coupon.IsCouponUsed=1 Then 'Used' " +
                        "When coupon.isActive=0 Then 'Cancelled' " +
                        "When DateAdd(d,ExpiryDays,CreatedOn)<GetDate() Then 'Coupon Expired' " +
                        "When IsNUll(coupon.CreatedInTransId,0)=0 Then 'Adjustment'Else 'Available' " +
                        "End as RowType From cl_Coupons coupon Where cardid= " + clCardId  +
                        " Order by TransDate ";
            DataSet result = new Search().SearchData(sql);
            Int32 availableCoupons= 0;
            foreach (DataRow row in result.Tables[0].Rows)
            {
                if (row["RowType"].ToString() == "Available" || row["RowType"].ToString() == "Adjustment")
                {
                    availableCoupons ++;
                }
            }
            this.grdCLCoupons.DataSource = result;

            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransNo"].Header.Caption = "Trans #";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["CouponId"].Header.Caption = "Coupon #";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["RowType"].Header.Caption = "Status";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransDate"].Format = "MM/dd/yyyy hh:mm:ss";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransDate"].Header.VisiblePosition = 4;
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransDate"].Header.Caption = "Date/Time";
            this.grdCLCoupons.Text = "# of available coupons :"+availableCoupons;
            this.resizeColumns(grdCLCoupons);
        }

    }
}

