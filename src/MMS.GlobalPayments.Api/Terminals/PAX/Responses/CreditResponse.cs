using System;
using System.IO;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.Extensions;

namespace MMS.GlobalPayments.Api.Terminals.PAX {
    public class CreditResponse : PaxTerminalResponse {
        public Transaction SubTransaction { get; set; }

        public CreditResponse(byte[] buffer)
            : base(buffer, PAX_MSG_ID.T01_RSP_DO_CREDIT) {
        }

        protected override void ParseResponse(BinaryReader br) {
            base.ParseResponse(br);

            if (acceptedCodes.Contains(DeviceResponseCode)) {
                HostResponse = new HostResponse(br);
                TransactionType = ((TerminalTransactionType)int.Parse(br.ReadToCode(ControlCodes.FS))).ToString().Replace("_", " ");
                AmountResponse = new AmountResponse(br);
                AccountResponse = new AccountResponse(br);
                TraceResponse = new TraceResponse(br);
                AvsResponse = new AvsResponse(br);
                CommercialResponse = new CommercialResponse(br);
                EcomResponse = new EcomSubGroup(br);
                ExtDataResponse = new ExtDataSubGroup(br);

                MapResponse();
            }
        }
    }
}
