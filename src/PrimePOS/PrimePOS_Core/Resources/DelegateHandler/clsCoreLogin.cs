using POS_Core.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Resources.DelegateHandler
{
    public class clsCoreLogin
    {
        public delegate bool coreloginForPreviliges(string strPrevlige, string strPermission, out string sUserID, string strWindowTitle);
        public static coreloginForPreviliges loginForPreviliges;
    }
}
