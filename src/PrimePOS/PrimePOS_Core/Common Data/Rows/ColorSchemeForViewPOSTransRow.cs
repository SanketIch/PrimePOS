// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class ColorSchemeForViewPOSTransRow : DataRow
    {
        private ColorSchemeForViewPOSTransTable table;

        /// <summary>
        /// Constructor
        /// </summary>
        internal ColorSchemeForViewPOSTransRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (ColorSchemeForViewPOSTransTable)this.Table;
        }
        #region Public Properties

        /// <summary>
        /// Public Property FunKey
        /// </summary>
        public System.Int32 ID
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

        /// <summary>
        /// Public Property Operation
        /// </summary>
        public System.Decimal FromAmount
        {
            get
            {
                try
                {
                    return (System.Decimal) (this[this.table.FromAmount]);
                }
                catch
                {
                   return 0;
                }
            }
            set { this[this.table.FromAmount] = value; }
        }

        public System.Decimal ToAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)(this[this.table.ToAmount]);
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ToAmount] = value; }
        }

        public System.String BackColor
        {
            get
            {
                try
                {
                    return this[this.table.BackColor].ToString();
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.BackColor] = value; }
        }

        public System.String ForeColor
        {
            get
            {
                try
                {
                    return this[this.table.ForeColor].ToString();
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ForeColor] = value; }
        }
        #endregion // Properties
    }
    public class ColorSchemeForViewPOSTransRowChangeEvent : EventArgs
    {
        private ColorSchemeForViewPOSTransRow eventRow;
        private DataRowAction eventAction;

        public ColorSchemeForViewPOSTransRowChangeEvent(ColorSchemeForViewPOSTransRow row, DataRowAction action)
        {
            this.eventRow = row;
            this.eventAction = action;
        }

        public ColorSchemeForViewPOSTransRow Row
        {
            get { return this.eventRow; }
        }

        public DataRowAction Action
        {
            get { return this.eventAction; }
        }
    }
}
