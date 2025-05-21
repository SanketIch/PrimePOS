using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Rows
{
    public class ScheduledTasksLogRow : BaseRow
    {
        private ScheduledTasksLogTable table;

        // Constructor
        internal ScheduledTasksLogRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (ScheduledTasksLogTable)this.Table;
        }

        #region Public Properties
        public System.Int32 ScheduledTasksLogID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ScheduledTasksLogID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ScheduledTasksLogID] = value;
            }
        }

        public System.String TaskStatus
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaskStatus];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.TaskStatus] = value;
            }
        }

        public System.String LogDescription
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.LogDescription];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.LogDescription] = value;
            }
        }

        public System.Object StartDate
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.StartDate];
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            set
            {
                this[this.table.StartDate] = value;
            }
        }

        public System.Object StartTime
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.StartTime];
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            set
            {
                this[this.table.StartTime] = value;
            }
        }

        public System.Object EndTime
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.EndTime];
                }
                catch
                {
                    return DBNull.Value;
                }
            }
            set
            {
                this[this.table.EndTime] = value;
            }
        }

        public System.Int32 ScheduledTasksID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ScheduledTasksID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ScheduledTasksID] = value;
            }
        }

        public System.String ComputerName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ComputerName];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.ComputerName] = value;
            }
        }
        #endregion
    }
}