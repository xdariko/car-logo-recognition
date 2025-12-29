using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using AForge.Imaging;

namespace AForge.WindowsForms
{
    public static class ImageUtils
    {
        public static Bitmap Ensure24bpp(Bitmap src)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));

            if (src.PixelFormat == PixelFormat.Format24bppRgb)
                return (Bitmap)src.Clone();

            Bitmap dst = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(dst))
                g.DrawImage(src, 0, 0, src.Width, src.Height);

            return dst;
        }

        public static void GetBytes24(Bitmap bmp, out byte[] buf, out int stride)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            stride = bd.Stride;
            int bytes = stride * bmp.Height;

            buf = new byte[bytes];
            Marshal.Copy(bd.Scan0, buf, 0, bytes);

            bmp.UnlockBits(bd);
        }

        public static Rectangle ClampRect(Rectangle r, int w, int h)
        {
            int x = System.Math.Max(0, System.Math.Min(r.X, w - 1));
            int y = System.Math.Max(0, System.Math.Min(r.Y, h - 1));
            int rw = System.Math.Max(1, System.Math.Min(r.Width, w - x));
            int rh = System.Math.Max(1, System.Math.Min(r.Height, h - y));
            return new Rectangle(x, y, rw, rh);
        }

        public static Rectangle ExpandAndClamp(Rectangle r, int pad, int w, int h)
        {
            Rectangle expanded = Rectangle.FromLTRB(
                r.Left - pad,
                r.Top - pad,
                r.Right + pad,
                r.Bottom + pad
            );

            return ClampRect(expanded, w, h);
        }

        public static Rectangle MakeSquareAndClamp(Rectangle r, int w, int h)
        {
            int side = System.Math.Max(r.Width, r.Height);
            int cx = r.X + r.Width / 2;
            int cy = r.Y + r.Height / 2;

            int x = ImageUtils.ClampCenter(cx - side / 2, side, w);
            int y = ImageUtils.ClampCenter(cy - side / 2, side, h);
            return new Rectangle(x, y, side, side);
        }

        private static int ClampCenter(int start, int side, int max)
        {
            if (start < 0) start = 0;
            if (start + side > max) start = System.Math.Max(0, max - side);
            return System.Math.Min(start, System.Math.Max(0, max - side));
        }

        public static Rectangle FindForegroundBBox(Bitmap bin24)
        {
            if (bin24 == null) throw new ArgumentNullException(nameof(bin24));

            GetBytes24(bin24, out byte[] buf, out int stride);
            return FindBBoxCore(buf, bin24.Width, bin24.Height, stride, 3);
        }

        public static Rectangle FindBoundingBoxOfForeground(UnmanagedImage img)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));

            int pixelSize = System.Drawing.Image.GetPixelFormatSize(img.PixelFormat) / 8;
            if (pixelSize <= 0) pixelSize = 1;

            int stride = img.Stride;
            int bufferLen = System.Math.Abs(stride) * img.Height;

            byte[] buffer = new byte[bufferLen];
            Marshal.Copy(img.ImageData, buffer, 0, bufferLen);

            return FindBBoxCore(buffer, img.Width, img.Height, stride, pixelSize);
        }

        private static Rectangle FindBBoxCore(byte[] buffer, int w, int h, int stride, int pixelSize)
        {
            long blackCount = 0, whiteCount = 0;

            for (int y = 0; y < h; y++)
            {
                int rowOffset = y * stride;
                for (int x = 0; x < w; x++)
                {
                    int idx = rowOffset + x * pixelSize;
                    byte v = buffer[idx];
                    if (v < 128) blackCount++;
                    else whiteCount++;
                }
            }

            bool foregroundIsBlack = blackCount <= whiteCount;

            int minX = w, minY = h, maxX = -1, maxY = -1;

            for (int y = 0; y < h; y++)
            {
                int rowOffset = y * stride;
                for (int x = 0; x < w; x++)
                {
                    int idx = rowOffset + x * pixelSize;
                    byte v = buffer[idx];

                    bool isFg = foregroundIsBlack ? (v < 128) : (v >= 128);
                    if (!isFg) continue;

                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                    if (x > maxX) maxX = x;
                    if (y > maxY) maxY = y;
                }
            }

            if (maxX < 0) return Rectangle.Empty;
            return Rectangle.FromLTRB(minX, minY, maxX + 1, maxY + 1);
        }

        public static Bitmap Crop24(Bitmap src24, Rectangle r)
        {
            if (src24 == null) throw new ArgumentNullException(nameof(src24));

            Bitmap dst = new Bitmap(r.Width, r.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.DrawImage(src24, new Rectangle(0, 0, r.Width, r.Height), r, GraphicsUnit.Pixel);
            }
            return dst;
        }

        public static Bitmap ResizeNearest24(Bitmap src24, int w, int h)
        {
            if (src24 == null) throw new ArgumentNullException(nameof(src24));

            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.SmoothingMode = SmoothingMode.None;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(src24, 0, 0, w, h);
            }
            return dst;
        }

        public static void BinarizeInPlace24(Bitmap bmp24)
        {
            if (bmp24 == null) throw new ArgumentNullException(nameof(bmp24));

            Rectangle rect = new Rectangle(0, 0, bmp24.Width, bmp24.Height);
            BitmapData bd = bmp24.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bd.Stride;
            int bytes = stride * bmp24.Height;
            byte[] buf = new byte[bytes];
            Marshal.Copy(bd.Scan0, buf, 0, bytes);

            for (int y = 0; y < bmp24.Height; y++)
            {
                int row = y * stride;
                for (int x = 0; x < bmp24.Width; x++)
                {
                    int i = row + x * 3;
                    byte v = buf[i];
                    byte b = (byte)(v < 128 ? 0 : 255);
                    buf[i] = b; buf[i + 1] = b; buf[i + 2] = b;
                }
            }

            Marshal.Copy(buf, 0, bd.Scan0, bytes);
            bmp24.UnlockBits(bd);
        }
    }
}
