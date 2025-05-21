//Author: Manoj Kumar
//Micro Merchant Systems
//Date: 8/25/2011
//No Bugs

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace POS_Core_UI.Resources
{
    public class XchargeWin
    {
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);// Class Name and Window Name

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(int hWnd);// Get the window handle
    }
}
