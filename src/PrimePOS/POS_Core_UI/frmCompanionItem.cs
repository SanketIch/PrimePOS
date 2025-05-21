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
	public class frmCompanionItem : System.Windows.Forms.Form
	{
		private bool m_FromError = false;
		private ItemCompanion oItemCompanion = new ItemCompanion();
		private ItemCompanionData oItemCompanionData = new ItemCompanionData();
		private string mItemId = "";
		private string mCompanionItemId = "";
		private int m_rowIndex = -1;
		private int m_ColIndex = -1;
		private bool isCellUpdateCalled;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtItemCode;
		private System.Windows.Forms.TextBox txtCompanionItemCode;
		private Infragistics.Win.Misc.UltraButton btnSearch;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private IContainer components;

		public frmCompanionItem(string ItemId)
		{
			//
			// Required for Windows Form Designer support
			//
			mItemId = ItemId;
			InitializeComponent();
			try
			{
				oItemCompanionData = oItemCompanion.PopulateList(" Companion." + clsPOSDBConstants.ItemCompanion_Fld_ItemID + " = '" + mItemId + "'" );
				grdDetail.DataSource = oItemCompanionData;
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

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].MaxLength = 20;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].ButtonDisplayStyle =  Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_ItemID].Hidden = true;
			
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_ItemDescription].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Amount].CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Amount].MaxValue = 99999.99;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Amount].MaskInput="99999.99";

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Amount].Format="#####.00";

			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Percentage].CellAppearance.TextHAlign= Infragistics.Win.HAlign.Right;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Percentage].MaxValue = 100;
			grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Percentage].MaskInput="999.99";    //PRIMEPOS-2923 13-Nov-2020 JY modified

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_Percentage].Format="###.00";   //PRIMEPOS-2923 13-Nov-2020 JY modified

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].Header.Caption = "Companion Item#";

			for (int i=0;i<grdDetail.Rows.Count;i++)
			{
				grdDetail.Rows[i].Cells[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].Activation=Activation.Disabled;
			}

			clsUIHelper.SetAppearance(this.grdDetail);
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Item Code");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Amount", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentage");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 0);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompanionItem));
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
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Item Code");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Percentage");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Qty");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Cost");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Price");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCompanionItemCode = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdDetail
            // 
            this.grdDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.AddNewBox.Appearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            appearance2.BackColorDisabled = System.Drawing.Color.White;
            appearance2.BackColorDisabled2 = System.Drawing.Color.White;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance2;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.ShowInkButton = Infragistics.Win.ShowInkButton.Always;
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn1.Width = 104;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 165;
            ultraGridColumn3.Format = "nnnnnn0.00";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 79;
            ultraGridColumn4.Format = "nnn0.00";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.MaxValue = 100;
            ultraGridColumn4.MinValue = 0;
            ultraGridColumn4.Width = 120;
            ultraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn5.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            ultraGridColumn5.CellButtonAppearance = appearance3;
            ultraGridColumn5.DefaultCellValue = "Delete";
            ultraGridColumn5.Header.Caption = "";
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.NullText = "Delete";
            ultraGridColumn5.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridColumn5.TabStop = false;
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
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance7.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance7;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnBottom;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BackColorDisabled = System.Drawing.Color.White;
            appearance9.BackColorDisabled2 = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.Black;
            appearance9.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance10.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            appearance10.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            appearance10.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.DataErrorRowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance12;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackColorDisabled = System.Drawing.Color.White;
            appearance15.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance16.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.FontData.BoldAsString = "True";
            appearance17.FontData.SizeInPoints = 9F;
            appearance17.ForeColor = System.Drawing.Color.White;
            appearance17.TextHAlignAsString = "Left";
            appearance17.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance17;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDetail.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance19.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            appearance21.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance21;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance22.BackColor = System.Drawing.Color.Navy;
            appearance22.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.Navy;
            appearance23.BackColorDisabled = System.Drawing.Color.Navy;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance23.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            appearance23.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance23;
            this.grdDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdDetail.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            appearance24.BackColorDisabled = System.Drawing.Color.White;
            appearance24.BackColorDisabled2 = System.Drawing.Color.White;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance25.BackColor2 = System.Drawing.Color.White;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance25.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance25.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance25;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(14, 18);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(598, 200);
            this.grdDetail.TabIndex = 1;
            this.grdDetail.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.AfterCellActivate += new System.EventHandler(this.grdDetail_AfterCellActivate);
            this.grdDetail.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_AfterCellUpdate);
            this.grdDetail.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdDetail_InitializeLayout);
            this.grdDetail.AfterRowsDeleted += new System.EventHandler(this.grdDetail_AfterRowsDeleted);
            this.grdDetail.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDetail_AfterRowInsert);
            this.grdDetail.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_CellChange);
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdDetail_BeforeRowInsert);
            this.grdDetail.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdDetail_BeforeCellUpdate);
            this.grdDetail.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdDetail_Error);
            this.grdDetail.CellDataError += new Infragistics.Win.UltraWinGrid.CellDataErrorEventHandler(this.grdDetail_CellDataError);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            this.grdDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCompanionItem_KeyDown);
            this.grdDetail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmCompanionItem_KeyUp);
            this.grdDetail.Leave += new System.EventHandler(this.grdDetail_Leave);
            this.grdDetail.Validated += new System.EventHandler(this.grdDetail_Validated);
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4});
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn5,
            ultraDataColumn6,
            ultraDataColumn7});
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance26.FontData.BoldAsString = "True";
            appearance26.ForeColor = System.Drawing.Color.Black;
            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
            this.btnClose.Appearance = appearance26;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(516, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 33);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Cancel";
            this.btnClose.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance27.FontData.BoldAsString = "True";
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            this.btnSave.Appearance = appearance27;
            this.btnSave.Location = new System.Drawing.Point(418, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 33);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Ok";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSearch
            // 
            appearance28.ForeColor = System.Drawing.Color.Black;
            appearance28.Image = ((object)(resources.GetObject("appearance28.Image")));
            this.btnSearch.Appearance = appearance28;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(429, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(73, 26);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "S&earch";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(237, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Item Code";
            // 
            // txtItemCode
            // 
            this.txtItemCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtItemCode.Location = new System.Drawing.Point(313, 15);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(110, 21);
            this.txtItemCode.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(138, 299);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cmpn Itm Code";
            // 
            // txtCompanionItemCode
            // 
            this.txtCompanionItemCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanionItemCode.Location = new System.Drawing.Point(121, 15);
            this.txtCompanionItemCode.Name = "txtCompanionItemCode";
            this.txtCompanionItemCode.Size = new System.Drawing.Size(110, 21);
            this.txtCompanionItemCode.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdDetail);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(621, 230);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Co&mpanion Items";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCompanionItemCode);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtItemCode);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(50, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(22, 44);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sea&rch";
            this.groupBox2.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 271);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(621, 54);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            // 
            // lblTransactionType
            // 
            appearance29.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance29.BackColor2 = System.Drawing.Color.Azure;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance29.ForeColor = System.Drawing.Color.Navy;
            appearance29.ForeColorDisabled = System.Drawing.Color.White;
            appearance29.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance29;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(641, 35);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Companion Items";
            // 
            // frmCompanionItem
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(641, 337);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCompanionItem";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "";
            this.Text = "Companion Item";
            this.Activated += new System.EventHandler(this.frmCompanionItem_Activated);
            this.Load += new System.EventHandler(this.frmCompanionItem_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCompanionItem_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmCompanionItem_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		private void ultraButton2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private bool Save()
		{
			try
			{
				oItemCompanion.Persist(oItemCompanionData);
				return true;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				return false;
			}
		}


		private void Search()
		{
			oItemCompanionData = oItemCompanion.PopulateList( buildCriteria() );
			grdDetail.DataSource = oItemCompanionData;
			grdDetail.Refresh();
		}

		private string buildCriteria()
		{
			string sCriteria = "";
			if (txtCompanionItemCode.Text!= "")
			{
				sCriteria = sCriteria + ((sCriteria=="")? " WHERE ":" AND ");
				sCriteria = sCriteria + clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID + " LIKE '" + txtCompanionItemCode.Text + "%'";
			}
			if (txtItemCode.Text!= "")
			{
				sCriteria = sCriteria + ((sCriteria=="")? " WHERE ":" AND ");
				sCriteria = sCriteria + " Companion." + clsPOSDBConstants.ItemCompanion_Fld_ItemID + " LIKE '" + txtItemCode.Text + "%'";
			}

			return sCriteria;
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			Search();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (Save())
				this.Close();
		}

		private void grdDetail_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			Item oItem = new Item();
			ItemData oItemData ;
			ItemRow oItemRow;
			if (isCellUpdateCalled) 
			{
				isCellUpdateCalled = false;
				return;
			}

			isCellUpdateCalled = false;
			if (m_ColIndex ==grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].Index)
			{
				try
				{
					oItemData = oItem.Populate(mCompanionItemId);
					oItemRow = (ItemRow)oItemData.Item.Rows[0];
				    grdDetail.Rows[m_rowIndex].Cells[2].Value = oItemRow.Description;
				}
				catch(Exception )
				{
					isCellUpdateCalled = true;
					
					grdDetail.Rows[m_rowIndex].Cells[2].Value = System.DBNull.Value;
					isCellUpdateCalled = true;

					grdDetail.Rows[m_rowIndex].Cells[0].Value = System.DBNull.Value;
					isCellUpdateCalled = true;

					grdDetail.Focus();
				}
			}
		}


		private void grdDetail_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
				try
				{

					if ((m_rowIndex) < 0) return;
                    if (oItemCompanionData.Tables[0].Rows.Count > m_rowIndex)
                    {
                        ItemCompanionRow oRow = (ItemCompanionRow)oItemCompanionData.Tables[0].Rows[m_rowIndex];
                        oRow.ItemID = mItemId;
                        grdDetail.Update();
                    }
				}
				catch(Exception Exp)
				{
					clsUIHelper.ShowErrorMsg(Exp.Message);
				}
		}

		private void grdDetail_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
		}

		private void grdDetail_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (grdDetail.ActiveRow==null)
				{
					if (grdDetail.Rows.Count !=0)
						m_rowIndex = grdDetail.Rows.Count;
					else
						m_rowIndex = -1;

					return;
				}
				if (grdDetail.ActiveCell.Column.Index==grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].Index)
					mCompanionItemId = grdDetail.ActiveCell.Text;

				m_rowIndex = grdDetail.ActiveRow.Index;
				m_ColIndex = grdDetail.ActiveCell.Column.Index;
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
                if (e.Cell.Column.Key == "Delete")
                {
                    e.Cell.Row.Delete(false);
                }
                else
                {
                    if (grdDetail.ActiveCell.Column.Index == grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].Index)
                    {
                        SearchItem();
                    }
                }
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void SearchItem()
		{
			Item oItem = new Item();
			ItemData oItemData ;
			ItemRow oItemRow;

			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (oSearch.IsCanceled) return;

				oItemData = oItem.Populate(oSearch.SelectedRowID());
				oItemRow = (ItemRow)oItemData.Item.Rows[0];
				mCompanionItemId =  oItemRow.ItemID;
				grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[0].Value = oItemRow.ItemID;
				grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[2].Value = oItemRow.Description;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmCompanionItem_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{

			try
			{
				if (e.KeyData == Keys.Enter)
				{
					if (this.grdDetail.ContainsFocus==true && this.grdDetail.ActiveCell.Text.Trim()=="" && this.grdDetail.ActiveCell.Column.Key==clsPOSDBConstants.ItemCompanion_Fld_ItemID && this.grdDetail.ActiveCell.Row.IsAddRow==true)
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

		private void frmCompanionItem_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (this.grdDetail.ContainsFocus==true)
				{
					if (this.grdDetail.ActiveCell!=null)
					{
						if (this.grdDetail.ActiveCell.Column.Key==clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID)
							if (e.KeyData == Keys.F4) 
								this.SearchItem();
					}
				}
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void frmCompanionItem_Load(object sender, System.EventArgs e)
		{
			this.txtCompanionItemCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtCompanionItemCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtItemCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtItemCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			
			clsUIHelper.SetAppearance(this.grdDetail);
			clsUIHelper.SetKeyActionMappings(this.grdDetail);
			clsUIHelper.setColorSchecme(this);
		}

		private void grdDetail_Enter(object sender, System.EventArgs e)
		{
			
			if (this.grdDetail.Rows.Count>0)
			{
				if (!m_FromError) 
				{
					if(this.grdDetail.Rows[0].Cells[0].Text.ToString()!="")
						this.grdDetail.ActiveCell=this.grdDetail.Rows[0].Cells[clsPOSDBConstants.ItemCompanion_Fld_Amount];
					else
						this.grdDetail.ActiveCell=this.grdDetail.Rows[0].Cells[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID];
				}
				this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
			}
			else
			{
				this.grdDetail.Rows.Band.AddNew();
			}
			m_FromError = false;
		}

		private void grdDetail_Leave(object sender, System.EventArgs e)
		{

//			grdDetail_AfterRowInsert(sender,null);
		}

		private void grdDetail_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{
		}

		private void grdDetail_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
		{
			m_FromError = true;
            #region PRIMEPOS-2923 13-Nov-2020 JY Added
            if (e.DataErrorInfo != null && e.DataErrorInfo.Cell.Column.Header.Caption.Trim().ToUpper() == "Percentage".ToUpper())
            {
                try
                {
                    e.Cancel = true;
                    string strErrorText = string.Empty;
                    if (e.DataErrorInfo.ErrorText == "Unable to update the data value: Input value does not satisfy maximum value constraint.")
                        strErrorText = "the value should not be greater than 100";
                    else
                        strErrorText = e.DataErrorInfo.ErrorText;

                    var ex = new Exception(string.Format("{0} - {1}", e.DataErrorInfo.Cell.Column.Header.Caption.Trim(), strErrorText));
                    ex.Data.Add(e.DataErrorInfo.Cell.Column.Header.Caption.Trim(), strErrorText);
                    throw ex;
                }
                catch (Exception Ex)
                {
                    clsUIHelper.ShowErrorMsg(Ex.Message);
                }
            }
            #endregion
            else
            {
                CommonUI.checkGridError((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID, clsPOSDBConstants.ItemCompanion_Fld_Amount);
                int index = e.ErrorText.IndexOf("\n");
                e.ErrorText = e.ErrorText.Substring(index + 1, (e.ErrorText.Length - index) - 1);
            }  
		}

		private string getColumnName(String ErrorText)
		{
			int StartIndex = ErrorText.IndexOf("Column '");
			int EndIndex = ErrorText.IndexOf("'",StartIndex+8);

			string columnName = ErrorText.Substring(StartIndex+8,EndIndex-StartIndex-8);
			return columnName;
		}

		private void grdDetail_CellDataError(object sender, Infragistics.Win.UltraWinGrid.CellDataErrorEventArgs e)
		{
			e.RaiseErrorEvent=true;
		}

		private void grdDetail_AfterCellActivate(object sender, System.EventArgs e)
		{
			this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
		}

		private void grdDetail_Validated(object sender, System.EventArgs e)
		{
			grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
		}

		private void frmCompanionItem_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void grdDetail_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            
        }

        private void grdDetail_AfterRowsDeleted(object sender, EventArgs e)
        {
            if (this.grdDetail.Rows.Count > 0)
            {
                this.grdDetail.ActiveRow = this.grdDetail.Rows[0];
                this.grdDetail.ActiveCell = this.grdDetail.ActiveRow.Cells[0];
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
            }
            else
            {
                this.grdDetail.DisplayLayout.Bands[0].AddNew();
            }
        }
	}
}
