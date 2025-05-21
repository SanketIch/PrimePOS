// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemCompanionData : DataSet {

		private ItemCompanionTable _ItemCompanionTable;

		#region Constructors
		public ItemCompanionData() {
			this.InitClass();

			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}


		public ItemCompanionTable ItemCompanion {
			get {
				return this._ItemCompanionTable;
			}
			set {
				this._ItemCompanionTable = value;
			}
		}
		#endregion

		public override DataSet Clone() {
			ItemCompanionData cln = (ItemCompanionData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_ItemCompanionTable = (ItemCompanionTable)this.Tables[clsPOSDBConstants.ItemCompanion_tbl];
			if (_ItemCompanionTable != null) {
			_ItemCompanionTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "ItemCompanionData";
			this.Prefix = "";
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_ItemCompanionTable = new ItemCompanionTable();
			this.Tables.Add(this.ItemCompanion);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.ItemCompanion_tbl] != null) {
				this.Tables.Add(new ItemCompanionTable(ds.Tables[clsPOSDBConstants.ItemCompanion_tbl]));
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


    public delegate void ItemCompanionRowChangeEventHandler(object sender, ItemCompanionRowChangeEvent e);
  }

}
