using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMile
{
    public static  class GlobalConstantTags
    {
        

        #region "Hide Tab Tags"

        public  const string HIDE_GENERAL = "/HideGeneral:";
        public  const string HIDE_RECIEPT = "/HideReceipt:";
        public  const string HIDE_ACH = "/HideACH:";
        public  const string HIDE_HARDWARE = "/HideHardware:";
        public  const string HIDE_CHECKSCANNER = "/HideCheckScanner:";
        public  const string HIDE_SIGPAD = "/HideSignaturePad:";
        public  const string HIDE_CREDITAPPLICATION = "/HideCreditApplication:";
        public const string HIDE_AUTOUPDATE = "/HideAutoUpdate:";


        #endregion

        #region " Merchant Settings Tag"

        public const string ACCT_ID = "/ATSID:";
        public const string SUB_ID = "/ATSSubID:";
        public const string MERCHANT_PIN = "/MerchantPIN:";


        #endregion

        #region "Processing Settings and tags"

        public const string SWIPE_IMMEDIATE = "/SwipeImmediate:";
        public const string PROCESS_ON_SWIPE = "/ProcessOnSwipe:";
        public const string PROCESS_IMMEDIATE = "/ProcessImmediate:";

        public const string MEMO = "/Memo:";
        public const string CENTER_SCREEN = "/CenterScreen";
        public const string LOCK_PARAMETERS = "/LockParameter:";

        public const string DIALOG_SUPRESS_ACCEPTED = "/SupressAcceptedDialog:";
        public const string DIALOG_SUPRESS_PARTIAL_ACCEPTED = "/SupressPartialAcceptedDialog:";
        public const string DIALOG_ACCEPTED_TIMEOUT = "/AcceptedDialogTimeout:";
        public const string DISABLE_OVERRIDE_PIN = "/DisableOverridePIN:";

        #endregion

        #region "Common Transactaction Tags"
        public const string TRANS_TYPE = "/TransactionType:";
        public const string TRANS_AMOUNT = "/Amount:";
        public const string TRANS_MERCHANT_ORDER_NUMBER = "/SwipeImmediate:";
        public const string TRANS_REF_CODE = "/RefCode:";
        public const string TRANS_ORDER_ID = "/OrderID:";
        public const string TRANS_ID = "/TransactionID:";

        #endregion

        



        #region "Credit Card Tag"

        public const string CC_REQUIRE_TAX = "/RequireTax:";
        public const string CC_TAX_AMOUNT = "/TaxAmount:";
        public const string CC_TAX_EXEMPT = "/TaxExempt";
        public const string CC_CASHBACK_AMOUNT = "/TransactionID:";


        public const string CC_DISABLE_CASH_BACK = "/DisableCashBack:";

        public const string CC_EXIT_ON_DECLINE = "/ExitOnDecline:";
        public const string CC_EXIT_ON_CANCELLED_SWIPE = "/ExitOnCancelledSwipe:";

        public const string CC_ISV_EMV = "/ISVEMV:";

        //Tokenization
        public const string CC_ALLOW_TOKENIZATION = "/AllowTokenization:";
        public const string CC_TOKENIZE = "/Tokenize:";
        public const string CC_TOKEN = "/Token:";
        public const string CC_LAST4 = "/Last4Digits:";


        #endregion

        #region "Signature Image Tags"

        public const string SIG_RETURN_IMAGE_ENCODING = "/ReturnImageEncoding:";
        public const string SIG_RETURN_IMAGE_FORMAT = "/ReturnImageFormat:";
        public const string SIG_RETURN_IMAGE_HEIGHT = "/ReturnImageHeight:";
        public const string SIG_RETURN_IMAGE_WIDTH = "/ReturnImageWidth:";
        public const string SIG_PROMPT_FOR_SIGNATURE = "/SignaturePrompt:";
        public const string SIG_DEBIT_OPTION = "/SignatureOptional:";

        #endregion

        #region "Fsa and EBT Related Tags"
        public const string TRANSACTION_SUB_TYPE = "/TransactionSubType:";

        public const string FSA_ENABLE = "/EnableFSA:";        
        public const string FSA_CLINIC_AMOUNT = "/ClinicAmount:";
        public const string FSA_RX_AMOUNT = "/RXAmount:";
        public const string FSA_DENTAL_AMOUNT = "/DentalAmount:";
        public const string FSA_VISION_AMOUNT = "/VisionAmount:";

        public const string EBT_ENABLE = "/EnableEBT:";
        public const string EBT_APPROVAL_CODE = "/EBTApprovalCode:";
        public const string EBT_VOUCHER_SERIAL_NUMBER = "/VoucherSerialNumber:";

        public const string EBT = "EBT";
        public const string EBT_CBS = "EBT_CashBenefitSale";
        public const string EBT_FOODSTAMPSALE = "EBT_FoodStampSale";
        public const string EBT_FOODSTAMP_VOUCHER_SALE = "EBT_FoodStampVoucherSale";
        public const string EBT_CB_WITHDRAW = "EBT_CashBenefitWithdrawal";
        public const string EBT_BALANCEINQUIRY = "EBT_BalanceInquiry";
        public const string EBT_CB_BALANCEINQUIRY = "EBT_CashBenefitBalanceInquiry";
        public const string EBT_FOODSTAMP_BALANCEINQUIRY = "EBT_FoodStampBalanceInquiry";

        public const string EBT_CB_RETURN = "EBT_CashBenefitReturn";
        public const string EBT_FOODSTAMP_RETURN = "EBT_FoodStampReturn";


        #endregion


    }

    /// <summary>
    /// Defines the diffrent Main Transaction Types Supported
    /// </summary>
    public enum TransactionType
    {
        Sale,
        PreAuth,
        PostAuth,
        VAC,
        Refund,
        Void,
        Credit,
        BillPayment,
        AccountPayment,
        AccountPaymentReversal,
        None
    };

    /// <summary>
    /// Specifies the Encoding Format Used for Signature Image Files
    /// </summary>
    public enum EncodingFormat
    {
        Base64,
        BinHex,
        None
    };

    /// <summary>
    /// Specifies the Image format of Signature
    /// </summary>
    public enum ImageFormat
    {
        Bmp,
        Emf,
        Gif,
        Jpeg,
        Pcl,
        Png,
        Tiff,
        Wmf,
        None
    };

    public enum MMSFlag
    {
        No=0,
        Yes=1
    };

    /// <summary>
    /// Specifies the Sub Transaction type Mainly used for EBT and FSA Otherwise Set to None
    /// </summary>
    public enum TransactionSubType
    {
        EBT,
        EBT_CashBenifitSale,
        EBT_FoodStampSale,
        EBT_FoodStampVoucherSale,
        EBT_CashBenifitWithdrawal,
        EBT_BalanceInquiry,
        EBT_CashBenifitBalanceInquiry,
        EBT_FoodStampBalanceInquiry,
        EBT_FoodStampReturn,
        EBT_CashBenifitReturn,
        FSA,
        None

    };

}
