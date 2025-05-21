
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemComboPricingDetailRow : DataRow 
	{
		private ItemComboPricingDetailTable table;

		internal ItemComboPricingDetailRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemComboPricingDetailTable)this.Table;
		}
		#region Public Properties

		public System.Int32 Id
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.Id];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.Id] = value; }
		}

		public System.Int32 ItemComboPricingId
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.ItemComboPricingId];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.ItemComboPricingId] = value; }
		}

		public System.Int32 QTY
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.QTY];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.QTY] = value; }
		}

		public System.Decimal SalePrice
		{
			get 
			{ 
				try 
				{ 
					return (System.Decimal)this[this.table.SalePrice];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.SalePrice] = value; }
		}

		public System.String ItemID
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.ItemID];
				}
				catch
				{ 
					return ""; 
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
					return ""; 
				}
			} 
			set { this[this.table.ItemDescription] = value; }
		}

#endregion 
	}
}
