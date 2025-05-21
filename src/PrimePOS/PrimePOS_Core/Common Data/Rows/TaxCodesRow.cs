
   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TaxCodesRow : DataRow {
		private TaxCodesTable table;

		internal TaxCodesRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (TaxCodesTable)this.Table;
		}
		#region Public Properties

		public System.Int32 TaxID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.TaxID];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.TaxID] = value; }
		}

		public System.String TaxCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.TaxCode];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.TaxCode] = value; }
		}

		public System.String Description
		{
			get { 
				try { 
					return (System.String)this[this.table.Description];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.Description] = value; }
		}
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
		public System.String UserID
		{
			get { 
				try { 
					return (System.String)this[this.table.UserID];
				}
					catch{ 
						return System.String.Empty ; 
				}
			} 
			set { this[this.table.UserID] = value; }
		}

        /// <summary>
        /// Added By Shitaljit to store tax type.
        /// </summary>
        public System.Int32 TaxType
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TaxType];
                }
                catch
                {
                    return -1;
                }
            }
            set { this[this.table.TaxType] = value; }
        }
		public System.Boolean Active//2974
		{
			get
			{
				try
				{
					return (System.Boolean)this[this.table.Active];
				}
				catch
				{
					return false;
				}
			}
			set { this[this.table.Active] = value; }
		}
		#endregion
	}
}
