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
using System.ComponentModel;
using MMS.XLINK;
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
    public class CreditProcessor : MMS.XLINK.Processor, ICreditProcessor 
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants
        private const String PROCESSOR = "XLCREDIT";
        private const String COMMAND = "COMMAND";
        private const String XLC_SALE ="Purchase";
        private const String XLC_RETURN="Return";
        private const String XLC_VOID = "Void";
        private const String XLC_PREAUTH="PreAuth";

        #endregion

        #region variables
        private string transactionType = String.Empty;
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        //Added By SRT(Gaurav) Date : 8 NOV 2008
        private string AppPath = String.Empty;
        //End Of Added By SRT(Gaurav)
        #endregion

        #region ICreditProcessor Members
        //Enumerator for Credit Transaction Codes    
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
        public CreditProcessor(MerchantInfo merchant):base(PROCESSOR,merchant)
        {
            transactionType = string.Empty;
        }

        /// <summary>
        /// Author : Ritesh 
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
            logger.Trace("--XLINK CREDIT PROCESSOR - (Start) Sale() XLC_SALE");
            transactionType = XLC_SALE;
            requestMsgKeys.Add(COMMAND, XLC_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK CREDIT PROCESSOR - XLC_SALE Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK CREDIT PROCESSOR - XLC_SALE Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK CREDIT PROCESSOR - (ENDING) XLC_SALE Payment Response Result: " + pmtResp.Result);
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
        public String VoidSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK CREDIT PROCESSOR - (Start) VoidSale() XLC_VOID");
            transactionType = XLC_VOID;
            requestMsgKeys.Add(COMMAND, XLC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK CREDIT PROCESSOR - VoidSale Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result; 
        }
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
        public String Credit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--XLINK CREDIT PROCESSOR - (Start) Credit() XLC_RETURN");
            transactionType = XLC_RETURN;
            requestMsgKeys.Add(COMMAND, XLC_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK CREDIT PROCESSOR - Credit Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK CREDIT PROCESSOR - Credit Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
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
        public String VoidCredit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK CREDIT PROCESSOR - (Start) VoidCredit XLC_VOID");
            transactionType = XLC_VOID;
            requestMsgKeys.Add(COMMAND, XLC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;  
        }
        /// <summary>
        /// Author : Ritesh 
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
            transactionType = XLC_PREAUTH;
            requestMsgKeys.Add(COMMAND, XLC_PREAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.XLINK.Processor.FAILED_OPRN;
            return pmtResp.Result;  
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method does a POSTAUTH transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        
        // Note : PostAuth method is not applicable in x-charge
        public String PostAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            //Need to check this whether there is no PostAuth as Force should mean the same.
            PaymentResponse tempPmtResonse = new XLinkPaymentResponse();            
            pmtResp = tempPmtResonse;
            return Processor.FAILED_OPRN;
        }

        public String Reverse(ref MMSDictionary<String, String> requsetMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK CREDIT PROCESSOR - (Start) Reverse() XLC_VOID");
            transactionType = XLC_VOID;
            requsetMsgKeys.Add(COMMAND, XLC_VOID);
            if (!ValidateParameters(ref requsetMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK CREDIT PROCESSOR - Reverse Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requsetMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
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
        /// Author : Ritesh 
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
