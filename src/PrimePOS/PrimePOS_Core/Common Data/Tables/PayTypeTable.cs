namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class PayTypeTable : DataTable, System.Collections.IEnumerable
    {

        private DataColumn colPayTypeID;
        private DataColumn colPayTypeDesc;
        private DataColumn colPayType;
        private DataColumn colUserID;
        private DataColumn colIsHide;   //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added    
        private DataColumn colStopAtRefNo;  //PRIMEPOS-2309 08-Mar-2019 JY Added
        private DataColumn colSortOrder;    //PRIMEPOS-2966 20-May-2021 JY Added

        #region Constants
        private const String _TableName = "PayType";
        #endregion
        #region Constructors 
        internal PayTypeTable() : base(_TableName) { this.InitClass(); }
        internal PayTypeTable(DataTable table) : base(table.TableName) { }
        #endregion
        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public PayTypeRow this[int index]
        {
            get
            {
                return ((PayTypeRow)(this.Rows[index]));
            }
        }

        public DataColumn PaytypeID
        {
            get
            {
                return this.colPayTypeID;
            }
        }

        public DataColumn PayTypeDesc
        {
            get
            {
                return this.colPayTypeDesc;
            }
        }

        public DataColumn PayType
        {
            get
            {
                return this.colPayType;
            }
        }

        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }
        //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
        public DataColumn IsHide
        {
            get
            {
                return this.colIsHide;
            }
        }

        //PRIMEPOS-2309 08-Mar-2019 JY Added
        public DataColumn StopAtRefNo
        {
            get
            {
                return this.colStopAtRefNo;
            }
        }

        //PRIMEPOS-2966 20-May-2021 JY Added
        public DataColumn SortOrder
        {
            get
            {
                return this.colSortOrder;
            }
        }
        #endregion //Properties

        #region Add and Get Methods 
        public void AddRow(PayTypeRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(PayTypeRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.PaytypeID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }
        //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added IsHide parameter
        //PRIMEPOS-2309 08-Mar-2019 JY Added StopAtRefNo parameter
        //PRIMEPOS-2966 20-May-2021 JY Added SortOrder
        public PayTypeRow AddRow(System.String PaytypeID, System.String colPayTypeDesc, System.String PayType, System.String UserID, System.Boolean IsHide, System.Boolean StopAtRefNo, System.Int32 SortOrder)
        {
            PayTypeRow row = (PayTypeRow)this.NewRow();
            row.ItemArray = new object[] { PaytypeID, colPayTypeDesc, PayType, UserID, IsHide, StopAtRefNo, SortOrder };
            this.Rows.Add(row);
            return row;
        }

        public PayTypeRow GetRowByID(System.String colPayTypeDesc)
        {
            return (PayTypeRow)this.Rows.Find(new object[] { colPayTypeDesc });
        }

        public void MergeTable(DataTable dt)
        {

            PayTypeRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (PayTypeRow)this.NewRow();

                if (dr[0] == DBNull.Value)
                    row[0] = DBNull.Value;
                else
                    row[0] = Convert.ToString(dr[0].ToString());
                if (dr[1] == DBNull.Value)
                    row[1] = DBNull.Value;
                else
                    row[1] = Convert.ToString(dr[1].ToString());

                if (dr[2] == DBNull.Value)
                    row[2] = DBNull.Value;
                else
                    row[2] = Convert.ToString(dr[2].ToString());

                row[4] = Convert.ToBoolean(dr[3]);   //Sprint-23 - PRIMEPOS-2255 18-May-2016 JY Added 
                row[5] = Convert.ToBoolean(dr[4]);  //PRIMEPOS-2309 08-Mar-2019 JY Added
                row[6] = Convert.ToInt32(dr[5]);  //PRIMEPOS-2966 20-May-2021 JY Added
                this.AddRow(row);
            }
        }

        #endregion
        public override DataTable Clone()
        {
            PayTypeTable cln = (PayTypeTable)base.Clone();
            cln.InitVars();
            return cln;
        }
        protected override DataTable CreateInstance()
        {
            return new PayTypeTable();
        }

        internal void InitVars()
        {
            this.colPayTypeID = this.Columns[clsPOSDBConstants.PayType_Fld_PayTypeID];
            this.colPayTypeDesc = this.Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription];
            this.colPayType = this.Columns[clsPOSDBConstants.PayType_Fld_PayType];
            this.colUserID = this.Columns[clsPOSDBConstants.fld_UserID];
            this.colIsHide = this.Columns[clsPOSDBConstants.PayType_Fld_IsHide];  //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            this.colStopAtRefNo = this.Columns[clsPOSDBConstants.PayType_Fld_StopAtRefNo];   //PRIMEPOS-2309 08-Mar-2019 JY Added
            this.colSortOrder = this.Columns[clsPOSDBConstants.PayType_Fld_SortOrder]; //PRIMEPOS-2966 20-May-2021 JY Added
        }
        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colPayTypeID = new DataColumn(clsPOSDBConstants.PayType_Fld_PayTypeID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayTypeID);
            this.colPayTypeID.AllowDBNull = true;

            this.colPayTypeDesc = new DataColumn(clsPOSDBConstants.PayType_Fld_PayTypeDescription, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayTypeDesc);
            this.colPayTypeDesc.AllowDBNull = false;

            this.colPayType = new DataColumn(clsPOSDBConstants.PayType_Fld_PayType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayType);
            this.colPayType.AllowDBNull = false;


            this.colUserID = new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = true;

            //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            this.colIsHide = new DataColumn(clsPOSDBConstants.PayType_Fld_IsHide, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsHide);
            this.colIsHide.AllowDBNull = true;

            //PRIMEPOS-2309 08-Mar-2019 JY Added
            this.colStopAtRefNo = new DataColumn(clsPOSDBConstants.PayType_Fld_StopAtRefNo, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStopAtRefNo);
            this.colStopAtRefNo.AllowDBNull = true;

            //PRIMEPOS-2966 20-May-2021 JY Added
            this.colSortOrder = new DataColumn(clsPOSDBConstants.PayType_Fld_SortOrder, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSortOrder);
            this.colSortOrder.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colPayTypeID };
        }
        public PayTypeRow NewTaxCodesRow()
        {
            return (PayTypeRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new PayTypeRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(PayTypeRow);
        }
    }
}
