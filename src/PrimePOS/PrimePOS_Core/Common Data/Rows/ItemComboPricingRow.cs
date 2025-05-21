
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemComboPricingRow : DataRow 
	{
		private ItemComboPricingTable table;

		internal ItemComboPricingRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemComboPricingTable)this.Table;
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

		public System.Boolean ForceGroupPricing
		{
			get 
			{ 
				try 
				{
                    return (System.Boolean)this[this.table.ForceGroupPricing];
				}
				catch
				{ 
					return false; 
				}
			}
            set { this[this.table.ForceGroupPricing] = value; }
		}

		public System.Decimal ComboItemPrice
		{
			get 
			{ 
				try 
				{
                    return (System.Decimal)this[this.table.ComboItemPrice];
				}
				catch
				{ 
					return 0; 
				}
			}
            set { this[this.table.ComboItemPrice] = value; }
		}

        public System.Int32 MinComboItems
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.MinComboItems];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.MinComboItems] = value; }
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

        #region Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added
        public System.Int32 MaxComboItems
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.MaxComboItems];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.MaxComboItems] = value; }
        }
        #endregion
        #endregion
    }
}
