
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TransDetailData : DataSet 
	{

		private TransDetailTable _TransDetailTable;

		#region Constructors
		public TransDetailData() 
		{
			this.InitClass();
		}

		#endregion


		public TransDetailTable TransDetail 
		{
			get 
			{
				return this._TransDetailTable;
			}
			set 
			{
				this._TransDetailTable = value;
			}
		}

		public override DataSet Clone() 
		{
			TransDetailData cln = (TransDetailData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_TransDetailTable = (TransDetailTable)this.Tables[clsPOSDBConstants.TransDetail_tbl];
			if (_TransDetailTable != null) 
			{
				_TransDetailTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.TransDetail_tbl;
			this.Prefix = "";


			_TransDetailTable = new TransDetailTable();
			this.Tables.Add(this.TransDetail);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.TransDetail_tbl] != null) 
			{
				this.Tables.Add(new TransDetailTable(ds.Tables[clsPOSDBConstants.TransDetail_tbl]));
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
