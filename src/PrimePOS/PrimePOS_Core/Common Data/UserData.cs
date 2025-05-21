namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	//     This class is used to define the shape of UserData.
	
	public class UserData : DataSet 
	{
         
		private UserTable _clsUserTable;


		#region Constructors
		//     Constructor for VendorData.

		public UserData() 
		{
			this.InitClass();
		}

		#endregion

		public override DataSet Clone() 
		{
			UserData cln = (UserData)base.Clone();
			cln.InitVars();
			return cln;
		}


		public UserTable User 
		{
			get 
			{
				return this._clsUserTable;
			}
			set 
			{
				this._clsUserTable = value;
			}
		}

		#region Initialization

		internal void InitVars() 
		{

			_clsUserTable = (UserTable)this.Tables[clsPOSDBConstants.Users_tbl];
			if (_clsUserTable != null) 
			{
				_clsUserTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "UserData";
			this.Prefix = "";


			_clsUserTable = new UserTable();
			this.Tables.Add(this.User);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.Users_tbl] != null) 
			{
				this.Tables.Add(new UserTable(ds.Tables[clsPOSDBConstants.Users_tbl]));
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
