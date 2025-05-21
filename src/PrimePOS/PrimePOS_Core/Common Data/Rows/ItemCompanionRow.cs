// ----------------------------------------------------------------
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemCompanionRow : DataRow {
		private ItemCompanionTable table;

		internal ItemCompanionRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemCompanionTable)this.Table;
		}
		#region Public Properties

		public System.String CompanionItemID
		{
			get { 
				try { 
					return (System.String)this[this.table.CompanionItemID];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.CompanionItemID] = value; }
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

		public System.String ItemDescription
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.ItemDescription];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.ItemDescription] = value; }
		}

		/// <summary>
		/// Public Property Amount
		/// </summary>
		public System.Decimal Amount
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.Amount];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.Amount] = value; }
		}
		/// <summary>
		/// Public Property Percentage
		/// </summary>
		public System.Decimal Percentage
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.Percentage];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.Percentage] = value; }
		}

		#endregion // Properties
	}
	public class ItemCompanionRowChangeEvent : EventArgs {
		private ItemCompanionRow eventRow;  
		private DataRowAction eventAction;

		public ItemCompanionRowChangeEvent(ItemCompanionRow row, DataRowAction action) {
			this.eventRow = row;
			this.eventAction = action;
		}

		public ItemCompanionRow Row {
			get { return this.eventRow; }
		}

	public DataRowAction Action { 
		get { return this.eventAction; }
	}
}
}
