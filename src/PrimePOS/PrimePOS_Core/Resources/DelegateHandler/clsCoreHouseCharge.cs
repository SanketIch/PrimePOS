using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.Data_Tier;
using POS_Core.CommonData;

namespace POS_Core.Resources.DelegateHandler
{
    public class clsCoreHouseCharge
    {
        public delegate void CorepostHouseCharge(TransDetailData oTransDData, POSTransPaymentData oTransPData, TransType.POSTransactionType Sales,Decimal InvDiscPerc, CommonData.Rows.POSTransPaymentRow orow, System.Int32 TransID);
        public static CorepostHouseCharge postHouseCharge;

        public delegate DataSet CoreGetPatientByChargeAccountNumber(long acctno);
        public static CoreGetPatientByChargeAccountNumber GetPatientByChargeAccountNumber;

        public delegate void CorepostROA(System.Int64 AccountNo, POSTransPaymentData oTransPData, TransType.POSTransactionType TransType);
        public static CorepostROA postROA;


        public delegate HouseChargeAccount CoreGetAccountInformation(string AccCode);
        public static CoreGetAccountInformation GetAccountInformation;

        public delegate DataSet CoregetHouseChargeInfo(string SearchCode, string SearchName);
        public static CoregetHouseChargeInfo getHouseChargeInfo;
    }
}
