namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    public class CouponTable : DataTable
    {
        private DataColumn colCouponID;
        private DataColumn colCouponCode;
        private DataColumn colStartDate;
        private DataColumn colEndDate;
        private DataColumn colDiscountPerc;
        private DataColumn colUserID;
        private DataColumn colCreatedDate;
        private DataColumn colIsCouponInPercent;
        private DataColumn colCouponDesc;    //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added

         #region Constructors

        internal CouponTable() : base(clsPOSDBConstants.Coupon_tbl) { this.InitClass(); }

        internal CouponTable(DataTable table) : base(table.TableName) { }

        #endregion Constructors
          public override DataTable Clone()
        {
            CouponTable cln = (CouponTable)base.Clone();
            cln.InitVars();
            return cln;
        }
          
        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public CouponRow this[int index]
        {
            get
            {
                return ((CouponRow) (this.Rows[index]));
            }
        }

        public DataColumn CouponID
        {
            get
            {
                return this.colCouponID;
            }
            
        }

        public DataColumn CouponCode
        {
            get
            {
                return this.colCouponCode;
            }
            
        }
       
        public DataColumn EndDate
        {
            get
            {
                return this.colEndDate;
            }
            
        }
        public DataColumn StartDate
        {
            get
            {
                return this.colStartDate;
            }
            
        }

        public DataColumn CreatedDate
        {
            get
            {
                return this.colCreatedDate;
            }
            
        }
        public DataColumn DiscountPerc
        {
            get
            {
                return this.colDiscountPerc;
            }
            
        }
        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
            
        }

        public DataColumn IsCouponInPercent
        {
            get
            {
                return this.colIsCouponInPercent;

            }
        }

        //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
        public DataColumn CouponDesc
        {
            get
            {
                return this.colCouponDesc;
            }

        }
        #endregion Properties

        #region Add and Get Methods
         public void AddRow(CouponRow row)
        {
            AddRow(row, false);
        }
         public CouponRow GetRowByID(System.Int32 CouponID)
         {
             return (CouponRow)this.Rows.Find(new object[] { CouponID });
         }
        public void AddRow(CouponRow row, bool preserveChanges)
        {
            if(this.GetRowByID(row.CouponID) == null)
            {
                this.Rows.Add(row);
                if(!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public CouponRow AddRow(System.Int32 CouponID, System.String CouponCode, System.String UserID, DateTime CreatedDate, Boolean IsCouponInPercent, System.String CouponDesc)   //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added CouponDesc parameter
        {
            CouponRow row = (CouponRow)this.NewRow();            
            row.CouponID = CouponID;        
            row.CouponCode = CouponCode;          
            row.UserID = UserID;
            row.CreatedDate = CreatedDate;
            row.IsCouponInPercent = IsCouponInPercent;
            row.CouponDesc = CouponDesc;    //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
            this.Rows.Add(row);
            return row;
        }
        public void MergeTable(DataTable dt)
        {
            CouponRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (CouponRow)this.NewRow();

                if (dr[clsPOSDBConstants.Coupon_Fld_CouponID] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_CouponID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Coupon_Fld_CouponID] = Convert.ToInt32((dr[clsPOSDBConstants.Coupon_Fld_CouponID].ToString() == "") ? "0" : dr[clsPOSDBConstants.Coupon_Fld_CouponID].ToString());

                if (dr[clsPOSDBConstants.Coupon_Fld_CouponCode] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_CouponCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Coupon_Fld_CouponCode] = ((dr[clsPOSDBConstants.Coupon_Fld_CouponCode].ToString() == "") ? "" : dr[clsPOSDBConstants.Coupon_Fld_CouponCode].ToString());

                if (dr[clsPOSDBConstants.Coupon_Fld_UserID] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_UserID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Coupon_Fld_UserID] = ((dr[clsPOSDBConstants.Coupon_Fld_UserID].ToString() == "") ? "" : dr[clsPOSDBConstants.Coupon_Fld_UserID].ToString());

                if (dr[clsPOSDBConstants.Coupon_Fld_StartDate] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_StartDate] = DBNull.Value;

                else if (dr[clsPOSDBConstants.Coupon_Fld_StartDate].ToString().Trim() == "")
                        row[clsPOSDBConstants.Coupon_Fld_StartDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                        row[clsPOSDBConstants.Coupon_Fld_StartDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Coupon_Fld_StartDate].ToString());

                if (dr[clsPOSDBConstants.Coupon_Fld_DiscountPerc] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_DiscountPerc] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Coupon_Fld_DiscountPerc] = Convert.ToDecimal((dr[clsPOSDBConstants.Coupon_Fld_DiscountPerc].ToString() == "") ? "0" : dr[clsPOSDBConstants.Coupon_Fld_DiscountPerc].ToString());

                if (dr[clsPOSDBConstants.Coupon_Fld_CreatedDate] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_CreatedDate] = DBNull.Value;

                else if (dr[clsPOSDBConstants.Coupon_Fld_CreatedDate].ToString().Trim() == "")
                        row[clsPOSDBConstants.Coupon_Fld_CreatedDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                        row[clsPOSDBConstants.Coupon_Fld_CreatedDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Coupon_Fld_CreatedDate].ToString());

                if (dr[clsPOSDBConstants.Coupon_Fld_EndDate] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_EndDate] = DBNull.Value;

                else if (dr[clsPOSDBConstants.Coupon_Fld_EndDate].ToString().Trim() == "")
                        row[clsPOSDBConstants.Coupon_Fld_EndDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                        row[clsPOSDBConstants.Coupon_Fld_EndDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Coupon_Fld_EndDate].ToString());

                if (dr[clsPOSDBConstants.Coupon_Fld_IsCouponInPercent] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_IsCouponInPercent] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Coupon_Fld_IsCouponInPercent] = POS_Core.Resources.Configuration.convertNullToBoolean(dr[clsPOSDBConstants.Coupon_Fld_IsCouponInPercent].ToString());

                //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
                if (dr[clsPOSDBConstants.Coupon_Fld_CouponDesc] == DBNull.Value)
                    row[clsPOSDBConstants.Coupon_Fld_CouponDesc] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Coupon_Fld_CouponDesc] = ((dr[clsPOSDBConstants.Coupon_Fld_CouponDesc].ToString() == "") ? "" : dr[clsPOSDBConstants.Coupon_Fld_CouponDesc].ToString());

                this.AddRow(row);
            }
        }
        protected override DataTable CreateInstance()
        {
            return new CouponTable();
        }
       
        internal void InitVars()
        {
            this.colCouponID = this.Columns[clsPOSDBConstants.Coupon_Fld_CouponID];
            this.colCouponCode = this.Columns[clsPOSDBConstants.Coupon_Fld_CouponCode];          
            this.colDiscountPerc = this.Columns[clsPOSDBConstants.Coupon_Fld_DiscountPerc]; 
            this.colStartDate = this.Columns[clsPOSDBConstants.Coupon_Fld_StartDate];
            this.colEndDate = this.Columns[clsPOSDBConstants.Coupon_Fld_EndDate];
            this.colCreatedDate = this.Columns[clsPOSDBConstants.Coupon_Fld_CreatedDate];
            this.colUserID = this.Columns[clsPOSDBConstants.Coupon_Fld_UserID];
            this.colIsCouponInPercent = this.Columns[clsPOSDBConstants.Coupon_Fld_IsCouponInPercent];
            this.colCouponDesc= this.Columns[clsPOSDBConstants.Coupon_Fld_CouponDesc]; //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added          
        }
         private void InitClass()
         {
             this.colCouponID = new DataColumn(clsPOSDBConstants.Coupon_Fld_CouponID, typeof(System.Int32), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colCouponID);
             this.colCouponID.AllowDBNull = false;

             this.colCouponCode = new DataColumn(clsPOSDBConstants.Coupon_Fld_CouponCode, typeof(System.String), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colCouponCode);
             this.colCouponCode.AllowDBNull = false;

             this.colDiscountPerc = new DataColumn(clsPOSDBConstants.Coupon_Fld_DiscountPerc, typeof(System.Decimal), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colDiscountPerc);
             this.colDiscountPerc.AllowDBNull = true;

             this.colStartDate = new DataColumn(clsPOSDBConstants.Coupon_Fld_StartDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colStartDate);
             this.colStartDate.AllowDBNull = true;

             this.colEndDate = new DataColumn(clsPOSDBConstants.Coupon_Fld_EndDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colEndDate);
             this.colEndDate.AllowDBNull = true;
             
             this.colUserID = new DataColumn(clsPOSDBConstants.Coupon_Fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colUserID);
             this.colUserID.AllowDBNull = true;

             this.colCreatedDate = new DataColumn(clsPOSDBConstants.Coupon_Fld_CreatedDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colCreatedDate);
             this.colCreatedDate.AllowDBNull = true;

             this.colIsCouponInPercent = new DataColumn(clsPOSDBConstants.Coupon_Fld_IsCouponInPercent, typeof(System.Boolean), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colIsCouponInPercent);
             this.colIsCouponInPercent.AllowDBNull = true;

             //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
             this.colCouponDesc = new DataColumn(clsPOSDBConstants.Coupon_Fld_CouponDesc, typeof(System.String), null, System.Data.MappingType.Element);
             this.Columns.Add(this.colCouponDesc);
             this.colCouponDesc.AllowDBNull = false;

             this.PrimaryKey = new DataColumn[] { this.colCouponID };

         }

         public CouponRow NewCouponRow()
         {
             return (CouponRow)this.NewRow();
         }

         protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
         {
             return new CouponRow(builder);
         }

         protected override System.Type GetRowType()
         {
             return typeof(CouponRow);
         }
        #endregion
    }
}
