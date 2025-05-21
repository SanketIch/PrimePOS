namespace POS_Core_UI.Reports.ReportsUI
{
    partial class frmRptTransactionTime
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
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton2 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRptTransactionTime));
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.gbInventoryReceived = new System.Windows.Forms.GroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cboStnId = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboUsers = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cboTransType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.dtpToDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.dtpFromDate = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.txtFromRange = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtToRange = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.gbInventoryReceived.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboStnId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFromRange)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtToRange)).BeginInit();
            this.SuspendLayout();
            // 
            // gbInventoryReceived
            // 
            this.gbInventoryReceived.Controls.Add(this.ultraLabel1);
            this.gbInventoryReceived.Controls.Add(this.cboStnId);
            this.gbInventoryReceived.Controls.Add(this.cboUsers);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel12);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel2);
            this.gbInventoryReceived.Controls.Add(this.cboTransType);
            this.gbInventoryReceived.Controls.Add(this.dtpToDate);
            this.gbInventoryReceived.Controls.Add(this.dtpFromDate);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel13);
            this.gbInventoryReceived.Controls.Add(this.ultraLabel14);
            this.gbInventoryReceived.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbInventoryReceived.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInventoryReceived.Location = new System.Drawing.Point(11, 70);
            this.gbInventoryReceived.Name = "gbInventoryReceived";
            this.gbInventoryReceived.Size = new System.Drawing.Size(306, 208);
            this.gbInventoryReceived.TabIndex = 0;
            this.gbInventoryReceived.TabStop = false;
            this.gbInventoryReceived.Text = "Report Criteria";
            // 
            // ultraLabel1
            // 
            appearance15.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance15;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(17, 171);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(62, 15);
            this.ultraLabel1.TabIndex = 8;
            this.ultraLabel1.Text = "Station ID";
            // 
            // cboStnId
            // 
            this.cboStnId.AlwaysInEditMode = true;
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.cboStnId.Appearance = appearance13;
            this.cboStnId.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance14.BackColor2 = System.Drawing.Color.Silver;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboStnId.ButtonAppearance = appearance14;
            this.cboStnId.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboStnId.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboStnId.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStnId.LimitToList = true;
            this.cboStnId.Location = new System.Drawing.Point(147, 166);
            this.cboStnId.Name = "cboStnId";
            this.cboStnId.NullText = "Select";
            this.cboStnId.Size = new System.Drawing.Size(123, 23);
            this.cboStnId.TabIndex = 4;
            this.cboStnId.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // cboUsers
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.cboUsers.Appearance = appearance6;
            this.cboUsers.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance7.BackColor2 = System.Drawing.Color.Silver;
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboUsers.ButtonAppearance = appearance7;
            this.cboUsers.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboUsers.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboUsers.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboUsers.LimitToList = true;
            this.cboUsers.Location = new System.Drawing.Point(147, 128);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.NullText = "Select";
            this.cboUsers.Size = new System.Drawing.Size(123, 23);
            this.cboUsers.TabIndex = 3;
            this.cboUsers.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboUsers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            // 
            // ultraLabel12
            // 
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ultraLabel12.Location = new System.Drawing.Point(17, 133);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(47, 15);
            this.ultraLabel12.TabIndex = 11;
            this.ultraLabel12.Text = "User ID";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance1;
            this.ultraLabel2.Location = new System.Drawing.Point(17, 97);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(71, 15);
            this.ultraLabel2.TabIndex = 12;
            this.ultraLabel2.Text = "Trans Type";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboTransType
            // 
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.cboTransType.Appearance = appearance2;
            this.cboTransType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance11.BackColor = System.Drawing.Color.WhiteSmoke;
            appearance11.BackColor2 = System.Drawing.Color.Silver;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.cboTransType.ButtonAppearance = appearance11;
            this.cboTransType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.cboTransType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboTransType.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "ALL";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "Sales";
            valueListItem3.DataValue = "ValueListItem2";
            valueListItem3.DisplayText = "Return";
            this.cboTransType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.cboTransType.LimitToList = true;
            this.cboTransType.Location = new System.Drawing.Point(147, 92);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.NullText = "Select";
            this.cboTransType.Size = new System.Drawing.Size(124, 23);
            this.cboTransType.TabIndex = 2;
            this.cboTransType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            // 
            // dtpToDate
            // 
            this.dtpToDate.AllowNull = false;
            appearance16.FontData.BoldAsString = "False";
            appearance16.FontData.ItalicAsString = "False";
            appearance16.FontData.StrikeoutAsString = "False";
            appearance16.FontData.UnderlineAsString = "False";
            appearance16.ForeColor = System.Drawing.Color.Black;
            appearance16.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpToDate.Appearance = appearance16;
            this.dtpToDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpToDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpToDate.DateButtons.Add(dateButton1);
            this.dtpToDate.Location = new System.Drawing.Point(147, 58);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.NonAutoSizeHeight = 10;
            this.dtpToDate.Size = new System.Drawing.Size(125, 21);
            this.dtpToDate.TabIndex = 1;
            this.dtpToDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpToDate.Value = new System.DateTime(2011, 6, 2, 0, 0, 0, 0);
            this.dtpToDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.AllowNull = false;
            appearance5.FontData.BoldAsString = "False";
            appearance5.FontData.ItalicAsString = "False";
            appearance5.FontData.StrikeoutAsString = "False";
            appearance5.FontData.UnderlineAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpFromDate.Appearance = appearance5;
            this.dtpFromDate.BackColor = System.Drawing.SystemColors.Window;
            this.dtpFromDate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpFromDate.DateButtons.Add(dateButton2);
            this.dtpFromDate.Location = new System.Drawing.Point(147, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.NonAutoSizeHeight = 10;
            this.dtpFromDate.Size = new System.Drawing.Size(125, 21);
            this.dtpFromDate.TabIndex = 0;
            this.dtpFromDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpFromDate.Value = new System.DateTime(2011, 6, 2, 0, 0, 0, 0);
            this.dtpFromDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            // 
            // ultraLabel13
            // 
            this.ultraLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel13.Location = new System.Drawing.Point(17, 61);
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel13.TabIndex = 11;
            this.ultraLabel13.Text = "To Date";
            // 
            // ultraLabel14
            // 
            this.ultraLabel14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel14.Location = new System.Drawing.Point(17, 27);
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.Size = new System.Drawing.Size(91, 15);
            this.ultraLabel14.TabIndex = 10;
            this.ultraLabel14.Text = "From Date";
            // 
            // btnClose
            // 
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance17.FontData.BoldAsString = "True";
            appearance17.ForeColor = System.Drawing.Color.White;
            appearance17.Image = ((object)(resources.GetObject("appearance17.Image")));
            this.btnClose.Appearance = appearance17;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(213, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            // 
            // btnView
            // 
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance18.FontData.BoldAsString = "True";
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.Image = ((object)(resources.GetObject("appearance18.Image")));
            this.btnView.Appearance = appearance18;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(121, 18);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            this.btnView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrint);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(13, 345);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 55);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnPrint
            // 
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance19.FontData.BoldAsString = "True";
            appearance19.ForeColor = System.Drawing.Color.White;
            appearance19.Image = ((object)(resources.GetObject("appearance19.Image")));
            this.btnPrint.Appearance = appearance19;
            this.btnPrint.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPrint.Location = new System.Drawing.Point(29, 17);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(85, 26);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnPrint.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            // 
            // lblTransactionType
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.ForeColorDisabled = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance4;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(13, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(289, 30);
            this.lblTransactionType.TabIndex = 2;
            this.lblTransactionType.Text = "Transaction Time Report";
            // 
            // txtFromRange
            // 
            this.txtFromRange.AutoSize = false;
            this.txtFromRange.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtFromRange.Location = new System.Drawing.Point(64, 26);
            this.txtFromRange.MaxLength = 20;
            this.txtFromRange.Name = "txtFromRange";
            this.txtFromRange.Size = new System.Drawing.Size(73, 19);
            this.txtFromRange.TabIndex = 5;
            this.txtFromRange.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.txtToRange);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.txtFromRange);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 290);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 55);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time Range(in sec)";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ultraLabel3.Location = new System.Drawing.Point(15, 28);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(33, 15);
            this.ultraLabel3.TabIndex = 13;
            this.ultraLabel3.Text = "From";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ultraLabel4.Location = new System.Drawing.Point(157, 28);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(18, 15);
            this.ultraLabel4.TabIndex = 15;
            this.ultraLabel4.Text = "To";
            // 
            // txtToRange
            // 
            this.txtToRange.AutoSize = false;
            this.txtToRange.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtToRange.Location = new System.Drawing.Point(183, 26);
            this.txtToRange.MaxLength = 20;
            this.txtToRange.Name = "txtToRange";
            this.txtToRange.Size = new System.Drawing.Size(73, 19);
            this.txtToRange.TabIndex = 14;
            this.txtToRange.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // frmRptTransactionTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 413);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.gbInventoryReceived);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.Name = "frmRptTransactionTime";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transaction Time Report";
            this.Load += new System.EventHandler(this.frmRptTransactionTime_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRptTransactionTime_KeyDown);
            this.gbInventoryReceived.ResumeLayout(false);
            this.gbInventoryReceived.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboStnId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTransType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFromDate)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFromRange)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtToRange)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInventoryReceived;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboTransType;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpToDate;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpFromDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel13;
        private Infragistics.Win.Misc.UltraLabel ultraLabel14;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnView;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton btnPrint;
        private Infragistics.Win.Misc.UltraLabel ultraLabel12;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboUsers;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboStnId;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFromRange;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtToRange;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
    }
}