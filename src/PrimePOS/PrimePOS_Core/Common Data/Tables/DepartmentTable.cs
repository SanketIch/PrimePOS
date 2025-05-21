
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class DepartmentTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colDeptId;
		private DataColumn colDeptCode;
		private DataColumn colDeptName;
		private DataColumn colDiscount;
        private DataColumn colSaleDiscount;
		private DataColumn colIsTaxable;

		private DataColumn colIsDiscountable;
		private DataColumn colSaleStartDate;
		private DataColumn colSaleEndDate;
		private DataColumn colTaxId;
		private DataColumn colSalePrice;
		private DataColumn colTaxCode;
		private DataColumn colTaxDescription;
        private DataColumn colPointsPerDollar;  //Sprint-18 - 2041 22-Oct-2014 JY Added PointsPerDollar

		#region Constants
		private const String _TableName = "department";
		#endregion
		#region Constructors 
		internal DepartmentTable() : base(_TableName) { this.InitClass(); }
		internal DepartmentTable(DataTable table) : base(table.TableName) {}
		#endregion
		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public DepartmentRow this[int index] 
		{
			get 
			{
				return ((DepartmentRow)(this.Rows[index]));
			}
		}

		public DataColumn DeptId 
		{
			get 
			{
				return this.colDeptId;
			}
		}

		public DataColumn DeptCode
		{
			get 
			{
				return this.colDeptCode;
			}
		}

		public DataColumn DeptName 
		{
			get 
			{
				return this.colDeptName;
			}
		}

		public DataColumn Discount 
		{
			get 
			{
				return this.colDiscount;
			}
		}

        public DataColumn SaleDiscount
        {
            get
            {
                return this.colSaleDiscount;
            }
        }

		public DataColumn IsTaxable 
		{
			get 
			{
				return this.colIsTaxable;
			}
		}

		public DataColumn IsDiscountable 
		{
			get 
			{
				return this.colIsDiscountable;
			}
		}

		public DataColumn SaleStartDate 
		{
			get 
			{
				return this.colSaleStartDate;
			}
		}

		public DataColumn SaleEndDate 
		{
			get 
			{
				return this.colSaleEndDate;
			}
		}

		public DataColumn TaxId 
		{
			get 
			{
				return this.colTaxId;
			}
		}

		public DataColumn TaxCode 
		{
			get 
			{
				return this.colTaxCode;
			}
		}

		public DataColumn TaxDescription 
		{
			get 
			{
				return this.colTaxDescription;
			}
		}


		public DataColumn SalePrice 
		{
			get 
			{
				return this.colSalePrice;
			}
		}

        //Sprint-18 - 2041 26-Oct-2014 JY Added colPointsPerDollar
        public DataColumn PointsPerDollar
        {
            get
            {
                return this.colPointsPerDollar;
            }
        }

		#endregion //Properties
		#region Add and Get Methods 

		public  void AddRow(DepartmentRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(DepartmentRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.DeptCode) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public DepartmentRow AddRow(System.Int32 DeptId 
										,System.String DeptCode
										,System.String DeptName
										,System.Decimal Discount
                                        , System.Decimal SaleDiscount
										,System.Boolean IsTaxable 
										, System.DateTime  SaleStartDate 
										,System.DateTime  SaleEndDate 
										, System.Int32  TaxId 
										, System.Decimal SalePrice
                                        , System.Int32 PointsPerDollar) 
		{
			DepartmentRow row = (DepartmentRow)this.NewRow();
			//row.ItemArray = new object[] {DeptId,DeptCode,DeptName,Discount,IsTaxable , SaleStartDate , SaleEndDate ,TaxId , SalePrice};
			row.DeptID=DeptId;
			row.DeptCode=DeptCode;
			row.DeptName=DeptName;
			row.Discount=Discount;
            row.SaleDiscount = SaleDiscount;
			row.IsTaxable=IsTaxable;
			row.SaleStartDate=SaleStartDate;
			row.SaleEndDate=SaleEndDate;
			row.TaxId=TaxId;
			row.SalePrice=SalePrice;
            row.PointsPerDollar = PointsPerDollar;
			
			this.Rows.Add(row);
			return row;
		}

		public DepartmentRow GetRowByID(System.String DeptCode) 
		{
			return (DepartmentRow)this.Rows.Find(new object[] {DeptCode});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			DepartmentRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (DepartmentRow)this.NewRow();

				if (dr[clsPOSDBConstants.Department_Fld_DeptID] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_DeptID] = DBNull.Value;
				else
					row[clsPOSDBConstants.Department_Fld_DeptID] = Convert.ToInt32((dr[clsPOSDBConstants.Department_Fld_DeptID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.Department_Fld_DeptCode] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_DeptCode] = DBNull.Value;
				else
					row[clsPOSDBConstants.Department_Fld_DeptCode] = Convert.ToString(dr[1].ToString());

				if (dr[clsPOSDBConstants.Department_Fld_DeptName] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_DeptName] = DBNull.Value;
				else
					row[clsPOSDBConstants.Department_Fld_DeptName] = Convert.ToString(dr[clsPOSDBConstants.Department_Fld_DeptName].ToString());

				if (dr[clsPOSDBConstants.Department_Fld_Discount] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_Discount] = DBNull.Value;
				else
					row[clsPOSDBConstants.Department_Fld_Discount] = Convert.ToDecimal((dr[clsPOSDBConstants.Department_Fld_Discount].ToString()=="")? "0":dr[clsPOSDBConstants.Department_Fld_Discount].ToString());

                if (dr[clsPOSDBConstants.Department_Fld_SaleDiscount] == DBNull.Value)
                    row[clsPOSDBConstants.Department_Fld_SaleDiscount] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Department_Fld_SaleDiscount] = Convert.ToDecimal((dr[clsPOSDBConstants.Department_Fld_SaleDiscount].ToString() == "") ? "0" : dr[clsPOSDBConstants.Department_Fld_SaleDiscount].ToString());

				if (dr[clsPOSDBConstants.Department_Fld_IsTaxable] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_IsTaxable] = DBNull.Value;
				else
					row[clsPOSDBConstants.Department_Fld_IsTaxable] = Convert.ToBoolean((dr[clsPOSDBConstants.Department_Fld_IsTaxable].ToString()=="")? "false":dr[clsPOSDBConstants.Department_Fld_IsTaxable].ToString());

				if (dr[clsPOSDBConstants.Department_Fld_SaleStartDate] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_SaleStartDate] = DBNull.Value;
				else
					if (dr[clsPOSDBConstants.Department_Fld_SaleStartDate].ToString().Trim()=="") 
						row[clsPOSDBConstants.Department_Fld_SaleStartDate]= Convert.ToDateTime(System.DateTime.MinValue.ToString());
					else
						row[clsPOSDBConstants.Department_Fld_SaleStartDate]= Convert.ToDateTime(dr[clsPOSDBConstants.Department_Fld_SaleStartDate].ToString());

					//row[5] =Convert.ToDateTime( (dr[5].ToString()=="") ? System.DateTime.MinValue.ToString() : dr[5].ToString());

				if (dr[clsPOSDBConstants.Department_Fld_SaleEndDate] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_SaleEndDate] = DBNull.Value;
				else
					if (dr[clsPOSDBConstants.Department_Fld_SaleEndDate].ToString().Trim()=="") 
						row[clsPOSDBConstants.Department_Fld_SaleEndDate]=Convert.ToDateTime(System.DateTime.MinValue.ToString());
					else
						row[clsPOSDBConstants.Department_Fld_SaleEndDate]=Convert.ToDateTime(dr[clsPOSDBConstants.Department_Fld_SaleEndDate].ToString());
					//row[6] = Convert.ToDateTime((dr[6].ToString()=="")? System.DateTime.MinValue.ToString() :dr[6].ToString());

				if (dr[clsPOSDBConstants.Department_Fld_SalePrice] == DBNull.Value) 
					row[clsPOSDBConstants.Department_Fld_SalePrice] = DBNull.Value;
				else
					row[clsPOSDBConstants.Department_Fld_SalePrice] = Convert.ToDecimal((dr[clsPOSDBConstants.Department_Fld_SalePrice].ToString()=="")? "0":dr[clsPOSDBConstants.Department_Fld_SalePrice].ToString());

				string strField=clsPOSDBConstants.Department_Fld_TaxID	;
				if (dr[strField] == DBNull.Value) 
					row[strField] = DBNull.Value;
				else
					row[strField] = Convert.ToInt32((dr[strField].ToString()=="")? "0":dr[strField].ToString());
				
				if (dr[clsPOSDBConstants.TaxCodes_Fld_TaxCode] == DBNull.Value) 
					row[clsPOSDBConstants.TaxCodes_Fld_TaxCode] = DBNull.Value;
				else
					row[clsPOSDBConstants.TaxCodes_Fld_TaxCode] = ((dr[clsPOSDBConstants.TaxCodes_Fld_TaxCode].ToString()=="")? "":dr[clsPOSDBConstants.TaxCodes_Fld_TaxCode].ToString());

				//if (dr[clsPOSDBConstants.Department_Fld_UserID] == DBNull.Value) 
				//	row[clsPOSDBConstants.Department_Fld_UserID] = DBNull.Value;
				//else
				//	row[clsPOSDBConstants.Department_Fld_UserID] = dr[clsPOSDBConstants.Department_Fld_UserID].ToString();
				
				if (dr[clsPOSDBConstants.TaxCodes_Fld_Description] == DBNull.Value) 
					row[clsPOSDBConstants.TaxCodes_Fld_Description] = DBNull.Value;
				else
					row[clsPOSDBConstants.TaxCodes_Fld_Description] = dr[clsPOSDBConstants.TaxCodes_Fld_Description].ToString();

                //Sprint-18 - 2041 26-Oct-2014 JY added clsPOSDBConstants.Department_Fld_PointsPerDollar
                if (dr[clsPOSDBConstants.Department_Fld_PointsPerDollar] == DBNull.Value)
                    row[clsPOSDBConstants.Department_Fld_PointsPerDollar] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Department_Fld_PointsPerDollar] = Convert.ToInt32((dr[clsPOSDBConstants.Department_Fld_PointsPerDollar].ToString() == "") ? "0" : dr[clsPOSDBConstants.Department_Fld_PointsPerDollar].ToString());

				this.AddRow(row);
			}
		}
		
		#endregion 
		public override DataTable Clone() 
		{
			DepartmentTable cln = (DepartmentTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new DepartmentTable();
		}

		internal void InitVars() 
		{
			this.colDeptId = this.Columns[clsPOSDBConstants.Department_Fld_DeptID];
			this.colDeptCode = this.Columns[clsPOSDBConstants.Department_Fld_DeptCode];
			this.colDeptName = this.Columns[clsPOSDBConstants.Department_Fld_DeptName];
			this.colDiscount = this.Columns[clsPOSDBConstants.Department_Fld_Discount];
            this.colSaleDiscount = this.Columns[clsPOSDBConstants.Department_Fld_SaleDiscount];
			this.colIsTaxable = this.Columns[clsPOSDBConstants.Department_Fld_IsTaxable];
			this.colSaleStartDate = this.Columns[clsPOSDBConstants.Department_Fld_SaleStartDate];
			this.colSaleEndDate= this.Columns[clsPOSDBConstants.Department_Fld_SaleEndDate];
			this.colTaxId= this.Columns[clsPOSDBConstants.Department_Fld_TaxID];
			this.colSalePrice= this.Columns[clsPOSDBConstants.Department_Fld_SalePrice];

			this.colTaxCode= this.Columns[clsPOSDBConstants.TaxCodes_Fld_TaxCode];
			this.colTaxDescription= this.Columns[clsPOSDBConstants.TaxCodes_Fld_Description];
            this.colPointsPerDollar = this.Columns[clsPOSDBConstants.Department_Fld_PointsPerDollar];   //Sprint-18 - 2041 26-Oct-2014 JY Added
        }

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colDeptId = new DataColumn("DeptId", typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDeptId);
			this.colDeptId.AllowDBNull = true;

			this.colDeptCode = new DataColumn("DeptCode", typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDeptCode);
			this.colDeptCode.AllowDBNull = false;

			this.colDeptName = new DataColumn("DeptName", typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDeptName);
			this.colDeptName.AllowDBNull = true;

			this.colDiscount = new DataColumn("Discount", typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDiscount);
			this.colDiscount.AllowDBNull = true;

            this.colSaleDiscount = new DataColumn("SaleDiscount", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSaleDiscount);
            this.colSaleDiscount.AllowDBNull = true;

			this.colIsTaxable = new DataColumn("IsTaxable", typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colIsTaxable);
			this.colIsTaxable.AllowDBNull = true;

			this.colIsDiscountable = new DataColumn("IsDiscountable", typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colIsDiscountable);
			this.colIsDiscountable.AllowDBNull = true;

			this.colSaleStartDate = new DataColumn("SaleStartDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSaleStartDate);
			this.colSaleStartDate.AllowDBNull = true;

			this.colSaleEndDate = new DataColumn("SaleEndDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSaleEndDate);
			this.colSaleEndDate.AllowDBNull = true;

			this.colTaxId = new DataColumn("TaxId", typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxId);
			this.colTaxId.AllowDBNull = true;

			this.colSalePrice = new DataColumn("SalePrice", typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSalePrice);
			this.colSalePrice.AllowDBNull = true;

			this.colTaxCode = new DataColumn("TaxCode", typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxCode);
			this.colTaxCode.AllowDBNull = true;

			this.colTaxDescription = new DataColumn("Description", typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxDescription);
			this.colTaxDescription.AllowDBNull = true;

            //Sprint-18 - 2041 26-Oct-2014 JY Added
            this.colPointsPerDollar = new DataColumn("PointsPerDollar", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPointsPerDollar);
            this.colPointsPerDollar.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.DeptCode};
		}
		public  DepartmentRow NewDepartmentRow() 
		{
			return (DepartmentRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new DepartmentRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(DepartmentRow);
		}
	}
}
