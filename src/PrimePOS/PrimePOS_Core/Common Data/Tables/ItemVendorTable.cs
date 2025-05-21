// ----------------------------------------------------------------
// ----------------------------------------------------------------

 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;

  public class ItemVendorTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colItemDetailID;
     private DataColumn colVendorItemID;
     private DataColumn colItemID;
     private DataColumn colVendorID;
     private DataColumn colVendorCode;
     private DataColumn colVendorName;
     private DataColumn colVenorCostPrice;
     private DataColumn colLastOrderDate;

     //properties Added By SRT(Abhishek) Date : 01/12/2009
      private DataColumn colAvgWholeSalePrice;
      private DataColumn colCatPrice;
      private DataColumn colContractPrice;
      private DataColumn colDealerAdjustPrice;
      private DataColumn colFederalUpperLimitPrice;
      private DataColumn colManufacturerSuggPrice;
      private DataColumn colNetItemPrice;
      private DataColumn colProducerPrice;
      private DataColumn colRetailPrice;
      private DataColumn colIsDeleted;
     // End of properties Added By SRT(Abhishek) Date : 01/12/2009

      //Added by SRT(Abhishek) Date : 24/09/2009
      // INV - Invoice Billing Price
      // BCH - Base Charge         
      // PBQ	- Unit Price Beginning Quantity
      // RES	- Resale
     
      private DataColumn colInvoiceBillingPrcice;
      private DataColumn colBaseCharge;
      private DataColumn colUnitPriceBegQuantity;
      private DataColumn colResalePrice;
      //End Of Added by SRT(Abhishek) Date : 24/09/2009

      //Added by Atul joshi on 22-10-2010
      private DataColumn colUnitCostPrice;

      private DataColumn colPckSize;
      private DataColumn colPckQty;
      private DataColumn colPckUnit;

      private DataColumn colHammacherDeptClass;//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
      private DataColumn colVendorSalePrice;//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
      private DataColumn colSaleStartDate;//Promotional Pricing
      private DataColumn colSaleEndDate;//Promotional Pricing
     #region Constructors 
	  
     internal ItemVendorTable() : base(clsPOSDBConstants.ItemVendor_tbl) { this.InitClass(); }
     internal ItemVendorTable(DataTable table) : base(table.TableName) {}
     #endregion
      #region Properties
      /// 
      /// Public Property for get all Rows in Table
      /// 
      public int Count {
        get {
          return this.Rows.Count;
        }
      }

    public ItemVendorRow this[int index] {
      get {
        return ((ItemVendorRow)(this.Rows[index]));
      }
    }

    /// <summary>
    /// Public Property DataColumn VendorItemID
    /// </summary>	
    /// 

	public DataColumn ItemDetailID 
	{
		get 
		{
			return this.colItemDetailID;
		}
	}

    public DataColumn VendorItemID 
	{
      get {
        return this.colVendorItemID;
      }
    }

    /// <summary>
    /// Public Property DataColumn ItemID
    /// </summary>	
    public DataColumn ItemID {
      get {
        return this.colItemID;
      }
    }

    /// <summary>
    /// Public Property DataColumn VendorID
    /// </summary>	
    public DataColumn VendorID {
      get {
        return this.colVendorID;
      }
    }

    /// <summary>
    /// Public Property DataColumn VendorCode
    /// </summary>	
    public DataColumn VendorCode {
      get {
        return this.colVendorCode;
      }
    }

    /// <summary>
    /// Public Property DataColumn VendorName
    /// </summary>	
    public DataColumn VendorName
    {
        get
        {
            return this.colVendorName;
        }
    }

    public DataColumn HammacherDeptClass
    {
        get
        {
            return this.colHammacherDeptClass;
        }
    }

    public DataColumn SaleStartDate
    {
        get
        {
            return this.colSaleStartDate;
        }
    }
    public DataColumn SaleEndDate
    {
        get
        {
            return this.colSaleEndDate;
        }
    }
    public DataColumn VendorSalePrice
    {
        get
        {
            return this.colVendorSalePrice;
        }
    }

    /// <summary>
    /// Public Property DataColumn VenorCostPrice
    /// </summary>	
    public DataColumn VenorCostPrice
    {
        get
        {
            return this.colVenorCostPrice;
        }
    }

    /// <summary>
    /// Public Property DataColumn LastOrderDate
    /// </summary>	
    public DataColumn LastOrderDate {
      get {
        return this.colLastOrderDate;
      }
    }

      //Added By  SRT (Abhishek) Date : 01/12 /2009
      
      public DataColumn RetailPrice
      {
          get
          {
              return this.colRetailPrice;
          }
      }

      public DataColumn ProducersPrice
      {
          get
          {
              return this.colProducerPrice;
          }
      }

      public DataColumn NetItemPrice
      {
          get
          {
              return this.colNetItemPrice;
          }
      }

      public DataColumn ManufacturerSuggPrice
      {
          get
          {
              return this.colManufacturerSuggPrice;
          }
      }

      public DataColumn FedrelUpperLimitPrice
      {
          get
          {
              return this.colFederalUpperLimitPrice;
          }
      }

      public DataColumn DealerAdjustedPrice
      {
          get
          {
              return this.colDealerAdjustPrice;
          }
      }

       public DataColumn ContractPrice
       {
          get
          {
              return this.colContractPrice;
          }
       }

       public DataColumn CatalogPrice
       {
          get
          {
              return this.colCatPrice;
          }
       }

       public DataColumn AverageWholeSalePrice
       {
          get
          {
              return this.colAvgWholeSalePrice;
          }
       }


      //Added by SRT(Abhishek) Date : 24/09/2009

       // INV - Invoice Billing Price
       // BCH - Base Charge         
       // PBQ	- Unit Price Beginning Quantity
       // RES	- Resale

      public DataColumn InVoiceBillingPrice
      {
          get
          {
              return this.colInvoiceBillingPrcice;
          }
      }

      public DataColumn BaseCharge
      {
          get
          {
              return this.colBaseCharge;
          }
      }

      public DataColumn UnitPriceBegQuantity
      {
          get
          {
              return this.colUnitPriceBegQuantity;
          }
      }
      
      //Added by ATul Joshi on 22-10-2010
      public DataColumn UnitCostPrice
      {
          get
          {
              return this.colUnitCostPrice;
          }
      }

      public DataColumn Resale
      {
          get
          {
              return this.colResalePrice;
          }
      }
      //End Of Added by SRT(Abhishek) Date : 24/09/2009


      public DataColumn IsDeleted
      {
          get
          {
              return this.colIsDeleted;
          }
      }
     //End of Added By  SRT (Abhishek) Date : 01/12 /2009

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

	  #endregion //Properties
      #region Add and Get Methods 

      public void AddRow(ItemVendorRow row) {
        AddRow(row, false);
      }

    public void AddRow(ItemVendorRow row, bool preserveChanges) {
      if(this.GetRowByID(row.ItemDetailID) == null) {
        this.Rows.Add(row);
        if(!preserveChanges) {
          row.AcceptChanges();
        }
      }
    }

    public ItemVendorRow AddRow(System.String VendorItemID, System.String ItemID, System.Int32 VendorID, System.String VendorCode, System.String VendorName, System.Decimal VenorCostPrice, System.DateTime LastOrderDate, System.Decimal VendorSalePrice, System.String HammacherDeptClass, System.DateTime SaleStartDate, System.DateTime SaleEndDate)
    {
      ItemVendorRow row = (ItemVendorRow)this.NewRow();
      //row.ItemArray = new object[] {VendorItemID,ItemID,VendorID,VendorCode,VendorName,VenorCostPrice,LastOrderDate};
      row.VendorItemID = VendorItemID;
      row.ItemID = ItemID;
      row.VendorID = VendorID;
      row.VendorCode = VendorCode;
      row.VendorName = VendorName;
      row.VenorCostPrice = VenorCostPrice;
      row.LastOrderDate = LastOrderDate;
      row.ProducersPrice = 0;
      row.NetItemPrice = 0;
      row.ManufacturerSuggPrice = 0;
      row.DealerAdjustedPrice = 0;
      row.ContractPrice = 0;
      row.CatalogPrice = 0;
      row.HammacherDeptClass = HammacherDeptClass;
      row.VendorSalePrice = VendorSalePrice;
      row.SaleStartDate = SaleStartDate;
      row.SaleEndDate = SaleEndDate;
      row.AverageWholeSalePrice = 0;
        
      this.Rows.Add(row);
      return row;
    }

    public ItemVendorRow GetRowByID(System.Int32 ItemDetailID) {
      return (ItemVendorRow)this.Rows.Find(new object[] {ItemDetailID});
    }

    public void MergeTable(DataTable dt) { 
      //add any rows in the DataTable 
      ItemVendorRow row;
      foreach(DataRow dr in dt.Rows) {
        row = (ItemVendorRow)this.NewRow();

		if (dr[clsPOSDBConstants.ItemVendor_Fld_ItemDetailID] == DBNull.Value) 
			row[clsPOSDBConstants.ItemVendor_Fld_ItemDetailID] = DBNull.Value;
		else
			row[clsPOSDBConstants.ItemVendor_Fld_ItemDetailID] = Convert.ToInt32(dr[clsPOSDBConstants.ItemVendor_Fld_ItemDetailID].ToString());

       if (dr[clsPOSDBConstants.ItemVendor_Fld_VendorItemID] == DBNull.Value) 
         row[clsPOSDBConstants.ItemVendor_Fld_VendorItemID] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemVendor_Fld_VendorItemID] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_VendorItemID].ToString());

       if (dr[clsPOSDBConstants.ItemVendor_Fld_ItemID] == DBNull.Value) 
         row[clsPOSDBConstants.ItemVendor_Fld_ItemID] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemVendor_Fld_ItemID] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_ItemID].ToString());

       if (dr[clsPOSDBConstants.ItemVendor_Fld_VendorID] == DBNull.Value) 
         row[clsPOSDBConstants.ItemVendor_Fld_VendorID] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemVendor_Fld_VendorID] = Convert.ToInt32(dr[clsPOSDBConstants.ItemVendor_Fld_VendorID].ToString());

       if (dr[clsPOSDBConstants.ItemVendor_Fld_VendorCode] == DBNull.Value) 
         row[clsPOSDBConstants.ItemVendor_Fld_VendorCode] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemVendor_Fld_VendorCode] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_VendorCode].ToString());

       if (dr[clsPOSDBConstants.ItemVendor_Fld_VendorName] == DBNull.Value) 
         row[clsPOSDBConstants.ItemVendor_Fld_VendorName] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemVendor_Fld_VendorName] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_VendorName].ToString());

       if (dr[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] == DBNull.Value) 
         row[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] = 0.00;
       else
         row[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].ToString());

       if (dr[clsPOSDBConstants.ItemVendor_Fld_LastOrderDate] == DBNull.Value) 
         row[clsPOSDBConstants.ItemVendor_Fld_LastOrderDate] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemVendor_Fld_LastOrderDate] = Convert.ToDateTime(dr[clsPOSDBConstants.ItemVendor_Fld_LastOrderDate].ToString());

        //Properties Added By SRT(Abhishek) Date : 01/12/2009

        if(dr[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_CatPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_CatPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_CatPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_CatPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_ContractPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_ContractPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_ContractPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_ContractPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_RetailPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_RetailPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_RetailPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_RetailPrice].ToString());

       //Added by SRT(Abhishek) Date : 24/09/2009  
     
        // INV - Invoice Billing Price
        // BCH - Base Charge         
        // PBQ	- Unit Price Beginning Quantity
        // RES	- Resale
     
        if (dr[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_BaseCharge] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_BaseCharge] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_BaseCharge] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_BaseCharge].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity].ToString());

        //Added by ATul Joshi on 22-10-2010
        if (dr[clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice].ToString());

        


        if (dr[clsPOSDBConstants.ItemVendor_Fld_ResalePrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_ResalePrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_ResalePrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_ResalePrice].ToString());

       //End Of Added by SRT(Abhishek) Date : 24/09/2009

        if (dr[clsPOSDBConstants.ItemVendor_Fld_IsDeleted] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_IsDeleted] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_IsDeleted] = Convert.ToBoolean(dr[clsPOSDBConstants.ItemVendor_Fld_IsDeleted].ToString());  
     
       //End of Properties Added By SRT(Abhishek) Date : 01/12/2009  

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

        //Till here Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
        if (dr[clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass] = DBNull.Value;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass] = Convert.ToString(dr[clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass].ToString());

        if (dr[clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice] = 0.00;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice].ToString());


        if (dr[clsPOSDBConstants.ItemVendor_Fld_SaleStartDate] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_SaleStartDate] = DBNull.Value;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_SaleStartDate] = Convert.ToDateTime(dr[clsPOSDBConstants.ItemVendor_Fld_SaleStartDate].ToString());


        if (dr[clsPOSDBConstants.ItemVendor_Fld_SaleEndDate] == DBNull.Value)
            row[clsPOSDBConstants.ItemVendor_Fld_SaleEndDate] = DBNull.Value;
        else
            row[clsPOSDBConstants.ItemVendor_Fld_SaleEndDate] = Convert.ToDateTime(dr[clsPOSDBConstants.ItemVendor_Fld_SaleEndDate].ToString());
        //Till here Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
          this.AddRow(row);
      }
    }
		
		#endregion //Add and Get Methods 
   public override DataTable Clone() {
     ItemVendorTable cln = (ItemVendorTable)base.Clone();
     cln.InitVars();
     return cln;
}
      protected override DataTable CreateInstance() {
        return new ItemVendorTable();
      }

    internal void InitVars() {
		
	  this.colItemDetailID = this.Columns[clsPOSDBConstants.ItemVendor_Fld_ItemDetailID];
      this.colVendorItemID = this.Columns[clsPOSDBConstants.ItemVendor_Fld_VendorItemID];
      this.colItemID = this.Columns[clsPOSDBConstants.ItemVendor_Fld_ItemID];
      this.colVendorID = this.Columns[clsPOSDBConstants.ItemVendor_Fld_VendorID];
      this.colVendorCode = this.Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCode];
      this.colVendorName = this.Columns[clsPOSDBConstants.ItemVendor_Fld_VendorName];
      this.colVenorCostPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice];
      this.colLastOrderDate = this.Columns[clsPOSDBConstants.ItemVendor_Fld_LastOrderDate];

      //properties Added by Srt(Abhishek) 

      this.colAvgWholeSalePrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice];
      this.colCatPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_CatPrice];
      this.colContractPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_ContractPrice];
      
      this.colDealerAdjustPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice];
      this.colFederalUpperLimitPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice];
      this.colManufacturerSuggPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice];

      this.colNetItemPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_NetItemPrice];
      this.colProducerPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_ProducerPrice];
      this.colRetailPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_RetailPrice];

      //Added by SRT(Abhishek) Date : 24/09/2009 
      // INV - Invoice Billing Price
      // BCH - Base Charge         
      // PBQ	- Unit Price Beginning Quantity
      // RES	- Resale
      this.colInvoiceBillingPrcice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice];
      this.colBaseCharge = this.Columns[clsPOSDBConstants.ItemVendor_Fld_BaseCharge];
      this.colUnitPriceBegQuantity = this.Columns[clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity];
      this.colResalePrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_ResalePrice];
      //End Of Added by SRT(Abhishek) Date : 24/09/2009

      this.colIsDeleted = this.Columns[clsPOSDBConstants.ItemVendor_Fld_IsDeleted];  
      //End of properties Added by Srt(Abhishek) 

      this.colPckSize = this.Columns[clsPOSDBConstants.ItemVendor_Fld_PckSize];
      this.colPckQty = this.Columns[clsPOSDBConstants.ItemVendor_Fld_PckQty];
      this.colPckUnit = this.Columns[clsPOSDBConstants.ItemVendor_Fld_PckUnit];
        //Added by Atul Joshi on 22-10-2010
      this.colUnitCostPrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice];
      this.colHammacherDeptClass = this.Columns[clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass];//Till here Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
      this.colVendorSalePrice = this.Columns[clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice];//Till here Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

      this.colSaleStartDate = this.Columns[clsPOSDBConstants.ItemVendor_Fld_SaleStartDate];
      this.colSaleEndDate = this.Columns[clsPOSDBConstants.ItemVendor_Fld_SaleEndDate]; 
 
    }
     public System.Collections.IEnumerator GetEnumerator() {
       return this.Rows.GetEnumerator();
     }
      /// <summary>
      /// Initialise the DataTable
      /// </summary>
      /// <remarks>
      ///	 Add strongly typed rows with constraints
      /// </remarks>
      private void InitClass()
      {
		this.colItemDetailID = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_ItemDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
		this.Columns.Add(this.colItemDetailID);
		this.colItemDetailID.AllowDBNull = false;
		this.colItemDetailID.AutoIncrement = true;


      this.colVendorItemID = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_VendorItemID, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colVendorItemID);
      this.colVendorItemID.AllowDBNull = false;

      this.colItemID = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colItemID);
      this.colItemID.AllowDBNull = true;

      this.colVendorID = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_VendorID, typeof(System.Int32), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colVendorID);
      this.colVendorID.AllowDBNull = false;

      this.colVendorCode = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_VendorCode, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colVendorCode);
      this.colVendorCode.AllowDBNull = false;

      this.colVendorName = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_VendorName, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colVendorName);
      this.colVendorName.AllowDBNull = true;

      this.colVenorCostPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colVenorCostPrice);
      this.colVenorCostPrice.AllowDBNull = false;

      this.colLastOrderDate = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_LastOrderDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colLastOrderDate);
      this.colLastOrderDate.AllowDBNull = true;

      
      //Added By SRT(Abhishek) Date : 01/12/2009

      this.colAvgWholeSalePrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colAvgWholeSalePrice);
      this.colAvgWholeSalePrice.AllowDBNull = true;

      this.colCatPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_CatPrice,typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colCatPrice);
      this.colCatPrice.AllowDBNull = true;

      this.colContractPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_ContractPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colContractPrice);
      this.colContractPrice.AllowDBNull = true;

      this.colDealerAdjustPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colDealerAdjustPrice);
      this.colDealerAdjustPrice.AllowDBNull = true;

      this.colFederalUpperLimitPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice,typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colFederalUpperLimitPrice);
      this.colFederalUpperLimitPrice.AllowDBNull = true;

      this.colManufacturerSuggPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice,typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colManufacturerSuggPrice);
      this.colManufacturerSuggPrice.AllowDBNull = true;

      this.colNetItemPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_NetItemPrice,typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colNetItemPrice);
      this.colNetItemPrice.AllowDBNull = true;

      this.colProducerPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_ProducerPrice,typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colProducerPrice);
      this.colProducerPrice.AllowDBNull = true;

      this.colRetailPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_RetailPrice,typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colRetailPrice);
      this.colRetailPrice.AllowDBNull = true;

      this.colIsDeleted = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_IsDeleted, typeof(System.Boolean), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colIsDeleted);
      this.colIsDeleted.AllowDBNull = true;  
      //End Of Added By SRT(Abhishek) Date : 01/12/2009        

      //Added by SRT(Abhishke) Date : 24/09/2009
      // INV - Invoice Billing Price
      // BCH - Base Charge         
      // PBQ	- Unit Price Beginning Quantity
      // RES	- Resale

      this.colInvoiceBillingPrcice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colInvoiceBillingPrcice);
      this.colInvoiceBillingPrcice.AllowDBNull = true;

      this.colBaseCharge = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_BaseCharge, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colBaseCharge);
      this.colBaseCharge.AllowDBNull = true;

      this.colUnitPriceBegQuantity = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colUnitPriceBegQuantity);
      this.colUnitPriceBegQuantity.AllowDBNull = true;

      this.colResalePrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_ResalePrice,typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colResalePrice);
      this.colResalePrice.AllowDBNull = true; 
      //End Of Added by SRT(Abhishke) Date : 24/09/2009

      this.colPckSize = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_PckSize, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colPckSize);
      this.colPckSize.AllowDBNull = true;

      this.colPckQty = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_PckQty, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colPckQty);
      this.colPckQty.AllowDBNull = true;

      this.colPckUnit = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_PckUnit, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colPckUnit);
      this.colPckUnit.AllowDBNull = true;

      //Added by ATul Joshi on 22-10-2010
      this.colUnitCostPrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colUnitCostPrice);
      this.colUnitCostPrice.AllowDBNull = true;
      //Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
      this.colHammacherDeptClass = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colHammacherDeptClass);
      this.colHammacherDeptClass.AllowDBNull = true;

      this.colVendorSalePrice = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colVendorSalePrice);
      this.colVendorSalePrice.AllowDBNull = true; //Till here Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing

      this.colSaleStartDate = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_SaleStartDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colSaleStartDate);
      this.colVendorSalePrice.AllowDBNull = true;
      this.colSaleEndDate = new DataColumn(clsPOSDBConstants.ItemVendor_Fld_SaleEndDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colSaleEndDate);
      this.colSaleEndDate.AllowDBNull = true;
      this.PrimaryKey = new DataColumn[] {this.colItemDetailID};
   }

      public ItemVendorRow NewItemVendorRow() {
          return  (ItemVendorRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new ItemVendorRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(ItemVendorRow);
    }

	 #region Event Handlers

    public delegate void ItemVendorRowChangeEventHandler(object sender, ItemVendorRowChangeEvent e);

    public event ItemVendorRowChangeEventHandler ItemVendorRowChanged;	  
    public event ItemVendorRowChangeEventHandler ItemVendorRowChanging;	  
    public event ItemVendorRowChangeEventHandler ItemVendorRowDeleted;
    public event ItemVendorRowChangeEventHandler ItemVendorRowDeleting;

    protected override void OnRowChanged(DataRowChangeEventArgs e) {
      base.OnRowChanged(e);
      if (this.ItemVendorRowChanged != null) {
        this.ItemVendorRowChanged(this, new ItemVendorRowChangeEvent((ItemVendorRow)e.Row, e.Action));				}
    }

    protected override void OnRowChanging(DataRowChangeEventArgs e) {
      base.OnRowChanging(e);
      if (this.ItemVendorRowChanging != null) {
        this.ItemVendorRowChanging(this, new ItemVendorRowChangeEvent((ItemVendorRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleted(DataRowChangeEventArgs e) {
      base.OnRowDeleted(e);
      if (this.ItemVendorRowDeleted != null) {
        this.ItemVendorRowDeleted(this, new ItemVendorRowChangeEvent((ItemVendorRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleting(DataRowChangeEventArgs e) {
      base.OnRowDeleting(e);
      if (this.ItemVendorRowDeleting != null) {
        this.ItemVendorRowDeleting(this, new ItemVendorRowChangeEvent((ItemVendorRow)e.Row, e.Action));
      }
    }

		#endregion
}
}
