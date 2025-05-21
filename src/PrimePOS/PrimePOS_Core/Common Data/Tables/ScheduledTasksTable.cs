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
    public class ScheduledTasksTable : DataTable
    {
        private DataColumn colScheduledTasksID;
        private DataColumn colTaskName;
        private DataColumn colTaskDescription;
        private DataColumn colPerformTask;
        private DataColumn colRepeatTask;
        private DataColumn colStartDate;
        private DataColumn colStartTime;
        private DataColumn colTaskId;
        private DataColumn colSendEmail;
        private DataColumn colEmailAddress;
        private DataColumn colAdvancedSeetings;
        private DataColumn colRepeatTaskInterval;
        private DataColumn colDuration;
        private DataColumn colSendPrint;
        private DataColumn colEnabled;
        private DataColumn colTaskNameOld;
        private DataColumn colPerformTaskText;
        private DataColumn colSendEmailText;
        private DataColumn colEnabledText;
        private DataColumn colSendPrintText;
        private DataColumn colcolTask;
        private DataColumn colLastExecuted;

        #region Constructors 
        internal ScheduledTasksTable() : base(clsPOSDBConstants.ScheduledTasks_tbl) { this.InitClass(); }
        internal ScheduledTasksTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Public Property for get all Rows in Table        
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ScheduledTasksRow this[int index]
        {
            get
            {
                return ((ScheduledTasksRow)(this.Rows[index]));
            }
        }

        public DataColumn ScheduledTasksID
        {
            get
            {
                return this.colScheduledTasksID;
            }
        }

        public DataColumn TaskName
        {
            get
            {
                return this.colTaskName;
            }
        }

        public DataColumn TaskDescription
        {
            get
            {
                return this.colTaskDescription;
            }
        }

        public DataColumn PerformTask
        {
            get
            {
                return this.colPerformTask;
            }
        }

        public DataColumn RepeatTask
        {
            get
            {
                return this.colRepeatTask;
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

        public DataColumn TaskId
        {
            get
            {
                return this.colTaskId;
            }
        }

        public DataColumn SendEmail
        {
            get
            {
                return this.colSendEmail;
            }
        }

        public DataColumn EmailAddress
        {
            get
            {
                return this.colEmailAddress;
            }
        }

        public DataColumn AdvancedSeetings
        {
            get
            {
                return this.colAdvancedSeetings;
            }
        }

        public DataColumn RepeatTaskInterval
        {
            get
            {
                return this.colRepeatTaskInterval;
            }
        }

        public DataColumn Duration
        {
            get
            {
                return this.colDuration;
            }
        }

        public DataColumn SendPrint
        {
            get
            {
                return this.colSendPrint;
            }
        }

        public DataColumn Enabled
        {
            get
            {
                return this.colEnabled;
            }
        }

        public DataColumn TaskNameOld
        {
            get
            {
                return this.colTaskNameOld;
            }
        }
        public DataColumn PerformTaskText
        {
            get
            {
                return this.colPerformTaskText;
            }
        }
        public DataColumn SendEmailText
        {
            get
            {
                return this.colSendEmailText;
            }
        }
        public DataColumn EnabledText
        {
            get
            {
                return this.colEnabledText;
            }
        }
        public DataColumn SendPrintText
        {
            get
            {
                return this.colSendPrintText;
            }
        }
        public DataColumn colTask
        {
            get
            {
                return this.colcolTask;
            }
        }
        public DataColumn LastExecuted
        {
            get
            {
                return this.colLastExecuted;
            }
        }
        #endregion

        #region Add and Get Methods
        public void AddRow(ScheduledTasksRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ScheduledTasksRow row, bool preserveChanges)
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

        public ScheduledTasksRow AddRow(System.Int32 ScheduledTasksID, System.String TaskName, System.String TaskDescription,
            System.Int32 PerformTask, System.Int32 RepeatTask, DateTime StartDate, DateTime StartTime, System.Int32 TaskId, System.Boolean SendEmail,
            System.String EmailAddress, System.Boolean AdvancedSeetings, System.Int64 RepeatTaskInterval, System.Int64 Duration, System.Boolean SendPrint,
            System.Boolean Enabled)
        {

            ScheduledTasksRow row = (ScheduledTasksRow)this.NewRow();
            row.ItemArray = new object[] {ScheduledTasksID, TaskName, TaskDescription, PerformTask, RepeatTask, StartDate, StartTime, TaskId, SendEmail,
                            EmailAddress, AdvancedSeetings, RepeatTaskInterval, Duration, SendPrint, Enabled};
            this.Rows.Add(row);
            return row;
        }

        public ScheduledTasksRow GetRowByID(System.String ScheduledTasksID)
        {
            return (ScheduledTasksRow)this.Rows.Find(new object[] { ScheduledTasksID });
        }

        public void MergeTable(DataTable dt)
        {
            ScheduledTasksRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ScheduledTasksRow)this.NewRow();

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskName] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_TaskName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_TaskName] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskName].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_PerformTask] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_PerformTask] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_PerformTask] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasks_Fld_PerformTask].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_StartDate] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_StartDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_StartDate] = Convert.ToDateTime(dr[clsPOSDBConstants.ScheduledTasks_Fld_StartDate].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_StartTime] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_StartTime] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_StartTime] = Convert.ToDateTime(dr[clsPOSDBConstants.ScheduledTasks_Fld_StartTime].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskId] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_TaskId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_TaskId] = Convert.ToInt32(dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskId].ToString());

                row[clsPOSDBConstants.ScheduledTasks_Fld_SendEmail] = Convert.ToBoolean(dr[clsPOSDBConstants.ScheduledTasks_Fld_SendEmail].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress].ToString());

                row[clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings] = Convert.ToBoolean(dr[clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval] = Convert.ToInt64(dr[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval].ToString());

                if (dr[clsPOSDBConstants.ScheduledTasks_Fld_Duration] == DBNull.Value)
                    row[clsPOSDBConstants.ScheduledTasks_Fld_Duration] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ScheduledTasks_Fld_Duration] = Convert.ToInt64(dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskId].ToString());

                row[clsPOSDBConstants.ScheduledTasks_Fld_SendPrint] = Convert.ToBoolean(dr[clsPOSDBConstants.ScheduledTasks_Fld_SendPrint].ToString());
                row[clsPOSDBConstants.ScheduledTasks_Fld_Enabled] = Convert.ToBoolean(dr[clsPOSDBConstants.ScheduledTasks_Fld_Enabled].ToString());


                try
                {
                    if (dr[clsPOSDBConstants.ScheduledTasks_Fld_PerformTaskText] == DBNull.Value)
                        row[clsPOSDBConstants.ScheduledTasks_Fld_PerformTaskText] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.ScheduledTasks_Fld_PerformTaskText] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_PerformTaskText].ToString());
                }
                catch { }
                try
                {
                    if (dr[clsPOSDBConstants.ScheduledTasks_Fld_SendEmailText] == DBNull.Value)
                        row[clsPOSDBConstants.ScheduledTasks_Fld_SendEmailText] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.ScheduledTasks_Fld_SendEmailText] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_SendEmailText].ToString());
                }
                catch { }
                try
                {
                    if (dr[clsPOSDBConstants.ScheduledTasks_Fld_EnabledText] == DBNull.Value)
                        row[clsPOSDBConstants.ScheduledTasks_Fld_EnabledText] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.ScheduledTasks_Fld_EnabledText] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_EnabledText].ToString());
                }
                catch { }
                try
                {
                    if (dr[clsPOSDBConstants.ScheduledTasks_Fld_SendPrintText] == DBNull.Value)
                        row[clsPOSDBConstants.ScheduledTasks_Fld_SendPrintText] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.ScheduledTasks_Fld_SendPrintText] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_SendPrintText].ToString());
                }
                catch { }
                try
                {
                    if (dr[clsPOSDBConstants.ScheduledTasks_Fld_LastExecuted] == DBNull.Value)
                        row[clsPOSDBConstants.ScheduledTasks_Fld_LastExecuted] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.ScheduledTasks_Fld_LastExecuted] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_LastExecuted].ToString());
                }
                catch { }
                try
                {
                    if (dr[clsPOSDBConstants.ScheduledTasks_Fld_colTask] == DBNull.Value)
                        row[clsPOSDBConstants.ScheduledTasks_Fld_colTask] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.ScheduledTasks_Fld_colTask] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_colTask].ToString());
                }
                catch { }
                try
                {
                    if (dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskNameOld] == DBNull.Value)
                        row[clsPOSDBConstants.ScheduledTasks_Fld_TaskNameOld] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.ScheduledTasks_Fld_TaskNameOld] = Convert.ToString(dr[clsPOSDBConstants.ScheduledTasks_Fld_TaskNameOld].ToString());
                }
                catch { }
                this.AddRow(row);
            }
        }
        #endregion //Add and Get Methods

        protected override DataTable CreateInstance()
        {
            return new ScheduledTasksTable();
        }

        internal void InitVars()
        {
            try
            {
                this.colScheduledTasksID = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID];
                this.colTaskName = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_TaskName];
                this.colTaskDescription = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription];
                this.colPerformTask = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_PerformTask];
                this.colRepeatTask = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask];
                this.colStartDate = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_StartDate];
                this.colStartTime = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_StartTime];
                this.colTaskId = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_TaskId];
                this.colSendEmail = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_SendEmail];
                this.colEmailAddress = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress];
                this.colAdvancedSeetings = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings];
                this.colRepeatTaskInterval = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval];
                this.colDuration = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_Duration];
                this.colSendPrint = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_SendPrint];
                this.colEnabled = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_Enabled];
                                
                this.colTaskNameOld = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_TaskNameOld];
                this.colPerformTaskText = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_PerformTaskText];
                this.colSendEmailText = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_SendEmailText];
                this.colEnabledText = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_EnabledText];
                this.colSendPrintText = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_SendPrintText];
                this.colcolTask = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_colTask];
                this.colLastExecuted = this.Columns[clsPOSDBConstants.ScheduledTasks_Fld_LastExecuted];
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colScheduledTasksID = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_ScheduledTasksID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colScheduledTasksID);
            this.colScheduledTasksID.AllowDBNull = false;

            this.colTaskName = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_TaskName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaskName);
            this.colTaskName.AllowDBNull = true;

            this.colTaskDescription = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_TaskDescription, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaskDescription);
            this.colTaskDescription.AllowDBNull = true;

            this.colPerformTask = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_PerformTask, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPerformTask);
            this.colPerformTask.AllowDBNull = true;

            this.colRepeatTask = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_RepeatTask, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRepeatTask);
            this.colRepeatTask.AllowDBNull = true;

            this.colStartDate = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_StartDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStartDate);
            this.colStartDate.AllowDBNull = true;

            this.colStartTime = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_StartTime, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStartTime);
            this.colStartTime.AllowDBNull = true;

            this.colTaskId = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_TaskId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaskId);
            this.colTaskId.AllowDBNull = true;

            this.colSendEmail = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_SendEmail, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSendEmail);
            this.colSendEmail.AllowDBNull = true;

            this.colEmailAddress = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_EmailAddress, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEmailAddress);
            this.colEmailAddress.AllowDBNull = true;

            this.colAdvancedSeetings = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_AdvancedSeetings, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAdvancedSeetings);
            this.colAdvancedSeetings.AllowDBNull = true;

            this.colRepeatTaskInterval = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_RepeatTaskInterval, typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRepeatTaskInterval);
            this.colRepeatTaskInterval.AllowDBNull = true;

            this.colDuration = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_Duration, typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDuration);
            this.colDuration.AllowDBNull = true;

            this.colSendPrint = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_SendPrint, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSendPrint);
            this.colSendPrint.AllowDBNull = true;

            this.colEnabled = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_Enabled, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEnabled);
            this.colEnabled.AllowDBNull = true;

            this.colTaskNameOld = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_TaskNameOld, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaskNameOld);
            this.colTaskNameOld.AllowDBNull = true;
            this.colPerformTaskText = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_PerformTaskText, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPerformTaskText);
            this.colPerformTaskText.AllowDBNull = true;
            this.colSendEmailText = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_SendEmailText, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSendEmailText);
            this.colSendEmailText.AllowDBNull = true;
            this.colEnabledText = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_EnabledText, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEnabledText);
            this.colEnabledText.AllowDBNull = true;
            this.colSendPrintText = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_SendPrintText, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSendPrintText);
            this.colSendPrintText.AllowDBNull = true;
            this.colcolTask = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_colTask, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colcolTask);
            this.colcolTask.AllowDBNull = true;
            this.colLastExecuted = new DataColumn(clsPOSDBConstants.ScheduledTasks_Fld_LastExecuted, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastExecuted);
            this.colLastExecuted.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.ScheduledTasksID };
        }

        public ScheduledTasksRow NewScheduledTasksRow()
        {
            return (ScheduledTasksRow)this.NewRow();
        }

        public override DataTable Clone()
        {
            ScheduledTasksTable cln = (ScheduledTasksTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ScheduledTasksRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ScheduledTasksRow);
        }
    }
}