namespace POS_Core_UI
{
    partial class frmTaxBreakDown
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TaxID");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Amount");
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
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.grdTaxBreakDown = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTaxBreakDown)).BeginInit();
            this.tlpBottom.SuspendLayout();
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
            this.lblTransactionType.Size = new System.Drawing.Size(526, 29);
            this.lblTransactionType.TabIndex = 3;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Tax Breakdown";
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.grdTaxBreakDown, 0, 1);
            this.tlpMain.Controls.Add(this.tlpBottom, 0, 2);
            this.tlpMain.Controls.Add(this.lblTransactionType, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(5);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpMain.Size = new System.Drawing.Size(534, 221);
            this.tlpMain.TabIndex = 0;
            this.tlpMain.Tag = "";
            // 
            // grdTaxBreakDown
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            appearance2.BackColorDisabled = System.Drawing.Color.White;
            appearance2.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdTaxBreakDown.DisplayLayout.Appearance = appearance2;
            ultraGridColumn1.Header.VisiblePosition = 0;
            appearance3.BackColor = System.Drawing.Color.Gray;
            ultraGridColumn1.MergedCellAppearance = appearance3;
            ultraGridColumn1.Width = 82;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 302;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(102)))), ((int)(((byte)(127)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(102)))), ((int)(((byte)(127)))));
            ultraGridBand1.Header.Appearance = appearance4;
            ultraGridBand1.SummaryFooterCaption = "";
            this.grdTaxBreakDown.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdTaxBreakDown.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(102)))), ((int)(((byte)(127)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.SizeInPoints = 9F;
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Left";
            this.grdTaxBreakDown.DisplayLayout.CaptionAppearance = appearance5;
            this.grdTaxBreakDown.DisplayLayout.InterBandSpacing = 10;
            this.grdTaxBreakDown.DisplayLayout.MaxColScrollRegions = 1;
            this.grdTaxBreakDown.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            this.grdTaxBreakDown.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            appearance7.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.AddRowAppearance = appearance8;
            this.grdTaxBreakDown.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdTaxBreakDown.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdTaxBreakDown.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.grdTaxBreakDown.DisplayLayout.Override.CardAreaAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BackColorDisabled = System.Drawing.Color.White;
            appearance10.BackColorDisabled2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Black;
            appearance10.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdTaxBreakDown.DisplayLayout.Override.CellAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance11.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            appearance11.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance11.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance11.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdTaxBreakDown.DisplayLayout.Override.CellButtonAppearance = appearance11;
            this.grdTaxBreakDown.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdTaxBreakDown.DisplayLayout.Override.EditCellAppearance = appearance12;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.FilteredInRowAppearance = appearance13;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.FilteredOutRowAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackColorDisabled = System.Drawing.Color.White;
            appearance15.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdTaxBreakDown.DisplayLayout.Override.FixedCellAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance16.BackColor2 = System.Drawing.Color.Beige;
            this.grdTaxBreakDown.DisplayLayout.Override.FixedHeaderAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.FontData.BoldAsString = "True";
            appearance17.FontData.SizeInPoints = 9F;
            appearance17.ForeColor = System.Drawing.Color.Black;
            appearance17.TextHAlignAsString = "Left";
            appearance17.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdTaxBreakDown.DisplayLayout.Override.HeaderAppearance = appearance17;
            this.grdTaxBreakDown.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance18.BackColor = System.Drawing.Color.LightCyan;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.RowAlternateAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance19.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.RowAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.RowPreviewAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BackColor2 = System.Drawing.SystemColors.Control;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.RowSelectorAppearance = appearance21;
            this.grdTaxBreakDown.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdTaxBreakDown.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance22.BackColor = System.Drawing.Color.Navy;
            appearance22.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdTaxBreakDown.DisplayLayout.Override.SelectedCellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.Navy;
            appearance23.BackColorDisabled = System.Drawing.Color.Navy;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance23.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            appearance23.ForeColor = System.Drawing.Color.Black;
            this.grdTaxBreakDown.DisplayLayout.Override.SelectedRowAppearance = appearance23;
            this.grdTaxBreakDown.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdTaxBreakDown.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdTaxBreakDown.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdTaxBreakDown.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.grdTaxBreakDown.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdTaxBreakDown.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.SystemColors.Control;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.White;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance28;
            this.grdTaxBreakDown.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdTaxBreakDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTaxBreakDown.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdTaxBreakDown.Location = new System.Drawing.Point(4, 40);
            this.grdTaxBreakDown.Name = "grdTaxBreakDown";
            this.grdTaxBreakDown.Size = new System.Drawing.Size(526, 136);
            this.grdTaxBreakDown.TabIndex = 1;
            this.grdTaxBreakDown.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdTaxBreakDown.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdTaxBreakDown_InitializeRow);
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 3;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpBottom.Controls.Add(this.btnOk, 1, 0);
            this.tlpBottom.Controls.Add(this.btnClose, 2, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(4, 183);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(526, 34);
            this.tlpBottom.TabIndex = 0;
            // 
            // btnOk
            // 
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(168)))), ((int)(((byte)(90)))));
            appearance29.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance29;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnOk.HotTrackAppearance = appearance30;
            this.btnOk.Location = new System.Drawing.Point(288, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(115, 28);
            this.btnOk.TabIndex = 4;
            this.btnOk.Tag = "NOCOLOR";
            this.btnOk.Text = "Ok";
            this.btnOk.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance31.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance31;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance32.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance32;
            this.btnClose.Location = new System.Drawing.Point(409, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(114, 28);
            this.btnClose.TabIndex = 0;
            this.btnClose.Tag = "NOCOLOR";
            this.btnClose.Text = "Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmTaxBreakDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 221);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTaxBreakDown";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tax Breakdown";
            this.Load += new System.EventHandler(this.frmTaxBreakDown_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTaxBreakDown_KeyDown);
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTaxBreakDown)).EndInit();
            this.tlpBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private Infragistics.Win.Misc.UltraButton btnClose;
        internal Infragistics.Win.UltraWinGrid.UltraGrid grdTaxBreakDown;
        private Infragistics.Win.Misc.UltraButton btnOk;
    }
}