
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TransDetailRXTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colRXDetailID;
        private DataColumn colTransDetailID;
		private DataColumn colDateFilled;
		
		private DataColumn colRXNo;
		private DataColumn colDrugNDC;
        private DataColumn colPatientNo;
		private DataColumn colInsType;

		private DataColumn colTransType;
		
		private DataColumn colNRefill;
        private DataColumn colPatType;
        //added by atul 07-jan-2011
        private DataColumn colCounsellingReq;
        private DataColumn colEzcap;		
		/// <summary>
		/// Added by Manoj - 1/2/2015 fix return bug, no return ID stored
		/// </summary>
		private DataColumn colReturnTransDetailID;
		//end of added by atul 07-jan-2011
		private DataColumn colDelivery; //PRIMEPOS-3008 30-Sep-2021 JY Added
		private DataColumn colPartialFillNo;

		#region Constructors 
		internal TransDetailRXTable() : base(clsPOSDBConstants.TransDetailRX_tbl) { this.InitClass(); }
		internal TransDetailRXTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties

		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public TransDetailRXRow this[int index] 
		{
			get 
			{
				return ((TransDetailRXRow)(this.Rows[index]));
			}
		}

		public DataColumn RXDetailID 
		{
			get 
			{
				return this.colRXDetailID;
			}
			set
			{
				this.colRXDetailID=value;
			}
		}

        public DataColumn TransDetailID
        {
            get
            {
                return this.colTransDetailID;
            }
            set
            {
                this.colTransDetailID = value;
            }
        }

		public DataColumn DateFilled
		{
			get 
			{
				return this.colDateFilled;
			}
		}

		public DataColumn RXNo
		{
			get 
			{
				return this.colRXNo;
			}
		}

		public DataColumn DrugNDC
		{
			get 
			{
				return this.colDrugNDC;
			}
		}

        public DataColumn PatientNo
        {
            get
            {
                return this.colPatientNo;
            }
        }

		public DataColumn InsType
		{
			get 
			{
				return this.colInsType;
			}
		}

        public DataColumn PatType
        {
            get
            {
                return this.colPatType;
            }
        }

		public DataColumn TransType
		{
			get 
			{
				return this.colTransType;
			}
		}
		
		public DataColumn NRefill
		{
			get 
			{
				return this.colNRefill;
			}
		}

		public DataColumn PartialFillNo
		{
			get
			{
				return this.colPartialFillNo;
			}
		}

		//added by atul 07-jan-2011
		public DataColumn CounsellingReq
        {
            get
            {
                return this.colCounsellingReq;
            }
        }
        public DataColumn Ezcap
        {
            get
            {
                return this.colEzcap;
            }
        }	

		/// <summary>
		/// Added by Manoj - return ID
		/// </summary>
		public DataColumn ReturnTransDetailID
        {
            get
            {
                return this.colReturnTransDetailID;
            }
        }
		//end of added by atul 07-jan-2011

		public DataColumn Delivery
		{
			get
			{
				return this.colDelivery;
			}
		}
		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(TransDetailRXRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(TransDetailRXRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.RXDetailID) == null) 
			{
				this.Rows.Add(row);
				if(preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public TransDetailRXRow AddRow( System.Int32 iTransDetailID
										, System.DateTime  dDateFilled 
										, System.Int64  iRXNo
                                        , System.String sDrugNDC
                                        , System.Int64 iPatientNo
                                        , System.String sInsType
                                        , System.String sPatType
                                        , System.Int32 iNRefill
                                        , System.String sCounsellingReq // added by atul 07-jan-2011
                                        , System.String sEzcap
										, System.String sDelivery   //PRIMEPOS-3008 30-Sep-2021 JY Added
										, System.Int32 iPartialFillNo
			) // end of added by atul 07-jan-2011
		{


			TransDetailRXRow row = (TransDetailRXRow)this.NewRow();
			//row.ItemArray = new object[] {DeptId,DeptCode,DeptName,Discount,IsTaxable , SaleStartDate , SaleEndDate ,TaxId , SalePrice};
			row.TransDetailID=iTransDetailID;
            row.RXDetailID = iTransDetailID;
			row.DateFilled=dDateFilled;
			row.RXNo=iRXNo;
            row.DrugNDC = sDrugNDC;
            row.PatientNo = iPatientNo;
            row.InsType = sInsType;
            row.NRefill = iNRefill;
            row.PatType = sPatType;
            // added by atul 07-jan-2011
            row.CounsellingReq = sCounsellingReq;
            row.Ezcap = sEzcap;
			// end of added by atul 07-jan-2011
			row.Delivery = sDelivery;   //PRIMEPOS-3008 30-Sep-2021 JY Added
			row.PartialFillNo = iPartialFillNo;
			this.Rows.Add(row); 
			return row;
		}

		public TransDetailRXRow GetRowByID(System.Int32 RXDetailID) 
		{
			return (TransDetailRXRow)this.Rows.Find(new object[] {RXDetailID});
		}

        public TransDetailRXRow GetRow(System.Int64 RXNo,System.Int32 refillNo)
        {
            TransDetailRXRow oReturnRow=null;
            foreach (TransDetailRXRow orow in this.Rows)
            {
                if (orow.RXNo == RXNo && orow.NRefill == refillNo)
                {
                    oReturnRow = orow;
                    break;
                }
            }
            return oReturnRow;
        }

		public TransDetailRXRow GetRow(System.Int64 RXNo, System.Int32 refillNo, Int32 iPartialFillNo)
		{
			TransDetailRXRow oReturnRow = null;
			foreach (TransDetailRXRow orow in this.Rows)
			{
				if (orow.RXNo == RXNo && orow.NRefill == refillNo && orow.PartialFillNo == iPartialFillNo)
				{
					oReturnRow = orow;
					break;
				}
			}
			return oReturnRow;
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			TransDetailRXRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (TransDetailRXRow)this.NewRow();

				if (dr[clsPOSDBConstants.TransDetailRX_Fld_RXDetailID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetailRX_Fld_RXDetailID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetailRX_Fld_RXDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetailRX_Fld_RXDetailID].ToString()=="")?"0":dr[clsPOSDBConstants.TransDetailRX_Fld_RXDetailID].ToString());

                if (dr[clsPOSDBConstants.TransDetailRX_Fld_TransDetailID] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetailRX_Fld_TransDetailID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransDetailRX_Fld_TransDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetailRX_Fld_TransDetailID].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransDetailRX_Fld_TransDetailID].ToString());

				if (dr[clsPOSDBConstants.TransDetailRX_Fld_DateFilled] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetailRX_Fld_DateFilled] = DBNull.Value;
				else
					if (dr[clsPOSDBConstants.TransDetailRX_Fld_DateFilled].ToString().Trim()=="") 
						row[clsPOSDBConstants.TransDetailRX_Fld_DateFilled]= Convert.ToDateTime(System.DateTime.MinValue.ToString());
					else
						row[clsPOSDBConstants.TransDetailRX_Fld_DateFilled]= Convert.ToDateTime(dr[clsPOSDBConstants.TransDetailRX_Fld_DateFilled].ToString());

				string strField=clsPOSDBConstants.TransDetailRX_Fld_RXNo;
				if (dr[strField] == DBNull.Value) 
					row[strField] = DBNull.Value;
				else
					row[strField] = Convert.ToInt32((dr[strField].ToString()=="")? "0":dr[strField].ToString());
				
				if (dr[clsPOSDBConstants.TransDetailRX_Fld_DrugNDC] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetailRX_Fld_DrugNDC] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetailRX_Fld_DrugNDC] = ((dr[clsPOSDBConstants.TransDetailRX_Fld_DrugNDC].ToString()=="")? "":dr[clsPOSDBConstants.TransDetailRX_Fld_DrugNDC].ToString());

				if (dr[clsPOSDBConstants.TransDetailRX_Fld_InsType] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetailRX_Fld_InsType] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetailRX_Fld_InsType] = dr[clsPOSDBConstants.TransDetailRX_Fld_InsType].ToString();

				strField=clsPOSDBConstants.TransDetailRX_Fld_NRefill;
				if (dr[strField] == DBNull.Value) 
					row[strField] = DBNull.Value;
				else
					row[strField] = Convert.ToInt32((dr[strField].ToString()=="")? "0":dr[strField].ToString());

                strField = clsPOSDBConstants.TransDetailRX_Fld_PatientNo;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt64((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                if (dr[clsPOSDBConstants.TransDetailRX_Fld_PatType] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetailRX_Fld_PatType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransDetailRX_Fld_PatType] = dr[clsPOSDBConstants.TransDetailRX_Fld_PatType].ToString();
                // added by atul 07-jan-2011
                if (dr[clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq] = dr[clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq].ToString();

                if (dr[clsPOSDBConstants.TransDetailRX_Fld_Ezcap] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetailRX_Fld_Ezcap] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransDetailRX_Fld_Ezcap] = dr[clsPOSDBConstants.TransDetailRX_Fld_Ezcap].ToString();

				strField = clsPOSDBConstants.TransDetailRX_Fld_PartialFillNo;
				if (dr[strField] == DBNull.Value)
					row[strField] = DBNull.Value;
				else
					row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

				// end of added by atul 07-jan-2011
				this.AddRow(row);
			}
		}
		
		#endregion 

		public override DataTable Clone() 
		{
			TransDetailRXTable cln = (TransDetailRXTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new TransDetailRXTable();
		}

		internal void InitVars() 
		{
			this.colRXDetailID = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_RXDetailID];
			this.colDateFilled= this.Columns[clsPOSDBConstants.TransDetailRX_Fld_DateFilled];
			
			this.colDrugNDC= this.Columns[clsPOSDBConstants.TransDetailRX_Fld_DrugNDC];
			this.colRXNo= this.Columns[clsPOSDBConstants.TransDetailRX_Fld_RXNo];
            this.colPatientNo = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_PatientNo];
			this.colInsType= this.Columns[clsPOSDBConstants.TransDetailRX_Fld_InsType];
			this.colNRefill=this.Columns[clsPOSDBConstants.TransDetailRX_Fld_NRefill];
            this.colTransDetailID = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_TransDetailID];
            this.colPatType = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_PatType];
            // added by atul 07-jan-2011
            this.colCounsellingReq = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq];
            this.colEzcap = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_Ezcap];
            // end of added by atul 07-jan-2011
            //Added by Manoj 1/2/2015 - fix return issue, no record stored.
            this.colReturnTransDetailID = this.Columns[clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId];
			this.colDelivery = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_Delivery];  //PRIMEPOS-3008 30-Sep-2021 JY Added
			this.colPartialFillNo = this.Columns[clsPOSDBConstants.TransDetailRX_Fld_PartialFillNo];
		}

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colRXDetailID = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_RXDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRXDetailID);
			this.colRXDetailID.AllowDBNull = true;

			this.colDateFilled= new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_DateFilled,typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDateFilled);
			this.colDateFilled.AllowDBNull = true;

			this.colRXNo= new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_RXNo,typeof(System.Int64), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRXNo);
			this.colRXNo.AllowDBNull = true;

			this.colDrugNDC= new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_DrugNDC, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDrugNDC);
			this.colDrugNDC.AllowDBNull = true;

			this.colInsType= new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_InsType, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colInsType);
			this.colInsType.AllowDBNull = true;

			this.colNRefill= new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_NRefill, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colNRefill);
			this.colNRefill.AllowDBNull = true;

            this.colPatientNo = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_PatientNo, typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPatientNo);
            this.colPatientNo.AllowDBNull = true;

            this.colTransDetailID = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_TransDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransDetailID);
            this.colTransDetailID.AllowDBNull = true;

            this.colPatType = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_PatType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPatType);
            this.colPatType.AllowDBNull = true;

            // added by atul 07-jan-2011
            this.colCounsellingReq = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCounsellingReq);
            this.colCounsellingReq.AllowDBNull = true;

            this.colEzcap = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_Ezcap, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEzcap);
            this.colEzcap.AllowDBNull = true;
            // end of added by atul 07-jan-2011

            //Added by Manoj 1/2/2015 - Return ID
            this.colReturnTransDetailID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colReturnTransDetailID);
            this.colReturnTransDetailID.AllowDBNull = true;

			//PRIMEPOS-3008 30-Sep-2021 JY Added
			this.colDelivery = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_Delivery, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDelivery);
			this.colDelivery.AllowDBNull = true;

			this.colPartialFillNo = new DataColumn(clsPOSDBConstants.TransDetailRX_Fld_PartialFillNo, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colPartialFillNo);
			this.colPartialFillNo.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] { this.colRXDetailID };
		}

		public  TransDetailRXRow NewTransDetailRXRow() 
		{
			return (TransDetailRXRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new TransDetailRXRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(TransDetailRXRow);
		}

	}
}
