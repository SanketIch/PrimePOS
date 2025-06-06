﻿using MMS.GlobalPayments.Api.Entities.Billing;
using MMS.GlobalPayments.Api.PaymentMethods;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class DeleteCustomerAccountRequest : BillPayRequestBase {
        public DeleteCustomerAccountRequest(ElementTree et) : base(et) { }

        public string Build(Element envelope, Credentials credentials, RecurringPaymentMethod paymentMethod) {
            var body = et.SubElement(envelope, "soapenv:Body");
            var methodElement = et.SubElement(body, "bil:DeleteCustomerAccount");
            var requestElement = et.SubElement(methodElement, "bil:DeleteCustomerAccountRequest");

            BuildCredentials(requestElement, credentials);

            et.SubElement(requestElement, "bdms:CustomerAccountNameToDelete", paymentMethod.Id);
            et.SubElement(requestElement, "bdms:MerchantCustomerID", paymentMethod.CustomerKey);

            return et.ToString(envelope);
        }
    }
}
