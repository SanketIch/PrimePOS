using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;

namespace POS_Core.Document
{
    /// <summary>
    /// Class having static methods for encrypting / decrypting strings and files using RijndaelManaged encryption/decryption class
    /// </summary>
    public class NativeEncryption
    {
        #region Static Member Variables
        static byte[] _keyBytes = Encoding.ASCII.GetBytes("}n`=|,^#H%~<8g(@");
        static byte[] _initVectorBytes = Encoding.ASCII.GetBytes("+~*w>/c$@0!^m&]Z");
        #endregion

        #region Text Encryption / Decryption
        /// <summary>
        /// Encrypt the specified text
        /// </summary>
        /// <param name="sPlainText">Plaint text which is required to be encrypted</param>
        /// <param name="sEncryptedText">Encrypted text generated from the specified plain text OR the exception details if encryption fails</param>
        /// <returns>Process success as TRUE or FALSE</returns>
        public static bool EncryptText(string sPlainText, ref string sEncryptedText)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;

            //Set encrypted text variable as blank
            sEncryptedText = "";

            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(sPlainText);

                // Create Rijndael encryption object and set encryption mode to Cipher Block Chaining (CBC)
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;

                // Generate encryptor from the existing key bytes and initialization vector
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(_keyBytes, _initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream();

                // Define cryptographic stream (always use Write mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                // Start encrypting.
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                // Finish encrypting.
                cryptoStream.FlushFinalBlock();

                // Convert encrypted data from a memory stream into a byte array.
                byte[] cipherTextBytes = memoryStream.ToArray();

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert encrypted data into a base64-encoded string and assign it to specified variable
                sEncryptedText = Convert.ToBase64String(cipherTextBytes);

                // Set the boolean return variable to true
                bReturn = true;
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the encrypted string variable
                sEncryptedText = "Exception in MMS.Encryption.NativeEncryption.EncryptText(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }

            return bReturn;
        }

        /// <summary>
        /// Decrypt the specified text
        /// <param name="sEncryptedText">Encrypted text which is required to be decrypted</param>
        /// <param name="sDecryptedText">Decrypted text generated from the specified encrypted text OR the exception details if decryption fails</param>
        /// <returns>Process success as TRUE or FALSE</returns>
        public static bool DecryptText(string sEncryptedText, ref string sPlainText)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;

            //Set decrypted text variable as blank
            sPlainText = "";
            if (string.IsNullOrWhiteSpace(sEncryptedText))
                return bReturn;
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(sEncryptedText);

                // Create Rijndael encryption object and set encryption mode to Cipher Block Chaining (CBC)
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization vector
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(_keyBytes, _initVectorBytes);

                // Define memory stream which will be used to hold decrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for decryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string and assign it to specified variable
                sPlainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Set the boolean return variable to true
                bReturn = true;
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the decrypted string variable
                sPlainText = "Exception in MMS.Encryption.NativeEncryption.DecryptText(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }

            return bReturn;
        }
        #endregion

        #region File Encryption / Decryption
        /// <summary>
        /// Encrypt the specified file
        /// </summary>
        /// <param name="sFileToEncrypt">Source file to be encrypted</param>
        /// <param name="sEncryptedFile">Encrypted destination file name as string which will contain exception details if encryption fails</param>
        /// <returns>Process success as TRUE or FALSE</returns>
        public static bool EncryptFile(string sFileToEncrypt, ref string sEncryptedFile)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;

