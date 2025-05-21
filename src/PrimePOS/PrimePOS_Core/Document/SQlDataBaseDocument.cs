using System;
using System.Collections.Generic;
using System.Drawing;
using MMS.Encryption;
using System.IO;
using System.Reflection;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using PharmData;

namespace POS_Core.Document
{
    internal class SQlDataBaseDocument : DocumentStrategy
    {
        private Byte[] GetImageWithoutEncryption(string documentId, Image oimage)
        {
            byte[] blankimg;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                blankimg = ms.ToArray();
            }

            //ContScanDocument oCont = new ContScanDocument();
            //to get image dpi from db
            float ImageDPI = 250;
            //dsElecDocument oDsDoc = new dsElecDocument();
            //dsElecDocument.DM_DocumentRow oDocRow;
            //oDsDoc.DM_DocumentCat.GetFromDB = false;
            //oDsDoc.DM_DocumentSubCat.GetFromDB = false;
            //oDsDoc.DM_Document.DocumentIdColumn.KeyValue = documentId;
            //oDsDoc.EnforceConstraints = false;
            //oCont.getByKey(oDsDoc);
            //if (oDsDoc.DM_Document.Rows.Count > 0)
            //{
            //    oDocRow = (dsElecDocument.DM_DocumentRow)oDsDoc.DM_Document.Rows[0];
            //    ImageDPI = oDocRow.DPI;
            //}
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(oimage);
            bmp.SetResolution(ImageDPI, ImageDPI);

            byte[] img;
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = stream.ToArray();
            }
            bmp.Dispose();
            return img;  // stream.ToArray(); 
        }

        public override byte[] GetImageDocument(string documentid)
        {
            PharmBL oPharmBL = new PharmBL();
            //ContDM_DocumentImageFiles oContDM_Documents = new ContDM_DocumentImageFiles();
            //ContScanDocument oCont = new ContScanDocument();

            byte[] blankimg;
            System.Drawing.Image oImage = null;
            if (documentid != "")
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    blankimg = ms.ToArray();
                }

                byte[] byt = oPharmBL.GetDocumentFromdb(documentid);
                if (byt == null)
                    return null;
                byte[] bytdecrypt = NativeEncryption.DecryptImageDB(byt);
                if(bytdecrypt!=null)
                return bytdecrypt;
                else
              //oImage =  DocumentConfig.GetImageFromByts(byt);

                if (bytdecrypt == null)
                    return GetImageWithoutEncryption(documentid, oImage);

                System.Drawing.Bitmap bmp;
                using (MemoryStream memstream = new MemoryStream())
                {
                    oImage.Save(memstream, System.Drawing.Imaging.ImageFormat.Bmp);
                    //to get image dpi from db
                    float ImageDPI = 250;
                    //dsElecDocument oDsDoc = new dsElecDocument();
                    //dsElecDocument.DM_DocumentRow oDocRow;
                    //oDsDoc.DM_DocumentCat.GetFromDB = false;
                    //oDsDoc.DM_DocumentSubCat.GetFromDB = false;
                    //oDsDoc.DM_Document.DocumentIdColumn.KeyValue = documentid;
                    //oDsDoc.EnforceConstraints = false;
                    //oCont.getByKey(oDsDoc);
                    //if (oDsDoc.DM_Document.Rows.Count > 0)
                    //{
                    //    oDocRow = (dsElecDocument.DM_DocumentRow)oDsDoc.DM_Document.Rows[0];
                    //    ImageDPI = oDocRow.DPI;
                    //}
                    bmp = new System.Drawing.Bitmap(memstream);
                    bmp.SetResolution(ImageDPI, ImageDPI);

                    byte[] img;

                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        img = stream.ToArray();
                    }
                    bmp.Dispose();
                    return img;  // stream.ToArray();
                }
            }
            else
            {
                return blankimg = null;
            }
        }

        //public override string GetPDFDocument(string documentid)
        //{
        //    if (documentid == "")
        //        return "";
        //    if (documentid.ToString().StartsWith("F"))
        //    {
        //        //old logic was there
        //    }

        //    string sDecryptedFile = string.Empty;
        //    string sError = string.Empty;

        //    byte[] blankimg;

        //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //    {
        //        blankimg = ms.ToArray();
        //    }
        //    DecryptFileDB(documentid, out sDecryptedFile);
        //    if (File.Exists(sDecryptedFile))
        //        return sDecryptedFile;
        //    else
        //        return "";
        //}

        //public override void SaveImageDoucment(Image oImage, string sDocID)
        //{
        //    if (oImage == null) return;

           

        //    Bitmap bmp = new Bitmap(oImage);
        //    bmp.SetResolution(oImage.HorizontalResolution, oImage.VerticalResolution);


        //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //    {
        //        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        if (ms.Length / 1024 > 500)//if greater than 500kb then compress.
        //        {
        //            if (SaveJPGWithCompressionSetting(ref oImage, 25))
        //            {
        //                EncryptImageDB(oImage, sDocID);
        //            }
        //            else
        //            {
        //                EncryptImageDB(bmp, sDocID);
        //            }
        //        }
        //        else
        //        {
        //            EncryptImageDB(bmp, sDocID);
        //        }
        //    }
        //    bmp.Dispose();
        //}
        public bool SaveJPGWithCompressionSetting(ref System.Drawing.Image image, long lCompression)
        {

            try
            {
                MemoryStream ms = new MemoryStream();
                EncoderParameters eps = new EncoderParameters(1);

                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, lCompression);

                ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

                image.Save(ms, ici, eps);
                image = System.Drawing.Image.FromStream(ms);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }


        }

        private ImageCodecInfo GetEncoderInfo(String mimeType)
        {

            int j;

            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (j = 0; j < encoders.Length; ++j)
            {

                if (encoders[j].MimeType == mimeType)

                    return encoders[j];

            }

            return null;

        }
    }
}
