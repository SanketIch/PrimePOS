
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class PODetailTable : DataTable, System.Collections.IEnumerable 
	{
        //Commented by Prashant(SRT) Date:4-06-02009
        private DataColumn colPODetailID;
        private DataColumn colOrderID;
        //End of Commented by Prashant(SRT) Date:4-06-02009

        //Added by Prashant(SRT) Date:4-06-02009
        private DataColumn colLastOrdQty;
        private DataColumn colLastOrdOrder;
        private DataColumn colBestVendor;
        private DataColumn colBestVendPrice;
        private DataColumn colVendorName;
        private DataColumn colVendorID;
        //End of Added by Prashant(SRT) Date:4-06-02009
		private DataColumn colQTY;
		private DataColumn colCost;
        private DataColumn colLastCostPrice;//Added by Ravindra PRIMEPOS-1043
        private DataColumn colPrice;
		private DataColumn colItemID;
		private DataColumn colItemDescription;
		private DataColumn colComments;
		private DataColumn colAckQty;
		private DataColumn colAckStatus;
		private DataColumn colVendorItemCode;

        private DataColumn colChangedProductQualifier;
        private DataColumn colChangedProductID;
        private static int poDetailID;
        //Added By SRT(Gaurav) Date: 03-Jul-2009
        private DataColumn colQtyInStock;
        private DataColumn colReorderLevel;
        private DataColumn colQtySold100Days;
        //End Of Added By SRT(Gaurav)

        //Added By SRT(Gaurav) Date: 03-Jul-2009
        private DataColumn colPacketSize;
        private DataColumn colPacketunit;
        private DataColumn colPacketQuant;
        ////End Of Added By SRT(Gaurav)
        private DataColumn colSoldItems;
       
        //Added by atul 22-oct-2010
        private DataColumn colItemDescType;
        private DataColumn colIdescription;
        //End of Added by atul 22-oct-2010

        //Added By Amit Date 15 May 2011
        private DataColumn colQtyOnOrder;
        private DataColumn colMinOrdQty;
        //End
        //Added by amit 1 july 2011
        private DataColumn colDeptName;
        private DataColumn colSubDeptName;
        //End
        //Added by Amit date 27 july 2011
        private DataColumn colRetailPrice;
        private DataColumn colItemPrice;
        private DataColumn colDiscount;
        //End
        //Added By Amit Date 29 Nov 2011
        private DataColumn colInvRecDate;
        //End        
        //Add by Ravindra to Save processed Quantity 3 April 2013
        private DataColumn colProcessedQty;
        
		#region Constructors 
		public PODetailTable() : base(clsPOSDBConstants.PODetail_tbl) { this.InitClass(); }
		internal PODetailTable(DataTable table) : base(table.TableName) {}
		#endregion
		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}
		public PODetailRow this[int index] 
		{
			get 
			{
				return ((PODetailRow)(this.Rows[index]));
			}
		}
		public DataColumn PODetailID 
		{
			get 
			{
				return this.colPODetailID;
			}
		}      
		public DataColumn OrderID 
		{
			get 
			{
				return this.colOrderID;
			}
		}

		public DataColumn QTY
		{
			get 
			{
				return this.colQTY;
			}
		}

		public DataColumn AckQTY
		{
			get 
			{
				return this.colAckQty;
			}
		}
        public DataColumn VendorID
        {
            get
            {
                return this.colVendorID;
            }
        }
        public DataColumn BestVendor
        {
            get
            {
                return this.colBestVendor;
            }
        }
        public DataColumn BestVendorPrice
        {
            get
            {
                return this.colBestVendPrice;
            }
        }
		public DataColumn Cost
		{
			get 
			{
				return this.colCost;
			}
		}
        public DataColumn LastCostPrice
        {
            get
            {
                return this.colLastCostPrice;
            }
        }
        public DataColumn Price
        {
            get
            {
                return this.colPrice;
            }
        }
		public DataColumn Comments
		{
			get 
			{
				return this.colComments;
			}
		}

		public DataColumn AckStatus
		{
			get 
			{
				return this.colAckStatus;
			}
		}

		public DataColumn ItemID
		{
			get 
			{
				return this.colItemID;
			}
		}

		public DataColumn ItemDescription
		{
			get 
			{
				return this.colItemDescription;
			}
		}
        public DataColumn LastOrdOrder
        {
            get
            {
                return this.colLastOrdOrder; 
            }
        }
        public DataColumn LastOrdQty
        {
            get
            {
                return this.colLastOrdQty; 
            }
        }
        public DataColumn VendorName
        {
            get
            {
                return this.colVendorName;
            }
        }    
		public DataColumn VendorItemCode
		{
			get 
			{
				return this.colVendorItemCode;
			}
		}
        public DataColumn ChangedProductQualifier
        {
            get
            {
                return this.colChangedProductQualifier;
            }
        }
        public DataColumn ChangedProductID
        {
            get
            {
                return this.colChangedProductID;
            }
        }

        //Added By SRT(Gaurav) Date: 03-Jul-2009
        public DataColumn QtyInStock
        {
            get
            {
                return this.colQtyInStock;
            }
        }
        public DataColumn ReorderLevel
        {
            get
            {
                return this.colReorderLevel;
            }
        }
        public DataColumn QtySold100Days
        {
            get
            {
                return this.colQtySold100Days;
            }
        }

        public DataColumn PacketSize
        {
            get
            {
                return this.colPacketSize;
            }
        }

        public DataColumn Packetunit
        {
            get
            {
                return this.colPacketunit;
            }
        }

        public DataColumn PacketQuant
        {
            get
            {
                return this.colPacketQuant;
            }
        }
        public DataColumn SoldItems
        {
            get
            {
                return this.colSoldItems;
            }
        }

        //added by atul 22-oct-2010
        public DataColumn ItemDescType
        {
            get
            {
                return this.colItemDescType;
            }
        }
        public DataColumn Idescription
        {
            get
            {
                return this.colIdescription;
            }
        }
        //end of added by atul 22-oct-2010
       
        //End Of Added By SRT(Gaurav)
        //Added by Amit Date 5 july 2011
        public DataColumn DeptName
        {
            get
            {
                return this.colDeptName;
            }
        }
        public DataColumn SubDeptName
        {
            get
            {
                return this.colSubDeptName;
            }
        }

        public DataColumn MinOrdQty
        {
            get
            {
                return this.colMinOrdQty;
            }
        }
        public DataColumn QtyOnOrder
        {
            get
            {
                return this.colQtyOnOrder;
            }
        }
        //End
        //Added by Amit date 27 July 2011
        public DataColumn RetailPrice
        {
            get
            {
                return this.colRetailPrice;
            }
        }

        public DataColumn ItemPrice
        {
            get
            {
                return this.colItemPrice;
            }
        }

        public DataColumn Discount
        {
            get
            {
                return this.colDiscount;
            }
        }
        //End
        //Added By AMit Date 29 Nov 2011
        public DataColumn InvRecDate
        {
            get 
            {
                return this.colInvRecDate;
            }
        }
        //End      
        //Added by Ravindra to Save Processed Quantity 3 April 2013
        public DataColumn ProcessedQty
        {
            get
            {
                return this.colProcessedQty;
            }
        }

		#endregion //Properties

		#region Add and Get Methods 
		public  void AddRow(PODetailRow row) 
		{
			AddRow(row, false);
		}
		public  void AddRow(PODetailRow row, bool preserveChanges) 
		{
            if (this.GetRowByID(row.PODetailID) == null)
            {
                if (row.PODetailID == 0)
                {
                    for (int count = 0; count < this.Rows.Count; count++)
                    {
                        if (poDetailID < (int)this.Rows[count][clsPOSDBConstants.PODetail_Fld_PODetailID])
                        {
                            poDetailID = (int)this.Rows[count][clsPOSDBConstants.PODetail_Fld_PODetailID];
                        }
                    }
                    row.PODetailID = poDetailID + 1;
                }

                this.Rows.Add(row);

                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
		}
		public PODetailRow GetRowByID(System.Int32 PODetailID) 
		{
			return (PODetailRow)this.Rows.Find(new object[] {PODetailID});
		}

        public PODetailRow AddRow(System.String VendorID
                                        , System.String VendorName
                                        , System.String VendorItemCode
                                        , System.Int32 BestVendPrice
                                        , System.String BestVendor
                                        , System.Int32 LastOrdOrder
                                        , System.Int32 LastOrdQty
                                        , System.Int32 PODetailID                                        
                                        , System.Int32 OrderId
                                        , System.Int32 QTY
                                        , System.Decimal Cost
                                        , System.String ItemID
                                        , System.String Comments
                                        , System.String ItemDescription
            , System.Decimal LastCostPrice
                                       )//  LastCostPrice added by ravindra for PRIMEPOS-10431 sub-taskShow COST from purchase order(850) against the acknowledgement(855\810)
        {
            PODetailRow row = (PODetailRow)this.NewRow();
            //row.ItemArray = new object[] {PODetailID,Convert.ToInt32( VendorID), VendorName, BestVendPrice, BestVendor, LastOrdOrder, LastOrdQty, Cost, Comments, QTY, ItemID, ItemDescription };
            row.VendorID = Convert.ToInt16(VendorID);
            row.VendorItemCode = VendorItemCode; 
            row.VendorName = VendorName;
            row.BestPrice = BestVendPrice.ToString();
            row.BestVendor = BestVendor;
            row.PODetailID = PODetailID;
            row.OrderID = OrderId;
            row.ItemID = ItemID;
            row.ItemDescription = ItemDescription;
            row.Cost = Cost;
            row.Comments = Comments;
            row.QTY = QTY;
            row.LastCostPrice = LastCostPrice;
           

            this.Rows.Add(row);
            return row;
        }

        public PODetailRow AddRow(System.String VendorID
                                       , System.String VendorName
                                       , System.Int32 BestVendPrice
                                       , System.String BestVendor
                                       , System.Int32 LastOrdOrder
                                       , System.Int32 LastOrdQty
                                       , System.Int32 PODetailID
                                       , System.Int32 OrderId
                                       , System.Int32 QTY
                                       , System.Decimal Cost
                                       , System.String ItemID
                                       , System.String Comments
                                       , System.String ItemDescription
                                       , System.Decimal Price
                                       ,System.Int32 soldItems
                                       ,System.String VendorItemCode
                                        ,System.Int32 QtyInStock
                                        ,System.Int32 ReorderLevel
                                        ,System.Int32 MinOrdQty
                                        ,System.String DeptName
                                        ,System.String SubDeptName
                                        , System.Decimal RetailPrice,
                                        System.Decimal ItemPrice
                                        ,System.Decimal Discount
                                        ,System.Object InvRecDate
                                        , System.String Packetunit

                                        , System.String PacketSize, System.Int32 ProcessedQty, System.Decimal LastCostPrice) //LastCostPrice added by ravindra for PRIMEPOS-10431 sub-taskShow COST from purchase order(850) against the acknowledgement(855\810)
        {
            PODetailRow row = (PODetailRow)this.NewRow();
            //row.ItemArray = new object[] {PODetailID,Convert.ToInt32( VendorID), VendorName, BestVendPrice, BestVendor, LastOrdOrder, LastOrdQty, Cost, Comments, QTY, ItemID, ItemDescription };
            row.VendorID = Convert.ToInt16(VendorID);
            row.VendorName = VendorName;
            row.BestPrice = BestVendPrice.ToString();
            row.BestVendor = BestVendor;
            row.PODetailID = PODetailID;
            row.OrderID = OrderId;
            row.ItemID = ItemID;
            row.ItemDescription = ItemDescription;
            row.Cost = Cost;
            row.Comments = Comments;
            row.QTY = QTY;
            row.Price = Price;
            row.SoldItems = soldItems;
            row.VendorItemCode = VendorItemCode;
            //Added By Amit Date 6 Jul 2011
            row.QtyInStock = QtyInStock;
            row.ReOrderLevel = ReorderLevel;
            row.MinOrdQty = MinOrdQty;
            row.DeptName = DeptName;
            row.SubDeptName = SubDeptName;
            //End
            //Added by Amit Date 27 July 2011
            row.RetailPrice = RetailPrice;
            row.ItemPrice = ItemPrice;
            row.Discount = Discount;
            //End
            //Added By Amit Date 29 Nov 2011
            row.InvRecDate = InvRecDate;
            row.Packetunit = Packetunit;
            row.PacketSize = PacketSize;
            //End            

            row.ProcessedQTY = ProcessedQty;//Added by Ravindra to Save Processed Quantity
            row.LastCostPrice = LastCostPrice;//Added by Ravindra PRIMEPOS-1043

            this.Rows.Add(row);
            return row;
        }

        public PODetailRow AddRow(System.Int32 PODetailID
                                        , System.Int32 OrderID
                                        , System.Int32 QTY
                                        , System.Decimal Cost
                                        , System.String ItemID
                                        , System.String Comments
                                        , System.String ItemDescription
            ,System.Decimal LastCostPrice
                                        )
        {
            PODetailRow row = (PODetailRow)this.NewRow();
            // row.ItemArray = new object[] { PODetailID, OrderID,Cost, Comments, QTY, ItemID, ItemDescription };
            row.PODetailID = PODetailID;
            row.OrderID = OrderID;
            row.ItemID = ItemID;
            row.ItemDescription = ItemDescription;
            row.Cost = Cost;
            row.Comments = Comments;
            row.QTY = QTY;
            row.LastCostPrice = LastCostPrice;

            this.Rows.Add(row);
            return row;
        }

        
		public  void MergeTable(DataTable dt) 
		{ 
      		PODetailRow row;			
            foreach(DataRow dr in dt.Rows) 
			{
				row = (PODetailRow)this.NewRow();

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_PODetailID))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_PODetailID] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_PODetailID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_PODetailID] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_PODetailID].ToString() == "") ? "0" : dr[0].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_ItemID))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_ItemID] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_ItemID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_ItemID] = Convert.ToString((dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString() == "") ? "" : dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.Item_Fld_Description))
                {
                    if (dr[clsPOSDBConstants.Item_Fld_Description] == DBNull.Value)
                        row[clsPOSDBConstants.Item_Fld_Description] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Item_Fld_Description] = dr[clsPOSDBConstants.Item_Fld_Description].ToString();
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_OrderID))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_OrderID] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_OrderID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_OrderID] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_OrderID].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_OrderID].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_VendorID))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_VendorID] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_VendorID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_VendorID] = Convert.ToInt32((dr[clsPOSDBConstants.POHeader_Fld_VendorID].ToString() == "") ? "0" : dr[clsPOSDBConstants.POHeader_Fld_VendorID].ToString());
                }                
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_Comments))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_Comments] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_Comments] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_Comments] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_Comments].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_QTY))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_QTY] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_QTY] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_QTY] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_QTY].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_QTY].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_Cost))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_Cost] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_Cost] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_Cost] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_Cost].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_Cost].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_LastCostPrice))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_LastCostPrice] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_LastCostPrice] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_LastCostPrice] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_LastCostPrice].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_LastCostPrice].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_Price))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_Price] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_Price] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_Price] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_Price].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_Price].ToString());
                }
                //Added by Prashant(SRT)Date:4-06-2009
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_VendorName))
                {
                    if (dr["VendorName"] == DBNull.Value)
                        row["VendorName"] = DBNull.Value;
                    else
                        row["VendorName"] = (dr["VendorName"].ToString() == "") ? "" : dr["VendorName"].ToString();
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_BestVendor))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_BestVendor] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_BestVendor] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_BestVendor] = Convert.ToString((dr[clsPOSDBConstants.PODetail_Fld_BestVendor].ToString() == "") ? "" : dr[clsPOSDBConstants.PODetail_Fld_BestVendor].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_BestVendPrice))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_BestVendPrice] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_BestVendPrice] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_BestVendPrice] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_BestVendPrice].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_BestVendPrice].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_LastOrdVendor))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_LastOrdVendor] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_LastOrdVendor] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_LastOrdVendor] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_LastOrdVendor].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_LastOrdQty))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_LastOrdQty] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_LastOrdQty] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_LastOrdQty] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_LastOrdQty].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_LastOrdQty].ToString());
                }
                    //End Of Added by Prashant(SRT)Date:4-06-2009
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_AckQTY))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_AckQTY] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_AckQTY] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_AckQTY] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_AckQTY].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_AckQTY].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_AckStatus))
                {
                    row[clsPOSDBConstants.PODetail_Fld_AckStatus] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_AckStatus].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_VendorItemCode))
                {
                    row[clsPOSDBConstants.PODetail_Fld_VendorItemCode] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier))
                {
                    row[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_ChangedProductID))
                {
                    row[clsPOSDBConstants.PODetail_Fld_ChangedProductID] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_ChangedProductID].ToString());
                }
                //Added By SRT(Gaurav) Date: 03-Jul-2009
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_QtyInStock))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_QtyInStock] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_QtyInStock] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_QtyInStock] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_QtyInStock].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_QtyInStock].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_ReOrderLevel))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_ReOrderLevel] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_ReOrderLevel] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_ReOrderLevel] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_ReOrderLevel].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_ReOrderLevel].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_QtySold100Days))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_QtySold100Days] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_QtySold100Days] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_QtySold100Days] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_QtySold100Days].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_QtySold100Days].ToString());
                }
                //End Of Added By SRT(Gaurav)
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_PackSize))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_PackSize] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_PackSize] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_PackSize] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_PackSize].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_PackQuant))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_PackQuant] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_PackQuant] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_PackQuant] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_PackQuant].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_PackUnit))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_PackUnit] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_PackUnit] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_PackUnit] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_PackUnit].ToString());
                }

                //Added by Ravindra to Save ProcessedQTY 3 April 2013
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_ProcessedQty))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_ProcessedQty] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_ProcessedQty] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_ProcessedQty] = Convert.ToInt32((dr[clsPOSDBConstants.PODetail_Fld_ProcessedQty].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_ProcessedQty].ToString());
                }//Till here Added by Ravindra
                /*if (dr[clsPOSDBConstants.PODetail_Fld_ItemSold] == DBNull.Value)
                    row[clsPOSDBConstants.PODetail_Fld_ItemSold] = DBNull.Value;
                else
                    row[clsPOSDBConstants.PODetail_Fld_ItemSold] = Convert.ToInt32(dr[clsPOSDBConstants.PODetail_Fld_ItemSold].ToString());*/
                #region PRIMEPOS-3155 12-Oct-2022 JY Added
                if (dr.Table.Columns.Contains(clsPOSDBConstants.Item_Fld_MinOrdQty))
                {
                    if (dr[clsPOSDBConstants.Item_Fld_MinOrdQty] == DBNull.Value)
                        row[clsPOSDBConstants.Item_Fld_MinOrdQty] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Item_Fld_MinOrdQty] = Convert.ToInt32((dr[clsPOSDBConstants.Item_Fld_MinOrdQty].ToString() == "") ? "0" : dr[clsPOSDBConstants.Item_Fld_MinOrdQty].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_RetailPrice))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_RetailPrice] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_RetailPrice] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_RetailPrice] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_RetailPrice].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_RetailPrice].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_Discount))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_Discount] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_Discount] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_Discount] = Convert.ToDecimal((dr[clsPOSDBConstants.PODetail_Fld_Discount].ToString() == "") ? "0" : dr[clsPOSDBConstants.PODetail_Fld_Discount].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_InvRecDate))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_InvRecDate] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_InvRecDate] = DBNull.Value;
                    else
                        if (dr[clsPOSDBConstants.PODetail_Fld_InvRecDate].ToString().Trim() == "")
                        row[clsPOSDBConstants.PODetail_Fld_InvRecDate] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_InvRecDate] = Convert.ToDateTime(dr[clsPOSDBConstants.PODetail_Fld_InvRecDate].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_DeptName))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_DeptName] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_DeptName] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_DeptName] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_DeptName].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PODetail_Fld_SubDeptName))
                {
                    if (dr[clsPOSDBConstants.PODetail_Fld_SubDeptName] == DBNull.Value)
                        row[clsPOSDBConstants.PODetail_Fld_SubDeptName] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PODetail_Fld_SubDeptName] = Convert.ToString(dr[clsPOSDBConstants.PODetail_Fld_SubDeptName].ToString());
                }
                #endregion

                this.AddRow(row);
			}
		}		
		#endregion 
		public override DataTable Clone() 
		{
			PODetailTable cln = (PODetailTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new PODetailTable();
		}
		internal void InitVars() 
		{
			this.colQTY= this.Columns[clsPOSDBConstants.PODetail_Fld_QTY];
			this.colItemID= this.Columns[clsPOSDBConstants.PODetail_Fld_ItemID];
            this.colItemDescription = this.Columns[clsPOSDBConstants.Item_Fld_Description];
			this.colOrderID= this.Columns[clsPOSDBConstants.PODetail_Fld_OrderID];
			this.colPODetailID= this.Columns[clsPOSDBConstants.PODetail_Fld_PODetailID];
			this.colCost= this.Columns[clsPOSDBConstants.PODetail_Fld_Cost];
            this.colLastCostPrice = this.Columns[clsPOSDBConstants.PODetail_Fld_LastCostPrice];
            this.colPrice = this.Columns[clsPOSDBConstants.PODetail_Fld_Price];
			this.colComments= this.Columns[clsPOSDBConstants.PODetail_Fld_Comments];
			this.colAckQty= this.Columns[clsPOSDBConstants.PODetail_Fld_AckQTY];
			this.colAckStatus= this.Columns[clsPOSDBConstants.PODetail_Fld_AckStatus];
			this.colVendorItemCode= this.Columns[clsPOSDBConstants.PODetail_Fld_VendorItemCode];
            //Added by Prashant[SRT] Date:5-06-2009
            this.colVendorID = this.Columns[clsPOSDBConstants.POHeader_Fld_VendorID];
            this.colVendorName = this.Columns[clsPOSDBConstants.PODetail_Fld_VendorName];
            this.colBestVendor = this.Columns[clsPOSDBConstants.PODetail_Fld_BestVendor];
            this.colBestVendPrice = this.Columns[clsPOSDBConstants.PODetail_Fld_BestVendPrice];
            this.colLastOrdOrder = this.Columns[clsPOSDBConstants.PODetail_Fld_LastOrdVendor];
            this.colLastOrdQty = this.Columns[clsPOSDBConstants.PODetail_Fld_LastOrdQty];
            //End of Added by Prashant[SRT] Date:5-06-2009
            this.colChangedProductQualifier = this.Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier];
            this.colChangedProductID = this.Columns[clsPOSDBConstants.PODetail_Fld_ChangedProductID];
            //Added By SRT(Gaurav) Date: 03-Jul-2009
            this.colQtyInStock = this.Columns[clsPOSDBConstants.PODetail_Fld_QtyInStock];
            this.colReorderLevel = this.Columns[clsPOSDBConstants.PODetail_Fld_ReOrderLevel];
            this.colQtySold100Days = this.Columns[clsPOSDBConstants.PODetail_Fld_QtySold100Days];

            this.colPacketSize = this.Columns[clsPOSDBConstants.PODetail_Fld_PackSize];
            this.colPacketQuant = this.Columns[clsPOSDBConstants.PODetail_Fld_PackQuant];
            this.colPacketunit = this.Columns[clsPOSDBConstants.PODetail_Fld_PackUnit];
            //End Of Added By SRT(Gaurav)
            this.colSoldItems = this.Columns[clsPOSDBConstants.PODetail_Fld_ItemSold];
            //Added by atul 22-oct-2010
            this.colItemDescType = this.Columns[clsPOSDBConstants.PODetail_Fld_ItemDescType];
            this.colIdescription = this.Columns[clsPOSDBConstants.PODetail_Fld_Idescription];
            //End of Added by atul 22-oct-2010

            //Added by Amit Date 15 May 2011
            this.colQtyOnOrder = this.Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder];
            this.colMinOrdQty = this.Columns[clsPOSDBConstants.Item_Fld_MinOrdQty];
            //End
            //Added By Amit Date 1 July 2011
            this.colDeptName = this.Columns[clsPOSDBConstants.PODetail_Fld_DeptName];
            this.colSubDeptName = this.Columns[clsPOSDBConstants.PODetail_Fld_SubDeptName];
            //Added by Amit Date 27 july 2011
            this.colRetailPrice = this.Columns[clsPOSDBConstants.PODetail_Fld_RetailPrice];
            this.colItemPrice = this.Columns[clsPOSDBConstants.PODetail_Fld_ItemPrice];
            this.colDiscount = this.Columns[clsPOSDBConstants.PODetail_Fld_Discount];
            //End
            //Added By Amit Date 29 Nov 2011
            this.colInvRecDate =this.Columns[clsPOSDBConstants.PODetail_Fld_InvRecDate];
            //End            

            this.colProcessedQty = this.Columns[clsPOSDBConstants.PODetail_Fld_ProcessedQty];//Added by Ravindra to Save Proceeed Qty 3 April 2013

		}

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
            this.colPODetailID = new DataColumn(clsPOSDBConstants.PODetail_Fld_PODetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPODetailID);
            this.colPODetailID.AllowDBNull = true;

            this.colOrderID = new DataColumn(clsPOSDBConstants.PODetail_Fld_OrderID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOrderID);
            this.colOrderID.AllowDBNull = true;

            this.colItemID = new DataColumn(clsPOSDBConstants.PODetail_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemID);
            this.colItemID.AllowDBNull = true;

            this.colPrice = new DataColumn(clsPOSDBConstants.PODetail_Fld_Price, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrice);
            this.colPrice.AllowDBNull = true;

			this.colCost = new DataColumn(clsPOSDBConstants.PODetail_Fld_Cost, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCost);
			this.colCost.AllowDBNull = true;

            this.colLastCostPrice = new DataColumn(clsPOSDBConstants.PODetail_Fld_LastCostPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastCostPrice);
            this.colLastCostPrice.AllowDBNull = true;

			this.colComments = new DataColumn(clsPOSDBConstants.PODetail_Fld_Comments, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colComments);
			this.colComments.AllowDBNull = true;

			this.colQTY= new DataColumn(clsPOSDBConstants.PODetail_Fld_QTY, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colQTY);
			this.colQTY.AllowDBNull = true;

            this.colVendorID = new DataColumn(clsPOSDBConstants.POHeader_Fld_VendorID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorID);
            this.colVendorID.AllowDBNull = true;			

            this.colItemDescription = new DataColumn(clsPOSDBConstants.Item_Fld_Description,typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemDescription);
			this.colItemDescription.AllowDBNull = true;

            this.colAckQty = new DataColumn(clsPOSDBConstants.PODetail_Fld_AckQTY, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAckQty);
            this.colAckQty.AllowDBNull = true;

            this.colAckStatus = new DataColumn(clsPOSDBConstants.PODetail_Fld_AckStatus, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAckStatus);
            this.colAckStatus.AllowDBNull = true;

            this.colVendorName = new DataColumn(clsPOSDBConstants.PODetail_Fld_VendorName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVendorName);
            this.colVendorName.AllowDBNull = true;

			this.colVendorItemCode= new DataColumn(clsPOSDBConstants.PODetail_Fld_VendorItemCode, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colVendorItemCode);
			this.colVendorItemCode.AllowDBNull = true;

            this.colBestVendPrice = new DataColumn(clsPOSDBConstants.PODetail_Fld_BestVendPrice, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colBestVendPrice);
            this.colBestVendPrice.AllowDBNull = true;

            this.colBestVendor = new DataColumn(clsPOSDBConstants.PODetail_Fld_BestVendor, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colBestVendor);
            this.colBestVendor.AllowDBNull = true;

            this.colLastOrdOrder = new DataColumn(clsPOSDBConstants.PODetail_Fld_LastOrdVendor, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastOrdOrder);
            this.colLastOrdOrder.AllowDBNull = true;

            this.colChangedProductQualifier = new DataColumn(clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colChangedProductQualifier);
            this.colChangedProductQualifier.AllowDBNull = true;

            this.colLastOrdQty = new DataColumn(clsPOSDBConstants.PODetail_Fld_LastOrdQty, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastOrdQty);
            this.colLastOrdQty.AllowDBNull = true;

            this.colChangedProductID = new DataColumn(clsPOSDBConstants.PODetail_Fld_ChangedProductID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colChangedProductID);
            this.colChangedProductID.AllowDBNull = true;

            //Added By SRT(Gaurav) Date: 03-Jul-2009
            this.colQtyInStock = new DataColumn(clsPOSDBConstants.PODetail_Fld_QtyInStock, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQtyInStock);
            this.colQtyInStock.AllowDBNull = true;

            this.colReorderLevel = new DataColumn(clsPOSDBConstants.PODetail_Fld_ReOrderLevel, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colReorderLevel);
            this.colReorderLevel.AllowDBNull = true;

            this.colQtySold100Days = new DataColumn(clsPOSDBConstants.PODetail_Fld_QtySold100Days, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQtySold100Days);
            this.colQtySold100Days.AllowDBNull = true;
            //End Of Added By SRT(Gaurav)


            this.colPacketSize = new DataColumn(clsPOSDBConstants.PODetail_Fld_PackSize, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPacketSize);
            this.colPacketSize.AllowDBNull = true;

            this.colPacketQuant = new DataColumn(clsPOSDBConstants.PODetail_Fld_PackQuant, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPacketQuant);
            this.colPacketQuant.AllowDBNull = true;

            this.colPacketunit = new DataColumn(clsPOSDBConstants.PODetail_Fld_PackUnit,typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPacketunit);
            this.colPacketunit.AllowDBNull = true;

            this.colSoldItems = new DataColumn(clsPOSDBConstants.PODetail_Fld_ItemSold, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSoldItems);
            this.colSoldItems.AllowDBNull = true;

            //added by atul 12-oct-2010
            this.colItemDescType = new DataColumn(clsPOSDBConstants.PODetail_Fld_ItemDescType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemDescType);
            this.colItemDescType.AllowDBNull = true;

            this.colIdescription = new DataColumn(clsPOSDBConstants.PODetail_Fld_Idescription, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIdescription);
            this.colIdescription.AllowDBNull = true;
            //end of added by atul 12-oct-2010

            //Added by Amit Date 15 May 2011
            this.colQtyOnOrder = new DataColumn(clsPOSDBConstants.Item_Fld_QtyOnOrder, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQtyOnOrder);
            this.colQtyOnOrder.AllowDBNull = true;
            
            this.colMinOrdQty = new DataColumn(clsPOSDBConstants.Item_Fld_MinOrdQty, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMinOrdQty);
            this.colMinOrdQty.AllowDBNull = true;
            //End
            //Added by Amit Date 1 July 2011
            this.colDeptName = new DataColumn(clsPOSDBConstants.PODetail_Fld_DeptName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDeptName);
            this.colDeptName.AllowDBNull = true;

            this.colSubDeptName = new DataColumn(clsPOSDBConstants.PODetail_Fld_SubDeptName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSubDeptName);
            this.colSubDeptName.AllowDBNull = true;
            //End
            //Added by Amit date 27 July 2011
            this.colRetailPrice = new DataColumn(clsPOSDBConstants.PODetail_Fld_RetailPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRetailPrice);
            this.colRetailPrice.AllowDBNull = true;

            this.colItemPrice = new DataColumn(clsPOSDBConstants.PODetail_Fld_ItemPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemPrice);
            this.colItemPrice.AllowDBNull = true;

            this.colDiscount = new DataColumn(clsPOSDBConstants.PODetail_Fld_Discount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDiscount);
            this.colDiscount.AllowDBNull = true;
            //End
            //Start Added By Amit Date 29 Nov 2011
            this.colInvRecDate = new DataColumn(clsPOSDBConstants.PODetail_Fld_InvRecDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInvRecDate);
            this.colInvRecDate.AllowDBNull = true;
            //End         


            //Added by Ravindra to Save Processed Qty 3 April 2013
            this.colProcessedQty = new DataColumn(clsPOSDBConstants.PODetail_Fld_ProcessedQty, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProcessedQty);
            this.colAckQty.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colPODetailID };
		}

		public  PODetailRow NewPODetailRow() 
		{
			return (PODetailRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
            return new PODetailRow(builder);
		}
	}
}
