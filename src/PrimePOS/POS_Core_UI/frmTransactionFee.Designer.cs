
namespace POS_Core_UI
{
    partial class frmTransactionFee
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
            #region PRIMEPOS-3234
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            #endregion
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnWaive = new Infragistics.Win.Misc.UltraButton(); //PRIMEPOS-3234
            this.tlpDetail = new System.Windows.Forms.TableLayoutPanel();
            this.lblFinalAmt = new Infragistics.Win.Misc.UltraLabel();
            this.lblFinalAmount = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionFee = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalAmount = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransFeeAmt = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalAmt = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.tlpMain.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.tlpDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tlpBottom, 0, 1);
            this.tlpMain.Controls.Add(this.tlpDetail, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(5);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(289, 136);
            this.tlpMain.TabIndex = 1;
            this.tlpMain.Tag = "";
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 3; 
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 113F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 113F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 113F)); //PRIMEPOS-3234
            this.tlpBottom.Controls.Add(this.btnOk, 0, 0);
            this.tlpBottom.Controls.Add(this.btnWaive, 1, 0); //PRIMEPOS-3234
            this.tlpBottom.Controls.Add(this.btnCancel, 2, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(4, 98);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpBottom.Size = new System.Drawing.Size(281, 34);
            this.tlpBottom.TabIndex = 0;
            // 
            // btnOk
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(168)))), ((int)(((byte)(90)))));
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance1;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnOk.HotTrackAppearance = appearance2;
            this.btnOk.Location = new System.Drawing.Point(43, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(115, 28);
            this.btnOk.TabIndex = 0;
            this.btnOk.Tag = "NOCOLOR";
            this.btnOk.Text = "Ok";
            this.btnOk.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Appearance = appearance3;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnCancel.HotTrackAppearance = appearance4;
            this.btnCancel.Location = new System.Drawing.Point(164, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Tag = "NOCOLOR";
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            #region PRIMEPOS-3234
            // 
            // btnWaive
            // 
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            appearance11.ForeColor = System.Drawing.Color.Black;
            this.btnWaive.Appearance = appearance11;
            this.btnWaive.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnWaive.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnWaive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWaive.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnWaive.HotTrackAppearance = appearance12;
            this.btnWaive.Name = "btnWaive";
            this.btnWaive.Enabled = false;
            this.btnWaive.Size = new System.Drawing.Size(114, 28);
            this.btnWaive.TabIndex = 2;
            this.btnWaive.Tag = "NOCOLOR";
            this.btnWaive.Text = "Waive Fee";
            this.btnWaive.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnWaive.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnWaive.Click += new System.EventHandler(this.btnWaive_Click);
            #endregion
            // 
            // tlpDetail
            // 
            this.tlpDetail.ColumnCount = 3;
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.16216F));
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.91892F));
            this.tlpDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.91892F));
            this.tlpDetail.Controls.Add(this.lblFinalAmt, 1, 2);
            this.tlpDetail.Controls.Add(this.lblFinalAmount, 0, 2);
            this.tlpDetail.Controls.Add(this.lblTransactionFee, 0, 1);
            this.tlpDetail.Controls.Add(this.lblTotalAmount, 0, 0);
            this.tlpDetail.Controls.Add(this.lblTransFeeAmt, 1, 1);
            this.tlpDetail.Controls.Add(this.lblTotalAmt, 1, 0);
            this.tlpDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetail.Location = new System.Drawing.Point(4, 4);
            this.tlpDetail.Name = "tlpDetail";
            this.tlpDetail.RowCount = 3;
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpDetail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpDetail.Size = new System.Drawing.Size(281, 87);
            this.tlpDetail.TabIndex = 4;
            // 
            // lblFinalAmt
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.lblFinalAmt.Appearance = appearance5;
            this.lblFinalAmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinalAmt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalAmt.Location = new System.Drawing.Point(174, 57);
            this.lblFinalAmt.Margin = new System.Windows.Forms.Padding(0);
            this.lblFinalAmt.Name = "lblFinalAmt";
            this.lblFinalAmt.Size = new System.Drawing.Size(107, 30);
            this.lblFinalAmt.TabIndex = 15;
            this.lblFinalAmt.Tag = "TAGTOTAL";
            // 
            // lblFinalAmount
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Right";
            appearance6.TextVAlignAsString = "Middle";
            this.lblFinalAmount.Appearance = appearance6;
            this.lblFinalAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinalAmount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalAmount.Location = new System.Drawing.Point(0, 57);
            this.lblFinalAmount.Margin = new System.Windows.Forms.Padding(0);
            this.lblFinalAmount.Name = "lblFinalAmount";
            this.lblFinalAmount.Size = new System.Drawing.Size(174, 30);
            this.lblFinalAmount.TabIndex = 14;
            this.lblFinalAmount.Tag = "TAGTOTAL";
            this.lblFinalAmount.Text = "Final Amount";
            // 
            // lblTransactionFee
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Right";
            appearance7.TextVAlignAsString = "Middle";
            this.lblTransactionFee.Appearance = appearance7;
            this.lblTransactionFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransactionFee.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionFee.Location = new System.Drawing.Point(3, 31);
            this.lblTransactionFee.Name = "lblTransactionFee";
            this.lblTransactionFee.Size = new System.Drawing.Size(168, 23);
            this.lblTransactionFee.TabIndex = 13;
            this.lblTransactionFee.Tag = "TAGSUBTOTAL";
            this.lblTransactionFee.Text = "Transaction Fee";
            // 
            // lblTotalAmount
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.TextHAlignAsString = "Right";
            appearance8.TextVAlignAsString = "Middle";
            this.lblTotalAmount.Appearance = appearance8;
            this.lblTotalAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(3, 3);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(168, 22);
            this.lblTotalAmount.TabIndex = 12;
            this.lblTotalAmount.Tag = "TAGSUBTOTAL";
            this.lblTotalAmount.Text = "Total Amount";
            // 
            // lblTransFeeAmt
            // 
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.TextHAlignAsString = "Right";
            appearance9.TextVAlignAsString = "Middle";
            this.lblTransFeeAmt.Appearance = appearance9;
            this.lblTransFeeAmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTransFeeAmt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransFeeAmt.Location = new System.Drawing.Point(177, 31);
            this.lblTransFeeAmt.Name = "lblTransFeeAmt";
            this.lblTransFeeAmt.Size = new System.Drawing.Size(101, 23);
            this.lblTransFeeAmt.TabIndex = 9;
            this.lblTransFeeAmt.Tag = "TAGSUBTOTAL";
            // 
            // lblTotalAmt
            // 
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.TextHAlignAsString = "Right";
            appearance10.TextVAlignAsString = "Middle";
            this.lblTotalAmt.Appearance = appearance10;
            this.lblTotalAmt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalAmt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmt.Location = new System.Drawing.Point(177, 3);
            this.lblTotalAmt.Name = "lblTotalAmt";
            this.lblTotalAmt.Size = new System.Drawing.Size(101, 22);
            this.lblTotalAmt.TabIndex = 8;
            this.lblTotalAmt.Tag = "TAGSUBTOTAL";
            // 
            // frmTransactionFee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 136);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransactionFee";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Fee";
            this.Load += new System.EventHandler(this.frmTransactionFee_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTransactionFee_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.tlpMain.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            this.tlpDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnWaive; //PRIMEPOS-3234
        private System.Windows.Forms.TableLayoutPanel tlpDetail;
        private Infragistics.Win.Misc.UltraLabel lblTransFeeAmt;
        private Infragistics.Win.Misc.UltraLabel lblTotalAmt;
        private Infragistics.Win.Misc.UltraLabel lblTransactionFee;
        private Infragistics.Win.Misc.UltraLabel lblTotalAmount;
        private Infragistics.Win.Misc.UltraLabel lblFinalAmount;
        private Infragistics.Win.Misc.UltraLabel lblFinalAmt;
    }
}