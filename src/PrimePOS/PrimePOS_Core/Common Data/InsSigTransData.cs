//----------------------------------------------------------------------------------------------------
//PRIMEPOS-2339 04-Oct-2016 JY Added to maintain InsSigTrans
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class InsSigTransData : DataSet
    {
        private InsSigTransTable _InsSigTransTable;

        #region Constructors
        public InsSigTransData()
        {
            this.InitClass();
        }
        #endregion

        public InsSigTransTable InsSigTrans
        {
            get
            {
                return this._InsSigTransTable;
            }
            set
            {
                this._InsSigTransTable = value;
            }
        }

        public override DataSet Clone()
        {
            InsSigTransData cln = (InsSigTransData)base.Clone();
            cln.InitVars();
            return cln;
        }

        #region Initialization
        internal void InitVars()
        {
            _InsSigTransTable = (InsSigTransTable)this.Tables[clsPOSDBConstants.InsSigTrans_tbl];
            if (_InsSigTransTable != null)
            {
                _InsSigTransTable.InitVars();
            }
        }

        private void InitClass()
        {
            this.DataSetName = "InsSigTransData";
            this.Prefix = "";

            _InsSigTransTable = new InsSigTransTable();
            this.Tables.Add(this.InsSigTrans);
        }

        private void InitClass(DataSet ds)
        {
            if (ds.Tables[clsPOSDBConstants.InsSigTrans_tbl] != null)
            {
                this.Tables.Add(new InsSigTransTable(ds.Tables[clsPOSDBConstants.InsSigTrans_tbl]));
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
