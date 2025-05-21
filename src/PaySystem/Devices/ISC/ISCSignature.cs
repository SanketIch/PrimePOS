using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using NLog;

namespace EDevice
{
    /// <summary>
    /// Return signature format
    /// </summary>
    public enum SigFormat
    {
        /// <summary>
        /// Return Signature in Byte form
        /// </summary>
        InByte,
        /// <summary>
        /// Return Signature in string form. To get the signature from string use Convert.FromBase64String()
        /// </summary>
        InString,
        /// <summary>
        /// Return Signature in Image form
        /// </summary>
        inBmp
    }

    public enum SigStatus
    {
        IsAccepted,
        IsCancel
    }

    internal class ISCSignature:IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Rectange for signature
        /// </summary>
        class Rect
        {
            public int left { get; set; }
            public int top { get; set; }
            public int right { get; set; }
            public int bottom { get; set; }
            public int width() { return Math.Abs(left - right); }
            public int height() { return Math.Abs(top - bottom); }
        }

        /// <summary>
        /// Point for signature
        /// </summary>
        class SigPoint
        {
            public int numPoints { get; set; }
            public Point[] points { get; set; }
            public Rect bounds { get; set; }
        }

        private SigPoint _sigPtData = new SigPoint();      

        /// <summary>
        /// Process the Signature bytes and Crop the signature
        /// </summary>
        /// <param name="SigData3BA"></param>
        /// <param name="SigLen"></param>
        /// <param name="Crop"></param>
        public void ProcessSignature(byte[] SigData3BA, int SigLen, bool Crop)
        {
            logger.Trace("In ProcessSignature Drawing Siganture From DataPOints Recieved");
            Point[] pts = new Point[SigLen];

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            int X = 0, Y = 0;
            int idx, numPts = 0;
            short DX, DY;

            pts[numPts].X = -1;
            pts[numPts].Y = -1;
            numPts++;

            for (idx = 0; idx < SigLen; idx++)
            {
                //pen up
                if (SigData3BA[idx] == 0x70)
                {
                    pts[numPts].X = -1;
                    pts[numPts].Y = -1;
                    numPts++;

                    X = 0;
                    Y = 0;
                }
                else if (SigData3BA[idx] >= 0x60 && SigData3BA[idx] <= 0x6f)
                {
                    X = (((SigData3BA[idx] - 0x60) & 0x0c) << 7) |
                          ((SigData3BA[idx + 1] - 0x20) << 3) |
                          (((SigData3BA[idx + 3] - 0x20) & 0x38) >> 3);


                    Y = (((SigData3BA[idx] - 0x60) & 0x03) << 9) |
                          ((SigData3BA[idx + 2] - 0x20) << 3) |
                          (((SigData3BA[idx + 3] - 0x20) & 0x07));

                    idx += 3;

                    pts[numPts].X = X;
                    pts[numPts].Y = Y;
                    numPts++;

                    if (X < minX) minX = X;
                    if (X > maxX) maxX = X;
                    if (Y < minY) minY = Y;
                    if (Y > maxY) maxY = Y;
                }
                else
                {
                    DX = (short)((((SigData3BA[idx] - 0x20)) << 3) | (((SigData3BA[idx + 2] - 0x20) & 0x38) >> 3));
                    //may have to sign extend offset
                    DX = (short)((short)(DX << 7) >> 7);
                    idx++;

                    DY = (short)(((SigData3BA[idx] - 0x20) << 3) | (((SigData3BA[idx + 1] - 0x20) & 0x07)));
                    //may have to sign extend offset
                    DY = (short)((short)(DY << 7) >> 7);
                    idx++;

                    X += DX;
                    Y += DY;

                    pts[numPts].X = X;
                    pts[numPts].Y = Y;
                    numPts++;

                    if (X < minX) minX = X;
                    if (X > maxX) maxX = X;
                    if (Y < minY) minY = Y;
                    if (Y > maxY) maxY = Y;
                }
            }

            _sigPtData.points = new Point[numPts];
            for (idx = 0; idx < numPts; idx++)
            {
                _sigPtData.points[idx].X = pts[idx].X;

                if (pts[idx].Y == -1L)
                {
                    _sigPtData.points[idx].Y = pts[idx].Y;
                }
                else
                {
                    _sigPtData.points[idx].Y = minY + (maxY - pts[idx].Y);
                }
            }

            _sigPtData.bounds = new Rect();
            _sigPtData.numPoints = numPts;
            _sigPtData.bounds.left = minX;
            _sigPtData.bounds.top = minY;
            _sigPtData.bounds.right = maxX + minX;
            _sigPtData.bounds.bottom = maxY + minY;

            if (Crop)
            {
                this.Crop();
            }               
        }

        private void Crop()
        {
            logger.Trace("In Crop()");
            int minWidth = Math.Min(_sigPtData.bounds.left, _sigPtData.bounds.right);
            int minHeight = Math.Min(_sigPtData.bounds.top, _sigPtData.bounds.bottom);

            // crop the signature so that it starts at 0,0
            for (int i = 0; i < _sigPtData.numPoints; i++)
            {
                if (_sigPtData.points[i].X != -1 && _sigPtData.points[i].Y != -1)
                {
                    _sigPtData.points[i].X -= minWidth;
                    _sigPtData.points[i].Y -= minHeight;
                }
            }

            // adjust the bounds of the rectangle of the signature
            _sigPtData.bounds.right -= (_sigPtData.bounds.left + minWidth);
            _sigPtData.bounds.bottom -= (_sigPtData.bounds.top + minHeight);
            _sigPtData.bounds.left = 0;
            _sigPtData.bounds.top = 0;
        }

        public string Draw(SigFormat sform)
        {
            logger.Trace("In Draw()");
            try
            {
                int width = (_sigPtData.bounds.width() / 16) * 16;
                int height = (_sigPtData.bounds.height() / 16) * 16;

                width += (_sigPtData.bounds.width() % 16 > 0) ? 16 : 0;
                height += (_sigPtData.bounds.height() % 16 > 0) ? 16 : 0;

                using (Bitmap bmp = new Bitmap(width, height))
                {
                    using (Graphics gImg = Graphics.FromImage(bmp))
                    {
                        Pen pen = new Pen(Color.Black);
                        gImg.Clear(Color.White);

                        bool penUp = false;
                        Point lastPt = new Point(0, 0);

                        for (int i = 0; i < _sigPtData.numPoints; i++)
                        {
                            if (_sigPtData.points[i].X == -1 && _sigPtData.points[i].Y == -1)
                            {
                                penUp = true;
                            }
                            else
                            {
                                if (!penUp)
                                {
                                    gImg.DrawLine(pen, lastPt, _sigPtData.points[i]);
                                }
                                lastPt = _sigPtData.points[i];
                                penUp = false;
                            }
                        }
                        gImg.Save();
                    }

                    if (sform == SigFormat.InString)
                    {
                        string sign = string.Empty;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            ms.Close();
                            byte[] bArry = ms.GetBuffer();
                            sign = Convert.ToBase64String(bArry);
                        }
                        return sign;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "error occured while drawing signature " + ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public void Dispose()
        {
            _sigPtData = null;
            GC.Collect();
        }

        ~ISCSignature()
        {
            Dispose();
        }
    }
}
