using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using System.ComponentModel;

namespace POS_Core.CommonData
{
    public class CCCustomerTokInfoData :DataSet
    {
        private CCCustomerTokInfoTable _CCCustomerTokInfoTable;

        #region "Constructors"

        public CCCustomerTokInfoData()
        {
            this.InitClass();
        }

        #endregion


        public CCCustomerTokInfoTable CCCustomerTokInfo
        {
            get
            {
                return this._CCCustomerTokInfoTable;
            }
            set
            {
                this._CCCustomerTokInfoTable = value;
            }
        }

        public override DataSet Clone()
        {
            CCCustomerTokInfoData cln = (CCCustomerTokInfoData)base.Clone();
            cln.InitVars();
            return cln;
        }

        #region "Initializtion"

        internal void InitVars()
        {
            _CCCustomerTokInfoTable = (CCCustomerTokInfoTable)this.Tables[clsPOSDBConstants.CCCustomerTokInfo_tbl];
            if(_CCCustomerTokInfoTable != null)
            {
                _CCCustomerTokInfoTable.InitVars();
            }
        }

        private void InitClass()
        {
            this.DataSetName = "CCCustomerTokInfoData";
            this.Prefix = "";

            _CCCustomerTokInfoTable = new CCCustomerTokInfoTable();
            this.Tables.Add(this.CCCustomerTokInfo);
        }

        private void InitClass(DataSet ds)
        {
            if(ds.Tables[clsPOSDBConstants.CCCustomerTokInfo_tbl]!=null)
            {
                this.Tables.Add(new CCCustomerTokInfoTable(ds.Tables[clsPOSDBConstants.CCCustomerTokInfo_tbl]));
            }

            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, MissingSchemaAction.Add);
            this.InitVars();
        }

        #endregion

        private void SchemaChanged(object sender, CollectionChangeEventArgs e)
        {
            if(e.Action == CollectionChangeAction.Remove)
            {
                this.InitVars();
            }
        }


    }
}
