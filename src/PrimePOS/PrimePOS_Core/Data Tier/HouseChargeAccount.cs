using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Data_Tier
{
    public class HouseChargeAccount
    {
        private string sAccountCode;
        private string sAccountName;
        private string sAccountAddr1;
        private string sAccountAddr2;
        private string sCity;
        private string sState;
        private string sZipCode;
        private string sPhoneNo;
        private string sMobileNo;
        private string sComment;
        private decimal dCurrentBalance = 0;


        public string AccountCode
        {
            get { return sAccountCode; }
            set { sAccountCode = value; }
        }

        public string AccountName
        {
            get { return sAccountName; }
            set { sAccountName = value; }
        }

        public string AccountAddress1
        {
            get { return sAccountAddr1; }
            set { sAccountAddr1 = value; }
        }

        public string AccountAddress2
        {
            get { return sAccountAddr2; }
            set { sAccountAddr2 = value; }
        }

        public string City
        {
            get { return sCity; }
            set { sCity = value; }
        }

        public string State
        {
            get { return sState; }
            set { sState = value; }
        }

        public string ZipCode
        {
            get { return sZipCode; }
            set { sZipCode = value; }
        }

        public string PhoneNo
        {
            get { return sPhoneNo; }
            set { sPhoneNo = value; }
        }

        public string MobileNo
        {
            get { return sMobileNo; }
            set { sMobileNo = value; }
        }

        public string Comment
        {
            get { return sComment; }
            set { sComment = value; }
        }

        public decimal CurrentBalance
        {
            get { return dCurrentBalance; }
            set { dCurrentBalance = value; }
        }



    }
}
