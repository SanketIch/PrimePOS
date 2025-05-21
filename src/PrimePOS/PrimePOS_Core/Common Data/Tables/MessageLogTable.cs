using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;

namespace POS_Core.CommonData.Tables
{
    public partial class MessageLogTable : DataTable
    {
        private DataColumn colLogDate;
        private DataColumn colLogMessage;
       
        private DataColumn colLogTime;

        #region Constructors
        public MessageLogTable() : base(clsPOSDBConstants.MessageLog_tbl) { this.InitClass(); }
        internal MessageLogTable(DataTable table) : base(table.TableName) { }
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

        public MessageLogRow this[int index]
        {
            get
            {
                return ((MessageLogRow)(this.Rows[index]));
            }
        }

        // Public Property DataColumn Date
        public DataColumn LogDate
        {
            get
            {
                return this.colLogDate;
            }
        }


        public DataColumn LogMessage
        {
            get
            {
                return this.colLogMessage;
            }
        }

        public DataColumn LogTime
        {
            get
            {
                return this.colLogTime;
            }
        }

        #endregion //Properties
        #region Add and Get Methods

        public void AddRow(MessageLogRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(MessageLogRow row, bool preserveChanges)
        {
            //if (this.GetRow(row.LogDate) == null)
            //{
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            //}
        }

        public MessageLogRow AddRow(System.DateTime date,System.String logTime,System.String messageString)
        {

            MessageLogRow row = (MessageLogRow)this.NewRow();
            row.ItemArray = new object[] { date,logTime,messageString };
            this.Rows.Add(row);
            return row;
        }

        public MessageLogRow GetRowByID(System.DateTime date)
        {
            return (MessageLogRow)this.Rows.Find(new object[] { date });
        }

        public void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            MessageLogRow row;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    row = (MessageLogRow)this.NewRow();

                    if (dr[clsPOSDBConstants.MessageLog_Fld_Date] == DBNull.Value)
                        row[clsPOSDBConstants.MessageLog_Fld_Date] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.MessageLog_Fld_Date] = Convert.ToDateTime(dr[clsPOSDBConstants.MessageLog_Fld_Date]);

                    if (dr[clsPOSDBConstants.MessageLog_Fld_Time] == DBNull.Value)
                        row[clsPOSDBConstants.MessageLog_Fld_Time] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.MessageLog_Fld_Time] = dr[clsPOSDBConstants.MessageLog_Fld_Time].ToString();

                    if (dr[clsPOSDBConstants.MessageLog_Fld_MessageString] == DBNull.Value)
                        row[clsPOSDBConstants.MessageLog_Fld_MessageString] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.MessageLog_Fld_MessageString] = dr[clsPOSDBConstants.MessageLog_Fld_MessageString].ToString();

                    this.AddRow(row);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion //Add and Get Methods

        protected override DataTable CreateInstance()
        {
            return new MessageLogTable();
        }

        internal void InitVars()
        {
            try
            {

                this.colLogDate = this.Columns[clsPOSDBConstants.MessageLog_Fld_Date];
                this.colLogTime = this.Columns[clsPOSDBConstants.MessageLog_Fld_Time];
                this.colLogMessage = this.Columns[clsPOSDBConstants.MessageLog_Fld_MessageString];
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colLogDate = new DataColumn(clsPOSDBConstants.MessageLog_Fld_Date, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLogDate);
            this.colLogDate.AllowDBNull = true;

            this.colLogTime = new DataColumn(clsPOSDBConstants.MessageLog_Fld_Time, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLogTime);
            this.colLogTime.AllowDBNull = true;

            this.colLogMessage = new DataColumn(clsPOSDBConstants.MessageLog_Fld_MessageString, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLogMessage);
            this.colLogMessage.AllowDBNull = true;

            //this.PrimaryKey = new DataColumn[] {this.CustomerCode};
        }
        public MessageLogRow NewCustomerRow()
        {
            return (MessageLogRow)this.NewRow();
        }


        public override DataTable Clone()
        {
            MessageLogTable cln = (MessageLogTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new MessageLogRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(MessageLogRow);
        }
    }
}

