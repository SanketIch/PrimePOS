// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemPriceValidationData : DataSet {

		private ItemPriceValidationTable _ItemPriceValidationTable;

		#region Constructors
		public ItemPriceValidationData() {
			this.InitClass();

			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}


		public ItemPriceValidationTable ItemPriceValidation {
			get {
				return this._ItemPriceValidationTable;
			}
			set {
				this._ItemPriceValidationTable = value;
			}
		}
		#endregion

		public override DataSet Clone() {
			ItemPriceValidationData cln = (ItemPriceValidationData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_ItemPriceValidationTable = (ItemPriceValidationTable)this.Tables[clsPOSDBConstants.ItemPriceValidation_tbl];
			if (_ItemPriceValidationTable != null) {
			_ItemPriceValidationTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "ItemPriceValidationData";
			this.Prefix = "";
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_ItemPriceValidationTable = new ItemPriceValidationTable();
			this.Tables.Add(this.ItemPriceValidation);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.ItemPriceValidation_tbl] != null) {
				this.Tables.Add(new ItemPriceValidationTable(ds.Tables[clsPOSDBConstants.ItemPriceValidation_tbl]));
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


    public delegate void ItemPriceValidationRowChangeEventHandler(object sender, ItemPriceValidationRowChangeEvent e);
  }

}
