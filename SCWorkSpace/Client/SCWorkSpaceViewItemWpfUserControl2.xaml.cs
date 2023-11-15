using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

using VideoOS.Platform.UI.ItemPicker.Utilities;
using Item = VideoOS.Platform.Item;

namespace SCWorkSpace.Client
{
    public partial class SCWorkSpaceViewItemWpfUserControl2 : ViewItemWpfUserControl
    {
        private List<object> _messageRegistrationObjects = new List<object>();
        private ImageViewerWpfControl _imageViewerWpfControl;
        private AudioPlayer _audioPlayer;
        private AudioPlayer _audioPlayerSpeaker;
        private Item _selectedCameraItem;
        private FQID _playbackFQID;
        private PlaybackWpfUserControl _playbackWpfUserControl;

        public SCWorkSpaceViewItemWpfUserControl2()
        {
            InitializeComponent();

            ClientControl.Instance.RegisterUIControlForAutoTheming(this);
        }

        public override void Init()
        {
            base.Init();



            //set up camera video
            _imageViewerWpfControl = new ImageViewerWpfControl(WindowInformation);
            _imageViewerWpfControl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            _imageViewerWpfControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            _imageViewerWpfControl.EnableMouseControlledPtz = true;
            _imageViewerWpfControl.Selected = true;
            canvasVideoGrid.Children.Add(_imageViewerWpfControl);

            //set up timeline
            _playbackWpfUserControl = new PlaybackWpfUserControl();
            canvasPlaybackControlGrid.Children.Add(_playbackWpfUserControl);

            _playbackFQID = ClientControl.Instance.GeneratePlaybackController();
            _playbackWpfUserControl.Init(_playbackFQID);
            _imageViewerWpfControl.PlaybackControllerFQID = _playbackFQID;
            // set up microphone
            _audioPlayer = new AudioPlayer(_playbackFQID);

            //set up speaker
            _audioPlayerSpeaker = new AudioPlayer(_playbackFQID);
        }

        public override void Close()
        {
            base.Close();

            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();
        }



        /// <summary>
        /// Do not show the sliding toolbar!
        /// </summary>
        public override bool ShowToolbar
        {
            get { return false; }
        }

        #region Component events
        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        #endregion

        private void selectCameraBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _imageViewerWpfControl.Disconnect();
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                SelectedItems = new List<Item>() { _selectedCameraItem },
                Items = Configuration.Instance.GetItemsByKind(Kind.Camera),
            };
            form.ShowDialog();
            if (form.SelectedItems != null && form.SelectedItems.Any())
            {
                _selectedCameraItem = form.SelectedItems.First();
                FillCameraSelection();
                FillMicrophoneSelection();
            }
        }

        private void FillCameraSelection()
        {
            selectCameraBtn.Content = "";
            if (_selectedCameraItem != null)
            {
                selectCameraBtn.Content = _selectedCameraItem.Name;
                _imageViewerWpfControl.CameraFQID = _selectedCameraItem.FQID;
                _imageViewerWpfControl.EnableMouseControlledPtz = true;
                _imageViewerWpfControl.EnableMousePtzEmbeddedHandler = false;
                _imageViewerWpfControl.EnableDigitalZoom = false;
                _imageViewerWpfControl.Initialize();
                _imageViewerWpfControl.Connect();
                _imageViewerWpfControl.Selected = true;
                _playbackWpfUserControl.SetCameras(new List<FQID>() { _selectedCameraItem.FQID });
            }
        }

        private void FillMicrophoneSelection()
        {
            if (_audioPlayer.MicrophoneFQID != null)
                _audioPlayer.Disconnect();
            if (_audioPlayerSpeaker.MicrophoneFQID != null)
                _audioPlayerSpeaker.Disconnect();
            if (_selectedCameraItem == null)
                return;

            foreach (Item item in _selectedCameraItem.GetRelated())
            {
                if (item.FQID.Kind == Kind.Microphone)
                {
                    _audioPlayer.MicrophoneFQID = item.FQID;
                    _audioPlayer.Initialize();
                    _audioPlayer.Connect();
                }
                if (item.FQID.Kind == Kind.Speaker)
                {
                    _audioPlayerSpeaker.MicrophoneFQID = item.FQID;
                    _audioPlayerSpeaker.Initialize();
                    _audioPlayerSpeaker.Connect();
                }
            }
        }

        private void checkBoxLive_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            FQID newPlayback = null;
            _imageViewerWpfControl.PlaybackControllerFQID = newPlayback;
            _audioPlayer.PlaybackControllerFQID = newPlayback;
            _audioPlayerSpeaker.PlaybackControllerFQID = newPlayback;
            canvasPlaybackControlGrid.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void checkBoxLive_unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            FQID newPlayback = null;
            newPlayback = _playbackFQID;
            _imageViewerWpfControl.PlaybackControllerFQID = newPlayback;
            _audioPlayer.PlaybackControllerFQID = newPlayback;
            _audioPlayerSpeaker.PlaybackControllerFQID = newPlayback;
            canvasPlaybackControlGrid.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
