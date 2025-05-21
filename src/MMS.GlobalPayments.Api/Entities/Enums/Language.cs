using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    [MapTarget(Target.GP_API)]
    public enum Language {
        [Map(Target.GP_API, "EN")]
        English,

        [Map(Target.GP_API, "ES")]
        Spanish,
    }
}