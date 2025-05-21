namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Rows;

    public class ItemMonitorCategoryTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colID;
        private DataColumn colDescription;
        private DataColumn colUserID;
        private DataColumn colUOM;
        private DataColumn colDailyLimit;
        private DataColumn colThirtyDaysLimit;
        private DataColumn colMaxExempt;
        private DataColumn colVerificationBy;//Added By shitaljit on 30 April 2012
        private DataColumn colLimitPeriodQty;
        private DataColumn colLimitPeriodDays;
        private DataColumn colAgeLimit; //Added by Manoj 7/11/2013
        private DataColumn colIsAgeLimit; //Added by Manoj 7/11/2013
        private DataColumn colePSE;    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        private DataColumn colcanOverrideMonitorItem; //PRIMEPOS-3166

        #region Constants

        private const String _TableName = "ItemMonitorCategory";

        #endregion Constants

        #region Constructors

        internal ItemMonitorCategoryTable() : base(_TableName) { this.InitClass(); }

        internal ItemMonitorCategoryTable(DataTable table) : base(table.TableName) { }

        #endregion Constructors

        #region Properties

        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ItemMonitorCategoryRow this[int index]
        {
            get
            {
                return ((ItemMonitorCategoryRow) (this.Rows[index]));
            }
        }

        public DataColumn ID
        {
            get
            {
                return this.colID;
            }
        }

        public DataColumn Description
        {
            get
            {
                return this.colDescription;
            }
        }

        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }

        public DataColumn UOM
        {
            get
            {
                return this.colUOM;
            }
        }

        public DataColumn DailyLimit
        {
            get
            {
                return this.colDailyLimit;
            }
        }

        public DataColumn ThirtyDaysLimit
        {
            get
            {
                return this.colThirtyDaysLimit;
            }
        }

        public DataColumn MaxExempt
        {
            get
            {
                return this.colMaxExempt;
            }
        }

        public DataColumn VerificationBy
        {
            get
            {
                return this.colVerificationBy;
            }
        }

        public DataColumn LimitPeriodDays
        {
            get
            {
                return this.colLimitPeriodDays;
            }
        }

        //Added by Manoj 7/11/2013
        public DataColumn AgeLimit
        {
            get
            {
                return this.colAgeLimit;
            }
        }

        //Added by Manoj 7/11/2013
        public DataColumn IsAgeLimit
        {
            get
            {
                return this.colIsAgeLimit;
            }
        }

        public DataColumn LimitPeriodQty
        {
            get
            {
                return this.colLimitPeriodQty;
            }
        }

        //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        public DataColumn ePSE
        {
            get
            {
                return this.colePSE;
            }
        }

        #region PRIMEPOS-3166
        public DataColumn canOverrideMonitorItem
        {
            get
            {
                return this.colcanOverrideMonitorItem;
            }
        }
        #endregion

        #endregion Properties

        #region Add and Get Methods

        public void AddRow(ItemMonitorCategoryRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ItemMonitorCategoryRow row, bool preserveChanges)
        {
            if(this.GetRowByID(row.ID) == null)
            {
                this.Rows.Add(row);
                if(!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public ItemMonitorCategoryRow AddRow(System.Int32 ID, System.String Description, System.String UserID, System.String VerificationBy, System.Int32 LimitPeriodDays, System.Decimal LimitPeriodQty)
        {
            ItemMonitorCategoryRow row = (ItemMonitorCategoryRow) this.NewRow();
            row.ItemArray = new object[] { ID, Description, UserID, VerificationBy, LimitPeriodDays, LimitPeriodQty };
            this.Rows.Add(row);
            return row;
        }

        public ItemMonitorCategoryRow GetRowByID(System.Int32 iID)
        {
            return (ItemMonitorCategoryRow) this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt)
        {
            ItemMonitorCategoryRow row;
            foreach(DataRow dr in dt.Rows)
            {
                row = (ItemMonitorCategoryRow) this.NewRow();

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ID] == DBNull.Value)
                    row.ID = 0;
                else
                    row.ID = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_Description] == DBNull.Value)
                    row.Description = "";
                else
                    row.Description = Convert.ToString(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ID] == DBNull.Value)
                    row.UserID = "";
                else
                    row.UserID = Convert.ToString(dr[clsPOSDBConstants.Users_Fld_UserID].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_UOM] == DBNull.Value)
                    row.UOM = "";
                else
                    row.UOM = Convert.ToString(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_UOM].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit] == DBNull.Value)
                    row.DailyLimit = 0;
                else
                    row.DailyLimit = Convert.ToDecimal((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit] == DBNull.Value)
                    row.ThirtyDaysLimit = 0;
                else
                    row.ThirtyDaysLimit = Convert.ToDecimal((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt] == DBNull.Value)
                    row.MaxExempt = 0;
                else
                    row.MaxExempt = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt].ToString());

                //Added By shitaljit on 30 April 2012
                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy] == DBNull.Value)
                    row.VerificationBy = "";
                else
                    row.VerificationBy = Convert.ToString(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays] == DBNull.Value)
                    row.LimitPeriodDays = 0;
                else
                    row.LimitPeriodDays = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty] == DBNull.Value)
                    row.LimitPeriodQty = 0;
                else
                    row.LimitPeriodQty = Convert.ToDecimal((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit] == DBNull.Value)
                    row.AgeLimit = 0;
                else
                    row.AgeLimit = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit].ToString());

                if(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit] == DBNull.Value)
                    row.IsAgeLimit = false;
                else
                    row.IsAgeLimit = Convert.ToBoolean((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit].ToString());

                //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
                if (dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE] == DBNull.Value)
                    row.ePSE = false;
                else
                    row.ePSE = Convert.ToBoolean((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE].ToString());

                #region PRIMEPOS-3166
                if (dr[clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem] == DBNull.Value)
                    row.canOverrideMonitorItem = false;
                else
                    row.canOverrideMonitorItem = Convert.ToBoolean((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem].ToString());
                #endregion

                this.AddRow(row);
            }
        }

        #endregion Add and Get Methods

        public override DataTable Clone()
        {
            ItemMonitorCategoryTable cln = (ItemMonitorCategoryTable) base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new ItemMonitorCategoryTable();
        }

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ID];
            this.colDescription = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_Description];
            this.colUserID = this.Columns[clsPOSDBConstants.fld_UserID];
            this.colUOM = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_UOM];
            this.colDailyLimit = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit];
            this.colThirtyDaysLimit = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit];
            this.colMaxExempt = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt];
            this.colVerificationBy = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy];
            this.colLimitPeriodDays = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays];
            this.colLimitPeriodQty = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty];
            this.colAgeLimit = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit];
            this.colePSE = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE];    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            this.colcanOverrideMonitorItem = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem]; //PRIMEPOS-3166
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colID = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colID);

            this.colDescription = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDescription);

            this.colUserID = new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);

            this.colUOM = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_UOM, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUOM);

            this.colDailyLimit = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_DailyLimit, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDailyLimit);

            this.colThirtyDaysLimit = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_ThirtyDaysLimit, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colThirtyDaysLimit);

            this.colMaxExempt = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_MaxExempt, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMaxExempt);

            //Added By shitaljit on 30 April 2012
            this.colVerificationBy = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_VerificationBy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVerificationBy);

            this.colLimitPeriodDays = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodDays, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLimitPeriodDays);

            this.colLimitPeriodQty = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_LimitPeriodQty, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLimitPeriodQty);

            this.colAgeLimit = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_AgeLimit, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAgeLimit);

            this.colIsAgeLimit = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_IsAgeLimit, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsAgeLimit);

            //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            this.colePSE = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colePSE);

            #region PRIMEPOS-3166
            this.colcanOverrideMonitorItem = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_canOverrideMonitorItem, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colcanOverrideMonitorItem);
            #endregion

            this.PrimaryKey = new DataColumn[] { this.ID };
        }

        public ItemMonitorCategoryRow NewItemMonitorCategoryRow()
        {
            return (ItemMonitorCategoryRow) this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ItemMonitorCategoryRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ItemMonitorCategoryRow);
        }
    }
}