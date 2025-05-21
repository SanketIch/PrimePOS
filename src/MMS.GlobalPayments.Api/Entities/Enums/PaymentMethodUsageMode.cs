using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    public enum PaymentMethodUsageMode {        
        [Map(Target.GP_API, "SINGLE")]
        Single,

        [Map(Target.GP_API, "MULTIPLE")]
        Multiple,
    }
}
