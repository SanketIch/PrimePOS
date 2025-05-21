using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FirstMile;

namespace FirstMileTest.Model
{
    class Transaction:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


        private string _Amount;
        private TransactionType _TransType;
        private bool _Tokenize;
        private string _MerchantOrderNumber;
        private string _RefCode;
        private string _OrderID;
        private string _TransactionID;
        private string _Token;
        private string _Last4Digits;

        private string _TotalFSAAmount;
        private string _RxAmount;
        private string _DentalAmount;
        private string _VisionAmount;
        private string _ClinicAmount;

        public string Amount
        {
            get
            {
                return _Amount;                
            }

            set
            {
                _Amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public TransactionType TransType
        {
            get
            {
                return _TransType;
            }

            set
            {
                _TransType = value;
                OnPropertyChanged("TransType");
            }
        }

        public bool Tokenize
        {
            get
            {
                return _Tokenize;
            }

            set
            {
                _Tokenize = value;
                OnPropertyChanged("Tokenize");
            }
        }

        public string MerchantOrderNumber
        {
            get
            {
                return _MerchantOrderNumber;
            }

            set
            {
                _MerchantOrderNumber = value;
                OnPropertyChanged("MerchantOrderNumber");
            }
        }

        public string RefCode
        {
            get
            {
                return _RefCode;
            }

            set
            {
                _RefCode = value;
                OnPropertyChanged("RefCode");
            }
        }

        public string OrderID
        {
            get
            {
                return _OrderID;
            }

            set
            {
                _OrderID = value;
                OnPropertyChanged("OrderID");
            }
        }

        public string TransactionID
        {
            get
            {
                return _TransactionID;
            }

            set
            {
                _TransactionID = value;
                OnPropertyChanged("TransactionID");
            }
        }

        public string Token
        {
            get
            {
                return _Token;
            }

            set
            {
                _Token = value;
                OnPropertyChanged("Token");
            }
        }

        public string Last4Digits
        {
            get
            {
                return _Last4Digits;
            }

            set
            {
                _Last4Digits = value;
                OnPropertyChanged("Last4Digits");
            }
        }

        public string TotalFSAAmount
        {
            get
            {
                return _TotalFSAAmount;
            }

            set
            {
                _TotalFSAAmount = value;
                OnPropertyChanged("TotalFSAAmount");
            }
        }

        public string RxAmount
        {
            get
            {
                return _RxAmount;
            }

            set
            {
                _RxAmount = value;
                OnPropertyChanged("RxAmount");
            }
        }

        public string DentalAmount
        {
            get
            {
                return _DentalAmount;
            }

            set
            {
                _DentalAmount = value;
                OnPropertyChanged("DentalAmount");
            }
        }

        public string VisionAmount
        {
            get
            {
                return _VisionAmount;
            }

            set
            {
                _VisionAmount = value;
                OnPropertyChanged("VisionAmount");
            }
        }

        public string ClinicAmount
        {
            get
            {
                return _ClinicAmount;
            }

            set
            {
                _ClinicAmount = value;
                OnPropertyChanged("ClinicAmount");
            }
        }
    }
}
