namespace POS_Core_UI
{
    partial class frmRxMarkedForDelivery
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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.lblMSG = new Infragistics.Win.Misc.UltraLabel();
            this.lvItemList = new System.Windows.Forms.ListView();
            this.Rx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Refill = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnYes = new Infragistics.Win.Misc.UltraButton();
            this.btnNo = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // lblMSG
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.lblMSG.Appearance = appearance1;
            this.lblMSG.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblMSG.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMSG.Location = new System.Drawing.Point(5, 135);
            this.lblMSG.Name = "lblMSG";
            this.lblMSG.Size = new System.Drawing.Size(387, 68);
            this.lblMSG.TabIndex = 52;
            this.lblMSG.Text = "Above RX(s) are marked for delivery. Do you want to hold the transaction for deli" +
    "very?  ";
            this.lblMSG.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lvItemList
            // 
            this.lvItemList.BackColor = System.Drawing.SystemColors.Info;
            this.lvItemList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Rx,
            this.Refill});
            this.lvItemList.FullRowSelect = true;
            this.lvItemList.GridLines = true;
            this.lvItemList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvItemList.HideSelection = false;
            this.lvItemList.Location = new System.Drawing.Point(10, 10);
            this.lvItemList.MultiSelect = false;
            this.lvItemList.Name = "lvItemList";
            this.lvItemList.Size = new System.Drawing.Size(382, 118);
            this.lvItemList.TabIndex = 0;
            this.lvItemList.TabStop = false;
            this.lvItemList.UseCompatibleStateImageBehavior = false;
            this.lvItemList.View = System.Windows.Forms.View.Details;
            // 
            // Rx
            // 
            this.Rx.Text = "Rx#";
            this.Rx.Width = 149;
            // 
            // Refill
            // 
            this.Refill.Text = "Refill#";
            this.Refill.Width = 185;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.SystemColors.Control;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnYes.Appearance = appearance2;
            this.btnYes.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnYes.HotTrackAppearance = appearance3;
            this.btnYes.Location = new System.Drawing.Point(209, 208);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(85, 28);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "&Yes";
            this.btnYes.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnYes.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnYes.Click += new System.EventHandler(this.btnHoldForDelivery_Click);
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.btnNo.Appearance = appearance4;
            this.btnNo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnNo.HotTrackAppearance = appearance5;
            this.btnNo.Location = new System.Drawing.Point(309, 208);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(85, 28);
            this.btnNo.TabIndex = 53;
            this.btnNo.Text = "&No";
            this.btnNo.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // frmRxMarkedForDelivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(404, 241);
            this.ControlBox = false;
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblMSG);
            this.Controls.Add(this.lvItemList);
            this.KeyPreview = true;
            this.Name = "frmRxMarkedForDelivery";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Rx For Delivery";
            this.Load += new System.EventHandler(this.frmRxMarkedForDelivery_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRxMarkedForDelivery_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        public Infragistics.Win.Misc.UltraLabel lblMSG;
        public System.Windows.Forms.ListView lvItemList;
        private System.Windows.Forms.ColumnHeader Rx;
        private System.Windows.Forms.ColumnHeader Refill;
        private Infragistics.Win.Misc.UltraButton btnYes;
        private Infragistics.Win.Misc.UltraButton btnNo;
    }
}