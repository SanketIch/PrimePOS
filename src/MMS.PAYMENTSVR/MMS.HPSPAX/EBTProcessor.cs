//Author: Manoj Kumar Balkaran
//Functions: This is use for EBT Sales and Return.
//Known Bug: None
//Date : 08/15/2010

using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;
using System;

namespace MMS.HPSPAX
{
    public class EBTProcessor : Processor, IEbtProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants

        private const String PROCESSOR = "HPSPAX_EBT";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String PAX_SALE = "HPSPAX_EBT_SALE";
        private const String PAX_RETURN = "HPSPAX_EBT_RETURN";
        private const String PAX_CANCEL = "HPSPAX_CANCEL";




        #endregion constants

        #region variables

        private string transactionType = String.Empty;
        private string AppPath = String.Empty;
       
        #endregion variables

        public EBTProcessor(MerchantInfo merchant): base(PROCESSOR, merchant)
        {
            //Command = 0;
        }

        ~EBTProcessor()
        {
            System.Diagnostics.Debug.Print("EBT Processor destructor\n");
            // Command = 0;
        }

        #region IEbtProcessor Members

        public String EBTReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--PAX EBT PROCESSOR - (Start) EBTReturn() PAXHPS_EBTRETURN");

            if (!requestMsgKeys.ContainsKey("HOSTADDR")) {
                pmtResp = null;
                return INVALID_PARAMETERS;
            } else {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }

            transactionType = PAX_RETURN;
            requestMsgKeys.Add(COMMAND, PAX_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPSPAX EBT PROCESSOR - EBTReturn Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX EBT PROCESSOR - EBTReturn: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***HPSPAX EBT PROCESSOR - EBTReturn Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("HPSPAX EBT PROCESSOR - (ENDING) EBTReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String EBTSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--HPSPAX EBT PROCESSOR - (Start) EBTSale() PAX_EBTSALE");

            if (!requestMsgKeys.ContainsKey("HOSTADDR")) {
                pmtResp = null;
                return INVALID_PARAMETERS;
            } else {
                base.InitDevice(requestMsgKeys["HOSTADDR"]);
                requestMsgKeys.Remove("HOSTADDR");
            }


            transactionType = PAX_SALE;
            requestMsgKeys.Add(COMMAND, PAX_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***HPSPAX EBT PROCESSOR - EBTSale Invalid Parameters: " + HPSPAX.Processor.INVALID_PARAMETERS);//PRIMEPOS-2931
                return HPSPAX.Processor.INVALID_PARAMETERS;//PRIMEPOS-2931
            }
            #region PRIMEPOS-3090
            if (!isPaxDeviceConnected)
            {
                pmtResp = null;
                logger.Error("*** PAX EBT PROCESSOR - EBTSale: Signature pad is not connected. ");
                return SIGNATURE_PAD_NOT_CONNECTED;
            }
            #endregion
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***HPSPAX EBT PROCESSOR - EBTSale Payment Response is NULL");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
            logger.Trace("HPSPAX EBT PROCESSOR - (ENDING) EBTSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp) {
            pmtResp = ProcessTxn(transactionType = PAX_CANCEL, ref requestMsgKeys);
            if (pmtResp != null && pmtResp.Result == "FAILED")// Added - COndition pmtResp.Result == "FAILED" - Nilesh,Sajid PRIMEPOS-2853
            {
                logger.Error("***HPSPAX EBT PROCESSOR - Cancel EBT Transaction");
                return HPSPAX.Processor.FAILED_OPRN;//PRIMEPOS-2931
            }
                return pmtResp.Result;
        }


        #endregion IDebitProcessor Members

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion IDisposable Members
    }
}