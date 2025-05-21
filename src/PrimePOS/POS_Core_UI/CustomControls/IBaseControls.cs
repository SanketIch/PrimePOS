using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace POS_Core_UI
{
    interface IBaseControls
    {
        void setControlsValues(ref DataTable dt);
        void getControlsValues(ref DataTable dt);
    }
}
