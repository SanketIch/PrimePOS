using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData
{
    // StoreCredit - PRIMEPOS-2747 - NileshJ
    public partial class StoreCreditData : DataSet
    {
        private StoreCreditTable _StoreCreditTable;
        #region Constructors
        public StoreCreditData()
        {
            this.InitClass();
        }
        #endregion

        public StoreCreditTable StoreCredit {
            get {
                return this._StoreCreditTable;
            }
            set {
                this._StoreCreditTable = value;
            }
        }

        public override DataSet Clone() {
            StoreCreditData cln = (StoreCreditData)base.Clone();
            cln.InitVars();
            return cln;
        }

        #region Initialization

        internal void InitVars() {

            _StoreCreditTable = (StoreCreditTable)this.Tables[clsPOSDBConstants.StoreCredit_tbl];
            if (_StoreCreditTable != null) {
                _StoreCreditTable.InitVars();
            }

        }

        private void InitClass() {
            this.DataSetName = "StoreCreditData";
            this.Prefix = "";


            _StoreCreditTable = new StoreCreditTable();
            this.Tables.Add(this.StoreCredit);

        }

        private void InitClass(DataSet ds) {

            if (ds.Tables[clsPOSDBConstants.StoreCredit_tbl] != null) {
                this.Tables.Add(new StoreCreditTable(ds.Tables[clsPOSDBConstants.StoreCredit_tbl]));
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
