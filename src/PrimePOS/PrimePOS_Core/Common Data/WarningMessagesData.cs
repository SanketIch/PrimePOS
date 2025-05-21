
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class WarningMessagesData : DataSet 
	{

		private WarningMessagesTable _WarningMessagesTable;

		#region Constructors
		public WarningMessagesData() 
		{
			this.InitClass();
		}

		#endregion


		public WarningMessagesTable WarningMessages
		{
			get 
			{
				return this._WarningMessagesTable;
			}
			set 
			{
				this._WarningMessagesTable = value;
			}
		}

		public override DataSet Clone() 
		{
			WarningMessagesData cln = (WarningMessagesData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_WarningMessagesTable = (WarningMessagesTable)this.Tables[clsPOSDBConstants.WarningMessages_tbl];
			if (_WarningMessagesTable != null) 
			{
				_WarningMessagesTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.WarningMessages_tbl;
			this.Prefix = "";
			_WarningMessagesTable = new WarningMessagesTable();
			this.Tables.Add(this.WarningMessages);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.WarningMessages_tbl] != null) 
			{
				this.Tables.Add(new WarningMessagesTable(ds.Tables[clsPOSDBConstants.WarningMessages_tbl]));
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
