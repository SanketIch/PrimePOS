using POS_Core.CommonData.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.CommonData.Rows
{
    public class ConsentTransmissionDataRow : DataRow
    {
        private ConsentTransmissionDataTable table;

        internal ConsentTransmissionDataRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (ConsentTransmissionDataTable)this.Table;
        }
        #region Public Properties
        public int ConsentLogId
        {
            get
            {
                try
                {
                    return (int)this[this.table.ConsentLogId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ConsentLogId] = value;
            }
        }
        public int ConsentSourceID
        {
            get
            {
                try
                {
                    return (int)this[this.table.ConsentSourceID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ConsentSourceID] = value;
            }
        }
        public int ConsentTypeId
        {
            get
            {
                try
                {
                    return (int)this[this.table.ConsentTypeId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ConsentTypeId] = value;
            }
        }
        public int ConsentTextId
        {
            get
            {
                try
                {
                    return (int)this[this.table.ConsentTextId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ConsentTextId] = value;
            }
        }
        public int ConsentStatusId
        {
            get
            {
                try
                {
                    return (int)this[this.table.ConsentStatusId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ConsentStatusId] = value;
            }
        }
        public int ConsentRelationId
        {
            get
            {
                try
                {
                    return (int)this[this.table.ConsentRelationId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ConsentRelationId] = value;
            }
        }
        public int PatientNo
        {
            get
            {
                try
                {
                    return (int)this[this.table.PatientNo];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.PatientNo] = value;
            }
        }
        public int Nrefill
        {
            get
            {
                try
                {
                    return (int)this[this.table.Nrefill];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.Nrefill] = value;
            }
        }
        public long RxNo
        {
            get
            {
                try
                {
                    return (long)this[this.table.RxNo];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.RxNo] = value;
            }
        }
        public DateTime ConsentCaptureDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this[this.table.ConsentCaptureDate]);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                this[this.table.ConsentCaptureDate] = value;
            }
        }
        public DateTime ConsentExpiryDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this[this.table.ConsentExpiryDate]);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                this[this.table.ConsentExpiryDate] = value;
            }
        }
        public string SigneeName
        {
            get
            {
                try
                {
                    return this[this.table.SigneeName].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                this[this.table.SigneeName] = value;
            }
        }
        public byte[] SignatureData
        {
            get
            {
                try
                {
                    return (byte[])this[this.table.SignatureData];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                this[this.table.SignatureData] = value;
            }
        }
        #endregion  
        public void Copy(ConsentTransmissionDataRow oCopyTo)
        {
            oCopyTo.ConsentLogId = this.ConsentLogId;
            oCopyTo.ConsentSourceID = this.ConsentSourceID;
            oCopyTo.ConsentTypeId = this.ConsentTypeId;
            oCopyTo.ConsentTextId = this.ConsentTextId;
            oCopyTo.ConsentStatusId = this.ConsentStatusId;
            oCopyTo.ConsentRelationId = this.ConsentRelationId;
            oCopyTo.PatientNo = this.PatientNo;
            oCopyTo.ConsentCaptureDate = this.ConsentCaptureDate;
            oCopyTo.ConsentExpiryDate = this.ConsentExpiryDate;
            oCopyTo.SigneeName = this.SigneeName;
            oCopyTo.SignatureData = this.SignatureData;
        }
    }
}
