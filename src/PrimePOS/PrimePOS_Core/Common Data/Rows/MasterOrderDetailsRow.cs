using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData.Rows
{
    public partial class MasterOrderDetailsRow : DataRow
    {
        private MasterOrderDetailsTable table;

        internal MasterOrderDetailsRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (MasterOrderDetailsTable)this.Table;
        }

        public System.Int32 VendorID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.VendorID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.VendorID] = value; 
            }
        }

        public System.String  VendorName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.VendorName];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.VendorName] = value;
            }
        }

        public System.Int32 TotalPOs
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TotalPOs];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.TotalPOs] = value; 
            }
        }
        public System.String TimeToOrder
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TimeToOrder];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.TimeToOrder] = value;
            }
        }
    }
}
