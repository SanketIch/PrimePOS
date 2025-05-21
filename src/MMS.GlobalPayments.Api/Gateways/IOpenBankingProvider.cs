using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Gateways
{
    public interface IOpenBankingProvider
    {
        bool SupportsHostedPayments { get; }
        Transaction ProcessOpenBanking(BankPaymentBuilder builder);
        string SerializeRequest(BankPaymentBuilder builder);
    }
}
