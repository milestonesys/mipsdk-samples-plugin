using SCExport.Background;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCExport.Client
{
    /// <summary>
    /// Interaction logic for SCExportSidePanelWpfUserControl.xaml
    /// </summary>
    public partial class SCExportSidePanelWpfUserControl : SidePanelWpfUserControl, INotifyPropertyChanged
    {
        // if path plus the name is more than 60 characters in length the db export will fail, this is checked later but do not create a long path here
        private const string PATH = "C:\\Export";

        private Item _selectedItem;
        private object _selectedCameraChangedReceiver;

        private string _overlayImageFileName = null;

        private int _progressBar;
        public int ProgressBar
        {
            get
            {
                return _progressBar;
            }
            set
            {
                _progressBar = value;
                OnPropertyChanged();
            }
        }

        private string _progressBarText;
        public string ProgressBarText
        {
            get
            {
                return _progressBarText;
            }
            set
            {
                _progressBarText = value;
                OnPropertyChanged();
            }
        }

        private string _currentItemName = "";
        public string CurrentItemName
        {
            get
            {
                return _currentItemName;
            }
            set
            {
                _currentItemName = value;
                OnPropertyChanged();
            }
        }

        private bool _buttonsEnabled;

        public bool ButtonsEnabled
        {
            get
            {
                return _buttonsEnabled;
            }
            set
            {
                _buttonsEnabled = value;
                OnPropertyChanged();
            }
        }

        public SCExportSidePanelWpfUserControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public override void Init()
        {
            //set up ApplicationController events
            _selectedCameraChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(SelectedCameraChangedHandler),
                                                         new MessageIdFilter(MessageId.SmartClient.SelectedCameraChangedIndication));

            // Set initially selected camera if relevant
            Collection<object> result = EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.GetSelectedCameraRequest));
            if (result != null && result.Count > 0)
            {
                SetSelectedItem(result[0] as FQID);
            }
            else
            {
                SetSelectedItem(null);
            }

            SCExportBackgroundPlugin.UserControl = this;
        }

        public override void Close()
        {
            //remove ApplicationController events
            EnvironmentManager.Instance.UnRegisterReceiver(_selectedCameraChangedReceiver);
            SCExportBackgroundPlugin.UserControl = null;
        }

        private object SelectedCameraChangedHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            SetSelectedItem(message.RelatedFQID);
            return null;
        }

        private void SetSelectedItem(FQID selectedItemId)
        {
            if (selectedItemId != null)
                _selectedItem = Configuration.Instance.GetItem(selectedItemId);

            if (_selectedItem != null)
            {
                CurrentItemName = _selectedItem.Name;
                ButtonsEnabled = true;
            }
            else
            {
                CurrentItemName = "";
                ButtonsEnabled = false;
            }
        }

        // if the path plus the name is more than 60 characters in length the export will fail
        private string TruncateName(string name)
        {
            int maxLength = 60 - PATH.Length;
            if (name.Length > maxLength)
            {
                name = name.Substring(0, maxLength).TrimEnd();
            }
            return name;
        }

        private delegate void StringMethod(string s, int p);
        internal void ShowStatus(string status, int percent)
        {

            ProgressBarText = status;
            ProgressBar = percent;
        }

        private void OverlayImage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*|BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _overlayImageFileName = openFileDialog.FileName;
                OverlayImage.Content = _overlayImageFileName;
            }
        }

        private void OnMKVExport_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                Directory.CreateDirectory(PATH);
                SCExportJob job = new SCExportJob()
                {
                    Item = _selectedItem,
                    StartTime = DateTime.Now - TimeSpan.FromSeconds(15),
                    EndTime = DateTime.Now,
                    FileName = _selectedItem.Name + ".mkv",
                    Path = PATH,
                    MKVexport = true
                };
                SCExportBackgroundPlugin.AddJob(job);
            }
        }

        private void OnDBExport_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                Directory.CreateDirectory(PATH);
                SCExportJob job = new SCExportJob()
                {
                    Item = _selectedItem,
                    StartTime = DateTime.Now - TimeSpan.FromSeconds(15),
                    EndTime = DateTime.Now,
                    FileName = TruncateName(_selectedItem.Name),
                    Path = PATH,
                    AVIexport = false,
                    SignExport = (bool)SignExport.IsChecked,
                    PreventReExport = (bool)PreventReExport.IsChecked
                };
                SCExportBackgroundPlugin.AddJob(job);
            }
        }

        private void OnAVIExport_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                Directory.CreateDirectory(PATH);

                Bitmap overlayImage = null;
                if ((bool)IncludeOverlayImage.IsChecked)
                {
                    if (_overlayImageFileName == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select an image file for the overlay image.", "Select image file");
                        return;
                    }

                    overlayImage = (Bitmap)Image.FromFile(_overlayImageFileName);
                }


                SCExportJob job = new SCExportJob()
                {
                    Item = _selectedItem,
                    StartTime = DateTime.Now - TimeSpan.FromSeconds(15),
                    EndTime = DateTime.Now,
                    FileName = _selectedItem.Name + ".avi",
                    Path = PATH,
                    AVIexport = true,
                    OverlayImage = overlayImage,
                    VerticalOverlayPosition = VideoOS.Platform.Data.AVIExporter.VerticalOverlayPositionTop,
                    HorizontalOverlayPosition = VideoOS.Platform.Data.AVIExporter.HorizontalOverlayPositionLeft,
                    ScaleFactor = 0.1,
                    IgnoreAspect = false
                };
                SCExportBackgroundPlugin.AddJob(job);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
