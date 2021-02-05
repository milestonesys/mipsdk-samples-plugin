using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.UI;

namespace SCAudioExport.Client
{
    public partial class SCAudioExportSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        private Item _item;
        private List<Item> _audioList = new List<Item>();
        private string _selectedPath;
        private ConcurrentQueue<ExportJob> _exportJobsQueue = new ConcurrentQueue<ExportJob>();
        private ConcurrentQueue<CancellationTokenSource> _ctsQueue = new ConcurrentQueue<CancellationTokenSource>();
        public SCAudioExportSidePanelWpfUserControl()
        {
            InitializeComponent();
            _comboBoxSampleRates.SelectedIndex = 0;
            BuildCodecList();
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        private void _buttonAddMicrophone_Click(object sender, RoutedEventArgs e)
        {
            AddItemByKind(Kind.Microphone);
        }

        private void _buttonAddSpeaker_Click(object sender, RoutedEventArgs e)
        {
            AddItemByKind(Kind.Speaker);
        }

        private void AddItemByKind(Guid kind)
        {
            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = kind;
            form.Init();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _item = form.SelectedItem;
                if (!_audioList.Contains(_item))
                    _audioList.Add(_item);

                string audioDevicesName = _item.Name;
                if (!_listBoxAudioDevices.Items.Contains(audioDevicesName))
                    _listBoxAudioDevices.Items.Add(audioDevicesName);
            }
        }

        private void _buttonSelectDestination_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _selectedPath = dialog.SelectedPath;
                _buttonSelectDestination.Content = _selectedPath;
            }
        }

        private void _buttonExport_Click(object sender, RoutedEventArgs e)
        {
            if (_audioList.Count == 0)
            {
                UpdateStatus("Please Select audio device before export");
                return;
            }

            if (_datePickerStart.Value.CompareTo(_datePickerEnd.Value) >= 0)
            {
                UpdateStatus("Start time has to be smaller than end time");
                return;
            }

            if (string.IsNullOrWhiteSpace(_textBoxFileName.Text))
            {
                UpdateStatus("Please give it a export file name");
                return;
            }

            if (_buttonSelectDestination.Content.ToString() == "Select Destination")
            {
                UpdateStatus("Please specify a export destination");
                return;
            }

            var wavExporter = new WAVExporter()
            {
                FileName = _textBoxFileName.Text,
                Codec = _comboBoxCodec.SelectedItem.ToString(),
                AudioSampleRate = Int32.Parse(_comboBoxSampleRates.SelectedItem.ToString()),
                AudioList = _audioList,
                Path = _selectedPath
            };

            if (!string.IsNullOrWhiteSpace(_textBoxExportName.Text))
            {
                wavExporter.ExportName = _textBoxExportName.Text;
            }

            _exportJobsQueue.Enqueue(new ExportJob() { Exporter = wavExporter, StartTime = _datePickerStart.Value.ToUniversalTime(), EndTime = _datePickerEnd.Value.ToUniversalTime() });

            //We have different threads accessing the same UpdateProgress, however, since the export jobs in SC are executed sequentially, the progressbar will not show concurret updates
            Progress<int> progressIndicator = new Progress<int>(UpdateProgress);
            var cts = new CancellationTokenSource();
            _ctsQueue.Enqueue(cts);
            Task.Factory.StartNew(() => ExportFromQueueAsync(progressIndicator, cts.Token), cts.Token);
        }

        private async void ExportFromQueueAsync(IProgress<int> progressIndicator, CancellationToken token)
        {
            bool resetProgressBar = true;
            ExportJob currentJob;
            bool success = _exportJobsQueue.TryDequeue(out currentJob);
            try
            {
                if (success && currentJob != null)
                {
                    var wavExporter = currentJob.Exporter;
                    bool started = await Task.Run(() => wavExporter.StartExport(currentJob.StartTime, currentJob.EndTime));
                    if (started)
                    {
                        while (wavExporter.Progress == 0)
                        {
                            await Task.Delay(100, token);
                        }
                        while (wavExporter.Progress < 100 && wavExporter.Progress > 0)
                        {
                            if (resetProgressBar)
                            {
                                progressIndicator.Report(0);
                                resetProgressBar = false;
                            }
                            progressIndicator.Report(wavExporter.Progress);
                            UpdateStatus("Remaining jobs in the queue: " + _exportJobsQueue.Count + " Current job progress: " + wavExporter.Progress);
                            await Task.Delay(300, token);
                        }
                        while (wavExporter.Progress != 100)
                        {
                            await Task.Delay(100, token);
                        }
                        UpdateStatus("Remaining jobs in the queue: " + _exportJobsQueue.Count + " Current job progress: " + wavExporter.Progress);
                        progressIndicator.Report(wavExporter.Progress);
                    }
                    if (wavExporter.LastError != 0)
                    {
                        UpdateStatus("Error:" + wavExporter.LastErrorString);
                        //if (wavExporter.LastError == 102)
                        //Make progress bar red and update it with progress value
                    }

                    wavExporter.EndExport();
                    UpdateStatus("Remaining jobs in the queue: " + _exportJobsQueue.Count);
                }
            }
            catch (OperationCanceledException)
            {
                if (currentJob != null)
                {
                    WAVExporter wavExporter;
                    if ((wavExporter = currentJob.Exporter) != null)
                    {
                        wavExporter.Cancel();
                        if (wavExporter.Progress != 100)
                            progressIndicator.Report(0);
                    }
                }
                while (!_exportJobsQueue.IsEmpty)
                {
                    ExportJob frontJob;
                    bool suc = _exportJobsQueue.TryDequeue(out frontJob);
                    if (suc)
                    {
                        frontJob.Exporter.Cancel();
                        if (frontJob.Exporter.Progress != 100)
                            progressIndicator.Report(0);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateStatus(string msg)
        {
            if (Dispatcher.CheckAccess())
            {
                textBlockStatus.Text = msg;
            }
            else
            {
                Dispatcher.Invoke(() => UpdateStatus(msg));
            }
        }

        private void UpdateProgress(int value)
        {
            if (Dispatcher.CheckAccess())
            {
                _progressBar.Value = value;
            }
            else
            {
                Dispatcher.Invoke(() => UpdateProgress(value));
            }
        }

        private void _buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            while (!_ctsQueue.IsEmpty)
            {
                CancellationTokenSource cts;
                bool success = _ctsQueue.TryDequeue(out cts);
                if (success)
                    cts.Cancel();
            }
        }

        private void BuildCodecList()
        {
            _comboBoxCodec.Items.Clear();
            WAVExporter tempExporter = new WAVExporter();
            tempExporter.Init();
            string[] codecList = tempExporter.CodecList;
            tempExporter.Close();
            foreach (string name in codecList)
            {
                _comboBoxCodec.Items.Add(name);
            }
            _comboBoxCodec.SelectedIndex = 0;
        }

    }
}
