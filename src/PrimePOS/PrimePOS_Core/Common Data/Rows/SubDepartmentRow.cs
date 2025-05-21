// ----------------------------------------------------------------
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class SubDepartmentRow : DataRow {
		private SubDepartmentTable table;

		internal SubDepartmentRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (SubDepartmentTable)this.Table;
		}
		#region Public Properties

		/// <summary>
		/// Public Property VendorItemID
		/// </summary>
		/// 		
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
					return 0 ; 
				}
			} 
			set { this[this.table.SubDepartmentID] = value; }
		}

		/// <summary>
		/// Public Property DepartmentID
		/// </summary>
		public System.Int32 DepartmentID
		{
			get { 
				try { 
					return Configuration.convertNullToInt(this[this.table.DepartmentID]);
				}
					catch{ 
						return System.Int32.MinValue ; 
				}
			} 
			set { this[this.table.DepartmentID] = value; }
		}
		/// <summary>
		/// Public Property Description
		/// </summary>
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
                    return System.String.Empty;
                }
            }
            set { this[this.table.Description] = value; }
        }

         /// <summary>
        /// Public Property IncludeOnSale
		/// </summary>
		public System.Boolean IncludeOnSale
		{
			get { 
				try { 
					return (System.Boolean)this[this.table.IncludeOnSale];
				}
					catch{ 
						return false ; 
				}
			}
            set { this[this.table.IncludeOnSale] = value; }
		}

        //Sprint-18 - 2041 27-Oct-2014 JY  Added
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
		#endregion // Properties
	}

	public class SubDepartmentRowChangeEvent : EventArgs {
		private SubDepartmentRow eventRow;  
		private DataRowAction eventAction;

		public SubDepartmentRowChangeEvent(SubDepartmentRow row, DataRowAction action) {
			this.eventRow = row;
			this.eventAction = action;
		}

		public SubDepartmentRow Row {
			get { return this.eventRow; }
		}

	public DataRowAction Action { 
		get { return this.eventAction; }
	}
}
}
