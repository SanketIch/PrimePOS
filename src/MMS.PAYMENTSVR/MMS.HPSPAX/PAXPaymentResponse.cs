//Author : Ritesh
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
//External functions:MMSDictioanary,XmlToKeys
//Known Bugs : None
//Start Date : 29 January 2008.

using MMS.GlobalPayments.Api.Terminals.PAX;
using MMS.PROCESSOR;
//using SecureSubmit.Terminals.PAX;//Commented by Amit on 02 Dec 2020//PRIMEPOS-2931
using NLog;
using System;

namespace MMS.HPSPAX
{
    //Author : Ritesh
    //Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
    //External functions:None
    //Known Bugs : None
    //Start Date : 29 January 2008.
    public class PAXPaymentResponse : PaymentResponse {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region variables

        private object locker = new object();

        #endregion variables
        #region constants

        //Response constants 
        private const String DEVICE_RESPONSE_CODE_OK = "000000";
        private const String DEVICE_RESPONSE_CODE_DECLINED = "000100";
        private const String DEVICE_RESPONSE_CODE_TIMEOUT = "100001";
        private const String DEVICE_RESPONSE_CODE_ABORTED = "100002";
        private const String DEVICE_RESPONSE_CODE_REQ_ERROR = "100003";
        private const String DEVICE_RESPONSE_CODE_UNSUPPORTED_TXN = "100004";
        private const String DEVICE_RESPONSE_CODE_DUP_TRANSACTION = "100011";//NileshJ - Duplicate Transaction - 20-Dec-2018

        private const String RESPONSE_ERROR = "ERROR";
        private const String RESPONSE_SUCCESS = "SUCCESS";

        private const String SIGNSTATUS = "SIGNSTATUS";
        private const String CARDBIN = "CARDBIN";
        private const String HREF = "HREF";
        private const String PROGRAMTYPE = "PROGRAMTYPE";
        private const String SIGNDATA = "SIGNDATA";
        private const String TOKEN = "TOKEN"; // PRIMEPOS - 2530 - Added For Tokenization
        private const String SN = "SN";
        private const String TC = "TC";
        private const String TVR = "TVR";
        private const String AID = "AID";
        private const string TSI = "TSI";
        private const string ATC = "ATC";
        private const string APPLAB = "APPLAB";
        private const string APPPN = "APPPN";
        private const string IAD = "IAD";
        private const string ARC = "ARC";
        private const string CID = "CID";
        private const string CVM = "CVM";
        //PRIMEPOS-2578 - Added for EMV Receipt
        private const string RECEIPT_VALIDATIONCODE = "VALCODE";
        private const string RECEIPT_TRANSACTIONREFERENCENUMBER = "TRANSACTIONREFERENCENUMBER";
        private const string RECEIPT_MERCHANTID = "MERCHANTID";
        private const string RECEIPT_ENTRYLEGEND = "ENTRYLEGEND";
        private const string RECEIPT_TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const string AC = "AC";
        private const string TerminalID = "SN";
        private const string CardTypetag = "Card Type";
        private const string TransID = "Trans ID";
        //

        #endregion constants
        #region public methods

        //Constructor
        public PAXPaymentResponse() {
        }

        public override int ParseResponse(string xmlResponse, string FilterNode) {
            return 0;
        }

