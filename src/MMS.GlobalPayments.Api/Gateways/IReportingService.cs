using MMS.GlobalPayments.Api.Builders;

namespace MMS.GlobalPayments.Api.Gateways {
    internal interface IReportingService {
        T ProcessReport<T>(ReportBuilder<T> builder) where T : class;
    }
}
