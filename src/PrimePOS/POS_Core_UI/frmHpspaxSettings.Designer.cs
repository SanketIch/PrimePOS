namespace POS_Core_UI
{
    partial class frmHpspaxSettings
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
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSaveConfig = new Infragistics.Win.Misc.UltraButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtVersion = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDeveloperID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtUsername = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDeviceID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtLicenseID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.txtSiteID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.tlpMain.SuspendLayout();
            this.tlpBottom.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeveloperID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviceID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicenseID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSiteID)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.lblTransactionType, 0, 0);
            this.tlpMain.Controls.Add(this.tlpBottom, 0, 3);
            this.tlpMain.Controls.Add(this.tableLayoutPanel1, 0, 2);
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
            this.tlpMain.Size = new System.Drawing.Size(892, 466);
            this.tlpMain.TabIndex = 1;
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
            this.lblTransactionType.Size = new System.Drawing.Size(884, 65);
            this.lblTransactionType.TabIndex = 3;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "HpsPax Settings";
            // 
            // tlpBottom
            // 
            this.tlpBottom.ColumnCount = 3;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 675F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tlpBottom.Controls.Add(this.btnClose, 2, 0);
            this.tlpBottom.Controls.Add(this.btnSaveConfig, 1, 0);
            this.tlpBottom.Location = new System.Drawing.Point(4, 383);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(884, 50);
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
            this.btnClose.Location = new System.Drawing.Point(785, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(111, 44);
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
            this.btnSaveConfig.Location = new System.Drawing.Point(678, 3);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(101, 44);
            this.btnSaveConfig.TabIndex = 3;
            this.btnSaveConfig.Tag = "NOCOLOR";
            this.btnSaveConfig.Text = "Ok";
            this.btnSaveConfig.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSaveConfig.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.txtVersion, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtDeveloperID, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtUsername, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDeviceID, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtLicenseID, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtSiteID, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 136);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 240);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // txtVersion
            // 
            this.txtVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVersion.Location = new System.Drawing.Point(445, 202);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(436, 26);
            this.txtVersion.TabIndex = 13;
            // 
            // txtDeveloperID
            // 
            this.txtDeveloperID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeveloperID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeveloperID.Location = new System.Drawing.Point(445, 168);
            this.txtDeveloperID.Name = "txtDeveloperID";
            this.txtDeveloperID.Size = new System.Drawing.Size(436, 26);
            this.txtDeveloperID.TabIndex = 12;
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(445, 133);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(436, 26);
            this.txtPassword.TabIndex = 11;
            // 
            // txtUsername
            // 
            this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(445, 97);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(436, 26);
            this.txtUsername.TabIndex = 10;
            // 
            // txtDeviceID
            // 
            this.txtDeviceID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeviceID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeviceID.Location = new System.Drawing.Point(445, 64);
            this.txtDeviceID.Name = "txtDeviceID";
            this.txtDeviceID.Size = new System.Drawing.Size(436, 26);
            this.txtDeviceID.TabIndex = 9;
            // 
            // txtLicenseID
            // 
            this.txtLicenseID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLicenseID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLicenseID.Location = new System.Drawing.Point(445, 35);
            this.txtLicenseID.Name = "txtLicenseID";
            this.txtLicenseID.Size = new System.Drawing.Size(436, 26);
            this.txtLicenseID.TabIndex = 8;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(3, 3);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(436, 26);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "SiteID";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(3, 35);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(436, 23);
            this.ultraLabel2.TabIndex = 1;
            this.ultraLabel2.Text = "LicenseID";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(3, 64);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(436, 27);
            this.ultraLabel3.TabIndex = 2;
            this.ultraLabel3.Text = "DeviceID";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(3, 97);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(436, 30);
            this.ultraLabel4.TabIndex = 3;
            this.ultraLabel4.Text = "Username";
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(3, 133);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(436, 29);
            this.ultraLabel5.TabIndex = 4;
            this.ultraLabel5.Text = "Password";
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(3, 168);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(436, 28);
            this.ultraLabel6.TabIndex = 5;
            this.ultraLabel6.Text = "DeveloperId";
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(3, 202);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(436, 35);
            this.ultraLabel7.TabIndex = 6;
            this.ultraLabel7.Text = "VersionNumber";
            // 
            // txtSiteID
            // 
            this.txtSiteID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSiteID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSiteID.Location = new System.Drawing.Point(445, 3);
            this.txtSiteID.Name = "txtSiteID";
            this.txtSiteID.Size = new System.Drawing.Size(436, 26);
            this.txtSiteID.TabIndex = 7;
            // 
            // frmHpspaxSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.tlpMain);
            this.Name = "frmHpspaxSettings";
            this.Text = "Hpspax Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHpspaxSettings_FormClosed);
            this.Load += new System.EventHandler(this.frmHpspaxSettings_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpBottom.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeveloperID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviceID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicenseID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSiteID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSaveConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVersion;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeveloperID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPassword;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUsername;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDeviceID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLicenseID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSiteID;
    }
}