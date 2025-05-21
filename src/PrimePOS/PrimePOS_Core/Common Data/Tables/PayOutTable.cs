namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;

	//         This class is used to define the shape of PayOutTable.
	public class PayOutTable : DataTable
	{

		private DataColumn colPayOutId;
		private DataColumn colDescription;
		private DataColumn colAmount;
		private DataColumn colStationID;
		private DataColumn colUserID;
		private DataColumn colTransDate;
		private DataColumn colDrawNo;
        private DataColumn colPayoutCatType;
        private DataColumn colPayoutcatID;
		#region Constructors 
		internal PayOutTable() : base(clsPOSDBConstants.PayOut_tbl) { this.InitClass(); }
		internal PayOutTable(DataTable table) : base(table.TableName) {}
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

		public PayOutRow this[int index] 
		{
			get 
			{
				return ((PayOutRow)(this.Rows[index]));
			}
		}

		// Public Property DataColumn PayOutcode
		public DataColumn PayOutId 
		{
			get 
			{
				return this.colPayOutId;
			}
		}

		public DataColumn DrawNo
		{
			get 
			{
				return this.colDrawNo;
			}
		}

		public DataColumn StationID
		{
			get 
			{
				return this.colStationID;
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

		public DataColumn Amount
		{
			get 
			{
				return this.colAmount;
			}
		}
        public DataColumn PayoutCatType
        {
            get
            {
                return this.colPayoutCatType;
            }
        }
        public DataColumn PayoutcatID
        {
            get
            {
                return this.colPayoutcatID;
            }
        }

		#endregion //Properties
		#region Add and Get Methods 

		public  void AddRow(PayOutRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(PayOutRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.PayOutId.ToString()) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public  PayOutRow AddRow(System.Int32 PayOutId , System.String Description,System.Decimal Amount) 
		{
		
			PayOutRow row = (PayOutRow)this.NewRow();
			row.ItemArray = new object[] {PayOutId,Description,Amount};
			this.Rows.Add(row);
			return row;
		}

		public PayOutRow GetRowByID(System.String PayOutCode) 
		{
			return (PayOutRow)this.Rows.Find(new object[] {PayOutCode});
		}

		public  void MergeTable(DataTable dt) 
		{ 
			//add any rows in the DataTable 
			PayOutRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (PayOutRow)this.NewRow();

				if (dr[clsPOSDBConstants.PayOut_Fld_PayOutID] == DBNull.Value) 
					row[clsPOSDBConstants.PayOut_Fld_PayOutID] = DBNull.Value;
				else
					row[clsPOSDBConstants.PayOut_Fld_PayOutID] = Convert.ToInt32(dr[clsPOSDBConstants.PayOut_Fld_PayOutID].ToString());

				if (dr[clsPOSDBConstants.PayOut_Fld_Description] == DBNull.Value) 
					row[clsPOSDBConstants.PayOut_Fld_Description] = DBNull.Value;
				else
					row[clsPOSDBConstants.PayOut_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.PayOut_Fld_Description].ToString());

				if (dr[clsPOSDBConstants.PayOut_Fld_Amount] == DBNull.Value) 
					row[clsPOSDBConstants.PayOut_Fld_Amount] = DBNull.Value;
				else
					row[clsPOSDBConstants.PayOut_Fld_Amount] = Convert.ToDecimal(dr[clsPOSDBConstants.PayOut_Fld_Amount].ToString());

				if (dr[clsPOSDBConstants.PayOut_Fld_TransDate] == DBNull.Value) 
					row[clsPOSDBConstants.PayOut_Fld_TransDate] = DateTime.Now;
				else
					row[clsPOSDBConstants.PayOut_Fld_TransDate] = Convert.ToDateTime(dr[clsPOSDBConstants.PayOut_Fld_TransDate].ToString());

				row[clsPOSDBConstants.fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.fld_UserID].ToString());
				row[clsPOSDBConstants.fld_StationID] = Convert.ToString(dr[clsPOSDBConstants.fld_StationID].ToString());

				if (dr[clsPOSDBConstants.PayOut_Fld_DrawNo] == DBNull.Value) 
					row[clsPOSDBConstants.PayOut_Fld_DrawNo] = DBNull.Value;
				else
					row[clsPOSDBConstants.PayOut_Fld_DrawNo] = Convert.ToInt32(dr[clsPOSDBConstants.PayOut_Fld_DrawNo].ToString());

                if (dr[clsPOSDBConstants.PayOut_Fld_PayoutCatID] == DBNull.Value)
                    row[clsPOSDBConstants.PayOut_Fld_PayoutCatID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.PayOut_Fld_PayoutCatID] = Convert.ToInt32(dr[clsPOSDBConstants.PayOut_Fld_PayoutCatID].ToString());

                if (dr[clsPOSDBConstants.PayOut_Fld_PayoutCatType] == DBNull.Value)//Shitaljit
                    row[clsPOSDBConstants.PayOut_Fld_PayoutCatType] = DBNull.Value;
                else

                    row[clsPOSDBConstants.PayOut_Fld_PayoutCatType] = Convert.ToString(dr[clsPOSDBConstants.PayOutCat_Fld_PayoutType].ToString());

				this.AddRow(row);
			}
		}
		
		#endregion //Add and Get Methods 
		protected override DataTable CreateInstance() 
		{
			return new PayOutTable();
		}

		internal void InitVars() 
		{
			try 
			{

				this.colPayOutId = this.Columns[clsPOSDBConstants.PayOut_Fld_PayOutID];
				this.colStationID = this.Columns[clsPOSDBConstants.PayOut_Fld_StationID];
				this.colUserID= this.Columns[clsPOSDBConstants.PayOut_Fld_UserID];
				this.colDescription = this.Columns[clsPOSDBConstants.PayOut_Fld_Description];
				this.colAmount= this.Columns[clsPOSDBConstants.PayOut_Fld_Amount];
				this.colTransDate= this.Columns[clsPOSDBConstants.PayOut_Fld_TransDate];
				this.colDrawNo= this.Columns[clsPOSDBConstants.PayOut_Fld_DrawNo];
                this.colPayoutCatType = this.Columns[clsPOSDBConstants.PayOutCat_Fld_PayoutType];
                this.colPayoutcatID = this.Columns[clsPOSDBConstants.PayOut_Fld_PayoutCatID];
			}
			catch(Exception exp)
			{
				throw(exp);
			}
		}

		private void InitClass() 
		{
			this.colPayOutId = new DataColumn(clsPOSDBConstants.PayOut_Fld_PayOutID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colPayOutId);
			this.colPayOutId.AllowDBNull = false;

			this.colDescription = new DataColumn(clsPOSDBConstants.PayOut_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDescription);
			this.colDescription.AllowDBNull = false;

			this.colAmount= new DataColumn(clsPOSDBConstants.PayOut_Fld_Amount, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAmount);
			this.colAmount.AllowDBNull = false;

			this.colStationID= new DataColumn(clsPOSDBConstants.PayOut_Fld_StationID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colStationID);
			this.colAmount.AllowDBNull = true;

			this.colUserID= new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colUserID);
			this.colAmount.AllowDBNull = true;

			this.colTransDate= new DataColumn(clsPOSDBConstants.PayOut_Fld_TransDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransDate);
			this.colTransDate.AllowDBNull = true;

			this.colDrawNo= new DataColumn(clsPOSDBConstants.PayOut_Fld_DrawNo, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDrawNo);
			this.colTransDate.AllowDBNull = true;
            //Shitaljit  Start
            this.colPayoutCatType = new DataColumn(clsPOSDBConstants.PayOutCat_Fld_PayoutType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayoutCatType);
            this.colPayoutCatType.AllowDBNull = true;

            this.colPayoutcatID = new DataColumn(clsPOSDBConstants.PayOut_Fld_PayoutCatID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayoutcatID);
            this.colPayoutcatID.AllowDBNull = true;

            //Shitaljit End
			this.PrimaryKey = new DataColumn[] {this.colPayOutId};
		}
		public PayOutRow NewclsPayOutRow() 
		{
			return (PayOutRow)this.NewRow();
		}

		public override DataTable Clone() 
		{
			PayOutTable cln = (PayOutTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new PayOutRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(PayOutRow);
		}
	} 
}
