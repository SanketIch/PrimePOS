
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLPointsAdjustmentLogTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colID;
		private DataColumn colCreatedOn;
		private DataColumn colCLCardID;
        private DataColumn colCreatedBy;
        private DataColumn colOldPoints;
        private DataColumn colNewPoints;
        private DataColumn colRemarks;
        
		#region Constants
        private const String _TableName = "CL_PointsAdjustmentLog";
		#endregion

		#region Constructors 
		internal CLPointsAdjustmentLogTable() : base(_TableName) { this.InitClass(); }
		internal CLPointsAdjustmentLogTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public CLPointsAdjustmentLogRow this[int index] 
		{
			get 
			{
				return ((CLPointsAdjustmentLogRow)(this.Rows[index]));
			}
		}

		public DataColumn ID 
		{
			get 
			{
				return this.colID;
			}
		}

		public DataColumn CLCardID 
		{
			get 
			{
				return this.colCLCardID;
			}
		}

        public DataColumn OldPoints
        {
            get
            {
                return this.colOldPoints;
            }
        }
        
        public DataColumn CreatedBy 
		{
			get 
			{
				return this.colCreatedBy;
			}
		}

        public DataColumn CreatedOn
        {
            get
            {
                return this.colCreatedOn;
            }
        }

        public DataColumn NewPoints
        {
            get
            {
                return this.colNewPoints;
            }
        }

        public DataColumn Remarks
        {
            get
            {
                return this.colRemarks;
            }
        }

		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(CLPointsAdjustmentLogRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(CLPointsAdjustmentLogRow row, bool preserveChanges) 
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

		public CLPointsAdjustmentLogRow AddRow(System.Int32 ID 
										, System.Int64  iCLCardID
                                        , System.Decimal dNewPoints
                                        , System.Decimal dOldPoints
                                        , System.String sRemarks) 
		{
			CLPointsAdjustmentLogRow row = (CLPointsAdjustmentLogRow)this.NewRow();
			row.ID=ID;
			row.CLCardID=iCLCardID;
            row.NewPoints=dNewPoints;
            row.OldPoints = dOldPoints;
            row.Remarks = sRemarks;
            this.Rows.Add(row);
			return row;
		}

		public CLPointsAdjustmentLogRow GetRowByID(System.Int32 iID) 
		{
            return (CLPointsAdjustmentLogRow)this.Rows.Find(new object[] { iID });
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			CLPointsAdjustmentLogRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (CLPointsAdjustmentLogRow)this.NewRow();

				if (dr[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID] == DBNull.Value) 
					row[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID] = 0;
				else
					row[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID] = Convert.ToInt32((dr[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedOn] == DBNull.Value)
                    row[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedOn] = DateTime.Now;
                else
                {
                    if (dr[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedOn].ToString().Trim() == "")
                        row[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedOn] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                    {
                        DateTime createdonDate = DateTime.Now;
                        DateTime.TryParse(dr[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedOn].ToString(), out createdonDate);
                        row[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedOn] = createdonDate;
                    }
                }

				string strField=clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CLCardID	;
				if (dr[strField] == DBNull.Value) 
					row[strField] = 0;
				else
					row[strField] = Convert.ToInt64((dr[strField].ToString()=="")? "0":dr[strField].ToString());

                strField=clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedBy;
				if (dr[strField] == DBNull.Value) 
					row[strField] = DBNull.Value;
				else
					row[strField] = dr[strField].ToString();
                 
                strField = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_OldPoints;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString()); //Sprint-18 - 06-Nov-2014 JY Changed datatype from int to decimal

                strField = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_NewPoints;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.CLPointsAdjustmentLog_Fld_Remarks;
                row[strField] = dr[strField].ToString();

				this.AddRow(row);
			}
		}
		
		#endregion 

		public override DataTable Clone() 
		{
			CLPointsAdjustmentLogTable cln = (CLPointsAdjustmentLogTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		
        protected override DataTable CreateInstance() 
		{
			return new CLPointsAdjustmentLogTable();
		}

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_ID];
            this.colCreatedOn = this.Columns[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedOn];
            this.colCLCardID = this.Columns[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CLCardID];
            this.colCreatedBy = this.Columns[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_CreatedBy];
            this.colOldPoints = this.Columns[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_OldPoints];
            this.colNewPoints = this.Columns[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_NewPoints];
            this.colRemarks = this.Columns[clsPOSDBConstants.CLPointsAdjustmentLog_Fld_Remarks];
        }

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colID = new DataColumn("ID", typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colID);
			this.colID.AllowDBNull = false;

			this.colCreatedOn = new DataColumn("CreatedOn", typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCreatedOn);
			this.colCreatedOn.AllowDBNull = true;

			this.colCLCardID = new DataColumn("CardID", typeof(System.Int64), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCLCardID);
			this.colCLCardID.AllowDBNull = true;

            this.colCreatedBy = new DataColumn("CreatedBy", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreatedBy);
            this.colCreatedBy.AllowDBNull = true;

            this.colOldPoints = new DataColumn("OldPoints", typeof(System.Decimal), null, System.Data.MappingType.Element); //Sprint-18 - 06-Nov-2014 JY Changed datatype from int to decimal
            this.Columns.Add(this.colOldPoints);
            this.colOldPoints.AllowDBNull = false;

            this.colNewPoints = new DataColumn("NewPoints", typeof(System.Decimal), null, System.Data.MappingType.Element); //Sprint-18 - 06-Nov-2014 JY Changed datatype from int to decimal
            this.Columns.Add(this.colNewPoints);
            this.colNewPoints.AllowDBNull = false;

            this.colRemarks = new DataColumn("Remarks", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRemarks);
            this.colRemarks.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] {this.colID};
		}
		
        public  CLPointsAdjustmentLogRow NewCLPointsAdjustmentLogRow() 
		{
			return (CLPointsAdjustmentLogRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new CLPointsAdjustmentLogRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(CLPointsAdjustmentLogRow);
		}
	}
}
