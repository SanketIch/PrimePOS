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

namespace MMS.PCCHARGE
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make Credit card transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 14 January 2008.
    public class DebitProcessor : Processor, IDebitProcessor 
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "DEBIT";
        private const String COMMAND = "COMMAND";
        #endregion

        #region variables
        private int Command = 0;
        #endregion
        //Enumerator for Credit Transaction Codes    
        private enum DebitTxns
        {
            Sale = 41,
            Return = 42,
            Void = 43,
            VoidReturn = 44,
            Reverse=45
        };

        #region Constructor and Destructor
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
        public DebitProcessor(MerchantInfo merchant):base(PROCESSOR,merchant)
        {
            Command = 0;
        }  
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        ~DebitProcessor()
        {
            System.Diagnostics.Debug.Print("DebitProcessor destructor\n");
            Command = 0;
        }
        #endregion

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
        public String Return(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--PCCHARGE DEBIT PROCESSOR - (Start) Return()");
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE DEBIT PROCESSOR - Return Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }

            Command = (int)DebitTxns.Return;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)DebitTxns.Return, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE DEBIT PROCESSOR - Return Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (ENDING) Return Payment Response Result: " + pmtResp.Result);
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
            logger.Trace("--PCCHARGE DEBIT PROCESSOR - (Start) Sale()");
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE DEBIT PROCESSOR - SALE Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            Command = (int)DebitTxns.Sale;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)DebitTxns.Sale, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE DEBIT PROCESSOR - SALE Payment Response is NULL");
                return FAILED_OPRN;
            }
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (ENDING) Sale Payment Response Result: " + pmtResp.Result);
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
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (Start) Void()");
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE DEBIT PROCESSOR - Void Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }

            Command = (int)DebitTxns.Void;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)DebitTxns.Void, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE DEBIT PROCESSOR - Void Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (ENDING) Void Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        /// <summary>
        /// Author : Gaurav
        /// Functionality Desciption : This method does a VOIDRETURN transaction on the Debit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 24-NOV-2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public string VoidReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (Start) VoidReturn() HPS_DEBITREVERSE");
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE DEBIT PROCESSOR - VoidReturn Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }

            Command = (int)DebitTxns.VoidReturn;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)DebitTxns.Void, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE DEBIT PROCESSOR - VoidReturn Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (ENDING) VoidReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (Start) Reverse()");
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE DEBIT PROCESSOR - Reverse Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            Command = (int)DebitTxns.Reverse;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)DebitTxns.Void, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE DEBIT PROCESSOR - Reverse Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE DEBIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
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
