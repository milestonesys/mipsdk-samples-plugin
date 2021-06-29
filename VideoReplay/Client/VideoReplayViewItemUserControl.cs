using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;

namespace VideoReplay.Client
{
    public partial class VideoReplayViewItemUserControl : ViewItemUserControl
	{
        private const int VideoIntervalSeconds = 15;
        private const int ReplaySpeedFactor = 5;

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
		public VideoReplayViewItemUserControl(VideoReplayViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();

			panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
			panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
			ClientControl.Instance.RegisterUIControlForAutoTheming(panelMain);
		}

		private void SetUpApplicationEventListeners()
		{
			//set up ViewItem event listeners
			_viewItemManager.PropertyChangedEvent += new EventHandler(viewItemManager_PropertyChangedEvent);

			//set up ApplicationController events
			_selectedCameraChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(SelectedCameraChangedHandler),
														 new MessageIdFilter(MessageId.SmartClient.SelectedCameraChangedIndication));
			Image imageClear = new Bitmap(320, 240);
			Graphics g = Graphics.FromImage(imageClear);
			g.FillRectangle(Brushes.Black, 0, 0, 320, 240);
			g.Dispose();
			Invoke(new SetImageDelegate(SetImage), new[] { imageClear });
		}

		private void RemoveApplicationEventListeners()
		{
			//remove ViewItem event listeners
			_viewItemManager.PropertyChangedEvent -= new EventHandler(viewItemManager_PropertyChangedEvent);

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

		private void OnClick(object sender, EventArgs e)
		{
			FireClickEvent();
		}

		private void OnDoubleClick(object sender, EventArgs e)
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

		void viewItemManager_PropertyChangedEvent(object sender, EventArgs e)
		{
			labelName.Text = _viewItemManager.SomeName;
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
				if (_hidden != value)
				{
					base.Hidden = value;
				}
			}
		}

		#endregion

		#region Component event handlers


		private object SelectedCameraChangedHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
		{
			_selectedItem = Configuration.Instance.GetItem(message.RelatedFQID);
			if (_selectedItem!=null)
			{
				label1.Text = "Camera: " + _selectedItem.Name;
				_previousSelectedName = _selectedItem.Name;
			} 
			_stop = true;
			return null;
		}

		#endregion

		private void OnReplay(object sender, EventArgs e)
		{
			if (_selectedItem != null)
			{
				_stop = true;
				Thread thread = new Thread(new ThreadStart(ShowReplay));
				thread.Start();
			}
		}

		private void ShowReplay()
		{
			Image imageClear = new Bitmap(320, 240);
			Graphics g = Graphics.FromImage(imageClear);
			g.FillRectangle(Brushes.Black, 0, 0, 320, 240);
			g.Dispose();
			Invoke(new SetImageDelegate(SetImage), new[] { imageClear });

			JPEGVideoSource source = new JPEGVideoSource(_selectedItem);
		    source.Width = pictureBox2.Width;
		    source.Height = pictureBox2.Height;
		    source.SetKeepAspectRatio(true, true);
            source.Init();
            var interval = TimeSpan.FromSeconds(VideoIntervalSeconds);
			List<object> resultList = source.Get(DateTime.Now - interval, interval, 150);

			_stop = true;
			if (resultList != null)
			{
				Invoke((MethodInvoker)delegate() { label2.Text = "Number of frames: " + resultList.Count; });
				if (resultList.Count > 0)
				{
					_stop = false;
				}
			}

			while (!_stop)
			{
				int avgInterval = 1000 * VideoIntervalSeconds / (ReplaySpeedFactor * resultList.Count);
				foreach (JPEGData jpeg in resultList)
				{
					MemoryStream ms = new MemoryStream(jpeg.Bytes);
					Image image = new Bitmap(ms);
					Invoke(new SetImageDelegate(SetImage), new[] {image});
					ms.Close();
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

		private delegate void SetImageDelegate(Bitmap bitmap);
		private void SetImage(Bitmap bitmap)
		{
			if (pictureBox2.Visible && pictureBox2.Height > 0)
			{
				double ratio = Convert.ToDouble(pictureBox2.Width)/pictureBox2.Height;
				double ratioNew = Convert.ToDouble(bitmap.Width)/bitmap.Height;

				double w = pictureBox2.Width;
				double h = pictureBox2.Height;
				if (ratio > ratioNew)
				{
					w = h*ratioNew;
				}
				else
				{
					h = w/ratioNew;
				}

				pictureBox2.Image = new Bitmap(bitmap, new Size(Convert.ToInt32(w), Convert.ToInt32(h)));
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			_stop = true;
		}

	
	}
}
