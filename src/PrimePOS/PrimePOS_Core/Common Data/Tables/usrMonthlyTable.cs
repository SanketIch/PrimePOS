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
    public class usrMonthlyTable : DataTable
    {
        private DataColumn colScheduledTasksID;
        private DataColumn colDaysOrOn;
        private DataColumn colSelectionMonths;
        private DataColumn colSelectionDays;
        private DataColumn colMonthperiods;
        private DataColumn colweekDays;

        #region Constructors 
        internal usrMonthlyTable() : base(clsPOSDBConstants.ScheduledTasksDetailMonth_tbl) { this.InitClass(); }
        internal usrMonthlyTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Public Property for get all Rows in Table        
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public usrMonthlyRow this[int index]
        {
            get
            {
                return ((usrMonthlyRow)(this.Rows[index]));
            }
        }

        public DataColumn ScheduledTasksID
        {
            get
            {
                return this.colScheduledTasksID;
            }
        }

        public DataColumn DaysOrOn
        {
            get
            {
                return this.colDaysOrOn;
            }
        }

        public DataColumn SelectionMonths
        {
            get
            {
                return this.colSelectionMonths;
            }
        }

        public DataColumn SelectionDays
        {
            get
            {
                return this.colSelectionDays;
            }
        }

        public DataColumn Monthperiods
        {
            get
            {
                return this.colMonthperiods;
            }
        }

        public DataColumn weekDays
        {
            get
            {
                return this.colweekDays;
            }
        }
        #endregion

        #region Add and Get Methods
        public void AddRow(usrMonthlyRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(usrMonthlyRow row, bool preserveChanges)
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

        public usrMonthlyRow AddRow(System.Int32 ScheduledTasksID, System.Boolean DaysOrOn, System.String SelectionMonths, System.String SelectionDays, System.String Monthperiods, System.String weekDays)
        {
            usrMonthlyRow row = (usrMonthlyRow)this.NewRow();
            row.ItemArray = new object[] { ScheduledTasksID, DaysOrOn, SelectionMonths, SelectionDays, Monthperiods, weekDays };
            this.Rows.Add(row);
            return row;
        }

        public usrMonthlyRow GetRowByID(System.String ScheduledTasksID)
        {
            return (usrMonthlyRow)this.Rows.Find(new object[] { ScheduledTasksID });
        }

        public void MergeTable(DataTable dt)
        {
            usrMonthlyRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (usrMonthlyRow)this.NewRow();

                if (dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID].ToString());

                row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn] = Convert.ToBoolean(dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays].ToString());

                this.AddRow(row);
            }
        }
        #endregion

        protected override DataTable CreateInstance()
        {
            return new usrMonthlyTable();
        }

        internal void InitVars()
        {
            try
            {
                this.colScheduledTasksID = this.Columns[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID];
                this.colDaysOrOn = this.Columns[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn];
                this.colSelectionMonths = this.Columns[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths];
                this.colSelectionDays = this.Columns[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays];
                this.colMonthperiods = this.Columns[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods];
                this.colweekDays = this.Columns[clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays];
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colScheduledTasksID = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_ScheduledTasksID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colScheduledTasksID);
            this.colScheduledTasksID.AllowDBNull = false;

            this.colDaysOrOn = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_DaysOrOn, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDaysOrOn);
            this.colDaysOrOn.AllowDBNull = true;

            this.colSelectionMonths = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionMonths, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSelectionMonths);
            this.colSelectionMonths.AllowDBNull = true;

            this.colSelectionDays = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_SelectionDays, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSelectionDays);
            this.colSelectionDays.AllowDBNull = true;

            this.colMonthperiods = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_Monthperiods, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMonthperiods);
            this.colMonthperiods.AllowDBNull = true;

            this.colweekDays = new DataColumn(clsPOSDBConstants.ScheduledTasksDetailMonth_Fld_weekDays, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colweekDays);
            this.colweekDays.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.ScheduledTasksID };
        }

        public usrMonthlyRow NewusrMonthlyRow()
        {
            return (usrMonthlyRow)this.NewRow();
        }

        public override DataTable Clone()
        {
            usrMonthlyTable cln = (usrMonthlyTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new usrMonthlyRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(usrMonthlyRow);
        }
    }
}