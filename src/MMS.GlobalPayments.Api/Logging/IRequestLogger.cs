using System;

namespace MMS.GlobalPayments.Api.Logging {
    public interface IRequestLogger {
        void RequestSent(string request);
        void ResponseReceived(string response);
    }
}
