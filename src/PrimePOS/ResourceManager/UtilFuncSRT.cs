using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;
using System.Diagnostics;

namespace MMSUtil
{
    public class UtilFuncSRT
    {
        private const int PHONENUMBER = 10;

        public static DateTime ValorZeroDT(string strDate)
        {
            DateTime minValue = DateTime.MinValue;
            try
            {
                DateTime.TryParse(strDate, out minValue);
            }
            catch (Exception ex)
            {
                LogData.Write(ex.ToString(), LogData.ExceptionLog, 0, 1, TraceEventType.Critical);
            }
            return minValue;
        }

        public static string FormatPhoneNum(string phoneNum)
        {
            string result = string.Empty;
            try
            {
                phoneNum = phoneNum.Trim();
                if (phoneNum != null)
                {
                    phoneNum = phoneNum.PadLeft(10, '0');
                }
                else
                {
                    phoneNum = "0000000000";
                }
                result = phoneNum.Insert(3, "-");
            }
            catch (Exception ex)
            {
                LogData.Write(ex.ToString(), LogData.ExceptionLog, 0, 1, TraceEventType.Critical);
            }
            return result;
        }
    }
}
