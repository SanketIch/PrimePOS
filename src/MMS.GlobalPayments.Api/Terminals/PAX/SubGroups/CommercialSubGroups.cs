using System;
using System.IO;
using System.Text;
using MMS.GlobalPayments.Api.Terminals.Extensions;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Terminals.PAX {
    public class CommercialRequest : IRequestSubGroup { //NG SDKUPDATE 20/9/2022
        public string PoNumber { get; set; }
        public string CustomerCode { get; set; }
        public string TaxExempt { get; set; }
        public string TaxExemptId { get; set; }

        public string GetElementString() {
            var sb = new StringBuilder();
            sb.Append(PoNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(CustomerCode);
            sb.Append((char)ControlCodes.US);
            sb.Append(TaxExempt);
            sb.Append((char)ControlCodes.US);
            sb.Append(TaxExemptId);

            return sb.ToString().TrimEnd((char)ControlCodes.US);
        }
    }
//NG SDKUPDATE 20/9/2022
    public class CommercialResponse : IResponseSubGroup {
        public string PoNumber { get;  set; }
        public string CustomerCode { get;  set; }
        public bool TaxExempt { get;  set; }
        public string TaxExemptId { get;  set; }

        public CommercialResponse(BinaryReader br) {
            var values = br.ReadToCode(ControlCodes.FS);
            if (string.IsNullOrEmpty(values))
                return;

            var data = values.Split((char)ControlCodes.US);
            try {
                PoNumber = data[0];
                CustomerCode = data[1];
                TaxExempt = data[2] == "0" ? false : true;
                TaxExemptId = data[3];
            }
            catch (IndexOutOfRangeException exc) {
                EventLogger.Instance.Error(exc.Message);
            }
        }
    }
}
