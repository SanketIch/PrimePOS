using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    public class TransactionManager
    {
        private List<Tuple<string, byte[],string>> _SecureDataList;
        private DataEncryptionKey _Params;

        private PayType oPayType;
        private TransactionType oTransType;

        public  DataEncryptionKey Params
        {
            //get { return _Params; }
            set { _Params = value; }
        }

        public List<Tuple<string, byte[],string>> SecureDataList
        {
            get { return _SecureDataList; }
            set { _SecureDataList = value; }
        }

        public void SendToGateway(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            foreach (var tmpSecureData in SecureDataList)
            {
                string tag = tmpSecureData.Item1;
                byte[] data = tmpSecureData.Item2;

                string decryptData = string.Empty;
                

                switch (tag.ToUpper())
                {

                    case "TPAYT":   //Payment type
                        decryptData = AppGlobal.Decryption(data, _Params).ToUpper();
                        SetPaymentType(decryptData);
                        decryptData = string.Empty;
                        break;
                    case "TTYPE":   //Transaction Type
                        decryptData = AppGlobal.Decryption(data, _Params).ToUpper();
                        SetTransType(decryptData);
                        decryptData = string.Empty;
                        break;
                    default:
                        break;


                }
            }

            ProcessTransaction(out oresult, out ex);
        }

        private void SetPaymentType(string oPay)
        {
            switch (oPay)
            {
                case "SALE":
                    oTransType = TransactionType.Sale;
                    break;
                case "CREDIT":
                    oTransType = TransactionType.Credit;
                    break;
                case "VOID":
                    oTransType = TransactionType.Void;
                    break;
                case "RETURN":
                    oTransType = TransactionType.Return;
                    break;
                case "BAL_ENQUIRY":
                    oTransType = TransactionType.BalanceEnquiry;
                    break;
                case "WITHDRAW":
                    oTransType = TransactionType.Withdraw;
                    break;
                case "ADD":
                    oTransType = TransactionType.Add;
                    break;
                case "UPDATE":
                    oTransType = TransactionType.Update;
                    break;
                case "RETRIEVE":
                    oTransType = TransactionType.Retrieve;
                    break;
                case "DELETE":
                    oTransType = TransactionType.Delete;
                    break;
                default:
                    break;

            }


        }

        private void SetTransType(string oTrans)
        {
            switch (oTrans)
            {
                case "CREDIT":
                    oPayType = PayType.Credit;
                    break;
                case "DEBIT":
                    oPayType = PayType.Debit;
                    break;
                case "EBT":
                    oPayType = PayType.EBT;
                    break;
                case "STORED_PROFILE":
                    oPayType = PayType.Stored_Profile;
                    break;
                case "EBT_CASH_BENIFIT":
                    oPayType = PayType.EBTCashBenifit;
                    break;
                case "EBT_FOOD_STAMP":
                    oPayType = PayType.EBTFoodStamp;
                    break;
                case "EBT_FOOD_STAMP_VOUCHER":
                    oPayType = PayType.EBTFoodStampVoucher;
                    break;
                default:
                    break;

            }


        }


        private void ProcessTransaction(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch(oPayType)
            {
                case PayType.Credit:
                    ProcessCC(out oresult, out ex);
                    break;
                case PayType.Debit:
                    ProcessDBC(out oresult, out ex);
                    break;
                case PayType.EBT:
                    ProcessEBT(out oresult, out ex);
                    break;
                case PayType.EBTCashBenifit:
                    ProcessEBTCashBenifit(out oresult, out ex);
                    break;
                case PayType.EBTFoodStamp:
                    ProcessEBTFoodStamp(out oresult, out ex);
                    break;
                case PayType.EBTFoodStampVoucher:
                    ProcessEBTFoodStampVoucher(out oresult, out ex);
                    break;
                case PayType.Stored_Profile:
                    ProcessStoredProfile(out oresult, out ex);
                    break;
                default:
                    break;

            }
        }


        private void ProcessStoredProfile(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();
            switch (oTransType)
            {
                case TransactionType.Sale:
                    using (ProcessStoredProfile oProfile = new ProcessStoredProfile())
                    {
                        oProfile.SecureDataList = SecureDataList;
                        oProfile.LoadSecureData(_Params);
                        oProfile.ProcessStoredProfileSale(out oresult, out ex);
                    }
                    break;
                case TransactionType.Add:
                    using (ProcessStoredProfile oProfile = new ProcessStoredProfile())
                    {
                        oProfile.SecureDataList = _SecureDataList;
                        oProfile.LoadSecureData(_Params);
                        oProfile.AddCreditCardProfile(out oresult, out ex);
                    }
                    break;
                case TransactionType.Credit:
                    using (ProcessStoredProfile oProfile = new ProcessStoredProfile())
                    {
                        oProfile.SecureDataList = _SecureDataList;
                        oProfile.LoadSecureData(_Params);
                        oProfile.ProcessStoredProfileCredit(out oresult, out ex);

                    }
                    break;
                case TransactionType.Update:
                    using (ProcessStoredProfile oProfile = new ProcessStoredProfile())
                    {
                        oProfile.SecureDataList = _SecureDataList;
                        oProfile.LoadSecureData(_Params);
                        oProfile.UpdateStoredProfile(out oresult, out ex);

                    }
                    break;
                case TransactionType.Retrieve:
                    using (ProcessStoredProfile oProfile = new ProcessStoredProfile())
                    {
                        oProfile.SecureDataList = _SecureDataList;
                        oProfile.LoadSecureData(_Params);
                        oProfile.RetrieveStoredProfile(out oresult, out ex);

                    }
                    break;
                case TransactionType.Delete:
                    using (ProcessStoredProfile oProfile = new ProcessStoredProfile())
                    {
                        oProfile.SecureDataList = _SecureDataList;
                        oProfile.LoadSecureData(_Params);
                        oProfile.DeleteStoredProfile(out oresult, out ex);

                    }
                    break;
                default:
                    break;


            }
        }

        private void ProcessCC(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();
            switch (oTransType)
            {
                case TransactionType.Sale:                   
                        using (ProcessCCard oCreditCard = new ProcessCCard())
                        {
                            oCreditCard.SecureDataList = SecureDataList;
                            oCreditCard.LoadSecureData(_Params);
                            oCreditCard.ProcessCreditCardSale(out oresult, out ex);
                        }
                        break;                   
                case TransactionType.Void:
                        using (ReverseCCTrans oCreditTrans = new ReverseCCTrans())
                        {
                            oCreditTrans.SecureDataList = _SecureDataList;
                            oCreditTrans.LoadSecureData(_Params);
                            oCreditTrans.VoidCCTransaction(out oresult, out ex);
                        }
                        break;                    
                case TransactionType.Credit:
                        using (ReverseCCTrans oCreditTrans = new ReverseCCTrans())
                        {
                            oCreditTrans.SecureDataList = _SecureDataList;
                            oCreditTrans.LoadSecureData(_Params);
                            oCreditTrans.CreditCCTransaction(out oresult, out ex);
                            
                        }
                        break;
                default:
                    break;


            }
        }

        private void ProcessDBC(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();
            switch (oTransType)
            {
                case TransactionType.Sale:
                    using (ProcessDebitCard oDebitCard = new ProcessDebitCard())
                    {
                        oDebitCard.SecureDataList = SecureDataList;
                        oDebitCard.LoadSecureData(_Params);
                        oDebitCard.ProcessDebitCardSale(out oresult, out ex);
                    }
                    break;
                case TransactionType.Void:
                    using(ReverseDebitTrans oDebitTrans =new ReverseDebitTrans())
                    {
                        oDebitTrans.SecureDataList = _SecureDataList;
                        oDebitTrans.LoadSecureData(_Params);
                        oDebitTrans.VoidDebitTrans(out oresult, out ex);
                    }
                    break;
                    
                case TransactionType.Credit:
                    using (ReverseDebitTrans oDebitTrans = new ReverseDebitTrans())
                    {
                        oDebitTrans.SecureDataList = _SecureDataList;
                        oDebitTrans.LoadSecureData(_Params);
                        oDebitTrans.ReturnDebitTrans(out oresult, out ex);
                    }
                    break;                    
                default:
                    break;


            }
        }

        private void ProcessEBT(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();
            switch (oTransType)
            {
                case TransactionType.BalanceEnquiry:
                    using(ProcessEBT oEBTTrans = new ProcessEBT())
                    {
                        oEBTTrans.SecureDataList = _SecureDataList;
                        oEBTTrans.LoadSecureData(_Params);
                        oEBTTrans.ProcessEBTBalInquiry(out oresult, out ex);
                    }
                    break;
                case TransactionType.Void:
                    using (ReverseEBTTrans oEBTTrans = new ReverseEBTTrans())
                    {
                        oEBTTrans.SecureDataList = _SecureDataList;
                        oEBTTrans.LoadSecureData(_Params);
                        oEBTTrans.VoidEBTTransaction(out oresult, out ex);
                    }
                    break;
                default:
                    break;


            }
        }

        private void ProcessEBTCashBenifit(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();
            switch (oTransType)
            {
                case TransactionType.BalanceEnquiry:
                    using (ProcessEBT oEBTTrans = new ProcessEBT())
                    {
                        oEBTTrans.SecureDataList = _SecureDataList;
                        oEBTTrans.LoadSecureData(_Params);
                        oEBTTrans.ProcessEBTCashBenefitBalInquiry(out oresult, out ex);
                    }
                    break;
                case TransactionType.Withdraw:
                    using(ProcessEBT oEBTTrans = new ProcessEBT())
                    {
                        oEBTTrans.SecureDataList = _SecureDataList;
                        oEBTTrans.LoadSecureData(_Params);
                        oEBTTrans.ProcessEBTCashBenefitWithDraw(out oresult, out ex);
                        
                    }
                    break;
                case TransactionType.Return:
                    using (ProcessEBT oEBTTrans = new ProcessEBT())
                    {
                        
                        oEBTTrans.SecureDataList = _SecureDataList;
                        oEBTTrans.LoadSecureData(_Params);
                        oEBTTrans.ProcessEBTFoodStampReturn(out oresult, out ex);
                        
                    }
                    break;
                case TransactionType.Sale:
                    using(ProcessEBT oEBTTrans = new ProcessEBT())
                    {
                        oEBTTrans.SecureDataList = _SecureDataList;
                        oEBTTrans.LoadSecureData(_Params);
                        oEBTTrans.ProcessEBTCashBenefitSale(out oresult, out ex);
                        
                    }
                    break;
                default:
                    break;


            }
        }

        private void ProcessEBTFoodStampVoucher(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();
            switch (oTransType)
            {
                case TransactionType.Sale:
                    using (ProcessEBT oEBTTrans = new ProcessEBT())
                    {
                        oEBTTrans.SecureDataList = _SecureDataList;
                        oEBTTrans.LoadSecureData(_Params);
                        oEBTTrans.ProcessEBTFoodStampVoucherSale(out oresult, out ex);
                        
                    }
                    break;
                default:
                    break;


            }
        }

        private void ProcessEBTFoodStamp(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();
            switch (oTransType)
            {
                case TransactionType.Sale:
                
                        using (ProcessEBT oEBTTrans = new ProcessEBT())
                        {
                            oEBTTrans.SecureDataList = _SecureDataList;
                            oEBTTrans.LoadSecureData(_Params);
                            oEBTTrans.ProcessEBTFoodStampSale(out oresult, out ex);
                        }
                        break;
                
                case TransactionType.Return:

                        using (ProcessEBT oEBTTrans = new ProcessEBT())
                        {
                            oEBTTrans.SecureDataList = _SecureDataList;
                            oEBTTrans.LoadSecureData(_Params);
                            oEBTTrans.ProcessEBTFoodStampReturn(out oresult, out ex);
                        }
                        break;
                    
                case TransactionType.BalanceEnquiry:
                        using (ProcessEBT oEBTTrans = new ProcessEBT())
                        {
                            oEBTTrans.SecureDataList = _SecureDataList;
                            oEBTTrans.LoadSecureData(_Params);
                            oEBTTrans.ProcessEBTFoodStampBalanceInquiry(out oresult, out ex);
                        }
                        break;
                   
                default:
                    break;


            }
        }

        






    }
}
