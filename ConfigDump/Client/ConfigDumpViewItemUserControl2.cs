using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

namespace ConfigDump.Client
{
	public class ConfigDumpViewItemUserControl2 : ViewItemUserControl
	{
		private Admin.DumpConfigurationUserControl dumpConfigurationUserControl1;
		private ConfigDumpViewItemManager2 _viewItemManager;
		private Panel panelHeader;
		private Label label1;
		private Label labelName;
		private ItemPickerUserControl itemPickerUserControl;

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a ConfigDumpViewItemUserControl instance
		/// </summary>
		public ConfigDumpViewItemUserControl2(ConfigDumpViewItemManager2 viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();

			SetUpApplicationEventListeners();

			itemPickerUserControl = new ItemPickerUserControl();
			itemPickerUserControl.Init();
			itemPickerUserControl.Dock = DockStyle.Fill;

			dumpConfigurationUserControl1.FillContent();

            ClientControl.Instance.RegisterUIControlForAutoTheming(dumpConfigurationUserControl1);
            ClientControl.Instance.RegisterUIControlForAutoTheming(panelHeader);
            dumpConfigurationUserControl1.ShowHardware = false;
            //panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
            //panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
        }

		private void InitializeComponent()
		{
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.dumpConfigurationUserControl1 = new ConfigDump.Admin.DumpConfigurationUserControl();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.Goldenrod;
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.labelName);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(680, 18);
            this.panelHeader.TabIndex = 4;
            this.panelHeader.Click += new System.EventHandler(this.OnClick);
            this.panelHeader.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Configuration dump";
            this.label1.Click += new System.EventHandler(this.OnClick);
            this.label1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelName.Location = new System.Drawing.Point(123, 3);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(0, 13);
            this.labelName.TabIndex = 2;
            // 
            // dumpConfigurationUserControl1
            // 
            this.dumpConfigurationUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dumpConfigurationUserControl1.Location = new System.Drawing.Point(0, 21);
            this.dumpConfigurationUserControl1.Name = "dumpConfigurationUserControl1";
            this.dumpConfigurationUserControl1.Size = new System.Drawing.Size(680, 409);
            this.dumpConfigurationUserControl1.TabIndex = 0;
            this.dumpConfigurationUserControl1.Click += new System.EventHandler(this.OnClick);
            this.dumpConfigurationUserControl1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // ConfigDumpViewItemUserControl2
            // 
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.dumpConfigurationUserControl1);
            this.Name = "ConfigDumpViewItemUserControl2";
            this.Size = new System.Drawing.Size(680, 430);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

		}

		private void SetUpApplicationEventListeners()
		{
			//set up ViewItem event listeners
			_viewItemManager.PropertyChangedEvent += new EventHandler(viewItemManager_PropertyChangedEvent);
		}

		private void RemoveApplicationEventListeners()
		{
			//remove ViewItem event listeners
			_viewItemManager.PropertyChangedEvent -= new EventHandler(viewItemManager_PropertyChangedEvent);
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
			//labelName.Text = _viewItemManager.SomeName;
		}

		/*
		private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			listBox1.Items.Clear();
			Item item = e.Node.Tag as Item;
			if (item != null)
			{
				Item item2 = Configuration.Instance.GetItem(item.FQID);
				if (item2 != null && item2.Properties != null)
				{
					foreach (String p in item2.Properties.Keys)
					{
						listBox1.Items.Add(p + " = " + item2.Properties[p]);
					}
				}
				if (item2 != null)
				{
					listBox1.Items.Add("ServerType = " + item2.FQID.ServerId.ServerType);
				}
			}
		}
		*/
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

		#endregion



	}
}
