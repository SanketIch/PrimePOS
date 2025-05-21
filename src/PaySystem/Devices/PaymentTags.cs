using System;
using System.ComponentModel;
namespace EDevice
{
	/// <summary>
	/// Required Payment Tags
	/// </summary>
	public class PaymentTags
    {
        #region Transaction Type
        /// <summary>
		/// Transaction Type
		/// </summary>
		public enum transactionType
		{
            /// <summary>
            /// Sale transaction
            /// </summary>
			Sale = 1,
            /// <summary>
            /// Void a sale transaction that is done on the same day
            /// </summary>
			Void = 2,
            /// <summary>
            /// Return a sale transaction that was done a previous day
            /// </summary>
            Credit = 3,
			Return = 4,
			EBTCashBalanceInquiry,
			EBTFoodStampInquiry,
			EBTBalanceInguiry
		}
        /// <summary>
        /// Transaction type to be performed
        /// </summary>
        public transactionType TransactionType;
        #endregion Transaction Type

        #region Credit and Void 
        public struct transType_Reversal
		{
            public string OrderID { internal get; set; }
            public string HistoryID { internal get; set; }
		}
        /// <summary>
        /// Parameters for Reversal (Credit) Transaction
        /// </summary>
        public transType_Reversal Reversal;
        /// <summary>
        /// Parameters for Void Tranaction
        /// </summary>
        public transType_Reversal Void;
        #endregion Credit and Void

        #region Pay Type
        public enum payType
		{
			/// <summary>
			/// Debit
			/// </summary>
			Debit = 'A',
            /// <summary>
            /// Credit
            /// </summary>
            Credit = 'B',
			/// <summary>
			/// EBT Cash
			/// </summary>
			EBT_Cash = 'C',
			/// <summary>
			/// EBT Food Stamps
			/// </summary>
			EBT_Food_Stamp = 'D',
			/// <summary>
			/// Store Charge
			/// </summary>
			Store_Charge = 'E',
			/// <summary>
			/// Loyalty
			/// </summary>
			Loyalty = 'F',
			/// <summary>
			/// PayPal
			/// </summary>
			PayPal = 'G'
		}
        /// <summary>
        /// Payment Type of the Transaction
        /// </summary>
        public payType PaymentType;
        #endregion Pay Type

        #region FSA (HeathCare)
        /// <summary>
		/// Tags for All Healthcare Industry
		/// </summary>
		public struct FSATag
		{
			/// <summary>
			/// Auto Substantiation
			/// </summary>
			public struct Auto_Substantiation
			{
				/// <summary>
				/// Indicates the total of all healthcare amount(4S)
				/// </summary>
                public double TotalHealthCareAmount { internal get; set; }
				/// <summary>
				/// Indicates the subtotal of the Rx(Prescription) Amount (4U)
				/// </summary>
				public double RxAmount { internal get; set; }
			}
            /// <summary>
			/// Use for Prescription (4S) and (4U)
			/// </summary>
			public Auto_Substantiation AutoSubstantiation;
			/// <summary>
			/// Set flag to True if this transaction is FSA or contain an IIAS item
			/// </summary>
            public bool IsHealthCare { internal get; set; }
		}
        /// <summary>
        /// FSA Transaction fields if item is FSA 
        /// </summary>
        public FSATag FSA;
        #endregion FSA (HeathCare)

        #region Amounts
        /// <summary>
		/// Total Amount of the transaction. Amount should be in the Format as ASCII string
		/// E.g Amount $120.12 should be 12012
		/// </summary>
        public struct amount
        {
            /// <summary>
            /// Total Amount of the Transaction
            /// </summary>
            public double TotalAmount { internal get; set; }
            /// <summary>
            /// Cash Back Amount
            /// </summary>
            // public double CashBackAmount { internal get; set; }
        }
        public amount Amount;
        #endregion Amounts     

        #region Stored Profile
        /// <summary>
        /// Stored Profile 
        /// </summary>
        public class StoredProfile
        {
            /// <summary>
            /// Set if Card Number should be stored (Stored Profile)
            /// </summary>
            public bool IsStoredProfile { internal get; set; }
            /// <summary>
            /// Use in sale transaction
            /// </summary>
            public struct useStoreProfile
            {
                /// <summary>
                /// Token id of the original transaction
                /// </summary>
                public string TokenID { internal get; set; }
                /// <summary>
                /// last4digits of the Credit card or ACH number
                /// </summary>
                public string Las4Digits { internal get; set; }
            }
            /// <summary>
            /// Use the stored profile
            /// </summary>
            public useStoreProfile UseStoredProfile;
        }
        /// <summary>
        /// Use for storing of card information
        /// </summary>
        public StoredProfile StoreProfile { internal get; set; }
        #endregion Stored Profile

        #region Manual Entered Card Data
        /// <summary>
        /// Use if Card will be manually entered
        /// </summary>
        public class ManualCardInfo
        {
            /// <summary>
            /// Credit Card number is Keyed in Manually
            /// </summary>
            public string CCNumber { internal get; set; }
            /// <summary>
            /// Name on the Card (Card Holder Name)
            /// </summary>
            public string CCHolderName { internal get; set; }
            /// <summary>
            /// Expiration date on the Card in the format MMYY
            /// </summary>
            public string CCExpiryDate { internal get; set; }
            /// <summary>
            /// Is the Card Present 
            /// </summary>
            public bool IsCardPresent { internal get; set; }
        }
        #endregion Manual Entered Card Data
        
        
    }

    public class GatewayTags
    {
        public enum Processor
        {
            Xcharge,
            WorldPay
        }
        public Processor ProcessorName { get; set; }
        public string AccID { get; set; }
        public string MerchantPIN { get; set; }
        public string SubID { get; set; }
    }
}
