using System;
using System.IO;
using MMS.GlobalPayments.Api.Terminals.Extensions;

namespace MMS.GlobalPayments.Api.Terminals.PAX {
    public class CheckSubResponse : PaxTerminalResponse {
        public string CustomerId { get; set; }
        
        public CheckSubResponse(byte[] buffer)
            : base(buffer, PAX_MSG_ID.T13_RSP_DO_CHECK) {
        }

        protected override void ParseResponse(BinaryReader br) {
            base.ParseResponse(br);

            if (acceptedCodes.Contains(DeviceResponseCode)) {
                HostResponse = new HostResponse(br);
                TransactionType = ((TerminalTransactionType)Int32.Parse(br.ReadToCode(ControlCodes.FS))).ToString().Replace("_", " ");
                AmountResponse = new AmountResponse(br);
                CheckSubResponse = new CheckSubGroup(br);
                TraceResponse = new TraceResponse(br);
                ExtDataResponse = new ExtDataSubGroup(br);
            }

            MapResponse();
        }

        protected override void MapResponse() {
            base.MapResponse();

            // Host Response
            if (HostResponse != null) {
                AuthorizationCode = HostResponse.AuthCode;
            }
        }
    }
}
