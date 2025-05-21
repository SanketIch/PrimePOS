using MMS.PROCESSOR;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MMS.EVERTEC
{
    //Arvind
    public class CreditProcessor : MMS.EVERTEC.Processor, ICreditProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "EVERTEC_CREDIT";
        private const String EVERTEC_SALE = "EVERTEC_CREDIT_SALE";
        private const String EVERTEC_RETURN = "EVERTEC_CREDIT_RETURN";
        private const String EVERTEC_VOID = "EVERTEC_CREDIT_VOID";
        private const String EVERTEC_PREAUTH = "EVERTEC_CREDIT_PREAUTH";
        private const String EVERTEC_POSTAUTH = "EVERTEC_CREDIT_PREAUTH";
        private const String EVERTEC_REVERSE = "EVERTEC_CREDIT_REVERSE";
        private const String EVERTEC_CANCEL = "EVERTEC_CANCEL";


        //Referred as transaction type in processor
        private const String COMMAND = "COMMAND";
        #endregion

        #region variables
        private string transactionType = String.Empty;

        #endregion

        #region ICreditProcessor Members
        //Enumerator for Credit Transaction Codes    
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        /// <param name="server"></param>
        /// <param name="merchantNumber"></param>
        /// <param name="userId"></param> 
        public CreditProcessor(MerchantInfo merchant) : base(PROCESSOR, merchant)
        {
            transactionType = string.Empty;
        }

        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a SALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Sale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {


            logger.Trace("--EVERTEC CREDIT PROCESSOR - (Start) Sale() EVERTEC_SALE");
            transactionType = EVERTEC_SALE;
            requestMsgKeys.Add(COMMAND, EVERTEC_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("*** EVERTEC CREDIT PROCESSOR - EVERTEC_SALE Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC CREDIT PROCESSOR - EVERTEC_SALE Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC CREDIT PROCESSOR - (ENDING) EVERTEC_SALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a VOIDSALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String VoidSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("EVERTEC CREDIT PROCESSOR - (Start) VoidSale() EVERTEC_VOID");
            transactionType = EVERTEC_VOID;
            requestMsgKeys.Add(COMMAND, EVERTEC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC CREDIT PROCESSOR - VoidSale Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a CREDIT transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Credit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--EVERTEC CREDIT PROCESSOR - (Start) Credit() XLC_RETURN");
            transactionType = EVERTEC_RETURN;
            requestMsgKeys.Add(COMMAND, EVERTEC_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC CREDIT PROCESSOR - Credit Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC CREDIT PROCESSOR - Credit Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a VOIDSALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String VoidCredit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {            
            logger.Trace("EVERTEC CREDIT PROCESSOR - (Start) VoidCredit EVERTEC_VOID");
            transactionType = EVERTEC_VOID;
            requestMsgKeys.Add(COMMAND, EVERTEC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a PREAUTH transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String PreAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {            

            transactionType = EVERTEC_PREAUTH;
            requestMsgKeys.Add(COMMAND, EVERTEC_PREAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a POSTAUTH transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 


        public String PostAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            

            transactionType = EVERTEC_POSTAUTH;
            requestMsgKeys.Add(COMMAND, EVERTEC_POSTAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            return pmtResp.Result;
        }

        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            

            logger.Trace("EVERTEC CREDIT PROCESSOR - (Start) Reverse() EVERTEC_VOID");
            transactionType = EVERTEC_REVERSE;
            requestMsgKeys.Add(COMMAND, EVERTEC_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC CREDIT PROCESSOR - Reverse Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }



        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = EVERTEC_CANCEL, ref requestMsgKeys);
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

        #endregion

        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 10 April 2019
        /// </summary>
        ~CreditProcessor()
        {
            System.Diagnostics.Debug.Print("CreditProcessor destructor\n");
            //      Command = 0;
        }
    }
}
