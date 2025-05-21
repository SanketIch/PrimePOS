namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;

	//         This class is used to define the shape of CustomerNotesTable.
	public class CustomerNotesTable : DataTable
	{

		private DataColumn colId;
        private DataColumn colCustomerID;
		private DataColumn colNotes;
		private DataColumn colUserID;
		private DataColumn colLastUpdatedOn;
        private DataColumn colIsActive;
		
		#region Constructors 
		internal CustomerNotesTable() : base(clsPOSDBConstants.CustomerNotes_tbl) { this.InitClass(); }
		internal CustomerNotesTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		// Public Property for get all Rows in Table
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public CustomerNotesRow this[int index] 
		{
			get 
			{
				return ((CustomerNotesRow)(this.Rows[index]));
			}
		}

		// Public Property DataColumn CustomerNotescode
		public DataColumn Id 
		{
			get 
			{
				return this.colId;
			}
		}

        public DataColumn CustomerID
        {
            get
            {
                return this.colCustomerID;
            }
        }

		public DataColumn UserID
		{
			get 
			{
				return this.colUserID;
			}
		}

		public DataColumn LastUpdatedOn
		{
			get 
			{
				return this.colLastUpdatedOn;
			}
		}

		public DataColumn Notes
		{
			get 
			{
				return this.colNotes;
			}
		}

        public DataColumn IsActive
        {
            get
            {
                return this.colIsActive;
            }
        }
		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(CustomerNotesRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(CustomerNotesRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.ID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

        public CustomerNotesRow AddRow(System.Int32 iId, System.Int32 iCustomerID, System.String sNotes) 
		{
		
			CustomerNotesRow row = (CustomerNotesRow)this.NewRow();
			row.ItemArray = new object[] {iId,iCustomerID,sNotes,"",DateTime.Now,false};
			this.Rows.Add(row);
			return row;
		}

		public CustomerNotesRow GetRowByID(System.Int32 ID) 
		{
			return (CustomerNotesRow)this.Rows.Find(new object[] {ID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
			//add any rows in the DataTable 
			CustomerNotesRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (CustomerNotesRow)this.NewRow();

				if (dr[clsPOSDBConstants.CustomerNotes_Fld_ID] == DBNull.Value) 
					row[clsPOSDBConstants.CustomerNotes_Fld_ID] = 0;
				else
					row[clsPOSDBConstants.CustomerNotes_Fld_ID] = Convert.ToInt32(dr[clsPOSDBConstants.CustomerNotes_Fld_ID].ToString());

                if (dr[clsPOSDBConstants.CustomerNotes_Fld_CustomerID] == DBNull.Value)
                    row[clsPOSDBConstants.CustomerNotes_Fld_CustomerID] = 0;
                else
                    row[clsPOSDBConstants.CustomerNotes_Fld_CustomerID] = Convert.ToInt32(dr[clsPOSDBConstants.CustomerNotes_Fld_CustomerID].ToString());

                row[clsPOSDBConstants.CustomerNotes_Fld_Notes] = Convert.ToString(dr[clsPOSDBConstants.CustomerNotes_Fld_Notes].ToString());

				row[clsPOSDBConstants.fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.fld_UserID].ToString());
				
                if (dr[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn] == DBNull.Value) 
					row[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn] = DateTime.Now;
				else
					row[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn] = Convert.ToDateTime(dr[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn].ToString());

                if (dr[clsPOSDBConstants.CustomerNotes_Fld_IsActive] == DBNull.Value)
                    row[clsPOSDBConstants.CustomerNotes_Fld_IsActive] = false;
                else
                    row[clsPOSDBConstants.CustomerNotes_Fld_IsActive] = Convert.ToBoolean(dr[clsPOSDBConstants.CustomerNotes_Fld_IsActive].ToString());

				this.AddRow(row);
			}
		}
		
		#endregion //Add and Get Methods 

		protected override DataTable CreateInstance() 
		{
			return new CustomerNotesTable();
		}

		internal void InitVars() 
		{
			try 
			{
				this.colId = this.Columns[clsPOSDBConstants.CustomerNotes_Fld_ID];
                this.colCustomerID = this.Columns[clsPOSDBConstants.CustomerNotes_Fld_CustomerID];
                this.colNotes = this.Columns[clsPOSDBConstants.CustomerNotes_Fld_Notes];
				this.colUserID= this.Columns[clsPOSDBConstants.CustomerNotes_Fld_UserID];
				this.colLastUpdatedOn= this.Columns[clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn];
                this.colIsActive= this.Columns[clsPOSDBConstants.CustomerNotes_Fld_IsActive];
				
			}
			catch(Exception exp)
			{
				throw(exp);
			}
		}

		private void InitClass() 
		{
			this.colId = new DataColumn(clsPOSDBConstants.CustomerNotes_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colId);
			this.colId.AllowDBNull = false;

            this.colCustomerID = new DataColumn(clsPOSDBConstants.CustomerNotes_Fld_CustomerID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerID);
            this.colCustomerID.AllowDBNull = true;

			this.colNotes = new DataColumn(clsPOSDBConstants.CustomerNotes_Fld_Notes, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colNotes);
			this.colNotes.AllowDBNull = true;

			this.colUserID= new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colUserID);
			this.colUserID.AllowDBNull = true;

			this.colLastUpdatedOn= new DataColumn(clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn, typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colLastUpdatedOn);
			this.colLastUpdatedOn.AllowDBNull = true;

            this.colIsActive= new DataColumn(clsPOSDBConstants.CustomerNotes_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.colId};
		}
		
        public CustomerNotesRow NewclsCustomerNotesRow() 
		{
			return (CustomerNotesRow)this.NewRow();
		}

		public override DataTable Clone() 
		{
			CustomerNotesTable cln = (CustomerNotesTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new CustomerNotesRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(CustomerNotesRow);
		}
	} 
}
