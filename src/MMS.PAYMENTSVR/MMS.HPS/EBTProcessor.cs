//Author: Manoj Kumar
//Company: Micro Merchant Systems, 2012
//Function: EBT transaction
//Implementation: For HPS (Heartland Payment Systems)

using System;
using System.Collections.Generic;
using System.Text;
using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.HPS
{
    public class EBTProcessor:Processor,IEbtProcessor
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private string transactionType = String.Empty;
        private const string HPS_EBTSALES = "HPS_EBTSALES";
        private const string HPS_EBTRETURN = "HPS_EBTRETURN";
        private const String PROCESSOR = "HPSEBT";

        public EBTProcessor(MerchantInfo merchant)
            : base(PROCESSOR, merchant)
        {
            //Command = 0;
        }

        ~EBTProcessor()
        {
            logger.Trace("EBT Processor destructor\n");
        }

        /// <summary>
        /// Author: Manoj Kumar
        /// Description: EBT Sale Transaction
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public String EBTSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--HPS EBT PROCESSOR - (Start) EBTSale() HPS_EBTSALES"); //write to log
            transactionType = HPS_EBTSALES;  //card type
            if (!ValidateParameters(ref requestMsgKeys)) //check all parameters to pass to the process 
            {
                pmtResp = null;
                logger.Error("***HPS EBT PROCESSOR - EBTSale Invalid Parameters: " + MMS.HPS.Processor.INVALID_PARAMETERS); //write error log
                return MMS.HPS.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys); //send to get the work done processtxn and return result
            if (pmtResp == null)
            {
                logger.Error("***HPS EBT PROCESSOR - EBTSale Payment Response is NULL"); //write error log
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS EBT PROCESSOR - (ENDING) EBTSale Payment Response Result: " + pmtResp.Result); // write the result
            return pmtResp.Result;
        }

        /// <summary>
        /// Author: Manoj Kumar
        /// Description: EBT Return Transaction
        /// External functions: MMSDictioanary,PaymentResponse
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public String EBTReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--HPS EBT PROCESSOR - (Start) EBTReturn() HPS_EBTRETURN");
            transactionType = HPS_EBTRETURN;
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPS EBT PROCESSOR - EBTReturn Invalid Parameters: " + MMS.HPS.Processor.INVALID_PARAMETERS);
                return MMS.HPS.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***HPS EBT PROCESSOR - EBTReturn Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("HPS EBT PROCESSOR - (ENDING) EBTReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
    }
}
