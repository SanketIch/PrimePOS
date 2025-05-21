using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using POS_Core.DataAccess;
using System.Data;
using POS_Core.Resources;
using POS_Core.LabelHandler;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.LabelHandler.RxLabel;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPayOut.
    /// </summary>
    public class frmPayOut : System.Windows.Forms.Form
    {
        private PayOutData oPayOutData = new PayOutData();
        private PayOutRow oPayOutRow;
        private PayOutCatRow oPayoutCatRow;
        private PayOut oPayOut = new PayOut();
        private PayOutCatSvr oPayoutCatSvr;
        public static int intPayOutId = 0;
        private bool ISUserAllow = false;
        private bool bIsEdit = false;

        private bool unloadMe;

        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHistory;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtAmount;
        private System.Windows.Forms.GroupBox grbFields;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.Panel frmPayOut_Fill_Panel;
        private System.Windows.Forms.ImageList imageList1;
        private Infragistics.Win.Misc.UltraLabel txtTotalAmt;
        private Infragistics.Win.Misc.UltraLabel lblHSubTotal;
        private Infragistics.Win.Misc.UltraButton btnNew;
        private Infragistics.Win.Misc.UltraLabel ultralblCat;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbPayoutCat;
        private System.ComponentModel.IContainer components;
        private Infragistics.Win.Misc.UltraButton btnPayoutCategory;
        private Infragistics.Win.Misc.UltraPanel pnlNew;
        private Infragistics.Win.Misc.UltraLabel lblNew;
        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlSave;
        private Infragistics.Win.Misc.UltraLabel lblSave;
        private Infragistics.Win.Misc.UltraPanel pnlPayoutCategory;
        private Infragistics.Win.Misc.UltraLabel lblPayoutCategory;
        private int CatID;
        public frmPayOut()
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Reference");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Amount", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPayOut));
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Reference");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.grdHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.grbFields = new System.Windows.Forms.GroupBox();
            this.pnlPayoutCategory = new Infragistics.Win.Misc.UltraPanel();
            this.btnPayoutCategory = new Infragistics.Win.Misc.UltraButton();
            this.lblPayoutCategory = new Infragistics.Win.Misc.UltraLabel();
            this.cmbPayoutCat = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultralblCat = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSave = new Infragistics.Win.Misc.UltraPanel();
            this.lblSave = new Infragistics.Win.Misc.UltraLabel();
            this.pnlNew = new Infragistics.Win.Misc.UltraPanel();
            this.btnNew = new Infragistics.Win.Misc.UltraButton();
            this.lblNew = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtTotalAmt = new Infragistics.Win.Misc.UltraLabel();
            this.lblHSubTotal = new Infragistics.Win.Misc.UltraLabel();
            this.frmPayOut_Fill_Panel = new System.Windows.Forms.Panel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.grbFields.SuspendLayout();
            this.pnlPayoutCategory.ClientArea.SuspendLayout();
            this.pnlPayoutCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPayoutCat)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            this.pnlSave.ClientArea.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlNew.ClientArea.SuspendLayout();
            this.pnlNew.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.frmPayOut_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel11
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel11.Appearance = appearance1;
            this.ultraLabel11.Location = new System.Drawing.Point(14, 94);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(70, 14);
            this.ultraLabel11.TabIndex = 3;
            this.ultraLabel11.Text = "Amount";
            this.ultraLabel11.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel14
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel14.Appearance = appearance2;
            this.ultraLabel14.Location = new System.Drawing.Point(14, 66);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(92, 20);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "Description";
            this.ultraLabel14.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(123, 64);
            this.txtDescription.MaxLength = 100;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(770, 23);
            this.txtDescription.TabIndex = 2;
            // 
            // txtAmount
            // 
            this.txtAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtAmount.Location = new System.Drawing.Point(123, 90);
            this.txtAmount.MaskInput = "nn,nnn.nn";
            this.txtAmount.MaxValue = 99999.99D;
            this.txtAmount.MinValue = 0;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.NullText = "0.00";
            this.txtAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtAmount.Size = new System.Drawing.Size(98, 23);
            this.txtAmount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtAmount.SpinWrap = true;
            this.txtAmount.TabIndex = 3;
            this.txtAmount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grdHistory
            // 
            this.grdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackColorDisabled = System.Drawing.Color.White;
            appearance3.BackColorDisabled2 = System.Drawing.Color.White;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdHistory.DisplayLayout.Appearance = appearance3;
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn1.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn1.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn1.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn1.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.Header.VisiblePosition = 0;
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdHistory.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistory.DisplayLayout.InterBandSpacing = 10;
            this.grdHistory.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHistory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.AddRowAppearance = appearance6;
            this.grdHistory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdHistory.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdHistory.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdHistory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.grdHistory.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BackColorDisabled = System.Drawing.Color.White;
            appearance8.BackColorDisabled2 = System.Drawing.Color.White;
            appearance8.BorderColor = System.Drawing.Color.Black;
            appearance8.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdHistory.DisplayLayout.Override.CellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            appearance9.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            appearance9.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdHistory.DisplayLayout.Override.CellButtonAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdHistory.DisplayLayout.Override.EditCellAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredInRowAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredOutRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BackColorDisabled = System.Drawing.Color.White;
            appearance13.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.FixedCellAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance14.BackColor2 = System.Drawing.Color.Beige;
            this.grdHistory.DisplayLayout.Override.FixedHeaderAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.FontData.BoldAsString = "True";
            appearance15.FontData.SizeInPoints = 10F;
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdHistory.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.grdHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdHistory.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAlternateAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance17.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowPreviewAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowSelectorAppearance = appearance19;
            this.grdHistory.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdHistory.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdHistory.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance20.BackColor = System.Drawing.Color.Navy;
            appearance20.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdHistory.DisplayLayout.Override.SelectedCellAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.Navy;
            appearance21.BackColorDisabled = System.Drawing.Color.Navy;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance21.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            appearance21.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.SelectedRowAppearance = appearance21;
            this.grdHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.TemplateAddRowAppearance = appearance22;
            this.grdHistory.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdHistory.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance23.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance23.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            appearance23.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            scrollBarLook1.Appearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.LightGray;
            appearance24.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            scrollBarLook1.ButtonAppearance = appearance24;
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook1.ThumbAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.Gainsboro;
            appearance26.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance26.BorderAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance26.BorderColor = System.Drawing.Color.White;
            appearance26.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            appearance26.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance26;
            this.grdHistory.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdHistory.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdHistory.Location = new System.Drawing.Point(14, 23);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(879, 208);
            this.grdHistory.TabIndex = 0;
            this.grdHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdHistory.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdHistory_InitializeRow);
            this.grdHistory.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdHistory_ClickCellButton);
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            // 
            // btnClose
            // 
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance27.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance27;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance28;
            this.btnClose.Location = new System.Drawing.Point(65, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Tag = "NOCOLOR";
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance = appearance29;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(65, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance30.ForeColor = System.Drawing.Color.White;
            appearance30.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance30.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance30;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(12, 1);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(905, 38);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Pay Out Information";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblTransactionType.Click += new System.EventHandler(this.lblTransactionType_Click);
            // 
            // grbFields
            // 
            this.grbFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbFields.Controls.Add(this.pnlPayoutCategory);
            this.grbFields.Controls.Add(this.cmbPayoutCat);
            this.grbFields.Controls.Add(this.ultralblCat);
            this.grbFields.Controls.Add(this.ultraLabel1);
            this.grbFields.Controls.Add(this.ultraLabel7);
            this.grbFields.Controls.Add(this.ultraLabel14);
            this.grbFields.Controls.Add(this.txtDescription);
            this.grbFields.Controls.Add(this.ultraLabel11);
            this.grbFields.Controls.Add(this.txtAmount);
            this.grbFields.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grbFields.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbFields.Location = new System.Drawing.Point(12, 37);
            this.grbFields.Name = "grbFields";
            this.grbFields.Size = new System.Drawing.Size(905, 125);
            this.grbFields.TabIndex = 0;
            this.grbFields.TabStop = false;
            // 
            // pnlPayoutCategory
            // 
            // 
            // pnlPayoutCategory.ClientArea
            // 
            this.pnlPayoutCategory.ClientArea.Controls.Add(this.btnPayoutCategory);
            this.pnlPayoutCategory.ClientArea.Controls.Add(this.lblPayoutCategory);
            this.pnlPayoutCategory.Location = new System.Drawing.Point(404, 19);
            this.pnlPayoutCategory.Name = "pnlPayoutCategory";
            this.pnlPayoutCategory.Size = new System.Drawing.Size(190, 30);
            this.pnlPayoutCategory.TabIndex = 40;
            // 
            // btnPayoutCategory
            // 
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.SystemColors.Control;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance31.FontData.BoldAsString = "True";
            appearance31.ForeColor = System.Drawing.Color.Black;
            this.btnPayoutCategory.Appearance = appearance31;
            this.btnPayoutCategory.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnPayoutCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance32.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnPayoutCategory.HotTrackAppearance = appearance32;
            this.btnPayoutCategory.Location = new System.Drawing.Point(30, 0);
            this.btnPayoutCategory.Name = "btnPayoutCategory";
            this.btnPayoutCategory.Size = new System.Drawing.Size(160, 30);
            this.btnPayoutCategory.TabIndex = 1;
            this.btnPayoutCategory.TabStop = false;
            this.btnPayoutCategory.Text = "&Payout Category";
            this.btnPayoutCategory.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnPayoutCategory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPayoutCategory.Click += new System.EventHandler(this.btnPayoutCategory_Click);
            // 
            // lblPayoutCategory
            // 
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance33.ForeColor = System.Drawing.Color.White;
            appearance33.TextHAlignAsString = "Center";
            appearance33.TextVAlignAsString = "Middle";
            this.lblPayoutCategory.Appearance = appearance33;
            this.lblPayoutCategory.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPayoutCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblPayoutCategory.Location = new System.Drawing.Point(0, 0);
            this.lblPayoutCategory.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblPayoutCategory.Name = "lblPayoutCategory";
            this.lblPayoutCategory.Size = new System.Drawing.Size(30, 30);
            this.lblPayoutCategory.TabIndex = 0;
            this.lblPayoutCategory.Tag = "NOCOLOR";
            this.lblPayoutCategory.Text = "F4";
            this.lblPayoutCategory.Click += new System.EventHandler(this.btnPayoutCategory_Click);
            // 
            // cmbPayoutCat
            // 
            this.cmbPayoutCat.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbPayoutCat.Location = new System.Drawing.Point(123, 22);
            this.cmbPayoutCat.Name = "cmbPayoutCat";
            this.cmbPayoutCat.Size = new System.Drawing.Size(275, 25);
            this.cmbPayoutCat.TabIndex = 0;
            this.cmbPayoutCat.SelectionChangeCommitted += new System.EventHandler(this.cmbPayoutCat_SelectionChangeCommitted);
            this.cmbPayoutCat.SelectionChanged += new System.EventHandler(this.cmbPayoutCat_SelectionChanged);
            this.cmbPayoutCat.ValueChanged += new System.EventHandler(this.cmbPayoutCat_ValueChanged);
            this.cmbPayoutCat.Leave += new System.EventHandler(this.cmbPayoutCat_Leave);
            // 
            // ultralblCat
            // 
            appearance34.ForeColor = System.Drawing.Color.White;
            this.ultralblCat.Appearance = appearance34;
            this.ultralblCat.Location = new System.Drawing.Point(14, 23);
            this.ultralblCat.Name = "ultralblCat";
            this.ultralblCat.Size = new System.Drawing.Size(92, 20);
            this.ultralblCat.TabIndex = 39;
            this.ultralblCat.Text = "Category";
            this.ultralblCat.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance35.ForeColor = System.Drawing.Color.White;
            appearance35.TextHAlignAsString = "Center";
            appearance35.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance35;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(107, 99);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel1.TabIndex = 2;
            this.ultraLabel1.Text = "*";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel7
            // 
            appearance36.ForeColor = System.Drawing.Color.White;
            appearance36.TextHAlignAsString = "Center";
            appearance36.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance36;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(107, 71);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel7.TabIndex = 1;
            this.ultraLabel7.Text = "*";
            this.ultraLabel7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.pnlClose);
            this.groupBox2.Controls.Add(this.pnlSave);
            this.groupBox2.Controls.Add(this.pnlNew);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(905, 56);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            this.pnlClose.Location = new System.Drawing.Point(753, 17);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(140, 30);
            this.pnlClose.TabIndex = 5;
            // 
            // lblClose
            // 
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance37.ForeColor = System.Drawing.Color.White;
            appearance37.TextHAlignAsString = "Center";
            appearance37.TextVAlignAsString = "Middle";
            this.lblClose.Appearance = appearance37;
            this.lblClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblClose.Location = new System.Drawing.Point(0, 0);
            this.lblClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(65, 30);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = "NOCOLOR";
            this.lblClose.Text = "Alt + C";
            this.lblClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlSave
            // 
            // 
            // pnlSave.ClientArea
            // 
            this.pnlSave.ClientArea.Controls.Add(this.btnSave);
            this.pnlSave.ClientArea.Controls.Add(this.lblSave);
            this.pnlSave.Location = new System.Drawing.Point(591, 17);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(140, 30);
            this.pnlSave.TabIndex = 4;
            // 
            // lblSave
            // 
            appearance38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance38.ForeColor = System.Drawing.Color.White;
            appearance38.TextHAlignAsString = "Center";
            appearance38.TextVAlignAsString = "Middle";
            this.lblSave.Appearance = appearance38;
            this.lblSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblSave.Location = new System.Drawing.Point(0, 0);
            this.lblSave.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(65, 30);
            this.lblSave.TabIndex = 0;
            this.lblSave.Tag = "NOCOLOR";
            this.lblSave.Text = "Alt + S";
            this.lblSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlNew
            // 
            // 
            // pnlNew.ClientArea
            // 
            this.pnlNew.ClientArea.Controls.Add(this.btnNew);
            this.pnlNew.ClientArea.Controls.Add(this.lblNew);
            this.pnlNew.Location = new System.Drawing.Point(470, 17);
            this.pnlNew.Name = "pnlNew";
            this.pnlNew.Size = new System.Drawing.Size(100, 30);
            this.pnlNew.TabIndex = 3;
            // 
            // btnNew
            // 
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.ForeColor = System.Drawing.Color.Black;
            this.btnNew.Appearance = appearance39;
            this.btnNew.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnNew.Location = new System.Drawing.Point(40, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(60, 30);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "&New";
            this.btnNew.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnNew.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lblNew
            // 
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance40.ForeColor = System.Drawing.Color.White;
            appearance40.TextHAlignAsString = "Center";
            appearance40.TextVAlignAsString = "Middle";
            this.lblNew.Appearance = appearance40;
            this.lblNew.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblNew.Location = new System.Drawing.Point(0, 0);
            this.lblNew.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(40, 30);
            this.lblNew.TabIndex = 0;
            this.lblNew.Tag = "NOCOLOR";
            this.lblNew.Text = "F2";
            this.lblNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtTotalAmt);
            this.groupBox3.Controls.Add(this.lblHSubTotal);
            this.groupBox3.Controls.Add(this.grdHistory);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(905, 272);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pay Out History for this session";
            // 
            // txtTotalAmt
            // 
            appearance41.BackColor = System.Drawing.Color.Black;
            appearance41.BorderColor = System.Drawing.Color.Lime;
            appearance41.FontData.BoldAsString = "True";
            appearance41.ForeColor = System.Drawing.Color.White;
            appearance41.TextHAlignAsString = "Right";
            this.txtTotalAmt.Appearance = appearance41;
            this.txtTotalAmt.BackColorInternal = System.Drawing.Color.Black;
            this.txtTotalAmt.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTotalAmt.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTotalAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalAmt.Location = new System.Drawing.Point(786, 237);
            this.txtTotalAmt.Name = "txtTotalAmt";
            this.txtTotalAmt.Size = new System.Drawing.Size(107, 26);
            this.txtTotalAmt.TabIndex = 2;
            this.txtTotalAmt.Tag = "1";
            this.txtTotalAmt.Text = "0.00";
            // 
            // lblHSubTotal
            // 
            appearance42.BackColor = System.Drawing.Color.Black;
            appearance42.ForeColor = System.Drawing.Color.Lime;
            this.lblHSubTotal.Appearance = appearance42;
            this.lblHSubTotal.BackColorInternal = System.Drawing.Color.Black;
            this.lblHSubTotal.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHSubTotal.Location = new System.Drawing.Point(695, 241);
            this.lblHSubTotal.Name = "lblHSubTotal";
            this.lblHSubTotal.Size = new System.Drawing.Size(85, 17);
            this.lblHSubTotal.TabIndex = 1;
            this.lblHSubTotal.Tag = "";
            this.lblHSubTotal.Text = "Total Payout";
            // 
            // frmPayOut_Fill_Panel
            // 
            this.frmPayOut_Fill_Panel.Controls.Add(this.groupBox3);
            this.frmPayOut_Fill_Panel.Controls.Add(this.groupBox2);
            this.frmPayOut_Fill_Panel.Controls.Add(this.grbFields);
            this.frmPayOut_Fill_Panel.Controls.Add(this.lblTransactionType);
            this.frmPayOut_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.frmPayOut_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frmPayOut_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.frmPayOut_Fill_Panel.Name = "frmPayOut_Fill_Panel";
            this.frmPayOut_Fill_Panel.Size = new System.Drawing.Size(929, 503);
            this.frmPayOut_Fill_Panel.TabIndex = 0;
            this.frmPayOut_Fill_Panel.VisibleChanged += new System.EventHandler(this.frmPayOut_Fill_Panel_VisibleChanged);
            this.frmPayOut_Fill_Panel.Paint += new System.Windows.Forms.PaintEventHandler(this.frmPayOut_Fill_Panel_Paint);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "change password1.jpg");
            this.imageList1.Images.SetKeyName(2, "delete.jpg");
            // 
            // frmPayOut
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(929, 503);
            this.Controls.Add(this.frmPayOut_Fill_Panel);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmPayOut";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pay Out";
            this.Activated += new System.EventHandler(this.frmPayOut_Activated);
            this.Load += new System.EventHandler(this.frmPayOut_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPayOut_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.grbFields.ResumeLayout(false);
            this.grbFields.PerformLayout();
            this.pnlPayoutCategory.ClientArea.ResumeLayout(false);
            this.pnlPayoutCategory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPayoutCat)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            this.pnlSave.ClientArea.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.pnlNew.ClientArea.ResumeLayout(false);
            this.pnlNew.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.frmPayOut_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void frmPayOut_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Payout.ID, 0, UserPriviliges.Screens.Payout.Name))
                {
                    Clear();
                    Display();
                    SetNew();
                    clsUIHelper.SetReadonlyRow(this.grdHistory);
                    clsUIHelper.SetAppearance(this.grdHistory);

                    this.grdHistory.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                    this.grdHistory.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                    this.txtDescription.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                    this.txtDescription.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                    this.txtAmount.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                    this.txtAmount.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                    clsUIHelper.setColorSchecme(this);
                    btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                    btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
                    unloadMe = false;
                    SetDefaultDescription();
                    //cmbPayoutCat.Focus();//Shitaljit
                    this.btnPayoutCategory.Focus();
                }
                else
                {
                    unloadMe = true;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void SetDefaultDescription()
        {
            try
            {
                if (cmbPayoutCat.Value != null)
                {
                    CatID = Configuration.convertNullToInt(cmbPayoutCat.Value.ToString());
                    PayOutCatData ooPayOutCatData = new PayOutCatData();
                    PayOutCatRow oPayOutCatRow;
                    PayOutCat oPayOutCat = new PayOutCat();
                    ooPayOutCatData = oPayOutCat.Populate(CatID);
                    oPayOutCatRow = ooPayOutCatData.PayOutCat.GetRowByID(CatID.ToString());
                    this.txtDescription.Text = Configuration.convertNullToString(oPayOutCatRow.DefaultDescription.ToString());
                }
            }
            catch { }

        }
        private void Display()
        {
            PayOutData oGPayOutData;
            this.grdHistory.DisplayLayout.Bands[0].Columns.ClearUnbound();
            oGPayOutData = oPayOut.PopulateList(" and isstationclosed=0 order by payoutid desc");
            this.grdHistory.DataSource = oGPayOutData;
            Infragistics.Win.UltraWinGrid.UltraGridColumn oCol = null;

            oCol = this.grdHistory.DisplayLayout.Bands[0].Columns.Add("btnEdit");
            oCol.Header.Caption = "Edit";
            oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            oCol.CellAppearance.Image = this.imageList1.Images[1];
            oCol.CellButtonAppearance.Image = this.imageList1.Images[1];
            oCol.Width = 15;
            oCol.AutoEdit = false;
            oCol.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grdHistory.DisplayLayout.Bands[0].Columns["btnEdit"].Header.ToolTipText = "Contain button to edit record";

            oCol = this.grdHistory.DisplayLayout.Bands[0].Columns.Add("btnDelete");
            oCol.Header.Caption = "Delete";
            oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            oCol.CellAppearance.Image = this.imageList1.Images[2];
            oCol.CellButtonAppearance.Image = this.imageList1.Images[2];
            oCol.Width = 15;
            oCol.AutoEdit = false;
            oCol.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            this.grdHistory.DisplayLayout.Bands[0].Columns["btnDelete"].Header.ToolTipText = "Contain button to delete record";

            oCol = this.grdHistory.DisplayLayout.Bands[0].Columns.Add("btnPrint");
            oCol.Header.Caption = "Print";
            oCol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            oCol.CellAppearance.Image = this.imageList1.Images[0];
            oCol.CellButtonAppearance.Image = this.imageList1.Images[0];
            oCol.Width = 15;
            oCol.AutoEdit = false;
            oCol.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grdHistory.DisplayLayout.Bands[0].Columns["btnPrint"].Header.ToolTipText = "Contain button to print record";


            //Adshutosh Start 

            string Str = "Select * from " + clsPOSDBConstants.PayOutCat_tbl + "";
            oPayoutCatSvr = new PayOutCatSvr();
            PayOutCatData oPayOutCatData = new PayOutCatData();
            oPayOutCatData = oPayoutCatSvr.Populate(Str);
            this.cmbPayoutCat.Items.Clear();//Shitaljit
            foreach (PayOutCatRow oRow in oPayOutCatData.Tables[0].Rows)
            {
                this.cmbPayoutCat.Items.Add(oRow.ID.ToString(), oRow.PayoutCatType.ToString());
            }
            this.cmbPayoutCat.SelectedIndex = 0;

            this.ISUserAllow = false;
            //Shitaljit End

            decimal totalPayout = 0;
            foreach (PayOutRow oRow in oGPayOutData.PayOut.Rows)
            {
                totalPayout += oRow.Amount;
            }
            this.txtTotalAmt.Text = totalPayout.ToString(Configuration.CInfo.CurrencySymbol + "#####0.00");

            this.grdHistory.Refresh();

            ApplyGrigFormat();
        }

        private void ApplyGrigFormat()
        {
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_PayOutID].Hidden = true;
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_PayoutCatID].Hidden = true;//Added by Shitaljit
            //ugTax.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.fld_UserID].Hidden = true;
            //ugTax.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.fld_UserID].Hidden = true;
            clsUIHelper.SetAppearance(this.grdHistory);
            this.grdHistory.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_Amount].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //Added By Shitaljit on 18 April 2013 for PRIMEPOS 480  
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_Amount].Format = "########0.00";
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_PayoutCatType].Header.Caption = "Category";
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_TransDate].Header.Caption = "Date";
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_UserID].Header.Caption = "User";
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_StationID].Header.Caption = "Station";
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_DrawNo].Header.Caption = "Drawer";
            this.grdHistory.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayOut_Fld_TransDate].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grdHistory.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            resizeColumns();

        }

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdHistory.DisplayLayout.Bands)
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

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void txtAmount_Enter(object sender, System.EventArgs e)
        {
            try
            {
                this.txtAmount.SelectionStart = 0;
                this.txtAmount.SelectionLength = this.txtAmount.MaxValue.ToString().Length;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }
        private bool ChkeckPaoutPrevilage(int PaoutID, string UserID)
        {
            Util_UserOptionDetailRightsData oUtil_UserOptionDetailRightsData;
            Util_UserOptionDetailRightsSvr oUtil_UserOptionDetailRightsSvr = new Util_UserOptionDetailRightsSvr();
            oUtil_UserOptionDetailRightsData = oUtil_UserOptionDetailRightsSvr.Populate(" where " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId + " = " + CatID + " And (" + clsPOSDBConstants.PayOut_Fld_UserID + " = '" + UserID + "'" + " OR " + clsPOSDBConstants.PayOut_Fld_UserID + " = 'All')");
            if (oUtil_UserOptionDetailRightsData.Tables[0] != null && oUtil_UserOptionDetailRightsData.Tables[0].Rows.Count > 0)
            {
                PayOutCatData ooPayOutCatData = new PayOutCatData();
                PayOutCatRow oPayOutCatRow;
                PayOutCat oPayOutCat = new PayOutCat();
                ooPayOutCatData = oPayOutCat.Populate(CatID);
                oPayOutCatRow = ooPayOutCatData.PayOutCat.GetRowByID(CatID.ToString());
                if (bIsEdit == false && string.IsNullOrEmpty(oPayOutCatRow.DefaultDescription) == false)
                {
                    this.txtDescription.Text = Configuration.convertNullToString(oPayOutCatRow[clsPOSDBConstants.PayOutCat_Fld_DefaultDescription].ToString());
                }
                this.ISUserAllow = true;
                return true;
            }
            else
            {
                UserManagement.clsLogin oLogin = new POS_Core_UI.UserManagement.clsLogin();
                //oLogin.GetUsersRole(Configuration.UserName);

                string sUserID = "";
                if (oLogin.loginForPreviliges(clsPOSDBConstants.UserUsePayOutCatagory, "", out sUserID, "Security Override For Use Payout Catagory "))
                {
                    PayOutCatData ooPayOutCatData = new PayOutCatData();
                    PayOutCatRow oPayOutCatRow;
                    PayOutCat oPayOutCat = new PayOutCat();
                    ooPayOutCatData = oPayOutCat.Populate(CatID);
                    oPayOutCatRow = ooPayOutCatData.PayOutCat.GetRowByID(CatID.ToString());
                    if (bIsEdit == false)
                    {
                        this.txtDescription.Text = Configuration.convertNullToString(oPayOutCatRow[clsPOSDBConstants.PayOutCat_Fld_DefaultDescription].ToString());
                    }
                    this.ISUserAllow = true;
                    return true;
                }
                else
                {
                    this.ISUserAllow = false;
                    return false;
                }
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {//ChkeckPaoutPrevilage
                intPayOutId = CatID;
                if (!this.ISUserAllow)
                {
                    if (CatID < 1)
                    {
                        PayOutCatData oPayOutCatData = new PayOutCatData();
                        oPayOutCatData = oPayoutCatSvr.PopulateList(" Where " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + "='" + cmbPayoutCat.Text.Trim() + "'");
                        if (oPayOutCatData.Tables[0].Rows.Count > 0)
                        {
                            CatID = Configuration.convertNullToInt(oPayOutCatData.Tables[0].Rows[0][clsPOSDBConstants.payoutCat_Fld_Id].ToString());
                        }
                    }

                    if (!ChkeckPaoutPrevilage(CatID, Configuration.UserName))
                    {
                        //POS  clsUIHelper.ShowErrorMsg( "User Not Have Previlage to use this Pay out catagory");
                        return;
                    }
                }
                //PayOutRow oRow;

                oPayOutRow.Description = this.txtDescription.Text;

                oPayOutRow.Amount = Configuration.convertNullToDecimal(this.txtAmount.Value.ToString());
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Payout, "Save()", clsPOSDBConstants.Log_Entering + "Amount :" + oPayOutRow.Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00"));
                if (oPayOutRow.Amount > 999)
                {
                    if (Resources.Message.Display("Amount is great than " + Configuration.CInfo.CurrencySymbol + "999.\nDo you want to continue?", "Payout", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        txtAmount.Focus();
                        return;
                    }
                }

                oPayOutRow.TransDate = System.DateTime.Now;
                oPayOutRow.StationID = Configuration.StationName;
                oPayOutRow.DrawNo = Configuration.DrawNo;
                oPayOutRow.UserID = Configuration.UserName;
                oPayOutRow.PayoutCatType = this.cmbPayoutCat.Text.ToString();//Added by Shitaljit
                CatID = Configuration.convertNullToInt(cmbPayoutCat.Value.ToString());//added by shitaljit.
                oPayOutRow.PayoutCatID = CatID;
                oPayOut.Persist(oPayOutData);
                RxLabel oRX = new RxLabel(oPayOutRow);
                oRX.OpenDrawer();
                oRX.PrintLabelPayout();
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Payout, "Save()", clsPOSDBConstants.Log_Exiting + "Amount :" + oPayOutRow.Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00"));
                this.ISUserAllow = false;
                SetNew();
                Display();

            }
            catch (POSExceptions exp)
            {
                this.ISUserAllow = false;
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.PayOut_DescriptionCanNotBeNULL:
                        this.txtDescription.Focus();
                        break;
                    case (long)POSErrorENUM.PayOut_AmountCanNotBeNull:
                        this.txtAmount.Focus();
                        break;
                }
            }

            catch (Exception exp)
            {
                this.ISUserAllow = false;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Clear()
        {
            this.ISUserAllow = false;
            this.txtDescription.Text = String.Empty;
            this.txtAmount.Value = 0;

        }

        private void SetNew()
        {
            oPayOut = new PayOut();
            oPayOutData = new PayOutData();
            bIsEdit = false;
            Clear();
            oPayOutRow = oPayOutData.PayOut.AddRow(0, "", 0);
            cmbPayoutCat.SelectedIndex = 0;
            PayOutCatData ooPayOutCatData = new PayOutCatData();
            PayOutCatRow oPayOutCatRow;
            PayOutCat oPayOutCat = new PayOutCat();
            CatID = Configuration.convertNullToInt(cmbPayoutCat.Value.ToString());
            ooPayOutCatData = oPayOutCat.Populate(CatID);
            oPayOutCatRow = ooPayOutCatData.PayOutCat.GetRowByID(CatID.ToString());
            this.txtDescription.Text = Configuration.convertNullToString(oPayOutCatRow.DefaultDescription.ToString());
            this.grbFields.Enabled = true;
            //this.txtDescription.Focus();Shitaljit
        }

        private void frmPayOut_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F2)
                {
                    SetNew();
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    SearchPayoutCat();
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmPayOut_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void frmPayOut_Fill_Panel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void frmPayOut_Fill_Panel_VisibleChanged(object sender, System.EventArgs e)
        {
            if (this.unloadMe)
            {
                this.Close();

            }
        }

        private void grdHistory_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Key == "btnPrint")
            {
                PayOutRow oRow = oPayOutData.PayOut.NewclsPayOutRow();
                oRow.Amount = Configuration.convertNullToDecimal(this.grdHistory.ActiveRow.Cells["Amount"].Text.ToString());
                oRow.Description = this.grdHistory.ActiveRow.Cells["Description"].Text.ToString();
                oRow.DrawNo = Configuration.convertNullToInt(this.grdHistory.ActiveRow.Cells["drawno"].Text.ToString());
                oRow.StationID = this.grdHistory.ActiveRow.Cells["stationid"].Text.ToString();
                oRow.UserID = this.grdHistory.ActiveRow.Cells["userid"].Text.ToString();
                oRow.TransDate = Convert.ToDateTime(this.grdHistory.ActiveRow.Cells["transdate"].Text.ToString());

                RxLabel oRX = new RxLabel(oRow);
                oRX.OpenDrawer();
                oRX.PrintLabelPayout();
                SetNew();
            }
            else if (e.Cell.Column.Key == "btnEdit")
            {
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Payout.ID, UserPriviliges.Permissions.EditPayout.ID))
                {
                    oPayOutData = oPayOut.Populate(e.Cell.Row.Cells[clsPOSDBConstants.PayOut_Fld_PayOutID].Text);
                    if (oPayOutData.PayOut.Rows.Count > 0)
                    {
                        oPayOutRow = oPayOutData.PayOut[0];
                        PopulateData();
                        ValidateUserRights();
                        bIsEdit = true;
                    }
                }
                else
                    clsUIHelper.ShowErrorMsg("You do not have permision to Edit Payout.");
            }
            else if (e.Cell.Column.Key == "btnDelete")
            {
                try
                {
                    if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Payout.ID, UserPriviliges.Permissions.DeletePayout.ID))
                    {
                        oPayOutData = oPayOut.Populate(e.Cell.Row.Cells[clsPOSDBConstants.PayOut_Fld_PayOutID].Text);
                        if (oPayOutData.PayOut.Rows.Count > 0)
                        {
                            if (Resources.Message.Display("This action will delete selected record. Are your sure?", "Payout", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                oPayOutData.PayOut[0].Delete();
                                oPayOut.Persist(oPayOutData);
                                Display();
                                SetNew();
                            }
                        }
                    }
                    else
                        clsUIHelper.ShowErrorMsg("You do not have permision to Delete Payout.");
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
            }

        }

        private void PopulateData()
        {
            this.cmbPayoutCat.Value = oPayOutRow.PayoutCatID;
            this.txtAmount.Value = oPayOutRow.Amount;
            this.txtDescription.Text = oPayOutRow.Description;
            this.cmbPayoutCat.Focus();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SetNew();
            this.grbFields.Enabled = true;
        }

        private void cmbPayoutCat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ValidateUserRights();
        }

        private void ValidateUserRights()
        {
            if (cmbPayoutCat.Value != null)
            {
                try
                {

                    CatID = Configuration.convertNullToInt(cmbPayoutCat.Value.ToString());
                    intPayOutId = CatID;
                    Util_UserOptionDetailRightsData oUtil_UserOptionDetailRightsData;

                    Util_UserOptionDetailRightsSvr oUtil_UserOptionDetailRightsSvr = new Util_UserOptionDetailRightsSvr();

                    oUtil_UserOptionDetailRightsData = oUtil_UserOptionDetailRightsSvr.Populate(" where " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId + " = " + CatID + " And (" + clsPOSDBConstants.PayOut_Fld_UserID + " = '" + Configuration.UserName + "'" + " OR " + clsPOSDBConstants.PayOut_Fld_UserID + " = 'All')");
                    if (oUtil_UserOptionDetailRightsData.Tables[0] != null && oUtil_UserOptionDetailRightsData.Tables[0].Rows.Count > 0)
                    {
                        PayOutCatData ooPayOutCatData = new PayOutCatData();
                        PayOutCatRow oPayOutCatRow;
                        PayOutCat oPayOutCat = new PayOutCat();
                        ooPayOutCatData = oPayOutCat.Populate(CatID);
                        oPayOutCatRow = ooPayOutCatData.PayOutCat.GetRowByID(CatID.ToString());
                        if (bIsEdit == false)
                        {
                            this.txtDescription.Text = Configuration.convertNullToString(oPayOutCatRow.DefaultDescription.ToString());
                        }
                        if (this.txtDescription.Text.Trim() != Configuration.convertNullToString(null))
                            this.txtAmount.Focus();
                        this.ISUserAllow = true;
                    }
                    else
                    {
                        UserManagement.clsLogin oLogin = new POS_Core_UI.UserManagement.clsLogin();

                        string sUserID = "";
                        if (oLogin.loginForPreviliges(clsPOSDBConstants.UserUsePayOutCatagory, "", out sUserID, " Security Override For Use Payout Catagory "))
                        {
                            PayOutCatData ooPayOutCatData = new PayOutCatData();
                            PayOutCatRow oPayOutCatRow;
                            PayOutCat oPayOutCat = new PayOutCat();
                            ooPayOutCatData = oPayOutCat.Populate(CatID);
                            oPayOutCatRow = ooPayOutCatData.PayOutCat.GetRowByID(CatID.ToString());
                            if (bIsEdit == false)
                            {
                                this.txtDescription.Text = Configuration.convertNullToString(oPayOutCatRow.DefaultDescription.ToString());
                            }
                            this.ISUserAllow = true;
                            if (this.txtDescription.Text.Trim() != Configuration.convertNullToString(null))
                                this.txtAmount.Focus();
                        }
                        else
                        {
                            this.cmbPayoutCat.Focus();
                            this.ISUserAllow = false;
                        }
                    }
                }
                catch { }

            }
        }

        private void grdHistory_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                e.Row.Cells["btnEdit"].ToolTipText = "Click to edit current row";
                e.Row.Cells["btnDElete"].ToolTipText = "Click to delete current row";
                e.Row.Cells["btnPrint"].ToolTipText = "Click to print current row";
            }
            catch (Exception Ex)
            {

            }
        }

        private void cmbPayoutCat_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbPayoutCat_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                PayOutCatData ooPayOutCatData = new PayOutCatData();
                PayOutCatRow oPayOutCatRow;
                PayOutCat oPayOutCat = new PayOutCat();
                intPayOutId = CatID;
                ooPayOutCatData = oPayOutCat.Populate(CatID);
                if (ooPayOutCatData.Tables[0].Rows.Count > 0)
                {
                    oPayOutCatRow = ooPayOutCatData.PayOutCat.GetRowByID(CatID.ToString());
                    this.txtDescription.Text = oPayOutCatRow.DefaultDescription.ToString();
                }
            }
            catch { }
        }
        private void SearchPayoutCat()
        {
            try
            {
                frmPaytoutCatFuncKeys ofrmPayOutCatKey = new frmPaytoutCatFuncKeys();
                if (ofrmPayOutCatKey.ShowDialog() == DialogResult.OK)
                {
                    cmbPayoutCat.Value = ofrmPayOutCatKey.SelectedKey;
                    ValidateUserRights();
                }
            }
            catch { }
        }
        private void btnPayoutCategory_Click(object sender, EventArgs e)
        {
            SearchPayoutCat();
        }

        private void cmbPayoutCat_Leave(object sender, EventArgs e)
        {

            if (this.ActiveControl != null && this.ActiveControl.Name == this.btnSave.Name)
            {
                ValidateUserRights();
            }
        }

        private void lblTransactionType_Click(object sender, EventArgs e)
        {

        }
    }
}
