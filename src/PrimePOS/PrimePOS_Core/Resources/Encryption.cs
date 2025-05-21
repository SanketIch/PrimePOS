using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.IO.Compression;

namespace POS_Core.Resources
{
    public class EncryptString
    {
        static byte[] key;
        static byte[] IV;

        public static string Encrypt(string str)
        {

            // Create a new instance of the RC2CryptoServiceProvider class
            // and automatically generate a Key and IV.
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();

            //MessageBox.Show("Effective key size is {0} bits." + rc2CSP.EffectiveKeySize);

            GenerateKays();
            // Get the key and IV.
            rc2CSP.Key = key;
            rc2CSP.IV = IV;
            
            // Get an encryptor.
            ICryptoTransform encryptor = rc2CSP.CreateEncryptor(key, IV);

            // Encrypt the data as an array of encrypted bytes in memory.
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            // Convert the data to a byte array.
            string original = str;
            byte[] toEncrypt = Encoding.ASCII.GetBytes(original);
            
            // Write all data to the crypto stream and flush it.
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();

            // Get the encrypted array of bytes.
            byte[] encrypted = msEncrypt.ToArray();

            return Convert.ToBase64String(encrypted);
            // Display the original data and the decrypted data.
        }

        public static string Decrypt(string str)
        {

            // Create a new instance of the RC2CryptoServiceProvider class
            // and automatically generate a Key and IV.
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();

            GenerateKays();
            // Get the key and IV.
            rc2CSP.Key = key;
            rc2CSP.IV = IV;

            // Convert the data to a byte array.
            byte[] toDecrypt = null;

            try
            {
                toDecrypt = Convert.FromBase64String(str);
            }
            catch (Exception exp)
            {
                return str;
            }

            //Get a decryptor that uses the same key and IV as the encryptor.
            ICryptoTransform decryptor = rc2CSP.CreateDecryptor(key, IV);

            // Now decrypt the previously encrypted message using the decryptor
            // obtained in the above step.
            MemoryStream msDecrypt = new MemoryStream(toDecrypt);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

            // Read the decrypted bytes from the decrypting stream
            // and place them in a StringBuilder class.

            StringBuilder roundtrip = new StringBuilder();

            int b = 0;

            do
            {
                b = csDecrypt.ReadByte();

                if (b != -1)
                {
                    roundtrip.Append((char)b);
                }

            } while (b != -1);


            // Display the original data and the decrypted data.
            return roundtrip.ToString();

        }

        private static void GenerateKays()
        {
            key = new byte[] { 42, 0, 173, 131, 7, 39, 96, 74, 141, 13, 153, 252, 182, 45, 19, 224 };
            IV = new byte[] { 132, 4, 246, 11, 61, 160, 16, 202 };

        }

        public static string CustomEncrypt(long number)
        {
            string strNumber = number.ToString();
            string retString = "";
            Random oRandom = new Random();
            int rndNum=oRandom.Next(65, 80);
            
            retString = Convert.ToChar(rndNum).ToString();
            retString += Convert.ToChar(rndNum + strNumber.Length);

            for (int i = 0; i < 5; i++)
            {
                if (i < strNumber.Length)
                {
                    retString += Convert.ToChar(rndNum + Convert.ToInt16(strNumber.Substring(i, 1)));
                }
                else
                {
                    retString += Convert.ToChar(oRandom.Next(65,90));
                }
            }

            return retString;
        }

        public static string CustomDecrypt(string str)
        {
            string strReturn = "";
            int startNum = Convert.ToInt32(Convert.ToChar(str.Substring(0, 1)));
            int numLength = Convert.ToInt32(Convert.ToChar(str.Substring(1, 1)))-startNum;

            for (int i = 2; i < numLength+2; i++)
            {
                strReturn += Convert.ToString( Convert.ToInt32(Convert.ToChar(str.Substring(i, 1))) - startNum);
            }
            
            return strReturn;
        }
    }
}
