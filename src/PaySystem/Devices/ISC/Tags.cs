using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice
{
    #region Payment Tags
    /// <summary>
    /// Tags for Payment
    /// </summary>
    internal static class Tags
    {
        /// <summary>
        /// Terminal ID
        /// </summary>
        public static string TID = "TID";
        /// <summary>
        /// Merchant ID
        /// </summary>
        public static string MID = "MID";
        /// <summary>
        /// Terminal Serial Number
        /// </summary>
        public static string TSN = "TSN";
        /// <summary>
        /// Encrypted Magnetic swipe Information
        /// </summary>
        public static string EMSI = "EMSI";
        /// <summary>
        /// Use for Tag 57 on MP Gateway
        /// </summary>
        public static string MSID = "MSID";
        /// <summary>
        /// Pin Block
        /// </summary>
        public static string PINB = "PINB";
        /// <summary>
        /// Pin Key 
        /// </summary>
        public static string PKSI = "PKSI";
        /// <summary>
        /// Total Amount
        /// </summary>
        public static string TAMT = "TAMT";
        /// <summary>
        /// Cash Back Amount
        /// </summary>
        public static string CBAMT = "CBAMT";
        /// <summary>
        /// Transaction Payment Type
        /// </summary>
        public static string TPAYT = "TPAYT";
        /// <summary>
        /// Transaction Type
        /// </summary>
        public static string TTYPE = "TTYPE";
        /// <summary>
        /// Device Name
        /// </summary>
        public static string DNAME = "DNAME";
        /// <summary>
        /// FSA RxAmount 
        /// </summary>
        public static string RXAMOUNT = "RXAMOUNT";
        /// <summary>
        /// Tip Amount
        /// </summary>
        public static string TIPAMOUNT = "TIPAMT";
        /// <summary>
        /// EBT Approval Code
        /// </summary>
        public static string EBTACODE = "EBTAPPROVALCODE";
        /// <summary>
        /// EBT Voucher Serial Number
        /// </summary>
        public static string EBTVSN = "EBTVOUCHERSERIALNUMBER";
        /// <summary>
        /// History ID of a sales transaction
        /// </summary>
        public static string HISTID = "HISTID";
        /// <summary>
        /// Order ID of a sales transaction
        /// </summary>
        public static string ORDID = "ORDID";
        /// <summary>
        /// Card Holder Name
        /// </summary>
        public static string MCARDHOLDERNAME = "MCARDHOLDER";
        /// <summary>
        /// Manual Entered Card Number
        /// </summary>
        public static string MCARD_DIGIT = "MCARDDIGIT";
        /// <summary>
        /// Manual Entered Expired Month
        /// </summary>
        public static string MEXP_MONTH = "MEXPMONTH";
        /// <summary>
        /// Manual Entered Expired Year
        /// </summary>
        public static string MEXP_YEAR = "MEXPYEAR";
        /// <summary>
        /// Store Profile Tags
        /// </summary>
        public static string STOREDPROFILE = "STOREDPROFILE";
        /// <summary>
        /// CVV Tags
        /// </summary>
        public static string CVV = "CVV";
        /// <summary>
        /// Last 4 Digits of the Card or ACH
        /// </summary>
        public static string LAST4 = "LAST4";
        /// <summary>
        /// Store Profile Authorize Method
        /// </summary>
        public static string PROFILEAUTHMETHOD = "PAUTHMETHOD";
        /// <summary>
        /// Account Type Credit or ACH
        /// </summary>
        public static string ACCOUNT_TYPE = "ACCOUNTTYPE";
        /// <summary>
        /// Store Profile Authorize Update
        /// </summary>
        public static string PREAUTHUPDATE = "PREAUTHUPDATE";
        /// <summary>
        /// TokenID
        /// </summary>
        public static string TOKENID = "TOKENID";

    }
    #endregion Payment Tags

    #region Terminal Compatibality
    /// <summary>
    /// Terminal Compatibality
    /// </summary>
    internal static class TagID
    {
        public const int Tag84 = 0x84;
        public const int Tag0x9F33 = 0x9F33;
        public const string PKSI_CONSTANT = "1000";
        public const string REQ_CASHBACK_AMT = "305";
        public const string REQ_TRACK1 = "406";
        public const string REQ_TRACK2 = "407";
        public const string REQ_PINBLOCK = "408";
        public const string REQ_PAYTYPE = "404";
        public const string REQ_VERSION = "254";
        public const string REQ_BEGIN_SIGN = "000712";
        public const string REQ_ACCOUNT_NAME = "402";
        public const string SET_NFC_NAME = "NFC";
        public const string CONTACTLESS = "d";
        public const string MSR = "D";
        public const string MANUAL = "T";
        public const string Tag57 = "57";
    }
    #endregion Terminal Compatibality

    #region Device Payment Action Type
    /// <summary>
    /// Device Payment Action Type
    /// </summary>
    public enum TagType
    {
        /// <summary>
        /// User Swipe Card
        /// </summary>
        SWIPE,
        /// <summary>
        /// EMV Card and Transaction Detected
        /// </summary>
        EMV,
        /// <summary>
        /// Contactless 
        /// </summary>
        NFC,
        /// <summary>
        /// Manual Input
        /// </summary>
        MANUAL,
        /// <summary>
        /// Use Stored Profile
        /// </summary>
        STOREDPROFILE
    }
     #endregion Device Payment Action Type
}
