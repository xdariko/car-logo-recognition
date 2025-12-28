using System;
using System.Drawing;
using System.Threading.Tasks;
using NeuralNetwork1;

namespace AForge.WindowsForms
{
    class Controller
    {
        private FormUpdateDelegate formUpdateDelegate = null;

        public MagicEye processor = new MagicEye();

        public Settings settings
        {
            get { return processor.settings; }
            set { processor.settings = value; }
        }

        private bool _imageProcessed = true;
        public bool Ready { get { return _imageProcessed; } }

        public BaseNetwork Net { get; private set; }

        public event Action<double, double, TimeSpan> TrainProgress;

        public Controller(FormUpdateDelegate updater)
        {
            formUpdateDelegate = updater;
        }

        public async Task<bool> ProcessImage(Bitmap image)
        {
            if (!Ready) return false;
            _imageProcessed = false;

            bool processResult = await Task.Run(() => processor.ProcessImage(image));

            if (formUpdateDelegate != null)
                formUpdateDelegate.Invoke();

            _imageProcessed = true;
            return processResult;
        }

        public Bitmap GetOriginalImage() { return processor.original; }
        public Bitmap GetProcessedImage() { return processor.processed; }

        public void RecreateNetwork(string netType, int[] structure)
        {
            if (Net != null)
                Net.TrainProgress -= OnNetTrainProgress;

            Net = CreateNetworkByName(netType, structure);

            if (Net != null)
                Net.TrainProgress += OnNetTrainProgress;
        }

        private void OnNetTrainProgress(double progress, double error, TimeSpan elapsed)
        {
            var h = TrainProgress;
            if (h != null) h(progress, error, elapsed);
        }

        private static BaseNetwork CreateNetworkByName(string netType, int[] structure)
        {
            string s = (netType ?? "").Trim().ToLowerInvariant();

            if (s == "student")
                return new StudentNetwork(structure);

            if (s == "accord")
                return new AccordNet(structure);

            return new StudentNetwork(structure);
        }

        public async Task<Tuple<double, double>> TrainFromFolderAsync(
            string folder,
            FeatureConfig fcfg,
            string netType,
            int[] hiddenLayers,
            int epochs,
            double acceptableError,
            bool parallel,
            double trainPart)
        {
            SamplesSet all = DatasetLoader.LoadFromFolder(folder, fcfg, 10);
            Tuple<SamplesSet, SamplesSet> split = DatasetLoader.Split(all, trainPart, 1);

            SamplesSet train = split.Item1;
            SamplesSet test = split.Item2;

            int inputLen = FeatureExtractors.GetInputLength(fcfg);
            int[] structure = BuildStructure(inputLen, 10, hiddenLayers);

            RecreateNetwork(netType, structure);

            double trainErr = await Task.Run(() => Net.TrainOnDataSet(train, epochs, acceptableError, parallel));
            double acc = test.TestNeuralNetwork(Net);

            return Tuple.Create(trainErr, acc);
        }

        public double TestOnFolder(string folder, FeatureConfig fcfg)
        {
            if (Net == null) return 0.0;
            SamplesSet all = DatasetLoader.LoadFromFolder(folder, fcfg, 10);
            return all.TestNeuralNetwork(Net);
        }

        public BrandType PredictCurrent(FeatureConfig fcfg)
        {
            if (Net == null) return BrandType.Undef;
            if (processor.processed == null) return BrandType.Undef;

            double[] x = FeatureExtractors.Extract(processor.processed, fcfg);

            Sample sample = new Sample(x, 10, BrandType.Undef);

            BrandType pred = Net.Predict(sample);
            return pred;
        }

        public static int[] BuildStructure(int inputLen, int classesCount, int[] hidden)
        {
            if (hidden == null) hidden = new int[0];

            var list = new System.Collections.Generic.List<int>(2 + hidden.Length);
            list.Add(inputLen);

            for (int i = 0; i < hidden.Length; i++)
                if (hidden[i] > 0) list.Add(hidden[i]);

            list.Add(classesCount);
            return list.ToArray();
        }
    }
}
