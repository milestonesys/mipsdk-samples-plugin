using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ServerSideCarrousel.Admin;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace ServerSideCarrousel.Client
{
    public partial class CarrouselViewItemUserControl : ViewItemUserControl
    {
		#region Component private class variables

		private Panel _toolbarPanel;
        private Timer _hideToolPanelTimer;
        private Button _nextButton;
        private CheckBox _pauseCheckBox;
        private Button _previousButton;
        private Panel _imageViewerControlPanel;
        private bool _inLive = true;
		private ResourceManager _stringResourceManager;

		private CarrouselViewItemManager _carrouselViewItemManager;
		private ImageViewerControl _imageViewerControl;

		private CarrouselTreeNode _cameraDeviceFront;
		private bool _isPlayingForward = true;
		private bool _pause = false;
		private int _currentIndex = -1;

    	private object _messageReference;

		/// <summary>
		/// Timer that trigger new camera load
		/// </summary>
		protected System.Windows.Forms.Timer _loadCameraTimer = new System.Windows.Forms.Timer();

		#endregion

        #region Component constructors + dispose

        /// <summary>
		/// Constructs a CarrouselViewItemUserControl item
        /// </summary>
		public CarrouselViewItemUserControl(CarrouselViewItemManager carrouselViewItemManager)
        {
        	InitializeComponent();

			_carrouselViewItemManager = carrouselViewItemManager;
        	Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
			_stringResourceManager = new System.Resources.ResourceManager(assembly.GetName().Name + ".Resources.Strings", assembly);
			this.copyToolStripMenuItem.Image =  new Bitmap(Image.FromStream(assembly.GetManifestResourceStream(name + ".Resources.Copy.png")));

			SetUpApplicationEventListeners();

			InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;

			_imageViewerControl = ClientControl.Instance.GenerateImageViewerControl();
			_imageViewerControl.EnableBrowseMode = false;
			_imageViewerControl.EnableSetupMode = false;
			_imageViewerControl.SuppressUpdateOnMotionOnly = true;
			_imageViewerControl.EnableRecordedImageDisplayedEvent = false;
			_imageViewerControl.EnableSmartSearch = false;
			_imageViewerControl.EnableMouseControlledPtz = false;
			_imageViewerControl.EnableScrollWheel = false;
			_imageViewerControl.EnableMiddleButtonClick = false;

			_imageViewerControl.Dock = DockStyle.Fill;
			_imageViewerControlPanel.Controls.Add(_imageViewerControl);

			_imageViewerControl.Selected = _selected;
			_imageViewerControl.Hidden = _hidden;

            _imageViewerControl.MouseHoverEvent += _imageViewerControl_MouseHoverEvent;
            _imageViewerControl.MouseLeaveEvent += _imageViewerControl_MouseLeaveEvent;
            _imageViewerControl.MouseMoveEvent += _imageViewerControl_MouseMoveEvent;
            _imageViewerControl.MouseScrollWheelEvent += _imageViewerControl_MouseScrollWheelEvent;

			// init timers
			_loadCameraTimer.Interval = ((CarrouselViewItemManager)carrouselViewItemManager).DefaultTime * 1000;

			// set up timer event listeners
			_loadCameraTimer.Tick += new EventHandler(LoadCameraTimer_Tick);
		}

        private void _imageViewerControl_MouseScrollWheelEvent(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("_imageViewerControl_MouseScrollWheelEvent");
        }

        private void _imageViewerControl_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("_imageViewerControl_MouseMoveEvent");
        }

        private void _imageViewerControl_MouseLeaveEvent(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("_imageViewerControl_MouseLeaveEvent");
        }

        private void _imageViewerControl_MouseHoverEvent(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("_imageViewerControl_MouseHoverEvent");
        }

        /// <summary>
        /// Overrides method defined in ViewItemUserControl. 
        /// </summary>
        public override void Init()
		{
			ImageViewerInit();

			SetUpImageViewerEventListeners(_imageViewerControl);

			//Listen to form close events so the imageviewer activeX can be close gracefully if the application is closed
			//ParentForm.Closing += new CancelEventHandler(ParentForm_Closing);

			UpdateInfoTextIfNessesary();
			_imageViewerControl.ShowImageViewer = false;

			//if live mode then start the carrousel timer
			if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
			{
				StartTimer();
			}
		}

		/// <summary>
		/// Overrides method defined in ViewItemUserControl. Disconnects and closes the ImageViewer. 
		/// </summary>
		public override void Close()
		{
			StopTimer();

			//remove form close event listener to avoid memory leak (DTS:3912)
			if (ParentForm != null)
			{
				ParentForm.Closing -= new CancelEventHandler(ParentForm_Closing);
			}

			RemoveImageViewerEventListeners(_imageViewerControl);
			RemoveApplicationEventListeners();

			ImageViewerDisconnect();
			ImageViewerClose();

			if (_loadCameraTimer != null)
			{
				_loadCameraTimer.Stop();
				_loadCameraTimer.Dispose();
				_loadCameraTimer = null;
			}
		}

		private void SetUpApplicationEventListeners()
		{
			_carrouselViewItemManager.PropertyChangedEvent += new EventHandler(viewItem_PropertyChangedEvent);

			//set up ApplicationController events
			_messageReference = EnvironmentManager.Instance.RegisterReceiver(ModeChangedHandler, new MessageIdFilter(MessageId.System.ModeChangedIndication));
		}

		private void RemoveApplicationEventListeners()
		{
			_carrouselViewItemManager.PropertyChangedEvent -= new EventHandler(viewItem_PropertyChangedEvent);

            //remove ApplicationController events
            EnvironmentManager.Instance.UnRegisterReceiver(_messageReference);
		}

		/// <summary>
		/// Do not show the sliding toolbar!
		/// </summary>
		public override bool ShowToolbar
		{
			get { return true; }
		}


        #endregion

        #region Component events

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

        #endregion

        #region Component private properties


		private bool InLive
		{
			get { return _inLive; }
			set { _inLive = value; }
		}

        private bool Paused
        {
            set
            {
                _pauseCheckBox.Checked = value;
            }
            get
            {
                return _pauseCheckBox.Checked;
            }
        }

        #endregion

		#region Component properties


		public override bool Maximizable
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Overrides property (get) in ViewItemUserControl. 
		/// </summary>
		public override bool Selectable
		{
			get { return true; }
		}

		/// <summary>
		/// Overrides property (set) in ViewItemUserControl. First the Base implementaion is called and then the selected state of the image viewer 
		/// is updated If the image viewer is initialized (connected) the the content holder is selectable. 
		/// </summary>
		public override bool Selected
		{
			set
			{
				base.Selected = value;
				_imageViewerControl.Selected = value;
			}
		}

		/// <summary>
		/// Overrides property (set) in ViewItemUserControl. First the Base implementaion is called and then image viewer image quality is updated. 
		/// When maximized the image quality should always be forced to full quality. 
		/// </summary>
		public override bool Maximized
		{
			set
			{
				if (base.Maximized != value)
				{
					base.Maximized = value;
					_imageViewerControl.Maximized = value;
				}
			}
		}

		/// <summary>
		/// Overrides property (set) in ViewItemUserControl. First the Base implementaion is called. If the application is in live mode then image 
		/// viewer live is started or stopped according to the hidden state. 
		/// </summary>
		public override bool Hidden
		{
			set
			{
				if (_hidden != value)
				{
					base.Hidden = value;

					_imageViewerControl.Hidden = value;
				}
			}
		}

		#endregion

		#region Tool Panel management
		private void CarouselViewItemUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("CarouselViewItemUserControl_MouseMove");
            ShowToolPanel();
        }

        private void ShowToolPanel()
        {
			if (InLive)
            {
                _toolbarPanel.Visible = true;
                RestartHideToolPanelTimer();
            }
            else
            {
                _toolbarPanel.Visible = false;
            }
        }

        private void RestartHideToolPanelTimer()
        {
            _hideToolPanelTimer.Interval = 1000;
            _hideToolPanelTimer.Start();
        }

        private void _hideToolPanelTimer_Tick(object sender, EventArgs e)
        {
            _toolbarPanel.Visible = false;
            _hideToolPanelTimer.Stop();
        }

        private void _toolbarPanel_MouseEnter(object sender, EventArgs e)
        {
            _hideToolPanelTimer.Stop();
		}

        private void _toolbarPanel_MouseLeave(object sender, EventArgs e)
        {
            RestartHideToolPanelTimer();
		}

		#endregion

		#region Component event handlers

        private void PauseCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
        	Pause = _pauseCheckBox.Checked;
        }

		/// <summary>
		/// Tick to handle camera load
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void LoadCameraTimer_Tick(object sender, EventArgs e)
		{
			if (_isPlayingForward)
			{
				ShowNext();
			}
			else
			{
				ShowPrevious();
			}
		}

		private void ImageViewer_ImageViewerClickEvent(object sender, EventArgs e)
		{
			FireClickEvent();
			UpdateAudio();
		}

		private void ImageViewer_ImageViewerDoubleClickEvent(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
			UpdateAudio();
		}

		private void ImageViewer_ImageViewerRightClickEvent(object sender, EventArgs e)
		{
            FireClickEvent();
            ShowContextMenu(PointToClient(Control.MousePosition));
        }

        private void ParentForm_Closing(object sender, CancelEventArgs e)
		{
			ImageViewerDisconnect();
			ImageViewerClose();
		}

		private void ImageViewer_ImageViewerConnectResponseReceivedEvent(object sender, ConnectResponseEventEventArgs e)
		{
			//return now if connection to camera failed
			if (e.ConnectionGranted == 0)
			{
				return;
			}

			switch (EnvironmentManager.Instance.Mode)
			{
				case Mode.ClientLive:
					StartLive();
					break;
				case Mode.ClientPlayback:
					StartBrowse();
					break;
				case Mode.ClientSetup:
					StartSetup();
					break;
			}
		}

		private object ModeChangedHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
		{
			Paused = false;

			switch (EnvironmentManager.Instance.Mode)
			{
				case Mode.ClientLive:
					ImageViewerInit();
					StartLive();
					InLive = true;
					StartTimer();
					break;
				case Mode.ClientPlayback:
					StopTimer();
					ShowNoCarruoselCamera();
					StartBrowse();
					InLive = false;
					break;
				case Mode.ClientSetup:
					StopTimer();
					ShowNoCarruoselCamera();
					StartSetup();
					InLive = false;
					break;
			}
			return null;
		}

		private void viewItem_PropertyChangedEvent(object sender, EventArgs e)
		{
			if (_imageViewerControl != null)
			{
				_imageViewerControl.UpdateStates();
                if (_loadCameraTimer.Enabled == false)
                    ShowNext();
			}
		}

        void CarouselViewItemUserControl_Previous(object sender, EventArgs e)
        {
            if (_carrouselViewItemManager.Items.Count <= 1)
            {
                return;
            }

            this.ShowPrevious();
        }

        void CarouselViewItemUserControl_Next(object sender, EventArgs e)
        {
            if (_carrouselViewItemManager.Items.Count <= 1)
            {
                return;
            }

            this.ShowNext();
        }

        #endregion

		#region Component private functions


		#region Carrousel item control methods
		/// <summary>
		/// Shows the next.
		/// </summary>
		private void ShowNext()
		{
			if (NextIndex != -1)
			{
				_currentIndex = NextIndex;
				_isPlayingForward = true;

				bool loadCameraTimerRunning = _loadCameraTimer.Enabled;
				_loadCameraTimer.Stop();
				_loadCameraTimer.Interval = 1000 * ((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex].Seconds;
				if (loadCameraTimerRunning)
				{
					_loadCameraTimer.Start();
				}
				Show(((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex]);
			}
		}

		/// <summary>
		/// Shows the previous.
		/// </summary>
		private void ShowPrevious()
		{
			if (PreviousIndex != -1)
			{
				_currentIndex = PreviousIndex;
				_isPlayingForward = false;

				bool loadCameraTimerRunning = _loadCameraTimer.Enabled;
				_loadCameraTimer.Stop();
				_loadCameraTimer.Interval = 1000 * ((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex].Seconds;
				if (loadCameraTimerRunning)
				{
					_loadCameraTimer.Start();
				}
				Show(((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex]);
			}
		}

		private void Show(CarrouselTreeNode carrouselTreeNode)
		{
			if (carrouselTreeNode != null)
			{
				LoadAndShow(carrouselTreeNode);

				_imageViewerControl.Focus();
				UpdateAudio();
			}
			UpdateInfoTextIfNessesary();
		}

		private void UpdateAudio()
		{
			if (_currentIndex > -1)
			{
				CarrouselTreeNode carrouselTreeNode = ((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex];
				if (carrouselTreeNode != null /*&& _cameraNames.ContainsKey(item.Camera.ToString())*/ && Selected)
				{
					//Camera camera = _cameraNames[item.Camera.ToString()];
					//TODO ? _controllerManager.ApplicationController.CameraChangedNotification(camera);
				}
			}
		}

		private void LoadAndShow(CarrouselTreeNode carrouselTreeNode)
		{
			_cameraDeviceFront = carrouselTreeNode;
			if (carrouselTreeNode.Item.FQID.Kind == Kind.Camera)
			{
				ImageViewerDisconnect();
				_imageViewerControl.CameraFQID = carrouselTreeNode.Item.FQID;
				_imageViewerControl.ShowImageViewer = true;

				ImageViewerInitNoEvents(_imageViewerControl);
				ImageViewerConnect(_imageViewerControl);
			}
			else
				if (carrouselTreeNode.Item.FQID.Kind == Kind.Preset)
				{
					if (_imageViewerControl.CameraFQID == null || _imageViewerControl.CameraFQID.ObjectId != carrouselTreeNode.Item.FQID.ParentId)
					{
						ImageViewerDisconnect();
						_imageViewerControl.CameraFQID = carrouselTreeNode.Item.FQID.GetParent();
						_imageViewerControl.ShowImageViewer = true;

						ImageViewerInitNoEvents(_imageViewerControl);
						ImageViewerConnect(_imageViewerControl);
					}
					EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.TriggerCommand),
					                                        carrouselTreeNode.Item.FQID, null);
				}
		}

		/// <summary>
		/// Starts this item.
		/// </summary>
		private void StartTimer()
		{
			_currentIndex = PreviousIndex;
			ShowNext();

			_isPlayingForward = true;

			if (_currentIndex != -1)
			{
                _loadCameraTimer.Enabled = true;
				_loadCameraTimer.Start();
			}
		}

		/// <summary>
		/// Stops this item.
		/// </summary>
		private void StopTimer()
		{
			if (_currentIndex != -1)
			{
				_loadCameraTimer.Stop();
			}
		}
		#endregion

		#region private properties

		/// <summary>
        /// Gets or sets a value indicating whether this is pause.
        /// </summary>
        /// <value><c>true</c> if pause; otherwise, <c>false</c>.</value>
        private bool Pause
        {
            set
            {
                // only handle pause in Live mode
				if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
                {
                    if (_pause != value)
                    {
                        _pause = value;
                        if (_pause)
                        {
                            StopTimer();
                        }
                        else
                        {
                            StartTimer();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the index of the next.
        /// </summary>
        /// <value>The index of the next.</value>
        private int NextIndex
        {
            get
            {
                if (((CarrouselViewItemManager)_carrouselViewItemManager).Items == null || ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count == 0)
                {
                    return -1;
                }

                if (_currentIndex + 1 >= ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count)
                {
                    return 0;
                }

                return _currentIndex + 1;
            }
        }

        /// <summary>
        /// Gets the index of the previous.
        /// </summary>
        /// <value>The index of the previous.</value>
        private int PreviousIndex
        {
            get
            {
                if (((CarrouselViewItemManager)_carrouselViewItemManager).Items == null || ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count == 0)
                {
                    return -1;
                }
                if (_currentIndex <= 0)
                {
                    return ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count - 1;
                }
                return _currentIndex - 1;
            }
		}

		#endregion

		private void UpdateInfoTextIfNessesary()
		{
			string infoText = String.Empty;
			if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
			{
				if (((CarrouselViewItemManager)_carrouselViewItemManager).Items == null || ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count == 0)
				{
					infoText = _stringResourceManager.GetString("NoCarrouselCameras");
				}
				else if (_cameraDeviceFront == null ||
						 (_currentIndex != -1 && _currentIndex < ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count &&
						 (((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex] == null)))
				{
					infoText = _stringResourceManager.GetString("CameraNotAvailable");
				}
			}
			else
			{
				infoText = _stringResourceManager.GetString("CarrouselOnlyActiveInLiveMode");
			}

			if (infoText != String.Empty)
			{
				_imageViewerControl.InfoText = infoText;
			}
		}

		private void ShowNoCarruoselCamera()
		{
			ImageViewerDisconnect();
		}

		/// <summary>
		/// Build and show the camera context menu.
		/// </summary>
		/// <param name="position">The desired menu position</param>
		private void ShowContextMenu(Point position)
		{
			//force focus away from image viewer ActiveX when ever the context menu is shown. 
			Focus();

			//do not show context menu in setup mode
			if (EnvironmentManager.Instance.Mode == Mode.ClientSetup)
			{
				return;
			}

			//do not show context menu if not connected to a device
			if (_imageViewerControl == null)
			{
				return;
			}

			copyToolStripMenuItem.Click += new EventHandler(ContextMenuCopyToClipboard_Click);
			contextMenuStripcopy.Show(this, position);
		}

		private void ContextMenuCopyToClipboard_Click(object sender, EventArgs e)
		{
			if (_imageViewerControl != null)
			{
				Bitmap bitmap = _imageViewerControl.GetCurrentDisplayedImageAsBitmap();
				Clipboard.SetImage(bitmap);
			}
		}


		#endregion

		#region ImageView interface handling

		/// <summary>
        /// Puts the camera into live mode
        /// </summary>
        protected void StartLive()
        {
            UpdateInfoTextIfNessesary();
        }

        /// <summary>
        /// Puts the camera into browse mode
        /// </summary>
        protected void StartBrowse()
        {
            StopTimer();
            _currentIndex = -1;

            UpdateInfoTextIfNessesary();

            _imageViewerControl.ShowImageViewer = false;
        }

        /// <summary>
        /// Puts the camera into setup mode
        /// </summary>
        protected void StartSetup()
        {
            StopTimer();
            _currentIndex = -1;

            UpdateInfoTextIfNessesary();

            _imageViewerControl.ShowImageViewer = false;
        }

        /// <summary>
        /// Initializes the ImageViewer. Calling setup methods and sets up events listeners
        /// </summary>
        protected void ImageViewerInit(ImageViewerControl imageViewer)
        {
            if (imageViewer == null)
            {
                return;
            }

			imageViewer.Initialize();
        }

        /// <summary>
        /// Initializes ImageViewer
        /// </summary>
        protected void ImageViewerInit()
        {
            // determine initial camera
            int index;
            for (index = 0; index < ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count; index++)
            {
                CarrouselTreeNode carrouselTreeNode = ((CarrouselViewItemManager)_carrouselViewItemManager).Items[index];
                if (carrouselTreeNode != null && carrouselTreeNode.Item.FQID.Kind == Kind.Camera) 
                {
                    _cameraDeviceFront = carrouselTreeNode;
                	_imageViewerControl.CameraFQID = carrouselTreeNode.Item.FQID;
                    ImageViewerInit(_imageViewerControl);
                    break;
                }
            }
        }

        /// <summary>
        /// Initializes the ImageViewer without event registrering.
        /// </summary>
        protected void ImageViewerInitNoEvents(ImageViewerControl imageViewer)
        {
            if (imageViewer == null)
            {
                return;
            }
			imageViewer.Initialize();
		}

        /// <summary>
        /// Closes the image viewer. After the image viewer is closed the image viewer reference is set to null.
        /// </summary>
        protected void ImageViewerClose(ImageViewerControl imageViewer)
        {
            if (imageViewer == null)
            {
                return;
            }

            imageViewer.Close();
        }

        /// <summary>
        /// Closes both the front and the back image viewer.
        /// </summary>
        protected void ImageViewerClose()
        {
            ImageViewerClose(_imageViewerControl);

            _imageViewerControl.Dispose();
        }

        /// <summary>
        /// Connects the ImageViewer to a camera device
        /// </summary>
        /// <param name="device">The camera device that the ImageViewer shall connect to</param>
        protected void ImageViewerConnect(ImageViewerControl imageViewer)
        {
            if (imageViewer == null)
            {
                return;
            }

            imageViewer.Connect();
        }

        /// <summary>
        /// Disconnects the ImageViewer.
        /// </summary>
        protected void ImageViewerDisconnect()
        {
            try
            {
                _imageViewerControl.Disconnect();
            }
            catch (Exception)
            {
            }
        }

        private void SetUpImageViewerEventListeners(ImageViewerControl imageViewer)
        {
            //set up imageViewerActiveX event listeners
            imageViewer.ClickEvent += new EventHandler(ImageViewer_ImageViewerClickEvent);
            imageViewer.DoubleClickEvent += new EventHandler(ImageViewer_ImageViewerDoubleClickEvent);
            imageViewer.RightClickEvent += new EventHandler(ImageViewer_ImageViewerRightClickEvent);
            imageViewer.ConnectResponseEvent += new ConnectResponseEventHandler(ImageViewer_ImageViewerConnectResponseReceivedEvent);
        }

        private void RemoveImageViewerEventListeners(ImageViewerControl imageViewer)
        {
            //set up imageViewerActiveX event listeners
            imageViewer.ClickEvent -= new EventHandler(ImageViewer_ImageViewerClickEvent);
            imageViewer.DoubleClickEvent -= new EventHandler(ImageViewer_ImageViewerDoubleClickEvent);
            imageViewer.RightClickEvent -= new EventHandler(ImageViewer_ImageViewerRightClickEvent);
			imageViewer.ConnectResponseEvent -= new ConnectResponseEventHandler(ImageViewer_ImageViewerConnectResponseReceivedEvent);
		}

        #endregion

        private void MouseHoverHandler(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("MouseHover event ");
        }

        private void MouseLeaveHandler(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("MouseLeave event ");

        }
    }
}
