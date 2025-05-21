using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Network.Entities {
    [MapTarget(Target.NWS)]
    public enum EncryptionType {
        [Map(Target.NWS, "1")]
        TEP1,
        [Map(Target.NWS, "2")]
        TEP2
        
    }
}
