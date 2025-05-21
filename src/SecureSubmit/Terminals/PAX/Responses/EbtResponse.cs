﻿using System;
using System.IO;
using SecureSubmit.Terminals.Extensions;

namespace SecureSubmit.Terminals.PAX {
    public class EbtResponse : PaxDeviceResponse {
        public string AuthorizationCode { get; set; }
        public string AvsResultCode { get; set; }
        public string AvsResultText { get; set; }
        // TODO: CVV Response Code
        // TODO: CVV Response Text
        // TODO: Authorized Amount
        public string CardType { get; set; }
        // TODO: Available Balance

        internal EbtResponse(byte[] buffer)
            : base(buffer, PAX_MSG_ID.T05_RSP_DO_EBT) {
        }

        protected override void ParseResponse(BinaryReader br) {
            base.ParseResponse(br);

              if (DeviceResponseCode == "000000" || DeviceResponseCode == "100011") // Added for Duplicate Transaction - 20-Dec-2018 NileshJ 
            {
                HostResponse = new HostResponse(br);
                TransactionType = br.ReadToCode(ControlCodes.FS);
                AmountResponse = new AmountResponse(br);
                AccountResponse = new AccountResponse(br);
                TraceResponse = new TraceResponse(br);
                ExtDataResponse = new ExtDataSubGroup(br);

                MapResponse();
            } else { //PRIMEPOS-2543 NileshJ - Add else for other response coode  [07/06/2018]
                HostResponse = new HostResponse(br);
                MapResponse();
            }
        }

        protected override void MapResponse() {
            base.MapResponse();

            // Host Response
            if (HostResponse != null) {
                AuthorizationCode = HostResponse.AuthCode;
                //AvsResultCode = AvsResponse.AvsResponseCode;
                //AvsResultText = AvsResponse.AvsResponseMessage;
            }

            // Account Response
            if (AccountResponse != null) {
                CardType = AccountResponse.CardType.ToString();
            }
        }
    }
}
