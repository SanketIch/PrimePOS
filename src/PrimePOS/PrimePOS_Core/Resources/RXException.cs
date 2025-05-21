using System;
using System.Collections.Generic;
using System.Text;

namespace POS_Core.Resources
{
    public class RXException:Exception
    {

        private Int64 transID = 0;
        private bool displayYesNoButonOnly = false;
        public Int64 TransID
        {
            get { return transID; }
            set { transID = value; }
        }

        public bool DisplayYesNoButonOnly
        {
            get { return displayYesNoButonOnly; }
            set { displayYesNoButonOnly = value; }
        }

        public RXException(string message)
            : base(message)
        {
        }
    }
}
