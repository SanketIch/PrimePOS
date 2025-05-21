namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class UserRow : DataRow
    {
        private UserTable table;

        // Constructor
        internal UserRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (UserTable)this.Table;
        }
        #region Public Properties

        public System.Int32 Id
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.Id];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {

                if (value != 0) this[this.table.Id] = value;
            }
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
            set { if (value != "") this[this.table.UserID] = value; }
        }
        
        public System.String Password
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Password];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { if (value != "") this[this.table.Password] = value; }
        }

        public System.String FirstName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.FirstName];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.FirstName] = value; }
        }
        
        public System.String LastName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.LastName];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { if (value != "") this[this.table.LastName] = value; }
        }

        public System.Int32 DrawNo
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.DrawNo];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {

                if (value != 0) this[this.table.DrawNo] = value;
            }
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

        public System.Decimal MaxDiscountLimit
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.MaxDiscountLimit];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.MaxDiscountLimit] = value; }
        }
        public System.Decimal MaxTransactionLimit//Added by Ravindra
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.MaxTransactionLimit];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.MaxTransactionLimit] = value; }
        
        }
        public System.Byte[] UserImage//Added by Ravindra
        {
            get
            {
                try
                {
                    return (System.Byte[])this[this.table.UserImage];
                }
                catch
                {
                     return null;;
                }
            }
            set { this[this.table.UserImage] = value; }
        }

        #endregion // Properties
    }
}
