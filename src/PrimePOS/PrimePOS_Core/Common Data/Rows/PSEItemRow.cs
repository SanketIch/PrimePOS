using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Rows
{
    public class PSEItemRow : DataRow
    {
        private PSEItemTable table;

        internal PSEItemRow(DataRowBuilder rb) : base(rb) 
		{
            this.table = (PSEItemTable)this.Table;
        }
        #region Public Properties

        public System.Int32 Id
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.id];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.id] = value; }
        }

        public System.String ProductId
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProductId];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ProductId] = value; }
        }

        public System.String ProductName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProductName];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ProductName] = value; }
        }

        public System.String ProductNDC
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProductNDC];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.ProductNDC] = value; }
        }

        public System.Decimal ProductGrams
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.ProductGrams];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ProductGrams] = value; }
        }

        public System.Int32 ProductPillCnt
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ProductPillCnt];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ProductPillCnt] = value; }
        }

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

        public System.DateTime CreatedOn
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.CreatedOn];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.CreatedOn] = value; }
        }

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

        public System.DateTime UpdatedOn
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.UpdatedOn];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.UpdatedOn] = value; }
        }

        public System.Boolean IsActive
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsActive];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsActive] = value; }
        }

        public System.String RecordStatus
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.RecordStatus];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.RecordStatus] = value; }
        }
        #endregion
    }
}
