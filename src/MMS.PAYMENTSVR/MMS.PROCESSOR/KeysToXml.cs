//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make XML from keys.
//External functions:None   
//Known Bugs : None
//Start Date : 17 January 2008.
using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace MMS.PROCESSOR
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make XML from keys.
    //External functions:None   
    //Known Bugs : None
    //Start Date : 17 January 2008.
    public class KeysToXml
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        String XmlString = String.Empty;

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the XmlToKeys class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 17 Jan 2008.
        /// </summary>
        public KeysToXml()
        {
            logger.Trace("In KeystoXML() contructor");
            XmlString = String.Empty;
        }
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used to create xml from kesu
        /// External functions:MMSDictioanary
        /// Known Bugs : None
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="header"></param>
        /// <param name="footer"></param> 
        /// <returns>String result</returns>
        public String BuildXML(ref MMSDictionary<String, String> fields, String header, String footer)
        {
            logger.Trace("In BuildXML()");
            XmlString = String.Empty;
            XmlString += header.Trim();
            foreach (KeyValuePair<String, String> kvp in fields)
            {
                XmlString += "<" + kvp.Key.Trim() + ">";
                XmlString += kvp.Value;
                XmlString += "</" + kvp.Key.Trim() + ">";
            }
            XmlString += footer.Trim()+ "\n";
            return XmlString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public String BuildXCXML(ref MMSDictionary<String, String> fields)
        {
            logger.Trace("In BuildXCXML()");
            String xmlData = String.Empty;
            return xmlData;

         
        }
        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : Destructor
        /// External functions:None.  
        /// Known Bugs : None
        /// Start Date : 17 January 2008.
        /// </summary>
        ~KeysToXml()
        {
            logger.Trace("KeysToXml destructor\n");
            XmlString = null;
        }
    }
}
