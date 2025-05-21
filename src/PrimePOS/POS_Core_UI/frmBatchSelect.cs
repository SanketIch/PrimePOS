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
using PharmData;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmVendorSearch.
    /// </summary>
    public class frmBatchSelect : System.Windows.Forms.Form
    {
        public bool IsCanceled = true;
        private string sPatientNo = "";
        private int iPatientNo;
        private int CurrentX;
        private int CurrentY;
        private RXHeader oPatient = null;
        private string buttonStatusCheck = "C&heck All";
        private string buttonStatusUnCheck = "&UnCheck All";

        ILogger logger = LogManager.GetCurrentClassLogger();

        #region builtin var
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clTo1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo clFrom1;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnEdit;
        private IContainer components;
        private CheckBox chkSelectAll;
        private Infragistics.Win.Misc.UltraButton btnCheckBoxStatus;

        public DataTable SelectedData = null;
        PharmBL oPharmBL = new PharmBL();//Added by Krishna on 21 August 2012
        #endregion


        public frmBatchSelect(RXHeader oRXHeader)
        {           
        
            InitializeComponent();
           
            if(oRXHeader != null && !string.IsNullOrWhiteSpace( oRXHeader.PatientNo))
            {
                try
                {
                    iPatientNo = Convert.ToInt32(oRXHeader.PatientNo);
                }
                catch(Exception ex)
                {
                    logger.Error(ex);
                    iPatientNo = 0;
                }
            }

        }

        #region Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
        public frmBatchSelect(int PatientNo)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            iPatientNo = PatientNo;
        }
        #endregion

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
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsSelected", 0);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            this.clTo1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.clFrom1 = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnCheckBoxStatus = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            this.SuspendLayout();
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
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackColorDisabled = System.Drawing.Color.White;
            appearance1.BackColorDisabled2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance1;
            this.grdSearch.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Width = 223;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 221;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 221;
            ultraGridColumn4.DataType = typeof(bool);
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ultraGridColumn4.Width = 164;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.HeaderVisible = true;
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance4;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackColorDisabled = System.Drawing.Color.White;
            appearance6.BackColorDisabled2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Black;
            appearance6.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Gray;
            appearance7.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance7.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BackColorDisabled = System.Drawing.Color.White;
            appearance11.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance12.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.SystemColors.Control;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.Black;
            appearance13.TextHAlignAsString = "Left";
            appearance13.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance15.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.Navy;
            appearance18.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.Navy;
            appearance19.BackColorDisabled = System.Drawing.Color.Navy;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance19.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            appearance19.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance19;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance20;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.SystemColors.Control;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.SystemColors.Control;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance24;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(12, 32);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(860, 284);
            this.grdSearch.TabIndex = 6;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.BeforeRowExpanded += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdSearch_BeforeRowExpanded);
            this.grdSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdSearch_KeyUp);
            this.grdSearch.Leave += new System.EventHandler(this.grdSearch_Leave);
            this.grdSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseClick);
            this.grdSearch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseMove);
            // 
            // sbMain
            // 
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BorderColor = System.Drawing.Color.Black;
            appearance25.FontData.Name = "Verdana";
            appearance25.FontData.SizeInPoints = 10F;
            appearance25.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance25;
            this.sbMain.Location = new System.Drawing.Point(0, 369);
            this.sbMain.Name = "sbMain";
            appearance26.BorderColor = System.Drawing.Color.Black;
            appearance26.BorderColor3DBase = System.Drawing.Color.Black;
            appearance26.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance26;
            appearance27.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance27;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(884, 25);
            this.sbMain.TabIndex = 7;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.FontData.BoldAsString = "True";
            appearance28.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance28;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance29;
            this.btnClose.Location = new System.Drawing.Point(754, 332);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.SystemColors.Control;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.FontData.BoldAsString = "True";
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance30;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance31;
            this.btnEdit.Location = new System.Drawing.Point(583, 332);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(102, 30);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "&Ok";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.chkSelectAll.Checked = true;
            this.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectAll.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSelectAll.Location = new System.Drawing.Point(12, 12);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(15, 14);
            this.chkSelectAll.TabIndex = 10;
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnCheckBoxStatus
            // 
            this.btnCheckBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance32.FontData.BoldAsString = "True";
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.btnCheckBoxStatus.Appearance = appearance32;
            this.btnCheckBoxStatus.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance33.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnCheckBoxStatus.HotTrackAppearance = appearance33;
            this.btnCheckBoxStatus.Location = new System.Drawing.Point(26, 332);
            this.btnCheckBoxStatus.Name = "btnCheckBoxStatus";
            this.btnCheckBoxStatus.Size = new System.Drawing.Size(121, 30);
            this.btnCheckBoxStatus.TabIndex = 11;
            this.btnCheckBoxStatus.TabStop = false;
            this.btnCheckBoxStatus.Text = "&Uncheck All";
            this.btnCheckBoxStatus.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnCheckBoxStatus.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCheckBoxStatus.Click += new System.EventHandler(this.btnCheckBoxStatus_Click);
            // 
            // frmBatchSelect
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(884, 394);
            this.Controls.Add(this.btnCheckBoxStatus);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.grdSearch);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatchSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Intake Batch";
            this.Activated += new System.EventHandler(this.frmSearchMain_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.clTo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clFrom1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void grdSearch_MouseClick(object sender, MouseEventArgs e)
        {
            UIElement element = grdSearch.DisplayLayout.UIElement.ElementFromPoint(e.Location);
            UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
            bool isSelected = false;
            UltraGridRow row = grdSearch.DisplayLayout.ActiveRow;
            if (element is CheckIndicatorUIElement)
            {

                cell.Value = !Configuration.convertNullToBoolean(cell.Value.ToString());

            }
        }
        #endregion

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                logger.Error(exp);
            }
        }

        public void Search()
        {
            SelectedData = Search(iPatientNo);
            //Added By Shitaljit for JIRA-920
           

            

            grdSearch.DataSource = SelectedData;
            

            this.resizeColumns();
            grdSearch.Focus();
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
            grdSearch.Refresh();
            if (grdSearch.Rows.Count == 0)
            {
                this.chkSelectAll.Visible = false;
                this.btnCheckBoxStatus.Visible = false;
            }
            else
            {
                this.chkSelectAll.Visible = true;
                this.btnCheckBoxStatus.Visible = true;
                ChangeCheckButtonStatus(true);
            }
            

            sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count;

            //this.btnEdit.Focus();
        }




        public DataTable Search(int PatientNo)
        {
           
            DataTable selectedBatches = null;
            //bool HasOpenBatches = false;
            string batches = oPharmBL.GetAllBatchesForPatient(PatientNo); 
            DataTable dt = oPharmBL.GetBatchStatusfromView(batches);
            dt.Columns.Add("IsSelected", typeof(Boolean));
            foreach (DataRow oRow in dt.Rows)
            {
                oRow["IsSelected"] = true;
                
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                //PrimePOS-2518 Jenny
                //var verifiedBatches = dt.AsEnumerable().Where(x => x.Field<string>("OFSbatchStatus").Trim().Equals(Configuration.CInfo.IntakeBatchStatus, StringComparison.OrdinalIgnoreCase)).CopyToDataTable();
                var verifiedBatches = dt.AsEnumerable().Where(x => Convert.ToBoolean(x.Field<bool>("IsReadyForPayment"))).CopyToDataTable();//Arvind 2925
                if (verifiedBatches != null && verifiedBatches.Rows.Count > 0)
                {
                    selectedBatches = verifiedBatches.Copy();
                }

            }

            return selectedBatches;
           
            
        }

      
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }


        private void frmSearch_Load(object sender, System.EventArgs e)
        {
            try
            {
               
                clsUIHelper.SetAppearance(this.grdSearch);
                clsUIHelper.SetReadonlyRow(this.grdSearch);


                this.IsCanceled = false;
      

                clsUIHelper.setColorSchecme(this);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                logger.Error(exp);
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
                logger.Error(exp);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.IsCanceled = true;
            this.Close();
        }

        private void grdSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (this.grdSearch.ActiveRow != null)
                {
                    this.grdSearch.ActiveRow.Cells["IsSelected"].Value = (Configuration.convertNullToBoolean(grdSearch.ActiveRow.Cells["IsSelected"].Value.ToString()) == false);
                }
            }
            
        }

       

        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                this.grdSearch.DisplayLayout.Bands[0].HeaderVisible = false;
                
                this.grdSearch.DisplayLayout.Bands[0].Columns["isSelected"].Header.SetVisiblePosition(0, false);
                this.grdSearch.DisplayLayout.Bands[0].Columns["isSelected"].Header.Caption = "";

                this.grdSearch.DisplayLayout.Bands[0].Columns["BatchId"].Header.SetVisiblePosition(1,false);
                this.grdSearch.DisplayLayout.Bands[0].Columns["BatchId"].Header.Caption = "Batch ID";

                this.grdSearch.DisplayLayout.Bands[0].Columns["CreateDate"].Header.Caption = "Created On";
                this.grdSearch.DisplayLayout.Bands[0].Columns["CreateDate"].Header.SetVisiblePosition(2, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["ReadyByDate"].Header.Caption = "Ready By";
                this.grdSearch.DisplayLayout.Bands[0].Columns["ReadyByDate"].Header.SetVisiblePosition(3, false);

                this.grdSearch.DisplayLayout.Bands[0].Columns["RX_COUNT"].Header.Caption = "Rx's in Batch";
                this.grdSearch.DisplayLayout.Bands[0].Columns["RX_COUNT"].Header.SetVisiblePosition(4, false);

                

                //this.grdSearch.DisplayLayout.Bands[0].Columns["ExpectedPickupTime"].Header.Caption = "Expected Pickup";
                //this.grdSearch.DisplayLayout.Bands[0].Columns["ExpectedPickupTime"].Header.SetVisiblePosition(5, false);


                this.grdSearch.DisplayLayout.Bands[0].Columns["OFSbatchStatus"].Header.SetVisiblePosition(5, false);   
                this.grdSearch.DisplayLayout.Bands[0].Columns["OFSbatchStatus"].Header.Caption = "Batch Status Code";   
                
                
                

                
            }
            catch (Exception ex) { logger.Error(ex); }
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
            catch (Exception ex) { logger.Error(ex); }
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
            catch (Exception ex) { logger.Error(ex); }
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


        

        private void btnEdit_Click(object sender, EventArgs e)
        {
            for (int i=0;i<this.SelectedData.Rows.Count;i++)
            {
                if (Configuration.convertNullToBoolean(SelectedData.Rows[i]["IsSelected"].ToString()) == false)
                {
                    SelectedData.Rows.Remove(SelectedData.Rows[i]);
                    i--;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.IsCanceled = false;
            this.Close();
        }

       

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            ChangeGridRowSelection(chkSelectAll.Checked);
        }
        //Added By Dharmendra on Apr-29-09
        //to set the focus on OK button after leaving grid
        private void grdSearch_Leave(object sender, EventArgs e)
        {
            this.btnEdit.Focus();
        }
        //Added Till Here

        private void ChangeCheckButtonStatus(bool CheckAll)
        {
            if (CheckAll == true)
            {
                this.btnCheckBoxStatus.Text = buttonStatusUnCheck;
            }
            else
            {
                this.btnCheckBoxStatus.Text = buttonStatusCheck;
            }
        }

        private void btnCheckBoxStatus_Click(object sender, EventArgs e)
        {
            if (btnCheckBoxStatus.Text == buttonStatusUnCheck)
            {
                ChangeGridRowSelection(false);
            }
            else
            {
                ChangeGridRowSelection(true);
            }
        }

        private void ChangeGridRowSelection(bool bSelectAll)
        {

            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow orow in grdSearch.DisplayLayout.Rows)
            {
                
                    orow.Cells["IsSelected"].Value = bSelectAll;
                
            }
            chkSelectAll.Checked = bSelectAll;
            ChangeCheckButtonStatus(bSelectAll);
        }

    }
}
