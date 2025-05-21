//Author: Manoj Kumar
//Company: Micro Merchant Systems, 2012
//Function: Debit card transaction
//Implementation: For HPS (Heartland Payment Systems)

using System;
using System.Collections.Generic;
using System.Text;
using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.HPS
{
    public class DebitProcessor:Processor,IDebitProcessor
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const string HPS_DEBITSALE = "HPS_DEBITSALE";
        private const string HPS_DEBITREVERSE = "HPS_DEBITREVERSE";
        private const string HPS_DEBITRETURN = "HPS_DEBITRETURN";
        private const string HPS_DEBITVOIDSALE = "HPS_DEBITVOIDSALE";
        private const string HPS_DEBITRETURNVOID = "HPS_DEBITRETURNVOID";
        private const String PROCESSOR = "HPS";
        #endregion

        #region variables
        private string transactionType = String.Empty;
        #endregion

        #region Constructor and Destructor
        public DebitProcessor(MerchantInfo merchant)
            : base(PROCESSOR, merchant)
        {
            //Command = 0;
        }

        ~DebitProcessor()
        {
            System.Diagnostics.Debug.Print("DebitProcessor destructor\n");
           // Command = 0;
        }
        #endregion

        #region IDebitProcessor Members

        /// <summary>
        /// Author : Manoj Kumar 
        /// Desciption :  Debit Card Sale Transaction
        /// External functions:MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public string Sale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--HPS DEBIT PROCESSOR - (Start) Sale() HPS_DEBITSALE"); //write to Log
            transactionType = HPS_DEBITSALE; // Transaction type
            if (!ValidateParameters(ref requestMsgKeys)) //check all parameters to pass to the process 
            {
                pmtResp = null;
                logger.Error("***HPS DEBIT PROCESSOR - HPS_DEBITSALE Invalid Parameters: " + MMS.HPS.Processor.INVALID_PARAMETERS); //write error to log
                return MMS.HPS.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys); //send to get the work done processtxn and return result
            if (pmtResp == null)
            {
                logger.Error("***HPS DEBIT PROCESSOR - HPS_DEBITSALE Payment Response is NULL"); //write the error
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS DEBIT PROCESSOR - (ENDING) HPS_Sale Payment Response Result: " + pmtResp.Result); // wite the result
            return pmtResp.Result;
        }

        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Debit Card Return
        /// External functions:MMSDictioanary,PaymentResponse 
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public string Return(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--HPS DEBIT PROCESSOR - (Start) Return() HPS_DEBITRETURN");
            transactionType = HPS_DEBITRETURN;
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS DEBIT PROCESSOR - Return Invalid Parameters: " + MMS.HPS.Processor.INVALID_PARAMETERS);
                return MMS.HPS.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***HPS DEBIT PROCESSOR - Return Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS DEBIT PROCESSOR - (ENDING) Return Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }


        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : This method does a VOIDSALE transaction on the Debit card (Just for implementation)
        /// External functions:MMSDictioanary,PaymentResponse 
        /// Known Bugs : None
        /// Start Date :  26 JULY 2010.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public string Void(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("HPS DEBIT PROCESSOR - (Start) Void() HPS_DEBITREVERSE");
            transactionType = HPS_DEBITREVERSE;
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS DEBIT PROCESSOR - Void Invalid Parameters: " + MMS.HPS.Processor.INVALID_PARAMETERS);
                return MMS.HPS.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***HPS DEBIT PROCESSOR - Void Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS DEBIT PROCESSOR - (ENDING) Void Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Debit Card Void Return (Just for implementation)
        /// External functions:MMSDictioanary,PaymentResponse 
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public string VoidReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("HPS DEBIT PROCESSOR - (Start) VoidReturn() HPS_DEBITREVERSE");
            transactionType = HPS_DEBITREVERSE;
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS DEBIT PROCESSOR - VoidReturn Invalid Parameters: " + MMS.HPS.Processor.INVALID_PARAMETERS);
                return MMS.HPS.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***HPS DEBIT PROCESSOR - VoidReturn Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS DEBIT PROCESSOR - (ENDING) VoidReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }


        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Debit Card Reversal.
        /// External functions:MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("HPS DEBIT PROCESSOR - (Start) Reverse() HPS_DEBITREVERSE");
            transactionType = HPS_DEBITREVERSE;
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS DEBIT PROCESSOR - Reverse Invalid Parameters: " + MMS.HPS.Processor.INVALID_PARAMETERS);
                return MMS.HPS.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***HPS DEBIT PROCESSOR - Reverse Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS DEBIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        #endregion
    }
}
