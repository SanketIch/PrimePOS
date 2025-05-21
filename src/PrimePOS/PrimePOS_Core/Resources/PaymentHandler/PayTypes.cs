using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Resources
{
    public class PayTypes
    {
        public const string CreditCard = "CC";
        public const string Cash = "CA";
        public const string Cheque = "CH";
        public const string Misc = "MI";
        public const string CancelTrans = "CT";
        public const string DebitCard = "DB"; //Added By Dharmendra(SRT) ON 26-08-08
        public const string EBT = "BT";
        public const string NBS = "NB"; //PRIMEPOS-3372
    }
}
