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
    class SaleViewModel : BaseViewModel, INotifyPropertyChanged
    {

        #region Constants

        private const string TRANS_REGULAR = "REGULAR";
        private const string TRANS_EBT = "EBT";
        private const string TRANS_FSA = "FSA";


        private const string EBT = "EBT";
        private const string EBT_CBS = "EBT_CashBenefitSale";
        private const string EBT_FOODSTAMPSALE = "EBT_FoodStampSale";
        private const string EBT_FOODSTAMP_VOUCHER_SALE = "EBT_FoodStampVoucherSale";
        private const string EBT_CB_WITHDRAW = "EBT_CashBenefitWithdrawal";
        private const string EBT_BALANCEINQUIRY = "EBT_BalanceInquiry";
        private const string EBT_CB_BALANCEINQUIRY = "EBT_CashBenefitBalanceInquiry";
        private const string EBT_FOODSTAMP_BALANCEINQUIRY = "EBT_FoodStampBalanceInquiry";



        #endregion

        Transaction _objTransaction = new Transaction();
        string _Result;
        string _TransType;
        string _EBTSubType;
        string _ApprovalCode;
        string _VoucherSerialNo;

        RelayCommand _ProcessButtonCommand;
        RelayCommand _CloseCommand;

        private bool _UseToken;

        


        #region Data Properties
        public string ApprovalCode
        {
            get { return _ApprovalCode; }
            set
            {
                _ApprovalCode = value;
                OnPropertyChanged("ApprovalCode");
            }
        }
        public string VoucherSerialNo
        {
            get { return _VoucherSerialNo; }
            set
            {
                _VoucherSerialNo = value;
                OnPropertyChanged("ApprovalCode");
            }
        }


        public string EBTSubType
        {
            get { return _EBTSubType; }
            set
            {
                _EBTSubType = value;
                OnPropertyChanged("EBTSubType");
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
        public bool Use_Token
        {
            get { return _UseToken; }
            set
            {
                _UseToken = value;
                OnPropertyChanged("UseToken");
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

        public bool Tokenize
        {
            get
            {
                return _objTransaction.Tokenize;
            }

            set
            {
                _objTransaction.Tokenize = value;
               // OnPropertyChanged("Tokenize");
            }
        }
        public string Token
        {
            get
            {
                return _objTransaction.Token;
            }

            set
            {
                _objTransaction.Token = value;
                //OnPropertyChanged("Token");
            }
        }

        public string Last4Digits
        {
            get
            {
                return _objTransaction.Last4Digits;
            }

            set
            {
                _objTransaction.Last4Digits = value;
                //OnPropertyChanged("Last4Digits");
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

        #region Observable Collection

        private ObservableCollection<string> _TransTypeList;
        private ObservableCollection<string> _EBTSubTypeList;

        public ObservableCollection<string> TransTypeList
        {
            get { return _TransTypeList; }
            set
            {
                _TransTypeList = value;
                OnPropertyChanged("TransTypeList");
            }
        }

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

        #region Constructor

        public SaleViewModel():base()
        {
            TransTypeList = new ObservableCollection<string>();
            EBTSubTypeList = new ObservableCollection<string>();
            _objTransaction.TransType = TransactionType.Sale;
            TransType = TRANS_REGULAR;
            LoadCollection();
            LoadSubTypes();
            Amount = "0.00";
            ResetFSAAmount();
            Tokenize = false;
            Use_Token = false;
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

        #region Private Functions
        private void ResetFSAAmount()
        {
            TotalFSAAmount = "0.00";
            RxAmount = "0.00";
            ClinicAmount = "0.00";
            DentalAmount = "0.00";
            VisionAmount = "0.00";
        }
        private void LoadCollection()
        {
            TransTypeList.Add(TRANS_REGULAR);
            TransTypeList.Add(TRANS_EBT);
            TransTypeList.Add(TRANS_FSA);
        }
        private void LoadSubTypes()
        {
            EBTSubTypeList.Add(EBT);
            EBTSubTypeList.Add(EBT_CBS);
            EBTSubTypeList.Add(EBT_FOODSTAMPSALE);
            EBTSubTypeList.Add(EBT_FOODSTAMP_VOUCHER_SALE);
            EBTSubTypeList.Add(EBT_CB_WITHDRAW);
            EBTSubTypeList.Add(EBT_BALANCEINQUIRY);
            EBTSubTypeList.Add(EBT_CB_BALANCEINQUIRY);
            EBTSubTypeList.Add(EBT_FOODSTAMP_BALANCEINQUIRY);
        }

        private void ProcessTransaction()
        {
            ProcessCard ocard = new ProcessCard();

            switch (TransType)
            {
                case TRANS_REGULAR:
                    ocard.TransSubType = TransactionSubType.None;
                    break;
                case TRANS_EBT:
                    ocard.IsEBT = true;
                    switch (EBTSubType)
                    {
                        case EBT:
                            ocard.TransSubType = TransactionSubType.EBT;
                            break;
                        case EBT_CBS:
                            ocard.TransSubType = TransactionSubType.EBT_CashBenifitSale;
                            break;
                        case EBT_FOODSTAMPSALE:
                            ocard.TransSubType = TransactionSubType.EBT_FoodStampSale;
                            break;
                        case EBT_FOODSTAMP_VOUCHER_SALE:
                            ocard.TransSubType = TransactionSubType.EBT_FoodStampVoucherSale;
                            break;
                        case EBT_CB_WITHDRAW:
                            ocard.TransSubType = TransactionSubType.EBT_CashBenifitWithdrawal;
                            break;
                        case EBT_BALANCEINQUIRY:
                            ocard.TransSubType = TransactionSubType.EBT_BalanceInquiry;
                            break;
                        case EBT_CB_BALANCEINQUIRY:
                            ocard.TransSubType = TransactionSubType.EBT_CashBenifitBalanceInquiry;
                            break;
                        case EBT_FOODSTAMP_BALANCEINQUIRY:
                            ocard.TransSubType = TransactionSubType.EBT_BalanceInquiry;
                            break;
                    }
                    if(!string.IsNullOrWhiteSpace(ApprovalCode))
                    {
                        ocard.EBTApprovalCode = ApprovalCode;
                    }
                    if (!string.IsNullOrWhiteSpace(VoucherSerialNo))
                    {
                        ocard.VoucherSerialNumber = VoucherSerialNo;
                    }
                    
                    break;
                case TRANS_FSA:
                    ocard.IsFSA = true;
                    ocard.TransSubType = TransactionSubType.FSA;
                    ocard.RXAmount = decimal.Parse(RxAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    ocard.DentalAmount = decimal.Parse(DentalAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    ocard.VisionAmount = decimal.Parse(VisionAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    ocard.ClinicalAmount = decimal.Parse(ClinicAmount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                    break;
                default:
                    ocard.TransSubType = TransactionSubType.None;
                    break;
            }

            ocard.Amount = decimal.Parse(Amount, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat); 
            ocard.TransType = _objTransaction.TransType;

            ocard.RequireTax = false;

            ocard.AllowTokenization = true;
            ocard.ReturnImgEncoding = EncodingFormat.BinHex;
            ocard.ReturnImgType = ImageFormat.Png;
            /* ocard.IsEBT = true;
             ocard.TransSubType = TransactionSubType.EBT_CashBenifitSale;*/
            //ocard.IsFSA = true;
           

            if (Tokenize)
            {
                ocard.Tokenize = true;
            }

            if (Use_Token)
            {
                ocard.Token = Token;
                ocard.Last4Digits = Last4Digits;
            }

            Result = ocard.DoTransaction();
            TransactionResult oresult = new TransactionResult();
            oresult.LoadResponse(Result);

            //FirstMileTest.Views.frmSignature ofrmsig = new Views.frmSignature();
            //ViewModel.SignatureViewModel osigvm = new SignatureViewModel(oresult.SignatureString);

           // ofrmsig.DataContext = osigvm;
           // ofrmsig.ShowDialog();

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
