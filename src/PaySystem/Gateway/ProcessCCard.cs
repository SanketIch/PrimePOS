using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Gateway
{
    /// <summary>
    /// Properties and Methods that Handle CreditCard Processing
    /// </summary>
    public class ProcessCCard : IDisposable
    {
        #region "Private Fields"

        //private string _AccountId;
        private string _CCCustname;
        private float _Amount;
        private string _CCNumber;
        private int _CCExpMonth;
        private int _CCExpYear;
        private int _CCEncryptedDeviceType;
        private string _CCCvv2cid;
        private string _CCEncryptedSwipeData;
        private string _CCSwipedata;
        private string _MerchantPin;
        private FSAData _FSA = new FSAData();
        private EMV _oEMV = new EMV();
        private bool _IsNFC = false;

        private List<Tuple<string, byte[],string>> _SecureDataList;

        private float _GratuityAmount;

        

        #endregion

        
        public ProcessCCard()
        {
            this.IsEMV = false;
            IsFSA = false;
        }

        #region Properties

        public bool IsEMV { set; get; }

        public bool IsFSA { set; get; }

        public bool IsNFC { set; get; }
        
       
        /// <summary>
        /// Name of the Customer as it appears in the card
        /// </summary>
        public string CCCustname
        {
            set
            { this._CCCustname = value; }

        }

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
        /// Keyed in Credit card Number (use when Swipe reader is unable to detect the card number)
        /// </summary>
        public string CCNumber
        {
            set { this._CCNumber = value; }
        }

        /// <summary>
        /// Keyed in Credit card Expiration month (use when Swipe reader is unable to detect the card )
        /// </summary>
        public int CCExpMonth
        {
            set { this._CCExpMonth = value; }
        }

        /// <summary>
        /// Keyed in Credit card Expiration Year (use when Swipe reader is unable to detect the card )
        /// </summary>
        public int CCExpYear
        {
            set { this._CCExpYear = value; }
        }
        
        
        /// <summary>
        /// Determines the Encrypted Device type  The supported Devices are
        /// 1. Magtech (for most Magtech devices) 2. Magtech Ipad 3. IDTECH 4. Ingenico (iSC250/350  iPP250/350)
        /// </summary>
        public DeviceType CCEncryptedDeviceType
        {
            set
            {
                switch (value)
                {
                    case DeviceType.Magtech:
                        this._CCEncryptedDeviceType = 1;
                        break;
                    case DeviceType.Magtech_IPad:
                        this._CCEncryptedDeviceType = 2;
                        break;
                    case DeviceType.IDTECH:
                        this._CCEncryptedDeviceType = 3;
                        break;
                    case DeviceType.Ingenico:
                        this._CCEncryptedDeviceType = 4;
                        break;
                    default:
                        this._CCEncryptedDeviceType = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// Credit Card Verification code
        /// </summary>
        public string CCCvv2Cid
        {
            set { this._CCCvv2cid = value; }
        }

        /// <summary>
        /// Encrypted Credit Card Swipe Data (Must Include Track 1 and Track 2)
        /// Format of the data Depends on the device
        /// </summary>
        public string CCEncryptedSwipeData
        {
            set { this._CCEncryptedSwipeData = value; }
        }

        public string CCSwipedata
        {
            set { this._CCSwipedata = value; }
        }

        /// <summary>
        /// Merchant Unique pin . Leave it bank if you are unsure if you have one
        /// </summary>
        public string MerchantPin
        {
            set { this._MerchantPin = value; }
        }

        /// <summary>
        /// Sets the FSA HealthCare data
        /// </summary>
        public FSAData FSA
        {
            set { this._FSA = value; }
            get { return this._FSA; }
        }

        public EMV oEMV
        {
            set { this._oEMV = value; }
            get { return _oEMV; }
        }


        /// <summary>
        /// LIst That Contains the Encrypted Data to be passed on to the Gateway
        /// </summary>
        public List<Tuple<string, byte[], string>> SecureDataList
        {
            get { return _SecureDataList; }
            set { _SecureDataList = value; }
        }

        /// <summary>
        /// Gratuity (TIP) for Restraunts
        /// </summary>
        public float GratuityAmount
        {
            get { return _GratuityAmount; }
            set { _GratuityAmount = value; }
        }

        #endregion

        public void LoadSecureData(DataEncryptionKey Params)
        {
            foreach (var tmpSecureData in SecureDataList)
            {
                string tag = tmpSecureData.Item1;
                byte[] data = tmpSecureData.Item2;
                string oType = tmpSecureData.Item3;

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

                string decryptData = string.Empty;
                //decryptData = AppGlobal.Decryption(data, Params).ToUpper();

                switch (tag.ToUpper())
                {
                    case "EMSI":
                        this.CCEncryptedSwipeData = AppGlobal.Decryption(data, Params);
                        break;
                    case "MSID":
                        this.CCSwipedata = AppGlobal.Decryption(data, Params);
                        break;
                    case "TAMT":
                        this.Amount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                        break;
                    case "DNAME":
                        //this.MerchantPin = AppGlobal.Decryption(data, Params);
                        decryptData = AppGlobal.Decryption(data,Params);
                        switch (decryptData)
                        {
                            case "Magtech":
                                this.CCEncryptedDeviceType = DeviceType.Magtech;
                                break;
                            case "Magtech_Ipad":
                                this.CCEncryptedDeviceType = DeviceType.Magtech_IPad;
                                break;
                            case "IDTECH":
                                this.CCEncryptedDeviceType = DeviceType.IDTECH;
                                break;
                            case "Ingenico":
                                this.CCEncryptedDeviceType = DeviceType.Ingenico;
                                break;
                            default:
                                this.CCEncryptedDeviceType = DeviceType.None;
                                break;

                        }
                        break;
                    case "RXAMOUNT":
                        this.FSA.RxAmount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                        this.FSA.IsFSA = true;
                        break;
                    case "TIPAMT":
                        this._GratuityAmount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                        break;
                    case "MCARDHOLDER":
                        this.CCCustname = AppGlobal.Decryption(data, Params);
                        break;
                    case "MCARDDIGIT":
                        this.CCNumber = AppGlobal.Decryption(data, Params);
                        break;
                    case "MEXPYEAR":
                        this.CCExpYear = Convert.ToInt32(AppGlobal.Decryption(data, Params));
                        break;
                    case "MEXPMONTH":
                        this.CCExpMonth = Convert.ToInt32(AppGlobal.Decryption(data, Params));
                        break;
                    case "CVV":
                        this.CCCvv2Cid = AppGlobal.Decryption(data, Params);
                        break;
                    default:
                        break;


                }
            }
            if (this.IsEMV)
            {
                this.oEMV.SecureDataList = SecureDataList;
                this.oEMV.LoadSecureData(Params);
            }

        }

        /// <summary>
        /// Processes the Creditcard sale Transaction and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void ProcessCreditCardSale(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
             oresult = new TransactionResult();

             switch (GatewayManager.SelectedGateway)
             {
                 case Gateway.PrismPay:
                     PP_ProcessCreditcardSale(out oresult, out ex);
                     break;
                 case Gateway.WorldPay:
                     WP_ProcessCreditcardSale(out oresult, out ex);
                     break;
                 default:
                     ex = new Exception("Gateway not selected");
                     break;
             }

            

        }


        /// <summary>
        /// Processes the Creditcard Authorization and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void ProcessCreditcardAuth(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessCreditcardAuth(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessCreditcardAuth(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }



        }

        #region "Private Credit Sale"

        private void PP_ProcessCreditcardSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.CreditCardInfo oCardInfo = new PrismPay.CreditCardInfo();
                oCardInfo.acctid = GatewayManager.AccountId;

                oCardInfo.ccname = _CCCustname;
                oCardInfo.ccnum = _CCNumber;
                oCardInfo.expmon = _CCExpMonth;
                oCardInfo.expyear = _CCExpYear;
                oCardInfo.cvv2_cid = _CCCvv2cid;
                oCardInfo.swipedata = _CCSwipedata;

                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _CCEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _CCEncryptedSwipeData;
                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oCardInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oCardInfo.merchantpin = GatewayManager.MerchantPin;


                if(!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oCardInfo.subid = GatewayManager.SubId;
                }
                if (GratuityAmount > 0)
                {
                    PrismPay.Restaurant orestraunt = new PrismPay.Restaurant();
                    orestraunt.gratuityamount = _GratuityAmount;
                    oCardInfo.restaurant = orestraunt;

                }

                PrismPay.EMVData oTmpEMV = new PrismPay.EMVData();

                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                _FSA.LoadFSAData(oCardInfo.fsa);
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata=oTmpEMV;

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processCCSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_ProcessCreditcardSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.CreditCardInfo oCardInfo = new WorldPay.CreditCardInfo();
                oCardInfo.acctid = GatewayManager.AccountId;

                oCardInfo.ccname = _CCCustname;
                oCardInfo.ccnum = _CCNumber;
                oCardInfo.expmon = _CCExpMonth;
                oCardInfo.expyear = _CCExpYear;
                oCardInfo.cvv2_cid = _CCCvv2cid;
                oCardInfo.swipedata = _CCSwipedata;

                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _CCEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _CCEncryptedSwipeData;

                if (GratuityAmount > 0)
                {
                    WorldPay.Restaurant orestraunt = new WorldPay.Restaurant();
                    orestraunt.gratuityamount = _GratuityAmount;
                    oCardInfo.restaurant = orestraunt;

                }

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
                WorldPay.EMVData oTmpEMV = new WorldPay.EMVData();
                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                _FSA.LoadFSAData(oCardInfo.fsa);
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata = oTmpEMV;

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processCCSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        #endregion


        #region "Private Credit Authorization"

        private void PP_ProcessCreditcardAuth(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.CreditCardInfo oCardInfo = new PrismPay.CreditCardInfo();
                oCardInfo.acctid = GatewayManager.AccountId;

                oCardInfo.ccname = _CCCustname;
                oCardInfo.ccnum = _CCNumber;
                oCardInfo.expmon = _CCExpMonth;
                oCardInfo.expyear = _CCExpYear;
                oCardInfo.cvv2_cid = _CCCvv2cid;
                oCardInfo.swipedata = _CCSwipedata;

                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _CCEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _CCEncryptedSwipeData;
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
                if (GratuityAmount > 0)
                {
                    PrismPay.Restaurant orestraunt = new PrismPay.Restaurant();
                    orestraunt.gratuityamount = _GratuityAmount;
                    oCardInfo.restaurant = orestraunt;

                }

                PrismPay.EMVData oTmpEMV = new PrismPay.EMVData();

                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                _FSA.LoadFSAData(oCardInfo.fsa);
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata = oTmpEMV;

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processCCAuth(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_ProcessCreditcardAuth(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.CreditCardInfo oCardInfo = new WorldPay.CreditCardInfo();
                oCardInfo.acctid = GatewayManager.AccountId;

                oCardInfo.ccname = _CCCustname;
                oCardInfo.ccnum = _CCNumber;
                oCardInfo.expmon = _CCExpMonth;
                oCardInfo.expyear = _CCExpYear;
                oCardInfo.cvv2_cid = _CCCvv2cid;
                oCardInfo.swipedata = _CCSwipedata;

                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _CCEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _CCEncryptedSwipeData;

                if (GratuityAmount > 0)
                {
                    WorldPay.Restaurant orestraunt = new WorldPay.Restaurant();
                    orestraunt.gratuityamount = _GratuityAmount;
                    oCardInfo.restaurant = orestraunt;

                }

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
                WorldPay.EMVData oTmpEMV = new WorldPay.EMVData();
                if (IsNFC)
                {
                    oCardInfo.contactlessflag = 1;
                }

                _FSA.LoadFSAData(oCardInfo.fsa);
                oEMV.LoadEMV(out oTmpEMV);
                oCardInfo.emvdata = oTmpEMV;

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processCCAuth(oCardInfo);
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
                this.CCCustname = "";
                this.CCNumber = "";
                this.CCExpYear = 0;
                this.CCExpMonth = 0;
                this.CCCvv2Cid = "";
                this.CCEncryptedDeviceType = DeviceType.None;
                this.CCEncryptedSwipeData = "";
                this.Amount = 0.0f;
                this.CCSwipedata = "";
                this.MerchantPin = "";
                if (_FSA != null)
                {
                    _FSA.Dispose();
                }
                //oCardInfo = null;
                //OGatewayClient = null;
                this.IsEMV = false;
                IsFSA = false;
                oEMV.Dispose();
                //oCardInfo = null;
                //OGatewayClient = null;


            }
            GC.Collect();

        }
        #endregion

        
    }
}
