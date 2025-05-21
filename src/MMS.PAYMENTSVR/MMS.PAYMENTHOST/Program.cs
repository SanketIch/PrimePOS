///Author : Ritesh 
///Copy Right : © Micro Merchant Systems, Inc 2008
///Functionality Desciption : The purpose of this class is to run the application PaymentHostFrm().
///External functions:None   
///Known Bugs : None
///Start Date : 22 Feb 2008.
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace MMS.PAYMENTHOST
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Process[] pArry = Process.GetProcessesByName("MMS.PAYMENTHOST");
            if (pArry.Length > 1)
                MessageBox.Show("Another Instance of Host Server is Running");
            else
                Application.Run(new PaymentHostFrm());

            
        }
    }
}