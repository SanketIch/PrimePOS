using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.CommonData;

namespace POS_Core.Resources.DelegateHandler
{
    public class clsCorePrimeRXHelper
    {
        public delegate bool CoreImportPatientAsCustomer(int patientno);
        public static CoreImportPatientAsCustomer ImportPatientAsCustomer;


        public delegate string CoreGetPatientDeliveryAddress(string patient);
        public static CoreGetPatientDeliveryAddress GetPatientDeliveryAddress;

        public delegate string CoreVarifyChargeAlreadyPosted(TransDetailRXData oTransRXDData, TransDetailData oTransDData, ref DataSet oDS);    //PRIMEPOS-3015 26-Oct-2021 JY Added oDS
        public static CoreVarifyChargeAlreadyPosted VarifyChargeAlreadyPosted;
    }
}
