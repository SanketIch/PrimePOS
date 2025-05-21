 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;

  public class TaxCodesTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colTaxID;
	 private DataColumn colTaxCode;
     private DataColumn colDescription;
     private DataColumn colAmount;
     private DataColumn colUserID;
     private DataColumn colTaxType;
     private DataColumn colActive;//2974

        #region Constants
        private const String _TableName = "TAXCODES";
     #endregion
     #region Constructors 
     internal TaxCodesTable() : base(_TableName) { this.InitClass(); }
     internal TaxCodesTable(DataTable table) : base(table.TableName) {}
     #endregion
      #region Properties
      public int Count {
        get {
          return this.Rows.Count;
        }
      }

    public TaxCodesRow this[int index] {
      get {
        return ((TaxCodesRow)(this.Rows[index]));
      }
    }

    public DataColumn TaxID {
      get {
        return this.colTaxID;
      }
    }

	public DataColumn TaxCode
	{
		get 
		{
			return this.colTaxCode;
		}
	}

    public DataColumn Description 
	{
      get {
        return this.colDescription;
      }
    }

    public DataColumn Amount {
      get {
        return this.colAmount;
      }
    }

    public DataColumn UserID {
      get {
        return this.colUserID;
      }
    }

    public DataColumn TaxType
    {
        get
        {
            return this.colTaxType;
        }
    }
        public DataColumn Active//2974
        {
            get
            {
                return this.colActive;
            }
        }
        #endregion //Properties
        #region Add and Get Methods 

        public  void AddRow(TaxCodesRow row) {
        AddRow(row, false);
      }

    public  void AddRow(TaxCodesRow row, bool preserveChanges) {
      if(this.GetRowByID(row.TaxCode) == null) {
        this.Rows.Add(row);
        if(!preserveChanges) {
          row.AcceptChanges();
        }
      }
    }

    public  TaxCodesRow AddRow(System.Int32 TaxID,System.String TaxCode,System.String Description,System.Decimal Amount,System.String UserID, Int32 TaxType,bool Active) {//2974
      TaxCodesRow row = (TaxCodesRow)this.NewRow();
      row.ItemArray = new object[] {TaxID,TaxCode,Description,Amount,UserID, TaxType,Active};//2974
      this.Rows.Add(row);
      return row;
    }

    public TaxCodesRow GetRowByID(System.String TaxCode) {
      return (TaxCodesRow)this.Rows.Find(new object[] {TaxCode});
    }

    public  void MergeTable(DataTable dt) { 
      
      TaxCodesRow row;
      foreach(DataRow dr in dt.Rows) {
        row = (TaxCodesRow)this.NewRow();

       if (dr[0] == DBNull.Value) 
         row[0] = DBNull.Value;
       else
		   row[0] = Convert.ToInt32((dr[0].ToString()=="")?"0":dr[0].ToString());

		if (dr[1] == DBNull.Value) 
			row[1] = DBNull.Value;
		else
			row[1] = Convert.ToString(dr[1].ToString());

       if (dr[2] == DBNull.Value) 
         row[2] = DBNull.Value;
       else
         row[2] = Convert.ToString(dr[2].ToString());

       if (dr[3] == DBNull.Value) 
         row[3] = DBNull.Value;
       else
		   row[3] = Convert.ToDecimal((dr[3].ToString()=="")? "0":dr[3].ToString());

       if (dr[4] == DBNull.Value)
           row[4] = DBNull.Value;
       else
           row[4] = Convert.ToString((dr[4].ToString() == "") ? "0" : dr[4].ToString());

       if (dr[5] == DBNull.Value)
           row[5] = DBNull.Value;
       else
           row[5] = Convert.ToInt32((dr[5].ToString() == "") ? "0" : dr[5].ToString());

                row[6] = Convert.ToBoolean(dr[6]);//2974

        this.AddRow(row);
      }
    }
		
		#endregion 
   public override DataTable Clone() {
     TaxCodesTable cln = (TaxCodesTable)base.Clone();
     cln.InitVars();
     return cln;
}
      protected override DataTable CreateInstance() {
        return new TaxCodesTable();
      }

    internal void InitVars() {
      this.colTaxID = this.Columns[clsPOSDBConstants.TaxCodes_Fld_TaxID];
	  this.colTaxCode = this.Columns[clsPOSDBConstants.TaxCodes_Fld_TaxCode];
      this.colDescription = this.Columns[clsPOSDBConstants.TaxCodes_Fld_Description];
      this.colAmount = this.Columns[clsPOSDBConstants.TaxCodes_Fld_Amount];
      this.colUserID = this.Columns[clsPOSDBConstants.fld_UserID];
      this.colTaxType = this.Columns[clsPOSDBConstants.TaxCodes_Fld_TaxType];
            this.colActive = this.Columns[clsPOSDBConstants.TaxCodes_Fld_Active];//2974
        }
     public System.Collections.IEnumerator GetEnumerator() {
       return this.Rows.GetEnumerator();
     }

     private void InitClass()
     {
         this.colTaxID = new DataColumn(clsPOSDBConstants.TaxCodes_Fld_TaxID, typeof(System.Int32), null, System.Data.MappingType.Element);
         this.Columns.Add(this.colTaxID);
         //this.colTaxID.AllowDBNull = true;

         this.colTaxCode = new DataColumn(clsPOSDBConstants.TaxCodes_Fld_TaxCode, typeof(System.String), null, System.Data.MappingType.Element);
         this.Columns.Add(this.colTaxCode);
         // this.colTaxCode.AllowDBNull = false;

         this.colDescription = new DataColumn(clsPOSDBConstants.TaxCodes_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
         this.Columns.Add(this.colDescription);
         //this.colDescription.AllowDBNull = false;

         this.colAmount = new DataColumn(clsPOSDBConstants.TaxCodes_Fld_Amount, typeof(System.Decimal), null, System.Data.MappingType.Element);
         this.Columns.Add(this.colAmount);
         //this.colAmount.AllowDBNull = false;

         this.colUserID = new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
         this.Columns.Add(this.colUserID);
         //this.colUserID.AllowDBNull = true;

         this.colTaxType = new DataColumn(clsPOSDBConstants.TaxCodes_Fld_TaxType, typeof(System.Int32), null, System.Data.MappingType.Element);
         this.Columns.Add(this.colTaxType);
         this.colTaxType.AllowDBNull = true;

            this.colActive = new DataColumn(clsPOSDBConstants.TaxCodes_Fld_Active, typeof(System.Boolean), null, System.Data.MappingType.Element);//2974
            this.Columns.Add(this.colActive);
            //this.colTaxType.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.TaxCode };
     }

      public  TaxCodesRow NewTaxCodesRow() {
        return (TaxCodesRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new TaxCodesRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(TaxCodesRow);
    }
}
}
