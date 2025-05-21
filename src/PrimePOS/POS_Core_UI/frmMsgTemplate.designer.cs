namespace POS_Core_UI
{
    partial class frmMsgTemplate
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.txtMessage = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtMessageSubject = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtMessageTitle = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.opnMessageType = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.cboMessageCategory = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblMessageType = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessageCategory = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessageSubject = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessageTitle = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageSubject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opnMessageType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMessageCategory)).BeginInit();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.BackColor = System.Drawing.Color.DeepSkyBlue;
            appearance1.BackColor2 = System.Drawing.Color.Azure;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance1.ForeColor = System.Drawing.Color.Navy;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Margin = new System.Windows.Forms.Padding(4);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(839, 43);
            this.lblTransactionType.TabIndex = 1;
            this.lblTransactionType.Text = "Message Template";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(735, 10);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Tag = "";
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance3;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnOk.Location = new System.Drawing.Point(644, 10);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.txtMessage);
            this.ultraPanel1.ClientArea.Controls.Add(this.txtMessageSubject);
            this.ultraPanel1.ClientArea.Controls.Add(this.txtMessageTitle);
            this.ultraPanel1.ClientArea.Controls.Add(this.opnMessageType);
            this.ultraPanel1.ClientArea.Controls.Add(this.cboMessageCategory);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMessageType);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMessageCategory);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMessage);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMessageSubject);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblMessageTitle);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 43);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(839, 240);
            this.ultraPanel1.TabIndex = 4;
            // 
            // txtMessage
            // 
            this.txtMessage.AutoSize = false;
            this.txtMessage.Location = new System.Drawing.Point(147, 80);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(668, 150);
            this.txtMessage.TabIndex = 4;
            // 
            // txtMessageSubject
            // 
            this.txtMessageSubject.AutoSize = false;
            this.txtMessageSubject.Location = new System.Drawing.Point(147, 50);
            this.txtMessageSubject.MaxLength = 200;
            this.txtMessageSubject.Name = "txtMessageSubject";
            this.txtMessageSubject.Size = new System.Drawing.Size(260, 20);
            this.txtMessageSubject.TabIndex = 1;
            // 
            // txtMessageTitle
            // 
            this.txtMessageTitle.AutoSize = false;
            this.txtMessageTitle.Location = new System.Drawing.Point(147, 20);
            this.txtMessageTitle.MaxLength = 100;
            this.txtMessageTitle.Name = "txtMessageTitle";
            this.txtMessageTitle.Size = new System.Drawing.Size(260, 20);
            this.txtMessageTitle.TabIndex = 0;
            // 
            // opnMessageType
            // 
            this.opnMessageType.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance4.TextHAlignAsString = "Center";
            this.opnMessageType.ItemAppearance = appearance4;
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "Email";
            this.opnMessageType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1});
            this.opnMessageType.Location = new System.Drawing.Point(577, 50);
            this.opnMessageType.Name = "opnMessageType";
            this.opnMessageType.Size = new System.Drawing.Size(238, 20);
            this.opnMessageType.TabIndex = 3;
            // 
            // cboMessageCategory
            // 
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance5.BorderColor3DBase = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.cboMessageCategory.Appearance = appearance5;
            this.cboMessageCategory.AutoSize = false;
            this.cboMessageCategory.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cboMessageCategory.ButtonAppearance = appearance6;
            this.cboMessageCategory.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboMessageCategory.Location = new System.Drawing.Point(577, 20);
            this.cboMessageCategory.Name = "cboMessageCategory";
            this.cboMessageCategory.Size = new System.Drawing.Size(238, 20);
            this.cboMessageCategory.TabIndex = 2;
            // 
            // lblMessageType
            // 
            this.lblMessageType.Location = new System.Drawing.Point(431, 50);
            this.lblMessageType.Name = "lblMessageType";
            this.lblMessageType.Size = new System.Drawing.Size(132, 20);
            this.lblMessageType.TabIndex = 5;
            this.lblMessageType.Text = "Message Type";
            // 
            // lblMessageCategory
            // 
            this.lblMessageCategory.Location = new System.Drawing.Point(431, 20);
            this.lblMessageCategory.Name = "lblMessageCategory";
            this.lblMessageCategory.Size = new System.Drawing.Size(132, 20);
            this.lblMessageCategory.TabIndex = 4;
            this.lblMessageCategory.Text = "Message Category";
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 80);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(125, 20);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Message *";
            // 
            // lblMessageSubject
            // 
            this.lblMessageSubject.Location = new System.Drawing.Point(12, 50);
            this.lblMessageSubject.Name = "lblMessageSubject";
            this.lblMessageSubject.Size = new System.Drawing.Size(125, 20);
            this.lblMessageSubject.TabIndex = 1;
            this.lblMessageSubject.Text = "Message Subject";
            // 
            // lblMessageTitle
            // 
            this.lblMessageTitle.Location = new System.Drawing.Point(12, 20);
            this.lblMessageTitle.Name = "lblMessageTitle";
            this.lblMessageTitle.Size = new System.Drawing.Size(125, 20);
            this.lblMessageTitle.TabIndex = 0;
            this.lblMessageTitle.Text = "Message Title *";
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.btnClose);
            this.ultraPanel2.ClientArea.Controls.Add(this.btnOk);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 283);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(839, 40);
            this.ultraPanel2.TabIndex = 5;
            // 
            // frmMsgTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(839, 323);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 10F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmMsgTemplate";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Message Template";
            this.Load += new System.EventHandler(this.frmMsgTemplate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageSubject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opnMessageType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMessageCategory)).EndInit();
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraLabel lblMessageType;
        private Infragistics.Win.Misc.UltraLabel lblMessageCategory;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private Infragistics.Win.Misc.UltraLabel lblMessageSubject;
        private Infragistics.Win.Misc.UltraLabel lblMessageTitle;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboMessageCategory;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet opnMessageType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMessageTitle;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMessage;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMessageSubject;
    }
}