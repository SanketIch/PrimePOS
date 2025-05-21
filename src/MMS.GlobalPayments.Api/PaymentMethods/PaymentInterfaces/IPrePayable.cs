using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IPrePaid {
        AuthorizationBuilder AddValue(decimal? amount = null);
    }
}
