   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	//     This class is used to define the shape of CustomerRow.
	public class PayOutRow : DataRow {
		private PayOutTable table;

		// Constructor
		internal PayOutRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (PayOutTable)this.Table;
		}
		#region Public Properties


		public System.Int32 PayOutId
		{
			get { 
				try { 
					return (System.Int32)this[this.table.PayOutId];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.PayOutId] = value; }
		}

		public System.Int32 DrawNo
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.DrawNo];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.DrawNo] = value; }
		}

		public System.String Description
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.Description];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.Description] = value; }
		}

		public System.Decimal Amount
		{
			get 
			{ 
				try 
				{ 
					return (System.Decimal)this[this.table.Amount];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.Amount] = value; }
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
					return "0" ; 
				}
			} 
			set { this[this.table.UserID] = value; }
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
					return DateTime.Now; 
				}
			} 
			set { this[this.table.TransDate] = value; }
		}
        public System.String PayoutCatType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PayoutCatType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PayoutCatType] = value; }
        }

        public System.Int32 PayoutCatID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PayoutcatID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PayoutcatID] = value; }

        }
		#endregion // Properties
	}
}
