// ----------------------------------------------------------------
// ----------------------------------------------------------------

 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;

  public class SubDepartmentTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colSubDepartmentID;
     private DataColumn colDepartmentID;
     private DataColumn colDescription;
     private DataColumn colIncludeOnSale;
     private DataColumn colPointsPerDollar;

     #region Constructors 
	  
     internal SubDepartmentTable() : base(clsPOSDBConstants.SubDepartment_tbl) { this.InitClass(); }
     internal SubDepartmentTable(DataTable table) : base(table.TableName) {}
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

    public SubDepartmentRow this[int index] {
      get {
        return ((SubDepartmentRow)(this.Rows[index]));
      }
    }

    /// <summary>
    /// Public Property DataColumn VendorDepartmentID
    /// </summary>	
    /// 

	public DataColumn SubDepartmentID 
	{
		get 
		{
			return this.colSubDepartmentID;
		}
	}

    
    /// <summary>
    /// Public Property DataColumn DepartmentID
    /// </summary>	
    public DataColumn DepartmentID {
      get {
        return this.colDepartmentID;
      }
    }

    /// <summary>
    /// Public Property DataColumn Description
    /// </summary>	
    public DataColumn Description {
      get {
        return this.colDescription;
      }
    }

    /// <summary>
    /// Added By shitaljit on 9/7/2013
    ///  Public Property DataColumn IncludeOnSale
    /// </summary>
    /// <param name="row"></param>
    /// 

    public DataColumn IncludeOnSale
    {
        get
        {
            return this.colIncludeOnSale;
        }
    }

    //Sprint-18 - 2041 27-Oct-2014 JY  Added
    public DataColumn PointsPerDollar
    {
        get
        {
            return this.colPointsPerDollar;
        }
    }
    #endregion //Properties

    #region Add and Get Methods

    public void AddRow(SubDepartmentRow row) {
        AddRow(row, false);
      }

    public void AddRow(SubDepartmentRow row, bool preserveChanges) {
      if(this.GetRowByID(row.SubDepartmentID) == null) {
        this.Rows.Add(row);
        if(!preserveChanges) {
          row.AcceptChanges();
        }
      }
    }

    public SubDepartmentRow AddRow(System.Int32 DepartmentID,System.String sDescription) {
      SubDepartmentRow row = (SubDepartmentRow)this.NewRow();
      //row.ItemArray = new object[] {VendorDepartmentID,DepartmentID,VendorID,Description,VendorName,VenorCostPrice,LastOrderDate};
      row.DepartmentID = DepartmentID;
      row.Description = sDescription;
        
      this.Rows.Add(row);
      return row;
    }

    public SubDepartmentRow GetRowByID(System.Int32 SubDepartmentID) {
      return (SubDepartmentRow)this.Rows.Find(new object[] {SubDepartmentID});
    }

    public void MergeTable(DataTable dt) { 
      //add any rows in the DataTable 
      SubDepartmentRow row;
      foreach(DataRow dr in dt.Rows) {
        row = (SubDepartmentRow)this.NewRow();

		if (dr[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID] == DBNull.Value) 
			row[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID] = DBNull.Value;
		else
			row[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID] = Convert.ToInt32(dr[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID].ToString());

       if (dr[clsPOSDBConstants.SubDepartment_Fld_DepartmentID] == DBNull.Value) 
         row[clsPOSDBConstants.SubDepartment_Fld_DepartmentID] = DBNull.Value;
       else
         row[clsPOSDBConstants.SubDepartment_Fld_DepartmentID] = Convert.ToInt32(dr[clsPOSDBConstants.SubDepartment_Fld_DepartmentID].ToString());

       if (dr[clsPOSDBConstants.SubDepartment_Fld_Description] == DBNull.Value) 
         row[clsPOSDBConstants.SubDepartment_Fld_Description] = DBNull.Value;
       else
         row[clsPOSDBConstants.SubDepartment_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.SubDepartment_Fld_Description].ToString());

       if (dr[clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale] == DBNull.Value)
           row[clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale] = DBNull.Value;
       else
           row[clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale] = Convert.ToBoolean(dr[clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale].ToString());

       //Sprint-18 - 2041 27-Oct-2014 JY  Added
       if (dr[clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar] == DBNull.Value)
           row[clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar] = DBNull.Value;
       else
           row[clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar] = Convert.ToInt32(dr[clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar].ToString());

          this.AddRow(row);
      }
    }
		
		#endregion //Add and Get Methods 

      public override DataTable Clone() {
     SubDepartmentTable cln = (SubDepartmentTable)base.Clone();
     cln.InitVars();
     return cln;
}
      
      protected override DataTable CreateInstance() {
        return new SubDepartmentTable();
      }

      internal void InitVars()
      {
          this.colSubDepartmentID = this.Columns[clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID];
          this.colDepartmentID = this.Columns[clsPOSDBConstants.SubDepartment_Fld_DepartmentID];
          this.colDescription = this.Columns[clsPOSDBConstants.SubDepartment_Fld_Description];
          this.colIncludeOnSale = this.Columns[clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale];//Added By shitaljit on 9/7/2013 for include/exclude sub dept from dept sale
          this.colPointsPerDollar = this.Columns[clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar];  //Sprint-18 - 2041 27-Oct-2014 JY  Added
      }

      public System.Collections.IEnumerator GetEnumerator()
      {
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
          this.colSubDepartmentID = new DataColumn(clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID, typeof(System.Int32), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colSubDepartmentID);
          this.colSubDepartmentID.AllowDBNull = false;
          this.colSubDepartmentID.AutoIncrement = true;

          this.colDepartmentID = new DataColumn(clsPOSDBConstants.SubDepartment_Fld_DepartmentID, typeof(System.String), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colDepartmentID);
          this.colDepartmentID.AllowDBNull = true;

          this.colDescription = new DataColumn(clsPOSDBConstants.SubDepartment_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colDescription);
          this.colDescription.AllowDBNull = false;

          //Added By shitaljit on 9/7/2013 for include/exclude sub dept from dept sale
          this.colIncludeOnSale = new DataColumn(clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale, typeof(System.Boolean), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colIncludeOnSale);
          this.colIncludeOnSale.AllowDBNull = true;

          //Sprint-18 - 2041 27-Oct-2014 JY Added
          this.colPointsPerDollar = new DataColumn(clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar, typeof(System.Int32), null, System.Data.MappingType.Element);
          this.Columns.Add(this.colPointsPerDollar);
          this.colPointsPerDollar.AllowDBNull = true;

          this.PrimaryKey = new DataColumn[] {this.colSubDepartmentID};
      }

      public SubDepartmentRow NewSubDepartmentRow() {
          return  (SubDepartmentRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new SubDepartmentRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(SubDepartmentRow);
    }

	 #region Event Handlers

    public delegate void SubDepartmentRowChangeEventHandler(object sender, SubDepartmentRowChangeEvent e);

    public event SubDepartmentRowChangeEventHandler SubDepartmentRowChanged;	  
    public event SubDepartmentRowChangeEventHandler SubDepartmentRowChanging;	  
    public event SubDepartmentRowChangeEventHandler SubDepartmentRowDeleted;
    public event SubDepartmentRowChangeEventHandler SubDepartmentRowDeleting;

    protected override void OnRowChanged(DataRowChangeEventArgs e) {
      base.OnRowChanged(e);
      if (this.SubDepartmentRowChanged != null) {
        this.SubDepartmentRowChanged(this, new SubDepartmentRowChangeEvent((SubDepartmentRow)e.Row, e.Action));				}
    }

    protected override void OnRowChanging(DataRowChangeEventArgs e) {
      base.OnRowChanging(e);
      if (this.SubDepartmentRowChanging != null) {
        this.SubDepartmentRowChanging(this, new SubDepartmentRowChangeEvent((SubDepartmentRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleted(DataRowChangeEventArgs e) {
      base.OnRowDeleted(e);
      if (this.SubDepartmentRowDeleted != null) {
        this.SubDepartmentRowDeleted(this, new SubDepartmentRowChangeEvent((SubDepartmentRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleting(DataRowChangeEventArgs e) {
      base.OnRowDeleting(e);
      if (this.SubDepartmentRowDeleting != null) {
        this.SubDepartmentRowDeleting(this, new SubDepartmentRowChangeEvent((SubDepartmentRow)e.Row, e.Action));
      }
    }

		#endregion
}
}
