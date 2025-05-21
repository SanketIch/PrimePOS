// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------


namespace POS_Core.Resources
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Configuration;
    using System.Xml;
    using System.Collections.Specialized;
    
    public sealed class ApplicationConfiguration 
    {

                    
        public static String ReadSetting(NameValueCollection settings, String key, String defaultValue)
        {
            try
            {
                Object setting = settings[key];
                
                return (setting == null) ? defaultValue : (String)setting;
            }

            catch
            {
                return defaultValue;
            }
        }
        
        
		public static void OnApplicationStart(string path)
		{
			System.Configuration.ConfigurationSettings.GetConfig("appSettings"); 
			//InitializeApplication(System.Configuration.ConfigurationSettings.AppSettings);
		}
        
        
    } 
} 

 
