﻿using System;
using System.Collections.Generic;

namespace Vantiv.ResponseModels
{
    public class ReversalResponse
    {
        public int convenienceFeeAmount { get; set; }
        public string paymentType { get; set; }
        public string terminalId { get; set; }
        public double totalAmount { get; set; }
        public string accountNumber { get; set; }
        public string approvalNumber { get; set; }
        public bool isApproved { get; set; }
        public Processor _processor { get; set; }
        public string statusCode { get; set; }
        public DateTime transactionDateTime { get; set; }
        public string transactionId { get; set; }
        public string merchantId { get; set; }
        public bool isOffline { get; set; }
        public List<object> _errors { get; set; }
        public bool _hasErrors { get; set; }
        public List<object> _links { get; set; }
        public List<object> _logs { get; set; }
        public string _type { get; set; }
        public List<object> _warnings { get; set; }
    }

}
