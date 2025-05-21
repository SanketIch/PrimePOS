using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.Gateways {
    interface IRecurringService {
        bool SupportsRetrieval { get; }
        bool SupportsUpdatePaymentDetails { get; }
        T ProcessRecurring<T>(RecurringBuilder<T> builder) where T : class;
    }
}
