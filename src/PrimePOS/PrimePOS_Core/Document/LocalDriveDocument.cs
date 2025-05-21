using MMS.Encryption;
using PharmData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Document
{
    public class LocalDriveDocument : DocumentStrategy
    {
        public override byte[] GetImageDocument(string documentid)
        {
            byte[] blankimg;
            if (documentid != "")
            {
                PharmBL oPharmBL = new PharmBL();
                System.Drawing.Image oImage = null;
                string sError = string.Empty;
                string imageStorePath = oPharmBL.GetDocumentPhysicalPath();
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    blankimg = ms.ToArray();
                }

                if (imageStorePath == null)
                    return blankimg;

                DecryptImage(imageStorePath, documentid, out oImage);
                if (oImage == null)
                    return GetImageWithoutEncryption(documentid, imageStorePath);

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

        //public override byte[] GetImageDocument1(string documentid)
        //{
        //    PharmBL oPharmBL = new PharmBL();
        //    byte[] blankimg;
        //    System.Drawing.Image oImage = null;
        //    if (documentid != "")
        //    {
        //        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //        {
        //            blankimg = ms.ToArray();
        //        }

        //        byte[] byt = oPharmBL.GetDocumentFromdb(documentid);
        //        if (byt == null)
        //            return null;
        //        byte[] bytdecrypt = NativeEncryption.DecryptImageDB(byt);
        //        if (bytdecrypt != null)
        //            return bytdecrypt;
        //        else
        //            oImage = GetImageFromByts(byt);

        //        if (bytdecrypt == null)
        //            return GetImageWithoutEncryption(documentid, oImage);

        //        System.Drawing.Bitmap bmp;
        //        using (MemoryStream memstream = new MemoryStream())
        //        {
        //            oImage.Save(memstream, System.Drawing.Imaging.ImageFormat.Bmp);
        //            //to get image dpi from db
        //            float ImageDPI = 250;
        //            //oDsDoc.DM_Document.DocumentIdColumn.KeyValue = documentid;
        //            //ImageDPI = oDocRow.DPI;

        //            bmp = new System.Drawing.Bitmap(memstream);
        //            bmp.SetResolution(ImageDPI, ImageDPI);

        //            byte[] img;
        //            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
        //            {
        //                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                img = stream.ToArray();
        //            }

        //            bmp.Dispose();
        //            return img;  // stream.ToArray();
        //        }
        //    }
        //    else
        //    {
        //        return blankimg = null;
        //    }
        //}

        public static System.Drawing.Image GetImageFromByts(Byte[] img)
        {
            if (img == null)
                return null;

            System.Drawing.Bitmap bmp = null;
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(img))
            {
                if (stream.Length != 0)
                    bmp = new System.Drawing.Bitmap(stream);
            }
            return bmp;
        }

        private Byte[] GetImageWithoutEncryption(string documentId, string path)
        {
            byte[] blankimg;
            string pathfile = string.Empty;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                blankimg = ms.ToArray(); // = AppGlobal.GetImage(dr["documentid"].ToString());    
            }

            if (path == null)
                return blankimg;   //(new System.IO.MemoryStream()).ToArray();

            pathfile = System.IO.Path.Combine(path, documentId.Trim() + ".jpg");

            if (System.IO.File.Exists(pathfile) == false)
            {

                if (path == null) return blankimg;  // (new System.IO.MemoryStream()).ToArray();
                pathfile = System.IO.Path.Combine(path, documentId.Trim() + ".jpeg");
                if (System.IO.File.Exists(path) == false)
                {
                    return blankimg;  // (new System.IO.MemoryStream()).ToArray();
                }
            }
            //ContScanDocument oCont = new ContScanDocument();
            ////to get image dpi from db
            //float ImageDPI = 250;
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
            ////
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(path));
            //bmp.SetResolution(ImageDPI, ImageDPI);

            byte[] img;

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = stream.ToArray();
            }
            bmp.Dispose();
            return img;  // stream.ToArray(); 
        }

        //private Byte[] GetImageWithoutEncryption(string documentId, Image oimage)
        //{
        //    byte[] blankimg;
        //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //    {
        //        blankimg = ms.ToArray();
        //    }

        //    //to get image dpi from db
        //    float ImageDPI = 250;
        //    //ImageDPI = oDocRow.DPI;
        //    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(oimage);
        //    bmp.SetResolution(ImageDPI, ImageDPI);

        //    byte[] img;
        //    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        img = stream.ToArray();
        //    }
        //    bmp.Dispose();

        //    return img;  // stream.ToArray(); 
        //}

        private void DecryptImage(string imageStorePath, string sDocID, out System.Drawing.Image oImage)
        {
            oImage = null;
            try
            {
                //File after encryption.enc
                string DecFile = string.Empty;
                //New Path Year and Month
                string NewPath = string.Empty;
                string sError = string.Empty;
                NewPath = FolderName(sDocID);
                //set the new encrypted file with the new .enc extension. This will replace the .jpeg after encryption
                DecFile = imageStorePath + NewPath + "\\" + Path.GetFileNameWithoutExtension(sDocID);

                if (!Directory.Exists(Path.GetDirectoryName(DecFile)))
                {
                    return;
                }

                //Encrypt the Image(JPEG) using NativeEncryption.dll
                DecFile = DecFile.Trim() + ".enc";
                if (!NativeEncryption.DecryptFile(DecFile, out oImage, out sError) || !string.IsNullOrWhiteSpace(sError))
                {
                    //ErrorHandler.ShowMessage(new Exception(sError));
                }
            }
            catch (UnauthorizedAccessException uA)
            {
                //ErrorHandler.ShowMessage(uA);
            }
            catch (Exception ex)
            {
                //ErrorHandler.ShowMessage(ex);
            }
        }

        //private  void DecryptImage(string imageStorePath, string sDocID, out System.Drawing.Image oImage)
        //{
        //    oImage = null;
        //    try
        //    {
        //        //File after encryption.enc
        //        string DecFile = string.Empty;
        //        //New Path Year and Month
        //        string NewPath = string.Empty;
        //        string sError = string.Empty;
        //        NewPath = FolderName(sDocID);
        //        //set the new encrypted file with the new .enc extension. This will replace the .jpeg after encryption
        //        DecFile = imageStorePath + NewPath + "\\" + Path.GetFileNameWithoutExtension(sDocID);

        //        if (!Directory.Exists(Path.GetDirectoryName(DecFile)))
        //        {
        //            return;
        //        }

        //        //Encrypt the Image(JPEG) using NativeEncryption.dll
        //        DecFile = DecFile.Trim() + ".enc";
        //        if (!NativeEncryption.DecryptFile(DecFile, out oImage, out sError) || !string.IsNullOrWhiteSpace(sError))
        //        {
        //            //ErrorHandler.ShowMessage(new Exception(sError));
        //        }
        //    }
        //    catch (UnauthorizedAccessException uA)
        //    {
        //        //ErrorHandler.ShowMessage(uA);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorHandler.ShowMessage(ex);
        //    }
        //}

        private string FolderName(string FileName)
        {
            string Folder = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    var yr = FileName.Substring(1, 2); //Year 2 digits
                    var mm = FileName.Substring(3, 2); //Month
                    var dd = FileName.Substring(5, 2); //Day
                    var yyyy = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar.ToFourDigitYear(Convert.ToInt32(yr)); //get the 4 digit year

                    var Month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(mm)); //Name of the Month by pass the 2 digit Month
                    Folder = "\\" + Convert.ToString(yyyy) + "\\" + Month + "\\" + dd; //Create the sub path
                }
            }
            catch (Exception ex)
            {
                return Folder;
            }
            return Folder;
        }
    }
}
