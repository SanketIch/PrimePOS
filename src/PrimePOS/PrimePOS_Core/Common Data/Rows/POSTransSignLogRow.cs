
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
    /// <summary>
    /// Created by Shitaljit to capture sign log for POS Transactions.
    /// </summary>
    public class POSTransSignLogRow: DataRow 
	{
		private POSTransSignLogTable table;

		internal POSTransSignLogRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (POSTransSignLogTable)this.Table;
		}
		#region Public Properties

		public System.Int32 SignLogID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.SignLogID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.SignLogID] = value; }
		}

        public System.Int32 POSTransId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.POSTransId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.POSTransId] = value; }
        }

		public System.String SignContext
		{
			get 
			{ 
				try 
				{
                    return (System.String)this[this.table.SignContext];
				}
				catch
				{
                    return System.String.Empty; 
				}
			} 
			set { this[this.table.SignContext] = value; }
		}
		
        public System.String SignContextData
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SignContextData];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.SignContextData] = value; }
        }

        public System.Byte[] SignDataBinary
        {
            get
            {
                try
                {
                    return (System.Byte[])this[this.table.SignDataBinary];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.SignDataBinary] = value; }
        }


        public System.String SignDataText	
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SignDataText];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.SignDataText] = value; }
        }


		public System.String CustomerIDType
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.CustomerIDType];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.CustomerIDType] = value; }
		}

		public System.String CustomerIDDetail
		{
			get 
			{ 
				try 
				{
                    return (System.String)this[this.table.CustomerIDDetail];
				}
				catch
				{ 
					return  System.String.Empty ;   
				}
			} 
			set { this[this.table.CustomerIDDetail] = value; }
		}

        public System.Int32 TransDetailID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransDetailID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransDetailID] = value; }
        }

		#endregion 
	}
    
}
