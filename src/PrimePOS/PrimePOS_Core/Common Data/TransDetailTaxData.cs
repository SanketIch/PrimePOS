
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TransDetailTaxData : DataSet 
	{

		private TransDetailTaxTable _TransDetailTaxTable;

		#region Constructors
		public TransDetailTaxData() 
		{
			this.InitClass();
		}

		#endregion


		public TransDetailTaxTable TransDetailTax
		{
			get 
			{
				return this._TransDetailTaxTable;
			}
			set 
			{
				this._TransDetailTaxTable = value;
			}
		}

		public override DataSet Clone() 
		{
			TransDetailTaxData cln = (TransDetailTaxData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_TransDetailTaxTable = (TransDetailTaxTable)this.Tables[clsPOSDBConstants.TransDetailTax_tbl];
			if (_TransDetailTaxTable != null) 
			{
				_TransDetailTaxTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.TransDetailTax_tbl;
			this.Prefix = "";


			_TransDetailTaxTable = new TransDetailTaxTable();
			this.Tables.Add(this.TransDetailTax);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.TransDetailTax_tbl] != null) 
			{
				this.Tables.Add(new TransDetailTaxTable(ds.Tables[clsPOSDBConstants.TransDetailTax_tbl]));
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
