using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    /// <summary>
    /// Handles Properties and Methords for the processing of EBT(Food Stamp)
    /// </summary>
    public class ProcessEBT:IDisposable
    {

        #region "Private Fields"

        private float _Amount;
        private string _SwipeData;
        private string _EncryptedPinData;
        private int _EBTEncryptedDeviceType;
        private string _EBTEncryptedSwipeData;
        private float _CashBackAmount;
        private string _EBTApprovalField;
        private string _EBTVoucherSerialNumber;

        private string _MerchantPin;

        private List<Tuple<string, byte[],string>> _SecureDataList;

        #endregion

        #region Properties


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
        /// 32 Byte Encrypted Pin Pad Data(Includes 16 Bytes of PIN data followed by 6 Byte Key set abd 10 Byte PIN Pad Serial Number)
        /// </summary>
        public string EncryptedPinData
        {
            set { this._EncryptedPinData = value; }
        }

        /// <summary>
        /// Determines the Encrypted Device type The supported Devices are
        /// 1. Magtech (for most Magtech devices) 2. Magtech Ipad 3. IDTECH 4. Ingenico (iSC250/350  iPP250/350)
        /// </summary>
        public DeviceType EBTEncryptedDeviceType
        {
            set
            {
                switch (value)
                {
                    case DeviceType.Magtech:
                        this._EBTEncryptedDeviceType = 1;
                        break;
                    case DeviceType.Magtech_IPad:
                        this._EBTEncryptedDeviceType = 2;
                        break;
                    case DeviceType.IDTECH:
                        this._EBTEncryptedDeviceType = 3;
                        break;
                    case DeviceType.Ingenico:
                        this._EBTEncryptedDeviceType = 4;
                        break;
                    default:
                        this._EBTEncryptedDeviceType = 0;
                        break;
                }
            }
        }



        /// <summary>
        /// Encrypted Card Swipe Data (Must Include Track 1 and Track 2)
        /// Format of the data Depends on the device
        /// </summary>
        public string EBTEncryptedSwipeData
        {
            set { this._EBTEncryptedSwipeData = value; }
        }

        /// <summary>
        /// Cash back amount in the format 0.00
        /// </summary>
        public float CashBackAmount
        { 
            set { this._CashBackAmount = value; } 
        }


        /// <summary>
        /// EBT Approval Code
        /// </summary>
        public string EBTApprovalCode
        {
            set { this._EBTApprovalField = value; }
        }

        /// <summary>
        /// EBT Voucher Serial Number
        /// </summary>
        public string EBTVoucherSerialNumber
        {
            set { this._EBTVoucherSerialNumber = value; }
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
        /// LIst That Contains the Encrypted Data to be passed on to the Gateway
        /// </summary>
        public List<Tuple<string, byte[],string>> SecureDataList
        {
            get { return _SecureDataList; }
            set { _SecureDataList = value; }
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

                string decryptData = string.Empty;
                //decryptData = AppGlobal.Decryption(data, Params).ToUpper();

                switch (tag.ToUpper())
                {
                    case "EMSI":
                        this.EBTEncryptedSwipeData = AppGlobal.Decryption(data, Params);
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
                                this.EBTEncryptedDeviceType = DeviceType.Magtech;
                                break;
                            case "Magtech_Ipad":
                                this.EBTEncryptedDeviceType = DeviceType.Magtech_IPad;
                                break;
                            case "IDTECH":
                                this.EBTEncryptedDeviceType = DeviceType.IDTECH;
                                break;
                            case "Ingenico":
                                this.EBTEncryptedDeviceType = DeviceType.Ingenico;
                                break;
                            default:
                                this.EBTEncryptedDeviceType = DeviceType.None;
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
                    case "EBTAPPROVALCODE":
                        this.EBTApprovalCode = AppGlobal.Decryption(data, Params);
                        break;
                    case "EBTVOUCHERSERIALNUMBER":
                        this.EBTVoucherSerialNumber = AppGlobal.Decryption(data, Params);
                        break;
                    default:
                        break;


                }
            }
            this.EncryptedPinData = pindata + keyset + pinpadserial;
        }

        /// <summary>
        /// Process a EBT Balance Inquiry
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTBalInquiry(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTBalInquiry(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTBalInquiry(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT Cash Benefit Balance Inquiry
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTCashBenefitBalInquiry(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTCashBenefitBalInquiry(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTCashBenefitBalInquiry(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT Cash Benefit Return
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTCashBenefitReturn(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTCashBenefitReturn(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTCashBenefitReturn(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT Cash Benefit Withdrawal
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTCashBenefitWithDraw(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTCashBenefitWithDraw(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTCashBenefitWithDraw(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT Cash Benefit Sale
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTCashBenefitSale(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTCashBenefitSale(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTCashBenefitSale(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT FoodStamp Voucher Sale
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTFoodStampVoucherSale(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTFoodStampVoucherSale(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTFoodStampVoucherSale(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT FoodStamp Return
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTFoodStampReturn(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTFoodStampReturn(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTFoodStampReturn(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT FoodStamp Sale
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTFoodStampSale(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTFoodStampSale(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTFoodStampSale(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }

        /// <summary>
        /// Process a EBT FoodStamp Balance Inquiry
        /// </summary>
        /// <param name="oresult">Returns the Transaction result as an instance of the TransactionResult class </param>
        /// <param name="ex">Returns the Exception(if any) that Occured During the Transaction or null</param>
        public void ProcessEBTFoodStampBalanceInquiry(out TransactionResult oresult, out Exception ex)
        {
            ex = null;
            oresult = new TransactionResult();

            switch (GatewayManager.SelectedGateway)
            {
                case Gateway.PrismPay:
                    PP_ProcessEBTFoodStampBalanceInquiry(out oresult, out ex);
                    break;
                case Gateway.WorldPay:
                    WP_ProcessEBTFoodStampBalanceInquiry(out oresult, out ex);
                    break;
                default:
                    ex = new Exception("Gateway not selected");
                    break;
            }
        }




        #region "Private EBT Balance Inquiry"

        private void PP_ProcessEBTBalInquiry(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTBalanceInquiry(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTBalInquiry(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTBalanceInquiry(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion


        #region "Private EBT Cash Benifit Balance Inquiry"

        private void PP_ProcessEBTCashBenefitBalInquiry(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitBalanceInquiry(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTCashBenefitBalInquiry(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitBalanceInquiry(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion

        #region "Private EBT Cash Benefit Return"

        private void PP_ProcessEBTCashBenefitReturn(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitReturn(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTCashBenefitReturn(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitReturn(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion

        #region "Private EBT Cash Benefit WithDrawal"

        private void PP_ProcessEBTCashBenefitWithDraw(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitWithdrawal(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTCashBenefitWithDraw(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitWithdrawal(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion

        #region "Private EBT Cash Benefit Sale"

        private void PP_ProcessEBTCashBenefitSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTCashBenefitSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTCashBenefitSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion


        #region "Private EBT Food Stamp Voucher Sale"

        private void PP_ProcessEBTFoodStampVoucherSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampVoucherSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTFoodStampVoucherSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampVoucherSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion


        #region "Private EBT Food Stamp Return"

        private void PP_ProcessEBTFoodStampReturn(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampReturn(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTFoodStampReturn(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampReturn(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion

        #region "Private EBT Food Stamp Sale"

        private void PP_ProcessEBTFoodStampSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTFoodStampSale(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampSale(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }

        #endregion

        #region "Private EBT Food Stamp Balance Inquiry"

        private void PP_ProcessEBTFoodStampBalanceInquiry(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                PrismPay.EBTInfo oCardInfo = new PrismPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

                PrismPay.ProcessResult oProcessResult = new PrismPay.ProcessResult();
                PrismPay.TransactionSOAPBindingImplService oGatewayService = new PrismPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampBalanceInquiry(oCardInfo);
                oResult.LoadResultData(oProcessResult);
            }
            catch (Exception e)
            {
                Log.Net.Logs.Logger(Log.Net.BodyType.Body, e.ToString(), Log.Net.ErrorLevel.Critical);
                oEx = e;
            }
        }


        private void WP_ProcessEBTFoodStampBalanceInquiry(out TransactionResult oResult, out Exception oEx)
        {
            oResult = new TransactionResult();
            oEx = null;
            try
            {
                WorldPay.EBTInfo oCardInfo = new WorldPay.EBTInfo();

                oCardInfo.acctid = GatewayManager.AccountId;
                oCardInfo.swipedata = _SwipeData;
                oCardInfo.customerid = _EncryptedPinData;
                oCardInfo.amount = _Amount;
                oCardInfo.encryptedreadertype = _EBTEncryptedDeviceType;
                oCardInfo.encryptedswipedata = _EBTEncryptedSwipeData;
                oCardInfo.cashbackamount = _CashBackAmount;
                oCardInfo.ebtapprovalcode = _EBTApprovalField;
                oCardInfo.voucherserialnumber = _EBTVoucherSerialNumber;

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

                WorldPay.ProcessResult oProcessResult = new WorldPay.ProcessResult();
                WorldPay.TransactionSOAPBindingImplService oGatewayService = new WorldPay.TransactionSOAPBindingImplService();

                oProcessResult = oGatewayService.processEBTFoodStampBalanceInquiry(oCardInfo);
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
                this.SwipeData = "";
                this.EncryptedPinData = "";
                this.EBTEncryptedDeviceType = DeviceType.None;
                this.EBTEncryptedSwipeData = "";
                this.CashBackAmount = 0.0f;
                this.EBTApprovalCode = "";
                this.EBTVoucherSerialNumber = "";
            }
            GC.Collect();

        }
        #endregion

    }
}
