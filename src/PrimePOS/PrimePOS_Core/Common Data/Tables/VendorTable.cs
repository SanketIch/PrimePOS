
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;

	//         This class is used to define the shape of VendorTable.
	public class VendorTable : DataTable
	{

		private DataColumn colVendorId;
		private DataColumn colVendorcode;
		private DataColumn colVendorname;
		private DataColumn colAddress1;
		private DataColumn colAddress2;
		private DataColumn colCity;
		private DataColumn colState;
		private DataColumn colZip;
		private DataColumn colTelephoneno;
		private DataColumn colFaxno;
		private DataColumn colCellno;
		private DataColumn colUrl;
		private DataColumn colEmail;
		private DataColumn colIsActive;
        private DataColumn colIsAutoClose;

		private DataColumn colPriceQualifier;
		private DataColumn colUSEVICForEPO;
        private DataColumn colCostQualifier;

        private DataColumn colSalePriceQualifier;//14-Nov-2014 Ravindra added For SalePriceQualifier;

        private DataColumn colTimeToOrder;
        private DataColumn colPrimePOVendorCode;
        private DataColumn colUpdatePrice;
        private DataColumn colSalePriceUpdate;  //12-Nov-2014 JY added new field IsSalePriceUpdate
        private DataColumn colReduceSellingPrice;   //Sprint-21 - 2208 24-Jul-2015 JY Added

        private DataColumn colSendVendCostPrice;
        private DataColumn colProcess810;
        //AckPriceUpdate Added By Ravindra on 20 Feb 2013
        private DataColumn colAckPriceUpdate;
        //End Of Added By Ravindra on 20 Feb 2013
		#region Constants
		// The constant used for Vendor table. 
		private const String _TableName = clsPOSDBConstants.Vendor_tbl;
		#endregion
		#region Constructors 
		internal VendorTable() : base(_TableName) { this.InitClass(); }
		internal VendorTable(DataTable table) : base(table.TableName) {}
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

		public VendorRow this[int index] 
		{
			get 
			{
				return ((VendorRow)(this.Rows[index]));
			}
		}

		// Public Property DataColumn Vendorcode
		public DataColumn VendorId 
		{
			get 
			{
				return this.colVendorId;
			}
		}

		public DataColumn Vendorcode 
		{
			get 
			{
				return this.colVendorcode;
			}
		}

		// Public Property DataColumn Vendorname
		public DataColumn Vendorname 
		{
			get 
			{
				return this.colVendorname;
			}
		}

		// Public Property DataColumn Address1
		public DataColumn Address1 
		{
			get 
			{
				return this.colAddress1;
			}
		}

		// Public Property DataColumn Address2

		public DataColumn Address2 
		{
			get 
			{
				return this.colAddress2;
			}
		}

		// Public Property DataColumn City
		public DataColumn City 
		{
			get 
			{
				return this.colCity;
			}
		}

		// Public Property DataColumn State
		public DataColumn State 
		{
			get 
			{
				return this.colState;
			}
		}

		// Public Property DataColumn Zip
		public DataColumn Zip 
		{
			get 
			{
				return this.colZip;
			}
		}

		// Public Property DataColumn Telephoneno
		public DataColumn Telephoneno 
		{
			get 
			{
				return this.colTelephoneno;
			}
		}

		// Public Property DataColumn Faxno

		public DataColumn Faxno 
		{
			get 
			{
				return this.colFaxno;
			}
		}

		// Public Property DataColumn Cellno

		public DataColumn Cellno 
		{
			get 
			{
				return this.colCellno;
			}
		}

		// Public Property DataColumn Url
		public DataColumn Url 
		{
			get 
			{
				return this.colUrl;
			}
		}

		// Public Property DataColumn Email
		public DataColumn Email 
		{
			get 
			{
				return this.colEmail;
			}
		}

		public DataColumn IsActive 
		{
			get 
			{
				return this.colIsActive;
			}
		}
        //Added by Prashant(SRT) Date:1-06-2009
        public DataColumn IsAutoClose
        {
            get
            {
                return this.colIsAutoClose; 
            }
        }
        //End of Added by Prashant(SRT) Date:1-06-2009

        //Added By SRT(Abhishek) Date : 01/07/2009 Wed.

        public DataColumn USEVICForEPO
        {
            get
            {
                return this.colUSEVICForEPO;
            }
        }
        public DataColumn PrimePOVendorCode
        {
            get
            {
                return this.colPrimePOVendorCode;
            }
        }
        public DataColumn PriceQualifier
        {
            get
            {
                return this.colPriceQualifier;
            }
        }
        public DataColumn CostQualifier
        {
            get
            {
                return this.colCostQualifier;
            }
        }
        //4-Nov-2014 Ravindra added For SalePriceQualifier;
        public DataColumn SalePriceQualifier
        {
            get
            {
                return this.colSalePriceQualifier;
            }
        }

        public DataColumn TimeToOrder
        {
            get
            {
                return this.colTimeToOrder; 
            }
        }
        public DataColumn UpdatePrice
        {
            get
            {
                return this.colUpdatePrice;
            }
        }

        public DataColumn SendVendCostPrice
        {
            get
            {
                return this.colSendVendCostPrice;
            }
        }
        public DataColumn Process810
        {
            get { return this.colProcess810; }
        }
        //AckPriceUpdate Added By Ravindra on 20 Feb 2013
        public DataColumn AckPriceUpdate
        {
            get { return this.colAckPriceUpdate; }
        }

        //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.

        #region 12-Nov-2014 JY added new field IsSalePriceUpdate
        public DataColumn SalePriceUpdate
        {
            get
            {
                return this.colSalePriceUpdate;
            }
        }
        #endregion

        #region Sprint-21 - 2208 24-Jul-2015 JY Added
        public DataColumn ReduceSellingPrice
        {
            get
            {
                return this.colReduceSellingPrice;
            }
        }
        #endregion

        #endregion //Properties
        #region Add and Get Methods

        public  void AddRow(VendorRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(VendorRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.Vendorcode) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public  VendorRow AddRow(System.Int32 VendorId , System.String Vendorcode,System.String Vendorname,System.String Address1,System.String Address2,System.String City,System.String State,System.String Zip,System.String Telephoneno,System.String Faxno,System.String Cellno,
            System.String Url, System.String Email, System.Boolean IsActive, System.Boolean IsAutoClose, System.String PrimePOVendorCode, System.Boolean usedeviceorepo, System.String PriceQualifier, System.String CostQualifier, System.String TimeToOrder, System.Boolean updatePrice, System.Boolean vendCostPrice, System.Boolean process810, System.Boolean ackPriceUpdate, Boolean SalePriceUpdate, Boolean ReduceSellingPrice, System.String SalePriceQualifier)     //12-Nov-2014 JY added new field IsSalePriceUpdate    //Sprint-21 - 2208 24-Jul-2015 JY Added ReduceSellingPrice
		{
		
			VendorRow row = (VendorRow)this.NewRow();
			row.ItemArray = new object[] {VendorId,Vendorcode,Vendorname,Address1,Address2,City,State,Zip,Telephoneno,Faxno,Cellno,
											 Url,Email,IsActive,IsAutoClose,PrimePOVendorCode,usedeviceorepo,PriceQualifier,CostQualifier,TimeToOrder,updatePrice,vendCostPrice,process810,ackPriceUpdate,SalePriceUpdate, ReduceSellingPrice, SalePriceQualifier};   //12-Nov-2014 JY added new field IsSalePriceUpdate    //Sprint-21 - 2208 24-Jul-2015 JY Added ReduceSellingPrice
			this.Rows.Add(row);
			return row;
		}

		public VendorRow GetRowByID(System.String Vendorcode) 
		{
			return (VendorRow)this.Rows.Find(new object[] {Vendorcode});
		}

		public  void MergeTable(DataTable dt) 
		{ 
			//add any rows in the DataTable 
			VendorRow row;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    row = (VendorRow)this.NewRow();

                    if (dr[clsPOSDBConstants.Vendor_Fld_VendorId] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_VendorId] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_VendorId] = Convert.ToInt32(dr[clsPOSDBConstants.Vendor_Fld_VendorId].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_VendorCode] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_VendorCode] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_VendorCode] = dr[clsPOSDBConstants.Vendor_Fld_VendorCode].ToString();

                    if (dr[clsPOSDBConstants.Vendor_Fld_VendorName] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_VendorName] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_VendorName] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_VendorName].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_Address1] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_Address1] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_Address1] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_Address1].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_Address2] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_Address2] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_Address2] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_Address2].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_City] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_City] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_City] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_City].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_State] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_State] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_State] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_State].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_Zip] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_Zip] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_Zip] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_Zip].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_TelephoneNo] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_TelephoneNo] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_TelephoneNo] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_TelephoneNo].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_FaxNo] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_FaxNo] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_FaxNo] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_FaxNo].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_CellNo] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_CellNo] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_CellNo] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_CellNo].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_URL] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_URL] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_URL] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_URL].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_Email] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_Email] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_Email] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_Email].ToString());

                    row[clsPOSDBConstants.Vendor_Fld_IsActive] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_IsActive].ToString());

                    //Added by Prashant
                    if (dr.Table.Columns.Contains(clsPOSDBConstants.Vendor_Fld_IsAutoClose))
                    {
                        if (dr[clsPOSDBConstants.Vendor_Fld_IsAutoClose] == DBNull.Value)
                            row[clsPOSDBConstants.Vendor_Fld_IsAutoClose] = 1;
                        else
                            row[clsPOSDBConstants.Vendor_Fld_IsAutoClose] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_IsAutoClose].ToString());
                    }
                    //Added By SRT(Abhishek)  Date :01/07/2009      
                    //coulomns Added for VendorInterface

                    if (dr.Table.Columns.Contains(clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode))
                    {
                        if (dr[clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode] == DBNull.Value)
                            row[clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode] = DBNull.Value;
                        else
                            row[clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode] = dr[clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode].ToString();
                    }

                    row[clsPOSDBConstants.Vendor_Fld_USEVICForEPO] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_USEVICForEPO].ToString());

                    row[clsPOSDBConstants.Vendor_Fld_PriceQualifier] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_PriceQualifier].ToString());

                    row[clsPOSDBConstants.Vendor_Fld_CostQualifier] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_CostQualifier].ToString());

                    row[clsPOSDBConstants.Vendor_Fld_SalePriceQualifier] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_SalePriceQualifier].ToString());//4-Nov-2014 Ravindra added For SalePricetQualifier;

                    row[clsPOSDBConstants.Vendor_Fld_TimeToOrder] = Convert.ToString(dr[clsPOSDBConstants.Vendor_Fld_TimeToOrder].ToString());

                    // row[clsPOSDBConstants.Vendor_Fld_UpdatePrice] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_UpdatePrice].ToString());

                    if (dr[clsPOSDBConstants.Vendor_Fld_UpdatePrice] == DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_UpdatePrice] = true;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_UpdatePrice] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_UpdatePrice].ToString());

                    //Updated by SRT(Gaurav) Date: 03-Jul-2009
                    //Checked in datatable for column exist or not.
                    if (dt.Columns.Contains(clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice))
                    {
                        if (dr[clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice] == DBNull.Value)
                            row[clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice] = true;
                        else
                            row[clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice].ToString());
                    }
                    //End of Updated by SRT(Gaurav)
                    //End Of  Added By SRT(Abhishek)  Date :01/07/2009
                    
                    //Added by Atul joshi on 29-10-2010
                    if (dr[clsPOSDBConstants.Vendor_Fld_Process810] ==System.DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_Process810] = false;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_Process810] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_Process810]);   
                    //AckPriceUpdate Added by Ravindra on 20 Feb 2013
                    if (dr[clsPOSDBConstants.Vendor_Fld_AckPriceUpdate] == System.DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_AckPriceUpdate] = false;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_AckPriceUpdate] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_AckPriceUpdate]);

                    //12-Nov-2014 JY added new field IsSalePriceUpdate
                    if (dr[clsPOSDBConstants.Vendor_Fld_SalePriceUpdate] == System.DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_SalePriceUpdate] = false;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_SalePriceUpdate] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_SalePriceUpdate]);

                    //Sprint-21 - 2208 24-Jul-2015 JY Added
                    if (dr[clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice] == System.DBNull.Value)
                        row[clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice] = false;
                    else
                        row[clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice] = Convert.ToBoolean(dr[clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice]);   

                    this.AddRow(row);
                }
            }
            catch (Exception ex)
            {
            }
		}
		
		#endregion //Add and Get Methods 

		protected override DataTable CreateInstance() 
			{
				return new VendorTable();
			}

		internal void InitVars() 
		{
			try 
			{

				this.colVendorId = this.Columns[clsPOSDBConstants.Vendor_Fld_VendorId];
				this.colVendorcode = this.Columns[clsPOSDBConstants.Vendor_Fld_VendorCode];
				this.colVendorname = this.Columns[clsPOSDBConstants.Vendor_Fld_VendorName];
				this.colAddress1 = this.Columns[clsPOSDBConstants.Vendor_Fld_Address1];
				this.colAddress2 = this.Columns[clsPOSDBConstants.Vendor_Fld_Address2];
				this.colCity = this.Columns[clsPOSDBConstants.Vendor_Fld_City];
				this.colState = this.Columns[clsPOSDBConstants.Vendor_Fld_State];
				this.colZip = this.Columns[clsPOSDBConstants.Vendor_Fld_Zip];
				this.colTelephoneno = this.Columns[clsPOSDBConstants.Vendor_Fld_TelephoneNo];
				this.colFaxno = this.Columns[clsPOSDBConstants.Vendor_Fld_FaxNo];
				this.colCellno = this.Columns[clsPOSDBConstants.Vendor_Fld_CellNo];
				this.colUrl = this.Columns[clsPOSDBConstants.Vendor_Fld_URL];
				this.colEmail = this.Columns[clsPOSDBConstants.Vendor_Fld_Email];
				this.colIsActive = this.Columns[clsPOSDBConstants.Vendor_Fld_IsActive];
                //Added by Prashant(SRT) Date:1-06-2009
                this.colIsAutoClose = this.Columns[clsPOSDBConstants.Vendor_Fld_IsAutoClose];
                //End of Added by Prashant(SRT) Date:1-06-2009

                //Added By SRT(Abhishek)  Date :01/07/2009      
                //coulomns Added for VendorInterface
                this.colPrimePOVendorCode = this.Columns[clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode];
                this.colUSEVICForEPO = this.Columns[clsPOSDBConstants.Vendor_Fld_USEVICForEPO];
                this.colPriceQualifier = this.Columns[clsPOSDBConstants.Vendor_Fld_PriceQualifier];
                this.colCostQualifier = this.Columns[clsPOSDBConstants.Vendor_Fld_CostQualifier];               
                this.colUpdatePrice = this.Columns[clsPOSDBConstants.Vendor_Fld_UpdatePrice];
                this.colSendVendCostPrice = this.Columns[clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice];
                //End Of  Added By SRT(Abhishek)  Date :01/07/2009
                //Added By SRT(Prashant)  Date :01/06/2009      
                this.colTimeToOrder = this.Columns[clsPOSDBConstants.Vendor_Fld_TimeToOrder];
                //End of Added By SRT(Prashant)  Date :01/06/2009
                this.colProcess810 = this.Columns[clsPOSDBConstants.Vendor_Fld_Process810];
                //AckPriceUpdate Added by Ravindra on 20 Feb 2013
                this.colAckPriceUpdate= this.Columns[clsPOSDBConstants.Vendor_Fld_AckPriceUpdate];
                this.colSalePriceUpdate = this.Columns[clsPOSDBConstants.Vendor_Fld_SalePriceUpdate]; //12-Nov-2014 JY added new field IsSalePriceUpdate
                this.colReduceSellingPrice = this.Columns[clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice]; //Sprint-21 - 2208 24-Jul-2015 JY Added
                this.colSalePriceQualifier = this.Columns[clsPOSDBConstants.Vendor_Fld_SalePriceQualifier]; //12-Nov-2014 JY added new field IsSalePriceUpdate
            }
			catch(Exception exp)
			{
				throw(exp);
			}
		}

		private void InitClass() 
		{
			this.colVendorId = new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorId, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colVendorId);
			this.colVendorId.AllowDBNull = false;

			this.colVendorcode = new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorCode, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colVendorcode);
			this.colVendorcode.AllowDBNull = false;

			this.colVendorname = new DataColumn(clsPOSDBConstants.Vendor_Fld_VendorName, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colVendorname);
			this.colVendorname.AllowDBNull = true;

			this.colAddress1 = new DataColumn(clsPOSDBConstants.Vendor_Fld_Address1, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAddress1);
			this.colAddress1.AllowDBNull = true;

			this.colAddress2 = new DataColumn(clsPOSDBConstants.Vendor_Fld_Address2, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAddress2);
			this.colAddress2.AllowDBNull = true;

			this.colCity = new DataColumn(clsPOSDBConstants.Vendor_Fld_City, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCity);
			this.colCity.AllowDBNull = true;

			this.colState = new DataColumn(clsPOSDBConstants.Vendor_Fld_State, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colState);
			this.colState.AllowDBNull = true;

			this.colZip = new DataColumn(clsPOSDBConstants.Vendor_Fld_Zip, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colZip);
			this.colZip.AllowDBNull = true;

			this.colTelephoneno = new DataColumn(clsPOSDBConstants.Vendor_Fld_TelephoneNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTelephoneno);
			this.colTelephoneno.AllowDBNull = true;

			this.colFaxno = new DataColumn(clsPOSDBConstants.Vendor_Fld_FaxNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colFaxno);
			this.colFaxno.AllowDBNull = true;

			this.colCellno = new DataColumn(clsPOSDBConstants.Vendor_Fld_CellNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCellno);
			this.colCellno.AllowDBNull = true;

			this.colUrl = new DataColumn(clsPOSDBConstants.Vendor_Fld_URL, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colUrl);
			this.colUrl.AllowDBNull = true;

			this.colEmail = new DataColumn(clsPOSDBConstants.Vendor_Fld_Email, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colEmail);
			this.colEmail.AllowDBNull = true;

			this.colIsActive = new DataColumn(clsPOSDBConstants.Vendor_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colIsActive);
			this.colIsActive.AllowDBNull = true;

            //Added by Prashant(SRT) Date:1-06-2009
            this.colIsAutoClose = new DataColumn(clsPOSDBConstants.Vendor_Fld_IsAutoClose, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsAutoClose);
            this.colIsAutoClose.AllowDBNull = true;
            //End of Added by Prashant(SRT) Date:1-06-2009

            ////Added By SRT(Abhishek)  Date :01/07/2009      
            ////coulomns Added for VendorInterface

            this.colPrimePOVendorCode = new DataColumn(clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrimePOVendorCode);
            this.colPrimePOVendorCode.AllowDBNull = true;

            this.colUSEVICForEPO = new DataColumn(clsPOSDBConstants.Vendor_Fld_USEVICForEPO, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUSEVICForEPO);
            this.colUSEVICForEPO.AllowDBNull = true;

            this.colPriceQualifier = new DataColumn(clsPOSDBConstants.Vendor_Fld_PriceQualifier, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPriceQualifier);
            this.colPriceQualifier.AllowDBNull = true;


            this.colCostQualifier = new DataColumn(clsPOSDBConstants.Vendor_Fld_CostQualifier, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCostQualifier);
            this.colCostQualifier.AllowDBNull = true;

            //Added by Prashant(SRT) Date:1-06-2009
            this.colTimeToOrder = new DataColumn(clsPOSDBConstants.Vendor_Fld_TimeToOrder, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTimeToOrder);
            this.colTimeToOrder.AllowDBNull = true;
            //End of Added by Prashant(SRT) Date:1-06-2009

            this.colUpdatePrice = new DataColumn(clsPOSDBConstants.Vendor_Fld_UpdatePrice, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatePrice);
            this.colUpdatePrice.AllowDBNull = true;

            this.colSendVendCostPrice = new DataColumn(clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSendVendCostPrice);
            this.colSendVendCostPrice.AllowDBNull = true;

            this.colProcess810 = new DataColumn(clsPOSDBConstants.Vendor_Fld_Process810, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProcess810);
            this.colProcess810.AllowDBNull = true;
            
            ////End Of  Added By SRT(Abhishek)  Date :01/07/2009

            //AckPriceUpdate Added by Ravindra on 20 Feb 2013
            this.colAckPriceUpdate = new DataColumn(clsPOSDBConstants.Vendor_Fld_AckPriceUpdate, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAckPriceUpdate);
            this.colAckPriceUpdate.AllowDBNull = true;

            

            #region 12-Nov-2014 JY added new field IsSalePriceUpdate
            this.colSalePriceUpdate = new DataColumn(clsPOSDBConstants.Vendor_Fld_SalePriceUpdate, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSalePriceUpdate);
            this.colSalePriceUpdate.AllowDBNull = true;
            #endregion

            #region Sprint-21 - 2208 24-Jul-2015 JY Added
            this.colReduceSellingPrice = new DataColumn(clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colReduceSellingPrice);
            this.colReduceSellingPrice.AllowDBNull = true;
            #endregion

            //4-Nov-2014 Ravindra added For SalePriceQualifier;
            this.colSalePriceQualifier = new DataColumn(clsPOSDBConstants.Vendor_Fld_SalePriceQualifier, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSalePriceQualifier);
            this.colSalePriceQualifier.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] {this.Vendorcode};
		}
		public VendorRow NewclsVendorRow() 
		{
			return (VendorRow)this.NewRow();
		}

		public override DataTable Clone() 
		{
			VendorTable cln = (VendorTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new VendorRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(VendorRow);
		}
	} 
}
