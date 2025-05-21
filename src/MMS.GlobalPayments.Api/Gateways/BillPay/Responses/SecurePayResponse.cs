using MMS.GlobalPayments.Api.Entities.Billing;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class SecurePayResponse : BillPayResponseBase<LoadSecurePayResponse> {
        public override LoadSecurePayResponse Map() {
            return new LoadSecurePayResponse {
                PaymentIdentifier = response.GetValue<string>("a:GUID"),
                IsSuccessful = response.GetValue<bool>("a:isSuccessful"),
                ResponseCode = response.GetValue<string>("a:ResponseCode"),
                ResponseMessage = GetFirstResponseMessage(response),
            };
        }
    }
}
