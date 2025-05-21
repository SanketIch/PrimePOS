//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make DEBIT card transactions.
//External functions:None   
//Known Bugs : None
//Start Date : 14 January 2008.

using System;
using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.Elavon
{
    //Author : Arvind 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make DEBIT card transactions.
    //External functions:None   
    //Known Bugs : None
    public class DebitProcessor : MMS.Elavon.Processor, IDebitProcessor 
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "ELAVON_DEBIT";
        private const String Elavon_SALE = "ELAVON_DEBIT_SALE";
        private const String Elavon_RETURN = "ELAVON_DEBIT_RETURN";
        private const String Elavon_VOID = "ELAVON_DEBIT_VOID";        
        private const String Elavon_AUTH = "ELAVON_DEBIT_AUTH";
        private const String Elavon_REVERSE = "ELAVON_DEBIT_REVERSE";
        private const String Elavon_CANCEL = "ELAVON_CANCEL";


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
        /// Author : Arvind
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
        /// Author : Arvind
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
        /// Author : Arvind 
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

            logger.Trace("--Elavon DEBIT PROCESSOR - (Start) Sale() Elavon_DEBITSALE");

            //if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID")) {
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //} else {
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}

            transactionType = Elavon_SALE;
            requestMsgKeys.Add(COMMAND, Elavon_SALE);            
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***Elavon DEBIT PROCESSOR - Elavon_DEBITSALE Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***Elavon DEBIT PROCESSOR - Elavon_DEBITSALE Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("Elavon DEBIT PROCESSOR - (ENDING) Elavon_DEBITSALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a DEBIT transaction on the DEBIT card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Return(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp) {
            logger.Trace("--Elavon DEBIT PROCESSOR - (Start) Return() Elavon_DEBITRETURN");


            /*
             * PRIMEPOS-2531 removed repeating Code Block of HOSTADDR Removal
             */
            //if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID")) {
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //} else {
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}

            transactionType = Elavon_RETURN;
            requestMsgKeys.Add(COMMAND, Elavon_RETURN);
            if (!ValidateParameters(ref requestMsgKeys)) {
                pmtResp = null;
                logger.Error("***Elavon DEBIT PROCESSOR - Elavon_DEBITRETURN Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null) {
                logger.Error("***Elavon DEBIT PROCESSOR - Elavon_DEBITRETURN Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("Elavon DEBIT PROCESSOR - (ENDING) Elavon_DEBITRETURN Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
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
            logger.Trace("Elavon DEBIT PROCESSOR - (Start) Void() Elavon_VOID");

            //if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID")) {
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //} else {
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}

            transactionType = Elavon_VOID;
            requestMsgKeys.Add(COMMAND, Elavon_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***Elavon DEBIT PROCESSOR - Void Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***Elavon DEBIT PROCESSOR - Void Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("Elavon DEBIT PROCESSOR - (ENDING) Void Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }

        /// <summary>
        /// Author : Arvind 
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
            logger.Trace("Elavon DEBIT PROCESSOR - (Start) VoidReturn() Elavon_VOIDRETURN");

            //if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID")) {
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //} else {
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}
            
            transactionType = Elavon_VOID;
            requestMsgKeys.Add(COMMAND, Elavon_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***Elavon DEBIT PROCESSOR - VoidReturn Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***Elavon DEBIT PROCESSOR - VoidReturn Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("Elavon DEBIT PROCESSOR - (ENDING) VoidReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("Elavon DEBIT PROCESSOR - (Start) Reverse() Elavon_DEBITREVERSE");

            //if (!requestMsgKeys.ContainsKey("URL")) {
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //} else {
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //}


            transactionType = Elavon_REVERSE;
            requestMsgKeys.Add(COMMAND, Elavon_REVERSE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***Elavon DEBIT PROCESSOR - Reverse Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***Elavon DEBIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("Elavon DEBIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp) {
            pmtResp = ProcessTxn(transactionType = Elavon_CANCEL, ref requestMsgKeys);
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
