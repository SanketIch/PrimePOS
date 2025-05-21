

namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class Util_UserOptionDetailRightsRow : DataRow 
    {
        private Util_UserOptionDetailRightsTable table;
        internal Util_UserOptionDetailRightsRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (Util_UserOptionDetailRightsTable)this.Table;
		}

       
        public System.Int64 ID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ID] = value; }
        }
        public System.Boolean isAllowed
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.isAllowed];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.isAllowed] = value; }
        }
        public System.Int32 ModuleID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ModuleID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ModuleID] = value; }
        }
        public System.Int32 PermissionId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PermissionId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PermissionId] = value; }
        }
        public System.Int32 ScreenID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ScreenID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ScreenID] = value; }
        }
        public System.Int32 DetailId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.DetailId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.DetailId] = value; }
        }
        public System.String UserID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.UserID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.UserID] = value; }
        }


        

    }
}
