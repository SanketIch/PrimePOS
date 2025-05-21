namespace POS_Core.CommonData.Rows
{
    using POS_Core.CommonData.Tables;
    using POS_Core.Resources;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StoreCreditDetailsRow : DataRow
    {
        // StoreCredit - PRIMEPOS-2747 - NileshJ
        private StoreCreditDetailsTable table;

        internal StoreCreditDetailsRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (StoreCreditDetailsTable)this.Table;
        }
        #region Public Properties

        public System.Int32 StoreCreditDetailsID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.StoreCreditDetailsID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.StoreCreditDetailsID] = value; }
        }

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

        public System.Int32 TransactionID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransactionID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransactionID] = value; }
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

        public System.DateTime UpdatedOn
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.UpdatedOn];
                }
                catch
                {
                    return Convert.ToDateTime(UpdatedOn);
                }
            }
            set { this[this.table.UpdatedOn] = value; }
        }


        public System.String UpdatedBy
        {
            get
            {
                return this[this.table.UpdatedBy].ToString();
            }
            set { this[this.table.UpdatedBy] = value; }
        }

        public System.Decimal RemainingAmount
        {
            get
            {
                try
                {
                    return Configuration.convertNullToDecimal(this[this.table.RemainingAmount]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.RemainingAmount] = value; }
        }
        #endregion

    }
}
