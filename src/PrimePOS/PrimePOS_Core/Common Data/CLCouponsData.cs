
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLCouponsData : DataSet 
	{

		private CLCouponsTable _CLCouponsTable;

		#region Constructors
		public CLCouponsData() 
		{
			this.InitClass();
		}

		#endregion


		public CLCouponsTable CLCoupons 
		{
			get 
			{
				return this._CLCouponsTable;
			}
			set 
			{
				this._CLCouponsTable = value;
			}
		}

		public override DataSet Clone() 
		{
			CLCouponsData cln = (CLCouponsData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_CLCouponsTable = (CLCouponsTable)this.Tables[clsPOSDBConstants.CLCoupons_tbl];
			if (_CLCouponsTable != null) 
			{
				_CLCouponsTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "CLCouponsData";
			this.Prefix = "";


			_CLCouponsTable = new CLCouponsTable();
			this.Tables.Add(this.CLCoupons);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.CLCoupons_tbl] != null) 
			{
				this.Tables.Add(new CLCouponsTable(ds.Tables[clsPOSDBConstants.CLCoupons_tbl]));
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
