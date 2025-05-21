﻿using System.Collections.Generic;
using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.Services {
    public class ReportingService {
        public static TransactionReportBuilder<List<TransactionSummary>> FindTransactions(string transactionId = null) {
            return new TransactionReportBuilder<List<TransactionSummary>>(ReportType.FindTransactions)
                .WithTransactionId(transactionId);
        }

        public static TransactionReportBuilder<List<DepositSummary>> FindDeposits() {
            return new TransactionReportBuilder<List<DepositSummary>>(ReportType.FindDeposits);
        }

        public static TransactionReportBuilder<List<DisputeSummary>> FindDisputes() {
            return new TransactionReportBuilder<List<DisputeSummary>>(ReportType.FindDisputes);
        }

        public static TransactionReportBuilder<List<TransactionSummary>> Activity() {
            return new TransactionReportBuilder<List<TransactionSummary>>(ReportType.Activity);
        }

        public static TransactionReportBuilder<TransactionSummary> TransactionDetail(string transactionId) {
            return new TransactionReportBuilder<TransactionSummary>(ReportType.TransactionDetail)
                .WithTransactionId(transactionId);
        }

        public static TransactionReportBuilder<DepositSummary> DepositDetail(string depositId) {
            return new TransactionReportBuilder<DepositSummary>(ReportType.DepositDetail)
                .WithDepositReference(depositId);
        }
       
        public static TransactionReportBuilder<DisputeSummary> DisputeDetail(string disputeId) {
            return new TransactionReportBuilder<DisputeSummary>(ReportType.DisputeDetail)
                .WithDisputeId(disputeId);
        }

        public static TransactionReportBuilder<DisputeDocument> DocumentDisputeDetail(string disputeId)
        {
            return new TransactionReportBuilder<DisputeDocument>(ReportType.DocumentDisputeDetail)
                .WithDisputeId(disputeId);
        }

        public static TransactionReportBuilder<DisputeSummary> SettlementDisputeDetail(string settlementDisputeId) {
            return new TransactionReportBuilder<DisputeSummary>(ReportType.SettlementDisputeDetail)
                .WithSettlementDisputeId(settlementDisputeId);
        }

        public static TransactionReportBuilder<StoredPaymentMethodSummary> StoredPaymentMethodDetail(string storedPaymentMethodId) {
            return new TransactionReportBuilder<StoredPaymentMethodSummary>(ReportType.StoredPaymentMethodDetail)
                .WithStoredPaymentMethodId(storedPaymentMethodId);
        }

        public static TransactionReportBuilder<ActionSummary> ActionDetail(string actionId) {
            return new TransactionReportBuilder<ActionSummary>(ReportType.ActionDetail)
                .WithActionId(actionId);
        }

        public static TransactionReportBuilder<PagedResult<TransactionSummary>> BankPaymentDetail(string bankPaymentId, int page, int pageSize)
        {
            return new TransactionReportBuilder<PagedResult<TransactionSummary>>(ReportType.FindBankPayment)
                .WithBankPaymentId(bankPaymentId)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<TransactionSummary>> FindBankPaymentTransactions(int page, int pageSize)
        {
            return new TransactionReportBuilder<PagedResult<TransactionSummary>>(ReportType.FindBankPayment)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<TransactionSummary>> FindTransactionsPaged(int page, int pageSize, string transactionId = null) {
            return new TransactionReportBuilder<PagedResult<TransactionSummary>>(ReportType.FindTransactionsPaged)
                .WithTransactionId(transactionId)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<TransactionSummary>> FindSettlementTransactionsPaged(int page, int pageSize) {
            return new TransactionReportBuilder<PagedResult<TransactionSummary>>(ReportType.FindSettlementTransactionsPaged)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<DepositSummary>> FindDepositsPaged(int page, int pageSize) {
            return new TransactionReportBuilder<PagedResult<DepositSummary>>(ReportType.FindDepositsPaged)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<DisputeSummary>> FindDisputesPaged(int page, int pageSize) {
            return new TransactionReportBuilder<PagedResult<DisputeSummary>>(ReportType.FindDisputesPaged)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<DisputeSummary>> FindSettlementDisputesPaged(int page, int pageSize) {
            return new TransactionReportBuilder<PagedResult<DisputeSummary>>(ReportType.FindSettlementDisputesPaged)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<StoredPaymentMethodSummary>> FindStoredPaymentMethodsPaged(int page, int pageSize) {
            return new TransactionReportBuilder<PagedResult<StoredPaymentMethodSummary>>(ReportType.FindStoredPaymentMethodsPaged)
                .WithPaging(page, pageSize);
        }

        public static TransactionReportBuilder<PagedResult<ActionSummary>> FindActionsPaged(int page, int pageSize) {
            return new TransactionReportBuilder<PagedResult<ActionSummary>>(ReportType.FindActionsPaged)
                .WithPaging(page, pageSize);
        }
    }
}
