
namespace POS_Core.CommonData.Rows
{
    // StoreCredit - PRIMEPOS-2747 - NileshJ
    using System;
    using System.Data;
    using POS.Resources;
    using POS_Core.CommonData.Tables;
    using POS_Core.Resources;

    public class StoreCreditRow : DataRow
    {
        private StoreCreditTable table;

        internal StoreCreditRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (StoreCreditTable)this.Table;
        }
        #region Public Properties

        public System.Int32 StoreCreditID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.StoreCreditID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.StoreCreditID] = value; }
        }

        public System.Int32 CustomerID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.CustomerID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CustomerID] = value; }
        }

        public System.Decimal CreditAmt
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.CreditAmt]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CreditAmt] = value; }
        }

        public System.DateTime LastUpdated
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.LastUpdated];
                }
                catch
                {
                    return Convert.ToDateTime(LastUpdated);
                }
            }
            set { this[this.table.LastUpdated] = value; }
        }


        public System.String LastUpdatedBy
        {
            get
            {
                return this[this.table.LastUpdatedBy].ToString();
            }
            set { this[this.table.LastUpdatedBy] = value; }
        }

        public System.Int32 ExpiresOn
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ExpiresOn];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ExpiresOn] = value; }
        }
        #endregion
    }
}
