using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    public abstract class GpApiEntity {
        internal abstract void FromJson(JsonDoc doc);
    }
}
