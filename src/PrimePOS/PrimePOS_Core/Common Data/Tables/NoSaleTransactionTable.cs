using System;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using System.Collections.Generic;
using Resources;

namespace POS_Core.CommonData.Tables
{
    public class NoSaleTransactionTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colId;
        private DataColumn colUserId;
        private DataColumn colStationId;
        private DataColumn colDrawerOpenedDate;

        #region properties
        public DataColumn UserId
        {
            get
            {
                return this.colUserId;
            }
        }
        public DataColumn Id
        {
            get
            {
                return this.colId;
            }
        }
        public DataColumn StationId
        {
            get
            {
                return this.colStationId;
            }
        }
        public DataColumn DrawerOpenedDate
        {
            get
            {
                return this.colDrawerOpenedDate;
            }
        }
        #endregion
        #region Constants
        private const String _TableName = "NoSale";
        #endregion
        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        #region Constructors 
        internal NoSaleTransactionTable() : base(_TableName) { this.InitClass(); }
        internal NoSaleTransactionTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Add and Get Methods 
        public void AddRow(NoSaleTransactionRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(NoSaleTransactionRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.Id) == null)
            {
                this.Rows.Add(row);
                if (preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }
        
        public NoSaleTransactionRow AddRow(System.Int32 Id
                                        , System.String UserId
                                        , System.String StationId
                                        , System.DateTime DrawerOpenedDate)             
        {
            NoSaleTransactionRow row = (NoSaleTransactionRow)this.NewRow();
            row.Id = Id;
            row.UserId = UserId;
            row.StationId = StationId;
            row.DrawerOpenedDate = DrawerOpenedDate;          

            this.Rows.Add(row);
            return row;
        }

        public NoSaleTransactionRow GetRowByID(System.Int32 TransPayID)
        {
            return (NoSaleTransactionRow)this.Rows.Find(new object[] { TransPayID });
        }

        public void MergeTable(DataTable dt)
        {

            NoSaleTransactionRow row;

            foreach (DataRow dr in dt.Rows)
            {
                row = (NoSaleTransactionRow)this.NewRow();

                if (dr[clsPOSDBConstants.NoSaleTable_Fld_Id] == DBNull.Value)
                    row[clsPOSDBConstants.NoSaleTable_Fld_Id] = DBNull.Value;
                else
                    row[clsPOSDBConstants.NoSaleTable_Fld_Id] = Convert.ToInt32((dr[clsPOSDBConstants.NoSaleTable_Fld_Id].ToString() == "") ? "0" : dr[clsPOSDBConstants.NoSaleTable_Fld_Id].ToString());

                if (dr[clsPOSDBConstants.NoSaleTable_Fld_UserId] == DBNull.Value)
                    row[clsPOSDBConstants.NoSaleTable_Fld_UserId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.NoSaleTable_Fld_UserId] = Convert.ToString(dr[clsPOSDBConstants.NoSaleTable_Fld_UserId].ToString());

                if (dr[clsPOSDBConstants.NoSaleTable_Fld_StationId] == DBNull.Value)
                    row[clsPOSDBConstants.NoSaleTable_Fld_StationId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.NoSaleTable_Fld_StationId] = Convert.ToString(dr[clsPOSDBConstants.NoSaleTable_Fld_StationId].ToString());

                if (dr[clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate] == DBNull.Value)
                    row[clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate] = Convert.ToDateTime((dr[clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate].ToString()));
                
                this.AddRow(row);
            }
        }

        #endregion
        public override DataTable Clone()
        {
            NoSaleTransactionTable cln = (NoSaleTransactionTable)base.Clone();
            cln.InitVars();
            return cln;
        }
        protected override DataTable CreateInstance()
        {
            return new NoSaleTransactionTable();
        }

        internal void InitVars()
        {
            this.colId = this.Columns[clsPOSDBConstants.NoSaleTable_Fld_Id];
            this.colUserId = this.Columns[clsPOSDBConstants.NoSaleTable_Fld_UserId];
            this.colStationId = this.Columns[clsPOSDBConstants.NoSaleTable_Fld_StationId];
            this.colDrawerOpenedDate = this.Columns[clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate];            
        }        
        private void InitClass()
        {
            this.colId = new DataColumn(clsPOSDBConstants.NoSaleTable_Fld_Id, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colId);
            this.colId.AllowDBNull = true;

            this.colUserId = new DataColumn(clsPOSDBConstants.NoSaleTable_Fld_UserId, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserId);
            this.colUserId.AllowDBNull = false;

            this.colStationId = new DataColumn(clsPOSDBConstants.NoSaleTable_Fld_StationId, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStationId);
            this.colStationId.AllowDBNull = true;

            this.colDrawerOpenedDate = new DataColumn(clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDrawerOpenedDate);
            this.colDrawerOpenedDate.AllowDBNull = true;
            
            this.PrimaryKey = new DataColumn[] { this.colId };
        }

        public NoSaleTransactionRow NewNoSaleTransactionRow()
        {
            int Id = 1;
            foreach (NoSaleTransactionRow oRow in this.Rows)
            {
                if (oRow.Id >= Id)
                {
                    Id = oRow.Id + 1;
                }
            }
            NoSaleTransactionRow oNewRow = (NoSaleTransactionRow)this.NewRow();
            oNewRow.Id = Id;
            return oNewRow;

        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new NoSaleTransactionRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(NoSaleTransactionRow);
        }

        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
        }

        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
        }

    }
}
