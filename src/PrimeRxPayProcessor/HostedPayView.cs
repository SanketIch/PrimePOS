using Microsoft.Web.WebView2.Core;
﻿using Newtonsoft.Json;
using NLog;
using PrimeRxPay.ResponseModels;
using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace PrimeRxPay
{
    public partial class HostedPayView : Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private static CoreWebView2Environment _webViewEnvironment; //PRIMEPOS-3540
        private bool isClosedBtnSkipped = false; //PRIMEPOS-3533
        private string _url = string.Empty;
        private string _primeRxPayUrl = string.Empty;
        private string _transId = string.Empty;
        private string _clientId = string.Empty;
        private string _secretkey = string.Empty;
        private string _payproviderId = string.Empty;
        private string _transSetupId = string.Empty;
        private string _stationID = string.Empty; //PRIMEPOS-3540
        private string _webViewPath = string.Empty; //PRIMEPOS-3540
        public string getTransactionResponse = string.Empty;
        public HostedPayView(string Url, string TransId, string PrimeRxPayUrl, string ClientId, string Secretkey, string PayProviderID, string TransactionSetupID, string stationId)
        {
            this._url = Url;
            this._transId = TransId;
            this._primeRxPayUrl = PrimeRxPayUrl;
            this._clientId = ClientId;
            this._secretkey = Secretkey;
            this._payproviderId = PayProviderID;
            this._transSetupId = TransactionSetupID;
            this._stationID = stationId; //PRIMEPOS-3540
            this._webViewPath = $"POS.exe.WebView2/{Environment.MachineName}/{Environment.UserName}/{_stationID}"; //PRIMEPOS-3540
            InitializeComponent();
        }
        private bool FstRun = true;
        private bool WebReady = false;
        private void HostedPayView_Activated(object sender, EventArgs e)
        {
            try
            {
                if (FstRun)
                {
                    FstRun = false;
                    //InitAsync(); //PRIMEPOS-3540
                    Wait();  // wait for webview to initiaise                              
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Following Exception occured HostedPayView==> HostedPayView_Activated(){ex.ToString()}");
            }
        }

        private void HostedPayView_Load(object sender, EventArgs e)
        {
            try
            {
                if (webView != null && webView.CoreWebView2 == null)  //PRIMEPOS-3540
                {
                    InitAsync();
                }
                if (webView.CoreWebView2 != null)
                {
                    webView.CoreWebView2.Navigate(this._url);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Following Exception occured HostedPayView==> HostedPayView_Load(){ex.ToString()}");
            }
        }

        private void HostedPayView_OnClosed(object sender, FormClosedEventArgs e) //PRIMEPOS-3533
        {
            try
            {
                if(!isClosedBtnSkipped && string.IsNullOrEmpty(getTransactionResponse))
                {
                    GetTransactionDetail getTransactionDetail = new GetTransactionDetail();
                    getTransactionDetail.PayProviderResponseCode = "90";
                    getTransactionDetail.PayProviderResponseMessage = "Cancelled by user";
                    getTransactionResponse = JsonConvert.SerializeObject(getTransactionDetail);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Following Exception occured HostedPayView==> HostedPayView_Load(){ex.ToString()}");
            }
        }

        private void HostedPayView_OnClosing(object sender, FormClosingEventArgs e) //PRIMEPOS-3533
        {
            try
            {
                if(!isClosedBtnSkipped)
                {
                    var result = MessageBox.Show("Are you sure you want to close?", "PrimePOS", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Following Exception occured HostedPayView==> HostedPayView_OnClosing(){ex.ToString()}");
            }
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                webView.CoreWebView2.Navigate(this._url);
                WebReady = true;
            }
        }
        private void WebView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                string navigationUrl = Convert.ToString(webView.Source);

                if (!InternetState.IsConnectedToInternet())
                {
                    MessageBox.Show(" You are not connected to Internet ", "Connection Issue");
                    isClosedBtnSkipped = true; //PRIMEPOS-3533
                    this.Close();
                }
                logger.Trace(navigationUrl.ToString());
                var uri = new Uri(this._primeRxPayUrl);//PRIMEPOS-3032

                //PRIMEPOS-3057 add payproviderId condition
                if ((this._payproviderId != ((int)PaymentProviderEnum.HeartlandPayment).ToString()
                    && (navigationUrl.Replace("-", "").ToUpper().Contains(uri.Host.ToUpper()) || navigationUrl.Replace("-", "").ToUpper().Contains("LOGIN"))) ||
                    (this._payproviderId == ((int)PaymentProviderEnum.HeartlandPayment).ToString() && navigationUrl.Replace("-", "").ToUpper().Contains("CHARGE")) ||
                    (this._payproviderId == ((int)PaymentProviderEnum.WorldPay).ToString() && navigationUrl.Replace("-", "").ToUpper().Contains("#TOPOFPAGE"))
                    )
                {

                    logger.Trace("Entered in GetTransactionDetail DocumentComplete Method()");
                    string response = string.Empty;
                    int count = 0;
                    HttpStatusCode httpStatusCode = HttpStatusCode.ServiceUnavailable;
                    logger.Trace("HttpStatusCode = " + httpStatusCode.ToString());
                    while (httpStatusCode != System.Net.HttpStatusCode.OK)
                    {
                        logger.Trace("COUNT = " + count.ToString());
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
                            query["LookUpDays"] = "5";//PRIMEPOS-3453
                            uriBuilder.Query = query.ToString();

                            string getTransactionURL = uriBuilder.ToString();
                            response = Constant.SendRequestGet(getTransactionURL, out httpStatusCode, _clientId, _secretkey);
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
                    logger.Trace($" GetTransaction Response : {response}");
                    isClosedBtnSkipped = true; //PRIMEPOS-3533
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Following Exception occured HostedPayView==> webView_NavigationCompleted(){ex.ToString()}");
            }
        }

        private async void InitAsync()
        {
            try
            {
                if (_webViewEnvironment == null) //PRIMEPOS-3540
                {
                    _webViewEnvironment = await CoreWebView2Environment.CreateAsync(userDataFolder: _webViewPath);
                }

                if (webView.CoreWebView2 == null) //PRIMEPOS-3540
                {
                    await webView.EnsureCoreWebView2Async(_webViewEnvironment);
                }
                //await webView.EnsureCoreWebView2Async(_webViewEnvironment);
            }
            catch (AggregateException ae)
            {
                foreach (Exception exc in ae.InnerExceptions)
                {
                    logger.Error($"Following AggregateException Exception occured HostedPayView==> InitAsync(){exc.ToString()}");
                }
            }
            catch (Exception exc)
            {
                logger.Error($"Following Exception occured HostedPayView==> InitAsync(){exc.ToString()}");
            }

        }

        private void Wait()
        {
            try
            {
                while (!WebReady)
                    Application.DoEvents();
            }
            catch (Exception exc)
            {
                logger.Error($"Following Exception occured HostedPayView==> Wait(){exc.ToString()}");
            }

        }
    }
}
