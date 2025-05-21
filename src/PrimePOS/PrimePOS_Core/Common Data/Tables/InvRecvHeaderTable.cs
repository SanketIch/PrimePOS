
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class InvRecvHeaderTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colInvRecvId;
		private DataColumn colRefNo;
		private DataColumn colRecvDate;
		
		private DataColumn colVendorId;
		private DataColumn colVendorCode;
		private DataColumn colVendorName;
        //Added By Shitaljit(QuicSolv) on 24 June 2011
        private DataColumn colInvTransTypeId;
        //End
        private DataColumn colPOOrderNo;//Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577

		#region Constructors 
		internal InvRecvHeaderTable() : base(clsPOSDBConstants.InvRecvHeader_tbl) { this.InitClass(); }
		internal InvRecvHeaderTable(DataTable table) : base(table.TableName) {}
		#endregion
		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public InvRecvHeaderRow this[int index] 
		{
			get 
			{
				return ((InvRecvHeaderRow)(this.Rows[index]));
			}
		}

		public DataColumn InvRecvId 
		{
			get 
			{
				return this.colInvRecvId;
			}
		}

		public DataColumn RefNo
		{
			get 
			{
				return this.colRefNo;
			}
		}


		public DataColumn RecvDate
		{
			get 
			{
				return this.colRecvDate;
			}
		}

		public DataColumn VendorId 
		{
			get 
			{
				return this.colVendorId;
			}
		}

		public DataColumn VendorCode
		{
			get 
			{
				return this.colVendorCode;
			}
		}

		public DataColumn VendorName
		{
			get 
			{
				return this.colVendorName;
			}
		}
        //Added By Shitaljit(QuicSolv) 0n 24 june 2011
        public DataColumn InvTransTypeID
        {
            get
            {
                return this.colInvTransTypeId;
            }
        }
        //End of added By Shitaljit
        public DataColumn POOrderNo
        {
            get
            {
                return this.colPOOrderNo;
            }
        }
		#endregion //Properties
		#region Add and Get Methods 

		public  void AddRow(InvRecvHeaderRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(InvRecvHeaderRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.InvRecvID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}
        //Modified By Shitaljit(QuicSolv) on 24 june 2011 added System.Int32 InvTransTypeID
		public InvRecvHeaderRow AddRow( System.Int32 InvRecvId
										, System.String RefNo
										, System.DateTime  InvRecvDate 
										, System.Int32  VendorId
                                        , System.Int32 InvTransTypeID
                                        , System.String POOrderNO) 
		{
			InvRecvHeaderRow row = (InvRecvHeaderRow)this.NewRow();
			//row.ItemArray = new object[] {DeptId,DeptCode,DeptName,Discount,IsTaxable , SaleStartDate , SaleEndDate ,TaxId , SalePrice};
			row.InvRecvID=InvRecvId;
			row.RefNo=RefNo;
			row.RecvDate=InvRecvDate;
			row.VendorID=VendorId;
            row.InvTransTypeID = InvTransTypeID;//Added By Shitaljit(QuicSolv) on 24 june 2011
            row.POOrderNo = POOrderNO;
			this.Rows.Add(row);
			return row;
		}

		public InvRecvHeaderRow GetRowByID(System.Int32 InvRecvID) 
		{
			return (InvRecvHeaderRow)this.Rows.Find(new object[] {InvRecvID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			InvRecvHeaderRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (InvRecvHeaderRow)this.NewRow();

				if (dr[clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID] == DBNull.Value) 
					row[clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID] = DBNull.Value;
				else
					row[clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID] = Convert.ToInt32((dr[clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.InvRecvHeader_Fld_RefNo] == DBNull.Value) 
					row[clsPOSDBConstants.InvRecvHeader_Fld_RefNo] = DBNull.Value;
				else
					row[clsPOSDBConstants.InvRecvHeader_Fld_RefNo] = Convert.ToString(dr[clsPOSDBConstants.InvRecvHeader_Fld_RefNo].ToString());

				if (dr[clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate] == DBNull.Value) 
					row[clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate] = DBNull.Value;
				else
					if (dr[clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate].ToString().Trim()=="") 
						row[clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate]= Convert.ToDateTime(System.DateTime.MinValue.ToString());
					else
						row[clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate]= Convert.ToDateTime(dr[clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate].ToString());

				string strField=clsPOSDBConstants.InvRecvHeader_Fld_VendorID;
				if (dr[strField] == DBNull.Value) 
					row[strField] = DBNull.Value;
				else
					row[strField] = Convert.ToInt32((dr[strField].ToString()=="")? "0":dr[strField].ToString());
				
				if (dr[clsPOSDBConstants.Vendor_Fld_VendorCode] == DBNull.Value) 
					row[clsPOSDBConstants.Vendor_Fld_VendorCode] = DBNull.Value;
				else
					row[clsPOSDBConstants.Vendor_Fld_VendorCode] = ((dr[clsPOSDBConstants.Vendor_Fld_VendorCode].ToString()=="")? "":dr[clsPOSDBConstants.Vendor_Fld_VendorCode].ToString());

				if (dr[clsPOSDBConstants.InvRecvHeader_Fld_VendorID] == DBNull.Value) 
					row[clsPOSDBConstants.InvRecvHeader_Fld_VendorID] = DBNull.Value;
				else
					row[clsPOSDBConstants.InvRecvHeader_Fld_VendorID] = dr[clsPOSDBConstants.InvRecvHeader_Fld_VendorID].ToString();
				
				if (dr[clsPOSDBConstants.Vendor_Fld_VendorName] == DBNull.Value) 
					row[clsPOSDBConstants.Vendor_Fld_VendorName] = DBNull.Value;
				else
					row[clsPOSDBConstants.Vendor_Fld_VendorName] = dr[clsPOSDBConstants.Vendor_Fld_VendorName].ToString();
                //Added By Shitaljit(QuicSolv) on 24 june 2011
                if (dr[clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID] = dr[clsPOSDBConstants.InvTransType_Fld_ID].ToString();
                //End of added by shitaljit

                //Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577
                if (dr[clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo] == DBNull.Value)
                    row[clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo] = dr[clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo].ToString();
				this.AddRow(row);
			}
		}
		
		#endregion 
		public override DataTable Clone() 
		{
			InvRecvHeaderTable cln = (InvRecvHeaderTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new InvRecvHeaderTable();
		}

		internal void InitVars() 
		{
			this.colInvRecvId = this.Columns[clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID];
			this.colRecvDate= this.Columns[clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate];
			this.colRefNo = this.Columns[clsPOSDBConstants.InvRecvHeader_Fld_RefNo];
			this.colVendorCode= this.Columns[clsPOSDBConstants.Vendor_Fld_VendorCode];
			this.colVendorId= this.Columns[clsPOSDBConstants.InvRecvHeader_Fld_VendorID];
			this.colVendorName= this.Columns[clsPOSDBConstants.Vendor_Fld_VendorName];
            this.colInvTransTypeId = this.Columns[clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID];//Added By Shitaljit(QuicSolv) on 24 june 2011
            this.colPOOrderNo = this.Columns[clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo];//Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577
		}
		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colInvRecvId = new DataColumn(clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colInvRecvId);
			this.colInvRecvId.AllowDBNull = true;

			this.colRefNo = new DataColumn(clsPOSDBConstants.InvRecvHeader_Fld_RefNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRefNo);
			this.colRefNo.AllowDBNull = true;

			this.colRecvDate= new DataColumn(clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate,typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRecvDate);
			this.colRecvDate.AllowDBNull = true;

			this.colVendorId= new DataColumn(clsPOSDBConstants.InvRecvHeader_Fld_VendorID,typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colVendorId);
			this.colVendorId.AllowDBNull = true;

			this.colVendorCode= new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorCode, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colVendorCode);
			this.colVendorCode.AllowDBNull = true;

			this.colVendorName= new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorName, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colVendorName);
			this.colVendorName.AllowDBNull = true;

            //Added By shitaljit(QuicSolv) on 24 june 2011
            this.colInvTransTypeId = new DataColumn(clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInvTransTypeId);
            this.colInvTransTypeId.AllowDBNull = true;
            //End iof added By shitaljit.
            //Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577
            this.colPOOrderNo = new DataColumn(clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPOOrderNo);
            this.colPOOrderNo.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.colInvRecvId};
		}
		public  InvRecvHeaderRow NewInvRecvHeaderRow() 
		{
			return (InvRecvHeaderRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new InvRecvHeaderRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(InvRecvHeaderRow);
		}
	}
}
