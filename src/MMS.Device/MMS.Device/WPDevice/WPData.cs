using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMS.Device.WPDevice
{
    public class WPData
    {
        public struct WPAccountInfo
        {
            public string AccountID;
            public string SubID;
            public string MerchantPIN;
        }
        public enum PayTypes
        {
            Credit,
            Debit,
            EBT_CashBenifit_Sale
        }

        public enum TransTypes
        {
            Sale,
            Credit,
            Void,
            BalanceInquery
        }
        
        public struct Payment
        {
            public struct amount
            {
                public double TotalAmount;
                public struct fsa
                {
                    public double RxAmount;
                    public double TotalHealthCareAmount;
                }
                public fsa FSA;
            }
            public amount Amount;

            public PayTypes PayType { get; set; }
            public TransTypes TransType { get; set; }

            public struct reversal
            {
                public string HistoryID;
                public string OrderID;
            }
            public reversal Return;
            public reversal Void;

            public struct message
            {
                public string SignMessage;
                public string PaymentMessage;
            }
            public message Message;
        }

    }
}
