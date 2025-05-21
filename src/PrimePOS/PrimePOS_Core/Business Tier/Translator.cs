using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
//using POS.UI;
using System.Runtime.Serialization;
using System.Web;
using POS_Core.Resources.DelegateHandler;
using Newtonsoft.Json;
using System.Net.Http;

namespace POS.BusinessTier
{
    [DataContract]
    public class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    public class AdmAuthentication
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientId;
        private string clientSecret;
        private string request;
        private AdmAccessToken token;
        private Timer accessTokenRenewer;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        public AdmAuthentication(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            this.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret));
            this.token = HttpPost(DatamarketAccessUri, this.request);
            //renew the token every specfied minutes
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }

        public AdmAccessToken GetAccessToken()
        {
            return this.token;
        }


        private void RenewAccessToken()
        {
            AdmAccessToken newAccessToken = HttpPost(DatamarketAccessUri, this.request);
            //swap the new token with old one
            //Note: the swap is thread unsafe
            this.token = newAccessToken;
            Console.WriteLine(string.Format("Renewed token for user: {0} is: {1}", this.clientId, this.token.access_token));
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        private AdmAccessToken HttpPost(string DatamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(DatamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }

    public class Translator
    {
        private AdmAuthentication _auth;
        public string ClientSecret = "HztmdXtQhUWXr/A0h/iergcimzTZTTo5tMJrxaL0EZg=";
        public string ClientID = "PrimePOS";

        private void SetNew()
        {
            try
            {
                _auth = new AdmAuthentication(ClientID, ClientSecret);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public string Translate(string text, string from, string to)
        //{
        //    string authToken;
        //    string uri;
        //    try
        //    {
        //        SetNew();
        //        uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
        //        authToken = "Bearer" + " " + _auth.GetAccessToken().access_token;

        //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
        //        httpWebRequest.Headers.Add("Authorization", authToken);

        //        WebResponse response = null;

        //        response = httpWebRequest.GetResponse();
        //        using (Stream stream = response.GetResponseStream())
        //        {
        //            System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
        //            string translation = (string)dcs.ReadObject(stream);
        //            Console.WriteLine("Translation for source text '{0}' from {1} to {2} is", text, from, to);
        //            return translation;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsCoreUIHelper.ShowErrorMsg(ex.Message);
        //        return String.Empty;
        //    }
        //}

        //PRIMEPOS-3164 01-Nov-2022 JY Modified
        public async Task<string> Translate(string text, string from, string to)
        {
            string translation = string.Empty;

            try
            {
                string route = POS_Core.Resources.Configuration.CSetting.TranslatorAPIRoute + "&from=" + from + "&to=" + to;
                string uri = POS_Core.Resources.Configuration.CSetting.TranslatorAPIEndPoint + route;
                object[] body = new object[] { new { Text = text } };
                var requestBody = JsonConvert.SerializeObject(body);

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(uri);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", POS_Core.Resources.Configuration.CSetting.TranslatorAPIkey);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", POS_Core.Resources.Configuration.CSetting.TranslatorAPILocation);
                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                    string ResponseBody = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<List<Dictionary<string, List<Dictionary<string, string>>>>>(ResponseBody);
                    translation = result[0]["translations"][0]["text"];
                }
            }
            catch (Exception ex)
            {
                clsCoreUIHelper.ShowErrorMsg(ex.Message);
                return String.Empty;
            }
            return translation;
        }
    }
}
