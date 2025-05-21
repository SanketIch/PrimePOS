
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class POHeaderData : DataSet 
	{

		private POHeaderTable _POHeaderTable;

		#region Constructors
		public POHeaderData() 
		{
			this.InitClass();
		}

		#endregion


		public POHeaderTable POHeader 
		{
			get 
			{
				return this._POHeaderTable;
			}
			set 
			{
				this._POHeaderTable = value;
			}
		}

		public override DataSet Clone() 
		{
			POHeaderData cln = (POHeaderData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_POHeaderTable = (POHeaderTable)this.Tables[clsPOSDBConstants.POHeader_tbl];
			if (_POHeaderTable != null) 
			{
				_POHeaderTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.POHeader_tbl;
			this.Prefix = "";
			_POHeaderTable = new POHeaderTable();
			this.Tables.Add(this.POHeader);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.POHeader_tbl] != null) 
			{
				this.Tables.Add(new POHeaderTable(ds.Tables[clsPOSDBConstants.POHeader_tbl]));
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
