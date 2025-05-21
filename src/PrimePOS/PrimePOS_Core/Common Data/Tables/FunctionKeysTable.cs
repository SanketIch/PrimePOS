// ----------------------------------------------------------------
// ----------------------------------------------------------------

 namespace POS_Core.CommonData.Tables {
 using System;
 using System.Data;
 using POS_Core.CommonData.Tables;
 using POS_Core.CommonData.Rows;


   public class FunctionKeysTable : DataTable, System.Collections.IEnumerable 
   {

     private DataColumn colFunKey;
	 private DataColumn colKeyId;
     private DataColumn colOperation;
     private DataColumn colButtonBackColor;
     private DataColumn colButtonForeColor;
     //Added By Shitaljit on 24 May 2013 for JIRA-932 Assign unlimited touch kyes
     private DataColumn colFunctionType;
     private DataColumn colParent;
     private DataColumn colMainPosition;
     private DataColumn colSubPosition;
     private DataColumn colDescription;
     #region Constructors 
     internal FunctionKeysTable() : base(clsPOSDBConstants.FunctionKeys_tbl) { this.InitClass(); }
     internal FunctionKeysTable(DataTable table) : base(table.TableName) {}
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

    public FunctionKeysRow this[int index] {
      get {
        return ((FunctionKeysRow)(this.Rows[index]));
      }
    }

    /// <summary>
    /// Public Property DataColumn FunKey
    /// </summary>	
    /// 

	   public DataColumn FunKey 
	   {
		   get 
		   {
			   return this.colFunKey;
		   }
	   }

    public DataColumn KeyId 
	{
      get {
        return this.colKeyId;
      }
    }

    /// <summary>
    /// Public Property DataColumn Operation
    /// </summary>	
    public DataColumn Operation {
      get {
        return this.colOperation;
      }
    }

	   public DataColumn ButtonBackColor
	   {
		   get 
		   {
			   return this.colButtonBackColor;
		   }
	   }

	   public DataColumn ButtonForeColor
	   {
		   get 
		   {
			   return this.colButtonForeColor;
		   }
	   }

       //Added By Shitaljit on 24 May 2013 for JIRA-932 Assign unlimited touch kyes
       public DataColumn FunctionType
       {
           get
           {
               return this.colFunctionType;
           }
       }

       public DataColumn Parent
       {
           get
           {
               return this.colParent;
           }
       }

       public DataColumn MainPosition
       {
           get
           {
               return this.colMainPosition;
           }
       }
       public DataColumn SubPosition
       {
           get
           {
               return this.colSubPosition;
           }
       }
       public DataColumn Description
       {
           get
           {
               return this.colDescription;
           }
       }
       //END
		#endregion //Properties
      #region Add and Get Methods 

      public  void AddRow(FunctionKeysRow row) {
        AddRow(row, false);
      }

    public  void AddRow(FunctionKeysRow row, bool preserveChanges) {
      if(this.GetRowByID(row.KeyId) == null) {
        this.Rows.Add(row);
        if(!preserveChanges) {
          row.AcceptChanges();
        }
      }
    }

    public FunctionKeysRow AddRow(System.Int32 KeyId, System.String FunKey, System.String Operation, System.String ButtonBackColor, System.String ButtonForeColor, System.String FunctionType, System.Int32 Parent, System.Int32 MainPosition, System.Int32 SubPosition)
    {
      FunctionKeysRow row = (FunctionKeysRow)this.NewRow();
      row.ItemArray = new object[] {KeyId, FunKey,Operation,ButtonBackColor,ButtonForeColor, colFunctionType,Parent,MainPosition,SubPosition};
      this.Rows.Add(row);
      return row;
    }

    public FunctionKeysRow GetRowByID(System.Int32 KeyId) {
      return (FunctionKeysRow)this.Rows.Find(new object[] {KeyId});
    }

    public void MergeTable(DataTable dt)
    {
        //add any rows in the DataTable 
        FunctionKeysRow row;
        foreach (DataRow dr in dt.Rows)
        {
            row = (FunctionKeysRow)this.NewRow();

            if (dr[clsPOSDBConstants.FunctionKeys_Fld_KeyId] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_KeyId] = 0;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_KeyId] = Convert.ToInt32(dr[clsPOSDBConstants.FunctionKeys_Fld_KeyId].ToString());

            if (dr[clsPOSDBConstants.FunctionKeys_Fld_FunKey] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_FunKey] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_FunKey] = Convert.ToString(dr[clsPOSDBConstants.FunctionKeys_Fld_FunKey].ToString());

            if (dr[clsPOSDBConstants.FunctionKeys_Fld_Operation] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_Operation] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_Operation] = Convert.ToString(dr[clsPOSDBConstants.FunctionKeys_Fld_Operation].ToString());

            if (dr[clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor] = Convert.ToString(dr[clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor].ToString());

            if (dr[clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor] = Convert.ToString(dr[clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor].ToString());

            //Added By Shitaljit on 24 May 2013 for JIRA-932 Assign unlimited touch kyes
            if (dr[clsPOSDBConstants.FunctionKeys_Fld_FunctionType] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_FunctionType] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_FunctionType] = Convert.ToString(dr[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].ToString());

            if (dr[clsPOSDBConstants.FunctionKeys_Fld_Parent] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_Parent] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_Parent] = Convert.ToInt32(dr[clsPOSDBConstants.FunctionKeys_Fld_Parent].ToString());
             
            if (dr[clsPOSDBConstants.FunctionKeys_Fld_MainPosition] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_MainPosition] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_MainPosition] = Convert.ToInt32(dr[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].ToString());

            if (dr[clsPOSDBConstants.FunctionKeys_Fld_SubPosition] == DBNull.Value)
                row[clsPOSDBConstants.FunctionKeys_Fld_SubPosition] = DBNull.Value;
            else
                row[clsPOSDBConstants.FunctionKeys_Fld_SubPosition] = Convert.ToInt32(dr[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].ToString());

            if (dr[clsPOSDBConstants.Item_Fld_Description] == DBNull.Value)
                row[clsPOSDBConstants.Item_Fld_Description] = DBNull.Value;
            else
                row[clsPOSDBConstants.Item_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_Description].ToString());
            //END
            
            this.AddRow(row);
        }
    }
		
		#endregion //Add and Get Methods 
   public override DataTable Clone() {
     FunctionKeysTable cln = (FunctionKeysTable)base.Clone();
     cln.InitVars();
     return cln;
}
      protected override DataTable CreateInstance() {
        return new FunctionKeysTable();
      }

    internal void InitVars() {
		this.colKeyId = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_KeyId];
		this.colFunKey = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey];
		this.colOperation = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation];
		this.colButtonBackColor = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor];
        this.colButtonForeColor = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor];
        //Added By Shitaljit on 24 May 2013 for JIRA-932 Assign unlimited touch kyes
		this.colFunctionType= this.Columns[clsPOSDBConstants.FunctionKeys_Fld_FunctionType];
        this.colParent = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_Parent];
        this.colMainPosition = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_MainPosition];
        this.colSubPosition = this.Columns[clsPOSDBConstants.FunctionKeys_Fld_SubPosition];
        this.colDescription = this.Columns[clsPOSDBConstants.Item_Fld_Description];
        //END
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
		   this.colKeyId = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_KeyId, typeof(System.Int32), null, System.Data.MappingType.Element);
		   this.Columns.Add(this.colKeyId);
		   this.colKeyId.AllowDBNull = false;
		   this.colKeyId.AutoIncrement = true;

		   this.colFunKey = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_FunKey, typeof(System.String), null, System.Data.MappingType.Element);
		   this.Columns.Add(this.colFunKey);
           this.colFunKey.AllowDBNull = true;

		   this.colOperation = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_Operation, typeof(System.String), null, System.Data.MappingType.Element);
		   this.Columns.Add(this.colOperation);
		   this.colOperation.AllowDBNull = true;

		   this.colButtonBackColor = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor, typeof(System.String), null, System.Data.MappingType.Element);
		   this.Columns.Add(this.colButtonBackColor);
		   this.colButtonBackColor.AllowDBNull = true;

		   this.colButtonForeColor = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor, typeof(System.String), null, System.Data.MappingType.Element);
		   this.Columns.Add(this.colButtonForeColor);
		   this.colButtonForeColor.AllowDBNull = true;
           //Added By Shitaljit on 24 May 2013 for JIRA-932 Assign unlimited touch kyes
           this.colFunctionType = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_FunctionType, typeof(System.String), null, System.Data.MappingType.Element);
           this.Columns.Add(this.colFunctionType);
           this.colFunctionType.AllowDBNull = true;

           this.colParent = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_Parent, typeof(System.Int32), null, System.Data.MappingType.Element);
           this.Columns.Add(this.colParent);
           this.colParent.AllowDBNull = true;

           this.colMainPosition = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_MainPosition, typeof(System.Int32), null, System.Data.MappingType.Element);
           this.Columns.Add(this.colMainPosition);
           this.colMainPosition.AllowDBNull = true;

           this.colSubPosition = new DataColumn(clsPOSDBConstants.FunctionKeys_Fld_SubPosition, typeof(System.Int32), null, System.Data.MappingType.Element);
           this.Columns.Add(this.colSubPosition);
           this.colSubPosition.AllowDBNull = true;

           this.colDescription = new DataColumn(clsPOSDBConstants.Item_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
           this.Columns.Add(this.colDescription);
           this.colDescription.AllowDBNull = true;
           //END
		   this.PrimaryKey = new DataColumn[] {this.KeyId};
	   }
      public  FunctionKeysRow NewFunctionKeysRow() {
        return (FunctionKeysRow)this.NewRow();
      }

    protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
      return new FunctionKeysRow(builder);
    }

    protected override System.Type GetRowType() {
      return typeof(FunctionKeysRow);
    }

	 #region Event Handlers

    public delegate void FunctionKeysRowChangeEventHandler(object sender, FunctionKeysRowChangeEvent e);

    public event FunctionKeysRowChangeEventHandler FunctionKeysRowChanged;	  
    public event FunctionKeysRowChangeEventHandler FunctionKeysRowChanging;	  
    public event FunctionKeysRowChangeEventHandler FunctionKeysRowDeleted;
    public event FunctionKeysRowChangeEventHandler FunctionKeysRowDeleting;

    protected override void OnRowChanged(DataRowChangeEventArgs e) {
      base.OnRowChanged(e);
      if (this.FunctionKeysRowChanged != null) {
        this.FunctionKeysRowChanged(this, new FunctionKeysRowChangeEvent((FunctionKeysRow)e.Row, e.Action));				}
    }

    protected override void OnRowChanging(DataRowChangeEventArgs e) {
      base.OnRowChanging(e);
      if (this.FunctionKeysRowChanging != null) {
        this.FunctionKeysRowChanging(this, new FunctionKeysRowChangeEvent((FunctionKeysRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleted(DataRowChangeEventArgs e) {
      base.OnRowDeleted(e);
      if (this.FunctionKeysRowDeleted != null) {
        this.FunctionKeysRowDeleted(this, new FunctionKeysRowChangeEvent((FunctionKeysRow)e.Row, e.Action));
      }
    }

    protected override void OnRowDeleting(DataRowChangeEventArgs e) {
      base.OnRowDeleting(e);
      if (this.FunctionKeysRowDeleting != null) {
        this.FunctionKeysRowDeleting(this, new FunctionKeysRowChangeEvent((FunctionKeysRow)e.Row, e.Action));
      }
    }

		#endregion
}
}
