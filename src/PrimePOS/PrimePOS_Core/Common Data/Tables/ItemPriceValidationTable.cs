// ----------------------------------------------------------------
// ----------------------------------------------------------------

 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;

  public class ItemPriceValidationTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colID;
     private DataColumn colItemID;
      private DataColumn colDeptID;
     private DataColumn colAllowNegative;
      private DataColumn colMinSellingAmount;
      private DataColumn colMinSellingPercentage;
      private DataColumn colIsActive;
      private DataColumn colMinCostPercentage;

     
     #region Constructors 
     internal ItemPriceValidationTable() : base(clsPOSDBConstants.ItemPriceValidation_tbl) { this.InitClass(); }
     internal ItemPriceValidationTable(DataTable table) : base(table.TableName) {}
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

    public ItemPriceValidationRow this[int index] {
      get {
        return ((ItemPriceValidationRow)(this.Rows[index]));
      }
    }

      public DataColumn ID
      {
          get
          {
              return this.colID;
          }
      }
    /// <summary>
    /// Public Property DataColumn DeptID
    /// </summary>	
    public DataColumn DeptID {
      get {
        return this.colDeptID;
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

	public DataColumn AllowNegative 
	  {
		  get 
		  {
			  return this.colAllowNegative;
		  }
	  }

    /// <summary>
    /// Public Property DataColumn MinSellingAmount
    /// </summary>	
    public DataColumn MinSellingAmount {
      get {
        return this.colMinSellingAmount;
      }
    }

    /// <summary>
    /// Public Property DataColumn MinSellingPercentage
    /// </summary>	
    public DataColumn MinSellingPercentage {
      get {
        return this.colMinSellingPercentage;
      }
    }

      public DataColumn IsActive
      {
          get
          {
              return this.colIsActive;
          }
      }

      public DataColumn MinCostPercentage
      {
          get
          {
              return this.colMinCostPercentage;
          }
      }
      
		#endregion //Properties

#region Add and Get Methods 

    public void AddRow(ItemPriceValidationRow row) {
        AddRow(row, false);
      }

    public virtual void AddRow(ItemPriceValidationRow row, bool preserveChanges) {
      if(this.GetRowByID(row.ID) == null) {
        this.Rows.Add(row);
        if(!preserveChanges) {
          row.AcceptChanges();
        }
      }
    }

    public ItemPriceValidationRow AddRow(System.Int32 iID,System.Int32 iDeptID,System.String sItemID ,bool bAllowNegative, System.Decimal dMinSellingAmount,System.Decimal dMinSellingPercentage,bool bIsActive,Decimal dMinCostPercentage) {
      ItemPriceValidationRow row = (ItemPriceValidationRow)this.NewRow();
      row.ItemArray = new object[] {iID,sItemID,iDeptID,bAllowNegative, dMinSellingAmount,dMinSellingPercentage,bIsActive,dMinCostPercentage};
      this.Rows.Add(row);
      return row;
    }

    public ItemPriceValidationRow GetRowByID(System.Int32 ID) {
      return (ItemPriceValidationRow)this.Rows.Find(new object[] {ID});
    }

    public void MergeTable(DataTable dt) { 
      //add any rows in the DataTable 
      ItemPriceValidationRow row;
      foreach (DataRow dr in dt.Rows)
      {
          row = (ItemPriceValidationRow)this.NewRow();

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_ID] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_ID] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_ID] = Convert.ToInt32(dr[clsPOSDBConstants.ItemPriceValidation_Fld_ID].ToString());

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_DeptID] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_DeptID] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_DeptID] = Convert.ToInt32(dr[clsPOSDBConstants.ItemPriceValidation_Fld_DeptID].ToString());

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount].ToString());

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_ItemID] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_ItemID] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_ItemID] = Convert.ToString(dr[clsPOSDBConstants.ItemPriceValidation_Fld_ItemID].ToString());

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage].ToString());

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage].ToString());

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative] = Convert.ToBoolean(dr[clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative].ToString());

          if (dr[clsPOSDBConstants.ItemPriceValidation_Fld_IsActive] == DBNull.Value)
              row[clsPOSDBConstants.ItemPriceValidation_Fld_IsActive] = DBNull.Value;
          else
              row[clsPOSDBConstants.ItemPriceValidation_Fld_IsActive] = Convert.ToBoolean(dr[clsPOSDBConstants.ItemPriceValidation_Fld_IsActive].ToString());

          this.AddRow(row);
      }
    }
		
		#endregion //Add and Get Methods 
   public override DataTable Clone() {
     ItemPriceValidationTable cln = (ItemPriceValidationTable)base.Clone();
     cln.InitVars();
     return cln;
}
      protected override DataTable CreateInstance() {
        return new ItemPriceValidationTable();
      }

    internal void InitVars() {
      this.colID = this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_ID];
      this.colItemID = this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_ItemID];
        this.colDeptID = this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_DeptID];
        this.colAllowNegative = this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative];
      this.colMinSellingAmount = this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount];
      this.colMinSellingPercentage = this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage];
      this.colIsActive= this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_IsActive];
      this.colMinCostPercentage = this.Columns[clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage];
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

          this.colID = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colID);
          this.colID.AllowDBNull = false;

          this.colItemID = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colItemID);
          this.colItemID.AllowDBNull = true;

          this.colDeptID = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_DeptID, typeof(System.Int32), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colDeptID);
          this.colDeptID.AllowDBNull = true;

          this.colAllowNegative = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_AllowNegative, typeof(System.Boolean), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colAllowNegative);
          this.colAllowNegative.AllowDBNull = false;

          this.colMinSellingAmount = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colMinSellingAmount);
          this.colMinSellingAmount.AllowDBNull = false;

          this.colMinSellingPercentage = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_MinSellingPercentage, typeof(System.Decimal), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colMinSellingPercentage);
          this.colMinSellingPercentage.AllowDBNull = true;

          this.colIsActive = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colIsActive);
          this.colIsActive.AllowDBNull = false;

          this.colMinCostPercentage = new DataColumn(clsPOSDBConstants.ItemPriceValidation_Fld_MinCostPercentage, typeof(System.Decimal), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colMinCostPercentage);
          this.colMinCostPercentage.AllowDBNull = false;

          this.PrimaryKey = new DataColumn[] { this.ID };
      }

      public ItemPriceValidationRow NewItemPriceValidationRow() {
        return (ItemPriceValidationRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new ItemPriceValidationRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(ItemPriceValidationRow);
    }

	 #region Event Handlers

    public delegate void ItemPriceValidationRowChangeEventHandler(object sender, ItemPriceValidationRowChangeEvent e);

    public event ItemPriceValidationRowChangeEventHandler ItemPriceValidationRowChanged;	  
    public event ItemPriceValidationRowChangeEventHandler ItemPriceValidationRowChanging;	  
    public event ItemPriceValidationRowChangeEventHandler ItemPriceValidationRowDeleted;
    public event ItemPriceValidationRowChangeEventHandler ItemPriceValidationRowDeleting;

    protected override void OnRowChanged(DataRowChangeEventArgs e) {
      base.OnRowChanged(e);
      if (this.ItemPriceValidationRowChanged != null) {
        this.ItemPriceValidationRowChanged(this, new ItemPriceValidationRowChangeEvent((ItemPriceValidationRow)e.Row, e.Action));				}
    }

    protected override void OnRowChanging(DataRowChangeEventArgs e) {
      base.OnRowChanging(e);
      if (this.ItemPriceValidationRowChanging != null) {
        this.ItemPriceValidationRowChanging(this, new ItemPriceValidationRowChangeEvent((ItemPriceValidationRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleted(DataRowChangeEventArgs e) {
      base.OnRowDeleted(e);
      if (this.ItemPriceValidationRowDeleted != null) {
        this.ItemPriceValidationRowDeleted(this, new ItemPriceValidationRowChangeEvent((ItemPriceValidationRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleting(DataRowChangeEventArgs e) {
      base.OnRowDeleting(e);
      if (this.ItemPriceValidationRowDeleting != null) {
        this.ItemPriceValidationRowDeleting(this, new ItemPriceValidationRowChangeEvent((ItemPriceValidationRow)e.Row, e.Action));
      }
    }

		#endregion
}
}
