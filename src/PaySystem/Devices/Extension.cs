using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NLog;

namespace EDevice
{
    /// <summary>
    /// Extension class for some common functions
    /// </summary>
    internal static  class Extension
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Byte to String Conversion
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string ByteToString(this byte[] data)
        {
            logger.Trace("In ByteToString()");

            StringBuilder strHex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
            {
                strHex.AppendFormat("{0:x2}", b);
            }
            return strHex.ToString();
        }

        /// <summary>
        /// String to Byte Conversion
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static byte[] ToByte(this string str)
        {

            logger.Trace("IN ToByte()");

            byte[] bytes = new byte[str.Length];
            bytes = System.Text.Encoding.ASCII.GetBytes(str);
            return bytes;
        }

        /// <summary>
        /// Bytes to String
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string FromByte(this byte[] data)
        {
            logger.Trace("In FromByte()");

            string result = System.Text.Encoding.ASCII.GetString(data);
            return result;
        }
        /// <summary>
        /// Check if a String is a Number
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static bool IsNumeric(this string str)
        {
            logger.Trace("In IsNumeric()");
            long val;
            return long.TryParse(str, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out val);
        }

        internal static string HexToAscii(this string str)
        {
            logger.Trace("In HexToAscii()");

            string Ascii = string.Empty;
            string[] newStr = new string[(str.Length) + (str.Length % 2 == 0 ? 0 : 1)];
            for (int i = 0; i < newStr.Length; i = i + 2)
            {
                newStr[i] = str.Substring(i, i + 2 > str.Length ? 1 : 2);
                newStr[i] = ToChar(newStr[i]);
            }       
            return string.Join("", newStr);
        }

        internal static string ToChar(this string str)
        {
            logger.Trace("In ToChar()");
            int charVal = Int32.Parse(str, NumberStyles.AllowHexSpecifier);
            char charC = (char)charVal;
            return string.Format("{0:X}", charC);
        }

        internal static Dictionary<string, string> ToDictionary(this object obj)
        {
            logger.Trace("In ToDictionary()");
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                if (obj != null)
                {
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        string p = prop.Name;
                        string v = (prop.GetValue(obj, null) ?? "").ToString();
                        if (v != null)
                        {
                            result.Add(p, v);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new Exception(ex.ToString());
            }
            return result;
        }
    }
}
