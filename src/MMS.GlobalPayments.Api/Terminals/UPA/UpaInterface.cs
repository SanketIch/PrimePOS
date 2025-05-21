using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Terminals.Builders;
using MMS.GlobalPayments.Api.Utils;
using MMS.GlobalPayments.Api.Terminals.PAX;
using System.Text;

namespace MMS.GlobalPayments.Api.Terminals.UPA {
    public class UpaInterface : DeviceInterface<UpaController>, IDeviceInterface {
        internal UpaInterface(UpaController controller) : base(controller) { }

        public override TerminalAuthBuilder TipAdjust(decimal? amount) {
            return new TerminalAuthBuilder(TransactionType.Edit, PaymentMethodType.Credit)
                .WithGratuity(amount);
        }

        public override TerminalAuthBuilder Tokenize() {
            return new TerminalAuthBuilder(TransactionType.Tokenize, PaymentMethodType.Credit);
        }

        public override TerminalAuthBuilder AuthCompletion() {
            return new TerminalAuthBuilder(TransactionType.Auth, PaymentMethodType.Credit);
        }

        public override TerminalManageBuilder DeletePreAuth()
        {
            return new TerminalManageBuilder(TransactionType.Delete, PaymentMethodType.Credit)
                .WithTransactionModifier(TransactionModifier.DeletePreAuth);
        }

        public override IEODResponse EndOfDay() {
            var requestId = _controller.GetRequestId();
            var response = _controller.Send(TerminalUtilities.BuildUpaAdminRequest(requestId, EcrId, UpaTransType.EodProcessing));
            string jsonObject = Encoding.UTF8.GetString(response);
            var jsonParse = JsonDoc.Parse(jsonObject);
            return new UpaEODResponse(jsonParse);
        }

        public override PaxTerminalResponse Reboot() {
            var requestId = _controller.GetRequestId();
            var response = _controller.Send(TerminalUtilities.BuildUpaAdminRequest(requestId, EcrId, UpaTransType.Reboot));
            string jsonObject = Encoding.UTF8.GetString(response);
            var jsonParse = JsonDoc.Parse(jsonObject);
            return null;  //NG SDKUPDATE 20/9/2022
           // return new TransactionResponse(jsonParse);
        }

        public override IDeviceResponse LineItem(string leftText, string rightText = null, string runningLeftText = null, string runningRightText = null) {
            var requestId = _controller.GetRequestId();
            var response = _controller.Send(TerminalUtilities.BuildUpaAdminRequest(requestId, EcrId, UpaTransType.LineItemDisplay, leftText, rightText));
            string jsonObject = Encoding.UTF8.GetString(response);
            var jsonParse = JsonDoc.Parse(jsonObject);
            return new TransactionResponse(jsonParse);
        }
        
        public override PaxTerminalResponse Cancel() {
            var requestId = _controller.GetRequestId();
            var response = _controller.Send(TerminalUtilities.BuildUpaAdminRequest(requestId, EcrId, UpaTransType.CancelTransaction));
            return null;  //NG SDKUPDATE 20/9/2022
        }

        public override ISAFResponse SendStoreAndForward()
        {
            var requestId = _controller.GetRequestId();
            var response = _controller.Send(TerminalUtilities.BuildUpaAdminRequest(requestId, EcrId, UpaTransType.SendSAF));
            string jsonObject = Encoding.UTF8.GetString(response);
            JsonDoc doc = JsonDoc.Parse(jsonObject);
            return new UpaSAFResponse(doc);
        }

        #region Reporting 
        //NG SDKUPDATE 20/9/2022
        /*  public override TerminalReportBuilder GetSAFReport()
          {
              return new TerminalReportBuilder(TerminalReportType.GetSAFReport);
          }

          public override TerminalReportBuilder GetBatchReport()
          {
              return new TerminalReportBuilder(TerminalReportType.GetBatchReport);
          }

          public override TerminalReportBuilder GetOpenTabDetails()
          {
              return new TerminalReportBuilder(TerminalReportType.GetOpenTabDetails);
          }*/
        #endregion
    }
}