using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace VideoPreview.Client
{
    public partial class VideoPreviewViewItemUserControl : ViewItemUserControl
    {
        #region Component private class variables

        private VideoPreviewViewItemManager _viewItemManager;
        private Item _selectedCameraItem = null;
        private double _absolutePosX = 0.0, _absolutePosY = 0.0;
        private bool _inPlacementMode = false;
        private object _themeChangedHandler;

        private ImageViewerControl _imageViewerControl;
        private AudioPlayerControl _audioPlayerControl;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a VideoPreviewViewItemUserControl instance
        /// </summary>
        public VideoPreviewViewItemUserControl(VideoPreviewViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();
        }

        public override void Init()
        {
            _imageViewerControl = ClientControl.Instance.GenerateImageViewerControl(WindowInformation);
            panel2.Controls.Add(_imageViewerControl);
            _imageViewerControl.Dock = DockStyle.Fill;

            _audioPlayerControl = ClientControl.Instance.GenerateAudioPlayerControl(WindowInformation);
            panel2.Controls.Add(_audioPlayerControl);

            SetUpApplicationEventListeners();

            _imageViewerControl.EnableMouseControlledPtz = true;
            _imageViewerControl.EnableMousePtzEmbeddedHandler = false;
            _imageViewerControl.EnableDigitalZoom = false;
            _imageViewerControl.EnableScrollWheel = true;

            panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
            panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
            ClientControl.Instance.RegisterUIControlForAutoTheming(panelMain);

            string fqidString = _viewItemManager.GetProperty(ClientControl.EmbeddedCameraFQIDProperty);
            if (fqidString != null)
            {
                _selectedCameraItem = Configuration.Instance.GetItem(new FQID(fqidString));
                FillCameraSelection();

                FillMicrophoneSelection();
            }
        }

        /// <summary>
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            RemoveApplicationEventListeners();
            if (_imageViewerControl.CameraFQID != null)
            {
                _imageViewerControl.Disconnect();
                _imageViewerControl.Close();
            }
            if (_audioPlayerControl.MicrophoneFQID != null)
            {
                _audioPlayerControl.Disconnect();
                _audioPlayerControl.Close();
            }
            _imageViewerControl.Dispose();
            _imageViewerControl = null;

            _audioPlayerControl.Dispose();
            _audioPlayerControl = null;
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _viewItemManager.PropertyChangedEvent += viewItemManager_PropertyChangedEvent;

            _imageViewerControl.ClickEvent += OnImageClick;
            //Note the usercontrol MouseClick does not work for ImageViewerControl - use above
            //this.imageViewerControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControl_MouseClick);
            _imageViewerControl.DoubleClickEvent += OnDoubleClick;

            _imageViewerControl.RecordedImageReceivedEvent += imageViewerControl_RecordedImageReceivedEvent;
            _imageViewerControl.ImageDisplayedEvent += imageViewerControl_ImageDisplayedEvent;

            _themeChangedHandler = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ThemeChangedIndicationHandler),
                                                         new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            _viewItemManager.PropertyChangedEvent -= viewItemManager_PropertyChangedEvent;

            _imageViewerControl.ClickEvent -= OnImageClick;
            _imageViewerControl.DoubleClickEvent -= OnDoubleClick;
            _imageViewerControl.RecordedImageReceivedEvent -= imageViewerControl_RecordedImageReceivedEvent;
            _imageViewerControl.ImageDisplayedEvent -= imageViewerControl_ImageDisplayedEvent;

            if (checkBox1.Checked)
            {
                _imageViewerControl.LiveStreamInformationEvent -= imageViewerControl_LiveStreamInformationEvent;
            }

            if (_themeChangedHandler != null)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedHandler);
                _themeChangedHandler = null;
            }
        }

        #endregion

        #region Component events

        void viewItemManager_PropertyChangedEvent(object sender, EventArgs e)
        {
            labelName.Text = _viewItemManager.SomeName;
        }

        void imageViewerControl_RecordedImageReceivedEvent(object sender, RecordedImageReceivedEventArgs e)
        {
            _imageViewerControl.EnableRecordedImageDisplayedEvent = false;      // Just to receive 1 time
        }

        void imageViewerControl_ImageDisplayedEvent(object sender, ImageDisplayedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() => { textBoxDisplayTime.Text = e.ImageTime.ToString("yyyy-MM-dd HH:mm:ss.fff"); }));
        }

        private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            Selected = _selected;
            return null;
        }

        private void OnImageClick(object sender, EventArgs e)
        {
            if (_inPlacementMode)
            {
                Point pos = panel2.PointToClient(Cursor.Position);
                _absolutePosX = pos.X / Convert.ToDouble(panel2.Width);
                _absolutePosY = pos.Y / Convert.ToDouble(panel2.Height);
                _inPlacementMode = false;
                Cursor = Cursors.Default;
            }
            OnClick(e);
        }

        private void OnBufferChange(object sender, ScrollEventArgs e)
        {
            _imageViewerControl.SetVideoQuality(hScrollBarSmoothBuffer.Value, 8);
        }

        private void OnLiveTickChanged(object sender, EventArgs e)
        {
            _imageViewerControl.EnableLiveStreamInformation = checkBox1.Checked;
            if (checkBox1.Checked)
            {
                _imageViewerControl.LiveStreamInformationEvent += imageViewerControl_LiveStreamInformationEvent;
            }
            else
            {
                _imageViewerControl.LiveStreamInformationEvent -= imageViewerControl_LiveStreamInformationEvent;
            }
            textBoxLineInfo.Text = "";
        }

        void imageViewerControl_LiveStreamInformationEvent(object sender, LiveStreamInformationEventArgs liveInformation)
        {
            textBoxLineInfo.Text = liveInformation.Information;
        }

        private void OnShowBitmap(object sender, EventArgs e)
        {
            Bitmap bitmap = _imageViewerControl.GetCurrentDisplayedImageAsBitmap();
            if (bitmap != null)
            {
                System.Windows.Forms.Clipboard.SetImage(bitmap);
            }
        }

        private void OnShowHeaderChanged(object sender, EventArgs e)
        {
            _imageViewerControl.EnableVisibleHeader = checkBoxHeader.Checked;
        }

        private void OnAspectChange(object sender, EventArgs e)
        {
            if (_imageViewerControl != null)
            {
                _imageViewerControl.MaintainImageAspectRatio = checkBoxAspectRatio.Checked;
            }
        }

        private void OnDigitalZoomChanged(object sender, EventArgs e)
        {
            if (_imageViewerControl != null)
            {
                _imageViewerControl.EnableDigitalZoom = checkBoxDigitalPtz.Checked;
                _imageViewerControl.EnableMousePtzEmbeddedHandler = checkBoxDigitalPtz.Checked;
            }
        }

        private void checkBoxLiveIndicator_CheckedChanged(object sender, EventArgs e)
        {
            _imageViewerControl.EnableVisibleLiveIndicator = checkBoxLiveIndicator.Checked;
        }

        private void comboBoxStreams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxStreams.DataSource != null)
            {

                if (_imageViewerControl != null)
                {
                    if (_imageViewerControl.CameraFQID != null)
                    {
                        _imageViewerControl.Disconnect();
                        DataType selectStream;
                        selectStream = (DataType)comboBoxStreams.SelectedItem;
                        if (selectStream.Properties["Default"] == "No")
                        {
                            _imageViewerControl.StreamId = selectStream.Id;
                        }
                        else
                        {
                            _imageViewerControl.StreamId = Guid.Empty;
                        }
                        _imageViewerControl.Connect();
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (_imageViewerControl != null)
            {
                _imageViewerControl.PtzCenter(100, 100, 33, 33, 75);
            }
        }

        private void OnMakeSquare(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(100, 100);

            Graphics g = Graphics.FromHwnd(_imageViewerControl.Handle);
            g.DrawRectangle(new Pen(Brushes.AntiqueWhite, 5.0F), 20, 20, 60, 30);

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0, 255));
                }
            }
            for (int x = 50; x < 99; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
            }
            for (int x = 0; x < 99; x++)
            {
                bitmap.SetPixel(x, 0, Color.FromArgb(255, 255, 0, 0));
                bitmap.SetPixel(x, 99, Color.FromArgb(255, 255, 0, 0));
            }
            for (int y = 0; y < 99; y++)
            {
                bitmap.SetPixel(00, y, Color.FromArgb(255, 255, 0, 0));
                bitmap.SetPixel(99, y, Color.FromArgb(255, 255, 0, 0));
            }

            try
            {
                _imageViewerControl.ClearOverlay(20);
                if (GetVerticalDock() == DockStyle.None || GetHorizontalDock() == DockStyle.None)
                {
                    checkBoxScaleOverlay.Checked = false;
                }

                _imageViewerControl.SetOverlay(bitmap, 20, checkBoxOverlayKeepAspect.Checked, false, checkBoxScaleOverlay.Checked,
                                              GetScaleFactor(), GetVerticalDock(), GetHorizontalDock(), _absolutePosX, _absolutePosY);
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("SetOverlay", ex);
            }

            g.Dispose();
            bitmap.Dispose();
        }

        private void OnClear(object sender, EventArgs e)
        {
            _imageViewerControl.ClearOverlay(20);
        }

        private void OnSelectPosition(object sender, EventArgs e)
        {
            _inPlacementMode = !_inPlacementMode;

            Cursor = _inPlacementMode ? Cursors.Cross : Cursors.Default;
        }

        private void OnSelectCamera(object sender, EventArgs e)
        {
            _imageViewerControl.Disconnect();

            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.Camera;
            form.SelectedItem = _selectedCameraItem;
            form.AutoAccept = true;
            form.Init();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _selectedCameraItem = form.SelectedItem;
                FillCameraSelection();
                FillMicrophoneSelection();
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

        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FireClickEvent();
            }
        }

        private void OnDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FireDoubleClickEvent();
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            FireClickEvent();
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            FireDoubleClickEvent();
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
                if (_hidden != value)
                {
                    base.Hidden = value;
                }
            }
        }

        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                _imageViewerControl.Selected = value;
                if (value)
                {
                    panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderColor;
                    panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderTextColor;
                }
                else
                {
                    panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
                    panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
                }
            }
        }

        public override bool ShowToolbar
        {
            get { return false; }
        }

        #endregion

        private void FillCameraSelection()
        {
            button1.Text = "";
            if (_selectedCameraItem != null)
            {
                button1.Text = _selectedCameraItem.Name;
                _imageViewerControl.CameraFQID = _selectedCameraItem.FQID;
                OnLiveTickChanged(this, null);
                _imageViewerControl.EnableVisibleLiveIndicator = checkBoxLiveIndicator.Checked;
                _imageViewerControl.EnableRecordedImageDisplayedEvent = true;
                _imageViewerControl.EnableMouseControlledPtz = true;
                _imageViewerControl.EnableMousePtzEmbeddedHandler = false;
                _imageViewerControl.EnableDigitalZoom = false;
                _imageViewerControl.Initialize();
                _imageViewerControl.Connect();

                comboBoxStreams.DataSource = null;
                comboBoxStreams.Items.Clear();
                var streamDataSource = new StreamDataSource(_selectedCameraItem);
                IList<DataType> streams = streamDataSource.GetTypes();
                if (streams.Count > 1)
                {
                    comboBoxStreams.DataSource = streams;
                    comboBoxStreams.DisplayMember = "Name";
                }
                foreach (DataType stream in comboBoxStreams.Items)
                {
                    if (stream.Properties.ContainsKey("Default"))
                    {
                        if (stream.Properties["Default"] == "Yes")
                        {
                            comboBoxStreams.SelectedItem = stream;
                        }
                    }
                }
                comboBoxStreams.Enabled = comboBoxStreams.Items.Count > 0;
            }
        }

        private void FillMicrophoneSelection()
        {
            if (_audioPlayerControl.MicrophoneFQID != null)
            {
                _audioPlayerControl.Disconnect();
            }

            if (_selectedCameraItem == null)
            {
                return;
            }

            Item mic = null;
            foreach (Item item in _selectedCameraItem.GetRelated())
            {
                if (item.FQID.Kind == Kind.Microphone)
                {
                    mic = item;
                }
            }

            if (mic != null)
            {
                _audioPlayerControl.MicrophoneFQID = mic.FQID;
                _audioPlayerControl.Initialize();
                _audioPlayerControl.Connect();
            }

        }

        private DockStyle GetVerticalDock()
        {
            DockStyle result;
            switch ((String)comboBoxVertical.SelectedItem)
            {
                case "Top":
                    result = DockStyle.Top;
                    break;
                case "Bottom":
                    result = DockStyle.Bottom;
                    break;
                default:
                    result = DockStyle.None;
                    break;
            }
            return result;
        }

        private DockStyle GetHorizontalDock()
        {
            DockStyle result;
            switch ((String)comboBoxHorizontal.SelectedItem)
            {
                case "Left":
                    result = DockStyle.Left;
                    break;
                case "Right":
                    result = DockStyle.Right;
                    break;
                default:
                    result = DockStyle.None;
                    break;
            }
            return result;
        }

        private double GetScaleFactor()
        {            
            return (double)numericUpDownScaleFactor.Value;
        }
    }
}
