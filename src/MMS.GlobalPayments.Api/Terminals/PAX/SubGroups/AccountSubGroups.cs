using System;
using System.IO;
using System.Text;
using MMS.GlobalPayments.Api.Terminals.Extensions;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Terminals.PAX {
    public class AccountRequest : IRequestSubGroup {
        public string AccountNumber { get; set; }
        public string EXPD { get; set; }
        public string CvvCode { get; set; }
        public string EbtType { get; set; }
        public string VoucherNumber { get; set; }
        public string DupOverrideFlag { get; set; }

        public string GetElementString() {
            var sb = new StringBuilder();
            sb.Append(AccountNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(EXPD);
            sb.Append((char)ControlCodes.US);
            sb.Append(CvvCode);
            sb.Append((char)ControlCodes.US);
            sb.Append(EbtType);
            sb.Append((char)ControlCodes.US);
            sb.Append(VoucherNumber);
            sb.Append((char)ControlCodes.US);
            sb.Append(DupOverrideFlag);

            return sb.ToString().TrimEnd((char)ControlCodes.US);
        }
    }
//NG SDKUPDATE 20/9/2022
    public class AccountResponse : IResponseSubGroup {
        public string AccountNumber { get;  set; }
        public EntryMode EntryMode { get;  set; }
        public string ExpireDate { get;  set; }
        public string EbtType { get;  set; }
        public string VoucherNumber { get;  set; }
        public string NewAccountNumber { get;  set; }
        public TerminalCardType CardType { get;  set; }
        public string CardHolder { get;  set; }
        public string CvdApprovalCode { get;  set; }
        public string CvdMessage { get;  set; }
        public bool CardPresent { get;  set; }

        public AccountResponse(BinaryReader br) {
            var values = br.ReadToCode(ControlCodes.FS);
            if (string.IsNullOrEmpty(values))
                return;

            var data = values.Split((char)ControlCodes.US);
            try {
                this.AccountNumber = data[0];
                this.EntryMode = (EntryMode)Int32.Parse(data[1]);
                this.ExpireDate = data[2];
                EbtType = data[3];
                VoucherNumber = data[4];
                NewAccountNumber = data[5];
                if(!string.IsNullOrEmpty(data[6]))
                    CardType = (TerminalCardType)Int32.Parse(data[6]);
                CardHolder = data[7];
                CvdApprovalCode = data[8];
                CvdMessage = data[9];
                CardPresent = data[10] == "0" ? true : false;
            }
            catch (IndexOutOfRangeException exc) {
                EventLogger.Instance.Error(exc.Message);
            }
        }
    }
}
