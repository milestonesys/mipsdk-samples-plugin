using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace VideoPreview.Admin
{
	public partial class VideoPreviewUserControl : UserControl
	{
		#region Fields

		internal event EventHandler ConfigurationChangedByUser;
		private Item _selectedCameraItem;
		private bool _lineMode;
		private Point _first, _next;
		private bool _lineDefined;

		private PlaybackUserControl _playbackUserControl;
		private BitmapSource _bitmapSource;
        private bool _inLiveMode = false;
		#endregion

		#region Initialization

		public VideoPreviewUserControl()
		{
			InitializeComponent();

			_playbackUserControl = ClientControl.Instance.GeneratePlaybackUserControl();
			_playbackUserControl.Dock = DockStyle.Top;
			_playbackUserControl.TimeSpan = TimeSpan.FromHours(1);
			_playbackUserControl.ShowTallUserControl = true;

			panelPlayback.Controls.Add(_playbackUserControl);

			FQID playbackControllerFQID = ClientControl.Instance.GeneratePlaybackController();
			_playbackUserControl.Init(playbackControllerFQID);

			_bitmapSource = new BitmapSource();
			_bitmapSource.PlaybackFQID = playbackControllerFQID;
			_bitmapSource.NewBitmapEvent += _bitmapSource_NewBitmapEvent;
			_bitmapSource.Selected = true;

            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.PlaybackSkipModeCommand, PlaybackSkipModeData.Skip), playbackControllerFQID);

            _playbackUserControl.Enabled = false;
		}

		void _bitmapSource_NewBitmapEvent(Bitmap bitmap)
		{
            if (pictureBox.Width != 0 && pictureBox.Height != 0)
            {
                Bitmap temp = new Bitmap(bitmap, pictureBox.Width, pictureBox.Height);
                if (_lineDefined)
                {
                    using (Graphics g = Graphics.FromImage(temp))
                    {
                        g.DrawLine(new Pen(Brushes.AntiqueWhite, 5.0F), _first, _next);
                    }
                }
                pictureBox.Image = temp;
            }
		    bitmap.Dispose();
		}

		#endregion

		#region ItemManager support methods

		internal void Close()
		{
			if (_bitmapSource != null)
			{
				CloseBitmap();
				_bitmapSource.NewBitmapEvent -= _bitmapSource_NewBitmapEvent;
				_bitmapSource = null;
			}

			if (_playbackUserControl != null)
			{
				_playbackUserControl.Close();
				_playbackUserControl = null;
			}
		}

		internal String DisplayName
		{
			get { return textBoxName.Text; }
			set { textBoxName.Text = value; }
		}

        private bool _ignoreChanges = false;
		internal void OnUserChange(object sender, EventArgs e)
		{
			if (!_ignoreChanges && ConfigurationChangedByUser != null)
				ConfigurationChangedByUser(this, new EventArgs());
		}

		private void InitBitmap()
		{
			if (_bitmapSource != null)
			{
				_bitmapSource.Init();
			}
		}

		private void CloseBitmap()
		{
			if (_bitmapSource != null)
			{
				_bitmapSource.Close();
			}			
		}

		internal void FillContent(Item item)
		{
		    _ignoreChanges = true;
			ClearContent();

			if (item==null)
				return;

			DisplayName = item.Name;
			if (item.Properties.ContainsKey("CameraId"))		// Get the FQID of previous selected camera
			{
                Guid streamId = Guid.Empty;
                if (item.Properties.ContainsKey("StreamId"))
                {
                    Guid.TryParse(item.Properties["StreamId"], out streamId);
                }

				_selectedCameraItem = Configuration.Instance.GetItem(new FQID(item.Properties["CameraId"]));
                if (_selectedCameraItem == null)
                {
                    // Device deleted 
                    item.Properties.Remove("CameraId");
                    item.Name += " (Camera deleted)";
                    _ignoreChanges = false;
                    return;
                }
                FillStreamSelections(streamId);

                textBoxCameraName.Text = "";
				if (_selectedCameraItem != null)
				{
					textBoxCameraName.Text = _selectedCameraItem.Name;
					if (_bitmapSource != null)
					{
						_bitmapSource.Item = _selectedCameraItem;
                        _bitmapSource.StreamId = streamId;
						InitBitmap();
					}
				}
				buttonStart.Enabled = true;
				buttonPreviewImage.Enabled = false;
			} else
			{
				buttonPreviewImage.Enabled = false;
				buttonStart.Enabled = false;	
			}
			buttonLine.Enabled = true;
			buttonPlayback.Enabled = false;
            _ignoreChanges = false;
        }

		internal void ClearContent()
		{
			if (_bitmapSource != null)
			{
				_bitmapSource.LiveStop();
				CloseBitmap();
			}
			ResetUserControl();
		}

		internal void ResetUserControl()
		{

			DisplayName = "";
			textBoxCameraName.Text = "";
			textBoxHeader.Text = "";
			buttonPreviewImage.Enabled = false;
			buttonLine.Enabled = false;
			buttonPlayback.Enabled = false;
			buttonStart.Enabled = false;
			pictureBox.Image = null;
		}

		internal void UpdateItem(Item item)
		{
			item.Name = DisplayName;
			if (_selectedCameraItem != null)
			{
				item.Properties["CameraId"] = _selectedCameraItem.FQID.ToXmlNode().OuterXml;
                item.Properties["StreamId"] = _bitmapSource.StreamId.ToString();
			}
		}

		#endregion

		#region Button Click Handling

		private void OnSelectCamera(object sender, EventArgs e)
		{
			_bitmapSource.Close();
			textBoxHeader.Text = "";

			ItemPickerForm form = new ItemPickerForm();
			form.CategoryFilter = Category.VideoIn;
			form.SelectedItem = _selectedCameraItem;
			form.AutoAccept = true;
			form.Init( Configuration.Instance.GetItemsByKind(Kind.Camera));
			if (form.ShowDialog()==DialogResult.OK)
			{
				_selectedCameraItem = form.SelectedItem;
				textBoxCameraName.Text = "";
				if (_selectedCameraItem!=null)
				{
					textBoxCameraName.Text = _selectedCameraItem.Name;
				}
				OnUserChange(this, null);		// Indicate that something changed, and should be saved.
				buttonStart.Enabled = true;
				buttonPreviewImage.Enabled = false;
				buttonLine.Enabled = true;
				buttonPlayback.Enabled = false;
				buttonLive.Enabled = false;

				_bitmapSource.Item = _selectedCameraItem;
                InitBitmap();
                _playbackUserControl.Enabled = false;

                FillStreamSelections(Guid.Empty);
			}
		}

        private void FillStreamSelections(Guid selectedId)
        {
            VideoOS.Platform.Data.StreamDataSource streams = new VideoOS.Platform.Data.StreamDataSource(_selectedCameraItem);

            comboBoxStream.Items.Clear();
            foreach (var stream in streams.GetTypes())
            {
                int ix = comboBoxStream.Items.Add(stream);
                if (stream.Id == selectedId || stream.Id == _selectedCameraItem.FQID.ObjectId)
                    comboBoxStream.SelectedIndex = ix;
            }
            comboBoxStream.Enabled = true;
        }

        private void OnPlayback(object sender, EventArgs e)
		{
            _inLiveMode = false;
            _bitmapSource.LiveStop();
			buttonPlayback.Enabled = false;
			buttonLive.Enabled = true;
			buttonLine.Enabled = true;
			
			_playbackUserControl.SetEnabled(true);
			
		}

		private void OnLive(object sender, EventArgs e)
		{
            if (comboBoxStream.SelectedItem != null)
            {
                VideoOS.Platform.Data.DataType dataType = comboBoxStream.SelectedItem as VideoOS.Platform.Data.DataType;
                if (dataType != null)
                {
                    _bitmapSource.StreamId = dataType.Id;
                }
            }
            _bitmapSource.LiveStart();
			buttonPlayback.Enabled = true;
			buttonLive.Enabled = false;
			buttonLine.Enabled = true;
			buttonPreviewImage.Enabled = true;
			_playbackUserControl.SetEnabled(false);
            _inLiveMode = true;
        }

        private void OnLine(object sender, EventArgs e)
		{
			_lineMode = true;
		}

		private void OnShowSingleImage(object sender, EventArgs e)
		{
			ImagePreviewForm form = new ImagePreviewForm();
			form.Image = pictureBox.Image;
			form.ShowDialog();
		}

		private void OnStart(object sender, EventArgs e)
		{
            textBoxHeader.Text = DisplayName;
            _playbackUserControl.SetCameras(new List<FQID>() { _selectedCameraItem.FQID});
			OnLive(this, null);
		}


		#endregion

		#region Event Handling

		void ImageUserControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (_lineMode)
			{
			    _lineDefined = false;
				_first = e.Location; 
			}
		}

        private void OnStreamChanged(object sender, EventArgs e)
        {
            if (_bitmapSource != null && comboBoxStream.SelectedItem is VideoOS.Platform.Data.DataType)
            {
                _bitmapSource.StreamId = (comboBoxStream.SelectedItem as VideoOS.Platform.Data.DataType).Id;
                if (_bitmapSource.IsInitialized && _inLiveMode)
                {
                    _bitmapSource.LiveStop();
                    _bitmapSource.LiveStart();
                }
            }
            OnUserChange(this, null);		// Indicate that something changed, and should be saved.
        }

        void ImageUserControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (_lineMode)
			{
				_next = e.Location;
				_lineDefined = true;
				using (Graphics g = Graphics.FromHwnd(pictureBox.Handle))
				{
					g.DrawLine(new Pen(Brushes.AntiqueWhite, 5.0F), _first, _next);
				}
			}
		}
		#endregion


	}
}
