using System;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
//using POS.Resources;
namespace POS_Core.CommonData.Rows
{


    public class CCCustomerTokInfoRow : DataRow
    {
        private CCCustomerTokInfoTable table;

        internal CCCustomerTokInfoRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (CCCustomerTokInfoTable)this.Table;
        }


        #region "Public Properties"

        public Int32 EntryID
        {
            get
            {
                try
                {
                    return (Int32)this[this.table.EntryID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.EntryID] = value;
            }
        }

        public Int32 CustomerID
        {
            get
            {
                try
                {
                    return (Int32)this[this.table.CustomerID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.CustomerID] = value;
            }
        }

        public String CardType
        {
            get
            {
                return (String)this[this.table.CardType];
            }
            set
            {
                this[this.table.CardType] = value;
            }
        }

        public String Last4
        {
            get
            {
                return (String)this[this.table.Last4];
            }
            set
            {
                this[this.table.Last4] = value;
            }
        }

        public String ProfiledID
        {
            get
            {
                return (String)this[this.table.ProfiledID];
            }
            set
            {
                this[this.table.ProfiledID] = value;
            }
        }

        public String Processor
        {
            get
            {
                return (String)this[this.table.Processor];
            }
            set
            {
                this[this.table.Processor] = value;
            }
        }

        public String EntryType
        {
            get
            {
                return (String)this[this.table.EntryType];
            }
            set
            {
                this[this.table.EntryType] = value;
            }
        }

        //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
        public System.DateTime TokenDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.TokenDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.TokenDate] = value; }
        }

        public System.DateTime ExpDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.ExpDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.ExpDate] = value; }
        }

        //PRIMEPOS-2634 30-Jan-2019 JY Added
        public String CardAlias
        {
            get
            {
                return (String)this[this.table.CardAlias];
            }
            set
            {
                this[this.table.CardAlias] = value;
            }
        }

        //PRIMEPOS-2635 31-Jan-2019 JY Added
        public int PreferenceId
        {
            get
            {
                return (int)this[this.table.PreferenceId];
            }
            set
            {
                this[this.table.PreferenceId] = value;
            }
        }

        public bool IsFsaCard//2990
        {
            get
            {
                return (Boolean)this[this.table.IsFsaCard];
            }
            set
            {
                this[this.table.IsFsaCard] = value;
            }
        }
        #endregion
    }
}
