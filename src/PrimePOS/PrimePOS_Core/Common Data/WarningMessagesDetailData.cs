
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class WarningMessagesDetailData : DataSet 
	{

		private WarningMessagesDetailTable _WarningMessagesDetailTable;

		#region Constructors
		public WarningMessagesDetailData() 
		{
			this.InitClass();
		}

		#endregion


		public WarningMessagesDetailTable WarningMessagesDetail 
		{
			get 
			{
				return this._WarningMessagesDetailTable;
			}
			set 
			{
				this._WarningMessagesDetailTable = value;
			}
		}

		public override DataSet Clone() 
		{
			WarningMessagesDetailData cln = (WarningMessagesDetailData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_WarningMessagesDetailTable = (WarningMessagesDetailTable)this.Tables[clsPOSDBConstants.WarningMessagesDetail_tbl];
			if (_WarningMessagesDetailTable != null) 
			{
				_WarningMessagesDetailTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.WarningMessagesDetail_tbl;
			this.Prefix = "";


			_WarningMessagesDetailTable = new WarningMessagesDetailTable();
			this.Tables.Add(this.WarningMessagesDetail);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.WarningMessagesDetail_tbl] != null) 
			{
				this.Tables.Add(new WarningMessagesDetailTable(ds.Tables[clsPOSDBConstants.WarningMessagesDetail_tbl]));
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
