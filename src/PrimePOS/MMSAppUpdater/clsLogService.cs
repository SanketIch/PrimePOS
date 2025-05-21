using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MMSAppUpdater
{    
   public class clsLogService
    {
        StreamWriter _writer = null;
        string path = "";

        public StreamWriter writer
        {
           get
           {
               if (_writer == null || _writer.BaseStream == null)
               {
                   string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                   path = Path.Combine(path, "AutoUpdatePOS.log");
                     _writer = new StreamWriter(path, true);
               }
               return _writer;
           }
           set { _writer = value; }
        }
       
        public clsLogService()
        {
             path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
             path = Path.Combine(path, "AutoUpdatePOS.log");
            _writer = new StreamWriter(path, true);
            writer.Close();
        }

        public clsLogService(string Diff)
        {
            path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, Diff);
            writer = new StreamWriter(path, true);
            writer.Close();
        }

        public void AddError(string log)
        {
            WriteLog(log, "ERROR");
        }

        public void AddMessage(string log)
        {
            WriteLog(log, "MESSAGE");
        }

        private void WriteLog(string message, string type)
        {
            try
            {
                lock (_writer)
                {
                    #region JY 23-Oct-2015 delete file if exceeds size by 2 mb
                    FileInfo fileInfo = null;
                    StreamWriter fs = null;
                    try
                    {
                        fileInfo = new FileInfo(path);
                        long fsize = fileInfo.Length;
                        if (fileInfo.Length > 10485760) //Set the max file size limit to 10 mb (10485760 bytes) for log file(s) 
                        {
                            File.Delete(path);
                            fs = new StreamWriter(path);
                            fileInfo = new FileInfo(path);
                            fs.Close();
                        }
                    }
                    catch
                    { }
                    #endregion

                    _writer = new StreamWriter(path, true);

                    writer.WriteLine("Event Type: " + type);
                    writer.WriteLine(String.Format("Time: {0}{1}Description: {2}", DateTime.Now.ToString(), Environment.NewLine, message));    //JY 23-Oct-2015 changed logic to record datetime instead of only time
                    writer.WriteLine("--------------------------------------------------");
                    writer.WriteLine();
                    writer.Close();
                }
            }
            catch
            {
            }
        }

        public void Close()
        {
            try
            {
                writer.Close();
            }
            catch
            { }
        }
    }
}
