using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using NeuralNetwork1;

namespace AForge.WindowsForms
{
    delegate void FormUpdateDelegate();

    public partial class MainForm : Form
    {
        private Controller controller = null;

        private AutoResetEvent evnt = new AutoResetEvent(false);
        private FilterInfoCollection videoDevicesList;
        private IVideoSource videoSource;
        private Stopwatch sw = new Stopwatch();
        private System.Threading.Timer updateTmr;

        private FeatureConfig CurrentFeatureConfig()
        {
            FeatureConfig cfg = new FeatureConfig();

            string mode = (cmbFeatureMode.SelectedItem as string) ?? "Zoning";

            if (mode == "Transitions")
                cfg.Mode = FeatureMode.Transitions;
            else if (mode == "RowColHist")
                cfg.Mode = FeatureMode.RowColHist;
            else
                cfg.Mode = FeatureMode.Zoning;

            cfg.ZoningGrid = (int)numZoningGrid.Value;
            cfg.Padding = 4;

            return cfg;
        }

        private int[] ParseHiddenLayers()
        {
            string s = (txtHiddenLayers.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(s))
                return new int[0];

            return s.Split(';')
                .Select(p => p.Trim())
                .Where(p => p.Length > 0)
                .Select(int.Parse)
                .ToArray();
        }

        private void UpdateFormFields()
        {
            if (statusLabel.InvokeRequired)
            {
                this.Invoke(new FormUpdateDelegate(UpdateFormFields));
                return;
            }

            sw.Stop();
            ticksLabel.Text = "Тики : " + sw.Elapsed.ToString();

            originalImageBox.Image = controller.GetOriginalImage();
            processedImgBox.Image = controller.GetProcessedImage();

            if (chkAutoPredict.Checked)
            {
                BrandType pred = controller.PredictCurrent(CurrentFeatureConfig());
                lblPredict.Text = "Predict: " + pred.ToString();
            }
        }

        public MainForm()
        {
            InitializeComponent();

            // важно, чтобы KeyDown ловился
            this.KeyPreview = true;

            originalImageBox.SizeMode = PictureBoxSizeMode.Zoom;
            processedImgBox.SizeMode = PictureBoxSizeMode.Zoom;

            // камеры
            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoDevice in videoDevicesList)
                cmbVideoSource.Items.Add(videoDevice.Name);

            if (cmbVideoSource.Items.Count > 0)
                cmbVideoSource.SelectedIndex = 0;
            else
                MessageBox.Show("А нет у вас камеры!", "Ошибочка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            controller = new Controller(new FormUpdateDelegate(UpdateFormFields));
            controller.TrainProgress += Controller_TrainProgress;

            // init UI combo
            cmbNetType.Items.Clear();
            cmbNetType.Items.AddRange(new object[] { "Student", "Accord" });
            cmbNetType.SelectedIndex = 0;

            cmbFeatureMode.Items.Clear();
            cmbFeatureMode.Items.AddRange(new object[] { "Zoning", "Transitions", "RowColHist" });
            cmbFeatureMode.SelectedIndex = 0;

            cmbFeatureMode_SelectedIndexChanged(this, EventArgs.Empty);

            chkParallel.Checked = true;
            chkAutoPredict.Checked = true;
        }

        private void Controller_TrainProgress(double progress, double error, TimeSpan elapsed)
        {
            if (trainProgressBar.InvokeRequired)
            {
                trainProgressBar.Invoke(new Action(() => Controller_TrainProgress(progress, error, elapsed)));
                return;
            }

            int p = (int)System.Math.Round(progress * 100);
            if (p < 0) p = 0;
            if (p > 100) p = 100;

            trainProgressBar.Value = p;
            lblTrainStatus.Text = string.Format("Ошибка: {0:F6}", error);
            lblTrainTime.Text = string.Format("Время: {0:hh\\:mm\\:ss}", elapsed);
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            sw.Restart();
            if (controller.Ready)
            {
#pragma warning disable CS4014
                controller.ProcessImage((Bitmap)eventArgs.Frame.Clone());
#pragma warning restore CS4014
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (videoSource == null)
            {
                var vcd = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);
                vcd.VideoResolution = vcd.VideoCapabilities[resolutionsBox.SelectedIndex];

                videoSource = vcd;
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                videoSource.Start();

                StartButton.Text = "Стоп";
                controlPanel.Enabled = true;
                cmbVideoSource.Enabled = false;
            }
            else
            {
                videoSource.SignalToStop();
                if (videoSource != null && videoSource.IsRunning && originalImageBox.Image != null)
                    originalImageBox.Image.Dispose();

                videoSource = null;

                StartButton.Text = "Старт";
                controlPanel.Enabled = false;
                cmbVideoSource.Enabled = true;
            }
        }

