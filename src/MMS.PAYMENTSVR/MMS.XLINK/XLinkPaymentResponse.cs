//Author : Ritesh
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
//External functions:MMSDictioanary,XmlToKeys
//Known Bugs : None
//Start Date : 29 January 2008.

using System;
using System.Collections.Generic;
using System.Text;
using MMS.PROCESSOR;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.XLINK
{
    //Author : Ritesh
    //Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
    //External functions:None
    //Known Bugs : None
    //Start Date : 29 January 2008.
    public class XLinkPaymentResponse : PaymentResponse
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region variables

        #endregion variables
        #region constants

        private const String RESULT_SUCCESS = "SUCCESS";
        private const String RESULT_FAILURE = "FAILURE";
        private const String RESULT = "RESULT";
        private const String TRANSACTIONID = "XCTRANSACTIONID";
        private const String AUTHCODE = "APPROVALCODE";
        //Need to check these tags
        private const String AMOUNT_APPROVED = "APPROVEDAMOUNT"; //Added by Manoj 8/18/2011 Required by Xcharge as of 8/15/2011
        private const String ADDITIONAL_FUNDS_REQUIRED = "ADDITIONALFUNDSREQUIRED";
        //Added By SRT(Gaurav) Date: 18 NOV 2008
        private const String RES_EXPIRATION = "EXPIRATION";
        private const String RES_ADDRESS = "ADDRESS";
        private const String RES_ZIP = "ZIP";
        private const string REQUESTAMOUNTFIELD = "AMOUNT";
        //End Of Added By SRT(Gaurav)

        //Added By SRT(Gaurav) Date: 20 NOV 2008
        private const String IIASTRANSACTION = "IIASTRANSACTION";
        //End Of Added By SRT(Gaurav)
        //Added By Dharmendra (SRT) on Dec-16-08
        private const string INV_MESG = "INV_MESG";
        //End Added Dec-16-08
        //Added By Dharmendra (SRT) on Dec-26-08
        private const string XCHARGE_ERRORXMLFILE = "XchargeErrorCode.xml";
        private const string DESCRIPTION = "DESCRIPTION";
        private const string XAUTHCODE = "AUTHCODE";
        private const string ACCOUNT = "ACCOUNT";
        private const string ACCOUNTTYPE = "ACCOUNTTYPE";
        private const string CARD_BALANCE = "BALANCE"; // Added by Manoj 8/18/2011  Required by Xcharge to be on Receipt
        private const string CASHBACK = "RECEIPT_CASHBACKAMOUNT";
        public const string PROFILEDID = "XCACCOUNTID";
        public const string ENTRYMETHOD = "RECEIPT_ENTRYMETHOD";

        //End Added

        #endregion constants
        #region public methods

        private PrimeChargeLogWriter xlinkLogger = new PrimeChargeLogWriter(true.ToString());
        //Constructor

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is constructor for the PCChargePaymentResponse class
        /// External functions:MMSDictioanary
        /// Known Bugs : None
        /// Start Date : 29 Jan 2008.
        /// </summary>
        public XLinkPaymentResponse()
        {
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method is used to parse the response of PCCharge Payment server
        /// External functions:MMSDictioanary,XmlToKeys
        /// Known Bugs : MMSDictioanary
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="xmlMessage"></param>
        /// <param name="xmlFilterNodeName"></param>
        /// <param name="fields"></param>
        /// <param name="isFile"></param>
        /// <returns>int result</returns>
        public override int ParseResponse(String xmlResponse, String FilterNode)
        {
            logger.Trace("XLINK ParseRespone Start");
            int status = FAILURE;

            if(FilterNode.Equals(Processor.CLOUD_RESP_FILTERNODE))
            {
                status = CloudParseResponse(xmlResponse, FilterNode);
                return status;
            }

            //Added By SRT(Gaurav) Date : 20 NOV 2008
            int lPadChars = 20;
            string TagName = string.Empty;
            //End Of Added By SRT(Gaurav)
            xlinkLogger.AppendCommentsToLogger("Logging Responce recieved from XLINK.\n");
            xlinkLogger.AppendResponseToLogger(xmlResponse);
            String value = String.Empty;
            base.ClearFields();
            //Modified By Dharmendra (SRT) on Dec-16-08 placed the accsseing of values from xmlResponse in to try..catch block
            try
            {
                status = XmlToKeys.GetFields(xmlResponse, FilterNode, ref ResponseMsgAllKeys, false);
                logger.Trace("XLink # of Tags: " + status.ToString());
            }
            //catch exception if xmlresponse is not in xml readable format
            catch (Exception ex)
            {
                //Added By Dharmendra (SRT) on Dec-15-08 to initialize the response variables with error details & result failure
                string tempException = ex.ToString();
                base.Result = INV_MESG; //Modified By Dharmendra (SRT) on Dec-16-08 to assign INV_MESG to the result variable
                TagName = (TagName = "Result").PadLeft(lPadChars);
                value = "Either Xlink can not be contacted. The system cannot find the file specified.";
                value += "\r\nOr the message received from XLink is not valid.\r\nPelease ensure you reverse the transaction manually if it exist in X-Charge Payment Report.";
                value += "\r\nAlso check Xcharge application path in the tag <PAYMENTCLIENT></PAYMENTCLIENT>\r\nin MerchantConfig.xml file in the application folder";
                TagName = (TagName = "RESULT DESCRIPTION").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //Added Till Here Dec-15-08
                logger.Error(ex,"*** XLINK ParseResponse() ERROR \r\n" + tempException + "\r\nResult: " + INV_MESG + "\r\nStatus: " + status.ToString());
                return status;
            }
            if (ResponseMsgAllKeys.TryGetValue(RESULT, out value))
            {
                logger.Trace("XLINK ParseResponse() Result: " + value.Trim());
                if (value.ToUpper().Trim().Equals(RESULT_SUCCESS))
                {
                    base.Result = RESULT_SUCCESS;
                    //Added By SRT(Gaurav) Date : 20 NOV 2008
                    TagName = (TagName = "Result").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + RESULT_SUCCESS;
                    //End Of Added By SRT(Gaurav)
                    TagName = (TagName = "Details").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                }
                else
                {
                    //Added By Dharmendra (SRT) on Dec-26-08 to find the value of DESCRIPTION field in the response
                    // & to process furthur
                    string sDescriptionValue = string.Empty;
                    if (ResponseMsgAllKeys.TryGetValue(DESCRIPTION, out sDescriptionValue))
                    {
                        //String sXchargeErrorDescription = GetXChargeErrorDescription(sDescriptionValue.Trim()); //Commented To ByPass reading from xml XchargeErrorCode.xml file
                        base.Result = RESULT_FAILURE;
                        TagName = (TagName = "Result").PadLeft(lPadChars);
                        base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                        //End Of Added By SRT(Gaurav)
                        TagName = (TagName = "Details").PadLeft(lPadChars);
                        //base.ResultDescription += " \r\n" + TagName + " : " + sXchargeErrorDescription;
                        //Modified By Dharmendra on Mar-12-09
                        //Bypassing reading the error description fro XchargeErrorCode.xml
                        base.ResultDescription += "\r\n" + TagName + ": " + sDescriptionValue;
                        //Modified Till Here Mar-12-09
                        logger.Trace("***XLINK ParseRespone() FAILURE : " + RESULT_FAILURE);
                        return status; //Added By Dharmendra (SRT) on Dec-17-08
                        //Added Till Here Dec-26-08
                    }
                    else
                    {
                        base.Result = RESULT_FAILURE;
                        //Added By SRT(Gaurav) Date : 20 NOV 2008
                        TagName = (TagName = "Result").PadLeft(lPadChars);
                        base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                        //End Of Added By SRT(Gaurav)
                        TagName = (TagName = "Details").PadLeft(lPadChars);
                        base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                        logger.Error("***XLINK ParseRespone() FAILURE : " + RESULT_FAILURE);
                        return status; //Added By Dharmendra (SRT) on Dec-17-08
                    }
                }
            }
            //Added By Dharmendra (SRT) on Dec-12-08 to assign the error message in case if result tag is missing
            else
            {
                base.Result = RESULT_FAILURE;
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Result").PadLeft(lPadChars);
                base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                TagName = (TagName = "Details").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + xmlResponse.Trim();
                logger.Error("***XLINK ParseRespone() FAILURE : " + RESULT_FAILURE);
                return status; //Added By Dharmendra (SRT) on Dec-17-08
            }
            //End Added
            if (ResponseMsgAllKeys.TryGetValue(TRANSACTIONID, out value))
            {
                logger.Trace("***XLINK ParseRespone() TransactionID: " + value.Trim());
                base.TransactionNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Transaction ID").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(AUTHCODE, out value))
            {
                base.AuthNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Auth Code").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(IIASTRANSACTION, out value))
            {
                base.IsFSATransaction = value; //Added By Dharmendra (SRT) on Dec-16-08 earlier it was excluded wrongly.
                if (value.Trim() == "T")
                {
                    //Added By SRT(Gaurav) Date : 20 NOV 2008
                    TagName = (TagName = "IIAS Transaction").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : Available";
                    //End Of Added By SRT(Gaurav)
                }
                else
                {
                    //Added By SRT(Gaurav) Date : 20 NOV 2008
                    TagName = (TagName = "IIAS Transaction").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : Not Available";
                    //End Of Added By SRT(Gaurav)
                }
            }
            //End Of Added By SRT(Gaurav)
            if (ResponseMsgAllKeys.TryGetValue(AMOUNT_APPROVED, out value))
            {
                base.AmountApproved = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                //Added By Dharmendra (SRT) on Dec-09-08 to check the value of Approved Amount
                if (base.Result == RESULT_SUCCESS && base.AmountApproved.Trim() != string.Empty)
                {
                    TagName = (TagName = "Approved Amount").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                }
            }
            //Added By Dharmendra (SRT) on Dec-15-08
            // If Approved amount is empty then fill the requested amount
            else if (base.Result == RESULT_SUCCESS && base.AmountApproved.Trim() == string.Empty)
            {
                if (ResponseMsgAllKeys.TryGetValue(REQUESTAMOUNTFIELD, out value)) // try to get the requested amount
                {
                    base.AmountApproved = value.Trim();
                    TagName = (TagName = "Approved Amount").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                }
            }
            //Added Till Here  Dec-15-08
            //End Of Added By SRT(Gaurav)

            //Added By SRT(Gaurav) Date : 20 NOV 2008
            if (ResponseMsgAllKeys.TryGetValue(ADDITIONAL_FUNDS_REQUIRED, out value))
            {
                // Added By Dharmendra (SRT) on Nov-28-08
                base.AdditionalFundsRequired = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Additional Funds Required").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }

            //Added By SRT(Gaurav) Date : 18 NOV 2008
            if (ResponseMsgAllKeys.TryGetValue(ACCOUNT, out value))
            {
                base.MaskedCardNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Transaction Account").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(RES_EXPIRATION, out value))
            {
                base.Expiration = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Account Expiration").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(RES_ADDRESS, out value))
            {
                base.Address = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Account Address").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(RES_ZIP, out value))
            {
                base.ZIP = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Account Zip").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            //End Of Added By SRT(Gaurav)
            if (ResponseMsgAllKeys.TryGetValue(ACCOUNT, out value))
            {
                base.AccountNo = value;
                TagName = (TagName = "Account No").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            if (ResponseMsgAllKeys.TryGetValue(ACCOUNTTYPE, out value))
            {
                base.CardType = value;
                TagName = (TagName = "Card Type").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            // Added by Manoj 8/19/2011 This is required to be on the Receipt by Xcharge.
            if (ResponseMsgAllKeys.TryGetValue(CARD_BALANCE, out value))
            {
                base.Balance = value.Trim();
                TagName = (TagName = "Card Balance").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            if (ResponseMsgAllKeys.TryGetValue(CASHBACK, out value))
            {
                base.CashBack = value.Trim();
                TagName = (TagName = "CashBack Amount").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            if (ResponseMsgAllKeys.TryGetValue(PROFILEDID, out value))
            {
                base.ProfiledID = value.Trim();
                TagName = (TagName = "ProfiledID").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            if (ResponseMsgAllKeys.TryGetValue(ENTRYMETHOD, out value))
            {
                base.EntryMethod = value.Trim();
                TagName = (TagName = "EntryMethod").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }

            if (EmvReceipt == null && ResponseMsgAllKeys.TryGetValue(xEmvTags.APPIDENTIFIER_AID, out value))
            {
                base.EmvReceipt = new EmvReceiptTags();
                foreach (var r in ResponseMsgAllKeys)
                {
                    switch (r.Key.ToUpper())
                    {
                        case xEmvTags.APPIDENTIFIER_AID:
                            {
                                base.EmvReceipt.AppIndentifer = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APP_PREFEREDNAME_AIDNAME:
                            {
                                base.EmvReceipt.AppPreferedName = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APP_CRYPTOGRAM_AC:
                            {
                                base.EmvReceipt.AppCrytogram = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APP_TRANSACTION_COUNTER_ATC:
                            {
                                base.EmvReceipt.AppTransactionCounter = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TERMINALVERIFY_TVR:
                            {
                                base.EmvReceipt.TerminalVerficationResult = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANS_STATUS_INFO_TSI:
                            {
                                base.EmvReceipt.TransStatusInformation = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.AUTHORIZATION_RESP_CODE_CD:
                            {
                                base.EmvReceipt.AuthorizationResposeCode = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANS_REF_NUM_TRN:
                            {
                                base.EmvReceipt.TransRefNumber = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.VALIDATE_CODE_VC:
                            {
                                base.EmvReceipt.ValidationCode = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.MERCHANTID:
                            {
                                base.EmvReceipt.MerchantID = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANSACTIONID:
                            {
                                base.EmvReceipt.TransID = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.ENTRYLEGEND:
                            {
                                base.EmvReceipt.EntryLegend = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.CARDTYPE_CT:
                            {
                                base.EmvReceipt.AccountType = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANSTYPE:
                            {
                                base.EmvReceipt.TransType = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.VERBIAGE:
                            {
                                base.EmvReceipt.Verbiage = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APPROVAL_CODE:
                            {
                                base.EmvReceipt.ApprovalCode = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.ACCOUNT:
                            {
                                base.EmvReceipt.Account = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APPROVED_AMOUNT:
                            {
                                base.EmvReceipt.ApprovedAmount = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }

                    }
                }

            }
            logger.Trace("Result: " + base.ResultDescription);
            logger.Trace("XLINK ParseResponse() END, Status: " + status.ToString());
            xlinkLogger.AppendCommentsToLogger("Logging Result Description.");
            xlinkLogger.AppendCommentsToLogger(ResultDescription);
            return status;
        }

        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method passes the XCharge error code to get the corresponding error description
        /// from the XchargeErrorCode.xml file.
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : Dec-26-2008
        /// </summary>
        /// <param name="sXChargeErrorCode"></param>
        /// <returns></returns>
        private String GetXChargeErrorDescription(string sXChargeErrorCode)
        {
            //split the value into an array and prefix R in the zeroth element
            String xChargeErrorDescription = String.Empty;
            //split the value into an array and prefix R in the zeroth element
            //Modified by Dharmendra on Jan-28-09 added the splitter :
            string[] errorValues = sXChargeErrorCode.Split(Convert.ToChar(" "), Convert.ToChar(":"));
            //Modified Till Here Jan-28-09
            string respErrorCode = "R" + errorValues[0].ToString();
            MMSDictionary<String, String> xChargeErrorList = new MMSDictionary<string, string>();
            int errorFetchStatus = 0;
            try
            {
                errorFetchStatus = XmlToKeys.GetFields(XCHARGE_ERRORXMLFILE, XAUTHCODE, ref xChargeErrorList, true);
                if (errorFetchStatus > 0)
                {
                    xChargeErrorList.TryGetValue(respErrorCode, out xChargeErrorDescription);
                }
                else
                {
                    xChargeErrorDescription = "UNKNOWN ERROR";
                }
            }
            catch (Exception ex)
            {
                string sException = ex.ToString();
                System.Exception fileNotFoundException = new Exception("Unable To Read the file " + XCHARGE_ERRORXMLFILE, null);
                throw fileNotFoundException;
            }

            return errorValues[0].ToString() + "-" + xChargeErrorDescription;
        }

        private int CloudParseResponse(String xmlResponse, String FilterNode)
        {
            int status = FAILURE;

            //Added By SRT(Gaurav) Date : 20 NOV 2008
            int lPadChars = 20;
            string TagName = string.Empty;
            //End Of Added By SRT(Gaurav)
            xlinkLogger.AppendCommentsToLogger("Logging Responce recieved from XLINK.\n");
            xlinkLogger.AppendResponseToLogger(xmlResponse);
            String value = String.Empty;
            base.ClearFields();
            //Modified By Dharmendra (SRT) on Dec-16-08 placed the accsseing of values from xmlResponse in to try..catch block
            try
            {
                status = XmlToKeys.GetFields(xmlResponse, FilterNode, ref ResponseMsgAllKeys, false);
                logger.Trace("XLink # of Tags: " + status.ToString());
            }
            //catch exception if xmlresponse is not in xml readable format
            catch (Exception ex)
            {
                //Added By Dharmendra (SRT) on Dec-15-08 to initialize the response variables with error details & result failure
                string tempException = ex.ToString();
                base.Result = INV_MESG; //Modified By Dharmendra (SRT) on Dec-16-08 to assign INV_MESG to the result variable
                TagName = (TagName = "Result").PadLeft(lPadChars);
                value = "Either RCM cannot be contacted.";
                value += "\r\nOr the message received from RCM is not valid.\r\nPlease ensure you reverse the transaction manually if it was created.";
                value += "\r\nAlso please check if RCM is running with correct configuration.";
                TagName = (TagName = "RESULT DESCRIPTION").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //Added Till Here Dec-15-08
                logger.Error(ex, "*** XLINK ParseResponse() ERROR \r\n" + tempException + "\r\nResult: " + INV_MESG + "\r\nStatus: " + status.ToString());
                return status;
            }
            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.RESULTMSG, out value))
            {
                logger.Trace("XLINK ParseResponse() Result: " + value.Trim());
                if (value.ToUpper().Trim().Equals(RESULT_SUCCESS))
                {
                    base.Result = RESULT_SUCCESS;
                    //Added By SRT(Gaurav) Date : 20 NOV 2008
                    TagName = (TagName = "Result").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + RESULT_SUCCESS;
                    //End Of Added By SRT(Gaurav)
                    TagName = (TagName = "Details").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                }
                else
                {
                    //Added By Dharmendra (SRT) on Dec-26-08 to find the value of DESCRIPTION field in the response
                    // & to process furthur
                    string sDescriptionValue = string.Empty;
                    if (ResponseMsgAllKeys.TryGetValue(DESCRIPTION, out sDescriptionValue))
                    {
                        //String sXchargeErrorDescription = GetXChargeErrorDescription(sDescriptionValue.Trim()); //Commented To ByPass reading from xml XchargeErrorCode.xml file
                        base.Result = RESULT_FAILURE;
                        TagName = (TagName = "Result").PadLeft(lPadChars);
                        base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                        //End Of Added By SRT(Gaurav)
                        TagName = (TagName = "Details").PadLeft(lPadChars);
                        //base.ResultDescription += " \r\n" + TagName + " : " + sXchargeErrorDescription;
                        //Modified By Dharmendra on Mar-12-09
                        //Bypassing reading the error description fro XchargeErrorCode.xml
                        base.ResultDescription += "\r\n" + TagName + ": " + sDescriptionValue;
                        //Modified Till Here Mar-12-09
                        logger.Trace("***XLINK ParseRespone() FAILURE : " + RESULT_FAILURE);
                        return status; //Added By Dharmendra (SRT) on Dec-17-08
                        //Added Till Here Dec-26-08
                    }
                    else
                    {
                        base.Result = RESULT_FAILURE;
                        //Added By SRT(Gaurav) Date : 20 NOV 2008
                        TagName = (TagName = "Result").PadLeft(lPadChars);
                        base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                        //End Of Added By SRT(Gaurav)
                        TagName = (TagName = "Details").PadLeft(lPadChars);
                        base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                        logger.Error("***XLINK ParseRespone() FAILURE : " + RESULT_FAILURE);
                        return status; //Added By Dharmendra (SRT) on Dec-17-08
                    }
                }
            }
            //Added By Dharmendra (SRT) on Dec-12-08 to assign the error message in case if result tag is missing
            else
            {
                base.Result = RESULT_FAILURE;
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Result").PadLeft(lPadChars);
                base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                TagName = (TagName = "Details").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + xmlResponse.Trim();
                logger.Error("***XLINK ParseRespone() FAILURE : " + RESULT_FAILURE);
                return status; //Added By Dharmendra (SRT) on Dec-17-08
            }
            //End Added
            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.CLOUDTRANSACTIONID, out value))
            {
                logger.Trace("***XLINK ParseRespone() TransactionID: " + value.Trim());
                base.TransactionNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Transaction ID").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(AUTHCODE, out value))
            {
                base.AuthNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Auth Code").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(IIASTRANSACTION, out value))
            {
                base.IsFSATransaction = value; //Added By Dharmendra (SRT) on Dec-16-08 earlier it was excluded wrongly.
                if (value.Trim() == "T")
                {
                    //Added By SRT(Gaurav) Date : 20 NOV 2008
                    TagName = (TagName = "IIAS Transaction").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : Available";
                    //End Of Added By SRT(Gaurav)
                }
                else
                {
                    //Added By SRT(Gaurav) Date : 20 NOV 2008
                    TagName = (TagName = "IIAS Transaction").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : Not Available";
                    //End Of Added By SRT(Gaurav)
                }
            }
            //End Of Added By SRT(Gaurav)
            if (ResponseMsgAllKeys.TryGetValue(AMOUNT_APPROVED, out value))
            {
                base.AmountApproved = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                //Added By Dharmendra (SRT) on Dec-09-08 to check the value of Approved Amount
                if (base.Result == RESULT_SUCCESS && base.AmountApproved.Trim() != string.Empty)
                {
                    TagName = (TagName = "Approved Amount").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                }
            }
            else if (base.Result == RESULT_SUCCESS && base.AmountApproved.Trim() == string.Empty)
            {
                if (ResponseMsgAllKeys.TryGetValue(REQUESTAMOUNTFIELD, out value)) // try to get the requested amount
                {
                    base.AmountApproved = value.Trim();
                    TagName = (TagName = "Approved Amount").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                }
                else if(ResponseMsgAllKeys.TryGetValue(xEmvTags.CLOUDRECEIPTTEXT, out value)) // Cloud - Total Amount
                {
                    string cloudTotalAmount = CloudGetValueFromReceiptText(value, "Total Amount");
                    base.AmountApproved = cloudTotalAmount.Trim();
                    TagName = (TagName = "Approved Amount").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + cloudTotalAmount.Trim();
                }
            }

            if (ResponseMsgAllKeys.TryGetValue(ADDITIONAL_FUNDS_REQUIRED, out value)) //Jenny ? need to get case to test it
            {
                // Added By Dharmendra (SRT) on Nov-28-08
                base.AdditionalFundsRequired = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Additional Funds Required").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }

            //Added By SRT(Gaurav) Date : 18 NOV 2008
            if (ResponseMsgAllKeys.TryGetValue(ACCOUNT, out value))
            {
                base.MaskedCardNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Transaction Account").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.EXPMONTH, out value))
            {
                base.Expiration = value.Trim();
                if (ResponseMsgAllKeys.TryGetValue(xEmvTags.EXPYEAR, out value))
                {
                    base.Expiration += value.Trim();
                }
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Account Expiration").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + base.Expiration;
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(RES_ADDRESS, out value))
            {
                base.Address = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Account Address").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(RES_ZIP, out value))
            {
                base.ZIP = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Account Zip").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }

            if (ResponseMsgAllKeys.TryGetValue(ACCOUNT, out value))
            {
                base.AccountNo = value;
                TagName = (TagName = "Account No").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.CARDBRAND, out value))
            {
                base.CardType = value;
                TagName = (TagName = "Card Type").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }

            #region PRIMEPOS-2738 
            if (ResponseMsgAllKeys.TryGetValue("ORDERID", out value))
            {
                base.TransactionNo = base.TransactionNo + "|" + value.Trim();
                TagName = (TagName = "Order ID").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            #endregion

            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.CLOUDRECEIPTTEXT, out value)) //Cloud - Card Balance
            {
                string cloudBalance = CloudGetValueFromReceiptText(value, "Card Balance");
                if (!string.IsNullOrWhiteSpace(cloudBalance))
                {
                    base.Balance = cloudBalance.Trim();
                    TagName = (TagName = "Card Balance").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + cloudBalance.Trim();
                }
            }
            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.CLOUDCASHBACKAMOUNT, out value)) // CASHBACKAMOUNT - RECEIPT_CASHBACKAMOUNT
            {
                base.CashBack = value.Trim();
                TagName = (TagName = "CashBack Amount").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.CLOUDALIAS, out value))  // ALIAS - XCACCOUNTID
            {
                base.ProfiledID = value.Trim();
                TagName = (TagName = "ProfiledID").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.CLOUDENTRYTYPE, out value)) // ENTRYTYPE - RECEIPT_ENTRYMETHOD
            {
                base.EntryMethod = value.Trim();
                TagName = (TagName = "EntryMethod").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }

            if (EmvReceipt == null && ResponseMsgAllKeys.TryGetValue(xEmvTags.APPIDENTIFIER_AID, out value)) //Jenny this is inside of RECEIPTTEXT
            {
                base.EmvReceipt = new EmvReceiptTags();
                foreach (var r in ResponseMsgAllKeys)
                {
                    switch (r.Key.ToUpper())
                    {
                        case xEmvTags.APPIDENTIFIER_AID:
                            {
                                base.EmvReceipt.AppIndentifer = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APP_PREFEREDNAME_AIDNAME:
                            {
                                base.EmvReceipt.AppPreferedName = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APP_CRYPTOGRAM_AC:
                            {
                                base.EmvReceipt.AppCrytogram = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APP_TRANSACTION_COUNTER_ATC:
                            {
                                base.EmvReceipt.AppTransactionCounter = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TERMINALVERIFY_TVR:
                            {
                                base.EmvReceipt.TerminalVerficationResult = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANS_STATUS_INFO_TSI:
                            {
                                base.EmvReceipt.TransStatusInformation = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.AUTHORIZATION_RESP_CODE_CD:
                            {
                                base.EmvReceipt.AuthorizationResposeCode = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANS_REF_NUM_TRN:
                            {
                                base.EmvReceipt.TransRefNumber = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.VALIDATE_CODE_VC:
                            {
                                base.EmvReceipt.ValidationCode = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.MERCHANTID:
                            {
                                base.EmvReceipt.MerchantID = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANSACTIONID:
                            {
                                base.EmvReceipt.TransID = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.ENTRYLEGEND:
                            {
                                base.EmvReceipt.EntryLegend = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.CARDTYPE_CT:
                            {
                                base.EmvReceipt.AccountType = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.TRANSTYPE:
                            {
                                base.EmvReceipt.TransType = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.VERBIAGE:
                            {
                                base.EmvReceipt.Verbiage = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APPROVAL_CODE:
                            {
                                base.EmvReceipt.ApprovalCode = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.ACCOUNT:
                            {
                                base.EmvReceipt.Account = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }
                        case xEmvTags.APPROVED_AMOUNT:
                            {
                                base.EmvReceipt.ApprovedAmount = r.Value.Trim();
                                TagName = (TagName = r.Key).PadLeft(lPadChars);
                                base.ResultDescription += " \r\n" + TagName + " : " + r.Value.Trim();
                                break;
                            }

                    }
                }

            }

            if (ResponseMsgAllKeys.TryGetValue(xEmvTags.CLOUDRECEIPTTEXT, out value)) // RECEIPTTEXT
            {
                if (EmvReceipt == null)
                    base.EmvReceipt = new EmvReceiptTags();
                base.EmvReceipt.CloudReceiptText = value;
            }

            logger.Trace("Result: " + base.ResultDescription);
            logger.Trace("XLINK ParseResponse() END, Status: " + status.ToString());
            xlinkLogger.AppendCommentsToLogger("Logging Result Description.");
            xlinkLogger.AppendCommentsToLogger(ResultDescription);
            return status;
        }


        private string CloudGetValueFromReceiptText(string receiptText, string fieldName)
        {
            string dataValue = string.Empty;

            string[] allData = receiptText.Split(Convert.ToChar("\n"));
            for(int i=0; i< allData.Length; i++)
            {
                string lineData = allData[i].Trim();
                if(lineData.Contains(fieldName))
                {
                    int idx = lineData.IndexOf('$');
                    dataValue = lineData.Substring(idx+1);
                    break;
                }
            }
            return dataValue;
        }
        #endregion public methods
    }

    //Added By Dharmendra on Dec-06-08
    //The purpose of this class is to provide functionality for Appending transaction related
    //parameters into PPOSTrace.log file.
    public class PrimeChargeLogWriter
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        
        private String loggerString = String.Empty;
        private string isTraceEnable = string.Empty;

        //This a constructor which requires the value whether tracing is enabled or not
        public PrimeChargeLogWriter(string isTraceEnable)
        {
            this.isTraceEnable = isTraceEnable;
        }

        public void AppendCommentsToLogger(string comments)
        {
            LoggerWrite(comments);
        }

        public void AppendResponseToLogger(String dataToLog)
        {
            loggerString = "Payment Responce:\n" + dataToLog;

            LoggerWrite(loggerString);
            loggerString = "";
        }

        //Added By Dharmedra (SRT) on Dec-08-08
        //This method  writes the logger info to C:/PPOSTrace.log file.
        private void LoggerWrite(Object obj)
        {
            if (isTraceEnable.ToUpper().Equals("Y"))
            {
                logger.Trace(obj);
            }
        }

        //Added Till Here
    }
}