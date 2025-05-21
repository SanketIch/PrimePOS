using Microsoft.Win32;
using NLog;
using PrimeRxPay;
using System;
using System.Drawing;
using System.IO;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace PrimeRxPay
{
    public partial class CardDetails : Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private string _url = string.Empty;
        private string _primeRxPayUrl = string.Empty;
        private string _transId = string.Empty;
        private string _clientId = string.Empty;
        private string _secretkey = string.Empty;
        private string _payproviderId = string.Empty;
        private string _transSetupId = string.Empty;
        public string getTransactionResponse = string.Empty;        
        int uiCount = 8;
        public CardDetails(string Url, string TransId, string PrimeRxPayUrl,string ClientId,string Secretkey,string PayProviderID, string TransactionSetupID)
        {
            this._url = Url;
            this._transId = TransId;
            this._primeRxPayUrl = PrimeRxPayUrl;
            this._clientId = ClientId;
            this._secretkey = Secretkey;
            this._payproviderId = PayProviderID;
            this._transSetupId = TransactionSetupID;
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string navigationUrl = e.Url.ToString();
            if (!InternetState.IsConnectedToInternet())
            {
                MessageBox.Show(" You are not connected to Internet ", "Connection Issue");
                this.Close();
            }
            logger.Trace(navigationUrl.ToString());
            var uri = new Uri(this._primeRxPayUrl);//PRIMEPOS-3032

            //PRIMEPOS-3057 add payproviderId condition
            if ((this._payproviderId != ((int)PaymentProviderEnum.HeartlandPayment).ToString()
                && navigationUrl.Replace("-", "").ToUpper().Contains(uri.Host.ToUpper())) ||
                (this._payproviderId == ((int)PaymentProviderEnum.HeartlandPayment).ToString()
                && navigationUrl.Replace("-", "").ToUpper().Contains("CHARGE"))
                )

            //if (navigationUrl.Replace("-", "").ToUpper().Contains(uri.Host.ToUpper()))//PRIMEPOS-3032
            //if (!navigationUrl.Replace("-","").ToUpper().Contains("HOSTEDPAYMENTS"))
            {
                try
                {
                    logger.Trace("Entered in GetTransactionDetail DocumentComplete Method()");
                    string response = string.Empty;
                    int count = 0;
                    System.Net.HttpStatusCode httpStatusCode = System.Net.HttpStatusCode.ServiceUnavailable;
                    logger.Trace("HTTPSTATUSCODE = " + httpStatusCode.ToString());
                    while (httpStatusCode != System.Net.HttpStatusCode.OK)
                    {
                        logger.Trace("COUNT = "+count.ToString());
                        if (count < 3)
                        {
                            logger.Trace("ENTERED IN ATTEMPT GetTransactionDetail");
                            Thread.Sleep(120);
                            var uriBuilder = new UriBuilder(string.Concat(_primeRxPayUrl, Constant.GetTransactionStatusUrl));
                            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                            query["primerxPayTransId"] = this._transId;
                            query["applicationId"] = "2";
                            query["paymentProviderId"] = this._payproviderId;
                            query["transSetupID"] = this._transSetupId;
                            uriBuilder.Query = query.ToString();

                            string getTransactionURL = uriBuilder.ToString();
                            response = Constant.SendRequestGet(getTransactionURL, out httpStatusCode,_clientId,_secretkey);
                            count++;
                            if (count == 3)
                            {
                                if (httpStatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                                {
                                    logger.Trace(" You are not connected to Internet ");
                                    MessageBox.Show(" You are not connected to Internet ", " Internet Connection");
                                }
                                break;
                            }
                        }
                    }
                    this.getTransactionResponse = response;
                    logger.Debug(" GetTransaction Response : " + response);
                    this.Close();
                }
                catch (Exception ex)
                {
                    logger.Error("ERROR IN DOCUMENTCOMPLETE : " + ex.ToString());
                }
            }
        }             
        private void CardDetails_Load(object sender, EventArgs e)
        {
            if (!WBEmulator.IsBrowserEmulationSet())
            {
                WBEmulator.SetBrowserEmulationVersion();
            }
            try
            {
                webBrowser1.Navigate(this._url);
                if (!InternetState.IsConnectedToInternet())
                {
                    MessageBox.Show(" You are not connected to Internet ", "Connection Issue");
                    this.Close();
                }
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }

        public enum BrowserEmulationVersion
        {
            Default = 0,
            Version7 = 7000,
            Version8 = 8000,
            Version8Standards = 8888,
            Version9 = 9000,
            Version9Standards = 9999,
            Version10 = 10000,
            Version10Standards = 10001,
            Version11 = 11000,
            Version11Edge = 11001
        }
        public static class WBEmulator
        {
            private const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";

            public static int GetInternetExplorerMajorVersion()
            {
                int result;

                result = 0;

                try
                {
                    RegistryKey key;

                    key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);

                    if (key != null)
                    {
                        object value;

                        value = key.GetValue("svcVersion", null) ?? key.GetValue("Version", null);

                        if (value != null)
                        {
                            string version;
                            int separator;

                            version = value.ToString();
                            separator = version.IndexOf('.');
                            if (separator != -1)
                            {
                                int.TryParse(version.Substring(0, separator), out result);
                            }
                        }
                    }
                }
                catch (SecurityException)
                {
                    logger.Error("Security exception for the Browser control for PrimeRxPay integration");
                    // The user does not have the permissions required to read from the registry key.
                }
                catch (UnauthorizedAccessException)
                {
                    logger.Error("UnauthorizedAccessException Security exception for the Browser control for PrimeRxPay integration");

                    // The user does not have the necessary registry rights.
                }

                return result;
            }
            private const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

            public static BrowserEmulationVersion GetBrowserEmulationVersion()
            {
                BrowserEmulationVersion result;

                result = BrowserEmulationVersion.Default;

                try
                {
                    RegistryKey key;

                    key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
                    if (key != null)
                    {
                        string programName;
                        object value;

                        programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                        value = key.GetValue(programName, null);

                        if (value != null)
                        {
                            result = (BrowserEmulationVersion)Convert.ToInt32(value);
                        }
                    }
                }
                catch (SecurityException)
                {
                    // The user does not have the permissions required to read from the registry key.
                }
                catch (UnauthorizedAccessException)
                {
                    // The user does not have the necessary registry rights.
                }

                return result;
            }
            public static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion)
            {
                bool result;

                result = false;

                try
                {
                    RegistryKey key;

                    key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);

                    if (key != null)
                    {
                        string programName;

                        programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

                        if (browserEmulationVersion != BrowserEmulationVersion.Default)
                        {
                            // if it's a valid value, update or create the value
                            key.SetValue(programName, (int)browserEmulationVersion, RegistryValueKind.DWord);
                        }
                        else
                        {
                            // otherwise, remove the existing value
                            key.DeleteValue(programName, false);
                        }

                        result = true;
                    }
                }
                catch (SecurityException)
                {
                    logger.Error("Security exception for the Browser control for PrimeRxPay integration");
                    // The user does not have the permissions required to read from the registry key.
                }
                catch (UnauthorizedAccessException)
                {
                    logger.Error("UnauthorizedAccessException Security exception for the Browser control for PrimeRxPay integration");
                    // The user does not have the permissions required to read from the registry key.
                }
                
                return result;
            }

            public static bool SetBrowserEmulationVersion()
            {
                int ieVersion;
                BrowserEmulationVersion emulationCode;

                ieVersion = GetInternetExplorerMajorVersion();

                if (ieVersion >= 11)
                {
                    emulationCode = BrowserEmulationVersion.Version11;
                }
                else
                {
                    switch (ieVersion)
                    {
                        case 10:
                            emulationCode = BrowserEmulationVersion.Version10;
                            break;
                        case 9:
                            emulationCode = BrowserEmulationVersion.Version9;
                            break;
                        case 8:
                            emulationCode = BrowserEmulationVersion.Version8;
                            break;
                        default:
                            emulationCode = BrowserEmulationVersion.Version7;
                            break;
                    }
                }

                return SetBrowserEmulationVersion(emulationCode);
            }
            public static bool IsBrowserEmulationSet()
            {
                return GetBrowserEmulationVersion() != BrowserEmulationVersion.Default;
            }
        }


    }
}
