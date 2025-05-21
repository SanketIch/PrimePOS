
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class PODetailData : DataSet 
	{

		private PODetailTable _PODetailTable;

		#region Constructors
		public PODetailData() 
		{
			this.InitClass();
		}

		#endregion


		public PODetailTable PODetail 
		{
			get 
			{
				return this._PODetailTable;
			}
			set 
			{
				this._PODetailTable = value;
			}
		}

		public override DataSet Clone() 
		{
			PODetailData cln = (PODetailData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_PODetailTable = (PODetailTable)this.Tables[clsPOSDBConstants.PODetail_tbl];
			if (_PODetailTable != null) 
			{
				_PODetailTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.PODetail_tbl;
			this.Prefix = "";


			_PODetailTable = new PODetailTable();
			this.Tables.Add(this.PODetail);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.PODetail_tbl] != null) 
			{
				this.Tables.Add(new PODetailTable(ds.Tables[clsPOSDBConstants.PODetail_tbl]));
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
