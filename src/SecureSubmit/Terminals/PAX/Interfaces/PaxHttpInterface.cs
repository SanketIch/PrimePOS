﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using SecureSubmit.Infrastructure;
using SecureSubmit.Terminals.Abstractions;

namespace SecureSubmit.Terminals.PAX {
    internal class PaxHttpInterface : IDeviceCommInterface {
        ITerminalConfiguration _settings;
        WebRequest _client;

        public event MessageSentEventHandler OnMessageSent;

        public PaxHttpInterface(ITerminalConfiguration settings) {
            this._settings = settings;
        }

        public void Connect() {
            // not required for this connection mode
        }

        public void Disconnect() {
            // not required for this connection mode
        }

        public byte[] Send(IDeviceMessage message) {
            if (OnMessageSent != null)
                OnMessageSent(message.ToString());

            try {
                string payload = Convert.ToBase64String(message.GetSendBuffer());
    
                _client = HttpWebRequest.Create(string.Format("http://{0}:{1}?{2}", _settings.IpAddress, _settings.Port, payload));
                var response = (HttpWebResponse)_client.GetResponse();
                var buffer = new List<byte>();
                using (var sr = new StreamReader(response.GetResponseStream())) {
                    var rec_buffer = sr.ReadToEnd();
                    foreach (char c in rec_buffer)
                        buffer.Add((byte)c);
                }
                return buffer.ToArray();
            }
            catch (Exception exc) {
                throw new HpsMessageException("Failed to send message. Check inner exception for more details.", exc);
            }
        }
    }
}
