using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Rows
{
    public class MessageLogRow : DataRow
    {
        private MessageLogTable table;

        // Constructor
        internal MessageLogRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (MessageLogTable)this.Table;
        }
        #region Public Properties

        public System.DateTime LogDate
        {
            get
            {
                //try
                //{
                    return (System.DateTime)this[this.table.LogDate];
                //}
                //catch
                //{
                //    //return 0;
                //}
            }
            set
            {

                this[this.table.LogDate] = value;
            }
        }

        public System.String LogTime
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.LogTime];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {

                this[this.table.LogDate] = value;
            }
        }



        public System.String LogMessage
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.LogMessage];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { if (value != "") this[this.table.LogMessage] = value; }
        }
        #endregion Public Properties
    }
}
