//Author: Manoj Kumar Balkaran
//Functions: This is use for EBT Sales and Return.
//Known Bug: None
//Date : 08/15/2010

using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.PROCESSOR
{
    public interface  IEbtProcessor:IDisposable
    {
        String EBTSale(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        //String EBTVoid(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        //String EBTVoidReturn(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
        String EBTReturn(ref MMSDictionary<String, String> requestMsgKeys, out PaymentResponse pmtResp);
    }
}