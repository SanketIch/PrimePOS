using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrismPay;
//using Gateway.PrismPay;

namespace Gateway
{
    /// <summary>
    /// Handles Properties and Methords required for processing Debit Card
    /// </summary>
    public class ProcessDebitCard : IDisposable
    {
        
        public ProcessDebitCard()
        {
            IsEMV = false;
            IsFSA = false;
            
        }

        #region "Private Fields"

        private float _Amount;
        private string _SwipeData;
        private string _EncryptedPinData;
        private int _DbEncryptedDeviceType;
        private string _DbEncryptedSwipeData;

        private float _CashBackAmount;

        private string _MerchantPin;

        private List<Tuple<string, byte[],string>> _SecureDataList;
        private EMV _oEMV = new EMV();
        private bool _IsNFC = false;

        #endregion

        #region Properties

        public bool IsNFC { set; get; }
        
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
        /// Card Swipe Data (Must Include Either Track1 or Track 2)
        /// </summary>
        public string SwipeData
        {
            set { this._SwipeData = value; }
        }

        /// <summary>
        /// 32 Byte Encrypted Pin Pad Data(Includes 16 Bytes of PIN data followed by 6 Byte Key set and 10 Byte PIN Pad Serial Number)
        /// </summary>
        public string EncryptedPinData
        {
            set { this._EncryptedPinData = value; }
        }

        /// <summary>
        /// Determines the Encrypted Device type The supported Devices are
        /// 1. Magtech (for most Magtech devices) 2. Magtech Ipad 3. IDTECH 4. Ingenico (iSC250/350  iPP250/350)
        /// </summary>
        public DeviceType DbCEncryptedDeviceType
        {
            set
            {
                switch (value)
                {
                    case DeviceType.Magtech:
                        this._DbEncryptedDeviceType = 1;
                        break;
                    case DeviceType.Magtech_IPad:
                        this._DbEncryptedDeviceType = 2;
                        break;
                    case DeviceType.IDTECH:
                        this._DbEncryptedDeviceType = 3;
                        break;
                    case DeviceType.Ingenico:
                        this._DbEncryptedDeviceType = 4;
                        break;
                    default:
                        this._DbEncryptedDeviceType = 0;
                        break;
                }
            }
        }



        /// <summary>
        /// Encrypted Credit Card Swipe Data (Must Include Track 1 and Track 2)
        /// Format of the data Depends on the device
        /// </summary>
        public string DbCEncryptedSwipeData
        {
            set { this._DbEncryptedSwipeData = value; }
        }

        /// <summary>
        /// Cash back Amount in format 0.00
        /// </summary>
        public float CashBackAmount
        {
            set { this._CashBackAmount = value; }
        }


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

        public bool IsEMV { set; get; }

        public bool IsFSA { set; get; }

        public EMV oEMV
        {
            set { this._oEMV = value; }
            get { return _oEMV; }
        }


        #endregion


