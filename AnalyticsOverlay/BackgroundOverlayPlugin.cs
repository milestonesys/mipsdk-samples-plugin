using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;
using FormattedText = System.Windows.Media.FormattedText;
using RectangleGeometry = System.Windows.Media.RectangleGeometry;
using Size = System.Drawing.Size;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using Typeface = System.Windows.Media.Typeface;

namespace AnalyticsOverlay
{
    public class BackgroundOverlayPlugin : BackgroundPlugin
    {
        #region private fields

        private List<ImageViewerAddOn> _activeImageViewerAddOns = new List<ImageViewerAddOn>();
        private Thread _thread;
        private bool _stop = false;
        private static int _sampleRateOfOverlay = 5;    // 5 overlays per second
        public static BackgroundOverlayPlugin Instance;
        private FQID _watchCameraFQID;

        private Dictionary<ImageViewerAddOn, Guid> _dictShapes = new Dictionary<ImageViewerAddOn, Guid>();

        #endregion

        #region Settings panel configuration
        public static FQID WatchCameraFQID
        {
            get
            {
                return Instance._watchCameraFQID;
            }
            set
            {
                if (Instance._watchCameraFQID != value)
                {
                    Instance._watchCameraFQID = value;
                    // Copy array to avoid deadlocks
                    ImageViewerAddOn[] tempList = new ImageViewerAddOn[Instance._activeImageViewerAddOns.Count];
                    lock (Instance._activeImageViewerAddOns)
                    {
                        Instance._activeImageViewerAddOns.CopyTo(tempList, 0);
                    }

                    //Go through all registered AddOns and identify the one we are looking for
                    foreach (ImageViewerAddOn addOn in tempList)
                    {
                        try
                        {
                            if (!Instance._stop) // Just in case we are stopping down in the middle of this loop
                                                         //Clear overlay
                            {
                                Instance.ClearOverlay(addOn);
                            }
                        }
                        catch (Exception ex)
                        {
                            EnvironmentManager.Instance.ExceptionDialog("WatchCameraFQID", ex);
                        }
                    }
                }
            }
        }
        #endregion

        #region Abstract class implementation

        public override Guid Id
        {
            get { return AnalyticsOverlayDefinition.AnalyticsOverlayBackgroundPlugin; }
        }

        public override void Init()
        {
            Instance = this;
            _watchCameraFQID = null;
            try
            {
                // Reading the configuration belonging to the Settings Panel
                XmlNode result = Configuration.Instance.GetOptionsConfiguration(AnalyticsOverlayDefinition.AnalyticsOverlaySettingsPanel, true);
                if (result != null)
                {
                    XmlNode childNode = result.FirstChild;
                    string id = childNode.Attributes["value"].Value;
                    if (id != null)
                    {
                        Guid guid = new Guid(id);
                        Item camera = Configuration.Instance.GetItem(guid, Kind.Camera);
                        if (camera != null)
                        {
                            _watchCameraFQID = camera.FQID;
                        }
                    }
                }
            }
            catch
            {
                //ignore if there is no camera
            }
            ClientControl.Instance.NewImageViewerControlEvent += new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
            _stop = false;
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        public override void Close()
        {
            ClientControl.Instance.NewImageViewerControlEvent -= new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
            _stop = true;
        }

        public override string Name
        {
            get { return "BackgroundOverlayCheck"; }
        }

        /// <summary>
        /// Only run in the Smart Client
        /// </summary>
        public override List<EnvironmentType> TargetEnvironments
        {
            get
            {
                return new List<EnvironmentType>() { EnvironmentType.SmartClient };
            }
        }

        #endregion

        #region Event registration on/off
        /// <summary>
        /// Register all the events we need
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void RegisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent += new EventHandler(ImageViewerAddOn_CloseEvent);
            imageViewerAddOn.PropertyChangedEvent += new EventHandler(imageViewerAddOn_PropertyChangedEvent);
            imageViewerAddOn.RecordedImageReceivedEvent += new RecordedImageReceivedHandler(ImageViewerAddOn_RecordedImageReceivedEvent);
            imageViewerAddOn.StartLiveEvent += new PassRequestEventHandler(ImageViewerAddOn_StartLiveEvent);
            imageViewerAddOn.StopLiveEvent += new PassRequestEventHandler(ImageViewerAddOn_StopLiveEvent);
            imageViewerAddOn.ImageDisplayedEvent += imageViewerAddOn_ImageDisplayedEvent;
        }

