using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    [MapTarget(Target.GP_API)]
    public enum Channel {
        [Map(Target.GP_API, "CP")]
        CardPresent,

        [Map(Target.GP_API, "CNP")]
        CardNotPresent,
    }
}
