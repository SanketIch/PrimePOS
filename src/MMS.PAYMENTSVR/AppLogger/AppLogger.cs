//Author: Manoj Kumar
//Function: If the Enable trace is set to 'Y' write to C:\\PrimeCharge.log
//Date: 7/10/2012

using System;
using System.Collections.Generic;
using System.Text;
//using MMS.PROCESSOR;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace AppLogger
{
    public class AppLogger
    {
        /// <summary>
        /// Write Log to pc. Use for tracking what is happening in the payments
        /// </summary>
 
        static ExeConfigurationFileMap LogSettingMap = null;
        static AppSettingsSection appSettings = null;
        static Configuration LogSetting = null;
        static bool isConfig = false;

        private static void ReadConfig()
        {
            LogSettingMap = new ExeConfigurationFileMap();
            LogSettingMap.ExeConfigFilename = "PaymentLog.config";
            LogSetting = ConfigurationManager.OpenMappedExeConfiguration(LogSettingMap, ConfigurationUserLevel.None);
            appSettings = (AppSettingsSection) LogSetting.GetSection("appSettings");
        }

        /// <summary>
        /// Take the Data to write in the log.
        /// </summary>
        /// <param name="obj"></param>
        public static void LogWritter(Object obj)
        {
            try
            {
                if(!isConfig)
                {
                    ReadConfig();
                    isConfig = true;
                }

                if(isConfig && Convert.ToBoolean(appSettings.Settings["LogEnable"].Value.ToString())) // check is log is enable
                {
                    if (obj.ToString().StartsWith("--")) // All function begin with '--' as a header
                    {
                        DeleteLog(); // Check if the file size is more than 10MB, delete, if not continue writing to log
                        WriteToLog("\n"); //New line for each Payment type. Credit, debit etc.
                        WriteToLog(DateTime.Now + " " + obj.ToString().Substring(2)); //Log will have the current Date + the Log msg
                    }
                    else if (obj.ToString().StartsWith("**")) //Mean this is an error message. All error start with '**'
                    {
                        WriteToLog("--->>" + DateTime.Now + " " + obj.ToString()); //Show the error in the Log
                    }
                    else
                    {
                        WriteToLog(DateTime.Now + " " + obj.ToString()); //Write each entry into the log for that transaction.
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString()); 
            }
        }

        /// <summary>
        /// Rename and delete log file when it certain size
        /// </summary>
        private static void DeleteLog()
        {
            string LogFile = string.Empty;
            LogFile = appSettings.Settings["LogPath"].Value.ToString(); //Original file
            string RenameLogFile = string.Empty;
            RenameLogFile = appSettings.Settings["LogRename"].Value.ToString(); //After 10mb rename to this
            decimal LogInBytes = 0M;
            decimal LogInMb = 0M;
            decimal LogSize = 0M;
            decimal SizeConst = 0M;
            SizeConst = Convert.ToDecimal(appSettings.Settings["LogSize"].Value.ToString()); //Constant size to keep
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
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Write messsage to log file
        /// </summary>
        /// <param name="data"></param>
        private static void WriteToLog(string data)
        {
            string LogFile = string.Empty;
            LogFile = appSettings.Settings["LogPath"].Value.ToString(); // get data from log.config
            try
            {
                FileStream outStream = null;
                StreamWriter Writer = null;

                using(outStream = new FileStream(LogFile, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                {
                    using(Writer = new StreamWriter(outStream))
                    {
                        Writer.WriteLine(data);
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
