
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TransDetailRXRow : DataRow 
	{
		private TransDetailRXTable table;

        internal TransDetailRXRow(DataRowBuilder rb)
            : base(rb) 
		{
			this.table = (TransDetailRXTable)this.Table;
		}

		#region Public Properties

		public System.Int32 TransDetailID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.TransDetailID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.TransDetailID] = value; }
		}

        public System.Int64 RXNo
        {
            get
            {
                try
                {
                    if (this[this.table.RXNo].ToString().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return (System.Int64)this[this.table.RXNo];
                    }
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.RXNo] = value; }
        }

		public System.DateTime DateFilled
		{
			get 
			{ 
				try 
				{ 
					return (System.DateTime)this[this.table.DateFilled];
				}
				catch
				{ 
					return System.DateTime.MinValue; 
				}
			} 
			set { this[this.table.DateFilled] = value; }
		}

		public System.Int32 NRefill
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.NRefill];
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.NRefill] = value; }
		}

		public System.String DrugNDC
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.DrugNDC];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.DrugNDC] = value; }
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

        public System.String PatType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PatType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.PatType] = value; }
        }

		public System.Int32 RXDetailID
		{
			get
			{
				try
				{
					return (System.Int32)this[this.table.RXDetailID];
				}
				catch
				{
					return 0;
				}
			}
				set { this[this.table.RXDetailID]=value; }
		}

        public System.Int64 PatientNo
        {
            get
            {
                try
                {
                    return (System.Int64)this[this.table.PatientNo];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.PatientNo] = value; }
        }
        //Added by atul 07-jan-2011
        public System.String CounsellingReq
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CounsellingReq];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CounsellingReq] = value; }
        }

        public System.String Ezcap
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Ezcap];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.Ezcap] = value; }
        }        

        /// <summary>
        /// Added by Manoj 1/2/2015
        /// Fix the return bug - not return ID stored
        /// </summary>
        public System.Int32 ReturnTransDetailID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ReturnTransDetailID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ReturnTransDetailID] = value; }
        }
        //End of Added by atul 07-jan-2011

        //PRIMEPOS-3008 30-Sep-2021 JY Added
        public System.String Delivery
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Delivery];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.Delivery] = value; }
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
        #endregion
    }
}
