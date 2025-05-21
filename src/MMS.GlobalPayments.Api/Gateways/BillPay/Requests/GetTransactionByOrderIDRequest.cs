using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Entities.Billing;
using MMS.GlobalPayments.Api.PaymentMethods;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal class GetTransactionByOrderIDRequest : BillPayRequestBase {
        public GetTransactionByOrderIDRequest(ElementTree et) : base(et) { }

        public string Build<T>(Element envelope, ReportBuilder<T> builder, Credentials credentials) where T : class {
            if (builder is TransactionReportBuilder<T> trb) {
                var body = et.SubElement(envelope, "soapenv:Body");
                var methodElement = et.SubElement(body, "bil:GetTransactionByOrderID");
                var requestElement = et.SubElement(methodElement, "bil:GetTransactionByOrderIDRequest");

                BuildCredentials(requestElement, credentials);

                et.SubElement(requestElement, "bdms:OrderID", trb.TransactionId);

                return et.ToString(envelope);
            }
            else
                throw new BuilderException("This method only supports TransactionReportBuilder");
        }
    }
}
