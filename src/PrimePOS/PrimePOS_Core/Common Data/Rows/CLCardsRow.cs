
namespace POS_Core.CommonData.Rows 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class CLCardsRow : DataRow 
	{
		private CLCardsTable table;

		internal CLCardsRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (CLCardsTable)this.Table;
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

		public System.Boolean IsPrepetual
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.IsPrepetual];
				}
				catch
				{ 
					return false ; 
				}
			} 
			set { this[this.table.IsPrepetual] = value; }
		}

		public System.DateTime RegisterDate
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.RegisterDate];
				}
				catch
				{
                    return Convert.ToDateTime("1/1/1753 12:00:00"); //return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.RegisterDate] = value; }
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

        public System.String Description
        {
            get
            {
                    return this[this.table.Description].ToString();
            }
            set { this[this.table.Description] = value; }
        }

        public System.Decimal CurrentPoints
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.CurrentPoints]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CurrentPoints] = value; }
        }

        public System.Int32 CustomerID
        {
            get
            {
                try
                {
                    return Configuration.convertNullToInt(this[this.table.CustomerID]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CustomerID] = value; }
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
		#endregion 
	}
}
