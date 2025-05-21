using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.Gateways.BillPay {
    internal sealed class UpdateTokenResponse : BillPayResponseBase<Transaction> {
        public override Transaction Map() {
            return new Transaction() {
                ResponseCode = response.GetValue<string>("a:ResponseCode"),
                ResponseMessage = GetFirstResponseMessage(response),
            };
        }
    }
}
