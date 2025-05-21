// ----------------------------------------------------------------
//Added By Shitaljit(QuicSolv) 0n 5 oct 2011
// ----------------------------------------------------------------

using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData.Tables
{
   public class NotesTable : DataTable, System.Collections.IEnumerable
    {
        #region columns declaration
        private DataColumn colNoteId;
        private DataColumn colEntityId;
        private DataColumn colEntityType;
        private DataColumn colNote;
        private DataColumn colCreatedDate;
        private DataColumn colCreatedBy;
        private DataColumn colUpdatedDate;
        private DataColumn colUpdatedBy;
        private DataColumn colPOPUPMSG;
        #endregion

        #region Constants
        /// <value>The constant used for Notes table. </value>
        private const String _TableName = "Notes";
        #endregion

        
      #region Constructors 
     internal NotesTable() : base(_TableName) { this.InitClass(); }
     internal NotesTable(DataTable table) : base(table.TableName) {}
     #endregion


     #region Properties
     /// 
     /// Public Property DataColumn all Rows in Table
     /// 
        public int Count
         {
             get
             {
                 return this.Rows.Count;
             }
         }
        /// <summary>
         /// Public Property DataColumn  NoteId
        /// </summary>
        public DataColumn NoteId
        {
            get
            {
                return this.colNoteId;
            }
        }
        /// <summary>
        /// Public Property DataColumn EntityId
        /// </summary>
        public DataColumn EntityId
        {
            get
            {
                return this.colEntityId;
            }
        }

        /// <summary>
        /// Public Property DataColumn EntityType
        /// </summary>
       public DataColumn EntityType
        {
            get
            {
                return this.colEntityType;
            }
        }

        /// <summary>
        /// Public Property DataColumn Note
        /// </summary>
        public DataColumn Note
        {
            get
            {
                return this.colNote;
            }
        }

        /// <summary>
        /// Public Property DataColumn CreatedBy
        /// </summary>
        public DataColumn CreatedBy
        {
            get
            {
                return this.colCreatedBy;
            }
        }

        /// <summary>
        /// Public Property DataColumn CreatedDate
        /// </summary>
        public DataColumn CreatedDate
        {
            get
            {
                return this.colCreatedDate;
            }
        }

        /// <summary>
        /// Public Property DataColumn UpdatedDate
        /// </summary>
        public DataColumn UpdatedDate
        {
            get
            {
                return this.colUpdatedDate;
            }
        }
        /// <summary>
        /// Public Property DataColumn UpdatedBy
        /// </summary>
        public DataColumn UpdatedBy
        {
            get
            {
                return this.colUpdatedBy;
            }
        }

        /// <summary>
        /// Public Property DataColumn POPUPMSG
        /// </summary>
        public DataColumn POPUPMSG
        {
            get
            {
                return this.colPOPUPMSG;
            }
        }

        #endregion

       

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }
        public override DataTable Clone()
        {
            NotesTable cln = (NotesTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new NotesTable();
        }

        #region Add and Get Methods

        public virtual void AddRow(NotesRow row)
        {
            AddRow(row, false);
        }

        public virtual void AddRow(NotesRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.NoteId) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

       public NotesRow AddRow(System.Int32 NoteID, System.String EntityId, System.String EntityType, System.String Note, System.DateTime CreatedDate, System.String CreatedBy,
            System.Object UpdatedDate, System.String UpdatedBy, System.Boolean POPUPMSG)
    
        {
            NotesRow row = (NotesRow)this.NewRow();
            row.ItemArray = new object[] { NoteID, EntityId,EntityType,Note, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, POPUPMSG };
            this.Rows.Add(row);
            return row;
        }

       
        public NotesRow GetRowByID(System.Int32 NoteID)
        {
            return (NotesRow)this.Rows.Find(new object[] { NoteID });
        }

        public virtual void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            NotesRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (NotesRow)this.NewRow();

                if (dr[clsPOSDBConstants.Notes_Fld_NoteId] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_NoteId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_NoteId] = Convert.ToInt32(dr[clsPOSDBConstants.Notes_Fld_NoteId].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_EntityId] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_EntityId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_EntityId] = Convert.ToString(dr[clsPOSDBConstants.Notes_Fld_EntityId].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_EntityType] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_EntityType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_EntityType] = Convert.ToString(dr[clsPOSDBConstants.Notes_Fld_EntityType].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_Note] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_Note] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_Note] = Convert.ToString(dr[clsPOSDBConstants.Notes_Fld_Note].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_CreatedDate] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_CreatedDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_CreatedDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Notes_Fld_CreatedDate].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_CreatedBy] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_CreatedBy] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_CreatedBy] = Convert.ToString(dr[clsPOSDBConstants.Notes_Fld_CreatedBy].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_UpdatedDate] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_UpdatedDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_UpdatedDate] = Convert.ToDateTime(dr[clsPOSDBConstants.Notes_Fld_UpdatedDate].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_UpdatedBy] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_UpdatedBy] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_UpdatedBy] = Convert.ToString(dr[clsPOSDBConstants.Notes_Fld_UpdatedBy].ToString());

                if (dr[clsPOSDBConstants.Notes_Fld_POPUPMSG] == DBNull.Value)
                    row[clsPOSDBConstants.Notes_Fld_POPUPMSG] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Notes_Fld_POPUPMSG] = Convert.ToString(dr[clsPOSDBConstants.Notes_Fld_POPUPMSG].ToString());

                this.AddRow(row);
            }
        }
        #endregion

        internal void InitVars()
        { 
            this.colNoteId = this.Columns[clsPOSDBConstants.Notes_Fld_NoteId];
            this.colEntityId = this.Columns[clsPOSDBConstants.Notes_Fld_EntityId];
            this.colEntityType = this.Columns[clsPOSDBConstants.Notes_Fld_EntityType];
            this.colNote = this.Columns[clsPOSDBConstants.Notes_Fld_Note];
            this.colCreatedDate = this.Columns[clsPOSDBConstants.Notes_Fld_CreatedDate];
            this.colCreatedBy = this.Columns[clsPOSDBConstants.Notes_Fld_CreatedBy];
            this.colUpdatedDate = this.Columns[clsPOSDBConstants.Notes_Fld_UpdatedDate];
            this.colUpdatedBy = this.Columns[clsPOSDBConstants.Notes_Fld_UpdatedBy];
            this.colPOPUPMSG = this.Columns[clsPOSDBConstants.Notes_Fld_POPUPMSG];
        }
        private void InitClass()
        {
            this.colNoteId = new DataColumn(clsPOSDBConstants.Notes_Fld_NoteId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNoteId);
            this.colNoteId.AllowDBNull = false;

            this.colEntityId = new DataColumn(clsPOSDBConstants.Notes_Fld_EntityId, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEntityId);
            this.colEntityId.AllowDBNull = true;

            this.colEntityType = new DataColumn(clsPOSDBConstants.Notes_Fld_EntityType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEntityType);
            this.colEntityType.AllowDBNull = true;

            this.colNote = new DataColumn(clsPOSDBConstants.Notes_Fld_Note, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNote);
            this.colNote.AllowDBNull = true;

            this.colCreatedDate = new DataColumn(clsPOSDBConstants.Notes_Fld_CreatedDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreatedDate);
            this.colCreatedDate.AllowDBNull = true;

            this.colCreatedBy = new DataColumn(clsPOSDBConstants.Notes_Fld_CreatedBy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreatedBy);
            this.colCreatedBy.AllowDBNull = true;

            this.colUpdatedDate = new DataColumn(clsPOSDBConstants.Notes_Fld_UpdatedDate, typeof(System.Object), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatedDate);
            this.colUpdatedDate.AllowDBNull = true;

            this.colUpdatedBy = new DataColumn(clsPOSDBConstants.Notes_Fld_UpdatedBy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatedBy);
            this.colUpdatedBy.AllowDBNull = true;

            this.colPOPUPMSG = new DataColumn(clsPOSDBConstants.Notes_Fld_POPUPMSG, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPOPUPMSG);
            this.colPOPUPMSG.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.NoteId };
        
        }
        public virtual NotesRow NewNotesRow()
        {
            return (NotesRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new NotesRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(NotesRow);
        }

        #region Event Handlers

        public delegate void NotesRowChangeEventHandler(object sender, NotesRowChangeEvent e);

        public event NotesRowChangeEventHandler NotesRowChanged;
        public event NotesRowChangeEventHandler NotesRowChanging;
        public event NotesRowChangeEventHandler NotesRowDeleted;
        public event NotesRowChangeEventHandler NotesRowDeleting;

        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            if (this.NotesRowChanged != null)
            {
                this.NotesRowChanged(this, new NotesRowChangeEvent((NotesRow)e.Row, e.Action));
            }
        }

        protected override void OnRowChanging(DataRowChangeEventArgs e)
        {
            base.OnRowChanging(e);
            if (this.NotesRowChanging != null)
            {
                this.NotesRowChanging(this, new NotesRowChangeEvent((NotesRow)e.Row, e.Action));
            }
        }

        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
            if (this.NotesRowDeleted != null)
            {
                this.NotesRowDeleted(this, new NotesRowChangeEvent((NotesRow)e.Row, e.Action));
            }
        }

        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
            if (this.NotesRowDeleting != null)
            {
                this.NotesRowDeleting(this, new NotesRowChangeEvent((NotesRow)e.Row, e.Action));
            }
        }

        #endregion
        
 }

}
