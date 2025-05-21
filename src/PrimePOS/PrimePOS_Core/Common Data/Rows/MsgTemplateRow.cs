namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using POS_Core.CommonData.Tables;
    using System.Data;

    public class MsgTemplateRow : BaseRow
    {
        private MsgTemplateTable table;

        // Constructor
        internal MsgTemplateRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (MsgTemplateTable)this.Table;
        }

        #region Public Properties
        public System.Int32 RecID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.RecID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                if (value != 0) this[this.table.RecID] = value;
            }
        }

        public System.String MessageCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.MessageCode];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.MessageCode] = value;
            }
        }

        public System.String Message
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Message];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.Message] = value;
            }
        }

        public System.String MessageSub
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.MessageSub];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.MessageSub] = value;
            }
        }

        public System.Int32 MessageCatId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.MessageCatId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.MessageCatId] = value;
            }
        }

        public System.Int32 MessageTypeId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.MessageTypeId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.MessageTypeId] = value;
            }
        }
        #endregion
    }
}
