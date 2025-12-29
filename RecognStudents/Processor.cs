using System;
using System.Drawing;
using AForge.Imaging;
using AForge.Imaging.Filters;

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
        public int blocksCount = 10;
        public Size orignalDesiredSize = new Size(500, 500);
        public Size processedDesiredSize = new Size(500, 500);
        public int margin = 10;
        public int top = 40;
        public int left = 40;
        public bool processImg = false;
        public byte threshold = 120;
        public float differenceLim = 0.15f;

        public void incTop() { if (top < 2 * _border) ++top; }
        public void decTop() { if (top > 0) --top; }
        public void incLeft() { if (left < 2 * _border) ++left; }
        public void decLeft() { if (left > 0) --left; }
    }

    internal class MagicEye
    {
        public Bitmap processed;
        public Bitmap original;
        public Settings settings = new Settings();

        public MagicEye() { }

        public bool ProcessImage(Bitmap bitmap)
        {
            if (bitmap == null) return false;

            int side = System.Math.Min(bitmap.Width, bitmap.Height);
            if (side < 4 * settings.border) settings.border = side / 4;
            side -= 2 * settings.border;

            var cropRect = new Rectangle(
                (bitmap.Width - side) / 2 + settings.left + settings.border,
                (bitmap.Height - side) / 2 + settings.top + settings.border,
                side,
                side
            );

            cropRect = ImageUtils.ClampRect(cropRect, bitmap.Width, bitmap.Height);

            // обрезаем оригинал
            original = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g0 = Graphics.FromImage(original))
                g0.DrawImage(bitmap, new Rectangle(0, 0, original.Width, original.Height), cropRect, GraphicsUnit.Pixel);

            // в серый
            var grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            var uProcessed = grayFilter.Apply(UnmanagedImage.FromManagedImage(original));

            // масштабируем
            var scaleFilter = new ResizeBilinear(settings.orignalDesiredSize.Width, settings.orignalDesiredSize.Height);
            uProcessed = scaleFilter.Apply(uProcessed);
            original = scaleFilter.Apply(original);

            // бинаризация
            var thresholdFilter = new BradleyLocalThresholding
            {
                PixelBrightnessDifferenceLimit = settings.differenceLim
            };
            thresholdFilter.ApplyInPlace(uProcessed);

            // сетка и bbox
            using (Graphics g = Graphics.FromImage(original))
            {
                DrawSensorGrid(g, original.Width, original.Height);

                if (settings.processImg)
                {
                    Rectangle bbox;
                    string info = ProcessSample(ref uProcessed, out bbox);

                    using (Pen bbPen = new Pen(Color.Lime, 2))
                        if (!bbox.IsEmpty)
                            g.DrawRectangle(bbPen, bbox);

                    using (Font f = new Font(FontFamily.GenericSansSerif, 14))
                        g.DrawString(info, f, Brushes.Black, 10, 10);
                }
            }

            processed = uProcessed.ToManagedImage();
            return true;
        }

        private void DrawSensorGrid(Graphics g, int w, int h)
        {
            int bw = w / settings.blocksCount;
            int bh = h / settings.blocksCount;

            using (Pen p = new Pen(Color.Red, 1))
                for (int r = 0; r < settings.blocksCount; r++)
                    for (int c = 0; c < settings.blocksCount; c++)
                        g.DrawRectangle(p, new Rectangle(c * bw, r * bh, bw, bh));
        }

        private string ProcessSample(ref UnmanagedImage img, out Rectangle bboxOnCurrentImage)
        {
            bboxOnCurrentImage = ImageUtils.FindForegroundBBox(img.ToManagedImage());
            if (bboxOnCurrentImage.IsEmpty)
                return "BBox: пусто (нет объекта)";

            int pad = settings.samplePadding;
            var expanded = ImageUtils.ExpandAndClamp(bboxOnCurrentImage, pad, img.Width, img.Height);
            var square = ImageUtils.MakeSquareAndClamp(expanded, img.Width, img.Height);

            var crop = new Crop(square);
            img = crop.Apply(img);

            var resize = new ResizeNearestNeighbor(settings.sampleSize.Width, settings.sampleSize.Height);
            img = resize.Apply(img);

            return $"BBox: {bboxOnCurrentImage.Width}x{bboxOnCurrentImage.Height}, square {square.Width}x{square.Height}";
        }
    }
}
