// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemGroupPriceData : DataSet {

		private ItemGroupPriceTable _ItemGroupPriceTable;

		#region Constructors

		public ItemGroupPriceData() {
			this.InitClass();
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}

		#endregion


		public ItemGroupPriceTable ItemGroupPrice {
			get {
				return this._ItemGroupPriceTable;
			}
			set {
				this._ItemGroupPriceTable = value;
			}
		}

		public override DataSet Clone() {
			ItemGroupPriceData cln = (ItemGroupPriceData)base.Clone();
			cln.InitVars();
			return cln;
		}



		#region Initialization

		internal void InitVars() {

		_ItemGroupPriceTable = (ItemGroupPriceTable)this.Tables[clsPOSDBConstants.ItemGroupPrice_tbl];
			if (_ItemGroupPriceTable != null) {
			_ItemGroupPriceTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "ItemGroupPriceData";
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_ItemGroupPriceTable = new ItemGroupPriceTable();
			this.Tables.Add(this.ItemGroupPrice);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.ItemGroupPrice_tbl] != null) {
				this.Tables.Add(new ItemGroupPriceTable(ds.Tables[clsPOSDBConstants.ItemGroupPrice_tbl]));
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


    public delegate void ItemGroupPriceRowChangeEventHandler(object sender, ItemGroupPriceRowChangeEvent e);
  }

}
