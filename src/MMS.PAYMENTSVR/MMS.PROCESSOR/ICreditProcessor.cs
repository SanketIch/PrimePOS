using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.PROCESSOR
{
    public interface ICreditProcessor:IDisposable
    {
        String Sale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String PreRead(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp); //PRIMEPOS-3526
        String PreReadSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp); //PRIMEPOS-3526
        String PreReadCancel(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp); //PRIMEPOS-3526
        String PreReadReturn(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp); //PRIMEPOS-352
        String VoidSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String Credit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String VoidCredit(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String PreAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String PostAuth(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String Reverse(ref MMSDictionary<String, String> requsetMsgKeys, out PaymentResponse pmtResp);
    }
}
