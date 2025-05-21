﻿using System;
using System.Collections.Generic;
using System.Text;
using SecureSubmit.Terminals.Abstractions;
using SecureSubmit.Terminals.PAX;
using NLog;
namespace SecureSubmit.Terminals
{
    internal class TerminalUtilities
    {
        const string _version = "1.37";
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private static string GetElementString(object[] elements)
        {
            logger.Debug("GetElementString() - Entered");
            var sb = new StringBuilder();
            foreach (var element in elements)
            {
                if (element is ControlCodes)
                    sb.Append((char)((ControlCodes)element));
                else if (element is IRequestSubGroup)
                    sb.Append(((IRequestSubGroup)element).GetElementString());
                else if (element is string[])
                    foreach (var sub_element in element as string[])
                    {
                        sb.Append((char)ControlCodes.FS);
                        sb.Append(sub_element);
                    }
                else if (sb.Length != 0)
                {
                    sb.Append((char)ControlCodes.FS);
                    sb.Append(element);
                }
                else
                    sb.Append(element);
            }
            logger.Debug("GetElementString() - " + sb.ToString());
            return sb.ToString();
        }

        private static DeviceMessage BuildMessage(string messageId, string message)
        {
            var buffer = new List<byte>();

            // Begin Message
            buffer.Add((byte)ControlCodes.STX);

            // Add Message ID
            foreach (char c in messageId)
                buffer.Add((byte)c);
            buffer.Add((byte)ControlCodes.FS);

            // Add Version
            foreach (char c in _version)
                buffer.Add((byte)c);
            buffer.Add((byte)ControlCodes.FS);

            // Add the Message
            if (!string.IsNullOrEmpty(message))
            {
                foreach (char c in message)
                    buffer.Add((byte)c);
            }

            // End the Message
            buffer.Add((byte)ControlCodes.ETX);

            byte lrc = CalculateLRC(buffer.ToArray());
            buffer.Add(lrc);

            return new DeviceMessage(buffer.ToArray());
        }

        public static DeviceMessage BuildRequest(string messageId, params object[] elements)
        {
            var message = GetElementString(elements);
            logger.Debug("BuildRequest() : messageId=" + messageId + " ; Message = " + message);
            return BuildMessage(messageId, message);
        }

        public static byte CalculateLRC(byte[] buffer)
        {
            // account for LRC still being attached
            var length = buffer.Length;
            if (buffer[buffer.Length - 1] != (byte)ControlCodes.ETX)
                length--;

            byte lrc = new byte();
            for (int i = 1; i < length; i++)
                lrc = (byte)(lrc ^ buffer[i]);
            return lrc;
        }
    }
}
