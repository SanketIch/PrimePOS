﻿using MMS.GlobalPayments.Api.Entities.Billing;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class CommitPreloadBillsResponse: BillPayResponseBase<BillingResponse> {
        public override BillingResponse Map() {
            return new BillingResponse() {
                IsSuccessful = response.GetValue<bool>("a:IsSuccessful"),
                ResponseCode = GetFirstResponseCode(response),
                ResponseMessage = GetFirstResponseMessage(response),
            };
        }
    }
}
