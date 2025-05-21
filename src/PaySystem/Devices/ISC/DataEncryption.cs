using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace EDevice
{
    /// <summary>
    /// This class is use for the encryption and decrytpion of all data
    /// Method use are RSA Cryptography of the AES Key and IV.
    /// RSA + AES as a hybrid encryption
    /// </summary>
    internal class DataEncryption
    {
        #region Properties
        public static DataEncryptionKey DEK = null;
        RSAParameters RSAParams
        {
            get { return DEK.TransEKey.RSAParams; }
            set
            {
                if (DEK == null)
                {
                    DEK = new DataEncryptionKey();
                }
                DEK.TransEKey.RSAParams = value;
            }
        }
        #endregion Properties

        #region Constructor DataEncryption
        /// <summary>
        /// Data encryption using Private and Public Keys
        /// </summary>
        internal DataEncryption()
        {        
            try
            {
                DEK = new DataEncryptionKey();
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.PersistKeyInCsp = false;                 
                    RSAParams = RSA.ExportParameters(true);
                    using (AesCryptoServiceProvider Aes = new AesCryptoServiceProvider())
                    {
                        /* Get the Aes key and Iv and encrypt them using RSA */
                        DEK.TransEKey.TransKey = RSAEncryption(Aes.Key, RSA.ExportParameters(false));
                        DEK.TransEKey.IV = RSAEncryption(Aes.IV, RSA.ExportParameters(false));
                    }
                    RSA.Clear();
                }

            }
            catch (CryptographicException ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion Constructor DataEncryption

        #region RSA Encryption of Key and IV
        /// <summary>
        /// RSA Encryption
        /// </summary>
        /// <param name="DataToEncrypt"></param>
        /// <param name="RSAPars"></param>
        /// <returns></returns>
        private byte[] RSAEncryption(byte[] DataToEncrypt, RSAParameters RSAPars)
        {       
            byte[] encrypted = null;
            try
            {
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.PersistKeyInCsp = false;
                    RSA.ImportParameters(RSAPars);
                    encrypted = RSA.Encrypt(DataToEncrypt, false);
                    RSA.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return encrypted;
        }

        /// <summary>
        /// Used for testing of the RSA Decryption
        /// </summary>
        /// <param name="DataToDecrypt"></param>
        /// <param name="RSAPars"></param>
        /// <returns></returns>
        private byte[] RSADecryption(byte[] DataToDecrypt, RSAParameters RSAPars)
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
        #endregion RSA Encryption of Key and IV

        #region Encryption Of ALL Data Using Private and Public keys
        /// <summary>
        /// Encryption Method to encrypt all data for transmission
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        public byte[] Encryption(byte[] bData)
        {
            byte[] EncryptedData = null;
            try
            {
                if (DEK != null && DEK.TransEKey.TransKey != null && DEK.TransEKey.IV != null)
                {
                    using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                    {
                        aes.Key = RSADecryption(DEK.TransEKey.TransKey, RSAParams);
                        aes.IV = RSADecryption(DEK.TransEKey.IV, RSAParams);

                        ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                            {
                                using (StreamWriter sw = new StreamWriter(cs))
                                {
                                    sw.Write(Encoding.Default.GetString(bData, 0, bData.Length));
                                }
                                EncryptedData = ms.ToArray();
                                //Decryption(EncryptedData, DEK);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Error in encryption. No Key found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return EncryptedData;
        }
        #region Test Decryption
        public string Decryption(byte[] encryptedData, DataEncryptionKey oDEK)
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
                                   // string t = sr.ReadToEnd();
                                   // string b = Encoding.Default.GetBytes(t).FromByte();
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
        #endregion Test Decryption

        # endregion Encryption Of ALL Data Using Private and Public keys
    }
}
