
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

    public class InvRecvDetailTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colInvRecvDetailID;
        private DataColumn colInvRecievedID;
        private DataColumn colItemID;
        private DataColumn colVendorItemId; //Sprint-21 - 2207 03-Aug-2015 JY Added
        private DataColumn colItemDescription;
        private DataColumn colQtyOrdered;
        private DataColumn colQTY;
        private DataColumn colCost;
        private DataColumn colTotalCost;    //Sprint-21 - 2002 21-Jul-2015 JY Added
        private DataColumn colSalePrice;
        private DataColumn colComments;
        private DataColumn colVendorCode; //Sprint-21 - 2207 03-Aug-2015 JY Added
        private DataColumn colExpDate;  //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
        private DataColumn colQtyInStock;   //PRIMEPOS-2396 12-Jun-2018 JY Added
        private DataColumn colLastInvUpdatedQty;   //PRIMEPOS-2396 12-Jun-2018 JY Added
        private DataColumn colDeptID;   //PRIMEPOS-2419 28-May-2019 JY Added
        private DataColumn colSubDepartmentID;  //PRIMEPOS-2419 28-May-2019 JY Added
        private DataColumn colIsEBTItem;  //PRIMEPOS-2419 28-May-2019 JY Added
        private DataColumn colDeptCode; //PRIMEPOS-2725 29-Aug-2019 JY Added
        private DataColumn colDeptName; //PRIMEPOS-2725 29-Aug-2019 JY Added
        private DataColumn colSubDept;  //PRIMEPOS-2725 29-Aug-2019 JY Added

        #region Constructors 
        internal InvRecvDetailTable() : base(clsPOSDBConstants.InvRecvDetail_tbl) { this.InitClass(); }
        internal InvRecvDetailTable(DataTable table) : base(table.TableName) { }
        #endregion
        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public InvRecvDetailRow this[int index]
        {
            get
            {
                return ((InvRecvDetailRow)(this.Rows[index]));
            }
        }


        public DataColumn InvRecvDetailID
        {
            get
            {
                return this.InvRecvDetailID;
            }
        }

        public DataColumn InvRecievedID
        {
            get
            {
                return this.colInvRecievedID;
            }
        }

        public DataColumn ItemID
        {
            get
            {
                return this.colItemID;
            }
        }

        public DataColumn VendorItemID  //Sprint-21 - 2207 03-Aug-2015 JY Added
        {
            get
            {
                return this.colVendorItemId;
            }
        }

        public DataColumn ItemDescription
        {
            get
            {
                return this.colItemDescription;
            }
        }

        public DataColumn QtyOrdered
        {
            get
            {
                return this.colQtyOrdered;
            }
        }

        public DataColumn QTY
        {
            get
            {
                return this.colQTY;
            }
        }

        public DataColumn Cost
        {
            get
            {
                return this.colCost;
            }
        }

        public DataColumn TotalCost  //Sprint-21 - 2002 21-Jul-2015 JY Added
        {
            get
            {
                return this.colTotalCost;
            }
        }

        public DataColumn SalePrice
        {
            get
            {
                return this.colSalePrice;
            }
        }

        public DataColumn Comments
        {
            get
            {
                return this.colComments;
            }
        }

        public DataColumn VendorCode  //Sprint-21 - 2207 03-Aug-2015 JY Added
        {
            get
            {
                return this.colVendorCode;
            }
        }

        //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
        public DataColumn ExpDate
        {
            get
            {
                return this.colExpDate;
            }
        }

        #region PRIMEPOS-2396 12-Jun-2018 JY Added
        public DataColumn QtyInStock
        {
            get
            {
                return this.colQtyInStock;
            }
        }
        public DataColumn LastInvUpdatedQty
        {
            get
            {
                return this.colLastInvUpdatedQty;
            }
        }
        #endregion

        #region PRIMEPOS-2419 28-May-2019 JY Added
        public DataColumn DeptID
        {
            get
            {
                return this.colDeptID;
            }
        }

        public DataColumn SubDepartmentID
        {
            get
            {
                return this.colSubDepartmentID;
            }
        }
        public DataColumn IsEBTItem
        {
            get
            {
                return this.colIsEBTItem;
            }
        }
        #endregion

        #region PRIMEPOS-2725 29-Aug-2019 JY Added
        public DataColumn DeptCode
        {
            get
            {
                return this.colDeptCode;
            }
        }

        public DataColumn DeptName
        {
            get
            {
                return this.colDeptName;
            }
        }

        public DataColumn SubDept
        {
            get
            {
                return this.colSubDept;
            }
        }
        #endregion

        #endregion //Properties

        #region Add and Get Methods 

        public void AddRow(InvRecvDetailRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(InvRecvDetailRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.InvRecvDetailID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public InvRecvDetailRow GetRowByID(System.Int32 InvRecvDetailID)
        {
            return (InvRecvDetailRow)this.Rows.Find(new object[] { InvRecvDetailID });
        }

        public InvRecvDetailRow AddRow(System.Int32 InvRecvDetailID
                                        , System.Int32 InvRecievedID
                                        , System.Int32 QTY
                                        , System.Decimal SalePrice
                                        , System.Decimal Cost
                                        , System.String ItemID
                                        , System.String Comments
                                        , System.Int32 QtyOrdered)
        {
            InvRecvDetailRow row = (InvRecvDetailRow)this.NewRow();
            //row.ItemArray = new object[] { InvRecvDetailID, InvRecievedID, Cost, Comments, QTY, SalePrice, ItemID, "", QtyOrdered };  //Sprint-21 - 2207 04-Aug-2015 JY Commented
            row.ItemArray = new object[] { 0, 0, "", 0, "", 0, 0, 0, 0, 0, "", "" };  //Sprint-21 - 2207 04-Aug-2015 JY Commented
                                                                                      /*
                                                                                      row.InvRecvDetailID=InvRecvDetailID;
                                                                                      row.InvRecievedID=InvRecievedID;
                                                                                      row.ItemID=ItemID;
                                                                                      row.Cost=Cost;
                                                                                      row.SalePrice=SalePrice;
                                                                                      row.Comments=Comments;
                                                                                      row.QTY=QTY;
                                                                                      */
            this.Rows.Add(row);
            return row;
        }

        public InvRecvDetailRow AddRow()
        {
            InvRecvDetailRow row = (InvRecvDetailRow)this.NewRow();
            //row.ItemArray = new object[] {0,0,0,0,0, "", "",0};
            row.ItemArray = new object[] { 0, 0, 0, "", 0, 0, "", "", 0 };

            /*
			row.InvRecvDetailID=InvRecvDetailID;
			row.InvRecievedID=InvRecievedID;
			row.ItemID=ItemID;
			row.Cost=Cost;
			row.SalePrice=SalePrice;
			row.Comments=Comments;
			row.QTY=QTY;
			*/
            this.Rows.Add(row);
            return row;
        }

        public void MergeTable(DataTable dt)
        {

            InvRecvDetailRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (InvRecvDetailRow)this.NewRow();

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID].ToString() == "") ? "0" : dr[0].ToString());

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_ItemID] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_ItemID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_ItemID] = Convert.ToString((dr[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].ToString() == "") ? "" : dr[clsPOSDBConstants.InvRecvDetail_Fld_ItemID].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Description] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Description] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Description] = dr[clsPOSDBConstants.Item_Fld_Description].ToString();

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID] = Convert.ToInt32((dr[clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID].ToString() == "") ? "0" : dr[0].ToString());

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_Comments] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_Comments] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_Comments] = Convert.ToString(dr[clsPOSDBConstants.InvRecvDetail_Fld_Comments].ToString());

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_QTY] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_QTY] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_QTY] = Convert.ToInt32((dr[clsPOSDBConstants.InvRecvDetail_Fld_QTY].ToString() == "") ? "0" : dr[clsPOSDBConstants.InvRecvDetail_Fld_QTY].ToString());

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_Cost] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_Cost] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_Cost] = Convert.ToDecimal((dr[clsPOSDBConstants.InvRecvDetail_Fld_Cost].ToString() == "") ? "0" : dr[clsPOSDBConstants.InvRecvDetail_Fld_Cost].ToString());

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice] = Convert.ToDecimal((dr[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].ToString() == "") ? "0" : dr[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice].ToString());

                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered] = Convert.ToInt32((dr[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered].ToString() == "") ? "0" : dr[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered].ToString());

                #region Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
                if (dr[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate] = Convert.ToDateTime(dr[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate].ToString());
                #endregion

                this.AddRow(row);
            }
        }

        #endregion
        public override DataTable Clone()
        {
            InvRecvDetailTable cln = (InvRecvDetailTable)base.Clone();
            cln.InitVars();
            return cln;
        }
        protected override DataTable CreateInstance()
        {
            return new InvRecvDetailTable();
        }

        internal void InitVars()
        {
            this.colSalePrice = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice];
            this.colQTY = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_QTY];
            this.colItemID = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_ItemID];
            this.colItemDescription = this.Columns[clsPOSDBConstants.Item_Fld_Description];
            this.colInvRecievedID = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID];
            this.colInvRecvDetailID = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID];
            this.colCost = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_Cost];
            this.colComments = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_Comments];
            this.colQtyOrdered = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered];
            this.colTotalCost = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_TotalCost];  //Sprint-21 - 2002 21-Jul-2015 JY Added
            this.colVendorItemId = this.Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID];  //Sprint-21 - 2207 03-Aug-2015 JY Added
            this.colVendorCode = this.Columns[clsPOSDBConstants.Vendor_Fld_VendorCode];  //Sprint-21 - 2207 03-Aug-2015 JY Added
            this.colExpDate = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_ExpDate];    //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
            this.colQtyInStock = this.Columns[clsPOSDBConstants.Item_Fld_QtyInStock];   //PRIMEPOS-2396 12-Jun-2018 JY Added
            this.colLastInvUpdatedQty = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty];    //PRIMEPOS-2396 12-Jun-2018 JY Added
            this.colDeptID = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_DeptID];  //PRIMEPOS-2419 28-May-2019 JY Added
            this.colSubDepartmentID = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID];    //PRIMEPOS-2419 28-May-2019 JY Added
            this.colIsEBTItem = this.Columns[clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem];    //PRIMEPOS-2419 28-May-2019 JY Added
            this.colDeptCode = this.Columns[clsPOSDBConstants.Department_Fld_DeptCode];   //PRIMEPOS-2725 29-Aug-2019 JY Added
            this.colDeptName = this.Columns[clsPOSDBConstants.Department_Fld_DeptName];   //PRIMEPOS-2725 29-Aug-2019 JY Added
            this.colSubDept = this.Columns["SubDept"];    //PRIMEPOS-2725 29-Aug-2019 JY Added
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colInvRecvDetailID = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInvRecvDetailID);
            this.colInvRecvDetailID.AllowDBNull = true;

            this.colInvRecievedID = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInvRecievedID);
            this.colInvRecievedID.AllowDBNull = true;

            this.colItemID = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemID);
            this.colItemID.AllowDBNull = true;

            #region  Sprint-21 - 2207 03-Aug-2015 JY Added
            this.colVendorItemId = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_VendorItemID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorItemId);
            this.colVendorItemId.AllowDBNull = true;
            #endregion

            this.colItemDescription = new DataColumn(clsPOSDBConstants.Item_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemDescription);
            this.colItemDescription.AllowDBNull = true;

            this.colQtyOrdered = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQtyOrdered);
            this.colQtyOrdered.AllowDBNull = true;

            this.colQTY = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_QTY, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQTY);
            this.colQTY.AllowDBNull = true;

            this.colCost = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_Cost, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCost);
            this.colCost.AllowDBNull = true;

            #region  Sprint-21 - 2002 21-Jul-2015 JY Added
            this.colTotalCost = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_TotalCost, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalCost);
            this.colTotalCost.AllowDBNull = true;
            #endregion

            this.colSalePrice = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_SalePrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSalePrice);
            this.colSalePrice.AllowDBNull = true;

            this.colComments = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_Comments, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colComments);
            this.colComments.AllowDBNull = true;

            #region  Sprint-21 - 2207 03-Aug-2015 JY Added
            this.colVendorCode = new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorCode);
            this.colVendorCode.AllowDBNull = true;
            #endregion

            #region Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
            this.colExpDate = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_ExpDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExpDate);
            this.colExpDate.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-2396 12-Jun-2018 JY Added
            this.colQtyInStock = new DataColumn(clsPOSDBConstants.Item_Fld_QtyInStock, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQtyInStock);
            this.colQtyInStock.AllowDBNull = true;
            this.colLastInvUpdatedQty = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_LastInvUpdatedQty, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastInvUpdatedQty);
            this.colLastInvUpdatedQty.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-2419 28-May-2019 JY Added
            this.colDeptID = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_DeptID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDeptID);
            this.colDeptID.AllowDBNull = true;
            this.colSubDepartmentID = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSubDepartmentID);
            this.colSubDepartmentID.AllowDBNull = true;
            this.colIsEBTItem = new DataColumn(clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsEBTItem);
            this.colIsEBTItem.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-2725 29-Aug-2019 JY Added
            this.colDeptCode = new DataColumn(clsPOSDBConstants.Department_Fld_DeptCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDeptCode);
            this.colDeptCode.AllowDBNull = true;

            this.colDeptName = new DataColumn(clsPOSDBConstants.Department_Fld_DeptName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDeptName);
            this.colDeptName.AllowDBNull = true;

            this.colSubDept = new DataColumn("SubDept", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSubDept);
            this.colSubDept.AllowDBNull = true;
            #endregion

            //this.PrimaryKey = new DataColumn[] {this.colInvRecvDetailID};
        }

        public InvRecvDetailRow NewInvRecvDetailRow()
        {
            return (InvRecvDetailRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new InvRecvDetailRow(builder);
        }
    }
}
