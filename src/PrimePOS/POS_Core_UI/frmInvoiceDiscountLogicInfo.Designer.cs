namespace POS_Core_UI
{
    partial class frmInvoiceDiscountLogicInfo
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
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.txtInvoiceDiscountLogic = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceDiscountLogic)).BeginInit();
            this.SuspendLayout();
            // 
            // txtInvoiceDiscountLogic
            // 
            this.txtInvoiceDiscountLogic.AlwaysInEditMode = true;
            this.txtInvoiceDiscountLogic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance50.FontData.BoldAsString = "False";
            appearance50.FontData.ItalicAsString = "False";
            appearance50.FontData.StrikeoutAsString = "False";
            appearance50.FontData.UnderlineAsString = "False";
            appearance50.ForeColor = System.Drawing.Color.Black;
            appearance50.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtInvoiceDiscountLogic.Appearance = appearance50;
            this.txtInvoiceDiscountLogic.AutoSize = false;
            this.txtInvoiceDiscountLogic.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtInvoiceDiscountLogic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoiceDiscountLogic.HideSelection = false;
            this.txtInvoiceDiscountLogic.Location = new System.Drawing.Point(31, 44);
            this.txtInvoiceDiscountLogic.MaxLength = 800;
            this.txtInvoiceDiscountLogic.Multiline = true;
            this.txtInvoiceDiscountLogic.Name = "txtInvoiceDiscountLogic";
            this.txtInvoiceDiscountLogic.ReadOnly = true;
            this.txtInvoiceDiscountLogic.Scrollbars = System.Windows.Forms.ScrollBars.Both;
            this.txtInvoiceDiscountLogic.Size = new System.Drawing.Size(594, 408);
            this.txtInvoiceDiscountLogic.TabIndex = 0;
            this.txtInvoiceDiscountLogic.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtInvoiceDiscountLogic.Enter += new System.EventHandler(this.txtInvoiceDiscountLogic_Enter);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(31, 12);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(594, 26);
            this.lblTransactionType.TabIndex = 27;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Invoice Discount Logic ";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnClose
            // 
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.White;
            this.btnClose.Appearance = appearance13;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.Location = new System.Drawing.Point(511, 458);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmInvoiceDiscountLogicInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(657, 489);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.txtInvoiceDiscountLogic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvoiceDiscountLogicInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmInvoiceDiscountLogicInfo";
            this.Load += new System.EventHandler(this.frmInvoiceDiscountLogicInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtInvoiceDiscountLogic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtInvoiceDiscountLogic;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
    }
}