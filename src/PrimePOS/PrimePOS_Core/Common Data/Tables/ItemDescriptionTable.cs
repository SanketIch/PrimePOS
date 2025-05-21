
namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class ItemDescriptionTable : DataTable, System.Collections.IEnumerable
    {

        private DataColumn colID;
        private DataColumn colDescription;
        private DataColumn colUserID;
        private DataColumn colLanguageId;
        private DataColumn colItemID;
        private DataColumn colLanguage;

        #region Constants
        private const String _TableName = clsPOSDBConstants.ItemDescription_tbl;
        #endregion

        #region Constructors
        internal ItemDescriptionTable() : base(_TableName) { this.InitClass(); }
        internal ItemDescriptionTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ItemDescriptionRow this[int index]
        {
            get
            {
                return ((ItemDescriptionRow)(this.Rows[index]));
            }
        }

        public DataColumn ID
        {
            get
            {
                return this.colID;
            }
        }
        public DataColumn LanguageId
        {
            get
            {
                return this.colLanguageId;
            }
        }
        public DataColumn Description
        {
            get
            {
                return this.colDescription;
            }
        }
        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }
        public DataColumn ItemID
        {
            get
            {
                return this.colItemID;
            }
        }
        public DataColumn Language
        {
            get
            {
                return this.colLanguage;
            }
        }
       

        #endregion //Properties

        #region Add and Get Methods

        public void AddRow(ItemDescriptionRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ItemDescriptionRow row, bool preserveChanges)
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

        public ItemDescriptionRow AddRow(System.Int32 ID, System.String Description, System.String UserID,System.Int32 ItemMonCatID,string ItemID)
        {
            ItemDescriptionRow row = (ItemDescriptionRow)this.NewRow();
            row.ItemArray = new object[] { ID, Description, UserID, ItemMonCatID, ItemID };
            this.Rows.Add(row);
            return row;
        }

        public ItemDescriptionRow GetRowByID(System.Int32 iID)
        {
            return (ItemDescriptionRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt)
        {

            ItemDescriptionRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ItemDescriptionRow)this.NewRow();

                if (dr[clsPOSDBConstants.ItemDescription_Fld_ID] == DBNull.Value)
                    row.ID = 0;
                else
                    row.ID = Convert.ToInt32((dr[clsPOSDBConstants.ItemDescription_Fld_ID].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemDescription_Fld_ID].ToString());

                if (dr[clsPOSDBConstants.ItemDescription_Fld_Description] == DBNull.Value)
                    row.Description= "";
                else
                    row.Description = Convert.ToString(dr[clsPOSDBConstants.ItemDescription_Fld_Description].ToString());

                if (dr[clsPOSDBConstants.fld_UserID] == DBNull.Value)
                    row.UserID = "";
                else
                    row.UserID= Convert.ToString(dr[clsPOSDBConstants.Users_Fld_UserID].ToString());

                if (dr[clsPOSDBConstants.ItemDescription_Fld_LanguageId] == DBNull.Value)
                    row.LanguageId = 0;
                else
                    row.LanguageId = Convert.ToInt32((dr[clsPOSDBConstants.ItemDescription_Fld_LanguageId].ToString() == "") ? "0" : dr[clsPOSDBConstants.ItemDescription_Fld_LanguageId].ToString());

                if (dr[clsPOSDBConstants.Item_Fld_ItemID] == DBNull.Value)
                    row.ItemID= "";
                else
                    row.ItemID = Convert.ToString(dr[clsPOSDBConstants.Item_Fld_ItemID].ToString());

                if (dr[clsPOSDBConstants.Language_tbl] == DBNull.Value)
                    row.Language = "";
                else
                    row.Language = Convert.ToString(dr[clsPOSDBConstants.Language_tbl].ToString());

                this.AddRow(row);
            }
        }

        #endregion

        public override DataTable Clone()
        {
            ItemDescriptionTable cln = (ItemDescriptionTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new ItemDescriptionTable();
        }

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.ItemDescription_Fld_ID];
            this.colDescription = this.Columns[clsPOSDBConstants.ItemDescription_Fld_Description];
            this.colUserID = this.Columns[clsPOSDBConstants.fld_UserID];
            this.colLanguageId= this.Columns[clsPOSDBConstants.ItemDescription_Fld_LanguageId];
            this.colItemID = this.Columns[clsPOSDBConstants.Item_Fld_ItemID];
            colLanguage = this.Columns[clsPOSDBConstants.Language_tbl];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colID = new DataColumn(clsPOSDBConstants.ItemDescription_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colID);
            this.colID.AllowDBNull = false;

            this.colDescription = new DataColumn(clsPOSDBConstants.ItemDescription_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDescription);
            this.colDescription.AllowDBNull = false;

            this.colUserID = new DataColumn(clsPOSDBConstants.fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = true;

            this.colLanguageId = new DataColumn(clsPOSDBConstants.ItemDescription_Fld_LanguageId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLanguageId);
            this.colUserID.AllowDBNull = true;

            this.colItemID= new DataColumn(clsPOSDBConstants.Item_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemID);
            this.colItemID.AllowDBNull = true;

            this.colLanguage = new DataColumn(clsPOSDBConstants.Language_tbl, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLanguage);
            this.colLanguage.AllowDBNull = true;
           
            this.PrimaryKey = new DataColumn[] { this.ID };
        }

        public ItemDescriptionRow NewItemDescriptionRow()
        {
            return (ItemDescriptionRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ItemDescriptionRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ItemDescriptionRow);
        }

        public int GetNextID()
        {
            if (this.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                Int32 MaxNo = 0;
                foreach (ItemDescriptionRow oRow in this.Rows)
                {
                    if (oRow.RowState != DataRowState.Deleted)
                    {
                        if (oRow.ID > MaxNo)
                        {
                            MaxNo = oRow.ID;
                        }
                    }
                }
                return MaxNo + 1;
            }
        }
    }
}
