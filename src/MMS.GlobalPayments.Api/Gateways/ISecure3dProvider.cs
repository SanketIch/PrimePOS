using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.Gateways {
    public interface ISecure3dProvider {
        Secure3dVersion Version { get; }

        Transaction ProcessSecure3d(Secure3dBuilder builder);
    }
}