        /// <summary>
        /// Unhook all my event handlers
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void UnregisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent -= new EventHandler(ImageViewerAddOn_CloseEvent);
            imageViewerAddOn.PropertyChangedEvent -= new EventHandler(imageViewerAddOn_PropertyChangedEvent);
            imageViewerAddOn.RecordedImageReceivedEvent -= new RecordedImageReceivedHandler(ImageViewerAddOn_RecordedImageReceivedEvent);
            imageViewerAddOn.StartLiveEvent -= new PassRequestEventHandler(ImageViewerAddOn_StartLiveEvent);
            imageViewerAddOn.StopLiveEvent -= new PassRequestEventHandler(ImageViewerAddOn_StopLiveEvent);
            imageViewerAddOn.ImageDisplayedEvent -= imageViewerAddOn_ImageDisplayedEvent;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// A new ImageViewer has been created
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
        {
            lock (_activeImageViewerAddOns)
            {
                String tx = "null";
                if (imageViewerAddOn.WindowInformation != null)
                    tx = "" + imageViewerAddOn.WindowInformation.WindowId;
                Debug.WriteLine("--->Windowinformation :" + tx);
                if (CameraMatch(imageViewerAddOn))
                {
                    imageViewerAddOn.ShowMetadataOverlay = false;
                }
                RegisterEvents(imageViewerAddOn);
                _activeImageViewerAddOns.Add(imageViewerAddOn);
            }
        }

