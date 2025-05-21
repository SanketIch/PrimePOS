using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData
{
    public class usrWeeklyData : DataSet
    {
        private usrWeeklyTable _clsusrWeeklyTable;

        #region Constructors
        public usrWeeklyData()
        {
            this.InitClass();
        }
        #endregion

        public override DataSet Clone()
        {
            usrWeeklyData cln = (usrWeeklyData)base.Clone();
            cln.InitVars();
            return cln;
        }

        public usrWeeklyTable ScheduledTasksDetailWeek
        {
            get
            {
                return this._clsusrWeeklyTable;
            }
            set
            {
                this._clsusrWeeklyTable = value;
            }
        }

        #region Initialization
        internal void InitVars()
        {
            _clsusrWeeklyTable = (usrWeeklyTable)this.Tables[clsPOSDBConstants.ScheduledTasksDetailWeek_tbl];
            if (_clsusrWeeklyTable != null)
            {
                _clsusrWeeklyTable.InitVars();
            }
        }

        private void InitClass()
        {
            this.DataSetName = "usrWeeklyData";
            this.Prefix = "";
            _clsusrWeeklyTable = new usrWeeklyTable();
            this.Tables.Add(this.ScheduledTasksDetailWeek);
        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.ScheduledTasksDetailWeek_tbl] != null)
            {
                this.Tables.Add(new usrWeeklyTable(ds.Tables[clsPOSDBConstants.ScheduledTasksDetailWeek_tbl]));
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
