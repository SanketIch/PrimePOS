using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.Gateways {
    internal interface IPaymentGateway {
        Transaction ProcessAuthorization(AuthorizationBuilder builder);
        Transaction ManageTransaction(ManagementBuilder builder);
        string SerializeRequest(AuthorizationBuilder builder);
        bool SupportsHostedPayments { get; }
    }
}
