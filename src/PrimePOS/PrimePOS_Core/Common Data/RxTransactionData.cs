using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData;

namespace POS_Core.CommonData
{
    public class RxTransactionData : DataSet
    {

        private RxTransactionDataTable _RxTransactionDataTable;

        #region Constructors
        public RxTransactionData()
        {
            this.InitClass();
        }

        #endregion


        public RxTransactionDataTable RxTransaction
        {
            get
            {
                return this._RxTransactionDataTable;
            }
            set
            {
                this._RxTransactionDataTable = value;
            }
        }

        public override DataSet Clone()
        {
            RxTransactionData cln = (RxTransactionData)base.Clone();
            cln.InitVars();
            return cln;
        }


        #region Initialization

        internal void InitVars()
        {

            _RxTransactionDataTable = (RxTransactionDataTable)this.Tables[clsPOSDBConstants.RxTransactionData_tbl];
            if (_RxTransactionDataTable != null)
            {
                _RxTransactionDataTable.InitVars();
            }

        }

        private void InitClass()
        {
            this.DataSetName = clsPOSDBConstants.RxTransactionData_tbl;
            this.Prefix = "";


            _RxTransactionDataTable = new RxTransactionDataTable();
            this.Tables.Add(this.RxTransaction);

        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.RxTransactionData_tbl] != null)
            {
                this.Tables.Add(new RxTransactionDataTable(ds.Tables[clsPOSDBConstants.RxTransactionData_tbl]));
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

        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove))
            {
                this.InitVars();
            }
        }
     

    }

}

