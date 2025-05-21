using NLog;
using System;
using System.Net.Http;
using NLog;
using System.Net;

namespace NBS
{
    public static class NBSHelper
    {
        static ILogger logger = LogManager.GetCurrentClassLogger();
        public static string CurrencyCode = "USD";
        public static string trackOneId = "trackOneId";
        public static string NBSAnalyze = "Analyze";
        public static string NBSRedeem = "Redeem";
        public static string NBSReturn = "Return";
        public static string NBSReversal = "Reversal";
        public static string GetNbsBinData = "GetNBSBinsData";
        public static string ApplicationID = "PRIMEPOS"; //PRIMEPOS-3412
        public static string GetToken = "auth/generateAccessToken"; //PRIMEPOS-3412
        public static string NBSVoid = "Void";  //PRIMEPOS-3373
        private static readonly object _locker = new object();
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

        public static string SendRequestGet(string url, out System.Net.HttpStatusCode statusCode, string bearerToken)
        {
            logger.Debug("NBSHelper==>SendRequestGet()==> Request is : " + url);
            logger.Trace("NBSHelper==>SendRequestGet()==> ENTERED IN NBS SENDREQUESTGET METHOD ");

            string actualResponse = string.Empty;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var response = httpClient.SendAsync(request).Result;
                statusCode = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    actualResponse = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    logger.Warn("NBSHelper==>SendRequestGet()==> ERROR IS " + response.StatusCode.ToString());
                    actualResponse = string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSHelper==>SendRequestGet()==> ERROR SENDING DATA : ", ex.ToString());
                actualResponse = string.Empty;
                statusCode = System.Net.HttpStatusCode.ExpectationFailed;
            }
            logger.Debug("NBSHelper==>SendRequestGet()==> response is : " + actualResponse);
            return actualResponse;
        }

        public static string SendRequestPost(string postBody, string Url, string bearerToken)
        {
            logger.Debug("NBSHelper==>SendRequestPost()==> Request is : " + postBody);
            logger.Trace("NBSHelper==>SendRequestPost()==> ENTERED IN SENDREQUESTPOST METHOD ");
            string actualResponse = string.Empty;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(Url));
                request.Content = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                var response = httpClient.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    actualResponse = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    logger.Warn("NBSHelper==>SendRequestPost()==> ERROR IS " + response.StatusCode.ToString());
                    actualResponse = string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSHelper==>SendRequestPost()==> error sending data : ", ex.ToString());
                actualResponse = String.Empty;
            }
            logger.Debug("NBSHelper==>SendRequestPost()==> Response is : " + actualResponse);
            return actualResponse;
        }

        public static string SendRequestGetToken(string postBody, string Url)
        {
            logger.Debug("NBSHelper==>SendRequestPostWithoutToken()==> Request is : " + postBody);
            logger.Trace("NBSHelper==>SendRequestPostWithoutToken()==> ENTERED IN SendRequestPostWithoutToken METHOD ");
            string actualResponse = string.Empty;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(Url));
                request.Content = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        actualResponse = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        logger.Warn("NBSHelper==>SendRequestPostWithoutToken()==> ERROR IS " + response.StatusCode.ToString());
                        actualResponse = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSHelper==>SendRequestPostWithoutToken()==> error sending data : " + ex.ToString());
                actualResponse = String.Empty;
            }

            logger.Debug("NBSHelper==>SendRequestPostWithoutToken()==> Response is : " + actualResponse);
            return actualResponse;
        }

        #region PRIMEPOS-3479 AND PRIMEPOS-3480
        public static string GetApprovalDescription(int code)
        {
            switch (code)
            {
                case 200:
                    return "Transaction entirely declined due to unauthorized products in the basket";
                case 201:
                    return "Transaction entirely declined due to card reported lost or stolen";
                case 202:
                    return "Transaction entirely declined due to card not activated";
                case 203:
                    return "Transaction entirely declined due to no funds in the purses";
                case 204:
                    return "Transaction entirely declined due to other membership issues. contact nations for more details";
                case 205:
                    return "Transaction entirely declined due to purchase at an unauthorized store location or online retailer";
                case 206:
                    return "Transaction entirely declined due to suspected fraud";
                default:
                    return "Transaction declined";
            }
        }
        #endregion
    }
}
