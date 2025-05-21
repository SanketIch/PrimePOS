// Decompiled with JetBrains decompiler
// Type: SigDiplay.HPDecomp
// Assembly: SigDiplay, Version=1.0.5254.20486, Culture=neutral, PublicKeyToken=null
// MVID: F175A3B1-8C63-45A5-8A8A-4FE3E5A5D870
// Assembly location: E:\Test\SigDiplay.dll

using System;

namespace SigDiplay
{
    public class HPDecomp
    {
        private short stat = 0;
        private short mode = 0;
        private short lstepflg = 0;
        private long OUT_X_RESOL = 273;
        private long OUT_Y_RESOL = 455;
        private byte ALGORM = 17;
        private byte DOWN_PEN = 8;
        private short FLGIB0 = 31414;
        private byte UP_PEN = 9;
        private byte INFO_BYTE = 10;
        private byte STEP_LONG = 11;
        private byte DRAWTO = 1;
        private byte MOVETO = 0;
        private byte PENDOWN = 16;
        public static int mmm;
        private short[] xdecode;
        private short[] ydecode;
        private short accur;
        private short bites;
        private short xresol;
        private short yresol;
        private short x;
        private short y;
        private short dx;
        private short dy;
        private short halflstep;
        private byte[] bDecompress;

        public HPDecomp()
        {
            this.xdecode = new short[16]
            {
        (short) -2,
        (short) -1,
        (short) 0,
        (short) 1,
        (short) 2,
        (short) 2,
        (short) 2,
        (short) 2,
        (short) 2,
        (short) 1,
        (short) 0,
        (short) -1,
        (short) -2,
        (short) -2,
        (short) -2,
        (short) -2
            };
            this.ydecode = new short[16]
            {
        (short) 2,
        (short) 2,
        (short) 2,
        (short) 2,
        (short) 2,
        (short) 1,
        (short) 0,
        (short) -1,
        (short) -2,
        (short) -2,
        (short) -2,
        (short) -2,
        (short) -2,
        (short) -1,
        (short) 0,
        (short) 1
            };
        }

        public byte[] DecompBytes
        {
            set
            {
                this.bDecompress = value;
            }
            get
            {
                return this.bDecompress;
            }
        }

