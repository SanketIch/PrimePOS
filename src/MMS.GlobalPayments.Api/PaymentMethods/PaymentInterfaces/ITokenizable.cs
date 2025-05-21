using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface ITokenizable {
        string Token { get; set; }
        string Tokenize(string configName = "default", PaymentMethodUsageMode paymentMethodUsageMode = PaymentMethodUsageMode.Multiple);
        string Tokenize(bool validateCard, string configName = "default", PaymentMethodUsageMode paymentMethodUsageMode = PaymentMethodUsageMode.Multiple);
        bool UpdateTokenExpiry(string configName = "default");
        bool DeleteToken(string configName = "default");
        ManagementBuilder UpdateToken();
    }
}
