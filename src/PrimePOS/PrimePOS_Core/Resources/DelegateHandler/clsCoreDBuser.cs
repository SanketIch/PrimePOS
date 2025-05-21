using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Resources.DelegateHandler
{
    class clsCoreDBuser
    {
        public delegate string coreDeleteDBUser(string userID);
        public static coreDeleteDBUser DeleteDBUser;


        public delegate bool coreChangeDBUserPwd(string Userid, string OldPassword,string NewPassword);
        public static coreChangeDBUserPwd ChangeDBUserPwd;
    }
}
