namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;

	//         This class is used to define the shape of CustomerTable.
	public class CustomerTable : DataTable
	{

		private DataColumn colCustomerId;
		private DataColumn colCustomerName;
		private DataColumn colAddress1;
		private DataColumn colAddress2;
		private DataColumn colCity;
		private DataColumn colState;
		private DataColumn colZip;
		private DataColumn colPhoneOffice;
		private DataColumn colFaxNo;
		private DataColumn colCellNo;
		private DataColumn colPhoneHome;
		private DataColumn colEmail;
		private DataColumn colIsActive;
        private DataColumn colAcctNumber;
        private DataColumn colPrimaryContact;
        private DataColumn colDateOfBirth;
        private DataColumn colComments;
        private DataColumn colGender;

        private DataColumn colUseForCustomerLoyalty;
        //Naim 7Aug2009
        private DataColumn colCustomerCode;
        private DataColumn colFirstName;
        //End Naim 7Aug2009

        private DataColumn colDriveLicNo;
        private DataColumn colDriveLicState;
        private DataColumn colHouseChargeAcctID;
        private DataColumn colPatientNo;

        private DataColumn colDiscount;//Added By shitaljit on 17 Feb 2012
        private DataColumn colLanguageId;//Added By shitaljit on 10 Oct 2013
        private DataColumn colSaveCardProfile;  //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added

        #region Constructors 
        internal CustomerTable() : base(clsPOSDBConstants.Customer_tbl) { this.InitClass(); }
		internal CustomerTable(DataTable table) : base(table.TableName) {}
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

		public CustomerRow this[int index] 
		{
			get 
			{
				return ((CustomerRow)(this.Rows[index]));
			}
		}

		// Public Property DataColumn Customercode
		public DataColumn CustomerId 
		{
			get 
			{
				return this.colCustomerId;
			}
		}

        public DataColumn HouseChargeAcctID
        {
            get
            {
                return this.colHouseChargeAcctID;
            }
        }

        public DataColumn PrimaryContact
        {
            get
            {
                return this.colPrimaryContact;
            }
        }

        public DataColumn Gender
        {
            get
            {
                return this.colGender;
            }
        }

        public DataColumn AccountNumber
        {
            get
            {
                return this.colAcctNumber;
            }
        }

		// Public Property DataColumn Customername

		public DataColumn CustomerName 
		{
			get 
			{
				return this.colCustomerName;
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

		// Public Property DataColumn PhoneOffice
		public DataColumn PhoneOffice 
		{
			get 
			{
				return this.colPhoneOffice;
			}
		}

		// Public Property DataColumn Faxno
		public DataColumn FaxNo 
		{
			get 
			{
				return this.colFaxNo;
			}
		}

		// Public Property DataColumn Cellno
		public DataColumn CellNo 
		{
			get 
			{
				return this.colCellNo;
			}
		}

		// Public Property DataColumn PhoneHome
		public DataColumn PhoneHome 
		{
			get 
			{
				return this.colPhoneHome;
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

        public DataColumn UseForCustomerLoyalty
        {
            get
            {
                return this.colUseForCustomerLoyalty;
            }
        }

        //Naim 07Aug2009
        public DataColumn CustomerCode
        {
            get
            {
                return this.colCustomerCode;
            }
        }

        public DataColumn FirstName
        {
            get
            {
                return this.colFirstName;
            }
        }
        //End Naim 07Aug2009

        public DataColumn DriveLicNo
        {
            get
            {
                return this.colDriveLicNo;
            }
        }

        public DataColumn DriveLicState
        {
            get
            {
                return this.colDriveLicState;
            }
        }

        public DataColumn DateOfBirth
        {
            get
            {
                return this.colDateOfBirth;
            }
        }

        public DataColumn Comments
        {
            get
            {
                return this.colComments;
            }
        }

        public DataColumn PatientNo
        {
            get
            {
                return this.colPatientNo;
            }
        }
        //added by shitaljit on 17 Feb 2012
        public DataColumn Discount
        {
            get
            {
                return this.colDiscount;
            }
        }
        public DataColumn LanguageId
        {
            get
            {
                return this.colLanguageId;
            }
        }

        //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added
        public DataColumn SaveCardProfile
        {
            get
            {
                return this.colSaveCardProfile;
            }
        }
        #endregion //Properties

        #region Add and Get Methods 

        public  void AddRow(CustomerRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(CustomerRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.CustomerId.ToString()) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

        //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added bSaveCardProfile parameter
        public CustomerRow AddRow(System.Int32 CustomerId ,System.String CustomerName,System.String Address1,System.String Address2,System.String City,System.String State,System.String Zip,System.String PhoneOffice,System.String FaxNo,System.String CellNo,
            System.String PhoneHome, System.String Email, System.Boolean IsActive, System.Int32 AcctNumber, System.Int32 PrimaryContact, DateTime DateOfBirth, string Comments, System.Int32 iGender, System.Boolean bUseForCustomerLoyalty,System.Decimal Discount, System.Int32 LanguageId, System.Boolean bSaveCardProfile)
        {
		
			CustomerRow row = (CustomerRow)this.NewRow();
            row.ItemArray = new object[] {CustomerId,CustomerName,Address1,Address2,City,State,Zip,PhoneOffice,FaxNo,CellNo,
                                             PhoneHome,Email,IsActive,AcctNumber,PrimaryContact,DateOfBirth,Comments,iGender,bUseForCustomerLoyalty,"","","","",0,Discount,LanguageId, bSaveCardProfile};
            this.Rows.Add(row);
			return row;
		}

        public CustomerRow GetRowByID(System.String CustomerCode) 
		{
			return (CustomerRow)this.Rows.Find(new object[] {CustomerCode});
		}

		public  void MergeTable(DataTable dt) 
		{ 
			//add any rows in the DataTable 
			CustomerRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (CustomerRow)this.NewRow();

				if (dr[clsPOSDBConstants.Customer_Fld_CustomerId] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_CustomerId] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_CustomerId] = Convert.ToInt32(dr[clsPOSDBConstants.Customer_Fld_CustomerId].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_CustomerName] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_CustomerName] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_CustomerName] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_CustomerName].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_Address1] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_Address1] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_Address1] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_Address1].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_Address2] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_Address2] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_Address2] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_Address2].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_City] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_City] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_City] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_City].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_State] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_State] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_State] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_State].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_Zip] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_Zip] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_Zip] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_Zip].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_PhoneOffice] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_PhoneOffice] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_PhoneOffice] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_PhoneOffice].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_FaxNo] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_FaxNo] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_FaxNo] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_FaxNo].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_CellNo] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_CellNo] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_CellNo] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_CellNo].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_PhoneHome] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_PhoneHome] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_PhoneHome] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_PhoneHome].ToString());

				if (dr[clsPOSDBConstants.Customer_Fld_Email] == DBNull.Value) 
					row[clsPOSDBConstants.Customer_Fld_Email] = DBNull.Value;
				else
					row[clsPOSDBConstants.Customer_Fld_Email] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_Email].ToString());

				row[clsPOSDBConstants.Customer_Fld_IsActive] = Convert.ToBoolean(dr[clsPOSDBConstants.Customer_Fld_IsActive].ToString());

                row[clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty] = POS_Core.Resources.Configuration.convertNullToBoolean(dr[clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_CustomerCode] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_CustomerCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Customer_Fld_CustomerCode] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_CustomerCode].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_FirstName] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_FirstName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Customer_Fld_FirstName] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_FirstName].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_DriveLicNo] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_DriveLicNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Customer_Fld_DriveLicNo] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_DriveLicNo].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_DriveLicState] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_DriveLicState] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Customer_Fld_DriveLicState] = Convert.ToString(dr[clsPOSDBConstants.Customer_Fld_DriveLicState].ToString());


                if (dr[clsPOSDBConstants.Customer_Fld_AcctNumber] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_AcctNumber] = Convert.ToInt32(row[clsPOSDBConstants.Customer_Fld_CustomerId].ToString());
                else
                    row[clsPOSDBConstants.Customer_Fld_AcctNumber] = Convert.ToInt32(dr[clsPOSDBConstants.Customer_Fld_AcctNumber].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_PrimaryContact] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_PrimaryContact] = 0;
                else
                    row[clsPOSDBConstants.Customer_Fld_PrimaryContact] = Convert.ToInt32(dr[clsPOSDBConstants.Customer_Fld_PrimaryContact].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_DateOfBirth] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_DateOfBirth] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Customer_Fld_DateOfBirth] = Convert.ToDateTime(dr[clsPOSDBConstants.Customer_Fld_DateOfBirth].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_Comments] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_Comments] = "";
                else
                    row[clsPOSDBConstants.Customer_Fld_Comments] = dr[clsPOSDBConstants.Customer_Fld_Comments].ToString();

                if (dr[clsPOSDBConstants.Customer_Fld_Gender] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_Gender] = 0;
                else
                    row[clsPOSDBConstants.Customer_Fld_Gender] = Convert.ToInt32(dr[clsPOSDBConstants.Customer_Fld_Gender].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_HouseChargeAcctID] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_HouseChargeAcctID] = 0;
                else
                    row[clsPOSDBConstants.Customer_Fld_HouseChargeAcctID] = Convert.ToInt32(dr[clsPOSDBConstants.Customer_Fld_HouseChargeAcctID].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_PatientNo] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_PatientNo] = 0;
                else
                    row[clsPOSDBConstants.Customer_Fld_PatientNo] = Convert.ToInt32(dr[clsPOSDBConstants.Customer_Fld_PatientNo].ToString());
                
                //Added By Shitaljit 0n 17 Feb 2012
                if (dr["Discount"] == DBNull.Value)
                    row["Discount"] = 0;
                else
                    row["Discount"] = Convert.ToDecimal(dr["Discount"].ToString());

                if (dr[clsPOSDBConstants.Customer_Fld_LanguageId] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_LanguageId] = 0;
                else
                    row[clsPOSDBConstants.Customer_Fld_LanguageId] = Convert.ToInt32(dr[clsPOSDBConstants.Customer_Fld_LanguageId].ToString());

                row[clsPOSDBConstants.Customer_Fld_SaveCardProfile] = POS_Core.Resources.Configuration.convertNullToBoolean(dr[clsPOSDBConstants.Customer_Fld_SaveCardProfile].ToString());  //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added

                this.AddRow(row);
			}
		}
		#endregion //Add and Get Methods 

		protected override DataTable CreateInstance() 
			{
				return new CustomerTable();
			}

		internal void InitVars() 
		{
			try 
			{
				this.colCustomerId = this.Columns[clsPOSDBConstants.Customer_Fld_CustomerId];
				this.colCustomerName = this.Columns[clsPOSDBConstants.Customer_Fld_CustomerName];
				this.colAddress1 = this.Columns[clsPOSDBConstants.Customer_Fld_Address1];
				this.colAddress2 = this.Columns[clsPOSDBConstants.Customer_Fld_Address2];
				this.colCity = this.Columns[clsPOSDBConstants.Customer_Fld_City];
				this.colState = this.Columns[clsPOSDBConstants.Customer_Fld_State];
				this.colZip = this.Columns[clsPOSDBConstants.Customer_Fld_Zip];
				this.colPhoneOffice = this.Columns[clsPOSDBConstants.Customer_Fld_PhoneOffice];
				this.colFaxNo = this.Columns[clsPOSDBConstants.Customer_Fld_FaxNo];
				this.colCellNo = this.Columns[clsPOSDBConstants.Customer_Fld_CellNo];
				this.colPhoneHome = this.Columns[clsPOSDBConstants.Customer_Fld_PhoneHome];
				this.colEmail = this.Columns[clsPOSDBConstants.Customer_Fld_Email];
				this.colIsActive = this.Columns[clsPOSDBConstants.Customer_Fld_IsActive];
                this.colUseForCustomerLoyalty = this.Columns[clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty];
                this.colAcctNumber= this.Columns[clsPOSDBConstants.Customer_Fld_AcctNumber];
                this.colPrimaryContact= this.Columns[clsPOSDBConstants.Customer_Fld_PrimaryContact];
                this.colDateOfBirth = this.Columns[clsPOSDBConstants.Customer_Fld_DateOfBirth];
                this.colComments = this.Columns[clsPOSDBConstants.Customer_Fld_Comments];
                this.colGender= this.Columns[clsPOSDBConstants.Customer_Fld_Gender];

                this.colCustomerCode= this.Columns[clsPOSDBConstants.Customer_Fld_CustomerCode];
                this.colFirstName= this.Columns[clsPOSDBConstants.Customer_Fld_FirstName];
                this.colDriveLicNo= this.Columns[clsPOSDBConstants.Customer_Fld_DriveLicNo];
                this.colDriveLicState= this.Columns[clsPOSDBConstants.Customer_Fld_DriveLicState];
                this.colHouseChargeAcctID = this.Columns[clsPOSDBConstants.Customer_Fld_HouseChargeAcctID];
                this.colPatientNo= this.Columns[clsPOSDBConstants.Customer_Fld_PatientNo];
                this.colDiscount = this.Columns["Discount"]; //Added By Shitaljit 0n 17 Feb 2012
                this.colLanguageId = this.Columns[clsPOSDBConstants.Customer_Fld_LanguageId];//Added By Shitajit on 10 Oct 2013
                this.colSaveCardProfile = this.Columns[clsPOSDBConstants.Customer_Fld_SaveCardProfile];
            }
			catch(Exception exp)
			{
				throw(exp);
			}
		}

		private void InitClass() 
		{
			this.colCustomerId = new DataColumn(clsPOSDBConstants.Customer_Fld_CustomerId, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCustomerId);
			this.colCustomerId.AllowDBNull = false;

			this.colCustomerName = new DataColumn(clsPOSDBConstants.Customer_Fld_CustomerName, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCustomerName);
			this.colCustomerName.AllowDBNull = true;

			this.colAddress1 = new DataColumn(clsPOSDBConstants.Customer_Fld_Address1, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAddress1);
			this.colAddress1.AllowDBNull = true;

			this.colAddress2 = new DataColumn(clsPOSDBConstants.Customer_Fld_Address2, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAddress2);
			this.colAddress2.AllowDBNull = true;

			this.colCity = new DataColumn(clsPOSDBConstants.Customer_Fld_City, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCity);
			this.colCity.AllowDBNull = true;

			this.colState = new DataColumn(clsPOSDBConstants.Customer_Fld_State, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colState);
			this.colState.AllowDBNull = true;

			this.colZip = new DataColumn(clsPOSDBConstants.Customer_Fld_Zip, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colZip);
			this.colZip.AllowDBNull = true;

			this.colPhoneOffice = new DataColumn(clsPOSDBConstants.Customer_Fld_PhoneOffice, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colPhoneOffice);
			this.colPhoneOffice.AllowDBNull = true;

			this.colFaxNo = new DataColumn(clsPOSDBConstants.Customer_Fld_FaxNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colFaxNo);
			this.colFaxNo.AllowDBNull = true;

			this.colCellNo = new DataColumn(clsPOSDBConstants.Customer_Fld_CellNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCellNo);
			this.colCellNo.AllowDBNull = true;

			this.colPhoneHome = new DataColumn(clsPOSDBConstants.Customer_Fld_PhoneHome, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colPhoneHome);
			this.colPhoneHome.AllowDBNull = true;

			this.colEmail = new DataColumn(clsPOSDBConstants.Customer_Fld_Email, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colEmail);
			this.colEmail.AllowDBNull = true;

            this.colIsActive = new DataColumn(clsPOSDBConstants.Customer_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;


			this.colAcctNumber = new DataColumn(clsPOSDBConstants.Customer_Fld_AcctNumber, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAcctNumber);
			this.colAcctNumber.AllowDBNull = true;

            this.colPrimaryContact = new DataColumn(clsPOSDBConstants.Customer_Fld_PrimaryContact, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrimaryContact);
            this.colPrimaryContact.AllowDBNull = false;

            //Sprint-25 - PRIMEPOS-433 02-Feb-2017 JY Modified
            this.colDateOfBirth= new DataColumn(clsPOSDBConstants.Customer_Fld_DateOfBirth, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDateOfBirth);
            this.colDateOfBirth.AllowDBNull = true; 

            this.colComments = new DataColumn(clsPOSDBConstants.Customer_Fld_Comments, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colComments);
            this.colComments.AllowDBNull = false;

            this.colGender= new DataColumn(clsPOSDBConstants.Customer_Fld_Gender, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colGender);
            this.colGender.AllowDBNull = false;

            this.colUseForCustomerLoyalty= new DataColumn(clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUseForCustomerLoyalty);
            this.colUseForCustomerLoyalty.AllowDBNull = true;

            this.colCustomerCode = new DataColumn(clsPOSDBConstants.Customer_Fld_CustomerCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerCode);
            this.colCustomerCode.AllowDBNull = true;

            this.colFirstName= new DataColumn(clsPOSDBConstants.Customer_Fld_FirstName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colFirstName);
            this.colFirstName.AllowDBNull = true;

            this.colDriveLicNo= new DataColumn(clsPOSDBConstants.Customer_Fld_DriveLicNo, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDriveLicNo);
            this.colDriveLicNo.AllowDBNull = true;

            this.colDriveLicState= new DataColumn(clsPOSDBConstants.Customer_Fld_DriveLicState, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDriveLicState);
            this.colDriveLicState.AllowDBNull = true;

            this.colHouseChargeAcctID= new DataColumn(clsPOSDBConstants.Customer_Fld_HouseChargeAcctID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colHouseChargeAcctID);
            this.colHouseChargeAcctID.AllowDBNull = false;

            this.colPatientNo= new DataColumn(clsPOSDBConstants.Customer_Fld_PatientNo, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPatientNo);
            this.colPatientNo.AllowDBNull = true;

            //Added By Shitaljit 0n 17 Feb 2012
            this.colDiscount = new DataColumn(clsPOSDBConstants.Customer_Fld_Discount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDiscount);
            this.colDiscount.AllowDBNull = true;

            //Added By shitaljit 10/13/2013
            this.colLanguageId = new DataColumn(clsPOSDBConstants.Customer_Fld_LanguageId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLanguageId);
            this.colLanguageId.AllowDBNull = true;

            //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added
            this.colSaveCardProfile = new DataColumn(clsPOSDBConstants.Customer_Fld_SaveCardProfile, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSaveCardProfile);
            this.colSaveCardProfile.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] {this.CustomerId};
		}

        public CustomerRow NewCustomerRow() 
		{
			return (CustomerRow)this.NewRow();
		}

		public override DataTable Clone() 
		{
			CustomerTable cln = (CustomerTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new CustomerRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(CustomerRow);
		}
	} 
}
