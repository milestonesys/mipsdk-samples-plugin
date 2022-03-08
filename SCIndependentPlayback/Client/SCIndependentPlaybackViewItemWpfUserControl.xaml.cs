using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCIndependentPlayback.Client
{
    /// <summary>
    /// SCIndependentPlaybackViewItemWpfUserControl uses the WPF versions of the PlaybackWpfUserControl, ImageViewerWpfControl, winforms version of AudioPlayerControl
    /// </summary>
    public partial class SCIndependentPlaybackViewItemWpfUserControl : ViewItemWpfUserControl
    {

        #region Component private class variables

        private SCIndependentPlaybackViewItemManager _viewItemManager;
        private PlaybackWpfUserControl _playbackWpfUserControl;
        private ImageViewerWpfControl _imageViewerControl;
        private AudioPlayerControl _audioPlayerControl;
        private FQID _playbackFQID;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a SCIndependentPlaybackViewItemUserControl instance
        /// </summary>

        public SCIndependentPlaybackViewItemWpfUserControl(SCIndependentPlaybackViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _viewItemManager.PropertyChangedEvent += ViewItemManagerPropertyChangedEvent;
            _imageViewerControl.MouseLeftButtonDown += ImageViewerControlClickEvent;
            _imageViewerControl.MouseRightButtonDown += _imageViewerControl_RightClickEvent;
            _imageViewerControl.MouseDoubleClick += _imageViewerControl_DoubleClickEvent;
            _imageViewerControl.AllowDrop = true;
            _imageViewerControl.Drop += _imageViewerControl_Drop;
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            _viewItemManager.PropertyChangedEvent -= ViewItemManagerPropertyChangedEvent;
            _imageViewerControl.MouseLeftButtonDown -= ImageViewerControlClickEvent;
            _imageViewerControl.MouseRightButtonDown -= _imageViewerControl_RightClickEvent;
            _imageViewerControl.MouseDoubleClick -= _imageViewerControl_DoubleClickEvent;
            _imageViewerControl.AllowDrop = false;
            _imageViewerControl.Drop -= _imageViewerControl_Drop;
        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {
            _playbackWpfUserControl = new PlaybackWpfUserControl();
            canvasPlaybackControl.Children.Add(_playbackWpfUserControl);

            _audioPlayerControl = ClientControl.Instance.GenerateAudioPlayerControl(WindowInformation);
            _audioPlayerControl.Dock = DockStyle.Fill;
            _audioPlayerControlHost.Child = _audioPlayerControl;

            _imageViewerControl = new ImageViewerWpfControl(WindowInformation);
            _imageViewerControl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            _imageViewerControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            _imageViewerControl.EnableMouseControlledPtz = true;
            _imageViewerControl.Selected = true;

            canvasVideo.Children.Add(_imageViewerControl);

            _playbackFQID = ClientControl.Instance.GeneratePlaybackController();
            _playbackWpfUserControl.Init(_playbackFQID);
            _imageViewerControl.PlaybackControllerFQID = _playbackFQID;

            _audioPlayerControl.PlaybackControllerFQID = _playbackFQID;

            PlaybackController pc = ClientControl.Instance.GetPlaybackController(_playbackFQID);
            pc.SkipGaps = false;

            SetUpApplicationEventListeners();
            _imageViewerControl.Initialize();               // Make sure Click events have been configured
            if (_viewItemManager.SelectedCamera != null)
            {
                ViewItemManagerPropertyChangedEvent(this, null);
            }
        }

        /// <summary>
        /// Method that is called when the view item is closed. The view item should free all resources when the method is called.
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            _playbackWpfUserControl.Close();
            _imageViewerControl.Disconnect();
            _imageViewerControl.Close();
            _audioPlayerControl.Disconnect();
            _audioPlayerControl.Close();
            ClientControl.Instance.ReleasePlaybackController(_playbackFQID);
            RemoveApplicationEventListeners();
        }

        #endregion

        #region Component events

        private void ImageViewerControlClickEvent(object sender, EventArgs e)
        {
            FireClickEvent();
            _imageViewerControl.Selected = true;
        }

        void _imageViewerControl_DoubleClickEvent(object sender, EventArgs e)
        {
            FireDoubleClickEvent();
        }

        void _imageViewerControl_RightClickEvent(object sender, EventArgs e)
        {
            if (_playbackWpfUserControl.IsEnabled)
            {
                _playbackWpfUserControl.SetEnabled(false);
                _playbackWpfUserControl.IsEnabled = false;
                _playbackWpfUserControl.Visibility = System.Windows.Visibility.Collapsed;
                _audioPlayerControl.PlaybackControllerFQID = null;
                _imageViewerControl.PlaybackControllerFQID = null;
                if (WindowInformation.Mode == Mode.ClientLive)
                    _imageViewerControl.StartLive();
                else
                    _imageViewerControl.StartBrowse();
            }
            else
            {
                _playbackWpfUserControl.SetEnabled(true);
                _playbackWpfUserControl.IsEnabled = true;
                _playbackWpfUserControl.Visibility = System.Windows.Visibility.Visible;
                _audioPlayerControl.PlaybackControllerFQID = _playbackFQID;
                _imageViewerControl.PlaybackControllerFQID = _playbackFQID;
                _imageViewerControl.StartBrowse();
            }
        }

        void ViewItemManagerPropertyChangedEvent(object sender, EventArgs e)
        {
            if (_imageViewerControl.CameraFQID != null)
            {
                _imageViewerControl.Disconnect();
            }
            _imageViewerControl.CameraFQID = _viewItemManager.SelectedCamera.FQID;
            _imageViewerControl.Initialize();
            _imageViewerControl.Connect();

            _playbackWpfUserControl.SetCameras(new List<FQID>() { _viewItemManager.SelectedCamera.FQID });

            Item camera = Configuration.Instance.GetItem(_viewItemManager.SelectedCamera.FQID);
            Item mic = null;
            foreach (Item item in camera.GetRelated())
            {
                if (item.FQID.Kind == Kind.Microphone)
                    mic = item;
            }
            if (_audioPlayerControl.MicrophoneFQID != null)
                _audioPlayerControl.Disconnect();

            if (mic != null)
            {
                _audioPlayerControl.MicrophoneFQID = mic.FQID;
                _audioPlayerControl.Initialize();
                _audioPlayerControl.Connect();
            }
        }

        private void _imageViewerControl_Drop(object sender, System.Windows.DragEventArgs e)
        {
            // Obtaining the Guid of the dropped camera like this
            var cameraGuidList = e.Data.GetData("VideoOS.RemoteClient.Application.DragDrop.DraggedDeviceIdList") as List<Guid>;
            if (cameraGuidList != null && cameraGuidList.Any())
            {
                _viewItemManager.SelectedCamera = Configuration.Instance.GetItem(cameraGuidList[0], Kind.Camera);
                ViewItemManagerPropertyChangedEvent(null, null);
                e.Handled = true;
            }
            else
            {
                MessageBox.Show("DragDrop: the only supported dragged content for this plugin is a camera");

                // Keeping this false lets the SmartClient to continue handling this event (in the case if you dropped another plugin, it will change to this plugin in the view)
                e.Handled = false;
            }
        }
        #endregion

        #region Component properties

        /// <summary>
        /// Gets boolean indicating whether the view item can be maximized or not. <br/>
        /// The content holder should implement the click and double click events even if it is not maximizable. 
        /// </summary>
        public override bool Maximizable
        {
            get { return true; }
        }

        /// <summary>
        /// Tell if ViewItem is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Do not show the sliding toolbar!
        /// </summary>
        public override bool ShowToolbar
        {
            get { return true; }
        }

        public override void Print()
        {
            if (_viewItemManager.SelectedCamera != null)
                base.Print("MyName", "Some Information", _viewItemManager.SelectedCamera.Name);
            else
                base.Print("MyName", "Some Information");
        }

        public override bool Selected
        {
            get => base.Selected;
            set
            {
                if (_imageViewerControl != null)
                {
                    _imageViewerControl.Selected = value;
                }
                base.Selected = value;
            }
        }

        #endregion
    }
}
