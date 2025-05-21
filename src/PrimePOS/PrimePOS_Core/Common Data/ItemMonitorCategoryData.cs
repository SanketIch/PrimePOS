
namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemMonitorCategoryData : DataSet {

		#region Constructors
		public ItemMonitorCategoryData() {
			this.InitClass();
		}

		#endregion
		private ItemMonitorCategoryTable _ItemMonitorCategoryTable;

		public ItemMonitorCategoryTable ItemMonitorCategory {
			get {
				return this._ItemMonitorCategoryTable;
			}
			set {
				this._ItemMonitorCategoryTable = value;
			}
		}

		public override DataSet Clone() {
			ItemMonitorCategoryData cln = (ItemMonitorCategoryData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_ItemMonitorCategoryTable = (ItemMonitorCategoryTable)this.Tables[clsPOSDBConstants.ItemMonitorCategory_tbl];
			if (_ItemMonitorCategoryTable != null) {
			_ItemMonitorCategoryTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = clsPOSDBConstants.ItemMonitorCategory_tbl;
			this.Prefix = "";


			_ItemMonitorCategoryTable = new ItemMonitorCategoryTable();
			this.Tables.Add(this.ItemMonitorCategory);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.ItemMonitorCategory_tbl] != null) {
				this.Tables.Add(new ItemMonitorCategoryTable(ds.Tables[clsPOSDBConstants.ItemMonitorCategory_tbl]));
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
