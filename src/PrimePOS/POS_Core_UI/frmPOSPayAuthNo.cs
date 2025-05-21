using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
//using POS_Core.DataAccess;
using System;
using System.Windows.Forms;
using NLog;
using POS_Core.TransType;
using POS_Core.Resources;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPOSChangeDue.
    /// </summary>
    public class frmPOSPayAuthNo : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private POSTransactionType oTransType = POSTransactionType.Sales;
        private Infragistics.Win.Misc.UltraLabel lblOTCInfo;
        private GroupBox groupBox1;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor cmbVerificationID;
        private Infragistics.Win.Misc.UltraLabel lblVerificationID;
        private Infragistics.Win.Misc.UltraButton btnAuthCancel;
        private Infragistics.Win.Misc.UltraButton btnAuthOverride; //PRIMEPOS-3166
        public TextBox txtAuthorizationNo;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraLabel lblAuthorization;
        public bool isCancelled = false;
        private bool isScan = false; //Added by Manoj 7/16/2013
        private bool wasScan = false; //Added By Manoj 9/16/2013
        public POS_Core.Business_Tier.DL ID = null;
        //Added by Manoj 7/16/2013
        private string data;
        private string _dob;

        //Added By Manoj 9/12/2013
        private delegate void ScanEventHandler();
        private event ScanEventHandler ScanData;
        private event ScanEventHandler AfterScanData;
        //private DateTimePicker dtpDob = null; //PRIMEPOS-2693 13-Jun-2019 JY Commented
        private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit dtpDob = null;  //PRIMEPOS-2693 13-Jun-2019 JY Added
        private Label lblDob = null;
        public int CustomerId = 0;
        private Infragistics.Win.Misc.UltraGroupBox gbCustAddress;
        private Infragistics.Win.Misc.UltraLabel lblZip;
        private Infragistics.Win.Misc.UltraLabel lblState;
        private Infragistics.Win.Misc.UltraLabel lblCity;
        private Infragistics.Win.Misc.UltraLabel lblAddress;
        private TextBox txtState;
        private TextBox txtCity;
        private TextBox txtAddress;
        private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtZipCode;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region PRIMEPOS-2821 03-Nov-2020 JY Added
        public bool bCalledForNplex = false;
        public CustomerRow oCustAddress;
        public bool isOverrideMonitorItem = false; //PRIMEPOS-3166
        public string monitorItemOverriddenBy; //PRIMEPOS-3166
        private string LastName;
        private string FirstName;
        #endregion

        public string Dob //Added by Manoj 7/16/2013
        {
            get { return _dob; }
            set
            {
                _dob = value;
                Configuration.strDOB = _dob;    //PRIMEPOS-2729 06-Sep-2019 JY Added
            }
        }

        /// <summary>
        /// Get or Set Age Limit requirement so it can accept
        /// user input of Date Of Birth.
        /// </summary>
        public bool isAgeLimit //Added by Manoj 9/16/2013
        {
            get;
            set;
        }

        public bool showOverrideBtn //PRIMEPOS-3166N
        {
            get;
            set;
        }

        public frmPOSPayAuthNo(POSTransactionType oTType)
        {
            InitializeComponent();
            ClearFields();  //PRIMEPOS-2821 03-Nov-2020 JY Added
            oTransType = oTType;

            #region

            this.groupBox1.Location = new System.Drawing.Point(8, 7);
            this.groupBox1.Size = new System.Drawing.Size(461, 84);
            this.lblAuthorization.Location = new System.Drawing.Point(62, 32);
            this.lblAuthorization.Size = new System.Drawing.Size(176, 26);

            this.btnClose.Location = new System.Drawing.Point(62, 100);
            this.btnClose.Size = new System.Drawing.Size(136, 26);

            this.btnAuthCancel.Location = new System.Drawing.Point(246, 100);
            this.btnAuthCancel.Size = new System.Drawing.Size(136, 26);

            this.txtAuthorizationNo.Location = new System.Drawing.Point(246, 34);
            this.txtAuthorizationNo.Size = new System.Drawing.Size(136, 24);

            this.ClientSize = new System.Drawing.Size(483, 136);

            #endregion
        }

        //Added by Shitaljit on 3 May 2012
        public frmPOSPayAuthNo()
        {
            InitializeComponent();
            ClearFields();  //PRIMEPOS-2821 03-Nov-2020 JY Added
            this.lblAuthorization.Width = this.lblAuthorization.Width - 10;
            this.lblAuthorization.Text = "Enter Verification ID No:";
            this.Text = "OTC Item Authentication ";
            this.lblVerificationID.Visible = true;
            this.cmbVerificationID.Visible = true;
            this.lblOTCInfo.Visible = true;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
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
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance(); //PRIMEPOS-3166
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSPayAuthNo));
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            this.lblOTCInfo = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbCustAddress = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtZipCode = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.txtState = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblZip = new Infragistics.Win.Misc.UltraLabel();
            this.lblState = new Infragistics.Win.Misc.UltraLabel();
            this.lblCity = new Infragistics.Win.Misc.UltraLabel();
            this.lblAddress = new Infragistics.Win.Misc.UltraLabel();
            this.cmbVerificationID = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblVerificationID = new Infragistics.Win.Misc.UltraLabel();
            this.txtAuthorizationNo = new System.Windows.Forms.TextBox();
            this.lblAuthorization = new Infragistics.Win.Misc.UltraLabel();
            this.btnAuthCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnAuthOverride = new Infragistics.Win.Misc.UltraButton(); //PRIMEPOS-3166
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbCustAddress)).BeginInit();
            this.gbCustAddress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVerificationID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOTCInfo
            // 
            appearance20.FontData.Name = "Arial";
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.TextVAlignAsString = "Middle";
            this.lblOTCInfo.Appearance = appearance20;
            this.lblOTCInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOTCInfo.Location = new System.Drawing.Point(8, 6);
            this.lblOTCInfo.Name = "lblOTCInfo";
            this.lblOTCInfo.Size = new System.Drawing.Size(523, 37);
            this.lblOTCInfo.TabIndex = 21;
            this.lblOTCInfo.Text = "There are some item (s) in this transaction that require a form of ID. Please sel" +
    "ect an ID type and enter its ID#.";
            this.lblOTCInfo.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gbCustAddress);
            this.groupBox1.Controls.Add(this.cmbVerificationID);
            this.groupBox1.Controls.Add(this.lblVerificationID);
            this.groupBox1.Controls.Add(this.txtAuthorizationNo);
            this.groupBox1.Controls.Add(this.lblAuthorization);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(8, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(527, 190);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // gbCustAddress
            // 
            this.gbCustAddress.Controls.Add(this.txtZipCode);
            this.gbCustAddress.Controls.Add(this.txtState);
            this.gbCustAddress.Controls.Add(this.txtCity);
            this.gbCustAddress.Controls.Add(this.txtAddress);
            this.gbCustAddress.Controls.Add(this.lblZip);
            this.gbCustAddress.Controls.Add(this.lblState);
            this.gbCustAddress.Controls.Add(this.lblCity);
            this.gbCustAddress.Controls.Add(this.lblAddress);
            this.gbCustAddress.Location = new System.Drawing.Point(0, 115);
            this.gbCustAddress.Name = "gbCustAddress";
            this.gbCustAddress.Size = new System.Drawing.Size(527, 75);
            this.gbCustAddress.TabIndex = 22;
            // 
            // txtZipCode
            // 
            appearance22.FontData.SizeInPoints = 8F;
            this.txtZipCode.Appearance = appearance22;
            this.txtZipCode.AutoSize = false;
            this.txtZipCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtZipCode.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.UseSpecifiedMask;
            this.txtZipCode.InputMask = "99999";
            this.txtZipCode.Location = new System.Drawing.Point(432, 10);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.NonAutoSizeHeight = 24;
            this.txtZipCode.Size = new System.Drawing.Size(83, 20);
            this.txtZipCode.TabIndex = 4;
            this.txtZipCode.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtZipCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtState
            // 
            this.txtState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtState.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.txtState.Location = new System.Drawing.Point(337, 40);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(178, 21);
            this.txtState.TabIndex = 6;
            // 
            // txtCity
            // 
            this.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.txtCity.Location = new System.Drawing.Point(85, 40);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(178, 21);
            this.txtCity.TabIndex = 5;
            // 
            // txtAddress
            // 
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.txtAddress.Location = new System.Drawing.Point(85, 10);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(305, 21);
            this.txtAddress.TabIndex = 3;
            // 
            // lblZip
            // 
            appearance14.FontData.Name = "Arial";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.TextVAlignAsString = "Middle";
            this.lblZip.Appearance = appearance14;
            this.lblZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZip.Location = new System.Drawing.Point(395, 10);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(32, 20);
            this.lblZip.TabIndex = 9;
            this.lblZip.Text = "Zip";
            // 
            // lblState
            // 
            appearance23.FontData.Name = "Arial";
            appearance23.ForeColor = System.Drawing.Color.White;
            appearance23.TextVAlignAsString = "Middle";
            this.lblState.Appearance = appearance23;
            this.lblState.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblState.Location = new System.Drawing.Point(281, 40);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(52, 20);
            this.lblState.TabIndex = 8;
            this.lblState.Text = "State";
            // 
            // lblCity
            // 
            appearance24.FontData.Name = "Arial";
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.TextVAlignAsString = "Middle";
            this.lblCity.Appearance = appearance24;
            this.lblCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCity.Location = new System.Drawing.Point(10, 40);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(52, 20);
            this.lblCity.TabIndex = 7;
            this.lblCity.Text = "City";
            // 
            // lblAddress
            // 
            appearance25.FontData.Name = "Arial";
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.TextVAlignAsString = "Middle";
            this.lblAddress.Appearance = appearance25;
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(10, 10);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(70, 20);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "Address";
            // 
            // cmbVerificationID
            // 
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance7.BorderColor3DBase = System.Drawing.Color.Black;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.ItalicAsString = "False";
            appearance7.FontData.StrikeoutAsString = "False";
            appearance7.FontData.UnderlineAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbVerificationID.Appearance = appearance7;
            this.cmbVerificationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.cmbVerificationID.ButtonAppearance = appearance8;
            this.cmbVerificationID.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbVerificationID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem1.DataValue = "DL";
            valueListItem1.DisplayText = "Driver\'s License(US or Canada)";
            valueListItem2.DataValue = "UP";
            valueListItem2.DisplayText = "US Passport";
            valueListItem3.DataValue = "RC";
            valueListItem3.DisplayText = "Alien Registration or Permanent Resident Card";
            valueListItem4.DataValue = "FP";
            valueListItem4.DisplayText = "Unexpired Foreign Passport with temporary I-551 Stamp";
            valueListItem5.DataValue = "EA";
            valueListItem5.DisplayText = "Unexpired Employment Authorization Document";
            valueListItem6.DataValue = "SI";
            valueListItem6.DisplayText = "School ID with Picture";
            valueListItem7.DataValue = "VC";
            valueListItem7.DisplayText = "Voter\'s Registration Card";
            valueListItem8.DataValue = "MC";
            valueListItem8.DisplayText = "US Military Card";
            valueListItem9.DataValue = "TD";
            valueListItem9.DisplayText = "Native American Tribal Documents";
            valueListItem10.DataValue = "STATE_ID";
            valueListItem10.DisplayText = "Other state-issued ID";
            valueListItem11.DataValue = "ALIEN";
            valueListItem11.DisplayText = "Alien Registration Card";
            this.cmbVerificationID.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4,
            valueListItem5,
            valueListItem6,
            valueListItem7,
            valueListItem8,
            valueListItem9,
            valueListItem10,
            valueListItem11});
            this.cmbVerificationID.Location = new System.Drawing.Point(214, 21);
            this.cmbVerificationID.MaxLength = 20;
            this.cmbVerificationID.Name = "cmbVerificationID";
            this.cmbVerificationID.Size = new System.Drawing.Size(305, 21);
            this.cmbVerificationID.TabIndex = 0;
            this.cmbVerificationID.Visible = false;
            // 
            // lblVerificationID
            // 
            this.lblVerificationID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerificationID.Location = new System.Drawing.Point(9, 21);
            this.lblVerificationID.Name = "lblVerificationID";
            this.lblVerificationID.Size = new System.Drawing.Size(118, 25);
            this.lblVerificationID.TabIndex = 20;
            this.lblVerificationID.Text = "Verification ID";
            this.lblVerificationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblVerificationID.Visible = false;
            // 
            // txtAuthorizationNo
            // 
            this.txtAuthorizationNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuthorizationNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuthorizationNo.Location = new System.Drawing.Point(214, 58);
            this.txtAuthorizationNo.Multiline = true;
            this.txtAuthorizationNo.Name = "txtAuthorizationNo";
            this.txtAuthorizationNo.Size = new System.Drawing.Size(305, 20);
            this.txtAuthorizationNo.TabIndex = 1;
            this.txtAuthorizationNo.TextChanged += new System.EventHandler(this.txtAuthorizationNo_TextChanged);
            this.txtAuthorizationNo.Leave += new System.EventHandler(this.txtAuthorizationNo_Leave);
            // 
            // lblAuthorization
            // 
            appearance21.FontData.Name = "Arial";
            appearance21.ForeColor = System.Drawing.Color.White;
            appearance21.TextVAlignAsString = "Middle";
            this.lblAuthorization.Appearance = appearance21;
            this.lblAuthorization.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthorization.Location = new System.Drawing.Point(10, 56);
            this.lblAuthorization.Name = "lblAuthorization";
            this.lblAuthorization.Size = new System.Drawing.Size(201, 26);
            this.lblAuthorization.TabIndex = 5;
            this.lblAuthorization.Text = "Authorization No.";
            // 
            // btnAuthCancel
            // 
            appearance26.ForeColor = System.Drawing.Color.Black;
            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
            this.btnAuthCancel.Appearance = appearance26;
            this.btnAuthCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnAuthCancel.Enabled = false;
            this.btnAuthCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuthCancel.Location = new System.Drawing.Point(387, 246);
            this.btnAuthCancel.Name = "btnAuthCancel";
            this.btnAuthCancel.Size = new System.Drawing.Size(136, 26);
            this.btnAuthCancel.TabIndex = 7;
            this.btnAuthCancel.TabStop = false;
            this.btnAuthCancel.Text = "ESC to Cancel";
            // 
            // btnClose
            // 
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            this.btnClose.Appearance = appearance27;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.Location = new System.Drawing.Point(245, 246);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(136, 26);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Continue";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            #region PRIMEPOS-3166
            // 
            // btnAuthOverride
            // 
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.btnAuthOverride.Appearance = appearance12;
            this.btnAuthOverride.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnAuthOverride.Location = new System.Drawing.Point(103, 246);
            this.btnAuthOverride.Name = "btnAuthOverride";
            this.btnAuthOverride.Size = new System.Drawing.Size(136, 26);
            this.btnAuthOverride.TabIndex = 22;
            this.btnAuthOverride.Text = "Override";
            this.btnAuthOverride.Visible = false; //PRIMEPOS-3166N
            this.btnAuthOverride.Click += new System.EventHandler(this.btnAuthOverride_Click);
            #endregion
            // 
            // frmPOSPayAuthNo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(547, 281);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAuthOverride); //PRIMEPOS-3166
            this.Controls.Add(this.lblOTCInfo);
            this.Controls.Add(this.btnAuthCancel);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSPayAuthNo";
            this.ShowInTaskbar = false;
            this.Text = "Authorization Information";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPOSChangeDue_Load);
            this.InputLanguageChanged += new System.Windows.Forms.InputLanguageChangedEventHandler(this.frmPOSPayAuthNo_InputLanguageChanged);
            this.Shown += new System.EventHandler(this.frmPOSPayAuthNo_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOSPayAuthNo_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPOSPayAuthNo_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbCustAddress)).EndInit();
            this.gbCustAddress.ResumeLayout(false);
            this.gbCustAddress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVerificationID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private void frmPOSChangeDue_Load(object sender, System.EventArgs e)
        {
            this.Left = 100;		//( frmMain.getInstance().Width-this.Width)/2;
            this.Top = 100;			//(frmMain.getInstance().Height-this.Height)/2;
            clsUIHelper.setColorSchecme(this);

            #region PRIMEPOS-2693 13-Jun-2019 JY Added
            this.cmbVerificationID.Enter -= new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbVerificationID.Leave -= new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtAuthorizationNo.Enter -= new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtAuthorizationNo.Leave -= new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cmbVerificationID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbVerificationID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtAuthorizationNo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtAuthorizationNo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            #endregion

            this.txtAuthorizationNo.KeyDown -= txtAuthorizationNo_KeyDown;
            this.cmbVerificationID.SelectionChanged -= cmbVerificationID_SelectionChanged;

            this.txtAuthorizationNo.KeyDown += txtAuthorizationNo_KeyDown;//Added By Manoj 9/12/2013
            this.cmbVerificationID.SelectionChanged += cmbVerificationID_SelectionChanged;
            //Added by shitaljit 0n 16v May 2012
            isCancelled = false;
            this.cmbVerificationID.SelectedIndex = 0;
            this.txtAuthorizationNo.Text = "";
            this.data = string.Empty;
            this.Dob = string.Empty;
            this.btnClose.Enabled = false;
            //Added by Manoj 9/12/2013
            ScanData -= frmPOSPayAuthNo_ScanData;
            ScanData += frmPOSPayAuthNo_ScanData;
            ScanData.Invoke(); //Added by Manoj 9/12/2013
            DisposeDobControls(); //Added by Manoj 9/16/2013

            #region PRIMEPOS-2821 03-Nov-2020 JY Added
            try
            {
                if (bCalledForNplex)
                {
                    if (CustomerId > 0)
                    {
                        POS_Core.BusinessRules.Customer oCustomer = new POS_Core.BusinessRules.Customer();
                        CustomerData oCustData = oCustomer.GetCustomerByID(CustomerId);
                        if (oCustData != null && oCustData.Tables.Count > 0 && oCustData.Customer.Rows.Count > 0)
                        {
                            CustomerRow oCustomerRow = (CustomerRow)oCustData.Customer.Rows[0];
                            oCustAddress = oCustomerRow;

                            LastName = Configuration.convertNullToString(oCustomerRow.CustomerName).Trim();
                            FirstName = Configuration.convertNullToString(oCustomerRow.FirstName).Trim();
                            txtAddress.Text = Configuration.convertNullToString(oCustomerRow.Address1).Trim() + " " + Configuration.convertNullToString(oCustomerRow.Address2).Trim();
                            txtZipCode.Text = Configuration.convertNullToString(oCustomerRow.Zip).Trim();
                            txtCity.Text = Configuration.convertNullToString(oCustomerRow.City).Trim();
                            txtState.Text = Configuration.convertNullToString(oCustomerRow.State).Trim();
                            oCustAddress.DateOfBirth = Convert.ToDateTime(oCustAddress.DateOfBirth).ToShortDateString();
                        }
                    }
                }
                else
                {
                    this.groupBox1.Height = 115;
                    this.btnClose.Top = this.btnAuthCancel.Top = this.btnAuthOverride.Top = 170;  //PRIMEPOS-3166
                    if (showOverrideBtn) //PRIMEPOS-3166N
                    {
                        this.btnAuthOverride.Visible = true;
                    }
                    else
                    {
                        this.btnAuthOverride.Visible = false; //PRIMEPOS-3166N
                    }
                    this.ClientSize = new System.Drawing.Size(547, 210);
                }
            }
            catch (Exception Ex) { }
            #endregion
        }

        #region PRIMEPOS-2729 06-Sep-2019 JY Added to reset public variables 
        private void ClearFields()
        {
            ID = null;
            isCancelled = false;
            CustomerId = 0;
            Configuration.AuthorizationNo = "";
            Configuration.strDOB = "";
            oCustAddress = null;
        }
        #endregion

        /// <summary>
        /// Create Dob date picker and Label controls
        /// </summary>      
        private void CreateDobControls()    //PRIMEPOS-2693 13-Jun-2019 JY Added
        {
            if (dtpDob == null)
            {
                dtpDob = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
                dtpDob.Name = "dtpDob";
                dtpDob.TabStop = true;
                dtpDob.TabIndex = 2;
                dtpDob.Location = new System.Drawing.Point(214, 85);
                dtpDob.Size = new System.Drawing.Size(132, 24);
                dtpDob.Visible = true;
                dtpDob.Enabled = true;
                dtpDob.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Date;
                dtpDob.Text = "//";
                dtpDob.ValueChanged += dtpDob_ValueChanged;
                dtpDob.KeyDown += new KeyEventHandler(dtpDob_KeyDown);
                dtpDob.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                dtpDob.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            }
            if (lblDob == null)
            {
                lblDob = new Label();
                lblDob.Location = new System.Drawing.Point(10, 85);
                lblDob.Size = new System.Drawing.Size(201, 26);
                lblDob.Text = "DOB (MM/DD/YYYY)";
            }

            if (!this.groupBox1.Controls.Contains(this.lblDob))
                this.groupBox1.Controls.Add(this.lblDob);
            if (!this.groupBox1.Controls.Contains(this.dtpDob))
                this.groupBox1.Controls.Add(this.dtpDob);

            btnClose.Enabled = false;
        }

        #region PRIMEPOS-2693 13-Jun-2019 JY Commented
        //private void CreateDobControls()
        //{
        //    if(dtpDob == null)
        //    {
        //        dtpDob = new DateTimePicker();
        //    }
        //    if(lblDob == null)
        //    {
        //        lblDob = new Label();
        //    }
        //    dtpDob.TabStop = true;
        //    dtpDob.Location = new System.Drawing.Point(214, 85);
        //    dtpDob.Size = new System.Drawing.Size(132, 24);
        //    dtpDob.Visible = true;
        //    dtpDob.Enabled = true;
        //    dtpDob.Format = DateTimePickerFormat.Custom;
        //    dtpDob.CustomFormat = "MM/dd/yyyy";
        //    dtpDob.MinDate = Convert.ToDateTime("01/01/1900");
        //    //DateTime maxD = new DateTime(DateTime.Now.Year - 13, DateTime.Now.Month, DateTime.Now.Day); //if you want to dob age limit
        //    dtpDob.MaxDate = DateTime.Now;

        //    lblDob.Location = new System.Drawing.Point(10, 85);
        //    lblDob.Size = new System.Drawing.Size(201, 26);
        //    lblDob.Text = "Date Of Birth (MMDDYYYY)";

        //    this.groupBox1.Controls.Add(lblDob);
        //    this.groupBox1.Controls.Add(dtpDob);

        //    dtpDob.ValueChanged += dtpDob_ValueChanged;
        //    dtpDob.KeyDown += new KeyEventHandler(dtpDob_KeyDown);
        //    btnClose.Enabled = false;
        //}
        #endregion

        void dtpDob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnClose.Focus();
            }
        }


        /// <summary>
        /// Dispose of the Dob Controls
        /// </summary>
        private void DisposeDobControls()
        {
            if(dtpDob != null)
            {
                dtpDob.ValueChanged -= dtpDob_ValueChanged;
                dtpDob.KeyDown -= new KeyEventHandler(dtpDob_KeyDown);
                dtpDob.Enter -= new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                dtpDob.Leave -= new System.EventHandler(clsUIHelper.AfterExitEditMode);
                dtpDob.Visible = false;
                dtpDob = null;
            }
            if(lblDob != null)
            {
                lblDob.Visible = false;
                lblDob = null;
            }
            txtAuthorizationNo.PasswordChar = '\0';
            wasScan = false;
        }

        private void dtpDob_ValueChanged(object sender, EventArgs e)
        {
            btnClose.Enabled = true;
        }

        //Added by Manoj 9/15/2013
        private void cmbVerificationID_SelectionChanged(object sender, EventArgs e)
        {
            if (cmbVerificationID.SelectedIndex != 0 && isAgeLimit)
            {
                CreateDobControls();
            }
            else
            {
                txtAuthorizationNo.Text = string.Empty;
                DisposeDobControls();
                btnClose.Enabled = true;
            }
        }

        //Added by Manoj 9/12/2013
        private void txtAuthorizationNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && txtAuthorizationNo.Focused)
            {
                Validate();
                SetDOB();//Added  By Manoj 9/20/2013
                btnClose.Focus();
            }
        }

        /// <summary>
        /// Manoj 9/12/2013
        /// Text box Events
        /// </summary>
        private void frmPOSPayAuthNo_ScanData()
        {
            txtAuthorizationNo.TextChanged -= txtAuthorizationNo_TextChanged;
            txtAuthorizationNo.LostFocus -= txtAuthorizationNo_LostFocus;
            txtAuthorizationNo.Leave -= txtAuthorizationNo_Leave;   //PRIMEPOS-3102 24-Jun-2022 JY Added
            txtAuthorizationNo.TextChanged += txtAuthorizationNo_TextChanged;
            txtAuthorizationNo.LostFocus += txtAuthorizationNo_LostFocus;
            txtAuthorizationNo.Leave += txtAuthorizationNo_Leave;   //PRIMEPOS-3102 24-Jun-2022 JY Added
        }

        /// <summary>
        /// Manoj 9/12/2013
        /// After scan focus is set to a different control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAuthorizationNo_LostFocus(object sender, EventArgs e)
        {
            if(isScan && !string.IsNullOrEmpty(data))
            {
                logger.Trace("txtAuthorizationNo_LostFocus(object sender, EventArgs e) - " + data); //PRIMEPOS-3162 18-Nov-2022 JY Added
                if (ID != null)
                {
                    ID = null;
                }
                ID = new POS_Core.Business_Tier.DL(data);
                isScan = false;
                if (!string.IsNullOrEmpty(ID.DAQ))
                {
                    AfterScanData -= frmPOSPayAuthNo_AfterScanData;
                    AfterScanData += frmPOSPayAuthNo_AfterScanData;
                    AfterScanData.Invoke();
                }
            }
        }

        /// <summary>
        /// Manoj 9/12/2013
        /// After the Scan Event
        /// </summary>
        private void frmPOSPayAuthNo_AfterScanData()
        {
            if(!string.IsNullOrEmpty(ID.DAQ))
            {
                txtAuthorizationNo.Text = string.Empty;
                txtAuthorizationNo.Multiline = false;
                txtAuthorizationNo.PasswordChar = '\0';

                if(cmbVerificationID.SelectedIndex == 0)
                {
                    txtAuthorizationNo.Text = ID.DAQ.Trim(); // Lic#
                    Application.DoEvents();
                    Dob = ID.DBB; //DOb
                }

                #region PRIMEPOS-2821 03-Nov-2020 JY Added
                try
                {
                    LastName = Configuration.convertNullToString(ID.DCS);    //DCS	Family Name
                    FirstName = Configuration.convertNullToString(ID.DCT);  //DCT	Given Name

                    String dateString = Configuration.convertNullToString(ID.DBB); //DBB	Date of Birth
                    logger.Trace("frmPOSPayAuthNo_AfterScanData() - " + dateString); //PRIMEPOS-3162 18-Nov-2022 JY Added
                    String format = "MMddyyyy";
                    DateTime result = DateTime.Now;
                    bool bError = false;
                    try
                    {
                        result = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception Ex)
                    {
                        bError = true;
                        logger.Fatal(Ex, "POSDrugClassCapture_AfterScanData() - 1");    //PRIMEPOS-3162 18-Nov-2022 JY Added
                    }
                    if (!bError)
                    {
                        dtpDob.Text = result.ToShortDateString();
                        //if (dtpDob.Visible == true && dtpDob.Enabled == true)   dtpDob.Focus();
                    }
                    else
                    {
                        try
                        {
                            dtpDob.Text = ID.DBB.Substring(0, 2) + "/" + ID.DBB.Substring(2, 2) + "/" + ID.DBB.Substring(4, 4); //DOB
                            //if (dtpDob.Visible == true && dtpDob.Enabled == true) dtpDob.Focus();
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "POSDrugClassCapture_AfterScanData() - 2");    //PRIMEPOS-3162 18-Nov-2022 JY Added
                        }
                    }

                    txtAddress.Text = Configuration.convertNullToString(ID.DAG);    //DAG	Street Address 1
                    txtZipCode.Text = Configuration.convertNullToString(ID.DAK);   //DAK	Address Postal Code
                    txtState.Text = Configuration.convertNullToString(ID.DAJ);  //DAJ	Address – Jurisdiction Code    //required for PO
                    txtCity.Text = Configuration.convertNullToString(ID.DAI);    //DAI	Address City    //not mandatory
                }
                catch { }
                #endregion
            }
        }

        //Added  By Manoj 9/20/2013
        private void SetDOB()
        {
            if(!wasScan)
            {
                if(dtpDob != null && !string.IsNullOrEmpty(dtpDob.Text))
                {
                    string cDob = dtpDob.Text;
                    if(cDob.Contains("/"))
                    {
                        Dob = cDob.Replace("/", "").Trim();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            if(this.txtAuthorizationNo.Text.Trim() != "" || oTransType == POSTransactionType.SalesReturn)
            {
                Configuration.AuthorizationNo = this.txtAuthorizationNo.Text.Trim();    //12-Sep-2016 JY Added to preserve ID 
                SetDOB();//Added  By Manoj 9/20/2013
                ValidateID();
                this.Close();

                #region PRIMEPOS-2821 03-Nov-2020 JY Added
                if (bCalledForNplex)
                {
                    oCustAddress.CustomerName = LastName;
                    oCustAddress.FirstName = FirstName;
                    oCustAddress.DriveLicNo = txtAuthorizationNo.Text.ToString();
                    oCustAddress.Address1 = txtAddress.Text.ToString();
                    oCustAddress.Zip = txtZipCode.Text.ToString();
                    oCustAddress.City = txtCity.Text.ToString();
                    oCustAddress.State = txtState.Text.ToString();

                    if (dtpDob != null && !string.IsNullOrEmpty(dtpDob.Text))
                    {
                        string cDob = dtpDob.Text;
                        if (cDob.Contains("/"))
                        {
                            Dob = cDob.Replace("/", "").Trim();
                        }
                    }
                }
                #endregion
            }
            else
            {
                this.txtAuthorizationNo.Focus();
            }
        }

        private void frmPOSPayAuthNo_InputLanguageChanged(object sender, System.Windows.Forms.InputLanguageChangedEventArgs e)
        {
        }

        /// <summary>
        /// Author:Shitaljit Create Date: 10/10/2013
        /// Added To Validate drivers license# against the scan/enter#
        /// </summary>
        /// <returns></returns>
        private bool ValidateID()
        {
            logger.Trace("ValidateID() - " + clsPOSDBConstants.Log_Entering);
            bool RetVal = true;
            try
            {
                if (CustomerId > 0 && cmbVerificationID.SelectedIndex == 0)
                {
                    POS_Core.BusinessRules.Customer oCust = new POS_Core.BusinessRules.Customer();
                    CustomerData oCustData = oCust.GetCustomerByID(CustomerId);
                    if (Configuration.isNullOrEmptyDataSet(oCustData) == false)
                    {
                        CustomerRow oCustRow = (CustomerRow)oCustData.Customer.Rows[0];
                        if (string.IsNullOrEmpty(oCustRow.DriveLicNo) == false && oCustRow.DriveLicNo.ToUpper().Equals(this.txtAuthorizationNo.Text.ToUpper().Trim()) == false)
                        {
                            clsUIHelper.ShowWarningMsg("Enter driver license# does not matched with driver license# stored in customer profile.", "Driver license# Not Matched");
                            RetVal = false;
                        }
                    }
                }
                logger.Trace("ValidateID() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ValidateID()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            return RetVal;
        }
        private void frmPOSPayAuthNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt == true || e.Control == true)
            {
                e.Handled = true;
            }
            //Added By Shitaljit to close the form if some data is there in the ID# and hiting ENTER.
            if(this.cmbVerificationID.Visible == true && e.KeyData == Keys.Enter && this.txtAuthorizationNo.Text.Trim() != "" && !isScan)
            {
                ValidateID();
                if (dtpDob != null && dtpDob.Visible == true && dtpDob.Text.Trim() != "//")
                {
                    SetDOB();
                    //this.Close(); //PRIMEPOS-2693 13-Jun-2019 JY Commented
                }
            }
            //PRIMEPOS-2693 13-Jun-2019 JY Added
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                if (this.ActiveControl.Name == "dtpDob" && dtpDob.Visible == true && dtpDob.Enabled == true)
                    dtpDob.Focus();
            }
        }

        //Added this function to ensure the AUTNO dialog is getting closed.
        private void frmPOSPayAuthNo_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Escape && !isScan)
            {
                //Added by shitaljit on 16 may 2012
                this.txtAuthorizationNo.Text = "";
                isCancelled = true;
                this.Close();
            }
        }

        //Added by Manoj 7/16/2013 - for scanning the driver Lic#
        private void txtAuthorizationNo_TextChanged(object sender, EventArgs e)
        {
            data = txtAuthorizationNo.Text;
            if(cmbVerificationID.SelectedIndex == 0)
            {
                if(data.StartsWith("@") && !isScan)
                {
                    isScan = true;
                    txtAuthorizationNo.Multiline = true;
                    txtAuthorizationNo.PasswordChar = 'X';
                    //try
                    //{
                    //    txtAuthorizationNo_LostFocus(sender, e);
                    //}
                    //catch { }
                }
                else if(!string.IsNullOrEmpty(data) && !data.StartsWith("@") && (data.Length == 1 || dtpDob == null) && !isScan && !wasScan && isAgeLimit)
                {
                    CreateDobControls();
                }
                else if(string.IsNullOrEmpty(txtAuthorizationNo.Text) && !isScan && isAgeLimit)
                {
                    DisposeDobControls();
                }
                else
                {
                    btnClose.Enabled = true;
                }
            }
            else if(cmbVerificationID.SelectedIndex != 0 && !string.IsNullOrEmpty(data) && data.StartsWith("@"))
            {
                if(!wasScan)
                {
                    wasScan = true;
                    txtAuthorizationNo.Multiline = true;
                    txtAuthorizationNo.PasswordChar = 'X';
                    data = string.Empty;
                    btnClose.Enabled = false;
                }
            }
        }

        #region PRIMEPOS-2693 13-Jun-2019 JY Added
        private void frmPOSPayAuthNo_Shown(object sender, EventArgs e)
        {
            this.txtAuthorizationNo.Focus();
            this.txtAuthorizationNo.Select();
        }
        #endregion

        #region PRIMEPOS-3102 24-Jun-2022 JY Added
        private void txtAuthorizationNo_Leave(object sender, EventArgs e)
        {
            Application.DoEvents();
            txtAuthorizationNo.Text = txtAuthorizationNo.Text.Trim();
        }
        #endregion
        #region PRIMEPOS-3166
        private void btnAuthOverride_Click(object sender, EventArgs e) 
        {
            string UserId = string.Empty;//PRIMEPOS-2808
            if (UserPriviliges.getPermission(UserPriviliges.Modules.AppSettings.ID, UserPriviliges.Screens.OverrideMonitorItem.ID, 0, UserPriviliges.Screens.OverrideMonitorItem.Name, out UserId))
            {
                monitorItemOverriddenBy = UserId;
                isOverrideMonitorItem = true;
                txtAuthorizationNo.Text = "OVERRIDDEN";
                this.Close();
            }
        }
        #endregion
    }
}