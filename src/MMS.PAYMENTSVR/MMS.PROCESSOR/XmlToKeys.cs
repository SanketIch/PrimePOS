//Author : Ritesh 
//CopyRight: MMS 2008
//Functionality Desciption : The purpose of this class is to convert XmlTo keys.
//External functions:MMSDictioanary   
//Known Bugs : None
//Start Date : 21 January 2008.
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NLog;

namespace MMS.PROCESSOR
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to convert XmlTo keys.
    //External functions:MMSDictioanary   
    //Known Bugs : None
    //Start Date : 21 January 2008.
    public class XmlToKeys: IDisposable
    {
        ILogger logger = LogManager.GetCurrentClassLogger();  
        #region variables
        private XmlDocument MsgDocument = null;
        private XmlNodeList NodeList = null;
        private Boolean Disposed = false;
        #endregion
        #region constants
        private const int SUCCESS = 0;
        private const int FAILURE = 1;
        #endregion

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is constructor for the KeysToXml class
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 21 Jan 2008.
        /// </summary>
        public XmlToKeys()
        {
            logger.Trace("In XMLToKeys() CTOR");
            NodeList = null;
            MsgDocument = new XmlDocument();
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This method is used to create keys from xml
        /// External functions:MMSDictioanary,XmlDocument
        /// Known Bugs : MMSDictioanary
        /// Start Date : 14 Jan 2008.
        /// </summary>
        /// <param name="xmlMessage"></param>
        /// <param name="xmlFilterNodeName"></param>
        /// <param name="fields"></param> 
        /// <param name="isFile"></param> 
        /// <returns>int result</returns>
        public int GetFields(String xmlMessage, String xmlFilterNodeName, ref MMSDictionary<String, String> fields, bool isFile) 
        {
            logger.Trace("In GetFields()");
            try
            {
                MsgDocument.RemoveAll();
                if (!isFile) 
                    MsgDocument.LoadXml(xmlMessage);
                else
                    MsgDocument.Load(xmlMessage);
                NodeList = MsgDocument.DocumentElement.ChildNodes;
                foreach (XmlNode node in NodeList)
                {
                    //if no filter is ther means all child from root elemet are required
                    if (xmlFilterNodeName.Trim() != "")
                    {
                        if (node.Name == xmlFilterNodeName.Trim())
                        {
                            foreach (XmlNode field in node.ChildNodes)
                            {
                                fields.Add(field.Name, field.InnerText);
                            }
                            break;
                        }
                    }
                    else
                    {
                        fields.Add(node.Name, node.InnerXml);
                    }
                }
                NodeList = null;
                MsgDocument.RemoveAll();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw ex; // Modified by Dharmendra (SRTt) on Dec-15-08 throwing exception if xmlMessage is not in xml readable format 
                return FAILURE;
            }
            return fields.Count;
        }
        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : Destructor
        /// External functions:None.  
        /// Known Bugs : None
        /// Start Date : 17 January 2008.
        /// </summary>
        ~XmlToKeys()
        {
            logger.Trace("XmlToKeys destructor\n");
            Dispose(false);
            
        }


        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;
            if (disposing)
            {
                if (NodeList != null)
                    NodeList = null;
                if (MsgDocument != null)
                {
                    MsgDocument.RemoveAll();
                    MsgDocument = null;
                }

            }
            // Unmanaged cleanup code here
            Disposed = true;
        }

        #endregion
    }
}
