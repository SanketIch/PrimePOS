using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Network.Entities {
    [MapTarget(Target.NWS)]
    public enum Iso4217_CurrencyCode {
        [Map(Target.NWS, "124")]
        CAD,
        [Map(Target.NWS, "840")]
        USD
    }
}
