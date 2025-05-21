// ----------------------------------------------------------------
//Added By Shitaljit(QuicSolv) 0n 5 oct 2011
// ----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
//using POS.Resources;

namespace POS_Core.CommonData.Rows
{
    public class NotesRow : DataRow
    {
        #region Declaration
        private NotesTable table;
        #endregion

        internal NotesRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (NotesTable)this.Table;
        }

        #region Public Properties

        /// <summary>
        /// Public Property NoteId
        /// </summary>
        public System.Int32 NoteId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.NoteId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.NoteId] = value; }
        }
        /// <summary>
        ///  Public Property EntityId
        /// </summary>
        public System.String EntityId
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EntityId];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.EntityId] = value; }

        }

        /// <summary>
        ///  Public Property EntityType
        /// </summary>
        public System.String EntityType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EntityType];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.EntityType] = value; }

        }

        /// <summary>
        /// Public Property  Note
        /// </summary>
        public System.String Note
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Note];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.Note] = value; }

        }
        /// <summary>
        /// Public Property CreatedDate
        /// </summary>
        public System.DateTime CreatedDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.CreatedDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.CreatedDate] = value; }

        }

        /// <summary>
        /// Public Property  CreatedBy
        /// </summary>
        public System.String CreatedBy
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CreatedBy];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.CreatedBy] = value; }

        }
        /// <summary>
        /// Public Property UpdatedDate
        /// </summary>
        public System.Object UpdatedDate
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.UpdatedDate];
                }
                catch
                {
                   return DBNull.Value;
                }
            }
            set { this[this.table.UpdatedDate] = value; }

        }

        /// <summary>
        /// Public Property  UpdatedBy
        /// </summary>
        public System.String UpdatedBy
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.UpdatedBy];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.UpdatedBy] = value; }

        }
        /// <summary>
        /// Public Property  POPUPMSG
        /// </summary>
        public System.Boolean POPUPMSG
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.POPUPMSG];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.POPUPMSG] = value; }

        }
        #endregion
    }

        public class NotesRowChangeEvent : EventArgs
        {
            private NotesRow eventRow;
            private DataRowAction eventAction;

            public NotesRowChangeEvent(NotesRow row, DataRowAction action)
            {
                this.eventRow = row;
                this.eventAction = action;
            }

            public NotesRow Row
            {
                get { return this.eventRow; }
            }

            public DataRowAction Action
            {
                get { return this.eventAction; }
            }
        
    }
}
