using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace Resources
{
    public class LogData
    {
        private const string LOGENABLE = "LOGENABLE";

        public static bool DeleteFTPFile = false;

        public static bool DeleteOldFTPFile = true;

        public static string ExceptionLog = "Exception";

        public static string ThroughTheFlow = "DataInfo";

        private static string exceptionLogFileName = string.Empty;

        private static string PoLogFileName = string.Empty;

        private static readonly object locker = new object();

        private IDbConnection conn;

        public static bool isLogEnable = false;


        public LogData()
        {
            try
            {
                this.conn = DataFactory.CreateConnection(DBConfig.ConnectionString);
                string commandText = "select LOGENABLE from PoUtil ";
                IDbCommand dbCommand = DataFactory.CreateCommand();
                dbCommand.CommandType = CommandType.Text;
                dbCommand.CommandText = commandText;
                dbCommand.Connection = this.conn;
                bool flag = (bool)dbCommand.ExecuteScalar();
                LogData.isLogEnable = flag;
            }
            catch (Exception ex)
            {
                LogData.Write(ex.ToString(), LogData.ExceptionLog, 0, 1, TraceEventType.Critical);
            }
        }

        public static void Write(string message, string category)
        {
            string str = string.Empty;
            if (LogData.isLogEnable || category == "Exception")
            {
                StackTrace stackTrace = new StackTrace(true);
                str = LogData.BuildStackTrace(stackTrace);
                str = message + str;
                try
                {
                    lock (LogData.locker)
                    {
                        if (LogData.exceptionLogFileName == "" || LogData.PoLogFileName == "")
                        {
                            LogData.GetLogFileName();
                        }
                        FileInfo fileInfo;
                        if (category.ToString() == "Exception")
                        {
                            fileInfo = new FileInfo(LogData.exceptionLogFileName);
                        }
                        else
                        {
                            fileInfo = new FileInfo(LogData.PoLogFileName);
                        }
                        StreamWriter streamWriter;
                        if (!fileInfo.Exists)
                        {
                            if (category.ToString().Contains("Exc"))
                            {
                                streamWriter = new StreamWriter(LogData.exceptionLogFileName);
                                fileInfo = new FileInfo(LogData.exceptionLogFileName);
                            }
                            else
                            {
                                streamWriter = new StreamWriter(LogData.PoLogFileName);
                                fileInfo = new FileInfo(LogData.PoLogFileName);
                            }
                            streamWriter.Close();
                        }
                        long length = fileInfo.Length;
                        if (fileInfo.Length > 300000000L && fileInfo.Name.Contains("Exc"))
                        {
                            File.Delete(LogData.exceptionLogFileName);
                            streamWriter = new StreamWriter(LogData.exceptionLogFileName);
                            fileInfo = new FileInfo(LogData.exceptionLogFileName);
                            streamWriter.Close();
                        }
                        else if (fileInfo.Length > 300000000L && fileInfo.Name.Contains("Flow"))
                        {
                            File.Delete(LogData.PoLogFileName);
                            streamWriter = new StreamWriter(LogData.PoLogFileName);
                            fileInfo = new FileInfo(LogData.PoLogFileName);
                            streamWriter.Close();
                        }
                        if (category.ToString().Contains("Exc"))
                        {
                            streamWriter = new StreamWriter(LogData.exceptionLogFileName, true);
                        }
                        else
                        {
                            streamWriter = new StreamWriter(LogData.PoLogFileName, true);
                        }
                        streamWriter.WriteLine("-----------------------------------------------------");
                        streamWriter.WriteLine("-----------------------------------------------------");
                        streamWriter.WriteLine("");
                        streamWriter.WriteLine("Timestamp :" + DateTime.Now.ToString());
                        streamWriter.WriteLine(message);
                        streamWriter.WriteLine("Category :" + category.ToString());
                        streamWriter.WriteLine("Machine  :" + Environment.MachineName);
                        string value = "Process ID :" + Process.GetCurrentProcess().Id;
                        streamWriter.WriteLine(value);
                        Process currentProcess = Process.GetCurrentProcess();
                        value = "Process Name : " + Process.GetCurrentProcess().ProcessName.ToString();
                        string text = Path.GetDirectoryName(Application.ExecutablePath);
                        text += "\\POServer.exe";
                        streamWriter.WriteLine("Process Name : " + text);
                        streamWriter.Close();
                    }
                }
                catch (Exception var_8_30D)
                {
                }
            }
        }

        public static void Write(string message, string category, int priority, int eventId, TraceEventType transEventType)
        {
            string str = string.Empty;
            StackTrace stackTrace = new StackTrace(true);
            str = LogData.BuildStackTrace(stackTrace);
            str = message + str;
            if (message.Contains("forcibly"))
            {
                ConfigurationSection configurationSection = (ConfigurationSection)System.Configuration.ConfigurationManager.GetSection("loggingConfiguration");
            }
            if (LogData.exceptionLogFileName == "" || LogData.PoLogFileName == "")
            {
                LogData.GetLogFileName();
            }
            try
            {
                lock (LogData.locker)
                {
                    FileInfo fileInfo;
                    if (category.ToString() == "Exception")
                    {
                        fileInfo = new FileInfo(LogData.exceptionLogFileName);
                    }
                    else
                    {
                        fileInfo = new FileInfo(LogData.PoLogFileName);
                    }
                    StreamWriter streamWriter;
                    if (!fileInfo.Exists)
                    {
                        if (category.ToString().Contains("Exc"))
                        {
                            streamWriter = new StreamWriter(LogData.exceptionLogFileName);
                            fileInfo = new FileInfo(LogData.exceptionLogFileName);
                        }
                        else
                        {
                            streamWriter = new StreamWriter(LogData.PoLogFileName);
                            fileInfo = new FileInfo(LogData.PoLogFileName);
                        }
                        streamWriter.Close();
                    }
                    long length = fileInfo.Length;
                    if (fileInfo.Length > 300000000L && fileInfo.Name.Contains("Exc"))
                    {
                        try
                        {
                            File.Delete(LogData.exceptionLogFileName);
                            streamWriter = new StreamWriter(LogData.exceptionLogFileName);
                            fileInfo = new FileInfo(LogData.exceptionLogFileName);
                            streamWriter.Close();
                        }
                        catch (Exception var_6_185)
                        {
                        }
                        File.Delete(LogData.exceptionLogFileName);
                        streamWriter = new StreamWriter(LogData.exceptionLogFileName);
                        fileInfo = new FileInfo(LogData.exceptionLogFileName);
                        streamWriter.Close();
                    }
                    if (category.ToString().Contains("Exc"))
                    {
                        streamWriter = new StreamWriter(LogData.exceptionLogFileName, true);
                    }
                    else
                    {
                        streamWriter = new StreamWriter(LogData.PoLogFileName, true);
                    }
                    streamWriter.WriteLine("-----------------------------------------------------");
                    streamWriter.WriteLine("-----------------------------------------------------");
                    streamWriter.WriteLine("");
                    streamWriter.WriteLine("Timestamp :" + DateTime.Now.ToString());
                    streamWriter.WriteLine(message);
                    streamWriter.WriteLine("Severity :" + transEventType.ToString());
                    streamWriter.WriteLine("Priority :" + priority.ToString());
                    streamWriter.WriteLine("EventId  :" + eventId.ToString());
                    streamWriter.WriteLine("Category :" + category.ToString());
                    streamWriter.WriteLine("priority :" + priority.ToString());
                    streamWriter.WriteLine("Machine  :" + Environment.MachineName);
                    string value = "Process ID :" + Process.GetCurrentProcess().Id;
                    streamWriter.WriteLine(value);
                    Process currentProcess = Process.GetCurrentProcess();
                    value = "Process Name : " + Process.GetCurrentProcess().ProcessName.ToString();
                    string text = Path.GetDirectoryName(Application.ExecutablePath);
                    text += "\\POServer.exe";
                    streamWriter.WriteLine("Process Name : " + text);
                    streamWriter.WriteLine("");
                    streamWriter.Close();
                }
            }
            catch (Exception var_6_185)
            {
            }
        }

        public static void GetLogFileName()
        {
            try
            {
                LogSource logSource = new LogSource("BAC");
                System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                LoggingSettings loggingSettings = (LoggingSettings)configuration.GetSection("loggingConfiguration");
                FlatFileTraceListenerData flatFileTraceListenerData = new FlatFileTraceListenerData();
                TraceListenerDataCollection traceListeners = loggingSettings.TraceListeners;
                foreach (TraceListenerData current in traceListeners)
                {
                    flatFileTraceListenerData = (FlatFileTraceListenerData)current;
                    if (flatFileTraceListenerData.FileName.Contains("PrimePOException") && LogData.exceptionLogFileName == "")
                    {
                        LogData.exceptionLogFileName = flatFileTraceListenerData.FileName;
                    }
                    else if (LogData.PoLogFileName == "")
                    {
                        LogData.PoLogFileName = flatFileTraceListenerData.FileName;
                    }
                }
            }
            catch (Exception var_6_D6)
            {
            }
        }

        private static string BuildStackTrace(StackTrace stackTrace)
        {
            string text = string.Empty;
            StackFrame frame = stackTrace.GetFrame(1);
            text = text + "\r\nLog in Method :" + frame.GetMethod().Name;
            text = text + "\r\nFile:" + frame.GetFileName();
            text = text + "\r\nLine Number:" + frame.GetFileLineNumber();
            frame = stackTrace.GetFrame(2);
            text = text + "\r\nCalling in Method :" + frame.GetMethod().Name;
            text = text + "\r\nFile:" + frame.GetFileName();
            return text + "\r\nLine Number:" + frame.GetFileLineNumber();
        }
    }
}

