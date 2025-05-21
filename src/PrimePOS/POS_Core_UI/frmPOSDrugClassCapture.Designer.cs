namespace POS_Core_UI
{
    partial class frmPOSDrugClassCapture
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
            if(disposing && (components != null))
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.lblOTCInfo = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpPatientInformation = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtCellPhone = new System.Windows.Forms.TextBox();
            this.txtDOB = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtPatientName = new System.Windows.Forms.TextBox();
            this.lblCellPhone = new System.Windows.Forms.Label();
            this.lblDOB = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.grpCustomerInformation = new Infragistics.Win.Misc.UltraGroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtState = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboRelation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIDNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboVerifID = new System.Windows.Forms.ComboBox();
            this.btncancel = new Infragistics.Win.Misc.UltraButton();
            this.btnContinue = new Infragistics.Win.Misc.UltraButton();
            this.dtpDriversLicenseExpDate = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.lblDriversLicenseExpDate = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpPatientInformation)).BeginInit();
            this.grpPatientInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCustomerInformation)).BeginInit();
            this.grpCustomerInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOTCInfo
            // 
            appearance1.FontData.Name = "Arial";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextVAlignAsString = "Middle";
            this.lblOTCInfo.Appearance = appearance1;
            this.lblOTCInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOTCInfo.Location = new System.Drawing.Point(6, 8);
            this.lblOTCInfo.Name = "lblOTCInfo";
            this.lblOTCInfo.Size = new System.Drawing.Size(511, 37);
            this.lblOTCInfo.TabIndex = 22;
            this.lblOTCInfo.Text = "There are some item (s) in this transaction that require a form of ID. Please sel" +
    "ect an ID type and enter its ID#.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grpPatientInformation);
            this.groupBox1.Controls.Add(this.grpCustomerInformation);
            this.groupBox1.Controls.Add(this.btncancel);
            this.groupBox1.Controls.Add(this.btnContinue);
            this.groupBox1.Controls.Add(this.lblOTCInfo);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 349);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // grpPatientInformation
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.grpPatientInformation.ContentAreaAppearance = appearance2;
            this.grpPatientInformation.Controls.Add(this.txtCellPhone);
            this.grpPatientInformation.Controls.Add(this.txtDOB);
            this.grpPatientInformation.Controls.Add(this.txtAddress);
            this.grpPatientInformation.Controls.Add(this.txtPhone);
            this.grpPatientInformation.Controls.Add(this.txtPatientName);
            this.grpPatientInformation.Controls.Add(this.lblCellPhone);
            this.grpPatientInformation.Controls.Add(this.lblDOB);
            this.grpPatientInformation.Controls.Add(this.lblAddress);
            this.grpPatientInformation.Controls.Add(this.lblPhone);
            this.grpPatientInformation.Controls.Add(this.lblPatientName);
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.SizeInPoints = 10F;
            appearance3.ForeColor = System.Drawing.Color.Blue;
            this.grpPatientInformation.HeaderAppearance = appearance3;
            this.grpPatientInformation.Location = new System.Drawing.Point(8, 190);
            this.grpPatientInformation.Name = "grpPatientInformation";
            this.grpPatientInformation.Size = new System.Drawing.Size(508, 110);
            this.grpPatientInformation.TabIndex = 26;
            this.grpPatientInformation.Tag = "NOCOLOR";
            this.grpPatientInformation.Text = "Patient Information";
            this.grpPatientInformation.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // txtCellPhone
            // 
            this.txtCellPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCellPhone.Location = new System.Drawing.Point(328, 55);
            this.txtCellPhone.Name = "txtCellPhone";
            this.txtCellPhone.ReadOnly = true;
            this.txtCellPhone.Size = new System.Drawing.Size(174, 20);
            this.txtCellPhone.TabIndex = 23;
            this.txtCellPhone.TabStop = false;
            // 
            // txtDOB
            // 
            this.txtDOB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDOB.Location = new System.Drawing.Point(328, 31);
            this.txtDOB.Name = "txtDOB";
            this.txtDOB.ReadOnly = true;
            this.txtDOB.Size = new System.Drawing.Size(174, 20);
            this.txtDOB.TabIndex = 22;
            this.txtDOB.TabStop = false;
            // 
            // txtAddress
            // 
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.Location = new System.Drawing.Point(80, 80);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(420, 20);
            this.txtAddress.TabIndex = 21;
            this.txtAddress.TabStop = false;
            // 
            // txtPhone
            // 
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Location = new System.Drawing.Point(80, 55);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.ReadOnly = true;
            this.txtPhone.Size = new System.Drawing.Size(174, 20);
            this.txtPhone.TabIndex = 20;
            this.txtPhone.TabStop = false;
            // 
            // txtPatientName
            // 
            this.txtPatientName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPatientName.Location = new System.Drawing.Point(80, 30);
            this.txtPatientName.Name = "txtPatientName";
            this.txtPatientName.ReadOnly = true;
            this.txtPatientName.Size = new System.Drawing.Size(174, 20);
            this.txtPatientName.TabIndex = 19;
            this.txtPatientName.TabStop = false;
            // 
            // lblCellPhone
            // 
            this.lblCellPhone.AutoSize = true;
            this.lblCellPhone.BackColor = System.Drawing.Color.Transparent;
            this.lblCellPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCellPhone.ForeColor = System.Drawing.Color.White;
            this.lblCellPhone.Location = new System.Drawing.Point(266, 59);
            this.lblCellPhone.Name = "lblCellPhone";
            this.lblCellPhone.Size = new System.Drawing.Size(61, 13);
            this.lblCellPhone.TabIndex = 18;
            this.lblCellPhone.Text = "Cell Phone:";
            // 
            // lblDOB
            // 
            this.lblDOB.AutoSize = true;
            this.lblDOB.BackColor = System.Drawing.Color.Transparent;
            this.lblDOB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDOB.ForeColor = System.Drawing.Color.White;
            this.lblDOB.Location = new System.Drawing.Point(294, 35);
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Size = new System.Drawing.Size(33, 13);
            this.lblDOB.TabIndex = 17;
            this.lblDOB.Text = "DOB:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.BackColor = System.Drawing.Color.Transparent;
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.ForeColor = System.Drawing.Color.White;
            this.lblAddress.Location = new System.Drawing.Point(32, 84);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 16;
            this.lblAddress.Text = "Address:";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.BackColor = System.Drawing.Color.Transparent;
            this.lblPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhone.ForeColor = System.Drawing.Color.White;
            this.lblPhone.Location = new System.Drawing.Point(39, 59);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(41, 13);
            this.lblPhone.TabIndex = 15;
            this.lblPhone.Text = "Phone:";
            // 
            // lblPatientName
            // 
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.BackColor = System.Drawing.Color.Transparent;
            this.lblPatientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.ForeColor = System.Drawing.Color.White;
            this.lblPatientName.Location = new System.Drawing.Point(37, 35);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(43, 13);
            this.lblPatientName.TabIndex = 14;
            this.lblPatientName.Text = "Patient:";
            // 
            // grpCustomerInformation
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            this.grpCustomerInformation.ContentAreaAppearance = appearance4;
            this.grpCustomerInformation.Controls.Add(this.lblDriversLicenseExpDate);
            this.grpCustomerInformation.Controls.Add(this.dtpDriversLicenseExpDate);
            this.grpCustomerInformation.Controls.Add(this.label6);
            this.grpCustomerInformation.Controls.Add(this.label5);
            this.grpCustomerInformation.Controls.Add(this.txtLastName);
            this.grpCustomerInformation.Controls.Add(this.txtFirstName);
            this.grpCustomerInformation.Controls.Add(this.label4);
            this.grpCustomerInformation.Controls.Add(this.txtState);
            this.grpCustomerInformation.Controls.Add(this.label3);
            this.grpCustomerInformation.Controls.Add(this.cboRelation);
            this.grpCustomerInformation.Controls.Add(this.label2);
            this.grpCustomerInformation.Controls.Add(this.txtIDNum);
            this.grpCustomerInformation.Controls.Add(this.label1);
            this.grpCustomerInformation.Controls.Add(this.cboVerifID);
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.SizeInPoints = 10F;
            appearance6.ForeColor = System.Drawing.Color.Blue;
            this.grpCustomerInformation.HeaderAppearance = appearance6;
            this.grpCustomerInformation.Location = new System.Drawing.Point(8, 49);
            this.grpCustomerInformation.Name = "grpCustomerInformation";
            this.grpCustomerInformation.Size = new System.Drawing.Size(508, 135);
            this.grpCustomerInformation.TabIndex = 25;
            this.grpCustomerInformation.Tag = "NOCOLOR";
            this.grpCustomerInformation.Text = "Customer Information";
            this.grpCustomerInformation.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2003;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(28, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "First Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(266, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Last Name:";
            // 
            // txtLastName
            // 
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastName.Location = new System.Drawing.Point(328, 81);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(168, 20);
            this.txtLastName.TabIndex = 5;
            // 
            // txtFirstName
            // 
            this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFirstName.Location = new System.Drawing.Point(88, 81);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(174, 20);
            this.txtFirstName.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(292, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "State:";
            // 
            // txtState
            // 
            this.txtState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtState.Location = new System.Drawing.Point(328, 56);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(39, 20);
            this.txtState.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(278, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Relation:";
            // 
            // cboRelation
            // 
            this.cboRelation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboRelation.FormattingEnabled = true;
            this.cboRelation.Items.AddRange(new object[] {
            "Patient",
            "Parent/Legal Guardian",
            "Spouse",
            "Caregiver",
            "Other"}); //PRIMEPOS-3267
            this.cboRelation.Location = new System.Drawing.Point(328, 30);
            this.cboRelation.Name = "cboRelation";
            this.cboRelation.Size = new System.Drawing.Size(168, 21);
            this.cboRelation.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(29, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Id Number:";
            // 
            // txtIDNum
            // 
            this.txtIDNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIDNum.Location = new System.Drawing.Point(88, 56);
            this.txtIDNum.Multiline = true;
            this.txtIDNum.Name = "txtIDNum";
            this.txtIDNum.Size = new System.Drawing.Size(174, 20);
            this.txtIDNum.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Verification ID:";
            // 
            // cboVerifID
            // 
            this.cboVerifID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboVerifID.FormattingEnabled = true;
            this.cboVerifID.Items.AddRange(new object[] {
            "Driver\'s License",
            "US Passport",
            "Permanent Resident Card",
            "Social Security Card",
            "Military ID",
            "State Issued ID",
            "Native American Tribal Documents(ID)",
            "Unique System ID",
            "Other"});
            this.cboVerifID.Location = new System.Drawing.Point(88, 30);
            this.cboVerifID.Name = "cboVerifID";
            this.cboVerifID.Size = new System.Drawing.Size(174, 21);
            this.cboVerifID.TabIndex = 0;
            // 
            // btncancel
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.btncancel.Appearance = appearance7;
            this.btncancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btncancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncancel.Location = new System.Drawing.Point(393, 305);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(116, 36);
            this.btncancel.TabIndex = 7;
            this.btncancel.Text = "Esc To Cancel";
            // 
            // btnContinue
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.btnContinue.Appearance = appearance8;
            this.btnContinue.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.Location = new System.Drawing.Point(260, 305);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(116, 36);
            this.btnContinue.TabIndex = 6;
            this.btnContinue.Text = "Continue";
            // 
            // dtpDriversLicenseExpDate
            // 
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.dtpDriversLicenseExpDate.Appearance = appearance5;
            this.dtpDriversLicenseExpDate.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Date;
            this.dtpDriversLicenseExpDate.Location = new System.Drawing.Point(88, 107);
            this.dtpDriversLicenseExpDate.Name = "dtpDriversLicenseExpDate";
            this.dtpDriversLicenseExpDate.NonAutoSizeHeight = 24;
            this.dtpDriversLicenseExpDate.Size = new System.Drawing.Size(100, 20);
            this.dtpDriversLicenseExpDate.TabIndex = 6;
            this.dtpDriversLicenseExpDate.Text = "//";
            // 
            // lblDriversLicenseExpDate
            // 
            this.lblDriversLicenseExpDate.ForeColor = System.Drawing.Color.White;
            this.lblDriversLicenseExpDate.Location = new System.Drawing.Point(4, 104);
            this.lblDriversLicenseExpDate.Name = "lblDriversLicenseExpDate";
            this.lblDriversLicenseExpDate.Size = new System.Drawing.Size(84, 27);
            this.lblDriversLicenseExpDate.TabIndex = 25;
            this.lblDriversLicenseExpDate.Text = "Driver\'s License Exp. Date:";
            this.lblDriversLicenseExpDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmPOSDrugClassCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(528, 359);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmPOSDrugClassCapture";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Authorization for Drug Class";
            this.Load += new System.EventHandler(this.frmPOSDrugClassCapture_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpPatientInformation)).EndInit();
            this.grpPatientInformation.ResumeLayout(false);
            this.grpPatientInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCustomerInformation)).EndInit();
            this.grpCustomerInformation.ResumeLayout(false);
            this.grpCustomerInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        private Infragistics.Win.Misc.UltraLabel lblOTCInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraGroupBox grpCustomerInformation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        internal Infragistics.Win.Misc.UltraButton btnContinue;
        internal Infragistics.Win.Misc.UltraButton btncancel;
        internal System.Windows.Forms.TextBox txtIDNum;
        internal System.Windows.Forms.ComboBox cboVerifID;
        internal System.Windows.Forms.ComboBox cboRelation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtLastName;
        internal System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.Misc.UltraGroupBox grpPatientInformation;
        private System.Windows.Forms.Label lblCellPhone;
        private System.Windows.Forms.Label lblDOB;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblPatientName;
        internal System.Windows.Forms.TextBox txtCellPhone;
        internal System.Windows.Forms.TextBox txtDOB;
        internal System.Windows.Forms.TextBox txtAddress;
        internal System.Windows.Forms.TextBox txtPhone;
        internal System.Windows.Forms.TextBox txtPatientName;
        private System.Windows.Forms.Label lblDriversLicenseExpDate;
        internal Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit dtpDriversLicenseExpDate;
    }
}