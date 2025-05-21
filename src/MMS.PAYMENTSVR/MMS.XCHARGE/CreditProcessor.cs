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
using MMS.XCHARGE;
using Logger = AppLogger.AppLogger;

namespace MMS.XCHARGE
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make Credit card transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 14 January 2008.
    public class CreditProcessor : MMS.XCHARGE.Processor,ICreditProcessor 
    {
        #region constants
        private const String PROCESSOR = "XCREDIT";
        private const String COMMAND = "COMMAND";
        private const String XC_SALE ="XC_SALE";
        private const String XC_RETURN="XC_RETURN";
        private const String XC_VOID = "XC_VOID";
        private const String XC_PREAUTH="XC_PREAUTH";

        #endregion

        #region variables
        private string transactionType = String.Empty;
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        #endregion
        //Enumerator for Credit Transaction Codes    

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
        /// Changed By SRT(Gaurav ) 21-NOV-2008
        public CreditProcessor(MerchantInfo merchant):base(PROCESSOR,merchant)
        {
          //  Command = 0;
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the CreditProcessor class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /*
        ~CreditProcessor()
        {
            System.Diagnostics.Debug.Print("CreditProcessor destructor\n");
            //      Command = 0;
        }*/
        #endregion

        #region ICreditProcessor Members
        //End OF Changed By SRT(Gaurav)
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
            Logger.LogWritter("--XCHARGE CREDIT PROCESSOR - (Start) Sale() XC_SALE");
            transactionType = XC_SALE;
            requestMsgKeys.Add(COMMAND, XC_SALE);            

            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - XC_SALE Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - XC_SALE Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (ENDING) XC_SALE Payment Response Result: " + pmtResp.Result);
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
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (Start) VoidSale() XC_VOID");
            transactionType = XC_VOID;
            requestMsgKeys.Add(COMMAND, XC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - VoidSale Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
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
            Logger.LogWritter("--XCHARGE CREDIT PROCESSOR - (Start) Credit() XC_RETURN");
            transactionType = XC_RETURN;
            requestMsgKeys.Add(COMMAND, XC_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - Credit Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - Credit Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
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
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (Start) VoidCredit() XC_VOID");
            transactionType = XC_VOID;
            requestMsgKeys.Add(COMMAND, XC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
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
            transactionType = XC_PREAUTH;
            requestMsgKeys.Add(COMMAND, XC_PREAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
                return MMS.XCHARGE.Processor.FAILED_OPRN;
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
            XChargePaymentResponse tempPmtResonse = new XChargePaymentResponse();            
            pmtResp = tempPmtResonse;
            return pmtResp.Result;
            return Processor.FAILED_OPRN;
        }
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

        public String Reverse(ref MMSDictionary<String, String> requsetMsgKeys, out PaymentResponse pmtResp)
        {
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (Start) Reverse() HPS_REVERSE");
            transactionType = XC_VOID;
            requsetMsgKeys.Add(COMMAND, XC_VOID);
            if (!ValidateParameters(ref requsetMsgKeys))
            {
                pmtResp = null;
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - Reverse Invalid Parameters: " + MMS.XCHARGE.Processor.INVALID_PARAMETERS);
                return MMS.XCHARGE.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requsetMsgKeys);
            if (pmtResp == null)
            {
                Logger.LogWritter("***XCHARGE CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return MMS.XCHARGE.Processor.FAILED_OPRN;
            }
            Logger.LogWritter("XCHARGE CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
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
    }
}
