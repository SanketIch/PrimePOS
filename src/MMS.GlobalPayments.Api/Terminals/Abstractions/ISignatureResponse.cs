namespace MMS.GlobalPayments.Api.Terminals.Abstractions {
    public interface ISignatureResponse : IDeviceResponse {
        byte[] SignatureData { get; set; }
    }
}
