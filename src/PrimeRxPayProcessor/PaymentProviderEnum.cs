using System.ComponentModel;

namespace PrimeRxPay
{
    //PRIMEPOS-3057 New file
    public enum PaymentProviderEnum
    {
        [Description("Vantiv")]
        WorldPay = 1,
        [Description("Elavon")]
        ElavonPayment = 2,
        [Description("HPS")]
        HeartlandPayment = 3
    }
}
