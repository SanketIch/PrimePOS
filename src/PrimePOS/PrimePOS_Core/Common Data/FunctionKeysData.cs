// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class FunctionKeysData : DataSet {

		private FunctionKeysTable _FunctionKeysTable;

		#region Constructors

		public FunctionKeysData() {
			this.InitClass();

			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}
		#endregion


		public FunctionKeysTable FunctionKeys {
			get {
				return this._FunctionKeysTable;
			}
			set {
				this._FunctionKeysTable = value;
			}
		}

		/// <summary>
		///     override DataSet Clone() methods.
		///     <remarks>Copies the structure of the FunctionKeysData, including all DataTable schemas, relations, and constraints. Does not copy any data.</remarks> 
		/// </summary>
		public override DataSet Clone() {
			FunctionKeysData cln = (FunctionKeysData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization

		internal void InitVars() {

		_FunctionKeysTable = (FunctionKeysTable)this.Tables[clsPOSDBConstants.FunctionKeys_tbl];
			if (_FunctionKeysTable != null) {
			_FunctionKeysTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "FunctionKeysData";
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_FunctionKeysTable = new FunctionKeysTable();
			this.Tables.Add(this.FunctionKeys);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.FunctionKeys_tbl] != null) {
				this.Tables.Add(new FunctionKeysTable(ds.Tables[clsPOSDBConstants.FunctionKeys_tbl]));
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


    public delegate void FunctionKeysRowChangeEventHandler(object sender, FunctionKeysRowChangeEvent e);
  }

}
