using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IRefundable {
        AuthorizationBuilder Refund(decimal? amount = null);
    }
}
