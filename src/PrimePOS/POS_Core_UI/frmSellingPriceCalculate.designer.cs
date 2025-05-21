namespace POS_Core_UI
{
    partial class frmSellingPriceCalculate
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSellingPriceCalculate));
            this.lblAmt = new Infragistics.Win.Misc.UltraLabel();
            this.numAmt = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.bttnClear = new Infragistics.Win.Misc.UltraButton();
            this.bttnOK = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.numAmt)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAmt
            // 
            appearance2.FontData.BoldAsString = "True";
            this.lblAmt.Appearance = appearance2;
            this.lblAmt.Location = new System.Drawing.Point(26, 38);
            this.lblAmt.Name = "lblAmt";
            this.lblAmt.Size = new System.Drawing.Size(230, 40);
            this.lblAmt.TabIndex = 27;
            // 
            // numAmt
            // 
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.SizeInPoints = 8F;
            this.numAmt.Appearance = appearance1;
            this.numAmt.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numAmt.Location = new System.Drawing.Point(288, 43);
            this.numAmt.MaxValue = 100;
            this.numAmt.MinValue = 0;
            this.numAmt.Name = "numAmt";
            this.numAmt.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numAmt.Size = new System.Drawing.Size(100, 19);
            this.numAmt.TabIndex = 24;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.bttnClear);
            this.groupBox4.Controls.Add(this.bttnOK);
            this.groupBox4.Controls.Add(this.btnCancel);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(58, 125);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(342, 50);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            // 
            // bttnClear
            // 
            this.bttnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance34.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance34.FontData.BoldAsString = "True";
            appearance34.ForeColor = System.Drawing.Color.White;
            this.bttnClear.Appearance = appearance34;
            this.bttnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.bttnClear.Location = new System.Drawing.Point(127, 14);
            this.bttnClear.Name = "bttnClear";
            this.bttnClear.Size = new System.Drawing.Size(70, 26);
            this.bttnClear.TabIndex = 9;
            this.bttnClear.Text = "&Clear";
            this.bttnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.bttnClear.Click += new System.EventHandler(this.bttnClear_Click);
            // 
            // bttnOK
            // 
            this.bttnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance31.FontData.BoldAsString = "True";
            appearance31.ForeColor = System.Drawing.Color.White;
            this.bttnOK.Appearance = appearance31;
            this.bttnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.bttnOK.Location = new System.Drawing.Point(20, 14);
            this.bttnOK.Name = "bttnOK";
            this.bttnOK.Size = new System.Drawing.Size(74, 26);
            this.bttnOK.TabIndex = 8;
            this.bttnOK.Text = "&OK";
            this.bttnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.bttnOK.Click += new System.EventHandler(this.bttnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance44.FontData.BoldAsString = "True";
            appearance44.ForeColor = System.Drawing.Color.White;
            appearance44.Image = ((object)(resources.GetObject("appearance44.Image")));
            this.btnCancel.Appearance = appearance44;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(230, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSellingPriceCalculate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 197);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.lblAmt);
            this.Controls.Add(this.numAmt);
            this.Name = "frmSellingPriceCalculate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSeelingPriceAmount";
            this.Load += new System.EventHandler(this.frmSellingPriceCalculate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numAmt)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblAmt;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor numAmt;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraButton bttnClear;
        private Infragistics.Win.Misc.UltraButton bttnOK;
        private Infragistics.Win.Misc.UltraButton btnCancel;
    }
}