
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class PhysicalInvData : DataSet 
	{

		private PhysicalInvTable _PhysicalInvTable;

		#region Constructors
		public PhysicalInvData() 
		{
			this.InitClass();
		}

		#endregion


		public PhysicalInvTable PhysicalInv 
		{
			get 
			{
				return this._PhysicalInvTable;
			}
			set 
			{
				this._PhysicalInvTable = value;
			}
		}

		public override DataSet Clone() 
		{
			PhysicalInvData cln = (PhysicalInvData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_PhysicalInvTable = (PhysicalInvTable)this.Tables[clsPOSDBConstants.PhysicalInv_tbl];
			if (_PhysicalInvTable != null) 
			{
				_PhysicalInvTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.PhysicalInv_tbl;
			this.Prefix = "";


			_PhysicalInvTable = new PhysicalInvTable();
			this.Tables.Add(this.PhysicalInv);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.PhysicalInv_tbl] != null) 
			{
				this.Tables.Add(new PhysicalInvTable(ds.Tables[clsPOSDBConstants.PhysicalInv_tbl]));
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
