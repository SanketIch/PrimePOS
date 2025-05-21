using POS_Core.CommonData.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.CommonData
{
    public class ConsentTransmissionData : DataSet
    {

        private ConsentTransmissionDataTable _ConsentTransmissionData;

        #region Constructors
        public ConsentTransmissionData()
        {
            this.InitClass();
        }

        #endregion


        public ConsentTransmissionDataTable ConsentTransmission
        {
            get
            {
                return this._ConsentTransmissionData;
            }
            set
            {
                this._ConsentTransmissionData = value;
            }
        }

        public override DataSet Clone()
        {
            ConsentTransmissionData cln = (ConsentTransmissionData)base.Clone();
            cln.InitVars();
            return cln;
        }


        #region Initialization

        internal void InitVars()
        {

            _ConsentTransmissionData = (ConsentTransmissionDataTable)this.Tables[clsPOSDBConstants.ConsentTransmissionData_tbl];
            if (_ConsentTransmissionData != null)
            {
                _ConsentTransmissionData.InitVars();
            }

        }

        private void InitClass()
        {
            this.DataSetName = clsPOSDBConstants.ConsentTransmissionData_tbl;
            this.Prefix = "";
            _ConsentTransmissionData = new ConsentTransmissionDataTable();
            this.Tables.Add(this.ConsentTransmission);
        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.ConsentTransmissionData_tbl] != null)
            {
                this.Tables.Add(new ConsentTransmissionDataTable(ds.Tables[clsPOSDBConstants.ConsentTransmissionData_tbl]));
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
