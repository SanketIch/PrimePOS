//using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirstMile;
using Gateway;

namespace GatewayTestApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       // [STAThread]
        static void Main()
        {

            //GatewaySettings.AccountId = "MPTST";//"WYWVM"
            //GatewaySettings.SubId = "BILL0";//"WLISO"
            // GatewaySettings.MerchantPin = "1234567890";//"mmsdemo"
            GatewayManager.AccountId = "MPTST";
            GatewayManager.SubId = "BILL0";
            GatewayManager.MerchantPin = "1234567890";
            Gateway.GatewayManager.SelectedGateway = Gateway.Gateway.WorldPay;
            //Gateway.GatewayManager.MerchantPin = "mmsdemo";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
