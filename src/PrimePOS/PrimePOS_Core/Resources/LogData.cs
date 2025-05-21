using System;
using System.IO;


namespace POS_Core.Resources
{
	/// <summary>
	/// Summary description for LogData.
	/// </summary>
	public class LogData
	{
		private LogData()
		{
		}
		public static void Log(String FileName,String LogMessage,bool AppendMode)
		{
			StreamWriter w;
			if (AppendMode==true)
			{
				if (File.Exists(FileName)==false)
				{
					w = File.CreateText(FileName);
				}
				w = File.AppendText(FileName);
			}
			else
			{
				w = File.CreateText(FileName);
			}
			Log (LogMessage, w);
			// Close the writer and underlying file.
			w.Close();
			// Open and read the file.
		}
		public static void Log (String logMessage, TextWriter w)
		{
			w.Write("\r\nLog Entry : ");
			w.WriteLine("  {0}", logMessage);
			w.Flush(); 
		}
		public static void OpenLogFile(String FileName)
		{
			System.Diagnostics.Process oProc=new System.Diagnostics.Process();
			System.Diagnostics.ProcessStartInfo sInfo=new System.Diagnostics.ProcessStartInfo("notepad.exe");
			sInfo.Arguments=System.Windows.Forms.Application.StartupPath + "\\" + FileName;
			sInfo.WindowStyle=System.Diagnostics.ProcessWindowStyle.Normal;
			sInfo.UseShellExecute=true;
			oProc.StartInfo=sInfo;
			oProc.Start();
		}
	}
}
