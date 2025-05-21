using POS_Core.Resources;
using System;
using System.Text.RegularExpressions;
//using SecurityManager;

namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for clsCommandLineOptions.
    /// </summary>
    public class clsCommandLineOptions
    {
        public clsCommandLineOptions()
        {
        }

        private void ProcessCommand(string command, object[] param)
        {
            string methodName = "";
            bool valid = true;

            //if (param != null)
            //{
            //    if (param.Length != 0)
            //    {
            //        valid = IsValidCommand(command, ref param, ref methodName);
            //    }
            //    else
            //    {
            //        valid = IsValidCommand(command, ref methodName);
            //    }
            //}
            //else
            //{
            //    valid = IsValidCommand(command, ref methodName);
            //}

            methodName = "ScheduledTaskExecute";
            try
            {
                param[0] = Configuration.convertNullToInt(param[0]);
                param[1] = Configuration.convertNullToInt(param[1]);
            }
            catch { }
            if (valid)
            {
                methodName = methodName.Replace("[", "");
                methodName = methodName.Replace("]", "");

                //clsCommandMethods commMethods = new clsCommandMethods();
                frmScheduledTasksView commMethods = new frmScheduledTasksView();

                System.Reflection.MethodInfo method = commMethods.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.Default
                    );

                if (param != null && method != null)
                {
                    //AppGlobal.CalledFromCommLine = true;
                    method.Invoke(commMethods, param);
                    return;
                }
            }
            //ErrorHandler.ShowMessage(null, new Exception("Invalid paramerts or insufficient parmeters entered"), false);
        }

        public bool ExecuteCommand(string[] args)
        {
            if (args.Length < 2)
                return false;

            //ContPhUser oPhUser = new ContPhUser();
            //         oPhUser.GetPhUser(AppGlobal.objSysSettings.CommLineUser, out AppGlobal.gPhUser);
            //         if (AppGlobal.gPhUser != null) clsSecurityManager.UserId = AppGlobal.gPhUser.PH_INIT;

            object[] param = null;
            int length = 0;
            try
            {
                string command = "";

                if (args.Length > 1)
                {
                    command = args[1];
                    length = args.Length - 2;
                    param = new object[length];
                    for (int i = 0; i < param.Length; i++)
                    {
                        param[i] = args[i + 2];
                    }
                }

                if (command.Trim() != string.Empty)
                {
                    ProcessCommand(command, param);

                    return true;
                }
                return false;
            }
            catch (Exception exp)
            {
                throw (exp);
            }
            finally
            {
                //AppMain.bApplicationClosed = true;
            }
        }

        //private bool IsValidCommand(string comm, ref object[] param, ref string method)
        //{
        //    bool retValue = false;
        //    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(this.GetType()).Location);

        //    path = System.IO.Path.Combine(path, "CommLineOptions.ini");

        //    try
        //    {
        //        System.IO.StreamReader reader = System.IO.File.OpenText(path);
        //        //"CommLineOptions.ini",System.Text.Encoding.Default );
        //        string line = "";

        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            string[] tokens = line.Split(' ');
        //            if (tokens.Length == 0) continue;
        //            if (tokens[0].ToUpper() == comm.ToUpper())
        //            {
        //                if (tokens.Length != (param.Length + 2)) continue;
        //                for (int i = 0; i < param.Length; i++)
        //                {
        //                    string value = (string)param[i];
        //                    string type = tokens[i + 1];
        //                    switch (type.ToLower())
        //                    {
        //                        case "datetime":

        //                            if (value.Trim() == "''")
        //                                param[i] = DateTime.Parse("1/1/1900");
        //                            else if (value.Equals("[TD]", StringComparison.CurrentCultureIgnoreCase))
        //                                param[i] = DateTime.Today;
        //                            else if (value.Trim().StartsWith("[TD-", StringComparison.CurrentCultureIgnoreCase) && value.Trim().EndsWith("]"))
        //                                param[i] = DateTime.Today.AddDays(-MMSUtil.UtilFunc.ValorZeroI(value.Trim().Replace("[TD-", "").Replace("[td-", "").Replace("]", "")));
        //                            else if (value.Trim().StartsWith("[TD+", StringComparison.CurrentCultureIgnoreCase) && value.Trim().EndsWith("]"))
        //                                param[i] = DateTime.Today.AddDays(MMSUtil.UtilFunc.ValorZeroI(value.Trim().Replace("[TD+", "").Replace("[td+", "").Replace("]", "")));
        //                            else
        //                                param[i] = Convert.ToDateTime(value);
        //                            break;
        //                        case "int":

        //                            if (value.Trim() == "''")
        //                                param[i] = 0;
        //                            else
        //                                param[i] = Convert.ToInt32(value);
        //                            break;
        //                        case "string":

        //                            if (value.Trim() == "''")
        //                                param[i] = "";
        //                            else
        //                                param[i] = Convert.ToString(value);
        //                            break;
        //                        case "double":
        //                            if (value.Trim() == "''")
        //                                param[i] = 0.0;
        //                            else
        //                                param[i] = Convert.ToDouble(value);
        //                            break;
        //                        case "decimal":

        //                            if (value.Trim() == "''")
        //                                param[i] = 0.0;
        //                            else
        //                                param[i] = Convert.ToDecimal(value);
        //                            break;
        //                    }
        //                }
        //                method = tokens[tokens.Length - 1];
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        ErrorHandler.ShowMessage(null, exp, false);
        //    }

        //    return retValue;
        //}

        //private bool IsValidCommand(string comm, ref string method)
        //{
        //    bool retValue = false;
        //    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //    path = System.IO.Path.Combine(path, "CommLineOptions.ini");

        //    try
        //    {
        //        System.IO.StreamReader reader = new System.IO.StreamReader(path, System.Text.Encoding.Default);
        //        string line = "";

        //        while ((line = reader.ReadLine()) != null)
        //        {

        //            string[] tokens = line.Split(' ');
        //            //string [] tokens = comm.Split(' ');

        //            if (tokens.Length == 0) continue;

        //            if (tokens[0].ToUpper() == comm.ToUpper())
        //            {

        //                method = tokens[tokens.Length - 1];
        //                return true;
        //            }
        //        }
        //        return retValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.ShowMessage(null, ex, false);
        //        return false;
        //    }
        //}
    }
}
