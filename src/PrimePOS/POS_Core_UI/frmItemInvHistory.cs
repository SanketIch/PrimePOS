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
using POS_Core_UI.Reports.Reports;
using POS_Core_UI.Reports.ReportsUI;
//using POS_Core_UI.Reports.ReportsUI;
namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPhysicalInvView.
	/// </summary>
	public class frmItemInvHistory : System.Windows.Forms.Form
	{
		public String CurrentItem;
        public string CurrentItemDesc;
		private PhysicalInvData oPhysicalInvData = new PhysicalInvData();
		private PhysicalInv oPhysicalInv = new PhysicalInv();
        private System.Data.DataSet oDS = null;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdHistory;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
        private GroupBox groupBox1;
        private RadioButton optReturn;
        private RadioButton optSales;
        private RadioButton optAll;
        private RadioButton optInvRecieved;
        private RadioButton optPhaysicalInv;
        private GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private GroupBox gbInventoryReceived;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private IContainer components;


		public frmItemInvHistory(String ItemID, string sDescription)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			CurrentItem=ItemID;
            CurrentItemDesc = sDescription;
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemCode");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OldQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NewQty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("isProcessed");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TransDate");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PUserID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PTransDate");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemInvHistory));
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
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Reference");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            this.grdHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optPhaysicalInv = new System.Windows.Forms.RadioButton();
            this.optInvRecieved = new System.Windows.Forms.RadioButton();
            this.optReturn = new System.Windows.Forms.RadioButton();
            this.optSales = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            this.SuspendLayout();
            // 
            // grdHistory
            // 
            this.grdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackColorDisabled = System.Drawing.Color.White;
            appearance1.BackColorDisabled2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdHistory.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(34, 0);
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(159, 0);
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(108, 0);
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.Header.VisiblePosition = 9;
            ultraGridColumn10.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10});
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdHistory.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistory.DisplayLayout.InterBandSpacing = 10;
            this.grdHistory.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHistory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.AddRowAppearance = appearance4;
            this.grdHistory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdHistory.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdHistory.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdHistory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.grdHistory.DisplayLayout.Override.CardAreaAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackColorDisabled = System.Drawing.Color.White;
            appearance6.BackColorDisabled2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Black;
            appearance6.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdHistory.DisplayLayout.Override.CellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Gray;
            appearance7.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            appearance7.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdHistory.DisplayLayout.Override.CellButtonAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdHistory.DisplayLayout.Override.EditCellAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredInRowAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredOutRowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BackColorDisabled = System.Drawing.Color.White;
            appearance11.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.FixedCellAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance12.BackColor2 = System.Drawing.Color.Beige;
            this.grdHistory.DisplayLayout.Override.FixedHeaderAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.FontData.BoldAsString = "True";
            appearance13.FontData.SizeInPoints = 10F;
            appearance13.ForeColor = System.Drawing.Color.White;
            appearance13.TextHAlignAsString = "Left";
            appearance13.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdHistory.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.grdHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdHistory.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAlternateAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance15.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowPreviewAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.grdHistory.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdHistory.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdHistory.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.Navy;
            appearance18.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdHistory.DisplayLayout.Override.SelectedCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.Navy;
            appearance19.BackColorDisabled = System.Drawing.Color.Navy;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance19.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            appearance19.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.SelectedRowAppearance = appearance19;
            this.grdHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.TemplateAddRowAppearance = appearance20;
            this.grdHistory.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdHistory.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance21.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            appearance21.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            scrollBarLook1.Appearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.LightGray;
            appearance22.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            scrollBarLook1.ButtonAppearance = appearance22;
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook1.ThumbAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.Gainsboro;
            appearance24.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance24.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance24.BorderAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance24.BorderColor = System.Drawing.Color.White;
            appearance24.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            appearance24.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance24;
            this.grdHistory.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdHistory.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdHistory.Location = new System.Drawing.Point(14, 17);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(664, 298);
            this.grdHistory.TabIndex = 5;
            this.grdHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance25.FontData.BoldAsString = "True";
            appearance25.ForeColor = System.Drawing.Color.Black;
            appearance25.Image = ((object)(resources.GetObject("appearance25.Image")));
            this.btnClose.Appearance = appearance25;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(581, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTransactionType
            // 
            appearance26.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance26.BackColor2 = System.Drawing.Color.Azure;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance26.ForeColor = System.Drawing.Color.Navy;
            appearance26.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance26.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance26;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(724, 35);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Tag = "CUSTOM";
            this.lblTransactionType.Text = "Inventory History View";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(22, 544);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(690, 50);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance27.FontData.BoldAsString = "True";
            appearance27.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Appearance = appearance27;
            this.btnPrint.Location = new System.Drawing.Point(467, 14);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 30);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "&Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.grdHistory);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(22, 211);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(690, 327);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.optPhaysicalInv);
            this.groupBox1.Controls.Add(this.optInvRecieved);
            this.groupBox1.Controls.Add(this.optReturn);
            this.groupBox1.Controls.Add(this.optSales);
            this.groupBox1.Controls.Add(this.optAll);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(21, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(690, 50);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // optPhaysicalInv
            // 
            this.optPhaysicalInv.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optPhaysicalInv.ForeColor = System.Drawing.Color.Black;
            this.optPhaysicalInv.Location = new System.Drawing.Point(509, 17);
            this.optPhaysicalInv.Name = "optPhaysicalInv";
            this.optPhaysicalInv.Size = new System.Drawing.Size(155, 24);
            this.optPhaysicalInv.TabIndex = 12;
            this.optPhaysicalInv.Text = "Physical Inv.";
            this.optPhaysicalInv.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // optInvRecieved
            // 
            this.optInvRecieved.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optInvRecieved.ForeColor = System.Drawing.Color.Black;
            this.optInvRecieved.Location = new System.Drawing.Point(380, 17);
            this.optInvRecieved.Name = "optInvRecieved";
            this.optInvRecieved.Size = new System.Drawing.Size(123, 24);
            this.optInvRecieved.TabIndex = 11;
            this.optInvRecieved.Text = "Inv. Received";
            this.optInvRecieved.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // optReturn
            // 
            this.optReturn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optReturn.ForeColor = System.Drawing.Color.Black;
            this.optReturn.Location = new System.Drawing.Point(270, 17);
            this.optReturn.Name = "optReturn";
            this.optReturn.Size = new System.Drawing.Size(104, 24);
            this.optReturn.TabIndex = 10;
            this.optReturn.Text = "Return";
            this.optReturn.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // optSales
            // 
            this.optSales.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optSales.ForeColor = System.Drawing.Color.Black;
            this.optSales.Location = new System.Drawing.Point(152, 17);
            this.optSales.Name = "optSales";
            this.optSales.Size = new System.Drawing.Size(104, 24);
            this.optSales.TabIndex = 9;
            this.optSales.Text = "Sales";
            this.optSales.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // optAll
            // 
            this.optAll.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optAll.ForeColor = System.Drawing.Color.Black;
            this.optAll.Location = new System.Drawing.Point(42, 17);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(104, 24);
            this.optAll.TabIndex = 8;
            this.optAll.Text = "All";
            this.optAll.CheckedChanged += new System.EventHandler(this.OptionButton_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtItemCode);
            this.groupBox4.Controls.Add(this.txtDescription);
            this.groupBox4.Controls.Add(this.ultraLabel2);
            this.groupBox4.Controls.Add(this.ultraLabel1);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(21, 52);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(690, 44);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            // 
            // txtItemCode
            // 
            appearance28.FontData.BoldAsString = "False";
            appearance28.FontData.ItalicAsString = "False";
            appearance28.FontData.StrikeoutAsString = "False";
            appearance28.FontData.UnderlineAsString = "False";
            appearance28.ForeColor = System.Drawing.Color.Black;
            appearance28.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtItemCode.Appearance = appearance28;
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtItemCode.Location = new System.Drawing.Point(91, 14);
            this.txtItemCode.MaxLength = 20;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(132, 23);
            this.txtItemCode.TabIndex = 2;
            this.txtItemCode.TabStop = false;
            // 
            // txtDescription
            // 
            appearance29.FontData.BoldAsString = "False";
            appearance29.FontData.ItalicAsString = "False";
            appearance29.FontData.StrikeoutAsString = "False";
            appearance29.FontData.UnderlineAsString = "False";
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtDescription.Appearance = appearance29;
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(317, 14);
            this.txtDescription.MaxLength = 100;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(361, 23);
            this.txtDescription.TabIndex = 4;
            this.txtDescription.TabStop = false;
            // 
            // ultraLabel2
            // 
            appearance30.FontData.BoldAsString = "False";
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance30;
            this.ultraLabel2.Location = new System.Drawing.Point(237, 16);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(85, 19);
            this.ultraLabel2.TabIndex = 3;
            this.ultraLabel2.Text = "Description";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance31.FontData.BoldAsString = "False";
            appearance31.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance31;
            this.ultraLabel1.Location = new System.Drawing.Point(12, 16);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(80, 19);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "Item Code";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.btnView);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.ForeColor = System.Drawing.Color.White;
            this.gbInventoryReceived.Location = new System.Drawing.Point(22, 102);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(689, 49);
            this.gbInventoryReceived.TabIndex = 32;
            this.gbInventoryReceived.TabStop = false;
            // 
            // btnView
            // 
            appearance32.FontData.BoldAsString = "True";
            appearance32.ForeColor = System.Drawing.Color.Black;
            this.btnView.Appearance = appearance32;
            this.btnView.Location = new System.Drawing.Point(567, 14);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(111, 26);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "&View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance33.FontData.BoldAsString = "False";
            appearance33.FontData.ItalicAsString = "False";
            appearance33.FontData.StrikeoutAsString = "False";
            appearance33.FontData.UnderlineAsString = "False";
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance33;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(317, 17);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(123, 21);
            this.dtpToDate.TabIndex = 2;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance34.FontData.BoldAsString = "False";
            appearance34.FontData.ItalicAsString = "False";
            appearance34.FontData.StrikeoutAsString = "False";
            appearance34.FontData.UnderlineAsString = "False";
            appearance34.ForeColor = System.Drawing.Color.Black;
            appearance34.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance34;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(91, 17);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(123, 21);
            this.dtpFromDate.TabIndex = 1;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel13
            // 
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel13.Appearance = appearance35;
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(237, 20);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(72, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel14.Appearance = appearance36;
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(9, 20);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(72, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "From Date";
            // 
            // frmItemInvHistory
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(724, 606);
            this.Controls.Add(this.gbInventoryReceived);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmItemInvHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "";
            this.Text = "Inventory History View";
            this.Activated += new System.EventHandler(this.frmPhysicalInvView_Activated);
            this.Load += new System.EventHandler(this.frmPhysicalInvView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPhysicalInvView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmPhysicalInvView_Load(object sender, System.EventArgs e)
		{
			try
			{
                this.optAll.Checked = true;
                this.dtpToDate.Value = DateTime.Now;
                this.dtpFromDate.Value = DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0, 0));
                Display();
                this.txtItemCode.Text = CurrentItem;
                this.txtDescription.Text = CurrentItemDesc;
				clsUIHelper.SetReadonlyRow(this.grdHistory);
				clsUIHelper.SetAppearance(this.grdHistory);
				clsUIHelper.setColorSchecme(this);
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}
		private void Display() 
		{

            string strSQL="";
            string strSales="";
            string strReturns="";
            string strInvRecv="";
            string strPhyInv="";

            if (optAll.Checked==true || optPhaysicalInv.Checked==true)
            {
                strPhyInv = " Select ID as TransID,'Physical Inventory' as TransType , NewQty as Qty, PTransDate as TransDate FROM PhysicalInv, Item WHERE ItemID = ItemCode and itemcode='" + CurrentItem.Replace("'", "''") + "' and isprocessed=1 ";
            }
            if (optAll.Checked==true || optSales.Checked==true)
            {
                strSales = "Select PT.TransID as TransID, Case PT.TransType When 1  Then 'Sales' Else 'Return' End as TransType, Qty as Qty, TransDate FROM POSTransactionDetail PTD, POSTransaction PT Where PT.TransID=PTD.TransID and PTD.ItemID ='" + CurrentItem.Replace("'", "''") + "' and PT.TransType=1";
            }
            if (optAll.Checked==true || optReturn.Checked==true)
            {
                strReturns = "Select PT.TransID as TransID, Case PT.TransType When 1  Then 'Sales' Else 'Return' End as TransType, Qty as Qty, TransDate FROM POSTransactionDetail PTD, POSTransaction PT Where PT.TransID=PTD.TransID and PTD.ItemID ='" + CurrentItem.Replace("'", "''") + "' and PT.TransType=2";
            }

            if (optAll.Checked==true || optInvRecieved.Checked==true)
            {
                //Sprint-22 - PRIMEPOS-2250 01-Dec-2015 JY Commented the incorrect query which alway prints "Inventory Received" even of the the record is for other type like Inventory Returned
                //strInvRecv = "Select IR.InvRecievedID as TransID,'Invenotry Recieved' as TransType,Qty , IR.RecieveDate TransDate  from InventoryRecieved IR, InvRecievedDetail IRD where IR.InvRecievedID=IRD.InvRecievedID and IRD.ItemID ='" + CurrentItem.Replace("'", "''") + "'";
                strInvRecv = " SELECT IR.InvRecievedID as TransID, ITT.TypeName as TransType, Qty, IR.RecieveDate TransDate FROM InventoryRecieved IR " +
                            " INNER JOIN InvRecievedDetail IRD  ON IR.InvRecievedID=IRD.InvRecievedID " + 
                            " INNER JOIN InvTransType ITT ON ITT.ID = IR.InvTransTypeID " +
                            " WHERE IRD.ItemID ='" + CurrentItem.Replace("'", "''") + "'";
            }

            strSQL=strPhyInv;

            if(strSales.Trim().Length>0)
            {
            if (strSQL.Trim().Length>0)
            {
                strSQL+=" Union All ";
            }
                strSQL+=strSales;
            }

            if(strReturns.Trim().Length>0)
            {
            if (strSQL.Trim().Length>0)
            {
                strSQL+=" Union All ";
            }
                strSQL+=strReturns;
            }

            if(strInvRecv.Trim().Length>0)
            {
            if (strSQL.Trim().Length>0)
            {
                strSQL+=" Union All ";
            }
                strSQL+=strInvRecv;
            }

            strSQL = " Select * From ( " + strSQL + " ) as TempTable ";

            strSQL += " where TransDate between Cast('" + this.dtpFromDate.Value.ToString() + "' as datetime)";
            strSQL += " and Cast('" + this.dtpToDate.Value.ToString() + "' as datetime)";
            strSQL += " Order by TransDate Desc ";

            Search oSearch=new Search();
            oDS=oSearch.SearchData(strSQL);
            this.grdHistory.DataSource = oDS;
			this.grdHistory.Refresh();
			ApplyGrigFormat();
		}

		private void ApplyGrigFormat()
		{
			clsUIHelper.SetAppearance(this.grdHistory);
			this.grdHistory.DisplayLayout.Override.RowSelectors=DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Bands[0].Columns["TransDate"].Header.Caption = "Trans Date";
            grdHistory.DisplayLayout.Bands[0].Columns["TransDate"].Format = "MM/dd/yyyy hh:mm tt";
            resizeColumns();
		}

        private void resizeColumns()
        {
            foreach (UltraGridColumn oCol in grdHistory.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }


                //oDataSet.Tables[0].Columns[oCol.Key].DataType.
            }
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmPhysicalInvView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void frmPhysicalInvView_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
        }

        private void OptionButton_CheckedChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Display();
        }

        /// <summary>
        /// This will print the Inv History Of Item.
        /// </summary>
        /// <param name="printIt"></param>
        private void Print(bool printIt)
        {
            try
            {
                rptItemInventoryHistory oRpt = new rptItemInventoryHistory();
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtstartdate"]).Text = this.dtpFromDate.Value.ToString();
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtEnddate"]).Text = this.dtpToDate.Value.ToString();
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtItemDesc"]).Text = this.txtItemCode.Text.ToString()+" ("+ this.txtDescription.Text.ToString()+")";
                oRpt.SetDataSource(oDS.Tables[0]);
                clsReports.Preview(printIt,oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print(true);
        }

	}
}
