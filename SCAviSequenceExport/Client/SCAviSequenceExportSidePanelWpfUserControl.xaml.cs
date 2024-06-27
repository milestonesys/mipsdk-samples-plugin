using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.UI;

namespace SCAviSequenceExport.Client
{
    public partial class SCAviSequenceExportSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        Item _selectedCamera = null;
        bool _shutDown = false;
        AVIExporter _exporter = null;

        public SCAviSequenceExportSidePanelWpfUserControl()
        {
            InitializeComponent();
            EnableAddButton();
        }

        public override void Init()
        {
        }

        public override void Close()
        {
            _shutDown = true;
        }

        private void selectCameraButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                Items = Configuration.Instance.GetItems(ItemHierarchy.SystemDefined)
            };
            form.ShowDialog();
            if(form.SelectedItems != null && form.SelectedItems.Any())
            {
                _selectedCamera = form.SelectedItems.First();
                selectCameraButton.Content = _selectedCamera.Name;
                EnableAddButton();
            }
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            EnableAddButton();
        }

        private void EnableAddButton()
        {
            addButton.IsEnabled = _selectedCamera != null && startDatePicker.Value < endDatePicker.Value;
        }

        private void addButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            exportItemsListBox.Items.Add(
                new ExportItem()
                {
                    Camera = _selectedCamera,
                    StartTime = startDatePicker.Value,
                    EndTime = endDatePicker.Value,
                    OverlayText = overlayTextBox.Text
                });
            _selectedCamera = null;
            selectCameraButton.Content = "Select camera...";
            overlayTextBox.Text = string.Empty;
            EnableAddButton();
            startExportButton.IsEnabled = true;
        }

        private class ExportItem : SequenceAviExportElement
        {
            public override string ToString()
            {
                return Camera.Name + "," + StartTime.ToShortTimeString() + "," + EndTime.ToShortTimeString() + "," + OverlayText;
            }
        }

        private void startExportButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            exportProgressBar.Value = 0;
            errorLabel.Content = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                _exporter = new AVIExporter();
                _exporter.Path = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
                _exporter.Filename = System.IO.Path.GetFileName(saveFileDialog.FileName + ".avi");
                _exporter.Timestamp = (bool)timestampCheckBox.IsChecked;
                List<ExportItem> exportItems = new List<ExportItem>();
                foreach (ExportItem item in exportItemsListBox.Items)
                    exportItems.Add(item);
                if (_exporter.StartExport(exportItems))
                {
                    var thread = new Thread(UpdateProgressThread);
                    thread.Start();
                    cancelButton.IsEnabled = true;
                    startExportButton.IsEnabled = false;
                }
                else
                {
                    ShowError(_exporter.LastErrorString);
                    _exporter.EndExport();
                }
            }
        }

        private delegate void UpdateProgressDelegate(int progress);

        private void UpdateProgress(int progress)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new UpdateProgressDelegate(UpdateProgress), progress);
            }
            else
            {
                exportProgressBar.Value = progress;
                if (progress == 100)
                {
                    exportItemsListBox.Items.Clear();
                    cancelButton.IsEnabled = false;
                }
            }
        }

        private delegate void ShowErrorDelegate(string error);
        private void ShowError(string error)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ShowErrorDelegate(ShowError), error);
            }
            else
            {
                errorLabel.Content = error;
                cancelButton.IsEnabled = false;
            }
        }

        private void UpdateProgressThread()
        {
            while (!_shutDown && _exporter.LastError == -1)
            {
                UpdateProgress(_exporter.Progress);
                Thread.Sleep(100);
            }
            if (_shutDown)
            {
                _exporter.Cancel();
                return;
            }
            if (_exporter.LastError == 0)
                UpdateProgress(100);
            else if (_exporter.LastError == 1)
            {
                ShowError("Cancelled");
                return;
            }
            else
                ShowError(_exporter.LastErrorString);

            _exporter.EndExport();
        }

        private void cancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _exporter.Cancel();
        }
    }
}

