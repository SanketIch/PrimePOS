using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities.Billing;

namespace MMS.GlobalPayments.Api.Gateways {
    internal interface IBillingProvider {
        /// <summary>
        /// Indicates if the Billing Gateway hosts merchant bill records
        /// </summary>
        bool IsBillDataHosted { get; }
        BillingResponse ProcessBillingRequest(BillingBuilder builder);
    }
}