        private void tresholdTrackBar_ValueChanged(object sender, EventArgs e)
        {
            controller.settings.threshold = (byte)tresholdTrackBar.Value;
            controller.settings.differenceLim = (float)tresholdTrackBar.Value / tresholdTrackBar.Maximum;
        }

        private void borderTrackBar_ValueChanged(object sender, EventArgs e)
        {
            controller.settings.border = borderTrackBar.Value;
        }

        private void marginTrackBar_ValueChanged(object sender, EventArgs e)
        {
            controller.settings.margin = marginTrackBar.Value;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (updateTmr != null) updateTmr.Dispose();
            if (videoSource != null && videoSource.IsRunning) videoSource.SignalToStop();
        }

        private void cmbVideoSource_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var vcd = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);
            resolutionsBox.Items.Clear();
            for (int i = 0; i < vcd.VideoCapabilities.Length; i++)
                resolutionsBox.Items.Add(vcd.VideoCapabilities[i].FrameSize.ToString());
            resolutionsBox.SelectedIndex = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            controller.settings.processImg = checkBox1.Checked;
        }

        // === ВАЖНО: Designer у тебя подписан на этот метод ===
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W: controller.settings.decTop(); break;
                case Keys.S: controller.settings.incTop(); break;
                case Keys.A: controller.settings.decLeft(); break;
                case Keys.D: controller.settings.incLeft(); break;
                case Keys.Q: controller.settings.border++; break;
                case Keys.E: controller.settings.border--; break;
            }
        }

        // ====== UI сети ======

        private void cmbFeatureMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mode = (cmbFeatureMode.SelectedItem as string) ?? "Zoning";
            numZoningGrid.Enabled = (mode == "Zoning");
        }

        private void btnRecreateNet_Click(object sender, EventArgs e)
        {
            FeatureConfig cfg = CurrentFeatureConfig();
            int inputLen = FeatureExtractors.GetInputLength(cfg);
            int[] hidden = ParseHiddenLayers();

            int[] structure = Controller.BuildStructure(inputLen, 10, hidden);

            string netType = (cmbNetType.SelectedItem as string) ?? "Student";
            controller.RecreateNetwork(netType, structure);

            lblTrainStatus.Text = string.Format("Сеть пересоздана. In={0}, Out=10", inputLen);
        }

        private async void btnTrain_Click(object sender, EventArgs e)
        {
            // FIX: теперь FindDatasetFolder требует maxUp
            string folder = DatasetLoader.FindDatasetFolder("output_images", 6);

            FeatureConfig cfg = CurrentFeatureConfig();
            string netType = (cmbNetType.SelectedItem as string) ?? "Student";
            int[] hidden = ParseHiddenLayers();

            int epochs = (int)numEpochs.Value;
            double acceptableError = (100 - trkAccuracy.Value) / 100.0;
            bool parallel = chkParallel.Checked;

            // FIX: TrainFromFolderAsync требует trainPart
            double trainPart = 0.8;

            try
            {
                controlPanel.Enabled = false;
                lblTrainStatus.Text = "Обучение...";

                Tuple<double, double> result = await controller.TrainFromFolderAsync(
                    folder, cfg, netType, hidden, epochs, acceptableError, parallel, trainPart);

                // FIX: это Tuple -> Item1/Item2
                double err = result.Item1;
                double acc = result.Item2;

                lblTrainStatus.Text = string.Format("Готово. Ошибка={0:F6}, TestAcc={1:F2}%",
                    err, acc * 100.0);
            }
            catch (Exception ex)
            {
                lblTrainStatus.Text = "Ошибка: " + ex.Message;
            }
            finally
            {
                controlPanel.Enabled = true;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string folder = DatasetLoader.FindDatasetFolder("output_images", 6);

            FeatureConfig cfg = CurrentFeatureConfig();

            double acc = controller.TestOnFolder(folder, cfg);
            lblTrainStatus.Text = string.Format("Accuracy on folder: {0:F2}%", acc * 100.0);
        }
    }
}
