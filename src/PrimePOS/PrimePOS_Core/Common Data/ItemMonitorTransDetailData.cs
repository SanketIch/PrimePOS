//----------------------------------------------------------------------------------------------------
//Sprint-23 - PRIMEPOS-2029 19-Apr-2016 JY Added to maintain item monitor trans log
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class ItemMonitorTransDetailData : DataSet
    {
        private ItemMonitorTransDetailTable _ItemMonitorTransDetailTable;
        
		#region Constructors
        public ItemMonitorTransDetailData() 
		{
			this.InitClass();
		}
		#endregion

        public ItemMonitorTransDetailTable ItemMonitorTransDetail
		{
			get 
			{
				return this._ItemMonitorTransDetailTable;
			}
			set 
			{
				this._ItemMonitorTransDetailTable = value;
			}
		}

		public override DataSet Clone() 
		{
			ItemMonitorTransDetailData cln = (ItemMonitorTransDetailData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization
		internal void InitVars() 
		{
            _ItemMonitorTransDetailTable = (ItemMonitorTransDetailTable)this.Tables[clsPOSDBConstants.ItemMonitorTransDetail_tbl];
			if (_ItemMonitorTransDetailTable != null) 
			{
				_ItemMonitorTransDetailTable.InitVars();
			}
		}

		private void InitClass() 
		{
            this.DataSetName = "ItemMonitorTransDetailData";
			this.Prefix = "";

			_ItemMonitorTransDetailTable = new ItemMonitorTransDetailTable();
			this.Tables.Add(this.ItemMonitorTransDetail);
		}

		private void InitClass(DataSet ds) 
		{
			if (ds.Tables[clsPOSDBConstants.ItemMonitorTransDetail_tbl] != null) 
			{
				this.Tables.Add(new ItemMonitorTransDetailTable(ds.Tables[clsPOSDBConstants.ItemMonitorTransDetail_tbl]));
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
