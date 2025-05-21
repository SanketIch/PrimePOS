
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
    using System.Data.SqlTypes;

	public class TimesheetRow : DataRow 
	{
		private TimesheetTable table;

		internal TimesheetRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (TimesheetTable)this.Table;
		}
		#region Public Properties

		public System.Int64 ID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int64)this[this.table.ID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.ID] = value; }
		}

        public System.String UserID
        {
            get
            {
               return (System.String)this[this.table.UserID];
            }
            set { this[this.table.UserID] = value; }
        }

		public System.DateTime? TimeIn
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime?)this[this.table.TimeIn];
				}
				catch
				{
                    return Resources.Configuration.MinimumDate;
				}
			} 
			set { this[this.table.TimeIn] = value; }
		}

        public SqlDateTime TimeOut
        {
            get
            {
                try
                {
                    return (SqlDateTime)this[this.table.TimeOut];
                }
                catch
                {
                    return Resources.Configuration.MinimumDate;
                }
            }
            set {
                //if (value == null)
                //{
                //    this[this.table.TimeOut] =System.Data.SqlTypes.SqlDateTime.Null;
                //}
                //else
                //{
                    this[this.table.TimeOut] = value;
                //}
            }
        }

		public System.Boolean IsManualTimeIn
		{
			get 
			{ 
				try 
				{ 
					return (System.Boolean)this[this.table.IsManualIn];
				}
				catch
				{
                    return false; 
				}
			} 
			set { this[this.table.IsManualIn] = value; }
		}

        public System.Boolean IsTimeIn
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsTimeIn];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsTimeIn] = value; }
        }

        public System.Boolean IsTimeOut
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsTimeOut];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsTimeOut] = value; }
        }
        public System.Boolean IsManualTimeOut
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsManualOut];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsManualOut] = value; }
        }

		public System.String LastUpdatedBy
		{
			get 
			{ 
				return (System.String)this[this.table.LastUpdatedBy];
			} 
			set { this[this.table.LastUpdatedBy] = value; }
		}

		public System.DateTime LastUpdatedOn
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.LastUpdatedOn];
				}
				catch
				{
                    return Resources.Configuration.MinimumDate;
				}
			} 
			set { this[this.table.LastUpdatedOn] = value; }
		}

		#endregion 
	}
}
