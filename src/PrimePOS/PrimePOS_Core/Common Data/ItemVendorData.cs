// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemVendorData : DataSet {

		private ItemVendorTable _ItemVendorTable;

		#region Constructors
		public ItemVendorData() {
			this.InitClass();
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}

		#endregion


		public ItemVendorTable ItemVendor {
			get {
				return this._ItemVendorTable;
			}
			set {
				this._ItemVendorTable = value;
			}
		}

		/// <summary>
		///     override DataSet Clone() methods.
		///     <remarks>Copies the structure of the ItemVendorData, including all DataTable schemas, relations, and constraints. Does not copy any data.</remarks> 
		/// </summary>
		public override DataSet Clone() {
			ItemVendorData cln = (ItemVendorData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization

		internal void InitVars() {

		_ItemVendorTable = (ItemVendorTable)this.Tables[clsPOSDBConstants.ItemVendor_tbl];
			if (_ItemVendorTable != null) {
			_ItemVendorTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "ItemVendorData";
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_ItemVendorTable = new ItemVendorTable();
			this.Tables.Add(this.ItemVendor);

		}

		private void InitClass(DataSet ds) {
			
			if (ds.Tables[clsPOSDBConstants.ItemVendor_tbl] != null) {
				this.Tables.Add(new ItemVendorTable(ds.Tables[clsPOSDBConstants.ItemVendor_tbl]));
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


    public delegate void ItemVendorRowChangeEventHandler(object sender, ItemVendorRowChangeEvent e);
  }

}
