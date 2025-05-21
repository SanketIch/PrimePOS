using System;

namespace PharmData
{
	/// <summary>
	/// Summary description for Util.
	/// </summary>
	internal class Util
	{
		public Util()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public static bool IsNumeric(string sVal)
		{			
			bool bRetVal=false;
			try
			{
				double temp=Convert.ToDouble(sVal);				
				bRetVal=true;
			}
			catch
			{}
			return bRetVal;
		}
	
		public static bool IsDate(string sVal)
		{
			bool bRetVal=false;
			try
			{
				DateTime o=Convert.ToDateTime(sVal);
				bRetVal=true;
			}
			catch
			{}
			return bRetVal;
		}
	
	}
}
