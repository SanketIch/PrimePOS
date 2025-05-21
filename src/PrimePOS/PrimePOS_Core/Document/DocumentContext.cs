using PharmData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Document
{
   public class DocumentContext
    {
        PharmBL oPharmBL = new PharmBL();
        DocumentStrategy documentStrategy;

        public DocumentLocation CurrentLocation 
        { 
            get 
            { 
                return (DocumentLocation)oPharmBL.GetDocumentLocation();
            } 
        }        

        public DocumentContext()
        {
            switch (CurrentLocation)
            {
                case DocumentLocation.LocalDrivePath:
                    documentStrategy = new LocalDriveDocument();
                    break;
                case DocumentLocation.SQLDatabase:
                    documentStrategy = new SQlDataBaseDocument();
                    break;
                //case DocumentLocation.AmazonCloudStorage:
                //    documentStrategy = new AmazonCloudStorage();
                //    break;
                default:
                    documentStrategy = new LocalDriveDocument();
                    break;
            }
        }

        public  byte[] GetImageDocument(string documentid)
        {
          return  documentStrategy.GetImageDocument(documentid);
        }
    }
}
