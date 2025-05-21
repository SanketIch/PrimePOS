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
using Logger = AppLogger.AppLogger;

namespace MMS.XCHARGE
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make Credit card transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 14 January 2008.
    public class DebitProcessor : MMS.XCHARGE.Processor, IDebitProcessor 
    {
        #region constants
        private const String PROCESSOR = "XDEBIT";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String XC_DEBITSALE = "XC_DEBITSALE";
        private const String XC_DEBITRETURN = "XC_DEBITRETURN";
        private const String XC_VOID = "XC_VOID";
        //Added By SRT(Gaurav) Date: 24-NOV-2008
        private const String XC_VOIDRETURN = "XC_VOIDRETURN";
        //End Of Added By SRT(Gaurav)
        private const string XC_DEBITREVERSE = "XC_DEBITREVERSE";
        #endregion

        #region variables
        private string transactionType = String.Empty;
        #endregion

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
        public String Return(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            Logger.LogWritter("--XCHARGE DEBIT PROCESSOR - (Start) Return() XC_DEBITRETURN");
            transactionType = XC_DEBITRETURN;
            requestMsgKeys.Add(COMMAND, XC_DEBITRETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - Return Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - Return Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (ENDING) Return Payment Response Result: " + pmtResp.Result);
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
            Logger.LogWritter("--XCHARGE DEBIT PROCESSOR - (Start) Sale() XC_DEBITSALE");
            transactionType = XC_DEBITSALE;
            requestMsgKeys.Add(COMMAND, XC_DEBITSALE);            
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - XC_DEBITSALE Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - XC_DEBITSALE Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (ENDING) XC_DEBITSALE Payment Response Result: " + pmtResp.Result);
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
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (Start) Void() XC_VOID");
            transactionType = XC_VOID;
            requestMsgKeys.Add(COMMAND, XC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - Void Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - Void Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (ENDING) Void Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }

        /// <summary>
        /// Author : Gaurav 
        /// Functionality Desciption : This method does a VOIDSALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 24-NOV-2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public string VoidReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (Start) VoidReturn() XC_VOIDRETURN");
            transactionType = XC_VOIDRETURN;
            requestMsgKeys.Add(COMMAND, XC_VOIDRETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - VoidReturn Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - VoidReturn Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (ENDING) VoidReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (Start) Reverse() XC_DEBITREVERSE");
            transactionType = XC_DEBITREVERSE;
            requestMsgKeys.Add(COMMAND, XC_DEBITREVERSE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - Reverse Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE DEBIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE DEBIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
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
