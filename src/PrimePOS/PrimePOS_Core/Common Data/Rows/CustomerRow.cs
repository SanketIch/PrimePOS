namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    //This class is used to define the shape of CustomerRow.
    public class CustomerRow : BaseRow
    {
        private CustomerTable table;

        // Constructor
        internal CustomerRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (CustomerTable)this.Table;
        }

        #region Public Properties
        // Public Property Customercode
        public System.Int32 CustomerId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.CustomerId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.CustomerId] = value;
            }
        }

        public System.Int32 AccountNumber
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.AccountNumber];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.AccountNumber] = value;
                NotifyPropertyChanged("AccountNumber");
            }
        }

        public System.Int32 PrimaryContact
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.PrimaryContact];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.PrimaryContact] = value;
                NotifyPropertyChanged("PrimaryContact");
            }
        }

        public System.Int32 HouseChargeAcctID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.HouseChargeAcctID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.HouseChargeAcctID] = value;
                NotifyPropertyChanged("HouseChargeAcctID");
            }
        }

        public System.Int32 Gender
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.Gender];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                if (value == 0 || value == 1)
                    this[this.table.Gender] = value;
                else
                    this[this.table.Gender] = 0;

                NotifyPropertyChanged("Gender");
            }
        }

        // Public Property Customername
        public System.String CustomerName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CustomerName];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.CustomerName] = value;
                NotifyPropertyChanged("CustomerName");
            }
        }

        // Public Property Customername
        public System.String CustomerFullName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CustomerName] + ", " + (System.String)this[this.table.FirstName];
                }
                catch
                {
                    return (System.String)this[this.table.CustomerName];
                }
            }
        }
        
        // Public Property Address1
        public System.String Address1
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Address1];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.Address1] = value;
                NotifyPropertyChanged("Address1");
            }
        }
        // Public Property Address2
        public System.String Address2
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Address2];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.Address2] = value;
                NotifyPropertyChanged("Address2");
            }
        }

        // Public Property City
        public System.String City
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.City];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.City] = value;
                NotifyPropertyChanged("City");
            }
        }

        // Public Property State
        public System.String State
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.State];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.State] = value;
                NotifyPropertyChanged("State");
            }
        }

        // Public Property Zip
        public System.String Zip
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Zip];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.Zip] = value;
                NotifyPropertyChanged("Zip");
            }
        }

        // Public Property Telephoneno
        public System.String PhoneOffice
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PhoneOffice];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.PhoneOffice] = value;
                NotifyPropertyChanged("PhoneOffice");
            }
        }

        // Public Property Faxno
        public System.String FaxNo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.FaxNo];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.FaxNo] = value;
                NotifyPropertyChanged("FaxNo");
            }
        }

        // Public Property Cellno
        public System.String CellNo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CellNo];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.CellNo] = value;
                NotifyPropertyChanged("CellNo");
            }
        }

        // Public Property URL
        public System.String PhoneHome
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PhoneHome];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.PhoneHome] = value;
                NotifyPropertyChanged("PhoneHome");
            }
        }

        // Public Property Email
        public System.String Email
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Email];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.Email] = value;
                NotifyPropertyChanged("Email");
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
            set
            {
                this[this.table.IsActive] = value;
                NotifyPropertyChanged("IsActive");
            }
        }

        public System.Boolean UseForCustomerLoyalty
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.UseForCustomerLoyalty];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                this[this.table.UseForCustomerLoyalty] = value;
                NotifyPropertyChanged("UseForCustomerLoyalty");
            }
        }

        public System.Object DateOfBirth
        {
            get
            {
                try
                {
                    return (System.Object)this[this.table.DateOfBirth];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                this[this.table.DateOfBirth] = value;
                NotifyPropertyChanged("DateOfBirth");
            }
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
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.Comments] = value;
                NotifyPropertyChanged("Comments");
            }
        }

        //Prog1 07Aug2009
        public System.String CustomerCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CustomerCode];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.CustomerCode] = value;
                NotifyPropertyChanged("CustomerCode");
            }
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
            set
            {
                this[this.table.FirstName] = value;
                NotifyPropertyChanged("FirstName");
            }
        }
        //End Prog1 07Aug2009

        public System.String DriveLicNo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DriveLicNo];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.DriveLicNo] = value;
                NotifyPropertyChanged("DriveLicNo");
            }
        }

        public System.String DriveLicState
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.DriveLicState];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set
            {
                this[this.table.DriveLicState] = value;
                NotifyPropertyChanged("DriveLicState");
            }
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
            set
            {
                this[this.table.PatientNo] = value;
                NotifyPropertyChanged("PatientNo");
            }
        }

        //Added By Shitaljit 0n 17 Feb 2012
        public System.Decimal Discount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.Discount];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.Discount] = value;
                NotifyPropertyChanged("Discount");
            }
        }

        //Added By Shitaljit 0n 17 Feb 2012
        public System.Int32 LanguageId
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.LanguageId];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.LanguageId] = value;
                NotifyPropertyChanged("LanguageId");
            }
        }

        //Sprint-23 - PRIMEPOS-2314 09-Jun-2016 JY Added
        public System.Boolean SaveCardProfile
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.SaveCardProfile];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                this[this.table.SaveCardProfile] = value;
                NotifyPropertyChanged("SaveCardProfile");
            }
        }
        #endregion // Properties

        public String GetAddress()
        {
            return Address1 + " " + Address2 + ", " + City + " " + State + " " + Zip;
        }
    }
}
