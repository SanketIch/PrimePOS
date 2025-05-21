﻿using System;
using System.IO;
using SecureSubmit.Entities;
using SecureSubmit.Terminals.Extensions;

namespace SecureSubmit.Terminals.PAX {
    public class CreditResponse : PaxDeviceResponse {
        public string AuthorizationCode { get; set; }
        public HpsTransaction SubTransaction { get; set; }

        internal CreditResponse(byte[] buffer)
            : base(buffer, PAX_MSG_ID.T01_RSP_DO_CREDIT) {
        }

        protected override void ParseResponse(BinaryReader br) {
            base.ParseResponse(br);
           
            if (DeviceResponseCode == "000000" || DeviceResponseCode == "100011") // Added for Duplicate Transaction - 20-Dec-2018 NileshJ
            {
                HostResponse = new HostResponse(br);
                TransactionType = ((TransactionType)Int32.Parse(br.ReadToCode(ControlCodes.FS))).ToString().Replace("_", " ");
                AmountResponse = new AmountResponse(br);
                AccountResponse = new AccountResponse(br);
                TraceResponse = new TraceResponse(br);
                AvsResponse = new AvsResponse(br);
                CommercialResponse = new CommercialResponse(br);
                EcomResponse = new EcomSubGroup(br);
                ExtDataResponse = new ExtDataSubGroup(br);

                MapResponse();
            }
            else { //PRIMEPOS-2543 NileshJ - Add else for other response coode  [07/06/2018]
                HostResponse = new HostResponse(br);
                MapResponse();
            }
        }

        protected override void MapResponse() {
            base.MapResponse();

            // Host Response
            if (HostResponse.AuthCode != null) {
                AuthorizationCode = HostResponse.AuthCode;
            }
        }
    }
}
