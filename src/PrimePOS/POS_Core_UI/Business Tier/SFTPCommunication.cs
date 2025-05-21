using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebex.Net;
using Rebex.Legacy;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using POS_Core.CommonData;

namespace POS_Core.BusinessRules
{
    public static class SFTPCommunication
    {
        public static bool Send(string strFTPAddress, string strFTPUserId, string strFTPPassword, Int32 nPort, string strFTPFolderPath, string strCSVFilePath, string strCSVFileName)
        {
            POS_Core.ErrorLogging.Logs.Logger("SFTPCommunication", "Send() - sFTP", clsPOSDBConstants.Log_Entering);
            String localFilePath = null;
            String remoteFilePath = null;
             Sftp ftpSend = new Sftp();
            try
            {
                localFilePath = strCSVFilePath; //path with file name
                if (!strFTPFolderPath.StartsWith("/"))
                    strFTPFolderPath = "/" + strFTPFolderPath;

                remoteFilePath = strFTPFolderPath + "/" + strCSVFileName;
                //ftpSend.Timeout = 1000 * 12 * 60;
                IAsyncResult ar = ftpSend.BeginConnect(strFTPAddress, nPort, null, null);
                while (!ar.IsCompleted)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                }
                ftpSend.EndConnect(ar);
                ftpSend.Login(strFTPUserId, strFTPPassword);
                ftpSend.ChangeDirectory(strFTPFolderPath);

                //Put the file in the requried path.
                long lBytesTransferred = ftpSend.PutFile(localFilePath, remoteFilePath);
                return true;
            }
            catch (System.Net.Sockets.SocketException exp)
            {
                POS_Core.ErrorLogging.Logs.Logger("SFTPCommunication", "Send() - sFTP Sending fail1 - " + localFilePath + " - " + exp.ToString(), clsPOSDBConstants.Log_Exiting);
                return false;
            }
            catch (System.Exception ex1)
            {
                POS_Core.ErrorLogging.Logs.Logger("SFTPCommunication", "Send() - sFTP Sending fail2 - " + localFilePath + " - " + ex1.ToString(), clsPOSDBConstants.Log_Exiting);
                return false;
            }
            finally
            {
                if (ftpSend != null)
                {
                    ftpSend.Disconnect();
                    ftpSend.Dispose();
                }
            }
        }
    }
}