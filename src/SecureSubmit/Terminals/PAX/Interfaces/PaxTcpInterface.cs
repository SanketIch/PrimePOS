using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using SecureSubmit.Infrastructure;
using SecureSubmit.Terminals.Abstractions;
using SecureSubmit.Terminals.Extensions;
using System.Text;
using NLog;
namespace SecureSubmit.Terminals.PAX {
    internal class PaxTcpInterface : IDeviceCommInterface {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        TcpClient _client;
        NetworkStream _stream;
        ITerminalConfiguration _settings;
        int _nakCount = 0;
        bool isCancelled = false;

        public event MessageSentEventHandler OnMessageSent;

        public PaxTcpInterface(ITerminalConfiguration settings) {
            this._settings = settings;
           // Connect();
        }

        public void Connect() {
            if (_client==null) {
                _client = new TcpClient();
                _client.ConnectAsync(_settings.IpAddress, int.Parse(_settings.Port)).Wait(_settings.TimeOut);
                _stream = _client.GetStream();
                _stream.ReadTimeout = _settings.TimeOut;
            }
        }

        public void Disconnect() {
            if (_stream != null) {
                _stream.Dispose();
                _stream = null;
            }

            if (_client != null) {
                _client.Close();
                _client = null;
            }
        }

        public byte[] Send(IDeviceMessage message) {
            logger.Trace("public byte[] Send(IDeviceMessage message) - Entered");
            byte[] rvalue ;
            //if (!IsConnected()) {
                Connect();
            //}
            using (_stream) {
                byte[] buffer = message.GetSendBuffer();

                try {
                    if (OnMessageSent != null)
                        OnMessageSent(message.ToString());
                    for (int i = 0; i < 3; i++) {
                        if (message.ToString() == "[STX]A14[FS]1.37[FS][ETX]\\" && _stream.CanWrite) {
                            _stream.WriteAsync(buffer, 0, buffer.Length).Wait();
                            isCancelled = true;
                            rvalue = _stream.GetTerminalResponse();
                            #region TCP Disconnection issue PRIMEPOS-2882
                            if (rvalue == null)
                            {
                                Disconnect();
                                Connect();
                            }
                            #endregion
                            //Thread.Sleep(2000);
                        }
                        else {
                            isCancelled = false;
                            _stream.WriteAsync(buffer, 0, buffer.Length).Wait();
                            rvalue = _stream.GetTerminalResponse();
                            if (rvalue != null) {
                                byte lrc = rvalue[rvalue.Length - 1]; // should the the LRC
                                if (lrc != TerminalUtilities.CalculateLRC(rvalue)) {
                                    SendControlCode(ControlCodes.NAK);
                                } else {
                                    SendControlCode(ControlCodes.ACK);
                                    return rvalue;
                                }
                            }
                            else // TCP Disconnection issue PRIMEPOS-2882
                            {
                                Disconnect();
                                Connect();
                            }
                        }
                        if (isCancelled) {
                            var rResponse = (char)ControlCodes.STX + "0" + (char)ControlCodes.FS + "A15" + (char)ControlCodes.FS + "1.37" + (char)ControlCodes.FS + "000000" + (char)ControlCodes.FS + "OK" + (char)ControlCodes.ETX;
                            rvalue = Encoding.ASCII.GetBytes(rResponse);
                            isCancelled = false;
                            return rvalue;
                        }
                    }
                    throw new HpsMessageException("Terminal did not respond in the given timeout.");
                } catch (Exception exc) {
                    logger.Error(exc.Message);
                    if (isCancelled) {
                        var rResponse = (char)ControlCodes.STX+"0"+ (char)ControlCodes.FS +"A15"+ (char)ControlCodes.FS +"1.37"+ (char)ControlCodes.FS +"000000"+ (char)ControlCodes.FS +"OK"+ (char)ControlCodes.ETX;
                        rvalue = Encoding.ASCII.GetBytes(rResponse);
                        isCancelled = false;
                        return rvalue;
                    } else {
                        throw new HpsMessageException(exc.Message, exc);
                    }
                } finally {
                    Disconnect();
                }

            }
            logger.Trace("public byte[] Send(IDeviceMessage message) - Exited");
        }

        private void SendControlCode(ControlCodes code) {
            if (code != ControlCodes.NAK) {
                _nakCount = 0;
                _stream.Write(new byte[] { (byte)code }, 0, 1);
            } else if (++_nakCount == 3)
                SendControlCode(ControlCodes.EOT);
        }
    }
}
