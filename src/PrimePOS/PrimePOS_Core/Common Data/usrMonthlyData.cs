using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData
{
    public class usrMonthlyData : DataSet
    {
        private usrMonthlyTable _clsusrMonthlyTable;

        #region Constructors
        public usrMonthlyData()
        {
            this.InitClass();
        }
        #endregion

        public override DataSet Clone()
        {
            usrMonthlyData cln = (usrMonthlyData)base.Clone();
            cln.InitVars();
            return cln;
        }

        public usrMonthlyTable ScheduledTasksDetailMonth
        {
            get
            {
                return this._clsusrMonthlyTable;
            }
            set
            {
                this._clsusrMonthlyTable = value;
            }
        }

        #region Initialization
        internal void InitVars()
        {
            _clsusrMonthlyTable = (usrMonthlyTable)this.Tables[clsPOSDBConstants.ScheduledTasksDetailMonth_tbl];
            if (_clsusrMonthlyTable != null)
            {
                _clsusrMonthlyTable.InitVars();
            }
        }

        private void InitClass()
        {
            this.DataSetName = "usrMonthlyData";
            this.Prefix = "";
            _clsusrMonthlyTable = new usrMonthlyTable();
            this.Tables.Add(this.ScheduledTasksDetailMonth);
        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.ScheduledTasksDetailMonth_tbl] != null)
            {
                this.Tables.Add(new usrMonthlyTable(ds.Tables[clsPOSDBConstants.ScheduledTasksDetailMonth_tbl]));
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