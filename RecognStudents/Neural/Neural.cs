using System;
using System.Collections;
using System.Collections.Generic;

namespace NeuralNetwork1
{
    /// <summary>
    /// Класс для хранения образа – входной массив сигналов на сенсорах,
    /// выходные сигналы сети, ошибка, и т.д.
    /// </summary>
    public class Sample
    {
        /// <summary>Входной вектор</summary>
        public double[] input = null;

        /// <summary>Вектор ошибки</summary>
        public double[] error = null;

        /// <summary>Действительный класс (задаётся учителем)</summary>
        public BrandType actualClass;

        /// <summary>Распознанный класс</summary>
        public BrandType recognizedClass;

        /// <summary>
        /// Конструктор образа.
        /// Если sampleClass != Undef, то целевой one-hot вектор выставится автоматически.
        /// </summary>
        public Sample(double[] inputValues, int classesCount, BrandType sampleClass = BrandType.Undef)
        {
            input = (double[])inputValues.Clone();
            Output = new double[classesCount];

            if (sampleClass != BrandType.Undef)
                Output[(int)sampleClass] = 1.0;

            recognizedClass = BrandType.Undef;
            actualClass = sampleClass;
        }

        /// <summary>Выходной вектор</summary>
        public double[] Output { get; private set; }

        /// <summary>
        /// Обработка реакции сети на данный образ на основе выходного вектора сети.
        /// Возвращает распознанный класс.
        /// </summary>
        public BrandType ProcessPrediction(double[] neuralOutput)
        {
            Output = neuralOutput;

            if (error == null || error.Length != Output.Length)
                error = new double[Output.Length];

            // определяем класс с максимальным выходом
            int bestIndex = 0;

            for (int i = 0; i < Output.Length; ++i)
            {
                // ошибка относительно идеального (1 для верного класса, 0 для остальных)
                error[i] = Output[i] - (i == (int)actualClass ? 1.0 : 0.0);

                if (Output[i] > Output[bestIndex])
                    bestIndex = i;
            }

            recognizedClass = (BrandType)bestIndex;
            return recognizedClass;
        }

        /// <summary>
        /// Суммарная квадратичная ошибка (по error[]).
        /// </summary>
        public double EstimatedError()
        {
            double result = 0;
            for (int i = 0; i < Output.Length; ++i)
                result += Math.Pow(error[i], 2);

            return result;
        }

        /// <summary>
        /// Добавляет к errorVector ошибку, соответствующую данному образу (не квадратичную).
        /// </summary>
        public void updateErrorVector(double[] errorVector)
        {
            for (int i = 0; i < errorVector.Length; ++i)
                errorVector[i] += error[i];
        }

        public override string ToString()
        {
            string result =
                "Sample decoding : " + actualClass + "(" + (int)actualClass + "); " + Environment.NewLine +
                "Input : ";

            for (int i = 0; i < input.Length; ++i)
                result += input[i] + "; ";

            result += Environment.NewLine + "Output : ";
            if (Output == null) result += "null;";
            else
                for (int i = 0; i < Output.Length; ++i)
                    result += Output[i] + "; ";

            result += Environment.NewLine + "Error : ";
            if (error == null) result += "null;";
            else
                for (int i = 0; i < error.Length; ++i)
                    result += error[i] + "; ";

            result += Environment.NewLine +
                      "Recognized : " + recognizedClass + "(" + (int)recognizedClass + "); " +
                      Environment.NewLine;

            return result;
        }

        /// <summary>Правильно ли распознан образ</summary>
        public bool Correct()
        {
            return actualClass == recognizedClass;
        }
    }

    /// <summary>
    /// Выборка образов (обучающая/тестовая/для распознавания).
    /// </summary>
    public class SamplesSet : IEnumerable
    {
        /// <summary>Накопленные образы</summary>
        public List<Sample> samples = new List<Sample>();

        public void AddSample(Sample image)
        {
            samples.Add(image);
        }

        public int Count => samples.Count;

        public IEnumerator GetEnumerator()
        {
            return samples.GetEnumerator();
        }

        public Sample this[int i]
        {
            get => samples[i];
            set => samples[i] = value;
        }

        /// <summary>
        /// Тест точности: сколько правильно распознано / всего.
        /// </summary>
        public double TestNeuralNetwork(BaseNetwork network)
        {
            double correct = 0;
            double wrong = 0;

            foreach (var sample in samples)
            {
                // Predict(sample) должен возвращать BrandType
                if (sample.actualClass == network.Predict(sample))
                    ++correct;
                else
                    ++wrong;
            }

            return (correct + wrong) > 0 ? correct / (correct + wrong) : 0.0;
        }
    }
}
