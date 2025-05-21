namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Data;
    using POS_Core.CommonData.Rows;

    public class MsgTemplateTable : DataTable
    {
        private DataColumn colRecID;
        private DataColumn colMessageCode;
        private DataColumn colMessage;
        private DataColumn colMessageSub;
        private DataColumn colMessageCatId;
        private DataColumn colMessageTypeId;

        #region Constructors 
        internal MsgTemplateTable() : base(clsPOSDBConstants.FMessage_tbl) { this.InitClass(); }
        internal MsgTemplateTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties // Public Property for get all Rows in Table        
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public MsgTemplateRow this[int index]
        {
            get
            {
                return ((MsgTemplateRow)(this.Rows[index]));
            }
        }

        public DataColumn RecID
        {
            get
            {
                return this.colRecID;
            }
        }

        public DataColumn MessageCode
        {
            get
            {
                return this.colMessageCode;
            }
        }

        public DataColumn Message
        {
            get
            {
                return this.colMessage;
            }
        }

        public DataColumn MessageSub
        {
            get
            {
                return this.colMessageSub;
            }
        }

        public DataColumn MessageCatId
        {
            get
            {
                return this.colMessageCatId;
            }
        }

        public DataColumn MessageTypeId
        {
            get
            {
                return this.colMessageTypeId;
            }
        }
        #endregion

        #region Add and Get Methods 
        public void AddRow(MsgTemplateRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(MsgTemplateRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.RecID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public MsgTemplateRow AddRow(System.Int32 RecID, System.String MessageCode, System.String Message, System.String MessageSub, System.Int32 MessageCatId, System.Int32 MessageTypeId)
        {

            MsgTemplateRow row = (MsgTemplateRow)this.NewRow();
            row.ItemArray = new object[] {RecID, MessageCode, Message, MessageSub, MessageCatId, MessageTypeId};
            this.Rows.Add(row);
            return row;
        }

        public MsgTemplateRow GetRowByID(System.Int32 RecID)
        {
            return (MsgTemplateRow)this.Rows.Find(new object[] { RecID });
        }

        public void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            MsgTemplateRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (MsgTemplateRow)this.NewRow();

                if (dr[clsPOSDBConstants.FMessage_Fld_RecID] == DBNull.Value)
                    row[clsPOSDBConstants.FMessage_Fld_RecID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.FMessage_Fld_RecID] = Convert.ToInt32(dr[clsPOSDBConstants.FMessage_Fld_RecID].ToString());

                if (dr[clsPOSDBConstants.FMessage_Fld_MessageCode] == DBNull.Value)
                    row[clsPOSDBConstants.FMessage_Fld_MessageCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.FMessage_Fld_MessageCode] = Convert.ToString(dr[clsPOSDBConstants.FMessage_Fld_MessageCode].ToString());

                if (dr[clsPOSDBConstants.FMessage_Fld_Message] == DBNull.Value)
                    row[clsPOSDBConstants.FMessage_Fld_Message] = DBNull.Value;
                else
                    row[clsPOSDBConstants.FMessage_Fld_Message] = Convert.ToString(dr[clsPOSDBConstants.FMessage_Fld_Message].ToString());

                if (dr[clsPOSDBConstants.FMessage_Fld_MessageSub] == DBNull.Value)
                    row[clsPOSDBConstants.FMessage_Fld_MessageSub] = DBNull.Value;
                else
                    row[clsPOSDBConstants.FMessage_Fld_MessageSub] = Convert.ToString(dr[clsPOSDBConstants.FMessage_Fld_MessageSub].ToString());
                
                if (dr[clsPOSDBConstants.FMessage_Fld_MessageCatId] == DBNull.Value)
                    row[clsPOSDBConstants.FMessage_Fld_MessageCatId] = Convert.ToInt32(row[clsPOSDBConstants.FMessage_Fld_MessageCatId].ToString());
                else
                    row[clsPOSDBConstants.FMessage_Fld_MessageCatId] = Convert.ToInt32(dr[clsPOSDBConstants.FMessage_Fld_MessageCatId].ToString());

                if (dr[clsPOSDBConstants.FMessage_Fld_MessageTypeId] == DBNull.Value)
                    row[clsPOSDBConstants.FMessage_Fld_MessageTypeId] = 0;
                else
                    row[clsPOSDBConstants.FMessage_Fld_MessageTypeId] = Convert.ToInt32(dr[clsPOSDBConstants.FMessage_Fld_MessageTypeId].ToString());
                
                this.AddRow(row);
            }
        }
        #endregion

        protected override DataTable CreateInstance()
        {
            return new MsgTemplateTable();
        }

        internal void InitVars()
        {
            try
            {
                this.colRecID = this.Columns[clsPOSDBConstants.FMessage_Fld_RecID];
                this.colMessageCode = this.Columns[clsPOSDBConstants.FMessage_Fld_MessageCode];
                this.colMessage = this.Columns[clsPOSDBConstants.FMessage_Fld_Message];
                this.colMessageSub = this.Columns[clsPOSDBConstants.FMessage_Fld_MessageSub];
                this.colMessageCatId = this.Columns[clsPOSDBConstants.FMessage_Fld_MessageCatId];
                this.colMessageTypeId = this.Columns[clsPOSDBConstants.FMessage_Fld_MessageTypeId];
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colRecID = new DataColumn(clsPOSDBConstants.FMessage_Fld_RecID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRecID);
            this.colRecID.AllowDBNull = false;

            this.colMessageCode = new DataColumn(clsPOSDBConstants.FMessage_Fld_MessageCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMessageCode);
            this.colMessageCode.AllowDBNull = true;

            this.colMessage = new DataColumn(clsPOSDBConstants.FMessage_Fld_Message, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMessage);
            this.colMessage.AllowDBNull = true;

            this.colMessageSub = new DataColumn(clsPOSDBConstants.FMessage_Fld_MessageSub, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMessageSub);
            this.colMessageSub.AllowDBNull = true;

            this.colMessageCatId = new DataColumn(clsPOSDBConstants.FMessage_Fld_MessageCatId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMessageCatId);
            this.colMessageCatId.AllowDBNull = true;

            this.colMessageTypeId = new DataColumn(clsPOSDBConstants.FMessage_Fld_MessageTypeId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMessageTypeId);
            this.colMessageTypeId.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colMessageCode };
        }

        public MsgTemplateRow NewMsgTemplateRow()
        {
            return (MsgTemplateRow)this.NewRow();
        }

        public override DataTable Clone()
        {
            MsgTemplateTable cln = (MsgTemplateTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new MsgTemplateRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(MsgTemplateRow);
        }
    }
}
