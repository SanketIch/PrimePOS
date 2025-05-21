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
	public class frmGroupPricing : System.Windows.Forms.Form
	{
		private bool m_FromError = false;
		private ItemGroupPrice oItemGroupPrice = new ItemGroupPrice();
		private ItemGroupPriceData oItemGroupPriceData = new ItemGroupPriceData();
		private string mItemId = "";
		private int m_rowIndex = -1;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdGroupPricing;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private IContainer components;

		public frmGroupPricing(string ItemId)
		{
			//
			// Required for Windows Form Designer support
			//
			mItemId = ItemId;

			InitializeComponent();
			try
			{
				oItemGroupPriceData = oItemGroupPrice.PopulateList(clsPOSDBConstants.ItemGroupPrice_Fld_ItemID + " = '" + mItemId + "'" );
				grdGroupPricing.DataSource = oItemGroupPriceData;
				grdGroupPricing.Refresh();
				ApplyGrigFormat();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void ApplyGrigFormat()
		{

			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID].MaxLength = 20;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID].Hidden = true;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_ItemID].Hidden = true;
			
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Qty].CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Qty].MaxValue = 99999;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Qty].MaskInput="99999";

			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Cost].CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Cost].MaxValue = 99999.99;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Cost].MaskInput="99999.99";

			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Cost].Format="#####.00";

			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice].CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice].MaxValue = 99999.99;
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice].MaskInput="99999.99";
			
			grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice].Format="#####.00";

            grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_StartDate].Header.Caption = "Start Date";
            grdGroupPricing.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemGroupPrice_Fld_EndDate].Header.Caption = "End Date";

            grdGroupPricing.DisplayLayout.Bands[0].Columns["Delete"].Header.VisiblePosition = grdGroupPricing.DisplayLayout.Bands[0].Columns.Count - 1;

            clsUIHelper.SetAppearance(this.grdGroupPricing);
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Qty");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Price");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SalePrice");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupPricing));
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
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Qty");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Cost");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            this.grdGroupPricing = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdGroupPricing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdGroupPricing
            // 
            this.grdGroupPricing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackColorDisabled = System.Drawing.Color.White;
            appearance1.BackColorDisabled2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdGroupPricing.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 100;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 100;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn5.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            ultraGridColumn5.CellButtonAppearance = appearance2;
            ultraGridColumn5.DefaultCellValue = "Delete";
            ultraGridColumn5.Header.Caption = "";
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.NullText = "Delete";
            ultraGridColumn5.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn5.TabStop = false;
            ultraGridColumn5.Width = 100;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.grdGroupPricing.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdGroupPricing.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdGroupPricing.DisplayLayout.InterBandSpacing = 10;
            this.grdGroupPricing.DisplayLayout.MaxColScrollRegions = 1;
            this.grdGroupPricing.DisplayLayout.MaxRowScrollRegions = 1;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            this.grdGroupPricing.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.ActiveRowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.AddRowAppearance = appearance5;
            this.grdGroupPricing.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdGroupPricing.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdGroupPricing.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdGroupPricing.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdGroupPricing.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdGroupPricing.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.grdGroupPricing.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackColorDisabled = System.Drawing.Color.White;
            appearance7.BackColorDisabled2 = System.Drawing.Color.White;
            appearance7.BorderColor = System.Drawing.Color.Black;
            appearance7.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdGroupPricing.DisplayLayout.Override.CellAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Gray;
            appearance8.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            appearance8.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdGroupPricing.DisplayLayout.Override.CellButtonAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdGroupPricing.DisplayLayout.Override.EditCellAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.FilteredInRowAppearance = appearance10;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.FilteredOutRowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackColorDisabled = System.Drawing.Color.White;
            appearance12.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdGroupPricing.DisplayLayout.Override.FixedCellAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance13.BackColor2 = System.Drawing.Color.Beige;
            this.grdGroupPricing.DisplayLayout.Override.FixedHeaderAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.FontData.BoldAsString = "True";
            appearance14.FontData.SizeInPoints = 10F;
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.TextHAlignAsString = "Left";
            appearance14.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdGroupPricing.DisplayLayout.Override.HeaderAppearance = appearance14;
            this.grdGroupPricing.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdGroupPricing.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance15.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.RowAlternateAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.White;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance16.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.RowPreviewAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderColor = System.Drawing.Color.Transparent;
            appearance18.ForeColor = System.Drawing.Color.White;
            this.grdGroupPricing.DisplayLayout.Override.RowSelectorAppearance = appearance18;
            this.grdGroupPricing.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdGroupPricing.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdGroupPricing.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance19.BackColor = System.Drawing.Color.Navy;
            appearance19.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdGroupPricing.DisplayLayout.Override.SelectedCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.Navy;
            appearance20.BackColorDisabled = System.Drawing.Color.Navy;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance20.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            appearance20.ForeColor = System.Drawing.Color.White;
            this.grdGroupPricing.DisplayLayout.Override.SelectedRowAppearance = appearance20;
            this.grdGroupPricing.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGroupPricing.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdGroupPricing.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdGroupPricing.DisplayLayout.Override.TemplateAddRowAppearance = appearance21;
            this.grdGroupPricing.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdGroupPricing.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance22.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance22;
            this.grdGroupPricing.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdGroupPricing.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdGroupPricing.Location = new System.Drawing.Point(12, 18);
            this.grdGroupPricing.Name = "grdGroupPricing";
            this.grdGroupPricing.Size = new System.Drawing.Size(541, 168);
            this.grdGroupPricing.TabIndex = 1;
            this.grdGroupPricing.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdGroupPricing.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdGroupPricing.AfterRowsDeleted += new System.EventHandler(this.grdGroupPricing_AfterRowsDeleted);
            this.grdGroupPricing.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdGroupPricing_AfterRowInsert);
            this.grdGroupPricing.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdGroupPricing_CellChange);
            this.grdGroupPricing.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdGroupPricing_ClickCellButton);
            this.grdGroupPricing.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdGroupPricing_Error);
            this.grdGroupPricing.Enter += new System.EventHandler(this.grdGroupPricing_Enter);
            this.grdGroupPricing.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGroupPricing_KeyDown);
            this.grdGroupPricing.Validated += new System.EventHandler(this.grdGroupPricing_Validated);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance23.FontData.BoldAsString = "True";
            appearance23.ForeColor = System.Drawing.Color.Black;
            appearance23.Image = ((object)(resources.GetObject("appearance23.Image")));
            this.btnClose.Appearance = appearance23;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(465, 16);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 35);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance24.FontData.BoldAsString = "True";
            appearance24.ForeColor = System.Drawing.Color.Black;
            appearance24.Image = ((object)(resources.GetObject("appearance24.Image")));
            this.btnSave.Appearance = appearance24;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(367, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Ok";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            appearance25.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance25.BackColor2 = System.Drawing.Color.Azure;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.ForeColor = System.Drawing.Color.Navy;
            appearance25.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance25.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance25;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(598, 35);
            this.lblTransactionType.TabIndex = 25;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Group Pricing";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdGroupPricing);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(14, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(566, 198);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add / Edit / Delete Group Pricing";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(13, 256);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(567, 60);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // frmGroupPricing
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(598, 333);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmGroupPricing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "Item Group Pricing";
            this.Activated += new System.EventHandler(this.frmGroupPricing_Activated);
            this.Load += new System.EventHandler(this.frmGroupPricing_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGroupPricing_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdGroupPricing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private void frmGroupPricing_Load(object sender, System.EventArgs e)
		{
			this.grdGroupPricing.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.grdGroupPricing.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			clsUIHelper.SetKeyActionMappings(this.grdGroupPricing);
			clsUIHelper.SetAppearance(this.grdGroupPricing);

			clsUIHelper.setColorSchecme(this);
		}

		private bool Save()
		{
			try
			{
                foreach (ItemGroupPriceRow oRow in oItemGroupPriceData.ItemGroupPrice.Rows)
                {
                    if (oRow.RowState != System.Data.DataRowState.Deleted)
                    {
                        if (oRow.ItemID == "")
                        {
                            oRow.ItemID = mItemId;
                        }
                    }
                }

				oItemGroupPrice.Persist(oItemGroupPriceData);
				return true;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				return false;
			}
		}

		private void grdGroupPricing_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
                if ((m_rowIndex) < 0)
                {
                    e.Row.Cells[clsPOSDBConstants.ItemGroupPrice_Fld_Cost].Value = 0;
                    return;
                }
                
				//ItemGroupPriceRow oRow = (ItemGroupPriceRow)oItemGroupPriceData.Tables[0].Rows[m_rowIndex];
				//oRow.ItemID = mItemId;
                e.Row.Cells[clsPOSDBConstants.ItemGroupPrice_Fld_Cost].Value = 0;
                e.Row.Cells[clsPOSDBConstants.ItemGroupPrice_Fld_ItemID].Value = mItemId;
				grdGroupPricing.Update();
			}
			catch(Exception Exp)
			{
				clsUIHelper.ShowErrorMsg(Exp.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (Save()==true)
					this.Close();
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

		private void grdGroupPricing_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (grdGroupPricing.ActiveRow==null)
				{
					m_rowIndex = -1;
					return;
				}
				m_rowIndex = grdGroupPricing.ActiveRow.Index;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void frmGroupPricing_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData == Keys.Enter)
				{
					if (this.grdGroupPricing.ContainsFocus==true && this.grdGroupPricing.ActiveCell.Text.Trim()=="" && this.grdGroupPricing.ActiveCell.Column.Key==clsPOSDBConstants.ItemGroupPrice_Fld_Qty && this.grdGroupPricing.ActiveCell.Row.IsAddRow==true)
					{
						//this.SelectNextControl(this.grdGroupPricing,true,true,true,true);
						System.Windows.Forms.SendKeys.Send("{Tab}");
						e.Handled=true;
					}
					else if(this.grdGroupPricing.ContainsFocus==false)
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

		private void grdGroupPricing_Enter(object sender, System.EventArgs e)
		{
			if (this.grdGroupPricing.Rows.Count>0)
			{
                if (!m_FromError)
                {
                    this.grdGroupPricing.Rows[0].Cells[clsPOSDBConstants.ItemGroupPrice_Fld_Qty].Activate();
                    m_rowIndex = 0;
                }
				this.grdGroupPricing.PerformAction(UltraGridAction.EnterEditMode);
			}
			else
			{
				this.grdGroupPricing.Rows.Band.AddNew();
			}		
		}

		private void grdGroupPricing_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
		{
			m_FromError = true;
			CommonUI.checkGridError((Infragistics.Win.UltraWinGrid.UltraGrid)sender,e,clsPOSDBConstants.ItemGroupPrice_Fld_Qty,clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice,clsPOSDBConstants.ItemGroupPrice_Fld_Cost);
		}

		private void grdGroupPricing_Validated(object sender, System.EventArgs e)
		{
			grdGroupPricing.PerformAction(UltraGridAction.LastCellInGrid);
		}

		private void frmGroupPricing_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void grdGroupPricing_AfterRowsDeleted(object sender, EventArgs e)
        {
            if (this.grdGroupPricing.Rows.Count > 0)
            {
                this.grdGroupPricing.ActiveRow = this.grdGroupPricing.Rows[0];
                this.grdGroupPricing.ActiveCell = this.grdGroupPricing.ActiveRow.Cells[0];
                this.grdGroupPricing.PerformAction(UltraGridAction.EnterEditMode);
            }
            else
            {
                this.grdGroupPricing.DisplayLayout.Bands[0].AddNew();
            }
        }

        private void grdGroupPricing_ClickCellButton(object sender, CellEventArgs e)
        {
            if (e.Cell.Column.Key == "Delete")
            
            {
                if (e.Cell.Row.IsAddRow == false)
                {
                    e.Cell.Row.Delete(false);
                }
            }
        }
	}
}
