using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Utils;
using System;

namespace MMS.GlobalPayments.Api.Terminals.HPA.Responses {
    public class SignatureResponse : SipTerminalResponse, ISignatureResponse {
        public SignatureResponse(byte[] response, params string[] messageIds) : base(response, messageIds) { }

        internal override void MapResponse(Element response) {
            base.MapResponse(response);

            if (DeviceResponseCode == "00") {
                var attachmentData = response.GetValue<string>("AttachmentData");
                if(!string.IsNullOrEmpty(attachmentData))
                    SignatureData = Convert.FromBase64String(attachmentData);
            }
        }
    }
}
