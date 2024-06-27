using ServerSideCarrousel.Admin;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace ServerSideCarrousel.Client
{
    public partial class CarrouselViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private bool _inLive = true;
        private ResourceManager _stringResourceManager;

        private CarrouselViewItemManager _carrouselViewItemManager;
        private ImageViewerWpfControl _imageViewerWpfControl;

        private CarrouselTreeNode _cameraDeviceFront;
        private bool _isPlayingForward = true;
        private bool _paused = false;
        private int _currentIndex = -1;

        private object _messageReference;


        /// <summary>
        /// Timer that trigger new camera load
        /// </summary>
        protected DispatcherTimer _loadCameraTimer = new DispatcherTimer();

        #endregion

        #region Component constructors + dispose

        public CarrouselViewItemWpfUserControl(CarrouselViewItemManager carrouselViewItemManager)
        {
            InitializeComponent();

            _carrouselViewItemManager = carrouselViewItemManager;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            _stringResourceManager = new ResourceManager(assembly.GetName().Name + ".Resources.Strings", assembly);
            SetUpApplicationEventListeners();

            InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;

            _imageViewerWpfControl =  new ImageViewerWpfControl();
            _imageViewerWpfControl.EnableBrowseMode = false;
            _imageViewerWpfControl.EnableSetupMode = false;
            _imageViewerWpfControl.SuppressUpdateOnMotionOnly = true;
            _imageViewerWpfControl.EnableMouseControlledPtz = false;


             imageViewerControlCanvas.Children.Add(_imageViewerWpfControl);

            _imageViewerWpfControl.Selected = Selected;
            _imageViewerWpfControl.Hidden = Hidden;

            _loadCameraTimer.Interval = new TimeSpan(0, 0, carrouselViewItemManager.DefaultTime);
            _loadCameraTimer.Tick += new EventHandler(LoadCameraTimer_Tick);
        }

        public override void Init()
        {
            ImageViewerInit();
            UpdateInfoTextIfNessesary();
            _imageViewerWpfControl.ShowImageViewer = false;

            //if live mode then start the carrousel timer
            if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
            {
                StartTimer();
            }
        }

        /// <summary>
        /// Disconnects and closes the ImageViewer. 
        /// </summary>
        public override void Close()
        {
            StopTimer();
            RemoveApplicationEventListeners();
            ImageViewerDisconnect();
            ImageViewerClose();

            if (_loadCameraTimer != null)
            {
                _loadCameraTimer.Stop();
                _loadCameraTimer = null;
            }
        }

        private void SetUpApplicationEventListeners()
        {
            _carrouselViewItemManager.PropertyChangedEvent += new EventHandler(viewItem_PropertyChangedEvent);

            //set up ApplicationController event
            _messageReference = EnvironmentManager.Instance.RegisterReceiver(ModeChangedHandler, new MessageIdFilter(MessageId.System.ModeChangedIndication));
        }

        private void RemoveApplicationEventListeners()
        {
            _carrouselViewItemManager.PropertyChangedEvent -= new EventHandler(viewItem_PropertyChangedEvent);

            //remove ApplicationController event
            EnvironmentManager.Instance.UnRegisterReceiver(_messageReference);
        }

        public override bool ShowToolbar
        {
            get { return true; }
        }

        #endregion


        #region Component private properties


        private bool InLive
        {
            get { return _inLive; }
            set { _inLive = value; }
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

        public override bool Selectable
        {
            get { return true; }
        }


        public override bool Selected
        {
            set
            {
                base.Selected = value;
                _imageViewerWpfControl.Selected = value;
            }
        }


        public override bool Hidden
        {
            set
            {
                if (base.Hidden != value)
                {
                    base.Hidden = value;

                    _imageViewerWpfControl.Hidden = value;
                }
            }
        }


        #endregion


        #region Component event handlers


        /// <summary>
        /// Tick to handle camera load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoadCameraTimer_Tick(object sender, EventArgs e)
        {
            if(_paused == false)
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
        }

        private object ModeChangedHandler(Message message, FQID destination, FQID source)
        {
            _paused = false;

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
            if (_imageViewerWpfControl != null)
            {
                _imageViewerWpfControl.UpdateStates();
                ShowNext();
            }
        }

        void CarouselViewItemUserControl_Previous(object sender, RoutedEventArgs e)
        {
            if (_carrouselViewItemManager.Items.Count <= 1)
            {
                return;
            }

            this.ShowPrevious();
        }

        void CarouselViewItemUserControl_Next(object sender, RoutedEventArgs e)
        {
            if (_carrouselViewItemManager.Items.Count <= 1)
            {
                return;
            }

            this.ShowNext();
        }

        private void OnMouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        private void ShowPlayerPanel(object sender, MouseEventArgs e)
        {
            if (EnvironmentManager.Instance.Mode == Mode.ClientLive)
            {
                panelPlayer.Visibility = Visibility.Visible;
            }
        }

        private void HidePlayerPanel(object sender, MouseEventArgs e)
        {
            panelPlayer.Visibility = Visibility.Hidden;
        }

        private void pauseButtonClick(object sender, RoutedEventArgs e)
        {
            if (_paused == false)
            {
                _paused = true;
                pauseButton.Content =" ► ";
                StopTimer();
            }
            else
            {
                _paused = false;
                pauseButton.Content = " | | ";
                StartTimer();
            }
        }


        #endregion


        #region Component private functions


        #region Carrousel item control methods


        /// <summary>
        /// Shows the next camera.
        /// </summary>
        private void ShowNext()
        {
            if (NextIndex != -1)
            {
                _currentIndex = NextIndex;
                _isPlayingForward = true;

                _loadCameraTimer.Stop();
                _loadCameraTimer.Interval = new TimeSpan(0, 0, _carrouselViewItemManager.Items[_currentIndex].Seconds);
                _loadCameraTimer.Start();
                Show(_carrouselViewItemManager.Items[_currentIndex]);
            }
        }

        /// <summary>
        /// Shows the previous camera.
        /// </summary>
        private void ShowPrevious()
        {
            if (PreviousIndex != -1)
            {
                _currentIndex = PreviousIndex;
                _isPlayingForward = false;

                _loadCameraTimer.Stop();
                _loadCameraTimer.Interval = new TimeSpan(0, 0, _carrouselViewItemManager.Items[_currentIndex].Seconds);
                _loadCameraTimer.Start();
                Show(_carrouselViewItemManager.Items[_currentIndex]);
            }
        }

        private void Show(CarrouselTreeNode carrouselTreeNode)
        {
            if (carrouselTreeNode != null)
            {
                LoadAndShow(carrouselTreeNode);
                _imageViewerWpfControl.Focus();
                UpdateAudio();
            }
            UpdateInfoTextIfNessesary();
        }

        private void UpdateAudio()
        {
            if (_currentIndex > -1)
            {
                CarrouselTreeNode carrouselTreeNode = _carrouselViewItemManager.Items[_currentIndex];
                if (carrouselTreeNode != null && Selected)
                {
                }
            }
        }

        private void LoadAndShow(CarrouselTreeNode carrouselTreeNode)
        {
            _cameraDeviceFront = carrouselTreeNode;
            if (carrouselTreeNode.Item.FQID.Kind == Kind.Camera)
            {
                ImageViewerDisconnect();
                _imageViewerWpfControl.CameraFQID = carrouselTreeNode.Item.FQID;
                _imageViewerWpfControl.ShowImageViewer = true;

                ImageViewerInitNoEvents(_imageViewerWpfControl);
                ImageViewerConnect(_imageViewerWpfControl);
            }
            else
                if (carrouselTreeNode.Item.FQID.Kind == Kind.Preset)
            {
                if (_imageViewerWpfControl.CameraFQID == null || _imageViewerWpfControl.CameraFQID.ObjectId != carrouselTreeNode.Item.FQID.ParentId)
                {
                    ImageViewerDisconnect();
                    _imageViewerWpfControl.CameraFQID = carrouselTreeNode.Item.FQID.GetParent();
                    _imageViewerWpfControl.ShowImageViewer = true;

                     ImageViewerInitNoEvents(_imageViewerWpfControl);
                    ImageViewerConnect(_imageViewerWpfControl);
                }
                EnvironmentManager.Instance.SendMessage(new Message(MessageId.Control.TriggerCommand),
                                                        carrouselTreeNode.Item.FQID, null);
            }
        }

        private void StartTimer()
        {
            _currentIndex = PreviousIndex;
            ShowNext();
            _isPlayingForward = true;
            if (_currentIndex != -1)
            {
                _loadCameraTimer.Start();
            }
        }

        private void StopTimer()
        {
            if (_currentIndex != -1)
            {
                _loadCameraTimer.Stop();
            }
        }


        #endregion


        #region Private properties


        private int NextIndex
        {
            get
            {
                if (_carrouselViewItemManager.Items == null || _carrouselViewItemManager.Items.Count == 0)
                {
                    return -1;
                }
                if (_currentIndex + 1 >= _carrouselViewItemManager.Items.Count)
                {
                    return 0;
                }
                return _currentIndex + 1;
            }
        }

        private int PreviousIndex
        {
            get
            {
                if (_carrouselViewItemManager.Items == null || _carrouselViewItemManager.Items.Count == 0)
                {
                    return -1;
                }
                if (_currentIndex <= 0)
                {
                    return _carrouselViewItemManager.Items.Count - 1;
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
                if (_carrouselViewItemManager.Items == null || _carrouselViewItemManager.Items.Count == 0)
                {
                    infoText = _stringResourceManager.GetString("NoCarrouselCameras");
                }
                else if (_cameraDeviceFront == null ||
                         (_currentIndex != -1 && _currentIndex < _carrouselViewItemManager.Items.Count &&
                         (_carrouselViewItemManager.Items[_currentIndex] == null)))
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
                _imageViewerWpfControl.InfoText = infoText;
            }
        }

        private void ShowNoCarruoselCamera()
        {
            ImageViewerDisconnect();
        }

        private void ContextMenuCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (_imageViewerWpfControl != null)
            {
                Bitmap bitmap = _imageViewerWpfControl.GetCurrentDisplayedImageAsBitmap();
                Clipboard.SetImage(ToWpfBitmap(bitmap));
            }
        }

        public static System.Windows.Media.Imaging.BitmapSource ToWpfBitmap(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);
                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
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

            _imageViewerWpfControl.ShowImageViewer = false;
        }

        /// <summary>
        /// Puts the camera into setup mode
        /// </summary>
        protected void StartSetup()
        {
            StopTimer();
            _currentIndex = -1;

            UpdateInfoTextIfNessesary();

            _imageViewerWpfControl.ShowImageViewer = false;
        }

        /// <summary>
        /// Initializes the ImageViewer. Calling setup methods and sets up events listeners
        /// </summary>
        protected void ImageViewerInit(ImageViewerWpfControl imageViewer)
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
            for (index = 0; index < _carrouselViewItemManager.Items.Count; index++)
            {
                CarrouselTreeNode carrouselTreeNode = _carrouselViewItemManager.Items[index];
                if (carrouselTreeNode != null && carrouselTreeNode.Item.FQID.Kind == Kind.Camera)
                {
                    _cameraDeviceFront = carrouselTreeNode;
                    _imageViewerWpfControl.CameraFQID = carrouselTreeNode.Item.FQID;
                    ImageViewerInit(_imageViewerWpfControl);
                    break;
                }
            }
        }

        ///// <summary>
        ///// Initializes the ImageViewer without event registrering.
        ///// </summary>
        protected void ImageViewerInitNoEvents(ImageViewerWpfControl imageViewer)
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
        protected void ImageViewerClose(ImageViewerWpfControl imageViewer)
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
            ImageViewerClose(_imageViewerWpfControl);

            _imageViewerWpfControl.Dispose();
        }

        /// <summary>
        /// Connects the ImageViewer to a camera device
        /// </summary>
        /// <param name="device">The camera device that the ImageViewer shall connect to</param>
        protected void ImageViewerConnect(ImageViewerWpfControl imageViewer)
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
                _imageViewerWpfControl.Disconnect();
            }
            catch (Exception)
            {
            }
        }


        #endregion
 

    }
}
