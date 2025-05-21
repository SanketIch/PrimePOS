﻿using System;
using System.IO;
using System.Text;
using SecureSubmit.Terminals.Extensions;
using SecureSubmit.Terminals.Abstractions;

namespace SecureSubmit.Terminals.PAX {
    public class AvsRequest : IRequestSubGroup {
        public string ZipCode { get; set; }
        public string Address { get; set; }

        public string GetElementString() {
            var sb = new StringBuilder();
            sb.Append(ZipCode);
            sb.Append((char)ControlCodes.US);
            sb.Append(Address);

            return sb.ToString().TrimEnd((char)ControlCodes.US);
        }
    }

    public class AvsResponse : IResponseSubGroup {
        public string AvsResponseCode { get; private set; }
        public string AvsResponseMessage { get; private set; }

        public AvsResponse(BinaryReader br) {
            var values = br.ReadToCode(ControlCodes.FS);
            if (string.IsNullOrEmpty(values))
                return;

            var data = values.Split((char)ControlCodes.US);
            try {
                this.AvsResponseCode = data[0];
                this.AvsResponseMessage = data[1];
            }
            catch (IndexOutOfRangeException) {
            }
        }
    }
}
