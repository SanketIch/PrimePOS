// ----------------------------------------------------------------
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemPriceValidationRow : DataRow {
		private ItemPriceValidationTable table;

		internal ItemPriceValidationRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemPriceValidationTable)this.Table;
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
                    return 0;
                }
            }
            set { this[this.table.DeptID] = value; }
        }

		public System.Int32 DeptID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.DeptID];
				}
					catch{ 
						return 0; 
				}
			} 
			set { this[this.table.DeptID] = value; }
		}
		/// <summary>
		/// Public Property ItemID
		/// </summary>
		public System.String ItemID
		{
			get { 
				try { 
					return (System.String)this[this.table.ItemID];
				}
					catch{ 
						return "" ; 
				}
			} 
			set { this[this.table.ItemID] = value; }
		}

		public System.Boolean AllowNegative
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.AllowNegative];
				}
				catch
				{ 
					return false; 
				}
			} 
			set { this[this.table.AllowNegative] = value; }
		}

		/// <summary>
		/// Public Property MinSellingAmount
		/// </summary>
		public System.Decimal MinSellingAmount
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.MinSellingAmount];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.MinSellingAmount] = value; }
		}
		/// <summary>
		/// Public Property MinSellingPercentage
		/// </summary>
		public System.Decimal MinSellingPercentage
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.MinSellingPercentage];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.MinSellingPercentage] = value; }
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

        public System.Decimal MinCostPercentage
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.MinCostPercentage];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.MinCostPercentage] = value; }
        }
		
#endregion // Properties

	}
	public class ItemPriceValidationRowChangeEvent : EventArgs {
		private ItemPriceValidationRow eventRow;  
		private DataRowAction eventAction;

		public ItemPriceValidationRowChangeEvent(ItemPriceValidationRow row, DataRowAction action) {
			this.eventRow = row;
			this.eventAction = action;
		}

		public ItemPriceValidationRow Row {
			get { return this.eventRow; }
		}

	public DataRowAction Action { 
		get { return this.eventAction; }
	}
}
}
