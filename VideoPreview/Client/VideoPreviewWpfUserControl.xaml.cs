using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace VideoPreview.Client
{
    public partial class VideoPreviewWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private VideoPreviewViewItemManager _viewItemManager;

        private Item _selectedCameraItem;
        private AudioPlayer _audioPlayer;
        private bool _lineMode;
        private Point _next;
        private Point _first;
        private Point _firstPointNormed;
        private Point _nextPointNormed;
        private Guid _overlayId;
        private List<object> _messageRegistrationObjects = new List<object>();
        private bool _inLiveMode = false;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a VideoPreviewWpfViewItemUserControl instance
        /// </summary>
        public VideoPreviewWpfUserControl(VideoPreviewViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        private void SetupApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _imageViewer.ImageDisplayed += IVC_ImageEvent;
            _imageViewer.RecordedImageReceived += IVC_ImageEvent;
            _imageViewer.ImageOrPaintInfoChanged += IVC_ImageInfoChangedEvent;

            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ShownWorkSpaceChangedReceiver, new MessageIdFilter(MessageId.SmartClient.ShownWorkSpaceChangedIndication)));
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            _imageViewer.ImageDisplayed -= IVC_ImageEvent;
            _imageViewer.RecordedImageReceived -= IVC_ImageEvent;
            _imageViewer.ImageOrPaintInfoChanged -= IVC_ImageInfoChangedEvent;

            if (chkLiveInfo.IsChecked.Equals(true))
            {
                _imageViewer.LiveStreamInformationReceived -= IVC_LiveStreamInformationEvent;
            }

            foreach (var messageRegistration in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistration);
            }
            _messageRegistrationObjects.Clear();
        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {

            _audioPlayer = new AudioPlayer();
            SetupApplicationEventListeners();

            string fqidString = _viewItemManager.GetProperty(ClientControl.EmbeddedCameraFQIDProperty);
            if (fqidString != null)
            {
                _selectedCameraItem = Configuration.Instance.GetItem(new FQID(fqidString));
                FillCameraSelection();
                FillMicrophoneSelection();
            }
            HandleWorkspace();
        }


        /// <summary>
        /// Method that is called when the view item is closed. The view item should free all resources when the method is called.
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            RemoveApplicationEventListeners();
        }

        #endregion

        #region Component events

        private void ViewItemWpfUserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireClickEvent();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                FireRightClickEvent(e);
            }
        }

        private void ViewItemWpfUserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireDoubleClickEvent();
            }
        }

        /// <summary>
        /// Signals that the control is right clicked
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

        private object ShownWorkSpaceChangedReceiver(Message message, FQID destination, FQID sender)
        {
            HandleWorkspace();
            return null;
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
        /// Tell if the ViewItem is selected.
        /// Modified setter so that the Selected property is set in the ImageViewer also.
        /// </summary>
        public override bool Selected
        {
            get => base.Selected;
            set
            {
                if (_imageViewer != null)
                {
                    _imageViewer.Selected = value;
                }
                base.Selected = value;
            }
        }

        /// <summary>
        /// Tell if the toolbar that normally pops up from the bottom of a viewitem should be present
        /// </summary>
        public override bool ShowToolbar
        {
            get { return false; }
        }

        #endregion

        #region UI interaction

        /// <summary>
        /// Selecting the camera and starting it. 
        /// Persist the camera choice by saving properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectCamera(object sender, System.Windows.RoutedEventArgs e)
        {
            _imageViewer.Disconnect();
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                SelectedItems = new List<Item>() { _selectedCameraItem },
                Items = Configuration.Instance.GetItemsByKind(Kind.Camera),
            };

            if (_selectedCameraItem != null)
            {
                form.SelectedItems = new List<Item> { _selectedCameraItem };
            }

            form.ShowDialog();
            if (form.SelectedItems != null && form.SelectedItems.Any())
            {
                _selectedCameraItem = form.SelectedItems.First();
            }
            FillCameraSelection();
            FillMicrophoneSelection();
            // By  setting the EmbeddedCameraFQIDProperty SC recogizes this as a camera and automtically fills the time line etc..
            if (_selectedCameraItem != null)
            {
                _viewItemManager.SetProperty(ClientControl.EmbeddedCameraFQIDProperty, _selectedCameraItem.FQID.ToXmlNode().OuterXml);
            }
            else
            {
                _viewItemManager.SetProperty(ClientControl.EmbeddedCameraFQIDProperty, string.Empty);
            }
            _viewItemManager.SaveAllProperties();
        }

        private void FillCameraSelection()
        {
            btnSelectCamera.Content = "";
            if (_selectedCameraItem != null)
            {
                btnSelectCamera.Content = _selectedCameraItem.Name;
                _imageViewer.CameraFQID = _selectedCameraItem.FQID;
                _imageViewer.EnableVisibleLiveIndicator = chkLiveIndicator.IsChecked.Value;
                _imageViewer.EnableVisibleHeader = chkHeader.IsChecked.Value;
                _imageViewer.MaintainImageAspectRatio = chkAspectRatio.IsChecked.Value;
                _imageViewer.EnableMouseControlledPtz = true;
                _imageViewer.EnableDigitalZoom = chkDigitalZoom.IsChecked.Value;
                _imageViewer.EnableMousePtzEmbeddedHandler = chkDigitalZoom.IsChecked.Value;
                _imageViewer.Initialize();
                _imageViewer.Connect();

                cmbStreams.ItemsSource = null;
                cmbStreams.Items.Clear();
                var streamDataSource = new StreamDataSource(_selectedCameraItem);
                IList<DataType> streams = streamDataSource.GetTypes();
                if (streams.Count > 1)
                {
                    cmbStreams.ItemsSource = streams;
                }
                foreach (DataType stream in cmbStreams.Items)
                {
                    string value;
                    if (stream.Properties.TryGetValue("Default", out value))
                    {
                        if (value == "Yes")
                        {
                            cmbStreams.SelectedItem = stream;
                        }

                    }
                }
                cmbStreams.IsEnabled = _inLiveMode && (cmbStreams.Items.Count > 0);
            }
        }

        private void FillMicrophoneSelection()
        {
            if (_audioPlayer.MicrophoneFQID != null)
            {
                _audioPlayer.Disconnect();
            }

            if (_selectedCameraItem == null)
            {
                return;
            }

            Item mic = _selectedCameraItem.GetRelated().Find(x => x.FQID.Kind.Equals(Kind.Microphone));

            var test = _imageViewer.PlaybackControllerFQID;
            if (mic != null)
            {
                _audioPlayer.Initialize();
                _audioPlayer.MicrophoneFQID = mic.FQID;
                try
                {
                    _audioPlayer.Connect();
                }
                catch (MIPException ex) 
                {
                    // AudioPlayer.Connect() throws MIPException if:
                    // - AudioPlayer.MicrophoneFQID is not set
                    // - No audio devices are available e.g. when Windows Audio service is not running
                    DisplayMessage(ex.Message, SmartClientMessageDataType.Warning);
                    _audioPlayer.Close();
                }
            }
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

        private void OnAspectChange(object sender, RoutedEventArgs e)
        {
            if (_imageViewer != null)
            {
                _imageViewer.MaintainImageAspectRatio = chkAspectRatio.IsChecked ?? false;
                _imageViewer.Disconnect();
                _imageViewer.Connect();
            }
        }

        private void OnLiveTickChanged(object sender, RoutedEventArgs e)
        {
            if (chkLiveInfo.IsChecked.Equals(true))
            {
                _imageViewer.LiveStreamInformationReceived += IVC_LiveStreamInformationEvent;
            }
            else
            {
                _imageViewer.LiveStreamInformationReceived -= IVC_LiveStreamInformationEvent;
            }

            _imageViewer.Disconnect();
            _imageViewer.Initialize();
            _imageViewer.Connect();
            txtLiveInfo.Text = "";
        }

        private void OnShowHeaderChanged(object sender, RoutedEventArgs e)
        {
            _imageViewer.EnableVisibleHeader = chkHeader.IsChecked.Value;
            chkLiveIndicator.IsEnabled = chkHeader.IsChecked.Value;
            _imageViewer.Disconnect();
            _imageViewer.Initialize();
            _imageViewer.Connect();
        }

        private void OnShowLiveIndicatorChanged(object sender, RoutedEventArgs e)
        {
            _imageViewer.EnableVisibleLiveIndicator = chkLiveIndicator.IsChecked.Value;
            _imageViewer.Disconnect();
            _imageViewer.Connect();
        }

        private void OnClip(object sender, RoutedEventArgs e)
        {
            Bitmap bitmap = _imageViewer.GetCurrentDisplayedImageAsBitmap();
            if (bitmap != null)
            {
                System.Windows.Forms.Clipboard.SetImage(bitmap);
            }
        }

        private void OnDigitalZoom(object sender, RoutedEventArgs e)
        {
            bool isSet = chkDigitalZoom.IsChecked.Value;
            if (_imageViewer != null)
            {
                _imageViewer.EnableDigitalZoom = isSet;
                _imageViewer.EnableMousePtzEmbeddedHandler = isSet;
                btnPtzCenter.IsEnabled = isSet;
                chkLine.IsEnabled = !isSet;
                _imageViewer.Disconnect();
                _imageViewer.Connect();
            }
        }

        private void OnPtzCenter(object sender, RoutedEventArgs e)
        {
            if (_imageViewer != null)
            {
                _imageViewer.PtzCenter(100, 100, 33, 33, 75);
            }
        }

        private void OnLine(object sender, RoutedEventArgs e)
        {
            _lineMode = chkLine.IsChecked ?? false;
            chkDigitalZoom.IsEnabled = !_lineMode;
        }

        private void OnStreamSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbStreams.ItemsSource != null)
            {
                if (_imageViewer != null)
                {
                    if (_imageViewer.CameraFQID != null)
                    {
                        _imageViewer.Disconnect();
                        DataType selectStream;
                        selectStream = (DataType)cmbStreams.SelectedItem;
                        if (selectStream.Properties["Default"] == "No")
                        {
                            _imageViewer.StreamId = selectStream.Id;
                        }
                        else
                        {
                            _imageViewer.StreamId = Guid.Empty;
                        }
                        _imageViewer.Connect();
                    }
                }
            }

        }

        private void HandleWorkspace()
        {
            _inLiveMode = ClientControl.Instance.ShownWorkSpace.FQID.ObjectId == ClientControl.LiveBuildInWorkSpaceId;
            chkLiveInfo.IsEnabled = _inLiveMode;
            cmbStreams.IsEnabled = _inLiveMode && (cmbStreams.Items.Count > 0);
            txtLiveInfo.Text = "";
        }

        #endregion

        #region Drawing the line methods

        /// <summary>
        /// Will transform and draw a line between the two points picked up by the mouse down and mouse up operation of the user.
        /// First make sure to transform the points in a way so the black areas pertaining to keeping aspect ratio for the camera images are included.
        /// Next save the points in a normalized value; to allow the possibility of screen resizing, the points are saved as fraction of the paint size.
        /// Finally; will call DrawLine method to do the actual drawing.
        /// </summary>
        private void DoLine()
        {
            Point firstPoint = TransformRatio(_first, _imageViewer.PaintSize, _imageViewer.RenderSize);
            Point nextPoint = TransformRatio(_next, _imageViewer.PaintSize, _imageViewer.RenderSize);
            _firstPointNormed = Norm(firstPoint, _imageViewer.PaintSize);
            _nextPointNormed = Norm(nextPoint, _imageViewer.PaintSize);
            DrawLine(firstPoint, nextPoint);
        }

        /// <summary>
        /// Draw the line by creating a shape and then applying this as overlay.
        /// Works with absolute values in pixels
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="nextPoint"></param>
        private void DrawLine(Point firstPoint, Point nextPoint)
        {
            if (_imageViewer == null) return;

            _imageViewer.ShapesOverlayRemove(_overlayId);
            Shape lineShape;
            Path path2 = new Path();
            path2.Data = new LineGeometry(firstPoint, nextPoint);
            path2.Stroke = Brushes.Red;
            lineShape = path2;
            List<Shape> shapes = new List<Shape>() { lineShape };
            ShapesOverlayRenderParameters parameters = new ShapesOverlayRenderParameters() { FollowDigitalZoom = true, ZOrder = 1 };
            _overlayId = _imageViewer.ShapesOverlayAdd(shapes, parameters);

        }

        /// <summary>
        /// Upon resize of the control, redraw the line based on the global normalized points.
        /// </summary>
        private void RedoLine()
        {
            Point firstPoint = DeNorm(_firstPointNormed, _imageViewer.PaintSize);
            Point nextPoint = DeNorm(_nextPointNormed, _imageViewer.PaintSize);
            DrawLine(firstPoint, nextPoint);
        }

        private Point TransformRatio(Point point, Size paintSize, Size renderSize)
        {
            var leftMargin = (renderSize.Width - paintSize.Width) / 2;
            point.X -= leftMargin;
            var topMargin = (renderSize.Height - paintSize.Height) / 2;
            point.Y -= topMargin;
            return point;
        }

        private Point Norm(Point point, Size paintSize)
        {
            return new Point(point.X / paintSize.Width, point.Y / paintSize.Height);
        }

        private Point DeNorm(Point point, Size paintSize)
        {
            return new Point(point.X * paintSize.Width, point.Y * paintSize.Height);
        }

        #endregion 

        #region ImageViewer Event handlers

        void IVC_ImageEvent(object sender, EventArgs e)
        {
            ImageDisplayedEventArgs imageDisplayedEventArgs = e as ImageDisplayedEventArgs;
            if (imageDisplayedEventArgs != null)
            {
                txtDisplayTime.Text = imageDisplayedEventArgs.ImageTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            RecordedImageReceivedEventArgs recordedImageReceivedEventArgs = e as RecordedImageReceivedEventArgs;
            if (recordedImageReceivedEventArgs != null)
            {
                txtDisplayTime.Text = recordedImageReceivedEventArgs.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }

        private void IVC_LiveStreamInformationEvent(object sender, LiveStreamInformationReceivedEventArgs liveInformation)
        {
            txtLiveInfo.Text = liveInformation.Information;
        }

        private void IVC_ImageInfoChangedEvent(object sender, ImageOrPaintInfoChangedEventArgs e)
        {
            if (_overlayId != null && _overlayId != Guid.Empty)
            {
                RedoLine();
            }
        }

        private void IVC_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_lineMode)
            {
                _first = e.GetPosition(_imageViewer);
            }
        }

        private void IVC_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_lineMode)
            {
                _next = e.GetPosition(_imageViewer);
                DoLine();
            }
        }

        #endregion

    }
}
