using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using System.Diagnostics;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using Infragistics.Win.UltraWinGrid;
using NLog;
using Infragistics.Win.UltraWinMaskedEdit;
using System.Drawing.Imaging;
using Infragistics.Win;
using POS_Core_UI.Layout;
using System.Timers;
using POS_Core.TransType;
using POS_Core.Resources.PaymentHandler;
using POS_Core.Resources;
using POS_Core_UI.UI;
using Solutran;//PRIMEPOS-2663 - Solutran
using Resources;
using Evertech;//PRIMEPOS-2664
using Evertech.Data;
using PossqlData;
using POS_Core.DataAccess;
using System.Collections;
using NBS; //PRIMEPOS-3372
using NBS.RequestModels; //PRIMEPOS-3372

namespace POS_Core_UI
{
    public partial class frmPOSPayTypesList : frmTransactionLayout
    {

        #region UI Objects Declared

        private IContainer components;

        #endregion UI Objects Declared

        #region variable declaration
        public bool IsElavonTax = false;//2943
        public string ElavonTotalTax = string.Empty;//2943
        public POSPayTypeList oPosPTList = new POSPayTypeList();
        frmPOSTransaction frmPOSTransaction = new frmPOSTransaction();
        private string TransNo = string.Empty;//PRIMEPOS-2887
        private PccCardInfo pccCardInfo = null;
        private Customer oCustomer = new Customer();
        private CustomerData oCustData = new CustomerData();
        private string[] NBS_SALES_TYPE = { "SALES", "VAT", "GST", "EXCISE", "IMPORT_DUTY", "LUXURY" }; //PRIMEPOS-3372

        public decimal PendingAmount = 0;//2943
        public bool IsFsaTransaction = false;//2943
        public string ExpiryDate = "";//2943
        public string NBSTransID = string.Empty;//PRIMEPOS-3375
        public string NBSUid = string.Empty;//PRIMEPOS-3375
        public string NBSPaymentType = string.Empty;//PRIMEPOS-3375
        public bool IsNBSTransaction = false;//PRIMEPOS-3526 //PRIMEPOS-3504
        public bool IsNBSTransDoneOnce = false;//PRIMEPOS-3524 //PRIMEPOS-3504

        //private POSTransPaymentData oPOSTransPaymentData;
        //public  POSTransPaymentData oPOSTransPaymentDataPaid;
        private Form frmNumPad = null;
        private DataTable dtChargeAccount;
        public frmPOSTransaction parentForm = null;
        private List<char> pressedKeysList = new List<char>();
        private DateTime lastKeyPressedDateTime = DateTime.Now;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        float widthRatio = 0;
        float heightRatio = 0;
        public int TransID = 0;
        public bool isTransOnHold = false; //PRIMEPOS-3283
        private string LastClickPaytype = string.Empty;
        private string paymentType = string.Empty; //Added By Dharmendra(SRT)       
        private string sNewAddedPayTypesID = string.Empty;
        private string sDeliveryAddress = string.Empty; //Added by shitaljit to get delivery address
        private bool bPrintGiftReciept = false; //Added by Shitaljit for printing gift reciept on 8 jan 2013

        //Added For HPS Intigration
        private bool bIsReverse = false;
        public readonly Decimal totalNonIIASAmount; //Added by Prog1
        private bool bPaymentRowAdded = false;  //PRIMEPOS-2499 29-Mar-2018 JY Added
        private bool bAllowCheckPayment = false;    //PRIMEPOS-2539 09-Jul-2018 JY Added
        System.Timers.Timer tmrBlinking;    //PRIMEPOS-2611 15-Nov-2018 JY Added
        private long iBlinkCnt = 0;  //PRIMEPOS-2611 15-Nov-2018 JY Added
        public String ticketNum = null; // NileshJ - 20-Dec-2018
        string TransTypeCode = string.Empty;//PRIMEPOS-3087
        #region PRIMEPOS-2663 - Solutran NileshJ
        POSTransaction posTrans = null;
        string s3TransID = string.Empty;
        string ProcessTransID = string.Empty;
        string Amount = string.Empty;

        string TransDate = string.Empty;//2943

        public const string S3_Success = "000";
        public const string Invalid_S3_Merchant_ID = "109";
        public const string Invalid_CVV = "980";
        public const string No_Benefits = "BEN";
        public const string Card_Not_Active = "114";
        public const string Format_Error = "307";
        public const string Invalid_Card = "111";
        public const string Duplicate_TRX = "DUP";
        public const string Invalid_TRX_ID = "165";
        public const string Unmatched_Transaction_ID = "914";
        public const string Current_APL_Expired = "CAP";
        public const string Both_APLs_Expired = "BAP";
        public const string Both_APLs_Good = "GAP";
        public const string Process_but_suspend_settlement = "164";
        public const string Transaction_Return_not_available = "168";
        public const string Product_not_found_in_Original_Transaction = "170";
        public const string Unknown_Error_Contact_Solutran = "913";
        public const string Requires_manual_exception_handling_Card_Activated = "CAT";
        public const string Card_Activation_Denied = "CAD";
        public const string No_Discount_Available = "DIS";
        public const string Invalid_API_Key = "401";
        public const string Endpoint_Not_Found = "404";
        public const string Expired_Card = "101";
        public const string Lost_Stolen_Card = "132";
        public bool IsSolutranException = false; // Revrsal //PRIMEPOS-2887
        #endregion

        private string strDefaultCreditCardPayTypeId = "4";   //PRIMEPOS-2724 22-Aug-2019 JY Added 
        private bool bExecuteOnce = false;  //PRIMEPOS-2726 28-Aug-2019 JY Added
        private bool bOverPay = false; //PRIMEPOS-2763 26-Nov-2019 JY Added

        // Added for Evertec - PRIMEPOS - 2664 Arvind        
        public const string EVERTEC = "EVERTEC";

        #region StoreCredit PRIMEPOS-2747 NileshJ
        StoreCreditData oStoreCreditData = new StoreCreditData();
        StoreCredit oStoreCredit = new StoreCredit();
        private StoreCreditDetailsData oStoreCreditDetailsData = new StoreCreditDetailsData();
        private StoreCreditRow oStoreCreditRow;
        private StoreCreditDetails oStoreCreditDetails = new StoreCreditDetails();
        #endregion

        //PRIMEPOS-2664 
        EvertechProcessor evertecProcessor = null;
        public string controlNumber = string.Empty;

        public bool IsCustomerDriven = false;//2915
        int tokenID = 0;//3009
        public bool IsStatusPending = false;//2915
        public string PrimeRxPayTransID = string.Empty;

        //2857
        public string EvertecTaxDetails = string.Empty;
        public decimal CashBackAmount = 0;//PRIMEPOS-2857
        public List<AnalyzeLineItem> lineItemsdata = new List<AnalyzeLineItem>(); //PRIMEPOS-3372

        public bool IsFondoUnica = false;//PRIMEPOS-2664

        #endregion variable declaration

        #region PRIMEPOS-2738
        string ReversedAmount = string.Empty;
        string TransPayID = string.Empty;
        public DataSet oOrigPayTransData = new DataSet();
        public DataSet oShowTransData = new DataSet();
        public String transType = string.Empty;
        Decimal reversedAmount;
        public POSTransaction oPosTrans = null;
        string HrefNumber = string.Empty;
        string nbsUid = string.Empty; //PRIMEPOS-3375
        string nbsType = string.Empty; //PRIMEPOS-3375
        public bool isStrictReturn = false;
        #endregion

        public POSTransPaymentData oPOSTransPaymentData = new POSTransPaymentData();//2915

        #region PRIMEPOS-2915
        public string Email = string.Empty;
        public string Mobile = string.Empty;
        public int TransactionProcessingMode;
        #endregion

        DataTable dtTransDetail = new DataTable();  //PRIMEPOS-2836 15-Apr-2020 JY Added
        string OverrideHousechargeLimitUser = string.Empty;   //PRIMEPOS-2402 09-Jul-2021 JY Added
        string strMaxTenderedAmountOverrideUser = string.Empty; //PRIMEPOS-2402 20-Jul-2021 JY Added
        #region Properties

        //Added by Shitaljit for printing gift reciept on 8 jan 2013
        public bool PrintGiftReciept
        {
            get
            {
                return bPrintGiftReciept;
            }
        }

        public bool IsDelivery
        {
            get
            {
                return oPosPTList.bIsDelivery;
            }
        }

        public string DeliveryAddress
        {
            get
            {
                return sDeliveryAddress;
            }
            set
            {
                sDeliveryAddress = value;
            }
        }

        public decimal TotalEBTAmount
        {
            get
            {
                return oPosPTList.totalEBTAmount;
            }
            set
            {
                oPosPTList.totalEBTAmount = value;
            }
        }

        //Added by shitaljit on 26Dec2013 for PRIMEPOS-1627 Remove Tax on EBT Transaction
        public decimal TotalEBTItemsTaxAmount
        {
            get
            {
                return oPosPTList.totalEBTItesTaxAmount;
            }
            set
            {
                oPosPTList.totalEBTItesTaxAmount = value;
            }
        }
        //end

        public DataTable ChargeAccount
        {
            set
            {
                this.dtChargeAccount = value;
            }
        }

        public CLCardsRow CLCard
        {
            set
            {
                oPosPTList.oCLCardRow = value;
            }
        }


        public frmPOSTransaction ParentForm
        {
            get
            {
                return parentForm;
            }
            set
            {
                parentForm = value;
            }
        }

        //public POSTransPayment_CCLogList POSTransPaymentList_CCLogList
        //{
        //    get { return oPOSTransPayment_CCLogList; }
        //}

        public override string ToString()
        {
            string strData = string.Empty;
            strData += "\n\tIs ROA : " + oPosPTList.isROA.ToString();
            strData += "\n\tTotal Amount : " + oPosPTList.totalAmount.ToString();
            strData += "\n\tTotal IIAS Amount : " + oPosPTList.totalIIASAmount.ToString();
            strData += "\n\tTotal Non IIAS Amount : " + this.totalNonIIASAmount.ToString();
            strData += "\n\tTotal IIAS Rx Amount : " + oPosPTList.totalIIASRxAmount.ToString();
            strData += "\n\tTotal IIAS Non Rx Amount : " + oPosPTList.totalIIASNonRxAmount.ToString();
            strData += "\n\tCancel Transaction : " + oPosPTList.CancelTransaction.ToString();

            //Data Binding ??
            strData += "\n\tText Box Amount Total : " + this.txtAmtTotal.Text;
            strData += "\n\tText Box IIAS Amount Total : " + this.txtAmtTotalIIAS.Text;
            strData += "\n\tText Box Non IIAS Amount Total : " + this.txtAmtTotalNonIIAS.Text;
            strData += "\n\tText Box IIAS Rx Amount Total : " + this.txtAmtTotalIIASRx.Text;
            strData += "\n\tText Box EBT Amount Total : " + this.lblAmtTotalEBT.Text;

            strData += "\n\tTax Amount : " + oPosPTList.totalTaxAmt;
            strData += "\n\tTransaction Type Amount : " + oPosPTList.oTransactionType;
            strData += "\n\tSig Pad Trans Id : " + oPosPTList.sSigPadTransID.ToString();
            strData += "\n\tPay Type : " + oPosPTList.oPayTpes;
            return strData;
        }

        public bool Tokenize
        {
            get
            {
                return oPosPTList._Tokenize;
            }
            set
            {
                oPosPTList._Tokenize = value;
            }
        }

        #region StoreCredit - PRIMEPOS-2747 - NileshJ
        public int CustomerTag
        {
            get;
            set;
        }
        public string CustomerText
        {
            get;
            set;
        }
        public string CustomerName
        {
            get;
            set;
        }
        public bool IsStoreCredit
        {
            get
            {
                return oPosPTList.IsStoreCredit;
            }
            set
            {
                oPosPTList.IsStoreCredit = value;
            }
        }

        #endregion
        #endregion Properties

        #region Form events

        #region Constructor
        // NileshJ - SoluTran - Added POSTransaction - PRIMEPOS-2663
        // NileshJ BatchDelivery - Add bool IsBatchDelivery, decimal BatchDelCopyCollecedAmount PRIMERX-7688 - 23-Sept-2019
        public frmPOSPayTypesList(System.Decimal Total, POS_Core.TransType.POSTransactionType oTranType, System.Decimal TaxAmt, bool isReceiveOnAccount, string sPadID, decimal dIIASAmount, string sCustomerName, Boolean bIsCustomerTokenExists, POSTransaction oPOSTrans, bool IsBatchDelivery, decimal BatchDelTotalPaidAmount)   //PRIMEPOS-2611 13-Nov-2018 JY Added bIsCustomerTokenExists
            : this(Total, oTranType, TaxAmt, isReceiveOnAccount, sPadID, dIIASAmount, 0, sCustomerName, bIsCustomerTokenExists, oPOSTrans, IsBatchDelivery, BatchDelTotalPaidAmount)
        {
            oPosPTList.ClCouponAmount = 0;
            oPosPTList.allowCouponPayment = false;
            Tokenize = false;
        }

        // NileshJ - SoluTran - Added POSTransaction - PRIMEPOS-2663 
        // NileshJ BatchDelivery - Add bool IsBatchDelivery, decimal BatchDelCopyCollecedAmount PRIMERX-7688 23-Sept-2019
        public frmPOSPayTypesList(System.Decimal Total, POS_Core.TransType.POSTransactionType oTranType, System.Decimal TaxAmt, bool isReceiveOnAccount, string sPadID, decimal dIIASAmount, decimal dIIASRxAmount, string sCustomerName, Boolean bIsCustomerTokenExists, POSTransaction oPOSTrans, bool IsBatchDelivery, decimal BatchDelTotalPaidAmount)    //PRIMEPOS-2611 13-Nov-2018 JY Added bIsCustomerTokenExists
        {
            InitializeComponent();
            posTrans = oPOSTrans;// NileshJ - Added For SoluTran - PRIMEPOS-2663
            oPosTrans = oPOSTrans;// Arvind - Added For Reversal - PRIMEPOS-2738
            oPosPTList.isROA = isReceiveOnAccount;
            oPosPTList.totalAmount = Total;
            oPosPTList.totalIIASAmount = dIIASAmount;
            this.totalNonIIASAmount = oPosPTList.totalAmount - oPosPTList.totalIIASAmount;
            oPosPTList.totalIIASRxAmount = dIIASRxAmount;
            oPosPTList.totalIIASNonRxAmount = dIIASAmount - dIIASRxAmount;
            oPosPTList.CancelTransaction = false;

            //Data Binding????
            txtAmtTotal.Text = oPosPTList.totalAmount.ToString("###########0.00");
            txtAmtTotalIIAS.Text = oPosPTList.totalIIASAmount.ToString("###########0.00");
            txtAmtTotalNonIIAS.Text = this.totalNonIIASAmount.ToString("###########0.00");
            txtAmtTotalIIASRx.Text = oPosPTList.totalIIASRxAmount.ToString("###########0.00");

            oPosPTList.tenderedAmount = 0.0M;
            oPosPTList.totalTaxAmt = TaxAmt;
            oPosPTList.oTransactionType = oTranType;
            oPosPTList.sSigPadTransID = sPadID;
            oPosPTList.oPayTpes = "";
            oPosPTList.sTransactionType = oTranType.ToString();
            if (oTranType == POS_Core.TransType.POSTransactionType.Sales)
            {
                btnReverse.Enabled = true;
            }
            oPosPTList.allowCouponPayment = false;
            if (sCustomerName.Trim().Length > 0)
            {
                Text += " [Customer: " + sCustomerName + "]";
                lblCustomerName.Text = sCustomerName;

                if (bIsCustomerTokenExists == true)
                    lblCustomerName.Appearance.Image = Properties.Resources.CreditCard;
                else
                    lblCustomerName.Appearance.Image = null;
            }
            oPosPTList.ClCouponAmount = 0;
            Tokenize = false;

            #region PRIMEPOS-2611 15-Nov-2018 JY Added
            tmrBlinking = new System.Timers.Timer();
            tmrBlinking.Interval = 1000;//1 seconds
            tmrBlinking.Elapsed -= new ElapsedEventHandler(tmrBlinkingTimedEvent);
            tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
            tmrBlinking.Enabled = false;
            if (bIsCustomerTokenExists)
            {
                tmrBlinking.Enabled = true;
            }
            else
            {
                tmrBlinking.Enabled = false;
                iBlinkCnt = 0;
            }
            #endregion

            #region BatchDelivery - NileshJ - PRIMERX-7688  23-Sept-2019 
            if (IsBatchDelivery)
            {
                oPosPTList.IsBatchDelivery = IsBatchDelivery;
                oPosPTList.BatchDelTotalPaidAmount = BatchDelTotalPaidAmount; //BatchDelTotalPaidAmount is the amount of Copay Collected at Delivery Order level by Delivery App.
            }
            #endregion
            dtTransDetail = posTrans.oTransDData.TransDetail.Copy();    //PRIMEPOS-2836 15-Apr-2020 JY Added
        }
        #endregion Constructor

        private void frmPOSPayTypesList_Activated(object sender, EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void frmPOSPayTypesList_Load(object sender, EventArgs e)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "frmPOSPayTypesList_Load()", clsPOSDBConstants.Log_Entering);        
            logger.Trace("frmPOSPayTypesList_Load() - " + clsPOSDBConstants.Log_Entering);
            ApplyGridFormat();
            populatePayType();
            if (!this.IsCustomerDriven)//2915
            {
                oPosPTList.oPOSTransPaymentDataPaid = new POSTransPaymentData();
                grdPaymentComplete.DataSource = oPosPTList.oPOSTransPaymentDataPaid;
            }
            else
            {
                string paytype = string.Empty;
                ProcessTransactionPaymentClogCustomerDriven(this.oPOSTransPaymentData, ref paytype);
                grdPaymentComplete.DataSource = this.oPOSTransPaymentData;
                if (isTransOnHold) //PRIMEPOS-3345
                {
                    oPosPTList.oPOSTransPaymentDataPaid = this.oPOSTransPaymentData;
                }
            }
            //if (oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            //{
            //    this.oPOSTransPaymentDataPaid = GetCompletedPay();
            //}
            #region BatchDelivery - NileshJ - PRIMERX-7688 23-Sept-2019
            if (oPosPTList.IsBatchDelivery && oPosPTList.BatchDelTotalPaidAmount != 0)
            {
                ProcessAlreadyPaidAmount(); //If there is valid amount that was collected then only process the amount else if 0 no need.
            }
            #endregion

            this.IsCustomerDriven = false;//2915

            Left = 1; //( frmMain.getInstance().Width-this.Width)/2;
            Top = 20; //(frmMain.getInstance().Height-this.Height)/2;
            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Hidden = true;
            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_SigType].Hidden = true;
            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].Hidden = true;
            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].Hidden = true;
            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_ExpDate].Hidden = true;
            for (int i = 19; i <= 75; i++)//Changed from 36 to 42 for Evertec Added by Arvind PRIMEPOS-2664 //CHANGED FROM 42 TO 47 PRIMEPOS-2636 BY ARVIND // PRIMEPOS-2761 Changed 47 to 48 //CHANGED FROM 48 TO 54 PRIMEPOS-2786 AND PRIMEPOS-2664//Changed from 54 to 60 2915//2664//2990//3009 //PRIMEPOS-3117 11-Jul-2022 JY changed 69 to 70 //PRIMEPOS-3145 28-Sep-2022 JY 70 to 71 //PRIMEPOS-3375 71 to 74
            {
                this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[i].Hidden = true;
            }
            #region PRIMEPOS-2556 03-Jul-2018 JY Added
            Top = Left = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
            //SetFontSize(this, this.Width, this.Height, (decimal)0.64);
            SetFontSize(this, this.Width, this.Height, 1); //PrimePOS-2523 Added by Farman Ansari on 24 May 2018
            #endregion
            if (oPosPTList.oTransactionType != POS_Core.TransType.POSTransactionType.Sales)
            {
                #region PRIMEPOS-2860 12-Jun-2020 JY Added logic to show Sales Payment History screen
                //check if it is sales return then make it visible, use it to bring up Sales Payment History screen
                if (Configuration.convertNullToInt(oPosTrans.oTransHRow.ReturnTransID) > 0)
                {
                    btnCashBack.Text = "Sales Payment History";
                }
                #endregion
                else
                {
                    lblF5.Visible = false;
                    btnCashBack.Visible = false;
                }
            }

            #region BatchDelivery - NileshJ - PRIMERX-7688 23-Sept-2019
            if (oPosPTList.IsBatchDelivery)
            {
                oPosPTList.amountBalanceDue = oPosPTList.totalAmount - oPosPTList.BatchDelTotalPaidAmount;  //There will be already a Cash type PaymentRecord alredy inserted which will be deducted from total amount
            }
            else
            {
                oPosPTList.amountBalanceDue = oPosPTList.totalAmount;
            }
            #endregion
            this.txtAmtBalanceDue.Text = oPosPTList.amountBalanceDue.ToString("##########0.00").ToString();//Data Binding??
            this.lblAmtTotalEBT.Text = oPosPTList.totalEBTAmount.ToString("###########0.00");//Data Binding??

            if (oPosPTList.totalIIASAmount == 0)
            {
                tableLayoutPanel8.Visible = false;
                lblTotNonIIASAmt.Visible = false;
                txtAmtTotalNonIIAS.Visible = false;
            }

            clsUIHelper.CurrentForm = this;
            if (Configuration.showNumPad == true)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Application.DoEvents();
                //clsUIHelper.CurrentForm = this; Moved out of the block
                //SetEnabledLoad(this, true);
                ShowNumericPad();

            }

            // show form and set current window active
            this.Show();
            clsUIHelper.SETACTIVEWINDOW(this.Handle);

            // System.Threading.Thread.Sleep(800);
            //Application.DoEvents();   //PRIMEPOS-2512 07-Oct-2020 JY Commented //PRIMEPOS-3285 changes reverted
            WriteTotalToPoleDisplay();

            if (oPosPTList.amountBalanceDue != 0 && oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)    //PRIMEPOS-2520 10-May-2018 JY Added condition "oPosPTList.amountBalanceDue" to bring up coupon message if transaction amount is zero
            {
                if (oPosPTList.oCLCardRow != null && Configuration.CLoyaltyInfo.UseCustomerLoyalty == true && Configuration.CLoyaltyInfo.RedeemMethod == (int)CLRedeemMethod.Auto)
                {
                    AddAutoCLDiscount();
                }
            }

            //clsUIHelper.setColorSchecme(this);
            //Application.DoEvents();    //PRIMEPOS-2556 03-Jul-2018 JY Added
            if (this.grdPayment.ActiveRow != null)
            {
                lblTransCompleteMSG.Text = "Click at " + this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString().Trim() + " again or press Enter key to complete Transction.";
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "frmPOSPayTypesList_Load()", clsPOSDBConstants.Log_Exiting);

            #region PRIMEPOS-2512 07-Oct-2020 JY Added
            try
            {
                if (Configuration.CSetting.DefaultPaytype == "" || Configuration.CSetting.DefaultPaytype == "0")    //default focus will be on "Cash"
                {
                    this.grdPayment.Focus();
                    for (int i = 0; i < this.grdPayment.Rows.Count; i++)
                    {
                        if (this.grdPayment.Rows[i].Hidden == false)
                        {
                            this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                            grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                            break;
                        }
                    }
                }
                else if (Configuration.CSetting.DefaultPaytype == "-999")   //no focus
                {
                    Resources.Message.Display("Please select a payment type.", "Payment Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //this.grdPayment.Focus();
                    //for (int i = 0; i < this.grdPayment.Rows.Count; i++)
                    //{
                    //    if (this.grdPayment.Rows[i].Hidden == false)
                    //    {
                    //        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                    //        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                    //        //grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                    //        break;
                    //    }
                    //}
                }
                else
                {
                    foreach (UltraGridRow oRow in this.grdPayment.Rows)
                    {
                        if ((Configuration.CSetting.DefaultPaytype.Trim().ToUpper() == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim().ToUpper()) || (Configuration.CInfo.ShowOnlyOneCCType == true && oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "-99" && (Configuration.CSetting.DefaultPaytype.Trim() == "3" || Configuration.CSetting.DefaultPaytype.Trim() == "4" || Configuration.CSetting.DefaultPaytype.Trim() == "5" || Configuration.CSetting.DefaultPaytype.Trim() == "6")))
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                            grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                            break;
                        }
                    }
                }
                Application.DoEvents();
            }
            catch { }
            #endregion

            #region StoreCredit PRIMEPOS-2747 - NileshJ
            if (Configuration.CPOSSet.EnableStoreCredit)
            {
                btnStoreTransactionDetails.Visible = true;
                lblR.Visible = true;
                StoreCreditCalculation();
            }
            else
            {
                btnStoreTransactionDetails.Visible = false;
                lblR.Visible = false;
            }
            #endregion

            #region 2636
            //if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV" || Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
            //{
            //this.grdPayment.Focus();
            //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
            //}
            #endregion           

            logger.Trace("frmPOSPayTypesList_Load() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void frmPOSPayTypesList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (pressedKeysList.Count > 0 || e.KeyChar == '!')
            {
                if (pressedKeysList.Count == 0)
                    lastKeyPressedDateTime = DateTime.Now;

                int Sec = Convert.ToInt16(((TimeSpan)DateTime.Now.Subtract(lastKeyPressedDateTime)).TotalSeconds);
                if (Sec > 0)
                {
                    pressedKeysList.Clear();
                }
                pressedKeysList.Add(e.KeyChar);
                lastKeyPressedDateTime = DateTime.Now;
            }
        }

        #region frmPOSPayTypesList KeyDown splited area
        private void frmPOSPayTypesList_KeyDown(object sender, KeyEventArgs e)
        {
            this.PressEnterKeyOnKeyDown(e);

            #region PRIMEPOS-2512 23-Dec-2020 JY Added
            if (Configuration.CSetting.DefaultPaytype == "-999" && grdPayment.ActiveCell == null && grdPaymentComplete.Rows.Count == 0 && (e.KeyData == Keys.Insert || e.KeyCode == Keys.ShiftKey || e.KeyData == Keys.Space))
            {
                this.grdPayment.Focus();
                for (int i = 0; i < this.grdPayment.Rows.Count; i++)
                {
                    if (this.grdPayment.Rows[i].Hidden == false)
                    {
                        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                        grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                        break;
                    }
                }
            }
            #endregion

            if (e.KeyData == Keys.F4 && e.Control == false && e.Shift == false && e.Alt == false)
            {
                this.PressF4KeyOnKeyDown();
            }
            else if (e.KeyData == Keys.F5 && e.Control == false && e.Shift == false && e.Alt == false)
            {
                btnCashBack_Click(btnCashBack, new EventArgs());
            }
            else if (e.KeyData == Keys.Escape)
            {
                if (oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.Count == 0 || this.grdPaymentComplete.Rows.VisibleRowCount == 0)
                {
                    btnClose_Click(btnClose, new System.EventArgs());
                }
                e.Handled = true;
            }
            else if (e.KeyData == Keys.C || e.KeyData == Keys.Home)
            {
                if (this.grdPayment.ContainsFocus == true)
                {
                    if (this.grdPayment.ActiveCell != null && this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)    //Sprint-18 20-Nov-2014 JY Added additional condition to resolve object reference error - this.grdPayment.ActiveCell != null
                    {
                        CancelTrans();
                        //e.Handled=true;
                    }
                }
                else
                {
                    CancelTrans();
                }
            }
            else if (e.KeyData == Keys.L)
            {
                this.PressLKeyOnKeyDown();
            }
            //Added by shitaljit for printing gift reciepts on 8 jan 2013
            else if (e.KeyData == Keys.G)
            {
                this.PressGKeyOnKeyDown();
            }
            else if (e.KeyData == Keys.F10) //user wants to get cc info  from patient record or charge account record
            {
                if (bExecuteOnce == false)  //PRIMEPOS-2726 28-Aug-2019 JY Added if condition
                {
                    bool tokenize = true;   //PRIMEPOS-3145 28-Sep-2022 JY Added
                    this.PressF10KeyOnKeyDown(ref tokenize);
                }
            }
            else if (e.KeyData == Keys.R) //PRIMEPOS-2747 - StoreCredit
            {
                //getStoreCreditTransactionDetails();   //PRIMEPOS-2889 01-Sep-2020 JY Commented
                if (Configuration.CPOSSet.EnableStoreCredit)
                    this.PressRKeyOnKeyDown();  //PRIMEPOS-2889 01-Sep-2020 JY Added
            }
        }

        private void PressEnterKeyOnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (pressedKeysList.Count > 2)
                {
                    if (pressedKeysList[0] == '!' && pressedKeysList[pressedKeysList.Count - 1] == '!')
                    {
                        bool wasGridActive = (this.ActiveControl == this.grdPayment);
                        this.ActiveControl = this.grdPayment;
                        MoveToRow("C");
                        String cellData = new String(pressedKeysList.ToArray());
                        this.grdPayment.ActiveCell = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo];
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = cellData;
                        if (wasGridActive == false)
                        {
                            grdPayment_KeyDown(this.grdPayment, e);
                        }
                    }
                }
                pressedKeysList.Clear();

                #region PRIMEPOS-2512 08-Oct-2020 JY Added
                if (Configuration.CSetting.DefaultPaytype == "-999" && grdPayment.ActiveCell == null && grdPaymentComplete.Rows.Count == 0)
                {
                    Resources.Message.Display("Please select a payment type.", "Payment Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //this.grdPayment.Focus();
                    //for (int i = 0; i < this.grdPayment.Rows.Count; i++)
                    //{
                    //    if (this.grdPayment.Rows[i].Hidden == false)
                    //    {
                    //        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                    //        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                    //        //grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                    //        break;
                    //    }
                    //}
                }
                #endregion
            }
        }

        private void PressF4KeyOnKeyDown()
        {
            if (this.grdPayment.ContainsFocus == true)
            {
                //if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo)
                //{
                if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "H")
                {
                    this.SearchHouseChargeInfo();
                }
                if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "C")
                {
                    SelectCLCoupons();
                }
                if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "2")    //PRIMEPOS-2539 09-Jul-2018 JY Added
                {
                    if (!bAllowCheckPayment)
                        bAllowCheckPayment = oPosPTList.AllowCheckPaymentPriviliges();
                }
            }
        }

        private void PressLKeyOnKeyDown()
        {
            if (this.grdPayment.ContainsFocus == true)
            {
                if (this.grdPayment.ActiveCell != null && this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)    //Sprint-18 20-Nov-2014 JY Added additional condition to resolve object reference error - this.grdPayment.ActiveCell != null
                {
                    //this.chkIsDelivery.Checked = true;//commented by shitaljit as we are using buttons instead of check box
                    oPosPTList.bIsDelivery = !oPosPTList.bIsDelivery;
                    if (oPosPTList.bIsDelivery == true)
                    {
                        GetDeliveryAddress();
                    }
                    ChangeButtonBackColorAtClick(btnIsDeliveryTrans, oPosPTList.bIsDelivery);
                    ChangeButtonBackColorAtClick(btnPrintGiftRecpt, bPrintGiftReciept);
                    //e.Handled=true;
                }
            }
            else
            {
                //this.chkIsDelivery.Checked = true;//commented by shitaljit as we are using buttons instead of check box
                oPosPTList.bIsDelivery = !oPosPTList.bIsDelivery;
                ChangeButtonBackColorAtClick(btnIsDeliveryTrans, oPosPTList.bIsDelivery);
                ChangeButtonBackColorAtClick(btnPrintGiftRecpt, bPrintGiftReciept);
            }
        }

        private void PressGKeyOnKeyDown()
        {
            if (this.grdPayment.ContainsFocus == true)
            {
                if (this.grdPayment.ActiveCell != null && this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)    //Sprint-18 20-Nov-2014 JY Added additional condition to resolve object reference error - this.grdPayment.ActiveCell != null
                {
                    bPrintGiftReciept = !bPrintGiftReciept;
                    //clsUIHelper.setColorSchecme(this);
                    ChangeButtonBackColorAtClick(btnIsDeliveryTrans, oPosPTList.bIsDelivery);
                    ChangeButtonBackColorAtClick(btnPrintGiftRecpt, bPrintGiftReciept);
                }
            }
            else
            {
                bPrintGiftReciept = !bPrintGiftReciept;
                ChangeButtonBackColorAtClick(btnIsDeliveryTrans, oPosPTList.bIsDelivery);
                ChangeButtonBackColorAtClick(btnPrintGiftRecpt, bPrintGiftReciept);
            }
        }

        private Boolean PressF10KeyOnKeyDown(ref bool tokenize)  //PRIMEPOS-3145 28-Sep-2022 JY modified return type
        {
            bool bReturn = true;    //PRIMEPOS-3145 28-Sep-2022 JY Added            
            POSTransaction oPOSTrans = new POSTransaction();
            Customer oCustomer = new Customer();
            CustomerData oCustData = new CustomerData();
            oCustData = oCustomer.Populate("-1");
            CustomerRow oCustRow = null;

            if (oCustData != null)
            {
                if (oCustData.Tables[0].Rows.Count > 0)
                {
                    oCustRow = oCustData.Customer[0];
                }
            }
            //Added By Rohit Nair  for POS F10 feature 
            if (!string.IsNullOrEmpty(Configuration.CPOSSet.PaymentProcessor))
            {
                #region PRIMEPOS-3103
                if (Configuration.CPOSSet.PaymentProcessor.ToUpper().Trim() == "HPS" && Configuration.CSetting.OnlinePayment)
                {
                    string PatientNo = "";
                    string strCode = "";
                    frmCustomerSearch oSearch = new frmCustomerSearch("", false);
                    if (oCustRow.CustomerFullName == this.lblCustomerName.Text || this.lblCustomerName.Text == "" || oPosPTList.oCurrentCustRow == null)
                    {
                        oSearch.ActiveOnly = 1;
                        oSearch.ShowDialog(this);
                        if (!oSearch.IsCanceled)
                        {
                            strCode = oSearch.SelectedRowID();
                        }
                    }
                    else if (oPosPTList.oCurrentCustRow != null)
                    {
                        strCode = oPosPTList.oCurrentCustRow.CustomerId.ToString();
                    }

                    if (strCode == "")
                    {
                        return false;   //PRIMEPOS-3145 28-Sep-2022 JY modified
                    }
                    oCustData = oCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
                    //Added By shitaljit to add customer to DB if it is a customer from PrimeRx that is not exist in POS currently.
                    if (oCustData.Tables[0].Rows.Count == 0)
                    {
                        oCustRow = oSearch.SelectedRow();
                        if (oCustRow != null)
                        {
                            if (string.IsNullOrEmpty(oCustRow.Address1) == true)
                            {
                                oCustRow.Address1 = "Default";
                            }
                            oCustData.Tables[0].ImportRow(oCustRow);
                            oCustomer.Persist(oCustData);
                            oCustData = oCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                            if (oCustData.Tables[0].Rows.Count > 0)
                            {
                                oCustRow = oCustData.Customer[0];
                                strCode = oCustRow.CustomerId.ToString();
                            }
                        }
                    }
                    if (oCustData != null)
                    {
                        if (oCustData.Tables[0].Rows.Count > 0)
                        {
                            oPosPTList.oCurrentCustRow = oCustData.Customer[0];
                            PatientNo = oPosPTList.oCurrentCustRow.PatientNo.ToString();
                            lblCustomerName.Text = oPosPTList.oCurrentCustRow.CustomerFullName;
                            ShowCustomerTokenImage(oPosPTList.oCurrentCustRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                        }
                    }

                    int tokenCount = 0;
                    int cardInfoCount = 0;

                    oPOSTrans.CheckPatientTokenAndCardData(PatientNo, Convert.ToInt32(strCode), ref tokenCount, ref cardInfoCount);

                    if (tokenCount > 0 && cardInfoCount > 0)
                    {
                        frmPatientPayCCSelect frmPatientPayCCSelect = new frmPatientPayCCSelect();
                        frmPatientPayCCSelect.ShowDialog();
                        if (frmPatientPayCCSelect.IsPatientCCFromPrimeRx)
                        {
                            bReturn = this.PressF10KeyOtherCustomerCode(oCustRow, oPOSTrans);   //PRIMEPOS-3145 28-Sep-2022 JY modified
                        }
                        else
                        {
                            bReturn = this.OtherPressF10KeyOnKeyDown(oCustRow, ref tokenize);   //PRIMEPOS-3145 28-Sep-2022 JY modified
                        }
                    }
                    else if (cardInfoCount > 0)
                    {
                        bReturn = this.PressF10KeyOtherCustomerCode(oCustRow, oPOSTrans);   //PRIMEPOS-3145 28-Sep-2022 JY modified
                    }
                    else
                    {
                        bReturn = this.OtherPressF10KeyOnKeyDown(oCustRow, ref tokenize); //PRIMEPOS-3145 28-Sep-2022 JY modified
                    }
                }
                #endregion
                //Commented out by Rohit Nair
                else if (Configuration.CPOSSet.PaymentProcessor.ToUpper().Trim() == "HPS")
                {
                    #region "Old F10 code"
                    if (oPosPTList.isROA)
                    {
                        this.F10KeyOnKeyDownROA(oPOSTrans);
                    }
                    //Following else part is added by Shitaljit(QuicSolv) on 17 May 2011
                    //it will invoke if there area any RX item in the transaction.
                    else if ((oPosPTList.RXHeaderList != null && oPosPTList.RXHeaderList.Count > 0)) //.frmPOSTransaction.oTRxDetailDt != null)//.TransDetailRX.Rows.Count > 0)
                    {
                        string CardType = oPOSTrans.GetCCInformation(F10TransType.RXTrans, oPosPTList.RXHeaderList, dtChargeAccount, "", out oPosPTList.CustomerCardInfo);
                        if (CardType != "")
                        {
                            //Following if-else is added by shitaljit on 5 March 2012
                            //to resolve payment posting as Cash if ShowOnlyOneCCType = true and we hit F10
                            if (Configuration.CInfo.ShowOnlyOneCCType == true)
                            {
                                MoveToRow("-99");
                            }
                            else
                            {
                                MoveToRow(CardType);
                            }

                            this.lblCustomerName.Text = oPosPTList.F10KeyImportCustomer(oCustomer, oCustData, oCustRow);

                            #region PRIMEPOS-2886 31-Aug-2020 JY Commented
                            //if (Configuration.CInfo.AutoImportCustAtTrans == true)
                            //{
                            //    CustomerData oTempCustData = oCustomer.GetCustomerByPatientNo(Configuration.convertNullToInt(oPosPTList.RXHeaderList[0].PatientNo));
                            //    if (oTempCustData == null || oTempCustData.Tables.Count == 0 || oTempCustData.Tables[0].Rows.Count == 0)
                            //    {
                            //        DataSet oDS = null;
                            //        MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                            //        oAcct.GetPatientByCode(Convert.ToInt32(Configuration.convertNullToInt(oPosPTList.RXHeaderList[0].PatientNo)), out oDS);
                            //        frmCustomerPatientMapping ofrmCustomerPatientMapping = new frmCustomerPatientMapping(oDS);
                            //        ofrmCustomerPatientMapping.ShowDialog();
                            //        oTempCustData = ofrmCustomerPatientMapping.oCustomerData;
                            //        if (oTempCustData != null && oTempCustData.Tables.Count > 0 && oTempCustData.Tables[0].Rows.Count > 0)
                            //        {
                            //            oCustData = oTempCustData;
                            //            if (oCustData != null && oCustData.Tables[0].Rows.Count > 0)
                            //            {
                            //                this.lblCustomerName.Text = Configuration.convertNullToString(oCustData.Customer[0].CustomerFullName);
                            //            }
                            //        }
                            //    }
                            //}
                            #endregion

                            ShowCustomerTokenImage(Configuration.convertNullToInt(oCustData.Tables[0].Rows[0][clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID].ToString()));   //PRIMEPOS-2611 13-Nov-2018 JY Added
                            oPosPTList.is_F10 = true; // Add by Manoj to know if the user press F10 to get card info 8/23/2011
                            this.grdPayment_KeyDown(this, new KeyEventArgs(Keys.Enter));
                        }
                        //frmPOSTransaction.oTRxDetailDt = null;
                    }
                    //Added By Shitaljit(QuicSolv) on 17 Sept 2012
                    else
                    {
                        bReturn = this.PressF10KeyOtherCustomerCode(oCustRow, oPOSTrans); //PRIMEPOS-3145 28-Sep-2022 JY modified
                    }
                    #endregion
                }
                else //Commented out by Rohit Nair
                {
                    bReturn = this.OtherPressF10KeyOnKeyDown(oCustRow, ref tokenize);   //PRIMEPOS-3145 28-Sep-2022 JY modified
                }
            }
            return bReturn;
        }

        private void F10KeyOnKeyDownROA(POSTransaction oPOSTrans)
        {
            string CardType = oPOSTrans.GetCCInformation(F10TransType.ROA, null, dtChargeAccount, "", out oPosPTList.CustomerCardInfo);
            if (CardType != "")
            {
                //Following if-else is added by shitaljit on 5 March 2012
                //to resolve payment posting as Cash if ShowOnlyOneCCType = true and we hit F10
                if (Configuration.CInfo.ShowOnlyOneCCType == true)
                {
                    MoveToRow("-99");
                }
                else
                {
                    MoveToRow(CardType);
                }

                oPosPTList.is_F10 = true; // Add by Manoj to know if the user press F10 to get card info 8/23/2011
                this.grdPayment_KeyDown(this, new KeyEventArgs(Keys.Enter));
            }
        }

        private Boolean PressF10KeyOtherCustomerCode(CustomerRow oCustRow, POSTransaction oPOSTrans)    //PRIMEPOS-3145 28-Sep-2022 JY modified rturn type
        {
            bool bReturn = true;
            string PatientNo = "";
            string strCode = "";
            frmCustomerSearch oSearch = new frmCustomerSearch("", false);
            if (oCustRow.CustomerFullName == this.lblCustomerName.Text || this.lblCustomerName.Text == "" || oPosPTList.oCurrentCustRow == null)
            {
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                }
            }
            else if (oPosPTList.oCurrentCustRow != null)
            {
                strCode = oPosPTList.oCurrentCustRow.CustomerId.ToString();
            }

            if (strCode == "")
            {
                return false;   //PRIMEPOS-3145 28-Sep-2022 JY modified
            }
            oCustData = oCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
            //Added By shitaljit to add customer to DB if it is a customer from PrimeRx that is not exist in POS currently.
            if (oCustData.Tables[0].Rows.Count == 0)
            {
                oCustRow = oSearch.SelectedRow();
                if (oCustRow != null)
                {
                    if (string.IsNullOrEmpty(oCustRow.Address1) == true)
                    {
                        oCustRow.Address1 = "Default";
                    }
                    oCustData.Tables[0].ImportRow(oCustRow);
                    oCustomer.Persist(oCustData);
                    oCustData = oCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                    if (oCustData.Tables[0].Rows.Count > 0)
                    {
                        oCustRow = oCustData.Customer[0];
                        strCode = oCustRow.CustomerId.ToString();
                    }
                }
            }
            if (oCustData != null)
            {
                if (oCustData.Tables[0].Rows.Count > 0)
                {
                    oPosPTList.oCurrentCustRow = oCustData.Customer[0];
                    PatientNo = oPosPTList.oCurrentCustRow.PatientNo.ToString();
                    lblCustomerName.Text = oPosPTList.oCurrentCustRow.CustomerFullName;
                    ShowCustomerTokenImage(oPosPTList.oCurrentCustRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added
                }
            }

            string CardType = oPOSTrans.GetCCInformation(F10TransType.NonRXTrans, oPosPTList.RXHeaderList, dtChargeAccount, PatientNo, out oPosPTList.CustomerCardInfo);
            if (CardType != "")
            {
                if (Configuration.CInfo.ShowOnlyOneCCType == true)
                {
                    MoveToRow("-99");
                }
                else
                {
                    MoveToRow(CardType);
                }
                oPosPTList.is_F10 = true; // Add by Manoj to know if the user press F10 to get card info 8/23/2011
                this.grdPayment_KeyDown(this, new KeyEventArgs(Keys.Enter));
            }
            else
            {
                oPosPTList.oRXHeaderList = null;
            }

            return bReturn;
        }

        private Boolean OtherPressF10KeyOnKeyDown(CustomerRow oCustRow, ref bool tokenize) //PRIMEPOS-3145 28-Sep-2022 JY modified return type
        {
            bool bReturn = true;
            #region "New F10 Code"
            string PatientNo = "";
            string strCode = "";
            frmCustomerSearch oSearch = new frmCustomerSearch("", false);
            if (oCustRow.CustomerFullName == lblCustomerName.Text || this.lblCustomerName.Text == "" || oPosPTList.oCurrentCustRow == null)
            {
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                }
            }
            else if (oPosPTList.oCurrentCustRow != null)
            {
                strCode = oPosPTList.oCurrentCustRow.CustomerId.ToString();
            }

            if (strCode == "")
            {
                return false;   //PRIMEPOS-3145 28-Sep-2022 JY modified
            }
            if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON)//2943
            {
                Resources.Message.Display("We don't support for tokenize return");
                tokenize = false;   //PRIMEPOS-3145 28-Sep-2022 JY Added
                return false;   //PRIMEPOS-3145 28-Sep-2022 JY modified
            }
            oCustData = oCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
            //Added By shitaljit to add customer to DB if it is a customer from PrimeRx that is not exist in POS currently.
            if (oCustData.Tables[0].Rows.Count == 0)
            {
                oCustRow = oSearch.SelectedRow();
                if (oCustRow != null)
                {
                    if (string.IsNullOrEmpty(oCustRow.Address1) == true)
                    {
                        oCustRow.Address1 = "Default";
                    }
                    oCustData.Tables[0].ImportRow(oCustRow);
                    oCustomer.Persist(oCustData);
                    oCustData = oCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                    if (oCustData.Tables[0].Rows.Count > 0)
                    {
                        oCustRow = oCustData.Customer[0];
                        strCode = oCustRow.CustomerId.ToString();
                    }
                }
            }
            if (oCustData != null)
            {
                if (oCustData.Tables[0].Rows.Count > 0)
                {
                    oPosPTList.oCurrentCustRow = oCustData.Customer[0];
                    PatientNo = oPosPTList.oCurrentCustRow.PatientNo.ToString();
                    lblCustomerName.Text = oPosPTList.oCurrentCustRow.CustomerFullName;
                    ShowCustomerTokenImage(oPosPTList.oCurrentCustRow.CustomerId);   //PRIMEPOS-2611 13-Nov-2018 JY Added                    
                }
            }
            string CardType = "";
            string strSelectedToken = string.Empty;
            CCCustomerTokInfoRow selectedTokenRow = null;

            //PRIMEPOS-2902
            //if (Configuration.CSetting.OnlinePayment == true)//PRIMEPOS-TOKENSALE
            //{
            //    if (Resources.Message.Display("If you want to use PrimeRxPay's token then select Yes otherwise No ?", "Token Select", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    {
            //        Configuration.isPrimeRxPay = true;
            //    }
            //    else
            //        Configuration.isPrimeRxPay = false;
            //}

            frmSelectCustomerToken oSelectToken = new frmSelectCustomerToken(oCustData);

            oSelectToken.ShowDialog(this);

            if (!oSelectToken.bIsCancelled)
            {
                selectedTokenRow = oSelectToken.SelectedRow();
            }
            else
            {
                Configuration.isPrimeRxPay = false;//PRIMEPOS-TOKENSALE
                bReturn = false;    //PRIMEPOS-3145 28-Sep-2022 JY modified
            }

            if (selectedTokenRow != null)
            {
                if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON)//2943
                {
                    if (oPosPTList.CustomerCardInfo == null)
                    {
                        oPosPTList.CustomerCardInfo = new PccCardInfo();
                    }
                    oPosPTList.CustomerCardInfo.cardExpiryDate = selectedTokenRow.ExpDate.ToString("MMyy");
                }
                if (Configuration.CSetting.OnlinePayment && !string.IsNullOrWhiteSpace(Configuration.CSetting.PayProviderName))
                {
                    if (selectedTokenRow.Processor.Trim() == Configuration.CPOSSet.PaymentProcessor && selectedTokenRow.Processor == Configuration.CSetting.PayProviderName.Trim())
                    {
                        frmPrimeRxPayTokenSelect oPrimeRxPay = new frmPrimeRxPayTokenSelect(Configuration.CPOSSet.PaymentProcessor);
                        oPrimeRxPay.ShowDialog();
                        if (oPrimeRxPay.isPrimeRxPay)
                            //if (Resources.Message.Display("Both the Payment Processor's are same if you want to use PrimeRxPay's token then select Yes otherwise No ?", "Token Select", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            Configuration.isPrimeRxPay = true;
                        else
                            Configuration.isPrimeRxPay = false;
                    }
                    else if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == selectedTokenRow.Processor)
                    {
                        Configuration.isPrimeRxPay = false;
                    }
                    else if (selectedTokenRow.Processor.Trim() == Configuration.CSetting.PayProviderName.Trim())
                    {
                        Configuration.isPrimeRxPay = true;
                    }
                    //else
                    //{
                    //    Configuration.isPrimeRxPay = true;
                    //}
                }

                if (oPosPTList.CustomerCardInfo == null)
                {
                    oPosPTList.CustomerCardInfo = new PccCardInfo();
                }
                #region PRIMEPOS-3057 Start
                if (!string.IsNullOrWhiteSpace(selectedTokenRow.ProfiledID) && selectedTokenRow.ProfiledID.Contains("|") && Configuration.CPOSSet.PaymentProcessor != clsPOSDBConstants.ELAVON) //PRIMEPOS-3244-Added condition for Elavon.
                {
                    oPosPTList.CustomerCardInfo.ProfileID = selectedTokenRow.ProfiledID.Split('|')[0];
                }
                else
                {
                    oPosPTList.CustomerCardInfo.ProfileID = selectedTokenRow.ProfiledID;
                }
                #endregion PRIMEPOS-3057 End                 
                oPosPTList.CustomerCardInfo.IsFsaCard = selectedTokenRow.IsFsaCard;//2990
                this.tokenID = selectedTokenRow.EntryID;//3009
                oPosPTList.CustomerCardInfo.Last4 = selectedTokenRow.Last4;
                oPosPTList.CustomerCardInfo.UseToken = true;
                //Tokenize issue -NileshJ
                #region PRIMEPOS-3081
                //if (SigPadUtil.DefaultInstance.isPAX)
                //{
                //    oPosPTList.CustomerCardInfo.cardType = selectedTokenRow.CardType;//Added Suraj For HPSPAX Tokenization
                //}
                oPosPTList.CustomerCardInfo.cardType = selectedTokenRow.CardType;
                #endregion
                CardType = selectedTokenRow.CardType;
            }
            else
            {
                return false;   //PRIMEPOS-3145 28-Sep-2022 JY modified
            }
            if (CardType != "")
            {
                if (!Configuration.isPrimeRxPay)//PRIMEPOS-3011
                {
                    if (Configuration.CInfo.ShowOnlyOneCCType == true)
                    {
                        MoveToRow("-99");
                    }
                    else
                    {
                        MoveToRow(CardType);
                    }
                }
                else
                {
                    if (this.grdPayment.ActiveRow != null)
                    {
                        if (txtAmtTotal.Text == Convert.ToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value))
                        {
                            MoveToRow("O");
                        }
                    }
                }
                oPosPTList.is_F10 = true; // Add by Manoj to know if the user press F10 to get card info 8/23/2011
                this.grdPayment_KeyDown(this, new KeyEventArgs(Keys.Enter));
            }
            else
            {
                oPosPTList.oRXHeaderList = null;
            }
            #endregion

            return bReturn;
        }

        #endregion frmPOSPayTypesList KeyDown splited area

        private void frmPOSPayTypesList_ResizeBegin(object sender, EventArgs e)
        {
            //SetEnabledAdd(this, true);
        }

        private void frmPOSPayTypesList_ResizeEnd(object sender, EventArgs e)
        {
            //SetEnabledMinus(this, true);
        }


        private void frmPOSPayTypesList_Closing(object sender, CancelEventArgs e)
        {
            logger.Trace("frmPOSPayTypesList_Closing() - " + clsPOSDBConstants.Log_Entering);
            if (!this.IsCustomerDriven)
            {
                if (oPosPTList.oPOSTransPaymentDataPaid == null)
                {
                    frmMain.PoleDisplay.ClearPoleDisplay();
                    clsUIHelper.ShowWelcomeMessage();
                    return;
                }
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales || oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.ReceiveOnAccount)
                {
                    if (oPosPTList.amountPaid < oPosPTList.totalAmount + oPosPTList.TransFeeAmt + oPosPTList.amountCashBack)    //PRIMEPOS-3117 11-Jul-2022 JY modified
                    {
                        clsUIHelper.ShowErrorMsg("Payment is incomplete.");
                        e.Cancel = true;
                        this.grdPaymentComplete.Focus();
                        this.grdPaymentComplete.PerformAction(UltraGridAction.EnterEditMode);
                        return;
                    }
                    else
                    {
                        oPosPTList.CashBackForOverPay();
                    }
                }
            }

            //Added by Farman Ansari form remove Zero transaction
            oPosPTList.RemoveZeroTrans();
            this.ShowNumericPadOnFormClosing();
            logger.Trace("frmPOSPayTypesList_Closing() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void ShowNumericPadOnFormClosing()
        {
            if (Configuration.showNumPad == true)
            {
                try
                {
                    if (frmNumPad != null && frmNumPad.IsDisposed == false)
                    {
                        ((frmNumericPad)frmNumPad).closeForm();
                    }
                }
                catch { }
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

        //public void SetEnabledLoad(Control control, bool enabled)
        //{
        //    control.Enabled = enabled;

        //    foreach (Control child in control.Controls)
        //    {

        //        widthRatio = this.Width / 800f;
        //        heightRatio = this.Height / 600f;
        //        if ((widthRatio * heightRatio * child.Font.Size) > 12)
        //        {
        //            child.Font = new Font(child.Font.Name, 12, child.Font.Style);
        //            if (this.Height != 600f)
        //            {
        //                ultraLabel14.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        //                txtAmtTotal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        //                btnMoneyOne.Font = new Font("Segoe UI", 14);
        //                btnMoneyFive.Font = new Font("Segoe UI", 14);
        //                btnMoneyTen.Font = new Font("Segoe UI", 14);
        //                btnMoneyTwenty.Font = new Font("Segoe UI", 14);
        //                btnMoneyFifty.Font = new Font("Segoe UI", 14);
        //                btnMoneyHundred.Font = new Font("Segoe UI", 14);
        //            }
        //        }
        //        else
        //        {
        //            child.Font = new Font(child.Font.Name, widthRatio * heightRatio * child.Font.Size, child.Font.Style);// widthRatio* heightRatio*child.Font.Size
        //        }
        //        SetEnabledLoad(child, enabled);
        //    }
        //}

        //public void SetEnabledAdd(Control control, bool enabled)
        //{
        //    control.Enabled = enabled;
        //    foreach (Control child in control.Controls)
        //    {
        //        widthRatio = this.Width / 800f;
        //        heightRatio = this.Height / 600f;
        //        child.Font = new Font("Segoe UI", child.Font.SizeInPoints + heightRatio + widthRatio);
        //        //child.Font = new Font("Segoe UI", 12);
        //        if (child is Infragistics.Win.Misc.UltraLabel)
        //        {
        //            Infragistics.Win.Misc.UltraLabel lbl = (Infragistics.Win.Misc.UltraLabel)child;
        //            if (lbl.Text.StartsWith("ultraLabel"))
        //                //lbl.Font = new Font("Segoe UI ", 10);
        //                lbl.Font = new Font("Segoe UI", child.Font.SizeInPoints + heightRatio + widthRatio);
        //        }
        //        heightRatio = 0;
        //        widthRatio = 0;
        //        SetEnabledAdd(child, enabled);
        //    }
        //}

        //public void SetEnabledMinus(Control control, bool enabled)
        //{
        //    control.Enabled = enabled;
        //    foreach (Control child in control.Controls)
        //    {

        //        float aa = this.Width;

        //        widthRatio = this.Width / 800f;
        //        heightRatio = this.Height / 600f;
        //        child.Font = new Font("Segoe UI", child.Font.SizeInPoints - heightRatio - widthRatio);
        //        if (child is Infragistics.Win.Misc.UltraLabel)
        //        {
        //            Infragistics.Win.Misc.UltraLabel lbl = (Infragistics.Win.Misc.UltraLabel)child;
        //            if (lbl.Text.StartsWith("ultraLabel"))
        //                //lbl.Font = new Font("Segoe UI ", 10);
        //                lbl.Font = new Font("Segoe UI", child.Font.SizeInPoints - heightRatio - widthRatio);

        //        }
        //        heightRatio = 0;
        //        widthRatio = 0;
        //        SetEnabledMinus(child, enabled);
        //    }
        //}

        #endregion Form events

        #region Grid Events
        private string strTempRowPayTypeDesc = string.Empty;
        private void SetRowAppearance(UltraGridRow oRow)
        {
            logger.Trace("SetRowAppearance() - " + clsPOSDBConstants.Log_Entering);
            switch (oRow.Cells["TransTypeCode"].Text.ToString().Trim())
            {
                case "1":
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            //this.stbAmount.Enabled = true;
                            // this.stbAmount2.Enabled = true;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2857
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
                case "2":
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2857
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
                case "C":
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                        //Added oTransactionType != POS_Core.TransType.POSTransactionType.Sales by shitaljit to restrict coupon selection in return transaction.
                        //PRIMEPOS-2434 06-May-2021 JY Added ROA condition
                        if (Configuration.CLoyaltyInfo.UseCustomerLoyalty == false || Configuration.CLoyaltyInfo.RedeemMethod != (int)CLRedeemMethod.Manual
                            || (oPosPTList.isROA == false && oPosPTList.oCLCardRow == null) || (oPosPTList.isROA == true && oPosPTList.oCLCardRow != null) || oPosPTList.oTransactionType != POS_Core.TransType.POSTransactionType.Sales)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                        }
                        else //PRIMEPOS-2783 18-Feb-2020 JY Added
                        {
                            try
                            {
                                Infragistics.Win.UltraWinEditors.UltraTextEditor btnCLCoupon = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
                                btnCLCoupon.EditorButtonClick -= new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(btnCLCoupon_EditorButtonClick);
                                btnCLCoupon.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(btnCLCoupon_EditorButtonClick);
                                Infragistics.Win.UltraWinEditors.EditorButton oEditorButton = new Infragistics.Win.UltraWinEditors.EditorButton();
                                oEditorButton.Text = "CL Coupon";
                                oEditorButton.Width = 90;
                                oEditorButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
                                btnCLCoupon.ButtonsRight.Add(oEditorButton);
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Editor = btnCLCoupon.Editor;
                                strTempRowPayTypeDesc = oRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString().Trim();

                                #region PRIMEPOS-2434 06-May-2021 JY Added
                                //oPosPTList.isROA == true add logic to assign oPosPTList.oCLCardRow
                                if (oPosPTList.isROA == true && oPosTrans.oCustomerRow != null && oPosTrans.oCustomerRow.AccountNumber != -1)
                                {
                                    oPosPTList.oCLCardRow = oPosTrans.GetActiveCardForCustomerID(oPosTrans.oCustomerRow.CustomerId);
                                    oPosPTList.maxClCouponAmount = oPosPTList.totalAmount;
                                }
                                #endregion
                            }
                            catch { }
                        }

                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2857
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
                case "H":
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.NoEdit;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
                        //grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value=0;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.NoEdit;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2857
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        //this.stbAmount.Enabled = false;
                        //this.stbAmount2.Enabled = false;
                        break;
                    }
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "-99":
                case "N": //PRIMEPOS-3375
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;

                        if (oRow.Cells["TransTypeCode"].Text.ToString().Trim() != "7")
                        {
                            if (Configuration.CInfo.SaveCCToken && oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.CustomerCode != "-1" && oPosPTList.oCurrentCustRow.SaveCardProfile)
                                Tokenize = true;
                            else
                                Tokenize = false;
                        }


                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "XLINK")
                        {
                            if (oRow.Cells["TransTypeCode"].Text.ToString().Trim() == "7" && Configuration.convertNullToDecimal(this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString()) == 0)
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            }
                            else
                            {
                                if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                                {
                                    oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                                    oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                                    oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                                }
                                else
                                {
                                    oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                                    oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;   //PRIMEPOS-2653 For Jenny
                                }
                            }
                        }
                        else
                        {
                            if (Configuration.convertNullToDecimal(this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString()) == 0)
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            }
                            else
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
                            }
                        }
                        if (Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2857
                        {
                            if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && oRow.Cells["TransTypeCode"].Text.ToString().Trim() != "7")
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            }
                            else if (oRow.Cells["TransTypeCode"].Text.ToString().Trim() == "7")
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.NoEdit;
                                //oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            }
                        }
                        //this.stbAmount.Enabled = true;
                        break;
                    }
                case "E":
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2664
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
                                //oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.NoEdit;
                            }
                            else
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                            }
                        }
                        //Added by Shitaljit ton 1/17/2014 to disable EBT paytype is htere is no EBT items.
                        else if (Math.Abs(oPosPTList.GetPendingEBTAmount(this.totalNonIIASAmount)) == 0)
                        {
                            oRow.Activation = Activation.Disabled;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
                            //following if-else is Added By Shitaljit to deactivate refNo col if allow manual EBT is not allowed.
                            if (Configuration.CInfo.EBTAsManualTrans == true)
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                            }
                            else
                            {
                                oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                            }
                        }
                        break;
                    }
                case "S": //  Added for Solutran - PRIMEPOS-2663 - NileshJ
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2664
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
                case "X": // PRIMEPOS-2747 - StoreCredit - NileshJ - 08-nov-2019
                    {
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        else if (IsStoreCredit && oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2664
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
                #region PRIMEPOS-2841
                case "O":
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                        }

                        if (Configuration.CInfo.SaveCCToken && oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.CustomerCode != "-1" && oPosPTList.oCurrentCustRow.SaveCardProfile)//PRIMEPOS-Arvind Token
                            Tokenize = true;
                        else
                            Tokenize = false;

                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2664
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
                case "F":
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (Configuration.convertNullToDecimal(oPosPTList.amountCashBack) > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2664
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
                #endregion
                default:
                    {
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                        //oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;//Added by shitaljit to allow users to enter RefNo if any.
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Activation = Activation.Disabled;
                        if (oPosPTList.amountCashBack > 0)
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                        }
                        else
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            //this.stbAmount.Enabled = true;
                            //this.stbAmount2.Enabled = true;
                        }
                        if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Configuration.convertNullToDecimal(CashBackAmount) > 0)//PRIMEPOS-2664
                        {
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = 0;
                            oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        }
                        break;
                    }
            }
            this.lblTransCompleteMSG.Text = "Click at " + oRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString().Trim() + " again or press Enter key to complete Transction.";
            //}catch(Exception ) {}
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "SetRowAppearance()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("SetRowAppearance() - " + clsPOSDBConstants.Log_Exiting);
            grdPayment.Focus();
        }

        private void btnCLCoupon_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    #region PRIMEPOS-2434 06-May-2021 JY Added
                    if (oPosPTList.isROA == true && oPosPTList.oCLCardRow == null)
                    {
                        Resources.Message.Display("Active coupons are not available for the selected customer.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    #endregion

                    SelectCLCoupons();
                }
                else if (this.lblTransCompleteMSG.Text.Contains(strTempRowPayTypeDesc) == true && LastClickPaytype == strTempRowPayTypeDesc)
                {
                    this.grdPayment.ActiveCell = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                    grdPayment_KeyDown(this, new KeyEventArgs(Keys.Enter));
                }
                else
                {
                    LastClickPaytype = strTempRowPayTypeDesc;
                }
            }
            catch (Exception exp)
            {
                //clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region Apply Grid Format
        private void ApplyGridFormat()
        {
            this.Formatgrdpayment();
            this.FormatgrdPaymentComplete();
        }

        private void Formatgrdpayment()
        {
            clsUIHelper.SetAppearance(this.grdPayment);
            grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Width = 20;
            grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].CellAppearance.TextHAlign = HAlign.Right;
            grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MaxValue = 999999.99;
            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MinValue = 0;
                grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MaskInput = "nnnnnn.nn";
            }
            else
            {
                grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MinValue = -999999.99;
                grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MaskInput = "-nnnnnn.nn";
            }
            grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].Format = "######0.00";
            clsUIHelper.SetEditonlyRow(this.grdPayment);
            this.grdPayment.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.UndoCell, 0, UltraGridState.InEdit, 0, 0));
            this.grdPayment.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.EnterEditMode, 0, UltraGridState.InEdit, 0, 0));
            this.grdPayment.DisplayLayout.TabNavigation = TabNavigation.NextControl;
            this.grdPayment.DisplayLayout.Bands[0].Override.SelectTypeCell = SelectType.Single;
            this.grdPayment.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.None;
            this.grdPayment.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.Edit;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].CellActivation = Activation.ActivateOnly;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransDate].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransID].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransPayID].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID].Hidden = true;
            try
            {
                this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Hidden = true;
                this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_SigType].Hidden = true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ApplyGridFormat()");
            }
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Hidden = true;
            //Added By SRT(Ritesh Parekh) Date: 29-Jul-2009
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].Hidden = true;//New column added and hidden in payment grid for storing Payment Processor
            //End Of Added By SRT(Ritesh Parekh)
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_RefNo].MaxLength = 50;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_IsManual].Hidden = true;   //Sprint-19 - 2139 06-Jan-2015 JY Added


        }

        private void FormatgrdPaymentComplete()
        {

            clsUIHelper.SetAppearance(this.grdPaymentComplete);

            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].CellAppearance.TextHAlign = HAlign.Right;
            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MaxValue = 99999999999.99;
            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MinValue = 0;
                grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MaskInput = "nnnnnnnnnnn.nn";
            }
            else
            {
                grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MinValue = -99999999999.99;
                grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].MaskInput = "-nnnnnnnnnnn.nn";
            }

            grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount].Format = "##########0.00";
            clsUIHelper.SetReadonlyRow(this.grdPaymentComplete);
            this.grdPaymentComplete.DisplayLayout.TabNavigation = TabNavigation.NextControl;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransDate].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransID].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_TransPayID].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID].Hidden = true;
            try
            {
                this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Hidden = true;
                this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_SigType].Hidden = true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ApplyGridFormat()");
            }
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Hidden = true;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_RefNo].MaxLength = 50;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_RefNo].CellActivation = Activation.NoEdit;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            this.grdPaymentComplete.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_IsManual].Hidden = true;
        }

        #endregion Apply Grid Format

        private void grdPayment_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["TransTypeCode"].Value.ToString().Trim() == "B")
            {
                e.Row.Hidden = true;
            }
        }

        private void grdPayment_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            ClearGridRow(this.grdPayment.ActiveRow);
        }

        private void grdPayment_AfterRowActivate(object sender, EventArgs e)
        {
            if (grdPayment.ActiveRow.Cells["TransTypeCode"].Text.ToString().Trim() == "E")
            {
                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.GetPendingEBTAmount(this.totalNonIIASAmount);
                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
            }
            else
            {
                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue;
            }
            #region PRIMEPOS-2747
            if (IsStoreCredit == false && grdPayment.ActiveRow.Cells["TransTypeCode"].Text.ToString().Trim() == "X")  //PRIMEPOS-2938 27-Jan-2021 JY modified
            {
                StoreCreditCalculation();
                IsStoreCredit = false;
            }
            #endregion
            SetRowAppearance(grdPayment.ActiveRow);
        }

        // Added by Farman Ansari 21/12/2017 
        #region grdPayment enter area
        private void grdPayment_Enter(object sender, EventArgs e)
        {
            //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
            calculatePaidAmount();
            if (this.grdPayment.ActiveRow != null)
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue;
            if (Configuration.CSetting.DefaultPaytype != "-999")    //PRIMEPOS-2512 08-Oct-2020 JY Added if clause
                this.grdPayment.PerformAction(UltraGridAction.EnterEditMode);
        }

        private Boolean ValidateAmount(KeyEventArgs e)  //PRIMEPOS-2521 21-May-2018 JY changed return type from Void to Boolean as function should return false for invalid amount
        {
            bOverPay = false;   //PRIMEPOS-2763 26-Nov-2019 JY Added
            #region PRIMEPOS-2589 26-Sep-2018 JY Added to validate amount
            try
            {
                decimal dTemp = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
            }
            catch
            {
                clsUIHelper.ShowErrorMsg("Please enter valid amount");
                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].SelectAll();
                return false;
            }
            #endregion

            //Change by SRT (Abhishek D) Date : 12 March 2010
            if (Configuration.CPOSSet.AllowZeroAmtTransaction == false)
            {
                if (Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) == 0 && oPosPTList.totalAmount + oPosPTList.amountCashBack == 0)    //PRIMEPOS-2589 26-Sep-2018 JY modified        
                {
                    calculatePaidAmount();
                    return false;
                }
            }
            //added By shitaljit To stop Recording $0 Payments

            if (Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) == 0 /*&& oPosPTList.totalAmount > 0 && oPosPTList.amountCashBack == 0*/)//Changed by Arvind //PRIMEPOS-2589 26-Sep-2018 JY modified
            {
                #region PRIMEPOS-3300
                string checkCustomePay = Convert.ToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value).Trim();
                string customePayPayTypeCode = string.Empty;
                if (!string.IsNullOrEmpty(checkCustomePay) && checkCustomePay.Length > 0)
                {
                    customePayPayTypeCode = checkCustomePay[0].ToString();
                }
                #endregion
                if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "-99"
                || this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "3"
                || this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "4"
                || this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "5"
                || this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "6"
                || this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "7"
                || this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "O"
                || customePayPayTypeCode != string.Empty && customePayPayTypeCode == "D"
                )//PRIMEPOS-3300
                {
                    calculatePaidAmount();
                    clsUIHelper.ShowErrorMsg("Please enter an amount greater than $0.00.");
                    grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue;
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].SelectAll();
                    return false;
                }
            }

            //Added By Shitaljit on 20 june 2014 for warning user in case of overpayment 
            decimal CurrentPaytypeAmount = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
            decimal TotaldueAmount = oPosPTList.amountBalanceDue;
            // Added by Farman Ansari 
            string ActiveRowTranTypeCode = oPosPTList.GetPayType(this.grdPayment.ActiveRow.Cells["transtypecode"].Value.ToString().Trim().ToUpper());

            #region PRIMEPOS-2763 26-Nov-2019 JY Added
            bool bstatus = ValidateOverPayment(ActiveRowTranTypeCode, CurrentPaytypeAmount, TotaldueAmount);
            if (bstatus == false)
            {
                e.Handled = true;
                return false;
            }
            else
                bOverPay = true;
            #endregion

            /// <summary>
            /// Author: Shitaljit 
            /// Added to warn users for overpay in case of check oand CC payments
            /// </summary>
            /// <param name="activeRow"></param>
            ///                      
            // End
            //if (oPosPTList.WarnForOverPayment(ActiveRowTranTypeCode, CurrentPaytypeAmount, TotaldueAmount, oPosPTList.oTransactionType, oPosPTList.oPayTpes) == true)// Modified by Farman Ansari for Warn for overpayment
            //{
            //    if (Resources.Message.Display("Amount you are paying $" + CurrentPaytypeAmount.ToString("######0.00") + " is greater than total due amount $" + TotaldueAmount.ToString("######0.00") + ".\nThis will result in cashback.\nAre you sure you want to proceed?", "Payment Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
            //    {
            //        e.Handled = true;
            //        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].SelectAll();
            //        return false;
            //    }
            //    else
            //    {
            //        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activate();
            //    }
            //}
            //End of added by shitaljit

            //End of Change by SRT (Abhishek D) Date : 12 March 2010
            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
            {
                decimal cellValue = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                if (Convert.ToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim() != "1")
                {
                    if (this.grdPayment.ActiveRow.Index > 0)
                        if ((oPosPTList.totalAmount + oPosPTList.TransFeeAmt + oPosPTList.amountCashBack) - (oPosPTList.amountPaid + cellValue) > 0)    //PRIMEPOS-3117 11-Jul-2022 JY modified
                        {
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            this.grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                            return false;
                        }
                }
            }
            else if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                if (!(ActiveRowTranTypeCode == POS_Core.Resources.PayTypes.CreditCard || ActiveRowTranTypeCode == POS_Core.Resources.PayTypes.DebitCard))   //PRIMEPOS-2463 26-May-2020 JY Added if condition to bypass CC
                {
                    decimal cellValue = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                    //if (this.grdPayment.ActiveRow.Index > 0)  //PRIMEPOS-3023 16-Nov-2021 JY Commented
                    if (ActiveRowTranTypeCode != POS_Core.Resources.PayTypes.Cash) //PRIMEPOS-3023 16-Nov-2021 JY Added
                    {
                        if ((oPosPTList.totalAmount + oPosPTList.amountCashBack) - (oPosPTList.amountPaid + cellValue) < 0)
                        {
                            e.SuppressKeyPress = true;
                            this.grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                            e.Handled = true;
                            return false;
                        }
                    }
                }
            }

            //added By shitaljit to control coupon payment by user rights
            //if user does not have permission then login screen will be shown.
            if (Convert.ToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim() == "C")
            {
                oPosPTList.AllowCouponPayment();
            }
            return true;
        }

        private Boolean AmountColumnProcess(KeyEventArgs e) //PRIMEPOS-2521 22-May-2018 JY changed return type from Void to Boolean as function should return status
        {
            oPosPTList.oPayTpes = oPosPTList.GetPayType(this.grdPayment.ActiveRow.Cells["transtypecode"].Value.ToString().Trim().ToUpper());
            oPosPTList.ProcessDebitCard();
            //Naim 22Oct2007 Look for overpayment if overpayment is greater than $1000 then give user warning
            //so if user has type a huge figure by mistake then he can take care of this.
            bool bTenderedAmountOverrideCancel = false; //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
            if (isOverPayment(out bTenderedAmountOverrideCancel) == true)
            {
                if (Resources.Message.Display("Paid amount is far greater than total due amount.\n Are you sure to continue? ", "Payment Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    oPosPTList.LastKey = "Esc"; //Added by SRT (3-Nov-08).
                    return false;
                }
            }
            else //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added to handle condition when user calcel the override window
            {
                if (bTenderedAmountOverrideCancel == true)
                    return false;
            }

            if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "1")
            {
                AddRowToPaid(this.grdPayment.ActiveRow);
                //CalculateOnReturn();
            }
            else if ((this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "3" ||
                  this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "4" ||
                  this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "5" ||
                  this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "6" ||
                  this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "7" ||
                  this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "-99" ||
                  this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "E"
                 ) && Configuration.isPrimeRxPay == false)//PRIMEPOS-TOKENSALE
            {
                //this.CreditCardProcess(e);
                #region PRIMEPOS-3145 28-Sep-2022 JY Added
                try
                {
                    if (oPosPTList.oTransactionType == POSTransactionType.Sales && oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.AccountNumber != -1 && !oPosPTList.is_F10 && Configuration.CInfo.SaveCCToken)
                    {
                        if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "3" ||
                      this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "4" ||
                      this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "5" ||
                      this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "6" ||
                      this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "-99")
                        {
                            if ((Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "1" && oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.CustomerCode != "-1" && oPosPTList.oCurrentCustRow.SaveCardProfile) || Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "2")
                            {
                                bool tokenizeStatus = true;
                                bool bReturn = false;
                                if (oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.CustomerCode != "-1")
                                {
                                    CCCustomerTokInfo tokinfo = new CCCustomerTokInfo();
                                    using (CCCustomerTokInfoData oCustomerCCProfileTokenData = tokinfo.GetTokenByCustomerandProcessor(oPosPTList.oCurrentCustRow.CustomerId))
                                    {
                                        if (oCustomerCCProfileTokenData != null && oCustomerCCProfileTokenData.Tables.Count > 0 && oCustomerCCProfileTokenData.Tables[0].Rows.Count > 0)
                                            bReturn = this.PressF10KeyOnKeyDown(ref tokenizeStatus);
                                    }
                                }
                                if (bReturn && tokenizeStatus)
                                {
                                    Tokenize = false;   //Token selected so no need to save it
                                }
                                else
                                {
                                    if (tokenizeStatus)
                                    {
                                        if (Resources.Message.Display("Click \"Yes\" to save the card or \"No\" to proceed without saving the card.", "Tokenization", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                        {
                                            Tokenize = true;
                                        }
                                        else
                                        {
                                            Tokenize = false;
                                        }
                                    }
                                    else if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON)
                                    {
                                        Resources.Message.Display("We don't support for tokenize return");
                                        Tokenize = false;
                                    }
                                }
                            }
                            else if ((Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "0" || Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "") && oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.CustomerCode != "-1" && oPosPTList.oCurrentCustRow.SaveCardProfile)    //default behavior if customer card profile turned on
                            {
                                //if (bReturn && tokenizeStatus)
                                //{
                                //    Tokenize = false;   //Token selected so no need to save it
                                //}
                                //else
                                Tokenize = true;
                            }
                        }
                    }
                    this.IsNBSTransDoneOnce = false; //PRIEMPOS-3524 //PRIMEPOS-3504
                    CheckNBSTransaction();//PRIEMPOS-3524 //PRIMEPOS-3504
                    this.CreditCardProcess(e);
                }
                catch (Exception Ex)
                {
                }
                #endregion
            }
            else if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "H")
            {
                //cmt by SRT (Abhishek D) Date : 15 March 2010
                {
                    this.SearchHouseChargeInfo();
                }
            }
            else if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "C")
            {
            }
            else if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "2")
            {
                if (!bAllowCheckPayment)
                    bAllowCheckPayment = oPosPTList.AllowCheckPaymentPriviliges();  //PRIMEPOS-2539 09-Jul-2018 JY Added
                return bAllowCheckPayment;    //PRIMEPOS-2539 09-Jul-2018 JY Added
            }
            else if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "S")// Solutran PRIMEPOS-2663
            {
                if (!CheckExistSolutranTransaction()) // 14_Feb_2020
                {
                    this.SoluTranProcess(e);
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Only one S3 card payment is allowed in a POS transaction.\nThe current S3 Card payment must be voided before another can be attempted.");
                }
            }
            else if (Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim() == "N")//PRIMEPOS-3372-need to change
            {
                if(Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") //PRIMEPOS-3412
                {
                    if(!string.IsNullOrEmpty(Configuration.CSetting.NBSUrl) && !string.IsNullOrEmpty(Configuration.CSetting.NBSEntityID) && !string.IsNullOrEmpty(Configuration.CSetting.NBSStoreID)) //PRIMEPOS-3479-PRIMEPOS-3480
                    {
                        if (!CheckExistNBSTransaction()) //PRIMEPOS-3418
                        {
                            this.CreditCardProcess(e);
                        }
                        else
                        {
                            clsUIHelper.ShowErrorMsg("Only one NB card payment is allowed in a POS transaction.\nThe current NB Card payment must be voided before another can be attempted."); //PRIMEPOS-3482
                        }
                    }
                    else
                    {
                        Resources.Message.Display("Configure NationsBenefits to proceed.", "Payment Failure", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    Resources.Message.Display("Please check the Payment Processor.", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //this.NBSProcess(e);
            }
            else if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "X")// StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019
            {
                this.StoreCreditProcess(e);
            }

            #region PRIMEPOS-2841
            else if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "O" || Configuration.isPrimeRxPay == true)// primepos-2841//PRIMEPOS-TOKENSALE
            {
                string response = Configuration.PrimeRxPayHealthTest();
                if (response != "SUCCESS")
                {
                    Resources.Message.Display("Configuration Missing in PrimeRxPay");
                }
                else
                    this.OnlinePaymentProcess(e);
            }
            #endregion

            else if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "F")//2664
            {
                this.IsFondoUnica = true;
                oPosPTList.oPayTpes = POS_Core.Resources.PayTypes.EBT;
                this.CreditCardProcess(e);
            }

            #region For Newly added POS_Core.Resources.PayTypes
            //Added By Shitaljit  0n 25 Jan 2013
            else
            {
                AddRowToPaid(this.grdPayment.ActiveRow);
            }
            #endregion For Newly added POS_Core.Resources.PayTypes
            return true;
        }


        #region Added for Solutran - PRIMEPOS-2663 - NileshJ 
        private DataTable AddSolutranCard(ref string cardDetails)
        {
            DataTable dtCard = new DataTable();
            try
            {
                logger.Trace("AddSolutranCard() - " + clsPOSDBConstants.Log_Entering);
                frmAddCards oAddCards = new frmAddCards();
                oAddCards.ShowDialog(this);
                cardDetails = oAddCards.Cards;
                if (!oAddCards.IsCanceled)
                {
                    dtCard = oAddCards.dtCardDetails;
                }
                else
                {
                    return dtCard;
                }
            }
            catch (Exception exp)
            {
                logger.Trace(exp, "AddSolutranCard()");
                clsUIHelper.ShowErrorMsg(exp.Message);

            }
            finally
            {
                clsUIHelper.CurrentForm = this;
            }
            logger.Trace("AddSolutranCard() - " + clsPOSDBConstants.Log_Exiting);
            return dtCard;
        }
        private void SoluTranProcess(KeyEventArgs e)
        {
            logger.Trace("SoluTranProcess(KeyEventArgs e)" + clsPOSDBConstants.Log_Entering);
            DataSet oPayTransData = new DataSet();
            String transType = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim();
            transType = "'" + transType + "'";
            string actionCode = string.Empty;
            try
            {
                DataTable dtCardsList = new DataTable();
                string CardDet = string.Empty;
                SoluTranProcessor soluTranProcessor = new SoluTranProcessor();
                this.grdPayment.ActiveRow.Update();
                frmPOSProcessCC oProcessCC = new frmPOSProcessCC(oPosPTList.oTransactionType, oPosPTList.oPayTpes);
                DataSet dsS3TransData = new DataSet();
                //dsS3TransData = posTrans.oTransDData; //PRIMEPOS-2836 15-Apr-2020 JY Commented
                dsS3TransData = posTrans.oTransDData.Copy();    //PRIMEPOS-2836 15-Apr-2020 JY Added

                if (oPosPTList.oTransactionType != POSTransactionType.SalesReturn)
                {
                    // 14_Feb_2020
                    if (dsS3TransData.Tables.Contains("Card Details"))
                    {
                        dsS3TransData.Tables.Remove("Card Details");
                    }
                    dsS3TransData.Merge(AddSolutranCard(ref CardDet));
                }

                DataSet dsTransData = new DataSet();
                if (oPosPTList.oTransactionType == POSTransactionType.Sales && CardDet != "")
                {
                    logger.Trace("soluTranProcessor.AuthorizeDiscount(dsS3TransData, Configuration.CInfo.S3Url, Configuration.CInfo.S3Key, ref s3TransID, ref actionCode, Configuration.StationID, Configuration.CInfo.S3Merchant, Configuration.CInfo.StoreID.Trim().ToUpper())" + clsPOSDBConstants.Log_Entering);

                    dsTransData = soluTranProcessor.AuthorizeDiscount(dsS3TransData, Configuration.CInfo.S3Url, Configuration.CInfo.S3Key, ref s3TransID, ref actionCode, Configuration.StationID, Configuration.CInfo.S3Merchant, Configuration.CInfo.StoreID.Trim().ToUpper());
                    logger.Trace("soluTranProcessor.AuthorizeDiscount(dsS3TransData, Configuration.CInfo.S3Url, Configuration.CInfo.S3Key, ref s3TransID, ref actionCode, Configuration.StationID, Configuration.CInfo.S3Merchant, Configuration.CInfo.StoreID.Trim().ToUpper())" + clsPOSDBConstants.Log_Exiting);
                }
                else if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                {
                    String originalTransID = TransID.ToString();
                    oPayTransData = posTrans.GetTransPaymentDetail(originalTransID, transType);
                    if (oPayTransData != null && oPayTransData.Tables[0].Rows.Count > 0)
                    {
                        frmPaymentReturn oPayTrans = new frmPaymentReturn();
                        oPayTrans.isSolutran = true;// 14_Feb_2020
                        oPayTrans.grdPaymentTrans(oPayTransData);
                        oPayTrans.ShowDialog(this);
                        if (!oPayTrans.IsCanceled)
                        {
                            s3TransID = oPayTrans.ProcessTransID;
                            CardDet = oPayTrans.CardNumber;
                            logger.Trace("soluTranProcessor.Refund(dsS3TransData, Configuration.CInfo.S3Url, Configuration.CInfo.S3Key, s3TransID, CardDet, ref actionCode, Configuration.StationID, Configuration.CInfo.S3Merchant, Configuration.CInfo.StoreID.Trim().ToUpper())" + clsPOSDBConstants.Log_Entering);
                            dsTransData = soluTranProcessor.Refund(dsS3TransData, Configuration.CInfo.S3Url, Configuration.CInfo.S3Key, s3TransID, CardDet, ref actionCode, Configuration.StationID, Configuration.CInfo.S3Merchant, Configuration.CInfo.StoreID.Trim().ToUpper());
                            logger.Trace("soluTranProcessor.Refund(dsS3TransData, Configuration.CInfo.S3Url, Configuration.CInfo.S3Key, s3TransID, CardDet, ref actionCode, Configuration.StationID, Configuration.CInfo.S3Merchant, Configuration.CInfo.StoreID.Trim().ToUpper())" + clsPOSDBConstants.Log_Exiting);
                        }
                        else
                        {
                            logger.Trace("soluTranProcessor.Refund() - Cancel");
                        }
                    }
                }

                if (actionCode == Invalid_S3_Merchant_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_S3_Merchant_ID + "\nMessage       : Invalid S3 Merchant ID", "Solutran Response");
                    return;
                }
                else if (actionCode == Invalid_Card)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_Card + "\nMessage       : Invalid Card", "Solutran Response");
                    return;
                }
                else if (actionCode == Invalid_CVV)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_CVV + "\nMessage       : Invalid CVV", "Solutran Response");
                    return;
                }
                else if (actionCode == No_Benefits)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + No_Benefits + "\nMessage       : No Benefits", "Solutran Response");
                    return;
                }
                else if (actionCode == Card_Not_Active)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Card_Not_Active + "\nMessage       : Card Not Active", "Solutran Response");
                    return;
                }
                else if (actionCode == Format_Error)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Format_Error + "\nMessage       : Format Error", "Solutran Response");
                    return;
                }
                else if (actionCode == Duplicate_TRX)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Duplicate_TRX + "\nMessage       : Duplicate TRX", "Solutran Response");
                    return;
                }
                else if (actionCode == Unmatched_Transaction_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Unmatched_Transaction_ID + "\nMessage       : Unmatched Transaction ID", "Solutran Response");
                    return;
                }
                else if (actionCode == Invalid_TRX_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_TRX_ID + "\nMessage       : Invalid TRX ID", "Solutran Response");
                    return;
                }
                ////
                else if (actionCode == Unmatched_Transaction_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Unmatched_Transaction_ID + "\nMessage       : Unmatched Transaction ID", "Solutran Response");
                    return;
                }
                else if (actionCode == Current_APL_Expired)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Current_APL_Expired + "\nMessage       : Current APL Expired", "Solutran Response");
                    return;
                }
                else if (actionCode == Both_APLs_Expired)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Both_APLs_Expired + "\nMessage       : Both APLs Expired", "Solutran Response");
                    return;
                }
                else if (actionCode == Both_APLs_Good)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Both_APLs_Good + "\nMessage       : Both APLs Good", "Solutran Response");
                    return;
                }
                else if (actionCode == Process_but_suspend_settlement)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Process_but_suspend_settlement + "\nMessage       : Process but suspend settlement if to store, Already Been Cleared ", "Solutran Response");
                    return;
                }
                else if (actionCode == Transaction_Return_not_available)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Transaction_Return_not_available + "\nMessage       : Transaction Return not available", "Solutran Response");
                    return;
                }
                else if (actionCode == Product_not_found_in_Original_Transaction)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Product_not_found_in_Original_Transaction + "\nMessage       : Product not found in Original Transaction", "Solutran Response");
                    return;
                }
                else if (actionCode == Unknown_Error_Contact_Solutran)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Unknown_Error_Contact_Solutran + "\nMessage       : Unknown Error Contact Solutran", "Solutran Response");
                    return;
                }
                else if (actionCode == Requires_manual_exception_handling_Card_Activated)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Requires_manual_exception_handling_Card_Activated + "\nMessage       : Requires manual exception handling Card Activated", "Solutran Response");
                    return;
                }
                else if (actionCode == Card_Activation_Denied)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Card_Activation_Denied + "\nMessage       : Card Activation Denied", "Solutran Response");
                    return;
                }
                else if (actionCode == No_Discount_Available)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + No_Discount_Available + "\nMessage       : No Discount Available", "Solutran Response");
                    return;
                }
                else if (actionCode == Invalid_API_Key)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_API_Key + "\nMessage       : Invalid API Key", "Solutran Response");
                    return;
                }
                else if (actionCode == Endpoint_Not_Found)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Endpoint_Not_Found + "\nMessage       : Endpoint Not Found", "Solutran Response");
                    return;
                }
                else if (actionCode == Expired_Card)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Expired_Card + "\nMessage       : Expired Card", "Solutran Response");
                    return;
                }
                else if (actionCode == Lost_Stolen_Card)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Lost_Stolen_Card + "\nMessage       : Lost/Stolen_Card", "Solutran Response");
                    return;
                }

                if (dsTransData != null && dsTransData.Tables.Count > 0 && actionCode == S3_Success)
                {
                    //posTrans.oTransDData = (POS_Core.CommonData.TransDetailData)dsTransData.Copy();   //PRIMEPOS-2836 15-Apr-2020 JY Commented
                    //posTrans.oTransDData.AcceptChanges(); //PRIMEPOS-2836 15-Apr-2020 JY Commented

                    #region PRIMEPOS-2836 15-Apr-2020 JY Added
                    if (posTrans.oTransDData.Tables[0].Rows.Count > 0)
                    {
                        foreach (TransDetailRow oRow in dsTransData.Tables[0].Rows)
                        {
                            if (oRow.S3TransID > 0)
                            {
                                foreach (TransDetailRow row in posTrans.oTransDData.TransDetail.Rows)
                                {
                                    if (oRow.TransDetailID == row.TransDetailID)
                                    {
                                        row.S3TransID = oRow.S3TransID;
                                        row.S3PurAmount = oRow.S3PurAmount;
                                        row.S3DiscountAmount = oRow.S3DiscountAmount;
                                        row.S3TaxAmount = oRow.S3TaxAmount;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region PRIMEPOS-2836 15-Apr-2020 JY Added
                    frmSolutranDetails objfrmSolutranDetails = new frmSolutranDetails();
                    dsTransData.Tables[0].DefaultView.RowFilter = "ISNULL(s3TransID,0) <> 0";
                    DataTable dtS3Items = (dsTransData.Tables[0].DefaultView).ToTable();
                    if(oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                    {
                        objfrmSolutranDetails.IsReturnTransaction = true;
                    }
                    objfrmSolutranDetails.dtS3Items = dtS3Items;
                    objfrmSolutranDetails.ShowDialog();

                    if (objfrmSolutranDetails.IsCanceled)
                    {
                        if(oPosPTList.oTransactionType != POSTransactionType.SalesReturn)
                        {
                            RemoveSolutranPayment(s3TransID);
                        }
                        return;
                    }
                    #endregion

                    decimal amtBalanceDue = 0;

                    decimal TotalTranAmt = 0;
                    decimal TotalS3DiscAmt = 0;
                    for (int i = 0; dsTransData.Tables[0].Rows.Count > i; i++)
                    {
                        TotalTranAmt += (Convert.ToDecimal(dsTransData.Tables[0].Rows[i]["TaxAmount"].ToString()) + Convert.ToDecimal(dsTransData.Tables[0].Rows[i]["ExtendedPrice"].ToString())) - Convert.ToDecimal(dsTransData.Tables[0].Rows[i]["Discount"].ToString());
                        if (Configuration.convertNullToInt64(dsTransData.Tables[0].Rows[i]["S3TransID"]) != 0) //PRIMEPOS-3265
                        {
                            TotalS3DiscAmt += Convert.ToDecimal(dsTransData.Tables[0].Rows[i]["S3DiscountAmount"]);
                        }
                    }

                    if (oPosPTList.oTransactionType == POSTransactionType.Sales)
                        clsUIHelper.ShowSuccessMsg("Action Code : " + S3_Success + "\nMessage       : Approved\nAmount        : " + TotalS3DiscAmt + "\nTrans Type   : Sales", "Solutran Response");
                    else if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                        clsUIHelper.ShowSuccessMsg("Action Code : " + S3_Success + "\nMessage       : Approved\nAmount        : " + TotalS3DiscAmt + "\nTrans Type   : Return", "Solutran Response");
                    else
                        clsUIHelper.ShowSuccessMsg("Action Code : " + S3_Success + "\nMessage       : Approved\nAmount        : " + TotalS3DiscAmt + "\nTrans Type   : Void", "Solutran Response");

                    #region  Disable Button for refund S3 Transaction
                    if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                    {
                        btnCancel.Enabled = false;
                        btnRemovePayment.Enabled = false;
                        btnClose.Enabled = false;
                    }
                    #endregion

                    Amount = string.Empty;

                    if (TotalS3DiscAmt > 0)
                    {
                        this.grdPayment.ActiveRow.Update();
                        string payTypeId = "S";
                        oProcessCC.isFSATransaction = false;
                        oProcessCC.isManualProcess = "";
                        oProcessCC.SubCardType = "SolutranDiscount";
                        oPosPTList.S3TransID = s3TransID;
                        oPosPTList.S3PurAmount = TotalS3DiscAmt;// TotalTranAmt;
                        oPosPTList.S3TaxAmount = 0;
                        oPosPTList.S3DiscAmount = TotalS3DiscAmt;
                        oProcessCC.ApprovedPCCCardInfo = new PccCardInfo();
                        oProcessCC.ApprovedPCCCardInfo.tRoutId = s3TransID;
                        //oProcessCC.Amount = TotalS3DiscAmt;
                        oProcessCC.Amount = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);
                        this.ProcessPayTypeId(oProcessCC, ref payTypeId);
                        if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                        {
                            this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1) * TotalS3DiscAmt;
                            TotalS3DiscAmt = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);
                        }
                        else
                        {
                            this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = TotalS3DiscAmt;// oProcessCC.Amount;
                        }
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Value = 0;
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = CardDet;
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].Value = "Solutran";
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = false;
                        ProcessTransactionPaymentClog(oProcessCC, ref payTypeId);
                        #region Reversal PRIMEPOS-2887
                        if (IsSolutranException)
                        {
                            TotalTranAmt = oPosPTList.amountBalanceDue;
                            TotalS3DiscAmt = 0;
                        }
                        #endregion
                        amtBalanceDue = TotalTranAmt - TotalS3DiscAmt;
                        oPosPTList.amountBalanceDue = amtBalanceDue;
                        if (oPosPTList.amountBalanceDue != 0)
                            bPaymentRowAdded = true;
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue.ToString("###########0.00");
                        oProcessCC.ApprovedPCCCardInfo.tRoutId = string.Empty;
                        this.grdPayment.Update();
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Payment is not Successful");
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #region PRIMEPOS-2841
        private void OnlinePaymentProcess(KeyEventArgs e)
        {
            try
            {
                string userID = Configuration.CSetting.PrimeRxPayClientId;
                this.grdPayment.ActiveRow.Update();
                frmPOSOnlineCC onlineProcessCC = new frmPOSOnlineCC(oPosPTList.oTransactionType, oPosPTList.oPayTpes, "PRIMERXPAY");
                #region PRIMEPOS-TOKENSALE
                Configuration.isPrimeRxPay = onlineProcessCC.Tokenize = Tokenize;
                if (!oPosPTList.is_F10)
                {
                    if (oPosPTList.oTransactionType == POSTransactionType.Sales)
                    {
                        #region PRIMEPOS-3145 28-Sep-2022 JY Added
                        if (Configuration.CInfo.SaveCCToken)
                        {
                            if ((Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "1" && oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.CustomerCode != "-1" && oPosPTList.oCurrentCustRow.SaveCardProfile) || Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "2")
                            {
                                bool tokenizeStatus = true;
                                bool bReturn = false;
                                if (oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.AccountNumber != -1)
                                {
                                    CCCustomerTokInfo tokinfo = new CCCustomerTokInfo();
                                    using (CCCustomerTokInfoData oCustomerCCProfileTokenData = tokinfo.GetTokenByCustomerandProcessor(oPosPTList.oCurrentCustRow.CustomerId))
                                    {
                                        if (oCustomerCCProfileTokenData != null && oCustomerCCProfileTokenData.Tables.Count > 0 && oCustomerCCProfileTokenData.Tables[0].Rows.Count > 0)
                                            bReturn = this.PressF10KeyOnKeyDown(ref tokenizeStatus);
                                    }
                                }
                                else
                                {
                                    tokenizeStatus = false;
                                }
                                if (bReturn && tokenizeStatus)
                                {
                                    Tokenize = false;   //Token selected so no need to save it
                                }
                                else
                                {
                                    if (tokenizeStatus)
                                    {
                                        if (Resources.Message.Display("Click \"Yes\" to save the card or \"No\" to proceed without saving the card.", "Tokenization", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                        {
                                            Tokenize = true;
                                        }
                                        else
                                        {
                                            Tokenize = false;
                                        }
                                    }
                                    //else if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON)
                                    //{
                                    //    Resources.Message.Display("We don't support for tokenize return");
                                    //    Tokenize = false;
                                    //}
                                }
                            }
                            else if ((Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "0" || Configuration.convertNullToString(Configuration.CSetting.PromptToSaveCCToken).Trim() == "") && oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.CustomerCode != "-1" && oPosPTList.oCurrentCustRow.SaveCardProfile)    //default behavior if customer card profile turned on
                            {
                                //if (bReturn && tokenizeStatus)
                                //{
                                //    Tokenize = false;   //Token selected so no need to save it
                                //}
                                //else
                                Tokenize = true;
                            }
                        }
                        #endregion

                        DialogResult dialogResult;
                        //PRIMEPOS-2915
                        if (Configuration.CSetting.OnlineOption == Configuration.OnlinePayment.AskForOption.ToString())
                        {
                            dialogResult = Resources.Message.Display("Do you want send PrimeRxPay payment link to the customer? \n This will hold the transaction until the payment is completed.", "Pay Later", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        }
                        else if (Configuration.CSetting.OnlineOption == Configuration.OnlinePayment.NoOnlinePayment.ToString())
                        {
                            dialogResult = DialogResult.No;
                        }
                        else
                        {
                            dialogResult = DialogResult.Yes;
                        }

                        if (dialogResult == DialogResult.Yes)
                        {
                            frmCustomerDetailsPrimeRxPay customerDetailsPrimeRxPay = new frmCustomerDetailsPrimeRxPay();
                            if (oPosPTList.oCurrentCustRow.CustomerCode != "-1")
                            {
                                customerDetailsPrimeRxPay.IsSelectedCustomer = true;
                                customerDetailsPrimeRxPay.CustomerCode = oPosPTList.oCurrentCustRow.CustomerId.ToString();
                            }
                            customerDetailsPrimeRxPay.ShowDialog();

                            if (customerDetailsPrimeRxPay.IsCancel)
                            {
                                logger.Trace("Canceled from Customer Driven");
                                return;
                            }
                            else if (customerDetailsPrimeRxPay.IsCustomerDriven)
                            {
                                logger.Trace("Setting values on Customer Driven");
                                onlineProcessCC.IsEmail = customerDetailsPrimeRxPay.IsEmail;
                                onlineProcessCC.IsPhone = customerDetailsPrimeRxPay.IsMobile;
                                onlineProcessCC.Email = this.Email = customerDetailsPrimeRxPay.Email;
                                onlineProcessCC.Phone = this.Mobile = customerDetailsPrimeRxPay.Mobile;
                                onlineProcessCC.DOB = customerDetailsPrimeRxPay.DOB;
                                onlineProcessCC.IsPrimeRxPayLinkSend = customerDetailsPrimeRxPay.IsPrimeRxPayLinkSend; //PRIMEPOS-3248
                                this.TransactionProcessingMode = customerDetailsPrimeRxPay.IsMobile ? 1 : 2;
                                onlineProcessCC.CustomerName = customerDetailsPrimeRxPay.Name;
                                //onlineProcessCC.DOB = customerDetailsPrimeRxPay.dob
                                onlineProcessCC.IsCustomerDriven = customerDetailsPrimeRxPay.IsCustomerDriven;
                                oPosPTList.oCurrentCustRow = customerDetailsPrimeRxPay.oCustData?.Customer[0];
                                lblCustomerName.Text = oPosPTList.oCurrentCustRow.CustomerFullName;
                            }

                        }
                        else if (dialogResult == DialogResult.Cancel)
                        {
                            logger.Trace("Aborted from Customer Driven");
                            Configuration.isPrimeRxPay = false; //PRIMEPOS-3432
                            return;
                        }
                    }
                }
                Fetchis_F10_PrimeRxPay(onlineProcessCC);
                #endregion

                if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn != true)
                {
                    clsUIHelper.ShowErrorMsg("Returns are only possible with PrimeRxPay if strict return is enabled. Please enable this setting");
                    Configuration.isPrimeRxPay = false; //PRIMEPOS-3432
                    return;
                }
                if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true)
                {
                    if (chkReversal(onlineProcessCC) == false)
                    {
                        Configuration.isPrimeRxPay = false; //PRIMEPOS-3432
                        return;
                    }
                }
                if ((Configuration.CInfo.EBTAsManualTrans == false && oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT) || oPosPTList.oPayTpes != POS_Core.Resources.PayTypes.EBT)
                {
                    #region PRIMEPOS-2738
                    if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true && onlineProcessCC.Amount != 0)//PRIMEPOS-2738
                    {
                        onlineProcessCC.Amount = decimal.Round(Convert.ToDecimal(Amount), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        onlineProcessCC.Amount = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);
                        #region PRIMEPOS-3117 11-Jul-2022 JY Added
                        if (onlineProcessCC.Amount != 0)
                        {
                            decimal OTCAmount, RxAmount;
                            if (GetTransFeeApplicableFor(out OTCAmount, out RxAmount))
                            {
                                TransFee oTransFee = new TransFee();
                                TransFeeData oTransFeeData = oTransFee.GetTransFeeDataByPayTypeID(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim(), oPosPTList.oTransactionType);
                                if (oTransFeeData != null && oTransFeeData.Tables.Count > 0 && oTransFeeData.TransFee.Rows.Count > 0)
                                {
                                    frmTransactionFee ofrmTransactionFee = null;
                                    if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && onlineProcessCC.Amount != 0)
                                        ofrmTransactionFee = new frmTransactionFee(oTransFeeData, -1 * Math.Abs(onlineProcessCC.Amount), -1 * Math.Abs(oPosPTList.totalAmount), OTCAmount, RxAmount);
                                    else
                                        ofrmTransactionFee = new frmTransactionFee(oTransFeeData, onlineProcessCC.Amount, oPosPTList.totalAmount, OTCAmount, RxAmount);
                                    if (ofrmTransactionFee.ShowDialog(this) == DialogResult.Cancel)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && onlineProcessCC.Amount != 0)
                                            onlineProcessCC.Amount = Math.Abs(ofrmTransactionFee.FinalAmount);
                                        else
                                            onlineProcessCC.Amount = ofrmTransactionFee.FinalAmount;
                                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt].Value = ofrmTransactionFee.TransFeeAmt;
                                        oPosPTList.TransFeeAmt = ofrmTransactionFee.TransFeeAmt;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                    onlineProcessCC.FSAAmount = oPosPTList.GetIIASAmount(totalNonIIASAmount);
                    //Added By SRT(Ritesh Parekh) Date : 18-Aug-2009
                    onlineProcessCC.FSARxAmount = oPosPTList.GetIIASRxAmount(totalNonIIASAmount);
                    //End Of Added by SRT(Ritesh Parekh)                
                    //added by akbar
                    onlineProcessCC.CustomerCardInfo = oPosPTList.CustomerCardInfo; //customer card info if retrieved        
                                                                                    //PRIMEPOS-2738 ADDED BY ARVIND 
                    onlineProcessCC.originalTransID = ProcessTransID;
                    onlineProcessCC.hrefNumber = HrefNumber;
                    // 
                    onlineProcessCC.ShowDialog(this);

                    if (onlineProcessCC.ticketNum != null)
                    {
                        this.ticketNum = onlineProcessCC.ticketNum;
                    }
                    else
                    {
                        this.ticketNum = null;
                    }
                    if (onlineProcessCC.isCanceled == true)
                    {
                        CancelIsValidSignPad(e);
                        Configuration.isPrimeRxPay = false;//PRIMEPOS-2902
                        if (onlineProcessCC.IsCustomerDriven && !string.IsNullOrWhiteSpace(onlineProcessCC.PrimeRxPayTransID))//2915
                        {
                            this.IsStatusPending = true;
                            this.IsCustomerDriven = onlineProcessCC.IsCustomerDriven;
                            this.PrimeRxPayTransID = onlineProcessCC.PrimeRxPayTransID;
                            this.pccCardInfo = onlineProcessCC.ApprovedPCCCardInfo;
                            //PRIMEPOS-3117 11-Jul-2022 JY Added
                            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1 * Math.Abs(onlineProcessCC.Amount)).ToString("###########0.00");
                            else
                                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = onlineProcessCC.Amount.ToString("###########0.00");
                            AddRowToPaid(this.grdPayment.ActiveRow);
                            string payTypeId = string.Empty;
                            ProcessTransactionPaymentClogCustomerDriven(oPosPTList.oPOSTransPaymentDataPaid, ref payTypeId);
                            this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue.ToString("###########0.00");
                            this.grdPayment.Update();
                            //this.Close();
                        }
                    }
                    else
                    {
                        this.TransNo = onlineProcessCC.ApprovedPCCCardInfo.tRoutId;//PRIMEPOS-2887
                        this.pccCardInfo = onlineProcessCC.ApprovedPCCCardInfo;
                        string payTypeId = string.Empty;
                        this.ProcessPayTypeId(onlineProcessCC, ref payTypeId);
                        this.ProcessPrimeRxPay(onlineProcessCC);
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = onlineProcessCC.isFSATransaction;
                        if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                            this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1) * Math.Abs(onlineProcessCC.Amount);
                        else
                            this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = onlineProcessCC.Amount;

                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].Value = onlineProcessCC.PaymentProcessor;
                        this.grdPayment.Update();

                        ProcessTransactionPaymentClog(onlineProcessCC, ref payTypeId);
                    }
                    if (oPosPTList.amountBalanceDue != 0)
                        bPaymentRowAdded = true;    //PRIMEPOS-2499 17-May-2018 JY Added    
                                                    //PRIMEPOS-2738
                    if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true && onlineProcessCC.isCanceled != true)
                    {
                        updRevAmtInOrigTxn();
                        TransPayID = string.Empty;
                    }
                    //
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue.ToString("###########0.00");
                    this.grdPayment.Update();
                    Configuration.isPrimeRxPay = false;//PRIMEPOS-3189 //PRIMEPOS-3249 commented this line //PRIMEPOS-3432
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }
        #endregion
        private string RemoveSolutranPayment(string sTransID)
        {
            string response = string.Empty;
            try
            {
                SoluTranProcessor soluTranProcessor = new SoluTranProcessor();

                DataSet dsS3TransData = new DataSet();
                //dsS3TransData = posTrans.oTransDData; //PRIMEPOS-2836 15-Apr-2020 JY Commented
                dsS3TransData = posTrans.oTransDData.Copy();    //PRIMEPOS-2836 15-Apr-2020 JY Added

                response = soluTranProcessor.Void(dsS3TransData, Configuration.CInfo.S3Url, Configuration.CInfo.S3Key, sTransID, Configuration.StationID, Configuration.CInfo.S3Merchant, Configuration.CInfo.StoreID.Trim().ToUpper());

                #region Action Messages
                string strTransTypeMsg = " \nTrans Type   : Void";
                if (response == Invalid_S3_Merchant_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_S3_Merchant_ID + "\nMessage       : Invalid S3 Merchant ID" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Invalid_Card)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_Card + "\nMessage       : Invalid Card" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Invalid_CVV)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_CVV + "\nMessage       : Invalid CVV" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == No_Benefits)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + No_Benefits + "\nMessage       : No Benefits" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Card_Not_Active)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Card_Not_Active + "\nMessage       : Card Not Active" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Format_Error)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Format_Error + "\nMessage       : Format Error" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Duplicate_TRX)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Duplicate_TRX + "\nMessage       : Duplicate TRX" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Unmatched_Transaction_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Unmatched_Transaction_ID + "\nMessage       : Unmatched Transaction ID" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Invalid_TRX_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_TRX_ID + "\nMessage       : Invalid TRX ID" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == S3_Success)
                {
                    clsUIHelper.ShowSuccessMsg("Action Code : " + S3_Success + "\nMessage       : Approved" + strTransTypeMsg, "Solutran Response");
                }
                ////
                else if (response == Unmatched_Transaction_ID)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Unmatched_Transaction_ID + "\nMessage       : Unmatched Transaction ID" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Current_APL_Expired)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Current_APL_Expired + "\nMessage       : Current APL Expired" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Both_APLs_Expired)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Both_APLs_Expired + "\nMessage       : Both APLs Expired" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Both_APLs_Good)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Both_APLs_Good + "\nMessage       : Both APLs Good" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Process_but_suspend_settlement)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Process_but_suspend_settlement + "\nMessage       : Process but suspend settlement if to store, Already Been Cleared " + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Transaction_Return_not_available)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Transaction_Return_not_available + "\nMessage       : Transaction Return not available" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Product_not_found_in_Original_Transaction)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Product_not_found_in_Original_Transaction + "\nMessage       : Product not found in Original Transaction" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Unknown_Error_Contact_Solutran)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Unknown_Error_Contact_Solutran + "\nMessage       : Unknown Error Contact Solutran" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Requires_manual_exception_handling_Card_Activated)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Requires_manual_exception_handling_Card_Activated + "\nMessage       : Requires manual exception handling Card Activated" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Card_Activation_Denied)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Card_Activation_Denied + "\nMessage       : Card Activation Denied" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == No_Discount_Available)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + No_Discount_Available + "\nMessage       : No Discount Available" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Invalid_API_Key)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Invalid_API_Key + "\nMessage       : Invalid API Key" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Endpoint_Not_Found)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Endpoint_Not_Found + "\nMessage       : Endpoint Not Found" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Expired_Card)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Expired_Card + "\nMessage       : Expired Card" + strTransTypeMsg, "Solutran Response");
                }
                else if (response == Lost_Stolen_Card)
                {
                    clsUIHelper.ShowErrorMsg("Action Code : " + Lost_Stolen_Card + "\nMessage       : Lost/Stolen_Card" + strTransTypeMsg, "Solutran Response");
                }
                #endregion
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return response;
        }

        #region PRIMEPOS-2841
        private bool RemoveOnlinePayment(UltraGridRow oRow)
        {
            if (oPosPTList.oPOSTransPayment_CCLogList.Count > 0)
            {
                foreach (POSTransPayment_CCLog oLog in oPosPTList.oPOSTransPayment_CCLogList)
                {
                    //Modified By Dharmendra SRT on Mar-10-09 included formatting in Amount.ToString()
                    if (oLog.Amount.ToString("###########0.00") == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text &&
                        oLog.RefNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Text &&
                        oLog.AuthNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Text)
                    {
                        frmPOSOnlineCC oProcessOnline = new frmPOSOnlineCC(oPosPTList.oTransactionType, "", "PRIMERXPAY");
                        oLog.PccCardInfo.transAmount = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text.ToString(); //Added by Manoj 7/2/2012
                        oLog.PccCardInfo.cardType = "CC";//PRIMEPOS-3189
                        if (oProcessOnline.PerformVoidTransaction(oLog.PccCardInfo, oPosPTList.oTransactionType) == true)
                        {
                            oRow.Delete(false);
                            this.grdPaymentComplete.UpdateData();
                            oPosPTList.oPOSTransPayment_CCLogList.Remove(oLog);
                            return true;
                        }
                        break;
                    }
                }
            }
            return false;
        }
        #endregion
        // 14_Feb_2020
        private bool CheckExistSolutranTransaction()
        {
            bool isExist = false;
            UltraGridBand bandMaster = this.grdPaymentComplete.DisplayLayout.Bands[0];
            foreach (UltraGridRow solrow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            {

                if (solrow.Cells["PayTypeDesc"].Value.ToString().Contains("Solutran"))
                {
                    isExist = true;
                    return isExist;
                }
            }
            return isExist;
        }
        #endregion

        #region PRIMEPOS-3418
        private bool CheckExistNBSTransaction()
        {
            bool isExist = false;
            UltraGridBand bandMaster = this.grdPaymentComplete.DisplayLayout.Bands[0];
            foreach (UltraGridRow solrow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            {

                if (Convert.ToString(solrow.Cells["PaymentProcessor"].Value).Contains("NB_VANTIV")) //PRIMEPOS-3482
                {
                    this.IsNBSTransDoneOnce = true; //PRIEMPOS-3524 //PRIEMPOS-3504
                    return isExist;
                }
            }
            return isExist;
        }

        private void CheckNBSTransaction() //PRIEMPOS-3524 //PRIEMPOS-3504
        {
            UltraGridBand bandMaster = this.grdPaymentComplete.DisplayLayout.Bands[0];
            foreach (UltraGridRow solrow in bandMaster.GetRowEnumerator(GridRowType.DataRow))
            {

                if (Convert.ToString(solrow.Cells["PaymentProcessor"].Value).Contains("NB_VANTIV")) //PRIMEPOS-3482
                {
                    this.IsNBSTransDoneOnce = true;
                }
            }
        }
        #endregion

        private void CreditCardProcess(KeyEventArgs e)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Process CC paymnet", clsPOSDBConstants.Log_Entering);
            logger.Trace("CreditCardProcess(KeyEventArgs e) - Process CC paymnet - " + clsPOSDBConstants.Log_Entering);
            this.grdPayment.ActiveRow.Update();
            frmPOSProcessCC oProcessCC = new frmPOSProcessCC(oPosPTList.oTransactionType, oPosPTList.oPayTpes);
            oProcessCC.Tokenize = Tokenize;

            oProcessCC.IsElavonTax = this.IsElavonTax;//2943
            oProcessCC.ElavonTotalTax = this.ElavonTotalTax;//2943
            oProcessCC.IsFondoUnica = IsFondoUnica;//2664
            oProcessCC.lineItemsdata = StoreLineItems(); //PRIMEPOS-3372
            
            // NileshJ - Set TicketNum for ProcessCC - 20-Dec-2018
            if (SigPadUtil.DefaultInstance.isPAX)
                oProcessCC.ticketNum = this.ticketNum;

            if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
            {
                oProcessCC.EvertecTaxDetails = this.EvertecTaxDetails;//2857
                oProcessCC.CashBackAmount = this.CashBackAmount;//2857
            }

            Fetchis_F10(oProcessCC);
            #region PRIMEPOS-2738 
            // if txnType=refund and strict reversal
            //FOR CHECKING THE AMOUNT NOT GREATER THAN THE SELECTED TRANSACTION, FOR UPDATING THE AMOUNT IN DATASET AND UPDATING IN DATABSE AFTER COMPLITING THE WHOLE TRANSACTION DOUBTFULL
            //While testing check for debit and EBT also
            if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true)
            {
                if (chkReversal(oProcessCC) == false)
                {
                    return;
                }
            }
            #endregion

            if ((Configuration.CInfo.EBTAsManualTrans == false && oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT) || oPosPTList.oPayTpes != POS_Core.Resources.PayTypes.EBT)
            {
                #region PRIMEPOS-2738
                if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true && oProcessCC.Amount != 0)//PRIMEPOS-2738
                {
                    oProcessCC.Amount = decimal.Round(Convert.ToDecimal(Amount), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    oProcessCC.Amount = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);
                }
                #endregion
                //if (Configuration.CPOSSet.PaymentProcessor == "ELAVON" && (oPosPTList.totalNonIIASAmountPaid != 0) || totalNonIIASAmount != 0)//2943
                //{
                //    if (totalNonIIASAmount != 0 && ((this.PendingAmount != oPosPTList.totalIIASAmountPaid) && this.PendingAmount !=0))
                //    {
                //        oProcessCC.FSAAmount = oPosPTList.GetIIASAmount(totalNonIIASAmount);                        
                //        oProcessCC.FSARxAmount = oPosPTList.GetIIASRxAmount(totalNonIIASAmount);
                //    }
                //    //else if (oPosPTList.totalNonIIASAmountPaid != 0)
                //    //{
                //    //    oProcessCC.FSAAmount = oPosPTList.totalNonIIASAmountPaid;                        
                //    //    oProcessCC.FSARxAmount = oPosPTList.totalNonIIASAmountPaid;
                //    //}
                //}
                //else
                //{
                oProcessCC.FSAAmount = oPosPTList.GetIIASAmount(totalNonIIASAmount);
                //Added By SRT(Ritesh Parekh) Date : 18-Aug-2009
                oProcessCC.FSARxAmount = oPosPTList.GetIIASRxAmount(totalNonIIASAmount);
                //}
                //End Of Added by SRT(Ritesh Parekh)
                //Added by SRT(Abhishek) Date : 11 Aug 08
                EBTSalesFSAAmount(oProcessCC);
                //End of Added by SRT(Abhishek) Date : 11 Aug 08

                //added by akbar
                oProcessCC.CustomerCardInfo = oPosPTList.CustomerCardInfo; //customer card info if retrieved        
                //PRIMEPOS-2738 ADDED BY ARVIND 
                oProcessCC.originalTransID = ProcessTransID;
                //oProcessCC.ExpirayDate = oPosPTList.CustomerCardInfo.
                oProcessCC.hrefNumber = HrefNumber;
                oProcessCC.NBSReturnUid = nbsUid; //PRIMEPOS-3375
                oProcessCC.NBSPaymentType = nbsType; //PRIMEPOS-3375
                oProcessCC.IsNBSTransDoneOnce = IsNBSTransDoneOnce; //PRIMEPOS-3524 //PRIEMPOS-3504

                if (Configuration.CPOSSet.PaymentProcessor == "VANTIV") //PRIEMPOS-3521 //PRIEMPOS-3504
                {
                    if (!string.IsNullOrWhiteSpace(TransTypeCode))
                    {
                        if(TransTypeCode.ToUpper().Trim() == "N")
                        {
                            oProcessCC.isNBSPayment = true;
                        }
                        else if(TransTypeCode.ToUpper().Trim() == "7")
                        {
                            oProcessCC.ReturnTransType = "DEBIT";
                        }
                    }
                }
                // 
                oProcessCC.TransDate = TransDate;//2943
                oProcessCC.PendingAmount = this.PendingAmount;//2943
                #region PRIMEPOS-3117 11-Jul-2022 JY Added
                decimal OriginalAmount = oProcessCC.Amount;
                if (oProcessCC.Amount != 0)
                {
                    decimal OTCAmount, RxAmount;
                    if (GetTransFeeApplicableFor(out OTCAmount, out RxAmount))
                    {
                        TransFee oTransFee = new TransFee();
                        TransFeeData oTransFeeData = oTransFee.GetTransFeeDataByPayTypeID(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim(), oPosPTList.oTransactionType);
                        if (oTransFeeData != null && oTransFeeData.Tables.Count > 0 && oTransFeeData.TransFee.Rows.Count > 0)
                        {
                            frmTransactionFee ofrmTransactionFee = null;
                            if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && oProcessCC.Amount != 0)
                                ofrmTransactionFee = new frmTransactionFee(oTransFeeData, -1 * Math.Abs(oProcessCC.Amount), -1 * Math.Abs(oPosPTList.totalAmount), OTCAmount, RxAmount);
                            else
                                ofrmTransactionFee = new frmTransactionFee(oTransFeeData, oProcessCC.Amount, oPosPTList.totalAmount, OTCAmount, RxAmount);
                            if (ofrmTransactionFee.ShowDialog(this) == DialogResult.Cancel)
                            {
                                return;
                            }
                            else
                            {
                                if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && oProcessCC.Amount != 0)
                                    oProcessCC.Amount = Math.Abs(ofrmTransactionFee.FinalAmount);
                                else
                                    oProcessCC.Amount = ofrmTransactionFee.FinalAmount;
                                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt].Value = ofrmTransactionFee.TransFeeAmt;
                                oPosPTList.TransFeeAmt = ofrmTransactionFee.TransFeeAmt;
                            }
                        }
                    }
                }
                #endregion
                oProcessCC.ShowDialog(this);
                this.PendingAmount = oProcessCC.PendingAmount;//2943
                this.IsFsaTransaction = oProcessCC.isFSATransaction;//2943
                this.ExpiryDate = oProcessCC.ExpirayDate;//2943
                this.NBSPaymentType = oProcessCC.NBSTransType;//PRIMEPOS-3375
                this.NBSTransID = oProcessCC.NBSTransId;//PRIMEPOS-3375
                this.IsNBSTransaction = oProcessCC.isNBSPayment;//PRIMEPOS-3526 //PRIEMPOS-3504
                this.paymentType = oProcessCC.PaymentType;//PRIMEPOS-3519 //PRIEMPOS-3504
                this.NBSUid = oProcessCC.NBSUid;//PRIMEPOS-3375
                //NileshJ - ticketnum - 20-Dec-2018
                if (SigPadUtil.DefaultInstance.isPAX)
                {
                    if (oProcessCC.ticketNum != null)
                    {
                        this.ticketNum = oProcessCC.ticketNum;
                    }
                    else
                    {
                        this.ticketNum = null;
                    }
                }
                //
                if (oProcessCC.isCanceled == true)
                {
                    #region PRIMEPOS-3117 11-Jul-2022 JY Added
                    if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && oProcessCC.Amount != 0)
                        oProcessCC.Amount = Math.Abs(OriginalAmount);
                    else
                        oProcessCC.Amount = OriginalAmount;
                    oPosPTList.TransFeeAmt = 0;
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt].Value = 0;
                    #endregion
                    CancelIsValidSignPad(e);
                }
                else
                {
                    if (this.IsFondoUnica)//2664
                    {
                        oPosPTList.oPayTpes = POS_Core.Resources.PayTypes.CreditCard;
                        this.IsFondoUnica = false;
                    }
                    this.TransNo = oProcessCC.ApprovedPCCCardInfo.tRoutId;//PRIMEPOS-2887
                    this.pccCardInfo = oProcessCC.ApprovedPCCCardInfo;
                    this.CheckCCProceesorSwitched(oProcessCC);
                    this.CheckOfflineTransaction(oProcessCC);
                    this.CheckWOLRLPAYDebitCCAndCreditCC(oProcessCC);
                    string payTypeId = string.Empty;
                    this.ProcessPayTypeId(oProcessCC, ref payTypeId);
                    this.ProcessWORLDPAYFirstMile(oProcessCC);
                    this.ProcessSign(oProcessCC);
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = oProcessCC.isFSATransaction;
                    if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                    {
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1) * Math.Abs(oProcessCC.Amount);
                    }
                    if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oProcessCC.Amount;
                    }

                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].Value = oProcessCC.PaymentProcessor;
                    this.grdPayment.Update();

                    ProcessTransactionPaymentClog(oProcessCC, ref payTypeId);
                }
                if (oPosPTList.amountBalanceDue != 0)
                    bPaymentRowAdded = true;    //PRIMEPOS-2499 17-May-2018 JY Added    
                //PRIMEPOS-2738
                if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn && Configuration.CSetting.StrictReturn == true && oProcessCC.isCanceled != true)
                {
                    updRevAmtInOrigTxn();
                    TransPayID = string.Empty;
                }
                //
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue.ToString("###########0.00");
                this.grdPayment.Update();
            }
        }

        #region PRIMEPOS-3372
        private List<AnalyzeLineItem> StoreLineItems()
        {
            DataSet dsTransData = new DataSet();
            dsTransData = posTrans.oTransDData.Copy();
            List<AnalyzeLineItem> lineItemsdata = new List<AnalyzeLineItem>();

            if (dsTransData.Tables.Count > 0 && dsTransData.Tables["POSTransactionDetail"].Rows.Count > 0)
            {
                foreach (DataRow row in dsTransData.Tables["POSTransactionDetail"].Rows)
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(row["ItemID"])) && Convert.ToString(row["ItemID"]).All(char.IsDigit)) //PRIMEPOS-3407 //PRIMEPOS-3417
                    {
                        List<AnalyzeTax> taxes = new List<AnalyzeTax>();

                        if (!string.IsNullOrWhiteSpace(Convert.ToString(row["TaxCode"])))
                        {
                            AnalyzeTax tax = new AnalyzeTax();

                            if (NBS_SALES_TYPE.Contains(Convert.ToString(row["TaxCode"]).ToUpper()))
                            {
                                tax.TaxType = Convert.ToString(row["TaxCode"]).ToUpper();
                            }
                            else
                            {
                                tax.TaxType = "OTHER";
                            }
                            tax.Value = Convert.ToDecimal(row["TaxAmount"]);
                            taxes.Add(tax);
                        }

                        AnalyzeLineItem lineItem = new AnalyzeLineItem
                        {
                            LineItemNumber = lineItemsdata.Count + 1,
                            ProductCode = Convert.ToString(row["ItemID"]),
                            ProductCodeType = "UPC",
                            UnitPrice = (Convert.ToDecimal(row["ExtendedPrice"]) - Convert.ToDecimal(row["Discount"])) / Math.Abs(Convert.ToInt32(row["Qty"])),
                            Quantity = Math.Abs(Convert.ToInt32(row["Qty"])),
                            Units = "COUNT",
                            Taxes = taxes.Count == 0 ? null : taxes,
                            Fees = null
                        };
                        lineItemsdata.Add(lineItem);
                    }
                }

            }
            return lineItemsdata;
        }
        #endregion

        private void Fetchis_F10(frmPOSProcessCC oProcessCC)
        {

            if (oPosPTList.is_F10)// Added by Manoj 8/23/2011
            {
                oProcessCC.isF10(oPosPTList.is_F10, oPosPTList.CustomerCardInfo.cardNumber, oPosPTList.CustomerCardInfo.cardExpiryDate);
                oPosPTList.is_F10 = false;
            }
        }
        //PRIMEPOS-TOKENSALE
        private void Fetchis_F10_PrimeRxPay(frmPOSOnlineCC oProcessCC)
        {

            if (oPosPTList.is_F10)
            {
                oProcessCC.isF10(oPosPTList.is_F10, oPosPTList.CustomerCardInfo.cardNumber, oPosPTList.CustomerCardInfo.cardExpiryDate);
                oPosPTList.is_F10 = false;
            }
        }

        private void EBTSalesFSAAmount(frmPOSProcessCC oProcessCC)
        {
            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                if (oProcessCC.FSAAmount > oProcessCC.Amount)
                {
                    oProcessCC.FSAAmount = oProcessCC.Amount;
                    if (oProcessCC.FSARxAmount > oProcessCC.FSAAmount)
                    {
                        oProcessCC.FSARxAmount = oProcessCC.FSAAmount;
                    }
                }
            }
        }

        private void CancelIsValidSignPad(KeyEventArgs e)
        {
            if (Configuration.CPOSSet.UseSigPad == true)
            {
                SigPadUtil.DefaultInstance.ShowCustomScreen(" Total Amount : " + Configuration.CInfo.CurrencySymbol.ToString() + this.txtAmtTotal.Text + "\n Processing Payment...");//NileshJ - After Aborted
                //SigPadUtil.DefaultInstance.ShowCustomScreen("Processing Payment...");//Added by SRT for better flow.
            }

            //Naim 22Oct2007 Commented below mentioned line because if user cancels cc processing then remaining payment
            //amt should remina in cell so user should press enter again to process it.
            e.Handled = true;
            oPosPTList.LastKey = "Esc";
            logger.Trace("grdPayment_KeyDown() - Process CC paymnet canceled");
            return;
        }

        private void CheckCCProceesorSwitched(frmPOSProcessCC oProcessCC)
        {
            //Added By SRT(Ritesh Parekh) Date : 23-Jul-2009
            //Check if processor was switched.
            if (oProcessCC.ApprovedPCCCardInfo != null && oProcessCC.WasProcessorChanged && !oProcessCC.isFSATransaction && Configuration.CPOSSet.PaymentProcessor != "XLINK")
            {
                if (POS_Core_UI.Resources.Message.Display("The card processed was a NonFSA card.\nAre you sure you want to continue FSA transaction with NonFSA Card?", "PrimePOS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    oProcessCC.isFSATransaction = true;
                }
                else
                {
                    //Call void transaction here.
                    oProcessCC.ApprovedPCCCardInfo.cardType = oProcessCC.oPayTpes;
                    if (!oProcessCC.PerformVoidTransaction(oProcessCC.ApprovedPCCCardInfo, oPosPTList.oTransactionType, oProcessCC.NBSTransId))
                    {
                        clsUIHelper.ShowErrorMsg("Void was not possible for current FSA transaction through NonFSA card.\nPlease void this transaction manually.");
                        oProcessCC.isFSATransaction = true;
                    }
                    else
                    {
                        return;//Transaction successfully voided.
                    }
                }
            }
            //End Of Added By SRT(Ritesh Parekh)
            //Added by Dharmendra Till Here Mar-13-03
        }

        private void CheckOfflineTransaction(frmPOSProcessCC oProcessCC)
        {
            //Added by SRT (Abhishek D) Date : 17 March 2010
            if (oProcessCC.AuthNo == "OFFLIN")
            {
                clsUIHelper.ShowErrorMsg("This is offline transaction, Please contact to MMS for support.");
            }
            //End of  //Added by SRT (Abhishek D)Date : 17 March 2010
        }

        private void CheckWOLRLPAYDebitCCAndCreditCC(frmPOSProcessCC oProcessCC)
        {
            if (oProcessCC != null && oProcessCC.PaymentProcessor.ToUpper() == "WORLDPAY")
            {
                if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.DebitCard && oProcessCC.SubCardType.ToUpper().Contains("DISCOVER"))
                {
                    oPosPTList.oPayTpes = POS_Core.Resources.PayTypes.CreditCard;
                }
                else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.CreditCard && oProcessCC.SubCardType.ToUpper().Contains("DEBIT"))
                {
                    oPosPTList.oPayTpes = POS_Core.Resources.PayTypes.DebitCard;
                }
            }
        }

        private void ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId)
        {
            if(Configuration.CPOSSet.PaymentProcessor == "VANTIV") //PRIMEPOS-3519 //PRIEMPOS-3504
            {
                if(string.IsNullOrEmpty(oProcessCC.PaymentType))
                {
                    if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.CreditCard)
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            if (Configuration.convertNullToString(oProcessCC.SubCardType).Length > 0)
                            {
                                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" && oProcessCC.SubCardType == "Debit Card")
                                {
                                    if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                                    {
                                        payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                        break;
                                    }
                                }
                                else if ((string)oRow.Cells["Unb"].Value == oProcessCC.SubCardType.Substring(0, 1))
                                {
                                    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                    break;
                                }
                                else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))
                                {
                                    payTypeId = "8";
                                    break;
                                }
                            }
                            else
                            {
                                payTypeId = strDefaultCreditCardPayTypeId;
                                logger.Trace("ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId) - Credit Card SubCardType is blank or null");
                                break;
                            }
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(oProcessCC.PaymentType) && oProcessCC.PaymentType.ToUpper() == "DEBIT" || oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.DebitCard)
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            if (IsNBSTransaction)
                            {
                                if ((string)oRow.Cells["Unb"].Value == oProcessCC.SubCardType.Substring(0, 1))
                                {
                                    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                    break;
                                }
                            }
                            else
                            {
                                if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                                {
                                    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                    break;
                                }
                                else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))
                                {
                                    payTypeId = "8";
                                    break;
                                }
                            }
                        }
                    }
                    else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT)
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            if (oProcessCC.SubCardType != null)
                            {
                                if ((string)oRow.Cells["Unb"].Value == oProcessCC.SubCardType.Substring(0, 1))
                                {
                                    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.NBS) //PRIMEPOS-3372
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            if (Configuration.convertNullToString(oProcessCC.SubCardType).Length > 0)
                            {
                                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" && oProcessCC.SubCardType == "Debit Card")
                                {
                                    if (Convert.ToString(oRow.Cells["Unb"].Value).Trim().ToUpper() == "T")
                                    {
                                        payTypeId = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value);
                                        break;
                                    }
                                }
                                else if (Convert.ToString(oRow.Cells["Unb"].Value) == oProcessCC.SubCardType.Substring(0, 1))
                                {
                                    payTypeId = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value);
                                    break;
                                }
                                else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))
                                {
                                    payTypeId = "8";
                                    break;
                                }
                            }
                            else
                            {
                                payTypeId = strDefaultCreditCardPayTypeId;
                                logger.Trace("ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId) - Credit Card SubCardType is blank or null");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.CreditCard)
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            //if (oProcessCC.SubCardType != null)
                            if (Configuration.convertNullToString(oProcessCC.SubCardType).Length > 0)
                            {
                                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" && oProcessCC.SubCardType == "Debit Card")
                                {
                                    if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                                    {
                                        payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                        break;
                                    }
                                }
                                else if ((string)oRow.Cells["Unb"].Value == oProcessCC.SubCardType.Substring(0, 1))
                                {
                                    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                    break;
                                }
                                else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))//2664 //PRIMEPOS-3087
                                {
                                    payTypeId = "8";
                                    break;
                                }
                            }
                            else
                            {
                                //payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString(); //PRIMEPOS-2724 22-Aug-2019 JY Commented
                                payTypeId = strDefaultCreditCardPayTypeId;  //PRIMEPOS-2724 22-Aug-2019 JY Added
                                logger.Trace("ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId) - Credit Card SubCardType is blank or null");    //PRIMEPOS-2724 22-Aug-2019 JY Added
                                break;
                            }
                        }
                    }
                    else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.DebitCard)
                    {
                        //Modified By Dharmendra on April-23-09
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                            {
                                payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                break;
                            }
                            //else if ("AT" == oProcessCC.SubCardType.Substring(0, 2))//2664
                            else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))//2664 //PRIMEPOS-3087
                            {
                                payTypeId = "8";
                                break;
                            }
                        }
                        //Modified till here April-23-09
                    }
                    else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT) //Added by Manoj 8/24/2011.  Save EBT (E) transaction in DB.
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {//Added by Manoj 8/24/2011.  Save EBT transaction in DB.
                            if (oProcessCC.SubCardType != null)
                            {
                                if ((string)oRow.Cells["Unb"].Value == oProcessCC.SubCardType.Substring(0, 1))
                                {
                                    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.NBS) //PRIMEPOS-3372
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            //if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                            //{
                            //    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                            //    break;
                            //}
                            ////else if ("AT" == oProcessCC.SubCardType.Substring(0, 2))//2664
                            //else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))//2664 //PRIMEPOS-3087
                            //{
                            //    payTypeId = "8";
                            //    break;
                            //}
                            if (Configuration.convertNullToString(oProcessCC.SubCardType).Length > 0)
                            {
                                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" && oProcessCC.SubCardType == "Debit Card")
                                {
                                    if (Convert.ToString(oRow.Cells["Unb"].Value).Trim().ToUpper() == "T")
                                    {
                                        payTypeId = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value);
                                        break;
                                    }
                                }
                                else if (Convert.ToString(oRow.Cells["Unb"].Value) == oProcessCC.SubCardType.Substring(0, 1))
                                {
                                    payTypeId = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value);
                                    break;
                                }
                                else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))
                                {
                                    payTypeId = "8";
                                    break;
                                }
                            }
                            else
                            {
                                payTypeId = strDefaultCreditCardPayTypeId;
                                logger.Trace("ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId) - Credit Card SubCardType is blank or null");
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.CreditCard)
                {
                    foreach (UltraGridRow oRow in this.grdPayment.Rows)
                    {
                        //if (oProcessCC.SubCardType != null)
                        if (Configuration.convertNullToString(oProcessCC.SubCardType).Length > 0)
                        {
                            if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" && oProcessCC.SubCardType == "Debit Card")
                            {
                                if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                                {
                                    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                    break;
                                }
                            }
                            else if ((string)oRow.Cells["Unb"].Value == oProcessCC.SubCardType.Substring(0, 1))
                            {
                                payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                break;
                            }
                            else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))//2664 //PRIMEPOS-3087
                            {
                                payTypeId = "8";
                                break;
                            }
                        }
                        else
                        {
                            //payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString(); //PRIMEPOS-2724 22-Aug-2019 JY Commented
                            payTypeId = strDefaultCreditCardPayTypeId;  //PRIMEPOS-2724 22-Aug-2019 JY Added
                            logger.Trace("ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId) - Credit Card SubCardType is blank or null");    //PRIMEPOS-2724 22-Aug-2019 JY Added
                            break;
                        }
                    }
                }
                else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.DebitCard)
                {
                    //Modified By Dharmendra on April-23-09
                    foreach (UltraGridRow oRow in this.grdPayment.Rows)
                    {
                        if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                        {
                            payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                            break;
                        }
                        //else if ("AT" == oProcessCC.SubCardType.Substring(0, 2))//2664
                        else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))//2664 //PRIMEPOS-3087
                        {
                            payTypeId = "8";
                            break;
                        }
                    }
                    //Modified till here April-23-09
                }
                else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT) //Added by Manoj 8/24/2011.  Save EBT (E) transaction in DB.
                {
                    foreach (UltraGridRow oRow in this.grdPayment.Rows)
                    {//Added by Manoj 8/24/2011.  Save EBT transaction in DB.
                        if (oProcessCC.SubCardType != null)
                        {
                            if ((string)oRow.Cells["Unb"].Value == oProcessCC.SubCardType.Substring(0, 1))
                            {
                                payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                                break;
                            }
                        }
                    }
                }
                else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.NBS) //PRIMEPOS-3372
                {
                    foreach (UltraGridRow oRow in this.grdPayment.Rows)
                    {
                        //if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                        //{
                        //    payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                        //    break;
                        //}
                        ////else if ("AT" == oProcessCC.SubCardType.Substring(0, 2))//2664
                        //else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))//2664 //PRIMEPOS-3087
                        //{
                        //    payTypeId = "8";
                        //    break;
                        //}
                        if (Configuration.convertNullToString(oProcessCC.SubCardType).Length > 0)
                        {
                            if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" && oProcessCC.SubCardType == "Debit Card")
                            {
                                if (Convert.ToString(oRow.Cells["Unb"].Value).Trim().ToUpper() == "T")
                                {
                                    payTypeId = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value);
                                    break;
                                }
                            }
                            else if (Convert.ToString(oRow.Cells["Unb"].Value) == oProcessCC.SubCardType.Substring(0, 1))
                            {
                                payTypeId = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value);
                                break;
                            }
                            else if (oProcessCC.SubCardType.Length > 2 && "AT" == oProcessCC.SubCardType.Substring(0, 2))
                            {
                                payTypeId = "8";
                                break;
                            }
                        }
                        else
                        {
                            payTypeId = strDefaultCreditCardPayTypeId;
                            logger.Trace("ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId) - Credit Card SubCardType is blank or null");
                            break;
                        }
                    }
                }
            }
            //Added Till Here Mar-13-03
        }

        #region PRIMEPOS-2841
        private void ProcessPayTypeId(frmPOSOnlineCC oProcessOnlineCC, ref string payTypeId)
        {
            if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.CreditCard)
            {
                foreach (UltraGridRow oRow in this.grdPayment.Rows)
                {
                    //if (oProcessCC.SubCardType != null)
                    if (Configuration.convertNullToString(oProcessOnlineCC.SubCardType).Length > 0)
                    {
                        if ((string)oRow.Cells["Unb"].Value == oProcessOnlineCC.SubCardType.Substring(0, 1))
                        {
                            payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                            break;
                        }
                    }
                    else
                    {
                        //payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString(); //PRIMEPOS-2724 22-Aug-2019 JY Commented
                        payTypeId = strDefaultCreditCardPayTypeId;  //PRIMEPOS-2724 22-Aug-2019 JY Added
                        logger.Trace("ProcessPayTypeId(frmPOSProcessCC oProcessCC, ref string payTypeId) - Credit Card SubCardType is blank or null");    //PRIMEPOS-2724 22-Aug-2019 JY Added
                        break;
                    }
                }
            }
            else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.DebitCard)
            {
                //Modified By Dharmendra on April-23-09
                foreach (UltraGridRow oRow in this.grdPayment.Rows)
                {
                    if ((string)oRow.Cells["Unb"].Value.ToString().Trim().ToUpper() == "T")
                    {
                        payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                        break;
                    }
                }
                //Modified till here April-23-09
            }
            else if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT) //Added by Manoj 8/24/2011.  Save EBT (E) transaction in DB.
            {
                foreach (UltraGridRow oRow in this.grdPayment.Rows)
                {//Added by Manoj 8/24/2011.  Save EBT transaction in DB.
                    if (oProcessOnlineCC.SubCardType != null)
                    {
                        if ((string)oRow.Cells["Unb"].Value == oProcessOnlineCC.SubCardType.Substring(0, 1))
                        {
                            payTypeId = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString();
                            break;
                        }
                    }
                }
            }

            //Added Till Here Mar-13-03
        }
        #endregion
        private void ProcessWORLDPAYFirstMile(frmPOSProcessCC oProcessCC)
        {
            if (oProcessCC != null && oProcessCC.PaymentProcessor.ToUpper() == "WORLDPAY")
            {
                FirstMile.TransactionResult wpResult = new FirstMile.TransactionResult();
                wpResult = PccPaymentSvr.GetProcessorInstance(oProcessCC.PaymentProcessor).WP_TransResult;
                PccPaymentSvr.DefaultInstance.WP_TransResult = wpResult;
                oProcessCC.SigType = "M";
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Value = wpResult.ResponseTags.AuthCode;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = "************" + wpResult.Account + "|" + wpResult.Expiration;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].Value = oProcessCC.SigType;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Value = wpResult.AccountType;
                if (!string.IsNullOrEmpty(wpResult.MerchantID))
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_MerchantID].Value = wpResult.MerchantID;
                }
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_EntryMethod].Value = wpResult.ResponseTags.EntryMethod;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CardType].Value = wpResult.AccountType;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_ProcTransType].Value = wpResult.ResponseTags.TransactionType;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Verbiage].Value = wpResult.ResponseTags.TransactionResult;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RTransactionID].Value = wpResult.ResponseTags.TranstactionID;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsManual].Value = oProcessCC.isManualProcess;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = oProcessCC.isFSATransaction;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID].Value = wpResult.TransactionID + ":" + wpResult.OrderID;
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1) * Configuration.convertNullToDecimal(wpResult.Amount);
                }
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = Configuration.convertNullToDecimal(wpResult.Amount);
                }
            }
            //PRIMEPOS-2664 ADDED BY ARVIND
            else if (oProcessCC != null && oProcessCC.PaymentProcessor.ToUpper() == "EVERTEC")
            {
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Value = oProcessCC.AuthNo;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = "************" + oProcessCC.txtCCNo.Text.Substring(oProcessCC.txtCCNo.Text.Length - 4);//oProcessCC.sCardNumber.Substring(oProcessCC.sCardNumber.Length - 4);   //
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value += " | " + oProcessCC.txtExpDate.Text;
                //Added by RiteshMx
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].Value = oProcessCC.SigType;
                //Added By shitaljit on 7/3/2013 to save Card Holdrs's name and print same in CC Receipt.
                // this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Value = string.IsNullOrEmpty(oProcessCC.ApprovedPCCCardInfo.cardHolderName) ? "************" + oProcessCC.txtCCNo.Text.Substring(oProcessCC.txtCCNo.Text.Length - 4) : oProcessCC.ApprovedPCCCardInfo.cardHolderName;
                //END
                //Added By Nilesh For Manual Entry
                if (string.IsNullOrEmpty(oProcessCC.ApprovedPCCCardInfo.cardHolderName))
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                }
                else
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Value = oProcessCC.ApprovedPCCCardInfo.cardHolderName;
                }
                //End
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsManual].Value = oProcessCC.isManualProcess; //Sprint-19 - 2139 06-Jan-2015 JY Added
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = oProcessCC.isFSATransaction;
                if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1) * oProcessCC.Amount;
                }
                if (oPosPTList.oTransactionType == POSTransactionType.Sales)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oProcessCC.Amount;
                }
            }
            else
            {
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Value = oProcessCC.AuthNo;
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = "************" + oProcessCC.txtCCNo.Text.Substring(oProcessCC.txtCCNo.Text.Length - 4);//oProcessCC.sCardNumber.Substring(oProcessCC.sCardNumber.Length - 4);   //
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value += " | " + oProcessCC.txtExpDate.Text;
                //Added by RiteshMx
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].Value = oProcessCC.SigType;
                ////Added By shitaljit on 7/3/2013 to save Card Holdrs's name and print same in CC Receipt.
                //this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Value = string.IsNullOrEmpty(oProcessCC.ApprovedPCCCardInfo.cardHolderName) ? "************" + oProcessCC.txtCCNo.Text.Substring(oProcessCC.txtCCNo.Text.Length - 4) : oProcessCC.ApprovedPCCCardInfo.cardHolderName;
                ////END
                //Added By Nilesh For Manual Entry
                if (string.IsNullOrEmpty(oProcessCC.ApprovedPCCCardInfo.cardHolderName))
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                }
                else
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Value = oProcessCC.ApprovedPCCCardInfo.cardHolderName;
                }
                //End
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsManual].Value = oProcessCC.isManualProcess; //Sprint-19 - 2139 06-Jan-2015 JY Added
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = oProcessCC.isFSATransaction;
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1) * oProcessCC.Amount;
                }
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oProcessCC.Amount;
                }

            }
        }
        #region PRIMEPOS-2841
        //primepos-2841
        private void ProcessPrimeRxPay(frmPOSOnlineCC oProcessCC)
        {
            if (oProcessCC != null)
            {
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Value = oProcessCC.AuthNo;
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = "************" + oProcessCC.txtCCNo.Text.Substring(oProcessCC.txtCCNo.Text.Length - 4);//oProcessCC.sCardNumber.Substring(oProcessCC.sCardNumber.Length - 4);   //
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value += " | " + oProcessCC.txtExpDate.Text;
                }
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].Value = oProcessCC.SigType;
                if (string.IsNullOrEmpty(oProcessCC.ApprovedPCCCardInfo.cardHolderName))
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                }
                else
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Value = oProcessCC.ApprovedPCCCardInfo.cardHolderName;
                }
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsManual].Value = oProcessCC.isManualProcess; //Sprint-19 - 2139 06-Jan-2015 JY Added
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = oProcessCC.isFSATransaction;
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (-1) * oProcessCC.Amount;
                }
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oProcessCC.Amount;
                }

            }
        }
        #endregion
        private void ProcessSign(frmPOSProcessCC oProcessCC)
        {
            if (oProcessCC.SigType == clsPOSDBConstants.BINARYIMAGE && !SigPadUtil.DefaultInstance.isEvertec && !SigPadUtil.DefaultInstance.isVantiv && !SigPadUtil.DefaultInstance.isPAX && !SigPadUtil.DefaultInstance.isElavon)//PRIMEPOS-2664 Added by Arvind //PRIMEPOS-2636 ADDED ISVANTIV //PRIMEPOS-2952//2943 Arvind
            {
                try
                {
                    //System.Drawing.Graphics g =null;
                    //Bitmap bmp = new Bitmap(335, 245,  PixelFormat.Format16bppArgb1555);
                    Bitmap bmp = new Bitmap(335, 245, PixelFormat.Format32bppPArgb);//   System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    string errorMsg = string.Empty;
                    SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                    if (oProcessCC.BinarySignature == null)
                    {
                        if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.isISC || POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.isWP)
                        {
                            byte[] signByte = Convert.FromBase64String(SigPadUtil.DefaultInstance.CustomerSignature);
                            sigDisp.DrawSignatureMX(signByte, ref bmp, out errorMsg);
                        }
                        else if (POS_Core.Resources.PaymentHandler.SigPadUtil.DefaultInstance.isPAX)
                        {
                            //PRIMEPOS-2528 (Suraj) 1-june-18 Added PAX Signature Drawing method in SigDisplay 
                            sigDisp.DrawSignaturePAX(SigPadUtil.DefaultInstance.CustomerSignature, ref bmp, out errorMsg);
                        }
                        else
                        {
                            sigDisp.DrawSignature(oProcessCC.CustomerSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                        }
                    }
                    else //for touch screen PrimePOS#2549
                    {
                        sigDisp.DrawSignatureMX(oProcessCC.BinarySignature, ref bmp, out errorMsg);
                    }
                    //bmp.Save("c:\\temp.bmp");//,System.Drawing.Imaging.ImageFormat.Bmp);

                    Bitmap converted = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
                    using (Graphics g = Graphics.FromImage(converted))
                    {
                        // Prevent DPI conversion
                        g.PageUnit = GraphicsUnit.Pixel;
                        g.Clear(Color.White);
                        // Draw the image
                        g.DrawImageUnscaled(bmp, 0, 0);
                    }

                    ImageConverter converter = new ImageConverter();
                    byte[] btarr = (byte[])converter.ConvertTo(converted, typeof(byte[]));
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value = btarr;// ms.ToArray();
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "grdPayment_KeyDown()");
                }
            }
            else if (oProcessCC.PaymentProcessor == clsPOSDBConstants.EVERTEC)//PRIMEPOS-2664
            {
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value = Encoding.Default.GetBytes(oProcessCC.CustomerSignature);
            }
            //PRIMEPOS-2636
            else if (SigPadUtil.DefaultInstance.isVantiv || SigPadUtil.DefaultInstance.isPAX || SigPadUtil.DefaultInstance.isElavon)   //PRIMEPOS-2952//2932 Arvind
            {
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value = Encoding.Default.GetBytes(oProcessCC.CustomerSignature);
            }
            //
            else
                if (oProcessCC.SigType == clsPOSDBConstants.STRINGIMAGE) //Added by Prashant 20-sep-2010
            {
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].Value = oProcessCC.CustomerSignature;
            }

        }

        private void ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId)
        {
            logger.Trace("ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId) - " + clsPOSDBConstants.Log_Entering); //PRIMEPOS-2764 22-Nov-2019 JY Added

            POSTransPayment_CCLog oPOSTransPayment_CLog = new POSTransPayment_CCLog(); //20Nov2008 by Naim : Add to log
            oPOSTransPayment_CLog.Amount = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);
            oPOSTransPayment_CLog.AuthNo = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Value);
            oPOSTransPayment_CLog.CustomerSign = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].Value);
            try
            {
                if ((this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value) != System.DBNull.Value)
                    oPOSTransPayment_CLog.BinarySign = (byte[])this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value;

                oPOSTransPayment_CLog.SigType = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].Value);
            }
            catch (Exception EX)
            {
                logger.Fatal(EX, "ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId) - it just a trace - no showstopper for the application");
            }
            oPOSTransPayment_CLog.RefNo = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value);
            oPOSTransPayment_CLog.TransType = (int)oPosPTList.oTransactionType;

            if (Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim() == "-99")
            {
                if(IsNBSTransaction) //PRIMEPOS-3519 //PRIEMPOS-3504
                {
                    oPOSTransPayment_CLog.PayTypeCode = "N";
                }
                else
                {
                    oPOSTransPayment_CLog.PayTypeCode = payTypeId.Trim();
                }
            }
            else
            {
                oPOSTransPayment_CLog.PayTypeCode = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim();
            }

            //Commented By shitaljit on 5 March 2012 after discussion with Manoj
            //It was giving problem if Configuration.CInfo.ShowOnlyOneCCType == false
            ////added by Manoj on 2/28/2012.  PayTypeId something get pass as 1(cash) when its a CC. Now it will make sure that the
            oPOSTransPayment_CLog.Status = POSTransPayment_CCLog_Status.Approved;
            oPOSTransPayment_CLog.IsIIASTrans = oProcessCC.isFSATransaction;
            if (oProcessCC.PaymentProcessor.ToUpper() != "WORLDPAY")
            {
                if (oProcessCC.ApprovedPCCCardInfo != null)
                {
                    oPOSTransPayment_CLog.TransID = oProcessCC.ApprovedPCCCardInfo.tRoutId;
                    oPOSTransPayment_CLog.PccCardInfo = oProcessCC.ApprovedPCCCardInfo.Copy();
                    oPOSTransPayment_CLog.PccCardInfo.cardType = oPosPTList.oPayTpes;
                    //Added By SRT(Ritesh Parekh) Date : 23-Jul-2009
                    //if (Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim() == "N") //PRIMEPOS-3372-TEMP
                    //{
                    //    oPOSTransPayment_CLog.PccCardInfo.PaymentProcessor = "NB_VANTIV"; //PRIMEPOS-3482
                    //}
                    //else
                    //{
                    //    oPOSTransPayment_CLog.PccCardInfo.PaymentProcessor = oProcessCC.PaymentProcessor;// Store processor name XCharge or PCCharge which was added to support multiple payment processor
                    //}
                    if(IsNBSTransaction) //PRIMEPOS-3519 //PRIMEPOS-3504
                    {
                        oPOSTransPayment_CLog.PccCardInfo.PaymentProcessor = "NB_VANTIV"; //PRIMEPOS-3482
                    }
                    else
                    {
                        oPOSTransPayment_CLog.PccCardInfo.PaymentProcessor = oProcessCC.PaymentProcessor;
                    }
                    //End Of Added By SRT(Ritesh Parekh)
                }
            }
            else
            {
                oPOSTransPayment_CLog.TransID = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID].Value);
                oPOSTransPayment_CLog.PccCardInfo = oProcessCC.ApprovedPCCCardInfo.Copy();
                // this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransID].Value = oPOSTransPayment_CLog.TransID;
            }
            //Added By SRT(Ritesh Parekh) Date : 23-Jul-2009
            //if (Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim() == "N") //PRIMEPOS-3372-TEMP
            //{
            //    oPOSTransPayment_CLog.PaymentProcessor = "NB_VANTIV"; //PRIMEPOS-3482
            //}
            //else
            //{
            //    oPOSTransPayment_CLog.PaymentProcessor = oProcessCC.PaymentProcessor; // Store processor name XCharge or PCCharge which was added to support multiple payment processor
            //}
            if (IsNBSTransaction) //PRIMEPOS-3519 //PRIMEPOS-3505
            {
                oPOSTransPayment_CLog.PaymentProcessor = "NB_VANTIV"; //PRIMEPOS-3482
            }
            else
            {
                oPOSTransPayment_CLog.PaymentProcessor = oProcessCC.PaymentProcessor; // Store processor name XCharge or PCCharge which was added to support multiple payment processor
            }
            //End Of Added By SRT(Ritesh Parekh)
            oPOSTransPayment_CLog.IsManual = oProcessCC.isManualProcess; //Sprint-19 - 2139 06-Jan-2015 JY Added 
            oPOSTransPayment_CLog.Add();
            oPosPTList.oPOSTransPayment_CCLogList.Add(oPOSTransPayment_CLog);

            AddRowToPaid(this.grdPayment.ActiveRow, oProcessCC.SubCardType, payTypeId);
            //Following if is added by shitaljit to move payment option to cash in case EBT payment is successfull.
            if (oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT)
            {
                MoveToRow("S");
                this.grdPayment.Update();
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Process CC paymnet", clsPOSDBConstants.Log_Exiting);
            logger.Trace("ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId) - " + clsPOSDBConstants.Log_Exiting); //PRIMEPOS-2764 22-Nov-2019 JY Modified
        }

        #region PRIMEPOS-2915
        private void ProcessTransactionPaymentClogCustomerDriven(POSTransPaymentData oPaymentCC, ref string payTypeId)
        {
            if (oPaymentCC.Tables.Count > 0)
            {
                for (int i = 0; i < oPaymentCC.Tables[0].Rows.Count; i++)
                {
                    logger.Trace("ProcessTransactionPaymentClog(frmPOSProcessCC oPaymentCC, ref string payTypeId) - " + clsPOSDBConstants.Log_Entering); //PRIMEPOS-2764 22-Nov-2019 JY Added

                    //this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value = onlineProcessCC.isFSATransaction;                    

                    POSTransPayment_CCLog oPOSTransPayment_CLog = new POSTransPayment_CCLog(); //20Nov2008 by Naim : Add to log
                    oPOSTransPayment_CLog.Amount = Configuration.convertNullToDecimal(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_Amount]);
                    oPOSTransPayment_CLog.AuthNo = Configuration.convertNullToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_AuthNo]);
                    oPOSTransPayment_CLog.CustomerSign = Configuration.convertNullToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_CustomerSign]);
                    try
                    {
                        if ((oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_BinarySign]) != System.DBNull.Value)
                            oPOSTransPayment_CLog.BinarySign = (byte[])oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_BinarySign];

                        oPOSTransPayment_CLog.SigType = Configuration.convertNullToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_SigType]);
                    }
                    catch (Exception EX)
                    {
                        logger.Fatal(EX, "ProcessTransactionPaymentClog(frmPOSProcessCC oPaymentCC, ref string payTypeId) - it just a trace - no showstopper for the application");
                    }
                    oPOSTransPayment_CLog.RefNo = Configuration.convertNullToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_RefNo]);
                    oPOSTransPayment_CLog.TransType = (int)oPosPTList.oTransactionType;

                    if (Configuration.convertNullToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode]).Trim() == "-99")
                    {
                        oPOSTransPayment_CLog.PayTypeCode = payTypeId.Trim();
                    }
                    else
                    {
                        oPOSTransPayment_CLog.PayTypeCode = Configuration.convertNullToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode]).Trim();
                    }
                    oPOSTransPayment_CLog.PccCardInfo = new PccCardInfo();
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID])))
                    {
                        oPOSTransPayment_CLog.PccCardInfo.tRoutId = Convert.ToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID]);
                        oPOSTransPayment_CLog.PccCardInfo.IsVoidCustomerDriven = false;
                    }
                    else
                    {
                        oPOSTransPayment_CLog.PccCardInfo.tRoutId = Convert.ToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID]);
                        oPOSTransPayment_CLog.PccCardInfo.IsVoidCustomerDriven = true;
                    }
                    oPOSTransPayment_CLog.PccCardInfo.PaymentProcessor = Convert.ToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]);
                    oPOSTransPayment_CLog.PccCardInfo.transAmount = Convert.ToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_Amount]);
                    oPOSTransPayment_CLog.PccCardInfo.TicketNo = Convert.ToString(oPaymentCC.Tables[0].Rows[i][clsPOSDBConstants.POSTransPayment_Fld_TicketNumber]);
                    oPOSTransPayment_CLog.Add();
                    oPosPTList.oPOSTransPayment_CCLogList.Add(oPOSTransPayment_CLog);
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Process CC paymnet", clsPOSDBConstants.Log_Exiting);
            logger.Trace("ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId) - " + clsPOSDBConstants.Log_Exiting); //PRIMEPOS-2764 22-Nov-2019 JY Modified
        }
        #endregion

        #region PRIMEPOS-2841
        private void ProcessTransactionPaymentClog(frmPOSOnlineCC oProcessCC, ref string payTypeId)
        {
            logger.Trace("ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId) - " + clsPOSDBConstants.Log_Entering); //PRIMEPOS-2764 22-Nov-2019 JY Added

            POSTransPayment_CCLog oPOSTransPayment_CLog = new POSTransPayment_CCLog(); //20Nov2008 by Naim : Add to log
            oPOSTransPayment_CLog.Amount = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);
            oPOSTransPayment_CLog.AuthNo = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Value);
            oPOSTransPayment_CLog.CustomerSign = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].Value);
            try
            {
                if ((this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value) != System.DBNull.Value)
                    oPOSTransPayment_CLog.BinarySign = (byte[])this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value;

                oPOSTransPayment_CLog.SigType = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].Value);
            }
            catch (Exception EX)
            {
                logger.Fatal(EX, "ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId) - it just a trace - no showstopper for the application");
            }
            oPOSTransPayment_CLog.RefNo = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value);
            oPOSTransPayment_CLog.TransType = (int)oPosPTList.oTransactionType;

            if (Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim() == "-99")
            {
                oPOSTransPayment_CLog.PayTypeCode = payTypeId.Trim();
            }
            else
            {
                oPOSTransPayment_CLog.PayTypeCode = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text).Trim();
            }

            //Commented By shitaljit on 5 March 2012 after discussion with Manoj
            //It was giving problem if Configuration.CInfo.ShowOnlyOneCCType == false
            ////added by Manoj on 2/28/2012.  PayTypeId something get pass as 1(cash) when its a CC. Now it will make sure that the
            oPOSTransPayment_CLog.Status = POSTransPayment_CCLog_Status.Approved;
            oPOSTransPayment_CLog.IsIIASTrans = oProcessCC.isFSATransaction;
            if (oProcessCC.PaymentProcessor.ToUpper() != "WORLDPAY")
            {
                if (oProcessCC.ApprovedPCCCardInfo != null)
                {
                    oPOSTransPayment_CLog.TransID = oProcessCC.ApprovedPCCCardInfo.tRoutId;
                    oPOSTransPayment_CLog.PccCardInfo = oProcessCC.ApprovedPCCCardInfo.Copy();
                    oPOSTransPayment_CLog.PccCardInfo.cardType = oPosPTList.oPayTpes;
                    //Added By SRT(Ritesh Parekh) Date : 23-Jul-2009
                    oPOSTransPayment_CLog.PccCardInfo.PaymentProcessor = oProcessCC.PaymentProcessor;// Store processor name XCharge or PCCharge which was added to support multiple payment processor
                                                                                                     //End Of Added By SRT(Ritesh Parekh)
                }
            }
            else
            {
                oPOSTransPayment_CLog.TransID = Configuration.convertNullToString(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID].Value);
                oPOSTransPayment_CLog.PccCardInfo = oProcessCC.ApprovedPCCCardInfo.Copy();
                // this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransID].Value = oPOSTransPayment_CLog.TransID;
            }
            //Added By SRT(Ritesh Parekh) Date : 23-Jul-2009
            oPOSTransPayment_CLog.PaymentProcessor = oProcessCC.PaymentProcessor; // Store processor name XCharge or PCCharge which was added to support multiple payment processor
                                                                                  //End Of Added By SRT(Ritesh Parekh)
            oPOSTransPayment_CLog.IsManual = oProcessCC.isManualProcess; //Sprint-19 - 2139 06-Jan-2015 JY Added 
            oPOSTransPayment_CLog.Add();
            oPosPTList.oPOSTransPayment_CCLogList.Add(oPOSTransPayment_CLog);

            AddRowToPaid(this.grdPayment.ActiveRow, oProcessCC.SubCardType, payTypeId);
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "Process CC paymnet", clsPOSDBConstants.Log_Exiting);
            logger.Trace("ProcessTransactionPaymentClog(frmPOSProcessCC oProcessCC, ref string payTypeId) - " + clsPOSDBConstants.Log_Exiting); //PRIMEPOS-2764 22-Nov-2019 JY Modified
        }
        #endregion

        #endregion grdPayment enter area

        // End by Farman Ansari 21/12/2017 

        #region HandleMoveToRow
        private void HandleMoveToRow(KeyEventArgs e)
        {

            if (e.KeyData == Keys.S)
            {
                MoveToRow("1");
            }
            else if (e.KeyData == Keys.K)
            {
                MoveToRow("2");
            }
            else if (e.KeyData == Keys.A)
            {
                MoveToRow("3");
            }
            else if (e.KeyData == Keys.V)
            {
                MoveToRow("4");
            }
            else if (e.KeyData == Keys.M)
            {
                MoveToRow("5");
            }
            else if (e.KeyData == Keys.D)
            {
                MoveToRow("6");
            }
            else if (e.KeyData == Keys.T)
            {
                MoveToRow("7");
            }
            else if (e.KeyData == Keys.U)
            {
                MoveToRow("C");
            }
            else if (e.KeyData == Keys.H)
            {
                MoveToRow("H");
            }
            else if (e.KeyData == Keys.E)
            {
                MoveToRow("E");
            }
            else if (e.KeyData == Keys.R)
            {
                MoveToRow("-99");
            }
            else
            {
                string[] sPayTypes = sNewAddedPayTypesID.Split('$');
                KeysConverter kc = new KeysConverter();
                foreach (string key in sPayTypes)
                {
                    if (kc.ConvertToString(e.KeyData) == key)
                    {
                        MoveToRow(key);
                        break;
                    }
                }
            }
        }

        #endregion MoveToRow

        // Added by Farman Ansari 21/12/2017 
        #region grdPayment KeyDown area
        private void grdPayment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                #region PRIMEPOS-2760 18-Nov-2019 JY Added logic to verify line items "Rx" exists in "PrimeRx"
                if (posTrans.oTransDRXData.TransDetailRX.Rows.Count > 0)
                {
                    string strMessage = string.Empty;
                    if (posTrans.RxExistsInPrimeRxDb(out strMessage) == true)
                    {
                        Resources.Message.Display(strMessage, "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                #endregion

                #region PRIMEPOS-3044 24-Feb-2021 JY Added
                try
                {
                    if (posTrans != null && posTrans.oTransDRXData != null && posTrans.oTransDRXData.Tables.Count > 0 && posTrans.oTransDRXData.TransDetailRX.Rows.Count > 0)
                    {
                        ArrayList alRxNos = new ArrayList();
                        bool bStatus = posTrans.CheckRxAlreadyPickedup(posTrans.CurrentTransactionType, posTrans.oTransDRXData.TransDetailRX, ref alRxNos);
                        if (bStatus)
                        {
                            string strRxNos = String.Join(",", alRxNos.ToArray());
                            if (posTrans.CurrentTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                                POS_Core_UI.Resources.Message.Display(strRxNos + " - Rx(s) are already picked up, please remove it to proceed with the transaction.", "PrimePOS", MessageBoxButtons.OK);
                            else
                                POS_Core_UI.Resources.Message.Display(strRxNos + " - Rx(s) are already returned, please remove it to proceed with the transaction.", "PrimePOS", MessageBoxButtons.OK);
                            btnClose_Click(sender, e);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "grdPayment_KeyDown(object sender, KeyEventArgs e)");
                    throw ex;
                }
                #endregion

                #region PRIMEPOS-2726 28-Aug-2019 JY Added
                if (bExecuteOnce == false)
                    bExecuteOnce = true;
                else
                    return;
                #endregion

                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "grdPayment_KeyDown()", clsPOSDBConstants.Log_Entering);
                logger.Trace("grdPayment_KeyDown() - " + clsPOSDBConstants.Log_Entering);
                bPaymentRowAdded = false;
                oPosPTList.LastKey = e.KeyCode.ToString();
                if (e.KeyCode == Keys.Enter)
                {
                    #region If enter key is pressed accept the payment
                    if (this.grdPayment.ContainsFocus == true && this.grdPayment.ActiveRow != null && this.grdPayment.ActiveCell != null)
                    {
                        if (grdPayment.ActiveCell.Activation == Activation.Disabled)
                        {
                            e.Handled = true;
                            return;
                        }
                        if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)
                        {
                            bool bStatus = this.ValidateAmount(e);
                            if (bStatus == false)
                                return;   //PRIMEPOS-2521 21-May-2018 JY we need to return from function if it returns false

                            bStatus = this.AmountColumnProcess(e);
                            if (bStatus == false)
                                return;   //PRIMEPOS-2521 21-May-2018 JY we need to return from function if it returns false
                        }
                        else if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo
                              && this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "2")
                        {
                            this.SearchCheckInfo(e);

                        }
                        else if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo
                          && this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "E")
                        {
                            this.SearchEBTInfo();
                        }
                        else if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo && this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "H")
                        {
                            if (Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString()) != 0)
                            {
                                if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].GetText(MaskMode.Raw).ToString().Trim() == "")
                                {
                                    this.SearchHouseChargeInfo();
                                    //throw (new Exception("Please select a house change account for transaction."));
                                }
                            }
                        }
                        else if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo
                           && this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "C")
                        {
                            if (oPosPTList.isROA == true)//PRIMEPOS-2434 06-May-2021 JY Added
                            {
                                //Resources.Message.Display("Regular coupon payment is not allowed for ROA.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            bool bStatus = this.SearchCouponInfo();
                            if (bStatus == false)
                                return;   //PRIMEPOS-2783 30-Jan-2020 JY we need to return from function if it returns false
                        }
                        #region PRIMEPOS-2938 29-Jan-2021 JY Added
                        else if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo && this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim().ToUpper() == "X")
                        {
                            try
                            {
                                if (Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) != 0)
                                {
                                    bool bStatus = this.ValidateAmount(e);
                                    if (bStatus == false)
                                        return;

                                    bStatus = this.AmountColumnProcess(e);
                                    if (bStatus == false)
                                        return;
                                }
                            }
                            catch (Exception Ex)
                            {
                                clsUIHelper.ShowErrorMsg(Ex.Message);
                            }
                        }
                        #endregion
                        else if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo)
                        {
                            this.SearchRefNumberInfo();
                        }
                    }
                    #endregion If enter key is pressed accept the payment
                }
                else if (e.KeyData == Keys.Escape)
                {
                }
                else if (e.KeyData == Keys.C)
                {
                }
                else if (this.grdPayment.ActiveCell != null && this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)
                {
                    HandleMoveToRow(e);
                }
                if (grdPayment.ActiveCell != null) //Sprint-18 20-Nov-2014 JY Added to resolve object reference error occur while clicking on inactive cell
                    ControlCursorNavigation(sender, e);
                this.CheckManualEBT(e);

                if (Configuration.CInfo.PromptForPartialPayment == true && bPaymentRowAdded == true && oPosPTList.amountBalanceDue != 0)    //PRIMEPOS-2499 29-Mar-2018 JY Added
                {
                    clsUIHelper.ShowErrorMsg("Remaining Balance: " + oPosPTList.amountBalanceDue + Environment.NewLine + "please select payment method");
                }

            }
            catch (Exception exp)
            {
                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "grdPayment_KeyDown()", clsPOSDBConstants.Log_Exception_Occured + exp.StackTrace.ToString());
                logger.Fatal(exp, "grdPayment_KeyDown()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            finally { bExecuteOnce = false; }   //PRIMEPOS-2726 28-Aug-2019 JY Added
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "grdPayment_KeyDown()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("grdPayment_KeyDown() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void SearchCheckInfo(KeyEventArgs e)
        {
            if (!bAllowCheckPayment)
                bAllowCheckPayment = oPosPTList.AllowCheckPaymentPriviliges();    //PRIMEPOS-2539 09-Jul-2018 JY Added
            if (bAllowCheckPayment == false)
                return;   //PRIMEPOS-2539 09-Jul-2018 JY Added

            this.grdPayment.ActiveRow.Update();
            //POSTransPaymentRow oRow = (POSTransPaymentRow)oPOSTransPaymentDataPaid.POSTransPayment.Rows.Add(this.oPOSTransPaymentData.POSTransPayment[this.grdPayment.ActiveRow.Index].ItemArray);
            if (Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) != 0)
            {
                //oRow.Amount = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                //oRow.RefNo = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].GetText(MaskMode.Raw).ToString();
                //this.oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRow);

                #region PRIMEPOS-2763 26-Nov-2019 JY Added
                decimal CurrentPaytypeAmount = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                decimal TotaldueAmount = oPosPTList.amountBalanceDue;
                string ActiveRowTranTypeCode = oPosPTList.GetPayType(this.grdPayment.ActiveRow.Cells["transtypecode"].Value.ToString().Trim().ToUpper());

                bool bstatus = ValidateOverPayment(ActiveRowTranTypeCode, CurrentPaytypeAmount, TotaldueAmount);
                if (bstatus == false)
                {
                    e.Handled = true;
                    return;
                }
                #endregion

                AddRowToPaid(this.grdPayment.ActiveRow);
            }
        }

        //PRIMEPOS-2763 25-Nov-2019 JY Added
        private bool ValidateOverPayment(string ActiveRowTranTypeCode, decimal CurrentPaytypeAmount, decimal TotaldueAmount)
        {
            bool bStatus = true;
            try
            {
                if (oPosPTList.WarnForOverPayment(ActiveRowTranTypeCode, CurrentPaytypeAmount, TotaldueAmount, oPosPTList.oTransactionType, oPosPTList.oPayTpes) == true)
                {
                    if (oPosPTList.isROA == true)
                    {
                        Resources.Message.Display("The amount you are paying $" + CurrentPaytypeAmount.ToString("######0.00") + " is greater than the total due amount $" + TotaldueAmount.ToString("######0.00") + ".\nYou need to pay the exact amount for ROA transaction", "Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)
                            this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].SelectAll();
                        else
                            this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].SelectAll();
                        return false;
                    }
                    else
                    {
                        if (bOverPay == false && Resources.Message.Display("The amount you are paying $" + CurrentPaytypeAmount.ToString("######0.00") + " is greater than the total due amount $" + TotaldueAmount.ToString("######0.00") + ".\nThis will result in cashback.\nAre you sure you want to proceed?", "Payment Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)
                                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].SelectAll();
                            else
                                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].SelectAll();
                            return false;
                        }
                        else
                        {
                            //this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activate();  //PRIMEPOS-2463 26-May-2020 JY Commented
                            #region PRIMEPOS-2463 26-May-2020 JY Added
                            if (ActiveRowTranTypeCode == POS_Core.Resources.PayTypes.CreditCard || ActiveRowTranTypeCode == POS_Core.Resources.PayTypes.DebitCard)
                            {
                                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales && Configuration.CInfo.AllowCashBack == false)
                                {
                                    Resources.Message.Display("User does not have enough privileges, please make sure to turn on \"Allow Cashback\" system level setings.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                                else
                                {
                                    string sUserID = Configuration.UserName;
                                    if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.AllowCashback.ID, UserPriviliges.Permissions.AllowCashback.Name, out sUserID) == false)
                                        return false;
                                }
                            }
                            else
                            {
                                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activate();
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                return false;
            }
            return bStatus;
        }

        private void SearchEBTInfo()
        {
            if (Configuration.CPOSSet.UsePinPad == true && (Configuration.CPOSSet.PaymentProcessor == "XLINK" || Configuration.CPOSSet.PaymentProcessor == "HPS") && Configuration.CPOSSet.PinPadModel == "Verifone MX870 with PinPad")
            {
                clsUIHelper.ShowErrorMsg("Manual EBT transaction is not permited.");
                MoveToRow("E");
            }
            else if (Configuration.CInfo.EBTAsManualTrans == true)
            {
                this.grdPayment.ActiveRow.Update();
                oPosPTList.oPayTpes = POS_Core.Resources.PayTypes.EBT;
                if (Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) != 0)
                {
                    AddRowToPaid(this.grdPayment.ActiveRow);
                }
            }
        }

        private Boolean SearchCouponInfo() //PRIMEPOS-2783 30-Jan-2020 JY changed return type from void to bool
        {
            bool bReturn = true;
            string sUserID = Configuration.UserName;
            if (oPosPTList.allowCouponPayment == false && UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.MakeCouponPayment.ID, UserPriviliges.Permissions.MakeCouponPayment.Name, out sUserID) == false)
            {
                return false;
            }
            else
            {
                oPosPTList.allowCouponPayment = true;
            }
            this.grdPayment.ActiveRow.Update();
            if (Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) != 0)
            {
                bReturn = this.ProcessCoupons();
            }
            return bReturn;
        }

        private void SearchRefNumberInfo()
        {
            this.grdPayment.ActiveRow.Update();
            if (Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) != 0)
            {
                AddRowToPaid(this.grdPayment.ActiveRow);
            }
        }

        private void CheckManualEBT(KeyEventArgs e)
        {

            //Added By Shitaljit(QuicSolv) om 23 Sept 2011
            if (this.grdPayment.ActiveCell != null && Configuration.CPOSSet.UsePinPad == true && this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "E" && this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_RefNo && e.KeyData != Keys.Enter)
            {
                if ((Configuration.CPOSSet.PaymentProcessor == "XLINK" || Configuration.CPOSSet.PaymentProcessor == "HPS") && Configuration.CPOSSet.PinPadModel == "Verifone MX870 with PinPad")
                {
                    clsUIHelper.ShowErrorMsg("Manual EBT transaction is not permited.");
                    MoveToRow("E");
                }
            }
            //End of Added By Shitaljit(QuicSolv) om 23 Sept 2011

        }

        #endregion grdPayment KeyDown area    

        // End by Farman Ansari 20/12/2017 
        private void grdPayment_ClickCellButton(object sender, CellEventArgs e)
        {
            string strRowPayTypeDesc = e.Cell.Row.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString().Trim();
            if (e.Cell.Column.Key == clsPOSDBConstants.PayType_Fld_PayTypeDescription && e.Cell.Row.Cells["TransTypeCode"].Text.ToString().Trim() != "C")
            {
                if (this.lblTransCompleteMSG.Text.Contains(strRowPayTypeDesc) == true && LastClickPaytype == strRowPayTypeDesc)
                {
                    if (e.Cell.Row.Cells["TransTypeCode"].Text.ToString().Trim() == "2" && string.IsNullOrEmpty(e.Cell.Row.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Text.ToString().Trim()) == false) //PRIMEPOS-2539 09-Jul-2018 JY Added
                    {
                        if (!bAllowCheckPayment)
                            bAllowCheckPayment = oPosPTList.AllowCheckPaymentPriviliges();
                        if (bAllowCheckPayment == false)
                            return;
                        this.grdPayment.ActiveCell = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo];
                    }
                    else if (e.Cell.Row.Cells["TransTypeCode"].Text.ToString().Trim() == "E" && string.IsNullOrEmpty(e.Cell.Row.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Text.ToString().Trim()) == false)
                    {
                        this.grdPayment.ActiveCell = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo];
                    }
                    else
                    {
                        this.grdPayment.ActiveCell = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                    }
                    grdPayment_KeyDown(this, new KeyEventArgs(Keys.Enter));
                }
                else
                {
                    string strRowCode = e.Cell.Row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim();
                    MoveToRow(strRowCode);
                    LastClickPaytype = strRowPayTypeDesc;
                }
            }
            else if (e.Cell.Row.Cells["TransTypeCode"].Text.ToString().Trim() == "C")
            {
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    SelectCLCoupons();
                }
                else if (this.lblTransCompleteMSG.Text.Contains(strRowPayTypeDesc) == true && LastClickPaytype == strRowPayTypeDesc)
                {
                    this.grdPayment.ActiveCell = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                    grdPayment_KeyDown(this, new KeyEventArgs(Keys.Enter));
                }
                else
                {
                    LastClickPaytype = strRowPayTypeDesc;
                }

            }
            else if (e.Cell.Row.Cells["TransTypeCode"].Text.ToString().Trim() == "H")
            {
                SearchHouseChargeInfo();
            }
        }

        private void ClearGridRow(UltraGridRow oGridRow)
        {
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].SetValue(0, false);
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].SetValue("", false);
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].SetValue("", false);
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].SetValue("", false);
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].SetValue("", false);
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo].SetValue("", false);
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].SetValue(0, false);
            oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsManual].SetValue("", false);   //Sprint-19 - 2139 06-Jan-2015 JY Added
        }

        private void grdPaymentComplete_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "B")
            {
                e.Row.Hidden = true;
            }
            if (grdPaymentComplete.Rows.Count > 0)
            {
                btnRemovePayment.Visible = true;
            }
        }

        private void populatePayType()
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "populatePayType()", clsPOSDBConstants.Log_Entering);
            logger.Trace("populatePayType() - " + clsPOSDBConstants.Log_Entering);
            oPosPTList.oPOSTransPaymentData = oPosPTList.PopulatePayTypeData();
            this.grdPayment.DataSource = oPosPTList.oPOSTransPaymentData;
            this.grdPayment.DisplayLayout.Bands[0].Columns.Add("Unb");
            this.grdPayment.DisplayLayout.Bands[0].Columns["Unb"].Header.VisiblePosition = 0;
            this.grdPayment.DisplayLayout.Bands[0].Columns["Unb"].CellActivation = Activation.Disabled;
            this.grdPayment.DisplayLayout.Bands[0].Columns["Unb"].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            this.grdPayment.DisplayLayout.Bands[0].Columns["Unb"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
            this.grdPayment.DisplayLayout.Bands[0].Columns["Unb"].CellAppearance.ForeColorDisabled = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            this.grdPayment.DisplayLayout.Bands[0].Columns["Unb"].Width = 20;
            this.grdPayment.DisplayLayout.Bands[0].Columns["Unb"].Header.Caption = "";
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_SigType].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].Hidden = true;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POSTransPayment_Fld_ExpDate].Hidden = true;//2943
            //Added By Shitaljit on 1/24/13 to make Payment Description clickable Button.
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].CellAppearance.FontData.Bold = DefaultableBoolean.True;
            //	        this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].CellAppearance.BackColor = Color.GreenYellow;
            // We chan ged the color of the control from GreenYellow to Control - Shrikant Mali
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].CellAppearance.BackColor = SystemColors.Control;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].CellAppearance.ForeColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Width = this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Width + 10;
            this.grdPayment.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription].CellButtonAppearance.BorderColor = Color.Green;

            for (int i = 19; i <= 75; i++)// Change 38 to 39 NileshJ  - for Solutran - 2663 CHANGED FROM 39 TO 42 FOR EVERTEC ARVIND PRIMEPOS-2664 // PRIMEPOS-2761 Change 42 to 43 //CHANGED FROM 43 TO 45 PRIMEPOS-2786 AND PRIMEPOS-2664//CHANGED FROM 45 TO 54 PRIMEPOS-2636//Changed from 54 to 60 2915//2664//2990//3009  //PRIMEPOS-3117 11-Jul-2022 JY changed 69 to 70 //PRIMEPOS-3145 28-Sep-2022 JY 70 to 71 //PRIMEPOS-3375 71 to 74
            {
                this.grdPayment.DisplayLayout.Bands[0].Columns[i].Hidden = true;
            }
            #region PRIMEPOS-2738 
            //check for Strict Revesal and Txn type=Refund
            //if success call ShowOnlyOrigTxnPaytpyes 
            if (Configuration.CSetting.StrictReturn == true && oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
            {
                this.ShowOnlyOrigTxnPaytpyes();
            }
            else
            {
                #endregion
                this.GetShowOnlyOneCCType();
            }
            HighlighShortCut();
            HidePayTypes();  //PRIMEPOS-2675 17-Apr-2019 JY Added
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "populatePayType()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("populatePayType() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void GetShowOnlyOneCCType()
        {
            //End of added By Shitaljit
            if (Configuration.CInfo.ShowOnlyOneCCType == true)
            {
                POSTransPaymentRow newRow = oPosPTList.oPOSTransPaymentData.POSTransPayment.NewPOSTransPaymentRow();
                newRow.TransTypeCode = "-99";
                newRow.TransTypeDesc = "Credit Card";
                newRow.TransPayID = -99;
                newRow.Amount = 0;
                newRow.PaymentProcessor = clsPOSDBConstants.NOPROCESSOR;
                newRow.TransDate = DateTime.Now;

                #region PRIMEPOS-2966 25-May-2021 JY Added
                int nsortOrder = 2;
                try
                {
                    PayType oPayType = new PayType();
                    nsortOrder = oPayType.GetCreditCardSortOrder();
                    if (nsortOrder > 0)
                        nsortOrder -= 1;
                    if (oPosPTList.oPOSTransPaymentData.POSTransPayment.Rows.Count < nsortOrder)
                        nsortOrder = oPosPTList.oPOSTransPaymentData.POSTransPayment.Rows.Count;
                }
                catch { }
                #endregion

                oPosPTList.oPOSTransPaymentData.POSTransPayment.Rows.InsertAt(newRow, nsortOrder);
                foreach (UltraGridRow row in grdPayment.Rows)
                {
                    switch (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim())
                    {
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                            row.Hidden = true;
                            break;
                        case "S": //  Added for Solutran - PRIMEPOS-2663 - NileshJ
                            if (!Configuration.CInfo.S3Enable)
                            {
                                row.Hidden = true;
                            }
                            break;
                        case "X":// StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019 -NileshJ
                            if (!Configuration.CPOSSet.EnableStoreCredit)
                            {
                                row.Hidden = true;
                            }
                            break;
                        case "O":// PRIMEPOS-2841
                            if (!Configuration.CSetting.OnlinePayment)
                            {
                                row.Hidden = true;
                            }
                            break;
                        case "8"://2664
                            row.Hidden = true;
                            break;
                        case "F":
                            if (Configuration.CPOSSet.PaymentProcessor != "EVERTEC")
                            {
                                row.Hidden = true;
                            }
                            break;
                        case "N": //  Added for NBS - PRIMEPOS-3370 //PRIMEPOS-3517
                            row.Hidden = true;
                            break;
                        case "7": //PRIMEPOS-3517
                            if (Configuration.CPOSSet.PaymentProcessor == "VANTIV")
                            {
                                row.Hidden = true;
                            }
                            break;
                    }
                }
            }
        }
        #region PRIMEPOS-2738 
        private void ShowOnlyOrigTxnPaytpyes()
        {
            if (Configuration.CInfo.ShowOnlyOneCCType == true)
            {

                bool isCreditCardUsed = false;

                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = Configuration.ConnectionString;
                DataSet oTransData = oPosTrans.GetPOSPaymentDetail(TransID.ToString(), "");
                string transCode = string.Empty;

                var PaymentProcessor = oTransData.Tables[0].AsEnumerable().Select(p => p.Field<string>("PaymentProcessor").Trim()).ToList();    //PRIMEPOS-3181 19-Jan-2023 JY Added
                for (int count = 0; count < oTransData.Tables.Count; count++)
                {
                    // Get individual datatables here...
                    DataTable table = oTransData.Tables[count];
                    var transCodes = table.AsEnumerable().Select(p => p.Field<string>("TransTypeCode").Trim()).ToList();

                    foreach (var code in transCodes)
                    {
                        foreach (UltraGridRow row in grdPayment.Rows)
                        {
                            if (!isStrictReturn)
                            {
                                if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "3" ||
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "4" ||
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "5" ||
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "6" ||
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "7" ||
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "E" ||
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "-99")
                                {
                                    row.Hidden = true;
                                    isCreditCardUsed = true;
                                }
                            }
                            else
                            {
                                if(Configuration.CPOSSet.PaymentProcessor == "VANTIV") //PRIMEPOS-3523 //PRIMEPOS-3504
                                {
                                    if (transCodes.Contains(row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim()))
                                    {
                                        if (code != "-99" && code != "3" && code != "4" && code != "5" && code != "6" && code != "N" && code != "7" && row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == code)
                                            row.Hidden = false;
                                        else if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "3" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "4" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "5" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "6" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "7" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "N")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                    else if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "-99"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "3"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "4"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "5"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "6"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "7"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "N"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim().ToUpper() == "E")
                                    {
                                        row.Hidden = true;
                                    }
                                    else
                                    {
                                        row.Hidden = false;
                                    }

                                    if (Configuration.CPOSSet.PaymentProcessor != "EVERTEC")
                                    {
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "8")
                                        {
                                            row.Hidden = true;
                                        }
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "F")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                    #region PRIMEPOS-2747
                                    if (!Configuration.CPOSSet.EnableStoreCredit)
                                    {
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "X")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                    #endregion

                                    if (!PaymentProcessor.Contains("PRIMERXPAY"))   //PRIMEPOS-3181 19-Jan-2023 JY Added
                                    {
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "O")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (transCodes.Contains(row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim()))
                                    {
                                        if (code != "-99" && code != "3" && code != "4" && code != "5" && code != "6" && code != "N" && row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == code)
                                            row.Hidden = false;
                                        else if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "3" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "4" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "5" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "6" ||
                                            row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "N")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                    else if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "-99"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "3"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "4"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "5"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "6"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "7"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "N"
                                        || row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim().ToUpper() == "E")
                                    {
                                        row.Hidden = true;
                                    }
                                    else
                                    {
                                        row.Hidden = false;
                                    }

                                    if (Configuration.CPOSSet.PaymentProcessor != "EVERTEC")
                                    {
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "8")
                                        {
                                            row.Hidden = true;
                                        }
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "F")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                    #region PRIMEPOS-2747
                                    if (!Configuration.CPOSSet.EnableStoreCredit)
                                    {
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "X")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                    #endregion

                                    if (!PaymentProcessor.Contains("PRIMERXPAY"))   //PRIMEPOS-3181 19-Jan-2023 JY Added
                                    {
                                        if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "O")
                                        {
                                            row.Hidden = true;
                                        }
                                    }
                                }

                                
                                //PRIMEPOS-3375
                                //if (!PaymentProcessor.Contains("NB_VANTIV"))   //PRIMEPOS-3375 //PRIMEPOS-3482
                                //{
                                //    if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "N")
                                //    {
                                //        row.Hidden = true;
                                //    }
                                //}
                            }
                        }
                    }
                    if (transCodes.Count == 0)
                    {
                        if (!isStrictReturn)
                        {
                            foreach (UltraGridRow row in grdPayment.Rows)
                            {
                                if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "3" ||
                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "4" ||
                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "5" ||
                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "6" ||
                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "7" ||
                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "E" ||
                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "-99")
                                {
                                    row.Hidden = true;
                                    isCreditCardUsed = true;
                                }
                                if (!Configuration.CPOSSet.EnableStoreCredit)
                                {
                                    if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "X")
                                    {
                                        row.Hidden = true;
                                    }
                                }
                            }
                        }
                        else // PRIMEPOS-2747 - StoreCredit
                        {
                            foreach (UltraGridRow row in grdPayment.Rows)
                            {
                                if (!Configuration.CPOSSet.EnableStoreCredit)
                                {
                                    if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "X")
                                    {
                                        row.Hidden = true;
                                    }
                                }
                            }
                        }
                    }
                    if (isCreditCardUsed == false)
                    {
                        if(Configuration.CPOSSet.PaymentProcessor == "VANTIV") //PRIMEPOS-3521 //PRIMEPOS-3522 //PRIMEPOS-3504
                        {
                            if (transCodes.Contains("3") || transCodes.Contains("4") || transCodes.Contains("5") || transCodes.Contains("6") || transCodes.Contains("N") || transCodes.Contains("7"))
                            {
                                if (PaymentProcessor.Contains(Configuration.CPOSSet.PaymentProcessor) || PaymentProcessor.Contains("NB_VANTIV"))   //PRIMEPOS-3181 19-Jan-2023 JY Added if condition
                                {
                                    POSTransPaymentRow newRow = oPosPTList.oPOSTransPaymentData.POSTransPayment.NewPOSTransPaymentRow();
                                    newRow.TransTypeCode = "-99";
                                    newRow.TransTypeDesc = "Credit Card";
                                    newRow.TransPayID = -99;
                                    newRow.Amount = 0;
                                    newRow.PaymentProcessor = clsPOSDBConstants.NOPROCESSOR;
                                    newRow.TransDate = DateTime.Now;

                                    oPosPTList.oPOSTransPaymentData.POSTransPayment.Rows.InsertAt(newRow, 2);
                                }
                                isCreditCardUsed = true;
                            }
                        }
                        else
                        {
                            if (transCodes.Contains("3") || transCodes.Contains("4") || transCodes.Contains("5") || transCodes.Contains("6"))
                            {
                                if (PaymentProcessor.Contains(Configuration.CPOSSet.PaymentProcessor))   //PRIMEPOS-3181 19-Jan-2023 JY Added if condition
                                {
                                    POSTransPaymentRow newRow = oPosPTList.oPOSTransPaymentData.POSTransPayment.NewPOSTransPaymentRow();
                                    newRow.TransTypeCode = "-99";
                                    newRow.TransTypeDesc = "Credit Card";
                                    newRow.TransPayID = -99;
                                    newRow.Amount = 0;
                                    newRow.PaymentProcessor = clsPOSDBConstants.NOPROCESSOR;
                                    newRow.TransDate = DateTime.Now;

                                    oPosPTList.oPOSTransPaymentData.POSTransPayment.Rows.InsertAt(newRow, 2);
                                }
                                isCreditCardUsed = true;
                            }
                        }                       
                    }
                    this.grdPayment.Update();
                }
            }
        }
        //HIDING THE PAYTYPE ShowOnlyOrigTxnPaytpyes
        #endregion
        private void HighlighShortCut()
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "HighlighShortCut()", clsPOSDBConstants.Log_Entering);
            logger.Trace("populatePayType() - " + clsPOSDBConstants.Log_Entering);
            foreach (UltraGridRow oRow in this.grdPayment.Rows)
            {
                String strValue = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim();
                if (strValue == "1")
                {
                    oRow.Cells["Unb"].Value = "S";
                }
                else if (strValue == "2")
                {
                    oRow.Cells["Unb"].Value = "K";
                }
                else if (strValue == "3")
                {
                    oRow.Cells["Unb"].Value = "A";
                }
                else if (strValue == "4")
                {
                    oRow.Cells["Unb"].Value = "V";
                }
                else if (strValue == "5")
                {
                    oRow.Cells["Unb"].Value = "M";
                }
                else if (strValue == "6")
                {
                    oRow.Cells["Unb"].Value = "D";
                }
                else if (strValue == "7")
                {
                    oRow.Cells["Unb"].Value = "T";
                }
                else if (strValue == "C")
                {
                    oRow.Cells["Unb"].Value = "U";
                }
                else if (strValue == "H")
                {
                    oRow.Cells["Unb"].Value = "H";
                }
                else if (strValue == "E")
                {
                    oRow.Cells["Unb"].Value = "E";
                }
                else if (strValue == "-99")
                {
                    oRow.Cells["Unb"].Value = "R";
                }
                else if (strValue == "8")//2664
                {
                    oRow.Cells["Unb"].Value = "AT";
                }
                //Added By Shitaljit for new POS_Core.Resources.PayTypes on 25 Jan 2013
                //SKAVMDTUHER already use short keys.
                else
                {
                    sNewAddedPayTypesID += strValue + "$";
                    oRow.Cells["Unb"].Value = strValue;
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "HighlighShortCut()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("populatePayType() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void HidePayTypes()
        {
            try
            {
                logger.Trace("HidePayTypes() - " + clsPOSDBConstants.Log_Entering);
                PayType oPayType = new PayType();
                PayTypeData oPayTypeData;
                oPayTypeData = oPayType.PopulateList("");

                foreach (PayTypeRow oPayTypeRow in oPayTypeData.PayTypes.Rows)
                {
                    if (oPayTypeRow.IsHide == true)
                    {
                        foreach (UltraGridRow oRow in this.grdPayment.Rows)
                        {
                            if (oPayTypeRow.PaytypeID.ToString().Trim().ToUpper() == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim().ToUpper())
                            {
                                oRow.Hidden = true;
                                break;
                            }
                        }
                    }
                }
                logger.Trace("HidePayTypes() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "HidePayTypes()");
            }
        }
        #endregion Grid Events

        #region WriteTotalToPoleDisplay
        public void WriteTotalToPoleDisplay()
        {
            try
            {
                // Added by Farman for screen Amount Display

                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "WriteTotalToPoleDisplay()", clsPOSDBConstants.Log_Entering);
                logger.Trace("WriteTotalToPoleDisplay() - " + clsPOSDBConstants.Log_Entering);
                int LineLen = Configuration.CPOSSet.PD_LINELEN;
                frmMain.PoleDisplay.ClearPoleDisplay();
                String strPoleData = oPosPTList.FormatPoleDisplayText(oPosPTList.totalAmount, oPosPTList.amountCashBack, oPosPTList.totalTaxAmt);
                frmMain.PoleDisplay.WriteToPoleDisplay(strPoleData);

                // End
            }
            catch (Exception Exp)
            {
                //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "WriteTotalToPoleDisplay()", clsPOSDBConstants.Log_Exception_Occured+Exp.StackTrace.ToString());
                logger.Fatal(Exp, "WriteTotalToPoleDisplay()");
            }
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "WriteTotalToPoleDisplay()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("WriteTotalToPoleDisplay() - " + clsPOSDBConstants.Log_Exiting);
        }

        #endregion WriteTotalToPoleDisplay

        #region btnClose
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            bool retval = RemoveAllPayments();
            #region PRIMEPOS-2836 15-Apr-2020 JY Added
            if (posTrans.oTransDData != null && posTrans.oTransDData.Tables.Count > 0)
            {
                if (posTrans.oTransDData.Tables[0].Rows.Count > 0)
                {
                    foreach (TransDetailRow row in posTrans.oTransDData.TransDetail.Rows)
                    {
                        if (row.S3TransID > 0)
                        {
                            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                            {
                                if (dtTransDetail != null && dtTransDetail.Rows.Count > 0)
                                {
                                    foreach (TransDetailRow oRow in dtTransDetail.Rows)
                                    {
                                        if (row.TransDetailID == oRow.TransDetailID)
                                        {
                                            row.S3TransID = oRow.S3TransID;
                                            row.S3PurAmount = oRow.S3PurAmount;
                                            row.S3DiscountAmount = oRow.S3DiscountAmount;
                                            row.S3TaxAmount = oRow.S3TaxAmount;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                row.S3TransID = 0;
                                row.S3PurAmount = 0;
                                row.S3DiscountAmount = 0;
                                row.S3TaxAmount = 0;
                            }
                        }
                    }
                }

                if (posTrans.oTransDData.Tables.Count > 1)
                {
                    string strTableName = posTrans.oTransDData.Tables[1].TableName;
                    posTrans.oTransDData.Tables.Remove(strTableName);
                }
            }
            #endregion
            oPosPTList.oPOSTransPaymentDataPaid = null;
            posTrans.oStoreCreditData = null;   //PRIMEPOS-2938 28-Jan-2021 JY Added
            if (!retval)//2943
                this.Close();
        }
        #endregion btnClose

        #region isOverPayment
        //Naim 22Oct2007 this will take care of any amount that generates due amount greater than $1000
        //wll return true if difference of paid amount and due amount is more than $1000
        //other wise its false

        private void GetGridPaymentAmount(ref Decimal totalPaid, ref Decimal dCashPayment)
        {
            for (int i = 0; i < this.grdPayment.Rows.Count; i++)
            {
                //totalPaid+=Convert.ToDecimal(grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text);
                totalPaid += Configuration.convertNullToDecimal(this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                if (this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].GetText(MaskMode.Raw).Trim() == "1")
                    dCashPayment += Configuration.convertNullToDecimal(this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)); //Sprint-25 - PRIMEPOS-2411 24-Apr-2017 JY Added 
            }
        }
        private bool isOverPayment(out bool bTenderedAmountOverrideCancel)
        {
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "isOverPayment()", clsPOSDBConstants.Log_Entering);
            logger.Trace("isOverPayment() - " + clsPOSDBConstants.Log_Entering);
            bool retVal = false;
            bTenderedAmountOverrideCancel = false;
            Decimal totalPaid = 0;
            Decimal dCashPayment = 0; //Sprint-25 - PRIMEPOS-2411 24-Apr-2017 JY Added 
            strMaxTenderedAmountOverrideUser = "";  //PRIMEPOS-2402 20-Jul-2021 JY Added
            oPosPTList.GetTotalPaidAmount(ref totalPaid, ref dCashPayment);
            GetGridPaymentAmount(ref totalPaid, ref dCashPayment);

            #region Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
            if (dCashPayment > Configuration.UserMaxTenderedAmountLimit)
            {
                //Added by Farman Ansari
                oPosPTList.CheckMaxTenderAmt(dCashPayment, ref bTenderedAmountOverrideCancel, ref strMaxTenderedAmountOverrideUser);    //PRIMEPOS-2619 16-Nov-2018 JY modified //PRIMEPOS-2402 20-Jul-2021 JY Added strMaxTenderedAmount
                //End
            }
            #endregion

            else if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                oPosPTList.CheckTotalPaidAmount(totalPaid, ref retVal);
            }
            //}			catch(Exception ) {}
            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "isOverPayment()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("isOverPayment() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }
        #endregion isOverPayment

        #region split functions for CalculatePaidAmount

        private void AllowGridPaymentEdits(Decimal CashbackAmount)
        {
            if (CashbackAmount > 0)
            {
                for (int i = 0; i < grdPayment.Rows.Count; i++)
                {
                    switch (this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim())
                    {
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                            this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Activation = Activation.AllowEdit;
                            break;
                        default:
                            this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Activation = Activation.Disabled;
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < grdPayment.Rows.Count; i++)
                {
                    this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Activation = Activation.AllowEdit;
                }
            }
        }

        private void ComputeBalanceDue(Decimal CashbackAmount)
        {
            Decimal totalPaid = 0;
            logger.Trace("AmountPaid : " + oPosPTList.amountPaid.ToString());
            logger.Trace("Total Amount : " + oPosPTList.totalAmount.ToString() + " and Trans Fee: " + oPosPTList.TransFeeAmt);  //PRIMEPOS-3117 11-Jul-2022 JY Added TransFeeAmt
            //totalPaid = oPosPTList.totalAmount + CashbackAmount - oPosPTList.amountPaid; //Can we not use the backend field
            totalPaid = oPosPTList.totalAmount + oPosPTList.TransFeeAmt + CashbackAmount - oPosPTList.amountPaid; //Can we not use the backend field //PRIMEPOS-3117 11-Jul-2022 JY modified
            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                if (totalPaid >= 0)
                {
                    oPosPTList.amountBalanceDue = oPosPTList.totalAmount + oPosPTList.TransFeeAmt - oPosPTList.amountPaid + CashbackAmount; //Change    //PRIMEPOS-3117 11-Jul-2022 JY modified
                    this.txtAmtBalanceDue.Text = oPosPTList.amountBalanceDue.ToString("##########0.00"); //DataBinding ??
                    oPosPTList.ChangeDue = 0;
                    this.txtAmtChangeDue.Text = "0.00";
                }
                else
                {
                    oPosPTList.amountBalanceDue = 0;
                    this.txtAmtBalanceDue.Text = "0.00";
                    oPosPTList.ChangeDue = totalPaid * -1;
                    this.txtAmtChangeDue.Text = Convert.ToString(oPosPTList.ChangeDue);
                    this.txtAmtChangeDue.Text = Convert.ToDecimal(this.txtAmtChangeDue.Text).ToString("##########0.00");
                }
            }
            else
            {
                oPosPTList.amountBalanceDue = oPosPTList.totalAmount + oPosPTList.TransFeeAmt - oPosPTList.amountPaid; //Change here    //PRIMEPOS-3117 11-Jul-2022 JY modified
                this.txtAmtBalanceDue.Text = oPosPTList.amountBalanceDue.ToString("##########0.00");//Data Binding??
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn && oPosPTList.isROA == false)
                {
                    oPosPTList.ChangeDue = Math.Abs(oPosPTList.amountPaid);
                    this.txtAmtChangeDue.Text = oPosPTList.ChangeDue.ToString();
                }
                else if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                {
                    oPosPTList.ChangeDue = Math.Abs((oPosPTList.totalAmount + oPosPTList.TransFeeAmt - oPosPTList.amountPaid)); //PRIMEPOS-3117 11-Jul-2022 JY modified
                    this.txtAmtChangeDue.Text = Convert.ToString(Math.Abs(oPosPTList.totalAmount + oPosPTList.TransFeeAmt - oPosPTList.amountPaid));    //PRIMEPOS-3117 11-Jul-2022 JY modified
                }
            }

        }

        private void DisposeFormForZeroTransactionAmount()
        {

            //Change by SRT(Abhishek D ) Date : 12 march 2010
            if (Configuration.CPOSSet.AllowZeroAmtTransaction == true)
            {
                if (oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.Rows.Count > 0)//|| this.totalAmount == 0)//Changed by Naim for additional check
                {
                    if (oPosPTList.amountBalanceDue == 0)
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                if (oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.Rows.Count > 0 || oPosPTList.totalAmount + oPosPTList.TransFeeAmt + oPosPTList.amountCashBack == 0) //PRIMEPOS-3117 11-Jul-2022 JY modified
                {
                    if (oPosPTList.amountBalanceDue == 0)
                    {
                        this.Close();
                    }
                }
            }
        }

        private void calculatePaidAmount()
        {
            logger.Trace("calculatePaidAmount() - " + clsPOSDBConstants.Log_Entering);
            Decimal totalPaid = 0;
            Decimal CashbackAmount = 0;
            Decimal totTransFeeAmt = 0; //PRIMEPOS-3117 11-Jul-2022 JY Added
            oPosPTList.totalNonIIASAmountPaid = 0;
            oPosPTList.totalIIASAmountPaid = 0;
            oPosPTList.totalIIASRxAmountPaid = 0;
            //bool removeEBTTax = false;

            //if (oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.Rows.Count > 0)
            //{
            //    removeEBTTax = ParentForm.RevomeTaxFromEBTItems();
            //}
            //oPosPTList.RemoveEBTTaxAndGetTotals(removeEBTTax, ref totalPaid, ref CashbackAmount);


            foreach (POSTransPaymentRow oRow in oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.Rows)
            {
                if (oRow.Amount > 0 && this.TotalEBTAmount > 0 && oPosPTList.oPayTpes == POS_Core.Resources.PayTypes.EBT && TotalEBTItemsTaxAmount > 0)
                {
                    //Added ny shitaljit on 26Dec2013 for Remove Tax from EBT items.
                    if (ParentForm.RevomeTaxFromEBTItems() == true)
                    {
                        oPosPTList.totalAmount = oPosPTList.totalAmount - TotalEBTItemsTaxAmount;
                        oPosPTList.totalTaxAmt = oPosPTList.totalTaxAmt - TotalEBTItemsTaxAmount;
                        this.txtAmtTotal.Text = oPosPTList.totalAmount.ToString("##########0.00");
                        TotalEBTItemsTaxAmount = 0;
                    }
                }
                if (oRow.TransTypeCode.Trim() != "B")
                {
                    totalPaid += oRow.Amount;
                    totTransFeeAmt += oRow.TransFeeAmt; //PRIMEPOS-3117 11-Jul-2022 JY Added
                    if (oRow.IsIIASPayment == true)
                    {
                        oPosPTList.totalIIASAmountPaid += oRow.Amount;
                    }
                    else
                    {
                        oPosPTList.totalNonIIASAmountPaid += oRow.Amount;
                    }
                }

                if (oRow.TransTypeCode.Trim() == "B")
                {
                    CashbackAmount = Math.Abs(oRow.Amount);
                }
            }


            //Allow Edits for GridPayment
            AllowGridPaymentEdits(CashbackAmount);
            //Added By SRT(Ritesh Parekh) Date : 19-Aug-2009
            oPosPTList.ComputeIIASAmounts(totalNonIIASAmount);
            //End Of Added By SRT(Ritesh Parekh)
            //Data Binding??
            oPosPTList.amountPaid = totalPaid;
            oPosPTList.TransFeeAmt = totTransFeeAmt;    //PRIMEPOS-3117 11-Jul-2022 JY Added
            this.txtAmtTotal.Text = oPosPTList.totalAmount.ToString("##########0.00");
            this.txtAmtPaid.Text = oPosPTList.amountPaid.ToString("##########0.00");
            oPosPTList.amountCashBack = CashbackAmount;
            this.txtAmtCashBack.Text = oPosPTList.amountCashBack.ToString("##########0.00");
            //Code To Remove
            txtIIASRxPaid.Text = Convert.ToDecimal(oPosPTList.totalIIASRxAmountPaid.ToString()).ToString("##########0.00");
            txtIIASNonRxPaid.Text = Convert.ToDecimal(oPosPTList.totalIIASNonRxAmountPaid.ToString()).ToString("##########0.00");
            //End Of Code To Remove
            ComputeBalanceDue(CashbackAmount);
            DisposeFormForZeroTransactionAmount();
            logger.Trace("calculatePaidAmount() - " + clsPOSDBConstants.Log_Exiting);

        }

        #endregion split functions for CalculatePaidAmount

        #region ProcessCoupons
        private bool ProcessCoupons()
        {
            bool returnValue = true;
            decimal cellValue = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
            if (Configuration.CLoyaltyInfo.UseCustomerLoyalty && Configuration.CLoyaltyInfo.RedeemMethod == (int)CLRedeemMethod.Manual)
            {
                if (cellValue != 0)
                {
                    string strRefNo = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].GetText(MaskMode.Raw);
                    if (strRefNo != null && strRefNo.Trim().Length > 0)
                    {
                        if (strRefNo.StartsWith("!") && strRefNo.EndsWith("!"))
                        {
                            strRefNo = strRefNo.Replace("!", "").Trim();
                            Int64 clcouponID = Configuration.convertNullToInt64(strRefNo);
                            returnValue = ProcessCouponID(clcouponID, cellValue);
                        }
                        //else
                        //{
                        //    returnValue = false;    //PRIMEPOS-2783 30-Jan-2020 JY Added
                        //}
                    }
                    //else
                    //{
                    //    returnValue = false;    //PRIMEPOS-2783 30-Jan-2020 JY Added
                    //}
                }
            }
            if (returnValue == true)
            {
                AddRowToPaid(this.grdPayment.ActiveRow);
            }
            return returnValue;
        }

        private bool ProcessCouponID(Int64 clCouponID, decimal amount)
        {
            logger.Trace("ProcessCouponID() - " + clsPOSDBConstants.Log_Entering);
            bool returnValue = true;
            if (clCouponID > 0)
            {
                if (!oPosPTList.IsValidCoupon(clCouponID))
                    return false;

                decimal calculatedCouponValue = 0;
                decimal remainingclamount = 0;
                CLCouponsRow oCLCouponsRow = oPosPTList.ApplyCoupon(clCouponID, amount, ref calculatedCouponValue, ref remainingclamount);
                returnValue = false;
                if (oCLCouponsRow == null)
                {
                    return returnValue;
                }
                else if (oCLCouponsRow.IsCouponUsed == false)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = string.Empty;
                    if (remainingclamount > 0)
                    {
                        returnValue = true;
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = calculatedCouponValue;
                    }
                    if (calculatedCouponValue > 0)
                    {
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].Value = oCLCouponsRow.ID;
                    }
                    //returnValue = true;
                }
            }
            logger.Trace("ProcessCouponID() - " + clsPOSDBConstants.Log_Exiting);
            return returnValue;
        }

        #endregion ProcessCoupons

        #region MoveToRow
        private void MoveToRow(string rowCode)
        {
            foreach (UltraGridRow oRow in this.grdPayment.Rows)
            {

                if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().ToUpper().Trim() == rowCode.Trim() ||
                     oRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString().ToUpper().Trim() == rowCode.Trim())
                {
                    if (oRow.Hidden == false)
                    {
                        this.grdPayment.ActiveRow = oRow;
                        this.grdPayment.ActiveCell = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                        this.grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                        if (rowCode.Trim() == "H" && oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation == Activation.AllowEdit)
                        {
                            this.SearchHouseChargeInfo();
                        }
                        if (rowCode.Trim() == "2" && oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation == Activation.AllowEdit)   //PRIMEPOS-2539 09-Jul-2018 JY Added
                        {
                            if (!bAllowCheckPayment)
                                bAllowCheckPayment = oPosPTList.AllowCheckPaymentPriviliges();
                        }
                    }
                    break;
                }
            }
        }
        #endregion MoveToRow

        #region RemoveAllPayments
        private bool RemoveAllPayments()
        {
            bool retval = false;
            int grdCount = this.grdPaymentComplete.Rows.Count;//PRIMEPOS-2814
            while (this.grdPaymentComplete.Rows.Count > 0)
            {
                #region PRIMEPOS-2814
                try
                {
                    RemovePayment(this.grdPaymentComplete.Rows[0]);
                    if (grdCount == this.grdPaymentComplete.Rows.Count)
                    {
                        if (!(this.oPosTrans.CurrentTransactionType == POSTransactionType.Sales && Configuration.CPOSSet.PaymentProcessor == "ELAVON"))
                        {
                            clsUIHelper.ShowOKMsg(" The transaction cannot be voided due to some issue");
                            break;
                        }
                        else
                        {
                            retval = true;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsUIHelper.ShowOKMsg(" The transaction cannot be voided due to some issue");
                    break;
                }
                #endregion
            }
            IsStoreCredit = false;  //PRIMEPOS-2938 28-Jan-2021 JY Added
            return retval;
        }
        #endregion RemoveAllPayments

        #region CancelTrans
        private void CancelTrans()
        {
            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.DeleteTrans.ID, UserPriviliges.Permissions.DeleteTrans.Name))
            {
                if (Resources.Message.Display("Are your sure, you want to cancel current transaction?", "Cancel Transaction", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool retval = RemoveAllPayments();
                    oPosPTList.oPOSTransPaymentDataPaid = null;
                    oPosPTList.CancelTransaction = true;
                    posTrans.oStoreCreditData = null;   //PRIMEPOS-2938 28-Jan-2021 JY Added
                    if (!retval)//2943
                        this.Close();
                }
                else
                {
                    this.grdPayment.Focus();
                }
            }
            else
            {
                this.grdPayment.Focus();
            }
        }
        #endregion CancelTrans

        #region ControlCursorNavigation
        private void ControlCursorNavigation(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        if (this.grdPayment.ActiveCell != null && grdPayment.ActiveRow.Index == grdPayment.Rows.Count - 1 && grdPayment.ActiveCell.Column.Index == grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Column.Index)    //Sprint-18 20-Nov-2014 JY Added additional condition to resolve object reference error - this.grdPayment.ActiveCell != null
                        {
                            grdPayment.ActiveRow = grdPayment.Rows[0];
                            grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                            grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                            e.Handled = true;
                        } //Following Else If Added by Krishna on 8 July 2011
                        else if (this.grdPayment.ActiveCell != null && (grdPayment.ActiveCell.Column.Index == grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Column.Index || this.grdPayment.ActiveCell.Column.Index == grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Column.Index))  //Sprint-18 20-Nov-2014 JY Added additional condition to resolve object reference error - this.grdPayment.ActiveCell != null
                        {
                            grdPayment.ActiveRow = grdPayment.Rows[0];
                            grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                            grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                            e.Handled = true;
                        }//Till here Added by Krishna on 8 July 2011
                        else if (IsStoreCredit) // PRIMEPOS-2747 - StoreCredit
                        {
                            grdPayment.ActiveRow = grdPayment.Rows[0];
                            grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                            grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                            if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                            {
                                int SelectedIndex = 0;
                                DataRow[] result = oPosPTList.oPOSTransPaymentData.Tables[0].Select("TransTypeCode = 'X'");
                                if (result.Length > 0)
                                {
                                    SelectedIndex = oPosPTList.oPOSTransPaymentData.Tables[0].Rows.IndexOf(result[0]);
                                }
                                this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                                this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                            }
                            e.Handled = true;
                        }
                        else
                        {
                            grdPayment.PerformAction(UltraGridAction.ExitEditMode, false, false);
                            grdPayment.PerformAction(UltraGridAction.NextCell, false, false);
                            grdPayment.PerformAction(UltraGridAction.EnterEditMode, false, false);
                            e.Handled = true;
                        }
                        break;
                    }
                case Keys.Up:
                    {
                        if (grdPayment.ActiveRow.VisibleIndex > 0)
                        {
                            grdPayment.ActiveRow = grdPayment.Rows.GetRowAtVisibleIndex(grdPayment.ActiveRow.VisibleIndex - 1);
                            grdPayment.ActiveCell = grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                        }
                        grdPayment.PerformAction(UltraGridAction.EnterEditMode, false, false);
                        grdPayment.Refresh();
                        e.Handled = true;
                        break;
                    }
                case Keys.Down:
                    {
                        if (grdPayment.ActiveRow.VisibleIndex < grdPayment.Rows.VisibleRowCount - 1)
                        {
                            grdPayment.ActiveRow = grdPayment.Rows.GetRowAtVisibleIndex(grdPayment.ActiveRow.VisibleIndex + 1);
                            grdPayment.ActiveCell = grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                        }
                        grdPayment.PerformAction(UltraGridAction.EnterEditMode, false, false);
                        grdPayment.Refresh();
                        e.Handled = true;
                        break;
                    }
                case Keys.Right:
                    {
                        grdPayment.PerformAction(UltraGridAction.ExitEditMode, false, false);
                        grdPayment.PerformAction(UltraGridAction.NextCellByTab, false, false);
                        e.Handled = true;
                        grdPayment.PerformAction(UltraGridAction.EnterEditMode, false, false);
                        grdPayment.Refresh();
                        break;
                    }
                case Keys.Left:
                    {
                        grdPayment.PerformAction(UltraGridAction.ExitEditMode, false, false);
                        grdPayment.PerformAction(UltraGridAction.PrevCellByTab, false, false);
                        e.Handled = true;
                        grdPayment.PerformAction(UltraGridAction.EnterEditMode, false, false);
                        grdPayment.Refresh();
                        break;
                    }
            }

        }

        #endregion ControlCursorNavigation

        #region SearchHouseChargeInfo
        private void SearchHouseChargeInfo()
        {
            try
            {
                bool bStatus = oPosPTList.HouseInfoUserPreviliges();
                if (bStatus == false)
                    return;   //PRIMEPOS-2557 06-Jul-2018 JY Added

                //Fix by Manoj 7/28/2015
                Decimal amount = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));

                string acctID = "";
                string strPatientName = string.Empty;
                string sHousechargeAccountNo = string.Empty;
                oPosPTList.FetchHouseInfoCustomer(ref sHousechargeAccountNo, ref acctID, ref strPatientName);
                oPosPTList.VerifyHousechargeAcc(ref sHousechargeAccountNo);
                this.SearchAccNoHouseChargeInfo(ref sHousechargeAccountNo, ref acctID, ref strPatientName);
                this.ValidateHouseChargeConfirmation(ref sHousechargeAccountNo, amount);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchHouseCHargeInfo()");
                ClearGridRow(this.grdPayment.ActiveRow);
                if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 22-Jun-2022 JY Added
                    clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void SearchAccNoHouseChargeInfo(ref string sHousechargeAccountNo, ref string acctID, ref string strPatientName)
        {
            //frmSearch oSearch = null;
            frmSearchMain oSearch = null;
            if (sHousechargeAccountNo == string.Empty || sHousechargeAccountNo == "0")
            {
                //oSearch = new frmSearch(clsPOSDBConstants.PrimeRX_HouseChargeInterface, acctID, strPatientName.Replace(",", ""));
                oSearch = new frmSearchMain(clsPOSDBConstants.PrimeRX_HouseChargeInterface, acctID, strPatientName.Replace(",", ""), true); //20-Dec-2017 JY Added new reference
                oSearch.SearchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    sHousechargeAccountNo = oSearch.SelectedRowID();
                }
            }

        }

        private void ValidateHouseChargeConfirmation(ref string sHousechargeAccountNo, Decimal amount)
        {
            if (sHousechargeAccountNo != "" && sHousechargeAccountNo != "0")
            {
                string AcctName = "";
                string strSignature = "";
                string strSigType = "";
                //bool result = clsHouseCharge.getHCConfirmation(strCode, out AcctName, amount, out strSignature);
                bool result = clsHouseCharge.getHCConfirmation(sHousechargeAccountNo, out AcctName, amount, out strSignature, out strSigType, out OverrideHousechargeLimitUser);

                if (result == true)
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = sHousechargeAccountNo + "\\" + AcctName;
                    // Code to uncomment after checking flow
                    if (strSigType == clsPOSDBConstants.BINARYIMAGE)
                    {
                        Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        //Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                        string errorMsg = string.Empty;
                        SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                        //PRIMEPOS-2528 (Suraj) 1-june-18 Added if else for drawing HPSPAX sign
                        if (SigPadUtil.DefaultInstance.isPAX)
                        {
                            sigDisp.DrawSignaturePAX(strSignature, ref bmp, out errorMsg);
                        }
                        else if (SigPadUtil.DefaultInstance.isISC || SigPadUtil.DefaultInstance.isWP)
                        {
                            byte[] signByte = Convert.FromBase64String(SigPadUtil.DefaultInstance.CustomerSignature);
                            sigDisp.DrawSignatureMX(signByte, ref bmp, out errorMsg);
                        }
                        else
                        {
                            sigDisp.DrawSignature(strSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                        }
                        ImageConverter converter = new ImageConverter();
                        byte[] btarr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value = btarr;// ms.ToArray();
                                                                                                                        //this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value = strSignature;
                    }
                    else
                    {
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].Value = strSignature;
                    }
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].Value = strSigType;
                    AddRowToPaid(this.grdPayment.ActiveRow);
                    SendKeys.Send("{ENTER}");
                }
                else
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = "";
                }
            }
        }

        #endregion SearchHouseChargeInfo

        #region SelectCLCoupons
        private void SelectCLCoupons()
        {
            //Following if is added by shitaljit for now allowing users to look for active coupons in return ROA transactions.
            if (oPosPTList.oTransactionType != POS_Core.TransType.POSTransactionType.Sales)
            {
                return;
            }
            if (Configuration.CLoyaltyInfo.UseCustomerLoyalty && Configuration.CLoyaltyInfo.RedeemMethod == (int)CLRedeemMethod.Manual && oPosPTList.oCLCardRow != null)
            {
                frmCLCouponsView ofrm = new frmCLCouponsView(oPosPTList.oCLCardRow.CLCardID);
                if (ofrm.ShowDialog(this) == DialogResult.OK)
                {
                    Int64 couponID = ofrm.GetSelectedCouponID();
                    if (couponID > 0)
                    {
                        decimal cellValue = Convert.ToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                        if (cellValue == 0)
                        {
                            cellValue = oPosPTList.amountBalanceDue;
                            if (cellValue == 0)
                                return; //PRIMEPOS-2434 06-May-2021 JY Added
                        }

                        if (ProcessCouponID(couponID, cellValue))
                        {
                            AddRowToPaid(this.grdPayment.ActiveRow);
                            SendKeys.Send("{ENTER}");
                        }
                    }
                }
            }
        }

        #endregion SelectCLCoupons

        /// <summary>
        /// Author: Shitaljit
        /// Added to get delivery address in case the current customer is default customer.
        /// </summary>
        /// 

        #region GetDeliveryAddress
        private void GetDeliveryAddress()
        {
            //POS_Core.ErrorLogging.Logs.Logger(this.Text, ">>>> GetDeliveryAddress()<<<<  for Deleivery address", clsPOSDBConstants.Log_Entering);
            logger.Trace("GetDeliveryAddress() - " + clsPOSDBConstants.Log_Entering);
            Customer oCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();
            CustomerRow oCustRow = null;
            string strCode = "";
            if (oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.AccountNumber == -1)
            {
                //POS_Core.ErrorLogging.Logs.Logger(this.Text, ">>>> SearchCustomer()<<<<  for Deleivery address", "Starting");
                logger.Trace("GetDeliveryAddress() - frmCustomerSearch called");
                frmCustomerSearch oSearch = new frmCustomerSearch(oPosPTList.oCurrentCustRow.AccountNumber.ToString(), false);
                oSearch.ActiveOnly = 1;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        return;
                    }

                    //Added by Farman Ansari
                    oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
                    if (oCustdata.Tables[0].Rows.Count == 0)
                    {
                        oCustRow = oSearch.SelectedRow();
                        oPosPTList.GetAllDeliveryAddress(oCustomer, oCustdata, oCustRow, ref strCode);
                    }
                    else
                    {
                        oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                        oPosPTList.oCurrentCustRow = oCustRow;
                    }
                    //End                           
                }
                //POS_Core.ErrorLogging.Logs.Logger(this.Text, ">>>> SearchCustomer()<<<<  for Deleivery address", "Completed");
                logger.Trace("GetDeliveryAddress() - frmCustomerSearch completed");
            }
            if (oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.PatientNo > 0)
            {
                sDeliveryAddress = PrimeRXHelper.GetPatientDeliveryAddress(oPosPTList.oCurrentCustRow.PatientNo.ToString());
            }
            //POS_Core.ErrorLogging.Logs.Logger(this.Text, ">>>> GetDeliveryAddress()<<<<  for Deleivery address", clsPOSDBConstants.Log_Exiting);
            logger.Trace("GetDeliveryAddress() - " + clsPOSDBConstants.Log_Exiting);
        }

        #endregion GetDeliveryAddress

        #region AddRowToPaid

        private void AddRowToPaid(UltraGridRow oGridRow)
        {

            logger.Trace("AddRowToPaid() - Payment Made of $" + Configuration.convertNullToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)).ToString() + "By " + Configuration.convertNullToString(grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString()) + " - " + clsPOSDBConstants.Log_Entering);
            try
            {
                POSTransPaymentRow oRowPaid = (POSTransPaymentRow)oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.NewPOSTransPaymentRow();
                #region PRIMEPOS-2738 ADDED BY ARVIND
                if (Configuration.CSetting.StrictReturn == true && oPosPTList.oTransactionType == POSTransactionType.SalesReturn && !string.IsNullOrWhiteSpace(TransPayID))
                {
                    //oOrigPayTransData.Tables[0].AsEnumerable().Where()
                    oRowPaid.TransPayID = Convert.ToInt32(TransPayID);
                }
                #endregion
                oRowPaid.TransTypeCode = oPosPTList.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].TransTypeCode;
                oRowPaid.TransTypeDesc = oPosPTList.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].TransTypeDesc;
                oRowPaid.TransID = oPosPTList.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].TransID;
                oRowPaid.TransDate = DateTime.Now;
                #region PRIMEPOS-2402 09-Jul-2021 JY Added
                if (oRowPaid.TransTypeCode.Trim().ToUpper() == "H" && OverrideHousechargeLimitUser != "")
                {
                    oRowPaid.OverrideHousechargeLimitUser = OverrideHousechargeLimitUser;
                    OverrideHousechargeLimitUser = "";
                }
                if (strMaxTenderedAmountOverrideUser != "")
                {
                    oRowPaid.MaxTenderedAmountOverrideUser = strMaxTenderedAmountOverrideUser;
                    strMaxTenderedAmountOverrideUser = "";
                }
                #endregion
                oRowPaid.Tokenize = Tokenize;   //PRIMEPOS-3145 28-Sep-2022 JY Added

                //oRowPaid.RefNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].GetText(MaskMode.Raw);   //PRIMEPOS-2783 19-Feb-2020 JY Commented
                #region PRIMEPOS-2783 19-Feb-2020 JY Added
                if (Configuration.convertNullToInt64(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].Value) != 0)
                    oRowPaid.RefNo = "CL Coupon# " + Configuration.convertNullToString(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].Value);
                else
                    oRowPaid.RefNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].GetText(MaskMode.Raw);
                #endregion

                oRowPaid.HC_Posted = oPosPTList.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].HC_Posted;
                oRowPaid.CustomerSign = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].GetText(MaskMode.Raw);
                oRowPaid.CCTransNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo].GetText(MaskMode.Raw);
                oRowPaid.CCName = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].GetText(MaskMode.Raw);
                oRowPaid.AuthNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].GetText(MaskMode.Raw);
                oRowPaid.Amount = Configuration.convertNullToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                oRowPaid.IsIIASPayment = Configuration.convertNullToBoolean(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value.ToString());
                oRowPaid.CLCouponID = Configuration.convertNullToInt(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].Value.ToString());
                if (oGridRow.Cells["SigType"].Value.ToString() != "")
                    oRowPaid.SigType = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].GetText(MaskMode.Raw);
                //Code added to initialize Payment Processor. Need to update as development goes onword.
                oRowPaid.PaymentProcessor = clsPOSDBConstants.NOPROCESSOR;

                //PRIMEPOS-2664 ADDED BY ARVIND 
                //EvertecCash(oGridRow);
                //oRowPaid.ControlNumber = !string.IsNullOrEmpty(this.controlNumber) ? this.controlNumber : string.Empty;
                //

                oRowPaid.AuthNo = EvertecCash(oGridRow);

                string Amount = string.Empty;
                string EvertecCityStateTax = CalculateTaxAmount(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw), out Amount);//PRIMEPOS-2857
                oRowPaid.EvertecTaxBreakdown = !string.IsNullOrEmpty(EvertecCityStateTax) ? EvertecCityStateTax : string.Empty;//PRIMEPOS-2857

                oRowPaid.PrimeRxPayTransID = !string.IsNullOrEmpty(this.PrimeRxPayTransID) ? this.PrimeRxPayTransID : string.Empty;//2915
                if (string.IsNullOrWhiteSpace(oRowPaid.PrimeRxPayTransID))
                {
                    oRowPaid.ApprovedAmount = Configuration.convertNullToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                }
                else
                {
                    oRowPaid.Status = clsPOSDBConstants.PosTransPayment_Status_InProgress;//2915
                    oRowPaid.Email = this.Email;//2915
                    oRowPaid.Mobile = this.Mobile;//2915
                    oRowPaid.TransactionProcessingMode = this.TransactionProcessingMode;//2915
                    oRowPaid.PaymentProcessor = "PRIMERXPAY";//2915
                    oRowPaid.TicketNumber = PccPaymentSvr.sCurrentTicket; //PRIMEPOS-3345
                }

                this.PrimeRxPayTransID = string.Empty;//2915

                if (PccPaymentSvr.DefaultInstance != null && PccPaymentSvr.DefaultInstance.EmvTags != null)
                {
                    oRowPaid.ReferenceNumber = oRowPaid.ProcessorTransID = PccPaymentSvr.DefaultInstance.EmvTags.ReferenceNumber;
                }
                oRowPaid.ControlNumber = !string.IsNullOrEmpty(this.controlNumber) ? this.controlNumber : string.Empty;
                //                        
                if (string.IsNullOrWhiteSpace(oRowPaid.ControlNumber))
                {
                    oRowPaid.ControlNumber = EvertecReceipt("IVUCASH");
                }
                try
                {
                    if (oGridRow.Cells["BinarySign"].Value.ToString() != "" && oGridRow.Cells["SigType"].Value.ToString() != "")
                    {
                        oRowPaid.BinarySign = (byte[])oGridRow.Cells["BinarySign"].Value;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "AddRowToPaid()");
                }
                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                {
                    ValidatePCCPayment(oRowPaid);
                }

                try
                {
                    if (oGridRow.Cells["BinarySign"].Value.ToString() != "" && oGridRow.Cells["SigType"].Value.ToString() != "")
                    {
                        oRowPaid.BinarySign = (byte[])oGridRow.Cells["BinarySign"].Value;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "AddRowToPaid()");
                }
                oRowPaid.TransFeeAmt = Configuration.convertNullToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt].GetText(MaskMode.Raw));  //PRIMEPOS-3117 11-Jul-2022 JY Added
                oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);
                bPaymentRowAdded = true;    //PRIMEPOS-2499 29-Mar-2018 JY Added
                                            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Payment Made of $" + oRowPaid.Amount + "By " + Configuration.convertNullToString(grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString()), clsPOSDBConstants.Log_Success);
                logger.Trace("AddRowToPaid() - Payment Made of $" + oRowPaid.Amount + "By " + Configuration.convertNullToString(grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString()) + " - " + clsPOSDBConstants.Log_Success);
                ClearGridRow(oGridRow);
                calculatePaidAmount();
                if (oRowPaid.TransTypeCode.Trim().Equals("E") == true)
                {
                    oGridRow.Activation = Activation.Disabled;
                }
            }
            catch (Exception ex)
            {
                //PRIMEPOS-2915
                logger.Error(ex.ToString());
                if (pccCardInfo != null)
                {
                    logger.Trace("The transaction is reverting now ");
                    if (this.ticketNum == null)
                    {
                        this.ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();
                    }
                    pccCardInfo.IsVoidCustomerDriven = true;
                    pccCardInfo.TransactionID = this.PrimeRxPayTransID;
                    if (oPosPTList.oTransactionType == POSTransactionType.Sales)
                    {
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo);
                    }
                    else if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                    {
                        PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo);
                    }
                    logger.Trace("The transaction is reverted now ");
                }
            }
        }

        private void AddRowToPaid(UltraGridRow oGridRow, String subCardType, String payTypeId)
        {
            try
            {
                int TransPayID = 1;
                logger.Trace("AddRowToPaid() - Payment Made of $" + Configuration.convertNullToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)).ToString() + "By " + Configuration.convertNullToString(grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString()) + " - " + clsPOSDBConstants.Log_Entering);
                foreach (POSTransPaymentRow oRow in oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.Rows)
                {
                    if (oRow.TransPayID >= TransPayID)
                    {
                        TransPayID = oRow.TransPayID + 1;
                    }
                }
                POSTransPaymentRow oRowPaid = (POSTransPaymentRow)oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.NewRow();
                oRowPaid.TransTypeDesc = subCardType;
                #region PRIMEPOS-2738 ADDED BY ARVIND
                if (Configuration.CSetting.StrictReturn == true && oPosPTList.oTransactionType == POSTransactionType.SalesReturn && !string.IsNullOrWhiteSpace(this.TransPayID))
                {
                    oRowPaid.TransPayID = Convert.ToInt32(this.TransPayID);
                    oRowPaid.TransTypeCode = TransTypeCode;//PRIMEPOS-3087
                }
                else
                {
                    if(IsNBSTransaction) //PRIMEPOS-3519 //PRIMEPOS-3504
                    {
                        oRowPaid.TransPayID = TransPayID;
                        oRowPaid.TransTypeCode = "N";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(paymentType) && paymentType.ToUpper().Equals("DEBIT")) //PRIMEPOS-3519 Need to verify
                        {
                            oRowPaid.TransPayID = TransPayID;
                            oRowPaid.TransTypeCode = "7";
                        }
                        else
                        {
                            oRowPaid.TransPayID = TransPayID;
                            oRowPaid.TransTypeCode = payTypeId;//PRIMEPOS-3087
                        }
                    }
                }
                #endregion
                oRowPaid.TransID = oPosPTList.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].TransID;
                oRowPaid.TransDate = DateTime.Now;
                oRowPaid.RefNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].GetText(MaskMode.Raw);
                oRowPaid.HC_Posted = oPosPTList.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].HC_Posted;
                oRowPaid.CustomerSign = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].GetText(MaskMode.Raw);
                oRowPaid.CCTransNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo].GetText(MaskMode.Raw);
                oRowPaid.CCName = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].GetText(MaskMode.Raw);
                oRowPaid.AuthNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].GetText(MaskMode.Raw);
                oRowPaid.Amount = Convert.ToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                oRowPaid.IsIIASPayment = Configuration.convertNullToBoolean(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value.ToString());
                //PRIMEPOS-3372-NBS-NEED-TO-ADD
                if(IsNBSTransaction) //PRIMEPOS-PaymentDesc //PRIMEPOS-3482 might will cause an issue  //PRIMEPOS-3519 //PRIMEPOS-3504
                {
                    oRowPaid.PaymentProcessor = "NB_VANTIV";  //PRIMEPOS-3482
                    oRowPaid.NBSTransId = Convert.ToString(NBSTransID); //PRIMEPOS-3375
                    oRowPaid.NBSTransUid = Convert.ToString(NBSUid); //PRIMEPOS-3375
                    oRowPaid.NBSPaymentType = Convert.ToString(NBSPaymentType); //PRIMEPOS-3375
                }
                else
                {
                    oRowPaid.PaymentProcessor = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].GetText(MaskMode.Raw);
                }
                if (!string.IsNullOrWhiteSpace(this.ExpiryDate) && this.ExpiryDate.Length>=4)
                {
                    string month = this.ExpiryDate.Substring(0, 2);
                    string year = this.ExpiryDate.Substring(2, 2);
                    string day = "01";

                    oRowPaid.ExpiryDate = DateTime.Parse(day + "/" + month + "/" + year);
                }
                //PRIMEPOS-2664 ADDED BY ARVIND
                //if (subCardType.Contains("EBT"))
                //{
                //    EvertecReceipt("EBT");
                //}
                //else
                //{
                //    EvertecReceipt("SALE");
                //}
                oRowPaid.ControlNumber = !string.IsNullOrEmpty(this.controlNumber) ? this.controlNumber : string.Empty;
                //

                if (payTypeId == "S")//  Added for Solutran - PRIMEPOS-2663 - NileshJ
                {
                    oRowPaid.ProcessorTransID = oPosPTList.S3TransID;
                    oRowPaid.S3TransID = oPosPTList.S3TransID;
                }
                else
                {
                    oRowPaid.ProcessorTransID = oPosPTList.oPOSTransPayment_CCLogList[(oPosPTList.oPOSTransPayment_CCLogList.Count) == 1 ? 0 : (oPosPTList.oPOSTransPayment_CCLogList.Count) - 1].TransID;
                }
                if (string.IsNullOrWhiteSpace(oRowPaid.ControlNumber))
                {
                    if (Convert.ToString(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)).Contains("-"))
                    {
                        if (subCardType.Contains("EBT"))
                        {
                            oRowPaid.ControlNumber = EvertecReceipt("EBT");
                        }
                        else
                        {
                            oRowPaid.ControlNumber = EvertecReceipt("REFUND");
                        }
                    }
                    else
                    {
                        if (subCardType.Contains("EBT"))
                        {
                            oRowPaid.ControlNumber = EvertecReceipt("EBT");
                        }
                        else
                        {
                            oRowPaid.ControlNumber = EvertecReceipt("SALE");
                        }
                    }
                }
                oRowPaid.IsManual = oPosPTList.oPOSTransPayment_CCLogList[0].IsManual;    //Sprint-19 - 2139 06-Jan-2015 JY Added
                oRowPaid.ProfiledID = !string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.ProfiledID) ? PccPaymentSvr.DefaultInstance.ProfiledID : string.Empty;
                oRowPaid.EntryMethod = !string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.EntryMethod) ? PccPaymentSvr.DefaultInstance.EntryMethod : string.Empty;
                oRowPaid.CashBack = !string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.CashBack) ? PccPaymentSvr.DefaultInstance.CashBack : string.Empty;
                ValidatePCCPayment(oRowPaid);
                try
                {
                    if ((this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value) != System.DBNull.Value)
                        oRowPaid.BinarySign = (byte[])oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_BinarySign].Value;

                    oRowPaid.SigType = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].GetText(MaskMode.Raw);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "AddRowToPaid()");
                }
                #region PRIMEPOS-3009
                if (this.tokenID != 0)
                {
                    oRowPaid.TokenID = this.tokenID;
                    this.tokenID = 0;
                }
                #endregion
                #region PRIMEPOS-2761
                //if (Configuration.CPOSSet.PaymentProcessor.ToUpper().Trim() == "HPSPAX" || Configuration.CPOSSet.PaymentProcessor.ToUpper().Trim() == "HPS" || Configuration.CPOSSet.PaymentProcessor.ToUpper().Trim() == "XLINK" || Configuration.CPOSSet.PaymentProcessor.ToUpper().Trim() == "XCHARGE")
                //{
                //oRowPaid.TicketNumber = PccPaymentSvr.GetProcessorInstance(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].GetText(MaskMode.Raw)).pccRespInfo.TrouTd;
                oRowPaid.TicketNumber = PccPaymentSvr.sCurrentTicket;
                //}
                //else if(Configuration.CPOSSet.PaymentProcessor.ToUpper().Trim() == "HPS")
                //{
                //    //oRowPaid.TicketNumber =  PccPaymentSvr.GetProcessorInstance(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].GetText(MaskMode.Raw)).resp.ticketNum;
                //}
                #endregion
                oRowPaid.TransFeeAmt = Configuration.convertNullToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt].GetText(MaskMode.Raw));  //PRIMEPOS-3117 11-Jul-2022 JY Added

                #region PRIMEPOS-2402 09-Jul-2021 JY Added
                if (oRowPaid.TransTypeCode.Trim().ToUpper() == "H" && OverrideHousechargeLimitUser != "")
                {
                    oRowPaid.OverrideHousechargeLimitUser = OverrideHousechargeLimitUser;
                    OverrideHousechargeLimitUser = "";
                }
                if (strMaxTenderedAmountOverrideUser != "")
                {
                    oRowPaid.MaxTenderedAmountOverrideUser = strMaxTenderedAmountOverrideUser;
                    strMaxTenderedAmountOverrideUser = "";
                }
                #endregion
                oRowPaid.Tokenize = Tokenize;   //PRIMEPOS-3145 28-Sep-2022 JY Added

                oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);
                PccPaymentSvr.DefaultInstance.EmvTags = null;
                PccPaymentSvr.DefaultInstance.ProfiledID = string.Empty;
                PccPaymentSvr.DefaultInstance.CashBack = string.Empty;
                PccPaymentSvr.DefaultInstance.EntryMethod = string.Empty;
                SigPadUtil.DefaultInstance.CashBack = string.Empty;
                logger.Trace("AddRowToPaid() - Payment Made of $" + oRowPaid.Amount + "By " + Configuration.convertNullToString(grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value).ToString());
                ClearGridRow(oGridRow);
                calculatePaidAmount();
                if (oRowPaid.TransTypeCode.Trim().Equals("E") == true)
                {
                    oGridRow.Activation = Activation.Disabled;
                }
                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC" && this.CashBackAmount != 0)//2664
                {
                    var cashbackRow = oPosPTList.oPOSTransPaymentDataPaid.Tables[0].AsEnumerable().Where(a => a.Field<string>("PayTypeDesc") == "Cash Back").FirstOrDefault();

                    cashbackRow["Amount"] = cashbackRow["CashBack"] = "-" + this.CashBackAmount.ToString();
                }
            }
            #region PRIMEPOS-2887 
            catch (Exception ex)
            {
                try
                {
                    logger.Error("AddRowToPaid()", ex.Message);
                    if (payTypeId == "S")//  Added for Solutran  NileshJ
                    {
                        this.RemoveSolutranPayment(oPosPTList.S3TransID);
                        Resources.Message.Display("Transaction is Voided because of some failure transaction", "Solutran Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        IsSolutranException = true;
                    }
                    else
                    {
                        using (var db = new Possql())
                        {
                            CCTransmission_Log cclog = new CCTransmission_Log();
                            //cclog = db.CCTransmission_Logs.Where(w => w.HostTransID == TransNo).SingleOrDefault();

                            if (pccCardInfo.PaymentProcessor == "HPSPAX") // Added for HPSPAX NileshJ
                            {
                                cclog = db.CCTransmission_Logs.OrderByDescending(r => r.TransNo).Where(w => w.TicketNo == pccCardInfo.TicketNo).Take(1).SingleOrDefault();
                            }
                            else if (pccCardInfo.PaymentProcessor == "WORLDPAY")
                            {
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == pccCardInfo.TicketNo).FirstOrDefault();
                            }
                            else
                            {
                                cclog = db.CCTransmission_Logs.Where(w => w.HostTransID == TransNo).SingleOrDefault();
                            }
                            cclog.ResponseMessage = "Reverting transaction";
                            db.CCTransmission_Logs.Attach(cclog);
                            db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                            db.SaveChanges();
                            logger.Trace(" The transaction is reverting due to some issue in AddRowToPaid() method");
                        }

                        //PrimePOS-3144
                        if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.XLINK)
                            SigPadUtil.DefaultInstance.XlinkOnOff("ON");

                        if (this.ticketNum == null)
                        {
                            this.ticketNum = Configuration.StationID + clsUIHelper.GetRandomNo().ToString();
                        }
                        if (oPosPTList.oTransactionType == POSTransactionType.Sales)
                        {
                            PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo);
                        }
                        else if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
                        {
                            PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo);
                        }

                        //PrimePOS-3144
                        if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.XLINK)
                            SigPadUtil.DefaultInstance.XlinkOnOff("OFF");
                        string statusResp = string.Empty;
                        string descResp = string.Empty;
                        bool bProcSuccess = true;
                        PccResponse objResp = PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).pccRespInfo;
                        if (objResp != null)
                        {
                            statusResp = objResp.ResponseStatus;
                            descResp = objResp.ResultDescription;
                            if (!string.IsNullOrWhiteSpace(statusResp) && statusResp.Trim().Equals("FAILURE", StringComparison.OrdinalIgnoreCase))
                                bProcSuccess = false;
                        }
                        else
                            bProcSuccess = false;

                        using (var db = new Possql())
                        {
                            CCTransmission_Log cclog = new CCTransmission_Log();
                            //cclog = db.CCTransmission_Logs.Where(w => w.HostTransID == TransNo).SingleOrDefault();
                            if (pccCardInfo.PaymentProcessor == "HPSPAX") // Added for HPSPAX NileshJ
                            {
                                cclog = db.CCTransmission_Logs.OrderByDescending(r => r.TransNo).Where(w => w.TicketNo == pccCardInfo.TicketNo).Take(1).SingleOrDefault();
                            }
                            else if (pccCardInfo.PaymentProcessor == "WORLDPAY")
                            {
                                cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == pccCardInfo.TicketNo).FirstOrDefault();
                            }
                            else
                            {
                                cclog = db.CCTransmission_Logs.Where(w => w.HostTransID == TransNo).FirstOrDefault();  //PrimePOS-3144
                            }
                            cclog.ResponseMessage = "Transaction successfully reverted";
                            if (!bProcSuccess)
                                cclog.ResponseMessage = "Failed to revert Transaction";
                            db.CCTransmission_Logs.Attach(cclog);
                            db.Entry(cclog).Property(p => p.ResponseMessage).IsModified = true;
                            db.SaveChanges();
                        }

                        if (!bProcSuccess)
                        {
                            clsUIHelper.ShowSuccessMsg("Try to revert Transaction due to some issue but failed.\nTransaction is not saved. Please call MicroMerchant !", "Failed to revert Transaction");
                            logger.Trace(" Try to revert transaction due to some issue in AddRowToPaid() method but failed.\n" + descResp);
                        }
                        else
                        {
                            clsUIHelper.ShowSuccessMsg("The Transaction is reverted due to some issue \n please try again", "Transaction Reverted");
                            logger.Trace(" The transaction is reverted due to some issue in AddRowToPaid() method");
                        }
                    }
                }
                catch (Exception EX)
                {
                    logger.Trace("CATCH BLOCK ADDROWTOPAID ", EX.Message);
                    clsUIHelper.ShowErrorMsg(EX.Message);
                }
            }
            #endregion            
        }

        private void ValidatePCCPayment(POSTransPaymentRow oRowPaid)
        {
            if (PccPaymentSvr.DefaultInstance.EmvTags != null && !string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.EmvTags.AppIndentifer))
            {
                oRowPaid.Aid = PccPaymentSvr.DefaultInstance.EmvTags.AppIndentifer;
                oRowPaid.AidName = PccPaymentSvr.DefaultInstance.EmvTags.AppPreferedName;
                oRowPaid.Cryptogram = PccPaymentSvr.DefaultInstance.EmvTags.AppCrytogram;
                oRowPaid.TransCounter = PccPaymentSvr.DefaultInstance.EmvTags.AppTransactionCounter;
                oRowPaid.TerminalTvr = PccPaymentSvr.DefaultInstance.EmvTags.TerminalVerficationResult;
                oRowPaid.TransStatusInfo = PccPaymentSvr.DefaultInstance.EmvTags.TransStatusInformation;
                oRowPaid.AuthorizeRespCode = PccPaymentSvr.DefaultInstance.EmvTags.AuthorizationResposeCode;
                oRowPaid.TransRefNum = PccPaymentSvr.DefaultInstance.EmvTags.TransRefNumber;
                oRowPaid.ValidateCode = PccPaymentSvr.DefaultInstance.EmvTags.ValidationCode;
                oRowPaid.MerchantID = PccPaymentSvr.DefaultInstance.EmvTags.MerchantID;
                oRowPaid.EntryLegend = PccPaymentSvr.DefaultInstance.EmvTags.EntryLegend;
                oRowPaid.CardType = PccPaymentSvr.DefaultInstance.EmvTags.AccountType;
                oRowPaid.ProcTransType = PccPaymentSvr.DefaultInstance.EmvTags.TransType;
                oRowPaid.Verbiage = PccPaymentSvr.DefaultInstance.EmvTags.Verbiage;
                oRowPaid.RTransID = PccPaymentSvr.DefaultInstance.EmvTags.TransID;
                //Added by Arvind PRIMEPOS-2664
                oRowPaid.BatchNumber = PccPaymentSvr.DefaultInstance.EmvTags.BatchNumber;
                oRowPaid.TraceNumber = PccPaymentSvr.DefaultInstance.EmvTags.TraceNumber;
                oRowPaid.InvoiceNumber = PccPaymentSvr.DefaultInstance.EmvTags.InvoiceNumber;
                oRowPaid.EbtBalance = PccPaymentSvr.DefaultInstance.EmvTags.EbtBalance;
                //
                //PRIMEPOS-2636
                if (Configuration.CPOSSet.PaymentProcessor == "VANTIV")
                {
                    oRowPaid.ReferenceNumber = PccPaymentSvr.DefaultInstance.EmvTags.ReferenceNumber;
                    oRowPaid.EntryMethod = PccPaymentSvr.DefaultInstance.EmvTags.EntryLegend;
                    oRowPaid.TransactionID = PccPaymentSvr.DefaultInstance.EmvTags.TransactionID;
                    oRowPaid.ApprovalCode = PccPaymentSvr.DefaultInstance.EmvTags.ApprovalCode;
                    oRowPaid.ResponseCode = PccPaymentSvr.DefaultInstance.EmvTags.ResponseCode;
                    oRowPaid.LaneID = PccPaymentSvr.DefaultInstance.EmvTags.LaneID;
                    oRowPaid.CardLogo = PccPaymentSvr.DefaultInstance.EmvTags.CardLogo;
                    oRowPaid.PinVerified = PccPaymentSvr.DefaultInstance.EmvTags.PinVerified;
                    oRowPaid.ApplicaionLabel = PccPaymentSvr.DefaultInstance.EmvTags.ApplicationLabel;
                    oRowPaid.TerminalID = PccPaymentSvr.DefaultInstance.EmvTags.TerminalID;
                }
                oRowPaid.IsEvertecForceTransaction = PccPaymentSvr.DefaultInstance.EmvTags.IsEvertecForceTransaction;//PRIMEPOS-2831
                oRowPaid.IsEvertecSign = PccPaymentSvr.DefaultInstance.EmvTags.IsEvertecSign;//PRIMEPOS-2831
                //
                oRowPaid.EvertecTaxBreakdown = PccPaymentSvr.DefaultInstance.EmvTags.EvertecTaxBreakdown;//PRIMEPOS-2857
                oRowPaid.ControlNumber = PccPaymentSvr.DefaultInstance.EmvTags.ControlNumber;//PRIMEPOS-2857
                oRowPaid.ATHMovil = PccPaymentSvr.DefaultInstance.EmvTags.ATHMovil;//PRIMEPOS-2857
                oRowPaid.EntryMethod = PccPaymentSvr.DefaultInstance.EntryMethod;

                if (Configuration.CPOSSet.PaymentProcessor == "ELAVON")//2943
                {
                    oRowPaid.ReferenceNumber = PccPaymentSvr.DefaultInstance.EmvTags.ReferenceNumber;
                    oRowPaid.MerchantID = PccPaymentSvr.DefaultInstance.EmvTags.MerchantID;
                    oRowPaid.ApprovalCode = PccPaymentSvr.DefaultInstance.EmvTags.ApprovalCode;
                    oRowPaid.ApplicaionLabel = PccPaymentSvr.DefaultInstance.EmvTags.ApplicationLabel;
                    oRowPaid.CCName = PccPaymentSvr.DefaultInstance.EmvTags.CcName;
                    oRowPaid.TransTypeDesc = PccPaymentSvr.SetActualCardType(PccPaymentSvr.DefaultInstance.EmvTags.CardLogo);
                    //if (!oRowPaid.IsIIASPayment)
                    //{
                    //    if (PccPaymentSvr.DefaultInstance.WP_TransResult.IsFSA)
                    //    {
                    //        oRowPaid.IsIIASPayment = true;
                    //    }
                    //}
                }
                if (Configuration.CPOSSet.PaymentProcessor == "HPSPAX")//2990
                {
                    oRowPaid.IsFsaCard = PccPaymentSvr.DefaultInstance.EmvTags.IsFsaCard;
                }
            }
            else if (PccPaymentSvr.DefaultInstance.WP_TransResult != null)
            {
                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.TerminalID))
                {
                    oRowPaid.TransRefNum = PccPaymentSvr.DefaultInstance.WP_TransResult.TerminalID;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.MerchantID))
                {
                    oRowPaid.MerchantID = PccPaymentSvr.DefaultInstance.WP_TransResult.MerchantID;
                }
                else if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.MerchantID))
                {
                    oRowPaid.MerchantID = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.MerchantID;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AccountType))
                {
                    oRowPaid.CardType = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AccountType;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TransactionType))
                {
                    oRowPaid.ProcTransType = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TransactionType;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TransactionResult))
                {
                    oRowPaid.Verbiage = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TransactionResult;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.Name))
                {
                    oRowPaid.CCName = oRowPaid.CCName + "|" + PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.Name;
                }
                if (PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.CashBack > 0)
                {
                    oRowPaid.CashBack = string.Format("{0:F}", PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.CashBack);
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AppName))
                {
                    oRowPaid.AidName = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AppName;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AppID))
                {
                    oRowPaid.Aid = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AppID;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TermVerifResult))
                {
                    oRowPaid.TerminalTvr = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TermVerifResult;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.IssuerAppData))
                {
                    oRowPaid.IssuerAppData = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.IssuerAppData;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TransStatusIndicator))
                {
                    oRowPaid.TransStatusInfo = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.TransStatusIndicator;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AppCryptoGram))
                {
                    oRowPaid.Cryptogram = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AppCryptoGram;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AuthRespCode))
                {
                    oRowPaid.AuthorizeRespCode = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.AuthRespCode;
                }

                if (!string.IsNullOrEmpty(PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.CardVerifMethod))
                {
                    oRowPaid.CardVerificationMethod = PccPaymentSvr.DefaultInstance.WP_TransResult.ResponseTags.CardVerifMethod;
                }

                if (!oRowPaid.IsIIASPayment)
                {
                    if (PccPaymentSvr.DefaultInstance.WP_TransResult.IsFSA)
                    {
                        oRowPaid.IsIIASPayment = true;
                    }
                }

            }
            //Added by Arvind  PRIMEPOS-2664
            else if (PccPaymentSvr.DefaultInstance.EmvTags != null && Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
            {
                oRowPaid.MerchantID = PccPaymentSvr.DefaultInstance.EmvTags.MerchantID?.Trim();
                oRowPaid.BatchNumber = PccPaymentSvr.DefaultInstance.EmvTags.BatchNumber;
                oRowPaid.TraceNumber = PccPaymentSvr.DefaultInstance.EmvTags.TraceNumber;
                oRowPaid.InvoiceNumber = PccPaymentSvr.DefaultInstance.EmvTags.InvoiceNumber;
                oRowPaid.EbtBalance = PccPaymentSvr.DefaultInstance.EmvTags.EbtBalance;
                oRowPaid.IsEvertecForceTransaction = PccPaymentSvr.DefaultInstance.EmvTags.IsEvertecForceTransaction;//PRIMEPOS-2831
                oRowPaid.IsEvertecSign = PccPaymentSvr.DefaultInstance.EmvTags.IsEvertecSign;//PRIMEPOS-2831
                oRowPaid.EvertecTaxBreakdown = PccPaymentSvr.DefaultInstance.EmvTags.EvertecTaxBreakdown;//PRIMEPOS-2857
                oRowPaid.ControlNumber = PccPaymentSvr.DefaultInstance.EmvTags.ControlNumber;//PRIMEPOS-2857
                oRowPaid.ATHMovil = PccPaymentSvr.DefaultInstance.EmvTags.ATHMovil;//PRIMEPOS-2857
                oRowPaid.EntryLegend = PccPaymentSvr.DefaultInstance.EntryMethod;
                oRowPaid.ReferenceNumber = PccPaymentSvr.DefaultInstance.EmvTags.ReferenceNumber;
            }
            //
            //Added by Arvind PRIMEPOS-2636 
            else if (PccPaymentSvr.DefaultInstance?.EmvTags != null && Configuration.CPOSSet.PaymentProcessor == "VANTIV")
            {
                oRowPaid.ReferenceNumber = PccPaymentSvr.DefaultInstance.EmvTags.ReferenceNumber;
                oRowPaid.EntryMethod = PccPaymentSvr.DefaultInstance.EmvTags.EntryLegend;
                oRowPaid.TransTypeDesc = PccPaymentSvr.SetActualCardType(PccPaymentSvr.DefaultInstance.EmvTags.AccountType);//PRIMEPOS-2636 VANTIV //PRIMEPOS-2896
                oRowPaid.ResponseCode = PccPaymentSvr.DefaultInstance.EmvTags.ResponseCode;
                oRowPaid.ApprovalCode = PccPaymentSvr.DefaultInstance.EmvTags.ApprovalCode;
                #region PRIMEPOS-2793 VANTIV
                oRowPaid.ApplicaionLabel = PccPaymentSvr.DefaultInstance.EmvTags.ApplicationLabel;
                oRowPaid.PinVerified = PccPaymentSvr.DefaultInstance.EmvTags.PinVerified;
                oRowPaid.LaneID = PccPaymentSvr.DefaultInstance.EmvTags.LaneID;
                oRowPaid.CardLogo = PccPaymentSvr.DefaultInstance.EmvTags.CardLogo;
                oRowPaid.TerminalID = PccPaymentSvr.DefaultInstance.EmvTags.TerminalID;
                oRowPaid.TransactionID = PccPaymentSvr.DefaultInstance.EmvTags.TransactionID;
                oRowPaid.IsFsaCard = PccPaymentSvr.DefaultInstance.EmvTags.IsFsaCard; //PRIMEPOS-3545
                #endregion
            }
            else if (PccPaymentSvr.DefaultInstance?.EmvTags != null && Configuration.CPOSSet.PaymentProcessor == "ELAVON")//2943
            {
                oRowPaid.ReferenceNumber = PccPaymentSvr.DefaultInstance.EmvTags.ReferenceNumber;
                oRowPaid.MerchantID = PccPaymentSvr.DefaultInstance.EmvTags.MerchantID;
                oRowPaid.ApprovalCode = PccPaymentSvr.DefaultInstance.EmvTags.ApprovalCode;
                oRowPaid.CCName = PccPaymentSvr.DefaultInstance.EmvTags.CcName;
                oRowPaid.TransTypeDesc = PccPaymentSvr.SetActualCardType(PccPaymentSvr.DefaultInstance.EmvTags.CardLogo);
                //if (!oRowPaid.IsIIASPayment)
                //{
                //    if (PccPaymentSvr.DefaultInstance.WP_TransResult.IsFSA)
                //    {
                //        oRowPaid.IsIIASPayment = true;
                //    }
                //}
            }
            if (PccPaymentSvr.DefaultInstance?.EmvTags != null && Configuration.CPOSSet.PaymentProcessor == "HPSPAX")//2990
            {
                oRowPaid.IsFsaCard = PccPaymentSvr.DefaultInstance.EmvTags.IsFsaCard;
            }
            //ADDED FOR TRANSREFNUMBER - HREF NUMBER FOR HPSPAX and HPS PRIMEPOS-2738 
            if (!String.IsNullOrWhiteSpace(PccPaymentSvr.DefaultInstance?.EmvTags?.TransRefNumber))
            {
                oRowPaid.TransRefNum = PccPaymentSvr.DefaultInstance?.EmvTags?.TransRefNumber;
            }
            //
            if (PccPaymentSvr.DefaultInstance.EmvTags != null && string.IsNullOrWhiteSpace(oRowPaid.TransTypeDesc))
            {
                oRowPaid.TransTypeDesc = PccPaymentSvr.SetActualCardType(PccPaymentSvr.DefaultInstance.EmvTags.AccountType);
            }
        }

        #endregion AddRowToPaid

        #region AddZeroAmount 
        public void AddZeroAmount(UltraGridRow oGridRow)
        {
            AddRowToPaid(oGridRow);
        }

        #endregion AddZeroAmount

        #region AddAutoCLDiscount
        private void AddAutoCLDiscount()
        {
            CLCoupons oCLCoupons = new CLCoupons();
            CLCouponsData oCLCouponsData = oCLCoupons.GetUnUsedCLCoupons(oPosPTList.oCLCardRow.CLCardID);
            if (oCLCouponsData.CLCoupons.Rows.Count > 0)
            {
                decimal paidAmount = oPosPTList.amountBalanceDue;
                oPosPTList.FetchAutoCLDiscount(oCLCoupons, oCLCouponsData, ref paidAmount);
                calculatePaidAmount();
                if (grdPayment.ActiveRow == null)
                {
                    grdPayment.ActiveRow = grdPayment.Rows[0];
                }
                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = oPosPTList.amountBalanceDue;    //Sprint-25 - PRIMEPOS-2297 19-Apr-2017 JY Added logic to reset the amount which was wrong in case of customer loyalty "Auto redeem method"
                grdPayment.Refresh();   //Sprint-25 - PRIMEPOS-2297 19-Apr-2017 JY Added
                if (Configuration.CLoyaltyInfo.ShowDiscountAppliedMsg)
                {
                    clsUIHelper.ShowSuccessMsg("Congratulation! $" + paidAmount.ToString() + " discount is being applied on this transaction", "Customer Loyalty");
                }
            }
        }

        #endregion AddAutoCLDiscount

        #region RemovePayment
        private void btnRemovePayment_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnRemovePayment_Click() - " + clsPOSDBConstants.Log_Entering);
                if (grdPaymentComplete.Selected.Rows.Count > 0)
                {
                    if (Resources.Message.Display("This action will remove selected payment from list.\n Are you sure to continue? ", "Payment Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                    if (Configuration.CPOSSet.PreferReverse && oPosPTList.sTransactionType == POS_Core.TransType.POSTransactionType.Sales.ToString())
                    {
                        RemovePayment(grdPaymentComplete.Selected.Rows[0]);
                    }
                    else
                    {
                        if (Configuration.CPOSSet.PreferReverse && oPosPTList.sTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn.ToString())
                        {
                            Resources.Message.Display("Transaction is Voided", "Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        }
                        RemovePayment(grdPaymentComplete.Selected.Rows[0]);
                    }
                    this.grdPaymentComplete.Update();
                    calculatePaidAmount();
                    this.grdPayment.Focus();

                    if (grdPaymentComplete.Rows.Count < 1)
                    {
                        btnRemovePayment.Visible = false;
                    }
                }
                else
                {
                    if (grdPaymentComplete.Rows.Count > 0)
                    {
                        Resources.Message.Display("Please select payment from list. ", "Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    }
                }
                logger.Trace("btnRemovePayment_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "btnRemovePayment_Click()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private bool RemovePayment(UltraGridRow oRow)
        {
            bool retVal = false;
            string tempTransPayId = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransPayID].Value); //PRIMEPOS-3245
            string strPaymentType = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim().ToUpper();
            string strAmount = Configuration.convertNullToDecimal(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value).ToString("######0.00");
            logger.Trace("RemovePayment() - Payment Remove of $" + strAmount + "By " + strPaymentType + " - " + clsPOSDBConstants.Log_Entering);

            #region  PRIMEPOS-2738 ADDED BY ARVIND 
            if (Configuration.CSetting.StrictReturn == true && oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
            {
                var removePay = oOrigPayTransData.Tables[0].AsEnumerable().Where(a => a.Field<int>("TransPayID") == Convert.ToInt32(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransPayID].Value));

                foreach (var removeReverseAmount in removePay)
                {
                    removeReverseAmount.SetField<decimal>("ReversedAmount", removeReverseAmount.Field<decimal>("Amount") - removeReverseAmount.Field<decimal>("ReversedAmount"));
                }
            }
            #endregion
            #region  PRIMEPOS-2841
            if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].Value.ToString().Trim().ToUpper() == "PRIMERXPAY")
            {
                this.RemoveOnlinePayment(oRow);
            }
            #endregion
            switch (strPaymentType)
            {
                case "1":
                case "2":
                case "C":
                case "H": //added by Manoj 1/27/2014
                    oRow.Delete(false);
                    this.grdPaymentComplete.UpdateData();
                    break;
                case "B":
                    oRow.Delete(false);
                    RemoveCashBackCharge();
                    this.grdPaymentComplete.UpdateData();
                    break;
                //case "H":  //Comment out because it freeze the POS. by Manoj
                //    break;
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "-99":
                case "E":
                case "N": //PRIMEPOS-3504
                    this.RemoveEBTManually(oRow);
                    this.SelectEBTPayType();
                    break;
                #region  Added for Solutran to void transaction - PRIMEPOS-2663 - NileshJ
                case "S":
                    if (this.RemoveSolutranPayment(s3TransID) == S3_Success)
                    {
                        oRow.Delete(false);
                        this.grdPaymentComplete.UpdateData();
                    }
                    break;
                #endregion
                #region PRIMEPOS-2938 27-Jan-2021 JY Added
                case "X":
                    try
                    {
                        oStoreCreditData = null;
                        posTrans.oStoreCreditData = null;
                        oStoreCreditDetailsData = null;
                        IsStoreCredit = false;
                        oRow.Delete(false);
                        this.grdPaymentComplete.UpdateData();

                        foreach (UltraGridRow row in grdPayment.Rows)
                        {
                            switch (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim())
                            {
                                case "X":
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.NoEdit;
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                                    //row.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_CardType].Activation = Activation.NoEdit;
                                    row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Activation = Activation.NoEdit;
                                    row.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Activation = Activation.NoEdit;
                                    break;
                            }
                        }


                    }
                    catch { }
                    break;
                #endregion
                #region Sprint-22 19-Oct-2015 JY Added logic to remove the custom paytype payment records 
                default:
                    oRow.Delete(false);
                    this.grdPaymentComplete.UpdateData();
                    break;
                    #endregion
            }
            retVal = true;

            #region PRIMEPOS-3345
            if (isTransOnHold)
            {
                POSTransPaymentSvr oPosTransPaymentSvr = new POSTransPaymentSvr();
                if (!string.IsNullOrEmpty(tempTransPayId))
                {
                    oPosTransPaymentSvr.DeletePrimeRxPayCanceledTrans(tempTransPayId);
                }
            }
            #endregion

            //POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Payment Remove of $" + strAmount + "By " + strPaymentType, clsPOSDBConstants.Log_Success);
            logger.Trace("RemovePayment() - Payment Remove of $" + strAmount + "By " + strPaymentType + " - " + clsPOSDBConstants.Log_Success);
            calculatePaidAmount();
            return retVal;
        }

        private void RemoveEBTManually(UltraGridRow oRow)
        {
            //Below if-else is added by shitaljit on 1/6/2014 to allow user to remove EBT payment if they are done manually.
            if (oPosPTList.oPOSTransPayment_CCLogList.Count > 0)
            {
                foreach (POSTransPayment_CCLog oLog in oPosPTList.oPOSTransPayment_CCLogList)
                {
                    //Modified By Dharmendra SRT on Mar-10-09 included formatting in Amount.ToString()
                    if (oLog.Amount.ToString("###########0.00") == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text &&
                        oLog.RefNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Text &&
                        oLog.AuthNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Text)
                    {
                        frmPOSProcessCC oFrmPOSProcessCC = new frmPOSProcessCC(oPosPTList.oTransactionType, "");
                        oLog.PccCardInfo.transAmount = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text.ToString(); //Added by Manoj 7/2/2012
                        oLog.PccCardInfo.nbsSaleType = Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType].Text); //PRIMEPOS-3373
                        #region PRIMEPOS-3189
                        if (oLog.PayTypeCode == "3" || oLog.PayTypeCode == "4" || oLog.PayTypeCode == "5"
                            || oLog.PayTypeCode == "6" || oLog.PayTypeCode == "-99")
                        {
                            oLog.PccCardInfo.cardType = "CC";
                        }
                        else if (oLog.PayTypeCode == "7")
                        {
                            oLog.PccCardInfo.cardType = "DB";
                        }
                        else if (oLog.PayTypeCode == "E")
                        {
                            oLog.PccCardInfo.cardType = "BT";
                        }
                        else if (oLog.PayTypeCode == "N") //PRIMEPOS-3504-DOUBT
                        {
                            oLog.PccCardInfo.cardType = "NB";
                        }
                        #endregion

                        if (Configuration.CPOSSet.PaymentProcessor == clsPOSDBConstants.ELAVON && !string.IsNullOrWhiteSpace(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_ExpDate].Text))//2943
                        {
                            oLog.PccCardInfo.cardExpiryDate = Convert.ToDateTime(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_ExpDate].Text).ToString("ddyy");
                        }
                        if (oLog.PccCardInfo == null)
                        {
                            oRow.Delete(false);
                            this.grdPaymentComplete.UpdateData();
                            oPosPTList.oPOSTransPayment_CCLogList.Remove(oLog);
                        }
                        else if (oFrmPOSProcessCC.PerformVoidTransaction(oLog.PccCardInfo, oPosPTList.oTransactionType, Convert.ToString(oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_NBSTransId].Text)) == true)
                        {
                            oRow.Delete(false);
                            this.grdPaymentComplete.UpdateData();
                            oPosPTList.oPOSTransPayment_CCLogList.Remove(oLog);
                        }

                        break;
                    }
                }
            }
            else
            {
                oRow.Delete(false);
                this.grdPaymentComplete.UpdateData();
            }
        }

        private void SelectEBTPayType()
        {
            //Added by Shitaljit to allow user to select EBT paytyoes
            for (int i = 0; i < grdPayment.Rows.Count; i++)
            {
                switch (this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim())
                {
                    case "E":
                        this.grdPayment.Rows[i].Activation = Activation.AllowEdit;
                        break;
                }
            }

        }

        #endregion RemovePayment

        #region btnCancel_click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelTrans();
        }

        #endregion btnCancel_Click

        #region chkIsDelivery_CheckedChanged
        private void chkIsDelivery_CheckedChanged(object sender, EventArgs e)
        {
            oPosPTList.bIsDelivery = this.chkIsDelivery.Checked;
        }

        #endregion chkIsDelivery_CheckedChanged

        #region btnCashBack_Click
        private void btnCashBack_Click(object sender, EventArgs e)
        {
            try
            {
                #region PRIMEPOS-2860 12-Jun-2020
                if (Configuration.convertNullToInt(oPosTrans.oTransHRow.ReturnTransID) > 0)
                {
                    logger.Trace("btnCashBack_Click() - For Sales Payment History - " + clsPOSDBConstants.Log_Entering);
                    frmSalesPaymentHistory ofrmSalesPaymentHistory = new frmSalesPaymentHistory(Configuration.convertNullToInt(oPosTrans.oTransHRow.ReturnTransID));
                    ofrmSalesPaymentHistory.ShowDialog();
                    logger.Trace("btnCashBack_Click() - For Sales Payment History - " + clsPOSDBConstants.Log_Exiting);
                }
                #endregion
                else
                {
                    logger.Trace("btnCashBack_Click() - " + clsPOSDBConstants.Log_Entering);
                    if (btnCashBack.Visible == false)
                    {
                        return;
                    }

                    if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                    {
                        {
                            frmPOSCashBack ofrm = new frmPOSCashBack();
                            if (ofrm.ShowDialog(this) == DialogResult.OK)
                            {
                                foreach (UltraGridRow oRow in this.grdPaymentComplete.Rows)
                                {
                                    if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "B") //if code is cashback
                                    {
                                        RemovePayment(oRow);
                                        break;
                                    }
                                }
                                decimal cashBackAmount = Configuration.convertNullToDecimal(ofrm.numDiscAmount.Value);
                                //IfGreaterCashBackAmount(cashBackAmount);
                                foreach (UltraGridRow oRow in grdPayment.Rows)
                                {
                                    SetRowAppearance(oRow);
                                }

                                if (cashBackAmount > 0)
                                {

                                    foreach (UltraGridRow oRow in this.grdPayment.Rows)
                                    {
                                        if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "B")
                                        {
                                            ParentForm.ChargeCashBack(cashBackAmount, ref oPosPTList.totalAmount);
                                            this.txtAmtTotal.Text = oPosPTList.totalAmount.ToString("###########0.00");  //Data Bindin??
                                            this.txtAmtTotalNonIIAS.Text = (oPosPTList.totalAmount - oPosPTList.totalIIASAmount).ToString("###########0.00");
                                            //calculatePaidAmount();                                            
                                            for (int i = 0; i < grdPayment.Rows.Count; i++)
                                            {
                                                switch (this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim())
                                                {
                                                    case "E":
                                                    case "7":
                                                        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
                                                        break;
                                                    default:
                                                        this.grdPayment.Rows[i].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                                                        break;
                                                }
                                            }
                                            AddRowToPaid(oRow);
                                            CashBackAmount = cashBackAmount;
                                            this.txtAmtCashBack.Text = cashBackAmount.ToString("##########0.00");
                                            break;
                                        }
                                    }
                                    SigPadUtil.DefaultInstance.CashBack = cashBackAmount.ToString();
                                }
                                this.grdPaymentActiveNullCB();
                                this.grdPaymentAmountActivationDisabledCBEvertec();

                                //grdPayment.Refresh();
                            }
                        }
                    }

                    //oPosPTList.CheckUserPriviligesCashBack(); //PRIMEPOS-2741 25-Sep-2019 JY Commented
                    else if (oPosPTList.CheckUserPriviligesCashBack())   //PRIMEPOS-2741 25-Sep-2019 JY Added
                    {
                        this.ComputeCashBack();
                        grdPayment.Focus();
                    }
                    logger.Trace("btnCashBack_Click() - " + clsPOSDBConstants.Log_Exiting);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //PRIMEPOS-2857
        private void grdPaymentAmountActivationDisabledCBEvertec()
        {
            if (grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation == Activation.Disabled)
            {
                foreach (UltraGridRow oRow in grdPayment.Rows)
                {
                    grdPayment.ActiveRow = oRow;
                    if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation == Activation.NoEdit)
                    {
                        grdPayment.ActiveCell = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                        grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                        break;
                    }
                }
            }
        }

        private void ComputeCashBack()
        {
            frmPOSCashBack ofrm = new frmPOSCashBack();
            if (ofrm.ShowDialog(this) == DialogResult.OK)
            {
                foreach (UltraGridRow oRow in this.grdPaymentComplete.Rows)
                {
                    if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim() == "B") //if code is cashback
                    {
                        RemovePayment(oRow);
                        break;
                    }
                }
                decimal cashBackAmount = Configuration.convertNullToDecimal(ofrm.numDiscAmount.Value);
                IfGreaterCashBackAmount(cashBackAmount);
                foreach (UltraGridRow oRow in grdPayment.Rows)
                {
                    SetRowAppearance(oRow);
                }
                this.grdPaymentActiveNullCB();

                grdPayment_Enter(this.grdPayment, new EventArgs());

                grdPayment_AfterRowActivate(this.grdPayment, new EventArgs());

                this.grdPaymentAmountActivationDisabledCB();

                grdPayment.Refresh();
            }
        }

        private void IfGreaterCashBackAmount(decimal cashBackAmount)
        {
            if (cashBackAmount > 0)
            {
                foreach (UltraGridRow oRow in this.grdPayment.Rows)
                {
                    if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "B")
                    {
                        ParentForm.ChargeCashBack(cashBackAmount, ref oPosPTList.totalAmount);
                        this.txtAmtTotal.Text = oPosPTList.totalAmount.ToString("###########0.00");  //Data Bindin??
                        this.txtAmtTotalNonIIAS.Text = (oPosPTList.totalAmount - oPosPTList.totalIIASAmount).ToString("###########0.00");
                        calculatePaidAmount();
                        WriteTotalToPoleDisplay();
                        oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = cashBackAmount * -1;
                        AddRowToPaid(oRow);
                        break;
                    }
                }
                SigPadUtil.DefaultInstance.CashBack = cashBackAmount.ToString();
            }
        }

        private void grdPaymentActiveNullCB()
        {
            if (grdPayment.ActiveRow == null)
            {
                grdPayment.ActiveRow = grdPayment.Rows[0];
            }
        }

        private void grdPaymentAmountActivationDisabledCB()
        {
            if (grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation == Activation.Disabled)
            {
                foreach (UltraGridRow oRow in grdPayment.Rows)
                {
                    grdPayment.ActiveRow = oRow;
                    if (oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation == Activation.AllowEdit)
                    {
                        grdPayment.ActiveCell = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount];
                        grdPayment.PerformAction(UltraGridAction.EnterEditMode);
                        break;
                    }
                }
            }
        }

        #endregion btnCashBack_Click

        #region RemoveCashBackCharge
        private void RemoveCashBackCharge()
        {
            ParentForm.RemoveCashBack(ref oPosPTList.totalAmount);
            SigPadUtil.DefaultInstance.CashBack = string.Empty;
            this.txtAmtTotal.Text = oPosPTList.totalAmount.ToString("###########0.00");
            this.txtAmtTotalNonIIAS.Text = (oPosPTList.totalAmount - oPosPTList.totalIIASAmount).ToString("###########0.00");
            calculatePaidAmount();
            WriteTotalToPoleDisplay();
        }

        #endregion RemoveCashBackCharge

        #region btnReverse_Click
        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnReverse_Click() - " + clsPOSDBConstants.Log_Entering);
                if (grdPaymentComplete.Selected.Rows.Count > 0)
                {
                    bIsReverse = true;
                    RemovePayment(grdPaymentComplete.Selected.Rows[0]);
                    this.grdPaymentComplete.Update();
                    calculatePaidAmount();
                    this.grdPayment.Focus();
                }
                else
                {
                    if (grdPaymentComplete.Rows.Count > 0)
                    {
                        Resources.Message.Display("Please select payment from list. ", "Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    }
                }
                logger.Trace("btnReverse_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "btnReverse_Click()");
            }
        }

        #endregion btnReverse_Click

        #region NumericPad
        private void ShowNumericPad()
        {
            try
            {

                frmNumPad = new frmNumericPad(this.Handle);
                //frmNumPad.Move += new EventHandler(frmNumPad_Move); //Commented by Farman Ansari to remove the frmNumPad_Move
                frmNumPad.TopLevel = false;
                frmNumPad.Dock = DockStyle.Fill;
                frmNumPad.Show();
                //this.Activate();

                //this.Show();
                //clsUIHelper.SETACTIVEWINDOW(this.Handle);
                tableLayoutPanel3.Controls.Add(frmNumPad);
                //frmNumPad.Top = this.lblTransactionType.Top + this.lblTransactionType.Height;
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        #endregion NumericPad

        #region ChangeButtonBackColorAtClick
        private void ChangeButtonBackColorAtClick(Infragistics.Win.Misc.UltraButton Button, bool ChangeBackColor)
        {
            try
            {
                if (ChangeBackColor == true)
                {
                    Button.ButtonStyle = UIElementButtonStyle.FlatBorderless;
                    Button.Appearance.BackColor = clsPOSDBConstants.ClickButtonBackColor;
                    // We chan ged the color of the control from GreenYellow to Control - Shrikant Mali.
                    Button.Appearance.BackColor2 = SystemColors.Control;
                    Button.Appearance.ForeColor = Color.HotPink;
                }
                else
                {
                    Button.ButtonStyle = btnCancel.ButtonStyle;
                    Button.Appearance.BackColor = btnCancel.Appearance.BackColor;
                    Button.Appearance.BackColor2 = btnCancel.Appearance.BackColor2;
                    Button.Appearance.ForeColor = btnCancel.Appearance.ForeColor;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ChangeButtonBackColorAtClick()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #endregion ChangeButtonBackColorAtClick

        #region btnIsDeliveryTrans_Click
        private void btnIsDeliveryTrans_Click(object sender, EventArgs e)
        {
            oPosPTList.bIsDelivery = !oPosPTList.bIsDelivery;
            if (oPosPTList.bIsDelivery == true)
            {
                GetDeliveryAddress();
            }
            //clsUIHelper.setColorSchecme(this);
            ChangeButtonBackColorAtClick(btnIsDeliveryTrans, oPosPTList.bIsDelivery);
            ChangeButtonBackColorAtClick(btnPrintGiftRecpt, bPrintGiftReciept);
            this.grdPayment.Focus();
        }

        #endregion btnIsDeliveryTrans_Click

        #region btnPrintGiftRecpt_Click
        private void btnPrintGiftRecpt_Click(object sender, EventArgs e)
        {
            bPrintGiftReciept = !bPrintGiftReciept;
            ChangeButtonBackColorAtClick(btnIsDeliveryTrans, oPosPTList.bIsDelivery);
            ChangeButtonBackColorAtClick(btnPrintGiftRecpt, bPrintGiftReciept);
            this.grdPayment.Focus();
        }

        #endregion btnPrintGiftRecpt_Click

        #region MoneyKeys
        public void GetMoneyAmount(decimal amt)
        {
            if (grdPayment.ActiveCell == null)
                return;
            if (grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)
            {
                grdPayment.ActiveCell.Value = amt;
                grdPayment.Focus();
                //SendKeys.Send("{Enter}");
                Application.DoEvents();
            }
        }

        private void btnMoneyOne_Click(object sender, EventArgs e)
        {
            string txtKey = btnMoneyOne.Text.Replace("$", string.Empty).Trim();
            decimal amt = Convert.ToDecimal(txtKey.ToString());
            GetMoneyAmount(amt);

        }

        private void btnMoneyFive_Click(object sender, EventArgs e)
        {
            string txtKey = btnMoneyFive.Text.Replace("$", string.Empty).Trim();
            decimal amt = Convert.ToDecimal(txtKey.ToString());
            GetMoneyAmount(amt);
        }

        private void btnMoneyTen_Click(object sender, EventArgs e)
        {
            string txtKey = btnMoneyTen.Text.Replace("$", string.Empty).Trim();
            decimal amt = Convert.ToDecimal(txtKey.ToString());
            GetMoneyAmount(amt);
        }

        private void btnMoneyTwenty_Click(object sender, EventArgs e)
        {
            string txtKey = btnMoneyTwenty.Text.Replace("$", string.Empty).Trim();
            decimal amt = Convert.ToDecimal(txtKey.ToString());
            GetMoneyAmount(amt);
        }

        private void btnMoneyFifty_Click(object sender, EventArgs e)
        {
            string txtKey = btnMoneyFifty.Text.Replace("$", string.Empty).Trim();
            decimal amt = Convert.ToDecimal(txtKey.ToString());
            GetMoneyAmount(amt);
        }

        private void btnMoneyHundred_Click(object sender, EventArgs e)
        {
            string txtKey = btnMoneyHundred.Text.Replace("$", string.Empty).Trim();
            decimal amt = Convert.ToDecimal(txtKey.ToString());
            GetMoneyAmount(amt);
        }

        #endregion MoneyKeys

        #region btnCashCount_Click
        private void btnCashCount_Click(object sender, EventArgs e)
        {
            logger.Trace("btnCashCount_Click() - " + clsPOSDBConstants.Log_Entering);
            frmCashCount ofrmcashcount = new frmCashCount();
            this.grdPayment.Focus();
            this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
            if (DialogResult.OK == ofrmcashcount.ShowDialog())
            {
                this.CashCountAmountIsGreaterThanZero(ofrmcashcount);
            }
            logger.Trace("btnCashCount_Click() - " + clsPOSDBConstants.Log_Exiting);
            //grdPayment_KeyDown(sender, KeyEventArgs.);
        }

        private void CashCountAmountIsGreaterThanZero(frmCashCount ofrmcashcount)
        {
            if (Convert.ToDouble(ofrmcashcount.utxtTotalAmount.Text) > 0)
            {
                this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = Convert.ToDouble(ofrmcashcount.utxtTotalAmount.Text);
                bool bTenderedAmountOverrideCancel = false; //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
                if (isOverPayment(out bTenderedAmountOverrideCancel) == true)
                {
                    if (Resources.Message.Display("Paid amount is far greater than total due amount.\n Are you sure to continue? ", "Payment Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        oPosPTList.LastKey = "Esc";//Added by SRT (3-Nov-08).
                        return;
                    }
                }
                else  //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added to handle condition when user calcel the override window
                {
                    if (bTenderedAmountOverrideCancel == true)
                        return;
                }

                decimal paidAmount = oPosPTList.amountBalanceDue;
                if (paidAmount > 0)
                {
                    AddRowToPaid(grdPayment.ActiveRow);
                    this.grdPayment.Rows[1].Activate();
                    this.grdPayment.Rows[1].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                    this.grdPayment.Rows[1].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();// = Activation.AllowEdit;
                }
            }

        }

        #endregion btnCashCount_Click

        #region IsHouseChargeAccount
        public void IsHouseChargeAccount()
        {
            try
            {
                string sHousechargeAccountNo = oPosPTList.GetHouseChargeAccount();
                if (sHousechargeAccountNo != "" && sHousechargeAccountNo != "0")
                {
                    MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                    DataSet oDS = new DataSet();
                    //oAcct.GetAccountByCode(sHousechargeAccountNo, out oDS);
                    oAcct.GetAccountByCode(sHousechargeAccountNo, out oDS, true); //PRIMEPOS-2888 28-Aug-2020 JY Added third parameter as "true" to get the exact HouseCharge record
                    if (oDS == null || oDS.Tables[0].Rows.Count == 0)
                    {
                        lblHCAccPresent.Visible = false;
                    }
                    else
                    {
                        lblHCAccPresent.Visible = true;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsHouseChargeAccount()");
                lblHCAccPresent.Visible = false;
                if (Configuration.CPOSSet.UsePrimeRX) //PRIMEPOS-3106 22-Jun-2022 JY Added if condition
                    clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region PRIMEPOS-2611 13-Nov-2018 JY Added 
        private void ShowCustomerTokenImage(int CustomerId)
        {
            try
            {
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                if (oCCCustomerTokInfo.IsCustomerTokenExists(CustomerId))
                {
                    lblCustomerName.Appearance.Image = Properties.Resources.CreditCard;
                    tmrBlinking.Enabled = true;
                }
                else
                {
                    lblCustomerName.Appearance.Image = null;
                    tmrBlinking.Enabled = false;
                    lblCustomerName.Appearance.ForeColor = Color.White;
                    iBlinkCnt = 0;
                }
            }
            catch (Exception Ex)
            {
                logger.Trace(Ex, "ShowCustomerTokenImage(int CustomerId)");
            }
        }

        public void tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                iBlinkCnt++;
                if (iBlinkCnt % 2 == 0)
                    lblCustomerName.Appearance.ForeColor = Color.Transparent;
                else
                    lblCustomerName.Appearance.ForeColor = Color.White;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)");
            }
        }
        #endregion

        #endregion IsHouseChargeAccount

        #region PRIMEPOS-2738 
        public bool chkReversal(frmPOSProcessCC oProcessCC)
        {
            FetchOriginalPaymentTransactions();
            bool retVal = ShowOriginalPaymentDetails();

            //RITESH FUNCTION VARIABLE
            if (retVal == true)
            {
                return false;
            }
            if (retVal != true)
            {
                if (oOrigPayTransData.Tables.Count > 0)
                {
                    //RITESH ADD COMMENTS
                    //This collects the data of row presents in both the tables
                    var collectDatarow = from c in oShowTransData.Tables[0].AsEnumerable()
                                         join p in oOrigPayTransData.Tables[0].AsEnumerable() on c.Field<int>("TransPayID") equals p.Field<int>("TransPayID") into ps
                                         from p in ps.DefaultIfEmpty()
                                         select c;

                    List<DataRow> lstDatarow = collectDatarow.ToList();

                    foreach (DataRow dr in lstDatarow)
                    {
                        //Adds only the row which is not present in this dataset oOrigPayTransData 
                        if (oOrigPayTransData.Tables[0].Select("TransPayID = " + dr["TransPayID"]).Length == 0)
                        {
                            DataRow drNew = oOrigPayTransData.Tables[0].NewRow();
                            drNew["TransPayID"] = dr["TransPayID"];
                            drNew["ProcessorTransID"] = dr["ProcessorTransID"];
                            drNew["Amount"] = dr["Amount"];
                            drNew["Refno"] = dr["Refno"];
                            drNew["TransTypeCode"] = dr["TransTypeCode"];
                            drNew["Transrefnum_trn"] = dr["Transrefnum_trn"];
                            drNew["ReversedAmount"] = dr["ReversedAmount"];
                            drNew["Transdate"] = dr["Transdate"]; //PRIMEPOS-3375 Needs to check
                            oOrigPayTransData.Tables[0].Rows.Add(drNew);

                        }
                    }
                }
            }

            String grdAmount = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString().Contains("-") ? this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString().Remove(0, 1) : this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString();
            if (Convert.ToDecimal(grdAmount) >= Convert.ToDecimal(Amount))
            {
                if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() != "1" && oProcessCC != null)
                {
                    oProcessCC.Amount = decimal.Round(Convert.ToDecimal(Amount), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = "-" + Amount;
                }
            }
            else
            {
                Amount = grdAmount;
            }

            this.grdPayment.Focus();
            this.grdPayment.Update();
            this.grdPayment.Refresh();
            return true;
        }

        public void updRevAmtInOrigTxn()
        {
            for (int i = 0; i < oOrigPayTransData.Tables[0].Rows.Count; i++)
            {
                if (oOrigPayTransData.Tables[0].Rows[i]["TransPayID"].ToString().Trim() == TransPayID)
                {
                    oOrigPayTransData.Tables[0].Rows[i]["ReversedAmount"] = Convert.ToDecimal(oOrigPayTransData.Tables[0].Rows[i]["ReversedAmount"]) + Convert.ToDecimal(Amount);
                }
            }
        }

        public DataSet FetchOriginalPaymentTransactions()
        {
            if (oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
            {
                bool isPrimeRxPay = false; //PRIMEPOS-3189
                //oPosTrans = new POSTransPaymentSvr();
                //oPayTransData = new DataSet();  //reversal PRIMEPOS-2738 make it class level
                transType = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim();
                if (transType.Trim() == "O")//PRIMEPOS-2841
                {
                    //transType = "3,4,5,6";
                    transType = "'3','4','5','6'";//PRIMEPOS-3173
                    isPrimeRxPay = true;//PRIMEPOS-3189
                }
                else
                {
                    if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") //PRIMEPOS-3521 //PRIMEPOS-3522
                    {
                        if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "-99")
                        {
                            //transType = "3,4,5,6";
                            transType = "'3','4','5','6','7','N'";//PRIMEPOS-3173
                        }
                    }
                    else
                    {
                        if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() == "-99")
                        {
                            //transType = "3,4,5,6";
                            transType = "'3','4','5','6'";//PRIMEPOS-3173
                        }
                    }
                }
                String originalTransID = TransID.ToString();

                if (Configuration.CSetting.StrictReturn == true)
                {
                    oShowTransData = oPosTrans.GetPaymentDetail(originalTransID, transType, oOrigPayTransData, isPrimeRxPay);
                    if (oOrigPayTransData.Tables.Count == 0)
                    {
                        oOrigPayTransData.Merge(oShowTransData);
                    }
                }
                else
                {
                    oOrigPayTransData = oPosTrans.GetPOSPaymentDetail(originalTransID, transType);
                }

            }
            return oOrigPayTransData;
        }

        public bool ShowOriginalPaymentDetails()
        {
            bool retVal = false;
            if (Configuration.CSetting.StrictReturn == true && oPosPTList.oTransactionType == POSTransactionType.SalesReturn)
            {
                if (oShowTransData.Tables[0].Rows.Count != 0)  //// if txnType=refund and strict reversal PRIMEPOS-2738
                {
                    frmPaymentReturn oPayTrans = new frmPaymentReturn();
                    oPayTrans.isSolutran = false;
                    oPayTrans.grdPaymentTrans(oShowTransData);
                    //oPayTrans.ControlBox = false;

                    oPayTrans.ShowDialog(this);
                    retVal = oPayTrans.IsCanceled;

                    if (oPayTrans.IsCanceled != true)
                    {
                        HrefNumber = oPayTrans.TransRefNumber;
                        nbsUid = oPayTrans.nbsUid;
                        nbsType = oPayTrans.nbsPayType;
                        ProcessTransID = oPayTrans.ProcessTransID;
                        Amount = oPayTrans.Amount;
                        ReversedAmount = oPayTrans.ReversedAmount;//PRIMEPOS-2738                        
                        TransPayID = oPayTrans.TransPayID;//PRIMEPOS-2738    
                        TransDate = oPayTrans.TransDate;//PRIMEPOS-2943
                        TransTypeCode = oPayTrans.TransTypeCode;//PRIMEPOS-3087
                    }

                }
                else
                {
                    clsUIHelper.ShowErrorMsg("The Amount is already returned for this Payment Type");
                    retVal = true;
                }
            }
            return retVal;
        }
        //Addition of new method for updating the reversed amount in the database
        #endregion

        #region PRIMEPOS-2664 ADDED BY ARVIND
        private string EvertecCash(UltraGridRow oGridRow)
        {
            string AuthNo = string.Empty;
            try
            {
                if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC" && Convert.ToDouble(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) != 0)
                {
                    PmtTxnResponse evertecDevResponse = new PmtTxnResponse();
                    string hostAddress = Configuration.CPOSSet.SigPadHostAddr.Split(':')[0];
                    string portNo = Configuration.CPOSSet.SigPadHostAddr.Split(':')[1].Split('/')[0];

                    evertecProcessor = EvertechProcessor.getInstance(hostAddress, Convert.ToInt32(portNo));

                    if (!evertecProcessor.isLoggedOn)
                        evertecProcessor.Logon(Configuration.CPOSSet.TerminalID, Configuration.StationID, Configuration.CashierID);
                    Dictionary<String, String> fields = new Dictionary<string, string>();
                    string ActualAmount = string.Empty;
                    string requesy = CalculateTaxAmount(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw), out ActualAmount);
                    fields.Add("AMOUNT", ActualAmount);
                    if (!string.IsNullOrWhiteSpace(requesy))
                    {
                        fields.Add("STATETAX", requesy.Split('|')[0]);
                        fields.Add("CITYTAX", requesy.Split('|')[1]);
                        fields.Add("REDUCESTATETAX", requesy.Split('|')[2]);//PRIMEPOS-3099
                        fields.Add("REDUCECITYTAX", requesy.Split('|')[3]);//PRIMEPOS-3099
                    }
                    else
                    {
                        fields.Add("STATETAX", "0");
                        fields.Add("CITYTAX", "0");
                        fields.Add("REDUCESTATETAX", "0");//PRIMEPOS-3099
                        fields.Add("REDUCECITYTAX", "0");//PRIMEPOS-3099
                    }
                    evertecDevResponse = evertecProcessor.IVUCash(fields);

                    if (evertecDevResponse.ResultDescription.ToUpper() != "SUCCESS")
                    {
                        clsUIHelper.ShowErrorMsg("Falied from Evertec device. Error is :" + evertecDevResponse.ResultDescription);
                        return AuthNo;
                    }
                    else
                    {
                        //EvertecReceipt("IVUCASH");
                        if (evertecDevResponse.EmvReceipt != null)
                        {
                            PccPaymentSvr.DefaultInstance.EmvTags = new MMS.PROCESSOR.EmvReceiptTags();
                            PccPaymentSvr.DefaultInstance.EmvTags.MerchantID = evertecDevResponse.EmvReceipt.MerchantID.Trim();
                            PccPaymentSvr.DefaultInstance.EmvTags.BatchNumber = evertecDevResponse.EmvReceipt.BatchNumber;
                            PccPaymentSvr.DefaultInstance.EmvTags.TraceNumber = evertecDevResponse.EmvReceipt.TraceNumber;
                            PccPaymentSvr.DefaultInstance.EmvTags.InvoiceNumber = evertecDevResponse.EmvReceipt.InvoiceNumber;
                            PccPaymentSvr.DefaultInstance.EmvTags.ReferenceNumber = evertecDevResponse.EmvReceipt.ReferenceNumber;
                            PccPaymentSvr.DefaultInstance.EmvTags.EbtBalance = evertecDevResponse.EmvReceipt.EbtBalance;
                            PccPaymentSvr.DefaultInstance.EmvTags.IsEvertecForceTransaction = evertecDevResponse.EmvReceipt.IsEvertecForceTransaction;//PRIMEPOS-2831
                            PccPaymentSvr.DefaultInstance.EmvTags.IsEvertecSign = evertecDevResponse.EmvReceipt.IsEvertecSign;//PRIMEPOS-2831
                            PccPaymentSvr.DefaultInstance.EmvTags.EvertecTaxBreakdown = evertecDevResponse.EmvReceipt.EvertecTaxBreakdown;//PRIMEPOS-2857
                            PccPaymentSvr.DefaultInstance.EmvTags.ControlNumber = evertecDevResponse.EmvReceipt.ControlNumber;//PRIMEPOS-2857
                            PccPaymentSvr.DefaultInstance.EmvTags.ATHMovil = evertecDevResponse.EmvReceipt.ATHMovil;//PRIMEPOS-2857
                            PccPaymentSvr.DefaultInstance.EntryMethod = evertecDevResponse.EntryMethod;
                            AuthNo = evertecDevResponse.AuthNo;
                        }
                    }
                }
                return AuthNo;
            }
            catch (Exception ex)
            {
                logger.Error("Error in EvertecCash() : " + ex.Message);
                return AuthNo;
            }
        }
        private string EvertecReceipt(string transType)
        {
            try
            {
                if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                {
                    string hostAddress = Configuration.CPOSSet.SigPadHostAddr.Split(':')[0];
                    string portNo = Configuration.CPOSSet.SigPadHostAddr.Split(':')[1].Split('/')[0];

                    evertecProcessor = EvertechProcessor.getInstance(hostAddress, Convert.ToInt32(portNo));

                    if (!evertecProcessor.isLoggedOn)
                        evertecProcessor.Logon(Configuration.CPOSSet.TerminalID, Configuration.StationID, Configuration.CashierID);
                    controlNumber = evertecProcessor.ReceiptData(transType).Trim();
                    return controlNumber;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in EvertecRceipt() :" + ex.Message);
            }
            return controlNumber;
        }
        #endregion

        #region  primepos-2841
        public bool chkReversal(frmPOSOnlineCC oProcessCC)
        {
            FetchOriginalPaymentTransactions();
            bool retVal = ShowOriginalPaymentDetails();

            //RITESH FUNCTION VARIABLE
            if (retVal == true)
            {
                return false;
            }
            if (retVal != true)
            {
                if (oOrigPayTransData.Tables.Count > 0)
                {
                    //RITESH ADD COMMENTS
                    //This collects the data of row presents in both the tables
                    var collectDatarow = from c in oShowTransData.Tables[0].AsEnumerable()
                                         join p in oOrigPayTransData.Tables[0].AsEnumerable() on c.Field<int>("TransPayID") equals p.Field<int>("TransPayID") into ps
                                         from p in ps.DefaultIfEmpty()
                                         select c;

                    List<DataRow> lstDatarow = collectDatarow.ToList();

                    foreach (DataRow dr in lstDatarow)
                    {
                        //Adds only the row which is not present in this dataset oOrigPayTransData 
                        if (oOrigPayTransData.Tables[0].Select("TransPayID = " + dr["TransPayID"]).Length == 0)
                        {
                            DataRow drNew = oOrigPayTransData.Tables[0].NewRow();
                            drNew["TransPayID"] = dr["TransPayID"];
                            drNew["ProcessorTransID"] = dr["ProcessorTransID"];
                            drNew["Amount"] = dr["Amount"];
                            drNew["Refno"] = dr["Refno"];
                            drNew["TransTypeCode"] = dr["TransTypeCode"];
                            drNew["Transrefnum_trn"] = dr["Transrefnum_trn"];
                            drNew["ReversedAmount"] = dr["ReversedAmount"];
                            drNew["Transdate"] = dr["Transdate"];
                            oOrigPayTransData.Tables[0].Rows.Add(drNew);

                        }
                    }
                }
            }

            //reversedAmount = oOrigPayTransData.Tables[0].Rows.Cast<DataRow>()
            //                    .Where(r => r.Field<int>("TransPayID").ToString().Trim() == TransPayID)
            //                    .Select(p => p.Field<decimal>("ReversedAmount")).FirstOrDefault();


            String grdAmount = this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString().Contains("-") ? this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString().Remove(0, 1) : this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString();
            if (Convert.ToDecimal(grdAmount) >= Convert.ToDecimal(Amount))
            {
                if (this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Text.Trim() != "1" && oProcessCC != null)
                {
                    oProcessCC.Amount = decimal.Round(Convert.ToDecimal(Amount), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = "-" + Amount;
                }
            }
            else
            {
                Amount = grdAmount;
                //if
                //reversedAmount = Convert.ToDecimal(Amount);
            }

            this.grdPayment.Focus();
            this.grdPayment.Update();
            this.grdPayment.Refresh();
            return true;
        }
        #endregion

        #region Commented Code
        #region GetCCInformation        ////////this method does the following:
        //////// if the Receive  on Account isROA is true, it get the cc information from the charge account table
        //////// if the transaction has a Rx in it, it finds out the patient and gets the cc infor from there
        //////// if cc info is not found returns blank
        //////// if cc info is found, populates the PCCCardINfo object with the cc info and makes it available for the next actual cc process to use as default
        //////private string GetCCInformation()
        //////{
        //////    CustomerCardInfo = new PccCardInfo();
        //////    string sCardTypeName = "";
        //////    bool ccinfoFound=false;

        //////    if (isROA)
        //////    {
        //////        if (this.dtChargeAccount != null && this.dtChargeAccount.Rows.Count > 0 && dtChargeAccount.Rows[0]["CCNUMBER"].ToString().Trim().Length > 0)
        //////        {
        //////            DataTable dtCC = new DataTable();
        //////            dtCC.Columns.Add("Name");
        //////            dtCC.Columns.Add("CCNUMBERMASK");
        //////            dtCC.Columns.Add("CCNUMBER");
        //////            dtCC.Columns.Add("CCEXPDATE");
        //////            dtCC.Columns.Add("NameOnCC");
        //////            dtCC.Columns.Add("ZIP");
        //////            string[] fields = new String[6];
        //////            fields[0]=dtChargeAccount.Rows[0]["ACCT_NAME"].ToString();
        //////            fields[1]=GetMaskedCC(dtChargeAccount.Rows[0]["CCNUMBER"].ToString());
        //////            fields[2]=dtChargeAccount.Rows[0]["CCNUMBER"].ToString();
        //////            fields[3]=dtChargeAccount.Rows[0]["CCEXPDATE"].ToString();
        //////            fields[4]=dtChargeAccount.Rows[0]["NameOnCC"].ToString();
        //////            fields[5]=dtChargeAccount.Rows[0]["ZIP"].ToString();

        //////            dtCC.Rows.Add(fields);

        //////            frmCCInfoSelect frmccInfo = new frmCCInfoSelect();
        //////            frmccInfo.CCInfo = dtCC;
        //////            if (frmccInfo.ShowDialog() == DialogResult.Yes)
        //////            {
        //////                this.CustomerCardInfo = frmccInfo.SelectedCC;

        //////                //CustomerCardInfo.cardNumber = dtChargeAccount.Rows[0]["CCNUMBER"].ToString();
        //////                //CustomerCardInfo.cardExpiryDate = dtChargeAccount.Rows[0]["CCEXPDATE"].ToString();
        //////                //CustomerCardInfo.zipCode = dtChargeAccount.Rows[0]["ZIP"].ToString();
        //////                //CustomerCardInfo.cardHolderName = dtChargeAccount.Rows[0]["NameOnCC"].ToString();
        //////                ccinfoFound=true;
        //////            }
        //////        }
        //////    }

        //////    if (ccinfoFound)
        //////    {
        //////        PccPaymentSvr pmtSrv = new PccPaymentSvr();
        //////        string sCardType=pmtSrv.GetCardType(CustomerCardInfo.cardNumber);

        //////        //now convert the cardtype to the card name as present in our system
        //////        if (sCardType.Trim().Length > 0)
        //////        {
        //////            switch (sCardType)
        //////            {
        //////                case "VISA":
        //////                    sCardTypeName = "VISA";
        //////                    break;

        //////                case "MC":
        //////                    sCardTypeName = "MASTER CARD";
        //////                    break;

        //////                case "AMEX":
        //////                    sCardTypeName = "AMERICAN EXPRESS";
        //////                    break;

        //////                case "DISC":
        //////                    sCardTypeName = "DISOVER";
        //////                    break;
        //////                default:
        //////                    sCardTypeName = "VISA";
        //////                    break;
        //////            }
        //////        }
        //////    }
        //////    return sCardTypeName;
        //////}
        #endregion GetCCInformation

        #region GetMaskedCC
        //////private string GetMaskedCC(string CCNumber)
        //////{
        //////    string smaskedCC = "";
        //////    if (CCNumber.Trim().Length > 4)
        //////    {
        //////        string mX = CCNumber.Trim().Substring(0, CCNumber.Trim().Length - 4);
        //////        mX = "XXXXXXXXXXXXXXXX".Substring(0, mX.Length);
        //////        string mN = CCNumber.Trim().Substring(CCNumber.Trim().Length - 4, 4);
        //////        smaskedCC = mX + mN;
        //////    }
        //////    else
        //////        smaskedCC = CCNumber;

        //////    return smaskedCC;
        //////}
        #endregion GetMaskedCC

        #region  ChkToken_CheckedChanged
        //Sprint-23 - PRIMEPOS-2315 21-Jun-2016 JY Commented
        //private void ChkToken_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ChkToken.Checked)
        //    {
        //        Tokenize = true;
        //    }
        //    else
        //    {
        //        Tokenize = false;
        //    }
        //}
        #endregion  ChkToken_CheckedChanged

        #region GetCompletedPay
        //private POSTransPaymentData GetCompletedPay()
        //{
        //    POSTransPaymentData paidTrans = new POSTransPaymentData();
        //    if (TransID > 0)
        //    {
        //        POS_Core.DataAccess.POSTransPaymentSvr payTrans = new DataAccess.POSTransPaymentSvr();
        //        paidTrans = payTrans.Populate(TransID);
        //    }
        //    return paidTrans;
        //}
        #endregion GetCompletedPay

        #region CalculateOnReturn
        //private void CalculateOnReturn()
        //{
        //    if (Configuration.CPOSSet.AllowZeroAmtTransaction == true)
        //    {
        //        if (oPOSTransPaymentDataPaid.POSTransPayment.Rows.Count > 0)//|| this.totalAmount == 0)//Changed by Naim for additional check
        //        {
        //            if (Convert.ToDecimal(this.txtAmtBalanceDue.Text) <= 0)
        //            {
        //                this.Close();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (oPOSTransPaymentDataPaid.POSTransPayment.Rows.Count > 0 || this.totalAmount + Convert.ToDecimal(txtAmtCashBack.Text) == 0)
        //        {
        //            if (Convert.ToDecimal(this.txtAmtBalanceDue.Text) <= 0)
        //            {
        //                this.Close();
        //            }
        //        }
        //    }
        //}
        #endregion CalculateOnReturn

        #region PayTypeID
        //private string PayTypeID(string selected, string PaidTransType)
        //{
        //    string payTypeId = string.Empty;
        //    switch (selected)
        //    {
        //        case "T":
        //            {
        //                payTypeId = "7";
        //                break;
        //            }
        //        case "R":
        //            {
        //                if (PaidTransType.Trim().Substring(0, 1) == "A")
        //                {
        //                    payTypeId = "3";
        //                }
        //                else if (PaidTransType.Trim().Substring(0, 1) == "V")
        //                {
        //                    payTypeId = "4";
        //                }
        //                else if (PaidTransType.Trim().Substring(0, 1) == "M")
        //                {
        //                    payTypeId = "5";
        //                }
        //                else if (PaidTransType.Trim().Substring(0, 1) == "D")
        //                {
        //                    payTypeId = "6";
        //                }
        //                break;
        //            }
        //        default:
        //            {
        //                payTypeId = string.Empty;
        //                break;
        //            }
        //    }
        //    return payTypeId;
        //}
        #endregion PayTypeID

        #region ProcessCouponID
        //private bool ProcessCouponID(Int64 clCouponID)
        //{
        //    return ProcessCouponID(clCouponID, Convert.ToDecimal(this.txtAmtBalanceDue.Text));
        //}

        #endregion ProcessCouponID

        #region btnCancelTrans_Click
        //private void btnCancelTrans_Click(object sender, System.EventArgs e)
        //{
        //    CancelTrans();
        //}

        #endregion btnCancelTrans_Click

        #region FKEdit
        //private void FKEdit(string code, string senderName)
        //{
        //    #region HouseCharge

        //    try
        //    {
        //    }
        //    catch (System.IndexOutOfRangeException)
        //    {
        //        SearchHouseChargeInfo();
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }

        //    #endregion HouseCharge
        //}

        #endregion FKEdit

        #region AddRowToPaid 
        //private void AddRowToPaid(Infragistics.Win.UltraWinGrid.UltraGridRow oGridRow)
        //{
        //    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Payment Made of $" + Convert.ToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) + "By " + Configuration.convertNullToString(grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString()), clsPOSDBConstants.Log_Entering);
        //    bool isAdded = false;
        //    POSTransPaymentRow oRowPaid = (POSTransPaymentRow) this.oPOSTransPaymentDataPaid.POSTransPayment.NewPOSTransPaymentRow();

        //    oRowPaid.TransTypeCode = this.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].TransTypeCode;
        //    oRowPaid.TransTypeDesc = this.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].TransTypeDesc;

        //    oRowPaid.TransID = this.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].TransID;
        //    oRowPaid.TransDate = DateTime.Now;
        //    oRowPaid.RefNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].GetText(MaskMode.Raw);
        //    oRowPaid.HC_Posted = this.oPOSTransPaymentData.POSTransPayment[oGridRow.Index].HC_Posted;
        //    oRowPaid.CustomerSign = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].GetText(MaskMode.Raw);
        //    oRowPaid.CCTransNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo].GetText(MaskMode.Raw);
        //    oRowPaid.CCName = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].GetText(MaskMode.Raw);
        //    oRowPaid.AuthNo = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].GetText(MaskMode.Raw);
        //    oRowPaid.Amount = Convert.ToDecimal(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
        //    oRowPaid.IsIIASPayment = Configuration.convertNullToBoolean(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment].Value.ToString());
        //    oRowPaid.CLCouponID = Configuration.convertNullToInt(oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID].Value.ToString());
        //    if(oGridRow.Cells["SigType"].Value.ToString() != "")
        //        oRowPaid.SigType = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_SigType].GetText(MaskMode.Raw);

        //    //Code added to initialize Payment Processor. Need to update as development goes onword.
        //    oRowPaid.PaymentProcessor = clsPOSDBConstants.NOPROCESSOR;




        //    oRowPaid.IsManual = oGridRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_IsManual].GetText(MaskMode.Raw);  //Sprint-19 - 2139 06-Jan-2015 JY Added
        //    try

        //    {
        //        if(oGridRow.Cells["BinarySign"].Value.ToString() != "" && oGridRow.Cells["SigType"].Value.ToString() != "")
        //        {
        //            oRowPaid.BinarySign = (byte[]) oGridRow.Cells["BinarySign"].Value;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //    }
        //    #region comment
        //    //bool greaterAmt = false;
        //    //bool isCash = false;
        //    ////Fix by Manoj for split cash payment. 7/9/2015
        //    //foreach(POSTransPaymentRow oCRow in oPOSTransPaymentDataPaid.Tables[0].Rows)
        //    //{
        //    //    if (oRowPaid.TransTypeCode == this.oPOSTransPaymentData.POSTransPayment[0].TransTypeCode && oCRow.TransTypeCode == this.oPOSTransPaymentData.POSTransPayment[0].TransTypeCode)
        //    //     {
        //    //        isCash = true;
        //    //         EnteredAmt = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
        //    //         if (EnteredAmt > (totalAmount - FullAmountPaid))
        //    //         {
        //    //             decimal remainingAmt = Convert.ToDecimal(txtAmtBalanceDue.Text);
        //    //             this.tenderedAmount = EnteredAmt + oCRow.Amount;
        //    //             oCRow.Amount += remainingAmt;
        //    //             FullAmountPaid = tenderedAmount;
        //    //             greaterAmt = true;
        //    //         }
        //    //         else
        //    //         {
        //    //             oCRow.Amount += EnteredAmt;
        //    //             this.tenderedAmount = oCRow.Amount; 
        //    //             FullAmountPaid = oCRow.Amount;
        //    //         }
        //    //         isAdded = true;

        //    //     }
        //    //    else 
        //    //    {
        //    //        if (!isCash && !greaterAmt)
        //    //            FullAmountPaid = 0;
        //    //        else if (isCash && greaterAmt)
        //    //            FullAmountPaid += EnteredAmt + oCRow.Amount;
        //    //        else
        //    //            FullAmountPaid += oCRow.Amount;
        //    //        //if (greaterAmt)
        //    //        //{
        //    //        //    FullAmountPaid += oCRow.Amount;
        //    //        //}
        //    //        //else
        //    //        //{
        //    //        //    FullAmountPaid = 0;
        //    //        //    FullAmountPaid = EnteredAmt + oCRow.Amount;
        //    //        //}
        //    //    }              
        //    //}

        //    //if (!isAdded)
        //    //{
        //    //    EnteredAmt = Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
        //    //    if(EnteredAmt > totalAmount)
        //    //    {
        //    //        oRowPaid.Amount = totalAmount;
        //    //    }
        //    //    else
        //    //    {
        //    //        oRowPaid.Amount = EnteredAmt;
        //    //    }
        //    //    if (oRowPaid.TransTypeCode == this.oPOSTransPaymentData.POSTransPayment[0].TransTypeCode)
        //    //    {
        //    //        this.tenderedAmount += EnteredAmt;
        //    //    }
        //    //    FullAmountPaid += EnteredAmt;
        //    //    oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);
        //    //}
        //    #endregion
        //    //oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);
        //    //Fix by Manoj for split cash payment. 7/9/2015
        //    //foreach (POSTransPaymentRow oCRow in oPOSTransPaymentDataPaid.Tables[0].Rows)
        //    //{
        //    //    if (oRowPaid.TransTypeCode == this.oPOSTransPaymentData.POSTransPayment[0].TransTypeCode && oCRow.TransTypeCode == this.oPOSTransPaymentData.POSTransPayment[0].TransTypeCode)
        //    //    {
        //    //        oCRow.Amount += oRowPaid.Amount;
        //    //        isAdded = true;
        //    //    }
        //    //}
        //    //if (!isAdded)
        //    oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);

        //    POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Payment Made of $" + oRowPaid.Amount +"By "+Configuration.convertNullToString(grdPayment.ActiveRow.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Value.ToString()), clsPOSDBConstants.Log_Success);
        //    ClearGridRow(oGridRow);
        //    calculatePaidAmount();
        //    if (oRowPaid.TransTypeCode.Trim().Equals("E") == true)
        //    {
        //        oGridRow.Activation = Activation.Disabled;
        //    }
        //}
        #endregion AddRowToPaid

        #region stTBTerminal_ButtonClick
        //private void stTBTerminal_ButtonClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelEventArgs e)
        //{
        //    //try			{
        //    if (this.grdPayment.ActiveCell == null) return;
        //    if (this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)
        //    {
        //        decimal amt = Convert.ToDecimal(e.Panel.Key.ToString());
        //        this.grdPayment.ActiveCell.Value = amt;
        //        this.grdPayment.Focus();
        //        SendKeys.Send("{Enter}");
        //        Application.DoEvents();
        //        //POSNumericKeyPad.NumericKeyPad nkp = new POSNumericKeyPad.NumericKeyPad();
        //        //nkp.buttonClick("btnenter");
        //    }
        //    //}catch(Exception )			{}
        //}
        #endregion stTBTerminal_ButtonClick

        #region ReversePayment
        /*private bool ReversePayment(Infragistics.Win.UltraWinGrid.UltraGridRow oRow)
        {
            bool retVal = false;

            try
            {
                string strPaymentType = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim().ToUpper();
                switch (strPaymentType)
                {
                    case "1":
                    case "2":
                    case "C":
                    case "H": //Added to remove HC by Manoj 1/27/2014
                        oRow.Delete(false);
                        this.grdPaymentComplete.UpdateData();
                        break;
                    //  case "H":
                    //    break;
                    case "4":
                    case "5":
                    case "7":
                    case "E":
                        if (bIsReverse)
                        {
                            bIsReverse = false;
                            if (Resources.Message.Display("This action will reverse selected payment from list.\n Are you sure to continue? ", "Payment Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return false;
                            }
                        }
                        foreach (POSTransPayment_CCLog oLog in oPOSTransPayment_CCLogList)
                        {
                            if (oLog.Amount.ToString("###########0.00") == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text &&
                                oLog.RefNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Text &&
                                oLog.AuthNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Text)
                            {
                                oLog.PccCardInfo.transAmount = oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text.ToString();
                                frmPOSProcessCC oFrmPOSProcessCC = new frmPOSProcessCC(this.oTransactionType, "");
                                if (oLog.PccCardInfo == null)
                                {
                                    oRow.Delete(false);
                                    this.grdPaymentComplete.UpdateData();
                                    oPOSTransPayment_CCLogList.Remove(oLog);
                                }
                                else if (oFrmPOSProcessCC.PerformReverseTransaction(oLog.PccCardInfo, this.oTransactionType) == true)
                                {
                                    oRow.Delete(false);
                                    this.grdPaymentComplete.UpdateData();
                                    oPOSTransPayment_CCLogList.Remove(oLog);
                                }
                                break;
                            }
                        }
                        break;
                    case "3":
                    case "6":
                        if (bIsReverse && Configuration.CPOSSet.PaymentProcessor != "HPS") // Add HPS by Manoj - 7/11/2012
                        {
                            Resources.Message.Display("Transaction is Reversed only for VISA and Master card ", "Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            bIsReverse = false;
                            return false;
                        }
                        else
                        {
                            try
                            {
                                foreach (POSTransPayment_CCLog oLog in oPOSTransPayment_CCLogList)
                                {
                                    if (oLog.Amount.ToString("###########0.00") == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Text &&
                                        oLog.RefNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Text &&
                                        oLog.AuthNo == oRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_AuthNo].Text)
                                    {
                                        frmPOSProcessCC oFrmPOSProcessCC = new frmPOSProcessCC(this.oTransactionType, "");
                                        if (oLog.PccCardInfo == null)
                                        {
                                            oRow.Delete(false);
                                            this.grdPaymentComplete.UpdateData();
                                            oPOSTransPayment_CCLogList.Remove(oLog);
                                        }
                                        else if (oFrmPOSProcessCC.PerformVoidTransaction(oLog.PccCardInfo, this.oTransactionType) == true)
                                        {
                                            oRow.Delete(false);
                                            this.grdPaymentComplete.UpdateData();
                                            oPOSTransPayment_CCLogList.Remove(oLog);
                                        }
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                logger.Trace(ex, "ReversePayment() - 1");
                            }
                        }
                        break;
                }
                retVal = true;
                calculatePaidAmount();
            }
            catch (Exception ex)
            {
                logger.Trace(ex, "ReversePayment() - 2");
            }
            return retVal;
        }*/
        #endregion ReversePayment

        #region RunNumericPad
        /* private void RunNumericPad()
         {
             try
             {
                 ((frmNumericPad2)frmNumPad).AttachParent(this.Handle);
             }
             catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
         }

         #endregion RunNumericPad*/

        #endregion RunNumericPad

        #region grdPayment_InitializeLayout
        //private void grdPayment_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //}

        #endregion grdPayment_InitializeLayout

        #region lblTransactionType_Click
        //private void lblTransactionType_Click(object sender, EventArgs e)
        //{
        //}

        #endregion lblTransactionType_Click

        #region frmNumPad_Move
        /*  private void frmNumPad_Move(object sender, EventArgs e)
           {
               try
               {
                   frmNumPad = (Form)sender;
                   frmNumPad.Location = new Point(6, 158);
               }
               catch { }
               //throw new NotImplementedException();
           }*/
        #endregion frmNumPad_Move
        #endregion Commented Code

        private void grdPayment_AfterRowRegionScroll(object sender, RowScrollRegionEventArgs e)
        {
            grdPayment.Refresh();
        }

        private void frmPOSPayTypesList_Shown(object sender, EventArgs e)
        {
            #region PRIMEPOS-2520 10-May-2018 JY Commented
            //if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales) {
            //    if (oPosPTList.oCLCardRow != null && Configuration.CLoyaltyInfo.UseCustomerLoyalty == true && Configuration.CLoyaltyInfo.RedeemMethod == (int)CLRedeemMethod.Auto) {

            //        AddAutoCLDiscount();
            //    }
            //}
            #endregion
        }

        #region BatchDelivery - NileshJ - PRIMERX-7688 - 23-Sept-2019
        // Add partial payment for already paid amount of order level
        public void ProcessAlreadyPaidAmount()
        {
            POSTransPaymentRow oRowPaid = (POSTransPaymentRow)oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.NewPOSTransPaymentRow();
            oRowPaid.TransTypeCode = "1"; //Cash
            oRowPaid.TransTypeDesc = "Cash"; //It is hard coded to Cash right now because if we have credit etc then if the transaction is cancelled it will try doing a void which would not work in current scenario
            oRowPaid.TransID = 0;// Autogenerated by db
            oRowPaid.TransDate = DateTime.Now;
            oRowPaid.RefNo = "";
            oRowPaid.HC_Posted = false;
            oRowPaid.CustomerSign = "";
            oRowPaid.CCTransNo = "";
            oRowPaid.CCName = "";
            oRowPaid.AuthNo = "";
            oRowPaid.Amount = oPosPTList.BatchDelTotalPaidAmount;
            oRowPaid.IsIIASPayment = true;
            oRowPaid.CLCouponID = 0;
            oRowPaid.SigType = "";
            oRowPaid.PaymentProcessor = clsPOSDBConstants.NOPROCESSOR;
            oPosPTList.oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);
            calculatePaidAmount();
        }
        #endregion

        #region StoreCredit PRIMEPOS-2747
        private void StoreCreditProcess(KeyEventArgs e)
        {
            logger.Trace("StoreCreditProcess(KeyEventArgs e)" + clsPOSDBConstants.Log_Entering);
            Customer oCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();
            CustomerRow oCustRow = null;
            string strCode = "";

            if (oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.AccountNumber == -1)
            {
                logger.Trace("StoreCreditProcess(KeyEventArgs e) - frmCustomerSearch - Entered");
                frmCustomerSearch oSearch = new frmCustomerSearch(oPosPTList.oCurrentCustRow.AccountNumber.ToString(), false);
                oSearch.ActiveOnly = 1;
                //oSearch.IsStoreCredit = true;
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    oSearch.IsStoreCredit = false;
                }
                else
                {
                    oSearch.IsStoreCredit = true;
                }
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        return;
                    }

                    oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
                    if (oCustdata.Tables[0].Rows.Count == 0)
                    {
                        oCustRow = oSearch.SelectedRow();
                        oPosPTList.GetAllDeliveryAddress(oCustomer, oCustdata, oCustRow, ref strCode);
                    }
                    else
                    {
                        oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                        oPosPTList.oCurrentCustRow = oCustRow;
                        posTrans.oCustomerRow = oCustRow;

                        CustomerTag = posTrans.oCustomerRow.CustomerId;
                        CustomerText = posTrans.oCustomerRow.AccountNumber.ToString();
                        CustomerName = posTrans.oCustomerRow.CustomerFullName;
                        this.lblCustomerName.Text = CustomerName;//NileshJ
                    }
                }
                logger.Trace("StoreCreditProcess(KeyEventArgs e) - frmCustomerSearch completed");
            }

            if (oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.AccountNumber != -1)
            {
                decimal remAmount = 0;
                decimal creditAmount = 0;
                decimal storeCreditAmount = 0;
                if (oStoreCreditData != null)
                {
                    oStoreCreditData.StoreCredit.Rows.Clear();
                }
                oStoreCreditData = oStoreCredit.GetByCustomerID(Convert.ToInt32(posTrans.oCustomerRow.CustomerId));
                CustomerTag = posTrans.oCustomerRow.CustomerId;
                CustomerText = posTrans.oCustomerRow.AccountNumber.ToString();
                CustomerName = posTrans.oCustomerRow.CustomerFullName;
                this.lblCustomerName.Text = CustomerName;//NileshJ
                if (oStoreCreditData.Tables[0].Rows.Count > 0)
                {
                    if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
                    {
                        if (Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]) != 0)
                        {
                            storeCreditAmount = Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]);
                            //creditAmount = Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]) - Convert.ToDecimal(grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);    //PRIMEPOS-2938 27-Jan-2021 JY Commented
                            //remAmount = Convert.ToDecimal(grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value) - Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]);   //PRIMEPOS-2938 27-Jan-2021 JY Commented
                            creditAmount = Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]) - Configuration.convertNullToDecimal(grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)); //PRIMEPOS-2938 27-Jan-2021 JY Added
                            remAmount = Configuration.convertNullToDecimal(grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) - Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]);    //PRIMEPOS-2938 27-Jan-2021 JY Added

                            if (creditAmount.ToString().Contains("-"))
                            {
                                oStoreCreditData.Tables[0].Rows[0]["CreditAmt"] = "0.00";
                            }
                            else
                            {
                                oStoreCreditData.Tables[0].Rows[0]["CreditAmt"] = Math.Abs(creditAmount);
                            }
                            oPosPTList.oStoreCreditData = oStoreCreditData;
                            posTrans.oStoreCreditData = oStoreCreditData;   //PRIMEPOS-2938 28-Jan-2021 JY Added

                            if (remAmount.ToString().Contains("-"))
                            {
                                AddRowToPaid(this.grdPayment.ActiveRow);
                                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = "0.00";
                            }
                            else
                            {
                                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = storeCreditAmount.ToString("F"); // Convert.ToDecimal(grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value);
                                AddRowToPaid(this.grdPayment.ActiveRow);
                                grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = Math.Abs(remAmount);

                                //this.grdPayment.Rows[0].Activate();
                                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                            }
                            foreach (UltraGridRow row in grdPayment.Rows)
                            {
                                switch (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Value.ToString().Trim())
                                {
                                    case "X":
                                        {
                                            if (row.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value.ToString().Trim() == "")
                                            {
                                                //row.Hidden = true;
                                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_CCName].Activation = Activation.Disabled;
                                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.Disabled;
                                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.NoEdit;
                                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_CardType].Activation = Activation.Disabled;
                                                row.Cells[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].Activation = Activation.Disabled;
                                                row.Cells[clsPOSDBConstants.PayType_Fld_PayTypeDescription].Activation = Activation.Disabled;
                                            }
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            Resources.Message.Display("No Store Credit available", "Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            this.grdPayment.Rows[0].Activate();
                            this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                            this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                            grdPayment.PerformAction(UltraGridAction.PrevCell, false, false);
                            grdPayment.PerformAction(UltraGridAction.EnterEditMode, false, false);
                            grdPayment.Refresh();
                            return;
                        }
                    }
                    else if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                    {
                        if (grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value.ToString().Contains("-"))
                        {
                            creditAmount = Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]) - Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw));
                        }
                        else
                        {
                            creditAmount = Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]) + Convert.ToDecimal(grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value);
                        }
                        if (creditAmount.ToString().Contains("-"))
                        {
                            oStoreCreditData.Tables[0].Rows[0]["CreditAmt"] = "0.00";
                        }
                        else
                        {
                            oStoreCreditData.Tables[0].Rows[0]["CreditAmt"] = Math.Abs(creditAmount);
                        }
                        oPosPTList.oStoreCreditData = oStoreCreditData;
                        posTrans.oStoreCreditData = oStoreCreditData;   //PRIMEPOS-2938 28-Jan-2021 JY Added

                        AddRowToPaid(this.grdPayment.ActiveRow);
                        this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Value = (Configuration.convertNullToDecimal(this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].GetText(MaskMode.Raw)) +
                        Convert.ToDecimal((oPosPTList.amountBalanceDue))).ToString("###########0.00");
                        this.grdPayment.Update();
                    }
                }
                else
                {
                    if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                    {
                        AddRowToPaid(this.grdPayment.ActiveRow);
                    }
                    else
                    {
                        Resources.Message.Display("No Store Credit available", "Payment Process", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        int SelectedIndex = 0;
                        DataRow[] result = oPosPTList.oPOSTransPaymentData.Tables[0].Select("TransTypeCode = 'X'");
                        if (result.Length > 0)
                        {
                            SelectedIndex = oPosPTList.oPOSTransPaymentData.Tables[0].Rows.IndexOf(result[0]);
                        }

                        this.grdPayment.Rows[SelectedIndex].Activate();
                        this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                        this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activate();

                        this.grdPayment.Rows[SelectedIndex].Activate();
                        this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                        this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                        return;
                    }
                }


                //this.grdPayment.Rows[0].Activate();
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.Disabled;
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activate();



                IsStoreCredit = true;
                logger.Trace("StoreCreditProcess(KeyEventArgs e)" + clsPOSDBConstants.Log_Exiting);
            }
            else
            {
                return;
            }
            //IsStoreCredit = true;
        }

        public void StoreCreditCalculation()
        {
            logger.Trace("StoreCreditCalculation()" + clsPOSDBConstants.Log_Entering);
            if (oStoreCreditData != null)
            {
                oStoreCreditData.StoreCredit.Rows.Clear();
            }

            oStoreCreditData = oStoreCredit.GetByCustomerID(Convert.ToInt32(posTrans.oCustomerRow.CustomerId));
            if (oStoreCreditData.Tables[0].Rows.Count > 0 && Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]).ToString("###########0.00") != "0.00" && oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.Sales)
            {
                int SelectedIndex = 0;
                DataRow[] result = oPosPTList.oPOSTransPaymentData.Tables[0].Select("TransTypeCode = 'X'");
                if (result.Length > 0)
                {
                    SelectedIndex = oPosPTList.oPOSTransPaymentData.Tables[0].Rows.IndexOf(result[0]);
                }

                this.grdPayment.Rows[SelectedIndex].Activate();
                this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                this.grdPayment.Rows[SelectedIndex].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = "Available Credit: $" + Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]).ToString("###########0.00");
            }
            else
            {
                //this.grdPayment.Rows[0].Activate();
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activation = Activation.AllowEdit;
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_Amount].Activate();
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activation = Activation.AllowEdit;
                //this.grdPayment.Rows[0].Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Activate();
            }
            logger.Trace("StoreCreditCalculation()" + clsPOSDBConstants.Log_Exiting);
        }

        private void btnStoreTransactionDetails_Click(object sender, EventArgs e)
        {
            getStoreCreditTransactionDetails();
        }

        private void getStoreCreditTransactionDetails()
        {
            if (oPosPTList.oCurrentCustRow != null && oPosPTList.oCurrentCustRow.AccountNumber == -1)
            {
                oPosPTList.oCurrentCustRow.AccountNumber = -1;
                string strCode = "";
                Customer oCustomer = new Customer();
                CustomerData oCustdata = new CustomerData();
                CustomerRow oCustRow = null;
                logger.Trace("getStoreCreditTransactionDetails() - Entered");
                frmCustomerSearch oSearch = new frmCustomerSearch(oPosPTList.oCurrentCustRow.AccountNumber.ToString(), false);
                oSearch.ActiveOnly = 1;
                //oSearch.IsStoreCredit = true;
                if (oPosPTList.oTransactionType == POS_Core.TransType.POSTransactionType.SalesReturn)
                {
                    oSearch.IsStoreCredit = false;
                }
                else
                {
                    oSearch.IsStoreCredit = true;
                }
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                    {
                        return;
                    }

                    oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
                    if (oCustdata.Tables[0].Rows.Count == 0)
                    {
                        oCustRow = oSearch.SelectedRow();
                        oPosPTList.GetAllDeliveryAddress(oCustomer, oCustdata, oCustRow, ref strCode);
                    }
                    else
                    {
                        oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                        oPosPTList.oCurrentCustRow = oCustRow;
                        posTrans.oCustomerRow = oCustRow;

                        CustomerTag = posTrans.oCustomerRow.CustomerId;
                        CustomerText = posTrans.oCustomerRow.AccountNumber.ToString();
                        CustomerName = posTrans.oCustomerRow.CustomerFullName;
                        this.lblCustomerName.Text = CustomerName;

                    }
                    StoreCreditCalculation();
                    frmViewStoreCreditDetails ofrmViewStoreCreditDetails = new frmViewStoreCreditDetails(strCode, this.lblCustomerName.Text);
                    ofrmViewStoreCreditDetails.ShowDialog();
                    IsStoreCredit = true;
                }
                logger.Trace("getStoreCreditTransactionDetails() - Exited");
            }
            else
            {
                frmViewStoreCreditDetails ofrmViewStoreCreditDetails = new frmViewStoreCreditDetails(oPosPTList.oCurrentCustRow.AccountNumber.ToString(), this.lblCustomerName.Text);
                ofrmViewStoreCreditDetails.ShowDialog();
                IsStoreCredit = true;
            }
        }
        #endregion

        private string CalculateTaxAmount(string Amount, out string ActualAmount)
        {
            try
            {
                if (Configuration.CPOSSet.PaymentProcessor == "EVERTEC")
                {
                    string cityTaxAmount = string.Empty;
                    string stateTaxAmount = string.Empty;
                    string reduceStateTaxAmount = string.Empty;
                    string reduceCityTaxAmount = string.Empty;//PRIMEPOS-3099
                    string reduceStateTaxPercent = string.Empty;//PRIMEPOS-3099
                    string totalAmount = string.Empty;
                    string totalTaxAmount = string.Empty;
                    DataTable dt = (DataTable)Newtonsoft.Json.JsonConvert.DeserializeObject(EvertecTaxDetails, typeof(DataTable));

                    totalAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalAmount")).FirstOrDefault();
                    totalTaxAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalTaxAmount")).FirstOrDefault();
                    //cityTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                    cityTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.Municipality && a.Field<string>("TaxCode").ToUpper().Contains("PRM")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                    stateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                    reduceStateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                    reduceStateTaxPercent = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxPercent")).SingleOrDefault();//PRIMEPOS-3099 
                    if (Convert.ToDecimal(Amount) > Convert.ToDecimal(totalAmount))
                    {
                        ActualAmount = totalAmount = Amount;//PRIMEPOS-3099
                    }
                    if (Convert.ToDecimal(totalTaxAmount) != 0)
                    {
                        decimal currentTotalTax = Math.Round((Convert.ToDecimal(Amount) * Convert.ToDecimal(totalTaxAmount)) / Convert.ToDecimal(totalAmount), 2);
                        stateTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(stateTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
                        cityTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(cityTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
                        reduceStateTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(reduceStateTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
                        #region PRIMEPOS-3099
                        if (Convert.ToDecimal(reduceStateTaxPercent) == 0)
                        {
                            reduceCityTaxAmount = "0.00";
                        }
                        else
                        {
                            reduceCityTaxAmount = ((Convert.ToDecimal(reduceStateTaxAmount) * 100) / Convert.ToDecimal(reduceStateTaxPercent)).ToString();
                        }
                        #endregion
                        ActualAmount = Amount;
                        return stateTaxAmount + "|" + cityTaxAmount + "|" + reduceStateTaxAmount + "|" + reduceCityTaxAmount;//PRIMEPOS-3099
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            ActualAmount = Amount;
            return string.Empty;
        }

        #region PRIMEPOS-2889 01-Sep-2020 JY Added
        private void PressRKeyOnKeyDown()
        {
            if (this.grdPayment.ContainsFocus == true)
            {
                if (this.grdPayment.ActiveCell != null && this.grdPayment.ActiveCell.Column.Key == clsPOSDBConstants.POSTransPayment_Fld_Amount)
                {
                    getStoreCreditTransactionDetails();
                }
            }
        }
        #endregion

        #region PRIMEPOS-3117 11-Jul-2022 JY Added
        private bool GetTransFeeApplicableFor(out decimal OTCAmount, out decimal RxAmount)
        {
            bool bReturn = false;
            OTCAmount = 0;
            RxAmount = 0;
            if (Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "" || Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "0")  //None
                bReturn = false;
            else if (Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "1")   //All
                bReturn = true;
            else if (oPosPTList.isROA == true)
                bReturn = true;
            else
            {
                if (oPosTrans.oTransDData != null && oPosTrans.oTransDData.Tables.Count > 0 && oPosTrans.oTransDData.TransDetail.Rows.Count > 0)
                {
                    foreach (TransDetailRow oTransDetailRow in oPosTrans.oTransDData.TransDetail.Rows)
                    {
                        if (Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "2" && oTransDetailRow.IsRxItem == false)   //OTC
                        {
                            OTCAmount += Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.ExtendedPrice)) + Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.TaxAmount)) - Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.Discount)) - Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.InvoiceDiscount));
                            bReturn = true;
                        }
                        else if (Configuration.convertNullToString(Configuration.CSetting.TransactionFeeApplicableFor).Trim() == "3" && oTransDetailRow.IsRxItem == true)   //Rx
                        {
                            RxAmount += Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.ExtendedPrice)) + Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.TaxAmount)) - Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.Discount)) - Math.Abs(Configuration.convertNullToDecimal(oTransDetailRow.InvoiceDiscount));
                            bReturn = true;
                        }
                    }
                }
            }
            return bReturn;
        }
        #endregion
    }

    //public class F10TransType
    //{
    //    public const string ROA = "ROA";
    //    public const string RXTrans = "RX";
    //    public const string NonRXTrans = "NONRX";
    //}
}