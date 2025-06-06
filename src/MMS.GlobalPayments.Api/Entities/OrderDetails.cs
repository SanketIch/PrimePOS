﻿using MMS.GlobalPayments.Api.Utils;
using System;
using System.Text;

namespace MMS.GlobalPayments.Api.Entities {
    public class OrderDetails {
        public decimal? InsuranceAmount { get; set; }
        public bool HasInsurance { get; set; }
        public decimal? HandlingAmount { get; set; }
        public string Description { get; set; }
    }
}