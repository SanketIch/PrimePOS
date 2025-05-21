
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TransHeaderData : DataSet 
	{

		private TransHeaderTable _TransHeaderTable;

		#region Constructors
		public TransHeaderData() 
		{
			this.InitClass();
		}

		#endregion


		public TransHeaderTable TransHeader 
		{
			get 
			{
				return this._TransHeaderTable;
			}
			set 
			{
				this._TransHeaderTable = value;
			}
		}

		public override DataSet Clone() 
		{
			TransHeaderData cln = (TransHeaderData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_TransHeaderTable = (TransHeaderTable)this.Tables[clsPOSDBConstants.TransHeader_tbl];
			if (_TransHeaderTable != null) 
			{
				_TransHeaderTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.TransHeader_tbl;
			this.Prefix = "";
			_TransHeaderTable = new TransHeaderTable();
			this.Tables.Add(this.TransHeader);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.TransHeader_tbl] != null) 
			{
				this.Tables.Add(new TransHeaderTable(ds.Tables[clsPOSDBConstants.TransHeader_tbl]));
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
