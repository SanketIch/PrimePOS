using System;
namespace MMS.GlobalPayments.Api.Terminals.Abstractions {
    public interface IInitializeResponse : IDeviceResponse {
        string SerialNumber { get; set; }
    }
}
