
namespace POS_Core.CommonData.Rows 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class CLPointsAdjustmentLogRow : DataRow 
	{
		private CLPointsAdjustmentLogTable table;

		internal CLPointsAdjustmentLogRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (CLPointsAdjustmentLogTable)this.Table;
		}
		#region Public Properties

		public System.Int32 ID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.ID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.ID] = value; }
		}

		public System.DateTime CreatedOn
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.CreatedOn];
				}
				catch
				{
                    return Convert.ToDateTime("1/1/1753 12:00:00"); //return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.CreatedOn] = value; }
		}

		public System.Int64 CLCardID
		{
			get 
			{ 
				try 
				{ 
					return Configuration.convertNullToInt64( this[this.table.CLCardID]);
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.CLCardID] = value; }
		}

        public System.String CreatedBy
        {
            get
            {
                    return this[this.table.CreatedBy].ToString();
            }
            set { this[this.table.CreatedBy] = value; }
        }

        public System.Decimal OldPoints
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.OldPoints]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.OldPoints] = value; }
        }

        public System.Decimal NewPoints
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.NewPoints]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.NewPoints] = value; }
        }

        public System.String Remarks
        {
            get
            {
                try
                {
                    return Configuration.convertNullToString(this[this.table.Remarks]);
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { this[this.table.Remarks] = value; }
        }
        
		#endregion 
	}
}
