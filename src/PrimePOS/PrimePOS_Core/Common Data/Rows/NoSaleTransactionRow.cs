namespace POS_Core.CommonData.Rows
{

    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class NoSaleTransactionRow : DataRow
    {
        private NoSaleTransactionTable table;
        internal NoSaleTransactionRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (NoSaleTransactionTable)this.Table;
        }

        #region Public Properties
        public System.Int32 Id
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.Id];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.Id] = value; }
        }
        public System.String UserId
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.UserId];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.UserId] = value; }
        }
        public System.String StationId
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.StationId];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.StationId] = value; }
        }
        public System.DateTime DrawerOpenedDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.DrawerOpenedDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.DrawerOpenedDate] = value; }
        }
        #endregion
    }
}