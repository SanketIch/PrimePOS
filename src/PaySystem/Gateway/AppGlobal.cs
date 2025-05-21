using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Gateway.PrismPay;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security;
using System.Runtime.InteropServices;
//using PrismPay;
using System.Security.Cryptography;

namespace Gateway
{

    internal class AppGlobal
    {
        public static string SearealizeResult(TransactionResult oresult)
        {
            string xml = string.Empty;
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(TransactionResult));
            using(StringWriter sWriter = new StringWriter())
            using (XmlWriter xWriter = XmlWriter.Create(sWriter))
            {
                oXmlSerializer.Serialize(xWriter, oresult);
                xml = sWriter.ToString();

            }


            return xml;
        }
        public static string SearealizeResult(PrismPay.ProcessResult oresult)
        {
            string xml = string.Empty;
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(PrismPay.ProcessResult));
            using (StringWriter sWriter = new StringWriter())
            using (XmlWriter xWriter = XmlWriter.Create(sWriter))
            {
                oXmlSerializer.Serialize(xWriter, oresult);
                xml = sWriter.ToString();

            }


            return xml;
        }


        public static string ConvertSecureStringtoString(SecureString oSecureString)
        {
            if (oSecureString == null)
            {
                return "";
            }
            else
            {
                IntPtr unmanagedString = IntPtr.Zero;
                try
                {
                    unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(oSecureString);
                    return Marshal.PtrToStringUni(unmanagedString);
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
                }
            }


            //return "";
        }

        /// <summary>
        /// Used for testing of the RSA Decryption
        /// </summary>
        /// <param name="DataToDecrypt"></param>
        /// <param name="RSAPars"></param>
        /// <returns></returns>
        private static byte[] RSADecryption(byte[] DataToDecrypt, RSAParameters RSAPars)
        {
            byte[] decrypted = null;
            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.PersistKeyInCsp = false;
                    RSA.ImportParameters(RSAPars);
                    decrypted = RSA.Decrypt(DataToDecrypt, false);
                    RSA.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return decrypted;
        }


        /// <summary>
        /// Decrypt the Encrypted Byte array Parameter with the provided Keys and return back the Decrypted string
        /// </summary>
        /// <param name="encryptedData">Encrypted Data in a Byte Array Format</param>
        /// <param name="oDEK">The Encrypted Data Encryption Key  as an object of DataEncryptionKey Class</param>
        /// <returns>The Decrypted Data as String</returns>
        public static string Decryption(byte[] encryptedData, DataEncryptionKey oDEK)//, RSAParameters RSAParams
        {
            string sdata = string.Empty;
            try
            {
                if (oDEK != null && oDEK.TransEKey.TransKey != null && oDEK.TransEKey.IV != null)
                {
                    using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                    {
                        aes.Key = RSADecryption(oDEK.TransEKey.TransKey, oDEK.TransEKey.RSAParams);
                        aes.IV = RSADecryption(oDEK.TransEKey.IV, oDEK.TransEKey.RSAParams);

                        ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

                        using (MemoryStream ms = new MemoryStream(encryptedData))
                        {
                            using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                            {
                                using (StreamReader sr = new StreamReader(cs))
                                {
                                    sdata = Encoding.Default.GetBytes(sr.ReadToEnd()).FromByte();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sdata;
        }
    }

    internal static class Extension
    {
        internal static string ByteToString(this byte[] data)
        {
            StringBuilder strHex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
            {
                strHex.AppendFormat("{0:x2}", b);
            }
            return strHex.ToString();
        }

        internal static string FromByte(this byte[] data)
        {
            string result = System.Text.Encoding.ASCII.GetString(data);
            return result;
        }
    }

    /// <summary>
    /// The supported Device type
    /// </summary>
    public enum DeviceType
        {
            /// <summary>
            /// For Most Magtech Devices
            /// </summary>
            Magtech,
            /// <summary>
            /// For Magtech Ipad
            /// </summary>
            Magtech_IPad,
            /// <summary>
            /// For IDTECH Devices
            /// </summary>
            IDTECH,
            /// <summary>
            /// For Ingenice iSC250,350 / iPP250,350
            /// </summary>
            Ingenico,
            /// <summary>
            /// None Specified
            /// </summary>
            None
        };

    /// <summary>
    /// Shows the supported Gateways that can be used to process a Transaction
    /// </summary>
    public enum Gateway
    {
        /// <summary>
        /// To use PrismPay Gateway to process transaction
        /// </summary>
        PrismPay,
        /// <summary>
        /// To use WorldPay Gateway to process a Transaction
        /// </summary>
        WorldPay

    };

    /// <summary>
    /// The Supported Payment (Transaction ) Methord
    /// </summary>
    internal enum PayType
    {
        Credit,
        Debit,
        EBT,
        Stored_Profile,
        EBTCashBenifit,
        EBTFoodStamp,
        EBTFoodStampVoucher

    };

    /// <summary>
    /// Supported Transaction types for a Payment Methord
    /// </summary>
    internal enum TransactionType
    {
        Sale,
        Credit,
        Void,
        Return,
        BalanceEnquiry,
        Withdraw,
        Add,
        Update,
        Retrieve,
        Delete
    };

    public enum ResultType
    {
        Regular,
        Profile
    };
}
