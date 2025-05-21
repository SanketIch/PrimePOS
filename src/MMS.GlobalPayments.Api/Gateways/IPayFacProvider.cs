using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Gateways
{
    interface IPayFacProvider {
        Transaction ProcessPayFac(PayFacBuilder builder);
    }
}
