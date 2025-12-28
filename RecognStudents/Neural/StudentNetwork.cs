using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    public class StudentNetwork : BaseNetwork
    {
        private int[] structure;
        private double[][,] weights;
        private double[][] activations;
        private double learningRate = 0.1;
        public Stopwatch stopWatch = new Stopwatch();

        public StudentNetwork(int[] structure)
        {
            this.structure = structure;
            InitializeNetwork();
        }

        private void InitializeNetwork()
        {
            Random rand = new Random();

            activations = new double[structure.Length][];
            for (int i = 0; i < structure.Length; i++)
            {
                activations[i] = new double[structure[i]];
            }

            weights = new double[structure.Length - 1][,];
            for (int i = 0; i < structure.Length - 1; i++)
            {
                weights[i] = new double[structure[i + 1], structure[i] + 1];
                for (int j = 0; j < structure[i + 1]; j++)
                {
                    for (int k = 0; k < structure[i] + 1; k++)
                    {
                        weights[i][j, k] = (rand.NextDouble() * 2) - 1; // [-1,1]
                    }
                }
            }
        }

        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        private double SigmoidDer(double x)
        {
            return x * (1.0 - x);
        }

        protected override double[] Compute(double[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                activations[0][i] = input[i];
            }

            for (int layer = 1; layer < structure.Length; layer++)
            {
                int prevSize = structure[layer - 1];
                int curSize = structure[layer];

                Parallel.For(0, curSize, neuron =>
                {
                    double sum = 0;
                    for (int prevNeuron = 0; prevNeuron < prevSize; prevNeuron++)
                    {
                        sum += activations[layer - 1][prevNeuron] * weights[layer - 1][neuron, prevNeuron];
                    }

                    sum += weights[layer - 1][neuron, prevSize]; // bias

                    activations[layer][neuron] = Sigmoid(sum);
                });
            }

            return activations[structure.Length - 1].ToArray();
        }

        private double TrainSample(double[] input, double[] target)
        {
            Compute(input);

            int lastLayer = structure.Length - 1;
            double totalError = 0;

            double[] outputDeltas = new double[structure[lastLayer]];
            for (int i = 0; i < structure[lastLayer]; i++)
            {
                double error = target[i] - activations[lastLayer][i];
                totalError += error * error;
                outputDeltas[i] = error * SigmoidDer(activations[lastLayer][i]);
            }

            double[][] deltas = new double[structure.Length - 1][];
            deltas[lastLayer - 1] = outputDeltas;

            for (int layer = lastLayer - 1; layer > 0; layer--)
            {
                int curSize = structure[layer];
                int nextSize = structure[layer + 1];

                deltas[layer - 1] = new double[curSize];

                Parallel.For(0, curSize, neuron =>
                {
                    double sum = 0;

                    for (int nextNeuron = 0; nextNeuron < nextSize; nextNeuron++)
                    {
                        sum += deltas[layer][nextNeuron] * weights[layer][nextNeuron, neuron];
                    }
                    deltas[layer - 1][neuron] = sum * SigmoidDer(activations[layer][neuron]);
                });
            }

            for (int layer = 0; layer < structure.Length - 1; layer++)
            {
                int prevSize = structure[layer];
                int curSize = structure[layer + 1];

                Parallel.For(0, curSize, neuron =>
                {
                    for (int prevNeuron = 0; prevNeuron < prevSize; prevNeuron++)
                    {
                        weights[layer][neuron, prevNeuron] +=
                            learningRate * deltas[layer][neuron] * activations[layer][prevNeuron];
                    }

                    weights[layer][neuron, prevSize] += learningRate * deltas[layer][neuron];
                });
            }

            return totalError;
        }

        public override int Train(Sample sample, double acceptableError, bool parallel)
        {
            int iterations = 0;
            double error;

            do
            {
                iterations++;
                error = TrainSample(sample.input, sample.Output);
            }
            while (error > acceptableError && iterations < 10000);
            return iterations;
        }

        public override double TrainOnDataSet(SamplesSet samplesSet, int epochsCount, double acceptableError, bool parallel)
        {
            stopWatch.Restart();
            double totalError = double.PositiveInfinity;

            for (int epoch = 0; epoch < epochsCount && totalError > acceptableError; epoch++)
            {
                totalError = 0;

                var shuffledSamples = samplesSet.samples.OrderBy(x => Guid.NewGuid()).ToList();

                foreach (var sample in shuffledSamples)
                {
                    totalError += TrainSample(sample.input, sample.Output);
                }

                totalError /= samplesSet.Count;

                OnTrainProgress((double)epoch / epochsCount, totalError, stopWatch.Elapsed);
            }

            OnTrainProgress(1.0, totalError, stopWatch.Elapsed);
            stopWatch.Stop();

            return totalError;
        }
    }
}