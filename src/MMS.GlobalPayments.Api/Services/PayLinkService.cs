using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Entities.Reporting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Services
{
    public class PayLinkService
    {
        public static AuthorizationBuilder Create(PayLinkData payLink, decimal amount)
        {
            return (new AuthorizationBuilder(TransactionType.Create))
                .WithAmount(amount)
                .WithPayLinkData(payLink);
        }

        public static ManagementBuilder Edit(string payLinkId)
        {
            return (new ManagementBuilder(TransactionType.PayLinkUpdate))
                .WithPaymentLinkId(payLinkId);
        }


        public static TransactionReportBuilder<PayLinkSummary> PayLinkDetail(string payLinkId)
        {
            return (new TransactionReportBuilder<PayLinkSummary>(ReportType.PayLinkDetail))
                .WithPayLinkId(payLinkId);
        }

        public static TransactionReportBuilder<PagedResult<PayLinkSummary>> FindPayLink(int page, int pageSize)
        {
            return (new TransactionReportBuilder<PagedResult<PayLinkSummary>>(ReportType.FindPayLinkPaged))
                .WithPaging(page, pageSize);
        }
    }
}
