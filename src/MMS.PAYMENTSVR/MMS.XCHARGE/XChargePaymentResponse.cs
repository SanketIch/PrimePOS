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
using Logger = AppLogger.AppLogger;

namespace MMS.XCHARGE
{
    //Author : Ritesh    
    //Functionality Desciption : The purpose of this class is to provide response of PCCharge Server on any transaction.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 29 January 2008.    
    public class XChargePaymentResponse : PaymentResponse
    {
        #region variables

        #endregion

        #region constants
        //Modified By SRT(Gaurav) Date : 24-NOV-2008
        //Mantis ID: 0000136
        private const String RESULT = "RESULT_SUCCESS";
        private const String RESULT_DESCRIPTION = "RESULT_DESCRIPTION";
        //Added By SRT(Gaurav) Date : 20-NOV-2008 
        private const String RESULT_SUCCESS = "SUCCESS";
        private const String RESULT_FAILURE = "FAILURE";
        //End Of Added By SRT(Gaurav)
        private const String TRANSACTIONID = "RESULT_XCTRANSACTIONID";
        private const String AUTHCODE = "RESULT_APPROVALCODE";
        //Need to check these tags
        private const String AMOUNT_APPROVED = "RESULT_APPROVEDAMOUNT";
        private const String ADDITIONAL_FUNDS_REQUIRED = "RESULT_ADDITIONALFUNDSREQUIRED";
        //Added By SRT(Gaurav) Date: 18 NOV 2008
        private const String RES_ACCOUNT = "RESULT_XACCOUNT";
        private const String RES_EXPIRATION = "RESULT_EXPIRATION";
        private const String RES_ADDRESS = "RESULT_ADDRESS";
        private const String RES_ZIP = "RESULT_ZIP";
        private const String IIASTRANSACTION = "RESULT_IIASTRANSACTION";
        private const string REQUESTAMOUNTFIELD = "AMOUNT"; //Modified By Dharmendra (SRT) on Dec-15-08 Replace constant name
        //Added By Dharmendra (SRT) on Dec-16-08
        private const string INV_PROCESSOR = "INV_PROCESSOR"; 
        private const string INV_MESG = "INV_MESG";
        private const string ACCOUNTTYPE = "RESULT_ACCOUNTTYPE";
        //End Added Dec-16-08
        //End Of Added By SRT(Gaurav)
        //End Of Modified By SRT(Gaurav)
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
        public XChargePaymentResponse()
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
        //Modified By SRT(Gaurav) Date : 24-NOV-2008
        public override int ParseResponse(String xmlResponse, String FilterNode)
        {
            Logger.LogWritter("XCHARGE ParseRespone Start");
            int status = FAILURE;
            String value = String.Empty;
            //Added By SRT(Gaurav) Date : 20 NOV 2008
            int lPadChars = 20;
            string TagName = string.Empty;
            //End Of Added By SRT(Gaurav)

            //Clear old Data
            base.ClearFields();
            //ResponseMsgAllKeys should have all the Key Values available from the response
            //Modified By Dharmendra (SRT) on Dec-15-08 placed the accsseing of values from xmlResponse in to try..catch block 
            try
            {
                status = XmlToKeys.GetFields(xmlResponse, FilterNode, ref ResponseMsgAllKeys, false);
                Logger.LogWritter("XCHARGE ParseResponse() Status Failure = 1, Success = 0, status: " + status.ToString());
            }
            //catch the exception if xml response is not in xml readable format 
            catch (Exception ex)
            {
                //Added By Dharmendra (SRT) on Dec-15-08 to initialize the response variables with error details & result failure
                string tempException = ex.ToString();
                base.Result = INV_MESG; //Modified By Dharmendra (SRT) on Dec-16-08 to assign INV_MESG to the result variable
                TagName = (TagName = "Result").PadLeft(lPadChars);               
                value = "\r\nThe message received from XCharge Processor is not valid.\r\nPelease ensure you reverse the transaction manually if it exist in X-Charge Payment Report.";
                TagName = (TagName = "RESULT DESCRIPTION").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                Logger.LogWritter("*** XCHARGE ParseResponse() ERROR \r\n" + tempException + "\r\nResult: " + INV_MESG + "\r\nStatus: " + status.ToString());
                return status;
                //Added Till Here Dec-15-08 
            }
            if (ResponseMsgAllKeys.TryGetValue(RESULT, out value))
            {
                Logger.LogWritter("XCHARGE ParseResponse() Result: " + value.Trim());
                if (value.ToUpper().Trim().Equals("T"))
                {
                    base.Result = RESULT_SUCCESS;
                    //Added By SRT(Gaurav) Date : 20 NOV 2008
                    TagName = (TagName = "Result").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + RESULT_SUCCESS;
                    //End Of Added By SRT(Gaurav)
                }
                //Modified & Added By Dharmendra (SRT) on Dec-16-08      
                else if (value.ToUpper().Trim().Equals("F"))
                {
                    base.Result = RESULT_FAILURE;
                    TagName = (TagName = "Result").PadLeft(lPadChars);                   
                    TagName = (TagName = "RESULT DESCRIPTION").PadLeft(lPadChars);
                    //Added By Dharmendra on Jan-28-09 to reflect the value of result description
                    ResponseMsgAllKeys.TryGetValue(RESULT_DESCRIPTION, out value);
                    //Added Till Here Jan-28-09
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                    Logger.LogWritter("***XCHARGE ParseRespone() FAILURE : " + RESULT_FAILURE);
                    return status; //Added By Dharmendra (SRT) on Dec-17-08
                    
                }
                else if (value.ToUpper().Trim().Equals(INV_PROCESSOR)) //check the result value for invalid processor
                {
                    base.Result = INV_PROCESSOR; //assign the value INV_PROCESSOR to the result variable
                    TagName = (TagName = "Result").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n Result : " + INV_PROCESSOR;
                    ResponseMsgAllKeys.TryGetValue(INV_PROCESSOR, out value);
                    TagName = (TagName = "RESULT DESCRIPTION").PadLeft(lPadChars);
                    base.ResultDescription += " \r\n" + TagName + " : " + value.Trim(); //assigning the value to Result Description
                    Logger.LogWritter("***XCHARGE ParseRespone() FAILURE : " + INV_PROCESSOR);
                    return status; //Added By Dharmendra (SRT) on Dec-17-08
                }
                //Modified & Added Till Here Dec-16-08
            }
            //Added By SRT(Gaurav) Date : 22-NOV-2008
            if (ResponseMsgAllKeys.TryGetValue(RESULT_DESCRIPTION, out value))
            {
                TagName = (TagName = "RESULT DESCRIPTION").PadLeft(lPadChars);
                base.ResultDescription += " \r\n Result : " + value.Trim();
            }
            //End Of Added By SRT(Gaurav)
            if (ResponseMsgAllKeys.TryGetValue(TRANSACTIONID, out value))
            {
                base.TransactionNo = value.Trim();
                //Added By SRT(Gaurav) Date : 20-NOV-2008
                TagName = (TagName = "Transaction ID").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                Logger.LogWritter("XCHARGE ParseRespone() TransactionID: " + value.Trim());
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
            //Added By Dharmendra(SRT) on Nov-27-08
            if (ResponseMsgAllKeys.TryGetValue(IIASTRANSACTION, out value))
            {
                base.IsFSATransaction = value.Trim();
                //Added By Dharmendra(SRT) on Nov-27-08
                TagName = (TagName = "Account IsFSATransaction").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                //End Added
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

                //Added Till Here

                //End Of Added By SRT(Gaurav)
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
            // End Added Dec-15-08 

            if (ResponseMsgAllKeys.TryGetValue(ADDITIONAL_FUNDS_REQUIRED, out value))
            {
                base.AdditionalFundsRequired = value.Trim(); // Added By Dharmendra (SRT) on Nov-28-08
                //Added By SRT(Gaurav) Date : 20 NOV 2008
                TagName = (TagName = "Additional Funds Required").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();
                Logger.LogWritter("XCHARGE ParseRespone() Additional Funds Required: " + value.Trim());
                //End Of Added By SRT(Gaurav)
            }
            //Added By SRT(Gaurav) Date : 18 NOV 2008
            if (ResponseMsgAllKeys.TryGetValue(RES_ACCOUNT, out value))
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
            if (ResponseMsgAllKeys.TryGetValue(ACCOUNTTYPE, out value))
            {
                
                base.CardType = value;
                TagName = (TagName = "Card Type").PadLeft(lPadChars);
                base.ResultDescription += " \r\n" + TagName + " : " + value.Trim();

            }
            Logger.LogWritter("XCHARGE ParseResponse() END, Status: " + status.ToString());
            return status;
        }
        //End Of Modified By SRT(Gaurav)
        #endregion
    }
}
