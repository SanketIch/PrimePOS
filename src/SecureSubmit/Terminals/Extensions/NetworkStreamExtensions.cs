using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SecureSubmit.Infrastructure;
using System.Diagnostics;
using System.IO;
namespace SecureSubmit.Terminals.Extensions {
    internal static class NetworkStreamExtensions {

        #region NileshJ Commented - 20-Dec-2018

        //public static byte[] GetTerminalResponse(this NetworkStream stream) {
        //    var buffer = new byte[4096];
        //    int bytesReceived = stream.ReadAsync(buffer, 0, buffer.Length).Result;

        //    byte[] readBuffer = new byte[bytesReceived];
        //    Array.Copy(buffer, readBuffer, bytesReceived);

        //    var code = (ControlCodes)readBuffer[0];
        //    if (code == ControlCodes.NAK)
        //        return null;
        //    else if (code == ControlCodes.EOT)
        //        throw new HpsMessageException("Terminal returned EOT for the current message.");
        //    else if (code == ControlCodes.ACK) {
        //        return stream.GetTerminalResponse();
        //    }
        //    else if (code == ControlCodes.STX) {
        //        var queue = new Queue<byte>(readBuffer);

        //        // break off only one message
        //        var rec_buffer = new List<byte>();
        //        do {
        //            rec_buffer.Add(queue.Dequeue());
        //            if (rec_buffer[rec_buffer.Count - 1] == (byte)ControlCodes.ETX)
        //                break;
        //        }
        //        while (true);

        //        // Should be the LRC
        //        rec_buffer.Add(queue.Dequeue());
        //        return rec_buffer.ToArray();
        //    }
        //    else throw new HpsMessageException(string.Format("Unknown message received: {0}", code));
        //}
        #endregion


        public static byte[] GetTerminalResponse(this NetworkStream stream)
        {
            Stopwatch PingTimer = new Stopwatch();
            //stream.ReadTimeout = 60000;//Nileshj - PRIMEPOS-2706
            stream.WriteTimeout = 50;//Nileshj
            bool ClientStillConnected = true;
            var rec_buffer = new List<byte>();
            var buffer = new byte[4096];

            PingTimer.Start();
            while (ClientStillConnected)
            {
                //System.Threading.Thread.Sleep(1);
                //if (stream.DataAvailable)
                //{
                try
                {
                    int bytesReceived = stream.Read(buffer, 0, buffer.Length);

                    byte[] readBuffer = new byte[bytesReceived];
                    Array.Copy(buffer, readBuffer, bytesReceived);

                    if (bytesReceived == 0)
                        return null;
                    var code = (ControlCodes)readBuffer[0];
                    if (code == ControlCodes.NAK)
                        return null;
                    else if (code == ControlCodes.EOT)
                        throw new HpsMessageException("Terminal returned EOT for the current message.");
                    else if (code == ControlCodes.ACK)
                    {
                        return stream.GetTerminalResponse();
                    }
                    else if (code == ControlCodes.STX)
                    {
                        var queue = new Queue<byte>(readBuffer);

                        // break off only one message


                        do
                        {
                            rec_buffer.Add(queue.Dequeue());
                            if (rec_buffer[rec_buffer.Count - 1] == (byte)ControlCodes.ETX)
                                break;
                        }
                        while (true);

                        // Should be the LRC
                        rec_buffer.Add(queue.Dequeue());
                    }
                    else throw new HpsMessageException(string.Format("Unknown message received: {0}", code));
                }
                catch (Exception e)
                {
                    if (e is IOException || e is ObjectDisposedException)
                    {
                        ClientStillConnected = false;
                        throw new HpsMessageException(string.Format("Network error found", 01));
                    }
                    else
                        throw e;
                }

                // Ping client.
                try
                {
                    if (PingTimer.ElapsedMilliseconds > 5000)
                    {
                        byte[] KGPING = System.Text.Encoding.ASCII.GetBytes("Hi");
                        System.Threading.Thread.Sleep(100);
                        //stream.Write(KGPING, 0, KGPING.Length);
                        PingTimer.Restart();
                    }
                }
                catch (Exception e)
                {
                    if (e is IOException || e is ObjectDisposedException)
                    {
                        ClientStillConnected = false;
                        throw new HpsMessageException(string.Format("Network error found", 01));
                    }
                    else
                        throw e;
                }

                return rec_buffer.ToArray();
            }
            //}
            return null;
        }
    }
}
