using MMS.GlobalPayments.Api.Entities.Billing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class PreloadBillsResponse : BillPayResponseBase<BillingResponse> {
        public override BillingResponse Map() {
            return new BillingResponse {
                IsSuccessful = response.GetValue<bool>("a:isSuccessful"),
                ResponseCode = response.GetValue<string>("a:ResponseCode"),
                ResponseMessage = GetFirstResponseMessage(response),
            };
        }
    }
}
