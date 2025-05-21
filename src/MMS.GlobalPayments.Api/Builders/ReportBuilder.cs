using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Gateways;

namespace MMS.GlobalPayments.Api.Builders {
    public abstract class ReportBuilder<TResult> : BaseBuilder<TResult> where TResult : class {
        internal ReportType ReportType { get; set; }
        internal TimeZoneConversion TimeZoneConversion { get; set; }

        public ReportBuilder(ReportType type) : base() {
            ReportType = type;
        }

        /// <summary>
        /// Executes the builder against the gateway.
        /// </summary>
        /// <returns>TResult</returns>
        public override TResult Execute(string configName = "default") {
            base.Execute(configName);
            object client;
            switch (ReportType)
            {
                case ReportType.FindBankPayment:
                    client = ServicesContainer.Instance.GetOpenBanking(configName);                    
                    break;
                default:
                    client = ServicesContainer.Instance.GetReportingClient(configName);
                    break;

            }
            return ((IReportingService)client).ProcessReport(this);
        }
    }
}
