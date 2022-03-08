using System.Collections.Generic;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;
using Item = VideoOS.Platform.Item;

namespace SCWorkSpace.Client
{
    public partial class SCWorkSpaceViewItemWpfUserControl2 : ViewItemWpfUserControl
    {
        private List<object> _messageRegistrationObjects = new List<object>();
        private ImageViewerWpfControl _imageViewerWpfControl;
        private AudioPlayerControl _audioPlayerControl;
        private AudioPlayerControl _audioPlayerControlSpeaker;
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

            // set up microphone
            _audioPlayerControl = ClientControl.Instance.GenerateAudioPlayerControl(WindowInformation);
            _audioPlayerControlHost.Child = _audioPlayerControl;

            //set up speaker
            _audioPlayerControlSpeaker = ClientControl.Instance.GenerateAudioPlayerControl(WindowInformation);
            _audioPlayerControlSpeakerHost.Child = _audioPlayerControlSpeaker;

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
            _audioPlayerControl.PlaybackControllerFQID = _playbackFQID;
            _audioPlayerControlSpeaker.PlaybackControllerFQID = _playbackFQID;
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

            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.Camera;
            form.SelectedItem = _selectedCameraItem;
            form.AutoAccept = true;
            form.Init();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _selectedCameraItem = form.SelectedItem;
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
            if (_audioPlayerControl.MicrophoneFQID != null)
                _audioPlayerControl.Disconnect();
            if (_audioPlayerControlSpeaker.MicrophoneFQID != null)
                _audioPlayerControlSpeaker.Disconnect();
            if (_selectedCameraItem == null)
                return;

            foreach (Item item in _selectedCameraItem.GetRelated())
            {
                if (item.FQID.Kind == Kind.Microphone)
                {
                    _audioPlayerControl.MicrophoneFQID = item.FQID;
                    _audioPlayerControl.Initialize();
                    _audioPlayerControl.Connect();
                }
                if (item.FQID.Kind == Kind.Speaker)
                {
                    _audioPlayerControlSpeaker.MicrophoneFQID = item.FQID;
                    _audioPlayerControlSpeaker.Initialize();
                    _audioPlayerControlSpeaker.Connect();
                }
            }
        }

        private void checkBoxLive_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            FQID newPlayback = null;
            _imageViewerWpfControl.PlaybackControllerFQID = newPlayback;
            _audioPlayerControl.PlaybackControllerFQID = newPlayback;
            _audioPlayerControlSpeaker.PlaybackControllerFQID = newPlayback;
            canvasPlaybackControlGrid.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void checkBoxLive_unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            FQID newPlayback = null;
            newPlayback = _playbackFQID;
            _imageViewerWpfControl.PlaybackControllerFQID = newPlayback;
            _audioPlayerControl.PlaybackControllerFQID = newPlayback;
            _audioPlayerControlSpeaker.PlaybackControllerFQID = newPlayback;
            canvasPlaybackControlGrid.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
