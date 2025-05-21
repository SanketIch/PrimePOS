using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMile.Helpers
{
    public static class GlobalHelpers
    {
        internal static decimal StringToDecimal(string dstring)
        {
            decimal dresult = 0.00m;
            if (!string.IsNullOrEmpty(dstring))
            {
                try
                {
                    dresult=decimal.Parse(dstring, System.Globalization.CultureInfo.InstalledUICulture.NumberFormat);
                }
                catch (Exception ex)
                {
                    dresult = 0.00m;
                }
            }


            return dresult;
        }

        internal static bool StringToBool(string bstring)
        {
            bool bresult = false;

            if (!string.IsNullOrEmpty(bstring))
            {
                if (bstring.Trim() == "1")
                {
                    bresult = true;
                }
            }

            return bresult;
        }

        internal static string DecodeEntryMethod(string sEntryMethod)
        {
            string sReturn = string.Empty;
            if (!string.IsNullOrWhiteSpace(sEntryMethod))
            {
                switch (sEntryMethod.ToUpper())
                {
                    case "S":
                        sReturn = "SWIPED";
                        break;
                    case "C":
                        sReturn = "EMV CONTACT";
                        break;
                    case "RFID":
                        sReturn = "CONTACTLESS";
                        break;
                    case "M":
                        sReturn = "KEYED";
                        break;
                    default:
                        sReturn = sEntryMethod;
                        break;
                }
            }


            return sReturn;
        }


        internal static string DecodeCVM(string sCVM)
        {
            string sReturn = string.Empty;
            if (!string.IsNullOrWhiteSpace(sCVM))
            {
                switch (sCVM.ToUpper())
                {
                    case "P":
                        sReturn = "PIN VERIFIED";
                        break;
                    case "S":
                        sReturn = "SIGNATURE VERIFIED";
                        break;                   
                    case "N":
                        sReturn = "NONE";
                        break;
                    default:
                        sReturn = sCVM;
                        break;
                }
            }


            return sReturn;
        }

    }
}
