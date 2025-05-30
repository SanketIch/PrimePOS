﻿using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Entities.Billing;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class DeleteSingleSignOnAccountRequest : BillPayRequestBase {
        public DeleteSingleSignOnAccountRequest(ElementTree et) : base(et) { }

        public string Build(Element envelope, Credentials credentials, Customer customer) {
            var body = et.SubElement(envelope, "soapenv:Body");
            var methodElement = et.SubElement(body, "bil:DeleteSingleSignOnAccount");
            var requestElement = et.SubElement(methodElement, "bil:request");

            BuildCredentials(requestElement, credentials);

            et.SubElement(requestElement, "bdms:MerchantCustomerID", customer.Id);

            return et.ToString(envelope);
        }
    }
}
