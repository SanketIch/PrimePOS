using MMS.GlobalPayments.Api.Terminals.Builders;

namespace MMS.GlobalPayments.Api.Terminals.Abstractions {
    public interface IRequestIdProvider {
        int GetRequestId();
    }
}
