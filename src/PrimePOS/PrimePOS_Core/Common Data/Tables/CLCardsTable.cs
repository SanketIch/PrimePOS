
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLCardsTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colID;
		private DataColumn colIsPrepetual;

		private DataColumn colRegisterDate;
		private DataColumn colCLCardID;
        private DataColumn colDescription;
        private DataColumn colCurrentPoints;
        private DataColumn colExpiryDays;
        private DataColumn colCustomerID;
        private DataColumn colIsActive;

		#region Constants
		private const String _TableName = "CL_Cards";
		#endregion

		#region Constructors 
		internal CLCardsTable() : base(_TableName) { this.InitClass(); }
		internal CLCardsTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public CLCardsRow this[int index] 
		{
			get 
			{
				return ((CLCardsRow)(this.Rows[index]));
			}
		}

		public DataColumn ID 
		{
			get 
			{
				return this.colID;
			}
		}

		public DataColumn IsPrepetual 
		{
			get 
			{
				return this.colIsPrepetual;
			}
		}

		public DataColumn CLCardID 
		{
			get 
			{
				return this.colCLCardID;
			}
		}

        public DataColumn CurrentPoints
        {
            get
            {
                return this.colCurrentPoints;
            }
        }
        
        public DataColumn Description 
		{
			get 
			{
				return this.colDescription;
			}
		}


        public DataColumn RegisterDate
        {
            get
            {
                return this.colRegisterDate;
            }
        }

        public DataColumn ExpiryDays
        {
            get
            {
                return this.colExpiryDays;
            }
        }


        public DataColumn CustomerID
        {
            get
            {
                return this.colCustomerID;
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

		public  void AddRow(CLCardsRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(CLCardsRow row, bool preserveChanges) 
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

		public CLCardsRow AddRow(System.Int32 ID 
										,System.Boolean bIsPrepetual 
										, System.Int64  iCLCardID
                                        , System.String sDescription
                                        , System.Int32 iExpiryDays
                                        ,Boolean bIsActive) 
		{
			CLCardsRow row = (CLCardsRow)this.NewRow();
			//row.ItemArray = new object[] {ID,DeptCode,DeptName,Discount,IsPrepetual , IsPrepetual , RegisterDate ,CLCardID , SalePrice};
			row.ID=ID;
			row.IsPrepetual=bIsPrepetual;
			row.RegisterDate=DateTime.Now;
			row.CLCardID=iCLCardID;
            row.Description=sDescription;
            row.CurrentPoints = 0;
            row.ExpiryDays = iExpiryDays;
            row.CustomerID = 0;
            row.IsActive = bIsActive;
			this.Rows.Add(row);
			return row;
		}

		public CLCardsRow GetRowByID(System.Int64 iID) 
		{
            return (CLCardsRow)this.Rows.Find(new object[] { iID });
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			CLCardsRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (CLCardsRow)this.NewRow();

				if (dr[clsPOSDBConstants.CLCards_Fld_ID] == DBNull.Value) 
					row[clsPOSDBConstants.CLCards_Fld_ID] = DBNull.Value;
				else
					row[clsPOSDBConstants.CLCards_Fld_ID] = Convert.ToInt32((dr[clsPOSDBConstants.CLCards_Fld_ID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.CLCards_Fld_IsPrepetual] == DBNull.Value) 
					row[clsPOSDBConstants.CLCards_Fld_IsPrepetual] = DBNull.Value;
				else
					row[clsPOSDBConstants.CLCards_Fld_IsPrepetual] = Convert.ToBoolean((dr[clsPOSDBConstants.CLCards_Fld_IsPrepetual].ToString()=="")? "false":dr[clsPOSDBConstants.CLCards_Fld_IsPrepetual].ToString());

				
				if (dr[clsPOSDBConstants.CLCards_Fld_RegisterDate] == DBNull.Value) 
					row[clsPOSDBConstants.CLCards_Fld_RegisterDate] = DBNull.Value;
				else
					if (dr[clsPOSDBConstants.CLCards_Fld_RegisterDate].ToString().Trim()=="") 
						row[clsPOSDBConstants.CLCards_Fld_RegisterDate]=Convert.ToDateTime(System.DateTime.MinValue.ToString());
					else
						row[clsPOSDBConstants.CLCards_Fld_RegisterDate]=Convert.ToDateTime(dr[clsPOSDBConstants.CLCards_Fld_RegisterDate].ToString());

				string strField=clsPOSDBConstants.CLCards_Fld_CLCardID	;
				if (dr[strField] == DBNull.Value) 
					row[strField] = 0;
				else
					row[strField] = Convert.ToInt64((dr[strField].ToString()=="")? "0":dr[strField].ToString());

                strField=clsPOSDBConstants.CLCards_Fld_Description;
				if (dr[strField] == DBNull.Value) 
					row[strField] = DBNull.Value;
				else
					row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.CLCards_Fld_CurrentPoints;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.CLCards_Fld_ExpiryDays;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.CLCards_Fld_CustomerID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                if (dr[clsPOSDBConstants.CLCards_Fld_IsActive] == DBNull.Value)
                    row[clsPOSDBConstants.CLCards_Fld_IsActive] = DBNull.Value;
                else
                    row[clsPOSDBConstants.CLCards_Fld_IsActive] = Convert.ToBoolean((dr[clsPOSDBConstants.CLCards_Fld_IsActive].ToString() == "") ? "false" : dr[clsPOSDBConstants.CLCards_Fld_IsActive].ToString());

				this.AddRow(row);
			}
		}
		
		#endregion 

		public override DataTable Clone() 
		{
			CLCardsTable cln = (CLCardsTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		
        protected override DataTable CreateInstance() 
		{
			return new CLCardsTable();
		}

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.CLCards_Fld_ID];
            this.colIsPrepetual = this.Columns[clsPOSDBConstants.CLCards_Fld_IsPrepetual];
            this.colRegisterDate = this.Columns[clsPOSDBConstants.CLCards_Fld_RegisterDate];
            this.colCLCardID = this.Columns[clsPOSDBConstants.CLCards_Fld_CLCardID];
            this.colDescription = this.Columns[clsPOSDBConstants.CLCards_Fld_Description];
            this.colCurrentPoints = this.Columns[clsPOSDBConstants.CLCards_Fld_CurrentPoints];
            this.colExpiryDays = this.Columns[clsPOSDBConstants.CLCards_Fld_ExpiryDays];
            this.colCustomerID = this.Columns[clsPOSDBConstants.CLCards_Fld_CustomerID];
            this.colIsActive = this.Columns[clsPOSDBConstants.CLCards_Fld_IsActive];
        }

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colID = new DataColumn("ID", typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colID);
			this.colID.AllowDBNull = true;

			this.colIsPrepetual = new DataColumn("IsPrepetual", typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colIsPrepetual);
			this.colIsPrepetual.AllowDBNull = true;

			this.colRegisterDate = new DataColumn("RegisterDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRegisterDate);
			this.colRegisterDate.AllowDBNull = true;

			this.colCLCardID = new DataColumn("CardID", typeof(System.Int64), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCLCardID);
			this.colCLCardID.AllowDBNull = true;

            this.colDescription = new DataColumn("Description", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDescription);
            this.colDescription.AllowDBNull = true;

            this.colCurrentPoints = new DataColumn("CurrentPoints", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCurrentPoints);
            this.colCurrentPoints.AllowDBNull = true;

            this.colExpiryDays = new DataColumn("ExpiryDays", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExpiryDays);
            this.colExpiryDays.AllowDBNull = true;

            this.colCustomerID = new DataColumn("CustomerID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerID);
            this.colCustomerID.AllowDBNull = true;

            this.colIsActive = new DataColumn("IsActive", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.colID};
		}
		public  CLCardsRow NewCLCardsRow() 
		{
			return (CLCardsRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new CLCardsRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(CLCardsRow);
		}
	}
}
