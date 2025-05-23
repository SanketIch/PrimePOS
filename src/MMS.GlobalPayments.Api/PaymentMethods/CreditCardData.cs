﻿using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Utils;
using System;
using System.Collections.Generic;

namespace MMS.GlobalPayments.Api.PaymentMethods {
    /// <summary>
    /// Use credit tokens or manual entry data as a payment method.
    /// </summary>
    public class CreditCardData : Credit, ICardData {
        private string cvn;

        /// <summary>
        /// Indicates if the card is present with the merchant at time of payment.
        /// </summary>
        /// <remarks>
        /// Default value is `false`.
        /// </remarks>
        public bool CardPresent { get; set; }

        /// <summary>
        /// The card's card verification number (CVN).
        /// </summary>
        /// <remarks>
        /// When set, `CreditCardData.CvnPresenceIndicator` is set to
        /// `CvnPresenceIndicator.Present`.
        /// </remarks>
        public string Cvn {
            get { return cvn; }
            set {
                if (!string.IsNullOrEmpty(value)) {
                    cvn = value;
                    CvnPresenceIndicator = CvnPresenceIndicator.Present;
                }
            }
        }

        /// <summary>
        /// The name on the front of the card.
        /// </summary>
        public string CardHolderName { get; set; }

        /// <summary>
        /// Indicates card verification number (CVN) presence.
        /// </summary>
        /// <remarks>
        /// Default value is `CvnPresenceIndicator.NotRequested`.
        /// </remarks>
        public CvnPresenceIndicator CvnPresenceIndicator { get; set; }

        /// <summary>
        /// The card's number.
        /// </summary>
        private string _number;
        public string Number {
            get { return _number; }
            set {
                _number = value;
                try {
                    CardType = CardUtils.MapCardType(_number);
                    FleetCard = CardUtils.IsFleet(CardType, _number);
                    PurchaseCard = CardUtils.IsPurchase(CardType, _number);
                    ReadyLinkCard = CardUtils.IsReadyLink(CardType, _number);
                }
                catch (Exception) {
                    CardType = "Unknown";
                }
            }
        }

       public ManualEntryMethod? EntryMethod { get; set; }

        /// <summary>
        /// The card's expiration month.
        /// </summary>
        public int? ExpMonth { get; set; }

        internal int? _expYear;

        /// <summary>
        /// The card's expiration year.
        /// </summary>
        public int? ExpYear {
            get { return _expYear; }
            set {
                if (value.HasValue && (int)Math.Floor(Math.Log10(value.Value)) + 1 == 2) {
                    _expYear = value + 2000;
                } else {
                    _expYear = value;
                }
            }
        }

        public string ShortExpiry {
            get {
                var month = (ExpMonth.HasValue) ? ExpMonth.ToString().PadLeft(2, '0') : string.Empty;
                var year = (ExpYear.HasValue) ? ExpYear.ToString().PadLeft(4, '0').Substring(2, 2) : string.Empty;
                return month + year;
            }
        }

        /// <summary>
        /// Indicates if a card reader was used when accepting the card data.
        /// </summary>
        /// <remarks>
        /// Default value is `false`.
        /// </remarks>
        public bool ReaderPresent { get; set; }
        public string Cardutils { get; private set; }

        public CreditCardData(string token = null) : base() {
            Token = token;
            CardPresent = false;
            ReaderPresent = false;
            CvnPresenceIndicator = CvnPresenceIndicator.NotRequested;
        }

        public AuthorizationBuilder GetDccRate(DccRateType dccRateType = DccRateType.None, DccProcessor dccProcessor = DccProcessor.None) {
            DccRateData dccRateData = new DccRateData {
                DccRateType = dccRateType,
                DccProcessor = dccProcessor
            };

            return new AuthorizationBuilder(TransactionType.DccRateLookup, this).WithDccRateData(dccRateData);
        }

        [Obsolete("VerifyEnrolled is deprecated. Please use CheckEnrollment from Secure3dService")]
        public bool VerifyEnrolled(decimal amount, string currency, string orderId = null, string configName = "default") {
            Transaction response = new AuthorizationBuilder(TransactionType.VerifyEnrolled, this)
                .WithAmount(amount)
                .WithCurrency(currency)
                .WithOrderId(orderId)
                .Execute(configName);

            if (response.ThreeDSecure != null) {
                ThreeDSecure = response.ThreeDSecure;
                ThreeDSecure.Amount = amount;
                ThreeDSecure.Currency = currency;
                ThreeDSecure.OrderId = response.OrderId;

                if (new List<string> { "N", "U" }.Contains(ThreeDSecure.Enrolled)) {
                    ThreeDSecure.Xid = null;
                    if (ThreeDSecure.Enrolled == "N")
                        ThreeDSecure.Eci = CardType == "MC" ? "1" : "6";
                    else if (ThreeDSecure.Enrolled == "U")
                        ThreeDSecure.Eci = CardType == "MC" ? "0" : "7";
                }

                return ThreeDSecure.Enrolled == "Y";
            }
            return false;
        }

        [Obsolete("VerifySignature is deprecated. Please use GetAuthenticationData from Secure3dService")]
        public bool VerifySignature(string authorizationResponse, decimal? amount, string currency, string orderId, string configName = "default") {
            // ensure we have an object
            if (ThreeDSecure == null)
                ThreeDSecure = new ThreeDSecure();

            ThreeDSecure.Amount = amount;
            ThreeDSecure.Currency = currency;
            ThreeDSecure.OrderId = orderId;

            return VerifySignature(authorizationResponse, null, configName);
        }

        [Obsolete("VerifySignature is deprecated. Please use GetAuthenticationData from Secure3dService")]
        public bool VerifySignature(string authorizationResponse, MerchantDataCollection merchantData = null, string configName = "default") {
            // ensure we have an object
            if (ThreeDSecure == null)
                ThreeDSecure = new ThreeDSecure();

            // if we have some merchantData use it
            if (merchantData != null)
                ThreeDSecure.MerchantData = merchantData;

            Transaction response = new ManagementBuilder(TransactionType.VerifySignature)
            .WithAmount(ThreeDSecure.Amount)
            .WithCurrency(ThreeDSecure.Currency)
            .WithPayerAuthenticationResponse(authorizationResponse)
            .WithPaymentMethod(new TransactionReference {
                OrderId = ThreeDSecure.OrderId
            })
            .Execute(configName);

            ThreeDSecure.Status = response.ThreeDSecure.Status;
            ThreeDSecure.Cavv = response.ThreeDSecure.Cavv;
            ThreeDSecure.Algorithm = response.ThreeDSecure.Algorithm;
            ThreeDSecure.Xid = response.ThreeDSecure.Xid;

            if (new List<string> { "A", "Y" }.Contains(ThreeDSecure.Status) && response.ResponseCode == "00") {
                ThreeDSecure.Eci = response.ThreeDSecure.Eci;
                return true;
            }
            else {
                ThreeDSecure.Eci = CardType == "MC" ? "0" : "7";
                return false;
            }
        }
        public bool HasInAppPaymentData()
        {
            return (!string.IsNullOrEmpty(this.Token) && !string.IsNullOrEmpty(this.MobileType));
        }
    }
}
