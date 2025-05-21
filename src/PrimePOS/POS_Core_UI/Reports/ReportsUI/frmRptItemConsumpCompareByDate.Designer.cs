namespace POS_Core_UI.Reports.ReportsUI
{
    partial class frmRptItemConsumptionCompare
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptItemConsumptionCompare));
            Infragistics.Win.UltraWinEditors.EditorButton editorButton2 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinEditors.EditorButton editorButton3 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpToDateFirst = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpFromDateFirst = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtItem = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtSubDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDepartment = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDateFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDateFirst)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Location = new System.Drawing.Point(0, 0);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(200, 100);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpToDateFirst);
            this.groupBox1.Controls.Add(this.ultraLabel13);
            this.groupBox1.Controls.Add(this.dtpFromDateFirst);
            this.groupBox1.Controls.Add(this.ultraLabel14);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 64);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "First Filter Criteria";
            // 
            // dtpToDateFirst
            // 
            this.dtpToDateFirst.AllowNull = false;
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.ItalicAsString = "False";
            appearance6.FontData.StrikeoutAsString = "False";
            appearance6.FontData.UnderlineAsString = "False";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDateFirst.Appearance = appearance6;
            this.dtpToDateFirst.BackColor = System.Drawing.SystemColors.Window;
            this.dtpToDateFirst.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpToDateFirst.DateButtons.Add(dateButton1);
            this.dtpToDateFirst.Location = new System.Drawing.Point(318, 24);
            this.dtpToDateFirst.Name = "dtpToDateFirst";
            this.dtpToDateFirst.NonAutoSizeHeight = 10;
            this.dtpToDateFirst.Size = new System.Drawing.Size(123, 21);
            this.dtpToDateFirst.TabIndex = 2;
            this.dtpToDateFirst.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDateFirst.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(244, 26);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(59, 15);
            this.ultraLabel13.TabIndex = 11;
            this.ultraLabel13.Text = "To Date";
            // 
            // dtpFromDateFirst
            // 
            this.dtpFromDateFirst.AllowNull = false;
            appearance7.FontData.BoldAsString = "False";
            appearance7.FontData.ItalicAsString = "False";
            appearance7.FontData.StrikeoutAsString = "False";
            appearance7.FontData.UnderlineAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDateFirst.Appearance = appearance7;
            this.dtpFromDateFirst.BackColor = System.Drawing.SystemColors.Window;
            this.dtpFromDateFirst.ButtonStyle = Infragistics.Win.UIElementButtonStyle.PopupSoft;
            this.dtpFromDateFirst.DateButtons.Add(dateButton2);
            this.dtpFromDateFirst.Location = new System.Drawing.Point(82, 24);
            this.dtpFromDateFirst.Name = "dtpFromDateFirst";
            this.dtpFromDateFirst.NonAutoSizeHeight = 10;
            this.dtpFromDateFirst.Size = new System.Drawing.Size(122, 21);
            this.dtpFromDateFirst.TabIndex = 1;
            this.dtpFromDateFirst.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDateFirst.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(10, 27);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(72, 15);
            this.ultraLabel14.TabIndex = 10;
            this.ultraLabel14.Text = "From Date";
            // 
            // lblTransactionType
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.ForeColorDisabled = System.Drawing.Color.White;
            appearance8.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance8;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(24, 11);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(438, 31);
            this.lblTransactionType.TabIndex = 34;
            this.lblTransactionType.Text = "Item Sale Historical Comparison";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtItem);
            this.groupBox3.Controls.Add(this.ultraLabel3);
            this.groupBox3.Controls.Add(this.txtSubDepartment);
            this.groupBox3.Controls.Add(this.txtDepartment);
            this.groupBox3.Controls.Add(this.ultraLabel5);
            this.groupBox3.Controls.Add(this.ultraLabel4);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(15, 130);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(468, 122);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            // 
            // txtItem
            // 
            this.txtItem.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            appearance12.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance12.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance12.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton1.Appearance = appearance12;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton1.Text = "";
            editorButton1.Width = 20;
            this.txtItem.ButtonsRight.Add(editorButton1);
            this.txtItem.Location = new System.Drawing.Point(298, 18);
            this.txtItem.Name = "txtItem";
            this.txtItem.ReadOnly = true;
            this.txtItem.Size = new System.Drawing.Size(152, 23);
            this.txtItem.TabIndex = 101;
            this.txtItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtItem.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtItem_EditorButtonClick);
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(15, 21);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(213, 18);
            this.ultraLabel3.TabIndex = 9;
            this.ultraLabel3.Text = "Item <Blank = ALL Items>";
            // 
            // txtSubDepartment
            // 
            this.txtSubDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.Image = ((object)(resources.GetObject("appearance13.Image")));
            appearance13.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance13.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton2.Appearance = appearance13;
            editorButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton2.Text = "";
            editorButton2.Width = 20;
            this.txtSubDepartment.ButtonsRight.Add(editorButton2);
            this.txtSubDepartment.Location = new System.Drawing.Point(298, 83);
            this.txtSubDepartment.Name = "txtSubDepartment";
            this.txtSubDepartment.ReadOnly = true;
            this.txtSubDepartment.Size = new System.Drawing.Size(152, 23);
            this.txtSubDepartment.TabIndex = 103;
            this.txtSubDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtSubDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtSubDepartment_EditorButtonClick);
            // 
            // txtDepartment
            // 
            this.txtDepartment.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            appearance14.ImageAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.ImageBackgroundAlpha = Infragistics.Win.Alpha.Opaque;
            appearance14.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            editorButton3.Appearance = appearance14;
            editorButton3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            editorButton3.Text = "";
            editorButton3.Width = 20;
            this.txtDepartment.ButtonsRight.Add(editorButton3);
            this.txtDepartment.Location = new System.Drawing.Point(298, 51);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(152, 23);
            this.txtDepartment.TabIndex = 102;
            this.txtDepartment.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtDepartment.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.txtDepartment_EditorButtonClick);
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(15, 87);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(269, 18);
            this.ultraLabel5.TabIndex = 1;
            this.ultraLabel5.Text = "Sub Department <Blank = Ignore Sub  Dept>";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(15, 57);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(213, 18);
            this.ultraLabel4.TabIndex = 0;
            this.ultraLabel4.Text = "Department <Blank = ALL Dept>";
            // 
            // btnView
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            this.btnView.Appearance = appearance9;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(282, 19);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnClose
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            this.btnClose.Appearance = appearance10;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(374, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnPrint.Appearance = appearance3;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(190, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 258);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(468, 57);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            // 
            // frmRptItemConsumptionCompare
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(499, 323);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.KeyPreview = true;
            this.Name = "frmRptItemConsumptionCompare";
            this.Text = "Item Sale Historical Comparison";
            this.Load += new System.EventHandler(this.frmRptSalesCompare_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmRptItemConsumptionCompare_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptSalesCompare_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDateFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDateFirst)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDepartment)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInventoryReceived;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDateFirst;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDateFirst;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSubDepartment;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDepartment;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItem;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}