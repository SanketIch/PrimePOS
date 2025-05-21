namespace POS_Core_UI.Reports.ReportsUI
{
    partial class frmRptSalesbyVendor
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
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptSalesbyVendor));
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpSaleEndDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpSaleStartDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtVendorCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnView
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            this.btnView.Appearance = appearance14;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(209, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnPrint
            // 
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            this.btnPrint.Appearance = appearance15;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(117, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 385);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(441, 57);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance16.FontData.BoldAsString = "True";
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            this.btnClose.Appearance = appearance16;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(301, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTransactionType
            // 
            appearance17.ForeColor = System.Drawing.Color.White;
            appearance17.ForeColorDisabled = System.Drawing.Color.White;
            appearance17.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance17;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(18, 24);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(435, 30);
            this.lblTransactionType.TabIndex = 32;
            this.lblTransactionType.Text = "Sales Summary  By Vendor";
            // 
            // txtItemCode
            // 
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            appearance5.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance5.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance5.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance5;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtItemCode.ButtonsRight.Add(editorButton1);
            this.txtItemCode.Location = new System.Drawing.Point(267, 253);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(144, 23);
            this.txtItemCode.TabIndex = 6;
            this.txtItemCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItemCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItemCode_EditorButtonClick);
            // 
            // ultraLabel2
            // 
            appearance18.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Appearance = appearance18;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(9, 214);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(123, 18);
            this.ultraLabel2.TabIndex = 23;
            this.ultraLabel2.Text = "Transaction Type";
            // 
            // cboTransType
            // 
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboTransType.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(267, 212);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(144, 23);
            this.cboTransType.TabIndex = 5;
            this.cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboTransType_KeyDown);
            this.cboTransType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTransType_KeyPress);
            // 
            // ultraLabel20
            // 
            appearance19.ForeColor = System.Drawing.Color.White;
            this.ultraLabel20.Appearance = appearance19;
            this.ultraLabel20.Location = new System.Drawing.Point(9, 24);
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.Size = new System.Drawing.Size(116, 14);
            this.ultraLabel20.TabIndex = 20;
            this.ultraLabel20.Text = "Start Date";
            // 
            // ultraLabel19
            // 
            appearance20.ForeColor = System.Drawing.Color.White;
            this.ultraLabel19.Appearance = appearance20;
            this.ultraLabel19.Location = new System.Drawing.Point(9, 60);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(106, 14);
            this.ultraLabel19.TabIndex = 22;
            this.ultraLabel19.Text = "End Date";
            // 
            // dtpSaleEndDate
            // 
            this.dtpSaleEndDate.AllowNull = false;
            this.dtpSaleEndDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleEndDate.DateButtons.Add(dateButton3);
            this.dtpSaleEndDate.Location = new System.Drawing.Point(266, 56);
            this.dtpSaleEndDate.Name = "dtpSaleEndDate";
            this.dtpSaleEndDate.NonAutoSizeHeight = 10;
            this.dtpSaleEndDate.Size = new System.Drawing.Size(144, 22);
            this.dtpSaleEndDate.TabIndex = 1;
            this.dtpSaleEndDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleEndDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleEndDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // dtpSaleStartDate
            // 
            this.dtpSaleStartDate.AllowNull = false;
            this.dtpSaleStartDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpSaleStartDate.DateButtons.Add(dateButton4);
            this.dtpSaleStartDate.Location = new System.Drawing.Point(265, 20);
            this.dtpSaleStartDate.Name = "dtpSaleStartDate";
            this.dtpSaleStartDate.NonAutoSizeHeight = 10;
            this.dtpSaleStartDate.Size = new System.Drawing.Size(144, 22);
            this.dtpSaleStartDate.TabIndex = 0;
            this.dtpSaleStartDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpSaleStartDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtpSaleStartDate.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel1
            // 
            appearance21.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance21;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(9, 255);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(225, 18);
            this.ultraLabel1.TabIndex = 19;
            this.ultraLabel1.Text = "Item Code <Blank = All Items>";
            // 
            // txtStationID
            // 
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Location = new System.Drawing.Point(266, 171);
            this.txtStationID.MaxLength = 20;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(144, 23);
            this.txtStationID.TabIndex = 4;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel21
            // 
            appearance22.ForeColor = System.Drawing.Color.White;
            this.ultraLabel21.Appearance = appearance22;
            this.ultraLabel21.AutoSize = true;
            this.ultraLabel21.Location = new System.Drawing.Point(9, 173);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(228, 18);
            this.ultraLabel21.TabIndex = 17;
            this.ultraLabel21.Text = "Station # <Blank = All Station>";
            // 
            // ultraLabel12
            // 
            appearance23.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance23;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(9, 132);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(195, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "User ID <Blank = Any User";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(266, 130);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(144, 23);
            this.txtUserID.TabIndex = 3;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.txtVendorCode);
            this.groupBox1.Controls.Add(this.txtItemCode);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.cboTransType);
            this.groupBox1.Controls.Add(this.ultraLabel20);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.dtpSaleEndDate);
            this.groupBox1.Controls.Add(this.dtpSaleStartDate);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.txtStationID);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 316);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            // 
            // ultraLabel3
            // 
            appearance24.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Appearance = appearance24;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(9, 93);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(220, 18);
            this.ultraLabel3.TabIndex = 37;
            this.ultraLabel3.Text = "Vendor <Blank = Any Vendor>";
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            editorButton2.Appearance = appearance13;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton2.Text = "";
            this.txtVendorCode.ButtonsRight.Add(editorButton2);
            this.txtVendorCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorCode.Location = new System.Drawing.Point(265, 92);
            this.txtVendorCode.MaxLength = 20;
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.Size = new System.Drawing.Size(146, 20);
            this.txtVendorCode.TabIndex = 2;
            this.txtVendorCode.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendorCode_EditorButtonClick);
            // 
            // frmRptSalesbyVendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(542, 459);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRptSalesbyVendor";
            this.Text = "Sales Summary  by Vendor";
            this.Load += new System.EventHandler(this.frmRptSalesbyVendor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesbyVendor_KeyDown);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSaleStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel20;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleEndDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpSaleStartDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendorCode;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
    }
}