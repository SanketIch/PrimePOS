﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay.ResponseModels
{
    public class ResendLinkRes
    {
        public string IsProcessed { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
