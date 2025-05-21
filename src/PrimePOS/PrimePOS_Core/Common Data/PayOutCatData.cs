//Shitaljit 20Feb2012

namespace POS_Core.CommonData 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class PayOutCatData:DataSet
    {
        private PayOutCatTable _PayOutCatTable;

		#region Constructors
		public PayOutCatData() 
		{
			this.InitClass();
		}

		#endregion
        public PayOutCatTable PayOutCat
        {
            get
            {
                return this._PayOutCatTable;
            }
            set
            {
                this._PayOutCatTable = value;
            }
        }
        public override DataSet Clone()
        {
            PayOutCatData cln = (PayOutCatData)base.Clone();
            cln.InitVars();
            return cln;
        }
        #region Initialization

        internal void InitVars()
        {

            _PayOutCatTable = (PayOutCatTable)this.Tables[clsPOSDBConstants.PayOutCat_tbl];
            if (_PayOutCatTable != null)
            {
                _PayOutCatTable.InitVars();
            }

        }

        private void InitClass()
        {
            this.DataSetName = clsPOSDBConstants.PayOutCat_tbl;
            this.Prefix = "";


            _PayOutCatTable = new PayOutCatTable();
            this.Tables.Add(this.PayOutCat);

        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.PayOutCat_tbl] != null)
            {
                this.Tables.Add(new PayOutTable(ds.Tables[clsPOSDBConstants.PayOutCat_tbl]));
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

    }
}
