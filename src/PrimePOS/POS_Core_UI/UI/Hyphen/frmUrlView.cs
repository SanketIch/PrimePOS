using Microsoft.Web.WebView2.Core;
using NLog;
using POS_Core.Resources;
using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmUrlView : Form
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private static CoreWebView2Environment _webViewEnvironment; //PRIMEPOS-3540
        public string _url = string.Empty;
        public static bool isUserClosed = true; //PRIMEPOS-3207 New

        public frmUrlView(string Url)
        {
            this._url = Url;
            InitializeComponent();
        }
        private bool FstRun = true;
        private bool WebReady = false;
        private void frmUrlView_Activated(object sender, EventArgs e)
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
                logger.Error($"Following Exception occured frmUrlView==> frmUrlView_Activated(){ex}");
            }
        }

        private void frmUrlView_Load(object sender, EventArgs e)
        {
            try
            {
                if (webView2 != null && webView2.CoreWebView2 == null) //PRIMEPOS-3540
                {
                    InitAsync();
                }
                if (webView2.CoreWebView2 != null)
                {
                    webView2.CoreWebView2.Navigate(this._url);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Following Exception occured frmUrlView==> frmUrlView_Load(){ex}");
            }
        }
        private void WebView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                webView2.CoreWebView2.Navigate(this._url); //PRIMEPOS-3540
                WebReady = true;
            }
        }
        private void WebView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }
        private async void InitAsync()
        {
            try
            {
                if (_webViewEnvironment == null) //PRIMEPOS-3540
                {
                    _webViewEnvironment = await CoreWebView2Environment.CreateAsync(userDataFolder: Configuration.WebViewPath);
                }

                if (webView2.CoreWebView2 == null) //PRIMEPOS-3540
                {
                    await webView2.EnsureCoreWebView2Async(_webViewEnvironment);
                }
                //await webView2.EnsureCoreWebView2Async(_webViewEnvironment);
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
        private void FrmUrlView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason.Equals(CloseReason.UserClosing))
            {
                if (isUserClosed)
                {
                    if (MessageBox.Show("Do you want to exit Hyphen Interface?", "Confirm Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    isUserClosed = true;
                }
            }
        }
    }
}
