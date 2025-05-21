using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using NLog;

namespace POS_Core.Business_Tier
{
    ///<Author>Manoj Kumar</Author>
    ///<Date>6/6/2014</Date>
    /// <summary>
    /// Use to Crop the Image to a particular size. This allow the Signature Image to 
    /// be visible in any report. The entire Image will not resize as was previously
    /// </summary>
    class ImageCroping
    {
        /// <summary>Required size of the Image</summary>
        private Size size;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Constructor take the Size of the New Image.
        /// This will Resize the Image to the Size that is pass
        /// </summary>
        /// <param name="s"></param>
        public ImageCroping(Size s)
        {
            this.size = s;
        }

        ///<summary>
        /// Take in a BMP Image to get the Color of each pixels
        /// and store them in a byte[][] for processing. Return a byte[][]
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns>byte[][]</returns>
        private byte[][] GetColors(Bitmap bmp)
        {
            //Create the attribute of the bmp
            BitmapData bmpImage = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            IntPtr ptr = bmpImage.Scan0; //First scan line of the bmp
            int numOfPix = bmp.Width * bmp.Height; //# of Pixels in the image
            int padding = bmpImage.Stride - bmp.Width; //the scan width * the bmp width
            long i = 0;
            int ct = (bmp.Width - bmp.Height) / 3; //28center image
            byte[] red = new byte[numOfPix], green = new byte[numOfPix], blue = new byte[numOfPix], rgb = new byte[numOfPix]; //Arrays for the colors
            Marshal.Copy(ptr, rgb, 0, numOfPix); //copy from the line scan pointer to the rgb array

            try
            {
                //check each pixel 
                for (int x = 0; x <= numOfPix; x++)
                {
                    if (x == (bmpImage.Stride * ct - padding))
                    {
                        x += padding;
                        ct++;
                    }
                    if (x < numOfPix)
                    {
                        red[i] = rgb[x];
                        green[i] = rgb[x];
                        blue[i] = rgb[x];
                        i++;
                    }
                }
                return new byte[3][] { red, green, blue };
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetColors()");
                throw new Exception(ex.ToString());
            }
            finally
            {
                bmp.UnlockBits(bmpImage);
            }
        }


        /// <summary>
        /// Crop a Bitmap(BMP) Image to any size.
        /// Input a BMP Image and Size
        /// Return a Bitmap(BMP) of the Crop size
        /// </summary>
        /// <param name="sig"></param>
        /// <returns></returns>
        public Bitmap CropBmp(Bitmap sig)
        {
            var pix = GetColors(sig);
            int h = sig.Height - 1;
            int w = sig.Width;
            int top = 0;
            int bottom = 0;
            int left = 0;
            int right = 0;
            int white = 0;
            int Threshold = 50; //99% this is the more white pixel you want to detect
            bool prevColor = false; //Previous pix

            try
            {
                for (int i = 0; i < pix[0].Length; i++)
                {
                    int x = (i % (w));
                    int y = (int)(Math.Floor((decimal)(i / w)));
                    int thres = 255 * Threshold / 100;
                    //White space checking against the Threshold
                    if (pix[0][i] >= thres && pix[1][i] >= thres && pix[2][i] >= thres)
                    {
                        white++;
                        right = (x > right && white == 1) ? x : right;
                    }
                    else
                    {
                        left = (x < left && white >= 1) ? x : left;
                        right = (x == w - 1 && white == 0) ? w - 1 : right;
                        white = 0;
                    }
                    if (white == w)
                    {
                        top = (y - top < 3) ? y : top;
                        bottom = x - y - top;
                    }
                    left = (x == 0 && white == 0) ? 0 : left;
                    bottom = x - y - top;
                    if (x == w - 1)
                    {
                        prevColor = (white < w) ? true : false;
                        white = 0;
                    }
                }
                right = (right == 0) ? w : right;
                left = (left == w) ? 0 : left;

                top = top < 1 ? h : top;

                //Now crop the image
                if (Math.Abs(bottom - top) > 0)
                {
                    //Croping of the original to remove the white space.
                    try
                    {
                        Bitmap bmpCrop = sig.Clone(new Rectangle(left, top, (right - left), Math.Abs(bottom)), sig.PixelFormat);
                        Bitmap Resize = new Bitmap(bmpCrop, size);
                        return Resize;
                    }
                    catch
                    {
                        Bitmap Resize = new Bitmap(sig, size);
                        return Resize;
                    }
                }
                else
                {
                    return sig; //Return the original if croping failed
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "CropBmp()");
                throw new Exception(ex.ToString());
            }
            finally
            {
                pix = null; //clear all the byte
            }
        }
    }
}
