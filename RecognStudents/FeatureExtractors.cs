using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AForge.WindowsForms
{
    public enum FeatureMode
    {
        Zoning,
        Transitions,
        RowColHist
    }

    public sealed class FeatureConfig
    {
        public FeatureMode Mode = FeatureMode.Zoning;
        public int InputSize = 32;
        public int ZoningGrid = 10;
        public int Padding = 4;
    }

    public static class FeatureExtractors
    {
        public static int GetInputLength(FeatureConfig cfg)
        {
            if (cfg == null) throw new ArgumentNullException("cfg");

            int s = cfg.InputSize;

            switch (cfg.Mode)
            {
                case FeatureMode.Zoning:
                    return cfg.ZoningGrid * cfg.ZoningGrid;
                case FeatureMode.Transitions:
                    return 2 * s;
                case FeatureMode.RowColHist:
                    return 2 * s;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static double[] Extract(Bitmap thresholdBitmap, FeatureConfig cfg)
        {
            if (thresholdBitmap == null) throw new ArgumentNullException("thresholdBitmap");
            if (cfg == null) throw new ArgumentNullException("cfg");

            Bitmap src24 = Ensure24bpp(thresholdBitmap);
            Bitmap normalized = null;
            try
            {
                // Если картинка уже приведена к cfg.InputSize x cfg.InputSize (например, нормализация
                // была выполнена ранее), то повторно bbox/crop/resize не делаем.
                if ((src24.Width == cfg.InputSize) && (src24.Height == cfg.InputSize))
                {
                    normalized = src24;
                }
                else
                {
                    Rectangle bbox = FindForegroundBBox(src24);
                    if (bbox.IsEmpty)
                        return new double[GetInputLength(cfg)];

                    bbox = ExpandAndClamp(bbox, cfg.Padding, src24.Width, src24.Height);
                    Rectangle square = MakeSquareAndClamp(bbox, src24.Width, src24.Height);

                    Bitmap cropped = Crop(src24, square);
                    try
                    {
                        normalized = ResizeNearest(cropped, cfg.InputSize, cfg.InputSize);
                    }
                    finally { cropped.Dispose(); }
                }

                BinarizeInPlace(normalized);

                switch (cfg.Mode)
                {
                    case FeatureMode.Zoning:
                        return Zoning(normalized, cfg.ZoningGrid);
                    case FeatureMode.Transitions:
                        return Transitions(normalized);
                    case FeatureMode.RowColHist:
                        return RowColHist(normalized);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            finally
            {
                // normalized может быть тем же объектом, что и src24.
                if ((normalized != null) && !object.ReferenceEquals(normalized, src24))
                    normalized.Dispose();
                src24.Dispose();
            }
        }

        private static double[] Zoning(Bitmap bin, int grid)
        {
            int w = bin.Width;
            int h = bin.Height;
            if (grid < 1) grid = 1;

            double[] result = new double[grid * grid];

            byte[] buf;
            int stride;
            GetBytes24(bin, out buf, out stride);

            int cellW = w / grid; if (cellW <= 0) cellW = 1;
            int cellH = h / grid; if (cellH <= 0) cellH = 1;

            for (int gy = 0; gy < grid; gy++)
            {
                for (int gx = 0; gx < grid; gx++)
                {
                    int x0 = gx * cellW;
                    int y0 = gy * cellH;
                    int x1 = (gx == grid - 1) ? w : System.Math.Min(w, x0 + cellW);
                    int y1 = (gy == grid - 1) ? h : System.Math.Min(h, y0 + cellH);

                    int black = 0;
                    int total = 0;

                    for (int y = y0; y < y1; y++)
                    {
                        int row = y * stride;
                        for (int x = x0; x < x1; x++)
                        {
                            byte v = buf[row + x * 3];
                            if (v < 128) black++;
                            total++;
                        }
                    }

                    result[gy * grid + gx] = (total == 0) ? 0.0 : (double)black / total;
                }
            }

            return result;
        }

        private static double[] Transitions(Bitmap bin)
        {
            int s = bin.Width;
            double[] result = new double[2 * s];

            byte[] buf;
            int stride;
            GetBytes24(bin, out buf, out stride);

            for (int y = 0; y < s; y++)
            {
                int row = y * stride;
                int trans = 0;

                bool prevBlack = buf[row + 0] < 128;
                for (int x = 1; x < s; x++)
                {
                    bool curBlack = buf[row + x * 3] < 128;
                    if (curBlack != prevBlack) trans++;
                    prevBlack = curBlack;
                }

                result[y] = (s <= 1) ? 0.0 : (double)trans / (s - 1);
            }

            for (int x = 0; x < s; x++)
            {
                int trans = 0;
                bool prevBlack = buf[0 * stride + x * 3] < 128;

                for (int y = 1; y < s; y++)
                {
                    bool curBlack = buf[y * stride + x * 3] < 128;
                    if (curBlack != prevBlack) trans++;
                    prevBlack = curBlack;
                }

                result[s + x] = (s <= 1) ? 0.0 : (double)trans / (s - 1);
            }

            return result;
        }

        private static double[] RowColHist(Bitmap bin)
        {
            int s = bin.Width;
            double[] result = new double[2 * s];

            byte[] buf;
            int stride;
            GetBytes24(bin, out buf, out stride);

            for (int y = 0; y < s; y++)
            {
                int row = y * stride;
                int black = 0;

                for (int x = 0; x < s; x++)
                    if (buf[row + x * 3] < 128) black++;

                result[y] = (double)black / s;
            }

            for (int x = 0; x < s; x++)
            {
                int black = 0;

                for (int y = 0; y < s; y++)
                    if (buf[y * stride + x * 3] < 128) black++;

                result[s + x] = (double)black / s;
            }

            return result;
        }

        private static Bitmap Ensure24bpp(Bitmap src)
        {
            if (src.PixelFormat == PixelFormat.Format24bppRgb)
                return (Bitmap)src.Clone();

            Bitmap dst = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(dst))
                g.DrawImage(src, 0, 0, src.Width, src.Height);
            return dst;
        }

        private static Bitmap Crop(Bitmap src, Rectangle r)
        {
            Bitmap dst = new Bitmap(r.Width, r.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(dst))
                g.DrawImage(src, new Rectangle(0, 0, r.Width, r.Height), r, GraphicsUnit.Pixel);
            return dst;
        }

        private static Bitmap ResizeNearest(Bitmap src, int w, int h)
        {
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.SmoothingMode = SmoothingMode.None;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(src, 0, 0, w, h);
            }
            return dst;
        }

        private static void BinarizeInPlace(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bd.Stride;
            int bytes = stride * bmp.Height;
            byte[] buf = new byte[bytes];
            Marshal.Copy(bd.Scan0, buf, 0, bytes);

            for (int y = 0; y < bmp.Height; y++)
            {
                int row = y * stride;
                for (int x = 0; x < bmp.Width; x++)
                {
                    int i = row + x * 3;
                    byte v = buf[i];
                    byte b = (byte)(v < 128 ? 0 : 255);
                    buf[i] = b; buf[i + 1] = b; buf[i + 2] = b;
                }
            }

            Marshal.Copy(buf, 0, bd.Scan0, bytes);
            bmp.UnlockBits(bd);
        }

        private static void GetBytes24(Bitmap bmp, out byte[] buf, out int stride)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            stride = bd.Stride;
            int bytes = stride * bmp.Height;
            buf = new byte[bytes];
            Marshal.Copy(bd.Scan0, buf, 0, bytes);

            bmp.UnlockBits(bd);
        }

        private static Rectangle FindForegroundBBox(Bitmap bin24)
        {
            byte[] buf;
            int stride;
            GetBytes24(bin24, out buf, out stride);

            int w = bin24.Width;
            int h = bin24.Height;

            long black = 0, white = 0;

            for (int y = 0; y < h; y++)
            {
                int row = y * stride;
                for (int x = 0; x < w; x++)
                {
                    byte v = buf[row + x * 3];
                    if (v < 128) black++; else white++;
                }
            }

            bool fgIsBlack = black <= white;

            int minX = w, minY = h, maxX = -1, maxY = -1;

            for (int y = 0; y < h; y++)
            {
                int row = y * stride;
                for (int x = 0; x < w; x++)
                {
                    byte v = buf[row + x * 3];
                    bool isFg = fgIsBlack ? (v < 128) : (v >= 128);
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

        private static Rectangle ExpandAndClamp(Rectangle r, int pad, int w, int h)
        {
            int x1 = System.Math.Max(0, r.X - pad);
            int y1 = System.Math.Max(0, r.Y - pad);
            int x2 = System.Math.Min(w, r.Right + pad);
            int y2 = System.Math.Min(h, r.Bottom + pad);
            return Rectangle.FromLTRB(x1, y1, x2, y2);
        }

        private static Rectangle MakeSquareAndClamp(Rectangle r, int w, int h)
        {
            int side = System.Math.Max(r.Width, r.Height);

            int cx = r.X + r.Width / 2;
            int cy = r.Y + r.Height / 2;

            int x = cx - side / 2;
            int y = cy - side / 2;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x + side > w) x = System.Math.Max(0, w - side);
            if (y + side > h) y = System.Math.Max(0, h - side);

            side = System.Math.Min(side, System.Math.Min(w, h));
            x = System.Math.Min(x, w - side);
            y = System.Math.Min(y, h - side);

            return new Rectangle(x, y, side, side);
        }
    }
}
