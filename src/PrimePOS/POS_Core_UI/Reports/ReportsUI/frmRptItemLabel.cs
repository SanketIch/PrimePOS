using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using POS_Core.CommonData;
using System.Data;
using POS_Core.BusinessRules;
using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
//using POS_Core.DataAccess;
using System.Data.SqlClient;
using Resources;
using POS_Core.Resources;
using POS_Core.LabelHandler;

namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmInventoryReports.
	/// </summary>
	public class frmRptItemLabel : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbInventoryReceived;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnPrint;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
		private Infragistics.Win.Misc.UltraButton btnDeleteItem;
		private Infragistics.Win.Misc.UltraButton btnAddItem;
		private DataSet oDS;
        private IDataAdapter da;
		private Mabry.Windows.Forms.Barcode.Barcode brLabel;
        private CheckBox chkUseLabelPrinter;
        POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
        VendorData vendorData = new VendorData();
        private CheckBox chkInstockItm;
        VendorSvr vendorSvr = new VendorSvr();
        private void fillVendorList()
        {
            string whereClause = " ORDER BY vendorname";
            vendorData = vendor.PopulateList(whereClause);
           /* dataGridList.DataSource = vendorData.Vendor;//.Select("1 = 1", "vendorname");
            for (int i = 1; i <= vendorData.Vendor.Columns.Count; i++)
            {
                dataGridList.Columns[i].ReadOnly = true;

                if (i != 3)
                    dataGridList.Columns[i].Visible = false;
                else
                {
                    dataGridList.Columns[i].Width = dataGridList.Width - dataGridList.Columns[0].Width - 20;
                    dataGridList.Columns[i].Name = "Vendors";
                }

            }*/
        }
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRptItemLabel()
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptItemLabel));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NoofLabels");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("NoofSheets");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ProductCode");
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
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.btnDeleteItem = new Infragistics.Win.Misc.UltraButton();
            this.btnAddItem = new Infragistics.Win.Misc.UltraButton();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkUseLabelPrinter = new System.Windows.Forms.CheckBox();
            this.brLabel = new Mabry.Windows.Forms.Barcode.Barcode();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.chkInstockItm = new System.Windows.Forms.CheckBox();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.chkInstockItm);
            this.gbInventoryReceived.Controls.Add(this.btnDeleteItem);
            this.gbInventoryReceived.Controls.Add(this.btnAddItem);
            this.gbInventoryReceived.Controls.Add(this.grdSearch);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(15, 56);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(554, 271);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnDeleteItem.Appearance = appearance1;
            this.btnDeleteItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnDeleteItem.Location = new System.Drawing.Point(275, 231);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(128, 26);
            this.btnDeleteItem.TabIndex = 15;
            this.btnDeleteItem.TabStop = false;
            this.btnDeleteItem.Text = "Delete Item ";
            this.btnDeleteItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnAddItem.Appearance = appearance2;
            this.btnAddItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnAddItem.Location = new System.Drawing.Point(411, 231);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(128, 26);
            this.btnAddItem.TabIndex = 14;
            this.btnAddItem.TabStop = false;
            this.btnAddItem.Text = "Add Item (F2)";
            this.btnAddItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // grdSearch
            // 
            this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSearch.DataMember = null;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.White;
            appearance31.BackColorDisabled = System.Drawing.Color.White;
            appearance31.BackColorDisabled2 = System.Drawing.Color.White;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance31;
            ultraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            ultraGridColumn1.Header.Caption = "Item Code";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(132, 0);
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            ultraGridColumn2.Header.Caption = "Item Name";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(215, 0);
            ultraGridColumn3.Format = "";
            ultraGridColumn3.Header.Caption = "No. of Labels";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.MaskInput = "###";
            ultraGridColumn3.NullText = "0";
            ultraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(84, 0);
            ultraGridColumn4.Header.Caption = "No. of Sheets";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.MaskInput = "";
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(84, 0);
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            ultraGridBand1.UseRowLayout = true;
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            this.grdSearch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSearch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance32.BackColor = System.Drawing.Color.White;
            appearance32.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.Color.White;
            appearance33.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.Color.White;
            appearance34.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance34;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance35.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance35;
            appearance36.BackColor = System.Drawing.Color.White;
            appearance36.BackColor2 = System.Drawing.Color.White;
            appearance36.BackColorDisabled = System.Drawing.Color.White;
            appearance36.BackColorDisabled2 = System.Drawing.Color.White;
            appearance36.BorderColor = System.Drawing.Color.Black;
            appearance36.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance36;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance37.BorderColor = System.Drawing.Color.Gray;
            appearance37.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance37.Image = ((object)(resources.GetObject("appearance37.Image")));
            appearance37.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance37.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance38.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance38;
            appearance39.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance39;
            appearance40.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance40;
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.Color.White;
            appearance41.BackColorDisabled = System.Drawing.Color.White;
            appearance41.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance42.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance42;
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.SystemColors.Control;
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance43.FontData.BoldAsString = "True";
            appearance43.ForeColor = System.Drawing.Color.Black;
            appearance43.TextHAlignAsString = "Left";
            appearance43.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance44;
            appearance45.BackColor = System.Drawing.Color.White;
            appearance45.BackColor2 = System.Drawing.Color.White;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance45.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance45.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance45;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.White;
            appearance47.BackColor2 = System.Drawing.SystemColors.Control;
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance47.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance47;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance48.BackColor = System.Drawing.Color.Navy;
            appearance48.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance48;
            appearance49.BackColor = System.Drawing.Color.Navy;
            appearance49.BackColorDisabled = System.Drawing.Color.Navy;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance49.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance49.BorderColor = System.Drawing.Color.Gray;
            appearance49.ForeColor = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance49;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            appearance50.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance50;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance51.BackColor = System.Drawing.Color.White;
            appearance51.BackColor2 = System.Drawing.SystemColors.Control;
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance51;
            appearance52.BackColor = System.Drawing.Color.White;
            appearance52.BackColor2 = System.Drawing.SystemColors.Control;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance52.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance52;
            appearance53.BackColor = System.Drawing.Color.White;
            appearance53.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance53;
            appearance54.BackColor = System.Drawing.Color.White;
            appearance54.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance54;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(8, 16);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(531, 206);
            this.grdSearch.TabIndex = 2;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.Enter += new System.EventHandler(this.grdSearch_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkUseLabelPrinter);
            this.groupBox2.Controls.Add(this.brLabel);
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 328);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(554, 57);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            // 
            // chkUseLabelPrinter
            // 
            this.chkUseLabelPrinter.AutoSize = true;
            this.chkUseLabelPrinter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkUseLabelPrinter.Location = new System.Drawing.Point(8, 22);
            this.chkUseLabelPrinter.Name = "chkUseLabelPrinter";
            this.chkUseLabelPrinter.Size = new System.Drawing.Size(142, 21);
            this.chkUseLabelPrinter.TabIndex = 9;
            this.chkUseLabelPrinter.Text = "Use Label Printer";
            this.chkUseLabelPrinter.UseVisualStyleBackColor = true;
            this.chkUseLabelPrinter.CheckedChanged += new System.EventHandler(this.chkUseLabelPrinter_CheckedChanged);
            // 
            // brLabel
            // 
            this.brLabel.BackColor = System.Drawing.Color.White;
            this.brLabel.BarColor = System.Drawing.Color.Black;
            this.brLabel.BarRatio = 2F;
            this.brLabel.Data = null;
            this.brLabel.DataExtension = null;
            this.brLabel.DisplayData = false;
            this.brLabel.DisplayStartStop = false;
            this.brLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brLabel.Location = new System.Drawing.Point(148, 20);
            this.brLabel.Name = "brLabel";
            this.brLabel.Size = new System.Drawing.Size(75, 23);
            this.brLabel.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128;
            this.brLabel.TabIndex = 8;
            this.brLabel.Visible = false;
            // 
            // btnPrint
            // 
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance27.FontData.BoldAsString = "True";
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            this.btnPrint.Appearance = appearance27;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(270, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click_1);
            // 
            // btnClose
            // 
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance28.FontData.BoldAsString = "True";
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.Image = ((object)(resources.GetObject("appearance28.Image")));
            this.btnClose.Appearance = appearance28;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(454, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance29.FontData.BoldAsString = "True";
            appearance29.ForeColor = System.Drawing.Color.White;
            appearance29.Image = ((object)(resources.GetObject("appearance29.Image")));
            this.btnView.Appearance = appearance29;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(362, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTransactionType
            // 
            appearance30.ForeColor = System.Drawing.Color.White;
            appearance30.ForeColorDisabled = System.Drawing.Color.White;
            appearance30.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance30;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(15, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 30);
            this.lblTransactionType.TabIndex = 31;
            this.lblTransactionType.Text = "Item Label Report";
            // 
            // chkInstockItm
            // 
            this.chkInstockItm.AutoSize = true;
            this.chkInstockItm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInstockItm.Location = new System.Drawing.Point(8, 240);
            this.chkInstockItm.Name = "chkInstockItm";
            this.chkInstockItm.Size = new System.Drawing.Size(148, 17);
            this.chkInstockItm.TabIndex = 16;
            this.chkInstockItm.Text = "InStock Items Only";
            this.chkInstockItm.UseVisualStyleBackColor = true;
            // 
            // frmRptItemLabel
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(585, 403);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbInventoryReceived);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptItemLabel";
            this.ShowInTaskbar = false;
            this.Text = "Item Label Report";
            this.Load += new System.EventHandler(this.frmInventoryReports_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptInventoryReceived_KeyDown);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmInventoryReports_Load(object sender, System.EventArgs e)
		{
			Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
			clsUIHelper.setColorSchecme(this);
			clsUIHelper.SetKeyActionMappings(this.grdSearch);

			grdSearch.DisplayLayout.Bands[0].Columns[2].CellAppearance.TextHAlign=HAlign.Right;
			grdSearch.DisplayLayout.Bands[0].Columns[2].MaxValue = 999;
			grdSearch.DisplayLayout.Bands[0].Columns[2].MaskInput="999";
			grdSearch.DisplayLayout.Bands[0].Columns[2].Format = "##0";
			
			grdSearch.DisplayLayout.Bands[0].Columns[3].CellAppearance.TextHAlign=HAlign.Right;
			grdSearch.DisplayLayout.Bands[0].Columns[3].MaxValue = 999;
			grdSearch.DisplayLayout.Bands[0].Columns[3].MaskInput="999";
			grdSearch.DisplayLayout.Bands[0].Columns[3].Format = "##0";

			setNew();
            fillVendorList();
		}

		private void setNew()
		{
			oDS=new DataSet();
			oDS.Tables.Add();
			oDS.Tables[0].Columns.Add("ItemID",typeof(System.String));
			oDS.Tables[0].Columns.Add("ItemName",typeof(System.String));
			oDS.Tables[0].Columns.Add("NoofLabels",typeof(System.String));
			oDS.Tables[0].Columns.Add("NoofSheets",typeof(System.String));
            oDS.Tables[0].Columns.Add("SellingPrice", typeof(System.Decimal));
            oDS.Tables[0].Columns.Add("ProductCode", typeof(System.String));
            oDS.Tables[0].Columns.Add("Location", typeof(System.String));
            oDS.Tables[0].Columns.Add("VendorCode", typeof(System.String));
            oDS.Tables[0].Columns.Add("VendorID", typeof(System.String));
            oDS.Tables[0].Columns.Add("PCKSIZE", typeof(System.String));
            oDS.Tables[0].Columns.Add("PCKQTY", typeof(System.String));
            oDS.Tables[0].Columns.Add("PCKUNIT", typeof(System.String));
            oDS.Tables[0].Columns.Add("DesireQuantity", typeof(System.String));
            oDS.Tables[0].Columns.Add("VendorItemID", typeof(System.String));

            oDS.Tables[0].Columns.Add("ManufacturerName", typeof(System.String));
            oDS.Tables[0].Columns.Add("AvgPrice", typeof(System.Decimal));
            oDS.Tables[0].Columns.Add("OnSalePrice", typeof(System.Decimal));
            //oDS.Tables[0].Columns.Add("ProductCode", typeof(System.String));

            #region Sprint-21 - 08-Sep-2015 JY Added sale price related objects
            oDS.Tables[0].Columns.Add("isOnSale", typeof(System.Boolean));
            oDS.Tables[0].Columns.Add("SaleLimitQty", typeof(System.Int32));
            oDS.Tables[0].Columns.Add("SaleStartDate", typeof(System.DateTime));
            oDS.Tables[0].Columns.Add("SaleEndDate", typeof(System.DateTime));
            #endregion

            this.grdSearch.DataSource=oDS;
            grdSearch.DisplayLayout.Bands[0].Columns["SellingPrice"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["ProductCode"].Hidden = true;
            grdSearch.DisplayLayout.Bands[0].Columns["Location"].Hidden = false;    //PRIMEPOS-3064 18-Mar-2022 JY modified
        }

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			Preview(true);
		}

        ItemVendorData itemVendorData = null;
		private void Preview(bool PrintId)
		{
			try
			{
				rptItemLabel oRpt= new rptItemLabel();
                itemVendorData = new ItemVendorData();
				DataSet rptDS=new DataSet();
				rptDS.Tables.Add();
				rptDS.Tables[0].Columns.Add("ItemID");
				rptDS.Tables[0].Columns.Add("Description");
				rptDS.Tables[0].Columns.Add("Picture",System.Type.GetType("System.Byte[]"));
                rptDS.Tables[0].Columns.Add("SellingPrice");
                rptDS.Tables[0].Columns.Add("ProductCode");
                rptDS.Tables[0].Columns.Add("Location");
                rptDS.Tables[0].Columns.Add("VendorCode");
                rptDS.Tables[0].Columns.Add("VendorID");
                rptDS.Tables[0].Columns.Add("PCKSIZE");
                rptDS.Tables[0].Columns.Add("PCKQTY");
                rptDS.Tables[0].Columns.Add("PCKUNIT");
                //Added By Shitaljit(QuicSolv) on 17 Nov 2011
                rptDS.Tables[0].Columns.Add("VendorItemID");
                rptDS.Tables[0].Columns.Add("VendorItemIDBarcode", System.Type.GetType("System.Byte[]"));
                //End of Added By Shitaljit(QuicSolv) on 17 Nov 2011

                rptDS.Tables[0].Columns.Add("DesireQuantity");//Added By Shitaljit on 10 April 2013 for PRIMEPOS-776

                rptDS.Tables[0].Columns.Add("ManufacturerName");//Added By Ravindra on 30 Sep 2014 for PRIMEPOS-2047
                rptDS.Tables[0].Columns.Add("AvgPrice");//Added By Ravindra on 30 Sep 2014 for PRIMEPOS-2047
                rptDS.Tables[0].Columns.Add("OnSalePrice");//Added By Ravindra on 30 Sep 2014 for PRIMEPOS-2047
                rptDS.Tables[0].Columns.Add("SKUCode");//Added By Ravindra on 30 Sep 2014 for PRIMEPOS-2047

                #region Sprint-21 - 08-Sep-2015 JY Added sale price related objects
                rptDS.Tables[0].Columns.Add("isOnSale");
                rptDS.Tables[0].Columns.Add("SaleLimitQty");
                rptDS.Tables[0].Columns.Add("SaleStartDate");
                rptDS.Tables[0].Columns.Add("SaleEndDate");
                #endregion

                //ManufacturerName
                //    AvgPrice
                //    OnSalePrice
                //        ProductCode
                frmSearchMain ofrm = new frmSearchMain();
				foreach(DataRow oRow in oDS.Tables[0].Rows)
				{
					try	
					{
                        Configuration.PrintBarcode(oRow[0].ToString(), 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp");
                        itemVendorData = GetTargetedVenorInfo(oRow["ItemID"].ToString());
                        if (itemVendorData != null)
                        {
                            if (itemVendorData.Tables.Count > 0 && itemVendorData.ItemVendor.Rows.Count > 0)
                            {
                                Configuration.PrintBarcode(oRow["VendorItemID"].ToString(), 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + oRow["vendorItemID"].ToString() + ".bmp");
                            }
                        }
                         
						//brLabel.Data=
						//brLabel.Refresh();
                        //brLabel.Image().Save(System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
					}
					catch(Exception ){}

                    string itemName = oRow[1].ToString();
                    if (itemName.Length > 25)
                    {
                        itemName = itemName.Substring(0, 25);
                    }

                    int noOfLabels = Configuration.convertNullToInt(oRow[3].ToString());
                    //for (int i=0; i< Convert.ToInt32( oRow[3].ToString())*Configuration.LabelPerSheet;i++)
                    for (int i = 0; i < noOfLabels * Configuration.LabelPerSheet; i++)
					{
                        itemVendorData = GetTargetedVenorInfo(oRow["ItemID"].ToString());
                        //DataSet itempacksDS = GetPckData(oRow["ItemID"].ToString());
                        if (itemVendorData != null && itemVendorData.Tables.Count > 0 && itemVendorData.ItemVendor.Rows.Count > 0)
                        {
                            // if (itempacksDS != null)
                            rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itemVendorData.ItemVendor.Rows[0]["PCKSIZE"].ToString(), itemVendorData.ItemVendor.Rows[0]["PCKQTY"].ToString(), itemVendorData.ItemVendor.Rows[0]["PCKUNIT"].ToString(), oRow["VendorItemID"].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["VendorItemID"].ToString() + ".bmp"), Configuration.convertNullToString(oRow["DesireQuantity"].ToString()), Configuration.convertNullToString(oRow["ManufacturerName"].ToString()), Configuration.convertNullToDecimal(oRow["AvgPrice"].ToString()).ToString("AvgPrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()).ToString("OnSalePrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToString(oRow["ProductCode"].ToString()), Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });  //Sprint-21 - 10-Sep-2015 JY Added sale price related objects
                            //else
                            //   rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price $#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty });
                        }
                        else
                        {
                            //rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, Configuration.convertNullToString(oRow["DesireQuantity"].ToString()), Configuration.convertNullToString(oRow["ManufacturerName"].ToString()), Configuration.convertNullToDecimal(oRow["AvgPrice"].ToString()).ToString("AvgPrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()).ToString("OnSalePrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToString(oRow["ProductCode"].ToString()), Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });    //Sprint-21 - 10-Sep-2015 JY Added sale price related objects
                            rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, Configuration.convertNullToString(oRow["DesireQuantity"].ToString()), Configuration.convertNullToString(oRow["ManufacturerName"].ToString()), Configuration.convertNullToDecimal(oRow["AvgPrice"].ToString()).ToString("AvgPrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()).ToString("OnSalePrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToString(oRow["ProductCode"].ToString()), Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });    //Sprint-21 - 10-Sep-2015 JY Added sale price related objects   //PRIMEPOS-2909 16-Oct-2020 JY modified
                        }
					}

                    int noOfSheets = Configuration.convertNullToInt(oRow[2].ToString());
                    //for (int i=0; i< Convert.ToInt32( oRow[2].ToString());i++)
                    for (int i = 0; i < noOfSheets; i++)
					{
                        itemVendorData = GetTargetedVenorInfo(oRow["ItemID"].ToString());
                        //DataSet itempacksDS = GetPckData(oRow["ItemID"].ToString());
                        if (itemVendorData != null && itemVendorData.Tables.Count > 0 && itemVendorData.ItemVendor.Rows.Count > 0)
                        {
                            //if (itempacksDS != null)
                            rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), itemVendorData.ItemVendor.Rows[0]["PCKSIZE"].ToString(), itemVendorData.ItemVendor.Rows[0]["PCKQTY"].ToString(), itemVendorData.ItemVendor.Rows[0]["PCKUNIT"].ToString(), oRow["VendorItemID"].ToString(), GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow["VendorItemID"].ToString() + ".bmp"), Configuration.convertNullToString(oRow["DesireQuantity"].ToString()), Configuration.convertNullToString(oRow["ManufacturerName"].ToString()), Configuration.convertNullToDecimal(oRow["AvgPrice"].ToString()).ToString("AvgPrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()).ToString("OnSalePrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToString(oRow["ProductCode"].ToString()), Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });  //Sprint-21 - 10-Sep-2015 JY Added sale price related objects
                            //else
                            //   rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price $#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorCode"].ToString(), itemVendorData.ItemVendor.Rows[0]["VendorID"].ToString(), string.Empty, string.Empty, string.Empty });
                        }
                        else
                        {
                            //rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, Configuration.convertNullToString(oRow["DesireQuantity"].ToString()), Configuration.convertNullToString(oRow["ManufacturerName"].ToString()), Configuration.convertNullToDecimal(oRow["AvgPrice"].ToString()).ToString("AvgPrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()).ToString("OnSalePrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToString(oRow["ProductCode"].ToString()), Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] }); //Sprint-21 - 10-Sep-2015 JY Added sale price related objects
                            rptDS.Tables[0].Rows.Add(new object[] { oRow[0].ToString(), itemName, GetImageData(System.IO.Path.GetTempPath() + "\\" + oRow[0].ToString() + ".bmp"), Configuration.convertNullToDecimal(oRow["sellingprice"].ToString()).ToString("Price " + Configuration.CInfo.CurrencySymbol + "#####0.00"), oRow["ProductCode"].ToString(), oRow["Location"].ToString(), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, Configuration.convertNullToString(oRow["DesireQuantity"].ToString()), Configuration.convertNullToString(oRow["ManufacturerName"].ToString()), Configuration.convertNullToDecimal(oRow["AvgPrice"].ToString()).ToString("AvgPrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToDecimal(oRow["OnSalePrice"].ToString()).ToString("OnSalePrice " + Configuration.CInfo.CurrencySymbol + "#####0.00"), Configuration.convertNullToString(oRow["ProductCode"].ToString()), Configuration.convertNullToBoolean(oRow["isOnSale"]), Configuration.convertNullToInt(oRow["SaleLimitQty"]), oRow["SaleStartDate"], oRow["SaleEndDate"] });  //Sprint-21 - 10-Sep-2015 JY Added sale price related objects    //PRIMEPOS-2909 16-Oct-2020 JY modified
                        }
					}
				}

                if (chkUseLabelPrinter.Checked == true)
                {
                    BarcodeLabelDataCollection oBarcodeColl = new BarcodeLabelDataCollection();
                    Item oItem = new Item();
                    foreach (DataRow oRow in rptDS.Tables[0].Rows)
                    {
                        BarcodeLabelData oBarcode = new BarcodeLabelData();

                        oBarcode.Item= oItem.FindItemByID(oRow["ItemID"].ToString());
                        oBarcode.ItemCode= oRow["ItemID"].ToString();
                        oBarcode.ItemCode2 = oRow["ProductCode"].ToString();
                        oBarcode.ItemName = oRow["Description"].ToString();
						oBarcode.SellingPrice = Configuration.convertNullToDecimal(oRow["SellingPrice"].ToString().Replace("Price", "").Replace(Configuration.CInfo.CurrencySymbol.ToString(), "").Trim());
                        oBarcode.BarCode =Image.FromFile( System.IO.Path.GetTempPath() + "\\" + oRow["ItemID"].ToString() + ".bmp");
                        oBarcode.IsItemFSA= oItem.IsIIASItem(oBarcode.ItemCode);
                        oBarcode.FineLineCode = oItem.GetFineLineCode(oBarcode.ItemCode);
                        oBarcode.Location = oRow["Location"].ToString();
                        oBarcode.VendorCode = oRow["VendorCode"].ToString(); //Added by SRT (Abhishek D) Date : 04 April 2010
                        oBarcode.VendorID = oRow["VendorID"].ToString();
                        oBarcode.PckSize = oRow["PCKSIZE"].ToString();
                        oBarcode.PckQty = oRow["PCKQTY"].ToString();
                        oBarcode.PckUnit = oRow["PCKUNIT"].ToString();

                        oBarcode.ManufacturerName = oRow["ManufacturerName"].ToString();
                        oBarcode.AvgPrice = oRow["AvgPrice"].ToString();
                        oBarcode.ProductCode = oRow["SKUCode"].ToString();
                        oBarcode.OnSalePrice = oRow["OnSalePrice"].ToString();

                        #region Sprint-21 - 08-Sep-2015 JY Added sale price related objects
                        oBarcode.IsOnSale = Configuration.convertNullToBoolean(oRow["IsOnSale"]);
                        oBarcode.SaleLimitQty = Configuration.convertNullToInt(oRow["SaleLimitQty"]);
                        oBarcode.SaleStartDate = (oRow["SaleStartDate"].ToString() == string.Empty ? DateTime.MinValue : Convert.ToDateTime(oRow["SaleStartDate"]));
                        oBarcode.SaleEndDate = (oRow["SaleEndDate"].ToString() == string.Empty ? DateTime.MinValue : Convert.ToDateTime(oRow["SaleEndDate"]));
                        #endregion

                        oBarcode.DesireQuantity =Configuration.convertNullToString(oRow["DesireQuantity"].ToString());//Added By Shitaljit on 10 April 2013 for PRIMEPOS-776
                        oBarcodeColl.Add(oBarcode);
                    }

                    BarcodeLabel oBarcodeLabel=new BarcodeLabel(oBarcodeColl);
                    oBarcodeLabel.Print();
                }
                else
                {
                    rptDS.Tables[0].Columns.Remove("Location");
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
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private ItemVendorData GetTargetedVenorInfo(String itemCode)
        {
            Item item = new Item();
            ItemVendor itemVendor = new ItemVendor();
            ItemVendorData itemVendorData = null;
            DataRow[] vendorRows = null;
            string vendrorID = string.Empty;
            string lastOrderVendor = string.Empty;
            string preferredVendor = string.Empty;
            bool isItemExistForPrefVend = false;
            bool isItemExistForLastVend = false;
            bool isItemExistForDefVend = false;
            string sqlSelectPreferredVendor = "SELECT PREFERREDVENDOR FROM ITEM WHERE LTRIM(ITEMID) = '{0}'";
            string sqlSelectLastVendor = "SELECT LASTVENDOR FROM ITEM WHERE RTRIM(ITEMID) = '{0}'";
            Search oBLSearch = new Search();
            try
            {
                preferredVendor = oBLSearch.SearchScalar(String.Format(sqlSelectPreferredVendor, new object[] { itemCode }));
                lastOrderVendor = oBLSearch.SearchScalar(String.Format(sqlSelectLastVendor, new object[] { itemCode }));

                if (preferredVendor != string.Empty)
                {
                    vendorRows = vendorData.Vendor.Select("VendorCode ='" + preferredVendor + "'");
                    vendrorID = vendorRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    itemVendorData = itemVendor.PopulateList(" AND LTRIM(RTRIM(" + clsPOSDBConstants.ItemVendor_tbl + ".ItemID)) = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorID = " + vendrorID);
                    if (itemVendorData.ItemVendor.Rows.Count == 0)
                    {
                        isItemExistForPrefVend = false;
                    }
                    else
                    {
                        isItemExistForPrefVend = true;
                    }
                }
                if (lastOrderVendor != string.Empty && !isItemExistForPrefVend)
                {
                    //vendorRows = vendorData.Vendor.Select("VendorCode ='" + preferredVendor + "'");
                    //vendrorID = vendorRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                    POS_Core.BusinessRules.Vendor tempVendor = new POS_Core.BusinessRules.Vendor();
                    itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorId = '" + tempVendor.GetVendorId(lastOrderVendor).ToString() + "'");

                    if (itemVendorData.ItemVendor.Rows.Count == 0)
                    {
                        isItemExistForLastVend = false;
                    }
                    else
                    {
                        isItemExistForLastVend = true;
                    }
                }
                if (Configuration.CPrimeEDISetting.DefaultVendor != string.Empty && !isItemExistForLastVend && !isItemExistForPrefVend) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                {
                    DataRow[] defaultVendorRows = vendorData.Vendor.Select("VendorCode ='" + Configuration.CPrimeEDISetting.DefaultVendor + "'");
                    if (defaultVendorRows.Length > 0)
                    {
                        String defaultVendorId = defaultVendorRows[0][clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                        itemVendorData = itemVendor.PopulateList(" AND LTRIM(RTRIM(" + clsPOSDBConstants.ItemVendor_tbl + ".ItemID)) = '" + itemCode + "' AND " + clsPOSDBConstants.ItemVendor_tbl + ".VendorID = " + defaultVendorId);
                        if (itemVendorData.ItemVendor.Rows.Count == 0)
                        {
                            isItemExistForDefVend = false;
                        }
                        else
                        {
                            isItemExistForDefVend = true;
                        }
                    }
                    
                }
                if (!isItemExistForDefVend && !isItemExistForLastVend && !isItemExistForPrefVend)
                {
                    itemVendorData = itemVendor.PopulateList(" AND " + clsPOSDBConstants.ItemVendor_tbl + ".ItemID like '" + itemCode + "%'");
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            return itemVendorData;
        }

		private void frmRptInventoryReceived_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter && this.grdSearch.ContainsFocus==false)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				else if (e.KeyData==System.Windows.Forms.Keys.F2)
				{
					SearchItem();
					e.Handled=true;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void gbInventoryReceived_Enter(object sender, System.EventArgs e)
		{
			
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			Preview(false);
		}

		private void btnPrint_Click_1(object sender, System.EventArgs e)
		{
			Preview(true);
		}

		private void btnAddItem_Click(object sender, System.EventArgs e)
		{
			SearchItem();
		}

		private void SearchItem()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl,"--##--","");
                frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Item_tbl, "--##--", "", true);  //20-Dec-2017 JY Added new reference
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
					{
						return;
					}
					System.Windows.Forms.Application.DoEvents();
					this.Cursor=Cursors.WaitCursor;
					FKEdit(strCode,clsPOSDBConstants.Item_tbl);
					this.Cursor=Cursors.Default;
				}
			}
			catch(Exception exp)
			{
				this.Cursor=Cursors.Default;
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void FKEdit(string code,string senderName)
		{
			if (senderName==clsPOSDBConstants.Item_tbl)
			{
				#region Items
				try
				{
					Item oItem=new  Item();
					ItemData oItemData;
					ItemRow oItemRow=null;
					oItemData = oItem.Populate(code);
					oItemRow = oItemData.Item[0];
					if (oItemRow!=null)
					{
                        if (oItemRow.QtyInStock > 0 || !chkInstockItm.Checked)
                        {
                            string itemVendorID = string.Empty;
                            ItemVendorData itemVendorData = GetTargetedVenorInfo(oItemRow.ItemID);
                            if (itemVendorData != null)
                            {
                                if (itemVendorData.Tables.Count > 0 && itemVendorData.ItemVendor.Rows.Count > 0)
                                {
                                    ItemVendorRow oRow = (ItemVendorRow)itemVendorData.ItemVendor.Rows[0];
                                    itemVendorID = oRow.VendorItemID;
                                }
                            }
                            oDS.Tables[0].Rows.Add(new object[] { oItemRow.ItemID, oItemRow.Description, 0, 0, oItemRow.SellingPrice, oItemRow.ProductCode, oItemRow.Location,
                            string.Empty,string.Empty, oItemRow.PckSize,oItemRow.PckQty, oItemRow.PckUnit,oItemRow.MinOrdQty,itemVendorID,oItemRow.ManufacturerName,oItemRow.AvgPrice,oItemRow.OnSalePrice, oItemRow.isOnSale, oItemRow.SaleLimitQty, oItemRow.SaleStartDate, oItemRow.SaleEndDate});   //Sprint-21 - 08-Sep-2015 JY Added sale price related objects
                            this.grdSearch.Focus();
                            this.grdSearch.ActiveRow = this.grdSearch.Rows[this.grdSearch.Rows.Count - 1];
                            this.grdSearch.ActiveCell = this.grdSearch.ActiveRow.Cells[2];
                            this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("Item is not present in stock");
                        }
					}
				}
				catch(System.IndexOutOfRangeException )
				{
					this.grdSearch.ActiveCell.Value=String.Empty;
					this.grdSearch.ActiveRow.Cells["Description"].Value=String.Empty;
				}
				catch(Exception exp)
				{
					clsUIHelper.ShowErrorMsg(exp.Message);
					exp=null;
					this.grdSearch.ActiveCell.Value=String.Empty;
					this.grdSearch.ActiveRow.Cells["Description"].Value=String.Empty;
				}
				#endregion
			}
		
		}

		private void btnDeleteItem_Click(object sender, System.EventArgs e)
		{
			if (this.grdSearch.ActiveRow!=null)
			{
				this.grdSearch.DeleteSelectedRows(true);
			}
		}

		private void grdSearch_Enter(object sender, System.EventArgs e)
		{
			try
			{
				if (this.grdSearch.Rows.Count>0)
				{
					this.grdSearch.Rows[0].Cells[2].Activate();
					this.grdSearch.PerformAction(UltraGridAction.EnterEditMode);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private byte[] GetImageData(String fileName )
		{
			//'Method to load an image from disk and return it as a bytestream
			System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
			return (br.ReadBytes(Convert.ToInt32(br.BaseStream.Length)));
		}

        private void chkUseLabelPrinter_CheckedChanged(object sender, EventArgs e)
        {
            this.btnView.Enabled = this.chkUseLabelPrinter.Checked==false;
        }

        private DataSet GetPckData(string ItemID)
        {
            DataSet dsItem = new DataSet();
            try
            {
                //rptItemPriceChangeLog oRpt = new rptItemPriceChangeLog();


                IDbCommand cmd = DataFactory.CreateCommand();
                string sSQL = "";
                string Criteria = "";


                IDbConnection conn = DataFactory.CreateConnection();

                conn.ConnectionString = Configuration.ConnectionString;

                conn.Open();

                try
                {
                    sSQL = " SELECT " +
                                  " PCKSIZE " +
                                  " ,PCKQTY " +
                                  " ,PCKUNIT " +
                              " FROM  Item" +
                              " WHERE Item.itemID = '" + ItemID + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sSQL;
                    cmd.Connection = conn;
                    SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                    sqlDa.SelectCommand = (SqlCommand)cmd;
                    da.Fill(dsItem);
                    //da.Fill(ds);
                    conn.Close();




                    //grdSearch1.DataSource = ds;
                }
                catch (NullReferenceException)
                {
                    conn.Close();
                    //ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
                }
                catch (Exception exp)
                {
                    conn.Close();
                    throw (exp);
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return dsItem;
        }

	}

    public class BarcodeLabelDataCollection : System.Collections.Generic.List<BarcodeLabelData>
    {
    }

    public class BarcodeLabelData
    {
        public BarcodeLabelData()
        {
        }

        public string ItemCode;        
        public string ItemName;
        public decimal SellingPrice;
        public Image BarCode;
        public string ItemCode2;
        public bool IsItemFSA;
        public string Location;
        public int FineLineCode;
        public string VendorCode; //Added by SRT (Abhishek D ) Date : 04 April 2010
        public string VendorID;
        public string PckSize;
        public string PckQty;
        public string PckUnit;
        public ItemRow Item;
        //Added by Amit Date 
        public string VendorItemID;
        public Image VendorItemIDBarCode;
        public string DesireQuantity;//Added By Shitaljit on 10 April 2013 for PRIMEPOS-776
        //From Here added by Ravindra
        public string ManufacturerName;
        public string AvgPrice;
        public string ProductCode ;  //SKU Code
        public string OnSalePrice;
        #region Sprint-21 - 08-Sep-2015 JY Added sale price related objects
        public bool IsOnSale;
        public int SaleLimitQty;
        public DateTime SaleStartDate;
        public DateTime SaleEndDate;
        #endregion
        public string VendorName;   //PRIMEPOS-2758 11-Nov-2019 JY Added 
    }
}
