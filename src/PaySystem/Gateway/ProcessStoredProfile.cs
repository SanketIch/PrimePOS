using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    /// <summary>
    /// Handles the properties and Methords for storing the Payment Profile on the Gateway Vault and use the stored profile to process a transaction
    /// </summary>
    public class ProcessStoredProfile : IDisposable
    {
        #region "Private Fields"

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
        private int _ProfileAuthenticationMethord = 0;

        private string _UserProfileID;
        private string _Last4Digits;

        private int _AccountType=1;

        private int _PreAuthonUpdate;
        private string _IPAddress;
        private EMV _oEMV = new EMV();

        private FSAData _FSA = new FSAData();

        private List<Tuple<string, byte[],string>> _SecureDataList;
        

        #endregion

        #region "Public Properties"

        public bool IsEMV { set; get; }

        public bool IsFSA { set; get; }

        /// <summary>
        /// Optional:  for Profile add / Or Update
        /// Name of the Customer as it appears in the card
        /// </summary>
        public string CCCustname
        {
            set
            { this._CCCustname = value; }

        }

        /// <summary>
        /// Optional:  for Profile add / Or Update
        /// required for sall and credit
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
        /// Optional:  for Profile add / Or Update
        /// Keyed in Credit card Number (use when Swipe reader is unable to detect the card number)
        /// </summary>
        public string CCNumber
        {
            set { this._CCNumber = value; }
        }

        /// <summary>
        /// Optional:  for Profile add / Or Update
        /// Keyed in Credit card Expiration month (use when Swipe reader is unable to detect the card )
        /// </summary>
        public int CCExpMonth
        {
            set { this._CCExpMonth = value; }
        }

        /// <summary>
        /// Optional:  for Profile add / Or Update
        /// Keyed in Credit card Expiration Year (use when Swipe reader is unable to detect the card )
        /// </summary>
        public int CCExpYear
        {
            set { this._CCExpYear = value; }
        }


        /// <summary>
        /// Optional:  for Profile add / Or Update
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
        /// Optional:  for Profile add / Or Update
        /// Credit Card Verification code
        /// </summary>
        public string CCCvv2Cid
        {
            set { this._CCCvv2cid = value; }
        }

        /// <summary>
        /// Optional:  for Profile add / Or Update
        /// Encrypted Credit Card Swipe Data (Must Include Track 1 and Track 2)
        /// Format of the data Depends on the device
        /// </summary>
        public string CCEncryptedSwipeData
        {
            set { this._CCEncryptedSwipeData = value; }
        }

        /// <summary>
        /// Optional:  for Profile add / Or Update
        /// Card Swipe data must include either track1 or track2 data
        /// </summary>
        public string CCSwipedata
        {
            set { this._CCSwipedata = value; }
        }

        /// <summary>
        /// Optional on all Methods
        /// Merchant Unique pin . Leave it bank if you are unsure if you have one
        /// </summary>
        public string MerchantPin
        {
            set { this._MerchantPin = value; }
        }


        /// <summary>
        /// Required for Profile Add
        /// Accepts Integers 0-3. Default 0
        /// 0: Will validate the CC with $1.00 Authorization. If the Authorization is successful, then the card is added to the vault
        /// 1: Will run an Authorization of the amount requested. If the Authorization is successful, then the card is added to the vault
        /// 2: Will run a sale of the Amount requested . Iff sale is successful, then the card is added to the vault
        /// 3: Will Import the Paymenttype to the vault, no other transactions will be done
        /// </summary>
        public int ProfileAuthenticationMethord
        {
            //get { return _ProfileAuthenticationMethord; }
            set { _ProfileAuthenticationMethord = value; }
        }

        /// <summary>
        /// Required for Sale, Credit, Update, Retrieve and Delete
        /// Profile ID of the card holder (Token)
        /// </summary>
        public string UserProfileID
        {
            set { this._UserProfileID = value; }
        }

        /// <summary>
        /// Required for Sale, Credit, Update, Retrieve and Delete
        /// Last 4 digits of the credit card or ACH number
        /// </summary>
        public string Last4Digits
        {
            set { this._Last4Digits = value; }
        }

        /// <summary>
        /// Required for profile Update
        /// 1=Credit card, 2 = ACH
        /// </summary>
        public int AccountType
        {
            set { this._AccountType = value; }
        }

        /// <summary>
        /// Required for Profile Update
        /// Accepts int Default is empty (PreAuthorize CC on update)
        /// 1: Do not Pre Authorize CC
        /// </summary>
        public int PreAuthonUpdate
        {
            set { this._PreAuthonUpdate = value; }
        }


        /// <summary>
        /// Required for Profile Delete
        /// Customers WebBrowser IPAddress
        /// </summary>
        public string IPAddress
        {
            set { this._IPAddress = value; }
        }


        /// <summary>
        /// Sets the FSA HealthCare data
        /// </summary>
        public FSAData FSA
        {
            set { this._FSA = value; }
            get { return this._FSA; }
        }


        /// <summary>
        /// LIst That Contains the Encrypted Data to be passed on to the Gateway
        /// </summary>
        public List<Tuple<string, byte[],string>> SecureDataList
        {
            get { return _SecureDataList; }
            set { _SecureDataList = value; }
        }


        public EMV oEMV
        {
            set { this._oEMV = value; }
            get { return _oEMV; }
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

               /* if (!this.IsNFC)
                {
                    if (oType.ToUpper() == "NFC")
                    {
                        this.IsNFC = true;
                    }
                }
                */
                string decryptData = string.Empty;
                //decryptData = AppGlobal.Decryption(data, Params).ToUpper();

                switch (tag.ToUpper())
                {
                    case "EMSI":
                        this.CCEncryptedSwipeData = AppGlobal.Decryption(data, Params);
                        break;
                    case "TAMT":
                        this.Amount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                        break;
                    case "DNAME":
                        //this.MerchantPin = AppGlobal.Decryption(data, Params);
                        decryptData = AppGlobal.Decryption(data, Params);
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
                    case "MCARDHOLDER":
                        this.CCCustname = AppGlobal.Decryption(data, Params);
                        break;
                    case "MCARDDIGIT":
                        this.CCNumber = AppGlobal.Decryption(data, Params);
                        break;
                    case "MEXPYEAR":
                        this.CCExpYear = Convert.ToInt32( AppGlobal.Decryption(data, Params));
                        break;
                    case "MEXPMONTH":
                        this.CCExpMonth = Convert.ToInt32(AppGlobal.Decryption(data, Params));
                        break;
                    case "CVV":
                        this.CCCvv2Cid = AppGlobal.Decryption(data, Params);
                        break;
                    case "PAUTHMETHOD":
                        this.ProfileAuthenticationMethord = Convert.ToInt32(AppGlobal.Decryption(data, Params));
                        break;
                    case "ACCOUNTTYPE":
                        this.AccountType = Convert.ToInt32(AppGlobal.Decryption(data, Params));
                        break;
                    case "PREAUTHUPDATE":
                        this.PreAuthonUpdate = Convert.ToInt32(AppGlobal.Decryption(data, Params));
                        break;
                    case "TOKENID":
                        this.UserProfileID = AppGlobal.Decryption(data, Params);
                        break;
                    case "LAST4":
                        this.Last4Digits = AppGlobal.Decryption(data, Params);
                        break;
                    default:
                        break;


                }
            }
            if (this.IsEMV)
            {
                this.oEMV.LoadSecureData(Params);
            }

        }

        /*public void LoadSecureData(DataEncryptionKey Params)
        {
            foreach (var tmpSecureData in SecureDataList)
            {
                string tag = tmpSecureData.Item1;
                byte[] data = tmpSecureData.Item2;

                string decryptData = string.Empty;
                //decryptData = AppGlobal.Decryption(data, Params).ToUpper();

                switch (tag.ToUpper())
                {
                    case "EMSI":
                        this.CCEncryptedSwipeData = AppGlobal.Decryption(data, Params);
                        break;
                    case "TAMT":
                        this.Amount = float.Parse(AppGlobal.Decryption(data, Params), System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
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
                    default:
                        break;


                }
            }
        }*/

        /// <summary>
        /// Adds a CreditCard Profile to the Vault and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class
        /// The User Profile ID of the  customer is also returned as an object of the result class (UserProfileID) Property</param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void AddCreditCardProfile(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_AddCreditCardProfile(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_AddCreditCardProfile(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }


        /// <summary>
        /// Processes the Stored profile  sale Transaction and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void ProcessStoredProfileSale(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessStoredProfileSale(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessStoredProfileSale(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }


        // <summary>
        /// Processes the Stored profile  Credit Transaction and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void ProcessStoredProfileCredit(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessStoredProfileCredit(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessStoredProfileCredit(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }


        /// <summary>
        /// Updates a stored profile and returns the result as an output parameter
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void UpdateStoredProfile(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessStoredProfileUpdate(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessStoredProfileUpdate(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }


        /// <summary>
        /// Retrieves the Account Number, address and billing info
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void RetrieveStoredProfile(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessStoredProfileRetrive(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessStoredProfileRetrive(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Deletes a stored profile from the vault
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null </param>
        public void DeleteStoredProfile(out TransactionResult oresult, out Exception ex)
        {
            //resultString = "";
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessStoredProfileDelete(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessStoredProfileDelete(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }


        #region "Add Credit card Profile"

        private void PP_AddCreditCardProfile(out TransactionResult oResult, out Exception oEx)
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

                //oCardInfo.merchantpin = _MerchantPin;
                oCardInfo.profileactiontype = _ProfileAuthenticationMethord;

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
                //_FSA.LoadFSAData(oCardInfo.fsa);

                PrismPay.ProcessProfileResult oProcessResult = new PrismPay.ProcessProfileResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processCCProfileAdd(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_AddCreditCardProfile(out TransactionResult oResult, out Exception oEx)
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

                //oCardInfo.merchantpin = _MerchantPin;
                oCardInfo.profileactiontype = _ProfileAuthenticationMethord;

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

                //_FSA.LoadFSAData(oCardInfo.fsa);

                WorldPay.ProcessProfileResult oProcessResult = new WorldPay.ProcessProfileResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processCCProfileAdd(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }


        }

        #endregion


        #region "Process Stored profile Sale"

        private void PP_ProcessStoredProfileSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.ProfileSale oProfileInfo = new PrismPay.ProfileSale();
                oProfileInfo.acctid = GatewayManager.AccountId;
                oProfileInfo.amount = _Amount;
                //oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;
                

                _FSA.LoadFSAData(oProfileInfo.fsa);
                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                //_FSA.LoadFSAData(oCardInfo.fsa);

                PrismPay.ProcessProfileResult oProcessResult = new PrismPay.ProcessProfileResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileSale(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_ProcessStoredProfileSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.ProfileSale oProfileInfo = new WorldPay.ProfileSale();
                oProfileInfo.acctid = GatewayManager.AccountId;
                oProfileInfo.amount = _Amount;
               // oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;

                _FSA.LoadFSAData(oProfileInfo.fsa);
                //_FSA.LoadFSAData(oCardInfo.fsa);

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                WorldPay.ProcessProfileResult oProcessResult = new WorldPay.ProcessProfileResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileSale(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }


        }

        #endregion

        #region "Process Stored profile Credit"

        private void PP_ProcessStoredProfileCredit(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.ProfileCredit oProfileInfo = new PrismPay.ProfileCredit();
                oProfileInfo.acctid = GatewayManager.AccountId;
                oProfileInfo.amount = _Amount;
            //    oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;


                //_FSA.LoadFSAData(oCardInfo.fsa);
                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                PrismPay.ProcessProfileResult oProcessResult = new PrismPay.ProcessProfileResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileCredit(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_ProcessStoredProfileCredit(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.ProfileCredit oProfileInfo = new WorldPay.ProfileCredit();
                oProfileInfo.acctid = GatewayManager.AccountId;
                oProfileInfo.amount = _Amount;
            //    oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;

                //_FSA.LoadFSAData(oCardInfo.fsa);
                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                WorldPay.ProcessProfileResult oProcessResult = new WorldPay.ProcessProfileResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileCredit(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }


        }

        #endregion


        #region "Process Stored profile Update"

        private void PP_ProcessStoredProfileUpdate(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.ProfileUpdate oProfileInfo = new PrismPay.ProfileUpdate();
                oProfileInfo.acctid = GatewayManager.AccountId;
                //oProfileInfo.amount = _Amount;
            //    oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;

                oProfileInfo.ccname = _CCCustname;
                oProfileInfo.ccnum = _CCNumber;
                oProfileInfo.expmon = _CCExpMonth;
                oProfileInfo.expyear = _CCExpYear;
                oProfileInfo.cvv2_cid = _CCCvv2cid;
                oProfileInfo.swipedata = _CCSwipedata;

                //oProfileInfo.amount = _Amount;
                oProfileInfo.encryptedreadertype = _CCEncryptedDeviceType;
                oProfileInfo.encryptedswipedata = _CCEncryptedSwipeData;

                oProfileInfo.accttype = _AccountType;
                oProfileInfo.profilenobill = _PreAuthonUpdate;

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                //_FSA.LoadFSAData(oCardInfo.fsa);

                PrismPay.ProcessProfileResult oProcessResult = new PrismPay.ProcessProfileResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileUpdate(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_ProcessStoredProfileUpdate(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.ProfileUpdate oProfileInfo = new WorldPay.ProfileUpdate();
                oProfileInfo.acctid = GatewayManager.AccountId;
                //oProfileInfo.amount = _Amount;
            //    oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;

                oProfileInfo.accttype = _AccountType;
                oProfileInfo.profilenobill = _PreAuthonUpdate;

                oProfileInfo.ccname = _CCCustname;
                oProfileInfo.ccnum = _CCNumber;
                oProfileInfo.expmon = _CCExpMonth;
                oProfileInfo.expyear = _CCExpYear;
                oProfileInfo.cvv2_cid = _CCCvv2cid;
                oProfileInfo.swipedata = _CCSwipedata;

                //oProfileInfo.amount = _Amount;
                oProfileInfo.encryptedreadertype = _CCEncryptedDeviceType;
                oProfileInfo.encryptedswipedata = _CCEncryptedSwipeData;

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                WorldPay.ProcessProfileResult oProcessResult = new WorldPay.ProcessProfileResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileUpdate(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }


        }

        #endregion

        #region "Process Stored profile Retrive"

        private void PP_ProcessStoredProfileRetrive(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.ProfileRetrieve oProfileInfo = new PrismPay.ProfileRetrieve();
                oProfileInfo.acctid = GatewayManager.AccountId;
                //oProfileInfo.amount = _Amount;
            //    oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;


                //_FSA.LoadFSAData(oCardInfo.fsa);
                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                PrismPay.ProcessProfileResult oProcessResult = new PrismPay.ProcessProfileResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileRetrieve(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_ProcessStoredProfileRetrive(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.ProfileRetrieve oProfileInfo = new WorldPay.ProfileRetrieve();
                oProfileInfo.acctid = GatewayManager.AccountId;
                //oProfileInfo.amount = _Amount;
            //    oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;

                //_FSA.LoadFSAData(oCardInfo.fsa);

                if (string.IsNullOrEmpty(GatewayManager.MerchantPin))
                {
                    if (!string.IsNullOrEmpty(_MerchantPin))
                    {
                        oProfileInfo.merchantpin = _MerchantPin;
                    }
                }
                else
                    oProfileInfo.merchantpin = GatewayManager.MerchantPin;


                if (!string.IsNullOrEmpty(GatewayManager.SubId))
                {
                    oProfileInfo.subid = GatewayManager.SubId;
                }

                WorldPay.ProcessProfileResult oProcessResult = new WorldPay.ProcessProfileResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileRetrieve(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }


        }

        #endregion

        #region "Process Stored Profile Delete"

        private void PP_ProcessStoredProfileDelete(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.ProfileDelete oProfileInfo = new PrismPay.ProfileDelete();
                oProfileInfo.acctid = GatewayManager.AccountId;
                //oProfileInfo.amount = _Amount;
                oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;
                oProfileInfo.ipaddress = _IPAddress;


                //_FSA.LoadFSAData(oCardInfo.fsa);

                PrismPay.ProcessProfileResult oProcessResult = new PrismPay.ProcessProfileResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileDelete(oProfileInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }




        }

        private void WP_ProcessStoredProfileDelete(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.ProfileDelete oProfileInfo = new WorldPay.ProfileDelete();
                oProfileInfo.acctid = GatewayManager.AccountId;
                //oProfileInfo.amount = _Amount;
                oProfileInfo.merchantpin = _MerchantPin;
                oProfileInfo.userprofileid = _UserProfileID;
                oProfileInfo.last4digits = _Last4Digits;
                oProfileInfo.ipaddress = _IPAddress;

                //_FSA.LoadFSAData(oCardInfo.fsa);

                WorldPay.ProcessProfileResult oProcessResult = new WorldPay.ProcessProfileResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processProfileDelete(oProfileInfo);
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
                this.PreAuthonUpdate = 0;
                this.UserProfileID = "";
                this.Last4Digits = "";
                this.AccountType = 1;
                this.ProfileAuthenticationMethord = 0;
                this.IPAddress = "";

                if (_FSA != null)
                {
                    _FSA.Dispose();
                }
                //oCardInfo = null;
                //OGatewayClient = null;


            }
            GC.Collect();

        }
        #endregion

    }
}
