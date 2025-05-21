
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TimesheetData : DataSet 
	{

		private TimesheetTable _TimesheetTable;

		#region Constructors
		public TimesheetData() 
		{
			this.InitClass();
		}

		#endregion


		public TimesheetTable Timesheet 
		{
			get 
			{
				return this._TimesheetTable;
			}
			set 
			{
				this._TimesheetTable = value;
			}
		}

		public override DataSet Clone() 
		{
			TimesheetData cln = (TimesheetData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_TimesheetTable = (TimesheetTable)this.Tables[clsPOSDBConstants.Timesheet_tbl];
			if (_TimesheetTable != null) 
			{
				_TimesheetTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.Timesheet_tbl;
			this.Prefix = "";
			_TimesheetTable = new TimesheetTable();
			this.Tables.Add(this.Timesheet);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.Timesheet_tbl] != null) 
			{
				this.Tables.Add(new TimesheetTable(ds.Tables[clsPOSDBConstants.Timesheet_tbl]));
			}

			this.DataSetName = ds.DataSetName;
			this.Prefix = ds.Prefix;
			this.Namespace = ds.Namespace;
			this.Locale = ds.Locale;
			this.CaseSensitive = ds.CaseSensitive;
			this.EnforceConstraints = ds.EnforceConstraints;
			this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
			this.InitVars();
		}

		#endregion

		private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) 
		{
			if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) 
			{
				this.InitVars();
			}
		}


	}

}
