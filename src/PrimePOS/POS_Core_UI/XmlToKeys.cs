using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace POS_Core_UI
{
    public class XmlToKeys : IDisposable
    {
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
        /// Sprint-22 - 2240 30-Sep-2015 JY Added
        /// Functionality Desciption : This method is constructor for the KeysToXml class
        /// </summary>
        public XmlToKeys()
        {
            NodeList = null;
            MsgDocument = new XmlDocument();
        }

        /// <summary>
        /// Functionality Desciption : This method is used to create keys from xml
        /// </summary>
        /// <param name="xmlMessage"></param>
        /// <param name="xmlFilterNodeName"></param>
        /// <param name="fields"></param> 
        /// <param name="isFile"></param> 
        /// <returns>int result</returns>
        public int GetFields(String xmlMessage, String xmlFilterNodeName, ref Dictionary<String, String> fields, bool isFile)
        {
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
                                try
                                {
                                    fields.Add(field.Name, field.InnerText);
                                }
                                catch { }
                            }
                            break;
                        }
                    }
                    else
                    {
                        try
                        {
                            fields.Add(node.Name, node.InnerXml);
                        }
                        catch { }
                    }
                }
                NodeList = null;
                MsgDocument.RemoveAll();
            }
            catch (Exception ex)
            {
                throw ex;
                return FAILURE;
            }
            return fields.Count;
        }
        /// <summary>
        /// Functionality Desciption : Destructor
        /// </summary>
        ~XmlToKeys()
        {
            System.Diagnostics.Debug.Print("XmlToKeys destructor\n");
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
