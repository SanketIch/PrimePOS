//Sprint-22 - PRIMEPOS-2245 15-Oct-2015 JY Added class
namespace POS_Core.CommonData
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class SystemInfoData : DataSet 
    {
        private SystemInfoTable _SystemInfoTable;

		#region Constructors
        public SystemInfoData() 
		{
			this.InitClass();
		}
		#endregion


        public SystemInfoTable SystemInfo
		{
			get 
			{
                return this._SystemInfoTable;
			}
			set 
			{
                this._SystemInfoTable = value;
			}
		}

		public override DataSet Clone() 
		{
            SystemInfoData cln = (SystemInfoData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization
		internal void InitVars() 
		{
            _SystemInfoTable = (SystemInfoTable)this.Tables[clsPOSDBConstants.SystemInfo_tbl];
            if (_SystemInfoTable != null) 
			{
                _SystemInfoTable.InitVars();
			}
		}

		private void InitClass() 
		{
            this.DataSetName = "SystemInfoData";
			this.Prefix = "";


            _SystemInfoTable = new SystemInfoTable();
            this.Tables.Add(this.SystemInfo);
		}

		private void InitClass(DataSet ds) 
		{
			if (ds.Tables[clsPOSDBConstants.SystemInfo_tbl] != null) 
			{
				this.Tables.Add(new CLCardsTable(ds.Tables[clsPOSDBConstants.SystemInfo_tbl]));
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
