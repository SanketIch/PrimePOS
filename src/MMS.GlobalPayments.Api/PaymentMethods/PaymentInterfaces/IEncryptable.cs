using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface IEncryptable {
        EncryptionData EncryptionData { get; set; }
    }
}
