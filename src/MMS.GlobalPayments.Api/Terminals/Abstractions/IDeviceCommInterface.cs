using MMS.GlobalPayments.Api.Terminals.Messaging;

namespace MMS.GlobalPayments.Api.Terminals.Abstractions {
    public interface IDeviceCommInterface {
        void Connect();

        void Disconnect();

        byte[] Send(IDeviceMessage message);

        event MessageSentEventHandler OnMessageSent;
    }
}
