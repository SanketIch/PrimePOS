
   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemMonitorCategoryDetailRow : DataRow {
		private ItemMonitorCategoryDetailTable table;

		internal ItemMonitorCategoryDetailRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (ItemMonitorCategoryDetailTable)this.Table;
		}
		
        #region Public Properties

		public System.Int32 ID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.ID];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.ID] = value; }
		}

        public System.Int32 ItemMonCatID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ItemMonCatID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ItemMonCatID] = value; }
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
                    return System.String.Empty;
                }
            }
            set { this[this.table.ItemID] = value; }
        }

        public System.Decimal UnitsPerPackage
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.UnitsPerPackage];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.UnitsPerPackage] = value; }
        }

        //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        public System.Boolean ePSE
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.ePSE.ToString()];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.ePSE] = value; }
        }

		#endregion 
	}
}
