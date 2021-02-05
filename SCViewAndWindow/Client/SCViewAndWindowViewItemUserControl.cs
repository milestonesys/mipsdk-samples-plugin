using System;
using System.Collections.Generic;		//Add to Template
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;				//Add to Template

namespace SCViewAndWindow.Client
{
	public partial class SCViewAndWindowViewItemUserControl : ViewItemUserControl
	{
		#region Component private class variables

		private bool _inLive = true;
		private SCViewAndWindowViewItemManager _viewItemManager;
		private object _modeChangedReceiver;

		#endregion

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a SCViewAndWindowViewItemUserControl instance
		/// </summary>
		public SCViewAndWindowViewItemUserControl(SCViewAndWindowViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();

			ClientControl.Instance.RegisterUIControlForAutoTheming(tabControl);

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

		private void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)
			{
				viewCommandUserControl1.ClickEvent += new EventHandler(OnClick);
				playbackUControl1.ClickEvent += new EventHandler(OnClick);
				lensUserControl1.ClickEvent += new EventHandler(OnClick);
			}
		}

		public override void Init()
		{
			SetUpApplicationEventListeners();

			InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;			
		}

		/// <summary>
		/// Is called when userControl is not displayed anymore. Either because of 
		/// user clicking on another View or Item has been removed from View.
		/// </summary>
		public override void Close()
		{
			RemoveApplicationEventListeners();
		}


		#endregion



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
		/// Do not show the toolbar - it overwrites the tab control.
		/// </summary>
		public override bool ShowToolbar
		{
			get
			{
				return false;
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

	}

	internal class TaggedItem
	{
		public Item Item { get; set; }
		public TaggedItem(Item item)
		{
			Item = item;
		}

		public override string ToString()
		{
			return Item.Name;
		}
	}
}
