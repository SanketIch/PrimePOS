using System;
using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.PaymentMethods;

namespace MMS.GlobalPayments.Api.Services {
    public class CheckService {
        public CheckService(GatewayConfig config, string configName= "default") {
            ServicesContainer.ConfigureService(config, configName);
        }

        // Recurring


        // Charge
        public AuthorizationBuilder Charge(decimal? amount = null) {
            return new AuthorizationBuilder(TransactionType.Sale).WithAmount(amount);
        }

        // Void
        public ManagementBuilder Void(string transactionId) {
            return new ManagementBuilder(TransactionType.Void)
                .WithPaymentMethod(new TransactionReference {
                    PaymentMethodType = PaymentMethodType.ACH,
                    TransactionId = transactionId
                });
        }
    }
}
