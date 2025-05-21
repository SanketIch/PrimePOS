using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    [MapTarget(Target.GP_API)]
    public enum StoredPaymentMethodStatus {
        [Map(Target.GP_API, "ACTIVE")]
        Active,
    }
}
