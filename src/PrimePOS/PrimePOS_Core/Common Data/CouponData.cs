using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData
{
    public class CouponData : DataSet
    {
        private CouponTable _CouponTable;

        #region Constructors
        public CouponData()
        {
            this.InitClass();
        }

        #endregion
        public CouponTable Coupon
        {
            get
            {
                return this._CouponTable;
            }
            set
            {
                this._CouponTable = value;
            }
        }
        public override DataSet Clone()
        {
            CouponData cln = (CouponData)base.Clone();
            cln.InitVars();
            return cln;
        }
        #region Initialization

        internal void InitVars()
        {

            _CouponTable = (CouponTable)this.Tables[clsPOSDBConstants.Coupon_tbl];
            if (_CouponTable != null)
            {
                _CouponTable.InitVars();
            }

        }

        private void InitClass()
        {
            this.DataSetName = clsPOSDBConstants.Coupon_tbl;
            this.Prefix = "";
            _CouponTable = new CouponTable();
            this.Tables.Add(this.Coupon);

        }

        private void InitClass(DataSet ds)
        {

            if (ds.Tables[clsPOSDBConstants.Coupon_tbl] != null)
            {
                this.Tables.Add(new PayOutTable(ds.Tables[clsPOSDBConstants.Coupon_tbl]));
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

