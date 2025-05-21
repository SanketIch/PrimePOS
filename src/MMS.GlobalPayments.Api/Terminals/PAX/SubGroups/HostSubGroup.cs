using System;
using System.IO;
using MMS.GlobalPayments.Api.Terminals.Extensions;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Terminals.PAX
{
    public class HostResponse : IResponseSubGroup
    {
        public string HostResponseCode
        {
            get; set;
        }
        public string HostResponseMessage
        {
            get; set;
        }
        public string AuthCode
        {
            get; set;
        }
        public string HostRefereceNumber
        {
            get; set;
        }
        public string TraceNumber
        {
            get; set;
        }
        public string BatchNumber
        {
            get; set;
        }
        #region PRIMEPOS-3146
        public string TransactionIdentifier
        {
            get; set;
        }
        public string GatewayTransactionID
        {
            get; set;
        }
        public string HostDetailedMessage
        {
            get; set;
        }
        public string TransactionIntegrityClass
        {
            get; set;
        }
        public string RetrievalReferenceNumber
        {
            get; set;
        }
        #endregion
        public HostResponse(BinaryReader br)
        {
            var values = br.ReadToCode(ControlCodes.FS);
            if (string.IsNullOrEmpty(values))
                return;

            var data = values.Split((char)ControlCodes.US);
            try
            {
                HostResponseCode = data[0];
                HostResponseMessage = data[1];
                AuthCode = data[2];
                HostRefereceNumber = data[3];
                TraceNumber = data[4];
                BatchNumber = data[5];
                #region PRIMEPOS-3146
                TransactionIdentifier = data[6];
                GatewayTransactionID = data[7];
                HostDetailedMessage = data[8];
                #endregion
            }
            catch (IndexOutOfRangeException exc)
            {
                EventLogger.Instance.Error(exc.Message);
            }
        }
    }
}
