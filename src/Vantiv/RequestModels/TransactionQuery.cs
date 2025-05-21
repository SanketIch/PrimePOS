using System.Xml.Serialization;

namespace Vantiv.RequestModels
{
    [XmlRoot(ElementName = "TransactionQuery", Namespace = "https://reporting.elementexpress.com")]
    public class TransactionQuery
    {
        [XmlElement(ElementName = "Credentials")]
        public Credentials Credentials
        {
            get; set;
        }
        [XmlElement(ElementName = "Application")]
        public Application Application
        {
            get; set;
        }
        [XmlElement(ElementName = "Parameters")]
        public Parameters Parameters
        {
            get; set;
        }
         
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns
        {
            get; set;
        }
        #region PRIMEPOS-2796
        [XmlElement(ElementName = "Terminal")]
        public Terminal Terminal
        {
            get; set;
        }
        #endregion
    }
    [XmlRoot(ElementName = "Parameters")]
    public class Parameters
    {
        [XmlElement(ElementName = "ReferenceNumber")]
        public string ReferenceNumber
        {
            get; set;
        }

    }
}