        /// <summary>
        /// The smart client is now going into setup or playback mode (Or just this one camera is in instant playback mode)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_StopLiveEvent(object sender, PassRequestEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
            }
        }

        /// <summary>
        /// The Smart Client is now going into live mode.  We would overtake or reject that this item is going into live.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_StartLiveEvent(object sender, PassRequestEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
            }
        }

        /// <summary>
        /// Some property has changed on the camera View Item (or hotspot etc.)
        /// Let's check that the camera id is the one we watch for.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageViewerAddOn_PropertyChangedEvent(object sender, EventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
                if (WatchCameraFQID == null || imageViewerAddOn.CameraFQID.ObjectId != WatchCameraFQID.ObjectId)
                {
                    try
                    {
                        //Clear the overlay
                        ClearOverlay(imageViewerAddOn);
                    }
                    catch (Exception ex)
                    {
                        EnvironmentManager.Instance.ExceptionDialog("imageViewerAddOn_PropertyChangedEvent", ex);
                    }
                }
            }
        }

        /// <summary>
        /// In Playback mode, an Image has been received.
        /// In our sample we redisplay / draw the image every time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_RecordedImageReceivedEvent(object sender, RecordedImageReceivedEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null && WatchCameraFQID != null)
            {
                if (imageViewerAddOn.CameraFQID.ObjectId == WatchCameraFQID.ObjectId)
                {
                    DrawOverlay(imageViewerAddOn, e.DateTime);
                }
            }
        }

        /// <summary>
        /// One of the ImageViewer has been closed / Removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_CloseEvent(object sender, EventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
                UnregisterEvents(imageViewerAddOn);

                try
                {
                    ClearOverlay(imageViewerAddOn);
                }
                catch (Exception ex)
                {
                    EnvironmentManager.Instance.ExceptionDialog("ImageViewerAddOn_CloseEvent", ex);
                }
                lock (_activeImageViewerAddOns)
                {
                    // Remove from list
                    if (_activeImageViewerAddOns.Contains(imageViewerAddOn))
                        _activeImageViewerAddOns.Remove(imageViewerAddOn);
                }
            }
        }

        /// <summary>
        /// Here we get detailed information about the actual displayed image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageViewerAddOn_ImageDisplayedEvent(object sender, ImageDisplayedEventArgs e)
        {
            // Be sure to handle quickly!!
        }

        private void ClearOverlay(ImageViewerAddOn imageViewerAddOn)
        {
            try
            {
                // Clear the overlay
                Guid shapeID;
                if (_dictShapes.TryGetValue(imageViewerAddOn, out shapeID))
                {
                    ClientControl.Instance.CallOnUiThread(() => imageViewerAddOn.ShapesOverlayRemove(shapeID));
                    _dictShapes.Remove(imageViewerAddOn);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("ImageViewerAddOn_ClearOverlay", ex);
            }
        }

        #endregion

        #region Background thread
        /// <summary>
        /// Draw overlay for live mode ImageViewerControls.
        /// </summary>
        void Run()
        {
            while (!_stop)
            {
                if (WatchCameraFQID != null && _activeImageViewerAddOns.Count > 0)
                {
                    try
                    {
                        // Copy array to avoid deadlocks
                        ImageViewerAddOn[] tempList = new ImageViewerAddOn[_activeImageViewerAddOns.Count];
                        lock (_activeImageViewerAddOns)
                        {
                            _activeImageViewerAddOns.CopyTo(tempList, 0);
                        }

                        //Go through all registered AddOns and identify the one we are looking for
                        foreach (ImageViewerAddOn addOn in tempList)
                        {
                            if (!_stop)         // Avoid going into this loop if we are in progress of closing the app 
                            {
                                if (CameraMatch(addOn))
                                {
                                    //Only draw the ones in Live mode
                                    if (addOn.InLiveMode)
                                    {
                                        DrawOverlay(addOn, DateTime.Now);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EnvironmentManager.Instance.ExceptionDialog("Background Overlay", ex);
                    }
                }
                Thread.Sleep(1000 / _sampleRateOfOverlay);
            }
            _thread = null;
        }

        private bool CameraMatch(ImageViewerAddOn addOn)
        {
            return addOn.CameraFQID != null &&
                    addOn.CameraFQID.ObjectId != Guid.Empty &&
                    WatchCameraFQID != null &&
                    addOn.CameraFQID.ObjectId == WatchCameraFQID.ObjectId;
        }


        private delegate void DrawOverlayDelegate(ImageViewerAddOn addOn, DateTime dateTime);
        private void DrawOverlay(ImageViewerAddOn addOn, DateTime dateTime)
        {
            if (addOn.PaintSize.Width == 0 || addOn.PaintSize.Height == 0)
            {
                return;
            }
            // We need to be on the UI thread for setting the overlay
            ClientControl.Instance.CallOnUiThread(() =>
                {
                    try
                    {
                            // number between 0 and 100 -counting 1/10 of seconds
                            int s = dateTime.Second % 10 * 10 + dateTime.Millisecond / 100;
                        List<Shape> shapes = new List<Shape>();
                        shapes.Add(CreateBoxShape(addOn.PaintSize, s * 50 / _sampleRateOfOverlay, s * 50 / _sampleRateOfOverlay, 100, 200, Colors.Yellow, 0.7));
                        shapes.Add(CreateTextShape(addOn.PaintSize, s.ToString(), s * 50 / _sampleRateOfOverlay, s * 50 / _sampleRateOfOverlay, 100, Colors.Red));

                        if (!_dictShapes.ContainsKey(addOn))
                        {
                            _dictShapes.Add(addOn, addOn.ShapesOverlayAdd(shapes, new ShapesOverlayRenderParameters() { ZOrder = 100 }));
                        }
                        else
                        {
                            addOn.ShapesOverlayUpdate(_dictShapes[addOn], shapes, new ShapesOverlayRenderParameters() { ZOrder = 100 });
                        }
                    }
                    catch (Exception ex)        // Ignore exceptions during close
                    {
                        Debug.WriteLine("DrawOverlay:" + ex.Message);
                    }
                });
        }

        #endregion

        #region create shapes.
        /// <summary>
        /// scale values of 0 - 1000 will be used to calculate the right placement of true display values
        /// </summary>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <param name="scaleFontSize"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Shape CreateTextShape(Size size, string text, double scaleX, double scaleY, double scaleFontSize, Color color)
        {
            double x = size.Width * scaleX / 1000;
            double y = size.Height * scaleY / 1000;
            double fontsize = size.Height * scaleFontSize / 1000;

            return CreateTextShape(text, x, y, fontsize, color);
        }

        private Shape CreateTextShape(string text, double placeX, double placeY, double fontSize, Color color)
        {
            Shape textShape;
            System.Windows.Media.FontFamily fontFamily = new System.Windows.Media.FontFamily("Times New Roman");
            Typeface typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Bold, new FontStretch());
            Path path = new Path();

            FormattedText fText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                typeface, fontSize, System.Windows.Media.Brushes.Black, System.Windows.Media.VisualTreeHelper.GetDpi(path).PixelsPerDip);

            Point textPosition1;
            textPosition1 = new Point(placeX, placeY);

            path.Data = fText.BuildGeometry(textPosition1);
            path.Fill = new SolidColorBrush(color);
            textShape = path;
            return textShape;
        }

        /// <summary>
        /// scale values of 0-1000 will be used to calculate the right placement of true display values
        /// </summary>
        /// <param name="displayWidth"></param>
        /// <param name="displayHeight"></param>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <param name="scaleWidth"></param>
        /// <param name="scaleHeight"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Shape CreateBoxShape(Size size, int scaleX, int scaleY, int scaleWidth, int scaleHeight, Color color, double opacity)
        {
            int x = size.Width * scaleX / 1000;
            int y = size.Height * scaleY / 1000;
            int width = size.Width * scaleWidth / 1000;
            int height = size.Height * scaleHeight / 1000;

            return CreateBoxShape(x, y, width, height, color, opacity);
        }
        private Shape CreateBoxShape(int x, int y, int width, int height, Color color, double opacity)
        {
            Shape boxShape;
            Path path2 = new Path();
            path2.Data = new RectangleGeometry(new Rect(new Point(x, y), new System.Windows.Size(width, height)));
            path2.Stroke = new SolidColorBrush(color);
            path2.Fill = new SolidColorBrush(color);
            path2.Fill.Opacity = opacity;

            boxShape = path2;
            return boxShape;
        }
        #endregion
    }
}
