//Sprint-22 - PRIMEPOS-2245 15-Oct-2015 JY Added class
namespace POS_Core.CommonData.Rows 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    //using POS.Resources;

    public class SystemInfoRow : DataRow 
    {
        private SystemInfoTable table;

        internal SystemInfoRow(DataRowBuilder rb) : base(rb) 
		{
            this.table = (SystemInfoTable)this.Table;
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
            set { this[this.table.Id] = value; }
        }

        public System.String PharmNo
        {
            get
            {
                return this[this.table.PharmNo].ToString();
            }
            set 
            { 
                this[this.table.PharmNo] = value; 
            }
        }

        public System.String OSName
        {
            get
            {
                return this[this.table.OSName].ToString();
            }
            set
            {
                this[this.table.OSName] = value;
            }
        }

        public System.String Version
        {
            get
            {
                return this[this.table.Version].ToString();
            }
            set
            {
                this[this.table.Version] = value;
            }
        }

        public System.String SystemName
        {
            get
            {
                return this[this.table.SystemName].ToString();
            }
            set
            {
                this[this.table.SystemName] = value;
            }
        }

        public System.String SystemManufacturer
        {
            get
            {
                return this[this.table.SystemManufacturer].ToString();
            }
            set
            {
                this[this.table.SystemManufacturer] = value;
            }
        }

        public System.String SystemModel
        {
            get
            {
                return this[this.table.SystemModel].ToString();
            }
            set
            {
                this[this.table.SystemModel] = value;
            }
        }

        public System.String SystemType
        {
            get
            {
                return this[this.table.SystemType].ToString();
            }
            set
            {
                this[this.table.SystemType] = value;
            }
        }

        public System.String Processor
        {
            get
            {
                return this[this.table.Processor].ToString();
            }
            set
            {
                this[this.table.Processor] = value;
            }
        }

        public System.String RAM
        {
            get
            {
                return this[this.table.RAM].ToString();
            }
            set
            {
                this[this.table.RAM] = value;
            }
        }

        public System.String DriveInfo
        {
            get
            {
                return this[this.table.DriveInfo].ToString();
            }
            set
            {
                this[this.table.DriveInfo] = value;
            }
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
                    return Convert.ToDateTime("1/1/1753 12:00:00"); //return System.DateTime.MinValue; 
                }
            }
            set { this[this.table.UpdatedOn] = value; }
        }

        public System.Boolean Status
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.Status];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.Status] = value; }
        }
        #endregion 
    }
}
