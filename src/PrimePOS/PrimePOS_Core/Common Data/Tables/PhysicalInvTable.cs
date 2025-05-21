namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;

	//         This class is used to define the shape of PhysicalInvTable.
	public class PhysicalInvTable : DataTable
	{

		private DataColumn colId;
		private DataColumn colItemCode;
		private DataColumn colDescription;
		private DataColumn colOldQty;
		private DataColumn colNewQty;
		private DataColumn colUserID;
		private DataColumn colTransDate;
		private DataColumn colisProcessed;
		private DataColumn colPUserID;
		private DataColumn colPTransDate;
        private DataColumn colOldSellingPrice;
        private DataColumn colNewSellingPrice;
        private DataColumn colOldCostPrice;
        private DataColumn colNewCostPrice;
        private DataColumn colOldExpDate;  //Sprint-21 - 2206 09-Mar-2016 JY Added
        private DataColumn colNewExpDate;  //Sprint-21 - 2206 09-Mar-2016 JY Added
        private DataColumn colLastInvUpdatedQty;    //PRIMEPOS-2395 21-Jun-2018 JY Added

        #region Constructors 
        internal PhysicalInvTable() : base(clsPOSDBConstants.PhysicalInv_tbl) { this.InitClass(); }
		internal PhysicalInvTable(DataTable table) : base(table.TableName) {}
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

		public PhysicalInvRow this[int index] 
		{
			get 
			{
				return ((PhysicalInvRow)(this.Rows[index]));
			}
		}

		// Public Property DataColumn PhysicalInvcode
		public DataColumn Id 
		{
			get 
			{
				return this.colId;
			}
		}

		public DataColumn UserID
		{
			get 
			{
				return this.colUserID;
			}
		}

		public DataColumn TransDate
		{
			get 
			{
				return this.colTransDate;
			}
		}

		public DataColumn Description
		{
			get 
			{
				return this.colDescription;
			}
		}

		public DataColumn ItemCode
		{
			get 
			{
				return this.colItemCode;
			}
		}

		public DataColumn OldQty
		{
			get 
			{
				return this.colOldQty;
			}
		}
		
        public DataColumn NewQty
		{
			get 
			{
				return this.colNewQty;
			}
		}
		
        public DataColumn PUserID
		{
			get 
			{
				return this.colPUserID;
			}
		}

		public DataColumn PTransDate
		{
			get 
			{
				return this.colPTransDate;
			}
		}

		public DataColumn isProcessed
		{
			get 
			{
				return this.colisProcessed;
			}
		}

        public DataColumn OldSellingPrice
        {
            get
            {
                return this.colOldSellingPrice;
            }
        }

        public DataColumn NewSellingPrice
        {
            get
            {
                return this.colNewSellingPrice;
            }
        }

        public DataColumn OldCostPrice
        {
            get
            {
                return this.colOldCostPrice;
            }
        }

        public DataColumn NewCostPrice
        {
            get
            {
                return this.colNewCostPrice;
            }
        }

        #region Sprint-21 - 2206 09-Mar-2016 JY Added code for exp. date
        /// <summary>
        /// Public Property DataColumn ExpirationDate
        /// </summary>	
        public DataColumn OldExpDate
        {
            get
            {
                return this.colOldExpDate;
            }
        }

        /// <summary>
        /// Public Property DataColumn ExpirationDate
        /// </summary>	
        public DataColumn NewExpDate
        {
            get
            {
                return this.colNewExpDate;
            }
        }
        #endregion

        //PRIMEPOS-2395 21-Jun-2018 JY Added
        public DataColumn LastInvUpdatedQty
        {
            get
            {
                return this.colLastInvUpdatedQty;
            }
        }
        #endregion //Properties

        #region Add and Get Methods 

        public  void AddRow(PhysicalInvRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(PhysicalInvRow row, bool preserveChanges) 
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

		public  PhysicalInvRow AddRow(System.Int32 Id , System.String ItemCode,System.Int32 OldQty, System.Int32 NewQty,
            System.Decimal dOldSellingPrice,System.Decimal dNewSellingPrice,System.Decimal dOldCostPrice,System.Decimal dNewCostPrice) 
		{
		
			PhysicalInvRow row = (PhysicalInvRow)this.NewRow();
            row.ItemArray = new object[] { Id, ItemCode, "", OldQty, NewQty, 0, "", DateTime.Now, "", DateTime.Now, dOldSellingPrice, dNewSellingPrice, dOldCostPrice, dNewCostPrice };
			this.Rows.Add(row);
			return row;
		}

		public PhysicalInvRow GetRowByID(System.Int32 ID) 
		{
			return (PhysicalInvRow)this.Rows.Find(new object[] {ID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
			//add any rows in the DataTable 
			PhysicalInvRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (PhysicalInvRow)this.NewRow();

				if (dr[clsPOSDBConstants.PhysicalInv_Fld_ID] == DBNull.Value) 
					row[clsPOSDBConstants.PhysicalInv_Fld_ID] = 0;
				else
					row[clsPOSDBConstants.PhysicalInv_Fld_ID] = Convert.ToInt32(dr[clsPOSDBConstants.PhysicalInv_Fld_ID].ToString());

				if (dr[clsPOSDBConstants.PhysicalInv_Fld_ItemCode] == DBNull.Value) 
					row[clsPOSDBConstants.PhysicalInv_Fld_ItemCode] = DBNull.Value;
				else
					row[clsPOSDBConstants.PhysicalInv_Fld_ItemCode] = Convert.ToString(dr[clsPOSDBConstants.PhysicalInv_Fld_ItemCode].ToString());

				if (dr[clsPOSDBConstants.Item_Fld_Description] == DBNull.Value) 
					row[clsPOSDBConstants.Item_Fld_Description] = DBNull.Value;
				else
					row[clsPOSDBConstants.Item_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_Description].ToString());

				if (dr[clsPOSDBConstants.PhysicalInv_Fld_OldQty] == DBNull.Value) 
					row[clsPOSDBConstants.PhysicalInv_Fld_OldQty] = 0;
				else
					row[clsPOSDBConstants.PhysicalInv_Fld_OldQty] = Convert.ToInt32(dr[clsPOSDBConstants.PhysicalInv_Fld_OldQty].ToString());

				if (dr[clsPOSDBConstants.PhysicalInv_Fld_NewQty] == DBNull.Value) 
					row[clsPOSDBConstants.PhysicalInv_Fld_NewQty] = DBNull.Value;
				else
					row[clsPOSDBConstants.PhysicalInv_Fld_NewQty] = Convert.ToInt32(dr[clsPOSDBConstants.PhysicalInv_Fld_NewQty].ToString());

				row[clsPOSDBConstants.fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.fld_UserID].ToString());
				if (dr[clsPOSDBConstants.PhysicalInv_Fld_TransDate] == DBNull.Value) 
					row[clsPOSDBConstants.PhysicalInv_Fld_TransDate] = DateTime.Now;
				else
					row[clsPOSDBConstants.PhysicalInv_Fld_TransDate] = Convert.ToDateTime(dr[clsPOSDBConstants.PhysicalInv_Fld_TransDate].ToString());

				row[clsPOSDBConstants.PhysicalInv_Fld_PUserID] = Convert.ToString(dr[clsPOSDBConstants.PhysicalInv_Fld_PUserID].ToString());

				if (dr[clsPOSDBConstants.PhysicalInv_Fld_PTransDate] == DBNull.Value) 
					row[clsPOSDBConstants.PhysicalInv_Fld_PTransDate] = DateTime.Now;
				else
					row[clsPOSDBConstants.PhysicalInv_Fld_PTransDate] = Convert.ToDateTime(dr[clsPOSDBConstants.PhysicalInv_Fld_PTransDate].ToString());
				
				if (dr[clsPOSDBConstants.PhysicalInv_Fld_isProcessed] == DBNull.Value) 
					row[clsPOSDBConstants.PhysicalInv_Fld_isProcessed] = 0;
				else
					row[clsPOSDBConstants.PhysicalInv_Fld_isProcessed] = Convert.ToBoolean(dr[clsPOSDBConstants.PhysicalInv_Fld_isProcessed].ToString());

                if (dr[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice] == DBNull.Value)
                    row[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice] = 0;
                else
                    row[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice].ToString());

                if (dr[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice] == DBNull.Value)
                    row[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice] = 0;
                else
                    row[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice].ToString());

                if (dr[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice] == DBNull.Value)
                    row[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice] = 0;
                else
                    row[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice].ToString());

                if (dr[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice] == DBNull.Value)
                    row[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice] = 0;
                else
                    row[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice] = Convert.ToDecimal(dr[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice].ToString());

                #region Sprint-21 - 2206 10-Mar-2016 JY Added code for exp. date
                if (dr[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate] == DBNull.Value)
                    row[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate] = Convert.ToDateTime(dr[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate].ToString());
                if (dr[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate] == DBNull.Value)
                    row[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate] = Convert.ToDateTime(dr[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate].ToString());
                #endregion

				this.AddRow(row);
			}
		}
		
		#endregion //Add and Get Methods 
	
        protected override DataTable CreateInstance() 
		{
			return new PhysicalInvTable();
		}

		internal void InitVars() 
		{
			try 
			{
				this.colId = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_ID];
				this.colItemCode = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_ItemCode];
				this.colDescription = this.Columns[clsPOSDBConstants.Item_Fld_Description];
				this.colOldQty= this.Columns[clsPOSDBConstants.PhysicalInv_Fld_OldQty];
				this.colNewQty= this.Columns[clsPOSDBConstants.PhysicalInv_Fld_NewQty];
				this.colUserID= this.Columns[clsPOSDBConstants.PhysicalInv_Fld_UserID];
				this.colTransDate= this.Columns[clsPOSDBConstants.PhysicalInv_Fld_TransDate];
				this.colPUserID= this.Columns[clsPOSDBConstants.PhysicalInv_Fld_PUserID];
				this.colPTransDate= this.Columns[clsPOSDBConstants.PhysicalInv_Fld_PTransDate];
				this.colisProcessed = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_isProcessed];
                this.colOldSellingPrice = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice];
                this.colNewSellingPrice = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice];
                this.colOldCostPrice = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice];
                this.colNewCostPrice = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice];
                this.colOldExpDate = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_OldExpDate];    //Sprint-21 - 2206 09-Mar-2016 JY Added
                this.colNewExpDate = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_NewExpDate];    //Sprint-21 - 2206 09-Mar-2016 JY Added
                this.colLastInvUpdatedQty = this.Columns[clsPOSDBConstants.PhysicalInv_Fld_LastInvUpdatedQty];    //PRIMEPOS-2395 21-Jun-2018 JY Added
            }
			catch(Exception exp)
			{
				throw(exp);
			}
		}

		private void InitClass() 
		{
			this.colId = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colId);
			this.colId.AllowDBNull = false;

			this.colItemCode = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_ItemCode, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemCode);
			this.colItemCode.AllowDBNull = false;

			this.colDescription = new DataColumn(clsPOSDBConstants.Item_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDescription);
			this.colDescription.AllowDBNull = true;

			this.colOldQty= new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_OldQty, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colOldQty);
			this.colOldQty.AllowDBNull = false;

			this.colNewQty = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_NewQty, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colNewQty);
			this.colNewQty.AllowDBNull = false;

			this.colisProcessed= new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_isProcessed, typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colisProcessed);
			this.colisProcessed.AllowDBNull = true;

			this.colUserID= new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colUserID);
			this.colUserID.AllowDBNull = true;

			this.colTransDate= new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_TransDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransDate);
			this.colTransDate.AllowDBNull = true;

			this.colPUserID= new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_PUserID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colPUserID);
			this.colPUserID.AllowDBNull = true;

			this.colPTransDate= new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_PTransDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colPTransDate);
			this.colPTransDate.AllowDBNull = true;

            this.colOldSellingPrice= new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOldSellingPrice);
            this.colOldSellingPrice.AllowDBNull = true;

            this.colNewSellingPrice = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNewSellingPrice);
            this.colNewSellingPrice.AllowDBNull = true;

            this.colOldCostPrice= new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOldCostPrice);
            this.colOldCostPrice.AllowDBNull = true;

            this.colNewCostPrice = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNewCostPrice);
            this.colNewCostPrice.AllowDBNull = true;

            #region Sprint-21 - 2206 09-Mar-2016 JY Added code for  exp. date
            this.colOldExpDate = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_OldExpDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOldExpDate);
            this.colOldExpDate.AllowDBNull = true;

            this.colNewExpDate = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_NewExpDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNewExpDate);
            this.colNewExpDate.AllowDBNull = true;
            #endregion

            //PRIMEPOS-2395 21-Jun-2018 JY Added
            this.colLastInvUpdatedQty = new DataColumn(clsPOSDBConstants.PhysicalInv_Fld_LastInvUpdatedQty, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastInvUpdatedQty);
            this.colLastInvUpdatedQty.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] {this.colId};
		}
		
        public PhysicalInvRow NewPhysicalInvRow() 
		{
			return (PhysicalInvRow)this.NewRow();
		}

		public override DataTable Clone() 
		{
			PhysicalInvTable cln = (PhysicalInvTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new PhysicalInvRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(PhysicalInvRow);
		}
	} 
}
