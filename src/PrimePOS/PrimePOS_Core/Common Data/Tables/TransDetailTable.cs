
namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using System.Collections.Generic;
    using Resources;

    //using POS.Resources;

    public class TransDetailTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colTransDetailID;
		private DataColumn colTransID;
		private DataColumn colQTY;
		private DataColumn colDiscount;
		private DataColumn colPrice;
        private DataColumn colItemCost;
		private DataColumn colTaxAmount;
		private DataColumn colExtendedPrice;
		private DataColumn colItemID;
		private DataColumn colTaxID;
		private DataColumn colTaxCode;
		private DataColumn colItemDescription;
        private DataColumn colUserID;
        private DataColumn colisPriceChanged;        
        private DataColumn colisIIAS;
        private DataColumn colIsCompanionItem;
        private DataColumn colIsRxItem;
        private DataColumn colIsEBTItem;
        private DataColumn colReturnTransDetailId;//Added by Krishna on 15 july 2011
        //Added By Amit Date 23 Nov 2011
        private DataColumn colIsMonitored;
        private DataColumn colCategory;
        //End
        private DataColumn colNonComboUnitPrice;
        private DataColumn colItemComboPricingID;

        private DataColumn colIsComboItem;
        private DataColumn colOrignalPrice;
        private DataColumn colLoyaltyPoints;
        private DataColumn colItemDescriptionInOL;  //Sprint-21 - 1272 17-Aug-2015 JY Added
        private DataColumn colIsPriceChangedByOverride;  //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
        private DataColumn colCouponID; //PRIMEPOS-2034 02-Mar-2018 JY Added
        private DataColumn colIsNonRefundable;  //PRIMEPOS-2592 02-Nov-2018 JY Added 

        #region Added for Solutran - PRIMEPOS-2663 - NileshJ
        private DataColumn colS3TransID;
        private DataColumn colS3PurAmount;
        private DataColumn colS3TaxAmount;
        private DataColumn colS3DiscountAmount;
        #endregion

        private DataColumn colInvoiceDiscount;  //PRIMEPOS-2768 18-Dec-2019 JY Added
        private DataColumn colIsOnSale; //PRIMEPOS-2907 13-Oct-2020 JY Added

        #region PRIMEPOS-2402 08-Jul-2021 JY Added
        private DataColumn colOldDiscountAmt;
        private DataColumn colDiscountOverrideUser;
        private DataColumn colQuantityOverrideUser;
        private DataColumn colTaxOverrideUser;
        private DataColumn colOldTaxCodesWithPercentage;
        private DataColumn colTaxOverrideAllOTCUser;
        private DataColumn colTaxOverrideAllRxUser;
        private DataColumn colMaxDiscountLimitUser;
        #endregion
        private DataColumn colOverrideRemark;   //PRIMEPOS-3015 26-Oct-2021 JY Added
        private DataColumn colItemGroupPrice;   //PRIMEPOS-3098 20-Jun-2022 JY Added
        private DataColumn colMonitorItemOverrideUser; //PRIMEPOS-3166N
        private DataColumn colItemDescriptionMasked; //PRIMEPOS-3130
        #region Constructors 
        internal TransDetailTable() : base(clsPOSDBConstants.TransDetail_tbl) { this.InitClass(); }
		internal TransDetailTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public TransDetailRow this[int index] 
		{
			get 
			{
				return ((TransDetailRow)(this.Rows[index]));
			}
		}

		public DataColumn TransDetailID 
		{
			get 
			{
				return this.colTransDetailID;
			}
		}

		public DataColumn TransID 
		{
			get 
			{
				return this.colTransID;
			}
		}


        //Following Code Added by Krishna on 15 July 2011
        public DataColumn ReturnTransDetailId
        {
            get
            {
                return this.colReturnTransDetailId;
            }
        }
        //Till here added by krishna on 15 july 2011

		public DataColumn QTY
		{
			get 
			{
				return this.colQTY;
			}
		}

		public DataColumn Discount
		{
			get 
			{
				return this.colDiscount;
			}
		}

		public DataColumn Price
		{
			get 
			{
				return this.colPrice;
			}
		}

        public DataColumn OrignalPrice
        {
            get
            {
                return this.colOrignalPrice;
            }
        }

        public DataColumn ItemCost
        {
            get
            {
                return this.colItemCost;
            }
        }

		public DataColumn ExtendedPrice
		{
			get 
			{
				return this.colExtendedPrice;
			}
		}

		public DataColumn TaxAmount
		{
			get 
			{
				return this.colTaxAmount;
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

        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }

        public DataColumn IsPriceChanged
        {
            get
            {
                return this.colisPriceChanged;
            }
        }                

        public DataColumn TaxID
		{
			get 
			{
				return this.colTaxID;
			}
		}

        public DataColumn IsIIAS
        {
            get
            {
                return this.colisIIAS;
            }
        }

        public DataColumn IsComboItem
        {
            get
            {
                return this.colIsComboItem;
            }
        }

        public DataColumn IsCompanionItem
        {
            get
            {
                return this.colIsCompanionItem;
            }
        }

        //Added By SRT(Ritesh Parekh) Date: 17-Aug-2009
        public DataColumn IsRxItem
        {
            get
            {
                return this.colIsRxItem;
            }
        }
        //End Of Added By SRT(Ritesh Parekh)

		public DataColumn TaxCode
		{
			get 
			{
				return this.colTaxCode;
			}
		}

        public DataColumn IsEBTItem
        {
            get
            {
                return this.colIsEBTItem;
            }
        }

        //Added By Amit Date 23 Nov 2011        
        public DataColumn IsMonitored
        {
            get
            {
                return this.colIsMonitored;
            }
        }        

        public DataColumn Category
        {
            get 
            {
                return this.colCategory;
            }
        }
        //End
        public DataColumn NonComboUnitPrice
        {
            get 
            {
                return this.colNonComboUnitPrice;
            }
        }

        public DataColumn ItemComboPricingID
        {
            get
            {
                return this.colItemComboPricingID;
            }
        }

        public DataColumn LoyaltyPoints
        {
            get
            {
                return this.colLoyaltyPoints;
            }
        }

        #region Sprint-21 - 1272 17-Aug-2015 JY Added to preserve other landuage item description at the time of printing receipt
        public DataColumn ItemDescriptionInOL
        {
            get
            {
                return this.colItemDescriptionInOL;
            }
        }
        #endregion

        //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
        public DataColumn IsPriceChangedByOverride
        {
            get
            {
                return this.colIsPriceChangedByOverride;
            }
        }

        //PRIMEPOS-2034 02-Mar-2018 JY Added
        public DataColumn CouponID
        {
            get
            {
                return this.colCouponID;
            }
        }

        //PRIMEPOS-2592 02-Nov-2018 JY Added 
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

        //PRIMEPOS-2768 18-Dec-2019 JY Added
        public DataColumn InvoiceDiscount
        {
            get
            {
                return this.colInvoiceDiscount;
            }
        }

        #region PRIMEPOS-2907 13-Oct-2020 JY Added
        public DataColumn IsOnSale
        {
            get
            {
                return this.colIsOnSale;
            }
        }
        #endregion

        #region PRIMEPOS-2402 08-Jul-2021 JY Added
        public DataColumn OldDiscountAmt
        {
            get
            {
                return this.colOldDiscountAmt;
            }
        }
        public DataColumn DiscountOverrideUser
        {
            get
            {
                return this.colDiscountOverrideUser;
            }
        }
        public DataColumn QuantityOverrideUser
        {
            get
            {
                return this.colQuantityOverrideUser;
            }
        }
        #region PRIMEPOS-3166N
        public DataColumn MonitorItemOverrideUser
        {
            get
            {
                return this.colMonitorItemOverrideUser;
            }
        }
        #endregion
        #region PRIMEPOS-3130
        public DataColumn ItemDescriptionMasked
        {
            get
            {
                return this.colItemDescriptionMasked;
            }
        }
        #endregion
        public DataColumn TaxOverrideUser
        {
            get
            {
                return this.colTaxOverrideUser;
            }
        }
        public DataColumn OldTaxCodesWithPercentage
        {
            get
            {
                return this.colOldTaxCodesWithPercentage;
            }
        }
        public DataColumn TaxOverrideAllOTCUser
        {
            get
            {
                return this.colTaxOverrideAllOTCUser;
            }
        }
        public DataColumn TaxOverrideAllRxUser
        {
            get
            {
                return this.colTaxOverrideAllRxUser;
            }
        }
        public DataColumn MaxDiscountLimitUser
        {
            get
            {
                return this.colMaxDiscountLimitUser;
            }
        }
        #endregion

        #region PRIMEPOS-3015 26-Oct-2021 JY Added
        public DataColumn OverrideRemark
        {
            get
            {
                return this.colOverrideRemark;
            }
        }
        #endregion

        #region PRIMEPOS-3098 20-Jun-2022 JY Added
        public DataColumn ItemGroupPrice
        {
            get
            {
                return this.colItemGroupPrice;
            }
        }
        #endregion
        #endregion //Properties

        #region Add and Get Methods

        public void AddRow(TransDetailRow row) 
		{
			AddRow(row, false);
		}
		
		public  void AddRow(TransDetailRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.TransDetailID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}
		
		public TransDetailRow GetRowByID(System.Int32 TransDetailID) 
		{
			return (TransDetailRow)this.Rows.Find(new object[] {TransDetailID});
		}


		public TransDetailRow AddRow(System.Int32 TransDetailID 
										,System.Int32 TransID 
										,System.Int32 QTY
										,System.Decimal Price
										,System.Decimal Discount
										,System.Decimal TaxAmount
										,System.Decimal ExtendedPrice
										, System.Int32 TaxID
										, System.String ItemID
										, System.String ItemDescription) 
		{
			TransDetailRow row = (TransDetailRow)this.NewRow();
			row.ItemArray=new object[] {TransDetailID,TransID,Discount,TaxAmount,ExtendedPrice,QTY,Price,ItemID.Trim(),ItemDescription,TaxID,"",0,"",false,false};  //PRIMEPOS-3096 17-May-2022 JY Modified
            row.OrignalPrice= row.NonComboUnitPrice = Price;
            row.InvoiceDiscount = 0;    //PRIMEPOS-2768 18-Dec-2019 JY Added
            row.IsOnSale = false;   //PRIMEPOS-2907 13-Oct-2020 JY Added
			/*row.TransDetailID=TransDetailID;
			row.TransID=TransID;
			row.ItemID=ItemID;
			row.Discount=Discount;
			row.Price=Price;
			row.QTY=QTY;
			row.TaxAmount=TaxAmount;
			row.ExtendedPrice=ExtendedPrice;
			row.ItemDescription=ItemDescription;*/
			this.Rows.Add(row);
			return row;
		}

		public  void MergeTable(DataTable dt) 
		{ 
			TransDetailRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (TransDetailRow)this.NewRow();
				
				if (dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TransDetailID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TransDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID].ToString()=="")?"0":dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID].ToString());
				
				if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_ItemID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_ItemID] = Convert.ToString((dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString()=="")? "":dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString());
				
				if (dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_ItemDescription] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_ItemDescription] = dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString();

				if (dr[clsPOSDBConstants.TransDetail_Fld_TransID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TransID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TransID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_TransID].ToString()=="")?"0":dr[clsPOSDBConstants.TransDetail_Fld_TransID].ToString());

				if (dr[clsPOSDBConstants.TransDetail_Fld_QTY] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_QTY] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_QTY] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_QTY].ToString()=="")? "0":dr[clsPOSDBConstants.TransDetail_Fld_QTY].ToString());

				if (dr[clsPOSDBConstants.TransDetail_Fld_Discount] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_Discount] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_Discount] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetail_Fld_Discount].ToString()=="")? "0":dr[clsPOSDBConstants.TransDetail_Fld_Discount].ToString());

				if (dr[clsPOSDBConstants.TransDetail_Fld_Price] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_Price] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_Price] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetail_Fld_Price].ToString()=="")? "0":dr[clsPOSDBConstants.TransDetail_Fld_Price].ToString());

                if (dr[clsPOSDBConstants.TransDetail_Fld_ItemCost] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetail_Fld_ItemCost] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransDetail_Fld_ItemCost] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetail_Fld_ItemCost].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransDetail_Fld_ItemCost].ToString());

				if (dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TaxAmount] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TaxAmount] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString()=="")? "0":dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString());

				if (dr[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].ToString()=="")? "0":dr[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].ToString());

				if (dr[clsPOSDBConstants.TaxCodes_Fld_TaxCode] == DBNull.Value) 
					row[clsPOSDBConstants.TaxCodes_Fld_TaxCode] = DBNull.Value;
				else
					row[clsPOSDBConstants.TaxCodes_Fld_TaxCode] = dr[clsPOSDBConstants.TaxCodes_Fld_TaxCode].ToString();

				if (dr[clsPOSDBConstants.TransDetail_Fld_TaxID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TaxID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TaxID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_TaxID].ToString()=="")?"0":dr[clsPOSDBConstants.TransDetail_Fld_TaxID].ToString());

                if (dr[clsPOSDBConstants.TransDetail_Fld_IsIIAS] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetail_Fld_IsIIAS] = false;
                else
                    row[clsPOSDBConstants.TransDetail_Fld_IsIIAS] = Convert.ToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsIIAS].ToString());

                if (dr[clsPOSDBConstants.TransDetail_Fld_IsEBT] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetail_Fld_IsEBT] = false;
                else
                    row[clsPOSDBConstants.TransDetail_Fld_IsEBT] = Convert.ToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsEBT].ToString());

                #region PRIMEPOS-3012 02-Nov-2021 JY Added
                try
                {
                    if (dr[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId] == DBNull.Value)
                        row[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId].ToString());
                }
                catch { }
                #endregion

                if (dr[clsPOSDBConstants.TransDetail_Fld_OrignalPrice] != DBNull.Value)
                    row[clsPOSDBConstants.TransDetail_Fld_OrignalPrice] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetail_Fld_OrignalPrice].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransDetail_Fld_OrignalPrice].ToString());

                if (dr[clsPOSDBConstants.TransDetail_Fld_IsComboItem] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetail_Fld_IsComboItem] = false;
                else
                    row[clsPOSDBConstants.TransDetail_Fld_IsComboItem] = Convert.ToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsComboItem].ToString());

                row[clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints] = Configuration.convertNullToInt(dr[clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints].ToString());

                row[clsPOSDBConstants.TransDetail_Fld_IsNonRefundable] = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsNonRefundable].ToString());  //PRIMEPOS-2592 06-Nov-2018 JY Added 
                #region  Added for Solutran - PRIMEPOS-2663 - NileshJ - 05-July-2019
                //PRIMEPOS-2836 20-Apr-2020 JY Corrected conversions
                row[clsPOSDBConstants.TransDetail_Fld_S3TransID] = Configuration.convertNullToInt64(dr[clsPOSDBConstants.TransDetail_Fld_S3TransID].ToString());//PRIMPOS-3265
                row[clsPOSDBConstants.TransDetail_Fld_S3PurAmount] = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3PurAmount].ToString());
                row[clsPOSDBConstants.TransDetail_Fld_S3TaxAmount] = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3TaxAmount].ToString());
                row[clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount] = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount].ToString());
                #endregion
                                
                row[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount] = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount]);    //PRIMEPOS-2768 18-Dec-2019 JY Added 
                row[clsPOSDBConstants.TransDetail_Fld_IsOnSale] = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsOnSale]);    //PRIMEPOS-2907 13-Oct-2020 JY Added

                this.AddRow(row);
			}
		}
		
		#endregion 
		public override DataTable Clone() 
		{
			TransDetailTable cln = (TransDetailTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new TransDetailTable();
		}

		internal void InitVars() 
		{
			this.colPrice= this.Columns[clsPOSDBConstants.TransDetail_Fld_Price];
            this.colDiscount = this.Columns[clsPOSDBConstants.TransDetail_Fld_ItemCost];
			this.colQTY= this.Columns[clsPOSDBConstants.TransDetail_Fld_QTY];
			this.colItemID= this.Columns[clsPOSDBConstants.TransDetail_Fld_ItemID];
			this.colItemDescription= this.Columns[clsPOSDBConstants.TransDetail_Fld_ItemDescription]; // Replace Item_Fld_Description to TransDetail_Fld_ItemDescription - NileshJ - (Showing Null value in ItemDescription) - 04July2019
            this.colTransID= this.Columns[clsPOSDBConstants.TransDetail_Fld_TransID];
			this.colTransDetailID= this.Columns[clsPOSDBConstants.TransDetail_Fld_TransDetailID];
			this.colDiscount= this.Columns[clsPOSDBConstants.TransDetail_Fld_Discount];
			this.colTaxAmount= this.Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount];
			this.colExtendedPrice= this.Columns[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice];
			this.colTaxCode= this.Columns[clsPOSDBConstants.TaxCodes_Fld_TaxCode];
			this.colTaxID= this.Columns[clsPOSDBConstants.TransDetail_Fld_TaxID];
			this.colItemCost=this.Columns[clsPOSDBConstants.TransDetail_Fld_ItemCost];

            this.colUserID = this.Columns[clsPOSDBConstants.TransDetail_Fld_UserID];
            this.colisPriceChanged= this.Columns[clsPOSDBConstants.TransDetail_Fld_IsPriceChanged];            
            this.colisIIAS =this.Columns[clsPOSDBConstants.TransDetail_Fld_IsIIAS];
            this.colIsCompanionItem = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsCompanionItem];
            //Added By SRT(Ritesh Parekh) Date : 17-Aug-2009
            this.colIsRxItem = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsRxItem];
            this.colIsEBTItem = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsEBT];
            //Following Code Added by Krishna on 15 July 2011
            this.colReturnTransDetailId = this.Columns[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId];
            //Till here Adde d by Krishna on 15 July 2011
            //Added by Amit Date 23 Nov 2011
            this.colIsMonitored = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsMonitored];            
            this.colCategory = this.Columns[clsPOSDBConstants.TransDetail_Fld_Category];
            //End
            this.colNonComboUnitPrice = this.Columns["NonComboUnitPrice"];
            this.colItemComboPricingID = this.Columns["ItemComboPricingID"];
            this.colOrignalPrice= this.Columns[clsPOSDBConstants.TransDetail_Fld_OrignalPrice];
            this.colIsComboItem = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsComboItem];
            this.colLoyaltyPoints= this.Columns[clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints];
            this.colItemDescriptionInOL = this.Columns["ItemDescriptionInOL"];   //Sprint-21 - 1272 17-Aug-2015 JY Added
            this.colIsPriceChangedByOverride = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsPriceChangedByOverride];    //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
            this.colCouponID = this.Columns[clsPOSDBConstants.TransDetail_Fld_CouponID];    //PRIMEPOS-2034 02-Mar-2018 JY Added    
            this.colIsNonRefundable = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsNonRefundable];  //PRIMEPOS-2592 02-Nov-2018 JY Added 

            #region NileshJ - SoluTran PRIMEPOS-2663
            this.colS3TransID = this.Columns[clsPOSDBConstants.TransDetail_Fld_S3TransID];
            this.colS3PurAmount = this.Columns[clsPOSDBConstants.TransDetail_Fld_S3PurAmount];
            this.colS3TaxAmount = this.Columns[clsPOSDBConstants.TransDetail_Fld_S3TaxAmount];
            this.colS3DiscountAmount = this.Columns[clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount];
            #endregion

            this.colInvoiceDiscount = this.Columns[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount];  //PRIMEPOS-2768 18-Dec-2019 JY Added
            this.colIsOnSale = this.Columns[clsPOSDBConstants.TransDetail_Fld_IsOnSale];    //PRIMEPOS-2907 13-Oct-2020 JY Added

            #region PRIMEPOS-2402 08-Jul-2021 JY Added
            this.colOldDiscountAmt = this.Columns[clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt];
            this.colDiscountOverrideUser = this.Columns[clsPOSDBConstants.TransDetail_Fld_DiscountOverrideUser];
            this.colQuantityOverrideUser = this.Columns[clsPOSDBConstants.TransDetail_Fld_QuantityOverrideUser];
            this.colTaxOverrideUser = this.Columns[clsPOSDBConstants.TransDetail_Fld_TaxOverrideUser];
            this.colOldTaxCodesWithPercentage = this.Columns[clsPOSDBConstants.TransDetail_Fld_OldTaxCodesWithPercentage];
            this.colTaxOverrideAllOTCUser = this.Columns[clsPOSDBConstants.TransDetail_Fld_TaxOverrideAllOTCUser];
            this.colTaxOverrideAllRxUser = this.Columns[clsPOSDBConstants.TransDetail_Fld_TaxOverrideAllRxUser];
            this.colMaxDiscountLimitUser = this.Columns[clsPOSDBConstants.TransDetail_Fld_MaxDiscountLimitUser];
            #endregion
            this.colOverrideRemark = this.Columns[clsPOSDBConstants.TransDetail_Fld_OverrideRemark];    //PRIMEPOS-3015 26-Oct-2021 JY Added
            this.colItemGroupPrice = this.Columns[clsPOSDBConstants.TransDetail_Fld_ItemGroupPrice];    //PRIMEPOS-3098 20-Jun-2022 JY Added
            this.colMonitorItemOverrideUser = this.Columns[clsPOSDBConstants.TransDetail_Fld_MonitorItemOverrideUser]; //PRIMEPOS-3166N
            this.colItemDescriptionMasked = this.Columns[clsPOSDBConstants.TransDetail_Fld_ItemDescriptionMasked]; //PRIMEPOS-3130
        }

        public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colTransDetailID= new DataColumn(clsPOSDBConstants.TransDetail_Fld_TransDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransDetailID);
			this.colTransDetailID.AllowDBNull = true;

			this.colTransID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TransID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransID);
			this.colTransID.AllowDBNull = true;

			this.colDiscount = new DataColumn(clsPOSDBConstants.TransDetail_Fld_Discount, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDiscount);
			this.colDiscount.AllowDBNull = true;

			this.colTaxAmount = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TaxAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxAmount);
			this.colTaxAmount.AllowDBNull = true;

			this.colExtendedPrice = new DataColumn(clsPOSDBConstants.TransDetail_Fld_ExtendedPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colExtendedPrice);
			this.colExtendedPrice.AllowDBNull = true;

			this.colQTY= new DataColumn(clsPOSDBConstants.TransDetail_Fld_QTY, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colQTY);
			this.colQTY.AllowDBNull = true;

			this.colPrice = new DataColumn(clsPOSDBConstants.TransDetail_Fld_Price, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colPrice);
			this.colPrice.AllowDBNull = true;

			this.colItemID= new DataColumn(clsPOSDBConstants.TransDetail_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemID);
			this.colItemID.AllowDBNull = true;

            this.colItemDescription= new DataColumn(clsPOSDBConstants.TransDetail_Fld_ItemDescription, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemDescription);
			this.colItemDescription.AllowDBNull = true;

			this.colTaxID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TaxID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxID);
			this.colTaxID.AllowDBNull = true;

			this.colTaxCode = new DataColumn(clsPOSDBConstants.TaxCodes_Fld_TaxCode, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxCode);
			this.colTaxCode.AllowDBNull = true;

            this.colItemCost = new DataColumn(clsPOSDBConstants.TransDetail_Fld_ItemCost, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemCost);
            this.colItemCost.AllowDBNull = true;

            this.colUserID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = true;

            this.colisPriceChanged= new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsPriceChanged, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisPriceChanged);
            this.colisPriceChanged.AllowDBNull = true;

            this.colisIIAS= new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsIIAS, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisIIAS);
            this.colisIIAS.AllowDBNull = true;

            this.colIsCompanionItem = new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsCompanionItem, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsCompanionItem);
            this.colIsCompanionItem.AllowDBNull = true;

            //Added By SRT(Ritesh Parekh) Date : 17-Aug-2009
            this.colIsRxItem = new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsRxItem, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsRxItem);
            this.colIsRxItem.AllowDBNull = true;
            //End Of Added By SRT(Ritesh Parekh)

            this.colIsEBTItem = new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsEBT, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsEBTItem);
            this.colIsEBTItem.AllowDBNull = true;

            //Following Added by Krishna on 15 July 2011
            this.colReturnTransDetailId = new DataColumn(clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colReturnTransDetailId);
            this.colReturnTransDetailId.AllowDBNull = true;
            //Till here added by krishna on 15 july 2011

            //Added By Amit Date 23 Nov 2011
            this.colIsMonitored = new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsMonitored, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsMonitored);
            this.colIsMonitored.AllowDBNull = true;

            this.colCategory =new DataColumn(clsPOSDBConstants.TransDetail_Fld_Category,typeof(System.String),null,System.Data.MappingType.Element);
            this.Columns.Add(this.colCategory);
            this.colCategory.AllowDBNull=true;
            //End

            this.colNonComboUnitPrice = new DataColumn("NonComboUnitPrice", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNonComboUnitPrice);
            this.colNonComboUnitPrice.AllowDBNull = true;

            this.colItemComboPricingID = new DataColumn("ItemComboPricingID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemComboPricingID);

            this.colOrignalPrice= new DataColumn(clsPOSDBConstants.TransDetail_Fld_OrignalPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOrignalPrice);
            this.colOrignalPrice.AllowDBNull = true;

            this.colIsComboItem= new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsComboItem, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsComboItem);
            this.colIsComboItem.AllowDBNull = true;


            this.colLoyaltyPoints = new DataColumn(clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLoyaltyPoints);
            this.colLoyaltyPoints.AllowDBNull = true;

            #region Sprint-21 - 1272 17-Aug-2015 JY Added
            this.colItemDescriptionInOL = new DataColumn("ItemDescriptionInOL", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemDescriptionInOL);
            this.colItemDescriptionInOL.AllowDBNull = true;
            #endregion

            //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
            this.colIsPriceChangedByOverride = new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsPriceChangedByOverride, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsPriceChangedByOverride);
            this.colIsPriceChangedByOverride.AllowDBNull = true;

            //PRIMEPOS-2034 02-Mar-2018 JY Added
            this.colCouponID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_CouponID, typeof(long), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCouponID);
            this.colCouponID.AllowDBNull = true;

            //PRIMEPOS-2592 02-Nov-2018 JY Added 
            this.colIsNonRefundable = new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsNonRefundable, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsNonRefundable);
            this.colIsNonRefundable.AllowDBNull = true;

            #region NileshJ - SoluTran - PRIMEPOS-2663
            this.colS3TransID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_S3TransID, typeof(System.Int64), null, System.Data.MappingType.Element); //PRIMEPOS-3265
            this.Columns.Add(this.colS3TransID);
            this.colS3TransID.AllowDBNull = true;

            this.colS3PurAmount = new DataColumn(clsPOSDBConstants.TransDetail_Fld_S3PurAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3PurAmount);
            this.colS3PurAmount.AllowDBNull = true;

            this.colS3TaxAmount = new DataColumn(clsPOSDBConstants.TransDetail_Fld_S3TaxAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3TaxAmount);
            this.colS3TaxAmount.AllowDBNull = true;

            this.colS3DiscountAmount = new DataColumn(clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3DiscountAmount);
            this.colS3DiscountAmount.AllowDBNull = true;
            #endregion

            //PRIMEPOS-2768 18-Dec-2019 JY Added
            this.colInvoiceDiscount = new DataColumn(clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInvoiceDiscount);
            this.colInvoiceDiscount.AllowDBNull = true;

            #region PRIMEPOS-2907 13-Oct-2020 JY Added
            this.colIsOnSale = new DataColumn(clsPOSDBConstants.TransDetail_Fld_IsOnSale, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsOnSale);
            this.colIsOnSale.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-2402 08-Jul-2021 JY Added
            this.colOldDiscountAmt = new DataColumn(clsPOSDBConstants.TransDetail_Fld_OldDiscountAmt, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOldDiscountAmt);
            this.colOldDiscountAmt.AllowDBNull = true;
            this.colDiscountOverrideUser = new DataColumn(clsPOSDBConstants.TransDetail_Fld_DiscountOverrideUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDiscountOverrideUser);
            this.colDiscountOverrideUser.AllowDBNull = true;
            this.colQuantityOverrideUser = new DataColumn(clsPOSDBConstants.TransDetail_Fld_QuantityOverrideUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colQuantityOverrideUser);
            this.colQuantityOverrideUser.AllowDBNull = true;
            this.colTaxOverrideUser = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TaxOverrideUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaxOverrideUser);
            this.colTaxOverrideUser.AllowDBNull = true;
            this.colOldTaxCodesWithPercentage = new DataColumn(clsPOSDBConstants.TransDetail_Fld_OldTaxCodesWithPercentage, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOldTaxCodesWithPercentage);
            this.colOldTaxCodesWithPercentage.AllowDBNull = true;
            this.colTaxOverrideAllOTCUser = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TaxOverrideAllOTCUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaxOverrideAllOTCUser);
            this.colTaxOverrideAllOTCUser.AllowDBNull = true;
            this.colTaxOverrideAllRxUser = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TaxOverrideAllRxUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaxOverrideAllRxUser);
            this.colTaxOverrideAllRxUser.AllowDBNull = true;
            this.colMaxDiscountLimitUser = new DataColumn(clsPOSDBConstants.TransDetail_Fld_MaxDiscountLimitUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMaxDiscountLimitUser);
            this.colMaxDiscountLimitUser.AllowDBNull = true;
            #endregion
            #region PRIMEPOS-3015 26-Oct-2021 JY Added
            this.colOverrideRemark = new DataColumn(clsPOSDBConstants.TransDetail_Fld_OverrideRemark, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOverrideRemark);
            this.colOverrideRemark.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-3098 20-Jun-2022 JY Added
            this.colItemGroupPrice = new DataColumn(clsPOSDBConstants.TransDetail_Fld_ItemGroupPrice, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemGroupPrice);
            this.colItemGroupPrice.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-3166N
            this.colMonitorItemOverrideUser = new DataColumn(clsPOSDBConstants.TransDetail_Fld_MonitorItemOverrideUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMonitorItemOverrideUser);
            this.colMonitorItemOverrideUser.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-3130
            this.colItemDescriptionMasked = new DataColumn(clsPOSDBConstants.TransDetail_Fld_ItemDescriptionMasked, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemDescriptionMasked);
            this.colItemDescriptionMasked.AllowDBNull = true;
            #endregion

            this.PrimaryKey = new DataColumn[] {this.colTransDetailID};
		}

		public  TransDetailRow NewTransDetailRow() 
		{
			return (TransDetailRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new TransDetailRow(builder);
		}

        public IEnumerable<TransDetailRow> AsEnumerable()
        {
            foreach (TransDetailRow row in this.Rows)
            {
                yield return row;
            }
        }

        public TransDetailRow FindRow(int transDetID)
        {
            foreach (TransDetailRow row in this.Rows)
            {
                if (row.TransDetailID == transDetID)
                    return row;
            }
            return null;
        }
	}
}
