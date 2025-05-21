
namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TaxCodesData : DataSet {

		#region Constructors
		public TaxCodesData() {
			this.InitClass();
		}

		#endregion
		private TaxCodesTable _TaxCodesTable;

		public TaxCodesTable TaxCodes {
			get {
				return this._TaxCodesTable;
			}
			set {
				this._TaxCodesTable = value;
			}
		}

		public override DataSet Clone() {
			TaxCodesData cln = (TaxCodesData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_TaxCodesTable = (TaxCodesTable)this.Tables[clsPOSDBConstants.TaxCodes_tbl];
			if (_TaxCodesTable != null) {
			_TaxCodesTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = clsPOSDBConstants.TaxCodes_tbl;
			this.Prefix = "";


			_TaxCodesTable = new TaxCodesTable();
			this.Tables.Add(this.TaxCodes);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.TaxCodes_tbl] != null) {
				this.Tables.Add(new TaxCodesTable(ds.Tables[clsPOSDBConstants.TaxCodes_tbl]));
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
