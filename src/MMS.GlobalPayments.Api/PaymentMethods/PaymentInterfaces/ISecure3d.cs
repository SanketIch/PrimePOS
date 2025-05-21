using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    interface ISecure3d {
        ThreeDSecure ThreeDSecure { get; set; }
    }
}
