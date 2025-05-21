using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    /// <summary>
    /// Handles the properties and Methords that process a credit or void of a prevous EBT Transaction
    /// NOTE: A previous Transaction is required
    /// </summary>
    public class ReverseEBTTrans:IDisposable
    {

        #region "Private Fields"

        private float _Amount;
        private string _OrderId;
        private string _HistoryId;

        private List<Tuple<string, byte[],string>> _SecureDataList;

        private string _MerchantPin;


        #endregion


        public ReverseEBTTrans()
        {
        }

        #region "Properties"




        /// <summary>
        /// Transaction Dollar amount in the format 0.00
        /// </summary>
        public float Amount
        {
            set
            {
                this._Amount = value;
            }
        }

        /// <summary>
        /// OrderID of the Original Transaction 
        /// </summary>
        public string OrderId
        {
            set { this._OrderId = value; }
        }

        /// <summary>
        /// History ID of the Original Transaction
        /// </summary>
        public string HistoryId
        {
            set { this._HistoryId = value; }
        }


        /// <summary>
        /// LIst That Contains the Encrypted Data to be passed on to the Gateway
        /// </summary>
        public List<Tuple<string, byte[],string>> SecureDataList
        {
            get { return _SecureDataList; }
            set { _SecureDataList = value; }
        }


        /// <summary>
        /// Optional on all Methods
        /// Merchant Unique pin . Leave it bank if you are unsure if you have one
        /// </summary>
        public string MerchantPin
        {
            set { this._MerchantPin = value; }
        }



        #endregion


        public void LoadSecureData(DataEncryptionKey Params)
        {
            foreach (var tmpSecureData in SecureDataList)
            {
                string tag = tmpSecureData.Item1;
                byte[] data = tmpSecureData.Item2;

                string decryptData = string.Empty;
                //decryptData = AppGlobal.Decryption(data, Params).ToUpper();

                switch (tag.ToUpper())
                {

                    case "TAMT":
                        this.Amount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                        break;
                    case "MID":
                        this.MerchantPin = AppGlobal.Decryption(data, Params);
                        break;
                    case "HISTID":
                        this._HistoryId = AppGlobal.Decryption(data, Params);
                        break;
                    case "ORDID":
                        this._OrderId = AppGlobal.Decryption(data, Params);
                        break;
                    default:
                        break;


                }
            }
        }


        /// <summary>
        /// Void a Previous EBT Transaction
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void VoidEBTTransaction(out TransactionResult oresult, out Exception ex)
        {
            //resultString = string.Empty;
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_VoidEBTTransaction(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_VoidEBTTransaction(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }


        }

        #region "Private Void EBT Transaction"

        private void PP_VoidEBTTransaction(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.VoidCreditPost oVoidCC = new PrismPay.VoidCreditPost();
                oVoidCC.acctid = GatewayManager.AccountId;
                oVoidCC.amount = _Amount;
                oVoidCC.historyid = _HistoryId;
                oVoidCC.orderid = _OrderId;

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oVoidCC.merchantpin = _MerchantPin;
                    }
                }
                else
                    oVoidCC.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oVoidCC.subid = GatewayManager.SubId;
                }

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTVoid(oVoidCC);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }

        }


        private void WP_VoidEBTTransaction(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.VoidCreditPost oVoidCC = new WorldPay.VoidCreditPost();
                oVoidCC.acctid = GatewayManager.AccountId;
                oVoidCC.amount = _Amount;
                oVoidCC.historyid = _HistoryId;
                oVoidCC.orderid = _OrderId;

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oVoidCC.merchantpin = _MerchantPin;
                    }
                }
                else
                    oVoidCC.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oVoidCC.subid = GatewayManager.SubId;
                }

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTVoid(oVoidCC);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion

        #region "iDisposible Members"

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Amount = 0.0f;
                this.HistoryId = "";
                this.OrderId = "";
                //oVoidCC = null;
                //OGatewayClient = null;


            }
            GC.Collect();

        }
        #endregion
    }
}
