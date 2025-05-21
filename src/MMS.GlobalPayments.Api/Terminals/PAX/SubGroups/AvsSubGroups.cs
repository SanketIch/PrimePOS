using System;
using System.IO;
using System.Text;
using MMS.GlobalPayments.Api.Terminals.Extensions;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Terminals.PAX {
    public class AvsRequest : IRequestSubGroup { //NG SDKUPDATE 20/9/2022
        public string ZipCode { get; set; }
        public string Address { get; set; }

        public string GetElementString() {
            var sb = new StringBuilder();
            sb.Append(ZipCode);
            sb.Append((char)ControlCodes.US);
            sb.Append(Address);

            return sb.ToString().TrimEnd((char)ControlCodes.US);
        }
    }

    public class AvsResponse : IResponseSubGroup {
        public string AvsResponseCode { get; private set; }
        public string AvsResponseMessage { get; private set; }

        public AvsResponse(BinaryReader br) {
            var values = br.ReadToCode(ControlCodes.FS);
            if (string.IsNullOrEmpty(values))
                return;

            var data = values.Split((char)ControlCodes.US);
            try {
                AvsResponseCode = data[0];
                AvsResponseMessage = data[1];
            }
            catch (IndexOutOfRangeException exc) {
                EventLogger.Instance.Error(exc.Message);
            }
        }
    }
}
