using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace MMS.Device
{
    public static class IngenicoSignature
    {
        private class Rect
        {
            public int left { get; set; }
            public int top { get; set; }
            public int right { get; set; }
            public int bottom { get; set; }
            public int width() { return Math.Abs(left - right); }
            public int height() { return Math.Abs(top - bottom); }
        }

        private class SigPtData
        {
            public int numPoints { get; set; }
            public Point[] points { get; set; }
            public Rect bounds { get; set; }
        }

        private static SigPtData _sigPtData = new SigPtData();

        // convert and crop the 3-Byte-ASCII formatted signature
        //
        public static void Load(byte[] SigData3BA, int SigLen)
        {
            Load(SigData3BA, SigLen, true);
        }

        // convert the 3-Byte-ASCII formatted signature to point data
        // and save to the local static class variable.
        // Optionally crop the signature.
        //
        public static void Load(byte[] SigData3BA, int SigLen, bool Crop)
        {
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
                    _sigPtData.points[idx].Y = pts[idx].Y;
                else
                    _sigPtData.points[idx].Y = minY + (maxY - pts[idx].Y);
            }

            _sigPtData.bounds = new Rect();
            _sigPtData.numPoints = numPts;
            _sigPtData.bounds.left = minX;
            _sigPtData.bounds.top = minY;
            _sigPtData.bounds.right = maxX + minX;
            _sigPtData.bounds.bottom = maxY + minY;

            if (Crop)
                IngenicoSignature.Crop();
        }

        // Crop the white space from around the signature
        //
        public static void Crop()
        {
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

        // draw the signature and save it to the file name/type specified
        //
        public static string Draw(string FileName, ImageFormat ImageType)
        {
            int width = (_sigPtData.bounds.width() / 16) * 16;		// Make the dimensions evenly divisible by 16
            int height = (_sigPtData.bounds.height() / 16) * 16;

            width += (_sigPtData.bounds.width() % 16 > 0) ? 16 : 0;	// add 16 if we truncated above
            height += (_sigPtData.bounds.height() % 16 > 0) ? 16 : 0;

            Bitmap bmp = new Bitmap(width, height);
            Graphics gfx = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Black);
            gfx.Clear(Color.White);

            bool penUp = false;
            Point lastPt = new Point(0, 0);

            for (int i = 0; i < _sigPtData.numPoints; i++)
            {
                if (_sigPtData.points[i].X == -1 && _sigPtData.points[i].Y == -1)
                    penUp = true;
                else
                {
                    if (!penUp)
                        gfx.DrawLine(pen, lastPt, _sigPtData.points[i]);

                    lastPt = _sigPtData.points[i];
                    penUp = false;
                }
            }

            gfx.Save();
            byte[] arr = new byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Close();               
                arr = ms.ToArray();
            }
            char[] chars = arr.Select(s => (char)s).ToArray();
            return new string(chars);
        }

        public static Bitmap Draw()
        {
            int width = (_sigPtData.bounds.width() / 16) * 16;		// Make the dimensions evenly divisible by 16
            int height = (_sigPtData.bounds.height() / 16) * 16;

            width += (_sigPtData.bounds.width() % 16 > 0) ? 16 : 0;	// add 16 if we truncated above
            height += (_sigPtData.bounds.height() % 16 > 0) ? 16 : 0;

            Bitmap bmp = new Bitmap(width, height);
            Graphics gfx = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Black);
            gfx.Clear(Color.White);

            bool penUp = false;
            Point lastPt = new Point(0, 0);

            for (int i = 0; i < _sigPtData.numPoints; i++)
            {
                if (_sigPtData.points[i].X == -1 && _sigPtData.points[i].Y == -1)
                    penUp = true;
                else
                {
                    if (!penUp)
                        gfx.DrawLine(pen, lastPt, _sigPtData.points[i]);

                    lastPt = _sigPtData.points[i];
                    penUp = false;
                }
            }

            gfx.Save();

            return bmp;
        }
        // Resize the signature to the desired Height and Width.
        // If either value is 0, scale to the same % as the other value.
        //
        public static void Resize(int Height, int Width)
        {
            double scaleX = 0, scaleY = 0;

            // calculate the scale to fit in our extents
            if (Width > 0)
                scaleX = ((double)_sigPtData.bounds.width() / (double)Width);

            if (Height > 0)
            {
                scaleY = ((double)_sigPtData.bounds.height() / (double)Height);
                if (Width < 1)
                    scaleX = scaleY;
            }
            else
                scaleY = scaleX;

            // scale the coordinates
            for (int i = 0; i < _sigPtData.numPoints; i++)
            {
                //don't scale a pen up coordinate
                if (_sigPtData.points[i].X != -1 || _sigPtData.points[i].Y != -1)
                {
                    if (((double)_sigPtData.points[i].X / scaleX) % (double)1 >= 0.5)
                        _sigPtData.points[i].X = (ushort)((double)_sigPtData.points[i].X / scaleX) + 1;
                    else
                        _sigPtData.points[i].X = (ushort)((double)_sigPtData.points[i].X / scaleX);

                    if (((double)_sigPtData.points[i].Y / scaleY % (double)1) >= 0.5)
                        _sigPtData.points[i].Y = (ushort)((double)_sigPtData.points[i].Y / scaleY) + 1;
                    else
                        _sigPtData.points[i].Y = (ushort)((double)_sigPtData.points[i].Y / scaleY);
                }
            }

            // scale the signature bounds
            _sigPtData.bounds.left = (int)((double)_sigPtData.bounds.left / scaleX);
            _sigPtData.bounds.top = (int)((double)_sigPtData.bounds.top / scaleY);
            _sigPtData.bounds.right = (int)((double)_sigPtData.bounds.right / scaleX);
            _sigPtData.bounds.bottom = (int)((double)_sigPtData.bounds.bottom / scaleY);
        }
    }
}
