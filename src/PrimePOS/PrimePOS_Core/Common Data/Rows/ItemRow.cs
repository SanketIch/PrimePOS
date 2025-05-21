// ----------------------------------------------------------------
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class ItemRow : DataRow {
		private ItemTable table;

		internal ItemRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemTable)this.Table;
		}
		#region Public Properties

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
		/// Public Property DepartmentID
		/// </summary>
        public System.Int32 DepartmentID
        {
            get
            {
                try
                {
                    if ((System.Int32)this[this.table.DepartmentID] > 0)
                    {
                        return (System.Int32)this[this.table.DepartmentID];
                    }
                    else
                    {
                        return Configuration.CInfo.DefaultDeptId;
                    }
                }
                catch
                {
                    return System.Int32.MinValue;
                }
            }
            set
            {
                if (value > 0)
                {
                    this[this.table.DepartmentID] = value;
                }
                else
                {
                    this[this.table.DepartmentID] = Configuration.CInfo.DefaultDeptId;
                }
            }
        }

        public System.Int32 SubDepartmentID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.SubDepartmentID];
                    
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.SubDepartmentID] = value;
            }
        }

		/// <summary>
		/// Public Property Description
		/// </summary>
		public System.String Description
		{
			get { 
				try { 
					return Configuration.convertNullToString( this[this.table.Description]);
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Description] = value; }
		}
		/// <summary>
		/// Public Property Itemtype
		/// </summary>
		public System.String Itemtype
		{
			get { 
				try {

                    if (this[this.table.Itemtype] == null)
                    {
                        return System.String.Empty; 
                    }
                    else
                    {
                        return this[this.table.Itemtype].ToString();
                    }
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Itemtype] = value; }
		}
		/// <summary>
		/// Public Property ProductCode
		/// </summary>
		public System.String ProductCode
		{
			get { 
				try { 
					return this[this.table.ProductCode].ToString();
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.ProductCode] = value; }
		}
		/// <summary>
		/// Public Property SaleTypeCode
		/// </summary>
		public System.String SaleTypeCode
		{
			get { 
				try { 
					return this[this.table.SaleTypeCode].ToString();
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.SaleTypeCode] = value; }
		}
		/// <summary>
		/// Public Property SeasonCode
		/// </summary>
		public System.String SeasonCode
		{
			get { 
				try { 
					return this[this.table.SeasonCode].ToString();
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.SeasonCode] = value; }
		}
		/// <summary>
		/// Public Property Unit
		/// </summary>
		public System.String Unit
		{
			get { 
				try { 
					return this[this.table.Unit].ToString();
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Unit] = value; }
		}
		/// <summary>
		/// Public Property Freight
		/// </summary>
		public System.Decimal Freight
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.Freight];
				}
					catch{ 
						return System.Decimal.MinValue ; 
				}
			} 
			set { this[this.table.Freight] = value; }
		}
		/// <summary>
		/// Public Property SellingPrice
		/// </summary>
		public System.Decimal SellingPrice
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.SellingPrice];
				}
					catch{ 
						return System.Decimal.MinValue ; 
				}
			} 
			set { this[this.table.SellingPrice] = value; }
		}
		/// <summary>
		/// Public Property AvgPrice
		/// </summary>
		public System.Decimal AvgPrice
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.AvgPrice];
				}
					catch{ 
						return System.Decimal.MinValue ; 
				}
			} 
			set { this[this.table.AvgPrice] = value; }
		}
		/// <summary>
		/// Public Property LastCostPrice
		/// </summary>
		public System.Decimal LastCostPrice
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.LastCostPrice];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.LastCostPrice] = value; }
		}
		/// <summary>
		/// Public Property isTaxable
		/// </summary>
		public System.Boolean isTaxable
		{
			get { 
				try { 
					return (System.Boolean)this[this.table.isTaxable];
				}
					catch{ 
						return false ; 
				}
			} 
			set { this[this.table.isTaxable] = value; }
		}
		/// <summary>
		/// Public Property TaxID
		/// </summary>
		public System.Int32 TaxID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.TaxID];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.TaxID] = value; }
		}
		/// <summary>
		/// Public Property isOnSale
		/// </summary>
		/// 

		
		public System.Boolean isOnSale
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.isOnSale];
				}
				catch
				{ 
					return false ; 
				}
			} 
			set { this[this.table.isOnSale] = value; }
		}

		public System.Boolean isDiscountable
		{
			get { 
				try { 
					return (System.Boolean)this[this.table.isDiscountable];
				}
					catch{ 
						return false ; 
				}
			} 
			set { this[this.table.isDiscountable] = value; }
		}
		/// <summary>
		/// Public Property Discount
		/// </summary>
		public System.Decimal Discount
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.Discount];
				}
					catch{ 
						return System.Decimal.MinValue ; 
				}
			} 
			set { this[this.table.Discount] = value; }
		}
		/// <summary>
		/// Public Property SaleStartDate
		/// </summary>
		public System.Object SaleStartDate
		{
			get { 
				try { 
					return (System.Object)this[this.table.SaleStartDate];
				}
					catch{ 
						return DBNull.Value; 
				}
			} 
			set { this[this.table.SaleStartDate] = value; }
		}
		/// <summary>
		/// Public Property SaleEndDate
		/// </summary>
		public System.Object SaleEndDate
		{
			get { 
				try { 
					return (System.Object)this[this.table.SaleEndDate];
				}
					catch{ 
						return DBNull.Value; 
				}
			} 
			set { this[this.table.SaleEndDate] = value; }
		}
		/// <summary>
		/// Public Property OnSalePrice
		/// </summary>
		public System.Decimal OnSalePrice
		{
			get { 
				try { 
					return (System.Decimal)this[this.table.OnSalePrice];
				}
					catch{ 
						return 0; 
				}
			} 
			set { this[this.table.OnSalePrice] = value; }
		}
		/// <summary>
		/// Public Property QtyInStock
		/// </summary>
		public System.Int32 QtyInStock
		{
			get { 
				try { 
					return (System.Int32)this[this.table.QtyInStock];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.QtyInStock] = value; }
		}
		/// <summary>
		/// Public Property Location
		/// </summary>
		public System.String Location
		{
			get { 
				try { 
					return this[this.table.Location].ToString();
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Location] = value; }
		}
		/// <summary>
		/// Public Property MinOrdQty
		/// </summary>
		public System.Int32 MinOrdQty
		{
			get { 
				try { 
					return (System.Int32)this[this.table.MinOrdQty];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.MinOrdQty] = value; }
		}
		/// <summary>
		/// Public Property ReOrderLevel
		/// </summary>
		public System.Int32 ReOrderLevel
		{
			get { 
				try { 
					return (System.Int32)this[this.table.ReOrderLevel];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.ReOrderLevel] = value; }
		}
		/// <summary>
		/// Public Property QtyOnOrder
		/// </summary>
		public System.Int32 QtyOnOrder
		{
			get { 
				try { 
					return (System.Int32)this[this.table.QtyOnOrder];
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.QtyOnOrder] = value; }
		}
		/// <summary>
		/// Public Property ExptDeliveryDate
		/// </summary>
		public System.Object ExptDeliveryDate
		{
			get { 
				try { 
					return (System.Object)this[this.table.ExptDeliveryDate];
				}
					catch{ 
						return null; 
				}
			} 
			set { this[this.table.ExptDeliveryDate] = value; }
		}

        //Following code Added by Krishna on 5 October 2011
        /// <summary>
        /// Public Property Expiration Date
        /// </summary>
        //public System.Object ExpDate
        //{
        //    get
        //    {
        //        try
        //        {
        //            return (System.Object)this[this.table.ExpDate];
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    set { this[this.table.ExpDate] = value; }
        //}

        /// <summary>
        /// Public Property LotNumber
        /// </summary>
        //public System.String LotNumber
        //{
        //    get
        //    {
        //        try
        //        {
        //            return (System.String)this[this.table.LotNumber];
        //        }
        //        catch
        //        {
        //            return System.String.Empty;
        //        }
        //    }
        //    set { this[this.table.LotNumber] = value; }
        //}
        //Till here Added byb Krishna on 5 October 2011

        #region Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
        /// <summary>
        /// Public Property Expiration Date
        /// </summary>
        public System.Object ExpDate
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.ExpDate];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.ExpDate] = value; }
        }
        #endregion

        /// <summary>
		/// Public Property LastVendor
		/// </summary>
		public System.String LastVendor
		{
			get { 
				try { 
					return (System.String)this[this.table.LastVendor];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.LastVendor] = value; }
		}
        /// <summary>
        /// Public Property PreferredVendor
        /// </summary>
        public System.String PreferredVendor
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PreferredVendor];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.PreferredVendor] = value; }
        }
		/// <summary>
		/// Public Property LastRecievDate
		/// </summary>
		public System.DateTime LastRecievDate
		{
			get { 
				try { 
					return (System.DateTime)this[this.table.LastRecievDate];
				}
					catch{ 
						return System.DateTime.MinValue ; 
				}
			} 
			set { this[this.table.LastRecievDate] = value; }
		}
		/// <summary>
		/// Public Property LastSellingDate
		/// </summary>
		public System.DateTime LastSellingDate
		{
			get { 
				try { 
					return (System.DateTime)this[this.table.LastSellingDate];
				}
					catch{ 
						return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.LastSellingDate] = value; }
		}
		/// <summary>
		/// Public Property Remarks
		/// </summary>
		public System.String Remarks
		{
			get { 
				try { 
					return (System.String)this[this.table.Remarks];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Remarks] = value; }
		}

		public System.Boolean ExclFromAutoPO
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.ExclFromAutoPO];
				}
				catch
				{ 
					return true; 
				}
			} 
			set { this[this.table.ExclFromAutoPO] = value; }
		}
		public System.Boolean ExclFromRecpt
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.ExclFromRecpt];
				}
				catch
				{ 
					return true; 
				}
			} 
			set { this[this.table.ExclFromRecpt] = value; }
		}
		public System.Boolean isOTCItem
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.isOTCItem];
				}
				catch
				{ 
					return true; 
				}
			} 
			set { this[this.table.isOTCItem] = value; }
		}
        public System.Boolean UpdatePrice
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.UpdatePrice];
                }
                catch
                {
                    return true;
                }
            }
            set { this[this.table.UpdatePrice] = value; }
        }

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
        //Following try catch is added by shitaljit(QuicSolv) on 1 July 2011
        public System.Boolean IsEBTItem
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsEBTItem];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsEBTItem] = value; }
        }
        //Added By Shitaljit(QuicSolv) on 18 August 2011
        /// <summary>
        /// Public Property TaxPolicy
        /// </summary>
        public System.String TaxPolicy
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaxPolicy];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
              set { this[this.table.TaxPolicy] = value; }  
        }
        //Till here added by shitaljit.

        public System.String ManufacturerName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ManufacturerName];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ManufacturerName] = value; }
        }
        //Added by Ravindra for Sale Liit 22 MArch2013
        public System.Int32 SaleLimitQty
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.SaleLimitQty];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.SaleLimitQty] = value; }
        }

        //Added By Shitaljit(QuicSolv) on 3 April 2013
        /// <summary>
        /// Public Property DiscountPolicy
        /// </summary>
        public System.String DiscountPolicy
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DiscountPolicy];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.DiscountPolicy] = value; }
        }
        //Added By Shitaljit(QuicSolv) on 6Feb2014 for
        //PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
        public System.Int32 PointsPerDollar
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PointsPerDollar];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PointsPerDollar] = value; }
        }

        public System.Boolean IsDefaultCLPoint
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsDefaultCLPoint];
                }
                catch
                {
                    return true;
                }
            }
            set { this[this.table.IsDefaultCLPoint] = value; }
        }
        //END

        #region Sprint-18 - 2041 28-Oct-2014 JY  Added
        public System.String CLPointPolicy
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CLPointPolicy];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.CLPointPolicy] = value; }
        }
        #endregion

        #region Sprint-21 - 2173 06-Jul-2015 JY Added
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
                    return true;
                }
            }
            set { this[this.table.IsActive] = value; }
        }
        #endregion

        #region PRIMEPOS-2592 01-Nov-2018 JY Added 
        public System.Boolean IsNonRefundable
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsNonRefundable];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsNonRefundable] = value; }
        }
        #endregion

        #region Added for Solutran - PRIMEPOS-2663 - NileshJ

        public System.Int64 S3TransID //PRIMEPOS-3265
        {
            get
            {
                try
                {
                    return (System.Int64)this[this.table.S3TrasID]; //PRIMEPOS-3265
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3TrasID] = value; }
        }

        public System.Decimal S3PurAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3PurAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3PurAmount] = value; }
        }

        public System.Decimal S3TaxAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3TaxAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3TaxAmount] = value; }
        }

        public System.Decimal S3DiscountAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3DiscountAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3DiscountAmount] = value; }
        }

        #endregion

        #endregion // Properties
    }
    public class ItemRowChangeEvent : EventArgs {
		private ItemRow eventRow;  
		private DataRowAction eventAction;

		public ItemRowChangeEvent(ItemRow row, DataRowAction action) {
			this.eventRow = row;
			this.eventAction = action;
		}

		public ItemRow Row {
			get { return this.eventRow; }
		}

	public DataRowAction Action { 
		get { return this.eventAction; }
	}
}
}
