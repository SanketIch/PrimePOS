using MMS.PROCESSOR;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMS.EVERTEC;

namespace MMS.EVERTEC
{
    //Arvind
    public class DebitProcessor : MMS.EVERTEC.Processor, IDebitProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "EVERTEC_DEBIT";
        private const String EVERTEC_SALE = "EVERTEC_DEBIT_SALE";
        private const String EVERTEC_RETURN = "EVERTEC_DEBIT_RETURN";
        private const String EVERTEC_VOID = "EVERTEC_DEBIT_VOID";
        private const String EVERTEC_VOID_RETURN = "EVERTEC_DEBIT_VOID_RETURN"; //Unsupported
        private const String EVERTEC_AUTH = "EVERTEC_DEBIT_AUTH";
        private const String EVERTEC_REVERSE = "EVERTEC_DEBIT_REVERSE";
        private const String EVERTEC_CANCEL = "EVERTEC_CANCEL";


        //Referred as transaction type in processor
        private const String COMMAND = "COMMAND";

        // Added for Void transaction
        private const String AMOUNT = "AMOUNT";

        #endregion

        #region variables
        private string transactionType = String.Empty;

        private string AppPath = String.Empty;

        #endregion
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is constructor for the DEBITProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 12 April 2019
        /// </summary>
        /// <param name="server"></param>
        /// <param name="merchantNumber"></param>
        /// <param name="userId"></param> 
        public DebitProcessor(MerchantInfo merchant) : base(PROCESSOR, merchant)
        {
            transactionType = string.Empty;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is constructor for the DEBITProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 12 April 2019
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
        /// Start Date : 14 Jan 2019.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns>
        public String Sale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {

            logger.Trace("--EVERTEC DEBIT PROCESSOR - (Start) Sale() EVERTEC_DEBITSALE");


            transactionType = EVERTEC_SALE;
            requestMsgKeys.Add(COMMAND, EVERTEC_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC DEBIT PROCESSOR - EVERTEC_DEBITSALE Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC DEBIT PROCESSOR - EVERTEC_DEBITSALE Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC DEBIT PROCESSOR - (ENDING) EVERTEC_DEBITSALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a DEBIT transaction on the DEBIT card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2019.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Return(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--EVERTEC DEBIT PROCESSOR - (Start) Return() EVERTEC_DEBITRETURN");

            transactionType = EVERTEC_RETURN;
            requestMsgKeys.Add(COMMAND, EVERTEC_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC DEBIT PROCESSOR - EVERTEC_DEBITRETURN Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC DEBIT PROCESSOR - EVERTEC_DEBITRETURN Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC DEBIT PROCESSOR - (ENDING) EVERTEC_DEBITRETURN Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a VOIDSALE transaction on the DEBIT card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2019.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Void(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("EVERTEC DEBIT PROCESSOR - (Start) Void() EVERTEC_VOID");



            transactionType = EVERTEC_VOID;
            requestMsgKeys.Add(COMMAND, EVERTEC_VOID);
            var temptAmount = "";
            if (requestMsgKeys.TryGetValue(AMOUNT, out temptAmount))
            {
                requestMsgKeys.Remove(AMOUNT);
            }
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC DEBIT PROCESSOR - Void Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC DEBIT PROCESSOR - Void Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC DEBIT PROCESSOR - (ENDING) Void Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a VOIDRETURN transaction on the debit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 12 April 2019
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public string VoidReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("EVERTEC DEBIT PROCESSOR - (Start) VoidReturn() EVERTEC_VOIDRETURN");


            transactionType = EVERTEC_VOID_RETURN;
            requestMsgKeys.Add(COMMAND, EVERTEC_VOID_RETURN);

            var temptAmount = "";
            if (requestMsgKeys.TryGetValue(AMOUNT, out temptAmount))
            {
                requestMsgKeys.Remove(AMOUNT);
            }

            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC DEBIT PROCESSOR - VoidReturn Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC DEBIT PROCESSOR - VoidReturn Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC DEBIT PROCESSOR - (ENDING) VoidReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("EVERTEC DEBIT PROCESSOR - (Start) Reverse() EVERTEC_DEBITREVERSE");


            transactionType = EVERTEC_REVERSE;
            requestMsgKeys.Add(COMMAND, EVERTEC_REVERSE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC DEBIT PROCESSOR - Reverse Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC DEBIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC DEBIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = EVERTEC_CANCEL, ref requestMsgKeys);
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
