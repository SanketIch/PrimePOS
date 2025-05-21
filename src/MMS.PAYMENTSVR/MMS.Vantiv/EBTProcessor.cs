//Author: Manoj Kumar Balkaran
//Functions: This is use for EBT Sales and Return.
//Known Bug: None
//Date : 08/15/2010

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MMS.PROCESSOR;
//using Logger = AppLogger.AppLogger;
using NLog;

namespace MMS.VANTIV
{
    public class EBTProcessor : Processor, IEbtProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants

        private const String PROCESSOR = "VANTIV_EBT";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String VANTIV_SALE = "VANTIV_EBT_SALE";
        private const String VANTIV_RETURN = "VANTIV_EBT_RETURN";
        private const String VANTIV_CANCEL = "VANTIV_CANCEL";




        #endregion constants

        #region variables

        private string transactionType = String.Empty;
        private string AppPath = String.Empty;

        #endregion variables

        public EBTProcessor(MerchantInfo merchant) : base(PROCESSOR, merchant)
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
            logger.Trace("--VANTIV EBT PROCESSOR - (Start) EBTReturn() VANTIV_EBTRETURN");

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

            transactionType = VANTIV_RETURN;
            requestMsgKeys.Add(COMMAND, VANTIV_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV EBT PROCESSOR - EBTReturn Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV EBT PROCESSOR - EBTReturn Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV EBT PROCESSOR - (ENDING) EBTReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String EBTSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--VANTIV EBT PROCESSOR - (Start) EBTSale() VANTIV_EBTSALE");

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


            transactionType = VANTIV_SALE;
            requestMsgKeys.Add(COMMAND, VANTIV_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV EBT PROCESSOR - EBTSale Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV EBT PROCESSOR - EBTSale Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV EBT PROCESSOR - (ENDING) EBTSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = VANTIV_CANCEL, ref requestMsgKeys);
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