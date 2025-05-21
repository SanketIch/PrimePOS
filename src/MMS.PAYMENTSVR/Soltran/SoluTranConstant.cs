using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutran
{
    public static class SoluTranConstant
    {

    }


    internal class MessageType
    {
        public const string AuthMessType = "01";
        public const string CancelMessType = "02";
        public const string ConfirmMessType = "03";
        public const string BalanceMessType = "07";
        public const string VoidMessType = "08";
        public const string RefundMessType = "18";
    }

    internal class JsonServiceInfo
    {
        public const string BaseURL = "https://s3globaltest.solutranservices.com/MessagingServicev4/S3Auth.svc";
        public const string PostMethod = "POST";
        public const string GetMethod = "GET";
        public const string Authorization = "apikey F93F052E-E202-4427-AE03-FAD3D5F8D36D";
        public const string CacheControl = "no-cache";
        public const string ContentType = "application/json";
    }

    internal class SolutranProperties
    {
        public const string VersionType = "V04";
        public const string CurrencyCode = "USD";
        public const string POSDataCode = "31";
    }

}
