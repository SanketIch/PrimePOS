using System;

namespace PharmData
{
	/// <summary>
	/// This class will store the field and its value that needs to
	/// be matched by the LabDB class when searching on a index.
	/// </summary>
	internal class FieldValue
	{
		
		public string sField="";
		public string sValue="";		
		public FieldValue()
		{			
			
			//
			// TODO: Add constructor logic here
			//
		}	

		public FieldValue(string sF, string sV)
		{						
			this.sField = sF;
			this.sValue = sV;
		}		

	}
}
