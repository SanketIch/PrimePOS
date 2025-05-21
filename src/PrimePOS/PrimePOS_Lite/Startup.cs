using POS_Core_UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    static class Startup
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            //System.Diagnostics.Debugger.Break();
            //System.Windows.Forms.MessageBox.Show("Hello");
            ////string strArg= "St=01-IsExe=true-IsPOSLite=true-Items=10026rx-Username=POS-password=POS"; // Old
            //if (args.Length > 1)  //PRIMEPOS-2949 12-Mar-2021 JY Commented as this condition never executes
            if (args.Length > 0)    //PRIMEPOS-2949 12-Mar-2021 JY Added
            {
                if (args.Length > 1 && args[1].Trim().ToUpper() == "ScheduledTaskExecute".ToUpper())
                {
                }
                else
                {
                    string strArg = string.Empty;
                    foreach (string arg in args)
                    {
                        strArg = strArg + " " + arg;
                    }

                    // string strArg = "St=01-IsPOSLite=true-Items=7412322rx,7412321rx-Username=MMS-password=asmms$";
                    string stationCode = string.Empty, Items = string.Empty, Username = string.Empty, password = string.Empty;
                    bool IsPOSLite = false;
                    var argument = strArg.Trim();
                    var options = argument.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string opt in options)
                    {
                        string[] arg = opt.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arg.Length == 1) stationCode = arg[0].Trim();    //PRIMEPOS-2949 12-Mar-2021 JY Added
                        switch (arg[0])
                        {
                            case "St":
                                {
                                    stationCode = arg[1].Trim();
                                    break;
                                }
                            case "IsPOSLite":
                                {
                                    IsPOSLite = Convert.ToBoolean(arg[1].Trim());
                                    break;
                                }
                            case "Items":
                                {
                                    Items = arg[1].Trim();
                                    break;
                                }
                            case "Username":
                                {
                                    Username = arg[1].Trim();
                                    break;
                                }
                            case "password":
                                {
                                    password = arg[1].Trim();
                                    break;
                                }

                        }
                    }
                    try
                    {
                        args = new string[options.Length];
                        args[Convert.ToInt32(clsMain.EnumArgs.Station)] = stationCode;
                        args[Convert.ToInt32(clsMain.EnumArgs.IsPOSLite)] = IsPOSLite.ToString();
                        args[Convert.ToInt32(clsMain.EnumArgs.RXNumbers)] = Items;
                        args[Convert.ToInt32(clsMain.EnumArgs.username)] = Username;
                        args[Convert.ToInt32(clsMain.EnumArgs.password)] = password;
                    }
                    catch { }
                }
            }
            clsMain.Main(args);
        }
    }
}
