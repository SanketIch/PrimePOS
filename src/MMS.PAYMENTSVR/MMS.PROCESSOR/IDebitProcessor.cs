using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.PROCESSOR
{
    public interface  IDebitProcessor:IDisposable
    {
         String Sale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
         String Void(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
         //Added By SRT(Gaurav) Date: 24-NOV-2008
         String VoidReturn(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
         //End Of Added By SRT(Gaurav)
         String Return(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
         String Reverse(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
    }
}
