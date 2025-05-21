
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLCouponsTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colID;
		private DataColumn colIsCouponUsed;

		private DataColumn colCreatedOn;
		private DataColumn colCLCardID;
        private DataColumn colCreatedBy;
        private DataColumn colPoints;
        private DataColumn colExpiryDays;
        private DataColumn colCouponValue;
        private DataColumn colUsedInTransID;
        private DataColumn colCreatedInTransID;
        private DataColumn colIsCouponValueInPercentage;
        private DataColumn colIsActive;
        private DataColumn colCLTierID;//Added By Shitaljit for saving Reward Tier ID while creating coupons

		#region Constants
		private const String _TableName = "CL_Coupons";
		#endregion

		#region Constructors 
		internal CLCouponsTable() : base(_TableName) { this.InitClass(); }
		internal CLCouponsTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public CLCouponsRow this[int index] 
		{
			get 
			{
				return ((CLCouponsRow)(this.Rows[index]));
			}
		}

		public DataColumn ID 
		{
			get 
			{
				return this.colID;
			}
		}

		public DataColumn IsCouponUsed 
		{
			get 
			{
				return this.colIsCouponUsed;
			}
		}

		public DataColumn CLCardID 
		{
			get 
			{
				return this.colCLCardID;
			}
		}

        public DataColumn Points
        {
            get
            {
                return this.colPoints;
            }
        }
        
        public DataColumn CreatedBy 
		{
			get 
			{
				return this.CreatedBy;
			}
		}

        public DataColumn CreatedOn
        {
            get
            {
                return this.colCreatedOn;
            }
        }

        public DataColumn ExpiryDays
        {
            get
            {
                return this.colExpiryDays;
            }
        }

        public DataColumn CouponValue
        {
            get
            {
                return this.colCouponValue;
            }
        }

        public DataColumn UsedInTransID
        {
            get
            {
                return this.colUsedInTransID;
            }
        }

        public DataColumn CreatedInTransID
        {
            get
            {
                return this.colCreatedInTransID;
            }
        }

        public DataColumn IsCouponValueInPercentage
        {
            get
            {
                return this.colIsCouponValueInPercentage;
            }
        }

        public DataColumn IsActive
        {
            get
            {
                return this.colIsActive;
            }
        }

        //Added By Shitaljit for saving Reward Tier ID while creating coupons
        public DataColumn CLTierID
        {
            get
            {
                return this.colCLTierID;
            }
        }
        //END
		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(CLCouponsRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(CLCouponsRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.CLCardID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public CLCouponsRow AddRow(System.Int32 ID 
										,System.Boolean bIsCouponUsed 
										, System.Int64  iCLCardID
                                        , System.String sCreatedBy
                                        , System.Int32 iExpiryDays 
                                        , System.Decimal dCouponValue
                                        , System.Int64 iUsedInTransID
                                        , System.Int64 iCreatedInTransID
                                        , Boolean IsActive
                                        , Int64 CLTierID) 
		{
			CLCouponsRow row = (CLCouponsRow)this.NewRow();
			//row.ItemArray = new object[] {ID,DeptCode,DeptName,Discount,IsCouponUsed , IsCouponUsed , CreatedOn ,CLCardID , SalePrice};
			row.ID=ID;
			row.IsCouponUsed=bIsCouponUsed;
			row.CreatedOn=DateTime.Now;
			row.CLCardID=iCLCardID;
            row.CreatedBy=sCreatedBy;
            row.Points = 0;
            row.ExpiryDays = iExpiryDays;
            row.CouponValue = dCouponValue;
            row.UsedInTransID = iUsedInTransID;
            row.CreatedInTransID = iCreatedInTransID;
            row.IsCouponValueInPercentage = false;
            row.IsActive = true;
            row.CLTierID = CLTierID;//Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon
			this.Rows.Add(row);
			return row;
		}

		public CLCouponsRow GetRowByID(System.Int64 iID) 
		{
            return (CLCouponsRow)this.Rows.Find(new object[] { iID });
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			CLCouponsRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (CLCouponsRow)this.NewRow();

				if (dr[clsPOSDBConstants.CLCoupons_Fld_ID] == DBNull.Value) 
					row[clsPOSDBConstants.CLCoupons_Fld_ID] = DBNull.Value;
				else
					row[clsPOSDBConstants.CLCoupons_Fld_ID] = Convert.ToInt32((dr[clsPOSDBConstants.CLCoupons_Fld_ID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed] == DBNull.Value) 
					row[clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed] = DBNull.Value;
				else
					row[clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed] = Convert.ToBoolean((dr[clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed].ToString()=="")? "false":dr[clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed].ToString());


                if (dr[clsPOSDBConstants.CLCoupons_Fld_CreatedOn] == DBNull.Value)
                    row[clsPOSDBConstants.CLCoupons_Fld_CreatedOn] = DBNull.Value;
                else
                {
                    if (dr[clsPOSDBConstants.CLCoupons_Fld_CreatedOn].ToString().Trim() == "")
                        row[clsPOSDBConstants.CLCoupons_Fld_CreatedOn] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                    {
                        DateTime createdonDate = DateTime.Now;
                        DateTime.TryParse(dr[clsPOSDBConstants.CLCoupons_Fld_CreatedOn].ToString(), out createdonDate);
                        row[clsPOSDBConstants.CLCoupons_Fld_CreatedOn] = createdonDate; // Convert.ToDateTime(dr[clsPOSDBConstants.CLCoupons_Fld_CreatedOn].ToString());
                    }
                }

				string strField=clsPOSDBConstants.CLCoupons_Fld_CLCardID	;
				if (dr[strField] == DBNull.Value) 
					row[strField] = 0;
				else
					row[strField] = Convert.ToInt64((dr[strField].ToString()=="")? "0":dr[strField].ToString());

                strField=clsPOSDBConstants.CLCoupons_Fld_CreatedBy;
				if (dr[strField] == DBNull.Value) 
					row[strField] = DBNull.Value;
				else
					row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.CLCoupons_Fld_Points;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                {
                    //row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());//Sprint-18 - 2090 22-Oct-2014 JY Commented convert to int
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString()); //Sprint-18 - 2090 22-Oct-2014 JY Added convert to decimal
                }

                strField = clsPOSDBConstants.CLCoupons_Fld_ExpiryDays;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.CLCoupons_Fld_CouponValue;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.CLCoupons_Fld_CreatedInTransID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToInt64((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.CLCoupons_Fld_UsedInTransID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToInt64((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                if (dr[clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage] == DBNull.Value)
                    row[clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage] = false;
                else
                    row[clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage] = Convert.ToBoolean((dr[clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage].ToString() == "") ? "false" : dr[clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_IsActive] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_IsActive] = false;
                else
                    row[clsPOSDBConstants.Users_Fld_IsActive] = Convert.ToBoolean((dr[clsPOSDBConstants.Users_Fld_IsActive].ToString() == "") ? "false" : dr[clsPOSDBConstants.Users_Fld_IsActive].ToString());

                //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon
                strField = clsPOSDBConstants.CLCoupons_Fld_CLTierID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToInt64((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());
                //END
				this.AddRow(row);
			}
		}
		
		#endregion 

		public override DataTable Clone() 
		{
			CLCouponsTable cln = (CLCouponsTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		
        protected override DataTable CreateInstance() 
		{
			return new CLCouponsTable();
		}

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.CLCoupons_Fld_ID];
            this.colIsCouponUsed = this.Columns[clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed];
            this.colCreatedOn = this.Columns[clsPOSDBConstants.CLCoupons_Fld_CreatedOn];
            this.colCLCardID = this.Columns[clsPOSDBConstants.CLCoupons_Fld_CLCardID];
            this.colCreatedBy = this.Columns[clsPOSDBConstants.CLCoupons_Fld_CreatedBy];
            this.colPoints = this.Columns[clsPOSDBConstants.CLCoupons_Fld_Points];
            this.colExpiryDays = this.Columns[clsPOSDBConstants.CLCoupons_Fld_ExpiryDays];
            this.colCouponValue = this.Columns[clsPOSDBConstants.CLCoupons_Fld_CouponValue];
            this.colUsedInTransID = this.Columns[clsPOSDBConstants.CLCoupons_Fld_UsedInTransID];
            this.colCreatedInTransID = this.Columns[clsPOSDBConstants.CLCoupons_Fld_CreatedInTransID];

            this.colIsCouponValueInPercentage= this.Columns[clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage];
            this.colIsActive= this.Columns[clsPOSDBConstants.Users_Fld_IsActive];
            //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon
            this.colCLTierID = this.Columns[clsPOSDBConstants.CLCoupons_Fld_CLTierID];
            //END
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

			this.colIsCouponUsed = new DataColumn("IsCouponUsed", typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colIsCouponUsed);
			this.colIsCouponUsed.AllowDBNull = true;

			this.colCreatedOn = new DataColumn("CreatedOn", typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCreatedOn);
			this.colCreatedOn.AllowDBNull = true;

			this.colCLCardID = new DataColumn("CardID", typeof(System.Int64), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCLCardID);
			this.colCLCardID.AllowDBNull = true;

            this.colCreatedBy = new DataColumn("CreatedBy", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreatedBy);
            this.colCreatedBy.AllowDBNull = true;

            this.colPoints = new DataColumn("Points", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPoints);
            this.colPoints.AllowDBNull = true;

            this.colExpiryDays = new DataColumn("ExpiryDays", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExpiryDays);
            this.colExpiryDays.AllowDBNull = true;

            this.colCouponValue = new DataColumn("CouponValue", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCouponValue);
            this.colCouponValue.AllowDBNull = true;


            this.colUsedInTransID = new DataColumn("UsedInTransID", typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUsedInTransID);
            this.colUsedInTransID.AllowDBNull = true;


            this.colCreatedInTransID = new DataColumn("CreatedInTransID", typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreatedInTransID);
            this.colCreatedInTransID.AllowDBNull = true;

            this.colIsCouponValueInPercentage = new DataColumn("IsCouponValueInPercentage", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsCouponValueInPercentage);
            this.colIsCouponValueInPercentage.AllowDBNull = true;

            this.colIsActive= new DataColumn("IsActive", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;

            //Added By Shitaljit on 2/4/2014 to save tier ID while saving coupon
            this.colCLTierID = new DataColumn(clsPOSDBConstants.CLCoupons_Fld_CLTierID, typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCLTierID);
            this.colCLTierID.AllowDBNull = true;
            //END

			this.PrimaryKey = new DataColumn[] {this.colID};
		}
		
        public  CLCouponsRow NewCLCouponsRow() 
		{
			return (CLCouponsRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new CLCouponsRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(CLCouponsRow);
		}
	}
}
