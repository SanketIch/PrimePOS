using System;
using System.Data;
using MMSBClass;
using MMSChargeAccount;
using System.Windows.Forms;
using POS_Core.CommonData;

namespace POS_Core.Resources
{
	/// <summary>
	/// Summary description for clsPatient.
	/// </summary>
	public class clsPatient
	{
		private clsPatient()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet SearchPatientInfo(string searchCode,string SearchName)
		{
			DataSet oDS=null;
			if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
			{
				ContAccount oSearch = new ContAccount();
				if (searchCode.Trim() != "")
				{
					oSearch.SearchPatientByCode(Configuration.convertNullToInt(searchCode), out oDS);
				}
				else
				{
					oSearch.SearchPatientByName(SearchName.Replace(",", "%,"), out oDS);
				}
			}
			return oDS;
		}

        public static DataSet GetPatientInfo(int iPatientNo)
        {
            DataSet oDS = null;
            ContAccount oSearch = new ContAccount();
            
            oSearch.GetPatientByCode(iPatientNo, out oDS);
            
            return oDS;
        }

	}
}
