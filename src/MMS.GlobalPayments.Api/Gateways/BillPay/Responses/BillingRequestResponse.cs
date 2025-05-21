using MMS.GlobalPayments.Api.Entities.Billing;
using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class BillingRequestResponse : BillPayResponseBase<BillingResponse> {
        public override BillingResponse Map() {
            return new BillingResponse {
                IsSuccessful = response.GetValue<bool>("a:IsSuccessful"),
                ResponseCode = response.GetValue<string>("a:ResponseCode"),
                ResponseMessage = GetFirstResponseMessage(response),
            };
        }
    }
}
