using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCIndependentPlayback.Client2
{
    /// <summary>
    /// SCIndependentPlaybackViewItemWpfUserControl uses the winforms version of PlaybackUserControl, ImageViewerControl, AudioPlayerControl
    /// </summary>
	public partial class SCIndependentPlayback2ViewItemUserControl : ViewItemUserControl
	{
		#region Component private class variables

		private SCIndependentPlayback2ViewItemManager _viewItemManager;
		private PlaybackUserControl _playbackUserControl;
		private ImageViewerControl _imageViewerControl;
		private FQID _playbackFQID;
		private int _zoom = 0;

		#endregion

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a SCIndependentPlaybackViewItemUserControl instance
		/// </summary>
		public SCIndependentPlayback2ViewItemUserControl(SCIndependentPlayback2ViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();
		}

		void _imageViewerControl_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				_zoom += 5;
				if (_zoom > 100)
					_zoom = 100;
			}
			else
			{
				_zoom -= 5;
				if (_zoom < 0)
					_zoom = 0;
			}
			Point p = this.PointToClient(MousePosition);		
			_imageViewerControl.PtzCenter(_imageViewerControl.Height, _imageViewerControl.Width, p.X, p.Y, _zoom);
		}

		void _imageViewerControl_ClickEvent(object sender, EventArgs e)
		{
			_imageViewerControl.EnableDigitalZoom = true;
			MouseEventArgs mouseEventArgs = e as MouseEventArgs;
			if (mouseEventArgs!=null)
				_imageViewerControl.PtzCenter(_imageViewerControl.Height, _imageViewerControl.Width, mouseEventArgs.X, mouseEventArgs.Y, _zoom);
		}

		private void SetUpApplicationEventListeners()
		{
			//set up ViewItem event listeners
			_viewItemManager.PropertyChangedEvent += new EventHandler(ViewItemManagerPropertyChangedEvent);
			_imageViewerControl.MouseClick += new MouseEventHandler(ViewItemUserControlMouseClick);
			_imageViewerControl.DoubleClickEvent += new EventHandler(ViewItemUserControlMouseDoubleClick);
			_imageViewerControl.ClickEvent += new EventHandler(ImageViewerControlClickEvent);
		}

		private void RemoveApplicationEventListeners()
		{
			//remove ViewItem event listeners
			_viewItemManager.PropertyChangedEvent -= new EventHandler(ViewItemManagerPropertyChangedEvent);
			_imageViewerControl.MouseClick -= new MouseEventHandler(ViewItemUserControlMouseClick);
			_imageViewerControl.DoubleClickEvent -= new EventHandler(ViewItemUserControlMouseDoubleClick);
			_imageViewerControl.ClickEvent -= new EventHandler(ImageViewerControlClickEvent);
		}

		/// <summary>
		/// Method that is called immediately after the view item is displayed.
		/// </summary>
		public override void Init()
		{

            _playbackUserControl = ClientControl.Instance.GeneratePlaybackUserControl(this.WindowInformation);
            _playbackUserControl.Dock = DockStyle.Fill;
            panelPlaybackControl.Controls.Add(_playbackUserControl);

            _playbackUserControl.ShowTallUserControl = true;
            _playbackUserControl.ShowSpeedControl = true;
            _playbackUserControl.ShowTimeSpanControl = true;

            _imageViewerControl = ClientControl.Instance.GenerateImageViewerControl(this.WindowInformation);
            _imageViewerControl.Dock = DockStyle.Fill;
            _imageViewerControl.ClickEvent += new EventHandler(_imageViewerControl_ClickEvent);
            _imageViewerControl.MouseWheel += new MouseEventHandler(_imageViewerControl_MouseWheel);
            _imageViewerControl.Selected = true;

            panelVideo.Controls.Add(_imageViewerControl);

            _playbackFQID = ClientControl.Instance.GeneratePlaybackController();
            _playbackUserControl.Init(_playbackFQID);
            _imageViewerControl.PlaybackControllerFQID = _playbackFQID;

            ClientControl.Instance.RegisterUIControlForAutoTheming(this);

            EnvironmentManager.Instance.RegisterReceiver((p1, p2, p3) =>
            {
                System.Diagnostics.Trace.WriteLine("PlaybackCommand:" + ((PlaybackCommandData)p1.Data).Command + ", Main=" + (p3 == null));
                return null;
            }, new MessageIdFilter(MessageId.SmartClient.PlaybackIndication));

            SetUpApplicationEventListeners();
			_imageViewerControl.Initialize();				// Make sure Click events have been configured
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
			_playbackUserControl.Close();
			_imageViewerControl.Disconnect();
			_imageViewerControl.Close();
			ClientControl.Instance.ReleasePlaybackController(_playbackFQID);
			RemoveApplicationEventListeners();
		}

		#endregion

		/// <summary>
		/// Do not show the sliding toolbar!
		/// </summary>
		public override bool ShowToolbar
		{
			get { return false; }
		}

		#region Component events

		private void ImageViewerControlClickEvent(object sender, EventArgs e)
		{
			FireClickEvent();
		}


		private void ViewItemUserControlMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				FireClickEvent();
			}
			else if (e.Button == MouseButtons.Right)
			{
				FireRightClickEvent(e);
			}
		}

		private void ViewItemUserControlMouseDoubleClick(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
		}


		/// <summary>
		/// Signals that the form is right clicked
		/// </summary>
		public event EventHandler RightClickEvent;

		/// <summary>
		/// Activates the RightClickEvent
		/// </summary>
		/// <param name="e">Event args</param>
		protected virtual void FireRightClickEvent(EventArgs e)
		{
			if (RightClickEvent != null)
			{
				RightClickEvent(this, e);
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

			_playbackUserControl.SetCameras(new List<FQID>() { _viewItemManager.SelectedCamera.FQID });
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

		#endregion


	}
}
