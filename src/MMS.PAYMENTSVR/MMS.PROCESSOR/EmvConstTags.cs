using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.PROCESSOR
{
    public static class xEmvTags
    {
        public const string APPIDENTIFIER_AID = "RECEIPT_APPLICATIONIDENTIFIER";
        public const string APP_PREFEREDNAME_AIDNAME = "RECEIPT_APPLICATIONPREFERREDNAME";
        public const string AUTHORIZATION_RESP_CODE_CD = "RECEIPT_AUTHORIZATIONRESPONSECODE";
        public const string ENTRYLEGEND = "RECEIPT_ENTRYLEGEND";
        public const string ENTRYMETHOD = "RECEIPT_ENTRYMETHOD";
        public const string TERMINALVERIFY_TVR= "RECEIPT_TERMINALVERIFICATIONRESULTS";
        public const string MERCHANTID = "RECEIPT_MERCHANTID";
        public const string TRANSACTIONID = "RECEIPT_TRANSACTIONID";
        public const string TRANSTYPE = "RECEIPT_TRANSACTIONTYPE";
        public const string APP_CRYPTOGRAM_AC = "RECEIPT_APPLICATIONCRYPTOGRAM";
        public const string APP_TRANSACTION_COUNTER_ATC = "RECEIPT_APPLICATIONTRANSACTIONCOUNTER";
        public const string TRANS_STATUS_INFO_TSI = "RECEIPT_TRANSACTIONSTATUSINFORMATION";
        public const string TRANS_REF_NUM_TRN = "RECEIPT_TRANSACTIONREFERENCENUMBER";
        public const string VALIDATE_CODE_VC = "RECEIPT_VALIDATIONCODE";
        public const string CARDTYPE_CT = "RECEIPT_CARDTYPE";
        public const string VERBIAGE = "RECEIPT_VERBIAGE";
        public const string APPROVAL_CODE = "RECEIPT_APPROVALCODE";
        public const string ACCOUNT = "ACCOUNT";
        public const string APPROVED_AMOUNT = "APPROVEDAMOUNT";

        //Adding for Cloud
        public const string RESULTMSG = "RESULTMSG";
        public const string CLOUDTRANSACTIONID = "TRANSACTIONID";
        public const string EXPMONTH = "EXPMONTH";
        public const string EXPYEAR = "EXPYEAR";
        public const string CARDBRAND = "CARDBRAND";
        public const string CLOUDENTRYMETHOD = "Entry Method";
        public const string CLOUDAPPIDENTIFIER_AID = "AID";
        public const string CLOUDALIAS = "ALIAS";
        public const string CLOUDRECEIPTTEXT = "RECEIPTTEXT";
        public const string CLOUDENTRYTYPE = "ENTRYTYPE";
        public const string CLOUDCASHBACKAMOUNT = "CASHBACKAMOUNT";
    }
}
