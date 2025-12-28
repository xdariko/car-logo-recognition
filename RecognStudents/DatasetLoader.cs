using System;
using System.Drawing;
using System.IO;
using System.Linq;
using NeuralNetwork1;

namespace AForge.WindowsForms
{
    public static class DatasetLoader
    {
        public static SamplesSet LoadFromFolder(string folder, FeatureConfig cfg, int classesCount)
        {
            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException("Не найдена папка датасета: " + folder);

            SamplesSet set = new SamplesSet();

            string[] files = Directory.GetFiles(folder)
                .Where(f =>{
                    string ext = Path.GetExtension(f).ToLowerInvariant();
                    return ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp";
                })
                .ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i];
                string name = Path.GetFileNameWithoutExtension(path);

                int cls = BrandMap.FromFileName(name); // 0..9

                using (Bitmap bmp = (Bitmap)Image.FromFile(path))
                {
                    double[] features = FeatureExtractors.Extract(bmp, cfg);
                    Sample sample = new Sample(features, classesCount, (BrandType)cls);
                    set.AddSample(sample);
                }
            }

            return set;
        }

        public static Tuple<SamplesSet, SamplesSet> Split(SamplesSet all, double trainPart, int seed)
        {
            Random rnd = new Random(seed);

            var shuffled = all.samples.OrderBy(x => rnd.Next()).ToList();

            int trainCount = System.Math.Max(1, (int)System.Math.Round(shuffled.Count * trainPart));
            trainCount = System.Math.Min(trainCount, shuffled.Count - 1);

            SamplesSet train = new SamplesSet();
            SamplesSet test = new SamplesSet();

            for (int i = 0; i < shuffled.Count; i++)
            {
                if (i < trainCount) train.AddSample(shuffled[i]);
                else test.AddSample(shuffled[i]);
            }

            return Tuple.Create(train, test);
        }

        public static string FindDatasetFolder(string folderName, int maxUp)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i <= maxUp; i++)
            {
                string candidate = Path.Combine(dir, folderName);
                if (Directory.Exists(candidate)) return candidate;

                DirectoryInfo parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }

            throw new DirectoryNotFoundException($"Папка датасета '{folderName}' не найдена. ");
        }
    }
}
