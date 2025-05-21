//Author: Manoj Kumar
//Company: Micro Merchant Systems, 2012
//Function: Credit card transaction
//Implementation: For HPS (Heartland Payment Systems)

using System;
using System.Collections.Generic;
using System.Text;
using MMS.PROCESSOR;
using Hps.Exchange.PosGateway.Client;
using System.Diagnostics;
//using Logger = AppLogger.AppLogger;
using NLog;
namespace MMS.HPS
{
    public class CreditProcessor : MMS.HPS.Processor, ICreditProcessor 
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
         
        #region constants
        private const String PROCESSOR = "HPSCREDIT";
        private const String COMMAND = "COMMAND";
        private const string MMSCARD = "MMSCARD";
        private const string HPS_SALE = "HPS_SALE";
        private const string HPS_VOID = "HPS_VOID";
        private const string HPS_RETURN = "HPS_RETURN";
        private const string HPS_VOIDCREDIT = "HPS_VOIDCREDIT";
        private const string HPS_PRE_AUTH = "HPS_PRE_AUTHORIZATION";
        private const string HPS_POST_AUTH = "HPS_POST_AUTHORIZATION";
        private const string HPS_REVERSE = "HPS_REVERSE";
        #endregion

        #region variables
        private int Command = 0;
        private string transactionType = string.Empty;

        #endregion

        #region Constructor
        public CreditProcessor(MerchantInfo merchant)
            : base(PROCESSOR, merchant)
        {
            Command = 0;
        }

        ~CreditProcessor()
        {
            System.Diagnostics.Debug.Print("CreditProcessor destructor\n");
            Command = 0;
        }
           #endregion 


        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Credit Card SALE transaction
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public String Sale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--HPS CREDIT PROCESSOR - (Start) Sale() HPS_SALE"); //write to log
            string tempActualCardType = string.Empty;
            transactionType = HPS_SALE; //transaction Type
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType); //card type

            if (!ValidateParameters(ref requestMsgKeys)) //check all parameters to pass to the process 
            {
                pmtResp = null;
                logger.Warn("***HPS CREDIT PROCESSOR - HPS_Sale Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys); //send to get the work done processtxn and return result
            
            if (pmtResp != null)
            {
                pmtResp.CardType = tempActualCardType;
            }

            if (pmtResp == null)
            {
                logger.Error("***HPS CREDIT PROCESSOR - HPS_Sale Payment Response is NULL"); //write the error
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS CREDIT PROCESSOR - (ENDING) HPS_Sale Payment Response Result: " + pmtResp.Result); // wite the result
            return pmtResp.Result;
        }

        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Credit Card Void Transaction
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String VoidSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("HPS CREDIT PROCESSOR - (Start) VoidSale() HPS_VOID");
            string tempActualCardType = string.Empty;
            transactionType = HPS_VOID;
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS CREDIT PROCESSOR - VoidSale Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null)
            {
                pmtResp.CardType = tempActualCardType;
            }
            if (pmtResp == null)
            {
                logger.Error("***HPS CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Credit Card Credit(Return) Transaction
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Credit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--HPS CREDIT PROCESSOR - (Start) Credit() HPS_RETURN");
            string tempActualCardType = string.Empty;
            transactionType = HPS_RETURN;
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType);

            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS CREDIT PROCESSOR - Credit Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null)
            {
                pmtResp.CardType = tempActualCardType;
            }
            if (pmtResp == null)
            {
                logger.Error("***HPS CREDIT PROCESSOR - Credit Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }


        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : Credit Card Void transaction
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String VoidCredit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("HPS CREDIT PROCESSOR - (Start) VoidCredit HPS_VOID");
            string tempActualCardType = string.Empty;
            transactionType = HPS_VOID;
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType);

            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null)
            {
                pmtResp.CardType = tempActualCardType;
            }

            if (pmtResp == null)
            {
                logger.Error("***HPS CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;

        }

        /// <summary>
        /// Author : Manoj Kumar
        /// Desciption : PREAUTH transaction on the credit card ( we are not using this as yet, just for implementation)
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String PreAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            transactionType = HPS_PRE_AUTH;
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return Processor.FAILED_OPRN;
            return pmtResp.Result; 
        }


        /// <summary>
        /// Author : Manoj Kumar 
        /// Desciption : POSTAUTH transaction on the credit card (we are not using this as yet,just for implementation)
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String PostAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            transactionType = HPS_POST_AUTH;
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return Processor.FAILED_OPRN;
            return pmtResp.Result; 
        }

        /// <summary>
        /// Author : Manoj Kumar 
        /// Desciption : Credit Card Reversal transaction
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Reverse(ref MMSDictionary<String, String> requsetMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("HPS CREDIT PROCESSOR - (Start) Reverse() HPS_REVERSE");
            transactionType = HPS_REVERSE;
            if (!ValidateParameters(ref requsetMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS CREDIT PROCESSOR - Reverse Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requsetMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***HPS CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }

        #region PRIMEPOS-3526
        public string PreRead(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            throw new NotImplementedException();
        }

        public string PreReadSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            throw new NotImplementedException();
        }

        public string PreReadCancel(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            throw new NotImplementedException();
        }

        public string PreReadReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
