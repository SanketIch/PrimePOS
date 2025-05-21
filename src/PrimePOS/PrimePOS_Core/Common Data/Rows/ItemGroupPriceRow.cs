// ----------------------------------------------------------------
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemGroupPriceRow : DataRow {
		private ItemGroupPriceTable table;

		internal ItemGroupPriceRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemGroupPriceTable)this.Table;
		}
		#region Public Properties

		/// <summary>
		/// Public Property GroupPriceID
		/// </summary>
		public System.Int32 GroupPriceID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.GroupPriceID];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.GroupPriceID] = value; }
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
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.ItemID] = value; }
		}
		/// <summary>
		/// Public Property Qty
		/// </summary>
		public System.Int32 Qty
		{
			get { 
				try { 
					return (System.Int32)this[this.table.Qty];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.Qty] = value; }
		}
		/// <summary>
		/// Public Property Cost
		/// </summary>
		public System.Decimal Cost
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.Cost];
				}
					catch{ 
						return System.Decimal.MinValue ; 
				}
			} 
			set { this[this.table.Cost] = value; }
		}
		/// <summary>
		/// Public Property SalePrice
		/// </summary>
		public System.Decimal SalePrice
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.SalePrice];
				}
					catch{ 
						return System.Decimal.MinValue ; 
				}
			} 
			set { this[this.table.SalePrice] = value; }
		}

        public System.DateTime? StartDate
        {
            get {
                DateTime parsedDate;
                if (DateTime.TryParse(this[this.table.StartDate].ToString(), out parsedDate) == false)
                {
                    return null;
                }
                else
                {
                    return parsedDate;
                }
            }
            set { this[this.table.StartDate] = value; }
        }

        public System.DateTime? EndDate
        {
            get
            {
                DateTime parsedDate;
                if (DateTime.TryParse(this[this.table.EndDate].ToString(), out parsedDate) == false)
                {
                    return null;
                }
                else
                {
                    return parsedDate;
                } 
            }
            set { this[this.table.EndDate] = value; }
        }

		#endregion // Properties
	}
	public class ItemGroupPriceRowChangeEvent : EventArgs {
		private ItemGroupPriceRow eventRow;  
		private DataRowAction eventAction;

		public ItemGroupPriceRowChangeEvent(ItemGroupPriceRow row, DataRowAction action) {
			this.eventRow = row;
			this.eventAction = action;
		}

		public ItemGroupPriceRow Row {
			get { return this.eventRow; }
		}

	public DataRowAction Action { 
		get { return this.eventAction; }
	}
}
}
