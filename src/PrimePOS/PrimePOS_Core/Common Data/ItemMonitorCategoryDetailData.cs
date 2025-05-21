
namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemMonitorCategoryDetailData : DataSet {

		#region Constructors
		public ItemMonitorCategoryDetailData() {
			this.InitClass();
		}

		#endregion
		private ItemMonitorCategoryDetailTable _ItemMonitorCategoryDetailTable;

		public ItemMonitorCategoryDetailTable ItemMonitorCategoryDetail {
			get {
				return this._ItemMonitorCategoryDetailTable;
			}
			set {
				this._ItemMonitorCategoryDetailTable = value;
			}
		}

		public override DataSet Clone() {
			ItemMonitorCategoryDetailData cln = (ItemMonitorCategoryDetailData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_ItemMonitorCategoryDetailTable = (ItemMonitorCategoryDetailTable)this.Tables[clsPOSDBConstants.ItemMonitorCategoryDetail_tbl];
			if (_ItemMonitorCategoryDetailTable != null) {
			_ItemMonitorCategoryDetailTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = clsPOSDBConstants.ItemMonitorCategoryDetail_tbl;
			this.Prefix = "";


			_ItemMonitorCategoryDetailTable = new ItemMonitorCategoryDetailTable();
			this.Tables.Add(this.ItemMonitorCategoryDetail);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.ItemMonitorCategoryDetail_tbl] != null) {
				this.Tables.Add(new ItemMonitorCategoryDetailTable(ds.Tables[clsPOSDBConstants.ItemMonitorCategoryDetail_tbl]));
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

    private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
      if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
        this.InitVars();
      }
    }


   }

}
