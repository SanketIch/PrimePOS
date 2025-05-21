using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log.Net;
using ATSSecurePostUILib;
using NLog;
using PossqlData;

namespace FirstMile
{
    public class ProcessCard
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        #region "Private Vars"
        private TransactionType _TransType;
        private decimal _Amount = 0;
        private string _MerchantOrderNumber;
        private string _RefCode;
        private string _OrderID;
        private string _TransactionID;

        private bool _RequireTax;
        private decimal _TaxAmount = 0;
        private bool _TaxExempt;

        private decimal _CashBackAmount;

        private bool _AllowTokenization;
        private bool _Tokenize;
        private string _Token;
        private string _Last4Digits;

        private EncodingFormat _ReturnImgEncoding;
        private ImageFormat _ReturnImgType;
        private int _ReturnImageHeight;
        private int _ReturnImageWidth;
        private string _SignaturePrompt;
        private bool _SignatureOptional;

        private bool _IsFSA;

        private bool _IsEBT;
        private TransactionSubType _TransSubType;
        private string _EBTApprovalCode;
        private string _VoucherSerialNumber;
        private decimal _ClinicalAmount = 0;
        private decimal _RXAmount = 0;
        private decimal _DentalAmount = 0;
        private decimal _VisionAmount = 0;

        #region PRIMEPOS-2761
        private string _TicketNo;
        private string _StationID;
        private string _UserId;
        #endregion

        #endregion

        #region "Public properties"
        /// <summary>
        /// Enter the Transaction type.Supported Transaction Types are Sale, PreAuth, AuthReversal,PostAuth,VAC,Refund,Void,
        /// Credit,BillPayment,AccountPayment,AccountPaymentReversal
        /// </summary>
        public TransactionType TransType
        {
            //get
            //{
            //    return _TransType;
            //}

            set
            {
                _TransType = value;
            }
        }

        /// <summary> 
        /// Dollar Amount of the Transaction in the format 0.00
        /// </summary>
        public decimal Amount
        {
            //get
            //{
            //    return _Amount;
            //}

            set
            {
                _Amount = value;
            }
        }

        /// <summary>
        /// Optional:
        /// Merchant order number Provided By WP
        /// A unique value that can be used to reference transactions in WP Online Merchant Center
        /// </summary>
        public string MerchantOrderNumber
        {
            //get
            //{
            //    return _MerchantOrderNumber;
            //}

            set
            {
                _MerchantOrderNumber = value;
            }
        }

        /// <summary>
        /// Refenrence Code from a Pre Authentication transaction
        /// Only applicable to Trans Type : PreAuth
        /// </summary>
        public string RefCode
        {
            //get
            //{
            //    return _RefCode;
            //}

            set
            {
                _RefCode = value;
            }
        }

        /// <summary>
        /// Applicable to Auth Reversal, Refund and Void Treansaction
        /// Order Id of a Previous Transaction
        /// </summary>
        public string OrderID
        {
            //get
            //{
            //    return _OrderID;
            //}

            set
            {
                _OrderID = value;
            }
        }

        /// <summary>
        /// Applicable to Auth Reversal, Refund and Void Treansaction
        /// Transaction Id of a Previous Transaction
        /// </summary>
        public string TransactionID
        {
            //get
            //{
            //    return _TransactionID;
            //}

            set
            {
                _TransactionID = value;
            }
        }

        /// <summary>
        /// if True, the user has to specify Tax Amount or tax Excempt 
        /// </summary>
        public bool RequireTax
        {
            //get
            //{
            //    return _RequireTax;
            //}

            set
            {
                _RequireTax = value;
            }
        }

        /// <summary>
        /// Specifies the Tax Amount
        /// </summary>
        public decimal TaxAmount
        {
            //get
            //{
            //    return _TaxAmount;
            //}

            set
            {
                _TaxAmount = value;
            }
        }

        /// <summary>
        /// Shows if the Transaction is Tax Exempt
        /// </summary>
        public bool TaxExempt
        {
            //get
            //{
            //    return _TaxExempt;
            //}

            set
            {
                _TaxExempt = value;
            }
        }

