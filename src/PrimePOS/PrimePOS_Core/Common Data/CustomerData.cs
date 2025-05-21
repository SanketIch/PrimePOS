namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	//     This class is used to define the shape of CustomerData.
	
	public class CustomerData : DataSet 
	{

		private CustomerTable _clsCustomerTable;


		#region Constructors
		//     Constructor for VendorData.

		public CustomerData() 
		{
			this.InitClass();
		}

		#endregion

		public override DataSet Clone() 
		{
			CustomerData cln = (CustomerData)base.Clone();
			cln.InitVars();
			return cln;
		}


		public CustomerTable Customer 
		{
			get 
			{
				return this._clsCustomerTable;
			}
			set 
			{
				this._clsCustomerTable = value;
			}
		}

		#region Initialization

		internal void InitVars() 
		{

			_clsCustomerTable = (CustomerTable)this.Tables[clsPOSDBConstants.Customer_tbl];
			if (_clsCustomerTable != null) 
			{
				_clsCustomerTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "CustomerData";
			this.Prefix = "";


			_clsCustomerTable = new CustomerTable();
			this.Tables.Add(this.Customer);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.Customer_tbl] != null) 
			{
				this.Tables.Add(new CustomerTable(ds.Tables[clsPOSDBConstants.Customer_tbl]));
			}


			this.DataSetName = ds.DataSetName;
			this.CaseSensitive = ds.CaseSensitive;
			this.EnforceConstraints = ds.EnforceConstraints;
			this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
			this.InitVars();
		}

		#endregion

	}
}
