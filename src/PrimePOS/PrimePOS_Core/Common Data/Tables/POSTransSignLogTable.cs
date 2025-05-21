namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

    public class POSTransSignLogTable: DataTable, System.Collections.IEnumerable 
	{
        
		private DataColumn colSignLogID;
		private DataColumn colPOSTransId;
		private DataColumn colSignContext;
		private DataColumn colSignContextData;

		private DataColumn colSignDataBinary;
		private DataColumn colSignDataText;
		private DataColumn colCustomerIDType;
		private DataColumn colCustomerIDDetail;
        private DataColumn colTransDetailID;
	
		#region Constants
		private const String _TableName = "POSTransSignLog";
		#endregion
		#region Constructors 
		internal POSTransSignLogTable() : base(_TableName) { this.InitClass(); }
		internal POSTransSignLogTable(DataTable table) : base(table.TableName) {}
		#endregion
		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public POSTransSignLogRow this[int index] 
		{
			get 
			{
				return ((POSTransSignLogRow)(this.Rows[index]));
			}
		}

		public DataColumn SignLogID
		{
			get 
			{
				return this.colSignLogID;
			}
		}

        public DataColumn POSTransId	
        {
            get
            {
                return this.colPOSTransId;
            }
        }

        public DataColumn SignContext	
        {
            get
            {
                return this.colSignContext;
            }
        }
        public DataColumn SignContextData
        {
            get
            {
                return this.colSignContextData;
            }
        }
        public DataColumn SignDataBinary		
        {
            get
            {
                return this.colSignDataBinary;
            }
        }


        public DataColumn SignDataText	
        {
            get
            {
                return this.colSignDataText;
            }
        }

        public DataColumn CustomerIDType	
        {
            get
            {
                return this.colCustomerIDType;
            }
        }

        public DataColumn CustomerIDDetail
        {
            get
            {
                return this.colCustomerIDDetail;
            }
        }

        public DataColumn TransDetailID
        {
            get
            {
                return this.colTransDetailID;
            }
        }

		#endregion //Properties


		#region Add and Get Methods 

		public  void AddRow(POSTransSignLogRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(POSTransSignLogRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.SignLogID) == null) 
			{
				this.Rows.Add(row);
				if(preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public POSTransSignLogRow AddRow( System.Int32 SignLogID
										, System.Int32 POSTransId
										, System.String SignContext
										, System.String SignContextData
										, System.Byte[] SignDataBinary
										, System.String SignDataText 
										, System.String  CustomerIDType 
										, System.String  CustomerIDDetail
                                        , System.Int32 TransDetailID)
		{
			POSTransSignLogRow row = (POSTransSignLogRow)this.NewRow();
			row.SignLogID=SignLogID;
			row.POSTransId=POSTransId;
			row.SignContext=SignContext;
			row.SignContextData=SignContextData;
			row.SignDataBinary=SignDataBinary;
			row.SignDataText=SignDataText;
			row.CustomerIDType=CustomerIDType;
			row.CustomerIDDetail=CustomerIDDetail;
            row.TransDetailID = TransDetailID;

            this.Rows.Add(row);
			return row;
		}

		public POSTransSignLogRow GetRowByID(System.Int32 SignLogID) 
		{
			return (POSTransSignLogRow)this.Rows.Find(new object[] {SignLogID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			POSTransSignLogRow row;
	
			foreach(DataRow dr in dt.Rows) 
			{
				row = (POSTransSignLogRow)this.NewRow();

				if (dr[clsPOSDBConstants.POSTransSignLog_Fld_SignLogID] == DBNull.Value) 
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignLogID] = DBNull.Value;
				else
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignLogID] = Convert.ToInt32((dr[clsPOSDBConstants.POSTransSignLog_Fld_SignLogID].ToString()=="")?"0":dr[clsPOSDBConstants.POSTransSignLog_Fld_SignLogID].ToString());

				if (dr[clsPOSDBConstants.POSTransSignLog_Fld_POSTransId] == DBNull.Value) 
					row[clsPOSDBConstants.POSTransSignLog_Fld_POSTransId] = DBNull.Value;
				else
                    row[clsPOSDBConstants.POSTransSignLog_Fld_POSTransId] = Convert.ToInt32((dr[clsPOSDBConstants.POSTransSignLog_Fld_POSTransId].ToString() == "") ? "0" : dr[clsPOSDBConstants.POSTransSignLog_Fld_POSTransId].ToString());

				if (dr[clsPOSDBConstants.POSTransSignLog_Fld_SignContext] == DBNull.Value) 
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignContext] = DBNull.Value;
				else
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignContext] = Convert.ToString(dr[clsPOSDBConstants.POSTransSignLog_Fld_SignContext].ToString());

				if (dr[clsPOSDBConstants.POSTransSignLog_Fld_SignContextData] == DBNull.Value) 
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignContextData] = DBNull.Value;
				else
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignContextData] = Convert.ToString((dr[clsPOSDBConstants.POSTransSignLog_Fld_SignContextData].ToString()=="")? "0":dr[clsPOSDBConstants.POSTransSignLog_Fld_SignContextData].ToString());

				if (dr[clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary] == DBNull.Value) 
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary] = DBNull.Value;
				else
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary] = (byte[])dr[clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary];

				if (dr[clsPOSDBConstants.POSTransSignLog_Fld_SignDataText] == DBNull.Value) 
					row[clsPOSDBConstants.POSTransSignLog_Fld_SignDataText] = DBNull.Value;
				else
						row[clsPOSDBConstants.POSTransSignLog_Fld_SignDataText]= Convert.ToString(dr[clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary].ToString());

                if (dr[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType] = DBNull.Value;
				else
                    row[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType] = Convert.ToString(dr[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType].ToString());

                if (dr[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail] = Convert.ToString(dr[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail].ToString());

                if (dr[clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID].ToString() == "") ? "0" : dr[clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID].ToString());

                this.AddRow(row);
			}
		}
		
		#endregion 
		public override DataTable Clone() 
		{
			POSTransSignLogTable cln = (POSTransSignLogTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new POSTransSignLogTable();
		}

		internal void InitVars() 
		{
			this.colSignLogID = this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_SignLogID];
            this.colPOSTransId = this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_POSTransId];
			this.colSignContext = this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_SignContext];
			this.colSignContextData = this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_SignContextData];
            this.colSignDataBinary = this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary];
			this.colSignDataText= this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_SignDataText];
            this.colCustomerIDType = this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType];
			this.colCustomerIDDetail= this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail];
            this.colTransDetailID = this.Columns[clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID];
		}

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colSignLogID = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_SignLogID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSignLogID);
			this.colSignLogID.AllowDBNull = true;

            this.colPOSTransId = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_POSTransId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPOSTransId);
            this.colPOSTransId.AllowDBNull = true;

			this.colSignContext = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_SignContext, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSignContext);
			this.colSignContext.AllowDBNull = true;

            this.colSignContextData = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_SignContextData, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSignContextData);
			this.colSignContextData.AllowDBNull = true;

            this.colSignDataBinary = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_SignDataBinary,typeof(System.Byte[]), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSignDataBinary);
			this.colSignDataBinary.AllowDBNull = true;

            this.colSignDataText = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_SignDataText,typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSignDataText);
			this.colSignDataText.AllowDBNull = true;

			this.colCustomerIDType = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDType, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCustomerIDType);
			this.colCustomerIDType.AllowDBNull = true;

			this.colCustomerIDDetail = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_CustomerIDDetail, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCustomerIDDetail);
			this.colCustomerIDDetail.AllowDBNull = true;

            this.colTransDetailID = new DataColumn(clsPOSDBConstants.POSTransSignLog_Fld_TransDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransDetailID);
            this.colTransDetailID.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.colSignLogID};
		}

		public  POSTransSignLogRow NewPOSTransSignLogRow() 
		{
            int SignLogID = 1;
            foreach (POSTransSignLogRow oRow in this.Rows)
            {
                if (oRow.SignLogID >= SignLogID)
                {
                    SignLogID = oRow.SignLogID + 1;
                }
            }
			POSTransSignLogRow oNewRow =(POSTransSignLogRow)this.NewRow();
            oNewRow.SignLogID = SignLogID;
            return oNewRow;

		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new POSTransSignLogRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(POSTransSignLogRow);
		}

        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
        }

        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
        }
	}
}
