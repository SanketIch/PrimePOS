namespace POS_Core_UI.Reports.ReportsUI
{
    partial class frmSSalesByVendor
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSSalesByVendor));
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton3 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton4 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.txtVendor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2.SuspendLayout();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(3, 206);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnPrint.Appearance = appearance8;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(142, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 6;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnClose
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            this.btnClose.Appearance = appearance9;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(326, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnView
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            this.btnView.Appearance = appearance10;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(234, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.txtVendor);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(3, 56);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(424, 142);
            this.gbInventoryReceived.TabIndex = 33;
            this.gbInventoryReceived.TabStop = false;
            // 
            // txtVendor
            // 
            this.txtVendor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            editorButton1.Appearance = appearance4;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            editorButton1.Text = "";
            this.txtVendor.ButtonsRight.Add(editorButton1);
            this.txtVendor.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendor.Location = new System.Drawing.Point(270, 103);
            this.txtVendor.MaxLength = 20;
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.Size = new System.Drawing.Size(131, 20);
            this.txtVendor.TabIndex = 5;
            this.txtVendor.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtVendor_EditorButtonClick);
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance11.FontData.BoldAsString = "False";
            appearance11.FontData.ItalicAsString = "False";
            appearance11.FontData.StrikeoutAsString = "False";
            appearance11.FontData.UnderlineAsString = "False";
            appearance11.ForeColor = System.Drawing.Color.Black;
            appearance11.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance11;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton3);
            this.dtpToDate.Location = new System.Drawing.Point(270, 66);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(131, 21);
            this.dtpToDate.TabIndex = 4;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2011, 11, 9, 0, 0, 0, 0);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance12.FontData.BoldAsString = "False";
            appearance12.FontData.ItalicAsString = "False";
            appearance12.FontData.StrikeoutAsString = "False";
            appearance12.FontData.UnderlineAsString = "False";
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance12;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton4);
            this.dtpFromDate.Location = new System.Drawing.Point(270, 29);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(131, 21);
            this.dtpFromDate.TabIndex = 3;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2011, 11, 9, 0, 0, 0, 0);
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel12.Location = new System.Drawing.Point(21, 103);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(243, 25);
            this.ultraLabel12.TabIndex = 3;
            this.ultraLabel12.Text = "Vendor Code<Last Vendor in Item File>";
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(21, 64);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(52, 15);
            this.ultraLabel13.TabIndex = 2;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(21, 29);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(103, 15);
            this.ultraLabel14.TabIndex = 1;
            this.ultraLabel14.Text = "Order From Date";
            // 
            // lblTransactionType
            // 
            appearance13.ForeColor = System.Drawing.Color.White;
            appearance13.ForeColorDisabled = System.Drawing.Color.White;
            appearance13.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance13;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(3, 12);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(417, 30);
            this.lblTransactionType.TabIndex = 34;
            this.lblTransactionType.Text = "Summery Sales By Vendor";
            // 
            // frmSSalesByVendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 267);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.gbInventoryReceived);
            this.Controls.Add(this.groupBox2);
            this.Name = "frmSSalesByVendor";
            this.Text = "frmSSalesByVendor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSSalesByVendor_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnView;
        private System.Windows.Forms.GroupBox gbInventoryReceived;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendor;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
    }
}