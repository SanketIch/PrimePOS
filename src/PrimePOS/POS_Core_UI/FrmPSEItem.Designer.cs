namespace POS_Core_UI
{
    partial class FrmPSEItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPSEItem));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsActive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblPillCount = new Infragistics.Win.Misc.UltraLabel();
            this.numPillCount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblItemNDC = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemNDC = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblDescription = new Infragistics.Win.Misc.UltraLabel();
            this.lblItemGrams = new Infragistics.Win.Misc.UltraLabel();
            this.lblITemCode = new Infragistics.Win.Misc.UltraLabel();
            this.numGrams = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPillCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNDC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(504, 50);
            this.lblTransactionType.TabIndex = 0;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "PSE Item Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 280);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(485, 59);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(393, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnSave.Appearance = appearance3;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(301, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 26);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.lblPillCount);
            this.groupBox1.Controls.Add(this.numPillCount);
            this.groupBox1.Controls.Add(this.lblItemNDC);
            this.groupBox1.Controls.Add(this.txtItemNDC);
            this.groupBox1.Controls.Add(this.lblDescription);
            this.groupBox1.Controls.Add(this.lblItemGrams);
            this.groupBox1.Controls.Add(this.lblITemCode);
            this.groupBox1.Controls.Add(this.numGrams);
            this.groupBox1.Controls.Add(this.txtItemCode);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(485, 230);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // chkIsActive
            // 
            appearance4.FontData.BoldAsString = "False";
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.ForeColorDisabled = System.Drawing.Color.White;
            this.chkIsActive.Appearance = appearance4;
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Location = new System.Drawing.Point(10, 197);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(127, 20);
            this.chkIsActive.TabIndex = 10;
            this.chkIsActive.Text = "Is Active Item";
            this.chkIsActive.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsActive.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.chkIsActive.CheckedChanged += new System.EventHandler(this.chkIsActive_CheckedChanged);
            // 
            // lblPillCount
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.lblPillCount.Appearance = appearance5;
            this.lblPillCount.Location = new System.Drawing.Point(10, 162);
            this.lblPillCount.Name = "lblPillCount";
            this.lblPillCount.Size = new System.Drawing.Size(96, 20);
            this.lblPillCount.TabIndex = 8;
            this.lblPillCount.Text = "Pill Count";
            this.lblPillCount.WrapText = false;
            // 
            // numPillCount
            // 
            this.numPillCount.AutoSize = false;
            this.numPillCount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numPillCount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numPillCount.FormatString = "###";
            this.numPillCount.Location = new System.Drawing.Point(125, 160);
            this.numPillCount.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numPillCount.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numPillCount.MaskInput = "nnn";
            this.numPillCount.MaxValue = 100;
            this.numPillCount.MinValue = 0;
            this.numPillCount.Name = "numPillCount";
            this.numPillCount.NullText = "0";
            this.numPillCount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numPillCount.Size = new System.Drawing.Size(98, 25);
            this.numPillCount.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numPillCount.TabIndex = 9;
            this.numPillCount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numPillCount.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numPillCount.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // lblItemNDC
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.lblItemNDC.Appearance = appearance6;
            this.lblItemNDC.Location = new System.Drawing.Point(10, 92);
            this.lblItemNDC.Name = "lblItemNDC";
            this.lblItemNDC.Size = new System.Drawing.Size(96, 20);
            this.lblItemNDC.TabIndex = 4;
            this.lblItemNDC.Text = "Item NDC";
            // 
            // txtItemNDC
            // 
            this.txtItemNDC.AutoSize = false;
            this.txtItemNDC.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtItemNDC.Location = new System.Drawing.Point(125, 90);
            this.txtItemNDC.MaxLength = 20;
            this.txtItemNDC.Name = "txtItemNDC";
            this.txtItemNDC.Size = new System.Drawing.Size(194, 25);
            this.txtItemNDC.TabIndex = 5;
            this.txtItemNDC.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItemNDC.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // lblDescription
            // 
            appearance7.ForeColor = System.Drawing.Color.White;
            this.lblDescription.Appearance = appearance7;
            this.lblDescription.Location = new System.Drawing.Point(10, 57);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(96, 20);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description";
            // 
            // lblItemGrams
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            this.lblItemGrams.Appearance = appearance8;
            this.lblItemGrams.Location = new System.Drawing.Point(10, 127);
            this.lblItemGrams.Name = "lblItemGrams";
            this.lblItemGrams.Size = new System.Drawing.Size(96, 20);
            this.lblItemGrams.TabIndex = 6;
            this.lblItemGrams.Text = "Grams";
            this.lblItemGrams.WrapText = false;
            // 
            // lblITemCode
            // 
            appearance9.ForeColor = System.Drawing.Color.White;
            this.lblITemCode.Appearance = appearance9;
            this.lblITemCode.Location = new System.Drawing.Point(10, 22);
            this.lblITemCode.Name = "lblITemCode";
            this.lblITemCode.Size = new System.Drawing.Size(96, 20);
            this.lblITemCode.TabIndex = 0;
            this.lblITemCode.Text = "Item Code";
            // 
            // numGrams
            // 
            this.numGrams.AutoSize = false;
            this.numGrams.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numGrams.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.numGrams.FormatString = "###.##";
            this.numGrams.Location = new System.Drawing.Point(125, 125);
            this.numGrams.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numGrams.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.numGrams.MaskInput = "nnn.nn";
            this.numGrams.MaxValue = 100;
            this.numGrams.MinValue = 0;
            this.numGrams.Name = "numGrams";
            this.numGrams.NullText = "0";
            this.numGrams.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numGrams.Size = new System.Drawing.Size(98, 25);
            this.numGrams.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.numGrams.TabIndex = 7;
            this.numGrams.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.numGrams.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.numGrams.Validated += new System.EventHandler(this.txtNumericBoxs_Validate);
            // 
            // txtItemCode
            // 
            this.txtItemCode.AutoSize = false;
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtItemCode.Location = new System.Drawing.Point(125, 20);
            this.txtItemCode.MaxLength = 20;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(194, 25);
            this.txtItemCode.TabIndex = 1;
            this.txtItemCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItemCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtDescription
            // 
            this.txtDescription.AutoSize = false;
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDescription.Location = new System.Drawing.Point(125, 55);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(350, 25);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDescription.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // FrmPSEItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(504, 349);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmPSEItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PSE Item";
            this.Load += new System.EventHandler(this.FrmPSEItem_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPillCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemNDC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel lblDescription;
        private Infragistics.Win.Misc.UltraLabel lblItemGrams;
        private Infragistics.Win.Misc.UltraLabel lblITemCode;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numGrams;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.Misc.UltraLabel lblItemNDC;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemNDC;
        private Infragistics.Win.Misc.UltraLabel lblPillCount;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numPillCount;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsActive;
    }
}