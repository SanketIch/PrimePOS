
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class POHeaderTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colOrderID;
		private DataColumn colOrderNo;
		private DataColumn colOrderDate;
		private DataColumn colExptDelvDate;
		
		private DataColumn colVendorID;
		private DataColumn colVendorCode;
		private DataColumn colVendorName;
		private DataColumn colStatus;

		private DataColumn colisFTPUsed;
		private DataColumn colAckType;
		private DataColumn colAckStatus;
		private DataColumn colAckDate;

        //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //Coulomns Added for VendorInterface
         private DataColumn colIsMaxReached;
         private DataColumn colPrimePOOrderID;
        //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
         //Added By SRT(Gaurav) Date: 04-06-2009
         private DataColumn colDescription;
         private DataColumn colFlagged;
         //End Of Added By SRT(Gaurav)
        //added by atul 22-oct-2010
        private DataColumn colInvoiceDate;
        private DataColumn colInvoiceNumber;
        //End of added by atul 22-oct-2010
        //Added By Ravindra (QuicSolv) 16 Jan 2013
        private DataColumn colRefOrderNo;
        //End of Added by Ravindra(Quicsolv) 16 Jan 2013
        private DataColumn colAckFileType;//Added By shitaljit on 17May 2013 to add file type 
        private DataColumn colProcessedBy;  //Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added to display ProcessedBy
        private DataColumn colProcessedType;   //Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added to display ProcessedType
        private DataColumn colTransTypeCode;    //PRIMEPOS-2901 05-Nov-2020 JY Added
        
        #region Constructors 
        public POHeaderTable() : base(clsPOSDBConstants.POHeader_tbl) { this.InitClass(); }
		internal POHeaderTable(DataTable table) : base(table.TableName) {}
		#endregion
		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public POHeaderRow this[int index] 
		{
			get 
			{
				return ((POHeaderRow)(this.Rows[index]));
			}
		}

		public DataColumn OrderID
		{
			get 
			{
				return this.colOrderID;
			}
		}

		public DataColumn OrderNo
		{
			get 
			{
				return this.colOrderNo;
			}
		}


		public DataColumn OrderDate
		{
			get 
			{
				return this.colOrderDate;
			}
		}
		
		public DataColumn AckDate
		{
			get 
			{
				return this.colAckDate;
			}
		}

		public DataColumn ExptDelvDate
		{
			get 
			{
				return this.colExptDelvDate;
			}
		}

		
		public DataColumn isFTPUsed
		{
			get 
			{
				return this.colisFTPUsed;
			}
		}

		
		public DataColumn AckType
		{
			get 
			{
				return this.colAckType;
			}
		}

		
		public DataColumn AckStatus
		{
			get 
			{
				return this.colAckStatus;
			}
		}

		public DataColumn VendorId 
		{
			get 
			{
				return this.colVendorID;
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

		public DataColumn Status
		{
			get 
			{
				return this.colStatus;
			}
		}
        // Added by atul 22-oct-2010
        public DataColumn InvoiceDate
        {
            get
            {
                return this.colInvoiceDate;
            }
        }
        public DataColumn InvoiceNumber
        {
            get
            {
                return this.colInvoiceNumber;
            }
        }
        //end of Added by atul 22-oct-2010
        //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //Coulomns Added for VendorInterface
        
        public DataColumn PrimePOrderId
        {
            get
            {
                return this.colPrimePOOrderID;
            }
        }

        public DataColumn IsMaxReached
        {
            get
            {
                return this.colIsMaxReached;
            }
        }
        
        //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //Added By SRT(Gaurav) Date: 04-06-2009
        public DataColumn Description
        {
            get
            {
                return this.colDescription;
            }
        }
        public DataColumn Flagged
        {
            get
            {
                return this.colFlagged;
            }
        }
        //End Of Added By SRT(Gaurav)
        // Added by Ravindra (Quicsolv) 16 Jan 2013
        public DataColumn RefOrderNo
        {
            get
            {
                return this.colRefOrderNo;
            }
        }
        // End Of Added BY Ravindra(Quicsolv) 16 Jan 2013

       //Added By shitaljit to store file type on 17 May 13
        public DataColumn AckFileType
        {
            get
            {
                return this.colAckFileType;
            }
        }

        #region Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added
        public DataColumn ProcessedBy
        {
            get
            {
                return this.colProcessedBy;
            }
        }

        public DataColumn ProcessedType
        {
            get
            {
                return this.colProcessedType;
            }
        }
        #endregion

        public DataColumn TransTypeCode //PRIMEPOS-2901 05-Nov-2020 JY Added
        {
            get
            {
                return this.colTransTypeCode;
            }
        }
        #endregion //Properties
        #region Add and Get Methods

        public  void AddRow(POHeaderRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(POHeaderRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.OrderID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public POHeaderRow AddRow( System.Int32 OrderID
										, System.String OrderNo
										, System.DateTime  OrderDate 
										, System.DateTime  ExptDelvDate 
										, System.Int32  VendorId
										, System.Int32 Status) 
		{
			POHeaderRow row = (POHeaderRow)this.NewRow();
			row.OrderID=OrderID;
			row.OrderNo=OrderNo;
			row.OrderDate=OrderDate;
			row.ExptDelvDate=ExptDelvDate;
			row.VendorID=VendorId;
			row.Status=Status;
		
			this.Rows.Add(row);
			return row;
		}

        //Added By SRT(Gaurav) Date : 04/07/2009
        public POHeaderRow AddRow(System.Int32 OrderID
                                        , System.String OrderNo
                                        , System.DateTime OrderDate
                                        , System.DateTime ExptDelvDate
                                        , System.Int32 VendorId
                                        , System.Int32 Status
                                        , System.String Description
                                        , System.Boolean Flagged)
        {
            POHeaderRow row = (POHeaderRow)this.NewRow();
            row.OrderID = OrderID;
            row.OrderNo = OrderNo;
            row.OrderDate = OrderDate;
            row.ExptDelvDate = ExptDelvDate;
            row.VendorID = VendorId;
            row.Status = Status;
            row.Description = Description;
            row.Flagged = Flagged;
            this.Rows.Add(row);
            return row;
        }
        //End Of Added By sRT(Gaurav)

		public POHeaderRow GetRowByID(System.Int32 OrderID) 
		{
			return (POHeaderRow)this.Rows.Find(new object[] {OrderID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			POHeaderRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (POHeaderRow)this.NewRow();

                //Validation update by PRASHANT(SRT) Date:6-7-2009
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_OrderID))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_OrderID] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_OrderID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_OrderID] = Convert.ToInt32((dr[clsPOSDBConstants.POHeader_Fld_OrderID].ToString() == "") ? "0" : dr[0].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_OrderNo))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_OrderNo] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_OrderNo] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_OrderNo] = Convert.ToString(dr[clsPOSDBConstants.POHeader_Fld_OrderNo].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_OrderDate))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_OrderDate] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_OrderDate] = DBNull.Value;
                    else
                        if (dr[clsPOSDBConstants.POHeader_Fld_OrderDate].ToString().Trim() == "")
                            row[clsPOSDBConstants.POHeader_Fld_OrderDate] = DBNull.Value;
                        else
                            row[clsPOSDBConstants.POHeader_Fld_OrderDate] = Convert.ToDateTime(dr[clsPOSDBConstants.POHeader_Fld_OrderDate].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_ExptDelvDate))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_ExptDelvDate] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_ExptDelvDate] = DBNull.Value;
                    else
                        if (dr[clsPOSDBConstants.POHeader_Fld_ExptDelvDate].ToString().Trim() == "")
                            row[clsPOSDBConstants.POHeader_Fld_ExptDelvDate] = DBNull.Value;
                        else
                            row[clsPOSDBConstants.POHeader_Fld_ExptDelvDate] = Convert.ToDateTime(dr[clsPOSDBConstants.POHeader_Fld_ExptDelvDate].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.Vendor_Fld_VendorCode))
                {
                    if (dr[clsPOSDBConstants.Vendor_Fld_VendorCode] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_VendorCode] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_VendorCode] = ((dr[clsPOSDBConstants.Vendor_Fld_VendorCode].ToString() == "") ? "" : dr[clsPOSDBConstants.Vendor_Fld_VendorCode].ToString());
                }


                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_VendorID))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_VendorID] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_VendorID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_VendorID] = dr[clsPOSDBConstants.POHeader_Fld_VendorID].ToString();
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.Vendor_Fld_VendorName))
                {
                    if (dr[clsPOSDBConstants.Vendor_Fld_VendorName] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_VendorName] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_VendorName] = dr[clsPOSDBConstants.Vendor_Fld_VendorName].ToString();
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_Status))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_Status] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_Status] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_Status] = Convert.ToInt32((dr[clsPOSDBConstants.POHeader_Fld_Status].ToString() == "") ? "0" : dr[clsPOSDBConstants.POHeader_Fld_Status].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_isFTPUsed))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_isFTPUsed] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_isFTPUsed] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_isFTPUsed] = Convert.ToInt32((dr[clsPOSDBConstants.POHeader_Fld_isFTPUsed].ToString() == "") ? "0" : dr[clsPOSDBConstants.POHeader_Fld_isFTPUsed].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_AckStatus))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_AckStatus] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_AckStatus] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_AckStatus] = dr[clsPOSDBConstants.POHeader_Fld_AckStatus].ToString();
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_AckType))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_AckType] == DBNull.Value)
                        row[clsPOSDBConstants.POHeader_Fld_AckType] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_AckType] = dr[clsPOSDBConstants.POHeader_Fld_AckType].ToString();
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_AckDate))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_AckDate].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_AckDate] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_AckDate] = Convert.ToDateTime(dr[clsPOSDBConstants.POHeader_Fld_AckDate].ToString());
                }

                //Added By SRT(Abhishek) Date : 01/10/2009
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_PrimePOrderID))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_PrimePOrderID].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_PrimePOrderID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_PrimePOrderID] = Convert.ToInt32(dr[clsPOSDBConstants.POHeader_Fld_PrimePOrderID].ToString());
                }
                //End OF Added By SRT(Abhishek) Date : 01/10/2009

                //Added By Ravindra(Quicsolv) 16 Jan 2013
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_RefOrderNo))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_RefOrderNo].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_RefOrderNo] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_RefOrderNo] = Convert.ToString(dr[clsPOSDBConstants.POHeader_Fld_RefOrderNo].ToString());
                }
                //End OF Added By Ravindra(Quicsolv) 16 Jan 2013

                //Added By SRT(Gaurav) Date : 04/07/2009
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_Description))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_Description].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_Description] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.POHeader_Fld_Description].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_Flagged))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_Flagged].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_Flagged] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_Flagged] = Convert.ToBoolean(dr[clsPOSDBConstants.POHeader_Fld_Flagged].ToString());
                }
                //End Of Added By SRT(Gaurav)
                //End Validation update by PRASHANT(SRT) Date:6-7-2009
                //Added By shitaljit to store file type on 17 May 13
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_AckFileType))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_AckFileType].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_AckFileType] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_AckFileType] = Convert.ToString(dr[clsPOSDBConstants.POHeader_Fld_AckFileType].ToString());
                }

                #region Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_ProcessedBy))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_ProcessedBy].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_ProcessedBy] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_ProcessedBy] = Convert.ToString(dr[clsPOSDBConstants.POHeader_Fld_ProcessedBy].ToString());
                }
                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_ProcessedType))
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_ProcessedType].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_ProcessedType] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_ProcessedType] = Convert.ToString(dr[clsPOSDBConstants.POHeader_Fld_ProcessedType].ToString());
                }
                #endregion

                if (dr.Table.Columns.Contains(clsPOSDBConstants.POHeader_Fld_TransTypeCode))  //PRIMEPOS-2901 05-Nov-2020 JY Added
                {
                    if (dr[clsPOSDBConstants.POHeader_Fld_TransTypeCode].ToString().Trim() == "")
                        row[clsPOSDBConstants.POHeader_Fld_TransTypeCode] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.POHeader_Fld_TransTypeCode] = Convert.ToString(dr[clsPOSDBConstants.POHeader_Fld_TransTypeCode].ToString());
                }
                this.AddRow(row);
            }
		}
		
		#endregion 
		public override DataTable Clone() 
		{
			POHeaderTable cln = (POHeaderTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new POHeaderTable();
		}

		internal void InitVars() 
		{
			this.colOrderID = this.Columns[clsPOSDBConstants.POHeader_Fld_OrderID];
			this.colOrderDate= this.Columns[clsPOSDBConstants.POHeader_Fld_OrderDate];
			this.colExptDelvDate= this.Columns[clsPOSDBConstants.POHeader_Fld_ExptDelvDate];
			this.colOrderNo = this.Columns[clsPOSDBConstants.POHeader_Fld_OrderNo];
			this.colVendorCode= this.Columns[clsPOSDBConstants.Vendor_Fld_VendorCode];
			this.colVendorID= this.Columns[clsPOSDBConstants.POHeader_Fld_VendorID];
			this.colVendorName= this.Columns[clsPOSDBConstants.Vendor_Fld_VendorName];
			this.colStatus = this.Columns[clsPOSDBConstants.POHeader_Fld_Status];
			
			this.colisFTPUsed = this.Columns[clsPOSDBConstants.POHeader_Fld_isFTPUsed];
			this.colAckStatus = this.Columns[clsPOSDBConstants.POHeader_Fld_AckStatus];
			this.colAckType = this.Columns[clsPOSDBConstants.POHeader_Fld_AckType];
			this.colAckDate = this.Columns[clsPOSDBConstants.POHeader_Fld_AckDate];

           //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
           //Coulomns Added for VendorInterface
            this.colPrimePOOrderID = this.Columns[clsPOSDBConstants.POHeader_Fld_PrimePOrderID];
            //this.colIsMaxReached = this.Columns[clsPOSDBConstants.POHeader_Fld_IsMaxReached];
           //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
           //Added By SRT(Gaurav) Date : 04-07-2009
            this.colDescription = this.Columns[clsPOSDBConstants.POHeader_Fld_Description];
            this.colFlagged = this.Columns[clsPOSDBConstants.POHeader_Fld_Flagged];
           //End Of Added By SRT(Gaurav)
            //added by atul 22-oct-2010
            this.colInvoiceDate = this.Columns[clsPOSDBConstants.POHeader_Fld_InvoiceDate];
            this.colInvoiceNumber = this.Columns[clsPOSDBConstants.POHeader_Fld_InvoiceNumber];
            //End of added by atul 22-oct-2010
            //Added By Ravindra (QuicSolv) 16 Jan 2013
            this.colRefOrderNo = this.Columns[clsPOSDBConstants.POHeader_Fld_RefOrderNo]; 
            //End of Added by Ravindra(Quicsolv) 16 Jan 2013
            this.colAckFileType = this.Columns[clsPOSDBConstants.POHeader_Fld_AckFileType];//Added By shitaljit to store file type on 17 May 13
            this.colProcessedBy = this.Columns[clsPOSDBConstants.POHeader_Fld_ProcessedBy]; //Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added 
            this.colProcessedType = this.Columns[clsPOSDBConstants.POHeader_Fld_ProcessedType]; //Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added 
            this.colTransTypeCode = this.Columns[clsPOSDBConstants.POHeader_Fld_TransTypeCode]; //PRIMEPOS-2901 05-Nov-2020 JY Added
        }
		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
            try
            {
                this.colOrderID = new DataColumn(clsPOSDBConstants.POHeader_Fld_OrderID, typeof(System.Int32), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colOrderID);
                this.colOrderID.AllowDBNull = true;

                this.colOrderNo = new DataColumn(clsPOSDBConstants.POHeader_Fld_OrderNo, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colOrderNo);
                this.colOrderNo.AllowDBNull = true;

                this.colOrderDate = new DataColumn(clsPOSDBConstants.POHeader_Fld_OrderDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colOrderDate);
                this.colOrderDate.AllowDBNull = true;

                this.colExptDelvDate = new DataColumn(clsPOSDBConstants.POHeader_Fld_ExptDelvDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colExptDelvDate);
                this.colExptDelvDate.AllowDBNull = true;

                this.colVendorID = new DataColumn(clsPOSDBConstants.POHeader_Fld_VendorID, typeof(System.Int32), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colVendorID);
                this.colVendorID.AllowDBNull = true;

                this.colVendorCode = new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorCode, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colVendorCode);
                this.colVendorCode.AllowDBNull = true;

                this.colVendorName = new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorName, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colVendorName);
                this.colVendorName.AllowDBNull = true;

                this.colStatus = new DataColumn(clsPOSDBConstants.POHeader_Fld_Status, typeof(System.Int32), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colStatus);
                this.colStatus.AllowDBNull = true;

                this.colisFTPUsed = new DataColumn(clsPOSDBConstants.POHeader_Fld_isFTPUsed, typeof(System.Int32), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colisFTPUsed);
                this.colisFTPUsed.AllowDBNull = true;

                this.colAckStatus = new DataColumn(clsPOSDBConstants.POHeader_Fld_AckStatus, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colAckStatus);
                this.colAckStatus.AllowDBNull = true;

                this.colAckType = new DataColumn(clsPOSDBConstants.POHeader_Fld_AckType, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colAckType);
                this.colAckType.AllowDBNull = true;

                this.colAckDate = new DataColumn(clsPOSDBConstants.POHeader_Fld_AckDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colAckDate);
                this.colAckDate.AllowDBNull = true;

                //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
                //Coulomns Added for VendorInterface

                this.colPrimePOOrderID = new DataColumn(clsPOSDBConstants.POHeader_Fld_PrimePOrderID, typeof(System.Int32), null, System.Data.MappingType.Element);
                this.Columns.Add(this.PrimePOrderId);
                this.PrimePOrderId.AllowDBNull = true;

                //this.colIsMaxReached = new DataColumn(clsPOSDBConstants.POHeader_Fld_IsMaxReached,typeof(System.Boolean), null, System.Data.MappingType.Element);
                //this.Columns.Add(this.colIsMaxReached);
                //this.colIsMaxReached.AllowDBNull = true;

                //Added By Ravindra(QuicSolv) 16 jan 2013
                //Coulomns Added for OrderReference

                this.colRefOrderNo = new DataColumn(clsPOSDBConstants.POHeader_Fld_RefOrderNo, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.RefOrderNo);
                this.RefOrderNo.AllowDBNull = true;
                //

                //End of Added By Ravindra(QuicSolv) 16 jan 2013
                //Added By SRT(Gaurav) Date : 04/07/2009
                this.colDescription = new DataColumn(clsPOSDBConstants.POHeader_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colDescription);
                this.colDescription.AllowDBNull = true;

                this.colFlagged = new DataColumn(clsPOSDBConstants.POHeader_Fld_Flagged, typeof(System.Boolean), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colFlagged);
                this.colFlagged.AllowDBNull = true;
                //End Of Added By SRT(Gaurav)
                //added by atul 22-oct-2010
                this.colInvoiceDate = new DataColumn(clsPOSDBConstants.POHeader_Fld_InvoiceDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colInvoiceDate);
                this.colInvoiceDate.AllowDBNull = true;

                this.colInvoiceNumber = new DataColumn(clsPOSDBConstants.POHeader_Fld_InvoiceNumber, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colInvoiceNumber);
                this.colInvoiceNumber.AllowDBNull = true;
                //End of added by atul 22-oct-2010

                //Added By shitaljit to store file type on 17 May 13
                this.colAckFileType = new DataColumn(clsPOSDBConstants.POHeader_Fld_AckFileType, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colAckFileType);
                this.colAckFileType.AllowDBNull = true;
                //END

                #region Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added
                this.colProcessedBy = new DataColumn(clsPOSDBConstants.POHeader_Fld_ProcessedBy, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colProcessedBy);
                this.colProcessedBy.AllowDBNull = true;
                this.colProcessedType = new DataColumn(clsPOSDBConstants.POHeader_Fld_ProcessedType, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colProcessedType);
                this.colProcessedType.AllowDBNull = true;
                #endregion

                #region PRIMEPOS-2901 05-Nov-2020 JY Added
                this.colTransTypeCode = new DataColumn(clsPOSDBConstants.POHeader_Fld_TransTypeCode, typeof(System.String), null, System.Data.MappingType.Element);
                this.Columns.Add(this.colTransTypeCode);
                this.colTransTypeCode.AllowDBNull = true;
                #endregion

                this.PrimaryKey = new DataColumn[] { this.colOrderID };
            }
            catch(Exception ex){}
		}

		public  POHeaderRow NewPOHeaderRow() 
		{
			return (POHeaderRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new POHeaderRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(POHeaderRow);
		}
	}
}