        /// <summary>
        /// Cash Back Dollar Amount in the Format 0.00
        /// </summary>
        public decimal CashBackAmount
        {
            //get
            //{
            //    return _CashBackAmount;
            //}

            set
            {
                _CashBackAmount = value;
            }
        }

        /// <summary>
        /// Set to True to Allow Tokenisation (Saving Credit card Profile )
        /// </summary>
        public bool AllowTokenization
        {
            //get
            //{
            //    return _AllowTokenization;
            //}

            set
            {
                _AllowTokenization = value;
            }
        }

        /// <summary>
        /// Set to true to Tokenize the Current Transaction 
        /// </summary>
        public bool Tokenize
        {
            //get
            //{
            //    return _Tokenize;
            //}

            set
            {
                _Tokenize = value;
            }
        }

        /// <summary>
        /// Required When Processing a Transaction using a Token
        /// Gets The Token (Card Profile ID) Genrated  to Process the current Tranascation
        /// </summary>
        public string Token
        {
            //get
            //{
            //    return _Token;
            //}

            set
            {
                _Token = value;
            }
        }

        /// <summary>
        /// Required When Processing a Transaction using a Token
        /// Gets the Last For digits of the Card Number the token Belongs to
        /// </summary>
        public string Last4Digits
        {
            //get
            //{
            //    return _Last4Digits;
            //}

            set
            {
                _Last4Digits = value;
            }
        }

