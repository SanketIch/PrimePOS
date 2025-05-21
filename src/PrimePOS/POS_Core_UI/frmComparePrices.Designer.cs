namespace POS_Core_UI
{
    partial class frmComparePrices
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmComparePrices));
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Name", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Vendor Item Code", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Item Code", 2, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost Price", 4);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.groupBoxItemGrid = new System.Windows.Forms.GroupBox();
            this.ultraTxtEditorNoOfVendors = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLblNoOfVendors = new Infragistics.Win.Misc.UltraLabel();
            this.btnIncludeSelectedItem = new Infragistics.Win.Misc.UltraButton();
            this.gridItemDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraLblOrderDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.groupBoxItemGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtEditorNoOfVendors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridItemDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxItemGrid
            // 
            this.groupBoxItemGrid.Controls.Add(this.ultraTxtEditorNoOfVendors);
            this.groupBoxItemGrid.Controls.Add(this.ultraLblNoOfVendors);
            this.groupBoxItemGrid.Controls.Add(this.btnIncludeSelectedItem);
            this.groupBoxItemGrid.Controls.Add(this.gridItemDetails);
            this.groupBoxItemGrid.Controls.Add(this.ultraLblOrderDate);
            this.groupBoxItemGrid.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxItemGrid.ForeColor = System.Drawing.Color.White;
            this.groupBoxItemGrid.Location = new System.Drawing.Point(12, 46);
            this.groupBoxItemGrid.Name = "groupBoxItemGrid";
            this.groupBoxItemGrid.Size = new System.Drawing.Size(985, 299);
            this.groupBoxItemGrid.TabIndex = 7;
            this.groupBoxItemGrid.TabStop = false;
            this.groupBoxItemGrid.Text = "Best Prices";
            this.groupBoxItemGrid.Enter += new System.EventHandler(this.groupBoxItemGrid_Enter);
            // 
            // ultraTxtEditorNoOfVendors
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance1.ForeColor = System.Drawing.Color.White;
            this.ultraTxtEditorNoOfVendors.Appearance = appearance1;
            this.ultraTxtEditorNoOfVendors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ultraTxtEditorNoOfVendors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraTxtEditorNoOfVendors.Location = new System.Drawing.Point(130, 264);
            this.ultraTxtEditorNoOfVendors.Name = "ultraTxtEditorNoOfVendors";
            this.ultraTxtEditorNoOfVendors.ReadOnly = true;
            this.ultraTxtEditorNoOfVendors.Size = new System.Drawing.Size(100, 24);
            this.ultraTxtEditorNoOfVendors.TabIndex = 83;
            // 
            // ultraLblNoOfVendors
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLblNoOfVendors.Appearance = appearance2;
            this.ultraLblNoOfVendors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLblNoOfVendors.Location = new System.Drawing.Point(11, 264);
            this.ultraLblNoOfVendors.Name = "ultraLblNoOfVendors";
            this.ultraLblNoOfVendors.Size = new System.Drawing.Size(125, 23);
            this.ultraLblNoOfVendors.TabIndex = 82;
            this.ultraLblNoOfVendors.Text = "No. Of Vendors";
            // 
            // btnIncludeSelectedItem
            // 
            this.btnIncludeSelectedItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnIncludeSelectedItem.Appearance = appearance3;
            this.btnIncludeSelectedItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnIncludeSelectedItem.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncludeSelectedItem.Location = new System.Drawing.Point(860, 261);
            this.btnIncludeSelectedItem.Name = "btnIncludeSelectedItem";
            this.btnIncludeSelectedItem.Size = new System.Drawing.Size(110, 27);
            this.btnIncludeSelectedItem.TabIndex = 9;
            this.btnIncludeSelectedItem.Text = "Include";
            this.btnIncludeSelectedItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnIncludeSelectedItem.Click += new System.EventHandler(this.btnIncludeSelectedItem_Click);
            // 
            // gridItemDetails
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            this.gridItemDetails.DisplayLayout.Appearance = appearance4;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.gridItemDetails.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.gridItemDetails.DisplayLayout.GroupByBox.Hidden = true;
            this.gridItemDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.gridItemDetails.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.gridItemDetails.DisplayLayout.Override.CardAreaAppearance = appearance5;
            this.gridItemDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Arial";
            appearance6.FontData.SizeInPoints = 10F;
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.gridItemDetails.DisplayLayout.Override.HeaderAppearance = appearance6;
            this.gridItemDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.gridItemDetails.DisplayLayout.Override.RowSelectorAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(230)))), ((int)(((byte)(148)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(149)))), ((int)(((byte)(21)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.gridItemDetails.DisplayLayout.Override.SelectedRowAppearance = appearance8;
            this.gridItemDetails.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.gridItemDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.gridItemDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.gridItemDetails.Location = new System.Drawing.Point(11, 22);
            this.gridItemDetails.Name = "gridItemDetails";
            this.gridItemDetails.Size = new System.Drawing.Size(959, 233);
            this.gridItemDetails.TabIndex = 7;
            this.gridItemDetails.DoubleClick += new System.EventHandler(this.gridItemDetails_DoubleClick);
            this.gridItemDetails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridItemDetails_KeyDown);
            // 
            // ultraLblOrderDate
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            this.ultraLblOrderDate.Appearance = appearance9;
            this.ultraLblOrderDate.AutoSize = true;
            this.ultraLblOrderDate.Location = new System.Drawing.Point(216, 22);
            this.ultraLblOrderDate.Name = "ultraLblOrderDate";
            this.ultraLblOrderDate.Size = new System.Drawing.Size(90, 18);
            this.ultraLblOrderDate.TabIndex = 81;
            this.ultraLblOrderDate.Text = "Order Date";
            this.ultraLblOrderDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance10.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance10;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(12, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(979, 40);
            this.lblTransactionType.TabIndex = 36;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Best Prices";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            this.btnClose.Appearance = appearance11;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(908, 351);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 24);
            this.btnClose.TabIndex = 37;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmComparePrices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1017, 387);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBoxItemGrid);
            this.Name = "frmComparePrices";
            this.Text = "Best Prices";
            this.Load += new System.EventHandler(this.frmComparePrices_Load);
            this.groupBoxItemGrid.ResumeLayout(false);
            this.groupBoxItemGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtEditorNoOfVendors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridItemDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxItemGrid;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTxtEditorNoOfVendors;
        private Infragistics.Win.Misc.UltraLabel ultraLblNoOfVendors;
        private Infragistics.Win.Misc.UltraButton btnIncludeSelectedItem;
        private Infragistics.Win.UltraWinGrid.UltraGrid gridItemDetails;
        private Infragistics.Win.Misc.UltraLabel ultraLblOrderDate;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
    }
}