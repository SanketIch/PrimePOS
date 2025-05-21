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
using System.Diagnostics;
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
    public class CreditProcessor : Processor, ICreditProcessor 
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region constants
        private const String PROCESSOR = "CREDIT";
        private const String COMMAND = "COMMAND";
        private const string MMSCARD = "MMSCARD";
        #endregion

        #region variables
        private int Command = 0;
        #endregion
        //Enumerator for Credit Transaction Codes    
        private enum CreditTxns
        {
            Sale = 1,
            Credit = 2,
            VoidSale = 3,
            PreAuthorization = 4,
            PostAuthorization = 5,
            VoidCredit = 6,
            Reverse=7
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
        public CreditProcessor(MerchantInfo merchant):base(PROCESSOR,merchant)
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
        ~CreditProcessor()
        {
            logger.Trace("CreditProcessor destructor\n");
            Command = 0;
        }
        #endregion

        #region ICreditProcessor Members
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
            //Added By Dharmendra on Mar-14-09 
            logger.Trace("--PCCHARGE CREDIT PROCESSOR - (Start) Sale()");
            string tempActualCardType = string.Empty;
            //Accessing the value of the key MMSCARD before it is eliminated in furthur processing
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType);
            //Added Till Here Mar-14-09
            
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE CREDIT PROCESSOR - Sale Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            Command = (int)CreditTxns.Sale;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));            
            pmtResp = ProcessTxn((int)CreditTxns.Sale , ref requestMsgKeys);
            //Added By Dharmendra on Mar-14-09
            //assing the value of tempActualCardType to the membar variable CardType of pmtResp.
            //since in pc charge transaction we are not getting what was the actual card type
            if (pmtResp != null )
            {
                pmtResp.CardType = tempActualCardType;
            }
            
            //Added Till Here Mar-14-09

            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE CREDIT PROCESSOR - Sale Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (ENDING) HPS_Sale Payment Response Result: " + pmtResp.Result);
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
            //Added By Dharmendra on Mar-14-09 
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (Start) VoidSale()");
            string tempActualCardType = string.Empty;
            //Accessing the value of the key MMSCARD before it is eliminated in furthur processing
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType);
            //Added Till Here Mar-14-09
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE CREDIT PROCESSOR - VoidSale Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }

            Command = (int)CreditTxns.VoidSale;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)CreditTxns.VoidSale, ref requestMsgKeys);
            //Added By Dharmendra on Mar-14-09
            //assing the value of tempActualCardType to the membar variable CardType of pmtResp.
            //since in pc charge transaction we are not getting what was the actual card type
            if (pmtResp != null)
            {
                pmtResp.CardType = tempActualCardType;
            }
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
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
            //Added By Dharmendra on Mar-14-09  
            logger.Trace("--PCCHARGE CREDIT PROCESSOR - (Start) Credit()");
            string tempActualCardType = string.Empty;
            //Accessing the value of the key MMSCARD before it is eliminated in furthur processing
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType);
            //Added Till Here Mar-14-09

            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE CREDIT PROCESSOR - Credit Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }

            Command = (int)CreditTxns.Credit;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)CreditTxns.Credit, ref requestMsgKeys);
            //Added By Dharmendra on Mar-14-09
            //assing the value of tempActualCardType to the membar variable CardType of pmtResp.
            //since in pc charge transaction we are not getting what was the actual card type
            if (pmtResp != null)
            {
                pmtResp.CardType = tempActualCardType;
            }
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE CREDIT PROCESSOR - Credit Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
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
            //Added By Dharmendra on Mar-14-09 
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (Start) VoidCredit()");
            string tempActualCardType = string.Empty;
            //Accessing the value of the key MMSCARD before it is eliminated in furthur processing
            requestMsgKeys.TryGetValue(MMSCARD, out tempActualCardType);
            //Added Till Here Mar-14-09

            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            Command = (int)CreditTxns.VoidCredit;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)CreditTxns.VoidSale, ref requestMsgKeys);
            //Added By Dharmendra on Mar-14-09
            //assing the value of tempActualCardType to the membar variable CardType of pmtResp.
            //since in pc charge transaction we are not getting what was the actual card type
            if (pmtResp != null)
            {
                pmtResp.CardType = tempActualCardType;
            }
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
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
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return Processor.INVALID_PARAMETERS;
            }
            Command = (int)CreditTxns.PreAuthorization;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)CreditTxns.PreAuthorization, ref requestMsgKeys);
            if (pmtResp == null)
                return Processor.FAILED_OPRN;
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
        public String PostAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if(!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return Processor.INVALID_PARAMETERS;
            }
            Command = (int)CreditTxns.PostAuthorization;
            requestMsgKeys.Add(COMMAND, Convert.ToString(Command));
            pmtResp = ProcessTxn((int)CreditTxns.PostAuthorization, ref requestMsgKeys);
            if (pmtResp == null)
                return Processor.FAILED_OPRN;
            return pmtResp.Result;
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method does a Reverse transaction on the credit card
        /// External functions:MMSDictioanary,PaymentResponse
        /// Known Bugs : None
        /// Start Date : 22 JULY 2010.
        /// </summary>
        /// <param name="requestMsgKeys"></param>
        /// <param name="pmtResp"></param>
        /// <returns>String TxnStatus</returns> 
        public String Reverse(ref MMSDictionary<String, String> requsetMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (Start) Reverse()");
            if (!ValidateParameters(ref requsetMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PCCHARGE CREDIT PROCESSOR - Reverse Invalid Parameters: " + Processor.INVALID_PARAMETERS);
                return Processor.INVALID_PARAMETERS;
            }
            //Command = "P2";// (int)CreditTxns.Reverse;
            requsetMsgKeys.Add(COMMAND, "P2");
            pmtResp = ProcessTxn((int)CreditTxns.Reverse, ref requsetMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***PCCHARGE CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return Processor.FAILED_OPRN;
            }
            logger.Trace("PCCHARGE CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
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
