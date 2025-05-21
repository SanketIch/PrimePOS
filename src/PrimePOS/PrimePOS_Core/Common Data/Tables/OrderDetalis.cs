using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using POS_Core.CommonData;
using System.Data;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Tables
{
    public partial class OrderDetalis : DataTable, System.Collections.IEnumerable 
    {

        private DataColumn colVendorID;
        private DataColumn colVendorName;
        private DataColumn colOrderNO;
        private DataColumn colOrderDescription;
        private DataColumn colOrderID;    
        private DataColumn colTimeToOrder;
        private DataColumn colTotalItems;
        private DataColumn colTotalCost;
        private DataColumn colTotalQty;
        private DataColumn colAutoSend;
        private DataColumn colCloseOrder;         

        #region Constructors
        internal OrderDetalis() : base(clsPOSDBConstants.OrderDetail_tbl) { this.InitClass(); }
        internal OrderDetalis(DataTable table) : base(table.TableName) { }
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

        public DataColumn OrderNO
        {
            get
            {
                return this.colOrderNO;
            }
        }
        public DataColumn OrderDescription
        {
            get
            {
                return this.colOrderDescription;
            }
        }
        public DataColumn OrderID
        {
            get
            {
                return this.colOrderID;
            }
        }
        public DataColumn TimeToOrder
        {
            get
            {
                return this.colTimeToOrder;
            }
        }
        public DataColumn TotalItems
        {
            get
            {
                return this.colTotalItems;
            }
        }
        public DataColumn TotalQty
        {
            get
            {
                return this.colTotalQty;
            }
        }
        public DataColumn TotalCost
        {
            get
            {
                return this.colTotalCost;
            }
        }
        public DataColumn AutoSend
        {
            get
            {
                return this.colAutoSend;
            }
        }
        public DataColumn CloseOrder
        {
            get
            {
                return this.colCloseOrder;
            }
        }
      
        #endregion //Properties
        #region Add and Get Methods
        public void AddRow(OrderDetailRow row)
        {
            AddRow(row, false);
        }
        public void AddRow(OrderDetailRow row, bool preserveChanges)
        {
            if (this.PrimaryKey[0] != row.Table.Columns[clsPOSDBConstants.POHeader_Fld_OrderID])
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
            else
            {
                OrderDetailRow orderDetailRow = this.GetRowByID(row.OrderID);
                if (orderDetailRow == null)
                {
                    this.Rows.Add(row);
                    if (!preserveChanges)
                    {
                        row.AcceptChanges();
                    }
                }
                else
                {

                    this.Rows[0][clsPOSDBConstants.POHeader_Fld_VendorID] = row.VendorID; 
                    this.AcceptChanges(); 
                    //AddRow(row, false);
                }
            }
         
        }
        public OrderDetailRow GetRowByID(System.Int32 VendorID)
        {
           return (OrderDetailRow)this.Rows.Find(new object[] { VendorID });
        }
        public OrderDetailRow AddRow(System.Int32 vendorID
                                        ,System.String vendorName
                                         ,System.Int32 orderNO
                                        , System.Int32 orderDesc
                                        ,System.Int32 orderID
                                        , System.String timeToOrder
                                        , System.Int32 totalItems
                                        , System.Int32 totalQty
                                        , System.Int32 totalCost
                                        , System.Int32 autoSend
                                        , System.Int32 closeOrder
                                     )
        {
            OrderDetailRow row = (OrderDetailRow)this.NewRow();
            row.ItemArray = new object[] { vendorID, vendorName, orderNO,orderDesc,orderID,timeToOrder, totalItems, totalQty, totalCost, autoSend, closeOrder };
            this.Rows.Add(row);
            return row;
        }
        //public PODetailRow AddRow(System.Int32 PODetailID
        //                                , System.Int32 OrderID
        //                                , System.Int32 QTY
        //                                , System.Decimal Cost
        //                                , System.String ItemID
        //                                , System.String Comments
        //                                , System.String ItemDescription)
        //{
        //    PODetailRow row = (PODetailRow)this.NewRow();
        //    row.ItemArray = new object[] { PODetailID, OrderID, Cost, Comments, QTY, ItemID, ItemDescription };
        //    /*row.PODetailID=PODetailID;
        //    row.OrderID=OrderID;
        //    row.ItemID=ItemID;
        //    row.ItemDescription=ItemDescription;
        //    row.Cost=Cost;
        //    row.Comments=Comments;
        //    row.QTY=QTY;
        //    */
        //    this.Rows.Add(row);
        //    return row;
        //}
        //public void MergeTable(DataTable dt)
        //{
        //    PODetailRow row;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        row = (PODetailRow)this.NewRow();

        //        if (dr[clsPOSDBConstants.PODetail_Fld_PODetailID] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_PODetailID] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_PODetailID] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_PODetailID].ToString() == "") ? "0" : dr[0].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_ItemID] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_ItemID] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_ItemID] = Convert.ToString((dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString() == "") ? "" : dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString());

        //        if (dr[clsPOSDBConstants.Item_Fld_Description] == DBNull.Value)
        //            row[clsPOSDBConstants.Item_Fld_Description] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.Item_Fld_Description] = dr[clsPOSDBConstants.Item_Fld_Description].ToString();

        //        if (dr[clsPOSDBConstants.PODetail_Fld_OrderID] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_OrderID] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_OrderID] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_OrderID].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_OrderID].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_Comments] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_Comments] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_Comments] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_Comments].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_QTY] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_QTY] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_QTY] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_QTY].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_QTY].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_Cost] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_Cost] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_Cost].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_Cost].ToString());
        //        //Added by Prashant(SRT)Date:4-06-2009
        //        if (dr[clsPOSDBConstants.PODetail_Fld_VendorName] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_VendorName] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_VendorName] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_VendorName].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_VendorName].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_BestVendor] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_BestVendor] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_BestVendor] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_BestVendor].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_BestVendor].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_BestVendPrice] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_BestVendPrice] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_BestVendPrice] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_BestVendPrice].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_BestVendPrice].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_LastOrdVendor] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_LastOrdVendor] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_LastOrdVendor] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].ToString());

        //        if (dr[clsPOSDBConstants.PODetail_Fld_LastOrdQty] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_LastOrdQty] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_LastOrdQty] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_LastOrdQty].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_LastOrdQty].ToString());
        //        //End Of Added by Prashant(SRT)Date:4-06-2009

        //        if (dr[clsPOSDBConstants.PODetail_Fld_AckQTY] == DBNull.Value)
        //            row[clsPOSDBConstants.PODetail_Fld_AckQTY] = DBNull.Value;
        //        else
        //            row[clsPOSDBConstants.PODetail_Fld_AckQTY] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_AckQTY].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_AckQTY].ToString());

        //        row[clsPOSDBConstants.PODetail_Fld_AckStatus] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_AckStatus].ToString());
        //        row[clsPOSDBConstants.PODetail_Fld_VendorItemCode] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString());

        //        row[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier].ToString());
        //        row[clsPOSDBConstants.PODetail_Fld_ChangedProductID] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_ChangedProductID].ToString());

        //        this.AddRow(row);
        //    }
        //}
        #endregion
        public override DataTable Clone()
        {
            OrderDetalis cln = (OrderDetalis)base.Clone();
            cln.InitVars();
            return cln;
        }
        protected override DataTable CreateInstance()
        {
            return new OrderDetalis();
        }
        internal void InitVars()
        {
            this.colVendorID = this.Columns[clsPOSDBConstants.POHeader_Fld_VendorID];
            this.colOrderNO = this.Columns[clsPOSDBConstants.OrderDetail_Fld_OrderNo];
            this.colOrderDescription = this.Columns[clsPOSDBConstants.POHeader_Fld_Description];
            this.colOrderID  = this.Columns[clsPOSDBConstants.OrderDetail_Tbl_OrderId];
            this.colTimeToOrder = this.Columns[clsPOSDBConstants.OrderDetail_Fld_TimeToOrder];
            this.colTotalItems = this.Columns[clsPOSDBConstants.OrderDetail_Fld_TotalItems];
            this.colTotalQty = this.Columns[clsPOSDBConstants.OrderDetail_Fld_TotalQty];
            this.colTotalCost = this.Columns[clsPOSDBConstants.OrderDetail_Fld_TotalCost];
            this.colAutoSend = this.Columns[clsPOSDBConstants.OrderDetail_Fld_AuroSend];
            this.colVendorName = this.Columns[clsPOSDBConstants.OrderDetail_Fld_VendorName];
            this.colCloseOrder = this.Columns[clsPOSDBConstants.OrderDetail_Tbl_CloseOrder];       
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colVendorID = new DataColumn(clsPOSDBConstants.POHeader_Fld_VendorID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorID);
            this.colVendorID.AllowDBNull = true;

            this.colOrderNO = new DataColumn(clsPOSDBConstants.OrderDetail_Fld_OrderNo, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOrderNO);
            this.colOrderNO.AllowDBNull = true;

            this.colOrderDescription = new DataColumn(clsPOSDBConstants.POHeader_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOrderDescription);
            this.colOrderDescription.AllowDBNull = true;

            this.colOrderID  = new DataColumn(clsPOSDBConstants.OrderDetail_Tbl_OrderId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOrderID);
            this.colOrderID.AllowDBNull = true;

            this.colTimeToOrder = new DataColumn(clsPOSDBConstants.OrderDetail_Fld_TimeToOrder, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTimeToOrder);
            this.colTimeToOrder.AllowDBNull = true;

            this.colTotalItems = new DataColumn(clsPOSDBConstants.OrderDetail_Fld_TotalItems, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalItems);
            this.colTotalItems.AllowDBNull = true;

            this.colTotalQty = new DataColumn(clsPOSDBConstants.OrderDetail_Fld_TotalQty, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalQty);
            this.colTotalQty.AllowDBNull = true;

            this.colTotalCost = new DataColumn(clsPOSDBConstants.OrderDetail_Fld_TotalCost, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalCost);
            this.colTotalCost.AllowDBNull = true;

            this.colAutoSend = new DataColumn(clsPOSDBConstants.OrderDetail_Fld_AuroSend, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAutoSend);
            this.colAutoSend.AllowDBNull = true;

            this.colVendorName = new DataColumn(clsPOSDBConstants.OrderDetail_Fld_VendorName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorName);
            this.colVendorName.AllowDBNull = true;

            this.colCloseOrder = new DataColumn(clsPOSDBConstants.OrderDetail_Tbl_CloseOrder, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCloseOrder);
            this.colCloseOrder.AllowDBNull = true;


            this.PrimaryKey = new DataColumn[] { this.colVendorID };
        }

        public OrderDetailRow NewOrderDetailRow()
        {
            return (OrderDetailRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new OrderDetailRow(builder);
        }
    }
}
