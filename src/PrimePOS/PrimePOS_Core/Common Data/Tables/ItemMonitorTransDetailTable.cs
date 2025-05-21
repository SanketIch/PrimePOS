//----------------------------------------------------------------------------------------------------
//Sprint-23 - PRIMEPOS-2029 19-Apr-2016 JY Added to maintain item monitor trans log
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class ItemMonitorTransDetailTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colID;
        private DataColumn colTransDetailID;
        private DataColumn colItemMonCatID;
        private DataColumn colUOM;
        private DataColumn colUnitsPerPackage;
        private DataColumn colSentToNplex;
        private DataColumn colPostSaleInd;
        private DataColumn colpseTrxId; //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added

        #region Constants
        private const String _TableName = "ItemMonitorTransDetail";
        #endregion

        #region Constructors
        internal ItemMonitorTransDetailTable() : base(_TableName) { this.InitClass(); }
        internal ItemMonitorTransDetailTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ItemMonitorTransDetailRow this[int index]
        {
            get
            {
                return ((ItemMonitorTransDetailRow)(this.Rows[index]));
            }
        }

        public DataColumn ID
        {
            get
            {
                return this.colID;
            }
        }

        public DataColumn TransDetailID
        {
            get
            {
                return this.colTransDetailID;
            }
        }

        public DataColumn ItemMonCatID
        {
            get
            {
                return this.colItemMonCatID;
            }
        }

        public DataColumn UOM
        {
            get
            {
                return this.colUOM;
            }
        }

        public DataColumn UnitsPerPackage
        {
            get
            {
                return this.colUnitsPerPackage;
            }
        }

        public DataColumn SentToNplex
        {
            get
            {
                return this.colSentToNplex;
            }
        }

        public DataColumn PostSaleInd
        {
            get
            {
                return this.colPostSaleInd;
            }
        }

        //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
        public DataColumn pseTrxId
        {
            get
            {
                return this.colpseTrxId;
            }
        }
        #endregion Properties

        #region Add and Get Methods
        public void AddRow(ItemMonitorTransDetailRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ItemMonitorTransDetailRow row, bool preserveChanges)
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

        public ItemMonitorTransDetailRow AddRow(System.Int32 iID, 
                                        System.Int32 iTransDetailID, 
                                        System.Int32 iItemMonCatID, 
                                        System.String sUOM, 
                                        System.Decimal sUnitsPerPackage, 
                                        System.Boolean bSentToNplex, 
                                        System.Boolean bPostSaleInd,
                                        System.Int64 lpseTrxId)
        {
            ItemMonitorTransDetailRow row = (ItemMonitorTransDetailRow)this.NewRow();

            row.ID = iID;
            row.TransDetailID = iTransDetailID;
            row.ItemMonCatID = iItemMonCatID;
            row.UOM = sUOM;
            row.UnitsPerPackage = sUnitsPerPackage;
            row.SentToNplex = bSentToNplex;
            row.PostSaleInd = bPostSaleInd;
            row.pseTrxId = lpseTrxId;   //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
            this.Rows.Add(row);
            return row;
        }

        public ItemMonitorTransDetailRow GetRowByID(System.Int64 iID)
        {
            return (ItemMonitorTransDetailRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt)
        {
            ItemMonitorTransDetailRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ItemMonitorTransDetailRow)this.NewRow();

                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id] = 0;
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id] = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID] = 0;
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID] = 0;
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID] = Convert.ToInt32((dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM] = "";
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM] = Convert.ToString((dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM].ToString() == "") ? "" : dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage] = 0;
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage] = Convert.ToDecimal((dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_SentToNplex] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_SentToNplex] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_SentToNplex] = Convert.ToBoolean(dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_SentToNplex].ToString());

                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_PostSaleInd] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_PostSaleInd] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_PostSaleInd] = Convert.ToBoolean(dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_PostSaleInd].ToString());

                //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
                if (dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId] == DBNull.Value)
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId] = 0;
                else
                    row[clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId] = Convert.ToInt64((dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId].ToString());

                this.AddRow(row);
            }
        }
        #endregion 

        public override DataTable Clone()
        {
            ItemMonitorTransDetailTable cln = (ItemMonitorTransDetailTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new ItemMonitorTransDetailTable();
        }

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_Id];
            this.colTransDetailID = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_TransDetailID];
            this.colItemMonCatID = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_ItemMonCatID];
            this.colUOM = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UOM];
            this.colUnitsPerPackage = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_UnitsPerPackage];
            this.colSentToNplex = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_SentToNplex];
            this.colPostSaleInd = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_PostSaleInd];
            this.colpseTrxId = this.Columns[clsPOSDBConstants.ItemMonitorTransDetail_Fld_pseTrxId]; //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colID = new DataColumn("ID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colID);
            this.colID.AllowDBNull = true;

            this.colTransDetailID = new DataColumn("TransDetailID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransDetailID);
            this.colTransDetailID.AllowDBNull = true;

            this.colItemMonCatID = new DataColumn("ItemMonCatID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemMonCatID);
            this.colItemMonCatID.AllowDBNull = true;

            this.colUOM = new DataColumn("UOM", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUOM);
            this.colUOM.AllowDBNull = true;

            this.colUnitsPerPackage = new DataColumn("UnitsPerPackage", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUnitsPerPackage);
            this.colUnitsPerPackage.AllowDBNull = true;

            this.colSentToNplex = new DataColumn("SentToNplex", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSentToNplex);
            this.colSentToNplex.AllowDBNull = true;

            this.colPostSaleInd = new DataColumn("PostSaleInd", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPostSaleInd);
            this.colPostSaleInd.AllowDBNull = true;

            //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added
            this.colpseTrxId = new DataColumn("pseTrxId", typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colpseTrxId);
            this.colpseTrxId.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colID };
        }

        public ItemMonitorTransDetailRow NewItemMonitorTransDetailRow()
        {
            return (ItemMonitorTransDetailRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ItemMonitorTransDetailRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ItemMonitorTransDetailRow);
        }
    }
}
