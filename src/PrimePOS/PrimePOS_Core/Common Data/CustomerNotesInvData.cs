
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CustomerNotesData : DataSet 
	{

		private CustomerNotesTable _CustomerNotesTable;

		#region Constructors
		public CustomerNotesData() 
		{
			this.InitClass();
		}

		#endregion


		public CustomerNotesTable CustomerNotes 
		{
			get 
			{
				return this._CustomerNotesTable;
			}
			set 
			{
				this._CustomerNotesTable = value;
			}
		}

		public override DataSet Clone() 
		{
			CustomerNotesData cln = (CustomerNotesData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_CustomerNotesTable = (CustomerNotesTable)this.Tables[clsPOSDBConstants.CustomerNotes_tbl];
			if (_CustomerNotesTable != null) 
			{
				_CustomerNotesTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.CustomerNotes_tbl;
			this.Prefix = "";


			_CustomerNotesTable = new CustomerNotesTable();
			this.Tables.Add(this.CustomerNotes);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.CustomerNotes_tbl] != null) 
			{
				this.Tables.Add(new CustomerNotesTable(ds.Tables[clsPOSDBConstants.CustomerNotes_tbl]));
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
