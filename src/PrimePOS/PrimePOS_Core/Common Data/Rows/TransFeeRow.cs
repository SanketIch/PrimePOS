namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    public class TransFeeRow : DataRow
    {
        private TransFeeTable table;

        internal TransFeeRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (TransFeeTable)this.Table;
        }

        #region Public Properties
        public System.Int32 TransFeeID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.transFeeID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.transFeeID] = value; }
        }

        public System.String TransFeeDesc
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.transFeeDesc];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.transFeeDesc] = value; }
        }

        public System.Int16 ChargeTransFeeFor
        {
            get
            {
                try
                {
                    return (System.Int16)this[this.table.chargeTransFeeFor];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.chargeTransFeeFor] = value; }
        }

        public System.Boolean TransFeeMode
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.transFeeMode];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.transFeeMode] = value; }
        }

        public System.Decimal TransFeeValue
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.transFeeValue];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.transFeeValue] = value; }
        }

        public System.String PayTypeID
        {
            get
            {
                try
                {
                    return this[this.table.payTypeID].ToString();
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.payTypeID] = value; }
        }

        public System.Boolean IsActive
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.isActive];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.isActive] = value; }
        }
        #endregion        
    }

    public class TransFeeRowChangeEvent : EventArgs
    {
        private TransFeeRow eventRow;
        private DataRowAction eventAction;

        public TransFeeRowChangeEvent(TransFeeRow row, DataRowAction action)
        {
            this.eventRow = row;
            this.eventAction = action;
        }

        public TransFeeRow Row
        {
            get { return this.eventRow; }
        }

        public DataRowAction Action
        {
            get { return this.eventAction; }
        }
    }
}
