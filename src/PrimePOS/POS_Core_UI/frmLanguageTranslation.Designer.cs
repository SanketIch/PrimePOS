namespace POS_Core_UI
{
    partial class frmLanguageTranslation
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem15 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem16 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem17 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem18 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnTranslate = new Infragistics.Win.Misc.UltraButton();
            this.cmbToLanguage = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFromLanguage = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblItemDetail = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbToLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFromLanguage)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(823, 35);
            this.lblTransactionType.TabIndex = 0;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Language Translator";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnTranslate);
            this.groupBox4.Controls.Add(this.cmbToLanguage);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cmbFromLanguage);
            this.groupBox4.Controls.Add(this.lblItemDetail);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 41);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(799, 59);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Select Translation Language";
            // 
            // btnTranslate
            // 
            this.btnTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnTranslate.Appearance = appearance2;
            this.btnTranslate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnTranslate.Location = new System.Drawing.Point(692, 13);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(98, 34);
            this.btnTranslate.TabIndex = 2;
            this.btnTranslate.Text = "&Translate";
            this.btnTranslate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // cmbToLanguage
            // 
            this.cmbToLanguage.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.cmbToLanguage.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem3.DataValue = "1";
            valueListItem3.DisplayText = "1";
            valueListItem2.DataValue = valueListItem3;
            valueListItem2.DisplayText = "1";
            valueListItem1.DataValue = valueListItem2;
            valueListItem1.DisplayText = "1";
            valueListItem6.DataValue = "2";
            valueListItem6.DisplayText = "2";
            valueListItem5.DataValue = valueListItem6;
            valueListItem5.DisplayText = "2";
            valueListItem4.DataValue = valueListItem5;
            valueListItem4.DisplayText = "2";
            valueListItem9.DataValue = "3";
            valueListItem9.DisplayText = "3";
            valueListItem8.DataValue = valueListItem9;
            valueListItem8.DisplayText = "3";
            valueListItem7.DataValue = valueListItem8;
            valueListItem7.DisplayText = "3";
            this.cmbToLanguage.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem4,
            valueListItem7});
            this.cmbToLanguage.Location = new System.Drawing.Point(458, 19);
            this.cmbToLanguage.Name = "cmbToLanguage";
            this.cmbToLanguage.Size = new System.Drawing.Size(227, 24);
            this.cmbToLanguage.TabIndex = 1;
            this.cmbToLanguage.SelectionChangeCommitted += new System.EventHandler(this.cmbToLanguage_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(359, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 19);
            this.label1.TabIndex = 44;
            this.label1.Text = "Translate To";
            // 
            // cmbFromLanguage
            // 
            this.cmbFromLanguage.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.cmbFromLanguage.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem12.DataValue = "1";
            valueListItem12.DisplayText = "1";
            valueListItem11.DataValue = valueListItem12;
            valueListItem11.DisplayText = "1";
            valueListItem10.DataValue = valueListItem11;
            valueListItem10.DisplayText = "1";
            valueListItem15.DataValue = "2";
            valueListItem15.DisplayText = "2";
            valueListItem14.DataValue = valueListItem15;
            valueListItem14.DisplayText = "2";
            valueListItem13.DataValue = valueListItem14;
            valueListItem13.DisplayText = "2";
            valueListItem18.DataValue = "3";
            valueListItem18.DisplayText = "3";
            valueListItem17.DataValue = valueListItem18;
            valueListItem17.DisplayText = "3";
            valueListItem16.DataValue = valueListItem17;
            valueListItem16.DisplayText = "3";
            this.cmbFromLanguage.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem10,
            valueListItem13,
            valueListItem16});
            this.cmbFromLanguage.Location = new System.Drawing.Point(127, 19);
            this.cmbFromLanguage.Name = "cmbFromLanguage";
            this.cmbFromLanguage.Size = new System.Drawing.Size(227, 24);
            this.cmbFromLanguage.TabIndex = 0;
            // 
            // lblItemDetail
            // 
            this.lblItemDetail.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemDetail.ForeColor = System.Drawing.Color.Black;
            this.lblItemDetail.Location = new System.Drawing.Point(8, 21);
            this.lblItemDetail.Name = "lblItemDetail";
            this.lblItemDetail.Size = new System.Drawing.Size(120, 19);
            this.lblItemDetail.TabIndex = 6;
            this.lblItemDetail.Text = "Translate From";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnOK);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(13, 285);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(798, 54);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.btnOK.Appearance = appearance3;
            this.btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnOK.Location = new System.Drawing.Point(540, 19);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(155, 26);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&Use Translated Text";
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance4;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(702, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 26);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtTo
            // 
            this.txtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtTo.Location = new System.Drawing.Point(422, 106);
            this.txtTo.Multiline = true;
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(389, 178);
            this.txtTo.TabIndex = 2;
            // 
            // txtFrom
            // 
            this.txtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.txtFrom.Location = new System.Drawing.Point(12, 106);
            this.txtFrom.Multiline = true;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(389, 178);
            this.txtFrom.TabIndex = 1;
            // 
            // frmLanguageTranslation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(823, 345);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLanguageTranslation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Language Translation";
            this.Load += new System.EventHandler(this.frmLanguageTranslation_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbToLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFromLanguage)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblItemDetail;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbToLanguage;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbFromLanguage;
        private Infragistics.Win.Misc.UltraButton btnTranslate;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraButton btnOK;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtFrom;
    }
}