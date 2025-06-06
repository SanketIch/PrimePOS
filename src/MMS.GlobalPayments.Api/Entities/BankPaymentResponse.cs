﻿using MMS.GlobalPayments.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Entities
{
    public class BankPaymentResponse {
        public string Id { get; set; }
        public string RedirectUrl { get; set; }
        public string PaymentStatus { get; set; }
        public BankPaymentType? Type { get; set; }
        public string TokenRequestId { get; set; }
        public string SortCode { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Iban { get; set; }
    }
}
