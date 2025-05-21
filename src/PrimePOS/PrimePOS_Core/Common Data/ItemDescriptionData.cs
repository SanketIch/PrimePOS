
namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemDescriptionData : DataSet {

		#region Constructors
		public ItemDescriptionData() {
			this.InitClass();
		}

		#endregion
		private ItemDescriptionTable _ItemDescriptionTable;

		public ItemDescriptionTable ItemDescription {
			get {
				return this._ItemDescriptionTable;
			}
			set {
				this._ItemDescriptionTable = value;
			}
		}

		public override DataSet Clone() {
			ItemDescriptionData cln = (ItemDescriptionData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_ItemDescriptionTable = (ItemDescriptionTable)this.Tables[clsPOSDBConstants.ItemDescription_tbl];
			if (_ItemDescriptionTable != null) {
			_ItemDescriptionTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = clsPOSDBConstants.ItemDescription_tbl;
			this.Prefix = "";


			_ItemDescriptionTable = new ItemDescriptionTable();
			this.Tables.Add(this.ItemDescription);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.ItemDescription_tbl] != null) {
				this.Tables.Add(new ItemDescriptionTable(ds.Tables[clsPOSDBConstants.ItemDescription_tbl]));
			}


			this.DataSetName = clsPOSDBConstants.ItemDescription_tbl;
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
