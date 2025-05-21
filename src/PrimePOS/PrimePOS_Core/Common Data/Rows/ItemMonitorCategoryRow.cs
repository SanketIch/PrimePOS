namespace POS_Core.CommonData.Rows
{
    using System.Data;
    using POS_Core.CommonData.Tables;

    public class ItemMonitorCategoryRow : DataRow
    {
        private ItemMonitorCategoryTable table;

        internal ItemMonitorCategoryRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (ItemMonitorCategoryTable) this.Table;
        }

        #region Public Properties

        public System.Int32 ID
        {
            get
            {
                try
                {
                    return (System.Int32) this[this.table.ID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ID] = value; }
        }

        public System.String Description
        {
            get
            {
                try
                {
                    return (System.String) this[this.table.Description];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.Description] = value; }
        }

        public System.String UserID
        {
            get
            {
                try
                {
                    return (System.String) this[this.table.UserID];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.UserID] = value; }
        }

        public System.String UOM
        {
            get
            {
                try
                {
                    return (System.String) this[this.table.UOM];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.UOM] = value; }
        }

        public System.Decimal DailyLimit
        {
            get
            {
                try
                {
                    return (System.Decimal) this[this.table.DailyLimit];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.DailyLimit] = value; }
        }

        public System.Decimal ThirtyDaysLimit
        {
            get
            {
                try
                {
                    return (System.Decimal) this[this.table.ThirtyDaysLimit];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ThirtyDaysLimit] = value; }
        }

        public System.Int32 MaxExempt
        {
            get
            {
                try
                {
                    return (System.Int32) this[this.table.MaxExempt];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.MaxExempt] = value; }
        }

        //Added By shitaljit on 30 April 2012
        public System.String VerificationBy
        {
            get
            {
                try
                {
                    return (System.String) this[this.table.VerificationBy];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.VerificationBy] = value; }
        }

        public System.Int32 LimitPeriodDays
        {
            get
            {
                try
                {
                    return (System.Int32) this[this.table.LimitPeriodDays];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LimitPeriodDays] = value; }
        }

        //Added by Manoj 7/11/2013
        public System.Int32 AgeLimit
        {
            get
            {
                try
                {
                    return (System.Int32) this[this.table.AgeLimit];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.AgeLimit] = value; }
        }

        //Added by Manoj 7/11/2013
        public System.Boolean IsAgeLimit
        {
            get
            {
                try
                {
                    return (System.Boolean) this[this.table.IsAgeLimit.ToString()];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsAgeLimit] = value; }
        }

        public System.Decimal LimitPeriodQty
        {
            get
            {
                try
                {
                    return (System.Decimal) this[this.table.LimitPeriodQty];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LimitPeriodQty] = value; }
        }

        //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        public System.Boolean ePSE
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.ePSE.ToString()];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.ePSE] = value; }
        }

        #region PRIMEPOS-3166
        public System.Boolean canOverrideMonitorItem
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.canOverrideMonitorItem.ToString()];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.canOverrideMonitorItem] = value; }
        }
        #endregion

        #endregion Public Properties
    }
}