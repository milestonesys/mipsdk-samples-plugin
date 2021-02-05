using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.UI;

namespace ServiceTest.Client
{
	public partial class ServiceTestViewItemUserControl : ViewItemUserControl
	{
		#region Component private class variables

		private bool _inLive = true;
		private ServiceTestViewItemManager _viewItemManager;
		private ResourceManager _stringResourceManager;
		private object _modeChangedReceiver;

		#endregion

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a ServiceTestViewItemUserControl instance
		/// </summary>
		public ServiceTestViewItemUserControl(ServiceTestViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();

			_stringResourceManager =
				new System.Resources.ResourceManager(Assembly.GetExecutingAssembly().GetName().Name + ".Resources.Strings",
													 Assembly.GetExecutingAssembly());

			SetUpApplicationEventListeners();

			InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;

			FillServiceList();

			ClientControl.Instance.RegisterUIControlForAutoTheming(this);	// The panelHeader is excluded (Tag == "DoNotThemeMe")

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
		}

		private void RemoveApplicationEventListeners()
		{
			//remove ViewItem event listeners
			_viewItemManager.PropertyChangedEvent -= new EventHandler(viewItemManager_PropertyChangedEvent);

			//remove ApplicationController events
			EnvironmentManager.Instance.UnRegisterReceiver(_modeChangedReceiver);
		}

		/// <summary>
		/// Is called when userControl is not displayed anymore. Either because of 
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

		#endregion

		private void ViewItemUserControl_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				FireClickEvent();
			}
			else if (e.Button == MouseButtons.Right)
			{
				FireRightClickEvent(e);
			}
		}

		private void ViewItemUserControl_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				FireDoubleClickEvent();
			}
		}

		private void OnDoubleClick(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
		}

		private void OnClick(object sender, EventArgs e)
		{
			FireClickEvent();
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

		#endregion

		private void OnRefresh(object sender, EventArgs e)
		{
			FillServiceList();
		}

		private void FillServiceList()
		{
			_listBoxServices.Items.Clear();
			try
			{
				List<Configuration.ServiceURIInfo> serviceUriInfo =
					Configuration.Instance.GetRegisteredServiceUriInfo(Configuration.Instance.ServerFQID.ServerId);
				foreach (Configuration.ServiceURIInfo si in serviceUriInfo)
				{
					_listBoxServices.Items.Add(si.Name + ", url=" + si.UriArray[0] + ", ServiceId=" + si.Type.ToString());
				}

				// I create the AlarmClientManager object... 
				VideoOS.Platform.Proxy.AlarmClient.AlarmClientManager acm = new VideoOS.Platform.Proxy.AlarmClient.AlarmClientManager();

				// Then some where inside my plugin I try to create the AlarmClient object
				VideoOS.Platform.Proxy.AlarmClient.IAlarmClient ac = acm.GetAlarmClient(EnvironmentManager.Instance.MasterSite.ServerId);
				var lines = ac.GetAlarmLines(0, 10, new AlarmFilter());
				foreach (var line in lines)
				{
					listBoxAlarm.Items.Add("Alarm:" + line.Name + ", source:" + line.SourceName);
				}
			}
			catch (Exception ex)
			{
				_listBoxServices.Items.Add("Server services unavailable:" + ex.Message);
			}
		}

	}
}
