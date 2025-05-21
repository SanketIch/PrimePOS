using POS_Core.CommonData.Tables;
using System.Data;

namespace POS_Core.CommonData
{
    public class PSEItemData : DataSet
    {
        #region Constructors
        public PSEItemData()
        {
            this.InitClass();
        }
        #endregion
        
        private PSEItemTable _PSEItemTable;

        public PSEItemTable PSEItem
        {
            get
            {
                return this._PSEItemTable;
            }
            set
            {
                this._PSEItemTable = value;
            }
        }

        public override DataSet Clone()
        {
            PSEItemData cln = (PSEItemData)base.Clone();
            cln.InitVars();
            return cln;
        }

        #region Initialization
        internal void InitVars()
        {
            _PSEItemTable = (PSEItemTable)this.Tables[clsPOSDBConstants.PSE_Items_tbl];
            if (_PSEItemTable != null)
            {
                _PSEItemTable.InitVars();
            }
        }

        private void InitClass()
        {
            this.DataSetName = clsPOSDBConstants.PSE_Items_tbl;
            this.Prefix = "";

            _PSEItemTable = new PSEItemTable();
            this.Tables.Add(this.PSEItem);
        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.PSE_Items_tbl] != null)
            {
                this.Tables.Add(new PSEItemTable(ds.Tables[clsPOSDBConstants.PSE_Items_tbl]));
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
