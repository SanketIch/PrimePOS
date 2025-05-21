using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities.Payroll {
    public abstract class PayrollEntity {
        internal abstract void FromJson(JsonDoc doc, PayrollEncoder encoder);
    }
}
