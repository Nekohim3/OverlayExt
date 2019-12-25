using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.WIC;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using InterpolationMode = SharpDX.Direct2D1.InterpolationMode;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;

namespace OverlayExt
{
    public class Sbmp : IDisposable
    {
        public byte[] Buf;
        public int Height;
        public int Width;

        public Sbmp(byte[] b, int w, int h)
        {
            Buf = b;
            Width = w;
            Height = h;
        }

        public Sbmp(Bitmap source)
        {
            if (source.PixelFormat != PixelFormat.Format32bppArgb)
                source = Convert32(source);
            Width = source.Width;
            Height = source.Height;
            Buf = new byte[Width * Height * 4];
            var rect = new System.Drawing.Rectangle(0, 0, Width, Height);
            var bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite, source.PixelFormat);
            Marshal.Copy(bitmapData.Scan0, Buf, 0, Buf.Length);
            bitmapData = null;
            source.Dispose();
        }

        public Sbmp(string str)
        {
            var source = new Bitmap(str);
            if (source.PixelFormat != PixelFormat.Format32bppArgb)
                source = Convert32(source);
            Width = source.Width;
            Height = source.Height;
            Buf = new byte[Width * Height * 4];
            var rect = new System.Drawing.Rectangle(0, 0, Width, Height);
            var bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite, source.PixelFormat);
            Marshal.Copy(bitmapData.Scan0, Buf, 0, Buf.Length);
            bitmapData = null;
            source.Dispose();
        }

        public Sbmp()
        {

        }

        public Bitmap Resize(int w, int h)
        {
            var newImage = new Bitmap(w, h);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(GetBmp(), new System.Drawing.Rectangle(0, 0, w, h));
            }

            return newImage;
        }

        public static Bitmap Resize(string str, int w, int h)
        {
            var newImage = new Bitmap(w, h);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(new Bitmap(str), new System.Drawing.Rectangle(0, 0, w, h));
            }

            return newImage;
        }

        public Bitmap Convert32(Bitmap orig)
        {
            var clone = new Bitmap(orig.Width, orig.Height, PixelFormat.Format32bppPArgb);
            using (var gr = Graphics.FromImage(clone))
                gr.DrawImage(orig, new System.Drawing.Rectangle(0, 0, clone.Width, clone.Height));
            return clone;
        }

        public Color GetPixel(int x, int y)
        {
            if (x < 0 || y < 0)
                return Color.FromArgb(0, 0, 0, 0);
            var i = (y * Width + x) * 4;
            if (i > Buf.Length - 4)
                return Color.FromArgb(0, 0, 0, 0);
            var b = Buf[i];
            var g = Buf[i + 1];
            var r = Buf[i + 2];
            var a = Buf[i + 3];
            return Color.FromArgb(a, r, g, b);
        }

        public void SetPixel(int x, int y, Color color)
        {
            var i = (y * Width + x) * 4;
            Buf[i] = color.B;
            Buf[i + 1] = color.G;
            Buf[i + 2] = color.R;
            Buf[i + 3] = color.A;
        }

        public Sbmp Copy() => new Sbmp(Buf.ToArray(), Width, Height);

        public Sbmp Crop(System.Drawing.Rectangle r)
        {
            var bbuf = new byte[r.Width * r.Height * 4];
            for (int i = r.X, ii = 0; i < r.X + r.Width; i++, ii++)
            {
                for (int j = r.Y, jj = 0; j < r.Y + r.Height; j++, jj++)
                {
                    var x = (jj * r.Width + ii) * 4;
                    var color = GetPixel(i, j);
                    bbuf[x] = color.B;
                    bbuf[x + 1] = color.G;
                    bbuf[x + 2] = color.R;
                    bbuf[x + 3] = color.A;
                }
            }
            return new Sbmp(bbuf, r.Width, r.Height);
        }

        public Bitmap GetBmp()
        {
            var bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            var bd = bmp.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            var iptr = bd.Scan0;
            Marshal.Copy(Buf, 0, iptr, Buf.Length);
            bmp.UnlockBits(bd);
            return bmp;
        }

        public SharpDX.Direct2D1.Bitmap GetSDXBmp(RenderTarget rt)
        {
            var imagingFactory = new ImagingFactory();
            var ms = new MemoryStream();
            GetBmp().Save(ms, ImageFormat.Png);
            var bitmapDecoder = new BitmapDecoder(imagingFactory, ms, DecodeOptions.CacheOnDemand);
            var frame = bitmapDecoder.GetFrame(0);

            var converter = new FormatConverter(imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);
            var result = SharpDX.Direct2D1.Bitmap.FromWicBitmap(g.Graphics.GetRenderTarget(), converter);

            //var image = GetBmp();
            //if (image == null)
            //    throw new ArgumentNullException(nameof(image));

            //var imageData = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            //var dataStream = new DataStream(imageData.Scan0, imageData.Stride * imageData.Height, true, false);

            //var properties = new BitmapProperties
            //{
            //    PixelFormat = new SharpDX.Direct2D1.PixelFormat
            //    {
            //        Format = Format.B8G8R8A8_UNorm,
            //        AlphaMode = AlphaMode.Premultiplied
            //    }
            //};

            //var result = new SharpDX.Direct2D1.Bitmap(rt, new Size2(image.Width, image.Height), dataStream, imageData.Stride, properties);
            //image.UnlockBits(imageData);
            //return result;
            return result;

        }

        public void Save(string fileName) => GetBmp().Save(fileName);

        public bool Check(Sbmp b, Point p)
        {
            for (var i = 0; i < b.Width; i++)
                for (var j = 0; j < b.Height; j++)
                    if (GetPixel(p.X + i, p.Y + j) != b.GetPixel(i, j))
                        return false;
            return true;
        }

        public bool Check(Sbmp b, Point p, int thr, int npc)
        {
            var counter = 0;
            for (var i = 0; i < b.Width; i++)
            {
                for (var j = 0; j < b.Height; j++)
                {
                    var c = GetPixel(p.X + i, p.Y + j);
                    var e = b.GetPixel(i, j);
                    if (Math.Abs(c.R - e.R) <= thr && Math.Abs(c.G - e.G) <= thr &&
                        Math.Abs(c.B - e.B) <= thr)
                        counter++;
                }
            }
            double all = b.Width * b.Height;
            var proc = counter * 100 / all;
            return proc >= npc;
        }

        public bool Check(Sbmp b, int x, int y, int thr, int npc)
        {
            var counter = 0;
            for (var i = 0; i < b.Width; i++)
            {
                for (var j = 0; j < b.Height; j++)
                {
                    var c = GetPixel(x + i, y + j);
                    var e = b.GetPixel(i, j);
                    if (Math.Abs(c.R - e.R) <= thr && Math.Abs(c.G - e.G) <= thr &&
                        Math.Abs(c.B - e.B) <= thr)
                        counter++;
                }
            }
            double all = b.Width * b.Height;
            var proc = counter * 100 / all;
            return proc >= npc;
        }

        public bool Find(Sbmp b, Point p, int rad)
        {
            for (var i = p.X - rad; i < p.X + rad; i++)
                for (var j = p.Y - rad; j < p.Y + rad; j++)
                    if (Check(b, new Point(i, j)))
                        return true;
            return false;
        }

        public bool Find(Sbmp b, Point p, int radx, int rady)
        {
            for (var i = p.X - radx; i < p.X + radx; i++)
                for (var j = p.Y - rady; j < p.Y + rady; j++)
                    if (Check(b, new Point(i, j)))
                        return true;
            return false;
        }

        public bool Find(Sbmp b, System.Drawing.Rectangle rad)
        {
            for (var i = rad.X; i < rad.Width; i++)
                for (var j = rad.Y; j < rad.Height; j++)
                    if (Check(b, new Point(i, j)))
                        return true;
            return false;
        }
        public bool Find(Sbmp b, Point p, int rad, int thr, int npc)
        {
            for (var i = p.X - rad; i < p.X + rad; i++)
                for (var j = p.Y - rad; j < p.Y + rad; j++)
                    if (Check(b, new Point(i, j), thr, npc))
                        return true;
            return false;
        }

        public bool Find(Sbmp b, System.Drawing.Rectangle rad, int thr, int npc)
        {
            for (var i = rad.X; i < rad.Width; i++)
                for (var j = rad.Y; j < rad.Height; j++)
                    if (Check(b, new Point(i, j), thr, npc))
                        return true;
            return false;
        }

        public bool Find(Sbmp b, Point p, int radx, int rady, int thr, int npc)
        {
            for (var i = p.X - radx; i < p.X + radx; i++)
                for (var j = p.Y - rady; j < p.Y + rady; j++)
                    if (Check(b, new Point(i, j), thr, npc))
                        return true;
            return false;
        }

        public void Dispose()
        {
            Buf = null;
        }
    }
}
