using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IReversable {
        AuthorizationBuilder Reverse(decimal? amount = null);
    }
}
