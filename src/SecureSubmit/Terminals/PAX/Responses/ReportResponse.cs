using System;
using System.IO;
using SecureSubmit.Entities;
using SecureSubmit.Terminals.Extensions;

namespace SecureSubmit.Terminals.PAX
{
    public class ReportResponse : PaxDeviceResponse
    {
        public string AuthorizationCode { get; set; }
        public HpsTransaction SubTransaction { get; set; }
      
        internal ReportResponse(byte[] buffer)
            : base(buffer, PAX_MSG_ID.R03_RSP_LOCAL_DETAIL_REPORT)
        {
        }

        protected override void ParseResponse(BinaryReader br)
        {
            base.ParseResponse(br);

            if (DeviceResponseCode == "000000")
            {
                TotalRecord = br.ReadToCode(ControlCodes.FS);// Nilesh Sajid  temp // Total Record 
                RecordNumber = br.ReadToCode(ControlCodes.FS); // Nilesh Sajid temp // Record Number

                HostResponse = new HostResponse(br);
                EDCType = br.ReadToCode(ControlCodes.FS);// Nilesh Sajid  temp // EDC Type
                TransactionType = ((TransactionType)Int32.Parse(br.ReadToCode(ControlCodes.FS))).ToString().Replace("_", " ");               
                OrignalTransactionType = br.ReadToCode(ControlCodes.FS); // Nilesh Sajid temp // Orignal Transaction Type
                AmountResponse = new AmountResponse(br);
                AccountResponse = new AccountResponse(br);
                TraceResponse = new TraceResponse(br);
                AvsResponse = new AvsResponse(br);
                CommercialResponse = new CommercialResponse(br);
                EcomResponse = new EcomSubGroup(br);
                ExtDataResponse = new ExtDataSubGroup(br);
                MapResponse();
            }
            else
            {
                HostResponse = new HostResponse(br);
                MapResponse();
            }
        }

        protected override void MapResponse()
        {
            base.MapResponse();

            // Host Response
            if (HostResponse.AuthCode != null)
            {
                AuthorizationCode = HostResponse.AuthCode;
            }
        }
    }
}
