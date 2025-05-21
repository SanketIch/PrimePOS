//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make DEBIT card transactions.
//External functions:None   
//Known Bugs : None
//Start Date : 14 January 2008.

using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;
using System;

namespace MMS.HPSPAX
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make DEBIT card transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 14 January 2008.
    public class DebitProcessor : HPSPAX.Processor, IDebitProcessor //PRIMEPOS-2931
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "HPSPAX_DEBIT";
        private const String PAX_SALE = "HPSPAX_DEBIT_SALE";
        private const String PAX_RETURN = "HPSPAX_DEBIT_RETURN";
        private const String PAX_VOID = "HPSPAX_DEBIT_VOID";
        private const String PAX_VOID_RETURN = "HPSPAX_DEBIT_VOID_RETURN"; //Unsupported
        private const String PAX_AUTH = "HPSPAX_DEBIT_AUTH";
        private const String PAX_REVERSE = "HPSPAX_DEBIT_REVERSE";
        private const String PAX_CANCEL = "HPSPAX_CANCEL";


        //Referred as transaction type in processor
        private const String COMMAND = "COMMAND";

        // Added for Void transaction
        private const String AMOUNT = "AMOUNT";

        #endregion

        #region variables
        private string transactionType = String.Empty;
        //Added By SRT(Gaurav) Date : 8 NOV 2008
        private string AppPath = String.Empty;
        //End Of Added By SRT(Gaurav)
        #endregion
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the DEBITProcessor class
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
            transactionType = string.Empty;
        }  
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the DEBITProcessor class
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
        /// Functionality Desciption :  This method does a SALE transaction on the DEBIT card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public String Sale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {

            logger.Trace("--PAX DEBIT PROCESSOR - (Start) Sale() PAX_DEBITSALE");

            if (!requestMsgKeys.ContainsKey("HOSTADDR")) {
                pmtResp = null;
                return INVALID_PARAMETERS;
            } else {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = PAX_SALE;
            requestMsgKeys.Add(COMMAND, PAX_SALE);            
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX DEBIT PROCESSOR - PAX_DEBITSALE Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX DEBIT PROCESSOR - PAX_DEBITSALE: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX DEBIT PROCESSOR - PAX_DEBITSALE Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX DEBIT PROCESSOR - (ENDING) PAX_DEBITSALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method does a DEBIT transaction on the DEBIT card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Return(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp) {
            logger.Trace("--PAX DEBIT PROCESSOR - (Start) Return() PAX_DEBITRETURN");


            /*
             * PRIMEPOS-2531 removed repeating Code Block of HOSTADDR Removal
             */
            if (!requestMsgKeys.ContainsKey("HOSTADDR")) {
                pmtResp = null;
                return INVALID_PARAMETERS;
            } else {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = PAX_RETURN;
            requestMsgKeys.Add(COMMAND, PAX_RETURN);
            if (!ValidateParameters(ref requestMsgKeys)) {
                pmtResp = null;
                logger.Error("***PAX DEBIT PROCESSOR - PAX_DEBITRETURN Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX DEBIT PROCESSOR - PAX_DEBITRETURN: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX DEBIT PROCESSOR - PAX_DEBITRETURN Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX DEBIT PROCESSOR - (ENDING) PAX_DEBITRETURN Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method does a VOIDSALE transaction on the DEBIT card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Void(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("PAX DEBIT PROCESSOR - (Start) Void() PAX_VOID");

            if (!requestMsgKeys.ContainsKey("HOSTADDR")) {
                pmtResp = null;
                return INVALID_PARAMETERS;
            } else {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = PAX_VOID;
            requestMsgKeys.Add(COMMAND, PAX_VOID);
            var temptAmount = "";
            if (requestMsgKeys.TryGetValue(AMOUNT,out temptAmount)) {
                requestMsgKeys.Remove(AMOUNT);
            }
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX DEBIT PROCESSOR - Void Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX DEBIT PROCESSOR - Void: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX DEBIT PROCESSOR - Void Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX DEBIT PROCESSOR - (ENDING) Void Payment Response Result: " + pmtResp.Result);
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
            logger.Trace("PAX DEBIT PROCESSOR - (Start) VoidReturn() PAX_VOIDRETURN");

            if (!requestMsgKeys.ContainsKey("HOSTADDR")) {
                pmtResp = null;
                return INVALID_PARAMETERS;
            } else {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }


            transactionType = PAX_VOID_RETURN;
            requestMsgKeys.Add(COMMAND, PAX_VOID_RETURN);

            var temptAmount = "";
            if (requestMsgKeys.TryGetValue(AMOUNT, out temptAmount)) {
                requestMsgKeys.Remove(AMOUNT);
            }

            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX DEBIT PROCESSOR - VoidReturn Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX DEBIT PROCESSOR - VoidReturn: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX DEBIT PROCESSOR - VoidReturn Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX DEBIT PROCESSOR - (ENDING) VoidReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("PAX DEBIT PROCESSOR - (Start) Reverse() PAX_DEBITREVERSE");

            if (!requestMsgKeys.ContainsKey("HOSTADDR")) {
                pmtResp = null;
                return INVALID_PARAMETERS;
            } else {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }


            transactionType = PAX_REVERSE;
            requestMsgKeys.Add(COMMAND, PAX_REVERSE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX DEBIT PROCESSOR - Reverse Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX DEBIT PROCESSOR - Reverse: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX DEBIT PROCESSOR - Reverse Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX DEBIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp) {
            pmtResp = ProcessTxn(transactionType = PAX_CANCEL, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX DEBIT PROCESSOR - Cancel Debit Transaction");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
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
