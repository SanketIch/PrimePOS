﻿using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Terminals.Messaging;
using MMS.GlobalPayments.Api.Utils;
using System;
using System.Net.Sockets;

namespace MMS.GlobalPayments.Api.Terminals.UPA {
    internal class UpaTcpInterface : IDeviceCommInterface {
        TcpClient _client;
        NetworkStream _stream;
        ITerminalConfiguration _settings;
        int _connectionCount = 0;
        public event MessageSentEventHandler OnMessageSent;

        public UpaTcpInterface(ITerminalConfiguration settings) {
            _settings = settings;
        }

        public void Connect() {
            int connectionTimestamp = Int32.Parse(DateTime.Now.ToString("mmssfff"));

            if (_client == null) {
                _client = new TcpClient();
                _client.ConnectAsync(_settings.IpAddress, int.Parse(_settings.Port)).Wait(_settings.Timeout);

                if (Int32.Parse(DateTime.Now.ToString("mmssfff")) > connectionTimestamp + _settings.Timeout) {
                    throw new MessageException("Connection not established within the specified timeout.");
                }

                _stream = _client.GetStream();
                _stream.ReadTimeout = _settings.Timeout;
            }
            _connectionCount++;
        }

        public void Disconnect() {
            _connectionCount--;
            if (_connectionCount == 0) {
                _stream?.Dispose();
                _stream = null;

                _client?.Dispose();
                _client = null;
            }
        }

        public byte[] Send(IDeviceMessage message) {
            byte[] buffer = message.GetSendBuffer();

            var readyReceived = false;
            byte[] responseMessage = null;

            Connect();
            try {
                var task = _stream.WriteAsync(buffer, 0, buffer.Length);

                if (!task.Wait(_settings.Timeout)) {
                    throw new MessageException("Terminal did not respond in the given timeout.");
                }

                do {
                    var rvalue = _stream.GetTerminalResponseAsync();
                    if (rvalue != null) {
                        var msgValue = GetResponseMessageType(rvalue);

                        switch (msgValue)
                        {
                            case UpaMessageType.Ack:
                                break;
                            case UpaMessageType.Nak:
                                break;
                            case UpaMessageType.Ready:
                                readyReceived = true;
                                break;
                            case UpaMessageType.Busy:
                                break;
                            case UpaMessageType.TimeOut:
                                break;
                            case UpaMessageType.Msg:
                                responseMessage = TrimResponse(rvalue);
                                if (IsNonReadyResponse(responseMessage)) {
                                    readyReceived = true; // since reboot doesn't return READY
                                }
                                SendAckMessageToDevice();
                                break;
                            default:
                                throw new Exception("Message field value is unknown in API response.");
                        }
                    }
                    else
                    {
                        // Reset the connection before the next attempt
                        Disconnect();
                        Connect();
                    }
                } while (!readyReceived);

                return responseMessage;
            }
            catch (Exception exc) {
                throw new MessageException(exc.Message, exc);
            }
            finally {
                OnMessageSent?.Invoke(message.ToString());
                Disconnect();
            }
        }

        private byte[] TrimResponse(byte[] value) {
                return System.Text.Encoding.UTF8.GetBytes(
                    System.Text.Encoding.UTF8.GetString(value)
                        .TrimStart((char)ControlCodes.STX, (char)ControlCodes.LF)
                        .TrimEnd((char)ControlCodes.LF, (char)ControlCodes.ETX)
            );
        }

        private string GetResponseMessageType(byte[] response) {
            var jsonObject = System.Text.Encoding.UTF8.GetString(TrimResponse(response));
            var jsonDoc = JsonDoc.Parse(jsonObject);
            return jsonDoc.GetValue<string>("message");
        }

        private void SendAckMessageToDevice() {
            var doc = new JsonDoc();
            doc.Set("message", UpaMessageType.Ack);

            var message = TerminalUtilities.BuildUpaRequest(doc.ToString());
            var ackBuffer = message.GetSendBuffer();

            _stream.Write(ackBuffer, 0, ackBuffer.Length);
        }

        private bool IsNonReadyResponse(byte[] responseMessage) {
            var responseMessageString = System.Text.Encoding.UTF8.GetString(responseMessage);
            var response = JsonDoc.Parse(responseMessageString);
            var data = response.Get("data");
            if (data == null) {
                return false;
            }
            var cmdResult = data.Get("cmdResult");
            return (
                    data.GetValue<string>("response") == UpaTransType.Reboot ||
                    (cmdResult != null && cmdResult.GetValue<string>("result") == "Failed")
                );
        }
    }
}