        public void LoadSecureData(DataEncryptionKey Params)
        {
            string pindata = string.Empty;
            string keyset = string.Empty;
            string pinpadserial = string.Empty;

            foreach (var tmpSecureData in SecureDataList)
            {
                string tag = tmpSecureData.Item1;
                byte[] data = tmpSecureData.Item2;
                string oType = tmpSecureData.Item3;
                string decryptData = string.Empty;
                if (!this.IsEMV)
                {
                    if (oType.ToUpper() == "EMV")
                    {
                        this.IsEMV = true;
                    }
                }

                if (!this.IsNFC)
                {
                    if (oType.ToUpper() == "NFC")
                    {
                        this.IsNFC = true;
                    }
                }
                //decryptData = AppGlobal.Decryption(data, Params).ToUpper();
               
                switch (tag.ToUpper())
                {
                    case "EMSI":
                        this.DbCEncryptedSwipeData = AppGlobal.Decryption(data, Params);
                        break;
                    case "TAMT":
                        this.Amount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                        break;
                    case "CBAMT":
                        this.CashBackAmount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                        break;
                    case "MID":
                        this.MerchantPin = AppGlobal.Decryption(data, Params);
                        break;
                    case "DNAME":

                        //this.MerchantPin = AppGlobal.Decryption(data, Params);
                        decryptData = AppGlobal.Decryption(data, Params);
                        switch (decryptData)
                        {
                            case "Magtech":
                                this.DbCEncryptedDeviceType = DeviceType.Magtech;
                                break;
                            case "Magtech_Ipad":
                                this.DbCEncryptedDeviceType = DeviceType.Magtech_IPad;
                                break;
                            case "IDTECH":
                                this.DbCEncryptedDeviceType = DeviceType.IDTECH;
                                break;
                            case "Ingenico":
                                this.DbCEncryptedDeviceType = DeviceType.Ingenico;
                                break;
                            default:
                                this.DbCEncryptedDeviceType = DeviceType.None;
                                break;

                        }
                        break;
                    case "PINB":
                        pindata = AppGlobal.Decryption(data, Params);
                        break;
                    case "PKSI":
                        keyset = AppGlobal.Decryption(data, Params);
                        break;
                    case "TSN":
                        pinpadserial = AppGlobal.Decryption(data, Params);
                        break;
                    default:
                        break;


                }
            }

            this.EncryptedPinData = keyset + pinpadserial + pindata;
            //this.EncryptedPinData = pindata + keyset + pinpadserial;
            if (this.IsEMV)
            {
                this.oEMV.SecureDataList = SecureDataList;
                this.oEMV.LoadSecureData(Params);
            }
        }

        /// <summary>
        /// Processes the Debit card sale Transaction and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class</param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void ProcessDebitCardSale(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessDebitCardSale(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessDebitCardSale(out oresult, out ex);
                    break; 
                default:
                    ex = new Exception("Gateway not selected");
                    break;

            }

        }

        /// <summary>
        /// Processes the Debit card Authorization and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class</param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void ProcessDebitCardAuth(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessDebitCardAuth(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessDebitCardAuth(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;

            }

        }

        #region "Private Debit Sale"

        private void PP_ProcessDebitCardSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.DebitInfo oCardInfo = new PrismPay.DebitInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _DbEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _DbEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oCardInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oCardInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oCardInfo.subid = GatewayManager.SubId;
                }

                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                PrismPay.EMVData oTmpEMV = new PrismPay.EMVData();
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata = oTmpEMV;

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processDebitSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }

        }

        private void WP_ProcessDebitCardSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.DebitInfo oCardInfo = new WorldPay.DebitInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _DbEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _DbEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oCardInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oCardInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oCardInfo.subid = GatewayManager.SubId;
                }

                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                WorldPay.EMVData oTmpEMV = new WorldPay.EMVData();
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata = oTmpEMV;

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processDebitSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }

        }

        #endregion

        #region "Private Debit Authorization"

        private void PP_ProcessDebitCardAuth(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.DebitInfo oCardInfo = new PrismPay.DebitInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _DbEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _DbEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oCardInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oCardInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oCardInfo.subid = GatewayManager.SubId;
                }

                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                PrismPay.EMVData oTmpEMV = new PrismPay.EMVData();
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata = oTmpEMV;

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processDebitSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }

        }

        private void WP_ProcessDebitCardAuth(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.DebitInfo oCardInfo = new WorldPay.DebitInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _DbEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _DbEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;


                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oCardInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oCardInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oCardInfo.subid = GatewayManager.SubId;
                }

                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                WorldPay.EMVData oTmpEMV = new WorldPay.EMVData();
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata = oTmpEMV;

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processDebitSale(oCardInfo);
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
                this.EncryptedPinData = "";
                this.SwipeData = "";
                this.DbCEncryptedDeviceType = DeviceType.None;
                this.DbCEncryptedSwipeData = "";
                this.Amount = 0.0f;
                
                //OGatewayClient = null;
                this.IsEMV = false;
              //  IsFSA = false;
                oEMV.Dispose();

            }
            GC.Collect();

        }
        #endregion



    }
}
