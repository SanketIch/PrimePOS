using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MMS.PAYMENTHOST
{
    public class PaymentClient
    {
        public Socket ClientSocket = null;
        public String ReqMessage = null;
        public PaymentClient(Socket socket,String message)
        {
            ClientSocket = socket;
            ReqMessage = message;
        }
        
    }
}
