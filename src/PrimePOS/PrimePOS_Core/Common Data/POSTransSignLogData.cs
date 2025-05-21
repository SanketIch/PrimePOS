
namespace POS_Core.CommonData
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class POSTransSignLogData : DataSet
    {

        private POSTransSignLogTable _POSTransSignLogTable;

        #region Constructors
        public POSTransSignLogData()
        {
            this.InitClass();
        }

        #endregion


        public POSTransSignLogTable POSTransSignLog
        {
            get
            {
                return this._POSTransSignLogTable;
            }
            set
            {
                this._POSTransSignLogTable = value;
            }
        }

        public override DataSet Clone()
        {
            POSTransSignLogData cln = (POSTransSignLogData)base.Clone();
            cln.InitVars();
            return cln;
        }


        #region Initialization

        internal void InitVars()
        {

            _POSTransSignLogTable = (POSTransSignLogTable)this.Tables[clsPOSDBConstants.POSTransPayment_tbl];
            if (_POSTransSignLogTable != null)
            {
                _POSTransSignLogTable.InitVars();
            }

        }

        private void InitClass()
        {
            this.DataSetName = "POSTransSignLogData";
            this.Prefix = "";


            _POSTransSignLogTable = new POSTransSignLogTable();
            this.Tables.Add(this.POSTransSignLog);

        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.POSTransPayment_tbl] != null)
            {
                this.Tables.Add(new POSTransSignLogTable(ds.Tables[clsPOSDBConstants.POSTransPayment_tbl]));
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
