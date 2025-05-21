using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMile.Helpers
{
    internal static class ResponseStrings
    {
        #region Result Tags

        internal const string RESULT = "RESULT";
        internal const string ACCOUNT = "ACCOUNT";
        internal const string AMOUNT = "AMOUNT";
        internal const string EXPIRATION = "EXPIRATION";
        internal const string APPROVAL_CODE = "APPROVALCODE";
        internal const string NAME = "NAME";
        internal const string TRANSACTION_ID = "TRANSACTIONID";
        internal const string ORDER_ID = "ORDERID";
        internal const string TRANS_DETAIL = "DETAIL";
        internal const string ACCOUNT_TYPE = "ACCOUNTTYPE";
        internal const string PARTIAL_APPROVAL = "PARTIAL_APPROVAL";
        internal const string TERM_ID = "TERMID";
        internal const string MERCHANT_ID = "MERCHANTID";
        internal const string BALANCE = "BALANCE";
        internal const string TERMS = "TERMS";
        internal const string TOKEN = "TOKEN";
        internal const string LAST4 = "LAST4DIGITS";
        internal const string ENTRY_METHOD = "ENTRYMETHOD";
        internal const string RECEIPT_GROUP_1 = "RECEIPTGROUP1";
        internal const string RECEIPT_GROUP_2 = "RECEIPTGROUP2";
        internal const string RECEIPT_GROUP_3 = "RECEIPTGROUP3";
        internal const string SIGNATURE = "SIGNATURE";


        #endregion
    }


    internal static class EMVResponseStrings
    {
        internal const string TRANSACTION_TYPE = "TRANSACTIONTYPE";
        internal const string TRANSACTION_RESULT = "TRANSACTIONRESULT";
        internal const string TIME_STAMP = "TIMESTAMP";
        internal const string MERCHANT_ORDER_NUMBER = "MERCHANTORDERNUMBER";
        internal const string MERCHANT_ID = "MERCHANTID";
        internal const string TERM_ID = "TERMID";
        internal const string AMOUNT = "AMOUNT";
        internal const string CASH_BACK = "CASHBACK";
        internal const string ENTRY_METHOD = "ENTRYMETHOD";
        internal const string NAME = "NAME";
        internal const string ACCOUNT_TYPE = "ACCOUNTTYPE";
        internal const string ACCOUNT = "ACCOUNT";
        internal const string ORDER_ID = "ORDERID";
        internal const string TRANSACTION_ID = "TRANSACTIONID";
        internal const string AUTH_CODE = "AUTHCODE";
        internal const string DECLINE_CODE = "DECLINECODE";
        internal const string DECLINE_MESSAGE = "DECLINEMESSAGE";
        internal const string BATCH_NUMBER = "BATCHNUMBER";
        internal const string AVS_RESULT = "AVSRESULT";
        internal const string CVV2_RESULT = "CVV2RESULT";
        internal const string BALANCE = "BALANCE";


        // RESPONSE STRING 2

        internal const string APP_LABEL = "APPLABEL";
        internal const string AID = "AID";
        internal const string TVR = "TVR";
        internal const string IAD = "IAD";
        internal const string TSI = "TSI";
        internal const string ARQC = "ARQC";
        internal const string ARC = "ARC";
        internal const string CVM = "CVM";


    }
}
