
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLPointsRewardTierTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colID;
		private DataColumn colDescription;
		private DataColumn colDiscount;
        private DataColumn colPoints;
        private DataColumn colRewardPeriod;

		#region Constants
		private const String _TableName = "CLPointsRewardTier";
		#endregion

		#region Constructors 
		internal CLPointsRewardTierTable() : base(_TableName) { this.InitClass(); }
		internal CLPointsRewardTierTable(DataTable table) : base(table.TableName) {}
		#endregion
		
        #region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public CLPointsRewardTierRow this[int index] 
		{
			get 
			{
				return ((CLPointsRewardTierRow)(this.Rows[index]));
			}
		}

		public DataColumn ID 
		{
			get 
			{
				return this.colID;
			}
		}

		public DataColumn Description 
		{
			get 
			{
				return this.colDescription;
			}
		}

		public DataColumn Discount 
		{
			get 
			{
				return this.colDiscount;
			}
		}

        public DataColumn Points
        {
            get
            {
                return this.colPoints;
            }
        }

        public DataColumn RewardPeriod
        {
            get
            {
                return this.colRewardPeriod;
            }
        }

		#endregion //Properties
		
        #region Add and Get Methods 

		public  void AddRow(CLPointsRewardTierRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(CLPointsRewardTierRow row, bool preserveChanges) 
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

		public CLPointsRewardTierRow AddRow(System.Int32 ID 
										,System.String Description
										,System.Decimal Discount
                                        , System.Int32 Points
                                        , System.Int32 iRewardPeriod) 
		{
			CLPointsRewardTierRow row = (CLPointsRewardTierRow)this.NewRow();
			row.ID=ID;
			row.Description=Description;
			row.Discount=Discount;
            row.Points = Points;
            row.RewardPeriod = iRewardPeriod;
			this.Rows.Add(row);
			return row;
		}

		public CLPointsRewardTierRow GetRowByID(System.Int32 iID) 
		{
			return (CLPointsRewardTierRow)this.Rows.Find(new object[] {iID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			CLPointsRewardTierRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (CLPointsRewardTierRow)this.NewRow();

				if (dr[clsPOSDBConstants.CLPointsRewardTier_Fld_ID] == DBNull.Value) 
					row[clsPOSDBConstants.CLPointsRewardTier_Fld_ID] = DBNull.Value;
				else
					row[clsPOSDBConstants.CLPointsRewardTier_Fld_ID] = Convert.ToInt32((dr[clsPOSDBConstants.CLPointsRewardTier_Fld_ID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Description] == DBNull.Value) 
					row[clsPOSDBConstants.CLPointsRewardTier_Fld_Description] = string.Empty;
				else
					row[clsPOSDBConstants.CLPointsRewardTier_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Description].ToString());

				if (dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Discount] == DBNull.Value) 
					row[clsPOSDBConstants.CLPointsRewardTier_Fld_Discount] = 0;
				else
					row[clsPOSDBConstants.CLPointsRewardTier_Fld_Discount] = Convert.ToDecimal((dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Discount].ToString()=="")? "0":dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Discount].ToString());

                if (dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Points] == DBNull.Value)
                    row[clsPOSDBConstants.CLPointsRewardTier_Fld_Points] = 0;
                else
                    row[clsPOSDBConstants.CLPointsRewardTier_Fld_Points] = Convert.ToInt32((dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Points].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLPointsRewardTier_Fld_Points].ToString());

                if (dr[clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod] == DBNull.Value)
                    row[clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod] = 0;
                else
                    row[clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod] = Convert.ToInt32((dr[clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod].ToString());

                this.AddRow(row);
			}
		}
		
		#endregion 

        public override DataTable Clone() 
		{
			CLPointsRewardTierTable cln = (CLPointsRewardTierTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new CLPointsRewardTierTable();
		}

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.CLPointsRewardTier_Fld_ID];
            this.colDescription = this.Columns[clsPOSDBConstants.CLPointsRewardTier_Fld_Description];
            this.colDiscount = this.Columns[clsPOSDBConstants.CLPointsRewardTier_Fld_Discount];
            this.colPoints = this.Columns[clsPOSDBConstants.CLPointsRewardTier_Fld_Points];
            this.colRewardPeriod = this.Columns[clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod];
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

			this.colDescription = new DataColumn("Description", typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDescription);
			this.colDescription.AllowDBNull = true;

			this.colDiscount = new DataColumn("Discount", typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDiscount);
			this.colDiscount.AllowDBNull = true;

            this.colPoints = new DataColumn("Points", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPoints);
            this.colPoints.AllowDBNull = true;

            this.colRewardPeriod = new DataColumn("RewardPeriod", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRewardPeriod);
            this.colRewardPeriod.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.ID};
		}
		public  CLPointsRewardTierRow NewCLPointsRewardTierRow() 
		{
			return (CLPointsRewardTierRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new CLPointsRewardTierRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(CLPointsRewardTierRow);
		}
	}
}
