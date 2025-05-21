
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class InvRecvHeaderData : DataSet 
	{

		private InvRecvHeaderTable _InvRecvHeaderTable;

		#region Constructors
		public InvRecvHeaderData() 
		{
			this.InitClass();
		}

		#endregion


		public InvRecvHeaderTable InvRecievedHeader 
		{
			get 
			{
				return this._InvRecvHeaderTable;
			}
			set 
			{
				this._InvRecvHeaderTable = value;
			}
		}

		public override DataSet Clone() 
		{
			InvRecvHeaderData cln = (InvRecvHeaderData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_InvRecvHeaderTable = (InvRecvHeaderTable)this.Tables[clsPOSDBConstants.InvRecvHeader_tbl];
			if (_InvRecvHeaderTable != null) 
			{
				_InvRecvHeaderTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.InvRecvHeader_tbl;
			this.Prefix = "";
			_InvRecvHeaderTable = new InvRecvHeaderTable();
			this.Tables.Add(this.InvRecievedHeader);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.InvRecvHeader_tbl] != null) 
			{
				this.Tables.Add(new InvRecvHeaderTable(ds.Tables[clsPOSDBConstants.InvRecvHeader_tbl]));
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
