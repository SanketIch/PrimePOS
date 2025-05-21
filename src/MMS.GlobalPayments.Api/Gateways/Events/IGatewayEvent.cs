using System;

namespace MMS.GlobalPayments.Api.Gateways.Events {
    public interface IGatewayEvent {
        GatewayEventType GetEventType();
        string GetTimestamp();
        string GetEventMessage();
    }
}
