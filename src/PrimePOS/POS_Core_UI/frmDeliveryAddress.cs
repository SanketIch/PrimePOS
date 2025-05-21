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
using System.Data;
using Infragistics.Win.UltraWinGrid;
using NLog;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmDeliveryAddress.
	/// </summary>
	public class frmDeliveryAddress : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdHistory;
		private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private IContainer components;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private DataSet oDeliveryData;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmDeliveryAddress(DataSet deliveryData)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            oDeliveryData = deliveryData;

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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Source", 0);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDeliveryAddress));
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
            this.grdHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.grdHistory.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn1.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(182, 0);
            ultraGridColumn1.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn1.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(440, 0);
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 5;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.RowLayoutColumnInfo.OriginX = 7;
            ultraGridColumn3.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn3.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn3.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
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
            this.grdHistory.Location = new System.Drawing.Point(14, 24);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(740, 312);
            this.grdHistory.TabIndex = 5;
            this.grdHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdHistory_InitializeRow);
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance25.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance25;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(22, 15);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(766, 38);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Delivery Address";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnOk);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(22, 408);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(766, 50);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance26.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance26.FontData.BoldAsString = "True";
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
            this.btnClose.Appearance = appearance26;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(664, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 26);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // btnOk
            // 
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance27.FontData.BoldAsString = "True";
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            this.btnOk.Appearance = appearance27;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(567, 16);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 28);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.grdHistory);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(22, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(766, 348);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance28.TextHAlignAsString = "Left";
            this.ultraLabel1.Appearance = appearance28;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(22, 464);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(766, 23);
            this.ultraLabel1.TabIndex = 30;
            this.ultraLabel1.Tag = "Header";
            this.ultraLabel1.Text = "**Source, P = Patient, D= Delivery, H= House Charge ,F= Facility, C = Customer"; //PRIMEPOS-3494 corrected spell
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // frmDeliveryAddress
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(800, 492);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeliveryAddress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delivery Address";
            this.Activated += new System.EventHandler(this.frmDeliveryAddress_Activated);
            this.Load += new System.EventHandler(this.frmDeliveryAddress_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDeliveryAddress_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmDeliveryAddress_Load(object sender, System.EventArgs e)
		{
			try
			{
				clsUIHelper.SetReadonlyRow(this.grdHistory);
				clsUIHelper.SetAppearance(this.grdHistory);
				clsUIHelper.setColorSchecme(this);
                Display();
			}
			catch(Exception exp)
			{
                logger.Fatal(exp, "frmDeliveryAddress_Load(object sender, System.EventArgs e)");
                clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}
		private void Display() 
		{
			this.grdHistory.DataSource = oDeliveryData;
			this.grdHistory.Refresh();
            if (grdHistory.Rows.Count > 0)
            {   grdHistory.ActiveRow = grdHistory.Rows[0];
                grdHistory.Selected.Rows.Clear();
                grdHistory.Selected.Rows.Add(grdHistory.ActiveRow);
            }
            resizeColumns();
			ApplyGrigFormat();
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
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "resizeColumns()");
            }
        }

		private void ApplyGrigFormat()
		{
            grdHistory.DisplayLayout.Override.MinRowHeight = 30;
            this.grdHistory.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            this.grdHistory.DisplayLayout.Bands[0].Columns["Address"].Width = 500;
			clsUIHelper.SetAppearance(this.grdHistory);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmDeliveryAddress_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
                logger.Fatal(exp, "frmDeliveryAddress_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmDeliveryAddress_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        public String GetAddress()
        {
            if (grdHistory.ActiveRow!=null)
            {
                return grdHistory.ActiveRow.Cells["Address"].Value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (grdHistory.Selected.Rows.Count == 0)
            {
                clsUIHelper.ShowErrorMsg("Please select an address.");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdHistory_InitializeRow(object sender, InitializeRowEventArgs e)
        {
        }
	}
}
