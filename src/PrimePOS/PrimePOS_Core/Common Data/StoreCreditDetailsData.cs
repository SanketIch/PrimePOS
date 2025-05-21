using POS_Core.CommonData.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.CommonData
{
    // StoreCredit - PRIMEPOS-2747 - NileshJ
    public partial class StoreCreditDetailsData : DataSet
    {
        private StoreCreditDetailsTable _StoreCreditDetailsTable;
        #region Constructors
        public StoreCreditDetailsData() 
        {
            this.InitClass();
        }
        #endregion

        public StoreCreditDetailsTable StoreCreditDetails {
            get {
                return this._StoreCreditDetailsTable;
            }
            set {
                this._StoreCreditDetailsTable = value;
            }
        }

        public override DataSet Clone() {
            StoreCreditDetailsData cln = (StoreCreditDetailsData)base.Clone();
            cln.InitVars();
            return cln;
        }

        #region Initialization

        internal void InitVars() {

            _StoreCreditDetailsTable = (StoreCreditDetailsTable)this.Tables[clsPOSDBConstants.StoreCreditDetails_tbl];
            if (_StoreCreditDetailsTable != null) {
                _StoreCreditDetailsTable.InitVars();
            }

        }

        private void InitClass() {
            this.DataSetName = "StoreCreditDetailsData";
            this.Prefix = "";


            _StoreCreditDetailsTable = new StoreCreditDetailsTable();
            this.Tables.Add(this.StoreCreditDetails);

        }

        private void InitClass(DataSet ds) {

            if (ds.Tables[clsPOSDBConstants.StoreCreditDetails_tbl] != null) {
                this.Tables.Add(new StoreCreditDetailsTable(ds.Tables[clsPOSDBConstants.StoreCreditDetails_tbl]));
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
