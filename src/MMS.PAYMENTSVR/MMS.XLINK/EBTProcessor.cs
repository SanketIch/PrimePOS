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

namespace MMS.XLINK
{
    public class EBTProcessor : MMS.XLINK.Processor, IEbtProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants

        private const String PROCESSOR = "XLEBT";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String XLC_EBTSALE = "EBTSale";
        private const String XLC_EBTRETURN = "EBTReturn";
        private const String XLC_VOID = "Void";
        private const String XLC_VOIDRETURN = "VoidReturn";

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
            logger.Trace("--XLINK EBT PROCESSOR - (Start) EBTReturn() HPS_EBTRETURN");
            transactionType = XLC_EBTRETURN;
            requestMsgKeys.Add(COMMAND, XLC_EBTRETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK EBT PROCESSOR - EBTReturn Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK EBT PROCESSOR - EBTReturn Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK EBT PROCESSOR - (ENDING) EBTReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String EBTSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--XLINK EBT PROCESSOR - (Start) EBTSale() XLC_EBTSALE");
            transactionType = XLC_EBTSALE;
            requestMsgKeys.Add(COMMAND, XLC_EBTSALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK EBT PROCESSOR - EBTSale Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK EBT PROCESSOR - EBTSale Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK EBT PROCESSOR - (ENDING) EBTSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String EBTVoid(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK EBT PROCESSOR - (Start) EBTVoid XLC_VOID");
            transactionType = XLC_VOID;
            requestMsgKeys.Add(COMMAND, XLC_VOID);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK EBT PROCESSOR - EBTVoid Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK EBT PROCESSOR - EBTVoid Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK EBT PROCESSOR - (ENDING) EBTVoid  Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string EBTVoidReturn(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("XLINK EBT PROCESSOR - (Start) EBTVoidReturn XLC_VOIDRETURN");
            transactionType = XLC_VOIDRETURN;
            requestMsgKeys.Add(COMMAND, XLC_VOIDRETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***XLINK EBT PROCESSOR - EBTVoidReturn Invalid Parameters: " + MMS.XLINK.Processor.INVALID_PARAMETERS);
                return MMS.XLINK.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***XLINK EBT PROCESSOR - EBTVoidReturn Payment Response is NULL");
                return MMS.XLINK.Processor.FAILED_OPRN;
            }
            logger.Trace("XLINK EBT PROCESSOR - (ENDING) EBTVoidReturn  Payment Response Result: " + pmtResp.Result);
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