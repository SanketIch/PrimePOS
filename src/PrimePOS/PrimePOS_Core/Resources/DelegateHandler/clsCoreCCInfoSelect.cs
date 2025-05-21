using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace POS_Core.Resources.DelegateHandler
{
    public class clsCoreCCInfoSelect
    {
        public delegate DataTable coreCCInfo();
        public static coreCCInfo CCInfo;

        public delegate string coreSelectedCC();
        public static coreSelectedCC SelectedCC;

        public delegate bool coreSearchByPatientNO();
        public static coreSearchByPatientNO SearchByPatientNO;
    }
}
