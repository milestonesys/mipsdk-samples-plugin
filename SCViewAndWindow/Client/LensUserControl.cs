using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
	public partial class LensUserControl : UserControl
	{
		public event EventHandler ClickEvent;

		public LensUserControl()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)	
			{
				Type msgType = typeof(LensCommandData);
				FieldInfo[] info = msgType.GetFields();
				foreach (FieldInfo type in info)
				{
					if (type.IsLiteral)
					{
						String name = type.ToString();
						name = name.Substring(name.LastIndexOf(" ") + 1);
						listBoxIndicator.Items.Add(name);
					}
				}
			}

		}

		private void OnClick(object sender, EventArgs e)
		{
			if (ClickEvent != null)
				ClickEvent(this, new EventArgs());
		}

		private void OnFireIndicator(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.Control.LensCommand, listBoxIndicator.SelectedItem),
				null,
				null);

		}

	}
}
