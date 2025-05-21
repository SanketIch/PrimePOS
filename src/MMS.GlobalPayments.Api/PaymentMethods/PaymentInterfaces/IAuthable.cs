using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IAuthable {
        AuthorizationBuilder Authorize(decimal? amount = null, bool isEstimated = false);
    }
}
