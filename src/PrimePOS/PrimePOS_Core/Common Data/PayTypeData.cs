
namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class PayTypeData : DataSet {

		#region Constructors
		public PayTypeData() {
			this.InitClass();
		}

		#endregion
		private PayTypeTable _PayTypeTable;

		public PayTypeTable PayTypes {
			get {
				return this._PayTypeTable;
			}
			set {
				this._PayTypeTable = value;
			}
		}

		public override DataSet Clone() {
			PayTypeData cln = (PayTypeData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() {

		_PayTypeTable = (PayTypeTable)this.Tables[clsPOSDBConstants.PayType_tbl];
			if (_PayTypeTable != null) {
			_PayTypeTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = clsPOSDBConstants.PayType_tbl;
			this.Prefix = "";


			_PayTypeTable = new PayTypeTable();
			this.Tables.Add(this.PayTypes);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[clsPOSDBConstants.PayType_tbl] != null) {
				this.Tables.Add(new PayTypeTable(ds.Tables[clsPOSDBConstants.PayType_tbl]));
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
