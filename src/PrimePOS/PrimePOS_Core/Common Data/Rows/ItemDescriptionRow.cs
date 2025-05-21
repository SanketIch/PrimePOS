	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData.Rows
{
   public class ItemDescriptionRow : DataRow
    {
        private ItemDescriptionTable table;

        internal ItemDescriptionRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (ItemDescriptionTable)this.Table;
        }

        #region Public Properties

        public System.Int32 ID
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

        public System.Int32 LanguageId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.LanguageId];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LanguageId] = value; }
        }

        public System.String Description
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Description];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.Description] = value; }
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
                    return System.String.Empty;
                }
            }
            set { this[this.table.UserID] = value; }
        }

        public System.String ItemID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ItemID];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ItemID] = value; }
        }
        public System.String Language
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Language];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.Language] = value; }
        }

        #endregion
    }
}