        //public override void ParsePAXResponse(PaxDeviceResponse _DeviceResponse) { //Commented by Amit on 02 Dec 2020 //PRIMEPOS-2931
        public override void ParsePAXResponse(PaxTerminalResponse _DeviceResponse)
        {
        
            logger.Trace("PAX ParseRespone Start");
            lock (locker) {
                base.ClearFields();
                // PRIMEPOS-2529 NileshJ - Added check on DeviceResponseCode and removed ResponseCode Check on transaction response
                if ((_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_OK) || (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_DUP_TRANSACTION))// NileshJ - Add condtion _DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_DUP_TRANSACTION) 
                {
                    logger.Trace("PAX Device Responnse Code - Status" + _DeviceResponse.DeviceResponseCode + "-" + _DeviceResponse.DeviceResponseText);
                    //if (_DeviceResponse.ResponseCode == "00" || _DeviceResponse.ResponseCode == "10") { //removed

                    logger.Trace("Device Transaction Response  Code - Text" + _DeviceResponse.ResponseCode + "-" + _DeviceResponse.ResponseText);


                    // NileshJ - Duplicate Transaction
                    if (_DeviceResponse.DeviceResponseCode == DEVICE_RESPONSE_CODE_DUP_TRANSACTION)
                    {
                        base.Result = RESPONSE_ERROR;
                        base.ResultDescription = _DeviceResponse.DeviceResponseText;
                    }// NileshJ - Duplicate Transaction End
                    else
                    {
                        base.Result = RESPONSE_SUCCESS;
                        // PRIMEPOS-2543 NileshJ - Add " + Environment.NewLine + _DeviceResponse.ResponseText" [07/06/2018]
                        base.ResultDescription = _DeviceResponse.ResponseText + Environment.NewLine + _DeviceResponse.ResponseText;
                    }
                    base.AccountNo = _DeviceResponse.AccountResponse.AccountNumber;
                    if (_DeviceResponse.GetType() == typeof(CreditResponse)) {
                        base.AuthNo = ((CreditResponse)_DeviceResponse).AuthorizationCode;
                    }//PRIMEPOS-3087
                    else
                    {
                        base.AuthNo = _DeviceResponse.AuthorizationCode;
                    }
                    base.MaskedCardNo = _DeviceResponse.MaskedCardNumber;
                    base.Expiration = _DeviceResponse.ExpirationDate;
                    base.EntryMethod = _DeviceResponse.EntryMethod;
                    base.CashBack = Convert.ToString(_DeviceResponse.CashBackAmount);
                    base.CardType = _DeviceResponse.AccountResponse.CardType.ToString();
                    base.AmountApproved = Convert.ToString(_DeviceResponse.AmountResponse.ApprovedAmount);
                    base.Balance = Convert.ToString(_DeviceResponse.AmountResponse.Balance1);
                    base.TransactionNo = Convert.ToString(_DeviceResponse.TraceResponse.TransactionNumber);

                    //Process External Data
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(SIGNDATA, out base.SignatureString);
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(TOKEN, out base.ProfiledID);// PRIMEPOS - 2530 - Added For Tokenization
                    #region PRIMEPOS-3146
                    if (!string.IsNullOrWhiteSpace(_DeviceResponse.HostResponse?.TransactionIdentifier))
                    {
                        base.ProfiledID = $"{base.ProfiledID}|{_DeviceResponse.HostResponse?.TransactionIdentifier}";
                    }
                    #endregion
                    //PRIMEPOS-2578 - Added for EMV receipt TAGS NileshJ - START
                    base.EmvReceipt = new EmvReceiptTags();

                    string programType = String.Empty;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(PROGRAMTYPE, out programType);
                    if (programType != String.Empty && programType == "2")
                    {
                        base.EmvReceipt.IsFsaCard = true;//HPSPAX issues found while testing
                        base.IsFSATransaction = "T";
                    }
                    else
                    {
                        base.EmvReceipt.IsFsaCard = false;//HPSPAX issues found while testing
                        base.IsFSATransaction = "F";
                    }
                    base.EmvReceipt.AppCrytogram = _DeviceResponse.ApplicationCryptogram;

                    // AID
                    string AIDVal = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(AID, out AIDVal);
                    base.EmvReceipt.AppIndentifer = AIDVal;

                    // ATC
                    string ATCVal = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(ATC, out ATCVal);
                    base.EmvReceipt.AppTransactionCounter = ATCVal;

                    //TVR
                    string TVRVal = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(TVR, out TVRVal);
                    base.EmvReceipt.TerminalVerficationResult = TVRVal;

                    //TSI
                    string TSIVal = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(TSI, out TSIVal);
                    base.EmvReceipt.TransStatusInformation = TSIVal;

                    //Author Resp CD
                    string RespCodeVal = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(ARC, out RespCodeVal);
                    base.EmvReceipt.AuthorizationResposeCode = RespCodeVal;

                    //Refernce Number
                    string HRefVal = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(HREF, out HRefVal);
                    base.EmvReceipt.TransRefNumber = HRefVal;

                    //Val Code
                    string validationCodevalue = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(RECEIPT_VALIDATIONCODE, out validationCodevalue);
                    base.EmvReceipt.ValidationCode = validationCodevalue;

                    //AID Name
                    string AIDName = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(APPPN, out AIDName);
                    base.EmvReceipt.AppPreferedName = AIDName;

                    string CardTypeVal = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(CardTypetag, out CardTypeVal);
                    base.EmvReceipt.AccountType = CardTypeVal;

                    string TRNID = null;
                    _DeviceResponse.ExtDataResponse.Fields.TryGetValue(TransID, out TRNID);
                    base.EmvReceipt.TransID = TRNID;

                    //Discover Specific - 16AUg2018
                    //if (_DeviceResponse.AccountResponse.CardType == SecureSubmit.Terminals.PAX.CardType.DiSCOVER)//Commented by Amit on 02 Dec 2020
                    if (_DeviceResponse.AccountResponse.CardType == TerminalCardType.DiSCOVER)//PRIMEPOS-2931
                    {
                        string cardHolder = _DeviceResponse.AccountResponse.CardHolder;
                        base.ResponseMsgAllKeys.Add("CARDHOLDER", cardHolder);
                    }

                    base.ticketNum = _DeviceResponse.ReferenceNumber; // NileshJ Added for Handle Duplicate Transaction - 20-Dec-2018

                    //PRIMEPOS-2578 - Added for EMV receipt TAGS NileshJ - END

                    //} else {
                    //    base.Result = RESPONSE_ERROR;
                    //    base.ResultDescription = _DeviceResponse.DeviceResponseText;
                    //}


                }
                else {
                    base.Result = RESPONSE_ERROR;
                    // PRIMEPOS-2543 NileshJ - Add " + Environment.NewLine + _DeviceResponse.ResponseText" [07/06/2018]
                    base.ResultDescription = _DeviceResponse.DeviceResponseText + Environment.NewLine + _DeviceResponse.ResponseText; 
                    logger.Trace("PAX Device Responnse Code - Status" + _DeviceResponse.DeviceResponseCode + "-" + _DeviceResponse.DeviceResponseText);
                }
                logger.Trace("PAX ParseRespone END");
            }

        }
        #endregion public methods
    }
}