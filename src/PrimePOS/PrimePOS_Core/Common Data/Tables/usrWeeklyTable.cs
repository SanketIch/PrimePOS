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
    public class usrWeeklyTable : DataTable
    {
        private DataColumn colScheduledTasksID;
        private DataColumn colDays;
        private DataColumn colSelectedDays;

        #region Constructors 
        internal usrWeeklyTable() : base(clsPOSDBConstants.ScheduledTasksDetailWeek_tbl) { this.InitClass(); }
        internal usrWeeklyTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Public Property for get all Rows in Table        
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public usrWeeklyRow this[int index]
        {
            get
            {
                return ((usrWeeklyRow)(this.Rows[index]));
            }
        }

        public DataColumn ScheduledTasksID
        {
            get
            {
                return this.colScheduledTasksID;
            }
        }

        public DataColumn Days
        {
            get
            {
                return this.colDays;
            }
        }

        public DataColumn SelectedDays
        {
            get
            {
                return this.colSelectedDays;
            }
        }
        #endregion

        #region Add and Get Methods
        public void AddRow(usrWeeklyRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(usrWeeklyRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ScheduledTasksID.ToString()) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public usrWeeklyRow AddRow(System.Int32 ScheduledTasksID, System.Int32 Days, System.String SelectedDays)
        {
            usrWeeklyRow row = (usrWeeklyRow)this.NewRow();
            row.ItemArray = new object[] { ScheduledTasksID, Days, SelectedDays };
            this.Rows.Add(row);
            return row;
        }

        public usrWeeklyRow GetRowByID(System.String ScheduledTasksID)
        {
            return (usrWeeklyRow)this.Rows.Find(new object[] { ScheduledTasksID });
        }

        public void MergeTable(DataTable dt)
        {
            usrWeeklyRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (usrWeeklyRow)this.NewRow();

                if (dr[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays].ToString());

                this.AddRow(row);
            }
        }
        #endregion

        protected override DataTable CreateInstance()
        {
            return new usrWeeklyTable();
        }

        internal void InitVars()
        {
            try
            {
                this.colScheduledTasksID = this.Columns[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID];
                this.colDays = this.Columns[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days];
                this.colSelectedDays = this.Columns[clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays];
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colScheduledTasksID = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_ScheduledTasksID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colScheduledTasksID);
            this.colScheduledTasksID.AllowDBNull = false;

            this.colDays = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_Days, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDays);
            this.colDays.AllowDBNull = true;

            this.colSelectedDays = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailWeek_Fld_SelectedDays, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSelectedDays);
            this.colSelectedDays.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.ScheduledTasksID };
        }

        public usrWeeklyRow NewusrWeeklyRow()
        {
            return (usrWeeklyRow)this.NewRow();
        }

        public override DataTable Clone()
        {
            usrWeeklyTable cln = (usrWeeklyTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new usrWeeklyRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(usrWeeklyRow);
        }
    }
}
