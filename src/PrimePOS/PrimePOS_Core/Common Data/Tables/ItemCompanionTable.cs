// ----------------------------------------------------------------
// ----------------------------------------------------------------

 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;

  public class ItemCompanionTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colCompanionItemID;
     private DataColumn colItemID;
	 private DataColumn colItemDescription;
     private DataColumn colAmount;
     private DataColumn colPercentage;

     #region Constructors 
     internal ItemCompanionTable() : base(clsPOSDBConstants.ItemCompanion_tbl) { this.InitClass(); }
     internal ItemCompanionTable(DataTable table) : base(table.TableName) {}
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

    public ItemCompanionRow this[int index] {
      get {
        return ((ItemCompanionRow)(this.Rows[index]));
      }
    }

    /// <summary>
    /// Public Property DataColumn CompanionItemID
    /// </summary>	
    public DataColumn CompanionItemID {
      get {
        return this.colCompanionItemID;
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

	  public DataColumn ItemDescription 
	  {
		  get 
		  {
			  return this.colItemDescription;
		  }
	  }

    /// <summary>
    /// Public Property DataColumn Amount
    /// </summary>	
    public DataColumn Amount {
      get {
        return this.colAmount;
      }
    }

    /// <summary>
    /// Public Property DataColumn Percentage
    /// </summary>	
    public DataColumn Percentage {
      get {
        return this.colPercentage;
      }
    }

		#endregion //Properties
      #region Add and Get Methods 

      public void AddRow(ItemCompanionRow row) {
        AddRow(row, false);
      }

    public virtual void AddRow(ItemCompanionRow row, bool preserveChanges) {
      if(this.GetRowByID(row.CompanionItemID) == null) {
        this.Rows.Add(row);
        if(!preserveChanges) {
          row.AcceptChanges();
        }
      }
    }

    public ItemCompanionRow AddRow(System.Int32 CompanionItemID,System.String ItemID ,System.String ItemDescription , System.Decimal Amount,System.Decimal Percentage) {
      ItemCompanionRow row = (ItemCompanionRow)this.NewRow();
      row.ItemArray = new object[] {CompanionItemID,ItemID, ItemDescription , Amount,Percentage};
      this.Rows.Add(row);
      return row;
    }

    public ItemCompanionRow GetRowByID(System.String CompanionItemID) {
      return (ItemCompanionRow)this.Rows.Find(new object[] {CompanionItemID});
    }

    public void MergeTable(DataTable dt) { 
      //add any rows in the DataTable 
      ItemCompanionRow row;
      foreach(DataRow dr in dt.Rows) {
        row = (ItemCompanionRow)this.NewRow();

       if (dr[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID] == DBNull.Value) 
         row[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID] = Convert.ToString(dr[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID].ToString());

       if (dr[clsPOSDBConstants.ItemCompanion_Fld_Amount] == DBNull.Value) 
         row[clsPOSDBConstants.ItemCompanion_Fld_Amount] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemCompanion_Fld_Amount] = Convert.ToString(dr[clsPOSDBConstants.ItemCompanion_Fld_Amount].ToString());


       if (dr[clsPOSDBConstants.ItemCompanion_Fld_ItemID] == DBNull.Value) 
         row[clsPOSDBConstants.ItemCompanion_Fld_ItemID] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemCompanion_Fld_ItemID] = Convert.ToString(dr[clsPOSDBConstants.ItemCompanion_Fld_ItemID].ToString());

		  if (dr[clsPOSDBConstants.ItemCompanion_Fld_ItemDescription] == DBNull.Value) 
			  row[clsPOSDBConstants.ItemCompanion_Fld_ItemDescription] = DBNull.Value;
		  else
			  row[clsPOSDBConstants.ItemCompanion_Fld_ItemDescription] = Convert.ToString(dr[clsPOSDBConstants.ItemCompanion_Fld_ItemDescription].ToString());

       if (dr[clsPOSDBConstants.ItemCompanion_Fld_Percentage] == DBNull.Value) 
         row[clsPOSDBConstants.ItemCompanion_Fld_Percentage] = DBNull.Value;
       else
         row[clsPOSDBConstants.ItemCompanion_Fld_Percentage] = Convert.ToDecimal(dr[clsPOSDBConstants.ItemCompanion_Fld_Percentage].ToString());


        this.AddRow(row);
      }
    }
		
		#endregion //Add and Get Methods 
   public override DataTable Clone() {
     ItemCompanionTable cln = (ItemCompanionTable)base.Clone();
     cln.InitVars();
     return cln;
}
      protected override DataTable CreateInstance() {
        return new ItemCompanionTable();
      }

    internal void InitVars() {
      this.colCompanionItemID = this.Columns[clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID];
      this.colItemID = this.Columns[clsPOSDBConstants.ItemCompanion_Fld_ItemID];
	  this.colItemDescription = this.Columns[clsPOSDBConstants.ItemCompanion_Fld_ItemDescription];
      this.colAmount = this.Columns[clsPOSDBConstants.ItemCompanion_Fld_Amount];
      this.colPercentage = this.Columns[clsPOSDBConstants.ItemCompanion_Fld_Percentage];
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
      private void InitClass() {
		  
		  
      this.colCompanionItemID = new DataColumn(clsPOSDBConstants.ItemCompanion_Fld_CompanionItemID, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colCompanionItemID);
      this.colCompanionItemID.AllowDBNull = false;

      this.colItemID = new DataColumn(clsPOSDBConstants.ItemCompanion_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colItemID);
      this.colItemID.AllowDBNull = true;

	  this.colItemDescription = new DataColumn(clsPOSDBConstants.ItemCompanion_Fld_ItemDescription, typeof(System.String), null, System.Data.MappingType.Element);
	  this.Columns.Add(this.colItemDescription);
	  this.colItemDescription.AllowDBNull = false;

      this.colAmount = new DataColumn(clsPOSDBConstants.ItemCompanion_Fld_Amount, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colAmount);
      this.colAmount.AllowDBNull = false;

      this.colPercentage = new DataColumn(clsPOSDBConstants.ItemCompanion_Fld_Percentage, typeof(System.Decimal), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colPercentage);
      this.colPercentage.AllowDBNull = true;

       this.PrimaryKey = new DataColumn[] {this.CompanionItemID};
     }
      public ItemCompanionRow NewItemCompanionRow() {
        return (ItemCompanionRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new ItemCompanionRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(ItemCompanionRow);
    }

	 #region Event Handlers

    public delegate void ItemCompanionRowChangeEventHandler(object sender, ItemCompanionRowChangeEvent e);

    public event ItemCompanionRowChangeEventHandler ItemCompanionRowChanged;	  
    public event ItemCompanionRowChangeEventHandler ItemCompanionRowChanging;	  
    public event ItemCompanionRowChangeEventHandler ItemCompanionRowDeleted;
    public event ItemCompanionRowChangeEventHandler ItemCompanionRowDeleting;

    protected override void OnRowChanged(DataRowChangeEventArgs e) {
      base.OnRowChanged(e);
      if (this.ItemCompanionRowChanged != null) {
        this.ItemCompanionRowChanged(this, new ItemCompanionRowChangeEvent((ItemCompanionRow)e.Row, e.Action));				}
    }

    protected override void OnRowChanging(DataRowChangeEventArgs e) {
      base.OnRowChanging(e);
      if (this.ItemCompanionRowChanging != null) {
        this.ItemCompanionRowChanging(this, new ItemCompanionRowChangeEvent((ItemCompanionRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleted(DataRowChangeEventArgs e) {
      base.OnRowDeleted(e);
      if (this.ItemCompanionRowDeleted != null) {
        this.ItemCompanionRowDeleted(this, new ItemCompanionRowChangeEvent((ItemCompanionRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleting(DataRowChangeEventArgs e) {
      base.OnRowDeleting(e);
      if (this.ItemCompanionRowDeleting != null) {
        this.ItemCompanionRowDeleting(this, new ItemCompanionRowChangeEvent((ItemCompanionRow)e.Row, e.Action));
      }
    }

		#endregion
}
}
