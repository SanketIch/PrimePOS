namespace POS_Core_UI
{
    partial class frmStationCloseCashDetail
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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStationCloseCashDetail));
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblTotalCashEnter = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalPayout = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalEBT = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalCoupons = new Infragistics.Win.Misc.UltraLabel();
            this.lbltotal = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalROA = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalCC = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalCheck = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblStationCloseDetail = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionFee = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransFee = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Bottom;
            appearance1.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(12, 12);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(482, 42);
            this.lblTransactionType.TabIndex = 42;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Close Station Cash Details";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance2.FontData.Name = "Arial";
            appearance2.ForeColor = System.Drawing.Color.Blue;
            appearance2.TextHAlignAsString = "Right";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(37, 23);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(192, 26);
            this.ultraLabel1.TabIndex = 5;
            this.ultraLabel1.Text = "Total Cash Enter:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTransactionFee);
            this.groupBox1.Controls.Add(this.lblTransFee);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.lblTotalCashEnter);
            this.groupBox1.Controls.Add(this.lblTotalPayout);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.Controls.Add(this.lblTotalEBT);
            this.groupBox1.Controls.Add(this.ultraLabel10);
            this.groupBox1.Controls.Add(this.lblTotalCoupons);
            this.groupBox1.Controls.Add(this.lbltotal);
            this.groupBox1.Controls.Add(this.lblTotalROA);
            this.groupBox1.Controls.Add(this.ultraLabel6);
            this.groupBox1.Controls.Add(this.lblTotalCC);
            this.groupBox1.Controls.Add(this.ultraLabel8);
            this.groupBox1.Controls.Add(this.lblTotalCheck);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Blue;
            this.groupBox1.Location = new System.Drawing.Point(11, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 353);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            // 
            // btnClose
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnClose.Appearance = appearance5;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(260, 306);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 32);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "&Continue";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTotalCashEnter
            // 
            appearance6.FontData.Name = "Arial";
            appearance6.FontData.SizeInPoints = 15F;
            appearance6.ForeColor = System.Drawing.Color.Maroon;
            appearance6.TextHAlignAsString = "Right";
            appearance6.TextVAlignAsString = "Middle";
            this.lblTotalCashEnter.Appearance = appearance6;
            this.lblTotalCashEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCashEnter.Location = new System.Drawing.Point(240, 22);
            this.lblTotalCashEnter.Name = "lblTotalCashEnter";
            this.lblTotalCashEnter.Size = new System.Drawing.Size(148, 28);
            this.lblTotalCashEnter.TabIndex = 22;
            this.lblTotalCashEnter.Text = "0.00";
            // 
            // lblTotalPayout
            // 
            appearance7.FontData.Name = "Arial";
            appearance7.FontData.SizeInPoints = 15F;
            appearance7.ForeColor = System.Drawing.Color.Maroon;
            appearance7.TextHAlignAsString = "Right";
            appearance7.TextVAlignAsString = "Middle";
            this.lblTotalPayout.Appearance = appearance7;
            this.lblTotalPayout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPayout.Location = new System.Drawing.Point(240, 232);
            this.lblTotalPayout.Name = "lblTotalPayout";
            this.lblTotalPayout.Size = new System.Drawing.Size(148, 28);
            this.lblTotalPayout.TabIndex = 21;
            this.lblTotalPayout.Text = "0.00";
            // 
            // ultraLabel14
            // 
            appearance8.FontData.Name = "Arial";
            appearance8.ForeColor = System.Drawing.Color.Blue;
            appearance8.TextHAlignAsString = "Right";
            appearance8.TextVAlignAsString = "Middle";
            this.ultraLabel14.Appearance = appearance8;
            this.ultraLabel14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(37, 233);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(192, 26);
            this.ultraLabel14.TabIndex = 20;
            this.ultraLabel14.Text = "Total Payout: ";
            // 
            // lblTotalEBT
            // 
            appearance9.FontData.Name = "Arial";
            appearance9.FontData.SizeInPoints = 15F;
            appearance9.ForeColor = System.Drawing.Color.Maroon;
            appearance9.TextHAlignAsString = "Right";
            appearance9.TextVAlignAsString = "Middle";
            this.lblTotalEBT.Appearance = appearance9;
            this.lblTotalEBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalEBT.Location = new System.Drawing.Point(240, 197);
            this.lblTotalEBT.Name = "lblTotalEBT";
            this.lblTotalEBT.Size = new System.Drawing.Size(148, 28);
            this.lblTotalEBT.TabIndex = 19;
            this.lblTotalEBT.Text = "0.00";
            // 
            // ultraLabel10
            // 
            appearance10.FontData.Name = "Arial";
            appearance10.ForeColor = System.Drawing.Color.Blue;
            appearance10.TextHAlignAsString = "Right";
            appearance10.TextVAlignAsString = "Middle";
            this.ultraLabel10.Appearance = appearance10;
            this.ultraLabel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel10.Location = new System.Drawing.Point(37, 198);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(192, 26);
            this.ultraLabel10.TabIndex = 18;
            this.ultraLabel10.Text = "Total EBT: ";
            // 
            // lblTotalCoupons
            // 
            appearance11.FontData.Name = "Arial";
            appearance11.FontData.SizeInPoints = 15F;
            appearance11.ForeColor = System.Drawing.Color.Maroon;
            appearance11.TextHAlignAsString = "Right";
            appearance11.TextVAlignAsString = "Middle";
            this.lblTotalCoupons.Appearance = appearance11;
            this.lblTotalCoupons.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCoupons.Location = new System.Drawing.Point(240, 162);
            this.lblTotalCoupons.Name = "lblTotalCoupons";
            this.lblTotalCoupons.Size = new System.Drawing.Size(148, 28);
            this.lblTotalCoupons.TabIndex = 17;
            this.lblTotalCoupons.Text = "0.00";
            // 
            // lbltotal
            // 
            appearance12.FontData.Name = "Arial";
            appearance12.ForeColor = System.Drawing.Color.Blue;
            appearance12.TextHAlignAsString = "Right";
            appearance12.TextVAlignAsString = "Middle";
            this.lbltotal.Appearance = appearance12;
            this.lbltotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltotal.Location = new System.Drawing.Point(37, 163);
            this.lbltotal.Name = "lbltotal";
            this.lbltotal.Size = new System.Drawing.Size(192, 26);
            this.lbltotal.TabIndex = 16;
            this.lbltotal.Text = "Total Coupons: ";
            // 
            // lblTotalROA
            // 
            appearance13.FontData.Name = "Arial";
            appearance13.FontData.SizeInPoints = 15F;
            appearance13.ForeColor = System.Drawing.Color.Maroon;
            appearance13.TextHAlignAsString = "Right";
            appearance13.TextVAlignAsString = "Middle";
            this.lblTotalROA.Appearance = appearance13;
            this.lblTotalROA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalROA.Location = new System.Drawing.Point(240, 127);
            this.lblTotalROA.Name = "lblTotalROA";
            this.lblTotalROA.Size = new System.Drawing.Size(148, 28);
            this.lblTotalROA.TabIndex = 15;
            this.lblTotalROA.Text = "0.00";
            // 
            // ultraLabel6
            // 
            appearance14.FontData.Name = "Arial";
            appearance14.ForeColor = System.Drawing.Color.Blue;
            appearance14.TextHAlignAsString = "Right";
            appearance14.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance14;
            this.ultraLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(37, 128);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(192, 26);
            this.ultraLabel6.TabIndex = 14;
            this.ultraLabel6.Text = "Total Receive on Account: ";
            // 
            // lblTotalCC
            // 
            appearance15.FontData.Name = "Arial";
            appearance15.FontData.SizeInPoints = 15F;
            appearance15.ForeColor = System.Drawing.Color.Maroon;
            appearance15.TextHAlignAsString = "Right";
            appearance15.TextVAlignAsString = "Middle";
            this.lblTotalCC.Appearance = appearance15;
            this.lblTotalCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCC.Location = new System.Drawing.Point(240, 92);
            this.lblTotalCC.Name = "lblTotalCC";
            this.lblTotalCC.Size = new System.Drawing.Size(148, 28);
            this.lblTotalCC.TabIndex = 13;
            this.lblTotalCC.Text = "0.00";
            // 
            // ultraLabel8
            // 
            appearance16.FontData.Name = "Arial";
            appearance16.ForeColor = System.Drawing.Color.Blue;
            appearance16.TextHAlignAsString = "Right";
            appearance16.TextVAlignAsString = "Middle";
            this.ultraLabel8.Appearance = appearance16;
            this.ultraLabel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel8.Location = new System.Drawing.Point(37, 93);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(192, 26);
            this.ultraLabel8.TabIndex = 12;
            this.ultraLabel8.Text = "Total CC: ";
            // 
            // lblTotalCheck
            // 
            appearance17.FontData.Name = "Arial";
            appearance17.FontData.SizeInPoints = 15F;
            appearance17.ForeColor = System.Drawing.Color.Maroon;
            appearance17.TextHAlignAsString = "Right";
            appearance17.TextVAlignAsString = "Middle";
            this.lblTotalCheck.Appearance = appearance17;
            this.lblTotalCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCheck.Location = new System.Drawing.Point(240, 57);
            this.lblTotalCheck.Name = "lblTotalCheck";
            this.lblTotalCheck.Size = new System.Drawing.Size(148, 28);
            this.lblTotalCheck.TabIndex = 11;
            this.lblTotalCheck.Text = "0.00";
            // 
            // ultraLabel4
            // 
            appearance18.FontData.Name = "Arial";
            appearance18.ForeColor = System.Drawing.Color.Blue;
            appearance18.TextHAlignAsString = "Right";
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance18;
            this.ultraLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(37, 58);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(192, 26);
            this.ultraLabel4.TabIndex = 10;
            this.ultraLabel4.Text = "Total Checks: ";
            // 
            // ultraLabel3
            // 
            appearance19.ForeColor = System.Drawing.Color.Blue;
            appearance19.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance19;
            this.ultraLabel3.Location = new System.Drawing.Point(50, 309);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(204, 26);
            this.ultraLabel3.TabIndex = 9;
            this.ultraLabel3.Text = "Press Enter to Contnue";
            // 
            // lblStationCloseDetail
            // 
            appearance20.FontData.Name = "Arial";
            appearance20.ForeColor = System.Drawing.Color.Blue;
            appearance20.TextVAlignAsString = "Middle";
            this.lblStationCloseDetail.Appearance = appearance20;
            this.lblStationCloseDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStationCloseDetail.Location = new System.Drawing.Point(12, 71);
            this.lblStationCloseDetail.Name = "lblStationCloseDetail";
            this.lblStationCloseDetail.Size = new System.Drawing.Size(482, 26);
            this.lblStationCloseDetail.TabIndex = 23;
            // 
            // lblTransactionFee
            // 
            appearance3.FontData.Name = "Arial";
            appearance3.FontData.SizeInPoints = 15F;
            appearance3.ForeColor = System.Drawing.Color.Maroon;
            appearance3.TextHAlignAsString = "Right";
            appearance3.TextVAlignAsString = "Middle";
            this.lblTransactionFee.Appearance = appearance3;
            this.lblTransactionFee.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionFee.Location = new System.Drawing.Point(240, 267);
            this.lblTransactionFee.Name = "lblTransactionFee";
            this.lblTransactionFee.Size = new System.Drawing.Size(148, 28);
            this.lblTransactionFee.TabIndex = 25;
            this.lblTransactionFee.Text = "0.00";
            // 
            // lblTransFee
            // 
            appearance4.FontData.Name = "Arial";
            appearance4.ForeColor = System.Drawing.Color.Blue;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.lblTransFee.Appearance = appearance4;
            this.lblTransFee.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransFee.Location = new System.Drawing.Point(37, 268);
            this.lblTransFee.Name = "lblTransFee";
            this.lblTransFee.Size = new System.Drawing.Size(192, 26);
            this.lblTransFee.TabIndex = 24;
            this.lblTransFee.Text = "Transaction Fee: ";
            // 
            // frmStationCloseCashDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(520, 464);
            this.Controls.Add(this.lblStationCloseDetail);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.Name = "frmStationCloseCashDetail";
            this.Text = "Close Station Cash Details";
            this.Load += new System.EventHandler(this.frmStationCloseCashDetail_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel lblTotalROA;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel lblTotalCC;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.Misc.UltraLabel lblTotalCheck;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel lblTotalEBT;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        private Infragistics.Win.Misc.UltraLabel lblTotalCoupons;
        private Infragistics.Win.Misc.UltraLabel lbltotal;
        private Infragistics.Win.Misc.UltraLabel lblTotalPayout;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel lblTotalCashEnter;
        private Infragistics.Win.Misc.UltraLabel lblStationCloseDetail;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblTransactionFee;
        private Infragistics.Win.Misc.UltraLabel lblTransFee;
    }
}