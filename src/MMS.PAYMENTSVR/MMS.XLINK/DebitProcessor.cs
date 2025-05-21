//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make Credit card transactions.
//External functions:None   
//Known Bugs : None
//Start Date : 14 January 2008.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.XLINK
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make Credit card transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 14 January 2008.
    public class DebitProcessor : MMS.XLINK.Processor, IDebitProcessor 
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "XLDEBIT";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String XLC_DEBITSALE = "DebitPurchase";
        private const String XLC_DEBITRETURN = "DebitReturn";
        private const String XLC_VOID = "Void";
        private const String XLC_VOIDRETURN = "VoidReturn";
        private const String XLC_DEBITREVERSE = "XLC_DEBITREVERSE"; 

        #endregion

        #region variables
        private string transactionType = String.Empty;
        //Added By SRT(Gaurav) Date : 8 NOV 2008
        private string AppPath = String.Empty;
        //End Of Added By SRT(Gaurav)
        #endregion
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="merchantNumber"></param>
        /// <param name="userId"></param> 
        /// Changed By SRT(Gaurav)
        /// Date : 08 NOV 2008
        /// Details : private member AppPath initialized by parameter tAppPath
        public DebitProcessor(MerchantInfo merchant): base(PROCESSOR, merchant)
        {
            //Command = 0;
        }  
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// 


        ~DebitProcessor()
        {
            System.Diagnostics.Debug.Print("DebitProcessor destructor\n");
           // Command = 0;
        }

        #region IDebitProcessor Members
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method does a CREDIT transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Return(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--XLINK DEBIT PROCESSOR - (Start) Return() XLC_DEBITRETURN");
            transactionType = XLC_DEBITRETURN;
            requestMsgKeys.Add(COMMAND, XLC_DEBITRETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK DEBIT PROCESSOR - XLC_DEBITRETURN Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK DEBIT PROCESSOR - XLC_DEBITRETURN Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK DEBIT PROCESSOR - (ENDING) XLC_DEBITRETURN Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;  
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption :  This method does a SALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public String Sale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--XLINK DEBIT PROCESSOR - (Start) Sale() XLC_DEBITSALE");
            transactionType = XLC_DEBITSALE;
            requestMsgKeys.Add(COMMAND, XLC_DEBITSALE);            
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK DEBIT PROCESSOR - XLC_DEBITSALE Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK DEBIT PROCESSOR - XLC_DEBITSALE Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK DEBIT PROCESSOR - (ENDING) XLC_DEBITSALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method does a VOIDSALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Void(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK DEBIT PROCESSOR - (Start) Void() XLC_VOID");
            transactionType = XLC_VOID;
            requestMsgKeys.Add(COMMAND, XLC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK DEBIT PROCESSOR - Void Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK DEBIT PROCESSOR - Void Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK DEBIT PROCESSOR - (ENDING) Void Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }

        /// <summary>
        /// Author : Gaurav 
        /// Functionality Desciption : This method does a VOIDRETURN transaction on the debit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 24-NOV-2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public string VoidReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK DEBIT PROCESSOR - (Start) VoidReturn() XLC_VOIDRETURN");
            transactionType = XLC_VOIDRETURN;
            requestMsgKeys.Add(COMMAND, XLC_VOIDRETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK DEBIT PROCESSOR - VoidReturn Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK DEBIT PROCESSOR - VoidReturn Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK DEBIT PROCESSOR - (ENDING) VoidReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK DEBIT PROCESSOR - (Start) Reverse() XLC_DEBITREVERSE");
            transactionType = XLC_DEBITREVERSE;
            requestMsgKeys.Add(COMMAND, XLC_DEBITREVERSE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK DEBIT PROCESSOR - Reverse Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK DEBIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK DEBIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
