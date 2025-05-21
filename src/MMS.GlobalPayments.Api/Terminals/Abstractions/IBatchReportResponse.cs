using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.HPA.Responses;
using System.Collections.Generic;

namespace MMS.GlobalPayments.Api.Terminals.Abstractions {
    public interface IBatchReportResponse : IDeviceResponse {
        BatchSummary BatchSummary { get; }
        CardBrandSummary VisaSummary { get; }
        CardBrandSummary MasterCardSummary { get; }
        CardBrandSummary AmexSummary { get; }
        CardBrandSummary DiscoverSummary { get; }
        CardBrandSummary PaypalSummary { get; }
        List<TransactionSummary> TransactionSummaries { get; }
    }
}
