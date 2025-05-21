using MMS.GlobalPayments.Api.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Entities.PayFac
{
    public class FlashFundsPaymentCardData {
        public CreditCardData CreditCard { get; set; }
        public Address CardholderAddress { get; set; }

        public FlashFundsPaymentCardData() {
            CreditCard = new CreditCardData();
            CardholderAddress = new Address();
        }
    }
}
