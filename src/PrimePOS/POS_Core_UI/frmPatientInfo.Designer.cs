namespace POS_Core_UI
{
    partial class frmPatientInfo
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
            Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton dateButton1 = new Infragistics.Win.UltraWinSchedule.CalendarCombo.DateButton();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this.ultraLabel26 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.txtPatName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtBalance = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblBalance = new Infragistics.Win.Misc.UltraLabel();
            this.txtPatAddress = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPatPhone = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPatGender = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.dtpPatDOB = new Infragistics.Win.UltraWinSchedule.UltraCalendarCombo();
            this.lblDriversLicense = new Infragistics.Win.Misc.UltraLabel();
            this.txtDriversLicense = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblDriversLicenseExpDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtpDriversLicenseExpDate = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.dtpInputPatDOB = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.lblMatch = new Infragistics.Win.Misc.UltraLabel();
            this.btnMatch = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.pnlInputDOB = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlLicense = new System.Windows.Forms.Panel();
            this.pnlPatientData = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatGender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPatDOB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriversLicense)).BeginInit();
            this.pnlInputDOB.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlLicense.SuspendLayout();
            this.pnlPatientData.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraLabel26
            // 
            this.ultraLabel26.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel26.Name = "ultraLabel26";
            this.ultraLabel26.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel26.TabIndex = 0;
            // 
            // lblTransactionType
            // 
            appearance1.BackColor = System.Drawing.Color.LightCyan;
            appearance1.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.ForeColor = System.Drawing.Color.Azure;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 40);
            this.lblTransactionType.TabIndex = 0;
            this.lblTransactionType.Text = "Patient Information";
            // 
            // txtPatName
            // 
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.txtPatName.Appearance = appearance2;
            this.txtPatName.AutoSize = false;
            this.txtPatName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPatName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatName.Location = new System.Drawing.Point(115, 10);
            this.txtPatName.MaxLength = 20;
            this.txtPatName.Name = "txtPatName";
            this.txtPatName.ReadOnly = true;
            this.txtPatName.Size = new System.Drawing.Size(301, 20);
            this.txtPatName.TabIndex = 0;
            this.txtPatName.TabStop = false;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(5, 12);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(45, 17);
            this.ultraLabel2.TabIndex = 8;
            this.ultraLabel2.Text = "Name";
            // 
            // txtBalance
            // 
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.txtBalance.Appearance = appearance3;
            this.txtBalance.AutoSize = false;
            this.txtBalance.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtBalance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalance.Location = new System.Drawing.Point(115, 155);
            this.txtBalance.MaxLength = 20;
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size(140, 20);
            this.txtBalance.TabIndex = 5;
            this.txtBalance.TabStop = false;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(5, 47);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(60, 17);
            this.ultraLabel3.TabIndex = 9;
            this.ultraLabel3.Text = "Address";
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.Location = new System.Drawing.Point(5, 157);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(82, 17);
            this.lblBalance.TabIndex = 20;
            this.lblBalance.Text = "HC Balance";
            // 
            // txtPatAddress
            // 
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.txtPatAddress.Appearance = appearance4;
            this.txtPatAddress.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPatAddress.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatAddress.Location = new System.Drawing.Point(115, 35);
            this.txtPatAddress.MaxLength = 20;
            this.txtPatAddress.Multiline = true;
            this.txtPatAddress.Name = "txtPatAddress";
            this.txtPatAddress.ReadOnly = true;
            this.txtPatAddress.Size = new System.Drawing.Size(301, 40);
            this.txtPatAddress.TabIndex = 1;
            this.txtPatAddress.TabStop = false;
            // 
            // txtPatPhone
            // 
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.txtPatPhone.Appearance = appearance5;
            this.txtPatPhone.AutoSize = false;
            this.txtPatPhone.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPatPhone.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatPhone.Location = new System.Drawing.Point(115, 80);
            this.txtPatPhone.MaxLength = 20;
            this.txtPatPhone.Name = "txtPatPhone";
            this.txtPatPhone.ReadOnly = true;
            this.txtPatPhone.Size = new System.Drawing.Size(301, 20);
            this.txtPatPhone.TabIndex = 2;
            this.txtPatPhone.TabStop = false;
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.AutoSize = true;
            this.ultraLabel6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel6.Location = new System.Drawing.Point(5, 107);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(55, 17);
            this.ultraLabel6.TabIndex = 15;
            this.ultraLabel6.Text = "Gender";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(5, 82);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(63, 17);
            this.ultraLabel1.TabIndex = 19;
            this.ultraLabel1.Text = "Phone #";
            // 
            // txtPatGender
            // 
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.txtPatGender.Appearance = appearance6;
            this.txtPatGender.AutoSize = false;
            this.txtPatGender.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPatGender.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatGender.Location = new System.Drawing.Point(115, 105);
            this.txtPatGender.MaxLength = 20;
            this.txtPatGender.Name = "txtPatGender";
            this.txtPatGender.ReadOnly = true;
            this.txtPatGender.Size = new System.Drawing.Size(140, 20);
            this.txtPatGender.TabIndex = 3;
            this.txtPatGender.TabStop = false;
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.AutoSize = true;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(5, 132);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(35, 17);
            this.ultraLabel7.TabIndex = 17;
            this.ultraLabel7.Text = "DOB";
            // 
            // dtpPatDOB
            // 
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.ItalicAsString = "False";
            appearance7.FontData.StrikeoutAsString = "False";
            appearance7.FontData.UnderlineAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.dtpPatDOB.Appearance = appearance7;
            this.dtpPatDOB.AutoSize = false;
            this.dtpPatDOB.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.dtpPatDOB.DateButtons.Add(dateButton1);
            this.dtpPatDOB.Enabled = false;
            this.dtpPatDOB.Location = new System.Drawing.Point(115, 130);
            this.dtpPatDOB.Name = "dtpPatDOB";
            this.dtpPatDOB.NonAutoSizeHeight = 10;
            this.dtpPatDOB.ReadOnly = true;
            this.dtpPatDOB.Size = new System.Drawing.Size(140, 20);
            this.dtpPatDOB.TabIndex = 4;
            this.dtpPatDOB.TabStop = false;
            this.dtpPatDOB.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtpPatDOB.Value = new System.DateTime(2004, 5, 25, 0, 0, 0, 0);
            // 
            // lblDriversLicense
            // 
            this.lblDriversLicense.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriversLicense.Location = new System.Drawing.Point(5, 12);
            this.lblDriversLicense.Name = "lblDriversLicense";
            this.lblDriversLicense.Size = new System.Drawing.Size(115, 17);
            this.lblDriversLicense.TabIndex = 24;
            this.lblDriversLicense.Text = "Driver\'s License";
            // 
            // txtDriversLicense
            // 
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.txtDriversLicense.Appearance = appearance8;
            this.txtDriversLicense.AutoSize = false;
            this.txtDriversLicense.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtDriversLicense.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDriversLicense.Location = new System.Drawing.Point(194, 10);
            this.txtDriversLicense.Multiline = true;
            this.txtDriversLicense.Name = "txtDriversLicense";
            this.txtDriversLicense.Size = new System.Drawing.Size(140, 20);
            this.txtDriversLicense.TabIndex = 0;
            this.txtDriversLicense.TextChanged += new System.EventHandler(this.txtDriversLicense_TextChanged);
            // 
            // lblDriversLicenseExpDate
            // 
            this.lblDriversLicenseExpDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriversLicenseExpDate.Location = new System.Drawing.Point(5, 42);
            this.lblDriversLicenseExpDate.Name = "lblDriversLicenseExpDate";
            this.lblDriversLicenseExpDate.Size = new System.Drawing.Size(190, 17);
            this.lblDriversLicenseExpDate.TabIndex = 22;
            this.lblDriversLicenseExpDate.Text = "Driver\'s License Exp. Date";
            // 
            // dtpDriversLicenseExpDate
            // 
            appearance9.ForeColor = System.Drawing.Color.Black;
            this.dtpDriversLicenseExpDate.Appearance = appearance9;
            this.dtpDriversLicenseExpDate.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Date;
            this.dtpDriversLicenseExpDate.Location = new System.Drawing.Point(194, 40);
            this.dtpDriversLicenseExpDate.Name = "dtpDriversLicenseExpDate";
            this.dtpDriversLicenseExpDate.NonAutoSizeHeight = 24;
            this.dtpDriversLicenseExpDate.Size = new System.Drawing.Size(140, 20);
            this.dtpDriversLicenseExpDate.TabIndex = 1;
            this.dtpDriversLicenseExpDate.Text = "//";
            // 
            // btnCancel
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Appearance = appearance10;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(315, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 28);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Continue";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Visible = false;
            // 
            // btnOk
            // 
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance11.FontData.BoldAsString = "True";
            appearance11.ForeColor = System.Drawing.Color.White;
            this.btnOk.Appearance = appearance11;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(315, 14);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 28);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dtpInputPatDOB
            // 
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.dtpInputPatDOB.Appearance = appearance12;
            this.dtpInputPatDOB.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Date;
            this.dtpInputPatDOB.Location = new System.Drawing.Point(115, 10);
            this.dtpInputPatDOB.Name = "dtpInputPatDOB";
            this.dtpInputPatDOB.NonAutoSizeHeight = 24;
            this.dtpInputPatDOB.Size = new System.Drawing.Size(140, 20);
            this.dtpInputPatDOB.TabIndex = 0;
            this.dtpInputPatDOB.Text = "//";
            this.dtpInputPatDOB.Leave += new System.EventHandler(this.dtpInputPatDOB_Leave);
            // 
            // lblMatch
            // 
            appearance13.FontData.BoldAsString = "True";
            appearance13.ForeColor = System.Drawing.Color.Red;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.lblMatch.Appearance = appearance13;
            this.lblMatch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMatch.Location = new System.Drawing.Point(0, 48);
            this.lblMatch.Name = "lblMatch";
            this.lblMatch.Size = new System.Drawing.Size(422, 20);
            this.lblMatch.TabIndex = 19;
            this.lblMatch.Tag = "NOCOLOR";
            // 
            // btnMatch
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.White;
            this.btnMatch.Appearance = appearance14;
            this.btnMatch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnMatch.Location = new System.Drawing.Point(268, 6);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.Size = new System.Drawing.Size(116, 28);
            this.btnMatch.TabIndex = 1;
            this.btnMatch.Text = "&Match DOB";
            this.btnMatch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click);
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(5, 14);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(78, 17);
            this.ultraLabel4.TabIndex = 18;
            this.ultraLabel4.Text = "Input DOB";
            // 
            // pnlInputDOB
            // 
            this.pnlInputDOB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInputDOB.Controls.Add(this.lblMatch);
            this.pnlInputDOB.Controls.Add(this.dtpInputPatDOB);
            this.pnlInputDOB.Controls.Add(this.ultraLabel4);
            this.pnlInputDOB.Controls.Add(this.btnMatch);
            this.pnlInputDOB.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInputDOB.Location = new System.Drawing.Point(0, 40);
            this.pnlInputDOB.Name = "pnlInputDOB";
            this.pnlInputDOB.Size = new System.Drawing.Size(424, 70);
            this.pnlInputDOB.TabIndex = 1;
            // 
            // pnlButtons
            // 
            this.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 365);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(424, 50);
            this.pnlButtons.TabIndex = 4;
            // 
            // pnlLicense
            // 
            this.pnlLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLicense.Controls.Add(this.lblDriversLicense);
            this.pnlLicense.Controls.Add(this.dtpDriversLicenseExpDate);
            this.pnlLicense.Controls.Add(this.txtDriversLicense);
            this.pnlLicense.Controls.Add(this.lblDriversLicenseExpDate);
            this.pnlLicense.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLicense.Location = new System.Drawing.Point(0, 110);
            this.pnlLicense.Name = "pnlLicense";
            this.pnlLicense.Size = new System.Drawing.Size(424, 70);
            this.pnlLicense.TabIndex = 2;
            // 
            // pnlPatientData
            // 
            this.pnlPatientData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPatientData.Controls.Add(this.txtPatName);
            this.pnlPatientData.Controls.Add(this.txtPatAddress);
            this.pnlPatientData.Controls.Add(this.ultraLabel2);
            this.pnlPatientData.Controls.Add(this.dtpPatDOB);
            this.pnlPatientData.Controls.Add(this.txtBalance);
            this.pnlPatientData.Controls.Add(this.ultraLabel7);
            this.pnlPatientData.Controls.Add(this.ultraLabel3);
            this.pnlPatientData.Controls.Add(this.txtPatGender);
            this.pnlPatientData.Controls.Add(this.lblBalance);
            this.pnlPatientData.Controls.Add(this.ultraLabel1);
            this.pnlPatientData.Controls.Add(this.ultraLabel6);
            this.pnlPatientData.Controls.Add(this.txtPatPhone);
            this.pnlPatientData.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPatientData.Location = new System.Drawing.Point(0, 180);
            this.pnlPatientData.Name = "pnlPatientData";
            this.pnlPatientData.Size = new System.Drawing.Size(424, 185);
            this.pnlPatientData.TabIndex = 3;
            // 
            // frmPatientInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 415);
            this.Controls.Add(this.pnlPatientData);
            this.Controls.Add(this.pnlLicense);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlInputDOB);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPatientInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Patient Information";
            this.Load += new System.EventHandler(this.frmPatientInfo_Load);
            this.Shown += new System.EventHandler(this.frmPatientInfo_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPatientInfo_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBalance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatGender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPatDOB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriversLicense)).EndInit();
            this.pnlInputDOB.ResumeLayout(false);
            this.pnlInputDOB.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlLicense.ResumeLayout(false);
            this.pnlLicense.PerformLayout();
            this.pnlPatientData.ResumeLayout(false);
            this.pnlPatientData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel26;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPatAddress;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPatName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPatGender;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.UltraWinSchedule.UltraCalendarCombo dtpPatDOB;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPatPhone;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtBalance;
        private Infragistics.Win.Misc.UltraLabel lblBalance;
        private Infragistics.Win.Misc.UltraButton btnOk;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraButton btnMatch;
        private Infragistics.Win.Misc.UltraLabel lblMatch;
        private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit dtpInputPatDOB;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDriversLicense;
        private Infragistics.Win.Misc.UltraLabel lblDriversLicense;
        private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit dtpDriversLicenseExpDate;
        private Infragistics.Win.Misc.UltraLabel lblDriversLicenseExpDate;
        private System.Windows.Forms.Panel pnlInputDOB;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlLicense;
        private System.Windows.Forms.Panel pnlPatientData;
    }
}