// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class ItemTable : DataTable, System.Collections.IEnumerable
    {
        #region columns declaration
        private DataColumn colItemID;
        private DataColumn colDepartmentID;
        private DataColumn colSubDepartmentID;
        private DataColumn colDescription;
        private DataColumn colItemtype;
        private DataColumn colProductCode;
        private DataColumn colSaleTypeCode;
        private DataColumn colSeasonCode;
        private DataColumn colUnit;
        private DataColumn colFreight;
        private DataColumn colSellingPrice;
        private DataColumn colAvgPrice;
        private DataColumn colLastCostPrice;
        private DataColumn colisTaxable;
        private DataColumn colTaxID;
        private DataColumn colisDiscountable;
        private DataColumn colDiscount;
        private DataColumn colSaleStartDate;
        private DataColumn colSaleEndDate;
        private DataColumn colOnSalePrice;
        private DataColumn colQtyInStock;
        private DataColumn colLocation;
        private DataColumn colMinOrdQty;
        private DataColumn colReOrderLevel;
        private DataColumn colQtyOnOrder;
        private DataColumn colExptDeliveryDate;
        private DataColumn colLastVendor;
        //Added by Prashant(SRT) Date:1-06-2009
        private DataColumn colPreferredVendor;
        //End of Added by Prashant(SRT) Date:1-06-2009
        private DataColumn colLastRecievDate;
        private DataColumn colLastSellingDate;
        private DataColumn colRemarks;
        private DataColumn colisOnSale;
        private DataColumn colExclFromAutoPO;
        private DataColumn colExclFromRecpt;
        private DataColumn colIsOTCItem;
        private DataColumn colUpdatePrice;

        private DataColumn colPckSize;
        private DataColumn colPckQty;
        private DataColumn colPckUnit;
        //Added by Krishna on 5 October 2011
        //private DataColumn colExpDate;
        //private DataColumn colLotNumber;
        //Added by Krishna on 5 October 2011 

        private DataColumn colExpDate;  //Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date

        //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 18-04-2011
        private DataColumn ItemAddedDate;
        private DataColumn colTaxPolicy;//Added on 18 August 2011
        //End OfAdded By Shitaljit(QuicSolv)
        private DataColumn colIsEBTItem;
        private DataColumn colManufacturerName;
        //Added by Ravindra fro Sale limit 22 March 2013
        private DataColumn colSaleLimitQty;
        private DataColumn colDiscountPolicy; //Added By Shitaljit(QuicSolv) on 3 April 2013
        //Added By shitaljit for diff CL poins for RX and OTC items.
         private DataColumn  colIsDefaultCLPoint;
         private DataColumn  colPointsPerDollar;
        //End 
         private DataColumn colCLPointPolicy;   //Sprint-18 - 2041 28-Oct-2014 JY  Added   
         private DataColumn colIsActive;    //Sprint-21 - 2173 06-Jul-2015 JY Added
        private DataColumn colIsNonRefundable;    //PRIMEPOS-2592 01-Nov-2018 JY Added 

        #region Added for Solutran - PRIMEPOS-2663 - NileshJ
        private DataColumn colS3TransID;
        private DataColumn colS3PurAmount;
        private DataColumn colS3TaxAmount;
        private DataColumn colS3DiscountAmount;
        #endregion
        #endregion

        #region Constants
        /// <value>The constant used for Item table. </value>
        private const String _TableName = "ITEM";
        #endregion

        #region Constructors
        internal ItemTable() : base(_TableName) { this.InitClass(); }
        internal ItemTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        /// 
        /// Public Property for get all Rows in Table
        /// 
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ItemRow this[int index]
        {
            get
            {
                return ((ItemRow)(this.Rows[index]));
            }
        }

        /// <summary>
        /// Public Property DataColumn ItemID
        /// </summary>	
        public DataColumn ItemID
        {
            get
            {
                return this.colItemID;
            }
        }

        /// <summary>
        /// Public Property DataColumn DepartmentID
        /// </summary>	
        public DataColumn DepartmentID
        {
            get
            {
                return this.colDepartmentID;
            }
        }

        /// <summary>
        /// Public Property DataColumn SubDepartmentID
        /// </summary>	
        public DataColumn SubDepartmentID
        {
            get
            {
                return this.colSubDepartmentID;
            }
        }

        /// <summary>
        /// Public Property DataColumn Description
        /// </summary>	
        public DataColumn Description
        {
            get
            {
                return this.colDescription;
            }
        }

        /// <summary>
        /// Public Property DataColumn Itemtype
        /// </summary>	
        public DataColumn Itemtype
        {
            get
            {
                return this.colItemtype;
            }
        }

        /// <summary>
        /// Public Property DataColumn ProductCode
        /// </summary>	
        public DataColumn ProductCode
        {
            get
            {
                return this.colProductCode;
            }
        }

        /// <summary>
        /// Public Property DataColumn SaleTypeCode
        /// </summary>	
        public DataColumn SaleTypeCode
        {
            get
            {
                return this.colSaleTypeCode;
            }
        }

        /// <summary>
        /// Public Property DataColumn SeasonCode
        /// </summary>	
        public DataColumn SeasonCode
        {
            get
            {
                return this.colSeasonCode;
            }
        }

        /// <summary>
        /// Public Property DataColumn Unit
        /// </summary>	
        public DataColumn Unit
        {
            get
            {
                return this.colUnit;
            }
        }

        /// <summary>
        /// Public Property DataColumn Freight
        /// </summary>	
        public DataColumn Freight
        {
            get
            {
                return this.colFreight;
            }
        }

        /// <summary>
        /// Public Property DataColumn SellingPrice
        /// </summary>	
        public DataColumn SellingPrice
        {
            get
            {
                return this.colSellingPrice;
            }
        }

        /// <summary>
        /// Public Property DataColumn AvgPrice
        /// </summary>	
        public DataColumn AvgPrice
        {
            get
            {
                return this.colAvgPrice;
            }
        }

        /// <summary>
        /// Public Property DataColumn LastCostPrice
        /// </summary>	
        public DataColumn LastCostPrice
        {
            get
            {
                return this.colLastCostPrice;
            }
        }

        /// <summary>
        /// Public Property DataColumn isTaxable
        /// </summary>	
        public DataColumn isTaxable
        {
            get
            {
                return this.colisTaxable;
            }
        }

        /// <summary>
        /// Public Property DataColumn TaxID
        /// </summary>	
        public DataColumn TaxID
        {
            get
            {
                return this.colTaxID;
            }
        }

        /// <summary>
        /// Public Property DataColumn isDiscountable
        /// </summary>	
        /// 

        public DataColumn isOnSale
        {
            get
            {
                return this.colisOnSale;
            }
        }

        public DataColumn isDiscountable
        {
            get
            {
                return this.colisDiscountable;
            }
        }

        /// <summary>
        /// Public Property DataColumn Discount
        /// </summary>	
        public DataColumn Discount
        {
            get
            {
                return this.colDiscount;
            }
        }

        /// <summary>
        /// Public Property DataColumn SaleStartDate
        /// </summary>	
        public DataColumn SaleStartDate
        {
            get
            {
                return this.colSaleStartDate;
            }
        }

        /// <summary>
        /// Public Property DataColumn SaleEndDate
        /// </summary>	
        public DataColumn SaleEndDate
        {
            get
            {
                return this.colSaleEndDate;
            }
        }

        /// <summary>
        /// Public Property DataColumn OnSalePrice
        /// </summary>	
        public DataColumn OnSalePrice
        {
            get
            {
                return this.colOnSalePrice;
            }
        }

        /// <summary>
        /// Public Property DataColumn QtyInStock
        /// </summary>	
        public DataColumn QtyInStock
        {
            get
            {
                return this.colQtyInStock;
            }
        }

        /// <summary>
        /// Public Property DataColumn Location
        /// </summary>	
        public DataColumn Location
        {
            get
            {
                return this.colLocation;
            }
        }

        /// <summary>
        /// Public Property DataColumn MinOrdQty
        /// </summary>	
        public DataColumn MinOrdQty
        {
            get
            {
                return this.colMinOrdQty;
            }
        }

        /// <summary>
        /// Public Property DataColumn ReOrderLevel
        /// </summary>	
        public DataColumn ReOrderLevel
        {
            get
            {
                return this.colReOrderLevel;
            }
        }

        /// <summary>
        /// Public Property DataColumn QtyOnOrder
        /// </summary>	
        public DataColumn QtyOnOrder
        {
            get
            {
                return this.colQtyOnOrder;
            }
        }

        /// <summary>
        /// Public Property DataColumn ExptDeliveryDate
        /// </summary>	
        public DataColumn ExptDeliveryDate
        {
            get
            {
                return this.colExptDeliveryDate;
            }
        }
        
        //Following Added by Krishna on 5 October 2011
        /// <summary>
        /// Public Property DataColumn ExpirationDate
        /// </summary>	
        //public DataColumn ExpDate
        //{
        //    get
        //    {
        //        return this.colExpDate;
        //    }
        //}

        ///// <summary>
        ///// Public Property DataColumn LotNumber
        ///// </summary>	
        //public DataColumn LotNumber
        //{
        //    get
        //    {
        //        return this.colLotNumber;
        //    }
        //}
        //Till here added by Krishhna on 5 October 2011

        #region Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
        /// <summary>
        /// Public Property DataColumn ExpirationDate
        /// </summary>	
        public DataColumn ExpDate
        {
            get
            {
                return this.colExpDate;
            }
        }
        #endregion

        /// <summary>
        /// Public Property DataColumn LastVendor
        /// </summary>	
        public DataColumn LastVendor
        {
            get
            {
                return this.colLastVendor;
            }
        }
        //Added by Prashant(SRT) Date:1-06-2009
        /// <summary>
        /// Public Property DataColumn PreferredVendor
        /// </summary>	
        public DataColumn PreferredVendor
        {
            get
            {
                return this.colPreferredVendor;
            }
        }
        //End of Added by Prashant(SRT) Date:1-06-2009
        /// <summary>
        /// Public Property DataColumn LastRecievDate
        /// </summary>	
        public DataColumn LastRecievDate
        {
            get
            {
                return this.colLastRecievDate;
            }
        }

        /// <summary>
        /// Public Property DataColumn LastSellingDate
        /// </summary>	
        public DataColumn LastSellingDate
        {
            get
            {
                return this.colLastSellingDate;
            }
        }

        /// <summary>
        /// Public Property DataColumn Remarks
        /// </summary>	
        public DataColumn Remarks
        {
            get
            {
                return this.colRemarks;
            }
        }

        public DataColumn ExclFromAutoPO
        {
            get
            {
                return this.colExclFromAutoPO;
            }
        }

        public DataColumn ExclFromRecpt
        {
            get
            {
                return this.colExclFromRecpt;
            }
        }

        public DataColumn isOTCItem
        {
            get
            {
                return this.colIsOTCItem;
            }
        }
        public DataColumn UpdatePrice
        {
            get
            {
                return this.colUpdatePrice;
            }
        }

        public DataColumn PckSize
        {
            get
            {
                return this.colPckSize;
            }
        }

        public DataColumn PckQty
        {
            get
            {
                return this.colPckQty;
            }
        }

        public DataColumn PckUnit
        {
            get
            {
                return this.colPckUnit;
            }
        }

        public DataColumn IsEBTItem
        {
            get
            {
                return this.colIsEBTItem;
            }
        }
        //Added By Shitaljit(QuicSolv) on 18 August 2011
        /// <summary>
        /// public property DataColumn TaxPolicy
        /// </summary>
        public DataColumn TaxPolicy
        {
            get
            {
                return this.colTaxPolicy;
            }
        }
        //Till here added by shitaljit 

        public DataColumn ManufacturerName
        {
            get
            {
                return this.colManufacturerName;
            }
        }
        //Added BY Ravindra for Sale Limit 22 March 2013
        public DataColumn SaleLimitQty
        {
            get
            {
                return this.colSaleLimitQty;
            }
        }
        //Till here Added by Ravindra 22 March 2013
    
        //Added By Shitaljit(QuicSolv) on 3 April 2013
        /// <summary>
        /// public property DataColumn DiscountPolicy
        /// </summary>
        public DataColumn DiscountPolicy
        {
            get 
            {
                return this.colDiscountPolicy;
            }
        }

        /// <summary>
        /// Added By Shitaljit on 2/6/2014
        /// Property DefaultCLPoints
        /// </summary>
        public DataColumn IsDefaultCLPoint
        {
            get
            {
                return this.colIsDefaultCLPoint;
            }
        }

        /// <summary>
        /// Added By Shitaljit on 2/6/2014
        /// Property PointsPerDollar
        /// </summary>
        public DataColumn PointsPerDollar
        {
            get
            {
                return this.colPointsPerDollar;
            }
        }

        //Sprint-18 - 2041 28-Oct-2014 JY  Added for CLPointPolicy
        public DataColumn CLPointPolicy
        {
            get
            {
                return this.colCLPointPolicy;
            }
        }
        #endregion 

        #region Sprint-21 - 2173 06-Jul-2015 JY
        public DataColumn IsActive
        {
            get
            {
                return this.colIsActive;
            }
        }
        #endregion

        //PRIMEPOS-2592 01-Nov-2018 JY Added 
        public DataColumn IsNonRefundable
        {
            get
            {
                return this.colIsNonRefundable;
            }
        }

        #region Added for Solutran - PRIMEPOS-2663 - NileshJ

        public DataColumn S3PurAmount
        {
            get
            {
                return this.colS3PurAmount;
            }
        }

        public DataColumn S3TaxAmount
        {
            get
            {
                return this.colS3TaxAmount;
            }
        }

        public DataColumn S3DiscountAmount
        {
            get
            {
                return this.colS3DiscountAmount;
            }
        }

        public DataColumn S3TrasID
        {
            get
            {
                return this.colS3TransID;
            }
        }
        #endregion
        //Properties

        #region Add and Get Methods

        public virtual void AddRow(ItemRow row)
        {
            AddRow(row, false);
        }

        public virtual void AddRow(ItemRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ItemID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public ItemRow AddRow(System.String ItemID, System.Int32 DepartmentID, System.String Description, System.String Itemtype, System.String ProductCode,
                              System.String SaleTypeCode, System.String SeasonCode, System.String Unit, System.Decimal Freight,
                              System.Decimal SellingPrice, System.Decimal AvgPrice, System.Decimal LastCostPrice, System.Boolean isTaxable, System.Int32 TaxID, 
                              System.Boolean isDiscountable, System.Decimal Discount, System.Object SaleStartDate, System.Object SaleEndDate, System.Decimal OnSalePrice,
                              System.Int32 QtyInStock, System.String Location, System.Int32 MinOrdQty, System.Int32 ReOrderLevel, System.Int32 QtyOnOrder, 
                              System.Object ExptDeliveryDate, System.String LastVendor, System.String PreferredVendor, System.DateTime LastRecievDate, 
                              System.DateTime LastSellingDate,System.String Remarks, bool isOnSale, bool ExclFromAutoPO, bool ExclFromRecpt, bool isOTCItem, 
                              bool UpdatePrice, int SubDepartmentID, System.Boolean isEBTItem,System.Int32 SaleLimitQty, System.String DiscountPolicy,
                              bool IsDefaultCLPoint, bool IsActive, System.Boolean IsNonRefundable, int PointsPerDollar
                                , System.Int64 S3TransID, System.Decimal S3PurAmount, System.Decimal S3TaxAmount, System.Decimal S3DiscountAmount
            )   //System.DateTime ExpDate, System.String LotNumber)   //Sprint-21 - 2173 06-Jul-2015 JY Added isActive //PRIMEPOS-2592 01-Nov-2018 JY Added IsNonRefundable // Added for Solutran : System.Int32 S3TransID, System.Decimal S3PurAmount, System.Decimal S3TaxAmount, System.Decimal S3DiscountAmount - PRIMEPOS-2663 - NileshJ - 05-July-2019 //PRIMEPOS-3265-Change Datatype to Int64
        {
            ItemRow row = (ItemRow)this.NewRow();
            //row.ItemArray = new object[] {  ItemID,DepartmentID,Description,Itemtype,ProductCode,SaleTypeCode,SeasonCode,Unit,Freight,
            //                                SellingPrice,AvgPrice,LastCostPrice,isTaxable,TaxID,isDiscountable,Discount,SaleStartDate,SaleEndDate,OnSalePrice,
            //                                QtyInStock,Location,MinOrdQty,ReOrderLevel,QtyOnOrder,ExptDeliveryDate,LastVendor,PreferredVendor,LastRecievDate,
            //                                LastSellingDate,Remarks,isOnSale,ExclFromAutoPO,ExclFromRecpt,isOTCItem,UpdatePrice,SubDepartmentID, isEBTItem,
            //                                SaleLimitQty,DiscountPolicy,IsDefaultCLPoint,IsActive,PointsPerDollar};    //Sprint-21 - 2173 06-Jul-2015 JY Added isActive

            #region Sprint-21 16-Nov-2015 JY Added
            row.ItemID = ItemID;
            row.DepartmentID = DepartmentID;
            row.Description = Description;
            row.Itemtype = Itemtype;
            row.ProductCode = ProductCode;
            row.SaleTypeCode = SaleTypeCode;
            row.SeasonCode = SeasonCode;
            row.Unit = Unit;
            row.Freight = Freight;
            row.SellingPrice = SellingPrice;
            row.AvgPrice = AvgPrice;
            row.LastCostPrice = LastCostPrice;
            row.isTaxable = isTaxable;
            row.TaxID = TaxID;
            row.isDiscountable = isDiscountable;
            row.Discount = Discount;
            row.SaleStartDate = SaleStartDate;
            row.SaleEndDate = SaleEndDate;
            row.OnSalePrice = OnSalePrice;
            row.QtyInStock = QtyInStock;
            row.Location = Location;
            row.MinOrdQty = MinOrdQty;
            row.ReOrderLevel = ReOrderLevel;
            row.QtyOnOrder = QtyOnOrder;
            row.ExptDeliveryDate = ExptDeliveryDate;
            row.LastVendor = LastVendor;
            row.PreferredVendor = PreferredVendor;
            row.LastRecievDate = LastRecievDate;
            row.LastSellingDate = LastSellingDate;
            row.Remarks = Remarks;
            row.isOnSale = isOnSale;
            row.ExclFromAutoPO = ExclFromAutoPO;
            row.ExclFromRecpt = ExclFromRecpt;
            row.isOTCItem = isOTCItem;
            row.UpdatePrice = UpdatePrice;
            row.SubDepartmentID = SubDepartmentID;
            row.IsEBTItem = isEBTItem;
            row.SaleLimitQty = SaleLimitQty;
            row.DiscountPolicy = DiscountPolicy;
            row.IsDefaultCLPoint = IsDefaultCLPoint;
            row.IsActive = IsActive;
            row.IsNonRefundable = IsNonRefundable;    //PRIMEPOS-2592 01-Nov-2018 JY Added 
            row.PointsPerDollar = PointsPerDollar;
            #region  Added for Solutran - PRIMEPOS-2663 - NileshJ - 05-July-2019
            row.S3TransID = S3TransID;
            row.S3PurAmount = S3PurAmount;
            row.S3TaxAmount = S3TaxAmount;
            row.S3DiscountAmount = S3DiscountAmount;
            #endregion
            #endregion

            this.Rows.Add(row);
            return row;
        }



        public ItemRow GetRowByID(System.String ItemID)
        {
            return (ItemRow)this.Rows.Find(new object[] { ItemID });
        }

        public virtual void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            ItemRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ItemRow)this.NewRow();

                if (dr[clsPOSDBConstants.Item_Fld_ItemID] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ItemID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_ItemID] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_ItemID].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_DepartmentID] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_DepartmentID] = 0;
                else
                    row[clsPOSDBConstants.Item_Fld_DepartmentID] = Convert.ToInt32(dr[clsPOSDBConstants.Item_Fld_DepartmentID].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_SubDepartmentID] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_SubDepartmentID] = 0;
                else
                    row[clsPOSDBConstants.Item_Fld_SubDepartmentID] = Convert.ToInt32(dr[clsPOSDBConstants.Item_Fld_SubDepartmentID].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Description] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Description] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_Description].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Itemtype] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Itemtype] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Itemtype] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_Itemtype].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_ProductCode] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ProductCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_ProductCode] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_ProductCode].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_SaleTypeCode] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_SaleTypeCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_SaleTypeCode] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_SaleTypeCode].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_SeasonCode] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_SeasonCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_SeasonCode] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_SeasonCode].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Unit] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Unit] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Unit] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_Unit].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Freight] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Freight] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Freight] = Resources.Configuration.convertNullToDecimal(dr[clsPOSDBConstants.Item_Fld_Freight].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_SellingPrice] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_SellingPrice] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_SellingPrice] = Resources.Configuration.convertNullToDecimal(dr[clsPOSDBConstants.Item_Fld_SellingPrice].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_AvgPrice] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_AvgPrice] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_AvgPrice] = Resources.Configuration.convertNullToDecimal(dr[clsPOSDBConstants.Item_Fld_AvgPrice].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_LastCostPrice] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_LastCostPrice] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_LastCostPrice] = Resources.Configuration.convertNullToDecimal(dr[clsPOSDBConstants.Item_Fld_LastCostPrice].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_isTaxable] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_isTaxable] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_isTaxable] = Convert.ToBoolean(dr[clsPOSDBConstants.Item_Fld_isTaxable].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_TaxID] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_TaxID] = 0;
                else
                    row[clsPOSDBConstants.Item_Fld_TaxID] = Convert.ToInt32(dr[clsPOSDBConstants.Item_Fld_TaxID].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_isDiscountable] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_isDiscountable] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_isDiscountable] = Convert.ToBoolean(dr[clsPOSDBConstants.Item_Fld_isDiscountable].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Discount] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Discount] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Discount] = Resources.Configuration.convertNullToDecimal(dr[clsPOSDBConstants.Item_Fld_Discount].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_SaleStartDate] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_SaleStartDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_SaleStartDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Item_Fld_SaleStartDate].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_SaleEndDate] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_SaleEndDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_SaleEndDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Item_Fld_SaleEndDate].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_OnSalePrice] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_OnSalePrice] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_OnSalePrice] = Resources.Configuration.convertNullToDecimal(dr[clsPOSDBConstants.Item_Fld_OnSalePrice].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_QtyInStock] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_QtyInStock] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_QtyInStock] = Convert.ToInt32(dr[clsPOSDBConstants.Item_Fld_QtyInStock].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Location] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Location] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Location] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_Location].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_MinOrdQty] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_MinOrdQty] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_MinOrdQty] = Convert.ToInt32(dr[clsPOSDBConstants.Item_Fld_MinOrdQty].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_ReOrderLevel] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ReOrderLevel] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_ReOrderLevel] = Convert.ToInt32(dr[clsPOSDBConstants.Item_Fld_ReOrderLevel].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_QtyOnOrder] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_QtyOnOrder] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_QtyOnOrder] = Convert.ToInt32(dr[clsPOSDBConstants.Item_Fld_QtyOnOrder].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_ExptDeliveryDate] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ExptDeliveryDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_ExptDeliveryDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Item_Fld_ExptDeliveryDate].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_LastVendor] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_LastVendor] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_LastVendor] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_LastVendor].ToString());

                //Added by Prashant(SRT) Date:1-06-09
                if (dr[clsPOSDBConstants.Item_Fld_PreferredVendor] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_PreferredVendor] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_PreferredVendor] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_PreferredVendor].ToString());
                //End of Added by Prashant(SRT) Date:1-06-09
                if (dr[clsPOSDBConstants.Item_Fld_LastRecievDate] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_LastRecievDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_LastRecievDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Item_Fld_LastRecievDate].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_LastSellingDate] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_LastSellingDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_LastSellingDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Item_Fld_LastSellingDate].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_Remarks] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_Remarks] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_Remarks] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_Remarks].ToString());


                if (dr[clsPOSDBConstants.Item_Fld_isOnSale] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_isOnSale] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_isOnSale] = Convert.ToBoolean(dr[clsPOSDBConstants.Item_Fld_isOnSale].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_ExclFromAutoPO] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ExclFromAutoPO] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_ExclFromAutoPO] = Convert.ToBoolean(dr[clsPOSDBConstants.Item_Fld_ExclFromAutoPO].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_ExclFromRecpt] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ExclFromRecpt] = false;
                else
                    row[clsPOSDBConstants.Item_Fld_ExclFromRecpt] = Convert.ToBoolean(dr[clsPOSDBConstants.Item_Fld_ExclFromRecpt].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_isOTCItem] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_isOTCItem] = false;
                else
                    row[clsPOSDBConstants.Item_Fld_isOTCItem] = Convert.ToBoolean(dr[clsPOSDBConstants.Item_Fld_isOTCItem].ToString());


                if (dr[clsPOSDBConstants.Item_Fld_UpdatePrice] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_UpdatePrice] = true;
                else
                    row[clsPOSDBConstants.Item_Fld_UpdatePrice] = Convert.ToBoolean(dr[clsPOSDBConstants.Item_Fld_UpdatePrice].ToString());

                if (dr[clsPOSDBConstants.ItemVendor_Fld_PckSize] == DBNull.Value)
                    row[clsPOSDBConstants.ItemVendor_Fld_PckSize] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ItemVendor_Fld_PckSize] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_PckSize].ToString());

                if (dr[clsPOSDBConstants.ItemVendor_Fld_PckQty] == DBNull.Value)
                    row[clsPOSDBConstants.ItemVendor_Fld_PckQty] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ItemVendor_Fld_PckQty] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_PckQty].ToString());

                if (dr[clsPOSDBConstants.ItemVendor_Fld_PckUnit] == DBNull.Value)
                    row[clsPOSDBConstants.ItemVendor_Fld_PckUnit] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ItemVendor_Fld_PckUnit] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_PckUnit].ToString());

                if (dt.Columns.Contains(clsPOSDBConstants.IIAS_Items_Fld_IsEBTItem) == true)
                {
                    if (dr[clsPOSDBConstants.IIAS_Items_Fld_IsEBTItem] == DBNull.Value)
                        row[clsPOSDBConstants.IIAS_Items_Fld_IsEBTItem] = false;
                    else
                        row[clsPOSDBConstants.IIAS_Items_Fld_IsEBTItem] = POS_Core.Resources.Configuration.convertNullToBoolean(dr[clsPOSDBConstants.IIAS_Items_Fld_IsEBTItem].ToString());
                }
                //Added By Shitaljit(QuicSolv) on 19 August
                if (dr[clsPOSDBConstants.Item_Fld_TaxPolicy] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_TaxPolicy] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_TaxPolicy] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_TaxPolicy].ToString());
                //END

                //Following Code Added by Krishna on 5 October 2011
                //if (dr[clsPOSDBConstants.Item_Fld_ExpDate] == DBNull.Value)
                //    row[clsPOSDBConstants.Item_Fld_ExpDate] = DBNull.Value;
                //else
                //    row[clsPOSDBConstants.Item_Fld_ExpDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Item_Fld_ExpDate].ToString());

                //if (dr[clsPOSDBConstants.Item_Fld_LotNumber] == DBNull.Value)
                //    row[clsPOSDBConstants.Item_Fld_LotNumber] = DBNull.Value;
                //else
                //    row[clsPOSDBConstants.Item_Fld_LotNumber] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_LotNumber].ToString());
                //Till here added by Krishna on 5 October 2011

                #region Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
                if (dr[clsPOSDBConstants.Item_Fld_ExpDate] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ExpDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_ExpDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Item_Fld_ExpDate].ToString());
                #endregion

                if (dr[clsPOSDBConstants.Item_Fld_ManufacturerName] != DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_ManufacturerName] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_ManufacturerName].ToString());
                //Added by Ravindra fro Sale Limit 22 March 2013
                if (dr[clsPOSDBConstants.Item_Fld_SaleLimitQty] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_SaleLimitQty] = 0;
                else
                    row[clsPOSDBConstants.Item_Fld_SaleLimitQty] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_SaleLimitQty].ToString());
                //Till here Added BY Ravindra
                //Added By Shitaljit(QuicSolv) on 3 April
                if (dr[clsPOSDBConstants.Item_Fld_DiscountPolicy] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_DiscountPolicy] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_DiscountPolicy] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_DiscountPolicy].ToString());
                //END

                //Added By Shitaljit(QuicSolv) on 6Feb2014 for
                //PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
                if (dr[clsPOSDBConstants.Item_Fld_IsDefaultCLPoint] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_IsDefaultCLPoint] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_IsDefaultCLPoint] = POS_Core.Resources.Configuration.convertNullToBoolean(dr[clsPOSDBConstants.Item_Fld_IsDefaultCLPoint].ToString());
                
                if (dr[clsPOSDBConstants.Item_Fld_PointsPerDollar] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_PointsPerDollar] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_PointsPerDollar] = POS_Core.Resources.Configuration.convertNullToInt(dr[clsPOSDBConstants.Item_Fld_PointsPerDollar].ToString());
                //END

                #region Sprint-18 - 2041 28-Oct-2014 JY  Added for CLPointPolicy
                if (dr[clsPOSDBConstants.Item_Fld_CLPointPolicy] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_CLPointPolicy] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Item_Fld_CLPointPolicy] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_CLPointPolicy].ToString());
                #endregion

                #region Sprint-21 - 2173 06-Jul-2015 JY Added for IsActive
                if (dr[clsPOSDBConstants.Item_Fld_IsActive] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_IsActive] = false;
                else
                    row[clsPOSDBConstants.Item_Fld_IsActive] = POS_Core.Resources.Configuration.convertNullToBoolean(dr[clsPOSDBConstants.Item_Fld_IsActive].ToString());
                #endregion

                #region PRIMEPOS-2592 01-Nov-2018 JY Added 
                if (dr[clsPOSDBConstants.Item_Fld_IsNonRefundable] == DBNull.Value)
                    row[clsPOSDBConstants.Item_Fld_IsNonRefundable] = false;
                else
                    row[clsPOSDBConstants.Item_Fld_IsNonRefundable] = POS_Core.Resources.Configuration.convertNullToBoolean(dr[clsPOSDBConstants.Item_Fld_IsNonRefundable].ToString());
                #endregion

                this.AddRow(row);
            }
        }

        #endregion

        public override DataTable Clone()
        {
            ItemTable cln = (ItemTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new ItemTable();
        }

        internal void InitVars()
        {
            this.colItemID = this.Columns[clsPOSDBConstants.Item_Fld_ItemID];
            this.colDepartmentID = this.Columns[clsPOSDBConstants.Item_Fld_DepartmentID];
            this.colSubDepartmentID = this.Columns[clsPOSDBConstants.Item_Fld_SubDepartmentID];
            this.colDescription = this.Columns[clsPOSDBConstants.Item_Fld_Description];
            this.colItemtype = this.Columns[clsPOSDBConstants.Item_Fld_Itemtype];
            this.colProductCode = this.Columns[clsPOSDBConstants.Item_Fld_ProductCode];
            this.colSaleTypeCode = this.Columns[clsPOSDBConstants.Item_Fld_SaleTypeCode];
            this.colSeasonCode = this.Columns[clsPOSDBConstants.Item_Fld_SeasonCode];
            this.colUnit = this.Columns[clsPOSDBConstants.Item_Fld_Unit];
            this.colFreight = this.Columns[clsPOSDBConstants.Item_Fld_Freight];
            this.colSellingPrice = this.Columns[clsPOSDBConstants.Item_Fld_SellingPrice];
            this.colAvgPrice = this.Columns[clsPOSDBConstants.Item_Fld_AvgPrice];
            this.colLastCostPrice = this.Columns[clsPOSDBConstants.Item_Fld_LastCostPrice];
            this.colisTaxable = this.Columns[clsPOSDBConstants.Item_Fld_isTaxable];
            this.colTaxID = this.Columns[clsPOSDBConstants.Item_Fld_TaxID];
            this.colisDiscountable = this.Columns[clsPOSDBConstants.Item_Fld_isDiscountable];
            this.colDiscount = this.Columns[clsPOSDBConstants.Item_Fld_Discount];
            this.colSaleStartDate = this.Columns[clsPOSDBConstants.Item_Fld_SaleStartDate];
            this.colSaleEndDate = this.Columns[clsPOSDBConstants.Item_Fld_SaleEndDate];
            this.colOnSalePrice = this.Columns[clsPOSDBConstants.Item_Fld_OnSalePrice];
            this.colQtyInStock = this.Columns[clsPOSDBConstants.Item_Fld_QtyInStock];
            this.colLocation = this.Columns[clsPOSDBConstants.Item_Fld_Location];
            this.colMinOrdQty = this.Columns[clsPOSDBConstants.Item_Fld_MinOrdQty];
            this.colReOrderLevel = this.Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel];
            this.colQtyOnOrder = this.Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder];
            this.colExptDeliveryDate = this.Columns[clsPOSDBConstants.Item_Fld_ExptDeliveryDate];
            //Added by Prashant(SRT) Date:1-06-2009
            this.colPreferredVendor = this.Columns[clsPOSDBConstants.Item_Fld_PreferredVendor];
            //End of Added by Prashant(SRT) Date:1-06-2009
            this.colLastVendor = this.Columns[clsPOSDBConstants.Item_Fld_LastVendor];
            this.colLastRecievDate = this.Columns[clsPOSDBConstants.Item_Fld_LastRecievDate];
            this.colLastSellingDate = this.Columns[clsPOSDBConstants.Item_Fld_LastSellingDate];
            this.colRemarks = this.Columns[clsPOSDBConstants.Item_Fld_Remarks];
            this.colisOnSale = this.Columns[clsPOSDBConstants.Item_Fld_isOnSale];
            this.colExclFromAutoPO = this.Columns[clsPOSDBConstants.Item_Fld_ExclFromAutoPO];
            this.colExclFromRecpt = this.Columns[clsPOSDBConstants.Item_Fld_ExclFromRecpt];
            this.colIsOTCItem = this.Columns[clsPOSDBConstants.Item_Fld_isOTCItem];
            this.colUpdatePrice = this.Columns[clsPOSDBConstants.Item_Fld_UpdatePrice];

            this.colPckSize = this.Columns[clsPOSDBConstants.ItemVendor_Fld_PckSize];
            this.colPckQty = this.Columns[clsPOSDBConstants.ItemVendor_Fld_PckQty];
            this.colPckUnit = this.Columns[clsPOSDBConstants.ItemVendor_Fld_PckUnit];
            this.colIsEBTItem = this.Columns[clsPOSDBConstants.IIAS_Items_Fld_IsEBTItem];
            this.colTaxPolicy = this.Columns[clsPOSDBConstants.Item_Fld_TaxPolicy];//Added By Shitaljit(QuicSolv) on 18 August
            this.colExpDate = this.Columns[clsPOSDBConstants.Item_Fld_ExpDate];//Added by Krishna on 5 October 2011   //Sprint-21 - 2206 03-Jul-2015 JY uncommented code for item exp. date
            //this.colLotNumber = this.Columns[clsPOSDBConstants.Item_Fld_LotNumber];//Added by Krishna on 5 October 2011
            this.colManufacturerName= this.Columns[clsPOSDBConstants.Item_Fld_ManufacturerName];
            //Aded BY Ravindra For Sale Price 22 March 2013
            this.colSaleLimitQty = this.Columns[clsPOSDBConstants.Item_Fld_SaleLimitQty];
            this.colDiscountPolicy = this.Columns[clsPOSDBConstants.Item_Fld_DiscountPolicy]; //Added By Shitaljit(QuicSolv) on 3 April 2013
            // Added By Shitaljit on 2/6/2014
            //PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
            this.colPointsPerDollar = this.Columns[clsPOSDBConstants.Item_Fld_PointsPerDollar];
            this.colIsDefaultCLPoint = this.Columns[clsPOSDBConstants.Item_Fld_IsDefaultCLPoint]; 
            //END
            this.colCLPointPolicy = this.Columns[clsPOSDBConstants.Item_Fld_CLPointPolicy]; //Sprint-18 - 2041 28-Oct-2014 JY  Added
            this.colIsActive = this.Columns[clsPOSDBConstants.Item_Fld_IsActive];   //Sprint-21 - 2173 06-Jul-2015 JY Added 
            this.colIsNonRefundable = this.Columns[clsPOSDBConstants.Item_Fld_IsNonRefundable]; //PRIMEPOS-2592 01-Nov-2018 JY Added 
            #region Solutran PRIMEPOS-2663
            this.colS3TransID = this.Columns[clsPOSDBConstants.Item_Fld_S3TransID];
            this.colS3PurAmount = this.Columns[clsPOSDBConstants.Item_Fld_S3PurAmount];
            this.colS3DiscountAmount = this.Columns[clsPOSDBConstants.Item_Fld_S3DiscountAmount];
            this.colS3TaxAmount = this.Columns[clsPOSDBConstants.Item_Fld_S3TaxAmount];
            #endregion
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colItemID = new DataColumn(clsPOSDBConstants.Item_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemID);
            this.colItemID.AllowDBNull = false;

            this.colDepartmentID = new DataColumn(clsPOSDBConstants.Item_Fld_DepartmentID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDepartmentID);
            this.colDepartmentID.AllowDBNull = true;

            this.colDescription = new DataColumn(clsPOSDBConstants.Item_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDescription);
            this.colDescription.AllowDBNull = true;

            this.colItemtype = new DataColumn(clsPOSDBConstants.Item_Fld_Itemtype, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemtype);
            this.colItemtype.AllowDBNull = true;

            this.colProductCode = new DataColumn(clsPOSDBConstants.Item_Fld_ProductCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProductCode);
            this.colProductCode.AllowDBNull = true;

            this.colSaleTypeCode = new DataColumn(clsPOSDBConstants.Item_Fld_SaleTypeCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSaleTypeCode);
            this.colSaleTypeCode.AllowDBNull = true;

            this.colSeasonCode = new DataColumn(clsPOSDBConstants.Item_Fld_SeasonCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSeasonCode);
            this.colSeasonCode.AllowDBNull = true;

            this.colUnit = new DataColumn(clsPOSDBConstants.Item_Fld_Unit, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUnit);
            this.colUnit.AllowDBNull = true;

            this.colFreight = new DataColumn(clsPOSDBConstants.Item_Fld_Freight, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colFreight);
            this.colFreight.AllowDBNull = false;

            this.colSellingPrice = new DataColumn(clsPOSDBConstants.Item_Fld_SellingPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSellingPrice);
            //Commented By Amit Date 1 july 2011
            //this.colSellingPrice.AllowDBNull = false;
            this.colSellingPrice.AllowDBNull = true;

            this.colAvgPrice = new DataColumn(clsPOSDBConstants.Item_Fld_AvgPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAvgPrice);
            this.colAvgPrice.AllowDBNull = true;

            this.colLastCostPrice = new DataColumn(clsPOSDBConstants.Item_Fld_LastCostPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastCostPrice);
            this.colLastCostPrice.AllowDBNull = true;

            this.colisTaxable = new DataColumn(clsPOSDBConstants.Item_Fld_isTaxable, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisTaxable);
            this.colisTaxable.AllowDBNull = true;   //Sprint-24 - 12-Jan-2017 JY changed to TRUE as it throwing exception if this field is NULL

            this.colTaxID = new DataColumn(clsPOSDBConstants.Item_Fld_TaxID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaxID);
            this.colTaxID.AllowDBNull = true;

            this.colisDiscountable = new DataColumn(clsPOSDBConstants.Item_Fld_isDiscountable, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisDiscountable);
            this.colisDiscountable.AllowDBNull = false;

            this.colDiscount = new DataColumn(clsPOSDBConstants.Item_Fld_Discount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDiscount);
            this.colDiscount.AllowDBNull = false;

            this.colSaleStartDate = new DataColumn(clsPOSDBConstants.Item_Fld_SaleStartDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSaleStartDate);
            this.colSaleStartDate.AllowDBNull = true;

            this.colSaleEndDate = new DataColumn(clsPOSDBConstants.Item_Fld_SaleEndDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSaleEndDate);
            this.colSaleEndDate.AllowDBNull = true;

            this.colOnSalePrice = new DataColumn(clsPOSDBConstants.Item_Fld_OnSalePrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOnSalePrice);
            this.colOnSalePrice.AllowDBNull = true;

            this.colQtyInStock = new DataColumn(clsPOSDBConstants.Item_Fld_QtyInStock, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQtyInStock);
            this.colQtyInStock.AllowDBNull = false;

            this.colLocation = new DataColumn(clsPOSDBConstants.Item_Fld_Location, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLocation);
            this.colLocation.AllowDBNull = true;

            this.colMinOrdQty = new DataColumn(clsPOSDBConstants.Item_Fld_MinOrdQty, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMinOrdQty);
            this.colMinOrdQty.AllowDBNull = true;

            this.colReOrderLevel = new DataColumn(clsPOSDBConstants.Item_Fld_ReOrderLevel, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colReOrderLevel);
            this.colReOrderLevel.AllowDBNull = true;

            this.colQtyOnOrder = new DataColumn(clsPOSDBConstants.Item_Fld_QtyOnOrder, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQtyOnOrder);
            this.colQtyOnOrder.AllowDBNull = true;

            this.colExptDeliveryDate = new DataColumn(clsPOSDBConstants.Item_Fld_ExptDeliveryDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExptDeliveryDate);
            this.colExptDeliveryDate.AllowDBNull = true;

            this.colLastVendor = new DataColumn(clsPOSDBConstants.Item_Fld_LastVendor, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastVendor);
            this.colLastVendor.AllowDBNull = true;
            //Added by Prashant(SRT) Date:1-06-2009
            this.colPreferredVendor = new DataColumn(clsPOSDBConstants.Item_Fld_PreferredVendor, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPreferredVendor);
            this.colLastVendor.AllowDBNull = true;
            //End of Added by Prashant(SRT) Date:1-06-2009
            this.colLastRecievDate = new DataColumn(clsPOSDBConstants.Item_Fld_LastRecievDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastRecievDate);
            this.colLastRecievDate.AllowDBNull = true;

            this.colLastSellingDate = new DataColumn(clsPOSDBConstants.Item_Fld_LastSellingDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastSellingDate);
            this.colLastSellingDate.AllowDBNull = true;

            this.colRemarks = new DataColumn(clsPOSDBConstants.Item_Fld_Remarks, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRemarks);
            this.colRemarks.AllowDBNull = true;


            this.colisOnSale = new DataColumn(clsPOSDBConstants.Item_Fld_isOnSale, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisOnSale);
            this.colisOnSale.AllowDBNull = false;

            this.colExclFromAutoPO = new DataColumn(clsPOSDBConstants.Item_Fld_ExclFromAutoPO, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExclFromAutoPO);
            this.colExclFromAutoPO.AllowDBNull = false;

            this.colExclFromRecpt = new DataColumn(clsPOSDBConstants.Item_Fld_ExclFromRecpt, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExclFromRecpt);
            this.colExclFromRecpt.AllowDBNull = false;

            this.colIsOTCItem = new DataColumn(clsPOSDBConstants.Item_Fld_isOTCItem, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsOTCItem);
            this.colIsOTCItem.AllowDBNull = false;

            this.colUpdatePrice = new DataColumn(clsPOSDBConstants.Item_Fld_UpdatePrice, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatePrice);
            this.colUpdatePrice.AllowDBNull = false;

            this.colSubDepartmentID = new DataColumn(clsPOSDBConstants.Item_Fld_SubDepartmentID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSubDepartmentID);
            this.colSubDepartmentID.AllowDBNull = true;

            this.colPckSize = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_PckSize, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPckSize);
            this.colPckSize.AllowDBNull = true;

            this.colPckQty = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_PckQty, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPckQty);
            this.colPckQty.AllowDBNull = true;

            this.colPckUnit = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_PckUnit, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPckUnit);
            this.colPckUnit.AllowDBNull = true;

            //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 06-04-2011
            this.ItemAddedDate = new DataColumn(clsPOSDBConstants.Item_Fld_ItemAddedDate, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.ItemAddedDate);
            this.ItemAddedDate.AllowDBNull = true;
            //End OfAdded By Shitaljit(QuicSolv)

            this.colIsEBTItem = new DataColumn(clsPOSDBConstants.IIAS_Items_Fld_IsEBTItem, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsEBTItem);
            this.colIsEBTItem.AllowDBNull = true;
            //Added By Shitaljit(QuicSolv) 18 August 2011
            this.colTaxPolicy = new DataColumn(clsPOSDBConstants.Item_Fld_TaxPolicy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaxPolicy);
            this.colTaxPolicy.AllowDBNull = true;
            //End OfAdded By Shitaljit(QuicSolv) 

            //Added by Krishna on 5 October 2011
            //this.colExpDate = new DataColumn(clsPOSDBConstants.Item_Fld_ExpDate, typeof(System.Object), null, System.Data.MappingType.Element);
            //this.Columns.Add(this.colExpDate);
            //this.colExpDate.AllowDBNull = true;

            //this.colLotNumber = new DataColumn(clsPOSDBConstants.Item_Fld_LotNumber, typeof(System.String), null, System.Data.MappingType.Element);
            //this.Columns.Add(this.colLotNumber);
            //this.colLotNumber.AllowDBNull = true;
            //Till here added by Krishna on 5 October 2011

            #region Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
            this.colExpDate = new DataColumn(clsPOSDBConstants.Item_Fld_ExpDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExpDate);
            this.colExpDate.AllowDBNull = true;
            #endregion

            this.colManufacturerName= new DataColumn(clsPOSDBConstants.Item_Fld_ManufacturerName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colManufacturerName);

            this.colManufacturerName.AllowDBNull = true;

            //Added by Ravindra for Sale Limit 22 March 2013
            this.colSaleLimitQty = new DataColumn(clsPOSDBConstants.Item_Fld_SaleLimitQty, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSaleLimitQty);
            this.colSaleLimitQty.AllowDBNull = true;

            //Added By Shitaljit(QuicSolv) 3 April 2013
            this.colDiscountPolicy = new DataColumn(clsPOSDBConstants.Item_Fld_DiscountPolicy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDiscountPolicy);
            this.colDiscountPolicy.AllowDBNull = true;
            //End OfAdded By Shitaljit(QuicSolv) 

            // Added By Shitaljit on 2/6/2014
            //PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
            this.colIsDefaultCLPoint = new DataColumn(clsPOSDBConstants.Item_Fld_IsDefaultCLPoint, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsDefaultCLPoint);
            this.colIsDefaultCLPoint.AllowDBNull = true;

            this.colPointsPerDollar = new DataColumn(clsPOSDBConstants.Item_Fld_PointsPerDollar, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPointsPerDollar);
            this.colPointsPerDollar.AllowDBNull = true;
            //END

            #region Sprint-18 - 2041 28-Oct-2014 JY  Added
            this.colCLPointPolicy = new DataColumn(clsPOSDBConstants.Item_Fld_CLPointPolicy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCLPointPolicy);
            this.colCLPointPolicy.AllowDBNull = true;
            #endregion

            #region Sprint-21 - 2173 06-Jul-2015 JY Added
            this.colIsActive = new DataColumn(clsPOSDBConstants.Item_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;
            #endregion

            #region Sprint-21 - 2173 06-Jul-2015 JY Added
            this.colIsNonRefundable = new DataColumn(clsPOSDBConstants.Item_Fld_IsNonRefundable, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsNonRefundable);
            this.colIsNonRefundable.AllowDBNull = true;
            #endregion

            #region Solutran - PRIMEPOS-2663
            this.colS3TransID = new DataColumn(clsPOSDBConstants.Item_Fld_S3TransID, typeof(System.Int64), null, System.Data.MappingType.Element); //PRIMEPOS-3265
            this.Columns.Add(this.colS3TransID);
            this.colS3TransID.AllowDBNull = true;

            this.colS3PurAmount = new DataColumn(clsPOSDBConstants.Item_Fld_S3PurAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3PurAmount);
            this.colS3PurAmount.AllowDBNull = true;

            this.colS3DiscountAmount = new DataColumn(clsPOSDBConstants.Item_Fld_S3DiscountAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3DiscountAmount);
            this.colS3DiscountAmount.AllowDBNull = true;

            this.colS3TaxAmount = new DataColumn(clsPOSDBConstants.Item_Fld_S3TaxAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3TaxAmount);
            this.colS3TaxAmount.AllowDBNull = true;
            #endregion

            this.PrimaryKey = new DataColumn[] { this.ItemID };
        }

        public virtual ItemRow NewItemRow()
        {
            return (ItemRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ItemRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ItemRow);
        }

        #region Event Handlers

        public delegate void ItemRowChangeEventHandler(object sender, ItemRowChangeEvent e);

        public event ItemRowChangeEventHandler ItemRowChanged;
        public event ItemRowChangeEventHandler ItemRowChanging;
        public event ItemRowChangeEventHandler ItemRowDeleted;
        public event ItemRowChangeEventHandler ItemRowDeleting;

        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            if (this.ItemRowChanged != null)
            {
                this.ItemRowChanged(this, new ItemRowChangeEvent((ItemRow)e.Row, e.Action));
            }
        }

        protected override void OnRowChanging(DataRowChangeEventArgs e)
        {
            base.OnRowChanging(e);
            if (this.ItemRowChanging != null)
            {
                this.ItemRowChanging(this, new ItemRowChangeEvent((ItemRow)e.Row, e.Action));
            }
        }

        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
            if (this.ItemRowDeleted != null)
            {
                this.ItemRowDeleted(this, new ItemRowChangeEvent((ItemRow)e.Row, e.Action));
            }
        }

        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
            if (this.ItemRowDeleting != null)
            {
                this.ItemRowDeleting(this, new ItemRowChangeEvent((ItemRow)e.Row, e.Action));
            }
        }

        #endregion
    }
}
