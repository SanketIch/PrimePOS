namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using Resources;

    //using POS.Resources;


    public class Util_UserOptionDetailRightsTable : DataTable, System.Collections.IEnumerable 
    {
        private DataColumn colID;
        private DataColumn colUserID;
        private DataColumn colScreenID;
        private DataColumn colModuleID;
        private DataColumn colDetailId;
        private DataColumn colPermissionId;
        private DataColumn colisAllowed;
        #region Constructors 
        internal Util_UserOptionDetailRightsTable() : base(clsPOSDBConstants.Util_UserOptionDetailRights_tbl) { this.InitClass(); }
        internal Util_UserOptionDetailRightsTable(DataTable table) : base(table.TableName) { }
		#endregion
        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }
        protected override DataTable CreateInstance()
        {
            return new Util_UserOptionDetailRightsTable();
        }
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public Util_UserOptionDetailRightsRow this[int index]
        {
            get
            {
                return ((Util_UserOptionDetailRightsRow)(this.Rows[index]));
            }
        }

        // Public Property DataColumn PayOutcode
        public DataColumn ID
        {
            get
            {
                return this.colID;
            }
        }
        
        public DataColumn ModuleID
        {
            get
            {
                return this.colModuleID;//ModuleID//ScreenID //PermissionId//isAllowed//DetailId
            }
        }
        public DataColumn ScreenID
        {
            get
            {
                return this.colScreenID;//ModuleID//ScreenID //PermissionId//isAllowed//DetailId
            }
        }
        public DataColumn PermissionId
        {
            get
            {
                return this.colPermissionId;//ModuleID//ScreenID //PermissionId//isAllowed//DetailId
            }
        }
        public DataColumn isAllowed
        {
            get
            {
                return this.colisAllowed;//ModuleID//ScreenID //PermissionId//isAllowed//DetailId
            }
        }
        public DataColumn DetailId
        {
            get
            {
                return this.colDetailId;//ModuleID//ScreenID //PermissionId//isAllowed//DetailId
            }
        }


        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }


        
        public void AddRow(DataRow row)
        {
            AddRow(row, false);
        }
        ////ModuleID//ScreenID //PermissionId//isAllowed//DetailId
        public void AddRow(DataRow row, bool preserveChanges)
        {
            if (this.GetRowByModuleID(Configuration.convertNullToInt64(row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID].ToString())) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public DataRow  AddRow(System.Int64 ID, System.String UserID, System.Int32 DetailID)
        {

            Util_UserOptionDetailRightsRow row = (Util_UserOptionDetailRightsRow)this.NewRow();
            row.ItemArray = new object[] { ID, UserID, DetailID };
            this.Rows.Add(row);
            return row;
        }

        public DataRow AddRow(System.Int64 ID, System.String UserID, System.Int32 ScreenID, System.Int32 ModuleID, System.Int32 PermissionId, System.Boolean isAllowed, System.Int32 DetailId)
        {
            try
            {
                DataRow row = this.NewRow();

               // POS_Core.CommonData.Rows.Util_UserOptionDetailRightsRow rowwr = new Util_UserOptionDetailRightsRow();
                row.ItemArray = new object[] { ID, UserID, ScreenID, ModuleID, PermissionId, isAllowed, DetailId };
                this.Rows.Add(row);
               // POS_Core.CommonData.Rows.Util_UserOptionDetailRightsRow roww = (POS_Core.CommonData.Rows.Util_UserOptionDetailRightsRow)row;
                return row;
            }
            catch (Exception ex) { return null; }
        }

        public DataRow GetRowByModuleID(System.Int64 ID)
        {
            return this.Rows.Find(new object[] { ID });
        }

        public void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            DataRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = this.NewRow();
                if (dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID] == DBNull.Value)
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID] = Convert.ToInt64(dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID].ToString());

                if (dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId] == DBNull.Value)
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId] = Convert.ToInt32(dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId].ToString());

                if (dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID] == DBNull.Value)
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID].ToString());
                
                if (dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID] == DBNull.Value)
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID] = Convert.ToInt32(dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID].ToString());
                if (dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId] == DBNull.Value)
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId] = Convert.ToInt32(dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId].ToString());

                if (dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID] == DBNull.Value)
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID] = Convert.ToInt32(dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID].ToString());

                if (dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed] == DBNull.Value)
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed] = Convert.ToBoolean(dr[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed].ToString());                
                

                this.AddRow(row);
            }
        }
		


        internal void InitVars()
        {
            try
            {

                this.colID = this.Columns[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID];
                this.colScreenID = this.Columns[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID];
                this.colUserID = this.Columns[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID];
                this.colPermissionId = this.Columns[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId];
                this.colisAllowed = this.Columns[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed];
                this.colModuleID = this.Columns[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID];
                this.colDetailId = this.Columns[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId];
                //this. = this.Columns[clsPOSDBConstants.PayOutCat_Fld_PayoutType];
                //this.colPayoutcatID = this.Columns[clsPOSDBConstants.PayOut_Fld_PayoutCatID];
            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }
        private void InitClass()
        {
            this.colID = new DataColumn(clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID, typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colID);

            this.colID.AllowDBNull = false;
            //this.colID.

            this.colUserID = new DataColumn(clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = false;

            this.colModuleID = new DataColumn(clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colModuleID);
            this.colModuleID.AllowDBNull = false;


            this.colPermissionId = new DataColumn(clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPermissionId);
            this.colUserID.AllowDBNull = true;

            this.colScreenID = new DataColumn(clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colScreenID);
            //this.colScreenID.AllowDBNull = true;



            this.colDetailId = new DataColumn(clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDetailId);
            //this.colDetailId.AllowDBNull = true;

            this.colisAllowed = new DataColumn(clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisAllowed);
            //this.colisAllowed.AllowDBNull = true;
            this.PrimaryKey = new DataColumn[] { this.colID };
        }
    }
}
