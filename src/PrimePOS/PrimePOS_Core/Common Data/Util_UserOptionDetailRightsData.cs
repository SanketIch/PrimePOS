using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_Core.CommonData
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.CommonData.Tables;

    public class Util_UserOptionDetailRightsData:DataSet
    {
        Util_UserOptionDetailRightsTable _Util_UserOptionDetailRightsTable;

        public Util_UserOptionDetailRightsData() 
		{
			this.InitClass();
		}
        private void InitClass()
        {
            this.DataSetName = clsPOSDBConstants.Util_UserOptionDetailRights_tbl;
            this.Prefix = "";


            _Util_UserOptionDetailRightsTable = new Util_UserOptionDetailRightsTable();
            this.Tables.Add(this.Util_UserOptionDetailRights);

        }
        public Util_UserOptionDetailRightsTable Util_UserOptionDetailRights
        {
            get
            {
                return this._Util_UserOptionDetailRightsTable;
            }
            set
            {
                this._Util_UserOptionDetailRightsTable = value;
            }
        }

        public override DataSet Clone()
        {
            Util_UserOptionDetailRightsData cln = (Util_UserOptionDetailRightsData)base.Clone();
            cln.InitVars();
            return cln;
        }
        
        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.Util_UserOptionDetailRights_tbl] != null)
            {
                this.Tables.Add(new PayOutTable(ds.Tables[clsPOSDBConstants.Util_UserOptionDetailRights_tbl]));
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

        
         internal void InitVars()
        {

            _Util_UserOptionDetailRightsTable = (Util_UserOptionDetailRightsTable)this.Tables[clsPOSDBConstants.Util_UserOptionDetailRights_tbl];
            if (_Util_UserOptionDetailRightsTable != null)
            {
                _Util_UserOptionDetailRightsTable.InitVars();
            }

        }

    }
}