            try
            {
                byte[] bytesToEncrypt = File.ReadAllBytes(sFileToEncrypt);
                byte[] bytesEncrypted = EncryptBytes(bytesToEncrypt, ref sEncryptedFile);

                if (bytesEncrypted != null)
                {
                    File.WriteAllBytes(sEncryptedFile, bytesEncrypted);
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the encrypted file name variable
                sEncryptedFile = "Exception in MMS.Encryption.NativeEncryption.EncryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }

            return bReturn;
        }

        public static byte[] EncryptFileDB(string sFileToEncrypt)
        {
            //Declare and initialze the boolean return variable as false
            byte[] bytesEncrypted = null;
            string sEncryptedFile = string.Empty;
            try
            {
                byte[] bytesToEncrypt = File.ReadAllBytes(sFileToEncrypt);
                bytesEncrypted = EncryptBytes(bytesToEncrypt, ref sEncryptedFile);
            }
            catch (Exception oEx)
            {
                // Assign exception details to the encrypted file name variable
                sEncryptedFile = "Exception in MMS.Encryption.NativeEncryption.EncryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bytesEncrypted;
        }

        public static byte[] EncryptFileDB(byte[] bytesToEncrypt)
        {
            //Declare and initialze the boolean return variable as false
            byte[] bytesEncrypted = null;
            string sError = string.Empty; ;
            try
            {
                return EncryptBytes(bytesToEncrypt, ref sError);
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bytesEncrypted = null;

                // Assign exception details to the encrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.EncryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bytesEncrypted;
        }

        /// <summary>
        /// Encrypt the specified file
        /// </summary>
        /// <param name="sFileToEncrypt">Source file to be encrypted</param>
        /// <param name="sEncFilePath">Encrypted destination file name as string which will contain exception details if encryption fails</param>
        /// <returns>Process success as TRUE or FALSE</returns>
        /// 
        public static bool EncryptFile(Image image, string sEncFilePath, ref string sError)
        {
            return EncryptFile(image, sEncFilePath, System.Drawing.Imaging.ImageFormat.Bmp, ref sError);
        }

        public static bool EncryptFile(Image image, System.Drawing.Imaging.ImageFormat imgFormat, ref string sError, out Stream stream)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;
            stream = null;
            sError = string.Empty;
            try
            {
                byte[] bytesToEncrypt = imageToByteArray(image, imgFormat);
                byte[] bytesEncrypted = EncryptBytes(bytesToEncrypt, ref sError);

                stream = new MemoryStream(bytesEncrypted);
                if (stream != null)
                {
                    bReturn = true;
                }
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the encrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.EncryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bReturn;
        }

        public static bool EncryptFile(string sFileToEncrypt, out Stream stream, ref string sError)
        {
            stream = null;
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;

            try
            {
                byte[] bytesToEncrypt = File.ReadAllBytes(sFileToEncrypt);
                byte[] bytesEncrypted = EncryptBytes(bytesToEncrypt, ref sError);

                stream = new MemoryStream(bytesEncrypted);
                if (stream != null)
                {
                    bReturn = true;
                }
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the encrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.EncryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bReturn;
        }

        public static bool EncryptFile(Image image, string sEncFilePath, System.Drawing.Imaging.ImageFormat imgFormat, ref string sError)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;
            sError = string.Empty;
            try
            {
                byte[] bytesToEncrypt = imageToByteArray(image, imgFormat);
                byte[] bytesEncrypted = EncryptBytes(bytesToEncrypt, ref sError);

                if (bytesEncrypted != null && sError == "")
                {
                    File.WriteAllBytes(sEncFilePath, bytesEncrypted);
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }
                bytesToEncrypt = null;
                bytesEncrypted = null;
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the encrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.EncryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bReturn;
        }

        public static byte[] EncryptFileDB(Image image, System.Drawing.Imaging.ImageFormat imgFormat)
        {
            //Declare and initialze the boolean return variable as false
            byte[] bytesEncrypted = null;
            string sError = string.Empty; ;
            try
            {
                byte[] bytesToEncrypt = imageToByteArray(image, imgFormat);
                bytesEncrypted = EncryptBytes(bytesToEncrypt, ref sError);
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bytesEncrypted = null;

                // Assign exception details to the encrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.EncryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bytesEncrypted;
        }

        public static byte[] imageToByteArray(Image imageIn)
        {
            return imageToByteArray(imageIn, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public static byte[] imageToByteArray(Image imageIn, System.Drawing.Imaging.ImageFormat imgFormat)
        {
            if (imageIn == null)
            {
                return new byte[0];
            }
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, imgFormat); //System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// Decrypt the specified file
        /// </summary>
        /// <param name="sFileToDecrypt">Source file to be decrypted</param>
        /// <param name="sDecryptedFile">Decrypted destination file name as string which will contain exception details if decryption fails</param>
        /// <returns>Process success as TRUE or FALSE</returns>
        public static bool DecryptFile(string sFileToDecrypt, out Image oImage, out string sError)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;
            sError = string.Empty;
            oImage = null;
            try
            {
                if (!File.Exists(sFileToDecrypt))
                    return bReturn;

                byte[] bytesToDecrypt = File.ReadAllBytes(sFileToDecrypt);
                byte[] bytesDecrypted = DecryptBytes(bytesToDecrypt, ref sError);

                if (bytesDecrypted != null)
                {
                    oImage = byteArrayToImage(bytesDecrypted);
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }

                bytesToDecrypt = null;
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the decrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.DecryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bReturn;
        }

        public static byte[] DecryptImageDB(byte[] bytesToDecrypt)
        {
            //Declare and initialze the boolean return variable as false
            byte[] byt;
            string sError = string.Empty;

            try
            {
                byt = DecryptBytes(bytesToDecrypt, ref sError);
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                byt = null;

                // Assign exception details to the decrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.DecryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return byt;
        }

        /// <summary>
        /// Decrypt the specified file
        /// </summary>
        /// <param name="sFileToDecrypt">Source file to be decrypted</param>
        /// <param name="sDecryptedFile">Decrypted destination file name as string which will contain exception details if decryption fails</param>
        /// <returns>Process success as TRUE or FALSE</returns>
        public static bool DecryptFile(string sFileToDecrypt, ref string sDecryptedFile)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;

            try
            {
                if (File.Exists(sFileToDecrypt))
                {
                    byte[] bytesToDecrypt = File.ReadAllBytes(sFileToDecrypt);
                    byte[] bytesDecrypted = DecryptBytes(bytesToDecrypt, ref sDecryptedFile);

                    if (bytesDecrypted != null)
                    {
                        File.WriteAllBytes(sDecryptedFile, bytesDecrypted);
                        bReturn = true;
                    }
                    else
                    {
                        bReturn = false;
                    }
                }
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the decrypted file name variable
                sDecryptedFile = "Exception in MMS.Encryption.NativeEncryption.DecryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bReturn;
        }
        public static bool DecryptFileDB(byte[] bytesToDecrypt, ref string sDecryptedFile)
        {
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;

            try
            {
                byte[] bytesDecrypted = DecryptBytes(bytesToDecrypt, ref sDecryptedFile);

                if (bytesDecrypted != null)
                {
                    File.WriteAllBytes(sDecryptedFile, bytesDecrypted);
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the decrypted file name variable
                sDecryptedFile = "Exception in MMS.Encryption.NativeEncryption.DecryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bReturn;
        }

        /// <summary>
        /// Encrypt the input byte array
        /// </summary>
        /// <param name="bytesToEncrypt">The input byte array to be encrypted</param>
        /// <param name="sError">Error description in case the encryption fails</param>
        /// <returns>Encrypted byte array</returns>
        private static byte[] EncryptBytes(byte[] bytesToEncrypt, ref string sError)
        {
            try
            {
                // Assign the input byte array to the given memory stream.
                MemoryStream stream = new MemoryStream(bytesToEncrypt);

                // Create Rijndael encryption object and set encryption mode to Cipher Block Chaining (CBC)
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;

                // Generate encryptor from the existing key bytes and initialization vector
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(_keyBytes, _initVectorBytes);

                // Encrypt the compressed memory stream into the encrypted memory stream.
                MemoryStream encrypted = new MemoryStream();

                // Define cryptographic stream (always use Write mode for encryption).
                using (CryptoStream cryptor = new CryptoStream(encrypted, encryptor, CryptoStreamMode.Write))
                {
                    // Write the stream to the encrypted memory stream.
                    cryptor.Write(stream.ToArray(), 0, (int)stream.Length);
                    cryptor.FlushFinalBlock();
                    // Return the result.
                    return encrypted.ToArray();
                }
            }
            catch (Exception oEx)
            {
                sError = "Exception in MMS.Encryption.NativeEncryption.EncryptBytes(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
                return null;
            }
        }

        /// <summary>
        /// Dencrypt the input byte array
        /// </summary>
        /// <param name="bytesToDecrypt">The input byte array to be decrypted</param>
        /// <param name="sError">Error description in case the decryption fails</param>
        /// <returns>Decrypted byte array</returns>
        private static byte[] DecryptBytes(byte[] bytesToDecrypt, ref string sError)
        {
            try
            {
                // Create Rijndael encryption object and set encryption mode to Cipher Block Chaining (CBC)
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization vector
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(_keyBytes, _initVectorBytes);

                // Create the array that holds the result.
                byte[] decrypted = new byte[bytesToDecrypt.Length];

                // Create the crypto stream that is used for decrypt. The first argument holds the input as memory stream.
                using (CryptoStream cryptor = new CryptoStream(new MemoryStream(bytesToDecrypt), decryptor, CryptoStreamMode.Read))
                {
                    // Read the encrypted values into the decrypted stream. Decrypts the content.
                    cryptor.Read(decrypted, 0, decrypted.Length);

                    return decrypted;
                }
            }
            catch (Exception oEx)
            {
                sError = "Exception in MMS.Encryption.NativeEncryption.DecryptBytes(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
                return null;
            }
        }
        #endregion

        public static bool DecryptFile(byte[] encDocBytes, out MemoryStream stream, out string sError)
        {
            //TODO : CLOUD CHANGES
            //Declare and initialze the boolean return variable as false
            bool bReturn = false;
            sError = string.Empty;
            stream = null;
            try
            {
                byte[] bytesDecrypted = DecryptBytes(encDocBytes, ref sError);

                if (bytesDecrypted != null)
                {
                    stream = new MemoryStream(bytesDecrypted);
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }

                encDocBytes = null;
            }
            catch (Exception oEx)
            {
                // Set the boolean return variable to false
                bReturn = false;

                // Assign exception details to the decrypted file name variable
                sError = "Exception in MMS.Encryption.NativeEncryption.DecryptFile(): " + oEx.Message + Environment.NewLine + "Details: " + oEx.ToString();
            }
            return bReturn;
        }
    }
}