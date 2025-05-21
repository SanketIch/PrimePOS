namespace POS_Core_UI
{
    partial class frmPrimeRxPaySettings
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.tlpDetails = new System.Windows.Forms.TableLayoutPanel();
            this.tlpOuterLeft = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.tlpLeft = new System.Windows.Forms.TableLayoutPanel();
            this.cmbOnlineOption = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblOnlineOption = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtExpiryInMints = new System.Windows.Forms.TextBox();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSaveConfig = new Infragistics.Win.Misc.UltraButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tlpMain.SuspendLayout();
            this.tlpDetails.SuspendLayout();
            this.tlpOuterLeft.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.tlpLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOnlineOption)).BeginInit();
            this.tlpBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.lblTransactionType, 0, 0);
            this.tlpMain.Controls.Add(this.tlpDetails, 0, 2);
            this.tlpMain.Controls.Add(this.tlpBottom, 0, 3);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(4);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.881775F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.90148F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.15748F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.771654F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.20472F));
            this.tlpMain.Size = new System.Drawing.Size(544, 207);
            this.tlpMain.TabIndex = 0;
            this.tlpMain.Tag = "";
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
            this.lblTransactionType.Location = new System.Drawing.Point(4, 4);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Padding = new System.Drawing.Size(10, 0);
            this.lblTransactionType.Size = new System.Drawing.Size(536, 25);
            this.lblTransactionType.TabIndex = 3;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "PrimeRxPay Settings";
            // 
            // tlpDetails
            // 
            this.tlpDetails.ColumnCount = 3;
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 96.44195F));
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.621723F));
            this.tlpDetails.Controls.Add(this.tlpOuterLeft, 1, 0);
            this.tlpDetails.Location = new System.Drawing.Point(5, 64);
            this.tlpDetails.Margin = new System.Windows.Forms.Padding(4);
            this.tlpDetails.Name = "tlpDetails";
            this.tlpDetails.RowCount = 2;
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpDetails.Size = new System.Drawing.Size(534, 88);
            this.tlpDetails.TabIndex = 1;
            // 
            // tlpOuterLeft
            // 
            this.tlpOuterLeft.ColumnCount = 1;
            this.tlpOuterLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpOuterLeft.Controls.Add(this.pnlLeft, 0, 0);
            this.tlpOuterLeft.Location = new System.Drawing.Point(5, 0);
            this.tlpOuterLeft.Margin = new System.Windows.Forms.Padding(0);
            this.tlpOuterLeft.Name = "tlpOuterLeft";
            this.tlpOuterLeft.RowCount = 1;
            this.tlpOuterLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.69154F));
            this.tlpOuterLeft.Size = new System.Drawing.Size(514, 65);
            this.tlpOuterLeft.TabIndex = 2;
            // 
            // pnlLeft
            // 
            this.pnlLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLeft.Controls.Add(this.tlpLeft);
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(514, 65);
            this.pnlLeft.TabIndex = 2;
            // 
            // tlpLeft
            // 
            this.tlpLeft.ColumnCount = 2;
            this.tlpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.26403F));
            this.tlpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 264F));
            this.tlpLeft.Controls.Add(this.cmbOnlineOption, 1, 1);
            this.tlpLeft.Controls.Add(this.lblOnlineOption, 0, 1);
            this.tlpLeft.Controls.Add(this.ultraLabel1, 0, 0);
            this.tlpLeft.Controls.Add(this.txtExpiryInMints, 1, 0);
            this.tlpLeft.Location = new System.Drawing.Point(0, 0);
            this.tlpLeft.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLeft.Name = "tlpLeft";
            this.tlpLeft.RowCount = 3;
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.81818F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpLeft.Size = new System.Drawing.Size(514, 81);
            this.tlpLeft.TabIndex = 0;
            // 
            // cmbOnlineOption
            // 
            this.cmbOnlineOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbOnlineOption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOnlineOption.Location = new System.Drawing.Point(253, 29);
            this.cmbOnlineOption.Name = "cmbOnlineOption";
            this.cmbOnlineOption.Size = new System.Drawing.Size(258, 25);
            this.cmbOnlineOption.TabIndex = 1;
            // 
            // lblOnlineOption
            // 
            this.lblOnlineOption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnlineOption.Location = new System.Drawing.Point(3, 29);
            this.lblOnlineOption.Name = "lblOnlineOption";
            this.lblOnlineOption.Size = new System.Drawing.Size(244, 16);
            this.lblOnlineOption.TabIndex = 0;
            this.lblOnlineOption.Text = "Choose Option for Online Payment";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(3, 3);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(244, 20);
            this.ultraLabel1.TabIndex = 7;
            this.ultraLabel1.Text = "Link Expiry In Minutes";
            // 
            // txtExpiryInMints
            // 
            this.txtExpiryInMints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExpiryInMints.Location = new System.Drawing.Point(253, 3);
            this.txtExpiryInMints.Name = "txtExpiryInMints";
            this.txtExpiryInMints.Size = new System.Drawing.Size(258, 21);
            this.txtExpiryInMints.TabIndex = 8;
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 10;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpBottom.Controls.Add(this.btnClose, 8, 0);
            this.tlpBottom.Controls.Add(this.btnSaveConfig, 6, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(4, 171);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(536, 32);
            this.tlpBottom.TabIndex = 5;
            // 
            // btnClose
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnClose.HotTrackAppearance = appearance3;
            this.btnClose.Location = new System.Drawing.Point(404, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 26);
            this.btnClose.TabIndex = 4;
            this.btnClose.Tag = "NOCOLOR";
            this.btnClose.Text = "Close";
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveConfig
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(168)))), ((int)(((byte)(90)))));
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.btnSaveConfig.Appearance = appearance4;
            this.btnSaveConfig.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSaveConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSaveConfig.HotTrackAppearance = appearance5;
            this.btnSaveConfig.Location = new System.Drawing.Point(274, 3);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(114, 26);
            this.btnSaveConfig.TabIndex = 3;
            this.btnSaveConfig.Tag = "NOCOLOR";
            this.btnSaveConfig.Text = "Ok";
            this.btnSaveConfig.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSaveConfig.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmPrimeRxPaySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 207);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrimeRxPaySettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PrimeRxPay Settings";
            this.Load += new System.EventHandler(this.frmPrimeRxPaySettings_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPrimeRxPaySettings_FormClosed);
            this.tlpMain.ResumeLayout(false);
            this.tlpDetails.ResumeLayout(false);
            this.tlpOuterLeft.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.tlpLeft.ResumeLayout(false);
            this.tlpLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOnlineOption)).EndInit();
            this.tlpBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private Infragistics.Win.Misc.UltraButton btnSaveConfig;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tlpDetails;
        private System.Windows.Forms.TableLayoutPanel tlpOuterLeft;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.TableLayoutPanel tlpLeft;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.TextBox txtExpiryInMints;
        private Infragistics.Win.Misc.UltraLabel lblOnlineOption;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOnlineOption;
    }
}