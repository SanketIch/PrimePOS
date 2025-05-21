using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.TransType
{
    public class POSEnum
    {
    }

    public enum POSTransactionType
    {
        Sales = 1,
        SalesReturn = 2,
        ReceiveOnAccount = 3,
        Void = 4,
        VoidReturn = 5,
        Reverse = 6,
        PreRead = 7, //PRIMEPOS-3372
        Cancel = 8, //PRIMEPOS-3372
        PreReadSale = 9, //PRIMEPOS-3526 //PRIMEPOS-3504
        PreReadSaleReturn = 10 //PRIMEPOS-3526 //PRIMEPOS-3504
    }


}
