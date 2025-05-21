//----------------------------------------------------------------------------------------------------
//Sprint-18 - 2090 07-Oct-2014 JY Added table class for CL_TransDetail table
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class CLTransDetailTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colID;
        private DataColumn colTransID;
        private DataColumn colCardID;
        private DataColumn colCurrentPoints;
        private DataColumn colPointsAcquired;
        private DataColumn colRunningTotalPoints;
        private DataColumn colActionType;
        private DataColumn colTransDate;

        #region Constants
        private const String _TableName = "CL_TransDetail";
        #endregion

        #region Constructors 
		internal CLTransDetailTable() : base(_TableName) { this.InitClass(); }
        internal CLTransDetailTable(DataTable table) : base(table.TableName) { }
		#endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public CLTransDetailRow this[int index]
        {
            get
            {
                return ((CLTransDetailRow)(this.Rows[index]));
            }
        }

        public DataColumn ID
        {
            get
            {
                return this.colID;
            }
        }

        public DataColumn TransID
        {
            get
            {
                return this.colTransID;
            }
        }

        public DataColumn CardID
        {
            get
            {
                return this.colCardID;
            }
        }

        public DataColumn CurrentPoints
        {
            get
            {
                return this.colCurrentPoints;
            }
        }

        public DataColumn PointsAcquired
        {
            get
            {
                return this.colPointsAcquired;
            }
        }


        public DataColumn RunningTotalPoints
        {
            get
            {
                return this.colRunningTotalPoints;
            }
        }

        public DataColumn ActionType
        {
            get
            {
                return this.colActionType;
            }
        }


        public DataColumn TransDate
        {
            get
            {
                return this.colTransDate;
            }
        }
        #endregion //Properties

        #region Add and Get Methods

        public void AddRow(CLTransDetailRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(CLTransDetailRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public CLTransDetailRow AddRow(System.Int64 ID
                                        , System.Int32 iTransID
                                        , System.Int64 iCardID
                                        , System.Decimal dCurrentPoints
                                        , System.Decimal dPointsAcquired
                                        , System.Decimal dRunningTotalPoints
                                        , System.String sActionType
                                        , System.DateTime dtTransDate)
        {
            CLTransDetailRow row = (CLTransDetailRow)this.NewRow();
            
            row.ID = ID;
            row.TransID = iTransID;
            row.CardID = iCardID;
            row.CurrentPoints = dCurrentPoints;
            row.PointsAcquired = dPointsAcquired;
            row.RunningTotalPoints = dRunningTotalPoints;
            row.ActionType = sActionType;
            row.TransDate = dtTransDate;
            this.Rows.Add(row);
            return row;
        }

        public CLTransDetailRow GetRowByID(System.Int64 iID)
        {
            return (CLTransDetailRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt)
        {

            CLTransDetailRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (CLTransDetailRow)this.NewRow();

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_ID] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_ID] = 0;
                else
                    row[clsPOSDBConstants.CLTransDetail_Fld_ID] = Convert.ToInt64((dr[clsPOSDBConstants.CLTransDetail_Fld_ID].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLTransDetail_Fld_ID].ToString());

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_TransID] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_TransID] = 0;
                else
                    row[clsPOSDBConstants.CLTransDetail_Fld_TransID] = Convert.ToInt32((dr[clsPOSDBConstants.CLTransDetail_Fld_TransID].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLTransDetail_Fld_TransID].ToString());

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_CardID] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_CardID] = 0;
                else
                    row[clsPOSDBConstants.CLTransDetail_Fld_CardID] = Convert.ToInt64((dr[clsPOSDBConstants.CLTransDetail_Fld_CardID].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLTransDetail_Fld_CardID].ToString());

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints] = 0;
                else
                    row[clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints] = Convert.ToDecimal((dr[clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints].ToString());

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired] = 0;
                else
                    row[clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired] = Convert.ToDecimal((dr[clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired].ToString());

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints] = 0;
                else
                    row[clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints] = Convert.ToDecimal((dr[clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints].ToString() == "") ? "0" : dr[clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints].ToString());

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_ActionType] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints] = "";
                else
                    row[clsPOSDBConstants.CLTransDetail_Fld_ActionType] = Convert.ToString((dr[clsPOSDBConstants.CLTransDetail_Fld_ActionType].ToString() == "") ? "" : dr[clsPOSDBConstants.CLTransDetail_Fld_ActionType].ToString());

                if (dr[clsPOSDBConstants.CLTransDetail_Fld_TransDate] == DBNull.Value)
                    row[clsPOSDBConstants.CLTransDetail_Fld_TransDate] = DBNull.Value;
                else
                    if (dr[clsPOSDBConstants.CLTransDetail_Fld_TransDate].ToString().Trim() == "")
                        row[clsPOSDBConstants.CLTransDetail_Fld_TransDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                        row[clsPOSDBConstants.CLTransDetail_Fld_TransDate] = Convert.ToDateTime(dr[clsPOSDBConstants.CLTransDetail_Fld_TransDate].ToString());

                this.AddRow(row);
            }
        }

        #endregion 

        public override DataTable Clone()
        {
            CLTransDetailTable cln = (CLTransDetailTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new CLTransDetailTable();
        }

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.CLTransDetail_Fld_ID];
            this.colTransID = this.Columns[clsPOSDBConstants.CLTransDetail_Fld_TransID];
            this.colCardID = this.Columns[clsPOSDBConstants.CLTransDetail_Fld_CardID];
            this.colCurrentPoints = this.Columns[clsPOSDBConstants.CLTransDetail_Fld_CurrentPoints];
            this.colPointsAcquired= this.Columns[clsPOSDBConstants.CLTransDetail_Fld_PointsAcquired];
            this.colRunningTotalPoints = this.Columns[clsPOSDBConstants.CLTransDetail_Fld_RunningTotalPoints];
            this.colActionType = this.Columns[clsPOSDBConstants.CLTransDetail_Fld_ActionType];
            this.colTransDate = this.Columns[clsPOSDBConstants.CLTransDetail_Fld_TransDate];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colID = new DataColumn("ID", typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colID);
            this.colID.AllowDBNull = true;

            this.colTransID = new DataColumn("TransID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransID);
            this.colTransID.AllowDBNull = true;

            this.colCardID = new DataColumn("CardID", typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCardID);
            this.colCardID.AllowDBNull = true;

            this.colCurrentPoints = new DataColumn("CurrentPoints", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCurrentPoints);
            this.colCurrentPoints.AllowDBNull = true;

            this.colPointsAcquired = new DataColumn("PointsAcquired", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPointsAcquired);
            this.colPointsAcquired.AllowDBNull = true;

            this.colRunningTotalPoints = new DataColumn("RunningTotalPoints", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRunningTotalPoints);
            this.colRunningTotalPoints.AllowDBNull = true;

            this.colActionType = new DataColumn("ActionType", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colActionType);
            this.colActionType.AllowDBNull = true;

            this.colTransDate = new DataColumn("TransDate", typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransDate);
            this.colTransDate.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colID };
        }

        public CLTransDetailRow NewCLTransDetailRow()
        {
            return (CLTransDetailRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new CLTransDetailRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(CLTransDetailRow);
        }
    }
}
