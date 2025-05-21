using System;

namespace PharmData
{
	/// <summary>
	/// Summary description for FPUtil.
	/// </summary>
	internal class FPUtil
	{
		public FPUtil()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		public static bool GetCShDate(string sDate,out System.DateTime dtRetVal)
		{
			/*
			 * This function accepts a foxpro date and returns a
			 * c# date.
			 * */
			bool bRetVal=false;
			dtRetVal=DateTime.Now;
			if(sDate.Length == 8)
			{
				try
				{					
					dtRetVal=Convert.ToDateTime(sDate.Substring(4,2)+"/"+sDate.Substring(6,2)+"/"+sDate.Substring(0,4));	
					bRetVal=true;
				}
				catch
				{}				
			}
			return bRetVal;
		}

		public static double FPNumericConvert(string sVal)
		{
			/*
			 * this function will convert a value to a double
			 * it is more intelligent than convert.todouble in the sense
			 * if there is a 101.a it will parse out the 101 and
			 * return that numeric value.
			 * */
			 double dRetVal=0;
			 string sParsed="";	 			 
			 string sCh=""; 
			 
			 for(int i=0;i<sVal.Length;i++)
			 {				
				 sCh=sVal[i].ToString();
				 if(Util.IsNumeric(sCh))
					 sParsed+=sCh;
				 else
					 break;	 					
			 }			 
			 						 
			try
			{
				if(!(sParsed.Equals("")))
					dRetVal=Convert.ToDouble(sParsed);					
			}
			catch{}
			
			return dRetVal;
		
		}

		public static string GetFPDate(System.DateTime a)
		{
			//returns back the fp format of a date which needs to be written to codebase
			string sMonth=a.Month.ToString();
			string sDay=a.Day.ToString();
			string sYear=a.Year.ToString();			
			if (sMonth.Trim().Length == 1)
				sMonth="0"+sMonth;			
			if (sDay.Trim().Length == 1)
				sDay="0"+sDay;
			return (sYear.Trim()+sMonth.Trim()+sDay.Trim());			
		}
	
		public static double GetCCDouble(string sFpDouble)
		{
			double dRetVal=0;
			try
			{
				dRetVal=Convert.ToDouble(sFpDouble);
			}
			catch{}
			return dRetVal;
		}

		public static string GetFPTime(System.DateTime a)
		{			
			
			int iHour;
			string sMinute;
			iHour=a.Hour % 12;
			
			if (iHour==0)
				iHour=12;

			sMinute=a.Minute.ToString();

			if (sMinute.Length == 1)
				sMinute="0"+sMinute;			
			
			return iHour.ToString()+":"+sMinute+" "+(((DateTime.Now.ToString().IndexOf("AM")!=-1)) ? "AM" : "PM");
		}

	
	}
}
