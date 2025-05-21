using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.PROCESSOR
{
    public interface INBSProcessor : IDisposable
    {
        String NBSSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String NBSPreReadSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String NBSReturn(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);

        String CancelTansaction(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String NBSVoid(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);  //PRIMEPOS-3373
    }
}