        public string GetPointArray()
        {
            this.stat = (short)0;
            this.accur = (short)0;
            this.bites = (short)0;
            this.xresol = (short)0;
            this.yresol = (short)0;
            this.mode = (short)0;
            this.lstepflg = (short)0;
            this.x = (short)0;
            this.y = (short)0;
            this.dx = (short)0;
            this.dy = (short)0;
            this.halflstep = (short)0;
            uint[] numArray1 = new uint[1000];
            uint[] numArray2 = new uint[1000];
            byte[] numArray3 = new byte[1000];
            long length = 1000;
            byte pen = 0;
            short npts = 0;
            int num1 = 1;
            int x1 = 0;
            int x2 = 0;
            short num2 = 0;
            SigFilePoint[] p = new SigFilePoint[2]
            {
        new SigFilePoint(),
        new SigFilePoint()
            };
            this.stat = (short)0;
            int index1 = 0;
            for (int index2 = 0; index2 < this.bDecompress.Length; ++index2)
            {
                p[0].X = 0U;
                p[0].Y = 0U;
                short num3 = this.PCDecompress(this.bDecompress[index2], p, ref npts, ref pen);
                if (num1 > 0 && (int)num3 > 0)
                {
                    num1 = 0;
                    x1 = (int)p[0].X;
                    x2 = (int)p[0].Y;
                }
                else if (!this.ISPENDOWN(pen))
                {
                    num2 = (short)0;
                }
                else
                {
                    for (short index3 = 0; (int)index3 < (int)npts; ++index3)
                    {
                        if ((long)index1 >= length)
                        {
                            length *= 2L;
                            uint[] numArray4 = new uint[length];
                            numArray1.CopyTo((Array)numArray4, 0);
                            numArray1 = numArray4;
                            uint[] numArray5 = new uint[length];
                            numArray2.CopyTo((Array)numArray5, 0);
                            numArray2 = numArray5;
                            byte[] numArray6 = new byte[length];
                            numArray3.CopyTo((Array)numArray6, 0);
                            numArray3 = numArray6;
                        }
                        numArray1[index1] = p[(int)index3].X;
                        numArray2[index1] = p[(int)index3].Y;
                        numArray3[index1] = (byte)num2;
                        ++index1;
                        num2 = (short)1;
                    }
                }
            }
            if (x1 == 0)
                x1 = 273;
            if (x2 == 0)
                x2 = 455;
            int num4 = x1;
            int num5 = x2;
            int num6 = this.SCALE(x1, 375, 100);
            int num7 = this.SCALE(x2, 225, 100);
            string str1 = "Signature Header:\r\n" + " points= " + index1.ToString() + "\r\n" + " width = " + num6.ToString() + ", height = " + num7.ToString() + "\r\n" + " hdpi  =  " + num4.ToString() + ", vdpi   =  " + num5.ToString() + "\r\n\r\n" + "Signature points data:\r\n" + " index  type     x/y point position\r\n";
            for (int index2 = 0; index2 < index1; ++index2)
            {
                string str2 = str1 + index2.ToString().PadLeft(6, ' ') + "  ";
                str1 = ((int)numArray3[index2] != (int)this.DRAWTO ? str2 + "MoveTo*  " : str2 + "DrawTo   ") + numArray1[index2].ToString().PadLeft(4, ' ') + "x " + numArray2[index2].ToString().PadLeft(3, ' ') + "\r\n";
            }
            return str1;
        }

        private int SCALE(int x, int n, int d)
        {
            return d > 0 ? (int)((long)x * (long)n / (long)d) : 0;
        }

        private bool ISPENDOWN(byte test)
        {
            return ((int)test & (int)this.PENDOWN) == (int)this.PENDOWN;
        }

