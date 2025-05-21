using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NLog;
namespace EDevice
{
	internal static class ManageDLL
	{
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private static string GetSystemDirectory
		{
			get
			{
				StringBuilder path = new StringBuilder(300);
				ManageDLL.SHGetSpecialFolderPath(IntPtr.Zero, path, 41, false);
				return path.ToString();
			}
		}
		/// <summary>
		/// Path where is DLL is excuting from
		/// </summary>
		private static string ExcutingPath
		{
			get
			{
                string path = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                return Path.GetDirectoryName(path);
			}
		}

		[DllImport("shell32.dll")]
		public static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder, bool fCreate);
		public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
            logger.Trace("In CurrentDomain_AssemblyResolve()");

            string[] DLLNAME = args.Name.Split(new char[]
			{
				','
			});
			return ManageDLL.Load(DLLNAME[0]);
		}
		public static Assembly Load(string DName)
		{
            logger.Trace("In Load()");

            string dllPath = "EDevice.RBA." + DName.Trim() + ".dll";
			Assembly curAssembly = Assembly.GetExecutingAssembly();
			Assembly result;
			using (Stream dllStream = curAssembly.GetManifestResourceStream(dllPath))
			{
				byte[] Assemblydll = new byte[(int)dllStream.Length];
				dllStream.Read(Assemblydll, 0, (int)dllStream.Length);
				result = Assembly.Load(Assemblydll);
			}
			return result;
		}
		public static void CopyFromResource()
		{
            logger.Trace("In CopyFromResource()");
            try
			{
                string RBAFolder = "EDevice.RBA.";
                string[] RBAFiles = { "RBA_SDK_CS.dll", "RBA_SDK.dll" };
                foreach (var f in RBAFiles)
                {
                    if (!File.Exists(ManageDLL.ExcutingPath + "\\" + f))
                    {
                        Assembly curAssembly = Assembly.GetExecutingAssembly();
                        using (Stream dllStream = curAssembly.GetManifestResourceStream(RBAFolder + f))
                        {
                            using (FileStream fs = new FileStream(ManageDLL.ExcutingPath + "\\" + f, FileMode.CreateNew))
                            {
                                int i = 0;
                                while ((long)i < dllStream.Length)
                                {
                                    fs.WriteByte((byte)dllStream.ReadByte());
                                    i++;
                                }
                                fs.Close();
                                File.SetAttributes(ManageDLL.ExcutingPath + "\\" + f, FileAttributes.Hidden | FileAttributes.Archive);
                            }
                        }
                    }
                }
			}
			catch(Exception ex)
			{
                string message = "Failed to load Device Dll "+ex.Message;
                logger.Error(ex, message);
                throw new Exception("Failed to load Device Dll");
			}
		}
	}
}
