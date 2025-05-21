
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class POHeaderRow : DataRow 
	{
		private POHeaderTable table;

		internal POHeaderRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (POHeaderTable)this.Table;
		}
		#region Public Properties

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

		public System.String OrderNo
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.OrderNo];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.OrderNo] = value; }
		}


		public System.DateTime OrderDate
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.OrderDate];
				}
				catch
				{ 
					return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.OrderDate] = value; }
		}

		public System.DateTime ExptDelvDate
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.ExptDelvDate];
				}
				catch
				{ 
					return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.ExptDelvDate] = value; }
		}

		public System.Int32 VendorID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.VendorId];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.VendorId] = value; }
		}

		public System.String VendorCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.VendorCode];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.VendorCode] = value; }
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

		public System.Int32 Status
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.Status];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.Status] = value; }
		}
		
		public System.Int32 isFTPUsed
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.isFTPUsed];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.isFTPUsed] = value; }
		}

		public System.String AckType
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.AckType];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.AckType] = value; }
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

		public System.DateTime AckDate
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.AckDate];
				}
				catch
				{ 
					return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.AckDate] = value; }
		}

        //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //Coulomns Added for VendorInterface
             
        public System.Int64 PrimePOrderId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PrimePOrderId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PrimePOrderId] = value; }
        }

        //Added By SRT(Gaurav) Date : 04/07/2009
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
                    return String.Empty;
                }
            }
            set { this[this.table.Description] = value; }
        }
        //added by atul 22-oct-2010
        public System.DateTime InvoiceDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.InvoiceDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.InvoiceDate] = value; }
        }

        public System.String InvoiceNumber
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.InvoiceNumber];
                }
                catch
                {
                    return String.Empty;
                }
            }
            set { this[this.table.InvoiceNumber] = value; }
        }
        //End of added by atul 22-oct-2010
        public System.Boolean Flagged
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.Flagged];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.Flagged] = value; }
        }

        //End OF Added By SRT(Gaurav)
        //Added by Ravindra(Quicsolv) 16 Jan 2013
        public System.String RefOrderNO
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.RefOrderNo];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.RefOrderNo] = value; }
        }

        //End of Added by Ravindra(Quicsolv) 16 Jan 2013

        //Added By shitaljit to store file type on 17 May 13
        public System.String AckFileType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.AckFileType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.AckFileType] = value; }
        }

        //End

        #region Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added
        public System.String ProcessedBy
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProcessedBy];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ProcessedBy] = value; }
        }
        public System.String ProcessedType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProcessedType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ProcessedType] = value; }
        }
        #endregion

        #region PRIMEPOS-2901 05-Nov-2020 JY Added
        public System.String TransTypeCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransTypeCode];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TransTypeCode] = value; }
        }
        #endregion
        #endregion


    }
}
