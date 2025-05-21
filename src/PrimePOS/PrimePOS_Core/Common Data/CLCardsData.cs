
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLCardsData : DataSet 
	{

		private CLCardsTable _CLCardsTable;

		#region Constructors
		public CLCardsData() 
		{
			this.InitClass();
		}

		#endregion


		public CLCardsTable CLCards 
		{
			get 
			{
				return this._CLCardsTable;
			}
			set 
			{
				this._CLCardsTable = value;
			}
		}

		public override DataSet Clone() 
		{
			CLCardsData cln = (CLCardsData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_CLCardsTable = (CLCardsTable)this.Tables[clsPOSDBConstants.CLCards_tbl];
			if (_CLCardsTable != null) 
			{
				_CLCardsTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "CLCardsData";
			this.Prefix = "";


			_CLCardsTable = new CLCardsTable();
			this.Tables.Add(this.CLCards);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.CLCards_tbl] != null) 
			{
				this.Tables.Add(new CLCardsTable(ds.Tables[clsPOSDBConstants.CLCards_tbl]));
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
