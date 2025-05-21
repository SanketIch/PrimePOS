
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class InvRecvHeaderRow : DataRow 
	{
		private InvRecvHeaderTable table;

		internal InvRecvHeaderRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (InvRecvHeaderTable)this.Table;
		}
		#region Public Properties

		public System.Int32 InvRecvID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.InvRecvId];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.InvRecvId] = value; }
		}

		public System.String RefNo
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.RefNo];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.RefNo] = value; }
		}


		public System.DateTime RecvDate
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.RecvDate];
				}
				catch
				{ 
					return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.RecvDate] = value; }
		}

		public System.Int32 VendorID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.VendorId];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.VendorId] = value; }
		}

		public System.String VendorCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.VendorCode];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.VendorCode] = value; }
		}

		public System.String VendorName
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.VendorName];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.VendorName] = value; }
		}
        //Added By Shitaljit(QuicSolv) on 24 june 2011
        public System.Int32 InvTransTypeID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.InvTransTypeID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.InvTransTypeID] = value; }
        }
        //Till here added by shitaljit

        //Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577
        public System.String POOrderNo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.POOrderNo];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.POOrderNo] = value; }
        }
		#endregion 
	}
}
