// ----------------------------------------------------------------
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemVendorRow : DataRow {
		private ItemVendorTable table;

		internal ItemVendorRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemVendorTable)this.Table;
		}
		#region Public Properties

		/// <summary>
		/// Public Property VendorItemID
		/// </summary>
		/// 		
		public System.Int32 ItemDetailID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.ItemDetailID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.ItemDetailID] = value; }
		}

		public System.String VendorItemID
		{
			get { 
				try { 
					return (System.String)this[this.table.VendorItemID];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.VendorItemID] = value; }
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
		/// Public Property VendorID
		/// </summary>
		public System.Int32 VendorID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.VendorID];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.VendorID] = value; }
		}
		/// <summary>
		/// Public Property VendorCode
		/// </summary>
		public System.String VendorCode
		{
			get { 
				try { 
					return (System.String)this[this.table.VendorCode];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.VendorCode] = value; }
		}
		/// <summary>
		/// Public Property VendorName
		/// </summary>
		public System.String VendorName
		{
			get { 
				try { 
					return (System.String)this[this.table.VendorName];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.VendorName] = value; }
		}
		/// <summary>
		/// Public Property VenorCostPrice
		/// </summary>
		public System.Decimal VenorCostPrice
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.VenorCostPrice];
				}
					catch{ 
						return System.Decimal.MinValue ; 
				}
			} 
			set { this[this.table.VenorCostPrice] = value; }
		}
		/// <summary>
		/// Public Property LastOrderDate
		/// </summary>
		public System.DateTime LastOrderDate
		{
			get { 
				try { 
					return (System.DateTime)this[this.table.LastOrderDate];
				}
					catch{ 
						return Convert.ToDateTime("1/1/1753 12:00:00 AM"); 
				}
			} 
			set { this[this.table.LastOrderDate] = value; }
		}

        //Properties Added by  SRT(Abhishek) Date : 01/12/2009 

        public System.Decimal AverageWholeSalePrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.AverageWholeSalePrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.AverageWholeSalePrice] = value; }
        }


        public System.Decimal CatalogPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.CatalogPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.CatalogPrice] = value; }
        }

        public System.Decimal ContractPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.ContractPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.ContractPrice] = value; }
        }

        public System.Decimal DealerAdjustedPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.DealerAdjustedPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.DealerAdjustedPrice] = value; }
        }

        public System.Decimal FedrelUpperLimitPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.FedrelUpperLimitPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.FedrelUpperLimitPrice] = value; }
        }

        public System.Decimal ManufacturerSuggPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.ManufacturerSuggPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.ManufacturerSuggPrice] = value; }
        }

        public System.Decimal NetItemPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.NetItemPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.NetItemPrice] = value; }
        }

        public System.Decimal ProducersPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.ProducersPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.ProducersPrice] = value; }
        }

        public System.Decimal RetailPrice
        {
            get
            {                
                try
                {
                    return (System.Decimal)this[this.table.RetailPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.RetailPrice] = value; }
        }


        //Add more Prices specific to Vendor 
        //Added by SRT(Abhishek) Date : 22/09/2009

         // INV - Invoice Billing Price
         // BCH - Base Charge         
         // PBQ	- Unit Price Beginning Quantity
         // RES	- Resale

        public System.Decimal InVoiceBillingPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.InVoiceBillingPrice];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.InVoiceBillingPrice] = value; }
        }

        public System.Decimal BaseCharge
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.BaseCharge];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.BaseCharge] = value; }
        }

        public System.Decimal UnitPriceBegQuantity
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.UnitPriceBegQuantity];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.UnitPriceBegQuantity] = value; }
        }

        //Added by ATul Joshi on 22-10-2010
        public System.Decimal UnitCostPrice
        {
            get
            {
                Decimal dd = Decimal.MinValue;
                Decimal.TryParse(Convert.ToString(this[this.table.UnitCostPrice]), out dd);
                return dd;
            }
            set { this[this.table.UnitCostPrice] = value; }
        }
        public System.Decimal VendorSalePrice
        {
            get
            {
                try
                {
                    Decimal dd = Decimal.MinValue;
                    Decimal.TryParse(Convert.ToString(this[this.table.VendorSalePrice]), out dd);
                    return dd;
                }
                catch
                {
                    return Decimal.MinValue;
                }
            }
            set { this[this.table.VendorSalePrice] = value; }
        }
        public System.String HammacherDeptClass
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.HammacherDeptClass];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.HammacherDeptClass] = value; }
        }
        //For pramotional Pricing
        public System.DateTime SaleStartDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.SaleStartDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.SaleStartDate] = value; }
        }

        public System.DateTime SaleEndDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.SaleEndDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.SaleEndDate] = value; }
        }
        //For pramotional Pricing
        public System.Decimal Resale
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.Resale];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.Resale] = value; }
        }

        //End Of Added by SRT(Abhishek) Date : 22/09/2009

        public System.Boolean IsDeleted
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsDeleted];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsDeleted] = value; }
        }
        //End Of Properties Added by  SRT(Abhishek) Date : 01/12/2009

        //Added by SRT(Abhsihek) to handle newly added coulomn of Packets
        public System.String PckSize
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PckSize];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.PckSize] = value; }
        }

        public System.String PckQty
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PckQty];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.PckQty] = value; }
        }

        public System.String PckUnit
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PckUnit];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.PckUnit] = value; }
        }
		#endregion // Properties
	}
	public class ItemVendorRowChangeEvent : EventArgs {
		private ItemVendorRow eventRow;  
		private DataRowAction eventAction;

		public ItemVendorRowChangeEvent(ItemVendorRow row, DataRowAction action) {
			this.eventRow = row;
			this.eventAction = action;
		}

		public ItemVendorRow Row {
			get { return this.eventRow; }
		}

	public DataRowAction Action { 
		get { return this.eventAction; }
	}
}
}
