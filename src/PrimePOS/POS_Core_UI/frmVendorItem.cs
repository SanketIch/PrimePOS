using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinGrid;
namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPayOut.
	/// </summary>
	public class frmVendorItem : System.Windows.Forms.Form
	{
		private bool m_FromError = false;
		private ItemVendor oItemVendor = new ItemVendor();
		private ItemVendorData oItemVendorData = new ItemVendorData();
		private string mItemId = "";
		private int m_rowIndex = -1;
		private string m_VendorCode;
		private int m_LastCell;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
        private bool m_IsCellUpdateCalled = false;
        private IContainer components;

		public frmVendorItem(string ItemId)
		{
			//
			// Required for Windows Form Designer support
			//
			mItemId = ItemId;
			InitializeComponent();
			try
			{
				oItemVendorData = oItemVendor.PopulateList(" WHERE ItemVendor." + clsPOSDBConstants.ItemVendor_Fld_ItemID + " = '" + mItemId + "'" );
				grdDetail.DataSource = oItemVendorData;
				grdDetail.Refresh();
				ApplyGrigFormat();
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void ApplyGrigFormat()
		{

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].MaxLength = 20;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ItemID].Hidden = true;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorID].Hidden = true;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ItemDetailID].Hidden = true;
			
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].MaxValue = 99999.99;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Format="####0.00";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].MaskInput="99999.99";

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].MaxLength=20;

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].MaxLength= 20;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Style=  Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_LastOrderDate].DefaultCellValue=  DateTime.Now;

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Header.Caption = "Vendor Code";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Header.Caption = "Vendor Item Code";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Header.Caption = "Price";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Header.Caption = "Vendor Code";
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_VendorName].CellActivation=Activation.Disabled;

			clsUIHelper.SetAppearance(this.grdDetail); 

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>

		private bool Save()
		{
			try
			{
				oItemVendor.Persist(oItemVendorData);
				return true;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				return false;
			}
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Code");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Item Code");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost Price");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Last Order Date", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVendorItem));
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
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vendor Code");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vendor Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vendor Item Code");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Cost Price");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Last Order Date");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdDetail
            // 
            this.grdDetail.DataSource = this.ultraDataSource2;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackColorDisabled = System.Drawing.Color.White;
            appearance1.BackColorDisabled2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn1.Width = 95;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 128;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 157;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 87;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 140;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance4;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BackColorDisabled = System.Drawing.Color.White;
            appearance6.BackColorDisabled2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Black;
            appearance6.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Gray;
            appearance7.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            appearance7.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BackColorDisabled = System.Drawing.Color.White;
            appearance11.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance12.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.FontData.BoldAsString = "True";
            appearance13.FontData.SizeInPoints = 10F;
            appearance13.ForeColor = System.Drawing.Color.White;
            appearance13.TextHAlignAsString = "Left";
            appearance13.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance13;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDetail.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance15.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            appearance17.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.Navy;
            appearance18.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.Navy;
            appearance19.BackColorDisabled = System.Drawing.Color.Navy;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance19.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            appearance19.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance19;
            this.grdDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance20;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance21.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance21.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance21.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance21;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(12, 21);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(626, 185);
            this.grdDetail.TabIndex = 1;
            this.grdDetail.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmVendorItem_KeyUp);
            this.grdDetail.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdDetail_Error);
            this.grdDetail.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDetail_AfterRowInsert);
            this.grdDetail.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdDetail_BeforeCellDeactivate);
            this.grdDetail.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_AfterCellUpdate);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            this.grdDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grdDetail_KeyPress);
            this.grdDetail.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_CellChange);
            this.grdDetail.Validated += new System.EventHandler(this.grdDetail_Validated);
            this.grdDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmVendorItem_KeyDown);
            this.grdDetail.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.ValidateRow);
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5});
            // 
            // btnClose
            // 
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance22.FontData.BoldAsString = "True";
            appearance22.Image = ((object)(resources.GetObject("appearance22.Image")));
            this.btnClose.Appearance = appearance22;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(548, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance23.FontData.BoldAsString = "True";
            appearance23.Image = ((object)(resources.GetObject("appearance23.Image")));
            this.btnSave.Appearance = appearance23;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(450, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.ForeColorDisabled = System.Drawing.Color.White;
            appearance24.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance24;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(12, 7);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(649, 40);
            this.lblTransactionType.TabIndex = 25;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Vendor Items";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdDetail);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(649, 216);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add / Edit / Delete Vendor Item Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 270);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(647, 58);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            // 
            // frmVendorItem
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(676, 345);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmVendorItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmVendorItem_Load);
            this.Activated += new System.EventHandler(this.frmVendorItem_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmVendorItem_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmVendorItem_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdDetail_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (grdDetail.ActiveRow==null)
				{
					m_rowIndex = -1;
					return;
				}
			
				m_LastCell = e.Cell.Column.Index;

				if (e.Cell.Column.Index == 4)
				{
					m_VendorCode =  e.Cell.Text;
				}

				m_rowIndex = grdDetail.ActiveRow.Index;
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void grdDetail_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				if ((m_rowIndex) < 0) return;

				ItemVendorRow oRow = (ItemVendorRow)oItemVendorData.Tables[0].Rows[m_rowIndex];
				oRow.ItemID = mItemId;
				grdDetail.Update();
			}
			catch(Exception Exp)
			{
				clsUIHelper.ShowErrorMsg(Exp.Message);
			}
		}

		private void grdDetail_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (m_LastCell==4)
					EditVendor();
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void EditVendor()
		{
			try
			{
				if (m_IsCellUpdateCalled)
				{
					m_IsCellUpdateCalled = false;
					return;
				}

				System.Int32 iVendorId ;
				VendorData mVendorData = new VendorData();
                POS_Core.BusinessRules.Vendor mBRVendor = new  POS_Core.BusinessRules.Vendor();
				VendorRow mVendorRow;
				mVendorData = mBRVendor.Populate(m_VendorCode);
				mVendorRow = (VendorRow)mVendorData.Tables[0].Rows[0];
			
				iVendorId = mVendorRow.VendorId;
				grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorName].Value = mVendorRow.Vendorname;
				grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorID].Value = iVendorId;
			}
			catch(Exception )
			{
				m_IsCellUpdateCalled = true;
				grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorID].Value = System.DBNull.Value;
				m_IsCellUpdateCalled = true;
				grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Value = System.DBNull.Value;
				m_IsCellUpdateCalled = true;
				grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorName].Value = System.DBNull.Value;
				grdDetail.Focus();
			}
		}


		private void SearchVendor()
		{
			try
			{
				System.Int32 iVendorId ;
				VendorData mVendorData = new VendorData();
                POS_Core.BusinessRules.Vendor mBRVendor = new POS_Core.BusinessRules.Vendor();
				VendorRow mVendorRow;
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
                oSearch.ActiveOnly=1;
				oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					mVendorData = mBRVendor.Populate(oSearch.SelectedRowID());
					mVendorRow = (VendorRow)mVendorData.Tables[0].Rows[0];
				
					iVendorId = mVendorRow.VendorId;
					m_VendorCode = mVendorRow.Vendorcode;
					grdDetail.ActiveRow.Cells[5].Value = mVendorRow.Vendorname;
					grdDetail.ActiveRow.Cells[4].Value = mVendorRow.Vendorcode;
					grdDetail.ActiveRow.Cells[3].Value = iVendorId;
					grdDetail.Update();
					grdDetail.Refresh();
				}
			}
			catch(Exception Exp)
			{
				clsUIHelper.ShowErrorMsg(Exp.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (Save())
				this.Close();
		}

		private void frmVendorItem_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			
			if (this.grdDetail.ContainsFocus==true)
			{
				if (this.grdDetail.ActiveCell!=null)
				{
					if (this.grdDetail.ActiveCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorCode)
						if (e.KeyData==System.Windows.Forms.Keys.F4)
						{
							this.SearchVendor();
							e.Handled=true;
						}
				}
			}
			/*else if(e.KeyData==System.Windows.Forms.Keys.Enter)
			{
				this.SelectNextControl(this.ActiveControl,true,true,true,true);
			}
			*/
		}

		private void grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{	
			try
			{
				if (e.Cell.Column.Index==4)
					SearchVendor();
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void frmVendorItem_Load(object sender, System.EventArgs e)
		{
			this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_CatPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ContractPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_RetailPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_IsDeleted].Hidden = true;

            //Added by SRT(Abhishek) Date : 23/09/2009 
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_BaseCharge].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ResalePrice].Hidden = true;
            //End Of Added by SRT(Abhishek) Date : 23/09/2009

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_CatPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ContractPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_RetailPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_IsDeleted].DefaultCellValue = false;
            
            //Added By SRT(Abhishek) Date: 23/09/2009 
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_BaseCharge].DefaultCellValue = 0.00;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemVendor_Fld_ResalePrice].DefaultCellValue = 0.00;
            //End Of Added By SRT(Abhishek) Date: 23/09/2009
            
            clsUIHelper.SetAppearance(this.grdDetail);
			clsUIHelper.SetKeyActionMappings(this.grdDetail);
			clsUIHelper.setColorSchecme(this);
		}

		private void grdDetail_Enter(object sender, System.EventArgs e)
		{
			
			if (this.grdDetail.Rows.Count>0)
			{
				if (!m_FromError) this.grdDetail.Rows[0].Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Activate();
				this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
			}
			else
			{
				this.grdDetail.Rows.Band.AddNew();
			}	
		}

		private void grdDetail_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
			/*UltraGridCell oCurrentCell;
			oCurrentCell=this.grdDetail.ActiveCell;
			if (oCurrentCell.DataChanged==false)
				return;
			try
			{
				if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorCode && oCurrentCell.Value.ToString()!="")
				{
					EditVendor();
					if (oCurrentCell.Value.ToString()=="")
					{
						e.Cancel=true;
						this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
					}
				}
				else if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice)
				{
					oItemVendor.Validate_Price(oCurrentCell.Text.ToString());
				}
				else if (oCurrentCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorItemID )
				{
					oItemVendor.Validate_VendorItemID(oCurrentCell.Text.ToString());
				}
			} 
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				e.Cancel=true;
				this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
			}*/
		}
		
		private void ValidateRow(object sender,System.ComponentModel.CancelEventArgs e)
		{
			UltraGridRow oCurrentRow;
			UltraGridCell oCurrentCell;
			oCurrentRow=this.grdDetail.ActiveRow;		
			oCurrentCell=null;
			bool blnCellChanged;
			blnCellChanged=false;
			
			if (oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].Text!="" ||
				oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCode].Text!="" ||
				oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Text!="" )
			{
				blnCellChanged=true;
			}

			if (blnCellChanged==false)
			{
				return;
			}
			try
			{
				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorItemID];
				oItemVendor.Validate_VendorItemID(oCurrentCell.Text.ToString());
				
				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCode];
				oItemVendor.Validate_VendorID(oCurrentCell.Text.ToString());
				
				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice];
				oItemVendor.Validate_Price(oCurrentCell.Text.ToString());

			} 
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				if (oCurrentCell!=null)
				{
					//oCurrentCell.Activate();
					//oCurrentCell.Activate();
					e.Cancel=true;
					this.grdDetail.ActiveCell=oCurrentCell;
					this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
					this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
				}
			}	
		}

		private void frmVendorItem_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData == Keys.Enter)
				{
					if (this.grdDetail.ContainsFocus==true && this.grdDetail.ActiveCell.Text.Trim()=="" && this.grdDetail.ActiveCell.Column.Key==clsPOSDBConstants.ItemVendor_Fld_VendorItemID && this.grdDetail.ActiveCell.Row.IsAddRow==true)
					{
						//this.SelectNextControl(this.grdGroupPricing,true,true,true,true);
						System.Windows.Forms.SendKeys.Send("{Tab}");
						e.Handled=true;
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

        private void grdDetail_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            m_FromError = true;            
            e.Cancel = true;
            //CommonUI.checkGridError((Infragistics.Win.UltraWinGrid.UltraGrid)sender,e,clsPOSDBConstants.ItemVendor_Fld_VendorItemID,clsPOSDBConstants.ItemVendor_Fld_VendorCode);
        }

		private void grdDetail_Validated(object sender, System.EventArgs e)
		{
			grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
		}

		private void grdDetail_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{

		}

		private void frmVendorItem_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}
	}
}
