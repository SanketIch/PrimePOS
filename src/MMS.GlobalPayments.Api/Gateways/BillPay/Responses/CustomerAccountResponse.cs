using MMS.GlobalPayments.Api.Entities.Billing;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class CustomerAccountResponse : BillPayResponseBase<BillingResponse> {
        public override BillingResponse Map() {
            return new BillingResponse {
                IsSuccessful = response.GetValue<bool>("a:isSuccessful"),
                ResponseCode = response.GetValue<string>("a:ResponseCode"),
                ResponseMessage = GetFirstResponseMessage(response),
            };
        }
    }
}
