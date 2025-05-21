using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    public interface IPaymentMethod {
        PaymentMethodType PaymentMethodType { get; }
    }
}
