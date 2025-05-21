//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make Credit card transactions.
//External functions:None   
//Known Bugs : None
//Start Date : 14 January 2008.

using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;
using System;
//using SecureSubmit.Terminals.PAX;//Commented by Amit on 02 Dec 2020 //PRIMEPOS-2931
//using SecureSubmit.Terminals;//Commented by Amit on 02 Dec 2020 //PRIMEPOS-2931
using System.Net.NetworkInformation;

namespace MMS.HPSPAX
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make Credit card transactions.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 14 January 2008.
    //public class CreditProcessor : MMS.HPSPAX.Processor, ICreditProcessor//PRIMEPOS-2931
    public class CreditProcessor : HPSPAX.Processor, ICreditProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();        
        #region constants
        private const String PROCESSOR = "HPSPAX_CREDIT";
        private const String PAX_SALE = "HPSPAX_CREDIT_SALE";
        private const String PAX_RETURN = "HPSPAX_CREDIT_RETURN";
        private const String PAX_VOID = "HPSPAX_CREDIT_VOID";
        private const String PAX_PREAUTH = "HPSPAX_CREDIT_PREAUTH";
        private const String PAX_POSTAUTH = "HPSPAX_CREDIT_PREAUTH";
        private const String PAX_REVERSE = "HPSPAX_CREDIT_REVERSE";
        private const String PAX_CANCEL = "HPSPAX_CANCEL";


        //Referred as transaction type in processor
        private const String COMMAND = "COMMAND";
        #endregion

        #region variables
        private string transactionType = String.Empty;

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
        public CreditProcessor(MerchantInfo merchant) : base(PROCESSOR, merchant)
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
            if (!requestMsgKeys.ContainsKey("HOSTADDR"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            logger.Trace("--PAX CREDIT PROCESSOR - (Start) Sale() PAX_SALE");
            transactionType = PAX_SALE;
            requestMsgKeys.Add(COMMAND, PAX_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - HPSPAX_SALE Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - Sale: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion         

            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);

            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX CREDIT PROCESSOR - PAX_SALE Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX CREDIT PROCESSOR - (ENDING) PAX_SALE Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        //private static void PrintResult(PingResult reply)
        //{
        //    if (reply.LastStatus == IPStatus.Success)
        //        Console.WriteLine("{0,-15}: ({1}/{2}) {3:fff}ms", reply.Address, reply.PingsSuccessfull, reply.PingsTotal, reply.AverageTime);
        //    logger.Trace(string.Format("Status:{0} \n Address: {1} \n RoundTrip time: {2} \n Time to live: {3} \n Don't fragment: {4} \n Buffer size: {5}", reply.LastStatus, reply.Address.ToString(), reply.));//, reply.Buffer.Length));

        //}
        //public static async Task ContinuousPingAsync(IPAddress address,int timeout, int delayBetweenPings,IProgress<PingResult> progress, CancellationToken cancellationToken)
        //{
        //    var ping = new System.Net.NetworkInformation.Ping();
        //    var result = new PingResult(address);
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        var res = await ping.SendPingAsync(address, timeout).ConfigureAwait(false);
        //        result.AddResult(res);
        //        progress.Report(result);
        //        await Task.Delay(delayBetweenPings).ConfigureAwait(false);
        //    }
        //}
        public void DoPinging()
        {
            Ping ping = new Ping();
            PingOptions options = new PingOptions();

            options.DontFragment = true;

            //string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //byte[] buffer = Encoding.ASCII.GetBytes(data);
            //int timeout = 12000;
            //PingReply reply = ping.Send(sIP, timeout);
            //if (reply != null)
            //    //  logger.Trace(string.Format("Status:{0} \n Address: {1} \n RoundTrip time: {2} \n Time to live: {3} \n Don't fragment: {4}", reply.Status, reply.Address.ToString(), reply.RoundtripTime, reply.Options.Ttl, reply.Options.DontFragment));//, reply.Buffer.Length));
            //    //logger.Trace(string.Format("Status:{0} \n Address: {1} \n RoundTrip time: {2}", reply.Status, reply.Address.ToString(), reply.RoundtripTime));//, reply.Buffer.Length));
            //    logger.Trace(string.Format("Status:{0} \n Address: {1} \n RoundTrip time: {2} \n Time to live: {3}", reply.Status, reply.Address != null ? reply.Address.ToString() : sIP, reply.RoundtripTime, reply.Options != null ? reply.Options.Ttl.ToString() : ""));//, reply.Options.DontFragment));

        }
        /// <summary>        Buffer size: {0}", reply.Buffer.Length
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
            if (!requestMsgKeys.ContainsKey("HOSTADDR"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            logger.Trace("PAX CREDIT PROCESSOR - (Start) VoidSale() PAX_VOID");
            transactionType = PAX_VOID;
            requestMsgKeys.Add(COMMAND, PAX_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX CREDIT PROCESSOR - VoidSale Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - VoidSale Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX CREDIT PROCESSOR - VoidSale Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX CREDIT PROCESSOR - (ENDING) VoidSale Payment Response Result: " + pmtResp.Result);
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
            if (!requestMsgKeys.ContainsKey("HOSTADDR"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            logger.Trace("--PAX CREDIT PROCESSOR - (Start) Credit() XLC_RETURN");
            transactionType = PAX_RETURN;
            requestMsgKeys.Add(COMMAND, PAX_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX CREDIT PROCESSOR - Credit Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - Credit: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX CREDIT PROCESSOR - Credit Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX CREDIT PROCESSOR - (ENDING) Credit Payment Response Result: " + pmtResp.Result);
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
            if (!requestMsgKeys.ContainsKey("HOSTADDR"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            logger.Trace("PAX CREDIT PROCESSOR - (Start) VoidCredit PAX_VOID");
            transactionType = PAX_VOID;
            requestMsgKeys.Add(COMMAND, PAX_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX CREDIT PROCESSOR - VoidCredit Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - VoidCredit Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX CREDIT PROCESSOR - VoidCredit Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX CREDIT PROCESSOR - (ENDING) VoidCredit Payment Response Result: " + pmtResp.Result);
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
            if (!requestMsgKeys.ContainsKey("HOSTADDR"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = PAX_PREAUTH;
            requestMsgKeys.Add(COMMAND, PAX_PREAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - PreAuth Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
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
            if (!requestMsgKeys.ContainsKey("HOSTADDR"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = PAX_POSTAUTH;
            requestMsgKeys.Add(COMMAND, PAX_POSTAUTH);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - PostAuth: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            return pmtResp.Result;
        }

        public String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            if (!requestMsgKeys.ContainsKey("HOSTADDR"))
            {
                pmtResp = null;
                return INVALID_PARAMETERS;
            }
            else
            {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            logger.Trace("PAX CREDIT PROCESSOR - (Start) Reverse() PAX_VOID");
            transactionType = PAX_REVERSE;
            requestMsgKeys.Add(COMMAND, PAX_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***PAX CREDIT PROCESSOR - Reverse Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX CREDIT PROCESSOR - Reverse: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX CREDIT PROCESSOR - Reverse Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("PAX CREDIT PROCESSOR - (ENDING) Reverse Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }



        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = PAX_CANCEL, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***PAX CREDIT PROCESSOR - Cancel Credit Transaction");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
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
