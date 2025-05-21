using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay
{
    public static class Constant
    {
        static ILogger logger = LogManager.GetCurrentClassLogger();
        //public static string SaleUrl = "worldpay/SetupSaleTransaction";
        //public static string ReturnUrl = "worldpay/CreditCardReturn";
        //public static string ReversalUrl = "worldpay/CreditCardReversal";
        //public static string GetTransactionStatusUrl = "worldpay/TransactionDetail";
        //public static string VoidUrl = "worldpay/CreditCardVoid";
        //public static string CreditCardSaleUrl = "worldpay/CreditCardSale";//PRIMEPOS-TOKENSALE
        ////PRIMEPOS-2902 
        //public static string HealthTestUrl = "worldpay/HealthTest?";
        //public static string PayProviderUrl = "worldpay/GetPayProviders?";
        public static string SaleUrl = "/SetupSaleTransaction";
        public static string ReturnUrl = "/CreditCardReturn";
        public static string ReversalUrl = "/CreditCardReversal";
        public static string GetTransactionStatusUrl = "/TransactionDetail";
        public static string GetMultipleTransactionStatusUrl = "/MultipleTransactionDetail"; //PRIMEPOS-3333
        public static string VoidUrl = "/CreditCardVoid";
        public static string CreditCardSaleUrl = "/CreditCardSale";//PRIMEPOS-TOKENSALE
        //PRIMEPOS-2902 
        public static string HealthTestUrl = "/HealthTest?";
        public static string PayProviderUrl = "/GetPayProviders?";
        public static string ResendPayment = "/ResendPayment";
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
        //
        //
        public static string SendRequestGet(string url, out System.Net.HttpStatusCode statusCode, string clientId, string secretKey)
        {
            string newPostBody = url.Replace(clientId, "*****");
            newPostBody = newPostBody.Replace(secretKey, "*****");
            logger.Debug("Request is : " + newPostBody);
            logger.Trace(url);

            logger.Trace(" ENTERED IN SENDREQUESTGET METHOD ");

            string actualResponse = string.Empty;
            HttpResponseMessage respMessage = null;

            try
            {
                var message = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                message.Headers.Add("client_id", clientId);
                message.Headers.Add("secret_key", secretKey);
                Task<HttpResponseMessage> response = httpClient.SendAsync(message);
                response.Wait();
                respMessage = response.Result;
                statusCode = respMessage.StatusCode;
                if (respMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Task<string> readAsync = respMessage.Content.ReadAsStringAsync();
                    readAsync.Wait();
                    actualResponse = readAsync.Result;
                }
                else
                {
                    logger.Debug(" ERROR IS " + respMessage.StatusCode.ToString());
                    System.Windows.Forms.MessageBox.Show(respMessage.StatusCode.ToString());
                    actualResponse = string.Empty;
                }
                //logger.Trace("MEssage headers are : " + message.Headers.ToString());

            }
            catch (Exception ex)
            {
                logger.Fatal("ERROR SENDING DATA : ", ex.ToString());
                actualResponse = string.Empty;
                statusCode = System.Net.HttpStatusCode.ExpectationFailed;
                //throw ex;
            }
            logger.Debug("Response is : " + actualResponse);

            return actualResponse;
        }
        public static string SendRequestPost(string postBody, string Url, string ClientID, string SecretKey)
        {
            string newPostBody = postBody.Replace(ClientID, "*****");
            newPostBody = newPostBody.Replace(SecretKey, "*****");
            logger.Debug("Request is : " + newPostBody);

            logger.Trace(" ENTERED IN SENDREQUESTPOST METHOD ");

            string actualResponse = string.Empty;
            HttpResponseMessage respMessage = null;

            try
            {

                var message = new HttpRequestMessage(HttpMethod.Post, new Uri(Url));
                message.Content = new StringContent(postBody, System.Text.Encoding.UTF8, "application/json");
                //                client_id: asMMS203ePO$
                //secret_key: asMMS203ePO$
                message.Headers.Add("client_id", ClientID);
                message.Headers.Add("secret_key", SecretKey);
                //logger.Trace("MEssage headers are : " + message.Headers.ToString());
                Task<HttpResponseMessage> response = httpClient.SendAsync(message);
                response.Wait();
                respMessage = response.Result;
                if (respMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Task<string> readAsync = respMessage.Content.ReadAsStringAsync();
                    readAsync.Wait();
                    actualResponse = readAsync.Result;
                }
                else
                {
                    logger.Debug(" ERROR IS " + respMessage.StatusCode.ToString());
                    System.Windows.Forms.MessageBox.Show(respMessage.StatusCode.ToString());
                    actualResponse = string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ERROR SENDING DATA : ", ex.ToString());
                actualResponse = String.Empty;
                return actualResponse;
            }
            logger.Debug("Response is : " + actualResponse);

            return actualResponse;
        }
    }
}
