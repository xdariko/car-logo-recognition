namespace AForge.WindowsForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbVideoSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.originalImageBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.processedImgBox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkAutoPredict = new System.Windows.Forms.CheckBox();
            this.lblPredict = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tresholdTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.marginTrackBar = new System.Windows.Forms.TrackBar();
            this.borderTrackBar = new System.Windows.Forms.TrackBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.ticksLabel = new System.Windows.Forms.Label();
            this.resolutionsBox = new System.Windows.Forms.ComboBox();
            this.controlPanel = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTrainTime = new System.Windows.Forms.Label();
            this.lblTrainStatus = new System.Windows.Forms.Label();
            this.trainProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnRecreateNet = new System.Windows.Forms.Button();
            this.chkParallel = new System.Windows.Forms.CheckBox();
            this.trkAccuracy = new System.Windows.Forms.TrackBar();
            this.numEpochs = new System.Windows.Forms.NumericUpDown();
            this.txtHiddenLayers = new System.Windows.Forms.TextBox();
            this.numZoningGrid = new System.Windows.Forms.NumericUpDown();
            this.cmbFeatureMode = new System.Windows.Forms.ComboBox();
            this.cmbNetType = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processedImgBox)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tresholdTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marginTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderTrackBar)).BeginInit();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkAccuracy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEpochs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZoningGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbVideoSource
            // 
            this.cmbVideoSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbVideoSource.FormattingEnabled = true;
            this.cmbVideoSource.Location = new System.Drawing.Point(17, 711);
            this.cmbVideoSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbVideoSource.Name = "cmbVideoSource";
            this.cmbVideoSource.Size = new System.Drawing.Size(291, 24);
            this.cmbVideoSource.TabIndex = 1;
            this.cmbVideoSource.SelectionChangeCommitted += new System.EventHandler(this.cmbVideoSource_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 692);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выбор камеры";
            // 
            // StartButton
            // 
            this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartButton.Location = new System.Drawing.Point(317, 734);
            this.StartButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(167, 37);
            this.StartButton.TabIndex = 3;
            this.StartButton.Text = "Старт";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.originalImageBox);
            this.groupBox1.Location = new System.Drawing.Point(1, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(683, 639);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Камера";
            // 
            // originalImageBox
            // 
            this.originalImageBox.Location = new System.Drawing.Point(8, 15);
            this.originalImageBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(667, 615);
            this.originalImageBox.TabIndex = 1;
            this.originalImageBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.processedImgBox);
            this.panel1.Location = new System.Drawing.Point(692, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 547);
            this.panel1.TabIndex = 12;
            // 
            // processedImgBox
            // 
            this.processedImgBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processedImgBox.Location = new System.Drawing.Point(0, 0);
            this.processedImgBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.processedImgBox.Name = "processedImgBox";
            this.processedImgBox.Size = new System.Drawing.Size(588, 543);
            this.processedImgBox.TabIndex = 0;
            this.processedImgBox.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkAutoPredict);
            this.panel2.Controls.Add(this.lblPredict);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tresholdTrackBar);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.marginTrackBar);
            this.panel2.Controls.Add(this.borderTrackBar);
            this.panel2.Location = new System.Drawing.Point(692, 570);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(589, 248);
            this.panel2.TabIndex = 18;
            // 
            // chkAutoPredict
            // 
            this.chkAutoPredict.AutoSize = true;
            this.chkAutoPredict.Checked = true;
            this.chkAutoPredict.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoPredict.Location = new System.Drawing.Point(204, 101);
            this.chkAutoPredict.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAutoPredict.Name = "chkAutoPredict";
            this.chkAutoPredict.Size = new System.Drawing.Size(168, 20);
            this.chkAutoPredict.TabIndex = 26;
            this.chkAutoPredict.Text = "Авто-распознавание";
            this.chkAutoPredict.UseVisualStyleBackColor = true;
            // 
            // lblPredict
            // 
            this.lblPredict.AutoSize = true;
            this.lblPredict.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPredict.Location = new System.Drawing.Point(80, 174);
            this.lblPredict.Name = "lblPredict";
            this.lblPredict.Size = new System.Drawing.Size(172, 29);
            this.lblPredict.TabIndex = 25;
            this.lblPredict.Text = "Predict: NONE";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(23, 101);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 20);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "Обработать";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(465, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Порог";
            // 
            // tresholdTrackBar
            // 
            this.tresholdTrackBar.LargeChange = 1;
            this.tresholdTrackBar.Location = new System.Drawing.Point(396, 38);
            this.tresholdTrackBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tresholdTrackBar.Maximum = 255;
            this.tresholdTrackBar.Name = "tresholdTrackBar";
            this.tresholdTrackBar.Size = new System.Drawing.Size(187, 56);
            this.tresholdTrackBar.TabIndex = 22;
            this.tresholdTrackBar.TickFrequency = 25;
            this.tresholdTrackBar.Value = 120;
            this.tresholdTrackBar.ValueChanged += new System.EventHandler(this.tresholdTrackBar_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 21;
            this.label4.Text = "Зазор";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Поля";
            // 
            // marginTrackBar
            // 
            this.marginTrackBar.LargeChange = 10;
            this.marginTrackBar.Location = new System.Drawing.Point(204, 38);
            this.marginTrackBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.marginTrackBar.Maximum = 40;
            this.marginTrackBar.Name = "marginTrackBar";
            this.marginTrackBar.Size = new System.Drawing.Size(187, 56);
            this.marginTrackBar.TabIndex = 19;
            this.marginTrackBar.TickFrequency = 4;
            this.marginTrackBar.Value = 10;
            this.marginTrackBar.ValueChanged += new System.EventHandler(this.marginTrackBar_ValueChanged);
            // 
            // borderTrackBar
            // 
            this.borderTrackBar.LargeChange = 60;
            this.borderTrackBar.Location = new System.Drawing.Point(9, 38);
            this.borderTrackBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.borderTrackBar.Maximum = 160;
            this.borderTrackBar.Name = "borderTrackBar";
            this.borderTrackBar.Size = new System.Drawing.Size(187, 56);
            this.borderTrackBar.TabIndex = 18;
            this.borderTrackBar.TickFrequency = 10;
            this.borderTrackBar.Value = 40;
            this.borderTrackBar.ValueChanged += new System.EventHandler(this.borderTrackBar_ValueChanged);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.Location = new System.Drawing.Point(13, 650);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(96, 29);
            this.statusLabel.TabIndex = 24;
            this.statusLabel.Text = "Статус:";
            // 
            // ticksLabel
            // 
            this.ticksLabel.AutoSize = true;
            this.ticksLabel.Location = new System.Drawing.Point(501, 716);
            this.ticksLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ticksLabel.Name = "ticksLabel";
            this.ticksLabel.Size = new System.Drawing.Size(165, 16);
            this.ticksLabel.TabIndex = 30;
            this.ticksLabel.Text = "Ticks for frame processing";
            // 
            // resolutionsBox
            // 
            this.resolutionsBox.AllowDrop = true;
            this.resolutionsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionsBox.FormattingEnabled = true;
            this.resolutionsBox.Location = new System.Drawing.Point(19, 745);
            this.resolutionsBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.resolutionsBox.Name = "resolutionsBox";
            this.resolutionsBox.Size = new System.Drawing.Size(289, 24);
            this.resolutionsBox.TabIndex = 34;
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.label13);
            this.controlPanel.Controls.Add(this.label12);
            this.controlPanel.Controls.Add(this.label11);
            this.controlPanel.Controls.Add(this.label10);
            this.controlPanel.Controls.Add(this.label8);
            this.controlPanel.Controls.Add(this.label7);
            this.controlPanel.Controls.Add(this.label6);
            this.controlPanel.Controls.Add(this.lblTrainTime);
            this.controlPanel.Controls.Add(this.lblTrainStatus);
            this.controlPanel.Controls.Add(this.trainProgressBar);
            this.controlPanel.Controls.Add(this.btnTest);
            this.controlPanel.Controls.Add(this.btnTrain);
            this.controlPanel.Controls.Add(this.btnRecreateNet);
            this.controlPanel.Controls.Add(this.chkParallel);
            this.controlPanel.Controls.Add(this.trkAccuracy);
            this.controlPanel.Controls.Add(this.numEpochs);
            this.controlPanel.Controls.Add(this.txtHiddenLayers);
            this.controlPanel.Controls.Add(this.numZoningGrid);
            this.controlPanel.Controls.Add(this.cmbFeatureMode);
            this.controlPanel.Controls.Add(this.cmbNetType);
            this.controlPanel.Location = new System.Drawing.Point(1291, 15);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.controlPanel.Size = new System.Drawing.Size(343, 583);
            this.controlPanel.TabIndex = 35;
            this.controlPanel.TabStop = false;
            this.controlPanel.Text = "Параметры сети";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(7, 304);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 18);
            this.label13.TabIndex = 21;
            this.label13.Text = "Точность";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(65, 213);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(128, 18);
            this.label12.TabIndex = 20;
            this.label12.Text = "Количество эпох";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(109, 181);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 24);
            this.label11.TabIndex = 19;
            this.label11.Text = "Обучение";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(147, 123);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 18);
            this.label10.TabIndex = 18;
            this.label10.Text = "Сетка";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(7, 96);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(179, 18);
            this.label8.TabIndex = 16;
            this.label8.Text = "Конфигурация сенсоров";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(3, 64);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(192, 18);
            this.label7.TabIndex = 15;
            this.label7.Text = "Структура (скрытые слои)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(155, 33);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "Сеть";
            // 
            // lblTrainTime
            // 
            this.lblTrainTime.AutoSize = true;
            this.lblTrainTime.Location = new System.Drawing.Point(11, 544);
            this.lblTrainTime.Name = "lblTrainTime";
            this.lblTrainTime.Size = new System.Drawing.Size(41, 16);
            this.lblTrainTime.TabIndex = 13;
            this.lblTrainTime.Text = "Time:";
            // 
            // lblTrainStatus
            // 
            this.lblTrainStatus.AutoSize = true;
            this.lblTrainStatus.Location = new System.Drawing.Point(11, 485);
            this.lblTrainStatus.Name = "lblTrainStatus";
            this.lblTrainStatus.Size = new System.Drawing.Size(89, 16);
            this.lblTrainStatus.TabIndex = 12;
            this.lblTrainStatus.Text = "Status: NONE";
            // 
            // trainProgressBar
            // 
            this.trainProgressBar.Location = new System.Drawing.Point(11, 514);
            this.trainProgressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trainProgressBar.Name = "trainProgressBar";
            this.trainProgressBar.Size = new System.Drawing.Size(321, 20);
            this.trainProgressBar.TabIndex = 11;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(219, 425);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(113, 37);
            this.btnTest.TabIndex = 10;
            this.btnTest.Text = "Тест";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(5, 425);
            this.btnTrain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(113, 37);
            this.btnTrain.TabIndex = 9;
            this.btnTrain.Text = "Обучить";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnRecreateNet
            // 
            this.btnRecreateNet.Location = new System.Drawing.Point(69, 258);
            this.btnRecreateNet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRecreateNet.Name = "btnRecreateNet";
            this.btnRecreateNet.Size = new System.Drawing.Size(217, 34);
            this.btnRecreateNet.TabIndex = 8;
            this.btnRecreateNet.Text = "Пересоздать сеть";
            this.btnRecreateNet.UseVisualStyleBackColor = true;
            this.btnRecreateNet.Click += new System.EventHandler(this.btnRecreateNet_Click);
            // 
            // chkParallel
            // 
            this.chkParallel.AutoSize = true;
            this.chkParallel.Location = new System.Drawing.Point(11, 385);
            this.chkParallel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkParallel.Name = "chkParallel";
            this.chkParallel.Size = new System.Drawing.Size(176, 20);
            this.chkParallel.TabIndex = 7;
            this.chkParallel.Text = "Параллельный расчет";
            this.chkParallel.UseVisualStyleBackColor = true;
            // 
            // trkAccuracy
            // 
            this.trkAccuracy.Location = new System.Drawing.Point(43, 325);
            this.trkAccuracy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trkAccuracy.Maximum = 100;
            this.trkAccuracy.Name = "trkAccuracy";
            this.trkAccuracy.Size = new System.Drawing.Size(249, 56);
            this.trkAccuracy.TabIndex = 6;
            this.trkAccuracy.TickFrequency = 10;
            this.trkAccuracy.Value = 90;
            // 
            // numEpochs
            // 
            this.numEpochs.Location = new System.Drawing.Point(212, 210);
            this.numEpochs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numEpochs.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numEpochs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEpochs.Name = "numEpochs";
            this.numEpochs.Size = new System.Drawing.Size(120, 22);
            this.numEpochs.TabIndex = 5;
            this.numEpochs.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // txtHiddenLayers
            // 
            this.txtHiddenLayers.Location = new System.Drawing.Point(212, 63);
            this.txtHiddenLayers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtHiddenLayers.Name = "txtHiddenLayers";
            this.txtHiddenLayers.Size = new System.Drawing.Size(121, 22);
            this.txtHiddenLayers.TabIndex = 4;
            this.txtHiddenLayers.Text = "64;32";
            // 
            // numZoningGrid
            // 
            this.numZoningGrid.Location = new System.Drawing.Point(212, 123);
            this.numZoningGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numZoningGrid.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numZoningGrid.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numZoningGrid.Name = "numZoningGrid";
            this.numZoningGrid.Size = new System.Drawing.Size(120, 22);
            this.numZoningGrid.TabIndex = 3;
            this.numZoningGrid.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // cmbFeatureMode
            // 
            this.cmbFeatureMode.FormattingEnabled = true;
            this.cmbFeatureMode.Items.AddRange(new object[] {
            "Zoning",
            "Transitions",
            "RowColHist"});
            this.cmbFeatureMode.Location = new System.Drawing.Point(212, 92);
            this.cmbFeatureMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbFeatureMode.Name = "cmbFeatureMode";
            this.cmbFeatureMode.Size = new System.Drawing.Size(121, 24);
            this.cmbFeatureMode.TabIndex = 1;
            // 
            // cmbNetType
            // 
            this.cmbNetType.FormattingEnabled = true;
            this.cmbNetType.Items.AddRange(new object[] {
            "Student",
            "Accord"});
            this.cmbNetType.Location = new System.Drawing.Point(212, 32);
            this.cmbNetType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbNetType.Name = "cmbNetType";
            this.cmbNetType.Size = new System.Drawing.Size(121, 24);
            this.cmbNetType.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1640, 846);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.resolutionsBox);
            this.Controls.Add(this.ticksLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbVideoSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Распознавалка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.processedImgBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tresholdTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marginTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderTrackBar)).EndInit();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkAccuracy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEpochs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZoningGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbVideoSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar marginTrackBar;
        private System.Windows.Forms.TrackBar borderTrackBar;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label ticksLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tresholdTrackBar;
        private System.Windows.Forms.PictureBox processedImgBox;
        private System.Windows.Forms.ComboBox resolutionsBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lblPredict;
        private System.Windows.Forms.GroupBox controlPanel;
        private System.Windows.Forms.ComboBox cmbNetType;
        private System.Windows.Forms.ComboBox cmbFeatureMode;
        private System.Windows.Forms.NumericUpDown numZoningGrid;
        private System.Windows.Forms.TextBox txtHiddenLayers;
        private System.Windows.Forms.NumericUpDown numEpochs;
        private System.Windows.Forms.TrackBar trkAccuracy;
        private System.Windows.Forms.CheckBox chkParallel;
        private System.Windows.Forms.Button btnRecreateNet;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.ProgressBar trainProgressBar;
        private System.Windows.Forms.Label lblTrainStatus;
        private System.Windows.Forms.CheckBox chkAutoPredict;
        private System.Windows.Forms.Label lblTrainTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
    }
}

