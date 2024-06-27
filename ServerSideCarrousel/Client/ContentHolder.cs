using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ServerSideCarrousel.Admin;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ServerSideCarrousel.Client
{
	public class ContentHolderToDELETE //: ContentHolderBase
	{
		/*
		#region Component private class variables

		private CarrouselViewItemUserControl _uiComponentForm;
		private CarrouselViewItemManager _carrouselViewItemManager;
		private ImageViewerControl _imageViewerFront;

		private CarrouselItem _cameraDeviceFront;
		private bool _isPlayingForward = true;
		private bool _pause = false;
		private int _currentIndex = -1;
		// private static int _infiniteTimerInterval = 999999;

		/// <summary>
		/// Timer that trigger new camera load
		/// </summary>
		protected System.Windows.Forms.Timer _loadCameraTimer = new System.Windows.Forms.Timer();


		#endregion

		#region Component Initialization & Dispose

		public ContentHolder(CarrouselViewItemManager carrouselViewItemManager, UserControl contentHolderForm)
		{
			_uiComponentForm = (CarrouselViewItemUserControl)contentHolderForm;
			_carrouselViewItemManager = carrouselViewItemManager;

			SetUpApplicationEventListeners();

			_uiComponentForm.InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;

			_imageViewerFront = ClientControl.Instance.GenerateImageViewerControl();
			_imageViewerFront.EnableBrowseMode = false;
			_imageViewerFront.EnableSetupMode = false;
			_imageViewerFront.ReceiveLiveInformationStatePackages = false;
			_imageViewerFront.SuppressUpdateOnMotionOnly = true;
			_imageViewerFront.EnableRecordedImageDisplayedEvent = false;
			_imageViewerFront.EnableSmartSearch = false;
			_imageViewerFront.EnableMouseControlledPtz = false;
			_imageViewerFront.EnableScrollWheel = false;
			_imageViewerFront.EnableMiddleButtonClick = false;

			((CarrouselViewItemUserControl)_uiComponentForm).SetImageViewerControl(_imageViewerFront.Panel);

			_imageViewerFront.Selected = _selected;
			_imageViewerFront.Hidden = _hidden;
			_imageViewerFront.CameraId = Guid.Empty;

			// init timers
			_loadCameraTimer.Interval = ((CarrouselViewItemManager)carrouselViewItemManager).DefaultTime * 1000;

			// set up timer event listeners
			_loadCameraTimer.Tick += new EventHandler(_loadCameraTimer_Tick);

			_carrouselViewItemManager.PropertyChangedEvent += new EventHandler(_viewItem_PropertyChangedEvent);

			_uiComponentForm.ClickEvent += new EventHandler(ContentHolder_ClickEvent);
			_uiComponentForm.DoubleClickEvent += new EventHandler(ContentHolder_DoubleClickEvent);

		}

		#endregion

		#region Component events


		void ContentHolder_ClickEvent(object sender, EventArgs e)
		{
			FireClickEvent();
		}

		void ContentHolder_DoubleClickEvent(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
		}

		void _viewItem_PropertyChangedEvent(object sender, EventArgs e)
		{
		}

		void CarouselContentHolder_Pause(object sender, EventArgs e)
        {
            CarrouselViewItemUserControl contentHolder = sender as CarrouselViewItemUserControl;
            if (contentHolder != null)
            {
                this.Pause = contentHolder.Paused;
            }
        }

        void CarouselContentHolder_Previous(object sender, EventArgs e)
        {
            if (((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count <= 1)
            {
                return;
            }

            this.ShowPrevious();
        }

        void CarouselContentHolder_Next(object sender, EventArgs e)
        {
            if (((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count <= 1)
            {
                return;
            }

            this.ShowNext();
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
        /// Overrides property (get) in ContentHolderBase. Carrousel content holders are newer selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Overrides property (set) in ContentHolderBase. First the Base implementaion is called and then the selected state of the image viewer 
        /// is updated If the image viewer is initialized (connected) the the content holder is selectable. See the ContentHolderBase documentation for more information.
        /// </summary>
        public override bool Selected
        {
            set
            {
                base.Selected = value;
                _imageViewerFront.Selected = value;
            }
        }

        /// <summary>
        /// Overrides property (set) in ContentHolderBase. First the Base implementaion is called and then image viewer image quality is updated. 
        /// When maximized the image quality should always be forced to full quality. See the ContentHolderBase documentation for more information.
        /// </summary>
        public override bool Maximized
        {
            set
            {
                if (base.Maximized != value)
                {
                    base.Maximized = value;
                    _imageViewerFront.Maximized = value;
                }
            }
        }

        /// <summary>
        /// Overrides property (set) in ContentHolderBase. First the Base implementaion is called. If the application is in live mode then image 
        /// viewer live is started or stopped according to the hidden state. See the ContentHolderBase documentation for more information.
        /// </summary>
        public override bool Hidden
        {
            set
            {
                if (_hidden != value)
                {
                    base.Hidden = value;

                    _imageViewerFront.Hidden = value;
                }
            }
        }

		#endregion

		#region Component private functions

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

        #region Component event handlers

        /// <summary>
        /// Tick to handle camera load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _loadCameraTimer_Tick(object sender, EventArgs e)
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


        private void CarrouselContentHolder_ClickEvent(object sender, EventArgs e)
        {
            FireClickEvent();
            UpdateAudio();
        }

        private void CarrouselContentHolder_DoubleClickEvent(object sender, EventArgs e)
        {
            FireDoubleClickEvent();
            UpdateAudio();
        }

        private void CarrouselContentHolder_RightClickEvent(object sender, EventArgs e)
        {
            FireClickEvent();
            ShowContextMenu(_uiComponentForm.PointToClient(Control.MousePosition));
        }

        private void _imageViewer_ImageViewerClickEvent(object sender, EventArgs e)
        {
            CarrouselContentHolder_ClickEvent(sender, e);
        }

        private void _imageViewer_ImageViewerDoubleClickEvent(object sender, EventArgs e)
        {
            CarrouselContentHolder_DoubleClickEvent(sender, e);
        }

        private void _imageViewer_ImageViewerRightClickEvent(object sender, EventArgs e)
        {
            CarrouselContentHolder_RightClickEvent(sender, e);
        }

        private void ParentForm_Closing(object sender, CancelEventArgs e)
        {
            ImageViewerDisconnect();
            ImageViewerClose();
        }

        private void _imageViewer_ImageViewerConnectResponseReceivedEvent(object sender, ConnectResponseEventEventArgs e)
        {
            //return now if connection to camera failed
            if (e.bConnectionGranted == 0)
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

        private void ApplicationController_ApplicationModeChangedEvent(object sender, EventArgs e)
        {
            _uiComponentForm.Paused = false;

            switch (EnvironmentManager.Instance.Mode)
            {
				case Mode.ClientLive:
                    ImageViewerInit();
                    StartLive();
                    _uiComponentForm.InLive = true;
                    StartTimer();
                    break;
				case Mode.ClientPlayback:
                    StopTimer();
                    ShowNoCarruoselCamera();
                    StartBrowse();
                    _uiComponentForm.InLive = false;
                    break;
				case Mode.ClientSetup:
                    StopTimer();
                    ShowNoCarruoselCamera();
                    StartSetup();
                    _uiComponentForm.InLive = false;
                    break;
            }
        }

        private void viewItem_PropertyChangedEvent(object sender, EventArgs e)
        {
            if (_imageViewerFront != null)
            {
                _imageViewerFront.UpdateStates();
            }
        }


        #endregion

        #region Component public methods

        /// <summary>
        /// Overrides method defined in ContentHolderBase. Initializis the camera  content holder. See the ContentHolderBase documentation 
        /// for more information.
        /// </summary>
        public override void Init()
        {
            ImageViewerInit();

            SetUpImageViewerEventListeners(_imageViewerFront);

            //Listen to form close events so the imageviewer activeX can be close gracefully if the application is closed
            _uiComponentForm.ParentForm.Closing += new CancelEventHandler(ParentForm_Closing);
            
            UpdateInfoTextIfNessesary();
            _imageViewerFront.ShowImageViewer = false;

            //if live mode then start the carrousel timer
            if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
            {
                StartTimer();
            }
        }

        /// <summary>
        /// Overrides method defined in ContentHolderBase. Disconnects and closes the ImageViewer. See the ContentHolderBase documentation 
        /// for more information.
        /// </summary>
        public override void Close()
        {
            StopTimer();

            //remove form close event listener to avoid memory leak (DTS:3912)
            if (_uiComponentForm.ParentForm != null)
            {
                _uiComponentForm.ParentForm.Closing -= new CancelEventHandler(ParentForm_Closing);
            }

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

        #endregion

        #region Component protected implementation

        private void UpdateInfoTextIfNessesary()
        {
            string infoText = String.Empty;
            if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
            {
                if (((CarrouselViewItemManager)_carrouselViewItemManager).Items == null || ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count == 0)
                {
                    infoText = _uiComponentForm.StringResourceManager.GetString("NoCarrouselCameras");
                }
                else if (_cameraDeviceFront == null ||
                         (_currentIndex != -1 && _currentIndex < ((CarrouselViewItemManager)_carrouselViewItemManager).Items.Count &&
                         (((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex] == null )))
                {
                    infoText = _uiComponentForm.StringResourceManager.GetString("CameraNotAvailable");
                }
            }
            else
            {
                infoText = _uiComponentForm.StringResourceManager.GetString("CarrouselOnlyActiveInLiveMode");
            }

            if (infoText != String.Empty)
            {
                _imageViewerFront.InfoText = infoText;
            }
        }

        private void ShowNoCarruoselCamera()
        {
            ImageViewerDisconnect();
            ImageViewerClose();

            ((CarrouselViewItemManager)_carrouselViewItemManager).CameraId = Guid.Empty;
        }

        private void SetUpApplicationEventListeners()
        {
            //set up CarouselViewItem event listeners
            _carrouselViewItemManager.PropertyChangedEvent += new EventHandler(viewItem_PropertyChangedEvent);

            //set up ApplicationController events
        	EnvironmentManager.Instance.ModeChanged += new EventHandler(ApplicationController_ApplicationModeChangedEvent);

            //set up CarouselContentHolderForm event listeners
            ((CarrouselViewItemUserControl)_uiComponentForm).ClickEvent += new EventHandler(CarrouselContentHolder_ClickEvent);
            ((CarrouselViewItemUserControl)_uiComponentForm).DoubleClickEvent += new EventHandler(CarrouselContentHolder_DoubleClickEvent);
            ((CarrouselViewItemUserControl)_uiComponentForm).RightClickEvent += new EventHandler(CarrouselContentHolder_RightClickEvent);
            ((CarrouselViewItemUserControl)_uiComponentForm).Next += new EventHandler(CarouselContentHolder_Next);
            ((CarrouselViewItemUserControl)_uiComponentForm).Previous += new EventHandler(CarouselContentHolder_Previous);
            ((CarrouselViewItemUserControl)_uiComponentForm).Pause += new EventHandler(CarouselContentHolder_Pause);
        }

        private void RemoveApplicationEventListeners()
        {
            //remove CarouselViewItem event listeners
            _carrouselViewItemManager.PropertyChangedEvent -= new EventHandler(viewItem_PropertyChangedEvent);

            //remove ApplicationController events
			EnvironmentManager.Instance.ModeChanged -= new EventHandler(ApplicationController_ApplicationModeChangedEvent);

            //remove up CarouselContentHolderForm event listeners
            ((CarrouselViewItemUserControl)_uiComponentForm).ClickEvent -= new EventHandler(CarrouselContentHolder_ClickEvent);
            ((CarrouselViewItemUserControl)_uiComponentForm).DoubleClickEvent -= new EventHandler(CarrouselContentHolder_DoubleClickEvent);
            ((CarrouselViewItemUserControl)_uiComponentForm).RightClickEvent -= new EventHandler(CarrouselContentHolder_RightClickEvent);
            ((CarrouselViewItemUserControl)_uiComponentForm).Next -= new EventHandler(CarouselContentHolder_Next);
            ((CarrouselViewItemUserControl)_uiComponentForm).Previous -= new EventHandler(CarouselContentHolder_Previous);
            ((CarrouselViewItemUserControl)_uiComponentForm).Pause -= new EventHandler(CarouselContentHolder_Pause);

			_carrouselViewItemManager.PropertyChangedEvent -= new EventHandler(_viewItem_PropertyChangedEvent);
			_uiComponentForm.ClickEvent -= new EventHandler(ContentHolder_ClickEvent);
			_uiComponentForm.DoubleClickEvent -= new EventHandler(ContentHolder_DoubleClickEvent);

        }

        /// <summary>
        /// Build and show the camera context menu.
        /// </summary>
        /// <param name="position">The desired menu position</param>
        private void ShowContextMenu(Point position)
        {
            //force focus away from image viewer ActiveX when ever the context menu is shown. 
            _uiComponentForm.Focus();

            //do not show context menu in setup mode
            if (EnvironmentManager.Instance.Mode == Mode.ClientSetup)
            {
                return;
            }

            //do not show context menu if not connected to a device
            if (_imageViewerFront == null)
            {
                return;
            }

			_uiComponentForm.copyToolStripMenuItem.Click += new EventHandler(ContextMenuCopyToClipboard_Click);
			_uiComponentForm.contextMenuStripcopy.Show(_uiComponentForm, position);
        }

		private void ContextMenuCopyToClipboard_Click(object sender, EventArgs e)
		{
			if (_imageViewerFront != null)
			{
				object rgbImageData = null;
				Int64 rgbImageMilliseconds = 0;
				short rgbImageWidth = 0;
				short rgbImageHeight = 0;
				short rgbImageStride = 0;

				bool dispErrorMsg = false;
				short nResult =
					_imageViewerFront.GetCurrentDisplayedImageAsBitmap2(2, 1, ref rgbImageData, ref rgbImageMilliseconds,
																  ref rgbImageWidth, ref rgbImageHeight,
																  ref rgbImageStride); // Get as 24-bit BGR
				if (nResult != 0)
				{
					try
					{
						GCHandle imageDataHandle = GCHandle.Alloc(rgbImageData, GCHandleType.Pinned);
						Bitmap imageBitmap =
							new Bitmap(rgbImageWidth, rgbImageHeight, rgbImageStride, PixelFormat.Format24bppRgb,
									   imageDataHandle.AddrOfPinnedObject());
						// despite the pixel format name, GDI+ always assumes 24-bit RGB!!
						Clipboard.SetImage(imageBitmap);
						imageDataHandle.Free();
					}
					catch (Exception)
					{
						dispErrorMsg = true;
					}
				}

				if (nResult == 0 || dispErrorMsg)
				{
					MessageBox.Show(_uiComponentForm.StringResourceManager.GetString("FailedToCopyImageFromActiveXText"),
									_uiComponentForm.StringResourceManager.GetString("FailedToCopyImageFromActiveXHeadline"),
									MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

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

            _imageViewerFront.ShowImageViewer = false;
        }

        /// <summary>
        /// Puts the camera into setup mode
        /// </summary>
        protected void StartSetup()
        {
            StopTimer();
            _currentIndex = -1;

            UpdateInfoTextIfNessesary();

            _imageViewerFront.ShowImageViewer = false;
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

            imageViewer.Create();

            if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
            {
                imageViewer.Initialize();
            }
            else
            {
                imageViewer.InitializeNoEventRegistering();
            }
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
                CarrouselItem item = ((CarrouselViewItemManager)_carrouselViewItemManager).Items[index];
                if (item != null && item.Instance.FQId.InstanceType == InstanceType.Camera) 
                {
                    _cameraDeviceFront = item;
                	_imageViewerFront.CameraId = item.Instance.FQId.ObjectId;	
                    ImageViewerInit(_imageViewerFront);
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

            imageViewer.InitializeNoEventRegistering();
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
            ImageViewerClose(_imageViewerFront);

            _imageViewerFront.Dispose();
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
        /// Disables all callbacks and disconnects the image viewer from the camera
        /// </summary>
        protected void ImageViewerDisconnect(ImageViewerControl imageViewer)
        {
            if (imageViewer == null)
            {
                return;
            }
            imageViewer.Disconnect();
        }

        /// <summary>
        /// Disconnects both the front and the back ImageViewer.
        /// </summary>
        protected void ImageViewerDisconnect()
        {
            try
            {
                _imageViewerFront.Disconnect();
            }
            catch (Exception)
            {
                // Ignore C++ exceptions during Disconnect - DTS4713
            }
        }

        private void SetUpImageViewerEventListeners(ImageViewerControl imageViewer)
        {
            //set up imageViewerActiveX event listeners
            imageViewer.ClickEvent += new EventHandler(_imageViewer_ImageViewerClickEvent);
            imageViewer.DoubleClickEvent += new EventHandler(_imageViewer_ImageViewerDoubleClickEvent);
            imageViewer.RightClickEvent += new EventHandler(_imageViewer_ImageViewerRightClickEvent);
            imageViewer.ConnectResponseEvent += new ConnectResponseEventHandler(_imageViewer_ImageViewerConnectResponseReceivedEvent);
        }

        private void RemoveImageViewerEventListeners(ImageViewerControl imageViewer)
        {
            //set up imageViewerActiveX event listeners
            imageViewer.ClickEvent -= new EventHandler(_imageViewer_ImageViewerClickEvent);
            imageViewer.DoubleClickEvent -= new EventHandler(_imageViewer_ImageViewerDoubleClickEvent);
            imageViewer.RightClickEvent -= new EventHandler(_imageViewer_ImageViewerRightClickEvent);
			imageViewer.ConnectResponseEvent -= new ConnectResponseEventHandler(_imageViewer_ImageViewerConnectResponseReceivedEvent);
        }

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

        private void Show(CarrouselItem item)
        {
            if (item != null)
            {
				Load(item);

                _imageViewerFront.Focus();
                UpdateAudio();
            }
            UpdateInfoTextIfNessesary();
       }

        private void UpdateAudio()
        {
            if (_currentIndex > -1)
            {
                CarrouselItem item = ((CarrouselViewItemManager)_carrouselViewItemManager).Items[_currentIndex];
                if (item != null /*&& _cameraNames.ContainsKey(item.Camera.ToString())* / && Selected)
                {
                    //Camera camera = _cameraNames[item.Camera.ToString()];
                    //TODO ? _controllerManager.ApplicationController.CameraChangedNotification(camera);
                }
            }
        }

        private void Load(CarrouselItem item)
        {
			_cameraDeviceFront = item;
			if (item.Instance.FQId.InstanceType == InstanceType.Camera)
			{
				ImageViewerDisconnect(_imageViewerFront);
				_imageViewerFront.CameraId = item.Instance.FQId.ObjectId;
				_imageViewerFront.ShowImageViewer = true;

				ImageViewerInitNoEvents(_imageViewerFront);
				ImageViewerConnect(_imageViewerFront);
			} else
			if (item.Instance.FQId.InstanceType == InstanceType.Preset)
			{
				if (_imageViewerFront.CameraId != item.Instance.FQId.ParentId)
				{
					ImageViewerDisconnect(_imageViewerFront);
					_imageViewerFront.CameraId = item.Instance.FQId.ParentId;
					_imageViewerFront.ShowImageViewer = true;

					ImageViewerInitNoEvents(_imageViewerFront);
					ImageViewerConnect(_imageViewerFront);					
				}
				EnvironmentManager.Instance.TriggerEvent(null, item.Instance.FQId, "Carrousel", null, DateTime.Now, null);
			}
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void StartTimer()
        {
            _currentIndex = PreviousIndex;
            ShowNext();

            _isPlayingForward = true;

            if (_currentIndex != -1)
            {
                ////_showCameraTimer.Start();
                _loadCameraTimer.Start();
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        private void StopTimer()
        {
            if (_currentIndex != -1)
            {
                ////_showCameraTimer.Stop();
                _loadCameraTimer.Stop();
            }
        }
        #endregion
*/
    }

}
