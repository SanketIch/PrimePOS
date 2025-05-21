using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using System.Security.AccessControl;
using POS_Core.Resources;

namespace POS_Core.ErrorLogging
{
    public class Logs
    {
        static string LogFileFolderName = "PrimePOSLog";
        static string LogFile = string.Empty;
        static string LogFileFolder = string.Empty;
        static string RenameLogFile = string.Empty;
        static string data = string.Empty;

        public static void Init()
        {
            LogFile = Configuration.POSWDirectoryPath + "\\" + Logs.LogFileFolderName + "\\" + POS_Core.Resources.Configuration.StationName + ".log";
            LogFileFolder = Configuration.POSWDirectoryPath + "\\" + Logs.LogFileFolderName;
            RenameLogFile = Configuration.POSWDirectoryPath + "\\" + Logs.LogFileFolderName + "\\_" + POS_Core.Resources.Configuration.StationName + ".log";
        }

        public static void Logger(object Module, object Function, Object Operation)
        {
            try
            {
                if (Configuration.CPOSSet.TurnOnEventLog == false)
                {
                    return;
                }
                object obj = POS_Core.Resources.Configuration.convertNullToString(Module) + " \t" + POS_Core.Resources.Configuration.convertNullToString(Function) + "\t" + POS_Core.Resources.Configuration.convertNullToString(Operation);
                data = System.DateTime.Now + "\t" + Configuration.convertNullToString(obj);
                WriteToLog();
            }
            catch (Exception ex)
            {
                try
                {
                    ErrorHandler.SaveLog((int)POS_Core.CommonData.LogENUM.Exception_Occured, POS_Core.Resources.Configuration.UserName, "FAIL", ex.ToString() + ex.StackTrace.ToString());
                }
                catch
                {

                }
            }
        }

        public static void Logger(object obj)
        {
            try
            {
                if (Configuration.CPOSSet.TurnOnEventLog == false)
                {
                    return;
                }
                data = System.DateTime.Now + "\t" + Configuration.convertNullToString(obj);
                WriteToLog();
            }
            catch (Exception ex)
            {
                try
                {
                    ErrorHandler.SaveLog((int)POS_Core.CommonData.LogENUM.Exception_Occured, POS_Core.Resources.Configuration.UserName, "FAIL", ex.ToString() + ex.StackTrace.ToString());
                }
                catch
                {

                }
            }
        }

        private static void WriteToLog()
        {
            try
            {
                if (!(LogFile != null && LogFile.Trim() != "")) return; //PRIMEPOS-2651 07-Mar-2022 JY Added
                DeleteLog();
                //string LogFile = @"C:\PrimePOSLog\PrimePosEventsLog.log";                
                FileInfo log = new FileInfo(LogFile);
                if (!Directory.Exists(LogFileFolder))
                {
                    System.IO.Directory.CreateDirectory(LogFileFolder); // create folder if not exist.
                }
                FileStream outStream = null;
                StreamWriter Writer = null;
                FileSecurity security = new FileSecurity();
                FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, LogFile);
                permission.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, LogFile);
                permission.Demand();

                using (outStream = new FileStream(LogFile, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                {

                    using (Writer = new StreamWriter(outStream))
                    {
                        Writer.WriteLine(data);
                    }
                }

            }
            catch (IOException Ex)
            {
                throw Ex;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
       
        private static void DeleteLog()
        {
            //string LogFile = @"C:\PrimePOSLog\PrimePosEventsLog.log";//Original file
            //string RenameLogFile = @"C:\PrimePOSLog\_PrimePosEventsLog.log"; //After 10mb rename to this
            decimal LogInBytes = 0M;
            decimal LogInMb = 0M;
            decimal LogSize = 0M;
            decimal SizeConst = 20M; //Constant size to keep
            bool isFileExist = false;
            try
            {
                FileInfo log = new FileInfo(LogFile);
                if (File.Exists(LogFile))
                {
                    isFileExist = true; // If LogFile exist
                }

                if (isFileExist) // If file is there
                {
                    LogInBytes = log.Length; // Get the size of the file
                    LogInMb = (LogInBytes / (1024) / (1024)); // Change from bytes to MB
                    LogSize = decimal.Round(LogInMb, 2, MidpointRounding.AwayFromZero); // Round off to 2 decimals

                    if (LogSize > SizeConst) //If LogFile is larger than the constant size you want to keep
                    {
                        if (File.Exists(RenameLogFile)) // Check if the Rename LogFile exist
                        {
                            File.Delete(RenameLogFile); // Delete the renameLogFile if one is there already
                        }

                        File.Copy(LogFile, RenameLogFile); // Create a renamed LogFile => _LogFile
                        File.Delete(LogFile); // Delete the original LogFile because you have a copy.
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
