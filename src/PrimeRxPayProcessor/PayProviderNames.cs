using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeRxPay
{
    //PRIMEPOS-2902
    public class PayProvider
    {
        public int PayProviderID { get; set; }
        public string PayProviderName { get; set; }
    }
    public class HealthTest
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
    public class PayProviderNames
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        public List<PayProvider> PayProviders { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public string GetPayProviderNameAndId(string BasicUrl, string PharmacyNPI,string ClientId,string SecretKey)
        {
            string url = BasicUrl + Constant.PayProviderUrl + "PharmacyNPI=" + PharmacyNPI + "&ApplicationID=2";

            System.Net.HttpStatusCode httpCode = System.Net.HttpStatusCode.ServiceUnavailable;

            string response = Constant.SendRequestGet(url, out httpCode,ClientId,SecretKey);

            return response;
        }

        public string HealthTest(string BasicUrl, string PharmacyNPI, string PayProviderID, string ClientId, string SecretKey)
        {
            try
            {
                string url = BasicUrl + Constant.HealthTestUrl + "PharmacyNPI=" + PharmacyNPI + "&paymentProviderId=" + PayProviderID + "";

                System.Net.HttpStatusCode httpCode = System.Net.HttpStatusCode.ServiceUnavailable;

                string response = Constant.SendRequestGet(url, out httpCode, ClientId, SecretKey);

                HealthTest healthTest = JsonConvert.DeserializeObject<HealthTest>(response);

                if (healthTest.Code?.Trim() == "0")
                {
                    if (healthTest.StatusCode == 0)
                    {
                        return healthTest.Message.ToUpper();
                    }
                    else
                        return "MISSING CONFIGURATION";
                }
                return healthTest.Message;
            }
            catch (Exception ex)
            {
                logger.Error("Error in HealthTest" + ex.Message);
                return null;
            }
        }
    }
}
