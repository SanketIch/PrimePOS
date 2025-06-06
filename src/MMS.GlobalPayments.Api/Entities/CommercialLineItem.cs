﻿namespace MMS.GlobalPayments.Api.Entities {
    public class CommercialLineItem {
        public string AlternateTaxId { get; set; }
        public string CommodityCode { get; set; }
        public CreditDebitIndicator CreditDebitIndicator { get; set; }
        public string Description { get; set; }
        public DiscountDetails DiscountDetails { get; set; }
        public decimal? ExtendedAmount { get; set; }
        public string Name { get; set; }
        public NetGrossIndicator NetGrossIndicator { get; set; }
        public string ProductCode { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public string UnitOfMeasure { get; set; }
        public string UPC { get; set; }
        public decimal? TaxAmount { get; set; }
        public string TaxName { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
