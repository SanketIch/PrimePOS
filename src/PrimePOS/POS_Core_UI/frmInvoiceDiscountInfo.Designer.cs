namespace POS_Core_UI
{
    partial class frmInvoiceDiscountInfo
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
            this.lvItemList = new System.Windows.Forms.ListView();
            this.ItemID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ExtPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblMSG = new Infragistics.Win.Misc.UltraLabel();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // lvItemList
            // 
            this.lvItemList.BackColor = System.Drawing.SystemColors.Info;
            this.lvItemList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemID,
            this.Description,
            this.ExtPrice});
            this.lvItemList.FullRowSelect = true;
            this.lvItemList.GridLines = true;
            this.lvItemList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvItemList.HideSelection = false;
            this.lvItemList.Location = new System.Drawing.Point(12, 12);
            this.lvItemList.MultiSelect = false;
            this.lvItemList.Name = "lvItemList";
            this.lvItemList.Size = new System.Drawing.Size(430, 119);
            this.lvItemList.TabIndex = 48;
            this.lvItemList.TabStop = false;
            this.lvItemList.UseCompatibleStateImageBehavior = false;
            this.lvItemList.View = System.Windows.Forms.View.Details;
            // 
            // ItemID
            // 
            this.ItemID.Text = "Item Code";
            this.ItemID.Width = 80;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 239;
            // 
            // ExtPrice
            // 
            this.ExtPrice.Text = "Ext. Price";
            this.ExtPrice.Width = 111;
            // 
            // lblMSG
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.lblMSG.Appearance = appearance1;
            this.lblMSG.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMSG.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMSG.Location = new System.Drawing.Point(12, 138);
            this.lblMSG.Name = "lblMSG";
            this.lblMSG.Size = new System.Drawing.Size(429, 51);
            this.lblMSG.TabIndex = 49;
            this.lblMSG.Text = "Message";
            this.lblMSG.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.SystemColors.Control;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Appearance = appearance2;
            this.btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnOK.HotTrackAppearance = appearance3;
            this.btnOK.Location = new System.Drawing.Point(174, 198);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 26);
            this.btnOK.TabIndex = 50;
            this.btnOK.Text = "&OK";
            this.btnOK.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmInvoiceDiscountInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(456, 229);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblMSG);
            this.Controls.Add(this.lvItemList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvoiceDiscountInfo";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Invoice Discount ";
            this.Load += new System.EventHandler(this.frmInvoiceDiscountInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader ItemID;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ColumnHeader ExtPrice;
        public Infragistics.Win.Misc.UltraLabel lblMSG;
        private Infragistics.Win.Misc.UltraButton btnOK;
        public System.Windows.Forms.ListView lvItemList;

    }
}