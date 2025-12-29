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
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));
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
            if (thresholdBitmap == null) throw new ArgumentNullException(nameof(thresholdBitmap));
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));

            using (Bitmap src24 = ImageUtils.Ensure24bpp(thresholdBitmap))
            {
                Bitmap normalized = null;
                try
                {
                    if (src24.Width == cfg.InputSize && src24.Height == cfg.InputSize)
                    {
                        normalized = (Bitmap)src24.Clone();
                    }
                    else
                    {
                        Rectangle bbox = ImageUtils.FindForegroundBBox(src24);
                        if (bbox.IsEmpty)
                            return new double[GetInputLength(cfg)];

                        bbox = ImageUtils.ExpandAndClamp(bbox, cfg.Padding, src24.Width, src24.Height);
                        Rectangle square = ImageUtils.MakeSquareAndClamp(bbox, src24.Width, src24.Height);

                        using (Bitmap cropped = ImageUtils.Crop24(src24, square))
                        {
                            normalized = ImageUtils.ResizeNearest24(cropped, cfg.InputSize, cfg.InputSize);
                        }
                    }

                    ImageUtils.BinarizeInPlace24(normalized);

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
                    normalized?.Dispose();
                }
            }
        }

        private static double[] Zoning(Bitmap bin, int grid)
        {
            int w = bin.Width, h = bin.Height;
            ImageUtils.GetBytes24(bin, out byte[] buf, out int stride);

            double[] result = new double[grid * grid];
            int cellW = System.Math.Max(1, w / grid);
            int cellH = System.Math.Max(1, h / grid);

            for (int gy = 0; gy < grid; gy++)
                for (int gx = 0; gx < grid; gx++)
                {
                    int x0 = gx * cellW, y0 = gy * cellH;
                    int x1 = System.Math.Min(w, x0 + cellW);
                    int y1 = System.Math.Min(h, y0 + cellH);
                    int black = 0, total = 0;

                    for (int y = y0; y < y1; y++)
                    {
                        int row = y * stride;
                        for (int x = x0; x < x1; x++)
                        {
                            if (buf[row + x * 3] < 128) black++;
                            total++;
                        }
                    }
                    result[gy * grid + gx] = total == 0 ? 0 : (double)black / total;
                }
            return result;
        }

        private static double[] Transitions(Bitmap bin)
        {
            int s = bin.Width;
            double[] result = new double[2 * s];
            ImageUtils.GetBytes24(bin, out byte[] buf, out int stride);

            for (int y = 0; y < s; y++)
            {
                int row = y * stride;
                int trans = 0;
                bool prevBlack = buf[row] < 128;
                for (int x = 1; x < s; x++)
                {
                    bool curBlack = buf[row + x * 3] < 128;
                    if (curBlack != prevBlack) trans++;
                    prevBlack = curBlack;
                }
                result[y] = (double)trans / System.Math.Max(1, s - 1);
            }

            for (int x = 0; x < s; x++)
            {
                int trans = 0;
                bool prevBlack = buf[x * 3] < 128;
                for (int y = 1; y < s; y++)
                {
                    bool curBlack = buf[y * stride + x * 3] < 128;
                    if (curBlack != prevBlack) trans++;
                    prevBlack = curBlack;
                }
                result[s + x] = (double)trans / System.Math.Max(1, s - 1);
            }
            return result;
        }

        private static double[] RowColHist(Bitmap bin)
        {
            int s = bin.Width;
            double[] result = new double[2 * s];
            ImageUtils.GetBytes24(bin, out byte[] buf, out int stride);

            for (int y = 0; y < s; y++)
            {
                int row = y * stride, black = 0;
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
    }
}
