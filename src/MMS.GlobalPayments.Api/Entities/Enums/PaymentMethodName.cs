﻿using MMS.GlobalPayments.Api.Utils;

namespace MMS.GlobalPayments.Api.Entities {
    [MapTarget(Target.GP_API)]
    public enum PaymentMethodName
    {
        [Map(Target.GP_API, "APM")]
        APM,

        [Map(Target.GP_API, "DIGITAL WALLET")]
        DigitalWallet,

        [Map(Target.GP_API, "CARD")]
        Card,

        [Map(Target.GP_API, "BANK TRANSFER")]        
        BankTransfer,

        [Map(Target.GP_ECOM, "BANK PAYMENT")]
        BankPayment,
    }
}
