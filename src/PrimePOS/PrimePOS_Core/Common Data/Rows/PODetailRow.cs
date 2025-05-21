
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class PODetailRow : DataRow 
	{
		private PODetailTable table;

		internal PODetailRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (PODetailTable)this.Table;
		}
		#region Public Properties

		public System.Int32 PODetailID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.PODetailID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.PODetailID] = value; }
		}

		public System.Int32 OrderID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.OrderID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.OrderID] = value; }
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

		public System.Int32 AckQTY
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.AckQTY];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.AckQTY] = value; }
		}
        public System.String BestVendor
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.BestVendor];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.BestVendor] = value; }
        }
        public System.String BestPrice
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.BestVendorPrice];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.BestVendorPrice] = value; }
        }
		public System.Decimal Cost
		{
			get 
			{ 
				try 
				{ 
					return (System.Decimal)this[this.table.Cost];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.Cost] = value; }
		}
        public System.Decimal LastCostPrice //Added by Ravindra PRIMEPOS-1043
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.LastCostPrice];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LastCostPrice] = value; }
        }
        public System.Decimal Price
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.Price];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.Price] = value; }
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

		public System.String Comments
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.Comments];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.Comments] = value; }
		}
		public System.String AckStatus
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.AckStatus];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.AckStatus] = value; }
		}

		public System.String VendorItemCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.VendorItemCode];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.VendorItemCode] = value; }
		}
        public System.Int32 VendorID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.VendorID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.VendorID] = value; }
        }
        public System.String VendorName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.VendorName];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.VendorName] = value; }
        }
        public System.String ChangedProductQualifier
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ChangedProductQualifier];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ChangedProductQualifier] = value; }
        }

        public System.String ChangedProductID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ChangedProductID];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ChangedProductID] = value; }
        }
        //Added By SRT(Gaurav) Date: 03-Jul-2009
        //Added Extra columns to show Item Reorder Report.
        public System.Int32 QtyInStock
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.QtyInStock];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.QtyInStock] = value; }
        }

        public System.Int32 ReOrderLevel
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ReorderLevel];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ReorderLevel] = value; }
        }

        public System.Int32 QtySold100Days
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.QtySold100Days];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.QtySold100Days] = value; }
        }
        //End Of Added By SRT(Gaurav)

        public System.String PacketSize
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PacketSize];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.PacketSize] = value; }
        }

        public System.String Packetunit
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Packetunit];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.Packetunit] = value; }
        }

        public System.String PacketQuant
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PacketQuant];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.PacketQuant] = value; }
        }
        public System.Int32 SoldItems
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.SoldItems];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.SoldItems] = value; }
        }
        //Added by atul 22-oct-2010
        public System.String ItemDescType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ItemDescType];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ItemDescType] = value; }
        }

        public System.String Idescription
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Idescription];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.Idescription] = value; }
        }
        //end of Added by atul 22-oct-2010
		#endregion 

        //Added By Amit Date: 05-Jul-2011
        //Added Extra columns to show Item Reorder Report.
        public System.String DeptName
        {
            get
            {
                try
                {
                    return (System.String )this[this.table.DeptName];
                }
                catch
                {
                    return System.String.Empty; ;
                }
            }
            set { this[this.table.DeptName] = value; }
        }

        public System.String SubDeptName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SubDeptName];
                }
                catch
                {
                    return System.String.Empty; ;
                }
            }
            set { this[this.table.SubDeptName] = value; }
        }

        public System.Int32 MinOrdQty
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.MinOrdQty];
                }
                catch
                {
                    return 0; ;
                }
            }
            set { this[this.table.MinOrdQty] = value; }
        }

        public System.Int32 QtyOnOrder
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.QtyOnOrder];
                }
                catch
                {
                    return 0; ;
                }
            }
            set { this[this.table.QtyOnOrder] = value; }
        }
        //End
        //Added by Amit Date 27 July 2011
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
                    return 0;
                }
            }
            set { this[this.table.RetailPrice] = value; }
        }

        public System.Decimal ItemPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.ItemPrice];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ItemPrice] = value; }
        }
        public System.Decimal Discount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.Discount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.Discount] = value; }
        }
        //End
        //Added By Amit Date 29 Nov 2011
        public System.Object InvRecDate
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.InvRecDate];
                }
                catch
                {
                    return null ;
                }
            }
            set { this[this.table.InvRecDate] = value; }
        }
        //End       
        //Added by Ravindra to Save Processed Qty 3 April 2013

        public System.Int32 ProcessedQTY
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ProcessedQty];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ProcessedQty] = value; }
        }

	}
}
