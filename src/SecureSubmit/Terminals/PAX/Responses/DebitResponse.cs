﻿using System;
using System.IO;
using SecureSubmit.Terminals.Extensions;

namespace SecureSubmit.Terminals.PAX
{
    public class DebitResponse : PaxDeviceResponse
    {
        public string AuthorizationCode { get; set; }

        internal DebitResponse(byte[] buffer)
            : base(buffer, PAX_MSG_ID.T03_RSP_DO_DEBIT)
        {
        }

        protected override void ParseResponse(BinaryReader br)
        {
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
            } else {  //PRIMEPOS-2543 NileshJ - Add else for other response coode  [07/06/2018]
                HostResponse = new HostResponse(br);
                MapResponse();
            }
        }

        protected override void MapResponse()
        {
            base.MapResponse();

            // Host Response
            if (HostResponse != null)
            {
                AuthorizationCode = HostResponse.AuthCode;
            }

            // Account Response
            if (AccountResponse != null)
            {
                PaymentType = AccountResponse.CardType.ToString();
            }

            // Account Response
            if (AmountResponse != null)
            {
                TransactionAmount = AmountResponse.ApprovedAmount;
                AmountDue = AmountResponse.AmountDue;
            }
        }
    }
}