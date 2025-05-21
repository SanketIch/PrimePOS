using System;
using System.IO;
using SecureSubmit.Terminals.Extensions;
using SecureSubmit.Infrastructure;
using System.Text;
using System.Collections.Generic;
using NLog;
using SecureSubmit.CommonData;
namespace SecureSubmit.Terminals.PAX {
    public abstract class PaxBaseResponse : TerminalResponse {
        protected List<string> _messageIds;
        protected byte[] _buffer;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public HostResponse HostResponse { get; set; }
        public AmountResponse AmountResponse { get; set; }
        public AccountResponse AccountResponse { get; set; }
        public TraceResponse TraceResponse { get; set; }
        public AvsResponse AvsResponse { get; set; }
        public CommercialResponse CommercialResponse { get; set; }
        public EcomSubGroup EcomResponse { get; set; }
        public ExtDataSubGroup ExtDataResponse { get; set; }
        public CheckSubGroup CheckSubResponse { get; set; }

        internal PaxBaseResponse(byte[] buffer, params string[] messageIds) {
            this._messageIds = new List<string>();
            this._messageIds.AddRange(messageIds);

            this._buffer = buffer;

            using (var br = new BinaryReader(new MemoryStream(buffer))) {
                this.ParseResponse(br);
            }
        }

        protected virtual void ParseResponse(BinaryReader br) {
            logger.Debug("ParseResponse() :  " + clsPOSDBConstants.Log_Entering);
            var code = (ControlCodes)br.ReadByte(); // STX
            this.Status = br.ReadToCode(ControlCodes.FS);
            this.Command = br.ReadToCode(ControlCodes.FS);
            this.Version = br.ReadToCode(ControlCodes.FS);
            this.DeviceResponseCode = br.ReadToCode(ControlCodes.FS);
            this.DeviceResponseText = br.ReadToCode(ControlCodes.FS);
            
            if (_messageIds.Contains(Command)) {
                logger.Debug("Command :  " + Command);
                switch (Command) {
                    case "A09":
                        this.TotalLength = br.ReadToCode(ControlCodes.FS); // GetSignature
                        this.ResponseLength = br.ReadToCode(ControlCodes.FS); // GetSignature
                        this.SignatureData = br.ReadToCode(ControlCodes.FS); // GetSignature
                        break;
                    case "A57":
                        this.ButtonNumber = br.ReadToCode(ControlCodes.FS); // ShowTextbox click
                        break;
                    case "A07":
                        this.ButtonNumber = br.ReadToCode(ControlCodes.FS); // ShowTextbox click
                        break;
                    case "A69": //PRIMEPOS - 2555 Added for ShowDialogForm Response - NILESHJ
                        this.ButtonNumber = br.ReadToCode(ControlCodes.FS); // ShowTextbox click
                        break;
                }
            }

            if (!_messageIds.Contains(Command))
                throw new HpsMessageException(string.Format("Unexpected message type received. {1}.", this.Command));
        }

        public override string ToString() {
            var sb = new StringBuilder();
            foreach (byte b in _buffer) {
                if (Enum.IsDefined(typeof(ControlCodes), b)) {
                    var code = (ControlCodes)b;
                    sb.Append(string.Format("[{0}]", code.ToString()));
                }
                else sb.Append((char)b);
            }

            return sb.ToString();
        }
    }

    public class PaxDeviceResponse : PaxBaseResponse {
        public string ReferenceNumber { get; set; }

        public PaxDeviceResponse(byte[] buffer, params string[] messageIds)
            : base(buffer, messageIds) {
        }

        protected virtual void MapResponse() {
            // Host Data
            if (HostResponse != null) {
                ResponseCode = HostResponse.HostResponseCode;
                ResponseText = HostResponse.HostResponseMessage;
                ApprovalCode = HostResponse.HostResponseCode;
            }

            // Amount Response
            if (AmountResponse != null) {
                TransactionAmount = AmountResponse.ApprovedAmount;
                AmountDue = AmountResponse.AmountDue;
                TipAmount = AmountResponse.TipAmount;
                CashBackAmount = AmountResponse.CashBackAmount;
            }

            // Account Response
            if (AccountResponse != null) {
                MaskedCardNumber = AccountResponse.AccountNumber.PadLeft(16, '*');
                EntryMethod = AccountResponse.EntryMode.ToString();
                ExpirationDate = AccountResponse.ExpireDate;
                PaymentType = AccountResponse.CardType.ToString().Replace("_", " ");
                CardHolderName = AccountResponse.CardHolder;
                CvvResponseCode = AccountResponse.CvdApprovalCode;
                CvvResponseText = AccountResponse.CvdMessage;
                CardPresent = AccountResponse.CardPresent;
            }

            // Trace Data
            if (TraceResponse != null) {
                TerminalRefNumber = TraceResponse.TransactionNumber;
                ReferenceNumber = TraceResponse.ReferenceNumber;
            }

            // AVS
            if (AvsResponse != null) {
                AvsResponseCode = AvsResponse.AvsResponseCode;
                AvsResponseText = AvsResponse.AvsResponseMessage;
            }

            // Commercial Info
            if (CommercialResponse != null) {
                TaxExempt = CommercialResponse.TaxExempt;
                TaxExemptId = CommercialResponse.TaxExemptId;
            }

            // Ext Data
            if (ExtDataResponse != null) {
                TransactionId = ExtDataResponse[EXT_DATA.HOST_REFERENCE_NUMBER].ToInt32();
                Token = ExtDataResponse[EXT_DATA.TOKEN];
                CardBIN = ExtDataResponse[EXT_DATA.CARD_BIN];
                SignatureStatus = ExtDataResponse[EXT_DATA.SIGNATURE_STATUS];

                // EMV Stuff
                ApplicationPreferredName = ExtDataResponse[EXT_DATA.APPLICATION_PREFERRED_NAME];
                ApplicationLabel = ExtDataResponse[EXT_DATA.APPLICATION_LABEL];
                ApplicationId = ExtDataResponse[EXT_DATA.APPLICATION_ID];
                ApplicationCryptogramType = ApplicationCryptogramType.TC;
                ApplicationCryptogram = ExtDataResponse[EXT_DATA.TRANSACTION_CERTIFICATE];
                CardHolderVerificationMethod = ExtDataResponse[EXT_DATA.CUSTOMER_VERIFICATION_METHOD];
                TerminalVerificationResults = ExtDataResponse[EXT_DATA.TERMINAL_VERIFICATION_RESULTS];
            }
        }
    }
}
