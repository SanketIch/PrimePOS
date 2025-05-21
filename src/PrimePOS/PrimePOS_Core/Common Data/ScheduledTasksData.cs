using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData
{
    public class ScheduledTasksData : DataSet
    {
        private ScheduledTasksTable _clsScheduledTasksTable;

		#region Constructors
		public ScheduledTasksData()
		{
			this.InitClass();
		}
		#endregion

		public override DataSet Clone()
		{
			ScheduledTasksData cln = (ScheduledTasksData)base.Clone();
			cln.InitVars();
			return cln;
		}

		public ScheduledTasksTable ScheduledTasks
		{
			get
			{
				return this._clsScheduledTasksTable;
			}
			set
			{
				this._clsScheduledTasksTable = value;
			}
		}

		#region Initialization
		internal void InitVars()
		{
			_clsScheduledTasksTable = (ScheduledTasksTable)this.Tables[clsPOSDBConstants.ScheduledTasks_tbl];
			if (_clsScheduledTasksTable != null)
			{
				_clsScheduledTasksTable.InitVars();
			}
		}

		private void InitClass()
		{
			this.DataSetName = "ScheduledTasksData";
			this.Prefix = "";
			_clsScheduledTasksTable = new ScheduledTasksTable();
			this.Tables.Add(this.ScheduledTasks);
		}

		private void InitClass(DataSet ds)
		{

			if (ds.Tables[clsPOSDBConstants.ScheduledTasks_tbl] != null)
			{
				this.Tables.Add(new ScheduledTasksTable(ds.Tables[clsPOSDBConstants.ScheduledTasks_tbl]));
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
