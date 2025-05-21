using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EDevice
{
    internal class DataEncryptionKey
    {
        #region Transaction Key
        public struct transKey
        {
            /// <summary>
            /// RSA encrypted Data of AES IV
            /// </summary>
            public byte[] IV { get; internal set; }
            /// <summary>
            /// RSA encrypted Data of AES Key
            /// </summary>
            public byte[] TransKey { get; internal set; }
            /// <summary>
            /// RSA Parameters (RSA encryption)
            /// </summary>
            public RSAParameters RSAParams { get; internal set; }
            
        }
        /// <summary>
        /// Encryption Key and IV. Both are RSA
        /// </summary>
        public transKey TransEKey;
        #endregion Transaction Key
    }
}
