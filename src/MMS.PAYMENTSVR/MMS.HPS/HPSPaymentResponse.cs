//Author: Manoj Kumar
//Company: Micro Merchant Systems, 2012
//Function: EBT transaction
//Implementation: For HPS (Heartland Payment Systems)

using System;
using System.Collections.Generic;
using System.Text;
using MMS.PROCESSOR;
using Hps.Exchange.PosGateway.Client;
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.HPS
{
    public class HPSPaymentResponse : PaymentResponse
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants

        private const String RESULT = "RESULT";
        private const String RESPONSETEXT = "RSPTEXT";
        private const String TRANSACTIONID = "GateWayTxnID";//"TROUTD";
        private const String AUTHCODE = "AUTHCODE";
        private const String AMOUNT_APPROVED = "AUTH_AMOUNT";
        private const String ADDITIONAL_FUNDS_REQUIRED = "AMOUNT_DUE";
        private const String RESULT_CAPTURED = "CAPTURED";
        private const String RESULT_FAILURE = "FAILURE";
        private const String RESULT_INVALIDAMT = "Invalid Doller Amount";
        private const String RESULT_SUCCESS = "SUCCESS";
        private const String RESULT_APPROVED = "APPROVAL";
        private const String RESULT_VOIDED = "VOIDED";
        private const String RESULT_PROCESSED = "PROCESSED";
        private const String RESULT_SALERECEIVED = "SALERECEIVED";
        private const String RESULT_RETURNRECOVERED = "RETURNRECOVERED";
        private const String RESULT_NOT_CAPTURED = "NOT CAPTURED";
        private const String RESULT_NOT_APPROVED = "NOT APPROVED";
        private const String RESULT_CANCELLED = "CANCELLED";
        private const String RESULT_SALE_NOT_FOUND = "SALE NOT FOUND";
        private const String TRANS_DATE = "TRANS_DATE";
        private const string REQUESTAMOUNTFIELD = "TRANS_AMOUNT"; 
         private const String MAX_AUTH_AMOUNT = "MAX_AUTH_AMOUNT";
        private const int AmountRoundDigit = 2;
        private const string INVALIDPAYMENT_PROCESSOR = "INVALID PAYMENT PROCESSOR SELECTION"; 
        private const string INV_PROCESSOR = "INV_PROCESSOR";
        private const string INV_MESG = "INV_MESG";
        private const string PARTIAL_APP = "PARTIAL APPROVAL";
        #endregion

        public string cardnbr = string.Empty;
        public string sAmount = string.Empty;
        Dictionary<string, string> IssuerErrorCode = null;
        Dictionary<int, string> SystemErrorCode = null;
        public HPSPaymentResponse()
        { }


        #region Property

        public string CardNbr
        {
            get
            {
                return cardnbr;
            }
        }

        public string Amount
        {
            get
            {
                return sAmount;
            }
        }

        #endregion

        /// <summary>
        /// Author: Manoj Kumar
        /// Desciption : Parse response received from HPS Payment server
        /// External functions: POSResponse from HPS.dll
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <param name="FilterNode"></param>
        /// <returns>int</returns>
        public override int ParseResponse(string xmlResponse, string FilterNode)
        {
            logger.Trace("HPS PARSERESPONSE() - Start Parsing HPS Response");
            int status = FAILURE;
            int tempPadChars = 20;
            string TagName = string.Empty;
            string tempResultFound = string.Empty;
            String value = String.Empty;
            string responseText=string.Empty;
            string rspcode = string.Empty;
            LoadIssuerErrorCode(); // Loading all error code

            if (this.objPosResponse != null)
            {
                PosResponseVer10 rspVer1_0 = (PosResponseVer10)objPosResponse.Item;
                if (rspVer1_0.Header.GatewayRspCode == (int)PosGatewayErrors.POSGATEWAY_RESPCODE_OK)
                {
                    switch (rspVer1_0.Transaction.ItemElementName)
                    {
                        case ItemChoiceTypePosResponseVer10Transaction.CreditVoid:
                        case ItemChoiceTypePosResponseVer10Transaction.CreditReturn:
                            responseText = rspVer1_0.Header.GatewayRspMsg;
                            if (rspVer1_0.Header.GatewayRspMsg.ToUpper() == RESULT_SUCCESS)
                            {
                                logger.Trace("HPS PARSERESPONSE() - Result: " + rspVer1_0.Header.GatewayRspMsg.ToUpper());
                                status = SUCCESS;
                                base.Result = RESULT_SUCCESS;
                                TagName = (TagName = "Result").PadLeft(tempPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + RESULT_SUCCESS;
                                base.IsFSATransaction = "F";
                                if (HPSTranAmount != null || HPSTranAmount != string.Empty)
                                {
                                    base.AmountApproved = base.HPSTranAmount;
                                }
                            }
                            else
                            {
                                IssuerErrorCode.TryGetValue(rspVer1_0.Header.GatewayRspCode.ToString(), out rspcode);
                                if (string.IsNullOrEmpty(rspcode.Trim()))
                                {
                                    rspcode = rspVer1_0.Header.GatewayRspMsg.ToUpper() + "\r\n" + "Response code: " + rspVer1_0.Header.GatewayRspCode;
                                }
                                logger.Trace("***HPS PARSERESPONSE() - Result: " + rspcode);
                                base.Result = RESULT_FAILURE ;
                                TagName = (TagName = "Result").PadLeft(tempPadChars);
                                base.ResultDescription += " \r\n Result : " + RESULT_FAILURE + "\nMessage: " + rspcode;
                            }
                            break;
                        case ItemChoiceTypePosResponseVer10Transaction.CreditSale:
                        case ItemChoiceTypePosResponseVer10Transaction.DebitSale:
                        case ItemChoiceTypePosResponseVer10Transaction.CreditReversal:
                        case ItemChoiceTypePosResponseVer10Transaction.DebitReturn:
                        case ItemChoiceTypePosResponseVer10Transaction.DebitReversal:
                        case ItemChoiceTypePosResponseVer10Transaction.EBTFSPurchase:
                        case ItemChoiceTypePosResponseVer10Transaction.EBTFSReturn:
                            AuthRspStatusType creditSaleRsp = (AuthRspStatusType)rspVer1_0.Transaction.Item;
                            if (creditSaleRsp.RspText != null || creditSaleRsp.RspText != string.Empty)
                            {
                                status = SUCCESS;
                                switch (creditSaleRsp.RspText.ToUpper().ToString())
                                {
                                    case RESULT_APPROVED:
                                    case PARTIAL_APP:
                                    case RESULT_SUCCESS:
                                        {
                                            logger.Trace("HPS PARSERESPONSE Success() - Result: " + creditSaleRsp.RspText.ToUpper().ToString());
                                            base.Result = RESULT_SUCCESS;
                                            TagName = (TagName = "Result").PadLeft(tempPadChars);
                                            base.ResultDescription += " \r\n" + TagName + " : " + RESULT_SUCCESS;
                                            TagName = (TagName = "Details").PadLeft(tempPadChars);
                                            base.ResultDescription += " \r\n" + TagName + " : " + creditSaleRsp.RspText.Trim();
                                            base.AuthNo = creditSaleRsp.AuthCode;
                                            base.Balance = Convert.ToDecimal(creditSaleRsp.AvailableBalance).ToString();
                                            // To handle FSA Auth Amount and Approved Amount
                                            //int authAmt = Convert.ToInt32(creditSaleRsp.AuthAmt);
                                            if (creditSaleRsp.AuthAmt == 0.0m)
                                            {
                                                base.IsFSATransaction = "F";
                                                if (HPSTranAmount != null || HPSTranAmount != string.Empty)
                                                {
                                                    base.AmountApproved = base.HPSTranAmount;
                                                }
                                            }
                                            else
                                            {
                                                base.AmountApproved = creditSaleRsp.AuthAmt.ToString();
                                            }
                                            break;
                                        }
                                    case RESULT_NOT_APPROVED:
                                    case RESULT_CANCELLED:
                                    case RESULT_SALE_NOT_FOUND:
                                    case RESULT_FAILURE:
                                        {
                                            IssuerErrorCode.TryGetValue(creditSaleRsp.RspCode, out rspcode);
                                            if (string.IsNullOrEmpty(rspcode.Trim()))
                                            {
                                                rspcode = creditSaleRsp.RspText.ToUpper().ToString() + "\r\n"+"Response code: " + creditSaleRsp.RspCode;
                                            }
                                            logger.Trace("***HPS PARSERESPONSE Fail() - Result: " + rspcode);
                                            base.Result = RESULT_FAILURE ;
                                            TagName = (TagName = "Result").PadLeft(tempPadChars);
                                            base.ResultDescription += " \r\n Result : " + RESULT_FAILURE + "\n" + rspcode;
                                            TagName = (TagName = "Details").PadLeft(tempPadChars);
                                            base.ResultDescription += " \r\n" + TagName + " : " + creditSaleRsp.RspText.ToUpper().ToString();
                                            break;
                                        }
                                    default:
                                        {
                                            IssuerErrorCode.TryGetValue(creditSaleRsp.RspCode, out rspcode);
                                            if (string.IsNullOrEmpty(rspcode.Trim()))
                                            {
                                                rspcode = creditSaleRsp.RspText.ToUpper().ToString() + "\r\n" + "Response code: " + creditSaleRsp.RspCode;
                                            }
                                            logger.Trace("***HPS PARSERESPONSE() Default - Result: " + rspcode);
                                            base.Result = RESULT_FAILURE;
                                            TagName = (TagName = "Result").PadLeft(tempPadChars);
                                            base.ResultDescription += " \r\n Result : " + RESULT_FAILURE + "\n" + rspcode;
                                            TagName = (TagName = "Details").PadLeft(tempPadChars);
                                            base.ResultDescription += " \r\n" + TagName + " : " + creditSaleRsp.RspText.ToUpper().ToString();
                                            break;
                                        }
                                }
                            }
                            break;
                    }
                    base.TransactionNo = rspVer1_0.Header.GatewayTxnId.ToString();
                }
                else
                {
                    SystemErrorCode.TryGetValue(rspVer1_0.Header.GatewayRspCode, out rspcode);
                    if(string.IsNullOrEmpty(rspcode.Trim()))
                    {
                        rspcode = rspVer1_0.Header.GatewayRspMsg.ToString() + "\r\n" + "Response code: " + rspVer1_0.Header.GatewayRspCode.ToString();
                    }
                    logger.Trace("***HPS PARSERESPONSE System() - Result: " + rspcode);
                    base.Result = RESULT_FAILURE;
                    TagName = (TagName = "Result").PadLeft(tempPadChars);
                    TagName = (TagName = "ERROR").PadLeft(tempPadChars);
                    base.ResultDescription += " \r\nResult: " + RESULT_FAILURE + "\n" + rspcode;
                }
            }
            else
            {
                if (this.HPSTransType != null)
                {
                    if (this.HPSTransType == "HPS_REVERSE")
                    {
                        string[] sTransAmount = this.HPSTranAmount.Split('-');
                        if (sTransAmount.Length > 0)
                        {
                            logger.Trace("***HPS PARSERESPONSE() - HPS_Reverse Error: " + RESULT_FAILURE + "\r\n"+"Amount: " + RESULT_INVALIDAMT);
                            base.Result = RESULT_FAILURE + "\n" + RESULT_INVALIDAMT;
                            TagName = (TagName = "Result").PadLeft(tempPadChars);
                            base.ResultDescription += " \r\n Result : " + RESULT_INVALIDAMT;
                        }
                    }
                }
            }
            IssuerErrorCode = null;
            SystemErrorCode = null;
            logger.Trace("HPS PARSERESPONSE()- End the Parsing of the Result from the gateway");
            return status;
        }

        /// <summary>
        /// Author: Manoj Kumar
        /// Description: Dictionary with all HPS error code. From HPS sdk
        /// </summary>
        private void LoadIssuerErrorCode()
        {
            try
            {
                IssuerErrorCode = new Dictionary<string, string>();
                IssuerErrorCode.Add("02", "02-Often returned when the cardholder has \nexceeded daily credit limits. Please make \nsure this is the credit cardholder");
                IssuerErrorCode.Add("03", "03-Terminal ID error");
                IssuerErrorCode.Add("04", "04-HOLD-CALL, Retain card. Issuer would like \nthe merchant to take possession of the \ncard due to potential fraud");
                IssuerErrorCode.Add("05", "05-Delcine, Normally occurs when cardholder \nhas exceeded their allowable credit line.");
                IssuerErrorCode.Add("06", "06-Error, merchant closed, no match");
                IssuerErrorCode.Add("07", "07-Hold-Call");
                IssuerErrorCode.Add("10", "10-Partial Approval");
                IssuerErrorCode.Add("12", "12-Invalid Transaction");
                IssuerErrorCode.Add("13", "13-Amount Error, Amount submitted is $0.00");
                IssuerErrorCode.Add("14", "14-Card number error. Issuer \ncannot find account");
                IssuerErrorCode.Add("15", "15-No such issuer. Card number \nnot recognized by the issuer");
                IssuerErrorCode.Add("19", "19-Reenter Transaction. Bad swipe");
                IssuerErrorCode.Add("21", "21-No action take, txn back off match");
                IssuerErrorCode.Add("41", "41-Hold-Call, lost card");
                IssuerErrorCode.Add("43", "43-Hold-Call, stolen card");
                IssuerErrorCode.Add("44", "44-Hold-Call, pick up card");
                IssuerErrorCode.Add("51", "51-Decline, insufficient funds");
                IssuerErrorCode.Add("52", "52-No check amount, Debit or Check card \nis not linked to a checking account");
                IssuerErrorCode.Add("53", "53-No save account, Debit or Check card \nis not tied to a savings account");
                IssuerErrorCode.Add("54", "54-Expired Card");
                IssuerErrorCode.Add("55", "55-Wrong Pin number");
                IssuerErrorCode.Add("56", "56-Invalid Card");
                IssuerErrorCode.Add("57", "57-Service not allowed");
                IssuerErrorCode.Add("58", "58-Service not allowed. Merchant is \nnot set up for this transaction type");
                IssuerErrorCode.Add("61", "61-Decline, cardholder has exceeded \ntheir withdrawal limit");
                IssuerErrorCode.Add("62", "62-Decline, Service code on card \ndoes not match Issuer service code");
                IssuerErrorCode.Add("63", "63-Sec Violation");
                IssuerErrorCode.Add("65", "65-Decline, Cardholder exceeded \nthe number of times the card can be \nuse in a time period");
                IssuerErrorCode.Add("75", "75-Pin Exceeded, number of attempts \nto enter the PIN has been exceeded");
                IssuerErrorCode.Add("76", "76-No action taken, Reversal data does \nnot match Issuer data");
                IssuerErrorCode.Add("77", "77-No action taken, duplication reversal \nor duplicate transaction");
                IssuerErrorCode.Add("78", "78-No account, account suspended, \ncancelled, or inactive");
                IssuerErrorCode.Add("80", "80-Date Error");
                IssuerErrorCode.Add("82", "82-Cashback no app");
                IssuerErrorCode.Add("85", "85-Card OK");
                IssuerErrorCode.Add("86", "86-Can't verify PIN");
                IssuerErrorCode.Add("91", "91-No Reply, time out");
                IssuerErrorCode.Add("96", "96-System error");
                IssuerErrorCode.Add("EB", "EB-Check digit err");
                IssuerErrorCode.Add("EC", "EC-CID Format error, format error");
                IssuerErrorCode.Add("N7", "N7-CVV2 Mismatch");

                SystemErrorCode = new Dictionary<int, string>();
                SystemErrorCode.Add(-1, "-1 -POS Gateway Error - Developer are notified.");
                SystemErrorCode.Add(-2, "-2 -Authentication error, Verify and Correct Credentials");
                SystemErrorCode.Add(-21, "Unauthorized");
                SystemErrorCode.Add(1, "1-Gateway system error");
                SystemErrorCode.Add(2, "2-Duplicate transacations");
                SystemErrorCode.Add(3, "3-Invalid original transaction");
                SystemErrorCode.Add(4, "4-Transaction already associated with batch");
                SystemErrorCode.Add(5, "5-No current batch");
                SystemErrorCode.Add(6, "6-Invalid return amount or the return \namount is greater than the original \ntransaction's settle amount.");
                SystemErrorCode.Add(7, "7-Invalid report parameters");
                SystemErrorCode.Add(8, "8-Bad track data");
                SystemErrorCode.Add(9, "9-No transacation accociated with batch");
                SystemErrorCode.Add(10, "10-Empty report");
                SystemErrorCode.Add(11, "11-Original transaction not CPC");
                SystemErrorCode.Add(12, "12-Invalid CPC data");
                SystemErrorCode.Add(13, "13-Invalid edit data");
                SystemErrorCode.Add(14, "14-Invalid card number");
                SystemErrorCode.Add(15, "15-Batch close in progress");
                SystemErrorCode.Add(16, "16-Invalid ship Date");
                SystemErrorCode.Add(17, "17-Invalid encryption version");
                SystemErrorCode.Add(18, "18-E3 MSR failure");
                SystemErrorCode.Add(19, "19-Invalid Reversal Amount");
                SystemErrorCode.Add(20, "20-Database operation timeout");
                SystemErrorCode.Add(23, "23-An error was returned from the tokenization \nservice when looking up a supplied token");
                SystemErrorCode.Add(24, "24-Tokenization ia not yet supported");
                SystemErrorCode.Add(25, "25-Token cannot be presented and \nrequested in the same request");
                SystemErrorCode.Add(30, "30-HPS Gateway did not receive a \nresponse from their system.");
                SystemErrorCode.Add(32, "32-Missing KTB error");
                SystemErrorCode.Add(33, "33-Missing KSN error");
            }
            catch (Exception ex)
            {
                logger.Trace("***********HPS Payment Response-LoadIssuerErrorCode() Error loading errorcode.");
                throw new Exception(ex.ToString());
            }
        }
    }
}
