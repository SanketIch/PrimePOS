using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Rows
{
    public class usrWeeklyRow : BaseRow
    {
        private usrWeeklyTable table;

        // Constructor
        internal usrWeeklyRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (usrWeeklyTable)this.Table;
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

        public System.Int32 Days
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.Days];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.Days] = value;
            }
        }

        public System.String SelectedDays
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SelectedDays];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.SelectedDays] = value;
            }
        }
        #endregion
    }
}
