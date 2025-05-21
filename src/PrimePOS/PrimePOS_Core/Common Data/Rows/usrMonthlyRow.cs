using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Rows
{
    public class usrMonthlyRow : BaseRow
    {
        private usrMonthlyTable table;

        // Constructor
        internal usrMonthlyRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (usrMonthlyTable)this.Table;
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

        public System.Boolean DaysOrOn
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.DaysOrOn];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                this[this.table.DaysOrOn] = value;
            }
        }

        public System.String SelectionMonths
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SelectionMonths];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.SelectionMonths] = value;
            }
        }

        public System.String SelectionDays
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SelectionDays];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.SelectionDays] = value;
            }
        }

        public System.String Monthperiods
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Monthperiods];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.Monthperiods] = value;
            }
        }

        public System.String weekDays
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.weekDays];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.weekDays] = value;
            }
        }
        #endregion
    }
}