using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace MMSAppUpdater
{
    public class Encryption
    {
        public Encryption()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <remarks>
        /// Supported .Net intrinsic SymmetricAlgorithm classes.
        /// </remarks>
        public enum SymmProvEnum : int
        {
            DES, RC2, Rijndael
        }

        private SymmetricAlgorithm mobjCryptoService;

        /// <remarks>
        /// Constructor for using an intrinsic .Net SymmetricAlgorithm class.
        /// </remarks>
        public Encryption(SymmProvEnum NetSelected)
        {
            switch (NetSelected)
            {
                case SymmProvEnum.DES:
                    mobjCryptoService = new DESCryptoServiceProvider();
                    break;
                case SymmProvEnum.RC2:
                    mobjCryptoService = new RC2CryptoServiceProvider();
                    break;
                case SymmProvEnum.Rijndael:
                    mobjCryptoService = new RijndaelManaged();
                    break;
            }
        }

        /// <remarks>
        /// Constructor for using a customized SymmetricAlgorithm class.
        /// </remarks>
        public Encryption(SymmetricAlgorithm ServiceProvider)
        {
            mobjCryptoService = ServiceProvider;
        }

        /// <remarks>
        /// Depending on the legal key size limitations of a specific CryptoService provider
        /// and length of the private key provided, padding the secret key with space character
        /// to meet the legal size of the algorithm.
        /// </remarks>
        private byte[] GetLegalKey(string Key)
        {
            string sTemp;
            if (mobjCryptoService.LegalKeySizes.Length > 0)
            {
                int lessSize = 0, moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;
                // key sizes are in bits
                while (Key.Length * 8 > moreSize)
                {
                    lessSize = moreSize;
                    moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
                }
                sTemp = Key.PadRight(moreSize / 8, ' ');
            }
            else
                sTemp = Key;

            // convert the secret key to byte array
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

     

        public string Decrypting(string Source, string Key)
        {
            // convert from Base64 to binary
            byte[] bytIn = System.Convert.FromBase64String(Source);
            // create a MemoryStream with the input
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

            byte[] bytKey = GetLegalKey(Key);

            // set the private key
            mobjCryptoService.Key = bytKey;
            mobjCryptoService.IV = bytKey;

            // create a Decryptor from the Provider Service instance
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

            // create Crypto Stream that transforms a stream using the decryption
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            // read out the result from the Crypto Stream
            System.IO.StreamReader sr = new System.IO.StreamReader(cs);
            return sr.ReadToEnd();
        }
    }

}
