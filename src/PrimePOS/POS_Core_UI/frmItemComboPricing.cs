using System;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
using POS_Core.Resources;
//using POS_Core.DataAccess;
namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for InventoryRecieved.
	/// </summary>
	public class frmItemComboPricing : System.Windows.Forms.Form
	{

		private ItemComboPricingData oItemComboPricingHData ;
		private ItemComboPricingRow oItemComboPricingHRow;
		private ItemComboPricing oItemComboPricing= new  ItemComboPricing();
		private ItemComboPricingDetailData oItemComboPricingDData ;
        private bool m_exceptionAccoured = false;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnNew;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolTip toolTip1;
		public  Infragistics.Win.Misc.UltraButton btnAddItem;
        public Infragistics.Win.Misc.UltraButton btnDeleteItem;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private System.ComponentModel.IContainer components;
        private POHeaderData poHeadData = null;
        private PODetailData poDetailData = null;
        private Infragistics.Win.Misc.UltraLabel ultraLblNoOfItems;
        private String inventoryWay = String.Empty;
        private Infragistics.Win.Misc.UltraLabel txtEditorNoOfItems;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkForceComboPricing;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsActive;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtMinComboItems;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtMaxComboItems;
        private Infragistics.Win.Misc.UltraLabel lblMaxComboItems;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtComboItemPrice;

		public frmItemComboPricing()
		{
			InitializeComponent();
		}

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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("id");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemComboPricingId");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemID");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemComboPricing));
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Qty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SalePrice");
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
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
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
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnNew = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMaxComboItems = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblMaxComboItems = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsActive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtMinComboItems = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.chkForceComboPricing = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtComboItemPrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtEditorNoOfItems = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLblNoOfItems = new Infragistics.Win.Misc.UltraLabel();
            this.btnDeleteItem = new Infragistics.Win.Misc.UltraButton();
            this.btnAddItem = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxComboItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinComboItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkForceComboPricing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComboItemPrice)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.AutoSize = false;
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(173, 20);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(375, 20);
            this.txtDescription.TabIndex = 0;
            // 
            // ultraLabel12
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance1;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(10, 21);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(80, 18);
            this.ultraLabel12.TabIndex = 1;
            this.ultraLabel12.Text = "Description";
            this.ultraLabel12.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grdDetail
            // 
            this.grdDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            appearance2.BackColorDisabled = System.Drawing.Color.White;
            appearance2.BackColorDisabled2 = System.Drawing.Color.White;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance2;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            ultraGridColumn3.CellButtonAppearance = appearance3;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn3.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(135, 0);
            ultraGridColumn3.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn3.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn3.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(176, 0);
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Width = 123;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn5.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn5.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(78, 0);
            ultraGridColumn5.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn5.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn6.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(103, 0);
            ultraGridColumn6.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn6.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance6;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BackColorDisabled = System.Drawing.Color.White;
            appearance8.BackColorDisabled2 = System.Drawing.Color.White;
            appearance8.BorderColor = System.Drawing.Color.Black;
            appearance8.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            appearance9.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            appearance9.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BackColorDisabled = System.Drawing.Color.White;
            appearance13.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance14.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.FontData.BoldAsString = "True";
            appearance15.FontData.SizeInPoints = 10F;
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.White;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance17.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            appearance19.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance19;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance20.BackColor = System.Drawing.Color.Navy;
            appearance20.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.Navy;
            appearance21.BackColorDisabled = System.Drawing.Color.Navy;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance21.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            appearance21.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance22;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance23.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance23;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(6, 23);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(543, 271);
            this.grdDetail.TabIndex = 0;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.AfterRowsDeleted += new System.EventHandler(this.grdDetail_AfterRowsDeleted);
            this.grdDetail.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdDetail_BeforeRowUpdate);
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdDetail_BeforeCellDeactivate);
            this.grdDetail.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.ValidateRow);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            this.grdDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItemComboPricing_KeyDown);
            this.grdDetail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmItemComboPricing_KeyUp);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance24.FontData.BoldAsString = "True";
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.Image = ((object)(resources.GetObject("appearance24.Image")));
            this.btnNew.Appearance = appearance24;
            this.btnNew.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnNew.Location = new System.Drawing.Point(258, 19);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(90, 26);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "C&lear";
            this.btnNew.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance25.FontData.BoldAsString = "True";
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.Image = ((object)(resources.GetObject("appearance25.Image")));
            this.btnClose.Appearance = appearance25;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(450, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 26);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance26.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance26.FontData.BoldAsString = "True";
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
            this.btnSave.Appearance = appearance26;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(354, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance27.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance27;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(18, 8);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(554, 40);
            this.lblTransactionType.TabIndex = 21;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Item Combo Pricing";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtMaxComboItems);
            this.groupBox1.Controls.Add(this.lblMaxComboItems);
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.txtMinComboItems);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.chkForceComboPricing);
            this.groupBox1.Controls.Add(this.txtComboItemPrice);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(562, 124);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtMaxComboItems
            // 
            appearance28.FontData.BoldAsString = "False";
            this.txtMaxComboItems.Appearance = appearance28;
            this.txtMaxComboItems.AutoSize = false;
            this.txtMaxComboItems.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtMaxComboItems.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtMaxComboItems.FormatString = "###";
            this.txtMaxComboItems.Location = new System.Drawing.Point(173, 95);
            this.txtMaxComboItems.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtMaxComboItems.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtMaxComboItems.MaskInput = "nnn";
            this.txtMaxComboItems.MaxValue = 999;
            this.txtMaxComboItems.MinValue = 1;
            this.txtMaxComboItems.Name = "txtMaxComboItems";
            this.txtMaxComboItems.NullText = "0";
            this.txtMaxComboItems.Size = new System.Drawing.Size(74, 20);
            this.txtMaxComboItems.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtMaxComboItems.TabIndex = 5;
            this.txtMaxComboItems.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtMaxComboItems.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtMaxComboItems.Value = 1;
            // 
            // lblMaxComboItems
            // 
            appearance29.FontData.BoldAsString = "False";
            appearance29.ForeColor = System.Drawing.Color.White;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.lblMaxComboItems.Appearance = appearance29;
            this.lblMaxComboItems.AutoSize = true;
            this.lblMaxComboItems.Location = new System.Drawing.Point(10, 96);
            this.lblMaxComboItems.Name = "lblMaxComboItems";
            this.lblMaxComboItems.Size = new System.Drawing.Size(132, 18);
            this.lblMaxComboItems.TabIndex = 45;
            this.lblMaxComboItems.Text = "Max. Combo Items";
            this.lblMaxComboItems.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkIsActive
            // 
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Location = new System.Drawing.Point(10, 70);
            this.chkIsActive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(176, 20);
            this.chkIsActive.TabIndex = 3;
            this.chkIsActive.Text = "Is Active";
            this.chkIsActive.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // txtMinComboItems
            // 
            appearance30.FontData.BoldAsString = "False";
            this.txtMinComboItems.Appearance = appearance30;
            this.txtMinComboItems.AutoSize = false;
            this.txtMinComboItems.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtMinComboItems.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtMinComboItems.FormatString = "###";
            this.txtMinComboItems.Location = new System.Drawing.Point(474, 70);
            this.txtMinComboItems.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtMinComboItems.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtMinComboItems.MaskInput = "nnn";
            this.txtMinComboItems.MaxValue = 999;
            this.txtMinComboItems.MinValue = 1;
            this.txtMinComboItems.Name = "txtMinComboItems";
            this.txtMinComboItems.NullText = "0";
            this.txtMinComboItems.Size = new System.Drawing.Size(74, 20);
            this.txtMinComboItems.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtMinComboItems.TabIndex = 4;
            this.txtMinComboItems.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtMinComboItems.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtMinComboItems.Value = 1;
            // 
            // ultraLabel1
            // 
            appearance31.FontData.BoldAsString = "False";
            appearance31.ForeColor = System.Drawing.Color.White;
            appearance31.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance31;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(341, 71);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(128, 18);
            this.ultraLabel1.TabIndex = 43;
            this.ultraLabel1.Text = "Min. Combo Items";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkForceComboPricing
            // 
            this.chkForceComboPricing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkForceComboPricing.Location = new System.Drawing.Point(10, 45);
            this.chkForceComboPricing.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkForceComboPricing.Name = "chkForceComboPricing";
            this.chkForceComboPricing.Size = new System.Drawing.Size(175, 20);
            this.chkForceComboPricing.TabIndex = 1;
            this.chkForceComboPricing.Text = "Use Combo Item Price";
            this.chkForceComboPricing.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // txtComboItemPrice
            // 
            appearance32.FontData.BoldAsString = "False";
            this.txtComboItemPrice.Appearance = appearance32;
            this.txtComboItemPrice.AutoSize = false;
            this.txtComboItemPrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtComboItemPrice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtComboItemPrice.FormatString = "##,##,###.00";
            this.txtComboItemPrice.Location = new System.Drawing.Point(474, 45);
            this.txtComboItemPrice.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtComboItemPrice.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtComboItemPrice.MaskInput = "nn,nn,nnn.nn";
            this.txtComboItemPrice.MaxValue = 99999.99D;
            this.txtComboItemPrice.MinValue = -1;
            this.txtComboItemPrice.Name = "txtComboItemPrice";
            this.txtComboItemPrice.NullText = "0";
            this.txtComboItemPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtComboItemPrice.Size = new System.Drawing.Size(74, 20);
            this.txtComboItemPrice.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtComboItemPrice.TabIndex = 2;
            this.txtComboItemPrice.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtComboItemPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel2
            // 
            appearance33.FontData.BoldAsString = "False";
            appearance33.ForeColor = System.Drawing.Color.White;
            appearance33.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance33;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(341, 46);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(126, 18);
            this.ultraLabel2.TabIndex = 40;
            this.ultraLabel2.Text = "Combo Item Price";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtEditorNoOfItems);
            this.groupBox2.Controls.Add(this.ultraLblNoOfItems);
            this.groupBox2.Controls.Add(this.btnDeleteItem);
            this.groupBox2.Controls.Add(this.btnAddItem);
            this.groupBox2.Controls.Add(this.grdDetail);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(9, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(563, 335);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detail";
            // 
            // txtEditorNoOfItems
            // 
            this.txtEditorNoOfItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance34.FontData.BoldAsString = "False";
            appearance34.ForeColor = System.Drawing.Color.White;
            appearance34.ForeColorDisabled = System.Drawing.Color.Black;
            appearance34.TextVAlignAsString = "Middle";
            this.txtEditorNoOfItems.Appearance = appearance34;
            this.txtEditorNoOfItems.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtEditorNoOfItems.Location = new System.Drawing.Point(91, 301);
            this.txtEditorNoOfItems.Name = "txtEditorNoOfItems";
            this.txtEditorNoOfItems.Size = new System.Drawing.Size(100, 26);
            this.txtEditorNoOfItems.TabIndex = 86;
            this.txtEditorNoOfItems.Text = "0";
            // 
            // ultraLblNoOfItems
            // 
            this.ultraLblNoOfItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance35.ForeColor = System.Drawing.Color.White;
            appearance35.TextVAlignAsString = "Middle";
            this.ultraLblNoOfItems.Appearance = appearance35;
            this.ultraLblNoOfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLblNoOfItems.Location = new System.Drawing.Point(11, 301);
            this.ultraLblNoOfItems.Name = "ultraLblNoOfItems";
            this.ultraLblNoOfItems.Size = new System.Drawing.Size(98, 26);
            this.ultraLblNoOfItems.TabIndex = 84;
            this.ultraLblNoOfItems.Text = "No. Of Items";
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance36.FontData.BoldAsString = "True";
            appearance36.ForeColor = System.Drawing.Color.White;
            appearance36.Image = ((object)(resources.GetObject("appearance36.Image")));
            this.btnDeleteItem.Appearance = appearance36;
            this.btnDeleteItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnDeleteItem.Location = new System.Drawing.Point(254, 301);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(144, 26);
            this.btnDeleteItem.TabIndex = 1;
            this.btnDeleteItem.TabStop = false;
            this.btnDeleteItem.Text = "Delete Item ";
            this.btnDeleteItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance37.FontData.BoldAsString = "True";
            appearance37.ForeColor = System.Drawing.Color.White;
            appearance37.Image = ((object)(resources.GetObject("appearance37.Image")));
            this.btnAddItem.Appearance = appearance37;
            this.btnAddItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnAddItem.Location = new System.Drawing.Point(405, 301);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(144, 26);
            this.btnAddItem.TabIndex = 2;
            this.btnAddItem.TabStop = false;
            this.btnAddItem.Text = "Add Item (F2)";
            this.btnAddItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnNew);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 517);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(562, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // frmItemComboPricing
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(584, 591);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmItemComboPricing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Combo Pricing";
            this.Activated += new System.EventHandler(this.frmItemComboPricing_Activated);
            this.Load += new System.EventHandler(this.frmItemComboPricing_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmItemComboPricing_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmItemComboPricing_KeyUp);
            this.Resize += new System.EventHandler(this.frmItemComboPricing_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxComboItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinComboItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkForceComboPricing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComboItemPrice)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        void grdDetail_AfterRowsDeleted(object sender, EventArgs e)
        {
            UpdateNoOfItems();
        }

		private void ApplyGrigFormat()
		{
			clsUIHelper.SetAppearance(this.grdDetail);
		
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId].Hidden = true;
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_Id].Hidden = true;
			
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].MaxLength= 20;
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].Header.Caption= "Item#";

			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].MaxLength = 50;
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Caption = "Item Description";
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellActivation= Activation.Disabled;
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Width= 250;

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY].CellAppearance.TextHAlign= HAlign.Right;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY].MaxValue = 999999;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY].MinValue = 1;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY].MaskInput="999999";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY].Format="######";
			
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice].CellAppearance.TextHAlign= HAlign.Right;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice].MaxValue = 9999.99;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice].Header.Caption = "Sale Price";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice].MaskInput="9999.99";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice].Format = "####.00";

			this.txtDescription.MaxLength=50;

		}

		private void frmItemComboPricing_Load(object sender, System.EventArgs e)
		{
			try
			{
				clsUIHelper.SetKeyActionMappings(this.grdDetail);
				
				this.txtComboItemPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
				this.txtComboItemPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

				this.txtDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
				this.txtDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

				this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
				this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

				clsUIHelper.setColorSchecme(this);
                this.StartPosition = FormStartPosition.CenterScreen;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (Configuration.convertNullToInt(this.txtMinComboItems.Value) > Configuration.convertNullToInt(this.txtMaxComboItems.Value))
                {
                    Resources.Message.Display("Max. Combo Items should be greater than Min. Combo Items", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                oItemComboPricingHRow.Description = this.txtDescription.Text;
                oItemComboPricingHRow.ComboItemPrice = Configuration.convertNullToDecimal(this.txtComboItemPrice.Value);
                oItemComboPricingHRow.ForceGroupPricing = chkForceComboPricing.Checked;
                oItemComboPricingHRow.IsActive = chkIsActive.Checked;
                oItemComboPricingHRow.MinComboItems = Configuration.convertNullToInt(this.txtMinComboItems.Value);
                oItemComboPricingHRow.MaxComboItems = Configuration.convertNullToInt(this.txtMaxComboItems.Value);    //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added
                oItemComboPricing.checkIsValidData(oItemComboPricingHData, oItemComboPricingDData);
                oItemComboPricing.Persist(oItemComboPricingHData, oItemComboPricingDData);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (POSExceptions exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                switch (exp.ErrNumber)
                {
                    case (long)POSErrorENUM.InvRecvHeader_RecvDetailMissing:
                        this.grdDetail.Focus();
                        break;
                }
            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

		private void Clear()
		{
			this.txtComboItemPrice.Value = 0;
			this.txtDescription.Text= String.Empty;
            this.chkForceComboPricing.Checked = false;
		}
		
		public void SetNew()
		{
			oItemComboPricingHData = new  ItemComboPricingData();
			Clear();
            oItemComboPricingHRow=oItemComboPricingHData.ItemComboPricing.AddRow(0,"",false,0,0,true);
			oItemComboPricingDData=new ItemComboPricingDetailData();
			this.grdDetail.DataSource = oItemComboPricingDData ;
			this.grdDetail.Refresh();
			ApplyGrigFormat();
            UpdateNoOfItems();
			this.txtDescription.Focus();
		}
		
        private void FKEdit(string code,string senderName)
		{
			if (senderName==clsPOSDBConstants.Item_tbl)
			{
				#region Items
                try
                {
                    foreach (UltraGridRow row in grdDetail.Rows)
                    {
                        if (row.Index != grdDetail.ActiveRow.Index)
                        {
                            if (row.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].Text.Trim().ToUpper() == code.Trim().ToUpper())
                            {
                                throw new Exception("Duplicate item selected.");
                            }
                        }
                    }
                    Item oItem = new Item();
                    ItemData oItemData;
                    ItemRow oItemRow = null;
                    oItemData = oItem.Populate(code);
                    
                    if (oItemData.Tables[0].Rows.Count == 0)
                    {
                        if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998))
                        {
                            frmItems ofrmItem = new frmItems(code, this.txtComboItemPrice.Text);
                            ofrmItem.ShowDialog();
                            oItemData = oItem.Populate(code);
                        }
                    }
                    oItemRow = oItemData.Item[0];
                    if (oItemRow != null)
                    {
                        if (grdDetail.ActiveRow == null)
                            this.grdDetail.Rows.Band.AddNew();
                        this.grdDetail.ActiveCell.Value = oItemRow.ItemID;
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value = oItemRow.Description;
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice].Value = oItemRow.SellingPrice;

                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.grdDetail.ActiveCell.Value = String.Empty;
                    this.grdDetail.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                    this.grdDetail.ActiveCell.Value = String.Empty;
                    this.grdDetail.ActiveRow.Cells["Description"].Value = String.Empty;
                }
				#endregion
			}
		}

        private void UpdateNoOfItems()
        {
            this.txtEditorNoOfItems.Text = this.grdDetail.Rows.Count.ToString();
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdDetail_Enter(object sender, System.EventArgs e)
		{
			try
			{
				if (this.grdDetail.Rows.Count>0)
				{
                    if (!this.grdDetail.Rows[0].Cells["Qty"].Activated)
					this.grdDetail.Rows[0].Cells[clsPOSDBConstants.Item_Fld_ItemID].Activate();
					this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
				}
				else
				{
					this.grdDetail.Rows.Band.AddNew();
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (m_exceptionAccoured) 
				{
					m_exceptionAccoured = false;
					return;
				}

				if (e.Cell.Column.Key==clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID)
					SearchItem();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void SearchItem()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
                   
					FKEdit(strCode,clsPOSDBConstants.Item_tbl);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void grdDetail_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UltraGridCell oCurrentCell;
			oCurrentCell=this.grdDetail.ActiveCell;
            try
            {
                if (oCurrentCell.DataChanged == false)
                    return;
            }
            catch (Exception ex)
            { 
            }
			try
			{
				if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID && oCurrentCell.Value.ToString()!="")
				{
					FKEdit(oCurrentCell.Value.ToString(),clsPOSDBConstants.Item_tbl);
					if (oCurrentCell.Value.ToString()=="")
					{
						e.Cancel=true;
						this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
					}						
				}
				else if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY)
				{
					oItemComboPricing.Validate_Qty(oCurrentCell.Text.ToString());
				}
				else if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice)
				{
					oItemComboPricing.Validate_SalePrice(oCurrentCell.Text.ToString());
				}
			} 
			catch(Exception exp)
			{
				m_exceptionAccoured = true;
				clsUIHelper.ShowErrorMsg(exp.Message);
				e.Cancel=true;
				this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
			}
		}

		private void ValidateRow(object sender,System.ComponentModel.CancelEventArgs e)
		{
			UltraGridRow oCurrentRow;
			UltraGridCell oCurrentCell;
			oCurrentRow=this.grdDetail.ActiveRow;		
			oCurrentCell=null;
			bool blnCellChanged;
			blnCellChanged=false;
			
			foreach (UltraGridCell oCell in oCurrentRow.Cells)
			{
				if (oCell.DataChanged==true || oCell.Text.Trim()!="")
				{
					blnCellChanged=true;
					break;
				}
			}

			if (blnCellChanged==false)
			{
				return;
			}
            try
            {
                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID];
                oItemComboPricing.Validate_ItemID(oCurrentCell.Text.ToString());
                foreach (UltraGridRow row in grdDetail.Rows)
                {
                    if (row.Index != oCurrentRow.Index)
                    {
                        if (row.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].Text.Trim().ToUpper() == oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].Text.Trim().ToUpper())
                        {
                            throw new Exception("Duplicate item selected.");
                        }
                    }
                }

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY];
                oItemComboPricing.Validate_Qty(oCurrentCell.Text.ToString());

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice];
                oItemComboPricing.Validate_SalePrice(oCurrentCell.Text.ToString());
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    this.grdDetail.ActiveRow = oCurrentRow;
                    string sItemID = oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].Text;
                    this.grdDetail.ActiveRow.Delete(false);
                    m_exceptionAccoured = true;
                    e.Cancel = true;
                    this.grdDetail.ActiveCell = oCurrentCell;
                    this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
            finally
            {
                UpdateNoOfItems();
                if (m_exceptionAccoured == true)
                {
                    AddRow();
                }
            }
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			SetNew();
		}

		private void frmItemComboPricing_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
                    if (this.grdDetail.ContainsFocus == true &&  this.grdDetail.ActiveCell == null)
                    {
                        if (grdDetail.ActiveRow != null)
                        {
                            grdDetail.ActiveCell = grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID];
                            grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                            grdDetail.PerformAction(UltraGridAction.ActivateCell);
                            grdDetail.ActiveCell.Activate();
                        }
                        else
                        {
                            AddRow();
                        }
                    }
                    else  if (this.grdDetail.ContainsFocus == true && this.grdDetail.ActiveCell != null && this.grdDetail.ActiveCell.Text.Trim() == "" && this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID && this.grdDetail.ActiveCell.Row.IsAddRow == true)
					{
						this.SelectNextControl(this.grdDetail,true,true,true,true);
					}
					else if(this.grdDetail.ContainsFocus==false)
					{
						this.SelectNextControl(this.ActiveControl,true,true,true,true);
					}
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmItemComboPricing_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if (this.grdDetail.ContainsFocus==true)
					{
						if (this.grdDetail.ActiveCell!=null)
						{
							if (this.grdDetail.ActiveCell.Column.Key==clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID)
								this.SearchItem();
						}
					}
				}
				else if (e.KeyData==System.Windows.Forms.Keys.F2)
				{
					AddRow();

				}
				e.Handled=true;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void AddRow()
		{
			try
			{
				this.grdDetail.Focus();
				this.grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
				this.grdDetail.PerformAction(UltraGridAction.FirstCellInRow);
				this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
			}
			catch(Exception ){}
		}

		private void frmItemComboPricing_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;            
            this.Top = clsPOSDBConstants.formLeft;
		}

		private void grdDetail_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			UltraGridRow oCurrentRow;
			UltraGridCell oCurrentCell;
			oCurrentRow=e.Row;		
			oCurrentCell=null;
			
			try
			{
				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID];
				oItemComboPricing.Validate_ItemID(oCurrentCell.Text.ToString());
				
				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY];
				oItemComboPricing.Validate_Qty(oCurrentCell.Text.ToString());

				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice];
				oItemComboPricing.Validate_SalePrice(oCurrentCell.Text.ToString());
			} 
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				if (oCurrentCell!=null)
				{
					this.grdDetail.ActiveCell=oCurrentCell;
					this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
					this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
				}
				e.Cancel=true;
			}	
		
		}

		private void btnAddItem_Click(object sender, System.EventArgs e)
		{
			AddRow();
		}

		private void btnDeleteItem_Click(object sender, System.EventArgs e)
		{
            //Modified by shitaljit  on 9/19/2013 add POS confirnation message instead of ultragid default message.
            if (this.grdDetail.ActiveRow != null && Resources.Message.Display("Are you sure, you want to delete the selected item?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				this.grdDetail.ActiveRow.Delete(false);
			}
		}

        private void frmItemComboPricing_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }

        public void Edit(int itemComboPriceId)
        {
            try
            {
                oItemComboPricingHData= oItemComboPricing.Populate(itemComboPriceId);
                oItemComboPricingHRow= oItemComboPricingHData.ItemComboPricing[0];

                if (oItemComboPricingHRow != null)
                {
                    oItemComboPricingDData = oItemComboPricing.PopulateDetail(oItemComboPricingHRow.Id);
                    Display();
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ToString());
            }
        }

        private void Display()
        {
            txtDescription.Text = oItemComboPricingHRow.Description;
            chkForceComboPricing.Checked= oItemComboPricingHRow.ForceGroupPricing;
            txtComboItemPrice.Value = oItemComboPricingHRow.ComboItemPrice;
            chkIsActive.Checked = oItemComboPricingHRow.IsActive;
            txtMinComboItems.Value = oItemComboPricingHRow.MinComboItems;
            txtMaxComboItems.Value = oItemComboPricingHRow.MaxComboItems; //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added
            this.grdDetail.DataSource = oItemComboPricingDData;
            this.grdDetail.Refresh();
            ApplyGrigFormat();
            UpdateNoOfItems();
            this.txtDescription.Focus();
        }
        
	}
}
