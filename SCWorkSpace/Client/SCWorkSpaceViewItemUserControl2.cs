using System;
using System.Windows.Forms;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;
using VideoOS.Platform.Messaging;

namespace SCWorkSpace.Client
{
    public partial class SCWorkSpaceViewItemUserControl2 : ViewItemUserControl
    {
        private List<object> _messageRegistrationObjects = new List<object>();
        private ImageViewerControl imageViewerControl;
        private AudioPlayerControl _audioPlayerControl;
        private AudioPlayerControl _audioPlayerControlSpeaker;
        private Item _selectedCameraItem;
        private FQID _playbackFQID;
        private PlaybackUserControl _playbackUserControl;

        public SCWorkSpaceViewItemUserControl2()
        {
            InitializeComponent();

			ClientControl.Instance.RegisterUIControlForAutoTheming(this);
        }

        public override void Init()
        {
            base.Init();

            this.imageViewerControl = ClientControl.Instance.GenerateImageViewerControl(WindowInformation);
            this.panel2.Controls.Add(imageViewerControl);
            this.imageViewerControl.Dock = DockStyle.Fill;

            _playbackUserControl = ClientControl.Instance.GeneratePlaybackUserControl(this.WindowInformation);
            _playbackUserControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(_playbackUserControl);

            _playbackFQID = ClientControl.Instance.GeneratePlaybackController();

            //this.imageViewerControl.ClickEvent += new System.EventHandler(this.OnImageClick);
            //Note the usercontrol MouseClick does not work for ImageViewerControl - use above
            //this.imageViewerControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControl_MouseClick);
            //this.imageViewerControl.DoubleClickEvent += imageViewerControl_DoubleClickEvent;

            _audioPlayerControl = ClientControl.Instance.GenerateAudioPlayerControl(WindowInformation);
            panel2.Controls.Add(_audioPlayerControl);
            _audioPlayerControlSpeaker = ClientControl.Instance.GenerateAudioPlayerControl(WindowInformation);
            panel2.Controls.Add(_audioPlayerControlSpeaker);

            _playbackUserControl.Init(_playbackFQID);
            imageViewerControl.PlaybackControllerFQID = _playbackFQID;

            _audioPlayerControl.PlaybackControllerFQID = _playbackFQID;
            _audioPlayerControlSpeaker.PlaybackControllerFQID = _playbackFQID;
            /*
            string fqidString = _viewItemManager.GetProperty(ClientControl.EmbeddedCameraFQIDProperty);
            if (fqidString != null)
            {
                _selectedCameraItem = Configuration.Instance.GetItem(new FQID(fqidString));
                FillCameraSelection();

                FillMicrophoneSelection();
            }*/
        }

        public override void Close()
        {
            base.Close();

            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();

        }

		private void ViewItemUserControlDoubleClick(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
		}

		/// <summary>
		/// Do not show the sliding toolbar!
		/// </summary>
		public override bool ShowToolbar
		{
			get { return false; }
		}


        private void SCWorkSpaceViewItemUserControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                FireClickEvent();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            imageViewerControl.Disconnect();

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
                /*
                if (_selectedCameraItem != null)
                {
                    _viewItemManager.SetProperty(ClientControl.EmbeddedCameraFQIDProperty, _selectedCameraItem.FQID.ToXmlNode().OuterXml);
                }
                else
                {
                    _viewItemManager.SetProperty(ClientControl.EmbeddedCameraFQIDProperty, string.Empty);
                }
                _viewItemManager.SaveAllProperties();
                */
            }


        }
        private void FillCameraSelection()
        {
            button1.Text = "";
            if (_selectedCameraItem != null)
            {
                button1.Text = _selectedCameraItem.Name;
                imageViewerControl.CameraFQID = _selectedCameraItem.FQID;
                //OnLiveTickChanged(this, null);
                //imageViewerControl.EnableVisibleLiveIndicator = checkBoxLiveIndicator.Checked;
                imageViewerControl.EnableRecordedImageDisplayedEvent = true;
                imageViewerControl.EnableMouseControlledPtz = true;
                imageViewerControl.EnableMousePtzEmbeddedHandler = false;
                imageViewerControl.EnableDigitalZoom = false;
                imageViewerControl.Initialize();
                imageViewerControl.Connect();
                imageViewerControl.Selected = true;

                _playbackUserControl.SetCameras(new List<FQID>() { _selectedCameraItem.FQID });

            }
        }

        private void FillMicrophoneSelection()
        {
            if (_audioPlayerControl.MicrophoneFQID != null)
                _audioPlayerControl.Disconnect();
            if (_audioPlayerControlSpeaker.MicrophoneFQID != null)
                _audioPlayerControlSpeaker.Disconnect();

            if (_selectedCameraItem == null)
                return;

            foreach (Item item in _selectedCameraItem.GetRelated())
            {
                if (item.FQID.Kind == Kind.Microphone)
                {
                    _audioPlayerControl.MicrophoneFQID = item.FQID;
                    _audioPlayerControl.Initialize();
                    _audioPlayerControl.Connect();
                }
                if (item.FQID.Kind == Kind.Speaker)
                {
                    _audioPlayerControlSpeaker.MicrophoneFQID = item.FQID;
                    _audioPlayerControlSpeaker.Initialize();
                    _audioPlayerControlSpeaker.Connect();
                }
            }
        }

        private void checkBoxLive_CheckedChanged(object sender, EventArgs e)
        {
            FQID newPlayback = null;
            if (!checkBoxLive.Checked)
            {
                newPlayback = _playbackFQID;
            }
            imageViewerControl.PlaybackControllerFQID = newPlayback;
            _audioPlayerControl.PlaybackControllerFQID = newPlayback;
            _audioPlayerControlSpeaker.PlaybackControllerFQID = newPlayback;

            panel1.Visible = !checkBoxLive.Checked;
        }
    }
}
