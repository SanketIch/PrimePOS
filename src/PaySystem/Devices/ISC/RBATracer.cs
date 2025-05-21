using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using NLog;

namespace EDevice
{
    /// <summary>
    /// RBA Log. Log all event of RBA
    /// </summary>
    internal static class RBATracer
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #region Log for RBA, roll over after 5mb
        static object obj = new object();
        static decimal Size = 5.0M;
        static string logName = "ISC.log";
        static string rollName = "_ISC.log";
        static string rollOverName;
        static string sFileName;

        /// <summary>
        /// TraceLog log all event of RBA.  Pass each line
        /// </summary>
        /// <param name="line"></param>
        public static void TraceLog(string line)
        {
            logger.Trace(line);
            /*lock (obj)
            {
                sFileName = AssemblyPath + "\\" + logName;
                rollOverName = AssemblyPath + "\\" + rollName;

                if (!string.IsNullOrWhiteSpace(sFileName))
                {
                    if (!File.Exists(sFileName))
                    {
                        using (StreamWriter sw = File.CreateText(sFileName))
                        {
                            sw.WriteLine(line + "\n");
                            sw.Close();
                        }
                    }
                    else
                    {

                        using (StreamWriter sw = File.AppendText(sFileName))
                        {
                            sw.WriteLine(line + "\n");
                            sw.Close();
                        }
                    }
                }
            }*/
        }

        /// <summary>
        /// Roll over the log after 5MB
        /// </summary>
        private static void RollOverLog()
        {
            bool isExist = false;
            decimal lBytes = 0M;
            decimal lInMb = 0M;
            decimal lSize = 0M;
     
            try
            {
                if (!string.IsNullOrWhiteSpace(sFileName))
                {
                    FileInfo sLog = new FileInfo(sFileName);
                    if (File.Exists(sFileName))
                    {
                        isExist = true;
                    }

                    if (isExist)
                    {
                        lBytes = sLog.Length;
                        lInMb = (lBytes / (1024) / (1024));
                        lSize = decimal.Round(lInMb, 2, MidpointRounding.AwayFromZero);

                        /* Check if the log reach size to roll over to a new log */
                        if (lSize > Size)
                        {
                            if (File.Exists(rollOverName))
                            {
                                File.Delete(rollOverName); //delete old log
                            }
                            File.Copy(sFileName, rollOverName); 
                            File.Delete(sFileName);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Get the current directory path 
        /// </summary>
        private static string AssemblyPath
        {
            get
            {
                string path = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                return Path.GetDirectoryName(path);
            }
        }
        #endregion Log for RBA, roll over after 5mb
    }
}
