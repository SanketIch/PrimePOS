using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;

namespace POS_Core.CommonData.Tables
{
    public class ScheduledTasksLogTable : DataTable
    {
        private DataColumn colScheduledTasksLogID;
        private DataColumn colTaskStatus;
        private DataColumn colLogDescription;
        private DataColumn colStartDate;
        private DataColumn colStartTime;
        private DataColumn colEndTime;
        private DataColumn colScheduledTasksID;
        private DataColumn colComputerName;

        #region Constructors 
        internal ScheduledTasksLogTable() : base(clsPOSDBConstants.ScheduledTasksLog_tbl) { this.InitClass(); }
        internal ScheduledTasksLogTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Public Property for get all Rows in Table        
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ScheduledTasksLogRow this[int index]
        {
            get
            {
                return ((ScheduledTasksLogRow)(this.Rows[index]));
            }
        }

        public DataColumn ScheduledTasksLogID
        {
            get
            {
                return this.colScheduledTasksLogID;
            }
        }

        public DataColumn TaskStatus
        {
            get
            {
                return this.colTaskStatus;
            }
        }

        public DataColumn LogDescription
        {
            get
            {
                return this.colLogDescription;
            }
        }

        public DataColumn StartDate
        {
            get
            {
                return this.colStartDate;
            }
        }

        public DataColumn StartTime
        {
            get
            {
                return this.colStartTime;
            }
        }

        public DataColumn EndTime
        {
            get
            {
                return this.colEndTime;
            }
        }

        public DataColumn ScheduledTasksID
        {
            get
            {
                return this.colScheduledTasksID;
            }
        }

        public DataColumn ComputerName
        {
            get
            {
                return this.colComputerName;
            }
        }
        #endregion

        #region Add and Get Methods
        public void AddRow(ScheduledTasksLogRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ScheduledTasksLogRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ScheduledTasksLogID.ToString()) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public ScheduledTasksLogRow AddRow(System.Int32 ScheduledTasksLogID, System.String TaskStatus, System.String LogDescription,
            DateTime StartDate, DateTime  StartTime, DateTime EndTime, System.Int32 ScheduledTasksID, System.String ComputerName)
        {

            ScheduledTasksLogRow row = (ScheduledTasksLogRow)this.NewRow();
            row.ItemArray = new object[] {ScheduledTasksLogID, TaskStatus, LogDescription, StartDate, StartTime, EndTime, ScheduledTasksID, ComputerName};
            this.Rows.Add(row);
            return row;
        }

        public ScheduledTasksLogRow GetRowByID(System.String ScheduledTasksLogID)
        {
            return (ScheduledTasksLogRow)this.Rows.Find(new object[] { ScheduledTasksLogID });
        }

        public void MergeTable(DataTable dt)
        {
            ScheduledTasksLogRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ScheduledTasksLogRow)this.NewRow();

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate] = Convert.ToDateTime(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime] = Convert.ToDateTime(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime] = Convert.ToDateTime(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime].ToString());  

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName].ToString());

                this.AddRow(row);
            }
        }
        #endregion //Add and Get Methods

        protected override DataTable CreateInstance()
        {
            return new ScheduledTasksLogTable();
        }

        internal void InitVars()
        {
            try
            {
                this.colScheduledTasksLogID = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID];
                this.colTaskStatus = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus];
                this.colLogDescription = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription];
                this.colStartDate = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate];
                this.colStartTime = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime];
                this.colEndTime = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime];
                this.colScheduledTasksID = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID];
                this.colComputerName = this.Columns[clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName];
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colScheduledTasksLogID = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksLogID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colScheduledTasksLogID);
            this.colScheduledTasksLogID.AllowDBNull = false;

            this.colTaskStatus = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_TaskStatus, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaskStatus);
            this.colTaskStatus.AllowDBNull = true;

            this.colLogDescription = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_LogDescription, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLogDescription);
            this.colLogDescription.AllowDBNull = true;

            this.colStartDate = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_StartDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStartDate);
            this.colStartDate.AllowDBNull = true;

            this.colStartTime = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_StartTime, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStartTime);
            this.colStartTime.AllowDBNull = true;

            this.colEndTime = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_EndTime, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEndTime);
            this.colEndTime.AllowDBNull = true;

            this.colScheduledTasksID = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_ScheduledTasksID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colScheduledTasksID);
            this.colScheduledTasksID.AllowDBNull = true;

            this.colComputerName = new DataColumn(clsPOSDBConstants.ScheduledTasksLog_Fld_ComputerName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colComputerName);
            this.colComputerName.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.ScheduledTasksLogID };
        }

        public ScheduledTasksLogRow NewScheduledTasksLogRow()
        {
            return (ScheduledTasksLogRow)this.NewRow();
        }

        public override DataTable Clone()
        {
            ScheduledTasksLogTable cln = (ScheduledTasksLogTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ScheduledTasksLogRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ScheduledTasksLogRow);
        }
    }
}