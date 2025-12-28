using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AForge.WindowsForms
{
    internal class Settings
    {
        public readonly Size sampleSize = new Size(32, 32);
        public readonly int samplePadding = 4;

        private int _border = 20;
        public int border
        {
            get { return _border; }
            set
            {
                if ((value > 0) && (value < height / 3))
                {
                    _border = value;
                    if (top > 2 * _border) top = 2 * _border;
                    if (left > 2 * _border) left = 2 * _border;
                }
            }
        }

        public int width = 640;
        public int height = 640;

        /// <summary>
        /// Размер сетки для сенсоров по горизонтали
        /// </summary>
        public int blocksCount = 10;

        /// <summary>
        /// Желаемый размер изображения до обработки
        /// </summary>
        public Size orignalDesiredSize = new Size(500, 500);

        /// <summary>
        /// Желаемый размер изображения после обработки
        /// </summary>
        public Size processedDesiredSize = new Size(500, 500);

        public int margin = 10;
        public int top = 40;
        public int left = 40;

        /// <summary>
        /// Второй этап обработки
        /// </summary>
        public bool processImg = false;

        /// <summary>
        /// Порог при отсечении по цвету 
        /// </summary>
        public byte threshold = 120;
        public float differenceLim = 0.15f;

        public void incTop() { if (top < 2 * _border) ++top; }
        public void decTop() { if (top > 0) --top; }
        public void incLeft() { if (left < 2 * _border) ++left; }
        public void decLeft() { if (left > 0) --left; }
    }

    internal class MagicEye
    {
        /// <summary>
        /// Обработанное изображение
        /// </summary>
        public Bitmap processed;

        /// <summary>
        /// Оригинальное изображение после обработки
        /// </summary>
        public Bitmap original;

        /// <summary>
        /// Класс настроек
        /// </summary>
        public Settings settings = new Settings();

        public MagicEye() { }

        public bool ProcessImage(Bitmap bitmap)
        {
            // На вход поступает необработанное изображение с веб-камеры

            if (bitmap == null) return false;

            // Минимальная сторона изображения (обычно это высота)
            int side = System.Math.Min(bitmap.Width, bitmap.Height);
            if (side < 4 * settings.border) settings.border = side / 4;
            side -= 2 * settings.border;

            var cropRect = new Rectangle(
                (bitmap.Width - side) / 2 + settings.left + settings.border,
                (bitmap.Height - side) / 2 + settings.top + settings.border,
                side,
                side
            );

            cropRect = ClampRect(cropRect, bitmap.Width, bitmap.Height);

            // Обрезаем и готовим original
            original = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g0 = Graphics.FromImage(original))
            {
                g0.DrawImage(bitmap, new Rectangle(0, 0, original.Width, original.Height), cropRect, GraphicsUnit.Pixel);
            }

            // Переводим в серый
            var grayFilter = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
            var uProcessed = grayFilter.Apply(AForge.Imaging.UnmanagedImage.FromManagedImage(original));

            // Масштабируем до 500x500
            var scaleFilter = new AForge.Imaging.Filters.ResizeBilinear(
                settings.orignalDesiredSize.Width,
                settings.orignalDesiredSize.Height
            );
            uProcessed = scaleFilter.Apply(uProcessed);
            original = scaleFilter.Apply(original);

            // Пороговая бинаризация
            var threshldFilter = new AForge.Imaging.Filters.BradleyLocalThresholding();
            threshldFilter.PixelBrightnessDifferenceLimit = settings.differenceLim;
            threshldFilter.ApplyInPlace(uProcessed);

            // Отрисовка сетки и bbox
            using (Graphics g = Graphics.FromImage(original))
            {
                DrawSensorGrid(g, original.Width, original.Height);

                if (settings.processImg)
                {
                    Rectangle bbox;
                    string info = processSample(ref uProcessed, out bbox);

                    using (Pen bbPen = new Pen(Color.Lime, 2))
                    {
                        if (!bbox.IsEmpty)
                            g.DrawRectangle(bbPen, bbox);
                    }

                    using (Font f = new Font(FontFamily.GenericSansSerif, 14))
                    {
                        g.DrawString(info, f, Brushes.Black, 10, 10);
                    }
                }
            }

            // Возвращаем threshold изображение
            processed = uProcessed.ToManagedImage();

            return true;
        }

        private void DrawSensorGrid(Graphics g, int w, int h)
        {
            int bw = w / settings.blocksCount;
            int bh = h / settings.blocksCount;

            using (Pen p = new Pen(Color.Red, 1))
            {
                for (int r = 0; r < settings.blocksCount; r++)
                    for (int c = 0; c < settings.blocksCount; c++)
                        g.DrawRectangle(p, new Rectangle(c * bw, r * bh, bw, bh));
            }
        }

        private static Rectangle ClampRect(Rectangle r, int w, int h)
        {
            int x = System.Math.Max(0, System.Math.Min(r.X, w - 1));
            int y = System.Math.Max(0, System.Math.Min(r.Y, h - 1));
            int rw = System.Math.Max(1, System.Math.Min(r.Width, w - x));
            int rh = System.Math.Max(1, System.Math.Min(r.Height, h - y));
            return new Rectangle(x, y, rw, rh);
        }

        private string processSample(ref AForge.Imaging.UnmanagedImage img, out Rectangle bboxOnCurrentImage)
        {
            bboxOnCurrentImage = FindBoundingBoxOfForeground(img);
            if (bboxOnCurrentImage.IsEmpty)
                return "BBox: пусто (нет объекта)";

            int pad = settings.samplePadding;
            var expanded = ExpandAndClamp(bboxOnCurrentImage, pad, img.Width, img.Height);
            var square = MakeSquareAndClamp(expanded, img.Width, img.Height);

            var crop = new AForge.Imaging.Filters.Crop(square);
            img = crop.Apply(img);

            var resize = new AForge.Imaging.Filters.ResizeNearestNeighbor(
                settings.sampleSize.Width,
                settings.sampleSize.Height
            );
            img = resize.Apply(img);

            return $"BBox: {bboxOnCurrentImage.Width}x{bboxOnCurrentImage.Height}, square {square.Width}x{square.Height}";
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

        private static Rectangle FindBoundingBoxOfForeground(AForge.Imaging.UnmanagedImage img)
        {
            int w = img.Width, h = img.Height;
            int pixelSize = Image.GetPixelFormatSize(img.PixelFormat) / 8;
            if (pixelSize <= 0) pixelSize = 1;
            int stride = img.Stride;
            int bufferLen = System.Math.Abs(stride) * h;
            byte[] buffer = new byte[bufferLen];
            Marshal.Copy(img.ImageData, buffer, 0, bufferLen);

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
                    bool isForeground = foregroundIsBlack ? (v < 128) : (v >= 128);
                    if (!isForeground) continue;
                    if (x < minX) minX = x;
                    if (y < minY) minY = y;
                    if (x > maxX) maxX = x;
                    if (y > maxY) maxY = y;
                }
            }

            if (maxX < 0) return Rectangle.Empty;
            return Rectangle.FromLTRB(minX, minY, maxX + 1, maxY + 1);
        }
    }
}
