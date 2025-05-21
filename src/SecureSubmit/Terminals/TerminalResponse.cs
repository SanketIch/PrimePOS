﻿using SecureSubmit.Terminals.Abstractions;

namespace SecureSubmit.Terminals {
    public abstract class TerminalResponse : IDeviceResponse {
        // Internal
        public string Status { get; set; }
        public string Command { get; set; }
        public string Version { get; set; }

        // Functional
        public string DeviceResponseCode { get; set; }
        public string DeviceResponseText { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
        public int? TransactionId { get; set; }
        public string TerminalRefNumber { get; set; }
        public string Token { get; set; }
        public string SignatureStatus { get; set; }
        public string ButtonNumber { get; set; } // Added By Suraj Handle ShowTextbox click
        public string TotalLength { get; set; } // Added By Suraj Handle ShowTextbox click
        public string ResponseLength { get; set; } // Added By Suraj Handle ShowTextbox click
        public string SignatureData { get; set; } // Added By Suraj Handle ShowTextbox click

        // Transactional
        public string TransactionType { get; set; }
        public string MaskedCardNumber { get; set; }
        public string EntryMethod { get; set; }
        public string ApprovalCode { get; set; }
        public decimal? TransactionAmount { get; set; }
        public decimal? AmountDue { get; set; }
        public string CardHolderName { get; set; }
        public string CardBIN { get; set; }
        public bool CardPresent { get; set; }
        public string ExpirationDate { get; set; }
        public decimal? TipAmount { get; set; }
        public decimal? CashBackAmount { get; set; }
        public string AvsResponseCode { get; set; }
        public string AvsResponseText { get; set; }
        public string CvvResponseCode { get; set; }
        public string CvvResponseText { get; set; }
        public bool TaxExempt { get; set; }
        public string TaxExemptId { get; set; }
        public string TicketNumber { get; set; }
        public string PaymentType { get; set; }

        // EMV
        public string ApplicationPreferredName { get; set; }
        public string ApplicationLabel { get; set; }
        public string ApplicationId { get; set; }
        public ApplicationCryptogramType ApplicationCryptogramType { get; set; }
        public string ApplicationCryptogram { get; set; }
        public string CardHolderVerificationMethod { get; set; }
        public string TerminalVerificationResults { get; set; }

        // Local Details Report PRIMEPOS-2862
        public string TotalRecord { get; set; }
        public string RecordNumber { get; set; }
        public string EDCType { get; set; }
        public string OrignalTransactionType { get; set; }
    }

    public enum ApplicationCryptogramType {
        TC,
        ARQC
    }
}
