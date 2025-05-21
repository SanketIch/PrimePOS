using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData.Tables
{
    public partial class MasterOrderDetailsTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colVendorID;
        private DataColumn colVendorName;
        private DataColumn colTotalPOs;
        private DataColumn colTimeToOrder;
        #region Constructors
        internal MasterOrderDetailsTable() : base(clsPOSDBConstants.MasterOrderDetails_tbl) { this.InitClass(); }
        internal MasterOrderDetailsTable(DataTable table) : base(table.TableName) { }
        #endregion
        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }
        public OrderDetailRow this[int index]
        {
            get
            {
                return ((OrderDetailRow)(this.Rows[index]));
            }
        }
        public DataColumn VendorID
        {
            get
            {
                return this.colVendorID;
            }
        }
        public DataColumn VendorName
        {
            get
            {
                return this.colVendorName;
            }
        }
        public DataColumn TimeToOrder
        {
            get
            {
                return this.colTimeToOrder;
            }
        }

        public DataColumn TotalPOs
        {
            get
            {
                return this.colTotalPOs;
            }
        }
        #endregion
        #region Add and Get Methods
        public void AddRow(MasterOrderDetailsRow  row)
        {
            AddRow(row, false);
        }
        public void AddRow(MasterOrderDetailsRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.VendorID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }
        public MasterOrderDetailsRow GetRowByID(System.Int32 VendorID)
        {
            return (MasterOrderDetailsRow)this.Rows.Find(new object[] { VendorID });
        }
        public MasterOrderDetailsRow AddRow(System.Int32 vendorID
                                        , System.String vendorName
                                        , System.String timeToOrder
                                         , System.Int32 totalPos
                                     )
        {
            MasterOrderDetailsRow row = (MasterOrderDetailsRow)this.NewRow();
            row.ItemArray = new object[] { vendorID,timeToOrder, vendorName, totalPos };
            this.Rows.Add(row);
            return row;
        }
        public override DataTable Clone()
        {
            MasterOrderDetailsTable cln = (MasterOrderDetailsTable)base.Clone();
            cln.InitVars();
            return cln;
        }
        protected override DataTable CreateInstance()
        {
            return new MasterOrderDetailsTable();
        }
        internal void InitVars()
        {
            this.colVendorID = this.Columns[clsPOSDBConstants.MasterOrderDetails_Fld_VendorID];
            this.colTotalPOs = this.Columns[clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs];
            this.colVendorName = this.Columns[clsPOSDBConstants.MasterOrderDetails_Fld_VendorName];
            this.colTimeToOrder = this.Columns[clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder];
        }
        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }
        private void InitClass()
        {
            this.colVendorID = new DataColumn(clsPOSDBConstants.MasterOrderDetails_Fld_VendorID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorID);
            this.colVendorID.AllowDBNull = true;

            this.colTotalPOs = new DataColumn(clsPOSDBConstants.MasterOrderDetails_Fld_TotalPOs, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalPOs);
            this.colTotalPOs.AllowDBNull = true;

            this.colVendorName = new DataColumn(clsPOSDBConstants.MasterOrderDetails_Fld_VendorName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorName);
            this.colVendorName.AllowDBNull = true;

            this.colTimeToOrder = new DataColumn(clsPOSDBConstants.OrderDetail_Tbl_TimeToOrder, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTimeToOrder);
            this.colTimeToOrder.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colVendorID };
        }

        public MasterOrderDetailsRow NewOrderDetailRow()
        {
            return (MasterOrderDetailsRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new MasterOrderDetailsRow(builder);
        }
        #endregion
    }
}