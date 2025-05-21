using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IEditable {
        ManagementBuilder Edit(decimal? amount = null);
    }
}
