using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Entities.Billing {
    public class ConvenienceFeeResponse : BillingResponse {
        public decimal ConvenienceFee { get; set; }
    }
}
