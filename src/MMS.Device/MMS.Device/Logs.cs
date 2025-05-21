using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace MMS.Device
{
    /// <summary>
    /// This class is use to write Device log on a seperate Thread
    /// using Queue.  Configuration File is being read from
    /// Device.Config
    /// </summary>
    /// <Author>Author: Manoj Kumar</Author>
    
    #region Properities - Log Class Message, LogDate, LogTime
    public class Log
    {
        CultureInfo ci = CultureInfo.InvariantCulture;
        /// <summary>
        /// Get or Set Message to add to the Log
        /// </summary>
        public string MsgData
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Log Date for each log entry
        /// </summary>
        public string LogDate
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Log Time for each log entry
        /// </summary>
        public string LogTime
        {
            get;
            set;
        }

        /// <summary>
        /// Log Message
        /// </summary>
        /// <param name="message"></param>
        public Log(string message)
        {
            MsgData = message;
            LogDate = DateTime.Now.ToString("MM/dd/yyyy");
            LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt",ci);
        }
    }
    #endregion

    public class Logs
    {
        #region Variables
        static ExeConfigurationFileMap LogSettingMap = null;
        static AppSettingsSection appSettings = null;
        static Configuration LogSetting = null;
        static bool isConfig = false;
        static bool isLogEnable = false;
        static bool isWriting = false;
        static readonly object syncObj = new object();
        static readonly object DeqObj = new object();
        static Queue<Log> logQueue = new Queue<Log>();
        static string LogFile = string.Empty;
        static string RenameLogFile = string.Empty;
        static decimal SizeConst = 0M;
        delegate void EventLogHandler();
        static event EventLogHandler LogEvent;
        delegate void ErrorEventLog(string ex);
        static event ErrorEventLog ErrorLog;
        static Thread LogThread = null;
        static bool isDeviceHeader = false;
        #endregion

        /// <summary>
        /// Read the Device.Config file.  This is use to get setting information.
        /// </summary>
        private static void ReadConfig()
        {
            LogSettingMap = new ExeConfigurationFileMap();
            LogSettingMap.ExeConfigFilename = "Device.config";
            LogSetting = ConfigurationManager.OpenMappedExeConfiguration(LogSettingMap,ConfigurationUserLevel.None); 
            appSettings = (AppSettingsSection)LogSetting.GetSection("appSettings");
            LogFile = appSettings.Settings["LogPath"].Value.ToString();
            isLogEnable = Convert.ToBoolean(appSettings.Settings["DeviceLogEnable"].Value.ToString());
            RenameLogFile = appSettings.Settings["LogRename"].Value.ToString(); //New File name in config file, rename it.
            SizeConst = Convert.ToDecimal(appSettings.Settings["LogSize"].Value.ToString()); //Constant size to keep
            Constant.WaitTime = Convert.ToInt32(appSettings.Settings["WaitTime"].Value.ToString()); //Get the wait time.
            Constant.DisplayErrorCode = Convert.ToBoolean(appSettings.Settings["DisplayReturnCode"].Value.ToString());
            Constant.ShowPaymentScreen = Convert.ToBoolean(appSettings.Settings["ShowPayScreen"].Value.ToString());//Show or not to show PADSHOWCASH Screen
        }

        /// <summary>
        /// Public function to write the log. All requests to write are being
        /// Queued and then Process by the Thread.
        /// </summary>
        /// <param name="msg"></param>
        public static void Logger(string msg)
        {
            if(!isConfig)
            {
                try
                {
                    ReadConfig(); //Read from the Device.config file
                    isConfig = true;
                }
                catch(Exception ex)
                {
                    isConfig = false;
                    throw new Exception(ex.ToString());
                }
            }

            if(isConfig && isLogEnable) // check is log is enable
            {
                lock(logQueue) //Lock so only one at a time 
                {
                    Log logEntry = new Log(msg);
                    logQueue.Enqueue(logEntry); //Add to queue

                    if(logQueue.Count > 0)
                    {
                        if (LogThread == null ? true : !LogThread.IsAlive)
                        {
                            LogThread = new Thread(WriteToLog);
                            LogThread.Start();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Write messsage to log file 
        /// </summary>
        /// <param name="data"></param>
        private static void WriteToLog()
        {
            try
            {
                lock(syncObj)
                {
                    while(logQueue.Count > 0)
                    {
                        if(!isWriting)
                        {
                            isWriting = true;
                            bool isEntry = false;
                            Log entry = null;
                            try
                            {
                                entry = logQueue.Dequeue();
                                if(string.IsNullOrEmpty(entry.MsgData.Trim()))
                                {
                                    entry.MsgData = "No Data to write";//Just incase the data is empty 
                                }
                                isEntry = true;
                            }
                            catch
                            {
                                isEntry = false;
                            }


                            if(isEntry)
                            {
                                if(entry.MsgData.ToUpper().Contains("ENTERING STARTTXN") || entry.MsgData.ToUpper().Contains("INITALIZE"))
                                {
                                    DeleteLog(); //check to see if file reach max size to rename.
                                }

                                using(var outStream = new FileStream(LogFile, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                                {
                                    using(var Writer = new StreamWriter(outStream))
                                    {
                                        if(entry.MsgData.ToUpper().Contains("ENTERING STARTTXN"))
                                        {
                                            Writer.WriteLine("\n");
                                            Writer.WriteLine(string.Format("{0} {1}  {2}", entry.LogDate, entry.LogTime, entry.MsgData)); //Write the Data to the log
                                        }
                                        else if(entry.MsgData.ToUpper().Contains("INITALIZE"))
                                        {
                                            Writer.WriteLine("\n\n <======================= DEVICE STARTED =======================>");
                                            Writer.WriteLine(string.Format("{0} {1}  {2}", entry.LogDate, entry.LogTime, entry.MsgData)); //Write the Data to the log
                                        }
                                        else if(entry.MsgData.ToUpper().Contains("ERROR IN"))
                                        {
                                            Writer.WriteLine(string.Format("{0} {1}  {2}", entry.LogDate, entry.LogTime, "====>> " + entry.MsgData)); //Write the Data to the log
                                        }
                                        else if(entry.MsgData.ToUpper().Contains("DEVICE INFORMATION"))
                                        {
                                            if(!isDeviceHeader)
                                            {
                                                isDeviceHeader = true;
                                                Writer.WriteLine("\n");
                                                Writer.WriteLine(string.Format("{0} {1}  {2}", entry.LogDate, entry.LogTime, entry.MsgData)); //Write the Data to the log
                                            }
                                            else
                                            {
                                                Writer.WriteLine(string.Format("{0} {1}  {2}", entry.LogDate, entry.LogTime, entry.MsgData)); //Write the Data to the log
                                                Writer.WriteLine("\n");
                                                isDeviceHeader = false;
                                            }
                                        }
                                        else
                                        {
                                            Writer.WriteLine(string.Format("{0} {1}  {2}", entry.LogDate, entry.LogTime, entry.MsgData)); //Write the Data to the log
                                        }
                                    }
                                }
                            }                                
                            isWriting = false;
                        }
                    }
                    if(logQueue.Count == 0)
                        logQueue.Clear();
                }
            }
            catch(NullReferenceException ex)
            {
                isWriting = false;
                ErrorLog += Logs_ErrorLog;
                ErrorLog.Invoke(ex.ToString());
            }
            catch(Exception ex)
            {
                isWriting = false;
                ErrorLog += Logs_ErrorLog;
                ErrorLog.Invoke(ex.ToString());
            }
        }

        static void Logs_ErrorLog(string ex)
        {
            Logger("Error In "+ex);
        }

        /// <summary>
        /// Rename and delete log file when it certain size
        /// </summary>
        private static void DeleteLog()
        {
            decimal LogInBytes = 0M;
            decimal LogInMb = 0M;
            decimal LogSize = 0M;

            bool isFileExist = false;
            try
            {
                FileInfo log = new FileInfo(LogFile);
                if(File.Exists(LogFile))
                {
                    isFileExist = true; // If LogFile exist
                }

                if(isFileExist) // If file is there
                {
                    LogInBytes = log.Length; // Get the size of the file
                    LogInMb = (LogInBytes / (1024) / (1024)); // Change from bytes to MB
                    LogSize = decimal.Round(LogInMb, 2, MidpointRounding.AwayFromZero); // Round off to 2 decimals

                    if(LogSize > SizeConst) //If LogFile is larger than the constant size you want to keep
                    {
                        if(File.Exists(RenameLogFile)) // Check if the Rename LogFile exist
                        {
                            File.Delete(RenameLogFile); // Delete the renameLogFile if one is there already
                        }

                        File.Copy(LogFile, RenameLogFile); // Create a renamed LogFile => _LogFile
                        File.Delete(LogFile); // Delete the original LogFile because you have a copy.
                    }
                }
            }
            catch(Exception)
            {
                Logger("Unable to delete PosPadHost.log at the set size.  Will delete when POS restart.");
                SizeConst += 5;
            }
        }
    }
}
