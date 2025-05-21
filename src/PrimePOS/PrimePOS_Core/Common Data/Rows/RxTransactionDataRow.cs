using System;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using Resources;
using POS_Core.Resources;
using System.Data.SqlTypes;

namespace POS_Core.CommonData.Rows
{
    public class RxTransactionDataRow : DataRow
    {
        private RxTransactionDataTable table;

        internal RxTransactionDataRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (RxTransactionDataTable)this.Table;
        }

        #region Public Properties
        public System.Int32 RxTransNo
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.RxTransNo];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.RxTransNo] = value;
            }
        }
        public System.Int32 PatientID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PatientID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.PatientID] = value;
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

        public System.Int32 Nrefill
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.Nrefill];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.Nrefill] = value; }
        }

        public System.Int32 PartialFillNo
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PartialFillNo];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PartialFillNo] = value; }
        }

        public System.String PickedUp
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PickedUp];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.PickedUp] = value;
            }
        }

        public System.String PickUpPOS
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PickUpPOS];
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                this[this.table.PickUpPOS] = value;
            }
        }

        public System.Int32 TransID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransID] = value; }
        }

        public System.Boolean IsDelivery
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsDelivery];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsDelivery] = value; }
        }

        public System.Boolean IsRxSync
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsRxSync];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsRxSync] = value; }
        }

        public System.Decimal TransAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.TransAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransAmount] = value; }
        }

        public System.String StationID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.StationID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.StationID] = value; }
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
                    return "";
                }
            }
            set { this[this.table.UserID] = value; }
        }

        public System.DateTime TransDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.TransDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.TransDate] = value; }
        }


        public System.Int32 ConsentTextID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ConsentTextID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ConsentTextID] = value; }
        }

        public System.Int32 ConsentTypeID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ConsentTypeID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ConsentTypeID] = value; }
        }


        public System.Int32 ConsentStatusID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ConsentStatusID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ConsentStatusID] = value; }
        }
        public System.DateTime ConsentCaptureDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.ConsentCaptureDate];
                }
                catch
                {
                    return Convert.ToDateTime(SqlDateTime.MinValue.ToString());
                }
            }
            set { this[this.table.ConsentCaptureDate] = value; }
        }

        public System.DateTime ConsentEffectiveDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.ConsentEffectiveDate];
                }
                catch
                {
                    return Convert.ToDateTime(SqlDateTime.MinValue.ToString());
                }
            }
            set { this[this.table.ConsentEffectiveDate] = value; }
        }

        public System.DateTime ConsentEndDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.ConsentEndDate];
                }
                catch
                {
                    return Convert.ToDateTime(SqlDateTime.MinValue.ToString());
                }
            }
            set { this[this.table.ConsentEndDate] = value; }
        }

        public System.Int32 PatConsentRelationID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PatConsentRelationID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PatConsentRelationID] = value; }
        }

        public System.String SigneeName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SigneeName];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.SigneeName] = value; }
        }

        public System.Byte[] SignatureData
        {
            get
            {
                try
                {
                    return (System.Byte[])this[this.table.SignatureData];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.SignatureData] = value; }
        }

        public System.DateTime PickUpDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.PickUpDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.PickUpDate] = value; }
        }

        public System.Boolean CopayPaid
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.CopayPaid];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.CopayPaid] = value; }
        }

        public System.DateTime PackDATESIGNED
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.PackDATESIGNED];
                }
                catch
                {
                    return Convert.ToDateTime(SqlDateTime.MinValue.ToString());
                }
            }
            set { this[this.table.PackDATESIGNED] = value; }
        }

        public System.String PackPATACCEPT
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PackPATACCEPT];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PackPATACCEPT] = value; }
        }
        public System.String PackPRIVACYTEXT
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PackPRIVACYTEXT];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PackPRIVACYTEXT] = value; }
        }
        public System.String PackPRIVACYSIG
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PackPRIVACYSIG];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PackPRIVACYSIG] = value; }
        }
        public System.String PackSIGTYPE
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PackSIGTYPE];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PackSIGTYPE] = value; }
        }
        public System.Byte[] PackBinarySign
        {
            get
            {
                try
                {
                    return (System.Byte[])this[this.table.PackBinarySign];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.PackBinarySign] = value; }
        }
        public System.String PackEventType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PackEventType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PackEventType] = value; }
        }
        public System.String DeliveryStatus
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DeliveryStatus];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.DeliveryStatus] = value; }
        }

        public System.Boolean IsConsentSkip //PRIMEPOS-2866,PRIMEPOS-2871
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsConsentSkip];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsConsentSkip] = value; }
        }

        #endregion      

        public void Copy(RxTransactionDataRow oCopyTo)
        {
            oCopyTo.RxTransNo = this.RxTransNo;
            oCopyTo.PatientID = this.PatientID;
            oCopyTo.RxNo = this.RxNo;
            oCopyTo.Nrefill = this.Nrefill;
            oCopyTo.PickedUp = this.PickedUp;
            oCopyTo.PickUpPOS = this.PickUpPOS;
            oCopyTo.TransID = this.TransID;
            oCopyTo.IsDelivery = this.IsDelivery;
            oCopyTo.IsRxSync = this.IsRxSync;
            oCopyTo.TransAmount = this.TransAmount;
            oCopyTo.StationID = this.StationID;
            oCopyTo.UserID = this.UserID;
            oCopyTo.TransDate = this.TransDate;
            oCopyTo.ConsentTextID = this.ConsentTextID;
            oCopyTo.ConsentTypeID = this.ConsentTypeID;
            oCopyTo.ConsentStatusID = this.ConsentStatusID;
            oCopyTo.ConsentCaptureDate = this.ConsentCaptureDate;
            oCopyTo.ConsentEffectiveDate = this.ConsentEffectiveDate;
            oCopyTo.ConsentEndDate = this.ConsentEndDate;
            oCopyTo.PatConsentRelationID = this.PatConsentRelationID;
            oCopyTo.SigneeName = this.SigneeName;
            oCopyTo.SignatureData = this.SignatureData;
            oCopyTo.PickUpDate = this.PickUpDate;
            oCopyTo.CopayPaid = this.CopayPaid;
            oCopyTo.PackDATESIGNED = this.PackDATESIGNED;
            oCopyTo.PackPATACCEPT = this.PackPATACCEPT;
            oCopyTo.PackPRIVACYTEXT = this.PackPRIVACYTEXT;
            oCopyTo.PackPRIVACYSIG = this.PackPRIVACYSIG;
            oCopyTo.PackSIGTYPE = this.PackSIGTYPE;
            oCopyTo.PackBinarySign = this.PackBinarySign;
            oCopyTo.PackEventType = this.PackEventType;
            oCopyTo.DeliveryStatus = this.DeliveryStatus;
            oCopyTo.PartialFillNo = this.PartialFillNo;
        }
    }
}
