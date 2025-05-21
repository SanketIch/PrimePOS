using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum HttpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

namespace Solutran
{
    public class RestClientHelper
    {

        private static Logger errorLogs = LogManager.GetCurrentClassLogger();
        bool isSecured = false;
        Dictionary<string, string> headers;

        public RestClientHelper()
        {

        }
        public RestClientHelper(bool isSecured)
        {
            this.isSecured = isSecured;
        }
        public RestClientHelper(Dictionary<string, string> headerDictionary, bool isSecured)
        {
            headers = new Dictionary<string, string>(headerDictionary);
            this.isSecured = isSecured;
        }

        public string MakeRequest(string baseUrl, string endpoint, string method, string postData, string authorization,  string encodingType = "")
        {
            headers = new Dictionary<string, string>();
            headers["authorization"] = authorization;
            headers["cache-control"] = JsonServiceInfo.CacheControl;
           // headers["content-type"] = JsonServiceInfo.ContentType;

            var responseValue = string.Empty;
            try
            {
                //if (isSecured == true)
                //{
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;    //PRIMEPOS-3179 27-Jan-2023 JY Commented
                // }

                var request = (HttpWebRequest)WebRequest.Create(baseUrl + endpoint);
                request.Method = method.ToString();

                request.ContentType = "application/json";

                if (headers != null)
                {
                    AddHeader(request);
                }
                byte[] byteArray;
                if (encodingType == "ascii")
                {
                    //Ritesh
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byteArray = encoding.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    request.PreAuthenticate = true;
                }
                else
                {
                    byteArray = Encoding.UTF8.GetBytes(Regex.Unescape(postData));
                }

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            var message = string.Format("Request failed. Received HTTP {0}", response.StatusCode);
                            errorLogs.Trace(DateTime.Now.ToString() + ": " + message);
                        }
                        using (Stream dataStreamResponse = response.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                responseValue = sResponseFromServer;
                            }
                        }
                    }
                }
            }

            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    responseValue = reader.ReadToEnd();
                    errorLogs.Trace("Response Value : " + responseValue);
                    responseValue = null;
                    // PDI-82 updated by Farman Ansari on 11 Oct 2018.
                    errorLogs.Trace(string.Format("Exception Message:{0}{1}Response Code:{2}{1}Stack Trace:{3}{1}", webex.Message, Environment.NewLine, errResp.ToString(), webex.StackTrace));

                }
            }
            return responseValue;
        }

        public void AddHeader(HttpWebRequest req)
        {
            try
            {
                foreach (KeyValuePair<string, string> headerDict in headers)
                {
                    if (WebHeaderCollection.IsRestricted(headerDict.Key))
                    {
                        string restrictedHeader = headerDict.Key.ToString();
                        switch (restrictedHeader)
                        {
                            case "Accept":
                                req.Accept = headerDict.Value.ToString();
                                break;
                        }
                    }
                    else
                    {
                        req.Headers.Add(headerDict.Key.ToString(), headerDict.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
