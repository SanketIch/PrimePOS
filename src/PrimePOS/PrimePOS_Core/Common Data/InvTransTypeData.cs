
namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class InvTransTypeData : DataSet {

		#region Constructors
		public InvTransTypeData() {
			this.InitClass();
		}

		#endregion
		private InvTransTypeTable _InvTransTypeTable;

		public InvTransTypeTable InvTransType {
			get {
				return this._InvTransTypeTable;
			}
			set {
				this._InvTransTypeTable = value;
			}
		}

		public override DataSet Clone() {
			InvTransTypeData cln = (InvTransTypeData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_InvTransTypeTable = (InvTransTypeTable)this.Tables[clsPOSDBConstants.InvTransType_tbl];
			if (_InvTransTypeTable != null) {
			_InvTransTypeTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = clsPOSDBConstants.InvTransType_tbl;
			this.Prefix = "";


			_InvTransTypeTable = new InvTransTypeTable();
			this.Tables.Add(this.InvTransType);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.InvTransType_tbl] != null) {
				this.Tables.Add(new InvTransTypeTable(ds.Tables[clsPOSDBConstants.InvTransType_tbl]));
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
