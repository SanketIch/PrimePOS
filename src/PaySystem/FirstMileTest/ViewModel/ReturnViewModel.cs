using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FirstMileTest.Model;
using FirstMile;
using System.Collections.ObjectModel;

namespace FirstMileTest.ViewModel
{
    class ReturnViewModel: BaseViewModel, INotifyPropertyChanged
    {
        #region Constants

        private const string TRANS_VOID = "VOID TRANSACTION";
        private const string TRANS_REFUND = "REFUND TRANSACTION";
        private const string TRANS_CREDIT = "CREDIT TRANSACTION";



        private const string TRANS_REGULAR = "REGULAR";
        private const string TRANS_EBT = "EBT";
        private const string TRANS_FSA = "FSA";


        private const string EBT_CB_RETURN = "EBT_CashBenefitReturn";
        private const string EBT_FOODSTAMP_RETURN = "EBT_FoodStampReturn";

        #endregion


        Transaction _objTransaction = new Transaction();
        string _Result;
        string _TransType;
        string _TransSubType;
        string _EBTSubType;
        

        RelayCommand _ProcessButtonCommand;
        RelayCommand _CloseCommand;

        #region Constructor

        public ReturnViewModel():base()
        {
            TransTypeList = new ObservableCollection<string>();
            TransSubTypeList = new ObservableCollection<string>();
            EBTSubTypeList = new ObservableCollection<string>();
            LoadCollection();
            LoadTransSubTypeList();
            LoadEBTList();


            TransType = TRANS_VOID;
            TransSubType = TRANS_REGULAR;
            EBTSubType = string.Empty;


            ResetFSAAmount();
        }
           



        #endregion

        #region Observable Collection

        private ObservableCollection<string> _TransTypeList;

        public ObservableCollection<string> TransTypeList
        {
            get { return _TransTypeList; }
            set
            {
                _TransTypeList = value;
                OnPropertyChanged("TransTypeList");
            }
        }

        private ObservableCollection<string> _TransSubTypeList;

        public ObservableCollection<string> TransSubTypeList
        {
            get { return _TransSubTypeList; }
            set
            {
                _TransSubTypeList = value;
                OnPropertyChanged("TransSubTypeList");
            }
        }


        private ObservableCollection<string> _EBTSubTypeList;

        public ObservableCollection<string> EBTSubTypeList
        {
            get { return _EBTSubTypeList; }
            set
            {
                _EBTSubTypeList = value;
                OnPropertyChanged("EBTSubTypeList");
            }
        }

        #endregion

        #region Data Properties
        public string EBTSubType
        {
            get { return _EBTSubType; }
            set
            {
                _EBTSubType = value;
                OnPropertyChanged("EBTSubType");
            }
        }
        public string TransSubType
        {
            get { return _TransSubType; }
            set
            {
                _TransSubType = value;
                OnPropertyChanged("TransSubType");
            }
        }

        public string TransType
        {
            get { return _TransType; }
            set
            {
                _TransType = value;
                OnPropertyChanged("TransType");
            }
        }

        public string Result
        {
            get
            {
                return _Result;
            }

            set
            {
                _Result = value;
                OnPropertyChanged("Result");
            }
        }

        public string Amount
        {
            get
            {
                return _objTransaction.Amount;
            }

            set
            {
                _objTransaction.Amount = value;
                //OnPropertyChanged("Amount");
            }
        }

        public string OrderID
        {
            get
            {
                return _objTransaction.OrderID;
            }

            set
            {
                _objTransaction.OrderID = value;
               // OnPropertyChanged("OrderID");
            }
        }

        public string TransactionID
        {
            get
            {
                return _objTransaction.TransactionID;
            }

            set
            {
                _objTransaction.TransactionID = value;
                //OnPropertyChanged("TransactionID");
            }
        }


        public string TotalFSAAmount
        {
            get
            {
                return _objTransaction.TotalFSAAmount;
            }

            set
            {
                _objTransaction.TotalFSAAmount = value;
                //OnPropertyChanged("TotalFSAAmount");
            }
        }

