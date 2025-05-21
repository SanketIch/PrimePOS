using System;
using System.Collections;
namespace PharmData
{
	/// <summary>
	/// This stores a collection of field values to be used by functions
	/// in the LabDB class. This inherits from the Array List since
	/// all the function need to be the same as an array list. But in the 
	/// future it can be modified to do more than just an Array List. Since this
	/// is a collection i inhertied from an arraylist which is also a collection.
	///  
	/// </summary>
	internal class FieldValueCollection : ArrayList
	{				
		public FieldValueCollection()
		{
			//
			// TODO: Add constructor logic here
			//															
		}
	}
}
