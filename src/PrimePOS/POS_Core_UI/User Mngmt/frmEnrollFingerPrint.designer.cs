namespace POS_Core_UI.UserManagement
{
    partial class frmEnrollFingerPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEnrollFingerPrint));
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem15 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem16 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem21 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem22 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem23 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem24 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem25 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem26 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem27 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem28 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem29 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem30 = new Infragistics.Win.ValueListItem();
            this.picBoxFinger = new System.Windows.Forms.PictureBox();
            this.lblPlacefinger = new System.Windows.Forms.Label();
            this.combEdFingers1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.groupBoxUserInfo = new System.Windows.Forms.GroupBox();
            this.txtUserName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtUser = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEnroll2 = new System.Windows.Forms.Button();
            this.btnEnroll1 = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.splitContainerEnrollFP = new System.Windows.Forms.SplitContainer();
            this.lblFingerPic1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblFingerPic2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.combEdFingers2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxFinger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.combEdFingers1)).BeginInit();
            this.groupBoxUserInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerEnrollFP)).BeginInit();
            this.splitContainerEnrollFP.Panel1.SuspendLayout();
            this.splitContainerEnrollFP.Panel2.SuspendLayout();
            this.splitContainerEnrollFP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combEdFingers2)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoxFinger
            // 
            this.picBoxFinger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBoxFinger.Location = new System.Drawing.Point(0, 22);
            this.picBoxFinger.Name = "picBoxFinger";
            this.picBoxFinger.Size = new System.Drawing.Size(256, 360);
            this.picBoxFinger.TabIndex = 0;
            this.picBoxFinger.TabStop = false;
            // 
            // lblPlacefinger
            // 
            this.lblPlacefinger.BackColor = System.Drawing.Color.Khaki;
            this.lblPlacefinger.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlacefinger.Image = ((System.Drawing.Image)(resources.GetObject("lblPlacefinger.Image")));
            this.lblPlacefinger.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPlacefinger.Location = new System.Drawing.Point(0, 0);
            this.lblPlacefinger.Margin = new System.Windows.Forms.Padding(0);
            this.lblPlacefinger.Name = "lblPlacefinger";
            this.lblPlacefinger.Size = new System.Drawing.Size(258, 22);
            this.lblPlacefinger.TabIndex = 7;
            this.lblPlacefinger.Text = "Place a finger on the reader";
            // 
            // combEdFingers1
            // 
            valueListItem11.DataValue = "RI";
            valueListItem11.DisplayText = "Right Index Finger";
            valueListItem13.DataValue = "RT";
            valueListItem13.DisplayText = "Right Thumb";
            valueListItem14.DataValue = "RM";
            valueListItem14.DisplayText = "Right Middle Finger";
            valueListItem15.DataValue = "RR";
            valueListItem15.DisplayText = "Right Ring Finger";
            valueListItem16.DataValue = "RL";
            valueListItem16.DisplayText = "Right Little Finger";
            valueListItem21.DataValue = "LI";
            valueListItem21.DisplayText = "Left Index Finger";
            valueListItem22.DataValue = "LT";
            valueListItem22.DisplayText = "Left Thumb";
            valueListItem23.DataValue = "LM";
            valueListItem23.DisplayText = "Left Middle Finger";
            valueListItem24.DataValue = "LR";
            valueListItem24.DisplayText = "Left Ring Finger";
            valueListItem25.DataValue = "LL";
            valueListItem25.DisplayText = "Left Little Finger";
            this.combEdFingers1.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem11,
            valueListItem13,
            valueListItem14,
            valueListItem15,
            valueListItem16,
            valueListItem21,
            valueListItem22,
            valueListItem23,
            valueListItem24,
            valueListItem25});
            this.combEdFingers1.Location = new System.Drawing.Point(8, 51);
            this.combEdFingers1.Margin = new System.Windows.Forms.Padding(0);
            this.combEdFingers1.Name = "combEdFingers1";
            this.combEdFingers1.Size = new System.Drawing.Size(120, 21);
            this.combEdFingers1.TabIndex = 1;
            // 
            // groupBoxUserInfo
            // 
            this.groupBoxUserInfo.Controls.Add(this.txtUserName);
            this.groupBoxUserInfo.Controls.Add(this.txtUser);
            this.groupBoxUserInfo.Controls.Add(this.label4);
            this.groupBoxUserInfo.Controls.Add(this.label2);
            this.groupBoxUserInfo.Location = new System.Drawing.Point(270, 10);
            this.groupBoxUserInfo.Name = "groupBoxUserInfo";
            this.groupBoxUserInfo.Size = new System.Drawing.Size(260, 110);
            this.groupBoxUserInfo.TabIndex = 6;
            this.groupBoxUserInfo.TabStop = false;
            this.groupBoxUserInfo.Text = "User Info:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(80, 78);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(160, 21);
            this.txtUserName.TabIndex = 5;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(80, 20);
            this.txtUser.Name = "txtUser";
            this.txtUser.ReadOnly = true;
            this.txtUser.Size = new System.Drawing.Size(100, 21);
            this.txtUser.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(10, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Full Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Login:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 40);
            this.label1.TabIndex = 5;
            this.label1.Text = "Finger\r\n\r\nIndex:";
            // 
            // btnEnroll2
            // 
            this.btnEnroll2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnroll2.Image = ((System.Drawing.Image)(resources.GetObject("btnEnroll2.Image")));
            this.btnEnroll2.Location = new System.Drawing.Point(24, 173);
            this.btnEnroll2.Name = "btnEnroll2";
            this.btnEnroll2.Size = new System.Drawing.Size(92, 30);
            this.btnEnroll2.TabIndex = 3;
            this.btnEnroll2.Text = "Enroll";
            this.btnEnroll2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnroll2.UseVisualStyleBackColor = true;
            this.btnEnroll2.Click += new System.EventHandler(this.btnEnroll2_Click);
            // 
            // btnEnroll1
            // 
            this.btnEnroll1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnroll1.Image = ((System.Drawing.Image)(resources.GetObject("btnEnroll1.Image")));
            this.btnEnroll1.Location = new System.Drawing.Point(24, 173);
            this.btnEnroll1.Name = "btnEnroll1";
            this.btnEnroll1.Size = new System.Drawing.Size(92, 30);
            this.btnEnroll1.TabIndex = 0;
            this.btnEnroll1.Text = "Enroll";
            this.btnEnroll1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnroll1.UseVisualStyleBackColor = true;
            this.btnEnroll1.Click += new System.EventHandler(this.btnEnroll1_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Image = ((System.Drawing.Image)(resources.GetObject("btnReset.Image")));
            this.btnReset.Location = new System.Drawing.Point(285, 345);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(92, 30);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(426, 345);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(0, 382);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 30);
            this.panel1.TabIndex = 9;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.LightBlue;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(5, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(137, 17);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status - successfully";
            this.lblStatus.TextChanged += new System.EventHandler(this.lblStatus_TextChanged);
            // 
            // splitContainerEnrollFP
            // 
            this.splitContainerEnrollFP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerEnrollFP.IsSplitterFixed = true;
            this.splitContainerEnrollFP.Location = new System.Drawing.Point(260, 123);
            this.splitContainerEnrollFP.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainerEnrollFP.Name = "splitContainerEnrollFP";
            // 
            // splitContainerEnrollFP.Panel1
            // 
            this.splitContainerEnrollFP.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainerEnrollFP.Panel1.Controls.Add(this.lblFingerPic1);
            this.splitContainerEnrollFP.Panel1.Controls.Add(this.label5);
            this.splitContainerEnrollFP.Panel1.Controls.Add(this.label1);
            this.splitContainerEnrollFP.Panel1.Controls.Add(this.combEdFingers1);
            this.splitContainerEnrollFP.Panel1.Controls.Add(this.btnEnroll1);
            // 
            // splitContainerEnrollFP.Panel2
            // 
            this.splitContainerEnrollFP.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainerEnrollFP.Panel2.Controls.Add(this.lblFingerPic2);
            this.splitContainerEnrollFP.Panel2.Controls.Add(this.label7);
            this.splitContainerEnrollFP.Panel2.Controls.Add(this.label6);
            this.splitContainerEnrollFP.Panel2.Controls.Add(this.combEdFingers2);
            this.splitContainerEnrollFP.Panel2.Controls.Add(this.btnEnroll2);
            this.splitContainerEnrollFP.Size = new System.Drawing.Size(280, 210);
            this.splitContainerEnrollFP.SplitterDistance = 140;
            this.splitContainerEnrollFP.SplitterWidth = 1;
            this.splitContainerEnrollFP.TabIndex = 3;
            // 
            // lblFingerPic1
            // 
            this.lblFingerPic1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFingerPic1.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblFingerPic1.Image = ((System.Drawing.Image)(resources.GetObject("lblFingerPic1.Image")));
            this.lblFingerPic1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblFingerPic1.Location = new System.Drawing.Point(40, 82);
            this.lblFingerPic1.Name = "lblFingerPic1";
            this.lblFingerPic1.Size = new System.Drawing.Size(50, 82);
            this.lblFingerPic1.TabIndex = 7;
            this.lblFingerPic1.Text = "Saved";
            this.lblFingerPic1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 27F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Goldenrod;
            this.label5.Location = new System.Drawing.Point(60, 1);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 40);
            this.label5.TabIndex = 6;
            this.label5.Text = "1";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblFingerPic2
            // 
            this.lblFingerPic2.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblFingerPic2.Image = ((System.Drawing.Image)(resources.GetObject("lblFingerPic2.Image")));
            this.lblFingerPic2.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblFingerPic2.Location = new System.Drawing.Point(40, 82);
            this.lblFingerPic2.Name = "lblFingerPic2";
            this.lblFingerPic2.Size = new System.Drawing.Size(50, 82);
            this.lblFingerPic2.TabIndex = 10;
            this.lblFingerPic2.Text = "Saved";
            this.lblFingerPic2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 27F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Goldenrod;
            this.label7.Location = new System.Drawing.Point(60, 1);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 40);
            this.label7.TabIndex = 9;
            this.label7.Text = "2";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(5, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 40);
            this.label6.TabIndex = 8;
            this.label6.Text = "Finger\r\n\r\nIndex:";
            // 
            // combEdFingers2
            // 
            valueListItem3.DataValue = "LI";
            valueListItem3.DisplayText = "Left Index Finger";
            valueListItem7.DataValue = "LT";
            valueListItem7.DisplayText = "Left Thumb";
            valueListItem8.DataValue = "LM";
            valueListItem8.DisplayText = "Left Middle Finger";
            valueListItem9.DataValue = "LR";
            valueListItem9.DisplayText = "Left Ring Finger";
            valueListItem10.DataValue = "LL";
            valueListItem10.DisplayText = "Left Little Finger";
            valueListItem26.DataValue = "RI";
            valueListItem26.DisplayText = "Right Index Finger";
            valueListItem27.DataValue = "RT";
            valueListItem27.DisplayText = "Right Thumb";
            valueListItem28.DataValue = "RM";
            valueListItem28.DisplayText = "Right Middle Finger";
            valueListItem29.DataValue = "RR";
            valueListItem29.DisplayText = "Right Ring Finger";
            valueListItem30.DataValue = "RL";
            valueListItem30.DisplayText = "Right Little Finger";
            this.combEdFingers2.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem7,
            valueListItem8,
            valueListItem9,
            valueListItem10,
            valueListItem26,
            valueListItem27,
            valueListItem28,
            valueListItem29,
            valueListItem30});
            this.combEdFingers2.Location = new System.Drawing.Point(8, 51);
            this.combEdFingers2.Margin = new System.Windows.Forms.Padding(0);
            this.combEdFingers2.Name = "combEdFingers2";
            this.combEdFingers2.Size = new System.Drawing.Size(120, 21);
            this.combEdFingers2.TabIndex = 2;
            // 
            // frmEnrollFingerPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 414);
            this.Controls.Add(this.splitContainerEnrollFP);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.groupBoxUserInfo);
            this.Controls.Add(this.lblPlacefinger);
            this.Controls.Add(this.picBoxFinger);
            this.MaximizeBox = false;
            this.Name = "frmEnrollFingerPrint";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enroll Fingerprints";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEnrollFingerPrint_FormClosing);
            this.Load += new System.EventHandler(this.frmEnrollFingerPrint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxFinger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.combEdFingers1)).EndInit();
            this.groupBoxUserInfo.ResumeLayout(false);
            this.groupBoxUserInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainerEnrollFP.Panel1.ResumeLayout(false);
            this.splitContainerEnrollFP.Panel1.PerformLayout();
            this.splitContainerEnrollFP.Panel2.ResumeLayout(false);
            this.splitContainerEnrollFP.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerEnrollFP)).EndInit();
            this.splitContainerEnrollFP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.combEdFingers2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoxFinger;
        private System.Windows.Forms.Label lblPlacefinger;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor combEdFingers1;
        private System.Windows.Forms.GroupBox groupBoxUserInfo;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEnroll2;
        private System.Windows.Forms.Button btnEnroll1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.SplitContainer splitContainerEnrollFP;
        private System.Windows.Forms.Label label5;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor combEdFingers2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblFingerPic1;
        private System.Windows.Forms.Label lblFingerPic2;
    }
}