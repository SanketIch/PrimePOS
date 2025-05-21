//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make custom Dictionary.
//External functions:None   
//Known Bugs : None
//Start Date : 17 January 2008.
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.PROCESSOR
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make custom Dictionary.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 17 January 2008.
    public class MMSDictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue> 
    {
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is Rewritten to ensure the element is added irrespective of anything.
        /// External functions:MMSDictioanary
        /// Known Bugs : None
        /// Start Date : 18 Jan 2008.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            try
            {
                base.Remove(key);
                base.Add(key, value);
            }
            catch (ArgumentException argEx)
            {
                argEx = null;
                //base.Remove(key);
                //base.Add(key, value);
            }
        }
    }
}
