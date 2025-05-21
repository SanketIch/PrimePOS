
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TimesheetTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colID;
		private DataColumn colUserID;
		private DataColumn colTimeIn;
		private DataColumn colTimeOut;
		
		private DataColumn colIsManualIn;
		private DataColumn colIsManualOut;
		private DataColumn colLastUpdatedBy;
		private DataColumn colLastUpdatedOn;

        private DataColumn colIsTimeIn;
        private DataColumn colIsTimeOut;

		#region Constructors 
		internal TimesheetTable() : base(clsPOSDBConstants.Timesheet_tbl) { this.InitClass(); }
        internal TimesheetTable(DataTable table) : base(table.TableName) { }
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public TimesheetRow this[int index] 
		{
			get 
			{
				return ((TimesheetRow)(this.Rows[index]));
			}
		}

		public DataColumn ID
		{
			get 
			{
				return this.colID;
			}
		}

		public DataColumn UserID
		{
			get 
			{
				return this.colUserID;
			}
		}

		public DataColumn TimeIn
		{
			get 
			{
				return this.colTimeIn;
			}
		}
		
		public DataColumn TimeOut
		{
			get 
			{
				return this.colTimeOut;
			}
		}

		public DataColumn IsManualIn
		{
			get 
			{
				return this.colIsManualIn;
			}
		}
		
		public DataColumn IsManualOut
		{
			get 
			{
				return this.colIsManualOut;
			}
		}
		
		public DataColumn LastUpdatedBy
		{
			get 
			{
				return this.colLastUpdatedBy;
			}
		}

		public DataColumn LastUpdatedOn
		{
			get 
			{
				return this.colLastUpdatedOn;
			}
		}

        public DataColumn IsTimeIn
        {
            get
            {
                return this.colIsTimeIn;
            }
        }

        public DataColumn IsTimeOut
        {
            get
            {
                return this.colIsTimeOut;
            }
        }
		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(TimesheetRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(TimesheetRow row, bool preserveChanges) 
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

		public TimesheetRow AddRow( System.Int64 iID
                                        , System.String sUserID
										, System.DateTime  dtTimeIn
										, System.DateTime?  dtTimeOut
										, System.String  sLastUpdatedBy
                                        , System.Boolean bIsManualTimeIn
                                        , System.Boolean bIsManualTimeOut
										, System.DateTime dtLastUpdatedOn) 
		{
			TimesheetRow row = (TimesheetRow)this.NewRow();
			row.ID=iID;
            row.UserID = sUserID;
			row.TimeIn=dtTimeIn;

            if (dtTimeOut.HasValue)
            {
                row.TimeOut = (System.Data.SqlTypes.SqlDateTime)dtTimeOut.Value;
            }
            else
            {
                row.TimeOut = System.Data.SqlTypes.SqlDateTime.Null;
            }
			row.LastUpdatedBy=sLastUpdatedBy;
			row.IsManualTimeIn=bIsManualTimeIn;
			row.IsManualTimeOut=bIsManualTimeOut;
            row.LastUpdatedOn = dtLastUpdatedOn;
		
			this.Rows.Add(row);
			return row;
		}

		public TimesheetRow GetRowByID(System.Int64 iID) 
		{
			return (TimesheetRow)this.Rows.Find(new object[] {iID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			TimesheetRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (TimesheetRow)this.NewRow();

                row[clsPOSDBConstants.Timesheet_Fld_ID] = Convert.ToInt64((dr[clsPOSDBConstants.Timesheet_Fld_ID].ToString() == "") ? "0" : dr[clsPOSDBConstants.Timesheet_Fld_ID].ToString());

				row[clsPOSDBConstants.Timesheet_Fld_TimeIn]= Convert.ToDateTime(dr[clsPOSDBConstants.Timesheet_Fld_TimeIn].ToString());

                row["istimein"] = true;
                DateTime dtTimeOut;
                if (DateTime.TryParse(dr[clsPOSDBConstants.Timesheet_Fld_TimeOut].ToString(), out dtTimeOut) == true)
                {
                    row["isTimeout"] = true;
                    row[clsPOSDBConstants.Timesheet_Fld_TimeOut] = dtTimeOut;
                }
                else
                {
                    row["istimeout"] = false;
                    row[clsPOSDBConstants.Timesheet_Fld_TimeOut] = System.Data.SqlTypes.SqlDateTime.Null;
                }

                row[clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn] = Convert.ToDateTime(dr[clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn].ToString());

                row[clsPOSDBConstants.Timesheet_Fld_IsManualIn] = Convert.ToBoolean(dr[clsPOSDBConstants.Timesheet_Fld_IsManualIn].ToString());
                row[clsPOSDBConstants.Timesheet_Fld_IsManualOut] = Convert.ToBoolean(dr[clsPOSDBConstants.Timesheet_Fld_IsManualOut].ToString());

				row[clsPOSDBConstants.Timesheet_Fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.Timesheet_Fld_UserID].ToString());
                row[clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy] = Convert.ToString(dr[clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy].ToString());

				this.AddRow(row);
			}
		}
		
		#endregion 

		public override DataTable Clone() 
		{
			TimesheetTable cln = (TimesheetTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		
        protected override DataTable CreateInstance() 
		{
			return new TimesheetTable();
		}

		internal void InitVars() 
		{
			this.colID = this.Columns[clsPOSDBConstants.Timesheet_Fld_ID];
			this.colUserID= this.Columns[clsPOSDBConstants.Timesheet_Fld_UserID];
			this.colTimeIn= this.Columns[clsPOSDBConstants.Timesheet_Fld_TimeIn];
			this.colTimeOut = this.Columns[clsPOSDBConstants.Timesheet_Fld_TimeOut];
			this.colIsManualIn= this.Columns[clsPOSDBConstants.Timesheet_Fld_IsManualIn];
			this.colIsManualOut= this.Columns[clsPOSDBConstants.Timesheet_Fld_IsManualOut];
			this.colLastUpdatedBy = this.Columns[clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy];
			this.colLastUpdatedOn = this.Columns[clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn];
			
		}
		
        public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colID= new DataColumn(clsPOSDBConstants.Timesheet_Fld_ID, typeof(System.Int64), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colID);
			this.colID.AllowDBNull = true;

            this.colUserID = new DataColumn(clsPOSDBConstants.Timesheet_Fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = true;

			this.colTimeIn= new DataColumn(clsPOSDBConstants.Timesheet_Fld_TimeIn,typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTimeIn);
			this.colTimeIn.AllowDBNull = true;

            this.colTimeOut = new DataColumn(clsPOSDBConstants.Timesheet_Fld_TimeOut, typeof(System.Data.SqlTypes.SqlDateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTimeOut);
            this.colTimeOut.AllowDBNull = true;

			this.colIsManualIn= new DataColumn(clsPOSDBConstants.Timesheet_Fld_IsManualIn,typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colIsManualIn);
			this.colIsManualIn.AllowDBNull = true;

            this.colIsManualOut = new DataColumn(clsPOSDBConstants.Timesheet_Fld_IsManualOut, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsManualOut);
            this.colIsManualOut.AllowDBNull = true;

			this.colLastUpdatedBy= new DataColumn(clsPOSDBConstants.Timesheet_Fld_LastUpdatedBy,typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colLastUpdatedBy);
			this.colLastUpdatedBy.AllowDBNull = true;

			this.colLastUpdatedOn= new DataColumn(clsPOSDBConstants.Timesheet_Fld_LastUpdatedOn, typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colLastUpdatedOn);
			this.colLastUpdatedOn.AllowDBNull = true;

            this.colIsTimeIn = new DataColumn("IsTimeIn", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsTimeIn);
            this.colIsTimeIn.AllowDBNull = true;

            this.colIsTimeOut = new DataColumn("IsTimeOut", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsTimeOut);
            this.colIsTimeOut.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.colID};
		}

		public  TimesheetRow NewTimesheetRow() 
		{
			return (TimesheetRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new TimesheetRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(TimesheetRow);
		}
	}
}
