//using POS_Core.DataAccess;
using NLog;
using POS_Core.CommonData;
using POS_Core.Resources;
using POS_Core.Resources.PaymentHandler;
using POS_Core.TransType;
using System;
using System.Threading;
using System.Windows.Forms;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPOSChangeDue.
    /// </summary>
    public class frmPOSOnlineCC : System.Windows.Forms.Form
    {
        public delegate void CCResponse(string input);
        public event CCResponse eCCResponse;
        private string TrackNoII = "";
        public bool isCanceled = false;
        public String AuthNo;
        public bool AllowCancel = true;
        public System.Decimal Amount;
        //Added By SRT(Gaurav) Date: 19-NOV-2008
        //Mantis ID: 0000136
        public System.Decimal FSAAmount = 0.00M;
        public bool isFSATransaction = false;
        public PccCardInfo ApprovedPCCCardInfo = null;
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Ritesh Parekh) Date: 18-Aug-2009
        public System.Decimal FSARxAmount = 0.00M;
        //End Of Added By SRT(Ritesh Parekh)
        private POSTransactionType oTransType;
        private string strSignature = "";
        private string sigType = "";
        //private byte[] binSignature=null;
        private byte[] binSignature;
        private Thread oThread;
        private CbDbf.CCbDbf oCCDbf;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraLabel lblCreditCard;
        private Infragistics.Win.Misc.UltraLabel lblExpiryDate;
        private Infragistics.Win.Misc.UltraButton btnContinue;
        private Infragistics.Win.Misc.UltraLabel lblAmount;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAmount;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtExpDate;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCCNo;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
        private Infragistics.Win.Misc.UltraButton btnProcessManual;
        private Infragistics.Win.Misc.UltraButton btnProcessCancel;
        private bool isCancelTransaction = false;
        public string isManualProcess = "";    //Sprint-19 - 2139 06-Jan-2015 JY added
        public string oPayTpes = string.Empty; //Added By SRT
        private PccPaymentSvr objPccPmt = null; //Added By SRT
        private string pmtProcessMethod = string.Empty; //Modified By Dharmendra On Nov-14-08 Reading Payment Processor from POSSET instead of App.config
        private string TrackNoIII = ""; //Added By SRT
        private const string DBF = "DBF";
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtZipCode;
        private Infragistics.Win.Misc.UltraLabel lblZipCode;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtAddress;
        private Infragistics.Win.Misc.UltraLabel lblAddress;
        public String SubCardType = null;
        public String sCardNumber = string.Empty;
        //Added By SRT(Ritesh Parekh)
        //This variable will hold old processor name and reassign at end of flow.
        public String OldProcessor = string.Empty;
        //End Of Added By SRT(Ritesh Parekh)
        //End Of Added By SRT(Gaurav)
        //Added By SRT(Ritesh Parekh) Date: 23-Jul-2009

        private int sStartIndex = 0;
        private int sMaxLength = 12;
        private bool bISFourceTrans = false;
        private int count = 0; // Added by Manoj 8/23/2011
        private PccCardInfo objCustomerCardInfo;
        public bool isF10Press = false; // added by Manoj 8/24/2011
        private string cardnum, cardexp; // added by Manoj 8/24/2011
        private string On = "ON", Off = "OFF";
        private string DeviceName = "Verifone MX870 with PinPad";
        private string MXDevice = "VerifoneMX925WithPinPad";
        private string PinPadName = "Verifone 1001/1000";
        private string BlueToothICMP = "Ingenico_ICMP";//PRIMEPOS-2503 Jenny Added
        private string ISC480 = "WPIngenico_ISC480";
        private string PAX_DEVICE = "HPSPAX"; // Suraj HPASPAX Changes
        private string Evertec = "EVERTEC"; // ARVIND - PRIMEPOS-2664
        private string CardSwiperCCNo = string.Empty;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public String ticketNum = null;// NileshJ - 20-Dec-2018
        private string VANTIV = "VANTIV"; // Arvind - 11 July 2019 PRIMEPOS-2636 
        //ARVIND PRIMEPOS-2738 
        public String originalTransID = string.Empty;
        public String hrefNumber = string.Empty;
        //
        public bool isAllowDuplicate = false;//PRIMEPOS-2784

        #region PRIMEPOS-2915
        public bool IsEmail = false;
        public bool IsPhone = false;
        public string CustomerName = "";
        public string Email = "";
        public string Phone = "";
        public string DOB = "";
        public bool IsPrimeRxPayLinkSend = false;//PRIMEPOS-3248
        public bool IsCustomerDriven = false;
        public string PrimeRxPayTransID = string.Empty;
        #endregion

        public String PaymentProcessor
        {
            get
            {
                if (pmtProcessMethod != null && pmtProcessMethod.Trim().Length > 0)
                {
                    return (pmtProcessMethod);
                }
                else
                {
                    return (clsPOSDBConstants.NOPROCESSOR);
                }
            }
        }

        public bool WasProcessorChanged = false;
        private Infragistics.Win.Misc.UltraLabel lblCVV;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtCVVCode;

        //End Of Added By SRT(Ritesh Parekh)
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmPOSOnlineCC(POSTransactionType oType, string paymentType, string PaymentProcessor)//Added additional parameter for paymentType by Dharmendra(SRT)
        {
            objPccPmt = new PccPaymentSvr(PaymentProcessor);
            this.oTransType = oType;
            this.oPayTpes = paymentType; // Added by Dharmendra(SRT)
            pmtProcessMethod = PaymentProcessor;
            if (pmtProcessMethod == string.Empty || pmtProcessMethod == null)
                pmtProcessMethod = DBF;
            InitializeComponent();
            this.btnProcessCancel.Enabled = false;
            OldProcessor = PaymentProcessor;
        }

        public PccCardInfo CustomerCardInfo
        {
            private get { return this.objCustomerCardInfo; }
            set { this.objCustomerCardInfo = value; }
        }
        private void CheckFSAPaymentForPRIMERXPAY()
        {
            if (pmtProcessMethod.ToUpper() == clsPOSDBConstants.PRIMERXPAY)
            {
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {
                    pmtProcessMethod = clsPOSDBConstants.PRIMERXPAY;
                    //Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.VANTIV;//PRIMEPOS-3057 Commented
                    this.isFSATransaction = true;

                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSProcessCC));
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAddress = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblAddress = new Infragistics.Win.Misc.UltraLabel();
            this.txtZipCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblZipCode = new Infragistics.Win.Misc.UltraLabel();
            this.btnProcessCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnProcessManual = new Infragistics.Win.Misc.UltraButton();
            this.txtCCNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtExpDate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAmount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblAmount = new Infragistics.Win.Misc.UltraLabel();
            this.lblCreditCard = new Infragistics.Win.Misc.UltraLabel();
            this.lblExpiryDate = new Infragistics.Win.Misc.UltraLabel();
            this.btnContinue = new Infragistics.Win.Misc.UltraButton();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.lblCVV = new Infragistics.Win.Misc.UltraLabel();
            this.txtCVVCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtZipCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCCNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCVVCode)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel3.TabIndex = 0;
            // 
            // lblTransactionType
            // 
            appearance1.BackColor = System.Drawing.Color.LightCyan;
            appearance1.BackColor2 = System.Drawing.Color.DodgerBlue;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.ForeColor = System.Drawing.Color.Blue;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(480, 43);
            this.lblTransactionType.TabIndex = 66;
            this.lblTransactionType.Tag = "NOCOLOR";
            this.lblTransactionType.Text = "Process Credit Card";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCVVCode);
            this.groupBox2.Controls.Add(this.lblCVV);
            this.groupBox2.Controls.Add(this.txtAddress);
            this.groupBox2.Controls.Add(this.lblAddress);
            this.groupBox2.Controls.Add(this.txtZipCode);
            this.groupBox2.Controls.Add(this.lblZipCode);
            this.groupBox2.Controls.Add(this.btnProcessCancel);
            this.groupBox2.Controls.Add(this.btnProcessManual);
            this.groupBox2.Controls.Add(this.txtCCNo);
            this.groupBox2.Controls.Add(this.txtExpDate);
            this.groupBox2.Controls.Add(this.txtAmount);
            this.groupBox2.Controls.Add(this.lblAmount);
            this.groupBox2.Controls.Add(this.lblCreditCard);
            this.groupBox2.Controls.Add(this.lblExpiryDate);
            this.groupBox2.Controls.Add(this.btnContinue);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(12, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(453, 302);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            // 
            // txtAddress
            // 
            appearance4.BackColorDisabled = System.Drawing.Color.White;
            appearance4.BackColorDisabled2 = System.Drawing.Color.White;
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtAddress.Appearance = appearance4;
            this.txtAddress.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtAddress.Location = new System.Drawing.Point(149, 178);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(220, 24);
            this.txtAddress.TabIndex = 4;
            // 
            // lblAddress
            // 
            appearance5.FontData.Name = "Arial";
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.TextVAlignAsString = "Middle";
            this.lblAddress.Appearance = appearance5;
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(19, 181);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(89, 19);
            this.lblAddress.TabIndex = 15;
            this.lblAddress.Text = "Address";
            // 
            // txtZipCode
            // 
            appearance6.BackColorDisabled = System.Drawing.Color.White;
            appearance6.BackColorDisabled2 = System.Drawing.Color.White;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtZipCode.Appearance = appearance6;
            this.txtZipCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtZipCode.Location = new System.Drawing.Point(149, 217);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(220, 24);
            this.txtZipCode.TabIndex = 5;
            // 
            // lblZipCode
            // 
            appearance7.FontData.Name = "Arial";
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.TextVAlignAsString = "Middle";
            this.lblZipCode.Appearance = appearance7;
            this.lblZipCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZipCode.Location = new System.Drawing.Point(19, 220);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(89, 19);
            this.lblZipCode.TabIndex = 13;
            this.lblZipCode.Text = "Zip Code";
            // 
            // btnProcessCancel
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnProcessCancel.Appearance = appearance8;
            this.btnProcessCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnProcessCancel.Enabled = false;
            this.btnProcessCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessCancel.Location = new System.Drawing.Point(301, 263);
            this.btnProcessCancel.Name = "btnProcessCancel";
            this.btnProcessCancel.Size = new System.Drawing.Size(136, 26);
            this.btnProcessCancel.TabIndex = 9;
            this.btnProcessCancel.TabStop = false;
            this.btnProcessCancel.Text = "ESC to Cancel";
            this.btnProcessCancel.Click += new System.EventHandler(this.btnProcessCancel_Click);
            // 
            // btnProcessManual
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            this.btnProcessManual.Appearance = appearance9;
            this.btnProcessManual.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnProcessManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessManual.Location = new System.Drawing.Point(161, 263);
            this.btnProcessManual.Name = "btnProcessManual";
            this.btnProcessManual.Size = new System.Drawing.Size(136, 26);
            this.btnProcessManual.TabIndex = 8;
            this.btnProcessManual.Text = "Process &Manual";
            this.btnProcessManual.Click += new System.EventHandler(this.btnProcessManual_Click);
            // 
            // txtCCNo
            // 
            appearance10.BackColorDisabled = System.Drawing.Color.White;
            appearance10.BackColorDisabled2 = System.Drawing.Color.White;
            appearance10.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtCCNo.Appearance = appearance10;
            this.txtCCNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCCNo.Location = new System.Drawing.Point(149, 61);
            this.txtCCNo.Name = "txtCCNo";
            this.txtCCNo.PasswordChar = 'X';
            this.txtCCNo.Size = new System.Drawing.Size(220, 24);
            this.txtCCNo.TabIndex = 1;
            this.txtCCNo.ValueChanged += new System.EventHandler(this.txtCCNo_ValueChanged);
            this.txtCCNo.TextChanged += new System.EventHandler(this.txtCCNo_TextChanged);
            this.txtCCNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCCNo_KeyPress);
            this.txtCCNo.Leave += new System.EventHandler(this.txtCCNo_Leave);
            this.txtCCNo.Validating += new System.ComponentModel.CancelEventHandler(this.txtCCNo_Validating);
            // 
            // txtExpDate
            // 
            appearance11.BackColorDisabled = System.Drawing.Color.White;
            appearance11.BackColorDisabled2 = System.Drawing.Color.White;
            appearance11.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtExpDate.Appearance = appearance11;
            this.txtExpDate.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtExpDate.Location = new System.Drawing.Point(149, 100);
            this.txtExpDate.Name = "txtExpDate";
            this.txtExpDate.Size = new System.Drawing.Size(220, 24);
            this.txtExpDate.TabIndex = 2;
            this.txtExpDate.Leave += new System.EventHandler(this.txtExpDate_Leave);
            // 
            // txtAmount
            // 
            appearance12.BackColorDisabled = System.Drawing.Color.White;
            appearance12.BackColorDisabled2 = System.Drawing.Color.White;
            appearance12.FontData.BoldAsString = "True";
            appearance12.ForeColorDisabled = System.Drawing.Color.Black;
            appearance12.TextHAlignAsString = "Right";
            this.txtAmount.Appearance = appearance12;
            this.txtAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtAmount.Enabled = false;
            this.txtAmount.Location = new System.Drawing.Point(149, 22);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(220, 24);
            this.txtAmount.TabIndex = 0;
            this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
            this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAmount_KeyPress);
            // 
            // lblAmount
            // 
            appearance13.FontData.Name = "Arial";
            appearance13.ForeColor = System.Drawing.Color.Black;
            appearance13.TextVAlignAsString = "Middle";
            this.lblAmount.Appearance = appearance13;
            this.lblAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.Location = new System.Drawing.Point(19, 25);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(62, 19);
            this.lblAmount.TabIndex = 10;
            this.lblAmount.Text = "Amount";
            // 
            // lblCreditCard
            // 
            appearance14.FontData.Name = "Arial";
            appearance14.ForeColor = System.Drawing.Color.Black;
            appearance14.TextVAlignAsString = "Middle";
            this.lblCreditCard.Appearance = appearance14;
            this.lblCreditCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreditCard.Location = new System.Drawing.Point(19, 64);
            this.lblCreditCard.Name = "lblCreditCard";
            this.lblCreditCard.Size = new System.Drawing.Size(116, 19);
            this.lblCreditCard.TabIndex = 7;
            this.lblCreditCard.Text = "Credit Card No.";
            // 
            // lblExpiryDate
            // 
            appearance15.FontData.Name = "Arial";
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextVAlignAsString = "Middle";
            this.lblExpiryDate.Appearance = appearance15;
            this.lblExpiryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpiryDate.Location = new System.Drawing.Point(19, 103);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(89, 19);
            this.lblExpiryDate.TabIndex = 6;
            this.lblExpiryDate.Text = "Expiry Date";
            // 
            // btnContinue
            // 
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            this.btnContinue.Appearance = appearance16;
            this.btnContinue.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.Location = new System.Drawing.Point(19, 263);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(136, 26);
            this.btnContinue.TabIndex = 7;
            this.btnContinue.Text = "Process &Online";
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // sbMain
            // 
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BackColor2 = System.Drawing.SystemColors.Control;
            appearance17.BorderColor = System.Drawing.Color.Black;
            appearance17.FontData.Name = "Verdana";
            appearance17.FontData.SizeInPoints = 10F;
            appearance17.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance17;
            this.sbMain.Location = new System.Drawing.Point(0, 369);
            this.sbMain.Name = "sbMain";
            appearance18.BorderColor = System.Drawing.Color.Black;
            appearance18.BorderColor3DBase = System.Drawing.Color.Black;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance19;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Text = "Start Credit Card Process";
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(480, 25);
            this.sbMain.TabIndex = 67;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // lblCVV
            // 
            appearance3.FontData.Name = "Arial";
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextVAlignAsString = "Middle";
            this.lblCVV.Appearance = appearance3;
            this.lblCVV.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCVV.Location = new System.Drawing.Point(19, 142);
            this.lblCVV.Name = "lblCVV";
            this.lblCVV.Size = new System.Drawing.Size(89, 19);
            this.lblCVV.TabIndex = 17;
            this.lblCVV.Text = "CVV Code";
            // 
            // txtCVVCode
            // 
            appearance2.BackColorDisabled = System.Drawing.Color.White;
            appearance2.BackColorDisabled2 = System.Drawing.Color.White;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtCVVCode.Appearance = appearance2;
            this.txtCVVCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCVVCode.Location = new System.Drawing.Point(149, 139);
            this.txtCVVCode.MaxLength = 3;
            this.txtCVVCode.Name = "txtCVVCode";
            this.txtCVVCode.PasswordChar = 'X';
            this.txtCVVCode.Size = new System.Drawing.Size(220, 24);
            this.txtCVVCode.TabIndex = 3;
            this.txtCVVCode.ValueChanged += new System.EventHandler(this.txtCVVCode_ValueChanged);
            this.txtCVVCode.TextChanged += new System.EventHandler(this.txtCVVCode_TextChanged);
            this.txtCVVCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCVVCode_KeyPress);
            this.txtCVVCode.Leave += new System.EventHandler(this.txtCVVCode_Leave);
            // 
            // frmPOSProcessCC
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(480, 394);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSProcessCC";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmPOSProcessCC_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPOSProcessCC_FormClosed);
            this.Load += new System.EventHandler(this.frmPOSChangeDue_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOSProcessCC_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPOSProcessCC_KeyUp);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtZipCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCCNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCVVCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion Windows Form Designer generated code

        public string SigType
        {
            get { return sigType; }
            set { sigType = value; }
        }

        public bool Tokenize
        {
            set; get;
        }

        private void frmPOSChangeDue_Load(object sender, System.EventArgs e)
        {
            logger.Trace("frmPOSChangeDue_Load() - Process CC Load " + clsPOSDBConstants.Log_Entering + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00"));
            this.Left = 100;		//( frmMain.getInstance().Width-this.Width)/2;
            this.Top = 100;			//(frmMain.getInstance().Height-this.Height)/2;
            clsUIHelper.setColorSchecme(this);
            this.txtAmount.Text = Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00");
            this.txtAmount.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtAmount.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtCCNo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtCCNo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtExpDate.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtExpDate.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.btnProcessManual.Enabled = Configuration.CPOSSet.AllowManualCCTrans;

            eCCResponse += new CCResponse(CCResponseFound);
            this.Show();
            Thread.Sleep(1);

            if (this.oTransType == POSTransactionType.Sales)
            {
                CheckFSAPaymentForPRIMERXPAY();
            }
            if (pmtProcessMethod.ToUpper() == "PRIMERXPAY")
            {
                this.Visible = false;
                btnContinue_Click(Keys.Enter, e);
                if (pmtProcessMethod.ToUpper() == "PRIMERXPAY")
                {
                    this.Close();
                }
                //Modified till here Mar-12-09
            }
            logger.Trace("frmPOSChangeDue_Load() - Process CC Load " + clsPOSDBConstants.Log_Exiting + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00"));
        }

        private void CCResponseFound(String input)
        {
            if (input == "E")
            {
                if (MessageBox.Show("Error while processing \n " + oCCDbf.GetValString("ERRORDESC").Trim() + "\n Do you want to retry ?", "Process Payment", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                {
                    this.isCanceled = true;
                    this.oThread.Abort();
                    this.Close();
                }
                else
                {
                    AddRowToFoxpro();
                }
            }
            else if (input == "A")
            {
                this.AllowCancel = false;
                this.AuthNo = oCCDbf.GetValString("AUTHNO");
                if ((this.AuthNo.Trim() != "" || oTransType == POSTransactionType.SalesReturn) && this.isCancelTransaction == false)
                {
                    frmPOSPayAuthNo ofrmAuthNo = new frmPOSPayAuthNo(oTransType);
                    ofrmAuthNo.txtAuthorizationNo.Text = this.AuthNo;
                    ofrmAuthNo.txtAuthorizationNo.ReadOnly = true;
                    this.btnProcessCancel.Enabled = false;
                    if (Configuration.CPOSSet.ShowAuthorization == true)
                    {
                        ofrmAuthNo.ShowDialog(this);
                    }
                    this.btnProcessCancel.Enabled = true;
                    if (!isCanceled)
                    {
                        oThread.Abort();
                        this.Close();
                    }
                }
                else if (this.isCancelTransaction == true)
                {
                    this.isCanceled = true;
                    this.Close();
                    oThread.Abort();
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Authorization No. cannot be blank");
                    this.isCanceled = false;
                    this.Close();
                    oThread.Abort();
                }
                this.isCanceled = this.isCancelTransaction;
            }
            else if (input == "P")
            {
                frmPOSGetDebitCard ofrmDBCard = new frmPOSGetDebitCard(this.oCCDbf);
                if (ofrmDBCard.ShowDialog(this) == DialogResult.Cancel)
                {
                    this.isCanceled = true;
                    this.Close();
                    oThread.Abort();
                }
                else
                {
                    this.AllowCancel = false;
                    //sClaimStat=" ";
                }
                this.isCanceled = this.isCancelTransaction;
            }
            else if (input == "I")
            {
                this.btnProcessCancel.Enabled = false;
                this.AllowCancel = false;
            }
            this.sbMain.Panels[0].Text = oCCDbf.GetValString("CURR_STAT");
            if (this.sbMain.Panels[0].Text.Trim() == "")
            {
                this.sbMain.Panels[0].Text = "Processing...";
            }

        }

        private void frmPOSProcessCC_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (this.AllowCancel == true)
            {
                if (e.KeyData == Keys.Escape)
                {
                    isCanceled = true;
                    if (oThread != null)
                    {
                        oThread.Abort();
                        oThread = null;

                        oCCDbf.db.lockFile();
                        oCCDbf.GetValField4("CLAIMID").assign(String.Empty);
                        oCCDbf.GetValField4("CLAIMSTAT").assign("N");
                        oCCDbf.db.write();
                        oCCDbf.db.unlock();
                    }
                    this.Close();
                }
            }
        }


        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            logger.Trace("btnContinue_Click() - " + clsPOSDBConstants.Log_Entering + "btnContinue_Click for :" + pmtProcessMethod);

            #region CommonForAllCases

            isManualProcess = "0";
            this.AllowCancel = false;
            this.sbMain.Panels[0].Text = "Initializing ....";
            this.ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();

            string responseStatus = string.Empty;

            #endregion CommonForAllCases

            switch (pmtProcessMethod)
            {
                case "PRIMERXPAY":

                    #region PRIMERXPAY Integration PRIMEPOS-2636

                    {
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign  Card :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Processign  Card :" + pmtProcessMethod + " Starting");

                        //if (!CheckAVSFields())
                        //    return;

                        PccCardInfo cardInfo = new PccCardInfo();
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Starting");
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Completed");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Completed");

                        if (isF10Press)
                        {
                            //cardInfo.IsCardPresent = "0";
                            cardInfo = this.CustomerCardInfo;//PRIMEPOS - 2530 - Added For Tokenization
                        }

                        cardInfo.transAmount = this.Amount.ToString();
                        cardInfo.IsFSATransaction = this.isFSATransaction.ToString();
                        cardInfo.PaymentProcessor = pmtProcessMethod;
                        cardInfo.TransactionID = this.originalTransID;

                        //2915
                        cardInfo.Email = this.Email;
                        cardInfo.IsEmail = this.IsEmail;
                        cardInfo.IsPhone = this.IsPhone;
                        cardInfo.Phone = this.Phone;
                        cardInfo.IsCustomerDriven = this.IsCustomerDriven;
                        cardInfo.CustomerName = this.CustomerName;
                        cardInfo.DOB = this.DOB;
                        cardInfo.IsPrimeRxPayLinkSend = this.IsPrimeRxPayLinkSend; //PRIMEPOS-3248
                        cardInfo.Tokenize = this.Tokenize;//PRIMEPOS-3186

                        if (!this.IsCustomerDriven)//2915
                        {
                            if (this.isFSATransaction)
                            {
                                //if (Resources.Message.Display(" Do you have FSA card ? ", " Select FSA Card ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                //{
                                cardInfo.isFSACard = true;
                                //}
                                //else
                                //{
                                //    cardInfo.isFSACard = false;
                                //}
                                cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
                                cardInfo.transFSAAmount = this.FSAAmount.ToString();
                            }

                            if (this.Tokenize)
                            {
                                cardInfo.Tokenize = true;
                            }
                        }

                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        ProcessTransaction(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo);
                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {
                                        ProcessTransaction(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo);
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    clsUIHelper.ShowErrorMsg("Does not support Debit");
                                    break;
                                }
                            case (PayTypes.EBT):
                                {
                                    clsUIHelper.ShowErrorMsg("Does not support EBT");
                                    break;
                                }
                        }

                        if (PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).pccRespInfo != null)
                        {

                            if ((PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).pccRespInfo.TrouTd == this.ticketNum) && (PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).pccRespInfo.Result == "SUCCESS"))
                            {
                                this.ticketNum = null;
                            }
                            SubCardType = PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).pccRespInfo.CardType;
                            if (oPayTpes == PayTypes.CreditCard && !string.IsNullOrEmpty(PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).VANTIV_SigString) && PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).VANTIV_SigString.Length > 0)
                            {
                                SigPadUtil.DefaultInstance.CustomerSignature = PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).VANTIV_SigString;
                            }
                            else
                            {
                                SigPadUtil.DefaultInstance.CustomerSignature = string.Empty;
                            }
                        }
                        else
                        {
                            SubCardType = string.Empty;
                        }


                        responseStatus = PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).ResponseStatus;
                        string responseError = PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).ResponseError;

                        this.ApprovedPCCCardInfo = cardInfo.Copy();

                        if (!OnlineResponseFound(responseStatus, PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).pccRespInfo))
                        {
                            isCanceled = true;
                            return;
                        }
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign   Card  for:" + pmtProcessMethod, "Completed");
                        logger.Trace("btnContinue_Click() - Processign   Card  for:" + pmtProcessMethod + "Completed");
                        break;
                    }

                #endregion  Integration by ARVIND
                default:
                    logger.Error("Payment processor not supported by the functionality");
                    clsUIHelper.ShowErrorMsg("Payment processor not supported by the functionality");
                    break;
            }

            logger.Trace("btnContinue_Click() - " + clsPOSDBConstants.Log_Exiting);
        }


        //private bool CheckAVSFields()
        //{
        //    bool bFlag = true;
        //    logger.Trace("CheckAVSFields() - " + pmtProcessMethod + clsPOSDBConstants.Log_Entering);
        //    if (PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).IsAVSModeOn.Equals("Y") && this.TrackNoII.Equals(string.Empty)) 
        //    {
        //        if (Configuration.CPOSSet.PaymentProcessor == "HPS" ? txtZipCode.Text.Trim().Length == 0 : txtAddress.Text.Trim().Length == 0 || txtZipCode.Text.Trim().Length == 0) 
        //        {
        //            if (POS_Core_UI.Resources.Message.Display("Address verification mode is on.\r\nPlease enter the customer Address and Zip code.\r\nBY LEAVING THESE FIELDS BLANK YOU \r\nMAY BE CHARGED A HIGHER PROCESSING FEE.\r\nDo you wants to leave these fields blank?", "AVS Verification", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //            {
        //                txtAddress.Text = string.Empty;
        //                txtZipCode.Text = string.Empty;
        //                bFlag = true;
        //            }
        //            else
        //            {
        //                this.txtAddress.Focus();
        //                this.btnContinue.Enabled = true;
        //                bFlag = false;
        //            }
        //        }
        //    }
        //    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "CheckAVSFields()" + pmtProcessMethod, "Exiting " + bFlag.ToString());
        //    logger.Trace("CheckAVSFields() - " + pmtProcessMethod + clsPOSDBConstants.Log_Exiting + bFlag.ToString());
        //    return bFlag;
        //}

        private void FillCardInfo(ref PccCardInfo cardInfo, bool fillCardDetails)
        {
            logger.Trace("FillCardInfo() - " + clsPOSDBConstants.Log_Entering);
            if (fillCardDetails)
            {
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    if (SigPadUtil.DefaultInstance.SigPadCardInfo != null && SigPadUtil.DefaultInstance.SigPadCardInfo.Track2.Trim().Length > 0)
                    {
                        cardInfo.cardHolderName = SigPadUtil.DefaultInstance.SigPadCardInfo.LastName + " " + SigPadUtil.DefaultInstance.SigPadCardInfo.FirstName;
                        txtCCNo.Text = SigPadUtil.DefaultInstance.SigPadCardInfo.CardNo;
                        cardInfo.Completetrack = SigPadUtil.DefaultInstance.SigPadCardInfo.Completetrack;
                        cardInfo.trackII = SigPadUtil.DefaultInstance.SigPadCardInfo.Track2;
                        txtExpDate.Text = SigPadUtil.DefaultInstance.SigPadCardInfo.ExpireOn;
                    }
                    else
                    {
                        cardInfo.trackII = this.TrackNoII.Trim();
                        cardInfo.Completetrack = CardSwiperCCNo.Trim();
                    }
                }
                else
                {
                    cardInfo.trackII = this.TrackNoII.Trim();
                    cardInfo.Completetrack = CardSwiperCCNo.Trim();
                }
                cardInfo.cardExpiryDate = txtExpDate.Text;
                cardInfo.customerAddress = txtAddress.Text;
                cardInfo.cardNumber = txtCCNo.Text;//sCardNumber.ToString()
                cardInfo.zipCode = txtZipCode.Text;
                cardInfo.cardType = PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).GetCardType(cardInfo.cardNumber);
            }


            if (this.oTransType == POSTransactionType.SalesReturn)
            {
                this.Amount = -(this.Amount);
                if (this.isFSATransaction)
                {
                    this.FSAAmount = -(this.FSAAmount);
                }
            }
            cardInfo.transAmount = this.Amount.ToString();
            if (this.FSAAmount > 0 || this.FSARxAmount > 0)
            {
                cardInfo.transFSAAmount = this.FSAAmount.ToString();
                cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
            }
            CheckIISTransactionFlag(ref cardInfo);
            logger.Trace("FillCardInfo() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void CheckIISTransactionFlag(ref PccCardInfo cardInfo)
        {
#if RITESHREVIEW
            logger.Trace("CheckIISTransactionFlag() - " + clsPOSDBConstants.Log_Entering);
            if (frmMain.sForceFSA == "Y")
            {
                this.isFSATransaction = true;
            }
            else
            {
                this.isFSATransaction = false;
            }
            if (Configuration.CPOSSet.PaymentProcessor == "XCHARGE" || Configuration.CPOSSet.PaymentProcessor == "XLINK")
            {
                if (this.isFSATransaction == true) 
                {
                    cardInfo.IsFSATransaction = "T";
                }
                else
                {
                    cardInfo.IsFSATransaction = "U";
                }
            }
            else if (Configuration.CPOSSet.PaymentProcessor == "PCCHARGE")
            {
                if (this.isFSATransaction == true) //Modified By Dharmendra (SRT) on Dec-04-08
                {
                    cardInfo.IsFSATransaction = "1";
                }
                else
                {
                    cardInfo.IsFSATransaction = "0";
                }
            }
            else if (Configuration.CPOSSet.PaymentProcessor == "HPS")
            {
                if (this.isFSATransaction == true)
                {
                    cardInfo.IsFSATransaction = "1";
                }
                else
                {
                    cardInfo.IsFSATransaction = "0";
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "CheckIISTransactionFlag()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CheckIISTransactionFlag() - " + clsPOSDBConstants.Log_Exiting);
#endif
        }


        private void AddRowToFoxpro()
        {

#if RITESHREVIEW
            oCCDbf.db.top();
            while (oCCDbf.db.eof() == 0)
            {
                if (oCCDbf.GetValString("CLAIMID").Trim() == "" && oCCDbf.db.deleted() == 0)
                {
                    if (oCCDbf.GetValString("CLAIMSTAT") == "N" || oCCDbf.GetValString("CLAIMSTAT").Trim() == "")
                    {
                        break;
                    }
                }
                oCCDbf.db.skip(1);
            }

            if (oCCDbf.db.eof() == 1)
            {
                oCCDbf.db.lockFile();
                oCCDbf.db.appendBlank();
            }
            else
            {
                oCCDbf.db.lockRecord(oCCDbf.db.recNo());
            }

            oCCDbf.GetValField4("CLAIMID").assign(clsUIHelper.GetRandomNo().ToString());
            oCCDbf.GetValField4("USERID").assign(Configuration.StationID);
            oCCDbf.GetValField4("CLAIMSTAT").assign(" ");
            if (isCancelTransaction == false)
            {
                if (this.oTransType == POSTransactionType.Sales)
                {
                    oCCDbf.GetValField4("TRANSTYPE").assign("S");
                }
                else
                {
                    oCCDbf.GetValField4("TRANSTYPE").assign("R");
                }
            }
            else
            {
                if (this.oTransType == POSTransactionType.Sales)
                {
                    oCCDbf.GetValField4("TRANSTYPE").assign("R");
                }
                else
                {
                    oCCDbf.GetValField4("TRANSTYPE").assign("S");
                }
            }

            oCCDbf.GetValField4("CARDNUMBER").assign(this.txtCCNo.Text);//);sCardNumber
            oCCDbf.GetValField4("CARDEXPDT").assign(this.txtExpDate.Text);
            if (this.Amount < 0)
            {
                oCCDbf.GetValField4("TRAMOUNT").assign(Convert.ToString(-1 * this.Amount));
            }
            else
            {
                oCCDbf.GetValField4("TRAMOUNT").assign(this.Amount.ToString());
            }

            //store trackII information in dbf file.
            if (this.TrackNoII.Trim() != "")
            {
                oCCDbf.GetValField4("TRACKII").assign(this.TrackNoII.Trim());
            }
            //oCCDbf.GetValField4("CLAIMSTAT").assign("N");
            oCCDbf.db.write();
            oCCDbf.db.unlock();

            this.btnContinue.Enabled = false;
            this.btnProcessManual.Enabled = false;
            this.txtCCNo.Enabled = false;
            this.txtExpDate.Enabled = false;
#endif
        }

        private void ExecuteCCResponseFound(string strResp)
        {

            if (this.InvokeRequired)
            {
                CCResponse d = new CCResponse(CCResponseFound);
                if (this.IsHandleCreated == true)
                {
                    this.Invoke(d, new object[] { strResp });
                }
            }
        }



        private void txtCCNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            logger.Trace("txtCCNo_Validating() - " + clsPOSDBConstants.Log_Entering);
            if (this.txtCCNo.Text.Trim() == "")
            {
                txtCCNo.PasswordChar = '\0';
                e.Cancel = true;
            }
            else
            {
                if (txtCCNo.Text.StartsWith("%B") || txtCCNo.Text.StartsWith("B"))
                {
                    String strScanned = txtCCNo.Text.Trim();
                    txtCCNo.Text = String.Empty;
                    String CCNo, ExpDate = "", customerName = "";
                    CCNo = strScanned;
                    CardSwiperCCNo = strScanned;

                    int startNo;
                    if (strScanned.StartsWith("%B"))
                        startNo = 2;
                    else
                        startNo = 1;

                    int caret1 = 0;
                    int caret2 = 0;
                    string lcSep;
                    if (strScanned.IndexOf("^") > 0)
                    {
                        caret1 = strScanned.IndexOf("^");
                        lcSep = "^";
                    }
                    else
                    {
                        if (strScanned.IndexOf("=") > 0)
                        {
                            caret1 = strScanned.IndexOf("=");
                            lcSep = "=";
                        }
                        else
                        {
                            caret1 = 0;
                            lcSep = " ";
                        }
                    }

                    if (caret1 > 0)
                    {
                        CCNo = strScanned.Substring(startNo, caret1 - startNo).Trim();
                        if (lcSep == "^")
                        {
                            caret2 = strScanned.IndexOf("^", caret1 + 1);
                            if (caret2 > 0)
                            {
                                ExpDate = strScanned.Substring(caret2 + 1, 4);
                                customerName = strScanned.Substring(caret1 + 1, caret2 - caret1 - 1);
                            }
                        }
                        else
                        {
                            ExpDate = strScanned.Substring(caret1, 4);
                        }
                        ExpDate = ExpDate.Substring(2, 2) + ExpDate.Substring(0, 2);
                    }

                    if (CCNo.Trim() != "" && ExpDate.Trim() != "")
                    {
                        int iStart = strScanned.IndexOf(";");
                        if (iStart > 0)
                        {
                            int iEnd = strScanned.LastIndexOf("?");
                            if (iEnd == -1)
                            {
                                iEnd = strScanned.Length;
                            }
                            TrackNoII = strScanned.Substring(iStart + 1, iEnd - iStart - 1);
                        }
                    }

                    this.txtCCNo.Text = removeSpaces(CCNo);
                    this.txtCCNo.PasswordChar = 'X'; //Added by Manoj 9/5/2013 Task# 1324
                    this.txtExpDate.Text = ExpDate;
                }
            }
            logger.Trace("txtCCNo_Validating() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmPOSProcessCC_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmPOSProcessCC_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtCCNo_ValueChanged(object sender, System.EventArgs e)
        {
            if (txtCCNo.Text.EndsWith(Convert.ToChar(13).ToString()))
            {
                System.Windows.Forms.SendKeys.Send("{enter}");
            }
        }

        private void frmPOSProcessCC_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "frmPOSProcessCC_Closing()", clsPOSDBConstants.Log_Entering);
            logger.Trace("frmPOSProcessCC_Closing() - " + clsPOSDBConstants.Log_Entering);
            switch (pmtProcessMethod)
            {
                case "PRIMERXPAY": //PRIMEPOS-2761
                    {
                        PccPaymentSvr.sCurrentTicket = ticketNum;
                        break;
                    }
            }
            logger.Trace("frmPOSProcessCC_Closing() - " + clsPOSDBConstants.Log_Exiting);
        }

        private String removeSpaces(String str)
        {
            bool flag = true;
            int index = 0;

            while (flag)
            {
                index = str.IndexOf(" ");
                if (index > 0)
                {
                    str = str.Substring(0, index) + str.Substring(index + 1);
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            return str;
        }

        private void btnProcessManual_Click(object sender, System.EventArgs e)
        {
            logger.Trace("btnProcessManual_Click() - " + clsPOSDBConstants.Log_Entering);
            isManualProcess = "1";
            frmPOSPayAuthNo oAuthNo = new frmPOSPayAuthNo(oTransType);
            if (txtExpDate.Text.Trim().Length < 4)
            {
                POS_Core_UI.Resources.Message.Display("Invalid Card. Expiry date must be in the format YYMM");
                txtExpDate.Focus();
            }
            else if (ParseExpiryDate(txtExpDate.Text.Trim()) == false)
            {
                POS_Core_UI.Resources.Message.Display("Invalid Card. Expiry date must be in the format YYMM");
                txtExpDate.Focus();
            }
            else if (ValiDateExpiryDate(txtExpDate.Text.Trim()) == false)
            {
                POS_Core_UI.Resources.Message.Display("Invalid Card. Expiry date must be in the format YYMM");
                txtExpDate.Focus();
            }
            else
            {

                oAuthNo.ShowDialog(this);
                AuthNo = oAuthNo.txtAuthorizationNo.Text;
                if (AuthNo.Trim() == "")
                {
                    logger.Trace("btnProcessManual_Click() - " + clsPOSDBConstants.Log_Exiting + " with Blank Authorization No");
                    return;
                }
                PccCardInfo objPccCardInfo = new PccCardInfo();
                FillCardInfo(ref objPccCardInfo, true);
                SubCardType = objPccCardInfo.cardType;
                this.ApprovedPCCCardInfo = objPccCardInfo.Copy();
                logger.Trace("btnProcessManual_Click() - " + clsPOSDBConstants.Log_Exiting);
                this.Close();
            }
        }

        private bool ValiDateExpiryDate(string expDate)
        {
            bool isValidExpiryDate = true;
            int currentYear = DateTime.Now.Year;
            string leftmostTwoDigitsOfCurYear = currentYear.ToString().Trim().Substring(0, 2);
            int expYear = Convert.ToInt32(leftmostTwoDigitsOfCurYear + expDate.Substring(0, 2));
            int expMonth = Convert.ToInt32(expDate.Substring(2, 2));
            if (expYear < currentYear)
            {
                isValidExpiryDate = false;
            }
            if (expMonth < 0 || expMonth > 12)
            {
                isValidExpiryDate = false;
            }
            return isValidExpiryDate;
        }

        private bool ParseExpiryDate(string expDate)
        {
            bool isNumeric = true;
            int numValue = 0;
            isNumeric = Int32.TryParse(expDate, out numValue);
            return isNumeric;
        }


        private void txtExpDate_Leave(object sender, System.EventArgs e)
        {
        }


        private bool OnlineResponseFound(string status, PccResponse objResponse)
        {
            bool isFound = false;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(objResponse?.Result))
            {
                if (objResponse.Result.ToUpper() == "APPROVED" && IsCustomerDriven == true)
                {
                    clsUIHelper.ShowOKMsg(objResponse.ResultDescription);
                    isFound = false;
                    this.PrimeRxPayTransID = objResponse.TransId;
                }
                else if (objResponse.Result.ToUpper() != "SUCCESS")
                {
                    msg = string.IsNullOrEmpty(objResponse.Result) ? status.ToUpper() : objResponse.ResultDescription;
                    this.ticketNum = null;
                    Resources.Message.Display(" Card was rejected. \r \n " + objResponse.Result + " \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                    isFound = false;
                }
                else
                {
                    if (objResponse.EmvReceipt != null)
                    {
                        Resources.Message.Display(" Card XXXXXX" + objResponse.CardNo + " \n " + "ApprovalCode " + objResponse.EmvReceipt.ApprovalCode +
                            "\n " + "ApprovedAmount " + objResponse.AmountApproved.ToString("f2"), " Message", MessageBoxButtons.OK);
                    }
                    if (objResponse.AmountApproved > 0)
                    {
                        if (Tokenize == true)
                        {
                            if (CustomerCardInfo == null)
                            {
                                SubCardType = objResponse.CardType;
                            }
                            else
                            {
                                SubCardType = CustomerCardInfo.cardType;
                            }
                        }
                        else
                        {
                            SubCardType = objResponse.CardType;
                        }

                        this.ApprovedPCCCardInfo.tRoutId = objResponse.TransId;
                        this.isFSATransaction = objResponse.isFSATransaction;
                        this.Amount = (decimal)objResponse.AmountApproved;
                        this.AuthNo = objResponse.AuthNo;
                        if (this.txtCCNo.Text.Trim().Length == 0 && objResponse != null)
                        {
                            this.txtCCNo.Text = objResponse.CardNo;
                        }

                        if (this.txtExpDate.Text.Trim().Length == 0 && objResponse != null)
                        {
                            this.txtExpDate.Text = objResponse.Expiry;
                        }

                        isFound = true;
                    }
                }
            }
            return isFound;
        }


        private void btnProcessCancel_Click(object sender, EventArgs e)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "btnProcessCancel_Click", clsPOSDBConstants.Log_Entering);
            if (this.AllowCancel == true)
            {
                this.isCanceled = true;

                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    SigPadUtil.DefaultInstance.SigPadCardInfo = null;
                }

                this.Close();
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "btnProcessCancel_Click", clsPOSDBConstants.Log_Exiting);
        }
        public bool PerformVoidTransaction(PccCardInfo pccCardInfo, POSTransactionType oTransType)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformVoidTransaction", clsPOSDBConstants.Log_Entering);
            bool retVal = false;
            string ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();
            if (pccCardInfo.tRoutId.Trim() == string.Empty)
            {
                POS_Core_UI.Resources.Message.Display("This credit card payment was processed manually.\nEnsure this payment is also reversed externally.", "Cancel Pay", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }


            frmWaitScreen ofrmWait = new frmWaitScreen(true, "Please wait...", "Processing Payment Online..."); // NileshJ - Hide Cancel Button and Show //PRIMEPOS-3334
            ofrmWait.Show();

            if (oTransType == POSTransactionType.Sales)
            {
                if (pccCardInfo.cardType == PayTypes.CreditCard)
                {
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo);
                }
            }
            else if (oTransType == POSTransactionType.SalesReturn)
            {
                if (pccCardInfo.cardType == PayTypes.CreditCard)
                {
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo);
                }
            }
            else
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformVoidTransaction", clsPOSDBConstants.Log_Exiting);
                return retVal;
            }

            String responseStatus = PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).ResponseStatus;

            if (responseStatus.ToUpper().Trim() == "SUCCESS")
            {
                retVal = true;
                POS_Core_UI.Resources.Message.Display(" Voided Successfully ", " Message ");
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformVoidTransaction", clsPOSDBConstants.Log_Exiting);

            ofrmWait.Close();

            return retVal;
        }
        private void frmPOSProcessCC_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtCCNo_TextChanged(object sender, EventArgs e)
        {
            if (txtCCNo.Text.Length <= 1)
            {
                txtCCNo.PasswordChar = '\0';
            }

        }

        private void txtCVVCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        private void txtCVVCode_TextChanged(object sender, EventArgs e)
        {
            if (txtCVVCode.Text.Length <= 1)
            {
                txtCVVCode.PasswordChar = '\0';
            }
        }

        private void txtCVVCode_ValueChanged(object sender, EventArgs e)
        {
            if (txtCVVCode.Text.EndsWith(Convert.ToChar(13).ToString()))
            {
                System.Windows.Forms.SendKeys.Send("{enter}");
            }
        }

        private void txtCVVCode_Leave(object sender, EventArgs e)
        {
            if (txtCVVCode.Text.Length > 2)
            {
                txtCVVCode.PasswordChar = 'X';
            }
        }


        private void txtCCNo_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtCCNo_Leave(object sender, EventArgs e)
        {
            if (txtCCNo.Text.Length > 11)
            {
                txtCCNo.PasswordChar = 'X';
            }
        }


        private void ProcessTransaction(string cardType, string transType, string ticketNum, ref PccCardInfo cardInfo)
        {
            switch (cardType)
            {
                case PayTypes.CreditCard:
                    if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.Sales))
                    {
                        PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).PerformCreditSale(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.SalesReturn))
                    {
                        PccPaymentSvr.GetOnlinePaymentInstance(pmtProcessMethod).PerformCreditSalesReturn(ticketNum, ref cardInfo);
                    }
                    break;
            }

        }

        //PRIMEPOS-TOKENSALE
        public void isF10(bool press, string ccnumber, string ccexp)
        {
            isF10Press = press;
        }

    }
}