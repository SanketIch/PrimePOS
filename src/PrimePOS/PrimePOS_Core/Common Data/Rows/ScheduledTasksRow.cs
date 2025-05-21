using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Rows
{
    public class ScheduledTasksRow : BaseRow
    {
        private ScheduledTasksTable table;

        // Constructor
        internal ScheduledTasksRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (ScheduledTasksTable)this.Table;
        }

        #region Public Properties
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

        public System.String TaskName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaskName];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.TaskName] = value;
            }
        }

        public System.String TaskDescription
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaskDescription];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.TaskDescription] = value;
            }
        }

        public System.Int32 PerformTask
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PerformTask];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.PerformTask] = value;
            }
        }
        
        public System.Int32 RepeatTask
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.RepeatTask];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.RepeatTask] = value;
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

        public System.Int32 TaskId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TaskId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.TaskId] = value;
            }
        }

        public System.Boolean SendEmail
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.SendEmail];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                this[this.table.SendEmail] = value;
            }
        }

        public System.String EmailAddress
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EmailAddress];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.EmailAddress] = value;
            }
        }

        public System.Boolean AdvancedSeetings
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.AdvancedSeetings];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                this[this.table.AdvancedSeetings] = value;
            }
        }

        public System.Int64 RepeatTaskInterval
        {
            get
            {
                try
                {
                    return (System.Int64)this[this.table.RepeatTaskInterval];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.RepeatTaskInterval] = value;
            }
        }

        public System.Int64 Duration
        {
            get
            {
                try
                {
                    return (System.Int64)this[this.table.Duration];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.Duration] = value;
            }
        }

        public System.Boolean SendPrint
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.SendPrint];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                this[this.table.SendPrint] = value;
            }
        }

        public System.Boolean Enabled
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.Enabled];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                this[this.table.Enabled] = value;
            }
        }

        public System.String TaskNameOld
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaskNameOld];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.TaskNameOld] = value;
            }
        }

        public System.String PerformTaskText
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PerformTaskText];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.PerformTaskText] = value;
            }
        }
        public System.String SendEmailText
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SendEmailText];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.SendEmailText] = value;
            }
        }
        public System.String EnabledText
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EnabledText];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.EnabledText] = value;
            }
        }
        public System.String SendPrintText
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SendPrintText];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.SendPrintText] = value;
            }
        }
        public System.String colTask
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.colTask];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.colTask] = value;
            }
        }
        public System.String LastExecuted
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.LastExecuted];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.LastExecuted] = value;
            }
        }
        #endregion
    }
}