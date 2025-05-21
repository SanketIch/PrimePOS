using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IChargable {
        AuthorizationBuilder Charge(decimal? amount = null);
    }
}
