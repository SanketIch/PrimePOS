using System;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData.Rows
{
    public partial class OrderDetailRow : DataRow
    {
        private OrderDetalis table;

        internal OrderDetailRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (OrderDetalis)this.Table;
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

        public System.Int32 OrderNo
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.OrderNO];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.OrderNO] = value; 
            }
        }
        public System.String  OrderDescription
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.OrderDescription];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.OrderDescription] = value;
            }
        }
        public System.Int32 OrderID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.OrderID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.OrderID] = value;
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

        public System.Int32 TotalItems
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TotalItems];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.TotalItems] = value; 
            }
        }

        public System.Int32 TotalQty
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TotalQty];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.TotalQty] = value; 
            }
        }

        public System.Decimal TotalCost
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.TotalCost];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.TotalCost] = value; 
            }
        }
        public System.String AutoSend
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.AutoSend];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.AutoSend] = value; 
            }
        }
        public System.String CloseOrder
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CloseOrder];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.CloseOrder] = value;
            }
        }       
    }
}

