using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Entities.Enums
{
    public enum ProPayAccountStatus {
        ReadyToProcess,
        FraudAccount,
        RiskwiseDeclined,
        Hold,
        Canceled,
        FraudVictim,
        ClosedEula,
        ClosedExcessiveChargeback
    }
}
