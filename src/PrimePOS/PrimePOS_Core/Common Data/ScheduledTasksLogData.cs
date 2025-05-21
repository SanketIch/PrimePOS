using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData
{
    public class ScheduledTasksLogData : DataSet
    {
        private ScheduledTasksLogTable _clsScheduledTasksLogTable;

        #region Constructors
        public ScheduledTasksLogData()
        {
            this.InitClass();
        }
        #endregion

        public override DataSet Clone()
        {
            ScheduledTasksLogData cln = (ScheduledTasksLogData)base.Clone();
            cln.InitVars();
            return cln;
        }

        public ScheduledTasksLogTable ScheduledTasksLog
        {
            get
            {
                return this._clsScheduledTasksLogTable;
            }
            set
            {
                this._clsScheduledTasksLogTable = value;
            }
        }

        #region Initialization
        internal void InitVars()
        {
            _clsScheduledTasksLogTable = (ScheduledTasksLogTable)this.Tables[clsPOSDBConstants.ScheduledTasksLog_tbl];
            if (_clsScheduledTasksLogTable != null)
            {
                _clsScheduledTasksLogTable.InitVars();
            }
        }

        private void InitClass()
        {
            this.DataSetName = "ScheduledTasksLogData";
            this.Prefix = "";
            _clsScheduledTasksLogTable = new ScheduledTasksLogTable();
            this.Tables.Add(this.ScheduledTasksLog);
        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.ScheduledTasksLog_tbl] != null)
            {
                this.Tables.Add(new ScheduledTasksLogTable(ds.Tables[clsPOSDBConstants.ScheduledTasksLog_tbl]));
            }

            this.DataSetName = ds.DataSetName;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        #endregion
    }
}