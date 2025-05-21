// ----------------------------------------------------------------
// ----------------------------------------------------------------

 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;

  public class ItemGroupPriceTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colGroupPriceID;
     private DataColumn colItemID;
     private DataColumn colQty;
     private DataColumn colCost;
     private DataColumn colSalePrice;
      private DataColumn colStartDate;
      private DataColumn colEndDate;

     #region Constructors 
     internal ItemGroupPriceTable() : base(clsPOSDBConstants.ItemGroupPrice_tbl) { this.InitClass(); }
     internal ItemGroupPriceTable(DataTable table) : base(table.TableName) {}
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

    public ItemGroupPriceRow this[int index] {
      get {
        return ((ItemGroupPriceRow)(this.Rows[index]));
      }
    }

    /// <summary>
    /// Public Property DataColumn GroupPriceID
    /// </summary>	
    public DataColumn GroupPriceID {
      get {
        return this.colGroupPriceID;
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
    /// Public Property DataColumn Qty
    /// </summary>	
    public DataColumn Qty {
      get {
        return this.colQty;
      }
    }

    /// <summary>
    /// Public Property DataColumn Cost
    /// </summary>	
    public DataColumn Cost {
      get {
        return this.colCost;
      }
    }

    /// <summary>
    /// Public Property DataColumn SalePrice
    /// </summary>	
    public DataColumn SalePrice {
      get {
        return this.colSalePrice;
      }
    }

      public DataColumn StartDate
      {
          get
          {
              return this.colStartDate;
          }
      }

      public DataColumn EndDate
      {
          get
          {
              return this.colEndDate;
          }
      }
		#endregion //Properties
      #region Add and Get Methods 

      public void AddRow(ItemGroupPriceRow row) {
        AddRow(row, false);
      }

    public void AddRow(ItemGroupPriceRow row, bool preserveChanges) {
      if(this.GetRowByID(row.GroupPriceID) == null) {
        this.Rows.Add(row);
        if(!preserveChanges) {
          row.AcceptChanges();
        }
      }
    }

    public ItemGroupPriceRow AddRow(System.Int32 GroupPriceID,System.String ItemID,System.Int32 Qty,System.Decimal Cost,System.Decimal SalePrice,DateTime? StartDate,DateTime? EndDate) {
      ItemGroupPriceRow row = (ItemGroupPriceRow)this.NewRow();
      row.ItemArray = new object[] {GroupPriceID,ItemID,Qty,Cost,SalePrice,StartDate,EndDate};
      this.Rows.Add(row);
      return row;
    }

    public ItemGroupPriceRow GetRowByID(System.Int32 GroupPriceID) {
      return (ItemGroupPriceRow)this.Rows.Find(new object[] {GroupPriceID});
    }

      public void MergeTable(DataTable dt)
      {
          //add any rows in the DataTable 
          ItemGroupPriceRow row;
          foreach (DataRow dr in dt.Rows)
          {
              row = (ItemGroupPriceRow)this.NewRow();

              if (dr[clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID] == DBNull.Value)
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID] = DBNull.Value;
              else
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID] = Convert.ToInt32(dr[clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID].ToString());

              if (dr[clsPOSDBConstants.ItemGroupPrice_Fld_ItemID] == DBNull.Value)
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_ItemID] = DBNull.Value;
              else
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_ItemID] = Convert.ToString(dr[clsPOSDBConstants.ItemGroupPrice_Fld_ItemID].ToString());

              if (dr[clsPOSDBConstants.ItemGroupPrice_Fld_Qty] == DBNull.Value)
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_Qty] = DBNull.Value;
              else
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_Qty] = Convert.ToInt32(dr[clsPOSDBConstants.ItemGroupPrice_Fld_Qty].ToString());

              if (dr[clsPOSDBConstants.ItemGroupPrice_Fld_Cost] == DBNull.Value)
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_Cost] = DBNull.Value;
              else
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_Cost] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemGroupPrice_Fld_Cost].ToString());

              if (dr[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice] == DBNull.Value)
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice] = DBNull.Value;
              else
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice].ToString());

              if (dr[clsPOSDBConstants.ItemGroupPrice_Fld_StartDate] == DBNull.Value)
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_StartDate] = DBNull.Value;
              else
              {
                  DateTime parsedDate;
                  if (DateTime.TryParse(dr[clsPOSDBConstants.ItemGroupPrice_Fld_StartDate].ToString(), out parsedDate) == false)
                  {
                      row[clsPOSDBConstants.ItemGroupPrice_Fld_StartDate] = DBNull.Value;
                  }
                  else
                  {
                      row[clsPOSDBConstants.ItemGroupPrice_Fld_StartDate] = parsedDate;
                  }
              }

              if (dr[clsPOSDBConstants.ItemGroupPrice_Fld_EndDate] == DBNull.Value)
                  row[clsPOSDBConstants.ItemGroupPrice_Fld_EndDate] = DBNull.Value;
              else
              {
                  DateTime parsedDate;
                  if (DateTime.TryParse(dr[clsPOSDBConstants.ItemGroupPrice_Fld_EndDate].ToString(), out parsedDate) == false)
                  {
                      row[clsPOSDBConstants.ItemGroupPrice_Fld_EndDate] = DBNull.Value;
                  }
                  else
                  {
                      row[clsPOSDBConstants.ItemGroupPrice_Fld_EndDate] = parsedDate;
                  }
              }
              this.AddRow(row);
          }
      }
		
		#endregion //Add and Get Methods 
   public override DataTable Clone() {
     ItemGroupPriceTable cln = (ItemGroupPriceTable)base.Clone();
     cln.InitVars();
     return cln;
}
      protected override DataTable CreateInstance() {
        return new ItemGroupPriceTable();
      }

    internal void InitVars() {
      this.colGroupPriceID = this.Columns[clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID];
      this.colItemID = this.Columns[clsPOSDBConstants.ItemGroupPrice_Fld_ItemID];
      this.colQty = this.Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Qty];
      this.colCost = this.Columns[clsPOSDBConstants.ItemGroupPrice_Fld_Cost];
      this.colSalePrice = this.Columns[clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice];
      this.colStartDate = this.Columns[clsPOSDBConstants.ItemGroupPrice_Fld_StartDate];
      this.colEndDate = this.Columns[clsPOSDBConstants.ItemGroupPrice_Fld_EndDate];
    }
     public System.Collections.IEnumerator GetEnumerator() {
       return this.Rows.GetEnumerator();
     }
      private void InitClass() {
      this.colGroupPriceID = new DataColumn(clsPOSDBConstants.ItemGroupPrice_Fld_GroupPriceID, typeof(System.Int32), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colGroupPriceID);
      this.colGroupPriceID.AllowDBNull = true;
	  this.colGroupPriceID.AutoIncrement = true;

      this.colItemID = new DataColumn(clsPOSDBConstants.ItemGroupPrice_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colItemID);
      this.colItemID.AllowDBNull = true;

      this.colQty = new DataColumn(clsPOSDBConstants.ItemGroupPrice_Fld_Qty, typeof(System.Int32), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colQty);
      this.colQty.AllowDBNull = false;

      this.colCost = new DataColumn(clsPOSDBConstants.ItemGroupPrice_Fld_Cost, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colCost);
      this.colCost.AllowDBNull = false;

      this.colSalePrice = new DataColumn(clsPOSDBConstants.ItemGroupPrice_Fld_SalePrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colSalePrice);
      this.colSalePrice.AllowDBNull = false;

      this.colStartDate= new DataColumn(clsPOSDBConstants.ItemGroupPrice_Fld_StartDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colStartDate);
      this.colStartDate.AllowDBNull = true;

      this.colEndDate= new DataColumn(clsPOSDBConstants.ItemGroupPrice_Fld_EndDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colEndDate);
      this.colEndDate.AllowDBNull = true;

       this.PrimaryKey = new DataColumn[] {this.GroupPriceID};
     }
      public ItemGroupPriceRow NewItemGroupPriceRow() {
        return (ItemGroupPriceRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new ItemGroupPriceRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(ItemGroupPriceRow);
    }

	 #region Event Handlers

    public delegate void ItemGroupPriceRowChangeEventHandler(object sender, ItemGroupPriceRowChangeEvent e);

    public event ItemGroupPriceRowChangeEventHandler ItemGroupPriceRowChanged;	  
    public event ItemGroupPriceRowChangeEventHandler ItemGroupPriceRowChanging;	  
    public event ItemGroupPriceRowChangeEventHandler ItemGroupPriceRowDeleted;
    public event ItemGroupPriceRowChangeEventHandler ItemGroupPriceRowDeleting;

    protected override void OnRowChanged(DataRowChangeEventArgs e) {
      base.OnRowChanged(e);
      if (this.ItemGroupPriceRowChanged != null) {
        this.ItemGroupPriceRowChanged(this, new ItemGroupPriceRowChangeEvent((ItemGroupPriceRow)e.Row, e.Action));				}
    }

    protected override void OnRowChanging(DataRowChangeEventArgs e) {
      base.OnRowChanging(e);
      if (this.ItemGroupPriceRowChanging != null) {
        this.ItemGroupPriceRowChanging(this, new ItemGroupPriceRowChangeEvent((ItemGroupPriceRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleted(DataRowChangeEventArgs e) {
      base.OnRowDeleted(e);
      if (this.ItemGroupPriceRowDeleted != null) {
        this.ItemGroupPriceRowDeleted(this, new ItemGroupPriceRowChangeEvent((ItemGroupPriceRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleting(DataRowChangeEventArgs e) {
      base.OnRowDeleting(e);
      if (this.ItemGroupPriceRowDeleting != null) {
        this.ItemGroupPriceRowDeleting(this, new ItemGroupPriceRowChangeEvent((ItemGroupPriceRow)e.Row, e.Action));
      }
    }

		#endregion
}
}
