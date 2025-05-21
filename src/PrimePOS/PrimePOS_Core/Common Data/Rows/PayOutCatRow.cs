

    namespace POS_Core.CommonData.Rows
    {
        using System;
        using System.Data;
        using POS_Core.CommonData.Tables;
        using POS_Core.CommonData.Rows;

        public class PayOutCatRow : DataRow
    
        {
            private PayOutCatTable table;


		// Constructor
            internal PayOutCatRow(DataRowBuilder rb): base(rb) 
		{
            this.table = (PayOutCatTable)this.Table;
		}
            #region Public Properties
            public System.Int32 ID
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
                set { this[this.table.Id] = value; }
            }

            public System.String PayoutCatType
            {
                get
                {
                    try
                    {
                        return (System.String)this[this.table.PayoutCatType];
                    }
                    catch
                    {
                        return System.String.Empty;
                    }
                }
                set { this[this.table.PayoutCatType] = value; }
            }
            public System.String DefaultDescription
            {
                get
                {
                    try
                    {
                        return (System.String)this[this.table.DefaultDescription];
                    }
                    catch
                    {
                        return System.String.Empty;
                    }
                }
                set { this[this.table.DefaultDescription] = value; }
            }

            
            public System.String UserId
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



            #endregion
        }
    }