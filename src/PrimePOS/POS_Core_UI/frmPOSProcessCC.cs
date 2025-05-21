using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using FirstMile;
using NLog;
using POS_Core.Resources;
using POS_Core.Resources.PaymentHandler;
using POS_Core.TransType;
using POS_Core_UI.Resources;
using POS_Core.LabelHandler.RxLabel;
using POS.UI;
using Evertech;
using System.Text.RegularExpressions;
using NBS.RequestModels; //PRIMEPOS-3372
using System.Collections.Generic; //PRIMEPOS-3372
using NBS;
using NBS.ResponseModels;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for frmPOSChangeDue.
    /// </summary>
    public class frmPOSProcessCC : System.Windows.Forms.Form
    {
        public bool IsElavonTax = false;//2943
        public string ElavonTotalTax = string.Empty;//2943
        public delegate void CCResponse(string input);
        public event CCResponse eCCResponse;
        private string TrackNoII = "";
        public bool isCanceled = false;
        public String AuthNo;
        public string ExpirayDate = string.Empty;//2943
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
        private PccPaymentSvr objPccPmt = new PccPaymentSvr(); //Added By SRT
        private string pmtProcessMethod = Configuration.CPOSSet.PaymentProcessor; //Modified By Dharmendra On Nov-14-08 Reading Payment Processor from POSSET instead of App.config
        private string TrackNoIII = ""; //Added By SRT
        private const string DBF = "DBF";
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtZipCode;
        private Infragistics.Win.Misc.UltraLabel lblZipCode;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtAddress;
        private Infragistics.Win.Misc.UltraLabel lblAddress;
        public String SubCardType = null;
        public String sCardNumber = string.Empty;

        public string PaymentType = string.Empty; //PRIMEPOS-3526 //PRIMEPOS-3504
        public string AccountNo = string.Empty; //PRIMEPOS-3372
        public string AccountNoWithoutMask = string.Empty; //PRIMEPOS-3372
        public string BinValue = string.Empty; //PRIMEPOS-3372
        public string AccountHolderName = string.Empty; //PRIMEPOS-3372
        public string PreReadId = string.Empty; //PRIMEPOS-3372
        public string NBSTransId = string.Empty; //PRIMEPOS-3375
        public string NBSTransType = string.Empty; //PRIMEPOS-3375
        public string NBSPaymentType = string.Empty; //PRIMEPOS-3375
        public bool IsNBSTransDoneOnce = false; //PRIMEPOS-3524 //PRIMEPOS-3504
        public string ReturnTransType = string.Empty; //PRIMEPOS-3521 //PRIMEPOS-3522 //PRIMEPOS-3504
        public string NBSUid = string.Empty; //PRIMEPOS-3375
        public bool isNBSPayment = false; //PRIMEPOS-3326 //PRIMEPOS-3504
        public bool iSBinVerified = false; //PRIMEPOS-3372
        public bool iSNBSExOccurred = false; //PRIMEPOS-3372

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
        private string PAX_DEVICE_ARIES8 = "HPSPAX_ARIES8"; // Amit HPSPAX_ARIES8 Add    //PRIMEPOS-2952
        private string PAX_DEVICE_A920 = "HPSPAX_A920"; //PRIMEPOS-3146
        private string Evertec = "EVERTEC"; // ARVIND - PRIMEPOS-2664
        private string CardSwiperCCNo = string.Empty;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public String ticketNum = null;// NileshJ - 20-Dec-2018
        private string VANTIV = "VANTIV"; // Arvind - 11 July 2019 PRIMEPOS-2636 
        //ARVIND PRIMEPOS-2738 
        public String originalTransID = string.Empty;
        public String hrefNumber = string.Empty;
        public string TransTypeCode = string.Empty;//PRIMEPOS-3087
        public string NBSReturnUid = string.Empty;//PRIMEPOS-3375
        //
        public String TransDate = string.Empty;//2943
        public bool isAllowDuplicate = false;//PRIMEPOS-2784
        public decimal PendingAmount = 0;//2943
        //2664
        public string EvertecTaxDetails = string.Empty;
        public decimal CashBackAmount;
        public List<AnalyzeLineItem> lineItemsdata = new List<AnalyzeLineItem>(); //PRIMEPOS-3372
        //

        public bool IsFondoUnica = false;//2664
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

        public frmPOSProcessCC(POSTransactionType oType, string paymentType)//Added additional parameter for paymentType by Dharmendra(SRT)
        {
            this.oTransType = oType;
            this.oPayTpes = paymentType; // Added by Dharmendra(SRT)
            if (pmtProcessMethod == string.Empty || pmtProcessMethod == null)
                pmtProcessMethod = DBF;
            InitializeComponent();
            this.btnProcessCancel.Enabled = false;
            //Added By SRT(Ritesh Parekh) Date : 22-Jul-2009
            //This variable will hold old processor name and reassign at end of flow.
            OldProcessor = Configuration.CPOSSet.PaymentProcessor;

            //End Of Added By (Ritesh Parekh)

#if TEST
            this.FSAAmount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["FSAAMOUNT"].ToString());
#endif
        }

        public PccCardInfo CustomerCardInfo
        {
            private get
            {
                return this.objCustomerCardInfo;
            }
            set
            {
                this.objCustomerCardInfo = value;
            }
        }
        /*
      * Suraj Added For HPSPAX FSA Check - 1-Aug-2018
      * 
      * 
     */
        private void CheckFSAPaymentForHPSPAX()
        {
            if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.HPSPAX)
            {
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {

                    //if (POS_Core_UI.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    // PRIMEPOS-2644 - NileshJ
                    //if (Configuration.CInfo.RestrictFSACardMessage == false &&
                    //    (POS.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes))  //PRIMEPOS-2621 03-Jan-2019 JY Modified
                    //{
                    pmtProcessMethod = clsPOSDBConstants.HPSPAX;
                    Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.HPSPAX;
                    this.isFSATransaction = true;
                    // }

                    // WasProcessorChanged = true;
                    // objPccPmt = new PccPaymentSvr();
                }
            }
        }
        //PRIMEPOS-2664
        private void CheckFSAPaymentForEVERTEC()
        {
            if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.EVERTEC)
            {
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {

                    pmtProcessMethod = clsPOSDBConstants.EVERTEC;
                    Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.EVERTEC;
                    this.isFSATransaction = true;

                    // WasProcessorChanged = true;
                    // objPccPmt = new PccPaymentSvr();
                }
            }
        }
        private void CheckFSAPaymentForWorldPay()
        {
            if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.WORLDPAY)
            {
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {

                    //if (POS_Core_UI.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    if (Configuration.CInfo.RestrictFSACardMessage == false &&
                        (POS_Core_UI.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes))  //PRIMEPOS-2621 18-Dec-2018 JY Modified
                    {
                        pmtProcessMethod = clsPOSDBConstants.WORLDPAY;
                        Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.WORLDPAY;
                        this.isFSATransaction = true;
                    }

                    // WasProcessorChanged = true;
                    // objPccPmt = new PccPaymentSvr();
                }
            }
        }

        private void CheckFSAPaymentForPCCHARGE()
        {
            //Added By SRT(Ritesh Parekh) Date : 22-Jul-2009

            #region Code For FSA transaction Thro PCCHARGE

            //To Check whether the processor is pccharge
            if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.PCCHARGE)
            {
                //if processor is pccharge
                //To check whether iias amount is !=0
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {
                    //Ask user whether he is presenting the Flex Card / FSA Card? (yes/no)
                    //if(POS_Core_UI.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    if (Configuration.CInfo.RestrictFSACardMessage == false &&
                        (POS_Core_UI.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes))  //PRIMEPOS-2621 18-Dec-2018 JY Modified
                    {
                        //if iias amount is != 0
                        //To check whether ProcWithXCHARGE is set to True
                        if (Configuration.CPOSSet.procFSAWithXLINK)
                        {
                            //if ProcWithXCHARGE is set to True
                            //frmMain.sForceFSA = "Y"; //Need to check value to set 'Y' or 'N'
                            //Set Total Amount = IIAS Amount if total amount > fsaamount to avoid
                            //transaction issue with xlink
                            if (this.Amount > this.FSAAmount)
                            {
                                this.Amount = this.FSAAmount;
                            }
                            else
                            {
                                //To write here code for validating amount with fsa amount.
                                //Because xlink does not support partial payment of fsa amount
                            }
                            pmtProcessMethod = clsPOSDBConstants.XLINK;
                            Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.XLINK;
                            WasProcessorChanged = true;
                            objPccPmt = new PccPaymentSvr();
                            //Send Transaction to XLINK
                        }
                        else
                        {
                            //If user is presenting FSA card (yes)
                            //this.Amount = this.FSAAmount;
                            frmMain.sForceFSA = "Y";
                            //set FSA flag to True
                        }
                    }
                    //if ProcWithXCHARGE is set to False
                    else
                    {
                        //If user is not presenting FSA card. (no)
                        frmMain.sForceFSA = "N";
                        //set FSA flag to False
                        //Process the transaction thro PCCHARGE
                    }
                }
                else
                {
                    //if iias amount is = 0
                    frmMain.sForceFSA = "N";
                    //set FSA flag to False and process entire transaction with pccharge
                }
            }
            else if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.XLINK)
            {   // Added by Manoj - 9/27/2011 This fix the FSA for XLINK Payment
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {
                    if (this.Amount > this.FSAAmount)
                    {
                        this.Amount = this.Amount;
                    }
                    pmtProcessMethod = clsPOSDBConstants.XLINK;
                    Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.XLINK;
                    WasProcessorChanged = true;
                    objPccPmt = new PccPaymentSvr();
                    //Send Transaction to XLINK
                }
                else
                {
                    //if iias amount is = 0
                    frmMain.sForceFSA = "N";
                    //set FSA flag to False and process entire transaction with pccharge
                }
            }
            //if processor is not pccharge
            //Process transaction as default

            #endregion Code For FSA transaction Thro PCCHARGE

            //End Of Added By SRT(Ritesh Parekh)
        }

        private void CheckFSAPaymentForHPS()
        {
            if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.HPS)
            {
                //if processor is HPS
                //To check whether iias amount is !=0
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {
                    //Ask user whether he is presenting the Flex Card / FSA Card? (yes/no)
                    /*if (!isF10Press)  Comment out by Manoj- We are checking if the card bin matches HPS fsa bin in another function (HPS.dll)
                    {
                        if (POS_Core_UI.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS",
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            frmMain.sForceFSA = "Y";
                        }
                        else
                        {
                            frmMain.sForceFSA = "N";
                        }
                    }*/
                    //else
                    // {
                    frmMain.sForceFSA = "Y"; // Set FSA as yes so that if they save the FSA card in Primerx it will check HPS to verify FSA card is valid.
                    //}
                }
                else
                {
                    frmMain.sForceFSA = "N";
                }
            }
        }
        //PRIMEPOS-2636
        private void CheckFSAPaymentForVANTIV()
        {
            if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.VANTIV)
            {
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {

                    //if (POS.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //if (Configuration.CInfo.RestrictFSACardMessage == false //&&
                    //    //(POS.Resources.Message.Display("Are you presenting Flex Card / FSA Card ?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //    )  //PRIMEPOS-2621 03-Jan-2019 JY Modified
                    //{
                    //    pmtProcessMethod = clsPOSDBConstants.HPSPAX;
                    //    Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.HPSPAX;
                    //    this.isFSATransaction = true;
                    //}

                    pmtProcessMethod = clsPOSDBConstants.VANTIV;
                    Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.VANTIV;
                    this.isFSATransaction = true;

                    // WasProcessorChanged = true;
                    // objPccPmt = new PccPaymentSvr();
                }
            }
        }
        private void CheckFSAPaymentForELAVON()//2943
        {
            if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.ELAVON)
            {
                if (this.FSAAmount != Configuration.convertNullToDecimal("0.00"))
                {
                    pmtProcessMethod = clsPOSDBConstants.ELAVON;
                    Configuration.CPOSSet.PaymentProcessor = clsPOSDBConstants.ELAVON;
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

        public string CustomerSignature
        {
            get
            {
                return strSignature;
            }
        }

        public string SigType
        {
            get
            {
                return sigType;
            }
            set
            {
                sigType = value;
            }
        }

        public byte[] BinarySignature
        {
            get
            {
                return binSignature;
            }
        }

        /// <summary>
        /// Added by Manoj 9/24/2013
        /// This check the user permission to see if they have rights to skip F10 signature.
        /// </summary>
        /// <returns></returns>
        private bool F10Skip()
        {
            bool skip = false;
            try
            {
                if (isF10Press && Configuration.CPOSSet.SkipF10Sign)
                {
                    clsUIHelper.IsF10Trans = true;
                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.F10SignSkip.ID, UserPriviliges.Permissions.F10SignSkip.Name))
                    {
                        skip = true;
                    }
                    else
                    {
                        if (POS_Core_UI.Resources.Message.Display("This transaction requires Security Rights \nto Skip the Signature. \n\nDo you wish to Proceed?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.F10SignSkip.ID, UserPriviliges.Permissions.F10SignSkip.Name))
                            {
                                skip = true;
                            }
                        }
                    }
                }
                return skip;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "F10Skip()");
                return skip;
            }
            finally
            {
                clsUIHelper.IsF10Trans = false;
            }
        }

        /// <summary>
        /// SKip the signature for Any amount below $20.00
        /// </summary>
        /// <returns></returns>
        private bool SkipLessthan20()
        {
            bool skip = false;
            if (Convert.ToInt32(Amount) <= 20 && Configuration.CPOSSet.SkipAmountSign)
            {
                skip = true;
            }
            return skip;
        }

        public bool Tokenize
        {
            set; get;
        }

        //private bool CaptureSignature(bool isEMVCard = false)//Nileshj
        private bool CaptureSignature(bool isEMVCard = false)//Nileshj
        {
            bool retVal = true;

            if (F10Skip() || SkipLessthan20() || SkipEMVCardSign(isEMVCard))
            {
                return true;
            }
            //Added By SRT(Ritesh Parekh) Date : 20-Aug-2009
            //Checked if card type is debit card and capture signature for debit card is set to false
            //then used to exit from function.
            if ((oPayTpes == PayTypes.DebitCard && Configuration.CPOSSet.CaptureSigForDebit == false) ||
                (oPayTpes == PayTypes.EBT && !Configuration.CPOSSet.CaptureSigForEBT))
            {
                return retVal;
            }
            //End Of Added By SRT(Ritesh Parekh)

            if (Configuration.CPOSSet.IsTouchScreen && Configuration.CPOSSet.PinPadModel.Contains(BlueToothICMP))
            {
                retVal = TouchScreen_CaptureSignatureCC();
                return retVal;
            }

            if (Configuration.CPOSSet.UseSigPad == true)
            {
                if (SigPadUtil.DefaultInstance.isConnected()) //Added by Manoj 4/25/2012
                {
                    strSignature = "";
                    string strMessage = string.Empty;
                    if (oTransType == POSTransactionType.Sales)
                    {
                        strMessage = "Amount charged = " + Configuration.CInfo.CurrencySymbol + this.Amount.ToString("##,###,##0.00");
                    }
                    else if (oTransType == POSTransactionType.SalesReturn)
                    {
                        strMessage = "Amount Returned = " + Configuration.CInfo.CurrencySymbol + this.Amount.ToString("##,###,##0.00");
                    }
                    if (SigPadUtil.DefaultInstance.CaptureSignature(strMessage) == true)
                    {
                        strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                        SigType = SigPadUtil.DefaultInstance.SigType;
                        if ((Configuration.CPOSSet.DispSigOnTrans == true) && (strSignature.Trim().Length > 0))
                        {
                            //Modified By Dharmendra SRT on Jan-31-09
                            if (Configuration.CPOSSet.UseSigPad == true)
                            {
                                SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                            }
                            //Modified Till Here Jan-31-09
                            this.SigType = SigPadUtil.DefaultInstance.SigType;
                            //  Mantis Id : 0000119 Modified By Dharmendra (SRT) on Dec-02-08 passed the extraparameter as true
                            frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                            ofrm.SetMsgDetails("Validating Signature...");
                            ofrm.ShowDialog();
                            //Modified By Dharmendra(SRT) on Dec-15-08 to reinitialize the signature variables
                            if (ofrm.IsSignatureRejected == true)
                            {
                                ofrm = null;
                                strSignature = "";
                                if (Configuration.CPOSSet.UseSigPad == true)
                                {
                                    SigPadUtil.DefaultInstance.CustomerSignature = null;
                                }
                                retVal = false;
                                CaptureSignature();
                            }
                            else
                            {
                                retVal = true;
                            }
                            //End Modified
                            //Mantis Id : 0000119 Modified Till Here
                        }
                        else if (Configuration.CPOSSet.DispSigOnTrans == false && strSignature.Trim().Length > 0)
                        {
                            strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                        }
                    }
                    /*else
                    {
                        CancelTransaction();
                        retVal = false;
                    }*/
                }
            }
            return retVal;
        }

        private void frmPOSChangeDue_Load(object sender, System.EventArgs e)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Process CC Load", clsPOSDBConstants.Log_Entering + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00"));
            logger.Trace("frmPOSChangeDue_Load() - Process CC Load " + clsPOSDBConstants.Log_Entering + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00"));
            bool ispress = false;
            //Added By Dharmendra (SRT) on Dec-15-08 to check whether user have changed the default Payment Processor settings or not.
            //If the settings are changed then the code bolck below will prompt user to
            if (pmtProcessMethod != Configuration.DefaultPaymentProcessor)
            {
                POS_Core_UI.Resources.Message.Display("The Default Payment Processor Was " + Configuration.DefaultPaymentProcessor + "\r\nNow It Is Set As " + pmtProcessMethod + ". Card Payments Will Not Be Processed Unless The PaymentHost \r\nServer Settings are Confirmed/Changed And Application Is Restarted", "Warning", MessageBoxButtons.OK);
                this.AllowCancel = true;
                System.Windows.Forms.KeyEventArgs eventArgs = new System.Windows.Forms.KeyEventArgs(Keys.Escape); //PRIMEPOS-3372
                frmPOSProcessCC_KeyUp(this, eventArgs); //This will pass the key pressed as Esc key & will forcably close the form to return back to paymentlist screen
                return;
            }
            //End Added
            // string opsStatus = string.Empty;
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

            //Naim Ijaz 28Oct2008
            //added to disable/enable manual processing based on settings
            this.btnProcessManual.Enabled = Configuration.CPOSSet.AllowManualCCTrans;

            eCCResponse += new CCResponse(CCResponseFound);
            this.Show();
            Thread.Sleep(1);

            //call if it is sales transaction
            //changed by SRT(Abhishek) 12 Aug 09
            if (this.oTransType == POSTransactionType.Sales)
            {
                CheckFSAPaymentForPCCHARGE();
                CheckFSAPaymentForHPS();
                CheckFSAPaymentForWorldPay();
                CheckFSAPaymentForHPSPAX();
                CheckFSAPaymentForEVERTEC(); // ARVIND - PRIMEPOS-2664
                CheckFSAPaymentForVANTIV();//ADDED FOR VANTIV FSACheck PRIMEPOS-2636
                CheckFSAPaymentForELAVON();//2943
            }
            //End

            //PRIMEPOS-2528 (Suraj) 1-june-18 Added HPSPAX PaymentProcessor
            if ((Configuration.CPOSSet.ProcessOnLine == true && pmtProcessMethod == "XLINK") ||
                (pmtProcessMethod.ToUpper() == "WORLDPAY") || (pmtProcessMethod.ToUpper() == "HPSPAX") || (pmtProcessMethod.ToUpper() == "EVERTEC") || (pmtProcessMethod.ToUpper() == "VANTIV") || (pmtProcessMethod.ToUpper() == "ELAVON"))//PRIMEPOS-2664 ADDED BY ARVIND PRIMEPOS-2636//2943
            {
                this.Visible = false;
                btnContinue_Click(Keys.Enter, e);
                if (pmtProcessMethod.ToUpper() == "WORLDPAY" || (pmtProcessMethod.ToUpper() == "HPSPAX" || (pmtProcessMethod.ToUpper() == "EVERTEC")) || (pmtProcessMethod.ToUpper() == "VANTIV") || (pmtProcessMethod.ToUpper() == "ELAVON"))//PRIMEPOS-2664 ADDED BY ARVIND PRIMEPOS-2636//2943
                {
                    this.Close();
                }
                //Modified till here Mar-12-09
            }
            else
            {
                try
                {
                    if (Configuration.CPOSSet.UseSigPad == true)
                    {
                        //Changed By SRT(Gaurav) Date : 19-NOV-2008
                        //Change Datails: Changed position of if condition. This will enable to
                        //bipass card reading while processor type is xlink.
                        //Added By SRT(Gaurav) Date : 18-NOV-2008
                        //Mantis ID: 0000112
                        if (pmtProcessMethod == "XLINK")
                        {
                            //Added By SRT(Gaurav) Date : 19-NOV-2008
                            //Mantis ID: 0000112
                            txtCCNo.Text = string.Empty;
                            txtExpDate.Text = string.Empty;
                            txtAddress.Text = string.Empty;
                            txtZipCode.Text = string.Empty;
                            //End Of Added By SRT(Gaurav)
                            txtCCNo.Enabled = false;
                            txtExpDate.Enabled = false;
                            txtAddress.Enabled = false;
                            txtZipCode.Enabled = false;

                            //End Of Added By SRT(Gaurav)
                        }
                        //Changed By SRT(Gaurav) Date : 19 NOV 2008
                        //Mantis ID: 0000118
                        //isF10Press == false is added by shitaljit on 15 march 2012.
                        //comment by Manoj 3/16/2012
                        //else if (Configuration.CPOSSet.UseSigPad == true && isF10Press == false && SigPadUtil.DefaultInstance.CaptureCreditCardInfo(this.txtAmount.Text, this.oPayTpes) == true)

                        //change by Manoj 03/16/2012
                        else if (Configuration.CPOSSet.UseSigPad == true && (ispress = (isF10Press ? (SigPadUtil.DefaultInstance.oSigPadCardData != null || objCustomerCardInfo != null) : SigPadUtil.DefaultInstance.CaptureCreditCardInfo(this.txtAmount.Text, this.oPayTpes) == true))) //Added additional parameter by Dharmendra(SRT)
                        {
                            if (pmtProcessMethod == clsPOSDBConstants.PCCHARGE) //Add Manoj
                            {
                                if (objCustomerCardInfo != null)
                                {
                                    txtCCNo.Text = objCustomerCardInfo.cardNumber.Trim();
                                    txtExpDate.Text = objCustomerCardInfo.cardExpiryDate.Trim();
                                    txtAddress.Text = objCustomerCardInfo.customerAddress.Trim();
                                    txtZipCode.Text = objCustomerCardInfo.zipCode.Trim();
                                    this.txtCCNo.ReadOnly = true;
                                    this.txtExpDate.ReadOnly = true;
                                }
                                else
                                {
                                    this.txtCCNo.Text = SigPadUtil.DefaultInstance.SigPadCardInfo.CardNo;
                                    /*if (SigPadUtil.DefaultInstance.SigPadCardInfo.Track2.Trim().Length > 0)
                                    {
                                        if (SigPadUtil.DefaultInstance.SigPadCardInfo.Track2.StartsWith(";") == false)
                                        {
                                            this.txtCCNo.Text += ";" + SigPadUtil.DefaultInstance.SigPadCardInfo.Track2;
                                        }
                                        else
                                        {
                                            this.txtCCNo.Text += SigPadUtil.DefaultInstance.SigPadCardInfo.Track2;
                                        }
                                    }*/
                                    //txtCCNo_Validating(txtCCNo, new CancelEventArgs());
                                    //if (Configuration.CPOSSet.PaymentProcessor == "HPS")
                                    //{
                                    //    this.TrackNoII = "%B" + sCardNumber + "^" + SigPadUtil.DefaultInstance.SigPadCardInfo.LastName + " " + SigPadUtil.DefaultInstance.SigPadCardInfo.FirstName +"?;" + SigPadUtil.DefaultInstance.SigPadCardInfo.Track2 + "?";
                                    //}
                                    //else
                                    //{
                                    //    this.TrackNoII = SigPadUtil.DefaultInstance.SigPadCardInfo.Track2;
                                    //}
                                    this.TrackNoII = SigPadUtil.DefaultInstance.SigPadCardInfo.Track2;
                                    this.txtExpDate.Text = SigPadUtil.DefaultInstance.SigPadCardInfo.ExpireOn;
                                    this.txtCCNo.ReadOnly = true;
                                    this.txtExpDate.ReadOnly = true;
                                    //Added by Dharmendra(SRT)
                                    this.TrackNoIII = SigPadUtil.DefaultInstance.SigPadCardInfo.Track3;
                                }
                            }
                            else if (pmtProcessMethod == clsPOSDBConstants.HPS) //Add Manoj
                            {
                                if (objCustomerCardInfo != null)
                                {
                                    txtCCNo.Text = objCustomerCardInfo.cardNumber.Trim();
                                    txtExpDate.Text = objCustomerCardInfo.cardExpiryDate.Trim();
                                    txtAddress.Text = objCustomerCardInfo.customerAddress.Trim();
                                    txtZipCode.Text = objCustomerCardInfo.zipCode.Trim();
                                    this.txtCCNo.ReadOnly = true;
                                    this.txtExpDate.ReadOnly = true;
                                }
                                else if (SigPadUtil.DefaultInstance.SigPadCardInfo != null)
                                {
                                    this.txtCCNo.Text = SigPadUtil.DefaultInstance.SigPadCardInfo.CardNo;
                                    this.TrackNoII = SigPadUtil.DefaultInstance.SigPadCardInfo.Track2;
                                    this.txtExpDate.Text = SigPadUtil.DefaultInstance.SigPadCardInfo.ExpireOn;
                                    this.txtCCNo.ReadOnly = true;
                                    this.txtExpDate.ReadOnly = true;
                                }
                            }
                            //Added by SRT till here.
                            //Added By Dharmendra (SRT) on Nov-14-08 to process payment online automatically or to process payment online manually
                            if (Configuration.CPOSSet.ProcessOnLine == true)
                            {
                                btnContinue_Click(Keys.Enter, e);
                            }
                            else if (Configuration.CPOSSet.ProcessOnLine == false)
                            {
                                this.btnContinue.Focus();
                            }
                            //End Added
                        }
                        //Added By Ritesh (SRT)
                        else
                        {
                            this.btnProcessCancel.Enabled = false;
                            this.btnProcessCancel.Text = "ESC to Cancel";
                            this.AllowCancel = true; //Added By Dharmendra (SRT) on 02-Oct-08
                            this.txtCCNo.Text = string.Empty;
                            this.TrackNoII = string.Empty;
                            this.txtExpDate.Text = string.Empty;
                            this.txtAddress.Text = string.Empty;
                            this.txtZipCode.Text = string.Empty;
                        } // End till here.
                    }
                    //Changed By SRT(Gaurav) Date : 19 NOV 2008
                    //Change Datails: Changed position of if condition. This will enable to
                    //bipass card reading while processor type is xlink.
                    //Added By SRT(Gaurav) Date : 18 NOV 2008
                    //Mantis ID: 0000118
                    else if (pmtProcessMethod == "XLINK")
                    {
                        //Added By SRT(Gaurav) Date : 19 NOV 2008
                        txtCCNo.Text = string.Empty;
                        txtExpDate.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        txtZipCode.Text = string.Empty;
                        //End Of Added By SRT(Gaurav)
                        txtCCNo.Enabled = false;
                        txtExpDate.Enabled = false;
                        txtAddress.Enabled = false;
                        txtZipCode.Enabled = false;

                        //End Of Added By SRT(Gaurav)
                    }

                    if (txtCCNo.Text == "" && txtCCNo.Enabled && this.objCustomerCardInfo != null && objCustomerCardInfo.cardNumber.Trim().Length > 0)
                    {
                        txtCCNo.Text = objCustomerCardInfo.cardNumber.Trim();
                        txtExpDate.Text = objCustomerCardInfo.cardExpiryDate.Trim();
                        txtAddress.Text = objCustomerCardInfo.customerAddress.Trim();
                        txtZipCode.Text = objCustomerCardInfo.zipCode.Trim();
                    }
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "frmPOSChangeDue_Load(object sender, System.EventArgs e)");
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
            }
            /*Date 27-jan-2014
             * Modified by Shitaljit
             * For making currency symbol dynamic
             */
            //New Code
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Process CC Load", clsPOSDBConstants.Log_Exiting + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + " #####0.00"));
            logger.Trace("frmPOSChangeDue_Load() - Process CC Load " + clsPOSDBConstants.Log_Exiting + "Amount :" + Amount.ToString(Configuration.CInfo.CurrencySymbol + " #####0.00"));
            //old code
            // POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Process CC Load", clsPOSDBConstants.Log_Exiting + "Amount :" + Amount.ToString("$ #####0.00"));
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

                    //CaputreSignarure();
                    if (CaptureSignature() == true)
                    {
                        this.isCanceled = false;
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
                    if (isCancelTransaction == false)
                    {
                        CaptureSignature();
                    }
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
            //this.sbMain.Panels[0].Text=oCCDbf.GetValString("CURR_STAT");
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

        /* Added by Manoj 8/23/2011 (3 Functions, isF10, AutoPopXcharge, FillXcharge ) */

        public void isF10(bool press, string ccnumber, string ccexp)
        {
            isF10Press = press;
            cardnum = string.Empty;
            cardexp = string.Empty;
            cardnum = ccnumber;
            cardexp = ccexp;
        }

        bool XChargeValError;
        bool XChargeProcess;
        bool XchargeF10Complete;
        int xcount = 0;
        Thread startthread = null;

        private void AutoPopXcharge()
        {
            startthread = new Thread(new ThreadStart(FillXcharge));
            startthread.Start();
        }

        //Manoj 3-17-2015
        private void FillXcharge()
        {
            while (!XchargeF10Complete)
            {
                if (Configuration.CPOSSet.UseSigPad == true && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                {
                    int ihandle = 0;
                    int icount = 0;
                    while (ihandle == 0)
                    {
                        do
                        {
                            ihandle = XchargeWin.FindWindow(null, "Initializing");
                            if (ihandle > 0)
                            {
                                ihandle = 0;
                                icount++;
                                Thread.Sleep(150);
                            }
                            else if (ihandle == 0 && icount > 0)
                            {
                                break;
                            }
                        }
                        while (ihandle == 0);

                        icount = 0;
                        do
                        {
                            ihandle = 0;
                            ihandle = XchargeWin.FindWindow(null, "Validation Error");
                            XChargeValError = ihandle != 0 ? true : false;

                            if (XChargeValError)
                            {
                                XchargeWin.SetForegroundWindow(ihandle);
                                Thread.Sleep(150);
                                System.Windows.Forms.SendKeys.SendWait("{Enter}");
                                Application.DoEvents();
                                if (icount < 1)
                                {
                                    Thread.Sleep(150);
                                    ihandle = 0;
                                    icount++;
                                    XChargeValError = false;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Thread.Sleep(150);
                            }
                        }
                        while (ihandle == 0);
                    }

                    icount = 0;
                    do
                    {
                        ihandle = 0;
                        ihandle = XchargeWin.FindWindow(null, "Initializing");
                        if (ihandle > 0)
                        {
                            ihandle = 0;
                            icount++;
                            Thread.Sleep(150);
                        }
                        else if (ihandle == 0 && icount > 0)
                        {
                            break;
                        }
                    }
                    while (ihandle == 0);

                    do
                    {
                        ihandle = 0;
                        ihandle = XchargeWin.FindWindow(null, "Prime POS CC Processing");
                        if (ihandle > 0)
                        {
                            break;
                        }
                    }
                    while (ihandle == 0);

                    XChargeProcess = ihandle != 0 ? true : false;
                    if (XChargeProcess)
                    {
                        XchargeWin.SetForegroundWindow(ihandle);
                        Thread.Sleep(150);
                        if (this.oTransType == POSTransactionType.Sales)
                        {
                            System.Windows.Forms.SendKeys.SendWait(cardnum + "{Tab}" + cardexp + "{Tab}{Tab}{Tab}{Tab}");
                        }
                        else
                        {
                            System.Windows.Forms.SendKeys.SendWait(cardnum + "{Tab}" + cardexp + "{Tab}");
                        }
                        Thread.Sleep(100);
                        System.Windows.Forms.SendKeys.SendWait("{Enter}");
                        XchargeF10Complete = true;
                    }
                }
                else
                {
                    int ihandle = 0;
                    do
                    {
                        ihandle = XchargeWin.FindWindow(null, "Prime POS CC Processing");
                        XChargeProcess = ihandle != 0 ? true : false;
                        if (XChargeProcess)
                        {
                            XchargeWin.SetForegroundWindow(ihandle);
                            Thread.Sleep(100);
                            System.Windows.Forms.SendKeys.SendWait(cardnum + "{Tab}" + cardexp + "{Tab}{Tab}{Tab}{Tab}{Enter}");
                            Application.DoEvents();
                            XchargeF10Complete = true;
                        }
                    }
                    while (ihandle == 0);
                }
                xcount++;
            }
            if (startthread != null && startthread.IsAlive)
            {
                startthread.Abort();
            }
        }

        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "btnContinue_Click for :" + pmtProcessMethod, clsPOSDBConstants.Log_Entering);
            logger.Trace("btnContinue_Click() - " + clsPOSDBConstants.Log_Entering + "btnContinue_Click for :" + pmtProcessMethod);

            #region CommonForAllCases

            isManualProcess = "0";
            this.AllowCancel = false;
            this.sbMain.Panels[0].Text = "Initializing ....";
            if (pmtProcessMethod == "HPSPAX" || pmtProcessMethod == "VANTIV")// NileshJ - 20-Dec-2018 only for HPSPAX //PRIMEPOS-3156
            {
                if (this.ticketNum == null)//NileshJ -- Added for for HPSPAX Not tested on Other Processor's ( Special case )
                {
                    this.ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();
                }
            }
            else
            {
                this.ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();
            }
            string responseStatus = string.Empty;

            #endregion CommonForAllCases

            switch (pmtProcessMethod)
            {
                //Added By SRT(Gaurav) Date: 18-Nov-2008
                //Mantis ID: 0000112
                //Details : Implementation Of PADSS using XLINK.
                case "XLINK":
                case "WORLDPAY":

                    #region XLINKCase

                    {
                        // Calling the method of PccSvrPayment class
                        PccCardInfo cardInfo = new PccCardInfo();
                        if (isF10Press)
                        {
                            cardInfo = this.CustomerCardInfo;
                        }
                        #region PRIMEPOS-2738 ADDED BY ARVIND 
                        if (Configuration.CSetting.StrictReturn == true && this.oTransType == POSTransactionType.SalesReturn && pmtProcessMethod == "WORLDPAY")
                        {
                            cardInfo.TransactionID = originalTransID.Split(Convert.ToChar(":"))[0];
                            cardInfo.OrderID = originalTransID.Split(Convert.ToChar(":"))[1];
                        }
                        #endregion
                        //Alternate Defination Call
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "FillCardInfo for :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - FillCardInfo for :" + pmtProcessMethod + " Starting");
                        if (pmtProcessMethod.ToUpper() != "WORLDPAY")
                        {
                            FillCardInfo(ref cardInfo, false);
                            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "FillCardInfo for :" + pmtProcessMethod, " Completed");
                            #region PRIMEPOS-2738 ADDED BY ARVIND 
                            if (Configuration.CSetting.StrictReturn == true && this.oTransType == POSTransactionType.SalesReturn && pmtProcessMethod == "XLINK")
                            {
                                if (cardInfo.transAmount.Contains("-"))
                                    cardInfo.transAmount = cardInfo.transAmount.Remove(0, 1);
                                if (originalTransID.Contains("|"))
                                    cardInfo.TransactionID = originalTransID.Split('|')[1];
                            }
                            #endregion
                            logger.Trace("btnContinue_Click() - FillCardInfo for :" + pmtProcessMethod + " Completed");
                        }
                        else
                        {
                            cardInfo.transAmount = this.Amount.ToString();
                            cardInfo.IsFSATransaction = this.isFSATransaction.ToString();
                            cardInfo.PaymentProcessor = pmtProcessMethod;
                            if (this.isFSATransaction)
                            {
                                cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
                                cardInfo.transFSAAmount = this.FSAAmount.ToString();
                            }
                            if (this.Tokenize)
                            {
                                cardInfo.Tokenize = true;
                            }
                        }
                        Application.DoEvents();
                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processing Credit Card for :" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - FillCardInfo for :" + pmtProcessMethod + " Completed");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        if (pmtProcessMethod == "WORLDPAY")
                                        {
                                            if (Configuration.CPOSSet.UseSigPad == true && !isF10Press && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(On); //added by Rohit
                                            }
                                            else if (SigPadUtil.DefaultInstance.isISC)
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(On);
                                            }

                                            if (!PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSale(ticketNum, ref cardInfo))
                                            {
                                                isCanceled = true;
                                                return;
                                            }

                                            if (Configuration.CPOSSet.UseSigPad == true && !isF10Press && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(Off); // added by Rohit
                                            }
                                            else if (SigPadUtil.DefaultInstance.isISC)
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                                            }
                                        }
                                        else
                                        {
                                            //Commented By SRT(Ritesh Parekh) To Test whether multiple Default Instances work.
                                            //PccPaymentSvr.DefaultInstance.PerformCreditSale(ticketNum, ref cardInfo);
                                            // MMSHost.DisplayScreen(ticketNum, "XLINK", "ON");
                                            if (isF10Press && !SigPadUtil.DefaultInstance.isISC)
                                            {
                                                AutoPopXcharge();
                                            }
                                            /* Add by Manoj for Verifone */
                                            if (Configuration.CPOSSet.UseSigPad == true && !isF10Press && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(On); //added by Manoj
                                            }
                                            else if (SigPadUtil.DefaultInstance.isISC)
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(On);
                                            }
                                            /* End Verifone */
                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSale(ticketNum, ref cardInfo);
                                            /* Add by Manoj for Verifone */
                                            if (Configuration.CPOSSet.UseSigPad == true && !isF10Press && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(Off); // added by Manoj
                                            }
                                            else if (SigPadUtil.DefaultInstance.isISC)
                                            {
                                                SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                                            }
                                            /* End Verifone */
                                        }
                                    }
                                    //Modified and Added By SRT(Gaurav) Date: 01-Dec-2008
                                    //Mantis ID: 0000118
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {
                                        //Commented By SRT(Ritesh Parekh) To Test whether multiple Default Instances work.
                                        //PccPaymentSvr.DefaultInstance.PerformCreditSalesReturn(ticketNum, ref cardInfo);
                                        if (isF10Press)
                                        {
                                            AutoPopXcharge();
                                        }
                                        /* Add by Manoj for Verifone */
                                        if (Configuration.CPOSSet.UseSigPad == true && !isF10Press && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(On); //added by Manoj
                                        }
                                        else if (SigPadUtil.DefaultInstance.isISC)
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(On);
                                        }
                                        /* End Verifone */
                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSalesReturn(ticketNum, ref cardInfo);
                                        /* Add by Manoj for Verifone */
                                        if (Configuration.CPOSSet.UseSigPad == true && !isF10Press && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(Off); // added by Manoj
                                        }
                                        else if (SigPadUtil.DefaultInstance.isISC)
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                                        }
                                        /* End Verifone */
                                    }
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit Card for :" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit Card for :" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit Card for :" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Debit Card for :" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        #region Debit Sales
                                        //Changed By SRT(Gaurav) Date : 19 NOV 2008
                                        //Mantis ID: 0000112
                                        //ProcessDebitCard(ticketNum, ref cardInfo);
                                        if (Configuration.CPOSSet.UseSigPad == true && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim()
                                        || SigPadUtil.DefaultInstance.isISC))
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(On); //added by Manoj
                                            ProcessDebitCardForXLink(ticketNum, ref cardInfo);
                                            SigPadUtil.DefaultInstance.XlinkOnOff(Off); // added by Manoj
                                        }
                                        else if ((Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PinPadName.ToUpper().Trim() || Configuration.CPOSSet.PinPadModel.Contains(BlueToothICMP)) && Configuration.CPOSSet.UsePinPad == true) //PRIMEPOS-2503 Jenny Added ICMP device
                                        {
                                            ProcessDebitCardForXLink(ticketNum, ref cardInfo); // added by Manoj for external PinPad
                                        }
                                        else if (pmtProcessMethod.ToUpper() == "WORLDPAY")//&& Configuration.CPOSSet.PinPadModel.ToUpper() == ISC480.ToUpper()// Modified By Rohit Nair For WP
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(On);
                                            if (!PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformDebitSale(ticketNum, ref cardInfo))
                                            {

                                                isCanceled = true;
                                                return;
                                            }
                                            SigPadUtil.DefaultInstance.XlinkOnOff(Off);

                                        }
                                        else
                                        {
                                            POS_Core_UI.Resources.Message.Display("Debit option is currently not supported for this Device.\r\nPlease select another payment option.", "Payment Failure", MessageBoxButtons.OK);
                                            this.isCanceled = true;
                                            this.Close();
                                            return;
                                        }
                                        #endregion
                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {
                                        #region Debit Return
                                        if (pmtProcessMethod.ToUpper() == "WORLDPAY")//&& Configuration.CPOSSet.PinPadModel.ToUpper() == ISC480.ToUpper()// Modified By Rohit Nair For WP
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(On);
                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformDebitReturnWP(ticketNum, ref cardInfo);
                                            SigPadUtil.DefaultInstance.XlinkOnOff(Off);

                                        }
                                        else
                                        {
                                            POS_Core_UI.Resources.Message.Display("Debit option is currently not supported for this Device.\r\nPlease select another option.", "Payment Failure", MessageBoxButtons.OK);
                                            this.isCanceled = true;
                                            this.Close();
                                            return;
                                        }
                                        #endregion
                                    }

                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processing Debit Card for :" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processing Debit Card for :" + pmtProcessMethod + " Completed");
                                    //End Of Changed By SRT(Gaurav)
                                    break;
                                }
                            case (PayTypes.EBT):
                                {
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processing EBT Card for :" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processing EBT Card for :" + pmtProcessMethod + " Starting");
                                    if ((Configuration.CPOSSet.UseSigPad == true && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim()
                                        || SigPadUtil.DefaultInstance.isISC)) || (Configuration.CPOSSet.PinPadModel.Contains(BlueToothICMP))) //PRIMEPOS-2503 Jenny Added ICMP device
                                    {
                                        SigPadUtil.DefaultInstance.XlinkOnOff(On); //added by Manoj
                                        ProcessEBTXLink(ticketNum, ref cardInfo);
                                        SigPadUtil.DefaultInstance.XlinkOnOff(Off); // added by Manoj
                                    }
                                    else if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "VERIFONE 1001/1000" && Configuration.CPOSSet.UsePinPad == true)
                                    { //External PinPad
                                        string pinNumber = string.Empty;
                                        string keySerialNumber = string.Empty;
                                        ProcessEBTXLink(ticketNum, ref cardInfo);
                                        frmPinPad.DefaultInstance.TxnAmount = this.Amount.ToString();
                                        frmPinPad.DefaultInstance.GetPinPadData(cardInfo.cardNumber, out pinNumber, out keySerialNumber);

                                        if (pinNumber == string.Empty || keySerialNumber == string.Empty)
                                        {
                                            if (POS_Core_UI.Resources.Message.Display("Pin No. Or Key Serial No. is blank", "Pin Data Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                                            {
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus = "FAILURE";
                                                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR Pin No. Or Key Serial No. is blank .");
                                                logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR Pin No. Or Key Serial No. is blank .");
                                                this.isCanceled = true;
                                                this.Close();
                                            }
                                        }
                                        else
                                        {
                                            cardInfo.pinNumber = pinNumber + keySerialNumber;

                                            if (this.oTransType == POSTransactionType.Sales)
                                            {
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerfomEBTSale(ticketNum, ref cardInfo);
                                            }
                                            else if (this.oTransType == POSTransactionType.SalesReturn)
                                            {
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformEBTReturn(ticketNum, ref cardInfo);
                                            }
                                        }
                                    }
                                    else if (pmtProcessMethod.ToUpper() == "WORLDPAY")
                                    {
                                        if (this.oTransType == POSTransactionType.Sales)
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(On);
                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerfomEBTSale(ticketNum, ref cardInfo);
                                            SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                                        }
                                        else if (this.oTransType == POSTransactionType.SalesReturn)
                                        {
                                            SigPadUtil.DefaultInstance.XlinkOnOff(On);
                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformEBTReturn(ticketNum, ref cardInfo);
                                            SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                                        }
                                    }

                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processing EBT Card for :" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processing EBT Card for :" + pmtProcessMethod + " Completed");
                                    break;
                                }
                        }
                        //Added By SRT(Ritesh Parekh)
                        //To Remove after confirmation if not necessary
                        SetOldProcessor();

                        if (pmtProcessMethod.ToUpper() == "WORLDPAY")
                        {
                            #region PRIMEPOS-2724 22-Aug-2019 JY Added
                            try
                            {
                                logger.Trace("btnContinue_Click(object sender, System.EventArgs e) - CardType : " + Configuration.convertNullToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult.AccountType));
                            }
                            catch { }
                            #endregion

                            if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult != null)
                            {
                                SubCardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult.AccountType;

                                if (!string.IsNullOrEmpty(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult.SignatureString) && PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult.SignatureString.Length > 0)
                                {
                                    //strSignature = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult.SignatureString;
                                    SigPadUtil.DefaultInstance.CustomerSignature = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult.SignatureString;
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
                            responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                            string responseError = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseError;

                            this.ApprovedPCCCardInfo = cardInfo.Copy();

                            //if(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult.)
                            //Modified By Rohit Nair For FirstMile iNtegration
                            if (!WpResponseFound(responseStatus, responseError, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).WP_TransResult))
                            {
                                isCanceled = true;
                                return;
                            }
                            else
                            {

                                ShoW_WP_Signature();
                            }
                        }
                        else
                        {
                            //End Of Added By  SRT(Ritesh Parekh)
                            ////Added By Dharmendra on March-12-09
                            //if (Configuration.CPOSSet.PaymentProcessor == "XCHARGE" || Configuration.CPOSSet.PaymentProcessor == "XLINK")
                            //{
                            //    cardInfo.cardType = PccPaymentSvr.DefaultInstance.pccRespInfo.CardType;
                            //    cardInfo.cardExpiryDate = PccPaymentSvr.DefaultInstance.pccRespInfo.Expiry;
                            //}
                            ////Added Till Here March-12-09

                            //Commented By SRT(Ritesh Parekh) To Test whether multiple Default Instances work.
                            //SubCardType = PccPaymentSvr.DefaultInstance.pccRespInfo.CardType;
                            #region PRIMEPOS-2724 22-Aug-2019 JY Added
                            try
                            {
                                logger.Trace("btnContinue_Click(object sender, System.EventArgs e) - CardType : " + Configuration.convertNullToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType));
                            }
                            catch { }
                            #endregion
                            if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                            {
                                //NileshJ - TicketNum 
                                if ((PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.TrouTd == this.ticketNum) && (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result == "SUCCESS"))
                                {
                                    this.ticketNum = null;
                                }
                                // Till - NileshJ
                                SubCardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType;
                            }
                            else
                            {
                                SubCardType = string.Empty;
                            }
                            this.ApprovedPCCCardInfo = cardInfo.Copy();

                            //Commented By SRT(Ritesh Parekh) To Test whether multiple Default Instances work.
                            //responseStatus = PccPaymentSvr.DefaultInstance.ResponseStatus;
                            //CCResponseFound(responseStatus, PccPaymentSvr.DefaultInstance.pccRespInfo);
                            responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                            //responseStatus += "\n"+PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseError;
                            CCResponseFound(responseStatus, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo);
                        }
                        this.btnContinue.Enabled = false;
                        this.btnProcessManual.Enabled = false;
                        this.txtCCNo.Enabled = false;
                        this.txtExpDate.Enabled = false;
                        cardInfo.ClearPccCardInfo();
                        SdeleteResultFile();
                        cardInfo = null;

                        break;
                    }

                #endregion XLINKCase

                case "PCCHARGE":
                case "XCHARGE":
                    #region PCCHARGE&XCHARGE

                    {
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign  Card for :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Processign  Card for :" + pmtProcessMethod + " Starting");
                        if (!CheckAVSFields())
                            return;
                        // Calling the method of PccSvrPayment class
                        int errorCode = 0;
                        PccCardInfo cardInfo = new PccCardInfo();
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Fill  Card info for :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Fill  Card info for :" + pmtProcessMethod + " Starting");
                        FillCardInfo(ref cardInfo, true);
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Fill  Card info for :" + pmtProcessMethod, " Completed");
                        logger.Trace("btnContinue_Click() - Fill  Card info for :" + pmtProcessMethod + " Completed");

                        if (cardInfo.trackII.Equals(string.Empty))
                        {
                            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Fill  Card info for :" + pmtProcessMethod, " ERROR Track II data is missing");
                            logger.Trace("btnContinue_Click() - Fill  Card info for :" + pmtProcessMethod + " ERROR Track II data is missing");
                            if (POS_Core_UI.Resources.Message.Display("Track II Data missing, Continue with Payment?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                this.isCanceled = true;
                                this.Close();
                                return;
                            }
                        }
                        Application.DoEvents();
                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    //Added By SRT(Gaurav) Date: 01-Dec-2008
                                    //Mantis ID: 0000118
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit Card :" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Credit Card :" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        //Commented By SRT(Ritesh Parekh) To Test whether multiple Default Instances work.
                                        //PccPaymentSvr.DefaultInstance.PerformCreditSale(ticketNum, ref cardInfo);
                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSale(ticketNum, ref cardInfo);
                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {
                                        //Commented By SRT(Ritesh Parekh) To Test whether multiple Default Instances work.
                                        //PccPaymentSvr.DefaultInstance.PerformCreditSalesReturn(ticketNum, ref cardInfo);
                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSalesReturn(ticketNum, ref cardInfo);
                                    }
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit Card :" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit Card :" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit Card :" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Debit Card :" + pmtProcessMethod + " Starting");
                                    //Modified by Dharmendra(SRT) on Nov-14-08 for removing Pin Pad configuration Setting & reading the same from POSSET
                                    if (Configuration.CPOSSet.UsePinPad == false)
                                    {
                                        POS_Core_UI.Resources.Message.Display("Debit option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        this.Close();
                                        return;
                                    }

                                    frmPinPad.DefaultInstance.TxnAmount = this.Amount.ToString();
                                    if (frmPinPad.DefaultInstance.PinPadInitialized == false)
                                    {
                                        errorCode = frmPinPad.DefaultInstance.InitializePinPad();
                                        if (errorCode == 0)
                                        {
                                            ProcessDebitCard(ticketNum, ref cardInfo);
                                            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit Card :" + pmtProcessMethod, " Success");
                                            logger.Trace("btnContinue_Click() - Processign Debit Card :" + pmtProcessMethod + " Success");
                                        }
                                        else
                                        {
                                            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit Card :" + pmtProcessMethod, " Payment Failure");
                                            logger.Trace("btnContinue_Click() - Processign Debit Card :" + pmtProcessMethod + " Payment Failure");
                                            //Changed by SRT
                                            POS_Core_UI.Resources.Message.Display("Pinpad is not Initialized.\r\nDebit card is not processed.", "Payment Failure", MessageBoxButtons.OK);
                                            this.isCanceled = true;
                                            this.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ProcessDebitCard(ticketNum, ref cardInfo);
                                    }
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit Card :" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Debit Card :" + pmtProcessMethod + " Completed");
                                    break;
                                }
                        }
                        //Added By SRT(Ritesh Parekh)
                        //To Remove after confirmation if not necessary
                        SetOldProcessor();
                        //End Of Added By  SRT(Ritesh Parekh)
                        this.ApprovedPCCCardInfo = cardInfo.Copy();

                        //Commented By SRT(Ritesh Parekh) To Test whether multiple Default Instances work.
                        //responseStatus = PccPaymentSvr.DefaultInstance.ResponseStatus;
                        //CCResponseFound(responseStatus, PccPaymentSvr.DefaultInstance.pccRespInfo);
                        responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                        CCResponseFound(responseStatus, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo);

                        this.btnContinue.Enabled = false;
                        this.btnProcessManual.Enabled = false;
                        this.txtCCNo.Enabled = false;
                        this.txtExpDate.Enabled = false;
                        cardInfo.ClearPccCardInfo();
                        cardInfo = null;

                        if (Configuration.CPOSSet.UseSigPad == true)
                        {
                            SigPadUtil.DefaultInstance.SigPadCardInfo = null;
                        }
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit Card :" + pmtProcessMethod, " Payment Failure");
                        logger.Trace("btnContinue_Click() - Processign Debit Card :" + pmtProcessMethod + " Payment Failure");
                        break;
                    }
                case "HPS":

                    #region HPS Direction Integration by Manoj

                    {
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign  Card :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Processign  Card :" + pmtProcessMethod + " Starting");
                        if (!CheckAVSFields())
                            return;

                        int errorCode = 0;
                        PccCardInfo cardInfo = new PccCardInfo();
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Starting");
                        FillCardInfo(ref cardInfo, true);
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Completed");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Completed");
                        if (txtExpDate.Text.Trim().Length < 4)
                        {
                            POS_Core_UI.Resources.Message.Display("Invalid expiration date, Enter in the format of MMYY", "Expiration Date");
                            txtExpDate.Focus();
                            return;
                        }
                        if (isF10Press)
                        {
                            cardInfo.IsCardPresent = "0";
                        }
                        #region PRIMEPOS-2738 ADDED BY ARVIND 
                        if (Configuration.CSetting.StrictReturn == true && this.oTransType == POSTransactionType.SalesReturn)
                        {
                            if (cardInfo.transAmount.Contains("-"))
                            {
                                cardInfo.transAmount = cardInfo.transAmount.Remove(0, 1);
                            }
                            cardInfo.TransactionID = originalTransID;
                        }
                        #endregion
                        if (cardInfo.trackII.Equals(string.Empty) && !isF10Press)
                        {
                            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " ERROR Tranck II details missing.");
                            logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " ERROR Tranck II details missing.");
                            if (POS_Core_UI.Resources.Message.Display("Track II Data missing, Continue with Payment?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                this.isCanceled = true;
                                this.Close();
                                return;
                            }
                        }
                        Application.DoEvents();
                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSale(ticketNum, ref cardInfo);
                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {
                                        //cardInfo.tRoutId =
                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSalesReturn(ticketNum, ref cardInfo);
                                    }
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (Configuration.CPOSSet.UseSigPad == false && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != "VERIFONE 1001/1000" && Configuration.CPOSSet.UsePinPad != true)
                                    {
                                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        POS_Core_UI.Resources.Message.Display("Debit option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        this.Close();
                                        return;
                                    }
                                    else if (cardInfo.trackII.Equals(string.Empty))
                                    {
                                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        POS_Core_UI.Resources.Message.Display("Please select another payment method. \nAll Debit Card must be swiped. \nManual entry of a Debit Card is not allowed.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        this.Close();
                                        return;
                                    }
                                    else if ((Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim()) || (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == MXDevice.ToUpper().Trim())
                                        && SigPadUtil.DefaultInstance.isConnected())
                                    { //MX870
                                        ProcessDebitCardHPS(ticketNum, ref cardInfo);
                                    }
                                    else if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "VERIFONE 1001/1000" && Configuration.CPOSSet.UsePinPad == true)
                                    { // External PinPad
                                        frmPinPad.DefaultInstance.TxnAmount = this.Amount.ToString();
                                        ProcessDebitCard(ticketNum, ref cardInfo);
                                        frmPinPad.DefaultInstance.TxnAmount = "";
                                    }
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit Card  for:" + pmtProcessMethod, "Completed");
                                    logger.Trace("btnContinue_Click() - Processign Debit Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                            case (PayTypes.EBT):
                                {
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Starting.");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Starting.");
                                    if (Configuration.CPOSSet.UseSigPad == false && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != "VERIFONE 1001/1000" && Configuration.CPOSSet.UsePinPad != true)
                                    {
                                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR EBT is not supported.");
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        POS_Core_UI.Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        this.Close();
                                        return;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isConnected() && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() || (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == MXDevice.ToUpper().Trim())))
                                    { //MX870
                                        SigPadUtil.DefaultInstance.CaptureDebitPinBlock(cardInfo.cardNumber);
                                        if (this.oTransType == POSTransactionType.Sales)
                                        {
                                            if (SigPadUtil.DefaultInstance.PINBLOCK != string.Empty)
                                            {
                                                cardInfo.pinNumber = SigPadUtil.DefaultInstance.PINBLOCK.ToString();
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerfomEBTSale(ticketNum, ref cardInfo);
                                            }
                                            else
                                            {
                                                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR PINBLOCK is empty .");
                                                logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR PINBLOCK is empty .");
                                                return;
                                            }
                                        }
                                        else if (this.oTransType == POSTransactionType.SalesReturn)
                                        {
                                            if (SigPadUtil.DefaultInstance.PINBLOCK != string.Empty)
                                            {
                                                cardInfo.pinNumber = SigPadUtil.DefaultInstance.PINBLOCK.ToString();
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformEBTReturn(ticketNum, ref cardInfo);
                                            }
                                            else
                                            {
                                                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR PINBLOCK is empty .");
                                                logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR PINBLOCK is empty .");
                                                return;
                                            }
                                        }
                                    }
                                    else if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "VERIFONE 1001/1000" && Configuration.CPOSSet.UsePinPad == true)
                                    { //External PinPad
                                        string pinNumber = string.Empty;
                                        string keySerialNumber = string.Empty;
                                        frmPinPad.DefaultInstance.TxnAmount = this.Amount.ToString();
                                        frmPinPad.DefaultInstance.GetPinPadData(cardInfo.cardNumber, out pinNumber, out keySerialNumber);

                                        if (pinNumber == string.Empty || keySerialNumber == string.Empty)
                                        {
                                            if (POS_Core_UI.Resources.Message.Display("Pin No. Or Key Serial No. is blank", "Pin Data Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                                            {
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus = "FAILURE";
                                                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR Pin No. Or Key Serial No. is blank .");
                                                logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR Pin No. Or Key Serial No. is blank .");
                                                this.isCanceled = true;
                                                this.Close();
                                            }
                                        }
                                        else
                                        {
                                            cardInfo.pinNumber = pinNumber + keySerialNumber;

                                            if (this.oTransType == POSTransactionType.Sales)
                                            {
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerfomEBTSale(ticketNum, ref cardInfo);
                                            }
                                            else if (this.oTransType == POSTransactionType.SalesReturn)
                                            {
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformEBTReturn(ticketNum, ref cardInfo);
                                            }
                                        }
                                    }
                                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Completed");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                        }
                        SetOldProcessor();
                        this.ApprovedPCCCardInfo = cardInfo.Copy();
                        responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                        CCResponseFound(responseStatus, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo);

                        this.btnContinue.Enabled = false;
                        this.btnProcessManual.Enabled = false;
                        this.txtCCNo.Enabled = false;
                        this.txtExpDate.Enabled = false;
                        cardInfo.ClearPccCardInfo();
                        cardInfo = null;

                        if (Configuration.CPOSSet.UseSigPad == true)
                        {
                            SigPadUtil.DefaultInstance.SigPadCardInfo = null;
                        }
                        //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign   Card  for:" + pmtProcessMethod, "Completed");
                        logger.Trace("btnContinue_Click() - Processign   Card  for:" + pmtProcessMethod + "Completed");
                        break; // End HPS 5/22/2012
                    }

                #endregion HPS Direction Integration by Manoj

                #endregion PCCHARGE&XCHARGE
                case "HPSPAX":

                    #region HPSPAX Integration by Suraj
                    //PRIMEPOS-2528 (Suraj) 1-june-18 Added HPSPAX PaymentProcessor

                    {
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign  Card :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Processign  Card :" + pmtProcessMethod + " Starting");



                        frmWaitScreen ofrmWait = new frmWaitScreen(true, "Please wait...", "Processing Payment Online"); // NileshJ - Hide Cancel Button and Show Message To Aborted   for Payment Popup window 
                        ofrmWait.Show();
                        if (!CheckAVSFields())
                            return;

                        int errorCode = 0;
                        PccCardInfo cardInfo = new PccCardInfo();
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Starting");
                        //FillCardInfo(ref cardInfo, true);
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Completed");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Completed");
                        if (isF10Press)
                        {
                            //cardInfo.IsCardPresent = "0";
                            cardInfo = this.CustomerCardInfo;
                        }
                        cardInfo.transAmount = this.Amount.ToString();
                        #region PRIMEPOS-2854 Nilesh / SAJID LOCAL DETAILS REPORT
                        int transactionNumber = 0;
                        int.TryParse(sender.ToString(), out transactionNumber);
                        cardInfo.TransactionID = transactionNumber.ToString();
                        if (transactionNumber > 0)
                        {
                            cardInfo.isTransactionRecover = true;
                        }
                        else
                        {
                            cardInfo.isTransactionRecover = false;
                        }
                        #endregion
                        #region PRIMEPOS-2738 ADDED BY ARVIND
                        if (this.oTransType == POSTransactionType.SalesReturn)// Nilesh,Sajid - add if condition PRIMEPOS-2854
                        {
                            cardInfo.HrefNumber = hrefNumber;
                            cardInfo.TransactionID = originalTransID;
                            cardInfo.CardType = TransTypeCode;//PRIMEPOS-3087
                        }
                        #endregion
                        cardInfo.IsFSATransaction = this.isFSATransaction.ToString();
                        cardInfo.PaymentProcessor = pmtProcessMethod;
                        if (this.isFSATransaction)
                        {
                            cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
                            cardInfo.transFSAAmount = this.FSAAmount.ToString();
                        }

                        if (this.Tokenize)
                        {
                            cardInfo.Tokenize = true;
                        }
                        //PRIMEPOS-3047
                        if (isAllowDuplicate)
                        {
                            cardInfo.isAllowDuplicate = true;
                        }

                        #region PRIMEPOS-2761
                        cardInfo.Transtype = Enum.GetName(typeof(POSTransactionType), this.oTransType);
                        #endregion

                        #region Added by Arvind PRIMEPOS-2734
                        while (SigPadUtil.DefaultInstance.PD.IsStillWrite == true)
                        {
                            SigPadUtil.DefaultInstance.ClearDeviceQueue();  //Temp 07May2020 added for Nilesh (PRIMEPOS-2734)
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }
                        #endregion
                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.CreditCard);
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {

                                        Thread threadSalesReturn = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSalesReturn.Start();
                                        while (threadSalesReturn.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.CreditCard);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE.ToUpper().Trim()
                                       || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_ARIES8.ToUpper().Trim()
                                       || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_A920.ToUpper().Trim())) //PRIMEPOS-2952 //PRIMEPOS-3146
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.DebitCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.DebitCard);
                                                    break;
                                                }
                                            }
                                        }
                                        frmPinPad.DefaultInstance.TxnAmount = "";
                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("DebitButton_Click() - Processing Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported for HPSPAX.");
                                        POS_Core_UI.Resources.Message.Display("Debit return not allowed for HPSPAX", "Debit Card Return", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    else
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        POS_Core_UI.Resources.Message.Display("Please select another payment method. \nAll Debit Card must be swiped. \nManual entry of a Debit Card is not allowed.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    logger.Trace("btnContinue_Click() - Processign Debit Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                            case (PayTypes.EBT):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Starting.");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Starting.");
                                    if (Configuration.CPOSSet.UseSigPad == false
                                        && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != DeviceName.ToUpper().Trim()
                                        && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != "VERIFONE 1001/1000"
                                        && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != PAX_DEVICE.ToUpper().Trim()
                                        && Configuration.CPOSSet.UsePinPad != true
                                        && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != PAX_DEVICE_ARIES8.ToUpper().Trim()
                                        && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != PAX_DEVICE_A920.ToUpper().Trim()) //PRIMEPOS-2952 //PRIMEPOS-3146
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR EBT is not supported.");
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        POS_Core_UI.Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isConnected()
                                        && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE.ToUpper().Trim()
                                                || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_ARIES8.ToUpper().Trim()
                                                || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PAX_DEVICE_A920.ToUpper().Trim()))//PRIMEPOS-3110//PRIMEPOS-3146
                                    {
                                        if (this.oTransType == POSTransactionType.Sales)
                                        {

                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.EBT);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (this.oTransType == POSTransactionType.SalesReturn)
                                        {
                                            //Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));  //Commented For Aries8
                                            //threadSale.Start();
                                            //while (threadSale.IsAlive)
                                            //{
                                            //    Application.DoEvents();
                                            //    if (ofrmWait.IsDisposed)
                                            //    {
                                            //        if (ofrmWait.DialogResult == DialogResult.Cancel)
                                            //        {
                                            //            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.EBT);
                                            //            break;
                                            //        }
                                            //    }
                                            //}
                                            logger.Trace("EBTButton_Click - Processing EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported for HPSPAX."); //PRIMEPOS-2952
                                            POS_Core_UI.Resources.Message.Display("EBT Return not allowed for HPSPAX", "EBT Return", MessageBoxButtons.OK);
                                            this.isCanceled = true;
                                            ofrmWait.Close();
                                            this.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        POS_Core_UI.Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Completed");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                        }
                        SetOldProcessor();

                        #region PRIMEPOS-2724 22-Aug-2019 JY Added
                        try
                        {
                            logger.Trace("btnContinue_Click(object sender, System.EventArgs e) - CardType : " + Configuration.convertNullToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType));
                        }
                        catch { }
                        #endregion

                        if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                        {
                            //NileshJ - TicketNum - 20-Dec-2018
                            if ((PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.TrouTd == this.ticketNum) && (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result == "SUCCESS"))
                            {
                                this.ticketNum = null;
                            }
                            // Till - NileshJ
                            SubCardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType;
                            if (oPayTpes == PayTypes.CreditCard && !string.IsNullOrEmpty(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PAX_SigString) && PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PAX_SigString.Length > 0)
                            {
                                SigPadUtil.DefaultInstance.CustomerSignature = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PAX_SigString;
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


                        responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                        string responseError = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseError;

                        this.ApprovedPCCCardInfo = cardInfo.Copy();

                        ofrmWait.Close();


                        if (!PAXResponseFound(responseStatus, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo))
                        {
                            #region PRIMEPOS-3047
                            if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription.ToUpper() == "DUP TRANSACTION")
                            {
                                frmForceTransaction ofrmFT = new frmForceTransaction();
                                ofrmFT.ShowDialog();

                                if (ofrmFT.isAllowDuplicate)
                                {
                                    this.isAllowDuplicate = true;
                                    btnContinue_Click(sender, e);
                                }
                                else
                                {
                                    isCanceled = true;
                                    return;
                                }
                            }
                            else
                            {
                                isCanceled = true;
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            ShoW_PAX_Signature();
                        }
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign   Card  for:" + pmtProcessMethod, "Completed");
                        logger.Trace("btnContinue_Click() - Processign   Card  for:" + pmtProcessMethod + "Completed");
                        break; // End HPSPAX
                    }
                //END
                #endregion PAX Integration by Suraj

                case "EVERTEC":
                    #region EVERTEC Integration by Arvind
                    {
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign  Card :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Processign  Card :" + pmtProcessMethod + " Starting");

                        PccCardInfo cardInfo = new PccCardInfo();
                        #region PRIMEPOS-2805 EVERTEC
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                        {
                            if (Configuration.CSetting.PromptForEvertecManual && oPayTpes != PayTypes.EBT && oPayTpes != PayTypes.DebitCard && !this.IsFondoUnica)//PRIMEPOS-2664
                            {
                                frmEvertecManual oEvertecManual = new frmEvertecManual();
                                oEvertecManual.ShowDialog();
                                if (oEvertecManual.isManual == true)
                                {
                                    cardInfo.isManual = true;
                                }
                                else if (oEvertecManual.isCancel)
                                {
                                    cardInfo.isManual = false;
                                    isCanceled = true;
                                    return;
                                }
                                else
                                {
                                    cardInfo.isManual = false;
                                }
                            }
                        }
                        #endregion

                        if ((oPayTpes == PayTypes.EBT && this.oTransType == POSTransactionType.Sales) || this.IsFondoUnica)
                        {
                            if (CashBackAmount > 0 || this.IsFondoUnica)//2664
                            {
                                cardInfo.isFood = false;
                            }
                            else
                            {
                                frmEvertecEBT ofrmEvertecEbt = new frmEvertecEBT();//PRIMEPOS-2664
                                ofrmEvertecEbt.ShowDialog();
                                cardInfo.isFood = ofrmEvertecEbt.IsFood;
                            }
                        }

                        cardInfo.IsFondoUnica = this.IsFondoUnica;//2664

                        frmWaitScreen ofrmWait = new frmWaitScreen(true, "Please wait...", "Processing Payment Online");
                        ofrmWait.Show();
                        if (!CheckAVSFields())
                            return;

                        int errorCode = 0;
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Starting");
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Filling  Card info for:" + pmtProcessMethod, " Completed");
                        logger.Trace("btnContinue_Click() - Filling  Card info for:" + pmtProcessMethod + " Completed");

                        if (isF10Press)
                        {
                            cardInfo = this.CustomerCardInfo;
                        }

                        cardInfo.transAmount = this.Amount.ToString();
                        cardInfo.IsFSATransaction = this.isFSATransaction.ToString();
                        cardInfo.PaymentProcessor = pmtProcessMethod;

                        //2664
                        cardInfo.EvertecTaxDetails = this.EvertecTaxDetails;
                        //

                        #region PRIMEPOS-2784 EVERTEC
                        if (isAllowDuplicate)
                        {
                            cardInfo.isAllowDuplicate = true;
                        }
                        #endregion

                        if (this.isFSATransaction)
                        {
                            cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
                            cardInfo.transFSAAmount = this.FSAAmount.ToString();
                        }

                        if (this.Tokenize)
                        {
                            cardInfo.Tokenize = true;
                        }


                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.CreditCard);
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {

                                        Thread threadSalesReturn = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSalesReturn.Start();
                                        while (threadSalesReturn.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.CreditCard);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == Evertec.ToUpper().Trim())
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.DebitCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.DebitCard);
                                                    break;
                                                }
                                            }
                                        }
                                        frmPinPad.DefaultInstance.TxnAmount = "";
                                    }
                                    else
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        Resources.Message.Display("Please select another payment method. \nAll Debit Card must be swiped. \nManual entry of a Debit Card is not allowed.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    logger.Trace("btnContinue_Click() - Processign Debit Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                            case (PayTypes.EBT):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Starting.");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Starting.");
                                    if (Configuration.CPOSSet.UseSigPad == false && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != "VERIFONE 1001/1000" && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != Evertec.ToUpper().Trim() && Configuration.CPOSSet.UsePinPad != true)
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR EBT is not supported.");
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isConnected() && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == Evertec.ToUpper().Trim())
                                    {
                                        if (this.oTransType == POSTransactionType.Sales)
                                        {

                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.EBT);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (this.oTransType == POSTransactionType.SalesReturn)
                                        {
                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransPAX(PayTypes.EBT);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Completed");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                        }
                        SetOldProcessor();

                        if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                        {

                            if ((PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.TrouTd == this.ticketNum) && (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result == "SUCCESS"))
                            {
                                this.ticketNum = null;
                            }
                            SubCardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType;
                            if (oPayTpes == PayTypes.CreditCard && !string.IsNullOrEmpty(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).EVERTEC_SigString) && PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).EVERTEC_SigString.Length > 0)
                            {
                                SigPadUtil.DefaultInstance.CustomerSignature = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).EVERTEC_SigString;
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


                        responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                        string responseError = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseError;

                        this.ApprovedPCCCardInfo = cardInfo.Copy();

                        ofrmWait.Close();


                        if (!EVERTECResponseFound(responseStatus, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo))
                        {
                            if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription.ToUpper() != "SUCCESS" && PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription.ToUpper() == "DUP TRANSACTION")
                            {
                                frmForceTransaction ofrmFT = new frmForceTransaction();
                                ofrmFT.ShowDialog();

                                if (ofrmFT.isAllowDuplicate)
                                {
                                    this.isAllowDuplicate = true;
                                    btnContinue_Click(sender, e);
                                }
                                else
                                {
                                    isCanceled = true;
                                    return;
                                }
                            }
                            else
                            {
                                isCanceled = true;
                                return;
                            }
                        }
                        else
                        {
                            ShoW_EVERTEC_Signature();
                        }
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign   Card  for:" + pmtProcessMethod, "Completed");
                        logger.Trace("btnContinue_Click() - Processign   Card  for:" + pmtProcessMethod + "Completed");
                        break; // End EVERTEC
                    }

                #endregion EVERTEC Integration by Arvind 
                case "VANTIV":

                    #region VANTIV Integration PRIMEPOS-2636

                    {
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign  Card :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Processign  Card :" + pmtProcessMethod + " Starting");



                        frmWaitScreen ofrmWait = new frmWaitScreen(true, "Please wait...", "Processing Payment Online");
                        ofrmWait.Show();
                        if (!CheckAVSFields())
                            return;

                        int errorCode = 0;

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

                        #region PRIMEPOS-3156
                        int transactionNumber = 0;
                        int.TryParse(sender.ToString(), out transactionNumber);
                        cardInfo.TransactionID = transactionNumber.ToString();
                        if (transactionNumber > 0)
                        {
                            cardInfo.isTransactionRecover = true;
                        }
                        else
                        {
                            cardInfo.isTransactionRecover = false;
                        }
                        #endregion

                        #region PRIMEPOS-3156
                        if (this.oTransType == POSTransactionType.SalesReturn)
                        {
                            cardInfo.HrefNumber = hrefNumber;
                            cardInfo.TransactionID = originalTransID;
                            cardInfo.CardType = TransTypeCode;
                            cardInfo.nbsSaleType = NBSPaymentType;
                            cardInfo.returnTransType = ReturnTransType; //PRIMEPOS-3521 //PRIMEPOS-3522 //PRIMEPOS-3504
                        }
                        #endregion

                        if (this.isFSATransaction && !isF10Press) //PRIMERX-3553
                        {
                            if (DialogResult.Yes == Resources.Message.Display("Do you wish to use FSA/HSA card to process this transaction?", "FSA CARD", MessageBoxButtons.YesNo))
                            {
                                this.isFSATransaction = true;
                                //cardInfo.transAmount = this.FSARxAmount.ToString();
                            }
                            else
                            {
                                this.isFSATransaction = false;
                            }
                            cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
                            cardInfo.transFSAAmount = this.FSAAmount.ToString();
                        }

                        if (this.Tokenize) //PRIMEPOS-3528 //PRIMEPOS-3504
                        {
                            cardInfo.Tokenize = true;
                        }

                        while (SigPadUtil.DefaultInstance.VFD.IsStillWrite == true)
                        {
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }
                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        iSNBSExOccurred = false;
                                        if (isF10Press) //PRIMEPOS-3528 //PRIMEPOS-3504
                                        {
                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if(this.isFSATransaction) //PRIMEPOS-3545
                                            {
                                                Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                                threadSale.Start();
                                                while (threadSale.IsAlive)
                                                {
                                                    Application.DoEvents();
                                                    if (ofrmWait.IsDisposed)
                                                    {
                                                        if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                        {
                                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {

                                                isNBSPayment = false;
                                                cardInfo.preReadType = "SALE"; //PRIMEPOS-3522
                                                Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreRead), ticketNum, ref cardInfo));
                                                threadSale.Start();
                                                while (threadSale.IsAlive)
                                                {
                                                    Application.DoEvents();
                                                    if (ofrmWait.IsDisposed)
                                                    {
                                                        if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                        {
                                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                            break;
                                                        }
                                                    }
                                                }

                                                #region PRIMEPOS-3517
                                                if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                                                {
                                                    if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "PREREADSUCCESSFUL")
                                                    {
                                                        PaymentType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PaymentType;
                                                        PreReadId = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PreReadId;
                                                        AccountNo = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountNo;
                                                        BinValue = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.BinValue;
                                                        AccountHolderName = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountHolderName;
                                                        if (!string.IsNullOrWhiteSpace(AccountNo) && AccountNo.Trim().Length >= 4)
                                                        {
                                                            AccountNoWithoutMask = AccountNo.Trim().Substring(AccountNo.Trim().Length - 4, 4);
                                                        }

                                                        //PRIMEPOS-3529 BIN VERIFICATION
                                                        iSBinVerified = false;
                                                        List<string> NBSBINList = new List<string>(Configuration.CSetting.NBSBins.Split(','));

                                                        if (NBSBINList != null && NBSBINList.Count > 0)
                                                        {
                                                            foreach (string bin in NBSBINList)
                                                            {
                                                                if (BinValue == Convert.ToString(bin).Trim())
                                                                {
                                                                    iSBinVerified = true;
                                                                }
                                                            }
                                                        }

                                                        if(iSBinVerified)
                                                        {
                                                            if (Configuration.CSetting.NBSEnable)
                                                            {
                                                                if (this.IsNBSTransDoneOnce)
                                                                {
                                                                    iSNBSExOccurred = true;
                                                                    Resources.Message.Display($"Only one NB card payment is allowed in a POS transaction.\nThe current NB Card payment must be voided before another can be attempted.", "Payment Failure", MessageBoxButtons.OK);
                                                                    logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- NBS card found and the pharmacy has not configurred for NB.");
                                                                }
                                                                else
                                                                {
                                                                    #region PRIMEPOS-3517 Need to check token is expired or not
                                                                    if (Configuration.CSetting.NBSTokenExpriresAt != null)
                                                                    {
                                                                        TimeSpan difference = Configuration.CSetting.NBSTokenExpriresAt - DateTime.UtcNow;

                                                                        if (difference.TotalMinutes < 10)
                                                                        {
                                                                            NBSProcessor nbsProcessors = new NBSProcessor(Configuration.CSetting.NBSUrl, "");
                                                                            TokenResponseData nbsTokenData = new TokenResponseData();
                                                                            nbsTokenData = nbsProcessors.GetToken(Configuration.CInfo.PHNPINO);
                                                                            if (nbsTokenData != null)
                                                                            {
                                                                                Configuration.CSetting.NBSToken = nbsTokenData.AccessToken;
                                                                                Configuration.CSetting.NBSTokenExpriresAt = Convert.ToDateTime(nbsTokenData.ExpiresIn);
                                                                            }
                                                                        }
                                                                    }
                                                                    #endregion
                                                                    if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSToken) && !string.IsNullOrEmpty(Configuration.CSetting.NBSEntityID) && !string.IsNullOrEmpty(Configuration.CSetting.NBSStoreID))
                                                                    {
                                                                        NBSProcessor nbsProcessor = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);

                                                                        //PRIMEPOS-3504 if line items does not contain any items or conatains character values
                                                                        if (lineItemsdata != null && lineItemsdata.Count == 0) //PRIMEPOS-3407
                                                                        {
                                                                            Resources.Message.Display("The line items are not eligible for NB.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                            iSNBSExOccurred = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            #region CREATE UID FOR NBS
                                                                            if (!string.IsNullOrEmpty(AccountNoWithoutMask) && !string.IsNullOrEmpty(BinValue) && !string.IsNullOrEmpty(AccountHolderName))
                                                                            {
                                                                                //123456:SMITH/JOHN:3456
                                                                                string finalinput = BinValue + ":" + AccountHolderName + ":" + AccountNoWithoutMask;
                                                                                string uidNBS = nbsProcessor.ComputeSHA256Hash(finalinput);
                                                                                AnalyzeMerchant merchant = new AnalyzeMerchant
                                                                                {
                                                                                    EntityId = Configuration.CSetting.NBSEntityID,
                                                                                    StoreId = Configuration.CSetting.NBSStoreID,
                                                                                    TerminalId = Configuration.StationID
                                                                                };
                                                                                AnalyseData analyseData = nbsProcessor.AnalyzeRequest(lineItemsdata, merchant, uidNBS, ticketNum, false);
                                                                                if (analyseData != null && analyseData.Response != null)
                                                                                {
                                                                                    if (!string.IsNullOrWhiteSpace(analyseData.Response.Code) && analyseData.Response.Code == "000" || analyseData.Response.Code == "100")
                                                                                    {
                                                                                        string nbsBenifitsID = analyseData.Response.NationsBenefitsTransactionId;
                                                                                        double autAmount = analyseData.Response.AuthorizedTransactionAmount;
                                                                                        if (DialogResult.Yes == Resources.Message.Display($"The NB approved amount is $ {autAmount.ToString("0.00")} , Do you want to proceed? ", "NB TRANSACTION", MessageBoxButtons.YesNo))
                                                                                        {
                                                                                            #region NBS-SALE-Transaction
                                                                                            cardInfo.preReadId = PreReadId;
                                                                                            cardInfo.isNBSTransaction = true;
                                                                                            cardInfo.transAmount = Convert.ToString(autAmount);
                                                                                            Thread threadSaleRedeem = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreReadSale), ticketNum, ref cardInfo));
                                                                                            threadSaleRedeem.Start();
                                                                                            while (threadSaleRedeem.IsAlive)
                                                                                            {
                                                                                                Application.DoEvents();
                                                                                                if (ofrmWait.IsDisposed)
                                                                                                {
                                                                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                                                    {
                                                                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                                                                        break;
                                                                                                    }
                                                                                                }
                                                                                            }

                                                                                            if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "APPROVED" ||
                                                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "SUCCESS" ||
                                                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "1" ||
                                                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "8" ||
                                                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("Transaction Status :Approved") ||
                                                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("PartialApproved"))
                                                                                            {
                                                                                                NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;
                                                                                                #region NBS-REDEEM-CALL
                                                                                                RedeemData redeemData = nbsProcessor.RedeemTransaction(nbsBenifitsID, Convert.ToString(autAmount));
                                                                                                if (redeemData?.Response?.Code == "000")
                                                                                                {
                                                                                                    NBSTransId = redeemData.Response.NationsBenefitsTransactionId;
                                                                                                    NBSUid = uidNBS;
                                                                                                    isNBSPayment = true; //PRIMEPOS-3504
                                                                                                    logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBS Transaction Redeem successful");
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Redeem Transaction : {redeemData?.Response?.Code}.");
                                                                                                }
                                                                                                #endregion
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                iSNBSExOccurred = true;
                                                                                                Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                                                logger.Warn("btnContinue_Click() - Processing PreRead Sale is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                                                                #region NBS-REVERSAL-CALL
                                                                                                ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                                                                if (reversalData?.Response?.Code == "000")
                                                                                                {
                                                                                                    logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is Reversal successful");
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                                                                }
                                                                                                #endregion
                                                                                            }
                                                                                            #endregion
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            iSNBSExOccurred = true;
                                                                                            logger.Info("btnContinue_Click() - Processign NBS Card info for:- The user does not want to process the amount approved by NBS");
                                                                                            #region NBS-REVERSAL-CALL
                                                                                            ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                                                            if (reversalData?.Response?.Code == "000")
                                                                                            {
                                                                                                logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is" + reversalData.Response.NationsBenefitsTransactionId);
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                                                            }
                                                                                            #endregion
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        iSNBSExOccurred = true;
                                                                                        Resources.Message.Display($"{NBSHelper.GetApprovalDescription(Convert.ToInt16(analyseData.Response.Code))}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                                        logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- The NBS Analyze response is:- {Convert.ToString(analyseData.Response.Code)}");
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    iSNBSExOccurred = true;
                                                                                    Resources.Message.Display("Basket verification failed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Analyze request");
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                iSNBSExOccurred = true;
                                                                                Resources.Message.Display("Failed to Get Card Details.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the NBS Member Details (BinValue,AccountHolderName,AccountNoWithoutMask)");
                                                                                //PREREAD CANCEL
                                                                            }
                                                                            #endregion
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        iSNBSExOccurred = true;
                                                                        Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3504
                                                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + " ERROR NBS URL,ENTITYID,STOREID or TOKEN is MISSING.");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                iSNBSExOccurred = true;
                                                                Resources.Message.Display($"NB Configuartion is not configured. Please change the card", "Payment Failure", MessageBoxButtons.OK);
                                                                logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- NBS card found and the pharmacy has not configurred for NB.");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //PREREAD SALE
                                                            cardInfo.preReadId = PreReadId;
                                                            Thread threadPreReadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreReadSale), ticketNum, ref cardInfo));
                                                            threadPreReadSale.Start();
                                                            while (threadPreReadSale.IsAlive)
                                                            {
                                                                Application.DoEvents();
                                                                if (ofrmWait.IsDisposed)
                                                                {
                                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                    {
                                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "APPROVED" ||
                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "SUCCESS" ||
                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "1" ||
                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "8" ||
                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("Transaction Status :Approved") ||
                                                                Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("PartialApproved"))
                                                            {
                                                                NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;
                                                                PaymentType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PaymentType;
                                                            }
                                                            else
                                                            {
                                                                Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK);
                                                                logger.Warn("btnContinue_Click() - Processing PreRead Sale is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                                //PREREAD-CANCEL
                                                                this.isCanceled = true;
                                                                ofrmWait.Close();
                                                                this.Close();
                                                                return;
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3317
                                                        logger.Warn("btnContinue_Click() - Processing PreRead Response is : " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                        this.isCanceled = true;
                                                        ofrmWait.Close();
                                                        this.Close();
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    Resources.Message.Display($"Failed to retrieve PreRead data.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                    logger.Warn($"btnContinue_Click() - Processign NBS Card info for: The Failed to get PreRead Data");
                                                }
                                                #endregion

                                                #region PRIMEPOS-3504
                                                if (iSNBSExOccurred)
                                                {
                                                    Thread threadSaleCancel = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.Cancel), ticketNum, ref cardInfo));
                                                    threadSaleCancel.Start();
                                                    while (threadSaleCancel.IsAlive)
                                                    {
                                                        Application.DoEvents();
                                                        if (ofrmWait.IsDisposed)
                                                        {
                                                            if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                            {
                                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    this.isCanceled = true;
                                                    ofrmWait.Close();
                                                    this.Close();
                                                    return;
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {

                                        if (Configuration.CSetting.StrictReturn)
                                        {
                                            if (isNBSPayment)
                                            {
                                                if (Configuration.CSetting.NBSEnable)
                                                {
                                                    #region PRIMEPOS-3517 Need to check token is expired or not
                                                    if (Configuration.CSetting.NBSTokenExpriresAt != null)
                                                    {
                                                        TimeSpan difference = Configuration.CSetting.NBSTokenExpriresAt - DateTime.UtcNow;

                                                        if (difference.TotalMinutes < 10)
                                                        {
                                                            NBSProcessor nbsProcessors = new NBSProcessor(Configuration.CSetting.NBSUrl, "");
                                                            TokenResponseData nbsTokenData = new TokenResponseData();
                                                            nbsTokenData = nbsProcessors.GetToken(Configuration.CInfo.PHNPINO);
                                                            if (nbsTokenData != null)
                                                            {
                                                                Configuration.CSetting.NBSToken = nbsTokenData.AccessToken;
                                                                Configuration.CSetting.NBSTokenExpriresAt = Convert.ToDateTime(nbsTokenData.ExpiresIn);
                                                            }
                                                        }
                                                    }
                                                    #endregion


                                                    if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSToken) && !string.IsNullOrEmpty(Configuration.CSetting.NBSEntityID) && !string.IsNullOrEmpty(Configuration.CSetting.NBSStoreID))
                                                    {
                                                        iSNBSExOccurred = false;
                                                        NBSProcessor nbsPro = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                                                        #region PRIMEPOS-NBS-RETURN
                                                        AnalyzeMerchant merchant = new AnalyzeMerchant
                                                        {
                                                            EntityId = Configuration.CSetting.NBSEntityID,
                                                            StoreId = Configuration.CSetting.NBSStoreID,
                                                            TerminalId = Configuration.StationID //PRIMEPOS-3478
                                                        };
                                                        AnalyseData analyseData = nbsPro.AnalyzeRequest(lineItemsdata, merchant, NBSReturnUid, ticketNum, true);
                                                        if (analyseData != null)
                                                        {
                                                            if (analyseData.Response.Code != null && analyseData.Response.Code == "000" || analyseData.Response.Code == "100")
                                                            {
                                                                string nbsBenifitsID = analyseData.Response.NationsBenefitsTransactionId;
                                                                double autAmount = analyseData.Response.AuthorizedTransactionAmount;
                                                                if (DialogResult.Yes == Resources.Message.Display($"The NB approved amount is $ {autAmount.ToString("0.00")} , Do you want to proceed? ", "NB TRANSACTION", MessageBoxButtons.YesNo))
                                                                {
                                                                    #region NBS-RETURN-TRANSACTION
                                                                    cardInfo.isNBSTransaction = true;
                                                                    cardInfo.transAmount = Convert.ToString(Math.Abs(autAmount));
                                                                    Thread threadSalesReturn = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                                                    threadSalesReturn.Start();
                                                                    while (threadSalesReturn.IsAlive)
                                                                    {
                                                                        Application.DoEvents();
                                                                        if (ofrmWait.IsDisposed)
                                                                        {
                                                                            if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                            {
                                                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                                                break;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "APPROVED" || 
                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "SUCCESS" ||
                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "1" ||
                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "8" ||
                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("Transaction Status :Approved") ||
                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("PartialApproved"))
                                                                    {
                                                                        NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;  //PRIMEPOS-3427
                                                                        PaymentType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PaymentType;
                                                                        #region NBS-REDEEM-CALL
                                                                        //verify the amount approved and the device approved
                                                                        RedeemData rData = nbsPro.RedeemTransaction(nbsBenifitsID, Convert.ToString(autAmount));
                                                                        if (rData != null && rData?.Response?.Code == "000")
                                                                        {
                                                                            NBSTransId = rData.Response.NationsBenefitsTransactionId;
                                                                            NBSUid = NBSReturnUid; //PRIMEPOS-3375
                                                                            logger.Trace("The NBSTrans Redeem successful");
                                                                            logger.Trace("The NBSTransID is" + rData.Response.NationsBenefitsTransactionId);
                                                                        }
                                                                        else
                                                                        {
                                                                            logger.Trace($"The error is occurring while attempting to retrieve the response for the NBSRedeem request :  {rData?.Response?.Code}.");
                                                                        }
                                                                        #endregion
                                                                    }
                                                                    else
                                                                    {
                                                                        //Cancel
                                                                        logger.Trace("The NBS transaction is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription));
                                                                        Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                        #region NBS-REVERSAL-CALL
                                                                        //verify the amount approved and the device approved
                                                                        ReversalData revData = nbsPro.ReversalTransaction(nbsBenifitsID);
                                                                        if (revData != null && revData?.Response?.Code == "000")
                                                                        {
                                                                            logger.Trace("The NBSTransID is Reversal successful");
                                                                        }
                                                                        else
                                                                        {
                                                                            logger.Trace("The error is occurring while attempting to retrieve the response for the NBSReversal request");
                                                                        }
                                                                        #endregion
                                                                        iSNBSExOccurred = true;
                                                                    }
                                                                    #endregion
                                                                }
                                                                else
                                                                {
                                                                    //Cancel
                                                                    logger.Trace("The user does not want to process the amount approved by NBS");
                                                                    #region NBS-REVERSAL-CALL
                                                                    //verify the amount approved and the device approved
                                                                    ReversalData reveData = nbsPro.ReversalTransaction(nbsBenifitsID);
                                                                    if (reveData != null && reveData.Response.Code == "000")
                                                                    {
                                                                        logger.Trace("The NBSTransID is" + reveData.Response.NationsBenefitsTransactionId);
                                                                    }
                                                                    else
                                                                    {
                                                                        logger.Trace("The error is occurring while attempting to retrieve the response for the NBSReversal request");
                                                                    }
                                                                    #endregion
                                                                    iSNBSExOccurred = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Cancel
                                                                iSNBSExOccurred = true;
                                                                Resources.Message.Display($"{NBSHelper.GetApprovalDescription(Convert.ToInt16(analyseData.Response.Code))}.", "Payment Failure", MessageBoxButtons.OK);
                                                                logger.Trace($"The NBSAnalyze resp is {Convert.ToString(analyseData.Response.Code)}");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            iSNBSExOccurred = true;
                                                            Resources.Message.Display($"Basket verification failed.", "Payment Failure", MessageBoxButtons.OK);
                                                            logger.Trace("The error is occurring while attempting to retrieve the response for the NBSAnalyze request");
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        iSNBSExOccurred = true;
                                                        Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3504
                                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + " ERROR NBS URL,ENTITYID,STOREID or TOKEN is MISSING.");
                                                    }
                                                }
                                                else
                                                {
                                                    iSNBSExOccurred = true;
                                                    Resources.Message.Display($"NB Configuartion is not configured. Please change the card", "Payment Failure", MessageBoxButtons.OK);
                                                    logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- NBS card found and the pharmacy has not configurred for NB.");
                                                }
                                            }
                                            else
                                            {
                                                Thread threadSalesReturn = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                                threadSalesReturn.Start();
                                                while (threadSalesReturn.IsAlive)
                                                {
                                                    Application.DoEvents();
                                                    if (ofrmWait.IsDisposed)
                                                    {
                                                        if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                        {
                                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            iSNBSExOccurred = false;
                                            //PREREAD THE CARD DETAILS
                                            cardInfo.preReadType = "RETURN"; //PRIMEPOS-3407
                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreRead), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                        break;
                                                    }
                                                }
                                            }

                                            //GET PREREAD DATA
                                            if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                                            {
                                                if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "PREREADSUCCESSFUL")
                                                {
                                                    //GET PREREAD DATA AND VERIFY THE BIN
                                                    PaymentType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PaymentType;
                                                    PreReadId = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PreReadId;
                                                    AccountNo = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountNo;
                                                    BinValue = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.BinValue;
                                                    AccountHolderName = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountHolderName;
                                                    if (!string.IsNullOrWhiteSpace(AccountNo) && AccountNo.Trim().Length >= 4)
                                                    {
                                                        AccountNoWithoutMask = AccountNo.Trim().Substring(AccountNo.Trim().Length - 4, 4);
                                                    }

                                                    //PRIMEPOS-3529 BIN VERIFICATION
                                                    iSBinVerified = false;
                                                    List<string> NBSBINList = new List<string>(Configuration.CSetting.NBSBins.Split(','));

                                                    if (NBSBINList != null && NBSBINList.Count > 0)
                                                    {
                                                        foreach (string bin in NBSBINList)
                                                        {
                                                            if (BinValue == Convert.ToString(bin).Trim())
                                                            {
                                                                iSBinVerified = true;
                                                            }
                                                        }
                                                    }

                                                    if (iSBinVerified)
                                                    {
                                                        if (Configuration.CSetting.NBSEnable)
                                                        {
                                                            #region PRIMEPOS-3517 Need to check token is expired or not
                                                            if (Configuration.CSetting.NBSTokenExpriresAt != null)
                                                            {
                                                                TimeSpan difference = Configuration.CSetting.NBSTokenExpriresAt - DateTime.UtcNow;

                                                                if (difference.TotalMinutes < 10)
                                                                {
                                                                    NBSProcessor nbsProcessors = new NBSProcessor(Configuration.CSetting.NBSUrl, "");
                                                                    TokenResponseData nbsTokenData = new TokenResponseData();
                                                                    nbsTokenData = nbsProcessors.GetToken(Configuration.CInfo.PHNPINO);
                                                                    if (nbsTokenData != null)
                                                                    {
                                                                        Configuration.CSetting.NBSToken = nbsTokenData.AccessToken;
                                                                        Configuration.CSetting.NBSTokenExpriresAt = Convert.ToDateTime(nbsTokenData.ExpiresIn);
                                                                    }
                                                                }
                                                            }
                                                            #endregion

                                                            if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSToken) && !string.IsNullOrEmpty(Configuration.CSetting.NBSEntityID) && !string.IsNullOrEmpty(Configuration.CSetting.NBSStoreID))
                                                            {
                                                                NBSProcessor nbsProcessor = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);

                                                                if (lineItemsdata != null && lineItemsdata.Count == 0)
                                                                {
                                                                    Resources.Message.Display("The line items are not eligible for NB.", "Payment Failure", MessageBoxButtons.OK);
                                                                    iSNBSExOccurred = true;
                                                                }
                                                                else
                                                                {
                                                                    if (!string.IsNullOrEmpty(AccountNoWithoutMask) && !string.IsNullOrEmpty(BinValue) && !string.IsNullOrEmpty(AccountHolderName))
                                                                    {
                                                                        //123456:SMITH/JOHN:3456
                                                                        string finalinput = BinValue + ":" + AccountHolderName + ":" + AccountNoWithoutMask;
                                                                        string uidNBS = nbsProcessor.ComputeSHA256Hash(finalinput);
                                                                        AnalyzeMerchant merchant = new AnalyzeMerchant
                                                                        {
                                                                            EntityId = Configuration.CSetting.NBSEntityID,
                                                                            StoreId = Configuration.CSetting.NBSStoreID,
                                                                            TerminalId = Configuration.StationID
                                                                        };
                                                                        AnalyseData analyseData = nbsProcessor.AnalyzeRequest(lineItemsdata, merchant, uidNBS, ticketNum, false);
                                                                        if (analyseData != null && analyseData.Response != null)
                                                                        {
                                                                            if (!string.IsNullOrWhiteSpace(analyseData.Response.Code) && analyseData.Response.Code == "000" || analyseData.Response.Code == "100")
                                                                            {
                                                                                string nbsBenifitsID = analyseData.Response.NationsBenefitsTransactionId;
                                                                                double autAmount = analyseData.Response.AuthorizedTransactionAmount;
                                                                                if (DialogResult.Yes == Resources.Message.Display($"The NB approved amount is $ {autAmount.ToString("0.00")} , Do you want to proceed? ", "NB TRANSACTION", MessageBoxButtons.YesNo))
                                                                                {
                                                                                    #region NBS-REFUND-Transaction
                                                                                    cardInfo.preReadId = PreReadId;
                                                                                    cardInfo.isNBSTransaction = true;
                                                                                    cardInfo.transAmount = Convert.ToString(autAmount);
                                                                                    Thread threadSaleRedeem = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreReadSaleReturn), ticketNum, ref cardInfo));
                                                                                    threadSaleRedeem.Start();
                                                                                    while (threadSaleRedeem.IsAlive)
                                                                                    {
                                                                                        Application.DoEvents();
                                                                                        if (ofrmWait.IsDisposed)
                                                                                        {
                                                                                            if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                                            {
                                                                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                                                                break;
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "APPROVED" ||
                                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "SUCCESS" ||
                                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "1" ||
                                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "8" ||
                                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("Transaction Status :Approved") ||
                                                                                        Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("PartialApproved"))
                                                                                    {
                                                                                        NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;
                                                                                        #region NBS-REDEEM-CALL
                                                                                        RedeemData redeemData = nbsProcessor.RedeemTransaction(nbsBenifitsID, Convert.ToString(autAmount));
                                                                                        if (redeemData?.Response?.Code == "000")
                                                                                        {
                                                                                            NBSTransId = redeemData.Response.NationsBenefitsTransactionId;
                                                                                            NBSUid = uidNBS;
                                                                                            isNBSPayment = true; //PRIMEPOS-3504
                                                                                            logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBS Transaction Redeem successful");
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Redeem Transaction : {redeemData?.Response?.Code}.");
                                                                                        }
                                                                                        #endregion
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        iSNBSExOccurred = true;
                                                                                        Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                                        logger.Warn("btnContinue_Click() - Processing PreRead Sale is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                                                        #region NBS-REVERSAL-CALL
                                                                                        ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                                                        if (reversalData?.Response?.Code == "000")
                                                                                        {
                                                                                            logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is Reversal successful");
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                                                        }
                                                                                        #endregion
                                                                                    }
                                                                                    #endregion
                                                                                }
                                                                                else
                                                                                {
                                                                                    iSNBSExOccurred = true;
                                                                                    logger.Info("btnContinue_Click() - Processign NBS Card info for:- The user does not want to process the amount approved by NBS");
                                                                                    #region NBS-REVERSAL-CALL
                                                                                    ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                                                    if (reversalData?.Response?.Code == "000")
                                                                                    {
                                                                                        logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is" + reversalData.Response.NationsBenefitsTransactionId);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                                                    }
                                                                                    #endregion
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                iSNBSExOccurred = true;
                                                                                Resources.Message.Display($"{NBSHelper.GetApprovalDescription(Convert.ToInt16(analyseData.Response.Code))}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                                logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- The NBS Analyze response is:- {Convert.ToString(analyseData.Response.Code)}");
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            iSNBSExOccurred = true;
                                                                            Resources.Message.Display("Basket verification failed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                            logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Analyze request");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        iSNBSExOccurred = true;
                                                                        Resources.Message.Display("Failed to Get Card Details.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the NBS Member Details (BinValue,AccountHolderName,AccountNoWithoutMask)");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                iSNBSExOccurred = true;
                                                                Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3504
                                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + " ERROR NBS URL,ENTITYID,STOREID or TOKEN is MISSING.");
                                                            }

                                                        }
                                                        else
                                                        {
                                                            iSNBSExOccurred = true;
                                                            Resources.Message.Display($"NB Configuartion is not configured. Please change the card", "Payment Failure", MessageBoxButtons.OK);
                                                            logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- NBS card found and the pharmacy has not configurred for NB.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //PREREAD SALE-RETURN NEEDS TO CHNAGE
                                                        cardInfo.preReadId = PreReadId;
                                                        Thread threadPreReadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreReadSaleReturn), ticketNum, ref cardInfo));
                                                        threadPreReadSale.Start();
                                                        while (threadPreReadSale.IsAlive)
                                                        {
                                                            Application.DoEvents();
                                                            if (ofrmWait.IsDisposed)
                                                            {
                                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                {
                                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                                    break;
                                                                }
                                                            }
                                                        }

                                                        if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "APPROVED" ||
                                                            Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "SUCCESS" ||
                                                            Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "1" ||
                                                            Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "8" ||
                                                            Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("Transaction Status :Approved") ||
                                                            Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).Contains("PartialApproved"))
                                                        {
                                                            NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;
                                                            PaymentType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PaymentType;
                                                        }
                                                        else
                                                        {
                                                            Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK);
                                                            logger.Warn("btnContinue_Click() - Processing PreRead Sale is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                            //PREREAD-CANCEL
                                                            this.isCanceled = true;
                                                            ofrmWait.Close();
                                                            this.Close();
                                                            return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3317
                                                    logger.Warn("btnContinue_Click() - Processing PreRead Response is : " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                    this.isCanceled = true;
                                                    ofrmWait.Close();
                                                    this.Close();
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                Resources.Message.Display($"Failed to retrieve PreRead data.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                logger.Warn($"btnContinue_Click() - Processign NBS Card info for: The Failed to get PreRead Data(RETURN).");
                                            }
                                        }
                                        #region PRIMEPOS-3504
                                        if (iSNBSExOccurred)
                                        {
                                            Thread threadSaleCancel = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.Cancel), ticketNum, ref cardInfo));
                                            threadSaleCancel.Start();
                                            while (threadSaleCancel.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                        break;
                                                    }
                                                }
                                            }
                                            this.isCanceled = true;
                                            ofrmWait.Close();
                                            this.Close();
                                            return;
                                        }
                                        #endregion
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == VANTIV.ToUpper().Trim() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") //PRIMEPOS-3231
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.DebitCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        Resources.Message.Display("Please select another payment method. \nAll Debit Card must be swiped. \nManual entry of a Debit Card is not allowed.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    logger.Trace("btnContinue_Click() - Processign Debit Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                            case (PayTypes.EBT):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Starting.");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Starting.");
                                    if (Configuration.CPOSSet.UseSigPad == false && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != "VERIFONE 1001/1000" && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != VANTIV.ToUpper().Trim() && Configuration.CPOSSet.UsePinPad != true)
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR EBT is not supported.");
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isConnected() && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == VANTIV.ToUpper().Trim() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500")) //PRIMEPOS-3231
                                    {
                                        if (this.oTransType == POSTransactionType.Sales)
                                        {

                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.EBT);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (this.oTransType == POSTransactionType.SalesReturn)
                                        {
                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.EBT);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Completed");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                            case (PayTypes.NBS):
                                #region PRIMEPOS-3372
                                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign NBS Card info for:" + pmtProcessMethod, " Starting");
                                logger.Trace("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + " Starting");

                                #region PRIMEPOS-3412 Need to check token is expired or not
                                if (Configuration.CSetting.NBSTokenExpriresAt != null)
                                {
                                    TimeSpan difference = Configuration.CSetting.NBSTokenExpriresAt - DateTime.UtcNow;

                                    if (difference.TotalMinutes < 10)
                                    {
                                        NBSProcessor nbsProcessors = new NBSProcessor(Configuration.CSetting.NBSUrl, "");
                                        TokenResponseData nbsTokenData = new TokenResponseData();
                                        nbsTokenData = nbsProcessors.GetToken(Configuration.CInfo.PHNPINO);
                                        if (nbsTokenData != null)
                                        {
                                            Configuration.CSetting.NBSToken = nbsTokenData.AccessToken;
                                            Configuration.CSetting.NBSTokenExpriresAt = Convert.ToDateTime(nbsTokenData.ExpiresIn);
                                        }
                                        else
                                        {
                                            //TesxtBox Failed to connect NBSService
                                            Resources.Message.Display("Failed to connect to the NB service.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                            logger.Warn("frmMain() - btnContinue_Click() - Failed to get Access Token for NBS");
                                            this.isCanceled = true;
                                            iSNBSExOccurred = true;
                                            ofrmWait.Close();
                                            this.Close();
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    //TesxtBox NBS configuration is missing
                                    Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                    logger.Warn("frmMain() - btnContinue_Click() - Failed to get Access Token for NBS. Please check the NBS configuration.");
                                    this.isCanceled = true;
                                    iSNBSExOccurred = true;
                                    ofrmWait.Close();
                                    this.Close();
                                    return;
                                }
                                #endregion

                                #region PRIMEPOS-3407 if line items does not contain any items or conatains character values
                                if (lineItemsdata != null && lineItemsdata.Count == 0) //PRIMEPOS-3407
                                {
                                    Resources.Message.Display("The line items are not eligible for NB.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                    this.isCanceled = true;
                                    ofrmWait.Close();
                                    this.Close();
                                    return;
                                }
                                #endregion

                                if (this.oTransType == POSTransactionType.Sales)
                                {
                                    iSNBSExOccurred = false;

                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign NBS Card info for:" + pmtProcessMethod, "ERROR NBS option is currently not supported.");
                                    logger.Trace("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + "ERROR NBS option is currently not supported.");
                                    #region NBS-PREREAD
                                    cardInfo.preReadType = "SALE"; //PRIMEPOS-3407
                                    Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.NBS, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreRead), ticketNum, ref cardInfo));
                                    threadSale.Start();
                                    while (threadSale.IsAlive)
                                    {
                                        Application.DoEvents();
                                        if (ofrmWait.IsDisposed)
                                        {
                                            if (ofrmWait.DialogResult == DialogResult.Cancel)
                                            {
                                                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                break;
                                            }
                                        }
                                    }
                                    #endregion
                                    if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                                    {
                                        if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "PREREADSUCCESSFUL")
                                        {
                                            AccountNo = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountNo;
                                            #region PRIMEPOS-3372
                                            if (!string.IsNullOrWhiteSpace(AccountNo) && AccountNo.Trim().Length >= 4)
                                            {
                                                AccountNoWithoutMask = AccountNo.Trim().Substring(AccountNo.Trim().Length - 4, 4);
                                            }
                                            #endregion
                                            BinValue = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.BinValue;
                                            AccountHolderName = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountHolderName;
                                            PreReadId = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PreReadId;
                                        }
                                        else
                                        {
                                            //TextBox Prea read issue
                                            Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3317
                                            logger.Warn("btnContinue_Click() - Processign NBS PreRead Response is : " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                            this.isCanceled = true;
                                            iSNBSExOccurred = true;
                                            ofrmWait.Close();
                                            this.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        iSNBSExOccurred = true;
                                        Resources.Message.Display($"Failed to retrieve PreRead data.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                        logger.Warn($"btnContinue_Click() - Processign NBS Card info for: The Failed to get PreRead Data");
                                    }

                                    #region BIN-VALUE-VERIFING

                                    if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSToken) && !string.IsNullOrEmpty(Configuration.CSetting.NBSEntityID) && !string.IsNullOrEmpty(Configuration.CSetting.NBSStoreID)) //PRIMEPOS-3479-PRIMEPOS-3480
                                    {
                                        NBSProcessor nbsProcessor = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                                        iSBinVerified = false;
                                        if (Configuration.nBSBinRange != null && Configuration.nBSBinRange.Count > 0)
                                        {
                                            foreach (var i in Configuration.nBSBinRange)
                                            {
                                                if (i.binValue == BinValue && i.IsDeleted != true)
                                                {
                                                    iSBinVerified = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            #region EXTRA-CALL-TO-GET-BIN
                                            List<BinRangeData> nBSBinRanges = new List<BinRangeData>();
                                            nBSBinRanges = nbsProcessor.GetBinRange();
                                            if (nBSBinRanges != null)
                                            {
                                                foreach (var i in nBSBinRanges)
                                                {
                                                    NBSBinRange nBSBin = new NBSBinRange();
                                                    nBSBin.binValue = Convert.ToString(i.BinCode);
                                                    nBSBin.IsDeleted = Convert.ToBoolean(i.IsDelete);
                                                    Configuration.nBSBinRange.Add(nBSBin);
                                                }
                                            }
                                            else
                                            {
                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:- Failed to get BinRange for NBS");
                                            }

                                            if (Configuration.nBSBinRange != null && Configuration.nBSBinRange.Count > 0)
                                            {
                                                foreach (var i in Configuration.nBSBinRange)
                                                {
                                                    if (i.binValue == BinValue && i.IsDeleted != true)
                                                    {
                                                        iSBinVerified = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:- Failed to get BinRange for NBS");
                                            }
                                            #endregion
                                        }

                                        if (iSBinVerified) //Need to change
                                        {
                                            #region CREATE UID FOR NBS
                                            if (!string.IsNullOrEmpty(AccountNoWithoutMask) && !string.IsNullOrEmpty(BinValue) && !string.IsNullOrEmpty(AccountHolderName))
                                            {
                                                //123456:SMITH/JOHN:3456
                                                string finalinput = BinValue + ":" + AccountHolderName + ":" + AccountNoWithoutMask;
                                                string uidNBS = nbsProcessor.ComputeSHA256Hash(finalinput);
                                                //string uidNBS = string.Empty;
                                                //MMS.Encryption.NativeEncryption.EncryptText(finalinput, ref uidNBS); //PRIMEPOS-3375
                                                AnalyzeMerchant merchant = new AnalyzeMerchant
                                                {
                                                    EntityId = Configuration.CSetting.NBSEntityID,
                                                    StoreId = Configuration.CSetting.NBSStoreID,
                                                    TerminalId = Configuration.StationID //PRIMEPOS-3478
                                                };
                                                AnalyseData analyseData = nbsProcessor.AnalyzeRequest(lineItemsdata, merchant, uidNBS, ticketNum, false);
                                                if (analyseData != null && analyseData.Response != null)
                                                {
                                                    if (!string.IsNullOrWhiteSpace(analyseData.Response.Code) && analyseData.Response.Code == "000" || analyseData.Response.Code == "100")
                                                    {
                                                        string nbsBenifitsID = analyseData.Response.NationsBenefitsTransactionId;
                                                        double autAmount = analyseData.Response.AuthorizedTransactionAmount;
                                                        if (DialogResult.Yes == Resources.Message.Display($"The NB approved amount is $ {autAmount.ToString("0.00")} , Do you want to proceed? ", "NB TRANSACTION", MessageBoxButtons.YesNo)) //PRIMEPOS-3482
                                                        {
                                                            #region NBS-SALE-Transaction
                                                            cardInfo.preReadId = PreReadId;
                                                            cardInfo.transAmount = Convert.ToString(autAmount);
                                                            Thread threadSaleRedeem = new Thread(() => ProcessTransactionThread(PayTypes.NBS, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                                            threadSaleRedeem.Start();
                                                            while (threadSaleRedeem.IsAlive)
                                                            {
                                                                Application.DoEvents();
                                                                if (ofrmWait.IsDisposed)
                                                                {
                                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                    {
                                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "APPROVED")
                                                            {
                                                                NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;
                                                                #region NBS-REDEEM-CALL
                                                                //verify the amount approved and the device approved
                                                                RedeemData redeemData = nbsProcessor.RedeemTransaction(nbsBenifitsID, Convert.ToString(autAmount));
                                                                if (redeemData?.Response?.Code == "000")
                                                                {
                                                                    NBSTransId = redeemData.Response.NationsBenefitsTransactionId;
                                                                    NBSUid = uidNBS; //PRIMEPOS-3375
                                                                    logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBS Transaction Redeem successful");
                                                                }
                                                                else
                                                                {
                                                                    logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Redeem Transaction : {redeemData?.Response?.Code}.");
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                //Cancel
                                                                Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The NBS transaction is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                                #region NBS-REVERSAL-CALL
                                                                //verify the amount approved and the device approved
                                                                ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                                if (reversalData?.Response?.Code == "000")
                                                                {
                                                                    logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is Reversal successful");
                                                                }
                                                                else
                                                                {
                                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                                }
                                                                #endregion
                                                                iSNBSExOccurred = true;
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            //Cancel
                                                            logger.Info("btnContinue_Click() - Processign NBS Card info for:- The user does not want to process the amount approved by NBS");
                                                            #region NBS-REVERSAL-CALL
                                                            //verify the amount approved and the device approved
                                                            ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                            if (reversalData?.Response?.Code == "000")
                                                            {
                                                                logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is" + reversalData.Response.NationsBenefitsTransactionId);
                                                            }
                                                            else
                                                            {
                                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                            }
                                                            #endregion
                                                            iSNBSExOccurred = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Cancel
                                                        iSNBSExOccurred = true;
                                                        Resources.Message.Display($"{ NBSHelper.GetApprovalDescription(Convert.ToInt16(analyseData.Response.Code)) }.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                        logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- The NBS Analyze response is:- {Convert.ToString(analyseData.Response.Code)}");
                                                    }
                                                }
                                                else
                                                {
                                                    iSNBSExOccurred = true;
                                                    Resources.Message.Display("Basket verification failed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Analyze request");
                                                }
                                            }
                                            else
                                            {
                                                iSNBSExOccurred = true;
                                                Resources.Message.Display("Failed to retrieve PreRead data. NB Card details are missing.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The NBS Card details are missing");
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            iSNBSExOccurred = true;
                                            Resources.Message.Display("Invalid NB card.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3404
                                            logger.Trace("btnContinue_Click() - Processign NBS Card info for:- BinRange Verification failed");
                                        }
                                    }
                                    else
                                    {
                                        iSNBSExOccurred = true;
                                        Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + " ERROR NBS URL,ENTITYID,STOREID or TOKEN is MISSING.");
                                    }

                                    if (iSNBSExOccurred)
                                    {
                                        Thread threadSaleCancel = new Thread(() => ProcessTransactionThread(PayTypes.NBS, Enum.GetName(typeof(POSTransactionType), POSTransactionType.Cancel), ticketNum, ref cardInfo));
                                        threadSaleCancel.Start();
                                        while (threadSaleCancel.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                    break;
                                                }
                                            }
                                        }
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }

                                    #endregion

                                }
                                else if (this.oTransType == POSTransactionType.SalesReturn)
                                {
                                    if (Configuration.CSetting.StrictReturn) //PRIMEPOS-3407
                                    {
                                        if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSToken) && !string.IsNullOrEmpty(Configuration.CSetting.NBSEntityID) && !string.IsNullOrEmpty(Configuration.CSetting.NBSStoreID))  //PRIMEPOS-3479-PRIMEPOS-3480
                                        {
                                            iSNBSExOccurred = false;
                                            NBSProcessor nbsPro = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                                            #region PRIMEPOS-NBS-RETURN
                                            AnalyzeMerchant merchant = new AnalyzeMerchant
                                            {
                                                EntityId = Configuration.CSetting.NBSEntityID,
                                                StoreId = Configuration.CSetting.NBSStoreID,
                                                TerminalId = Configuration.StationID //PRIMEPOS-3478
                                            };
                                            AnalyseData analyseData = nbsPro.AnalyzeRequest(lineItemsdata, merchant, NBSReturnUid, ticketNum, true);
                                            if (analyseData != null)
                                            {
                                                if (analyseData.Response.Code != null && analyseData.Response.Code == "000" || analyseData.Response.Code == "100")
                                                {
                                                    string nbsBenifitsID = analyseData.Response.NationsBenefitsTransactionId;
                                                    double autAmount = analyseData.Response.AuthorizedTransactionAmount;
                                                    if (DialogResult.Yes == Resources.Message.Display($"The NB approved amount is $ {autAmount.ToString("0.00")} , Do you want to proceed? ", "NB TRANSACTION", MessageBoxButtons.YesNo))
                                                    {
                                                        #region NBS-RETURN-TRANSACTION
                                                        cardInfo.transAmount = Convert.ToString(Math.Abs(autAmount));
                                                        Thread threadSalesReturn = new Thread(() => ProcessTransactionThread(PayTypes.NBS, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                                        threadSalesReturn.Start();
                                                        while (threadSalesReturn.IsAlive)
                                                        {
                                                            Application.DoEvents();
                                                            if (ofrmWait.IsDisposed)
                                                            {
                                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                {
                                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                                    break;
                                                                }
                                                            }
                                                        }

                                                        if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "SUCCESS")
                                                        {
                                                            NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;  //PRIMEPOS-3427
                                                            #region NBS-REDEEM-CALL
                                                            //verify the amount approved and the device approved
                                                            RedeemData rData = nbsPro.RedeemTransaction(nbsBenifitsID, Convert.ToString(autAmount));
                                                            if (rData != null && rData?.Response?.Code == "000")
                                                            {
                                                                NBSTransId = rData.Response.NationsBenefitsTransactionId;
                                                                NBSUid = NBSReturnUid; //PRIMEPOS-3375
                                                                logger.Trace("The NBSTrans Redeem successful");
                                                                logger.Trace("The NBSTransID is" + rData.Response.NationsBenefitsTransactionId);
                                                            }
                                                            else
                                                            {
                                                                logger.Trace($"The error is occurring while attempting to retrieve the response for the NBSRedeem request :  {rData?.Response?.Code}.");
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            //Cancel
                                                            logger.Trace("The NBS transaction is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription));
                                                            Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                            #region NBS-REVERSAL-CALL
                                                            //verify the amount approved and the device approved
                                                            ReversalData revData = nbsPro.ReversalTransaction(nbsBenifitsID);
                                                            if (revData != null && revData?.Response?.Code == "000")
                                                            {
                                                                logger.Trace("The NBSTransID is Reversal successful");
                                                            }
                                                            else
                                                            {
                                                                logger.Trace("The error is occurring while attempting to retrieve the response for the NBSReversal request");
                                                            }
                                                            #endregion
                                                            iSNBSExOccurred = true;
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        //Cancel
                                                        logger.Trace("The user does not want to process the amount approved by NBS");
                                                        #region NBS-REVERSAL-CALL
                                                        //verify the amount approved and the device approved
                                                        ReversalData reveData = nbsPro.ReversalTransaction(nbsBenifitsID);
                                                        if (reveData != null && reveData.Response.Code == "000")
                                                        {
                                                            logger.Trace("The NBSTransID is" + reveData.Response.NationsBenefitsTransactionId);
                                                        }
                                                        else
                                                        {
                                                            logger.Trace("The error is occurring while attempting to retrieve the response for the NBSReversal request");
                                                        }
                                                        #endregion
                                                        iSNBSExOccurred = true;
                                                    }
                                                }
                                                else
                                                {
                                                    //Cancel
                                                    iSNBSExOccurred = true;
                                                    Resources.Message.Display($"{ NBSHelper.GetApprovalDescription(Convert.ToInt16(analyseData.Response.Code)) }.", "Payment Failure", MessageBoxButtons.OK);
                                                    logger.Trace($"The NBSAnalyze resp is {Convert.ToString(analyseData.Response.Code)}");
                                                }
                                            }
                                            else
                                            {
                                                iSNBSExOccurred = true;
                                                Resources.Message.Display($"Basket verification failed.", "Payment Failure", MessageBoxButtons.OK);
                                                logger.Trace("The error is occurring while attempting to retrieve the response for the NBSAnalyze request");
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            iSNBSExOccurred = true;
                                            Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                            logger.Warn("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + " ERROR NBS URL,ENTITYID,STOREID or TOKEN is MISSING.");
                                        }

                                        if (iSNBSExOccurred)
                                        {
                                            this.isCanceled = true;
                                            ofrmWait.Close();
                                            this.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        #region PRIMEPOS-3407
                                        #region NBS-PREREAD
                                        cardInfo.preReadType = "RETURN"; //PRIMEPOS-3407
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreRead), ticketNum, ref cardInfo)); //PRIMEPOS-3504
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                    break;
                                                }
                                            }
                                        }
                                        #endregion
                                        if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                                        {
                                            if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "PREREADSUCCESSFUL")
                                            {
                                                AccountNo = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountNo;
                                                #region PRIMEPOS-3372
                                                if (!string.IsNullOrWhiteSpace(AccountNo) && AccountNo.Trim().Length >= 4)
                                                {
                                                    AccountNoWithoutMask = AccountNo.Trim().Substring(AccountNo.Trim().Length - 4, 4);
                                                }
                                                #endregion
                                                BinValue = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.BinValue;
                                                AccountHolderName = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.AccountHolderName;
                                                PreReadId = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.PreReadId;
                                            }
                                            else
                                            {
                                                Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3317
                                                logger.Warn("btnContinue_Click() - Processign NBS PreRead Response is : " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                Resources.Message.Display($"Basket verification failed.", "Payment Failure", MessageBoxButtons.OK);
                                                this.isCanceled = true;
                                                iSNBSExOccurred = true;
                                                ofrmWait.Close();
                                                this.Close();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            iSNBSExOccurred = true;
                                            Resources.Message.Display($"Failed to retrieve PreRead data.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                            logger.Warn($"btnContinue_Click() - Processign NBS Card info for: The Failed to get PreRead Data");
                                        }

                                        if (!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSToken) && !string.IsNullOrEmpty(Configuration.CSetting.NBSEntityID) && !string.IsNullOrEmpty(Configuration.CSetting.NBSStoreID)) //PRIMEPOS-3479-PRIMEPOS-3480
                                        {
                                            NBSProcessor nbsProcessor = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                                            #region PRIMEPOS-3407 BIN-VERIFICATION
                                            iSBinVerified = false;
                                            if (Configuration.nBSBinRange != null && Configuration.nBSBinRange.Count > 0)
                                            {
                                                foreach (var i in Configuration.nBSBinRange)
                                                {
                                                    if (i.binValue == BinValue && i.IsDeleted != true)
                                                    {
                                                        iSBinVerified = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                #region EXTRA-CALL-TO-GET-BIN
                                                List<BinRangeData> nBSBinRanges = new List<BinRangeData>();
                                                nBSBinRanges = nbsProcessor.GetBinRange();
                                                if (nBSBinRanges != null)
                                                {
                                                    foreach (var i in nBSBinRanges)
                                                    {
                                                        NBSBinRange nBSBin = new NBSBinRange();
                                                        nBSBin.binValue = Convert.ToString(i.BinCode);
                                                        nBSBin.IsDeleted = Convert.ToBoolean(i.IsDelete);
                                                        Configuration.nBSBinRange.Add(nBSBin);
                                                    }
                                                }
                                                else
                                                {
                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- Failed to get BinRange for NBS");
                                                }

                                                if (Configuration.nBSBinRange != null && Configuration.nBSBinRange.Count > 0)
                                                {
                                                    foreach (var i in Configuration.nBSBinRange)
                                                    {
                                                        if (i.binValue == BinValue && i.IsDeleted != true)
                                                        {
                                                            iSBinVerified = true;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- Failed to get BinRange for NBS");
                                                }
                                                #endregion
                                            }
                                            #endregion

                                            if (iSBinVerified) //Need to change
                                            {
                                                #region CREATE UID FOR NBS
                                                if (!string.IsNullOrEmpty(AccountNoWithoutMask) && !string.IsNullOrEmpty(BinValue) && !string.IsNullOrEmpty(AccountHolderName))
                                                {
                                                    //123456:SMITH/JOHN:3456
                                                    string finalinput = BinValue + ":" + AccountHolderName + ":" + AccountNoWithoutMask;
                                                    string uidNBS = nbsProcessor.ComputeSHA256Hash(finalinput);
                                                    //string uidNBS = string.Empty;
                                                    //MMS.Encryption.NativeEncryption.EncryptText(finalinput, ref uidNBS); //PRIMEPOS-3375
                                                    AnalyzeMerchant merchant = new AnalyzeMerchant
                                                    {
                                                        EntityId = Configuration.CSetting.NBSEntityID,
                                                        StoreId = Configuration.CSetting.NBSStoreID,
                                                        TerminalId = Configuration.StationID //PRIMEPOS-3478
                                                    };
                                                    AnalyseData analyseData = nbsProcessor.AnalyzeRequest(lineItemsdata, merchant, uidNBS, ticketNum, true);
                                                    if (analyseData != null && analyseData.Response != null)
                                                    {
                                                        if (!string.IsNullOrWhiteSpace(analyseData.Response.Code) && analyseData.Response.Code == "000" || analyseData.Response.Code == "100")
                                                        {
                                                            string nbsBenifitsID = analyseData.Response.NationsBenefitsTransactionId;
                                                            double autAmount = analyseData.Response.AuthorizedTransactionAmount;
                                                            if (DialogResult.Yes == Resources.Message.Display($"The NB approved amount is $ {autAmount.ToString("0.00")} , Do you want to proceed? ", "NB TRANSACTION", MessageBoxButtons.YesNo))
                                                            {
                                                                #region NBS-REFUND-Transaction
                                                                cardInfo.preReadId = PreReadId;
                                                                cardInfo.transAmount = Convert.ToString(autAmount);
                                                                Thread threadSaleRedeem = new Thread(() => ProcessTransactionThread(PayTypes.NBS, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                                                threadSaleRedeem.Start();
                                                                while (threadSaleRedeem.IsAlive)
                                                                {
                                                                    Application.DoEvents();
                                                                    if (ofrmWait.IsDisposed)
                                                                    {
                                                                        if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                                        {
                                                                            PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                if (Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result).ToUpper() == "SUCCESS")
                                                                {
                                                                    NBSTransType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.NBSPaytype;
                                                                    #region NBS-REDEEM-CALL
                                                                    //verify the amount approved and the device approved
                                                                    RedeemData redeemData = nbsProcessor.RedeemTransaction(nbsBenifitsID, Convert.ToString(autAmount));
                                                                    if (redeemData?.Response?.Code == "000")
                                                                    {
                                                                        NBSTransId = redeemData.Response.NationsBenefitsTransactionId;
                                                                        NBSUid = uidNBS; //PRIMEPOS-3375
                                                                        logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBS Transaction Redeem successful");
                                                                    }
                                                                    else
                                                                    {
                                                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Redeem Transaction");
                                                                    }
                                                                    #endregion
                                                                }
                                                                else
                                                                {
                                                                    //Cancel
                                                                    Resources.Message.Display($"{Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result)}.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The NBS transaction is not completed the response is " + Convert.ToString(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result));
                                                                    #region NBS-REVERSAL-CALL
                                                                    //verify the amount approved and the device approved
                                                                    ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                                    if (reversalData?.Response?.Code == "000")
                                                                    {
                                                                        logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is Reversal successful");
                                                                    }
                                                                    else
                                                                    {
                                                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                                    }
                                                                    #endregion
                                                                    iSNBSExOccurred = true;
                                                                }
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                //Cancel
                                                                logger.Info("btnContinue_Click() - Processign NBS Card info for:- The user does not want to process the amount approved by NBS");
                                                                #region NBS-REVERSAL-CALL
                                                                //verify the amount approved and the device approved
                                                                ReversalData reversalData = nbsProcessor.ReversalTransaction(nbsBenifitsID);
                                                                if (reversalData?.Response?.Code == "000")
                                                                {
                                                                    logger.Info("btnContinue_Click() - Processign NBS Card info for:- The NBSTransID is" + reversalData.Response.NationsBenefitsTransactionId);
                                                                }
                                                                else
                                                                {
                                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Reversal Transaction");
                                                                }
                                                                #endregion
                                                                iSNBSExOccurred = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Cancel
                                                            iSNBSExOccurred = true;
                                                            Resources.Message.Display($"{ NBSHelper.GetApprovalDescription(Convert.ToInt16(analyseData.Response.Code)) }.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                            logger.Warn($"btnContinue_Click() - Processign NBS Card info for:- The NBS Analyze response is:- {Convert.ToString(analyseData.Response.Code)}");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        iSNBSExOccurred = true;
                                                        Resources.Message.Display("Basket verification failed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                        logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The error is occurring while attempting to retrieve the response for the NBS Analyze request");
                                                    }
                                                }
                                                else
                                                {
                                                    iSNBSExOccurred = true;
                                                    Resources.Message.Display("Failed to retrieve PreRead data. The NB Card details are missing.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3480
                                                    logger.Warn("btnContinue_Click() - Processign NBS Card info for:- The NBS Card details are missing");
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                iSNBSExOccurred = true;
                                                Resources.Message.Display("Invalid NB card.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3404
                                                logger.Trace("btnContinue_Click() - Processign NBS Card info for:- BinRange Verification failed");
                                            }
                                        }
                                        else
                                        {
                                            iSNBSExOccurred = true;
                                            Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK); //PRIMEPOS-3404
                                            logger.Warn("btnContinue_Click() - Processign NBS Card info for:" + pmtProcessMethod + " ERROR NBS URL,STOREID,ENTITYID or TOKEN is MISSING.");
                                        }

                                        if (iSNBSExOccurred)
                                        {
                                            Thread threadSaleCancel = new Thread(() => ProcessTransactionThread(PayTypes.NBS, Enum.GetName(typeof(POSTransactionType), POSTransactionType.Cancel), ticketNum, ref cardInfo));
                                            threadSaleCancel.Start();
                                            while (threadSaleCancel.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                        break;
                                                    }
                                                }
                                            }
                                            this.isCanceled = true;
                                            ofrmWait.Close();
                                            this.Close();
                                            return;
                                        }
                                        #endregion
                                    }
                                }
                                logger.Trace("btnContinue_Click() - Processign NBS Card  for:" + pmtProcessMethod + "Completed");
                                #endregion
                                break;
                        }
                        SetOldProcessor();

                        if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                        {

                            //NileshJ - TicketNum
                            if ((PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.TrouTd == this.ticketNum) && (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result == "SUCCESS"))
                            {
                                this.ticketNum = null;
                            }
                            // Till - NileshJ
                            //if (!string.IsNullOrEmpty(PaymentType) && PaymentType.ToUpper().Equals("DEBIT")) //PRIMEPOS-3519
                            //{
                            //    SubCardType = "Debit Card";
                            //}
                            //else
                            //{
                            //    SubCardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType;
                            //}
                            SubCardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType;
                            if ((oPayTpes == PayTypes.CreditCard || oPayTpes == PayTypes.NBS) && !string.IsNullOrEmpty(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).VANTIV_SigString) && PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).VANTIV_SigString.Length > 0) //PRIMEPOS-3407
                            {
                                SigPadUtil.DefaultInstance.CustomerSignature = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).VANTIV_SigString;
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


                        responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                        string responseError = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseError;

                        this.ApprovedPCCCardInfo = cardInfo.Copy();

                        ofrmWait.Close();


                        if (!VANTIVResponseFound(responseStatus, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo))
                        {
                            isCanceled = true;
                            return;
                        }
                        else
                        {
                            ShoW_VANTIV_Signature();
                        }
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign   Card  for:" + pmtProcessMethod, "Completed");
                        logger.Trace("btnContinue_Click() - Processign   Card  for:" + pmtProcessMethod + "Completed");
                        break; // End VANTIV
                    }

                #endregion  Integration by ARVIND
                case "ELAVON":
                    {
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign  Card :" + pmtProcessMethod, " Starting");
                        logger.Trace("btnContinue_Click() - Processign  Card :" + pmtProcessMethod + " Starting");



                        frmWaitScreen ofrmWait = new frmWaitScreen(true, "Please wait...", "Processing Payment Online");
                        ofrmWait.Show();
                        if (!CheckAVSFields())
                            return;

                        int errorCode = 0;

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
                        cardInfo.PaymentProcessor = pmtProcessMethod;
                        cardInfo.TransactionID = this.originalTransID;
                        cardInfo.TransDate = this.TransDate;
                        cardInfo.IsElavonTax = this.IsElavonTax;
                        cardInfo.ElavonTotalTax = this.ElavonTotalTax;//2943
                        //cardInfo.cardExpiryDate = this.ExpirayDate;
                        if (this.isFSATransaction)
                        {
                            //if(this.PendingAmount == )
                            if (this.PendingAmount != this.FSAAmount)
                            {
                                if (DialogResult.Yes == Resources.Message.Display("Do you have a FSA card", "FSA CARD", MessageBoxButtons.YesNo))
                                {
                                    this.isFSATransaction = true;
                                    //cardInfo.transAmount = this.FSARxAmount.ToString();
                                }
                                else
                                {
                                    this.isFSATransaction = false;
                                }
                                cardInfo.IsFSATransaction = this.isFSATransaction.ToString();
                                cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
                                cardInfo.transFSAAmount = this.FSAAmount.ToString();
                            }
                            else
                            {
                                this.isFSATransaction = false;
                                cardInfo.IsFSATransaction = this.isFSATransaction.ToString();
                            }
                        }

                        if (this.Tokenize)
                        {
                            cardInfo.Tokenize = true;
                        }

                        if (isAllowDuplicate)
                        {
                            cardInfo.isAllowDuplicate = true;
                        }

                        //while (SigPadUtil.DefaultInstance.VFD.IsStillWrite == true)
                        //{
                        //    Application.DoEvents();
                        //    Thread.Sleep(100);
                        //}
                        switch (oPayTpes)
                        {
                            case (PayTypes.CreditCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (this.oTransType == POSTransactionType.Sales)
                                    {
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                    else if (this.oTransType == POSTransactionType.SalesReturn)
                                    {

                                        Thread threadSalesReturn = new Thread(() => ProcessTransactionThread(PayTypes.CreditCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSalesReturn.Start();
                                        while (threadSalesReturn.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.CreditCard);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Credit  Card info for:" + pmtProcessMethod, " Completed");
                                    logger.Trace("btnContinue_Click() - Processign Credit  Card info for:" + pmtProcessMethod + " Completed");
                                    break;
                                }
                            case (PayTypes.DebitCard):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, " Starting");
                                    logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + " Starting");
                                    if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "ELAVON")
                                    {
                                        Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.DebitCard, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                        threadSale.Start();
                                        while (threadSale.IsAlive)
                                        {
                                            Application.DoEvents();
                                            if (ofrmWait.IsDisposed)
                                            {
                                                if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                {
                                                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.DebitCard);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign Debit  Card info for:" + pmtProcessMethod, "ERROR Debit option is currently not supported.");
                                        logger.Trace("btnContinue_Click() - Processign Debit  Card info for:" + pmtProcessMethod + "ERROR Debit option is currently not supported.");
                                        Resources.Message.Display("Please select another payment method. \nAll Debit Card must be swiped. \nManual entry of a Debit Card is not allowed.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    logger.Trace("btnContinue_Click() - Processign Debit Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                            case (PayTypes.EBT):
                                {
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Starting.");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Starting.");
                                    if (Configuration.CPOSSet.UseSigPad == false && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != "VERIFONE 1001/1000" && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != VANTIV.ToUpper().Trim() && Configuration.CPOSSet.UsePinPad != true)
                                    {
                                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "ERROR EBT is not supported.");
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isConnected() && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == "ELAVON")
                                    {
                                        if (this.oTransType == POSTransactionType.Sales)
                                        {

                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.EBT);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else if (this.oTransType == POSTransactionType.SalesReturn)
                                        {
                                            Thread threadSale = new Thread(() => ProcessTransactionThread(PayTypes.EBT, Enum.GetName(typeof(POSTransactionType), this.oTransType), ticketNum, ref cardInfo));
                                            threadSale.Start();
                                            while (threadSale.IsAlive)
                                            {
                                                Application.DoEvents();
                                                if (ofrmWait.IsDisposed)
                                                {
                                                    if (ofrmWait.DialogResult == DialogResult.Cancel)
                                                    {
                                                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).CancelTransVANTIV(PayTypes.EBT);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "ERROR EBT is not supported.");
                                        Resources.Message.Display("EBT option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                                        this.isCanceled = true;
                                        ofrmWait.Close();
                                        this.Close();
                                        return;
                                    }
                                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign EBT  Card  for:" + pmtProcessMethod, "Completed");
                                    logger.Trace("btnContinue_Click() - Processign EBT  Card  for:" + pmtProcessMethod + "Completed");
                                    break;
                                }
                        }
                        SetOldProcessor();

                        if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                        {

                            //NileshJ - TicketNum
                            if ((PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.TrouTd == this.ticketNum) && (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.Result == "SUCCESS"))
                            {
                                this.ticketNum = null;
                            }
                            // Till - NileshJ
                            SubCardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.CardType;
                            if (oPayTpes == PayTypes.CreditCard && !string.IsNullOrEmpty(PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ELAVON_SigString) && PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ELAVON_SigString.Length > 0)
                            {
                                SigPadUtil.DefaultInstance.CustomerSignature = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ELAVON_SigString;
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


                        responseStatus = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus;
                        string responseError = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseError;

                        this.ApprovedPCCCardInfo = cardInfo.Copy();

                        ofrmWait.Close();


                        if (!ELAVONResponseFound(responseStatus, PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo))
                        {
                            //if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription.ToUpper() != "SUCCESS" && (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription.ToUpper() == "DUPLICATE" || PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.ResultDescription.ToUpper() == "CALL AUTH CENTER"))
                            //{
                            //    frmForceTransaction ofrmFT = new frmForceTransaction();
                            //    ofrmFT.ShowDialog();

                            //    if (ofrmFT.isAllowDuplicate)
                            //    {
                            //        this.isAllowDuplicate = true;
                            //        btnContinue_Click(sender, e);
                            //    }
                            //    else
                            //    {
                            //        isCanceled = true;
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            isCanceled = true;
                            return;
                        }
                        else
                        {
                            ShoW_ELAVON_Signature();
                        }
                        POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Processign   Card  for:" + pmtProcessMethod, "Completed");
                        logger.Trace("btnContinue_Click() - Processign   Card  for:" + pmtProcessMethod + "Completed");
                        break;
                    }
                default:

                    #region DEFAULTCASE

                    {
                        try
                        {
                            oCCDbf = OpenDBFile(Configuration.CCProcessDB, true);

                            AddRowToFoxpro();

                            Thread.Sleep(1000);
                            oCCDbf.db.refreshRecord();
                            this.sbMain.Panels[0].Text = oCCDbf.GetValString("CURR_STAT");
                            oThread = new Thread(new System.Threading.ThreadStart(checkResponse));
                            oThread.Start();
                        }
                        catch (Exception exp)
                        {
                            logger.Fatal(exp, "btnContinue_Click(object sender, System.EventArgs e)");
                            clsUIHelper.ShowErrorMsg(exp.Message);
                        }
                        break;
                    }

                    #endregion DEFAULTCASE
            }
            //Added By SRT(Ritesh Parekh)
            //To Remove after confirmation if not necessary
            SetOldProcessor();
            //End Of Added By  SRT(Ritesh Parekh)
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "btnContinue_Click", clsPOSDBConstants.Log_Exiting);
            logger.Trace("btnContinue_Click() - " + clsPOSDBConstants.Log_Exiting);
        }

        public void SdeleteResultFile()
        {
            try
            {
                #region PRIMEPOS-2586 21-Sep-2018 JY Commented                                
                //FileInfo[] rgFiles = null;
                //string path = "c:\\"; 
                //string strCmdLine;
                //System.Diagnostics.Process process1;
                //DirectoryInfo di = new DirectoryInfo(path);
                //rgFiles = di.GetFiles("ResultFile.txt");
                //int ii = rgFiles.Length;
                //if(rgFiles.Length != 0)
                //{
                //    for(int g = 0; g < ii; g++)
                //    {
                //        if(rgFiles[g].Exists)
                //        {
                //            strCmdLine = path + rgFiles[g].Name;
                //            process1 = new System.Diagnostics.Process();
                //            System.Diagnostics.Process.Start(Application.StartupPath + "\\sdelete", strCmdLine);
                //            process1.Close();
                //        }
                //    }
                //}
                #endregion

                #region PRIMEPOS-2586 21-Sep-2018 JY Added
                string strResultFile = Configuration.objMerchantConfig.Payment_ResultFile;
                if (File.Exists(strResultFile) == true)
                {
                    File.Delete(strResultFile);
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "SdeleteResultFile()");
            }
        }

        private void SetOldProcessor()
        {
            //Added By SRT(Ritesh Parekh) Date : 22-Jul-2009
            //This variable will hold old processor name and reassign at end of flow.
            if (Configuration.CPOSSet.PaymentProcessor != OldProcessor)
            {
                Configuration.CPOSSet.PaymentProcessor = OldProcessor;
                objPccPmt = new PccPaymentSvr();
            }

            //Added By Manoj 6/25/2014
            isF10Press = false;
        }

        private bool CheckAVSFields()
        {
            bool bFlag = true;
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "CheckAVSFields()" + pmtProcessMethod, "Entering");
            logger.Trace("CheckAVSFields() - " + pmtProcessMethod + clsPOSDBConstants.Log_Entering);
            //Check
            //if (PccPaymentSvr.DefaultInstance.IsAVSModeOn.Equals("Y") && this.TrackNoII.Equals(string.Empty)) //Modified By Dharmedra (SRT) on Nov-19-08 to check TrackNoII
            if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).IsAVSModeOn.Equals("Y") && this.TrackNoII.Equals(string.Empty)) //Modified By Dharmedra (SRT) on Nov-19-08 to check TrackNoII
            {
                if (Configuration.CPOSSet.PaymentProcessor == "HPS" ? txtZipCode.Text.Trim().Length == 0 : txtAddress.Text.Trim().Length == 0 || txtZipCode.Text.Trim().Length == 0) //If its HPS check for ZIP code only
                {
                    if (POS_Core_UI.Resources.Message.Display("Address verification mode is on.\r\nPlease enter the customer Address and Zip code.\r\nBY LEAVING THESE FIELDS BLANK YOU \r\nMAY BE CHARGED A HIGHER PROCESSING FEE.\r\nDo you wants to leave these fields blank?", "AVS Verification", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        txtAddress.Text = string.Empty;
                        txtZipCode.Text = string.Empty;
                        bFlag = true;
                    }
                    else
                    {
                        this.txtAddress.Focus();
                        this.btnContinue.Enabled = true;
                        bFlag = false;
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "CheckAVSFields()" + pmtProcessMethod, "Exiting " + bFlag.ToString());
            logger.Trace("CheckAVSFields() - " + pmtProcessMethod + clsPOSDBConstants.Log_Exiting + bFlag.ToString());
            return bFlag;
        }

        private void FillCardInfo(ref PccCardInfo cardInfo, bool fillCardDetails)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "FillCardInfo()", clsPOSDBConstants.Log_Entering);
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
                        //if (Configuration.CPOSSet.PaymentProcessor == "HPS")
                        //{
                        //    cardInfo.trackII = "%B" + sCardNumber+"^"+cardInfo.cardHolderName+"?;"+ SigPadUtil.DefaultInstance.SigPadCardInfo.Track2 + "?";
                        //}
                        //else
                        //{
                        //    cardInfo.trackII = SigPadUtil.DefaultInstance.SigPadCardInfo.Track2;
                        //}
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
                //cardInfo.cardType = PccPaymentSvr.DefaultInstance.GetCardType(cardInfo.cardNumber);
                cardInfo.cardType = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).GetCardType(cardInfo.cardNumber);
            }


            if (this.oTransType == POSTransactionType.SalesReturn)
            {
                this.Amount = Math.Abs(this.Amount);
                if (this.isFSATransaction)
                {
                    this.FSAAmount = -(this.FSAAmount);
                }
            }
            cardInfo.transAmount = this.Amount.ToString();
            if (this.FSAAmount > 0 || this.FSARxAmount > 0)
            {
                cardInfo.transFSAAmount = this.FSAAmount.ToString();
                //Added By SRT(Ritesh Parekh) Date : 18-Aug-2009
                cardInfo.transFSARxAmount = this.FSARxAmount.ToString();
            }
            //End Of Added By SRT(Ritesh Parekh)
            CheckIISTransactionFlag(ref cardInfo);
            //End Of Added & Modified By Dharmendra (SRT)
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "FillCardInfo()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("FillCardInfo() - " + clsPOSDBConstants.Log_Exiting);
        }

        //Added By Dharmendra (SRT) on Dec-03-08. to check whether IS FSA Card(chkIIASTransaction)
        // and depending on its check status we will assign the value T/U/1/0 to the member IsFSATransaction
        // of PccCardInfo
        private void CheckIISTransactionFlag(ref PccCardInfo cardInfo)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "CheckIISTransactionFlag()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CheckIISTransactionFlag() - " + clsPOSDBConstants.Log_Entering);
            //Modified & Added By Dharmendra (SRT) on Dec-04-08 to set the value of bFSAFlag
            //Added By Dharmendra (SRT) on Jan-20-09
            if (frmMain.sForceFSA == "Y")
            {
                this.isFSATransaction = true;
            }
            else
            {
                this.isFSATransaction = false;
            }
            //Added Till Here Jan-20-09
#if TEST
            if (POS_Core_UI.Resources.Message.Display("Is This Card Is FSA Card ?", "FSA Card Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.isFSATransaction = true;
            }
#endif
            if (Configuration.CPOSSet.PaymentProcessor == "XCHARGE" || Configuration.CPOSSet.PaymentProcessor == "XLINK")
            {
                if (this.isFSATransaction == true) //Modified By Dharmendra (SRT) on Dec-04-08
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
        }

        //Added Till Here

        private void CancelTransaction()
        {
            /*this.btnContinue.Enabled = false;
            this.sbMain.Panels[0].Text = "Initializing Cancellation....";
            try
            {
                if (isManualProcess == false)
                {
                    oCCDbf = OpenDBFile(Configuration.CCProcessDB, true);

                    this.isCancelTransaction = true;
                    AddRowToFoxpro();

                    Thread.Sleep(1000);
                    oCCDbf.db.refreshRecord();
                    this.sbMain.Panels[0].Text = oCCDbf.GetValString("CURR_STAT");
                    oThread = new Thread(new System.Threading.ThreadStart(checkResponse));
                    oThread.Start();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }*/
        }

        private void AddRowToFoxpro()
        {
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
        }

        private void ExecuteCCResponseFound(string strResp)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                CCResponse d = new CCResponse(CCResponseFound);
                if (this.IsHandleCreated == true)
                {
                    this.Invoke(d, new object[] { strResp });
                }
            }
        }

        private void checkResponse()
        {
            String sClaimStat = oCCDbf.GetValString("CLAIMSTAT");

            while (true) //sClaimStat.Trim()=="N")
            {
                System.Threading.Thread.Sleep(1000);
                oCCDbf.db.refreshRecord();
                sClaimStat = oCCDbf.GetValString("CLAIMSTAT");
                ExecuteCCResponseFound(sClaimStat);

                //				if (sClaimStat=="E")
                //				{
                //					eCCResponse("E");
                //					if (clsUIHelper.ShowErrorMsg ("Error while processing \n " + oCCDbf.GetValString("ERRORDESC").Trim() + "\n Do you want to retry","Process Payment",MessageBoxButtons.RetryCancel)==DialogResult.Cancel)
                //					{
                //						this.isCanceled=true;
                //						this.Close();
                //						break;
                //					}
                //					else
                //					{
                //						AddRowToFoxpro();
                //						sClaimStat=" ";
                //					}
                //				}
                //				else if (sClaimStat=="A")
                //				{
                //					eCCResponse("A");
                //					this.AllowCancel=false;
                //					this.AuthNo=oCCDbf.GetValString("AUTHNO");
                //					frmPOSPayAuthNo ofrmAuthNo=new frmPOSPayAuthNo();
                //					ofrmAuthNo.txtAuthorizationNo.Text=this.AuthNo;
                //					ofrmAuthNo.txtAuthorizationNo.ReadOnly=true;
                //					this.btnProcessCancel.Enabled=false;
                //					ofrmAuthNo.ShowDialog(this);
                //					this.isCanceled=false;
                //					this.Close();
                //					break;
                //				}
                //				else if (sClaimStat=="P")
                //				{
                //					eCCResponse("P");
                //					frmPOSGetDebitCard ofrmDBCard=new frmPOSGetDebitCard(this.oCCDbf);
                //					if (ofrmDBCard.ShowDialog(this)==DialogResult.Cancel)
                //					{
                //						this.isCanceled=true;
                //						this.Close();
                //						break;
                //					}
                //					else
                //					{
                //						this.AllowCancel=false;
                //						//sClaimStat=" ";
                //					}
                //				}
                //				else if (sClaimStat=="I")
                //				{
                //					eCCResponse("I");
                //					this.btnProcessCancel.Enabled=false;
                //					this.AllowCancel=false;
                //				}
                ////				this.sbMain.Panels[0].Text=oCCDbf.GetValString("CURR_STAT");
                //				if (this.sbMain.Panels[0].Text.Trim()=="")
                //				{
                //					this.sbMain.Panels[0].Text="Processing...";
                //				}
            }
            //			this.sbMain.Panels[0].Text=oCCDbf.GetValString("CURR_STAT");
        }

        public CbDbf.CCbDbf OpenDBFile(string sFileName, bool bProcessMemo)
        {
            CbDbf.CCbDbf oFile = new CbDbf.CCbDbf();
            try
            {
                oFile.ProcessMemo = bProcessMemo;
                if (System.IO.File.Exists(sFileName) == true)
                {
                    oFile.OpenFile(sFileName);
                    if (oFile.db.isValid() != 0)
                    {
                        return oFile;
                    }
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "OpenDBFile(string sFileName, bool bProcessMemo)");
                throw new Exception("Error In Opening File " + ex.Message);
            }
        }

        private void txtCCNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "txtCCNo_Validating()", clsPOSDBConstants.Log_Entering);
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
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "txtCCNo_Validating()", clsPOSDBConstants.Log_Exiting);
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
                case "XLINK":
                case "PCCHARGE":
                case "XCHARGE":
                case "HPS":
                    {
                        #region PRIMEPOS-2761
                        if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                        {
                            PccPaymentSvr.sCurrentTicket = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.TrouTd;
                        }
                        if (pmtProcessMethod.Equals("XLINK"))
                        {
                            PccPaymentSvr.sCurrentTicket = ticketNum;
                        }
                        #endregion

                        //PccPaymentSvr.DefaultInstance.pccRespInfo = null;
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo = null;
                        //PRIMEPOS-2503 Jenny Added
                        if (!pmtProcessMethod.Equals("XLINK") || Configuration.CPOSSet.UseSigPad == true)
                        {
                            frmPinPad.DefaultInstance.TxnAmount = ""; // To clear the transaction assign to Pin Pad
                            frmPinPad.DefaultInstance.ClearPinData();
                        }
                        //End 2503
                        //added By shitaljit on 8 August
                        //to set isCanceled = false if user is doing Manual CC trans.
                        if (string.IsNullOrEmpty(AuthNo))
                        {
                            isCanceled = true; // Added by Manoj 7/13/2012
                        }
                        break;
                    }
                case "DBF": //THis was the original code by DPS.
                    {
                        if (oThread != null)
                        {
                            //try
                            // {
                            oThread.Abort();
                            //}
                            //catch (Exception) { }
                        }
                        break;
                    }
                case "HPSPAX": //PRIMEPOS-2761
                    {
                        if (PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo != null)
                        {
                            PccPaymentSvr.sCurrentTicket = PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).pccRespInfo.TrouTd;
                        }
                        break;
                    }
                case "WORLDPAY": //PRIMEPOS-2761
                case "EVERTEC": //PRIMEPOS-2761
                case "VANTIV": //PRIMEPOS-2761
                case "ELAVON": //PRIMEPOS-2761//2943
                    {
                        PccPaymentSvr.sCurrentTicket = ticketNum;
                        break;
                    }
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "frmPOSProcessCC_Closing()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("frmPOSProcessCC_Closing() - " + clsPOSDBConstants.Log_Exiting);
            //Added and Modified by SRT Till here
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
            //Modified By Dharmendra on May-08-09 to validate card Expiry Date
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "btnProcessManual_Click()", clsPOSDBConstants.Log_Entering);
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
                //#region Sprint-27 - PRIMEPOS-2301 07-Sep-2017 JY Added to validate Cvv code
                //if (txtCVVCode.Text.Trim().Length < 3)   
                //{
                //    POS_Core_UI.Resources.Message.Display("Invalid Card. CVV Code must be 3 digits");
                //    if (txtCVVCode.Enabled) txtCVVCode.Focus();
                //    return;
                //}
                //#endregion

                oAuthNo.ShowDialog(this);
                AuthNo = oAuthNo.txtAuthorizationNo.Text;
                if (AuthNo.Trim() == "")
                {
                    //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "btnProcessManual_Click()", clsPOSDBConstants.Log_Exiting + " with Blank Authorization No");
                    logger.Trace("btnProcessManual_Click() - " + clsPOSDBConstants.Log_Exiting + " with Blank Authorization No");
                    return;
                }
                CaptureSignature();
                //Added By Dharmendra on May-07-09 to assign the values in PcccCardInfo
                //since in manual processing the card info will not pass to the payment processors
                //hence there'll be no response but any how PccCardInfo should be filled and pass back
                //this info in frmPOSPayTypeList to continue furthur processing of transaction.

                //Comment out by Manoj - 4/25/2012 If and else is the same. Already check in capturesignature if UseSigpad is true;
                /*if (Configuration.CPOSSet.UseSigPad == true)
                {
                    if (SigPadUtil.DefaultInstance.CustomerSignature.Trim().Length > 0)
                    {
                        PccCardInfo objPccCardInfo = new PccCardInfo();
                        FillCardInfo(ref objPccCardInfo, true);
                        SubCardType = objPccCardInfo.cardType;
                        this.ApprovedPCCCardInfo = objPccCardInfo.Copy();
                    }
                }
                else
                {*/
                //end of comment - Manoj
                PccCardInfo objPccCardInfo = new PccCardInfo();
                FillCardInfo(ref objPccCardInfo, true);
                SubCardType = objPccCardInfo.cardType;
                this.ApprovedPCCCardInfo = objPccCardInfo.Copy();
                //}
                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "btnProcessManual_Click()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("btnProcessManual_Click() - " + clsPOSDBConstants.Log_Exiting);
                this.Close();
                //Modified Till Here May-08-09
            }
        }

        //Added By Dharmendra on May-08-09 to validate the card expiry date
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

        //Added Till Here May-08-09
        //private void btnProcessCancel_Click(object sender, System.EventArgs e)
        //{
        //}

        private void txtExpDate_Leave(object sender, System.EventArgs e)
        {
            //if (Configuration.CPOSSet.ONLINECCT == true) // 01-10-08 Commented By Dharmendra (SRT) bcz of inability to transfer control on Address textbox
            //{
            //    this.btnContinue.Focus();
            //}
            //else
            //{
            //    this.btnProcessManual.Focus();
            //}
            //this.txtAddress.Focus(); // Commented By Dharmendra on 02-Oct-08 bcz the tab order is set in property window
        }

        /// <summary>
        /// Author: Gaurav
        /// Mantis Id: 0000112
        /// Functionality Description: This method assigns the Responce object members to textboxes
        /// If textbox is empty and responce object is not empty.
        /// Known Bugs: If textbox is prefilled.
        /// </summary>
        private void AssignDataToTextBoxes(PccResponse objResponse)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "AssignDataToTextBoxes()", clsPOSDBConstants.Log_Entering);
            logger.Trace("AssignDataToTextBoxes() - " + clsPOSDBConstants.Log_Entering);

            //Changed By SRT(Gaurav) Date : 19-NOV-2008
            //Change Details: objResponse member names changed.
            //Added By SRT(Gaurav) Date : 18-NOV-2008
            if (txtAddress.Text.Trim() == string.Empty && objResponse.Address.Trim() != string.Empty)
            {
                txtAddress.Text = objResponse.Address;
            }
            if (txtZipCode.Text.Trim() == string.Empty && objResponse.ZIP.Trim() != string.Empty)
            {
                txtZipCode.Text = objResponse.ZIP;
            }
            if (txtCCNo.Text.Trim() == string.Empty && objResponse.CardNo.Trim() != string.Empty)
            {
                txtCCNo.Text = objResponse.CardNo;
            }
            if (txtExpDate.Text.Trim() == string.Empty && objResponse.Expiry.Trim() != string.Empty)
            {
                txtExpDate.Text = objResponse.Expiry;
            }
            //End Of Added By SRT(Gaurav)
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "AssignDataToTextBoxes()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("AssignDataToTextBoxes() - " + clsPOSDBConstants.Log_Exiting);
        }

        //added By rohit nair  for First mile
        private bool WpResponseFound(string status, string error, TransactionResult objResp)
        {
            bool isFound = false;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToUpper() != "SUCCESS")
                {
                    msg = string.IsNullOrEmpty(error) ? status.ToUpper() : error;
                    POS_Core_UI.Resources.Message.Display("Error while processing card \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                    isFound = false;
                }
                else
                {
                    SubCardType = objResp.AccountType;
                    this.AuthNo = objResp.ResponseTags.AuthCode;
                    this.isFSATransaction = false;
                    this.Amount = Configuration.convertNullToDecimal(objResp.Amount);
                    isFound = true;
                }
            }
            return isFound;
        }

        //Added By Manoj WP
        private bool WPResponseFound(string status, WPResponse objResp)
        {
            bool isFound = false;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToUpper() != "SUCCESS")
                {
                    msg = string.IsNullOrEmpty(objResp.Result) ? status.ToUpper() : objResp.Result;
                    POS_Core_UI.Resources.Message.Display("Error while processing card \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                    isFound = false;
                }
                else
                {
                    SubCardType = objResp.PayType;
                    this.AuthNo = objResp.AuthCode;
                    this.isFSATransaction = objResp.isFSA;
                    this.Amount = Configuration.convertNullToDecimal(objResp.TotalAmt);
                    isFound = true;
                }
            }
            return isFound;
        }

        private void ShoW_WP_Signature()
        {
            strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
            SigType = SigPadUtil.DefaultInstance.SigType;
            //SigPadUtil.DefaultInstance.isISC = true;

            if ((strSignature.Trim().Length > 0))//(Configuration.CPOSSet.DispSigOnTrans == true) &&
            {
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                }

                this.SigType = SigPadUtil.DefaultInstance.SigType;
                if (Configuration.CPOSSet.DispSigOnTrans == true)   //PRIMEPOS-2483 12-Feb-2021 JY Added
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, false);
                    ofrm.SetMsgDetails("Validating Signature...");
                    ofrm.ShowDialog();
                }
                /*if (ofrm.IsSignatureRejected == true)
                {
                    ofrm = null;
                    strSignature = "";
                    if (Configuration.CPOSSet.UseSigPad == true)
                    {
                        SigPadUtil.DefaultInstance.CustomerSignature = null;
                    }
                    //retVal = false;
                    CaptureSignature();
                }*/
            }
        }

        //PRIMEPOS-2528 (Suraj) 1-june-18 Added for HPSPAX Signature  Showing
        private void ShoW_PAX_Signature()
        {
            strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
            //SigPadUtil.DefaultInstance.isISC = true;

            if ((strSignature.Trim().Length > 0))//(Configuration.CPOSSet.DispSigOnTrans == true) &&
            {

                this.SigType = clsPOSDBConstants.BINARYIMAGE;
                if (Configuration.CPOSSet.DispSigOnTrans == true) // PRIMEPOS-2607 NileshJ - Added if condition
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                    ofrm.SetMsgDetails("Signature");
                    ofrm.ShowDialog();
                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;  //Aries8 PRIMEPOS-2952
                }
                else//PRIMEPOS-2952
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                }
            }

        }
        private void ShoW_EVERTEC_Signature()//PRIMEPOS-2664
        {
            strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
            //SigPadUtil.DefaultInstance.isISC = true;

            if ((strSignature.Trim().Length > 0))//(Configuration.CPOSSet.DispSigOnTrans == true) &&
            {

                this.SigType = clsPOSDBConstants.BINARYIMAGE;
                if (Configuration.CPOSSet.DispSigOnTrans == true)
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                    ofrm.SetMsgDetails("Signature");
                    ofrm.ShowDialog();
                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;  //Aries8 PRIMEPOS-2952
                }
                else//PRIMEPOS-2952
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                }
            }

        }
        //PRIMEPOS-2636
        private void ShoW_VANTIV_Signature()
        {
            strSignature = SigPadUtil.DefaultInstance.CustomerSignature;

            if ((strSignature.Trim().Length > 0))
            {

                this.SigType = clsPOSDBConstants.BINARYIMAGE;
                if (Configuration.CPOSSet.DispSigOnTrans == true)
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                    ofrm.SetMsgDetails("Signature");
                    ofrm.ShowDialog();
                }
            }


        }
        //2943
        private void ShoW_ELAVON_Signature()
        {
            strSignature = SigPadUtil.DefaultInstance.CustomerSignature;

            if ((strSignature.Trim().Length > 0))
            {

                this.SigType = clsPOSDBConstants.BINARYIMAGE;
                if (Configuration.CPOSSet.DispSigOnTrans == true)
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                    ofrm.SetMsgDetails("Signature");
                    ofrm.ShowDialog();
                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                }
                else
                {
                    frmViewSignature ofrm = new frmViewSignature(strSignature, this.SigType, true);
                    ofrm.SetMsgDetails("Signature");
                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                }
            }

        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method check whether card is processed or not
        /// If it is processed, then this method assigns the authroization code for further
        /// processing
        /// Known Bugs : None
        /// Start Date : 30-08-08
        /// </summary>
        /// <param name="status"></param>
        /// <param name="objResponse"></param>
        private void CCResponseFound(String status, PccResponse objResponse)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "CCResponseFound()", clsPOSDBConstants.Log_Entering);
            string msgString = string.Empty; //Added by SRT
            if (status.Trim() != "SUCCESS")
            {
                switch (status.Trim())
                {
                    case "PINPADERROR":
                        msgString = "PinPad not Initialized \r \n ";
                        break;
                    case "FAILURE":
                        msgString = "Error while processing card \r \n ";
                        break;
                }
                if (objResponse != null)
                {
                    msgString = msgString + status + "\n" + objResponse.ResultDescription;
                    //POS_Core_UI.Resources.Message.Display("Error while processing card \r \n " + msgString, "Card Processing Failed ", MessageBoxButtons.OK); //PRIMEPOS-2717 26-Jul-2019 JY Commented
                    #region PRIMEPOS-2717 26-Jul-2019 JY Added
                    if (msgString.Contains("Error while processing card"))
                        POS_Core_UI.Resources.Message.Display(msgString, "Card Processing Failed ", MessageBoxButtons.OK);
                    else
                        POS_Core_UI.Resources.Message.Display("Error while processing card \r \n " + msgString, "Card Processing Failed ", MessageBoxButtons.OK);
                    #endregion
                }
                else
                {
                    POS_Core_UI.Resources.Message.Display("Error while processing card \r \nNo response found. ", "Card Processing Failed ", MessageBoxButtons.OK);
                }
                //Added By Dharmendra on Mar-12-09
                //In case X-Link processor, x-charge client is directely opened bypassing card processing screen
                //so the text box txtCCNo does'nt get filled

                //Updated By SRT(Ritesh Parekh) Date: 27-Aug-2009
                //Updated for handeling the null responce object.
                if (this.txtCCNo.Text.Trim().Length == 0 && objResponse != null)
                {
                    this.txtCCNo.Text = objResponse.CardNo;
                }
                //Modified till Here Mar-12-09
                btnProcessCancel.Enabled = true; //Added By SRT 28-09-08
                this.AllowCancel = true;          //Added By SRT 29-09-08
                this.isCanceled = true;
                this.Close();
            }
            else
            {
                if (objResponse == null)
                    return;
                SubCardType = objResponse.CardType; //Added By Dharmendra On Mar-23-09
                this.ApprovedPCCCardInfo.tRoutId = objResponse.TransId;
                this.isFSATransaction = objResponse.isFSATransaction;

                //if (objResponse.isFSATransaction == true)
                //{
                // bool retVa = PerformVoidTransaction();
                //this.Amount = (decimal)objResponse.AmountApproved;
                //}
                //else if (this.ApprovedPCCCardInfo.IsFSATransaction == "1")
                //{
                //    this.Amount = (decimal)objResponse.AmountApproved;
                //}
                if (Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.PCCHARGE || Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper() == clsPOSDBConstants.HPS)
                {
                    if (this.ApprovedPCCCardInfo.IsFSATransaction == "1")
                        this.isFSATransaction = true;
                    else if (this.ApprovedPCCCardInfo.IsFSATransaction == "0")
                        this.isFSATransaction = false;
                }
                this.Amount = (decimal)objResponse.AmountApproved;

                this.AllowCancel = false;
                this.AuthNo = objResponse.PaymentProcessor.ToUpper() == "XLINK" ? objResponse.TransId : objResponse.AuthNo;
#if TEST
                    //Added By Dharmendra (SRT) on Nov-27-08 to show that how the values are stored in Response member variables
                    //Code To Remove
                    // TODO : To be deleted after actual flow is implemented by Naim from here onwards
                    string tempResultDescription = "Card No. :" + objResponse.CardNo + "\r\n Card Expiry : " + objResponse.Expiry + "\r\n " + objResponse.DateCharged;
                    tempResultDescription += "\r\n Result : " + objResponse.Result + "\r\n Authorization No. : " + objResponse.AuthNo;
                    tempResultDescription += "\r\n Approved Amount : " + objResponse.AmountApproved ;
                    tempResultDescription += "\r\n Additional Funds Required : " + objResponse.AdditionalFundsRequired;
                    tempResultDescription += "\r\n Is FSA Transaction: " + objResponse.isFSATransaction.ToString();
                    //Changed By SRT(Gaurav) Date: 02-Dec-2008 Details: Changed Messagebox Controls.
                    //Mantis ID: 0000136
                    //Info: The Following Code To Comment

                    MessageBox.Show(tempResultDescription, "Transaction Result", MessageBoxButtons.OK);// TODO : To Be Deleted
                    MessageBox.Show(objResponse.ResultDescription, "Transaction Result", MessageBoxButtons.OK); // TODO : To Be Deleted
                    this.btnProcessCancel.Enabled = false;

                    //End Of Code To Remove
                    //End Of Changed By SRT(Gaurav)
#endif
#if TEST
                if (objResponse.isFSATransaction && objResponse.AdditionalFundsRequired > 0.0)
                {
                    POS_Core_UI.Resources.Message.Display("Entire FSA Amount Is Not Eligible.\r\n Because It Exceeds The Card Limit", "FSA Transaction", MessageBoxButtons.OK);
                }
                //End Added
#endif
                if ((this.AuthNo.Trim() != "" || oTransType == POSTransactionType.SalesReturn) && this.isCancelTransaction == false)
                {
                    frmPOSPayAuthNo ofrmAuthNo = new frmPOSPayAuthNo(oTransType);
                    //Added By SRT(Gaurav) Date : 19-NOV-2008
                    //Mantis Id: 0000112
                    AssignDataToTextBoxes(objResponse);
                    //End Of Added By SRT(Gaurav)
                    ofrmAuthNo.txtAuthorizationNo.Text = this.AuthNo;
                    ofrmAuthNo.txtAuthorizationNo.ReadOnly = true;
                    this.btnProcessCancel.Enabled = false;
                    if (Configuration.CPOSSet.ShowAuthorization == true)
                    {
                        ofrmAuthNo.ShowDialog(this);
                    }

                    bool isEMVCard = false;
                    if (!string.IsNullOrWhiteSpace(objResponse.EntryMethod) && (objResponse.EntryMethod.Equals("Chip", StringComparison.OrdinalIgnoreCase) || objResponse.EntryMethod.Equals("CONTACT", StringComparison.OrdinalIgnoreCase)))
                        isEMVCard = true;
                    if (CaptureSignature(isEMVCard) == true)
                    {
                        this.isCanceled = false;
                    }
                    this.btnProcessCancel.Enabled = true;
                    if (!isCanceled)
                    {
                        this.Close();
                    }
                    else if (this.isCancelTransaction == true)
                    {
                        this.isCanceled = true;
                        this.Close();
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Authorization No. cannot be blank");
                    this.isCanceled = false;
                    this.Close();
                }
                this.isCanceled = this.isCancelTransaction;
            }

            if (this.sbMain.Panels[0].Text.Trim() == "")
            {
                this.sbMain.Panels[0].Text = "Processing...";
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "CCResponseFound()", clsPOSDBConstants.Log_Entering);
        }


        //PRIMEPOS-2528 (Suraj) 1-june-18 Added HPSPAX Response Parsing
        private bool PAXResponseFound(string status, PccResponse objResponse)
        {
            bool isFound = false;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToUpper() != "SUCCESS")
                {
                    if (objResponse.ResultDescription == "DUP TRANSACTION")
                    {
                        msg = string.IsNullOrEmpty(objResponse.Result) ? status.ToUpper() : objResponse.ResultDescription;
                        this.ticketNum = null;
                        if (POS_Core_UI.Resources.Message.Display(msg + "\r \nDo you want to Continue Transaction?", "Duplicate Transaction", MessageBoxButtons.OKCancel) == DialogResult.Yes)
                        {
                            #region PRIMEPOS-3047
                            //if (Tokenize == true)
                            //{
                            //    if (CustomerCardInfo == null)
                            //    {
                            //        SubCardType = objResponse.CardType;
                            //    }
                            //    else
                            //    {
                            //        SubCardType = CustomerCardInfo.cardType;
                            //    }
                            //}
                            //else
                            //{
                            //    SubCardType = objResponse.CardType;
                            //}

                            //this.ApprovedPCCCardInfo.tRoutId = objResponse.TransId;
                            //this.isFSATransaction = objResponse.isFSATransaction;
                            //this.Amount = (decimal)objResponse.AmountApproved;
                            //this.AuthNo = objResponse.AuthNo;
                            //if (this.txtCCNo.Text.Trim().Length == 0 && objResponse != null)
                            //{
                            //    this.txtCCNo.Text = objResponse.CardNo;
                            //}
                            isFound = false;
                            #endregion
                        }
                    }
                    else
                    {
                        #region Nilesh,Sajid PRIMEPOS-2853
                        if (objResponse.Result.ToUpper().Trim() == "FAILED")
                        {
                            string OldtransactionNumber = 0.ToString();
                            if (objResponse.ResultDescription.Contains("|"))
                            {
                                OldtransactionNumber = objResponse.ResultDescription.Trim().Split('|')[1];
                                msg = string.IsNullOrEmpty(objResponse.Result) ? string.Concat(status.ToUpper(), "\n", objResponse.ResultDescription.Split('|')[0]) : objResponse.ResultDescription.Split('|')[0];
                            }
                            if (Configuration.TransactionRequestCount < 3)
                            {
                                Configuration.TransactionRequestCount = Configuration.TransactionRequestCount + 1;
                                btnContinue_Click((object)OldtransactionNumber, null);
                                isFound = true;
                            }
                            else
                            {
                                logger.Error("Error while processing card. \r \n Please check if the Processor has already charged to avoid duplicate transaction further.  \r \n " + msg + "Card Processing Failed ");
                                POS_Core_UI.Resources.Message.Display("Error while processing card. \r \n Please check if the Processor has already charged to avoid duplicate transaction further.  \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                                isFound = false;
                                Configuration.TransactionRequestCount = 0;
                            }
                        }
                        #endregion
                        else
                        {
                            msg = string.IsNullOrEmpty(objResponse.Result) ? string.Concat(status.ToUpper(), "\n", objResponse.ResultDescription) : objResponse.ResultDescription;
                            isFound = false;
                            POS_Core_UI.Resources.Message.Display("Error while processing card. \r \n Please check if the Processor has already charged to avoid duplicate transaction further.  \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    if (Tokenize == true)
                    {
                        if (CustomerCardInfo == null)
                        {
                            SubCardType = objResponse.CardType;
                        }
                        else
                        {
                            SubCardType = string.IsNullOrEmpty(CustomerCardInfo.cardType) ? objResponse.CardType : CustomerCardInfo.cardType;//PRIMEPOS-3081
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
                    //NileshJ - ExpDate save in POSTransPayment_CClog table
                    if (this.txtExpDate.Text.Trim().Length == 0 && objResponse != null)
                    {
                        this.txtExpDate.Text = objResponse.Expiry;
                    }
                    isFound = true;
                }
            }
            return isFound;
        }

        /// <summary>
        /// Author: Manoj
        /// Description: HPS Debit card processing
        /// Date: 6/1/2012
        /// </summary>
        /// <param name="ticketNumber"></param>
        /// <param name="cardInfo"></param>
        //ARVIND PRIMEPOS-2664 EVERTEC
        private bool EVERTECResponseFound(string status, PccResponse objResponse)
        {
            bool isFound = false;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(objResponse.ResultDescription))
            {
                if (objResponse.ResultDescription.ToUpper() != "SUCCESS")
                {
                    if (objResponse.ResultDescription == "DUP TRANSACTION")
                    {
                        msg = string.IsNullOrEmpty(objResponse.Result) ? status.ToUpper() : objResponse.ResultDescription;
                        this.ticketNum = null;
                        Resources.Message.Display("Duplicate Transaction. \r \n Error While Processing Card.", "DUPLICATE TRANSACTION", MessageBoxButtons.OK);
                        isFound = false;
                    }
                    else
                    {
                        msg = string.IsNullOrEmpty(objResponse.Result) ? status.ToUpper() : objResponse.ResultDescription;
                        Resources.Message.Display("Error while processing card. \r \n ResultCode :" + objResponse.Result + " \r \n Result Description : " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                        isFound = false;
                        #region PRIMEPOS-2785
                        if (objResponse.ResultDescription?.ToUpper() == "DENIED BY CARD FIRST GEN"
                            || objResponse.ResultDescription?.ToUpper() == "DENIED BY CARD SECOND GEN"
                            || (objResponse.EmvReceipt != null &&
                            objResponse.EmvReceipt.ResponseCode?.ToUpper().Trim() == "INSUFFICIENT FUNDS"))
                        {
                            if (Resources.Message.Display(" Do you want to print the Denial receipt  ", " EVERTEC DENIAL RECEIPT ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                //PRIMEPOS-2664
                                string hostAddress = Configuration.CPOSSet.SigPadHostAddr.Split(':')[0];
                                string portNo = Configuration.CPOSSet.SigPadHostAddr.Split(':')[1].Split('/')[0];

                                EvertechProcessor evertechProcessor = EvertechProcessor.getInstance(hostAddress, Convert.ToInt32(portNo));
                                string emvTag = evertechProcessor.EmvTag();
                                RxLabel oRxLabel = new RxLabel();
                                oRxLabel.isDenialReceipt = true;
                                oRxLabel.AuthNo = objResponse.AuthNo;
                                oRxLabel.TransactionID = objResponse.TransId;
                                oRxLabel.ResultDescription = objResponse.ResultDescription;
                                oRxLabel.Amount = objResponse.AmountApproved.ToString();
                                oRxLabel.EvertecDenialDate = DateTime.Now;
                                oRxLabel.EmvTag = Regex.Replace(emvTag, "<br>", "\n", RegexOptions.IgnoreCase);//PRIMEPOS-2857
                                #region PRIMEPOS-2832 EVERTEC EBTBALANCE
                                if (objResponse.EmvReceipt != null && objResponse.EmvReceipt?.EbtBalance?.Length >= 3)
                                {
                                    oRxLabel.FoodBalance = objResponse.EmvReceipt.EbtBalance.Split('|')[0];
                                    oRxLabel.CashBalance = objResponse.EmvReceipt.EbtBalance.Split('|')[1];
                                }
                                oRxLabel.EntryMethod = objResponse.EmvReceipt.EntryLegend;
                                #endregion
                                if (!String.IsNullOrWhiteSpace(objResponse.EmvReceipt?.ResponseCode))
                                {
                                    oRxLabel.ResultDescription = objResponse.EmvReceipt?.ResponseCode;
                                }
                                oRxLabel.PaymentProcessor = objResponse.PaymentProcessor.ToUpper();
                                if (objResponse?.EmvReceipt != null)
                                {
                                    oRxLabel.MerchantID = objResponse.EmvReceipt.MerchantID.Trim();
                                    oRxLabel.Aid = objResponse.EmvReceipt.AppIndentifer;
                                    oRxLabel.InvoiceNumber = objResponse.EmvReceipt.InvoiceNumber;
                                    oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                                    oRxLabel.Batch = objResponse.EmvReceipt.BatchNumber;
                                    oRxLabel.ReferenceNumber = objResponse.EmvReceipt.ReferenceNumber;
                                }
                                oRxLabel.Print();
                            }
                        }
                        #endregion                        
                    }
                }
                else
                {
                    if (Tokenize == true)
                    {
                        if (CustomerCardInfo == null)
                        {
                            SubCardType = objResponse.CardType;
                        }
                        else
                        {
                            SubCardType = string.IsNullOrEmpty(CustomerCardInfo.cardType) ? objResponse.CardType : CustomerCardInfo.cardType;//PRIMEPOS-3081
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
            return isFound;
        }
        //PRIMEPOS-2636
        private bool VANTIVResponseFound(string status, PccResponse objResponse)
        {
            bool isFound = false;
            string msg = string.Empty;

            #region PRIMEPOS-3156
            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToUpper() != "SUCCESS" && status.ToUpper() != "1" && status.ToUpper() != "8" && !status.ToString().Contains("Transaction Status :Approved") && !status.ToString().Contains("Approved") && !status.ToString().Contains("PartialApproved")) //PRIMEPOS-3156
                {
                    if (objResponse.Result.ToUpper().Trim() == "FAILED")
                    {
                        string OldtransactionNumber = "0";
                        if (objResponse.ResultDescription.Contains("|"))
                        {
                            OldtransactionNumber = objResponse.ResultDescription.Trim().Split('|')[1];
                            msg = string.IsNullOrEmpty(objResponse.Result) ? string.Concat(status.ToUpper(), "\n", objResponse.ResultDescription.Split('|')[0]) : objResponse.ResultDescription.Split('|')[0];
                        }
                        if (Configuration.TransactionRequestCount < 3)
                        {
                            Configuration.TransactionRequestCount = Configuration.TransactionRequestCount + 1;
                            btnContinue_Click((object)OldtransactionNumber, null);
                            isFound = true;
                        }
                        else
                        {
                            logger.Error("Error while processing card. \r \n Please check if the Processor has already charged to avoid duplicate transaction further.  \r \n " + msg + "Card Processing Failed ");
                            POS_Core_UI.Resources.Message.Display("Error while processing card. \r \n Please check if the Processor has already charged to avoid duplicate transaction further.  \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                            isFound = false;
                            Configuration.TransactionRequestCount = 0;
                        }
                    }
                    else
                    {
                        msg = string.IsNullOrEmpty(objResponse.Result) ? string.Concat(status.ToUpper(), "\n", objResponse.ResultDescription) : objResponse.ResultDescription;
                        isFound = false;
                        POS_Core_UI.Resources.Message.Display("Error while processing card. \r \n Please check if the Processor has already charged to avoid duplicate transaction further.  \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    if (Tokenize == true)
                    {
                        if (CustomerCardInfo == null)
                        {
                            SubCardType = objResponse.CardType;
                        }
                        else
                        {
                            SubCardType = string.IsNullOrEmpty(CustomerCardInfo.cardType) ? objResponse.CardType : CustomerCardInfo.cardType;//PRIMEPOS-3081
                        }
                    }
                    else
                    {
                        if(isNBSPayment) //PRIMEPOS-3519 //PRIMEPOS-3504
                        {
                            SubCardType = "NB";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(objResponse.PaymentType) && objResponse.PaymentType.ToUpper().Equals("DEBIT"))
                            {
                                SubCardType = "Debit Card";
                            }
                            else
                            {
                                SubCardType = objResponse.CardType;
                            }
                        }
                    }

                    this.ApprovedPCCCardInfo.tRoutId = objResponse.TransId;
                    this.isFSATransaction = objResponse.isFSATransaction; //PRIMEPOS-3545
                    this.Amount = (decimal)objResponse.AmountApproved;
                    this.AuthNo = objResponse.AuthNo;
                    if (this.txtCCNo.Text.Trim().Length == 0 && objResponse != null)
                    {
                        this.txtCCNo.Text = objResponse.CardNo;
                    }
                    //NileshJ - ExpDate
                    if (this.txtExpDate.Text.Trim().Length == 0 && objResponse != null)
                    {
                        this.txtExpDate.Text = objResponse.Expiry;
                    }

                    isFound = true;
                }
            }

            #endregion

            return isFound;
        }
        private bool ELAVONResponseFound(string status, PccResponse objResponse)//2943
        {
            bool isFound = false;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(objResponse?.ResultDescription))
            {
                if (objResponse.ResultDescription.ToUpper() != "COMPLETE" && objResponse.ResultDescription.ToUpper() != "SUCCESS")
                {
                    msg = string.IsNullOrEmpty(objResponse.Result) ? status.ToUpper() : objResponse.ResultDescription;
                    this.ticketNum = null;
                    Resources.Message.Display(" Card was rejected. \r \n " + objResponse.Result + " \r \n " + msg, "Card Processing Failed ", MessageBoxButtons.OK);
                    isFound = false;
                    if (objResponse.ResultDescription.Contains("DECLINED"))
                    {
                        if (Resources.Message.Display(" Do you want to print the Denial receipt  ", " EVERTEC DENIAL RECEIPT ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //PRIMEPOS-2664
                            //string hostAddress = Configuration.CPOSSet.SigPadHostAddr.Split(':')[0];
                            //string portNo = Configuration.CPOSSet.SigPadHostAddr.Split(':')[1].Split('/')[0];

                            //EvertechProcessor evertechProcessor = EvertechProcessor.getInstance(hostAddress, Convert.ToInt32(portNo));
                            //string emvTag = evertechProcessor.EmvTag();
                            RxLabel oRxLabel = new RxLabel();
                            oRxLabel.isDenialReceipt = true;
                            oRxLabel.AuthNo = objResponse.AuthNo;
                            oRxLabel.TransactionID = objResponse.TransId;
                            oRxLabel.ResultDescription = objResponse.ResultDescription;
                            oRxLabel.Amount = objResponse.AmountApproved.ToString();
                            oRxLabel.EvertecDenialDate = DateTime.Now;
                            //oRxLabel.EmvTag = Regex.Replace(emvTag, "<br>", "\n", RegexOptions.IgnoreCase);//PRIMEPOS-2857
                            #region PRIMEPOS-2832 EVERTEC EBTBALANCE
                            //if (objResponse.EmvReceipt != null && objResponse.EmvReceipt?.EbtBalance?.Length >= 3)
                            //{
                            //    oRxLabel.FoodBalance = objResponse.EmvReceipt.EbtBalance.Split('|')[0];
                            //    oRxLabel.CashBalance = objResponse.EmvReceipt.EbtBalance.Split('|')[1];
                            //}
                            oRxLabel.EntryMethod = objResponse.EmvReceipt.EntryLegend;
                            #endregion
                            //if (!String.IsNullOrWhiteSpace(objResponse.EmvReceipt?.ResponseCode))
                            //{
                            //    oRxLabel.ResultDescription = objResponse.EmvReceipt?.ResponseCode;
                            //}
                            oRxLabel.PaymentProcessor = objResponse.PaymentProcessor.ToUpper();
                            if (objResponse?.EmvReceipt != null)
                            {
                                oRxLabel.MerchantID = objResponse.EmvReceipt.MerchantID.Trim();
                                if (!string.IsNullOrWhiteSpace(objResponse.EmvReceipt.ApplicationLabel))
                                {
                                    if (objResponse.EmvReceipt.ApplicationLabel.Contains("|"))
                                    {
                                        oRxLabel.ApplicationLabel = "AppLabel : " + objResponse.EmvReceipt.ApplicationLabel.Split('|')[0];
                                        oRxLabel.ApplicationLabel += "TC : " + objResponse.EmvReceipt.ApplicationLabel.Split('|')[1];
                                        oRxLabel.ApplicationLabel += "IAD : " + objResponse.EmvReceipt.ApplicationLabel.Split('|')[2];
                                    }
                                }
                                oRxLabel.Aid = objResponse.EmvReceipt.AppIndentifer;
                                //oRxLabel.InvoiceNumber = objResponse.EmvReceipt.InvoiceNumber;
                                //oRxLabel.TerminalID = Configuration.CPOSSet.TerminalID;
                                //oRxLabel.Batch = objResponse.EmvReceipt.BatchNumber;
                                oRxLabel.ReferenceNumber = objResponse.EmvReceipt.ReferenceNumber;
                            }
                            oRxLabel.Print();
                        }
                    }
                }
                else
                {
                    if (Tokenize == true)
                    {
                        if (CustomerCardInfo == null)
                        {
                            SubCardType = objResponse.CardType;
                        }
                        else
                        {
                            SubCardType = string.IsNullOrEmpty(CustomerCardInfo.cardType) ? objResponse.CardType : CustomerCardInfo.cardType;//PRIMEPOS-3081
                        }
                    }
                    else
                    {
                        SubCardType = objResponse.CardType;
                    }

                    if (this.isFSATransaction)//2943
                    {
                        this.PendingAmount = (decimal)objResponse.AmountApproved;
                    }
                    this.ApprovedPCCCardInfo.tRoutId = objResponse.TransId;
                    this.isFSATransaction = objResponse.isFSATransaction;
                    this.Amount = (decimal)objResponse.AmountApproved;
                    this.AuthNo = objResponse.AuthNo;
                    this.ExpirayDate = objResponse.Expiry;//2943
                    if (this.txtCCNo.Text.Trim().Length == 0 && objResponse != null)
                    {
                        this.txtCCNo.Text = objResponse.CardNo;
                    }
                    //NileshJ - ExpDate
                    if (this.txtExpDate.Text.Trim().Length == 0 && objResponse != null)
                    {
                        this.txtExpDate.Text = objResponse.Expiry;
                    }

                    isFound = true;
                }
            }
            return isFound;
        }
        private void ProcessDebitCardHPS(string ticketNumber, ref PccCardInfo cardInfo)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardHPS()", clsPOSDBConstants.Log_Entering);
            SigPadUtil.DefaultInstance.CaptureDebitPinBlock(cardInfo.cardNumber);
            if (this.oTransType == POSTransactionType.Sales)
            {
                if (SigPadUtil.DefaultInstance.PINBLOCK != string.Empty)
                {
                    cardInfo.pinNumber = SigPadUtil.DefaultInstance.PINBLOCK.ToString();
                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformDebitSale(ticketNumber, ref cardInfo);
                }
                else
                {
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardHPS()", clsPOSDBConstants.Log_Exiting);
                    return;
                }
            }
            else if (this.oTransType == POSTransactionType.SalesReturn)
            {
                if (SigPadUtil.DefaultInstance.PINBLOCK != string.Empty)
                {
                    cardInfo.pinNumber = SigPadUtil.DefaultInstance.PINBLOCK.ToString();
                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
                }
                else
                {
                    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardHPS()", clsPOSDBConstants.Log_Exiting);
                    return;
                }
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardHPS()", clsPOSDBConstants.Log_Exiting);
        }

        private void ProcessDebitCard(string ticketNumber, ref PccCardInfo cardInfo)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCard()", clsPOSDBConstants.Log_Entering);
            string pinNumber = string.Empty;
            string keySerialNumber = string.Empty;
            if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PinPadName.ToUpper().Trim() && Configuration.CPOSSet.UsePinPad == true)
            {
                frmPinPad.DefaultInstance.GetPinPadData(cardInfo.cardNumber, out pinNumber, out keySerialNumber);
                cardInfo.pinNumber = pinNumber;
                cardInfo.keySerialNumber = keySerialNumber;
            }
            else if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() && Configuration.CPOSSet.UsePinPad != true)
            {
                cardInfo.pinNumber = SigPadUtil.DefaultInstance.PINBLOCK.ToString();
            }
            cardInfo.cardType = PayTypes.DebitCard;

            if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() ? false : cardInfo.pinNumber == "" || cardInfo.keySerialNumber == "")
            {
                if (POS_Core_UI.Resources.Message.Display("Pin No. Or Key Serial No. is blank", "Pin Data Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                {
                    //Commented by SRT(Ritesh Parekh) Date: 27-Aug-2009
                    //PccPaymentSvr.DefaultInstance.ResponseStatus = "FAILURE";
                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).ResponseStatus = "FAILURE";
                    this.isCanceled = true;
                    this.Close();
                }
                else
                {
                    frmPinPad.DefaultInstance.GetPinPadData(cardInfo.cardNumber, out pinNumber, out keySerialNumber);
                    if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PinPadName.ToUpper().Trim() && Configuration.CPOSSet.PaymentProcessor == "HPS" && Configuration.CPOSSet.UsePinPad == true)
                    {
                        pinNumber = pinNumber + keySerialNumber;
                        cardInfo.pinNumber = pinNumber;
                    }
                    else
                    {
                        cardInfo.pinNumber = pinNumber;
                        cardInfo.keySerialNumber = keySerialNumber;
                    }
                    //Added By SRT(Gaurav) Date: 01-Dec-2008
                    //Mantis Id: 0000118
                    if (this.oTransType == POSTransactionType.Sales)
                    {
                        //Commented by SRT(Ritesh Parekh) Date: 27-Aug-2009
                        //PccPaymentSvr.DefaultInstance.PerformDebitSale(ticketNumber, ref cardInfo);
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformDebitSale(ticketNumber, ref cardInfo);
                    }
                    else if (this.oTransType == POSTransactionType.SalesReturn)
                    {
                        //Commented By SRT(Ritesh Parekh) Date: 27-Aug-2009
                        //PccPaymentSvr.DefaultInstance.PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
                    }
                    else if (this.oTransType == POSTransactionType.Reverse)
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformReverseOnCreditCardSale(ticketNumber, ref cardInfo);
                    }
                }
            }
            else
            {
                //Added By SRT(Gaurav) Date: 01-Dec-2008
                //Mantis Id: 0000118
                if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == PinPadName.ToUpper().Trim() && Configuration.CPOSSet.PaymentProcessor == "HPS" && Configuration.CPOSSet.UsePinPad == true)
                {
                    pinNumber = pinNumber + keySerialNumber;
                    cardInfo.pinNumber = pinNumber;
                }
                if (this.oTransType == POSTransactionType.Sales)
                {
                    //Commented By SRT(Ritesh Parekh) Date: 27-Aug-2009
                    //PccPaymentSvr.DefaultInstance.PerformDebitSale(ticketNumber, ref cardInfo);
                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformDebitSale(ticketNumber, ref cardInfo);
                }
                else if (this.oTransType == POSTransactionType.SalesReturn)
                {
                    //Commented By SRT(Ritesh Parekh) Date: 27-Aug-2009
                    //PccPaymentSvr.DefaultInstance.PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
                }
                else if (this.oTransType == POSTransactionType.Reverse)
                {
                    PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformReverseOnDebitCardSale(ticketNumber, ref cardInfo);
                }
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCard()", clsPOSDBConstants.Log_Exiting);
        }

        //Added By Dharmendra on March-27-09
        private void ProcessDebitCardForXLink(string ticketNumber, ref PccCardInfo cardInfo)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardForXLink()", clsPOSDBConstants.Log_Entering);
            string pinNumber = "DEMO";
            string keySerialNumber = "DEMO";
            cardInfo.pinNumber = pinNumber;
            cardInfo.keySerialNumber = keySerialNumber;
            cardInfo.cardType = PayTypes.DebitCard;
            if (this.oTransType == POSTransactionType.Sales)
            {
                //Commented By SRT(Ritesh Parekh) Date: 27-Aug-2009
                //PccPaymentSvr.DefaultInstance.PerformDebitSale(ticketNumber, ref cardInfo);
                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformDebitSale(ticketNumber, ref cardInfo);
            }
            else if (this.oTransType == POSTransactionType.SalesReturn)
            {
                //Commented By SRT(Ritesh Parekh) Date: 27-Aug-2009
                //PccPaymentSvr.DefaultInstance.PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardForXLink()", clsPOSDBConstants.Log_Exiting);
        }

        //Added Till Here March-27-09


        //PRIMEPOS-2528 (Suraj) 29-Mar-18 Bebit Processor HPSPAX
        private void ProcessDebitCardForPAX(string ticketNumber, ref PccCardInfo cardInfo)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardForPAX()", clsPOSDBConstants.Log_Entering);
            string pinNumber = "DEMO";
            string keySerialNumber = "DEMO";
            cardInfo.pinNumber = pinNumber;
            cardInfo.keySerialNumber = keySerialNumber;
            cardInfo.cardType = PayTypes.DebitCard;
            if (this.oTransType == POSTransactionType.Sales)
            {
                //Commented By SRT(Ritesh Parekh) Date: 27-Aug-2009
                //PccPaymentSvr.DefaultInstance.PerformDebitSale(ticketNumber, ref cardInfo);
                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformDebitSale(ticketNumber, ref cardInfo);
            }
            else if (this.oTransType == POSTransactionType.SalesReturn)
            {
                //Commented By SRT(Ritesh Parekh) Date: 27-Aug-2009
                //PccPaymentSvr.DefaultInstance.PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformReturnOnDebitCardSales(ticketNumber, ref cardInfo);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessDebitCardForPAX()", clsPOSDBConstants.Log_Exiting);
        }

        //Added by Manoj - EBT 8/24/2011
        private void ProcessEBTXLink(string ticketNumber, ref PccCardInfo cardInfo)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessEBTXLink()", clsPOSDBConstants.Log_Entering);
            string pinNumber = "DEMO";
            string keySerialNumber = "DEMO";
            cardInfo.pinNumber = pinNumber;
            cardInfo.keySerialNumber = keySerialNumber;
            cardInfo.cardType = PayTypes.EBT;

            if (this.oTransType == POSTransactionType.Sales)
            {
                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerfomEBTSale(ticketNumber, ref cardInfo);
            }
            else if (this.oTransType == POSTransactionType.SalesReturn)
            {
                PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformEBTReturn(ticketNumber, ref cardInfo);
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "ProcessEBTXLink()", clsPOSDBConstants.Log_Exiting);
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

        public bool PerformVoidTransaction(PccCardInfo pccCardInfo, POSTransactionType oTransType, string nbsTransId)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformVoidTransaction", clsPOSDBConstants.Log_Entering);
            bool retVal = false;
            string ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();
            //Added By Dharmendra on May-08-09
            // When a credit card payment is processed manually, we won't get troutid, in such case
            // the value in troutid in ppcCardInfo will remain blank in such case, there's no need
            // to pass this data to payment processor and forcebilly return the true value which will
            // indicate that the payment is voided
            if (pccCardInfo.PaymentProcessor.Trim().ToUpper().Equals("WORLDPAY"))
            {
                if (string.IsNullOrWhiteSpace(pccCardInfo.OrderID) && string.IsNullOrWhiteSpace(pccCardInfo.TransactionID))
                {
                    POS_Core_UI.Resources.Message.Display("Unable to reverse payment from POS!! \nPayment has to reversed from Online Merchant center", "Cancel Pay", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            else
            {
                if (pccCardInfo.tRoutId.Trim() == string.Empty)
                {
                    POS_Core_UI.Resources.Message.Display("This credit card payment was processed manually.\nEnsure this payment is also reversed externally.", "Cancel Pay", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }

            //Added Till here May-08-09
            if (oTransType == POSTransactionType.Sales)
            {
                if (pccCardInfo.cardType == PayTypes.CreditCard)
                {
                    //PccPaymentSvr.DefaultInstance.PerformVoidOnCreditCardSales(ticketNum,ref pccCardInfo);
                    if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.XLINK)
                        SigPadUtil.DefaultInstance.XlinkOnOff(On);
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo);
                    if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.XLINK)
                        SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                }
                else if (pccCardInfo.cardType == PayTypes.DebitCard)
                {
                    if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON)//2943
                    {
                        Resources.Message.Display("You cannot void a Debit transaction. Do full payment and refund it");
                        return false;
                    }
                    //PccPaymentSvr.DefaultInstance.PerformVoidOnDebitCardSales(ticketNum,ref pccCardInfo);
                    //PRIMEPOS-2503 Jenny New Device condition
                    if (((Configuration.CPOSSet.UseSigPad == true && (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() || Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == ISC480.ToUpper().Trim())) ||
                       Configuration.CPOSSet.PinPadModel.Contains(BlueToothICMP)) && Configuration.CPOSSet.PaymentProcessor == "XLINK")
                    {
                        //PccPaymentSvr.DefaultInstance.PerformVoidOnDebitCardSales(ticketNum,ref pccCardInfo);
                        SigPadUtil.DefaultInstance.XlinkOnOff(On);
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformReturnOnDebitCardSales(ticketNum, ref pccCardInfo);
                        SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                    }
                    else if (Configuration.CPOSSet.UseSigPad == true && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PaymentProcessor == "HPS")
                    {
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformReverseOnDebitCardSale(ticketNum, ref pccCardInfo);
                    }
                    else
                    {
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnDebitCardSales(ticketNum, ref pccCardInfo);
                    }
                }
                else if (pccCardInfo.cardType == PayTypes.EBT)
                {
                    if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.XLINK)
                        SigPadUtil.DefaultInstance.XlinkOnOff(On);
                    if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")//2664
                    {
                        Resources.Message.Display(" EBT cannot be voided. \r\n Please complete the transaction and then do a refund.", " EBT MESSAGE");
                    }
                    if (Configuration.CPOSSet.PaymentProcessor == "ELAVON")//2943
                    {
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).IsElavonEbtvoid = true;
                    }
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformEBTReturn(ticketNum, ref pccCardInfo);
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).IsElavonEbtvoid = false;//2943
                    if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.XLINK)
                        SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                }
                #region PRIMEPOS-3373
                else if (pccCardInfo.cardType == PayTypes.NBS)
                {
                    pccCardInfo.PaymentProcessor = "VANTIV";
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnNBSCardSales(ticketNum, ref pccCardInfo);
                    if (pccCardInfo.status.ToUpper() == "SUCCESS")
                    {
                        NBSProcessor nbsProc = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                        VoidData voidData = nbsProc.VoidRequest(nbsTransId);
                        if (voidData != null && voidData.Response.Code == "000")
                        {
                            logger.Trace("The NBS transaction void successfully.");
                        }
                        else
                        {
                            logger.Trace("The error is occurring while attempting to retrieve the response for the NBSVoid request");
                        }
                    }
                }
                #endregion
                else if (pccCardInfo.PaymentProcessor.Trim().ToUpper().Equals("WORLDPAY"))
                {
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnWP(FirstMile.TransactionType.Void, ref pccCardInfo); 
                }
            }
            else if (oTransType == POSTransactionType.SalesReturn)
            {
                if (pccCardInfo.cardType == PayTypes.CreditCard)
                {
                    //PccPaymentSvr.DefaultInstance.PerformVoidOnCreditCardSalesReturn(ticketNum,ref pccCardInfo);
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo);
                }
                else if (pccCardInfo.cardType == PayTypes.DebitCard)
                {
                    //PccPaymentSvr.DefaultInstance.PerformVoidOnDebitCardSalesReturn(ticketNum,ref pccCardInfo);
                    if (Configuration.CPOSSet.UseSigPad == true && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PaymentProcessor == "XLINK")
                    {
                        SigPadUtil.DefaultInstance.XlinkOnOff(On);
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformReturnOnDebitCardSales(ticketNum, ref pccCardInfo);
                        SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                    }
                    else
                    {
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnDebitCardSalesReturn(ticketNum, ref pccCardInfo);
                    }
                }
                #region PRIMEPOS-3427
                else if (pccCardInfo.cardType == PayTypes.NBS)
                {
                    pccCardInfo.PaymentProcessor = "VANTIV";
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnNBSCardSales(ticketNum, ref pccCardInfo);
                    if (pccCardInfo.status.ToUpper() == "SUCCESS")
                    {
                        NBSProcessor nbsProc = new NBSProcessor(Configuration.CSetting.NBSUrl, Configuration.CSetting.NBSToken);
                        VoidData voidData = nbsProc.VoidRequest(nbsTransId);
                        if (voidData != null && voidData.Response.Code == "000")
                        {
                            logger.Trace("The NBS transaction void successfully.");
                        }
                        else
                        {
                            logger.Trace("The error is occurring while attempting to retrieve the response for the NBSVoid request");
                        }
                    }
                }
                #endregion
            }
            else
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformVoidTransaction", clsPOSDBConstants.Log_Exiting);
                return retVal;
            }

            //String responseStatus = PccPaymentSvr.DefaultInstance.ResponseStatus;
            String responseStatus = PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).ResponseStatus;

            if (responseStatus.ToUpper().Trim() == "SUCCESS")
            {
                retVal = true;
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformVoidTransaction", clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        public bool PerformReverseTransaction(PccCardInfo pccCardInfo, POSTransactionType oTransType)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformReverseTransaction", clsPOSDBConstants.Log_Entering);
            bool retVal = false;
            string ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();
            if (pccCardInfo.tRoutId.Trim() == string.Empty)
            {
                POS_Core_UI.Resources.Message.Display("This credit card payment was processed manually.\nEnsure this payment is  reversed externally.", "Reverse Pay", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            if (oTransType == POSTransactionType.Sales)
            {
                if (pccCardInfo.cardType == PayTypes.CreditCard)
                {
                    SigPadUtil.DefaultInstance.ShowCustomScreen("Credit Transaction is being reversed.");
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformReverseOnCreditCardSale(ticketNum, ref pccCardInfo);
                }
                else if (pccCardInfo.cardType == PayTypes.DebitCard)//Added by Manoj to Return Debit transaction if the user cancel Partial Payment. (Requirement by xcharge)
                {
                    if (Configuration.CPOSSet.UseSigPad == true && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PaymentProcessor == "XLINK")
                    {
                        SigPadUtil.DefaultInstance.XlinkOnOff(On);
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformReturnOnDebitCardSales(ticketNum, ref pccCardInfo);
                        SigPadUtil.DefaultInstance.XlinkOnOff(Off);
                    }
                    else if (Configuration.CPOSSet.PinPadModel.ToUpper().Trim() != DeviceName.ToUpper().Trim())
                    {
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformReverseOnDebitCardSale(ticketNum, ref pccCardInfo);
                    }
                    else if (Configuration.CPOSSet.UseSigPad == true && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim() && Configuration.CPOSSet.PaymentProcessor == "HPS")
                    {
                        SigPadUtil.DefaultInstance.ShowCustomScreen("Debit Transaction is being reversed.");
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformReverseOnDebitCardSale(ticketNum, ref pccCardInfo);
                    }
                }
                else if (pccCardInfo.cardType == PayTypes.EBT)
                {
                    if (Configuration.CPOSSet.UseSigPad == true && Configuration.CPOSSet.PinPadModel.ToUpper().Trim() == DeviceName.ToUpper().Trim())
                    {
                        SigPadUtil.DefaultInstance.ShowCustomScreen("EBT Transaction is being reversed.");
                    }
                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformEBTReturn(ticketNum, ref pccCardInfo);
                }
            }
            else
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformReverseTransaction", clsPOSDBConstants.Log_Exiting);
                return retVal;
            }
            String responseStatus = PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).ResponseStatus;

            if (responseStatus.ToUpper().Trim() == "SUCCESS")
            {
                retVal = true;
            }
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "PerformReverseTransaction", clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        private void frmPOSProcessCC_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Added By SRT(Ritesh Parekh) Date : 22-Jul-2009
            //This variable will hold old processor name and reassign at end of flow.
            SetOldProcessor();
            //End Of Added By (Ritesh Parekh)
            //Added by Ritesh(SRT)
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
        }

        public bool ValidFormet()
        {
            if (txtCCNo.Text.Length < 15)
            {
                return false;
            }
            else
            {
                string validFormet = txtCCNo.Text.Substring(0, 11);
                string sLastDigit = txtCCNo.Text.Substring(12);
                if (txtCCNo.Text.Substring(0, 12) == "XXXXXXXXXXXX" && (txtCCNo.Text.Substring(12) != string.Empty || txtCCNo.Text.Substring(12) != null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void txtCCNo_TextChanged(object sender, EventArgs e)
        {
            if (txtCCNo.Text.Length <= 1)
            {
                txtCCNo.PasswordChar = '\0'; //added by Manoj 9/5/2013 Task #1324
            }
            /*
            try
            {
                if (e.Equals((char)Keys.Back))
                {
                    txtCCNo.Text = string.Empty;
                    return;
                }
                if (ValidFormet())
                {
                    return;
                }
                if (txtCCNo.Text != null || txtCCNo.Text != string.Empty)
                {
                    string val = string.Empty;
                    sCardNumber = txtCCNo.Text;
                    int iLength = txtCCNo.Text.Length;
                    if (iLength > 0)
                    {
                        if (txtCCNo.Text.StartsWith("F"))
                        {
                            sStartIndex = 1;
                            sMaxLength = 13;
                            bISFourceTrans=true;
                        }
                        for (int counter = 0; counter < txtCCNo.Text.Length; counter++)
                        {
                            if (txtCCNo.Text.Length >= sStartIndex && counter < sMaxLength)
                            {
                                val = val + "X";
                            }
                            else
                            {
                                string slastval = txtCCNo.Text.Substring(sMaxLength);
                                if (bISFourceTrans)
                                {
                                    txtCCNo.Text = "F"+val + slastval;
                                    break;
                                }
                                else
                                {
                                    txtCCNo.Value = val + slastval;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
             * */
        }

        private void txtCVVCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        #region Sprint-27 - PRIMEPOS-2301 07-Sep-2017 JY Added
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
        #endregion

        private void txtCCNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
          string   sCardno = e.KeyChar.ToString();
            try
            {
                //if(e.KeyChar==(char)key
                if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete)
                {
                    e.Handled = false;
                    return;
                }
                if (sCardno != string.Empty || sCardno == null)
                {
                    sCardNumber = sCardNumber + sCardno;
                    if (e.KeyChar == (char)Keys.F)
                    {
                        sStartIndex = 1;
                        sMaxLength = 13;
                        return;
                    }
                    else
                    {
                        //sStartIndex = 0;
                        //sMaxLength = 12;
                        if (Convert.ToInt32(txtCCNo.Text.Length) >= sStartIndex && Convert.ToInt32(txtCCNo.Text.Length) < sMaxLength)
                        {
                            //val += Convert.ToInt32(ultraTextEditor1.Text.ToString());
                            e.KeyChar = 'X';
                        }
                        else
                        {
                            ;
                            //ultraTextEditor1.PasswordChar = char.MinValue;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
             * */
        }

        private void txtCCNo_Leave(object sender, EventArgs e)
        {
            if (txtCCNo.Text.Length > 11)
            {
                txtCCNo.PasswordChar = 'X'; //added by Manoj 9/5/2013 Task# 1324
            }
        }

        //PRIMEPOS-2528 (Suraj) 1-june-18 Added for HPSPAX Transaction cancellation (Multithreaded)
        private void ProcessTransactionThread(string cardType, string transType, string ticketNum, ref PccCardInfo cardInfo)
        {
            switch (cardType)
            {
                case PayTypes.CreditCard:
                    if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreRead)) //PRIMEPOS-3526 //PRIMEPOS-3504
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformPreRead(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreReadSale)) //PRIMEPOS-3526 //PRIMEPOS-3504
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformPreReadSale(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.Cancel))  //PRIMEPOS-3526 //PRIMEPOS-3504
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformPreReadCancelTrans(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.Sales))
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSale(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.SalesReturn)) //PreReadSaleReturn
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformCreditSalesReturn(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreReadSaleReturn)) //PRIMEPOS-3521 PRIMEPOS-3522 //PRIMEPOS-3504
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformPreReadSalesReturn(ticketNum, ref cardInfo);
                    }
                    break;
                case PayTypes.DebitCard:
                    ProcessDebitCardForPAX(ticketNum, ref cardInfo);
                    break;
                case PayTypes.EBT:
                    if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.Sales))
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerfomEBTSale(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.SalesReturn))
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformEBTReturn(ticketNum, ref cardInfo);
                    }
                    break;
                case PayTypes.NBS:
                    #region PRIMEPOS-3372
                    if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.PreRead))
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformNBSPreRead(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.Sales))
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformNBSSale(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.Cancel))
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformPreReadCancel(ticketNum, ref cardInfo);
                    }
                    else if (transType == Enum.GetName(typeof(POSTransactionType), POSTransactionType.SalesReturn))
                    {
                        PccPaymentSvr.GetProcessorInstance(pmtProcessMethod).PerformNBSSalesReturn(ticketNum, ref cardInfo);
                    }
                    break;
                    #endregion
            }

        }
        private bool TouchScreen_CaptureSignatureCC()//RXHeader oRXHeader)
        {
            logger.Trace("TouchScreen_CaptureSignatureCC() - " + clsPOSDBConstants.Log_Entering);
            bool retVal = true;

            string patientCounceling = string.Empty;
            try
            {
                SigType = "";
                strSignature = string.Empty;
                binSignature = null;

                frmDrawSignInWPF ofrmDrawSignInWPF = new frmDrawSignInWPF();
                ofrmDrawSignInWPF.eCalledFromScreen = eCalledFrom.CreditCardSign;
                bool? rtn = ofrmDrawSignInWPF.ShowDialog();

                if (ofrmDrawSignInWPF.DialogResult.Equals(true))
                {
                    SigType = clsPOSDBConstants.BINARYIMAGE;
                    binSignature = ofrmDrawSignInWPF.imgData;
                    retVal = true;
                }
                logger.Trace("TouchScreen_CaptureSignatureCC() - " + clsPOSDBConstants.Log_Exiting);

            }
            catch (Exception exp)
            {
                retVal = false;
                logger.Fatal(exp, "TouchScreen_CaptureSignatureCC()");
            }
            return retVal;
        }

        private bool SkipEMVCardSign(bool isEMVCard)
        {
            bool skip = false;
            if (isEMVCard && Configuration.CPOSSet.SkipEMVCardSign)// (Convert.ToInt32(Amount) <= 20 && Configuration.CPOSSet.SkipAmountSign)
            {
                skip = true;
            }
            return skip;
        }
    }
}