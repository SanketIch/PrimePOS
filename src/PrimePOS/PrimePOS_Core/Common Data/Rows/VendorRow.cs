// ----------------------------------------------------------------
// Library: Common Data

// Author: adeel shehzad.
// Company: d-p-s (www.d-p-s.com)
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	//     This class is used to define the shape of VendorRow.
	public class VendorRow : DataRow {
		private VendorTable table;

		// Constructor
		internal VendorRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (VendorTable)this.Table;
		}
		#region Public Properties

		// Public Property Vendorcode
		public System.Int32 VendorId
		{
			get { 
				try { 
					return (System.Int32)this[this.table.VendorId];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.VendorId] = value; }
		}
		public System.String Vendorcode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.Vendorcode];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.Vendorcode] = value; }
		}

		// Public Property Vendorname
		public System.String Vendorname
		{
			get { 
				try { 
					return (System.String)this[this.table.Vendorname];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Vendorname] = value; }
		}
		// Public Property Address1
		public System.String Address1
		{
			get { 
				try { 
					return (System.String)this[this.table.Address1];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Address1] = value; }
		}
		// Public Property Address2
		public System.String Address2
		{
			get { 
				try { 
					return (System.String)this[this.table.Address2];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Address2] = value; }
		}
		// Public Property City
		public System.String City
		{
			get { 
				try { 
					return (System.String)this[this.table.City];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.City] = value; }
		}
		// Public Property State
		public System.String State
		{
			get { 
				try { 
					return (System.String)this[this.table.State];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.State] = value; }
		}
		// Public Property Zip
		public System.String Zip
		{
			get { 
				try { 
					return (System.String)this[this.table.Zip];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Zip] = value; }
		}
		// Public Property Telephoneno
		public System.String Telephoneno
		{
			get { 
				try { 
					return (System.String)this[this.table.Telephoneno];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Telephoneno] = value; }
		}
		// Public Property Faxno
		public System.String Faxno
		{
			get { 
				try { 
					return (System.String)this[this.table.Faxno];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Faxno] = value; }
		}
		// Public Property Cellno
		public System.String Cellno
		{
			get { 
				try { 
					return (System.String)this[this.table.Cellno];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Cellno] = value; }
		}
		// Public Property Url
		public System.String Url
		{
			get { 
				try { 
					return (System.String)this[this.table.Url];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Url] = value; }
		}
		// Public Property Email
		public System.String Email
		{
			get { 
				try { 
					return (System.String)this[this.table.Email];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Email] = value; }
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
					return false ; 
				}
			} 
			set { this[this.table.IsActive] = value; }
		}
        //Added by Prashant(SRT) Date:1-06-2009
        public System.Boolean IsAutoClose
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsAutoClose];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsAutoClose] = value; }
        }
        //End of Added by Prashant(SRT) Date:1-06-2009

        //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //Coulomns Added for VendorInterface

        public System.String PrimePOVendorCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PrimePOVendorCode];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PrimePOVendorCode] = value; }
        }


        public System.String PriceQualifier
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PriceQualifier];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PriceQualifier] = value; }
        }

        public System.Boolean USEVICForEPO
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.USEVICForEPO];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.USEVICForEPO] = value; }
        }

        public System.String CostQualifier
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CostQualifier];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CostQualifier] = value; }
        }

        public System.String SalePriceQualifier
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SalePriceQualifier];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.SalePriceQualifier] = value; }
        }
        public System.String TimeToOrder
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TimeToOrder];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TimeToOrder] = value; }
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
                    return false;
                }
            }
            set { this[this.table.UpdatePrice] = value; }
        }

        public System.Boolean SendVendCostPrice
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.SendVendCostPrice];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.SendVendCostPrice] = value; }
        }
       //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        public System.Boolean Process810
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.Process810];
                }
                catch
                {
                    return true;
                }
            }
            set { this[this.table.Process810] = value; }
        }

        public System.Boolean AckPriceUpdate
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.AckPriceUpdate];
                }
                catch
                {
                    return true;
                }
            }
            set { this[this.table.AckPriceUpdate] = value; }
        }

        #region 12-Nov-2014 JY added new field IsSalePriceUpdate
        public System.Boolean SalePriceUpdate
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.SalePriceUpdate];
                }
                catch
                {
                    return true;
                }
            }
            set { this[this.table.SalePriceUpdate] = value; }
        }
        #endregion

        #region Sprint-21 - 2208 24-Jul-2015 JY Added new field 
        public System.Boolean ReduceSellingPrice
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.ReduceSellingPrice];
                }
                catch
                {
                    return true;
                }
            }
            set { this[this.table.ReduceSellingPrice] = value; }
        }
        #endregion

        #endregion // Properties
    }
}
