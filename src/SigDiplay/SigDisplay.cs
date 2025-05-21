using MMSBitCrop;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace SigDiplay
{
    public class SigDisplay
    {
        public const string BINARYIMAGE = "M";
        public const string STRINGIMAGE = "D";
        private ArrayList SigLines;
        private Bitmap bmpSignature;
        private FileInfo mFileInfo;

        public Bitmap CropImage(Bitmap sig)
        {
            byte[][] color = this.GetColor(sig);
            int num1 = sig.Height - 1;
            int width = sig.Width;
            int y = 0;
            int num2 = num1;
            int num3 = sig.Width;
            int num4 = 0;
            int num5 = 0;
            int num6 = 99;
            bool flag = false;
            for (int index = 0; index < color[0].Length; ++index)
            {
                int num7 = index % width;
                int num8 = (int)Math.Floor((Decimal)(index / width));
                int num9 = (int)byte.MaxValue * num6 / 100;
                if ((int)color[0][index] >= num9 && (int)color[1][index] >= num9 && (int)color[2][index] >= num9)
                {
                    ++num5;
                    num4 = num7 <= num4 || num5 != 1 ? num4 : num7;
                }
                else
                {
                    num3 = num7 >= num3 || num5 < 1 ? num3 : num7;
                    num4 = num7 != width - 1 || num5 != 0 ? num4 : width - 1;
                    num5 = 0;
                }
                if (num5 == width)
                {
                    y = num8 - y < 3 ? num8 : y;
                    num2 = !flag || num7 != width - 1 || num8 <= y + 1 ? num2 : num8;
                }
                num3 = num7 != 0 || num5 != 0 ? num3 : 0;
                num2 = num8 != num1 || num7 != width - 1 || (num5 == width || !flag) ? num2 : num1 + 1;
                if (num7 == width - 1)
                {
                    flag = num5 < width;
                    num5 = 0;
                }
            }
            int num10 = num4 == 0 ? width : num4;
            int x = num3 == width ? 0 : num3;
            if (num2 - y > 0)
                return sig.Clone(new Rectangle(x, y, num10 - x, num2 - y), sig.PixelFormat);
            return sig;
        }

        private byte[][] GetColor(Bitmap bmp)
        {
            BitmapData bitmapdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            IntPtr scan0 = bitmapdata.Scan0;
            int length = bmp.Width * bmp.Height;
            int num1 = bitmapdata.Stride - bmp.Width;
            int index1 = 0;
            int num2 = 25;
            byte[] numArray1 = new byte[length];
            byte[] numArray2 = new byte[length];
            byte[] numArray3 = new byte[length];
            byte[] destination = new byte[length];
            Marshal.Copy(scan0, destination, 0, length);
            try
            {
                for (int index2 = 0; index2 < length; ++index2)
                {
                    if (index2 == bitmapdata.Stride * num2 - num1)
                    {
                        index2 += num1;
                        ++num2;
                    }
                    numArray1[index1] = destination[index2];
                    numArray2[index1] = destination[index2];
                    numArray3[index1] = destination[index2];
                    ++index1;
                }
            }
            catch (Exception ex)
            {
            }
            bmp.UnlockBits(bitmapdata);
            return new byte[3][]
            {
        numArray1,
        numArray2,
        numArray3
            };
        }

        public bool DrawSignatureMXCrop(byte[] sSigDataBin, ref Bitmap bmpSig, out string ErrorMessage, Size size)
        {
            bool flag = false;
            ErrorMessage = "";
            try
            {
                //MemoryStream memoryStream = new MemoryStream(sSigDataBin);
                //memoryStream.Position = 0L;
                //memoryStream.Seek(0L, SeekOrigin.Begin);

                //Bitmap bitmap1 = new Bitmap((Stream)memoryStream);
                //memoryStream.Position = 0L;
                //memoryStream.Seek(0L, SeekOrigin.Begin);
                //Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);
                //Graphics graphics = Graphics.FromImage(bitmap2);
                //graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //graphics.Clear(Color.White);
                //graphics.DrawImage(bitmap1, 0, 0);
                //bmpSig = bitmap1;
                //flag = true;

                //PRIMERX-9350
                MemoryStream memoryStream = new MemoryStream(sSigDataBin);
                memoryStream.Position = 0L;
                memoryStream.Seek(0L, SeekOrigin.Begin);
                Bitmap bitmap1 = new Bitmap((Stream)memoryStream);
                memoryStream.Position = 0L;
                memoryStream.Seek(0L, SeekOrigin.Begin);
                Bitmap bitmap2 = new Bitmap((Stream)memoryStream);
                Graphics graphics = Graphics.FromImage((Image)bitmap1);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                graphics.DrawImage((Image)bitmap2, 0, 0);
                bmpSig = bitmap1;
                flag = true;
            }
            catch (Exception ex)
            {
                try
                {
                    if (ex.Message.ToUpper() == "PARAMETER IS NOT VALID.")
                    {
                        SigDiplay.Signature oSigDisplay = new SigDiplay.Signature();
                        oSigDisplay.SetFormat("PointsLittleEndian");
                        oSigDisplay.SetData(sSigDataBin);

                        bmpSig = oSigDisplay.GetSignatureBitmap(10);
                        flag = true;
                    }
                    else
                    {
                        ErrorMessage = ex.Message;
                    }
                }
                catch (Exception exc)
                {
                    ErrorMessage = exc.Message;
                }
            }
            if (flag && size.Width != 0 && size.Height != 0)
            {
                CropBmpImages cropBmpImages = new CropBmpImages(size);
                bmpSig = cropBmpImages.CropBmp(bmpSig);
            }
            return flag;
        }

        public bool DrawSignatureMX(byte[] sSigDataBin, ref Bitmap bmpSig, out string ErrorMessage)
        {
            bool flag = false;
            ErrorMessage = "";
            try
            {
                //MemoryStream memoryStream = new MemoryStream(sSigDataBin);
                //memoryStream.Position = 0L;
                //memoryStream.Seek(0L, SeekOrigin.Begin);

                //Bitmap bitmap1 = new Bitmap((Stream)memoryStream);
                //memoryStream.Position = 0L;
                //memoryStream.Seek(0L, SeekOrigin.Begin);

                //Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);
                //Graphics graphics = Graphics.FromImage(bitmap2);
                //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //graphics.Clear(Color.White);
                //graphics.DrawImage(bitmap1, 0, 0);

                //bmpSig = bitmap1;
                //flag = true;

                //PRIMERX-9350
                MemoryStream memoryStream = new MemoryStream(sSigDataBin);
                memoryStream.Position = 0L;
                memoryStream.Seek(0L, SeekOrigin.Begin);
                Bitmap bitmap1 = new Bitmap((Stream)memoryStream);
                memoryStream.Position = 0L;
                memoryStream.Seek(0L, SeekOrigin.Begin);
                Bitmap bitmap2 = new Bitmap((Stream)memoryStream);
                Graphics graphics = Graphics.FromImage((Image)bitmap1);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                graphics.DrawImage((Image)bitmap2, 0, 0);
                bmpSig = bitmap1;
                flag = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return flag;
        }

        public bool DrawSignature(string spoints, ref Bitmap bmpSig, out string ErrorMessage, string sSigType)
        {
            this.bmpSignature = bmpSig;
            bool flag;
            if (sSigType == "D")
            {
                try
                {
                    flag = this.DrawSignatureHHP(spoints, ref bmpSig, out ErrorMessage);
                }
                catch (Exception ex)
                {
                    flag = false;
                    ErrorMessage = ex.Message;
                }
            }
            else if (sSigType == "M")
            {
                flag = this.DrawSignature(spoints, ref bmpSig);
                ErrorMessage = string.Empty;
            }
            else if (sSigType == "S")
            {
                try
                {
                    ErrorMessage = "";
                    flag = this.draw_signature(70, 20, spoints);
                }
                catch (Exception ex)
                {
                    flag = false;
                    ErrorMessage = ex.Message;
                }
            }
            else
                flag = this.CreatePointStruct(spoints, out ErrorMessage) && this.DrawToBmp();
            return flag;
        }

        private bool draw_signature(int baseX, int baseY, string data)
        {
            StringReader stringReader = new StringReader(data);
            Graphics graphics = Graphics.FromImage((Image)this.bmpSignature);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);
            string str;
            while ((str = stringReader.ReadLine()) != null)
            {
                if (str.Trim().Length > 0)
                {
                    string[] strArray1 = new string[4];
                    string[] strArray2 = str.Split(' ');
                    Pen pen = new Pen(Color.Black);
                    graphics.DrawLine(pen, baseX + int.Parse(strArray2[0].ToString()), baseY + int.Parse(strArray2[1].ToString()), baseX + int.Parse(strArray2[2].ToString()), baseY + int.Parse(strArray2[3].ToString()));
                }
            }
            return true;
        }

        public bool DrawSignaturePAX(string spoints, ref Bitmap bmp, out string sError)
        {
            bool flag = true;
            try
            {

                string str1 = string.Empty;
                str1 = spoints;
                var pointsCaret = str1.Split('^');

                Graphics graphics = Graphics.FromImage((Image)bmp);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);


                Point[] pointArray1 = new Point[pointsCaret.Length / 2];
                int num1 = 0;
                int index = 0;
                int startIndex = 0;
                while (startIndex < pointsCaret.Length - 2)
                {
                    ++num1;
                    Pen pen = new Pen(Color.Black, 3f);
                    var commSepX1Y1 = pointsCaret[startIndex].Split(',');
                    var commSepX2Y2 = pointsCaret[startIndex + 1].Split(',');
                    var x1 = commSepX1Y1[0].ToString();
                    var y1 = commSepX1Y1[1].ToString();
                    var x2 = commSepX2Y2[0].ToString();
                    var y2 = commSepX2Y2[1].ToString();

                    if (x1 != "~" && y1 != "~" && x1 != "0" && y1 != "0"
                        && x2 != "~" && y2 != "~" && x2 != "0" && y2 != "0")
                    {
                        graphics.DrawLine(pen, int.Parse(x1), int.Parse(y1), int.Parse(x2), int.Parse(y2));
                    }

                    //pointArray1[index] = point;
                    ++index;
                    startIndex += 1;
                }
                sError = "";
            }
            catch (Exception ex)
            {
                flag = false;
                sError = ex.Message.ToString();
            }
            return flag;
        }

        private bool DrawSignature(string spoints, ref Bitmap bmp)
        {
            bool flag = true;
            try
            {
                Point[] points = this.GetPoints(spoints);
                Graphics graphics = Graphics.FromImage((Image)bmp);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                Pen pen = new Pen(Color.Black, 3f);
                Point point1 = points[0];
                Point point2 = points[points.Length - 1];
                for (int index = 1; index < points.Length - 2; ++index)
                {
                    Point point3 = points[index];
                    Point point4 = points[index + 1];
                    if (point3.X != (int)ushort.MaxValue && point4.X != (int)ushort.MaxValue)
                        graphics.DrawLine(new Pen(Brushes.Black), point3.X / 7, point3.Y / 7, point4.X / 7, point4.Y / 7);
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        private Point[] GetPoints(string pointArray)
        {
            string str1 = string.Empty;
            str1 = pointArray;
            int num1 = 0;
            Point[] pointArray1 = new Point[pointArray.Length / 4];
            int index = 0;
            int startIndex = 0;
            while (startIndex < pointArray.Length)
            {
                ++num1;
                string str2;
                try
                {
                    str2 = pointArray.Substring(startIndex, 4);
                }
                catch (Exception ex)
                {
                    goto label_5;
                }
                int num2 = (int)(byte)str2[0];
                int num3 = (int)(byte)str2[1];
                int num4 = (int)(byte)str2[2];
                int num5 = (int)(byte)str2[3];
                Point point = new Point(256 * num3 + num2, 256 * num5 + num4);
                pointArray1[index] = point;
                ++index;
                label_5:
                startIndex += 4;
            }
            return pointArray1;
        }

        private bool CreatePointStruct(string spoints, out string ErrMessage)
        {
            this.SigLines = new ArrayList();
            bool flag = true;
            ErrMessage = "";
            ArrayList arrayList = new ArrayList();
            int length = spoints.Length;
            try
            {
                int startIndex = 0;
                while (startIndex < length - 1)
                {
                    string str1 = spoints.Substring(startIndex, 2);
                    if (str1 == "SS")
                    {
                        if (arrayList.Count > 0)
                        {
                            this.SigLines.Add((object)arrayList);
                            arrayList = new ArrayList();
                        }
                    }
                    else
                    {
                        startIndex += 2;
                        string str2 = spoints.Substring(startIndex, 2);
                        if (str2 == "SS")
                        {
                            str2 = str1;
                            startIndex -= 2;
                        }
                        Point point = new Point((int)Convert.ToInt16(str1, 16), (int)Convert.ToInt16(str2, 16));
                        arrayList.Add((object)point);
                    }
                    startIndex += 2;
                }
                if (arrayList.Count > 0)
                    this.SigLines.Add((object)arrayList);
            }
            catch (Exception ex)
            {
                flag = false;
                ErrMessage = ex.Message;
            }
            return flag;
        }

        private bool DrawToBmp()
        {
            bool flag = true;
            Graphics graphics = Graphics.FromImage((Image)this.bmpSignature);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);
            if (this.SigLines == null)
                return false;
            try
            {
                foreach (ArrayList sigLine in this.SigLines)
                {
                    if (sigLine != null && sigLine.Count > 0 && sigLine.Count > 1)
                    {
                        Point[] points = new Point[sigLine.Count];
                        sigLine.CopyTo((Array)points, 0);
                        graphics.DrawLines(Pens.Black, points);
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        private byte[] HexToBin(string sHexString)
        {
            int startIndex1 = 0;
            byte[] numArray1 = new byte[1000];
            int length1 = 1000;
            int length2 = 0;
            int startIndex2;
            for (; startIndex1 <= sHexString.Length - 1; startIndex1 = startIndex2 + 1)
            {
                ++length2;
                string sHexDigit1 = sHexString.Substring(startIndex1, 1);
                startIndex2 = startIndex1 + 1;
                if (startIndex2 <= sHexString.Length - 1)
                {
                    string sHexDigit2 = sHexString.Substring(startIndex2, 1);
                    short num = (short)((int)Convert.ToInt16((int)this.HexDigitToBin(sHexDigit1) * 16) | (int)this.HexDigitToBin(sHexDigit2));
                    if (length2 >= length1)
                    {
                        length1 *= 2;
                        byte[] numArray2 = new byte[length1];
                        numArray1.CopyTo((Array)numArray2, 0);
                        numArray1 = numArray2;
                    }
                    numArray1[length2 - 1] = Convert.ToByte(num);
                }
                else
                    break;
            }
            byte[] numArray3 = new byte[length2];
            for (int index = 0; index < length2; ++index)
                numArray3[index] = numArray1[index];
            return numArray3;
        }

        private short HexDigitToBin(string sHexDigit)
        {
            return Convert.ToInt16(sHexDigit, 16);
        }

        private bool DrawSignatureHHP(string sSigData, ref Bitmap obmpSig, out string sError)
        {
            sError = "";
            string pointArray = new HPDecomp()
            {
                DecompBytes = this.HexToBin(sSigData)
            }.GetPointArray();
            Graphics graphics = Graphics.FromImage((Image)obmpSig);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);
            int x1 = 0;
            int y1 = 0;
            int num1 = 0;
            int startIndex = 0;
            for (int index = pointArray.IndexOf("\r\n", 0); index != -1; index = pointArray.IndexOf("\r\n", index + 2))
            {
                string str1 = pointArray.Substring(startIndex, index - startIndex);
                ++num1;
                string str2 = str1;
                if (num1 >= 8)
                {
                    if (str2.IndexOf("*") >= 0)
                    {
                        int num2 = str2.IndexOf("x");
                        int int16_1 = (int)Convert.ToInt16(str2.Substring(num2 - 4, 4));
                        int int16_2 = (int)Convert.ToInt16(str2.Substring(num2 + 1, 4));
                        x1 = int16_1 / 3;
                        y1 = int16_2 / 3;
                    }
                    else
                    {
                        int num2 = str2.IndexOf("x");
                        int int16_1 = (int)Convert.ToInt16(str2.Substring(num2 - 4, 4));
                        int int16_2 = (int)Convert.ToInt16(str2.Substring(num2 + 1, 4));
                        int x2 = int16_1 / 3;
                        int y2 = int16_2 / 3;
                        graphics.DrawLine(Pens.Black, x1, y1, x2, y2);
                        x1 = x2;
                        y1 = y2;
                    }
                }
                startIndex = index + 2;
            }
            return true;
        }
    }
}
