
namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class ItemMonitorCategoryDetailTable : DataTable, System.Collections.IEnumerable
    {

        private DataColumn colID;
        private DataColumn colDescription;
        private DataColumn colUserID;
        private DataColumn colItemMonCatID;
        private DataColumn colItemID;
        private DataColumn colUnitsPerPackage;
        private DataColumn colePSE;    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        
        #region Constants
        private const String _TableName = "ItemMonitorCategoryDetail";
        #endregion

        #region Constructors
        internal ItemMonitorCategoryDetailTable() : base(_TableName) { this.InitClass(); }
        internal ItemMonitorCategoryDetailTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ItemMonitorCategoryDetailRow this[int index]
        {
            get
            {
                return ((ItemMonitorCategoryDetailRow)(this.Rows[index]));
            }
        }

        public DataColumn ID
        {
            get
            {
                return this.colID;
            }
        }
        public DataColumn ItemMonCatID
        {
            get
            {
                return this.colItemMonCatID;
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
        public DataColumn ItemID
        {
            get
            {
                return this.colItemID;
            }
        }
        public DataColumn UnitsPerPackage
        {
            get
            {
                return this.colUnitsPerPackage; ;
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

        #endregion //Properties

        #region Add and Get Methods

        public void AddRow(ItemMonitorCategoryDetailRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ItemMonitorCategoryDetailRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public ItemMonitorCategoryDetailRow AddRow(System.Int32 ID, System.String Description, System.String UserID,System.Int32 ItemMonCatID,string ItemID,System.Decimal UnitsPerPackage)
        {
            ItemMonitorCategoryDetailRow row = (ItemMonitorCategoryDetailRow)this.NewRow();
            row.ItemArray = new object[] { ID, Description, UserID, ItemMonCatID, ItemID, UnitsPerPackage };
            this.Rows.Add(row);
            return row;
        }

        public ItemMonitorCategoryDetailRow GetRowByID(System.Int32 iID)
        {
            return (ItemMonitorCategoryDetailRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt)
        {

            ItemMonitorCategoryDetailRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ItemMonitorCategoryDetailRow)this.NewRow();

                if (dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ID] == DBNull.Value)
                    row.ID = 0;
                else
                    row.ID = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ID].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorCategory_Fld_Description] == DBNull.Value)
                    row.Description= "";
                else
                    row.Description = Convert.ToString(dr[clsPOSDBConstants.ItemMonitorCategory_Fld_Description].ToString());

                if (dr[clsPOSDBConstants.fld_UserID] == DBNull.Value)
                    row.UserID = "";
                else
                    row.UserID= Convert.ToString(dr[clsPOSDBConstants.Users_Fld_UserID].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID] == DBNull.Value)
                    row.ItemMonCatID = 0;
                else
                    row.ItemMonCatID = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_ItemID] == DBNull.Value)
                    row.ItemID= "";
                else
                    row.ItemID = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_ItemID].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage] == DBNull.Value)
                    row.UnitsPerPackage = 0;
                else
                    row.UnitsPerPackage = Convert.ToDecimal((dr[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage].ToString());

                //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
                if (dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE] == DBNull.Value)
                    row.ePSE = false;
                else
                    row.ePSE = Convert.ToBoolean((dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE].ToString());

                this.AddRow(row);
            }
        }

        #endregion

        public override DataTable Clone()
        {
            ItemMonitorCategoryDetailTable cln = (ItemMonitorCategoryDetailTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new ItemMonitorCategoryDetailTable();
        }

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ID];
            this.colDescription = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_Description];
            this.colUserID = this.Columns[clsPOSDBConstants.fld_UserID];
            this.colItemMonCatID= this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID];
            this.colItemID = this.Columns[clsPOSDBConstants.Item_Fld_ItemID];
            this.colUnitsPerPackage = this.Columns[clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage];
            this.colePSE = this.Columns[clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE];    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colID = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colID);
            //this.colID.AllowDBNull = true;

            this.colDescription = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDescription);
            //this.colDescription.AllowDBNull = false;

            this.colUserID = new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            //this.colUserID.AllowDBNull = true;

            this.colItemMonCatID = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_ItemMonCatID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemMonCatID);

            this.colItemID= new DataColumn(clsPOSDBConstants.Item_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemID);

            this.colUnitsPerPackage = new DataColumn(clsPOSDBConstants.ItemMonitorCategoryDetail_Fld_UnitsPerPackage, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUnitsPerPackage);

            //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            this.colePSE = new DataColumn(clsPOSDBConstants.ItemMonitorCategory_Fld_ePSE, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colePSE);

            this.PrimaryKey = new DataColumn[] { this.ID };
        }

        public ItemMonitorCategoryDetailRow NewItemMonitorCategoryDetailRow()
        {
            return (ItemMonitorCategoryDetailRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ItemMonitorCategoryDetailRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ItemMonitorCategoryDetailRow);
        }

        public int GetNextID()
        {
            if (this.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                Int32 MaxNo = 0;
                foreach (ItemMonitorCategoryDetailRow oRow in this.Rows)
                {
                    if (oRow.RowState != DataRowState.Deleted)
                    {
                        if (oRow.ID > MaxNo)
                        {
                            MaxNo = oRow.ID;
                        }
                    }
                }
                return MaxNo + 1;
            }
        }
    }
}
