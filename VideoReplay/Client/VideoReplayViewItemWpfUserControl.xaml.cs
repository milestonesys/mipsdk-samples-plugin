using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using BitmapData = VideoOS.Platform.Data.BitmapData;

namespace VideoReplay.Client
{
    public partial class VideoReplayViewItemWpfUserControl : ViewItemWpfUserControl
    {
        private const int VideoIntervalSeconds = 15;
        private const int ReplaySpeedFactor = 2;

        #region Component private class variables

        private bool _inLive = true;
        private VideoReplayViewItemManager _viewItemManager;
        private object _selectedCameraChangedReceiver;
        private bool _stop = false;

        private Item _selectedItem;
        private string _previousSelectedName = "";
        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a VideoReplayViewItemUserControl instance
        /// </summary>
        public VideoReplayViewItemWpfUserControl(VideoReplayViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ApplicationController events
            _selectedCameraChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(SelectedCameraChangedHandler),
                                                         new MessageIdFilter(MessageId.SmartClient.SelectedCameraChangedIndication));
            var imageClear = new Bitmap(320, 240);
            Graphics g = Graphics.FromImage(imageClear);
            g.FillRectangle(System.Drawing.Brushes.Black, 0, 0, 320, 240);
            g.Dispose();
            Dispatcher.BeginInvoke(new SetImageDelegate(SetImage), new[] { ToWpfBitmap(imageClear) });
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ApplicationController events
            EnvironmentManager.Instance.UnRegisterReceiver(_selectedCameraChangedReceiver);
        }

        public override void Init()
        {
            SetUpApplicationEventListeners();

            InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;
            _stop = false;
        }

        /// <summary>
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            RemoveApplicationEventListeners();
            _stop = true;
        }

        /// <summary>
        /// Show toolbar to enable the print function
        /// </summary>
        public override bool ShowToolbar
        {
            get { return true; }
        }

        #endregion

        #region Print method
        /// <summary>
        /// This method is called when the user presses the Print button.
        /// You can supply your own information.
        /// This default implementation will print the UserControl as the bitmap.
        /// You can also use the ClientControl.Instance.Print method to supply your own bitmap.
        /// </summary>
        public override void Print()
        {
            Print(_previousSelectedName, "Some extra information");
        }

        #endregion

        #region Component events

        /// <summary>
        /// Activates the single click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        /// <summary>
        /// Activates the double click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
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


        #endregion

        #region Component properties


        public bool InLive
        {
            get { return _inLive; }
            set { _inLive = value; }
        }

        public override bool Maximizable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Tell if Item is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Overrides property (set). First the Base implementation is called.
        /// When maximized the image quality should always be forced to full quality.
        /// </summary>
        public override bool Maximized
        {
            set
            {
                if (base.Maximized != value)
                {
                    base.Maximized = value;
                }
            }
        }

        /// <summary>
        /// Overrides property (set). First the Base implementation is called. 
        /// </summary>
        public override bool Hidden
        {
            set
            {
                if (base.Hidden != value)
                {
                    base.Hidden = value;
                }
            }
        }

        #endregion

        #region Component event handlers


        /// <summary>
        /// Adds selected camera name to UI label1.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private object SelectedCameraChangedHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            var myitem = Configuration.Instance.GetItem(message.RelatedFQID);


            if (myitem != null)
            {
                _selectedItem = myitem;
                label1.Content = "Camera: " + _selectedItem.Name;
                _previousSelectedName = _selectedItem.Name;
            }
            _stop = true;
            return null;
        }

        #endregion

        /// <summary>
        /// Handles the buttonReplay click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReplay(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null)
            {
                _stop = true;
                Thread thread = new Thread(new ThreadStart(ShowReplay));
                thread.Start();
            }
        }
        /// <summary>
        /// Replays video received from the selected camera item on imageWindow.
        /// </summary>
        private void ShowReplay()
        {
            var imageClear = new Bitmap(320, 240);
            Graphics g = Graphics.FromImage(imageClear);
            g.FillRectangle(System.Drawing.Brushes.Black, 0, 0, 320, 240);
            g.Dispose();
            Dispatcher.BeginInvoke(new SetImageDelegate(SetImage), new[] { ToWpfBitmap(imageClear) });
            BitmapVideoSource source = new BitmapVideoSource(_selectedItem);
            source.SetKeepAspectRatio(true, true);
            source.Init();
            var interval = TimeSpan.FromSeconds(VideoIntervalSeconds);
            List<object> resultList = source.Get(DateTime.Now - interval, interval, 150);

            if (!resultList.Any())
            {
                Dispatcher.BeginInvoke(new Action(delegate () { label2.Content = "Number of frames: 0"; }));
            }
            else
            {
                var first = resultList.First() as BitmapData;
                var last = resultList.Last() as BitmapData;
                var resultInterval = last.DateTime - first.DateTime;
                List<System.Windows.Media.Imaging.BitmapSource> wpfResultList = resultList.Cast<BitmapData>().Select(bmp => ToWpfBitmap(bmp.GetBitmap())).ToList();
                resultList.Clear();
                Dispatcher.BeginInvoke(new Action(delegate () { label2.Content = "Number of frames: " + wpfResultList.Count; }));
                _stop = false;
                while (!_stop)
                {
                    int avgInterval = 1000 * (int)resultInterval.TotalSeconds / (wpfResultList.Count * ReplaySpeedFactor);
                    foreach (System.Windows.Media.Imaging.BitmapSource bitmap in wpfResultList)
                    {
                        Dispatcher.BeginInvoke(new SetImageDelegate(SetImage), new[] { bitmap });
                        Thread.Sleep(avgInterval);
                        if (_stop)
                            break;
                    }
                    if (!_stop)
                    {
                        Thread.Sleep(1500);
                    }
                }
                source.Close();
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
                bitmap.Dispose();
                return result;
            }
        }

        private delegate void SetImageDelegate(System.Windows.Media.Imaging.BitmapSource bitmap);

        /// <summary>
        /// Sets imageWindow size according to the size of the source.
        /// </summary>
        /// <param name="bitmap"></param>
        private void SetImage(System.Windows.Media.Imaging.BitmapSource bitmap)
        {
            if (imageWindowContainer.IsVisible && imageWindowContainer.ActualHeight > 0)
            {
                double ratio = Convert.ToDouble(imageWindowContainer.ActualWidth) / imageWindowContainer.ActualHeight;
                double ratioNew = Convert.ToDouble(bitmap.Width) / bitmap.Height;

                double w = imageWindowContainer.ActualWidth;
                double h = imageWindowContainer.ActualHeight;
                if (ratio > ratioNew)
                {
                    w = h * ratioNew;
                }
                else
                {
                    h = w / ratioNew;
                }
                imageWindow.Width = w;
                imageWindow.Height = h;
                imageWindow.Source = bitmap;
            }
        }

        /// <summary>
        /// Stops replaying the video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            _stop = true;
        }
    }
}
