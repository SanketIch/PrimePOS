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
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.PCCHARGE
{
    //Author : Ritesh    
    //Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 29 January 2008.    
    public class PCChargePaymentResponse : PaymentResponse
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region variables
        #endregion

        #region constants
        private const String RESULT = "RESULT";
        private const String TRANSACTIONID = "TROUTD";
        private const String AUTHCODE = "AUTH_CODE";
        private const String AMOUNT_APPROVED = "AUTH_AMOUNT";
        //Added & Updated By Dharmendra (SRT) on Nov-28-08
        private const String ADDITIONAL_FUNDS_REQUIRED = "AMOUNT_DUE";
        //End Added & Updated        

        private const String RESULT_CAPTURED = "CAPTURED";
        private const String RESULT_FAILURE = "FAILURE";
        private const String RESULT_SUCCESS = "SUCCESS";
        private const String RESULT_APPROVED = "APPROVED";
        private const String RESULT_VOIDED = "VOIDED";
        private const String RESULT_PROCESSED = "PROCESSED";
        private const String RESULT_SALERECEIVED = "SALERECEIVED";
        private const String RESULT_RETURNRECOVERED = "RETURNRECOVERED";
        private const String RESULT_NOT_CAPTURED = "NOT CAPTURED";
        private const String RESULT_NOT_APPROVED = "NOT APPROVED";
        private const String RESULT_CANCELLED = "CANCELLED";
        private const String RESULT_SALE_NOT_FOUND = "SALE NOT FOUND";
        private const String RESULT_REVERSED = "REVERSED";
        //Added By SRT(Gaurav) Date: 01-Dec-2008
        private const String TRANS_DATE = "TRANS_DATE";
        private const string REQUESTAMOUNTFIELD = "TRANS_AMOUNT"; // Modified By Dharmendra (SRT) on Dec-15-08 Replace TRANSAMOUNT = "TransAmount"
        //End Of Added By SRT(Gaurav)        
        //Added By SRT(Gaurav) Date: 04-Dec-2008
        //Mantis Id: 0000136
        private const String MAX_AUTH_AMOUNT = "MAX_AUTH_AMOUNT";
        private const int AmountRoundDigit = 2;
        private const string INVALIDPAYMENT_PROCESSOR = "INVALID PAYMENT PROCESSOR SELECTION"; //Added By Dharmendra (SRT) on Dec-15-08
        //Added By Dharmendra (SRT) on Dec-16-08
        private const string INV_PROCESSOR = "INV_PROCESSOR"; 
        private const string INV_MESG = "INV_MESG";       
        //Added Til Here Dec-16-08
        //End Of Addded By SRT(Gaurav)
        #endregion

        #region public methods
        //Constructor

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the PCChargePaymentResponse class
        /// External functions:MMSDictioanary
        /// Known Bugs : None
        /// Start Date : 29 Jan 2008.
        /// </summary>
        public PCChargePaymentResponse()
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
            logger.Trace("PCCHARGE ParseRespone Start");
            int status = FAILURE;
            String value = String.Empty;
            //Added By SRT(Gaurav) Date : 20 NOV 2008
            int tempPadChars = 20;
            string TagName = string.Empty;
            string tempResultFound = string.Empty;
            //End Of Added By SRT(Gaurav)
            //Added By SRT(Gaurav) Date : 04-Dec-2008
            //Details To Implement Additional Funds Required.
            Double tempMaxAuthAmount, tempAuthAmount, tempAmtDue, tempAddFundsReq;
            tempMaxAuthAmount = 0;
            tempAuthAmount = 0;
            tempAmtDue = 0;
            tempAddFundsReq = 0;
            //End Of Added By SRT(Gaurav)
            //Clear old Data
            base.ClearFields();

            //ResponseMsgAllKeys should have all the Key Values available from the response
            //Modified By Dharmendra (SRT) on Dec-15-08 placed the accsseing of values from xmlResponse in to try..catch block 
            try
            {
                status = XmlToKeys.GetFields(xmlResponse, FilterNode, ref ResponseMsgAllKeys, false);
                logger.Trace("PCCHARGE ParseResponse() Status: " + status.ToString());
            }
            //catching the exception assigning the custom error message to ResultDescription
            catch (Exception ex)
            {
                //Added By Dharmendra (SRT) on Dec-15-08 to initialize the response variables with error details & result failure
                string tempException = ex.ToString();
                base.Result = INV_MESG; //Modified By Dharmendra (SRT) on Dec-16-08 to assign INV_MESG to the result variable
                TagName = (TagName = "Result").PadLeft(tempPadChars);                
                value = "\r\nThe message received from PCCharge is not valid.\r\nPelease ensure you reverse the transaction manually if it exist in PCCharge Payment Report.";
                TagName = (TagName = "RESULT DESCRIPTION").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                logger.Trace("*** PCCHARGE ParseResponse() ERROR \r\n" + tempException + "\r\nResult: " + INV_MESG+"\r\nStatus: "+status.ToString());
                return status;
                //Added Till Here Dec-15-08
            }
            if (ResponseMsgAllKeys.TryGetValue(RESULT, out value))
            {
                logger.Trace("PCCHARGE ParseResponse() Result: " + value.Trim());
                tempResultFound = value.Trim();
                switch (value.ToUpper().Trim())
                {
                    case RESULT_CAPTURED:
                    case RESULT_APPROVED:
                    case RESULT_VOIDED:
                    case RESULT_PROCESSED:
                    case RESULT_SALERECEIVED:
                    case RESULT_RETURNRECOVERED:
                    case RESULT_REVERSED:
                    {
                        base.Result = RESULT_SUCCESS;
                        //Added By SRT(Gaurav) Date : 20 NOV 2008
                        TagName = (TagName = "Result").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n" + TagName + " : " + RESULT_SUCCESS;
                        //End Of Added By SRT(Gaurav)
                        TagName = (TagName = "Details").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                        break;
                    }
                    case RESULT_NOT_CAPTURED:
                    case RESULT_NOT_APPROVED:
                    case RESULT_CANCELLED:
                    case RESULT_SALE_NOT_FOUND:
                    case RESULT_FAILURE:
                    {
                        base.Result = RESULT_FAILURE;                 
                        logger.Trace("***PCCHARGE ParseRespone()FAILURE : " + RESULT_FAILURE);
                        //Added By SRT(Gaurav) Date : 20 NOV 2008
                        TagName = (TagName = "Result").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                        //End Of Added By SRT(Gaurav)
                        TagName = (TagName = "Details").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();                           
                        break;
                    }
                    //Modified & Added By Dharmendra (SRT) on Dec-16-08 to read whether the response contains invalid processor 
                    case INV_PROCESSOR:
                    {
                        TagName = (TagName = "Result").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n Result : " + INV_PROCESSOR;
                        ResponseMsgAllKeys.TryGetValue(INV_PROCESSOR, out value);
                        TagName = (TagName = "Details").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                        //Modified & Added Till Here Dec-16-08
                        logger.Trace("***PCCHARGE ParseRespone() Processor FAILURE : " + INV_PROCESSOR);
                        break;
                    }                    
                    default:
                    {
                        base.Result = RESULT_FAILURE;
                         logger.Trace("***PCCHARGE ParseRespone() Default FAILURE : " + RESULT_FAILURE);
                        //Added By SRT(Gaurav) Date : 20 NOV 2008
                        TagName = (TagName = "Result").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n Result : " + RESULT_FAILURE;
                        //End Of Added By SRT(Gaurav)
                        break;
                    }
                }

            }
            else
            {
                ResponseMsgAllKeys.TryGetValue("XML_FILE", out value);
                logger.Trace("*** PCCHARGE ParseRespone() Failure: " + RESULT_FAILURE + "\r\nResult: " + value.Trim());
                base.Result = RESULT_FAILURE;
                TagName = (TagName = "Result").PadLeft(tempPadChars);
                //Added By SRT(Gaurav) Date : 24-NOV-2008
                TagName = (TagName = "ERROR").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n Result : " + value.Trim();
                //End Of Added By SRT(Gaurav)
                return status; //Added By Dharmendra (SRT) on Dec-17-08

            }

            if (ResponseMsgAllKeys.TryGetValue(TRANSACTIONID, out value))
            {
                logger.Trace("PCCHARGE ParseRespone() TransactionID: " + value.Trim());
                base.TransactionNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Transaction ID").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            if (ResponseMsgAllKeys.TryGetValue(AUTHCODE, out value))
            {
                base.AuthNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Auth Code").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            // Get values for UserID,Troutd,Result,AuthCode
            if (ResponseMsgAllKeys.TryGetValue(AMOUNT_APPROVED, out value))
            {
                base.AmountApproved = value.Trim();
                tempAuthAmount = Math.Round(Double.Parse(value.Trim()), AmountRoundDigit);
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Approved Amount").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //Code commented by SRT(Gaurav) Date: 02-JUL-2009
                //Code Commented For diabling the FSA transaciton to avoid the issue of return transaction.
                //base.IsFSATransaction = "T";
                //End Of Commentd By SRT(Gaurav)
                //End Of Added By SRT(Gaurav)
            }
            else
            {
                base.IsFSATransaction = "F";
                if (base.Result == RESULT_SUCCESS)
                {
                    //Added By SRT(Gaurav) Date: 04-Dec-2008
                    if (ResponseMsgAllKeys.TryGetValue(REQUESTAMOUNTFIELD, out value)) // Modified By Dharmendra (SRT) on Dec-15-08 Replaced the constant name TRANSAMOUNT
                    {
                        base.AmountApproved = value.Trim();
                        TagName = (TagName = "Approved Amount").PadLeft(tempPadChars);
                        base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                    }

                    //End Of Added By SRT(Gaurav)
                }
            }
            //Added By SRT(Gaurav) Date : 04-Dec-2008
            if (ResponseMsgAllKeys.TryGetValue(MAX_AUTH_AMOUNT, out value))
            {
                tempMaxAuthAmount = Math.Round(Double.Parse(value.Trim()), AmountRoundDigit);
                TagName = (TagName = "Transaction Amount").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            //End Of Added By SRT(Gaurav)
            //Changed By SRT(Gaurav) Date : 02-Dec-2008 
            //Details : Condition enhanced for availability of Additional Funds Required by 
            //Checking Reponse Tag Amount Due.

            if (ResponseMsgAllKeys.TryGetValue(ADDITIONAL_FUNDS_REQUIRED, out value))
            {
                //Added By SRT(Gaurav) Date : 04-Dec-2008
                if (Double.TryParse(value.Trim(), out tempAmtDue) == true && tempAmtDue > 0)
                {

                    if (tempMaxAuthAmount > 0 && tempAuthAmount > 0)
                    {

                        if (tempAmtDue >= tempAuthAmount)
                        {
                            tempAddFundsReq = Math.Round((tempAuthAmount - (tempMaxAuthAmount - tempAmtDue)), AmountRoundDigit);
                        }
                        else
                        {
                            tempAddFundsReq = 0;
                        }
                    }
                    //Following condition to check if arise or to remove.
                    //Code To Remove After Testing
                    else if (tempMaxAuthAmount > 0 && tempAuthAmount == 0)
                    {
                        tempAddFundsReq = Math.Round(tempMaxAuthAmount - tempAmtDue, AmountRoundDigit);
                    }

                    else if (tempMaxAuthAmount == 0 && tempAuthAmount > 0)
                    {
                        tempAddFundsReq = Math.Round(tempAuthAmount - tempAmtDue, AmountRoundDigit);
                    }
                    //End Of Code To Remove After Testing.
                }


                base.AdditionalFundsRequired = tempAddFundsReq.ToString(); //Added By Dharmendra (SRT) on Nov-28-08
                logger.Trace("PCCHARGE ParseRespone() Additional Funds Required: " + tempAddFundsReq.ToString());
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Additional Funds Required").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + tempAddFundsReq.ToString();
                //End Of Added By SRT(Gaurav)
                //Added By SRT(Gaurav) Date : 04-Dec-2008
                TagName = (TagName = "Amount Due").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Of Added By SRT(Gaurav)
            }
            //Added By SRT(Gaurav) Date: 01-Dec-2008
            //Details : Added For Retriving TransDate From Response Object.
            if (ResponseMsgAllKeys.TryGetValue(TRANS_DATE, out value))
            {
                base.TransDate = value.Trim();
                TagName = (TagName = "Trans Date").PadLeft(tempPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
            }
            //End Of Added By SRT(Gaurav)           
            logger.Trace("PCCHARGE ParseResponse() END, Status: " + status.ToString());
        return status;
    }
        #endregion
    }
}
