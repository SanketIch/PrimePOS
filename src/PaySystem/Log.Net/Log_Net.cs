///<Copyright>
///No part of this DLL shall at anytime be copied or use without
///written permission from the Author (Manoj Kumar). This DLL is 
///intended to be use by the Author for his projects and whichever 
///company he works with on a project that it is used as a part of 
///that project.
///Copyright © 2015 by Manoj Kumar.
///</Copyright>

///<Author>
///<Name>Manoj Kumar</Name>
///<Email>mkumar19@gmail.com</Email>
///</Author>

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
using System.Reflection;


namespace Log.Net
{
    #region Enum Error Level and Body Type of the message
    /// <summary>
    /// Error Level, different type of logging
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>Regular Logging</summary>
        Logging = 0,
        /// <summary>Logging with warning</summary>
        Warning = 1,
        /// <summary>Logging Errors </summary>
        Critical = 2
    }

    /// <summary>
    /// Body Type of the log.
    /// Header - Start the log.
    /// Body - Regular logging.
    /// Footer - End of log.
    /// </summary>
    public enum BodyType
    {
        /// <summary>Header is the where you want to start the log</summary>
        Header = 0,
        /// <summary>Body is anything within the Header and Footer</summary>
        Body = 1,
        /// <summary>Footer should be where you want the log to end or the last line that should be in the log</summary>
        Footer = 2
    }
    #endregion Enum Error Level and Body Type of the message

    #region Queue class for queuing messages
    ///<summary>
    /// This class is use to write a log on a seperate Thread
    /// using Queue.  Configuration File is being read from
    /// LogNet.Config
    /// </summary>
    /// <Author>Author: Manoj Kumar</Author>
    internal class Log_Net
    {
        CultureInfo ci = CultureInfo.InvariantCulture;
         ///<summary>Get or Set Message to add to the Log</summary>
        internal string MsgData { get; set; }
        /// <summary>
        /// </summary>
        internal string MethodName { get; set; }
        internal ErrorLevel Level { get; set; }
        internal BodyType BodyType { get; set; }
         ///<summary>
         ///Get or Set Log Date for each log entry
         ///</summary>
        internal string LogDate { get; set; }
         ///<summary>
         ///Get or Set Log Time for each log entry
         ///</summary>
        internal string LogTime { get; set; }

         ///<summary>
         ///Log Message
         ///</summary>
         ///<param name="message"></param>
        internal Log_Net(BodyType Body, string message, string Method, ErrorLevel Level)
        {
            this.BodyType = Body;
            this.MsgData = message;
            this.MethodName = Method;
            this.Level = Level;
            this.LogDate = DateTime.Now.ToString("MM/dd/yyyy");
            this.LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt", ci);
        }
    }
    #endregion Queue class for queuing messages

    public class Logs
    {
        #region Variables
        static bool isConfig;
        static bool isWriting;
        static readonly object syncObj = new object();
        static readonly object DeqObj = new object();
        static Queue<Log_Net> logQueue = new Queue<Log_Net>();
        static string LogFile = string.Empty;
        static string RenameLogFile = string.Empty;
        static decimal SizeConst = 5M;
        delegate void EventLogHandler();
        static event EventLogHandler LogEvent;
        delegate void ErrorEventLog(string ex);
        static event ErrorEventLog ErrorLog;
        static Thread LogThread = null;
        #endregion Variables

        #region Get the excuting path of the DLL
        /// <summary>
        /// Check the Executing path for the Log Config file
        /// </summary>    
        private static string CheckConfigFile
        {
            get
            {
                string CodeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(CodeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        #endregion Get the excuting path of the DLL

        #region Logger, write the message from the calling fucntion and queue it
        /// <summary>
        /// Public function to write the log. All requests to write are being
        /// Queued and then Process by the Thread.
        /// </summary>
        /// <param name="msg"></param>
        public static void Logger(BodyType body, string msg, ErrorLevel level)
        {
            if (!isConfig)
            {
                LogFile = CheckConfigFile + "\\Activity.log";
                RenameLogFile = CheckConfigFile + "\\Activity.log";
                isConfig = true;
            }

            if (isConfig) // check is log is enable
            {
                //StackTrace stTrace = new StackTrace();
                //StackFrame stFrame = stTrace.GetFrame(1);
                //MethodBase mdBase = stFrame.GetMethod();

                string MethodName = ""; //"[" + mdBase.ReflectedType.Name + "." + mdBase.Name + "()]"; //Method Name and message pass

                lock (logQueue) //Lock so only one at a time 
                {
                    Log_Net logEntry = new Log_Net(body, msg, MethodName, level);
                    logQueue.Enqueue(logEntry); //Add to queue

                    if (logQueue.Count > 0 && !isWriting)
                    {
                        LogEvent += Logs_LogEvent; //event to tell the thread to start
                        LogEvent.Invoke();
                    }
                }
            }
        }
        #endregion Logger, write the message from the calling fucntion and queue it

        #region Writting the log to the file
        /// <summary>
        /// Write messsage to log file 
        /// </summary>
        /// <param name="data"></param>
        private static void WriteToLog()
        {
            try
            {
                lock (syncObj)
                {
                    while (logQueue.Count > 0)
                    {
                        if (!isWriting)
                        {
                            isWriting = true;
                            bool isEntry = false;
                            Log_Net entry = null;
                            do
                            {
                                try
                                {
                                    entry = logQueue.Dequeue();
                                    isEntry = true;
                                }
                                catch
                                {
                                    isEntry = false;
                                }
                            } while (!isEntry);

                            if (isEntry)
                            {
                                if (BodyType.Header == entry.BodyType)
                                {
                                    DeleteLog();//check to see if file reach max size to rename.
                                }

                                using (var outStream = new FileStream(LogFile, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                                {
                                    using (var Writer = new StreamWriter(outStream))
                                    {
                                        if (ErrorLevel.Logging == entry.Level)
                                        {
                                            if (BodyType.Header == entry.BodyType)
                                            {
                                                Writer.WriteLine("\n");
                                                Writer.WriteLine(string.Format("{0} {1}  {2}  {3}", entry.LogDate, entry.LogTime, "[START ENTRY] ", ""));
                                                Writer.WriteLine(string.Format("{0} {1}  {2}  {3}", entry.LogDate, entry.LogTime, "[INFO] " + entry.MsgData, entry.MethodName)); //Write the Data to the log
                                            }
                                            else if (BodyType.Footer == entry.BodyType)
                                            {
                                                Writer.WriteLine(string.Format("{0} {1}  {2}  {3}", entry.LogDate, entry.LogTime, "[INFO] " + entry.MsgData, entry.MethodName)); //Write the Data to the log
                                                Writer.WriteLine(string.Format("{0} {1}  {2}  {3}", entry.LogDate, entry.LogTime, "[END ENTRY] ", ""));
                                            }
                                            else
                                            {
                                                Writer.WriteLine(string.Format("{0} {1}  {2}  {3}", entry.LogDate, entry.LogTime, "[INFO] " + entry.MsgData, entry.MethodName)); //Write the Data to the log
                                            }
                                        }
                                        else if (ErrorLevel.Warning == entry.Level)
                                        {
                                            Writer.WriteLine(string.Format("{0} {1}  {2}  {3}", entry.LogDate, entry.LogTime, "[WARNING: ==>] " + entry.MsgData, entry.MethodName)); //Write the Data to the log
                                        }
                                        else if (ErrorLevel.Critical == entry.Level)
                                        {
                                            Writer.WriteLine(string.Format("{0} {1}  {2}  {3}", entry.LogDate, entry.LogTime, "[CRITICAL: ==>] " + entry.MsgData, entry.MethodName)); //Write the Data to the log                                       
                                        }
                                    }
                                }
                            }
                            isWriting = false;
                        }
                    }
                    if (logQueue.Count == 0)
                        logQueue.Clear();
                }
            }
            catch (NullReferenceException ex)
            {
                isWriting = false;
                ErrorLog += Logs_ErrorLog;
                ErrorLog.Invoke(ex.ToString());
            }
            catch (Exception ex)
            {
                isWriting = false;
                ErrorLog += Logs_ErrorLog;
                ErrorLog.Invoke(ex.ToString());
            }
        }
        #endregion Writting the log to the file

        #region Thread event to write the log
        /// <summary>
        /// Thread to write the Log. 
        /// </summary>
        private static void Logs_LogEvent()
        {
            LogEvent -= Logs_LogEvent; //remove event and start the thread
            if (LogThread == null ? true : !LogThread.IsAlive)
            {
                LogThread = new Thread(WriteToLog);
                LogThread.Start();
            }
        }
        #endregion Thread event to write the 

        #region Event if any error occur
        /// <summary>
        /// Event if error occur in log
        /// </summary>
        /// <param name="ex"></param>
        static void Logs_ErrorLog(string ex)
        {
            Logger(BodyType.Body, ex, ErrorLevel.Critical);
        }
        #endregion Event if any error occur

        #region Delete Log after a certain size that is in the Config
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
            catch (Exception)
            {
                Logger(BodyType.Body, "Unable to delete " + LogFile + " at the set size.  Will delete when POS restart.", ErrorLevel.Warning);
                SizeConst += 5; //increase the log size by 5mb for this instance until application close
            }
        }
        #endregion Delete Log after a certain size that is in the Config


    }
}
