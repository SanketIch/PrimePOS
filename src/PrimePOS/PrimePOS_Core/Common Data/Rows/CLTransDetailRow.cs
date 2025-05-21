//----------------------------------------------------------------------------------------------------
//Sprint-18 - 2090 07-Oct-2014 JY Added row class for CL_TransDetail table
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class CLTransDetailRow : DataRow 
    {
        private CLTransDetailTable table;

        internal CLTransDetailRow(DataRowBuilder rb) : base(rb) 
		{
            this.table = (CLTransDetailTable)this.Table;
		}
		#region Public Properties

		public System.Int64 ID
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

        public System.Int32 TransID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransID] = value; }
        }

        public System.Int64 CardID
        {
            get
            {
                try
                {
                    return Configuration.convertNullToInt64(this[this.table.CardID]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CardID] = value; }
        }

        public System.Decimal CurrentPoints
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.CurrentPoints];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.CurrentPoints] = value; }
        }

        public System.Decimal PointsAcquired
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.PointsAcquired];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.PointsAcquired] = value; }
        }

        public System.Decimal RunningTotalPoints
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.RunningTotalPoints];
                }
                catch
                {
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.RunningTotalPoints] = value; }
        }

        public System.String ActionType
        {
            get
            {
                return this[this.table.ActionType].ToString();
            }
            set { this[this.table.ActionType] = value; }
        }

        public System.DateTime TransDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.TransDate];
                }
                catch
                {
                    return Convert.ToDateTime("1/1/1753 12:00:00"); //return System.DateTime.MinValue; 
                }
            }
            set { this[this.table.TransDate] = value; }
        }
		#endregion 
    }
}
