
namespace POS_Core.CommonData.Rows 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class DepartmentRow : DataRow 
	{
		private DepartmentTable table;

		internal DepartmentRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (DepartmentTable)this.Table;
		}
		#region Public Properties

		public System.Int32 DeptID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.DeptId];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.DeptId] = value; }
		}

		public System.String DeptCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.DeptCode];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.DeptCode] = value; }
		}

		public System.String DeptName
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.DeptName];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.DeptName] = value; }
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

		public System.Boolean IsTaxable
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.IsTaxable];
				}
				catch
				{ 
					return false ; 
				}
			} 
			set { this[this.table.IsTaxable] = value; }
		}

		public System.Decimal Discount
		{
			get 
			{ 
				try 
				{ 
					return Configuration.convertNullToDecimal(this[this.table.Discount]);
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.Discount] = value; }
		}

        public System.Decimal SaleDiscount
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.SaleDiscount]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.SaleDiscount] = value; }
        }

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
                    return Convert.ToDateTime("1/1/1753 12:00:00"); //return System.DateTime.MinValue; 
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
                    return Convert.ToDateTime("1/1/1753 12:00:00");//return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.SaleEndDate] = value; }
		}

		public System.Int32 TaxId
		{
			get 
			{ 
				try 
				{ 
					return Configuration.convertNullToInt( this[this.table.TaxId]);
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.TaxId] = value; }
		}

		public System.String TaxCode
		{
			get 
			{ 
				try 
				{ 
					return Configuration.convertNullToString( this[this.table.TaxCode]);
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.TaxCode] = value; }
		}

		public System.String TaxDescription
		{
			get 
			{ 
				try 
				{ 
					return  Configuration.convertNullToString(this[this.table.TaxDescription]);
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.TaxDescription] = value; }
		}

        #region Sprint-18 - 2041 22-Oct-2014 JY Added PointsPerDollar
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
        #endregion

        #endregion
    }
}
