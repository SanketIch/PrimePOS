using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MMS.Device.WPDevice
{
    public static class Shared
    {
        public static event Action<bool> ShowRx;
        public static void ShowRXDetail()
        {
            if (ShowRx != null)
            {
                ShowRx(true);
            }
        }
    }
}
