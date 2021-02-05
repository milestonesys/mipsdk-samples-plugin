using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace SequenceViewer.Client
{
	public partial class SequenceViewerViewItemUserControl : ViewItemUserControl
	{
		#region Component private class variables

		private bool _inLive = true;
		private SequenceViewerViewItemManager _viewItemManager;
		private ResourceManager _stringResourceManager;
		private object _modeChangedReceiver;
		private object _themeChangedReceiver;

		private Item _item = null;

		#endregion

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a DataSourceViewItemUserControl instance
		/// </summary>
		public SequenceViewerViewItemUserControl(SequenceViewerViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();

			_stringResourceManager =
				new System.Resources.ResourceManager(Assembly.GetExecutingAssembly().GetName().Name + ".Resources.Strings",
													 Assembly.GetExecutingAssembly());

			SetUpApplicationEventListeners();

			InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;
			ClientControl.Instance.RegisterUIControlForAutoTheming(panelMain);

			panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
			panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
        }


		private void SetUpApplicationEventListeners()
		{
			//set up ViewItem event listeners
			_viewItemManager.PropertyChangedEvent += new EventHandler(viewItemManager_PropertyChangedEvent);

			//set up ApplicationController events
			_modeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ApplicationModeChangedMessage),
														 new MessageIdFilter(MessageId.System.ModeChangedIndication));

			_themeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ThemeChangedIndicationHandler),
														 new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));
		
		}

		private void RemoveApplicationEventListeners()
		{
			//remove ViewItem event listeners
			_viewItemManager.PropertyChangedEvent -= new EventHandler(viewItemManager_PropertyChangedEvent);

			//remove ApplicationController events
			EnvironmentManager.Instance.UnRegisterReceiver(_modeChangedReceiver);

			EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedReceiver);
			_themeChangedReceiver = null;

		}

		/// <summary>
		/// Is called when userControl is not displayed anymore. Aither becase of 
		/// user clicking on another View or Item has been removed from View.
		/// </summary>
		public override void Close()
		{
			RemoveApplicationEventListeners();
		}

		/// <summary>
		/// Do not show the sliding toolbar!
		/// </summary>
		public override bool ShowToolbar
		{
			get { return false; }
		}

		public override void Print()
		{
			String extra = "Some extra lines of information\r\n" +
			               "Selected camera:" + (_item != null ? _item.Name : "Unselected") + "\r\n+" +
			               "00000\r\n11111\r\n22222\r\n33333\r\n44444\r\n55555\r\n66666\r\n77777\r\n88888\r\n99999" +
			               "00000\r\n11111\r\n22222\r\n33333\r\n44444\r\n55555\r\n66666\r\n77777\r\n88888\r\n99999" +
			               "00000\r\n11111\r\n22222\r\n33333\r\n44444\r\n55555\r\n66666\r\n77777\r\n88888\r\n99999";
			Print("Data Source Sample", extra);
		}
		#endregion

		private void OnClick(object sender, EventArgs e)
		{
			FireClickEvent();
		}

		private void OnDoubleClick(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
		}


		#region Component events
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
		/// Overrides property (set). First the Base implementaion is called.
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
		/// Overrides property (set). First the Base implementaion is called. 
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

		/// <summary>
		/// Ensure we theme the header
		/// </summary>
		public override bool Selected
		{
			get
			{
				return base.Selected;
			}
			set
			{
				base.Selected = value;
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

		#endregion



		#region Component event handlers


		private object ApplicationModeChangedMessage(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
		{
			switch (EnvironmentManager.Instance.Mode)
			{
				case Mode.ClientLive:
					InLive = true;
					break;
				case Mode.ClientPlayback:
					InLive = false;
					break;
				case Mode.ClientSetup:
					InLive = false;
					break;
			}
			return null;
		}

		private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
		{
			this.Selected = _selected;
			return null;
		}

		#endregion

		private void OnSelectCamera(object sender, EventArgs e)
		{
			ItemPickerForm form = new ItemPickerForm();
			form.KindFilter = Kind.Camera;
			form.AutoAccept = true;
			form.Init();
			if (form.ShowDialog()==DialogResult.OK)
			{
				_item = form.SelectedItem;
				buttonSelectCamera.Text = _item.Name;
				buttonShowSeqAsync.Enabled = true;
				buttonShowSeq.Enabled = true;
				buttonShowMD.Enabled = true;
				buttonShowMDAsync.Enabled = true;
				if (_item.FQID.ServerId.ServerType != ServerId.EnterpriseServerType)
				{
					buttonShowTypes.Enabled = true;		// Not supported by XPE
				}
			}
		}

		private void OnShowSeq(object sender, EventArgs e)
		{
			if (_item != null)
			{
				SequenceDataSource dataSource = new SequenceDataSource(_item);
				List<object> list = dataSource.GetData(DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0);
				listBox1.Items.Clear();
				if (list!=null)
				{
					foreach (SequenceData sd in list)
					{
                        listBox1.Items.Add(sd.EventHeader.Class + " " + sd.EventHeader.Name + "  " + 
                            sd.EventHeader.Timestamp.ToLocalTime().ToShortTimeString());
					}
				}
			}
		}

		private void OnRefreshSeqAsync(object sender, EventArgs e)
		{
			if (_item != null)
			{
				SequenceDataSource dataSource = new SequenceDataSource(_item);
				dataSource.GetDataAsync(listBox1, DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0, AsyncSeqHandler);
			}
		}

		private void OnRefreshMD(object sender, EventArgs e)
		{
				SequenceDataSource dataSource = new SequenceDataSource(_item);
				List<object> list = dataSource.GetData(DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0, VideoOS.Platform.Data.DataType.SequenceTypeGuids.MotionSequence);
				listBox1.Items.Clear();
				if (list!=null)
				{
					foreach (SequenceData sd in list)
					{
						listBox1.Items.Add(sd.EventHeader.Class +" "+ sd.EventHeader.Name + "  " + 
                            sd.EventHeader.Timestamp.ToLocalTime().ToShortTimeString());
					}
				}
			
		}

		private void OnRefreshMDAsync(object sender, EventArgs e)
		{
			if (_item != null)
			{
				SequenceDataSource dataSource = new SequenceDataSource(_item);
				dataSource.GetDataAsync(listBox1, DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0, VideoOS.Platform.Data.DataType.SequenceTypeGuids.MotionSequence, AsyncSeqHandler);
			}

		}

		private void AsyncSeqHandler(object result, object asyncState)
		{
		    Invoke(new MethodInvoker(() =>
		    {
		        listBox1.Items.Clear();
		        if (result != null && result is SequenceData[])
		        {
		            foreach (SequenceData sd in (SequenceData[]) result)
		            {
                        listBox1.Items.Add(sd.EventHeader.Class + " " + sd.EventHeader.Name + "  " +
                                           sd.EventHeader.Timestamp.ToLocalTime().ToShortTimeString());
		            }
		        }
		    }));
		}


		private void OnGetSeqType(object sender, EventArgs e)
		{
			if (_item != null)
			{
				SequenceDataSource dataSource = new SequenceDataSource(_item);
				List<DataType> list = dataSource.GetTypes();
				listBox1.Items.Clear();
				if (list != null)
				{
					foreach (DataType dt in list)
					{
						listBox1.Items.Add(dt.Name + "  "+ dt.Id.ToString());
					}
				}
			}
		}



	}
}
