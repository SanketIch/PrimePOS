
   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class PayTypeRow : DataRow {
		private PayTypeTable table;

		internal PayTypeRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (PayTypeTable)this.Table;
		}
		#region Public Properties

		public System.String PaytypeID
		{
            get
            {
                try
                {
                    return (System.String)this[this.table.PaytypeID];
                }
                catch
                {
                    return System.String.Empty;
                }
            } 
			set { this[this.table.PaytypeID] = value; }
		}

		public System.String PayTypeDesc
		{
			get 
			{ 
				try 
				{
                    return (System.String)this[this.table.PayTypeDesc];
				}
				catch
				{
                    return System.String.Empty;
				}
			}
            set { this[this.table.PayTypeDesc] = value; }
		}

		public System.String PayType
		{
            get
            {
                try
                {
                    return (System.String)this[this.table.PayType];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.PayType] = value; }
		}
		
		public System.String UserID
		{
            get
            {
                try
                {
                    return (System.String)this[this.table.UserID];
                }
                catch
                {
                    return System.String.Empty;
                }
            } 
			set { this[this.table.UserID] = value; }
		}

        //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
        public System.Boolean IsHide
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsHide];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsHide] = value; }
        }

        //PRIMEPOS-2309 08-Mar-2019 JY Added
        public System.Boolean StopAtRefNo
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.StopAtRefNo];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.StopAtRefNo] = value; }
        }

        //PRIMEPOS-2966 20-May-2021 JY Added
        public System.Int32 SortOrder
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.SortOrder];
                }
                catch
                {
                    return System.Int32.MinValue;
                }
            }
            set { this[this.table.SortOrder] = value; }
        }
        #endregion
    }
}
