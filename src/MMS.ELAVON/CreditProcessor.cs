//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make Credit card transactions.
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
    //Functionality Desciption : The purpose of this class is to make Credit card transactions.
    //External functions:None   
    //Known Bugs : None
    public class CreditProcessor : MMS.Elavon.Processor, ICreditProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "ELAVON_CREDIT";
        private const String ELAVON_SALE = "ELAVON_CREDIT_SALE";
        private const String ELAVON_RETURN = "ELAVON_CREDIT_RETURN";
        private const String ELAVON_VOID = "ELAVON_CREDIT_VOID";
        private const String ELAVON_VOIDRETURN = "ELAVON_CREDIT_VOIDRETURN";
        private const String ELAVON_PREAUTH = "ELAVON_CREDIT_PREAUTH";
        private const String ELAVON_POSTAUTH = "ELAVON_CREDIT_PREAUTH";
        private const String ELAVON_REVERSE = "ELAVON_CREDIT_REVERSE";
        private const String ELAVON_CANCEL = "ELAVON_CANCEL";


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
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="merchantNumber"></param>
        /// <param name="userId"></param> 
        /// Changed By SRT(Gaurav)
        /// Date : 08 NOV 2008
        /// Details : private member AppPath initialized by parameter tAppPath
        public CreditProcessor(MerchantInfo merchant) : base(PROCESSOR, merchant)
        {
            transactionType = string.Empty;
        }

        /// <summary>
        /// Author : Arvind
        /// Functionality Desciption : This method does a SALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Sale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            //if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            //{
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //}
            //else
            //{
            //    //base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}

            logger.Trace("--ELAVON CREDIT PROCESSOR - (Start) Sale() ELAVON_SALE");
            transactionType = ELAVON_SALE;
            requestMsgKeys.Add(COMMAND, ELAVON_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("*** ELAVON CREDIT PROCESSOR - ELAVON_SALE Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***ELAVON CREDIT PROCESSOR - ELAVON_SALE Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("ELAVON CREDIT PROCESSOR - (ENDING) ELAVON_SALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a VOIDSALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String VoidSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            //if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID"))
            //{
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //}
            //else
            //{
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}

            logger.Trace("ELAVON CREDIT PROCESSOR - (Start) VoidSale() ELAVON_VOID");
            transactionType = ELAVON_VOID;
            requestMsgKeys.Add(COMMAND, ELAVON_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***ELAVON CREDIT PROCESSOR - VoidSale Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***ELAVON CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("ELAVON CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind
        /// Functionality Desciption : This method does a CREDIT transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Credit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            //if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID"))
            //{
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //}
            //else
            //{
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}

            logger.Trace("--ELAVON CREDIT PROCESSOR - (Start) Credit() XLC_RETURN");
            transactionType = ELAVON_RETURN;
            requestMsgKeys.Add(COMMAND, ELAVON_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***ELAVON CREDIT PROCESSOR - Credit Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***ELAVON CREDIT PROCESSOR - Credit Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("ELAVON CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind
        /// Functionality Desciption : This method does a VOIDSALE transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String VoidCredit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            //if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            //{
            //    pmtResp = null;
            //    return INVALID_PARAMETERS;
            //}
            //else
            //{
            //    base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
            //    requestMsgKeys.Remove("URL");
            //    requestMsgKeys.Remove("APPLICATIONNAME");
            //    requestMsgKeys.Remove("STATIONID");
            //}

            logger.Trace("ELAVON CREDIT PROCESSOR - (Start) VoidCredit ELAVON_VOID");
            transactionType = ELAVON_VOIDRETURN;
            requestMsgKeys.Add(COMMAND, ELAVON_VOIDRETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***ELAVON CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***ELAVON CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("ELAVON CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a PREAUTH transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String PreAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = ELAVON_PREAUTH;
            requestMsgKeys.Add(COMMAND, ELAVON_PREAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.Elavon.Processor.FAILED_OPRN;
            return pmtResp.Result;
        }
        /// <summary>
        /// Author : Arvind 
        /// Functionality Desciption : This method does a POSTAUTH transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 


        public String PostAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = ELAVON_POSTAUTH;
            requestMsgKeys.Add(COMMAND, ELAVON_POSTAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.Elavon.Processor.FAILED_OPRN;
            return pmtResp.Result;
        }

        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            logger.Trace("ELAVON CREDIT PROCESSOR - (Start) Reverse() ELAVON_VOID");
            transactionType = ELAVON_REVERSE;
            requestMsgKeys.Add(COMMAND, ELAVON_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***ELAVON CREDIT PROCESSOR - Reverse Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***ELAVON CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("ELAVON CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }



        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = ELAVON_CANCEL, ref requestMsgKeys);
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
        /// Start Date : 14 Jan 2008.
        /// </summary>
        ~CreditProcessor()
        {
            System.Diagnostics.Debug.Print("CreditProcessor destructor\n");
            //      Command = 0;
        }
    }

}
