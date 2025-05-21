//----------------------------------------------------------------------------------------------------
//Sprint-23 - PRIMEPOS-2029 19-Apr-2016 JY Added to maintain item monitor trans log
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    //using POS.Resources;

    public class ItemMonitorTransDetailRow : DataRow 
    {
        private ItemMonitorTransDetailTable table;

        internal ItemMonitorTransDetailRow(DataRowBuilder rb) : base(rb) 
		{
            this.table = (ItemMonitorTransDetailTable)this.Table;
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

        public System.String UOM
        {
            get
            {
                return this[this.table.UOM].ToString();
            }
            set { this[this.table.UOM] = value; }
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
                    return System.Decimal.MinValue;
                }
            }
            set { this[this.table.UnitsPerPackage] = value; }
        }

        public System.Boolean SentToNplex
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.SentToNplex];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.SentToNplex] = value; }
        }

        public System.Boolean PostSaleInd
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.PostSaleInd];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.PostSaleInd] = value; }
        }

        //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
        public System.Int64 pseTrxId
        {
            get
            {
                try
                {
                    return (System.Int64)this[this.table.pseTrxId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.pseTrxId] = value; }
        }
        #endregion
    }
}
