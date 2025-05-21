namespace POS_Core.CommonData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data;
    using POS_Core.CommonData.Tables;

    public class MsgTemplateData : DataSet
    {
        private MsgTemplateTable _clsMsgTemplateTable;

        #region Constructors
        public MsgTemplateData()
        {
            this.InitClass();
        }
        #endregion

        public override DataSet Clone()
        {
            MsgTemplateData cln = (MsgTemplateData)base.Clone();
            cln.InitVars();
            return cln;
        }

        public MsgTemplateTable MsgTemplate
        {
            get
            {
                return this._clsMsgTemplateTable;
            }
            set
            {
                this._clsMsgTemplateTable = value;
            }
        }

        #region Initialization
        internal void InitVars()
        {
            _clsMsgTemplateTable = (MsgTemplateTable)this.Tables[clsPOSDBConstants.FMessage_tbl];
            if (_clsMsgTemplateTable != null)
            {
                _clsMsgTemplateTable.InitVars();
            }
        }

        private void InitClass()
        {
            this.DataSetName = "MsgTemplateData";
            this.Prefix = "";

            _clsMsgTemplateTable = new MsgTemplateTable();
            this.Tables.Add(this.MsgTemplate);
        }

        private void InitClass(DataSet ds)
        {
            if (ds.Tables[clsPOSDBConstants.FMessage_tbl] != null)
            {
                this.Tables.Add(new MsgTemplateTable(ds.Tables[clsPOSDBConstants.FMessage_tbl]));
            }

            this.DataSetName = ds.DataSetName;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        #endregion
    }
}
