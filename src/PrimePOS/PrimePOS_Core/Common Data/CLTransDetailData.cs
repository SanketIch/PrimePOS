//----------------------------------------------------------------------------------------------------
//Sprint-18 - 2090 07-Oct-2014 JY Added common data class for CL_TransDetail table
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

    public class CLTransDetailData : DataSet 
    {
		private CLTransDetailTable _CLTransDetailTable;
        
		#region Constructors
        public CLTransDetailData() 
		{
			this.InitClass();
		}
		#endregion

        public CLTransDetailTable CLTransDetail
		{
			get 
			{
				return this._CLTransDetailTable;
			}
			set 
			{
				this._CLTransDetailTable = value;
			}
		}

		public override DataSet Clone() 
		{
			CLTransDetailData cln = (CLTransDetailData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization
		internal void InitVars() 
		{
			_CLTransDetailTable = (CLTransDetailTable)this.Tables[clsPOSDBConstants.CLTransDetail_tbl];
			if (_CLTransDetailTable != null) 
			{
				_CLTransDetailTable.InitVars();
			}
		}

		private void InitClass() 
		{
            this.DataSetName = "CLTransDetailData";
			this.Prefix = "";

			_CLTransDetailTable = new CLTransDetailTable();
			this.Tables.Add(this.CLTransDetail);
		}

		private void InitClass(DataSet ds) 
		{
			if (ds.Tables[clsPOSDBConstants.CLTransDetail_tbl] != null) 
			{
				this.Tables.Add(new CLTransDetailTable(ds.Tables[clsPOSDBConstants.CLTransDetail_tbl]));
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
