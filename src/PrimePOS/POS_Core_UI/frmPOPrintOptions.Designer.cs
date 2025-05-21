namespace POS_Core_UI
{
    partial class frmPOPrintOptions
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
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOPrintOptions));
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            this.ultraBtnPrint = new Infragistics.Win.Misc.UltraButton();
            this.ultraBtnPrintPreview = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.optPrintBarcode = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optPrintBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraBtnPrint
            // 
            this.ultraBtnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance16.FontData.BoldAsString = "True";
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            this.ultraBtnPrint.Appearance = appearance16;
            this.ultraBtnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraBtnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold);
            this.ultraBtnPrint.Location = new System.Drawing.Point(164, 86);
            this.ultraBtnPrint.Name = "ultraBtnPrint";
            this.ultraBtnPrint.Size = new System.Drawing.Size(117, 26);
            this.ultraBtnPrint.TabIndex = 2;
            this.ultraBtnPrint.Text = "&Print";
            this.ultraBtnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraBtnPrint.Click += new System.EventHandler(this.ultraBtnPrint_Click);
            // 
            // ultraBtnPrintPreview
            // 
            this.ultraBtnPrintPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance20.FontData.BoldAsString = "True";
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.Image = ((object)(resources.GetObject("appearance20.Image")));
            this.ultraBtnPrintPreview.Appearance = appearance20;
            this.ultraBtnPrintPreview.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraBtnPrintPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold);
            this.ultraBtnPrintPreview.Location = new System.Drawing.Point(23, 86);
            this.ultraBtnPrintPreview.Name = "ultraBtnPrintPreview";
            this.ultraBtnPrintPreview.Size = new System.Drawing.Size(117, 26);
            this.ultraBtnPrintPreview.TabIndex = 1;
            this.ultraBtnPrintPreview.Text = "Print Pre&view";
            this.ultraBtnPrintPreview.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraBtnPrintPreview.Click += new System.EventHandler(this.ultraBtnPrintPreview_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(-1, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.groupBox1);
            this.ultraGroupBox2.Controls.Add(this.optPrintBarcode);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox2.Location = new System.Drawing.Point(20, 20);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(401, 50);
            this.ultraGroupBox2.TabIndex = 0;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "Print Barcode";
            // 
            // optPrintBarcode
            // 
            this.optPrintBarcode.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.optPrintBarcode.CheckedIndex = 0;
            appearance1.FontData.BoldAsString = "False";
            this.optPrintBarcode.ItemAppearance = appearance1;
            this.optPrintBarcode.ItemOrigin = new System.Drawing.Point(0, 2);
            valueListItem1.DataValue = 1;
            valueListItem1.DisplayText = "Vendor Item Code";
            valueListItem2.DataValue = 2;
            valueListItem2.DisplayText = "Item ID";
            valueListItem3.DataValue = 3;
            valueListItem3.DisplayText = "None";
            this.optPrintBarcode.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.optPrintBarcode.ItemSpacingHorizontal = 50;
            this.optPrintBarcode.Location = new System.Drawing.Point(27, 20);
            this.optPrintBarcode.Name = "optPrintBarcode";
            this.optPrintBarcode.Size = new System.Drawing.Size(354, 20);
            this.optPrintBarcode.TabIndex = 1;
            this.optPrintBarcode.Text = "Vendor Item Code";
            this.optPrintBarcode.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.optPrintBarcode.ValueChanged += new System.EventHandler(this.optPrintBarcode_ValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance29.FontData.BoldAsString = "True";
            appearance29.ForeColor = System.Drawing.Color.White;
            appearance29.Image = ((object)(resources.GetObject("appearance29.Image")));
            this.btnClose.Appearance = appearance29;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(303, 85);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 26);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmPOPrintOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 127);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ultraBtnPrint);
            this.Controls.Add(this.ultraBtnPrintPreview);
            this.Controls.Add(this.ultraGroupBox2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOPrintOptions";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Purchase Order";
            this.Load += new System.EventHandler(this.frmPOPrintOptions_Load);
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optPrintBarcode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraBtnPrint;
        private Infragistics.Win.Misc.UltraButton ultraBtnPrintPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox ultraGroupBox2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet optPrintBarcode;
        private Infragistics.Win.Misc.UltraButton btnClose;

    }
}