using System;
using System.IO;
using System.Text;
using MMS.GlobalPayments.Api.Terminals.Extensions;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Terminals.PAX { //NG SDKUPDATE 20/9/2022
    public class EcomSubGroup : IRequestSubGroup, IResponseSubGroup {
        public string EcomMode { get; set; }
        public string TransactionType { get; set; }
        public string SecureType { get; set; }
        public string OrderNumber { get; set; }
        public int? Installments { get; set; }
        public int? CurrentInstallment { get; set; }

        public EcomSubGroup() { }
        public EcomSubGroup(BinaryReader br) {
            var values = br.ReadToCode(ControlCodes.FS);
            if (string.IsNullOrEmpty(values))
                return;

            var data = values.Split((char)ControlCodes.US);
            try {
                EcomMode = data[0];
                TransactionType = data[1];
                SecureType = data[2];
                OrderNumber = data[3];
                Installments = data[4].ToInt32();
                CurrentInstallment = data[5].ToInt32();
            }
            catch (IndexOutOfRangeException exc) {
                EventLogger.Instance.Error(exc.Message);
            }
        }

        public string GetElementString() {
            var sb = new StringBuilder();
            sb.Append(EcomMode);
            sb.Append((char)ControlCodes.US);
            sb.Append(TransactionType);
            sb.Append((char)ControlCodes.US);
            sb.Append(SecureType);
            sb.Append((char)ControlCodes.US);
            sb.Append(OrderNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(Installments.HasValue ? Installments.Value.ToString() : string.Empty);
            sb.Append((char)ControlCodes.US);
            sb.Append(CurrentInstallment.HasValue ? CurrentInstallment.Value.ToString() : string.Empty);

            return sb.ToString().TrimEnd((char)ControlCodes.US);
        }
    }
}
