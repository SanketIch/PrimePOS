//----------------------------------------------------------------------------------------------------
//PRIMEPOS-2339 03-Oct-2016 JY Added to maintain item InsSigTrans
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    //using POS.Resources;

    public class InsSigTransRow : DataRow
    {
        private InsSigTransTable table;

        internal InsSigTransRow(DataRowBuilder rb) : base(rb) 
		{
            this.table = (InsSigTransTable)this.Table;
        }

        #region Public Properties
        public System.Int32 ID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ID] = value; }
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

        public System.Int32 PatientNo
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PatientNo];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PatientNo] = value; }
        }

        public System.String InsType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.InsType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.InsType] = value; }
        }

        public System.String TransData
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransData];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.TransData] = value; }
        }

        public System.String TransSigData
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransSigData];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.TransSigData] = value; }
        }

        public System.String CounselingReq
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CounselingReq];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.CounselingReq] = value; }
        }

        public System.String SigType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SigType];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.SigType] = value; }
        }

        public System.Byte[] BinarySign
        {
            get
            {
                try
                {
                    return (System.Byte[])this[this.table.BinarySign];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.BinarySign] = value; }
        }

        //
        public System.String PrivacyPatAccept
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PrivacyPatAccept];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.PrivacyPatAccept] = value; }
        }

        public System.String PrivacyText
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PrivacyText];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.PrivacyText] = value; }
        }

        public System.String PrivacySig
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PrivacySig];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.PrivacySig] = value; }
        }

        public System.String PrivacySigType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PrivacySigType];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.PrivacySigType] = value; }
        }

        public System.Byte[] PrivacyBinarySign
        {
            get
            {
                try
                {
                    return (System.Byte[])this[this.table.PrivacyBinarySign];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.PrivacyBinarySign] = value; }
        }
        #endregion
    }
}
