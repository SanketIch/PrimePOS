   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	//     This class is used to define the shape of CustomerRow.
	public class PhysicalInvRow : DataRow {
		private PhysicalInvTable table;

		// Constructor
		internal PhysicalInvRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (PhysicalInvTable)this.Table;
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

		public System.String ItemCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.ItemCode];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.ItemCode] = value; }
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

		public System.Int32 OldQty
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.OldQty];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.OldQty] = value; }
		}

		public System.Int32 NewQty
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.NewQty];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.NewQty] = value; }
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
		
		public System.Boolean isProcessed
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.isProcessed];
				}
				catch
				{ 
					return false ; 
				}
			} 
			set { this[this.table.isProcessed] = value; }
		}
		
		public System.DateTime PTransDate
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.PTransDate];
				}
				catch
				{ 
					return DateTime.Now; 
				}
			} 
			set { this[this.table.PTransDate] = value; }
		}

		public System.String PUserID
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.PUserID];
				}
				catch
				{ 
					return "0" ; 
				}
			} 
			set { this[this.table.PUserID] = value; }
		}

        public System.Decimal OldSellingPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.OldSellingPrice];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.OldSellingPrice] = value; }
        }

        public System.Decimal NewSellingPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.NewSellingPrice];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.NewSellingPrice] = value; }
        }

        public System.Decimal OldCostPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.OldCostPrice];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.OldCostPrice] = value; }
        }

        public System.Decimal NewCostPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.NewCostPrice];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.NewCostPrice] = value; }
        }

        #region Sprint-21 - 2206 09-Mar-2016 JY Added
        public System.Object OldExpDate
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.OldExpDate];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.OldExpDate] = value; }
        }

        public System.Object NewExpDate
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.NewExpDate];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.NewExpDate] = value; }
        }
        #endregion

        //PRIMEPOS-2395 21-Jun-2018 JY Added
        public System.Int32 LastInvUpdatedQty
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.LastInvUpdatedQty];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LastInvUpdatedQty] = value; }
        }

        #endregion // Properties
    }
}
