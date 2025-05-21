
 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;

  public class InvTransTypeTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colID;
     private DataColumn colTypeName;
     private DataColumn colTransType;
	 private DataColumn colUserID;

     #region Constants
     #endregion
     #region Constructors 
     internal InvTransTypeTable() : base(clsPOSDBConstants.InvTransType_tbl) { this.InitClass(); }
     internal InvTransTypeTable(DataTable table) : base(table.TableName) {}
     #endregion
      #region Properties
      public int Count {
        get {
          return this.Rows.Count;
        }
      }

    public InvTransTypeRow this[int index] {
      get {
        return ((InvTransTypeRow)(this.Rows[index]));
      }
    }

    public DataColumn ID {
      get {
        return this.colID;
      }
    }

    public DataColumn TypeName 
	{
      get {
        return this.colTypeName;
      }
    }

	  public DataColumn UserID 
	  {
		  get 
		  {
			  return this.colUserID;
		  }
	  }

    public DataColumn TransType 
	{
      get {
        return this.colTransType;
      }
    }

	#endregion //Properties
      #region Add and Get Methods 

      public  void AddRow(InvTransTypeRow row) {
        AddRow(row, false);
      }

    public  void AddRow(InvTransTypeRow row, bool preserveChanges) {
        this.Rows.Add(row);
		if(!preserveChanges) 
		{
			row.AcceptChanges();
		}
    }

    public  InvTransTypeRow AddRow(System.Int32 ID,System.String TypeName,System.Int16 TransType,System.String UserID) {
      InvTransTypeRow row = (InvTransTypeRow)this.NewRow();
      row.ItemArray = new object[] {ID,TypeName,TransType,UserID};
      this.Rows.Add(row);
      return row;
    }

    public InvTransTypeRow GetRowByID(System.Int32 ID) {
      return (InvTransTypeRow)this.Rows.Find(new object[] {ID});
    }

    public  void MergeTable(DataTable dt) { 
      
      InvTransTypeRow row;
      foreach(DataRow dr in dt.Rows) {
        row = (InvTransTypeRow)this.NewRow();

       if (dr[clsPOSDBConstants.InvTransType_Fld_ID] == DBNull.Value) 
         row[clsPOSDBConstants.InvTransType_Fld_ID] = DBNull.Value;
       else
		   row[clsPOSDBConstants.InvTransType_Fld_ID] = Convert.ToInt32((dr[clsPOSDBConstants.InvTransType_Fld_ID].ToString()=="")?"0":dr[clsPOSDBConstants.InvTransType_Fld_ID].ToString());

		if (dr[clsPOSDBConstants.InvTransType_Fld_TypeName] == DBNull.Value) 
			row[clsPOSDBConstants.InvTransType_Fld_TypeName] = DBNull.Value;
		else
			row[clsPOSDBConstants.InvTransType_Fld_TypeName] = Convert.ToString(dr[clsPOSDBConstants.InvTransType_Fld_TypeName].ToString());

		if (dr[clsPOSDBConstants.InvTransType_Fld_TransType] == DBNull.Value) 
			row[clsPOSDBConstants.InvTransType_Fld_TransType] = DBNull.Value;
		else
			row[clsPOSDBConstants.InvTransType_Fld_TransType] = Convert.ToInt16((dr[clsPOSDBConstants.InvTransType_Fld_TransType].ToString()=="")? "0":dr[clsPOSDBConstants.InvTransType_Fld_TransType].ToString());

       if (dr[clsPOSDBConstants.InvTransType_Fld_UserID] == DBNull.Value) 
         row[clsPOSDBConstants.InvTransType_Fld_UserID] = DBNull.Value;
       else
         row[clsPOSDBConstants.InvTransType_Fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.InvTransType_Fld_UserID].ToString());


        this.AddRow(row);
      }
    }
		
		#endregion 
   public override DataTable Clone() {
     InvTransTypeTable cln = (InvTransTypeTable)base.Clone();
     cln.InitVars();
     return cln;
}
      protected override DataTable CreateInstance() {
        return new InvTransTypeTable();
      }

    internal void InitVars() {
      this.colID = this.Columns[clsPOSDBConstants.InvTransType_Fld_ID];
      this.colTypeName = this.Columns[clsPOSDBConstants.InvTransType_Fld_TypeName];
      this.colTransType = this.Columns[clsPOSDBConstants.InvTransType_Fld_TransType];
      this.colUserID = this.Columns[clsPOSDBConstants.fld_UserID];
    }
     public System.Collections.IEnumerator GetEnumerator() {
       return this.Rows.GetEnumerator();
     }

	  private void InitClass() {
      this.colID = new DataColumn(clsPOSDBConstants.InvTransType_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colID);
      //this.colID.AllowDBNull = true;

      this.colTypeName = new DataColumn(clsPOSDBConstants.InvTransType_Fld_TypeName,typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colTypeName);
      //this.colTypeName.AllowDBNull = false;

      this.colTransType = new DataColumn(clsPOSDBConstants.InvTransType_Fld_TransType, typeof(System.Int16), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colTransType);
      //this.colTransType.AllowDBNull = false;

      this.colUserID = new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
      this.Columns.Add(this.colUserID);
      //this.colUserID.AllowDBNull = true;

       this.PrimaryKey = new DataColumn[] {this.ID};
     }
      public  InvTransTypeRow NewInvTransTypeRow() {
        return (InvTransTypeRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new InvTransTypeRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(InvTransTypeRow);
    }
}
}
