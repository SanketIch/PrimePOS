using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;

namespace POS_Core_UI
{
    interface IScheduledControl
    {
        Object getObject();
        void SetObject(Object obj);
        Control getControl();
        void save(int itaskiD);
        void SetFocusControl();
        bool checkValidation();
    }
}
