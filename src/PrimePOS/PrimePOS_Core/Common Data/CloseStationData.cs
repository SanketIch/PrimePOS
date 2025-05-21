using System;
using System.Collections;
namespace POS_Core.CommonData
{
	/// <summary>
	/// Summary description for CloseStationData.
	/// </summary>
	public class CloseStationData
	{
		private System.Decimal m_TotalSale = 0;
		private System.Decimal m_TotalReturn = 0; 
		private System.Decimal m_TotalDiscount = 0;
		private System.Decimal m_NetSale = 0;
		private System.Decimal m_SalesTax = 0;
		private System.Decimal m_TotalCash = 0;
		private System.Decimal m_ReceiveOnAccount = 0;
		private System.Decimal m_GrandTotal = 0;
		private System.Decimal m_PayTypeTotal = 0;
		//private System.Decimal m_TotalCashPT = 0;
		private System.Decimal m_Payout = 0;
		private string m_StationCloseNo = "";
		private string m_StationID= "";
		//private System.Decimal m_NetCash = 0;
		private colPayoutDetail m_Details = new colPayoutDetail();
        //Added By Shitaljit(QuicSolv) on june 24 2011
        private string m_VerifiedBy = "";
        private string m_VerifiedDate;
        private System.Decimal m_VerifiedAmt = 0;
        //End
        private System.Decimal m_DefCDStartBalance = 0; //Sprint-19 - 2165 19-Mar-2015 JY Added
        private string m_CloseDate = string.Empty;  //PRIMEPOS-2480 26-Jun-2020 JY Added
		private System.Decimal m_TransFee = 0;  //PRIMEPOS-3118 03-Aug-2022 JY Added

		public CloseStationData()
		{
		}
						
		public System.Decimal NetCash
		{
			get 
			{ 
				return  this.TotalCashPT - m_Payout;
			} 
		}

		public System.Decimal Payout
		{
			get 
			{ 
				return m_Payout;
			} 
			set { this.m_Payout = value; }
		}

		public System.String StationCloseNo
		{
			get 
			{ 
				return m_StationCloseNo;
			} 
			set { this.m_StationCloseNo = value; }
		}

		public System.String StationID
		{
			get 
			{ 
				return m_StationID;
			} 
			set { this.m_StationID= value; }
		}

		public System.Decimal Total
		{
			get 
			{ 
				return this.Details.TotalCash;
			} 
		}

		public System.Decimal TotalCashPT
		{
			get 
			{ 
				return GetCash();
			} 
		}

		public System.Decimal GetCash()
		{
			System.Decimal CashAmount=0;
			for(int i = 0; i< this.Details.Count;i++)
			{
				if (this.Details[i].PayTypeName == "Cash")
					CashAmount = this.Details[i].Amount;
			}
			return CashAmount;
		}

		public System.Decimal PayTypeTotal
		{
			get 
			{ 
				return m_PayTypeTotal;
			} 
			set { this.m_PayTypeTotal = value; }
		}

		public System.Decimal GrandTotal
		{
			get 
			{ 
				return TotalCash+m_ReceiveOnAccount + m_TransFee;   //PRIMEPOS-3118 03-Aug-2022 JY Added Transfee
			} 
			set { this.m_GrandTotal = value; }
		}

		public System.Decimal ReceiveOnAccount
		{
			get 
			{ 
				return m_ReceiveOnAccount;
			} 
			set { this.m_ReceiveOnAccount = value; }
		}

		public System.Decimal TotalCash
		{
			get 
			{ 
				return this.NetSale+m_SalesTax;
			} 
			set { this.m_TotalCash = value; }
		}

		public System.Decimal SalesTax
		{
			get 
			{ 
				return m_SalesTax;
			} 
			set { this.m_SalesTax = value; }
		}

		public System.Decimal NetSale
		{
			get 
			{ 
				return m_TotalSale-m_TotalDiscount+m_TotalReturn;
			} 
			set { this.m_NetSale = value; }
		}

		public System.Decimal TotalDiscount
		{
			get 
			{ 
				return m_TotalDiscount;
			} 
			set { this.m_TotalDiscount = value; }
		}

		public System.Decimal TotalReturn
		{
			get 
			{ 
				return m_TotalReturn;
			} 
			set { this.m_TotalReturn = value; }
		}


		public System.Decimal TotalSale
		{
			get 
			{ 
					return m_TotalSale;
			} 
			set { this.m_TotalSale = value; }
		}

		public colPayoutDetail Details
		{
			get 
			{ 
				return m_Details;
			} 
		}
    //Added By shitaljit(QuicSolv) on 24 june 2011

        public System.Decimal VerifiedAmount
        {
            get
            {
                return m_VerifiedAmt;
            }
            set { this.m_VerifiedAmt = value; }
        }

        public string VerifiedBy
        {
            get
            {
                return m_VerifiedBy;
            }
            set { this.m_VerifiedBy = value; }
        }

        public string VerifiedDate
        {
            get
            {
                return m_VerifiedDate;
            }
            set { this.m_VerifiedDate = value; }
        }
        //End of added by shitaljit

        #region Sprint-19 - 2165 19-Mar-2015 JY Added
        public System.Decimal DefCDStartBalance
        {
            get
            {
                return m_DefCDStartBalance;
            }
            set { this.m_DefCDStartBalance = value; }
        }
        #endregion

        #region PRIMEPOS-2480 26-Jun-2020 JY Added
        public string CloseDate
        {
            get
            {
                return m_CloseDate;
            }
            set { this.m_CloseDate = value; }
        }
		#endregion

		//PRIMEPOS-3118 03-Aug-2022 JY Added
		public System.Decimal TransFee
		{
			get
			{
				return m_TransFee;
			}
			set { this.m_TransFee = value; }
		}
	}

    public class colPayoutDetail
	{
		
		ArrayList m_oCol = new ArrayList();

		public int Count
		{
			get
			{ 
				return this.m_oCol.Count;
			} 
		}

		public System.Decimal TotalCash
		{
			get
			{ 
				return CountTotalCash();
			} 
		}

		private System.Decimal CountTotalCash()
		{
			System.Decimal totalAmount=0;
			for( int i = 0; i<m_oCol.Count;i++)
			{
				totalAmount = totalAmount + this[i].Amount;
			}
			return totalAmount;
		}

		public void Clear()
		{
			m_oCol.Clear();
		}

		public int Add(CloseStationDetail Value)
		{
			return m_oCol.Add(Value);
		}

		public CloseStationDetail this[int index]
		{
			get
			{ 
				return (CloseStationDetail)m_oCol[index];
			} 
		}
	}

	public class CloseStationDetail
	{

		private string m_PayTypeName = "";
		private System.Decimal m_Amount = 0; 

		public System.String PayTypeName
		{
			get 
			{ 
				return m_PayTypeName;
			} 
			set { this.m_PayTypeName = value; }
		}
		public System.Decimal Amount
		{
			get 
			{ 
				return m_Amount;
			} 
			set { this.m_Amount = value; }
		}
        
	}
}
