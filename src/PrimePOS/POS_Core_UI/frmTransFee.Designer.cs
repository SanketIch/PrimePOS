
namespace POS_Core_UI
{
    partial class frmTransFee
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.tlpDetail = new System.Windows.Forms.TableLayoutPanel();
            this.lblChargeTransFeeFor = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransFeeDesc = new Infragistics.Win.Misc.UltraLabel();
            this.txtTransFeeDesc = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblPaymentType = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsActive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.numTransFeeValue = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.cboPaymentType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.osTransFeeMode = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.osChargeTransFeeFor = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.tlpMain.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.tlpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransFeeDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransFeeValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPaymentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osTransFeeMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osChargeTransFeeFor)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.lblTransactionType, 0, 0);
            this.tlpMain.Controls.Add(this.tlpBottom, 0, 2);
            this.tlpMain.Controls.Add(this.tlpDetail, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpMain.Size = new System.Drawing.Size(428, 254);
            this.tlpMain.TabIndex = 0;
            // 
            // lblTransactionType
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(3, 3);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(422, 40);
            this.lblTransactionType.TabIndex = 4;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Transaction Fee";
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 4;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tlpBottom.Controls.Add(this.btnOk, 1, 0);
            this.tlpBottom.Controls.Add(this.btnClose, 3, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(3, 211);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(422, 40);
            this.tlpBottom.TabIndex = 1;
            // 
            // btnOk
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(168)))), ((int)(((byte)(90)))));
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance2;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnOk.HotTrackAppearance = appearance3;
            this.btnOk.Location = new System.Drawing.Point(179, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(111, 34);
            this.btnOk.TabIndex = 0;
            this.btnOk.Tag = "NOCOLOR";
            this.btnOk.Text = "Ok";
            this.btnOk.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance4;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance5;
            this.btnClose.Location = new System.Drawing.Point(308, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(111, 34);
            this.btnClose.TabIndex = 1;
            this.btnClose.Tag = "NOCOLOR";
            this.btnClose.Text = "Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tlpDetail
            // 
            this.tlpDetail.ColumnCount = 2;
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.16981F));
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.83019F));
            this.tlpDetail.Controls.Add(this.lblChargeTransFeeFor, 0, 1);
            this.tlpDetail.Controls.Add(this.lblTransFeeDesc, 0, 0);
            this.tlpDetail.Controls.Add(this.txtTransFeeDesc, 1, 0);
            this.tlpDetail.Controls.Add(this.lblPaymentType, 0, 3);
            this.tlpDetail.Controls.Add(this.chkIsActive, 0, 4);
            this.tlpDetail.Controls.Add(this.numTransFeeValue, 1, 2);
            this.tlpDetail.Controls.Add(this.cboPaymentType, 1, 3);
            this.tlpDetail.Controls.Add(this.osTransFeeMode, 0, 2);
            this.tlpDetail.Controls.Add(this.osChargeTransFeeFor, 1, 1);
            this.tlpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetail.Location = new System.Drawing.Point(3, 49);
            this.tlpDetail.Name = "tlpDetail";
            this.tlpDetail.RowCount = 5;
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDetail.Size = new System.Drawing.Size(422, 156);
            this.tlpDetail.TabIndex = 0;
            // 
            // lblChargeTransFeeFor
            // 
            appearance6.TextHAlignAsString = "Right";
            this.lblChargeTransFeeFor.Appearance = appearance6;
            this.lblChargeTransFeeFor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChargeTransFeeFor.Location = new System.Drawing.Point(3, 33);
            this.lblChargeTransFeeFor.Name = "lblChargeTransFeeFor";
            this.lblChargeTransFeeFor.Size = new System.Drawing.Size(193, 24);
            this.lblChargeTransFeeFor.TabIndex = 12;
            this.lblChargeTransFeeFor.Text = "Charge Trans Fee For";
            // 
            // lblTransFeeDesc
            // 
            appearance7.TextHAlignAsString = "Right";
            this.lblTransFeeDesc.Appearance = appearance7;
            this.lblTransFeeDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransFeeDesc.Location = new System.Drawing.Point(3, 3);
            this.lblTransFeeDesc.Name = "lblTransFeeDesc";
            this.lblTransFeeDesc.Size = new System.Drawing.Size(193, 24);
            this.lblTransFeeDesc.TabIndex = 0;
            this.lblTransFeeDesc.Text = "Transaction Fee Description";
            // 
            // txtTransFeeDesc
            // 
            this.txtTransFeeDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTransFeeDesc.Location = new System.Drawing.Point(202, 3);
            this.txtTransFeeDesc.MaxLength = 256;
            this.txtTransFeeDesc.Name = "txtTransFeeDesc";
            this.txtTransFeeDesc.Size = new System.Drawing.Size(217, 23);
            this.txtTransFeeDesc.TabIndex = 0;
            // 
            // lblPaymentType
            // 
            appearance8.TextHAlignAsString = "Right";
            this.lblPaymentType.Appearance = appearance8;
            this.lblPaymentType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaymentType.Location = new System.Drawing.Point(3, 93);
            this.lblPaymentType.Name = "lblPaymentType";
            this.lblPaymentType.Size = new System.Drawing.Size(193, 24);
            this.lblPaymentType.TabIndex = 10;
            this.lblPaymentType.Text = "Payment Type";
            // 
            // chkIsActive
            // 
            appearance9.TextHAlignAsString = "Right";
            this.chkIsActive.Appearance = appearance9;
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkIsActive.Location = new System.Drawing.Point(3, 123);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(193, 30);
            this.chkIsActive.TabIndex = 5;
            this.chkIsActive.Text = "Is Active";
            // 
            // numTransFeeValue
            // 
            appearance10.FontData.BoldAsString = "False";
            appearance10.FontData.ItalicAsString = "False";
            appearance10.FontData.StrikeoutAsString = "False";
            appearance10.FontData.UnderlineAsString = "False";
            appearance10.ForeColor = System.Drawing.Color.Black;
            appearance10.ForeColorDisabled = System.Drawing.Color.Black;
            this.numTransFeeValue.Appearance = appearance10;
            this.numTransFeeValue.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numTransFeeValue.Location = new System.Drawing.Point(202, 63);
            this.numTransFeeValue.MaskInput = "{LOC}-nn,nnn.nn";
            this.numTransFeeValue.MaxValue = 99.99D;
            this.numTransFeeValue.MinValue = 0D;
            this.numTransFeeValue.Name = "numTransFeeValue";
            this.numTransFeeValue.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numTransFeeValue.Size = new System.Drawing.Size(111, 21);
            this.numTransFeeValue.TabIndex = 3;
            // 
            // cboPaymentType
            // 
            this.cboPaymentType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboPaymentType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboPaymentType.Location = new System.Drawing.Point(202, 93);
            this.cboPaymentType.Name = "cboPaymentType";
            this.cboPaymentType.Size = new System.Drawing.Size(217, 23);
            this.cboPaymentType.TabIndex = 4;
            // 
            // osTransFeeMode
            // 
            appearance11.TextHAlignAsString = "Right";
            this.osTransFeeMode.Appearance = appearance11;
            this.osTransFeeMode.Dock = System.Windows.Forms.DockStyle.Fill;
            valueListItem4.DataValue = "0";
            valueListItem4.DisplayText = "Percentage";
            valueListItem5.DataValue = "1";
            valueListItem5.DisplayText = "Fixed Amount";
            this.osTransFeeMode.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,
            valueListItem5});
            this.osTransFeeMode.Location = new System.Drawing.Point(3, 63);
            this.osTransFeeMode.Name = "osTransFeeMode";
            this.osTransFeeMode.Size = new System.Drawing.Size(193, 24);
            this.osTransFeeMode.TabIndex = 2;
            // 
            // osChargeTransFeeFor
            // 
            appearance12.TextHAlignAsString = "Right";
            this.osChargeTransFeeFor.Appearance = appearance12;
            this.osChargeTransFeeFor.Dock = System.Windows.Forms.DockStyle.Fill;
            valueListItem1.DataValue = "0";
            valueListItem1.DisplayText = "Both";
            valueListItem2.DataValue = "1";
            valueListItem2.DisplayText = "Sale Trans";
            valueListItem3.DataValue = "2";
            valueListItem3.DisplayText = "Return Trans";
            this.osChargeTransFeeFor.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.osChargeTransFeeFor.Location = new System.Drawing.Point(202, 33);
            this.osChargeTransFeeFor.Name = "osChargeTransFeeFor";
            this.osChargeTransFeeFor.Size = new System.Drawing.Size(217, 24);
            this.osChargeTransFeeFor.TabIndex = 1;
            // 
            // frmTransFee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 254);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransFee";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Fee";
            this.Load += new System.EventHandler(this.frmTransFee_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            this.tlpDetail.ResumeLayout(false);
            this.tlpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransFeeDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTransFeeValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPaymentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osTransFeeMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osChargeTransFeeFor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private System.Windows.Forms.TableLayoutPanel tlpDetail;
        private Infragistics.Win.Misc.UltraLabel lblTransFeeDesc;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransFeeDesc;
        private Infragistics.Win.Misc.UltraLabel lblPaymentType;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsActive;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor numTransFeeValue;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboPaymentType;
        public Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osTransFeeMode;
        private Infragistics.Win.Misc.UltraLabel lblChargeTransFeeFor;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osChargeTransFeeFor;
    }
}