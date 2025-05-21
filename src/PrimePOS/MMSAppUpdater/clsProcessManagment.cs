using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MMSAppUpdater
{
 [StructLayout(LayoutKind.Sequential)]
public struct PROCESS_BASIC_INFORMATION
{
  public int ExitStatus;
  public int PebBaseAddress;
  public int AffinityMask;
  public int BasePriority;
  public uint UniqueProcessId;
  public uint InheritedFromUniqueProcessId;
}


  public  class clsProcessManagment
    {
        [DllImport("kernel32.dll")]
        static extern bool TerminateProcess(uint hProcess, int exitCode);
        [DllImport("ntdll.dll")]
        static extern int NtQueryInformationProcess(
           IntPtr hProcess,
           int processInformationClass /* 0 */,
           ref PROCESS_BASIC_INFORMATION processBasicInformation,
           uint processInformationLength,
           out uint returnLength
        );
        /// <summary>/// Terminate a process tree/// 
        /// </summary>/// 
        /// <param name="hProcess">The handle of the process</param>/// 
        /// <param name="processID">The ID of the process</param>/// 
        /// <param name="exitCode">The exit code of the process</param>
        public void TerminateProcessTree(IntPtr hProcess, uint processID, int exitCode)
        {
            // Retrieve all processes on the system
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                // Get some basic information about the process
                PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
                try
                {
                    uint bytesWritten;
                    NtQueryInformationProcess(p.Handle,
                      0, ref pbi, (uint)Marshal.SizeOf(pbi),
                      out bytesWritten); // == 0 is OK

                    // Is it a child process of the process we're trying to terminate?
                    if (pbi.InheritedFromUniqueProcessId == processID)
                        // The terminate the child process and its child processes
                        TerminateProcessTree(p.Handle, pbi.UniqueProcessId, exitCode);
                }
                catch (Exception /* ex */)
                {
                    // Ignore, most likely 'Access Denied'
                }
            }

            // Finally, termine the process itself:
            TerminateProcess((uint)hProcess, exitCode);
        }

        /// <summary>/// Terminate a process tree/// 
        /// </summary>/// 
        /// <param name="hProcess">The handle of the process</param>/// 
        /// <param name="processID">The ID of the process</param>/// 
        /// <param name="exitCode">The exit code of the process</param>
        /// <param name="sExceptServieList">The Process which Should not be closed abc,exy etc </param>
        public void TerminateProcessTree(IntPtr hProcess, uint processID, int exitCode,string sExceptServieList)
        {
            // Retrieve all processes on the system
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if(sExceptServieList.Contains(p.ProcessName))
                {
                    continue;
                }
                // Get some basic information about the process
                PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
                try
                {
                    uint bytesWritten;
                    NtQueryInformationProcess(p.Handle,
                      0, ref pbi, (uint)Marshal.SizeOf(pbi),
                      out bytesWritten); // == 0 is OK

                    // Is it a child process of the process we're trying to terminate?
                    if (pbi.InheritedFromUniqueProcessId == processID)
                        // The terminate the child process and its child processes
                        TerminateProcessTree(p.Handle, pbi.UniqueProcessId, exitCode);
                }
                catch (Exception /* ex */)
                {
                    // Ignore, most likely 'Access Denied'
                }
            }

            // Finally, termine the process itself:
            TerminateProcess((uint)hProcess, exitCode);
        }

        public bool IsAppRunning(string AppName)
        {
            Process[] ProcessList;
            try
            {
                ProcessList = Process.GetProcessesByName(AppName);
            }
            catch (Exception)
            {
                return false;
            }
            if (ProcessList != null && ProcessList.Length > 0)
                return true;
            else
                return false;
        }

        public void StopService(clsAppUpdate oApp)
        {
            String ServiceName = oApp.ExeName.Substring(0, oApp.ExeName.IndexOf('.'));
            Process[] ProcessList = Process.GetProcessesByName(ServiceName);
            if (ProcessList != null && ProcessList.Length > 0)
            {
                foreach (Process pro in ProcessList)
                {
                    int exiteCode = 0;
                    int ProecessId = pro.Id;
                    IntPtr ProHandler = pro.Handle;

                    pro.Kill();
                    if (pro.HasExited)
                    {
                        exiteCode = pro.ExitCode;
                    }

                    TerminateProcessTree(ProHandler, (uint)ProecessId, exiteCode);


                }

            }

        }
      public void KillService(clsAppUpdate oApp)
      {

          String ServiceName = oApp.ExeName.Substring(0, oApp.ExeName.IndexOf('.'));
          Process[] ProcessList = Process.GetProcessesByName(ServiceName);
          if (ProcessList != null && ProcessList.Length > 0)
          {
              foreach (Process pro in ProcessList)
              {
                  int exiteCode = 0;
                  int ProecessId = pro.Id;
                  IntPtr ProHandler = pro.Handle;
                  pro.Kill();
                 
              }

          }

      }

      public void RunProcess(string sFilePath, string Arraguments, clsLogService Log, bool WatiForeExit, string oAppName)
      {
          Process oProcess = new Process();
          ProcessStartInfo oProcessInformation = new ProcessStartInfo();

          try
          {
              oProcessInformation.FileName = sFilePath; // set the file path
              if (Arraguments.Trim() != "")
              {
                  oProcessInformation.Arguments = Arraguments;
              }
              oProcess.StartInfo = oProcessInformation; // get the starting information
              oProcess.StartInfo.UseShellExecute = false;

              if (WatiForeExit)
              {
                  oProcess.Start();
                  oProcess.WaitForExit(); // waite un till the process is completed 
              }
              else
              {
                  oProcess.Start();
              }
          }
          catch (Exception ex)
          {
              Log.AddError(String.Format("ERROR: {0} " + Environment.NewLine + "STACK TRACE:{1} ", ex.Message, ex.StackTrace));
              System.Windows.Forms.MessageBox.Show("The system cannot find the " + oAppName + ". " + Environment.NewLine + "Please Call Micro Merchant immediately for troubleshooting.");
          }
      }
        public void RunProcess(string sFilePath,string Arraguments ,clsLogService Log,bool WatiForeExit)
        {
            try
            {
                RunProcess(sFilePath, Arraguments, Log, WatiForeExit, "");
            }
            catch (Exception ex)
            {
                Log.AddError(String.Format("ERROR: {0} " + Environment.NewLine + "STACK TRACE:{1} ", ex.Message, ex.StackTrace));
            }
                  
        
        }
    }
}
