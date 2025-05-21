using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AppStarter
{
    public class clsLogService
    {
        StreamWriter writer = null;

        public clsLogService(string LogName)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path,LogName);
            writer = new StreamWriter(path, false);
        }

        //public clsLogService(string Diff)
        //{
        //    string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //    path = Path.Combine(path, "setup(silent).log");
        //    writer = new StreamWriter(path, false);
        //}

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
            writer.WriteLine("Event Type: " + type);
            writer.WriteLine(String.Format("Time: {0}{1}Description: {2}", DateTime.Now.ToShortTimeString(), Environment.NewLine, message));
            writer.WriteLine("--------------------------------------------------");
            writer.WriteLine();
        }

        public void Close()
        {
            writer.Close();
        }

        public string ReadLog(string sFilepath)
        {
            string FileDate = string.Empty;
            if (System.IO.File.Exists(sFilepath))
            {
                StreamReader Reader = new StreamReader(sFilepath);
                FileDate = Reader.ReadToEnd();
            }
            return FileDate;
        }
    }
}
