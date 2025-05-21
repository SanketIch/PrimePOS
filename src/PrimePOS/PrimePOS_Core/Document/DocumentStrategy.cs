using System;
using System.Drawing;

namespace POS_Core.Document
{
    public abstract class DocumentStrategy
    {       
        public abstract Byte[] GetImageDocument(string documentid);
    }
}