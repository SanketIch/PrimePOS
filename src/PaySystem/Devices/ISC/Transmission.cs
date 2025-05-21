using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Gateway;


namespace EDevice
{
    internal class Transmission
    {
        #region Send to Gateway      
        /// <summary>
        /// Prepare transaction for transmission to the gateway
        /// </summary>
        /// <param name="SecureData"></param>
        /// <param name="Params"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public object SendToGateway(List<Tuple<string, byte[], string>> SecureData, DataEncryptionKey Params, PaymentTags trans)
        {
            object ostatus;
            try
            {
                Gateway.DataEncryptionKey oDEK = new Gateway.DataEncryptionKey();
                oDEK.TransEKey.IV = Params.TransEKey.IV;
                oDEK.TransEKey.TransKey = Params.TransEKey.TransKey;
                oDEK.TransEKey.RSAParams = Params.TransEKey.RSAParams;
                
                GatewayManager.SelectedGateway = Gateway.Gateway.WorldPay;

                TransactionResult tr = new TransactionResult();
                Exception ex1 = new Exception();
                switch (trans.TransactionType)
                {
                    case PaymentTags.transactionType.Sale:
                        {
                            switch (trans.PaymentType)
                            {
                                case PaymentTags.payType.Debit:
                                    {
                                        ProcessDebitCard db = new ProcessDebitCard();
                                        db.SecureDataList = SecureData;
                                        db.LoadSecureData(oDEK);
                                        db.ProcessDebitCardSale(out tr, out ex1);
                                    }
                                    break;
                                case PaymentTags.payType.Credit:
                                    {
                                        if (trans.StoreProfile != null && trans.StoreProfile.IsStoredProfile)
                                        {
                                            ProcessStoredProfile sp = new ProcessStoredProfile();
                                            sp.SecureDataList = SecureData;
                                            sp.LoadSecureData(oDEK);
                                            sp.AddCreditCardProfile(out tr, out ex1);
                                        }
                                        else
                                        {
                                            ProcessCCard cc = new ProcessCCard();
                                            cc.SecureDataList = SecureData;
                                            cc.LoadSecureData(oDEK);
                                            cc.ProcessCreditCardSale(out tr, out ex1);
                                        }
                                    }
                                    break;
                            }
                            break;
                        }
                    case PaymentTags.transactionType.Void:
                        {
                            ReverseCCTrans vT = new ReverseCCTrans();
                            vT.SecureDataList = SecureData;
                            vT.LoadSecureData(oDEK);
                            vT.VoidCCTransaction(out tr, out ex1);
                            break;
                        }
                    case PaymentTags.transactionType.Credit:
                    case PaymentTags.transactionType.Return:
                        {
                            ReverseCCTrans rcc = new ReverseCCTrans();
                            rcc.SecureDataList = SecureData;
                            rcc.LoadSecureData(oDEK);
                            rcc.CreditCCTransaction(out tr, out ex1);
                            break;
                        }
                }
                ostatus = tr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return ostatus;
        }
        #endregion Send to Gateway
    }      
}
