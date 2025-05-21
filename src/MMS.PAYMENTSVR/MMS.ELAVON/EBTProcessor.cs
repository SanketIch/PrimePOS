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

namespace MMS.Elavon
{
    public class EBTProcessor : Processor, IEbtProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants

        private const String PROCESSOR = "ELAVON_EBT";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String Elavon_SALE = "ELAVON_EBT_SALE";
        private const String Elavon_RETURN = "ELAVON_EBT_RETURN";
        private const String Elavon_CANCEL = "ELAVON_CANCEL";




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
            logger.Trace("--Elavon EBT PROCESSOR - (Start) EBTReturn() Elavon_EBTRETURN");

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

            transactionType = Elavon_RETURN;
            requestMsgKeys.Add(COMMAND, Elavon_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***Elavon EBT PROCESSOR - EBTReturn Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***Elavon EBT PROCESSOR - EBTReturn Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("Elavon EBT PROCESSOR - (ENDING) EBTReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String EBTSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--Elavon EBT PROCESSOR - (Start) EBTSale() Elavon_EBTSALE");            


            transactionType = Elavon_SALE;
            requestMsgKeys.Add(COMMAND, Elavon_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***Elavon EBT PROCESSOR - EBTSale Invalid Parameters: " + MMS.Elavon.Processor.INVALID_PARAMETERS);
                return MMS.Elavon.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***Elavon EBT PROCESSOR - EBTSale Payment Response is NULL");
                return MMS.Elavon.Processor.FAILED_OPRN;
            }
            logger.Trace("Elavon EBT PROCESSOR - (ENDING) EBTSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = Elavon_CANCEL, ref requestMsgKeys);
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