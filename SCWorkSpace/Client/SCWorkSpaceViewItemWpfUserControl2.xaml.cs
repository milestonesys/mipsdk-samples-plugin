using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using Item = VideoOS.Platform.Item;
using Message = VideoOS.Platform.Messaging.Message;

namespace SCWorkSpace.Client
{
    public partial class SCWorkSpaceViewItemWpfUserControl2 : ViewItemWpfUserControl
    {
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
            _imageViewerWpfControl.Selected = Selected;
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
                _imageViewerWpfControl.Selected = Selected;
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
                AudioPlayer audioPlayer;
                if (item.FQID.Kind == Kind.Microphone)
                {
                    audioPlayer = _audioPlayer;
                }
                else if (item.FQID.Kind == Kind.Speaker)
                {
                    audioPlayer = _audioPlayerSpeaker;
                }
                else
                {
                    continue;
                }

                audioPlayer.MicrophoneFQID = item.FQID;
                audioPlayer.Initialize();
                try
                {
                    audioPlayer.Connect();
                }
                catch (MIPException ex)
                {
                    // AudioPlayer.Connect() throws MIPException if:
                    // - AudioPlayer.MicrophoneFQID is not set
                    // - No audio devices are available e.g. when Windows Audio service is not running
                    DisplayMessage(ex.Message, SmartClientMessageDataType.Warning);
                    audioPlayer.Close();
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
            FQID newPlayback = _playbackFQID;
            _imageViewerWpfControl.PlaybackControllerFQID = newPlayback;
            _audioPlayer.PlaybackControllerFQID = newPlayback;
            _audioPlayerSpeaker.PlaybackControllerFQID = newPlayback;
            canvasPlaybackControlGrid.Visibility = System.Windows.Visibility.Visible;
        }

        // Displays given message in the message area
        private void DisplayMessage(string messageText, SmartClientMessageDataType messageType)
        {
            SmartClientMessageData smartClientMessageData = new SmartClientMessageData
            {
                MessageId = Guid.NewGuid(),
                Message = messageText,
                MessageType = messageType,
                IsClosable = true
            };

            Message message = new Message(MessageId.SmartClient.SmartClientMessageCommand, smartClientMessageData);
            EnvironmentManager.Instance.SendMessage(message);
        }

        /// <summary>
        /// We override the Selected property to inform the ImageViewerWpfControl about selection changes.
        /// </summary>
        public override bool Selected
        {
            get => base.Selected;
            set
            {
                if (_imageViewerWpfControl != null)
                {
                    _imageViewerWpfControl.Selected = value;
                }

                base.Selected = value;
            }
        }
    }
}
