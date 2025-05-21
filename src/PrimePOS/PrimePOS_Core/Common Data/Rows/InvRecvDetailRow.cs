
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class InvRecvDetailRow : DataRow 
	{
		private InvRecvDetailTable table;

		internal InvRecvDetailRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (InvRecvDetailTable)this.Table;
		}
        #region Public Properties

        public System.Int32 InvRecvDetailID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.InvRecvDetailID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.InvRecvDetailID] = value; }
        }

        public System.Int32 InvRecievedID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.InvRecievedID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.InvRecievedID] = value; }
        }

        public System.Int32 QtyOrdered
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.QtyOrdered];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.QtyOrdered] = value; }
        }

        public System.Int32 QTY
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.QTY];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.QTY] = value; }
        }

        public System.Decimal SalePrice
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.SalePrice];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.SalePrice] = value; }
        }

        public System.Decimal Cost
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.Cost];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.Cost] = value; }
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
                    return "";
                }
            }
            set { this[this.table.ItemID] = value; }
        }

        public System.String ItemDescription
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ItemDescription];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ItemDescription] = value; }
        }

        public System.String Comments
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Comments];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.Comments] = value; }
        }

        //Sprint-21 - 2002 21-Jul-2015 JY Added
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
            set { this[this.table.TotalCost] = value; }
        }

        //Sprint-21 - 2207 03-Aug-2015 JY Added
        public System.String VendorItemID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.VendorItemID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.VendorItemID] = value; }
        }

        public System.String VendorCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.VendorCode];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.VendorCode] = value; }
        }

        #region Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added
        /// <summary>
        /// Public Property Expiration Date
        /// </summary>
        public System.Object ExpDate
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.ExpDate];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.ExpDate] = value; }
        }
        #endregion

        #region PRIMEPOS-2396 12-Jun-2018 JY Added
        public System.Int32 QtyInStock
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.QtyInStock];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.QtyInStock] = value; }
        }
        public System.Int32 LastInvUpdatedQty
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.LastInvUpdatedQty];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.LastInvUpdatedQty] = value; }
        }
        #endregion

        #region PRIMEPOS-2419 28-May-2019 JY Added
        public System.Int32 DeptID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.DeptID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.DeptID] = value; }
        }

        public System.Int32 SubDepartmentID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.SubDepartmentID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.SubDepartmentID] = value; }
        }

        public System.Boolean IsEBTItem
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsEBTItem];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsEBTItem] = value; }
        }
        #endregion

        #region PRIMEPOS-2725 29-Aug-2019 JY Added
        public System.String DeptCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DeptCode];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.DeptCode] = value; }
        }

        public System.String DeptName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DeptName];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.DeptName] = value; }
        }

        public System.String SubDept
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SubDept];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.SubDept] = value; }
        }

        #endregion

        #endregion
    }
}
