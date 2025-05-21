
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class DepartmentData : DataSet 
	{

		private DepartmentTable _DepartmentTable;

		#region Constructors
		public DepartmentData() 
		{
			this.InitClass();
		}

		#endregion


		public DepartmentTable Department 
		{
			get 
			{
				return this._DepartmentTable;
			}
			set 
			{
				this._DepartmentTable = value;
			}
		}

		public override DataSet Clone() 
		{
			DepartmentData cln = (DepartmentData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_DepartmentTable = (DepartmentTable)this.Tables[clsPOSDBConstants.Department_tbl];
			if (_DepartmentTable != null) 
			{
				_DepartmentTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "DepartmentData";
			this.Prefix = "";


			_DepartmentTable = new DepartmentTable();
			this.Tables.Add(this.Department);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.Department_tbl] != null) 
			{
				this.Tables.Add(new DepartmentTable(ds.Tables[clsPOSDBConstants.Department_tbl]));
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
