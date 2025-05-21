using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;


namespace POS_Core.CommonData.Tables
{
    public class FileHistoryTable : DataTable
    {

        private DataColumn colFileID;
        private DataColumn colLastUpdateDate;
        private DataColumn colSynchronizedCentrally;

        #region Constructors

        internal FileHistoryTable() : base(clsPOSDBConstants.FileHistory_tbl) { this.InitClass(); }
        internal FileHistoryTable(DataTable table) : base(table.TableName) { }
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

        public FileHistoryRow this[int index]
        {
            get
            {
                return ((FileHistoryRow)(this.Rows[index]));
            }
        }

        // Public Property DataColumn Customercode
        public DataColumn FileID
        {
            get
            {
                return this.colFileID;
            }
        }


        public DataColumn LastUpdateDate
        {
            get
            {
                return this.colLastUpdateDate;
            }
        }

        public DataColumn SynchronizedCentrally
        {
            get
            {
                return this.colSynchronizedCentrally;
            }
        }

        // Public Property DataColumn Customername

        #endregion //Properties
        #region Add and Get Methods

        public void AddRow(FileHistoryRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(FileHistoryRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.FileID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public FileHistoryRow AddRow(System.Int64 FileID, System.DateTime LastUpdateDate, bool SynchronizedCentrally)
        {
            FileHistoryRow row = (FileHistoryRow)this.NewRow();
            row.ItemArray = new object[] { FileID, LastUpdateDate, SynchronizedCentrally };
            this.Rows.Add(row);
            return row;
        }

        public FileHistoryRow GetRowByID(System.Int64 FileID)
        {
            return (FileHistoryRow)this.Rows.Find(new object[] { FileID });
        }


        public void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 

            FileHistoryRow row;

            foreach (DataRow dr in dt.Rows)
            {
                row = (FileHistoryRow)this.NewRow();

                if (dr[clsPOSDBConstants.FileHistory_Fld_FileID] == DBNull.Value)
                    row[clsPOSDBConstants.FileHistory_Fld_FileID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.FileHistory_Fld_FileID] = Convert.ToInt64(dr[clsPOSDBConstants.FileHistory_Fld_FileID].ToString());

                if (dr[clsPOSDBConstants.FileHistory_Fld_LastUpdateDate] == DBNull.Value)
                    row[clsPOSDBConstants.FileHistory_Fld_LastUpdateDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.FileHistory_Fld_LastUpdateDate] = dr[clsPOSDBConstants.FileHistory_Fld_LastUpdateDate].ToString();

                if (dr[clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally] == DBNull.Value)
                    row[clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally] = DBNull.Value;
                else
                    row[clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally] = dr[clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally].ToString();
                
                this.AddRow(row);
            }
        }

        #endregion //Add and Get Methods

        protected override DataTable CreateInstance()
        {
            return new FileHistoryTable();
        }

        internal void InitVars()
        {
            try
            {
                this.colFileID = this.Columns[clsPOSDBConstants.FileHistory_Fld_FileID];
                this.colLastUpdateDate = this.Columns[clsPOSDBConstants.FileHistory_Fld_LastUpdateDate];
                this.colSynchronizedCentrally = this.Columns[clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally];

            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colFileID = new DataColumn(clsPOSDBConstants.FileHistory_Fld_FileID, typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colFileID);
            this.colFileID.AllowDBNull = false;

            this.colLastUpdateDate = new DataColumn(clsPOSDBConstants.FileHistory_Fld_LastUpdateDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastUpdateDate);
            this.colLastUpdateDate.AllowDBNull = false;

            this.colSynchronizedCentrally = new DataColumn(clsPOSDBConstants.FileHistory_Fld_SynchronizedCentrally, typeof(bool));
            this.Columns.Add(this.colSynchronizedCentrally);
            this.colSynchronizedCentrally.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.FileID };

        }
        public FileHistoryRow NewFileHistoryRow()
        {
            return (FileHistoryRow)this.NewRow();
        }


        public override DataTable Clone()
        {
            FileHistoryTable cln = (FileHistoryTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new FileHistoryRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(FileHistoryRow);
        }
    } 
}
