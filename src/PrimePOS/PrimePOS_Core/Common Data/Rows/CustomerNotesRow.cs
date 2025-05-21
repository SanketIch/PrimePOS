   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	//     This class is used to define the shape of CustomerRow.
	public class CustomerNotesRow : DataRow {
		private CustomerNotesTable table;

		// Constructor
		internal CustomerNotesRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (CustomerNotesTable)this.Table;
		}
		#region Public Properties


		public System.Int32 ID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.Id];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.Id] = value; }
		}

		public System.String Notes
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.Notes];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.Notes] = value; }
		}

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
					return 0 ; 
				}
			} 
			set { this[this.table.CustomerID] = value; }
		}

		public System.DateTime LastUpdatedOn
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.LastUpdatedOn];
				}
				catch
				{ 
					return DateTime.Now; 
				}
			} 
			set { this[this.table.LastUpdatedOn] = value; }
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
		#endregion // Properties
	}
}
