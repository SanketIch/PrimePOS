
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class PayOutData : DataSet 
	{

		private PayOutTable _PayOutTable;

		#region Constructors
		public PayOutData() 
		{
			this.InitClass();
		}

		#endregion


		public PayOutTable PayOut 
		{
			get 
			{
				return this._PayOutTable;
			}
			set 
			{
				this._PayOutTable = value;
			}
		}

		public override DataSet Clone() 
		{
			PayOutData cln = (PayOutData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_PayOutTable = (PayOutTable)this.Tables[clsPOSDBConstants.PayOut_tbl];
			if (_PayOutTable != null) 
			{
				_PayOutTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.PayOut_tbl;
			this.Prefix = "";


			_PayOutTable = new PayOutTable();
			this.Tables.Add(this.PayOut);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.PayOut_tbl] != null) 
			{
				this.Tables.Add(new PayOutTable(ds.Tables[clsPOSDBConstants.PayOut_tbl]));
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
