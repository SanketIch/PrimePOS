namespace POS_Core_UI
{
    partial class frmDiscountOptions
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
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.txtDiscount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.btnRevertSellingPrice = new Infragistics.Win.Misc.UltraButton();
            this.btnRemoveDiscount = new Infragistics.Win.Misc.UltraButton();
            this.btnUpdateDiscount = new Infragistics.Win.Misc.UltraButton();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDiscount
            // 
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.White;
            appearance25.BackColorDisabled = System.Drawing.Color.White;
            appearance25.BackColorDisabled2 = System.Drawing.Color.White;
            appearance25.FontData.BoldAsString = "True";
            appearance25.FontData.SizeInPoints = 10F;
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.txtDiscount.Appearance = appearance25;
            this.txtDiscount.BackColor = System.Drawing.Color.White;
            this.txtDiscount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDiscount.Location = new System.Drawing.Point(27, 52);
            this.txtDiscount.MaskInput = "-nnnnnnnn.nn";
            this.txtDiscount.MaxValue = 99999999.99;
            this.txtDiscount.MinValue = -99999999.99;
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtDiscount.Size = new System.Drawing.Size(115, 22);
            this.txtDiscount.TabIndex = 13;
            this.txtDiscount.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtDiscount.Visible = false;
            // 
            // btnRevertSellingPrice
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.btnRevertSellingPrice.Appearance = appearance3;
            this.btnRevertSellingPrice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnRevertSellingPrice.Location = new System.Drawing.Point(306, 85);
            this.btnRevertSellingPrice.Name = "btnRevertSellingPrice";
            this.btnRevertSellingPrice.Size = new System.Drawing.Size(125, 31);
            this.btnRevertSellingPrice.TabIndex = 16;
            this.btnRevertSellingPrice.Text = "Revert &Selling Price";
            this.btnRevertSellingPrice.Click += new System.EventHandler(this.btnRevertSellingPrice_Click);
            // 
            // btnRemoveDiscount
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnRemoveDiscount.Appearance = appearance2;
            this.btnRemoveDiscount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnRemoveDiscount.Location = new System.Drawing.Point(167, 85);
            this.btnRemoveDiscount.Name = "btnRemoveDiscount";
            this.btnRemoveDiscount.Size = new System.Drawing.Size(125, 31);
            this.btnRemoveDiscount.TabIndex = 15;
            this.btnRemoveDiscount.Text = "&Remove Discount";
            this.btnRemoveDiscount.Click += new System.EventHandler(this.btnRemoveDiscount_Click);
            // 
            // btnUpdateDiscount
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnUpdateDiscount.Appearance = appearance1;
            this.btnUpdateDiscount.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnUpdateDiscount.Location = new System.Drawing.Point(27, 85);
            this.btnUpdateDiscount.Name = "btnUpdateDiscount";
            this.btnUpdateDiscount.Size = new System.Drawing.Size(125, 31);
            this.btnUpdateDiscount.TabIndex = 14;
            this.btnUpdateDiscount.Text = "&Update Discount";
            this.btnUpdateDiscount.Click += new System.EventHandler(this.btnUpdateDiscount_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(27, 31);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(336, 23);
            this.lblMessage.TabIndex = 17;
            this.lblMessage.Text = "Current Discount is more than Selling Price.";
            // 
            // frmDiscountOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 146);
            this.ControlBox = false;
            this.Controls.Add(this.txtDiscount);
            this.Controls.Add(this.btnRevertSellingPrice);
            this.Controls.Add(this.btnRemoveDiscount);
            this.Controls.Add(this.btnUpdateDiscount);
            this.Controls.Add(this.lblMessage);
            this.Location = new System.Drawing.Point(230, 120);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDiscountOptions";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Apply Discount";
            this.Load += new System.EventHandler(this.frmDiscountOptions_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDiscountOptions_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtDiscount;
        private Infragistics.Win.Misc.UltraButton btnRevertSellingPrice;
        private Infragistics.Win.Misc.UltraButton btnRemoveDiscount;
        private Infragistics.Win.Misc.UltraButton btnUpdateDiscount;
        private Infragistics.Win.Misc.UltraLabel lblMessage;


    }
}