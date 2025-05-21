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

namespace MMS.VANTIV
{
    //Author : Arvind 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make Credit card transactions.
    //External functions:None   
    //Known Bugs : None
    public class CreditProcessor : MMS.VANTIV.Processor, ICreditProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "VANTIV_CREDIT";
        private const String VANTIV_SALE = "VANTIV_CREDIT_SALE";
        private const String VANTIV_PREREAD = "VANTIV_PREREAD"; //PRIMEPOS-3526
        private const String VANTIV_PREREAD_SALE = "VANTIV_PREREAD_SALE"; //PRIMEPOS-3526
        private const String VANTIV_PREREAD_CANCEL = "VANTIV_PREREAD_CANCEL"; //PRIMEPOS-3526
        private const String VANTIV_PREREAD_RETURN = "VANTIV_PREREAD_RETURN"; //PRIMEPOS-3522
        private const String VANTIV_RETURN = "VANTIV_CREDIT_RETURN";
        private const String VANTIV_VOID = "VANTIV_CREDIT_VOID";
        private const String VANTIV_PREAUTH = "VANTIV_CREDIT_PREAUTH";
        private const String VANTIV_POSTAUTH = "VANTIV_CREDIT_PREAUTH";
        private const String VANTIV_REVERSE = "VANTIV_CREDIT_REVERSE";
        private const String VANTIV_CANCEL = "VANTIV_CANCEL";


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
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("--VANTIV CREDIT PROCESSOR - (Start) Sale() VANTIV_SALE");
            transactionType = VANTIV_SALE;
            requestMsgKeys.Add(COMMAND, VANTIV_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("*** VANTIV CREDIT PROCESSOR - VANTIV_SALE Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - VANTIV_SALE Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) VANTIV_SALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        #region PRIMEPOS-3526
        public String PreRead(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("--VANTIV CREDIT PROCESSOR - (Start) Sale() VANTIV_PREREAD");
            transactionType = VANTIV_PREREAD;
            requestMsgKeys.Add(COMMAND, VANTIV_PREREAD);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("*** VANTIV CREDIT PROCESSOR - VANTIV_PREREAD Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - VANTIV_PREREAD Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) VANTIV_PREREAD Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        public String PreReadSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("--VANTIV CREDIT PROCESSOR - (Start) Sale() VANTIV_PREREAD_SALE");
            transactionType = VANTIV_PREREAD_SALE;
            requestMsgKeys.Add(COMMAND, VANTIV_PREREAD_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("*** VANTIV CREDIT PROCESSOR - VANTIV_PREREAD_SALE Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - VANTIV_PREREAD_SALE Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) VANTIV_PREREAD_SALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String PreReadCancel(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("--VANTIV CREDIT PROCESSOR - (Start) Sale() VANTIV_PREREAD_SALE");
            transactionType = VANTIV_PREREAD_CANCEL;
            requestMsgKeys.Add(COMMAND, VANTIV_PREREAD_CANCEL);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("*** VANTIV CREDIT PROCESSOR - VANTIV_PREREAD_SALE Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - VANTIV_PREREAD_SALE Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) VANTIV_PREREAD_SALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        //PRIMEPOS-3522
        public String PreReadReturn(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("--VANTIV CREDIT PROCESSOR - (Start) Credit() XLC_RETURN");
            transactionType = VANTIV_PREREAD_RETURN;
            requestMsgKeys.Add(COMMAND, VANTIV_PREREAD_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV CREDIT PROCESSOR - Credit Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - Credit Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        #endregion


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
            if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("VANTIV CREDIT PROCESSOR - (Start) VoidSale() VANTIV_VOID");
            transactionType = VANTIV_VOID;
            requestMsgKeys.Add(COMMAND, VANTIV_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV CREDIT PROCESSOR - VoidSale Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
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
            if (!requestMsgKeys.ContainsKey("URL") && !requestMsgKeys.ContainsKey("APPLICATIONNAME") && !requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("--VANTIV CREDIT PROCESSOR - (Start) Credit() XLC_RETURN");
            transactionType = VANTIV_RETURN;
            requestMsgKeys.Add(COMMAND, VANTIV_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV CREDIT PROCESSOR - Credit Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - Credit Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
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
            if (!requestMsgKeys.ContainsKey("URL") && requestMsgKeys.ContainsKey("APPLICATIONNAME") && requestMsgKeys.ContainsKey("STATIONID"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("URL");
                requestMsgKeys.Remove("APPLICATIONNAME");
                requestMsgKeys.Remove("STATIONID");
            }

            logger.Trace("VANTIV CREDIT PROCESSOR - (Start) VoidCredit VANTIV_VOID");
            transactionType = VANTIV_VOID;
            requestMsgKeys.Add(COMMAND, VANTIV_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
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
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = VANTIV_PREAUTH;
            requestMsgKeys.Add(COMMAND, VANTIV_PREAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.VANTIV.Processor.FAILED_OPRN;
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
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = VANTIV_POSTAUTH;
            requestMsgKeys.Add(COMMAND, VANTIV_POSTAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.VANTIV.Processor.FAILED_OPRN;
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
                base.InitDevice(requestMsgKeys["URL"], requestMsgKeys["APPLICATIONNAME"], requestMsgKeys["STATIONID"], requestMsgKeys["TRIPOSPATH"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            logger.Trace("VANTIV CREDIT PROCESSOR - (Start) Reverse() VANTIV_VOID");
            transactionType = VANTIV_REVERSE;
            requestMsgKeys.Add(COMMAND, VANTIV_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV CREDIT PROCESSOR - Reverse Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }



        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = VANTIV_CANCEL, ref requestMsgKeys);
            return pmtResp.Result;
        }

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
