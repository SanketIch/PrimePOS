using System.Collections.Generic;

namespace PrimeRxPay.RequestModels
{
    public class MultipleTransDetailRequest
    {
        public List<long> PrimerxPayTransId
        {
            get; set;
        }
        public List<string> ApplicationTransId
        {
            get; set;
        }
        public List<string> TransSetupID
        {
            get; set;
        }
        public int ApplicationId
        {
            get; set;
        }
        public int PaymentProviderID
        {
            get; set;
        }
        public int LookUpDays //PRIMEPOS-3453
        {
            get; set;
        }
    }
}
