
namespace POS_Core.CommonData.Rows 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class CLCouponsRow : DataRow 
	{
		private CLCouponsTable table;

		internal CLCouponsRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (CLCouponsTable)this.Table;
		}
		#region Public Properties

		public System.Int32 ID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.ID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.ID] = value; }
		}

		public System.Boolean IsCouponUsed
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.IsCouponUsed];
				}
				catch
				{ 
					return false ; 
				}
			} 
			set { this[this.table.IsCouponUsed] = value; }
		}

        public System.Boolean IsCouponValueInPercentage
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsCouponValueInPercentage];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsCouponValueInPercentage] = value; }
        }
        
        public System.DateTime CreatedOn
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.CreatedOn];
				}
				catch
				{
                    return Convert.ToDateTime("1/1/1753 12:00:00"); //return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.CreatedOn] = value; }
		}

		public System.Int64 CLCardID
		{
			get 
			{ 
				try 
				{ 
					return Configuration.convertNullToInt64( this[this.table.CLCardID]);
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.CLCardID] = value; }
		}

        public System.Int32 ExpiryDays
        {
            get
            {
                try
                {
                    return Configuration.convertNullToInt(this[this.table.ExpiryDays]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ExpiryDays] = value; }
        }

        public System.String CreatedBy
        {
            get
            {
                    return this[this.table.CreatedBy].ToString();
            }
            set { this[this.table.CreatedBy] = value; }
        }

        public System.Decimal Points
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.Points]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.Points] = value; }
        }

        public System.Decimal CouponValue
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.CouponValue]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CouponValue] = value; }
        }

        public System.Int64 UsedInTransID
        {
            get
            {
                try
                {
                    return Configuration.convertNullToInt64(this[this.table.UsedInTransID]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.UsedInTransID] = value; }
        }

        public System.Int64 CreatedInTransID
        {
            get
            {
                try
                {
                    return Configuration.convertNullToInt64(this[this.table.CreatedInTransID]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CreatedInTransID] = value; }
        }

        public System.Boolean IsActive
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsActive];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsActive] = value; }
        }

        //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon
        public System.Int64 CLTierID
        {
            get
            {
                try
                {
                    return Configuration.convertNullToInt64(this[this.table.CLTierID]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CLTierID] = value; }
        }
        //END
		#endregion 
	}
}
