using NLog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vantiv.RequestHeaderFiles;

namespace Vantiv
{
    public static class Constant
    {
        static ILogger logger = LogManager.GetCurrentClassLogger();


        public const string Idle = "/idle";

        public const string Sale = "/sale";

        public const string Display = "/display";

        public const string ScrollingDisplay = "/scrollingdisplay";

        public const string Signature = "/signature";

        public const string Selection = "/selection";

        public const string Space = "%20";

        public const string NewLine = "%0A";

        public const string Void = "/void";

        public const string EBTvoucher = "/ebtvoucher";

        public const string Refund = "/refund";

        public const string Json = "application/json";

        public const string Description = "Description";

        public const string Unit_Price = "Unit-Price";

        public const string Quantity = "Qty";

        public const string Discount = "Discount";

        public const string Total = "Total";

        public const string Success = "SUCCESS";

        public const string Approved = "APPROVED";

        public const string TriPosConfigFilePath = @"C:\Program Files (x86)\Vantiv\TriPOS Service\TriPOS.config";//PRIMEPOS-2895 Arvind

        public const string Return = "/return";

        public const string Reversal = "/reversal";

        public const string Cancel = "/cancel"; //PRIMEPOS-3372
        public const string Never = "Never"; //PRIMEPOS-3372

        #region Network
        //Always have HttpClient in static mode
        private static object _locker = new object();
        private static volatile HttpClient _client;

        private static HttpClient httpClient
        {
            get
            {
                if (_client == null)
                {
                    lock (_locker)
                    {
                        if (_client == null)
                        {
                            _client = new HttpClient();
                        }
                    }
                }
                return _client;
            }
        }
        #endregion

        /// <summary>
        /// This Method is used for Sending Get Request to the HttpClient
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postBody"></param>
        /// <returns></returns>
        /// <summary>
        /// This Method is used for Sending Get Request to the HttpClient
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postBody"></param>
        /// <returns></returns>
        public static string SendRequestGet(string url, string developerKey, string developerSecret)
        {
            logger.Debug("Request is : " + url);

            logger.Trace(" ENTERED IN SENDREQUESTGET METHOD ");

            string actualResponse = string.Empty;
            HttpResponseMessage respMessage = null;

            try
            {

                var message = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                AuthorizationHeader authorizationHeader = AuthorizationHeader.Create(message.Headers, message.RequestUri, "", message.Method.Method, "1.0", "TP-HMAC-SHA1", Guid.NewGuid().ToString(), DateTime.UtcNow.ToString("O"), developerKey, developerSecret);
                message.Headers.Add("tp-authorization", authorizationHeader.ToString());
                message.Headers.Add("tp-application-id", "10443");
                message.Headers.Add("tp-application-name", "PRIMEPOS");
                message.Headers.Add("tp-application-version", "1.0");
                message.Headers.Add("tp-request-id", Guid.NewGuid().ToString());
                Task<HttpResponseMessage> response = httpClient.SendAsync(message);
                response.Wait();
                respMessage = response.Result;
                Task<string> readAsync = respMessage.Content.ReadAsStringAsync();
                readAsync.Wait();
                actualResponse = readAsync.Result;
                logger.Trace("MEssage headers are : " + message.Headers.ToString());
            }
            catch (Exception ex)
            {
                logger.Fatal("ERROR SENDING DATA TO DEVICE : ", ex.ToString());
                throw ex;
            }
            logger.Debug("Response is : " + actualResponse);

            return actualResponse;
        }

        /// <summary>
        /// This Method is used for Sending Post Request to the HttpClient
        /// </summary>
        /// <param name="postBody"></param>
        /// <returns></returns>
        public static string SendRequestPost(string postBody, string Url, string developerKey, string developerSecret, string ApplicationName)
        {
            logger.Debug("Request is : " + postBody);

            logger.Trace(" ENTERED IN SENDREQUESTPOST METHOD ");

            string actualResponse = string.Empty;
            HttpResponseMessage respMessage = null;

            try
            {
                var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Url));
                AuthorizationHeader authorizationHeader = AuthorizationHeader.Create(message.Headers, message.RequestUri, postBody, message.Method.Method, "1.0", "TP-HMAC-SHA1", Guid.NewGuid().ToString(), DateTime.UtcNow.ToString("O"), developerKey, developerSecret);
                message.Headers.Add("tp-authorization", authorizationHeader.ToString());
                message.Headers.Add("tp-application-id", "10443");
                message.Headers.Add("tp-application-name", ApplicationName);
                message.Headers.Add("tp-application-version", "1.0");
                message.Headers.Add("tp-request-id", Guid.NewGuid().ToString());
                message.Content = new StringContent(postBody, Encoding.UTF8, Constant.Json);
                logger.Trace("MEssage headers are : " + message.Headers.ToString());
                Task<HttpResponseMessage> response = httpClient.SendAsync(message);
                response.Wait();
                respMessage = response.Result;
                Task<string> readAsync = respMessage.Content.ReadAsStringAsync();
                readAsync.Wait();
                actualResponse = readAsync.Result;
            }
            catch (Exception ex)
            {
                logger.Fatal("ERROR SENDING DATA TO DEVICE : ", ex.ToString());
                throw ex;
            }
            logger.Debug("Response is : " + actualResponse);

            return actualResponse;
        }
    }
}
