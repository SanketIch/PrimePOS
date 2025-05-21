namespace POS_Core_UI
{
    partial class frmSalesPaymentHistory
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn52 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PayTypeDesc");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn53 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Amount");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn54 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RefNo");
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.grdPaymentDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tlpMain.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(4, 4);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Padding = new System.Drawing.Size(10, 0);
            this.lblTransactionType.Size = new System.Drawing.Size(526, 36);
            this.lblTransactionType.TabIndex = 3;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Sales Payment History";
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tlpBottom, 0, 2);
            this.tlpMain.Controls.Add(this.lblTransactionType, 0, 0);
            this.tlpMain.Controls.Add(this.grdPaymentDetails, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(5);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.47619F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.52381F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(534, 211);
            this.tlpMain.TabIndex = 1;
            this.tlpMain.Tag = "";
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 2;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 405F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tlpBottom.Controls.Add(this.btnClose, 1, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(4, 172);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(526, 35);
            this.tlpBottom.TabIndex = 12;
            // 
            // btnClose
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance3;
            this.btnClose.Location = new System.Drawing.Point(408, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 29);
            this.btnClose.TabIndex = 4;
            this.btnClose.Tag = "NOCOLOR";
            this.btnClose.Text = "Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdPaymentDetails
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BackColorDisabled = System.Drawing.Color.White;
            appearance4.BackColorDisabled2 = System.Drawing.Color.White;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdPaymentDetails.DisplayLayout.Appearance = appearance4;
            this.grdPaymentDetails.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.CardSettings.AutoFit = true;
            ultraGridColumn52.Header.Caption = "Payment Type";
            ultraGridColumn52.Header.VisiblePosition = 0;
            ultraGridColumn52.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn52.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn52.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(135, 0);
            ultraGridColumn52.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn52.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn53.CellMultiLine = Infragistics.Win.DefaultableBoolean.False;
            ultraGridColumn53.DefaultCellValue = "";
            ultraGridColumn53.Header.VisiblePosition = 1;
            ultraGridColumn53.MaxLength = 10;
            ultraGridColumn53.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn53.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn53.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(114, 0);
            ultraGridColumn53.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn53.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn54.Header.Caption = "Ref No / CC No";
            ultraGridColumn54.Header.VisiblePosition = 2;
            ultraGridColumn54.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn54.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn54.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(205, 0);
            ultraGridColumn54.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn54.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn52,
            ultraGridColumn53,
            ultraGridColumn54});
            ultraGridBand1.Override.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.FormattedText;
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdPaymentDetails.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPaymentDetails.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPaymentDetails.DisplayLayout.InterBandSpacing = 10;
            this.grdPaymentDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPaymentDetails.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            this.grdPaymentDetails.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.AddRowAppearance = appearance7;
            this.grdPaymentDetails.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPaymentDetails.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdPaymentDetails.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdPaymentDetails.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdPaymentDetails.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentDetails.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
            this.grdPaymentDetails.DisplayLayout.Override.AllowGroupSwapping = Infragistics.Win.UltraWinGrid.AllowGroupSwapping.NotAllowed;
            this.grdPaymentDetails.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentDetails.DisplayLayout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this.grdPaymentDetails.DisplayLayout.Override.AllowRowLayoutLabelSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this.grdPaymentDetails.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.grdPaymentDetails.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BackColorDisabled = System.Drawing.Color.White;
            appearance9.BackColorDisabled2 = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.Black;
            appearance9.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdPaymentDetails.DisplayLayout.Override.CellAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance10.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            appearance10.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance10.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdPaymentDetails.DisplayLayout.Override.CellButtonAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPaymentDetails.DisplayLayout.Override.EditCellAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.FilteredInRowAppearance = appearance12;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.FilteredOutRowAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BackColorDisabled = System.Drawing.Color.White;
            appearance14.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdPaymentDetails.DisplayLayout.Override.FixedCellAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance15.BackColor2 = System.Drawing.Color.Beige;
            this.grdPaymentDetails.DisplayLayout.Override.FixedHeaderAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(221)))), ((int)(((byte)(223)))));
            appearance16.BorderColor = System.Drawing.Color.White;
            appearance16.ForeColor = System.Drawing.Color.Black;
            appearance16.TextHAlignAsString = "Center";
            appearance16.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdPaymentDetails.DisplayLayout.Override.HeaderAppearance = appearance16;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.RowAlternateAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance18.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.RowAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.RowPreviewAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.RowSelectorAppearance = appearance20;
            this.grdPaymentDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdPaymentDetails.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdPaymentDetails.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.grdPaymentDetails.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.RowSelectorsOnly;
            this.grdPaymentDetails.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance21.BackColor = System.Drawing.Color.Navy;
            appearance21.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdPaymentDetails.DisplayLayout.Override.SelectedCellAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.Yellow;
            appearance22.BackColorDisabled = System.Drawing.Color.Silver;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance22.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            appearance22.ForeColor = System.Drawing.Color.Black;
            this.grdPaymentDetails.DisplayLayout.Override.SelectedRowAppearance = appearance22;
            this.grdPaymentDetails.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPaymentDetails.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPaymentDetails.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdPaymentDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance23;
            this.grdPaymentDetails.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdPaymentDetails.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(191)))), ((int)(((byte)(193)))));
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance24.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance24;
            this.grdPaymentDetails.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdPaymentDetails.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.Vertical;
            this.grdPaymentDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPaymentDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdPaymentDetails.Location = new System.Drawing.Point(4, 47);
            this.grdPaymentDetails.Name = "grdPaymentDetails";
            this.grdPaymentDetails.Size = new System.Drawing.Size(526, 118);
            this.grdPaymentDetails.TabIndex = 0;
            this.grdPaymentDetails.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdPaymentDetails.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // frmSalesPaymentHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 211);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSalesPaymentHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sales Payment History";
            this.Load += new System.EventHandler(this.frmSalesPaymentHistory_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSalesPaymentHistory_KeyDown);
            this.tlpMain.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPaymentDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdPaymentDetails;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private Infragistics.Win.Misc.UltraButton btnClose;
    }
}