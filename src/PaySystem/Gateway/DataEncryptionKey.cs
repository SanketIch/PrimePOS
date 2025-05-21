using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Gateway
{
    public  class DataEncryptionKey
    {
        public struct transKey
        {
            /// <summary>
            /// RSA encrypted Data of AES IV
            /// </summary>
            public byte[] IV { get; set; }
            /// <summary>
            /// RSA encrypted Data of AES Key
            /// </summary>
            public byte[] TransKey { get; set; }

            public RSAParameters RSAParams { get; set; }
        }
        /// <summary>
        /// Encryption Key and IV. Both are RSA
        /// </summary>
        public transKey TransEKey;
    }
}
