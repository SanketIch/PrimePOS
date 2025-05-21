using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    class ProcessACH
    {
        #region "Private Fields"

        private string _PayorName;
        private string _RoutingNo;
        private string _CheckNo;
        private string _AccountNo;

       


        


        #endregion "Private Fields"


        public string PayorName
        {
            get { return _PayorName; }
            set { _PayorName = value; }
        }


        public string RoutingNo
        {
            get { return _RoutingNo; }
            set { _RoutingNo = value; }
        }

        public string CheckNo
        {
            get { return _CheckNo; }
            set { _CheckNo = value; }
        }

        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }


    }
}
