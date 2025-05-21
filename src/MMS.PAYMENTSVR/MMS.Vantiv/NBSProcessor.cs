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
    public class NBSProcessor : Processor, INBSProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants

        private const String PROCESSOR = "VANTIV_NBS";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String VANTIV_SALE = "VANTIV_NBS_SALE";
        private const String VANTIV_PREREAD = "VANTIV_NBS_PREREAD";
        private const String VANTIV_RETURN = "VANTIV_NBS_RETURN";
        private const String VANTIV_CANCEL = "VANTIV_CANCEL";
        private const String VANTIV_VOID = "VANTIV_NBS_VOID";  //PRIMEPOS-3373

        #endregion constants

        #region variables

        private string transactionType = String.Empty;
        private string AppPath = String.Empty;

        #endregion variables

        public NBSProcessor(MerchantInfo merchant) : base(PROCESSOR, merchant)
        {
            //Command = 0;
        }

        ~NBSProcessor()
        {
            System.Diagnostics.Debug.Print("NBS Processor destructor\n");
            // Command = 0;
        }

        #region IEbtProcessor Members

        public String NBSReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--VANTIV NBS PROCESSOR - (Start) NBSReturn() VANTIV_NBSRETURN");

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
                logger.Error("***VANTIV NBS PROCESSOR - NBSReturn Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV NBS PROCESSOR - NBSReturn Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV NBS PROCESSOR - (ENDING) NBSReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String NBSSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--VANTIV NBS PROCESSOR - (Start) NBSSale() VANTIV_NBSSALE");

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
                logger.Error("***VANTIV NBS PROCESSOR - NBSSale Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV NBS PROCESSOR - NBSSale Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV NBS PROCESSOR - (ENDING) NBSSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String NBSPreReadSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--VANTIV NBS PROCESSOR - (Start) NBSPreReadSale() VANTIV_NBSSALE");

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


            transactionType = VANTIV_PREREAD;
            requestMsgKeys.Add(COMMAND, VANTIV_PREREAD);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV NBS PROCESSOR - NBSPreReadSale Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV NBS PROCESSOR - NBSPreReadSale Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV NBS PROCESSOR - (ENDING) NBSPreReadSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            
            logger.Trace("--VANTIV NBS PROCESSOR - (Start) CancelTansaction() VANTIV_NBSSALE");

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


            //transactionType = VANTIV_PREREAD;
            //requestMsgKeys.Add(COMMAND, VANTIV_PREREAD);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV NBS PROCESSOR - CancelTansaction Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(VANTIV_CANCEL, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV NBS PROCESSOR - CancelTansaction Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV NBS PROCESSOR - (ENDING) CancelTansaction Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }


        #endregion IDebitProcessor Members

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion IDisposable Members

        #region PRIMEPOS-3373
        public String NBSVoid(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("VANTIV NBS PROCESSOR - (Start) NBSVoid() VANTIV_VOID");

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

            transactionType = VANTIV_VOID;
            requestMsgKeys.Add(COMMAND, VANTIV_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***VANTIV NBS PROCESSOR - NBSVoid Invalid Parameters: " + MMS.VANTIV.Processor.INVALID_PARAMETERS);
                return MMS.VANTIV.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***VANTIV NBS PROCESSOR - NBSVoid Payment Response is NULL");
                return MMS.VANTIV.Processor.FAILED_OPRN;
            }
            logger.Trace("VANTIV NBS PROCESSOR - (ENDING) NBSVoid Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }
        #endregion
    }
}