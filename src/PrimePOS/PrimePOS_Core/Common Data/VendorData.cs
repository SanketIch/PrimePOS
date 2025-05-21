// ----------------------------------------------------------------
// Author: adeel shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	//     This class is used to define the shape of VendorData.
	
	public class VendorData : DataSet 
	{

		
		private VendorTable _clsVendorTable;

		#region Constants
		// The constant used for Vendor table.
		public const String K_VENDOR_TABLE  = "tblVendor";

		#endregion

		#region Constructors
		//     Constructor for VendorData.

		public VendorData() 
		{
			this.InitClass();
		}

		#endregion

		public override DataSet Clone() 
		{
			VendorData cln = (VendorData)base.Clone();
			cln.InitVars();
			return cln;
		}


		public VendorTable Vendor 
		{
			get 
			{
				return this._clsVendorTable;
			}
			set 
			{
				this._clsVendorTable = value;
			}
		}

		#region Initialization

		internal void InitVars() 
		{

			_clsVendorTable = (VendorTable)this.Tables[K_VENDOR_TABLE];
			if (_clsVendorTable != null) 
			{
				_clsVendorTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "VendorData";
			this.Prefix = "";


			_clsVendorTable = new VendorTable();
			this.Tables.Add(this.Vendor);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[K_VENDOR_TABLE] != null) 
			{
				this.Tables.Add(new VendorTable(ds.Tables[K_VENDOR_TABLE]));
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
