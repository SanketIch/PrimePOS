using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IBalanceable {
        AuthorizationBuilder BalanceInquiry(InquiryType? inquiry);
    }
}
