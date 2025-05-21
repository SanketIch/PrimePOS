namespace POS_Core_UI
{
    partial class frmPOSCoupon
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSCoupon));
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDesc = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblDesc = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsDiscInPercent = new System.Windows.Forms.CheckBox();
            this.txtCouponCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel79 = new Infragistics.Win.Misc.UltraLabel();
            this.txtDiscountPerc = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAddNewCoupon = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.txtCouponID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCouponCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscountPerc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCouponID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(382, 29);
            this.lblTransactionType.TabIndex = 119;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Coupon";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDesc);
            this.groupBox2.Controls.Add(this.lblDesc);
            this.groupBox2.Controls.Add(this.chkIsDiscInPercent);
            this.groupBox2.Controls.Add(this.txtCouponCode);
            this.groupBox2.Controls.Add(this.ultraLabel79);
            this.groupBox2.Controls.Add(this.txtDiscountPerc);
            this.groupBox2.Controls.Add(this.ultraLabel1);
            this.groupBox2.Controls.Add(this.ultraLabel20);
            this.groupBox2.Controls.Add(this.ultraLabel19);
            this.groupBox2.Controls.Add(this.dtpSaleEndDate);
            this.groupBox2.Controls.Add(this.dtpSaleStartDate);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(13, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(357, 201);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coupon Details";
            // 
            // txtDesc
            // 
            this.txtDesc.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txtDesc.Location = new System.Drawing.Point(120, 62);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(224, 24);
            this.txtDesc.TabIndex = 1;
            // 
            // lblDesc
            // 
            this.lblDesc.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDesc.Location = new System.Drawing.Point(7, 63);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(100, 23);
            this.lblDesc.TabIndex = 140;
            this.lblDesc.Text = "Description";
            // 
            // chkIsDiscInPercent
            // 
            this.chkIsDiscInPercent.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsDiscInPercent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIsDiscInPercent.ForeColor = System.Drawing.Color.White;
            this.chkIsDiscInPercent.Location = new System.Drawing.Point(233, 87);
            this.chkIsDiscInPercent.Name = "chkIsDiscInPercent";
            this.chkIsDiscInPercent.Size = new System.Drawing.Size(107, 46);
            this.chkIsDiscInPercent.TabIndex = 3;
            this.chkIsDiscInPercent.Text = "Is Discount In %?";
            // 
            // txtCouponCode
            // 
            this.txtCouponCode.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txtCouponCode.Location = new System.Drawing.Point(120, 25);
            this.txtCouponCode.Name = "txtCouponCode";
            this.txtCouponCode.Size = new System.Drawing.Size(224, 24);
            this.txtCouponCode.TabIndex = 0;
            this.txtCouponCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCouponCode_KeyDown);
            // 
            // ultraLabel79
            // 
            this.ultraLabel79.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.ultraLabel79.Location = new System.Drawing.Point(7, 26);
            this.ultraLabel79.Name = "ultraLabel79";
            this.ultraLabel79.Size = new System.Drawing.Size(116, 23);
            this.ultraLabel79.TabIndex = 135;
            this.ultraLabel79.Text = "Coupon Code";
            // 
            // txtDiscountPerc
            // 
            this.txtDiscountPerc.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDiscountPerc.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.txtDiscountPerc.Location = new System.Drawing.Point(120, 99);
            this.txtDiscountPerc.MaskInput = "nn,nnn.nn";
            this.txtDiscountPerc.MaxValue = 100D;
            this.txtDiscountPerc.MinValue = 0;
            this.txtDiscountPerc.Name = "txtDiscountPerc";
            this.txtDiscountPerc.NullText = "0.00";
            this.txtDiscountPerc.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtDiscountPerc.Size = new System.Drawing.Size(107, 22);
            this.txtDiscountPerc.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtDiscountPerc.SpinWrap = true;
            this.txtDiscountPerc.TabIndex = 2;
            this.txtDiscountPerc.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDiscountPerc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDiscountPerc_KeyDown);
            // 
            // ultraLabel1
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.ultraLabel1.Location = new System.Drawing.Point(7, 103);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(91, 14);
            this.ultraLabel1.TabIndex = 138;
            this.ultraLabel1.Text = "Discount";
            // 
            // ultraLabel20
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance3;
            this.ultraLabel20.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.ultraLabel20.Location = new System.Drawing.Point(7, 137);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(91, 14);
            this.ultraLabel20.TabIndex = 136;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance4;
            this.ultraLabel19.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.ultraLabel19.Location = new System.Drawing.Point(7, 171);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(100, 14);
            this.ultraLabel19.TabIndex = 137;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton1);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(120, 168);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(224, 21);
            this.dtpSaleEndDate.TabIndex = 5;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton2);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(120, 134);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(224, 21);
            this.dtpSaleStartDate.TabIndex = 4;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAddNewCoupon);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Location = new System.Drawing.Point(13, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(357, 73);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // btnAddNewCoupon
            // 
            this.btnAddNewCoupon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.btnAddNewCoupon.Appearance = appearance5;
            this.btnAddNewCoupon.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnAddNewCoupon.HotTrackAppearance = appearance6;
            this.btnAddNewCoupon.Location = new System.Drawing.Point(34, 28);
            this.btnAddNewCoupon.Name = "btnAddNewCoupon";
            this.btnAddNewCoupon.Size = new System.Drawing.Size(119, 26);
            this.btnAddNewCoupon.TabIndex = 88;
            this.btnAddNewCoupon.Text = "&Add New Coupon";
            this.btnAddNewCoupon.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnAddNewCoupon.Visible = false;
            this.btnAddNewCoupon.Click += new System.EventHandler(this.btnAddNewCoupon_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            this.btnClose.Appearance = appearance7;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(254, 28);
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
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnSave.Appearance = appearance8;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(161, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 26);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCouponID
            // 
            this.txtCouponID.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.txtCouponID.Location = new System.Drawing.Point(304, 18);
            this.txtCouponID.Name = "txtCouponID";
            this.txtCouponID.Size = new System.Drawing.Size(66, 21);
            this.txtCouponID.TabIndex = 141;
            this.txtCouponID.Visible = false;
            // 
            // frmPOSCoupon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(382, 332);
            this.Controls.Add(this.txtCouponID);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.ForeColor = System.Drawing.Color.White;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSCoupon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Coupon";
            this.Load += new System.EventHandler(this.frmPOSCoupon_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOSCoupon_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCouponCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiscountPerc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCouponID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkIsDiscInPercent;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCouponCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel79;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor txtDiscountPerc;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        public Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        public Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private Infragistics.Win.Misc.UltraButton btnClose;
        public Infragistics.Win.Misc.UltraButton btnSave;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtDesc;
        private Infragistics.Win.Misc.UltraLabel lblDesc;
        public Infragistics.Win.Misc.UltraButton btnAddNewCoupon;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCouponID;
    }
}