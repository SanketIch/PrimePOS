using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;

namespace MMSAppUpdater
{
    internal class clsAppInstance
    {
        public string getPathByRunningInstance(string sAppName, clsLogService log)
        {
            string ProcessName = "";
            try
            {
                ManagementScope scope = new ManagementScope("\\\\"+Environment.MachineName+"\\root\\cimv2");

                //create object query
                if (sAppName.Contains(".exe") == false)
                    sAppName = sAppName + ".exe";

                ObjectQuery query = new ObjectQuery("select * from win32_process where name='"+sAppName.Trim()+"'");

                //create object searcher
                ManagementObjectSearcher searcher =
                                        new ManagementObjectSearcher(scope, query);

                //get collection of WMI objects
                ManagementObjectCollection queryCollection = searcher.Get();

                //enumerate the collection.
                foreach (ManagementObject m in queryCollection)
                {
                    if (sAppName.Equals(System.IO.Path.GetFileName(Application.ExecutablePath), StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessName = Application.ExecutablePath;
                        break;
                    }
                    else
                    {
                        try
                        {
                            if (m["ExecutablePath"] != null)
                                ProcessName = m["ExecutablePath"].ToString();// get the file name from the process 
                            break;
                        }
                        catch (Exception)
                        {
                            
                            
                        }
                       
                    }
                }
                  

                //Process p = Process.GetProcessById(19472);
                //Process[] pErx = Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(sAppName));
                //if (pErx != null && pErx.Length > 0)
                //{
                //    // if the current instance is application instace the return the application path 
                //    // because in this case appication processs in not initiated
                //    if (sAppName.Equals(System.IO.Path.GetFileName(Application.ExecutablePath),StringComparison.OrdinalIgnoreCase))
                //    {
                //        ProcessName = Application.ExecutablePath;
                //    }
                //    else
                //    {
                //        ProcessName = pErx[0].MainModule.FileName;// get the file name from the process 
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.AddError(String.Format("Always Running Product Error {0}  ", ex.Message));
            }
            return ProcessName;
        }

    }
}