        private short PCDecompress(byte c, SigFilePoint[] p, ref short npts, ref byte pen)
        {
            int index = 0;
            if ((int)this.stat == 0)
                this.mode = this.lstepflg = (short)0;
            if ((int)this.stat > 7 && (int)this.lstepflg != 0)
            {
                if ((int)this.lstepflg == 1)
                {
                    ++this.lstepflg;
                    this.halflstep = (short)(sbyte)c;
                }
                else
                {
                    this.lstepflg = (short)1;
                    this.dx = this.halflstep;
                    this.x += this.dx;
                    p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                    this.dy = (short)(sbyte)c;
                    this.y += this.dy;
                    p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                    ++index;
                    if ((int)Math.Max(Math.Abs(this.halflstep), (short)Math.Abs((sbyte)c)) < 10)
                        this.lstepflg = (short)0;
                }
            }
            else
            {
                switch (this.stat)
                {
                    case 0:
                        if ((int)c != (int)this.ALGORM)
                            return 1;
                        break;
                    case 1:
                        this.accur = (short)((double)((int)c & 128) / Math.Pow(2.0, 7.0));
                        this.xresol = (short)((int)c & (int)sbyte.MaxValue);
                        break;
                    case 2:
                        this.bites = (short)((double)((int)c & 128) / Math.Pow(2.0, 7.0));
                        this.yresol = (short)((int)c & (int)sbyte.MaxValue);
                        p[index].X = (uint)this.OUT_X_RESOL;
                        p[index].Y = (uint)this.OUT_Y_RESOL;
                        ++index;
                        break;
                    case 3:
                        pen = c;
                        break;
                    case 4:
                        if ((int)c != (int)this.DOWN_PEN && (double)c != (double)this.DOWN_PEN * Math.Pow(2.0, 4.0))
                            return 3;
                        pen = (byte)176;
                        break;
                    case 5:
                        this.x = (short)((double)c * Math.Pow(2.0, (double)this.accur));
                        break;
                    case 6:
                        if ((int)this.bites > 0)
                        {
                            ++this.stat;
                            this.y = (short)((double)c * Math.Pow(2.0, (double)this.accur));
                            p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                            p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                            ++index;
                            break;
                        }
                        this.x = (short)((double)this.x * Math.Pow(2.0, 4.0) + (double)((int)c & 240) / Math.Pow(2.0, 4.0) * Math.Pow(2.0, (double)this.accur));
                        this.y = (short)((double)((int)c & 15) * Math.Pow(2.0, 8.0) * Math.Pow(2.0, (double)this.accur));
                        break;
                    case 7:
                        p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                        p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                        ++index;
                        break;
                    case 31415:
                        pen = c;
                        this.stat = (short)10;
                        break;
                    default:
                        byte num1 = (byte)((uint)c & 15U);
                        byte num2 = (byte)((double)((int)c & 240) / Math.Pow(2.0, 4.0));
                        if ((int)this.mode > 0)
                        {
                            if ((int)num1 == 0)
                            {
                                this.x += this.xdecode[(int)num1];
                                this.y += this.ydecode[(int)num1];
                                p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                                p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                                ++index;
                                this.mode = (short)0;
                                if ((int)num2 > 0)
                                {
                                    this.x += this.xdecode[(int)num2];
                                    this.y += this.ydecode[(int)num2];
                                    p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                                    p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                                    ++index;
                                    break;
                                }
                                this.mode = (short)1;
                                break;
                            }
                            if ((int)num2 > 0)
                            {
                                this.dx = (short)((int)num1 - 8);
                                this.x += this.dx;
                                this.dy = (short)((int)num2 - 8);
                                this.y += this.dy;
                                p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                                p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                                if ((int)Math.Max(Math.Abs(this.dx), Math.Abs(this.dy)) < 4)
                                    this.mode = (short)0;
                                ++index;
                                break;
                            }
                            switch (num1)
                            {
                                case 1:
                                    this.mode = (short)0;
                                    break;
                                case 8:
                                    pen |= (byte)16;
                                    this.stat = (short)4;
                                    break;
                                case 9:
                                    pen &= (byte)239;
                                    break;
                                case 10:
                                    this.stat = this.FLGIB0;
                                    break;
                                case 11:
                                    this.lstepflg = (short)1;
                                    break;
                            }
                        }
                        else if ((int)num1 == 0)
                        {
                            switch (num2)
                            {
                                case 0:
                                    this.x += this.xdecode[(int)num1];
                                    this.y += this.ydecode[(int)num1];
                                    p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                                    p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                                    ++index;
                                    break;
                                case 1:
                                    this.mode = (short)1;
                                    break;
                                case 8:
                                    pen |= (byte)16;
                                    this.stat = (short)4;
                                    break;
                                case 9:
                                    pen &= (byte)239;
                                    break;
                                case 10:
                                    this.stat = this.FLGIB0;
                                    break;
                            }
                        }
                        else
                        {
                            this.x += this.xdecode[(int)num1];
                            this.y += this.ydecode[(int)num1];
                            p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                            p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                            ++index;
                            if ((int)num2 > 0)
                            {
                                this.x += this.xdecode[(int)num2];
                                this.y += this.ydecode[(int)num2];
                                p[index].X = (uint)((ulong)this.x * (ulong)this.OUT_X_RESOL / (ulong)this.xresol);
                                p[index].Y = (uint)((ulong)this.y * (ulong)this.OUT_Y_RESOL / (ulong)this.yresol);
                                ++index;
                                break;
                            }
                            this.mode = (short)1;
                            break;
                        }
                        break;
                }
            }
            ++this.stat;
            npts = (short)index;
            return 0;
        }
    }
}
