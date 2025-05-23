﻿using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    public interface ICardData {
        bool CardPresent { get; set; }
        string CardType { get; set; }
        string Cvn { get; set; }
        CvnPresenceIndicator CvnPresenceIndicator { get; set; }
        string Number { get; set; }
        int? ExpMonth { get; set; }
        int? ExpYear { get; set; }
        bool ReaderPresent { get; set; }
        string ShortExpiry { get; }
        ManualEntryMethod? EntryMethod { get; set; }
    }
}