        /// <summary>
        /// Specify the Encoding Format for the Signature Image
        /// Supported Parameters are Base64 (Radix-64 Encoding) and BinHex (Binary to Hexadecimal Encoding)
        /// Default is Base64
        /// </summary>
        public EncodingFormat ReturnImgEncoding
        {
            //get
            //{
            //    return _ReturnImgEncoding;
            //}

            set
            {
                _ReturnImgEncoding = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ImageFormat ReturnImgType
        {
            //get
            //{
            //    return _ReturnImgType;
            //}

            set
            {
                _ReturnImgType = value;
            }
        }

        public int ReturnImageHeight
        {
            //get
            //{
            //    return _ReturnImageHeight;
            //}

            set
            {
                _ReturnImageHeight = value;
            }
        }

        public int ReturnImageWidth
        {
            //get
            //{
            //    return _ReturnImageWidth;
            //}

            set
            {
                _ReturnImageWidth = value;
            }
        }

        public string SignaturePrompt
        {
            //get
            //{
            //    return _SignaturePrompt;
            //}

            set
            {
                _SignaturePrompt = value;
            }
        }

        public bool SignatureOptional
        {
            //get
            //{
            //    return _SignatureOptional;
            //}

            set
            {
                _SignatureOptional = value;
            }
        }

        public bool IsFSA
        {
            set
            {
                _IsFSA = value;
            }
        }

        public bool IsEBT
        {
            //get
            //{
            //    return _IsEBT;
            //}

            set
            {
                _IsEBT = value;
            }
        }

        public TransactionSubType TransSubType
        {
            //get
            //{
            //    return _TransSubType;
            //}

            set
            {
                _TransSubType = value;
            }
        }

        public string EBTApprovalCode
        {
            //get
            //{
            //    return _EBTApprovalCode;
            //}

            set
            {
                _EBTApprovalCode = value;
            }
        }

        public string VoucherSerialNumber
        {
            //get
            //{
            //    return _VoucherSerialNumber;
            //}

            set
            {
                _VoucherSerialNumber = value;
            }
        }

        public decimal ClinicalAmount
        {
            //get
            //{
            //    return _ClinicalAmount;
            //}

            set
            {
                _ClinicalAmount = value;
            }
        }

        public decimal RXAmount
        {
            //get
            //{
            //    return _RXAmount;
            //}

            set
            {
                _RXAmount = value;
            }
        }

        public decimal DentalAmount
        {
            //get
            //{
            //    return _DentalAmount;
            //}

            set
            {
                _DentalAmount = value;
            }
        }

        public decimal VisionAmount
        {
            //get
            //{
            //    return _VisionAmount;
            //}

            set
            {
                _VisionAmount = value;
            }
        }

        #region PRIMEPOS-2761 
        public string TicketNo
        {
            set
            {
                _TicketNo = value;
            }
        }
        public string StationID
        {
            set
            {
                _StationID = value;
            }
        }
        public string UserId
        {
            set
            {
                _UserId = value;
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// This Method Gets all the necessary data, Creates the Necessary tags, Calls the FirstMile application to process the transaction 
        /// and Returns the result as a Delimited String
        /// </summary>
        /// <returns> The Transaction result as a Delimited String </returns>
        public string DoTransaction(bool isManual = false) // PRIMEPOS-2737 - bool isManual Addded by NileshJ for allow first mile manual transaction )
        {
            logger.Debug("Starting Transaction");
            bool bHasAmt = false;
            string result = string.Empty;
            StringBuilder otagbuilder = new StringBuilder();


            try
            {
                // Tags For Gate way settings
                #region Gateway Settings
                logger.Debug("Fetching Gateway Settings");
                otagbuilder.Append(GatewaySettings.GetGatewayTags());
                #endregion

                //Transaction Amount
                #region Amount 
                logger.Debug("Setting the Amount " + _Amount.ToString("C"));
                if (_Amount > 0)
                {
                    otagbuilder.Append(GlobalConstantTags.TRANS_AMOUNT + string.Format("{0:F}", _Amount));
                    bHasAmt = true;

                }
                else
                {
                    logger.Error("Amount is less than or equal Zero: " + _Amount.ToString());
                }
                #endregion

                #region Tax Fields
                logger.Debug("Checking Tax");
                if (_RequireTax)
                {
                    logger.Debug("Requires Tax");
                    otagbuilder.Append(GlobalConstantTags.CC_REQUIRE_TAX + "1");

                    if (_TaxExempt)
                    {
                        logger.Debug("Transaction is Tax Exempt");
                        otagbuilder.Append(GlobalConstantTags.CC_TAX_EXEMPT + "1");
                    }
                    else
                    {
                        logger.Debug("Tax Amount " + _TaxAmount);
                        otagbuilder.Append(GlobalConstantTags.CC_TAX_AMOUNT + string.Format("{0:F}", _TaxAmount));
                    }

                }
                else
                {
                    logger.Debug("Transaction Does not Require Tax");
                    otagbuilder.Append(GlobalConstantTags.CC_REQUIRE_TAX + "0");
                }

                #endregion

                #region Behavior Related Fields
                logger.Debug("Setting properties for Controlling UI Behaviour");

                if ((_Amount > 0 && _RequireTax && _TaxExempt) || (_Amount > 0 && _RequireTax && _TaxAmount > 0) || (_Amount > 0 && _RequireTax == false))
                {
                    logger.Debug("Locking amount and tax Parameters In FirstMile UI");
                    otagbuilder.Append(GlobalConstantTags.LOCK_PARAMETERS + "1");
                }
                else
                {
                    logger.Debug("Leaving Amount and Tax parameters in FirstMile UI Editable");
                    otagbuilder.Append(GlobalConstantTags.LOCK_PARAMETERS + "0");
                }

                logger.Debug("Supressing Accepted and Partial Accepted Messages");
                otagbuilder.Append(GlobalConstantTags.DIALOG_SUPRESS_ACCEPTED + "1");
                otagbuilder.Append(GlobalConstantTags.DIALOG_SUPRESS_PARTIAL_ACCEPTED + "1");

                logger.Debug("Exit On Decline and Cancelled Swipe");
                otagbuilder.Append(GlobalConstantTags.CC_EXIT_ON_DECLINE + "1");
                otagbuilder.Append(GlobalConstantTags.CC_EXIT_ON_CANCELLED_SWIPE + "1");

                #endregion

                #region Return EMV Fields
                logger.Debug("Telling First Mile to return EMV Fields");
                otagbuilder.Append(GlobalConstantTags.CC_ISV_EMV + "1");


                #endregion

                #region Transaction Type Specific Fields
                logger.Debug("Setting Transaction Type");
                switch (_TransType)
                {
                    case TransactionType.Sale:
                        logger.Debug("Sale Transaction");
                        otagbuilder.Append(GlobalConstantTags.TRANS_TYPE + "Sale");
                        if (_CashBackAmount > 0)
                        {
                            logger.Debug("CashBack amt :" + _CashBackAmount.ToString("C"));
                            otagbuilder.Append(GlobalConstantTags.CC_CASHBACK_AMOUNT + string.Format("{0:F}", _CashBackAmount));
                        }
                        if (bHasAmt)
                        {
                            logger.Debug("Setting First Mile to Launch in Swipe ready mode");
                            if (!isManual) // PRIMEPOS-2737 - condition of isManual Addded by NileshJ for allow first mile manual transaction 
                            {
                                otagbuilder.Append(GlobalConstantTags.SWIPE_IMMEDIATE + "1");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_ON_SWIPE + "1");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "1");
                            }
                            else
                            {
                                otagbuilder.Append(GlobalConstantTags.SWIPE_IMMEDIATE + "0");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_ON_SWIPE + "0");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "0");
                            }
                        }

                        break;
                    case TransactionType.PreAuth:
                        break;
                    case TransactionType.PostAuth:
                        break;
                    case TransactionType.VAC:
                        break;
                    case TransactionType.Refund:
                        logger.Debug("Perform Refund on an Earlier Transaction");
                        otagbuilder.Append(GlobalConstantTags.TRANS_TYPE + "Refund");

                        otagbuilder.Append(GlobalConstantTags.TRANS_ORDER_ID + _OrderID);
                        otagbuilder.Append(GlobalConstantTags.TRANS_ID + _TransactionID);

                        if (bHasAmt)
                        {
                            if (!isManual) // PRIMEPOS-2737 - condition of isManual Addded by NileshJ for allow first mile manual transaction 
                            {
                                otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "1");
                                otagbuilder.Append(GlobalConstantTags.SWIPE_IMMEDIATE + "1");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_ON_SWIPE + "1");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "1");
                            }
                            else
                            {
                                otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "0");
                                otagbuilder.Append(GlobalConstantTags.SWIPE_IMMEDIATE + "0");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_ON_SWIPE + "0");
                                otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "0");
                            }

                        }
                        break;
                    case TransactionType.Void:
                        logger.Debug("Void a Previous Transation");
                        otagbuilder.Append(GlobalConstantTags.TRANS_TYPE + "Void");

                        otagbuilder.Append(GlobalConstantTags.TRANS_ORDER_ID + _OrderID);
                        otagbuilder.Append(GlobalConstantTags.TRANS_ID + _TransactionID);

                        if (bHasAmt)
                        {
                            otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "1");

                        }
                        break;
                    case TransactionType.Credit:
                        logger.Debug("Performing a  Stand Alone Credit");
                        otagbuilder.Append(GlobalConstantTags.TRANS_TYPE + "Credit");
                        otagbuilder.Append(GlobalConstantTags.DISABLE_OVERRIDE_PIN + "1");
                        if (!isManual) // PRIMEPOS-2737 - condition of isManual Addded by NileshJ for allow first mile manual transaction 
                        {
                            otagbuilder.Append(GlobalConstantTags.SWIPE_IMMEDIATE + "1");
                            otagbuilder.Append(GlobalConstantTags.PROCESS_ON_SWIPE + "1");
                            otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "1");
                        }
                        else
                        {
                            otagbuilder.Append(GlobalConstantTags.SWIPE_IMMEDIATE + "0");
                            otagbuilder.Append(GlobalConstantTags.PROCESS_ON_SWIPE + "0");
                            otagbuilder.Append(GlobalConstantTags.PROCESS_IMMEDIATE + "0");
                        }
                        break;
                    case TransactionType.BillPayment:
                        break;
                    case TransactionType.AccountPayment:
                        break;
                    case TransactionType.AccountPaymentReversal:
                        break;
                    default:
                        break;

                }

                #endregion

                #region Token
                logger.Debug("Setting Tokenisation related Fields");
                if (_AllowTokenization)
                {
                    logger.Debug("Setting Allow Tokenisation ");
                    otagbuilder.Append(GlobalConstantTags.CC_ALLOW_TOKENIZATION + "1");
                }


                if (_Tokenize)
                {
                    logger.Debug("Tokenize Current Transaction");
                    otagbuilder.Append(GlobalConstantTags.CC_TOKENIZE + "1");
                }

                if (!string.IsNullOrWhiteSpace(_Token))
                {
                    logger.Debug("Processing Transaction using Token ");
                    otagbuilder.Append(GlobalConstantTags.CC_TOKEN + _Token);
                }
                if (!string.IsNullOrWhiteSpace(_Last4Digits))
                {
                    logger.Debug("Adding last 4 digits of the stored profile Credit Card");
                    otagbuilder.Append(GlobalConstantTags.CC_LAST4 + _Last4Digits);
                }
                #endregion

                #region Signature
                logger.Debug("Setting signature options");
                if (!isManual) // PRIMEPOS-2737 - condition of isManual Addded by NileshJ for allow first mile manual transaction 
                {
                    otagbuilder.Append(GlobalConstantTags.SIG_PROMPT_FOR_SIGNATURE + _SignaturePrompt);
                    if (_SignatureOptional)
                    {
                        logger.Debug("Do not Prompt for Signature For Debit Transactions");
                        otagbuilder.Append(GlobalConstantTags.SIG_DEBIT_OPTION + "0");
                    }
                    else
                    {
                        logger.Debug("Prompt for Signature For Debit Transactions");
                        otagbuilder.Append(GlobalConstantTags.SIG_DEBIT_OPTION + "1");
                    }
                }
                else
                {
                    otagbuilder.Append(GlobalConstantTags.SIG_PROMPT_FOR_SIGNATURE + "0");
                    otagbuilder.Append(GlobalConstantTags.SIG_DEBIT_OPTION + "1");
                }


                switch (_ReturnImgEncoding)
                {
                    case EncodingFormat.Base64:
                        logger.Debug("Encoding Format: Base64");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_ENCODING + "Base64");
                        break;
                    case EncodingFormat.BinHex:
                        logger.Debug("Encoding Format: BinHex");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_ENCODING + "BinHex");
                        break;
                    default:
                        logger.Debug("Default Encoding Format: Base64");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_ENCODING + "Base64");
                        break;

                }

                switch (_ReturnImgType)
                {
                    case ImageFormat.Bmp:
                        logger.Debug("Image Format: BMP");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Bmp");
                        break;
                    case ImageFormat.Emf:
                        logger.Debug("Image Format: EMF");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Emf");
                        break;
                    case ImageFormat.Gif:
                        logger.Debug("Image Format: GIF");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Gif");
                        break;
                    case ImageFormat.Jpeg:
                        logger.Debug("Image Format: JPEG");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Jpeg");
                        break;
                    case ImageFormat.Pcl:
                        logger.Debug("Image Format: PCL");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Pcl");
                        break;
                    case ImageFormat.Png:
                        logger.Debug("Image Format: PNG");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Png");
                        break;
                    case ImageFormat.Tiff:
                        logger.Debug("Image Format: TIFF");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Tiff");
                        break;
                    case ImageFormat.Wmf:
                        logger.Debug("Image Format: WMF");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Wmf");
                        break;
                    default:
                        logger.Debug("Default Image Format: PNG");
                        otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_FORMAT + "Png");
                        break;

                }

                if (_ReturnImageHeight > 100)
                {
                    otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_HEIGHT + _ReturnImageHeight.ToString());
                }

                if (_ReturnImageWidth > 500)
                {
                    otagbuilder.Append(GlobalConstantTags.SIG_RETURN_IMAGE_WIDTH + _ReturnImageWidth.ToString());
                }


                #endregion

                #region FSA

                if (_IsFSA)
                {
                    logger.Debug("Transaction Marked as FSA : Setting FSA Flags");
                    otagbuilder.Append(GlobalConstantTags.FSA_ENABLE + "1");
                    otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + "FSA");

                    if (_ClinicalAmount > 0)
                    {
                        logger.Debug("Setting FSA Clinical Amount: " + _ClinicalAmount.ToString("C"));
                        otagbuilder.Append(GlobalConstantTags.FSA_CLINIC_AMOUNT + string.Format("{0:F}", _ClinicalAmount));
                    }

                    if (_RXAmount > 0)
                    {
                        logger.Debug("Setting FSA Rx Amount: " + _RXAmount.ToString("C"));
                        otagbuilder.Append(GlobalConstantTags.FSA_RX_AMOUNT + string.Format("{0:F}", _RXAmount));
                    }

                    if (_DentalAmount > 0)
                    {
                        logger.Debug("Setting FSA Dental Amount: " + _DentalAmount.ToString("C"));
                        otagbuilder.Append(GlobalConstantTags.FSA_DENTAL_AMOUNT + string.Format("{0:F}", _DentalAmount));
                    }

                    if (_VisionAmount > 0)
                    {
                        logger.Debug("Setting FSA Vision Amount: " + _VisionAmount.ToString("C"));
                        otagbuilder.Append(GlobalConstantTags.FSA_VISION_AMOUNT + string.Format("{0:F}", _VisionAmount));
                    }
                }

                #endregion

                #region EBT

                if (_IsEBT)
                {
                    logger.Debug("Transaction Marked as EBT:Setting EBT Flags");
                    otagbuilder.Append(GlobalConstantTags.EBT_ENABLE + "1");
                    switch (_TransSubType)
                    {
                        case TransactionSubType.EBT:
                            logger.Debug("Transaction Type: EBT");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT);
                            break;
                        case TransactionSubType.EBT_CashBenifitSale:
                            logger.Debug("Transaction Type: EBT Cash Benifit Sale");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_CBS);
                            break;
                        case TransactionSubType.EBT_FoodStampSale:
                            logger.Debug("Transaction Type: EBT FoodStamp Sale");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_FOODSTAMPSALE);
                            break;
                        case TransactionSubType.EBT_FoodStampVoucherSale:
                            logger.Debug("Transaction Type: EBT Food Stamp Voucher Sale");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_FOODSTAMP_VOUCHER_SALE);
                            if (!string.IsNullOrEmpty(_EBTApprovalCode))
                            {
                                otagbuilder.Append(GlobalConstantTags.EBT_APPROVAL_CODE + _EBTApprovalCode);
                            }

                            if (!string.IsNullOrEmpty(_VoucherSerialNumber))
                            {
                                otagbuilder.Append(GlobalConstantTags.EBT_VOUCHER_SERIAL_NUMBER + _VoucherSerialNumber);
                            }

                            break;
                        case TransactionSubType.EBT_CashBenifitWithdrawal:
                            logger.Debug("Transaction Type: EBT Cash Benifit WithDrawal");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_CB_WITHDRAW);
                            break;
                        case TransactionSubType.EBT_BalanceInquiry:
                            logger.Debug("Transaction Type: EBT Balance Inquiry");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_BALANCEINQUIRY);
                            break;
                        case TransactionSubType.EBT_CashBenifitBalanceInquiry:
                            logger.Debug("Transaction Type: EBT Cash Benifit Balance Inquiry");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_CB_BALANCEINQUIRY);
                            break;
                        case TransactionSubType.EBT_FoodStampBalanceInquiry:
                            logger.Debug("Transaction Type: EBT Food Stamp Balance Inquiry");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_FOODSTAMP_BALANCEINQUIRY);
                            break;
                        case TransactionSubType.EBT_FoodStampReturn:
                            logger.Debug("Transaction Type: EBT Food Stamp Return");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_FOODSTAMP_RETURN);
                            break;
                        case TransactionSubType.EBT_CashBenifitReturn:
                            logger.Debug("Transaction Type: EBT Cash Benifit Return");
                            otagbuilder.Append(GlobalConstantTags.TRANSACTION_SUB_TYPE + GlobalConstantTags.EBT_CB_RETURN);
                            break;
                        default:
                            break;

                    }
                }

                #endregion

                logger.Trace(otagbuilder.ToString());
                #region PRIMEPOS-2761
                long TransNo = 0;
                using (var db = new Possql())
                {
                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog.TransDateTime = DateTime.Now;
                    cclog.TransAmount = _Amount;
                    cclog.TicketNo = _TicketNo;
                    cclog.TransDataStr = otagbuilder.ToString();
                    cclog.PaymentProcessor = "WORLDPAY";
                    cclog.StationID = _StationID;
                    cclog.UserID = _UserId;
                    cclog.TransmissionStatus = "InProgress";
                    cclog.TransType = _TransType.ToString();
                    db.CCTransmission_Logs.Add(cclog);
                    db.SaveChanges();
                    db.Entry(cclog).GetDatabaseValues();
                    TransNo = cclog.TransNo;
                }
                #endregion

                ATSSecurePostUI oUi = new ATSSecurePostUI();

                result = oUi.ShowCreditCardForm(otagbuilder.ToString());
                System.Threading.Thread.Sleep(3000); //PRIMEPOS-2534 - add this for Lane Close Issue - 29-May-2019 - NileshJ 
                //added by Rohit Nair on 2/14/2017 for Prime POS 2374
                string parseresult = string.Empty;
                if (result.Contains(Helpers.ResponseStrings.SIGNATURE))
                {

                    int index = result.IndexOf(Helpers.ResponseStrings.SIGNATURE);
                    try
                    {
                        parseresult = result.Substring(0, index);
                        logger.Trace(parseresult);
                    }
                    catch (Exception expn)
                    {
                        logger.Error(expn, "Unable to Remove Signature String from result String " + expn.Message);
                        logger.Trace(result);
                        parseresult = result;
                    }
                }
                else
                {
                    logger.Trace(result);
                    parseresult = result;
                }

                try
                {
                    #region PRIMEPOS-2761
                    using (var db = new Possql())
                    {
                        CCTransmission_Log cclog = new CCTransmission_Log();
                        cclog = db.CCTransmission_Logs.Where(w => w.TransNo == TransNo).SingleOrDefault();
                        cclog.AmtApproved = _Amount;
                        cclog.RecDataStr = parseresult;
                        //cclog.HostTransID = _TransactionID;
                        cclog.TransmissionStatus = "Completed";
                        db.CCTransmission_Logs.Attach(cclog);
                        db.Entry(cclog).Property(p => p.RecDataStr).IsModified = true;
                        db.Entry(cclog).Property(p => p.TransmissionStatus).IsModified = true;
                        //db.Entry(cclog).Property(p => p.HostTransID).IsModified = true;
                        db.Entry(cclog).Property(p => p.AmtApproved).IsModified = true;
                        db.SaveChanges();
                    }
                    #endregion
                    // Commented for PRIMEPOS-2761
                    //using (var db = new Possql())
                    //{
                    //    CCTransmission_Log cclog = new CCTransmission_Log();
                    //    cclog.TransDateTime = DateTime.Now;
                    //    cclog.TransAmount = _Amount;
                    //    cclog.TransDataStr = otagbuilder.ToString();
                    //    cclog.RecDataStr = parseresult;

                    //    db.CCTransmission_Logs.Add(cclog);
                    //    db.SaveChanges();
                    //}
                }
                catch (Exception exep)
                {
                    logger.Fatal(exep, exep.Message);
                }





            }
            catch (Exception ex)
            {
                //Logs.Logger(BodyType.Body, ex.ToString(), ErrorLevel.Critical);

                logger.Fatal(ex, "An Error Occured While Processing Transaction " + ex.Message);
            }
            finally
            {
                otagbuilder.Clear();
            }

            // otagbuilder.disp



            return result;
        }



    }
}
