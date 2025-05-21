using MMS.GlobalPayments.Api.Entities.Billing;
using MMS.GlobalPayments.Api.PaymentMethods;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    class UpdateTokenRequest : BillPayRequestBase {
        public UpdateTokenRequest(ElementTree et) : base(et) { }

        public string Build(Element envelope, CreditCardData card, Credentials credentials) {
            var body = et.SubElement(envelope, "soapenv:Body");
            var methodElement = et.SubElement(body, "bil:UpdateTokenExpirationDate");
            var requestElement = et.SubElement(methodElement, "bil:UpdateTokenExpirationDateRequest");

            BuildCredentials(requestElement, credentials);

            et.SubElement(requestElement, "bdms:ExpirationMonth", card.ExpMonth);
            et.SubElement(requestElement, "bdms:ExpirationYear", card.ExpYear);
            et.SubElement(requestElement, "bdms:Token", card.Token);

            return et.ToString(envelope);
        }
    }
}
