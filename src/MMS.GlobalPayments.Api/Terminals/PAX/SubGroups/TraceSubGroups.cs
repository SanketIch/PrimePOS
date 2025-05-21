using System;
using System.IO;
using System.Text;
using MMS.GlobalPayments.Api.Terminals.Extensions;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Terminals.PAX
{ //NG SDKUPDATE 20/9/2022
    public class TraceRequest : IRequestSubGroup
    {
        public string ReferenceNumber
        {
            get; set;
        }
        public string InvoiceNumber
        {
            get; set;
        }
        public string AuthCode
        {
            get; set;
        }
        public string TransactionNumber
        {
            get; set;
        }
        public string TimeStamp
        {
            get; set;
        }
        public string ClientTransactionId
        {
            get; set;
        }

        #region PRIMEPOS-3146
        public string OrigECRRefNum
        {
            get; set;
        }
        public string OriginalPS2000
        {
            get; set;
        }
        public string OriginalAuthorizationResponse
        {
            get; set;
        }
        public string OriginalTraceNumber
        {
            get; set;
        }
        public string OriginalTransactionIdentifier
        {
            get; set;
        }
        #endregion

        public string GetElementString()
        {
            var sb = new StringBuilder();
            sb.Append(ReferenceNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(InvoiceNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(AuthCode);
            sb.Append((char)ControlCodes.US);
            sb.Append(TransactionNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(TimeStamp);
            sb.Append((char)ControlCodes.US);
            sb.Append(ClientTransactionId);
            #region PRIMEPOS-3146
            sb.Append((char)ControlCodes.US);
            sb.Append(OrigECRRefNum);
            sb.Append((char)ControlCodes.US);
            sb.Append(OriginalPS2000);
            sb.Append((char)ControlCodes.US);
            sb.Append(OriginalAuthorizationResponse);
            sb.Append((char)ControlCodes.US);
            sb.Append(OriginalTraceNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(OriginalTransactionIdentifier);
            #endregion

            return sb.ToString().TrimEnd((char)ControlCodes.US);
        }
    }

    public class TraceResponse : IResponseSubGroup
    {
        public string TransactionNumber
        {
            get; set;
        }
        public string ReferenceNumber
        {
            get; set;
        }
        public string TimeStamp
        {
            get; set;
        }

        #region PRIMEPOS-3146
        public string InvNum
        {
            get; set;
        }
        public string PS2000
        {
            get; set;
        }
        public string AuthorizationResponse
        {
            get; set;
        }
        public string ECRTransID
        {
            get; set;
        }
        public string HostTimeStamp
        {
            get; set;
        }
        #endregion

        public TraceResponse(BinaryReader br)
        {
            var values = br.ReadToCode(ControlCodes.FS);
            if (string.IsNullOrEmpty(values))
                return;

            var data = values.Split((char)ControlCodes.US);
            try
            {
                TransactionNumber = data[0];
                ReferenceNumber = data[1];
                TimeStamp = data[2];
                #region PRIMEPOS-3146
                PS2000 = data[3];
                AuthorizationResponse = data[4];
                ECRTransID = data[5];
                #endregion
            }
            catch (IndexOutOfRangeException exc)
            {
                EventLogger.Instance.Error(exc.Message);
            }
        }
    }
}
