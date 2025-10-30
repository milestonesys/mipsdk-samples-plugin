using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DemoDriverDevice
{
    public partial class WorkPanel : UserControl
    {
        private delegate byte[] GetPlaybackFrameDelegate(DateTime dateTime);
        private delegate void AddActionLineDelegate(string commandLine);
        private delegate byte[] MediaProviderGetMediaDataDelegate(int channel, int stream, string textOverlay);

        private Dictionary<string, string> _parameters;
        private readonly Guid _id;
        private readonly Session _session = new Session();
        private readonly List<string> _generatedEvents = new List<string>();
        private readonly object _generatedEventsLock = new object();
        private bool _videoChannel1Started = false;
        private bool _videoChannel2Started = false;
        private double _videoChannel1Fps = 8.0;
        private double _videoChannel2Fps = 8.0;
        private DateTime _videoChannel1LastFrame = DateTime.MinValue;
        private DateTime _videoChannel2LastFrame = DateTime.MinValue;
        private bool _stream0HighRes = true;
        private bool _stream1HighRes = true;

        private int _firmwareUpgradeProgress = 0;
        private Timer _firmwareUpgradeTimer = new Timer();

        public MetadataProvider MetadataProvider1 { get; }

        public MetadataProvider MetadataProvider2 { get; }

        public AudioProvider AudioProvider { get; }

        public WorkPanel()
        {
            _id = Guid.NewGuid();
            MetadataProvider1 = new MetadataProvider();
            MetadataProvider2 = new MetadataProvider();
            AudioProvider = new AudioProvider();
            InitializeComponent();
            _firmwareUpgradeTimer.Interval = 1000;
            _firmwareUpgradeTimer.Tick += _firmwareUpgradeTimer_Tick;
        }

        public void Init()
        {
            SendInput1State();
            SendInput2State();
        }

        public void Close()
        {
            _firmwareUpgradeTimer.Stop();
        }

        public Guid StartSession(int channel, Dictionary<string, string> parameters)
        {
            if (!_videoChannel1Started && !_videoChannel2Started)
            {
                _parameters = parameters;
                MediaProviderSessionOpening();
            }
            if (channel == DemoDeviceConstants.DeviceVideoChannel1)
            {
                _videoChannel1Started = true;
            }
            if (channel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                _videoChannel2Started = true;
            }

            return _id;
        }

        public void StopSession(int channel)
        {
            if (channel == DemoDeviceConstants.DeviceVideoChannel1)
            {
                _videoChannel1Started = false;
            }
            if (channel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                _videoChannel2Started = false;
            }
            if (!_videoChannel1Started && !_videoChannel2Started)
            {
                MediaProviderSessionClosed();
            }
        }

        public byte[] GetLiveFrame(int channel, int stream, string textOverlay)
        {
            // introduce delay in order to ensure proper framerate
            double frameDistance = 0.0;
            DateTime lastFrame = DateTime.MinValue;
            if (channel == DemoDeviceConstants.DeviceVideoChannel1)
            {
                frameDistance = 1000 / _videoChannel1Fps;
                lastFrame = _videoChannel1LastFrame;
            }
            else if (channel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                frameDistance = 1000 / _videoChannel2Fps;
                lastFrame = _videoChannel2LastFrame;
            }
            if (frameDistance != 0.0)
            {
                var timeDiff = DateTime.UtcNow - lastFrame;
                if (timeDiff < TimeSpan.FromMilliseconds(frameDistance))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(frameDistance) - timeDiff);
                }
            }
            return MediaProviderGetMediaData(channel, stream, textOverlay);
        }

        public bool GetInputState1()
        {
            return checkBoxInput1.Checked;
        }

        public bool GetInputState2()
        {
            return checkBoxInput2.Checked;
        }

        public string[] GetEvents()
        {
            lock (_generatedEventsLock)
            {
                var result = _generatedEvents.ToArray();
                _generatedEvents.Clear();
                return result;
            }
        }

        public byte[] GetPlaybackFrame(DateTime dateTime)
        {
            if (InvokeRequired)
            {
                // Get on the UI thread before executing the below code
                return (byte[])this.Invoke(new GetPlaybackFrameDelegate(GetPlaybackFrame), dateTime);
            }

            string tx = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:o}", dateTime);
            textBoxTime.Text = tx;

            Bitmap bitmap = new Bitmap(panelTimeDisplay.Width, panelTimeDisplay.Height);
            panelTimeDisplay.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), panelTimeDisplay.Size));
            MemoryStream ms = new MemoryStream();
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 75L);// session.Quality);
            bitmap.Save(ms, GetJpegEncoder(), new EncoderParameters(1) { Param = new[] { qualityParam } });
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            ms.Close();
            return data;
        }

        public void SetFPS(int videoChannel, double fps)
        {
            if (videoChannel == DemoDeviceConstants.DeviceVideoChannel1)
            {
                _videoChannel1Fps = fps;
            }
            else if (videoChannel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                _videoChannel2Fps = fps;
            }
        }

        public void AddDeviceAction(string info)
        {
            Invoke(new AddActionLineDelegate(AddActionLine), info);
        }

        private void AddActionLine(string commandLine)
        {
            deviceLog.Items.Insert(0, commandLine);
            deviceLog.Refresh();
        }

        private void ButtonCloseClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// This method is called when a new session is about to be created.
        /// This method should validate the requested session parameters, and change if needed.
        /// If unable to handle more sessions for this channel, you can throw an Exception.
        /// </summary>
        /// <param name="session"></param>
        private void MediaProviderSessionOpening()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { MediaProviderSessionOpening(); }));
            }
            else
            {
                // Here we can check what the server side has configured, and adjust to what make sense for us.
                int maxFPS = 30;
                if (_session.FPS > maxFPS)
                {
                    _session.FPS = maxFPS;
                }

                textBoxSessionCount.Text = "1";
            }
        }

    
        /// <summary>
        /// This is called when one of the sessions are closed.
        /// In this sample we just update the active session counter for this channel.
        /// </summary>
        /// <param name="session"></param>
        private void MediaProviderSessionClosed()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate { MediaProviderSessionClosed(); }));
            }
            else
            {
                textBoxSessionCount.Text = "0";
            }
        }

        /// <summary>
        /// For this sample, we provide JPEG frames.
        /// </summary>
        /// <param name="textOverlay"></param>
        /// <returns></returns>
        private byte[] MediaProviderGetMediaData(int channel, int stream, string textOverlay)
        {
            if (InvokeRequired)
            {                
                // Get on the UI thread before executing the below code
                return Invoke(new MediaProviderGetMediaDataDelegate(MediaProviderGetMediaData), channel, stream, textOverlay) as byte[];
            }
            if (IsDisposed)
            {
                return null;
            }

            try
            {
                int width = panelTimeDisplay.Width;
                int height = panelTimeDisplay.Height;
                if (stream == 1 && !_stream1HighRes || stream == 0 && !_stream0HighRes)
                {
                    width = DemoDeviceConstants.LowResWidth;
                    height = DemoDeviceConstants.LowResHeight;
                }
                textBoxTime.Text = DateTime.Now.ToString("o");
                Bitmap bitmap = new Bitmap(width, height);
                panelTimeDisplay.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), new Size(width, height)));

                if (textOverlay != null)
                {
                    Graphics g = Graphics.FromImage(bitmap);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawString(textOverlay, new Font("Tahoma", 10), Brushes.Red, 50, 50);
                    g.Flush();
                }

                var ms = new MemoryStream();
                EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, 75L);
                bitmap.Save(ms, GetJpegEncoder(), new EncoderParameters(1) { Param = new[] { qualityParam } });
                ms.Seek(0, SeekOrigin.Begin);
                byte[] data = new byte[ms.Length];
                ms.Read(data, 0, (int)ms.Length);
                ms.Close();
                if (channel == DemoDeviceConstants.DeviceVideoChannel1)
                {
                    _videoChannel1LastFrame = DateTime.UtcNow;
                }
                else if (channel == DemoDeviceConstants.DeviceVideoChannel2)
                {
                    _videoChannel2LastFrame = DateTime.UtcNow;
                }
                return data;
            }
            catch (Exception ex)
            {
                if (IsDisposed)
                {
                    return null;
                }

                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get the .Net JPEG Encoder
        /// </summary>
        /// <returns></returns>
        private ImageCodecInfo GetJpegEncoder()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(s => s.FormatID == ImageFormat.Jpeg.Guid);
        }

        public void AddEvent(string eventId)
        {
            lock (_generatedEventsLock)
            {
                _generatedEvents.Add(eventId);
            }
        }

        double _pan = 0, _tilt = 0, _zoom = 0;
        public void SetAbsolutePosition(double pan, double tilt, double zoom)
        {
            _pan = pan;
            _tilt = tilt;
            _zoom = zoom;
            AddDeviceAction($"SetAbsolutePosition({pan}, {tilt}, {zoom})");
        }

        public double[] GetAbsolutePosition()
        {
            AddDeviceAction($"GetAbsolutePosition => ({_pan}, {_tilt}, {_zoom})");
            return new[] { _pan, _tilt, _zoom };
        }

        public Guid StartFirmwareUpgrade(string firmwareContent)
        {
            _firmwareUpgradeProgress = 0;
            StartFirmwareUpgradeProgress();
            AddDeviceAction("Firmware upgrade started with following content:" + Environment.NewLine + firmwareContent );            
            return Guid.NewGuid();
        }

        private void StartFirmwareUpgradeProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { StartFirmwareUpgradeProgress(); }));
                return;
            }
            _firmwareUpgradeProgressBar.Value = _firmwareUpgradeProgress;
            _firmwareUpgradeProgressBar.Visible = true;
            _firmwareUpgradeLabel.Visible = true;
            _firmwareUpgradeTimer.Start();
        }

        private void _firmwareUpgradeTimer_Tick(object sender, EventArgs e)
        {
            UpdateFirmwareProgress();
        }

        private void UpdateFirmwareProgress()
        {
            _firmwareUpgradeProgress += 5;
            _firmwareUpgradeProgressBar.Value = _firmwareUpgradeProgress;
            if (_firmwareUpgradeProgress == 100)
            {
                _firmwareUpgradeProgressBar.Visible = false;
                _firmwareUpgradeLabel.Visible = false;
                AddDeviceAction("Firmware upgrade completed");
                _firmwareUpgradeTimer.Stop();
            }
        }

        public int GetFirmwareUpgradeProgress(Guid upgradeSessionId)
        {
            return _firmwareUpgradeProgress;
        }

        private void buttonMotionDetectStart_Click(object sender, EventArgs e)
        {
             AddEvent(DemoDeviceConstants.DeviceEventMotionDetectionStart);
        }

        private void buttonMotionDetectStop_Click(object sender, EventArgs e)
        {
            AddEvent(DemoDeviceConstants.DeviceEventMotionDetectionStop);
        }

        private void buttonLprEvent_Click(object sender, EventArgs e)
        {
            AddEvent(DemoDeviceConstants.DeviceEventLPR + textBoxLPR.Text);
        }

        private void checkBoxInput1_CheckedChanged(object sender, EventArgs e)
        {
            SendInput1State();
        }

        private void checkBoxInput2_CheckedChanged(object sender, EventArgs e)
        {
            SendInput2State();
        }

        private void SendInput1State()
        {
            AddEvent(checkBoxInput1.Checked ? DemoDeviceConstants.DeviceEventActivateInput1 : DemoDeviceConstants.DeviceEventDeactivateInput1);
        }

        private void SendInput2State()
        {
            AddEvent(checkBoxInput2.Checked ? DemoDeviceConstants.DeviceEventActivateInput2 : DemoDeviceConstants.DeviceEventDeactivateInput2);
        }

        internal void SetResolution(int channel, int stream, string data)
        {
            if (stream == 0)
            {
                _stream0HighRes = data == "high";
            }
            else if (stream == 1)
            {
                _stream1HighRes = data == "high";
            }
        }
    }
}
