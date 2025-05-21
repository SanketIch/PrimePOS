//using POS.Resources;

namespace POS_Core.CommonData.Rows 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    public class TransDetailRow : DataRow 
	{
		private TransDetailTable table;

		internal TransDetailRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (TransDetailTable)this.Table;
		}


		#region Public Properties
		public System.Int32 TransDetailID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.TransDetailID];
				}
				catch
				{ 
					return 0 ; 
				}
			}
			set 
			{
				this[this.table.TransDetailID] = value; 
			}
		}

		public System.Int32 TransID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.TransID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.TransID] = value; }
		}

        //Following Code Added by Krishna on 15 July 2011
        public System.Int32 ReturnTransDetailId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ReturnTransDetailId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ReturnTransDetailId] = value; }
        }
        //Till here Added by Krishna on 15 July

		public System.Int32 QTY
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.QTY];
				}
				catch
				{ 
					return 0 ; 
				}
			}
            set
            {
                this[this.table.QTY] = value;
            }
		}


		public System.Decimal Price
		{
			get 
			{ 
				try 
				{ 
					return (System.Decimal)this[this.table.Price];
				}
				catch
				{ 
					return 0 ; 
				}
			}
            set
            {
                decimal oldVal = this.Price;
                try
                {

                    this[this.table.Price] = value;
                    ValidatePrice(this,"P");
                }
                catch (Exception exp)
                {
                    this[this.table.Price] = oldVal;
                    throw (exp);
                }
            }
		}

        public System.Decimal OrignalPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.OrignalPrice];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {

                    this[this.table.OrignalPrice] = value;
                }
                catch (Exception exp)
                {
                    this[this.table.Price] = 0;
                }
            }
        }
        public System.Decimal ItemCost
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.ItemCost];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ItemCost] = value; }
        }

		public System.Decimal Discount
		{
			get 
			{ 
				try 
				{ 
					return (System.Decimal)this[this.table.Discount];
				}
				catch
				{ 
					return 0; 
				}
			}
            set
            {
                decimal oldVal = this.Discount;
                try
                {

                    this[this.table.Discount] = value;
                    ValidatePrice(this, clsPOSDBConstants.STRINGIMAGE);
                }
                catch (Exception exp)
                {
                    this[this.table.Discount] = oldVal;
                    throw (exp);
                }
            }
		}

		public System.Decimal TaxAmount
		{
			get 
			{ 
				try 
				{ 
					return (System.Decimal)this[this.table.TaxAmount];
				}
				catch
				{ 
					return 0; 
				}
			}
            set
            {
                decimal oldVal = this.TaxAmount;
                try
                {

                    this[this.table.TaxAmount] = value;
                    ValidatePrice(this,"T");
                }
                catch (Exception exp)
                {
                    this[this.table.TaxAmount] = oldVal;
                    throw (exp);
                }
            }
		}

		public System.Decimal ExtendedPrice
		{
			get 
			{ 
				try 
				{ 
					return (System.Decimal)this[this.table.ExtendedPrice];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set {
                decimal oldVal=this.ExtendedPrice;
                try
                {

                    this[this.table.ExtendedPrice] = value;
                    ValidatePrice(this,"E");
                }
                catch (Exception exp)
                {
                    this[this.table.ExtendedPrice] = oldVal;
                    throw (exp);
                }
            }
		}

		public System.String ItemID
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.ItemID];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.ItemID] = value.Trim(); } //PRIMEPOS-3096 17-May-2022 JY Modified
        }

		public System.String ItemDescription
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.ItemDescription];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.ItemDescription] = value; }
		}

		public System.String TaxCode
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.TaxCode];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.TaxCode] = value; }
		}

		public System.Int32 TaxID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.TaxID];
				}
				catch
				{ 
					return 0;
				}
			} 
			set { this[this.table.TaxID] = value; }
		}

        public System.String UserID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.UserID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.UserID] = value; }
        }

        //This property is used to track the ItemPriceHistory which covers all records like Taxable/non-taxable (NNN)
        public System.Boolean IsPriceChanged
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsPriceChanged];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsPriceChanged] = value; }
        }

        public System.Boolean IsComboItem
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsComboItem];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsComboItem] = value; }
        }

        public System.Boolean IsIIAS
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsIIAS];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsIIAS] = value; }
        }

        public System.Boolean IsCompanionItem
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsCompanionItem];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsCompanionItem] = value; }
        }
        
        public System.Boolean IsRxItem
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsRxItem];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsRxItem] = value; }
        }

        public System.Boolean IsEBTItem
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsEBTItem];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsEBTItem] = value; }
        }

        //Added By Amit Date 23 Nov 2011
        public System.Boolean IsMonitored
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsMonitored];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsMonitored] = value; }
        }        

        public System.String Category
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Category];
                }
                catch
                {
                    return null;
                }

            }
            set { this[this.table.Category] = value; }
        }

        //End

        public System.Decimal NonComboUnitPrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.NonComboUnitPrice];
                }
                catch
                {
                    return 0;
                }

            }
            set { this[this.table.NonComboUnitPrice] = value; }
        }

        public System.Int32 ItemComboPricingID
		{
			get 
			{ 
				try 
				{
                    return (System.Int32)this[this.table.ItemComboPricingID];
				}
				catch
				{ 
					return 0;
				}
			}
            set { this[this.table.ItemComboPricingID] = value; }
		}

        public System.Decimal LoyaltyPoints
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.LoyaltyPoints];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LoyaltyPoints] = value; }
        }

        #region Sprint-21 - 1272 17-Aug-2015 JY Added to preserve other landuage item description at the time of printing receipt
        public System.String ItemDescriptionInOL
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ItemDescriptionInOL];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ItemDescriptionInOL] = value; }
        }
        #endregion

        #region Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added this property to track the price override records with price change observed in it
        public System.Boolean IsPriceChangedByOverride
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsPriceChangedByOverride];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsPriceChangedByOverride] = value; }
        }
        #endregion

        #region PRIMEPOS-2034 02-Mar-2018 JY Added
        public System.Int64 CouponID
        {
            get
            {
                try
                {
                    if (this[this.table.CouponID].ToString().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return (long)this[this.table.CouponID];
                    }
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.CouponID] = value;
            }
        }
        #endregion

        #region PRIMEPOS-2592 02-Nov-2018 JY Added 
        public System.Boolean IsNonRefundable
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsNonRefundable];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsNonRefundable] = value; }
        }
        #endregion

        #region Added for Solutran - PRIMEPOS-2663 - NileshJ

        public System.Int64 S3TransID //PRIMEPOS-3265
        {
            get
            {
                try
                {
                    return (System.Int64)this[this.table.S3TrasID]; //PRIMEPOS-3265
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3TrasID] = value; }
        }

        public System.Decimal S3PurAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3PurAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3PurAmount] = value; }
        }

        public System.Decimal S3TaxAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3TaxAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3TaxAmount] = value; }
        }

        public System.Decimal S3DiscountAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.S3DiscountAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.S3DiscountAmount] = value; }
        }

        #endregion

        //PRIMEPOS-2768 18-Dec-2019 JY Added InvoiceDiscount
        public System.Decimal InvoiceDiscount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.InvoiceDiscount];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                decimal oldVal = this.InvoiceDiscount;
                try
                {
                    this[this.table.InvoiceDiscount] = Math.Round(value, 4, MidpointRounding.AwayFromZero);
                }
                catch
                {
                    this[this.table.InvoiceDiscount] = oldVal;
                }
            }
        }

        #region PRIMEPOS-2907 13-Oct-2020 JY Added
        public System.Boolean IsOnSale
        {
            get
            {
                try
                {
                    return (bool)this[this.table.IsOnSale];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsOnSale] = value; }
        }
        #endregion

        #region PRIMEPOS-2402 08-Jul-2021 JY Added
        //Added this property to track the discount override records with discount change observed in it
        public System.Decimal OldDiscountAmt
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.OldDiscountAmt];
                }
                catch
                {
                    return 0;
                }
            }
            set 
            { 
                this[this.table.OldDiscountAmt] = value; 
            }
        }
        public System.String DiscountOverrideUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DiscountOverrideUser];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.DiscountOverrideUser] = value; }
        }
        //Added for quantity override
        public System.String QuantityOverrideUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.QuantityOverrideUser];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.QuantityOverrideUser] = value; }
        }
        #region PRIMEPOS-3166N
        public System.String MonitorItemOverrideUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.MonitorItemOverrideUser];
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { this[this.table.MonitorItemOverrideUser] = value; }
        }
        #endregion
        //Added for tax override
        public System.String TaxOverrideUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaxOverrideUser];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TaxOverrideUser] = value; }
        }
        public System.String OldTaxCodesWithPercentage
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.OldTaxCodesWithPercentage];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.OldTaxCodesWithPercentage] = value; }
        }
        public System.String TaxOverrideAllOTCUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaxOverrideAllOTCUser];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TaxOverrideAllOTCUser] = value; }
        }
        public System.String TaxOverrideAllRxUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TaxOverrideAllRxUser];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TaxOverrideAllRxUser] = value; }
        }
        public System.String MaxDiscountLimitUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.MaxDiscountLimitUser];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.MaxDiscountLimitUser] = value; }
        }
        #endregion

        #region PRIMEPOS-3015 26-Oct-2021 JY Added
        public System.String OverrideRemark
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.OverrideRemark];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.OverrideRemark] = value; }
        }
        #endregion

        #region PRIMEPOS-3098 20-Jun-2022 JY Added
        public System.Boolean ItemGroupPrice
        {
            get
            {
                try
                {
                    return (bool)this[this.table.ItemGroupPrice];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.ItemGroupPrice] = value; }
        }
        #endregion
        #region PRIMEPOS-3130
        public System.String ItemDescriptionMasked
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ItemDescriptionMasked];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.ItemDescriptionMasked] = value;
            }
        }
        #endregion
        #endregion

        private void ValidatePrice(TransDetailRow oDRow,string ValidateBy)
        {
            if (this.isSalesTransaction == true && this.ItemID != "" && this.IsCompanionItem == false)
            {
                decimal unitPrice = 0;
                if (ValidateBy=="E")
                {
                    //unitPrice = oDRow.ExtendedPrice + oDRow.TaxAmount - oDRow.Discount;//Commented By shitaljit
                    //The price validation should condsider the price without Tax for minimum price.
                    unitPrice = oDRow.ExtendedPrice - oDRow.Discount;
                }
                else
                {
                    //unitPrice = (oDRow.Price * oDRow.QTY) + oDRow.TaxAmount - oDRow.Discount;//Commented By shitaljit
                    unitPrice = (oDRow.Price * oDRow.QTY)- oDRow.Discount;//The price validation should condsider the price without Tax for minimum price.
                }

                if (unitPrice > 0)
                {
                    unitPrice = unitPrice / oDRow.QTY;
                }

                bool bOnSale=false;
                POS_Core.BusinessRules.Item oItem=new POS_Core.BusinessRules.Item();
                ItemData oIData = oItem.Populate(oDRow.ItemID);
                if (oIData.Item.Count > 0)
                {
                    //if (oIData.Item[0].SaleStartDate != DBNull.Value && oIData.Item[0].SaleEndDate != DBNull.Value && oIData.Item[0].isOnSale == true)
                    if(oIData.Item[0].isOnSale == true && oIData.Item[0].SaleStartDate != DBNull.Value && oIData.Item[0].SaleEndDate != DBNull.Value
                        && DateTime.Now.Date >= Convert.ToDateTime(oIData.Item[0].SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oIData.Item[0].SaleEndDate).Date)  //PRIMEPOS-3138 01-Sep-2022 JY modified
                    {
                        //if (DateTime.Now.Date >= Convert.ToDateTime(oIData.Item[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oIData.Item[0].SaleEndDate))
                        //{
                            bOnSale = true;
                        //}
                    }
                    else
                    {
                        try
                        {
                            POS_Core.BusinessRules.Department oDept = new POS_Core.BusinessRules.Department();
                            DepartmentData oDeptData = oDept.Populate(oIData.Item[0].DepartmentID);
                            if (oDeptData.Department.Count > 0)
                            {
                                if (DateTime.Now.Date >= Convert.ToDateTime(oDeptData.Department[0].SaleStartDate) && DateTime.Now.Date <= Convert.ToDateTime(oDeptData.Department[0].SaleEndDate))
                                {
                                    bOnSale = true;
                                }
                            }
                        }
                        catch { }
                    }

                    if (bOnSale == false)
                    {
                        POS_Core.BusinessRules.ItemPriceValidation oItemPriceValid = new POS_Core.BusinessRules.ItemPriceValidation();
                       decimal dMinSellingAmount =0;
                        if (oItemPriceValid.ValidateItem(oDRow.ItemID, unitPrice,out dMinSellingAmount) == false)
                        {
                            throw (new ErrorLogging.POSExceptions(
                                "Item " + oDRow.ItemID + " - " + oDRow.ItemDescription +
                                " price is not valid.\nMinimum price defined for item " +
                                Configuration.CInfo.CurrencySymbol + dMinSellingAmount.ToString("######0.00") + ".",
                                (long) POSErrorENUM.ItemPrice_Validation));
                        }
                    }
                }
            }
        }

        public void Copy(TransDetailRow oCopyTo)
        {            
            oCopyTo.IsPriceChanged = this.IsPriceChanged;
            oCopyTo.isSalesTransaction = false;
            oCopyTo.QTY = this.QTY;
            oCopyTo.Price = this.Price;
            oCopyTo.ItemCost = this.ItemCost;
            oCopyTo.ItemDescription = this.ItemDescription;
            oCopyTo.ItemID = this.ItemID;
            oCopyTo.TaxAmount = this.TaxAmount;
            oCopyTo.TaxCode = this.TaxCode;
            oCopyTo.TaxID = this.TaxID;
            oCopyTo.Discount = this.Discount;
            oCopyTo.TransDetailID = this.TransDetailID;
            oCopyTo.TransID = this.TransID;
            oCopyTo.UserID = this.UserID;
            oCopyTo.ExtendedPrice = this.ExtendedPrice;
            oCopyTo.isSalesTransaction = this.isSalesTransaction;
            oCopyTo.IsIIAS = this.IsIIAS;
            oCopyTo.IsCompanionItem = this.IsCompanionItem;
            //Added By SRT(Ritesh Parekh) Date: 17-Aug-2009
            oCopyTo.IsRxItem = this.IsRxItem;
            //End Of Added By SRT(Ritesh Parekh)
            oCopyTo.ReturnTransDetailId = this.ReturnTransDetailId;//Added by Krishna on 15 July 2011
            //Adde By Amit Date 23 Nov 2011            
            oCopyTo.IsMonitored = this.IsMonitored;            
            oCopyTo.Category = this.Category;
            //End
            oCopyTo.OrignalPrice = this.OrignalPrice;
            oCopyTo.IsComboItem = this.IsComboItem;
            oCopyTo.LoyaltyPoints = this.LoyaltyPoints;
            oCopyTo.CouponID = this.CouponID;   //PRIMEPOS-2034 02-Mar-2018 JY Added
            oCopyTo.IsNonRefundable = this.IsNonRefundable; //PRIMEPOS-2592 02-Nov-2018 JY Added 
            oCopyTo.InvoiceDiscount = this.InvoiceDiscount; //PRIMEPOS-2768 03-Jan-2020 JY Added
            //oCopyTo.IsOnSale = this.IsOnSale;   //PRIMEPOS-2907 13-Oct-2020 JY intentionally added and commented as when we price update then it should not retain OnSale flag
        }

        public bool isSalesTransaction = false;
	}
}