        public string RxAmount
        {
            get
            {
                return _objTransaction.RxAmount;
            }

            set
            {
                _objTransaction.RxAmount = value;
                //OnPropertyChanged("RxAmount");
            }
        }

        public string DentalAmount
        {
            get
            {
                return _objTransaction.DentalAmount;
            }

            set
            {
                _objTransaction.DentalAmount = value;
                //OnPropertyChanged("DentalAmount");
            }
        }

        public string VisionAmount
        {
            get
            {
                return _objTransaction.VisionAmount;
            }

            set
            {
                _objTransaction.VisionAmount = value;
                //OnPropertyChanged("VisionAmount");
            }
        }

        public string ClinicAmount
        {
            get
            {
                return _objTransaction.ClinicAmount;
            }

            set
            {
                _objTransaction.ClinicAmount = value;
                //OnPropertyChanged("ClinicAmount");
            }
        }

        #endregion


        #region Command  Properties

        public RelayCommand ButtonCommand
        {

            get
            {
                if (_ProcessButtonCommand == null)
                {
                    _ProcessButtonCommand = new RelayCommand(param => this.RunCommand());
                }
                return _ProcessButtonCommand;
            }
        }

        public RelayCommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                    _CloseCommand = new RelayCommand(param => this.RequestClose());

                return _CloseCommand;
            }
        }



        #endregion


        #region Public Functions
        public void RunCommand()
        {
            ProcessTransaction();
        }

        void RequestClose()
        {
            /*EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);*/
            this.OnClosingRequest();


        }

        #endregion



        #region Private Methods

        private void LoadCollection()
        {
            TransTypeList.Add(TRANS_VOID);
            TransTypeList.Add(TRANS_REFUND);
            TransTypeList.Add(TRANS_CREDIT);
        }

        void LoadTransSubTypeList()
        {
            TransSubTypeList.Add(TRANS_REGULAR);
            TransSubTypeList.Add(TRANS_EBT);
            TransSubTypeList.Add(TRANS_FSA);
        }
        void LoadEBTList()
        {
            EBTSubTypeList.Add(EBT_CB_RETURN);
            EBTSubTypeList.Add(EBT_FOODSTAMP_RETURN);
        }

        private void ResetFSAAmount()
        {
            TotalFSAAmount = "0.00";
            RxAmount = "0.00";
            ClinicAmount = "0.00";
            DentalAmount = "0.00";
            VisionAmount = "0.00";
        }

        private void ProcessTransaction()
        {
            ProcessCard ocard = new ProcessCard();
            ocard.Amount = decimal.Parse(Amount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);

           

            switch (TransType)
            {
                case TRANS_REFUND:
                    ocard.TransType = TransactionType.Refund;
                    break;
                case TRANS_VOID:
                    ocard.TransType = TransactionType.Void;
                    break;
                case TRANS_CREDIT:
                    ocard.TransType = TransactionType.Credit;
                    break;
                default:
                    break;
            }
            if(TransType != TRANS_CREDIT)
            {
                ocard.TransactionID = TransactionID;
                ocard.OrderID = OrderID;
            } 
            switch(TransSubType)
            {
                case TRANS_FSA:
                    ocard.IsFSA = true;
                    ocard.TransSubType = TransactionSubType.FSA;
                    ocard.RXAmount = decimal.Parse(RxAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    ocard.DentalAmount = decimal.Parse(DentalAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    ocard.VisionAmount = decimal.Parse(VisionAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    ocard.ClinicalAmount = decimal.Parse(ClinicAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    
                    break;
                case TRANS_EBT:
                    ocard.IsEBT = true;
                    switch (EBTSubType)
                    {
                        case EBT_CB_RETURN:
                            ocard.TransSubType = TransactionSubType.EBT_CashBenifitReturn;
                            break;
                        case EBT_FOODSTAMP_RETURN:
                            ocard.TransSubType = TransactionSubType.EBT_FoodStampReturn;
                            break;
                        default:
                            break;

                    }
                    break;
            }     


            Result = ocard.DoTransaction();
        }

        #endregion



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


    }
}
