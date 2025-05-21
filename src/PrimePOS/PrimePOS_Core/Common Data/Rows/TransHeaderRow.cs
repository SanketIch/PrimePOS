namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TransHeaderRow : DataRow 
	{
		private TransHeaderTable table;

		internal TransHeaderRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (TransHeaderTable)this.Table;
		}
		#region Public Properties

		public System.Int32 TransID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.TransID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.TransID] = value; }
		}

        public System.Int32 ReturnTransID
        {
            get
            {
                try
                {
                    if (this[this.table.ReturnTransID].ToString().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return (System.Int32)this[this.table.ReturnTransID];
                    }
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ReturnTransID] = value; }
        }

		public System.DateTime TransDate
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.TransDate];
				}
				catch
				{ 
					return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.TransDate] = value; }
		}

        //Following Code Added by Krishna on 2 June 2011
        public System.DateTime TransactionStartDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.TransactionStartDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.TransactionStartDate] = value; }
        }

        //Till here Added by Krishna on 2 June 2011

		public System.Int32 CustomerID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.CustomerID];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.CustomerID] = value; }
		}

		public System.String CustomerCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.CustomerCode];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.CustomerCode] = value; }
		}

        public System.String StationID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.StationID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.StationID] = value; }
        }

		public System.String UserID
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.UserID];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.UserID] = value; }
		}

		public System.String CustomerName
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.CustomerName];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.CustomerName] = value; }
		}

		public System.Int32 TransType
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.TransType];
				}
				catch
				{
					return 0;
				}
			}
				set { this[this.table.TransType]=value; }
		}
		
		public System.Decimal GrossTotal
		{
			get
			{
				try
				{
					return (System.Decimal)this[this.table.GrossTotal];
				}
				catch
				{
					return 0;
				}
			}
				set { this[this.table.GrossTotal]=value; }
		}

		public System.Decimal TotalDiscAmount
		{
			get
			{
				try
				{
					return (System.Decimal)this[this.table.TotalDiscAmount];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.TotalDiscAmount]=value; }
		}

		public System.Decimal TotalTaxAmount
		{
			get
			{
				try
				{
					return (System.Decimal)this[this.table.TotalTaxAmount];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.TotalTaxAmount]=value; }
		}
		
		public System.Decimal TenderedAmount
		{
			get
			{
				try
				{
					return (System.Decimal)this[this.table.TenderedAmount];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.TenderedAmount]=value; }
		}

		public System.Decimal TotalPaid
		{
			get
			{
				try
				{
					return (System.Decimal)this[this.table.TotalPaid];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.TotalPaid]=value; }
		}

		public System.Int32 isStationClosed
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.isStationClosed];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.isStationClosed]=value; }
		}

        public System.Boolean IsDelivery
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsDelivery];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsDelivery] = value; }
        }
		
		public System.Int32 isEOD
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.isEOD];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.isEOD]=value; }
		}
		
		public System.Int32 DrawerNo
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.DrawerNo];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.DrawerNo]=value; }
		}

		public System.Int32 stClosedID
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.StClosedID];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.StClosedID]=value; }
		}
		
		public System.Int32 EODID
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.EODID];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.EODID]=value; }
		}

		public System.Int64 Acc_No
		{
			get
			{
				try
				{
                    if (this[this.table.Acc_No].ToString().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return (long)this[this.table.Acc_No];
                    }
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.Acc_No]=value; }
		}

        public System.Decimal LoyaltyPoints
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.LoyaltyPoints];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LoyaltyPoints] = value; }
        }

        //Added By Shitaljit(QuicSolv) on 31 August 2011
        public System.Decimal InvoiceDiscount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.InvoiceDiscount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.InvoiceDiscount] = value; }
        }
        //Till Here Added By shitaljit.

        public System.String DeliveryAddress
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DeliveryAddress];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.DeliveryAddress] = value; }
        }

        #region Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
        public System.Boolean WasonHold
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.WasonHold];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.WasonHold] = value; }
        }

        public System.Boolean DeliverySigSkipped
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.DeliverySigSkipped];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.DeliverySigSkipped] = value; }
        }
        #endregion

        #region NileshJ Solutran PRIMEPOS-2663
        public System.Int64 S3TransID //PRIMEPOS-3265
        {
            get
            {
                try
                {
                    return (System.Int64)this[this.table.S3TransID]; //PRIMEPOS-3265
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3TransID] = value; }
        }

        public System.Decimal S3PurAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3PurAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3PurAmount] = value; }
        }

        public System.Decimal S3TaxAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3TaxAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3TaxAmount] = value; }
        }
        public System.Decimal S3DiscountAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3DiscountAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3DiscountAmount] = value; }
        }
        #endregion

        #region PRIMEPOS-2865 16-Jul-2020 JY Added
        public System.Int32 AllowRxPicked
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.AllowRxPicked];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.AllowRxPicked] = value; }
        }
		#endregion

		#region PRIMEPOS-2915
		public System.Boolean IsCustomerDriven
		{
			get
			{
				try
				{
					return (System.Boolean)this[this.table.IsCustomerDriven];
				}
				catch
				{
					return false;
				}
			}
			set { this[this.table.IsCustomerDriven] = value; }
		}
		#endregion

		#region PRIMEPOS-3053 08-Feb-2021 JY Added
		public System.Int32 RxTaxPolicyID
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.RxTaxPolicyID];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.RxTaxPolicyID] = value; }
		}
		#endregion

		//PRIMEPOS-3117 11-Jul-2022 JY Added
		public System.Decimal TotalTransFeeAmt
		{
			get
			{
				try
				{
					return (System.Decimal)this[this.table.TotalTransFeeAmt];
				}
				catch
				{
					return 0;
				}
			}
			set { this[this.table.TotalTransFeeAmt] = value; }
		}
		#endregion
	}
}