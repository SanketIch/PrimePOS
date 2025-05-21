using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IVerifiable {
        AuthorizationBuilder Verify();
    }
}
