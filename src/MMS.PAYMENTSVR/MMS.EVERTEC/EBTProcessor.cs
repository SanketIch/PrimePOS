using MMS.PROCESSOR;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS.EVERTEC
{
    //Arvind
    public class EBTProcessor : Processor,IEbtProcessor
    {

        ILogger logger = LogManager.GetCurrentClassLogger();
        #region constants

        private const String PROCESSOR = "EVERTEC_EBT";
        private const String COMMAND = "COMMAND";
        private const String TRANSACTIONTYPE = "TRANSACTIONTYPE";
        private const String EVERTEC_SALE = "EVERTEC_EBT_SALE";
        private const String EVERTEC_RETURN = "EVERTEC_EBT_RETURN";
        private const String EVERTEC_CANCEL = "EVERTEC_CANCEL";




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
            logger.Trace("--EVERTEC EBT PROCESSOR - (Start) EBTReturn() EVERTEC_EBTRETURN");            

            transactionType = EVERTEC_RETURN;
            requestMsgKeys.Add(COMMAND, EVERTEC_RETURN);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC EBT PROCESSOR - EBTReturn Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC EBT PROCESSOR - EBTReturn Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC EBT PROCESSOR - (ENDING) EBTReturn Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public String EBTSale(ref MMSDictionary<string, string> requestMsgKeys, out PaymentResponse pmtResp)
        {
            logger.Trace("--EVERTEC EBT PROCESSOR - (Start) EBTSale() EVERTEC_EBTSALE");

            transactionType = EVERTEC_SALE;
            requestMsgKeys.Add(COMMAND, EVERTEC_SALE);
            if (!ValidateParameters(ref requestMsgKeys))
            {
                pmtResp = null;
                logger.Error("***EVERTEC EBT PROCESSOR - EBTSale Invalid Parameters: " + MMS.EVERTEC.Processor.INVALID_PARAMETERS);
                return MMS.EVERTEC.Processor.INVALID_PARAMETERS;
            }
            pmtResp = ProcessTxn(transactionType, ref requestMsgKeys);
            if (pmtResp == null)
            {
                logger.Error("***EVERTEC EBT PROCESSOR - EBTSale Payment Response is NULL");
                return MMS.EVERTEC.Processor.FAILED_OPRN;
            }
            logger.Trace("EVERTEC EBT PROCESSOR - (ENDING) EBTSale Payment Response Result: " + pmtResp.Result);
            return pmtResp.Result;
        }

        public string CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp)
        {
            pmtResp = ProcessTxn(transactionType = EVERTEC_CANCEL, ref requestMsgKeys);
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
