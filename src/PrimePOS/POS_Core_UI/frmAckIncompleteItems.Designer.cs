namespace POS_Core_UI
{
    partial class frmAckIncompleteItems
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemID", 0);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAckIncompleteItems));
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Qty", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Cost", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorCode", 4);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VendorItemID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("POVendorId", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderID", 7);
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNoOfItems = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLblNoOfItems = new Infragistics.Win.Misc.UltraLabel();
            this.btnItemEdit = new Infragistics.Win.Misc.UltraButton();
            this.btnSkipAllItems = new Infragistics.Win.Misc.UltraButton();
            this.grdItemDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoOfItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItemDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.FontData.SizeInPoints = 15F;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(991, 35);
            this.lblTransactionType.TabIndex = 9;
            this.lblTransactionType.Text = "Items Without Description";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.txtNoOfItems);
            this.groupBox1.Controls.Add(this.ultraLblNoOfItems);
            this.groupBox1.Controls.Add(this.btnItemEdit);
            this.groupBox1.Controls.Add(this.btnSkipAllItems);
            this.groupBox1.Location = new System.Drawing.Point(9, 369);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(968, 66);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // txtNoOfItems
            // 
            this.txtNoOfItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance3.ForeColor = System.Drawing.Color.White;
            this.txtNoOfItems.Appearance = appearance3;
            this.txtNoOfItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.txtNoOfItems.Enabled = false;
            this.txtNoOfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfItems.Location = new System.Drawing.Point(88, 13);
            this.txtNoOfItems.Name = "txtNoOfItems";
            this.txtNoOfItems.ReadOnly = true;
            this.txtNoOfItems.Size = new System.Drawing.Size(45, 21);
            this.txtNoOfItems.TabIndex = 15;
            // 
            // ultraLblNoOfItems
            // 
            this.ultraLblNoOfItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ultraLblNoOfItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold);
            this.ultraLblNoOfItems.Location = new System.Drawing.Point(6, 13);
            this.ultraLblNoOfItems.Name = "ultraLblNoOfItems";
            this.ultraLblNoOfItems.Size = new System.Drawing.Size(76, 21);
            this.ultraLblNoOfItems.TabIndex = 14;
            this.ultraLblNoOfItems.Text = "Total Items";
            // 
            // btnItemEdit
            // 
            this.btnItemEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            this.btnItemEdit.Appearance = appearance4;
            this.btnItemEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnItemEdit.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnItemEdit.Location = new System.Drawing.Point(560, 15);
            this.btnItemEdit.Name = "btnItemEdit";
            this.btnItemEdit.Size = new System.Drawing.Size(128, 41);
            this.btnItemEdit.TabIndex = 12;
            this.btnItemEdit.Text = "&Edit Item";
            this.btnItemEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnItemEdit.Click += new System.EventHandler(this.btnItemEdit_Click);
            // 
            // btnSkipAllItems
            // 
            this.btnSkipAllItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.Image = global::POS_Core_UI.Properties.Resources.close2;
            this.btnSkipAllItems.Appearance = appearance5;
            this.btnSkipAllItems.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSkipAllItems.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSkipAllItems.Location = new System.Drawing.Point(696, 15);
            this.btnSkipAllItems.Name = "btnSkipAllItems";
            this.btnSkipAllItems.Size = new System.Drawing.Size(128, 41);
            this.btnSkipAllItems.TabIndex = 11;
            this.btnSkipAllItems.Text = "Skip All Item(s)";
            this.btnSkipAllItems.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSkipAllItems.Click += new System.EventHandler(this.btnSkipAllItems_Click);
            // 
            // grdItemDetails
            // 
            this.grdItemDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance6.BackColor = System.Drawing.Color.White;
            this.grdItemDetails.DisplayLayout.Appearance = appearance6;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            ultraGridColumn1.CellButtonAppearance = appearance7;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.MinWidth = 18;
            ultraGridColumn1.Width = 140;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 200;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.MinWidth = 4;
            ultraGridColumn3.Width = 50;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Width = 80;
            ultraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn6.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.True;
            ultraGridColumn6.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            ultraGridColumn6.CellButtonAppearance = appearance8;
            ultraGridColumn6.Header.VisiblePosition = 4;
            ultraGridColumn6.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridColumn6.Width = 112;
            ultraGridColumn7.Header.VisiblePosition = 5;
            ultraGridColumn7.Width = 113;
            ultraGridColumn12.Header.VisiblePosition = 6;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn14.Header.VisiblePosition = 7;
            ultraGridColumn14.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn12,
            ultraGridColumn14});
            appearance9.FontData.SizeInPoints = 9F;
            ultraGridBand1.Header.Appearance = appearance9;
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridBand1.Override.TipStyleCell = Infragistics.Win.UltraWinGrid.TipStyle.Show;
            this.grdItemDetails.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdItemDetails.DisplayLayout.GroupByBox.Hidden = true;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdItemDetails.DisplayLayout.Override.AddRowAppearance = appearance10;
            this.grdItemDetails.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdItemDetails.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdItemDetails.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.grdItemDetails.DisplayLayout.Override.CardAreaAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BackColorDisabled = System.Drawing.Color.White;
            appearance12.BackColorDisabled2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Black;
            appearance12.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdItemDetails.DisplayLayout.Override.CellAppearance = appearance12;
            this.grdItemDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdItemDetails.DisplayLayout.Override.EditCellAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.FontData.BoldAsString = "True";
            appearance14.FontData.Name = "Arial";
            appearance14.FontData.SizeInPoints = 10F;
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdItemDetails.DisplayLayout.Override.HeaderAppearance = appearance14;
            this.grdItemDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(135)))), ((int)(((byte)(214)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(59)))), ((int)(((byte)(150)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdItemDetails.DisplayLayout.Override.RowSelectorAppearance = appearance15;
            this.grdItemDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(230)))), ((int)(((byte)(148)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(149)))), ((int)(((byte)(21)))));
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdItemDetails.DisplayLayout.Override.SelectedRowAppearance = appearance16;
            this.grdItemDetails.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdItemDetails.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdItemDetails.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance17.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdItemDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance17;
            this.grdItemDetails.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdItemDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdItemDetails.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdItemDetails.Location = new System.Drawing.Point(9, 41);
            this.grdItemDetails.Name = "grdItemDetails";
            this.grdItemDetails.Size = new System.Drawing.Size(968, 321);
            this.grdItemDetails.TabIndex = 15;
            this.grdItemDetails.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdItemDetails.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdItemDetails_InitializeRow);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Appearance = appearance2;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(832, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 41);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmAckIncompleteItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(991, 447);
            this.ControlBox = false;
            this.Controls.Add(this.grdItemDetails);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmAckIncompleteItems";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Items Without Description";
            this.Load += new System.EventHandler(this.frmAckIncompleteItems_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoOfItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdItemDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton btnSkipAllItems;
        private Infragistics.Win.Misc.UltraButton btnItemEdit;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtNoOfItems;
        private Infragistics.Win.Misc.UltraLabel ultraLblNoOfItems;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdItemDetails;
        private Infragistics.Win.Misc.UltraButton btnCancel;
    }
}