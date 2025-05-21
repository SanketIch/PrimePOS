using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    public enum StoredCredentialSequence {
        [Map(Target.GP_API, "FIRST")]
        First,
        [Map(Target.GP_API, "SUBSEQUENT")]
        Subsequent,
        [Map(Target.GP_API, "LAST")]
        Last
    }
}
